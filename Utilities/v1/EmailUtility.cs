using System.Net.Mail;
using System.Threading.Tasks;
using Leads.Domain.Contracts.v1;
using Leads.Library.Helpers.v1;

namespace Leads.Library.Utilities.v1
{
    public sealed class EmailUtility
    {
        private readonly SmtpClient _smtpClient;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public EmailUtility(EmailConfiguration configuration)
        {
            _smtpClient = SmtpClientHelper.Client(configuration);
        }


        /// <summary>
        /// Send email synchronously.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public void Send(EmailSettings settings)
        {
            using MailMessage message = new MailMessage();
            message.From = new MailAddress(settings.FromEmail);
            message.IsBodyHtml = settings.IsHtml;
            message.Subject = settings.Subject;
            message.Body = settings.Body ?? settings.Template;

            foreach (var email in settings.Recipients)
                message.To.Add(email);

            _smtpClient.Send(message);
        }


        /// <summary>
        /// Send email asynchronous.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public async Task SendAsync(EmailSettings settings)
        {
            using MailMessage message = new MailMessage();
            message.From = new MailAddress(settings.FromEmail);
            message.IsBodyHtml = settings.IsHtml;
            message.Subject = settings.Subject;
            message.Body = settings.Body ?? settings.Template;

            foreach (var email in settings.Recipients)
                message.To.Add(email);

            await _smtpClient.SendMailAsync(message);
        }
    }
}