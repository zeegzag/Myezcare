using System.Collections.Generic;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.Entity;
using Myezcare_Admin.Models.ViewModel;

namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public interface IGeneralMasterDataProvider
    {
        ServiceResponse AddGeneralMaster(int ddTypeId);
        ServiceResponse SaveDDmaster(DDMaster ddMaster, long loggedInUserId);
        ServiceResponse GetGeneralMasterList(SearchDDMasterListPage searchDDMasterListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection);
        ServiceResponse DeleteDDMaster(SearchDDMasterListPage searchDDMasterListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId);
        ServiceResponse GetParentChildMappingDDMaster(long DDMasterTypeID = 0, bool isFetchParentRecord = false, long parentID = 0);

        ServiceResponse SaveParentChildMapping(long parentTaskId, List<long> childTaskIds);
    }
}
