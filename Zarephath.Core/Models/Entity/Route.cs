using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("routes")]
    [PrimaryKey("RouteCode")]
    [Sort("RouteCode", "ASC")]
    public class Route
    {
        public string RouteCode { get; set; }
        public string RouteName { get; set; }
    }
}