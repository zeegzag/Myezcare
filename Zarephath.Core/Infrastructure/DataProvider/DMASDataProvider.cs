using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class DMASDataProvider : BaseDataProvider, IDMASDataProvider
    {
        #region DMAS 97
        public ServiceResponse GetDmas97Ab(Dmas97Model dmas)
        {
            ServiceResponse response = new ServiceResponse();
            long Dmas97ID = Convert.ToInt64(dmas.Dmas97ID);
            long ReferralID = !string.IsNullOrEmpty(dmas.EncryptedReferralID) ? Convert.ToInt64(Crypto.Decrypt(dmas.EncryptedReferralID)) : 0;
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "Dmas97ID", Value = Convert.ToString(Dmas97ID) });
                searchParam.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(ReferralID) });
                DmasModel model = GetMultipleEntity<DmasModel>(StoredProcedure.GetDMAS97ABdetail, searchParam);

                if (model.Dmas97AbModel == null)
                {
                    response.IsSuccess = true;
                    response.Data = model;
                }
                else
                {

                    DmasModel jsonData = JsonConvert.DeserializeObject<DmasModel>(model.Dmas97AbModel.JsonData);
                    jsonData.Dmas97AbModel.Dmas97ID = Dmas97ID;
                    response.IsSuccess = true;
                    response.Data = jsonData;
                }

                return response;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse GenerateDmas97AB(long Dmas97ID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "Dmas97ID", Value = Convert.ToString(Dmas97ID) });
                DmasModel model = GetMultipleEntity<DmasModel>(StoredProcedure.GetDMAS97ABdetail, searchParam);
                
                if (model.Dmas97AbModel==null)
                {
                    response.IsSuccess = true;
                    response.Data = model;
                }
                else
                {

                    DmasModel jsonData = JsonConvert.DeserializeObject<DmasModel>(model.Dmas97AbModel.JsonData);
                    jsonData.Dmas97AbModel.Dmas97ID = Dmas97ID;
                    response.IsSuccess = true;
                    response.Data = jsonData;
                }

                return response;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse AddUpdateDmas97Ab(DmasModel dmas, long loggedInUserId)
        {
             string jsonData = Common.SerializeObject<DmasModel>(dmas);
            long ReferralID = !string.IsNullOrEmpty(dmas.Dmas97AbModel.EncryptedReferralID) ? Convert.ToInt64(Crypto.Decrypt(dmas.Dmas97AbModel.EncryptedReferralID)) : 0;

            var response = new ServiceResponse();
            try
            {
                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "Dmas97Id", Value = Convert.ToString(dmas.Dmas97AbModel.Dmas97ID) });
                dataList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(ReferralID) });
                dataList.Add(new SearchValueData { Name = "jsonData", Value = Convert.ToString(jsonData) });
                int data = (int)GetScalar(StoredProcedure.Dmas97AbAddUpdate, dataList);
                if (data == 1)
                {
                    response.IsSuccess = true;
                    response.Message = dmas.Dmas97AbModel.Dmas97ID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.DMAS_97_AB) :
                        string.Format(Resource.RecordCreatedSuccessfully, Resource.DMAS_97_AB);
                    return response;
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse Dmas97AbList(long ReferralID)
        {
            var response = new ServiceResponse();
            var SearchList = new List<SearchValueData>();
            SearchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(ReferralID) });
            SetDmas97AbListPage list = GetMultipleEntity<SetDmas97AbListPage>(StoredProcedure.SetDmas97AbListPage, SearchList);
            response.Data = list;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse DeleteDmas97Ab(Dmas97AbModel dmas)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "Dmas97ID", Value = Convert.ToString(dmas.Dmas97ID) });
                var data = GetScalar(StoredProcedure.DeleteDMAS97AB, searchParam);             
                    response.IsSuccess = true;
                    response.Data = data;
                    return response;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse CloneDataDMAS97AB(Dmas97CloneModel dmas, long loggedInUserId)
        {
            long ReferralID = !string.IsNullOrEmpty(dmas.EncryptedReferralID) ? Convert.ToInt64(Crypto.Decrypt(dmas.EncryptedReferralID)) : 0;
            var response = new ServiceResponse();
            try
            {
                if (dmas.Dmas97ID != 0)
                 {
                    dmas.Dmas97ID = 0;
                    var dataList = new List<SearchValueData>();
                    dataList.Add(new SearchValueData { Name = "Dmas97ID", Value = Convert.ToString(dmas.Dmas97ID) });
                    dataList.Add(new SearchValueData { Name = "JsonData", Value = Convert.ToString(dmas.JsonData) });
                    dataList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(ReferralID) });
                    int data = (int)GetScalar(StoredProcedure.Dmas97AbAddUpdate, dataList);
                    if (data == 1)
                    {
                        response.IsSuccess = true;
                        response.Message = dmas.Dmas97ID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.DMAS_97_AB) :
                            string.Format(Resource.RecordCreatedSuccessfully, Resource.DMAS_97_AB);
                        return response;
                    }
                }                
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        #endregion

        #region DMAS 99
        public ServiceResponse GetDmas99Form(Dmas99Models dmas)
        {
            ServiceResponse response = new ServiceResponse();
            long Dmas99ID = Convert.ToInt64(dmas.Dmas99ID);
            long ReferralID = !string.IsNullOrEmpty(dmas.EncryptedReferralID) ? Convert.ToInt64(Crypto.Decrypt(dmas.EncryptedReferralID)) : 0;
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "Dmas99ID", Value = Convert.ToString(Dmas99ID) });
                searchParam.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(ReferralID) });
                GetDmas99Model model = GetMultipleEntity<GetDmas99Model>(StoredProcedure.GetDMAS99detail, searchParam);

                if (model.Dmas99Model == null)
                {
                    response.IsSuccess = true;
                    response.Data = model;
                    return response;
                }
                else
                {
                    GetDmas99Model jsonData = JsonConvert.DeserializeObject<GetDmas99Model>(model.Dmas99Model.JsonData);
                    jsonData.Dmas99Model.Dmas99ID = Dmas99ID;
                    jsonData.Dmas99Model.ReferralID = ReferralID;
                    response.IsSuccess = true;
                    response.Data = jsonData;
                }

                return response;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse GenerateDMAS99Form(long Dmas99ID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "Dmas99ID", Value = Convert.ToString(Dmas99ID) });
                GetDmas99Model model = GetMultipleEntity<GetDmas99Model>(StoredProcedure.GetDMAS99detail, searchParam);

                if (model.Dmas99Model == null)
                {
                    response.IsSuccess = true;
                    response.Data = model;
                    return response;
                }
                else
                {
                    GetDmas99Model jsonData = JsonConvert.DeserializeObject<GetDmas99Model>(model.Dmas99Model.JsonData);
                    jsonData.Dmas99Model.Dmas99ID = Dmas99ID;
                    response.IsSuccess = true;
                    response.Data = jsonData;
                }

                return response;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse AddUpdateDmas99(GetDmas99Model dmas, long loggedInUserId)
        {
            long ReferralID = !string.IsNullOrEmpty(dmas.Dmas99Model.EncryptedReferralID) ? Convert.ToInt64(Crypto.Decrypt(dmas.Dmas99Model.EncryptedReferralID)) : 0;
            string jsonData = Common.SerializeObject<GetDmas99Model>(dmas);

            var response = new ServiceResponse();
            try
            {
                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "Dmas99ID", Value = Convert.ToString(dmas.Dmas99Model.Dmas99ID) });
                dataList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(ReferralID) });
                dataList.Add(new SearchValueData { Name = "jsonData", Value = Convert.ToString(jsonData) });
                int data = (int)GetScalar(StoredProcedure.DMAS99AddUpdate, dataList);
                if (data == 1)
                {
                    response.IsSuccess = true;
                    response.Message = dmas.Dmas99Model.Dmas99ID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.DMAS_99) :
                        string.Format(Resource.RecordCreatedSuccessfully, Resource.DMAS_99);
                    return response;
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse Dmas99ListPage(long ReferralID)
        {
            var response = new ServiceResponse();
            var SearchList = new List<SearchValueData>();
            SearchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(ReferralID) });
            Dmas99ListPage list = GetMultipleEntity<Dmas99ListPage>(StoredProcedure.Dmas99List, SearchList);
            response.Data = list;
            return response;
        }

        public ServiceResponse DeleteDmas99Form(Dmas99Model dmas)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "Dmas99ID", Value = Convert.ToString(dmas.Dmas99ID) });
                var data = GetScalar(StoredProcedure.DeleteDMAS99, searchParam);
                response.IsSuccess = true;
                response.Data = data;
                return response;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse CloneDataDMAS99(Dmas99CloneModel dmas, long loggedInUserId)
        {
            long ReferralID = !string.IsNullOrEmpty(dmas.EncryptedReferralID) ? Convert.ToInt64(Crypto.Decrypt(dmas.EncryptedReferralID)) : 0;
            var response = new ServiceResponse();
            try
            { 
                if (dmas.Dmas99ID != 0)
                {
                    dmas.Dmas99ID = 0;
                    var dataList = new List<SearchValueData>();
                    dataList.Add(new SearchValueData { Name = "Dmas99ID", Value = Convert.ToString(dmas.Dmas99ID) });
                    dataList.Add(new SearchValueData { Name = "JsonData", Value = Convert.ToString(dmas.JsonData) });
                    dataList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(ReferralID) });
                    int data = (int)GetScalar(StoredProcedure.DMAS99AddUpdate, dataList);
                    if (data == 1)
                    {
                        response.IsSuccess = true;
                        response.Message = dmas.Dmas99ID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.DMAS_99) :
                            string.Format(Resource.RecordCreatedSuccessfully, Resource.DMAS_99);
                        return response;
                    }
                }

        }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        #endregion

        #region CMS 485
        public ServiceResponse GetCms485Form(Cms485AddModel cms)
        {
            ServiceResponse response = new ServiceResponse();
            long Cms485ID = Convert.ToInt64(cms.Cms485ID);
            long ReferralID = !string.IsNullOrEmpty(cms.EncryptedReferralID) ? Convert.ToInt64(Crypto.Decrypt(cms.EncryptedReferralID)) : 0;
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "Cms485ID", Value = Convert.ToString(Cms485ID) });
                searchParam.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(ReferralID) });
                GetCms485Model model = GetMultipleEntity<GetCms485Model>(StoredProcedure.GetCMS485detail, searchParam);

                if (model.Cms485Model == null)
                {
                    response.IsSuccess = true;
                    response.Data = model;
                    return response;
                }
                else
                {
                    GetCms485Model jsonData = JsonConvert.DeserializeObject<GetCms485Model>(model.Cms485Model.JsonData);
                    jsonData.Cms485Model.Cms485ID = Cms485ID;
                    jsonData.Cms485Model.ReferralID = ReferralID;
                    response.IsSuccess = true;
                    response.Data = jsonData;
                }

                return response;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse AddUpdateCms485Form(GetCms485Model cms, long loggedInUserId)
        {
            long ReferralID = !string.IsNullOrEmpty(cms.Cms485Model.EncryptedReferralID) ? Convert.ToInt64(Crypto.Decrypt(cms.Cms485Model.EncryptedReferralID)) : 0;
            string jsonData = Common.SerializeObject<GetCms485Model>(cms);

            var response = new ServiceResponse();
            try
            {
                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "Cms485ID", Value = Convert.ToString(cms.Cms485Model.Cms485ID) });
                dataList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(ReferralID) });
                dataList.Add(new SearchValueData { Name = "jsonData", Value = Convert.ToString(jsonData) });
                int data = (int)GetScalar(StoredProcedure.CMS485AddUpdate, dataList);
                if (data == 1)
                {
                    response.IsSuccess = true;
                    response.Message = cms.Cms485Model.Cms485ID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.CMS_485) :
                        string.Format(Resource.RecordCreatedSuccessfully, Resource.CMS_485);
                    return response;
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse Cms485FormList(long ReferralID)
        {
            var response = new ServiceResponse();
            var SearchList = new List<SearchValueData>();
            SearchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(ReferralID) });
            Cms485ListModel list = GetMultipleEntity<Cms485ListModel>(StoredProcedure.CMS485List, SearchList);
            response.Data = list;
            return response;
        }

        public ServiceResponse DeleteCms485Form(Cms485Model cms)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "Cms485ID", Value = Convert.ToString(cms.Cms485ID) });
                var data = GetScalar(StoredProcedure.DeleteCMS48Form, searchParam);
                response.IsSuccess = true;
                response.Data = data;
                return response;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse CloneCms485Form(Cms485CloneModel cms, long loggedInUserId)
        {
            long ReferralID = !string.IsNullOrEmpty(cms.EncryptedReferralID) ? Convert.ToInt64(Crypto.Decrypt(cms.EncryptedReferralID)) : 0;
            var response = new ServiceResponse();
            try
            {
                if (cms.Cms485ID != 0)
                {
                    cms.Cms485ID = 0;
                    var dataList = new List<SearchValueData>();
                    dataList.Add(new SearchValueData { Name = "Cms485ID", Value = Convert.ToString(cms.Cms485ID) });
                    dataList.Add(new SearchValueData { Name = "JsonData", Value = Convert.ToString(cms.JsonData) });
                    dataList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(ReferralID) });
                    int data = (int)GetScalar(StoredProcedure.CMS485AddUpdate, dataList);
                    if (data == 1)
                    {
                        response.IsSuccess = true;
                        response.Message = cms.Cms485ID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.CMS_485) :
                            string.Format(Resource.RecordCreatedSuccessfully, Resource.CMS_485);
                        return response;
                    }
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse GenerateCms485Form(long Cms485ID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "Cms485ID", Value = Convert.ToString(Cms485ID) });
                GetCms485Model model = GetMultipleEntity<GetCms485Model>(StoredProcedure.GetCMS485detail, searchParam);

                if (model.Cms485Model == null)
                {
                    response.IsSuccess = true;
                    response.Data = model;
                    return response;
                }
                else
                {
                    GetCms485Model jsonData = JsonConvert.DeserializeObject<GetCms485Model>(model.Cms485Model.JsonData);
                    jsonData.Cms485Model.Cms485ID = Cms485ID;
                    response.IsSuccess = true;
                    response.Data = jsonData;
                }

                return response;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

    }
}
