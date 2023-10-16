using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Controllers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Batches")]
    [PrimaryKey("BatchID")]
    [Sort("BatchID", "DESC")]
    public class Batch : BaseEntity
    {
        public long BatchID { get; set; }

        [Required(ErrorMessageResourceName = "BatchTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long BatchTypeID { get; set; }

        [Required(ErrorMessageResourceName = "PayorRequired", ErrorMessageResourceType = typeof(Resource))]
        public long PayorID { get; set; }

        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime EndDate { get; set; }

        public string Comment { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsSent { get; set; }

        [SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.CurrentTime)]
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        public DateTime SentDate { get; set; }

        [Ignore]
        public string BillingProviderIDs { get; set; }

        [Ignore]
        public string ServiceCodeIDs { get; set; }
        [Ignore]
        public string PatientIds { get; set; }

        [Ignore]
        public bool CreatePatientWiseBatch { get; set; }

        

        [Ignore]
        public string ListOfIdsInCSV { get; set; }

        [Ignore]
        public int IsSentStatus { get; set; }

        [Ignore]
        public Boolean OldVoidReplacement  { get; set; }

    }

    public enum EnumBatchNoteStatus
    {
        Addded = 1,
        ValidateFailed = 2,
        ValidateSuccess = 3,
        MarkAsSent = 4,
        MarkAsUnSent = 5,
        Approved = 6,
        Declined = 7,
    }

    public enum EnumBatchType
    {
        Initial_Submission = 1,
        Denial_Re_Submission = 2,
        Adjustment_Void_Replace_Submission = 3,
        Resend = 4 // Renamed DataValidation to Resend
    }
}
