using Myezcare_Admin.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Web;

namespace Myezcare_Admin.Helpers
{
    public class SessionHelper
    {

        public static void SetUserSession(LoginResponseModel model)
        {
            if (model != null)
            {
                LoggedInID = model.SessionValueData.AdminID;
                Email = model.SessionValueData.Email;
                FirstName = model.SessionValueData.FirstName;
                LastName = model.SessionValueData.LastName;
                IsSecurityQuestionSubmitted = model.SessionValueData.IsSecurityQuestionSubmitted;
                RoleID = model.SessionValueData.RoleID;
                UserName = model.SessionValueData.UserName;
                Permissions = model.SessionValueData.Permissions;
                EmployeeSignatureID = model.SessionValueData.EmployeeSignatureID;
                EmpCredential = model.SessionValueData.EmpCredential;
                //DomainName = model.SessionValueData.DomainName;
            }
        }

        public static long LoggedInID
        {
            get { return Convert.ToInt64(HttpContext.Current.Session["LoggedInID"]); }
            set { HttpContext.Current.Session["LoggedInID"] = value; }
        }

        public static long EmployeeSignatureID
        {
            get { return Convert.ToInt64(HttpContext.Current.Session["EmployeeSignatureID"]); }
            set { HttpContext.Current.Session["EmployeeSignatureID"] = value; }
        }

        public static long RoleID
        {
            get { return Convert.ToInt64(HttpContext.Current.Session["RoleID"]); }
            set { HttpContext.Current.Session["RoleID"] = value; }
        }

        public static bool IsSecurityQuestionSubmitted
        {
            get { return Convert.ToBoolean(HttpContext.Current.Session["IsSecurityQuestionSubmitted"]); }
            set { HttpContext.Current.Session["IsSecurityQuestionSubmitted"] = value; }
        }

        public static string UserName
        {
            get { return Convert.ToString(HttpContext.Current.Session["UserName"]); }
            set { HttpContext.Current.Session["UserName"] = value; }
        }

        public static string EmpCredential
        {
            get { return Convert.ToString(HttpContext.Current.Session["EmpCredential"]); }
            set { HttpContext.Current.Session["EmpCredential"] = value; }
        }

        public static string FirstName
        {
            get { return Convert.ToString(HttpContext.Current.Session["FirstName"]); }
            set { HttpContext.Current.Session["FirstName"] = value; }
        }

        public static string LastName
        {
            get { return Convert.ToString(HttpContext.Current.Session["LastName"]); }
            set { HttpContext.Current.Session["LastName"] = value; }
        }

        public static string Email
        {
            get { return Convert.ToString(HttpContext.Current.Session["Email"]); }
            set { HttpContext.Current.Session["Email"] = value; }
        }


        public static bool IsTimeOutHappened
        {
            get { return Convert.ToBoolean(HttpContext.Current.Session["IsTimeOutHappened"]); }
            set { HttpContext.Current.Session[" "] = value; }
        }


        public static List<PermissionIds> Permissions
        {
            get { return (List<PermissionIds>)HttpContext.Current.Session["Permissions"]; }
            set { HttpContext.Current.Session["Permissions"] = value; }
        }

        public static long ForgotPasswordUserID
        {
            get { return Convert.ToInt64(HttpContext.Current.Session["ForgotPasswordUserID"]); }
            set { HttpContext.Current.Session["ForgotPasswordUserID"] = value; }
        }


        public static string DomainName
        {
            get { return Convert.ToString(HttpContext.Current.Session["DomainName"]); }
            set { HttpContext.Current.Session["DomainName"] = value; }
        }
    }
}
