using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class OrganizationSettingModel
    {
    }

    public class AddOrganizationSettingModel
    {
        public AddOrganizationSettingModel()
        {
            OrganizationSetting = new OrganizationSetting();
            StateCodeList = new List<StateCodeList>();
        }
        public OrganizationSetting OrganizationSetting { get; set; }
        public List<StateCodeList> StateCodeList { get; set; }
    }
}
