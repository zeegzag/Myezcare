using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Edi277Files")]
    [PrimaryKey("Edi277FileID")]
    [Sort("Edi277FileID", "DESC")]
    public class Edi277File : BaseEntity
    {
        public long Edi277FileID { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public long PayorID { get; set; }
        public string Comment { get; set; }
        public string ReadableFilePath { get; set; }
        public int Upload277FileProcessStatus { get; set; }
        public bool IsDeleted { get; set; }


        [Ignore]
        public String StringUpload277FileProcessStatus
        {
            get { return Common.GetEnumDisplayValue((EnumUpload835FileProcessStatus)Upload277FileProcessStatus); }
        }

        [Ignore]
        public String SignedFilePath
        {
            get
            {
                AmazonFileUpload az = new AmazonFileUpload();
                return az.GetPreSignedUrl(ConfigSettings.ZarephathBucket, FilePath);
            }
        }


    }

    public enum Edi277FileType
    {
        FileType_277 = 1
    }

    public enum EnumUpload277FileProcessStatus
    {
        [Display(ResourceType = typeof(Resource), Name = "UnProcessed")]
        UnProcessed = 1,
        [Display(ResourceType = typeof(Resource), Name = "InProcess")]
        InProcess = 2,
        [Display(ResourceType = typeof(Resource), Name = "Processed")]
        Processed = 3,
        [Display(ResourceType = typeof(Resource), Name = "ErrorInProcess")]
        ErrorInProcess = 4,
        [Display(ResourceType = typeof(Resource), Name = "Running")]
        Running= 5,
    }
}
