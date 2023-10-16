using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Zarephath.Core.Infrastructure.Utility.Fcm
{
    public class FcmMessage
    {
        public FcmMessage()
        {
            ContentAvailable = true;
        }


        [JsonProperty("notification", NullValueHandling = NullValueHandling.Ignore)]
        public FcmNotification Notification
        {
            get;
            set;
        }


        /// <summary>
        /// This parameter specifies the custom key-value pairs of the message's payload.
        ///For example, with data:{"score":"3x1"}:
        /// Values in string types are recommended. You have to convert values in objects or other non-string data types (e.g., integers or booleans) to string.
        /// </summary>

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public object Data
        {
            get;
            set;
        }

        /// <summary>
        /// This parameter specifies the recipient of a message.
        /// The value can be a device's registration token, a device group's notification key, or a single topic (prefixed with /topics/). To send to multiple topics, use the condition parameter.
        /// </summary>
        [JsonProperty("to", NullValueHandling = NullValueHandling.Ignore)]
        public string To
        {
            get;
            set;
        }



        /// <summary>
        /// This parameter specifies the recipient of a multicast message, a message sent to more than one registration token.
        /// The value should be an array of registration tokens to which to send the multicast message. The array must contain at least 1 and at most 1000 registration tokens. To send a message to a single device, use the to parameter.
        /// </summary>
        [JsonProperty("registration_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> RegistrationIds
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the priority of the message. Valid values are "normal" and "high." On iOS, these correspond to APNs priorities 5 and 10.
        /// By default, notification messages are sent with high priority
        /// </summary>

        [JsonProperty("priority", NullValueHandling = NullValueHandling.Ignore)]
        public string Priority
        {
            get;
            set;
        }
        /// <summary>
        ///This parameter specifies how long (in seconds) the message should be kept in FCM storage if the device is offline.The maximum time to live supported is 4 weeks, and the default value is 4 weeks.
        /// optional
        /// </summary>
        [JsonProperty("time_to_live", NullValueHandling = NullValueHandling.Ignore)]
        public string TimeToLive
        {
            get;
            set;
        }

        /// <summary>
        /// On iOS, use this field to represent content-available in the APNs payload. When a notification or message is sent and this is set to true, an inactive client app is awoken. On Android, data messages wake the app by default. On Chrome, currently not supported.
        /// </summary>
        [JsonProperty("content_available", NullValueHandling = NullValueHandling.Ignore)]
        public bool ContentAvailable { get; set; }

        /// <summary>
        /// On iOS, use this field to represent mutable_content in the APNs payload. When a notification or message is sent and this is set to true, an inactive client app is awoken. On Android, data messages wake the app by default. On Chrome, currently not supported.
        /// </summary>
        [JsonProperty("mutable_content", NullValueHandling = NullValueHandling.Ignore)]
        public bool MutableContent { get; set; }
    }
}
