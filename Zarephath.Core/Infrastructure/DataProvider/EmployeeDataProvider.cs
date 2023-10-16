using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;
using System.Net;
using System.Globalization;
using Microsoft.SqlServer.Types;
using System.Web;
using Newtonsoft.Json;
using System.IO;
using Zarephath.Core.Infrastructure.Utility.Fcm;
using Zarephath.Core.Infrastructure.Utility;
using static Zarephath.Core.Infrastructure.Common;
using DocumentFormat.OpenXml.EMMA;
using System.Net.Mail;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class EmployeeDataProvider : BaseDataProvider, IEmployeeDataProvider
    {
        CacheHelper _cacheHelper = new CacheHelper();

        #region Add Employee

        public ServiceResponse SetAddEmployeePage(long employeeID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchParam = new List<SearchValueData>();

            try
            {
                searchParam.Add(new SearchValueData { Name = "IsDeleted", Value = "0" });
                var addEmployeeModel = new AddEmployeeModel
                {
                    SecurityQuestionList = GetEntityList<SecurityQuestion>(),
                    RoleList = GetEntityList<Role>(null, "", "RoleName", "ASC"),
                    CredentialList = GetEntityList<EmployeeCredential>(null, "", "CredentialID", "ASC"),
                    Employee = { IsActive = true }
                };
                if (employeeID == 0)
                {
                    addEmployeeModel.DepartmentList = GetEntityList<Department>(searchParam, "", "DepartmentName", "ASC");
                }
                if (employeeID > 0)
                {
                    List<SearchValueData> searchEmployee = new List<SearchValueData>
                        {
                            new SearchValueData { Name = "EmployeeID" , Value = employeeID.ToString()},
                            //new SearchValueData { Name = "IsDeleted" , Value = "0"},
                        };

                    addEmployeeModel.Employee = GetEntity<Employee>(searchEmployee);
                    if (addEmployeeModel.Employee.EmployeeSignatureID > 0)
                    {
                        EmployeeSignature employeeSignature =
                            GetEntity<EmployeeSignature>(addEmployeeModel.Employee.EmployeeSignatureID);
                        if (employeeSignature != null)
                        {
                            addEmployeeModel.Employee.TempSignaturePath = employeeSignature.SignaturePath;
                        }
                    }
                    string custWhere;
                    if (addEmployeeModel.Employee.DepartmentID > 0)
                    {
                        custWhere = string.Format("IsDeleted = {0} OR DepartmentID={1}", 0, addEmployeeModel.Employee.DepartmentID);
                    }
                    else
                    {
                        custWhere = string.Format("IsDeleted = {0}", 0);
                    }
                    addEmployeeModel.DepartmentList = GetEntityList<Department>(null, custWhere, "DepartmentName", "ASC");
                    if (addEmployeeModel.Employee != null && addEmployeeModel.Employee.EmployeeID > 0)
                    {
                        var path = ConfigSettings.AmazoneUploadPath + ConfigSettings.TempFiles + addEmployeeModel.Employee.EmployeeID + "/";
                        addEmployeeModel.AmazonSettingModel = AmazonFileUpload.GetAmazonModelForClientSideUpload(SessionHelper.LoggedInID, path, ConfigSettings.PublicAcl);

                        addEmployeeModel.IsEditMode = true;
                        response.IsSuccess = true;
                    }
                    else
                    {
                        addEmployeeModel.Employee = new Employee { IsActive = true };
                        response.ErrorCode = Constants.ErrorCode_NotFound;
                        return response;
                    }





                }
                response.Data = addEmployeeModel;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }


            return response;
        }


        public ServiceResponse HC_AddEmployeeSSNLog(HC_AddEmployeeModel addEmployeeModel, long loggedInUserID)
        {
            AuditLogTable audit = new AuditLogTable();
            audit.AuditActionType = "ViewSSSN";
            audit.DataModel = "Employee";
            audit.DateTimeStamp = DateTime.UtcNow; ;
            audit.ParentKeyFieldID = addEmployeeModel.Employee.EmployeeID;
            audit.ChildKeyFieldID = addEmployeeModel.Employee.EmployeeID;
            audit.ValueBefore = JsonConvert.SerializeObject(addEmployeeModel.Employee);
            audit.ValueAfter = JsonConvert.SerializeObject(addEmployeeModel.Employee);
            audit.Changes = "Show Employee SSN ";
            audit.SystemID = Common.GetHostAddress();
            IAuditLogDataProvider auditLogDataProvider = new AuditLogDataProvider();
            ServiceResponse response = auditLogDataProvider.AddAuditLog(audit, loggedInUserID);
            return response;
        }


        public ServiceResponse AddEmployee(AddEmployeeModel addEmployeeModel, long loggedInUserID)
        {
            var response = new ServiceResponse();

            try
            {
                if (loggedInUserID > 0)
                {
                    if (addEmployeeModel != null && addEmployeeModel.Employee != null)
                    {
                        addEmployeeModel.IsEditMode = addEmployeeModel.Employee.EmployeeID > 0;
                        //bool isEmailChanged = false;
                        var searchParam = new List<SearchValueData>();

                        #region Check if Department is assigned to another Employee
                        //**As Per Client Request Removed This Check

                        //if (addEmployeeModel.Employee.DepartmentID > 0)
                        //{
                        //    searchParam.Add(new SearchValueData { Name = "DepartmentID", Value = addEmployeeModel.Employee.DepartmentID.ToString(), IsEqual = true });
                        //    searchParam.Add(new SearchValueData { Name = "IsDeleted", Value = "0", IsEqual = true });

                        //    if (addEmployeeModel.Employee.IsDepartmentSupervisor)
                        //        searchParam.Add(new SearchValueData { Name = "IsDepartmentSupervisor", Value = "1" });

                        //    if (addEmployeeModel.IsEditMode)
                        //        searchParam.Add(new SearchValueData
                        //        {
                        //            Name = "EmployeeID",
                        //            Value = addEmployeeModel.Employee.EmployeeID.ToString(),
                        //            IsNotEqual = true
                        //        });

                        //    var employeeList = GetEntityList<Employee>(searchParam);
                        //    if (employeeList.Any() && addEmployeeModel.Employee.IsDepartmentSupervisor)
                        //    {
                        //        response.Message = Resource.DepartmentAlreadyAssiged;
                        //        return response;
                        //    }
                        //}

                        #endregion

                        #region Check UserName Exists

                        // This condition will check for the User Name Unique
                        if (!string.IsNullOrEmpty(addEmployeeModel.Employee.UserName))
                        {
                            searchParam.Clear();
                            searchParam.Add(new SearchValueData { Name = "UserName", Value = addEmployeeModel.Employee.UserName, IsEqual = true });
                            searchParam.Add(new SearchValueData { Name = "IsDeleted", Value = "1", IsEqual = true });
                            if (addEmployeeModel.IsEditMode)
                                searchParam.Add(new SearchValueData
                                {
                                    Name = "EmployeeID",
                                    Value = addEmployeeModel.Employee.EmployeeID.ToString(),
                                    IsNotEqual = true
                                });

                            var employee = GetEntity<Employee>(searchParam);
                            if (employee != null && employee.EmployeeID > 0)
                            {
                                response.Message = Resource.UserNameAlreadyExists;
                                return response;
                            }
                        }

                        #endregion

                        #region Check Email Address Exists

                        // This condition will check for the Email Unique
                        // COMMENTED FOR THE WAHIBA
                        if (!string.IsNullOrEmpty(addEmployeeModel.Employee.Email))
                        {
                            searchParam.Clear();
                            searchParam.Add(new SearchValueData { Name = "Email", Value = addEmployeeModel.Employee.Email, IsEqual = true });
                            if (addEmployeeModel.IsEditMode)
                                searchParam.Add(new SearchValueData
                                {
                                    Name = "EmployeeID",
                                    Value = addEmployeeModel.Employee.EmployeeID.ToString(),
                                    IsNotEqual = true
                                });

                            var employee = GetEntity<Employee>(searchParam);
                            if (employee != null && employee.EmployeeID > 0)
                            {
                                response.Message = Resource.EmailAddressAlreadyExists;
                                return response;
                            }
                        }

                        #endregion




                        #region Check Signature Exist

                        //if (string.IsNullOrEmpty(addEmployeeModel.Employee.EmpSignature))
                        //{
                        //    response.Message = Resource.EmpSignatureRequired;
                        //    return response;
                        //}
                        #endregion

                        PasswordDetail passwordDetail = Common.CreatePassword(addEmployeeModel.Employee.Password);

                        // This block will execute when the user is in the Edit Mode
                        if (addEmployeeModel.IsEditMode)
                        {
                            var editEmployee = GetEntity<Employee>(addEmployeeModel.Employee.EmployeeID);

                            //Check for the existing password is changed or not.
                            if (!string.IsNullOrEmpty(addEmployeeModel.Employee.NewPassword))
                            {
                                passwordDetail = Common.CreatePassword(addEmployeeModel.Employee.NewPassword);
                                editEmployee.Password = passwordDetail.Password;
                                editEmployee.PasswordSalt = passwordDetail.PasswordSalt;
                            }


                            if (editEmployee != null && editEmployee.EmployeeID > 0)
                            {
                                #region SIGN
                                if (addEmployeeModel.Employee.EmployeeSignatureID > 0)
                                {
                                    EmployeeSignature employeeSignature =
                                        GetEntity<EmployeeSignature>(addEmployeeModel.Employee.EmployeeSignatureID);
                                    if (employeeSignature != null)
                                    {
                                        addEmployeeModel.TempSignaturePath = employeeSignature.SignaturePath;
                                    }
                                }
                                if (addEmployeeModel.TempSignaturePath != addEmployeeModel.Employee.TempSignaturePath)
                                {
                                    AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                                    string bucket = ConfigSettings.ZarephathBucket;
                                    string destPath = ConfigSettings.AmazoneUploadPath + ConfigSettings.EmpSignatures +
                                                      editEmployee.EmployeeID +
                                                      addEmployeeModel.Employee.TempSignaturePath.Substring(
                                                          addEmployeeModel.Employee.TempSignaturePath.LastIndexOf('/'));

                                    amazonFileUpload.MoveFile(bucket, addEmployeeModel.Employee.TempSignaturePath, bucket, destPath, S3CannedACL.PublicRead);

                                    EmployeeSignature employeeSignature = new EmployeeSignature
                                    {
                                        EmployeeID = loggedInUserID,
                                        SignaturePath = destPath
                                    };
                                    SaveEntity(employeeSignature);
                                    editEmployee.EmployeeSignatureID = employeeSignature.EmployeeSignatureID;
                                }

                                #endregion SIGN
                                //isEmailChanged = addEmployeeModel.Employee.Email != editEmployee.Email;
                                editEmployee.FirstName = addEmployeeModel.Employee.FirstName;
                                editEmployee.MiddleName = addEmployeeModel.Employee.MiddleName;
                                editEmployee.LastName = addEmployeeModel.Employee.LastName;
                                editEmployee.Email = addEmployeeModel.Employee.Email;
                                editEmployee.SocialSecurityNumber = !string.IsNullOrEmpty(addEmployeeModel.Employee.SocialSecurityNumber) ? Crypto.Decrypt(addEmployeeModel.Employee.SocialSecurityNumber) : "";
                                editEmployee.PhoneWork = addEmployeeModel.Employee.PhoneWork;
                                editEmployee.PhoneHome = addEmployeeModel.Employee.PhoneHome;
                                editEmployee.IsDepartmentSupervisor = addEmployeeModel.Employee.IsDepartmentSupervisor;
                                editEmployee.UserName = addEmployeeModel.Employee.UserName;
                                editEmployee.IsVerify = true;
                                // editEmployee.IsVerify = addEmployeeModel.Employee.IsVerify;
                                editEmployee.EmpSignature = addEmployeeModel.Employee.EmpSignature;
                                editEmployee.IsActive = addEmployeeModel.Employee.IsActive;
                                editEmployee.RoleID = addEmployeeModel.Employee.RoleID;
                                editEmployee.DepartmentID = addEmployeeModel.Employee.DepartmentID;
                                editEmployee.CredentialID = addEmployeeModel.Employee.CredentialID;
                                editEmployee.Degree = addEmployeeModel.Employee.Degree;
                                SaveObject(editEmployee, loggedInUserID);

                                Common.CreateAuditTrail(addEmployeeModel.IsEditMode ? AuditActionType.Update : AuditActionType.Create, addEmployeeModel.Employee.EmployeeID, addEmployeeModel.Employee.EmployeeID,
                                                                 editEmployee, addEmployeeModel.Employee, loggedInUserID);
                            }

                            #region Scrap Code

                            //    #region Send Mail if the Email Address is Changed
                            //    if (isEmailChanged)
                            //    {
                            //        addEmployeeModel.Employee.IsVerify = false;

                            //        var emailToken = new EmailToken
                            //        {
                            //            VerificationLink = string.Format("{0}{1}/{2}", ConfigSettings.SiteBaseURL,
                            //                                             Constants.AccountVerificationURL,
                            //                                             Crypto.Encrypt(
                            //                                                 Convert.ToString(
                            //                                                     addEmployeeModel.Employee.EmployeeID)))
                            //        };


                            //        var emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                            //{
                            //    new SearchValueData
                            //    {
                            //        Name = "EmailTemplateType",
                            //     Value = Constants.SendEmployeeEmailChangedVerificationMail,
                            //        IsEqual = true
                            //    }
                            //});

                            //        emailTemplate.EmailTemplateBody = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, emailToken);
                            //        Common.SendEmail(emailTemplate.EmailTemplateSubject, "", addEmployeeModel.Employee.Email, emailTemplate.EmailTemplateBody);

                            //    }

                            //    #endregion

                            #endregion

                        }
                        else
                        {
                            // This block will save the new entry of employee in the database
                            addEmployeeModel.Employee.IsVerify = true;
                            addEmployeeModel.Employee.Password = passwordDetail.Password;
                            addEmployeeModel.Employee.PasswordSalt = passwordDetail.PasswordSalt;
                            SaveObject(addEmployeeModel.Employee, loggedInUserID);
                        }

                        #region Scrap Code
                        //#region Send Mail to Employee for verification only if the Email Address is not verified

                        //if (!addEmployeeModel.IsEditMode && !addEmployeeModel.Employee.IsVerify)
                        //{
                        //    EncryptedMailMessageToken encryptedMailMessageToken = new EncryptedMailMessageToken
                        //    {
                        //        EncryptedValue = addEmployeeModel.Employee.EmployeeID,
                        //        ExpireDateTime = DateTime.Now.AddHours(ConfigSettings.EmailVerificationLinkExpirationTime)
                        //    };
                        //    SaveEntity(encryptedMailMessageToken);


                        //    var emailToken = new EmailToken
                        //    {
                        //        VerificationLink = string.Format("{0}{1}/{2}", ConfigSettings.SiteBaseURL,
                        //                                         Constants.AccountVerificationURL,
                        //                                         Crypto.Encrypt(
                        //                                             Convert.ToString(
                        //                                                 encryptedMailMessageToken.EncryptedMailID)))
                        //    };


                        //    var emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                        //{
                        //    new SearchValueData
                        //    {
                        //        Name = "EmailTemplateType",
                        //        Value = Constants.SendEmployeeAccountActivationEmail,
                        //        IsEqual = true
                        //    }
                        //});

                        //    emailTemplate.EmailTemplateBody = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, emailToken);
                        //    Common.SendEmail(emailTemplate.EmailTemplateSubject, "", addEmployeeModel.Employee.Email, emailTemplate.EmailTemplateBody);
                        //    response.IsSuccess = true;
                        //    response.Message = !addEmployeeModel.IsEditMode
                        //                      ? string.Format("{0}. {1}", string.Format(Resource.RecordCreatedSuccessfully, Resource.Employee), Resource.VerificationLinkSentSuccessfully)
                        //                      : string.Format(Resource.RecordUpdatedSuccessfully, Resource.Employee);
                        //    return response;
                        //}

                        //#endregion

                        #endregion


                        response.IsSuccess = true;

                        response.Message = !addEmployeeModel.IsEditMode
                                               ? string.Format(Resource.RecordCreatedSuccessfully, Resource.Employee)
                                               : string.Format(Resource.RecordUpdatedSuccessfully, Resource.Employee);

                        // This message will be used when we are sending the verification link to employee
                        //response.Message = !addEmployeeModel.IsEditMode
                        //                       ? string.Format("{0}. {1}", string.Format(Resource.RecordCreatedSuccessfully, Resource.Employee), Resource.EmployeeAccountActivatedSuccessfully)
                        //                       : string.Format(Resource.RecordUpdatedSuccessfully, Resource.Employee);
                    }
                    else
                        response.Message = Common.MessageWithTitle(string.Format(Resource.CreateFailed, Resource.Employee), Resource.RecordNotFound);
                }
                else
                    response.Message = Common.MessageWithTitle(Resource.Error, Resource.UserSessionExpired);

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }

            return response;
        }
        #endregion

        #region Employee List

        public ServiceResponse SetListEmployeePage()
        {
            var response = new ServiceResponse();

            SetEmployeeListPage setEmployeeListPage = GetMultipleEntity<SetEmployeeListPage>(StoredProcedure.SetEmployeeListPage);
            setEmployeeListPage.DepartmentSupervisorStatusList = new List<NameValueData>
                {
                    new NameValueData { Name = Constants.All, Value = (int)Common.DepartmentSupervisorStatusEnum.All},
                    new NameValueData { Name = Constants.Yes, Value = (int)Common.DepartmentSupervisorStatusEnum.Yes},
                    new NameValueData { Name = Constants.No, Value = (int)Common.DepartmentSupervisorStatusEnum.No },
                };
            setEmployeeListPage.DeleteFilter = Common.SetDeleteFilter();
            setEmployeeListPage.SearchEmployeeModel.IsDeleted = 0;
            setEmployeeListPage.SearchEmployeeModel.IsSupervisor = -1;
            response.Data = setEmployeeListPage;
            return response;
        }

        public List<ListEmployeeModel> GetEmployeesForNurseSchedule(SearchEmployeeModel searchEmployeeModel, string sortIndex, string sortDirection)
        {
            //var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            searchList.Add(new SearchValueData { Name = "SortExpression", Value = Convert.ToString(sortIndex) });
            searchList.Add(new SearchValueData { Name = "SortType", Value = Convert.ToString(sortDirection) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = "0" });

            List<ListEmployeeModel> employees = new List<ListEmployeeModel>();
            //searchList.Add(new SearchValueData { Name = "EmployeeId", Value = Convert.ToString(SessionHelper.LoggedInID) });

            if (Common.HasPermission(Constants.AllRecordAccess))
            {
                employees = GetEntityList<ListEmployeeModel>(StoredProcedure.GetEmployeesForNurseSchedule, searchList);
            }
            else
            {
                searchList.Add(new SearchValueData { Name = "EmployeeId", Value = Convert.ToString(SessionHelper.LoggedInID) });
                employees = GetEntityList<ListEmployeeModel>(StoredProcedure.GetEmployeeLimitedRecordsForNurseSchedule, searchList);

            }

            return employees;
        }

        public ServiceResponse GetEmployeeList(SearchEmployeeModel searchEmployeeModel, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            SetSearchFilterForEmployeeListPage(searchEmployeeModel, searchList, Convert.ToInt32(SessionHelper.LoggedInID));

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));
            List<ListEmployeeModel> totalData = new List<ListEmployeeModel>();
            //searchList.Add(new SearchValueData { Name = "EmployeeId", Value = Convert.ToString(SessionHelper.LoggedInID) });

            totalData = GetEntityList<ListEmployeeModel>(StoredProcedure.GetEmployeeList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListEmployeeModel> getEmployeeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getEmployeeList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForEmployeeListPage(SearchEmployeeModel searchEmployeeModel, List<SearchValueData> searchList, long loggedInUserID)
        {
            if (!string.IsNullOrEmpty(searchEmployeeModel.EmployeeUniqueID))
                searchList.Add(new SearchValueData { Name = "EmployeeUniqueID", Value = Convert.ToString(searchEmployeeModel.EmployeeUniqueID) });
            if (!string.IsNullOrEmpty(searchEmployeeModel.Name))
                searchList.Add(new SearchValueData { Name = "Name", Value = Convert.ToString(searchEmployeeModel.Name) });

            if (!string.IsNullOrEmpty(searchEmployeeModel.Email))
                searchList.Add(new SearchValueData { Name = "Email", Value = Convert.ToString(searchEmployeeModel.Email) });

            if (!string.IsNullOrEmpty(searchEmployeeModel.Degree))
                searchList.Add(new SearchValueData { Name = "Degree", Value = Convert.ToString(searchEmployeeModel.Degree) });

            if (!string.IsNullOrEmpty(searchEmployeeModel.CredentialID))
                searchList.Add(new SearchValueData { Name = "CredentialID", Value = Convert.ToString(searchEmployeeModel.CredentialID) });

            if (searchEmployeeModel.GroupIds != null)
                searchList.Add(new SearchValueData { Name = "GroupIDs", Value = String.Join(",", searchEmployeeModel.GroupIds) });

            if (!string.IsNullOrEmpty(searchEmployeeModel.MobileNumber))
                searchList.Add(new SearchValueData { Name = "MobileNumber", Value = Convert.ToString(searchEmployeeModel.MobileNumber) });

            if (searchEmployeeModel.EmployeeId != 0)
                searchList.Add(new SearchValueData { Name = "EmployeeId", Value = Convert.ToString(searchEmployeeModel.EmployeeId) });

            if (searchEmployeeModel.DepartmentID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "DepartmentID",
                    Value = Convert.ToString(searchEmployeeModel.DepartmentID)
                });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchEmployeeModel.IsDeleted) });

            if (searchEmployeeModel.RoleID > 0)
                searchList.Add(new SearchValueData { Name = "RoleID", Value = Convert.ToString(searchEmployeeModel.RoleID) });

            if (searchEmployeeModel.DesignationID > 0)
                searchList.Add(new SearchValueData { Name = "DesignationID", Value = Convert.ToString(searchEmployeeModel.DesignationID) });

            searchList.Add(new SearchValueData
            {
                Name = "IsDepartmentSupervisor",
                Value = Convert.ToString(searchEmployeeModel.IsSupervisor)
            });

            searchList.Add(new SearchValueData
            {
                Name = "RecordAccess",
                Value = Common.HasPermission(Constants.AllRecordAccess) ? "-1" : Common.HasPermission(Constants.SameGroupAndLimitedEmployeeRecordAccess) ? "1" : "0"
            });

            searchList.Add(new SearchValueData
            {
                Name = "LoggedInUserID",
                Value = Convert.ToString(loggedInUserID)
            });

        }

        public ServiceResponse DeleteEmployee(SearchEmployeeModel searchEmployeeModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForEmployeeListPage(searchEmployeeModel, searchList, loggedInUserID);

            if (!string.IsNullOrEmpty(searchEmployeeModel.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchEmployeeModel.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });
            searchList.Add(new SearchValueData { Name = "CurrentDateTime", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat) });

            List<ListEmployeeModel> totalData = GetEntityList<ListEmployeeModel>(StoredProcedure.DeleteEmployee, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            //if (count == 0 && totalData != null && totalData.Count > 0)
            //{
            //    response.Message = Common.MessageWithTitle(string.Format(Resource.DeleteFailed, Resource.Employee), Resource.EmployeeReferralExistMessage);
            //    return response;
            //}

            Page<ListEmployeeModel> getEmployeeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getEmployeeList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Employee);
            return response;
        }

        #endregion Employee List

        #region EmployeeNotes
        public ServiceResponse HC_SaveEmployeeNotes(long EmployeeId, string noteDetail, long loggedInUserID, long CommonNoteID, bool IsEdit = false)
        {
            ServiceResponse response = new ServiceResponse();
            if (EmployeeId > 0)
            {
                var searchlist = new List<SearchValueData>
                {

                    new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(EmployeeId)},
                    new SearchValueData {Name = "NoteDetail", Value = noteDetail},
                    new SearchValueData {Name = "LoggedInUserID", Value = Convert.ToString(loggedInUserID)},
                    new SearchValueData {Name = "IsEdit", Value = Convert.ToString(IsEdit)},
                };

                if (CommonNoteID > 0)
                {
                    searchlist.Add(new SearchValueData { Name = "CommonNoteID", Value = Convert.ToString(CommonNoteID) });
                }

                GetScalar(StoredProcedure.SaveCommonNote, searchlist);
                response.IsSuccess = true;
                response.Message = Resource.NoteSavedSuccessfully;
            }
            else
                response.Message = Resource.NoteFailed;
            return response;
        }

        public ServiceResponse UpdateBulkGroup(string empids, string groupids)
        {
            ServiceResponse response = new ServiceResponse();
            if (empids.Length > 0)
            {
                var searchlist = new List<SearchValueData>
                {

                    new SearchValueData {Name = "Input", Value = Convert.ToString(empids)},
                    new SearchValueData {Name = "Character", Value = Convert.ToString(",")},
                    new SearchValueData {Name = "GroupIds", Value = Convert.ToString(groupids)},

                };
                GetScalar(StoredProcedure.BulkGroupUpdate, searchlist);
                response.IsSuccess = true;
                response.Message = "Employee group(s) updated successfully";
            }
            else
                response.Message = Resource.NoteFailed;
            return response;
        }




        public ServiceResponse GetEmployeeNotes(long EmployeeId)
        {
            ServiceResponse response = new ServiceResponse();
            if (EmployeeId > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(EmployeeId)},
                };

                List<EmployeeNotesModel> totalData = GetEntityList<EmployeeNotesModel>(StoredProcedure.GetCommonNotes, searchlist);


                response.IsSuccess = true;
                response.Data = totalData;
            }
            else
                response.Message = Resource.ExceptionMessage;
            return response;
        }



        public ServiceResponse GetEmployeeGroup(string name)
        {
            ServiceResponse response = new ServiceResponse();
            if (name.Length > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "Name", Value = Convert.ToString(name)},
                };

                List<CommonList> totalData = GetEntityList<CommonList>(StoredProcedure.GetCommonList, searchlist);


                response.IsSuccess = true;
                response.Data = totalData;
            }
            else
                response.Message = Resource.ExceptionMessage;
            return response;
        }

        public ServiceResponse GetEmployeeEmailSignature(string employeeid)
        {
            ServiceResponse response = new ServiceResponse();
            if (Convert.ToInt64(employeeid) > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(employeeid)},
                };

                EmailSignature totalData = GetEntity<EmailSignature>(StoredProcedure.GetEmployeeEmailSignature, searchlist);


                response.IsSuccess = true;
                response.Data = totalData;
            }

            return response;
        }


        public ServiceResponse SaveEmployeeEmailSignature(string EmployeeID, string Name, string Description, string UpdatedBy)
        {
            ServiceResponse response = new ServiceResponse();
            if (Name.Length > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "Name", Value = Convert.ToString(Name)},
                    new SearchValueData {Name = "Description", Value = Convert.ToString(Description)},
                    new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(EmployeeID)},
                    new SearchValueData {Name = "UpdatedBy", Value = Convert.ToString(UpdatedBy)},
                };

                GetScalar(StoredProcedure.SaveEmployeeEmailSignature, searchlist);
                response.IsSuccess = true;
            }

            if (!response.IsSuccess)
                response.Message = Resource.ExceptionMessage;

            return response;
        }

        public ServiceResponse DeleteEmployeeNote(long CommonNoteID, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            if (CommonNoteID > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "CommonNoteID", Value = Convert.ToString(CommonNoteID)},
                    new SearchValueData {Name = "LoggedInUserID", Value = Convert.ToString(loggedInUserID)},
                };


                GetScalar(StoredProcedure.DeleteCommonNote, searchlist);
                response.IsSuccess = true;
            }

            if (!response.IsSuccess)
                response.Message = Resource.ExceptionMessage;

            return response;
        }

        #endregion EmployeeNotes

        #region Employee Checklist

        public ServiceResponse HC_GetEmployeeChecklist(string employeeId)
        {
            var serviceResponse = new ServiceResponse();
            EmployeeChecklist model = new EmployeeChecklist();
            List<SearchValueData> paramList = new List<SearchValueData>
                        {
                            new SearchValueData { Name = "EmployeeID" , Value = employeeId.ToString()},
                        };

            model = GetEntity<EmployeeChecklist>(paramList);
            model = model ?? new EmployeeChecklist();
            if (!model.SubmittedDate.HasValue)
                model.SubmittedDate = DateTime.Today;
            serviceResponse.Data = model;
            serviceResponse.IsSuccess = true;
            return serviceResponse;
        }

        public ServiceResponse HC_SaveEmployeeChecklist(EmployeeChecklist model, long loggedInUserID)
        {
            var serviceResponse = new ServiceResponse();

            SaveObject(model, loggedInUserID);

            serviceResponse.Data = model;
            serviceResponse.Message = model.EmployeeChecklistID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.EmployeeChecklist) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.EmployeeChecklist);
            serviceResponse.IsSuccess = true;
            return serviceResponse;
        }

        #endregion Employee Checklist

        #region Employee Notification Prefs

        public ServiceResponse HC_GetEmployeeNotificationPrefs(string employeeId)
        {
            var serviceResponse = new ServiceResponse();
            EmployeeNotificationPrefsModel model = new EmployeeNotificationPrefsModel();
            List<SearchValueData> paramList = new List<SearchValueData>
            {
                new SearchValueData { Name = "EmployeeID" , Value = employeeId.ToString()},
            };

            model = GetEntity<EmployeeNotificationPrefsModel>(paramList);
            model = model ?? new EmployeeNotificationPrefsModel();
            serviceResponse.Data = model;
            serviceResponse.IsSuccess = true;
            return serviceResponse;
        }

        public ServiceResponse HC_SaveEmployeeNotificationPrefs(EmployeeNotificationPrefsModel model, long loggedInUserID)
        {
            var serviceResponse = new ServiceResponse();

            SaveObject(model, loggedInUserID);

            serviceResponse.Data = model;
            serviceResponse.Message = model.EmployeeNotificationPreferenceID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.EmployeeNotificationPreference) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.EmployeeNotificationPreference);
            serviceResponse.IsSuccess = true;
            return serviceResponse;
        }

        #endregion Employee Notification Prefs

        #region In HOME CARE Data Provider Code
        public ServiceResponse HC_SetAddEmployeePage(long employeeId)
        {
            var response = new ServiceResponse();
            var isEditMode = employeeId > 0;
            CacheHelper_MyezCare ch_MyezcareOrg = new CacheHelper_MyezCare();
            MyEzcareOrganization myezCareOrg = ch_MyezcareOrg.GetCachedData<MyEzcareOrganization>();
            var OrgTypes = myezCareOrg.OrganizationType.Split(',');


            List<SearchValueData> searchParam = new List<SearchValueData>();
            searchParam.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(employeeId) });
            searchParam.Add(new SearchValueData { Name = "PreferenceType_Preference", Value = Convert.ToString(Preference.PreferenceKeyType.Preference) });
            searchParam.Add(new SearchValueData { Name = "PreferenceType_Skill", Value = Convert.ToString(Preference.PreferenceKeyType.Skill) });
            searchParam.Add(new SearchValueData { Name = "DDType_CareType", Value = Convert.ToString((int)Common.DDType.CareType) });
            searchParam.Add(new SearchValueData { Name = "DDType_Designation", Value = Convert.ToString((int)Common.DDType.Designation) });
            searchParam.Add(new SearchValueData { Name = "DDType_LanguagePreference", Value = Convert.ToString((int)Common.DDType.LanguagePreference) });
            searchParam.Add(new SearchValueData { Name = "DDType_Gender", Value = Convert.ToString((int)Common.DDType.Gender) });

            var addEmployeeModel = GetMultipleEntity<HC_AddEmployeeModel>(StoredProcedure.HC_GetEmployeeDetails, searchParam);

            addEmployeeModel.OrgTypeList = Common.SetOrgTypeList(OrgTypes);

            if (addEmployeeModel.Employee == null)
            {
                addEmployeeModel.Employee = new Employee();
                addEmployeeModel.Employee.IsActive = true;
            }

            if (OrgTypes.Length == 1)
            {
                addEmployeeModel.Employee.AssociateWith = addEmployeeModel.OrgTypeList[0].Value;
            }

            if (addEmployeeModel.PreferenceList == null || addEmployeeModel.PreferenceList.Count == 0)
                addEmployeeModel.PreferenceList = new List<EmployeePreferenceModel>();
            if (addEmployeeModel.SkillList == null || addEmployeeModel.SkillList.Count == 0)
                addEmployeeModel.SkillList = new List<Preference>();
            if (addEmployeeModel.EmployeeSkillList == null || addEmployeeModel.EmployeeSkillList.Count == 0)
                addEmployeeModel.EmployeeSkillList = new List<string>();

            // addEmployeeModel.EmployeeDesignation = Common.SetEmployeeDesignationTypeList();
            if (string.IsNullOrWhiteSpace(addEmployeeModel.Employee.Designation))
                addEmployeeModel.Employee.Designation = string.Empty;
            try
            {
                if (isEditMode)
                {

                    if (addEmployeeModel.Employee != null && addEmployeeModel.Employee.EmployeeID > 0)
                    {
                        addEmployeeModel.Employee.SocialSecurityNumber = !string.IsNullOrEmpty(addEmployeeModel.Employee.SocialSecurityNumber) ? Crypto.Decrypt(addEmployeeModel.Employee.SocialSecurityNumber) : "";

                        if (addEmployeeModel.Employee.EmployeeSignatureID > 0)
                        {
                            EmployeeSignature employeeSignature =
                                GetEntity<EmployeeSignature>(addEmployeeModel.Employee.EmployeeSignatureID);
                            if (employeeSignature != null)
                                addEmployeeModel.Employee.TempSignaturePath = employeeSignature.SignaturePath;
                        }

                        var path = ConfigSettings.AmazoneUploadPath + ConfigSettings.TempFiles + addEmployeeModel.Employee.EmployeeID + "/";
                        addEmployeeModel.AmazonSettingModel = AmazonFileUpload.GetAmazonModelForClientSideUpload(SessionHelper.LoggedInID, path, ConfigSettings.PublicAcl);

                        addEmployeeModel.IsEditMode = true;
                        response.IsSuccess = true;
                    }
                    else
                    {
                        addEmployeeModel.Employee = new Employee { IsActive = true };
                        response.ErrorCode = Constants.ErrorCode_NotFound;
                        return response;
                    }

                }

                //searchParam.Add(new SearchValueData { Name = "IsDeleted", Value = "0" });
                //var addEmployeeModel = new HC_AddEmployeeModel
                //{
                //    SecurityQuestionList = GetEntityList<SecurityQuestion>(),
                //    RoleList = GetEntityList<Role>(null, "", "RoleName", "ASC"),
                //    CredentialList = GetEntityList<EmployeeCredential>(null, "", "CredentialID", "ASC"),
                //    Employee = { IsActive = true },
                //    StateList = GetEntityList<State>()
                //};
                //if (employeeID == 0)
                //{
                //    addEmployeeModel.DepartmentList = GetEntityList<Department>(searchParam, "", "DepartmentName", "ASC");
                //    addEmployeeModel.PreferenceList = new List<EmployeePreferenceModel>();
                //}
                //if (employeeID > 0)
                //{
                //    List<SearchValueData> searchEmployee = new List<SearchValueData>
                //        {
                //            new SearchValueData { Name = "EmployeeID" , Value = employeeID.ToString()},
                //            //new SearchValueData { Name = "IsDeleted" , Value = "0"},
                //        };

                //    addEmployeeModel.Employee = GetEntity<Employee>(StoredProcedure.GetEmployeeDetails,searchEmployee);
                //    addEmployeeModel.PreferenceList = GetEntityList<EmployeePreferenceModel>("GetEmployeePreference", searchEmployee);




                //    if (addEmployeeModel.Employee.EmployeeSignatureID > 0)
                //    {
                //        EmployeeSignature employeeSignature =
                //            GetEntity<EmployeeSignature>(addEmployeeModel.Employee.EmployeeSignatureID);
                //        if (employeeSignature != null)
                //        {
                //            addEmployeeModel.Employee.TempSignaturePath = employeeSignature.SignaturePath;
                //        }
                //    }
                //    string custWhere;
                //    if (addEmployeeModel.Employee.DepartmentID > 0)
                //    {
                //        custWhere = string.Format("IsDeleted = {0} OR DepartmentID={1}", 0, addEmployeeModel.Employee.DepartmentID);
                //    }
                //    else
                //    {
                //        custWhere = string.Format("IsDeleted = {0}", 0);
                //    }
                //    addEmployeeModel.DepartmentList = GetEntityList<Department>(null, custWhere, "DepartmentName", "ASC");
                //    if (addEmployeeModel.Employee != null && addEmployeeModel.Employee.EmployeeID > 0)
                //    {
                //        var path = ConfigSettings.AmazoneUploadPath + ConfigSettings.TempFiles + addEmployeeModel.Employee.EmployeeID + "/";
                //        addEmployeeModel.AmazonSettingModel = AmazonFileUpload.GetAmazonModelForClientSideUpload(SessionHelper.LoggedInID, path, ConfigSettings.PublicAcl);

                //        addEmployeeModel.IsEditMode = true;
                //        response.IsSuccess = true;
                //    }
                //    else
                //    {
                //        addEmployeeModel.Employee = new Employee { IsActive = true };
                //        response.ErrorCode = Constants.ErrorCode_NotFound;
                //        return response;
                //    }





                //}
                addEmployeeModel.EmergencyContactList = Common.SetYesNoListForBoolean();
                addEmployeeModel.NoticeProviderOnFileList = Common.SetYesNoListForBoolean();
                response.Data = addEmployeeModel;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }


            return response;
        }


        public ServiceResponse HC_AddEmployee(HC_AddEmployeeModel addEmployeeModel, long loggedInUserID)
        {
            CacheHelper _cacheHelper = new CacheHelper();
            var response = new ServiceResponse();
            List<EmployeePreferenceModel> PreferenceList = addEmployeeModel.PreferenceList;
            string preferenceList = string.Empty;

            //try
            //{
            if (loggedInUserID > 0)
            {
                if (addEmployeeModel != null && addEmployeeModel.Employee != null)
                {
                    addEmployeeModel.IsEditMode = addEmployeeModel.Employee.EmployeeID > 0;
                    //bool isEmailChanged = false;
                    var searchParam = new List<SearchValueData>();
                    bool isSentMail = false;
                    bool isSentSMS = false;
                    #region "Check Employee Contact Information"
                    // Check if there no primary contact and legal contact then show alert to user that referral cannot be created.
                    //var EmployeeContactresult = addEmployeeModel.ContactInformationList.FirstOrDefault(q => q.ContactTypeID == (int)Common.ContactTypes.EmployeePrimaryPlacement);
                    ////if (result != null)
                    ////result = addReferralModel.ContactInformationList.FirstOrDefault(q => q.ContactTypeID != (int)Common.ContactTypes.LegalGuardian);

                    //if (EmployeeContactresult == null)
                    //{
                    //    response.Message = Resource.EmployeeContactRequired;
                    //    return response;
                    //}

                    #endregion

                    #region Check UserName Exists
                    if (string.IsNullOrEmpty(addEmployeeModel.Employee.UserName))
                    {
                        response.Message = Resource.UsernameRequired;
                        return response;
                    }
                    if (addEmployeeModel.Employee.RoleID == 0)
                    {
                        response.Message = Resource.RoleIDRequired;
                        return response;
                    }
                    // This condition will check for the User Name Unique
                    if (!string.IsNullOrEmpty(addEmployeeModel.Employee.UserName))
                    {
                        searchParam.Clear();
                        searchParam.Add(new SearchValueData { Name = "UserName", Value = addEmployeeModel.Employee.UserName, IsEqual = true });
                        //searchParam.Add(new SearchValueData { Name = "IsDeleted", Value = "1", IsEqual = true });
                        if (addEmployeeModel.IsEditMode)
                            searchParam.Add(new SearchValueData
                            {
                                Name = "EmployeeID",
                                Value = addEmployeeModel.Employee.EmployeeID.ToString(),
                                IsNotEqual = true
                            });

                        var employee = GetEntity<Employee>(searchParam);
                        if (employee != null && employee.EmployeeID > 0)
                        {
                            response.Message = Resource.UserNameAlreadyExists;
                            return response;
                        }
                        else if (employee == null && !string.IsNullOrEmpty(addEmployeeModel.Employee.UserName))
                        {
                            searchParam.Clear();
                            searchParam.Add(new SearchValueData { Name = "UserName", Value = addEmployeeModel.Employee.UserName, IsEqual = true });
                            var referral = GetEntity<Referral>(searchParam);
                            if (referral != null && referral.ReferralID > 0)
                            {
                                response.Message = Resource.UserNameAlreadyExists;
                                return response;
                            }
                        }


                    }

                    #endregion

                    #region Check Email Address Exists

                    // This condition will check for the Email Unique
                    //COMMENTED FOR WAHIBA
                    //if (!string.IsNullOrEmpty(addEmployeeModel.Employee.Email))
                    //{
                    //    searchParam.Clear();
                    //    searchParam.Add(new SearchValueData { Name = "Email", Value = addEmployeeModel.Employee.Email, IsEqual = true });
                    //    if (addEmployeeModel.IsEditMode)
                    //        searchParam.Add(new SearchValueData
                    //        {
                    //            Name = "EmployeeID",
                    //            Value = addEmployeeModel.Employee.EmployeeID.ToString(),
                    //            IsNotEqual = true
                    //        });

                    //    var employee = GetEntity<Employee>(searchParam);
                    //    if (employee != null && employee.EmployeeID > 0)
                    //    {
                    //        response.Message = Resource.EmailAddressAlreadyExists;
                    //        return response;
                    //    }
                    //}

                    #endregion

                    #region Check Signature Exist

                    //if (string.IsNullOrEmpty(addEmployeeModel.Employee.EmpSignature))
                    //{
                    //    response.Message = Resource.EmpSignatureRequired;
                    //    return response;
                    //}
                    #endregion

                    #region Check Employee Unique ID Exists
                    searchParam.Clear();
                    searchParam.Add(new SearchValueData { Name = "EmployeeUniqueID", Value = addEmployeeModel.Employee.EmployeeUniqueID, IsEqual = true });
                    searchParam.Add(new SearchValueData { Name = "EmployeeID", Value = addEmployeeModel.Employee.EmployeeID.ToString(), IsNotEqual = true });
                    //searchParam.Add(new SearchValueData { Name = "IsDeleted", Value = "0", IsEqual = true });

                    var EmployeeUniqueID = GetScalar(StoredProcedure.CheckEmployeeUniqueID, searchParam);
                    if (EmployeeUniqueID != null)
                    {
                        response.Message = Resource.EmployeeIDAlreadyExists;
                        return response;
                    }
                    #endregion

                    #region Check Employee Mobile Number Exists

                    if (!string.IsNullOrEmpty(addEmployeeModel.Employee.MobileNumber))
                    {
                        searchParam.Clear();
                        searchParam.Add(new SearchValueData { Name = "MobileNumber", Value = addEmployeeModel.Employee.MobileNumber, IsEqual = true });
                        searchParam.Add(new SearchValueData { Name = "EmployeeID", Value = addEmployeeModel.Employee.EmployeeID.ToString(), IsNotEqual = true });
                        //searchParam.Add(new SearchValueData { Name = "IsDeleted", Value = "0", IsEqual = true });

                        var emp = GetEntity<Employee>(searchParam);
                        if (emp != null)
                        {
                            response.Message = Resource.MobileAlreadyExists;
                            return response;
                        }
                    }

                    #endregion

                    #region Check NPI Nuber Exists

                    if (!string.IsNullOrEmpty(addEmployeeModel.Employee.HHA_NPI_ID))
                    {
                        searchParam.Clear();
                        searchParam.Add(new SearchValueData { Name = "HHA_NPI_ID", Value = addEmployeeModel.Employee.HHA_NPI_ID, IsEqual = true });

                        if (addEmployeeModel.IsEditMode)
                            searchParam.Add(new SearchValueData
                            {
                                Name = "EmployeeID",
                                Value = addEmployeeModel.Employee.EmployeeID.ToString(),
                                IsNotEqual = true
                            });

                        var employee = GetEntity<Employee>(searchParam);
                        if (employee != null && employee.EmployeeID > 0)
                        {
                            response.Message = Resource.NPIExists;
                            return response;
                        }
                    }

                    #endregion

                    var address = addEmployeeModel.Employee.Address + ", " + addEmployeeModel.Employee.City + ", " + addEmployeeModel.Employee.StateCode;
                    // This block will execute when the user is in the Edit Mode
                    if (addEmployeeModel.IsEditMode)
                    {
                        //PasswordDetail passwordDetail = Common.CreatePassword(addEmployeeModel.Employee.Password);
                        //var editEmployee = GetEntity<Employee>(addEmployeeModel.Employee.EmployeeID);
                        List<SearchValueData> searchEmployee = new List<SearchValueData>
                            {
                                new SearchValueData { Name = "EmployeeID" , Value = addEmployeeModel.Employee.EmployeeID.ToString()}
                            };
                        var editEmployee = GetEntity<Employee>(StoredProcedure.GetEmployeeDetails, searchEmployee);
                        var tempEmployee = GetEntity<Employee>(StoredProcedure.GetEmployeeDetails, searchEmployee);

                        //Check for the existing password is changed or not.
                        if (!string.IsNullOrEmpty(addEmployeeModel.Employee.NewPassword))
                        {
                            PasswordDetail passwordDetail = Common.CreatePassword(addEmployeeModel.Employee.NewPassword);
                            editEmployee.Password = passwordDetail.Password;
                            editEmployee.PasswordSalt = passwordDetail.PasswordSalt;
                        }

                        if (editEmployee != null && editEmployee.EmployeeID > 0)
                        {
                            #region SIGN
                            if (addEmployeeModel.Employee.EmployeeSignatureID > 0)
                            {
                                EmployeeSignature employeeSignature =
                                    GetEntity<EmployeeSignature>(addEmployeeModel.Employee.EmployeeSignatureID);
                                if (employeeSignature != null)
                                {
                                    addEmployeeModel.TempSignaturePath = employeeSignature.SignaturePath;
                                }
                            }
                            if (addEmployeeModel.TempSignaturePath != addEmployeeModel.Employee.TempSignaturePath)
                            {
                                //AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                                //string bucket = ConfigSettings.ZarephathBucket;
                                //string destPath = ConfigSettings.AmazoneUploadPath + ConfigSettings.EmpSignatures +
                                //                  editEmployee.EmployeeID +
                                //                  addEmployeeModel.Employee.TempSignaturePath.Substring(
                                //                      addEmployeeModel.Employee.TempSignaturePath.LastIndexOf('/'));

                                //amazonFileUpload.MoveFile(bucket, addEmployeeModel.Employee.TempSignaturePath, bucket, destPath, S3CannedACL.PublicRead);


                                //string destPath = ConfigSettings.AmazoneUploadPath + ConfigSettings.EmpSignatures +
                                //                  editEmployee.EmployeeID +
                                //                  addEmployeeModel.Employee.TempSignaturePath.Substring(
                                //                      addEmployeeModel.Employee.TempSignaturePath.LastIndexOf('/'));

                                string destPath = String.Format(_cacheHelper.UploadPath, _cacheHelper.Domain) + ConfigSettings.EmpSignatures +
                                                  editEmployee.EmployeeID + "/";

                                ServiceResponse fileResponse = Common.MoveFile(addEmployeeModel.Employee.TempSignaturePath, destPath);


                                if (fileResponse.IsSuccess)
                                {
                                    EmployeeSignature employeeSignature = new EmployeeSignature
                                    {
                                        EmployeeID = loggedInUserID,
                                        SignaturePath = fileResponse.Data.ToString()
                                    };
                                    SaveEntity(employeeSignature);
                                    editEmployee.EmployeeSignatureID = employeeSignature.EmployeeSignatureID;
                                }
                            }

                            #endregion SIGN
                            //isEmailChanged = addEmployeeModel.Employee.Email != editEmployee.Email;
                            editEmployee.EmployeeUniqueID = addEmployeeModel.Employee.EmployeeUniqueID;
                            editEmployee.FirstName = addEmployeeModel.Employee.FirstName;
                            editEmployee.MiddleName = addEmployeeModel.Employee.MiddleName;
                            editEmployee.LastName = addEmployeeModel.Employee.LastName;
                            editEmployee.Email = addEmployeeModel.Employee.Email;
                            editEmployee.SocialSecurityNumber = !string.IsNullOrEmpty(addEmployeeModel.Employee.SocialSecurityNumber) ? Crypto.Encrypt(addEmployeeModel.Employee.SocialSecurityNumber) : "";
                            editEmployee.PhoneWork = addEmployeeModel.Employee.PhoneWork;
                            editEmployee.PhoneHome = addEmployeeModel.Employee.PhoneHome;
                            editEmployee.MobileNumber = addEmployeeModel.Employee.MobileNumber;
                            editEmployee.ApartmentNo = addEmployeeModel.Employee.ApartmentNo;
                            editEmployee.Address = addEmployeeModel.Employee.Address;
                            editEmployee.City = addEmployeeModel.Employee.City;
                            editEmployee.ZipCode = addEmployeeModel.Employee.ZipCode;
                            editEmployee.StateCode = addEmployeeModel.Employee.StateCode;
                            editEmployee.IsDepartmentSupervisor = addEmployeeModel.Employee.IsDepartmentSupervisor;
                            editEmployee.UserName = addEmployeeModel.Employee.UserName;
                            editEmployee.IsVerify = true;
                            // editEmployee.IsVerify = addEmployeeModel.Employee.IsVerify;
                            editEmployee.EmpSignature = addEmployeeModel.Employee.EmpSignature;

                            if (editEmployee.IsActive != addEmployeeModel.Employee.IsActive && addEmployeeModel.Employee.IsActive)
                            {
                                editEmployee.LoginFailedCount = 0;
                            }
                            editEmployee.IsActive = addEmployeeModel.Employee.IsActive;
                            editEmployee.RoleID = addEmployeeModel.Employee.RoleID;
                            editEmployee.DepartmentID = addEmployeeModel.Employee.DepartmentID;
                            editEmployee.CredentialID = addEmployeeModel.Employee.CredentialID;
                            editEmployee.Degree = addEmployeeModel.Employee.Degree;
                            editEmployee.Latitude = addEmployeeModel.Employee.Latitude;
                            editEmployee.Longitude = addEmployeeModel.Employee.Longitude;
                            editEmployee.IVRPin = addEmployeeModel.Employee.IVRPin;
                            editEmployee.HHA_NPI_ID = addEmployeeModel.Employee.HHA_NPI_ID;
                            editEmployee.Designation = addEmployeeModel.Employee.Designation;
                            editEmployee.CareTypeIds = addEmployeeModel.Employee.CareTypeIds;
                            editEmployee.AssociateWith = addEmployeeModel.Employee.AssociateWith;
                            editEmployee.DateOfBirth = addEmployeeModel.Employee.DateOfBirth;
                            editEmployee.FacilityID = addEmployeeModel.Employee.FacilityID;
                            editEmployee.HireDate = addEmployeeModel.Employee.HireDate;
                            editEmployee.DateOfBirth = addEmployeeModel.Employee.DateOfBirth;
                            editEmployee.GroupIDs = addEmployeeModel.Employee.GroupIDs;
                            editEmployee.EmpGender = addEmployeeModel.Employee.EmpGender;
                            editEmployee.StateRegistrationID = addEmployeeModel.Employee.StateRegistrationID;
                            editEmployee.ProfessionalLicenseNumber = addEmployeeModel.Employee.ProfessionalLicenseNumber;
                            editEmployee.CanUpdateCoordinate = addEmployeeModel.Employee.CanUpdateCoordinate;
                            SaveObject(editEmployee, loggedInUserID);

                            Common.CreateAuditTrail(addEmployeeModel.IsEditMode ? AuditActionType.Update : AuditActionType.Create, addEmployeeModel.Employee.EmployeeID, addEmployeeModel.Employee.EmployeeID,
                                                             tempEmployee, addEmployeeModel.Employee, loggedInUserID);
                        }
                    }
                    else
                    {
                        // This block will save the new entry of employee in the database
                        addEmployeeModel.Employee.IsActive = addEmployeeModel.Employee.IsActive;
                        addEmployeeModel.Employee.IsVerify = true;
                        addEmployeeModel.Employee.IsActive = true;
                        addEmployeeModel.Employee.SocialSecurityNumber = !string.IsNullOrEmpty(addEmployeeModel.Employee.SocialSecurityNumber) ? Crypto.Encrypt(addEmployeeModel.Employee.SocialSecurityNumber) : "";
                        //PasswordDetail passwordDetail = Common.CreatePassword(addEmployeeModel.Employee.Password);
                        //addEmployeeModel.Employee.Password = passwordDetail.Password;
                        //addEmployeeModel.Employee.PasswordSalt = passwordDetail.PasswordSalt;
                        SaveObject(addEmployeeModel.Employee, loggedInUserID);

                        string destPath = String.Format(_cacheHelper.UploadPath, _cacheHelper.Domain) + ConfigSettings.EmpSignatures +
                                                  addEmployeeModel.Employee.EmployeeID + "/";

                        ServiceResponse fileResponse = Common.MoveFile(addEmployeeModel.Employee.TempSignaturePath, destPath);


                        if (fileResponse.IsSuccess)
                        {
                            EmployeeSignature employeeSignature = new EmployeeSignature
                            {
                                EmployeeID = loggedInUserID,
                                SignaturePath = fileResponse.Data.ToString()
                            };
                            SaveEntity(employeeSignature);
                            addEmployeeModel.Employee.EmployeeSignatureID = employeeSignature.EmployeeSignatureID;
                        }
                        SaveObject(addEmployeeModel.Employee, loggedInUserID);

                        //if (!addEmployeeModel.Employee.IsActive && addEmployeeModel.IsEditMode)
                        //    UpdateMobileCache(addEmployeeModel.Employee.EmployeeID);

                        #region Mail Chimp Sync
                        var mailChimpHelper = new MailChimpHelper();
                        CacheHelper_MyezCare ch_MyezcareOrg = new CacheHelper_MyezCare();
                        MyEzcareOrganization myezCareOrg = ch_MyezcareOrg.GetCachedData<MyEzcareOrganization>();
                        mailChimpHelper.AddEmployeeToMailChimp(addEmployeeModel, myezCareOrg.CompanyName, myezCareOrg.DomainName);
                        #endregion

                        isSentMail = SendSetPasswordMail(addEmployeeModel.Employee);

                        if (!string.IsNullOrEmpty(addEmployeeModel.Employee.MobileNumber))
                            isSentSMS = SendSetPasswordSMS(addEmployeeModel.Employee);
                    }


                    #region Add Employee Preference

                    foreach (EmployeePreferenceModel item in PreferenceList.Where(c => c.EmployeePreferenceID == 0))
                    {
                        preferenceList +=
                            Convert.ToString(item.PreferenceID) + Constants.PipeChar + Convert.ToString(item.PreferenceName) + Constants.Comma;
                    }
                    if (preferenceList.Length > 0)
                        preferenceList = preferenceList.Remove(preferenceList.LastIndexOf(Constants.CommaChar));

                    var searchListValueData = new List<SearchValueData>
                                {
                                    new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(addEmployeeModel.Employee.EmployeeID)},
                                    new SearchValueData { Name = "Preferences", Value = preferenceList},
                                    new SearchValueData { Name = "Skills", Value = addEmployeeModel.StrEmployeeSkillList},
                                    new SearchValueData { Name = "PreferenceType_Preference", Value = Convert.ToString(Preference.PreferenceKeyType.Preference) },
                                    new SearchValueData { Name = "PreferenceType_Skill", Value = Convert.ToString(Preference.PreferenceKeyType.Skill) },
                                    new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(loggedInUserID) },
                                    new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() },

                                };
                    TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.SaveEmployeePreferences, searchListValueData);
                    response.IsSuccess = true;
                    response.Message = !addEmployeeModel.IsEditMode
                                           ? isSentMail ? Resource.EmployeeCreatedSuccessfullyMailSend : Resource.EmployeeCreatedSuccessfullyMailFailed
                                           : string.Format(Resource.RecordUpdatedSuccessfully, Resource.Employee);


                    #endregion

                    //                    EmployeelistURL

                    #region Add/Update Employee/Client Contact Information
                    // Save Contact Info in the database

                    if (addEmployeeModel.ContactInformationList.Any())
                    {
                        foreach (var addAndListContactInformation in addEmployeeModel.ContactInformationList.OrderBy(c => c.ContactTypeID))
                        {
                            addAndListContactInformation.EmployeeID = addEmployeeModel.Employee.EmployeeID;
                            addAndListContactInformation.ClientID = 0;
                            //addReferralModel.AddAndListContactInformation = addAndListContactInformation;
                            // This method will save the entry in the Contact and Contact Mapping table.
                            SaveContact(addAndListContactInformation, loggedInUserID);
                        }
                    }

                    // Save the payor info in the table ReferralPayorMapping
                    #endregion

                    response.Data = Crypto.Encrypt(Convert.ToString(addEmployeeModel.Employee.EmployeeID));
                }
                else
                    response.Message = Common.MessageWithTitle(string.Format(Resource.CreateFailed, Resource.Employee), Resource.RecordNotFound);
            }
            else
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.UserSessionExpired);

            //}
            //catch (Exception)
            //{
            //    response.IsSuccess = false;
            //    response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            //}

            return response;
        }
        public ServiceResponse AddEmployeeContact(HC_AddEmployeeModel addEmployeeModel, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            if (loggedInUserID == 0)
            {
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.UserSessionExpired);
                return response;
            }

            // This method will save the entry in the Contact and Contact Mapping Table
            SaveContact(addEmployeeModel.AddAndListContactInformation, loggedInUserID);

            List<SearchValueData> searchList = new List<SearchValueData>
            {
                new SearchValueData {Name = "EmployeeID", Value = addEmployeeModel.AddAndListContactInformation.EmployeeID.ToString()}
            };

            ContactInformationModal contactInformationModal = new ContactInformationModal
            {
                AddAndListContactInformation =
                        GetEntityList<AddAndListContactInformation>("GetEmployeeContactInformation", searchList),
                Contact = addEmployeeModel.Contact
            };
            response.IsSuccess = true;
            response.Data = contactInformationModal;
            return response;
        }

        public ServiceResponse DeteteEmployeeContact(long contactMappingID, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();


            if (loggedInUserID == 0)
            {
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.UserSessionExpired);
                return response;
            }

            if (contactMappingID > 0)
            {
                EmployeeContactMappings temp = GetEntity<EmployeeContactMappings>(contactMappingID);
                DeleteEntity<EmployeeContactMappings>(contactMappingID);
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.Contact);
                Common.CreateAuditTrail(AuditActionType.Delete, temp.EmployeeID, temp.ContactMappingID,
                                                 temp, new EmployeeContactMappings(), loggedInUserID);

            }
            return response;
        }
        private void SaveContact(AddAndListContactInformation model, long loggedInUserID)
        {
            if (model != null &&
                model.EmployeeID > 0)
            {
                switch (model.ContactTypeID)
                {
                    case (int)Common.ContactTypes.PrimaryPlacement:
                        model.IsDCSLegalGuardian = false;
                        model.IsNoticeProviderOnFile = false;
                        break;
                    case (int)Common.ContactTypes.LegalGuardian:
                        model.IsPrimaryPlacementLegalGuardian = false;
                        break;
                    default:
                        model.IsDCSLegalGuardian = false;
                        model.IsNoticeProviderOnFile = false;
                        model.IsPrimaryPlacementLegalGuardian = false;
                        break;
                }
                var searchParam = new List<SearchValueData>();
                Contact contact = new Contact();
                Contact tempContact = new Contact();

                bool isPrimaryPlacementAndLegalSame = false;
                bool isLegalGuardianNeedUpdate = true;

                long oldLegalGuardianContactId = 0;
                if (model.ContactTypeID == (int)Common.ContactTypes.LegalGuardian)
                {
                    oldLegalGuardianContactId = model.ContactID;
                    searchParam.Add(new SearchValueData { Name = "ContactTypeID", Value = ((int)Common.ContactTypes.PrimaryPlacement).ToString() });
                    searchParam.Add(new SearchValueData { Name = "EmployeeID", Value = model.EmployeeID.ToString() });
                    var contactMapping = GetEntity<EmployeeContactMappings>(searchParam) ?? new EmployeeContactMappings();
                    if (contactMapping.IsPrimaryPlacementLegalGuardian)
                    {
                        model.ContactID = contactMapping.ContactID;
                        model.IsDCSLegalGuardian = false;
                        model.IsNoticeProviderOnFile = false;
                        isLegalGuardianNeedUpdate = false;
                    }
                    else if (model.ContactID == contactMapping.ContactID)
                    {
                        isPrimaryPlacementAndLegalSame = true;
                        //add new contact id because here  
                        //model.ContactID = 0;
                    }
                }
                //else
                //{

                if (((model.MasterContactUpdated && model.ContactID > 0) || model.ContactID > 0) && isPrimaryPlacementAndLegalSame == false)
                {
                    searchParam.Clear();
                    searchParam.Add(new SearchValueData { Name = "ContactID", Value = model.ContactID.ToString() });
                    contact = GetEntity<Contact>(searchParam) ?? new Contact();
                }
                if (isLegalGuardianNeedUpdate)
                {
                    tempContact = JsonConvert.DeserializeObject<Contact>(JsonConvert.SerializeObject(contact));
                    tempContact.ContactType = GetContactTypeName(Convert.ToInt16(model.ContactTypeID));
                    if (contact.ContactID > 0 && !model.AddNewContactDetails)
                    {
                        contact.FirstName = model.FirstName;
                        contact.LastName = model.LastName;
                        contact.Email = model.Email;
                        contact.ApartmentNo = model.ApartmentNo;
                        contact.Address = model.Address;
                        contact.City = model.City;
                        contact.State = model.State;
                        contact.ZipCode = model.ZipCode;
                        contact.Phone1 = model.Phone1;
                        contact.Phone2 = model.Phone2;
                        contact.LanguageID = model.LanguageID;
                        contact.Latitude = model.Latitude;
                        contact.Longitude = model.Longitude;
                        contact.ReferenceMasterID = model.ReferenceMasterID;
                        SaveObject(contact, loggedInUserID);
                        //contact = new Contact();
                    }
                    else
                    {
                        contact.ContactID = 0;
                        contact.FirstName = model.FirstName;
                        contact.LastName = model.LastName;
                        contact.Email = model.Email;
                        contact.ApartmentNo = model.ApartmentNo;
                        contact.Address = model.Address;
                        contact.City = model.City;
                        contact.State = model.State;
                        contact.ZipCode = model.ZipCode;
                        contact.Phone1 = model.Phone1;
                        contact.Phone2 = model.Phone2;
                        contact.Latitude = model.Latitude;
                        contact.Longitude = model.Longitude;
                        contact.LanguageID = model.LanguageID;
                        contact.ReferenceMasterID = model.ReferenceMasterID;
                        SaveObject(contact, loggedInUserID);
                    }

                    contact.ContactType = GetContactTypeName(Convert.ToInt16(model.ContactTypeID));
                    Common.CreateAuditTrail(contact.ContactID == 0 ? AuditActionType.Create : AuditActionType.Update, model.EmployeeID, contact.ContactID,
                                                 tempContact, contact, loggedInUserID);

                }
                else
                {
                    tempContact = JsonConvert.DeserializeObject<Contact>(JsonConvert.SerializeObject(contact));
                    tempContact.ContactType = GetContactTypeName(Convert.ToInt16(model.ContactTypeID));
                    var oldContact = new Contact();
                    oldContact.ContactID = oldLegalGuardianContactId;
                    oldContact.FirstName = model.FirstName;
                    oldContact.LastName = model.LastName;
                    oldContact.Email = model.Email;
                    oldContact.ApartmentNo = model.ApartmentNo;
                    oldContact.Address = model.Address;
                    oldContact.City = model.City;
                    oldContact.State = model.State;
                    oldContact.ZipCode = model.ZipCode;
                    oldContact.Phone1 = model.Phone1;
                    oldContact.Phone2 = model.Phone2;
                    oldContact.LanguageID = model.LanguageID;
                    oldContact.ContactType = GetContactTypeName(Convert.ToInt16(model.ContactTypeID));
                    oldContact.ReferenceMasterID = model.ReferenceMasterID;
                    Common.CreateAuditTrail(contact.ContactID == 0 ? AuditActionType.Create : AuditActionType.Update, model.EmployeeID, contact.ContactID,
                                                 oldContact, tempContact, loggedInUserID);
                }

                if (contact.ContactID > 0)
                {
                    searchParam.Clear();
                    EmployeeContactMappings employeeContactMappings;
                    if (model.ContactMappingID > 0)
                    {
                        searchParam.Add(new SearchValueData { Name = "ContactMappingID", Value = model.ContactMappingID.ToString() });
                        searchParam.Add(new SearchValueData { Name = "EmployeeID", Value = model.EmployeeID.ToString() });
                        employeeContactMappings = GetEntity<EmployeeContactMappings>(searchParam) ?? new EmployeeContactMappings();
                    }
                    else
                    {
                        searchParam.Add(new SearchValueData { Name = "ContactTypeID", Value = model.ContactTypeID.ToString() });
                        searchParam.Add(new SearchValueData { Name = "EmployeeID", Value = model.EmployeeID.ToString() });
                        employeeContactMappings = GetEntity<EmployeeContactMappings>(searchParam) ?? new EmployeeContactMappings();
                    }
                    EmployeeContactMappings temp = JsonConvert.DeserializeObject<EmployeeContactMappings>(JsonConvert.SerializeObject(employeeContactMappings));
                    //ContactMapping contactMapping = GetEntity<ContactMapping>(model.ContactMappingID)??new ContactMapping();
                    employeeContactMappings.ContactID = contact.ContactID;
                    // Add EmployeeID as EmployeeID in contactMapping table
                    employeeContactMappings.EmployeeID = model.EmployeeID;

                    employeeContactMappings.ClientID = model.ClientID;
                    employeeContactMappings.IsEmergencyContact = model.IsEmergencyContact;
                    employeeContactMappings.ContactTypeID = model.ContactTypeID;
                    employeeContactMappings.IsPrimaryPlacementLegalGuardian = model.IsPrimaryPlacementLegalGuardian;
                    employeeContactMappings.IsDCSLegalGuardian = model.IsDCSLegalGuardian;
                    employeeContactMappings.ROIExpireDate = model.ROIExpireDate;
                    employeeContactMappings.ROIType = model.ROIType;
                    employeeContactMappings.Relation = model.Relation;
                    employeeContactMappings.IsNoticeProviderOnFile = model.IsNoticeProviderOnFile;
                    SaveObject(employeeContactMappings, loggedInUserID);

                    Common.CreateAuditTrail(temp.ContactMappingID == 0 ? AuditActionType.Create : AuditActionType.Update, model.EmployeeID, employeeContactMappings.ContactMappingID,
                                                 temp, employeeContactMappings, loggedInUserID);




                }


            }
        }
        private string GetContactTypeName(int? contactTypeId = null)
        {
            if (contactTypeId == (int)ContactType.ContactTypes.Primary_Placement)
                return ContactType.ContactTypes.Primary_Placement.ToString();
            if (contactTypeId == (int)ContactType.ContactTypes.Legal_Guardian)
                return ContactType.ContactTypes.Legal_Guardian.ToString();
            if (contactTypeId == (int)ContactType.ContactTypes.Secondary_Placement)
                return ContactType.ContactTypes.Secondary_Placement.ToString();
            if (contactTypeId == (int)ContactType.ContactTypes.School_Teacher)
                return ContactType.ContactTypes.School_Teacher.ToString();
            if (contactTypeId == (int)ContactType.ContactTypes.Relative)
                return ContactType.ContactTypes.Relative.ToString();
            if (contactTypeId == (int)ContactType.ContactTypes.Relative2)
                return ContactType.ContactTypes.Relative2.ToString();
            return "Same As After Change";

        }
        public ServiceResponse HC_EmployeeLogin(HC_AddEmployeeModel addEmployeeModel)
        {
            CacheHelper _cacheHelper = new CacheHelper();
            var response = new ServiceResponse();
            List<EmployeePreferenceModel> PreferenceList = addEmployeeModel.PreferenceList;
            string preferenceList = string.Empty;
            if (addEmployeeModel != null && addEmployeeModel.Employee != null)
            {
                var searchParam = new List<SearchValueData>();

                #region Check UserName Exists
                // This condition will check for the User Name Unique
                if (!string.IsNullOrEmpty(addEmployeeModel.Employee.UserName))
                {
                    searchParam.Clear();
                    searchParam.Add(new SearchValueData { Name = "UserName", Value = addEmployeeModel.Employee.UserName, IsEqual = true });

                    var employee = GetEntity<Employee>(searchParam);
                    if (employee != null && employee.EmployeeID > 0)
                    {
                        response.Message = Resource.UserNameAlreadyExists;
                        return response;
                    }
                    else if (employee == null && !string.IsNullOrEmpty(addEmployeeModel.Employee.UserName))
                    {
                        searchParam.Clear();
                        searchParam.Add(new SearchValueData { Name = "UserName", Value = addEmployeeModel.Employee.UserName, IsEqual = true });
                        var referral = GetEntity<Referral>(searchParam);
                        if (referral != null && referral.ReferralID > 0)
                        {
                            response.Message = Resource.UserNameAlreadyExists;
                            return response;
                        }
                    }
                }
                #endregion

                if (!string.IsNullOrEmpty(addEmployeeModel.Employee.Password))
                {
                    PasswordDetail passwordDetail = Common.CreatePassword(addEmployeeModel.Employee.Password);
                    addEmployeeModel.Employee.Password = passwordDetail.Password;
                    addEmployeeModel.Employee.PasswordSalt = passwordDetail.PasswordSalt;
                }

                var address = addEmployeeModel.Employee.Address + ", " + addEmployeeModel.Employee.City + ", " + addEmployeeModel.Employee.StateCode;
                addEmployeeModel.Employee.IsActive = addEmployeeModel.Employee.IsActive;
                addEmployeeModel.Employee.IsVerify = true;
                addEmployeeModel.Employee.IsActive = true;
                addEmployeeModel.Employee.Address = address;
                response = SaveObject(addEmployeeModel.Employee);
                response.Data = Crypto.Encrypt(Convert.ToString(addEmployeeModel.Employee.EmployeeID));
                response.Message = Common.MessageWithTitle(string.Format(Resource.Success, Resource.Employee), Resource.EmployeeCreatedLoginToContinue);
            }
            else
                response.Message = Common.MessageWithTitle(string.Format(Resource.CreateFailed, Resource.Employee), Resource.RecordNotFound);
            return response;
        }

        public ServiceResponse HC_ResendRegistrationMail(long EmployeeID)
        {
            var response = new ServiceResponse();
            //try
            //{
            Employee emp = GetEntity<Employee>(EmployeeID);
            response.IsSuccess = SendSetPasswordMail(emp);
            if (response.IsSuccess)
            {
                response.Message = Resource.RegistrationSendSuccess;
            }
            else
            {
                response.Message = Resource.EmailSentFail;
            }
            //}
            //catch (Exception)
            //{
            //    response.IsSuccess = false;
            //    response.Message = Common.MessEmployeeIDsageWithTitle(Resource.Error, Resource.ExceptionMessage);
            //}

            return response;
        }

        public ServiceResponse HC_BulkResendRegistrationMail(string EmployeeIDs)
        {
            string[] values = EmployeeIDs.Split(',');
            int count = 0;
            var response = new ServiceResponse();
            try
            {
                foreach (var id in values)
                {
                    Employee emp = GetEntity<Employee>(Convert.ToInt64(id));
                    response.IsSuccess = SendSetPasswordMail(emp);

                    if (response.IsSuccess)
                    {
                        count = count + 1;
                    }
                }
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }

            //if (count > 0)
            //{
            response.Data = count;
            response.IsSuccess = true;
            response.Message = "Registration email(" + count + ") sent successfully";
            //}
            return response;
        }








        public ServiceResponse HC_ResendRegistrationSMS(long EmployeeID)
        {
            var response = new ServiceResponse();

            Employee emp = GetEntity<Employee>(EmployeeID);
            response.IsSuccess = SendSetPasswordSMS(emp);
            if (response.IsSuccess)
            {
                response.Message = Resource.RegistrationSMSSendSuccess;
            }
            else
            {
                response.Message = Resource.SMSSentFail;
            }

            return response;
        }

        public bool SendSetPasswordMail(Employee emp)
        {
            bool isSentMail = false;
            EmployeeTokens token = new EmployeeTokens();
            EncryptedMailMessageToken encryptedmailmessagetoken = new EncryptedMailMessageToken();

            EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                                            {
                                                new SearchValueData{Name = "EmailTemplateTypeID",Value =Convert.ToInt16(EnumEmailType.HomeCare_Schedule_Registration_Notification).ToString(),IsEqual = true}
                                            });
            token.Name = emp.FirstName + " " + emp.LastName;
            token.HomeCareLogoImage = _cacheHelper.SiteBaseURL + _cacheHelper.TemplateLogo;
            token.UserName = emp.UserName;
            token.IvrCode = emp.MobileNumber;
            token.IvrPin = emp.IVRPin;
            token.SiteName = _cacheHelper.SiteName;

            encryptedmailmessagetoken.EncryptedValue = Convert.ToInt32(emp.EmployeeID);
            encryptedmailmessagetoken.IsUsed = false;
            encryptedmailmessagetoken.ExpireDateTime = DateTime.Now.AddDays(7);
            SaveObject(encryptedmailmessagetoken);

            string Attachmentpath = HttpContext.Current.Server.MapCustomPath("~/Assets/Files/myEZcareEVVClock-inDoc.pdf");
            List<string> attachments = new List<string>();
            attachments.Add(Attachmentpath);

            var id = Crypto.Encrypt(Convert.ToString(encryptedmailmessagetoken.EncryptedMailID));
            token.ResetPasswordLink = _cacheHelper.SiteBaseURL + Constants.HC_SetPasswordUrl + id;
            emailTemplate.EmailTemplateBody = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, token);
            isSentMail = Common.SendEmail(emailTemplate.EmailTemplateSubject, 
                                            null, emp.Email, emailTemplate.EmailTemplateBody, 
                                            EnumEmailType.HomeCare_Schedule_Registration_Notification.ToString(), null, 1, attachments);
            return isSentMail;
        }
        public bool SendSetPasswordSMS(Employee emp)
        {
            bool isSentMail = false;
            EmployeeTokens token = new EmployeeTokens();
            EncryptedMailMessageToken encryptedmailmessagetoken = new EncryptedMailMessageToken();

            EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                                            {
                                                new SearchValueData{Name = "EmailTemplateTypeID",Value =Convert.ToInt16(EnumEmailType.EmpRegistration_SMS).ToString(),IsEqual = true}
                                            });
            token.Name = emp.FirstName + " " + emp.LastName;
            token.HomeCareLogoImage = _cacheHelper.SiteBaseURL + Constants.AsapCareLogoImage;
            token.UserName = emp.UserName;
            token.IvrCode = emp.MobileNumber;
            token.IvrPin = emp.IVRPin;
            token.SiteName = _cacheHelper.SiteName;

            encryptedmailmessagetoken.EncryptedValue = Convert.ToInt32(emp.EmployeeID);
            encryptedmailmessagetoken.IsUsed = false;
            encryptedmailmessagetoken.ExpireDateTime = DateTime.Now.AddDays(7);
            SaveObject(encryptedmailmessagetoken);

            var id = Crypto.Encrypt(Convert.ToString(encryptedmailmessagetoken.EncryptedMailID));
            token.ResetPasswordLink = _cacheHelper.SiteBaseURL + Constants.HC_SetPasswordUrl + id;
            emailTemplate.EmailTemplateBody = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, token);
            isSentMail = Common.SendSms(emp.MobileNumber, emailTemplate.EmailTemplateBody, EnumEmailType.EmpRegistration_SMS.ToString());
            return isSentMail;
        }


        public ServiceResponse UploadFile(HttpRequestBase currentHttpRequest)
        {
            ServiceResponse response = new ServiceResponse();
            if (currentHttpRequest.Files.Count != 0)
            {
                string basePath = Common.GetFolderPath((int)Common.FileStorePathType.TempPath);
                var file = currentHttpRequest.Files[0];
                response = Common.SaveFile(file, basePath);
                return response;
            }
            response.Message = "File Upload Failed NoFile Selected";
            return response;
        }

        public List<EmployeePreferenceModel> GetSearchSkill(int pageSize, string searchText = null)
        {
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "SearchText", Value = searchText},
                    new SearchValueData {Name = "PageSize", Value =pageSize.ToString()},
                    new SearchValueData {Name = "PreferenceType", Value =Convert.ToString(Preference.PreferenceKeyType.Preference)}
                };

            List<EmployeePreferenceModel> model = GetEntityList<EmployeePreferenceModel>(StoredProcedure.GetSearchSkill, searchParam) ?? new List<EmployeePreferenceModel>();

            if (model.Count == 0 || model.Count(c => c.PreferenceName == searchText) == 0)
                model.Insert(0, new EmployeePreferenceModel { PreferenceName = searchText });

            return model;

        }
        //public ServiceResponse AddPreference(Preference preference)
        //{
        //    var response = new ServiceResponse();

        //    List<SearchValueData> searchModel = new List<SearchValueData>
        //        {
        //            new SearchValueData { Name = "PreferenceID", Value =Convert.ToString(preference.PreferenceID), IsNotEqual = true },
        //            new SearchValueData { Name = "PreferenceName", Value = preference.PreferenceName, IsNotEqual = true }
        //        };

        //    if ((int)GetScalar(StoredProcedure.CheckForDuplicateAddPreference, searchModel) > 0)
        //    {
        //        //response.Message = Resource.VisitQuestionAlreadyExists;
        //        return response;
        //    }

        //    TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.SavePreference, new List<SearchValueData>
        //        {
        //            new SearchValueData {Name = "PreferenceID",Value = Convert.ToString(preference.PreferenceID)},
        //            new SearchValueData {Name = "PreferenceName",Value =preference.PreferenceName }
        //        });
        //    response.Data = result;
        //    response.IsSuccess = true;
        //    response.Message = string.Format(Resource.RecordCreatedSuccessfully, Resource.Preference);
        //    return response;
        //}
        public ServiceResponse DeletePreference(EmployeePreferenceModel model)
        {
            ServiceResponse response = new ServiceResponse();
            if (model.EmployeePreferenceID > 0)
            {
                var searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData() { Name = "EmployeePreferenceID", Value = Convert.ToString(model.EmployeePreferenceID), IsEqual = true });
                GetScalar(StoredProcedure.DeleteEmployeePreference, searchList);
                response.IsSuccess = true;
            }

            if (!response.IsSuccess)
                response.Message = Resource.ExceptionMessage;

            return response;
        }



        #region Employee Days with time slots

        public ServiceResponse HC_EmployeeTimeSlots()
        {
            var response = new ServiceResponse();


            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                HC_ETSModel model = GetMultipleEntity<HC_ETSModel>(StoredProcedure.GetEmployeeTimeSlotsPageModel);
                response.IsSuccess = true;
                response.Data = model;

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }


            return response;
        }
        public ServiceResponse AddEtsDetailBulk(BulkEmployeeTimeSlotDetails model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            EmployeeTimeSlotMaster etsMaster = new EmployeeTimeSlotMaster();
            EmployeeTimeSlotDetail etsDetail = new EmployeeTimeSlotDetail();
            ServiceResponse masterRes = new ServiceResponse();
            ServiceResponse detailRes = new ServiceResponse();
            Dictionary<long, bool> processList = new Dictionary<long, bool>();
            string details = string.Empty;
            const string NEW_LINE = "<br />";

            long empID;
            string empName;
            bool isSuccess;
            string message;

            var employeeList = GetEntityList<EmployeeDropDownModel>(StoredProcedure.GetEmployeeListForDDL, null);

            foreach (var employeeId in model.EmployeeIDs.Split(Constants.CommaChar))
            {
                empID = Convert.ToInt64(employeeId);
                empName = employeeList.Where(e => e.EmployeeID == empID).Select(e => e.EmployeeName).FirstOrDefault();
                if (string.IsNullOrEmpty(empName)) { empName = $"{empID}"; }
                isSuccess = false;

                etsMaster.EmployeeTimeSlotMasterID = 0;
                etsMaster.EmployeeID = empID;
                etsMaster.StartDate = model.StartDate;
                etsMaster.EndDate = model.EndDate;
                etsMaster.IsEndDateAvailable = model.IsEndDateAvailable;

                masterRes = AddEtsMaster(etsMaster, loggedInUserID);
                message = string.Format("Master: {0}", masterRes.Message);
                if (masterRes != null && masterRes.IsSuccess)
                {
                    etsDetail.EmployeeTimeSlotDetailID = 0;
                    etsDetail.EmployeeTimeSlotMasterID = (long)masterRes.Data;
                    etsDetail.EmployeeID = empID;
                    etsDetail.StartTime = model.StartTime;
                    etsDetail.EndTime = model.EndTime;
                    etsDetail.Notes = model.Notes;
                    etsDetail.SelectedDays = model.SelectedDays;

                    detailRes = AddEtsDetail(etsDetail, loggedInUserID);
                    message = string.Format("Detail: {0}", detailRes.Message);
                    if (detailRes != null && detailRes.IsSuccess)
                    { isSuccess = true; }
                }

                details += string.Format("{0} > {1}", empName, message) + NEW_LINE;
                processList.Add(empID, isSuccess);
            }

            bool isComplete = processList.All(kv => kv.Value == true);
            bool isPartial = false;
            if (!isComplete) { isPartial = processList.Any(kv => kv.Value == true); }

            response.Message = Resource.SomethingWentWrong;
            int data = 0;
            if (isComplete)
            {
                response.Message = string.Format(Resource.RecordCreatedSuccessfully, string.Format("{0} {1}", Resource.All, Resource.TimeSlots));
                data = 1;
            }
            else if (isPartial)
            {
                response.Message = string.Format(Resource.RecordCreatedSuccessfully, string.Format("{0} {1}", Resource.Partial, Resource.TimeSlots));
                data = 2;
            }
            response.Message += NEW_LINE + details;
            response.IsSuccess = isComplete || isPartial;
            response.ErrorCode = isPartial ? Constants.ErrorCode_Warning : null;
            response.Data = data;
            return response;
        }


        #region ETS Msater
        public ServiceResponse GetEtsMasterlist(SearchETSMaster searchETSMaster, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchETSMaster != null)
                SetSearchFilterForEtsMasterListPage(searchETSMaster, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListETSMaster> totalData = GetEntityList<ListETSMaster>(StoredProcedure.GetEtsMasterList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListETSMaster> getEmployeeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getEmployeeList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForEtsMasterListPage(SearchETSMaster searchETSMaster, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(searchETSMaster.EmployeeID) });
            if (searchETSMaster.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchETSMaster.StartDate).ToString(Constants.DbDateFormat) });
            if (searchETSMaster.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchETSMaster.EndDate).ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "TodayDate", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat) });
        }

        public ServiceResponse DeleteEtsMaster(SearchETSMaster searchETSMaster, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForEtsMasterListPage(searchETSMaster, searchList);

            if (!string.IsNullOrEmpty(searchETSMaster.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchETSMaster.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

            List<ListETSMaster> totalData = GetEntityList<ListETSMaster>(StoredProcedure.DeleteEtsMaster, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;


            Page<ListETSMaster> getEmployeeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getEmployeeList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.EmployeeTimeSlot);
            return response;
        }

        public ServiceResponse AddEtsMaster(EmployeeTimeSlotMaster model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> paramList = new List<SearchValueData>();
            paramList.Add(new SearchValueData { Name = "EmployeeTimeSlotMasterID", Value = Convert.ToString(model.EmployeeTimeSlotMasterID) });
            paramList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
            paramList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(model.StartDate).ToString(Constants.DbDateFormat) });
            paramList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate != null ? Convert.ToDateTime(model.EndDate).ToString(Constants.DbDateFormat) : null });
            paramList.Add(new SearchValueData { Name = "IsEndDateAvailable", Value = Convert.ToString(model.IsEndDateAvailable) });
            paramList.Add(new SearchValueData { Name = "TodayDate", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat) });
            paramList.Add(new SearchValueData { Name = "SlotEndDate", Value = Common.GetOrgCurrentDateTime().AddDays(ConfigSettings.GenerateEmpRefTimeScheduleDays).ToString(Constants.DbDateFormat) });
            paramList.Add(new SearchValueData { Name = "loggedInUserID", Value = Convert.ToString(loggedInUserID) });
            paramList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
            TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.AddEtsMaster, paramList);

            if (result.TransactionResultId == -1)
            {
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
                return response;
            }
            if (result.TransactionResultId == -2)
            {
                response.Message = Resource.DateRangeExist;
                return response;
            }

            response.Message = model.EmployeeTimeSlotMasterID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.DateRange) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.DateRange);
            response.IsSuccess = true;
            response.Data = result.TablePrimaryId > 0 ? result.TablePrimaryId : model.EmployeeTimeSlotMasterID;
            return response;
        }


        #endregion





        #region ETS Detail
        public ServiceResponse GetEtsDetaillist(SearchETSDetail searchETSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchETSDetail != null)
                SetSearchFilterForEtsDetailListPage(searchETSDetail, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListETSDetails> totalData = GetEntityList<ListETSDetails>(StoredProcedure.GetEtsDetailList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListETSDetails> getEmployeeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getEmployeeList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForEtsDetailListPage(SearchETSDetail searchETSDetail, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "EmployeeTimeSlotMasterID", Value = Convert.ToString(searchETSDetail.EmployeeTimeSlotMasterID) });
        }

        public ServiceResponse DeleteEtsDetail(SearchETSDetail searchETSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForEtsDetailListPage(searchETSDetail, searchList);

            if (!string.IsNullOrEmpty(searchETSDetail.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchETSDetail.ListOfIdsInCsv });
            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

            List<ListETSDetails> totalData = GetEntityList<ListETSDetails>(StoredProcedure.DeleteEtsDetail, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;


            Page<ListETSDetails> getEmployeeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getEmployeeList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.EmployeeTimeSlot);
            return response;
        }

        public ServiceResponse AddEtsDetail(EmployeeTimeSlotDetail model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> paramList = new List<SearchValueData>();
            paramList.Add(new SearchValueData { Name = "EmployeeTimeSlotDetailID", Value = Convert.ToString(model.EmployeeTimeSlotDetailID) });
            paramList.Add(new SearchValueData { Name = "EmployeeTimeSlotMasterID", Value = Convert.ToString(model.EmployeeTimeSlotMasterID) });
            paramList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
            //paramList.Add(new SearchValueData { Name = "Day", Value = Convert.ToString(model.Day) });
            if (model.StartTime.HasValue)
                paramList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToString(model.StartTime) });
            if (model.EndTime.HasValue)
                paramList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToString(model.EndTime) });
            paramList.Add(new SearchValueData { Name = "AllDay", Value = model.AllDay ? "1" : "0" });
            paramList.Add(new SearchValueData { Name = "Is24Hrs", Value = model.Is24Hrs ? "1" : "0" });
            paramList.Add(new SearchValueData { Name = "Notes", Value = Convert.ToString(model.Notes) });
            paramList.Add(new SearchValueData { Name = "loggedInUserID", Value = Convert.ToString(loggedInUserID) });
            paramList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
            paramList.Add(new SearchValueData { Name = "SelectedDays", Value = string.Join(",", model.SelectedDays) });
            paramList.Add(new SearchValueData { Name = "TodayDate", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat) });
            paramList.Add(new SearchValueData { Name = "SlotEndDate", Value = Common.GetOrgCurrentDateTime().AddDays(ConfigSettings.GenerateEmpRefTimeScheduleDays).ToString(Constants.DbDateFormat) });

            int data = (int)GetScalar(StoredProcedure.AddEtsDetail, paramList);

            switch (data)
            {
                case (int)Common.TimeSlotSaveResult.SQLException:
                    response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotExistsNoRecordsAdded:
                    response.Message = Resource.TimeSlotExist;
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotExistInAnotherDateRange:
                    response.Message = "This time slot is exist in another date range please check";
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotsAdded:
                    response.Message = string.Format(Resource.RecordCreatedSuccessfully, Resource.TimeSlots);
                    response.IsSuccess = true;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotsPartiallyAdded:
                    response.Message = Resource.NonConflictingTimeSlotsAdded;
                    response.IsSuccess = true;
                    break;
            }

            //if (data == -1)
            //{
            //    response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            //    return response;
            //}

            //if (data == -2)
            //{
            //    response.Message = Resource.TimeSlotExist;
            //    return response;
            //}
            //response.Message = model.EmployeeTimeSlotMasterID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.TimeSlot) :
            //        string.Format(Resource.RecordCreatedSuccessfully, Resource.TimeSlot);
            //response.IsSuccess = true;
            response.Data = data;
            return response;
        }

        public ServiceResponse UpdateEtsDetail(EmployeeTimeSlotDetail model, SearchETSDetail searchETSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            //List<SearchValueData> paramList = new List<SearchValueData>();

            List<SearchValueData> paramList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);


            paramList.Add(new SearchValueData { Name = "EmployeeTimeSlotDetailID", Value = Convert.ToString(model.EmployeeTimeSlotDetailID) });
            paramList.Add(new SearchValueData { Name = "EmployeeTimeSlotMasterID", Value = Convert.ToString(model.EmployeeTimeSlotMasterID) });
            paramList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
            //paramList.Add(new SearchValueData { Name = "Day", Value = Convert.ToString(model.Day) });
            if (model.StartTime.HasValue)
                paramList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToString(model.StartTime) });
            if (model.EndTime.HasValue)
                paramList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToString(model.EndTime) });
            paramList.Add(new SearchValueData { Name = "AllDay", Value = model.AllDay ? "1" : "0" });
            paramList.Add(new SearchValueData { Name = "Is24Hrs", Value = model.Is24Hrs ? "1" : "0" });
            paramList.Add(new SearchValueData { Name = "Notes", Value = Convert.ToString(model.Notes) });
            paramList.Add(new SearchValueData { Name = "loggedInUserID", Value = Convert.ToString(loggedInUserID) });
            paramList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
            paramList.Add(new SearchValueData { Name = "SelectedDays", Value = Convert.ToString(model.Day) });
            paramList.Add(new SearchValueData { Name = "TodayDate", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat) });
            paramList.Add(new SearchValueData { Name = "SlotEndDate", Value = Common.GetOrgCurrentDateTime().AddDays(ConfigSettings.GenerateEmpRefTimeScheduleDays).ToString(Constants.DbDateFormat) });
            paramList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchETSDetail.ListOfIdsInCsv });
            paramList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(false) });

            int data = (int)GetScalar(StoredProcedure.UpdateEtsDetail, paramList);

            switch (data)
            {
                case (int)Common.TimeSlotSaveResult.SQLException:
                    response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotExistsNoRecordsAdded:
                    response.Message = Resource.TimeSlotExist;
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotExistInAnotherDateRange:
                    //response.Message = "This time slot is exist in another date range please check";
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotsAdded:
                    response.Message = string.Format(Resource.RecordCreatedSuccessfully, Resource.TimeSlots);
                    response.IsSuccess = true;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotsPartiallyAdded:
                    response.Message = Resource.NonConflictingTimeSlotsAdded;
                    response.IsSuccess = true;
                    break;
            }
            response.Data = data;
            return response;
        }

        public ServiceResponse EmployeeTimeSlotForceUpdate(EmployeeTimeSlotDetail model, SearchETSDetail searchETSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> paramList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            //List<SearchValueData> paramList = new List<SearchValueData>();
            paramList.Add(new SearchValueData { Name = "EmployeeTimeSlotDetailID", Value = Convert.ToString(model.EmployeeTimeSlotDetailID) });
            paramList.Add(new SearchValueData { Name = "EmployeeTimeSlotMasterID", Value = Convert.ToString(model.EmployeeTimeSlotMasterID) });
            paramList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
            //paramList.Add(new SearchValueData { Name = "Day", Value = Convert.ToString(model.Day) });
            paramList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToString(model.StartTime) });
            paramList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToString(model.EndTime) });
            paramList.Add(new SearchValueData { Name = "Notes", Value = Convert.ToString(model.Notes) });
            paramList.Add(new SearchValueData { Name = "loggedInUserID", Value = Convert.ToString(loggedInUserID) });
            paramList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
            paramList.Add(new SearchValueData { Name = "SelectedDays", Value = model.SelectedDays != null ? string.Join(",", model.SelectedDays) : Convert.ToString(model.Day) });
            paramList.Add(new SearchValueData { Name = "TodayDate", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat) });
            paramList.Add(new SearchValueData { Name = "SlotEndDate", Value = Common.GetOrgCurrentDateTime().AddDays(ConfigSettings.GenerateEmpRefTimeScheduleDays).ToString(Constants.DbDateFormat) });
            paramList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchETSDetail.ListOfIdsInCsv });
            paramList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(false) });
            paramList.Add(new SearchValueData { Name = "IsEdit", Value = (searchETSDetail.EmployeeTimeSlotMasterID == 0) ? Convert.ToString(false) : Convert.ToString(true) });

            int data = (int)GetScalar(StoredProcedure.EmployeeTimeSlotForceUpdate, paramList);

            switch (data)
            {
                case (int)Common.TimeSlotSaveResult.SQLException:
                    response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotExistsNoRecordsAdded:
                    response.Message = Resource.TimeSlotExist;
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotExistInAnotherDateRange:
                    //response.Message = "This time slot is exist in another date range please check";
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotsAdded:
                    response.Message = string.Format(Resource.RecordCreatedSuccessfully, Resource.TimeSlots);
                    response.IsSuccess = true;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotsPartiallyAdded:
                    response.Message = Resource.NonConflictingTimeSlotsAdded;
                    response.IsSuccess = true;
                    break;
            }

            //if (data == -1)
            //{
            //    response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            //    return response;
            //}

            //if (data == -2)
            //{
            //    response.Message = Resource.TimeSlotExist;
            //    return response;
            //}
            //response.Message = model.EmployeeTimeSlotMasterID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.TimeSlot) :
            //        string.Format(Resource.RecordCreatedSuccessfully, Resource.TimeSlot);
            //response.IsSuccess = true;
            response.Data = data;
            return response;
        }


        #endregion





        #endregion

        #region Employee Calender
        public ServiceResponse HC_EmployeeCalender()
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                HC_EmpCalenderModel model = GetMultipleEntity<HC_EmpCalenderModel>(StoredProcedure.GetEmployeeCalenderPageModel);
                //model.SearchEmpCalender.StartDate = DateTime.Now;
                model.SearchEmpCalender.StartDate = Common.GetOrgStartOfWeek();
                response.IsSuccess = true;
                response.Data = model;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }


            return response;
        }



        #endregion







        #region Employee Day Off


        public ServiceResponse HC_EmployeeDayOffPage()
        {
            var response = new ServiceResponse();

            SetEmpDayOffListPage model = GetMultipleEntity<SetEmpDayOffListPage>(StoredProcedure.HC_EmployeeDayOffPage);

            model.SearchEmployeeDayOffModel = new SearchEmpDayOffModel();
            model.EmployeeDayOff = new EmployeeDayOff();
            model.DeleteFilter = Common.SetDeleteFilter();
            model.DayOffTypes = Common.SetDayOffTypes();
            model.SearchEmployeeDayOffModel.IsDeleted = 0;
            response.Data = model;
            return response;
        }


        public ServiceResponse HC_SaveEmployeeDayOff(EmployeeDayOff model, long loggedInUserId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "EmployeeDayOffID", Value = model.EmployeeDayOffID.ToString(), IsEqual = true });
            searchList.Add(new SearchValueData() { Name = "EmployeeID", Value = model.EmployeeID.ToString(), IsEqual = true });
            searchList.Add(new SearchValueData() { Name = "StartTime", Value = model.StrStartTime }); //TimeZoneInfo.ConvertTimeToUtc(model.StartTime).ToString(Constants.DbDateTimeFormat)
            searchList.Add(new SearchValueData() { Name = "EndTime", Value = model.StrEndTime }); //TimeZoneInfo.ConvertTimeToUtc(model.EndTime).ToString(Constants.DbDateTimeFormat)
            searchList.Add(new SearchValueData() { Name = "DayOffStatus", Value = Convert.ToString(EmployeeDayOff.EmployeeDayOffStatus.InProgress) });
            searchList.Add(new SearchValueData() { Name = "DayOffTypeID", Value = Convert.ToString(model.DayOffTypeID) });
            searchList.Add(new SearchValueData() { Name = "EmployeeComment", Value = model.EmployeeComment });
            searchList.Add(new SearchValueData() { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });
            searchList.Add(new SearchValueData() { Name = "SystemID", Value = Common.GetHostAddress() });
            int data = (int)GetScalar(StoredProcedure.HC_SaveEmployeeDayOff, searchList);

            if (data == 1)
            {
                response.IsSuccess = true;
                response.Message = model.EmployeeDayOffID > 0
                                             ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Record)
                                             : string.Format(Resource.RecordCreatedSuccessfully, Resource.Record);
            }

            return response;
        }


        public ServiceResponse HC_CheckForEmpSchedules(EmployeeDayOff model)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "EmployeeDayOffID", Value = model.EmployeeDayOffID.ToString(), IsEqual = true });
            searchList.Add(new SearchValueData() { Name = "EmployeeID", Value = model.EmployeeID.ToString(), IsEqual = true });
            List<PatientScheduleList> data = GetEntityList<PatientScheduleList>(StoredProcedure.HC_GetEmpSchedulesOnDayOff, searchList);

            response.Data = data;
            response.IsSuccess = true;
            return response;
        }



        public ServiceResponse HC_DayOffAction(EmployeeDayOff model, long loggedInUserId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "EmployeeDayOffID", Value = model.EmployeeDayOffID.ToString(), IsEqual = true });
            searchList.Add(new SearchValueData() { Name = "ApproverComment", Value = model.ApproverComment });
            searchList.Add(new SearchValueData() { Name = "DayOffStatus", Value = Convert.ToString(model.DayOffStatus) });
            searchList.Add(new SearchValueData() { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });
            searchList.Add(new SearchValueData() { Name = "SystemID", Value = Common.GetHostAddress() });
            int data = (int)GetScalar(StoredProcedure.HC_DayOffAction, searchList);

            if (data == 1)
            {
                response.IsSuccess = true;
                response.Message = model.EmployeeDayOffID > 0
                                             ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Record)
                                             : string.Format(Resource.RecordCreatedSuccessfully, Resource.Record);
            }

            return response;
        }

        public ServiceResponse HC_GetEmployeeDayOffList(SearchEmpDayOffModel model, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (model != null)
                SetSearchFilterForEmployeeDayOffListPage(model, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListEmpDayOffModel> totalData = GetEntityList<ListEmpDayOffModel>(StoredProcedure.HC_GetEmployeeDayOffList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListEmpDayOffModel> getEmployeeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getEmployeeList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForEmployeeDayOffListPage(SearchEmpDayOffModel model, List<SearchValueData> searchList)
        {
            //if (!string.IsNullOrEmpty(model.Employee))
            //    searchList.Add(new SearchValueData { Name = "Employee", Value = Convert.ToString(model.Employee) });

            if (model.EmployeeID > 0)
                searchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });

            if (model.StartTime.HasValue)
                //searchList.Add(new SearchValueData { Name = "StartTime", Value = TimeZoneInfo.ConvertTimeToUtc(model.StartTime.Value).ToString(Constants.DbDateTimeFormat) });
                searchList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToDateTime(model.StartTime).ToString(Constants.DbDateFormat) });

            if (model.EndTime.HasValue)
                //searchList.Add(new SearchValueData { Name = "EndTime", Value = TimeZoneInfo.ConvertTimeToUtc(model.EndTime.Value).ToString(Constants.DbDateTimeFormat) });
                searchList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToDateTime(model.EndTime).ToString(Constants.DbDateFormat) });

            if (!string.IsNullOrEmpty(model.SubmittedBy))
                searchList.Add(new SearchValueData { Name = "SubmittedBy", Value = Convert.ToString(model.SubmittedBy) });

            if (model.SubmittedDate.HasValue)
                searchList.Add(new SearchValueData { Name = "SubmittedDate", Value = Convert.ToDateTime(model.SubmittedDate).ToString(Constants.DbDateFormat) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(model.IsDeleted) });

        }

        public ServiceResponse HC_DeleteEmployeeDayOffList(SearchEmpDayOffModel model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForEmployeeDayOffListPage(model, searchList);

            if (!string.IsNullOrEmpty(model.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = model.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });
            searchList.Add(new SearchValueData { Name = "DayOffStatusInProgress", Value = Convert.ToString(EmployeeDayOff.EmployeeDayOffStatus.InProgress) });


            List<ListEmpDayOffModel> totalData = GetEntityList<ListEmpDayOffModel>(StoredProcedure.HC_DeleteEmployeeDayOff, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListEmpDayOffModel> getEmployeeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getEmployeeList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.EmployeeDayOff);
            return response;
        }






        #endregion






        #region Send BULK SMS To Employees


        public ServiceResponse HC_SendBulkSms()
        {
            ServiceResponse response = new ServiceResponse();
            HC_SendBulkSmsModel model = new HC_SendBulkSmsModel();
            response.Data = model;
            return response;
        }

        public ServiceResponse HC_GetEmployeeListForSendSms(SearchSBSEmployeeModel model)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "EmployeeName", Value = model.EmployeeName });
            searchList.Add(new SearchValueData() { Name = "MobileNumber", Value = model.MobileNumber });
            searchList.Add(new SearchValueData() { Name = "ReferralTsDateID", Value = !string.IsNullOrWhiteSpace(model.EncryptedId) ? Crypto.Decrypt(model.EncryptedId) : "0" });
            List<GetSBSEmployeeList> list = GetEntityList<GetSBSEmployeeList>(StoredProcedure.GetSBSEmployeeList, searchList);
            response.Data = list;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_SendBulkSms(SendSMSModel model, long loggedinId)
        {
            ServiceResponse response = new ServiceResponse();


            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "EmployeeIDs", Value = model.EmployeeIds });
            List<TwilioNotifyModel> listModel = GetEntityList<TwilioNotifyModel>(StoredProcedure.GetTwilioNotifyModel, searchList);


            List<string> binding = new List<string>();

            string CountryCode = string.IsNullOrEmpty(_cacheHelper.TwilioDefaultCountryCode) ? Constants.DefaultCountryCodeForSms : _cacheHelper.TwilioDefaultCountryCode;

            foreach (var item in listModel)
            {
                item.address = CountryCode + item.address;
                //item.address = ConfigSettings.DefaultCountryCodeForSms + item.address;
                //string data = "{\"binding_type\":\"" + item.BindingType + "\",\"address\":\"" + item.PhoneNumber + "\"}";
                binding.Add(JsonConvert.SerializeObject(item));

            }
            var notificationSid = Common.SendTwilioNotification(model.Message, binding);

            if (!string.IsNullOrEmpty(notificationSid))
            {
                searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData() { Name = "EmployeeIDs", Value = model.EmployeeIds });
                searchList.Add(new SearchValueData() { Name = "Message", Value = model.Message });
                searchList.Add(new SearchValueData() { Name = "NotificationSID", Value = Convert.ToString(notificationSid) });
                searchList.Add(new SearchValueData() { Name = "LoggedInID", Value = Convert.ToString(loggedinId) });
                searchList.Add(new SearchValueData() { Name = "SystemID", Value = Common.GetMAcAddress() });
                int val = (int)GetScalar(StoredProcedure.SaveGroupMessageLogs, searchList);

                if (val == 1)
                {
                    response.Message = Resource.SMSMsgSentSuccess;
                    response.IsSuccess = true;
                    //response.Data = model;
                    return response;
                }

            }
            response.Message = Resource.ExceptionMessage;
            return response;
        }

        public ServiceResponse GetSentSmsList(SearchSentSmsModel model, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();


            searchList.Add(new SearchValueData { Name = "Message", Value = Convert.ToString(model.Message) });
            searchList.Add(new SearchValueData { Name = "NotificationSID", Value = Convert.ToString(model.NotificationSID) });
            //searchList.Add(new SearchValueData { Name = "Name", Value = Convert.ToString(searchEmployeeModel.Name) });
            //searchList.Add(new SearchValueData { Name = "Name", Value = Convert.ToString(searchEmployeeModel.Name) });


            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<SentSMSListModel> totalData = GetEntityList<SentSMSListModel>(StoredProcedure.GetSentSMSList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<SentSMSListModel> getSentSmsList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getSentSmsList;
            response.IsSuccess = true;
            return response;
        }



        public ServiceResponse GetEmployeesForSentSms(long groupMessageLogId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "GroupMessageLogID", Value = Convert.ToString(groupMessageLogId) });
            List<GetSBSEmployeeList> totalData = GetEntityList<GetSBSEmployeeList>(StoredProcedure.GetEmployeesForSentSms, searchList);
            response.Data = totalData;
            response.IsSuccess = true;
            return response;
        }



        #endregion

        #region Upload Employee Signature From API
        public ServiceResponse UploadEmpSignature(HttpRequestBase currentHttpRequest)
        {
            ServiceResponse response = new ServiceResponse();
            MemoryStream ms = new MemoryStream();

            var EmployeeID = currentHttpRequest.Headers["EmployeeID"];
            var EmployeeSignatureID = currentHttpRequest.Headers["EmployeeSignatureID"];
            var ImageKey = currentHttpRequest.Headers["ImageKey"];

            currentHttpRequest.InputStream.CopyTo(ms);
            byte[] byteData = ms.ToArray();

            if (byteData.Length > 0)
            {
                string basePath = string.Empty;
                if (ImageKey == "Siganture")
                {
                    basePath = Common.GetFolderPath((int)Common.FileStorePathType.EmpSignatures);
                }
                if (ImageKey == "ProfilePic")
                {
                    basePath = Common.GetFolderPath((int)Common.FileStorePathType.EmpProfileImg);
                }
                string fullPath = basePath + EmployeeID + "/";


                DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapCustomPath(fullPath));

                if (di.Exists)
                {
                    foreach (FileInfo item in di.GetFiles())
                    {
                        item.Delete();
                    }
                }

                response = Common.ByteToImage(byteData, fullPath);

                List<SearchValueData> srchParam = new List<SearchValueData>();

                srchParam.Add(new SearchValueData("EmployeeSignatureID", Convert.ToString(EmployeeSignatureID)));
                srchParam.Add(new SearchValueData("EmployeeID", Convert.ToString(EmployeeID)));
                srchParam.Add(new SearchValueData("SignaturePath", ((UploadedFileModel)response.Data).TempFilePath));
                srchParam.Add(new SearchValueData("ImageKey", ImageKey));

                GetScalar(StoredProcedure.UpdateEmployeeSignature, srchParam);
                //GetScalar(StoredProcedure.UpdateEmployeeSignature, new List<SearchValueData>
                //    {
                //        new SearchValueData("EmployeeSignatureID", Convert.ToString(EmployeeSignatureID)),
                //        new SearchValueData("EmployeeID", Convert.ToString(EmployeeID)),
                //        new SearchValueData("SignaturePath", ((UploadedFileModel)response.Data).TempFilePath)
                //    });

                return response;
            }
            response.Message = "File Upload Failed NoFile Selected";
            return response;
        }
        #endregion



        #endregion

        #region Broadcast Notification

        public ServiceResponse HC_BroadcastNotification(string type, long Id, string siteName)
        {
            ServiceResponse response = new ServiceResponse();
            HC_SendBulkSmsModel model = new HC_SendBulkSmsModel();
            if (type != Convert.ToString(Mobile_Notification.NotificationTypes.ScheduleNotification).ToLower() || Id == 0)
            {
                model.SendSMSModel.NotificationType = (int)Mobile_Notification.NotificationTypes.BroadcastNotification;
                response.Data = model;
            }
            else
            {
                model = GetMultipleEntity<HC_SendBulkSmsModel>(StoredProcedure.GetDataForBroadcastNotification,
                    new List<SearchValueData>
                    {
                        new SearchValueData("Type",type),
                        new SearchValueData("Id",Convert.ToString(Id)),
                        new SearchValueData("ScheduleNotification",Convert.ToString(Mobile_Notification.NotificationTypes.ScheduleNotification).ToLower())
                    });
                model.SendSMSModel.NotificationType = (int)Mobile_Notification.NotificationTypes.ScheduleNotification;
                model.IsModelDetailBind = true;

                string preferably = string.Empty;
                if (!string.IsNullOrWhiteSpace(model.PatientModel.PreferenceNames))
                {
                    preferably = string.Format(Resource.Preferably, model.PatientModel.PreferenceNames);
                }

                string referralTSDay = string.Empty;
                if (model.PatientModel.ReferralTSStartTime.HasValue)
                {
                    referralTSDay = model.PatientModel.ReferralTSStartTime.Value.ToString(Constants.DateFormatWithDay, CultureInfo.InvariantCulture);
                }
                string referralTSStartTime = string.Empty;
                if (model.PatientModel.ReferralTSStartTime.HasValue)
                {
                    referralTSStartTime = model.PatientModel.ReferralTSStartTime.Value.ToString(Constants.TimeFormat);
                }
                string referralTSEndTime = string.Empty;
                if (model.PatientModel.ReferralTSEndTime.HasValue)
                {
                    referralTSEndTime = model.PatientModel.ReferralTSEndTime.Value.ToString(Constants.TimeFormat);
                }

                model.ScheduleNotificationMessageContent = string.Format(Resource.ScheduleNotificationMessageContent, siteName, referralTSDay,
                    referralTSStartTime, referralTSEndTime, preferably);

                response.Data = model;
            }
            return response;
        }

        #region Web Notifications

        public ServiceResponse HC_SaveWebNotification(SendSMSModel model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                int result = (int)GetScalar(StoredProcedure.CreateWebNotifications,
                    new List<SearchValueData> {
                            new SearchValueData("Message", model.Message),
                            new SearchValueData("ServerDateTime", DateTime.UtcNow.ToString(Constants.DbDateTimeFormat)),
                            new SearchValueData("EmployeeIDs", model.EmployeeIds),
                            new SearchValueData("LoggedInId", Convert.ToString(loggedInUserID)),
                     });
                if (result > 0)
                {
                    response.IsSuccess = true;
                    response.Message = Resource.RecordSavedSuccessfully;
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

        #endregion

        //TODO: FCM notification code testing is remain for testing
        public ServiceResponse HC_SaveBroadcastNotification(SendSMSModel model, long loggedinId, string domainName)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                TransactionResultWithBroadcastUserDetails result = GetMultipleEntity<TransactionResultWithBroadcastUserDetails>(
                    StoredProcedure.CreateBroadcastNotifications, new List<SearchValueData> {
                            new SearchValueData("Message",model.Message,"NVarchar"),
                            new SearchValueData("NotificationType",Convert.ToString(model.NotificationType)),
                            new SearchValueData("ServerDateTime",DateTime.UtcNow.ToString(Constants.DbDateTimeFormat)),
                            new SearchValueData("NotificationStatus",Convert.ToString((int)Mobile_Notification.NotificationStatuses.Sent)),
                            new SearchValueData("EmployeeIDs", model.EmployeeIds),
                            new SearchValueData("PrimaryId",!string.IsNullOrWhiteSpace(model.EncryptedId) ? Crypto.Decrypt(model.EncryptedId) : string.Empty),
                            new SearchValueData("LoggedInId",Convert.ToString(loggedinId)),
                     });
                if (result.TransactionResult.TransactionResultId > 0)
                {
                    if (result.BroadcastNotificationUserDetailList != null && result.BroadcastNotificationUserDetailList.Count > 0)
                    {
                        FcmManager fcmManager = new FcmManager(new FcmManagerOptions
                        {
                            AuthenticationKey = ConfigSettings.FcmAuthenticationKey,
                            SenderId = ConfigSettings.FcmSenderId
                        });
                        var fcmResponseIos = fcmManager.SendMessage(new FcmMessage
                        {
                            RegistrationIds = result.BroadcastNotificationUserDetailList.Where(x => x.DeviceType.ToLower() == Constants.ios).Select(x => x.FcmTokenId).ToList(),
                            Notification = new FcmNotification
                            {
                                Body = model.Message,
                                Title = domainName
                            },
                            Data = new MobileNotificationModel
                            {
                                SiteName = domainName,
                                Body = model.Message,
                                NotificationType = model.NotificationType,
                                PrimaryId = !string.IsNullOrWhiteSpace(model.EncryptedId) ? Convert.ToInt32(Crypto.Decrypt(model.EncryptedId)) : 0
                            },
                        });
                        //Common.CreateLogFile(Common.SerializeObject(fcmResponseIos));
                        //Common.CreateLogFile(Common.SerializeObject(result.BroadcastNotificationUserDetailList.Where(x => x.DeviceType.ToLower() == "ios").Select(x => x.FcmTokenId).ToList()));
                        var fcmResponseAndroid = fcmManager.SendMessage(new FcmMessage
                        {
                            RegistrationIds = result.BroadcastNotificationUserDetailList.Where(x => x.DeviceType.ToLower() == Constants.android).Select(x => x.FcmTokenId).ToList(),
                            Notification = null,
                            Data = new MobileNotificationModel
                            {
                                SiteName = domainName,
                                Body = model.Message,
                                NotificationType = model.NotificationType,
                                PrimaryId = !string.IsNullOrWhiteSpace(model.EncryptedId) ? Convert.ToInt32(Crypto.Decrypt(model.EncryptedId)) : 0
                            },
                        });
                        //Common.CreateLogFile(Common.SerializeObject(fcmResponseAndroid));
                        //Common.CreateLogFile(Common.SerializeObject(result.BroadcastNotificationUserDetailList.Where(x => x.DeviceType.ToLower() == "android").Select(x => x.FcmTokenId).ToList()));
                    }
                    response.IsSuccess = true;
                    response.Message = Resource.BroadcastNotificationSuccessfully;
                }
                else
                {
                    response.Message = Resource.ExceptionMessage;
                }
            }
            catch (Exception e)
            {
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }

        public ServiceResponse HC_GetBroadcastNotification(SearchSentSmsModel model, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>
            {
                new SearchValueData("Message",Convert.ToString(model.Message)),
            };

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<BroadcastNotificationListModel> totalData = GetEntityList<BroadcastNotificationListModel>(
                StoredProcedure.GetBroadcastNotificationList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<BroadcastNotificationListModel> getBroadcastNotificationList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getBroadcastNotificationList;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetEmployeesForBroadcastNotifications(SearchSBSEmployeeModel searchSBSEmployeeModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "NotificationId", Value = Convert.ToString(searchSBSEmployeeModel.NotificationId) });
            searchList.Add(new SearchValueData { Name = "ScheduleNotificationAction", Value = searchSBSEmployeeModel.ScheduleNotificationAction });
            List<GetSBSEmployeeList> totalData = GetEntityList<GetSBSEmployeeList>(StoredProcedure.GetEmployeesForBroadcastNotifications, searchList);
            response.Data = totalData;
            response.IsSuccess = true;
            return response;
        }


        #endregion

        #region Employee Audit Logs

        public ServiceResponse GetAuditLogList(SearchRefAuditLogListModel searchModel, int pageIndex, int pageSize,
                                                string sortIndex, string sortDirection, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchModel != null)
            {
                searchList.Add(new SearchValueData { Name = "ParentKeyFieldID", Value = searchModel.EncryptedReferralID });
                searchList.Add(new SearchValueData { Name = "UpdatedBy", Value = searchModel.UpdatedBy });
                searchList.Add(new SearchValueData { Name = "Table", Value = searchModel.Table });
                searchList.Add(new SearchValueData { Name = "ActionName", Value = searchModel.ActionName });
                if (searchModel.UpdatedFromDate.HasValue)
                    searchList.Add(new SearchValueData { Name = "FromDate", Value = TimeZoneInfo.ConvertTimeToUtc(searchModel.UpdatedFromDate.Value).ToString(Constants.DbDateTimeFormat) });
                if (searchModel.UpdatedToDate.HasValue)
                    searchList.Add(new SearchValueData { Name = "ToDate", Value = TimeZoneInfo.ConvertTimeToUtc(searchModel.UpdatedToDate.Value.AddDays(1)).ToString(Constants.DbDateTimeFormat) });

            }

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<AuditChangeModel> totalData = GetEntityList<AuditChangeModel>(StoredProcedure.GetAuditLogList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<AuditChangeModel> getAuditChangeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getAuditChangeList;
            response.IsSuccess = true;
            return response;
        }

        #endregion


        public ServiceResponse HC_SaveEmployeeDocument(HttpRequestBase currentHttpRequest)
        {
            ServiceResponse response = new ServiceResponse();
            var EmployeeId = currentHttpRequest.Form["id"];
            string basePath = String.Format(_cacheHelper.EmployeeDocumentFullPath, _cacheHelper.Domain) + EmployeeId + "/";
            string KindOfDocument = Convert.ToString(DocumentType.DocumentKind.Internal);
            int DocumentTypeID = (int)DocumentType.DocumentTypes.Other;
            int UserType = (int)ReferralDocument.UserTypes.Employee;

            if (currentHttpRequest.Files.Count != 0)
            {
                for (int i = 0; i < currentHttpRequest.Files.Count; i++)
                {
                    HttpPostedFileBase file = currentHttpRequest.Files[i];
                    var fileResponse = Common.SaveFile(file, basePath);

                    if (fileResponse.IsSuccess)
                    {
                        try
                        {
                            var actualFilepath = ((UploadedFileModel)fileResponse.Data).TempFilePath;
                            List<SearchValueData> searchList = new List<SearchValueData>()
                            {
                                new SearchValueData {Name = "FileName", Value = file.FileName},
                                new SearchValueData {Name = "FilePath", Value = actualFilepath},
                                new SearchValueData {Name = "UserID ", Value = EmployeeId },
                                new SearchValueData {Name = "UserType", Value = UserType.ToString() },
                                new SearchValueData {Name = "KindOfDocument ", Value = KindOfDocument },
                                //new SearchValueData {Name = "DocumentTypeID ", Value = DocumentTypeID.ToString() },
                                new SearchValueData {Name = "CurrentDate", Value = Convert.ToString(Common.GetOrgCurrentDateTime())},
                                new SearchValueData {Name = "LoggedInUserID", Value = SessionHelper.LoggedInID.ToString()},
                                new SearchValueData {Name = "SystemID ", Value = HttpContext.Current.Request.UserHostAddress }
                            };
                            GetScalar(StoredProcedure.HC_SaveUserDocument, searchList);
                            response.IsSuccess = true;
                            return response;
                        }
                        catch (Exception e)
                        {
                            response.IsSuccess = false;
                            response.Message = e.Message;
                            return response;
                        }
                    }
                    else
                    {
                        response.Message = fileResponse.Message;
                        return response;
                    }
                }
            }
            response.Message = Resource.FileUploadFailedNoFileSelected;
            return response;
        }

        public ServiceResponse HC_GetEmployeeDocumentList(long EmployeeID, int pageIndex, int pageSize,
            string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            //List<SearchValueData> searchList = new List<SearchValueData>();
            //searchList.Add(new SearchValueData { Name = "ReferralID", Value = referralID.ToString(),IsEqual=true});
            //searchList.Add(new SearchValueData { Name = "UserType", Value = "1" , IsEqual=true});
            var customFIlter = "UserID=" + EmployeeID + " AND UserType=" + (int)ReferralDocument.UserTypes.Employee;
            response = GetPageRecords<ReferralDocumentList>(pageSize, pageIndex, sortIndex, sortDirection, null, customFIlter);
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_DeleteEmployeeDocument(long referralDocumentID, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            var UserType = (int)ReferralDocument.UserTypes.Employee;
            if (referralDocumentID > 0)
            {
                ReferralDocument referralDocument = GetEntity<ReferralDocument>(referralDocumentID);
                if (referralDocument != null)
                {
                    var fullPath = HttpContext.Current.Server.MapCustomPath(referralDocument.FilePath);
                    response = Common.DeleteFile(fullPath);

                    List<SearchValueData> searchParam = new List<SearchValueData>();
                    searchParam.Add(new SearchValueData("ReferralDocumentID", Convert.ToString(referralDocument.ReferralDocumentID)));
                    searchParam.Add(new SearchValueData("UserID", Convert.ToString(referralDocument.UserID)));
                    searchParam.Add(new SearchValueData("DocumentTypeID", Convert.ToString(referralDocument.DocumentTypeID)));
                    GetScalar(StoredProcedure.HC_DeleteDocument, searchParam);
                }
                response.IsSuccess = true;
                response.Data = HC_GetEmployeeDocumentList(referralDocument.UserID, pageIndex, pageSize, sortIndex,
                                                        sortDirection).Data;
                response.Message = Resource.DocumentDeleted;
            }
            else
            {
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }

        public ServiceResponse HC_SaveEmployeeDocument(ReferralDocument referralDoc, int pageIndex, int pageSize, string sortIndex,
                                            string sortDirection, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            var UserType = (int)ReferralDocument.UserTypes.Employee;
            if (referralDoc.ReferralDocumentID > 0)
            {
                ReferralDocument referralDocument = GetEntity<ReferralDocument>(referralDoc.ReferralDocumentID);
                Referral referral = GetEntity<Referral>(referralDocument.UserID);
                List<SearchValueData> searchParam = new List<SearchValueData>();

                searchParam.Add(new SearchValueData("ReferralDocumentID", Convert.ToString(referralDoc.ReferralDocumentID)));
                searchParam.Add(new SearchValueData("FileName", referralDoc.FileName));
                searchParam.Add(new SearchValueData("KindOfDocument", referralDoc.KindOfDocument));
                searchParam.Add(new SearchValueData("UserID", Convert.ToString(referralDoc.UserID)));
                searchParam.Add(new SearchValueData("DocumentTypeID", Convert.ToString(referralDoc.DocumentTypeID)));
                searchParam.Add(new SearchValueData("UpdatedDate", Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat)));
                searchParam.Add(new SearchValueData("LoggedInUserID", Convert.ToString(loggedInUserID)));
                searchParam.Add(new SearchValueData("SystemID", Common.GetHostAddress()));

                int data = (int)GetScalar(StoredProcedure.HC_SaveEmpDocument, searchParam);

                if (data == 1)
                {
                    response.IsSuccess = true;
                    response.Message = Resource.DocumentUpdated;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = Resource.DocumentTypeExists;
                }
                response.Data =
                    HC_GetEmployeeDocumentList(referralDoc.UserID, pageIndex, pageSize, sortIndex, sortDirection).Data;


                //ReferralDocument referralDocument = GetEntity<ReferralDocument>(referralDoc.ReferralDocumentID);
                //referralDocument.FileName = referralDoc.FileName;
                //SaveObject(referralDocument, loggedInUserID);
                //response.IsSuccess = true;
                //response.Message = Resource.DocumentUpdated;
                //response.Data =
                //    HC_GetEmployeeDocumentList(referralDocument.UserID, pageIndex, pageSize, sortIndex, sortDirection).Data;
            }
            else
            {
                response.Message = Resource.DocumentTypeExists;
            }
            return response;
        }


        public ServiceResponse HC_GenerateCertificateForEmployee(long EmployeeID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchparam = new List<SearchValueData>();
                searchparam.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(EmployeeID) });
                Employee employee = GetEntity<Employee>(StoredProcedure.HC_GetEmpDataForGenerateCertificate, searchparam);
                //if(!string.IsNullOrEmpty(employee.Designation))
                //{
                //    int designation = Convert.ToInt16(employee.Designation);
                //    employee.str_Designation = Common.GetEnumDisplayValue<Common.EmployeeDesignationType>((Common.EmployeeDesignationType)designation); // EnumHelper<Common.GetEnumDisplayValue>.GetDisplayValue(enu)
                //}
                response.IsSuccess = true;
                response.Data = employee;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
            }
            return response;
        }
        public ServiceResponse GetEmployeeOverTimePayBillingReportList(SearchEmployeeBillingReportListPage searchEmployeeBillingReportPage, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            if (searchEmployeeBillingReportPage != null)
                SetSearchFilterForEmployeeOverTimepayBillingReportList(searchEmployeeBillingReportPage, searchList);

            List<EmployeeBillingReportListModel> totalData = GetEntityList<EmployeeBillingReportListModel>(StoredProcedure.HC_GetEmployeeOverPayReportList, searchList);
            //if (!Common.HasPermission(Constants.AllRecordAccess))
            //{
            //    totalData = totalData.Where(_ => _.EmployeeID == Convert.ToInt32(SessionHelper.LoggedInID)).ToList();
            //}
            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<EmployeeBillingReportListModel> employeeBillingReportList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = employeeBillingReportList;
            response.IsSuccess = true;
            return response;
        }
        public TokenData GetToken(long employeeId, int expireLoginDuration, string token, bool isMobileToken)
        {
            var searchList = new List<SearchValueData>
            {
                new SearchValueData("EmployeeId", Convert.ToString(employeeId)),
                new SearchValueData("ExpireLoginDuration", Convert.ToString(expireLoginDuration)),
                new SearchValueData("Token", token),
                new SearchValueData("ServerCurrentDateTime", DateTime.UtcNow.ToString(Constants.DbDateTimeFormat)),
                new SearchValueData
                {
                    Name = "IsMobileToken",
                    Value = isMobileToken ? "1" : "0",
                    DataType = Constants.DataTypeBoolean
                }
            };
            return GetEntity<TokenData>(StoredProcedure.GetToken, searchList);
        }
        private static void SetSearchFilterForEmployeeOverTimepayBillingReportList(SearchEmployeeBillingReportListPage searchEmployeeBillingReportPage, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(searchEmployeeBillingReportPage.EmployeeID) });

            if (searchEmployeeBillingReportPage.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(searchEmployeeBillingReportPage.StartDate) });

            if (searchEmployeeBillingReportPage.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(searchEmployeeBillingReportPage.EndDate) });
        }
        public ServiceResponse SaveRegularHours(RegularHoursModel model)
        {
            var response = new ServiceResponse();
            try
            {
                int data = (int)GetScalar(StoredProcedure.SaveRegularHours, new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeID",Value = Convert.ToString(model.EmployeeID)},
                    new SearchValueData {Name = "RegularHours",Value = Convert.ToString(model.RegularHours)},
                    new SearchValueData {Name = "RegularHourType",Value = Convert.ToString(model.RegularHourType)},
                    new SearchValueData {Name = "RegularPayHours",Value = Convert.ToString(model.RegularPayHours)},
                    new SearchValueData {Name = "OvertimePayHours",Value = Convert.ToString(model.OvertimePayHours)},
                   // new SearchValueData {Name = "OvertimeHours",Value = Convert.ToString(model.OvertimeHours)},
                //    new SearchValueData {Name = "LoggedInID",Value = Convert.ToString(SessionHelper.LoggedInID)}
                });
                if (data == 1)
                {
                    response.Data = data;
                    response.IsSuccess = true;
                    response.Message = "Hours Saved Successfully";
                }
                else
                {

                    response.Message = "Hours not Saved Successfully";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }




        public ServiceResponse HC_GetEmployeeByUserName(string username)
        {
            var serviceResponse = new ServiceResponse();
            List<SearchValueData> paramList = new List<SearchValueData>
            {
                new SearchValueData { Name = "UserName" , Value = username},
            };

            Employee model = GetEntity<Employee>(StoredProcedure.HC_GetEmployeeByUserName, paramList);
            serviceResponse.Data = model;
            serviceResponse.IsSuccess = true;
            return serviceResponse;
        }

        public ServiceResponse HC_ClockInOut(long employeeID)
        {
            var serviceResponse = new ServiceResponse();
            var model = GetSaveClockInOutModel();
            serviceResponse.Data = model;
            serviceResponse.IsSuccess = true;
            return serviceResponse;
        }
        public SaveClockInOutModel GetSaveClockInOutModel()
        {
            List<SearchValueData> paramList = new List<SearchValueData>
            {
                new SearchValueData { Name = "EmployeeID" , Value = SessionHelper.LoggedInID.ToString()},
            };

            SaveClockInOutModel model = GetMultipleEntity<SaveClockInOutModel>(StoredProcedure.HC_ClockInOut, paramList);
            return model;
        }
        public int SaveEmployeeAttendanceDetail(EmployeeAttendanceDetail AttendanceDetailModel)
        {
            var dataList = new List<SearchValueData>();
            int data = 0;
            dataList = new List<SearchValueData>();
            if (AttendanceDetailModel.Id != null)
            {
                dataList.Add(new SearchValueData
                {
                    Name = "Id",
                    Value = Convert.ToString(AttendanceDetailModel.Id)
                });
            }
            dataList.Add(new SearchValueData
            {
                Name = "AttendanceMasterId",
                Value = Convert.ToString(AttendanceDetailModel.AttendanceMasterId)
            });
            dataList.Add(new SearchValueData
            {
                Name = "Type",
                Value = Convert.ToString(AttendanceDetailModel.Type)
            });
            dataList.Add(new SearchValueData
            {
                Name = "TypeDetail",
                Value = Convert.ToString(AttendanceDetailModel.TypeDetail)
            });
            dataList.Add(new SearchValueData
            {
                Name = "Note",
                Value = Convert.ToString(AttendanceDetailModel.Note)
            });

            data = (int)GetScalar(StoredProcedure.SaveEmployeeAttendanceDetail, dataList);
            return data;
        }
        public int SaveEmployeeAttendanceMaster(EmployeeAttendanceMaster AttendanceMasterModel)
        {
            var dataList = new List<SearchValueData>();
            int data = 0;
            dataList = new List<SearchValueData>();
            if (AttendanceMasterModel.Id != null)
            {
                dataList.Add(new SearchValueData
                {
                    Name = "Id",
                    Value = Convert.ToString(AttendanceMasterModel.Id)
                });
            }
            dataList.Add(new SearchValueData
            {
                Name = "EmployeeID",
                Value = Convert.ToString(SessionHelper.LoggedInID)
            });
            dataList.Add(new SearchValueData
            {
                Name = "WorkMinutes",
                Value = Convert.ToString(AttendanceMasterModel.WorkMinutes)
            });
            dataList.Add(new SearchValueData
            {
                Name = "FacilityID",
                Value = Convert.ToString(AttendanceMasterModel.FacilityID)
            });
            dataList.Add(new SearchValueData
            {
                Name = "OrganizationID",
                Value = Convert.ToString(SessionHelper.OrganizationId)
            });
            dataList.Add(new SearchValueData
            {
                Name = "Note",
                Value = Convert.ToString(AttendanceMasterModel.Note)
            });

            data = (int)GetScalar(StoredProcedure.SaveEmployeeAttendanceMaster, dataList);
            return data;
        }
        public ServiceResponse SaveClockInOut(SaveClockInOutModel saveClockInOutModel)
        {
            var response = new ServiceResponse();
            SaveClockInOutModel model2 = new SaveClockInOutModel();
            try
            {
                var dataList = new List<SearchValueData>();
                int data = 0;
                model2 = GetSaveClockInOutModel();
                if (saveClockInOutModel.EmployeeAttendanceMaster != null && saveClockInOutModel.EmployeeAttendanceMaster.Id == null)
                {
                    ///?Clock-in only
                    saveClockInOutModel.EmployeeAttendanceMaster.WorkMinutes = 0;

                    data = SaveEmployeeAttendanceMaster(saveClockInOutModel.EmployeeAttendanceMaster);
                    if (data == 1)
                    {
                        model2 = GetSaveClockInOutModel();
                        saveClockInOutModel.EmployeeAttendanceDetail[0].AttendanceMasterId = (int)model2.EmployeeAttendanceMaster.Id;
                        data = 0;
                        data = SaveEmployeeAttendanceDetail(saveClockInOutModel.EmployeeAttendanceDetail[0]);
                    }
                }
                else
                {
                    model2 = GetSaveClockInOutModel();
                    saveClockInOutModel.EmployeeAttendanceDetail[0].Note = saveClockInOutModel.EmployeeAttendanceMaster.Note;
                    var AttendanceDetail = saveClockInOutModel.EmployeeAttendanceDetail[0];

                    // Avoid exception if its first clock in
                    if (saveClockInOutModel.EmployeeAttendanceMaster == null)
                        saveClockInOutModel.EmployeeAttendanceMaster = new EmployeeAttendanceMaster();

                    saveClockInOutModel.EmployeeAttendanceMaster.WorkMinutes = 0;

                    if (AttendanceDetail.Type == (int)AttendanceDetailType.Break ||
                        AttendanceDetail.Type == (int)AttendanceDetailType.ClockOut
                        )
                    {
                        DateTime d1 = (DateTime)(model2.EmployeeAttendanceDetail
                            .Where(e =>
                                e.Type == (int)AttendanceDetailType.ClockIn
                                || e.Type == (int)AttendanceDetailType.Resume
                        ).OrderByDescending(e => e.CreatedDate).Select(e => e.CreatedDate).FirstOrDefault());
                        TimeSpan ts = (TimeZoneInfo.ConvertTime(Convert.ToDateTime(DateTime.Now),TimeZoneInfo.FindSystemTimeZoneById(saveClockInOutModel.TimeZone)))  - d1;
                        
                        saveClockInOutModel.EmployeeAttendanceMaster.WorkMinutes = (int)ts.TotalMinutes;
                    }

                    data = SaveEmployeeAttendanceMaster(saveClockInOutModel.EmployeeAttendanceMaster);
                    if (data == 1)
                    {
                        data = 0;
                        saveClockInOutModel.EmployeeAttendanceDetail[0].AttendanceMasterId = (int)GetSaveClockInOutModel().EmployeeAttendanceMaster.Id;

                        data = SaveEmployeeAttendanceDetail(saveClockInOutModel.EmployeeAttendanceDetail[0]);
                    }
                }
                if (data == 1)
                {
                    response.IsSuccess = true;
                    response.Message = string.Format(Resource.RecordCreatedSuccessfully, Resource.Clockinout);
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
        public ServiceResponse EmployeeAttendanceCalendar(SearchRefCalender model)
        {
            var response = new ServiceResponse();
            List<SearchValueData> paramList = new List<SearchValueData>
            {
                new SearchValueData { Name = "EmployeeID" , Value = model.EmployeeIDs},
                new SearchValueData { Name = "StartDate" , Value = model.StartDate.ToString()},
                new SearchValueData { Name = "EndDate" , Value = model.EndDate.ToString()},
            };

            EmployeeAttendanceCalendarModel returnModel = GetMultipleEntity<EmployeeAttendanceCalendarModel>(StoredProcedure.Get_EmployeeAttendanceCalendar, paramList);
            response.Data = returnModel;
            response.IsSuccess = true;
            return response;
        }
    }
}