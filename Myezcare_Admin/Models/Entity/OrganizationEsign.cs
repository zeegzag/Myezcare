using Myezcare_Admin.Infrastructure.Attributes;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Myezcare_Admin.Resources;
using Myezcare_Admin.Infrastructure;

namespace Myezcare_Admin.Models.Entity
{
    [TableName("OrganizationEsigns")]
    [PrimaryKey("OrganizationEsignID")]
    [Sort("OrganizationEsignID", "DESC")]
    public class OrganizationEsign
    {
        public long OrganizationEsignID { get; set; }

        public long OrganizationID { get; set; }

        public string EsignTerms { get; set; }

        [Required(ErrorMessageResourceName = "ServiceTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ServiceType { get; set; }

        public DateTime EsignSentDate { get; set; }

        public string EsignName { get; set; }

        public DateTime? EsignAcceptedDate { get; set; }
    }

    public enum ServiceType
    {
        ServiceType1 = 1,
        ServiceType2 = 2
    }
}