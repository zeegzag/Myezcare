using System.Collections.Generic;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IFacilityHouseDataProvider
    {
        ServiceResponse SetAddFacilityHousePage(long facilityId);
        ServiceResponse GetParentFacilityHouse(long facilityId);
        ServiceResponse AddFacility(Facility facility, long loggedInUserId);
        ServiceResponse SetFacilityHousePage();
        ServiceResponse SetFacilityHouseList(SearchFacilityHouseModel searchFacilityHouseModel, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteFacilityHouse(SearchFacilityHouseModel searchFacilityHouseModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse GetFacilityTransportLocationMapping(long facilityID, long transportLocationID);
        ServiceResponse SaveFacilityTransportLocationMapping(FacilityTransportLocationMapping model);

        #region Home Care
        ServiceResponse HC_SetAddFacilityHousePage(long facilityId);
        ServiceResponse HC_AddFacility(HC_AddFacilityHouseModel facilityModel, long loggedInUserId);
        List<FacilityHouseEquipmentModel> GetEquipment(int pageSize, string ignoreIds, string searchText);
        ServiceResponse HC_SetFacilityHouseListPage();
        ServiceResponse HC_GetFacilityHouseList(SearchFacilityHouseModel searchFacilityHouseModel, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeleteFacilityHouse(SearchFacilityHouseModel searchFacilityHouseModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        #endregion
    }
}
