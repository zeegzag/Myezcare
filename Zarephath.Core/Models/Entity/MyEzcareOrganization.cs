using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressiveAnnotations.Attributes;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Organizations")]
    [PrimaryKey("OrganizationID")]
    public class MyEzcareOrganization
    {
        public long OrganizationID { get; set; }

        public string DisplayName { get; set; }
        public string CompanyName { get; set; }
        public string DomainName { get; set; }

        public string DBServer { get; set; }
        public string DBName { get; set; }
        public string DBUserName { get; set; }
        public string DBPassword { get; set; }
        public string DBProviderName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string OrganizationType { get; set; }

        public string iOSAppDownloadURL { get; set; }
        public string AndroidAppDownloadURL { get; set; }

        public bool IsActive { get; set; }

        [Ignore]
        public string CurrentConnectionString { get; set; }
        
        public enum AgencyType
        {
            HomeCare=1,
            DayCare=2,
            PrivateDutyCare = 3,
            CaseManagement = 4,
            RAL = 5,
            Staffing = 6
        }
        public bool IsCompletedWizard { get; set; }
    }


    public class OrganizationModel
    {
        public OrganizationModel()
        {
            MyEzcareOrganization = new MyEzcareOrganization();
            ReleaseNote = new ReleaseNote();
        }
        public MyEzcareOrganization MyEzcareOrganization { get; set; }
        public ReleaseNote ReleaseNote { get; set; }
    }


    
}
