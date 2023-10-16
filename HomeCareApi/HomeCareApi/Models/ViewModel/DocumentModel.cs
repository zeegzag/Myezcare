using HomeCareApi.Infrastructure;
using HomeCareApi.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace HomeCareApi.Models.ViewModel
{
    public class DocumentModel
    {
        public long ReferralDocumentID { get; set; }
        public long EmployeeID { get; set; }
        public string FileName { get; set; }
        public int DocumentationType { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }

    public class DeleteDocumentModel
    {
        public long ReferralDocumentID { get; set; }
        public long EbriggsFormMppingID { get; set; }
        public string FilePath { get; set; }
        public long EmployeeID { get; set; }
        public string ServerCurrentDateTime { get; set; }
        public string SystemID { get; set; }
    }

    public class PostDocumentModel
    {
        public long EmployeeID { get; set; }
        public long ReferralID { get; set; }
        public long ComplianceID { get; set; }
        public string KindOfDocument { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }

    public class GetSubSecModel
    {
        public long ComplianceID { get; set; }
        public long ReferralID { get; set; }
    }

    public class DeleteSecSubSecModel
    {
        public long ComplianceID { get; set; }
        public long EmployeeID { get; set; }
    }

    public class GetSectionModel
    {
        public long ReferralID { get; set; }
    }

    public class FormPreferenceModel
    {
        public long ComplianceID { get; set; }
        public string EBFormID { get; set; }
        public bool SavePreference { get; set; }
        public long EmployeeID { get; set; }
        public long ReferralID { get; set; }
    }

    public class OpenFormModel
    {
        public long EbriggsFormMppingID { get; set; }
        public bool UpdateForm { get; set; }
    }

    public class OpenOrbeonFormModel
    {
        public long ReferralDocumentID { get; set; }
        public long ReferralID { get; set; }
    }

    public class SavedFormDetails
    {
        public string NameForUrl { get; set; }
        public string Version { get; set; }
        public string EBriggsFormID { get; set; }
        public bool IsInternalForm { get; set; }
        public bool IsOrbeonForm { get; set; }
        public string OrbeonFormID { get; set; }
        public string InternalFormPath { get; set; }
        public long ReferralID { get; set; }
        public string EBFormID { get; set; }
        public string FormId { get; set; }
        public string StoreType { get; set; }
    }

    public class SaveFormResult
    {
        public long EbriggsFormMppingID { get; set; }
        public string Action { get; set; }
        public string FormName { get; set; }
    }

    public class SectionSubsection
    {
        public long ComplianceID { get; set; }
        public string Name { get; set; }
        public bool IsTimeBased { get; set; }
        public string Color { get; set; }
        public Color ColorRGBA { get { return Common.HexToColor(Color); } }
        public int DocumentationType { get; set; }
        public string NameForUrl { get; set; }
        public string Version { get; set; }
        public bool IsInternalForm { get; set; }
        public bool IsOrbeonForm { get; set; }
        public string InternalFormPath { get; set; }
        public string EBFormID { get; set; }
        public string FormName { get; set; }
        public string FormURL { get; set; }
        public string FormId { get; set; }
    }

    public class SectionModal
    {
        public SectionModal()
        {
            SectionList = new List<SectionSubsection>();
        }
        public long Result { get; set; }
        public List<SectionSubsection> SectionList { get; set; }
    }

    public class AddDirSubDirModal
    {
        public long ComplianceID { get; set; }
        public int DocumentationType { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsTimeBased { get; set; }
        public long ParentID { get; set; }
        public string EBFormID { get; set; }
        public string UserType { get; set; }
        public long EmployeeID { get; set; }
    }

    public class FormListModel
    {
        public string EBFormID { get; set; }
        public string FormId { get; set; }
        public string Id { get; set; }

        public string FormNumber { get; set; }
        public string NameForUrl { get; set; }
        public string Version { get; set; }
        public string FormName { get; set; }
        public bool IsActive { get; set; }
        public bool HasHtml { get; set; }
        public string NewHtmlURI { get; set; }
        public bool HasPDF { get; set; }
        public string NewPdfURI { get; set; }
        public string OriginalFormName { get; set; }
        public string FriendlyFormName { get; set; }
        public string Tags { get; set; }

        public decimal? FormPrice { get; set; }


        public string EBCategoryID { get; set; }


        public string EbMarketIDs { get; set; }
        public List<string> EbMarketIDList
        {
            get
            {
                return new List<string>(EbMarketIDs.Split(','));
            }
        }

        public string FormCategory { get; set; }
        public string FormMarkets { get; set; }
        public bool IsDeleted { get; set; }


        public bool IsInternalForm { get; set; }
        public string InternalFormPath { get; set; }

        public long OrganizationFormID { get; set; }

        public bool IsNewForm { get; set; }

        public int Count { get; set; }
    }

    public class SearchDocumentModel
    {
        public string DocumentName { get; set; }
        public int DocumentationType { get; set; }
        public string SearchInDate { get; set; }
        public long ReferralID { get; set; }
        public long ComplianceID { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class DocumentList
    {
        public long ReferralDocumentID { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string KindOfDocument { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public long EbriggsFormMppingID { get; set; }
        public string EBriggsFormID { get; set; }
        public string NameForUrl { get; set; }
        public string Version { get; set; }
        public string Tags { get; set; }
        public string AddedBy { get; set; }
        public DateTime ReceivedDate { get; set; }
        public bool IsInternalForm { get; set; }
        public string StoreType { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }

    }

    public class SaveFormModel
    {
        public long ReferralID { get; set; }
        public long EmployeeID { get; set; }
        public string UserType { get; set; }
        public string EBriggsFormID { get; set; }
        public string OriginalEBFormID { get; set; }
        public string FormId { get; set; }
        public string HTMLFormContent { get; set; }

        public long? ComplianceID { get; set; }
        public long? TaskFormMappingID { get; set; }
        public long? ReferralTaskMappingID { get; set; }

        public long EbriggsFormMppingID { get; set; }
        public bool IsEditMode { get; set; }

    }

    public class SearchOrbeonFormSearch
    {
        DateTime CreatedDate { get; set; }
        public string FormName { get; set; }
        public string FormApp { get; set; }
        public string DocumentID { get; set; }
        public string OrganizationID { get; set; }
        public int EmployeeID { get; set; }
        public int ReferralID { get; set; }
    }

    public class SaveOrbeonFormModel
    {
        public string OrbeonFormID { get; set; }
        public bool IsEmployeeDocument { get; set; }
        public long ComplianceID { get; set; }
        public long ReferralDocumentID { get; set; }
        public long EmployeeID { get; set; }
        public long  ReferralID { get; set; }
        public DateTime ClockInTime { get; set; }
        public long EmployeeVisitID { get; set; }
        public long ReferralTaskFormMappingID { get; set; }
        public long ReferralTaskMappingID { get; set; }
        public long TaskFormMappingID { get; set; }
    }

    public class DeleteOrbeonFormModel
    {
        public long ReferralTaskFormMappingID { get; set; }
    }

    public class DocumentInfoRequestModal
    {
        public long EbriggsFormMppingID { get; set; }
        public long ReferralDocumentID { get; set; }
        public long ComplianceID { get; set; }
    }

    public class DocumentInfoModal
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class SaveFormNameRequestModal
    {
        public long EbriggsFormMppingID { get; set; }
        public string FormName { get; set; }
    }

    public class ConfigEBFormModel
    {
        public string EBBaseSiteUrl
        {
            get { return ConfigSettings.EbriggsUrl; }
        }
        public string ResuName
        {
            get { return ConfigSettings.EbriggsUserName; }
        }
        public string ResuKey
        {
            get { return ConfigSettings.EbriggsPassword; }
        }
        public string MyezcareFormsUrl
        {
            get { return ConfigSettings.MyezcareFormsUrl; }
        }
    }

    public enum UserTypes
    {
        Referral = 1, Employee
    }

    public enum DocumentKind
    {
        Internal = 1,
        External
    }
}