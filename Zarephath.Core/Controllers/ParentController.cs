using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Controllers
{
    public class ParentController : BaseController
    {
        private IParentDataProvider _parentDataProvider;

        #region Add Parent

        [CustomAuthorize(Permissions = Constants.Permission_CaseManager_AddUpdate)]
        public ActionResult AddParent(string id)
        {
            long parentId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _parentDataProvider = new ParentDataProvider();
            ServiceResponse response = _parentDataProvider.SetAddParentPage(parentId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_CaseManager_AddUpdate)]
        public JsonResult AddParent(Contact model)
        {
            _parentDataProvider = new ParentDataProvider();
            return Json(_parentDataProvider.AddParent(model, SessionHelper.LoggedInID));
        }

        #endregion

        #region Parent List
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_CaseManager_List)]
        public ActionResult ParentList()
        {
            _parentDataProvider = new ParentDataProvider();
            ServiceResponse response = _parentDataProvider.SetAddParentListPage();
            return View(response.Data);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_CaseManager_List)]
        public JsonResult GetParentList(SearchParentListPage searchParentListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _parentDataProvider=new ParentDataProvider();
            return
                Json(_parentDataProvider.GetParentList(searchParentListPage, pageIndex, pageSize, sortIndex,
                                                                 sortDirection));
        }
        
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_CaseManager_AddUpdate)]
        public JsonResult DeleteParent(SearchParentListPage searchParentListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _parentDataProvider = new ParentDataProvider();
            var response = _parentDataProvider.DeleteParent(searchParentListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }
        #endregion
    }
}
