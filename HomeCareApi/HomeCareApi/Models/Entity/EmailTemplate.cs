using System.ComponentModel.DataAnnotations;
using PetaPoco;
using HomeCareApi.Infrastructure.Attributes;
using System;

namespace Zarephath.Core.Models.Entity
{
    [TableName("EmailTemplates")]
    [PrimaryKey("EmailTemplateID")]
    [Sort("EmailTemplateID", "DESC")]
    public class EmailTemplate
    {
        public long EmailTemplateID { get; set; }
        
        public string EmailTemplateName { get; set; }
        
        public string EmailTemplateSubject { get; set; }
        
        public string EmailTemplateBody { get; set; }

        public long EmailTemplateTypeID { get; set; }

        public bool IsDeleted { get; set; }
        public string Token { get; set; }

        public int OrderNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
        public string SystemID { get; set; }

    }

    public enum EnumEmailType
    {
        MissingDocumentMail = 1,
        ReceiptNotificationMail = 2,
        Schedule_Notification_SMS = 3,
        Schedule_Notification = 4, //Schedule Notification Email
        Print_Notices = 5, //Encounter Print
        Schedule_Notification_Notice = 6,
        Missing_Expire_Document_Template = 7,
        NonMissing_Expire_Document_Template = 8,
        General_Notice = 9,
        Attendance_Notification = 10,
        None_Attendance_Notification = 11,
        Service_Plan = 12,
        Monthly_Summary_Email = 13,
        DTR_Print = 14,
        Notification_Of_Inactivate_Status = 15,
        Schedule_Reminder_Email = 16,
        Schedule_Reminder_SMS = 17,
        Notification_Of_ReferralAccepted_Status = 18,
        HomeCare_Schedule_Notification_SMS = 19,
        HomeCare_Schedule_Notification = 20,
        HomeCare_Schedule_Registration_Notification = 21,
        ForgotPassword_Email = 22,
        UnlockAccount_Email = 23,
        EmpRegistration_SMS = 24,
        SurveyAlert_SMS=25,
        Alert_SMS_Notification=26
    }
}
