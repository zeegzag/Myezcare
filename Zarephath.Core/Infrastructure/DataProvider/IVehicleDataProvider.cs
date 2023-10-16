using System.Collections.Generic;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IVehicleDataProvider
    {
        ServiceResponse HC_SetAddVehiclePage(long vehicleId);
        ServiceResponse HC_AddVehicle(SetVehicleModel vehicleModel);
        ServiceResponse HC_SetVehicleListPage();
        ServiceResponse HC_GetVehicleList(SearchVehicleModel searchVehicleModel, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeleteVehicle(SearchVehicleModel searchVehicleModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
    }
}
