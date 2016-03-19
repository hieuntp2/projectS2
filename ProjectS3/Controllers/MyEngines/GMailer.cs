using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System;
using System.IO;


namespace ProjectS3.Controllers.MyEngines
{
    public class GMailer
    {
        public async Task Send(string subject, string messagebody)
        {
            MyDynamicValues mydynamic = new MyDynamicValues();
            string mytoemail = mydynamic.getValue("toemail");
            string myfromemail = mydynamic.getValue("fromemail");
            string mypassword = mydynamic.getValue("password");

            string mysj = "Inthef.vn: " + subject;
            string body = messagebody;

            string templateName = "UpdateOrderToUserTemplate";
            var templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views/EmailTemplates/"+templateName);

            // Create a model for our email
            var model = new UpdateOrderToUserModel();

            // Generate the email body from the template file.
            // 'templateFilePath' should contain the absolute path of your template file.
            var templateService = new TemplateService();
            var emailHtmlBody = templateService.Parse(File.ReadAllText(templateFolderPath), model, null, null);

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
                Body = body,
                IsBodyHtml = true
            })
            {
                await smtp.SendMailAsync(message);
            }
        }
    }

    public class UpdateOrderToUserModel
    {
        public string hoten;
        public string email;
        public string iddonhang;
        public string tinhtrang;
    }
}