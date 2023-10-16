using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace HomeCareApi.Models.ApiModel
{
    public class StatusCode
    {
        /// <summary>
        /// OK. Required for all requests to confirm request is from an authorized party
        /// </summary>
        public static string Ok = Convert.ToString((int)HttpStatusCode.OK); //200

        /// <summary>
        /// Server error
        /// </summary>
        public static string InternalServerError = Convert.ToString((int)HttpStatusCode.InternalServerError); //500

        /// <summary>
        /// data is null,key invalid( Problem with the request, such as a missing, invalid or type mismatched parameter)
        /// </summary>
        public static string BadRequest = Convert.ToString((int)HttpStatusCode.BadRequest); //400

        /// <summary>
        /// user not found,data not found
        /// </summary>
        public static string NotFound = Convert.ToString((int)HttpStatusCode.NotFound); //404

        /// <summary>
        /// 
        /// </summary>
        public static string NotAcceptable = Convert.ToString((int)HttpStatusCode.NotAcceptable); //406

        /// <summary>
        /// Token expired or not found
        /// </summary>
        public static string TokenExpired = Convert.ToString((int)HttpStatusCode.Gone); //410

        /// <summary>
        /// User not verified
        /// </summary>
        public static string UserNotVerified = Convert.ToString((int)HttpStatusCode.Unauthorized); //401

        /// <summary>
        /// user has been block
        /// </summary>
        public static string BlockUser = Convert.ToString((int)HttpStatusCode.NonAuthoritativeInformation); //203

    }
}