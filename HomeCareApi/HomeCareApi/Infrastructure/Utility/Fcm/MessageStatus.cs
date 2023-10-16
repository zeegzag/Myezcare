using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Zarephath.Core.Infrastructure.Utility.Fcm
{
    public class MessageStatus
    {
        [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
        public string MessageId
        {
            get;
            set;
        }

        [JsonProperty("registration_id", NullValueHandling = NullValueHandling.Ignore)]
        public string RegistrationId
        {
            get;
            set;
        }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error
        {
            get;
            set;
        }

        public EnumGcmMessageResponseTypes Status
        {
            get;
            set;
        }
    }
}
