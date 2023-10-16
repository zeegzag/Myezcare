using System;
using System.Collections.Generic;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class SetDxCodeListPage
    {
        public SetDxCodeListPage()
        {
            SearchDxCodeListPage = new SearchDxCodeListPage();
            DeleteFilter = new List<NameValueData>();
        }
        public SearchDxCodeListPage SearchDxCodeListPage { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
    }

    public class SearchDxCodeListPage
    {
        public string DXCodeName { get; set; }
        public string DXCodeWithoutDot { get; set; }
        public string Description { get; set; }
        public string DXCodeShortName { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int IsDeleted { get; set; }
        public string ListOfIdsInCsv { get; set; }

    }

    public class ListDxCodeModel
    {
        public string DXCodeID { get; set; }
        public string DXCodeName { get; set; }
        public string Description { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string EncryptedDXCodeID { get { return Crypto.Encrypt(Convert.ToString(DXCodeID)); } }
        public string DXCodeWithoutDot { get; set; }
        public bool IsDeleted { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }
        public bool IsDxCodeExpired { get; set; }
        public string DxCodeShortName { get; set; }
    }

    public class AddDxCodeModel
    {
        public AddDxCodeModel()
        {
            DxCode = new DXCode();
            DxCodeTypes = new List<DxCodeType>();
        }
        public DXCode DxCode { get; set; }
        public bool IsEditMode { get; set; }
        public List<DxCodeType> DxCodeTypes { get; set; }
    }

    public class ReferralDxCodeMappingDeleteModel
    {
        public bool IsSoftDelte { get; set; }
        public long ReferralDXCodeMappingID { get; set; }
        public string EncryptedReferralID { get; set; }
        public bool IsEnable { get; set; }
    }
    public class DXCodesModel
    {
        public string DXCodeID { get; set; }
        public string DXCodeName { get; set; }
        public string DXCodeWithoutDot { get; set; }
        public string DxCodeType { get; set; }
        public string Description { get; set; }
        public string EffectiveFrom { get; set; }
        public string EffectiveTo { get; set; }
        public string IsDeleted { get; set; }
        public string DxCodeShortName { get; set; }
    }

}
