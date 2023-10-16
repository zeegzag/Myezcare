using PetaPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("EBCategories")]
    [PrimaryKey("ID")]
    [Sort("EBCategoryID", "DESC")]
    public class EBCategory : BaseEntity
    {
        public string ID { get; set; }
        public string EBCategoryID { get; set; }

        [Required(ErrorMessageResourceName = "CategoryNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}

