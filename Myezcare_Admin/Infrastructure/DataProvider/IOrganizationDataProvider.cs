using System.Collections.Generic;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.Entity;
using Myezcare_Admin.Models.ViewModel;
using System.Web;

namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public interface IOrganizationDataProvider
    {
        ServiceResponse SetAddOrganizationPage(long organizationId);
        ServiceResponse AddOrganization(MyEzcareOrganization organization, long loggedInUserId);
        ServiceResponse SetOrganizationListPage();
        ServiceResponse GetOrganizationList(SearchOrganizationModel searchOrganizationModel, int pageIndex, int pageSize, string sortIndex, string sortDirection);

        ServiceResponse SaveOrganization(AddOrganizationModel model, long loggedInUserId);

        ServiceResponse SaveFile(HttpRequestBase httpRequestBase, long loggedInUserID);
        ServiceResponse ImportDataInDatabase(ImportDataTypeModel model, long loggedInUserID);

        ServiceResponse SetOrganizationEsignPage(long organizationId, long organizationEsignId);
        ServiceResponse OrganizationEsign(OrganizationEsignModel organizationEsign, long loggedInUserId);
        ServiceResponse SendEsignEmail(OrganizationEsignDetails organizationEsign, long loggedInUserId);

        ServiceResponse SetCustomerEsignPage(string id);
        ServiceResponse CustomerEsign(CustomerEsignModel customerEsign, long loggedInUserId);
        ServiceResponse CheckDomainNameExists(string domainName);
        ServiceResponse GetAllOrganizationList();
    }
}
