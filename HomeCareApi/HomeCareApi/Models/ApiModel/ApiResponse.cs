using System.Collections.Generic;
using PetaPoco;

namespace HomeCareApi.Models.ApiModel
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            Code = StatusCode.Ok;
        }
        public string Code { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public List<ErrorList> Errors { get; set; }
    }

    public sealed class ApiResponse<T> : ApiResponse
    {
        public new T Data { get; set; }
    }

    public sealed class ApiResponseList<T> : ApiResponse
    {
        public new Page<T> Data { get; set; }
    }

    public class ServiceResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }

    public class ErrorList
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }

    public class LoginMessagemodel
    {
        public string DataKey { get; set; }
        public string DataMessage { get; set; }
    }
}