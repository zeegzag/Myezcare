using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class OrgPreferenceRegions
    {
        public static readonly NameValueStringData US = new NameValueStringData { Name = "United States", Value = "United States" };
        public static readonly NameValueStringData India = new NameValueStringData { Name = "India", Value = "India" };
        public static readonly NameValueStringData SA = new NameValueStringData { Name = "South Africa", Value = "South Africa" };
    }

    public class OrgPreferenceModel
    {
        public OrgPreferenceModel()
        {
            OrganizationPreference = new OrganizationPreference();
            LanguageList = new List<Language>();
            CurrencyList = new List<Currency>();
            CssConfigList = new List<CssConfig>();
        }

        public OrganizationPreference OrganizationPreference { get; set; }
        public List<Language> LanguageList { get; set; }
        public List<Currency> CurrencyList { get; set; }
        public List<CssConfig> CssConfigList { get; set; }

        public IReadOnlyCollection<NameValueStringData> RegionList
        {
            get
            {
                return new List<NameValueStringData>
                {
                    OrgPreferenceRegions.US,
                    OrgPreferenceRegions.India,
                    OrgPreferenceRegions.SA,
                };
            }
        }

        public IReadOnlyCollection<NameValueStringData> NameFormatList
        {
            get
            {
                return new List<NameValueStringData>
                {
                    new NameValueStringData { Name = "First Last", Value = "First Last" },
                    new NameValueStringData { Name = "Last First", Value = "Last First" },
                    new NameValueStringData { Name = "First, Last", Value = "First, Last" },
                    new NameValueStringData { Name = "Last, First", Value = "Last, First" },
                    new NameValueStringData { Name = "Last, First Middle", Value = "Last, First Middle" },
                    new NameValueStringData { Name = "First Middle Last", Value = "First Middle Last" },
                };
            }
        }

        public IReadOnlyCollection<NameValueStringData> DateFormatList
        {
            get
            {
                return new List<NameValueStringData>
                {
                    new NameValueStringData { Name = "DD/MM/YYYY" , Value = "DD/MM/YYYY" },
                    new NameValueStringData { Name = "DD/MMM/YYYY", Value = "DD/MMM/YYYY" },
                    new NameValueStringData { Name = "MM/DD/YYYY", Value = "MM/DD/YYYY" },
                    new NameValueStringData { Name = "MMM/DD/YYYY", Value = "MMM/DD/YYYY" },
                    new NameValueStringData { Name = "YYYY/MM/DD", Value = "YYYY/MM/DD" },
                    new NameValueStringData { Name = "DD.MM.YYYY", Value = "DD.MM.YYYY" },
                    new NameValueStringData { Name = "MM.DD.YYYY", Value = "MM.DD.YYYY" },
                    new NameValueStringData { Name = "YYYY.MM.DD", Value = "YYYY.MMDD" },
                };
            }
        }

        public List<NameValueData> WeekDaysList { get { return Common.SetWeekDays(); } }
    }

}
