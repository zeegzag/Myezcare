using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Credentials")]
    [PrimaryKey("CredentialID")]
    [Sort("CredentialID", "DESC")]
    public class Credential
    {
        public string CredentialID { get; set; }
        public string CredentialName { get; set; }
    }
}
