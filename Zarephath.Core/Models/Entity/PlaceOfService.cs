using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("PlaceOfServices")]
    [PrimaryKey("PosID")]
    [Sort("PosID", "DESC")]
    public class PlaceOfService
    {
        public long PosID { get; set; }
        public string PosName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
