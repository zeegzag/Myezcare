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
using OopFactory.Edi835Parser.Models;
using OopFactory.X12.Parsing.Model;
using OopFactory.X12.Parsing.Model.Typed;
using TypedLoopNM1 = OopFactory.X12.Parsing.Model.Typed.TypedLoopNM1;

namespace EDI_837_835_HCCP
{
    public class Edi270
    {
        #region CORE METHODS

        public string GenerateEdi270File(Edi270Model model, string fileServerPath, string fileName)
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
            group.ApplicationSendersCode = model.FunctionalGroupHeader.ApplicationSenderCode.Trim();
            group.ApplicationReceiversCode = model.FunctionalGroupHeader.ApplicationReceiverCode.Trim();
            group.ResponsibleAgencyCode = model.FunctionalGroupHeader.ResponsibleAgencyCode;
            //group.Date = Convert.ToDateTime("12/31/1999");
            //group.ControlNumber = 1;
            group.SetElement(5, model.InterchangeControlHeader.InterchangeTime);

            var transaction = group.AddTransaction(model.TransactionSetHeader.TransactionSetIdentifier, string.Format("{0:0000}", controlNumber));
            transaction.SetElement(3, model.FunctionalGroupHeader.VersionOrReleaseOrNo);

            var bhtSegment = transaction.AddSegment(new TypedSegmentBHT());
            bhtSegment.BHT01_HierarchicalStructureCode = model.BeginningOfHierarchicalTransaction.HierarchicalStructureCode;
            bhtSegment.BHT02_TransactionSetPurposeCode = model.BeginningOfHierarchicalTransaction.TransactionSetPurposeCode;
            //bhtSegment.BHT03_ReferenceIdentification = model.BeginningOfHierarchicalTransaction.ReferenceIdentification;
            bhtSegment.BHT04_Date = DateTime.Now;
            bhtSegment.BHT05_Time = DateTime.Now.ToString("HHmm");
            //bhtSegment.BHT06_TransactionTypeCode = model.BeginningOfHierarchicalTransaction.TransactionTypeCode;



            if (model.HlLevel270Model.Count > 0)
            {
                int hlLoop = 0;
                foreach (var hlLevel270 in model.HlLevel270Model)
                {
                    bool hasChild = hlLevel270.Subscribers.Count > 0;

                    #region Information Source HL

                    var providerInformationSource = transaction.AddHLoop(Convert.ToString(++hlLoop), hlLevel270.InformationSource.HeirarchicalLevelCode, hasChild); //*********HL 1 ******
                    var informationSource = providerInformationSource.AddLoop(new TypedLoopNM1(hlLevel270.InformationSource.EntityIdentifierCode));
                    informationSource.NM101_EntityIdentifierCodeEnum = GetEntityIdentifierTypeQualifier(hlLevel270.InformationSource.EntityIdentifierCode);
                    informationSource.NM102_EntityTypeQualifier = GetEntityTypeQualifier(hlLevel270.InformationSource.EntityTypeQualifier);
                    informationSource.NM103_NameLastOrOrganizationName = hlLevel270.InformationSource.NameLastOrOrganizationName;
                    informationSource.NM108_IdCodeQualifier = hlLevel270.InformationSource.IdCodeQualifier;
                    informationSource.NM109_IdCode = hlLevel270.InformationSource.IdCodeQualifierEnum;

                    var providerInformationReceiver = providerInformationSource.AddHLoop(Convert.ToString(++hlLoop), hlLevel270.InformationReceiver.HeirarchicalLevelCode, hasChild); //*********HL 1 ******
                    var informationReceiver = providerInformationReceiver.AddLoop(new TypedLoopNM1(hlLevel270.InformationReceiver.EntityIdentifierCode));
                    informationReceiver.NM101_EntityIdentifierCodeEnum = GetEntityIdentifierTypeQualifier(hlLevel270.InformationReceiver.EntityIdentifierCode);
                    informationReceiver.NM102_EntityTypeQualifier = GetEntityTypeQualifier(hlLevel270.InformationReceiver.EntityTypeQualifier);
                    informationReceiver.NM103_NameLastOrOrganizationName = hlLevel270.InformationReceiver.NameLastOrOrganizationName;
                    informationReceiver.NM108_IdCodeQualifier = hlLevel270.InformationReceiver.IdCodeQualifier;
                    informationReceiver.NM109_IdCode = hlLevel270.InformationReceiver.IdCodeQualifierEnum;


                    #endregion Information Source HL



                    #region Subscriber HL

                    if (hlLevel270.Subscribers.Count > 0)
                    {
                        foreach (var subscriber in hlLevel270.Subscribers)
                        {
                            var subscriber2000BHLoop = providerInformationReceiver.AddHLoop(Convert.ToString(++hlLoop), subscriber.HeirarchicalLevelCode, false);  // **** HL 2  ******

                            #region Subscriber HL > TRN

                            //var segmentTrn = subscriber2000BHLoop.AddSegment(new TypedSegmentTRN());
                            //segmentTrn.TRN01_TraceTypeCode = subscriber.TRN01_TraceTypeCode;
                            //segmentTrn.TRN02_ReferenceIdentification02 = subscriber.TRN02_ReferenceIdentification02;
                            //segmentTrn.TRN03_CompanyIdentifier = subscriber.TRN03_CompanyIdentifier;
                            //segmentTrn.TRN04_ReferenceIdentification04 = subscriber.TRN04_ReferenceIdentification04;

                            #endregion Subscriber HL > SBR

                            #region Subscriber HL > Subscriber Name

                            var subscriberName2010BALoop = subscriber2000BHLoop.AddLoop(new TypedLoopNM1(subscriber.SubmitterEntityIdentifierCode));
                            subscriberName2010BALoop.NM102_EntityTypeQualifier = GetEntityTypeQualifier(subscriber.SubmitterEntityTypeQualifier);
                            subscriberName2010BALoop.NM103_NameLastOrOrganizationName = subscriber.SubmitterNameLastOrOrganizationName;
                            subscriberName2010BALoop.NM104_NameFirst = subscriber.SubmitterNameFirst;
                            subscriberName2010BALoop.NM108_IdCodeQualifier = subscriber.SubmitterIdCodeQualifier;
                            subscriberName2010BALoop.NM109_IdCode = subscriber.SubmitterIdCodeQualifierEnum;

                            var subscriber_DMGSegment = subscriberName2010BALoop.AddSegment(new TypedSegmentDMG());
                            subscriber_DMGSegment.DMG01_DateTimePeriodFormatQualifier = subscriber.SubmitterDateTimePeriodFormatQualifier;
                            subscriber_DMGSegment.DMG02_DateOfBirth = DateTime.ParseExact(subscriber.SubmitterDateTimePeriod, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                            subscriber_DMGSegment.DMG03_Gender = GetGender(subscriber.SubmitterGenderCode);

                            var subscriber_DtpSegment = subscriberName2010BALoop.AddSegment(new TypedSegmentDTP());
                            subscriber_DtpSegment.DTP01_DateTimeQualifier = GetDtpQualifier(subscriber.SubmitterDTPQualifier);
                            subscriber_DtpSegment.DTP02_DateTimePeriodFormatQualifier = GetDtpFormatQualifier(subscriber.SubmitterDTPFormatQualifier);
                            subscriber_DtpSegment.DTP03_Date = new DateTimePeriod(DateTime.Now.Date);
                            //DateTime startDate = DateTime.ParseExact(DateTime.Now.Date.ToString(), "yyyyMMdd", null);
                            //DateTime endDate = DateTime.ParseExact(DateTime.Now.Date.ToString(), "yyyyMMdd", null);
                            //subscriber_DtpSegment.DTP03_Date = new DateTimePeriod(startDate, endDate);

                            var subscriber_EQSegment = subscriberName2010BALoop.AddLoop(new TypedLoopEQ());
                            subscriber_EQSegment.EQ01_Eligibility = subscriber.SubmitterEligibility01;

                            #endregion Subscriber HL > Subscriber Name

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

        #region 270 SUPPORTING METHODS

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
            switch (value)
            {
                case "1": return EntityTypeQualifier.Person;
                case "2": return EntityTypeQualifier.NonPersonEntity;
                default: return EntityTypeQualifier.NonPersonEntity;
            }
        }

        public EntityIdentifierCode GetEntityIdentifierTypeQualifier(string value)
        {
            switch (value)
            {
                case "1P": return EntityIdentifierCode.Provider;
                case "PR": return EntityIdentifierCode.Payer;
                default: return EntityIdentifierCode.Provider;
            }
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
                case "291": return DTPQualifier.Plan;
                    
                case "307": return DTPQualifier.Eligibility;
                default: return DTPQualifier.Service;
            }
        }
        public DTPFormatQualifier GetDtpFormatQualifier(string value)
        {
            switch (value)
            {
                case "RD8": return DTPFormatQualifier.CCYYMMDD_CCYYMMDD;
                case "D8": return DTPFormatQualifier.CCYYMMDD;
                default: return DTPFormatQualifier.CCYYMMDD_CCYYMMDD;
            }
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
