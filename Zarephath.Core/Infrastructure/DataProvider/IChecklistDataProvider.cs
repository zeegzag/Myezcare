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
    public interface IChecklistDataProvider
    {
        ServiceResponse GetChecklistItemTypes();
        ServiceResponse GetChecklistItems(ChecklistItemModel model);
        ServiceResponse SaveChecklistItems(SaveChecklistItemModel model, long loggedInUserId);
        ServiceResponse GetIsChecklistRemaining(ChecklistItemModel model);
        ServiceResponse GetVisitChecklistItems(ChecklistItemModel model);
        ServiceResponse GetVisitChecklistItemDetail(VisitChecklistItemModel model);
    }
}
