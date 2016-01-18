using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Wild8.Utils
{
    public class MailUtil
    {
        private static readonly MailUtil instance;
        private SmtpClient _smtpClient;
        private string _fromAddress;

        static MailUtil()
        {
            instance = new MailUtil();
        }

        private MailUtil()
        {
            _fromAddress = ConfigurationManager.AppSettings["Mail"];
            string username = ConfigurationManager.AppSettings["SmtpUsername"];
            string password = ConfigurationManager.AppSettings["SmtpPassword"];
            string host = ConfigurationManager.AppSettings["SmtpServer"];
            string port = ConfigurationManager.AppSettings["SmtpPort"];
            NetworkCredential credentials = new NetworkCredential(username, password);
            _smtpClient = new SmtpClient(host, Int32.Parse(port));
            _smtpClient.EnableSsl = true;
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.Credentials = credentials;
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        }

        public static MailUtil Instance
        {
            get
            {
                return instance;
            }
        }

        public void SendReceiptTo(string MailTo, string Subject, string Contents)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(_fromAddress);
            message.To.Add(new MailAddress(MailTo));
            message.Subject = Subject;
            message.IsBodyHtml = true;
            message.Body = Contents;
            _smtpClient.Send(message);
        }

    }
}