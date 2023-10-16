using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("EmployeeCredentials")]
    [PrimaryKey("CredentialID")]
    [Sort("CredentialID", "DESC")]
    public class EmployeeCredential
    {
        public string CredentialID { get; set; }
        public string CredentialName { get; set; }
    }


    public enum EmployeeCredentialEnum
        {
            BHP = 1,
            BHPP,
            BHT,
        }
}
