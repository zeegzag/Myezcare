using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{

    #region Group Note Model

    public class GroupNoteModel
    {
        public GroupNoteModel()
        {
            ServiceCodeTypes = new List<ServiceCodeType>();
            EmpSignatureDetails=new EmpSignatureDetails();
            Facilities = new List<NameValueData>();
            AllFacilities = new List<NameValueData>();
            AllDropOffList = new List<NameValueData>();
            PosCodes = new List<PosDropdownModel>();
            Payors = new List<NameValueData>();
            SearchGroupNoteClient = new SaerchGroupNoteClient();
            Note = new Note();
            ReferralNotes = new List<ReferralNoteForGroupNote>();
            Employees = new List<NameValueData>();
            POSList = new List<PlaceOfService>();
            NoteSentenceList = new List<NoteSentence>();
            TempNoteList = new List<Note>();
            RegionList = new List<NameValueData>();
            ClientStatusList = new List<NameValueData>();
        }
        public List<ServiceCodeType> ServiceCodeTypes { get; set; }
        //public string EmpSignature { get; set; }
        public EmpSignatureDetails EmpSignatureDetails { get; set; } 
        public List<NameValueData> Facilities { get; set; }
        public List<NameValueData> AllFacilities { get; set; }
        public List<NameValueData> AllDropOffList { get; set; }
        public List<PosDropdownModel> PosCodes { get; set; }
        public List<NameValueData> Payors { get; set; }
        public List<NameValueData> Employees { get; set; }

        public List<PlaceOfService> POSList { get; set; }
        public List<NoteSentence> NoteSentenceList { get; set; }
        public List<NameValueData> RegionList { get; set; }
        public List<NameValueData> ClientStatusList { get; set; }

        [Ignore]
        public List<Note> TempNoteList { get; set; }

        [Ignore]
        public List<NameValueDataInString> Services { get; set; }
        [Ignore]
        public List<NameValueDataInString> KindOfNotes { get; set; }
        [Ignore]
        public List<NameValueDataInString> Relations { get; set; }

        [Ignore]
        public Note Note { get; set; }
        [Ignore]
        public SaerchGroupNoteClient SearchGroupNoteClient { get; set; }

        [Ignore]
        public List<ReferralNoteForGroupNote> ReferralNotes { get; set; }

        [Ignore]
        public List<DXCodeMappingList> SelectedDxCodes { get; set; }

        [Ignore]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "DxCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int GroupNoteDxCodeCount { get { return SelectedDxCodes != null ? SelectedDxCodes.Count : 0; } }

    }

    public class SaerchGroupNoteClient
    {
        public SaerchGroupNoteClient()
        {
            PageSize = 50;
        }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ClientName { get; set; }
        public long PayorID { get; set; }
        public long FacilityID { get; set; }
        public long ReferralStatusID { get; set; }
        public long RegionID { get; set; }

        public long DropOffLocationID { get; set; }

        public int PageSize { get; set; }
    }

    public class ReferralNoteForGroupNote
    {
        public Note Note { get; set; }
        public ReferralDetailForNote ReferralDetailForNote { get; set; }
    }

    #endregion


    #region Chnage Service Code Model

    public class ChangeServiceCodeModel
    {
        public ChangeServiceCodeModel()
        {
            Facilities = new List<NameValueData>();
            Payors = new List<NameValueData>();
            Employees = new List<NameValueData>();
            ServiceCodes=new List<NameValueData>();
            POSList=new List<PlaceOfService>();
            SearchNote = new SearchNote();
        }
        public List<NameValueData> Facilities { get; set; }
        public List<NameValueData> Payors { get; set; }
        public List<NameValueData> Employees { get; set; }
        public List<NameValueData> ServiceCodes { get; set; }
        public List<PlaceOfService> POSList { get; set; }

        [Ignore]
        public SearchNote SearchNote { get; set; }
    }

    public class SearchNote
    {
        public string ClientName { get; set; }
        public long? CreatedBy { get; set; }


        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? EndDate { get; set; }

        

        [Required(ErrorMessageResourceName = "PayorRequired", ErrorMessageResourceType = typeof(Resource))]
        public long PayorID { get; set; }

        [Required(ErrorMessageResourceName = "POSRequired", ErrorMessageResourceType = typeof(Resource))]
        public long PosID { get; set; }

        [Required(ErrorMessageResourceName = "ServiceCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long ServiceCodeID { get; set; }


        [Required(ErrorMessageResourceName = "ServiceCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long NewServiceCodeID { get; set; }

        [Required(ErrorMessageResourceName = "ServiceDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime ServiceStartDate { get; set; }

        public long PayorServiceCodeMappingID { get; set; }
        
        
    }



    public class ChangeServiceCodeNotes
    {
        public long ReferralID { get; set; }
        public string ReferralName { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }

        public long NoteID { get; set; }
        public long BatchID { get; set; }

        public DateTime ServiceDate { get; set; }

        public string ServiceCode { get; set; }
        public string ModifierCode { get; set; }
        
        public string ServiceName { get; set; }
        public string Description { get; set; }
        
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public float CalculatedUnit { get; set; }

        public DateTime? SignatureDate { get; set; }
        public string PayorShortName { get; set; }
        public float CalculatedAmount { get; set; }


        public string CreatedByUserName { get; set; }
        public string UpdatedByUserName { get; set; }
        public string SignatureBy { get; set; }
        public string StrStartDate { get { return StartTime.HasValue ? StartTime.Value.ToString("MM/dd/yyyy h:mm tt") : ServiceDate.ToString("MM/dd/yyyy"); } }
        public string StrEndDate { get { return EndTime.HasValue ? EndTime.Value.ToString("MM/dd/yyyy h:mm tt") : ServiceDate.ToString("MM/dd/yyyy"); } }
        public string BatchDetails { get; set; }


    }



    public class SelectedServiceCodeModel
    {
        public string ServiceCode { get; set; }
        public string POSStartDate { get; set; }
        public string POSEndDate { get; set; }
        public string Payor { get; set; }
        public decimal Rate { get; set; }
        public long PosID { get; set; }
        public long PayorServiceCodeMappingID  { get; set; }
    }



    #endregion

    #region Change Note Details

    public class ChangeNoteDetailsModel
    {
        public ChangeNoteDetailsModel()
        {
            Facilities = new List<NameValueData>();
            Payors = new List<NameValueData>();
            Employees = new List<NameValueData>();
            ServiceCodes = new List<NameValueData>();
            POSList = new List<PlaceOfService>();
            SearchNote = new SearchNoteModel();
        }
        public List<NameValueData> Facilities { get; set; }
        public List<NameValueData> Payors { get; set; }
        public List<NameValueData> Employees { get; set; }
        public List<NameValueData> ServiceCodes { get; set; }
        public List<PlaceOfService> POSList { get; set; }

        [Ignore]
        public SearchNoteModel SearchNote { get; set; }
    }

    public class SearchNoteModel
    {
        public string ClientName { get; set; }
        public long? CreatedBy { get; set; }


        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? EndDate { get; set; }



        //[Required(ErrorMessageResourceName = "PayorRequired", ErrorMessageResourceType = typeof(Resource))]
        public long ClientPayorID { get; set; }

        //[Required(ErrorMessageResourceName = "PayorRequired", ErrorMessageResourceType = typeof(Resource))]
        public long NotePayorID { get; set; }



        //[Required(ErrorMessageResourceName = "PayorRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ClientDXCode { get; set; }

        //[Required(ErrorMessageResourceName = "PayorRequired", ErrorMessageResourceType = typeof(Resource))]
        public string NoteDXCode { get; set; }



        //[Required(ErrorMessageResourceName = "POSRequired", ErrorMessageResourceType = typeof(Resource))]
        public long PosID { get; set; }

        public int IsBillable { get; set; }

        //[Required(ErrorMessageResourceName = "ServiceCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public List<long> ServiceCodeIDs { get; set; }


        //[Required(ErrorMessageResourceName = "ServiceCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long NewServiceCodeID { get; set; }

        //[Required(ErrorMessageResourceName = "ServiceDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime ServiceStartDate { get; set; }

        public long PayorServiceCodeMappingID { get; set; }

        public string SelectedOptionForCND { get; set; }

        public bool ShowMisMatchedDXCode { get; set; }
        public bool ShowMisMatchedPayor { get; set; }


        public long NewPayorID { get; set; }
    }


    public class ChangeNoteDetailsNotes
    {
        public long ReferralID { get; set; }
        public string ReferralName { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }

        public long NoteID { get; set; }
        public string EncryptedNoteID { get { return Crypto.Encrypt(NoteID.ToString()); } }

        public long BatchID { get; set; }

        public DateTime ServiceDate { get; set; }

        public string ServiceCode { get; set; }
        public string ModifierCode { get; set; }

        public string ServiceName { get; set; }
        public string Description { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public float CalculatedUnit { get; set; }

        public DateTime? SignatureDate { get; set; }

        public string ClientPayor { get; set; }

        public long ClientPayorID { get; set; }
        public string EncryptedClientPayor { get { return Crypto.Encrypt(ClientPayorID.ToString()); } }

        public string NotePayor { get; set; }

        public string PayorMismatched
        {
            get { return ClientPayor != NotePayor ? Resource.PayorMismatched : ""; }
        }

        public string DxCodeMismatched
        {
            get { return ReferralDXCode != NoteDxCode ? Resource.DXCodeMismatched : ""; }
        }

        public string PayorShortName { get; set; }
        public float CalculatedAmount { get; set; }


        public string CreatedByUserName { get; set; }
        public string UpdatedByUserName { get; set; }
        public string SignatureBy { get; set; }
        public string StrStartDate { get { return StartTime.HasValue ? StartTime.Value.ToString("MM/dd/yyyy h:mm tt") : ServiceDate.ToString("MM/dd/yyyy"); } }
        public string StrEndDate { get { return EndTime.HasValue ? EndTime.Value.ToString("MM/dd/yyyy h:mm tt") : ServiceDate.ToString("MM/dd/yyyy"); } }
        public string BatchDetails { get; set; }

        public string NoteDxCode { get; set; }
        public string ReferralDXCode { get; set; }


        public string PosID { get; set; }
        public string NewPayorName { get; set; }

        public string ErrorMsg { get; set; }

    }

    public class GetNewPSCMForNoteModel
    {
        public long NoteID { get; set; }
        public long PayorServiceCodeMappingID { get; set; }
        public long PayorID { get; set; }
    }

    #endregion

    #region Group Note Model
    public class DTRPageModel
    {
        public DTRPageModel()
        {
            AllDropOffList = new List<NameValueData>();
            AllFacilities = new List<NameValueData>();
            Payors = new List<NameValueData>();
            SearchGroupNoteClient = new SaerchGroupNoteClient();
            Note = new DriverVehicleDetail();
            ReferralNotes = new List<ReferralNoteForGroupNote>();
            Employees = new List<NameValueData>();
            RegionList = new List<NameValueData>();
            TripDetailList = new List<TripDetails>();
        }
        public List<NameValueData> AllDropOffList { get; set; }
        public List<NameValueData> AllFacilities { get; set; }
        public List<NameValueData> Payors { get; set; }
        public List<NameValueData> Employees { get; set; }
        public List<NameValueData> RegionList { get; set; }
        public List<NameValueData> ClientStatusList { get; set; }

        [Ignore]
        public List<TripDetails> TripDetailList { get; set; }

        [Ignore]
        public DriverVehicleDetail Note { get; set; }

        [Ignore]
        public SaerchGroupNoteClient SearchGroupNoteClient { get; set; }

        [Ignore]
        public List<ReferralNoteForGroupNote> ReferralNotes { get; set; }

    }

    public class DriverVehicleDetail
    {
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleType { get; set; }
        public string VehicleMakeColor { get; set; }
        public string MultipleMemberTransported { get; set; }
        public string PickDropDifferentForMember { get; set; }
        public DateTime? Date { get; set; }
    }

    public class TripDetails
    {
        public string PickUpAddress { get; set; }
        public string DropOffAddress { get; set; }
        public bool RoundTrip { get; set; }
        public bool OneWay { get; set; }
        public bool MultiStops { get; set; }
    }

    public class SaveDTRModel
    {
        public List<string> ReferralIDs { get; set; }
        public List<DTRPrintListModel01> TripDetailList { get; set; }
        public DriverVehicleDetail DriverVehicleDetail { get; set; }
    }
    public class DTRPrintListModel01
    {
        public string PayorName { get; set; }
        public long NoteID { get; set; }
        public string ClientName { get; set; }
        public string CISNumber { get; set; }
        public string AHCCCSID { get; set; }
        public DateTime ServiceDate_DT { get; set; }
        public string ServiceDate { get; set; }
        public string DXCodeName { get; set; }

        public DateTime StartTime_DT { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public bool StartTimeAM { get; set; }
        public bool EndTimeAM { get; set; }

        public string TempStartTime
        {
            get
            {
                if (!String.IsNullOrEmpty(StartTime) && (StartTime.Contains("AM") || StartTime.Contains("PM")))
                {
                    if (StartTime.Contains("AM")) StartTimeAM = true;
                    return StartTime.Remove(StartTime.Length - 3, 3);
                }
                return StartTime;
            }
        }
        public string TempEndTime
        {
            get
            {
                if (!String.IsNullOrEmpty(EndTime) && (EndTime.Contains("AM") || EndTime.Contains("PM")))
                {
                    if (EndTime.Contains("AM")) EndTimeAM = true;
                    return EndTime.Remove(EndTime.Length - 3, 3);
                }
                return EndTime;
            }
        }


        public DateTime? DateStartTime { get; set; }

        public string CalculatedUnit { get; set; }
        public string Startingodometer { get; set; }
        public string Endingodometer { get; set; }

        public string EmpSignature { get; set; }
        public string PrintDate { get; set; }
        public int CountPage { get; set; }
        public int TotalPage { get; set; }
        public string CredentialID { get; set; }
        public string AHCCCSLogoImage { get; set; }
        public string DriverName { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleType { get; set; }
        public string PickUpAddress { get; set; }
        public string DropOffAddress { get; set; }
        public string RoundTrip { get; set; }
        public string OneWay { get; set; }
        public string MultiStops { get; set; }
        public string EscortName { get; set; }
        public string RelationShip { get; set; }
        public string Dob { get; set; }

        public string VehicleMakeColor { get; set; }
        public bool MultipleMemberTransported { get; set; }
        public bool PickDropDifferentForMember { get; set; }

        public string DTRLOGLIST { get; set; }
        public string MailingAddress { get; set; }
        public int Tripmile
        {
            get { return Convert.ToInt32(Endingodometer) - Convert.ToInt32(Startingodometer); }
        }
        public string RandomGroupID { get; set; }
        public string ServiceCode { get; set; }
        public bool IsUsed { get; set; }

        public bool TempObject { get; set; }
    }
    #endregion

    #region Confidentiality Models
    public class ConfedentialityLogPageModel
    {
        public ConfedentialityLogPageModel()
        {
            AllFacilities = new List<NameValueData>();
            Payors = new List<NameValueData>();
            SearchGroupCLClient = new SearchGroupCLClient();
            ConfedentialityLogDetail = new ConfedentialityLogDetail();

            //ReferralNotes = new List<ReferralNoteForGroupNote>();
            Employees = new List<NameValueData>();
            RegionList = new List<NameValueData>();
        }
        public List<NameValueData> AllFacilities { get; set; }
        public List<NameValueData> Payors { get; set; }
        public List<NameValueData> Employees { get; set; }
        public List<NameValueData> RegionList { get; set; }
        public List<NameValueData> ClientStatusList { get; set; }


        [Ignore]
        public SearchGroupCLClient SearchGroupCLClient { get; set; }

        [Ignore]
        public ConfedentialityLogDetail ConfedentialityLogDetail { get; set; }

        //[Ignore]
        //public List<ReferralNoteForGroupNote> ReferralNotes { get; set; }

    }


    public class SearchGroupCLClient
    {
        public SearchGroupCLClient()
        {
            PageSize = 50;
        }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ClientName { get; set; }
        public long PayorID { get; set; }
        public long FacilityID { get; set; }
        public long ReferralStatusID { get; set; }
        public long RegionID { get; set; }
        public int PageSize { get; set; }
    }
    public class ConfedentialityLogDetail
    {
        public long ReferralConfidentialityLogID { get; set; }

        [Required(ErrorMessageResourceName = "AuditorRequired", ErrorMessageResourceType = typeof(Resource))]
        public long AuditorId { get; set; }

        [Required(ErrorMessageResourceName = "AuditDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime AuditDate { get; set; }

        [Required(ErrorMessageResourceName = "AuditReasonRequired", ErrorMessageResourceType = typeof(Resource))]
        public string AuditReason { get; set; }
    }
    public class SaveConfedentialityLogModel
    {
        public List<string> ReferralIDs { get; set; }
        public ConfedentialityLogDetail ConfedentialityLogDetail { get; set; }
    }


    public class ClientForConfidentialLog
    {
        public ClientForConfidentialLog()
        {
            ReferralDetailForCL = new List<ReferralDetailForCL>();
            //Facilities = new List<NameValueData>();
        }
        public List<ReferralDetailForCL> ReferralDetailForCL { get; set; }
        //public List<NameValueData> Facilities { get; set; }
    }


    public class ReferralDetailForCL
    {
        public long ReferralID { get; set; }
        public string Name { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public string Phone1 { get; set; }
        public string Address { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }
        public long PayorID { get; set; }
        public string PayorName { get; set; }
        public string StrDxCodes { get; set; }
        public string StrFacilities { get; set; }
        public string CaseManager { get; set; }
        public string RegionName { get; set; }
    }


    #endregion
}
