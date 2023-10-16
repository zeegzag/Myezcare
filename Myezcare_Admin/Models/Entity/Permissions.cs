using Myezcare_Admin.Infrastructure.Attributes;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Myezcare_Admin.Resources;
using Myezcare_Admin.Infrastructure;

namespace Myezcare_Admin.Models.Entity
{
    [TableName("Permissions")]
    [PrimaryKey("PermissionID")]
    [Sort("PermissionID", "DESC")]
    public class Permissions
    {
        public long PermissionID { get; set; }

       // [Required(ErrorMessageResourceName = "PermissionNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string PermissionName { get; set; }

       // [Required(ErrorMessageResourceName = "DescriptionRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Description { get; set; }

      //  [Required(ErrorMessageResourceName = "ParentIDRequired", ErrorMessageResourceType = typeof(Resource))]
        public int ParentID { get; set; }
        public int OrderID { get; set; }
        public string PermissionCode { get; set; }
        public string PermissionPlatform { get; set; }
        public bool IsDeleted { get; set; }
    }
}