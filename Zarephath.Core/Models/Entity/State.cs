using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("states")]
    [PrimaryKey("StateCode")]
    [Sort("StateCode", "DESC")]
    public class State 
    {
        public string StateCode { get; set; }
        public string StateName { get; set; }
    }
}