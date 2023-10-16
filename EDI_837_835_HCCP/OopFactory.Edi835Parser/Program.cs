using System.Text;
using System.IO;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Xsl;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OopFactory.Edi835Parser.Models;
using OopFactory.X12.Parsing;
using OopFactory.X12.Parsing.Model;
using OopFactory.X12.Parsing.Model.Typed;
using OopFactory.X12.Validation;
using TypedLoopNM1 = OopFactory.X12.Parsing.Model.Typed.TypedLoopNM1;

namespace OopFactory.Edi835Parser
{
    public class Program
    {
        static void Main(string[] args)
        {
            //source: https://x12parser.codeplex.com/wikipage?title=Creating%20a%20flat%20file%20from%20the%20X12%20xml%20using%20XSLT%20and%20XslCompiledTransform

            //string fileName = @"D:\Projects\Zarepath\trunk\Zarephath\01. Project Management\01. Planning\03. Customer Supplied Items\837-835\CAZ_2016.06.01_2016.06.15_216.txt";
            //FileStream fstream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            //var parser = new X12Parser();
            //Interchange interchange = parser.Parse(fstream);
            //string x12 = interchange.SerializeToX12(true);

            // File837();
            //Generate837File();
            Validate837File();

            #region Parse 835 File

            /*
            Stream transformStream = null;

            //args[0] = @"D:\Desktop\Downloads\Chrome Downloads\Upto 2Aug16\OopFactory.Edi835Parser\OopFactory.Edi835Parser\OopFactory.Edi835Parser\Sample1.txt";

            Stream inputStream = new FileStream(args[0], FileMode.Open, FileAccess.Read);
            FileInfo outputFileInfo = new FileInfo(args[1]);

            if (outputFileInfo.Extension == ".xml" || outputFileInfo.Extension == ".xlsx")
                transformStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("OopFactory.Edi835Parser.Transformations.X12-835-XML-to-Excel-XML.xslt");
            else
                transformStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("OopFactory.Edi835Parser.Transformations.X12-835-XML-to-CSV.xslt");



            X12Parser parser = new X12Parser();
            Interchange interchange = parser.Parse(inputStream);
            interchange.SerializeToX12(true);
            string xml = interchange.Serialize();

            var transform = new XslCompiledTransform();
            transform.Load(XmlReader.Create(transformStream));
            XsltArgumentList arguments = new XsltArgumentList();
            arguments.AddParam("filename", "", new FileInfo(args[0]).Name);

            MemoryStream mstream = new MemoryStream();
            transform.Transform(XmlReader.Create(new StringReader(xml)), arguments, mstream);
            mstream.Flush();
            mstream.Position = 0;
            string content = new StreamReader(mstream).ReadToEnd();
            {
                string filename = String.Format("{0}\\{1}{2}", outputFileInfo.DirectoryName, outputFileInfo.Name.Replace(outputFileInfo.Extension, ""), outputFileInfo.Extension);
                using (Stream outputStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter writer = new StreamWriter(outputStream))
                    {
                        writer.Write(content);
                        writer.Close();
                    }
                    outputStream.Close();
                }

                List<Test> list = new List<Test>();
                using (CsvReader reader = new CsvReader(filename))
                {
                    int index = 0;
                    foreach (string[] vals in reader.RowEnumerator)
                    {
                        // Skip first 1 row
                        if (index >= 1)
                        {
                            var i = 0;
                                Test thisItem = new Test
                                {
                                    Filename = vals[i++],
                                    CheckSequence = vals[i++],
                                    PayerName = vals[i++],
                                    PayeeName = vals[i++],
                                    PayeeID = vals[i++],
                                    CheckDate = vals[i++]
                                };
                            list.Add(thisItem);
                        }
                        index++;
                    }
                }
            }
            */

            #endregion Parse 835 File

            /*
            string[] csvs = content.Split(new char[] {'|'}, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < csvs.Length; i++)
            {
                string filename = String.Format("{0}\\{1}_{3:000}{2}", outputFileInfo.DirectoryName, outputFileInfo.Name.Replace(outputFileInfo.Extension,""), outputFileInfo.Extension, i + 1);
                using (Stream outputStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter writer = new StreamWriter(outputStream))
                    {
                        writer.Write(csvs[i]);
                        writer.Close();
                    }
                    outputStream.Close();
                }
            }
             */
        }

        /// <summary>
        /// Following is the test code for generating 837 file.
        /// </summary>
        public static void File837()
        {
            #region Create 837 File

            var test123 = "";

            var message = new Interchange(Convert.ToDateTime("01/01/03"), 000905, false)
            {
                InterchangeSenderIdQualifier = "ZZ",
                InterchangeSenderId = "SUBMITTERS.ID",
                InterchangeReceiverIdQualifier = "ZZ",
                InterchangeReceiverId = "RECEIVERS.ID",
                SecurityInfo = "SECRET",
                SecurityInfoQualifier = "01",
            };
            message.SetElement(12, "00501");
            message.SetElement(10, "1253");
            message.SetElement(11, "^");

            var group = message.AddFunctionGroup("HC", DateTime.Now, 1, "005010X222");
            group.ApplicationSendersCode = "SENDER CODE";
            group.ApplicationReceiversCode = "RECEIVER CODE";
            group.Date = Convert.ToDateTime("12/31/1999");
            group.ControlNumber = 1;
            group.SetElement(5, "0802");


            var transaction = group.AddTransaction("837", "0021");
            transaction.SetElement(2, "0021");
            transaction.SetElement(3, "005010X222");

            var bhtSegment = transaction.AddSegment(new TypedSegmentBHT());
            bhtSegment.BHT01_HierarchicalStructureCode = "0019";
            bhtSegment.BHT02_TransactionSetPurposeCode = "00";
            bhtSegment.BHT03_ReferenceIdentification = "244579";
            bhtSegment.BHT04_Date = DateTime.Parse("2006-10-15");
            bhtSegment.BHT05_Time = "1023";
            bhtSegment.BHT06_TransactionTypeCode = "CH";

            var submitterLoop = transaction.AddLoop(new TypedLoopNM1("41")); //submitter identifier code
            submitterLoop.NM102_EntityTypeQualifier = EntityTypeQualifier.NonPersonEntity;
            submitterLoop.NM103_NameLastOrOrganizationName = "PREMIER BILLING SERVICE";
            submitterLoop.NM104_NameFirst = "";
            submitterLoop.NM109_IdCode = "TGJ23";
            submitterLoop.NM108_IdCodeQualifier = "46";

            var perSegment = submitterLoop.AddSegment(new TypedSegmentPER());
            perSegment.PER01_ContactFunctionCode = "IC"; //information contact function code
            perSegment.PER02_Name = "JERRY";
            perSegment.PER03_CommunicationNumberQualifier = CommunicationNumberQualifer.Telephone;
            perSegment.PER04_CommunicationNumber = "3055552222";
            perSegment.PER05_CommunicationNumberQualifier = CommunicationNumberQualifer.TelephoneExtension;
            perSegment.PER06_CommunicationNumber = "231";


            var submitterLoop2 = transaction.AddLoop(new TypedLoopNM1("40"));
            submitterLoop2.NM102_EntityTypeQualifier = EntityTypeQualifier.NonPersonEntity;
            submitterLoop2.NM103_NameLastOrOrganizationName = "KEY INSURANCE COMPANY";
            submitterLoop2.NM104_NameFirst = "";
            submitterLoop2.NM109_IdCode = "66783JJT";
            submitterLoop2.NM108_IdCodeQualifier = "46";

            #region Billing Provider HL

            var provider2000AHLoop = transaction.AddHLoop("1", "20", true); //*********HL 1 ******

            //TODO: This segment is not created in our files.
            #region Billing Provider HL > PRV
            var prvSegment = provider2000AHLoop.AddSegment(new TypedSegmentPRV()); //Specialty Segment
            prvSegment.PRV01_ProviderCode = "BI";
            prvSegment.PRV02_ReferenceIdQualifier = "PXC";
            prvSegment.PRV03_ProviderTaxonomyCode = "203BF0100Y";
            #endregion Billing Provider HL > PRV

            #region Billing Provider HL > Billing Provider Name

            var provider2010ACLoop = provider2000AHLoop.AddLoop(new TypedLoopNM1("85"));
            provider2010ACLoop.NM102_EntityTypeQualifier = EntityTypeQualifier.NonPersonEntity;
            provider2010ACLoop.NM103_NameLastOrOrganizationName = "BEN KILDARE SERVICE";
            provider2010ACLoop.NM109_IdCode = "9876543210";
            provider2010ACLoop.NM108_IdCodeQualifier = "XX";

            var provider2010AC_N3Segment = provider2010ACLoop.AddSegment(new TypedSegmentN3());
            provider2010AC_N3Segment.N301_AddressInformation = "234 SEAWAY ST";

            var provider2010AC_N4Segment = provider2010ACLoop.AddSegment(new TypedSegmentN4());
            provider2010AC_N4Segment.N401_CityName = "MIAMI";
            provider2010AC_N4Segment.N402_StateOrProvinceCode = "FL";
            provider2010AC_N4Segment.N403_PostalCode = "33111";

            var provider2010AC_REFSegment = provider2010ACLoop.AddSegment(new TypedSegmentREF());
            provider2010AC_REFSegment.REF01_ReferenceIdQualifier = "EI";
            provider2010AC_REFSegment.REF02_ReferenceId = "587654321";

            #endregion Billing Provider HL > Billing Provider Name

            //TODO: This segment is not created in our files.
            #region Billing Provider HL > Pay-To Address Name

            var provider2010ACLoop2 = provider2000AHLoop.AddLoop(new TypedLoopNM1("87"));
            provider2010ACLoop2.NM102_EntityTypeQualifier = EntityTypeQualifier.NonPersonEntity;

            var provider2010AC_N3Segment2 = provider2010ACLoop2.AddSegment(new TypedSegmentN3());
            provider2010AC_N3Segment2.N301_AddressInformation = "2345 OCEAN BLVD";

            var provider2010AC_N4Segment2 = provider2010ACLoop2.AddSegment(new TypedSegmentN4());
            provider2010AC_N4Segment2.N401_CityName = "MAIMI";  // MISSPELLED IN COMPARETO DOC
            provider2010AC_N4Segment2.N402_StateOrProvinceCode = "FL";
            provider2010AC_N4Segment2.N403_PostalCode = "33111";

            #endregion Billing Provider HL > Pay-To Address Name

            #endregion Billing Provider HL

            #region Subscriber HL

            var subscriber2000BHLoop = provider2000AHLoop.AddHLoop("2", "22", true);  // **** HL 2  ******

            #region Subscriber HL > SBR

            var segmentSBR = subscriber2000BHLoop.AddSegment(new TypedSegmentSBR());
            segmentSBR.SBR01_PayerResponsibilitySequenceNumberCode = "P";
            segmentSBR.SBR03_PolicyOrGroupNumber = "2222-SJ";
            segmentSBR.SBR09_ClaimFilingIndicatorCode = "CI";

            #endregion Subscriber HL > SBR

            #region Subscriber HL > Subscriber Name

            var subscriberName2010BALoop = subscriber2000BHLoop.AddLoop(new TypedLoopNM1("IL"));
            subscriberName2010BALoop.NM102_EntityTypeQualifier = EntityTypeQualifier.Person;
            subscriberName2010BALoop.NM104_NameFirst = "JANE";
            subscriberName2010BALoop.NM103_NameLastOrOrganizationName = "SMITH";
            subscriberName2010BALoop.NM109_IdCode = "JS00111223333";
            subscriberName2010BALoop.NM108_IdCodeQualifier = "MI";

            var subscriber_DMGSegment = subscriberName2010BALoop.AddSegment(new TypedSegmentDMG());
            subscriber_DMGSegment.DMG01_DateTimePeriodFormatQualifier = "D8";
            subscriber_DMGSegment.DMG02_DateOfBirth = DateTime.Parse("5/1/1943");
            subscriber_DMGSegment.DMG03_Gender = Gender.Female;

            #endregion Subscriber HL > Subscriber Name

            #region Subscriber HL > Payer Name

            var subscriberName2010BALoop2 = subscriber2000BHLoop.AddLoop(new TypedLoopNM1("PR"));
            subscriberName2010BALoop2.NM102_EntityTypeQualifier = EntityTypeQualifier.NonPersonEntity;
            subscriberName2010BALoop2.NM103_NameLastOrOrganizationName = "KEY INSURANCE COMPANY";
            subscriberName2010BALoop2.NM108_IdCodeQualifier = "PI";
            subscriberName2010BALoop2.NM109_IdCode = "999996666";

            var refSegment2 = subscriberName2010BALoop2.AddSegment(new TypedSegmentREF());
            refSegment2.REF01_ReferenceIdQualifier = "G2";
            refSegment2.REF02_ReferenceId = "KA6663";

            #endregion Subscriber HL > Payer Name

            #endregion Subscriber HL

            //TODO: We dont have this section in our files.
            #region Patient HL

            var HL3Loop = subscriber2000BHLoop.AddHLoop("3", "23", false);   // **** HL 3  ******

            var HL3PATSegment = HL3Loop.AddSegment(new TypedSegmentPAT());
            HL3PATSegment.PAT01_IndividualRelationshipCode = "19";

            var HL3NM1Segment = HL3Loop.AddLoop(new TypedLoopNM1("QC"));
            HL3NM1Segment.NM102_EntityTypeQualifier = EntityTypeQualifier.Person;
            HL3NM1Segment.NM104_NameFirst = "TED";
            HL3NM1Segment.NM103_NameLastOrOrganizationName = "SMITH";


            // add N3 and N4 segments under the above NM1 loop

            var HL3NM1_N3_Segment = HL3NM1Segment.AddSegment(new TypedSegmentN3());
            HL3NM1_N3_Segment.N301_AddressInformation = "236 N MAIN ST";

            var HL3NM1_N4_Segment = HL3NM1Segment.AddSegment(new TypedSegmentN4());
            HL3NM1_N4_Segment.N401_CityName = "MIAMI";
            HL3NM1_N4_Segment.N402_StateOrProvinceCode = "FL";
            HL3NM1_N4_Segment.N403_PostalCode = "33413";

            var HL3NM1_DMG_Segment = HL3NM1Segment.AddSegment(new TypedSegmentDMG());
            HL3NM1_DMG_Segment.DMG01_DateTimePeriodFormatQualifier = "D8";
            HL3NM1_DMG_Segment.DMG02_DateOfBirth = Convert.ToDateTime("5/1/1973");
            HL3NM1_DMG_Segment.DMG03_Gender = Gender.Male;

            #endregion Patient HL

            #region Claim Information > CLM

            var claim2300Loop = HL3Loop.AddLoop(new TypedLoopCLM());
            claim2300Loop.CLM01_PatientControlNumber = "26463774";
            claim2300Loop.CLM02_TotalClaimChargeAmount = Convert.ToDecimal(100);
            claim2300Loop.CLM05._1_FacilityCodeValue = "11";
            claim2300Loop.CLM05._2_FacilityCodeQualifier = "B";
            claim2300Loop.CLM05._3_ClaimFrequencyTypeCode = "1";
            claim2300Loop.CLM06_ProviderOrSupplierSignatureIndicator = true;
            claim2300Loop.CLM07_ProviderAcceptAssignmentCode = "A";
            claim2300Loop.CLM08_BenefitsAssignmentCerficationIndicator = "Y";
            claim2300Loop.CLM09_ReleaseOfInformationCode = "I";

            #endregion Claim Information > CLM

            #region Claim Information > Reference > REF

            var refSegment = claim2300Loop.AddSegment(new TypedSegmentREF());
            refSegment.REF01_ReferenceIdQualifier = "D9";
            refSegment.REF02_ReferenceId = "17312345600006351";

            #endregion Claim Information > Reference > REF

            #region Claim Information > HI

            var hiSegment = claim2300Loop.AddSegment(new TypedSegmentHI());
            hiSegment.HI01_HealthCareCodeInformation = "BK:0340";
            hiSegment.HI02_HealthCareCodeInformation = "BF:V7389";

            #endregion Claim Information > HI

            #region Claim Information > Service Line Number

            #region Claim Information > Service Line Number > LX

            var lxLoop = claim2300Loop.AddLoop(new TypedLoopLX("LX"));
            lxLoop.LX01_AssignedNumber = "1";

            #endregion Claim Information > Service Line Number > LX

            #region Claim Information > Service Line Number > SV1

            var sv1Segment = lxLoop.AddSegment(new TypedSegmentSV1());
            sv1Segment.SV101_CompositeMedicalProcedure = "HC:99213";
            sv1Segment.SV102_MonetaryAmount = "40";
            sv1Segment.SV103_UnitBasisMeasCode = "UN";
            sv1Segment.SV104_Quantity = "1";
            sv1Segment.SV107_CompDiagCodePoint = "1";

            #endregion Claim Information > Service Line Number > SV1

            #region Claim Information > Service Line Number > Dates > DTP

            var dtpSegment = lxLoop.AddSegment(new TypedSegmentDTP());
            dtpSegment.DTP01_DateTimeQualifier = DTPQualifier.Service;
            dtpSegment.DTP02_DateTimePeriodFormatQualifier = DTPFormatQualifier.CCYYMMDD;
            DateTime theDate = DateTime.ParseExact("20061003", "yyyyMMdd", null);
            dtpSegment.DTP03_Date = new DateTimePeriod(theDate);

            #endregion Claim Information > Service Line Number > Dates > DTP

            #endregion Claim Information > Service Line Number

            #region Claim Information > Service Line Number

            var lxLoop2 = claim2300Loop.AddLoop(new TypedLoopLX("LX"));
            lxLoop2.LX01_AssignedNumber = "2";

            var sv1Segment2 = lxLoop2.AddSegment(new TypedSegmentSV1());
            sv1Segment2.SV101_CompositeMedicalProcedure = "HC:87070";
            sv1Segment2.SV102_MonetaryAmount = "15";
            sv1Segment2.SV103_UnitBasisMeasCode = "UN";
            sv1Segment2.SV104_Quantity = "1";
            sv1Segment2.SV107_CompDiagCodePoint = "1";

            var dtpSegment2 = lxLoop2.AddSegment(new TypedSegmentDTP());
            dtpSegment2.DTP01_DateTimeQualifier = DTPQualifier.Service;
            dtpSegment2.DTP02_DateTimePeriodFormatQualifier = DTPFormatQualifier.CCYYMMDD;
            DateTime theDate2 = DateTime.ParseExact("20061003", "yyyyMMdd", null);
            dtpSegment2.DTP03_Date = new DateTimePeriod(theDate2);

            #endregion Claim Information > Service Line Number

            #region Claim Information > Service Line Number

            var lxLoop3 = claim2300Loop.AddLoop(new TypedLoopLX("LX"));
            lxLoop3.LX01_AssignedNumber = "3";

            var sv1Segment3 = lxLoop3.AddSegment(new TypedSegmentSV1());
            sv1Segment3.SV101_CompositeMedicalProcedure = "HC:99214";
            sv1Segment3.SV102_MonetaryAmount = "35";
            sv1Segment3.SV103_UnitBasisMeasCode = "UN";
            sv1Segment3.SV104_Quantity = "1";
            sv1Segment3.SV107_CompDiagCodePoint = "2";

            var dtpSegment3 = lxLoop3.AddSegment(new TypedSegmentDTP());
            dtpSegment3.DTP01_DateTimeQualifier = DTPQualifier.Service;
            dtpSegment3.DTP02_DateTimePeriodFormatQualifier = DTPFormatQualifier.CCYYMMDD;
            DateTime theDate3 = DateTime.ParseExact("20061010", "yyyyMMdd", null);
            dtpSegment3.DTP03_Date = new DateTimePeriod(theDate3);

            #endregion Claim Information > Service Line Number

            #region Claim Information > Service Line Number

            var lxLoop4 = claim2300Loop.AddLoop(new TypedLoopLX("LX"));
            lxLoop4.LX01_AssignedNumber = "4";

            var sv1Segment4 = lxLoop4.AddSegment(new TypedSegmentSV1());
            sv1Segment4.SV101_CompositeMedicalProcedure = "HC:86663";
            sv1Segment4.SV102_MonetaryAmount = "10";
            sv1Segment4.SV103_UnitBasisMeasCode = "UN";
            sv1Segment4.SV104_Quantity = "1";
            sv1Segment4.SV107_CompDiagCodePoint = "2";

            var dtpSegment4 = lxLoop4.AddSegment(new TypedSegmentDTP());
            dtpSegment4.DTP01_DateTimeQualifier = DTPQualifier.Service;
            dtpSegment4.DTP02_DateTimePeriodFormatQualifier = DTPFormatQualifier.CCYYMMDD_CCYYMMDD;
            DateTime theDate4 = DateTime.ParseExact("20061010", "yyyyMMdd", null);
            dtpSegment4.DTP03_Date = new DateTimePeriod(theDate4, DateTime.ParseExact("20061025", "yyyyMMdd", null));

            #endregion Claim Information > Service Line Number

            //var x12 = message.SerializeToX12(true);
            var x12 = message.Serialize(true);
            //Assert.AreEqual(new StreamReader(Extensions.GetEdi("INS._837P._5010.Example1_HealthInsurance.txt")).ReadToEnd(), message.SerializeToX12(true));

            #endregion Create 837 File
        }

        /// <summary>
        /// The following method will generate 837 file in the folder /Files/837. Model is needed of type CreateModel to generate 837 file.
        /// </summary>
        public static void Generate837File()
        {
            #region Create Model

            int controlNumber = 80;

            CreateModel model = new CreateModel();
            
            model.InterchangeControlHeader.AuthorizationInformationQualifier = "00";
            model.InterchangeControlHeader.AuthorizationInformation = "          ";
            model.InterchangeControlHeader.SecurityInformationQualifier = "00";
            model.InterchangeControlHeader.SecurityInformation = "          ";
            model.InterchangeControlHeader.InterchangeSenderIdQualifier = "30";
            model.InterchangeControlHeader.InterchangeSenderId = "412111326      ";
            model.InterchangeControlHeader.InterchangeReceiverIdQualifier = "30";
            model.InterchangeControlHeader.InterchangeReceiverId = "752524263      ";
            model.InterchangeControlHeader.InterchangeDate = "160808";
            model.InterchangeControlHeader.InterchangeTime = "0312";
            model.InterchangeControlHeader.RepetitionSeparator = "^";
            model.InterchangeControlHeader.InterchangeControlVersionNumber = "00501";
            model.InterchangeControlHeader.InterchangeControlNumber = string.Format("{0:000000000}", controlNumber);
            model.InterchangeControlHeader.AcknowledgementRequired = "0";
            model.InterchangeControlHeader.UsageIndicator = "P";
            //model.InterchangeControlHeader.ComponentElementSeparator = ":";

            model.FunctionalGroupHeader.FunctionalIdentifierCode = "HC";
            //model.FunctionalGroupHeader.ApplicationSenderCode = "412111326";
            //model.FunctionalGroupHeader.ApplicationReceiverCode  = "752524263";
            //model.FunctionalGroupHeader.Date  = "20160808";
            //model.FunctionalGroupHeader.Time = "0312";
            //model.FunctionalGroupHeader.GroupControlNumber = "80";
            model.FunctionalGroupHeader.ResponsibleAgencyCode = "X";
            model.FunctionalGroupHeader.VersionOrReleaseOrNo = "005010X222A1";

            model.TransactionSetHeader.TransactionSetIdentifier = "837";
            //model.TransactionSetHeader.TransactionSetControlNumber = "0080";
            //model.TransactionSetHeader.ImplementationConventionReference = "005010X222A1";

            model.BeginningOfHierarchicalTransaction.HierarchicalStructureCode = "0019";
            model.BeginningOfHierarchicalTransaction.TransactionSetPurposeCode = "00";
            model.BeginningOfHierarchicalTransaction.ReferenceIdentification = "000000222";
            model.BeginningOfHierarchicalTransaction.Date = "20160805";
            model.BeginningOfHierarchicalTransaction.InterchangeIdQualifier = "105900";
            model.BeginningOfHierarchicalTransaction.TransactionTypeCode = "CH";

            model.SubmitterName.EntityIdentifierCodeEnum = "41";
            model.SubmitterName.EntityTypeQualifier = "2";
            model.SubmitterName.NameLastOrOrganizationName = "Zarephath";
            model.SubmitterName.IdCodeQualifier = "46";
            model.SubmitterName.IdCodeQualifierEnum = "412111326";

            model.SubmitterEDIContact.ContactFunctionCode = "IC";
            model.SubmitterEDIContact.Name = "Claudia Zuniga";
            model.SubmitterEDIContact.CommunicationNumberQualifier1 = "TE";
            model.SubmitterEDIContact.CommunicationNumber1 = "4803551364";
            model.SubmitterEDIContact.CommunicationNumberQualifier2 = "EX";
            model.SubmitterEDIContact.CommunicationNumber2 = "1";
            model.SubmitterEDIContact.CommunicationNumberQualifier3 = "EM";
            model.SubmitterEDIContact.CommunicationNumber3 = "claudiaz@zrpath.com";

            model.ReceiverName.EntityIdentifierCodeEnum = "40";
            model.ReceiverName.EntityTypeQualifier = "2";
            model.ReceiverName.NameLastOrOrganizationName = "SPSI";
            model.ReceiverName.IdCodeQualifier = "46";
            model.ReceiverName.IdCodeQualifierEnum = "752524263";

            #region Subscriber

            #region Claim

            #region Service Line

            ServiceLine serviceLineObj = new ServiceLine();
            serviceLineObj.AssignedNumber = "1";

            serviceLineObj.CompositeMedicalProcedureIdentifier = "HC:S5150";
            serviceLineObj.MonetaryAmount = "217.21";
            serviceLineObj.UnitOrBasisForMeasurementCode = "UN";
            serviceLineObj.Quantity = "29";
            serviceLineObj.CompositeDiagnosisCodePointer = "1";

            serviceLineObj.DateTimeQualifier = "472";
            serviceLineObj.DateTimePeriodFormatQualifier = "RD8";
            serviceLineObj.DateTimePeriod = "20160708-20160708";

            serviceLineObj.ReferenceIdentificationQualifier = "6R";
            serviceLineObj.ReferenceIdentification = "00000022200001";

            #endregion Service Line

            Claim claimObj = new Claim();
            claimObj.PatientControlNumber = "177842E222ZRP";
            claimObj.TotalClaimChargeAmount = "217.21";
            claimObj.FacilityCodeValue = "99";
            claimObj.FacilityCodeQualifier = "B";
            claimObj.ClaimFrequencyTypeCode = "1";
            claimObj.ProviderOrSupplierSignatureIndicator = "Y";
            claimObj.ProviderAcceptAssignmentCode = "A";
            claimObj.BenefitsAssignmentCerficationIndicator = "Y";
            claimObj.ReleaseOfInformationCode = "Y";
            claimObj.PatientSignatureSourceCode = "P";

            claimObj.ReferenceIdentificationQualifier = "EA";
            claimObj.ReferenceIdentification = "JA07140810";

            claimObj.HealthCareCodeInformation01 = "ABK:F909";

            #region Provider

            claimObj.RenderingProviderEntityIdentifierCode = "82";
            claimObj.RenderingProviderEntityTypeQualifier = "2";
            claimObj.RenderingProviderNameLastOrOrganizationName = "Gable House";
            claimObj.RenderingProviderIdCodeQualifier = "XX";
            claimObj.RenderingProviderIdCodeQualifierEnum = "1689969834";

            claimObj.ServiceFacilityLocationEntityIdentifierCode = "77";
            claimObj.ServiceFacilityLocationEntityTypeQualifier = "2";
            claimObj.ServiceFacilityLocationNameLastOrOrganizationName = "Gable House";
            claimObj.ServiceFacilityLocationIdCodeQualifier = "XX";
            claimObj.ServiceFacilityLocationIdCodeQualifierEnum = "1689969834";

            claimObj.ServiceFacilityLocationAddressInformation = "2216 E. Gable Ave.";

            claimObj.ServiceFacilityLocationCityName = "Mesa";
            claimObj.ServiceFacilityLocationStateOrProvinceCode = "AZ";
            claimObj.ServiceFacilityLocationPostalCode = "852156173";

            #endregion Provider

            claimObj.ServiceLines.Add(serviceLineObj);

            #endregion Claim

            Subscriber subscriberObj = new Subscriber();
            subscriberObj.HeirarchicalLevelCode = "22";

            subscriberObj.PayerResponsibilitySequenceNumber = "P";
            subscriberObj.IndividualRelationshipCode = "18";
            subscriberObj.ClaimFilingIndicatorCode = "MB";

            subscriberObj.SubmitterEntityIdentifierCode = "IL";
            subscriberObj.SubmitterEntityTypeQualifier = "1";
            subscriberObj.SubmitterNameLastOrOrganizationName = "ANDERSON";
            subscriberObj.SubmitterNameFirst = "JERROD";
            subscriberObj.SubmitterIdCodeQualifier = "MI";
            subscriberObj.SubmitterIdCodeQualifierEnum = "1100770159";

            subscriberObj.SubmitterAddressInformation = "3463 E ELGIN";
                       
            subscriberObj.SubmitterCityName = "Gilbert";
            subscriberObj.SubmitterStateOrProvinceCode = "AZ";
            subscriberObj.SubmitterPostalCode = "852950000";

            subscriberObj.SubmitterDateTimePeriodFormatQualifier = "D8";
            subscriberObj.SubmitterDateTimePeriod = "20080714";
            subscriberObj.SubmitterGenderCode = "M";

            subscriberObj.PayerEntityIdentifierCode = "PR";
            subscriberObj.PayerEntityTypeQualifier = "2";
            subscriberObj.PayerNameLastOrOrganizationName = "Mercy Maricopa Integrated Care";
            subscriberObj.PayerIdCodeQualifier = "PI";
            subscriberObj.PayerIdCodeQualifierEnum = "33628";

            subscriberObj.PayerAddressInformation = "PO BOX 64835";

            subscriberObj.PayerCityName = "Phoenix";
            subscriberObj.PayerStateOrProvinceCode = "AZ";
            subscriberObj.PayerPostalCode = "850824835";

            subscriberObj.Claims.Add(claimObj);

            #endregion Subscriber

            #region Billing Provider

            BillingProvider billingProviderObj = new BillingProvider();
            billingProviderObj.HeirarchicalLevelCode = "20";

            billingProviderObj.EntityIdentifierCode = "85";
            billingProviderObj.EntityTypeQualifier = "2";
            billingProviderObj.NameLastOrOrganizationName = "Zarephath";
            billingProviderObj.IdCodeQualifier = "XX";
            billingProviderObj.IdCodeQualifierEnum = "1730439167";

            billingProviderObj.AddressInformation = "4856 E BASELINE RD STE 103";

            billingProviderObj.CityName = "MESA";
            billingProviderObj.StateOrProvinceCode = "AZ";
            billingProviderObj.PostalCode = "852064635";

            billingProviderObj.ReferenceIdentificationQualifier = "EI";
            billingProviderObj.ReferenceIdentification = "412111326";

            billingProviderObj.Subscribers.Add(subscriberObj);

            #endregion Billing Provider

            model.BillingProviders.Add(billingProviderObj);

            #endregion Create Model

            #region Generate 837 File

            string strInterchangeDate = model.InterchangeControlHeader.InterchangeDate;
            DateTime date = new DateTime(Convert.ToInt16("20" + strInterchangeDate.Substring(0,2)), Convert.ToInt32(strInterchangeDate.Substring(2, 2)), Convert.ToInt32(strInterchangeDate.Substring(2, 2)));

            var message = new Interchange(date, Convert.ToInt32(model.InterchangeControlHeader.InterchangeControlNumber), model.InterchangeControlHeader.UsageIndicator == "P")
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

            var group = message.AddFunctionGroup(model.FunctionalGroupHeader.FunctionalIdentifierCode, date,
                controlNumber, model.FunctionalGroupHeader.VersionOrReleaseOrNo);
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

            var perSegment = submitterLoop.AddSegment(new TypedSegmentPER());
            perSegment.PER01_ContactFunctionCode = model.SubmitterEDIContact.ContactFunctionCode; //information contact function code
            perSegment.PER02_Name = model.SubmitterEDIContact.Name;
            perSegment.PER03_CommunicationNumberQualifier = GetCommunicationNumberQualifer(model.SubmitterEDIContact.CommunicationNumberQualifier1);
            perSegment.PER04_CommunicationNumber = model.SubmitterEDIContact.CommunicationNumber1;
            perSegment.PER05_CommunicationNumberQualifier = GetCommunicationNumberQualifer(model.SubmitterEDIContact.CommunicationNumberQualifier2);
            perSegment.PER06_CommunicationNumber = model.SubmitterEDIContact.CommunicationNumber2;
            perSegment.PER07_CommunicationNumberQualifier = GetCommunicationNumberQualifer(model.SubmitterEDIContact.CommunicationNumberQualifier3);
            perSegment.PER08_CommunicationNumber = model.SubmitterEDIContact.CommunicationNumber3;

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
                                    claim2300Loop.CLM01_PatientControlNumber = claim.PatientControlNumber;
                                    claim2300Loop.CLM02_TotalClaimChargeAmount = Convert.ToDecimal(claim.TotalClaimChargeAmount);
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

                                    #endregion Claim Information > Reference > REF

                                    #region Claim Information > HI

                                    var hiSegment = claim2300Loop.AddSegment(new TypedSegmentHI());
                                    hiSegment.HI01_HealthCareCodeInformation = claim.HealthCareCodeInformation01;

                                    #endregion Claim Information > HI

                                    #region Claim Information > Provider

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

                                    #endregion Claim Information > Provider

                                    #region Claim Information > Service Line Number

                                    if (claim.ServiceLines.Count > 0)
                                    {
                                        foreach (var serviceLine in claim.ServiceLines)
                                        {
                                            #region Claim Information > Service Line Number > LX

                                            var lxLoop = claim2300Loop.AddLoop(new TypedLoopLX(Constants.ServiceLineNode));
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

                                            var lxRefSegment = lxLoop.AddSegment(new TypedSegmentREF());
                                            lxRefSegment.REF01_ReferenceIdQualifier = serviceLine.ReferenceIdentificationQualifier;
                                            lxRefSegment.REF02_ReferenceId = serviceLine.ReferenceIdentification;

                                            #endregion Claim Information > Service Line Number > Reference > REF
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

            var x12 = message.SerializeToX12(true);
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files\\837\\");
            string nameOfTheFile = DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileName = string.Format("{0}{1}.txt", basePath, nameOfTheFile);

            //var sr = new StreamWriter(fileName, true);
            //sr.Write(x12);

            System.IO.File.WriteAllText(fileName, x12);

            #endregion Generate 837 File
        }

        #region Validate 837 File

        public static void Validate837File()
        {
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files\\837\\");
            string nameOfTheFile = "20160813130015";
            string fileName = string.Format("{0}{1}.txt", basePath, nameOfTheFile);

            var service = new ProfessionalClaimAcknowledgmentService();
            var responses = service.AcknowledgeTransactions(TestStream(fileName));
        }

        private static Stream TestStream(string fileName)
        {
            Stream fs = File.OpenRead(fileName);
            return fs;
        }

        #endregion Validate 837 File

        public static CommunicationNumberQualifer GetCommunicationNumberQualifer(string value)
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

        public static EntityTypeQualifier GetEntityTypeQualifier(string value)
        {
            return value == "1" ? EntityTypeQualifier.Person : EntityTypeQualifier.NonPersonEntity;
        }

        public static Gender GetGender(string value)
        {
            switch (value)
            {
                case "F": return Gender.Female;
                case "M": return Gender.Male;
                case "U": return Gender.Unknown;
                default: return Gender.Undefined;
            }
        }

        public static bool GetBoolean(string value)
        {
            return value == "Y";
        }

        public static DTPQualifier GetDtpQualifier(string value)
        {
            switch (value)
            {
                case "472": return DTPQualifier.Service;
                default: return DTPQualifier.Service;
            }
        }

        public static DTPFormatQualifier GetDtpFormatQualifier(string value)
        {
            switch (value)
            {
                case "RD8": return DTPFormatQualifier.CCYYMMDD_CCYYMMDD;
                default: return DTPFormatQualifier.CCYYMMDD_CCYYMMDD;
            }
        }
    }


    public class Test
    {
        public string Filename { get; set; }
        public string CheckSequence { get; set; }
        public string PayerName { get; set; }
        public string PayeeName { get; set; }
        public string PayeeID { get; set; }
        public string CheckDate { get; set; }
    }
}
