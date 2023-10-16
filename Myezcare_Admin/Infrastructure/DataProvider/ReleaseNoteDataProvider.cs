using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Myezcare_Admin.Helpers;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.ViewModel;
using Myezcare_Admin.Resources;
using Myezcare_Admin.Models.Entity;
using System.Net;
using System.IO;
using System.Text;

namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public class ReleaseNoteDataProvider : BaseDataProvider, IReleaseNoteDataProvider
    {

        public ServiceResponse SetAddReleaseNotePage(long ReleaseNoteID)
        {
            var response = new ServiceResponse();
            AddReleaseNoteModel addreleaseNoteModel = new AddReleaseNoteModel();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "ReleaseNoteID", Value = Convert.ToString(ReleaseNoteID),IsEqual=true });
                addreleaseNoteModel.ReleaseNote = GetEntity<ReleaseNote>(searchParam);

                if (addreleaseNoteModel.ReleaseNote == null)
                    addreleaseNoteModel.ReleaseNote = new ReleaseNote();

                response.IsSuccess = true;
                response.Data = addreleaseNoteModel;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, e.Message);
            }
            return response;
        }

        public ServiceResponse SaveReleaseNote(ReleaseNote releaseNote, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "ReleaseNoteID", Value = Convert.ToString(releaseNote.ReleaseNoteID) });
                dataList.Add(new SearchValueData { Name = "Title", Value = releaseNote.Title });
                dataList.Add(new SearchValueData { Name = "Description", Value = releaseNote.Description });
                dataList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(releaseNote.StartDate)});
                dataList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(releaseNote.EndDate) });
                dataList.Add(new SearchValueData { Name = "loggedInUserId", Value = Convert.ToString(loggedInUserId) });
                dataList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });

                int data = (int)GetScalar(StoredProcedure.SaveReleaseNote, dataList);
                if (data == -1)
                {
                    response.Message = Resource.ReleaseNoteAlreadyExists;
                    return response;
                }

                if(data == 1)
                {
                    string url = string.Empty;
                    MyEzcareOrganizationModel myEzcareOrganizationModel = GetMultipleEntity<MyEzcareOrganizationModel>(StoredProcedure.GetOrganizationData);
                    if (myEzcareOrganizationModel.MyEzcareOrganization != null)
                    {
                        url = string.Format(ConfigSettings.PublicSiteUrl + ConfigSettings.UpdateSiteCacheUrl + "2", myEzcareOrganizationModel.MyEzcareOrganization.DomainName);
                    }


                    ServiceResponse res = new ServiceResponse();
                    res = Common.UpdateCache(catchType: Common.CatchType_ReleaseNote);


                    //ServiceResponse res = new ServiceResponse();
                    //try
                    //{
                    //    using (WebClient client = new WebClient())
                    //    {
                    //        var reqparm = new System.Collections.Specialized.NameValueCollection();
                    //        byte[] responsebytes = client.UploadValues(url, "POST", reqparm);
                    //        string responsebody = Encoding.UTF8.GetString(responsebytes);
                    //        res = Common.DeserializeObject<ServiceResponse>(responsebody);
                    //    }
                    //}
                    //catch(Exception ex) {
                    //    res.Message = ex.Message;
                    //}

                    response.IsSuccess = true;
                    response.Message = releaseNote.ReleaseNoteID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.ReleaseNote) :
                        string.Format(Resource.RecordCreatedSuccessfully, Resource.ReleaseNote);
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse SetReleaseNoteListPage()
        {
            ServiceResponse response = new ServiceResponse();
            SetReleaseNoteListPage model = new SetReleaseNoteListPage();

            model.DeleteFilter = Common.SetDeleteFilter();
            model.SearchReleaseNoteListPage = new SearchReleaseNoteListPage() { IsDeleted = 0 };
            response.Data = model;
            return response;
        }

        public ServiceResponse GetReleaseNoteList(SearchReleaseNoteListPage searchReleaseNoteListPage, int pageIndex, int pageSize,string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));
            if (searchReleaseNoteListPage != null)
                SetSearchFilterForReleaseNoteList(searchReleaseNoteListPage, searchList);


            List<ListReleaseNoteModel> totalData = GetEntityList<ListReleaseNoteModel>(StoredProcedure.GetReleaseNoteList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListReleaseNoteModel> releaseNoteList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = releaseNoteList;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse DeleteReleaseNote(SearchReleaseNoteListPage searchReleaseNoteListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                SetSearchFilterForReleaseNoteList(searchReleaseNoteListPage, searchList);

                if (!string.IsNullOrEmpty(searchReleaseNoteListPage.ListOfIdsInCsv))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchReleaseNoteListPage.ListOfIdsInCsv });

                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

                List<ListReleaseNoteModel> totalData = GetEntityList<ListReleaseNoteModel>(StoredProcedure.DeleteReleaseNote, searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListReleaseNoteModel> listReleaseNote = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);

                ServiceResponse res = new ServiceResponse();
                res = Common.UpdateCache(catchType: Common.CatchType_ReleaseNote);

                //try
                //{
                //    string url = string.Empty;
                //    MyEzcareOrganizationModel myEzcareOrganizationModel = GetMultipleEntity<MyEzcareOrganizationModel>(StoredProcedure.GetOrganizationData);
                //    if (myEzcareOrganizationModel.MyEzcareOrganization != null)
                //    {
                //        url = string.Format(ConfigSettings.PublicSiteUrl + ConfigSettings.UpdateSiteCacheUrl + "2", myEzcareOrganizationModel.MyEzcareOrganization.DomainName);
                //    }
                //    using (WebClient client = new WebClient())
                //    {
                //        var reqparm = new System.Collections.Specialized.NameValueCollection();
                //        byte[] responsebytes = client.UploadValues(url, "POST", reqparm);
                //        string responsebody = Encoding.UTF8.GetString(responsebytes);
                //        res = Common.DeserializeObject<ServiceResponse>(responsebody);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    res.Message = ex.Message;
                //}

                response.Data = listReleaseNote;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.ReleaseNote);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }

        private static void SetSearchFilterForReleaseNoteList(SearchReleaseNoteListPage searchReleaseNoteListPage, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(searchReleaseNoteListPage.Title))
                searchList.Add(new SearchValueData { Name = "Title", Value = Convert.ToString(searchReleaseNoteListPage.Title) });
            if (!string.IsNullOrEmpty(searchReleaseNoteListPage.Description))
                searchList.Add(new SearchValueData { Name = "Description", Value = Convert.ToString(searchReleaseNoteListPage.Description) });
            if (searchReleaseNoteListPage.StartDate != null)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(searchReleaseNoteListPage.StartDate) });
            if (searchReleaseNoteListPage.EndDate != null)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(searchReleaseNoteListPage.EndDate) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchReleaseNoteListPage.IsDeleted) });
        }

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
    }
}
