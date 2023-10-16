using System.Collections.Generic;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IGeneralMasterDataProvider
    {
        ServiceResponse AddGeneralMaster(int ddTypeId);
        ServiceResponse SaveDDmaster(DDMaster ddMaster, List<long> childTaskIds, long loggedInUserId);
        ServiceResponse GetGeneralMasterList(SearchDDMasterListPage searchDDMasterListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection);
        ServiceResponse DeleteDDMaster(SearchDDMasterListPage searchDDMasterListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId);
        ServiceResponse GetParentChildMappingDDMaster(long DDMasterTypeID = 0, bool isFetchParentRecord = false, long parentID = 0);
        ServiceResponse CheckForParentChildMapping(DDMasterTypeModel model);
        ServiceResponse SaveParentChildMapping(long parentTaskId, List<long> childTaskIds);
        //ServiceResponse GetMasterDetailsApi(int ddTypeId);
    }
}
