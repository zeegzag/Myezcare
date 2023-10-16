using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class SetFacilityHouseModel
    {
        public SetFacilityHouseModel()
        {
            RegionList = new List<Region>();
            PayorList = new List<PayorDropdown>();
            ParentFacilityList = new List<ParentFacilityList>();
            FacilityHouseModel = new FacilityHouseModel();
            StateCodeListModel = new List<StateCodeListModel>();
            FacilityTransportLocationMapping = new FacilityTransportLocationMapping();
            Facilities = new List<NameValueData>();
            TransportLocations = new List<NameValueData>();
            ScheduleStatuses = new List<ScheduleStatus>();
        }
        public List<Region> RegionList { get; set; }
        public List<PayorDropdown> PayorList { get; set; }
        public List<ParentFacilityList> ParentFacilityList { get; set; }
        public FacilityHouseModel FacilityHouseModel { get; set; }
        public List<StateCodeListModel> StateCodeListModel { get; set; }
        public List<NameValueData> Facilities { get; set; }
        public List<NameValueData> TransportLocations { get; set; }
        public List<ScheduleStatus> ScheduleStatuses { get; set; }

        [Ignore]
        public FacilityTransportLocationMapping FacilityTransportLocationMapping { get; set; }

        [Ignore]
        [Required(ErrorMessageResourceName = "FacilityRequired", ErrorMessageResourceType = typeof(Resource))]
        public long SelectedFaciltyID { get; set; }

        [Ignore]
        [Required(ErrorMessageResourceName = "TransportationLocationRequired", ErrorMessageResourceType = typeof(Resource))]
        public long SelectedTransportLocationID { get; set; }

    }

    public class HC_SetFacilityHouseModel
    {
        public HC_SetFacilityHouseModel()
        {
            RegionList = new List<Region>();
            StateCodeListModel = new List<StateCodeListModel>();
            FacilityHouseModel = new HC_FacilityHouseModel();
            EquipmentList = new List<FacilityHouseEquipmentModel>();
        }
        public List<Region> RegionList { get; set; }
        public List<StateCodeListModel> StateCodeListModel { get; set; }
        public HC_FacilityHouseModel FacilityHouseModel { get; set; }
        public List<FacilityHouseEquipmentModel> EquipmentList { get; set; }
    }

    public class PayorDropdown
    {
        public long PayorID { get; set; }
        public string PayorName { get; set; }
    }

    public class ParentFacilityList
    {
        public long ParentFacilityID { get; set; }
        public string FacilityName { get; set; }
    }

    public class SetFacilityHouseListModel
    {
        public SetFacilityHouseListModel()
        {
            RegionList = new List<Region>();
            FacilityHouseList = new List<FacilityHouseModel>();
            SearchFacilityHouseModel = new SearchFacilityHouseModel();
            DeleteFilter = new List<NameValueData>();
        }
        public List<Region> RegionList { get; set; }
        public List<FacilityHouseModel> FacilityHouseList { get; set; }
        public SearchFacilityHouseModel SearchFacilityHouseModel { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
    }

    public class HC_SetFacilityHouseListModel
    {
        public HC_SetFacilityHouseListModel()
        {
            SearchFacilityHouseModel = new SearchFacilityHouseModel();
            DeleteFilter = new List<NameValueData>();
            IsPartial = false;
        }
        public SearchFacilityHouseModel SearchFacilityHouseModel { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
        public bool IsPartial { get; set; }
    }

    public class FacilityHouseModel : BaseEntity
    {
        public long FacilityID { get; set; }

        [Display(Name = "FacilityName", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "FacilityNameRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "FacilityNameLength", ErrorMessageResourceType = typeof(Resource))]
        public string FacilityName { get; set; }

        public string FacilityBillingName { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "AddressRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "AddressLength", ErrorMessageResourceType = typeof(Resource))]
        public string Address { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "CityRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "CityLength", ErrorMessageResourceType = typeof(Resource))]
        public string City { get; set; }

        [Display(Name = "State", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "StateRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(10, ErrorMessageResourceName = "StateLength", ErrorMessageResourceType = typeof(Resource))]
        public string State { get; set; }

        [Display(Name = "ZipCode", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "ZipCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(15, ErrorMessageResourceName = "ZipCodeLength", ErrorMessageResourceType = typeof(Resource))]
        public string ZipCode { get; set; }

        [Display(Name = "Phone", ResourceType = typeof(Resource))]
        [StringLength(15, ErrorMessageResourceName = "PhoneLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "PhoneInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Phone { get; set; }

        [Display(Name = "Cell", ResourceType = typeof(Resource))]
        [StringLength(15, ErrorMessageResourceName = "CellLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidCell", ErrorMessageResourceType = typeof(Resource))]
        public string Cell { get; set; }

        [Display(Name = "Region", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RegionRequired", ErrorMessageResourceType = typeof(Resource))]
        public long RegionID { get; set; }
        public string RegionName { get; set; }

        [Display(Name = "County", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "CountyRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "CountyLength", ErrorMessageResourceType = typeof(Resource))]
        public string County { get; set; }

        [Display(Name = "GSA", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "GSARequired", ErrorMessageResourceType = typeof(Resource))]
        public int? GSA { get; set; }

        [Display(Name = "BadCapacity", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "BadCapacityRequired", ErrorMessageResourceType = typeof(Resource))]
        public int? BadCapacity { get; set; }

        [Display(Name = "PrivateRoomCount", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "PrivateRoomCountRequired", ErrorMessageResourceType = typeof(Resource))]
        public int? PrivateRoomCount { get; set; }

        [Display(Name = "SiteType", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "SiteTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "SiteTypeLength", ErrorMessageResourceType = typeof(Resource))]
        public string SiteType { get; set; }

        [Display(Name = "ProviderType", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "ProviderTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "ProviderTypeLength", ErrorMessageResourceType = typeof(Resource))]
        public string ProviderType { get; set; }

        [Display(Name = "Licensure", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "LicensureRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "LicensureLength", ErrorMessageResourceType = typeof(Resource))]
        public string Licensure { get; set; }

        [Display(Name = "LicensureRenewalDate", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "LicensureRenewalDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? LicensureRenewalDate { get; set; }

        [Display(Name = "FirePermitDate", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "FirePermitDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? FirePermitDate { get; set; }

        [Display(Name = "NPI", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "NPIRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(10, ErrorMessageResourceName = "NPILength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxNPI, ErrorMessageResourceName = "NPIInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string NPI { get; set; }

        [Display(Name = "AHCCCSID", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "AHCCCSIDRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(6, ErrorMessageResourceName = "AHCCCSIDLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxAHCCCSID, ErrorMessageResourceName = "AHCCCSIDInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string AHCCCSID { get; set; }

        [Display(Name = "EIN", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "EINRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(9, ErrorMessageResourceName = "EINLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxEIN, ErrorMessageResourceName = "EINInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string EIN { get; set; }
        public string PayorApproved { get; set; }
        public string EncryptedFacilityID { get { return FacilityID > 0 ? Crypto.Encrypt(Convert.ToString(FacilityID)) : ""; } }

        [Display(Name = "FacilityColorScheme", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "FacilityColorSchemeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string FacilityColorScheme { get; set; }

        [Display(Name = "DefaultScheduleStatus", ResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceName = "FacilityColorSchemeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? DefaultScheduleStatusID { get; set; }


        public long ParentFacilityID { get; set; }


        public int Row { get; set; }
        public int Count { get; set; }

        public string SetSelectedPayors { get; set; }
        public List<string> SelectedPayors { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class FacilityHouseEquipmentModel
    {
        public long FacilityHouseEquipmentID { get; set; }
        public long FacilityHouseID { get; set; }
        public long EquipmentID { get; set; }
        public string EquipmentName { get; set; }
    }

    public class HC_AddFacilityHouseModel
    {
        public HC_Facility Facility { get; set; }
        public List<FacilityHouseEquipmentModel> EquipmentList { get; set; }
    }

    public class HC_FacilityHouseModel : BaseEntity
    {
        public long FacilityID { get; set; }

        [Display(Name = "FacilityName", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "FacilityNameRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "FacilityNameLength", ErrorMessageResourceType = typeof(Resource))]
        public string FacilityName { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "AddressRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "AddressLength", ErrorMessageResourceType = typeof(Resource))]
        public string Address { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "CityRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "CityLength", ErrorMessageResourceType = typeof(Resource))]
        public string City { get; set; }

        [Display(Name = "State", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "StateRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(10, ErrorMessageResourceName = "StateLength", ErrorMessageResourceType = typeof(Resource))]
        public string State { get; set; }

        [Display(Name = "ZipCode", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "ZipCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(15, ErrorMessageResourceName = "ZipCodeLength", ErrorMessageResourceType = typeof(Resource))]
        public string ZipCode { get; set; }

        [Display(Name = "Phone", ResourceType = typeof(Resource))]
        [StringLength(15, ErrorMessageResourceName = "PhoneLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "PhoneInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Phone { get; set; }

        [Display(Name = "Cell", ResourceType = typeof(Resource))]
        [StringLength(15, ErrorMessageResourceName = "CellLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidCell", ErrorMessageResourceType = typeof(Resource))]
        public string Cell { get; set; }

        [Display(Name = "Region", ResourceType = typeof(Resource))]
        public long? RegionID { get; set; }
        public string RegionName { get; set; }

        [Display(Name = "County", ResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "CountyLength", ErrorMessageResourceType = typeof(Resource))]
        public string County { get; set; }

        [Display(Name = "GSA", ResourceType = typeof(Resource))]
        public int? GSA { get; set; }

        [Display(Name = "FacilityColorScheme", ResourceType = typeof(Resource))]
        public string FacilityColorScheme { get; set; }

        [Display(Name = "BadCapacity", ResourceType = typeof(Resource))]
        public int? BadCapacity { get; set; }

        [Display(Name = "PrivateRoomCount", ResourceType = typeof(Resource))]
        public int? PrivateRoomCount { get; set; }

        [Display(Name = "SiteType", ResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "SiteTypeLength", ErrorMessageResourceType = typeof(Resource))]
        public string SiteType { get; set; }

        [Display(Name = "ProviderType", ResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "ProviderTypeLength", ErrorMessageResourceType = typeof(Resource))]
        public string ProviderType { get; set; }

        [Display(Name = "Licensure", ResourceType = typeof(Resource))]
        public string Licensure { get; set; }

        [Display(Name = "LicensureRenewalDate", ResourceType = typeof(Resource))]
        public DateTime? LicensureRenewalDate { get; set; }

        [Display(Name = "FirePermitDate", ResourceType = typeof(Resource))]
        public DateTime? FirePermitDate { get; set; }

        [Display(Name = "NPI", ResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceName = "NPIRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(10, ErrorMessageResourceName = "NPILength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxNPI, ErrorMessageResourceName = "NPIInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string NPI { get; set; }

        [Display(Name = "AHCCCSID", ResourceType = typeof(Resource))]
        [StringLength(6, ErrorMessageResourceName = "AHCCCSIDLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxAHCCCSID, ErrorMessageResourceName = "AHCCCSIDInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string AHCCCSID { get; set; }

        [Display(Name = "EIN", ResourceType = typeof(Resource))]
        [StringLength(9, ErrorMessageResourceName = "EINLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxEIN, ErrorMessageResourceName = "EINInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string EIN { get; set; }
    }
    public class SearchFacilityHouseModel
    {
        public long FacilityID { get; set; }

        public long? AgencyID { get; set; }

        [StringLength(100, ErrorMessageResourceName = "NameLength", ErrorMessageResourceType = typeof(Resource))]
        public string FacilityName { get; set; }
        public string County { get; set; }
        public long RegionID { get; set; }

        [StringLength(15, ErrorMessageResourceName = "PhoneLength", ErrorMessageResourceType = typeof(Resource))]
        public string Phone { get; set; }

        [StringLength(15, ErrorMessageResourceName = "PhoneLength", ErrorMessageResourceType = typeof(Resource))]
        public string NPI { get; set; }

        [StringLength(15, ErrorMessageResourceName = "AHCCCSIDLength", ErrorMessageResourceType = typeof(Resource))]
        public string AHCCCSID { get; set; }

        [StringLength(15, ErrorMessageResourceName = "EINLength", ErrorMessageResourceType = typeof(Resource))]
        public string EIN { get; set; }

        [StringLength(50, ErrorMessageResourceName = "PayorLength", ErrorMessageResourceType = typeof(Resource))]
        public string PayorApproved { get; set; }

        public string ListOfIdsInCsv { get; set; }

        public int IsDeleted { get; set; }
        public DateTime StartDate { get; set; }
    }

}
