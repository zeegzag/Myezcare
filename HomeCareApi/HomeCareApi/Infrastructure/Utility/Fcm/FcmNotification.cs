using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Zarephath.Core.Infrastructure.Utility.Fcm
{
   public class FcmNotification
    {
       public FcmNotification()
       {
            Badge = "1";
            Sound = "default";
        }
        /// <summary>
        /// The notification's title.
        /// Optional
        /// </summary>
        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public  string Body { get; set; }

        /// <summary>
        /// The notification's body text.
        /// Optional
        /// </summary>
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        /// <summary>
        ///Sets the notification icon to myicon for drawable resource myicon. If you don't send this key in the request, FCM displays the launcher icon specified in your app manifest.
        /// optional
        /// </summary>
        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public string Icon { get; set; }


       /// <summary>
        /// The action associated with a user click on the notification.
        /// For all URL values, secure HTTPS is required.
        /// Optional
        /// </summary>
        [JsonProperty("click_action", NullValueHandling = NullValueHandling.Ignore)]
        public string ClickAction { get; set; }

        #region iOS/Android — keys for notification messages

        /// <summary>
        /// The sound to play when the device receives the notification.
        /// Supports "default" or the filename of a sound resource bundled in the app. Sound files must reside in /res/raw/.
        /// optional
        /// </summary>
        [JsonProperty("sound", NullValueHandling = NullValueHandling.Ignore)]
        public string Sound { get; set; }

        #endregion


        #region iOS — keys for notification messages
        /// <summary>
        /// The value of the badge on the home screen app icon.
        /// If not specified, the badge is not changed.
        /// If set to 0, the badge is removed.
        /// </summary>
        [JsonProperty("badge", NullValueHandling = NullValueHandling.Ignore)]
        public string Badge { get; set; }

        #endregion

        #region Android — keys for notification messages


        /// <summary>
        /// The notification's icon color, expressed in #rrggbb format.
        /// Optional
        /// </summary>
        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        #endregion

    }
}
