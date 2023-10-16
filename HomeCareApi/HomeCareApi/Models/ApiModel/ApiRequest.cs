using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeCareApi.Models.ApiModel
{
    public class ApiRequest<T>
    {
        public string Token { get; set; }
        public string Key { get; set; }
        public string CompanyName { get; set; }
        public T Data { get; set; }
    }

    public class ApiRequest
    {
        public string Token { get; set; }
        public string Key { get; set; }
        public string CompanyName { get; set; }
        
    }
}