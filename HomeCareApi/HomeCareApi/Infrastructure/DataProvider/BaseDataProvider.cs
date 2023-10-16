using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Elmah;
using HomeCareApi.Helpers;
using HomeCareApi.Infrastructure.Attributes;
using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.Entity;
using HomeCareApi.Models.General;
using HomeCareApi.Resources;
using PetaPoco;
using HomeCareApi.Models.ViewModel;

namespace HomeCareApi.Infrastructure.DataProvider
{
    public class BaseDataProvider
    {
        private readonly Database _db;
        private readonly Database _odb;

        public BaseDataProvider()
        {
            var companyName = Common.GetDatabaseNameFromApi();
            _db = new Database(Constants.MyezcareOrganizationConnectionString);


            MyEzcareOrganization orgData = GetOrganizationConnectionString(companyName);
            if (orgData != null)
            {
                _db = new Database(orgData);
            }
            _db.CommandTimeout = ConfigSettings.DbCommandTimeOut;

            //if (!string.IsNullOrEmpty(dbname))
            //    _db = new Database(dbname) { CommandTimeout = ConfigSettings.DbCommandTimeOut };


            ////_db = new Database("localhost") { CommandTimeout = ConfigSettings.DbCommandTimeOut };
            ///
            _odb = new Database(Constants.OrbeonConnectionString);
            _odb.CommandTimeout = ConfigSettings.DbCommandTimeOut;
        }

        public BaseDataProvider(string connectionString)
        {
            _db = new Database(connectionString);
            _db.CommandTimeout = ConfigSettings.DbCommandTimeOut;
        }

        public BaseDataProvider(long organizationID)
        {
            _db = new Database(Constants.MyezcareOrganizationConnectionString);


            MyEzcareOrganization orgData = GetOrganizationConnectionString(organizationID);
            if (orgData != null)
            {
                _db = new Database(orgData);
            }
            _db.CommandTimeout = ConfigSettings.DbCommandTimeOut;
        }

        public MyEzcareOrganization GetOrganizationConnectionString(string companyName)
        {
            CacheHelper_MyezcareOrganization ch_MyezcareOrg = new CacheHelper_MyezcareOrganization();
            MyEzcareOrganization myOrg = ch_MyezcareOrg.GetCachedData<MyEzcareOrganization>();
            if (myOrg != null)
            {
                return myOrg;
            }
            else
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "CompanyName", Value = companyName, IsEqual = true });
                OrganizationModel org = GetMultipleEntity<OrganizationModel>(StoredProcedure.GetOrganizationDetails, searchParam);
                if (org.MyEzcareOrganization != null)
                {
                    myOrg = org.MyEzcareOrganization;
                    ch_MyezcareOrg.AddCacheData(myOrg);
                    return myOrg;
                }
                Common.ThrowErrorMessage(Resource.CompanyNotFound);
            }
            return myOrg;
        }

        public MyEzcareOrganization GetOrganizationConnectionString(long organizationID)
        {
            CacheHelper_MyezcareOrganization ch_MyezcareOrg = new CacheHelper_MyezcareOrganization();
            string cachedName = Convert.ToString(organizationID);
            MyEzcareOrganization myOrg = ch_MyezcareOrg.GetCachedData<MyEzcareOrganization>(cachedName);
            if (myOrg != null)
            {
                return myOrg;
            }
            else
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(organizationID), IsEqual = true });
                myOrg = GetEntity<MyEzcareOrganization>(StoredProcedure.GetOrganizationDetailsById, searchParam);
                if (myOrg != null)
                {
                    ch_MyezcareOrg.AddCacheData(myOrg, cachedName);
                    return myOrg;
                }
                Common.ThrowErrorMessage(Resource.CompanyNotFound);
            }
            return myOrg;
        }

        public MyEzcareOrganization OrganizationData
        {
            get
            {
                var companyName = Common.GetDatabaseNameFromApi();
                MyEzcareOrganization orgData = GetOrganizationConnectionString(companyName);
                if (orgData != null)
                { return orgData; }
                return new MyEzcareOrganization();
            }
        }

        private Database _newDb;

        public void InsertIvrLogInDatabse(string fromNo,string callLogText=null,string errorLog=null,bool isCompleted = false)
        {
            try
            {
                var companyName = Common.GetDatabaseNameFromApi();
                _newDb = new Database("IvrDetails");
                MyEzcareOrganization orgData = GetOrganizationConnectionString(companyName);

                SecurityDataProvider sc = new SecurityDataProvider();
                OrganizationSetting orgSettings = sc.GetOrganizationDetail();

                var timeUtc = DateTime.UtcNow;
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(orgSettings.TimeZone);
                var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);

                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(orgData.OrganizationID) });
                searchParam.Add(new SearchValueData { Name = "OrganizationName", Value = companyName });
                searchParam.Add(new SearchValueData { Name = "TwilioNo", Value = orgSettings.TwilioFromNo });
                searchParam.Add(new SearchValueData { Name = "FromNo", Value = fromNo });
                searchParam.Add(new SearchValueData { Name = "StartTime", Value = today.ToString(Constants.DbDateTimeFormat) });
                searchParam.Add(new SearchValueData { Name = "EndTime", Value = today.ToString(Constants.DbDateTimeFormat) });
                searchParam.Add(new SearchValueData { Name = "IsCompleted", Value = Convert.ToString(isCompleted) });
                searchParam.Add(new SearchValueData { Name = "Errorlog", Value = errorLog });
                searchParam.Add(new SearchValueData { Name = "CallLogText", Value = callLogText });
                GetScalarForNewDb(StoredProcedure.API_InsertIvrLogInDatabse, searchParam);

                _newDb.CommandTimeout = ConfigSettings.DbCommandTimeOut;
            }
            catch(Exception e)
            {

            }
        }

        public object GetScalarForNewDb(string spname, List<SearchValueData> searchParam = null)
        {
            return _newDb.ExecuteScalar<object>(GetSPString(spname, searchParam));
        }


        public ServiceResponse SaveObject<T>(T obj, CachedData cached, string successMessage = "", bool checkForUniqueValues = true) where T : class, new()
        {
            if (string.IsNullOrEmpty(Resource.RecordSavedSuccessfully))
                successMessage = Resource.RecordSavedSuccessfully;

            ServiceResponse response = new ServiceResponse();
            SetDefaultValues(obj, cached);

            if (checkForUniqueValues)
            {
                string uniqueValue = CheckForUniqueValues(obj);
                if (!string.IsNullOrEmpty(uniqueValue))
                {
                    response.Message = string.Format(Resource.RecordAlreadyExists, uniqueValue);
                    if (uniqueValue.Contains(","))
                    {
                        response.Message = string.Format(Resource.RecordCombinedAlreadyExists, uniqueValue);
                    }

                    return response;
                }
            }

            response.Data = SaveEntity(obj);
            response.IsSuccess = true;
            response.Message = successMessage;
            return response;
        }

        //public T2 GetDetailsWithDropDownList<T1, T2>(long id, string mainEntityPropertyNameInList = "Model")
        //    where T1 : class, new()
        //    where T2 : class, new()
        //{
        //    T1 mainEntiry = new T1();
        //    T2 detailsList = new T2();
        //    foreach (var prop in mainEntiry.GetType().GetProperties().Where(q => q.IsDefined(typeof(DropDownAttribute), false)))
        //    {
        //        if (null != prop && prop.CanWrite)
        //        {
        //            foreach (DropDownAttribute attr in Attribute.GetCustomAttributes(prop).OfType<DropDownAttribute>())
        //            {
        //                PropertyInfo propertyInfo = detailsList.GetType().GetProperty(attr.DropDownListProperty);
        //                List<DropDownItem> list = GetEntityList<DropDownItem>(attr.SQLQuery);
        //                propertyInfo.SetValue(detailsList, list, null);
        //            }
        //        }
        //    }

        //    PropertyInfo mainEntiryPropertyInfo = detailsList.GetType().GetProperty(mainEntityPropertyNameInList);
        //    T1 src = GetEntityWithUserNameAndValue<T1>(id);

        //    mainEntiryPropertyInfo.SetValue(detailsList, src, null);
        //    return detailsList;
        //}

        //public T1 GetDetailsWithDropDownList<T1>(long id)
        //    where T1 : class, new()
        //{
        //    return GetEntityWithUserNameAndValue<T1>(id);
        //}

        //private T1 GetEntityWithUserNameAndValue<T1>(long id) where T1 : class, new()
        //{
        //    T1 src = GetEntity<T1>(id);
        //    Dictionary<long, string> userIDWithName = new Dictionary<long, string>();
        //    foreach (var prop in src.GetType().GetProperties().Where(q => q.IsDefined(typeof(SetUserNameAttribute), false)))
        //    {
        //        if (null != prop && prop.CanWrite)
        //        {
        //            foreach (SetUserNameAttribute attr in Attribute.GetCustomAttributes(prop).OfType<SetUserNameAttribute>())
        //            {
        //                long userID = Convert.ToInt64(src.GetType().GetProperty(attr.TargetPropertyName).GetValue(src, null));
        //                string employeeName = userIDWithName.ContainsKey(userID)
        //                                          ? userIDWithName[userID]
        //                                          : GetEntity<Employee>(userID).EmployeeName;
        //                if (!userIDWithName.ContainsKey(userID))
        //                    userIDWithName.Add(userID, employeeName);
        //                prop.SetValue(src, employeeName, null);
        //            }
        //        }
        //    }

        //    foreach (var prop in src.GetType().GetProperties().Where(q => q.IsDefined(typeof(SetValueAttribute), false)))
        //    {
        //        if (null != prop && prop.CanWrite)
        //        {
        //            foreach (SetValueAttribute attr in Attribute.GetCustomAttributes(prop).OfType<SetValueAttribute>())
        //            {
        //                object[] propValues = new string[attr.PropertyNames.Length];

        //                for (int i = 0; i < attr.PropertyNames.Length; i++)
        //                {
        //                    propValues[i] = (src.GetType().GetProperty(attr.PropertyNames[i]).GetValue(src, null)).ToString();
        //                }

        //                object value = GetScalar(string.Format(attr.SQLQuery, propValues));
        //                prop.SetValue(src, (value == null) ? null : value.ToString(), null);
        //            }
        //        }
        //    }
        //    return src;
        //}

        public string CheckForUniqueValues<T>(T item) where T : class, new()
        {
            List<SearchValueData> searchParam = new List<SearchValueData>();
            string propertyDisplayName = "";
            bool hasPrimaryKey = false;
            SearchValueData searchPrimaryKeyValueData = new SearchValueData();
            if (item.GetType().GetCustomAttributes().OfType<PrimaryKeyAttribute>().Any())
            {
                PrimaryKeyAttribute primaryKeyAttribute = item.GetType().GetCustomAttributes().OfType<PrimaryKeyAttribute>().First();
                string primaryKey = primaryKeyAttribute.Value;
                PropertyInfo propInfo = item.GetType().GetProperty(primaryKey);

                searchPrimaryKeyValueData = new SearchValueData
                {
                    Name = primaryKey,
                    Value = Convert.ToString(propInfo.GetValue(item, null)),
                    IsNotEqual = true
                };
                hasPrimaryKey = true;
            }

            foreach (var prop in item.GetType().GetProperties().Where(q => q.IsDefined(typeof(UniqueAttribute), false)))
            {
                if (null != prop && prop.CanWrite)
                {
                    propertyDisplayName = prop.Name;
                    if (prop.IsDefined(typeof(DisplayNameAttribute)))
                        propertyDisplayName = prop.GetCustomAttributes(typeof(DisplayNameAttribute),
                                                 false).Cast<DisplayNameAttribute>().Single().DisplayName;

                    foreach (UniqueAttribute attr in Attribute.GetCustomAttributes(prop).OfType<UniqueAttribute>())
                    {
                        searchParam = new List<SearchValueData>();

                        SearchValueData searchUniqueValueData = new SearchValueData
                        {
                            Name = prop.Name,
                            Value = Convert.ToString(prop.GetValue(item, null)),
                            IsEqual = true
                        };
                        searchParam.Add(searchUniqueValueData);

                        if (hasPrimaryKey)
                            searchParam.Add(searchPrimaryKeyValueData);

                        string withRespectToProperty = attr.WithRespectToProperty;
                        if (!string.IsNullOrEmpty(withRespectToProperty))
                        {
                            PropertyInfo propInfo = item.GetType().GetProperty(withRespectToProperty);

                            searchPrimaryKeyValueData = new SearchValueData
                            {
                                Name = withRespectToProperty,
                                Value = Convert.ToString(propInfo.GetValue(item, null)),
                                IsNotEqual = true
                            };
                        }

                        if (GetEntityCount<T>(searchParam) > 0)
                        {
                            return propertyDisplayName;
                        }
                    }
                }
            }
            searchParam = new List<SearchValueData>();
            propertyDisplayName = "";
            foreach (var prop in item.GetType().GetProperties().Where(q => q.IsDefined(typeof(CombinedUniqueAttribute), false)))
            {
                if (null != prop && prop.CanWrite)
                {
                    if (prop.IsDefined(typeof(DisplayNameAttribute)))
                        propertyDisplayName += ", " + prop.GetCustomAttributes(typeof(DisplayNameAttribute),
                                                 false).Cast<DisplayNameAttribute>().Single().DisplayName;
                    else
                    {
                        propertyDisplayName += ", " + prop.Name;
                    }

                    foreach (CombinedUniqueAttribute attr in Attribute.GetCustomAttributes(prop).OfType<CombinedUniqueAttribute>())
                    {
                        SearchValueData searchUniqueValueData = new SearchValueData
                        {
                            Name = prop.Name,
                            Value = Convert.ToString(prop.GetValue(item, null)),
                            IsEqual = true
                        };
                        searchParam.Add(searchUniqueValueData);

                        if (hasPrimaryKey)
                            searchParam.Add(searchPrimaryKeyValueData);

                        string withRespectToProperty = attr.WithRespectToProperty;
                        if (!string.IsNullOrEmpty(withRespectToProperty))
                        {
                            PropertyInfo propInfo = item.GetType().GetProperty(withRespectToProperty);

                            searchPrimaryKeyValueData = new SearchValueData
                            {
                                Name = withRespectToProperty,
                                Value = Convert.ToString(propInfo.GetValue(item, null)),
                                IsNotEqual = true
                            };
                        }
                    }
                }
            }
            if (GetEntityCount<T>(searchParam) > 0)
            {
                return propertyDisplayName.TrimStart(',');
            }
            return "";
        }

        public T SetDefaultValues<T>(T item, CachedData cached) where T : class
        {
            if (_db.IsNew(item))
            {
                foreach (var prop in item.GetType().GetProperties().Where(q => q.IsDefined(typeof(SetValueOnAddAttribute), false)))
                {
                    if (prop.CanWrite)
                    {
                        foreach (SetValueOnAddAttribute attr in Attribute.GetCustomAttributes(prop).OfType<SetValueOnAddAttribute>())
                        {
                            switch (attr.SetValue)
                            {
                                case (int)Common.SetValue.CurrentTime:
                                    prop.SetValue(item, DateTime.UtcNow, null);
                                    break;

                                case (int)Common.SetValue.LoggedInUserId:
                                    prop.SetValue(item, cached.EmployeeId, null);
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var prop in item.GetType().GetProperties().Where(q => q.IsDefined(typeof(SetValueOnUpdateAttribute), false)))
                {
                    if (prop.CanWrite)
                    {
                        foreach (SetValueOnUpdateAttribute attr in Attribute.GetCustomAttributes(prop).OfType<SetValueOnUpdateAttribute>())
                        {
                            switch (attr.SetValueOnUpdate)
                            {
                                case (int)Common.SetValue.CurrentTime:
                                    prop.SetValue(item, DateTime.UtcNow, null);
                                    break;

                                case (int)Common.SetValue.LoggedInUserId:
                                    prop.SetValue(item, cached.EmployeeId, null);
                                    break;
                            }
                        }
                    }
                }
            }
            return item;
        }

        public T SaveEntity<T>(T item) where T : class
        {
            if (_db.IsNew(item))
            {
                _db.Insert(item);
            }
            else
            {
                _db.Update(item);
            }
            return item;
        }

        // source: http://stackoverflow.com/questions/6595105/bulk-insert-update-with-petapoco
        /// <summary>
        /// This method will insert records into db in one single query.
        /// </summary>
        /// <typeparam name="T">Classname</typeparam>
        /// <param name="collection">List of objects to insert</param>
        /// <param name="cached">From cached we are getting userId so that we can insert that for CreatedByID and ModifiedByID</param>
        /// <param name="insertWithDivision">Insert records by diving into piece so that we dont get error for length exceeded for insert statement. By Default it allows 16M of file with records.</param>
        public void BulkInsertRecords<T>(List<T> collection, CachedData cached, bool insertWithDivision = true) where T : class
        {
            foreach (var item in collection)
            {
                SetDefaultValues(item, cached);
            }

            if (insertWithDivision)
            {
                int maxResultsToInsertInDatabase = 10; //ConfigSettings.MaxResultsToInsertInDatabase;
                for (int i = 0; i < collection.Count; i = i + maxResultsToInsertInDatabase)
                {
                    List<T> insertList = collection.Skip(i).Take(maxResultsToInsertInDatabase).ToList();
                    _db.BulkInsertRecords(insertList);
                }
            }
            else
            {
                _db.BulkInsertRecords(collection);
            }
        }

        public void DeleteEntity<T>(long id) where T : class
        {
            string primaryKeyName = GetPrimaryKeyName<T>();
            _db.Delete<T>("WHERE " + primaryKeyName + "=@0", id);
        }


        public object ExecQuery(string query)
        {
            return _db.Execute(query);
        }


        public object GetScalar(string spname, List<SearchValueData> searchParam = null)
        {
            return _db.ExecuteScalar<object>(GetSPString(spname, searchParam));
        }

        public object GetScalar(string sqlquery)
        {
            return _db.ExecuteScalar<object>(sqlquery);
        }

        public int GetEntityCount<T>(List<SearchValueData> searchParam = null, string customWhere = "") where T : class, new()
        {
            return _db.ExecuteScalar<int>(GetFilterString<T>(searchParam, "", "", customWhere, true));
        }

        private string GetFilterString<T>(List<SearchValueData> searchParam, string sortIndex = "", string sortDirection = "", string customWhere = "", bool getCount = false) where T : class, new()
        {
            string select = (getCount ? "SELECT COUNT(*) FROM " : "SELECT * FROM ");
            return GetFilterSQLString<T>(searchParam, sortIndex, sortDirection, customWhere, !getCount, select);
        }

        private string GetFilterSQLString<T>(List<SearchValueData> searchParam, string sortIndex, string sortDirection, string customWhere,
                                              bool allowSort, string select) where T : class, new()
        {
            T item = new T();
            string where = "";
            string tableName = GetTableName<T>();
            select += tableName;

            if (searchParam != null && searchParam.Count > 0)
            {
                where += " WHERE 1=1";
                foreach (SearchValueData val in searchParam)
                {
                    string value = val.Value.Replace("'", "''");
                    PropertyInfo propertyInfo = item.GetType().GetProperty(val.Name);
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        if (val.IsEqual)
                            where += " AND " + val.Name + " = '" + value + "'";
                        else if (val.IsNotEqual)
                            where += " AND " + val.Name + " != '" + value + "'";
                        else
                            where += " AND " + val.Name + " LIKE '%" + value + "%'";
                    }
                    else if (propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(bool?))
                    {
                        where += " AND " + val.Name + "=" + value;
                    }
                    else if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?) ||
                             propertyInfo.PropertyType == typeof(long?) || propertyInfo.PropertyType == typeof(long))
                    {
                        if (val.IsNotEqual)
                            where += " AND " + val.Name + "!=" + value;
                        else
                            where += " AND " + val.Name + "=" + value;
                    }
                    else if (propertyInfo.PropertyType == typeof(DateTime))
                    {
                        if (searchParam.Count(a => a.Name == val.Name) > 1)
                        {
                            where += " AND ((" + val.Name + " BETWEEN " +
                                     searchParam.FirstOrDefault(a => a.Name == val.Name).Value + " AND " +
                                     searchParam.LastOrDefault(a => a.Name == val.Name).Value + ") OR (" + val.Name +
                                     " BETWEEN " + searchParam.LastOrDefault(a => a.Name == val.Name).Value +
                                     " AND " + searchParam.FirstOrDefault(a => a.Name == val.Name).Value + "))";
                        }
                        else
                        {
                            where += " AND " + val.Name + "='" + value + "'";
                        }
                    }
                }
            }

            where += customWhere != ""
                         ? string.IsNullOrEmpty(where) ? " WHERE (" + customWhere + ")" : " AND (" + customWhere + ")"
                         : "";

            if (sortIndex != "")
                where += " ORDER BY " + sortIndex + " " + sortDirection;
            else if (!string.IsNullOrEmpty(GetSortKeyName<T>()) && !string.IsNullOrEmpty(GetSortDirection<T>()) && allowSort)
            {
                where += " ORDER BY " + GetSortKeyName<T>() + " " + GetSortDirection<T>();
            }

            return select + where.Replace("1=1 AND", "").Replace("1=1", "").Replace("@", "@@");
        }

        public T GetEntity<T>(long id) where T : class, new()
        {
            T item = new T();
            string tableName = GetTableName<T>();
            string primaryKeyName = GetPrimaryKeyName<T>();
            return _db.SingleOrDefault<T>("SELECT * FROM " + tableName + " WHERE " + primaryKeyName + "=" + id) ?? item;
        }

        public T GetEntity<T>(string spname, List<SearchValueData> searchParam = null) where T : class, new()
        {
            return GetEntityList<T>(spname, searchParam).SingleOrDefault();
        }

        public T GetEntity<T>(List<SearchValueData> searchParam = null, string customWhere = "", string sortIndex = "", string sortDirection = "") where T : class, new()
        {
            return _db.SingleOrDefault<T>(GetFilterString<T>(searchParam, sortIndex, sortDirection, customWhere));
        }

        public List<T> GetEntityList<T>(string spname, List<SearchValueData> searchParam = null) where T : class, new()
        {
            return _db.Fetch<T>(";" + GetSPString(spname, searchParam));
        }

        public List<T> Orbeon_GetEntityList<T>(string spname, List<SearchValueData> searchParam = null) where T : class, new()
        {
            return _odb.Fetch<T>(";" + GetSPString(spname, searchParam));
        }

        public List<dynamic> GetDynamicList(string spname, List<SearchValueData> searchParam = null)
        {
            return _db.Fetch<dynamic>(";" + GetSPString(spname, searchParam));
        }

        public List<T> GetEntityList<T>(List<SearchValueData> searchParam = null, string customWhere = "", string sortIndex = "", string sortDirection = "") where T : class, new()
        {
            return _db.Query<T>(GetFilterString<T>(searchParam, sortIndex, sortDirection, customWhere)).ToList<T>();
        }

        public List<T> GetEntityList<T>(string sqlQuery) where T : class, new()
        {
            return _db.Query<T>(sqlQuery).ToList<T>();
        }

        public Page<T> GetEntityPageList<T>(List<SearchValueData> searchParam, int? pageSizeCount, int pageIndex, string sortIndex, string sortDirection, string customWhere = "") where T : class, new()
        {
            int pageSize = Convert.ToInt32(ConfigSettings.PageSize);

            if (pageSizeCount != null)
                pageSize = pageSizeCount.Value;

            if (sortIndex == "")
                sortIndex = GetSortKeyName<T>();

            if (sortDirection == "")
                sortDirection = GetSortDirection<T>();

            Page<T> pageList = _db.Page<T>(pageIndex, pageSize, GetFilterString<T>(searchParam, sortIndex, sortDirection, customWhere));

            //To get last page data if current page index is greater than last page
            if (pageList.Items.Count == 0 && pageList.TotalPages > 0 && pageList.TotalItems > 0)
                pageList = _db.Page<T>(pageList.TotalPages, pageSize, GetFilterString<T>(searchParam, sortIndex, sortDirection, customWhere));

            return pageList;
        }

        public Page<T> GetEntityPageList<T>(string spname, List<SearchValueData> searchParam, int? pageSizeCount, int pageIndex, string sortIndex, string sortDirection, string customWhere = "") where T : class, new()
        {
            int pageSize = Convert.ToInt32(ConfigSettings.PageSize);

            if (pageSizeCount != null)
                pageSize = pageSizeCount.Value;

            if (sortIndex == "")
                sortIndex = GetSortKeyName<T>();

            if (sortDirection == "")
                sortDirection = GetSortDirection<T>();

            return _db.Page<T>(pageIndex, pageSize, GetSPString(spname, searchParam));
        }

        public Page<T> GetPageList<T>(List<T> items, int pageIndex, int pageSize, object context = null, string Count = "Count") where T : class, new()
        {
            int count = 0;
            if (items != null && items.Count > 0)
            {
                object obj = items.First();
                PropertyInfo prop = obj.GetType().GetProperty(Count);
                count = Convert.ToInt32(prop.GetValue(obj));
            }

            return GetPageInStoredProcResultSet(pageIndex, pageSize, count, items, context);
        }

        private string GetFilterString<T>(List<SearchValueData> searchParam, string sortIndex = "", string sortDirection = "", string customWhere = "") where T : class, new()
        {
            T item = new T();
            string tableName = GetTableName<T>();
            string select = "SELECT * FROM " + tableName;
            string where = "";

            if (searchParam != null && searchParam.Count > 0)
            {
                where += " WHERE 1=1";
                foreach (SearchValueData val in searchParam)
                {
                    string value = val.Value.Replace("'", "''");
                    PropertyInfo propertyInfo = item.GetType().GetProperty(val.Name);
                    var propertyType = propertyInfo.PropertyType;
                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propertyType = propertyType.GetGenericArguments()[0];
                    }

                    if (propertyType == typeof(string))
                    {
                        if (val.IsEqual)
                            where += " AND " + val.Name + " = '" + value + "'";
                        else if (val.IsNotEqual)
                            where += " AND " + val.Name + " != '" + value + "'";
                        else
                            where += " AND " + val.Name + " LIKE '%" + value + "%'";
                    }
                    else if (propertyType == typeof(bool))
                    {
                        where += " AND " + val.Name + "='" + value.ToLower() + "'";
                    }
                    else if (propertyType == typeof(int) || propertyType == typeof(long) || propertyType == typeof(decimal) || propertyType == typeof(float))
                    {
                        if (val.IsNotEqual)
                            where += " AND " + val.Name + "!=" + value;
                        else
                            where += " AND " + val.Name + "=" + value;
                    }
                    else if (propertyType == typeof(DateTime))
                    {
                        if (searchParam.Count(a => a.Name == val.Name) > 1)
                        {
                            where += " AND ((" + val.Name + " BETWEEN " + searchParam.First(a => a.Name == val.Name).Value + " AND " + searchParam.Last(a => a.Name == val.Name).Value + ") OR (" + val.Name + " BETWEEN " + searchParam.Last(a => a.Name == val.Name).Value + " AND " + searchParam.First(a => a.Name == val.Name).Value + "))";
                        }
                        else
                        {
                            where += " AND " + val.Name + "='" + value + "'";
                        }
                    }
                }
            }

            where += customWhere != "" ? string.IsNullOrEmpty(where) ? " WHERE (" + customWhere + ")" : " AND (" + customWhere + ")" : "";

            if (sortIndex != "")
                where += " ORDER BY " + sortIndex + " " + sortDirection;

            return select + where.Replace("1=1 AND", "").Replace("1=1", "").Replace("@", "@@");
        }

        private string GetSPString(string spname, List<SearchValueData> searchParam)
        {
            string sp = string.Format("EXEC {0}", spname);

            if (searchParam != null)
            {
                sp = searchParam.Aggregate(sp,
                    (current, data) =>
                        current +
                        (string.IsNullOrWhiteSpace(data.Value)
                            ? string.Format(" @@{0} = NULL,", data.Name)
                            : string.Format(" @@{0} = N'{1}',", data.Name, data.Value.Replace("@", "@@").Replace("'", "''"))));
                //sp = searchParam.Aggregate(sp,
                //    (current, searchValueData) =>
                //        current +
                //        string.Format(" @@{0} = N'{1}',", searchValueData.Name,
                //            searchValueData.Value == null ? searchValueData.Value : searchValueData.Value.Replace("@", "@@").Replace("'", "''")));
                if (searchParam.Any())
                    sp = sp.TrimEnd(',');
            }
            return sp;
        }

        protected string GetSortKeyName<T>()
        {
            return (typeof(T).GetCustomAttributes(typeof(SortAttribute), true)[0] as SortAttribute).KeyValue;
        }

        protected string GetStoreProcedureName<T>()
        {
            return (typeof(T).GetCustomAttributes(typeof(StoreProcedureAttribute), true)[0] as StoreProcedureAttribute).StoreProcedureName;
        }

        protected string GetSortDirection<T>()
        {
            return (typeof(T).GetCustomAttributes(typeof(SortAttribute), true)[0] as SortAttribute).DirectionValue;
        }

        protected string GetTableName<T>()
        {
            return (typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true)[0] as TableNameAttribute).Value;
        }

        protected string GetPrimaryKeyName<T>()
        {
            return (typeof(T).GetCustomAttributes(typeof(PrimaryKeyAttribute), true)[0] as PrimaryKeyAttribute).Value;
        }

        public Page<T> GetPageInStoredProcResultSet<T>(int pageIndex, int pageSize, int count, List<T> itemsList, object context = null) where T : class, new()
        {
            var result = new Page<T>
            {
                CurrentPage = pageIndex,
                ItemsPerPage = pageSize,
                TotalItems = count,
            };
            result.TotalPages = result.TotalItems / pageSize;

            if ((result.TotalItems % pageSize) != 0)
                result.TotalPages++;
            result.Items = itemsList;
            result.Context = context;
            return result;
        }

        #region Add For Generic List and Search

        public ServiceResponse GetPageRecords<T>(int pageSize, int pageIndex, string sortIndex, string sortDirection, SearchModel searchModel = null) where T : class, new()
        {
            ServiceResponse response = new ServiceResponse();
            if (searchModel != null && !string.IsNullOrEmpty(searchModel.Value))
                searchModel.Value = searchModel.Value.Replace("'", "''");

            string spName = GetStoreProcedureName<T>();

            List<SearchValueData> searchParam = new List<SearchValueData>();

            if (pageSize != Constants.AllRecordsConstant && pageIndex != Constants.AllRecordsConstant)
            {
                searchParam.Add(new SearchValueData
                {
                    Name = "FromIndex",
                    Value = Convert.ToString(((pageIndex - 1) * pageSize) + 1)
                });
                searchParam.Add(new SearchValueData { Name = "ToIndex", Value = Convert.ToString(pageIndex * pageSize) });
            }
            else
            {
                searchParam.Add(new SearchValueData
                {
                    Name = "FromIndex",
                    Value = Convert.ToString(Constants.AllRecordsConstant)
                });
                searchParam.Add(new SearchValueData
                {
                    Name = "ToIndex",
                    Value = Convert.ToString(Constants.AllRecordsConstant)
                });
            }

            searchParam.Add(new SearchValueData { Name = "SortExpression", Value = string.IsNullOrEmpty(sortIndex) ? GetSortKeyName<T>() : Convert.ToString(sortIndex) });

            searchParam.Add(new SearchValueData { Name = "SortType", Value = string.IsNullOrEmpty(sortDirection) ? GetSortDirection<T>() : Convert.ToString(sortDirection) });

            string customWhere = (searchModel != null && !string.IsNullOrEmpty(searchModel.Value)) ? "Where (1=1)" + GetCustomWhere<T>(searchModel) : "";

            searchParam.Add(new SearchValueData { Name = "CustomWhere", Value = customWhere });

            List<T> records = GetEntityList<T>(spName, searchParam);

            int count = 0;
            if (records != null && records.Count > 0)
            {
                object obj = records.First();
                PropertyInfo prop = obj.GetType().GetProperty("Count");
                count = Convert.ToInt32(prop.GetValue(obj));
            }

            Page<T> pageRecords = GetPageInStoredProcResultSet(pageIndex, pageSize, count, records);
            response.Data = pageRecords;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetPageRecords<T>(int pageSize, int pageIndex, string sortIndex, string sortDirection, List<SearchModel> searchModelList, string customFilters = null) where T : class, new()
        {
            ServiceResponse response = new ServiceResponse();
            //if (searchModel != null && !string.IsNullOrEmpty(searchModel.Value))
            //    searchModel.Value = searchModel.Value.Replace("'", "''");

            string spName = GetStoreProcedureName<T>();

            List<SearchValueData> searchParam = new List<SearchValueData>();

            if (pageSize != Constants.AllRecordsConstant && pageIndex != Constants.AllRecordsConstant)
            {
                searchParam.Add(new SearchValueData
                {
                    Name = "FromIndex",
                    Value = Convert.ToString(((pageIndex - 1) * pageSize) + 1)
                });
                searchParam.Add(new SearchValueData { Name = "ToIndex", Value = Convert.ToString(pageIndex * pageSize) });
            }
            else
            {
                searchParam.Add(new SearchValueData
                {
                    Name = "FromIndex",
                    Value = Convert.ToString(Constants.AllRecordsConstant)
                });
                searchParam.Add(new SearchValueData
                {
                    Name = "ToIndex",
                    Value = Convert.ToString(Constants.AllRecordsConstant)
                });
            }

            searchParam.Add(new SearchValueData { Name = "SortExpression", Value = string.IsNullOrEmpty(sortIndex) ? GetSortKeyName<T>() : Convert.ToString(sortIndex) });

            searchParam.Add(new SearchValueData { Name = "SortType", Value = string.IsNullOrEmpty(sortDirection) ? GetSortDirection<T>() : Convert.ToString(sortDirection) });

            string customWhere = "Where (1=1)";

            if (!string.IsNullOrEmpty(customFilters))
            {
                customWhere += "and (" + customFilters + ")";
            }
            foreach (var searchModel in searchModelList)
            {
                customWhere += (searchModel != null && !string.IsNullOrEmpty(searchModel.Value)) ? GetCustomWhere<T>(searchModel) : "";
            }

            searchParam.Add(new SearchValueData { Name = "CustomWhere", Value = customWhere });

            List<T> records = GetEntityList<T>(spName, searchParam);

            int count = 0;
            if (records != null && records.Count > 0)
            {
                object obj = records.First();
                PropertyInfo prop = obj.GetType().GetProperty("Count");
                count = Convert.ToInt32(prop.GetValue(obj));
            }

            Page<T> pageRecords = GetPageInStoredProcResultSet(pageIndex, pageSize, count, records);
            response.Data = pageRecords;
            response.IsSuccess = true;
            return response;
        }

        public string GetCustomWhere<T>(SearchModel searchModel) where T : class, new()
        {
            string customWhere = "";

            if (string.IsNullOrEmpty(searchModel.Name))
            {
                foreach (PropertyInfo prop in typeof(T).GetProperties().Where(p => p.IsDefined(typeof(SearchAttribute), false)))
                {
                    searchModel.Name = prop.Name;
                    if (string.IsNullOrEmpty(customWhere))
                        customWhere = GetCustomWhereStringWithPassedOperator(searchModel);
                    else
                        customWhere += " OR " + GetCustomWhereStringWithPassedOperator(searchModel);
                }
                if (!string.IsNullOrEmpty(customWhere))
                    customWhere = " (" + customWhere + ")";
            }
            else
                customWhere = GetCustomWhereStringWithPassedOperator(searchModel);
            return string.IsNullOrEmpty(customWhere) ? customWhere : " AND " + customWhere;
        }

        public string GetCustomWhereStringWithPassedOperator(SearchModel searchModel)
        {
            string customWhere = "";
            switch (searchModel.OperatorId)
            {
                case (int)Common.SearchOperator.EqualTo:
                    customWhere = searchModel.Name + " = '" + searchModel.Value + "'";
                    break;

                case (int)Common.SearchOperator.NotEqualTo:
                    customWhere = searchModel.Name + " != '" + searchModel.Value + "'";
                    break;

                case (int)Common.SearchOperator.BeginsWith:
                    customWhere = searchModel.Name + " LIKE '" + searchModel.Value + "%'";
                    break;

                case (int)Common.SearchOperator.EndsWith:
                    customWhere = searchModel.Name + " LIKE '%" + searchModel.Value + "'";
                    break;

                case (int)Common.SearchOperator.Contains:
                    customWhere = searchModel.Name + " LIKE '%" + searchModel.Value.Replace(" ", "%") + "%'";
                    break;

                case (int)Common.SearchOperator.DoesNotContains:
                    customWhere = searchModel.Name + " NOT LIKE '%" + searchModel.Value + "%'";
                    break;

                case (int)Common.SearchOperator.GreaterThan:
                    customWhere = searchModel.Name + " > '" + searchModel.Value + "'";
                    break;

                case (int)Common.SearchOperator.LessThan:
                    customWhere = searchModel.Name + " < '" + searchModel.Value + "'";
                    break;

                default:
                    customWhere = searchModel.Name + " LIKE '%" + searchModel.Value.Replace(" ", "%") + "%'";
                    break;
            }
            return customWhere;
        }

        public ServiceResponse DeleteRecord<T>(long id, string successMessage = "Record deleted successfully") where T : class, new()
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            searchList.Add(new SearchValueData { Name = "PrimaryKeyID", Value = Convert.ToString(id) });
            searchList.Add(new SearchValueData { Name = "PrimaryKeyName", Value = GetPrimaryKeyName<T>() });
            searchList.Add(new SearchValueData { Name = "TableName", Value = GetTableName<T>() });

            int status = Convert.ToInt32(GetScalar("DeleteRecord", searchList));
            if (status == 1)
            {
                response.Message = successMessage;
                response.IsSuccess = true;
            }

            if (status == 0)
            {
                response.Message = "Sorry, record is used in the system. You can't delete the record.";
                response.IsSuccess = false;
            }
            return response;
        }

        public ServiceResponse DeleteRecord<T>(string customWhere, string successMessage = "Record deleted successfully") where T : class, new()
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            //searchList.Add(new SearchValueData { Name = "PrimaryKeyID", Value = Convert.ToString(id) });
            //searchList.Add(new SearchValueData { Name = "PrimaryKeyName", Value = GetPrimaryKeyName<T>() });
            searchList.Add(new SearchValueData { Name = "CustomWhere", Value = customWhere });
            searchList.Add(new SearchValueData { Name = "TableName", Value = GetTableName<T>() });

            int status = Convert.ToInt32(GetScalar("DeleteRecord", searchList));
            if (status == 1)
            {
                response.Message = successMessage;
                response.IsSuccess = true;
            }

            if (status == 0)
            {
                response.Message = "Sorry, record is used in the system. You can't delete the record.";
                response.IsSuccess = false;
            }
            return response;
        }

        #endregion Add For Generic List and Search

        #region For Multiple ResutlSet

        /// <summary>
        /// Perform a multi-results set query
        /// </summary>
        /// <param name="query">The SQL query to be executed</param>
        /// <param name="args">Arguments to any embedded parameters in the SQL</param>
        /// <typeparam name="T">The Type representing Model in which Result Set Data will be Bind</typeparam>
        /// <returns>Return Object of Generic Type T</returns>
        public T GetMultipleEntity<T>(string query, params object[] args) where T : new()
        {
            GridReader grd = _db.QueryMultiple(query, args);
            T obj = new T();
            Type returnType = typeof(T);
            PropertyInfo[] propertyInfo = returnType.GetProperties();
            foreach (var info in propertyInfo)
            {
                bool isFirstorDefault = false;
                Type t;
                if (info.PropertyType.GetGenericArguments().Any())
                {
                    t = info.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    isFirstorDefault = true;
                    t = info.PropertyType;
                }
                var value =
                (typeof(BaseDataProvider).GetMethod("GetValueOf")
                .MakeGenericMethod(t)
                .Invoke(null, new object[] { grd, isFirstorDefault }));
                info.SetValue(obj, value, null);
            }
            _db.CloseSharedConnection();
            return obj;
        }

        public T GetMultipleEntity<T>(string spname, List<SearchValueData> searchParam) where T : new()
        {
            GridReader grd = _db.QueryMultiple(GetSPString(spname, searchParam));
            T obj = new T();
            Type returnType = typeof(T);
            PropertyInfo[] propertyInfo = returnType.GetProperties();
            foreach (var info in propertyInfo)
            {
                if (!info.CanWrite)
                {
                    continue;
                }
                bool isFirstorDefault = false;
                Type t;
                if (info.PropertyType.GetGenericArguments().Any())
                {
                    t = info.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    isFirstorDefault = true;
                    t = info.PropertyType;
                }
                var value =
                (typeof(BaseDataProvider).GetMethod("GetValueOf")
                .MakeGenericMethod(t)
                .Invoke(null, new object[] { grd, isFirstorDefault }));
                info.SetValue(obj, value, null);
            }
            _db.CloseSharedConnection();
            return obj;
        }

        /// <summary>
        /// Reads from a GridReader, Either List of Type T or Object if Type T
        /// </summary>
        /// <param name="reader">GridReader from which result set will be read.</param>
        /// <param name="isFirstorDefault">If true function returns first row or default value as object of type T else return List of T"></typeparam></param>
        /// <typeparam name="T">The Type representing a row in the result set</typeparam>
        /// <returns>An enumerable collection of result records</returns>
        public static object GetValueOf<T>(GridReader reader, bool isFirstorDefault)
        {
            IEnumerable<T> lst = reader.Read<T>();
            if (isFirstorDefault)
                return lst.FirstOrDefault();
            return lst.ToList();
        }

        /// <summary>
        /// Execute Query And Return DataSet (This Support Only for single Table ResultSet)
        /// </summary>
        /// <param name="sql">The SQL query to be executed</param>
        /// <param name="args">Arguments to any embedded parameters in the SQL</param>
        /// <returns>Dataset With Single Table Only</returns>
        public DataSet GetDataSet(string query, params object[] args)
        {
            return _db.QueryForDataSet(query, args);
        }

        #endregion For Multiple ResutlSet

        //public ServiceResponse<T> IsNullRequest<T>(ServiceRequest<T> request) where T : class
        //{
        //    ServiceResponse<T> response=new ServiceResponse<T>();
        //    if (request == null)
        //    {
        //        response.status = Resource.Error;
        //        response.code = Constants.StatusCode401;
        //        response.message = Resource.MSG_KeyMissingInvalid;
        //        return response;
        //    }
        //    return response;
        //}

        #region ValidateEntity
        // TODO: Also add validateObject Attribute
        public ApiResponse ValidateModel<T>(T requestData)
           where T : class, new()
        {
            ApiResponse response = new ApiResponse();

            var errorList = new List<ErrorList>();

            if (requestData != null)
            {
                var validateUser = ValidateEntity(requestData);

                if (validateUser.HasError)
                {
                    foreach (ValidationResult validationResult in validateUser.Errors)
                    {
                        var validationError = new ErrorList();
                        List<string> list = validationResult.ErrorMessage.Split('|').ToList();
                        if (list.Count > 0)
                        {
                            validationError.Field = validationResult.MemberNames.FirstOrDefault();
                            //validationError.ErrorCode = list[0];
                            validationError.Message = list[0];
                            errorList.Add(validationError);
                        }
                    }

                    if (errorList.Count > 0)
                    {
                        response.Code = StatusCode.BadRequest;
                        response.Message = Resource.ValidationFailed;
                        response.Errors = errorList;
                        return response;
                    }
                }
            }
            else
            {
                response.Code = StatusCode.BadRequest;
                response.Message = Resource.BadRequest;
                response.Errors = errorList;
                return response;
            }
            response.Code = StatusCode.Ok;
            return response;
        }

        public static EntityValidationResult ValidateEntity<T>(T entity) where T : class, new()
        {
            return new EntityValidator<T>().Validate(entity);
        }

        public class EntityValidator<T> where T : class, new()
        {
            public EntityValidationResult Validate(T entity)
            {
                var validationResults = new List<ValidationResult>();

                var vc = new ValidationContext(entity, null, null);
                var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

                return new EntityValidationResult(validationResults);
            }
        }

        #endregion ValidateEntity
        
        #region Elmah Logging

        public void ElmahError(Exception ex)
        {
            SqlErrorLog errorLog = new SqlErrorLog(_db.Connection.ConnectionString) { ApplicationName = "TTS Windows Service" };
            errorLog.Log(new Error(ex));
        }

        #endregion

        public string GetTimeZone()
        {
            var timeZone = (string)GetScalar(StoredProcedure.GetTimeZone);
            timeZone = (timeZone == null) ? Constants.EasternStandardTime : timeZone;
            return timeZone;
        }

        public bool GetPatientResignatureFlag()
        {
            return (bool)GetScalar(StoredProcedure.GetPatientResignatureFlag);
        }

    }
}