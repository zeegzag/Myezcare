using Myezcare_Admin.Infrastructure.DataProvider;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.Entity;
using Myezcare_Admin.Models.ViewModel;
using Myezcare_Admin.Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Myezcare_Admin.Infrastructure.Utility;


namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public class CronJobDataProvider : BaseDataProvider, ICronJobDataProvider
    {
        public ServiceResponse UpdateEbriggsFormDetails()
        {
            FormDataProvider formDataProvider = new FormDataProvider();
            ServiceResponse response = formDataProvider.UpdateEbriggsFormDetails();
            return response;
        }
    }
}
