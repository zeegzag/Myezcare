using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("EdiFileLogs")]
    [PrimaryKey("EdiFileLogID")]
    [Sort("EdiFileLogID", "DESC")]
    public class EdiFileLog : BaseEntity
    {
        public long EdiFileLogID { get; set; }
        public long EdiFileTypeID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long? BatchID { get; set; }
        public string FileSize { get; set; }
        public bool IsDeleted { get; set; }

    }

    public enum EnumEdiFileTypes
    {
        Edi835 = 1,
        Edi837 = 2,
        Edi837ValidationError = 3,
        Edi837GenerationError = 4,
    }
}
