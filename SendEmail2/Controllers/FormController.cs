using Microsoft.AspNetCore.Mvc;
using MimeKit;
using SendEmail2.Models;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Website.Controllers;
using MailKit.Net.Smtp;
using System.Threading.Tasks;


namespace SendEmail2.Controllers
{
    public class FormController : SurfaceController
    {
        public FormController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
        }

        // Action method for handling form submissions
        public async Task<ActionResult> Submit(FormData formData)
        {
            // Your form submission logic here
            // return Content("Form submitted successfully");

            try
            {
                // Process the form data
                // For example, you can access the submitted values using formData.Email, formData.Message, formData.Name

                // Send an email
                await SendEmailAsync(formData);

                // Return the submitted data as a JSON response
                return Json(new
                {
                    Email = formData.Email,
                    Message = formData.Message,
                    Name = formData.Name,
                    Success = true,
                });
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        private async Task SendEmailAsync(FormData formData)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sender Name", "sender@example.com"));
            message.To.Add(new MailboxAddress("Recipient Name", "ahmad.berkdar.sy@gmail.com"));
            message.Subject = "Form Submission";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = $"Name: {formData.Name}\nEmail: {formData.Email}\nMessage: {formData.Message}";
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.ethereal.email", 587, false);
                await client.AuthenticateAsync("gennaro89@ethereal.email", "Psg9QsrfAQjU5kZ4pw");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

    }
}
