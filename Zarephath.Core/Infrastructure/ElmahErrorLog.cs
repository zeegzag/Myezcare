using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Elmah;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure
{
    public class ElmahErrorLog : SqlErrorLog
    {

        public override string ConnectionString
        {
            get
            {
                try
                {


                    CacheHelper_MyezCare chHelperMyezCare = new CacheHelper_MyezCare();
                    var cachedName = Common.GetSubDomainName() + "023543";
                    MyEzcareOrganization orgData = chHelperMyezCare.GetCachedData<MyEzcareOrganization>(cachedName);
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
