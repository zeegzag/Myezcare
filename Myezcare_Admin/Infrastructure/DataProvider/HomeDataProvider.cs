using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Myezcare_Admin.Helpers;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.ViewModel;
using Myezcare_Admin.Resources;

namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public class HomeDataProvider : BaseDataProvider, IHomeDataProvider
    {
        public ServiceResponse SetDashboardPage(long loggedInUser)
        {
            var response = new ServiceResponse();
            
            response.IsSuccess = true;
            return response;
        }

    }
}
