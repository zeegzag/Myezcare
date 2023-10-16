using PetaPoco;
using System;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Clients")]
    [PrimaryKey("ClientID")]
    [Sort("ClientID", "DESC")]
    public class Client : BaseEntity
    {
        public long ClientID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public string ClientNumber { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public bool IsDeleted { get; set; }
    }
}
