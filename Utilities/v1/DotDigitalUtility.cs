using System;
using DotDigital.Sdk;
using DotDigital.Sdk.v1.AddressBooks;
using DotDigital.Sdk.v1.Contacts;
using Leads.Domain.Contracts.v1;
using Leads.Domain.Models.v1;

namespace Leads.Library.Utilities.v1
{
    public sealed class DotDigitalUtility : IDisposable
    {
        private readonly DotDigitalSettings _dotDigitalSettings;
        private readonly AddressBookSdk _addressBookSdk;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dotDigitalSettings"></param>
        public DotDigitalUtility(DotDigitalSettings dotDigitalSettings)
        {
            _dotDigitalSettings = dotDigitalSettings;

            ServiceCredential credentials = new()
            {
                BaseUrl = dotDigitalSettings.BaseUrl,
                ApiUser = dotDigitalSettings.ApiUser,
                ApiPassword = dotDigitalSettings.ApiPassword
            };

            _addressBookSdk = new AddressBookSdk();
            _addressBookSdk.Configure(credentials);
        }        


        /// <summary>
        /// Upload lead to Dot Digital.
        /// </summary>
        /// <param name="leadInformationModel"></param>
        public void UploadToDotDigital(
              string leadProviderName
            , LeadInformationModel leadInformationModel)
        {
            ContactModel contact = new(leadInformationModel.Email);
            contact.AddField("FIRSTNAME", leadInformationModel.FirstName);
            contact.AddField("LASTNAME", leadInformationModel.LastName);
            contact.AddField("ZIPCODE", leadInformationModel.Zip);
            contact.AddField("LEADSOURCE", leadProviderName);
            contact.AddField("LEADSOURCEDATE", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"));
            _addressBookSdk.CreateAddressBookContact(_dotDigitalSettings.AddressBookId, contact);
        }


        /// <summary>
        /// Garbage cleanup.
        /// </summary>
        public void Dispose()
        {
            _addressBookSdk?.Dispose();
        }
    }
}