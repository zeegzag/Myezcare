using System;
using System.Runtime.Serialization;

namespace HomeCareApi.Infrastructure
{
    [DataContract]
    [Serializable]
    public class CachedData
    {
        [DataMember]
        public long EmployeeId { get; set; }

        [DataMember]
        public string DeviceUDID { get; set; }

        [DataMember]
        public string Platform { get; set; }

        [DataMember]
        public DateTime ExpireLogin { get; set; }
    }

    public class CachedDataForKey
    {
        public string KeyValues { get; set; }

        public DateTime ExpireLogin { get; set; }
    }
}