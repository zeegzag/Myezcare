using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Controllers
{
    public class DepartmentController : BaseController
    {
        private IDepartmentDataProvider _departmentDataProvider;

        #region Add Department
        /// <summary>
        /// Common for add/edit
        /// On create new or Edit department, this action is called.
        /// </summary>
        /// <returns>
        /// For AddDepartment, Returns new Department() as a blank for create a blank model in angularjs.
        /// For EditDepartment, Returns Department Detail.
        /// </returns>
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Department_AddUpdate)]
        public ActionResult AddDepartment(string id)
        {
            _departmentDataProvider = new DepartmentDataProvider();
            if (!string.IsNullOrEmpty(id))
            {
                long departmentId = Convert.ToInt64(Crypto.Decrypt(id));
                var response = _departmentDataProvider.SetAddDepartmentPage(departmentId);
                return ShowUserFriendlyPages(response) ?? View(response.Data);
            }
            return View();
        }

        /// <summary>
        /// On save new/exist department details, this action is called.
        /// </summary>
        /// <param name="department">Instance of Department</param>
        /// <returns>if new department is created successfully then return "DepartmentCreatedSuccessfully".
        ///         if existing department is updated then return "DepartmentUpdatedSuccessfully".
        /// if any issues has been occured while save/update record then return exception message with "IsSuccess=false".
        /// </returns>
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Department_AddUpdate)]
        public JsonResult AddDepartment(Department department)
        {
            _departmentDataProvider = new DepartmentDataProvider();
            var response = _departmentDataProvider.AddDepartment(department, SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion

        #region Department List

        /// <summary>
        /// For Get list of departments, this action is called
        /// </summary>
        /// <returns>Returns 
        /// => "ListDepartmentModel" for a dpeartment list.
        /// => "DepartmentDropdownModel" for fill department dropdownlist
        /// => "ManagerDropdownModel" for fill Manager dropdownlist
        /// </returns>
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Department_List)]
        public ActionResult DepartmentList()
        {
            _departmentDataProvider = new DepartmentDataProvider();
            var response = _departmentDataProvider.SetDepartmentListPage().Data;
            return View("DepartmentList", response);
        }

        /// <summary>
        /// for get list of department, this action will be called.
        /// </summary>
        /// <param name="searchDepartmentModel"></param>
        /// <param name="pageIndex">pageIndex is a page no. for Get data.</param>
        /// <param name="pageSize">pageSize is size for get given pagesize of data for current page</param>
        /// <param name="sortIndex">SortType is for "ASC,DESC". This will changed when clicked on list table header(th tag) for ascending descending order.</param>
        /// <param name="sortDirection">sortDirection is for which column want to ascending or descinding.</param>
        /// <returns>Returns list of department list if found  else return perticular message.</returns>
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Department_List)]
        public JsonResult GetDepartmentList(SearchDepartmentModel searchDepartmentModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _departmentDataProvider = new DepartmentDataProvider();
            var response = _departmentDataProvider.GetDepartmentList(searchDepartmentModel, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        /// <summary>
        /// On Delete/DeleteAll department, this action will be called.
        /// </summary>
        /// <param name="searchDepartmentModel"></param>
        /// <param name="pageIndex">pageIndex is a page no. for Get data.</param>
        /// <param name="pageSize">pageSize is size for get given pagesize of data for current page</param>
        /// <param name="sortIndex">SortType is for "ASC,DESC". This will changed when clicked on list table header(th tag) for ascending descending order.</param>
        /// <param name="sortDirection">sortDirection is for which column want to ascending or descinding.</param>
        /// <returns>Returns list of department list with delete success message.</returns>
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Department_Delete)]
        public JsonResult DeleteDepartment(SearchDepartmentModel searchDepartmentModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _departmentDataProvider = new DepartmentDataProvider();
            var response = _departmentDataProvider.DeleteDepartment(searchDepartmentModel, pageIndex, pageSize, sortIndex, sortDirection,SessionHelper.LoggedInID);
            return Json(response);
        }


        /// <summary>
        /// For Zipcode Auto complete.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="pageSize">pageSize is size for get given pagesize of data for current page</param>
        /// <param name="searchText"></param>
        /// <returns>Returns list of department list with delete success message.</returns>
        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Department_List + "," + Constants.Permission_Department_AddUpdate)]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public JsonResult GetZipCodeList(string searchText, string state, int pageSize)
        {
            _departmentDataProvider = new DepartmentDataProvider();
            return Json(_departmentDataProvider.GetZipCodeList(searchText, state, pageSize));
        }

        #endregion

    }
}
