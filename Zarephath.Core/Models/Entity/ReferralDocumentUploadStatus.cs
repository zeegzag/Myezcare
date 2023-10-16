using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralDocumentUploadStatuses")]
    [PrimaryKey("ReferralDocumentUploadStatusID")]
    [Sort("ReferralDocumentUploadStatusID", "DESC")]
    public class ReferralDocumentUploadStatus
    {
        public long ReferralDocumentUploadStatusID { get; set; }
        public long ReferralID { get; set; }
        public string AHCCCSID { get; set; }
        public bool UploadStatus { get; set; }
    }
}
