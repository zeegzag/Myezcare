using HomeCareApi.Infrastructure.IDataProvider;
using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.Entity;
using HomeCareApi.Models.General;
using HomeCareApi.Models.ViewModel;
using HomeCareApi.Resources;
using Newtonsoft.Json;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace HomeCareApi.Infrastructure.DataProvider
{
    public class OrbeonFormsDataProvider : BaseDataProvider, IOrbeonFormsDataProvider
    {
        public OrbeonFormsDataProvider(long organizationID)
            : base(organizationID)
        {
        }

        public ApiResponse GetCMS485Data(CMS485DataRequest request)
        {
            ApiResponse response;
            CMS485Data formData = new CMS485Data();
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.ReferralID)));

                formData = GetMultipleEntity<CMS485Data>(StoredProcedure.API_GetCMS485Data, srchParam);
                if (formData?.Referral?.ReferralID == request.ReferralID)
                {
                    response = Common.ApiCommonResponse(true, Resource.DataFetchedSuccessfully, StatusCode.Ok, formData);
                }
                else
                { response = Common.ApiCommonResponse(false, Resource.RecordNotFound, StatusCode.BadRequest, formData); }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        #region Referral Detail
        public ApiResponse GetOrganizationDetail(ReferralChartDataRequest request)
        {
            ApiResponse response;
            OrganizationData formData = new OrganizationData();
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.OrganizationID, Convert.ToString(request.OrganizationID)));

                formData = GetMultipleEntity<OrganizationData>(StoredProcedure.GetOrganizationData, srchParam);
                if (formData?.OrganizationSettings != null)
                {
                    response = Common.ApiCommonResponse(true, Resource.DataFetchedSuccessfully, StatusCode.Ok, formData);
                }
                else
                { response = Common.ApiCommonResponse(false, Resource.RecordNotFound, StatusCode.BadRequest, formData); }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse GetReferralPersonalDetail(ReferralChartDataRequest request)
        {
            ApiResponse response;
            ReferralPersonalList formData = new ReferralPersonalList();
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.ReferralID)));
                srchParam.Add(new SearchValueData(Properties.OrganizationID, Convert.ToString(request.OrganizationID)));

                formData = GetMultipleEntity<ReferralPersonalList>(StoredProcedure.GetPatientPersonalData, srchParam);
                if (formData?.ReferralPersonalModel.ReferralID == request.ReferralID)
                {
                    response = Common.ApiCommonResponse(true, Resource.DataFetchedSuccessfully, StatusCode.Ok, formData);
                }
                else
                { response = Common.ApiCommonResponse(false, Resource.RecordNotFound, StatusCode.BadRequest, formData); }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse GetReferralContactDetail(ReferralChartDataRequest request)
        {
            ApiResponse response;
            ReferralContactList formData = new ReferralContactList();
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.ReferralID)));

                formData = GetMultipleEntity<ReferralContactList>(StoredProcedure.GetPatientContactData, srchParam);
                if (formData?.ReferralContactModel.Count != 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.DataFetchedSuccessfully, StatusCode.Ok, formData);
                }
                else
                { response = Common.ApiCommonResponse(false, Resource.RecordNotFound, StatusCode.BadRequest, formData); }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse GetReferralPhysicianDetail(ReferralChartDataRequest request)
        {
            ApiResponse response;
            ReferralPhysicianList formData = new ReferralPhysicianList();
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.ReferralID)));

                formData = GetMultipleEntity<ReferralPhysicianList>(StoredProcedure.GetPatientPhysicianData, srchParam);
                if (formData?.ReferralPhysicianModel.Count != 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.DataFetchedSuccessfully, StatusCode.Ok, formData);
                }
                else
                { response = Common.ApiCommonResponse(false, Resource.RecordNotFound, StatusCode.BadRequest, formData); }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse GetReferralDxCodeDetail(ReferralChartDataRequest request)
        {
            ApiResponse response;
            ReferralDXCodeList formData = new ReferralDXCodeList();
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.ReferralID)));

                formData = GetMultipleEntity<ReferralDXCodeList>(StoredProcedure.GetPatientDxCodeData, srchParam);
                if (formData?.ReferralDXCodeModel.Count != 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.DataFetchedSuccessfully, StatusCode.Ok, formData);
                }
                else
                { response = Common.ApiCommonResponse(false, Resource.RecordNotFound, StatusCode.BadRequest, formData); }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse GetReferralMedicationDetail(ReferralChartDataRequest request)
        {
            ApiResponse response;
            ReferralMedicationList formData = new ReferralMedicationList();
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.ReferralID)));

                formData = GetMultipleEntity<ReferralMedicationList>(StoredProcedure.GetPatientMedicationData, srchParam);
                if (formData?.ReferralMedicationModel.Count != 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.DataFetchedSuccessfully, StatusCode.Ok, formData);
                }
                else
                { response = Common.ApiCommonResponse(false, Resource.RecordNotFound, StatusCode.BadRequest, formData); }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse GetReferralNotesDetail(ReferralChartDataRequest request)
        {
            ApiResponse response;
            ReferralNotesList formData = new ReferralNotesList();
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.ReferralID)));

                formData = GetMultipleEntity<ReferralNotesList>(StoredProcedure.GetPatientNotesData, srchParam);
                if (formData?.ReferralNotesModel.Count != 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.DataFetchedSuccessfully, StatusCode.Ok, formData);
                }
                else
                { response = Common.ApiCommonResponse(false, Resource.RecordNotFound, StatusCode.BadRequest, formData); }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse GetReferralPreferencesDetail(ReferralChartDataRequest request)
        {
            ApiResponse response;
            ReferralPreferencesList formData = new ReferralPreferencesList();
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.ReferralID)));

                formData = GetMultipleEntity<ReferralPreferencesList>(StoredProcedure.GetPatientPreferencesData, srchParam);
                if (formData?.ReferralPreferencesModel.Count != 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.DataFetchedSuccessfully, StatusCode.Ok, formData);
                }
                else
                { response = Common.ApiCommonResponse(false, Resource.RecordNotFound, StatusCode.BadRequest, formData); }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse GetReferralTaskMappingsDetail(ReferralChartDataRequest request)
        {
            ApiResponse response;
            ReferralTaskMappingList formData = new ReferralTaskMappingList();
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.ReferralID)));

                formData = GetMultipleEntity<ReferralTaskMappingList>(StoredProcedure.GetPatientTaskMappingsData, srchParam);
                if (formData?.ReferralTaskMappingModel.Count != 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.DataFetchedSuccessfully, StatusCode.Ok, formData);
                }
                else
                { response = Common.ApiCommonResponse(false, Resource.RecordNotFound, StatusCode.BadRequest, formData); }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse GetReferralPayorDetail(ReferralChartDataRequest request)
        {
            ApiResponse response;
            ReferralPayorList formData = new ReferralPayorList();
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.ReferralID)));

                formData = GetMultipleEntity<ReferralPayorList>(StoredProcedure.GetPatientPayorData, srchParam);
                if (formData?.ReferralPayorModel.Count != 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.DataFetchedSuccessfully, StatusCode.Ok, formData);
                }
                else
                { response = Common.ApiCommonResponse(false, Resource.RecordNotFound, StatusCode.BadRequest, formData); }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse GetReferralAllergyDetail(ReferralChartDataRequest request)
        {
            ApiResponse response;
            ReferralAllergyList formData = new ReferralAllergyList();
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.ReferralID)));

                formData = GetMultipleEntity<ReferralAllergyList>(StoredProcedure.GetPatientAllergyData, srchParam);
                if (formData?.ReferralAllergyModel.Count != 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.DataFetchedSuccessfully, StatusCode.Ok, formData);
                }
                else
                { response = Common.ApiCommonResponse(false, Resource.RecordNotFound, StatusCode.BadRequest, formData); }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse GetReferralAllergyDetailAsCommaSeparated(ReferralChartDataRequest request)
        {
            ApiResponse response;
            ReferralAllergyList formData = new ReferralAllergyList();
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.ReferralID)));

                formData = GetMultipleEntity<ReferralAllergyList>(StoredProcedure.GetPatientAllergyDataAsCommaSeparted, srchParam);
                if (formData?.ReferralAllergyModel.Count != 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.DataFetchedSuccessfully, StatusCode.Ok, formData);
                }
                else
                { response = Common.ApiCommonResponse(false, Resource.RecordNotFound, StatusCode.BadRequest, formData); }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse GetReferralDxCodeDetailAsCommaSeparated(ReferralChartDataRequest request)
        {
            ApiResponse response;
            ReferralDXCodeList formData = new ReferralDXCodeList();
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.ReferralID)));

                formData = GetMultipleEntity<ReferralDXCodeList>(StoredProcedure.GetPatientDxCodeDataAsCommaSeparted, srchParam);
                if (formData?.ReferralDXCodeModel.Count != 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.DataFetchedSuccessfully, StatusCode.Ok, formData);
                }
                else
                { response = Common.ApiCommonResponse(false, Resource.RecordNotFound, StatusCode.BadRequest, formData); }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse GetReferralBillingAuthorizationDetail(ReferralChartDataRequest request)
        {
            ApiResponse response;
            ReferralBillingAuthorizationList formData = new ReferralBillingAuthorizationList();
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.ReferralID)));

                formData = GetMultipleEntity<ReferralBillingAuthorizationList>(StoredProcedure.GetPatientBillingAuthorizationData, srchParam);
                if (formData?.ReferralBillingAuthorizationModel.Count != 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.DataFetchedSuccessfully, StatusCode.Ok, formData);
                }
                else
                { response = Common.ApiCommonResponse(false, Resource.RecordNotFound, StatusCode.BadRequest, formData); }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }
        #endregion

        #region Employee Detail
        public ApiResponse GetEmployeePersonalDetail(DataRequest request)
        {
            ApiResponse response;
            EmployeePersonalList formData = new EmployeePersonalList();
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.OrganizationID, Convert.ToString(request.OrganizationID)));

                formData = GetMultipleEntity<EmployeePersonalList>(StoredProcedure.GetEmployeesPersonalData, srchParam);
                if (formData?.EmployeePersonalModel != null)
                {
                    response = Common.ApiCommonResponse(true, Resource.DataFetchedSuccessfully, StatusCode.Ok, formData);
                }
                else
                { response = Common.ApiCommonResponse(false, Resource.RecordNotFound, StatusCode.BadRequest, formData); }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }
        #endregion
    }
}