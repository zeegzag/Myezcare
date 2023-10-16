using HomeCareApi.Infrastructure.Attributes;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeCareApi.Models.Entity
{
    [TableName("ReferralDocuments")]
    [PrimaryKey("ReferralDocumentID")]
    [Sort("ReferralDocumentID", "DESC")]
    public class ReferralDocument
    {
        public long ReferralDocumentID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string KindOfDocument { get; set; }
        public int DocumentTypeID { get; set; }
        public long UserID { get; set; }
        public int UserType { get; set; }
        public long ComplianceID { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
        public string SystemID { get; set; }
    }
}