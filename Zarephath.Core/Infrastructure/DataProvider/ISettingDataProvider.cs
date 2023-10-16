using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Infrastructure.DataProvider
{
   public interface ISettingDataProvider
   {
       ServiceResponse GetSettings();
       ServiceResponse SaveSettings(OrganizationSetting settings, long userId);
       ServiceResponse SendTestEmail(OrganizationSetting settings, long userId);

        ServiceResponse SaveTermsAndConditions(long organizationId, string termsAndConditions, long userId);
    }
}
