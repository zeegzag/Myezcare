using System;
using System.Web.Http;
using Newtonsoft.Json.Converters;
using HomeCareApi.Infrastructure;
using HomeCareApi.Infrastructure.Attributes;
using HomeCareApi.Models.ApiModel;

namespace HomeCareApi.Controllers
{
    public class BaseController : ApiController
    {
        [HttpGet]
        public ServiceResponse Ping()
        {
            return new ServiceResponse
            {
                IsSuccess = true,
                Message = "Hey! It's working."
            };
        }

        [HttpPost]
        public ServiceResponse Error(ApiRequest request)
        {
            throw new Exception("Exception Test Error");
        }

        [HttpPost]
        [IgnoreAuthentication(true)]
        public ApiResponse PostPing(ApiRequest request)
        {
            ApiResponse response = new ApiResponse();
            response.IsSuccess = true;
            response.Message = "Hey! It's working.";
            response.Data = new DateDemo { ScheduleDate = DateTime.Now.ToString("s") };
            return response;

        }

        [HttpPost]
        [IgnoreAuthentication(true)]
        public ApiResponse PostCall(ApiRequest request)
        {
            ApiResponse response = new ApiResponse();
            response.IsSuccess = true;
            response.Message = "Hey! It's working.";
            response.Data = new Demo { FirstName = "Nisarg", LastName = "Shah" };
            return response;

        }

    }

    public class DateDemo
    {
        public string ScheduleDate { get; set; }
    }

    public class Demo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    //public class CustomDateTimeConverter : IsoDateTimeConverter
    //{
    //    public CustomDateTimeConverter()
    //    {
    //        base.DateTimeFormat = "yyyy-MM-ddTHH:mm:ss zzz";
    //    }

    //}

    public class CustomDateConverter : IsoDateTimeConverter
    {
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            DateTime dateTime;
            if (reader.Value == null)
                return null;
            if (DateTime.TryParse(reader.Value.ToString(), out dateTime))
                return DateTime.Parse(reader.Value.ToString());
            return null;
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString(ConfigSettings.AppDateTimeFormat));
        }
    }
    public class CustomDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;
            return DateTime.Parse(reader.Value.ToString());
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToUniversalTime().ToString(ConfigSettings.AppDateTimeFormat));
        }
    }
    public class CustomUTCDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;
            return DateTime.Parse(reader.Value.ToString());
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {

            value = DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
            writer.WriteValue(((DateTime)value).ToLocalTime().ToString(ConfigSettings.AppDateTimeFormat));
        }
    }
}