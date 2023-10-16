using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Helpers
{
    public class SessionHelper
    {
        public static System.Web.SessionState.HttpSessionState SessionObj;
        public static HttpContext httpContextObj;


        public SessionHelper()
        {
            if (SessionObj == null)
                SessionObj = HttpContext.Current.Session;


            if (httpContextObj != null)
                httpContextObj = HttpContext.Current;
        }

        public static object GetSessionValue(string keyName)
        {

            System.Web.SessionState.HttpSessionState tempSessionObj = HttpContext.Current == null || HttpContext.Current.Session == null ? null : HttpContext.Current.Session;

            if (tempSessionObj == null)
                tempSessionObj = SessionObj;

            if (tempSessionObj != null)
                return (object)tempSessionObj[keyName];
            else
                return "";

        }


        public static void SetSessionValue(string keyName, object value)
        {

            System.Web.SessionState.HttpSessionState tempSessionObj = HttpContext.Current == null || HttpContext.Current.Session == null ? null : HttpContext.Current.Session;

            if (tempSessionObj == null)
                tempSessionObj = SessionObj;

            if (tempSessionObj != null)
                tempSessionObj[keyName] = value;
        }

        public static void SetUserSession(LoginResponseModel model)
        {
            CacheHelper_MyezCare ch_MyezcareOrg = new CacheHelper_MyezCare();
            MyEzcareOrganization myezCareOrg = ch_MyezcareOrg.GetCachedData<MyEzcareOrganization>();
            var OrgTypes = myezCareOrg.OrganizationType.Split(',');

            if (model != null)
            {
                LoggedInID = model.SessionValueData.UserID;
                Email = model.SessionValueData.Email;
                FirstName = model.SessionValueData.FirstName;
                LastName = model.SessionValueData.LastName;
                IsSecurityQuestionSubmitted = model.SessionValueData.IsSecurityQuestionSubmitted;
                RoleID = model.SessionValueData.RoleID;
                UserName = model.SessionValueData.UserName;
                Permissions = model.SessionValueData.Permissions;
                EmployeeSignatureID = model.SessionValueData.EmployeeSignatureID;
                EmpCredential = model.SessionValueData.EmpCredential;
                IsEmployeeLogin = model.SessionValueData.IsEmployeeLogin;
                //DomainName = model.SessionValueData.DomainName;
                IsHomeCare = false;
                IsDayCare = false;
                IsPrivateDutyCare = false;
                IsCaseManagement = false;
                IsRAL = false;
                IsStaffing = false;

                if (myezCareOrg == null || OrgTypes[0].ToLower() == MyEzcareOrganization.AgencyType.HomeCare.ToString().ToLower())
                {
                    IsHomeCare = true;

                }
                else if (OrgTypes[0].ToLower() == MyEzcareOrganization.AgencyType.DayCare.ToString().ToLower())
                {
                    IsDayCare = true;
                }
                else if (OrgTypes[0].ToLower() == MyEzcareOrganization.AgencyType.RAL.ToString().ToLower())
                {
                    IsRAL = true;
                }
                else if (OrgTypes[0].ToLower() == MyEzcareOrganization.AgencyType.PrivateDutyCare.ToString().ToLower())
                {
                    IsPrivateDutyCare = true;
                }
                else if (OrgTypes[0].ToLower() == MyEzcareOrganization.AgencyType.CaseManagement.ToString().ToLower())
                {
                    IsCaseManagement = true;
                }
                else if (OrgTypes[0].ToLower() == MyEzcareOrganization.AgencyType.Staffing.ToString().ToLower())
                {
                    IsStaffing = true;
                }
                if (myezCareOrg != null)
                {
                    IsCompletedWizard = myezCareOrg.IsCompletedWizard;
                    OrganizationId = myezCareOrg.OrganizationID;
                    CompanyName = myezCareOrg.CompanyName;


                    //Get data for company unpaid invoice 
                    CacheHelper cacheHelper = new CacheHelper();
                    if (cacheHelper.ShowInvoiceReadyRibbon && myezCareOrg.OrganizationID > 0)
                    {
                        IInvoiceDataProvider _iInvoiceDataProvider;
                        _iInvoiceDataProvider = new InvoiceDataProvider(Constants.MyezcareOrganizationConnectionString);
                        var invoiceList = _iInvoiceDataProvider.NinjaInvoiceList();
                        if (invoiceList.Count > 0)
                        {
                            var filterData = invoiceList;
                            invoiceList = new List<InvoiceMod>();
                            foreach (var item in filterData)
                            {
                                if (item.invoice_status_id != Convert.ToString(6))
                                {
                                    invoiceList.Add(new InvoiceMod
                                    {
                                        id = item.id,
                                        amounts = item.amount,
                                        balance = item.balance,
                                        client_id = item.client_id,
                                        invoice_status_id = item.invoice_status_id,
                                        updated_at = item.updated_at,
                                        invoice_number = item.invoice_number,
                                        invoice_date = item.invoice_date,
                                        DueDate = item.DueDate,
                                        is_deleted = item.is_deleted,
                                        invoice_type_id = item.invoice_type_id,
                                        start_date = item.start_date,
                                    });
                                }
                            }
                            CompanyInvoiceInfo = invoiceList;
                        }
                    }
                }
            }
        }

        public static long LoggedInID
        {
            get { return Convert.ToInt64(HttpContext.Current.Session["LoggedInID"]); }
            set { HttpContext.Current.Session["LoggedInID"] = value; }
        }

        public static long ReferralDocumentID
        {
            get { return Convert.ToInt64(HttpContext.Current.Session["ReferralDocumentID"]); }
            set { HttpContext.Current.Session["ReferralDocumentID"] = value; }
        }
        public static long ReferraID
        {
            get { return Convert.ToInt64(HttpContext.Current.Session["ReferraID"]); }
            set { HttpContext.Current.Session["ReferraID"] = value; }
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

        public static bool IsCompletedWizard
        {
            get { return Convert.ToBoolean(HttpContext.Current.Session["IsCompletedWizard"]); }
            set { HttpContext.Current.Session["IsCompletedWizard"] = value; }
        }

        public static long OrganizationId
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["OrganizationId"]); }
            set { HttpContext.Current.Session["OrganizationId"] = value; }
        }

        public static string CompanyName
        {
            get { return Convert.ToString(HttpContext.Current.Session["CompanyName"]); }
            set { HttpContext.Current.Session["CompanyName"] = value; }
        }

        public static string UserName
        {
            get { return Convert.ToString(HttpContext.Current.Session["UserName"]); }
            set { HttpContext.Current.Session["UserName"] = value; }
        }

        public static bool IsMEAdmin
        {
            get { return UserName?.ToLower() == "me-admin"; }
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


            //get { if (HttpContext.Current.Session != null) 
            //        return Convert.ToString(HttpContext.Current.Session["DomainName"]);
            //    return "";
            //}
            //set { HttpContext.Current.Session["DomainName"] = value; }

            get { return (string)GetSessionValue("DomainName"); }
            set { SetSessionValue("DomainName", value); }
        }
        public static string OrgCssPath
        {
            //get { return Convert.ToString(HttpContext.Current.Session["OrgCssPath"]); }
            //set { HttpContext.Current.Session["OrgCssPath"] = value; }

            get { return (string)GetSessionValue("OrgCssPath"); }
            set { SetSessionValue("OrgCssPath", value); }
        }


        public static bool IsHomeCare
        {
            get { return Convert.ToBoolean(HttpContext.Current.Session["IsHomeCare"]); }
            set { HttpContext.Current.Session["IsHomeCare"] = value; }
        }


        public static bool IsDayCare
        {
            get { return Convert.ToBoolean(HttpContext.Current.Session["IsDayCare"]); }
            set { HttpContext.Current.Session["IsDayCare"] = value; }
        }

        public static bool IsRAL
        {
            get { return Convert.ToBoolean(HttpContext.Current.Session["IsRAL"]); }
            set { HttpContext.Current.Session["IsRAL"] = value; }
        }

        public static bool IsPrivateDutyCare
        {
            get { return Convert.ToBoolean(HttpContext.Current.Session["IsPrivateDutyCare"]); }
            set { HttpContext.Current.Session["IsPrivateDutyCare"] = value; }
        }

        public static bool IsCaseManagement
        {
            get { return Convert.ToBoolean(HttpContext.Current.Session["IsCaseManagement"]); }
            set { HttpContext.Current.Session["IsCaseManagement"] = value; }
        }
        public static bool IsStaffing
        {
            get { return Convert.ToBoolean(HttpContext.Current.Session["IsStaffing"]); }
            set { HttpContext.Current.Session["IsStaffing"] = value; }
        }

        public static bool IsEmployeeLogin
        {
            get { return Convert.ToBoolean(HttpContext.Current.Session["IsEmployeeLogin"]); }
            set { HttpContext.Current.Session["IsEmployeeLogin"] = value; }
        }

        public static string TenantGuid
        {
            get { return Convert.ToString(HttpContext.Current.Session["TenantGuid"]); }
            set { HttpContext.Current.Session["TenantGuid"] = value; }
        }


        public static string TempModel
        {
            get { return Convert.ToString(HttpContext.Current.Session["TempModel"]); }
            set { HttpContext.Current.Session["TempModel"] = value; }
        }

        public static DateTime? searchStartDate
        {
            get { return Convert.ToDateTime(HttpContext.Current.Session["StartDate"]); }
            set { HttpContext.Current.Session["StartDate"] = value; }
        }

        public static List<InvoiceMod> CompanyInvoiceInfo
        {
            get { return HttpContext.Current.Session["CompanyInvoiceDetails"] as List<InvoiceMod>; }
            set { HttpContext.Current.Session["CompanyInvoiceDetails"] = value; }
        }

        public static DateTime? TimeIntrvalSet
        {
            get { return Convert.ToDateTime(HttpContext.Current.Session["TimeIntrvalSet"]); }
            set { HttpContext.Current.Session["TimeIntrvalSet"] = value; }
        }

        public static OrganizationPreference OrganizationPreference
        {
            //get { return HttpContext.Current.Session["OrganizationPreference"] as OrganizationPreference; }
            //set { HttpContext.Current.Session["OrganizationPreference"] = value; }

            get { return (OrganizationPreference)GetSessionValue("OrganizationPreference"); }
            set { SetSessionValue("OrganizationPreference", value); }
        }
    }
}
