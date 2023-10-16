using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;

namespace Zarephath.Core.Models.ViewModel
{
    public class EmailTokenBase
    {
        public EmailTokenBase()
        {
            CacheHelper cacheHelper = new CacheHelper();
            SiteURL = cacheHelper.SiteBaseURL;
            SiteName = cacheHelper.SiteName;
            SiteLogo = cacheHelper.SiteLogo;
        }


        public string SiteURL { get; set; }
        public string SiteName { get; set; }
        public string SiteLogo { get; set; }
    }

    public class EmailToken : EmailTokenBase
    {
        public string VerificationLink { get; set; }
        public string ResetPasswordLink { get; set; }
        // public string ZerpathLogoImage { get; set; }
    }
}
