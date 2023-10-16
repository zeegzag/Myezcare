using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Utility;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    #region addPayor Detail && Service Code Detail

    public class AddServiceCodeModel
    {
        public AddServiceCodeModel()
        {
            ServiceCodes = new ServiceCodes();
            ModifierList = new List<NameValueData>();
            ServiceCodeTypeList = new List<NameValueData>();
            UnitTypeList = new List<NameValueData>();
        }
        public ServiceCodes ServiceCodes { get; set; }
        public List<NameValueData> ModifierList { get; set; }
        public List<NameValueData> ServiceCodeTypeList { get; set; }
        [Ignore]
        public List<NameValueData> UnitTypeList { get; set; }
    }
    public class SetServiceCodeListPage
    {
        public SetServiceCodeListPage()
        {
            ModifierList = new List<NameValueData>();
            ServiceCodeTypeList = new List<NameValueData>();
            SearchServiceCodeListPage = new SearchServiceCodeListPage();
            DeleteFilter = new List<NameValueData>();
        }
        public List<NameValueData> ModifierList { get; set; }
        public List<NameValueData> ServiceCodeTypeList { get; set; }
        [Ignore]
        public SearchServiceCodeListPage SearchServiceCodeListPage { get; set; }
        [Ignore]
        public List<NameValueData> DeleteFilter { get; set; }
    }

    public class HC_AddServiceCodeModel
    {
        public HC_AddServiceCodeModel()
        {
            ServiceCodes = new ServiceCodes();
            ModifierList = new List<Modifier>();
            CareTypeList = new List<NameValueData>();
            RevenueCodeList = new List<NameValueData>();
            PayorsList = new List<NameValueStringData>();
            VisitTypeList = new List<NameValueData>();
            UnitTypeList = new List<NameValueData>();
            ModifierModel = new Modifier();
            DeleteFilter = new List<NameValueData>();
        }
        
        public ServiceCodes ServiceCodes { get; set; }
        public List<Modifier> ModifierList { get; set; }
        public List<NameValueData> CareTypeList { get; set; }
        public List<NameValueData> RevenueCodeList { get; set; }
        public List<NameValueStringData> PayorsList { get; set; }
        public List<NameValueData> VisitTypeList { get; set; }
        [Ignore]
        public List<NameValueData> UnitTypeList { get; set; }
        [Ignore]
        public Modifier ModifierModel { get; set; }
        [Ignore]
        public List<NameValueData> DeleteFilter { get; set; }

        [Ignore]
        public PayorServiceCodeMapping PayorServiceCodeMapping { get; set; }
    }
    public class HC_SetServiceCodeListPage
    {
        public HC_SetServiceCodeListPage()
        {
            ModifierList = new List<NameValueData>();
            ServiceCodeTypeList = new List<NameValueData>();
            CareTypeList = new List<NameValueData>();
            SearchServiceCodeListPage = new SearchServiceCodeListPage();
            DeleteFilter = new List<NameValueData>();
        }
        public List<NameValueData> ModifierList { get; set; }
        public List<NameValueData> ServiceCodeTypeList { get; set; }
        public List<NameValueData> CareTypeList { get; set; }

        [Ignore]
        public SearchServiceCodeListPage SearchServiceCodeListPage { get; set; }
        [Ignore]
        public List<NameValueData> DeleteFilter { get; set; }
    }

    public class SearchServiceCodeListPage
    {
        public long CareType { get; set; }
        public string ServiceCode { get; set; }
        public string ModifierID { get; set; }
        public string ModifierName { get; set; }
        public string ServiceName { get; set; }
        public int ServiceCodeType { get; set; }
        public int UnitType { get; set; }
        public int IsBillable { get; set; }
        public int HasGroupOption { get; set; }
        public DateTime? ServiceCodeStartDate { get; set; }
        public DateTime? ServiceCodeEndDate { get; set; }
        public int IsDeleted { get; set; }
        public string ListOfIdsInCSV { get; set; }
        public string AccountCode { get; set; }
    }


    public class ListServiceCodeModel
    {
        public long ServiceCodeID { get; set; }
        public string CareType { get; set; }
        public string ServiceCode { get; set; }
        public string Modifier{ get; set; }
        public string ServiceName { get; set; }
        public string ServiceCodeType { get; set; }
        public int UnitType { get; set; }
        public string UnitTypeText
        {
            get {return Common.GetUnitType(UnitType); }
        }
        public string IsBillable  { get; set; }
        public string HasGroupOption  { get; set; }
        public DateTime ServiceCodeStartDate { get; set; }
        public DateTime ServiceCodeEndDate { get; set; }
        public string AccountCode { get; set; }
        public bool IsDeleted { get; set; }

        public string EncryptedServiceCodeID { get { return Crypto.Encrypt(Convert.ToString(ServiceCodeID)); } }
        public int Count { get; set; }
    }

    public class SearchModifierModel
    {
        public string ModifierCode { get; set; }
        public string ModifierName { get; set; }
        public int IsDeleted { get; set; }
        public string ListOfIdsInCSV { get; set; }
    }

    #endregion
}

