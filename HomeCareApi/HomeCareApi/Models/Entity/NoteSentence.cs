using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;

namespace HomeCareApi.Models.Entity
{
    [TableName("NoteSentences")]
    [PrimaryKey("NoteSentenceID")]
    public class NoteSentence
    {
        public long NoteSentenceID { get; set; }
        public string NoteSentenceTitle { get; set; }
        public string NoteSentenceDetails { get; set; }
        public bool IsDeleted { get; set; }
    }
}
