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
    public class Edi835
    {
        #region CORE METHODS


        public Edi835ResponseModel GenerateEdi835Model(string absolutePathForEdi835File, string absolutePathForGenerateReadableFile, string relativePathForGenerateReadableFile)
        {
            Edi835ResponseModel edi835ResponseModel = new Edi835ResponseModel()
                {
                    GeneratedFileAbsolutePath = absolutePathForGenerateReadableFile,
                    GeneratedFileRelativePath = relativePathForGenerateReadableFile
                };
            #region Parse 835 File

            Stream transformStream = null;
            Stream inputStream = new FileStream(absolutePathForEdi835File, FileMode.Open, FileAccess.Read);
            
            //string csvFileName = "Sample-835-Output.csv";
            //FileInfo outputFileInfo = new FileInfo(csvFileName);
            //string[] names = Assembly.GetManifestResourceNames();D:\projects\ZarePhath\trunk\Zarephath\06. Source Code\Project-Web\02. Development\Zarephath\EDI_837_835_HCCP\EDI_837_835_HCCP\Transformations\X12-835-XML-to-CSV.xslt
            transformStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EDI_837_835_HCCP.Transformations.X12-835-XML-to-CSV.xslt");


            X12Parser parser = new X12Parser();
            Interchange interchange = parser.Parse(inputStream);
            interchange.SerializeToX12(true);
            string xml = interchange.Serialize();

            var transform = new XslCompiledTransform();
            transform.Load(XmlReader.Create(transformStream));
            XsltArgumentList arguments = new XsltArgumentList();
            arguments.AddParam("filename", "", new FileInfo(absolutePathForEdi835File).Name);

            MemoryStream mstream = new MemoryStream();
            transform.Transform(XmlReader.Create(new StringReader(xml)), arguments, mstream);
            mstream.Flush();
            mstream.Position = 0;
            string content = new StreamReader(mstream).ReadToEnd();
            {
                FileInfo outputFileInfo = new FileInfo(absolutePathForGenerateReadableFile);
                if (!Directory.Exists(outputFileInfo.DirectoryName))
                    Directory.CreateDirectory(outputFileInfo.DirectoryName);



                string filename =  String.Format("{0}\\{1}{2}", outputFileInfo.DirectoryName, outputFileInfo.Name.Replace(outputFileInfo.Extension, ""), outputFileInfo.Extension);
                using (Stream outputStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter writer = new StreamWriter(outputStream))
                    {
                        writer.Write(content);
                        writer.Close();
                    }
                    outputStream.Close();
                }


                using (CsvReader reader = new CsvReader(filename))
                {
                    int index = 0;
                    foreach (string[] vals in reader.RowEnumerator)
                    {
                        //Skip first 1 row
                        if (index >= 1)
                        {
                            var i = 0;
                            Edi835Model model = new Edi835Model();

                            model.FileName = vals[i++];
                            model.CheckSequence = vals[i++];

                            model.N102_PayorName = vals[i++];
                            model.PER02_PayorBusinessContactName = vals[i++];
                            model.PER04_PayorBusinessContact = vals[i++];
                            model.PER02_PayorTechnicalContactName = vals[i++];
                            model.PER04_PayorTechnicalContact = vals[i++];
                            model.PER06_PayorTechnicalEmail = vals[i++];
                            model.PER04_PayorTechnicalContactUrl = vals[i++];


                            model.N102_PayeeName = vals[i++];
                            model.N103_PayeeIdentificationQualifier = vals[i++];
                            model.N104_PayeeIdentification = vals[i++];


                            model.LX01_ClaimSequenceNumber = vals[i++];
                            model.CLP01_ClaimSubmitterIdentifier = vals[i++];
                            model.CLP02_ClaimStatusCode  = vals[i++];
                            model.CLP03_TotalClaimChargeAmount = vals[i++];
                            model.CLP04_TotalClaimPaymentAmount = vals[i++];
                            model.CLP05_PatientResponsibilityAmount = vals[i++];
                            model.CLP07_PayerClaimControlNumber = vals[i++];
                            model.CLP08_PlaceOfService = vals[i++];


                            model.NM103_PatientLastName = vals[i++];
                            model.NM104_PatientFirstName = vals[i++];
                            model.NM109_PatientIdentifier = vals[i++];

                            model.NM103_ServiceProviderName = vals[i++];
                            model.NM109_ServiceProviderNpi = vals[i++];

                            model.SVC01_01_ServiceCodeQualifier = vals[i++];
                            model.SVC01_02_ServiceCode = vals[i++];
                            
                            model.SVC01_02_ServiceCode_Mod_01 = vals[i++];
                            model.SVC01_02_ServiceCode_Mod_02 = vals[i++];
                            model.SVC01_02_ServiceCode_Mod_03 = vals[i++];
                            model.SVC01_02_ServiceCode_Mod_04 = vals[i++];

                            model.SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount = vals[i++];
                            model.SVC03_LineItemProviderPaymentAmoun_PaidAmount = vals[i++];
                            model.SVC05_ServiceCodeUnit = vals[i++];
                            model.DTM02_ServiceStartDate = vals[i++];
                            model.DTM02_ServiceEndDate = vals[i++];
                            model.CAS01_ClaimAdjustmentGroupCode = vals[i++];
                            model.CAS02_ClaimAdjustmentReasonCode = vals[i++];
                            model.CAS03_ClaimAdjustmentAmount = vals[i++];


                            model.REF02_LineItem_ReferenceIdentification=  vals[i++];
                            model.AMT01_ServiceLineAllowedAmount_AllowedAmount = vals[i++];

                            model.CheckDate = vals[i++];
                            model.CheckAmount = vals[i++];
                            model.CheckNumber = vals[i++];
                            model.PolicyNumber = vals[i++];
                            model.AccountNumber = vals[i++];
                            model.ICN = vals[i++];
                            //model.BilledAmount = vals[i++];
                            //model.AllowedAmount = vals[i++];
                            model.Deductible = vals[i++];
                            model.Coins = vals[i++];
                            model.ProcessedDate = vals[i++];
                            //model.PaidAmount = vals[i++];

                            edi835ResponseModel.Edi835ModelList.Add(model);
                        }
                        index++;
                    }
                }
            }


            #endregion Parse 835 File

            return edi835ResponseModel;
        }

        #endregion


    }
}
