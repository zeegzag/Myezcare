using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Zarephath.Core.Infrastructure.Utility.Fcm
{
   public class FcmManager
    {
        private const string FcmNotificationUrl = "https://fcm.googleapis.com/fcm/send";
        //private const string FcmNotificationUrl = "https://fcm.googleapis.com/v1/projects/fcm-notification-test-1/messages:send";

        private HttpWebRequest request
        {
            get;
            set;
        }
        private string AuthenticationKey
        {
            get;
            set;
        }
        private string SenderId
        {
            get;
            set;
        }

        public FcmManager(FcmManagerOptions opts)
        {
            AuthenticationKey = opts.AuthenticationKey;
            SenderId = opts.SenderId;
        }
        
        public FcmMessageResponse SendMessage(FcmMessage message)
        {
            FcmMessageResponse fcmMessageResponse = Send<FcmMessage, FcmMessageResponse>(message, FcmNotificationUrl);
            fcmMessageResponse.Message = message;
            UpdateResponseStatusForFcmMessage(fcmMessageResponse);
            return fcmMessageResponse;
        }

        public FcmMessageResponse SendMessage(Message message)
        {
            FcmMessageResponse fcmMessageResponse = Send<Message, FcmMessageResponse>(message, FcmNotificationUrl);
            fcmMessageResponse.MessageV1 = message;
            UpdateResponseStatusForFcmMessage(fcmMessageResponse);
            return fcmMessageResponse;
        }

        public FcmMessageResponse SubscribeToTopic(string registerId, string topic)
        {
            string url = "https://iid.googleapis.com/iid/v1/" + registerId + "/rel/topics/" + topic;
            FcmMessageResponse fcmMessageResponse = Send<FcmMessage, FcmMessageResponse>(null, url);
            return fcmMessageResponse;
        }

        private K Send<T, K>(T obj, string url) where K : IResponse, new()
        {
            K result = (default(K) == null) ? Activator.CreateInstance<K>() : default(K);
            string text = JsonConvert.SerializeObject(obj);
            SetupRequest(url);
            //request.ContentLength = text.Length;
            using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.AutoFlush = false;
                streamWriter.Write(text);
                streamWriter.Flush();
                streamWriter.Close();
            }
            HttpWebResponse httpWebResponse;
            string value;
            try
            {
                httpWebResponse = (HttpWebResponse)request.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    value = streamReader.ReadToEnd();
                }
                httpWebResponse.Close();
            }
            catch (WebException ex)
            {
                httpWebResponse = (HttpWebResponse)ex.Response;
                Console.WriteLine("Error code: {0}", httpWebResponse.StatusCode);
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    value = streamReader.ReadToEnd();
                }
                ex.Response.Close();
                httpWebResponse.Close();
            }
            request.Abort();
            if (httpWebResponse.StatusCode == HttpStatusCode.OK)
            {
                result = JsonConvert.DeserializeObject<K>(value);
            }
            result.HttpWebResponse = httpWebResponse;
            result.WebRequest = request;
            return result;
        }

        private void SetupRequest(string url)
        {
            request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Timeout = 20000;
            request.Proxy = null;
            request.Headers.Add(HttpRequestHeader.Authorization, "key=" + AuthenticationKey);
            request.Headers.Add("Sender: id=" + SenderId);
        }

        private void UpdateResponseStatusForFcmMessage(FcmMessageResponse response)
        {
            HttpStatusCode statusCode = response.HttpWebResponse.StatusCode;
            if (statusCode == HttpStatusCode.BadRequest)
            {
                response.ResponseStatus = EnumGcmMessageResponseTypes.InvalidJson;
            }
            else if (statusCode == HttpStatusCode.OK)
            {
                response.ResponseStatus = EnumGcmMessageResponseTypes.Success;
                if (response.Results != null)
                {
                    foreach (MessageStatus current in response.Results)
                    {
                        string error = current.Error;
                        switch (error)
                        {
                            case "MissingRegistration":
                                current.Status = EnumGcmMessageResponseTypes.MissingRegistrationToken;
                                break;
                            case "InvalidRegistration":
                                current.Status = EnumGcmMessageResponseTypes.InvalidRegistrationToken;
                                break;
                            case "NotRegistered":
                                current.Status = EnumGcmMessageResponseTypes.UnregistedDevice;
                                break;
                            case "InvalidPackageName":
                                current.Status = EnumGcmMessageResponseTypes.InvalidPackageName;
                                break;
                            case "MismatchSenderId":
                                current.Status = EnumGcmMessageResponseTypes.MismathedSender;
                                break;
                            case "MessageTooBig":
                                current.Status = EnumGcmMessageResponseTypes.MessageTooBig;
                                break;
                            case "InvalidDataKey":
                                current.Status = EnumGcmMessageResponseTypes.InvalidDataKey;
                                break;
                            case "InvalidTtl":
                                current.Status = EnumGcmMessageResponseTypes.InvalidTimeToLive;
                                break;
                            case "Unavailable":
                                current.Status = EnumGcmMessageResponseTypes.Timeout;
                                response.ResponseStatus = EnumGcmMessageResponseTypes.Timeout;
                                break;
                            case "InternalServerError":
                                current.Status = EnumGcmMessageResponseTypes.InternalServerError;
                                response.ResponseStatus = EnumGcmMessageResponseTypes.InternalServerError;
                                break;
                            case "DeviceMessageRateExceeded":
                                current.Status = EnumGcmMessageResponseTypes.DeviceMessageRateExceeded;
                                break;
                            case "TopicsMessageRateExceeded":
                                current.Status = EnumGcmMessageResponseTypes.TopicTooManySubscribers;
                                break;
                        }
                    }
                }
            }
            else if (statusCode == HttpStatusCode.Unauthorized)
            {
                response.ResponseStatus = EnumGcmMessageResponseTypes.AuthenticationError;
            }
            else if (statusCode >= HttpStatusCode.InternalServerError)
            {
                response.ResponseStatus = EnumGcmMessageResponseTypes.InternalServerError;
            }
        }

    }
}
