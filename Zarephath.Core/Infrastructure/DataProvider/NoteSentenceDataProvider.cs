using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class NoteSentenceDataProvider : BaseDataProvider, INoteSentenceDataProvider
    {
        #region Add Note Sentence

        public ServiceResponse SetAddNoteSentencePage(long noteSentenceId)
        {
            ServiceResponse response = new ServiceResponse();
            NoteSentence noteSentence=new NoteSentence();
            if (noteSentenceId > 0)
            {
                List<SearchValueData> searchDxCodeParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "NoteSentenceID", Value = Convert.ToString(noteSentenceId)},
                };
               noteSentence = GetEntity<NoteSentence>(searchDxCodeParam)??new NoteSentence();
            }

            response.IsSuccess = true;
            response.Data = noteSentence;
            return response;
        }

        public ServiceResponse AddNoteSentence(NoteSentence noteSentence, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                SaveObject(noteSentence, loggedInUserId);
                response.IsSuccess = true;
                response.Message = noteSentence.NoteSentenceID > 0
                    ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.NoteSentence)
                    : string.Format(Resource.RecordCreatedSuccessfully, Resource.NoteSentence);

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        #endregion Add Note Sentence    

        #region Note Sentence List

        public ServiceResponse SetAddNoteSentenceListPage()
        {
            ServiceResponse response = new ServiceResponse();
            var setNoteSentencePage = new SetNoteSentenceListPage
            {
                DeleteFilter = Common.SetDeleteFilter(),
                SearchNoteSentenceListPage = { IsDeleted = 0 }
            };
            response.Data = setNoteSentencePage;
            return response;
        }

        public ServiceResponse GetNoteSentenceList(SearchNoteSentenceListPage searchNoteSentenceListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchNoteSentenceListPage != null)
                SetSearchFilterForNoteSentenceList(searchNoteSentenceListPage, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<NoteSentence> totalData = GetEntityList<NoteSentence>(StoredProcedure.GetNoteSentenceList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<NoteSentence> noteSentenceList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = noteSentenceList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForNoteSentenceList(SearchNoteSentenceListPage searchNoteSentenceListPage, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(searchNoteSentenceListPage.NoteSentenceTitle))
                searchList.Add(new SearchValueData { Name = "NoteSentenceTitle", Value = Convert.ToString(searchNoteSentenceListPage.NoteSentenceTitle) });

            if (!string.IsNullOrEmpty(searchNoteSentenceListPage.NoteSentenceDetails))
                searchList.Add(new SearchValueData { Name = "NoteSentenceDetails", Value = Convert.ToString(searchNoteSentenceListPage.NoteSentenceDetails) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchNoteSentenceListPage.IsDeleted) });
        }

        public ServiceResponse DeleteNoteSentence(SearchNoteSentenceListPage searchNoteSentenceListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForNoteSentenceList(searchNoteSentenceListPage, searchList);

            if (!string.IsNullOrEmpty(searchNoteSentenceListPage.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchNoteSentenceListPage.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });

            List<NoteSentence> totalData = GetEntityList<NoteSentence>(StoredProcedure.DeleteNoteSentence, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<NoteSentence> noteSentenceList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = noteSentenceList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.NoteSentence);
            return response;
        }

        #endregion Note Sentence List
    }
}
