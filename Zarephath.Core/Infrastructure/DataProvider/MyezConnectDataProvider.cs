using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class MyezConnectDataProvider : BaseDataProvider, IMyezConnectDataProvider
    {
        public MyezConnectDataProvider() : base(Constants.MyezcareOrganizationConnectionString)
        {
        }
        //BaseDataProvider baseDataProvider = new BaseDataProvider("MyezcareOrganization");

        public ServiceResponse ViewReleaseNote(long ReleaseNoteID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "ReleaseNoteID", Value = Convert.ToString(ReleaseNoteID), IsEqual = true });
                ReleaseNote releaseNote = GetEntity<ReleaseNote>(searchParam);

                if (releaseNote == null)
                    releaseNote = new ReleaseNote();

                response.IsSuccess = true;
                response.Data = releaseNote;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, e.Message);
            }
            return response;
        }

        public ServiceResponse UpdateSiteCache(string id)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchParam = new List<SearchValueData>();
            searchParam.Add(new SearchValueData { Name = "DomainName", Value = Common.GetSubDomainName(), IsEqual = true });
            OrganizationModel org = GetMultipleEntity<OrganizationModel>(StoredProcedure.GetOrganizationDetails, searchParam);

            //if (org.MyEzcareOrganization == null)
            //    Common.ThrowErrorMessage(Resource.DomainNotExist);

            CacheHelper_MyezCare cache = new CacheHelper_MyezCare();

            if (string.IsNullOrEmpty(id) || id == "1")
            {
                if (org.MyEzcareOrganization == null)
                {
                    cache.RemoveCacheData();
                    CacheHelper ch=new CacheHelper();
                    ch.RemoveCacheData(ch.OrganizationSettingCachedName);
                }
                else
                    cache.AddCacheData(org.MyEzcareOrganization);
            }
            if (string.IsNullOrEmpty(id) || id == "2")
            {
                if (org.ReleaseNote == null)
                    cache.RemoveCacheData(CacheHelperName.ReleaseNote);
                else
                    cache.AddCacheData(org.ReleaseNote, CacheHelperName.ReleaseNote);
            }
            response.IsSuccess = true;
            return response;
        }

    }
}
