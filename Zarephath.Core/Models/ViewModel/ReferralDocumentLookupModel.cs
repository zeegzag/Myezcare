using System;
using System.Collections.Generic;
using System.Drawing;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class ReferralDocumentLookupModel
    {
        public List<DocumentType> InternalDocuments { get; set; }
        public List<DocumentType> ExternalDocuments { get; set; }
    }

    public class DocumentPageModel
    {
        public ConfigEBFormModel ConfigEBFormModel { get; set; }
        public List<Section> SectionList { get; set; }
        public List<NameValueData> DocumentationTypeList { get; set; }
        public List<NameValueDataBoolean> SetYesNoList { get; set; }

        public List<Role> UserRoleList { get; set; }
        public List<string> SelectedRoles
        {
            get
            {
                List<string> data = new List<string>();
                foreach (var role in UserRoleList)
                    data.Add(Convert.ToString(role.RoleID));
                return data;
            }
        }
    }

    public class AddDirSubDirModal
    {
        public int DocumentationType { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsTimeBased { get; set; }
        public long ParentID { get; set; }
        public string EBFormID { get; set; }
        public string UserType { get; set; }
        public List<string> SelectedRoles { get; set; }
        public bool HideIfEmpty { get; set; }
        public string EmployeeID { get; set; }
        public string ReferralID { get; set; }
        public bool ShowToAll { get; set; }
    }

    public class Section
    {
        public long ComplianceID { get; set; }
        public string SectionName { get; set; }
        public bool IsTimeBased { get; set; }
        public string Color { get; set; }
        public Color ColorRGBA { get { return Common.HexToColor(Color); } }
        public int DocumentationType { get; set; }
        public string NameForUrl { get; set; }
        public string Version { get; set; }
        public bool IsInternalForm { get; set; }
        public string InternalFormPath { get; set; }
        public string FormName { get; set; }
        public string EBFormID { get; set; }
        public string FormId { get; set; }
        public bool IsOrbeonForm { get; set; }
    }

    public class SaveSection
    {
        public string SectionName { get; set; }
        public string Color { get; set; }
    }

    public class SectionModal
    {
        public SectionModal()
        {
            Result = new long();
            SectionList = new List<Section>();
        }
        public long Result { get; set; }
        public List<Section> SectionList { get; set; }
    }

    public class SaveSubSection
    {
        public long SectionID { get; set; }
        public string SubSectionName { get; set; }
        public string UserType { get; set; }
        public int DocumentationType { get; set; }
        public bool IsTimeBased { get; set; }
    }

    public class SubSection
    {
        public long ComplianceID { get; set; }
        public string SubSectionName { get; set; }
        public string NameForUrl { get; set; }
        public string Version { get; set; }
        public bool IsTimeBased { get; set; }
        public bool IsInternalForm { get; set; }
        public string InternalFormPath { get; set; }
        public string FormName { get; set; }
        public string EBFormID { get; set; }
        public string FormId { get; set; }
        public bool IsOrbeonForm { get; set; }
    }

    public class MapFormDetailModel
    {
        public MapFormDetailModel()
        {
            MapFormDetail = new MapFormDetail();
            SectionList = new List<Section>();
            SubSectionList = new List<SubSection>();
        }
        public MapFormDetail MapFormDetail { get; set; }
        public List<Section> SectionList { get; set; }
        public List<SubSection> SubSectionList { get; set; }
    }

    public class MapFormDetail
    {
        public string NameForUrl { get; set; }
        public string Version { get; set; }
        public string FormId { get; set; }
        public string EBFormID { get; set; }
        public string InternalFormPath { get; set; }
        public string FormName { get; set; }
        public bool IsInternalForm { get; set; }
        public bool IsOrbeonForm { get; set; }
    }

    public class EBFormAndDoc
    {
        public long ReferralDocumentID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public long EbriggsFormMppingID { get; set; }
        public string EBriggsFormID { get; set; }
        public string OriginalEBFormID { get; set; }
        public string Tags { get; set; }
        public string FormId { get; set; }
        public string NameForUrl { get; set; }
        public string Version { get; set; }
        public string FormName { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class DeleteDocModel
    {
        public long ReferralDocumentID { get; set; }
        public string FilePath { get; set; }
        public long EbriggsFormMppingID { get; set; }
        public long ComplianceID { get; set; }
        public long ReferralID { get; set; }
        public string StoreType { get; set; }
        public string GoogleFileId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class FormModal
    {
        public long ReferralID { get; set; }
        public long ComplianceID { get; set; }
    }

    public class DocFormNameModal
    {
        public long EbriggsFormMppingID { get; set; }
        public string FormName { get; set; }
        public bool UpdateFormName { get; set; }
    }

    public class MapFormDocModel
    {
        public long ComplianceID { get; set; }
        public long SectionID { get; set; }
        public string EBFormID { get; set; }
        public string UserType { get; set; }
        public bool MapPermanently { get; set; }
    }

    public class SearchReferralDocumentListPage
    {
        public string UserType { get; set; }
        public string EncryptedReferralID { get; set; }
        public string EncryptedEmployeeID { get; set; }
        public long ComplianceID { get; set; }
        public string Name { get; set; }
        public string SearchInDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int KindOfDocument { get; set; }
        public string SearchType { get; set; }
        public string IsDeleted { get; set; }
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
        public bool IsInternalForm { get; set; }
        public string InternalFormPath { get; set; }
        public string EBFormID { get; set; }
        public string FormId { get; set; }
        //public string AddedBy { get; set; }
        //public DateTime ReceivedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }
        public string StoreType { get; set; }
        public string GoogleFileId { get; set; }
        public bool IsOrbeonForm { get; set; }
        public string AccessFilePath { get => Common.GetAccessPath(FilePath); }
    }

    public class LinkDocModel
    {
        public long ReferralDocumentID { get; set; }
        public string FilePath { get; set; }
        public long ComplianceID { get; set; }
        public long ReferralID { get; set; }
        public long EmployeeID { get; set; }
        public string StoreType { get; set; }
        public string GoogleFileId { get; set; }
        public string DocumentID { get; set; }
    }
}
