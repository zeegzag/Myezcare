using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Xsl;
using OopFactory.Edi835Parser.Models;
using OopFactory.X12.Parsing;
using OopFactory.X12.Parsing.Model;
using OopFactory.X12.Parsing.Model.Typed;
using OopFactory.X12.Validation;
using TypedLoopNM1 = OopFactory.X12.Parsing.Model.Typed.TypedLoopNM1;
using EDI_837_835_HCCP.Models;

namespace EDI_837_835_HCCP
{
    public class Edi837
    {
        #region CORE METHODS

        public string GenerateEdi837File(Edi837Model model, string fileServerPath, string fileName)
        {
            #region Generate 837 File
            long controlNumber = Convert.ToInt64(model.InterchangeControlHeader.InterchangeControlNumber);


            string strInterchangeDate = model.InterchangeControlHeader.InterchangeDate;
            DateTime date = new DateTime(Convert.ToInt16("20" + strInterchangeDate.Substring(0, 2)), Convert.ToInt32(strInterchangeDate.Substring(2, 2)), Convert.ToInt32(strInterchangeDate.Substring(4, 2)));


            //var message = new Interchange(date, Convert.ToInt32(model.InterchangeControlHeader.InterchangeControlNumber), model.InterchangeControlHeader.UsageIndicator == "P")

            model.InterchangeControlHeader.SegmentTerminator = string.IsNullOrEmpty(model.InterchangeControlHeader.SegmentTerminator) ? "~" : model.InterchangeControlHeader.SegmentTerminator;
            model.InterchangeControlHeader.ElementSeparator = string.IsNullOrEmpty(model.InterchangeControlHeader.ElementSeparator) ? "*" : model.InterchangeControlHeader.ElementSeparator;

            var message = new Interchange(date, Convert.ToInt32(model.InterchangeControlHeader.InterchangeControlNumber), model.InterchangeControlHeader.UsageIndicator == "P",
                Convert.ToChar(model.InterchangeControlHeader.SegmentTerminator),
                Convert.ToChar(model.InterchangeControlHeader.ElementSeparator),
                Convert.ToChar(model.InterchangeControlHeader.ComponentElementSeparator))
            {
                InterchangeSenderIdQualifier = model.InterchangeControlHeader.InterchangeSenderIdQualifier,
                InterchangeSenderId = model.InterchangeControlHeader.InterchangeSenderId,
                InterchangeReceiverIdQualifier = model.InterchangeControlHeader.InterchangeReceiverIdQualifier,
                InterchangeReceiverId = model.InterchangeControlHeader.InterchangeReceiverId,
                SecurityInfo = model.InterchangeControlHeader.SecurityInformation,
                SecurityInfoQualifier = model.InterchangeControlHeader.SecurityInformationQualifier,
                AuthorInfo = model.InterchangeControlHeader.AuthorizationInformation,
                AuthorInfoQualifier = model.InterchangeControlHeader.AuthorizationInformationQualifier
            };
            message.SetElement(10, model.InterchangeControlHeader.InterchangeTime);
            message.SetElement(11, model.InterchangeControlHeader.RepetitionSeparator);
            message.SetElement(12, model.InterchangeControlHeader.InterchangeControlVersionNumber);
            message.SetElement(14, model.InterchangeControlHeader.AcknowledgementRequired);
            message.SetElement(15, model.InterchangeControlHeader.UsageIndicator);
            message.SetElement(16, model.InterchangeControlHeader.ComponentElementSeparator);



            var group = message.AddFunctionGroup(model.FunctionalGroupHeader.FunctionalIdentifierCode, date,
                Convert.ToInt32(controlNumber), model.FunctionalGroupHeader.VersionOrReleaseOrNo);
            group.ApplicationSendersCode = model.InterchangeControlHeader.InterchangeSenderId.Trim();
            group.ApplicationReceiversCode = model.InterchangeControlHeader.InterchangeReceiverId.Trim();
            group.ResponsibleAgencyCode = model.FunctionalGroupHeader.ResponsibleAgencyCode;
            //group.Date = Convert.ToDateTime("12/31/1999");
            //group.ControlNumber = 1;
            group.SetElement(5, model.InterchangeControlHeader.InterchangeTime);

            var transaction = group.AddTransaction(model.TransactionSetHeader.TransactionSetIdentifier, string.Format("{0:0000}", controlNumber));
            transaction.SetElement(3, model.FunctionalGroupHeader.VersionOrReleaseOrNo);

            var bhtSegment = transaction.AddSegment(new TypedSegmentBHT());
            bhtSegment.BHT01_HierarchicalStructureCode = model.BeginningOfHierarchicalTransaction.HierarchicalStructureCode;
            bhtSegment.BHT02_TransactionSetPurposeCode = model.BeginningOfHierarchicalTransaction.TransactionSetPurposeCode;
            bhtSegment.BHT03_ReferenceIdentification = model.BeginningOfHierarchicalTransaction.ReferenceIdentification;
            bhtSegment.BHT04_Date = DateTime.Now;
            bhtSegment.BHT05_Time = DateTime.Now.ToString("HHmm");
            bhtSegment.BHT06_TransactionTypeCode = model.BeginningOfHierarchicalTransaction.TransactionTypeCode;

            var submitterLoop = transaction.AddLoop(new TypedLoopNM1(model.SubmitterName.EntityIdentifierCodeEnum)); //submitter identifier code
            submitterLoop.NM102_EntityTypeQualifier = GetEntityTypeQualifier(model.SubmitterName.EntityTypeQualifier);
            submitterLoop.NM103_NameLastOrOrganizationName = model.SubmitterName.NameLastOrOrganizationName;
            //submitterLoop.NM104_NameFirst = model.SubmitterName.NameFirst;
            submitterLoop.NM108_IdCodeQualifier = model.SubmitterName.IdCodeQualifier;
            submitterLoop.NM109_IdCode = model.SubmitterName.IdCodeQualifierEnum;


            int count = model.SubmitterEDIContact.Count > 2 ? 2 : model.SubmitterEDIContact.Count;
            for (int i = 0; i < count; i++)
            {
                var perSegment = submitterLoop.AddSegment(new TypedSegmentPER());
                perSegment.PER01_ContactFunctionCode = model.SubmitterEDIContact[i].ContactFunctionCode; //information contact function code
                perSegment.PER02_Name = model.SubmitterEDIContact[i].Name;
                perSegment.PER03_CommunicationNumberQualifier = GetCommunicationNumberQualifer(model.SubmitterEDIContact[i].CommunicationNumberQualifier1);
                perSegment.PER04_CommunicationNumber = model.SubmitterEDIContact[i].CommunicationNumber1;
                perSegment.PER05_CommunicationNumberQualifier = GetCommunicationNumberQualifer(model.SubmitterEDIContact[i].CommunicationNumberQualifier2);
                perSegment.PER06_CommunicationNumber = model.SubmitterEDIContact[i].CommunicationNumber2;
                perSegment.PER07_CommunicationNumberQualifier = GetCommunicationNumberQualifer(model.SubmitterEDIContact[i].CommunicationNumberQualifier3);
                perSegment.PER08_CommunicationNumber = model.SubmitterEDIContact[i].CommunicationNumber3;
            }


            var submitterLoop2 = transaction.AddLoop(new TypedLoopNM1(model.ReceiverName.EntityIdentifierCodeEnum));
            submitterLoop2.NM102_EntityTypeQualifier = GetEntityTypeQualifier(model.ReceiverName.EntityTypeQualifier);
            submitterLoop2.NM103_NameLastOrOrganizationName = model.ReceiverName.NameLastOrOrganizationName;
            //submitterLoop2.NM104_NameFirst = model.ReceiverName.NameFirst;
            submitterLoop2.NM108_IdCodeQualifier = model.ReceiverName.IdCodeQualifier;
            submitterLoop2.NM109_IdCode = model.ReceiverName.IdCodeQualifierEnum;

            if (model.BillingProviders.Count > 0)
            {
                int hlLoop = 0;
                foreach (var billingProvider in model.BillingProviders)
                {
                    bool hasChild = billingProvider.Subscribers.Count > 0;

                    #region Billing Provider HL

                    var provider2000AHLoop = transaction.AddHLoop(Convert.ToString(++hlLoop), billingProvider.HeirarchicalLevelCode, hasChild); //*********HL 1 ******

                    #region Billing Provider HL > Billing Provider Name

                    var provider2010ACLoop = provider2000AHLoop.AddLoop(new TypedLoopNM1(billingProvider.EntityIdentifierCode));
                    provider2010ACLoop.NM102_EntityTypeQualifier = GetEntityTypeQualifier(billingProvider.EntityTypeQualifier);
                    provider2010ACLoop.NM103_NameLastOrOrganizationName = billingProvider.NameLastOrOrganizationName;
                    provider2010ACLoop.NM108_IdCodeQualifier = billingProvider.IdCodeQualifier;
                    provider2010ACLoop.NM109_IdCode = billingProvider.IdCodeQualifierEnum;

                    var provider2010AC_N3Segment = provider2010ACLoop.AddSegment(new TypedSegmentN3());
                    provider2010AC_N3Segment.N301_AddressInformation = billingProvider.AddressInformation;

                    var provider2010AC_N4Segment = provider2010ACLoop.AddSegment(new TypedSegmentN4());
                    provider2010AC_N4Segment.N401_CityName = billingProvider.CityName;
                    provider2010AC_N4Segment.N402_StateOrProvinceCode = billingProvider.StateOrProvinceCode;
                    provider2010AC_N4Segment.N403_PostalCode = billingProvider.PostalCode;

                    var provider2010AC_REFSegment = provider2010ACLoop.AddSegment(new TypedSegmentREF());
                    provider2010AC_REFSegment.REF01_ReferenceIdQualifier = billingProvider.ReferenceIdentificationQualifier;
                    provider2010AC_REFSegment.REF02_ReferenceId = billingProvider.ReferenceIdentification;

                    #endregion Billing Provider HL > Billing Provider Name

                    #endregion Billing Provider HL

                    #region Subscriber HL

                    if (billingProvider.Subscribers.Count > 0)
                    {
                        foreach (var subscriber in billingProvider.Subscribers)
                        {
                            var subscriber2000BHLoop = provider2000AHLoop.AddHLoop(Convert.ToString(++hlLoop), subscriber.HeirarchicalLevelCode, false);  // **** HL 2  ******

                            #region Subscriber HL > SBR

                            var segmentSBR = subscriber2000BHLoop.AddSegment(new TypedSegmentSBR());
                            segmentSBR.SBR01_PayerResponsibilitySequenceNumberCode = subscriber.PayerResponsibilitySequenceNumber;
                            segmentSBR.SBR02_IndividualRelationshipCode = subscriber.IndividualRelationshipCode;
                            segmentSBR.SBR03_PolicyOrGroupNumber = subscriber.PolicyNumber;
                            segmentSBR.SBR09_ClaimFilingIndicatorCode = subscriber.ClaimFilingIndicatorCode;

                            #endregion Subscriber HL > SBR

                            #region Subscriber HL > Subscriber Name

                            var subscriberName2010BALoop = subscriber2000BHLoop.AddLoop(new TypedLoopNM1(subscriber.SubmitterEntityIdentifierCode));
                            subscriberName2010BALoop.NM102_EntityTypeQualifier = GetEntityTypeQualifier(subscriber.SubmitterEntityTypeQualifier);
                            subscriberName2010BALoop.NM103_NameLastOrOrganizationName = subscriber.SubmitterNameLastOrOrganizationName;
                            subscriberName2010BALoop.NM104_NameFirst = subscriber.SubmitterNameFirst;
                            subscriberName2010BALoop.NM108_IdCodeQualifier = subscriber.SubmitterIdCodeQualifier;
                            subscriberName2010BALoop.NM109_IdCode = subscriber.SubmitterIdCodeQualifierEnum;

                            var subscriberName2010BA_N3Segment = subscriberName2010BALoop.AddSegment(new TypedSegmentN3());
                            subscriberName2010BA_N3Segment.N301_AddressInformation = subscriber.SubmitterAddressInformation;

                            var subscriberName2010BA_N4Segment = subscriberName2010BALoop.AddSegment(new TypedSegmentN4());
                            subscriberName2010BA_N4Segment.N401_CityName = subscriber.SubmitterCityName;
                            subscriberName2010BA_N4Segment.N402_StateOrProvinceCode = subscriber.SubmitterStateOrProvinceCode;
                            subscriberName2010BA_N4Segment.N403_PostalCode = subscriber.SubmitterPostalCode;

                            var subscriber_DMGSegment = subscriberName2010BALoop.AddSegment(new TypedSegmentDMG());
                            subscriber_DMGSegment.DMG01_DateTimePeriodFormatQualifier = subscriber.SubmitterDateTimePeriodFormatQualifier;
                            subscriber_DMGSegment.DMG02_DateOfBirth = DateTime.ParseExact(subscriber.SubmitterDateTimePeriod, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                            subscriber_DMGSegment.DMG03_Gender = GetGender(subscriber.SubmitterGenderCode);

                            #endregion Subscriber HL > Subscriber Name

                            #region Subscriber HL > Payer Name

                            var subscriberName2010BALoop2 = subscriber2000BHLoop.AddLoop(new TypedLoopNM1(subscriber.PayerEntityIdentifierCode));
                            subscriberName2010BALoop2.NM102_EntityTypeQualifier = GetEntityTypeQualifier(subscriber.PayerEntityTypeQualifier);
                            subscriberName2010BALoop2.NM103_NameLastOrOrganizationName = subscriber.PayerNameLastOrOrganizationName;
                            subscriberName2010BALoop2.NM108_IdCodeQualifier = subscriber.PayerIdCodeQualifier;
                            subscriberName2010BALoop2.NM109_IdCode = subscriber.PayerIdCodeQualifierEnum;

                            var subscriberName2010BA_N3Segment2 = subscriberName2010BALoop2.AddSegment(new TypedSegmentN3());
                            subscriberName2010BA_N3Segment2.N301_AddressInformation = subscriber.PayerAddressInformation;

                            var subscriberName2010BA_N4Segment2 = subscriberName2010BALoop2.AddSegment(new TypedSegmentN4());
                            subscriberName2010BA_N4Segment2.N401_CityName = subscriber.PayerCityName;
                            subscriberName2010BA_N4Segment2.N402_StateOrProvinceCode = subscriber.PayerStateOrProvinceCode;
                            subscriberName2010BA_N4Segment2.N403_PostalCode = subscriber.PayerPostalCode;

                            //var refSegment2 = subscriberName2010BALoop2.AddSegment(new TypedSegmentREF());
                            //refSegment2.REF01_ReferenceIdQualifier = "G2";
                            //refSegment2.REF02_ReferenceId = "KA6663";

                            #endregion Subscriber HL > Payer Name

                            #region Claim Information

                            if (subscriber.Claims.Count > 0)
                            {
                                foreach (var claim in subscriber.Claims)
                                {
                                    #region Claim Information > CLM

                                    var claim2300Loop = subscriber2000BHLoop.AddLoop(new TypedLoopCLM());
                                    //claim2300Loop.CLM01_PatientControlNumber = claim.PatientControlNumber;
                                    claim2300Loop.CLM01_PatientControlNumber = claim.ClaimSubmitterIdentifier;
                                    claim2300Loop.CLM02_TotalClaimChargeAmount = Convert.ToDecimal(String.Format("{0:0.00}", Math.Round(Convert.ToDecimal(claim.TotalClaimChargeAmount), 2)));
                                    claim2300Loop.CLM05._1_FacilityCodeValue = claim.FacilityCodeValue;
                                    claim2300Loop.CLM05._2_FacilityCodeQualifier = claim.FacilityCodeQualifier;
                                    claim2300Loop.CLM05._3_ClaimFrequencyTypeCode = claim.ClaimFrequencyTypeCode;
                                    claim2300Loop.CLM06_ProviderOrSupplierSignatureIndicator = GetBoolean(claim.ProviderOrSupplierSignatureIndicator);
                                    claim2300Loop.CLM07_ProviderAcceptAssignmentCode = claim.ProviderAcceptAssignmentCode;
                                    claim2300Loop.CLM08_BenefitsAssignmentCerficationIndicator = claim.BenefitsAssignmentCerficationIndicator;
                                    claim2300Loop.CLM09_ReleaseOfInformationCode = claim.ReleaseOfInformationCode;
                                    claim2300Loop.CLM10_PatientSignatureSourceCode = claim.PatientSignatureSourceCode;

                                    #endregion Claim Information > CLM

                                    #region Claim Information > Reference > REF

                                    var refSegment = claim2300Loop.AddSegment(new TypedSegmentREF());
                                    refSegment.REF01_ReferenceIdQualifier = claim.ReferenceIdentificationQualifier;
                                    refSegment.REF02_ReferenceId = claim.ReferenceIdentification;


                                    if (!string.IsNullOrEmpty(claim.ReferenceIdentificationQualifier_F8_02))
                                    {
                                        var lxRefSegment = claim2300Loop.AddSegment(new TypedSegmentREF());
                                        lxRefSegment.REF01_ReferenceIdQualifier = claim.ReferenceIdentificationQualifier_F8_02;
                                        lxRefSegment.REF02_ReferenceId = claim.ReferenceIdentification_F8_02;
                                    }


                                    #endregion Claim Information > Reference > REF

                                    #region Claim Information > HI

                                    var hiSegment = claim2300Loop.AddSegment(new TypedSegmentHI());

                                    hiSegment.HI01_HealthCareCodeInformation = claim.HealthCareCodeInformation01 ?? "";
                                    hiSegment.HI02_HealthCareCodeInformation = claim.HealthCareCodeInformation02 ?? "";
                                    hiSegment.HI03_HealthCareCodeInformation = claim.HealthCareCodeInformation03 ?? "";
                                    hiSegment.HI04_HealthCareCodeInformation = claim.HealthCareCodeInformation04 ?? "";
                                    hiSegment.HI05_HealthCareCodeInformation = claim.HealthCareCodeInformation05 ?? "";
                                    hiSegment.HI06_HealthCareCodeInformation = claim.HealthCareCodeInformation06 ?? "";
                                    hiSegment.HI07_HealthCareCodeInformation = claim.HealthCareCodeInformation07 ?? "";
                                    hiSegment.HI08_HealthCareCodeInformation = claim.HealthCareCodeInformation08 ?? "";
                                    hiSegment.HI09_HealthCareCodeInformation = claim.HealthCareCodeInformation09 ?? "";
                                    hiSegment.HI10_HealthCareCodeInformation = claim.HealthCareCodeInformation10 ?? "";
                                    hiSegment.HI11_HealthCareCodeInformation = claim.HealthCareCodeInformation11 ?? "";
                                    hiSegment.HI12_HealthCareCodeInformation = claim.HealthCareCodeInformation12 ?? "";


                                    #endregion Claim Information > HI

                                    #region Claim Information > Provider

                                    if (!string.IsNullOrEmpty(claim.RenderingProviderIdCodeQualifierEnum))
                                    {
                                        var renderingProvider2310BLoop = claim2300Loop.AddLoop(new TypedLoopNM1(claim.RenderingProviderEntityIdentifierCode));
                                        renderingProvider2310BLoop.NM102_EntityTypeQualifier = GetEntityTypeQualifier(claim.RenderingProviderEntityTypeQualifier);
                                        renderingProvider2310BLoop.NM103_NameLastOrOrganizationName = claim.RenderingProviderNameLastOrOrganizationName;
                                        renderingProvider2310BLoop.NM108_IdCodeQualifier = claim.RenderingProviderIdCodeQualifier;
                                        renderingProvider2310BLoop.NM109_IdCode = claim.RenderingProviderIdCodeQualifierEnum;

                                        var serviceFacilityLocation2310CLoop = claim2300Loop.AddLoop(new TypedLoopNM1(claim.ServiceFacilityLocationEntityIdentifierCode));
                                        serviceFacilityLocation2310CLoop.NM102_EntityTypeQualifier = GetEntityTypeQualifier(claim.ServiceFacilityLocationEntityTypeQualifier);
                                        serviceFacilityLocation2310CLoop.NM103_NameLastOrOrganizationName = claim.ServiceFacilityLocationNameLastOrOrganizationName;
                                        serviceFacilityLocation2310CLoop.NM108_IdCodeQualifier = claim.ServiceFacilityLocationIdCodeQualifier;
                                        serviceFacilityLocation2310CLoop.NM109_IdCode = claim.ServiceFacilityLocationIdCodeQualifierEnum;

                                        var serviceFacilityLocation2310CLoop_N3Segment = serviceFacilityLocation2310CLoop.AddSegment(new TypedSegmentN3());
                                        serviceFacilityLocation2310CLoop_N3Segment.N301_AddressInformation = claim.ServiceFacilityLocationAddressInformation;

                                        var serviceFacilityLocation2310CLoop_N4Segment = serviceFacilityLocation2310CLoop.AddSegment(new TypedSegmentN4());
                                        serviceFacilityLocation2310CLoop_N4Segment.N401_CityName = claim.ServiceFacilityLocationCityName;
                                        serviceFacilityLocation2310CLoop_N4Segment.N402_StateOrProvinceCode = claim.ServiceFacilityLocationStateOrProvinceCode;
                                        serviceFacilityLocation2310CLoop_N4Segment.N403_PostalCode = claim.ServiceFacilityLocationPostalCode;
                                    }
                                    #endregion Claim Information > Provider

                                    #region Claim Information > Service Line Number

                                    if (claim.ServiceLines.Count > 0)
                                    {
                                        foreach (var serviceLine in claim.ServiceLines)
                                        {
                                            #region Claim Information > Service Line Number > LX

                                            var lxLoop = claim2300Loop.AddLoop(new TypedLoopLX(ConstantsEdi.ServiceLineNode));
                                            lxLoop.LX01_AssignedNumber = serviceLine.AssignedNumber;

                                            #endregion Claim Information > Service Line Number > LX

                                            #region Claim Information > Service Line Number > SV1

                                            var sv1Segment = lxLoop.AddSegment(new TypedSegmentSV1());
                                            sv1Segment.SV101_CompositeMedicalProcedure = serviceLine.CompositeMedicalProcedureIdentifier;
                                            sv1Segment.SV102_MonetaryAmount = serviceLine.MonetaryAmount;
                                            sv1Segment.SV103_UnitBasisMeasCode = serviceLine.UnitOrBasisForMeasurementCode;
                                            sv1Segment.SV104_Quantity = serviceLine.Quantity;
                                            sv1Segment.SV107_CompDiagCodePoint = serviceLine.CompositeDiagnosisCodePointer;

                                            #endregion Claim Information > Service Line Number > SV1

                                            #region Claim Information > Service Line Number > Dates > DTP

                                            var dtpSegment = lxLoop.AddSegment(new TypedSegmentDTP());
                                            dtpSegment.DTP01_DateTimeQualifier = GetDtpQualifier(serviceLine.DateTimeQualifier);
                                            dtpSegment.DTP02_DateTimePeriodFormatQualifier = GetDtpFormatQualifier(serviceLine.DateTimePeriodFormatQualifier);

                                            var strDateTimePeriod = serviceLine.DateTimePeriod;
                                            DateTime startDate = DateTime.ParseExact(strDateTimePeriod.Substring(0, 8), "yyyyMMdd", null);
                                            DateTime endDate = DateTime.ParseExact(strDateTimePeriod.Substring(9, 8), "yyyyMMdd", null);
                                            dtpSegment.DTP03_Date = new DateTimePeriod(startDate, endDate);

                                            #endregion Claim Information > Service Line Number > Dates > DTP

                                            #region Claim Information > Service Line Number > Reference > REF

                                            //var lxRefSegment = lxLoop.AddSegment(new TypedSegmentREF());
                                            //lxRefSegment.REF01_ReferenceIdQualifier = serviceLine.ReferenceIdentificationQualifier;
                                            //lxRefSegment.REF02_ReferenceId = serviceLine.ReferenceIdentification;

                                            #endregion Claim Information > Service Line Number > Reference > REF


                                            #region Claim Information > Service Line Number > NTE LINE NOTE
                                            if (!string.IsNullOrEmpty(claim.ReferenceIdentificationQualifier_F8_02))
                                            {
                                                var lxNteSegment = lxLoop.AddSegment(new TypedSegmentNTE());
                                                lxNteSegment.NTE01_NoteReferenceCode = serviceLine.NTE01_NoteReferenceCode ?? "ADD";
                                                lxNteSegment.NTE02_Description = serviceLine.NTE02_Description ?? "No Details Available";
                                            }
                                            #endregion Claim Information > Service Line Number > NTE LINE NOTE
                                        }
                                    }

                                    #endregion Claim Information > Service Line Number
                                }
                            }

                            #endregion Claim Information
                        }
                    }

                    #endregion Subscriber HL
                }
            }

            var x12 = message.SerializeToX12(false);
            x12 = RemoveLineEndings(x12);
            {

                //x12 = x12.Replace("\r\n", "");
                //string fileServerPath = HttpContext.Current.Server.MapCustomPath(filePath);
                if (!Directory.Exists(fileServerPath))
                    Directory.CreateDirectory(fileServerPath);
                string fullFileName = fileServerPath + fileName;

                using (Stream outputStream = new FileStream(fullFileName, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter writer = new StreamWriter(outputStream))
                    {
                        writer.Write(x12);
                        writer.Flush();
                        writer.Close();
                        writer.Dispose();

                    }
                    //outputStream.Flush();
                    outputStream.Close();
                    outputStream.Dispose();

                }
                return fullFileName;
            }

            //File.WriteAllText(fullFileName, x12);





            #endregion Generate 837 File
        }

        public string HC_GenerateEdi837File(Edi837Model model, string fileServerPath, string fileName, EdiFileType ediFileType)
        {
            #region Generate 837 File
            long controlNumber = Convert.ToInt64(model.InterchangeControlHeader.InterchangeControlNumber);


            string strInterchangeDate = model.InterchangeControlHeader.InterchangeDate;
            DateTime date = new DateTime(Convert.ToInt16("20" + strInterchangeDate.Substring(0, 2)), Convert.ToInt32(strInterchangeDate.Substring(2, 2)), Convert.ToInt32(strInterchangeDate.Substring(4, 2)));


            //var message = new Interchange(date, Convert.ToInt32(model.InterchangeControlHeader.InterchangeControlNumber), model.InterchangeControlHeader.UsageIndicator == "P")

            model.InterchangeControlHeader.SegmentTerminator = string.IsNullOrEmpty(model.InterchangeControlHeader.SegmentTerminator) ? "~" : model.InterchangeControlHeader.SegmentTerminator;
            model.InterchangeControlHeader.ElementSeparator = string.IsNullOrEmpty(model.InterchangeControlHeader.ElementSeparator) ? "*" : model.InterchangeControlHeader.ElementSeparator;

            var message = new Interchange(date, Convert.ToInt32(model.InterchangeControlHeader.InterchangeControlNumber), model.InterchangeControlHeader.UsageIndicator == "P",
                Convert.ToChar(model.InterchangeControlHeader.SegmentTerminator),
                Convert.ToChar(model.InterchangeControlHeader.ElementSeparator),
                Convert.ToChar(model.InterchangeControlHeader.ComponentElementSeparator))
            {
                InterchangeSenderIdQualifier = model.InterchangeControlHeader.InterchangeSenderIdQualifier,
                InterchangeSenderId = model.InterchangeControlHeader.InterchangeSenderId,
                InterchangeReceiverIdQualifier = model.InterchangeControlHeader.InterchangeReceiverIdQualifier,
                InterchangeReceiverId = model.InterchangeControlHeader.InterchangeReceiverId,
                SecurityInfo = model.InterchangeControlHeader.SecurityInformation,
                SecurityInfoQualifier = model.InterchangeControlHeader.SecurityInformationQualifier,
                AuthorInfo = model.InterchangeControlHeader.AuthorizationInformation,
                AuthorInfoQualifier = model.InterchangeControlHeader.AuthorizationInformationQualifier
            };
            message.SetElement(10, model.InterchangeControlHeader.InterchangeTime);
            message.SetElement(11, model.InterchangeControlHeader.RepetitionSeparator);
            message.SetElement(12, model.InterchangeControlHeader.InterchangeControlVersionNumber);
            message.SetElement(14, model.InterchangeControlHeader.AcknowledgementRequired);
            message.SetElement(15, model.InterchangeControlHeader.UsageIndicator);
            message.SetElement(16, model.InterchangeControlHeader.ComponentElementSeparator);

            string versionOrReleaseOrNo = string.Empty;
            if (ediFileType == EdiFileType.Edi_837_P)
            {
                versionOrReleaseOrNo = model.FunctionalGroupHeader.VersionOrReleaseOrNo;
            }
            else if (ediFileType == EdiFileType.Edi_837_I)
            {
                versionOrReleaseOrNo = model.FunctionalGroupHeader.I_VersionOrReleaseOrNo;
            }

            var group = message.AddFunctionGroup(model.FunctionalGroupHeader.FunctionalIdentifierCode, date,
                Convert.ToInt32(controlNumber), versionOrReleaseOrNo);

            // group.ApplicationSendersCode = model.InterchangeControlHeader.InterchangeSenderId.Trim();
            //  group.ApplicationReceiversCode = model.InterchangeControlHeader.InterchangeReceiverId.Trim();

            group.ApplicationSendersCode = model.FunctionalGroupHeader.ApplicationSenderCode.Trim();
            group.ApplicationReceiversCode = model.FunctionalGroupHeader.ApplicationReceiverCode.Trim();

            group.ResponsibleAgencyCode = model.FunctionalGroupHeader.ResponsibleAgencyCode;
            //group.Date = Convert.ToDateTime("12/31/1999");
            //group.ControlNumber = 1;
            group.SetElement(5, model.InterchangeControlHeader.InterchangeTime);

            var transaction = group.AddTransaction(model.TransactionSetHeader.TransactionSetIdentifier, string.Format("{0:0000}", controlNumber));
            transaction.SetElement(3, versionOrReleaseOrNo);

            var bhtSegment = transaction.AddSegment(new TypedSegmentBHT());
            bhtSegment.BHT01_HierarchicalStructureCode = model.BeginningOfHierarchicalTransaction.HierarchicalStructureCode;
            bhtSegment.BHT02_TransactionSetPurposeCode = model.BeginningOfHierarchicalTransaction.TransactionSetPurposeCode;
            bhtSegment.BHT03_ReferenceIdentification = model.BeginningOfHierarchicalTransaction.ReferenceIdentification;
            bhtSegment.BHT04_Date = DateTime.Now;
            bhtSegment.BHT05_Time = DateTime.Now.ToString("HHmm");
            bhtSegment.BHT06_TransactionTypeCode = model.BeginningOfHierarchicalTransaction.TransactionTypeCode;

            var submitterLoop = transaction.AddLoop(new TypedLoopNM1(model.SubmitterName.EntityIdentifierCodeEnum)); //submitter identifier code
            submitterLoop.NM102_EntityTypeQualifier = GetEntityTypeQualifier(model.SubmitterName.EntityTypeQualifier);
            submitterLoop.NM103_NameLastOrOrganizationName = model.SubmitterName.NameLastOrOrganizationName;
            //submitterLoop.NM104_NameFirst = model.SubmitterName.NameFirst;
            submitterLoop.NM108_IdCodeQualifier = model.SubmitterName.IdCodeQualifier;
            submitterLoop.NM109_IdCode = model.SubmitterName.IdCodeQualifierEnum;


            int count = model.SubmitterEDIContact.Count > 2 ? 2 : model.SubmitterEDIContact.Count;
            for (int i = 0; i < count; i++)
            {
                var perSegment = submitterLoop.AddSegment(new TypedSegmentPER());
                perSegment.PER01_ContactFunctionCode = model.SubmitterEDIContact[i].ContactFunctionCode; //information contact function code
                perSegment.PER02_Name = model.SubmitterEDIContact[i].Name;
                perSegment.PER03_CommunicationNumberQualifier = GetCommunicationNumberQualifer(model.SubmitterEDIContact[i].CommunicationNumberQualifier1);
                perSegment.PER04_CommunicationNumber = model.SubmitterEDIContact[i].CommunicationNumber1;
                perSegment.PER05_CommunicationNumberQualifier = GetCommunicationNumberQualifer(model.SubmitterEDIContact[i].CommunicationNumberQualifier3);
                perSegment.PER06_CommunicationNumber = model.SubmitterEDIContact[i].CommunicationNumber3;
                //perSegment.PER05_CommunicationNumberQualifier = GetCommunicationNumberQualifer(model.SubmitterEDIContact[i].CommunicationNumberQualifier2);
                //perSegment.PER06_CommunicationNumber = model.SubmitterEDIContact[i].CommunicationNumber2;
                //perSegment.PER07_CommunicationNumberQualifier = GetCommunicationNumberQualifer(model.SubmitterEDIContact[i].CommunicationNumberQualifier3);
                //perSegment.PER08_CommunicationNumber = model.SubmitterEDIContact[i].CommunicationNumber3;
            }


            var submitterLoop2 = transaction.AddLoop(new TypedLoopNM1(model.ReceiverName.EntityIdentifierCodeEnum));
            submitterLoop2.NM102_EntityTypeQualifier = GetEntityTypeQualifier(model.ReceiverName.EntityTypeQualifier);
            submitterLoop2.NM103_NameLastOrOrganizationName = model.BillingProviders.FirstOrDefault().Subscribers.FirstOrDefault().PayerNameLastOrOrganizationName;
            submitterLoop2.NM108_IdCodeQualifier = model.ReceiverName.IdCodeQualifier;
            submitterLoop2.NM109_IdCode = model.BillingProviders.FirstOrDefault().Subscribers.FirstOrDefault().PayerIdCodeQualifierEnum;

            //submitterLoop2.NM103_NameLastOrOrganizationName = model.ReceiverName.NameLastOrOrganizationName;
            //submitterLoop2.NM104_NameFirst = model.ReceiverName.NameFirst;
            //submitterLoop2.NM108_IdCodeQualifier = model.ReceiverName.IdCodeQualifier;
            //submitterLoop2.NM109_IdCode = model.InterchangeControlHeader.InterchangeReceiverId.Trim();//model.ReceiverName.IdCodeQualifierEnum;

            if (model.BillingProviders.Count > 0)
            {
                int hlLoop = 0;
                foreach (var billingProvider in model.BillingProviders)
                {
                    bool hasChild = billingProvider.Subscribers.Count > 0;

                    #region Billing Provider HL

                    var provider2000AHLoop = transaction.AddHLoop(Convert.ToString(++hlLoop), billingProvider.HeirarchicalLevelCode, hasChild); //*********HL 1 ******

                    #region Billing Provider HL > PVR

                    var provider2000APVRLoop = provider2000AHLoop.AddSegment(new TypedSegmentPRV());
                    provider2000APVRLoop.PRV01_ProviderCode = billingProvider.PVR01_ProviderCode;
                    provider2000APVRLoop.PRV02_ReferenceIdQualifier = billingProvider.PVR02_ReferenceIdentificationQualifier;
                    provider2000APVRLoop.PRV03_ProviderTaxonomyCode = billingProvider.PVR03_ReferenceIdentification;

                    //if (ediFileType == EdiFileType.Edi_837_P)
                    //{
                    //}
                    //else if (ediFileType == EdiFileType.Edi_837_I)
                    //{
                    //    provider2000APVRLoop.PRV03_ProviderTaxonomyCode = billingProvider.I_PVR03_ReferenceIdentification;
                    //}

                    #endregion

                    #region Billing Provider HL > Billing Provider Name

                    var provider2010ACLoop = provider2000AHLoop.AddLoop(new TypedLoopNM1(billingProvider.EntityIdentifierCode));
                    provider2010ACLoop.NM102_EntityTypeQualifier = GetEntityTypeQualifier(billingProvider.EntityTypeQualifier);
                    provider2010ACLoop.NM103_NameLastOrOrganizationName = billingProvider.NameLastOrOrganizationName;
                    provider2010ACLoop.NM104_NameFirst = string.IsNullOrWhiteSpace(billingProvider.BillingProviderFirstName) ? "" : billingProvider.BillingProviderFirstName;
                    provider2010ACLoop.NM108_IdCodeQualifier = billingProvider.IdCodeQualifier;
                    provider2010ACLoop.NM109_IdCode = billingProvider.IdCodeQualifierEnum;

                    var provider2010AC_N3Segment = provider2010ACLoop.AddSegment(new TypedSegmentN3());
                    provider2010AC_N3Segment.N301_AddressInformation = billingProvider.AddressInformation;

                    var provider2010AC_N4Segment = provider2010ACLoop.AddSegment(new TypedSegmentN4());
                    provider2010AC_N4Segment.N401_CityName = billingProvider.CityName;
                    provider2010AC_N4Segment.N402_StateOrProvinceCode = billingProvider.StateOrProvinceCode;
                    provider2010AC_N4Segment.N403_PostalCode = billingProvider.PostalCode;

                    var provider2010AC_REFSegment = provider2010ACLoop.AddSegment(new TypedSegmentREF());
                    provider2010AC_REFSegment.REF01_ReferenceIdQualifier = billingProvider.ReferenceIdentificationQualifier;
                    provider2010AC_REFSegment.REF02_ReferenceId = billingProvider.ReferenceIdentification;

                    #endregion Billing Provider HL > Billing Provider Name

                    #endregion Billing Provider HL

                    #region Subscriber HL

                    if (billingProvider.Subscribers.Count > 0)
                    {
                        foreach (var subscriber in billingProvider.Subscribers)
                        {
                            var subscriber2000BHLoop = provider2000AHLoop.AddHLoop(Convert.ToString(++hlLoop), subscriber.HeirarchicalLevelCode, false);  // **** HL 2  ******

                            #region Subscriber HL > SBR

                            var segmentSBR = subscriber2000BHLoop.AddSegment(new TypedSegmentSBR());
                            segmentSBR.SBR01_PayerResponsibilitySequenceNumberCode = subscriber.PayerResponsibilitySequenceNumber;
                            segmentSBR.SBR02_IndividualRelationshipCode = subscriber.IndividualRelationshipCode;
                            segmentSBR.SBR03_PolicyOrGroupNumber = subscriber.PolicyNumber;
                            segmentSBR.SBR09_ClaimFilingIndicatorCode = subscriber.ClaimFilingIndicatorCode;

                            #endregion Subscriber HL > SBR

                            #region Subscriber HL > Subscriber Name

                            var subscriberName2010BALoop = subscriber2000BHLoop.AddLoop(new TypedLoopNM1(subscriber.SubmitterEntityIdentifierCode));
                            subscriberName2010BALoop.NM102_EntityTypeQualifier = GetEntityTypeQualifier(subscriber.SubmitterEntityTypeQualifier);
                            subscriberName2010BALoop.NM103_NameLastOrOrganizationName = subscriber.SubmitterNameLastOrOrganizationName;
                            subscriberName2010BALoop.NM104_NameFirst = subscriber.SubmitterNameFirst;
                            subscriberName2010BALoop.NM108_IdCodeQualifier = subscriber.SubmitterIdCodeQualifier;
                            subscriberName2010BALoop.NM109_IdCode = subscriber.SubmitterIdCodeQualifierEnum;

                            var subscriberName2010BA_N3Segment = subscriberName2010BALoop.AddSegment(new TypedSegmentN3());
                            subscriberName2010BA_N3Segment.N301_AddressInformation = subscriber.SubmitterAddressInformation;

                            var subscriberName2010BA_N4Segment = subscriberName2010BALoop.AddSegment(new TypedSegmentN4());
                            subscriberName2010BA_N4Segment.N401_CityName = subscriber.SubmitterCityName;
                            subscriberName2010BA_N4Segment.N402_StateOrProvinceCode = subscriber.SubmitterStateOrProvinceCode;
                            subscriberName2010BA_N4Segment.N403_PostalCode = subscriber.SubmitterPostalCode;

                            var subscriber_DMGSegment = subscriberName2010BALoop.AddSegment(new TypedSegmentDMG());
                            subscriber_DMGSegment.DMG01_DateTimePeriodFormatQualifier = subscriber.SubmitterDateTimePeriodFormatQualifier;
                            subscriber_DMGSegment.DMG02_DateOfBirth = DateTime.ParseExact(subscriber.SubmitterDateTimePeriod, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                            subscriber_DMGSegment.DMG03_Gender = GetGender(subscriber.SubmitterGenderCode);

                            #endregion Subscriber HL > Subscriber Name

                            #region Subscriber HL > Payer Name

                            var subscriberName2010BALoop2 = subscriber2000BHLoop.AddLoop(new TypedLoopNM1(subscriber.PayerEntityIdentifierCode));
                            subscriberName2010BALoop2.NM102_EntityTypeQualifier = GetEntityTypeQualifier(subscriber.PayerEntityTypeQualifier);
                            subscriberName2010BALoop2.NM103_NameLastOrOrganizationName = subscriber.PayerNameLastOrOrganizationName;
                            subscriberName2010BALoop2.NM108_IdCodeQualifier = subscriber.PayerIdCodeQualifier;
                            subscriberName2010BALoop2.NM109_IdCode = subscriber.PayerIdCodeQualifierEnum;

                            var subscriberName2010BA_N3Segment2 = subscriberName2010BALoop2.AddSegment(new TypedSegmentN3());
                            subscriberName2010BA_N3Segment2.N301_AddressInformation = subscriber.PayerAddressInformation;

                            var subscriberName2010BA_N4Segment2 = subscriberName2010BALoop2.AddSegment(new TypedSegmentN4());
                            subscriberName2010BA_N4Segment2.N401_CityName = subscriber.PayerCityName;
                            subscriberName2010BA_N4Segment2.N402_StateOrProvinceCode = subscriber.PayerStateOrProvinceCode;
                            subscriberName2010BA_N4Segment2.N403_PostalCode = subscriber.PayerPostalCode;

                            //var refSegment2 = subscriberName2010BALoop2.AddSegment(new TypedSegmentREF());
                            //refSegment2.REF01_ReferenceIdQualifier = "G2";
                            //refSegment2.REF02_ReferenceId = "KA6663";

                            #endregion Subscriber HL > Payer Name

                            #region Claim Information

                            if (subscriber.Claims.Count > 0)
                            {
                                foreach (var claim in subscriber.Claims)
                                {
                                    #region Claim Information > CLM

                                    var claim2300Loop = subscriber2000BHLoop.AddLoop(new TypedLoopCLM());
                                    //claim2300Loop.CLM01_PatientControlNumber = claim.PatientControlNumber;
                                    claim2300Loop.CLM01_PatientControlNumber = claim.ClaimSubmitterIdentifier;
                                    claim2300Loop.CLM02_TotalClaimChargeAmount = Convert.ToDecimal(String.Format("{0:0.00}", Math.Round(Convert.ToDecimal(claim.TotalClaimChargeAmount), 2)));
                                    claim2300Loop.CLM05._1_FacilityCodeValue = claim.FacilityCodeValue;
                                    //claim2300Loop.CLM05._1_FacilityCodeValue = claim.FacilityCodeValue;
                                    claim2300Loop.CLM05._2_FacilityCodeQualifier = claim.FacilityCodeQualifier;
                                    claim2300Loop.CLM05._3_ClaimFrequencyTypeCode = claim.ClaimFrequencyTypeCode;

                                    if (ediFileType == EdiFileType.Edi_837_P)
                                    {
                                        claim2300Loop.CLM06_ProviderOrSupplierSignatureIndicator = GetBoolean(claim.ProviderOrSupplierSignatureIndicator);
                                    }

                                    claim2300Loop.CLM07_ProviderAcceptAssignmentCode = claim.ProviderAcceptAssignmentCode;
                                    claim2300Loop.CLM08_BenefitsAssignmentCerficationIndicator = claim.BenefitsAssignmentCerficationIndicator;
                                    claim2300Loop.CLM09_ReleaseOfInformationCode = claim.ReleaseOfInformationCode;
                                    //claim2300Loop.CLM10_PatientSignatureSourceCode = claim.PatientSignatureSourceCode;
                                    if (ediFileType == EdiFileType.Edi_837_P)
                                    {
                                        claim2300Loop.CLM12_SpecialProgramCode = claim.SpecialProgramCode > 0 ? string.Format("{0:00}", claim.SpecialProgramCode) : string.Empty;
                                    }

                                    #endregion Claim Information > CLM

                                    #region Claim Information > Date > DTP
                                    if (ediFileType == EdiFileType.Edi_837_I)
                                    {
                                        var dtpSegment01 = claim2300Loop.AddSegment(new TypedSegmentDTP());
                                        dtpSegment01.DTP01_DateTimeQualifier = GetDtpQualifier(claim.I_Claim_DTP01_DateTimeQualifier);
                                        dtpSegment01.DTP02_DateTimePeriodFormatQualifier = GetDtpFormatQualifier(claim.I_Claim_DTP01_DateTimePeriodFormatQualifier);
                                        dtpSegment01.DTP03_Date = GetDtpDate(claim.I_Claim_DTP01_DateTimePeriodFormatQualifier, claim.I_Claim_DTP01_DateTimePeriod);

                                        var dtpSegment02 = claim2300Loop.AddSegment(new TypedSegmentDTP());
                                        dtpSegment02.DTP01_DateTimeQualifier = GetDtpQualifier(claim.I_Claim_DTP02_DateTimeQualifier);
                                        dtpSegment02.DTP02_DateTimePeriodFormatQualifier = GetDtpFormatQualifier(claim.I_Claim_DTP02_DateTimePeriodFormatQualifier);
                                        dtpSegment02.DTP03_Date = GetDtpDate(claim.I_Claim_DTP02_DateTimePeriodFormatQualifier, claim.I_Claim_DTP02_DateTimePeriod);
                                    }
                                    #endregion Claim Information > Date > DTP

                                    #region Claim Information > CL1
                                    if (ediFileType == EdiFileType.Edi_837_I)
                                    {
                                        var cl1Segment = claim2300Loop.AddSegment(new TypedSegmentCL1());
                                        cl1Segment.CL101_AdmissionTypeCode = claim.AdmissionTypeCode_UB04;
                                        cl1Segment.CL102_AdmissionSourceCode = claim.AdmissionSourceCode_UB04;
                                        cl1Segment.CL103_PatientStatusCode = claim.PatientStatusCode_UB04;
                                    }
                                    #endregion Claim Information > CL1

                                    #region Claim Information > Reference > REF

                                    var refSegment01 = claim2300Loop.AddSegment(new TypedSegmentREF());
                                    refSegment01.REF01_ReferenceIdQualifier = claim.Prior_ReferenceIdentificationQualifier;
                                    refSegment01.REF02_ReferenceId = claim.Prior_ReferenceIdentification;

                                    var refSegment = claim2300Loop.AddSegment(new TypedSegmentREF());
                                    refSegment.REF01_ReferenceIdQualifier = claim.ReferenceIdentificationQualifier;
                                    refSegment.REF02_ReferenceId = claim.ReferenceIdentification;

                                    var refSegment02 = claim2300Loop.AddSegment(new TypedSegmentREF());
                                    refSegment02.REF01_ReferenceIdQualifier = claim.REF_D9_Key;
                                    refSegment02.REF02_ReferenceId = claim.MyEzCare_BatchNoteID;

                                    if (!string.IsNullOrEmpty(claim.ReferenceIdentificationQualifier_F8_02))
                                    {
                                        var lxRefSegment = claim2300Loop.AddSegment(new TypedSegmentREF());
                                        lxRefSegment.REF01_ReferenceIdQualifier = claim.ReferenceIdentificationQualifier_F8_02;
                                        lxRefSegment.REF02_ReferenceId = claim.ReferenceIdentification_F8_02;
                                    }

                                    #endregion Claim Information > Reference > REF

                                    #region Claim Information > HI

                                    if (ediFileType == EdiFileType.Edi_837_P)
                                    {
                                        var hiSegment = claim2300Loop.AddSegment(new TypedSegmentHI());

                                        hiSegment.HI01_HealthCareCodeInformation = claim.HealthCareCodeInformation01 ?? "";
                                        hiSegment.HI02_HealthCareCodeInformation = claim.HealthCareCodeInformation02 ?? "";
                                        hiSegment.HI03_HealthCareCodeInformation = claim.HealthCareCodeInformation03 ?? "";
                                        hiSegment.HI04_HealthCareCodeInformation = claim.HealthCareCodeInformation04 ?? "";
                                        hiSegment.HI05_HealthCareCodeInformation = claim.HealthCareCodeInformation05 ?? "";
                                        hiSegment.HI06_HealthCareCodeInformation = claim.HealthCareCodeInformation06 ?? "";
                                        hiSegment.HI07_HealthCareCodeInformation = claim.HealthCareCodeInformation07 ?? "";
                                        hiSegment.HI08_HealthCareCodeInformation = claim.HealthCareCodeInformation08 ?? "";
                                        hiSegment.HI09_HealthCareCodeInformation = claim.HealthCareCodeInformation09 ?? "";
                                        hiSegment.HI10_HealthCareCodeInformation = claim.HealthCareCodeInformation10 ?? "";
                                        hiSegment.HI11_HealthCareCodeInformation = claim.HealthCareCodeInformation11 ?? "";
                                        hiSegment.HI12_HealthCareCodeInformation = claim.HealthCareCodeInformation12 ?? "";
                                    }
                                    else if (ediFileType == EdiFileType.Edi_837_I)
                                    {
                                        var hiSegment = claim2300Loop.AddSegment(new TypedSegmentHI());
                                        hiSegment.HI01_HealthCareCodeInformation = claim.HealthCareCodeInformation01 ?? "";

                                        if (!string.IsNullOrEmpty(claim.HealthCareCodeInformation02))
                                        {
                                            var hiSegment02 = claim2300Loop.AddSegment(new TypedSegmentHI());
                                            hiSegment02.HI01_HealthCareCodeInformation = claim.HealthCareCodeInformation02 ?? "";
                                            hiSegment02.HI02_HealthCareCodeInformation = claim.HealthCareCodeInformation03 ?? "";
                                            hiSegment02.HI03_HealthCareCodeInformation = claim.HealthCareCodeInformation04 ?? "";
                                            hiSegment02.HI04_HealthCareCodeInformation = claim.HealthCareCodeInformation05 ?? "";
                                            hiSegment02.HI05_HealthCareCodeInformation = claim.HealthCareCodeInformation06 ?? "";
                                            hiSegment02.HI06_HealthCareCodeInformation = claim.HealthCareCodeInformation07 ?? "";
                                            hiSegment02.HI07_HealthCareCodeInformation = claim.HealthCareCodeInformation08 ?? "";
                                            hiSegment02.HI08_HealthCareCodeInformation = claim.HealthCareCodeInformation09 ?? "";
                                            hiSegment02.HI09_HealthCareCodeInformation = claim.HealthCareCodeInformation10 ?? "";
                                            hiSegment02.HI10_HealthCareCodeInformation = claim.HealthCareCodeInformation11 ?? "";
                                        }

                                    }

                                    #endregion Claim Information > HI

                                    #region  Claim Information > Provider Information

                                    if (ediFileType == EdiFileType.Edi_837_P && claim.IsCaseManagement == false
                                        && !string.IsNullOrEmpty(claim.ReferringProvider_NameLastOrOrganizationName)
                                        && !string.IsNullOrEmpty(claim.ReferringProvider_NameFirst)
                                        && !string.IsNullOrEmpty(claim.ReferringProvider_IDCodeQualifier)
                                        && !string.IsNullOrEmpty(claim.ReferringProvider_IDCode))
                                    {
                                        #region Referring Provider
                                        var referringProvider2310BLoop = claim2300Loop.AddLoop(new TypedLoopNM1(claim.ReferringProvider_EntityIdentifierCode));
                                        referringProvider2310BLoop.NM102_EntityTypeQualifier = GetEntityTypeQualifier(Convert.ToString(claim.ReferringProvider_EntityTypeQualifier));
                                        referringProvider2310BLoop.NM103_NameLastOrOrganizationName = claim.ReferringProvider_NameLastOrOrganizationName;
                                        referringProvider2310BLoop.NM104_NameFirst = claim.ReferringProvider_NameFirst;
                                        referringProvider2310BLoop.NM108_IdCodeQualifier = claim.ReferringProvider_IDCodeQualifier;
                                        referringProvider2310BLoop.NM109_IdCode = claim.ReferringProvider_IDCode;
                                        #endregion Referring Provider
                                    }

                                    if (ediFileType == EdiFileType.Edi_837_I)
                                    {
                                        #region Attending Provider name
                                        var attendingProvider2310ALoop = claim2300Loop.AddLoop(new TypedLoopNM1(claim.I_AttendingProvider_EntityIdentifierCode));
                                        attendingProvider2310ALoop.NM102_EntityTypeQualifier = GetEntityTypeQualifier(Convert.ToString(claim.I_AttendingProvider_EntityTypeQualifier));
                                        attendingProvider2310ALoop.NM103_NameLastOrOrganizationName = claim.ReferringProvider_NameLastOrOrganizationName;
                                        attendingProvider2310ALoop.NM104_NameFirst = claim.ReferringProvider_NameFirst;
                                        attendingProvider2310ALoop.NM108_IdCodeQualifier = claim.I_AttendingProvider_IDCodeQualifier;
                                        attendingProvider2310ALoop.NM109_IdCode = claim.ReferringProvider_IDCode;
                                        #endregion Attending Provider name

                                        #region Rendering Provider
                                        var referringProvider2310BLoop = claim2300Loop.AddLoop(new TypedLoopNM1(claim.RenderingProvider_EntityIdentifierCode));
                                        referringProvider2310BLoop.NM102_EntityTypeQualifier = GetEntityTypeQualifier(Convert.ToString(claim.RenderingProvider_EntityTypeQualifier));
                                        referringProvider2310BLoop.NM103_NameLastOrOrganizationName = claim.RenderingProvider_NameLastOrOrganizationName;
                                        referringProvider2310BLoop.NM104_NameFirst = claim.RenderingProvider_NameFirst;
                                        referringProvider2310BLoop.NM108_IdCodeQualifier = claim.RenderingProvider_IDCodeQualifier;
                                        referringProvider2310BLoop.NM109_IdCode = claim.RenderingProvider_IDCode;
                                        #endregion Rendering Provider
                                    }

                                    #endregion  Claim Information > Provider Information




                                    #region Claim Information > Service Line Number

                                    if (claim.ServiceLines.Count > 0)
                                    {
                                        foreach (var serviceLine in claim.ServiceLines)
                                        {
                                            #region Claim Information > Service Line Number > LX

                                            var lxLoop = claim2300Loop.AddLoop(new TypedLoopLX(ConstantsEdi.ServiceLineNode));
                                            lxLoop.LX01_AssignedNumber = serviceLine.AssignedNumber;

                                            #endregion Claim Information > Service Line Number > LX

                                            #region Claim Information > Service Line Number > SV1

                                            if (ediFileType == EdiFileType.Edi_837_P)
                                            {
                                                var sv1Segment = lxLoop.AddSegment(new TypedSegmentSV1());
                                                sv1Segment.SV101_CompositeMedicalProcedure = serviceLine.CompositeMedicalProcedureIdentifier;
                                                sv1Segment.SV102_MonetaryAmount = serviceLine.MonetaryAmount;
                                                sv1Segment.SV103_UnitBasisMeasCode = serviceLine.UnitOrBasisForMeasurementCode;
                                                sv1Segment.SV104_Quantity = serviceLine.Quantity;
                                                sv1Segment.SV105_FacilityCode = serviceLine.FacilityCode;
                                                sv1Segment.SV107_CompDiagCodePoint = serviceLine.CompositeDiagnosisCodePointer;
                                            }
                                            else if (ediFileType == EdiFileType.Edi_837_I)
                                            {
                                                var sv2Segment = lxLoop.AddSegment(new TypedSegmentSV2());
                                                sv2Segment.SV201_Product_ServiceID = serviceLine.Product_ServiceID;
                                                sv2Segment.SV202_CompositeMedicalProcedure = serviceLine.CompositeMedicalProcedureIdentifier;
                                                sv2Segment.SV203_MonetaryAmount = serviceLine.MonetaryAmount;
                                                sv2Segment.SV204_UnitBasisMeasCode = serviceLine.UnitOrBasisForMeasurementCode;
                                                sv2Segment.SV205_Quantity = serviceLine.Quantity;
                                            }

                                            #endregion Claim Information > Service Line Number > SV1

                                            #region Claim Information > Service Line Number > Dates > DTP

                                            var dtpSegment = lxLoop.AddSegment(new TypedSegmentDTP());
                                            dtpSegment.DTP01_DateTimeQualifier = GetDtpQualifier(serviceLine.DateTimeQualifier);
                                            dtpSegment.DTP02_DateTimePeriodFormatQualifier = GetDtpFormatQualifier(serviceLine.DateTimePeriodFormatQualifier);
                                            dtpSegment.DTP03_Date = GetDtpDate(serviceLine.DateTimePeriodFormatQualifier, serviceLine.DateTimePeriod);

                                            #endregion Claim Information > Service Line Number > Dates > DTP

                                            if (ediFileType == EdiFileType.Edi_837_P)
                                            {


                                                #region 2420D  Supervising Provider Provider
                                                // As per disussion with Pallav, We  need to send Supervising Info HM Only

                                                //if (!claim.IsCaseManagement &&  !string.IsNullOrEmpty(claim.SupervisingProviderNameLastOrOrganizationName) && !string.IsNullOrEmpty(claim.SupervisingProvider_SecondaryIdentification_ReferenceIdentification))
                                                if (claim.IsHomeCare)
                                                {
                                                    var supervisingProvidername2420DLoop = claim2300Loop.AddLoop(new TypedLoopNM1(claim.SupervisingProviderEntityIdentifierCode));
                                                    supervisingProvidername2420DLoop.NM102_EntityTypeQualifier = GetEntityTypeQualifier(Convert.ToString(claim.SupervisingProviderEntityTypeQualifier));
                                                    supervisingProvidername2420DLoop.NM103_NameLastOrOrganizationName = claim.SupervisingProviderNameLastOrOrganizationName;
                                                    supervisingProvidername2420DLoop.NM104_NameFirst = string.IsNullOrEmpty(claim.SupervisingProviderFirstName) ? "" : claim.SupervisingProviderFirstName;

                                                    var sv_refSegment = supervisingProvidername2420DLoop.AddSegment(new TypedSegmentREF());
                                                    sv_refSegment.REF01_ReferenceIdQualifier = claim.SupervisingProvider_SecondaryIdentification_ReferenceIdentificationQualifier;
                                                    sv_refSegment.REF02_ReferenceId = claim.SupervisingProvider_SecondaryIdentification_ReferenceIdentification;

                                                }



                                                #endregion
                                                
                                                
                                                // 09/14/2021 - As per disussion with Pallav, We don't need to send Renderring Info For CM 
                                                //Need to Send Renderring Info for HC but only when client Taxonomy Code is avaialble
                                                #region Renderring Provider for HC

                                                if (!claim.IsCaseManagement && !string.IsNullOrEmpty(claim.RenderingProvider_EntityIdentifierCode))
                                                {

                                                    #region Claim Informcation > Service Line Number > Rendering Provider names

                                                    var renderingProvidername2420ALoop = claim2300Loop.AddLoop(new TypedLoopNM1(claim.RenderingProvider_EntityIdentifierCode));
                                                    renderingProvidername2420ALoop.NM102_EntityTypeQualifier = GetEntityTypeQualifier(Convert.ToString(claim.RenderingProvider_EntityTypeQualifier));
                                                    renderingProvidername2420ALoop.NM103_NameLastOrOrganizationName = claim.RenderingProvider_NameLastOrOrganizationName;
                                                    renderingProvidername2420ALoop.NM104_NameFirst = string.IsNullOrEmpty(claim.RenderingProvider_NameFirst) ? "" : claim.RenderingProvider_NameFirst;
                                                    renderingProvidername2420ALoop.NM108_IdCodeQualifier = claim.RenderingProvider_IDCodeQualifier;
                                                    renderingProvidername2420ALoop.NM109_IdCode = claim.RenderingProvider_IDCode;

                                                    var prvSegment = renderingProvidername2420ALoop.AddSegment(new TypedSegmentPRV());
                                                    prvSegment.PRV01_ProviderCode = claim.RenderingProvider_ProviderCode;
                                                    prvSegment.PRV02_ReferenceIdQualifier = claim.RenderingProvider_ReferenceIdentificationQualifier;
                                                    prvSegment.PRV03_ProviderTaxonomyCode = claim.RenderingProvider_TaxonomyCode;
                                                    //prvSegment.PRV03_ProviderTaxonomyCode = billingProvider.PVR03_ReferenceIdentification;

                                                    #endregion Claim Informcation > Service Line Number > Rendering Provider names

                                                    #region Claim Information > Service Line Number > Reference > REF

                                                    //var lxRefSegment = lxLoop.AddSegment(new TypedSegmentREF());
                                                    //lxRefSegment.REF01_ReferenceIdQualifier = serviceLine.ReferenceIdentificationQualifier;
                                                    //lxRefSegment.REF02_ReferenceId = serviceLine.ReferenceIdentification;

                                                    var lxRefSegment = renderingProvidername2420ALoop.AddSegment(new TypedSegmentREF());
                                                    lxRefSegment.REF01_ReferenceIdQualifier = "LU"; //LU
                                                    lxRefSegment.REF02_ReferenceId = claim.RenderingProvider_IDCode;

                                                    #endregion Claim Information > Service Line Number > Reference > REF
                                                }

                                                #endregion


                                                #region Renderring Provider for CM
                                                /*
                                                if (claim.IsCaseManagement && !string.IsNullOrEmpty(claim.RenderingProvider_EntityIdentifierCode))
                                                {

                                                    #region Claim Informcation > Service Line Number > Rendering Provider names

                                                    var renderingProvidername2420ALoop = claim2300Loop.AddLoop(new TypedLoopNM1(claim.RenderingProvider_EntityIdentifierCode));
                                                    renderingProvidername2420ALoop.NM102_EntityTypeQualifier = GetEntityTypeQualifier(Convert.ToString(claim.RenderingProvider_EntityTypeQualifier));
                                                    renderingProvidername2420ALoop.NM103_NameLastOrOrganizationName = claim.RenderingProvider_NameLastOrOrganizationName;
                                                    renderingProvidername2420ALoop.NM104_NameFirst = string.IsNullOrEmpty(claim.RenderingProvider_NameFirst) ? "" : claim.RenderingProvider_NameFirst;
                                                    renderingProvidername2420ALoop.NM108_IdCodeQualifier = claim.RenderingProvider_IDCodeQualifier;
                                                    renderingProvidername2420ALoop.NM109_IdCode = claim.RenderingProvider_IDCode;

                                                    var prvSegment = renderingProvidername2420ALoop.AddSegment(new TypedSegmentPRV());
                                                    prvSegment.PRV01_ProviderCode = claim.RenderingProvider_ProviderCode;
                                                    prvSegment.PRV02_ReferenceIdQualifier = claim.RenderingProvider_ReferenceIdentificationQualifier;
                                                    prvSegment.PRV03_ProviderTaxonomyCode = billingProvider.PVR03_ReferenceIdentification;

                                                    #endregion Claim Informcation > Service Line Number > Rendering Provider names

                                                    #region Claim Information > Service Line Number > Reference > REF

                                                    //var lxRefSegment = lxLoop.AddSegment(new TypedSegmentREF());
                                                    //lxRefSegment.REF01_ReferenceIdQualifier = serviceLine.ReferenceIdentificationQualifier;
                                                    //lxRefSegment.REF02_ReferenceId = serviceLine.ReferenceIdentification;

                                                    var lxRefSegment = renderingProvidername2420ALoop.AddSegment(new TypedSegmentREF());
                                                    lxRefSegment.REF01_ReferenceIdQualifier = "LU"; //LU
                                                    lxRefSegment.REF02_ReferenceId = claim.RenderingProvider_IDCode;

                                                    #endregion Claim Information > Service Line Number > Reference > REF
                                                }

                                                */

                                                #endregion





                                                if (!claim.IsCaseManagement)
                                                {
                                                    #region Claim Information > Service Line Number > AMBULANCE PickUP Address


                                                    var ambulancePickUpAddress2420G = claim2300Loop.AddLoop(new TypedLoopNM1("PW"));
                                                    ambulancePickUpAddress2420G.NM102_EntityTypeQualifier = GetEntityTypeQualifier("2");


                                                    var ambulancePickUpAddress2420G_N3Segment = ambulancePickUpAddress2420G.AddSegment(new TypedSegmentN3());
                                                    ambulancePickUpAddress2420G_N3Segment.N301_AddressInformation = subscriber.SubmitterAddressInformation; //TODO: NEED TO SET

                                                    var ambulancePickUpAddress2420G_N4Segment = ambulancePickUpAddress2420G.AddSegment(new TypedSegmentN4());
                                                    ambulancePickUpAddress2420G_N4Segment.N401_CityName = subscriber.SubmitterCityName; //TODO: NEED TO SET
                                                    ambulancePickUpAddress2420G_N4Segment.N402_StateOrProvinceCode = subscriber.SubmitterStateOrProvinceCode; //TODO: NEED TO SET
                                                    ambulancePickUpAddress2420G_N4Segment.N403_PostalCode = subscriber.SubmitterPostalCode; //TODO: NEED TO SET


                                                    #endregion

                                                    #region Claim Information > Service Line Number > AMBULANCE Drop Off Address

                                                    var ambulanceDropOffAddress2420G = claim2300Loop.AddLoop(new TypedLoopNM1("45"));
                                                    ambulanceDropOffAddress2420G.NM102_EntityTypeQualifier = GetEntityTypeQualifier("2");


                                                    var ambulanceDropOffAddress2420G_N3Segment = ambulanceDropOffAddress2420G.AddSegment(new TypedSegmentN3());
                                                    ambulanceDropOffAddress2420G_N3Segment.N301_AddressInformation = subscriber.SubmitterAddressInformation; //TODO: NEED TO SET

                                                    var ambulanceDropOffAddress2420G_N4Segment = ambulanceDropOffAddress2420G.AddSegment(new TypedSegmentN4());
                                                    ambulanceDropOffAddress2420G_N4Segment.N401_CityName = subscriber.SubmitterCityName; //TODO: NEED TO SET
                                                    ambulanceDropOffAddress2420G_N4Segment.N402_StateOrProvinceCode = subscriber.SubmitterStateOrProvinceCode; //TODO: NEED TO SET
                                                    ambulanceDropOffAddress2420G_N4Segment.N403_PostalCode = subscriber.SubmitterPostalCode; //TODO: NEED TO SET

                                                    #endregion

                                                }

                                                #region Claim Information > Service Line Number > NTE LINE NOTE
                                                if (!string.IsNullOrEmpty(claim.ReferenceIdentificationQualifier_F8_02))
                                                {
                                                    var lxNteSegment = lxLoop.AddSegment(new TypedSegmentNTE());
                                                    lxNteSegment.NTE01_NoteReferenceCode = serviceLine.NTE01_NoteReferenceCode ?? "ADD";
                                                    lxNteSegment.NTE02_Description = serviceLine.NTE02_Description ?? "No Details Available";
                                                }
                                                #endregion Claim Information > Service Line Number > NTE LINE NOTE
                                            }
                                        }
                                    }

                                    #endregion Claim Information > Service Line Number
                                }
                            }

                            #endregion Claim Information
                        }
                    }

                    #endregion Subscriber HL
                }
            }

            var x12 = message.SerializeToX12(false);
            x12 = RemoveLineEndings(x12);
            {

                //x12 = x12.Replace("\r\n", "");
                //string fileServerPath = HttpContext.Current.Server.MapCustomPath(filePath);
                if (!Directory.Exists(fileServerPath))
                    Directory.CreateDirectory(fileServerPath);
                string fullFileName = fileServerPath + fileName;

                using (Stream outputStream = new FileStream(fullFileName, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter writer = new StreamWriter(outputStream))
                    {
                        writer.Write(x12);
                        writer.Flush();
                        writer.Close();
                        writer.Dispose();

                    }
                    //outputStream.Flush();
                    outputStream.Close();
                    outputStream.Dispose();

                }
                return fullFileName;
            }

            //File.WriteAllText(fullFileName, x12);





            #endregion Generate 837 File
        }

        //public string GenerateEdi837CSVFile(Edi837Model model, string absoluteCsvFileBasePath, string fileName)
        //{

        //    //string absoluteCsvFileBasePath = HttpContext.Current.Server.MapCustomPath(filePath);
        //    if (!Directory.Exists(absoluteCsvFileBasePath))
        //        Directory.CreateDirectory(absoluteCsvFileBasePath);
        //    StreamWriter clssw = new StreamWriter(absoluteCsvFileBasePath + fileName);

        //    #region CSV Header
        //    //AddCsvCell(clssw, "PrimaryInsuranceName");
        //    //AddCsvCell(clssw, "PrimaryInsuranceAddress1");
        //    //AddCsvCell(clssw, "PrimaryInsuranceAddress2");
        //    //AddCsvCell(clssw, "PrimaryInsuranceCSZ");
        //    //AddCsvCell(clssw, "Invoice No");
        //    //AddCsvCell(clssw, "PrimaryMedicare");
        //    //AddCsvCell(clssw, "PrimaryMedicaid");
        //    //AddCsvCell(clssw, "1 Champus");
        //    //AddCsvCell(clssw, "1 ChampVa");
        //    //AddCsvCell(clssw, "1 Group Health");
        //    //AddCsvCell(clssw, "1 FECA");
        //    //AddCsvCell(clssw, "1 FECA");
        //    //AddCsvCell(clssw, "PrimaryInsurancePolicy");
        //    //AddCsvCell(clssw, "PatientName");
        //    //AddCsvCell(clssw, "PatientDOB");
        //    //AddCsvCell(clssw, "PatientGenderMale");
        //    //AddCsvCell(clssw, "PatientGenderFemale");
        //    //AddCsvCell(clssw, "InsuredName");
        //    //AddCsvCell(clssw, "PatientAddress");
        //    //AddCsvCell(clssw, "InsuredRelationshipSelf");
        //    //AddCsvCell(clssw, "InsuredRelationshipSpouse");
        //    //AddCsvCell(clssw, "InsuredRelationshipChild");
        //    AddCsvCell(clssw, "InsuredRelationshipOther");
        //    #endregion

        //    #region CSV Content
        //    #endregion

        //    #region Generate 837 File
        //    if (model.BillingProviders.Count > 0)
        //    {
        //        int hlLoop = 0;
        //        foreach (var billingProvider in model.BillingProviders)
        //        {
        //            bool hasChild = billingProvider.Subscribers.Count > 0;

        //            #region Billing Provider HL

        //            #region Billing Provider HL > Billing Provider Name

        //            #endregion Billing Provider HL > Billing Provider Name

        //            #endregion Billing Provider HL

        //            #region Subscriber HL

        //            if (billingProvider.Subscribers.Count > 0)
        //            {
        //                foreach (var subscriber in billingProvider.Subscribers)
        //                {
        //                    AddCsvCell(clssw, subscriber.PayerNameLastOrOrganizationName); //PrimaryInsuranceName
        //                    AddCsvCell(clssw, subscriber.PayerAddressInformation); //PrimaryInsuranceAddress1
        //                    AddCsvCell(clssw, ""); //PrimaryInsuranceAddress2
        //                    string payorCsz = string.Format("{0}, {1} {2}", subscriber.PayerCityName, subscriber.PayerStateOrProvinceCode, subscriber.PayerPostalCode);
        //                    AddCsvCell(clssw, payorCsz); //PrimaryInsuranceCSZ
        //                    AddCsvCell(clssw, ""); //Invoice No
        //                    AddCsvCell(clssw, ""); //PrimaryMedicare
        //                    AddCsvCell(clssw, ""); //PrimaryMedicaid
        //                    AddCsvCell(clssw, ""); //1 Champus
        //                    AddCsvCell(clssw, ""); //1 ChampVa
        //                    AddCsvCell(clssw, ""); //1 Group Health
        //                    AddCsvCell(clssw, ""); //1 FECA
        //                    AddCsvCell(clssw, ""); //PrimaryOther
        //                    AddCsvCell(clssw, ""); //PrimaryInsurancePolicy    TODO: HOW TO GET THIS??
        //                    string patietName = string.Format("{0}, {1}", subscriber.SubmitterNameLastOrOrganizationName, subscriber.SubmitterNameFirst);
        //                    AddCsvCell(clssw, patietName); //PatientName
        //                    DateTime bday = DateTime.ParseExact(subscriber.SubmitterDateTimePeriod, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
        //                    AddCsvCell(clssw, Convert.ToString(bday, CultureInfo.InvariantCulture)); //PatientDOB
        //                    AddCsvCell(clssw, subscriber.SubmitterGenderCode == "M" ? "" : "X"); //PatientGenderMale
        //                    AddCsvCell(clssw, subscriber.SubmitterGenderCode == "F" ? "X" : ""); //PatientGenderFeMale

        //                    AddCsvCell(clssw, patietName); //InsuredName
        //                    AddCsvCell(clssw, subscriber.SubmitterAddressInformation); //PatientAddress
        //                    AddCsvCell(clssw, "X"); //InsuredRelationshipSelf
        //                    AddCsvCell(clssw, ""); //InsuredRelationshipSpouse
        //                    AddCsvCell(clssw, ""); //InsuredRelationshipChild
        //                    AddCsvCell(clssw, ""); //InsuredRelationshipOther
        //                    AddCsvCell(clssw, subscriber.SubmitterAddressInformation); //InsuredAddress
        //                    AddCsvCell(clssw, subscriber.SubmitterCityName); //PatientCity
        //                    AddCsvCell(clssw, subscriber.SubmitterStateOrProvinceCode); //PatientState
        //                    AddCsvCell(clssw, ""); //Single
        //                    AddCsvCell(clssw, ""); //Married
        //                    AddCsvCell(clssw, ""); //Field29
        //                    AddCsvCell(clssw, subscriber.SubmitterCityName); //InsuredCity
        //                    AddCsvCell(clssw, subscriber.SubmitterStateOrProvinceCode); //InsuredState
        //                    AddCsvCell(clssw, subscriber.SubmitterPostalCode); //PatientZip
        //                    AddCsvCell(clssw, ""); //PatientPhone  //TODO: ADD THIS 
        //                    AddCsvCell(clssw, ""); //Employed
        //                    AddCsvCell(clssw, ""); //Full-Time
        //                    AddCsvCell(clssw, ""); //Part-Time
        //                    AddCsvCell(clssw, subscriber.SubmitterPostalCode); //InsuredZip
        //                    AddCsvCell(clssw, ""); //InsuredPhone  //TODO: ADD THIS 
        //                    AddCsvCell(clssw, ""); //9 OTHER INSURED'S NAME (Last Name, FirstName)
        //                    AddCsvCell(clssw, ""); //PrimaryInsuranceGroup //TODO: What is this, we are not taking any information like this
        //                    AddCsvCell(clssw, ""); //a OTHER INSURED'S POLICY OR GROUP NUMBE
        //                    AddCsvCell(clssw, ""); //10a Employment Y
        //                    AddCsvCell(clssw, ""); //10a Employment N
        //                    AddCsvCell(clssw, Convert.ToString(bday, CultureInfo.InvariantCulture)); //InsuredDOB
        //                    AddCsvCell(clssw, subscriber.SubmitterGenderCode == "M" ? "" : "X"); //InsuredGenderMale
        //                    AddCsvCell(clssw, subscriber.SubmitterGenderCode == "F" ? "X" : ""); //InsuredGenderFemale
        //                    AddCsvCell(clssw, ""); //b Other Insured's Date of Birth
        //                    AddCsvCell(clssw, ""); //Other Insured's Sex M
        //                    AddCsvCell(clssw, ""); //Other Insured's Sex F
        //                    AddCsvCell(clssw, ""); //10b Auto Accident Y
        //                    AddCsvCell(clssw, "X"); //10b Auto Accident N
        //                    AddCsvCell(clssw, ""); //10b State
        //                    AddCsvCell(clssw, ""); //bEmployer's Name or School name
        //                    AddCsvCell(clssw, ""); //c EMPLOYER'S NAME OR SCHOOL NAME
        //                    AddCsvCell(clssw, ""); //10c Other Accident Y
        //                    AddCsvCell(clssw, "X"); //10c Other Accident N

        //                    AddCsvCell(clssw, ""); //11c INS PLAN NAME OR PROGRAM NAME
        //                    AddCsvCell(clssw, ""); //d INSURANCE PLAN NAME OR PROGRAM NAME
        //                    AddCsvCell(clssw, ""); //10d RESERVED FOR LOCAL USE
        //                    AddCsvCell(clssw, ""); //11d Other Insurance Y
        //                    AddCsvCell(clssw, "X"); //11d Other Insurance N
        //                    AddCsvCell(clssw, "SOF"); //12 Sign  //TODO: What is this?
        //                    AddCsvCell(clssw, ""); //12 Date      //TODO: What date is this?
        //                    AddCsvCell(clssw, "SOF"); //13 Sign  //TODO: What is this?
        //                    AddCsvCell(clssw, ""); //DateOfInjury
        //                    AddCsvCell(clssw, ""); //15 First Date of similar illness
        //                    AddCsvCell(clssw, ""); //16 From
        //                    AddCsvCell(clssw, ""); //16 To
        //                    AddCsvCell(clssw, ""); //PhysicianUPIN
        //                    AddCsvCell(clssw, ""); //PhysiciantName
        //                    AddCsvCell(clssw, ""); //PhysicianNPI
        //                    AddCsvCell(clssw, ""); //18 From
        //                    AddCsvCell(clssw, ""); //18 To
        //                    AddCsvCell(clssw, ""); //CarolinaAccessCode
        //                    AddCsvCell(clssw, ""); //20 Yes
        //                    AddCsvCell(clssw, "X"); //20 No
        //                    AddCsvCell(clssw, ""); //20 $Charges



        //                    AddCsvCell(clssw, ""); //ICD9_1
        //                    AddCsvCell(clssw, ""); //ICD9_3
        //                    AddCsvCell(clssw, ""); //22 Medicaid resubmission code
        //                    AddCsvCell(clssw, ""); //22Original ref no
        //                    AddCsvCell(clssw, ""); //ICD9_2
        //                    AddCsvCell(clssw, ""); //ICD9_4
        //                    AddCsvCell(clssw, ""); //23 PRIOR AUTHORIZATION NUMBER

        //                    for (int i = 0; i < 6; i++)
        //                    {
        //                        AddCsvCell(clssw, ""); //Line 1 ID Qual.
        //                        AddCsvCell(clssw, ""); //Line 1 Provider ID
        //                        AddCsvCell(clssw, ""); //Line 1 DOS from
        //                        AddCsvCell(clssw, ""); //Line 1 DOS to
        //                        AddCsvCell(clssw, ""); //Line 1 POS
        //                        AddCsvCell(clssw, ""); //Line 1 EMG
        //                        AddCsvCell(clssw, ""); //Line 1 CPT/HCPCS
        //                        AddCsvCell(clssw, ""); //Line 1 Modifiers
        //                        AddCsvCell(clssw, ""); //Line 1 Diag code
        //                        AddCsvCell(clssw, ""); //Line 1 $Charges
        //                        AddCsvCell(clssw, ""); //Line 1 Units
        //                        AddCsvCell(clssw, ""); //Line 1 epsdt
        //                        AddCsvCell(clssw, ""); //Line 1 NPI    
        //                    }


        //                    AddCsvCell(clssw, model.SubmitterName.IdCodeQualifierEnum); //25 FEDERAL TAX ID
        //                    AddCsvCell(clssw, ""); //Box 25 ssn
        //                    AddCsvCell(clssw, "X"); //Box 25 ein
        //                    AddCsvCell(clssw, subscriber.SubmitterIdCodeQualifierEnum); //26 Patient account no
        //                    AddCsvCell(clssw, "X"); //AcceptAssignmentYes
        //                    AddCsvCell(clssw, ""); //AcceptAssignmentNo
        //                    AddCsvCell(clssw, ""); //28 Total Charges
        //                    AddCsvCell(clssw, "0"); //AmountPaid
        //                    AddCsvCell(clssw, ""); //30 Balance Due
        //                    AddCsvCell(clssw, ""); //Box 33 PHONE#
        //                    AddCsvCell(clssw, ""); //POS_FacilityName
        //                    AddCsvCell(clssw, ""); //Box 31 Signed
        //                    AddCsvCell(clssw, ""); //POS_FacilityAddress1
        //                    AddCsvCell(clssw, ""); //Box 33 line 2
        //                    AddCsvCell(clssw, ""); //Box 31 Date
        //                    AddCsvCell(clssw, ""); //POS_FacilityAddress2
        //                    AddCsvCell(clssw, ""); //Box 33 line 3
        //                    AddCsvCell(clssw, ""); //Box 32 a.
        //                    AddCsvCell(clssw, ""); //POS_FacilityCSZ
        //                    AddCsvCell(clssw, ""); //Box 33 NPI
        //                    AddCsvCell(clssw, ""); //Box 33 PIN.
        //                    AddCsvCell(clssw, ""); //PhysicianAddress
        //                    AddCsvCell(clssw, ""); //PhysicianCity
        //                    AddCsvCell(clssw, ""); //PhysicianState
        //                    AddCsvCell(clssw, ""); //PhysicianZip
        //                    AddCsvCell(clssw, ""); //PhysicianPhone
        //                    AddCsvCell(clssw, ""); //PrimaryInsurancePhone
        //                    AddCsvCell(clssw, ""); //SecondaryInsuranceName
        //                    AddCsvCell(clssw, ""); //SecondaryMedicare
        //                    AddCsvCell(clssw, ""); //SecondaryMedicaid
        //                    AddCsvCell(clssw, ""); //SecondaryOther
        //                    AddCsvCell(clssw, ""); //SecondaryInsurancePolicy
        //                    AddCsvCell(clssw, ""); //SecondaryInsuranceGroup
        //                    AddCsvCell(clssw, ""); //SecondaryInsuranceAddress1
        //                    AddCsvCell(clssw, ""); //SecondaryInsuranceAddress2
        //                    AddCsvCell(clssw, ""); //SecondaryInsuranceCSZ
        //                    AddCsvCell(clssw, ""); //SecondaryInsurancePhone
        //                    AddCsvCell(clssw, ""); //SecondaryInsuredRelationshipSelf
        //                    AddCsvCell(clssw, ""); //SecondaryInsuredRelationshipSpouse
        //                    AddCsvCell(clssw, ""); //SecondaryInsuredRelationshipChild
        //                    AddCsvCell(clssw, ""); //SecondaryInsuredRelationshipOther
        //                    AddCsvCell(clssw, ""); //SecondaryInsuredName
        //                    AddCsvCell(clssw, ""); //SecondaryInsuredGenderMale
        //                    AddCsvCell(clssw, ""); //SecondaryInsuredGenderFemale
        //                    AddCsvCell(clssw, ""); //SecondaryInsuredDOB
        //                    AddCsvCell(clssw, ""); //SecondaryInsuredAddress
        //                    AddCsvCell(clssw, ""); //SecondaryInsuredInsuredCity
        //                    AddCsvCell(clssw, ""); //SecondaryInsuredState
        //                    AddCsvCell(clssw, ""); //SecondaryInsuredZip
        //                    AddCsvCell(clssw, ""); //SecondaryInsuredPhone
        //                    AddCsvCell(clssw, ""); //SalesmanName
        //                    AddCsvCell(clssw, ""); //Line1Description
        //                    AddCsvCell(clssw, ""); //Line2Description
        //                    AddCsvCell(clssw, ""); //Line3Description
        //                    AddCsvCell(clssw, ""); //Line4Description
        //                    AddCsvCell(clssw, ""); //Line5Description
        //                    AddCsvCell(clssw, ""); //Line6Description
        //                    AddCsvCell(clssw, ""); //CheckNumber
        //                    AddCsvCell(clssw, ""); //DatePosted
        //                    AddCsvCell(clssw, ""); //Taxonomy
        //                    clssw.Write(clssw.NewLine);

        //                }
        //            }

        //            #endregion Subscriber HL
        //        }
        //    }



        //    return "";

        //    #endregion Generate 837 File
        //}

        public static string RemoveLineEndings(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }
            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();

            return value.Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty)
                        .Replace(lineSeparator, string.Empty).Replace(paragraphSeparator, string.Empty);//.Replace("\\s+", "");
        }

        #endregion

        #region 837 SUPPORTING METHODS

        public CommunicationNumberQualifer GetCommunicationNumberQualifer(string value)
        {
            switch (value)
            {
                case "EM": return CommunicationNumberQualifer.ElectronicMail;
                case "EX": return CommunicationNumberQualifer.TelephoneExtension;
                case "FX": return CommunicationNumberQualifer.Facsimile;
                case "TE": return CommunicationNumberQualifer.Telephone;
                default: return CommunicationNumberQualifer.Undefined;
            }
        }
        public EntityTypeQualifier GetEntityTypeQualifier(string value)
        {
            return value == "1" ? EntityTypeQualifier.Person : EntityTypeQualifier.NonPersonEntity;
        }
        public Gender GetGender(string value)
        {
            switch (value)
            {
                case "F": return Gender.Female;
                case "M": return Gender.Male;
                case "U": return Gender.Unknown;
                default: return Gender.Undefined;
            }
        }
        public bool GetBoolean(string value)
        {
            return value == "Y";
        }
        public DTPQualifier GetDtpQualifier(string value)
        {
            switch (value)
            {
                case "472": return DTPQualifier.Service;
                case "307": return DTPQualifier.Eligibility;
                case "434": return DTPQualifier.Statement;
                case "435": return DTPQualifier.Admission;
                default: return DTPQualifier.Service;
            }
        }
        public DTPFormatQualifier GetDtpFormatQualifier(string value)
        {
            switch (value)
            {
                case "RD8": return DTPFormatQualifier.CCYYMMDD_CCYYMMDD;
                case "D8": return DTPFormatQualifier.CCYYMMDD;
                case "DT": return DTPFormatQualifier.CCYYMMDDHHMM;
                default: return DTPFormatQualifier.CCYYMMDD_CCYYMMDD;
            }
        }
        public DateTimePeriod GetDtpDate(string dtpFormateQualifier, string value)
        {
            DateTimePeriod dateTimePeriod;
            DateTime startDate, endDate;
            switch (dtpFormateQualifier)
            {
                case "RD8":
                    startDate = DateTime.ParseExact(value.Substring(0, 8), "yyyyMMdd", null);
                    endDate = DateTime.ParseExact(value.Substring(9, 8), "yyyyMMdd", null);
                    dateTimePeriod = new DateTimePeriod(startDate, endDate);
                    break;
                case "D8":
                    startDate = DateTime.ParseExact(value.Substring(0, 8), "yyyyMMdd", null);
                    dateTimePeriod = new DateTimePeriod(startDate);
                    break;
                case "DT":
                    startDate = DateTime.ParseExact(value.Substring(0, 12), "yyyyMMddHHmm", null);
                    dateTimePeriod = new DateTimePeriod(startDate);
                    dateTimePeriod.IsWithTime = true;
                    break;
                default:
                    startDate = DateTime.ParseExact(value.Substring(0, 8), "yyyyMMdd", null);
                    endDate = DateTime.ParseExact(value.Substring(9, 8), "yyyyMMdd", null);
                    dateTimePeriod = new DateTimePeriod(startDate, endDate);
                    break;
            }
            return dateTimePeriod;
        }

        public StreamWriter AddCsvCell(StreamWriter clssw, string value, string seprator = ",", bool addSeprator = true)
        {
            seprator = addSeprator ? seprator : "";
            clssw.Write(value + seprator);
            return clssw;
        }
        #endregion

    }
}
