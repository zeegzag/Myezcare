using PetaPoco;
using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("OrganizationPreference")]
    [PrimaryKey("OrganizationID")]
    [Sort("OrganizationID", "DESC")]
    public class OrganizationPreference : BaseEntity
    {
        public long OrganizationID { get; set; }

        public string DateFormat { get; set; }

        public string Currency { get; set; }

        public string Region { get; set; }

        public string Language { get; set; }

        public string NameDisplayFormat { get; set; }

        public string CssFilePath { get; set; }

        public string WeekStartDay { get; set; }
    }
}

