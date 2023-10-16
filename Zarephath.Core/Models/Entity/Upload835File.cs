using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Upload835Files")]
    [PrimaryKey("Upload835FileID")]
    [Sort("Upload835FileID", "DESC")]
    public class Upload835File : BaseEntity
    {
        public long Upload835FileID { get; set; }
        public long PayorID { get; set; }
        public long? BatchID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public string Comment { get; set; }
        public bool IsProcessed { get; set; }
        public int Upload835FileProcessStatus { get; set; }
        [Ignore]
        public String StringUpload835FileProcessStatus {
            get { return Common.GetEnumDisplayValue((EnumUpload835FileProcessStatus) Upload835FileProcessStatus); }
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

        [ResultColumn]
        public string Payor { get; set; }

        public string ReadableFilePath { get; set; }
        public string LogFilePath { get; set; }

        

        public string A835TemplateType { get; set; }

        public string EraID { get; set; }
        public string EraMappedBatches { get; set; }

        public bool IsDeleted { get; set; }
    }


    public enum EnumUpload835FileProcessStatus
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
        Running = 6,
    }

    public enum Enum835TemplateType
    {
        Edi_File = 1,
        Paper_Remits_EOB = 2
    }


}
