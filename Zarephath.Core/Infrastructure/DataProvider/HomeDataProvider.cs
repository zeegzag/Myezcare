using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EDI_837_835_HCCP;
using OopFactory.Edi835Parser.Models;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class HomeDataProvider : BaseDataProvider, IHomeDataProvider
    {
        CacheHelper _chHelper = new CacheHelper();

        #region Page Load

        public ServiceResponse SetDashboardPage(long loggedInUser)
        {
            var response = new ServiceResponse();
            //Set Pager Vaue on Load
            int pageIndex = Convert.ToInt16(ConfigSettings.PageIndex);
            int pageSize = _chHelper.PageSize > 0 ? _chHelper.PageSize : ConfigSettings.PageSize;

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();

            bool hasReferralDataPermission = Common.HasPermission(Constants.Permission_View_All_Referral) || Common.HasPermission(Constants.Permission_View_Assinged_Referral);

            searchList.Add(new SearchValueData { Name = "GetReferralRelatedData", Value = hasReferralDataPermission ? "1" : "0" });
            searchList.Add(new SearchValueData { Name = "SortExpressionIM", Value = "ReferralInternalMessageID" });
            searchList.Add(new SearchValueData { Name = "SortExpressionCS", Value = "ReferralID" });
            searchList.Add(new SearchValueData { Name = "SortExpressionDL", Value = "ReferralID" });
            searchList.Add(new SearchValueData { Name = "InternalMessageAssigneeID", Value = Convert.ToString(loggedInUser) });
            searchList.Add(new SearchValueData { Name = "AssigneeID", Value = Common.HasPermission(Constants.Permission_View_All_Referral) ? "0" : Convert.ToString(loggedInUser) });
            searchList.Add(new SearchValueData { Name = "SortType", Value = "DESC" });
            searchList.Add(new SearchValueData { Name = "FromIndex", Value = Convert.ToString(pageIndex) });
            searchList.Add(new SearchValueData { Name = "PageSize", Value = Convert.ToString(pageSize) });
            searchList.Add(new SearchValueData { Name = "ReferralStatusIds", Value = Convert.ToString(Convert.ToInt32(Common.ReferralStatusEnum.Active) + "," + Convert.ToInt32(Common.ReferralStatusEnum.NewReferral)) });
            searchList.Add(new SearchValueData { Name = "ReferralStatusIdsForInDoc", Value = Convert.ToString(Convert.ToInt32(Common.ReferralStatusEnum.Active)) });

            Dashboard dashboard = GetMultipleEntity<Dashboard>(StoredProcedure.GetDashboardPageList, searchList);
            DashboardModel dashboardModel = new DashboardModel
            {
                ReferralSparFormChekListModel = { Items = dashboard.ReferralSparFormChekListModel },
                ReferralInternalMessageModel = { Items = dashboard.ReferralInternalMessageModel },
                ReferralMissingandExpireDocumentListModel = { Items = dashboard.ReferralMissingandExpireDocumentListModel },
                ReferralResolvedInternalMessageListModel = { Items = dashboard.ReferralResolvedInternalMessageListModel },
                ReferralInternalMissingandExpireDocumentListModel = { Items = dashboard.ReferralInternalMissingandExpireDocumentListModel }
            };

            if (hasReferralDataPermission)
            {
                int count = 0;
                if (dashboard.ReferralSparFormChekListModel != null && dashboard.ReferralSparFormChekListModel.Count > 0)
                    count = dashboard.ReferralSparFormChekListModel.First().Count;
                dashboardModel.ReferralSparFormChekListModel = GetPageInStoredProcResultSet(1, 10, count, dashboard.ReferralSparFormChekListModel);

                count = 0;
                if (dashboard.ReferralInternalMessageModel != null && dashboard.ReferralInternalMessageModel.Count > 0)
                    count = dashboard.ReferralInternalMessageModel.First().Count;
                dashboardModel.ReferralInternalMessageModel = GetPageInStoredProcResultSet(1, 10, count, dashboard.ReferralInternalMessageModel);

                count = 0;
                if (dashboard.ReferralMissingandExpireDocumentListModel != null && dashboard.ReferralMissingandExpireDocumentListModel.Count > 0)
                    count = dashboard.ReferralMissingandExpireDocumentListModel.First().Count;
                dashboardModel.ReferralMissingandExpireDocumentListModel = GetPageInStoredProcResultSet(1, 10, count, dashboard.ReferralMissingandExpireDocumentListModel);

                count = 0;
                if (dashboard.ReferralResolvedInternalMessageListModel != null && dashboard.ReferralResolvedInternalMessageListModel.Count > 0)
                    count = dashboard.ReferralResolvedInternalMessageListModel.First().Count;
                dashboardModel.ReferralResolvedInternalMessageListModel = GetPageInStoredProcResultSet(1, 10, count, dashboard.ReferralResolvedInternalMessageListModel);

                count = 0;
                if (dashboard.ReferralInternalMissingandExpireDocumentListModel != null && dashboard.ReferralInternalMissingandExpireDocumentListModel.Count > 0)
                    count = dashboard.ReferralInternalMissingandExpireDocumentListModel.First().Count;
                dashboardModel.ReferralInternalMissingandExpireDocumentListModel = GetPageInStoredProcResultSet(1, 10, count, dashboard.ReferralInternalMissingandExpireDocumentListModel);

            }

            response.Data = dashboardModel;
            //response.Data = new DashboardModel();
            response.IsSuccess = true;
            return response;
        }

        #endregion

        #region GetInternal Messgae List

        public ServiceResponse GetReferralInternalMessageList(long loggedInUser, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);
            searchList.Add(new SearchValueData { Name = "InternalMessageAssigneeID", Value = Convert.ToString(loggedInUser) });

            searchList.Add(new SearchValueData { Name = "AssigneeID", Value = Common.HasPermission(Constants.AllRecordAccess) ? "0" : Convert.ToString(loggedInUser) });
            //end

            List<ReferralInternalMessageListModel> setinternalmessgaeModel = GetEntityList<ReferralInternalMessageListModel>(StoredProcedure.GetDashboardInternalMessgaeList, searchList);

            DashboardModel dashboardModel = new DashboardModel
            {
                ReferralInternalMessageModel = { Items = setinternalmessgaeModel },
            };

            int count = 0;
            if (setinternalmessgaeModel != null && setinternalmessgaeModel.Count > 0)
                count = setinternalmessgaeModel.First().Count;
            dashboardModel.ReferralInternalMessageModel = GetPageInStoredProcResultSet(1, 10, count, setinternalmessgaeModel);

            response.Data = dashboardModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse MarkResolvedMessageAsRead(long messageId, long referralId, long loggedInUser, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();
            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralInternalMessageID", Value = Convert.ToString(messageId) });
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(referralId) });
            searchList.Add(new SearchValueData { Name = "CreatedBy", Value = Convert.ToString(loggedInUser) });
            //end

            ReferralInternalMessage model = GetEntity<ReferralInternalMessage>(searchList);
            if (model != null)
            {
                model.MarkAsResolvedRead = true;
                SaveObject(model, loggedInUser);
                //response.Data = GetReferralResolvedInternalMessageList(loggedInUser, sortDirection, sortIndex, pageIndex, pageSize).Data;
                response.Message = Resource.MessageMarkAsResolved;
                response.IsSuccess = true;
            }
            else
                response.Message = Resource.ErrorOccured;
            return response;
        }


        #endregion

        #region Get Spar Form Check List

        public ServiceResponse GetReferralSparFormList(long loggedInUser, string sortDirection, string sortIndex, int pageIndex, int pageSize)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);

            searchList.Add(new SearchValueData { Name = "ReferralStatusIds", Value = Convert.ToString(Convert.ToInt32(Common.ReferralStatusEnum.Active) + "," + Convert.ToInt32(Common.ReferralStatusEnum.NewReferral)) });
            searchList.Add(new SearchValueData { Name = "AssigneeID", Value = Common.HasPermission(Constants.Permission_View_All_Referral) ? "0" : Convert.ToString(loggedInUser) });
            //end
            List<ReferralSparFormChekListModel> setReferralSparFormModel = GetEntityList<ReferralSparFormChekListModel>(StoredProcedure.GetDashboardInCompeleteSparFormandCheckList, searchList);

            DashboardModel dashboardModel = new DashboardModel
            {
                ReferralSparFormChekListModel = { Items = setReferralSparFormModel },
            };

            int count = 0;
            if (setReferralSparFormModel != null && setReferralSparFormModel.Count > 0)
                count = setReferralSparFormModel.First().Count;
            dashboardModel.ReferralSparFormChekListModel = GetPageInStoredProcResultSet(1, 10, count, setReferralSparFormModel);

            response.Data = dashboardModel;
            response.IsSuccess = true;
            return response;
        }

        #endregion

        #region Get Missing Document List

        public ServiceResponse GetReferralMissingDocumentList(long loggedInUser, string sortDirection, string sortIndex, int pageIndex, int pageSize)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);
            searchList.Add(new SearchValueData { Name = "AssigneeID", Value = Common.HasPermission(Constants.Permission_View_All_Referral) ? "0" : Convert.ToString(loggedInUser) });
            searchList.Add(new SearchValueData { Name = "ReferralStatusIds", Value = Convert.ToString(Convert.ToInt32(Common.ReferralStatusEnum.Active) + "," + Convert.ToInt32(Common.ReferralStatusEnum.NewReferral)) });
            //end

            List<ReferralMissingandExpireDocumentListModel> referralMissingandExpireDocumentListModel = GetEntityList<ReferralMissingandExpireDocumentListModel>(StoredProcedure.GetDashboardMissingandExpireDocumentList, searchList);

            DashboardModel dashboardModel = new DashboardModel
            {
                ReferralMissingandExpireDocumentListModel = { Items = referralMissingandExpireDocumentListModel },
            };

            int count = 0;
            if (referralMissingandExpireDocumentListModel != null && referralMissingandExpireDocumentListModel.Count > 0)
                count = referralMissingandExpireDocumentListModel.First().Count;
            dashboardModel.ReferralMissingandExpireDocumentListModel = GetPageInStoredProcResultSet(1, 10, count, referralMissingandExpireDocumentListModel);

            response.Data = dashboardModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetReferralMissingDocument(long referralId)
        {
            var response = new ServiceResponse();
            if (referralId > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(referralId)},
                    new SearchValueData {Name = "AgencyROI", Value = Convert.ToString(Constants.AgencyROI)},
                    new SearchValueData {Name = "NetworkServicePlan", Value = Convert.ToString(Constants.NetworkServicePlan)},
                    new SearchValueData {Name = "BXAssessment", Value = Convert.ToString(Constants.BXAssessment)},
                    new SearchValueData {Name = "Demographic", Value = Convert.ToString(Constants.Demographic)},
                    new SearchValueData {Name = "SNCD", Value = Convert.ToString(Constants.SNCD)},
                    new SearchValueData {Name = "NetworkCrisisPlan", Value = Convert.ToString(Constants.NetworkCrisisPlan)},
                    new SearchValueData {Name = "CAZOnly", Value = Convert.ToString(Constants.CAZOnly)},
                    new SearchValueData {Name = "Missing", Value = Convert.ToString(Constants.Missing)},
                    new SearchValueData {Name = "Expired", Value = Convert.ToString(Constants.Expired)}
                   };
                List<MissingDocumentListModel> missingDocumentListModel = GetEntityList<MissingDocumentListModel>(StoredProcedure.GetDashboardMissingDocument, searchlist);

                string htmlString = missingDocumentListModel.Count > 0 ? missingDocumentListModel.OrderByDescending(x => x.MissingDocumentType).Aggregate("<ul>", (current, missingDocument) => current + string.Format("<li>{0}:&nbsp;{1}</li>", missingDocument.MissingDocumentType, missingDocument.MissingDocumentName))
                                : string.Format("<ul><li>{0}</li>", Resource.NoDocumentMissing);
                htmlString += "</ul>";

                response.Data = htmlString;
                response.IsSuccess = true;
            }
            else
                response.Message = Resource.GetReferralDocumentError;
            return response;
        }

        #endregion

        #region Get Missing Document List

        public ServiceResponse GetReferralInternalMissingDocumentList(long loggedInUser, string sortDirection, string sortIndex, int pageIndex, int pageSize)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);
            searchList.Add(new SearchValueData { Name = "AssigneeID", Value = Common.HasPermission(Constants.Permission_View_All_Referral) ? "0" : Convert.ToString(loggedInUser) });
            searchList.Add(new SearchValueData { Name = "ReferralStatusIds", Value = Convert.ToString(Convert.ToInt32(Common.ReferralStatusEnum.Active)) });
            //end

            List<ReferralMissingandExpireDocumentListModel> referralMissingandExpireDocumentListModel = GetEntityList<ReferralMissingandExpireDocumentListModel>(StoredProcedure.GetDashboardInternalMissingandExpireDocumentList, searchList);

            DashboardModel dashboardModel = new DashboardModel
            {
                ReferralInternalMissingandExpireDocumentListModel = { Items = referralMissingandExpireDocumentListModel },
            };

            int count = 0;
            if (referralMissingandExpireDocumentListModel != null && referralMissingandExpireDocumentListModel.Count > 0)
                count = referralMissingandExpireDocumentListModel.First().Count;
            dashboardModel.ReferralInternalMissingandExpireDocumentListModel = GetPageInStoredProcResultSet(1, 10, count, referralMissingandExpireDocumentListModel);

            response.Data = dashboardModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetReferralInternalMissingDocument(long referralId)
        {
            var response = new ServiceResponse();
            if (referralId > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(referralId)},
                    new SearchValueData {Name = "CareConsent", Value = Convert.ToString(Constants.CareConsent)},
                    new SearchValueData {Name = "SelfAdministrationofMedication", Value = Convert.ToString(Constants.SelfAdministrationofMedication)},
                    new SearchValueData {Name = "HealthInformationDisclosure", Value = Convert.ToString(Constants.HealthInformationDisclosure)},
                    new SearchValueData {Name = "AdmissionRequirements", Value = Convert.ToString(Constants.AdmissionRequirements)},
                    new SearchValueData {Name = "AdmissionOrientation", Value = Convert.ToString(Constants.AdmissionOrientation)},
                    new SearchValueData {Name = "ZarephathCrisisPlan", Value = Convert.ToString(Constants.ZarephathCrisisPlan)},
                    new SearchValueData {Name = "PHI", Value = Convert.ToString(Constants.PHI)},
                    new SearchValueData {Name = "ZarephathServicePlan", Value = Convert.ToString(Constants.ZarephathServicePlan)},
                    new SearchValueData {Name = "Missing", Value = Convert.ToString(Constants.Missing)},
                    new SearchValueData {Name = "Expired", Value = Convert.ToString(Constants.Expired)}
                   };
                List<MissingDocumentListModel> missingDocumentListModel = GetEntityList<MissingDocumentListModel>(StoredProcedure.GetDashboardInternalMissingDocument, searchlist);

                string htmlString = missingDocumentListModel.Count > 0 ? missingDocumentListModel.OrderByDescending(x => x.MissingDocumentType).Aggregate("<ul>", (current, missingDocument) => current + string.Format("<li>{0}:&nbsp;{1}</li>", missingDocument.MissingDocumentType, missingDocument.MissingDocumentName))
                                : string.Format("<ul><li>{0}</li>", Resource.NoDocumentMissing);
                htmlString += "</ul>";

                response.Data = htmlString;
                response.IsSuccess = true;
            }
            else
                response.Message = Resource.GetReferralDocumentError;
            return response;
        }

        #endregion

        #region GetInternal Resolved Messgae List

        public ServiceResponse GetReferralResolvedInternalMessageList(long loggedInUser, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List 
            List<SearchValueData> searchList = new List<SearchValueData>();
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);
            searchList.Add(new SearchValueData { Name = "InternalMessageAssigneeID", Value = Convert.ToString(loggedInUser) });
            searchList.Add(new SearchValueData { Name = "AssigneeID", Value = Common.HasPermission(Constants.AllRecordAccess) ? "0" : Convert.ToString(loggedInUser) });
            //end

            List<ReferralResolvedInternalMessageListModel> referralResolvedInternalMessageListModel = GetEntityList<ReferralResolvedInternalMessageListModel>(StoredProcedure.GetDashboardResolvedInternalMessgaeList, searchList);

            DashboardModel dashboardModel = new DashboardModel
            {
                ReferralResolvedInternalMessageListModel = { Items = referralResolvedInternalMessageListModel },
            };

            int count = 0;
            if (referralResolvedInternalMessageListModel != null && referralResolvedInternalMessageListModel.Count > 0)
                count = referralResolvedInternalMessageListModel.First().Count;
            dashboardModel.ReferralResolvedInternalMessageListModel = GetPageInStoredProcResultSet(1, 10, count, referralResolvedInternalMessageListModel);

            response.Data = dashboardModel;
            response.IsSuccess = true;
            return response;
        }

        #endregion


        public ServiceResponse GetPendingBypassVisit()
        {
            ServiceResponse response = new ServiceResponse();

            //Search List 
            List<SearchValueData> paramList = new List<SearchValueData>();
            paramList.Add(new SearchValueData { Name = "ActionTaken", Value = Convert.ToString((int)EmployeeVisit.BypassActions.Pending) });
            paramList.Add(new SearchValueData { Name = "IsApprovalRequired", Value = Convert.ToString(true) });
            //end

            int Count = (int)GetScalar(StoredProcedure.HC_GetPendingBypassVisit, paramList);


            response.Data = Count;
            response.IsSuccess = true;
            return response;
        }



        #region Get Layout Related Details

        public ServiceResponse GetLayoutRelatedDetails(SearchLayoutDetailModel searchModel)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            string newCheckTime = DateTime.UtcNow.ToString(Constants.DbDateTimeFormat);
            string newCheckTimeForPendingVisit = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat);

            searchModel.AssigneeLastCheckTime = string.IsNullOrEmpty(searchModel.AssigneeLastCheckTime) ? newCheckTime : searchModel.AssigneeLastCheckTime;
            searchModel.ResolvedLastCheckTime = string.IsNullOrEmpty(searchModel.ResolvedLastCheckTime) ? newCheckTime : searchModel.ResolvedLastCheckTime;
            searchModel.PendingVisitLastCheckTime = string.IsNullOrEmpty(searchModel.PendingVisitLastCheckTime) ? newCheckTimeForPendingVisit : searchModel.PendingVisitLastCheckTime;

            searchList.Add(new SearchValueData { Name = "PageSize", Value = Convert.ToString(searchModel.PageSize) });
            searchList.Add(new SearchValueData { Name = "Assignee", Value = Convert.ToString(searchModel.AssineeID) });
            searchList.Add(new SearchValueData { Name = "CreatedBY", Value = Convert.ToString(searchModel.CreatedID) });
            searchList.Add(new SearchValueData { Name = "AssigneeLastCheckTime", Value = Convert.ToString(searchModel.AssigneeLastCheckTime) });
            searchList.Add(new SearchValueData { Name = "ResolvedLastCheckTime", Value = Convert.ToString(searchModel.ResolvedLastCheckTime) });
            searchList.Add(new SearchValueData { Name = "PendingVisitLastCheckTime", Value = Convert.ToString(searchModel.PendingVisitLastCheckTime) });

            LayoutDetailModel layoutDetailModel = GetMultipleEntity<LayoutDetailModel>(StoredProcedure.GetLayoutRelatedDetails, searchList);


            if (layoutDetailModel.ResolvedMessagesCount > 0)
                layoutDetailModel.ResolvedMessagesCountMessage = string.Format(Resource.ResolvedMessagesCountMessage, layoutDetailModel.ResolvedMessagesCount);

            if (layoutDetailModel.NewMessagesCount > 0)
                layoutDetailModel.NewMessagesCountMessage = string.Format(Resource.NewMessagesCountMessage, layoutDetailModel.NewMessagesCount);

            if (layoutDetailModel.PendingVisitCount > 0)
                layoutDetailModel.PendingVisitCountMessage = string.Format(Resource.PendingVisitCountMessage, layoutDetailModel.PendingVisitCount);

            layoutDetailModel.NewCheckTime = newCheckTime;
            layoutDetailModel.NewCheckTimeForPendingVisit = newCheckTimeForPendingVisit;
            layoutDetailModel.CanHaveApprovePermission = Common.HasPermission(Constants.HC_Can_Approve_Bypass_ClockInOut);
            response.Data = layoutDetailModel;
            response.IsSuccess = true;
            return response;
        }

        #endregion

        private static void SetSearchList(string sortDirection, string sortIndex, int pageIndex, int pageSize, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "SortExpression", Value = Convert.ToString(sortDirection) });
            searchList.Add(new SearchValueData { Name = "SortType", Value = Convert.ToString(sortIndex) });
            searchList.Add(new SearchValueData { Name = "FromIndex", Value = Convert.ToString(pageIndex) });
            searchList.Add(new SearchValueData { Name = "PageSize", Value = Convert.ToString(pageSize) });
        }

        private static void SetSearchList(long loggedInUser, string IsDeleted, string sortDirection, string sortIndex, int pageIndex, int pageSize, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "SortExpression", Value = Convert.ToString(sortDirection) });
            searchList.Add(new SearchValueData { Name = "SortType", Value = Convert.ToString(sortIndex) });
            searchList.Add(new SearchValueData { Name = "FromIndex", Value = Convert.ToString(pageIndex) });
            searchList.Add(new SearchValueData { Name = "PageSize", Value = Convert.ToString(pageSize) });
            searchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(loggedInUser) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(IsDeleted) });
        }


        #region Get Ansell Casey List
        public ServiceResponse GetReferralAnsellCaseyReviewList(long loggedInUser, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);
            searchList.Add(new SearchValueData { Name = "AssigneeID", Value = Convert.ToString(loggedInUser) });
            //end

            List<ReferralAnsellCaseyListModel> referralAnsellCaseyListModel = GetEntityList<ReferralAnsellCaseyListModel>(StoredProcedure.GetDashboardAnsellCaseyReviewList, searchList);

            DashboardModel dashboardModel = new DashboardModel
            {
                ReferralAnsellCaseyReviewListModel = { Items = referralAnsellCaseyListModel },
            };

            int count = 0;
            if (referralAnsellCaseyListModel != null && referralAnsellCaseyListModel.Count > 0)
                count = referralAnsellCaseyListModel.First().Count;
            dashboardModel.ReferralAnsellCaseyReviewListModel = GetPageInStoredProcResultSet(1, 10, count, referralAnsellCaseyListModel);

            response.Data = dashboardModel;
            response.IsSuccess = true;
            return response;
        }

        #endregion


        #region Get Assigned Note Foor Review List
        public ServiceResponse GetReferralAssignedNotesReviewList(long loggedInUser, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);
            searchList.Add(new SearchValueData { Name = "AssigneeID", Value = Convert.ToString(loggedInUser) });
            //end

            List<ReferralAssignedNotesReviewListModel> referralAssignedNotesReviewListModel = GetEntityList<ReferralAssignedNotesReviewListModel>(StoredProcedure.GetDashboardAssignedNoteReviewList, searchList);

            DashboardModel dashboardModel = new DashboardModel
            {
                ReferralAssignedNotesReviewListModel = { Items = referralAssignedNotesReviewListModel },
            };

            int count = 0;
            if (referralAssignedNotesReviewListModel != null && referralAssignedNotesReviewListModel.Count > 0)
                count = referralAssignedNotesReviewListModel.First().Count;
            dashboardModel.ReferralAssignedNotesReviewListModel = GetPageInStoredProcResultSet(1, 10, count, referralAssignedNotesReviewListModel);

            response.Data = dashboardModel;
            response.IsSuccess = true;
            return response;
        }

        #endregion

        #region Scrap Code
        public ServiceResponse GenerateEdi837()
        {
            CacheHelper _cacheHelper = new CacheHelper();
            string data = String.Format("{0:yyyyMMdd}", DateTime.Now);
            ServiceResponse response = new ServiceResponse();
            Edi837 edi837 = new Edi837();
            string filePath = String.Format(_cacheHelper.EdiFile837UploadPath, _cacheHelper.Domain);
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".hippa";


            //PayorEdi837Setting payorEdi837Setting = GetEntity<PayorEdi837Setting>(1);
            //List<SearchValueData> searchList = new List<SearchValueData>() { new SearchValueData { Name = "BatchID", Value = Convert.ToString(108) } };
            //List<BatchRelatedAllDataModel> modelList = GetEntityList<BatchRelatedAllDataModel>("GetBatchRelatedAllData", searchList);

            //Edi837Model edi837Model = GetEdit837Model(6745, payorEdi837Setting, modelList);

            List<SearchValueData> searchList = new List<SearchValueData>() { new SearchValueData { Name = "BatchID", Value = Convert.ToString(108) } };
            ParentBatchRelatedAllDataModel model = GetMultipleEntity<ParentBatchRelatedAllDataModel>("GetBatchRelatedAllData", searchList);
            Edi837Model edi837Model = GetEdit837Model(6745, model.PayorEdi837Setting, model.BatchRelatedAllDataModel);
            string fileServerPath = HttpContext.Current.Server.MapCustomPath(filePath);
            edi837.GenerateEdi837File(edi837Model, fileServerPath, fileName);

            return response;
        }
        public Edi837Model GetEdit837Model(long batchID, PayorEdi837Setting payorEdi837Setting, List<BatchRelatedAllDataModel> batchRelatedAllDataList)
        {

            int controlNumber = 80;
            GroupedModelFor837 groupedModelFor837 = GenerateGroupedModelFor837(batchRelatedAllDataList);
            #region Get EDI 837 Model

            Edi837Model model = new Edi837Model();
            foreach (var tempBillingProvider in groupedModelFor837.BillingProviders)
            {

                #region Add Billing Provider
                BillingProvider billingProvider = new BillingProvider()
                {
                    HierarchicalIDNumber = payorEdi837Setting.BillingProvider_HL01_HierarchicalIDNumber,
                    HierarchicalParentIDNumber = payorEdi837Setting.BillingProvider_HL02_HierarchicalParentIDNumber,
                    HeirarchicalLevelCode = payorEdi837Setting.BillingProvider_HL03_HierarchicalLevelCode,
                    HierarchicalChildCode = payorEdi837Setting.BillingProvider_HL04_HierarchicalChildCode,
                    EntityIdentifierCode = payorEdi837Setting.BillingProvider_NM101_EntityIdentifierCode,
                    EntityTypeQualifier = payorEdi837Setting.BillingProvider_NM102_EntityTypeQualifier,
                    NameLastOrOrganizationName = tempBillingProvider.BillingProviderName,
                    IdCodeQualifier = payorEdi837Setting.BillingProvider_NM108_IdentificationCodeQualifier,
                    IdCodeQualifierEnum = tempBillingProvider.BillingProviderNPI,
                    AddressInformation = tempBillingProvider.BillingProviderAddress,
                    CityName = tempBillingProvider.BillingProviderCity,
                    StateOrProvinceCode = tempBillingProvider.BillingProviderState,
                    PostalCode = tempBillingProvider.BillingProviderZipcode,
                    ReferenceIdentificationQualifier = payorEdi837Setting.BillingProvider_REF01_ReferenceIdentificationQualifier,
                    ReferenceIdentification = tempBillingProvider.BillingProviderEIN
                };


                #endregion

                foreach (var tempSubscriber in tempBillingProvider.Subscribers)
                {

                    #region Add Subscriber
                    Subscriber subscriber = new Subscriber()
                    {
                        HeirarchicalLevelCode = payorEdi837Setting.Subscriber_HL03_HierarchicalLevelCode,   // HL03
                        PayerResponsibilitySequenceNumber = payorEdi837Setting.Subscriber_SBR01_PayerResponsibilitySequenceNumberCode, // SBR01
                        IndividualRelationshipCode = payorEdi837Setting.Subscriber_SBR02_RelationshipCode,// SBR02
                        ClaimFilingIndicatorCode = payorEdi837Setting.Subscriber_SBR09_ClaimFilingIndicatorCode,// SBR09

                        #region Subscriber Name
                        SubmitterEntityIdentifierCode = payorEdi837Setting.Subscriber_NM101_EntityIdentifierCode, // NM101
                        SubmitterEntityTypeQualifier = payorEdi837Setting.Subscriber_NM102_EntityIdentifierCode,// NM102
                        SubmitterNameLastOrOrganizationName = tempSubscriber.LastName, // NM103
                        SubmitterNameFirst = tempSubscriber.FirstName, // NM104
                        SubmitterIdCodeQualifier = payorEdi837Setting.Subscriber_NM108_IdentificationCodeQualifier, // NM108
                        SubmitterIdCodeQualifierEnum = tempSubscriber.SubscriberID,// NM109

                        SubmitterAddressInformation = tempSubscriber.Address, // N301
                        SubmitterCityName = tempSubscriber.City,// N401
                        SubmitterStateOrProvinceCode = tempSubscriber.State,// N402
                        SubmitterPostalCode = tempSubscriber.ZipCode,// N403

                        SubmitterDateTimePeriodFormatQualifier = payorEdi837Setting.Subscriber_DMG01_DateTimePeriodFormatQualifier,// DMG01
                        SubmitterDateTimePeriod = tempSubscriber.Dob,// DMG02
                        SubmitterGenderCode = tempSubscriber.Gender,// DMG03

                        #endregion Subscriber Name

                        #region Payer Name

                        PayerEntityIdentifierCode = payorEdi837Setting.Subscriber_Payer_NM101_EntityIdentifierCode,// NM101
                        PayerEntityTypeQualifier = payorEdi837Setting.Subscriber_Payer_NM102_EntityTypeQualifier,// NM102
                        PayerNameLastOrOrganizationName = tempSubscriber.PayorName, // NM103
                        PayerIdCodeQualifier = payorEdi837Setting.Subscriber_Payer_NM108_IdentificationCodeQualifier,// NM108
                        PayerIdCodeQualifierEnum = tempSubscriber.PayorIdentificationNumber,// NM109

                        PayerAddressInformation = tempSubscriber.PayorAddress, // N301
                        PayerCityName = tempSubscriber.PayorCity,// N401
                        PayerStateOrProvinceCode = tempSubscriber.PayorState,// N402
                        PayerPostalCode = tempSubscriber.PayorZipcode// N403

                        #endregion Payer Name
                    };
                    #endregion

                    foreach (var tempClaim in tempSubscriber.Claims)
                    {
                        #region Add Claims
                        Claim claim = new Claim()
                        {
                            //PatientControlNumber = "Referral|NoteID", // CLM01
                            ClaimSubmitterIdentifier = tempClaim.ClaimSubmitterIdentifier,// CLM01
                            TotalClaimChargeAmount = tempClaim.CalculatedAmount.ToString(), // CLM02
                            FacilityCodeValue = tempClaim.PosID.ToString(), // CLM05-01
                            FacilityCodeQualifier = payorEdi837Setting.Claim_CLM05_02_FacilityCodeQualifier,// CLM05-02
                            ClaimFrequencyTypeCode = payorEdi837Setting.Claim_CLM05_03_ClaimFrequencyCode,// CLM05-03
                            ProviderOrSupplierSignatureIndicator = payorEdi837Setting.Claim_CLM06_ProviderSignatureOnFile,// CLM06
                            ProviderAcceptAssignmentCode = payorEdi837Setting.Claim_CLM07_ProviderAcceptAssignment, // CLM07
                            BenefitsAssignmentCerficationIndicator = payorEdi837Setting.Claim_CLM08_AssignmentOfBenefitsIndicator, // CLM08
                            ReleaseOfInformationCode = payorEdi837Setting.Claim_CLM09_ReleaseOfInformationCode,// CLM09
                            PatientSignatureSourceCode = payorEdi837Setting.Claim_CLM010_PatientSignatureSource,// CLM10

                            ReferenceIdentificationQualifier = payorEdi837Setting.Claim_REF01_ReferenceIdentificationQualifier, // REF01
                            ReferenceIdentification = payorEdi837Setting.Claim_REF02_MedicalRecordNumber,// REF02

                            //HealthCareCodeInformation01_01 = payorEdi837Setting.Claim_HI01_01_PrincipalDiagnosisQualifier,// HI01-01
                            //HealthCareCodeInformation01_02 = tempClaim.ContinuedDX,// HI01-02

                            HealthCareCodeInformation01 = String.Format("{0}{1}{2}", payorEdi837Setting.Claim_HI01_01_PrincipalDiagnosisQualifier,
                            payorEdi837Setting.ISA16_ComponentElementSeparator, tempClaim.ContinuedDX), // HI01-01, HI01-02

                            #region Provider Information

                            #region Provider Information > Rendering Provider

                            RenderingProviderEntityIdentifierCode = payorEdi837Setting.Claim_RenderringProvider_NM01_EntityIdentifierCode,// NM101
                            RenderingProviderEntityTypeQualifier = payorEdi837Setting.Claim_RenderringProvider_NM02_EntityTypeQualifier,// NM102
                            RenderingProviderNameLastOrOrganizationName = tempClaim.RenderingProviderName,// NM103
                            RenderingProviderIdCodeQualifier = payorEdi837Setting.Claim_RenderringProvider_NM108_IdentificationCodeQualifier,// NM108
                            RenderingProviderIdCodeQualifierEnum = tempClaim.RenderingProviderNPI,// NM109

                            #endregion Provider Information > Rendering Provider

                            #region Provider Information > Service Facility Location

                            ServiceFacilityLocationEntityIdentifierCode = payorEdi837Setting.Claim_ServiceFacility_NM101_EntityIdentifierCode, // NM101
                            ServiceFacilityLocationEntityTypeQualifier = payorEdi837Setting.Claim_ServiceFacility_NM102_EntityTypeQualifier, // NM102
                            ServiceFacilityLocationNameLastOrOrganizationName = tempClaim.RenderingProviderName, // NM103
                            ServiceFacilityLocationIdCodeQualifier = payorEdi837Setting.Claim_ServiceFacility_NM108_IdentificationCodeQualifier,// NM108
                            ServiceFacilityLocationIdCodeQualifierEnum = tempClaim.RenderingProviderEIN,// NM109

                            ServiceFacilityLocationAddressInformation = tempClaim.RenderingProviderEIN,// N301
                            ServiceFacilityLocationCityName = tempClaim.RenderingProviderCity,// N401
                            ServiceFacilityLocationStateOrProvinceCode = tempClaim.RenderingProviderState,// N402
                            ServiceFacilityLocationPostalCode = tempClaim.RenderingProviderZipcode,// N403

                            #endregion Provider Information > Service Facility Location

                            #endregion Provider Information
                        };
                        #endregion

                        foreach (var tempServiceLine in tempClaim.ServiceLines)
                        {
                            #region Add ServiceLine
                            ServiceLine serviceLine = new ServiceLine()
                            {
                                AssignedNumber = "1", // LX01 TODO: NEED TO CHANGE
                                //CompositeMedicalProcedureIdentifier_01 = payorEdi837Setting.Claim_ServiecLine_SV101_01_ProductServiceIDQualifier,
                                //CompositeMedicalProcedureIdentifier_02 = tempServiceLine.ServiceCode,
                                CompositeMedicalProcedureIdentifier = String.Format("{0}{1}{2}", payorEdi837Setting.Claim_ServiecLine_SV101_01_ProductServiceIDQualifier,
                                payorEdi837Setting.ISA16_ComponentElementSeparator, tempServiceLine.ServiceCode), // SV101-01, SV101-02

                                MonetaryAmount = tempClaim.CalculatedAmount.ToString(),// SV102
                                UnitOrBasisForMeasurementCode = payorEdi837Setting.Claim_ServiecLine_SV103_BasisOfMeasurement,// SV103
                                Quantity = tempServiceLine.CalculatedUnit.ToString(),// SV104
                                //CompositeDiagnosisCodePointer = // SV107
                                CompositeDiagnosisCodePointer = payorEdi837Setting.Claim_ServiceLine_SV107_01_DiagnosisCodePointer,

                                DateTimeQualifier = payorEdi837Setting.Claim_ServiceLine_Date_DTP01_DateTimeQualifier,// DTP01
                                DateTimePeriodFormatQualifier = payorEdi837Setting.Claim_ServiceLine_Date_DTP02_DateTimePeriodFormatQualifier,// DTP02
                                DateTimePeriod = tempServiceLine.ServiceDateSpan,// DTP03

                                ReferenceIdentificationQualifier = payorEdi837Setting.Claim_ServiceLine_Ref_REF01_ReferenceIdentificationQualifier,// REF01
                                ReferenceIdentification = payorEdi837Setting.Claim_ServiceLine_Ref_REF02_ReferenceIdentification// REF02
                            };
                            #endregion

                            claim.ServiceLines.Add(serviceLine);
                        }

                        subscriber.Claims.Add(claim);
                    }



                    billingProvider.Subscribers.Add(subscriber);
                }


                model.BillingProviders.Add(billingProvider);

            }

            #region Header Setter
            //ISA
            model.InterchangeControlHeader.AuthorizationInformationQualifier = payorEdi837Setting.ISA01_AuthorizationInformationQualifier;
            model.InterchangeControlHeader.AuthorizationInformation = payorEdi837Setting.ISA02_AuthorizationInformation;
            model.InterchangeControlHeader.SecurityInformationQualifier = payorEdi837Setting.ISA03_SecurityInformationQualifier;
            model.InterchangeControlHeader.SecurityInformation = payorEdi837Setting.ISA04_SecurityInformation;
            model.InterchangeControlHeader.InterchangeSenderIdQualifier = payorEdi837Setting.ISA05_InterchangeSenderIdQualifier; ;
            model.InterchangeControlHeader.InterchangeSenderId = payorEdi837Setting.ISA06_InterchangeSenderId;
            model.InterchangeControlHeader.InterchangeReceiverIdQualifier = payorEdi837Setting.ISA07_InterchangeReceiverIdQualifier;
            model.InterchangeControlHeader.InterchangeReceiverId = payorEdi837Setting.ISA08_InterchangeReceiverId;
            model.InterchangeControlHeader.InterchangeDate = payorEdi837Setting.ISA09_InterchangeDate;
            model.InterchangeControlHeader.InterchangeTime = payorEdi837Setting.ISA10_InterchangeTime;
            model.InterchangeControlHeader.RepetitionSeparator = payorEdi837Setting.ISA11_RepetitionSeparator;
            model.InterchangeControlHeader.InterchangeControlVersionNumber = payorEdi837Setting.ISA12_InterchangeControlVersionNumber;
            model.InterchangeControlHeader.InterchangeControlNumber = string.Format("{0:00000000}", controlNumber);
            model.InterchangeControlHeader.AcknowledgementRequired = payorEdi837Setting.ISA14_AcknowledgementRequired;
            model.InterchangeControlHeader.UsageIndicator = payorEdi837Setting.ISA15_UsageIndicator;
            model.InterchangeControlHeader.ComponentElementSeparator = payorEdi837Setting.ISA16_ComponentElementSeparator;


            //GS
            model.FunctionalGroupHeader.FunctionalIdentifierCode = payorEdi837Setting.GS01_FunctionalIdentifierCode;
            model.FunctionalGroupHeader.ApplicationSenderCode = payorEdi837Setting.GS02_ApplicationSenderCode;
            model.FunctionalGroupHeader.ApplicationReceiverCode = payorEdi837Setting.GS03_ApplicationReceiverCode;
            model.FunctionalGroupHeader.Date = payorEdi837Setting.GS04_Date;
            model.FunctionalGroupHeader.Time = payorEdi837Setting.GS05_Time;
            model.FunctionalGroupHeader.GroupControlNumber = payorEdi837Setting.GS06_GroupControlNumber;// "80";
            model.FunctionalGroupHeader.ResponsibleAgencyCode = payorEdi837Setting.GS07_ResponsibleAgencyCode;
            model.FunctionalGroupHeader.VersionOrReleaseOrNo = payorEdi837Setting.GS08_VersionOrReleaseOrNo;

            //ST
            model.TransactionSetHeader.TransactionSetIdentifier = payorEdi837Setting.ST01_TransactionSetIdentifier;
            model.TransactionSetHeader.TransactionSetControlNumber = payorEdi837Setting.ST02_TransactionSetControlNumber;// "0080";
            model.TransactionSetHeader.ImplementationConventionReference = payorEdi837Setting.ST03_ImplementationConventionReference;

            //BHT
            model.BeginningOfHierarchicalTransaction.HierarchicalStructureCode = payorEdi837Setting.BHT01_HierarchicalStructureCode;
            model.BeginningOfHierarchicalTransaction.TransactionSetPurposeCode = payorEdi837Setting.BHT02_TransactionSetPurposeCode;
            model.BeginningOfHierarchicalTransaction.ReferenceIdentification = Convert.ToString(batchID);// BATCH ID
            model.BeginningOfHierarchicalTransaction.Date = payorEdi837Setting.BHT04_Date;
            model.BeginningOfHierarchicalTransaction.InterchangeIdQualifier = payorEdi837Setting.BHT05_Time;
            model.BeginningOfHierarchicalTransaction.TransactionTypeCode = payorEdi837Setting.BHT06_TransactionTypeCode;


            //SUBMITTER NAME NM1
            model.SubmitterName.EntityIdentifierCodeEnum = payorEdi837Setting.Submitter_NM101_EntityIdentifierCodeEnum;
            model.SubmitterName.EntityTypeQualifier = payorEdi837Setting.Submitter_NM102_EntityTypeQualifier;
            model.SubmitterName.NameLastOrOrganizationName = payorEdi837Setting.Submitter_NM103_NameLastOrOrganizationName;
            model.SubmitterName.NameFirst = payorEdi837Setting.Submitter_NM104_NameFirst;
            model.SubmitterName.NameMiddle = payorEdi837Setting.Submitter_NM105_NameMiddle;
            model.SubmitterName.NamePrefix = payorEdi837Setting.Submitter_NM106_NamePrefix;
            model.SubmitterName.NameSuffix = payorEdi837Setting.Submitter_NM107_NameSuffix;
            model.SubmitterName.IdCodeQualifier = payorEdi837Setting.Submitter_NM108_IdCodeQualifier;
            model.SubmitterName.IdCodeQualifierEnum = payorEdi837Setting.Submitter_NM109_IdCodeQualifierEnum;
            model.SubmitterName.EntityRelationshipCode = payorEdi837Setting.Submitter_NM110_EntityRelationshipCode;
            model.SubmitterName.EntityIdentifierCode = payorEdi837Setting.Submitter_NM111_EntityIdentifierCode;
            model.SubmitterName.NameLastOrOrganizationName112 = payorEdi837Setting.Submitter_NM112_NameLastOrOrganizationName;


            //SUBMITTER NAME PER
            TypedLoopPER typedLoopPer = new TypedLoopPER();
            typedLoopPer.ContactFunctionCode = payorEdi837Setting.Submitter_EDIContact1_PER01_ContactFunctionCode;
            typedLoopPer.Name = payorEdi837Setting.Submitter_EDIContact1_PER02_Name;
            typedLoopPer.CommunicationNumberQualifier1 = payorEdi837Setting.Submitter_EDIContact1_PER03_CommunicationNumberQualifier;
            typedLoopPer.CommunicationNumber1 = payorEdi837Setting.Submitter_EDIContact1_PER04_CommunicationNumber;
            typedLoopPer.CommunicationNumberQualifier2 = payorEdi837Setting.Submitter_EDIContact1_PER05_CommunicationNumberQualifier;
            typedLoopPer.CommunicationNumber2 = payorEdi837Setting.Submitter_EDIContact1_PER06_CommunicationNumber;
            typedLoopPer.CommunicationNumberQualifier3 = payorEdi837Setting.Submitter_EDIContact1_PER07_CommunicationNumberQualifier;
            typedLoopPer.CommunicationNumber3 = payorEdi837Setting.Submitter_EDIContact1_PER08_CommunicationNumber;
            typedLoopPer.ContactInquiryReference = payorEdi837Setting.Submitter_EDIContact1_PER09_ContactInquiryReference;
            model.SubmitterEDIContact.Add(typedLoopPer);

            if (!string.IsNullOrEmpty(payorEdi837Setting.Submitter_EDIContact2_PER02_Name))
            {
                typedLoopPer = new TypedLoopPER();
                typedLoopPer.ContactFunctionCode = payorEdi837Setting.Submitter_EDIContact2_PER01_ContactFunctionCode;
                typedLoopPer.Name = payorEdi837Setting.Submitter_EDIContact2_PER02_Name;
                typedLoopPer.CommunicationNumberQualifier1 = payorEdi837Setting.Submitter_EDIContact2_PER03_CommunicationNumberQualifier;
                typedLoopPer.CommunicationNumber1 = payorEdi837Setting.Submitter_EDIContact2_PER04_CommunicationNumber;
                typedLoopPer.CommunicationNumberQualifier2 = payorEdi837Setting.Submitter_EDIContact2_PER05_CommunicationNumberQualifier;
                typedLoopPer.CommunicationNumber2 = payorEdi837Setting.Submitter_EDIContact2_PER06_CommunicationNumber;
                typedLoopPer.CommunicationNumberQualifier3 = payorEdi837Setting.Submitter_EDIContact2_PER07_CommunicationNumberQualifier;
                typedLoopPer.CommunicationNumber3 = payorEdi837Setting.Submitter_EDIContact2_PER08_CommunicationNumber;
                typedLoopPer.ContactInquiryReference = payorEdi837Setting.Submitter_EDIContact2_PER09_ContactInquiryReference;
                model.SubmitterEDIContact.Add(typedLoopPer);
            }


            //RECEIVER NM1
            model.ReceiverName.EntityIdentifierCodeEnum = payorEdi837Setting.Receiver_NM101_EntityIdentifierCodeEnum;
            model.ReceiverName.EntityTypeQualifier = payorEdi837Setting.Receiver_NM102_EntityTypeQualifier;
            model.ReceiverName.NameLastOrOrganizationName = payorEdi837Setting.Receiver_NM103_NameLastOrOrganizationName;
            model.ReceiverName.NameFirst = payorEdi837Setting.Receiver_NM104_NameFirst;
            model.ReceiverName.NameMiddle = payorEdi837Setting.Receiver_NM105_NameMiddle;
            model.ReceiverName.NamePrefix = payorEdi837Setting.Receiver_NM106_NamePrefix;
            model.ReceiverName.NameSuffix = payorEdi837Setting.Receiver_NM107_NameSuffix;
            model.ReceiverName.IdCodeQualifier = payorEdi837Setting.Receiver_NM108_IdCodeQualifier;
            model.ReceiverName.IdCodeQualifierEnum = payorEdi837Setting.Receiver_NM109_IdCodeQualifierEnum;
            model.ReceiverName.EntityRelationshipCode = payorEdi837Setting.Receiver_NM110_EntityRelationshipCode;
            model.ReceiverName.EntityIdentifierCode = payorEdi837Setting.Receiver_NM111_EntityIdentifierCode;
            model.ReceiverName.NameLastOrOrganizationName112 = payorEdi837Setting.Receiver_NM112_NameLastOrOrganizationName;


            #endregion

            #endregion
            return model;
        }
        public GroupedModelFor837 GenerateGroupedModelFor837(List<BatchRelatedAllDataModel> batchRelatedAllDataList)
        {
            GroupedModelFor837 groupedModelFor837 = new GroupedModelFor837();
            #region Generate Group Data for 837 Model

            #region Group By Billing Provider
            List<BillingGroupClass> tempBillingGroupList = batchRelatedAllDataList.ToList().GroupBy(ac => new
            {
                ac.BillingProviderID,
                ac.BillingProviderName,
                ac.BillingProviderAddress,
                ac.BillingProviderCity,
                ac.BillingProviderState,
                ac.BillingProviderZipcode,
                ac.BillingProviderEIN,
                ac.BillingProviderNPI,
                ac.BillingProviderGSA,
            })
                                                                         .Select(grp => new BillingGroupClass
                                                                         {
                                                                             BillingProviderModel = new BillingProviderModel
                                                                             {
                                                                                 BillingProviderID = grp.Key.BillingProviderID,
                                                                                 BillingProviderName = grp.Key.BillingProviderName,
                                                                                 BillingProviderAddress = grp.Key.BillingProviderAddress,
                                                                                 BillingProviderCity = grp.Key.BillingProviderCity,
                                                                                 BillingProviderState = grp.Key.BillingProviderState,
                                                                                 BillingProviderZipcode = grp.Key.BillingProviderZipcode,
                                                                                 BillingProviderEIN = grp.Key.BillingProviderEIN,
                                                                                 BillingProviderNPI = grp.Key.BillingProviderNPI,
                                                                                 BillingProviderGSA = grp.Key.BillingProviderGSA,
                                                                             },
                                                                             ListModel = grp.ToList()

                                                                         })
                                                                         .ToList();

            #endregion

            foreach (BillingGroupClass bg_batchRelatedAllDataModel in tempBillingGroupList)
            {
                BillingProviderModel billingProviderModel = new BillingProviderModel();
                billingProviderModel = bg_batchRelatedAllDataModel.BillingProviderModel;
                #region Group By Subscriber & Payors
                List<SubscriberPayorGroupClass> tempSubscriberPayorGroupList = bg_batchRelatedAllDataModel.ListModel.GroupBy(ac => new
                {
                    ac.ReferralID,
                    ac.AHCCCSID,
                    ac.CISNumber,
                    ac.FirstName,
                    ac.LastName,
                    ac.Dob,
                    ac.Gender,
                    ac.SubscriberID,
                    ac.Address,
                    ac.City,
                    ac.State,
                    ac.ZipCode,
                    ac.PayorIdentificationNumber,
                    ac.PayorName,
                    ac.PayorAddress,
                    ac.PayorCity,
                    ac.PayorState,
                    ac.PayorZipcode
                })
                                                                     .Select(grp => new SubscriberPayorGroupClass
                                                                     {
                                                                         SubscriberModel = new SubscriberModel()
                                                                         {
                                                                             ReferralID = grp.Key.ReferralID,
                                                                             AHCCCSID = grp.Key.AHCCCSID,
                                                                             CISNumber = grp.Key.CISNumber,
                                                                             FirstName = grp.Key.FirstName,
                                                                             LastName = grp.Key.LastName,
                                                                             Dob = grp.Key.Dob,
                                                                             Gender = grp.Key.Gender,
                                                                             SubscriberID = grp.Key.SubscriberID,
                                                                             Address = grp.Key.Address,
                                                                             City = grp.Key.City,
                                                                             State = grp.Key.State,
                                                                             ZipCode = grp.Key.ZipCode,
                                                                             PayorIdentificationNumber = grp.Key.PayorIdentificationNumber,
                                                                             PayorName = grp.Key.PayorName,
                                                                             PayorAddress = grp.Key.PayorAddress,
                                                                             PayorCity = grp.Key.PayorCity,
                                                                             PayorState = grp.Key.PayorState,
                                                                             PayorZipcode = grp.Key.PayorZipcode
                                                                         },
                                                                         ListModel = grp.ToList()

                                                                     }).ToList();







                #endregion

                foreach (SubscriberPayorGroupClass sp_batchRelatedAllDataModel in tempSubscriberPayorGroupList)
                {

                    #region Group By Claims
                    List<ClaimGroupClass> tempClaimGroupList = bg_batchRelatedAllDataModel.ListModel.GroupBy(ac => new
                    {
                        ac.ClaimSubmitterIdentifier,//ac.PatientAccountNumber,
                        ac.CalculatedAmount,
                        ac.PosID,
                        ac.PosName,
                        ac.ContinuedDX,
                        ac.ModifierID,
                        ac.ModifierName,
                        ac.RenderingProviderID,
                        ac.RenderingProviderName,
                        ac.RenderingProviderEIN,
                        ac.RenderingProviderNPI,
                        ac.RenderingProviderGSA,
                        ac.RenderingProviderAddress,
                        ac.RenderingProviderCity,
                        ac.RenderingProviderState,
                        ac.RenderingProviderZipcode
                    })
                                                                 .Select(grp => new ClaimGroupClass
                                                                 {
                                                                     ClaimModel = new ClaimModel()
                                                                     {
                                                                         ClaimSubmitterIdentifier = grp.Key.ClaimSubmitterIdentifier,
                                                                         //PatientAccountNumber = grp.Key.PatientAccountNumber,
                                                                         CalculatedAmount = grp.Key.CalculatedAmount,
                                                                         PosID = grp.Key.PosID,
                                                                         PosName = grp.Key.PosName,
                                                                         ContinuedDX = grp.Key.ContinuedDX,
                                                                         ModifierID = grp.Key.ModifierID,
                                                                         ModifierName = grp.Key.ModifierName,
                                                                         RenderingProviderID = grp.Key.RenderingProviderID,
                                                                         RenderingProviderName = grp.Key.RenderingProviderName,
                                                                         RenderingProviderEIN = grp.Key.RenderingProviderEIN,
                                                                         RenderingProviderNPI = grp.Key.RenderingProviderNPI,
                                                                         RenderingProviderGSA = grp.Key.RenderingProviderGSA,
                                                                         RenderingProviderAddress = grp.Key.RenderingProviderAddress,
                                                                         RenderingProviderCity = grp.Key.RenderingProviderCity,
                                                                         RenderingProviderState = grp.Key.RenderingProviderState,
                                                                         RenderingProviderZipcode = grp.Key.RenderingProviderZipcode
                                                                     },
                                                                     ListModel = grp.ToList()

                                                                 }).ToList();

                    #endregion

                    foreach (ClaimGroupClass claim_batchRelatedAllDataModel in tempClaimGroupList)
                    {
                        #region Group By ServiceLine
                        List<ServiceLineGroupClass> tempServiceLineGroupList = claim_batchRelatedAllDataModel.ListModel.GroupBy(ac => new
                        {
                            ac.ServiceCode,
                            ac.CalculatedAmount,
                            ac.CalculatedUnit,
                            ac.ServiceDateSpan,
                        })
                                                                     .Select(grp => new ServiceLineGroupClass
                                                                     {
                                                                         ServiceLineModel = new ServiceLineModel()
                                                                         {
                                                                             ServiceCode = grp.Key.ServiceCode,
                                                                             CalculatedAmount = grp.Key.CalculatedAmount,
                                                                             CalculatedUnit = grp.Key.CalculatedUnit,
                                                                             ServiceDateSpan = grp.Key.ServiceDateSpan,
                                                                         }//,
                                                                         //ListModel = grp.ToList()

                                                                     }).ToList();

                        #endregion

                        foreach (ServiceLineGroupClass serviceLine_batchRelatedAllDataModel in tempServiceLineGroupList)
                            claim_batchRelatedAllDataModel.ClaimModel.ServiceLines.Add(serviceLine_batchRelatedAllDataModel.ServiceLineModel);

                        sp_batchRelatedAllDataModel.SubscriberModel.Claims.Add(claim_batchRelatedAllDataModel.ClaimModel);
                    }

                    billingProviderModel.Subscribers.Add(sp_batchRelatedAllDataModel.SubscriberModel);
                }

                groupedModelFor837.BillingProviders.Add(billingProviderModel);
            }

            #endregion
            return groupedModelFor837;
        }
        #endregion






        #region Home Care Code
        public ServiceResponse HC_SetDashboardPage(long loggedInUser)
        {
            var response = new ServiceResponse();

            DateTime orgDate = Common.GetOrgCurrentDateTime();

            var thisWeekStart = Common.GetOrgStartOfWeek();
            //var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            var thisWeekEnd = thisWeekStart.AddDays(6);

            HC_GetDashboardModel model = new HC_GetDashboardModel();
            model.EmployeeTimeStaticSearchModel.StartDate = orgDate.AddDays(-30);
            model.EmployeeTimeStaticSearchModel.EndDate = orgDate;

            model.WeeklySearchModel.StartDate = thisWeekStart;
            model.WeeklySearchModel.EndDate = thisWeekEnd;

            model.EmpClockInOutListSearchModel.StartDate = orgDate;// thisWeekStart;
            model.EmpClockInOutListSearchModel.EndDate = orgDate;

            model.PatientClockInOutListSearchModel.StartDate = orgDate;// thisWeekStart;
            model.PatientClockInOutListSearchModel.EndDate = orgDate;

            model.EmpOverTimeListSearchModel.StartDate = orgDate.AddDays(-7);
            model.EmpOverTimeListSearchModel.EndDate = orgDate.AddDays(-1);

            model.PatientNotScheduleListSearchModel.StartDate = orgDate.AddDays(1);
            model.PatientNotScheduleListSearchModel.EndDate = orgDate.AddDays(7);

            model.PatientNewListSearchModel.StartDate = orgDate;
            model.PatientNewListSearchModel.EndDate = orgDate;

            model.PatientBirthdaySearchModel.StartDate = orgDate;
            model.PatientBirthdaySearchModel.EndDate = orgDate.AddDays(15);

            model.EmpBirthdaySearchModel.StartDate = orgDate;
            model.EmpBirthdaySearchModel.EndDate = orgDate.AddDays(15);

            model.PatientDischargedListSearchModel.StartDate = orgDate;
            model.PatientDischargedListSearchModel.EndDate = orgDate;

            model.PatientTransferListSearchModel.StartDate = orgDate;
            model.PatientTransferListSearchModel.EndDate = orgDate;

            model.PatientPendingListSearchModel.StartDate = orgDate;
            model.PatientPendingListSearchModel.EndDate = orgDate;

            model.PatientOnHoldListSearchModel.StartDate = orgDate;
            model.PatientOnHoldListSearchModel.EndDate = orgDate;

            model.PatientMedicaidListSearchModel.StartDate = orgDate;
            model.PatientMedicaidListSearchModel.EndDate = orgDate;

            model.EmpClockInOutListResetSearchModel.StartDate = orgDate;
            model.EmpClockInOutListResetSearchModel.EndDate = orgDate;

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }


       
        public ServiceResponse HC_GetEmpClockInOutList(long loggedInUser, DateTime? startDate, DateTime? endDate, string employeeName, string careTypeID, string status, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10, string TimeSlots = "", string RegionID = "")
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            if (startDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(startDate.Value).ToString(Constants.DbDateFormat) });
            if (endDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(endDate.Value).ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "EmployeeName", Value = employeeName });
            searchList.Add(new SearchValueData { Name = "CareTypeID", Value = careTypeID });
            searchList.Add(new SearchValueData { Name = "Status", Value = status });
            searchList.Add(new SearchValueData { Name = "TimeSlots", Value = TimeSlots });
            searchList.Add(new SearchValueData { Name = "RegionID", Value = RegionID });
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);

            List<GetEmpClockInOutListModel> data = GetEntityList<GetEmpClockInOutListModel>(StoredProcedure.GetEmpClockInOutList, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;
            Page<GetEmpClockInOutListModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }
        public ServiceResponse GetPatientAddressList(long loggedInUser, DateTime? startDate, DateTime? endDate, string employeeName, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 100)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            if (startDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(startDate.Value).ToString(Constants.DbDateFormat) });
            if (endDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(endDate.Value).ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "EmployeeName", Value = employeeName });
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);

            List<GetEmpClockInOutListModel> data = GetEntityList<GetEmpClockInOutListModel>(StoredProcedure.GetPatientAddress, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;
            Page<GetEmpClockInOutListModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }




        public ServiceResponse HC_GetEmpOverTimeList(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>
            {
                    new SearchValueData { Name = "WeekStartDay", Value = Convert.ToString(Common.GetCalWeekStartDay()) },
            };
            if (startDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(startDate.Value).ToString(Constants.DbDateFormat) });
            if (endDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(endDate.Value).ToString(Constants.DbDateFormat) });
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);

            List<GetEmpOverTimeListModel> data = GetEntityList<GetEmpOverTimeListModel>(StoredProcedure.GetEmpOverTimeList, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;
            Page<GetEmpOverTimeListModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }




        public ServiceResponse HC_GetPatientNewList(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            //List<SearchValueData> searchList = new List<SearchValueData>();
            //SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            if (startDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(startDate.Value).ToString(Constants.DbDateFormat) });
            if (endDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(endDate.Value).ToString(Constants.DbDateFormat) });
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);

            List<GetNewPatientListModel> data = GetEntityList<GetNewPatientListModel>(StoredProcedure.GetNewPatientList, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;
            Page<GetNewPatientListModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }
        public ServiceResponse GetActivePatientCountList()
        {
            ServiceResponse response = new ServiceResponse();
            try
            {

                List<SearchValueData> searchList = new List<SearchValueData>();
                List<GetActivePatientCountListModel> data = GetEntityList<GetActivePatientCountListModel>(StoredProcedure.GetActivePatientCountList, searchList);

                int count = 0;
                if (data != null && data.Count > 0)
                    count = data.First().Count;
                Page<GetActivePatientCountListModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

                response.Data = model;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }



        public ServiceResponse HC_GetPatientNotScheduleList(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            if (startDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(startDate.Value).ToString(Constants.DbDateFormat) });
            if (endDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(endDate.Value).ToString(Constants.DbDateFormat) });
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);

            List<GetPatientNotScheduledListModel> data = GetEntityList<GetPatientNotScheduledListModel>(StoredProcedure.GetPatientNotScheduleList, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;
            Page<GetPatientNotScheduledListModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }



        public ServiceResponse HC_GetEmployeeTimeStatics(DateTime? startDate, DateTime? endDate)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (startDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(startDate.Value).ToString(Constants.DbDateFormat) });
            if (endDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(endDate.Value).ToString(Constants.DbDateFormat) });

            List<GetEmployeeTimeStaticsList> data = GetEntityList<GetEmployeeTimeStaticsList>(StoredProcedure.GetEmployeeTimeStatics, searchList);


            GetEmpTimeStaticsListModel model = new GetEmpTimeStaticsListModel();
            model.EmployeeList = data.OrderBy(c => c.Employee).Select(c => c.Employee).ToList();
            model.AvgDelayList = data.OrderBy(c => c.Employee).Select(c => c.AvgDelay).ToList();
            model.ColorList = data.OrderBy(c => c.Employee).Select(c => c.Color).ToList();

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }



        public ServiceResponse SaveFormLoad(NameValueDataInString model)
        {
            ServiceResponse response = new ServiceResponse();


            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "FormHTML", Value = model.Value });
            NameValueDataInString data = GetEntity<NameValueDataInString>("SaveCustomForm", searchList);
            response.Data = data;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetNotification()
        {
            var mylist = new List<LateClockInNotificationModel>();
            ServiceResponse response = new ServiceResponse();

            var searchlist = new List<SearchValueData>
            {
            };
            List<LateClockInNotificationModel> totalData = GetEntityList<LateClockInNotificationModel>(StoredProcedure.NotificationForEmployeeLateClockIn, searchlist);
            foreach (var item in totalData)
            {
                mylist.Add(new LateClockInNotificationModel
                {
                    FirstName = item.FirstName.ToString(),
                    LastName = item.LastName.ToString(),
                    ScheduleDate = Convert.ToDateTime(item.ScheduleDate).ToShortDateString(),
                    ScheduleEndDate = Convert.ToDateTime(item.ScheduleEndDate).ToShortDateString(),
                    StartTime = Convert.ToDateTime(item.StartTime).ToShortTimeString(),
                    ClockInTime = Convert.ToDateTime(item.ClockInTime).ToShortTimeString(),
                    ClockOutTime = Convert.ToDateTime(item.ClockOutTime).ToShortTimeString(),
                });
            }
            response.IsSuccess = true;
            response.Data = mylist;
            return response;
        }

        public ServiceResponse HC_GetNotificationsList(long loggedInUser, string IsDeleted, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            SetSearchList(loggedInUser, IsDeleted, sortDirection, sortIndex, pageIndex, pageSize, searchList);

            List<WebNotificationModel> data = GetEntityList<WebNotificationModel>(StoredProcedure.GetNotifications, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().MsgCount;
            Page<WebNotificationModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetPriorAuthExpiring(long loggedInUser, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10, bool isExpired = false)
        {
            ServiceResponse response = new ServiceResponse();
            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);
            if (isExpired == true)
            {
                searchList.Add(new SearchValueData { Name = "IsExpired", Value = "1" });
            }
            List<PriorAuthExpiringModel> data = GetEntityList<PriorAuthExpiringModel>(StoredProcedure.GetPriorAuthExpiring, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;
            Page<PriorAuthExpiringModel> model = GetPageInStoredProcResultSet(1, 10, count, data);
            response.Data = model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetPatientBirthday(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();
            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            if (startDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(startDate.Value).ToString(Constants.DbDateFormat) });
            if (endDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(endDate.Value).ToString(Constants.DbDateFormat) });

            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);

            List<PatientBirthdayModel> data = GetEntityList<PatientBirthdayModel>(StoredProcedure.GetPatientBirthday, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;
            Page<PatientBirthdayModel> model = GetPageInStoredProcResultSet(1, 5, count, data);
            response.Data = model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetEmployeeBirthday(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();
            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            if (startDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(startDate.Value).ToString(Constants.DbDateFormat) });
            if (endDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(endDate.Value).ToString(Constants.DbDateFormat) });

            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);

            List<EmployeeBirthdayModel> data = GetEntityList<EmployeeBirthdayModel>(StoredProcedure.GetEmployeeBirthday, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;
            Page<EmployeeBirthdayModel> model = GetPageInStoredProcResultSet(1, 10, count, data);
            response.Data = model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetPatientClockInOutList(long loggedInUser, DateTime? startDate, DateTime? endDate, string patientName, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            if (startDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(startDate.Value).ToString(Constants.DbDateFormat) });
            if (endDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(endDate.Value).ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "PatientName", Value = patientName });
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);

            List<GetPatientClockInOutListModel> data = GetEntityList<GetPatientClockInOutListModel>(StoredProcedure.GetPatientClockInOutList, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;
            Page<GetPatientClockInOutListModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetReferralPayor(long loggedInUser)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();

            List<GetReferralPayorModel> data = GetEntityList<GetReferralPayorModel>(StoredProcedure.GetReferralPayor, searchList);
            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;

            Page<GetReferralPayorModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetPatientDischargedList(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            if (startDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(startDate.Value).ToString(Constants.DbDateFormat) });
            if (endDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(endDate.Value).ToString(Constants.DbDateFormat) });
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);

            List<GetPatientListModel> data = GetEntityList<GetPatientListModel>(StoredProcedure.GetDischargedPatientList, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;
            Page<GetPatientListModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetPatientTransferList(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            if (startDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(startDate.Value).ToString(Constants.DbDateFormat) });
            if (endDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(endDate.Value).ToString(Constants.DbDateFormat) });
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);

            List<GetPatientListModel> data = GetEntityList<GetPatientListModel>(StoredProcedure.GetTransferPatientList, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;
            Page<GetPatientListModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetPatientPendingList(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            if (startDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(startDate.Value).ToString(Constants.DbDateFormat) });
            if (endDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(endDate.Value).ToString(Constants.DbDateFormat) });
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);

            List<GetPatientListModel> data = GetEntityList<GetPatientListModel>(StoredProcedure.GetPendingPatientList, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;
            Page<GetPatientListModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetPatientOnHoldList(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            if (startDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(startDate.Value).ToString(Constants.DbDateFormat) });
            if (endDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(endDate.Value).ToString(Constants.DbDateFormat) });
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);

            List<GetPatientListModel> data = GetEntityList<GetPatientListModel>(StoredProcedure.GetOnHoldPatientList, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;
            Page<GetPatientListModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetPatientMedicaidList(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            if (startDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(startDate.Value).ToString(Constants.DbDateFormat) });
            if (endDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(endDate.Value).ToString(Constants.DbDateFormat) });
            SetSearchList(sortDirection, sortIndex, pageIndex, pageSize, searchList);

            List<GetReferralMedicaidModel> data = GetEntityList<GetReferralMedicaidModel>(StoredProcedure.GetPatientMedicaidList, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;
            Page<GetReferralMedicaidModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetCaretype()
        {
            ServiceResponse response = new ServiceResponse();

            var searchlist = new List<SearchValueData>
            {
            };
            List<ReportCareTypeListModel> totalData = GetEntityList<ReportCareTypeListModel>(StoredProcedure.GetCaretype, searchlist);
            response.IsSuccess = true;
            response.Data = totalData;
            return response;
        }
        public ServiceResponse GetRegion()
        {
            ServiceResponse response = new ServiceResponse();

            var searchlist = new List<SearchValueData>
            {
            };
            List<RegionList> totalData = GetEntityList<RegionList>(StoredProcedure.GetRegion, searchlist);
            response.IsSuccess = true;
            response.Data = totalData;
            return response;
        }

        public ServiceResponse HC_GetEmpClockInOutListWithOutStatus(long loggedInUser, DateTime? startDate, DateTime? endDate)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            if (startDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(startDate.Value).ToString(Constants.DbDateFormat) });
            if (endDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(endDate.Value).ToString(Constants.DbDateFormat) });

            List<GetEmpClockInOutListModel> data = GetEntityList<GetEmpClockInOutListModel>(StoredProcedure.GetEmpClockInOutListWithOutStatus, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;
            Page<GetEmpClockInOutListModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }

        #endregion


        #region Web Notifications

        public ServiceResponse HC_GetWebNotifications(long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                var result = GetEntityList<WebNotificationModel>(StoredProcedure.GetWebNotifications,
                    new List<SearchValueData> {
                            new SearchValueData("EmployeeID", Convert.ToString(loggedInUserID))
                     });

                response.Data = result;
                response.IsSuccess = true;

            }
            catch (Exception)
            {
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }
        public ServiceResponse HC_DeleteWebNotification(string webNotificationIDs, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                int result = (int)GetScalar(StoredProcedure.DeleteWebNotification,
                    new List<SearchValueData> {
                            new SearchValueData("WebNotificationID", webNotificationIDs),
                            new SearchValueData("EmployeeID", Convert.ToString(loggedInUserID))
                     });
                if (result > 0)
                {
                    response.IsSuccess = true;
                    response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.WebNotification);
                }
                else
                {
                    response.Message = Resource.ExceptionMessage;
                }
            }
            catch (Exception)
            {
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }

        public ServiceResponse HC_MarkAsReadWebNotifications(string webNotificationIDs, long loggedInUserID, bool IsRead)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                int result = (int)GetScalar(StoredProcedure.MarkAsReadWebNotifications,
                    new List<SearchValueData> {
                            new SearchValueData("WebNotificationIDs", webNotificationIDs),
                            new SearchValueData("EmployeeID", Convert.ToString(loggedInUserID)),
                            new SearchValueData("IsRead", Convert.ToString(IsRead))
                    });
                if (result > 0)
                {
                    response.IsSuccess = true;
                    response.Message = string.Format(Resource.RecordProcessedSuccessfully, Resource.WebNotification);
                }
                else
                {
                    response.Message = Resource.ExceptionMessage;
                }
            }
            catch (Exception)
            {
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }

        public ServiceResponse HC_GetWebNotificationsCount(long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                var result = GetEntityList<WebNotificationModel>(StoredProcedure.GetWebNotificationsCount,
                    new List<SearchValueData> {
                            new SearchValueData("EmployeeID", Convert.ToString(loggedInUserID))
                     });

                response.Data = result;
                response.IsSuccess = true;

            }
            catch (Exception)
            {
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }
        #endregion

    }
}
