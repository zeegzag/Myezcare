using System.Collections.Generic;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IComplianceDataProvider
    {
        ServiceResponse AddCompliance(int userType);
        ServiceResponse GetAssigneeList(int userType);
        ServiceResponse SaveCompliance(Compliance compliance, long loggedInUserId);
        ServiceResponse GetComplianceList(SearchComplianceListPage searchComplianceListPage, int pageIndex, int pageSize,string sortIndex, string sortDirection);
        ServiceResponse DeleteCompliance(SearchComplianceListPage searchComplianceListPage, int pageIndex, int pageSize,string sortIndex, string sortDirection,long loggedInUserId);
        ServiceResponse GetOrganizationFormList();

        ServiceResponse ChangeSortingOrder(ChangeSortingOrderModel model);
    }
}
