//using Amazon.Auth.AccessControlPolicy;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;

using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("CaptureCall")]
    [PrimaryKey("Id")]
    [Sort("Id", "DESC")]
    public class CaptureCall : BaseEntity
    {
        public long Id { get; set; }

        [Required(ErrorMessageResourceName = "FirstNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "LastNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string LastName { get; set; }

        //[Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        //[RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidContactNumber", ErrorMessageResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceName = "ContactRequired", ErrorMessageResourceType = typeof(Resource))]
        [Required]
        public string Contact { get; set; }

        //[Required(ErrorMessageResourceName = "AddressRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Address { get; set; }

        //[Required(ErrorMessageResourceName = "CityRequired", ErrorMessageResourceType = typeof(Resource))]
        public string City { get; set; }

        //[Required(ErrorMessageResourceName = "StateRequired", ErrorMessageResourceType = typeof(Resource))]
        public string StateCode { get; set; }

        //[Required(ErrorMessageResourceName = "ZipCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ZipCode { get; set; }

        public bool IsDeleted { get; set; }
        [Required]
        public string Notes { get; set; }
        //[Required(ErrorMessageResourceName = "CallTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string CallType { get; set; }
        public string RelatedWithPatient { get; set; }
        [Required]
        public DateTime? InquiryDate { get; set; }
        public string RoleIds { get; set; }
        public string EmployeesIDs { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string OrbeonID { get; set; }
        public string Status { get; set; }
        public string CreatedByName { get; set; }
        public string GroupIDs { get; set; }

    }

    public class CaptureCalls 
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Contact { get; set; }

        //[Required(ErrorMessageResourceName = "AddressRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Address { get; set; }

        //[Required(ErrorMessageResourceName = "CityRequired", ErrorMessageResourceType = typeof(Resource))]
        public string City { get; set; }

        //[Required(ErrorMessageResourceName = "StateRequired", ErrorMessageResourceType = typeof(Resource))]
        public string StateCode { get; set; }

        //[Required(ErrorMessageResourceName = "ZipCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ZipCode { get; set; }

        public bool IsDeleted { get; set; }
        public string Notes { get; set; }
        public string RoleIds { get; set;}
        public string EmployeesIDs { get; set;}
        public string CallType { get; set; }
        public string RelatedWithPatient { get; set; }
        public DateTime? InquiryDate { get; set; }
        public string OrbeonID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Status { get; set; }
        public string GroupIDs { get; set; }

    }
    public class ConvertToReferralModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ReferralID { get; set; }
        public string EncryptedReferralID { get { if (ReferralID != 0) { return Crypto.Encrypt(ReferralID.ToString()); } return null; } }
        public long Contact { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string OrbeonID { get; set; }
        public string Status { get; set; }
        public string GroupIDs { get; set; }
    }

}
