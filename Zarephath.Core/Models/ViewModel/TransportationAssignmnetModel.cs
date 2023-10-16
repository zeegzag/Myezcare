using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PetaPoco;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class TransportationAssignmentModel
    {
        public TransportationAssignmentModel()
        {
            SearchRefrralForTransportatiAssignment = new SearchReferralListForTransportationAssignment();
            AddTransportationGroupModel = new AddTransportationGroupModel();
            SetTransportLocationDropDown = new List<NameValueForSpDropdown>();
            FacilityList = new List<NameValueForSpDropdown>();
            TransportationFilters = new List<TransportationFilter>();
            TransportationDirectionList = new List<NameValueData>
                {
                    new NameValueData
                        {
                            Name=TransportationGroup.TripDirectionUp
                        },
                        new NameValueData
                        {
                            Name=TransportationGroup.TripDirectionDown
                        }};
        }

        public List<NameValueForSpDropdown> SetTransportLocationDropDown { get; set; }
        public List<NameValueForSpDropdown> FacilityList { get; set; }
        public List<NameValueForSpDropdown> EmployeeList { get; set; }
        public List<TransportationFilter> TransportationFilters { get; set; }


        [Ignore]
        public List<NameValueData> TransportationDirectionList { get; set; }

        [Ignore]
        public SearchReferralListForTransportationAssignment SearchRefrralForTransportatiAssignment { get; set; }

        [Ignore]
        public AddTransportationGroupModel AddTransportationGroupModel { get; set; }

    }

    //public class SetTransportLocationDropDown
    //{
    //    public long TransportLocationID { get; set; }
    //    public string Location { get; set; }
    //}

    //public class SetFacilityDropDown
    //{
    //    public long FacilityID { get; set; }
    //    public string FacilityName { get; set; }
    //}

    public class SearchReferralListForTransportationAssignment
    {

        public long FacilityID { get; set; }
        public long TransportLocationID { get; set; }
        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? StartDate { get; set; }
        public string TripDirection { get; set; }
        public int ContactTypeID { get; set; }
    }

    public class ReferralListForTransportationAssignment
    {
        public long ReferralID { get; set; }
        public long ScheduleID { get; set; }
        public bool IsAssignedToTransportationGroupUp { get; set; }
        public bool IsAssignedToTransportationGroupDown { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string ParentName { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public string FacilityName { get; set; }
        public string ScheduleStatusName { get; set; }
        public string Email { get; set; }
        public string CISNumber { get; set; }
        public string AHCCCSID { get; set; }
        public string Phone2 { get; set; }
        public bool NeedPrivateRoom { get; set; }
        public string PayorName { get; set; }
        public string FrequencyCode { get; set; }

        public int Count { get; set; }
    }

    public class AddTransportationGroupModel
    {
        public AddTransportationGroupModel()
        {
            SelectedStaffs = new List<long>();
            TransportationGroup = new TransportationGroup();
        }
        public TransportationGroup TransportationGroup { get; set; }

        //[Required(ErrorMessageResourceName = "SelectStaff", ErrorMessageResourceType = typeof(Resource))]
        public List<long> SelectedStaffs { get; set; }
    }

    public class SearchAssignedClientListForTransportationAssignment
    {
        public DateTime Date { get; set; }
    }

    public class TransportationGroupDetail : TransportationGroup
    {
        public string FacilityName { get; set; }
        public string FacilityColorScheme { get; set; }
        public string Location { get; set; }
        public string StaffIDs { get; set; }
        public string TransportationFilterIDs { get; set; }
        public List<int> TransportationFilterID
        {
            get
            {
                return !string.IsNullOrEmpty(TransportationFilterIDs)
                           ? TransportationFilterIDs.Split(',').Select(int.Parse).ToList()
                           : new List<int>();
            }
        }
        public string TransportationFilterNames { get; set; }
        public string StaffNames { get; set; }
        public List<string> TransportationFilterName { get { return !string.IsNullOrEmpty(TransportationFilterNames) ? TransportationFilterNames.Split(',').ToList() : new List<string>(); } }
    }


    public class AssignedClientListForTransportationAssignment : TransportationGroupDetail
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public string ParentName { get; set; }
        public string Phone { get; set; }
        public string Age { get; set; }
        public string ScheduleStatusName { get; set; }
        public string GroupLocation { get; set; }
        public string GroupFacilityName { get; set; }
        public long TransportationGroupClientID { get; set; }
        public bool IsReferralDeleted { get; set; }
    }


    public class TransportationGroupList
    {
        public object TransportationGroupID { get; set; }
        public TransportationGroupDetail TransportationGroup { get; set; }
        public List<AssignedClientListForTransportationAssignment> ClientList { get; set; }
    }
}
