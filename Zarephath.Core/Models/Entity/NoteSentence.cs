using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("NoteSentences")]
    [PrimaryKey("NoteSentenceID")]
    [Sort("NoteSentenceID", "DESC")]
    public class NoteSentence:BaseEntity
    {
        public long NoteSentenceID { get; set; }

        [Required(ErrorMessageResourceName = "NoteSentenceTitleRequired", ErrorMessageResourceType = typeof(Resource))]
        public string NoteSentenceTitle { get; set; }

        [Required(ErrorMessageResourceName = "NoteSentenceDetailsRequired", ErrorMessageResourceType = typeof(Resource))]
        public string NoteSentenceDetails { get; set; }

        public bool IsDeleted { get; set; }

        [ResultColumn]
        public int Count { get; set; }

        [ResultColumn]
        public string EncryptedNoteSentenceID { get { return Crypto.Encrypt(Convert.ToString(NoteSentenceID)); } }
        

    }



}
