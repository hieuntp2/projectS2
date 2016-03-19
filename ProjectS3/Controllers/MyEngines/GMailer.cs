using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System;
using System.IO;
using RazorEngine.Templating;


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

        public async Task SendWithUpdateOrderToUserTemplate(string subject, UpdateOrderToUserModel model)
        {
            string templateName = "UpdateOrderToUserTemplate.cshtml";
            var templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views/EmailTemplates/" + templateName);

            var templateService = new TemplateService();
            var body = templateService.Parse(File.ReadAllText(templateFolderPath), model, null, null);

            MyDynamicValues mydynamic = new MyDynamicValues();
            string mytoemail = model.email;
            string myfromemail = mydynamic.getValue("fromemail");
            string mypassword = mydynamic.getValue("password");

            string mysj = "Inthef.vn: " + subject;

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
        public string hoten { get; set; }
        public string email { get; set; }
        public string iddonhang { get; set; }
        public string tinhtrang { get; set; }
    }
}