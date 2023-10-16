using Elmah;
using HomeCareApi.Infrastructure.DataProvider;
using HomeCareApi.Models.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeCareApi.Infrastructure
{
    public class ElmahErrorLog : SqlErrorLog
    {
        public override string ConnectionString
        {
            get
            {
                try
                {
                    BaseDataProvider bd = new BaseDataProvider();

                    var companyName = Common.GetDatabaseNameFromApi();
                    MyEzcareOrganization orgData = bd.GetOrganizationConnectionString(companyName);
                    
                    if (orgData != null)
                        return string.Format("Server={0};Database={1}; User ID={2};Password={3}", orgData.DBServer, orgData.DBName, orgData.DBUserName, orgData.DBPassword);
                }
                catch (Exception)
                {

                }

                return "";
                //Server=192.168.1.32\express17;Database=MyezcareOrganization; User ID=pprajapati;Password=PraPra1!

                //string connectionStr = "";
                //string domain = "MyezcareOrganization";// Common.GetSubDomainName();
                //var data = ConfigurationManager.ConnectionStrings[domain];
                //if (data != null)
                //    connectionStr = ConfigurationManager.ConnectionStrings[domain].ConnectionString;
                //return connectionStr;
            }
        }

        public ElmahErrorLog(IDictionary config)
            : base(config)
        {
        }

        public ElmahErrorLog(string connectionString)
            : base(connectionString)
        {
        }
    }
}