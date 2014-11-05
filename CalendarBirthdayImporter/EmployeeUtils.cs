using CalendarBirthdayImporter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace CalendarBirthdayImporter
{
    public static class EmployeeUtils
    {
        public static IEnumerable<Employee> ParseEmployeesFromFile(Stream stream)
        {
            var employees = new List<Employee>();
            var rawEmployees = new List<string>();
            var contents = new StreamReader(stream);
            using (contents)
            {
                while (!contents.EndOfStream)
                {
                    var line = contents.ReadLine();
                    if (!String.IsNullOrWhiteSpace(line))
                    {
                        rawEmployees.Add(line);
                    }
                }
            }
            
            foreach (var rawContact in rawEmployees)
            {
                var contactParts = rawContact.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (contactParts.Length < 2)
                {
                    throw new ArgumentException("Input file is not formated properly");
                }

                var contact = new Employee
                {
                    Name = contactParts[0],
                    UCN = contactParts[1]
                };

                employees.Add(contact);
            }

            return employees;
        }

        public static string GetBirthday(this Employee employee)
        {
            // TODO: Doesn't work if employee was born after 2000
            var ucnParts = employee.UCN.ToCharArray();

            if (ucnParts.Length == 10)
            {
                var month = ucnParts[2] + "" + ucnParts[3];
                var day = ucnParts[4] + "" + ucnParts[5];

                return "2014-" + month + "-" + day;
            }

            throw new ArgumentException("UCN is not valid");
        }
    }
}