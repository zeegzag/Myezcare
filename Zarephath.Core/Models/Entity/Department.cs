using System;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Departments")]
    [PrimaryKey("DepartmentID")]
    [Sort("DepartmentID", "DESC")]
    public class Department : BaseEntity
    {
        public long DepartmentID { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "DepartmentNameRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "DepartmentNameLength", ErrorMessageResourceType = typeof(Resource))]
        public string DepartmentName { get; set; }

        //[Required(ErrorMessageResourceName = "LocationRequired", ErrorMessageResourceType = typeof(Resource))]
        [RequiredIf("DepartmentName == '123'", ErrorMessageResourceName = "LocationRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "LocationLength", ErrorMessageResourceType = typeof(Resource))]
        public string Location { get; set; }

        //[Required(ErrorMessageResourceName = "AddressRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "AddressLength", ErrorMessageResourceType = typeof(Resource))]
        public string Address { get; set; }

        //[Required(ErrorMessageResourceName = "CityRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "CityLength", ErrorMessageResourceType = typeof(Resource))]
        public string City { get; set; }

        //[Display(Name = "State", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "StateRequired", ErrorMessageResourceType = typeof(Resource))]
        public string StateCode { get; set; }

        //[Display(Name = "ZipCode", ResourceType = typeof(Resource))]
        public string ZipCode { get; set; }

        public bool IsDeleted { get; set; }
    }
}
