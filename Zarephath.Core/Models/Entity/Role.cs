using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Roles")]
    [PrimaryKey("RoleID")]
    [Sort("RoleID", "DESC")]
    public class Role : BaseEntity
    {
        public long RoleID { get; set; }

        [Required(ErrorMessageResourceName = "RoleNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string RoleName { get; set; }

        public string Description { get; set; }


        public enum RoleEnum {
            Admin = 1,
            Nurse=18
        }
    }

}




