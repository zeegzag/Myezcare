using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface ICaptureCallDataProvider
    {
        ServiceResponse AddCaptureCall(long Id);
        ServiceResponse AddCaptureCall(Models.Entity.CaptureCalls physician, string EmployeesIDs, string RoleIds,string RelatedWithPatient, long loggedInUserId);
        ServiceResponse SetCaptureCallListPage();

        ServiceResponse DeleteCapture(SearchCaptureCallListPage searchCaptureCallListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId);

        
        ServiceResponse GetCaptureCallList(SearchCaptureCallListPage searchCaptureCallListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection);
        ServiceResponse ConvertToReferral(ConvertToReferralModel capturecall, long LoggedInID);
        ServiceResponse HC_OrbeonFormMapping(LinkDocModel model, bool isEmployeeDocument);
    }
}
