using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface ITransportaionLocation
    {
        #region Add Department

        ServiceResponse AddTransportaionLocation(AddTransportationLocationModel TransportaionLocation, long loggedInUserID);
        ServiceResponse SetAddTransportaionLocationPage(long departmentId, long loggedInUserID);
        //ServiceResponse SaveFile(HttpRequestBase file, long loggedInUserID);
        #endregion

        #region Department List

        ServiceResponse SetTransportaionLocationListPage();
        ServiceResponse GetTransportaionLocationList(SearchTransPortationListPage setTransPortationListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteTransportaionLocation(SearchTransPortationListPage setTransPortationListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);

        #endregion

        List<ZipCodes> GetZipCodeList(string searchText, string state, int pageSize);
    }
}
