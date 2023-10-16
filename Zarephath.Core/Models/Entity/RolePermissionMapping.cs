using System;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("RolePermissionMapping")]
    [PrimaryKey("RolePermissionMappingID")]
    [Sort("RolePermissionMappingID", "DESC")]
    public class RolePermissionMapping
    {
        public long RolePermissionMappingID { get; set; }
        public long RoleID { get; set; }
        public long PermissionID { get; set; }

        [SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.CurrentTime)]
        public DateTime CreatedDate { get; set; }

        [SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.LoggedInUserId)]
        public long CreatedBy { get; set; }

        [SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.CurrentTime)]
        [SetValueOnUpdate((int)SetValueOnAddAttribute.SetValueEnum.CurrentTime)]
        public DateTime UpdatedDate { get; set; }

        [SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.LoggedInUserId)]
        [SetValueOnUpdate((int)SetValueOnAddAttribute.SetValueEnum.LoggedInUserId)]
        public long UpdatedBy { get; set; }

        [SetIpAttribute]
        public string SystemID { get; set; }
        
        public bool IsDeleted { get; set; }
    }
}
