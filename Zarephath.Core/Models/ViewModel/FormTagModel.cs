using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Models.ViewModel
{
    public class FormTagModel
    {
        public long OrganizationFormTagID { get; set; }
        public long OrganizationFormID { get; set; }
        public long FormTagID { get; set; }
        public string FormTagName { get; set; }
    }
}
