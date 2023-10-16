using System.Collections.Generic;

namespace Zarephath.Core.Models.ViewModel
{
    public class SaveTransportationGroupFilter
    {
        public long TransportationGroupClientID { get; set; }
        public List<long> TransportationFilters { get; set; }
    }
}
