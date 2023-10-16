using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Models
{
    public class CronJobServiceModel
    {
        public string ServiceURL { get; set; }
        public string ServiceProgressURL { get; set; }
    }


    public class CronJobServiceProgressModel
    {
        public string ProgressMessage { get; set; }
        public string PercentComplete { get; set; }
    }
}
