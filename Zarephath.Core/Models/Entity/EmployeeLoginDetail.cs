using System;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("EmployeeLoginDetails")]
    [PrimaryKey("EmployeeLoginDetailID")]
    [Sort("EmployeeLoginDetailID", "DESC")]
    public class EmployeeLoginDetail : BaseEntity
    {
        public long EmployeeLoginDetailID { get; set; }

        public long EmployeeID { get; set; }

        public DateTime LoginTime { get; set; }

        public string ActionType { get; set; }
        public string ActionPlatform { get; set; }

        


        public enum LoginActionType
        {
            Login=1,
            Logout=2
        }

        public enum LoginActionPlatform
        {
            Web=1,
            Android=2,
            iOS = 2
        }

        


    }
}
