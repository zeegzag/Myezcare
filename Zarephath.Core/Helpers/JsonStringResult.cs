using System.Web.Mvc;

namespace Zarephath.Core.Helpers
{
    public class JsonStringResult : ContentResult
    {
        public JsonStringResult(string json)
        {
            Content = json;
            ContentType = "application/json";
        }
    }
}
