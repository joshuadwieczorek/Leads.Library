using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using AAG.Global.ExtensionMethods;
using Leads.Domain.Contracts.v1;

namespace Leads.Library.Helpers.v1
{
    public static class SmtpClientHelper
    {
        private static SmtpClient client;

        public static SmtpClient Client(EmailConfiguration configuration)
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            if (client is null)
                client = new SmtpClient();            

            client.DeliveryMethod = configuration.DeliveryMethod ?? throw new ArgumentNullException("configuration.DeliveryMethod is null");
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(configuration.SmtpUser, configuration.SmtpPassword);
            client.Host = configuration.SmtpHost;
            client.Port = configuration.Port ?? throw new ArgumentNullException("configuration.Port is null");
            client.EnableSsl = configuration.EnableSsl ?? throw new ArgumentNullException("configuration.EnableSsl is null");

            if (configuration.PickupDirectoryLocation is not null && configuration.PickupDirectoryLocation.HasValue())
            {
                if (!Directory.Exists(configuration.PickupDirectoryLocation))
                    throw new DirectoryNotFoundException($"Directory '{configuration.PickupDirectoryLocation}' does not exist");

                client.PickupDirectoryLocation = configuration.PickupDirectoryLocation;
            }

            return client;
        }
    }
}