using EDI_837_835_HCCP.Models;
using OopFactory.Edi835Parser.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.EdiValidation
{
    public class Common_Edi_837
    {
        public static void Validate_Edi_837(PayorEdi837Setting payorEdi837Setting, BatchRelatedAllDataModel item, ref int errorCount, ref string errorMessages)
        {
            #region Check Each Note Related Missing Informations


            //if (item.BatchTypeID == (int)EnumBatchType.Adjustment_Void_Replace_Submission)
            //{
            //    if (string.IsNullOrEmpty(item.Original_PayerClaimControlNumber))
            //    {
            //        {
            //            errorCount = errorCount + 1;
            //            string msg = string.Format("PayerClaimControlNumber is missing. It seems this note is not process anytime and we are trying to send it into Adjustment.");
            //            SetError(errorCount, ref errorMessages, msg);
            //        }
            //    }

            //}

            //if (item.PayorID != (long)Payor.PayorCode.PY && string.IsNullOrEmpty(item.CISNumber))
            //{
            //    errorCount = errorCount + 1;
            //    string msg = string.Format("Client's {0} is missing.",
            //                               string.IsNullOrEmpty(item.CISNumber) ? "CIS Number" : "");
            //    SetError(errorCount, ref errorMessages, msg);
            //}
            //else if (item.PayorID == (long)Payor.PayorCode.PY && string.IsNullOrEmpty(item.AHCCCSID))
            //{
            //    {
            //        errorCount = errorCount + 1;
            //        string msg = string.Format("Client's {0} is missing.",
            //                                   string.IsNullOrEmpty(item.AHCCCSID) ? "AHCCCS ID" : "");
            //        SetError(errorCount, ref errorMessages, msg);
            //    }
            //}

            if (string.IsNullOrEmpty(item.AHCCCSID))
            {
                {
                    errorCount = errorCount + 1;
                    string msg = "Client's Medicaid# is missing.";
                    SetError(errorCount, ref errorMessages, msg);
                }
            }


            if (string.IsNullOrEmpty(item.MedicalRecordNumber))
            {
                {
                    errorCount = errorCount + 1;
                    string msg = "Client's Medicaid# is missing.";
                    SetError(errorCount, ref errorMessages, msg);
                }
            }

            if (item.CalculatedAmount <=0)
            {
                {
                    errorCount = errorCount + 1;
                    string msg = "Service Line Items Amount is not valid. It should be greater than 0.";
                    SetError(errorCount, ref errorMessages, msg);
                }
            }

            //if (payorEdi837Setting.CheckForPolicyNumber && string.IsNullOrEmpty(item.PolicyNumber))
            //{
            //    {
            //        errorCount = errorCount + 1;
            //        string msg = string.Format("Client's {0} is missing.",
            //                                   string.IsNullOrEmpty(item.PolicyNumber) ? "Group / Policy Number" : "");
            //        SetError(errorCount, ref errorMessages, msg);
            //    }
            //}

            // CHECK PAYOR BILLING SETTING
            if (string.IsNullOrWhiteSpace(payorEdi837Setting.ISA06_InterchangeSenderId) ||
                string.IsNullOrWhiteSpace(payorEdi837Setting.ISA08_InterchangeReceiverId) ||
                string.IsNullOrWhiteSpace(payorEdi837Setting.GS02_ApplicationSenderCode) ||
                string.IsNullOrWhiteSpace(payorEdi837Setting.GS03_ApplicationReceiverCode) ||
                string.IsNullOrWhiteSpace(payorEdi837Setting.ISA11_RepetitionSeparator) ||
                string.IsNullOrWhiteSpace(payorEdi837Setting.ISA16_ComponentElementSeparator) ||
                string.IsNullOrWhiteSpace(payorEdi837Setting.Submitter_NM103_NameLastOrOrganizationName) ||
                string.IsNullOrWhiteSpace(payorEdi837Setting.Submitter_NM109_IdCodeQualifierEnum) ||
                string.IsNullOrWhiteSpace(payorEdi837Setting.Submitter_EDIContact1_PER02_Name) ||
                string.IsNullOrWhiteSpace(payorEdi837Setting.Submitter_EDIContact1_PER04_CommunicationNumber) ||
                string.IsNullOrWhiteSpace(payorEdi837Setting.Submitter_EDIContact1_PER08_CommunicationNumber) ||
                string.IsNullOrWhiteSpace(payorEdi837Setting.BillingProvider_PRV01_ProviderCode) ||
                string.IsNullOrWhiteSpace(payorEdi837Setting.BillingProvider_PRV02_ReferenceIdentificationQualifier) ||
                string.IsNullOrWhiteSpace(payorEdi837Setting.BillingProvider_PRV03_ProviderTaxonomyCode))
            {
                string msg = "Payor's {0} is missing.";
                errorCount = errorCount + 1;
                List<string> error = new List<string>();
                #region Add Error Message
                if (string.IsNullOrWhiteSpace(payorEdi837Setting.ISA06_InterchangeSenderId))
                {
                    error.Add("Interchange Sender Id");
                }
                if (string.IsNullOrWhiteSpace(payorEdi837Setting.ISA08_InterchangeReceiverId))
                {
                    error.Add("Interchange Receiver Id");
                }
                if (string.IsNullOrWhiteSpace(payorEdi837Setting.GS02_ApplicationSenderCode))
                {
                    error.Add("GS02 Application Sender Code");
                }
                if (string.IsNullOrWhiteSpace(payorEdi837Setting.GS03_ApplicationReceiverCode))
                {
                    error.Add("GS03 Application Receiver Code");
                }
                if (string.IsNullOrWhiteSpace(payorEdi837Setting.ISA11_RepetitionSeparator))
                {
                    error.Add("Repetition Separator");
                }
                if (string.IsNullOrWhiteSpace(payorEdi837Setting.ISA16_ComponentElementSeparator))
                {
                    error.Add("Element Separator");
                }
                if (string.IsNullOrWhiteSpace(payorEdi837Setting.Submitter_NM103_NameLastOrOrganizationName))
                {
                    error.Add("Sumbmitter Name");
                }
                if (string.IsNullOrWhiteSpace(payorEdi837Setting.Submitter_NM109_IdCodeQualifierEnum))
                {
                    error.Add("Sumbmitter Code Qualifier");
                }
                if (string.IsNullOrWhiteSpace(payorEdi837Setting.Submitter_EDIContact1_PER02_Name))
                {
                    error.Add("Sumbmitter EDI Contact");
                }
                if (string.IsNullOrWhiteSpace(payorEdi837Setting.Submitter_EDIContact1_PER04_CommunicationNumber))
                {
                    error.Add("Sumbmitter EDI Phone");
                }
                if (string.IsNullOrWhiteSpace(payorEdi837Setting.Submitter_EDIContact1_PER08_CommunicationNumber))
                {
                    error.Add("Sumbmitter EDI Email");
                }
                if (string.IsNullOrWhiteSpace(payorEdi837Setting.BillingProvider_PRV01_ProviderCode))
                {
                    error.Add("Provider Code");
                }
                if (string.IsNullOrWhiteSpace(payorEdi837Setting.BillingProvider_PRV02_ReferenceIdentificationQualifier))
                {
                    error.Add("Reference Identification Qualifier");
                }
                if (string.IsNullOrWhiteSpace(payorEdi837Setting.BillingProvider_PRV03_ProviderTaxonomyCode))
                {
                    error.Add("Provider Taxonomy Code");
                }
                #endregion
                msg = string.Format(msg, string.Join(Constants.Comma, error));
                SetError(errorCount, ref errorMessages, msg);
            }


            // CHECK CLIENT/SUBSCRIBER PHYSICIAN INFORMATION
            //if ((string.IsNullOrWhiteSpace(item.PhysicianNPINumber) || string.IsNullOrWhiteSpace(item.PhysicianFirstName) ||
            //    string.IsNullOrWhiteSpace(item.PhysicianLastName)) && item.IsCaseManagement == false)
            //{
            //    string msg = "Client's Physician {0} is missing.";
            //    errorCount = errorCount + 1;
            //    List<string> error = new List<string>();
            //    #region Add Error Message
            //    if (string.IsNullOrWhiteSpace(item.PhysicianNPINumber))
            //    {
            //        error.Add("NPI Number");
            //    }
            //    if (string.IsNullOrWhiteSpace(item.PhysicianFirstName))
            //    {
            //        error.Add("FirstName");
            //    }
            //    if (string.IsNullOrWhiteSpace(item.PhysicianLastName))
            //    {
            //        error.Add("LastName");
            //    }
            //    #endregion
            //    msg = string.Format(msg, string.Join(Constants.Comma, error));
            //    SetError(errorCount, ref errorMessages, msg);
            //}

            // CHECK CLIENT/SUBSCRIBER RELATED MISSSING INFORMATIONS
            if (string.IsNullOrEmpty(item.Dob) || string.IsNullOrEmpty(item.Gender) || string.IsNullOrEmpty(item.PatientAccountNumber)
                || string.IsNullOrEmpty(item.Address) || string.IsNullOrEmpty(item.City) || string.IsNullOrEmpty(item.State)
                || string.IsNullOrEmpty(item.ZipCode))
            {
                errorCount = errorCount + 1;
                string msg = string.Format("Client's {0}{1}{2}{3}{4}{5}{6} details are missing or legal guardian is not set.",
                                           string.IsNullOrEmpty(item.Dob) ? "date of birth," : "",
                                           string.IsNullOrEmpty(item.Gender) ? "gender," : "",
                                           string.IsNullOrEmpty(item.PatientAccountNumber) ? "patient account number," : "",
                                           string.IsNullOrEmpty(item.Address) ? "address," : "",
                                           string.IsNullOrEmpty(item.City) ? "city," : "",
                                           string.IsNullOrEmpty(item.State) ? "state," : "",
                                           string.IsNullOrEmpty(item.ZipCode) ? "zipcode" : "");
                SetError(errorCount, ref errorMessages, msg);

            }



            if (string.IsNullOrEmpty(item.ContinuedDX))
            {
                errorCount = errorCount + 1;
                SetError(errorCount, ref errorMessages, "Diagnosis codes details are missing.");
            }
            else
            {

                string dxCodeErrorMessage = "";
                string icd9dxCodeErrorMessage = "";
                var primaryDxCodeGroup = "";
                var primaryDxCodeType = "";
                var primaryDxCode = "";

                for (int i = 0; i < item.ContinuedDX.Split(',').Length; i++)
                {
                    var dXCodeDetails = item.ContinuedDX.Split(',')[i];
                    var dxCodeType = dXCodeDetails.Split(':').Length > 0 ? dXCodeDetails.Split(':')[0].Trim() : "";
                    var dxCode = dXCodeDetails.Split(':').Length > 1 ? dXCodeDetails.Split(':')[1] : dXCodeDetails;
                    var dxCodePrecedence = dXCodeDetails.Split(':').Length > 2 ? dXCodeDetails.Split(':')[2] : "";
                    var dxCodeGroup = dxCodeType == Constants.DXCodeType_ICD10_Primary ? Constants.DXCodeGroup_ICD10 : Constants.DXCodeGroup_ICD09;


                    if (dxCodeGroup == Constants.DXCodeGroup_ICD09)
                    {

                        if (string.IsNullOrEmpty(icd9dxCodeErrorMessage))
                            icd9dxCodeErrorMessage = string.Format("ICD-9 DX Code found. DX Codes {0}(#{1}), ", dxCode, dxCodeType);
                        else
                            icd9dxCodeErrorMessage = dxCodeErrorMessage + string.Format("{0}(#{1}), ", dxCode, dxCodeType);
                    }

                    var count = i + 1;
                    if (Convert.ToInt16(dxCodePrecedence) == 1)
                    {
                        // ABK:TX23403:ICD-10 
                        primaryDxCodeType = dxCodeType;
                        primaryDxCode = dxCode;
                        primaryDxCodeGroup = dxCodeGroup;
                    }

                    if (Convert.ToInt16(dxCodePrecedence) > 1)
                    {
                        var secondaryDxCodeType = dxCodeType;
                        var secondaryDxCode = dxCode;
                        var secondaryDxCodeGroup = dxCodeGroup;

                        if (primaryDxCodeType.Trim().ToLower() != secondaryDxCodeType.Trim().ToLower())
                        {
                            if (string.IsNullOrEmpty(dxCodeErrorMessage))
                                dxCodeErrorMessage = string.Format("Primary DX Code is {0}(#{1}) and its DX CODE Type is {2}. DX Codes {3}(#{4},{5}), ",
                                    primaryDxCode, primaryDxCodeType, primaryDxCodeGroup, secondaryDxCode, secondaryDxCodeType, secondaryDxCodeGroup);
                            else
                                dxCodeErrorMessage = dxCodeErrorMessage + string.Format("{0}(#{1},{2}), ", secondaryDxCode, secondaryDxCodeType, secondaryDxCodeGroup);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(dxCodeErrorMessage))
                {
                    dxCodeErrorMessage = dxCodeErrorMessage.Trim().TrimEnd(',') + string.Format(" would have same DX Code Type {0}.Update DX Codes details.DX Code details should be from same group.", primaryDxCodeGroup);
                    errorCount = errorCount + 1;
                    SetError(errorCount, ref errorMessages, dxCodeErrorMessage);
                }

                if (!string.IsNullOrEmpty(icd9dxCodeErrorMessage))
                {
                    icd9dxCodeErrorMessage = icd9dxCodeErrorMessage.Trim().TrimEnd(',');
                    errorCount = errorCount + 1;
                    SetError(errorCount, ref errorMessages, icd9dxCodeErrorMessage);
                }



            }

            // CHECK SERVICE RELATED MISSSING INFORMATIONS
            if (string.IsNullOrEmpty(item.ServiceCode) || string.IsNullOrEmpty(item.ServiceDateSpan)
                || string.IsNullOrEmpty(item.PosID.ToString()) || string.IsNullOrEmpty(item.CalculatedUnit.ToString())
                || string.IsNullOrEmpty(item.CalculatedAmount.ToString()))
            {
                errorCount = errorCount + 1;
                string msg = string.Format("Client's {0}{1}{2}{3}{4} details are missing.",
                                           string.IsNullOrEmpty(item.ServiceCode) ? "service code," : "",
                                           string.IsNullOrEmpty(item.ServiceDateSpan) ? "service date," : "",
                                           string.IsNullOrEmpty(item.PosID.ToString()) ? "place of service" : "",
                                           string.IsNullOrEmpty(item.CalculatedUnit.ToString()) ? "calculated unit," : "",
                                           string.IsNullOrEmpty(item.CalculatedAmount.ToString()) ? "billing amount" : "");
                SetError(errorCount, ref errorMessages, msg);
            }


            // CHECK BILLING PROCIDER RELATED MISSSING INFORMATIONS
            if (string.IsNullOrEmpty(item.BillingProviderName) || string.IsNullOrEmpty(item.BillingProviderAddress)
                || string.IsNullOrEmpty(item.BillingProviderCity) || string.IsNullOrEmpty(item.BillingProviderState)
                || string.IsNullOrEmpty(item.BillingProviderZipcode) || string.IsNullOrEmpty(item.BillingProviderNPI)
                || string.IsNullOrEmpty(item.BillingProviderEIN))
            {
                errorCount = errorCount + 1;
                string msg = string.Format("Billing provider's {0}{1}{2}{3}{4}{5}{6} details are missing.",
                                           string.IsNullOrEmpty(item.BillingProviderName) ? "name," : "",
                                           string.IsNullOrEmpty(item.BillingProviderAddress) ? "address," : "",
                                           string.IsNullOrEmpty(item.BillingProviderCity) ? "city," : "",
                                           string.IsNullOrEmpty(item.BillingProviderState) ? "state," : "",
                                           string.IsNullOrEmpty(item.BillingProviderZipcode) ? "zipcode," : "",
                                           string.IsNullOrEmpty(item.BillingProviderNPI) ? "national provider identifier number," : "",
                                           string.IsNullOrEmpty(item.BillingProviderEIN) ? "employer identification number" : "");
                SetError(errorCount, ref errorMessages, msg);
            }





            // CHECK Supervising Provider  RELATED MISSSING INFORMATIONS

            if (item.IsHomeCare)
            {
                if (string.IsNullOrEmpty(item.SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName) || string.IsNullOrEmpty(item.SupervisingProvidername2420DLoop_NM104_NameFirst)
                || string.IsNullOrEmpty(item.SupervisingProvidername2420DLoop_REF02_ReferenceId)
                || string.IsNullOrEmpty(payorEdi837Setting.SupervisingProvidername2420DLoop_NM101_EntityIdentifierCode)
                || string.IsNullOrEmpty(payorEdi837Setting.SupervisingProvidername2420DLoop_NM102_EntityTypeQualifier)
                || string.IsNullOrEmpty(payorEdi837Setting.SupervisingProvidername2420DLoop_REF01_ReferenceIdQualifier))
                {
                    errorCount = errorCount + 1;
                    string msg = string.Format("Supervising provider's {0}{1}{2}{3}{4}{5} details are missing.",
                                               string.IsNullOrEmpty(item.SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName) ? "Last Name," : "",
                                               string.IsNullOrEmpty(item.SupervisingProvidername2420DLoop_NM104_NameFirst) ? "First Name," : "",
                                               string.IsNullOrEmpty(item.SupervisingProvidername2420DLoop_REF02_ReferenceId) ? "ReferenceId," : "",

                                               string.IsNullOrEmpty(payorEdi837Setting.SupervisingProvidername2420DLoop_NM101_EntityIdentifierCode) ? "EntityIdentifierCode," : "",
                                               string.IsNullOrEmpty(payorEdi837Setting.SupervisingProvidername2420DLoop_NM102_EntityTypeQualifier) ? "EntityTypeQualifier," : "",
                                               string.IsNullOrEmpty(payorEdi837Setting.SupervisingProvidername2420DLoop_REF01_ReferenceIdQualifier) ? "ReferenceIdQualifier" : "");
                    SetError(errorCount, ref errorMessages, msg);
                }

            }
            // CHECK RENDERRING PROCIDER RELATED MISSSING INFORMATIONS
            //if (string.IsNullOrEmpty(item.RenderingProviderName) || string.IsNullOrEmpty(item.RenderingProviderAddress)
            //    || string.IsNullOrEmpty(item.RenderingProviderCity) || string.IsNullOrEmpty(item.RenderingProviderState)
            //    || string.IsNullOrEmpty(item.RenderingProviderZipcode) || string.IsNullOrEmpty(item.RenderingProviderNPI)
            //    //|| string.IsNullOrEmpty(item.RenderingProviderEIN)
            //    )
            //{
            //    errorCount = errorCount + 1;
            //    string msg = string.Format("Rendering provider's {0}{1}{2}{3}{4}{5} details are missing.",
            //                               string.IsNullOrEmpty(item.RenderingProviderName) ? "name," : "",
            //                               string.IsNullOrEmpty(item.RenderingProviderAddress) ? "address," : "",
            //                               string.IsNullOrEmpty(item.RenderingProviderCity) ? "city," : "",
            //                               string.IsNullOrEmpty(item.RenderingProviderState) ? "state," : "",
            //                               string.IsNullOrEmpty(item.RenderingProviderZipcode) ? "zipcode," : "",
            //                               string.IsNullOrEmpty(item.RenderingProviderNPI) ? "national provider identifier number," : ""
            //                               //,string.IsNullOrEmpty(item.RenderingProviderEIN) ? "employer identification number" : ""
            //                               );
            //    SetError(errorCount, ref errorMessages, msg);
            //}

            // CHECK PAYOR RELATED MISSSING FIELDS INFORMATIONS
            if (string.IsNullOrEmpty(item.PayorName) || string.IsNullOrEmpty(item.PayorAddress)
                || string.IsNullOrEmpty(item.PayorCity) || string.IsNullOrEmpty(item.PayorState)
                || string.IsNullOrEmpty(item.PayorZipcode) || string.IsNullOrEmpty(item.PayorIdentificationNumber))
            {
                errorCount = errorCount + 1;
                string msg = string.Format("Payor's {0}{1}{2}{3}{4}{5} details are missing.",
                                           string.IsNullOrEmpty(item.PayorName) ? "name," : "",
                                           string.IsNullOrEmpty(item.PayorAddress) ? "address," : "",
                                           string.IsNullOrEmpty(item.PayorCity) ? "city," : "",
                                           string.IsNullOrEmpty(item.PayorState) ? "state," : "",
                                           string.IsNullOrEmpty(item.PayorZipcode) ? "zipcode," : "",
                                           string.IsNullOrEmpty(item.PayorIdentificationNumber) ? "identification number" : "");
                SetError(errorCount, ref errorMessages, msg);
            }

            #endregion
        }

        public static void SetError(int errorCounter, ref string mainErrorSource, string currentErrorMessage)
        {
            mainErrorSource = mainErrorSource + string.Format("{0}. {1}{2}", errorCounter, currentErrorMessage, Environment.NewLine);
        }

        public static void CheckAndGenerateBatchValidationErrorCsv(ParentBatchRelatedAllDataModel model, ref BatchValidationResponseModel batchValidationResponseModel, EdiFileType ediFileType, string ediFile837ValidationErrorPath)
        {
            StreamWriter clssw = null;

            string tempFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string fileName = string.Format("edi837_validation_error_{0}{1}", tempFileName, ".csv");
            try
            {
                #region LOOP EACH NOTE DETAILS AND CHECK/VALIDATE REQUIRED INFORMATIONS ARE AVAILABLE OR NOT

                int errorNoteCount = 0;

                if (model.BatchRelatedAllDataModel == null || model.BatchRelatedAllDataModel.Count == 0)
                {
                    string fname = string.Format("edi837_validation_exce_error_{0}.txt", tempFileName);
                    string msg = "Batch related data are null..";
                    Common_Edi_837.GenerationBatchExceptionMsg(ref batchValidationResponseModel, (int)EnumEdiFileTypes.Edi837ValidationError, fname, null, msg, ediFile837ValidationErrorPath);
                    return;
                }


                if (model.PayorEdi837Setting == null || model.PayorEdi837Setting.PayorEdi837SettingId == 0)
                {
                    string fname = string.Format("edi837_validation_exce_error_{0}.txt", tempFileName);
                    string msg = "PayorEdi837Setting data are null..";
                    Common_Edi_837.GenerationBatchExceptionMsg(ref batchValidationResponseModel, (int)EnumEdiFileTypes.Edi837ValidationError, fname, null, msg, ediFile837ValidationErrorPath);
                    return;
                }


                foreach (BatchRelatedAllDataModel item in model.BatchRelatedAllDataModel)
                {
                    int errorCount = 0;
                    string errorMessages = "";

                    Common_Edi_837.Validate_Edi_837(model.PayorEdi837Setting, item, ref errorCount, ref errorMessages);

                    if (ediFileType == EdiFileType.Edi_837_P)
                    {
                        if (string.IsNullOrWhiteSpace(item.AuthorizationCode) ||
                            string.IsNullOrWhiteSpace(item.POS_CMS1500) ||
                            item.SpecialProgramCode_CMS1500 <= 0)
                        {
                            string msg = "Client's CMS-1500 {0} is missing.";
                            errorCount = errorCount + 1;
                            List<string> error = new List<string>();
                            #region Add Error Message
                            if (string.IsNullOrWhiteSpace(item.AuthorizationCode))
                            {
                                error.Add("Authorization Code");
                            }
                            if (string.IsNullOrWhiteSpace(item.POS_CMS1500))
                            {
                                error.Add("Facility Code");
                            }
                            if (item.SpecialProgramCode_CMS1500 <= 0)
                            {
                                error.Add("Special Program Code");
                            }
                            #endregion
                            msg = string.Format(msg, string.Join(Constants.Comma, error));
                            Common_Edi_837.SetError(errorCount, ref errorMessages, msg);
                        }
                    }
                    else if (ediFileType == EdiFileType.Edi_837_I)
                    {
                        if (string.IsNullOrWhiteSpace(item.AuthorizationCode) ||
                            string.IsNullOrWhiteSpace(item.POS_UB04) ||
                            string.IsNullOrWhiteSpace(item.AdmissionTypeCode_UB04) ||
                            string.IsNullOrWhiteSpace(item.AdmissionSourceCode_UB04) ||
                            string.IsNullOrWhiteSpace(item.PatientStatusCode_UB04))
                        {
                            string msg = "Client's UB-04 {0} is missing.";
                            errorCount = errorCount + 1;
                            List<string> error = new List<string>();
                            #region Add Error Message
                            if (string.IsNullOrWhiteSpace(item.AuthorizationCode))
                            {
                                error.Add("Authrization Code");
                            }
                            if (string.IsNullOrWhiteSpace(item.POS_UB04))
                            {
                                error.Add("Facility Code");
                            }
                            if (string.IsNullOrWhiteSpace(item.AdmissionTypeCode_UB04))
                            {
                                error.Add("Admission Type");
                            }
                            if (string.IsNullOrWhiteSpace(item.AdmissionSourceCode_UB04))
                            {
                                error.Add("Admission Source");
                            }
                            if (string.IsNullOrWhiteSpace(item.PatientStatusCode_UB04))
                            {
                                error.Add("Patient Status");
                            }
                            #endregion
                            msg = string.Format(msg, string.Join(Constants.Comma, error));
                            Common_Edi_837.SetError(errorCount, ref errorMessages, msg);
                        }

                        if (string.IsNullOrWhiteSpace(item.RevenueCode))
                        {
                            errorCount = errorCount + 1;
                            Common_Edi_837.SetError(errorCount, ref errorMessages, "Payor's revenue code is missing");
                        }

                        if ((!string.IsNullOrWhiteSpace(item.AdmissionSourceCode_UB04) && item.AdmissionSourceCode_UB04.Length > 1) || (!string.IsNullOrWhiteSpace(item.AdmissionTypeCode_UB04) && item.AdmissionTypeCode_UB04.Length > 1))
                        {
                            string msg = "Client's UB-04 {0} is one digit only.";
                            errorCount = errorCount + 1;
                            List<string> error = new List<string>();
                            #region Add Error Message
                            if (item.AdmissionTypeCode_UB04.Length > 1)
                            {
                                error.Add("Admission Type");
                            }
                            if (item.AdmissionSourceCode_UB04.Length > 1)
                            {
                                error.Add("Admission Source");
                            }
                            #endregion
                            msg = string.Format(msg, string.Join(Constants.Comma, error));
                            Common_Edi_837.SetError(errorCount, ref errorMessages, msg);
                        }

                    }

                    #region IF ERROR FOUND THEN OPEN CSV FILE AND WRITE INTO IT
                    if (errorCount > 0)
                    {
                        if (errorNoteCount == 0)
                        {
                            #region Set CSV File Header
                            string absoluteCsvFileBasePath = HttpContext.Current.Server.MapCustomPath(ediFile837ValidationErrorPath);
                            if (!Directory.Exists(absoluteCsvFileBasePath))
                                Directory.CreateDirectory(absoluteCsvFileBasePath);

                            clssw = clssw ?? new StreamWriter(absoluteCsvFileBasePath + fileName);

                            clssw.Write("RefferalID" + ",");
                            clssw.Write("FirstName" + ",");
                            clssw.Write("LastName" + ",");
                            clssw.Write("Beneficiary#" + ",");
                            clssw.Write("Batch #" + ",");
                            clssw.Write("Note #" + ",");
                            clssw.Write("Service Date" + ",");
                            clssw.Write("Error Messages" + ",");
                            clssw.Write(clssw.NewLine);

                            #endregion
                            errorNoteCount = errorNoteCount + 1;
                        }

                        clssw.Write(Convert.ToString(item.ReferralID) + ",");
                        clssw.Write(item.FirstName + ",");
                        clssw.Write(item.LastName + ",");
                        clssw.Write(item.AHCCCSID + ",");
                        clssw.Write(item.BatchID + ",");
                        clssw.Write(item.NoteID + ",");
                        clssw.Write(item.ServiceDate.ToString(Constants.GlobalDateFormat) + ",");
                        clssw.Write(Common.CsvQuote(errorMessages.Trim()) + ",");

                        clssw.Write(clssw.NewLine);
                    }
                    #endregion
                }

                if (errorNoteCount == 0)
                    batchValidationResponseModel.ValidationPassed = true;
                else
                {
                    batchValidationResponseModel.FileName = fileName;
                    batchValidationResponseModel.EdiFileTypeID = (int)EnumEdiFileTypes.Edi837ValidationError;
                    batchValidationResponseModel.ValidationErrorFilePath = string.Format("{0}{1}", ediFile837ValidationErrorPath, fileName);
                }

                #endregion
            }
            catch (Exception ex)
            {
                string fname = string.Format("edi837_validation_exce_error_{0}.txt", tempFileName);
                GenerationBatchExceptionMsg(ref batchValidationResponseModel, (int)EnumEdiFileTypes.Edi837ValidationError, fname, ex, null, ediFile837ValidationErrorPath);
            }
            finally
            {
                if (clssw != null)
                {
                    clssw.Flush();
                    clssw.Close();
                    clssw.Dispose();
                }
            }

        }



        public static void GenerationBatchExceptionMsg(ref BatchValidationResponseModel batchValidationResponseModel, int ediFileTypeId, string fileName, Exception ex, string message, string ediFile837ValidationErrorPath)
        {

            if (ex != null)
                message = string.Format("{1}{0}{2}{0}{3}", Environment.NewLine, ex.Message, ex.StackTrace, ex.Source);
            string fname = fileName; // string.Format("edi837_generate_exce_error_{0}_", tempFileName);

            string filePath = string.Format("{0}{1}/", ediFile837ValidationErrorPath, batchValidationResponseModel.BatchID);
            string errofilePath = Common.CreateLogFile(message, fname, filePath);
            batchValidationResponseModel.FileName = string.Format("{0}", fname);
            batchValidationResponseModel.EdiFileTypeID = ediFileTypeId;

            if (ediFileTypeId == (int)EnumEdiFileTypes.Edi837ValidationError)
            {
                batchValidationResponseModel.ValidationErrorFilePath = errofilePath;
                batchValidationResponseModel.ValidationPassed = false;
            }
            else
            {
                batchValidationResponseModel.Edi837FilePath = errofilePath;
            }
        }

        public static Edi837Model GetEdit837Model(long batchID, ref PayorEdi837Setting payorEdi837Setting, List<BatchRelatedAllDataModel> batchRelatedAllDataList, EdiFileType ediFileType)
        {
            payorEdi837Setting.ISA13_InterchangeControlNo = payorEdi837Setting.ISA13_InterchangeControlNo + 1;
            long controlNumber = payorEdi837Setting.ISA13_InterchangeControlNo;// 80;
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
                    BillingProviderFirstName = tempBillingProvider.BillingProviderFirstName,
                    IdCodeQualifier = payorEdi837Setting.BillingProvider_NM108_IdentificationCodeQualifier,
                    IdCodeQualifierEnum = tempBillingProvider.BillingProviderNPI,
                    AddressInformation = tempBillingProvider.BillingProviderAddress,
                    CityName = tempBillingProvider.BillingProviderCity,
                    StateOrProvinceCode = tempBillingProvider.BillingProviderState,
                    PostalCode = tempBillingProvider.BillingProviderZipcode,
                    ReferenceIdentificationQualifier = payorEdi837Setting.BillingProvider_REF01_ReferenceIdentificationQualifier,
                    ReferenceIdentification = tempBillingProvider.BillingProviderEIN,
                    PVR01_ProviderCode = payorEdi837Setting.BillingProvider_PRV01_ProviderCode,
                    PVR02_ReferenceIdentificationQualifier = payorEdi837Setting.BillingProvider_PRV02_ReferenceIdentificationQualifier,
                    PVR03_ReferenceIdentification = payorEdi837Setting.BillingProvider_PRV03_ProviderTaxonomyCode,
                    I_PVR03_ReferenceIdentification = payorEdi837Setting.I_PVR03_ReferenceIdentification
                };


                #endregion

                foreach (var tempSubscriber in tempBillingProvider.Subscribers.OrderBy(c => c.LastName + ' ' + c.FirstName))
                {

                    #region Add Subscriber
                    Subscriber subscriber = new Subscriber()
                    {
                        HeirarchicalLevelCode = payorEdi837Setting.Subscriber_HL03_HierarchicalLevelCode,   // HL03
                        PayerResponsibilitySequenceNumber = payorEdi837Setting.Subscriber_SBR01_PayerResponsibilitySequenceNumberCode, // SBR01
                        IndividualRelationshipCode = payorEdi837Setting.Subscriber_SBR02_RelationshipCode,// SBR02
                        PolicyNumber = tempSubscriber.PolicyNumber ?? "",// SBR03
                        ClaimFilingIndicatorCode = payorEdi837Setting.Subscriber_SBR09_ClaimFilingIndicatorCode,// SBR09

                        #region Subscriber Name
                        SubmitterEntityIdentifierCode = payorEdi837Setting.Subscriber_NM101_EntityIdentifierCode, // NM101
                        SubmitterEntityTypeQualifier = payorEdi837Setting.Subscriber_NM102_EntityIdentifierCode,// NM102
                        SubmitterNameLastOrOrganizationName = tempSubscriber.LastName, // NM103
                        SubmitterNameFirst = tempSubscriber.FirstName, // NM104
                        SubmitterIdCodeQualifier = payorEdi837Setting.Subscriber_NM108_IdentificationCodeQualifier, // NM108
                        SubmitterIdCodeQualifierEnum = tempSubscriber.SubscriberID,// NM109

                        SubmitterAddressInformation = tempSubscriber.Address.Trim(), // N301
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

                    foreach (var tempClaim in tempSubscriber.Claims.OrderBy(c => c.ServiceDate))
                    {
                        string strDateRange = string.Empty;
                        if (ediFileType == EdiFileType.Edi_837_I)
                        {
                            List<string> StringDates = new List<string>();
                            foreach (var tempServiceLine in tempClaim.ServiceLines)
                            {
                                StringDates.Add(tempServiceLine.ServiceDate.ToString());
                            }
                            DateTime minDate = DateTime.MaxValue;
                            DateTime maxDate = DateTime.MinValue;
                            foreach (string dateString in StringDates)
                            {
                                DateTime date = DateTime.Parse(dateString);
                                if (date < minDate)
                                    minDate = date;
                                if (date > maxDate)
                                    maxDate = date;
                            }
                            strDateRange = minDate.ToString("yyyyMMdd") + "-" + maxDate.ToString("yyyyMMdd");
                        }

                        #region Add Claims
                        Claim claim = new Claim()
                        {
                            //ClaimSubmitterIdentifier = tempClaim.BatchId.ToString(),
                            //C//ClaimSubmitterIdentifier = tempClaim.ClaimSubmitterIdentifier,

                            //PatientControlNumber = tempClaim.PatientAccountNumber,// "Referral|NoteID", // CLM01
                            //TotalClaimChargeAmount = tempClaim.CalculatedAmount.ToString(), // CLM02
                            FacilityCodeValue = tempClaim.POS_CMS1500, //tempClaim.PosID.ToString(), // CLM05-01
                            FacilityCodeQualifier = payorEdi837Setting.Claim_CLM05_02_FacilityCodeQualifier,// CLM05-02

                            //WILL CHNAGE AS PER CALL ORIGINAL, REPLACEMENT OR VOID

                            ClaimFrequencyTypeCode = payorEdi837Setting.Claim_CLM05_03_ClaimFrequencyCode,// CLM05-03

                            ProviderOrSupplierSignatureIndicator = payorEdi837Setting.Claim_CLM06_ProviderSignatureOnFile,// CLM06
                            ProviderAcceptAssignmentCode = payorEdi837Setting.Claim_CLM07_ProviderAcceptAssignment, // CLM07
                            BenefitsAssignmentCerficationIndicator = payorEdi837Setting.Claim_CLM08_AssignmentOfBenefitsIndicator, // CLM08
                            ReleaseOfInformationCode = payorEdi837Setting.Claim_CLM09_ReleaseOfInformationCode,// CLM09
                            PatientSignatureSourceCode = payorEdi837Setting.Claim_CLM010_PatientSignatureSource,// CLM10

                            ReferenceIdentificationQualifier = payorEdi837Setting.Claim_REF01_ReferenceIdentificationQualifier, // REF01
                            ReferenceIdentification = tempClaim.MedicalRecordNumber,// payorEdi837Setting.Claim_REF02_MedicalRecordNumber,// REF02

                            //HealthCareCodeInformation01_01 = payorEdi837Setting.Claim_HI01_01_PrincipalDiagnosisQualifier,// HI01-01
                            //HealthCareCodeInformation01_02 = string.IsNullOrEmpty(tempClaim.ContinuedDX) ? "" : tempClaim.ContinuedDX.Split(',')[0],// HI01-02


                            //HealthCareCodeInformation01 = String.Format("{0}{1}{2}", payorEdi837Setting.Claim_HI01_01_PrincipalDiagnosisQualifier,
                            //payorEdi837Setting.ISA16_ComponentElementSeparator, string.IsNullOrEmpty(tempClaim.ContinuedDX) ? "" : tempClaim.ContinuedDX.Split(',')[0]), // HI01-01, HI01-02
                            SpecialProgramCode = tempClaim.SpecialProgramCode,

                            Prior_ReferenceIdentification = tempClaim.Prior_ReferenceIdentification,
                            Prior_ReferenceIdentificationQualifier = payorEdi837Setting.Claim_Prior_REF01_ReferenceIdentificationQualifier,

                            ReferringProvider_EntityIdentifierCode = payorEdi837Setting.ReferringProvider_NM101_EntityIdentifierCode,
                            ReferringProvider_EntityTypeQualifier = payorEdi837Setting.ReferringProvider_NM102_EntityTypeQualifier,
                            ReferringProvider_IDCodeQualifier = payorEdi837Setting.ReferringProvider_NM108_IDCodeQualifier,
                            ReferringProvider_NameFirst = tempClaim.PhysicianFirstName,
                            ReferringProvider_NameLastOrOrganizationName = tempClaim.PhysicianLastName,
                            ReferringProvider_IDCode = tempClaim.PhysicianNPINumber,
                            IsCaseManagement = tempClaim.IsCaseManagement,
                            IsHomeCare = tempClaim.IsHomeCare,
                            IsDayCare = tempClaim.IsDayCare,


                            //RenderingProvider_EntityIdentifierCode = payorEdi837Setting.RenderingProvider_NM101_EntityIdentifierCode,
                            //RenderingProvider_EntityTypeQualifier = payorEdi837Setting.RenderingProvider_NM102_EntityTypeQualifier,
                            //RenderingProvider_IDCodeQualifier = payorEdi837Setting.RenderingProvider_NM108_IDCodeQualifier,
                            //RenderingProvider_NameFirst = tempClaim.RenderingProviderFirstName,
                            //RenderingProvider_NameLastOrOrganizationName = tempClaim.RenderingProviderName,
                            //RenderingProvider_IDCode = tempClaim.RenderingProviderNPI,

                            //RenderingProvider_ProviderCode = payorEdi837Setting.RenderingProvider_PRV01_ProviderCode,
                            //RenderingProvider_ReferenceIdentificationQualifier = payorEdi837Setting.RenderingProvider_PRV02_ReferenceIdentificationQualifier,

                            I_AttendingProvider_EntityIdentifierCode = payorEdi837Setting.I_AttendingProvider_NM101_EntityIdentifierCode,
                            I_AttendingProvider_EntityTypeQualifier = payorEdi837Setting.I_AttendingProvider_NM102_EntityTypeQualifier,
                            I_AttendingProvider_IDCodeQualifier = payorEdi837Setting.I_AttendingProvider_NM108_IDCodeQualifier,

                            I_Claim_DTP01_DateTimeQualifier = payorEdi837Setting.I_Claim_DTP01_01_DateTimeQualifier,
                            I_Claim_DTP01_DateTimePeriodFormatQualifier = payorEdi837Setting.I_Claim_DTP01_02_DateTimePeriodFormatQualifier,
                            I_Claim_DTP01_DateTimePeriod = strDateRange,


                            I_Claim_DTP02_DateTimeQualifier = payorEdi837Setting.I_Claim_DTP02_01_DateTimeQualifier,
                            I_Claim_DTP02_DateTimePeriodFormatQualifier = payorEdi837Setting.I_Claim_DTP02_02_DateTimePeriodFormatQualifier,
                            I_Claim_DTP02_DateTimePeriod = tempClaim.AdmissionDate,

                            AdmissionTypeCode_UB04 = tempClaim.AdmissionTypeCode_UB04,
                            AdmissionSourceCode_UB04 = tempClaim.AdmissionSourceCode_UB04,
                            PatientStatusCode_UB04 = tempClaim.PatientStatusCode_UB04
                        };


                        if (tempClaim.Submitted_ClaimAdjustmentTypeID == ClaimAdjustmentType.ClaimAdjustmentType_Replacement || tempClaim.Submitted_ClaimAdjustmentTypeID == ClaimAdjustmentType.ClaimAdjustmentType_Void)
                        {
                            if (tempClaim.Submitted_ClaimAdjustmentTypeID == ClaimAdjustmentType.ClaimAdjustmentType_Replacement)
                                claim.ClaimFrequencyTypeCode = payorEdi837Setting.Claim_CLM05_03_ClaimFrequencyCode_Replcement ?? "7";// CLM05-03
                            else if (tempClaim.Submitted_ClaimAdjustmentTypeID == ClaimAdjustmentType.ClaimAdjustmentType_Void)
                                claim.ClaimFrequencyTypeCode = payorEdi837Setting.Claim_CLM05_03_ClaimFrequencyCode_Void ?? "8";// CLM05-03

                            payorEdi837Setting.Claim_ServiceLine_Ref_REF02_ReferenceIdentification_F8_02 = tempClaim.Original_PayerClaimControlNumber;
                            claim.ReferenceIdentificationQualifier_F8_02 = payorEdi837Setting.Claim_ServiceLine_Ref_REF01_ReferenceIdentificationQualifier_F8_02;// REF01
                            claim.ReferenceIdentification_F8_02 = payorEdi837Setting.Claim_ServiceLine_Ref_REF02_ReferenceIdentification_F8_02;// REF02
                        }



                        #region Renderring Provider Information
                        // Remomved Renderring Provider Details for CM
                        // Need to Keep only got HM
                        #region For HM - Renderring Provider Details

                        if (tempClaim.RenderingProviderNPI != null && tempClaim.RenderingProvider_TaxonomyCode != null && tempClaim.IsCaseManagement == false && payorEdi837Setting.RequiredRenderingProvider)
                        {
                            #region Provider Information > Rendering Provider

                            claim.RenderingProvider_EntityIdentifierCode = payorEdi837Setting.RenderingProvider_NM101_EntityIdentifierCode;
                            claim.RenderingProvider_EntityTypeQualifier = payorEdi837Setting.RenderingProvider_NM102_EntityTypeQualifier;
                            claim.RenderingProvider_IDCodeQualifier = payorEdi837Setting.RenderingProvider_NM108_IDCodeQualifier;
                            claim.RenderingProvider_NameFirst = tempClaim.RenderingProviderFirstName;
                            claim.RenderingProvider_NameLastOrOrganizationName = tempClaim.RenderingProviderName;
                            claim.RenderingProvider_IDCode = tempClaim.RenderingProviderNPI;

                            claim.RenderingProvider_ProviderCode = payorEdi837Setting.RenderingProvider_PRV01_ProviderCode;
                            claim.RenderingProvider_ReferenceIdentificationQualifier = payorEdi837Setting.RenderingProvider_PRV02_ReferenceIdentificationQualifier;

                            claim.RenderingProvider_TaxonomyCode = tempClaim.RenderingProvider_TaxonomyCode;



                            #endregion Provider Information > Rendering Provider

                            #region Provider Information > Service Facility Location

                            claim.ServiceFacilityLocationEntityIdentifierCode =
                                payorEdi837Setting.Claim_ServiceFacility_NM101_EntityIdentifierCode; // NM101
                            claim.ServiceFacilityLocationEntityTypeQualifier =
                                payorEdi837Setting.Claim_ServiceFacility_NM102_EntityTypeQualifier; // NM102
                            claim.ServiceFacilityLocationNameLastOrOrganizationName = tempClaim.RenderingProviderName;
                            // NM103
                            claim.ServiceFacilityLocationIdCodeQualifier =
                                payorEdi837Setting.Claim_ServiceFacility_NM108_IdentificationCodeQualifier; // NM108
                            claim.ServiceFacilityLocationIdCodeQualifierEnum = tempClaim.RenderingProviderNPI; // NM109

                            claim.ServiceFacilityLocationAddressInformation = tempClaim.RenderingProviderAddress;
                            // N301
                            claim.ServiceFacilityLocationCityName = tempClaim.RenderingProviderCity; // N401
                            claim.ServiceFacilityLocationStateOrProvinceCode = tempClaim.RenderingProviderState; // N402
                            claim.ServiceFacilityLocationPostalCode = tempClaim.RenderingProviderZipcode; // N403

                            #endregion Provider Information > Service Facility Location

                        }

                        #endregion




                        #region For CM - Renderring Provider Details
                        /*
                        if (tempClaim.RenderingProviderNPI != null && tempClaim.IsCaseManagement == true && (tempBillingProvider.BillingProviderNPI.Trim() != tempClaim.RenderingProviderNPI.Trim() || payorEdi837Setting.RequiredRenderingProvider))
                        {
                            #region Provider Information > Rendering Provider

                            //claim.RenderingProviderEntityIdentifierCode =
                            //    payorEdi837Setting.Claim_RenderringProvider_NM01_EntityIdentifierCode; // NM101
                            //claim.RenderingProviderEntityTypeQualifier =
                            //    payorEdi837Setting.Claim_RenderringProvider_NM02_EntityTypeQualifier; // NM102
                            //claim.RenderingProviderNameLastOrOrganizationName = tempClaim.RenderingProviderName;
                            //// NM103
                            //claim.RenderingProviderIdCodeQualifier =
                            //    payorEdi837Setting.Claim_RenderringProvider_NM108_IdentificationCodeQualifier; // NM108
                            //claim.RenderingProviderIdCodeQualifierEnum = tempClaim.RenderingProviderNPI; // NM109





                            claim.RenderingProvider_EntityIdentifierCode = payorEdi837Setting.RenderingProvider_NM101_EntityIdentifierCode;
                            claim.RenderingProvider_EntityTypeQualifier = payorEdi837Setting.RenderingProvider_NM102_EntityTypeQualifier;
                            claim.RenderingProvider_IDCodeQualifier = payorEdi837Setting.RenderingProvider_NM108_IDCodeQualifier;
                            claim.RenderingProvider_NameFirst = tempClaim.RenderingProviderFirstName;
                            claim.RenderingProvider_NameLastOrOrganizationName = tempClaim.RenderingProviderName;
                            claim.RenderingProvider_IDCode = tempClaim.RenderingProviderNPI;

                            claim.RenderingProvider_ProviderCode = payorEdi837Setting.RenderingProvider_PRV01_ProviderCode;
                            claim.RenderingProvider_ReferenceIdentificationQualifier = payorEdi837Setting.RenderingProvider_PRV02_ReferenceIdentificationQualifier;



                            #endregion Provider Information > Rendering Provider

                            #region Provider Information > Service Facility Location

                            claim.ServiceFacilityLocationEntityIdentifierCode =
                                payorEdi837Setting.Claim_ServiceFacility_NM101_EntityIdentifierCode; // NM101
                            claim.ServiceFacilityLocationEntityTypeQualifier =
                                payorEdi837Setting.Claim_ServiceFacility_NM102_EntityTypeQualifier; // NM102
                            claim.ServiceFacilityLocationNameLastOrOrganizationName = tempClaim.RenderingProviderName;
                            // NM103
                            claim.ServiceFacilityLocationIdCodeQualifier =
                                payorEdi837Setting.Claim_ServiceFacility_NM108_IdentificationCodeQualifier; // NM108
                            claim.ServiceFacilityLocationIdCodeQualifierEnum = tempClaim.RenderingProviderNPI; // NM109

                            claim.ServiceFacilityLocationAddressInformation = tempClaim.RenderingProviderAddress;
                            // N301
                            claim.ServiceFacilityLocationCityName = tempClaim.RenderingProviderCity; // N401
                            claim.ServiceFacilityLocationStateOrProvinceCode = tempClaim.RenderingProviderState; // N402
                            claim.ServiceFacilityLocationPostalCode = tempClaim.RenderingProviderZipcode; // N403

                            #endregion Provider Information > Service Facility Location

                        }

                        */

                        #endregion

                        #endregion Provider Information


                        #region 2420D  Supervising Provider Provider

                        if (tempClaim.IsHomeCare)
                        {
                            //TODO: THIS CAN BE IN MULTIPLE LOOP 
                            claim.SupervisingProviderEntityIdentifierCode = payorEdi837Setting.SupervisingProvidername2420DLoop_NM101_EntityIdentifierCode;//  "DQ"; // NM101
                            claim.SupervisingProviderEntityTypeQualifier = payorEdi837Setting.SupervisingProvidername2420DLoop_NM102_EntityTypeQualifier;//"1"; // NM102
                            claim.SupervisingProviderNameLastOrOrganizationName = tempClaim.SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName;
                            claim.SupervisingProviderFirstName = tempClaim.SupervisingProvidername2420DLoop_NM104_NameFirst;

                            claim.SupervisingProvider_SecondaryIdentification_ReferenceIdentificationQualifier = payorEdi837Setting.SupervisingProvidername2420DLoop_REF01_ReferenceIdQualifier;// // "LU"; // NM101
                            claim.SupervisingProvider_SecondaryIdentification_ReferenceIdentification = tempClaim.SupervisingProvidername2420DLoop_REF02_ReferenceId; // NM102 // TODO: Confirm What Value will come here
                                                                                                                                                                      //claim.SupervisingProvider_SecondaryIdentification_PayorReferenceIdentificationQualifier = "2U"; // TODO: NEED TO CHANGE
                                                                                                                                                                      //claim.SupervisingProvider_SecondaryIdentification_PayorReferenceIdentification = payorEdi837Setting.Subscriber_Payer_NM108_IdentificationCodeQualifier;
                        }
                        #endregion




                        var ces = payorEdi837Setting.ISA16_ComponentElementSeparator;
                        var secondaryDxCodeType = Constants.DXCodeType_ICD10_Secondary;
                        for (int i = 0; i < tempClaim.ContinuedDX.Split(',').Length; i++)
                        {

                            if (i < 4)
                            {
                                if (i == 0)
                                {
                                    payorEdi837Setting.Claim_ServiceLine_SV107_01_DiagnosisCodePointer = string.Empty;
                                }

                                if (string.IsNullOrEmpty(payorEdi837Setting.Claim_ServiceLine_SV107_01_DiagnosisCodePointer))
                                {
                                    payorEdi837Setting.Claim_ServiceLine_SV107_01_DiagnosisCodePointer = Convert.ToString(i + 1);
                                }
                                else
                                {
                                    payorEdi837Setting.Claim_ServiceLine_SV107_01_DiagnosisCodePointer =
                                    payorEdi837Setting.Claim_ServiceLine_SV107_01_DiagnosisCodePointer + ces + Convert.ToString(i + 1);
                                }
                            }



                            var dXCodeDetails = tempClaim.ContinuedDX.Split(',')[i];
                            var dxCodeType = dXCodeDetails.Split(':').Length == 3 ? dXCodeDetails.Split(':')[0].Trim() : payorEdi837Setting.Claim_HI01_01_PrincipalDiagnosisQualifier.Trim();
                            var dxCode = dXCodeDetails.Split(':').Length == 3 ? dXCodeDetails.Split(':')[1] : dXCodeDetails;

                            var count = i + 1;
                            if (count == 1)
                            {
                                switch (dxCodeType)
                                {
                                    case Constants.DXCodeType_ICD10_Primary:
                                        secondaryDxCodeType = Constants.DXCodeType_ICD10_Secondary;
                                        break;
                                    case Constants.DXCodeType_ICD09_Primary:
                                        secondaryDxCodeType = Constants.DXCodeType_ICD09_Secondary;
                                        break;
                                    default:
                                        secondaryDxCodeType = dxCodeType;
                                        break;
                                }
                            }

                            if (count > 1)
                                secondaryDxCodeType = Common.CheckAndSetDxCodeType(secondaryDxCodeType);

                            switch (count)
                            {
                                case 1:
                                    claim.HealthCareCodeInformation01 = String.Format("{0}{1}{2}", dxCodeType, ces, dxCode);
                                    break;
                                case 2:
                                    claim.HealthCareCodeInformation02 = String.Format("{0}{1}{2}", secondaryDxCodeType, ces, dxCode);
                                    break;
                                case 3:
                                    claim.HealthCareCodeInformation03 = String.Format("{0}{1}{2}", secondaryDxCodeType, ces, dxCode);
                                    break;
                                case 4:
                                    claim.HealthCareCodeInformation04 = String.Format("{0}{1}{2}", secondaryDxCodeType, ces, dxCode);
                                    break;
                                case 5:
                                    claim.HealthCareCodeInformation05 = String.Format("{0}{1}{2}", secondaryDxCodeType, ces, dxCode);
                                    break;
                                case 6:
                                    claim.HealthCareCodeInformation06 = String.Format("{0}{1}{2}", secondaryDxCodeType, ces, dxCode);
                                    break;
                                case 7:
                                    claim.HealthCareCodeInformation07 = String.Format("{0}{1}{2}", secondaryDxCodeType, ces, dxCode);
                                    break;
                                case 8:
                                    claim.HealthCareCodeInformation08 = String.Format("{0}{1}{2}", secondaryDxCodeType, ces, dxCode);
                                    break;
                                case 9:
                                    claim.HealthCareCodeInformation09 = String.Format("{0}{1}{2}", secondaryDxCodeType, ces, dxCode);
                                    break;
                                case 10:
                                    claim.HealthCareCodeInformation10 = String.Format("{0}{1}{2}", secondaryDxCodeType, ces, dxCode);
                                    break;
                                case 11:
                                    claim.HealthCareCodeInformation11 = String.Format("{0}{1}{2}", secondaryDxCodeType, ces, dxCode);
                                    break;
                                case 12:
                                    claim.HealthCareCodeInformation12 = String.Format("{0}{1}{2}", secondaryDxCodeType, ces, dxCode);
                                    break;

                            }
                        }



                        #endregion

                        var amount = 0.00;
                        string strClaimId = tempBillingProvider.BillingProviderNPI;
                        string billingPoviderNPI = strClaimId;
                        strClaimId = "";
                        string authorizationId = tempClaim.Prior_ReferenceIdentification;
                        int lx_index = 0;
                        foreach (var tempServiceLine in tempClaim.ServiceLines)
                        {
                            #region Add ServiceLine

                            lx_index = lx_index + 1;
                            ServiceLine serviceLine = new ServiceLine()
                            {
                                AssignedNumber = Convert.ToString(lx_index), // LX01 TODO: NEED TO CHANGE
                                //CompositeMedicalProcedureIdentifier_01 = payorEdi837Setting.Claim_ServiecLine_SV101_01_ProductServiceIDQualifier,
                                //CompositeMedicalProcedureIdentifier_02 = tempServiceLine.ServiceCode,
                                Product_ServiceID = tempServiceLine.RevenueCode,
                                CompositeMedicalProcedureIdentifier = String.Format("{0}{1}{2}", payorEdi837Setting.Claim_ServiecLine_SV101_01_ProductServiceIDQualifier,
                                payorEdi837Setting.ISA16_ComponentElementSeparator, tempServiceLine.ServiceCode), // SV101-01, SV101-02

                                //MonetaryAmount = tempClaim.CalculatedAmount.ToString(),// SV102
                                MonetaryAmount = tempServiceLine.CalculatedAmount.ToString(),// SV102

                                UnitOrBasisForMeasurementCode = payorEdi837Setting.Claim_ServiecLine_SV103_BasisOfMeasurement,// SV103
                                Quantity = tempServiceLine.CalculatedUnit.ToString(),// SV104
                                FacilityCode = tempServiceLine.FacilityCode,
                                //CompositeDiagnosisCodePointer = // SV107
                                CompositeDiagnosisCodePointer = payorEdi837Setting.Claim_ServiceLine_SV107_01_DiagnosisCodePointer,

                                DateTimeQualifier = payorEdi837Setting.Claim_ServiceLine_Date_DTP01_DateTimeQualifier,// DTP01
                                DateTimePeriodFormatQualifier = payorEdi837Setting.Claim_ServiceLine_Date_DTP02_DateTimePeriodFormatQualifier,// DTP02
                                DateTimePeriod = tempServiceLine.ServiceDateSpan,// DTP03

                                ReferenceIdentificationQualifier = payorEdi837Setting.Claim_ServiceLine_Ref_REF01_ReferenceIdentificationQualifier,// REF01
                                ReferenceIdentification = payorEdi837Setting.Claim_ServiceLine_Ref_REF02_ReferenceIdentification,// REF02

                                NTE02_Description = "Need To Rollup Claims Again And Resend",//tempClaim.ClaimAdjustmentReason
                            };

                            //if (!string.IsNullOrEmpty(tempServiceLine.ModifierName) && batchID != 91)
                            //{
                            //    serviceLine.CompositeMedicalProcedureIdentifier = String.Format("{0}{1}{2}",
                            //        serviceLine.CompositeMedicalProcedureIdentifier, payorEdi837Setting.ISA16_ComponentElementSeparator,
                            //        string.Join(payorEdi837Setting.ISA16_ComponentElementSeparator, tempServiceLine.ModifierName.Split(',')));
                            //}

                            for (int i = 0; i < 4; i++)
                            {
                                string modifier = "";
                                try
                                {
                                    modifier = tempServiceLine.ModifierName.Split(',')[i];
                                }
                                catch (Exception e)
                                {
                                    modifier = "";
                                }

                                serviceLine.CompositeMedicalProcedureIdentifier = String.Format("{0}{1}{2}",
                                    serviceLine.CompositeMedicalProcedureIdentifier,
                                    payorEdi837Setting.ISA16_ComponentElementSeparator,
                                    modifier);

                            }

                            if (!string.IsNullOrEmpty(tempServiceLine.ServiceDescription))
                            {
                                serviceLine.CompositeMedicalProcedureIdentifier = String.Format("{0}{1}{2}",
                                    serviceLine.CompositeMedicalProcedureIdentifier,
                                    payorEdi837Setting.ISA16_ComponentElementSeparator,
                                    tempServiceLine.ServiceDescription);
                            }



                            #endregion
                            amount = amount + Convert.ToDouble(tempServiceLine.CalculatedAmount);

                            if (string.IsNullOrEmpty(strClaimId))
                                strClaimId = tempServiceLine.StrBathNoteID;
                            else
                                strClaimId = strClaimId +","+ tempServiceLine.StrBathNoteID;

                            claim.ServiceLines.Add(serviceLine);
                        }
                        //new line
                        claim.TotalClaimChargeAmount = Convert.ToString(amount, CultureInfo.InvariantCulture); // CLM02
                        //claim.ClaimSubmitterIdentifier = string.Format("{0}B{1}", billingPoviderNPI, batchID);
                        
                        
                        //Random generator = new Random();
                        //string strClaimSubmitterIdentifier = generator.Next(0, int.MaxValue).ToString("D10");
                        //claim.ClaimSubmitterIdentifier = strClaimSubmitterIdentifier;
                        claim.ClaimSubmitterIdentifier = string.Format("{0}B{1}", tempSubscriber.ReferralID, batchID);


                        //claim.MyEzCare_BatchNoteID = strClaimId;
                        //claim.MyEzCare_BatchNoteID = string.Format("{0}N{1}", billingPoviderNPI, batchID);
                        claim.StrClaimId = strClaimId;
                        claim.MyEzCare_BatchNoteID = tempClaim.ClaimUniqueTraceID;// string.Format("{0}N{1}", authorizationId, batchID);
                        claim.REF_D9_Key = "D9";


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

            model.InterchangeControlHeader.SegmentTerminator = payorEdi837Setting.SegmentTerminator;
            model.InterchangeControlHeader.ElementSeparator = payorEdi837Setting.ElementSeparator;

            //GS
            model.FunctionalGroupHeader.FunctionalIdentifierCode = payorEdi837Setting.GS01_FunctionalIdentifierCode;
            model.FunctionalGroupHeader.ApplicationSenderCode = payorEdi837Setting.GS02_ApplicationSenderCode;
            model.FunctionalGroupHeader.ApplicationReceiverCode = payorEdi837Setting.GS03_ApplicationReceiverCode;
            model.FunctionalGroupHeader.Date = payorEdi837Setting.GS04_Date;
            model.FunctionalGroupHeader.Time = payorEdi837Setting.GS05_Time;
            model.FunctionalGroupHeader.GroupControlNumber = payorEdi837Setting.GS06_GroupControlNumber;// "80";
            model.FunctionalGroupHeader.ResponsibleAgencyCode = payorEdi837Setting.GS07_ResponsibleAgencyCode;
            model.FunctionalGroupHeader.VersionOrReleaseOrNo = payorEdi837Setting.GS08_VersionOrReleaseOrNo;
            model.FunctionalGroupHeader.I_VersionOrReleaseOrNo = payorEdi837Setting.I_GS08_ST03_VersionOrReleaseOrNo;

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

        public static GroupedModelFor837 GenerateGroupedModelFor837(List<BatchRelatedAllDataModel> batchRelatedAllDataList)
        {
            GroupedModelFor837 groupedModelFor837 = new GroupedModelFor837();
            #region Generate Group Data for 837 Model

            #region Group By Billing Provider
            List<BillingGroupClass> tempBillingGroupList = batchRelatedAllDataList.ToList().GroupBy(ac => new
            {
                ac.BillingProviderID,
                ac.BillingProviderName,
                ac.BillingProviderFirstName,
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
                                                                                 BillingProviderFirstName = grp.Key.BillingProviderFirstName,
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
                    ac.PolicyNumber,
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
                    ac.PayorZipcode,
                    ac.PayorBillingType,
                    ac.PayorID
                })
                                                                     .Select(grp => new SubscriberPayorGroupClass
                                                                     {
                                                                         SubscriberModel = new SubscriberModel()
                                                                         {
                                                                             ReferralID = grp.Key.ReferralID,
                                                                             AHCCCSID = grp.Key.AHCCCSID,
                                                                             CISNumber = grp.Key.CISNumber,
                                                                             PolicyNumber = grp.Key.PolicyNumber,
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
                                                                             PayorZipcode = grp.Key.PayorZipcode,
                                                                             PayorBillingType = grp.Key.PayorBillingType,
                                                                             PayorID= grp.Key.PayorID
                                                                         },
                                                                         ListModel = grp.ToList()

                                                                     }).ToList();







                #endregion

                foreach (SubscriberPayorGroupClass sp_batchRelatedAllDataModel in tempSubscriberPayorGroupList)
                {

                    #region Group By Claims
                    List<ClaimGroupClass> tempClaimGroupList = sp_batchRelatedAllDataModel.ListModel.GroupBy(ac => new
                    {
                        ac.ReferralID,
                        //ac.ClaimSubmitterIdentifier,
                        ac.MedicalRecordNumber,
                        ////ac.PatientAccountNumber,
                        //ac.CalculatedAmount,
                        ac.PosID,
                        ac.PosName,
                        ac.ContinuedDX,
                        //ac.ModifierID,
                        //ac.ModifierName,
                        PhysicianCheckNotRequired = ac.IsCaseManagement,
                        ac.RenderingProviderID,
                        ac.RenderingProviderFirstName,
                        ac.RenderingProviderName,
                        //ac.RenderingProviderEIN,
                        ac.RenderingProviderNPI,
                        ac.RenderingProvider_TaxonomyCode,
                        ac.RenderingProviderGSA,
                        ac.RenderingProviderAddress,
                        ac.RenderingProviderCity,
                        ac.RenderingProviderState,
                        ac.RenderingProviderZipcode,
                        ac.Submitted_ClaimAdjustmentTypeID,
                        ac.Original_PayerClaimControlNumber,
                        //ac.ClaimAdjustmentReason,
                        //ac.ServiceDate,
                        //ac.GroupIDForMileServices,
                        ac.SpecialProgramCode_CMS1500,
                        ac.AuthorizationCode,
                        // ac.AuthrizationCode_CMS1500,
                        ac.POS_CMS1500,
                        ac.AdmissionTypeCode_UB04,
                        ac.AdmissionSourceCode_UB04,
                        ac.PatientStatusCode_UB04,
                        ac.AdmissionDate,
                        ac.PhysicianNPINumber,
                        ac.PhysicianFirstName,
                        ac.PhysicianLastName,
                        ac.BatchID,



                        ac.SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName,
                        ac.SupervisingProvidername2420DLoop_NM104_NameFirst,
                        ac.SupervisingProvidername2420DLoop_REF02_ReferenceId,
                        //ac.IsCaseManagement,
                        ac.IsHomeCare,
                        ac.IsDayCare

                    })
                                                                 .Select(grp => new ClaimGroupClass
                                                                 {
                                                                     ClaimModel = new ClaimModel()
                                                                     {
                                                                         ReferralID = grp.Key.ReferralID,
                                                                         //ClaimSubmitterIdentifier = grp.Key.ClaimSubmitterIdentifier,
                                                                         MedicalRecordNumber = grp.Key.MedicalRecordNumber,
                                                                         ////PatientAccountNumber = grp.Key.PatientAccountNumber,
                                                                         //CalculatedAmount = grp.Key.CalculatedAmount,
                                                                         PosID = grp.Key.PosID,
                                                                         POS_CMS1500 = grp.Key.POS_CMS1500,
                                                                         AdmissionTypeCode_UB04 = grp.Key.AdmissionTypeCode_UB04,
                                                                         AdmissionSourceCode_UB04 = grp.Key.AdmissionSourceCode_UB04,
                                                                         PatientStatusCode_UB04 = grp.Key.PatientStatusCode_UB04,
                                                                         AdmissionDate = grp.Key.AdmissionDate,
                                                                         PosName = grp.Key.PosName,
                                                                         ContinuedDX = grp.Key.ContinuedDX,
                                                                         //ModifierID = grp.Key.ModifierID,
                                                                         //ModifierName = grp.Key.ModifierName,
                                                                         IsCaseManagement = grp.Key.PhysicianCheckNotRequired,
                                                                         RenderingProviderID = grp.Key.RenderingProviderID,
                                                                         RenderingProviderFirstName = grp.Key.RenderingProviderFirstName,
                                                                         RenderingProviderName = grp.Key.RenderingProviderName,
                                                                         //RenderingProviderEIN = grp.Key.RenderingProviderEIN,
                                                                         RenderingProviderNPI = grp.Key.RenderingProviderNPI,
                                                                         RenderingProvider_TaxonomyCode = grp.Key.RenderingProvider_TaxonomyCode,
                                                                         RenderingProviderGSA = grp.Key.RenderingProviderGSA,
                                                                         RenderingProviderAddress = grp.Key.RenderingProviderAddress,
                                                                         RenderingProviderCity = grp.Key.RenderingProviderCity,
                                                                         RenderingProviderState = grp.Key.RenderingProviderState,
                                                                         RenderingProviderZipcode = grp.Key.RenderingProviderZipcode,
                                                                         Submitted_ClaimAdjustmentTypeID = grp.Key.Submitted_ClaimAdjustmentTypeID,
                                                                         Original_PayerClaimControlNumber = grp.Key.Original_PayerClaimControlNumber,
                                                                         //ClaimAdjustmentReason = grp.Key.ClaimAdjustmentReason,
                                                                         //ServiceDate = grp.Key.ServiceDate,
                                                                         //GroupIDForMileServices = grp.Key.GroupIDForMileServices,
                                                                         SpecialProgramCode = grp.Key.SpecialProgramCode_CMS1500,
                                                                         Prior_ReferenceIdentification = grp.Key.AuthorizationCode,
                                                                         //Prior_ReferenceIdentification = grp.Key.AuthrizationCode_CMS1500,
                                                                         PhysicianNPINumber = grp.Key.PhysicianNPINumber,
                                                                         PhysicianFirstName = grp.Key.PhysicianFirstName,
                                                                         PhysicianLastName = grp.Key.PhysicianLastName,
                                                                         BatchId = grp.Key.BatchID,

                                                                         SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName = grp.Key.SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName,
                                                                         SupervisingProvidername2420DLoop_NM104_NameFirst = grp.Key.SupervisingProvidername2420DLoop_NM104_NameFirst,
                                                                         SupervisingProvidername2420DLoop_REF02_ReferenceId = grp.Key.SupervisingProvidername2420DLoop_REF02_ReferenceId,

                                                                         //IsCaseManagement = grp.Key.IsCaseManagement,
                                                                         IsHomeCare = grp.Key.IsHomeCare,
                                                                         IsDayCare = grp.Key.IsDayCare
                                                                     },
                                                                     ListModel = grp.ToList()

                                                                 }).ToList();

                    #endregion

                    foreach (ClaimGroupClass claim_batchRelatedAllDataModel in tempClaimGroupList)
                    {
                        #region Group By ServiceLine
                        List<ServiceLineGroupClass> tempServiceLineGroupList = claim_batchRelatedAllDataModel.ListModel.GroupBy(ac => new
                        {
                            ac.ModifierName,
                            ac.ServiceCode,
                            ac.CalculatedAmount,
                            ac.AMT01_ServiceLineAllowedAmount_AllowedAmount,
                            ac.SVC03_LineItemProviderPaymentAmoun_PaidAmount,
                            ac.MPP_AdjustmentAmount,
                            ac.CalculatedUnit,
                            ac.ServiceDateSpan,
                            ac.ServiceDate,
                            ac.StrBathNoteID,
                            ac.BatchNoteID,
                            ac.NoteID,
                            ac.BatchID,
                            ac.POS_CMS1500,
                            ac.RevenueCode,
                            ac.ServiceDescription
                        })
                                                                     .Select(grp => new ServiceLineGroupClass
                                                                     {
                                                                         ServiceLineModel = new ServiceLineModel()
                                                                         {
                                                                             ModifierName = grp.Key.ModifierName,
                                                                             ServiceCode = grp.Key.ServiceCode,
                                                                             CalculatedAmount = grp.Key.CalculatedAmount,
                                                                             AMT01_ServiceLineAllowedAmount_AllowedAmount = grp.Key.AMT01_ServiceLineAllowedAmount_AllowedAmount,
                                                                             SVC03_LineItemProviderPaymentAmoun_PaidAmount= grp.Key.SVC03_LineItemProviderPaymentAmoun_PaidAmount,
                                                                             MPP_AdjustmentAmount = grp.Key.MPP_AdjustmentAmount,
                                                                             CalculatedUnit = grp.Key.CalculatedUnit,
                                                                             FacilityCode = grp.Key.POS_CMS1500,
                                                                             ServiceDateSpan = grp.Key.ServiceDateSpan,
                                                                             ServiceDate = grp.Key.ServiceDate,
                                                                             StrBathNoteID = grp.Key.StrBathNoteID,
                                                                             BatchNoteID = grp.Key.BatchNoteID,
                                                                             NoteID = grp.Key.NoteID,
                                                                             BatchID = grp.Key.BatchID,
                                                                             RevenueCode = grp.Key.RevenueCode,
                                                                             ServiceDescription = grp.Key.ServiceDescription
                                                                         }//,
                                                                          //ListModel = grp.ToList()

                                                                     }).ToList();

                        #endregion

                        foreach (ServiceLineGroupClass serviceLine_batchRelatedAllDataModel in tempServiceLineGroupList)
                        {
                            claim_batchRelatedAllDataModel.ClaimModel.ServiceLines.Add(serviceLine_batchRelatedAllDataModel.ServiceLineModel);

                        }

                        claim_batchRelatedAllDataModel.ClaimModel.ClaimUniqueTraceID =
                            string.Format("{0}{1}{2}{3}", claim_batchRelatedAllDataModel.ClaimModel.Prior_ReferenceIdentification,
                            claim_batchRelatedAllDataModel.ClaimModel.RenderingProviderNPI,
                            claim_batchRelatedAllDataModel.ClaimModel.PhysicianNPINumber,
                            claim_batchRelatedAllDataModel.ClaimModel.SupervisingProvidername2420DLoop_REF02_ReferenceId);

                        sp_batchRelatedAllDataModel.SubscriberModel.Claims.Add(claim_batchRelatedAllDataModel.ClaimModel);
                    }

                    billingProviderModel.Subscribers.Add(sp_batchRelatedAllDataModel.SubscriberModel);
                }

                groupedModelFor837.BillingProviders.Add(billingProviderModel);
            }

            #endregion
            return groupedModelFor837;
        }




        public static GroupedModelFor837 GetEdit837ModelForView(long batchID, ref PayorEdi837Setting payorEdi837Setting, List<BatchRelatedAllDataModel> batchRelatedAllDataList, EdiFileType ediFileType)
        {
            payorEdi837Setting.ISA13_InterchangeControlNo = payorEdi837Setting.ISA13_InterchangeControlNo + 1;
            long controlNumber = payorEdi837Setting.ISA13_InterchangeControlNo;// 80;
            GroupedModelFor837 groupedModelFor837 = GenerateGroupedModelFor837(batchRelatedAllDataList);
          
            return groupedModelFor837;
        }














        public static void ValidateTemporaryNotes(EdiFileType ediFileType, ref ParentBatchRelatedAllDataModel_Temporary model, ref bool IsValdiationPassed)
        {
            try
            {
                #region LOOP EACH NOTE DETAILS AND CHECK/VALIDATE REQUIRED INFORMATIONS ARE AVAILABLE OR NOT

                foreach (BatchRelatedAllDataModel_Temporary item in model.BatchRelatedAllDataModel_Temporary)
                {
                    int errorCount = 0;
                    string errorMessages = "";

                    Common_Edi_837.Validate_Edi_837(model.PayorEdi837Setting, item.BatchRelatedDataModel, ref errorCount, ref errorMessages);

                    if (ediFileType == EdiFileType.Edi_837_P)
                    {
                        if (string.IsNullOrWhiteSpace(item.BatchRelatedDataModel.AuthorizationCode) ||
                            string.IsNullOrWhiteSpace(item.BatchRelatedDataModel.POS_CMS1500) ||
                            item.BatchRelatedDataModel.SpecialProgramCode_CMS1500 <= 0)
                        {
                            string msg = "Client's CMS-1500 {0} is missing.";
                            errorCount = errorCount + 1;
                            List<string> error = new List<string>();
                            #region Add Error Message
                            if (string.IsNullOrWhiteSpace(item.BatchRelatedDataModel.AuthorizationCode))
                            {
                                error.Add("Authorization Code");
                            }
                            if (string.IsNullOrWhiteSpace(item.BatchRelatedDataModel.POS_CMS1500))
                            {
                                error.Add("Facility Code");
                            }
                            if (item.BatchRelatedDataModel.SpecialProgramCode_CMS1500 <= 0)
                            {
                                error.Add("Special Program Code");
                            }
                            #endregion
                            msg = string.Format(msg, string.Join(Constants.Comma, error));
                            Common_Edi_837.SetError(errorCount, ref errorMessages, msg);
                        }
                    }
                    else if (ediFileType == EdiFileType.Edi_837_I)
                    {
                        if (string.IsNullOrWhiteSpace(item.BatchRelatedDataModel.AuthorizationCode) ||
                            string.IsNullOrWhiteSpace(item.BatchRelatedDataModel.POS_UB04) ||
                            string.IsNullOrWhiteSpace(item.BatchRelatedDataModel.AdmissionTypeCode_UB04) ||
                            string.IsNullOrWhiteSpace(item.BatchRelatedDataModel.AdmissionSourceCode_UB04) ||
                            string.IsNullOrWhiteSpace(item.BatchRelatedDataModel.PatientStatusCode_UB04))
                        {
                            string msg = "Client's UB-04 {0} is missing.";
                            errorCount = errorCount + 1;
                            List<string> error = new List<string>();
                            #region Add Error Message
                            if (string.IsNullOrWhiteSpace(item.BatchRelatedDataModel.AuthorizationCode))
                            {
                                error.Add("Authrization Code");
                            }
                            if (string.IsNullOrWhiteSpace(item.BatchRelatedDataModel.POS_UB04))
                            {
                                error.Add("Facility Code");
                            }
                            if (string.IsNullOrWhiteSpace(item.BatchRelatedDataModel.AdmissionTypeCode_UB04))
                            {
                                error.Add("Admission Type");
                            }
                            if (string.IsNullOrWhiteSpace(item.BatchRelatedDataModel.AdmissionSourceCode_UB04))
                            {
                                error.Add("Admission Source");
                            }
                            if (string.IsNullOrWhiteSpace(item.BatchRelatedDataModel.PatientStatusCode_UB04))
                            {
                                error.Add("Patient Status");
                            }
                            #endregion
                            msg = string.Format(msg, string.Join(Constants.Comma, error));
                            Common_Edi_837.SetError(errorCount, ref errorMessages, msg);
                        }

                        if (string.IsNullOrWhiteSpace(item.BatchRelatedDataModel.RevenueCode))
                        {
                            errorCount = errorCount + 1;
                            Common_Edi_837.SetError(errorCount, ref errorMessages, "Payor's revenue code is missing");
                        }

                        if ((!string.IsNullOrWhiteSpace(item.BatchRelatedDataModel.AdmissionSourceCode_UB04) && item.BatchRelatedDataModel.AdmissionSourceCode_UB04.Length > 1) || (!string.IsNullOrWhiteSpace(item.BatchRelatedDataModel.AdmissionTypeCode_UB04) && item.BatchRelatedDataModel.AdmissionTypeCode_UB04.Length > 1))
                        {
                            string msg = "Client's UB-04 {0} is one digit only.";
                            errorCount = errorCount + 1;
                            List<string> error = new List<string>();
                            #region Add Error Message
                            if (item.BatchRelatedDataModel.AdmissionTypeCode_UB04.Length > 1)
                            {
                                error.Add("Admission Type");
                            }
                            if (item.BatchRelatedDataModel.AdmissionSourceCode_UB04.Length > 1)
                            {
                                error.Add("Admission Source");
                            }
                            #endregion
                            msg = string.Format(msg, string.Join(Constants.Comma, error));
                            Common_Edi_837.SetError(errorCount, ref errorMessages, msg);
                        }

                    }

                    if (errorCount > 0)
                    {
                        item.ErrorCount = errorCount;
                        item.ErrorMessage = errorMessages;
                        item.NoteID = item.BatchRelatedDataModel.NoteID;
                        item.ReferralID = item.BatchRelatedDataModel.ReferralID;
                        IsValdiationPassed = false;
                    }

                    item.BatchRelatedDataModel = null;

                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
