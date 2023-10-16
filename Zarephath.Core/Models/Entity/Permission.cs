using System;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Permissions")]
    [PrimaryKey("PermissionID")]
    [Sort("PermissionID", "DESC")]
    public class Permission
    {
        public long PermissionID { get; set; }

        public string PermissionName { get; set; }

        public string Description { get; set; }

        public long ParentID { get; set; }


        public int? OrderID { get; set; }
        
        
    }

}
