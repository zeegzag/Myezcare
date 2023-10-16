using System.Collections.Generic;

namespace Zarephath.Core.Models.ViewModel
{
    public class LatitudeLongitudeModel
    {
        public LatitudeLongitudeModel()
        {
            EmployeeAdressList = new List<EmployeeAddress>();
            ReferralAddressList = new List<ReferralAddress>();
        }
        public List<EmployeeAddress> EmployeeAdressList { get; set; }
        public List<ReferralAddress> ReferralAddressList { get; set; }
    }

    public class EmployeeAddress
    {
        public long EmployeeID { get; set; }
        public string FullAddress { get; set; }
    }
    public class ReferralAddress
    {
        public long ReferralID { get; set; }
        public string FullAddress { get; set; }
    }

    public class LatLong
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    
}
