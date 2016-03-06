using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using ProjectS3.Models;
namespace ProjectS3.Controllers.MyEngines
{
    public class GMailer
    {
        static GMailer()
        {
        }

        public void Send(string subject, string messagebody)
        {
            MyDynamicValues mydynamic = new MyDynamicValues();
            string mytoemail = mydynamic.getValue("toemail");
            string myfromemail = mydynamic.getValue("fromemail");
            string mypassword = mydynamic.getValue("password");

            var fromAddress = new MailAddress(myfromemail, "Inthef.vn");
            var toAddress = new MailAddress(mytoemail, "Website Inthef.vn");
            string fromPassword = mypassword;
            string mysj = "Inthef.vn: " + subject;
            string body = messagebody;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = mysj,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}