using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;

namespace HomeCareApi.Models.Entity
{
    [TableName("ReleaseNotes")]
    [PrimaryKey("ReleaseNoteID")]
    public class ReleaseNote
    {
        public long ReleaseNoteID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}