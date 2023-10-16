
namespace Myezcare_Admin.Infrastructure
{
    public class StoredProcedure
    {


        public const string SetOrganizationListModel = "SetOrganizationListModel";
        public const string SaveOrganization = "SaveOrganization";

        public const string SaveReleaseNote = "SaveReleaseNote";
        public const string GetReleaseNoteList = "GetReleaseNoteList";
        public const string DeleteReleaseNote = "DeleteReleaseNote";

        public const string SaveOrganizationData = "SaveOrganizationData";
        public const string GetOrganizationData = "GetOrganizationData";

        public const string GetOrganizationList = "GetOrganizationList";
        public const string GetAllOrganizationList = "GetAllOrganizationList";

        #region ServicePlan

        public const string SaveServicePlanData = "SaveServicePlanData";
        public const string GetServicePlanList = "GetServicePlanList";
        public const string SetRolePermissionPage = "SetRolePermissionPage";
        public const string GetServicePlanDetails = "GetServicePlanDetails";
        public const string DeleteServicePlan = "DeleteServicePlan";

        #endregion ServicePlan
        #region Permissions
        public const string GetPermissionsList = "GetPermissionsList";
        public const string SavePermissions = "SavePermissions";
        public const string SetPermissionsList = "SetPermissionsList";
        public const string DeletePermission = "DeletePermission";
        public const string GetPermissionsData = "GetPermissionsData";

        #endregion


        #region DataImport

        public const string ValidateAndInsertAdminPatient = "ValidateAndInsertAdminPatient";
        public const string ValidateAndInsertAdminEmployee = "ValidateAndInsertAdminEmployee";
        public const string GetOrganizationDetailsById = "GetOrganizationDetailsById";

        #endregion

        public const string GetDDMasterList = "GetDDMasterList";
        public const string DeleteDDMaster = "DeleteDDMaster";
        public const string SaveDDmaster = "SaveDDmaster";
        public const string GetParentGeneralDetailForMapping = "GetParentGeneralDetailForMapping";
        public const string SaveParentChildMapping = "SaveParentChildMapping";
        public const string GetServicePlanComponent = "GetServicePlanComponent";


        #region Organization Esign

        public const string GetOrganizationEsignDetails = "GetOrganizationEsignDetails";
        public const string SaveOrganizationEsign = "SaveOrganizationEsign";
        public const string UpdateEsignStatus = "UpdateEsignStatus";
        public const string GetCustomerEsignDetails = "GetCustomerEsignDetails";
        public const string CheckDomainNameExists = "CheckDomainNameExists";
        public const string SaveCustomerEsign = "SaveCustomerEsign";
        public const string SetOrganizationFormPage = "SetOrganizationFormPage";

        #endregion Organization Esign



        public const string SyncEbFromRelatedAllData = "SyncEbFromRelatedAllData";
        public const string SetFormListPage = "SetFormListPage";
        public const string GetFormList = "GetFormList";
        public const string DeleteForm = "DeleteForm";
        public const string UpdateFormPrice = "UpdateFormPrice";

        #region RolePermission

        //public const string SetRolePermissionPage = "SetRolePermissionPage";
        public const string SetOrgPermissionPage = "SetOrgPermissionPage";
        public const string AddNewRole = "AddNewRole";
        public const string SaveRoleWisePermission = "SaveRoleWisePermission";
        public const string SavePermission = "SavePermission";
        public const string SaveOrganizationPermission = "SaveOrganizationPermission";
        #endregion RolePermission


        #region Invoice
        public const string AddInvoice = "SaveInvoice";
        public const string GetALLFilterInvoiceList = "GetALLFilterInvoiceList";
        public const string GetALLInvoiceList = "GetALLInvoiceList";
        public const string GetInvoiceByInvoiceNumber = "GetInvoiceByInvoiceNumber";
        public const string GetInvoiceNumber = "GetInvoiceNumber";
        public const string UpdateInvoice = "UpdateInvoice";

        #endregion
    }
}