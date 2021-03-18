using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HandyCrypto.Services
{
    public static class MailSendingService
    {
        public static void Send(string subject,string body,string email)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("lukaminjoraia@gmail.com");
            mail.To.Add("lukaminjoraia1@gmail.com");
            mail.Subject = subject;
            mail.Body = body + " " + email;

            SmtpServer.Port = 587;
            SmtpServer.Host = "smtp.gmail.com";
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential("lukaminjoraia@gmail.com", "capitangerrard");

            SmtpServer.Send(mail);
        }
    }
}