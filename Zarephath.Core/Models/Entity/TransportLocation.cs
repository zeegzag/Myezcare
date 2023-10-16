using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("TransportLocations")]
    [PrimaryKey("TransportLocationID")]
    [Sort("TransportLocationID", "DESC")]
    public class TransportLocation : BaseEntity
    {
        public long TransportLocationID { get; set; }

        [Required(ErrorMessageResourceName = "LocationRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Location { get; set; }

        [Unique("Location Code Already Exist.")]
        [Required(ErrorMessageResourceName = "LocationCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string LocationCode { get; set; }

        public string MapImage { get; set; }

        [Required(ErrorMessageResourceName = "AddressRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "AddressLength", ErrorMessageResourceType = typeof(Resource))]
        public string Address { get; set; }

        [Required(ErrorMessageResourceName = "CityRequired", ErrorMessageResourceType = typeof(Resource))]
        public string City { get; set; }

        [Required(ErrorMessageResourceName = "StateRequired", ErrorMessageResourceType = typeof(Resource))]
        public string State { get; set; }

        [Display(Name = "ZipCode", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "ZipCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Zip { get; set; }

        [Required(ErrorMessageResourceName = "PhoneRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "PhoneInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Phone { get; set; }

        [Required(ErrorMessageResourceName = "RegionRequired", ErrorMessageResourceType = typeof(Resource))]
        public long RegionID { get; set; }

        //[Required(ErrorMessageResourceName = "MondayDropOffisrequired", ErrorMessageResourceType = typeof(Resource))]
        public string MondayDropOff { get; set; }

        //[Required(ErrorMessageResourceName = "TuesdayDropOffisrequired", ErrorMessageResourceType = typeof(Resource))]
        public string TuesdayDropOff { get; set; }

        //[Required(ErrorMessageResourceName = "WednesdayDropOffisrequired", ErrorMessageResourceType = typeof(Resource))]
        public string WednesdayDropOff { get; set; }

        //[Required(ErrorMessageResourceName = "ThursdayDropOffisrequired", ErrorMessageResourceType = typeof(Resource))]
        public string ThursdayDropOff { get; set; }

        //[Required(ErrorMessageResourceName = "FridayDropOffisrequired", ErrorMessageResourceType = typeof(Resource))]
        public string FridayDropOff { get; set; }

        //[Required(ErrorMessageResourceName = "SaturdayDropOffisrequired", ErrorMessageResourceType = typeof(Resource))]
        public string SaturdayDropOff { get; set; }

        //[Required(ErrorMessageResourceName = "SundayPickUpisrequired", ErrorMessageResourceType = typeof(Resource))]
        public string SundayDropOff { get; set; }

        //[Required(ErrorMessageResourceName = "MondayPickUPisrequired", ErrorMessageResourceType = typeof(Resource))]
        public string MondayPickUp { get; set; }

        //[Required(ErrorMessageResourceName = "TuesdayPickUpisrequired", ErrorMessageResourceType = typeof(Resource))]
        public string TuesdayPickUp { get; set; }

        //[Required(ErrorMessageResourceName = "WednesdayPickUpisrequired", ErrorMessageResourceType = typeof(Resource))]
        public string WednesdayPickUp { get; set; }

        //[Required(ErrorMessageResourceName = "ThursdayPickUpisrequired", ErrorMessageResourceType = typeof(Resource))]
        public string ThursdayPickUp { get; set; }

        //[Required(ErrorMessageResourceName = "FridayPickUpisrequired", ErrorMessageResourceType = typeof(Resource))]
        public string FridayPickUp { get; set; }

        //[Required(ErrorMessageResourceName = "SaturdayPickUpisrequired", ErrorMessageResourceType = typeof(Resource))]
        public string SaturdayPickUp { get; set; }

        //[Required(ErrorMessageResourceName = "SundayPickUpisrequired", ErrorMessageResourceType = typeof(Resource))]
        public string SundayPickUp { get; set; }

        public int IsDeleted { get; set; }
    }
}
