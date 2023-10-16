using System;
using System.ComponentModel.DataAnnotations;
using Myezcare_Admin.Infrastructure;
using Myezcare_Admin.Resources;
using PetaPoco;

namespace Myezcare_Admin.Models.Entity
{
    [TableName("ReleaseNotes")]
    [PrimaryKey("ReleaseNoteID")]
    public class ReleaseNote : BaseEntity
    {
        public long ReleaseNoteID { get; set;}
        [Required(ErrorMessageResourceName = "TitleRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Title { get; set;}
        [Required(ErrorMessageResourceName = "DescriptionRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Description { get; set;}
        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? EndDate { get; set;}

        public bool IsDeleted { get; set;}
    }
}
