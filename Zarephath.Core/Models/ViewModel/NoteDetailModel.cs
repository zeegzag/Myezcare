using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Amazon.Runtime.Internal;
using ExpressiveAnnotations.Attributes;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class NoteDetailModel
    {
        public NoteDetailModel()
        {
            ClientInfo = new ClientInfoForNote();
            Note = new Note();

            KindOfNotes = new List<NameValueDataInString>();
            DxCodes = new List<DXCodeMappingList>();
            ServiceCodeTypes = new List<ServiceCodeType>();
            //ServiceCodes = new List<ServiceCodes>();
            //Services = new List<NameValueData>();
            EmpSignatureDetails=new EmpSignatureDetails();
            DefaultProviders = new DefaultProviders();
            Facilities = new List<NameValueData>();
            SignatureLog = new SignatureLog();
            PosCodes = new List<PosDropdownModel>();
            SelectedDxCodes = new List<DXCodeMappingList>();
            Employees = new List<NameValueData>();
            TempNoteList = new List<Note>();
            NoteSentenceList=new List<NoteSentence>();
            NotePayor=new NotePayor();

        }
        public ClientInfoForNote ClientInfo { get; set; }
        public Note Note { get; set; }
        public List<DXCodeMappingList> DxCodes { get; set; }
        public List<ServiceCodeType> ServiceCodeTypes { get; set; }
        //public string EmpSignature { get; set; }

        public EmpSignatureDetails EmpSignatureDetails { get; set; } 

        public DefaultProviders DefaultProviders { get; set; }
        public List<NameValueData> Facilities { get; set; }
        public SignatureLog SignatureLog { get; set; }
        public List<PosDropdownModel> PosCodes { get; set; }
        public List<NameValueData> Employees { get; set; }
        public List<Note> TempNoteList { get; set; }

        public List<NoteSentence> NoteSentenceList { get; set; }


        public NotePayor NotePayor { get; set; }


        //[Ignore]
        //public List<ServiceCodes> ServiceCodes { get; set; }
        [Ignore]
        public List<NameValueDataInString> Services { get; set; }
        [Ignore]
        public List<NameValueDataInString> KindOfNotes { get; set; }
        [Ignore]
        public List<NameValueDataInString> Relations { get; set; }

        [Ignore]
        public string EncryptedReferralID { get { return Note.ReferralID > 0 ? Crypto.Encrypt(Note.ReferralID.ToString()) : null; } }
        [Ignore]
        public List<DXCodeMappingList> SelectedDxCodes { get; set; }

        [Ignore]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "DxCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int NoteDxCodeCount { get { return SelectedDxCodes.Count; } }
    }


    public class NotePayor
    {
        public long NotePayorID { get; set; }
        public string NotePayorName { get; set; }
    }

    public class DefaultProviders
    {
        public long? DefaultBillingProviderID { get; set; }
        public long? DefaultRenderingProviderID { get; set; }
    }

    public class EmpSignatureDetails
    {
        public string Signature { get; set; }
        public string SignatureName { get; set; }
        public long SignatureLogID { get; set; }
        public long EmployeeID { get; set; }
        
    }


    

    public class ServiceCodeDropdownModel : ServiceCodes
    {
        public int PosID { get; set; }
        public string PosName { get; set; }
        public string PosText
        {
            get
            {
                EnumPlaceOfServices foo = (EnumPlaceOfServices)Enum.ToObject(typeof(EnumPlaceOfServices), PosID);
                return Common.GetEnumDisplayValue(foo);
            }
        }
        public long PayorServiceCodeMappingID { get; set; }
    }

    public class PosDropdownModel
    {
        public long PayorServiceCodeMappingID { get; set; }
        public long PayorID { get; set; }
        public int ServiceCodeID { get; set; }

        public string ServiceCode { get; set; }
        public string Description { get; set; }

        public int PosID { get; set; }
        public float Rate { get; set; }
        public int UnitType { get; set; }
        public int ServiceCodeType { get; set; }
        public int MaxUnit { get; set; }
        public float TotalUsedUnit { get; set; }
        public int DailyUnitLimit { get; set; }
        public float TodayUsedUnit { get; set; }
        public float PerUnitQuantity { get; set; }
        public string PosName { get; set; }
        public bool CheckRespiteHours { get; set; }

        public float AvailableMaxUnit { get; set; }
        public float AvailableDailyUnit { get; set; }
        public float CalculatedUnit { get; set; }
        public int UsedUnit { get; set; }

        public long MileDiff { get; set; }
        public int MinutesDiff { get; set; }

        public int DefaultUnitIgnoreCalculation { get; set; }
        

    }

    public class ClientInfoForNote
    {
        public string PayorName { get; set; }
        public string PayorID { get; set; }
        public string PayorShortName { get; set; }
        public string ClientName { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public string Population { get; set; }
        public string Title { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public float Age { get; set; }
        public string Empl { get; set; }
        public string Position { get; set; }
        public bool PermissionForVoiceMail { get; set; }
        public bool PermissionForEmail { get; set; }
        public bool PermissionForSMS { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone1 { get; set; }
        public float UsedRespiteHours { get; set; }
        public string StrAddress
        {
            //get { return Address + State + City; }
            get
            {
                string address = Address;
                if (!string.IsNullOrEmpty(City))
                    address = address + City;

                if (!string.IsNullOrEmpty(State))
                {
                    if (!string.IsNullOrEmpty(address))
                        address = address + ", " + State;
                    else
                        address = State;

                }

                if (!string.IsNullOrEmpty(ZipCode))
                {
                    if (!string.IsNullOrEmpty(address))
                        address = address + ", " + ZipCode;
                    else
                        address = ZipCode;
                }

                return address;
            }
        }
    }

    public class SetNoteListModel
    {
        public SetNoteListModel()
        {
            ServiceCodes = new List<ServiceCodeDropdownModel>();
            NoteTypes = new List<NameValueData>();
            SearchNoteListModel = new SearchNoteListModel();
            DeleteFilter = new List<NameValueData>();
            ServiceCodeTypes = new List<ServiceCodeType>();
            CompletedFilter = new List<NameValueData>();
            NoteKinds = new List<NameValueDataInString>();
            Facilities = new List<NameValueData>();
            Regions=new List<NameValueData>();
            Departments = new List<NameValueData>();
            Employees = new List<NameValueData>();
            Assignees = new List<NameValueData>();
        }
        public List<ServiceCodeDropdownModel> ServiceCodes { get; set; }


        public List<ServiceCodeType> ServiceCodeTypes { get; set; }



        [Ignore]
        public List<NameValueData> NoteTypes { get; set; }
        [Ignore]
        public SearchNoteListModel SearchNoteListModel { get; set; }
        [Ignore]
        public List<NameValueData> DeleteFilter { get; set; }
        [Ignore]
        public List<NameValueData> CompletedFilter { get; set; }
        [Ignore]
        public List<NameValueDataInString> NoteKinds { get; set; }
        [Ignore]
        public bool IsPartial { get; set; }


        public List<NameValueData> Facilities { get; set; }
        public List<NameValueData> Regions { get; set; }
        public List<NameValueData> Departments { get; set; }
        public List<NameValueData> Employees { get; set; }

        public List<NameValueData> PayorList { get; set; }

        [Ignore]
        public List<NameValueData> Assignees { get; set; }

    }

    public class SearchNoteListModel
    {
        public long PayorID { get; set; }
        public int IsBillable { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        //public int ServiceCodeID { get; set; }
        public long? BatchID { get; set; }
        public long? NoteID { get; set; }

        public int ServiceCodeTypeID { get; set; }
        public string SearchText { get; set; }
        public string NoteKind { get; set; }
        public int IsCompleted { get; set; }
        public long ReferralID { get; set; }

        public long RenderingProviderID { get; set; }
        public long DepartmentID { get; set; }
        //public long EmployeeID { get; set; }

        public string ListOfIdsInCsv { get; set; }
        public int IsDeleted { get; set; }

        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? ServiceDateStart { get; set; }
        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? ServiceDateEnd { get; set; }
        //[Required(ErrorMessageResourceName = "FacilityRequired", ErrorMessageResourceType = typeof(Resource))]
        public long BillingProviderID { get; set; }

        public long RegionID { get; set; }


        public string ServiceCodeIDs { get; set; }
        public string CreatedByIDs { get; set; }
        public long AssigneeID { get; set; }
    }

    public class ServiceCodeSearchParam
    {
        public string encReferralID { get; set; }
        public DateTime serviceDate { get; set; }
        public int serviceCodeTypeID { get; set; }
        public long PayorID { get; set; }
    }

    public class NoteDxCodeMappingList : NoteDxCodeMapping
    {
        public string DXCodeName { get; set; }
    }
}
