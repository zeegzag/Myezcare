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
    public interface IDMASDataProvider
    {
        #region DMAS 97
        ServiceResponse GetDmas97Ab(Dmas97Model dmas);

        ServiceResponse GenerateDmas97AB(long Dmas97ID);

        ServiceResponse AddUpdateDmas97Ab(DmasModel dmas, long loggedInUserId);

        ServiceResponse Dmas97AbList(long ReferralID);

        ServiceResponse DeleteDmas97Ab(Dmas97AbModel dmas);

        ServiceResponse CloneDataDMAS97AB(Dmas97CloneModel dmas, long loggedInUserId);

        #endregion

        #region DMAS 99
        ServiceResponse GetDmas99Form(Dmas99Models dmas);

        ServiceResponse GenerateDMAS99Form(long Dmas99ID);

        ServiceResponse AddUpdateDmas99(GetDmas99Model dmas, long loggedInUserId);

        ServiceResponse Dmas99ListPage(long ReferralID);

        ServiceResponse DeleteDmas99Form(Dmas99Model dmas);

        ServiceResponse CloneDataDMAS99(Dmas99CloneModel dmas, long loggedInUserId);
        #endregion

        #region CMS 485
        ServiceResponse GetCms485Form(Cms485AddModel cms);

        ServiceResponse AddUpdateCms485Form(GetCms485Model cms, long loggedInUserId);

        ServiceResponse Cms485FormList(long ReferralID);

        ServiceResponse DeleteCms485Form(Cms485Model cms);

        ServiceResponse CloneCms485Form(Cms485CloneModel cms, long loggedInUserId);

        ServiceResponse GenerateCms485Form(long Cms485ID);
        #endregion
    }
}
