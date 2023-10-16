using Newtonsoft.Json;
using System.Collections.Generic;

namespace Zarephath.Core.Infrastructure.Utility.Fcm
{
    public partial class Message
    {
        [JsonProperty("topic", NullValueHandling = NullValueHandling.Ignore)]
        public string Topic { get; set; }

        [JsonProperty("registration_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Token { get; set; }
        
        [JsonProperty("notification", NullValueHandling = NullValueHandling.Ignore)]
        public MessageNotification Notification { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }

        [JsonProperty("android", NullValueHandling = NullValueHandling.Ignore)]
        public Android Android { get; set; }

        [JsonProperty("aps", NullValueHandling = NullValueHandling.Ignore)]
        public MessageAps Aps { get; set; }
    }

    public partial class Android
    {
        public Android()
        {

        }

        public Android(AndroidNotification notification)
        {
            Notification = notification;
        }

        [JsonProperty("notification", NullValueHandling = NullValueHandling.Ignore)]
        public AndroidNotification Notification { get; set; }
    }

    public partial class AndroidNotification
    {
        public AndroidNotification()
        {

        }

        public AndroidNotification(string clickAction)
        {
            ClickAction = clickAction;
        }

        [JsonProperty("click_action", NullValueHandling = NullValueHandling.Ignore)]
        public string ClickAction { get; set; }
    }

    public partial class MessageAps
    {
        public MessageAps()
        {

        }

        public MessageAps(Payload payload)
        {
            Payload = payload;
        }

        [JsonProperty("payload", NullValueHandling = NullValueHandling.Ignore)]
        public Payload Payload { get; set; }
    }

    public partial class Payload
    {
        public Payload()
        {

        }

        public Payload(PayloadAps aps)
        {
            Aps = aps;
        }

        [JsonProperty("aps", NullValueHandling = NullValueHandling.Ignore)]
        public PayloadAps Aps { get; set; }
    }

    public partial class PayloadAps
    {
        public PayloadAps()
        {

        }

        public PayloadAps(string category)
        {
            Category = category;
        }

        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }
    }
    
    public partial class MessageNotification
    {
        public MessageNotification()
        {

        }

        public MessageNotification(string title, string body)
        {
            Title = title;
            Body = body;
        }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public string Body { get; set; }
    }
}
