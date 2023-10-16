using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IEmailTemplateDataProvider
    {

        #region Add Department

        ServiceResponse AddEmailTemplate(AddEmailTemplateModel addEmailTemplateModel, long loggedInUserID);
        ServiceResponse SetAddEmailTemplatePage(long departmentId);
        #endregion

        #region Department List

        ServiceResponse SetEmailTemplateListPage();
        ServiceResponse GetEmailTemplateList(SearchEmailTemplateListPage searchEmailTemplateListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteEmailTemplate(SearchEmailTemplateListPage searchEmailTemplateListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        List<EmailType> GetEmailType();
        List<ModuleName> GetModuleNames();
        ServiceResponse GetTokenList(string module);
        ServiceResponse DeleteTemplate(long id);
        ServiceResponse GetTemplateBody(long id);



        #endregion

    }
}
