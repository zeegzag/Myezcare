using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Facilities")]
    [PrimaryKey("FacilityID")]
    [Sort("FacilityID", "DESC")]
    public class Facility : BaseEntity
    {
        public long FacilityID { get; set; }

        public string FacilityName { get; set; }
        public string FacilityBillingName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Cell { get; set; }
        public long RegionID { get; set; }
        public string County { get; set; }
        public int? GSA { get; set; }
        public int? BadCapacity { get; set; }
        public int? PrivateRoomCount { get; set; }
        public string SiteType { get; set; }
        public string ProviderType { get; set; }
        public string Licensure { get; set; }
        public DateTime LicensureRenewalDate { get; set; }
        public DateTime FirePermitDate { get; set; }
        public string NPI { get; set; }
        public string AHCCCSID { get; set; }
        public string EIN { get; set; }
        public string FacilityColorScheme { get; set; }
        public long ParentFacilityID { get; set; }
        public long? DefaultScheduleStatusID { get; set; }

        public bool IsDeleted { get; set; }

        [Ignore]
        public string SetSelectedPayors { get; set; }

        [Ignore]
        public List<string> SelectedPayors { get; set; }
    }


    [TableName("Facilities")]
    [PrimaryKey("FacilityID")]
    [Sort("FacilityID", "DESC")]
    public class HC_Facility : BaseEntity
    {
        public long FacilityID { get; set; }
        public long? AgencyID { get; set; }
        public string FacilityName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Cell { get; set; }
        public long? RegionID { get; set; }
        public string County { get; set; }
        public int? GSA { get; set; }
        public int? BadCapacity { get; set; }
        public int? PrivateRoomCount { get; set; }
        public string SiteType { get; set; }
        public string ProviderType { get; set; }
        public string Licensure { get; set; }
        public DateTime? LicensureRenewalDate { get; set; }
        public DateTime? FirePermitDate { get; set; }
        public string NPI { get; set; }
        public string AHCCCSID { get; set; }
        public string EIN { get; set; }
        public string FacilityColorScheme { get; set; }
        public bool IsDeleted { get; set; }
    }


}
