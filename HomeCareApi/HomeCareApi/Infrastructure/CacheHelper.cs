using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Script.Serialization;
using HomeCareApi.Infrastructure.DataProvider;
using HomeCareApi.Infrastructure.IDataProvider;
using HomeCareApi.Models.ApiModel;
using HomeCareApi.Resources;
using System.Configuration;
using HomeCareApi.Models.Entity;

namespace HomeCareApi.Infrastructure
{
    public class CacheHelper
    {
        public static string Token
        {
            get
            {
                var jsonSerializer = new JavaScriptSerializer();
                HttpContext.Current.Request.InputStream.Position = 0;
                var inputStream = new StreamReader(HttpContext.Current.Request.InputStream);
                var str = inputStream.ReadToEnd();
                var data = jsonSerializer.Deserialize<ApiRequest>(str);
                return data == null ? null : data.Token;
            }
        }


        public static string CompanyName
        {
            get
            {
                var jsonSerializer = new JavaScriptSerializer();
                HttpContext.Current.Request.InputStream.Position = 0;
                var inputStream = new StreamReader(HttpContext.Current.Request.InputStream);
                var str = inputStream.ReadToEnd();
                var data = jsonSerializer.Deserialize<ApiRequest>(str);
                //return data == null ? null : data.CompanyName;
                return data == null ? "localhost" : data.CompanyName;
            }
        }


        public static string Key
        {
            get
            {
                var jsonSerializer = new JavaScriptSerializer();
                HttpContext.Current.Request.InputStream.Position = 0;
                var inputStream = new StreamReader(HttpContext.Current.Request.InputStream);
                var str = inputStream.ReadToEnd();
                var data = jsonSerializer.Deserialize<ApiRequest>(str);
                return data == null ? null : data.Key;
            }
        }

        public static long EmployeeId
        {
            get
            {
                CachedData cache = GetUserByToken(Token);
                if (cache != null)
                    return GetUserByToken(Token).EmployeeId;

                return 0;
            }
        }

        public static string DeviceUDID
        {
            get
            {
                CachedData cache = GetUserByToken(Token);
                if (cache != null)
                    return GetUserByToken(Token).DeviceUDID;

                return null;
            }
        }

        public static string Platform
        {
            get
            {
                CachedData cache = GetUserByToken(Token);
                if (cache != null)
                    return GetUserByToken(Token).Platform;

                return null;
            }
        }

        public static DateTime ExpireLogin
        {
            get { return GetUserByToken(Token).ExpireLogin; }
        }

        /// <summary>
        /// Currently our token is going to work as follows. 
        /// We are going to have same expiration time in database and cache. Anytime we'll check if token is in cache and not expired, then the token is valid. 
        /// There can be one scenario in which token is deleted in cache (may be because cookie is cleared or something), this time we'll check that token in database and if exists there, set again in cookie and if not in database, send message that token not found.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static CachedData GetUserByToken(string token = "")
        {
            ObjectCache cache = MemoryCache.Default;
            token = string.IsNullOrEmpty(token) ? Token : token;
            if (cache.Contains(token))
            {
                // Here we are checking if token is in cache and not expired, then token is valid.
                CachedData tokenValue = (CachedData)cache.Get(token);

                if (tokenValue.ExpireLogin < DateTime.UtcNow)
                {
                    // Here the token is in cache but is expired so we'll remove the token from the cache and database both and send response that token is not valid.
                    DeleteTokenFromCache(token);
                    ISecurityDataProvider securityDataProvider = new SecurityDataProvider();
                    securityDataProvider.DeleteUserToken(token, string.Empty);
                    return null;
                }
                return tokenValue;
            }
            else
            {
                // Here token is not in the cache so we'll try to check in database
                ISecurityDataProvider securityDataProvider = new SecurityDataProvider();
                CachedData tokenValue = securityDataProvider.ValidateToken(token);
                if (tokenValue == null)
                {
                    // Here the token does not exist in database so send response that token not found.
                    Common.ThrowErrorMessage(Resource.TokenNotFound);
                }
                // Here the token is in the database so add in cache and validate the request.
                SetTokenInCache(token, tokenValue, cache);
                return tokenValue;
            }
        }

        /// <summary>
        /// This method will set user token in cache.
        /// </summary>
        /// <param name="tokenName">Name of the token</param>
        /// <param name="tokenValue">Value of the token in the form of CachedData object</param>
        /// <param name="cache">instance of ObjectCache</param>
        public static void SetTokenInCache(string tokenName, CachedData tokenValue, ObjectCache cache)
        {
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            DateTime utcDateTime = DateTime.SpecifyKind(tokenValue.ExpireLogin, DateTimeKind.Utc);
            cacheItemPolicy.AbsoluteExpiration = utcDateTime;
            cache.Add(tokenName, tokenValue, cacheItemPolicy);
        }

        public static bool IsValidToken(string token)
        {
            return GetUserByToken(token) != null;
        }

        /// <summary>
        /// This method remove token value from the cache.
        /// </summary>
        /// <param name="token"></param>
        public static void DeleteTokenFromCache(string token)
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove(token);
        }

        /// <summary>
        /// This method will validate whether the current key is valid or not
        /// </summary>
        /// <param name="currentKey">Key of the request</param>
        /// <returns>Key is valid or not</returns>
        public static bool ValidateKey(string currentKey = "")
        {
            ObjectCache cache = MemoryCache.Default;
            string keyName = Constants.CacheKeyNameForValidKeys;
            if (cache.Contains(keyName))
            {
                // Here we found that we have our keys in the cache. We'll now get all the keys from the cache and that call IsValidKey method which will give us response on whether the key is valid or not.
                CachedDataForKey cachedDataForKey = (CachedDataForKey)cache.Get(keyName);

                if (cachedDataForKey.ExpireLogin > DateTime.UtcNow)
                    return IsValidKey(currentKey, cachedDataForKey);

                return GetAndSetKeys(currentKey, keyName, cache);
            }
            // Here we didnt find key in the cache so we are going to get all the keys from the database and store those in cache
            return GetAndSetKeys(currentKey, keyName, cache);
        }

        /// <summary>
        /// get keys from database, srore in cache and return whether the current key is valid or not
        /// </summary>
        /// <param name="currentKey">Key of the request</param>
        /// <param name="keyName">Name of the key</param>
        /// <param name="cache">Instance of ObjectCache</param>
        /// <returns>Key is valid or not</returns>
        public static bool GetAndSetKeys(string currentKey, string keyName, ObjectCache cache)
        {
            ISecurityDataProvider securityDataProvider = new SecurityDataProvider();
            CachedDataForKey cachedDataForKey = securityDataProvider.GetValidKeys();
            if (cachedDataForKey == null)
                return false;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            DateTime utcDateTime = DateTime.SpecifyKind(cachedDataForKey.ExpireLogin, DateTimeKind.Utc);
            cacheItemPolicy.AbsoluteExpiration = utcDateTime;
            cache.Add(keyName, cachedDataForKey, cacheItemPolicy);
            return IsValidKey(currentKey, cachedDataForKey);
        }

        /// <summary>
        /// This method will check all the keys in the cache and check whether the current key is valid or not
        /// </summary>
        /// <param name="currentKey">Key of the request</param>
        /// <param name="cachedDataForKey">Instance of CachedDataForKey</param>
        /// <returns>Key is valid or not</returns>
        public static bool IsValidKey(string currentKey, CachedDataForKey cachedDataForKey)
        {
            if (string.IsNullOrEmpty(cachedDataForKey.KeyValues)) return false;
            string[] csvKeys = cachedDataForKey.KeyValues.Split(Constants.CommaChar);
            if (csvKeys.Any())
                return csvKeys.Contains(currentKey);
            return false;
        }
    }


    public class CacheHelper_MyezcareOrganization
    {

        //public readonly string OrganizationSettingCachedName = !string.IsNullOrEmpty(Domain) ? Domain : ""; // "CachedOrganizationSetting";

        public string CachedName
        {
            get
            {
                return !string.IsNullOrEmpty(CompanyName) ? CompanyName + "012345" : "";
            }
        }

        public string CompanyName
        {
            get
            {
                //var jsonSerializer = new JavaScriptSerializer();
                //HttpContext.Current.Request.InputStream.Position = 0;
                //var inputStream = new StreamReader(HttpContext.Current.Request.InputStream);
                //var str = inputStream.ReadToEnd();
                //var data = jsonSerializer.Deserialize<ApiRequest>(str);
                ////return data == null ? null : data.CompanyName;
                //return data == null ? "localhost" : data.CompanyName;
                return Common.GetDatabaseNameFromApi();
            }
        }

        #region Public Methods

        public T GetCachedData<T>(string cachedName = null)
        {

            if (string.IsNullOrEmpty(cachedName))
                cachedName = CachedName;

            ObjectCache cache = MemoryCache.Default;
            if (cache.Contains(cachedName))
            {
                //If Config exist in Cache then retuen detail.
                T tokenvalue = (T)cache.Get(cachedName);
                return tokenvalue;
            }
            return default(T);
        }

        public T AddCacheData<T>(T cachedData, string cachedName = null)
        {

            if (string.IsNullOrEmpty(cachedName))
                cachedName = CachedName;

            ObjectCache cache = MemoryCache.Default;
            cache.Remove(cachedName);
            if (cachedData != null)
            {
                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
                cache.Add(cachedName, cachedData, cacheItemPolicy);
            }
            return cachedData;
        }

        public void RemoveCacheData(string cachedName = null)
        {
            if (string.IsNullOrEmpty(cachedName))
                cachedName = CachedName;

            ObjectCache cache = MemoryCache.Default;
            cache.Remove(cachedName);
        }

        #endregion
    }

}