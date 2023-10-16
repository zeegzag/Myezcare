using System.Collections.Generic;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.Entity;
using Myezcare_Admin.Models.ViewModel;

namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public interface IServicePlanDataProvider
    {
        ServiceResponse SetAddServicePlanPage(long servicePlanId);
        ServiceResponse AddServicePlan(ServicePlanModel servicePlan, long loggedInUserId);
        ServiceResponse SetServicePlanListPage();
        ServiceResponse GetServicePlanList(SearchServicePlanModel searchServicePlanModel, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteServicePlan(SearchServicePlanModel searchServicePlanModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId);
        List<ServicePlanComponentModel> GetServicePlanComponent(int pageSize, string searchText = null);
    }
}
