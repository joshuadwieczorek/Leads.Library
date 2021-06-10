using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leads.Library.Helpers.Urls
{
    public static class UrlHelper
    {
        /// <summary>
        /// Lead providers urls.
        /// </summary>
        public static UrlsLeadProviders LeadProviders
            => new UrlsLeadProviders();
    }
}