using System.Collections.Generic;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IOrgPreferenceDataProvider
    {
        ServiceResponse GetPreference(long organizationID);
        ServiceResponse SavePreference(OrganizationPreference preference, long loggedInUserId);
        OrganizationPreference Preferences(long organizationID);
    }
}
