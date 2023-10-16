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
    public interface ITransportationGroupDataProvider
    {
        #region Transportation Assignment
        ServiceResponse SetTransPortationGroup();

        ServiceResponse GetReferralListForTransportationAssignment(SearchReferralListForTransportationAssignment searchTransportatioGroupList, int pageIndex,
                                                   int pageSize, string sortIndex, string sortDirection);

        ServiceResponse SaveTransportationGroup(AddTransportationGroupModel transportationGroup, SearchAssignedClientListForTransportationAssignment searchTransportatioGroupList, long loggedInUser);

        ServiceResponse SaveTransportationGroupMultipleClient(TransportationGroupClient transportationGroupClient, SearchAssignedClientListForTransportationAssignment searchTransportatioGroupList,
                                                      long loggedInUser);

        ServiceResponse SaveTransportationGroupClient(TransportationGroupClient transportationGroupClient,
                                                      long loggedInUser);

        ServiceResponse GetAssignedClientListForTransportationAssignment(
            SearchAssignedClientListForTransportationAssignment searchTransportatioGroupList);

        ServiceResponse RemoveTransportationGroupClient(long transportationGroupClientID, long loggedInUser);

        ServiceResponse RemoveTransportationGroup(long transportationGroupID,
                                                  SearchAssignedClientListForTransportationAssignment searchTransportatioGroupList, long loggedInUser);

        ServiceResponse SaveTransportationGroupFilter(SaveTransportationGroupFilter model, SearchAssignedClientListForTransportationAssignment searchTransportatioGroupList, long loggedInUser);

        #endregion

    }
}
