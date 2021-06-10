using System;
using Leads.Domain.Contracts.v1;

namespace Leads.Library.Helpers.v1
{
    public static class EmailConfigurationHelper
    {
        public static EmailConfiguration Merge(
              EmailConfiguration config1
            , EmailConfiguration config2)
        {
            return new()
            {
                SmtpHost = config1.SmtpHost ?? config2.SmtpHost,
                SmtpUser = config1.SmtpUser ?? config2.SmtpUser,
                SmtpPassword = config1.SmtpPassword ?? config2.SmtpPassword,
                FromName = config1.FromName ?? config2.FromName,
                FromEmail = config1.FromEmail ?? config2.FromEmail,
                Port = config1.Port ?? config2.Port ?? throw new ArgumentNullException("both config1.Port and config2.Port are null"),
                EnableSsl = config1.EnableSsl ?? config2.EnableSsl ?? throw new ArgumentNullException("both config1.EnableSsl and config2.EnableSsl are null"),
                DeliveryMethod = config1.DeliveryMethod ?? config2.DeliveryMethod ?? throw new ArgumentNullException("both config1.DeliveryMethod and config2.DeliveryMethod are null"),
                PickupDirectoryLocation = config1.PickupDirectoryLocation ?? config2.PickupDirectoryLocation
            };
        }
    }
}