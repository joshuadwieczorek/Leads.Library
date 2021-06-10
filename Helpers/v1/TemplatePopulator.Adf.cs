using System;
using AAG.Global.ExtensionMethods;
using Leads.Domain.Contracts.v1;
using Leads.Domain.Models.v1;
using AAG.Global.Security;
using AAG.Global.Utilities;

namespace Leads.Library.Helpers.v1
{
    public static partial class TemplatePopulator
    {
        /// <summary>
        /// Generate encrypted adf.
        /// </summary>
        /// <param name="cryptographyProvider"></param>
        /// <param name="leadInformation"></param>
        /// <param name="adfSettings"></param>
        /// <param name="leadProviderName"></param>
        /// <param name="omitDeclaration"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        public static string Adf(
              CryptographyProvider cryptographyProvider
            , LeadInformationModel leadInformation
            , AdfSettings adfSettings
            , string leadProviderName
            , bool omitDeclaration = false
            , string comments = null)
        {
            if (cryptographyProvider is null)
                throw new ArgumentNullException(nameof(cryptographyProvider));

            if (leadInformation is null)
                throw new ArgumentNullException(nameof(leadInformation));

            if (adfSettings is null)
                throw new ArgumentNullException(nameof(adfSettings));

            leadInformation.Address1 = cryptographyProvider.Encrypt(leadInformation.FirstName);
            leadInformation.Address2 = cryptographyProvider.Encrypt(leadInformation.Address2);
            leadInformation.Email = cryptographyProvider.Encrypt(leadInformation.Email);
            leadInformation.CellPhone = cryptographyProvider.Encrypt(leadInformation.CellPhone);
            leadInformation.HomePhone = cryptographyProvider.Encrypt(leadInformation.HomePhone);
            leadInformation.WorkPhone = cryptographyProvider.Encrypt(leadInformation.WorkPhone);
            adfSettings.ContactName = cryptographyProvider.Encrypt(adfSettings.ContactName);
            adfSettings.VendorName = cryptographyProvider.Encrypt(adfSettings.VendorName);
            adfSettings.SourceName = cryptographyProvider.Encrypt(adfSettings.SourceName);
            adfSettings.ProviderService = cryptographyProvider.Encrypt(adfSettings.ProviderService);

            return Adf(leadInformation, adfSettings, leadProviderName, omitDeclaration);
        }


        /// <summary>
        /// Generate un-encrypted adf.
        /// </summary>
        /// <param name="leadInformation"></param>
        /// <param name="adfSettings"></param>
        /// <param name="leadProviderName"></param>
        /// <param name="omitDeclaration"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        public static string Adf(
              LeadInformationModel leadInformation
            , AdfSettings adfSettings
            , string leadProviderName
            , bool omitDeclaration = false
            , string comments = null)
        {
            if (leadInformation is null)
                throw new ArgumentNullException(nameof(leadInformation));

            if (adfSettings is null)
                throw new ArgumentNullException(nameof(adfSettings));

            var adfGenerator = new AdfGeneratorUtility()
                .SetFullName(leadInformation.FirstName, leadInformation.LastName)
                .SetEmailAddress(leadInformation.Email)
                .SetPhoneNumber(leadInformation.CellPhone ?? leadInformation.HomePhone ?? leadInformation.WorkPhone)
                .SetCountry("USA")
                .SetLeadId(Guid.NewGuid().ToString())
                .SetLeadSource(adfSettings.VendorName)
                .SetVendorContact(adfSettings.ContactName)
                .SetVendorName(adfSettings.VendorName)
                .SetProviderContact(adfSettings.ContactName)
                .SetProviderService(adfSettings.ProviderService)
                .SetProviderName(leadProviderName)
                .SetComments(comments);

            if (leadInformation.VehicleCondition.HasValue() && leadInformation.VehicleCondition.Lower().Contains("used"))
            {
                adfGenerator.SetVehicleCondition("used")
                    .SetVehicleYear(1938)
                    .SetVehicleMake("Ford")
                    .SetVehicleModel("7Y")
                    .SetVehicleStockNumber("AAGNOTFORSALE");
            }
                
            if (omitDeclaration)
                return adfGenerator
                    .Generate()
                    .OuterXml;

            return adfGenerator.ToString();
        }
    }
}