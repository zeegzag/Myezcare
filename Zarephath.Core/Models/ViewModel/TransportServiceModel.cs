using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    #region Transport Service  Contact Model
    public class SetTransportContactModel
    {
        public SetTransportContactModel()
        {
            TransportContactModel = new TransportContactModel();
            StateCodeListModel = new List<StateCodeListModel>();
            TransportContactTypeListModel = new List<TransportContactTypeListModel>();
            OrganizationTypeListModel = new List<OrganizationTypeListModel>();
            SearchVehicleModel = new SearchVehicleModel();
        }
        public SearchVehicleModel SearchVehicleModel { get; set; }
        public TransportContactModel TransportContactModel { get; set; }
        public List<StateCodeListModel> StateCodeListModel { get; set; }
        public List<TransportContactTypeListModel> TransportContactTypeListModel { get; set; }
        public List<OrganizationTypeListModel> OrganizationTypeListModel { get; set; }

    }
    public class TransportContactModel : BaseEntity
    {
        public long ContactID { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "FirstNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "LastNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string LastName { get; set; }

        [RegularExpression(Constants.RegxMultipleEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }
        public string Phone { get; set; }

        [Required(ErrorMessageResourceName = "MobileNumberRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidMobileNumber", ErrorMessageResourceType = typeof(Resource))]
        public string MobileNumber { get; set; }
        public string Fax { get; set; }
        public string ApartmentNo { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "AddressRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "AddressLength", ErrorMessageResourceType = typeof(Resource))]
        public string Address { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "CityRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(25, ErrorMessageResourceName = "CityLength", ErrorMessageResourceType = typeof(Resource))]
        public string City { get; set; }

        [Display(Name = "State", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "StateRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(25, ErrorMessageResourceName = "StateLength", ErrorMessageResourceType = typeof(Resource))]
        public string State { get; set; }

        [Display(Name = "ZipCode", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "ZipCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(15, ErrorMessageResourceName = "ZipCodeLength", ErrorMessageResourceType = typeof(Resource))]
        public string ZipCode { get; set; }

        [Display(Name = "ContactType", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "ContactTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ContactType { get; set; }

        [Display(Name = "OrganizationID", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "OrganizationNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string OrganizationID { get; set; }
        public bool IsDeleted { get; set; }
        public string EncryptedContactID { get { return ContactID > 0 ? Crypto.Encrypt(Convert.ToString(ContactID)) : ""; } }
        public int Row { get; set; }
        public int Count { get; set; }

        public string FullName
        {
            get
            {
                if (!string.IsNullOrEmpty(FirstName))
                    return String.Format("{0} {1}", FirstName, LastName);

                return "";
            }
        }
        public string FullAddress
        {
            get
            {
                if (!string.IsNullOrEmpty(Address))
                    return String.Format("{0}, {1} - {2} {3} {4}", ApartmentNo, Address, City, State, ZipCode);

                return "";
            }
        }
    }

    public class TransportContactTypeListModel
    {
        public string ContactTypeID { get; set; }
        public string ContactTypes { get; set; }
    }

    public class TransportContactNameListModel
    {
        public string TransportID { get; set; }
        public string TransportName { get; set; }
    }

    public class HC_SetTransportContactListModel
    {
        public HC_SetTransportContactListModel()
        {
            TransportContactTypeListModel = new List<TransportContactTypeListModel>();
            SearchTransportContactModel = new SearchTransportContactModel();
            DeleteFilter = new List<NameValueData>();
            IsPartial = false;
        }
        public List<TransportContactTypeListModel> TransportContactTypeListModel { get; set; }
        public SearchTransportContactModel SearchTransportContactModel { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
        public bool IsPartial { get; set; }
    }

    public class SearchTransportContactModel
    {
        public long? ContactID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string MobileNumber { get; set; }
        public string Fax { get; set; }
        public string ApartmentNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ContactType { get; set; }
        public string OrganizationID { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public int IsDeleted { get; set; }
    }

    public class OrganizationTypeListModel
    {
        public string OrganizationID { get; set; }
        public string OrganizationName { get; set; }
    }
    public class GetSearchOrganizationNameModel
    {
        public string OrganizationID { get; set; }
    }
    #endregion

    #region Transport Assignment  
    public class SetTransportAssignmentModel
    {
        public SetTransportAssignmentModel()
        {
            TransportAssignmentModel = new TransportAssignmentModel();
            FacilityListModel = new List<FacilityListModel>();
            OrganizationTypeListModel = new List<OrganizationTypeListModel>();
            VehicleNameListModel = new List<VehicleNameListModel>();
            SearchTransportAssignmentModel = new SearchTransportAssignmentModel();
            RouteList = new List<RouteList>();
        }
        public TransportAssignmentModel TransportAssignmentModel { get; set; }
        public List<FacilityListModel> FacilityListModel { get; set; }
        public List<OrganizationTypeListModel> OrganizationTypeListModel { get; set; }
        public List<VehicleNameListModel> VehicleNameListModel { get; set; }
        public SearchTransportAssignmentModel SearchTransportAssignmentModel { get; set; }
        public List<RouteList> RouteList { get; set; }

    }
    public class TransportAssignmentModel : BaseEntity
    {
        public long TransportID { get; set; }
        public long FacilityID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Attendent { get; set; }
        public long VehicleID { get; set; }
        public long OrganizationID { get; set; }
        public long RouteCode { get; set; }
        public int IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
        public string EncryptedTransportID { get { return TransportID > 0 ? Crypto.Encrypt(Convert.ToString(TransportID)) : ""; } }
        public int Row { get; set; }
        public int Count { get; set; }

    }
    public class FacilityListModel
    {
        public long FacilityID { get; set; }
        public string FacilityName { get; set; }
    }
    public class VehicleNameListModel
    {
        public long VehicleID { get; set; }
        public string VehicleName { get; set; }
        public string VINNumber { get; set; }
        public string Model { get; set; }
        public string BrandName { get; set; }
    }
    public class SearchTransportAssignmentModel
    {
        public long? TransportID { get; set; }
        public long? FacilityID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Attendent { get; set; }
        public long? VehicleID { get; set; }
        public long? OrganizationID { get; set; }
        public long? RouteCode { get; set; }
        public int IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
        public string ListOfIdsInCsv { get; set; }
    }
    public class TransportAssignmentList
    {
        public long TransportID { get; set; }
        public long FacilityID { get; set; }
        public string FacilityName { get; set; }
        public long VehicleID { get; set; }
        public string VehicleName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Attendent { get; set; }
        public long RouteCode { get; set; }
        public string RouteName { get; set; }

        public long OrganizationID { get; set; }
        public int IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
        public string EncryptedTransportID { get { return TransportID > 0 ? Crypto.Encrypt(Convert.ToString(TransportID)) : ""; } }
        public int Row { get; set; }
        public int Count { get; set; }

    }
    public class RouteList
    {
        public long RouteCode { get; set; }
        public string RouteName { get; set; }
    }

    public class TransportAssignPatientModel : BaseEntity
    {
        public long TransportAssignPatientID { get; set; }
        public long ReferralID { get; set; }
        public long TransportID { get; set; }
        public DateTime? Startdate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Note { get; set; }
        public bool IsBillable { get; set; }
        public int IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
        public string EncryptedTransportID { get { return TransportID > 0 ? Crypto.Encrypt(Convert.ToString(TransportID)) : ""; } }
        public int Row { get; set; }
        public int Count { get; set; }

    }

    public class SetTransaportAssignPatientListModel
    {
        public SetTransaportAssignPatientListModel()
        {
            Payors = new List<PayorModel>();
            ReferralStatuses = new List<ReferralStatus>();
            //ReferralStatuses = new List<NameValueData>();
            Assignee = new List<EmployeeModel>();
            AssigneeList = new List<EmployeeDropDownModel>();
            NotifyCaseManager = new List<NameValueData>();
            Checklist = new List<NameValueData>();
            ClinicalReview = new List<NameValueData>();
            Facillator = new List<CaseManagerModel>();
            LanguageModel = new List<LanguageModel>();
            RegionModel = new List<RegionModel>();


            Services = new List<NameValueData>();
            Agencies = new List<AgencyModel>();
            AgencyLocations = new List<AgencyLocationModel>();
            SearchTransportAssignPatientListModel = new SearchTransportAssignPatientListModel();
            Draft = new List<NameValueData>();
            DeleteFilter = new List<NameValueData>();
            ServiceTypeList = new List<ServiceTypeModel>();
            TransportAssignPatientModel = new TransportAssignPatientModel();
        }
        public List<PayorModel> Payors { get; set; }
        public List<ReferralStatus> ReferralStatuses { get; set; }
        //public List<NameValueData> ReferralStatuses { get; set; }
        public List<EmployeeModel> Assignee { get; set; }
        public List<EmployeeDropDownModel> AssigneeList { get; set; }
        public List<NameValueData> NotifyCaseManager { get; set; }
        public List<NameValueData> Checklist { get; set; }
        public List<NameValueData> ClinicalReview { get; set; }
        public List<CaseManagerModel> Facillator { get; set; }
        public List<LanguageModel> LanguageModel { get; set; }
        public List<RegionModel> RegionModel { get; set; }
        public List<NameValueData> Services { get; set; }
        public List<AgencyModel> Agencies { get; set; }
        public List<AgencyLocationModel> AgencyLocations { get; set; }
        public SearchTransportAssignPatientListModel SearchTransportAssignPatientListModel { get; set; }
        public List<NameValueData> Draft { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
        public List<ServiceTypeModel> ServiceTypeList { get; set; }
        public TransportAssignPatientModel TransportAssignPatientModel { get; set; }
    }

    public class SearchTransportAssignPatientListModel
    {
        public long TransportID { get; set; }
        public long PayorID { get; set; }
        public long ReferralStatusID { get; set; }
        public long AssigneeID { get; set; }
        public string ClientName { get; set; }
        public int NotifyCaseManagerID { get; set; }
        public int ChecklistID { get; set; }
        public int ClinicalReviewID { get; set; }
        public long CaseManagerID { get; set; }
        public int ServiceID { get; set; }
        public long AgencyID { get; set; }
        public long AgencyLocationID { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public int IsSaveAsDraft { get; set; }
        public int IsDeleted { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public string ParentName { get; set; }
        public string ParentPhone { get; set; }
        public string CaseManagerPhone { get; set; }
        public long LanguageID { get; set; }
        public long RegionID { get; set; }
        public string ServiceTypeID { get; set; }
        public string[] GroupIds { get; set; }
        public string CommaSeparatedIds { get; set; }
        public string PayorName { get; set; }
        public string CareTypeID { get; set; }


    }

    public class TransportAssignPatientList
    {
        public long? TransportID { get; set; }//
        public long? ReferralID { get; set; }//
        public string Name { get; set; }//
        public string Address { get; set; }//
        public bool isAssigned { get; set; }//
        public DateTime? Startdate { get; set; }//
        public DateTime? EndDate { get; set; }//
        public string Note { get; set; }//
        public bool? IsBillable { get; set; }//
        public long TransportAssignPatientID { get; set; }//
        public long TransportGroupAssignPatientID { get; set; }//
        public long TransportGroupID { get; set; }
        public long RegionID { get; set; }
        public long Row { get; set; }

        public int Count { get; set; }

        public bool? IsDeleted { get; set; }
        public bool IsEdit { get; set; }
        public bool IsChecked { get; set; }
    }

    public class SetTransportAssignmentGroupModel
    {
        public SetTransportAssignmentGroupModel()
        {
            //TransportAssignmentModel = new TransportAssignmentModel();
            //OrganizationTypeListModel = new List<OrganizationTypeListModel>();
            FacilityListModel = new List<FacilityListModel>();
            TripDirectionList = new List<TripDirectionList>();            
            VehicleNameListModel = new List<VehicleNameListModel>();
            SearchTransportAssignmentGroupModel = new SearchTransportAssignmentGroupModel();
            TransportGroupModel = new TransportGroupModel();
            RegionModel = new List<RegionModel>();
        }
        //public TransportAssignmentModel TransportAssignmentModel { get; set; }
        public List<FacilityListModel> FacilityListModel { get; set; }
        public List<TripDirectionList> TripDirectionList { get; set; }
        //public List<OrganizationTypeListModel> OrganizationTypeListModel { get; set; }
        public List<VehicleNameListModel> VehicleNameListModel { get; set; }
        public SearchTransportAssignmentGroupModel SearchTransportAssignmentGroupModel { get; set; }
        public TransportGroupModel TransportGroupModel { get; set; }
        public List<RegionModel> RegionModel { get; set; }
        public TransportGroupAssignPatientModel TransportGroupAssignPatientModel { get; set; }

    }
    public class TripDirectionList
    {
        public long TripDirectionId { get; set; }
        public string TripDirectionName { get; set; }
    }
    public class SearchTransportAssignmentGroupModel
    {        
        public long? FacilityID { get; set; }
        public long? TripDirectionId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ClientName { get; set; }
        public string Attendent { get; set; }
        public long? VehicleID { get; set; }
        public long? OrganizationID { get; set; }
        public long? RouteCode { get; set; }
        public int IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
        public long TransportGroupID { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public string Address { get; set; }
        public long RegionID { get; set; }
    }
    public class TransportGroupModel
    {
        public long TransportGroupID { get; set; }
        public string Name { get; set; }
        public long FacilityID { get; set; }
        public long TripDirection { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long VehicleID { get; set; }
        public string RouteDesc { get; set; }
        public bool IsDeleted { get; set; }        
    }
    public class TransportGroupAssignPatientModel
    {
        public long TransportGroupAssignPatientID { get; set; }
        public long TransportGroupID { get; set; }
        public long ReferralID { get; set; }
        public string Note { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsBillable { get; set; }
        //
        public string Name { get; set; }
        public string Address { get; set; }
    }
    #endregion


}
