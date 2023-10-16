using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Infrastructure.Utility.Fcm
{
    public enum EnumGcmMessageResponseTypes
    {
        Success,
        /// <summary>
        /// Check that the request contains a registration token (in the registration_id in a plain text message, or in the to or registration_ids field in JSON).
        /// </summary>
        MissingRegistrationToken,

        /// <summary>
        /// Check the format of the registration token you pass to the server. Make sure it matches the registration token the client app receives from registering with Firebase Notifications. Do not truncate or add additional characters.
        /// </summary>
        InvalidRegistrationToken,

        /// <summary>
        /// If the client app unregisters with FCM.
        /// If the client app is automatically unregistered, which can happen if the user uninstalls the application. For example, on iOS, if the APNS Feedback Service reported the APNS token as invalid.
        /// If the registration token expires (for example, Google might decide to refresh registration tokens, or the APNS token has expired for iOS devices).
        /// If the client app is updated but the new version is not configured to receive messages.
        /// </summary>

        UnregistedDevice,

        /// <summary>
        /// Make sure the message was addressed to a registration token whose package name matches the value passed in the request.
        /// </summary>
        InvalidPackageName,

        MismathedSender = 5,
        
        MessageTooBig = 6,
        InvalidDataKey,
        InvalidTimeToLive,
        BadAckMessage = 100,
        Timeout = 9,
        InternalServerError = 500,
        DeviceMessageRateExceeded = 10,
        TopicsMessageRateExceeded,
        TopicTooManySubscribers = 50,
        TopicInvalidParameters,

        /// <summary>
        /// Authorization header missing or with invalid syntax in HTTP request.
        /// Invalid project number sent as key.
        /// Key valid but with FCM service disabled.
        /// Request originated from a server not whitelisted in the Server key IPs.
        /// </summary>
        AuthenticationError = 401,
        InvalidJson = 400,

    }
}
