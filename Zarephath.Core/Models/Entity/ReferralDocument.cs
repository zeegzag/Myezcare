using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralDocuments")]
    [PrimaryKey("ReferralDocumentID")]
    [Sort("ReferralDocumentID", "DESC")]
    public class ReferralDocument : BaseEntity
    {
        public long ReferralDocumentID { get; set; }
        [Required(ErrorMessageResourceName = "FileNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string FileName { get; set; }
        public string FilePath { get; set; }

        [Ignore]
        public string AWSSignedFilePath
        {
            get
            {
                AmazonFileUpload az = new AmazonFileUpload();
                return az.GetPreSignedUrl(ConfigSettings.ZarephathBucket, FilePath);
            }
        }

        [Required(ErrorMessageResourceName = "DocumentKindRequired", ErrorMessageResourceType = typeof(Resource))]
        public string KindOfDocument { get; set; }

        [Required(ErrorMessageResourceName = "DocumentTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int DocumentTypeID { get; set; }
        public long UserID { get; set; }
        public int UserType { get; set; }
        //public long ReferralID { get; set; }
        public long ComplianceID { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public enum UserTypes
        {
            Referral = 1, Employee
        }
    }
}
