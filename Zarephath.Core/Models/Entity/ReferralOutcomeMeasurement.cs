using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralOutcomeMeasurements")]
    [PrimaryKey("ReferralOutcomeMeasurementID")]
    [Sort("ReferralOutcomeMeasurementID", "DESC")]
    public class ReferralOutcomeMeasurement : BaseEntity
    {
        public ReferralOutcomeMeasurement()
        {
            OutcomeMeasurementDate = DateTime.Now.Date;
        }
        public long ReferralOutcomeMeasurementID { get; set; }
        public long ReferralID { get; set; }
        public int? WorkCommunity { get; set; }
        public int? ScheduledAppointment { get; set; }
        public int? AskForHelp { get; set; }
        public int? CommunicateEffectively { get; set; }
        public int? PositiveBehavior { get; set; }
        public int? QualityFriendship { get; set; }
        public int? RespectOther { get; set; }
        public int? StickUp { get; set; }
        public int? LifeSkillGoals { get; set; }
        public int? StayOutOfTrouble { get; set; }
        public string StayOutOfTroubleText { get; set; }
        public int? SuccessfulInSchool { get; set; }
        public string SuccessfulInSchoolText { get; set; }
        public int? SuccessfulInLiving { get; set; }
        public string SuccessfulInLivingText { get; set; }
        public int? AdulthoodNeeds { get; set; }
        public string AdulthoodNeedText { get; set; }

        public bool IsDeleted  { get; set; }

        [Required(ErrorMessageResourceName = "OutcomeMeasurementDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime OutcomeMeasurementDate { get; set; }


        [ResultColumn]
        public string Signature { get; set; }

        [ResultColumn]
        public string CompletedBy { get; set; }

        public List<List<string>> GetDataPoints()
        {
            List<List<string>> list = new List<List<string>>
                {
                    new List<string>{Resource.ROM1,WorkCommunity==null?"0": WorkCommunity.ToString() },
                    new List<string>{Resource.ROM2,ScheduledAppointment==null?"0":ScheduledAppointment.ToString() },
                    new List<string>{Resource.ROM3,AskForHelp==null?"0":AskForHelp.ToString() },
                    new List<string>{Resource.ROM4,CommunicateEffectively==null?"0":CommunicateEffectively.ToString() },
                    new List<string>{Resource.ROM5,PositiveBehavior==null?"0":PositiveBehavior.ToString() },
                    new List<string>{Resource.ROM6,QualityFriendship==null?"0":QualityFriendship.ToString() },
                    new List<string>{Resource.ROM7,RespectOther==null?"0":RespectOther.ToString() },
                    new List<string>{Resource.ROM8,StickUp==null?"0":StickUp.ToString() },
                    new List<string>{Resource.ROM9,LifeSkillGoals==null?"0":LifeSkillGoals.ToString() },
                    new List<string>{Resource.ROM10,StayOutOfTrouble==null?"0":StayOutOfTrouble.ToString() },
                    new List<string>{Resource.ROM11,SuccessfulInSchool==null?"0":SuccessfulInSchool.ToString() },
                    new List<string>{Resource.ROM12,SuccessfulInLiving==null?"0":SuccessfulInLiving.ToString() },
                    new List<string>{Resource.ROM13,AdulthoodNeeds==null?"0":AdulthoodNeeds.ToString() }
                };
            return list;
        }

    }
}