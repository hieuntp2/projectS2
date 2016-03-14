using System.Net.Mail;
using System.Net;


namespace ProjectS3.Controllers.MyEngines
{
    public class GMailer
    {
        public void Send(string subject, string messagebody)
        {
            MyDynamicValues mydynamic = new MyDynamicValues();
            string mytoemail = mydynamic.getValue("toemail");
            string myfromemail = mydynamic.getValue("fromemail");
            string mypassword = mydynamic.getValue("password");

            string mysj = "Inthef.vn: " + subject;
            string body = messagebody;


            var fromAddress = new MailAddress(myfromemail, "Website Inthef.vn");
            var toAddress = new MailAddress(mytoemail, "Admin Inthef.vn");
            string fromPassword = mypassword;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = mysj,
                Body = body
            })
            {
                smtp.Send(message);
#if DEBUG
#else
                smtp.Send(message);
#endif
            }
        }
    }
}