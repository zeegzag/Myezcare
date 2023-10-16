using System;
using PetaPoco;
using Myezcare_Admin.Infrastructure.Attributes;

namespace Myezcare_Admin.Models.Entity
{
    [TableName("EncryptedMailMessageTokens")]
    [PrimaryKey("EncryptedMailID")]
    [Sort("EncryptedMailID", "DESC")]
    public class EncryptedMailMessageToken
    {
        public long EncryptedMailID { get; set; }
        public long EncryptedValue { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpireDateTime { get; set; }
    }
}
