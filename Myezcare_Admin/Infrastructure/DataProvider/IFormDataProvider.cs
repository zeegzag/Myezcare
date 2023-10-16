using Myezcare_Admin.Models;
using Myezcare_Admin.Models.ViewModel;

namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public interface IFormDataProvider
    {
        #region Form List

        ServiceResponse SetFormListPage();
        ServiceResponse GetFormList(SearchFormModel model, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteForm(SearchFormModel model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse UpdateFormPrice(FormListModel model, long loggedInUserID);

        #endregion


        ServiceResponse UpdateEbriggsFormDetails();
    }
}
