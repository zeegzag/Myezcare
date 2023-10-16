using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class EmailTemplateDataProvider : BaseDataProvider, IEmailTemplateDataProvider
    {
        #region Add EmailTemplateList

        public ServiceResponse SetAddEmailTemplatePage(long emailtemplateid)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>
                          {
                              new SearchValueData { Name = "EmailTemplateID" , Value = emailtemplateid.ToString()}
                          };

                AddEmailTemplateModel emailTemplateModel = GetMultipleEntity<AddEmailTemplateModel>(StoredProcedure.GetSetAddEmailTemplate, searchParam);


                if (emailtemplateid > 0 && emailTemplateModel.EmailTemplate == null)
                {

                    response.ErrorCode = Constants.ErrorCode_NotFound;
                    response.IsSuccess = true;
                    return response;
                }
                if (emailTemplateModel.EmailTemplate == null)
                {
                    emailTemplateModel.EmailTemplate = new EmailTemplate();
                }

                response.Data = emailTemplateModel;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public List<EmailType> GetEmailType()
        {
            List<EmailType> totalData = new List<EmailType>();
            totalData.Add(new EmailType() { Value = "Email", Title = "Email" });
            totalData.Add(new EmailType() { Value = "Fax", Title = "Fax" });
            totalData.Add(new EmailType() { Value = "Notification", Title = "Notification" });
            totalData.Add(new EmailType() { Value = "SMS", Title = "SMS" });
            totalData.Add(new EmailType() { Value = "PDF", Title = "PDF" });

            return totalData;
        }

        public List<ModuleName> GetModuleNames()
        {
            List<ModuleName> totalData = new List<ModuleName>();
            totalData.Add(new ModuleName() { Value = "Employee", Title = "Employee" });
            totalData.Add(new ModuleName() { Value = "Patient", Title = "Patient" });
            totalData.Add(new ModuleName() { Value = "Scheduling", Title = "Scheduling" });
            return totalData;
        }

        public ServiceResponse GetTokenList(string name)
        {
            var response = new ServiceResponse();
            List<TokenList> totalData = new List<TokenList>();
            if (name.Length > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "Module", Value = Convert.ToString(name)},
                };

                totalData = GetEntityList<TokenList>(StoredProcedure.GetTokenList, searchlist);
                response.Data = totalData;
                response.IsSuccess = true;
            }

            return response;
        }


        public ServiceResponse GetTemplateBody(long id)
        {
            var response = new ServiceResponse();

            if (id > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "templateid", Value = Convert.ToString(id)},
                };

                EmailBody obj = GetEntity<EmailBody>(StoredProcedure.GetTemplateBody, searchlist);
                response.Data = obj;
                response.IsSuccess = true;
            }

            return response;
        }



        public ServiceResponse DeleteTemplate(long id)
        {
            var response = new ServiceResponse();

            if (id > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "emailTemplateId", Value = Convert.ToString(id)},
                };
                GetScalar(StoredProcedure.DeleteEmailTemplates, searchlist);
                response.IsSuccess = true;
                response.Message = "Template deleted successfully";
            }

            return response;
        }

        public ServiceResponse AddEmailTemplate(AddEmailTemplateModel AddEmailTemplateModel, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            if (AddEmailTemplateModel != null && AddEmailTemplateModel.EmailTemplate != null)
            {
                //AddEmailTemplateModel.IsEditMode = AddEmailTemplateModel.EmailTemplate.EmailTemplateID > 0;
                //string customWhere = string.Format("EmailTemplateTypeID='{0}'", Convert.ToString(AddEmailTemplateModel.EmailTemplate.EmailTemplateTypeID));

                //List<SearchValueData> searchModel = new List<SearchValueData>
                //    {
                //        new SearchValueData { Name = "EmailTemplateID", Value = AddEmailTemplateModel.EmailTemplate.EmailTemplateID.ToString(), IsNotEqual = true }
                //    };
                //EmailTemplate tempEmailTemplate = GetEntity<EmailTemplate>(AddEmailTemplateModel.IsEditMode ? searchModel : null, customWhere) ?? new EmailTemplate();

                //if (tempEmailTemplate.EmailTemplateTypeID > 0)
                //{
                //    response.IsSuccess = false;
                //    response.Message = string.Format(Resource.RecordAlreadyExists, Resource.EmailType);
                //    return response;
                //}

                if (AddEmailTemplateModel.IsEditMode)
                {
                    //List<SearchValueData> searchEmailTemplate = new List<SearchValueData>
                    //    {
                    //        new SearchValueData{Name = "EmailTemplateID",Value = AddEmailTemplateModel.EmailTemplate.EmailTemplateID.ToString()},
                    //    };

                    //EmailTemplate emailTemplate = GetMultipleEntity<EmailTemplate>(StoredProcedure.GetSetAddEmailTemplate, searchEmailTemplate);


                    //if (emailTemplate != null && emailTemplate.EmailTemplateID > 0)
                    //{
                    //    emailTemplate.EmailTemplateName = AddEmailTemplateModel.EmailTemplate.EmailTemplateName;
                    //    emailTemplate.EmailTemplateSubject = AddEmailTemplateModel.EmailTemplate.EmailTemplateSubject;
                    //    emailTemplate.EmailTemplateBody = AddEmailTemplateModel.EmailTemplate.EmailTemplateBody;
                    //    emailTemplate.Token = AddEmailTemplateModel.EmailTemplate.Token;
                    //    response = SaveObject(emailTemplate, loggedInUserID);
                    //    response.IsSuccess = true;
                    //}

                    var searchlist = new List<SearchValueData>
                        {
                            new SearchValueData {Name = "EmailTemplateID", Value = Convert.ToString(AddEmailTemplateModel.EmailTemplate.EmailTemplateID)},
                            new SearchValueData {Name = "EmailTemplateName", Value = Convert.ToString(AddEmailTemplateModel.EmailTemplate.EmailTemplateName)},
                            new SearchValueData {Name = "EmailTemplateSubject", Value = Convert.ToString(AddEmailTemplateModel.EmailTemplate.EmailTemplateSubject)},
                            new SearchValueData {Name = "EmailTemplateBody ", Value = Convert.ToString(AddEmailTemplateModel.EmailTemplate.EmailTemplateBody)},
                            new SearchValueData {Name = "UpdatedBy", Value = Convert.ToString(loggedInUserID)},
                            new SearchValueData {Name = "SystemID", Value = Convert.ToString("1")},
                            new SearchValueData {Name = "EmailType", Value = Convert.ToString(AddEmailTemplateModel.EmailTemplate.Email)},
                            new SearchValueData {Name = "Module", Value = Convert.ToString(AddEmailTemplateModel.EmailTemplate.Module)},
                        };
                    GetScalar(StoredProcedure.UpdateEmailTemplate, searchlist);
                    response.IsSuccess = true;
                    response.Message = "Template updated successfully";



                }
                else
                {
                    var searchlist = new List<SearchValueData>
                        {
                            new SearchValueData {Name = "EmailTemplateName", Value = Convert.ToString(AddEmailTemplateModel.EmailTemplate.EmailTemplateName)},
                            new SearchValueData {Name = "EmailTemplateSubject", Value = Convert.ToString(AddEmailTemplateModel.EmailTemplate.EmailTemplateSubject)},
                            new SearchValueData {Name = "EmailTemplateBody", Value = Convert.ToString(AddEmailTemplateModel.EmailTemplate.EmailTemplateBody)},
                            new SearchValueData {Name = "CreatedBy", Value = Convert.ToString(loggedInUserID)},
                            new SearchValueData {Name = "SystemID", Value = Convert.ToString("1")},
                            new SearchValueData {Name = "EmailType", Value = Convert.ToString(AddEmailTemplateModel.EmailTemplate.Email)},
                            new SearchValueData {Name = "Module", Value = Convert.ToString(AddEmailTemplateModel.EmailTemplate.Module)},
                        };
                    GetScalar(StoredProcedure.SaveEmailTemplate, searchlist);
                    response.IsSuccess = true;
                    response.Message = "Template saved successfully";
                }
                //response.Message = !AddEmailTemplateModel.IsEditMode ? string.Format(Resource.RecordCreatedSuccessfully, Resource.Template)
                //                       : string.Format(Resource.RecordUpdatedSuccessfully, Resource.Template);

            }
            return response;
        }

        #endregion

        #region EmailTemplateList List

        public ServiceResponse SetEmailTemplateListPage()
        {
            var response = new ServiceResponse();

            SetEmailTemplateListPage setEmailTemplateListPage = new SetEmailTemplateListPage();
            setEmailTemplateListPage.DeleteFilter = Common.SetDeleteFilter();
            setEmailTemplateListPage.SearchEmailTemplateListPage.IsDeleted = 0;
            response.Data = setEmailTemplateListPage;
            return response;
        }

        public ServiceResponse GetEmailTemplateList(SearchEmailTemplateListPage searchEmailTemplateListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();

                if (searchEmailTemplateListPage != null)
                    SetSearchFilterForTransportLocationListPage(searchEmailTemplateListPage, searchList);
                Page<ListEmailTemplateModel> listEmailTemplateModel = GetEntityPageList<ListEmailTemplateModel>(StoredProcedure.GetEmailTemplateList, searchList, pageSize,
                                                              pageIndex, sortIndex, sortDirection);
                response.Data = listEmailTemplateModel;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.Template), Resource.ExceptionMessage);
            }
            return response;
        }

        private static void SetSearchFilterForTransportLocationListPage(SearchEmailTemplateListPage searchEmailTemplateListPage, List<SearchValueData> searchList)
        {
            if (searchEmailTemplateListPage.EmailTemplateID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "EmailTemplateID",
                    Value = Convert.ToString(searchEmailTemplateListPage.EmailTemplateID)
                });

            if (!string.IsNullOrEmpty(searchEmailTemplateListPage.EmailTemplateName))
                searchList.Add(new SearchValueData { Name = "EmailTemplateName", Value = Convert.ToString(searchEmailTemplateListPage.EmailTemplateName) });

            if (!string.IsNullOrEmpty(searchEmailTemplateListPage.EmailTemplateSubject))
                searchList.Add(new SearchValueData { Name = "EmailTemplateSubject", Value = Convert.ToString(searchEmailTemplateListPage.EmailTemplateSubject) });

            if (!string.IsNullOrEmpty(searchEmailTemplateListPage.Tokens))
                searchList.Add(new SearchValueData { Name = "Token", Value = Convert.ToString(searchEmailTemplateListPage.Tokens) });

            if (!string.IsNullOrEmpty(searchEmailTemplateListPage.AddedBy))
                searchList.Add(new SearchValueData { Name = "AddedBy", Value = Convert.ToString(searchEmailTemplateListPage.AddedBy) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchEmailTemplateListPage.IsDeleted) });

            if (!string.IsNullOrEmpty(searchEmailTemplateListPage.EmailType))
                searchList.Add(new SearchValueData { Name = "EmailType", Value = Convert.ToString(searchEmailTemplateListPage.EmailType) });

            if (!string.IsNullOrEmpty(searchEmailTemplateListPage.Module))
                searchList.Add(new SearchValueData { Name = "module", Value = Convert.ToString(searchEmailTemplateListPage.Module) });
        }

        public ServiceResponse DeleteEmailTemplate(SearchEmailTemplateListPage searchEmailTemplateListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

                SetSearchFilterForTransportLocationListPage(searchEmailTemplateListPage, searchList);

                if (!string.IsNullOrEmpty(searchEmailTemplateListPage.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchEmailTemplateListPage.ListOfIdsInCSV });

                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

                List<ListEmailTemplateModel> totalData = GetEntityList<ListEmailTemplateModel>(StoredProcedure.DeleteEmailTemplate, searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListEmailTemplateModel> getEmailTemplateList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);

                response.Data = getEmailTemplateList;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Template);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }

        #endregion
    }
}
