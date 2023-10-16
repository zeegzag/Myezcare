using System;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.ViewModel;

namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public interface IHomeDataProvider
    {
        ServiceResponse SetDashboardPage(long loggedInUser);
    }
}
