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
            lock (_smtpClient) { 
                MailMessage message = new MailMessage();

                message.From = new MailAddress(_fromAddress);
                message.To.Add(new MailAddress(MailTo));
                message.Subject = Subject;
                message.Body = Contents;
                message.IsBodyHtml = true;
            
                _smtpClient.Send(message);
            }
        }

        private static string GenerateBody(OrderInfo Info)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<!DOCTYPE html>");
            builder.Append("<html>");
            builder.Append("<meta charset = \"utf-8\">");
            builder.Append("<body style = \"font-family: sans-serif;\">");
            builder.Append("<div style = \"font-size: 12pt;\">");
            builder.Append(RestaurauntInfo.Instance.RestaurauntName);
            builder.Append(", ");
            builder.Append(RestaurauntInfo.Instance.OwnerAddress);
            builder.Append(", ");
            builder.Append(RestaurauntInfo.Instance.OwnerCity);
            builder.Append("</div><br>");
            builder.Append("<div style = \"font-size: 30pt; border-bottom: solid 1px darkgray;\">Potvrda o narudžbi</div>");
            builder.Append("<p style = \"font-size: 18pt;\"><i>Okvirno vrijeme dostave: ");
            builder.Append(Info.DeliveryTime);
            builder.Append("</i></p>");
            builder.Append("<div style = \"font-size: 25pt;\">Sadržaj narudžbe:</div><br>");
            builder.Append("<table style = \"width: 100%; border-spacing: 0px;\">");
            builder.Append("<tr><td style = \"border-bottom: solid 1px black;\">Naziv</td>");
            builder.Append("<td style = \"border-bottom: solid 1px black;\">Količina</td>");
            builder.Append("<td style = \"border-bottom: solid 1px black; text-align: right;\">Ukupan Iznos</td></tr>");
            builder.Append(GenerateTable(Info));
            builder.Append("</table>");
            builder.Append("<h3>Ukupno: ");
            builder.Append(Info.TotalValue);
            builder.Append("</h3>");
            builder.Append("<h3>Način plaćanja: Gotovina (dostavljaču)</h3>");
            builder.Append("<p><i>Napomena restoranu: ");
            builder.Append(Info.Note);
            builder.Append("</i></p><br>");
            builder.Append("<p style = \"font-size: 15pt;\">Adresa za dostavu:</p>");
            builder.Append(Info.City);
            builder.Append("<br>");
            builder.Append(Info.AddressLine1);
            builder.Append("<br>");
            builder.Append(Info.AddressLine2);
            builder.Append("<br>");
            builder.Append("<br>");
            builder.Append("<p style = \"font-size: 15pt;\">Kontakt s restoranom:</p>");
            builder.Append(RestaurauntInfo.Instance.OwnerPhone);
            builder.Append("</body></html>");

            return builder.ToString();
        }

        private static string GenerateTable(OrderInfo Info)
        {
            bool lightBackground = true;
            const string lbg = "lightgray";
            const string dbg = "darkgray";

            StringBuilder builder = new StringBuilder();

            foreach (OrderUnit Unit in Info.OrderUnits)
            {
                builder.Append("<tr style = \"background-color: ");
                builder.Append(lightBackground ? lbg : dbg);
                builder.Append(";\">");
                builder.Append("<td>");
                builder.Append(Unit.IsAddon ? "+ ": "");
                builder.Append(Unit.Name);
                builder.Append("</td>");
                builder.Append("<td>");
                builder.Append(Unit.Amount);
                builder.Append("</td>");
                builder.Append("<td style = \"text-align: right;\">");
                builder.Append(Unit.TotalPrice);
                builder.Append("</td>");
                builder.Append("</tr>");
				
                lightBackground = !lightBackground;
            }

            return builder.ToString();
        }
    }

    public class OrderUnit
    {
        public bool IsAddon { get; set; }
        public string Name { get; set; }
        public string Amount { get; set; }
        public string TotalPrice { get; set; }

        public OrderUnit(bool isAddon, string name, string amount, string totalPrice)
        {
            IsAddon = isAddon;
            Name = name;
            Amount = amount;
            TotalPrice = totalPrice;
        }
    }

    public class OrderInfo
    {
        public string DeliveryTime { get; set; }
        public string TotalValue { get; set; }
        public List<OrderUnit> OrderUnits { get; }
        public string Note { get; set; }
        public string City { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        public OrderInfo()
        {
            OrderUnits = new List<OrderUnit>();
        }

        public void AddOrderUnit(OrderUnit Unit)
        {
            OrderUnits.Add(Unit);
        }
    }
}