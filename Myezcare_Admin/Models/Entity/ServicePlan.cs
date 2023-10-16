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
    [TableName("ServicePlans")]
    [PrimaryKey("ServicePlanID")]
    [Sort("ServicePlanID", "DESC")]
    public class ServicePlan
    {
        public long ServicePlanID { get; set; }

        [Required(ErrorMessageResourceName = "ServicePlanNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ServicePlanName { get; set; }

        [Required(ErrorMessageResourceName = "PlanPriceRequired", ErrorMessageResourceType = typeof(Resource))]
        public float PerPatientPrice { get; set; }

        [Required(ErrorMessageResourceName = "NumberOfDaysRequired", ErrorMessageResourceType = typeof(Resource))]
        public int NumberOfDaysForBilling { get; set; }

        [Range(0, 9999999999999999.99, ErrorMessageResourceName = "InvalidAmount", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceName = "InvalidAmount", ErrorMessageResourceType = typeof(Resource))]
        public decimal? SetupFees { get; set; }

        public bool IsDeleted { get; set; }
    }
}