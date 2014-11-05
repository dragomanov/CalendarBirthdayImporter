using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Calendar = Google.Apis.Calendar.v3.Data.Calendar;
using Event = Google.Apis.Calendar.v3.Data.Event;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Error_Handler_Control;

namespace CalendarBirthdayImporter
{
    public partial class _Default : Page
    {
        private const string AppName = "Calendar Birthday Importer";
        private const string CalendarName = "Birthdays";
        private const string CredentialName = "Credential";

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Session[CredentialName] != null)
            {
                pnlLogin.Visible = false;
                pnlImport.Visible = true;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            var credential = CalendarUtils.Login();

            if (credential != null)
            {
                Session.Add(CredentialName, credential);
            }
            else
            {
                Response.Write("Couldn't log you in :(");
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUploadControl.HasFile)
                {
                    var service = CalendarUtils.CreateService(AppName, Session[CredentialName] as UserCredential);
                    var employees = EmployeeUtils.ParseEmployeesFromFile(FileUploadControl.FileContent);
                    service.CreateBirthdayEventsInCalendar(CalendarName, employees);
                    ErrorSuccessNotifier.AddSuccessMessage("Added " + employees.Count() + " employees' birthdays to calendar :)");
                    return;
                }

                ErrorSuccessNotifier.AddWarningMessage("You didn't upload a file to import from!");
            }
            catch (ArgumentException ex)
            {
                ErrorSuccessNotifier.AddErrorMessage(ex.Message);
                return;
            }
        }
    }
}