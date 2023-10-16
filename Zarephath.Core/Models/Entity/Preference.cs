using PetaPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Preferences")]
    [PrimaryKey("PreferenceID")]
    [Sort("PreferenceID", "DESC")]
    public class Preference : BaseEntity
    {
        public long PreferenceID { get; set; }
        [Required(ErrorMessageResourceName = "PreferenceNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string PreferenceName { get; set; }

        [Required(ErrorMessageResourceName = "PreferenceTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string KeyType { get; set; }
        public bool IsDeleted { get; set; }


        public enum PreferenceKeyType
        {
            Preference=1,
            Skill=2

        }
    }
}
