using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models.Entity;
using System.Net;
using System.Security.Principal;
using System.Linq;
using System.Drawing;

namespace Zarephath.Web.Areas.HomeCare.Views.Report.Partial
{
    public partial class ReportTemplateWebForm : System.Web.UI.Page
    {
        string _reportURI = ConfigurationManager.AppSettings["ReportServerURL"];
        string solutionPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)));

        protected void Page_Load(object sender, EventArgs e)
        {
            //rvSiteMapping.ServerReport.ReportServerCredentials =
            //new MyReportServerCredentials();

            if (!this.IsPostBack)
            {
                rvSiteMapping.ServerReport.ReportServerCredentials =
           new MyReportServerCredentials();
                CacheHelper_MyezCare ch_MyezcareOrg = new CacheHelper_MyezCare();
                MyEzcareOrganization myOrg = ch_MyezcareOrg.GetCachedData<MyEzcareOrganization>();
                //string connString = myOrg.CurrentConnectionString;
                var dbName = myOrg.DBName.ToString();
                var servername = myOrg.DBServer.ToString();
                string connString = ConfigurationManager.ConnectionStrings["MyezcareOrganization"].ToString();
                //string OlddbName = ConfigurationManager.ConnectionStrings["MyezcareOrganization"].ToString();
                //string[] arr = OlddbName.Split(';');
                //string dbName = string.Empty;
                //foreach (var val in arr)
                //{
                //    //temp = val.Contains("database").ToString().ToUpper();
                //    if (val.ToUpper().Contains("DATABASE"))
                //    {
                //        string[] arrTempDB = val.Split('=');

                //        dbName = arrTempDB[1].ToString();
                //    }

                //}

                Dictionary<string, string> additionalParams = new Dictionary<string, string>();
                string ReportName = Request.QueryString["ReportName"].ToString();
                string ReferralID = Request.QueryString["ReferralID"];
                string StartDate = Request.QueryString["StartDate"];
                string EndDate = Request.QueryString["EndDate"];
                string EmployeeName = Request.QueryString["EmployeeName"];
                string CareTypeID = Request.QueryString["CareTypeID"];
                string Status = Request.QueryString["Status"];
                string ScheduleID = Request.QueryString["ScheduleID"];
                string EmployeeVisitID = Request.QueryString["EmployeeVisitID"];
                string TaskType = Request.QueryString["TaskType"];
                string ConclusionType = Request.QueryString["ConclusionType"];
                string RegionID = Request.QueryString["RegionID"];
                string TimeSlots = Request.QueryString["TimeSlots"];
                if (!string.IsNullOrEmpty(ReferralID))
                { additionalParams.Add("ReferralID", ReferralID); }
                if (!string.IsNullOrEmpty(StartDate))
                { additionalParams.Add("StartDate", StartDate); }
                if (!string.IsNullOrEmpty(EndDate))
                { additionalParams.Add("EndDate", EndDate); }
                if (!string.IsNullOrEmpty(EmployeeName))
                { additionalParams.Add("EmployeeName", EmployeeName); }
                if (!string.IsNullOrEmpty(CareTypeID))
                { additionalParams.Add("CareTypeID", CareTypeID); }
                if (!string.IsNullOrEmpty(Status))
                { additionalParams.Add("Status", Status); }
                if (!string.IsNullOrEmpty(ScheduleID))
                { additionalParams.Add("ScheduleID", ScheduleID); }

                if (!string.IsNullOrEmpty(EmployeeVisitID))
                { additionalParams.Add("EmployeeVisitID", EmployeeVisitID); }
                if (!string.IsNullOrEmpty(TaskType))
                { additionalParams.Add("TaskType", TaskType); }
                if (!string.IsNullOrEmpty(ConclusionType))
                { additionalParams.Add("ConclusionType", ConclusionType); }
                if (!string.IsNullOrEmpty(RegionID))
                { additionalParams.Add("RegionID", RegionID); }
                if (!string.IsNullOrEmpty(TimeSlots))
                { additionalParams.Add("TimeSlots", TimeSlots); }
                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string s = "select * from ReportMaster where ReportName ='" + ReportName + "'";
                    using (SqlCommand cmd = new SqlCommand(s, conn))
                    {
                        string temp = "";
                        string rpt = "";
                        string ds = "";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rpt += reader["RDL_FileName"];
                                temp += reader["SqlString"];
                                ds += reader["DataSet"];

                                dt = GetDataTable(temp);


                            }
                            GenerateReport(rpt, dbName, servername, additionalParams);
                        }
                    }
                }

            }


        }
        protected void GenerateReport(string reportName, string dbName, string servername, Dictionary<string, string> additionalParams) //string dataSet, DataTable dataTable)
        {
            try
            {

                //if (dataTable.Rows.Count > 0)
                //{
                rvSiteMapping.ProcessingMode = ProcessingMode.Remote;
                string filePath = "/rpt_myezcare/" + reportName;
                Uri uri = new Uri(_reportURI);
                rvSiteMapping.ServerReport.ReportServerUrl = uri;
                rvSiteMapping.ServerReport.ReportPath = filePath;
                rvSiteMapping.AsyncRendering = false;

                rvSiteMapping.LocalReport.DataSources.Clear();
                additionalParams.Add("dbname", dbName);
                additionalParams.Add("servername", servername);
                var repParams = additionalParams.Select(kv => new ReportParameter(kv.Key, kv.Value));
                //ReportParameter[] param = new ReportParameter[2];
                //param[0] = new ReportParameter("dbname", dbName);
                //param[1] = new ReportParameter("servername", servername);
                rvSiteMapping.ServerReport.SetParameters(repParams);
                rvSiteMapping.PromptAreaCollapsed = false;
                rvSiteMapping.PageCountMode = PageCountMode.Actual;
                rvSiteMapping.LocalReport.Refresh();
                //}
                //else
                //{
                //}
            }
            catch (Exception)
            {

            }
        }

        [Serializable]
        public sealed class MyReportServerCredentials : IReportServerCredentials
        {
            public WindowsIdentity ImpersonationUser
            {
                get
                {
                    // Use the default Windows user.  Credentials will be
                    // provided by the NetworkCredentials property.
                    return null;
                }
            }

            public ICredentials NetworkCredentials
            {
                get
                {
                    // Read the user information from the Web.config file.  
                    // By reading the information on demand instead of 
                    // storing it, the credentials will not be stored in 
                    // session, reducing the vulnerable surface area to the
                    // Web.config file, which can be secured with an ACL.

                    // User name
                    string userName =
                        ConfigurationManager.AppSettings
                            ["MyReportViewerUser"];

                    if (string.IsNullOrEmpty(userName))
                        throw new Exception(
                            "Missing user name from web.config file");

                    // Password
                    string password =
                        ConfigurationManager.AppSettings
                            ["MyReportViewerPassword"];

                    if (string.IsNullOrEmpty(password))
                        throw new Exception(
                            "Missing password from web.config file");

                    // Domain
                    string domain =
                        ConfigurationManager.AppSettings
                            ["MyReportViewerDomain"];

                    if (string.IsNullOrEmpty(domain))
                        throw new Exception(
                            "Missing domain from web.config file");

                    return new NetworkCredential(userName, password, domain);
                }
            }

            public bool GetFormsCredentials(out Cookie authCookie,
                        out string userName, out string password,
                        out string authority)
            {
                authCookie = null;
                userName = null;
                password = null;
                authority = null;

                // Not using form credentials
                return false;
            }
        }

        protected DataTable GetDataTable(string query)
        {
            CacheHelper_MyezCare ch_MyezcareOrg = new CacheHelper_MyezCare();
            MyEzcareOrganization myOrg = ch_MyezcareOrg.GetCachedData<MyEzcareOrganization>();
            string connString = myOrg.CurrentConnectionString;
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                // SqlConnection conn = new SqlConnection(connString);
                conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    da.Fill(dt);
                }

            }

            return dt;
        }


    }
}
