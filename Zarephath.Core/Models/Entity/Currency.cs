using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Currency")]
    [PrimaryKey("CurrencyID")]
    [Sort("CurrencyID", "DESC")]
    public class Currency
    {
        public long CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string Country { get; set; }
        public string Symbol { get; set; }
    }
}
