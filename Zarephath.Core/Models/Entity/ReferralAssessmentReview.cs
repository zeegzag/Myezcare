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
    [TableName("ReferralAssessmentReview")]
    [PrimaryKey("ReferralAssessmentID")]
    [Sort("ReferralAssessmentID", "DESC")]
    public class ReferralAssessmentReview : BaseEntity
    {
        public ReferralAssessmentReview()
        {
            AssessmentDate = DateTime.Now.Date;
        }
        public long ReferralAssessmentID { get; set; }
        public long ReferralID { get; set; }
        public float Permanency { get; set; }
        public float DailyLiving { get; set; }
        public float SelfCare { get; set; }
        public float RelationshipsAndCommunication { get; set; }
        public float HousingAndMoneyManagement { get; set; }
        public float WorkAndStudyLife { get; set; }
        public float CareerAndEducationPlanning { get; set; }
        public float LookingForward { get; set; }
        public string AssessmentReview { get; set; }

        [Required(ErrorMessageResourceName = "AssessmentDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime AssessmentDate { get; set; }

        [Required(ErrorMessageResourceName = "AssigneeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long Assignee { get; set; }


        public bool MarkAsComplete { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? SignatureDate { get; set; }

        public long? SignatureBy { get; set; }

        public long? AssignedBy { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? AssignedDate { get; set; }

        public string FileName { get; set; }
        public string FilePath { get; set; }


        public bool IsDeleted  { get; set; }

        [Ignore]
        public string AWSSignedFilePath
        {
            get
            {
                if (!string.IsNullOrEmpty(FilePath))
                {
                    AmazonFileUpload az = new AmazonFileUpload();
                    return az.GetPreSignedUrl(ConfigSettings.ZarephathBucket, FilePath);
                }
                return "";
            }
        }

        [Ignore]
        public string TempAssessmentResultUrl { get; set; }
        
        


        [ResultColumn]
        public string Signature { get; set; }

        [ResultColumn]
        public string CompletedBy { get; set; }


        public List<List<string>> GetDataPoints()
        {
            List<List<string>> list = new List<List<string>>
                {
                    new List<string>{Resources.Resource.Permanency,Permanency.ToString() },
                    new List<string>{Resources.Resource.DailyLiving,DailyLiving.ToString() },
                    new List<string>{Resources.Resource.SelfCare,SelfCare.ToString() },
                    new List<string>{Resources.Resource.RelationshipsAndCommunication,RelationshipsAndCommunication.ToString() },
                    new List<string>{Resources.Resource.HousingAndMoneyManagement,HousingAndMoneyManagement.ToString() },
                    new List<string>{Resources.Resource.WorkAndStudyLife,WorkAndStudyLife.ToString() },
                    new List<string>{Resources.Resource.CareerAndEducationPlanning,CareerAndEducationPlanning.ToString() },
                    new List<string>{Resources.Resource.LookingForward,LookingForward.ToString() }
                };
            return list;
        }

        [Ignore]
        public string EncryptedReferralAssessmentID { get { return Crypto.Encrypt(Convert.ToString(ReferralAssessmentID)); } }
        

    }
}
