using System.Collections.Generic;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IOnboardinDataProvider
    {
        ServiceResponse SetWizardStatus(long orgId, string menu, bool isCompleted, long loggedInUserId);
        List<OnboardingViewModel> GetWizardStatus(long orgId);
    }
}
