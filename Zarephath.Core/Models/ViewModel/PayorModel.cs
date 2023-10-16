using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public class AddPayorModel
    {
        public AddPayorModel()
        {
            StateCodeList = new List<StateCodeList>();
            PayorTypeList = new List<PayorTypeList>();
            Payor = new Payor();

            ModifierList = new List<Modifier>();
            POSList = new List<PlaceOfService>();
            PayorEdi837Setting = new PayorEdi837Setting();

            PayorServiceCodeMapping = new PayorServiceCodeMapping();
            SearchServiceCodeMappingList = new SearchServiceCodeMappingList();
            DeleteFilter = new List<NameValueData>();
            Facilities = new List<NameValueData>();
        }

        public List<StateCodeList> StateCodeList { get; set; }
        public List<PayorTypeList> PayorTypeList { get; set; }
        public Payor Payor { get; set; }

        public List<Modifier> ModifierList { get; set; }
        public List<PlaceOfService> POSList { get; set; }

        public PayorEdi837Setting PayorEdi837Setting { get; set; }

        public List<NameValueData> Facilities { get; set; }

        [Ignore]
        public PayorServiceCodeMapping PayorServiceCodeMapping { get; set; }



        //public List<PayorServiceCodeMappingList> PayorServiceCodeMappingList { get; set; }
        [Ignore]
        public SearchServiceCodeMappingList SearchServiceCodeMappingList { get; set; }
        [Ignore]
        public List<NameValueData> DeleteFilter { get; set; }
        //public string EncryptedPayorId { get; set; }
        //public ServiceCodeSearchModel ServiceCodeSearchModel { get; set; }
    }


    public class HC_SearchPayorModel
    {
        public HC_SearchPayorModel() { }
        public string PayorID { get; set; }
        public string PayorName { get; set; }

    }


    public class HC_PayorEnrollmentModel
    {
        public long PayorID { get; set; }
        public string EraPayorID { get; set; }
        public string EnrollType { get; set; }
        public string ProviderTaxID { get; set; }

        public string ProviderNPI { get; set; }
    }
    public class HC_AddPayorModel
    { 
        public HC_AddPayorModel()
        {
            StateCodeList = new List<StateCodeList>();
            ModifierList = new List<NameValueData>();
            Payor = new Payor();
            PayorEdi837Setting = new PayorEdi837Setting();


            NPIOptionsList = new List<NameValueStringData>();
            PayerGroupList = new List<NameValueStringData>();
            BussinessLineList = new List<NameValueStringData>();

            RevenueCodeList = new List<NameValueStringData>();
            UMList = new List<NameValueData>();

            CareTypeList = new List<NameValueStringData>();
            DeleteFilter = new List<NameValueData>();

            PayorServiceCodeMapping = new PayorServiceCodeMapping();
            SearchServiceCodeMappingList = new SearchServiceCodeMappingList();
            SerivceCodeModifierModel = new SerivceCodeModifierModel();
            PayorBillingTypeList = new List<NameValueDataInString>();
            PayorInvoiceTypeList = new List<NameValueData>();
            PayorClaimProcessorList = new List<NameValueStringData>();
            PayorVisitBilledByList = new List<NameValueStringData>();
            NPINumberList = new List<NameValueStringData>();
        }

        public List<StateCodeList> StateCodeList { get; set; }
        public List<NameValueData> ModifierList { get; set; }
        public Payor Payor { get; set; }

        [Ignore]
        public PayorEdi837Setting PayorEdi837Setting { get; set; }

        public List<NameValueStringData> NPIOptionsList { get; set; }
        public List<NameValueStringData> PayerGroupList { get; set; }
        public List<NameValueStringData> BussinessLineList { get; set; }
        public List<NameValueStringData> RevenueCodeList { get; set; }
        public List<NameValueStringData> CareTypeList { get; set; }

        [Ignore]
        public List<NameValueData> UMList { get; set; }
        [Ignore]
        public List<NameValueData> DeleteFilter { get; set; }
        [Ignore]
        public PayorServiceCodeMapping PayorServiceCodeMapping { get; set; }
        [Ignore]
        public SearchServiceCodeMappingList SearchServiceCodeMappingList { get; set; }
        [Ignore]
        public SerivceCodeModifierModel SerivceCodeModifierModel { get; set; }
        [Ignore]
        public List<NameValueDataInString> PayorBillingTypeList { get; set; }
        [Ignore]
        public List<NameValueData> PayorInvoiceTypeList { get; set; }
        [Ignore]
        public List<NameValueStringData> PayorClaimProcessorList { get; set; }
        [Ignore]
        public List<NameValueStringData> PayorVisitBilledByList { get; set; }
        public List<NameValueStringData> NPINumberList { get; set; }
    }

    public class SetPayorListPage
    {
        public SetPayorListPage()
        {
            PayorTypeList = new List<PayorTypeList>();
            SearchPayorListPage = new SearchPayorListPage();
            DeleteFilter = new List<NameValueData>();
        }
        public List<PayorTypeList> PayorTypeList { get; set; }
        public SearchPayorListPage SearchPayorListPage { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
    }

    public class HC_SetPayorListPage
    {
        public HC_SetPayorListPage()
        {
            PayerGroupList = new List<NameValueData>();
            SearchPayorListPage = new SearchPayorListPage();
            DeleteFilter = new List<NameValueData>();
            PayorBillingTypeList = new List<NameValueDataInString>();
        }
        public List<NameValueData> PayerGroupList { get; set; }
        [Ignore]
        public SearchPayorListPage SearchPayorListPage { get; set; }
        [Ignore]
        public List<NameValueData> DeleteFilter { get; set; }
        [Ignore]
        public List<NameValueDataInString> PayorBillingTypeList { get; set; }
    }

    public class SearchPayorListPage
    {
        public long PayorID { get; set; }
        public long? PayorTypeID { get; set; }
        public string PayorName { get; set; }
        public string ShortName { get; set; }
        public string PayorSubmissionName { get; set; }
        public string PayorIdentificationNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public int IsDeleted { get; set; }
        public string ListOfIdsInCSV { get; set; }
        public string AgencyNPID { get; set; }
        public int PayerGroup { get; set; }
        public string PayorBillingType { get; set; }
        public string NPINumber { get; set; }
    }

    public class StateCodeList
    {
        public string StateCode { get; set; }
        public string StateName { get; set; }
    }

    public class PayorTypeList
    {
        public string PayorTypeID { get; set; }
        public string PayorTypeName { get; set; }
    }


    public class SerivceCodeModifierModel
    {
        [Required(ErrorMessageResourceName = "ServiceCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ServiceCode { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
    }
    public class ModifierCheckModel
    {
        public long ModifierID { get; set; }
        public string ModifierCode { get; set; }
        public bool IsAvailable { get; set; }
    }

 
    public class SearchServiceCodeMappingList
    {
        public long CareType { get; set; }
        public string ServiceCode { get; set; }
        public string ModifierID { get; set; }
        public long PosID { get; set; }
        public DateTime? POSStartDate { get; set; }
        public DateTime? POSEndDate { get; set; }
        public long? PayorID { get; set; }
        public int IsDeleted { get; set; }
        public int Count { get; set; }
        public string ListOfIdsInCSV { get; set; }
        public string EncryptedPayorServiceCodeMappingId { get; set; }
        public long RevenueCode { get; set; }
        public string Rate { get; set; }

        public int? UnitType { get; set; }
        public decimal? PerUnitQuantity { get; set; }
        public int? RoundUpUnit { get; set; }
        public int? MaxUnit { get; set; }
        public int? DailyUnitLimit { get; set; }
    }

    public class ServiceCodeSearchModel
    {
        public string SearchText { get; set; }
        public List<string> ServiceCodes { get; set; }
    }

    public class PayorServiceCodeMappingListPage
    {
        public long PayorServiceCodeMappingID { get; set; }
        public long PayorID { get; set; }
        public string ModifierID { get; set; }
        public long PosID { get; set; }
        public string PayorName { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceCodeID { get; set; }
        public decimal Rate { get; set; }
        public string ModifierName { get; set; }
        public string PosName { get; set; }
        public bool ServiceCodeExpired { get; set; }
        public bool ServiceCodeMappingExpired { get; set; }

        public string StrServiceCodeExpired
        {
            get { return ServiceCodeExpired ? Resource.Expired : Resource.NotExpired; }
        }
        public string StrServiceCodeMappingExpired
        {
            get { return ServiceCodeMappingExpired ? Resource.Expired : Resource.NotExpired; }
        }
        public int UnitType { get; set; }
        public string StrUnitType
        {
            get { return UnitType == default(int) ? string.Empty : Common.GetEnumDisplayValue((EnumUnitType)UnitType); }
        }
        
        public long CareTypeID { get; set; }
        public string CareType { get; set; }
        //public string StrCareType
        //{
        //    get { return Common.GetEnumDisplayValue((EnumCareType)CareType); }
        //}
        public long RevenueCodeID { get; set; }
        public string RevenueCode { get; set; }

        public int ServiceCodeType { get; set; }
        //public string StrServiceCodeType
        //{
        //    get { return Common.GetEnumDisplayValue((EnumServiceCodeType)ServiceCodeType); }
        //}

        public DateTime POSStartDate { get; set; }
        public DateTime POSEndDate { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public int? MaxUnit { get; set; }
        public int? DailyUnitLimit { get; set; }
        public decimal? PerUnitQuantity { get; set; }
        public int? RoundUpUnit { get; set; }
        public bool IsBillable { get; set; }
        public bool HasGroupOption { get; set; }
        public DateTime ServiceCodeStartDate { get; set; }
        public DateTime ServiceCodeEndDate { get; set; }
        public int? BillingUnitLimit { get; set; }
        public int Count { get; set; }

        public bool IsDeleted { get; set; }

        [Ignore]
        public string EncryptedPayorId
        {
            get
            {
                return Crypto.Encrypt(Convert.ToString(PayorID));
            }
        }

        [Ignore]
        public string EncryptedPayorServiceCodeMappingId
        {
            get
            {
                return Crypto.Encrypt(Convert.ToString(PayorServiceCodeMappingID));
            }
        }
    }


    public class HC_PayorServiceCodeMappingListPage
    {
        public long PayorServiceCodeMappingID { get; set; }
        public long ServiceCodeID { get; set; }
        public long PayorID { get; set; }
        public string PayorName { get; set; }

        public long CareType { get; set; }
        public string StrCareType
        {
            get { return Common.GetEnumDisplayValue((EnumCareType)CareType); }
        }

        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public int MaxUnit { get; set; }
        public int DailyUnitLimit { get; set; }
        public int PerUnitQuantity { get; set; }
        public bool IsBillable { get; set; }

        public int UnitType { get; set; }
        public string StrUnitType
        {
            get { return Common.GetEnumDisplayValue((EnumUnitType)UnitType); }
        }

        public long ModifierID { get; set; }
        public string ModifierCode { get; set; }
        public string ModifierName { get; set; }

        public decimal Rate { get; set; }
        //public decimal UPCRate { get; set; }
        //public decimal NegRate { get; set; }
        public DateTime POSStartDate { get; set; }
        public DateTime POSEndDate { get; set; }
        public bool IsDeleted { get; set; }

        public long RevenueCode { get; set; }
        public long UM { get; set; }

        public bool ServiceCodeMappingExpired { get; set; }
        public string StrServiceCodeMappingExpired
        {
            get { return ServiceCodeMappingExpired ? Resource.Expired : Resource.NotExpired; }
        }

        public int Count { get; set; }

        [Ignore]
        public string EncryptedPayorId
        {
            get
            {
                return Crypto.Encrypt(Convert.ToString(PayorID));
            }
        }

        [Ignore]
        public string EncryptedPayorServiceCodeMappingId
        {
            get
            {
                return Crypto.Encrypt(Convert.ToString(PayorServiceCodeMappingID));
            }
        }
    }

    public class ListPayorModel
    {
        public long PayorID { get; set; }
        public string PayorName { get; set; }
        public string ShortName { get; set; }
        public string PayorSubmissionName { get; set; }
        public string PayorIdentificationNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string PayorTypeName { get; set; }
        public string EncryptedPayorID { get { return Crypto.Encrypt(Convert.ToString(PayorID)); } }
        public int Count { get; set; }
        public int EmpCount { get; set; }
        public bool IsDeleted { get; set; }
        public string AgencyNPID { get; set; }
        public string PayerGroup { get; set; }
        public string PayorBillingType { get; set; }
        public string NPINumber { get; set; }
    }

    public class GetPayorBillingSettings
    {
        public long PayorID { get; set; }
    }

    #endregion
}

