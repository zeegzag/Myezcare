using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure
{
    public interface IPreferenceDataProvider
    {
        ServiceResponse AddPreference(long preferenceId);
        ServiceResponse AddPreference(AddPreferenceModel addPreferenceModel, long loggedInUserId);

        ServiceResponse SetPreferenceListPage();
        ServiceResponse GetPreferenceList(SearchPreferenceListPage searchPreferenceListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeletePreference(SearchPreferenceListPage searchPreferenceListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId);
    }
}
