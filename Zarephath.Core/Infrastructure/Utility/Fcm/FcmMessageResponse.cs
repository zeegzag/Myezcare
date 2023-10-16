using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Zarephath.Core.Infrastructure.Utility.Fcm
{
    public class FcmMessageResponse : IResponse
    {
        /// <summary>
        /// Unique ID (number) identifying the multicast message.
        /// </summary>
        [JsonProperty("multicast_id", NullValueHandling = NullValueHandling.Ignore)]
        public long MultiCastId
        {
            get;
            set;
        }

        /// <summary>
        /// Number of messages that were processed without an error.
        /// </summary>
        [JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
        public int MessagesSucceededCount
        {
            get;
            set;
        }
        /// <summary>
        /// Number of messages that could not be processed.
        /// </summary>
        [JsonProperty("failure", NullValueHandling = NullValueHandling.Ignore)]
        public int MessagesFailedCount
        {
            get;
            set;
        }

        /// <summary>
        /// Number of results that contain a canonical registration token. A canonical registration ID is the registration token of the last registration requested by the client app. This is the ID that the server should use when sending messages to the device.
        /// </summary>
        [JsonProperty("canonical_ids", NullValueHandling = NullValueHandling.Ignore)]
        public int CoanonicalIds
        {
            get;
            set;
        }

        /// <summary>
        /// Array of objects representing the status of the messages processed. The objects are listed in the same order as the request (i.e., for each registration ID in the request, its result is listed in the same index in the response).
        /// </summary>
        [JsonProperty("results", NullValueHandling = NullValueHandling.Ignore)]
        public List<MessageStatus> Results
        {
            get;
            set;
        }

        public FcmMessage Message
        {
            get;
            set;
        }

        public Message MessageV1
        {
            get;
            set;
        }

        public HttpWebResponse HttpWebResponse
        {
            get;
            set;
        }

        public WebRequest WebRequest
        {
            get;
            set;
        }

        public EnumGcmMessageResponseTypes ResponseStatus
        {
            get;
            set;
        }

        public bool Success
        {
            get
            {
                return ResponseStatus == EnumGcmMessageResponseTypes.Success && MessagesFailedCount == 0;
            }
        }

        #region notificatino with Topic
        /// <summary>
        /// The topic message ID when FCM has successfully received the request and will attempt to deliver to all subscribed devices.
        /// </summary>
        [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
        public long MessageId
        {
            get;
            set;
        }

        /// <summary>
        /// Error that occurred when processing the message.
        /// </summary>
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public long Error
        {
            get;
            set;
        }
        #endregion notificatino with Topic

    }
}
