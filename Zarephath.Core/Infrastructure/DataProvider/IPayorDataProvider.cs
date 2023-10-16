using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IPayorDataProvider
    {
        #region Add Payor
        ServiceResponse SetAddPayorPage(long payorId);
        ServiceResponse AddPayorDetail(AddPayorModel addPayorModel, long loggedInUserId);
        #endregion

        #region Add Service Code
        ServiceResponse AddServiceCode(PayorServiceCodeMapping addPayorServiceCodeMappingModel, long loggedInUserId);
        ServiceResponse GetServiceCodeMappingList(string payorid, SearchServiceCodeMappingList searchServiceCodeMappingList, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteServiceCodeMapping(string encryptedPayorId, SearchServiceCodeMappingList searchServiceCodeMappingList, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        #endregion

        #region Payor List
        ServiceResponse SetPayorListPage();
        ServiceResponse GetPayorList(SearchPayorListPage searchPayorListPageModel, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeletePayor(SearchPayorListPage searchPayorListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);        
        #endregion

        #region Service Code Mapping
        List<ServiceCodes> GetServiceCodeList(string searchText, int pageSize);

        ServiceResponse HC_AddPayorServiceCodeNew(PayorServiceCodeMapping addPayorServiceCodeMappingModel, long loggedInUserId);
        ServiceResponse HC_GetServiceCodeMappingListNew(SearchServiceCodeMappingList searchServiceCodeMappingList, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeleteServiceCodeMappingNew(SearchServiceCodeMappingList searchServiceCodeMappingList, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        #endregion
        #region In Home Care Code

        #region Add Payor
        ServiceResponse HC_SetAddPayorPage(long payorId);
        ServiceResponse SearchPayor(HC_SearchPayorModel model);

        ServiceResponse PayorEnrollment(HC_PayorEnrollmentModel model);
        ServiceResponse HC_AddPayorDetail(AddPayorModel addPayorModel, long loggedInUserId);
        #endregion

        #region Add Service Code
        ServiceResponse HC_AddPayorServiceCode(PayorServiceCodeMapping addPayorServiceCodeMappingModel, long loggedInUserId);
        ServiceResponse HC_GetServiceCodeMappingList(string payorid, SearchServiceCodeMappingList searchServiceCodeMappingList, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeleteServiceCodeMapping(string encryptedPayorId, SearchServiceCodeMappingList searchServiceCodeMappingList, int pageIndex, int pageSize, string sortIndex, string sortDirection);

        ServiceResponse HC_AddServiceCode(SerivceCodeModifierModel serivceCodeModifierModel, long loggedInUserId);

        #endregion

        #region Payor List
        ServiceResponse HC_SetPayorListPage();
        ServiceResponse HC_GetPayorList(SearchPayorListPage searchPayorListPageModel, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeletePayor(SearchPayorListPage searchPayorListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        #endregion


        List<ServiceCodes> HC_GetServiceCodeList(string searchText, int pageSize);
        #endregion

        #region Save Billing Setting for 837 File
        ServiceResponse HC_GetPayorBillingSetting(long PayorID);
        ServiceResponse HC_SavePayorBillingSetting(PayorEdi837Setting payorEdi837Setting);

        ServiceResponse HC_GetAllBillingSetting(BillingSettingModel billingSettingModel);
        ServiceResponse HC_SaveBillingSetting(BillingSettingModel billingSettingModel);

        #endregion

    }
}
