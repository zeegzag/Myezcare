using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;

namespace HomeCareApi.Models.Entity
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

        public bool IsActive { get; set; }
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