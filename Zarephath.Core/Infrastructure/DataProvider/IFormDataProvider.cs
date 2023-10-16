using System.Collections.Generic;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IFormDataProvider
    {
        #region Form List For All Users To Save For The Patients & Employees
        ServiceResponse GetPatientEmpInfoModel();
        ServiceResponse SetFormListPage();
        ServiceResponse GetFormList(SearchFormModel model, int pageIndex, int pageSize, string sortIndex, string sortDirection);


        ServiceResponse GetSavedFormMappings(SearchFormModel model);
        ServiceResponse GetSavedFormList(List<UDT_EBFromMappingTable> mappingData, SearchFormModel model, int pageIndex, int pageSize, string sortIndex, string sortDirection);


        #endregion


        #region Organition Form Mapping Screen Page - This page Organization will use and add more forms for the Organization's Users
        ServiceResponse SetOrganizationFormsPage();
        ServiceResponse SaveOrganizationFormDetails(List<OrganizationFormModel> model, long loggedInId);
        ServiceResponse SaveOrganizationFormName(OrganizationForm model, long loggedInId);
        #endregion

        #region Form Tags
        List<FormTagModel> GetSearchTag(int pageSize, string searchText = null);
        ServiceResponse GetOrgFormTagList(long OrganizationFormID);
        ServiceResponse AddOrgFormTag(FormTagModel model);
        ServiceResponse DeleteFormTag(long OrganizationFormTagID);
        #endregion


        #region Load Internal HTML Forms
        ServiceResponse GetSavedHtmlFormContent(long ebriggsFormMppingID);

        ServiceResponse GetHTMLFormTokenReplaceModel(long referralID);

        ServiceResponse GetPdfFieldsData(long? id, int typeId);
        #endregion



        ServiceResponse AddEBCategory(string CategoryID, int IsINSUp);
        ServiceResponse AddEBCategory(EBCategory Category, int IsINSUp, long loggedInUserId);
        ServiceResponse SetEBCategoryListPage();
        ServiceResponse GetCategoryList(SearchEBCategoryListPage searchEBCategoryListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection);
        ServiceResponse DeleteCategory(SearchEBCategoryListPage searchEBCategoryListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId);

        List<EBCategory> HC_GetEBCategoryListForAutoComplete(string searchText, int pageSize);
    }
}
