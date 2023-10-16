using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressiveAnnotations.Attributes;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReleaseNotes")]
    [PrimaryKey("ReleaseNoteID")]
    public class ReleaseNote : BaseEntity
    {
        public long ReleaseNoteID { get; set; }
        [Required(ErrorMessageResourceName = "TitleRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Title { get; set; }
        [Required(ErrorMessageResourceName = "DescriptionRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Description { get; set; }
        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? EndDate { get; set; }

        public bool IsDeleted { get; set; }

        [Ignore]
        public string EncryptedReleaseNoteID { get { return Crypto.Encrypt(Convert.ToString(ReleaseNoteID)); } }

        [Ignore]
        public string DescriptionWithoutHtml { get { return Common.StripHTML(Description); } }

        [Ignore]
        public DateTime PostedDate { get { return Common.ConvertDateToOrgTimeZone(StartDate); } }

    }
}
