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
    public interface IEBFormsDataProvider
    {
        ServiceResponse AddEBForms(string FormID,int IsINSUp);
        ServiceResponse AddEBForms(EBForms Forms,  int IsINSUp, long loggedInUserId);
        ServiceResponse SetEBFormsListPage();
        ServiceResponse GetEBFormsList(SearchEBFormsListPage searchEBFormsListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection);
        ServiceResponse DeleteEBForms(SearchEBFormsListPage searchEBFormsListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId);

        List<EBForms> HC_GetEBFormsListForAutoComplete(string searchText, int pageSize);
    }
}
