using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using Wild8.StaticInfo;

namespace Wild8.Utils
{
    public class MailUtil
    {
        private static SmtpClient _smtpClient; //= "smtp.gmail.com"; // klijent za slanje maila
        private static string _fromAddress; //= "username@gmail.com"; // adresa sa koje se šalje
        // ako želite testirat sa gmailom, morate enablat na gmailu da less-secure aplikacije mogu slat sa vašeg računa. ne znam zašto.

        public static void Initialize(string SMTPClient, string FromAddress, string UserName, string Password)
        {
            SmtpClient client = new SmtpClient(SMTPClient);

            client.Port = 587;
            client.Host = SMTPClient;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(UserName, Password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            _smtpClient = new SmtpClient(SMTPClient);
            _fromAddress = FromAddress;
        }

        public static void SendReceiptTo(string MailTo, string Subject, string Contents)
        {
            lock (_smtpClient)
            {
                MailMessage message = new MailMessage();

                message.From = new MailAddress(_fromAddress);
                message.To.Add(new MailAddress(MailTo));
                message.Subject = Subject;
                message.Body = Contents;
                message.IsBodyHtml = true;

                _smtpClient.Send(message);
            }
        }
    }
}