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
    public interface IServiceCodeDataProvider
    {
        #region Add Service Code
        ServiceResponse SetServiceCode(long serviceCodeId);
        ServiceResponse AddServiceCode(AddServiceCodeModel addServiceCodeModel, long loggedInUserId);
        #endregion

        #region Service Code List
        ServiceResponse SetServiceCodeList();
        ServiceResponse GetServiceCodeList(SearchServiceCodeListPage searchServiceCodeListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        #endregion

        #region Delete Service Code
        ServiceResponse DeleteServiceCode(SearchServiceCodeListPage searchServiceCodeListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId);
        #endregion

        #region In Home Care Code

        ServiceResponse HC_SetServiceCode(long serviceCodeId);
        ServiceResponse HC_AddServiceCode(AddServiceCodeModel addServiceCodeModel, long loggedInUserId);

        ServiceResponse HC_SetServiceCodeList();
        ServiceResponse HC_GetServiceCodeList(SearchServiceCodeListPage searchServiceCodeListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);

        ServiceResponse HC_GetModifierList(SearchModifierModel searchModifierModel);
        ServiceResponse HC_GetModifierByServiceCode(long serviceCodeID);
        ServiceResponse HC_SaveModifier(Modifier modifier, long loggedInUserId);
        ServiceResponse HC_DeleteModifier(SearchModifierModel searchModifierModel);
        #endregion
    }
}
