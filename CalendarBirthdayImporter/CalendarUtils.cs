using CalendarBirthdayImporter.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace CalendarBirthdayImporter
{
    public static class CalendarUtils
    {
        public static UserCredential Login()
        {
            return GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = "686578978954-mch2q002h8c8oiin4ji234dcbl6mdf5v.apps.googleusercontent.com",
                    ClientSecret = "3HjvS7VVD4uYUzBCA6I6YOrp"
                },
                new[] { CalendarService.Scope.Calendar },
                "user",
                CancellationToken.None).Result;
        }

        public static CalendarService CreateService(string appName, UserCredential credential)
        {
            var service = new CalendarService(new BaseClientService.Initializer
            {
                ApplicationName = appName,
                HttpClientInitializer = credential
            });

            return service;
        }

        public static bool CalendarExists(this CalendarService service, string calendarName)
        {
            var calendarList = service.CalendarList.List().Execute();

            return calendarList.Items.Select(c => c.Summary).Contains(calendarName);
        }

        public static string CreateCalendar(this CalendarService service, string calendarName)
        {
            var newCalendar = new Calendar
            {
                Summary = calendarName
            };

            return service.Calendars.Insert(newCalendar).Execute().Id;
        }

        public static Event CreateBirthdayEvent(this CalendarService service, string calendarId, Employee employee)
        {
            var recurence = new List<string>(new[] { "RRULE:FREQ=YEARLY" });
            var eventName = employee.Name + "'s birthday";
            var formattedDay = employee.GetBirthday();
            var eventDay = new EventDateTime { Date = formattedDay };
            var birthdayEvent = new Event
            {
                Summary = eventName,
                Start = eventDay,
                End = eventDay,
                Recurrence = recurence
            };

            return service.Events.Insert(birthdayEvent, calendarId).Execute();
        }

        public static bool CreateBirthdayEventsInCalendar(this CalendarService service, string calendarName, IEnumerable<Employee> employees)
        {
            if (!service.CalendarExists(calendarName))
            {
                var calendarId = service.CreateCalendar(calendarName);

                foreach (var employee in employees)
                {
                    service.CreateBirthdayEvent(calendarId, employee);
                }

                return true;
            }

            throw new ArgumentException("Calendar '" + calendarName + "' already exists!");
        }
    }
}