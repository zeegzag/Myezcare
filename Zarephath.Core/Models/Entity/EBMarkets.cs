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
    [TableName("EBMarkets")]
    [PrimaryKey("ID")]
    [Sort("EBMarketID", "DESC")]
    public class EBMarkets : BaseEntity
    {
        public string ID { get; set; }
        public string EBMarketID { get; set; }

      [Required(ErrorMessageResourceName = "MarketNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}

