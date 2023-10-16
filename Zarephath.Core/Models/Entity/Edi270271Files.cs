using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Edi270271Files")]
    [PrimaryKey("Edi270271FileID")]
    [Sort("Edi270271FileID", "DESC")]
    public class Edi270271File : BaseEntity
    {
        public long Edi270271FileID { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public string Comment { get; set; }
        public string ReadableFilePath { get; set; }
        public string PayorIDs { get; set; }
        public string ServiceIDs { get; set; }
        [Required(ErrorMessageResourceName = "ReferralStatusRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ReferralStatusIDs { get; set; }
        public string Name { get; set; }
        public int Upload271FileProcessStatus { get; set; }
        public DateTime? EligibilityCheckDate { get; set; }
        public bool IsDeleted { get; set; }

    }

    public enum Edi270271FileType
    {
        FileType_270 = 1,
        FileType_271 = 2
    }

    public enum EnumUpload271FileProcessStatus
    {
        [Display(ResourceType = typeof(Resource), Name = "UnProcessed")]
        UnProcessed = 1,
        [Display(ResourceType = typeof(Resource), Name = "InProcess")]
        InProcess = 2,
        [Display(ResourceType = typeof(Resource), Name = "Processed")]
        Processed = 3,
        [Display(ResourceType = typeof(Resource), Name = "ErrorInProcess")]
        ErrorInProcess = 4,
    }
}
