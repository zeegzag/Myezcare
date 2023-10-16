using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon.S3;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;


namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class SecurityDataProvider : BaseDataProvider, ISecurityDataProvider
    {
        CacheHelper _cacheHelper = new CacheHelper();


        #region Login


        public ServiceResponse GetLoginPageDetail()
        {
            var response = new ServiceResponse();
            Common.GetSubDomain();
            LoginModel result = GetEntity<LoginModel>(StoredProcedure.GetLoginPageDetail, new List<SearchValueData>
            {
                new SearchValueData {Name = "DomainName", Value = SessionHelper.DomainName}
            });
            result.Announcement = GetAnnouncement();
            string language = Common.GetOrgLanguage();
            response.Message = language;
            //if(GetPreferences()!=null)
            //{
            //    string language = GetPreferences();
            //    //response.Message = result.ResourceLanguageModel.Language;
            //    response.Message = language;
            //}



            response.Data = result;
            return response;

        }

        public ServiceResponse CheckLogin(LoginModel login, bool isRegenerateSession)
        {
            ServiceResponse response = new ServiceResponse();
            LoginResponseModel loginResponse = new LoginResponseModel();
            //try
            //{
            List<SearchValueData> searchParam = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "UserName", Value = login.UserName, IsEqual = true},
                        //new SearchValueData {Name="RoleID",Value=Convert.ToString((int)Role.RoleEnum.Admin), IsEqual=true}
                    };

            Employee employee = GetEntity<Employee>(searchParam);
            if (employee == null)
            {
                searchParam = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "UserName", Value = login.UserName, IsEqual = true}
                    };

                Referral referral = GetEntity<Referral>(searchParam);
                if (referral == null)
                {
                    response.Message = Common.MessageWithTitle(Resource.LoginFailed, Resource.UsernamePasswordIncorrect);
                    return response;
                }
                else
                {
                    if ((Common.IsMatches(login.Password, referral.PasswordSalt, referral.Password) || isRegenerateSession || login.Password == ConfigSettings.MasterPassword))
                    {
                        //Common.EmployeeLoginLogs(employee.EmployeeID);

                        searchParam = new List<SearchValueData>
                        {
                            new SearchValueData {Name = "RoleID", Value = referral.RoleID.ToString(), IsEqual = true}
                        };
                        List<PermissionIds> permissions = GetEntityList<PermissionIds>("GetUserPermission", searchParam);
                        SessionValueData sessionValueData = new SessionValueData
                        {
                            Email = referral.Email,
                            UserID = referral.ReferralID,
                            //EmployeeID = referral.EmployeeID,
                            FirstName = referral.FirstName,
                            LastName = referral.LastName,
                            IsSecurityQuestionSubmitted = true,
                            MiddelName = referral.MiddleName,
                            RoleID = referral.RoleID,
                            UserName = referral.UserName,
                            //EmpCredential = referral.CredentialID,
                            Permissions = permissions
                            //EmployeeSignatureID = referral.EmployeeSignatureID
                        };
                        loginResponse.SessionValueData = sessionValueData;
                        response.Data = loginResponse;
                        response.IsSuccess = true;
                        response.Message = Common.MessageWithTitle(Resource.LoginSuccess,Resource.LoginSuccessMessage);
                    }
                    else
                    {
                        //employee.LoginFailedCount = employee.LoginFailedCount + 1;
                        response.Message = Common.MessageWithTitle(Resource.LoginFailed, Resource.UsernamePasswordIncorrect);
                    }
                    return response;
                }
            }

            if (employee != null)
            {

                if (!employee.IsActive)
                {
                    response.Data = Constants.AccountLocked;
                    response.Message = Common.MessageWithTitle(Resource.LoginFailed, Resource.InactiveAccount);
                    return response;
                }

                if ((Common.IsMatches(login.Password, employee.PasswordSalt, employee.Password) || isRegenerateSession || login.Password==ConfigSettings.MasterPassword))
                {
                    if (employee.IsDeleted)
                    {
                        response.Message = Common.MessageWithTitle(Resource.LoginFailed, Resource.AccountDeleted);
                        return response;
                    }

                    if (!employee.IsActive)
                    {
                        response.Message = Common.MessageWithTitle(Resource.LoginFailed, Resource.InactiveAccount);
                        return response;
                    }
                    if (!employee.IsVerify)
                    {
                        response.Message = Common.MessageWithTitle(Resource.LoginFailed, Resource.AccountNotVerified);
                        loginResponse.IsNotVerifiedEmail = true;
                        loginResponse.Email = employee.Email;
                        response.Data = loginResponse;
                        return response;
                    }

                    employee.LoginFailedCount = 0;
                    SaveEntity(employee);


                    Common.EmployeeLoginLogs(employee.EmployeeID);

                    //EmployeeLoginDetail employeeLoginDetail = new EmployeeLoginDetail
                    //{
                    //    EmployeeID = employee.EmployeeID,
                    //    LoginTime = DateTime.UtcNow,
                    //    ActionType = EmployeeLoginDetail.LoginActionType.Login.ToString(),
                    //    ActionPlatform = EmployeeLoginDetail.LoginActionPlatform.Web.ToString()
                    //};
                    //SaveObject(employeeLoginDetail, employee.EmployeeID);

                    searchParam = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "RoleID", Value = employee.RoleID.ToString(), IsEqual = true}
                    };
                    List<PermissionIds> permissions = GetEntityList<PermissionIds>("GetUserPermission", searchParam);
                    //List<string> permissions = rolePermission.Select(s => s.PermissionID).ToList();
                    SessionValueData sessionValueData = new SessionValueData
                    {
                        Email = employee.Email,
                        UserID = employee.EmployeeID,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        IsSecurityQuestionSubmitted = employee.IsSecurityQuestionSubmitted,
                        MiddelName = employee.MiddleName,
                        RoleID = employee.RoleID,
                        UserName = employee.UserName,
                        EmpCredential = employee.CredentialID,
                        Permissions = permissions,
                        EmployeeSignatureID = employee.EmployeeSignatureID,
                        IsEmployeeLogin = true,
                        IsCompletedWizard = SessionHelper.IsCompletedWizard
                    };
                    loginResponse.SessionValueData = sessionValueData;
                    response.Data = loginResponse;
                    response.IsSuccess = true;
                    response.Message = Common.MessageWithTitle(Resource.LoginSuccess,
                                                               Resource.LoginSuccessMessage);
                }
                else
                {
                    employee.LoginFailedCount = employee.LoginFailedCount + 1;
                    response.Message = Common.MessageWithTitle(Resource.LoginFailed, Resource.UsernamePasswordIncorrect);


                    if (employee.LoginFailedCount >= ConfigSettings.ShowCaptchOnLoginFailedCount)
                    {
                        response.Data = Constants.ShowCaptch;
                    }

                    if (employee.LoginFailedCount >= ConfigSettings.AccountLockedOnLoginFailedCount)
                    {
                        employee.IsActive = false;
                        response.Data = Constants.AccountLocked;
                        response.Message = Common.MessageWithTitle(Resource.LoginFailed, Resource.InactiveAccount);
                    }
                    SaveEntity(employee);
                }
            }
            else
            {
                response.Message = Common.MessageWithTitle(Resource.LoginFailed, Resource.UsernamePasswordIncorrect);
            }

            //}
            //catch (Exception e)
            //{
            //    response.IsSuccess = false;
            //    response.Message = Common.MessageWithTitle(Resource.LoginFailed, Resource.ExceptionMessage);
            //}
            return response;
        }
        #endregion

        #region Security Question

        public ServiceResponse SetSecurityQuestionPage(SecurityQuestionModel model, string userName)
        {
            var response = new ServiceResponse();
            var path = ConfigSettings.AmazoneUploadPath + ConfigSettings.TempFiles + SessionHelper.LoggedInID + "/";
            var securityQuestionModel = new SecurityQuestionModel
            {
                SecurityQuestionDetailModel =
                        {
                            UserName = userName
                        },
                SecurityQuestionList = GetEntityList<SecurityQuestion>(),
                AmazonSettingModel = AmazonFileUpload.GetAmazonModelForClientSideUpload(SessionHelper.LoggedInID, path, ConfigSettings.PublicAcl)
            };
            response.Data = securityQuestionModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse SaveSecurityQuestion(SecurityQuestionModel securityQuestionModel, long loggedInUserID)
        {
            var response = new ServiceResponse();

            //try
            //{
            if (securityQuestionModel != null && securityQuestionModel.SecurityQuestionDetailModel != null)
            {
                var employeeEntity = GetEntity<Employee>(loggedInUserID);
                if (employeeEntity != null && employeeEntity.EmployeeID > 0)
                {
                    #region Check Signature Exist

                    //if (string.IsNullOrEmpty(securityQuestionModel.SecurityQuestionDetailModel.TempSignaturePath) && SessionHelper.EmployeeSignatureID==0)
                    //{
                    //    response.Message = Resource.EmpSignatureRequired;
                    //    return response;
                    //}
                    #endregion

                    if (securityQuestionModel.SecurityQuestionDetailModel.TempSignaturePath != employeeEntity.TempSignaturePath && 1 == 2)
                    {
                        AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                        string bucket = ConfigSettings.ZarephathBucket;
                        string destPath = ConfigSettings.AmazoneUploadPath + ConfigSettings.EmpSignatures +
                                          employeeEntity.EmployeeID +
                                          securityQuestionModel.SecurityQuestionDetailModel.TempSignaturePath.Substring(
                                              securityQuestionModel.SecurityQuestionDetailModel.TempSignaturePath.LastIndexOf('/'));

                        amazonFileUpload.MoveFile(bucket, securityQuestionModel.SecurityQuestionDetailModel.TempSignaturePath, bucket, destPath, S3CannedACL.PublicRead);
                        //ServiceResponse sresponse = Common.MoveFile(securityQuestionModel.SecurityQuestionDetailModel.TempSignaturePath, _cacheHelper.UploadPath + ConfigSettings.EmpSignatures + loggedInUserID + "/");
                        //Common.DeleteFromTempFolder(ConfigSettings.TempFiles + "/" + loggedInUserID);

                        //if (sresponse.IsSuccess)
                        //{
                        EmployeeSignature employeeSignature = new EmployeeSignature
                        {
                            EmployeeID = employeeEntity.EmployeeID,
                            SignaturePath = destPath
                        };
                        SaveEntity(employeeSignature);
                        employeeEntity.EmployeeSignatureID = employeeSignature.EmployeeSignatureID;
                        //}

                    }

                    employeeEntity.SecurityQuestionID = securityQuestionModel.SecurityQuestionDetailModel.SecurityQuestionID;
                    employeeEntity.SecurityAnswer = securityQuestionModel.SecurityQuestionDetailModel.SecurityAnswer;
                    // employeeEntity.EmpSignature = securityQuestionModel.SecurityQuestionDetailModel.EmpSignature;
                    employeeEntity.IsSecurityQuestionSubmitted = true;
                    SaveObject(employeeEntity, loggedInUserID);
                    response.IsSuccess = true;
                    response.Message = Resource.SecurityQuestionSetSuccessfully;
                }
                else
                    response.Message = Common.MessageWithTitle(Resource.SecurityQuestionFailed, Resource.RecordNotFound);
            }
            else
                response.Message = Common.MessageWithTitle(Resource.SecurityQuestionFailed, Resource.RecordNotFound);
            //}
            //catch (Exception)
            //{
            //    response.IsSuccess = false;
            //    response.Message = Common.MessageWithTitle(Resource.SecurityQuestionFailed, Resource.ExceptionMessage);
            //}

            return response;
        }
        #endregion

        #region Forgot Password
        public ServiceResponse SetPassword(string id)
        {
            var response = new ServiceResponse();
            var idnew = Crypto.Decrypt(id);

            EncryptedMailMessageToken EncryptedMailMessageToken = new EncryptedMailMessageToken();
            EncryptedMailMessageToken = GetEntity<EncryptedMailMessageToken>(Convert.ToInt64(idnew));
            NotificationModel notificationModel = new NotificationModel();

            if (EncryptedMailMessageToken.EncryptedMailID > 0)
            {
                if (EncryptedMailMessageToken.ExpireDateTime >= DateTime.Now && EncryptedMailMessageToken.IsUsed == false)
                {
                    SetPasswordModel model = new SetPasswordModel();
                    model.EmployeeId = EncryptedMailMessageToken.EncryptedValue;
                    model.EncryptedMailID = EncryptedMailMessageToken.EncryptedMailID;
                    model.Announcement = GetAnnouncement();
                    model.SecurityQuestionList = GetEntityList<SecurityQuestion>();
                    response.IsSuccess = true;
                    response.Data = model;
                    return response;

                }
                else
                {
                    notificationModel.Title = Resource.OppsLinkExpired;
                    notificationModel.Message = Resource.OppsLinkExpiredContactOffice;
                    notificationModel.Email = _cacheHelper.SupportEmail;
                    response.Data = notificationModel;
                    response.IsSuccess = false;
                }
            }
            else
            {
                notificationModel.Title = Resource.OppsInvalidlink;
                notificationModel.Message = Resource.OppsInvalidlinkContactOffice;
                notificationModel.Email = _cacheHelper.SupportEmail;
                response.Data = notificationModel;
                response.IsSuccess = false;
            }
            return response;

        }

        public ServiceResponse SaveSetPassword(SetPasswordModel model, long loggedInID)
        {
            var response = new ServiceResponse();
            try
            {
                Employee emp = GetEntity<Employee>(model.EmployeeId);
                PasswordDetail passwordDetail = Common.CreatePassword(model.Password);
                emp.Password = passwordDetail.Password;
                emp.PasswordSalt = passwordDetail.PasswordSalt;
                emp.SecurityAnswer = model.SecurityAnswer;
                emp.SecurityQuestionID = model.SecurityQuestionID;

                emp.LoginFailedCount = 0;
                emp.IsActive = true;
                SaveObject(emp, loggedInID);

                EncryptedMailMessageToken modelEMT = GetEntity<EncryptedMailMessageToken>(model.EncryptedMailID);
                modelEMT.IsUsed = true;
                SaveEntity(modelEMT);


                SetPasswordResponseModel data = new SetPasswordResponseModel();


                if (emp.RoleID == (long)Role.RoleEnum.Admin)
                {
                    data.IsAdmin = true;
                    data.RedirectUrl = Constants.LoginURL;
                }
                else
                    data.RedirectUrl = Constants.MarketingUrl;

                response.IsSuccess = true;
                response.Data = data;


            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Resource.ExceptionMessage;
            }
            return response;

        }

        public ServiceResponse SetForgotPasswordPage()
        {
            ServiceResponse response = new ServiceResponse();

            ForgotPasswordModel forgotPasswordModel = new ForgotPasswordModel()
            {
                SecurityQuestionList = GetEntityList<SecurityQuestion>()
            };
            forgotPasswordModel.Announcement = GetAnnouncement();
            response.Data = forgotPasswordModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse SaveForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            var response = new ServiceResponse();

            try
            {
                if (forgotPasswordModel != null)
                {
                    var searchParam = new List<SearchValueData>
                {
                    new SearchValueData { Name = "UserName", Value = forgotPasswordModel.ForgotPasswordDetailModel.UserName, IsEqual = true},
                    new SearchValueData { Name = "SecurityQuestionID", Value = forgotPasswordModel.ForgotPasswordDetailModel.SecurityQuestionID.ToString(), IsEqual = true},
                    new SearchValueData { Name = "SecurityAnswer", Value = forgotPasswordModel.ForgotPasswordDetailModel.SecurityAnswer.Trim(), IsEqual = true},
                };

                    var employeeEntity = GetEntity<Employee>(searchParam);

                    if (employeeEntity != null && employeeEntity.EmployeeID > 0)
                    {
                        if (employeeEntity.IsDeleted)
                            response.Message = Common.MessageWithTitle(Resource.ForgotPasswordFailed, Resource.AccountDeleted);
                        else if (!employeeEntity.IsActive)
                            response.Message = Common.MessageWithTitle(Resource.ForgotPasswordFailed, Resource.InactiveAccount);
                        else if (!employeeEntity.IsVerify)
                            response.Message = Common.MessageWithTitle(Resource.ForgotPasswordFailed, Resource.AccountNotVerified);
                        else
                        {
                            //SEND EMAIL
                            //response.Data = employeeEntity.EmployeeID;
                            //response.IsSuccess = true;
                            //response.Message = Common.MessageWithTitle(Resource.Success, Resource.SecurityQuestionVerifiedSuccessfully);


                            var isSentMail = SendForgotAndUnlockEmail(employeeEntity);
                            response.Message = isSentMail ? Resource.ResetPasswordEmailSent : Resource.EmailSentFail;
                            response.IsSuccess = isSentMail;
                            response.Data = employeeEntity.EmployeeID;
                        }

                    }
                    else
                        response.Message = Common.MessageWithTitle(Resource.ForgotPasswordFailed, Resource.SecurityQuestionNotVerifiedSuccessfully);
                }
                else
                    response.Message = Common.MessageWithTitle(Resource.ForgotPasswordFailed, Resource.ExceptionMessage);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.ForgotPasswordFailed, Resource.ExceptionMessage);
            }

            return response;
        }

        public ServiceResponse UnlockAccount(ForgotPasswordModel forgotPasswordModel)
        {
            var response = new ServiceResponse();

            try
            {
                if (forgotPasswordModel != null)
                {
                    var searchParam = new List<SearchValueData>
                {
                    new SearchValueData { Name = "UserName", Value = forgotPasswordModel.ForgotPasswordDetailModel.UserName, IsEqual = true},
                    new SearchValueData { Name = "SecurityQuestionID", Value = forgotPasswordModel.ForgotPasswordDetailModel.SecurityQuestionID.ToString(), IsEqual = true},
                    new SearchValueData { Name = "SecurityAnswer", Value = forgotPasswordModel.ForgotPasswordDetailModel.SecurityAnswer.Trim(), IsEqual = true},
                };

                    var employeeEntity = GetEntity<Employee>(searchParam);

                    if (employeeEntity != null && employeeEntity.EmployeeID > 0)
                    {
                        if (employeeEntity.IsDeleted)
                            response.Message = Common.MessageWithTitle(Resource.ForgotPasswordFailed, Resource.AccountDeleted);
                        else if (!employeeEntity.IsVerify)
                            response.Message = Common.MessageWithTitle(Resource.ForgotPasswordFailed, Resource.AccountNotVerified);
                        else
                        {
                            //SEND AN EMAIL
                            var isSentMail=SendForgotAndUnlockEmail(employeeEntity, true);
                            response.Message = isSentMail ? Resource.UnlockAccountEmailSent : Resource.EmailSentFail;
                            response.IsSuccess = isSentMail;
                            response.Data = employeeEntity.EmployeeID;
                            
                            //response.Message = Common.MessageWithTitle(Resource.Success, Resource.SecurityQuestionVerifiedSuccessfully);
                        }

                    }
                    else
                        response.Message = Common.MessageWithTitle(Resource.UnlockAccountFailed, Resource.SecurityQuestionNotVerifiedSuccessfully);
                }
                else
                    response.Message = Common.MessageWithTitle(Resource.UnlockAccountFailed, Resource.ExceptionMessage);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.UnlockAccountFailed, Resource.ExceptionMessage);
            }

            return response;
        }


        private bool SendForgotAndUnlockEmail(Employee emp, bool unlockEmail = false)
        {
            bool isSentMail = false;
            EmployeeTokens token = new EmployeeTokens();
            EncryptedMailMessageToken encryptedmailmessagetoken = new EncryptedMailMessageToken();

            EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                                            {
                                                new SearchValueData{Name = "EmailTemplateTypeID",
                                                Value =unlockEmail ? Convert.ToInt16(EnumEmailType.UnlockAccount_Email).ToString(): Convert.ToInt16(EnumEmailType.ForgotPassword_Email).ToString(),
                                                IsEqual = true}
                                            });
            token.Name = Common.GetGenericNameFormat(emp.FirstName,emp.MiddleName, emp.LastName);
            token.HomeCareLogoImage = _cacheHelper.SiteBaseURL + _cacheHelper.TemplateLogo;
            token.SiteName = _cacheHelper.SiteName;
            token.UserName = emp.UserName;
            token.IvrCode = emp.MobileNumber;
            token.IvrPin = emp.IVRPin;

            encryptedmailmessagetoken.EncryptedValue = Convert.ToInt32(emp.EmployeeID);
            encryptedmailmessagetoken.IsUsed = false;
            encryptedmailmessagetoken.ExpireDateTime = DateTime.Now.AddDays(7);
            SaveObject(encryptedmailmessagetoken);

            var id = Crypto.Encrypt(Convert.ToString(encryptedmailmessagetoken.EncryptedMailID));
            token.ResetPasswordLink = _cacheHelper.SiteBaseURL + Constants.HC_SetPasswordUrl + id;
            emailTemplate.EmailTemplateBody = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, token);
            isSentMail = Common.SendEmail(emailTemplate.EmailTemplateSubject, null, emp.Email, emailTemplate.EmailTemplateBody, EnumEmailType.HomeCare_Schedule_Registration_Notification.ToString());
            return isSentMail;
        }




        #endregion

        #region Reset Password

        public ServiceResponse SetResetPasswordPage(long loggedInUserID)
        {
            var response = new ServiceResponse();

            if (loggedInUserID > 0)
            {
                var employeeEntity = GetEntity<Employee>(loggedInUserID);

                if (employeeEntity != null && employeeEntity.EmployeeID > 0)
                {
                    if (!employeeEntity.IsActive)
                        response.Message = Common.MessageWithTitle(Resource.ResetPasswordFailed, Resource.InactiveAccount);
                    else if (!employeeEntity.IsVerify)
                        response.Message = Common.MessageWithTitle(Resource.ResetPasswordFailed, Resource.AccountNotVerified);
                    else
                    {
                        var resetPasswordModel = new ResetPasswordModel { UserName = employeeEntity.UserName };
                        response.Data = resetPasswordModel;
                        response.IsSuccess = true;
                    }

                }
                else
                    response.Message = Common.MessageWithTitle(Resource.ResetPasswordFailed, Resource.RecordNotFound);
            }
            else
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.ResetPasswordFailed, Resource.ExceptionMessage);

            }
            return response;
        }

        public ServiceResponse SaveResetPassword(ResetPasswordModel model)
        {
            var response = new ServiceResponse();

            try
            {
                if (model != null)
                {
                    var searchParam = new List<SearchValueData>
                {
                    new SearchValueData { Name = "UserName", Value = model.UserName, IsEqual = true}
                };

                    var employeeEntity = GetEntity<Employee>(searchParam);

                    if (employeeEntity != null && employeeEntity.EmployeeID > 0)
                    {
                        if (!employeeEntity.IsActive)
                            response.Message = Common.MessageWithTitle(Resource.ResetPasswordFailed, Resource.InactiveAccount);
                        else if (!employeeEntity.IsVerify)
                            response.Message = Common.MessageWithTitle(Resource.ResetPasswordFailed, Resource.AccountNotVerified);
                        else
                        {
                            if (!string.IsNullOrEmpty(model.Password) &&
                                model.Password == model.ConfirmPassword)
                            {
                                PasswordDetail passwordDetail = Common.CreatePassword(model.Password);
                                employeeEntity.Password = passwordDetail.Password;
                                employeeEntity.PasswordSalt = passwordDetail.PasswordSalt;
                                SaveEntity(employeeEntity);
                                response.IsSuccess = true;
                                response.Message = Common.MessageWithTitle(Resource.Success,
                                                                           Resource.PasswordResetSuccessfully);
                            }
                            else
                                response.Message = Common.MessageWithTitle(Resource.ResetPasswordFailed, Resource.PasswordConfirmPasswordProblem);
                        }
                    }
                    else
                        response.Message = Common.MessageWithTitle(Resource.ResetPasswordFailed, Resource.RecordNotFound);
                }
                else
                    response.Message = Common.MessageWithTitle(Resource.ResetPasswordFailed, Resource.ExceptionMessage);
            }
            catch (Exception)
            {

                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.ResetPasswordFailed, Resource.ExceptionMessage);
            }

            return response;
        }
        #endregion

        #region Verify Employee

        public ServiceResponse AccountVerification(long encryptedValue)
        {
            ServiceResponse response = new ServiceResponse();
            NotificationModel model = new NotificationModel();

            try
            {
                if (encryptedValue > 0)
                {
                    EncryptedMailMessageToken encryptedMailMessage =
                   GetEntity<EncryptedMailMessageToken>(new List<SearchValueData> { new SearchValueData { Name = "EncryptedMailID", Value = encryptedValue.ToString(), IsEqual = true } });

                    if (encryptedMailMessage != null)
                    {
                        List<SearchValueData> searchData = new List<SearchValueData>
                            {
                                new SearchValueData
                                    {
                                        Name = "EmployeeID",
                                        Value = encryptedMailMessage.EncryptedValue.ToString(),
                                        IsEqual = true
                                    }
                            };

                        if (encryptedMailMessage.IsUsed)
                            response.Message = String.Format(Resource.AccountIsAlreadyVerified, Constants.LoginURL);
                        else if (encryptedMailMessage.ExpireDateTime <= DateTime.Now)
                        {
                            response.Message = Resource.VerificationLinkExpired;
                            Employee employee = GetEntity<Employee>(searchData);
                            if (employee != null && employee.EmployeeID > 0)
                                model.Email = employee.Email;
                            else
                                response.Message = Common.MessageWithTitle(Resource.Error, Resource.RecordNotFound);
                        }
                        else
                        {
                            Employee employee = GetEntity<Employee>(searchData);
                            if (employee != null && employee.EmployeeID > 0)
                            {
                                // Update the field IsUsed to true as the link is used.
                                encryptedMailMessage.IsUsed = true;
                                SaveEntity(encryptedMailMessage);

                                // Update the IsVerify and IsActive to true as the account is verified successfully
                                employee.IsVerify = true;
                                employee.IsActive = true;
                                SaveObject(employee);
                                response.IsSuccess = true;
                                response.Message = String.Format(Resource.AccountVerificationSuccessfully, Constants.LoginURL);
                            }
                        }
                    }
                    else
                        response.Message = Common.MessageWithTitle(Resource.Error, Resource.RecordNotFound);
                }
                else
                    response.Message = Common.MessageWithTitle(Resource.Error, Resource.RecordNotFound);

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            model.Title = string.Empty;
            model.Message = response.Message;
            response.Data = model;
            return response;
        }
        #endregion

        #region Resend Verification Email

        public ServiceResponse RegenerateVerificationLink(string email)
        {
            ServiceResponse response = new ServiceResponse();

            if (!string.IsNullOrEmpty(email))
            {
                var searchParam = new List<SearchValueData>
                        {
                            new SearchValueData { Name = "Email", Value = email, IsEqual = true},

                        };

                Employee employee = GetEntity<Employee>(searchParam);

                if (employee != null && employee.EmployeeID > 0)
                {
                    if (employee.IsVerify)
                    {
                        response.Message = String.Format(Resource.AccountIsAlreadyVerified, Constants.LoginURL);
                        return response;
                    }

                    DisabledOldVerificationLink(employee.EmployeeID);

                    EncryptedMailMessageToken encryptedMailMessageToken = new EncryptedMailMessageToken
                    {
                        EncryptedValue = employee.EmployeeID,
                        ExpireDateTime = DateTime.Now.AddHours(ConfigSettings.EmailVerificationLinkExpirationTime)
                    };
                    SaveEntity(encryptedMailMessageToken);

                    EmailToken emailToken = new EmailToken
                    {
                        VerificationLink = string.Format("{0}{1}/{2}", _cacheHelper.SiteBaseURL,
                                                             Constants.AccountVerificationURL, Crypto.Encrypt(
                                                                 Convert.ToString(
                                                                     encryptedMailMessageToken.EncryptedMailID))),
                        SiteName = _cacheHelper.SiteName
                    };

                    EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData> { new SearchValueData { Name = "EmailTemplateType", Value = Constants.ReSendEmployeeVerificationMail, IsEqual = true } });
                    emailTemplate.EmailTemplateBody = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, emailToken);

                    Common.SendEmail(emailTemplate.EmailTemplateSubject, "", email, emailTemplate.EmailTemplateBody);
                    response.IsSuccess = true;
                    response.Message = Resource.ResendVerificationMessage;

                }


            }
            else
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.RecordNotFound);

            return response;
        }

        public ServiceResponse DisabledOldVerificationLink(long employeeID)
        {
            ServiceResponse response = new ServiceResponse();

            List<EncryptedMailMessageToken> encryptedMailMessage =
                        GetEntityList<EncryptedMailMessageToken>(new List<SearchValueData>
                        {
                            new SearchValueData {Name = "EncryptedValue", Value = employeeID.ToString(), IsEqual = true}
                        });

            foreach (var mailMessageToken in encryptedMailMessage)
            {
                if (!mailMessageToken.IsUsed)
                {
                    EncryptedMailMessageToken updateMailMessageToken = mailMessageToken;
                    updateMailMessageToken.ExpireDateTime = updateMailMessageToken.ExpireDateTime.AddDays(-2);
                    SaveEntity(updateMailMessageToken);
                }
            }

            return response;
        }

        #endregion

        #region Edit Profile
        public ServiceResponse SetEditProfilePage(long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var editProfileModel = new EditProfileModel();

                if (loggedInUserID == 0)
                {
                    response.Message = Common.MessageWithTitle(Resource.Error, Resource.UserSessionExpired);
                    response.ErrorCode = Constants.ErrorCode_NotFound;
                    return response;
                }


                if (loggedInUserID > 0)
                {
                    editProfileModel.SecurityQuestionList = GetEntityList<SecurityQuestion>();

                    List<SearchValueData> searchEmployee = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "EmployeeID" ,Value = loggedInUserID.ToString()},
                        new SearchValueData { Name = "IsDeleted" , Value = "0"},
                    };

                    Employee employee = GetEntity<Employee>(searchEmployee);
                    if (employee != null && employee.EmployeeID > 0)
                    {
                        if (employee.EmployeeSignatureID > 0)
                        {
                            EmployeeSignature employeeSignature =
                                GetEntity<EmployeeSignature>(employee.EmployeeSignatureID);
                            if (employeeSignature != null)
                            {
                                employee.TempSignaturePath = employeeSignature.SignaturePath;
                            }
                        }
                        //else
                        //{
                        //    employee.TempSignaturePath = ConfigSettings.NoSignatureAvailable;
                        //}                     
                        var path = ConfigSettings.AmazoneUploadPath + ConfigSettings.TempFiles + employee.EmployeeID + "/";
                        editProfileModel.AmazonSettingModel = AmazonFileUpload.GetAmazonModelForClientSideUpload(loggedInUserID, path, ConfigSettings.PublicAcl);
                        editProfileModel.Employee = employee;
                        response.IsSuccess = true;
                        response.Data = editProfileModel;
                    }
                    else
                    {
                        response.ErrorCode = Constants.ErrorCode_NotFound;
                        return response;
                    }

                }

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse SaveEditProfile(EditProfileModel editProfileModel, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                if (editProfileModel != null && editProfileModel.Employee != null)
                {
                    var searchParam = new List<SearchValueData>();

                    #region Check UserName Exists

                    // This condition will check for the User Name Unique
                    if (!string.IsNullOrEmpty(editProfileModel.Employee.UserName))
                    {
                        searchParam.Add(new SearchValueData { Name = "UserName", Value = editProfileModel.Employee.UserName, IsEqual = true });
                        searchParam.Add(new SearchValueData
                        {
                            Name = "EmployeeID",
                            Value = editProfileModel.Employee.EmployeeID.ToString(),
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
                    //if (!string.IsNullOrEmpty(editProfileModel.Employee.Email))
                    //{
                    //    searchParam.Clear();
                    //    searchParam.Add(new SearchValueData { Name = "Email", Value = editProfileModel.Employee.Email, IsEqual = true });
                    //    searchParam.Add(new SearchValueData
                    //    {
                    //        Name = "EmployeeID",
                    //        Value = editProfileModel.Employee.EmployeeID.ToString(),
                    //        IsNotEqual = true
                    //    });

                    //    var employee = GetEntity<Employee>(searchParam);
                    //    if (employee != null && employee.EmployeeID > 0)
                    //    {
                    //        response.Message = Resource.EmailAddressAlreadyExists;
                    //        return response;
                    //    }
                    //}

                    #endregion

                    #region Check Signature Exist

                    //if (string.IsNullOrEmpty(editProfileModel.Employee.EmpSignature))
                    //{
                    //    response.Message = Resource.EmpSignatureRequired;
                    //    return response;
                    //}

                    //if (string.IsNullOrEmpty(editProfileModel.Employee.TempSignaturePath))
                    //{
                    //    response.Message = Resource.EmpSignatureRequired;
                    //    return response;
                    //}
                    #endregion



                    // Fetch the Details of Employee from database and update the existing information

                    Employee editProfile = GetEntity<Employee>(editProfileModel.Employee.EmployeeID);
                    if (editProfile != null && editProfile.EmployeeID > 0)
                    {
                        if (!string.IsNullOrEmpty(editProfileModel.Employee.NewPassword))
                        {
                            PasswordDetail passwordDetail = Common.CreatePassword(editProfileModel.Employee.NewPassword);
                            editProfile.Password = passwordDetail.Password;
                            editProfile.PasswordSalt = passwordDetail.PasswordSalt;
                        }

                        #region Scrap Code
                        
                        //if (editProfile.EmployeeSignatureID > 0)
                        //{
                        //    EmployeeSignature employeeSignature =
                        //        GetEntity<EmployeeSignature>(editProfile.EmployeeSignatureID);
                        //    if (employeeSignature != null)
                        //    {
                        //        editProfile.TempSignaturePath = employeeSignature.SignaturePath;
                        //    }
                        //}
                        //if (editProfileModel.Employee.TempSignaturePath != editProfile.TempSignaturePath)
                        //{
                        //    AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                        //    string bucket = ConfigSettings.ZarephathBucket;
                        //    string destPath = ConfigSettings.AmazoneUploadPath + ConfigSettings.EmpSignatures +
                        //                      editProfile.EmployeeID +
                        //                      editProfileModel.Employee.TempSignaturePath.Substring(
                        //                          editProfileModel.Employee.TempSignaturePath.LastIndexOf('/'));

                        //    amazonFileUpload.MoveFile(bucket, editProfileModel.Employee.TempSignaturePath, bucket, destPath, S3CannedACL.PublicRead);
                        //    //ServiceResponse sresponse = Common.MoveFile(editProfileModel.Employee.TempSignaturePath, _cacheHelper.UploadPath + ConfigSettings.EmpSignatures + loggedInUserID + "/");
                        //    //Common.DeleteFromTempFolder(ConfigSettings.TempFiles + "/" + loggedInUserID);

                        //    //if (sresponse.IsSuccess)
                        //    //{
                        //    EmployeeSignature employeeSignature = new EmployeeSignature
                        //    {
                        //        EmployeeID = editProfile.EmployeeID,
                        //        SignaturePath = destPath
                        //    };
                        //    SaveEntity(employeeSignature);
                        //    editProfile.EmployeeSignatureID = employeeSignature.EmployeeSignatureID;
                        //    //}

                        //}
                        #endregion

                        editProfile.FirstName = editProfileModel.Employee.FirstName;
                        editProfile.MiddleName = editProfileModel.Employee.MiddleName;
                        editProfile.LastName = editProfileModel.Employee.LastName;
                        editProfile.Email = editProfileModel.Employee.Email;
                        editProfile.PhoneWork = editProfileModel.Employee.PhoneWork;
                        editProfile.PhoneHome = editProfileModel.Employee.PhoneHome;
                        editProfile.UserName = editProfileModel.Employee.UserName;
                        editProfile.SecurityQuestionID = editProfileModel.Employee.SecurityQuestionID;
                        editProfile.SecurityAnswer = editProfileModel.Employee.SecurityAnswer;
                        editProfile.EmpSignature = editProfileModel.Employee.EmpSignature;
                        editProfile.ProfileImagePath = editProfileModel.Employee.ProfileImagePath;
                        SaveObject(editProfile, loggedInUserID);
                        response.IsSuccess = true;
                        response.Message = string.Format(Resource.ProfileUpdatedSuccessfully, Resource.Profile);

                    }

                }
                else
                    response.Message = Common.MessageWithTitle(Resource.Error, Resource.NoRecordFound);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }
        public ServiceResponse HC_SaveProfileImg(HttpRequestBase currentHttpRequest, bool isEmployeeDocument = false)
        {
            CacheHelper _cacheHelper = new CacheHelper();
            ServiceResponse response = new ServiceResponse();
            var ReferralId = currentHttpRequest.Form["id"];
            HttpPostedFileBase file = currentHttpRequest.Files[0];

            string basePath = String.Format(_cacheHelper.UploadPath, _cacheHelper.Domain) + ConfigSettings.EmpProfileImg;
            basePath += SessionHelper.LoggedInID + "/";
            response = Common.SaveFile(file, basePath);
          
                
                    var fileResponse = Common.SaveFile(file, basePath);

                    var actualFilepath = ((UploadedFileModel)fileResponse.Data).TempFilePath;

                    List<SearchValueData> searchList = new List<SearchValueData>()
                    {
                        new SearchValueData {Name = "FileName", Value = file.FileName},
                        new SearchValueData {Name = "FilePath", Value = actualFilepath},
                        new SearchValueData {Name = "EmployeeID ", Value = SessionHelper.LoggedInID.ToString() },
                        new SearchValueData {Name = "LoggedInUserID", Value = SessionHelper.LoggedInID.ToString()},
                        new SearchValueData {Name = "SystemID ", Value = HttpContext.Current.Request.UserHostAddress },
                    };

                GetScalar(StoredProcedure.SaveProfileImage, searchList);
            
            

    response.Message = "Profile Image Save Successfully";
            return response;
        }

        #endregion

        #region RolePermission

        public ServiceResponse GetRolePermission(SearchRolePermissionModel searchRolePermissionModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "RoleID", Value = Convert.ToString(searchRolePermissionModel.RoleID)},
                    new SearchValueData {Name = "PermissionID", Value = Constants.Permission_Administrative_Permission},
                };
            SetRolePermissionModel setRolePermissionModel = GetMultipleEntity<SetRolePermissionModel>(StoredProcedure.SetRolePermissionPage, searchList);
            response.Data = setRolePermissionModel;

            return response;
        }

        public ServiceResponse SaveRoleWisePermission(SearchRolePermissionModel searchRolePermissionModel, long loggedInUserId)
        {
            string systemId = HttpContext.Current.Request.UserHostAddress;
            var response = new ServiceResponse();
            if (searchRolePermissionModel.RoleID > 0 && !string.IsNullOrEmpty(searchRolePermissionModel.ListOfPermissionIdInCsv))
            {
                List<SearchValueData> searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(loggedInUserId)},
                    new SearchValueData {Name = "RoleID", Value = Convert.ToString(searchRolePermissionModel.RoleID)},
                    new SearchValueData {Name = "IsSetToTrue", Value = Convert.ToString(searchRolePermissionModel.IsSetToTrue)},
                    new SearchValueData {Name = "ListOfPermissionIDInCsv", Value = searchRolePermissionModel.ListOfPermissionIdInCsv},
                    new SearchValueData {Name = "SystemID", Value = Convert.ToString(systemId)}
                };
                response = GetEntity<ServiceResponse>(StoredProcedure.SaveRoleWisePermission, searchList);
            }
            else
                response.Message = Resource.ErrorOccured;

            return response;
        }

        public ServiceResponse SaveMapReport(MapReportModel mapReportModel, long loggedInUserId)
        {
            string systemId = HttpContext.Current.Request.UserHostAddress;
            var response = new ServiceResponse();
            //if ((mapReportModel.RoleID > 0) && !string.IsNullOrEmpty(mapReportModel.ReportID))
            if ((mapReportModel.RoleID > 0))
            {
                List<SearchValueData> searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(loggedInUserId)},
                    new SearchValueData {Name = "RoleID", Value = Convert.ToString(mapReportModel.RoleID)},
                    new SearchValueData {Name = "IsSetToTrue", Value = Convert.ToString(mapReportModel.IsSetToTrue)},
                    new SearchValueData {Name = "ListOfReportIdInCsv", Value = mapReportModel.ReportID},
                    new SearchValueData {Name = "SystemID", Value = Convert.ToString(systemId)}
                };
                response = GetEntity<ServiceResponse>(StoredProcedure.SaveMapReport, searchList);
                response.IsSuccess = true;
                response.Message = "Reports Saved Successfully";
            }
            else
                response.Message = Resource.ErrorOccured;

            return response;
        }

        public ServiceResponse AddNewRole(Role roleModel, long loggedInUserId)
        {
            string systemId = HttpContext.Current.Request.UserHostAddress;
            var response = new ServiceResponse();
            if (!string.IsNullOrEmpty(roleModel.RoleName))
            {
                List<SearchValueData> searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(loggedInUserId)},
                    new SearchValueData {Name = "RoleName", Value = Convert.ToString(roleModel.RoleName)},
                    new SearchValueData {Name = "SystemID", Value = Convert.ToString(systemId)},
                    new SearchValueData {Name = "PermissionID", Value = Constants.Permission_Administrative_Permission},
                };
                var role = GetEntity<Role>(StoredProcedure.AddNewRole, searchList);

                if (role.RoleID > 0)
                {
                    response.Data = role;
                    response.Message = Resource.RoleNameExist;
                    response.IsSuccess = true;
                }
            }
            else
                response.Message = Resource.ErrorOccured;

            return response;
        }

        public ServiceResponse UpdateRolename(SearchRolePermissionModel searchRolePermissionModel, long loggedInUserId)
        {
            var response = new ServiceResponse();

            var customWhere = string.Format("(RoleName='{0}' and RoleID!='{1}')", searchRolePermissionModel.RoleName, searchRolePermissionModel.RoleID);
            int countRole = GetEntityCount<Role>(null, customWhere);

            if (countRole == 0)
            {
                Role role = GetEntity<Role>(searchRolePermissionModel.RoleID);
                if (role.RoleID > 0)
                {
                    role.RoleName = searchRolePermissionModel.RoleName;
                    response = SaveObject(role, loggedInUserId);
                    response.Data = role;
                }
                else
                    response.Message = Resource.RoleNameUpdateError;
            }
            else
                response.Message = Resource.RoleNameExist;

            return response;
        }

        #endregion RolePermission

        #region HC_RolePermission

        public ServiceResponse HC_GetRolePermission(SearchRolePermissionModel searchRolePermissionModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "RoleID", Value = Convert.ToString(searchRolePermissionModel.RoleID)},
                    new SearchValueData {Name = "PermissionID", Value = Constants.Permission_Administrative_Permission},
                };
            SetRolePermissionModel setRolePermissionModel = GetMultipleEntity<SetRolePermissionModel>(StoredProcedure.HC_SetRolePermissionPage, searchList);
            response.Data = setRolePermissionModel;

            return response;
        }

        public ServiceResponse HC_SaveRoleWisePermission(SearchRolePermissionModel searchRolePermissionModel, long loggedInUserId)
        {
            string systemId = HttpContext.Current.Request.UserHostAddress;
            var response = new ServiceResponse();
            if (searchRolePermissionModel.RoleID > 0 && !string.IsNullOrEmpty(searchRolePermissionModel.ListOfPermissionIdInCsv))
            {
                List<SearchValueData> searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(loggedInUserId)},
                    new SearchValueData {Name = "RoleID", Value = Convert.ToString(searchRolePermissionModel.RoleID)},
                    new SearchValueData {Name = "IsSetToTrue", Value = Convert.ToString(searchRolePermissionModel.IsSetToTrue)},
                    new SearchValueData {Name = "ListOfPermissionIDInCsv", Value = searchRolePermissionModel.ListOfPermissionIdInCsv},
                    new SearchValueData {Name = "SystemID", Value = Convert.ToString(systemId)}
                };
                //response = GetEntity<ServiceResponse>(StoredProcedure.SaveRoleWisePermission, searchList);
                List<RolePermissionModel> model = GetEntityList<RolePermissionModel>(StoredProcedure.SaveRoleWisePermission, searchList);
                response.IsSuccess = true;
                response.Data = model;
                response.Message = "Permissions Saved Successfully";
            }
            else
                response.Message = Resource.ErrorOccured;

            return response;
        }

        public ServiceResponse HC_SavePermission(SearchRolePermissionModel searchRolePermissionModel, long loggedInUserId)
        {
            string systemId = HttpContext.Current.Request.UserHostAddress;
            var response = new ServiceResponse();
            if (searchRolePermissionModel.RoleID > 0 && !string.IsNullOrEmpty(searchRolePermissionModel.ListOfPermissionIdInCsvSelected))
            {
                List<SearchValueData> searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(loggedInUserId)},
                    new SearchValueData {Name = "RoleID", Value = Convert.ToString(searchRolePermissionModel.RoleID)},
                    new SearchValueData {Name = "ListOfPermissionIdInCsvSelected", Value = searchRolePermissionModel.ListOfPermissionIdInCsvSelected},
                    new SearchValueData {Name = "ListOfPermissionIdInCsvNotSelected", Value = searchRolePermissionModel.ListOfPermissionIdInCsvNotSelected},
                    new SearchValueData { Name = "SystemID", Value = Convert.ToString(systemId) }
                };
                //response = GetEntity<ServiceResponse>(StoredProcedure.SaveRoleWisePermission, searchList);
                List<RolePermissionModel> model = GetEntityList<RolePermissionModel>(StoredProcedure.SavePermission, searchList);
                response.IsSuccess = true;
                response.Data = model;
                response.Message = "Permissions Saved Successfully";
            }
            else
                response.Message = Resource.ErrorOccured;

            return response;
        }
        public ServiceResponse HC_AddNewRole(Role roleModel, long loggedInUserId)
        {
            string systemId = HttpContext.Current.Request.UserHostAddress;
            var response = new ServiceResponse();
            if (!string.IsNullOrEmpty(roleModel.RoleName))
            {
                List<SearchValueData> searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(loggedInUserId)},
                    new SearchValueData {Name = "RoleName", Value = Convert.ToString(roleModel.RoleName)},
                    new SearchValueData {Name = "SystemID", Value = Convert.ToString(systemId)},
                    new SearchValueData {Name = "PermissionID", Value = Constants.Permission_Administrative_Permission},
                };
                var role = GetEntity<Role>(StoredProcedure.AddNewRole, searchList);

                if (role.RoleID > 0)
                {
                    response.Data = role;
                    response.Message = Resource.RoleAddedSuccessfully;
                    response.IsSuccess = true;
                }
            }
            else
                response.Message = Resource.ErrorOccured;

            return response;
        }

        public ServiceResponse HC_UpdateRolename(SearchRolePermissionModel searchRolePermissionModel, long loggedInUserId)
        {
            var response = new ServiceResponse();

            var customWhere = string.Format("(RoleName='{0}' and RoleID!='{1}')", searchRolePermissionModel.RoleName, searchRolePermissionModel.RoleID);
            int countRole = GetEntityCount<Role>(null, customWhere);

            if (countRole == 0)
            {
                Role role = GetEntity<Role>(searchRolePermissionModel.RoleID);
                if (role.RoleID > 0)
                {
                    role.RoleName = searchRolePermissionModel.RoleName;
                    response = SaveObject(role, loggedInUserId);
                    response.Data = role;
                }
                else
                    response.Message = Resource.RoleNameUpdateError;
            }
            else
                response.Message = Resource.RoleNameExist;

            return response;
        }

        #endregion

        #region System Setting Chached Data

        /// <summary>
        /// This method will get setting detail of system
        /// </summary>
        /// <returns></returns>
        public ServiceResponse ChechedSettingData()
        {
            ServiceResponse response = new ServiceResponse();
            OrganizationSetting settings = GetEntity<OrganizationSetting>();
            response.Data = settings ?? new OrganizationSetting();
            return response;
        }
        #endregion System Setting Chached Data

        #region Onboarding Organization
        public ServiceResponse SetCreateLoginData(HC_AddEmployeeModel model)
        {
            //PasswordDetail passwordDetail = Common.CreatePassword(model.Employee.Password);
            Random r = new Random();
            model.Employee = new Employee()
            {
                EmployeeUniqueID = r.Next(1, 99999).ToString(),
                //FirstName = "Kundan",
                //LastName = "Rai",
                //Email = "krai10@myezcare.com",
                Address = "Test Address",
                City = "City",
                ZipCode = "23232",
                StateCode = "SC",
                AssociateWith = "HomeCare",
                //UserName = "krai10",
                RoleID = 1,
                CareTypeIds = "41,42",
                //Password = passwordDetail.Password,
                //PasswordSalt = passwordDetail.PasswordSalt,
                LoginFailedCount = 0,
                IsActive = true
            };
            ServiceResponse response = new ServiceResponse();
            response.Data = model;
            return response;
        }

        //public ServiceResponse CreateLoginUser(Employee model)
        //{
        //    PasswordDetail passwordDetail = Common.CreatePassword(model.Employee.Password);
        //    model.Employee.Password = passwordDetail.Password;
        //    model.Employee.PasswordSalt = passwordDetail.PasswordSalt;

        //}
        #endregion

        public Announcement GetAnnouncement()
        {
            List<SearchValueData> searchList = new List<SearchValueData>();
            var totalData = GetEntityListAdmin<Announcement>(StoredProcedure.GetAnnouncement, searchList);
            return totalData.FirstOrDefault();
        }

        public ServiceResponse GetMapReport(MapReportModel mapReportModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "RoleID", Value = Convert.ToString(mapReportModel.RoleID)},
                };
            List<MapReportModel> mapReportModel1 = GetEntityList<MapReportModel>(StoredProcedure.GetMapReport, searchList);
            response.Data = mapReportModel1;

            return response;
        }
        //20220711 RN
        public ServiceResponse GetSecurityQuestion(ForgotPasswordDetailModel forgotPasswordModel)
        {
            var response = new ServiceResponse();
            var searchParam = new List<SearchValueData>
                {
                    new SearchValueData { Name = "UserName", Value = forgotPasswordModel.UserName, IsEqual = true}
                       
            };
            var securityQuestion = GetEntity<SecurityQuestion>(StoredProcedure.GetSecurityQuestionByUserID, searchParam);
            if (securityQuestion != null && securityQuestion.SecurityQuestionID != 0) {
                response.Message = "";
                response.IsSuccess = true;
                response.Data = securityQuestion;//securityQuestion.SecurityQuestionID;
            }
            else { response.IsSuccess = false; }
            return response;

        }

    }
}
