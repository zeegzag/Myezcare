using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Contacts")]
    [PrimaryKey("ContactID")]
    [Sort("ContactID", "DESC")]
    public class Contact : BaseEntity
    {
        public long ContactID { get; set; }

        [Required(ErrorMessageResourceName = "FirstNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "LastNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string LastName { get; set; }

        [Ignore]
        public string FullName { get { return Common.GetGenericNameFormat(FirstName,"", LastName); } }
        //public string FullName { get; set; }

        [RegularExpression(Constants.RegxEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        public string ApartmentNo { get; set; }
        [Required(ErrorMessageResourceName = "AddressRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Address { get; set; }

        //[Required(ErrorMessageResourceName = "CityRequired", ErrorMessageResourceType = typeof(Resource))]
        public string City { get; set; }

        [Required(ErrorMessageResourceName = "StateRequired", ErrorMessageResourceType = typeof(Resource))]
        public string State { get; set; }

        // [Required(ErrorMessageResourceName = "ZipCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ZipCode { get; set; }

        [Required(ErrorMessageResourceName = "PhoneRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        [Required(ErrorMessageResourceName = "LanguagePreferenceRequired", ErrorMessageResourceType = typeof(Resource))]
        public long LanguageID { get; set; }

        public bool IsDeleted { get; set; }


        [Required(ErrorMessageResourceName = "LatitudeRequired", ErrorMessageResourceType = typeof(Resource))]
        public double? Latitude { get; set; }
        [Required(ErrorMessageResourceName = "LongitudeRequired", ErrorMessageResourceType = typeof(Resource))]
        public double? Longitude { get; set; }


        [Ignore]
        [IncludeInAuditAttribute]
        public string ContactType { get; set; }

        public string ReferenceMasterID { get; set; }

    }
}
