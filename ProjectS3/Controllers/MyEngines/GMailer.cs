using System;
using System.Net.Mail;
using System.IO;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System.Runtime.Remoting.Contexts;
using System.Net;
using System.Threading.Tasks;

namespace ProjectS3.Controllers.MyEngines
{
    public class GMailer
    {
        //public void Send(string subject, string messagebody)
        //{
        //    MyDynamicValues mydynamic = new MyDynamicValues();
        //    string mytoemail = mydynamic.getValue("toemail");
        //    string myfromemail = mydynamic.getValue("fromemail");
        //    string mypassword = mydynamic.getValue("password");

        //    string fromAddress = new MailAddress(myfromemail, "Inthef.vn").ToString();
        //    var toAddress = new MailAddress(mytoemail, "Website Inthef.vn");
        //    string fromPassword = mypassword;
        //    string mysj = "Inthef.vn: " + subject;
        //    string body = messagebody;

        //    var msg = new AE.Net.Mail.MailMessage
        //    {
        //        Subject = mysj,
        //        Body = body,
        //        From = new MailAddress(fromAddress)
        //    };
        //    msg.To.Add(new MailAddress(toAddress.ToString()));
        //    msg.ReplyTo.Add(msg.From); // Bounces without this!!
        //    var msgStr = new StringWriter();
        //    msg.Save(msgStr);

        //    var gmail = new GmailService();
        //    var result = gmail.Users.Messages.Send(new Message
        //    {
        //        Raw = Base64UrlEncode(msgStr.ToString())
        //    }, "me").Execute();
        //    Console.WriteLine("Message ID {0} sent.", result.Id);
        //}

        //private static string Base64UrlEncode(string input)
        //{
        //    var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
        //     Special "url-safe" base64 encode.
        //    return Convert.ToBase64String(inputBytes)
        //      .Replace('+', '-')
        //      .Replace('/', '_')
        //      .Replace("=", "");
        //}

        static GMailer()
        {
        }

        //public void Send(string subject, string messagebody)
        //{
        //    MyDynamicValues mydynamic = new MyDynamicValues();
        //    string mytoemail = mydynamic.getValue("toemail");
        //    string myfromemail = mydynamic.getValue("fromemail");
        //    string mypassword = mydynamic.getValue("password");

        //    var fromAddress = new MailAddress(myfromemail, "Inthef.vn");
        //    var toAddress = new MailAddress(mytoemail, "Website Inthef.vn");
        //    string fromPassword = mypassword;
        //    string mysj = "Inthef.vn: " + subject;
        //    string body = messagebody;

        //    var smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = true,
        //        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        //    };
        //    using (var message = new MailMessage(fromAddress, toAddress)
        //    {
        //        Subject = mysj,
        //        Body = body
        //    })
        //    {
        //        smtp.Send(message);
        //    }
        //}

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

            var message = new MailMessage();
            message.To.Add(new MailAddress(mytoemail));  // replace with valid value 
            message.From = new MailAddress(myfromemail);  // replace with valid value
            message.Subject = "Your email subject";
            message.Body = string.Format(body, "From name: HIEU", "From email: " + myfromemail, "This is message");
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = myfromemail,  // replace with valid value
                    Password = mypassword  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
        }
    }
}