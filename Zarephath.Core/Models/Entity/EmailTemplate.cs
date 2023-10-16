using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("EmailTemplates")]
    [PrimaryKey("EmailTemplateID")]
    [Sort("EmailTemplateID", "DESC")]
    public class EmailTemplate : BaseEntity
    {
        public long EmailTemplateID { get; set; }

        [Required(ErrorMessageResourceName = "EmailTemplateNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string EmailTemplateName { get; set; }
        [Required(ErrorMessageResourceName = "EmailTemplateSubjectRequired", ErrorMessageResourceType = typeof(Resource))]
        public string EmailTemplateSubject { get; set; }

        [Required(ErrorMessageResourceName = "EmailTemplateBodyRequired", ErrorMessageResourceType = typeof(Resource))]
        public string EmailTemplateBody { get; set; }

        public long EmailTemplateTypeID { get; set; }
        public bool IsDeleted { get; set; }
        public string Token { get; set; }
        public bool IsEdit { get; set; }
        public bool IsHide { get; set; }
        public int OrderNumber { get; set; }
        public string[] GetTokens { get; set; }
        public List<EmailType> EmailType { get; set; }
        public List<ModuleName> ModuleNames { get; set; }
        public bool IsEditUser { get; set; }

        public string Email { get; set; }
        public string Module { get; set; }


    }

    public class EmailType
    {
        public string Title { get; set; }
        public string Value { get; set; }
    }

    public class ModuleName
    {
        public string Title { get; set; }
        public string Value { get; set; }
    }

    public class TokenList
    {
        public string TokenID { get; set; }
        public string Tokens { get; set; }
    }

    public class EmailBody
    {
        public string EmailTemplateBody { get; set; }

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
        HomeCare_MonthlyChecklist_Notification_SMS = 27,
        HomeCare_Configured_Notification_Email = 28,
        HomeCare_Configured_Notification_SMS = 29,
        HomeCare_EmailPayment_Notification = 30,
        HomeCare_ScheduleVisit_SMS = 31
    }
}
