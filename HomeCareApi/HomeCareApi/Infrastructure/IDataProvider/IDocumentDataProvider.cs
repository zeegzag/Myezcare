using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.General;
using HomeCareApi.Models.ViewModel;
using System.Web;

namespace HomeCareApi.Infrastructure.IDataProvider
{
    public interface IDocumentDataProvider
    {
        ApiResponse GetSectionList(ApiRequest<GetSectionModel> request);
        ApiResponse SaveSectionSubSection(ApiRequest<AddDirSubDirModal> request);
        ApiResponse DeleteSectionSubSection(ApiRequest<DeleteSecSubSecModel> request);
        ApiResponse GetOrganizationFormList(ApiRequest request);
        ApiResponse GetSubSectionList(ApiRequest<GetSubSecModel> request);
        ApiResponseList<DocumentList> GetDocumentList(ApiRequest<ListModel<SearchDocumentModel>> request);
        ApiResponse GetDocumentInfo(ApiRequest<DocumentInfoRequestModal> request);
        ApiResponse SaveFormPreference(ApiRequest<FormPreferenceModel> request);
        ApiResponse OpenSavedForm(ApiRequest<OpenFormModel> request);
        ApiResponse OpenSavedOrbeonForm(ApiRequest<OpenOrbeonFormModel> request);
        ApiResponse UpdateDocument(ApiRequest<DocumentModel> request);
        ApiResponse UploadDocument(HttpRequest currentHttpRequest, ApiRequest<PostDocumentModel> request);
        ApiResponse SaveForm(ApiRequest<SaveFormModel> request);
        ApiResponse SaveOrbeonForm(ApiRequest<SaveOrbeonFormModel> request);
        ApiResponse SaveFormName(ApiRequest<SaveFormNameRequestModal> request);
        ApiResponse DeleteDocument(ApiRequest<DeleteDocumentModel> request);

        #region Organization Settings
        ApiResponse GetOrganizationSettingDetail(ApiRequest<OrganizationDataRequest> request);
        #endregion
    }
}