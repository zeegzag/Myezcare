using System.Collections.Generic;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IPhysicianDataProvider
    {
        ServiceResponse AddPhysician(long physicianID);
        ServiceResponse AddPhysician(Physicians physician, long loggedInUserId);
        ServiceResponse SetPhysicianListPage();
        ServiceResponse GetPhysicianList(SearchPhysicianListPage searchPhysicianListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection);
        ServiceResponse DeletePhysician(SearchPhysicianListPage searchPhysicianListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId);

        List<Physicians> HC_GetPhysicianListForAutoComplete(string searchText, int pageSize);
        List<Specialist> GetSpecialistListForAutoComplete(string searchText, string ignoreIds, int pageSize);
        ServiceResponse SaveSpecialist(string Specialist, string Name, string NPI, string PracticeAddress);
    }
}
