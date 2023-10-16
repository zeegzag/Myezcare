using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IDxCodeDataProvider
    {
        #region DX Code List

        ServiceResponse SetAddDxCodeListPage();
        ServiceResponse GetDxCodeList(SearchDxCodeListPage searchDxCodeListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteDxCode(SearchDxCodeListPage searchDxCodeListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId);

        #endregion DX Code List

        #region Add DX Code

        ServiceResponse SetAddDxCodePage(long dxCodeId);
        ServiceResponse AddDxCode(AddDxCodeModel addDxCodeModel, long loggedInId);

        #endregion Add DX Code

        #region HomeCare Related Code

        #region DX Code List

        ServiceResponse HC_SetAddDxCodeListPage();
        ServiceResponse HC_GetDxCodeList(SearchDxCodeListPage searchDxCodeListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeleteDxCode(SearchDxCodeListPage searchDxCodeListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId);

        #endregion DX Code List

        #region Add DX Code

        ServiceResponse HC_SetAddDxCodePage(long dxCodeId);
        ServiceResponse HC_AddDxCode(AddDxCodeModel addDxCodeModel, long loggedInId);

        #endregion Add DX Code

        #endregion
    }
}
