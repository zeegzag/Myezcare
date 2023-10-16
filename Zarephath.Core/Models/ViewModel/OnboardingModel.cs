using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Models.ViewModel
{
    public class OnboardingModel
    {
        public IDictionary<string, string[]> Wizard
        {
            get
            {
                return new Dictionary<string, string[]>()
                {
                    {"organizationsetting", new string[]{"Organization Details", "Billing Settings" } },
                    {"addservicecode", new string[] {"Add Service Code" } },
                    {"addpayor", new string[]{ "Payor Details"} },
                    {"generalmasterdetail", new string[] { "Visit Types", "Care Types" } },
                    {"addvisittask", new string[] { "Add Visit Task" } }
                };
            }
        }
        public List<string> Steps
        {
            get
            {
                return new List<string> { "Organization Details", "Billing Settings", "Add Service Code", "Payor Details", "Visit Types", "Care Types", "Add Visit Task" };
            }
        }
        public string CurrentStep { get; set; }
        public List<string> ActiveSteps { get; set; }
    }

    public class OnboardingViewModel
    {
        public long OnboardingWizardLogID { get; set; }
        public string Menu { get; set; }
        public bool IsCompleted { get; set; }
    }
}
