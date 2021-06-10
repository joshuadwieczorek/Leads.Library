using System;
using System.Collections.Generic;
using Leads.Domain.Models.v1;

namespace Leads.Library.Helpers.v1
{
    public static partial class TemplatePopulator
    {
        /// <summary>
        /// Populate template with lead information.
        /// </summary>
        /// <param name="leadInformation"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public static string LeadInformation(
              LeadInformationModel leadInformation
            , string template
            , Dictionary<string, string> extraReplaceables = null)
        {
            if (template is null)
                return template;

            template = template.Replace("{fullName}", leadInformation.FullName);
            template = template.Replace("{firstName}", leadInformation.FirstName);
            template = template.Replace("{lastName}", leadInformation.LastName);
            template = template.Replace("{email}", leadInformation.Email);
            template = template.Replace("{cellPhone}", leadInformation.CellPhone);
            template = template.Replace("{homePhone}", leadInformation.HomePhone);
            template = template.Replace("{workPhone}", leadInformation.WorkPhone);
            template = template.Replace("{address1}", leadInformation.Address1);
            template = template.Replace("{address2}", leadInformation.Address2);
            template = template.Replace("{city}", leadInformation.City);
            template = template.Replace("{state}", leadInformation.State);
            template = template.Replace("{zip}", leadInformation.Zip);
            template = template.Replace("{referrer}", leadInformation.Referrer);
            template = template.Replace("{vehicleYear}", leadInformation.VehicleYear.ToNValue());
            template = template.Replace("{vehicleMake}", leadInformation.VehicleMake);
            template = template.Replace("{vehicleModel}", leadInformation.VehicleModel);
            template = template.Replace("{vehicleTrim}", leadInformation.VehicleTrim);
            template = template.Replace("{vehicleVdpUrl}", leadInformation.VehicleVdpUrl);
            template = template.Replace("{NewLine}", Environment.NewLine);

            if (extraReplaceables is not null && extraReplaceables.Count > 0)
                foreach (var item in extraReplaceables)
                    template = template.Replace($"{item.Key}", item.Value);

            return template;
        }
    }
}