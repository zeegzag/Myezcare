using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class AddEmailTemplateModel
    {
        public AddEmailTemplateModel()
        {
            EmailTemplate = new EmailTemplate();
        }
        public EmailTemplate EmailTemplate { get; set; }
        [Ignore]
        public bool IsEditMode { get; set; }


    }

    public class SetEmailTemplateListPage
    {
        public SetEmailTemplateListPage()
        {
            SearchEmailTemplateListPage = new SearchEmailTemplateListPage();
            DeleteFilter = new List<NameValueData>();
        }
        public SearchEmailTemplateListPage SearchEmailTemplateListPage { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
    }

    public class SearchEmailTemplateListPage
    {
        public long EmailTemplateID { get; set; }
        public string EmailTemplateName { get; set; }
        public string EmailTemplateSubject { get; set; }
        public string Tokens { get; set; }

        public long? EmailTemplateTypeID { get; set; }
        public string AddedBy { get; set; }
        public int IsDeleted { get; set; }
        public string ListOfIdsInCSV { get; set; }
        public string Module { get; set; }
        public string EmailType { get; set; }

    }

    public class ListEmailTemplateModel
    {
        public long EmailTemplateID { get; set; }
        public string EmailTemplateName { get; set; }
        public string EmailTemplateSubject { get; set; }
        public string EmailTemplateBody { get; set; }

        public long EmailTemplateTypeID { get; set; }

        public string AddedBy { get; set; }
        public string Token { get; set; }

        public bool IsPdfTemplate
        {
            get
            {
                return EmailTemplateName.Contains(Resource.PleaseDoNotModify) || EmailTemplateSubject.Contains(Resource.PleaseDoNotModify);
            }
        }

        public DateTime CreatedDate { get; set; }
        public string EncryptedEmailTemplateID { get { return Crypto.Encrypt(Convert.ToString(EmailTemplateID)); } }

        public int Count { get; set; }
        public int EmpCount { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEdit { get; set; }
        public bool IsHide { get; set; }
        public string Module { get; set; }
        public string EmailType { get; set; }
    }
}
