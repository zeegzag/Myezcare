using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
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
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace EDI_837_835_HCCP
{
    public class Edi277
    {
        #region CORE METHODS


        public Edi277ResponseModel GenerateEdi277Model(string absolutePathForEdi277File, Edi277ResponseModel edi277ResponseModel)
        {

            #region Parse 277 File

            Stream transformStream = null;
            Stream inputStream = new FileStream(absolutePathForEdi277File, FileMode.Open, FileAccess.Read);
            //transformStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EDI_837_835_HCCP.Transformations.X12-277-XML-to-CSV.xslt");

            X12Parser parser = new X12Parser();
            Interchange interchange = parser.Parse(inputStream);
            interchange.SerializeToX12(true);
            string xml = interchange.Serialize();

            edi277ResponseModel.Response = ConvertToRowForDB(xml, edi277ResponseModel);



            #endregion Parse 277 File

            return edi277ResponseModel;
        }




        #endregion



        protected string ConvertToCSV(string xmlInput, ref string absolutePathForGenerateReadableFile)
        {
            List<Ack277Model> modeList = new List<Ack277Model>();

            string csvOut = string.Empty;
            XDocument doc = XDocument.Parse(xmlInput);
            StringBuilder master = new StringBuilder();
            master.AppendFormat("Source,Trace Number,Receipt Date,Process Date,Receiver,Total Accepted Claims,Total Accepted Amount,Total Rejected Claims,Total Rejected Amount,Provider,Last Name,First Name,AHCCCS #, Status, CSCC|CSC|EIC, Action, Amount, Message, Batch #, Note #, Claim #, Payor Claim#, Service Date");
            master.Append(Environment.NewLine);

            foreach (XElement node in doc.Descendants("Transaction"))
            {
                Ack277Model model = new Ack277Model();
                StringBuilder submasterBuilder = new StringBuilder();


                var source = node.Descendants("Loop").First(c => (string)c.Attribute("LoopId") == "2100A");
                var sourceName = source.Descendants("NM1").Elements("NM103").FirstOrDefault() != null ? source.Descendants("NM1").Elements("NM103").First().Value : "";
                var sourceId = source.Descendants("NM1").Elements("NM109").FirstOrDefault() != null ? source.Descendants("NM1").Elements("NM109").First().Value : "";
                submasterBuilder.AppendFormat("{0}-{1},", sourceName, sourceId);

                var sourceTracing = node.Descendants("Loop").First(c => (string)c.Attribute("LoopId") == "2200A");
                var trace = sourceTracing.Descendants("TRN").Elements("TRN02").FirstOrDefault() != null ? sourceTracing.Descendants("TRN").Elements("TRN02").First().Value : "";

                var receiptdate = ""; var processdate = "";
                var dateCollection = sourceTracing.Descendants("DTP");
                foreach (var item in dateCollection)
                {
                    var date = item.Elements("DTP01").FirstOrDefault();
                    var dateValue = item.Elements("DTP03").FirstOrDefault();
                    if (date != null && dateValue != null)
                    {
                        if (date.Value == "050")
                            receiptdate = DateTime.ParseExact(dateValue.Value, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                        if (date.Value == "009")
                            processdate = DateTime.ParseExact(dateValue.Value, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    }

                }


                submasterBuilder.AppendFormat("{0},", trace);
                submasterBuilder.AppendFormat("{0},", receiptdate);
                submasterBuilder.AppendFormat("{0},", processdate);


                //submasterBuilder.AppendFormat("{0},", sourceTracing.Descendants("DTP").Where(c => (string)c.Elements("DTP01").First().Value == "050").Elements("DTP03").First().Value);
                //submasterBuilder.AppendFormat("{0},", sourceTracing.Descendants("DTP").Where(c => (string)c.Elements("DTP01").First().Value == "009").Elements("DTP03").First().Value);


                var receiver = node.Descendants("Loop").FirstOrDefault(c => (string)c.Attribute("LoopId") == "2100B");
                if (receiver == null)
                    submasterBuilder.AppendFormat("{0},", "");
                else
                    submasterBuilder.AppendFormat("{0},", receiver.Descendants("NM1").Elements("NM103").First().Value + " - " + receiver.Descendants("NM1").Elements("NM109").First().Value);


                var receiverTracing = node.Descendants("Loop").FirstOrDefault(c => (string)c.Attribute("LoopId") == "2200B");
                if (receiverTracing == null)
                    submasterBuilder.AppendFormat(",,,,,");
                else
                {
                    var qtyAccepted = ""; var qtyRejected = "";
                    var amountAccepted = ""; var amountRejected = "";

                    var qtyCollection = receiverTracing.Descendants("QTY");
                    foreach (var item in qtyCollection)
                    {
                        var qtyStatus = item.Elements("QTY01").FirstOrDefault();
                        var qtyValue = item.Elements("QTY02").FirstOrDefault();
                        if (qtyStatus != null && qtyValue != null)
                        {
                            if (qtyStatus.Value == "90")
                                qtyAccepted = qtyValue.Value;

                            if (qtyStatus.Value == "AA")
                                qtyRejected = qtyValue.Value;
                        }

                    }

                    var amountCollection = receiverTracing.Descendants("AMT");
                    foreach (var item in amountCollection)
                    {
                        var amtStatus = item.Elements("AMT01").FirstOrDefault();
                        var amtValue = item.Elements("AMT02").FirstOrDefault();
                        if (amtStatus != null && amtValue != null)
                        {
                            if (amtStatus.Value == "YU")
                                amountAccepted = amtValue.Value;

                            if (amtStatus.Value == "YY")
                                amountRejected = amtValue.Value;
                        }

                    }

                    submasterBuilder.AppendFormat("{0},", qtyAccepted);
                    submasterBuilder.AppendFormat("{0},", amountAccepted);
                    submasterBuilder.AppendFormat("{0},", qtyRejected);
                    submasterBuilder.AppendFormat("{0},", amountRejected);


                    //submasterBuilder.AppendFormat("{0},", receiverTracing.Descendants("QTY").Where(c => (string)c.Elements("QTY01").First().Value == "90").Elements("QTY02").First().Value);
                    //submasterBuilder.AppendFormat("{0},", receiverTracing.Descendants("AMT").Where(c => (string)c.Elements("AMT01").First().Value == "YU").Elements("AMT02").First().Value);
                    //submasterBuilder.AppendFormat("{0},", receiverTracing.Descendants("QTY").Where(c => (string)c.Elements("QTY01").First().Value == "AA").Elements("QTY02").First().Value);
                    //submasterBuilder.AppendFormat("{0},", receiverTracing.Descendants("AMT").Where(c => (string)c.Elements("AMT01").First().Value == "YY").Elements("AMT02").First().Value);
                }

                var provider = node.Descendants("Loop").FirstOrDefault(c => (string)c.Attribute("LoopId") == "2100C");

                if (provider == null)
                    submasterBuilder.AppendFormat("{0},", "");
                else
                    submasterBuilder.AppendFormat("{0},", provider.Descendants("NM1").Elements("NM103").First().Value + " - " + provider.Descendants("NM1").Elements("NM109").First().Value);


                var patienList = node.Descendants("HierarchicalLoop").Where(c => (string)c.Attribute("LoopId") == "2000D").ToList();  //"[@LoopId='2000D']").Count();
                foreach (XElement innerNode in patienList)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(submasterBuilder);

                    var patientname = innerNode.Descendants("Loop").First(c => (string)c.Attribute("LoopId") == "2100D");
                    sb.AppendFormat("{0},", patientname.Descendants("NM1").Elements("NM103").First().Value);
                    sb.AppendFormat("{0},", patientname.Descendants("NM1").Elements("NM104").First().Value);
                    sb.AppendFormat("{0},", patientname.Descendants("NM1").Elements("NM109").First().Value);


                    // Status, CSCC|CSC|EIC, Action, Amount, Message, Batch #, Note #, Claim #, Payor Claim#, Service Date

                    var claimStatusTracking = innerNode.Descendants("Loop").Where(c => (string)c.Attribute("LoopId") == "2200D").ToList();
                    foreach (XElement subitemClaimStatusTracking in claimStatusTracking)
                    {
                        StringBuilder builder = new StringBuilder(sb.ToString());
                        var stc = subitemClaimStatusTracking.Descendants("STC").First();
                        var stc01 = stc.Descendants("STC01").First();
                        var stc0101 = stc01.Elements("STC0101").FirstOrDefault() != null ? stc01.Elements("STC0101").First().Value : "-";
                        var stc0102 = stc01.Elements("STC0102").FirstOrDefault() != null ? stc01.Elements("STC0102").First().Value : "-";
                        var stc0103 = stc01.Elements("STC0103").FirstOrDefault() != null ? stc01.Elements("STC0103").First().Value : "-";
                        var stc03 = stc.Elements("STC03").FirstOrDefault() != null ? stc.Elements("STC03").First().Value : "0.00";
                        var stc04 = stc.Elements("STC04").FirstOrDefault() != null ? stc.Elements("STC04").First().Value : "0.00";
                        var stc12 = stc.Elements("STC12").FirstOrDefault() != null ? stc.Elements("STC12").First().Value : "";
                        var trn02 = subitemClaimStatusTracking.Descendants("TRN").Elements("TRN02").FirstOrDefault() != null ? subitemClaimStatusTracking.Descendants("TRN").Elements("TRN02").First().Value : "";

                        builder.AppendFormat("{0},", GetClaimStatusCategoryCodes(stc0101));
                        builder.AppendFormat("{0},", string.Format("{0}|{1}|{2}", stc0101, stc0102, stc0103));
                        builder.AppendFormat("{0},", GetClaimStatus(stc03));
                        builder.AppendFormat("{0},", stc04);
                        builder.AppendFormat("{0},", stc12);
                        builder.AppendFormat("{0},", GetClaimDetails(trn02, "Batch"));
                        builder.AppendFormat("{0},", GetClaimDetails(trn02, "Note"));
                        builder.AppendFormat("{0},", trn02);


                        var ref03 = subitemClaimStatusTracking.Descendants("REF").Elements("REF02").FirstOrDefault() != null ? subitemClaimStatusTracking.Descendants("REF").Elements("REF02").First().Value : "";
                        builder.AppendFormat("{0},", ref03);


                        var dtp03 = subitemClaimStatusTracking.Descendants("DTP").Elements("DTP03").FirstOrDefault() != null ? subitemClaimStatusTracking.Descendants("DTP").Elements("DTP03").First().Value : "";
                        builder.AppendFormat("{0},", dtp03);


                        builder.Append(Environment.NewLine);
                        master.Append(builder);
                    }
                }

                if (patienList.Count == 0)
                {
                    master.Append(submasterBuilder);
                }
            }
            master.Remove(master.Length - 1, 1);
            master.AppendLine();
            csvOut = master.ToString();




            FileInfo outputFileInfo = new FileInfo(absolutePathForGenerateReadableFile);
            if (!Directory.Exists(outputFileInfo.DirectoryName))
                Directory.CreateDirectory(outputFileInfo.DirectoryName);



            string filename = String.Format("{0}\\{1}{2}", outputFileInfo.DirectoryName, outputFileInfo.Name.Replace(outputFileInfo.Extension, ""), outputFileInfo.Extension);
            using (Stream outputStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(outputStream))
                {
                    writer.Write(csvOut);
                    writer.Close();
                }
                outputStream.Close();
            }







            return csvOut;
        }




        protected string ConvertToRowForDB(string xmlInput, Edi277ResponseModel edi277ResponseModel)
        {




            string csvOut = string.Empty;

            string cellSeprator = edi277ResponseModel.FieldSeparator;// ",";
            string rowSeprator = edi277ResponseModel.RecordSeparator; // Environment.NewLine;


            XDocument doc = XDocument.Parse(xmlInput);
            StringBuilder master = new StringBuilder();
            //master.AppendFormat("Source,Trace Number,Receipt Date,Process Date,Receiver,Total Accepted Claims,Total Accepted Amount,Total Rejected Claims,Total Rejected Amount,Provider,Last Name,First Name,AHCCCS #, Status, CSCC|CSC|EIC, Action, Amount, Message, Batch #, Note #, Claim #, Payor Claim#, Service Date");
            //master.Append(rowSeprator);

            foreach (XElement node in doc.Descendants("Transaction"))
            {
                Edi277Model parentModel = new Edi277Model();
                StringBuilder submasterBuilder = new StringBuilder();
                var source = node.Descendants("Loop").First(c => (string)c.Attribute("LoopId") == "2100A");
                var sourceName = source.Descendants("NM1").Elements("NM103").FirstOrDefault() != null ? source.Descendants("NM1").Elements("NM103").First().Value : "";
                var sourceId = source.Descendants("NM1").Elements("NM109").FirstOrDefault() != null ? source.Descendants("NM1").Elements("NM109").First().Value : "";

                submasterBuilder.AppendFormat("{0}-{1}{2}", sourceName, sourceId, cellSeprator);
                parentModel.Source = string.Format("{0}-{1}", sourceName, sourceId);

                var sourceTracing = node.Descendants("Loop").First(c => (string)c.Attribute("LoopId") == "2200A");
                var trace = sourceTracing.Descendants("TRN").Elements("TRN02").FirstOrDefault() != null ? sourceTracing.Descendants("TRN").Elements("TRN02").First().Value : "";

                var receiptdate = ""; var processdate = "";
                var dateCollection = sourceTracing.Descendants("DTP");
                foreach (var item in dateCollection)
                {
                    var date = item.Elements("DTP01").FirstOrDefault();
                    var dateValue = item.Elements("DTP03").FirstOrDefault();
                    if (date != null && dateValue != null)
                    {
                        if (date.Value == "050")
                            receiptdate = DateTime.ParseExact(dateValue.Value, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                        if (date.Value == "009")
                            processdate = DateTime.ParseExact(dateValue.Value, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    }

                }


                submasterBuilder.AppendFormat("{0}{1}", trace, cellSeprator);
                submasterBuilder.AppendFormat("{0}{1}", receiptdate, cellSeprator);
                submasterBuilder.AppendFormat("{0}{1}", processdate, cellSeprator);

                parentModel.TraceNumber = string.Format("{0}", trace);
                parentModel.ReceiptDate = string.Format("{0}", receiptdate);
                parentModel.ProcessDate = string.Format("{0}", processdate);

                //submasterBuilder.AppendFormat("{0},", sourceTracing.Descendants("DTP").Where(c => (string)c.Elements("DTP01").First().Value == "050").Elements("DTP03").First().Value);
                //submasterBuilder.AppendFormat("{0},", sourceTracing.Descendants("DTP").Where(c => (string)c.Elements("DTP01").First().Value == "009").Elements("DTP03").First().Value);


                var receiver = node.Descendants("Loop").FirstOrDefault(c => (string)c.Attribute("LoopId") == "2100B");
                if (receiver == null)
                {
                    submasterBuilder.AppendFormat("{0}{1}", "", cellSeprator);
                }
                else
                {
                    submasterBuilder.AppendFormat("{0}{1}",
                        receiver.Descendants("NM1").Elements("NM103").First().Value + " - " +
                        receiver.Descendants("NM1").Elements("NM109").First().Value, cellSeprator);
                    parentModel.Receiver = string.Format("{0}",
                        receiver.Descendants("NM1").Elements("NM103").First().Value + " - " +
                        receiver.Descendants("NM1").Elements("NM109").First().Value);
                }


                var receiverTracing = node.Descendants("Loop").FirstOrDefault(c => (string)c.Attribute("LoopId") == "2200B");
                if (receiverTracing == null)
                    submasterBuilder.AppendFormat("{0}{0}{0}{0}{0}", cellSeprator);
                else
                {
                    var qtyAccepted = ""; var qtyRejected = "";
                    var amountAccepted = ""; var amountRejected = "";

                    var qtyCollection = receiverTracing.Descendants("QTY");
                    foreach (var item in qtyCollection)
                    {
                        var qtyStatus = item.Elements("QTY01").FirstOrDefault();
                        var qtyValue = item.Elements("QTY02").FirstOrDefault();
                        if (qtyStatus != null && qtyValue != null)
                        {
                            if (qtyStatus.Value == "90")
                                qtyAccepted = qtyValue.Value;

                            if (qtyStatus.Value == "AA")
                                qtyRejected = qtyValue.Value;
                        }

                    }

                    var amountCollection = receiverTracing.Descendants("AMT");
                    foreach (var item in amountCollection)
                    {
                        var amtStatus = item.Elements("AMT01").FirstOrDefault();
                        var amtValue = item.Elements("AMT02").FirstOrDefault();
                        if (amtStatus != null && amtValue != null)
                        {
                            if (amtStatus.Value == "YU")
                                amountAccepted = amtValue.Value;

                            if (amtStatus.Value == "YY")
                                amountRejected = amtValue.Value;
                        }

                    }

                    submasterBuilder.AppendFormat("{0}{1}", qtyAccepted, cellSeprator);
                    submasterBuilder.AppendFormat("{0}{1}", amountAccepted, cellSeprator);
                    submasterBuilder.AppendFormat("{0}{1}", qtyRejected, cellSeprator);
                    submasterBuilder.AppendFormat("{0}{1}", amountRejected, cellSeprator);

                    parentModel.TotalAcceptedClaims = string.Format("{0}", qtyAccepted);
                    parentModel.TotalAcceptedAmount = string.Format("{0}", amountAccepted);
                    parentModel.TotalRejectedClaims = string.Format("{0}", qtyRejected);
                    parentModel.TotalRejectedAmount = string.Format("{0}", amountRejected);

                    //submasterBuilder.AppendFormat("{0},", receiverTracing.Descendants("QTY").Where(c => (string)c.Elements("QTY01").First().Value == "90").Elements("QTY02").First().Value);
                    //submasterBuilder.AppendFormat("{0},", receiverTracing.Descendants("AMT").Where(c => (string)c.Elements("AMT01").First().Value == "YU").Elements("AMT02").First().Value);
                    //submasterBuilder.AppendFormat("{0},", receiverTracing.Descendants("QTY").Where(c => (string)c.Elements("QTY01").First().Value == "AA").Elements("QTY02").First().Value);
                    //submasterBuilder.AppendFormat("{0},", receiverTracing.Descendants("AMT").Where(c => (string)c.Elements("AMT01").First().Value == "YY").Elements("AMT02").First().Value);
                }

                var provider = node.Descendants("Loop").FirstOrDefault(c => (string)c.Attribute("LoopId") == "2100C");

                if (provider == null)
                    submasterBuilder.AppendFormat("{0}{1}", "", cellSeprator);
                else
                {
                    submasterBuilder.AppendFormat("{0}{1}",
                        provider.Descendants("NM1").Elements("NM103").First().Value + " - " +
                        provider.Descendants("NM1").Elements("NM109").First().Value, cellSeprator);

                    parentModel.Provider = string.Format("{0}",
                        provider.Descendants("NM1").Elements("NM103").First().Value + " - " +
                        provider.Descendants("NM1").Elements("NM109").First().Value);
                }

                var patienList = node.Descendants("HierarchicalLoop").Where(c => (string)c.Attribute("LoopId") == "2000D").ToList();  //"[@LoopId='2000D']").Count();
                foreach (XElement innerNode in patienList)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(submasterBuilder);


                    Edi277Model childModel = new Edi277Model();

                    var patientname = innerNode.Descendants("Loop").First(c => (string)c.Attribute("LoopId") == "2100D");
                    sb.AppendFormat("{0}{1}", patientname.Descendants("NM1").Elements("NM103").First().Value, cellSeprator);
                    sb.AppendFormat("{0}{1}", patientname.Descendants("NM1").Elements("NM104").First().Value, cellSeprator);
                    sb.AppendFormat("{0}{1}", patientname.Descendants("NM1").Elements("NM109").First().Value, cellSeprator);

                    childModel.LastName = string.Format("{0}", patientname.Descendants("NM1").Elements("NM103").First().Value);
                    childModel.FirstName = string.Format("{0}", patientname.Descendants("NM1").Elements("NM104").First().Value);
                    childModel.AHCCCSID = string.Format("{0}", patientname.Descendants("NM1").Elements("NM109").First().Value);

                    // Status, CSCC|CSC|EIC, Action, Amount, Message, Batch #, Note #, Claim #, Payor Claim#, Service Date

                    var claimStatusTracking = innerNode.Descendants("Loop").Where(c => (string)c.Attribute("LoopId") == "2200D").ToList();
                    foreach (XElement subitemClaimStatusTracking in claimStatusTracking)
                    {
                        Edi277Model model = new Edi277Model()
                        {
                            Source = parentModel.Source,
                            TraceNumber = parentModel.TraceNumber,
                            ReceiptDate = parentModel.ReceiptDate,
                            ProcessDate = parentModel.ProcessDate,
                            Receiver = parentModel.Receiver,
                            TotalAcceptedClaims = parentModel.TotalAcceptedClaims,
                            TotalAcceptedAmount = parentModel.TotalAcceptedAmount,
                            TotalRejectedClaims = parentModel.TotalRejectedClaims,
                            TotalRejectedAmount = parentModel.TotalRejectedAmount,
                            Provider = parentModel.Provider,
                            LastName = childModel.LastName,
                            FirstName = childModel.FirstName,
                            AHCCCSID = childModel.AHCCCSID
                        };

                        StringBuilder builder = new StringBuilder(sb.ToString());
                        var stc = subitemClaimStatusTracking.Descendants("STC").First();
                        var stc01 = stc.Descendants("STC01").First();
                        var stc0101 = stc01.Elements("STC0101").FirstOrDefault() != null ? stc01.Elements("STC0101").First().Value : "-";
                        var stc0102 = stc01.Elements("STC0102").FirstOrDefault() != null ? stc01.Elements("STC0102").First().Value : "-";
                        var stc0103 = stc01.Elements("STC0103").FirstOrDefault() != null ? stc01.Elements("STC0103").First().Value : "-";
                        var stc03 = stc.Elements("STC03").FirstOrDefault() != null ? stc.Elements("STC03").First().Value : "0.00";
                        var stc04 = stc.Elements("STC04").FirstOrDefault() != null ? stc.Elements("STC04").First().Value : "0.00";
                        var stc12 = stc.Elements("STC12").FirstOrDefault() != null ? stc.Elements("STC12").First().Value : "";
                        var trn02 = subitemClaimStatusTracking.Descendants("TRN").Elements("TRN02").FirstOrDefault() != null ? subitemClaimStatusTracking.Descendants("TRN").Elements("TRN02").First().Value : "";

                        builder.AppendFormat("{0}{1}", GetClaimStatusCategoryCodes(stc0101), cellSeprator);
                        //builder.AppendFormat("{0}{1}", string.Format("{0}|{1}|{2}", stc0101, stc0102, stc0103), cellSeprator);
                        builder.AppendFormat("{0}{1}", stc0101, cellSeprator);
                        builder.AppendFormat("{0}{1}", stc0102, cellSeprator);
                        builder.AppendFormat("{0}{1}", stc0103, cellSeprator);

                        model.Status = string.Format("{0}", GetClaimStatusCategoryCodes(stc0101));
                        model.CSCC = string.Format("{0}", stc0101);
                        model.CSC = string.Format("{0}", stc0102);
                        model.EIC = string.Format("{0}", stc0103);

                        builder.AppendFormat("{0}{1}", GetClaimStatus(stc03), cellSeprator);
                        builder.AppendFormat("{0}{1}", stc04, cellSeprator);
                        builder.AppendFormat("{0}{1}", stc12, cellSeprator);
                        builder.AppendFormat("{0}{1}", GetClaimDetails(trn02, "BatchNote"), cellSeprator);
                        builder.AppendFormat("{0}{1}", GetClaimDetails(trn02, "Batch"), cellSeprator);
                        builder.AppendFormat("{0}{1}", GetClaimDetails(trn02, "Note"), cellSeprator);
                        builder.AppendFormat("{0}{1}", trn02, cellSeprator);


                        model.Action = string.Format("{0}", GetClaimStatus(stc03));
                        model.Amount = string.Format("{0}", stc04);
                        model.Message = string.Format("{0}", stc12);
                        model.ClaimNumber = string.Format("{0}", trn02);



                        var ref03 = subitemClaimStatusTracking.Descendants("REF").Elements("REF02").FirstOrDefault() != null ? subitemClaimStatusTracking.Descendants("REF").Elements("REF02").First().Value : "";
                        builder.AppendFormat("{0}{1}", ref03, cellSeprator);
                        model.PayorClaimNumber = string.Format("{0}", ref03);

                        var dtp03 = subitemClaimStatusTracking.Descendants("DTP").Elements("DTP03").FirstOrDefault() != null ? subitemClaimStatusTracking.Descendants("DTP").Elements("DTP03").First().Value : "";
                        builder.AppendFormat("{0}{1}", dtp03, cellSeprator);
                        model.ServiceDate = string.Format("{0}", dtp03);

                        //builder.Append(Environment.NewLine);
                        builder.Append(rowSeprator);
                        master.Append(builder);






                        Edi277Model coreModel = new Edi277Model()
                        {
                            Source = model.Source,
                            TraceNumber = model.TraceNumber,
                            ReceiptDate = model.ReceiptDate,
                            ProcessDate = model.ProcessDate,
                            Receiver = model.Receiver,
                            TotalAcceptedClaims = model.TotalAcceptedClaims,
                            TotalAcceptedAmount = model.TotalAcceptedAmount,
                            TotalRejectedClaims = model.TotalRejectedClaims,
                            TotalRejectedAmount = model.TotalRejectedAmount,
                            Provider = model.Provider,
                            LastName = model.LastName,
                            FirstName = model.FirstName,
                            AHCCCSID = model.AHCCCSID,
                            Status = model.Status,
                            CSCC = model.CSCC,
                            CSC = model.CSC,
                            EIC = model.EIC,
                            Action = model.Action,
                            Amount = model.Amount,
                            Message = model.Message,
                            ClaimNumber = model.ClaimNumber,
                            PayorClaimNumber = model.PayorClaimNumber,
                            ServiceDate = model.ServiceDate
                        };




                        string value = trn02.Trim();

                        if (value.Contains("ZRPB") && value.Contains("BN"))
                        {
                            Edi277Model tempModel = JsonConvert.DeserializeObject<Edi277Model>(JsonConvert.SerializeObject(coreModel));
                            tempModel.BatchNoteID = string.Format("{0}", GetClaimDetails(value, "BatchNote"));

                            edi277ResponseModel.Edi277ModelList.Add(tempModel);
                        }
                        else if(value.Contains("N"))
                        {
                            int count = value.Trim('N').Split('N').Length;

                            Edi277Model tempModel = JsonConvert.DeserializeObject<Edi277Model>(JsonConvert.SerializeObject(coreModel));
                            tempModel.BatchNoteID = string.Format("{0}", GetClaimDetails(value, "BatchNote"));
                            edi277ResponseModel.Edi277ModelList.Add(tempModel);

                            if (count > 1)
                            {
                                Edi277Model tempModel01 = JsonConvert.DeserializeObject<Edi277Model>(JsonConvert.SerializeObject(coreModel));
                                tempModel01.BatchNoteID = string.Format("{0}", GetClaimDetails(value, "BatchNote",2));
                                edi277ResponseModel.Edi277ModelList.Add(tempModel01);
                            }
                        }

                        //model.BatchNoteID = string.Format("{0}", GetClaimDetails(trn02, "BatchNote"));
                        //model.BatchID = string.Format("{0}", GetClaimDetails(trn02, "Batch"));
                        //model.NoteID = string.Format("{0}", GetClaimDetails(trn02, "Note"));


                        




                        //edi277ResponseModel.Edi277ModelList.Add(model);
                    }
                }

                if (patienList.Count == 0)
                {
                    master.Append(submasterBuilder);
                    edi277ResponseModel.Edi277ModelList.Add(parentModel);
                }


            }
            master.Remove(master.Length - 3, 3);
            // master.AppendLine();
            csvOut = master.ToString();

            return csvOut;


            //FileInfo outputFileInfo = new FileInfo(absolutePathForGenerateReadableFile);
            //if (!Directory.Exists(outputFileInfo.DirectoryName))
            //    Directory.CreateDirectory(outputFileInfo.DirectoryName);



            //string filename = String.Format("{0}\\{1}{2}", outputFileInfo.DirectoryName, outputFileInfo.Name.Replace(outputFileInfo.Extension, ""), outputFileInfo.Extension);
            //using (Stream outputStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            //{
            //    using (StreamWriter writer = new StreamWriter(outputStream))
            //    {
            //        writer.Write(csvOut);
            //        writer.Close();
            //    }
            //    outputStream.Close();
            //}







            //return csvOut;
        }





























        #region Function

        public static string GetClaimStatusCategoryCodes(string value)
        {
            value = value.Trim();
            switch (value)
            {
                case "A0": return "Acknowledgement/Forwarded-The claim/encounter has been forwarded to another entity.";
                case "A1": return "Acknowledgement/Receipt-The claim/encounter has been received. This does not mean that the claim has been accepted for adjudication.";
                case "A2": return "Acknowledgement/Acceptance into adjudication system-The claim/encounter has been accepted into the adjudication system.";
                case "A3": return "Acknowledgement/Returned as unprocessable claim-The claim/encounter has been rejected and has not been entered into the adjudication system.";
                case "A4": return "Acknowledgement/Not Found-The claim/encounter can not be found in the adjudication system.";
                case "A5": return "Acknowledgement/Split Claim-The claim/encounter has been split upon acceptance into the adjudication system.";
                case "A6": return "Acknowledgement/Rejected for Missing Information - The claim/encounter is missing the information specified in the Status details and has been rejected.";
                case "A7": return "Acknowledgement/Rejected for Invalid Information - The claim/encounter has invalid information as specified in the Status details and has been rejected.";
                case "A8": return "Acknowledgement / Rejected for relational field in error.";

                default: return value;
            }
        }

        public static string GetClaimStatus(string value)
        {
            value = value.Trim();
            switch (value)
            {
                case "U": return "Rejected";
                case "WQ": return "Accepted"; // Accepted
                case "15": return "Correct and Resubmit Claim";
                case "F": return "Final";
                default: return value;
            }
        }


        public static long GetClaimDetails(string value, string type,int order=1)
        {
            value = value.Trim();

            if (value.Contains("ZRPB") && value.Contains("BN"))
            {
                string[] strTemp = value.Split(new string[] { "ZRPB" },
                    StringSplitOptions.None);
                long noteId = Convert.ToInt64(strTemp[0]);
                long batchId = Convert.ToInt64(strTemp[1].Split(new string[] { "BN" }, StringSplitOptions.None)[0]);
                long batchNoteId = Convert.ToInt64(strTemp[1].Split(new string[] { "BN" }, StringSplitOptions.None)[1]);

                if (type == "Note")
                    return noteId;
                if (type == "Batch")
                    return batchId;
                if (type == "BatchNote")
                    return batchNoteId;

            }
            else if (value.Contains("N"))
            {

                string[] strTemp = value.Trim('N').Split(new string[] { "N" }, StringSplitOptions.None);
                if(order==1)
                return  Convert.ToInt64(strTemp[0]);
                if(order==2)
                return Convert.ToInt64(strTemp[1]);
            }
            return 0;

        }

       

        #endregion


    }


    public class Ack277Model
    {
        // Batch #, Note #, Claim #, Payor Claim#, Service Date
        public string Source { get; set; }
        public string ReceiptDate { get; set; }
        public string ProcessDate { get; set; }
        public string Receiver { get; set; }
        public string ReceiverLvlStatus { get; set; }
        public string ReceiverLvlAction { get; set; }
        public string ReceiverLvlMessage { get; set; }

        public string ProviderLvlStatus { get; set; }
        public string ProviderLvlAction { get; set; }
        public string ProviderLvlMessage { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Ahcccs { get; set; }
        public string PatientLvlStatus { get; set; }
        public string PatientLvlAction { get; set; }
        public string PatientLvlMessage { get; set; }
        public string PatientLvlAmount { get; set; }

        public string Batch { get; set; }
        public string Note { get; set; }
        public string Claim { get; set; }
        public string PayorClaim { get; set; }
        public string ServiceDate { get; set; }
    }

    public class CustomXsltExtensionFunctions277
    {
        public const string Namespace = "http://XsltSampleSite.XsltFunctions/1.0";



        public string GetClaimStatusCategoryCodes(string value)
        {
            value = value.Trim();
            switch (value)
            {
                case "A0": return "Acknowledgement/Forwarded-The claim/encounter has been forwarded to another entity.";
                case "A1": return "Acknowledgement/Receipt-The claim/encounter has been received. This does not mean that the claim has been accepted for adjudication.";
                case "A2": return "Acknowledgement/Acceptance into adjudication system-The claim/encounter has been accepted into the adjudication system.";
                case "A3": return "Acknowledgement/Returned as unprocessable claim-The claim/encounter has been rejected and has not been entered into the adjudication system.";
                case "A4": return "Acknowledgement/Not Found-The claim/encounter can not be found in the adjudication system.";
                case "A5": return "Acknowledgement/Split Claim-The claim/encounter has been split upon acceptance into the adjudication system.";
                case "A6": return "Acknowledgement/Rejected for Missing Information - The claim/encounter is missing the information specified in the Status details and has been rejected.";
                case "A7": return "Acknowledgement/Rejected for Invalid Information - The claim/encounter has invalid information as specified in the Status details and has been rejected.";
                case "A8": return "Acknowledgement / Rejected for relational field in error.";

                default: return value;
            }
        }

        public string GetClaimStatus(string value)
        {
            value = value.Trim();
            switch (value)
            {
                case "U": return "U - Rejected";
                case "WQ": return "Accepted"; // Accepted
                case "15": return "Correct and Resubmit Claim";
                case "F": return "Final";
                default: return value;
            }
        }


        public long GetClaimDetails(string value, string type)
        {
            value = value.Trim();

            if (value.Contains("ZRPB") && value.Contains("BN"))
            {
                string[] strTemp = value.Split(new string[] { "ZRPB" },
                    StringSplitOptions.None);
                long noteId = Convert.ToInt64(strTemp[0]);
                long batchId = Convert.ToInt64(strTemp[1].Split(new string[] { "BN" }, StringSplitOptions.None)[0]);
                long batchNoteId = Convert.ToInt64(strTemp[1].Split(new string[] { "BN" }, StringSplitOptions.None)[1]);

                if (type == "Note")
                    return noteId;
                if (type == "Batch")
                    return batchId;

            }
            return 0;

        }

    }


}
