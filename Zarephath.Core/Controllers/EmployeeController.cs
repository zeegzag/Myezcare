using System;
using System.Web.Mvc;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Controllers
{
    public class EmployeeController : BaseController
    {
        private IEmployeeDataProvider _employeeDataProvider;

        #region Add Employee

        /// <summary>
        /// This is the Add Employee Page. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Employee_AddUpdate)]
        public ActionResult AddEmployee(string id)
        {

            long employeeID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.SetAddEmployeePage(employeeID);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        /// <summary>
        /// This method will add the new employee in the database.
        /// </summary>
        /// <param name="addEmployeeModel"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Employee_AddUpdate)]
        public JsonResult AddEmployee(AddEmployeeModel addEmployeeModel)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            return Json(_employeeDataProvider.AddEmployee(addEmployeeModel, SessionHelper.LoggedInID));
        }
        #endregion

        #region Employee List

        /// <summary>
        /// For Get list of employees, this action is called
        /// </summary>
        /// <returns>Returns "DepartmentDropdownModel" for fill department dropdownlist </returns>
        [CustomAuthorize(Permissions = Constants.Permission_Employee_List)]
        public ActionResult EmployeeList()
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.SetListEmployeePage().Data;
            return View("employeeList", response);
        }

        /// <summary>
        /// for get list of employee, this action will be called.
        /// </summary>
        /// <param name="searchEmployeeModel">SearchEmployeeModel is for search filter.</param>
        /// <param name="pageIndex">pageIndex is a page no. for Get data.</param>
        /// <param name="pageSize">pageSize is size for get given pagesize of data for current page</param>
        /// <param name="sortIndex">SortType is for "ASC,DESC". This will changed when clicked on list table header(th tag) for ascending descending order.</param>
        /// <param name="sortDirection">sortDirection is for which column want to ascending or descinding.</param>
        /// <returns>Returns list of employee list if found  else return perticular message.</returns>
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Employee_List)]
        public JsonResult GetEmployeeList(SearchEmployeeModel searchEmployeeModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.GetEmployeeList(searchEmployeeModel, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        /// <summary>
        /// On Delete/DeleteAll employee, this action will be called.
        /// </summary>
        /// <param name="searchEmployeeModel">SearchEmployeeModel is for search filter.</param>
        /// <param name="pageIndex">pageIndex is a page no. for Get data.</param>
        /// <param name="pageSize">pageSize is size for get given pagesize of data for current page</param>
        /// <param name="sortIndex">SortType is for "ASC,DESC". This will changed when clicked on list table header(th tag) for ascending descending order.</param>
        /// <param name="sortDirection">sortDirection is for which column want to ascending or descinding.</param>
        /// <returns>Returns list of employee list with delete success message.</returns>
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Employee_Delete)]
        public JsonResult DeleteEmployee(SearchEmployeeModel searchEmployeeModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.DeleteEmployee(searchEmployeeModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion
       

    }
}
