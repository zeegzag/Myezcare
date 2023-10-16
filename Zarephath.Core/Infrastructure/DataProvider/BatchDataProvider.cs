using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using DocumentFormat.OpenXml.Bibliography;
using EDI_837_835_HCCP;
using ExportToExcel;
using iTextSharp.text.pdf.qrcode;
using OopFactory.Edi835Parser.Models;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;
using OpenXml.Excel.Data;
using System.Data;
using Zarephath.Core.Infrastructure.EdiValidation;
using EDI_837_835_HCCP.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class BatchDataProvider : BaseDataProvider, IBatchDataProvider
    {
        CacheHelper _cacheHelper = new CacheHelper();

        #region Batch Creation And Listing

        #region Add Batch

        public ServiceResponse SetAddBatchPage(long batchId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "BatchID", Value = batchId.ToString()}
                };
            AddBatchModel addBatchModel = GetMultipleEntity<AddBatchModel>(StoredProcedure.GetSetAddBatchPage, searchParam);
            addBatchModel.BatchStatusFilter = Common.SetBatchStatusFilter();

            if (addBatchModel.Batch == null)
                addBatchModel.Batch = new Batch();

            addBatchModel.Batch.IsSentStatus = -1;
            response.Data = addBatchModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse AddBatchDetail(AddBatchModel addBatchModel, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();
            if (addBatchModel.Batch != null)
            {
                //response.Data = SaveObject(addBatchModel.Batch, loggedInUserId);

                //List of BatchNoteStatusID for check NoteID is already exist for this status
                string listBatchNoteStatusId = Convert.ToInt16(EnumBatchNoteStatus.Addded) + "," +
                    Convert.ToInt16(EnumBatchNoteStatus.ValidateFailed) + "," + Convert.ToInt16(EnumBatchNoteStatus.ValidateSuccess) + "," +
                    Convert.ToInt16(EnumBatchNoteStatus.MarkAsSent) + ","
                    + Convert.ToInt16(EnumBatchNoteStatus.MarkAsUnSent) + "," + Convert.ToInt16(EnumBatchNoteStatus.Approved);

                List<SearchValueData> searchParam = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "BatchID", Value =Convert.ToString(addBatchModel.Batch.BatchID)},
                        new SearchValueData {Name = "BatchTypeID", Value =Convert.ToString(addBatchModel.Batch.BatchTypeID)},
                        new SearchValueData {Name = "PayorID", Value =Convert.ToString(addBatchModel.Batch.PayorID)},
                        new SearchValueData {Name = "ApprovedFacilityIds", Value =Convert.ToString(addBatchModel.Batch.BillingProviderIDs)},
                        new SearchValueData {Name = "StartDate", Value =Convert.ToDateTime(addBatchModel.Batch.StartDate).ToString(Constants.DbDateFormat)},
                        new SearchValueData {Name = "EndDate", Value =Convert.ToDateTime(addBatchModel.Batch.EndDate).ToString(Constants.DbDateFormat)},
                        new SearchValueData {Name = "CreatedBy", Value =Convert.ToString(loggedInUserId)},
                        new SearchValueData {Name = "BatchNoteStatusID", Value =Convert.ToInt16(EnumBatchNoteStatus.Addded).ToString()},
                        new SearchValueData {Name = "ListBatchNoteStatusID", Value =Convert.ToString(listBatchNoteStatusId)},
                        new SearchValueData {Name = "Comment", Value =Convert.ToString(addBatchModel.Batch.Comment)},

                     };

                if (addBatchModel.Batch.BatchTypeID == (int)EnumBatchType.Adjustment_Void_Replace_Submission && addBatchModel.Batch.OldVoidReplacement)
                {
                    GetScalar(StoredProcedure.SaveNewBatch, searchParam);
                }
                else
                {
                    searchParam.Add(new SearchValueData { Name = "ServiceCodeIDs", Value = Convert.ToString(addBatchModel.Batch.ServiceCodeIDs) });
                    GetScalar(StoredProcedure.SaveNewBatch01, searchParam);
                }

                response.Message = string.Format(Resource.RecordCreatedSuccessfully, Resource.Batch);
                response.IsSuccess = true;
            }
            return response;
        }

        public ServiceResponse GetApprovedPayorsList(string payorid)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData> { new SearchValueData { Name = "PayorID ", Value = payorid } };
                List<ApprovedFacilityList> approvedFacilityList = GetEntityList<ApprovedFacilityList>(StoredProcedure.GetApprovedPayorsList, searchParam);
                response.Data = approvedFacilityList;
                response.IsSuccess = true;
                return response;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        #endregion

        #region Batch List

        public ServiceResponse GetBatchList(SearchBatchList searchBatchList, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                SetSearchFilterForBatchList(searchBatchList, searchList);
                Page<ListBatchModel> batchListPage = GetEntityPageList<ListBatchModel>(StoredProcedure.GetBatchList01,
                                                                    searchList, pageSize, pageIndex, sortIndex, sortDirection);
                response.IsSuccess = true;
                response.Data = batchListPage;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        private static void SetSearchFilterForBatchList(SearchBatchList searchBatchList, List<SearchValueData> searchList)
        {
            if (searchBatchList.BatchTypeID > 0)
                searchList.Add(new SearchValueData { Name = "BatchTypeID", Value = Convert.ToString(searchBatchList.BatchTypeID) });

            if (searchBatchList.PayorID > 0)
                searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchBatchList.PayorID) });

            if (searchBatchList.BillingProviderIDS != null)
                searchList.Add(new SearchValueData { Name = "BillingProviderIDS", Value = Convert.ToString(searchBatchList.BillingProviderIDS) });

            if ((searchBatchList.StartDate.HasValue && searchBatchList.StartDate.Value.ToString(Constants.DefaultDateFormat) != Constants.DefaultDateValue))
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchBatchList.StartDate).ToString(Constants.DbDateFormat) });

            if ((searchBatchList.EndDate.HasValue && searchBatchList.EndDate.Value.ToString(Constants.DefaultDateFormat) != Constants.DefaultDateValue))
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchBatchList.EndDate).ToString(Constants.DbDateFormat) });

            searchList.Add(new SearchValueData { Name = "IsSentStatus", Value = Convert.ToString(searchBatchList.IsSentStatus) });
        }

        public ServiceResponse DeleteBatch(SearchBatchList searchBatchList, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {

                List<Batch> unsentBatches = GetEntityList<Batch>(StoredProcedure.GetUnsentBatches, new List<SearchValueData>
                {
                    new SearchValueData
                        {
                           Name = "ListOfIdsInCSV",
                           Value = searchBatchList.ListOfIdsInCSV
                        }
                });

                foreach (var batchid in unsentBatches.Select(m => m.BatchID))
                {

                    string edifilesPath = string.Format("{0}{1}{2}{3}{4}", ConfigSettings.AmazoneUploadPath,
                                                        ConfigSettings.EdiFilePath, ConfigSettings.EdiFileUploadPath,
                                                        ConfigSettings.EdiFile837Path, batchid);

                    AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                    amazonFileUpload.DeleteAllObjectsInFolder(ConfigSettings.ZarephathBucket, edifilesPath);

                    string edivalidationerrorfilesPath = string.Format("{0}{1}{2}{3}{4}", ConfigSettings.AmazoneUploadPath,
                                                      ConfigSettings.EdiFilePath, ConfigSettings.TempEdiFileValidationErrorPath,
                                                      ConfigSettings.EdiFile837Path, batchid);
                    amazonFileUpload.DeleteAllObjectsInFolder(ConfigSettings.ZarephathBucket, edivalidationerrorfilesPath);
                }



                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                SetSearchFilterForBatchList(searchBatchList, searchList);

                if (!string.IsNullOrEmpty(searchBatchList.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = "," + searchBatchList.ListOfIdsInCSV + "," });
                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

                List<ListBatchModel> totalData = GetEntityList<ListBatchModel>(StoredProcedure.DeleteBatch, searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListBatchModel> listBatchLModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = listBatchLModel;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.Batch);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse MarkAsSentBatch(long loggedInUserId, SearchBatchList searchBatchList, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                SetSearchFilterForBatchList(searchBatchList, searchList);

                if (!string.IsNullOrEmpty(searchBatchList.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchBatchList.ListOfIdsInCSV });
                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                searchList.Add(new SearchValueData { Name = "MarkAsSentStatus", Value = Convert.ToInt16(EnumBatchNoteStatus.MarkAsSent).ToString() });
                searchList.Add(new SearchValueData { Name = "MarkAsUnSentStatus", Value = Convert.ToInt16(EnumBatchNoteStatus.MarkAsUnSent).ToString() });
                searchList.Add(new SearchValueData { Name = "SentReason", Value = Convert.ToString(Constants.MarkAsSent) });
                searchList.Add(new SearchValueData { Name = "UnSentReason", Value = Convert.ToString(Constants.MarkAsUnSent) });
                searchList.Add(new SearchValueData { Name = "IsSentBy", Value = Convert.ToString(loggedInUserId) });
                searchList.Add(new SearchValueData { Name = "IsSent", Value = Convert.ToString(searchBatchList.IsSent) });
                List<ListBatchModel> totalData = GetEntityList<ListBatchModel>(StoredProcedure.SetMarkasSentBatch, searchList);
                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListBatchModel> listBatchLModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = listBatchLModel;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Batch);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.ErrorOccured, Resource.ExceptionMessage);
            }
            return response;
        }


        public ServiceResponse GenrateOverViewFile(string csvBatchId, long loogedInID)
        {
            var response = new ServiceResponse();

            #region Declare Path

            var batchOverviewPath = string.Format("batchOverviewPath/{0}/", loogedInID);
            var batchOverviewPathZipPath = "batchOverviewPath/";

            #endregion

            DownloadFileModel downloadFileModel = new DownloadFileModel();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "BatchID", Value = Convert.ToString(csvBatchId) });

            List<ListOverViewFileModel> listOverViewFileModel = GetEntityList<ListOverViewFileModel>(StoredProcedure.GetOverviewFileList01, searchList);

            string basePath = HttpContext.Current.Server.MapCustomPath(String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain) + batchOverviewPath);



            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            string[] batchidnew = csvBatchId.Split(',');
            string batchIds = "";
            if (batchidnew.Count() > 1)
            {
                List<ListOverViewFileModel> firstFile = new List<ListOverViewFileModel>();
                List<List<ListOverViewFileModel>> listOfOtherBatches = new List<List<ListOverViewFileModel>>();
                for (int i = 0; i < batchidnew.Count(); i++)
                {


                    var listItem =
                        listOverViewFileModel.Where(k => k.BatchID == Convert.ToInt32(batchidnew[i])).ToList();



                    if (i == 0)
                    {
                        firstFile = listItem;
                        batchIds = batchidnew[i];
                        ;
                    }
                    else
                    {
                        listOfOtherBatches.Add(listItem);
                        batchIds = batchIds + "_" + batchidnew[i];
                    }

                    #region TO ZIP DOWNLOAD
                    //string fileName = string.Format("{0}_Batch_#{1}_{2}", Constants.OverViewFile, batchidnew[i],
                    //                               DateTime.Now.ToString(Constants.FileNameDateTimeFormat) + i);

                    //var absolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                    //CreateExcelFile.CreateExcelDocument(listItem, absolutePath);
                    #endregion
                }
                string fileName = string.Format("{0}_Batch_{1}_{2}", Constants.OverViewFile, batchIds,
                                                   DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                var absolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                CreateExcelFile.CreateExcelDocument(firstFile, absolutePath, listOfOtherBatches);
                downloadFileModel.VirtualPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain) + batchOverviewPath + fileName + Constants.Extention_xlsx;
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                downloadFileModel.AbsolutePath = "false";

                #region TO ZIP DOWNLOAD
                //string zipFile = HttpContext.Current.Server.MapCustomPath(_cacheHelper.ReportExcelFileUploadPath);
                //ZipFile.CreateFromDirectory(basePath, zipFile + batchOverviewPathZipPath + loogedInID + Constants.Extention_zip, CompressionLevel.NoCompression, false);
                //downloadFileModel.VirtualPath = zipFile + batchOverviewPathZipPath + loogedInID + Constants.Extention_zip;
                //downloadFileModel.FileName = loogedInID + Constants.Extention_zip;
                //downloadFileModel.AbsolutePath = "true";

                //DirectoryInfo di = new DirectoryInfo(basePath);
                //foreach (FileInfo file in di.GetFiles())
                //    file.Delete();
                //foreach (DirectoryInfo dir in di.GetDirectories())
                //    dir.Delete(true);
                #endregion

            }
            else
            {
                batchIds = csvBatchId;
                string fileName = string.Format("{0}_Batch_{1}_{2}", Constants.OverViewFile, batchIds, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                var absolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                CreateExcelFile.CreateExcelDocument(listOverViewFileModel, absolutePath);
                downloadFileModel.VirtualPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain) + batchOverviewPath + fileName + Constants.Extention_xlsx;
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                downloadFileModel.AbsolutePath = "false";
            }
            response.IsSuccess = true;
            response.Data = downloadFileModel;
            return response;
        }

        public ServiceResponse GenratePaperRemitsEOBTemplate(string csvBatchId, long loogedInID)
        {
            var response = new ServiceResponse();

            #region Declare Path

            var paperRemitsEOBPath = string.Format("PaperRemitsEOBTemplate/{0}/", loogedInID);

            #endregion

            DownloadFileModel downloadFileModel = new DownloadFileModel();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "BatchID", Value = Convert.ToString(csvBatchId) });

            List<ListPaperRemitModel> listModel = GetEntityList<ListPaperRemitModel>(StoredProcedure.GenratePaperRemitsEOBTemplate, searchList);

            string basePath = HttpContext.Current.Server.MapCustomPath(String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain) + paperRemitsEOBPath);

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            string fileName = string.Format("{0}_Batch_{1}", Constants.PaperRemitsEOBTemplate, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
            var absolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_csv);
            //CreateExcelFile.CreateExcelDocument(listModel, absolutePath);
            CreateExcelFile.CreateCsvFromList(listModel, absolutePath);
            downloadFileModel.VirtualPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain) + paperRemitsEOBPath + fileName + Constants.Extention_csv;
            downloadFileModel.FileName = fileName + Constants.Extention_csv;
            downloadFileModel.AbsolutePath = "false";



            response.IsSuccess = true;
            response.Data = downloadFileModel;
            return response;
        }
        #endregion

        #endregion

        #region Batch Validation And 837 Generation

        #region Batch Validation And 837 Generation

        public ServiceResponse ValidateBatchesAndGenerateEdi837Files(PostEdiValidateGenerateModel postEdiValidateGenerateModel, long loggedInUserId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            Edi837 edi837 = new Edi837();
            string filePath = String.Format(_cacheHelper.EdiFile837UploadPath, _cacheHelper.Domain);

            List<string> batchIds = string.IsNullOrEmpty(postEdiValidateGenerateModel.ListOfBacthIdsInCsv) ? new List<string>()
                : postEdiValidateGenerateModel.ListOfBacthIdsInCsv.Split(',').ToList();

            List<BatchValidationResponseModel> listBrm = new List<BatchValidationResponseModel>();
            foreach (var batchId in batchIds)
            {
                string initFName = "PY_05012017_05152017_IS";
                filePath = String.Format(_cacheHelper.EdiFile837UploadPath, _cacheHelper.Domain);
                filePath = string.Format("{0}{1}/", filePath, batchId);
                string tempFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");// +"_" + Guid.NewGuid().ToString();


                BatchValidationResponseModel batchValidationResponseModel = new BatchValidationResponseModel();
                batchValidationResponseModel.BatchID = Convert.ToInt64(batchId);


                #region DO validation

                var searchList = new List<SearchValueData>() { new SearchValueData { Name = "BatchID", Value = Convert.ToString(batchId) } };
                ParentBatchRelatedAllDataModel model = null;
                try
                {
                    model = GetMultipleEntity<ParentBatchRelatedAllDataModel>(StoredProcedure.GetBatchRelatedAllData01, searchList);
                    tempFileName = string.Format("{0}", model.FileName, tempFileName);
                    CheckAndGenerateBatchValidationErrorCsv(model, ref batchValidationResponseModel);
                    //model.BatchRelatedAllDataModel = model.BatchRelatedAllDataModel.Where(c => c.IsUseInBilling).ToList();
                    //UpdateBillingUnitAndAmount(ref model);

                }
                catch (Exception ex)
                {
                    string fname = string.Format("edi837_validation_exce_error_{0}.txt", tempFileName);
                    GenerationBatchExceptionMsg(ref batchValidationResponseModel, (int)EnumEdiFileTypes.Edi837ValidationError, fname, ex);

                    //string message = string.Format("{1}{0}{2}{0}{3}", Environment.NewLine, ex.Message, ex.StackTrace, ex.Source);
                    //string fname = string.Format("edi837_validation_exce_error_{0}_", tempFileName);
                    //string errofilePath = Common.CreateLogFile(message, fname, _cacheHelper.EdiFile837ValidationErrorPath);
                    //batchValidationResponseModel.FileName = string.Format("{0}.txt", fname);
                    //batchValidationResponseModel.EdiFileTypeID = (int)EnumEdiFileTypes.Edi837ValidationError;
                    //batchValidationResponseModel.Edi837FilePath = errofilePath;
                }
                #endregion
                string fileName = tempFileName + postEdiValidateGenerateModel.FileExtension;
                batchValidationResponseModel.FileName = fileName;
                if (batchValidationResponseModel.ValidationPassed && postEdiValidateGenerateModel.GenerateEdiFile && model != null)
                {
                    try
                    {
                        PayorEdi837Setting payorEdi837Setting = model.PayorEdi837Setting;
                        Edi837Model edi837Model = GetEdit837Model(Convert.ToInt64(batchId), ref payorEdi837Setting, model.BatchRelatedAllDataModel);

                        //if (postEdiValidateGenerateModel.FileExtension.ToLower() == "csv")
                        //    edi837.GenerateEdi837CSVFile(edi837Model, filePath, fileName);
                        //else
                        //    edi837.GenerateEdi837File(edi837Model, filePath, fileName);

                        string fileServerPath = HttpContext.Current.Server.MapCustomPath(filePath);
                        string generatedFilePath = edi837.GenerateEdi837File(edi837Model, fileServerPath, fileName);

                        SaveEntity(payorEdi837Setting);

                        //string fullpath = HttpContext.Current.Server.MapCustomPath(string.Format("{0}{1}", filePath, fileName));
                        //Common.DeleteFile(fullpath);
                        batchValidationResponseModel.FileName = fileName;
                        batchValidationResponseModel.EdiFileTypeID = (int)EnumEdiFileTypes.Edi837;
                        batchValidationResponseModel.Edi837FilePath = string.Format("{0}{1}", filePath, fileName);
                        batchValidationResponseModel.Edi837GenerationPassed = true;
                    }
                    catch (Exception ex)
                    {
                        string fname = string.Format("edi837_generate_exce_error_{0}.txt", tempFileName);
                        GenerationBatchExceptionMsg(ref batchValidationResponseModel, (int)EnumEdiFileTypes.Edi837GenerationError, fname, ex);
                        //string message = string.Format("{1}{0}{2}{0}{3}", Environment.NewLine, ex.Message, ex.StackTrace, ex.Source);
                        //string fname = string.Format("edi837_generate_exce_error_{0}_", tempFileName);
                        //string errofilePath = Common.CreateLogFile(message, fname, _cacheHelper.EdiFile837ValidationErrorPath);
                        //batchValidationResponseModel.FileName = string.Format("{0}.txt", fname);
                        //batchValidationResponseModel.EdiFileTypeID = (int)EnumEdiFileTypes.Edi837GenerationError;
                        //batchValidationResponseModel.Edi837FilePath = errofilePath;
                    }
                }
                if (batchValidationResponseModel.EdiFileTypeID > 0)
                {
                    EdiFileLog ediFileLog = new EdiFileLog();
                    ediFileLog.BatchID = batchValidationResponseModel.BatchID;
                    ediFileLog.FileName = batchValidationResponseModel.FileName;
                    ediFileLog.EdiFileTypeID = batchValidationResponseModel.EdiFileTypeID;
                    ediFileLog.FilePath = batchValidationResponseModel.ValidationPassed ? batchValidationResponseModel.Edi837FilePath : batchValidationResponseModel.ValidationErrorFilePath;
                    ediFileLog.FileSize = Common.GetFileSizeInBytes(HttpContext.Current.Server.MapCustomPath(ediFileLog.FilePath)).ToString();

                    #region amazonefileupload
                    AmazonFileUpload amazoneFileUpload = new AmazonFileUpload();
                    string fullpath = HttpContext.Current.Server.MapCustomPath(ediFileLog.FilePath);
                    ediFileLog.FilePath = amazoneFileUpload.UploadFile(ConfigSettings.ZarephathBucket, ediFileLog.FilePath.TrimStart('/'), fullpath, true);
                    if (batchValidationResponseModel.ValidationPassed)
                    {
                        batchValidationResponseModel.Edi837FilePath =
                            amazoneFileUpload.GetPreSignedUrl(ConfigSettings.ZarephathBucket, ediFileLog.FilePath);
                    }
                    else
                    {
                        batchValidationResponseModel.ValidationErrorFilePath =
                            amazoneFileUpload.GetPreSignedUrl(ConfigSettings.ZarephathBucket, ediFileLog.FilePath);
                    }

                    #endregion amazonefileupload

                    SaveObject(ediFileLog, loggedInUserId);
                }
                listBrm.Add(batchValidationResponseModel);
            }


            serviceResponse.IsSuccess = true;
            serviceResponse.Data = listBrm;
            return serviceResponse;
        }

        #endregion

        #region Private Funcation for Generate 837 File

        #region Batch And Batch Note Validation Check
        private void CheckAndGenerateBatchValidationErrorCsv(ParentBatchRelatedAllDataModel model, ref BatchValidationResponseModel batchValidationResponseModel)
        {
            StreamWriter clssw = null;

            string virtualCsvFileBasePath = String.Format(_cacheHelper.EdiFile837ValidationErrorPath, _cacheHelper.Domain);
            string tempFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string fileName = string.Format("edi837_validation_error_{0}{1}", tempFileName, ".csv");
            try
            {
                #region LOOP EACH NOTE DETAILS AND CHECK/VALIDATE REQUIRED INFORMATIONS ARE AVAILABLE OR NOT

                int errorNoteCount = 0;



                foreach (BatchRelatedAllDataModel item in model.BatchRelatedAllDataModel)
                {
                    int errorCount = 0;
                    #region Check Each Note Related Missing Informations
                    string errorMessages = "";

                    //item.PayorName
                    //    item.CISNumber
                    //item.Dob = "";
                    //item.Gender = "";
                    //item.ContinuedDX = "";
                    //item.BillingProviderName = "";
                    //item.BillingProviderState = "";
                    //item.PayorAddress = "";
                    //item.RenderingProviderEIN = "";
                    //item.PayorCity = "";



                    if (item.BatchTypeID == (int)EnumBatchType.Adjustment_Void_Replace_Submission)
                    {
                        if (string.IsNullOrEmpty(item.Original_PayerClaimControlNumber))
                        {
                            {
                                errorCount = errorCount + 1;
                                string msg = string.Format("PayerClaimControlNumber is missing. It seems this note is not process anytime and we are trying to send it into Adjustment.");
                                SetError(errorCount, ref errorMessages, msg);
                            }
                        }

                    }


                    if (item.PayorID != (long)Payor.PayorCode.PY && string.IsNullOrEmpty(item.CISNumber))
                    {
                        errorCount = errorCount + 1;
                        string msg = string.Format("Client's {0} is missing.",
                                                   string.IsNullOrEmpty(item.CISNumber) ? "CIS Number" : "");
                        SetError(errorCount, ref errorMessages, msg);
                    }
                    else if (item.PayorID == (long)Payor.PayorCode.PY && string.IsNullOrEmpty(item.AHCCCSID))
                    {
                        {
                            errorCount = errorCount + 1;
                            string msg = string.Format("Client's {0} is missing.",
                                                       string.IsNullOrEmpty(item.AHCCCSID) ? "AHCCCS ID" : "");
                            SetError(errorCount, ref errorMessages, msg);
                        }
                    }

                    if (model.PayorEdi837Setting.CheckForPolicyNumber && string.IsNullOrEmpty(item.PolicyNumber))
                    {
                        {
                            errorCount = errorCount + 1;
                            string msg = string.Format("Client's {0} is missing.",
                                                       string.IsNullOrEmpty(item.PolicyNumber) ? "Group / Policy Number" : "");
                            SetError(errorCount, ref errorMessages, msg);
                        }
                    }

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
                            var dxCodeGroup = dXCodeDetails.Split(':').Length > 2
                                                     ? dXCodeDetails.Split(':')[2]
                                                     : dxCodeType == Constants.DXCodeType_ICD10_Primary
                                                           ? Constants.DXCodeGroup_ICD10
                                                           : Constants.DXCodeGroup_ICD09;



                            if (dxCodeGroup == Constants.DXCodeGroup_ICD09)
                            {

                                if (string.IsNullOrEmpty(icd9dxCodeErrorMessage))
                                    icd9dxCodeErrorMessage = string.Format("ICD-9 DX Code found. DX Codes {0}(#{1}), ", dxCode, dxCodeType);
                                else
                                    icd9dxCodeErrorMessage = dxCodeErrorMessage + string.Format("{0}(#{1}), ", dxCode, dxCodeType);
                            }




                            var count = i + 1;
                            if (count == 1)
                            {
                                // ABK:TX23403:ICD-10 
                                primaryDxCodeType = dxCodeType;
                                primaryDxCode = dxCode;
                                primaryDxCodeGroup = dxCodeGroup;
                            }

                            if (count > 1)
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

                    // CHECK RENDERRING PROCIDER RELATED MISSSING INFORMATIONS
                    if (string.IsNullOrEmpty(item.RenderingProviderName) || string.IsNullOrEmpty(item.RenderingProviderAddress)
                        || string.IsNullOrEmpty(item.RenderingProviderCity) || string.IsNullOrEmpty(item.RenderingProviderState)
                        || string.IsNullOrEmpty(item.RenderingProviderZipcode) || string.IsNullOrEmpty(item.RenderingProviderNPI)
                        || string.IsNullOrEmpty(item.RenderingProviderEIN))
                    {
                        errorCount = errorCount + 1;
                        string msg = string.Format("Rendering provider's {0}{1}{2}{3}{4}{5}{6} details are missing.",
                                                   string.IsNullOrEmpty(item.RenderingProviderName) ? "name," : "",
                                                   string.IsNullOrEmpty(item.RenderingProviderAddress) ? "address," : "",
                                                   string.IsNullOrEmpty(item.RenderingProviderCity) ? "city," : "",
                                                   string.IsNullOrEmpty(item.RenderingProviderState) ? "state," : "",
                                                   string.IsNullOrEmpty(item.RenderingProviderZipcode) ? "zipcode," : "",
                                                   string.IsNullOrEmpty(item.RenderingProviderNPI) ? "national provider identifier number," : "",
                                                   string.IsNullOrEmpty(item.RenderingProviderEIN) ? "employer identification number" : "");
                        SetError(errorCount, ref errorMessages, msg);
                    }

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

                    #region IF ERROR FOUND THEN OPEN CSV FILE AND WRITE INTO IT
                    if (errorCount > 0)
                    {
                        if (errorNoteCount == 0)
                        {
                            #region Set CSV File Header
                            string absoluteCsvFileBasePath = HttpContext.Current.Server.MapCustomPath(virtualCsvFileBasePath);
                            if (!Directory.Exists(absoluteCsvFileBasePath))
                                Directory.CreateDirectory(absoluteCsvFileBasePath);

                            clssw = clssw ?? new StreamWriter(absoluteCsvFileBasePath + fileName);

                            clssw.Write("RefferalID" + ",");
                            clssw.Write("FirstName" + ",");
                            clssw.Write("LastName" + ",");
                            clssw.Write("AHCCCSID #" + ",");
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
                if ((model.BatchRelatedAllDataModel == null || model.BatchRelatedAllDataModel.Count == 0) &&
                    (model.PayorEdi837Setting == null || model.PayorEdi837Setting.PayorEdi837SettingId == 0))
                {
                    string fname = string.Format("edi837_validation_exce_error_{0}.txt", tempFileName);
                    GenerationBatchExceptionMsg(ref batchValidationResponseModel, (int)EnumEdiFileTypes.Edi837ValidationError, fname, null, Resource.PayorSettingMissing);
                    return;
                    //string message = string.Format("{0}", Resource.PayorSettingMissing);
                    //string fname = string.Format("edi837_validation_exce_error_{0}_", tempFileName);
                    //string errofilePath = Common.CreateLogFile(message, fname, _cacheHelper.EdiFile837ValidationErrorPath);
                    //batchValidationResponseModel.FileName = string.Format("{0}.txt", fname);
                    //batchValidationResponseModel.EdiFileTypeID = (int)EnumEdiFileTypes.Edi837ValidationError;
                    //batchValidationResponseModel.Edi837FilePath = errofilePath;
                }



                if (errorNoteCount == 0)
                    batchValidationResponseModel.ValidationPassed = true;
                else
                {
                    batchValidationResponseModel.FileName = fileName;
                    batchValidationResponseModel.EdiFileTypeID = (int)EnumEdiFileTypes.Edi837ValidationError;
                    batchValidationResponseModel.ValidationErrorFilePath = string.Format("{0}{1}", virtualCsvFileBasePath, fileName);
                }
                #endregion
            }
            catch (Exception ex)
            {
                string fname = string.Format("edi837_validation_exce_error_{0}.txt", tempFileName);
                GenerationBatchExceptionMsg(ref batchValidationResponseModel, (int)EnumEdiFileTypes.Edi837ValidationError, fname, ex);

                //string message = string.Format("{1}{0}{2}{0}{3}", Environment.NewLine, ex.Message, ex.StackTrace, ex.Source);
                //string fname = string.Format("edi837_validation_exce_error_{0}_", tempFileName);
                //string errofilePath = Common.CreateLogFile(message, fname, _cacheHelper.EdiFile837ValidationErrorPath);
                //batchValidationResponseModel.FileName = string.Format("{0}.txt", fname);
                //batchValidationResponseModel.EdiFileTypeID = (int)EnumEdiFileTypes.Edi837ValidationError;
                //batchValidationResponseModel.Edi837FilePath = errofilePath;
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

        private void UpdateBillingUnitAndAmount(ref ParentBatchRelatedAllDataModel model)
        {
            //BillingUnitLimit
            foreach (var item in model.BatchRelatedAllDataModel)
            {
                DateTime date = new DateTime(2017, 09, 1);
                if (item.BillingUnitLimit.HasValue && item.CalculatedUnit > item.BillingUnitLimit.Value && item.ServiceDate > date)
                {
                    var newUnit = item.BillingUnitLimit.Value;
                    var perUnitAmount = item.CalculatedAmount / item.CalculatedUnit;
                    var newAmount = newUnit * perUnitAmount;

                    item.CalculatedAmount = newAmount;
                    item.CalculatedUnit = newUnit;
                }

            }

        }

        private void SetError(int errorCounter, ref string mainErrorSource, string currentErrorMessage)
        {
            mainErrorSource = mainErrorSource + string.Format("{0}. {1}{2}", errorCounter, currentErrorMessage, Environment.NewLine);
        }

        private void GenerationBatchExceptionMsg(ref BatchValidationResponseModel batchValidationResponseModel, int ediFileTypeId, string fileName, Exception ex = null, string message = null)
        {

            if (ex != null)
                message = string.Format("{1}{0}{2}{0}{3}", Environment.NewLine, ex.Message, ex.StackTrace, ex.Source);
            string fname = fileName; // string.Format("edi837_generate_exce_error_{0}_", tempFileName);

            string ediFile837ValidationErrorPath = String.Format(_cacheHelper.EdiFile837ValidationErrorPath, _cacheHelper.Domain);
            string filePath = string.Format("{0}{1}/", ediFile837ValidationErrorPath, batchValidationResponseModel.BatchID);
            string errofilePath = Common.CreateLogFile(message, fname, filePath);
            batchValidationResponseModel.FileName = string.Format("{0}", fname);
            batchValidationResponseModel.EdiFileTypeID = ediFileTypeId;

            if (ediFileTypeId == (int)EnumEdiFileTypes.Edi837ValidationError)
                batchValidationResponseModel.ValidationErrorFilePath = errofilePath;
            else
                batchValidationResponseModel.Edi837FilePath = errofilePath;

        }

        #endregion

        #region Batch 837 Model Generation
        private Edi837Model GetEdit837Model(long batchID, ref PayorEdi837Setting payorEdi837Setting, List<BatchRelatedAllDataModel> batchRelatedAllDataList)
        {

            //if (payorEdi837Setting.ISA13_UpdatedDate == null || payorEdi837Setting.ISA13_UpdatedDate.Value.Date != DateTime.Now.Date)
            //{
            //    payorEdi837Setting.ISA13_InterchangeControlNo = 1;
            //    payorEdi837Setting.ISA13_UpdatedDate = DateTime.Now;
            //}
            //else if (payorEdi837Setting.ISA13_UpdatedDate.Value.Date == DateTime.Now.Date)
            //    payorEdi837Setting.ISA13_InterchangeControlNo = payorEdi837Setting.ISA13_InterchangeControlNo + 1;

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
                    IdCodeQualifier = payorEdi837Setting.BillingProvider_NM108_IdentificationCodeQualifier,
                    IdCodeQualifierEnum = tempBillingProvider.BillingProviderNPI,
                    AddressInformation = tempBillingProvider.BillingProviderAddress,
                    CityName = tempBillingProvider.BillingProviderCity,
                    StateOrProvinceCode = tempBillingProvider.BillingProviderState,
                    PostalCode = tempBillingProvider.BillingProviderZipcode,
                    ReferenceIdentificationQualifier = payorEdi837Setting.BillingProvider_REF01_ReferenceIdentificationQualifier,
                    ReferenceIdentification = tempBillingProvider.BillingProviderEIN
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

                        #region Add Claims
                        Claim claim = new Claim()
                        {
                            //C//ClaimSubmitterIdentifier = tempClaim.ClaimSubmitterIdentifier,

                            //PatientControlNumber = tempClaim.PatientAccountNumber,// "Referral|NoteID", // CLM01
                            //TotalClaimChargeAmount = tempClaim.CalculatedAmount.ToString(), // CLM02
                            FacilityCodeValue = tempClaim.PosID.ToString(), // CLM05-01
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

                        if (tempBillingProvider.BillingProviderNPI.Trim() != tempClaim.RenderingProviderNPI.Trim() || payorEdi837Setting.RequiredRenderingProvider)
                        {
                            #region Provider Information > Rendering Provider

                            claim.RenderingProviderEntityIdentifierCode =
                                payorEdi837Setting.Claim_RenderringProvider_NM01_EntityIdentifierCode; // NM101
                            claim.RenderingProviderEntityTypeQualifier =
                                payorEdi837Setting.Claim_RenderringProvider_NM02_EntityTypeQualifier; // NM102
                            claim.RenderingProviderNameLastOrOrganizationName = tempClaim.RenderingProviderName;
                            // NM103
                            claim.RenderingProviderIdCodeQualifier =
                                payorEdi837Setting.Claim_RenderringProvider_NM108_IdentificationCodeQualifier; // NM108
                            claim.RenderingProviderIdCodeQualifierEnum = tempClaim.RenderingProviderNPI; // NM109

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
                        #endregion Provider Information


                        var ces = payorEdi837Setting.ISA16_ComponentElementSeparator;
                        var secondaryDxCodeType = Constants.DXCodeType_ICD10_Secondary;
                        for (int i = 0; i < tempClaim.ContinuedDX.Split(',').Length; i++)
                        {
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
                        string strClaimId = string.Empty;
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
                                CompositeMedicalProcedureIdentifier = String.Format("{0}{1}{2}", payorEdi837Setting.Claim_ServiecLine_SV101_01_ProductServiceIDQualifier,
                                payorEdi837Setting.ISA16_ComponentElementSeparator, tempServiceLine.ServiceCode), // SV101-01, SV101-02

                                //MonetaryAmount = tempClaim.CalculatedAmount.ToString(),// SV102
                                MonetaryAmount = tempServiceLine.CalculatedAmount.ToString(),// SV102

                                UnitOrBasisForMeasurementCode = payorEdi837Setting.Claim_ServiecLine_SV103_BasisOfMeasurement,// SV103
                                Quantity = tempServiceLine.CalculatedUnit.ToString(),// SV104
                                //CompositeDiagnosisCodePointer = // SV107
                                CompositeDiagnosisCodePointer = payorEdi837Setting.Claim_ServiceLine_SV107_01_DiagnosisCodePointer,

                                DateTimeQualifier = payorEdi837Setting.Claim_ServiceLine_Date_DTP01_DateTimeQualifier,// DTP01
                                DateTimePeriodFormatQualifier = payorEdi837Setting.Claim_ServiceLine_Date_DTP02_DateTimePeriodFormatQualifier,// DTP02
                                DateTimePeriod = tempServiceLine.ServiceDateSpan,// DTP03

                                ReferenceIdentificationQualifier = payorEdi837Setting.Claim_ServiceLine_Ref_REF01_ReferenceIdentificationQualifier,// REF01
                                ReferenceIdentification = payorEdi837Setting.Claim_ServiceLine_Ref_REF02_ReferenceIdentification,// REF02

                                NTE02_Description = "Need To Rollup Claims Again And Resend",//tempClaim.ClaimAdjustmentReason
                            };

                            if (!string.IsNullOrEmpty(tempServiceLine.ModifierName) && batchID != 91)
                            {
                                serviceLine.CompositeMedicalProcedureIdentifier = String.Format("{0}{1}{2}",
                                    serviceLine.CompositeMedicalProcedureIdentifier, payorEdi837Setting.ISA16_ComponentElementSeparator, tempServiceLine.ModifierName);
                            }

                            #endregion
                            amount = amount + Convert.ToDouble(tempServiceLine.CalculatedAmount);
                            strClaimId = strClaimId + tempServiceLine.StrBathNoteID;
                            claim.ServiceLines.Add(serviceLine);
                        }
                        //new line
                        claim.TotalClaimChargeAmount = Convert.ToString(amount, CultureInfo.InvariantCulture); // CLM02
                        claim.ClaimSubmitterIdentifier = strClaimId;

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
        private GroupedModelFor837 GenerateGroupedModelFor837(List<BatchRelatedAllDataModel> batchRelatedAllDataList)
        {
            GroupedModelFor837 groupedModelFor837 = new GroupedModelFor837();
            #region Generate Group Data for 837 Model

            #region Group By Billing Provider
            List<BillingGroupClass> tempBillingGroupList = batchRelatedAllDataList.ToList().GroupBy(ac => new
            {
                ac.BillingProviderID,
                ac.BillingProviderName,
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
                    ac.PayorZipcode
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
                                                                             PayorZipcode = grp.Key.PayorZipcode
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
                        ac.RenderingProviderID,
                        ac.RenderingProviderName,
                        ac.RenderingProviderEIN,
                        ac.RenderingProviderNPI,
                        ac.RenderingProviderGSA,
                        ac.RenderingProviderAddress,
                        ac.RenderingProviderCity,
                        ac.RenderingProviderState,
                        ac.RenderingProviderZipcode,
                        ac.Submitted_ClaimAdjustmentTypeID,
                        ac.Original_PayerClaimControlNumber,
                        //ac.ClaimAdjustmentReason,
                        ac.ServiceDate,
                        ac.GroupIDForMileServices
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
                                                                         PosName = grp.Key.PosName,
                                                                         ContinuedDX = grp.Key.ContinuedDX,
                                                                         //ModifierID = grp.Key.ModifierID,
                                                                         //ModifierName = grp.Key.ModifierName,
                                                                         RenderingProviderID = grp.Key.RenderingProviderID,
                                                                         RenderingProviderName = grp.Key.RenderingProviderName,
                                                                         RenderingProviderEIN = grp.Key.RenderingProviderEIN,
                                                                         RenderingProviderNPI = grp.Key.RenderingProviderNPI,
                                                                         RenderingProviderGSA = grp.Key.RenderingProviderGSA,
                                                                         RenderingProviderAddress = grp.Key.RenderingProviderAddress,
                                                                         RenderingProviderCity = grp.Key.RenderingProviderCity,
                                                                         RenderingProviderState = grp.Key.RenderingProviderState,
                                                                         RenderingProviderZipcode = grp.Key.RenderingProviderZipcode,
                                                                         Submitted_ClaimAdjustmentTypeID = grp.Key.Submitted_ClaimAdjustmentTypeID,
                                                                         Original_PayerClaimControlNumber = grp.Key.Original_PayerClaimControlNumber,
                                                                         //ClaimAdjustmentReason = grp.Key.ClaimAdjustmentReason,
                                                                         ServiceDate = grp.Key.ServiceDate,
                                                                         GroupIDForMileServices = grp.Key.GroupIDForMileServices
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
                            ac.CalculatedUnit,
                            ac.ServiceDateSpan,
                            ac.StrBathNoteID
                        })
                                                                     .Select(grp => new ServiceLineGroupClass
                                                                     {
                                                                         ServiceLineModel = new ServiceLineModel()
                                                                         {
                                                                             ModifierName = grp.Key.ModifierName,
                                                                             ServiceCode = grp.Key.ServiceCode,
                                                                             CalculatedAmount = grp.Key.CalculatedAmount,
                                                                             CalculatedUnit = grp.Key.CalculatedUnit,
                                                                             ServiceDateSpan = grp.Key.ServiceDateSpan,
                                                                             StrBathNoteID = grp.Key.StrBathNoteID
                                                                         }//,
                                                                         //ListModel = grp.ToList()

                                                                     }).ToList();

                        #endregion

                        foreach (ServiceLineGroupClass serviceLine_batchRelatedAllDataModel in tempServiceLineGroupList)
                            claim_batchRelatedAllDataModel.ClaimModel.ServiceLines.Add(serviceLine_batchRelatedAllDataModel.ServiceLineModel);

                        sp_batchRelatedAllDataModel.SubscriberModel.Claims.Add(claim_batchRelatedAllDataModel.ClaimModel);
                    }

                    billingProviderModel.Subscribers.Add(sp_batchRelatedAllDataModel.SubscriberModel);
                }

                groupedModelFor837.BillingProviders.Add(billingProviderModel);
            }

            #endregion
            return groupedModelFor837;
        }
        #endregion

        #endregion

        #endregion

        #region Upload 835 And Processing

        public ServiceResponse SetUpload835Page()
        {
            var response = new ServiceResponse();
            AddUpload835Model addUpload835Model = GetMultipleEntity<AddUpload835Model>(StoredProcedure.GetSetUpload835);
            addUpload835Model.FileProcessStatus = Common.SetUpload835FileProcessStatusFilter();
            addUpload835Model.A835TemplateType = Enum835TemplateType.Edi_File.ToString();
            addUpload835Model.SearchUpload835ListPage.Upload835FileProcessStatus = -1;
            addUpload835Model.SearchUpload835ListPage.A835TemplateType = "-1";
            response.Data = addUpload835Model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse SaveUpload835File(AddUpload835Model model, HttpRequestBase httpRequestBase, long loggedInUserID)
        {
            var response = new ServiceResponse();
            string basePath = String.Format(_cacheHelper.EdiFile835DownLoadPath, _cacheHelper.Domain) + model.PayorID + "/";
            HttpPostedFileBase file = httpRequestBase.Files[0];
            response = Common.SaveFile(file, basePath);

            if (response.IsSuccess)
            {

                Upload835File upload835File = new Upload835File();
                upload835File.PayorID = model.PayorID;
                upload835File.FileName = ((UploadedFileModel)response.Data).FileOriginalName;
                upload835File.FilePath = ((UploadedFileModel)response.Data).TempFilePath;
                upload835File.Comment = model.Comment == "null" ? null : model.Comment;
                upload835File.IsProcessed = false;
                upload835File.Upload835FileProcessStatus = (int)EnumUpload835FileProcessStatus.UnProcessed;
                upload835File.A835TemplateType = model.A835TemplateType;
                upload835File.FileSize = Common.GetFileSizeInBytes(HttpContext.Current.Server.MapCustomPath(upload835File.FilePath)).ToString();


                #region amazonefileupload
                AmazonFileUpload amazoneFileUpload = new AmazonFileUpload();
                string fullpath = HttpContext.Current.Server.MapCustomPath(upload835File.FilePath);
                upload835File.FilePath = amazoneFileUpload.UploadFile(ConfigSettings.ZarephathBucket, upload835File.FilePath.TrimStart('/'), fullpath, true);
                #endregion amazonefileupload

                SaveObject(upload835File, loggedInUserID);
                response.Message = Resource._835FileUploaded;
            }
            return response;
        }

        #region Upload 835 List

        public ServiceResponse GetUpload835FileList(SearchUpload835ListPage searchUpload835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                if (searchUpload835Model != null)
                    SetSearchFilterForUpload835ListPage(searchUpload835Model, searchList);
                Page<ListUpload835Model> listUpload835Model = GetEntityPageList<ListUpload835Model>(StoredProcedure.GetUpload835FileList, searchList, pageSize,
                                                              pageIndex, sortIndex, sortDirection);
                response.Data = listUpload835Model;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.EdiFileLog), Resource.ExceptionMessage);
            }
            return response;
        }

        private static void SetSearchFilterForUpload835ListPage(SearchUpload835ListPage searchUpload835ListPage, List<SearchValueData> searchList)
        {
            if (searchUpload835ListPage.Upload835FileID > 0)
                searchList.Add(new SearchValueData { Name = "Upload835FileID", Value = Convert.ToString(searchUpload835ListPage.Upload835FileID) });

            if (searchUpload835ListPage.PayorID > 0)
                searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchUpload835ListPage.PayorID) });

            if (searchUpload835ListPage.A835TemplateType != null)
                searchList.Add(new SearchValueData { Name = "A835TemplateType", Value = Convert.ToString(searchUpload835ListPage.A835TemplateType) });


            if (searchUpload835ListPage.Comment != null)
                searchList.Add(new SearchValueData { Name = "Comment", Value = Convert.ToString(searchUpload835ListPage.Comment) });

            if (searchUpload835ListPage.FileName != null)
                searchList.Add(new SearchValueData { Name = "FileName", Value = Convert.ToString(searchUpload835ListPage.FileName) });

            searchList.Add(new SearchValueData { Name = "Upload835FileProcessStatus", Value = Convert.ToString(searchUpload835ListPage.Upload835FileProcessStatus) });

        }

        public ServiceResponse DeleteUpload835File(SearchUpload835ListPage searchUpload835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                #region Delete File from folder

                List<SearchValueData> deletesearchList = new List<SearchValueData>();
                if (!string.IsNullOrEmpty(searchUpload835Model.ListOfIdsInCSV))
                    deletesearchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchUpload835Model.ListOfIdsInCSV });
                deletesearchList.Add(new SearchValueData { Name = "UnProcess", Value = Convert.ToString(Convert.ToInt32(EnumUpload835FileProcessStatus.UnProcessed)) });

                List<ListUpload835Model> listEdiFilesLogModel = GetEntityList<ListUpload835Model>(StoredProcedure.GetDeleteUpload835FilePathList, deletesearchList);

                if (listEdiFilesLogModel != null)
                {
                    foreach (var model in listEdiFilesLogModel)
                    {
                        if (model.BatchID == null && !model.IsProcessed)
                        {
                            string filePath = HttpContext.Current.Server.MapCustomPath(model.FilePath);
                            if (File.Exists(filePath))
                                File.Delete(filePath);

                            AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                            amazonFileUpload.DeleteFile(ConfigSettings.ZarephathBucket, model.FilePath);
                        }
                    }
                }

                #endregion

                #region Delete Record from DataBase


                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                SetSearchFilterForUpload835ListPage(searchUpload835Model, searchList);

                if (!string.IsNullOrEmpty(searchUpload835Model.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchUpload835Model.ListOfIdsInCSV });
                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                searchList.Add(new SearchValueData { Name = "UnProcess", Value = Convert.ToString(Convert.ToInt32(EnumUpload835FileProcessStatus.UnProcessed)) });

                List<ListUpload835Model> totalData = GetEntityList<ListUpload835Model>(StoredProcedure.DeleteUpload835File, searchList);
                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListUpload835Model> getEdiFilesLogList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);

                #endregion

                response.Data = getEdiFilesLogList;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.Upload835File);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailedTitle, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse ProcessUpload835File(SearchUpload835ListPage searchUpload835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {

                #region Update IN DB
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                SetSearchFilterForUpload835ListPage(searchUpload835Model, searchList);
                if (!string.IsNullOrEmpty(searchUpload835Model.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchUpload835Model.ListOfIdsInCSV });
                searchList.Add(new SearchValueData { Name = "Upload835FileStatus", Value = ((int)EnumUpload835FileProcessStatus.InProcess).ToString() });
                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                List<ListUpload835Model> totalData = GetEntityList<ListUpload835Model>(StoredProcedure.ProcessUpload835File, searchList);


                #endregion

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListUpload835Model> getEdiFilesLogList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);



                response.Data = getEdiFilesLogList;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordInProcess, Resource.Upload835File);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;// Common.MessageWithTitle(Resource.ProcessFailedTitle, Resource.ExceptionMessage);
            }
            return response;
        }


        #endregion



        public ServiceResponse SaveUpload835Comment(Upload835CommentModel upload835CommentModel, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();

            Upload835File upload835File = GetEntity<Upload835File>(upload835CommentModel.Upload835FileID);
            if (upload835File != null)
            {
                upload835File.Comment = upload835CommentModel.Comment;
                SaveObject(upload835File, loggedInId);

                response.IsSuccess = true;
                response.Message = Resource.CommentSavedSuccessfully;
            }
            else
            {
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }
        #endregion

        #region Cron JOB TO Process Queued 835 Files

        public ServiceResponse BackEndProcessUpload835File()
        {
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.Edi835FileLog, Common.GenerateRandomNumber() + "/");
            if (!ConfigSettings.Service_Edi835FileProcess_ON)
            {
                Common.CreateLogFile("Edi 835 File Process CronJob can't run because it is mark as offline from web config.", ConfigSettings.Edi835FileName, logpath);
                return null;
            }

            ServiceResponse response = new ServiceResponse();
            try
            {
                Common.CreateLogFile("Process Edi835Files CronJob Started.", ConfigSettings.Edi835FileName, logpath);
                #region Process 835 Files
                //List<SearchValueData> searchList = new List<SearchValueData>();
                //searchList.Add(new SearchValueData { Name = "Upload835FileProcessStatus", Value = ((int)EnumUpload835FileProcessStatus.InProcess).ToString() });
                //List<Upload835File> upload835FileList = GetEntityList<Upload835File>(searchList);


                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "GetStatus", Value = ((int)EnumUpload835FileProcessStatus.InProcess).ToString() });
                searchList.Add(new SearchValueData { Name = "SetStatus", Value = ((int)EnumUpload835FileProcessStatus.Running).ToString() });
                List<Upload835File> upload835FileList = GetEntityList<Upload835File>(StoredProcedure.GetUpload835FilesForProcess, searchList);





                foreach (var upload835File in upload835FileList)
                {
                    Edi835 edi835 = new Edi835();
                    string ediFilePath = HttpContext.Current.Server.MapCustomPath("/" + upload835File.FilePath);
                    AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                    amazonFileUpload.DownloadFile(ConfigSettings.ZarephathBucket, upload835File.FilePath, ediFilePath);
                    string ediFile835CsvDownLoadPath = String.Format(_cacheHelper.EdiFile835CsvDownLoadPath, _cacheHelper.Domain);
                    string newRedablefilePath = string.Format("{0}{1}/{2}{3}", ediFile835CsvDownLoadPath, upload835File.PayorID, Guid.NewGuid().ToString(), Constants.FileExtension_Csv);

                    try
                    {

                        Edi835ResponseModel edi835ResponseModel = upload835File.A835TemplateType == Enum835TemplateType.Edi_File.ToString() ?
                            edi835.GenerateEdi835Model(ediFilePath, HttpContext.Current.Server.MapCustomPath(newRedablefilePath), newRedablefilePath) :
                            GenerateEdi835ModelFromPaperRemits(ediFilePath, ediFilePath, upload835File.FilePath);

                        foreach (var edi835Model in edi835ResponseModel.Edi835ModelList)
                        {
                            //long batchId = Convert.ToInt64(edi835Model.BatchID);
                            //long noteId = Convert.ToInt64(edi835Model.NoteID);
                            long batchNoteId = Convert.ToInt64(edi835Model.BatchNoteID);
                            long batchNoteId02 = 0;

                            if (batchNoteId == 0)
                            {
                                //string.Format("{0}", GetClaimDetails(value, "BatchNote"));
                                string value = edi835Model.CLP01_ClaimSubmitterIdentifier.Trim();
                                //int count = value.Trim('N'). Split('N').Length;
                                int count = value.ToCharArray().Count(c => c == 'N');
                                batchNoteId = Common.GetClaimDetails(value, "BatchNote");
                                if (count > 1)
                                    batchNoteId02 = Common.GetClaimDetails(value, "BatchNote", 2);

                                if (batchNoteId02 > 0)
                                {
                                    List<SearchValueData> searchParam = new List<SearchValueData>();
                                    searchParam.Add(new SearchValueData() { Name = "ServiceCode", Value = edi835Model.SVC01_02_ServiceCode });
                                    searchParam.Add(new SearchValueData() { Name = "BatchNoteID01", Value = Convert.ToString(batchNoteId) });
                                    searchParam.Add(new SearchValueData() { Name = "BatchNoteID02", Value = Convert.ToString(batchNoteId02) });
                                    //batchNoteId =  (long)GetScalar(StoredProcedure.GetBatchNoteDetailsBasedOnServiceDetails,searchParam);
                                    batchNoteId = Convert.ToInt64(GetScalar(StoredProcedure.GetBatchNoteDetailsBasedOnServiceDetails, searchParam));
                                }
                            }

                            if (batchNoteId == 0)
                            {
                                //TODO: CHECK WITH PROVIDED INFOMATION
                            }

                            if (batchNoteId > 0)
                            {
                                searchList = new List<SearchValueData>();
                                //searchList.Add(new SearchValueData { Name = "BatchID", Value = batchId.ToString() });
                                //searchList.Add(new SearchValueData { Name = "NoteID", Value = noteId.ToString() });
                                searchList.Add(new SearchValueData { Name = "BatchNoteID", Value = batchNoteId.ToString() });
                                string custWhere = "";//"ParentBatchNoteID IS NULL";
                                BatchNote tempBatchNote = GetEntity<BatchNote>(searchList, custWhere);

                                BatchNote subBatchNote = new BatchNote();
                                if (tempBatchNote != null && (tempBatchNote.ParentBatchNoteID == null || tempBatchNote.ParentBatchNoteID == 0) && string.IsNullOrEmpty(tempBatchNote.CLP02_ClaimStatusCode))
                                    subBatchNote = tempBatchNote;

                                if (tempBatchNote != null)
                                {
                                    subBatchNote.BatchID = tempBatchNote.BatchID;
                                    subBatchNote.NoteID = tempBatchNote.NoteID;
                                    subBatchNote.CLM_BilledAmount = tempBatchNote.CLM_BilledAmount;//   edi835Model.SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount;
                                    subBatchNote.CLM_UNIT = tempBatchNote.CLM_UNIT;
                                    subBatchNote.Original_Amount = tempBatchNote.Original_Amount;
                                    subBatchNote.Original_Unit = tempBatchNote.Original_Unit;
                                    subBatchNote.IsUseInBilling = tempBatchNote.IsUseInBilling;
                                    subBatchNote.IsNewProcess = tempBatchNote.IsNewProcess;
                                    subBatchNote.Submitted_ClaimAdjustmentTypeID = tempBatchNote.Submitted_ClaimAdjustmentTypeID;
                                    subBatchNote.Original_ClaimSubmitterIdentifier = tempBatchNote.Original_ClaimSubmitterIdentifier;
                                    subBatchNote.Original_PayerClaimControlNumber = tempBatchNote.Original_PayerClaimControlNumber;


                                    //subBatchNote.BatchNoteStatusID = null;
                                    subBatchNote.ParentBatchNoteID = batchNoteId;

                                    subBatchNote.N102_PayorName = edi835Model.N102_PayorName;
                                    subBatchNote.PER02_PayorBusinessContactName = edi835Model.PER02_PayorBusinessContactName;
                                    subBatchNote.PER04_PayorBusinessContact = edi835Model.PER04_PayorBusinessContact;
                                    subBatchNote.PER02_PayorTechnicalContactName = edi835Model.PER02_PayorTechnicalContactName;
                                    subBatchNote.PER04_PayorTechnicalContact = edi835Model.PER04_PayorTechnicalContact;
                                    subBatchNote.PER06_PayorTechnicalEmail = edi835Model.PER06_PayorTechnicalEmail;
                                    subBatchNote.PER04_PayorTechnicalContactUrl = edi835Model.PER04_PayorTechnicalContactUrl;
                                    subBatchNote.N102_PayeeName = edi835Model.N102_PayeeName;
                                    subBatchNote.N103_PayeeIdentificationQualifier = edi835Model.N103_PayeeIdentificationQualifier;
                                    subBatchNote.N104_PayeeIdentification = edi835Model.N104_PayeeIdentification;

                                    subBatchNote.LX01_ClaimSequenceNumber = edi835Model.LX01_ClaimSequenceNumber;
                                    subBatchNote.CLP02_ClaimStatusCode = edi835Model.CLP02_ClaimStatusCode;
                                    subBatchNote.CLP01_ClaimSubmitterIdentifier = edi835Model.CLP01_ClaimSubmitterIdentifier;
                                    subBatchNote.CLP03_TotalClaimChargeAmount = edi835Model.CLP03_TotalClaimChargeAmount;
                                    subBatchNote.CLP04_TotalClaimPaymentAmount = edi835Model.CLP04_TotalClaimPaymentAmount;
                                    subBatchNote.CLP05_PatientResponsibilityAmount = edi835Model.CLP05_PatientResponsibilityAmount;
                                    subBatchNote.CLP07_PayerClaimControlNumber = edi835Model.CLP07_PayerClaimControlNumber;
                                    subBatchNote.CLP08_PlaceOfService = edi835Model.CLP08_PlaceOfService;

                                    subBatchNote.NM103_PatientLastName = edi835Model.NM103_PatientLastName;
                                    subBatchNote.NM104_PatientFirstName = edi835Model.NM104_PatientFirstName;
                                    subBatchNote.NM109_PatientIdentifier = edi835Model.NM109_PatientIdentifier;
                                    subBatchNote.NM103_ServiceProviderName = edi835Model.NM103_ServiceProviderName;
                                    subBatchNote.NM109_ServiceProviderNpi = edi835Model.NM109_ServiceProviderNpi;

                                    subBatchNote.SVC01_01_ServiceCodeQualifier = edi835Model.SVC01_01_ServiceCodeQualifier;
                                    subBatchNote.SVC01_02_ServiceCode = edi835Model.SVC01_02_ServiceCode;
                                    subBatchNote.SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount = edi835Model.SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount;
                                    subBatchNote.SVC03_LineItemProviderPaymentAmoun_PaidAmount = edi835Model.SVC03_LineItemProviderPaymentAmoun_PaidAmount;
                                    subBatchNote.SVC05_ServiceCodeUnit = edi835Model.SVC05_ServiceCodeUnit;
                                    subBatchNote.DTM02_ServiceStartDate = edi835Model.DTM02_ServiceStartDate;
                                    subBatchNote.DTM02_ServiceEndDate = edi835Model.DTM02_ServiceEndDate;
                                    subBatchNote.CAS01_ClaimAdjustmentGroupCode = edi835Model.CAS01_ClaimAdjustmentGroupCode;
                                    subBatchNote.CAS02_ClaimAdjustmentReasonCode = edi835Model.CAS02_ClaimAdjustmentReasonCode;
                                    subBatchNote.CAS03_ClaimAdjustmentAmount = edi835Model.CAS03_ClaimAdjustmentAmount;
                                    subBatchNote.REF02_LineItem_ReferenceIdentification = edi835Model.REF02_LineItem_ReferenceIdentification;
                                    subBatchNote.AMT01_ServiceLineAllowedAmount_AllowedAmount = edi835Model.AMT01_ServiceLineAllowedAmount_AllowedAmount;

                                    subBatchNote.CheckDate = edi835Model.CheckDate;
                                    subBatchNote.CheckAmount = edi835Model.CheckAmount;
                                    subBatchNote.CheckNumber = edi835Model.CheckNumber;
                                    subBatchNote.PolicyNumber = edi835Model.PolicyNumber;
                                    subBatchNote.AccountNumber = edi835Model.AccountNumber;
                                    subBatchNote.ICN = edi835Model.ICN;
                                    //subBatchNote.BilledAmount = edi835Model.BilledAmount;
                                    //subBatchNote.AllowedAmount = edi835Model.AllowedAmount;
                                    //subBatchNote.PaidAmount = edi835Model.PaidAmount;
                                    subBatchNote.Deductible = edi835Model.Deductible;
                                    subBatchNote.Coins = edi835Model.Coins;
                                    subBatchNote.ProcessedDate = string.IsNullOrEmpty(edi835Model.ProcessedDate) ? DateTime.Now.Date : Convert.ToDateTime(edi835Model.ProcessedDate);
                                    subBatchNote.ReceivedDate = upload835File.CreatedDate;
                                    subBatchNote.LoadDate = DateTime.Now;

                                    subBatchNote.Upload835FileID = upload835File.Upload835FileID;
                                    subBatchNote.S277 = tempBatchNote.S277;
                                    subBatchNote.S277CA = tempBatchNote.S277CA;
                                    SaveEntity(subBatchNote);

                                    MarkNoteAsLatest(subBatchNote.BatchNoteID, subBatchNote.BatchID, subBatchNote.NoteID, upload835File.Upload835FileID);
                                }
                            }

                        }

                        Common.DeleteFile(ediFilePath);
                        if (upload835File.A835TemplateType == Enum835TemplateType.Edi_File.ToString())
                            upload835File.ReadableFilePath = amazonFileUpload.UploadFile(ConfigSettings.ZarephathBucket,
                                edi835ResponseModel.GeneratedFileRelativePath.TrimStart('/'),
                                edi835ResponseModel.GeneratedFileAbsolutePath, true);
                        else
                            upload835File.ReadableFilePath = upload835File.FilePath;

                        upload835File.Upload835FileProcessStatus = (int)EnumUpload835FileProcessStatus.Processed;
                        SaveEntity(upload835File);

                        #region SCRAP CODE
                        //if (upload835File.A835TemplateType == Enum835TemplateType.Edi_File.ToString())
                        //{
                        //    #region Reading & Processing 835 EDI File

                        //    Edi835ResponseModel edi835ResponseModel = edi835.GenerateEdi835Model(ediFilePath, HttpContext.Current.Server.MapCustomPath(newRedablefilePath), newRedablefilePath);
                        //    foreach (var edi835Model in edi835ResponseModel.Edi835ModelList)
                        //    {
                        //        //edi835Model.CLP01_ClaimSubmitterIdentifier = "204ZRPB260BN777";

                        //        if (edi835Model.CLP01_ClaimSubmitterIdentifier.Contains("ZRPB") && edi835Model.CLP01_ClaimSubmitterIdentifier.Contains("BN"))
                        //        {
                        //            string[] strTemp = edi835Model.CLP01_ClaimSubmitterIdentifier.Split(new string[] { "ZRPB" }, StringSplitOptions.None);
                        //            long noteId = Convert.ToInt64(strTemp[0]);
                        //            long batchId = Convert.ToInt64(strTemp[1].Split(new string[] { "BN" }, StringSplitOptions.None)[0]);
                        //            long batchNoteId = Convert.ToInt64(strTemp[1].Split(new string[] { "BN" }, StringSplitOptions.None)[1]);

                        //            searchList = new List<SearchValueData>();
                        //            searchList.Add(new SearchValueData { Name = "BatchID", Value = batchId.ToString() });
                        //            searchList.Add(new SearchValueData { Name = "NoteID", Value = noteId.ToString() });
                        //            searchList.Add(new SearchValueData { Name = "BatchNoteID", Value = batchNoteId.ToString() });
                        //            string custWhere = "";//"ParentBatchNoteID IS NULL";
                        //            BatchNote tempBatchNote = GetEntity<BatchNote>(searchList, custWhere);

                        //            BatchNote subBatchNote = new BatchNote();
                        //            if (tempBatchNote != null && (tempBatchNote.ParentBatchNoteID == null || tempBatchNote.ParentBatchNoteID == 0) && string.IsNullOrEmpty(tempBatchNote.CLP02_ClaimStatusCode))
                        //                subBatchNote = tempBatchNote;


                        //            subBatchNote.BatchID = batchId;
                        //            subBatchNote.NoteID = noteId;
                        //            subBatchNote.CLM_BilledAmount = edi835Model.SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount; ;
                        //            //subBatchNote.BatchNoteStatusID = null;
                        //            subBatchNote.ParentBatchNoteID = batchNoteId;

                        //            subBatchNote.N102_PayorName = edi835Model.N102_PayorName;
                        //            subBatchNote.PER02_PayorBusinessContactName = edi835Model.PER02_PayorBusinessContactName;
                        //            subBatchNote.PER04_PayorBusinessContact = edi835Model.PER04_PayorBusinessContact;
                        //            subBatchNote.PER02_PayorTechnicalContactName = edi835Model.PER02_PayorTechnicalContactName;
                        //            subBatchNote.PER04_PayorTechnicalContact = edi835Model.PER04_PayorTechnicalContact;
                        //            subBatchNote.PER06_PayorTechnicalEmail = edi835Model.PER06_PayorTechnicalEmail;
                        //            subBatchNote.PER04_PayorTechnicalContactUrl = edi835Model.PER04_PayorTechnicalContactUrl;
                        //            subBatchNote.N102_PayeeName = edi835Model.N102_PayeeName;
                        //            subBatchNote.N103_PayeeIdentificationQualifier = edi835Model.N103_PayeeIdentificationQualifier;
                        //            subBatchNote.N104_PayeeIdentification = edi835Model.N104_PayeeIdentification;

                        //            subBatchNote.LX01_ClaimSequenceNumber = edi835Model.LX01_ClaimSequenceNumber;
                        //            subBatchNote.CLP02_ClaimStatusCode = edi835Model.CLP02_ClaimStatusCode;
                        //            subBatchNote.CLP01_ClaimSubmitterIdentifier = edi835Model.CLP01_ClaimSubmitterIdentifier;
                        //            subBatchNote.CLP03_TotalClaimChargeAmount = edi835Model.CLP03_TotalClaimChargeAmount;
                        //            subBatchNote.CLP04_TotalClaimPaymentAmount = edi835Model.CLP04_TotalClaimPaymetAmount;
                        //            subBatchNote.CLP05_PatientResponsibilityAmount = edi835Model.CLP05_PatientResponsibilityAmount;
                        //            subBatchNote.CLP07_PayerClaimControlNumber = edi835Model.CLP07_PayerClaimControlNumber;
                        //            subBatchNote.CLP08_PlaceOfService = edi835Model.CLP08_PlaceOfService;

                        //            subBatchNote.NM103_PatientLastName = edi835Model.NM103_PatientLastName;
                        //            subBatchNote.NM104_PatientFirstName = edi835Model.NM104_PatientFirstName;
                        //            subBatchNote.NM109_PatientIdentifier = edi835Model.NM109_PatientIdentifier;
                        //            subBatchNote.NM103_ServiceProviderName = edi835Model.NM103_ServiceProviderName;
                        //            subBatchNote.NM109_ServiceProviderNpi = edi835Model.NM109_ServiceProviderNpi;

                        //            subBatchNote.SVC01_01_ServiceCodeQualifier = edi835Model.SVC01_01_ServiceCodeQualifier;
                        //            subBatchNote.SVC01_02_ServiceCode = edi835Model.SVC01_02_ServiceCode;
                        //            subBatchNote.SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount = edi835Model.SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount;
                        //            subBatchNote.SVC03_LineItemProviderPaymentAmoun_PaidAmount = edi835Model.SVC03_LineItemProviderPaymentAmoun_PaidAmount;
                        //            subBatchNote.SVC05_ServiceCodeUnit = edi835Model.SVC05_ServiceCodeUnit;
                        //            subBatchNote.DTM02_ServiceStartDate = edi835Model.DTM02_ServiceStartDate;
                        //            subBatchNote.DTM02_ServiceEndDate = edi835Model.DTM02_ServiceEndDate;
                        //            subBatchNote.CAS01_ClaimAdjustmentGroupCode = edi835Model.CAS01_ClaimAdjustmentGroupCode;
                        //            subBatchNote.CAS02_ClaimAdjustmentReasonCode = edi835Model.CAS02_ClaimAdjustmentReasonCode;
                        //            subBatchNote.CAS03_ClaimAdjustmentAmount = edi835Model.CAS03_ClaimAdjustmentAmount;
                        //            subBatchNote.REF02_LineItem_ReferenceIdentification = edi835Model.REF02_LineItem_ReferenceIdentification;
                        //            subBatchNote.AMT01_ServiceLineAllowedAmount_AllowedAmount = edi835Model.AMT01_ServiceLineAllowedAmount_AllowedAmount;

                        //            subBatchNote.CheckDate = edi835Model.CheckDate;
                        //            subBatchNote.CheckAmount = edi835Model.CheckAmount;
                        //            subBatchNote.CheckNumber = edi835Model.CheckNumber;
                        //            subBatchNote.PolicyNumber = edi835Model.PolicyNumber;
                        //            subBatchNote.AccountNumber = edi835Model.AccountNumber;
                        //            subBatchNote.ICN = edi835Model.ICN;
                        //            //subBatchNote.BilledAmount = edi835Model.BilledAmount;
                        //            //subBatchNote.AllowedAmount = edi835Model.AllowedAmount;
                        //            //subBatchNote.PaidAmount = edi835Model.PaidAmount;
                        //            subBatchNote.Deductible = edi835Model.Deductible;
                        //            subBatchNote.Coins = edi835Model.Coins;
                        //            subBatchNote.ProcessedDate = Convert.ToDateTime(edi835Model.ProcessedDate);
                        //            subBatchNote.ReceivedDate = upload835File.CreatedDate;
                        //            subBatchNote.LoadDate = DateTime.Now;


                        //            subBatchNote.Upload835FileID = upload835File.Upload835FileID;
                        //            SaveEntity(subBatchNote);


                        //            MarkNoteAsLatest(batchNoteId, batchId, noteId, upload835File.Upload835FileID);
                        //        }

                        //    }
                        //    Common.DeleteFile(ediFilePath);
                        //    upload835File.ReadableFilePath = amazonFileUpload.UploadFile(ConfigSettings.ZarephathBucket,
                        //                                    edi835ResponseModel.GeneratedFileRelativePath.TrimStart('/'), edi835ResponseModel.GeneratedFileAbsolutePath, true);

                        //    upload835File.Upload835FileProcessStatus = (int)EnumUpload835FileProcessStatus.Processed;
                        //    SaveEntity(upload835File);
                        //    #endregion
                        //}
                        //else  //Enum835TemplateType.Paper_Remits_EOB.ToString()
                        //{
                        //    #region Reading & Processing Paper Remits / EOB File

                        //    Edi835ResponseModel edi835ResponseModel = GenerateEdi835ModelFromPaperRemits(ediFilePath, HttpContext.Current.Server.MapCustomPath(newRedablefilePath), newRedablefilePath);
                        //    foreach (var edi835Model in edi835ResponseModel.Edi835ModelList)
                        //    {

                        //    }


                        //    #endregion
                        //}
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        string fileVirtualPath = Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.Edi835FileName, logpath);
                        string fullpath = HttpContext.Current.Server.MapCustomPath(fileVirtualPath);
                        upload835File.ReadableFilePath = amazonFileUpload.UploadFile(ConfigSettings.ZarephathBucket, fileVirtualPath.TrimStart('/'), fullpath, true);
                        upload835File.Upload835FileProcessStatus = (int)EnumUpload835FileProcessStatus.ErrorInProcess;
                        SaveEntity(upload835File);
                    }//GetScalar(StoredProcedure.UpdateBatchAfter835FileProcessed);
                }

                #endregion
                Common.CreateLogFile("Process Edi835Files CronJob Completed Succesfully.", ConfigSettings.Edi835FileName, logpath);

            }
            catch (Exception ex)
            {
                Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.Edi835FileName, logpath);
                response.IsSuccess = false;
            }
            return response;
        }


        private Edi835ResponseModel GenerateEdi835ModelFromPaperRemits(string absolutePathForEdi835File, string absolutePathForGenerateReadableFile, string relativePathForGenerateReadableFile)
        {
            Edi835ResponseModel edi835ResponseModel = new Edi835ResponseModel()
            {
                GeneratedFileAbsolutePath = absolutePathForGenerateReadableFile,
                GeneratedFileRelativePath = relativePathForGenerateReadableFile
            };

            //var dt = new DataTable();
            var dt = Common.CSVtoDataTable(absolutePathForEdi835File);
            //using (var reader = new ExcelDataReader(absolutePathForEdi835File))
            //    dt.Load(reader);

            foreach (DataRow row in dt.Rows)
            {
                Edi835Model model = new Edi835Model();
                model.BatchID = row[Resource.PR_BatchID].ToString();
                model.NoteID = row[Resource.PR_NoteID].ToString();
                model.BatchNoteID = row[Resource.PR_BatchNoteID].ToString();
                model.NM103_PatientLastName = row[Resource.PR_LastName].ToString();
                model.NM104_PatientFirstName = row[Resource.PR_FirstName].ToString();
                model.CheckDate = row[Resource.PR_CheckDate].ToString();// == DBNull.Value ? "" : DateTime.FromOADate(Convert.ToDouble(row[Resource.PR_CheckDate])).ToString("MM/dd/yyyy");
                model.CheckNumber = row[Resource.PR_CheckNumber].ToString();
                model.CheckAmount = row[Resource.PR_CheckAmount].ToString();
                //model.FreeField7 = Common.Getarrayvalue<string>(7, values);
                model.ProcessedDate = row[Resource.PR_ProcessedDate].ToString();// == DBNull.Value ? "" : DateTime.FromOADate(Convert.ToDouble(row[Resource.PR_ProcessedDate])).ToString("MM/dd/yyyy");
                model.NM109_PatientIdentifier = row[Resource.PR_ClientIdNumber].ToString();
                model.PolicyNumber = row[Resource.PR_PolicyNumber].ToString();
                model.ICN = row[Resource.PR_ClaimNumber].ToString();
                model.CLP07_PayerClaimControlNumber = model.ICN;
                model.NM103_ServiceProviderName = row[Resource.PR_BillingProviderName].ToString();
                model.NM109_ServiceProviderNpi = row[Resource.PR_BillingProviderNPI].ToString();
                model.DTM02_ServiceStartDate = row[Resource.PR_ServiceDate].ToString();// == DBNull.Value ? "" : DateTime.FromOADate(Convert.ToDouble(row[Resource.PR_ServiceDate])).ToString("MM/dd/yyyy"); 
                model.DTM02_ServiceEndDate = model.DTM02_ServiceStartDate;
                model.CLP08_PlaceOfService = row[Resource.PR_PosID].ToString();
                model.SVC05_ServiceCodeUnit = row[Resource.PR_BilledUnit].ToString();
                model.SVC01_02_ServiceCode = row[Resource.PR_ServiceCode].ToString();
                model.SVC01_01_ServiceCodeQualifier = row[Resource.PR_ModifierCode].ToString();
                model.SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount = row[Resource.PR_BilledAmount].ToString();
                //model.FreeField19 = Common.Getarrayvalue<string>(19, values);
                model.AMT01_ServiceLineAllowedAmount_AllowedAmount = row[Resource.PR_AllowedAmount].ToString();
                model.Deductible = row[Resource.PR_Deductible].ToString();
                model.Coins = row[Resource.PR_Coinsurance].ToString();
                model.SVC03_LineItemProviderPaymentAmoun_PaidAmount = row[Resource.PR_PaidAmount].ToString();
                //model.FreeField24 = Common.Getarrayvalue<string>(24, values);
                //model.FreeField25 = Common.Getarrayvalue<string>(25, values);
                //model.FreeField26 = Common.Getarrayvalue<string>(26, values);
                model.LX01_ClaimSequenceNumber = row[Resource.PR_LX01_ClaimSequenceNumber].ToString();
                model.CLP02_ClaimStatusCode = row[Resource.PR_CLP02_ClaimStatusCode].ToString();
                model.CLP01_ClaimSubmitterIdentifier = row[Resource.PR_CLP01_ClaimSubmitterIdentifier].ToString();
                model.CLP03_TotalClaimChargeAmount = row[Resource.PR_CLP03_TotalClaimChargeAmount].ToString();
                model.CLP04_TotalClaimPaymentAmount = row[Resource.PR_CLP04_TotalClaimPaymentAmount].ToString();
                model.CLP05_PatientResponsibilityAmount = row[Resource.PR_CLP05_PatientResponsibilityAmount].ToString();
                model.CLP07_PayerClaimControlNumber = row[Resource.PR_CLP07_PayerClaimControlNumber].ToString();
                model.CAS01_ClaimAdjustmentGroupCode = row[Resource.PR_CAS01_ClaimAdjustmentGroupCode].ToString();
                model.CAS02_ClaimAdjustmentReasonCode = row[Resource.PR_CAS02_ClaimAdjustmentReasonCode].ToString();
                model.CAS03_ClaimAdjustmentAmount = row[Resource.PR_CAS03_ClaimAdjustmentAmount].ToString();




                model.N102_PayeeName = row[Resource.PR_Payee].ToString();
                model.N103_PayeeIdentificationQualifier = row[Resource.PR_PayeeIDQualifier].ToString();
                model.N104_PayeeIdentification = row[Resource.PR_PayeeID].ToString();

                model.N102_PayorName = row[Resource.PR_Payor].ToString();
                model.PER02_PayorBusinessContactName = row[Resource.PR_PayorBusinessContactName].ToString();
                model.PER04_PayorBusinessContact = row[Resource.PR_PayorBusinessContact].ToString();
                model.PER02_PayorTechnicalContactName = row[Resource.PR_PayorTechnicalContactName].ToString();
                model.PER04_PayorTechnicalContact = row[Resource.PR_PayorTechnicalContact].ToString();
                model.PER06_PayorTechnicalEmail = row[Resource.PR_PayorTechnicalEmail].ToString();
                model.PER04_PayorTechnicalContactUrl = row[Resource.PR_PayorTechnicalContactUrl].ToString();


                edi835ResponseModel.Edi835ModelList.Add(model);


            }





            return edi835ResponseModel;

        }




        #endregion

        #region Reconcile 835 / EOB

        public ServiceResponse SetReconcile835Page()
        {
            var response = new ServiceResponse();
            AddReconcile835Model addReconcile835Model = GetMultipleEntity<AddReconcile835Model>(StoredProcedure.GetSetReconcile835);
            addReconcile835Model.SearchReconcile835ListPage.Str835ProcessedOnlyID = 0;
            addReconcile835Model.SearchReconcile835ListPage.ServiceID = "-1";
            addReconcile835Model.Services = Common.SetNoteServices();
            addReconcile835Model.SearchReconcile835ListPage.ClaimAdjustmentTypeID = "-1";
            addReconcile835Model.ClaimAdjustmentTypes = Common.ClaimAdjustmentTypes();

            addReconcile835Model.ClaimStatusCodeList.Add(new ClaimStatusCode() { ClaimStatusCodeID = -2, ClaimStatusName = Resource.NA });
            response.Data = addReconcile835Model;
            response.IsSuccess = true;
            return response;
        }

        public List<Upload835File> GetUpload835Files(long payorId, int pageSize, string searchText = null)
        {
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "PayorId", Value = payorId.ToString()},
                    new SearchValueData {Name = "SearchText", Value = searchText},
                    new SearchValueData {Name = "PageSize", Value = pageSize.ToString()}
                };

            return GetEntityList<Upload835File>(StoredProcedure.GetUpload835FilesForAutoComplete, searchParam);
        }

        public ServiceResponse GetReconcile835List(SearchReconcile835ListPage searchReconcile835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                if (searchReconcile835Model != null)
                    SetSearchFilterForReconcile835ListPage(searchReconcile835Model, searchList);
                Page<ListReconcile835Model> listReconcile835Model = GetEntityPageList<ListReconcile835Model>(StoredProcedure.GetReconcile835List, searchList, pageSize,
                    pageIndex, sortIndex, sortDirection);
                response.Data = listReconcile835Model;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.Reconcile835), Resource.ExceptionMessage);
            }
            return response;
        }


        private static void SetSearchFilterForReconcile835ListPage(SearchReconcile835ListPage searchReconcile835Model, List<SearchValueData> searchList)
        {
            if (searchReconcile835Model.PayorID > 0)
                searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchReconcile835Model.PayorID) });

            if (searchReconcile835Model.Batch != null)
                searchList.Add(new SearchValueData { Name = "Batch", Value = Convert.ToString(searchReconcile835Model.Batch) });

            if (searchReconcile835Model.ClaimNumber != null)
                searchList.Add(new SearchValueData { Name = "ClaimNumber", Value = Convert.ToString(searchReconcile835Model.ClaimNumber) });

            if (searchReconcile835Model.Client != null)
                searchList.Add(new SearchValueData { Name = "Client", Value = Convert.ToString(searchReconcile835Model.Client) });

            if (searchReconcile835Model.StrServiceCodeID != null)
                searchList.Add(new SearchValueData { Name = "ServiceCode", Value = Convert.ToString(searchReconcile835Model.StrServiceCodeID) });

            if (searchReconcile835Model.ServiceStartDate != null)
                searchList.Add(new SearchValueData { Name = "ServiceStartDate", Value = Convert.ToDateTime(searchReconcile835Model.ServiceStartDate).ToString(Constants.DbDateFormat) });

            if (searchReconcile835Model.ServiceEndDate != null)
                searchList.Add(new SearchValueData { Name = "ServiceEndDate", Value = Convert.ToDateTime(searchReconcile835Model.ServiceEndDate).ToString(Constants.DbDateFormat) });

            if (searchReconcile835Model.ModifierID > 0)
                searchList.Add(new SearchValueData { Name = "ModifierID", Value = Convert.ToString(searchReconcile835Model.ModifierID) });

            if (searchReconcile835Model.PosID > 0)
                searchList.Add(new SearchValueData { Name = "PosID", Value = Convert.ToString(searchReconcile835Model.PosID) });

            searchList.Add(new SearchValueData { Name = "ClaimStatusCodeID", Value = Convert.ToString(searchReconcile835Model.ClaimStatusCodeID) });

            if (!String.IsNullOrEmpty(searchReconcile835Model.Status))
                searchList.Add(new SearchValueData { Name = "ReconcileStatus", Value = Convert.ToString(searchReconcile835Model.Status) });

            if (searchReconcile835Model.ClaimAdjustmentGroupCodeID != null)
                searchList.Add(new SearchValueData { Name = "ClaimAdjustmentGroupCodeID", Value = Convert.ToString(searchReconcile835Model.ClaimAdjustmentGroupCodeID) });

            if (searchReconcile835Model.ClaimAdjustmentReasonCodeID != null)
                searchList.Add(new SearchValueData { Name = "ClaimAdjustmentReasonCodeID", Value = Convert.ToString(searchReconcile835Model.ClaimAdjustmentReasonCodeID) });

            searchList.Add(new SearchValueData { Name = "Get835ProcessedOnly", Value = Convert.ToString(searchReconcile835Model.Str835ProcessedOnlyID) });

            searchList.Add(new SearchValueData { Name = "Upload835FileID", Value = Convert.ToString(searchReconcile835Model.Upload835FileID) });


            searchList.Add(new SearchValueData { Name = "ServiceID", Value = Convert.ToString(searchReconcile835Model.ServiceID) });
            searchList.Add(new SearchValueData { Name = "ClaimAdjustmentTypeID", Value = Convert.ToString(searchReconcile835Model.ClaimAdjustmentTypeID) });


            searchList.Add(new SearchValueData { Name = "NoteID", Value = Convert.ToString(searchReconcile835Model.NoteID) });
            searchList.Add(new SearchValueData { Name = "PayorClaimNumber", Value = Convert.ToString(searchReconcile835Model.PayorClaimNumber) });


        }

        public ServiceResponse GetReconcileBatchNoteDetails(long BatchNoteID, long BatchID, long NoteID, long? Upload835FileID)
        {
            var response = new ServiceResponse();
            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ParentBatchNoteID", Value = Convert.ToString(BatchNoteID)},
                    new SearchValueData {Name = "BatchID", Value = Convert.ToString(BatchID)},
                    new SearchValueData {Name = "NoteID", Value = Convert.ToString(NoteID)},
                    new SearchValueData {Name = "Upload835FileID", Value = Convert.ToString(Upload835FileID)},
                   };
            ReconcileBatchNoteDetailsModel reconcileBatchNoteDetailsModel = GetMultipleEntity<ReconcileBatchNoteDetailsModel>(StoredProcedure.GetReconcileBatchNoteDetails, searchlist);
            response.Data = reconcileBatchNoteDetailsModel;
            response.IsSuccess = true;

            return response;
        }

        public ServiceResponse MarkNoteAsLatest(long BatchNoteID, long BatchID, long NoteID, long? Upload835FileID)
        {
            var response = new ServiceResponse();
            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "BatchNoteID", Value = Convert.ToString(BatchNoteID)},
                    new SearchValueData {Name = "BatchID", Value = Convert.ToString(BatchID)},
                    new SearchValueData {Name = "NoteID", Value = Convert.ToString(NoteID)},
                   };
            GetScalar(StoredProcedure.MarkNoteAsLatest, searchlist);
            response.IsSuccess = true;
            response.Message = Resource.MasrkAsLatestSuccessfully;
            return response;
        }


        public ServiceResponse HC_UpdateBatchWithERAReference(List<long> listOfBatchID, string EraID)
        {

            var response = new ServiceResponse();
            if (listOfBatchID.Count > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "BatchIDs", Value = String.Join(",", listOfBatchID.Select(x => x.ToString()).ToArray())},
                    new SearchValueData {Name = "EraID", Value = Convert.ToString(EraID)},
                   };
                GetScalar(StoredProcedure.HC_UpdateBatchWithERAReference, searchlist);
            }
            response.IsSuccess = true;
            //response.Message = Resource.MasrkAsLatestSuccessfully;
            return response;
        }


        public ServiceResponse ExportReconcile835List(SearchReconcile835ListPage searchReconcile835Model, string sortIndex = "", string sortDirection = "")
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            if (searchReconcile835Model != null)
                SetSearchFilterForReconcile835ListPage(searchReconcile835Model, searchList);
            searchList.AddRange(Common.SetPagerValues(0, 0, sortIndex, sortDirection));

            List<ExportReconcile835ListModel> totalData = GetEntityList<ExportReconcile835ListModel>(StoredProcedure.ExportReconcile835List, searchList);

            if (totalData.Count > 0)
            {
                string fileName = string.Format("{0}_{1}", Constants.ReportName_ClaimsOutcomeStatus, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);

                string basePath = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_xlsx);
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                CreateExcelFile.CreateExcelDocument(totalData, downloadFileModel.AbsolutePath);
                response.IsSuccess = true;
                response.Data = downloadFileModel;
                response.IsSuccess = true;
            }
            return response;
        }

        public ServiceResponse SetClaimAdjustmentFlag(long batchId, long noteId, string claimAdjustmentType, string claimAdjustmentReason)
        {
            var response = new ServiceResponse();

            string msg = string.Format(Resource.AdjTypeStatusChangeSuccessMessage, claimAdjustmentType);
            if (claimAdjustmentType == ClaimAdjustmentType.ClaimAdjustmentType_Remove)
            {
                claimAdjustmentType = null;
                claimAdjustmentReason = null;
                msg = Resource.AdjTypeStatusUnmarkedSuccessMessage;

            }

            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "BatchID", Value = Convert.ToString(batchId)},
                    new SearchValueData {Name = "NoteID", Value = Convert.ToString(noteId)},
                    new SearchValueData {Name = "ClaimAdjustmentType", Value = Convert.ToString(claimAdjustmentType)},
                    new SearchValueData {Name = "ClaimAdjustmentReason", Value = Convert.ToString(claimAdjustmentReason)},
                   };

            GetScalar(StoredProcedure.SetClaimAdjustmentFlag01, searchlist);
            response.IsSuccess = true;
            response.Message = msg;
            return response;
        }

        public ServiceResponse BulkSetClaimAdjustmentFlag(string itemId, string claimAdjustmentType, string claimAdjustmentReason)
        {
            var response = new ServiceResponse();

            string msg = string.Format(Resource.AdjTypeStatusChangeSuccessMessage, claimAdjustmentType);
            if (claimAdjustmentType == ClaimAdjustmentType.ClaimAdjustmentType_Remove)
            {
                claimAdjustmentType = null;
                claimAdjustmentReason = null;
                msg = Resource.AdjTypeStatusUnmarkedSuccessMessage;

            }
            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ItemID", Value = Convert.ToString(itemId)},
                    new SearchValueData {Name = "ClaimAdjustmentType", Value = Convert.ToString(claimAdjustmentType)},
                    new SearchValueData {Name = "ClaimAdjustmentReason", Value = Convert.ToString(claimAdjustmentReason)},
                   };

            GetScalar(StoredProcedure.BulkSetClaimAdjustmentFlag, searchlist);
            response.IsSuccess = true;
            response.Message = msg;
            return response;
        }



        #region New Changes

        public ServiceResponse GetParentReconcileList(SearchReconcile835ListPage searchReconcile835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                if (searchReconcile835Model != null)
                    SetSearchFilterForReconcile835ListPage(searchReconcile835Model, searchList);
                Page<ListReconcile835Model> listReconcile835Model = GetEntityPageList<ListReconcile835Model>(StoredProcedure.GetParentReconcileList, searchList, pageSize,
                    pageIndex, sortIndex, sortDirection);
                response.Data = listReconcile835Model;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.Reconcile835), Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse GetChildReconcileList(long NoteID)
        {
            var response = new ServiceResponse();
            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "NoteID", Value = Convert.ToString(NoteID)},
                };
            ChildReconcileListModel reconcileBatchNoteDetailsModel = GetMultipleEntity<ChildReconcileListModel>(StoredProcedure.GetChildReconcileList, searchlist);
            response.Data = reconcileBatchNoteDetailsModel;
            response.IsSuccess = true;

            return response;
        }

        public ServiceResponse MarkNoteAsLatest01(long BatchNoteID, long BatchID, long NoteID, long? Upload835FileID)
        {
            var response = new ServiceResponse();
            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "BatchNoteID", Value = Convert.ToString(BatchNoteID)},
                    new SearchValueData {Name = "BatchID", Value = Convert.ToString(BatchID)},
                    new SearchValueData {Name = "NoteID", Value = Convert.ToString(NoteID)},
                   };
            GetScalar(StoredProcedure.MarkNoteAsLatest01, searchlist);
            response.IsSuccess = true;
            response.Message = Resource.MasrkAsLatestSuccessfully;
            return response;
        }

        public ServiceResponse ExportReconcileList(SearchReconcile835ListPage searchReconcile835Model, string sortIndex = "", string sortDirection = "")
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            if (searchReconcile835Model != null)
                SetSearchFilterForReconcile835ListPage(searchReconcile835Model, searchList);
            searchList.AddRange(Common.SetPagerValues(0, 0, sortIndex, sortDirection));

            List<ExportReconcile835ListModel> totalData = GetEntityList<ExportReconcile835ListModel>(StoredProcedure.ExportReconcileList, searchList);

            if (totalData.Count > 0)
            {
                string fileName = string.Format("{0}_{1}", Constants.ReportName_ClaimsOutcomeStatus, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);

                string basePath = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_xlsx);
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                CreateExcelFile.CreateExcelDocument(totalData, downloadFileModel.AbsolutePath);
                response.IsSuccess = true;
                response.Data = downloadFileModel;
                response.IsSuccess = true;
            }
            return response;
        }
        #endregion




        #endregion

        #region EdiFileLog List

        public ServiceResponse SetEdiFileLogListPage()
        {
            var response = new ServiceResponse();
            SetEdiFileLogModelListPage setEdiFileLogModelListPage = GetMultipleEntity<SetEdiFileLogModelListPage>(StoredProcedure.SetEdiFilesLogListPage);
            response.Data = setEdiFileLogModelListPage;
            return response;
        }

        public ServiceResponse GetEdiFileLogList(SearchEdiFileLogListPage searchEdiFileLogListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                if (searchEdiFileLogListPage != null)
                    SetSearchFilterForEdiFilesLogListPage(searchEdiFileLogListPage, searchList);
                Page<ListEdiFileLogModel> listEdiFilesLogModel = GetEntityPageList<ListEdiFileLogModel>(StoredProcedure.GetEdiFileLogList, searchList, pageSize,
                                                              pageIndex, sortIndex, sortDirection);
                response.Data = listEdiFilesLogModel;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.EdiFileLog), Resource.ExceptionMessage);
            }
            return response;
        }

        private static void SetSearchFilterForEdiFilesLogListPage(SearchEdiFileLogListPage searchEdiFileLogListPage, List<SearchValueData> searchList)
        {
            if (searchEdiFileLogListPage.EdiFileLogID > 0)
                searchList.Add(new SearchValueData { Name = "EdiFileLogID", Value = Convert.ToString(searchEdiFileLogListPage.EdiFileLogID) });

            if (searchEdiFileLogListPage.EdiFileTypeID > 0)
                searchList.Add(new SearchValueData { Name = "EdiFileTypeID", Value = Convert.ToString(searchEdiFileLogListPage.EdiFileTypeID) });

            if (searchEdiFileLogListPage.FileName != null)
                searchList.Add(new SearchValueData { Name = "FileName", Value = Convert.ToString(searchEdiFileLogListPage.FileName) });

            if (searchEdiFileLogListPage.EdiFileLogID > 0)
                searchList.Add(new SearchValueData { Name = "FilePath", Value = Convert.ToString(searchEdiFileLogListPage.FilePath) });
        }

        public ServiceResponse DeleteEdiFileLog(SearchEdiFileLogListPage searchEdiFileLogListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                #region Delete File from folder

                List<SearchValueData> deletesearchList = new List<SearchValueData>();
                if (!string.IsNullOrEmpty(searchEdiFileLogListPage.ListOfIdsInCSV))
                    deletesearchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchEdiFileLogListPage.ListOfIdsInCSV });

                List<ListEdiFileLogModel> listEdiFilesLogModel = GetEntityList<ListEdiFileLogModel>(StoredProcedure.GetEdiFileLogsFilePathList, deletesearchList);

                if (listEdiFilesLogModel != null)
                {
                    foreach (var model in listEdiFilesLogModel)
                    {
                        if (model.EdiFileTypeID != (int)EnumEdiFileTypes.Edi835)
                        {
                            string filePath = HttpContext.Current.Server.MapCustomPath(model.FilePath);
                            if (File.Exists(filePath))
                                File.Delete(filePath);

                            AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                            amazonFileUpload.DeleteFile(ConfigSettings.ZarephathBucket, model.FilePath);
                        }
                    }
                }

                #endregion

                #region Delete Record from DataBase


                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                SetSearchFilterForEdiFilesLogListPage(searchEdiFileLogListPage, searchList);

                if (!string.IsNullOrEmpty(searchEdiFileLogListPage.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchEdiFileLogListPage.ListOfIdsInCSV });
                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

                List<ListEdiFileLogModel> totalData = GetEntityList<ListEdiFileLogModel>(StoredProcedure.DeleteEdiFileLog, searchList);
                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListEdiFileLogModel> getEdiFilesLogList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);

                #endregion

                response.Data = getEdiFilesLogList;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.EdiFileLog);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }

        #endregion

        #region Process 270/271


        #region Process 270
        public ServiceResponse Process270_271()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            var searchList = new List<SearchValueData>();
            AddProcess270271Model model = GetMultipleEntity<AddProcess270271Model>(StoredProcedure.GetAddProcess270271Model, searchList);
            model.AddProcess270Model.ServiceIDs = Resource.AllServicesText;
            model.AddProcess270Model.ReferralStatusIDs = Constants.Eligibilty270Status.Split(',').ToList();


            model.SearchProcess270ListPage.ServiceID = Resource.AllServicesText;
            model.SearchProcess270ListPage.IsDeleted = 0;
            model.SearchProcess271ListPage.ServiceID = Resource.AllServicesText;
            model.SearchProcess271ListPage.IsDeleted = 0;


            model.ServiceList = Common.SetServicesFilterFor270Process();
            model.DeleteFilter = Common.SetDeleteFilter();
            model.FileProcessStatus = Common.SetUpload835FileProcessStatusFilter();
            serviceResponse.Data = model;
            return serviceResponse;
        }

        public ServiceResponse Generate270File(AddProcess270Model addProcess270Model, SearchProcess270ListPage searchEdiFileLogListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            Edi270 edi270 = new Edi270();
            string filePath = String.Format(_cacheHelper.EdiFile270UploadPath, _cacheHelper.Domain);

            filePath = string.Format("{0}/", filePath);
            string tempFileName = string.Format("Edi270_{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmssfff"), Constants.FileExtension_Txt);


            var searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "PayorIDs", Value = addProcess270Model.PayorIDs });
            if (addProcess270Model.ReferralStatusIDs.Count > 0)
                searchList.Add(new SearchValueData() { Name = "ReferralStatusIDs", Value = String.Join(",", addProcess270Model.ReferralStatusIDs.Select(x => x.ToString()).ToArray()) });
            searchList.Add(new SearchValueData() { Name = "ServiceIDs", Value = addProcess270Model.ServiceIDs });
            searchList.Add(new SearchValueData() { Name = "ClientName", Value = addProcess270Model.Name });
            //searchList.Add(new SearchValueData() { Name = "ReferralStatusID", Value = ((int)ReferralStatus.ReferralStatuses.Active).ToString() });
            searchList.Add(new SearchValueData() { Name = "AllServiceText", Value = Resource.AllServicesText });

            Parent270DataModel model = GetMultipleEntity<Parent270DataModel>(StoredProcedure.GetClientDetailsFor270Process, searchList);
            if (model.ListClientDetailsfor270Model.Count > 0)
            {
                try
                {
                    PayorEdi270Setting payorEdi270Setting = model.PayorEdi270Setting;
                    Edi270Model edi837Model = GetEdit270Model(ref payorEdi270Setting, model);
                    string fileServerPath = HttpContext.Current.Server.MapCustomPath(filePath);
                    string generatedFilePath = edi270.GenerateEdi270File(edi837Model, fileServerPath, tempFileName);
                    string virtualPath = string.Format("{0}{1}", filePath, tempFileName);
                    SaveEntity(payorEdi270Setting);

                    Edi270271File ediFileLog = new Edi270271File();
                    ediFileLog.FileType = Edi270271FileType.FileType_270.ToString();
                    ediFileLog.FileName = tempFileName;
                    ediFileLog.FilePath = virtualPath;
                    ediFileLog.FileSize =
                        Common.GetFileSizeInBytes(HttpContext.Current.Server.MapCustomPath(ediFileLog.FilePath)).ToString();
                    ediFileLog.PayorIDs = model.AddProcess270Model.PayorIDs;
                    ediFileLog.ReferralStatusIDs = String.Join(",", addProcess270Model.ReferralStatusIDs.Select(x => x.ToString()).ToArray());

                    ediFileLog.ServiceIDs = model.AddProcess270Model.ServiceIDs;
                    ediFileLog.Name = addProcess270Model.Name;
                    ediFileLog.EligibilityCheckDate = addProcess270Model.EligibilityCheckDate;

                    #region amazonefileupload

                    AmazonFileUpload amazoneFileUpload = new AmazonFileUpload();
                    string fullpath = HttpContext.Current.Server.MapCustomPath(ediFileLog.FilePath);
                    if (ediFileLog.FilePath != null)
                        ediFileLog.FilePath = amazoneFileUpload.UploadFile(ConfigSettings.ZarephathBucket,
                            ediFileLog.FilePath.TrimStart('/'), fullpath, true);

                    #endregion amazonefileupload

                    SaveObject(ediFileLog, loggedInUserId);

                    serviceResponse = GetEdi270FileList(searchEdiFileLogListPage, pageIndex, pageSize, sortIndex,
                        sortDirection);
                    serviceResponse.IsSuccess = true;
                    serviceResponse.Message = Resource.EligibilityCheck270Generated;
                }
                catch (Exception ex)
                {
                    serviceResponse.IsSuccess = false;
                    serviceResponse.Message = Resource.ErrorOccured;
                }
            }
            else
            {
                serviceResponse.IsSuccess = false;
                serviceResponse.Message = Resource.SorryNoClientFound;
            }









            return serviceResponse;



        }

        public ServiceResponse GetEdi270FileList(SearchProcess270ListPage searchProcess270Model, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                if (searchProcess270Model != null)
                    SetSearchFilterForEdi270FilesListPage(searchProcess270Model, searchList);
                Page<ListEdi270FileLogModel> listEdiFilesLogModel = GetEntityPageList<ListEdi270FileLogModel>(StoredProcedure.GetEdi270271FileList, searchList, pageSize,
                                                              pageIndex, sortIndex, sortDirection);
                response.Data = listEdiFilesLogModel;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.EdiFileLog), Resource.ExceptionMessage);
            }
            return response;
        }

        private static void SetSearchFilterForEdi270FilesListPage(SearchProcess270ListPage searchEdiFileLogListPage, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "FileType", Value = Convert.ToString(Edi270271FileType.FileType_270) });
            searchList.Add(new SearchValueData { Name = "FileName", Value = searchEdiFileLogListPage.FileName });
            searchList.Add(new SearchValueData { Name = "Comment", Value = searchEdiFileLogListPage.Comment });
            searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchEdiFileLogListPage.PayorID) });
            searchList.Add(new SearchValueData { Name = "ServiceID", Value = Convert.ToString(searchEdiFileLogListPage.ServiceID) });
            searchList.Add(new SearchValueData { Name = "ClientName", Value = Convert.ToString(searchEdiFileLogListPage.Name) });
            searchList.Add(new SearchValueData { Name = "EligibilityCheckDate", Value = Convert.ToString(searchEdiFileLogListPage.EligibilityCheckDate) });
            searchList.Add(new SearchValueData { Name = "Upload271FileProcessStatus", Value = Convert.ToString(searchEdiFileLogListPage.Upload271FileProcessStatus) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchEdiFileLogListPage.IsDeleted) });
            searchList.Add(new SearchValueData() { Name = "AllServiceText", Value = Resource.AllServicesText });
        }

        public ServiceResponse DeleteEdi270File(SearchProcess270ListPage searchProcess270Model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

                SetSearchFilterForEdi270FilesListPage(searchProcess270Model, searchList);
                if (!string.IsNullOrEmpty(searchProcess270Model.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchProcess270Model.ListOfIdsInCSV });
                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                searchList.Add(new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(loggedInUserID) });

                List<ListEdi270FileLogModel> totalData = GetEntityList<ListEdi270FileLogModel>(StoredProcedure.DeleteEdi270271Files, searchList);
                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListEdi270FileLogModel> getEdiFilesLogList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);


                response.Data = getEdiFilesLogList;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.EdiFileLog);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }

        #endregion

        #region Process 271

        public ServiceResponse Upload271File(AddProcess271Model model, HttpRequestBase httpRequestBase, long loggedInUserId)
        {
            var response = new ServiceResponse();
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.Edi271FileLog);
            string basePath = String.Format(_cacheHelper.EdiFile271DownLoadPath, _cacheHelper.Domain);
            HttpPostedFileBase file = httpRequestBase.Files[0];
            response = Common.SaveFile(file, basePath);

            if (response.IsSuccess)
            {

                Edi270271File ediFileLog = new Edi270271File();
                ediFileLog.FileType = Edi270271FileType.FileType_271.ToString();
                ediFileLog.FileName = ((UploadedFileModel)response.Data).FileOriginalName; ;
                ediFileLog.FilePath = ((UploadedFileModel)response.Data).TempFilePath;
                ediFileLog.FileSize = Common.GetFileSizeInBytes(HttpContext.Current.Server.MapCustomPath(ediFileLog.FilePath)).ToString();
                ediFileLog.Comment = model.Comment;

                AmazonFileUpload amazonFileUpload = new AmazonFileUpload();

                #region  EDI 271 to CSV 271 Parser

                if (string.IsNullOrEmpty(ediFileLog.FilePath))
                {
                    response.Message = Resource.ErrorOccured;
                    return response;
                }
                try
                {
                    Edi271 edi271 = new Edi271();
                    string ediFilePath = ediFileLog.FilePath;

                    if (ediFileLog.FilePath.StartsWith("/"))
                        ediFilePath = HttpContext.Current.Server.MapCustomPath(ediFilePath);
                    else
                        ediFilePath = HttpContext.Current.Server.MapCustomPath("/" + ediFilePath);


                    string ediFile271CsvDownLoadPath = string.Format(_cacheHelper.EdiFile271CsvDownLoadPath, _cacheHelper.Domain);
                    string newRedablefilePath = string.Format("{0}{1}{2}", ediFile271CsvDownLoadPath, Guid.NewGuid(), Constants.FileExtension_Csv);

                    Edi271ResponseModel edi271ResponseModel = edi271.GenerateEdi271Model(ediFilePath, HttpContext.Current.Server.MapCustomPath(newRedablefilePath), newRedablefilePath);
                    ediFileLog.ReadableFilePath = amazonFileUpload.UploadFile(ConfigSettings.ZarephathBucket,
                                                        edi271ResponseModel.GeneratedFileRelativePath.TrimStart('/'), edi271ResponseModel.GeneratedFileAbsolutePath, true);
                    ediFileLog.Upload271FileProcessStatus = (int)EnumUpload835FileProcessStatus.Processed;
                }
                catch (Exception ex)
                {
                    string fileVirtualPath = Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.Edi835FileName, logpath);
                    string errorFullpath = HttpContext.Current.Server.MapCustomPath(fileVirtualPath);
                    if (fileVirtualPath != null)
                        ediFileLog.ReadableFilePath = amazonFileUpload.UploadFile(ConfigSettings.ZarephathBucket, fileVirtualPath.TrimStart('/'), errorFullpath, true);
                    ediFileLog.Upload271FileProcessStatus = (int)EnumUpload835FileProcessStatus.ErrorInProcess;
                }//GetScalar(StoredProcedure.UpdateBatchAfter835FileProcessed);


                #endregion

                #region Amazon File Upload

                string fullpath = HttpContext.Current.Server.MapCustomPath(ediFileLog.FilePath);
                if (ediFileLog.FilePath != null)
                    ediFileLog.FilePath = amazonFileUpload.UploadFile(ConfigSettings.ZarephathBucket, ediFileLog.FilePath.TrimStart('/'), fullpath, true);

                #endregion amazonefileupload

                SaveObject(ediFileLog, loggedInUserId);

                response.Message = Resource.Edi271FileUploaded;
            }
            return response;
        }



        public ServiceResponse GetEdi271FileList(SearchProcess271ListPage searchProcess271Model, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                if (searchProcess271Model != null)
                    SetSearchFilterForEdi271FilesListPage(searchProcess271Model, searchList);
                Page<ListEdi271FileLogModel> listEdiFilesLogModel = GetEntityPageList<ListEdi271FileLogModel>(StoredProcedure.GetEdi270271FileList, searchList, pageSize,
                                                              pageIndex, sortIndex, sortDirection);
                response.Data = listEdiFilesLogModel;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.EdiFileLog), Resource.ExceptionMessage);
            }
            return response;
        }

        private static void SetSearchFilterForEdi271FilesListPage(SearchProcess271ListPage searchEdiFileLogListPage, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "FileType", Value = Convert.ToString(Edi270271FileType.FileType_271) });
            searchList.Add(new SearchValueData { Name = "FileName", Value = searchEdiFileLogListPage.FileName });
            searchList.Add(new SearchValueData { Name = "Comment", Value = searchEdiFileLogListPage.Comment });
            searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchEdiFileLogListPage.PayorID) });
            searchList.Add(new SearchValueData { Name = "ServiceID", Value = Convert.ToString(searchEdiFileLogListPage.ServiceID) });
            searchList.Add(new SearchValueData { Name = "ClientName", Value = Convert.ToString(searchEdiFileLogListPage.Name) });
            searchList.Add(new SearchValueData { Name = "EligibilityCheckDate", Value = Convert.ToString(searchEdiFileLogListPage.EligibilityCheckDate) });
            searchList.Add(new SearchValueData { Name = "Upload271FileProcessStatus", Value = Convert.ToString(searchEdiFileLogListPage.Upload271FileProcessStatus) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchEdiFileLogListPage.IsDeleted) });
            searchList.Add(new SearchValueData() { Name = "AllServiceText", Value = Resource.AllServicesText });

        }

        public ServiceResponse DeleteEdi271File(SearchProcess271ListPage searchProcess271Model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

                SetSearchFilterForEdi271FilesListPage(searchProcess271Model, searchList);
                if (!string.IsNullOrEmpty(searchProcess271Model.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchProcess271Model.ListOfIdsInCSV });
                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                searchList.Add(new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(loggedInUserID) });

                List<ListEdi271FileLogModel> totalData = GetEntityList<ListEdi271FileLogModel>(StoredProcedure.DeleteEdi270271Files, searchList);
                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListEdi271FileLogModel> getEdiFilesLogList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);


                response.Data = getEdiFilesLogList;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.EdiFileLog);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }


        #endregion


        public ServiceResponse Process270_27112()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            Edi270 edi270 = new Edi270();
            string filePath = String.Format(_cacheHelper.EdiFile837UploadPath, _cacheHelper.Domain);
            filePath = string.Format("{0}/", filePath);
            string tempFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");// +"_" + Guid.NewGuid().ToString();
            var searchList = new List<SearchValueData>() { new SearchValueData() { Name = "ReferralStatusID", Value = ((int)ReferralStatus.ReferralStatuses.Active).ToString() } };
            Parent270DataModel model = GetMultipleEntity<Parent270DataModel>(StoredProcedure.GetClientDetailsFor270Process, searchList);
            if (model != null)
            {
                try
                {
                    PayorEdi270Setting payorEdi270Setting = model.PayorEdi270Setting;
                    Edi270Model edi837Model = GetEdit270Model(ref payorEdi270Setting, model);
                    string fileServerPath = HttpContext.Current.Server.MapCustomPath(filePath);
                    string generatedFilePath = edi270.GenerateEdi270File(edi837Model, fileServerPath, tempFileName + ".txt");
                    SaveEntity(payorEdi270Setting);
                }
                catch (Exception ex)
                {
                }
            }




            serviceResponse.IsSuccess = true;
            //serviceResponse.Data = listBrm;
            return serviceResponse;
        }


        private Edi270Model GetEdit270Model(ref PayorEdi270Setting payorEdi270Setting, Parent270DataModel parent270DataModel)
        {

            //if (payorEdi270Setting.ISA13_UpdatedDate == null || payorEdi270Setting.ISA13_UpdatedDate.Value.Date != DateTime.Now.Date)
            //{
            //    payorEdi270Setting.ISA13_InterchangeControlNumber = 1;
            //    payorEdi270Setting.ISA13_UpdatedDate = DateTime.Now;
            //}
            //else if (payorEdi270Setting.ISA13_UpdatedDate.Value.Date == DateTime.Now.Date)
            payorEdi270Setting.ISA13_InterchangeControlNumber = payorEdi270Setting.ISA13_InterchangeControlNumber + 1;

            long controlNumber = payorEdi270Setting.ISA13_InterchangeControlNumber;// 80;

            #region Get EDI 837 Model

            Edi270Model model = new Edi270Model();


            #region Add Information Source &  Receiver

            HlLevel270 hlLevel270 = new HlLevel270();

            hlLevel270.InformationSource.HeirarchicalLevelCode = parent270DataModel.InformationSource.HeirarchicalLevelCode;
            hlLevel270.InformationSource.EntityIdentifierCode = parent270DataModel.InformationSource.EntityIdentifierCode;
            hlLevel270.InformationSource.EntityTypeQualifier = parent270DataModel.InformationSource.EntityTypeQualifier;
            hlLevel270.InformationSource.NameLastOrOrganizationName = parent270DataModel.InformationSource.NameLastOrOrganizationName;
            hlLevel270.InformationSource.IdCodeQualifier = parent270DataModel.InformationSource.IdCodeQualifier;
            hlLevel270.InformationSource.IdCodeQualifierEnum = parent270DataModel.InformationSource.IdCodeQualifierEnum;


            hlLevel270.InformationReceiver.HeirarchicalLevelCode = parent270DataModel.InformationReceiver.HeirarchicalLevelCode;
            hlLevel270.InformationReceiver.EntityIdentifierCode = parent270DataModel.InformationReceiver.EntityIdentifierCode;
            hlLevel270.InformationReceiver.EntityTypeQualifier = parent270DataModel.InformationReceiver.EntityTypeQualifier;
            hlLevel270.InformationReceiver.NameLastOrOrganizationName = parent270DataModel.InformationReceiver.NameLastOrOrganizationName;
            hlLevel270.InformationReceiver.IdCodeQualifier = parent270DataModel.InformationReceiver.IdCodeQualifier;
            hlLevel270.InformationReceiver.IdCodeQualifierEnum = parent270DataModel.InformationReceiver.IdCodeQualifierEnum;



            #endregion

            foreach (var tempSubscriber in parent270DataModel.ListClientDetailsfor270Model)
            {

                #region Add Subscriber
                Subscriber270 subscriber = new Subscriber270()
                {
                    HeirarchicalLevelCode = tempSubscriber.HeirarchicalLevelCode,   // HL03

                    #region Subscriber Name
                    SubmitterEntityIdentifierCode = tempSubscriber.SubmitterEntityIdentifierCode, // NM101
                    SubmitterEntityTypeQualifier = tempSubscriber.SubmitterEntityTypeQualifier,// NM102
                    SubmitterNameLastOrOrganizationName = tempSubscriber.LastName, // NM103
                    SubmitterNameFirst = tempSubscriber.FirstName, // NM104
                    SubmitterIdCodeQualifier = tempSubscriber.SubmitterIdCodeQualifier, // NM108
                    SubmitterIdCodeQualifierEnum = tempSubscriber.SubmitterIdCodeQualifierEnum,// NM109


                    SubmitterDateTimePeriodFormatQualifier = tempSubscriber.SubmitterDateTimePeriodFormatQualifier,// DMG01
                    SubmitterDateTimePeriod = tempSubscriber.Dob,// DMG02
                    SubmitterGenderCode = tempSubscriber.Gender,// DMG03


                    SubmitterDTPQualifier = tempSubscriber.SubmitterDTPQualifier,// DTP01
                    SubmitterDTPFormatQualifier = tempSubscriber.SubmitterDTPFormatQualifier,// DTP02
                    SubmitterDTPDateTimePeriod = tempSubscriber.SubmitterDTPDateTimePeriod,// DTP03

                    SubmitterEligibility01 = tempSubscriber.SubmitterEligibility01
                    #endregion Subscriber Name


                };
                #endregion

                hlLevel270.Subscribers.Add(subscriber);
            }


            model.HlLevel270Model.Add(hlLevel270);



            #region Header Setter
            //ISA
            model.InterchangeControlHeader.AuthorizationInformationQualifier = payorEdi270Setting.ISA01_AuthorizationInformationQualifier;
            model.InterchangeControlHeader.AuthorizationInformation = payorEdi270Setting.ISA02_AuthorizationInformation;
            model.InterchangeControlHeader.SecurityInformationQualifier = payorEdi270Setting.ISA03_SecurityInformationQualifier;
            model.InterchangeControlHeader.SecurityInformation = payorEdi270Setting.ISA04_SecurityInformation;
            model.InterchangeControlHeader.InterchangeSenderIdQualifier = payorEdi270Setting.ISA05_InterchangeSenderIdQualifier; ;
            model.InterchangeControlHeader.InterchangeSenderId = payorEdi270Setting.ISA06_InterchangeSenderId;
            model.InterchangeControlHeader.InterchangeReceiverIdQualifier = payorEdi270Setting.ISA07_InterchangeReceiverIdQualifier;
            model.InterchangeControlHeader.InterchangeReceiverId = payorEdi270Setting.ISA08_InterchangeReceiverId;
            model.InterchangeControlHeader.InterchangeDate = payorEdi270Setting.ISA09_InterchangeDate;
            model.InterchangeControlHeader.InterchangeTime = payorEdi270Setting.ISA10_InterchangeTime;
            model.InterchangeControlHeader.RepetitionSeparator = payorEdi270Setting.ISA11_RepetitionSeparator;
            model.InterchangeControlHeader.InterchangeControlVersionNumber = payorEdi270Setting.ISA12_InterchangeControlVersionNumber;
            model.InterchangeControlHeader.InterchangeControlNumber = string.Format("{0:00000000}", controlNumber);
            model.InterchangeControlHeader.AcknowledgementRequired = payorEdi270Setting.ISA14_AcknowledgementRequired;
            model.InterchangeControlHeader.UsageIndicator = payorEdi270Setting.ISA15_UsageIndicator;
            model.InterchangeControlHeader.ComponentElementSeparator = payorEdi270Setting.ISA16_ComponentElementSeparator;

            model.InterchangeControlHeader.SegmentTerminator = payorEdi270Setting.SegmentTerminator;
            model.InterchangeControlHeader.ElementSeparator = payorEdi270Setting.ElementSeparator;

            //GS
            model.FunctionalGroupHeader.FunctionalIdentifierCode = payorEdi270Setting.GS01_FunctionalIdentifierCode;
            model.FunctionalGroupHeader.ApplicationSenderCode = payorEdi270Setting.GS02_ApplicationSenderCode;
            model.FunctionalGroupHeader.ApplicationReceiverCode = payorEdi270Setting.GS03_ApplicationReceiverCode;
            model.FunctionalGroupHeader.Date = payorEdi270Setting.GS04_Date;
            model.FunctionalGroupHeader.Time = payorEdi270Setting.GS05_Time;
            model.FunctionalGroupHeader.GroupControlNumber = payorEdi270Setting.GS06_GroupControlNumber;// "80";
            model.FunctionalGroupHeader.ResponsibleAgencyCode = payorEdi270Setting.GS07_ResponsibleAgencyCode;
            model.FunctionalGroupHeader.VersionOrReleaseOrNo = payorEdi270Setting.GS08_VersionOrReleaseOrNo;

            //ST
            model.TransactionSetHeader.TransactionSetIdentifier = payorEdi270Setting.ST01_TransactionSetIdentifier;
            model.TransactionSetHeader.TransactionSetControlNumber = payorEdi270Setting.ST02_TransactionSetControlNumber;// "0080";
            model.TransactionSetHeader.ImplementationConventionReference = payorEdi270Setting.ST03_ImplementationConventionReference;

            //BHT
            model.BeginningOfHierarchicalTransaction.HierarchicalStructureCode = payorEdi270Setting.BHT01_HierarchicalStructureCode;
            model.BeginningOfHierarchicalTransaction.TransactionSetPurposeCode = payorEdi270Setting.BHT02_TransactionSetPurposeCode;
            model.BeginningOfHierarchicalTransaction.ReferenceIdentification = payorEdi270Setting.BHT03_ReferenceIdentification;
            model.BeginningOfHierarchicalTransaction.Date = payorEdi270Setting.BHT04_Date;
            model.BeginningOfHierarchicalTransaction.InterchangeIdQualifier = payorEdi270Setting.BHT05_Time;
            model.BeginningOfHierarchicalTransaction.TransactionTypeCode = payorEdi270Setting.BHT06_TransactionTypeCode;


            //SUBMITTER NAME NM1

            #endregion



            #endregion
            return model;
        }

        #endregion

        #region Process 277

        public ServiceResponse Process277()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            var searchList = new List<SearchValueData>();
            AddProcess277PageModel model = GetMultipleEntity<AddProcess277PageModel>(StoredProcedure.GetAddProcess277Model, searchList);
            model.SearchProcess277ListPage.IsDeleted = 0;
            model.DeleteFilter = Common.SetDeleteFilter();
            serviceResponse.Data = model;
            return serviceResponse;
        }
        public ServiceResponse Upload277File(AddProcess277Model model, HttpRequestBase httpRequestBase, long loggedInUserId)
        {
            var response = new ServiceResponse();
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.Edi277FileLog);
            string basePath = string.Format(_cacheHelper.EdiFile277DownLoadPath, _cacheHelper.Domain);

            HttpPostedFileBase file = httpRequestBase.Files[0];
            response = Common.SaveFile(file, basePath);

            if (response.IsSuccess)
            {

                Edi277File ediFileLog = new Edi277File();
                ediFileLog.FileType = Edi277FileType.FileType_277.ToString();
                ediFileLog.FileName = ((UploadedFileModel)response.Data).FileOriginalName; ;
                ediFileLog.FilePath = ((UploadedFileModel)response.Data).TempFilePath;
                ediFileLog.FileSize = Common.GetFileSizeInBytes(HttpContext.Current.Server.MapCustomPath(ediFileLog.FilePath)).ToString();
                ediFileLog.Comment = model.Comment;
                ediFileLog.PayorID = model.PayorID;
                ediFileLog.Upload277FileProcessStatus = (int)EnumUpload835FileProcessStatus.UnProcessed;


                #region  EDI 277 to CSV 277 Parser

                if (string.IsNullOrEmpty(ediFileLog.FilePath))
                {
                    response.Message = Resource.ErrorOccured;
                    return response;
                }

                #endregion

                #region Amazon File Upload
                AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                string fullpath = HttpContext.Current.Server.MapCustomPath(ediFileLog.FilePath);
                if (ediFileLog.FilePath != null)
                    ediFileLog.FilePath = amazonFileUpload.UploadFile(ConfigSettings.ZarephathBucket, ediFileLog.FilePath.TrimStart('/'), fullpath, true);

                #endregion amazonefileupload

                SaveObject(ediFileLog, loggedInUserId);

                response.Message = Resource.Edi277FileUploaded;
            }
            return response;
        }

        public ServiceResponse BackEndProcess277File()
        {
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.Edi277FileLog, Common.GenerateRandomNumber() + "/");

            ServiceResponse response = new ServiceResponse();
            try
            {
                Common.CreateLogFile("Process Edi277Files CronJob Started.", ConfigSettings.Edi277FileName, logpath);
                #region Process 277 Files
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "GetUpload277FileProcessStatus", Value = ((int)EnumUpload277FileProcessStatus.UnProcessed).ToString() });
                searchList.Add(new SearchValueData { Name = "SetUpload277FileProcessStatus", Value = ((int)EnumUpload277FileProcessStatus.InProcess).ToString() });
                List<Edi277File> upload277FileList = GetEntityList<Edi277File>(StoredProcedure.GetEdi277FileForProcess, searchList);

                foreach (var upload277File in upload277FileList)
                {
                    Edi277ResponseModel edi277ResponseModel = new Edi277ResponseModel();
                    string ediFilePath = HttpContext.Current.Server.MapCustomPath("/" + upload277File.FilePath);
                    AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                    amazonFileUpload.DownloadFile(ConfigSettings.ZarephathBucket, upload277File.FilePath, ediFilePath);

                    try
                    {

                        Edi277 edi277 = new Edi277();
                        //if (ediFilePath.StartsWith("/"))
                        //    ediFilePath = HttpContext.Current.Server.MapCustomPath(ediFilePath);
                        //else
                        //    ediFilePath = HttpContext.Current.Server.MapCustomPath("/" + ediFilePath);

                        edi277ResponseModel = edi277.GenerateEdi277Model(ediFilePath, edi277ResponseModel);

                        foreach (var edi277Model in edi277ResponseModel.Edi277ModelList)
                        {

                            Edi277FileDetail edi277FileDetail = new Edi277FileDetail();
                            edi277FileDetail.Edi277FileID = upload277File.Edi277FileID;
                            edi277FileDetail.Source = edi277Model.Source;
                            edi277FileDetail.TraceNumber = edi277Model.TraceNumber;
                            edi277FileDetail.ReceiptDate = edi277Model.ReceiptDate;
                            edi277FileDetail.ProcessDate = edi277Model.ProcessDate;
                            edi277FileDetail.Receiver = edi277Model.Receiver;
                            edi277FileDetail.TotalAcceptedClaims = edi277Model.TotalAcceptedClaims;
                            edi277FileDetail.TotalAcceptedAmount = edi277Model.TotalAcceptedAmount;
                            edi277FileDetail.TotalRejectedClaims = edi277Model.TotalRejectedClaims;
                            edi277FileDetail.TotalRejectedAmount = edi277Model.TotalRejectedAmount;
                            edi277FileDetail.Provider = edi277Model.Provider;
                            edi277FileDetail.LastName = edi277Model.LastName;
                            edi277FileDetail.FirstName = edi277Model.FirstName;
                            edi277FileDetail.AHCCCSID = edi277Model.AHCCCSID;
                            edi277FileDetail.CSCC = edi277Model.CSCC;
                            edi277FileDetail.CSC = edi277Model.CSC;
                            edi277FileDetail.EIC = edi277Model.EIC;
                            edi277FileDetail.Action = edi277Model.Action;
                            edi277FileDetail.Amount = edi277Model.Amount;
                            edi277FileDetail.Message = edi277Model.Message;
                            edi277FileDetail.BatchNoteID = edi277Model.BatchNoteID;
                            //edi277FileDetail.BatchID = edi277Model.BatchID;
                            //edi277FileDetail.NoteID = edi277Model.NoteID;
                            edi277FileDetail.ClaimNumber = edi277Model.ClaimNumber;
                            edi277FileDetail.PayorClaimNumber = edi277Model.PayorClaimNumber;
                            edi277FileDetail.ServiceDate = edi277Model.ServiceDate;
                            SaveEntity(edi277FileDetail);
                        }

                        upload277File.Upload277FileProcessStatus = (int)EnumUpload835FileProcessStatus.Processed;
                        SaveEntity(upload277File);

                        searchList = new List<SearchValueData>();
                        searchList.Add(new SearchValueData { Name = "Edi277FileID", Value = upload277File.Edi277FileID.ToString() });
                        GetScalar(StoredProcedure.UpdateBNFromEdi277FileDetails, searchList);

                    }
                    catch (Exception ex)
                    {
                        string fileVirtualPath = Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.Edi277FileName, logpath);
                        string fullpath = HttpContext.Current.Server.MapCustomPath(fileVirtualPath);
                        upload277File.ReadableFilePath = amazonFileUpload.UploadFile(ConfigSettings.ZarephathBucket, fileVirtualPath.TrimStart('/'), fullpath, true);
                        upload277File.Upload277FileProcessStatus = (int)EnumUpload835FileProcessStatus.ErrorInProcess;
                        SaveEntity(upload277File);
                    }
                }

                #endregion
                Common.CreateLogFile("Process Edi277Files CronJob Completed Succesfully.", ConfigSettings.Edi277FileName, logpath);

            }
            catch (Exception ex)
            {
                Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.Edi277FileName, logpath);
                response.IsSuccess = false;
            }
            return response;
        }

        public ServiceResponse GetEdi277FileList(SearchProcess277ListPage searchProcess277Model, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                if (searchProcess277Model != null)
                    SetSearchFilterForEdi277FilesListPage(searchProcess277Model, searchList);
                Page<ListEdi277FileLogModel> listEdiFilesLogModel = GetEntityPageList<ListEdi277FileLogModel>(StoredProcedure.GetEdi277FileList, searchList, pageSize,
                                                              pageIndex, sortIndex, sortDirection);
                response.Data = listEdiFilesLogModel;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.EdiFileLog), Resource.ExceptionMessage);
            }
            return response;
        }
        private static void SetSearchFilterForEdi277FilesListPage(SearchProcess277ListPage searchEdiFileLogListPage, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "FileType", Value = Convert.ToString(Edi277FileType.FileType_277.ToString()) });
            searchList.Add(new SearchValueData { Name = "FileName", Value = searchEdiFileLogListPage.FileName });
            searchList.Add(new SearchValueData { Name = "Comment", Value = searchEdiFileLogListPage.Comment });
            searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchEdiFileLogListPage.PayorID) });
            searchList.Add(new SearchValueData { Name = "Upload277FileProcessStatus", Value = Convert.ToString(searchEdiFileLogListPage.Upload277FileProcessStatus) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchEdiFileLogListPage.IsDeleted) });

        }
        public ServiceResponse DeleteEdi277File(SearchProcess277ListPage searchProcess277Model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

                SetSearchFilterForEdi277FilesListPage(searchProcess277Model, searchList);
                if (!string.IsNullOrEmpty(searchProcess277Model.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchProcess277Model.ListOfIdsInCSV });
                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                searchList.Add(new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(loggedInUserID) });

                List<ListEdi277FileLogModel> totalData = GetEntityList<ListEdi277FileLogModel>(StoredProcedure.DeleteEdi277Files, searchList);
                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListEdi277FileLogModel> getEdiFilesLogList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);


                response.Data = getEdiFilesLogList;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.EdiFileLog);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }


        public ServiceResponse Download277RedableFile(string id)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "Edi277FileID", Value = id });
            List<Edi277RedableFileModel> totalData = GetEntityList<Edi277RedableFileModel>(StoredProcedure.Download277RedableFile, searchList);

            string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);

            if (totalData.Count > 0)
            {
                string basePath = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                string fileName = string.Format("{0}_{1}", Constants.ReportName_Edi277, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_xlsx);
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                CreateExcelFile.CreateExcelDocument(totalData, downloadFileModel.AbsolutePath);
                response.IsSuccess = true;
                response.Data = downloadFileModel;
            }
            return response;


        }



        #endregion



        #region Home Care Related Code

        #region Batch 837

        #region Add Batch /List Batch

        public ServiceResponse HC_SetAddERAPage(long claimId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ClaimID", Value = claimId.ToString()}
                };
            HC_ERAModel addERAModel = GetMultipleEntity<HC_ERAModel>(StoredProcedure.HC_GetSetAddERAPage, searchParam);
            response.Data = addERAModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_SetAddBatchPage(long batchId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "BatchID", Value = batchId.ToString()}
                };
            HC_AddBatchModel addBatchModel = GetMultipleEntity<HC_AddBatchModel>(StoredProcedure.HC_GetSetAddBatchPage, searchParam);
            addBatchModel.BatchStatusFilter = Common.SetBatchStatusFilter();
            addBatchModel.SearchPatientList = new SearchPatientList();
            addBatchModel.SearchBatchList = new SearchBatchList();
            addBatchModel.SearchBatchList.IsSentStatus = -1;
            addBatchModel.SearchPatientList.BatchTypeID = 1;
            addBatchModel.SearchBatchList.BatchView = true;

            if (addBatchModel.Batch == null)
                addBatchModel.Batch = new Batch();
            else
            {
                addBatchModel.SearchBatchList.BatchID = addBatchModel.Batch.BatchID;
                addBatchModel.SearchBatchList.StrBatchID = Convert.ToString(addBatchModel.Batch.BatchID);
                addBatchModel.SearchBatchList.BatchView = false;
                
            }

            #region Check Whether Cookies are Present for the Aging Report Page Or Not


            HttpCookie agingReportCookie = HttpContext.Current.Request.Cookies[Constants.Cookie_AgingReportFilters];


            if (agingReportCookie != null && agingReportCookie.Value != "null"  && !string.IsNullOrEmpty(agingReportCookie.Value))
            {
                ARAgingModel item = JsonConvert.DeserializeObject<ARAgingModel>(HttpUtility.UrlDecode(agingReportCookie.Value));

                HttpContext.Current.Request.Cookies.Remove(Constants.Cookie_AgingReportFilters);
                HttpContext.Current.Response.Cookies.Remove(Constants.Cookie_AgingReportFilters);
                agingReportCookie.Expires = DateTime.Now.AddDays(-1);
                agingReportCookie.Value = null;
                HttpContext.Current.Response.SetCookie(agingReportCookie);


                addBatchModel.SearchBatchList.PayorID = item.PayorID == 0 ? addBatchModel.SearchBatchList.PayorID :  item.PayorID;
                addBatchModel.SearchBatchList.ClientName = item.ClientName;
                addBatchModel.SearchBatchList.ReconcileStatus = item.StrReconcileStatus;
                addBatchModel.SearchBatchList.ClaimAdjustmentTypeID = ClaimAdjustmentType.ClaimAdjustmentType_WriteOff;


                switch (item.IndexForSelectedRange)
                {
                    case (int)Enum_IndexForSelectedRange.Range_0_60:
                        addBatchModel.SearchBatchList.EndDate = DateTime.Today;
                        addBatchModel.SearchBatchList.StartDate = DateTime.Today.AddDays(-60);
                        break;

                    case (int)Enum_IndexForSelectedRange.Range_61_90:
                        addBatchModel.SearchBatchList.EndDate = DateTime.Today.AddDays(-61);
                        addBatchModel.SearchBatchList.StartDate = DateTime.Today.AddDays(-90);
                        break;

                    case (int)Enum_IndexForSelectedRange.Range_91_120:
                        addBatchModel.SearchBatchList.EndDate = DateTime.Today.AddDays(-91);
                        addBatchModel.SearchBatchList.StartDate = DateTime.Today.AddDays(-120);
                        break;

                    case (int)Enum_IndexForSelectedRange.Range_121_180:
                        addBatchModel.SearchBatchList.EndDate = DateTime.Today.AddDays(-121);
                        addBatchModel.SearchBatchList.StartDate = DateTime.Today.AddDays(-180);
                        break;

                    case (int)Enum_IndexForSelectedRange.Range_181_270:
                        addBatchModel.SearchBatchList.EndDate = DateTime.Today.AddDays(-181);
                        addBatchModel.SearchBatchList.StartDate = DateTime.Today.AddDays(-270);
                        break;

                    case (int)Enum_IndexForSelectedRange.Range_271_365:
                        addBatchModel.SearchBatchList.EndDate = DateTime.Today.AddDays(-271);
                        addBatchModel.SearchBatchList.StartDate = DateTime.Today.AddDays(-365);
                        break;

                    case (int)Enum_IndexForSelectedRange.Range_0_365:
                        addBatchModel.SearchBatchList.EndDate = DateTime.Today;
                        addBatchModel.SearchBatchList.StartDate = DateTime.Today.AddDays(-365);
                        break;

                }
            }


            #endregion






            response.Data = addBatchModel;
            response.IsSuccess = true;
            return response;
        }


        public ServiceResponse HC_AddBatchDetail(HC_AddBatchModel addBatchModel, long loggedInUserId, bool isCaseManagement = false)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {






                if (addBatchModel.Batch != null)
                {
                    response = HC_ValidateTemporaryNotes(addBatchModel.Batch.PatientIds, addBatchModel.Batch.PayorID, addBatchModel.Batch.BatchTypeID, loggedInUserId, isCaseManagement);
                    if (response.ErrorCode != "Success")
                    {
                        response.IsSuccess = false;
                        return response;
                    }







                    List<string> patienIds = new List<string>();
                    if (addBatchModel.Batch.CreatePatientWiseBatch)
                    {
                        patienIds = addBatchModel.Batch.PatientIds.Split(',').ToList();
                    }
                    else
                    {
                        patienIds.Add(addBatchModel.Batch.PatientIds);
                    }

                    int batchcount = patienIds.Count();
                    int batchcreated = 0;
                    int batchfailed = 0;
                    foreach (var patienId in patienIds)
                    {
                        try
                        {
                            List<SearchValueData> searchParam = new List<SearchValueData>
                            {
                                new SearchValueData {Name = "BatchID", Value =Convert.ToString(addBatchModel.Batch.BatchID)},
                                new SearchValueData {Name = "BatchTypeID", Value =Convert.ToString(addBatchModel.Batch.BatchTypeID)},
                                new SearchValueData {Name = "PayorID", Value =Convert.ToString(addBatchModel.Batch.PayorID)},
                                new SearchValueData {Name = "ServiceCodeIDs", Value =Convert.ToString(addBatchModel.Batch.ServiceCodeIDs)},
                                //new SearchValueData {Name = "ReferralsIds", Value =Convert.ToString(addBatchModel.Batch.PatientIds)},
                                new SearchValueData {Name = "ReferralsIds", Value =Convert.ToString(patienId)},

                                new SearchValueData {Name = "StartDate", Value =Convert.ToDateTime(addBatchModel.Batch.StartDate).ToString(Constants.DbDateFormat)},
                                new SearchValueData {Name = "EndDate", Value =Convert.ToDateTime(addBatchModel.Batch.EndDate).ToString(Constants.DbDateFormat)},

                                new SearchValueData {Name = "Comment", Value =Convert.ToString(addBatchModel.Batch.Comment)},

                                new SearchValueData {Name = "CreatedBy", Value =Convert.ToString(loggedInUserId)},
                                new SearchValueData {Name = "BatchNoteStatusID", Value =Convert.ToInt16(EnumBatchNoteStatus.Addded).ToString()},
                             };



                            searchParam.Add(new SearchValueData { Name = "IsDayCare", Value = Convert.ToString(SessionHelper.IsDayCare) });
                            searchParam.Add(new SearchValueData { Name = "IsCaseManagement", Value = Convert.ToString(SessionHelper.IsCaseManagement) });


                            GetScalar(StoredProcedure.HC_SaveNewBatch, searchParam);



                            batchcreated = batchcreated + 1;
                        }
                        catch (Exception ex)
                        {
                            response.Message = response.Message + "\r\n" + ex.Message;
                            batchfailed = batchfailed + 1;
                        }

                    }
                    //if (!isCaseManagement)
                    //    GetScalar(StoredProcedure.HC_SaveNewBatch, searchParam);
                    //else
                    //    GetScalar(StoredProcedure.HC_CM_SaveNewBatch, searchParam);

                    //response.Message = string.Format(Resource.RecordCreatedSuccessfully, Resource.Batch);
                    if (batchcreated == batchcount)
                        response.Message = "All claims are validated. Batch(es) created successfully.";
                    else
                        response.Message = string.Format("All claims are validated. {0}/{1} batch(es) created successfully.", batchcreated, batchcount);

                    response.IsSuccess = true;
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_RefreshAndGroupingNotes()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                var searchList = new List<SearchValueData>();
                int data = (int)GetScalar(StoredProcedure.HC_RefreshAndGroupingNotes, searchList);
                if (data == 1)
                {
                    serviceResponse.Message = Resource.ServiceRunSuccessfully;
                    serviceResponse.Data = data;
                    serviceResponse.IsSuccess = true;
                }
                else
                {
                    serviceResponse.Message = Resource.ErrorOccured;
                    serviceResponse.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.IsSuccess = false;
                serviceResponse.Message = Resource.ServiceExceptionMessage;
            }
            return serviceResponse;
        }

        public ServiceResponse HC_GetPatientList(SearchPatientList SearchPatientList, bool isCaseManagement = false)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "BatchTypeID", Value = Convert.ToString(SearchPatientList.BatchTypeID) });
                searchParam.Add(new SearchValueData { Name = "PayorId", Value = Convert.ToString(SearchPatientList.PayorID) });
                searchParam.Add(new SearchValueData { Name = "StartDate", Value = (SearchPatientList.StartDate).ToString(Constants.DbDateFormat) });
                searchParam.Add(new SearchValueData { Name = "EndDate", Value = SearchPatientList.EndDate.ToString(Constants.DbDateFormat) });
                searchParam.Add(new SearchValueData { Name = "ClientName", Value = SearchPatientList.ClientName });
                searchParam.Add(new SearchValueData { Name = "ServiceCodeIDs", Value = SearchPatientList.ServiceCodeIDs });
                searchParam.Add(new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(SessionHelper.LoggedInID) });

                if (!isCaseManagement)
                {
                    searchParam.Add(new SearchValueData { Name = "IsDayCare", Value = Convert.ToString(SessionHelper.IsDayCare) });
                    //ListPatientModelClaims model = GetMultipleEntity<ListPatientModelClaims>(StoredProcedure.HC_GetPatientList, searchParam);
                    ListPatientModelClaims model = GetMultipleEntity<ListPatientModelClaims>(StoredProcedure.HC_GetPatientList_Temporary, searchParam);


                    response.Data = model;
                }
                else
                {
                    ListPatientModelClaims model = new ListPatientModelClaims();
                    model.ListPatientModel = GetEntityList<ListPatientModel>(StoredProcedure.HC_CM_GetPatientList_Temporary, searchParam);
                    response.Data = model;
                }
                response.IsSuccess = true;
                return response;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_GetChildNoteDetails(SearchPatientList SearchPatientList, bool isCaseManagement = false)
        {
            var response = new ServiceResponse();
            var searchlist = new List<SearchValueData>();
            searchlist.Add(new SearchValueData { Name = "BatchID", Value = Convert.ToString(SearchPatientList.BatchID) });
            searchlist.Add(new SearchValueData { Name = "BatchTypeID", Value = Convert.ToString(SearchPatientList.BatchTypeID) });
            searchlist.Add(new SearchValueData { Name = "PayorId", Value = Convert.ToString(SearchPatientList.PayorID) });
            searchlist.Add(new SearchValueData { Name = "StartDate", Value = SearchPatientList.StartDate.ToString(Constants.DbDateFormat) });
            searchlist.Add(new SearchValueData { Name = "EndDate", Value = SearchPatientList.EndDate.ToString(Constants.DbDateFormat) });
            searchlist.Add(new SearchValueData { Name = "ServiceCodeIDs", Value = SearchPatientList.ServiceCodeIDs });
            searchlist.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(SearchPatientList.ReferralID) });
            searchlist.Add(new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(SessionHelper.LoggedInID) });
            searchlist.Add(new SearchValueData { Name = "NoteIDs", Value = Convert.ToString(SearchPatientList.NoteIDs) });

            List<ChildNoteListModel> childNoteListModel = null;
            //childNoteListModel = GetEntityList<ChildNoteListModel>(StoredProcedure.HC_GetChildNoteList, searchlist);
            childNoteListModel = GetEntityList<ChildNoteListModel>(StoredProcedure.HC_GetChildNoteList_Temporary, searchlist);

            //if (!isCaseManagement)
            //    childNoteListModel = GetEntityList<ChildNoteListModel>(StoredProcedure.HC_GetChildNoteList, searchlist);
            //else
            //    childNoteListModel = GetEntityList<ChildNoteListModel>(StoredProcedure.HC_CaseManagement_GetChildNoteList, searchlist);
            response.Data = childNoteListModel;
            response.IsSuccess = true;

            return response;
        }



        public ServiceResponse HC_SaveMannualPaymentPostingDetails(MannualPaymentPostingModel model)
        {
            var response = new ServiceResponse();
            var searchlist = new List<SearchValueData>();
            searchlist.Add(new SearchValueData { Name = "BatchID", Value = Convert.ToString(model.BatchID) });
            searchlist.Add(new SearchValueData { Name = "BatchNoteID", Value = Convert.ToString(model.BatchNoteID) });
            searchlist.Add(new SearchValueData { Name = "MPP_AdjustmentAmount", Value = Convert.ToString(model.MPP_AdjustmentAmount) });
            searchlist.Add(new SearchValueData { Name = "MPP_AdjustmentGroupCodeID", Value = model.MPP_AdjustmentGroupCodeID });
            searchlist.Add(new SearchValueData { Name = "MPP_AdjustmentGroupCodeName", Value = model.MPP_AdjustmentGroupCodeName });
            searchlist.Add(new SearchValueData { Name = "MPP_AdjustmentComment", Value = model.MPP_AdjustmentComment });
            GetScalar(StoredProcedure.HC_SaveMannualPaymentPostingDetails, searchlist);
            response.IsSuccess = true;
            response.Message = Resource.DetaiSavedSuccessfully;
            return response;
        }
        
        public ServiceResponse HC_GetBatchDetails(string BatchID)
        {
            var response = new ServiceResponse();
            var searchlist = new List<SearchValueData>();
            searchlist.Add(new SearchValueData { Name = "BatchID", Value = BatchID });

            List<ListPatientModel> batchDetalsModel = GetEntityList<ListPatientModel>(StoredProcedure.HC_ViewBatchDetails, searchlist);
            response.Data = batchDetalsModel;
            response.IsSuccess = true;

            return response;
        }


        public ServiceResponse HC_GetEdi837BatchDetails(string batchId, long loggedInUserId, bool isCaseManagement = false)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            Edi837 edi837 = new Edi837();
            List<BatchValidationResponseModel> listBrm = new List<BatchValidationResponseModel>();


            var searchList = new List<SearchValueData>()
                {
                    new SearchValueData { Name = "BatchID", Value = Convert.ToString(batchId) },
                    new SearchValueData { Name = "FileType", Value = "CMS1500" },
                    new SearchValueData { Name = "IsCaseManagement",Value =  Convert.ToString(isCaseManagement)}
                };
            ParentBatchRelatedAllDataModel model = GetMultipleEntity<ParentBatchRelatedAllDataModel>(StoredProcedure.HC_GetBatchRelatedAllData, searchList);
            if (isCaseManagement)
            {
                model.BatchRelatedAllDataModel.ForEach(c => c.IsCaseManagement = true);
            }
            else if (SessionHelper.IsHomeCare)
            {
                model.BatchRelatedAllDataModel.ForEach(c => c.IsHomeCare = true);
            }
            else if (SessionHelper.IsDayCare)
            {
                model.BatchRelatedAllDataModel.ForEach(c => c.IsDayCare = true);
            }

            //model.BatchRelatedAllDataModel = model.BatchRelatedAllDataModel.Where(c => c.IsUseInBilling).ToList();


            if (model != null)
            {
                try
                {
                    PayorEdi837Setting payorEdi837Setting = model.PayorEdi837Setting;
                    GroupedModelFor837 edi837Model = Common_Edi_837.GetEdit837ModelForView(Convert.ToInt64(batchId), ref payorEdi837Setting, model.BatchRelatedAllDataModel, EdiFileType.Edi_837_P);
                    
                    Batch batchModel = GetEntity<Batch>(Convert.ToInt64(batchId));
                    foreach (var item in edi837Model.BillingProviders)
                    {
                        foreach (var subscriber in item.Subscribers)
                        {
                            long TotalClaimCount = 0;
                            float TotalAmount = 0;
                            float TotalAllowedAmount = 0;
                            float TotalPaidAmount = 0;
                            float TotalMPP_AdjustmentAmount = 0;
                            foreach (var claim in subscriber.Claims)
                            {
                                SubscriberModel_Updated modelSub = new SubscriberModel_Updated();
                                modelSub.ReferralID = subscriber.ReferralID;
                                modelSub.AHCCCSID = subscriber.AHCCCSID;
                                modelSub.CISNumber = subscriber.CISNumber;
                                modelSub.PolicyNumber = subscriber.PolicyNumber;
                                modelSub.FirstName = subscriber.FirstName;
                                modelSub.LastName = subscriber.LastName;
                                modelSub.Dob = subscriber.Dob;
                                modelSub.Gender = subscriber.Gender;
                                modelSub.SubscriberID = subscriber.SubscriberID;
                                modelSub.Address = subscriber.Address;
                                modelSub.City = subscriber.City;
                                modelSub.State = subscriber.State;
                                modelSub.ZipCode = subscriber.ZipCode;
                                modelSub.PayorIdentificationNumber = subscriber.PayorIdentificationNumber;
                                modelSub.PayorName = subscriber.PayorName;
                                modelSub.PayorAddress = subscriber.PayorAddress;
                                modelSub.PayorCity = subscriber.PayorCity;
                                modelSub.PayorState = subscriber.PayorState;
                                modelSub.PayorZipcode = subscriber.PayorZipcode;
                                modelSub.PayorBillingType = subscriber.PayorBillingType;
                                modelSub.PayorID = subscriber.PayorID;
                                modelSub.BatchID = Convert.ToInt64(batchId);




                                modelSub.Claim = claim;
                                modelSub.Claim.CalculatedAmount = claim.ServiceLines.Sum(c => c.CalculatedAmount);
                                modelSub.Claim.TotalAmount = modelSub.Claim.CalculatedAmount;
                                modelSub.Claim.TotalAllowedAmount = claim.ServiceLines.Sum(c => c.AMT01_ServiceLineAllowedAmount_AllowedAmount);
                                modelSub.Claim.TotalPaidAmount = claim.ServiceLines.Sum(c => c.SVC03_LineItemProviderPaymentAmoun_PaidAmount);
                                modelSub.Claim.TotalMPP_AdjustmentAmount = claim.ServiceLines.Sum(c => c.MPP_AdjustmentAmount);

                                TotalAmount = TotalAmount + modelSub.Claim.TotalAmount;
                                TotalAllowedAmount = TotalAllowedAmount + modelSub.Claim.TotalAllowedAmount;
                                TotalPaidAmount = TotalPaidAmount + modelSub.Claim.TotalPaidAmount;
                                TotalClaimCount = TotalClaimCount + claim.ServiceLines.Count;
                                TotalMPP_AdjustmentAmount = TotalMPP_AdjustmentAmount + modelSub.Claim.TotalMPP_AdjustmentAmount;

                                modelSub.Claim.StrNoteIds = string.Join(",", claim.ServiceLines.Select(i => Convert.ToString(i.NoteID)).ToArray());
                                item.Subscribers_Updated.Add(modelSub);



                                if (batchModel.IsSent)
                                {

                                    ClaimModel claimModel = new ClaimModel();
                                    claimModel.BatchId = Convert.ToInt64(batchId);
                                    claimModel.ClaimUniqueTraceID = claim.ClaimUniqueTraceID;
                                    claimModel.CalculatedAmount = modelSub.Claim.CalculatedAmount;
                                    ServiceResponse tempResponse = GetClaimMessageList(claimModel);
                                    if (tempResponse.IsSuccess)
                                    {

                                        claim.ClaimMessages = new List<ClaimMessageModel>();
                                        claim.ClaimMessages = (List<ClaimMessageModel>)tempResponse.Data;

                                        
                                        if (claim.ClaimMessages != null && claim.ClaimMessages.Count > 0)
                                        {
                                            string status = claim.ClaimMessages.Last().e_status;
                                            if (!string.IsNullOrEmpty(status))
                                            {
                                                claim.ClaimLevelStatus = status.ToUpper() == "A" ? "Accepted" : (status.ToUpper() == "R" ? "Rejected" : "NA");
                                            }
                                        }

                                        if (claim.ClaimLevelStatus!= "Rejected" && (TotalAllowedAmount != 0 || TotalPaidAmount != 0))
                                            claim.ClaimLevelStatus = "Processed";

                                    }
                                }


                            }

                            subscriber.TotalAmount = TotalAmount;
                            subscriber.TotalAllowedAmount = TotalAllowedAmount;
                            subscriber.TotalPaidAmount = TotalPaidAmount;
                            subscriber.TotalClaimCount = TotalClaimCount;
                            subscriber.TotalMPP_AdjustmentAmount = TotalMPP_AdjustmentAmount;
                        }
                    }

                    serviceResponse.Data = edi837Model;
                }
                catch (Exception ex)
                {
                }
            }
            serviceResponse.IsSuccess = true;
            return serviceResponse;
        }





        public ServiceResponse HC_GetBatchList(SearchBatchList searchBatchList, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                HC_SetSearchFilterForBatchList(searchBatchList, searchList);
                Page<ListBatchModel> batchListPage = GetEntityPageList<ListBatchModel>(StoredProcedure.HC_GetBatchList,
                                                                    searchList, pageSize, pageIndex, sortIndex, sortDirection);
                response.IsSuccess = true;
                response.Data = batchListPage;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_GetLatestERAList(SearchERAList SearchERAList, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                HC_SetSearchFilterForERAList(SearchERAList, searchList);
                Page<ListLatestERAModel> batchListPage = GetEntityPageList<ListLatestERAModel>(StoredProcedure.HC_GetLatestERA,
                                                                    searchList, pageSize, pageIndex, sortIndex, sortDirection);
                response.IsSuccess = true;
                response.Data = batchListPage;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_DeleteBatch(SearchBatchList searchBatchList, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {

                //List<Batch> unsentBatches = GetEntityList<Batch>(StoredProcedure.HC_GetUnsentBatches, new List<SearchValueData>
                //{
                //    new SearchValueData
                //        {
                //           Name = "ListOfIdsInCSV",
                //           Value = searchBatchList.ListOfIdsInCSV
                //        }
                //});

                //foreach (var batchid in unsentBatches.Select(m => m.BatchID))
                //{

                //    string edifilesPath = string.Format("{0}{1}{2}{3}{4}", ConfigSettings.AmazoneUploadPath,
                //                                        ConfigSettings.EdiFilePath, ConfigSettings.EdiFileUploadPath,
                //                                        ConfigSettings.EdiFile837Path, batchid);

                //    AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                //    amazonFileUpload.DeleteAllObjectsInFolder(ConfigSettings.ZarephathBucket, edifilesPath);

                //    string edivalidationerrorfilesPath = string.Format("{0}{1}{2}{3}{4}", ConfigSettings.AmazoneUploadPath,
                //                                      ConfigSettings.EdiFilePath, ConfigSettings.TempEdiFileValidationErrorPath,
                //                                      ConfigSettings.EdiFile837Path, batchid);
                //    amazonFileUpload.DeleteAllObjectsInFolder(ConfigSettings.ZarephathBucket, edivalidationerrorfilesPath);
                //}



                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                HC_SetSearchFilterForBatchList(searchBatchList, searchList);

                if (!string.IsNullOrEmpty(searchBatchList.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchBatchList.ListOfIdsInCSV });
                //searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = "," + searchBatchList.ListOfIdsInCSV + "," });

                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

                List<ListBatchModel> totalData = GetEntityList<ListBatchModel>(StoredProcedure.HC_DeleteBatch, searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListBatchModel> listBatchLModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = listBatchLModel;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.Batch);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.DeleteFailed, Resource.Batch), Resource.ExceptionMessage);
            }
            return response;
        }


        private ServiceResponse HC_MarkAsSentBatch(long loggedInUserId, SearchBatchList searchBatchList)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchBatchList.ListOfIdsInCSV });
                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(false) });
                searchList.Add(new SearchValueData { Name = "MarkAsSentStatus", Value = Convert.ToInt16(EnumBatchNoteStatus.MarkAsSent).ToString() });
                searchList.Add(new SearchValueData { Name = "MarkAsUnSentStatus", Value = Convert.ToInt16(EnumBatchNoteStatus.MarkAsUnSent).ToString() });
                searchList.Add(new SearchValueData { Name = "IsSentBy", Value = Convert.ToString(loggedInUserId) });
                searchList.Add(new SearchValueData { Name = "IsSent", Value = Convert.ToString(searchBatchList.IsSent) });

                GetScalar(StoredProcedure.HC_SetMarkasSentBatch, searchList);
                response.IsSuccess = true;
                response.Message = Resource.BatchSentSuccessfully;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.ErrorOccured, Resource.ExceptionMessage);
            }
            return response;
        }


        public ServiceResponse HC_MarkAsSentBatch(long loggedInUserId, SearchBatchList searchBatchList, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                HC_SetSearchFilterForBatchList(searchBatchList, searchList);

                if (!string.IsNullOrEmpty(searchBatchList.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchBatchList.ListOfIdsInCSV });

                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                searchList.Add(new SearchValueData { Name = "MarkAsSentStatus", Value = Convert.ToInt16(EnumBatchNoteStatus.MarkAsSent).ToString() });
                searchList.Add(new SearchValueData { Name = "MarkAsUnSentStatus", Value = Convert.ToInt16(EnumBatchNoteStatus.MarkAsUnSent).ToString() });
                //searchList.Add(new SearchValueData { Name = "SentReason", Value = Convert.ToString(Constants.MarkAsSent) });
                //searchList.Add(new SearchValueData { Name = "UnSentReason", Value = Convert.ToString(Constants.MarkAsUnSent) });
                searchList.Add(new SearchValueData { Name = "IsSentBy", Value = Convert.ToString(loggedInUserId) });
                searchList.Add(new SearchValueData { Name = "IsSent", Value = Convert.ToString(searchBatchList.IsSent) });

                List<ListBatchModel> totalData = GetEntityList<ListBatchModel>(StoredProcedure.HC_SetMarkasSentBatch, searchList);
                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListBatchModel> listBatchLModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = listBatchLModel;
                response.IsSuccess = true;
                response.Message = Resource.BatchSentSuccessfully;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.ErrorOccured, Resource.ExceptionMessage);
            }
            return response;
        }

        private static void HC_SetSearchFilterForBatchList(SearchBatchList searchBatchList, List<SearchValueData> searchList)
        {

            if (!string.IsNullOrEmpty(searchBatchList.StrBatchID))
                searchList.Add(new SearchValueData { Name = "BatchID", Value = Convert.ToString(searchBatchList.StrBatchID) });

            if (searchBatchList.BatchTypeID != null)
                searchList.Add(new SearchValueData { Name = "BatchTypeID", Value = Convert.ToString(searchBatchList.BatchTypeID) });

            if (searchBatchList.PayorID > 0)
                searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchBatchList.PayorID) });

            if (searchBatchList.Comment != null)
                searchList.Add(new SearchValueData { Name = "Comment", Value = searchBatchList.Comment });

            if ((searchBatchList.StartDate.HasValue && searchBatchList.StartDate.Value.ToString(Constants.DefaultDateFormat) != Constants.DefaultDateValue))
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchBatchList.StartDate).ToString(Constants.DbDateFormat) });

            if ((searchBatchList.EndDate.HasValue && searchBatchList.EndDate.Value.ToString(Constants.DefaultDateFormat) != Constants.DefaultDateValue))
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchBatchList.EndDate).ToString(Constants.DbDateFormat) });

            searchList.Add(new SearchValueData { Name = "IsSentStatus", Value = Convert.ToString(searchBatchList.IsSentStatus) });

            searchList.Add(new SearchValueData { Name = "ClientName", Value = Convert.ToString(searchBatchList.ClientName) });


            if (!string.IsNullOrEmpty(searchBatchList.ReconcileStatus))
                searchList.Add(new SearchValueData { Name = "ReconcileStatus", Value = searchBatchList.ReconcileStatus });

            if (!string.IsNullOrEmpty(searchBatchList.ClaimAdjustmentTypeID))
                searchList.Add(new SearchValueData { Name = "ClaimAdjustmentTypeID", Value = searchBatchList.ClaimAdjustmentTypeID });
        }

        private static void HC_SetSearchFilterForERAList(SearchERAList searchERAList, List<SearchValueData> searchList)
        {
            if (searchERAList.LatestEraID != null)
                searchList.Add(new SearchValueData { Name = "LatestEraID", Value = Convert.ToString(searchERAList.LatestEraID) });

            if (searchERAList.PayorID > 0)
                searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchERAList.PayorID) });



            searchList.Add(new SearchValueData { Name = "CheckNumber", Value = Convert.ToString(searchERAList.CheckNumber) });
            searchList.Add(new SearchValueData { Name = "EraId", Value = Convert.ToString(searchERAList.EraId) });



            if ((searchERAList.PaidStartDate.HasValue && searchERAList.PaidStartDate.Value.ToString(Constants.DefaultDateFormat) != Constants.DefaultDateValue))
                searchList.Add(new SearchValueData { Name = "PaidStartDate", Value = Convert.ToDateTime(searchERAList.PaidStartDate).ToString(Constants.DbDateFormat) });

            if ((searchERAList.PaidEndDate.HasValue && searchERAList.PaidEndDate.Value.ToString(Constants.DefaultDateFormat) != Constants.DefaultDateValue))
                searchList.Add(new SearchValueData { Name = "PaidEndDate", Value = Convert.ToDateTime(searchERAList.PaidEndDate).ToString(Constants.DbDateFormat) });



            if ((searchERAList.ReceivedStartDate.HasValue && searchERAList.ReceivedStartDate.Value.ToString(Constants.DefaultDateFormat) != Constants.DefaultDateValue))
                searchList.Add(new SearchValueData { Name = "ReceivedStartDate", Value = Convert.ToDateTime(searchERAList.ReceivedStartDate).ToString(Constants.DbDateFormat) });

            if ((searchERAList.ReceivedEndDate.HasValue && searchERAList.ReceivedEndDate.Value.ToString(Constants.DefaultDateFormat) != Constants.DefaultDateValue))
                searchList.Add(new SearchValueData { Name = "ReceivedEndDate", Value = Convert.ToDateTime(searchERAList.ReceivedEndDate).ToString(Constants.DbDateFormat) });



        }

        public ServiceResponse HC_GenrateOverViewFile(string csvBatchId, long loogedInID)
        {
            var response = new ServiceResponse();

            #region Declare Path

            var batchOverviewPath = string.Format("batchOverviewPath/{0}/", loogedInID);
            //var batchOverviewPathZipPath = "batchOverviewPath/";

            #endregion

            DownloadFileModel downloadFileModel = new DownloadFileModel();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "BatchID", Value = Convert.ToString(csvBatchId) });

            List<HC_ListOverViewFileModel> listOverViewFileModel = GetEntityList<HC_ListOverViewFileModel>(StoredProcedure.HC_GetOverviewFileList, searchList);

            string basePath = HttpContext.Current.Server.MapCustomPath(String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain) + batchOverviewPath);



            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            string[] batchidnew = csvBatchId.Split(',');
            string batchIds = "";
            if (batchidnew.Count() > 1)
            {
                List<HC_ListOverViewFileModel> firstFile = new List<HC_ListOverViewFileModel>();
                List<List<HC_ListOverViewFileModel>> listOfOtherBatches = new List<List<HC_ListOverViewFileModel>>();
                for (int i = 0; i < batchidnew.Count(); i++)
                {
                    var listItem =
                        listOverViewFileModel.Where(k => k.BatchID == Convert.ToInt32(batchidnew[i])).ToList();

                    if (i == 0)
                    {
                        firstFile = listItem;
                        batchIds = batchidnew[i];
                        ;
                    }
                    else
                    {
                        listOfOtherBatches.Add(listItem);
                        batchIds = batchIds + "_" + batchidnew[i];
                    }
                    #region TO ZIP DOWNLOAD
                    //string fileName = string.Format("{0}_Batch_#{1}_{2}", Constants.OverViewFile, batchidnew[i],
                    //                               DateTime.Now.ToString(Constants.FileNameDateTimeFormat) + i);

                    //var absolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                    //CreateExcelFile.CreateExcelDocument(listItem, absolutePath);
                    #endregion
                }
                string fileName = string.Format("{0}_Batch_{1}_{2}", Constants.OverViewFile, batchIds,
                                                   DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                var absolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                CreateExcelFile.CreateExcelDocument(firstFile, absolutePath, listOfOtherBatches);
                downloadFileModel.VirtualPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain) + batchOverviewPath + fileName + Constants.Extention_xlsx;
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                downloadFileModel.AbsolutePath = "false";

                #region TO ZIP DOWNLOAD
                //string zipFile = HttpContext.Current.Server.MapCustomPath(_cacheHelper.ReportExcelFileUploadPath);
                //ZipFile.CreateFromDirectory(basePath, zipFile + batchOverviewPathZipPath + loogedInID + Constants.Extention_zip, CompressionLevel.NoCompression, false);
                //downloadFileModel.VirtualPath = zipFile + batchOverviewPathZipPath + loogedInID + Constants.Extention_zip;
                //downloadFileModel.FileName = loogedInID + Constants.Extention_zip;
                //downloadFileModel.AbsolutePath = "true";

                //DirectoryInfo di = new DirectoryInfo(basePath);
                //foreach (FileInfo file in di.GetFiles())
                //    file.Delete();
                //foreach (DirectoryInfo dir in di.GetDirectories())
                //    dir.Delete(true);
                #endregion

            }
            else
            {
                batchIds = csvBatchId;
                string fileName = string.Format("{0}_Batch_{1}_{2}", Constants.OverViewFile, batchIds, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                var absolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                CreateExcelFile.CreateExcelDocument(listOverViewFileModel, absolutePath);
                downloadFileModel.VirtualPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain) + batchOverviewPath + fileName + Constants.Extention_xlsx;
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                downloadFileModel.AbsolutePath = "false";
            }
            response.IsSuccess = true;
            response.Data = downloadFileModel;
            return response;
        }

        public ServiceResponse HC_GenratePaperRemitsEOBTemplate(string csvBatchId, long loogedInID)
        {
            var response = new ServiceResponse();

            #region Declare Path

            var paperRemitsEOBPath = string.Format("PaperRemitsEOBTemplate/{0}/", loogedInID);

            #endregion

            DownloadFileModel downloadFileModel = new DownloadFileModel();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "BatchID", Value = Convert.ToString(csvBatchId) });

            List<ListPaperRemitModel> listModel = GetEntityList<ListPaperRemitModel>(StoredProcedure.HC_GenratePaperRemitsEOBTemplate, searchList);

            string basePath = HttpContext.Current.Server.MapCustomPath(String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain) + paperRemitsEOBPath);

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            string fileName = string.Format("{0}_Batch_{1}", Constants.PaperRemitsEOBTemplate, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
            var absolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_csv);
            //CreateExcelFile.CreateExcelDocument(listModel, absolutePath);
            CreateExcelFile.CreateCsvFromList(listModel, absolutePath);
            downloadFileModel.VirtualPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain) + paperRemitsEOBPath + fileName + Constants.Extention_csv;
            downloadFileModel.FileName = fileName + Constants.Extention_csv;
            downloadFileModel.AbsolutePath = "false";

            response.IsSuccess = true;
            response.Data = downloadFileModel;
            return response;
        }





        public ServiceResponse HC_DeleteNote_Temporary(long noteID, bool permanentDelete, long loggedInID)
        {
            var response = new ServiceResponse();
            try
            {

                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "NoteID", Value = Convert.ToString(noteID) });
                searchList.Add(new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(loggedInID) });
                searchList.Add(new SearchValueData { Name = "PermanentDelete", Value = Convert.ToString(permanentDelete) });
                searchList.Add(new SearchValueData { Name = "IsCaseManagement", Value = Convert.ToString(SessionHelper.IsCaseManagement) });
                GetScalar(StoredProcedure.HC_DeleteNote_Temporary, searchList);
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.ClaimId);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }





        public ServiceResponse HC_ValidateTemporaryNotes(string patientIds, long payorId, long batchTypeId, long loggedInUserId, bool isCaseManagement = false)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            var searchList = new List<SearchValueData>()
                {
                    new SearchValueData { Name = "LoggedInUserId", Value = Convert.ToString(loggedInUserId) },
                    new SearchValueData { Name = "IsCaseManagement", Value = Convert.ToString(isCaseManagement) },
                    new SearchValueData {Name = "ReferralsIds", Value =patientIds},
                    new SearchValueData {Name = "PayorID", Value =Convert.ToString(payorId)},
                    new SearchValueData {Name = "BatchTypeID", Value =Convert.ToString(batchTypeId)},
                };

            try
            {
                ParentBatchRelatedAllDataModel model = GetMultipleEntity<ParentBatchRelatedAllDataModel>(StoredProcedure.HC_GetBatchRelatedAllData_Temporary, searchList);



                ParentBatchRelatedAllDataModel_Temporary temp = new ParentBatchRelatedAllDataModel_Temporary();
                temp.PayorEdi837Setting = model.PayorEdi837Setting;
                temp.BatchRelatedAllDataModel_Temporary = new List<BatchRelatedAllDataModel_Temporary>();

                foreach (BatchRelatedAllDataModel item in model.BatchRelatedAllDataModel)
                {
                    if (isCaseManagement)
                    {
                        item.IsCaseManagement = true;
                    }
                    else if (SessionHelper.IsHomeCare)
                    {
                        model.BatchRelatedAllDataModel.ForEach(c => c.IsHomeCare = true);
                    }
                    else if (SessionHelper.IsDayCare)
                    {
                        model.BatchRelatedAllDataModel.ForEach(c => c.IsDayCare = true);
                    }
                    temp.BatchRelatedAllDataModel_Temporary.Add(new BatchRelatedAllDataModel_Temporary() { BatchRelatedDataModel = item });
                }




                if ((temp.BatchRelatedAllDataModel_Temporary == null || temp.BatchRelatedAllDataModel_Temporary.Count == 0) &&
                    (temp.PayorEdi837Setting == null || temp.PayorEdi837Setting.PayorEdi837SettingId == 0))
                {
                    serviceResponse.ErrorCode = "Failed";
                    serviceResponse.Message = "No claims are found, please check.";
                    return serviceResponse;
                }
                if (temp.PayorEdi837Setting == null || temp.PayorEdi837Setting.PayorEdi837SettingId == 0)
                {
                    serviceResponse.ErrorCode = "Failed";
                    serviceResponse.Message = "Payor Edi837 Settings are missing.";
                    return serviceResponse;
                }


                bool IsValdiationPassed = true;
                EdiFileType ediFileType = model.FileName == Common.PayorEDIFileType.Professional.ToString() ? EdiFileType.Edi_837_P : EdiFileType.Edi_837_I;

                Common_Edi_837.ValidateTemporaryNotes(ediFileType, ref temp, ref IsValdiationPassed);


                if (!IsValdiationPassed)
                {
                    serviceResponse.Data = temp.BatchRelatedAllDataModel_Temporary.Where(c => c.ErrorCount > 0).ToList();
                    serviceResponse.Message = "Issue(s) with claim(s). Please correct to proceed.";
                    serviceResponse.ErrorCode = "Failed";
                }
                else
                {
                    serviceResponse.ErrorCode = "Success";
                }



            }
            catch (Exception ex)
            {
                serviceResponse.IsSuccess = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.ErrorCode = "Failed";
            }

            return serviceResponse;
        }





        #endregion Add Batch /List Batch

        #region Batch Validation And 837 Generation
        public ServiceResponse HC_ValidateBatchesAndGenerateEdi837Files(PostEdiValidateGenerateModel postEdiValidateGenerateModel, long loggedInUserId, bool isCaseManagement = false)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            Edi837 edi837 = new Edi837();
            string filePath = String.Format(_cacheHelper.EdiFile837UploadPath, _cacheHelper.Domain);

            List<string> batchIds = string.IsNullOrEmpty(postEdiValidateGenerateModel.ListOfBacthIdsInCsv) ? new List<string>()
                : postEdiValidateGenerateModel.ListOfBacthIdsInCsv.Split(',').ToList();

            List<BatchValidationResponseModel> listBrm = new List<BatchValidationResponseModel>();
            foreach (var batchId in batchIds)
            {
                string initFName = "PY_05012017_05152017_IS";
                filePath = String.Format(_cacheHelper.EdiFile837UploadPath, _cacheHelper.Domain);
                filePath = string.Format("{0}{1}/", filePath, batchId);
                filePath = string.Format("{0}{1}/", filePath, DateTime.Now.ToString("yyyyMMddHHmmss"));

                string tempFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");// +"_" + Guid.NewGuid().ToString();

                BatchValidationResponseModel batchValidationResponseModel = new BatchValidationResponseModel();
                batchValidationResponseModel.BatchID = Convert.ToInt64(batchId);

                #region DO validation

                var searchList = new List<SearchValueData>()
                {
                    new SearchValueData { Name = "BatchID", Value = Convert.ToString(batchId) },
                    new SearchValueData { Name = "FileType", Value = postEdiValidateGenerateModel.EdiFileType == EdiFileType.Edi_837_P ? "CMS1500" : "UB04" },
                    new SearchValueData { Name = "IsCaseManagement",Value =  Convert.ToString(isCaseManagement)}
                };
                ParentBatchRelatedAllDataModel model = null;
                string ediFile837ValidationErrorPath = string.Format(_cacheHelper.EdiFile837ValidationErrorPath, _cacheHelper.Domain);
                try
                {
                    model = GetMultipleEntity<ParentBatchRelatedAllDataModel>(StoredProcedure.HC_GetBatchRelatedAllData, searchList);
                    tempFileName = string.Format("{0}", model.FileName, tempFileName);

                    if (isCaseManagement)
                    {
                        model.BatchRelatedAllDataModel.ForEach(c => c.IsCaseManagement = true);
                    }
                    else if (SessionHelper.IsHomeCare)
                    {
                        model.BatchRelatedAllDataModel.ForEach(c => c.IsHomeCare = true);
                    }
                    else if (SessionHelper.IsDayCare)
                    {
                        model.BatchRelatedAllDataModel.ForEach(c => c.IsDayCare = true);
                    }

                    Common_Edi_837.CheckAndGenerateBatchValidationErrorCsv(model, ref batchValidationResponseModel, postEdiValidateGenerateModel.EdiFileType, ediFile837ValidationErrorPath);
                    //model.BatchRelatedAllDataModel = model.BatchRelatedAllDataModel.Where(c => c.IsUseInBilling).ToList();
                }
                catch (Exception ex)
                {
                    string fname = string.Format("edi837_validation_exce_error_{0}.txt", tempFileName);
                    Common_Edi_837.GenerationBatchExceptionMsg(ref batchValidationResponseModel, (int)EnumEdiFileTypes.Edi837GenerationError, fname, ex, null, ediFile837ValidationErrorPath);
                }
                #endregion

                string fileName = tempFileName + postEdiValidateGenerateModel.FileExtension;
                batchValidationResponseModel.FileName = fileName;
                if (batchValidationResponseModel.ValidationPassed && postEdiValidateGenerateModel.GenerateEdiFile && model != null)
                {
                    try
                    {
                        PayorEdi837Setting payorEdi837Setting = model.PayorEdi837Setting;
                        Edi837Model edi837Model = Common_Edi_837.GetEdit837Model(Convert.ToInt64(batchId), ref payorEdi837Setting, model.BatchRelatedAllDataModel, postEdiValidateGenerateModel.EdiFileType);

                        string fileServerPath = HttpContext.Current.Server.MapCustomPath(filePath);
                        string generatedFilePath = edi837.HC_GenerateEdi837File(edi837Model, fileServerPath, fileName, postEdiValidateGenerateModel.EdiFileType);

                        SaveEntity(payorEdi837Setting);

                        //string fullpath = HttpContext.Current.Server.MapCustomPath(string.Format("{0}{1}", filePath, fileName));
                        //Common.DeleteFile(fullpath);
                        batchValidationResponseModel.FileName = fileName;
                        batchValidationResponseModel.EdiFileTypeID = (int)EnumEdiFileTypes.Edi837;
                        batchValidationResponseModel.Edi837FilePath = string.Format("{0}{1}", filePath, fileName);
                        batchValidationResponseModel.Edi837GenerationPassed = true;
                    }
                    catch (Exception ex)
                    {
                        string fname = string.Format("edi837_generate_exce_error_{0}.txt", tempFileName);
                        Common_Edi_837.GenerationBatchExceptionMsg(ref batchValidationResponseModel, (int)EnumEdiFileTypes.Edi837GenerationError, fname, ex, null, ediFile837ValidationErrorPath);
                        //string message = string.Format("{1}{0}{2}{0}{3}", Environment.NewLine, ex.Message, ex.StackTrace, ex.Source);
                        //string fname = string.Format("edi837_generate_exce_error_{0}_", tempFileName);
                        //string errofilePath = Common.CreateLogFile(message, fname, _cacheHelper.EdiFile837ValidationErrorPath);
                        //batchValidationResponseModel.FileName = string.Format("{0}.txt", fname);
                        //batchValidationResponseModel.EdiFileTypeID = (int)EnumEdiFileTypes.Edi837GenerationError;
                        //batchValidationResponseModel.Edi837FilePath = errofilePath;
                    }
                }

                if (batchValidationResponseModel.EdiFileTypeID > 0)
                {
                    EdiFileLog ediFileLog = new EdiFileLog();
                    ediFileLog.BatchID = batchValidationResponseModel.BatchID;
                    ediFileLog.FileName = batchValidationResponseModel.FileName;
                    ediFileLog.EdiFileTypeID = batchValidationResponseModel.EdiFileTypeID;
                    ediFileLog.FilePath = batchValidationResponseModel.ValidationPassed ? batchValidationResponseModel.Edi837FilePath : batchValidationResponseModel.ValidationErrorFilePath;
                    ediFileLog.FileSize = Common.GetFileSizeInBytes(HttpContext.Current.Server.MapCustomPath(ediFileLog.FilePath)).ToString();

                    #region amazonefileupload
                    if (ConfigSettings.IsUploadOnCloudServer)
                    {
                        AmazonFileUpload amazoneFileUpload = new AmazonFileUpload();
                        string fullpath = HttpContext.Current.Server.MapCustomPath(ediFileLog.FilePath);
                        ediFileLog.FilePath = amazoneFileUpload.UploadFile(ConfigSettings.ZarephathBucket, ediFileLog.FilePath.TrimStart('/'), fullpath, true);
                        if (batchValidationResponseModel.ValidationPassed)
                        {
                            batchValidationResponseModel.Edi837FilePath =
                                amazoneFileUpload.GetPreSignedUrl(ConfigSettings.ZarephathBucket, ediFileLog.FilePath);
                        }
                        else
                        {
                            batchValidationResponseModel.ValidationErrorFilePath =
                                amazoneFileUpload.GetPreSignedUrl(ConfigSettings.ZarephathBucket, ediFileLog.FilePath);
                        }
                    }
                    #endregion amazonefileupload

                    SaveObject(ediFileLog, loggedInUserId);
                }
                listBrm.Add(batchValidationResponseModel);
            }


            serviceResponse.IsSuccess = true;
            serviceResponse.Data = listBrm;
            return serviceResponse;
        }
        #endregion


        #region Upload 835 And Processing

        public ServiceResponse HC_SetUpload835Page()
        {
            var response = new ServiceResponse();
            AddUpload835Model addUpload835Model = GetMultipleEntity<AddUpload835Model>(StoredProcedure.HC_GetSetUpload835);
            addUpload835Model.FileProcessStatus = Common.SetUpload835FileProcessStatusFilter();
            addUpload835Model.A835TemplateType = Enum835TemplateType.Edi_File.ToString();
            addUpload835Model.SearchUpload835ListPage.Upload835FileProcessStatus = -1;
            addUpload835Model.SearchUpload835ListPage.A835TemplateType = "-1";
            response.Data = addUpload835Model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_SaveUpload835File(AddUpload835Model model, HttpRequestBase httpRequestBase, long loggedInUserID)
        {
            var response = new ServiceResponse();
            string basePath = String.Format(_cacheHelper.EdiFile835DownLoadPath, _cacheHelper.Domain) + model.PayorID + "/";

            #region OLD CODE (Single Upload)
            //HttpPostedFileBase file = httpRequestBase.Files[0];
            //response = Common.SaveFile(file, basePath);

            //if (response.IsSuccess)
            //{

            //    Upload835File upload835File = new Upload835File();
            //    upload835File.PayorID = model.PayorID;
            //    upload835File.FileName = ((UploadedFileModel)response.Data).FileOriginalName;
            //    upload835File.FilePath = ((UploadedFileModel)response.Data).TempFilePath;
            //    upload835File.Comment = model.Comment == "null" ? null : model.Comment;
            //    upload835File.IsProcessed = false;
            //    upload835File.Upload835FileProcessStatus = (int)EnumUpload835FileProcessStatus.UnProcessed;
            //    upload835File.A835TemplateType = model.A835TemplateType;
            //    upload835File.FileSize = Common.GetFileSizeInBytes(HttpContext.Current.Server.MapCustomPath(upload835File.FilePath)).ToString();


            //    //#region amazonefileupload
            //    //AmazonFileUpload amazoneFileUpload = new AmazonFileUpload();
            //    //string fullpath = HttpContext.Current.Server.MapCustomPath(upload835File.FilePath);
            //    //upload835File.FilePath = amazoneFileUpload.UploadFile(ConfigSettings.ZarephathBucket, upload835File.FilePath.TrimStart('/'), fullpath, true);
            //    //#endregion amazonefileupload

            //    SaveObject(upload835File, loggedInUserID);
            //    response.Message = Resource._835FileUploaded;
            //}
            #endregion

            try
            {
                for (int i = 0; i < httpRequestBase.Files.Count; i++)
                {
                    HttpPostedFileBase file = httpRequestBase.Files[i];
                    var fileResponse = Common.SaveFile(file, basePath);

                    if (fileResponse.IsSuccess)
                    {

                        Upload835File upload835File = new Upload835File();
                        upload835File.PayorID = model.PayorID;
                        upload835File.FileName = ((UploadedFileModel)fileResponse.Data).FileOriginalName;
                        upload835File.FilePath = ((UploadedFileModel)fileResponse.Data).TempFilePath;
                        upload835File.Comment = model.Comment == "null" ? null : model.Comment;
                        upload835File.IsProcessed = false;
                        upload835File.Upload835FileProcessStatus = (int)EnumUpload835FileProcessStatus.UnProcessed;
                        upload835File.A835TemplateType = model.A835TemplateType;
                        upload835File.FileSize = Common.GetFileSizeInBytes(HttpContext.Current.Server.MapCustomPath(upload835File.FilePath)).ToString();


                        //#region amazonefileupload
                        //AmazonFileUpload amazoneFileUpload = new AmazonFileUpload();
                        //string fullpath = HttpContext.Current.Server.MapCustomPath(upload835File.FilePath);
                        //upload835File.FilePath = amazoneFileUpload.UploadFile(ConfigSettings.ZarephathBucket, upload835File.FilePath.TrimStart('/'), fullpath, true);
                        //#endregion amazonefileupload

                        SaveObject(upload835File, loggedInUserID);
                    }
                }
                response.IsSuccess = true;
                response.Message = Resource._835FileUploaded;
            }
            catch
            {
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        #endregion







        #endregion

        #region Upload 835 List

        public ServiceResponse HC_GetUpload835FileList(SearchUpload835ListPage searchUpload835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                if (searchUpload835Model != null)
                    HC_SetSearchFilterForUpload835ListPage(searchUpload835Model, searchList);
                Page<ListUpload835Model> listUpload835Model = GetEntityPageList<ListUpload835Model>(StoredProcedure.HC_GetUpload835FileList, searchList, pageSize,
                                                              pageIndex, sortIndex, sortDirection);
                response.Data = listUpload835Model;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.EdiFileLog), Resource.ExceptionMessage);
            }
            return response;
        }

        private static void HC_SetSearchFilterForUpload835ListPage(SearchUpload835ListPage searchUpload835ListPage, List<SearchValueData> searchList)
        {
            if (searchUpload835ListPage.Upload835FileID > 0)
                searchList.Add(new SearchValueData { Name = "Upload835FileID", Value = Convert.ToString(searchUpload835ListPage.Upload835FileID) });

            if (searchUpload835ListPage.PayorID > 0)
                searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchUpload835ListPage.PayorID) });

            if (searchUpload835ListPage.A835TemplateType != null)
                searchList.Add(new SearchValueData { Name = "A835TemplateType", Value = Convert.ToString(searchUpload835ListPage.A835TemplateType) });


            if (searchUpload835ListPage.Comment != null)
                searchList.Add(new SearchValueData { Name = "Comment", Value = Convert.ToString(searchUpload835ListPage.Comment) });

            if (searchUpload835ListPage.FileName != null)
                searchList.Add(new SearchValueData { Name = "FileName", Value = Convert.ToString(searchUpload835ListPage.FileName) });

            searchList.Add(new SearchValueData { Name = "Upload835FileProcessStatus", Value = Convert.ToString(searchUpload835ListPage.Upload835FileProcessStatus) });

        }

        public ServiceResponse HC_DeleteUpload835File(SearchUpload835ListPage searchUpload835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                #region Delete File from folder

                List<SearchValueData> deletesearchList = new List<SearchValueData>();
                if (!string.IsNullOrEmpty(searchUpload835Model.ListOfIdsInCSV))
                    deletesearchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchUpload835Model.ListOfIdsInCSV });
                deletesearchList.Add(new SearchValueData { Name = "UnProcess", Value = Convert.ToString(Convert.ToInt32(EnumUpload835FileProcessStatus.UnProcessed)) });

                List<ListUpload835Model> listEdiFilesLogModel = GetEntityList<ListUpload835Model>(StoredProcedure.HC_GetDeleteUpload835FilePathList, deletesearchList);

                if (listEdiFilesLogModel != null)
                {
                    foreach (var model in listEdiFilesLogModel)
                    {
                        if (model.BatchID == null && !model.IsProcessed)
                        {
                            string filePath = HttpContext.Current.Server.MapCustomPath(model.FilePath);
                            if (File.Exists(filePath))
                                File.Delete(filePath);

                            //AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                            //amazonFileUpload.DeleteFile(ConfigSettings.ZarephathBucket, model.FilePath);
                        }
                    }
                }

                #endregion

                #region Delete Record from DataBase


                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                SetSearchFilterForUpload835ListPage(searchUpload835Model, searchList);

                if (!string.IsNullOrEmpty(searchUpload835Model.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchUpload835Model.ListOfIdsInCSV });
                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                searchList.Add(new SearchValueData { Name = "UnProcess", Value = Convert.ToString(Convert.ToInt32(EnumUpload835FileProcessStatus.UnProcessed)) });

                List<ListUpload835Model> totalData = GetEntityList<ListUpload835Model>(StoredProcedure.HC_DeleteUpload835File, searchList);
                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListUpload835Model> getEdiFilesLogList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);

                #endregion

                response.Data = getEdiFilesLogList;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.Upload835File);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailedTitle, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_ProcessUpload835File(SearchUpload835ListPage searchUpload835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {

                #region Update IN DB
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                SetSearchFilterForUpload835ListPage(searchUpload835Model, searchList);
                if (!string.IsNullOrEmpty(searchUpload835Model.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchUpload835Model.ListOfIdsInCSV });
                searchList.Add(new SearchValueData { Name = "Upload835FileStatus", Value = ((int)EnumUpload835FileProcessStatus.InProcess).ToString() });
                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                List<ListUpload835Model> totalData = GetEntityList<ListUpload835Model>(StoredProcedure.HC_ProcessUpload835File, searchList);


                #endregion

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListUpload835Model> getEdiFilesLogList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);



                response.Data = getEdiFilesLogList;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordInProcess, Resource.Upload835File);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;// Common.MessageWithTitle(Resource.ProcessFailedTitle, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_SaveUpload835Comment(Upload835CommentModel upload835CommentModel, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();

            Upload835File upload835File = GetEntity<Upload835File>(upload835CommentModel.Upload835FileID);
            if (upload835File != null)
            {
                upload835File.Comment = upload835CommentModel.Comment;
                SaveObject(upload835File, loggedInId);

                response.IsSuccess = true;
                response.Message = Resource.CommentSavedSuccessfully;
            }
            else
            {
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }
        #endregion

        #region Cron JOB TO Process Queued 835 Files
        public ServiceResponse HC_BackEndProcessUpload835File()
        {
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.Edi835FileLog, Common.GenerateRandomNumber() + "/");
            if (!ConfigSettings.Service_Edi835FileProcess_ON)
            {
                Common.CreateLogFile("Edi 835 File Process CronJob can't run because it is mark as offline from web config.", ConfigSettings.Edi835FileName, logpath);
                return null;
            }

            ServiceResponse response = new ServiceResponse();
            try
            {
                Common.CreateLogFile("Process Edi835Files CronJob Started.", ConfigSettings.Edi835FileName, logpath);
                #region Process 835 Files
                //List<SearchValueData> searchList = new List<SearchValueData>();
                //searchList.Add(new SearchValueData { Name = "Upload835FileProcessStatus", Value = ((int)EnumUpload835FileProcessStatus.InProcess).ToString() });
                //List<Upload835File> upload835FileList = GetEntityList<Upload835File>(searchList);


                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "GetStatus", Value = ((int)EnumUpload835FileProcessStatus.InProcess).ToString() });
                searchList.Add(new SearchValueData { Name = "SetStatus", Value = ((int)EnumUpload835FileProcessStatus.Running).ToString() });
                List<Upload835File> upload835FileList = GetEntityList<Upload835File>(StoredProcedure.HC_GetUpload835FilesForProcess, searchList);





                foreach (var upload835File in upload835FileList)
                {
                    Edi835 edi835 = new Edi835();
                    //string ediFilePath = HttpContext.Current.Server.MapCustomPath("/" + upload835File.FilePath);
                    string ediFilePath = Common.HttpContext_Current_Server_MapPath("/" + upload835File.FilePath);



                    //AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                    //amazonFileUpload.DownloadFile(ConfigSettings.ZarephathBucket, upload835File.FilePath, ediFilePath);

                    string ediFile835CsvDownLoadPath = String.Format(_cacheHelper.EdiFile835CsvDownLoadPath, _cacheHelper.Domain);
                    string newRedablefilePath = string.Format("{0}{1}/{2}{3}", ediFile835CsvDownLoadPath, upload835File.PayorID, Guid.NewGuid().ToString(), Constants.FileExtension_Csv);

                    try
                    {

                        Edi835ResponseModel edi835ResponseModel = upload835File.A835TemplateType == Enum835TemplateType.Edi_File.ToString() ?
                            //edi835.GenerateEdi835Model(ediFilePath, HttpContext.Current.Server.MapCustomPath(newRedablefilePath), newRedablefilePath) :
                            edi835.GenerateEdi835Model(ediFilePath, Common.HttpContext_Current_Server_MapPath(newRedablefilePath), newRedablefilePath) :
                            HC_GenerateEdi835ModelFromPaperRemits(ediFilePath, ediFilePath, upload835File.FilePath);


                        List<long> batchIDListForERAUpdate = new List<long>();

                        foreach (var edi835Model in edi835ResponseModel.Edi835ModelList)
                        {
                            //long batchId = Convert.ToInt64(edi835Model.BatchID);
                            //long noteId = Convert.ToInt64(edi835Model.NoteID);

                            string batchID = edi835Model.BatchNoteID;

                            long batchNoteId = 0;
                            long batchNoteId02 = 0;


                            List<SearchValueData> searchParam = new List<SearchValueData>();
                            searchParam.Add(new SearchValueData() { Name = "ServiceCode", Value = edi835Model.SVC01_02_ServiceCode });
                            searchParam.Add(new SearchValueData() { Name = "ServiceStartDate", Value = edi835Model.DTM02_ServiceStartDate });
                            searchParam.Add(new SearchValueData() { Name = "ServiceEndDate", Value = edi835Model.DTM02_ServiceEndDate });
                            searchParam.Add(new SearchValueData() { Name = "ServiceCode_Mod_01", Value = edi835Model.SVC01_02_ServiceCode_Mod_01 });
                            searchParam.Add(new SearchValueData() { Name = "ServiceCode_Mod_02", Value = edi835Model.SVC01_02_ServiceCode_Mod_02 });
                            searchParam.Add(new SearchValueData() { Name = "ServiceCode_Mod_03", Value = edi835Model.SVC01_02_ServiceCode_Mod_03 });
                            searchParam.Add(new SearchValueData() { Name = "ServiceCode_Mod_04", Value = edi835Model.SVC01_02_ServiceCode_Mod_04 });
                            
                            searchParam.Add(new SearchValueData() { Name = "ClientHIC", Value = edi835Model.NM109_PatientIdentifier });
                            searchParam.Add(new SearchValueData() { Name = "ClientFirstName", Value = edi835Model.NM104_PatientFirstName });
                            searchParam.Add(new SearchValueData() { Name = "ClientLastName", Value = edi835Model.NM103_PatientLastName });


                            if (!string.IsNullOrEmpty(edi835Model.CLP01_ClaimSubmitterIdentifier) && edi835Model.CLP01_ClaimSubmitterIdentifier.Contains('B'))
                            {
                                string referralID = edi835Model.CLP01_ClaimSubmitterIdentifier.Split('B')[0];
                                batchID = edi835Model.CLP01_ClaimSubmitterIdentifier.Split('B')[1];
                                searchParam.Add(new SearchValueData() { Name = "ClientReferralID", Value = Convert.ToString(referralID).Trim() });
                                searchParam.Add(new SearchValueData() { Name = "BatchID", Value = Convert.ToString(batchID).Trim() });
                            }
                            



                            
                            batchNoteId = Convert.ToInt64(GetScalar(StoredProcedure.HC_GetBatchNoteDetailsBasedOnServiceDetails, searchParam));


                            if (batchNoteId > 0)
                            {
                                searchList = new List<SearchValueData>();
                                //searchList.Add(new SearchValueData { Name = "BatchID", Value = batchId.ToString() });
                                //searchList.Add(new SearchValueData { Name = "NoteID", Value = noteId.ToString() });
                                searchList.Add(new SearchValueData { Name = "BatchNoteID", Value = batchNoteId.ToString() });
                                string custWhere = "";//"ParentBatchNoteID IS NULL";
                                BatchNote tempBatchNote = GetEntity<BatchNote>(searchList, custWhere);

                                BatchNote subBatchNote = new BatchNote();
                                if (tempBatchNote != null && (tempBatchNote.ParentBatchNoteID == null || tempBatchNote.ParentBatchNoteID == 0) && string.IsNullOrEmpty(tempBatchNote.CLP02_ClaimStatusCode))
                                    subBatchNote = tempBatchNote;

                                if (tempBatchNote != null)
                                {
                                    subBatchNote.BatchID = tempBatchNote.BatchID;
                                    subBatchNote.NoteID = tempBatchNote.NoteID;
                                    subBatchNote.CLM_BilledAmount = tempBatchNote.CLM_BilledAmount;//   edi835Model.SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount;
                                    subBatchNote.CLM_UNIT = tempBatchNote.CLM_UNIT;
                                    subBatchNote.Original_Amount = tempBatchNote.Original_Amount;
                                    subBatchNote.Original_Unit = tempBatchNote.Original_Unit;
                                    subBatchNote.IsUseInBilling = tempBatchNote.IsUseInBilling;
                                    subBatchNote.IsNewProcess = tempBatchNote.IsNewProcess;
                                    subBatchNote.Submitted_ClaimAdjustmentTypeID = tempBatchNote.Submitted_ClaimAdjustmentTypeID;
                                    subBatchNote.Original_ClaimSubmitterIdentifier = tempBatchNote.Original_ClaimSubmitterIdentifier;
                                    subBatchNote.Original_PayerClaimControlNumber = tempBatchNote.Original_PayerClaimControlNumber;

                                    //subBatchNote.BatchNoteStatusID = null;
                                    subBatchNote.ParentBatchNoteID = batchNoteId;

                                    subBatchNote.N102_PayorName = edi835Model.N102_PayorName;
                                    subBatchNote.PER02_PayorBusinessContactName = edi835Model.PER02_PayorBusinessContactName;
                                    subBatchNote.PER04_PayorBusinessContact = edi835Model.PER04_PayorBusinessContact;
                                    subBatchNote.PER02_PayorTechnicalContactName = edi835Model.PER02_PayorTechnicalContactName;
                                    subBatchNote.PER04_PayorTechnicalContact = edi835Model.PER04_PayorTechnicalContact;
                                    subBatchNote.PER06_PayorTechnicalEmail = edi835Model.PER06_PayorTechnicalEmail;
                                    subBatchNote.PER04_PayorTechnicalContactUrl = edi835Model.PER04_PayorTechnicalContactUrl;
                                    subBatchNote.N102_PayeeName = edi835Model.N102_PayeeName;
                                    subBatchNote.N103_PayeeIdentificationQualifier = edi835Model.N103_PayeeIdentificationQualifier;
                                    subBatchNote.N104_PayeeIdentification = edi835Model.N104_PayeeIdentification;

                                    subBatchNote.LX01_ClaimSequenceNumber = edi835Model.LX01_ClaimSequenceNumber;
                                    subBatchNote.CLP02_ClaimStatusCode = edi835Model.CLP02_ClaimStatusCode;
                                    subBatchNote.CLP01_ClaimSubmitterIdentifier = edi835Model.CLP01_ClaimSubmitterIdentifier;
                                    subBatchNote.CLP03_TotalClaimChargeAmount = edi835Model.CLP03_TotalClaimChargeAmount;
                                    subBatchNote.CLP04_TotalClaimPaymentAmount = edi835Model.CLP04_TotalClaimPaymentAmount;
                                    subBatchNote.CLP05_PatientResponsibilityAmount = edi835Model.CLP05_PatientResponsibilityAmount;
                                    subBatchNote.CLP07_PayerClaimControlNumber = edi835Model.CLP07_PayerClaimControlNumber;
                                    subBatchNote.CLP08_PlaceOfService = edi835Model.CLP08_PlaceOfService;

                                    subBatchNote.NM103_PatientLastName = edi835Model.NM103_PatientLastName;
                                    subBatchNote.NM104_PatientFirstName = edi835Model.NM104_PatientFirstName;
                                    subBatchNote.NM109_PatientIdentifier = edi835Model.NM109_PatientIdentifier;
                                    subBatchNote.NM103_ServiceProviderName = edi835Model.NM103_ServiceProviderName;
                                    subBatchNote.NM109_ServiceProviderNpi = edi835Model.NM109_ServiceProviderNpi;

                                    subBatchNote.SVC01_01_ServiceCodeQualifier = edi835Model.SVC01_01_ServiceCodeQualifier;
                                    subBatchNote.SVC01_02_ServiceCode = edi835Model.SVC01_02_ServiceCode;
                                    subBatchNote.SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount = edi835Model.SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount;
                                    subBatchNote.SVC03_LineItemProviderPaymentAmoun_PaidAmount = edi835Model.SVC03_LineItemProviderPaymentAmoun_PaidAmount;
                                    subBatchNote.SVC05_ServiceCodeUnit = edi835Model.SVC05_ServiceCodeUnit;
                                    subBatchNote.DTM02_ServiceStartDate = edi835Model.DTM02_ServiceStartDate;
                                    subBatchNote.DTM02_ServiceEndDate = edi835Model.DTM02_ServiceEndDate;
                                    subBatchNote.CAS01_ClaimAdjustmentGroupCode = edi835Model.CAS01_ClaimAdjustmentGroupCode;
                                    subBatchNote.CAS02_ClaimAdjustmentReasonCode = edi835Model.CAS02_ClaimAdjustmentReasonCode;
                                    subBatchNote.CAS03_ClaimAdjustmentAmount = edi835Model.CAS03_ClaimAdjustmentAmount;
                                    subBatchNote.REF02_LineItem_ReferenceIdentification = edi835Model.REF02_LineItem_ReferenceIdentification;
                                    subBatchNote.AMT01_ServiceLineAllowedAmount_AllowedAmount = edi835Model.AMT01_ServiceLineAllowedAmount_AllowedAmount;

                                    subBatchNote.CheckDate = edi835Model.CheckDate;
                                    subBatchNote.CheckAmount = edi835Model.CheckAmount;
                                    subBatchNote.CheckNumber = edi835Model.CheckNumber;
                                    subBatchNote.PolicyNumber = edi835Model.PolicyNumber;
                                    subBatchNote.AccountNumber = edi835Model.AccountNumber;
                                    subBatchNote.ICN = edi835Model.ICN;
                                    //subBatchNote.BilledAmount = edi835Model.BilledAmount;
                                    //subBatchNote.AllowedAmount = edi835Model.AllowedAmount;
                                    //subBatchNote.PaidAmount = edi835Model.PaidAmount;
                                    subBatchNote.Deductible = edi835Model.Deductible;
                                    subBatchNote.Coins = edi835Model.Coins;
                                    subBatchNote.ProcessedDate = string.IsNullOrEmpty(edi835Model.ProcessedDate) ? DateTime.Now.Date : Convert.ToDateTime(edi835Model.ProcessedDate);
                                    subBatchNote.ReceivedDate = upload835File.CreatedDate;
                                    subBatchNote.LoadDate = DateTime.Now;

                                    subBatchNote.Upload835FileID = upload835File.Upload835FileID;
                                    subBatchNote.S277 = tempBatchNote.S277;
                                    subBatchNote.S277CA = tempBatchNote.S277CA;
                                    SaveEntity(subBatchNote);

                                    if (!batchIDListForERAUpdate.Any(c => c == tempBatchNote.BatchID))
                                        batchIDListForERAUpdate.Add(tempBatchNote.BatchID);

                                    MarkNoteAsLatest(subBatchNote.BatchNoteID, subBatchNote.BatchID, subBatchNote.NoteID, upload835File.Upload835FileID);

                                }

                            }

                        }


                        if (batchIDListForERAUpdate.Count > 0)
                            upload835File.EraMappedBatches = String.Join(",", batchIDListForERAUpdate.Select(x => x.ToString()).ToArray());

                        if (upload835File.A835TemplateType == Enum835TemplateType.Edi_File.ToString())
                            upload835File.ReadableFilePath = edi835ResponseModel.GeneratedFileRelativePath;
                        else
                            upload835File.ReadableFilePath = upload835File.FilePath;

                        upload835File.Upload835FileProcessStatus = (int)EnumUpload835FileProcessStatus.Processed;
                        SaveEntity(upload835File);




                        #region Batch Updated With ERA ID
                        HC_UpdateBatchWithERAReference(batchIDListForERAUpdate, upload835File.EraID);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        string fileVirtualPath = Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.Edi835FileName, logpath);
                        //string fullpath = HttpContext.Current.Server.MapCustomPath(fileVirtualPath);
                        string fullpath = Common.HttpContext_Current_Server_MapPath(fileVirtualPath);

                        //upload835File.ReadableFilePath = amazonFileUpload.UploadFile(ConfigSettings.ZarephathBucket, fileVirtualPath.TrimStart('/'), fullpath, true);
                        upload835File.LogFilePath = fileVirtualPath;// upload835File.FilePath;
                        upload835File.Upload835FileProcessStatus = (int)EnumUpload835FileProcessStatus.ErrorInProcess;
                        SaveEntity(upload835File);
                    }//GetScalar(StoredProcedure.UpdateBatchAfter835FileProcessed);
                }

                #endregion
                Common.CreateLogFile("Process Edi835Files CronJob Completed Succesfully.", ConfigSettings.Edi835FileName, logpath);

            }
            catch (Exception ex)
            {
                Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.Edi835FileName, logpath);
                response.IsSuccess = false;
            }
            return response;
        }

        private Edi835ResponseModel HC_GenerateEdi835ModelFromPaperRemits(string absolutePathForEdi835File, string absolutePathForGenerateReadableFile, string relativePathForGenerateReadableFile)
        {
            Edi835ResponseModel edi835ResponseModel = new Edi835ResponseModel()
            {
                GeneratedFileAbsolutePath = absolutePathForGenerateReadableFile,
                GeneratedFileRelativePath = relativePathForGenerateReadableFile
            };

            //var dt = new DataTable();
            var dt = Common.CSVtoDataTable(absolutePathForEdi835File);
            //using (var reader = new ExcelDataReader(absolutePathForEdi835File))
            //    dt.Load(reader);

            foreach (DataRow row in dt.Rows)
            {
                Edi835Model model = new Edi835Model();
                model.BatchID = row[Resource.PR_BatchID].ToString();
                model.NoteID = row[Resource.PR_NoteID].ToString();
                model.BatchNoteID = row[Resource.PR_BatchNoteID].ToString();
                model.NM103_PatientLastName = row[Resource.PR_LastName].ToString();
                model.NM104_PatientFirstName = row[Resource.PR_FirstName].ToString();
                model.CheckDate = row[Resource.PR_CheckDate].ToString();// == DBNull.Value ? "" : DateTime.FromOADate(Convert.ToDouble(row[Resource.PR_CheckDate])).ToString("MM/dd/yyyy");
                model.CheckNumber = row[Resource.PR_CheckNumber].ToString();
                model.CheckAmount = row[Resource.PR_CheckAmount].ToString();
                //model.FreeField7 = Common.Getarrayvalue<string>(7, values);
                model.ProcessedDate = row[Resource.PR_ProcessedDate].ToString();// == DBNull.Value ? "" : DateTime.FromOADate(Convert.ToDouble(row[Resource.PR_ProcessedDate])).ToString("MM/dd/yyyy");
                model.NM109_PatientIdentifier = row[Resource.PR_ClientIdNumber].ToString();
                model.PolicyNumber = row[Resource.PR_PolicyNumber].ToString();
                model.ICN = row[Resource.PR_ClaimNumber].ToString();
                model.CLP07_PayerClaimControlNumber = model.ICN;
                model.NM103_ServiceProviderName = row[Resource.PR_BillingProviderName].ToString();
                model.NM109_ServiceProviderNpi = row[Resource.PR_BillingProviderNPI].ToString();
                model.DTM02_ServiceStartDate = row[Resource.PR_ServiceDate].ToString();// == DBNull.Value ? "" : DateTime.FromOADate(Convert.ToDouble(row[Resource.PR_ServiceDate])).ToString("MM/dd/yyyy"); 
                model.DTM02_ServiceEndDate = model.DTM02_ServiceStartDate;
                model.CLP08_PlaceOfService = row[Resource.PR_PosID].ToString();
                model.SVC05_ServiceCodeUnit = row[Resource.PR_BilledUnit].ToString();
                model.SVC01_02_ServiceCode = row[Resource.PR_ServiceCode].ToString();
                model.SVC01_01_ServiceCodeQualifier = row[Resource.PR_ModifierCode].ToString();
                model.SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount = row[Resource.PR_BilledAmount].ToString();
                //model.FreeField19 = Common.Getarrayvalue<string>(19, values);
                model.AMT01_ServiceLineAllowedAmount_AllowedAmount = row[Resource.PR_AllowedAmount].ToString();
                model.Deductible = row[Resource.PR_Deductible].ToString();
                model.Coins = row[Resource.PR_Coinsurance].ToString();
                model.SVC03_LineItemProviderPaymentAmoun_PaidAmount = row[Resource.PR_PaidAmount].ToString();
                //model.FreeField24 = Common.Getarrayvalue<string>(24, values);
                //model.FreeField25 = Common.Getarrayvalue<string>(25, values);
                //model.FreeField26 = Common.Getarrayvalue<string>(26, values);
                model.LX01_ClaimSequenceNumber = row[Resource.PR_LX01_ClaimSequenceNumber].ToString();
                model.CLP02_ClaimStatusCode = row[Resource.PR_CLP02_ClaimStatusCode].ToString();
                model.CLP01_ClaimSubmitterIdentifier = row[Resource.PR_CLP01_ClaimSubmitterIdentifier].ToString();
                model.CLP03_TotalClaimChargeAmount = row[Resource.PR_CLP03_TotalClaimChargeAmount].ToString();
                model.CLP04_TotalClaimPaymentAmount = row[Resource.PR_CLP04_TotalClaimPaymentAmount].ToString();
                model.CLP05_PatientResponsibilityAmount = row[Resource.PR_CLP05_PatientResponsibilityAmount].ToString();
                model.CLP07_PayerClaimControlNumber = row[Resource.PR_CLP07_PayerClaimControlNumber].ToString();
                model.CAS01_ClaimAdjustmentGroupCode = row[Resource.PR_CAS01_ClaimAdjustmentGroupCode].ToString();
                model.CAS02_ClaimAdjustmentReasonCode = row[Resource.PR_CAS02_ClaimAdjustmentReasonCode].ToString();
                model.CAS03_ClaimAdjustmentAmount = row[Resource.PR_CAS03_ClaimAdjustmentAmount].ToString();




                model.N102_PayeeName = row[Resource.PR_Payee].ToString();
                model.N103_PayeeIdentificationQualifier = row[Resource.PR_PayeeIDQualifier].ToString();
                model.N104_PayeeIdentification = row[Resource.PR_PayeeID].ToString();

                model.N102_PayorName = row[Resource.PR_Payor].ToString();
                model.PER02_PayorBusinessContactName = row[Resource.PR_PayorBusinessContactName].ToString();
                model.PER04_PayorBusinessContact = row[Resource.PR_PayorBusinessContact].ToString();
                model.PER02_PayorTechnicalContactName = row[Resource.PR_PayorTechnicalContactName].ToString();
                model.PER04_PayorTechnicalContact = row[Resource.PR_PayorTechnicalContact].ToString();
                model.PER06_PayorTechnicalEmail = row[Resource.PR_PayorTechnicalEmail].ToString();
                model.PER04_PayorTechnicalContactUrl = row[Resource.PR_PayorTechnicalContactUrl].ToString();


                edi835ResponseModel.Edi835ModelList.Add(model);


            }





            return edi835ResponseModel;

        }
        #endregion

        #region Reconcile 835 / EOB

        public ServiceResponse HC_SetReconcile835Page()
        {
            var response = new ServiceResponse();
            HC_AddReconcile835Model addReconcile835Model = GetMultipleEntity<HC_AddReconcile835Model>(StoredProcedure.HC_GetSetReconcile835);
            addReconcile835Model.SearchReconcile835ListPage.Str835ProcessedOnlyID = 0;
            addReconcile835Model.SearchReconcile835ListPage.ServiceID = "-1";
            addReconcile835Model.Services = Common.SetNoteServices();
            addReconcile835Model.SearchReconcile835ListPage.ClaimAdjustmentTypeID = "-1";
            addReconcile835Model.ClaimAdjustmentTypes = Common.ClaimAdjustmentTypes();

            addReconcile835Model.ClaimStatusCodeList.Add(new ClaimStatusCode() { ClaimStatusCodeID = -2, ClaimStatusName = Resource.NA });

            addReconcile835Model.ClaimStatusList.Add(new ClaimStatus() { ClaimStatusID = 1, StatusName = "Paid" });
            addReconcile835Model.ClaimStatusList.Add(new ClaimStatus() { ClaimStatusID = 2, StatusName = "Denied" });

            response.Data = addReconcile835Model;
            response.IsSuccess = true;
            return response;
        }

        public List<Upload835File> HC_GetUpload835Files(long payorId, int pageSize, string searchText = null)
        {
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "PayorId", Value = payorId.ToString()},
                    new SearchValueData {Name = "SearchText", Value = searchText},
                    new SearchValueData {Name = "PageSize", Value = pageSize.ToString()}
                };

            return GetEntityList<Upload835File>(StoredProcedure.HC_GetUpload835FilesForAutoComplete, searchParam);
        }

        public ServiceResponse HC_GetReconcile835List(SearchReconcile835ListPage searchReconcile835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                if (searchReconcile835Model != null)
                    HC_SetSearchFilterForReconcile835ListPage(searchReconcile835Model, searchList);
                Page<ListReconcile835Model> listReconcile835Model = GetEntityPageList<ListReconcile835Model>(StoredProcedure.HC_GetReconcile835List, searchList, pageSize,
                    pageIndex, sortIndex, sortDirection);
                response.Data = listReconcile835Model;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.Reconcile835), Resource.ExceptionMessage);
            }
            return response;
        }

        private static void HC_SetSearchFilterForReconcile835ListPage(SearchReconcile835ListPage searchReconcile835Model, List<SearchValueData> searchList)
        {
            if (searchReconcile835Model.ReferralID > 0)
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(searchReconcile835Model.ReferralID) });

            if (searchReconcile835Model.PayorID > 0)
                searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchReconcile835Model.PayorID) });

            if (searchReconcile835Model.Batch != null)
                searchList.Add(new SearchValueData { Name = "Batch", Value = Convert.ToString(searchReconcile835Model.Batch) });

            if (searchReconcile835Model.ClaimNumber != null)
                searchList.Add(new SearchValueData { Name = "ClaimNumber", Value = Convert.ToString(searchReconcile835Model.ClaimNumber) });

            if (searchReconcile835Model.Client != null)
                searchList.Add(new SearchValueData { Name = "Client", Value = Convert.ToString(searchReconcile835Model.Client) });

            if (searchReconcile835Model.StrServiceCodeID != null)
                searchList.Add(new SearchValueData { Name = "ServiceCode", Value = Convert.ToString(searchReconcile835Model.StrServiceCodeID) });

            if (searchReconcile835Model.ServiceStartDate != null)
                searchList.Add(new SearchValueData { Name = "ServiceStartDate", Value = Convert.ToDateTime(searchReconcile835Model.ServiceStartDate).ToString(Constants.DbDateFormat) });

            if (searchReconcile835Model.ServiceEndDate != null)
                searchList.Add(new SearchValueData { Name = "ServiceEndDate", Value = Convert.ToDateTime(searchReconcile835Model.ServiceEndDate).ToString(Constants.DbDateFormat) });

            if (searchReconcile835Model.ModifierID > 0)
                searchList.Add(new SearchValueData { Name = "ModifierID", Value = Convert.ToString(searchReconcile835Model.ModifierID) });

            if (searchReconcile835Model.PosID > 0)
                searchList.Add(new SearchValueData { Name = "PosID", Value = Convert.ToString(searchReconcile835Model.PosID) });

            searchList.Add(new SearchValueData { Name = "ClaimStatusCodeID", Value = Convert.ToString(searchReconcile835Model.ClaimStatusCodeID) });

            if (!String.IsNullOrEmpty(searchReconcile835Model.Status))
                searchList.Add(new SearchValueData { Name = "ReconcileStatus", Value = Convert.ToString(searchReconcile835Model.Status) });

            if (searchReconcile835Model.ClaimAdjustmentGroupCodeID != null)
                searchList.Add(new SearchValueData { Name = "ClaimAdjustmentGroupCodeID", Value = Convert.ToString(searchReconcile835Model.ClaimAdjustmentGroupCodeID) });

            if (searchReconcile835Model.ClaimAdjustmentReasonCodeID != null)
                searchList.Add(new SearchValueData { Name = "ClaimAdjustmentReasonCodeID", Value = Convert.ToString(searchReconcile835Model.ClaimAdjustmentReasonCodeID) });

            searchList.Add(new SearchValueData { Name = "Get835ProcessedOnly", Value = Convert.ToString(searchReconcile835Model.Str835ProcessedOnlyID) });

            searchList.Add(new SearchValueData { Name = "Upload835FileID", Value = Convert.ToString(searchReconcile835Model.Upload835FileID) });


            searchList.Add(new SearchValueData { Name = "ServiceID", Value = Convert.ToString(searchReconcile835Model.ServiceID) });
            searchList.Add(new SearchValueData { Name = "ClaimAdjustmentTypeID", Value = Convert.ToString(searchReconcile835Model.ClaimAdjustmentTypeID) });


            searchList.Add(new SearchValueData { Name = "NoteID", Value = Convert.ToString(searchReconcile835Model.NoteID) });
            searchList.Add(new SearchValueData { Name = "PayorClaimNumber", Value = Convert.ToString(searchReconcile835Model.PayorClaimNumber) });


        }

        public ServiceResponse HC_GetReconcileBatchNoteDetails(long BatchNoteID, long BatchID, long NoteID, long? Upload835FileID)
        {
            var response = new ServiceResponse();
            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ParentBatchNoteID", Value = Convert.ToString(BatchNoteID)},
                    new SearchValueData {Name = "BatchID", Value = Convert.ToString(BatchID)},
                    new SearchValueData {Name = "NoteID", Value = Convert.ToString(NoteID)},
                    new SearchValueData {Name = "Upload835FileID", Value = Convert.ToString(Upload835FileID)},
                   };
            ReconcileBatchNoteDetailsModel reconcileBatchNoteDetailsModel = GetMultipleEntity<ReconcileBatchNoteDetailsModel>(StoredProcedure.HC_GetReconcileBatchNoteDetails, searchlist);
            response.Data = reconcileBatchNoteDetailsModel;
            response.IsSuccess = true;

            return response;
        }

        public ServiceResponse HC_MarkNoteAsLatest(long BatchNoteID, long BatchID, long NoteID, long? Upload835FileID)
        {
            var response = new ServiceResponse();
            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "BatchNoteID", Value = Convert.ToString(BatchNoteID)},
                    new SearchValueData {Name = "BatchID", Value = Convert.ToString(BatchID)},
                    new SearchValueData {Name = "NoteID", Value = Convert.ToString(NoteID)},
                   };
            GetScalar(StoredProcedure.HC_MarkNoteAsLatest, searchlist);
            response.IsSuccess = true;
            response.Message = Resource.MasrkAsLatestSuccessfully;
            return response;
        }

        public ServiceResponse HC_ExportReconcile835List(SearchReconcile835ListPage searchReconcile835Model, string sortIndex = "", string sortDirection = "")
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            if (searchReconcile835Model != null)
                SetSearchFilterForReconcile835ListPage(searchReconcile835Model, searchList);
            searchList.AddRange(Common.SetPagerValues(0, 0, sortIndex, sortDirection));

            List<ExportReconcile835ListModel> totalData = GetEntityList<ExportReconcile835ListModel>(StoredProcedure.HC_ExportReconcile835List, searchList);

            if (totalData.Count > 0)
            {
                string fileName = string.Format("{0}_{1}", Constants.ReportName_ClaimsOutcomeStatus, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);

                string basePath = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_xlsx);
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                CreateExcelFile.CreateExcelDocument(totalData, downloadFileModel.AbsolutePath);
                response.IsSuccess = true;
                response.Data = downloadFileModel;
                response.IsSuccess = true;
            }
            return response;
        }

        public ServiceResponse HC_SetClaimAdjustmentFlag(long batchId, long noteId, string claimAdjustmentType, string claimAdjustmentReason)
        {
            var response = new ServiceResponse();

            string msg = string.Format(Resource.AdjTypeStatusChangeSuccessMessage, claimAdjustmentType);
            if (claimAdjustmentType == ClaimAdjustmentType.ClaimAdjustmentType_Remove)
            {
                claimAdjustmentType = null;
                claimAdjustmentReason = null;
                msg = Resource.AdjTypeStatusUnmarkedSuccessMessage;

            }

            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "BatchID", Value = Convert.ToString(batchId)},
                    new SearchValueData {Name = "NoteID", Value = Convert.ToString(noteId)},
                    new SearchValueData {Name = "ClaimAdjustmentType", Value = Convert.ToString(claimAdjustmentType)},
                    new SearchValueData {Name = "ClaimAdjustmentReason", Value = Convert.ToString(claimAdjustmentReason)},
                   };

            GetScalar(StoredProcedure.HC_SetClaimAdjustmentFlag01, searchlist);
            response.IsSuccess = true;
            response.Message = msg;
            return response;
        }

        public ServiceResponse HC_BulkSetClaimAdjustmentFlag(BulkClaimAdjustmentFlagModel model)
        {
            var response = new ServiceResponse();

            string msg = string.Format(Resource.AdjTypeStatusChangeSuccessMessage, model.AdjustmentType);

            if (model.AdjustmentType == ClaimAdjustmentType.ClaimAdjustmentType_Remove)
            {
                model.AdjustmentType = null;
                model.AdjustmentReason = null;
                msg = Resource.AdjTypeStatusUnmarkedSuccessMessage;

            }
            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "BatchIDs", Value = Convert.ToString(model.BatchIDs)},
                    new SearchValueData {Name = "ReferralIDs", Value = Convert.ToString(model.ReferralIDs)},
                    new SearchValueData {Name = "NoteIDs", Value = Convert.ToString(model.NoteIDs)},
                    new SearchValueData {Name = "ClaimAdjustmentType", Value = Convert.ToString(model.AdjustmentType)},
                    new SearchValueData {Name = "ClaimAdjustmentReason", Value = Convert.ToString(model.AdjustmentReason)},
                   };

            GetScalar(StoredProcedure.HC_BulkSetClaimAdjustmentFlag, searchlist);
            response.IsSuccess = true;
            response.Message = msg;
            return response;
        }

        #endregion

        #region New Changes

        public ServiceResponse HC_GetReconcileList(SearchReconcile835ListPage searchReconcile835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                if (searchReconcile835Model != null)
                    HC_SetSearchFilterForReconcile835ListPage(searchReconcile835Model, searchList);
                Page<ListReconcileModel> listReconcile835Model = GetEntityPageList<ListReconcileModel>(StoredProcedure.HC_GetReconcileList, searchList, pageSize,
                    pageIndex, sortIndex, sortDirection);
                response.Data = listReconcile835Model;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.Reconcile835), Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_GetParentReconcileList(SearchReconcile835ListPage searchReconcile835Model, int fromIndex, int pageSize)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();

                if (searchReconcile835Model != null)
                    HC_SetSearchFilterForReconcile835ListPage(searchReconcile835Model, searchList);
                //searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(searchGetParentReconcileList.ReferralID) });
                //if (searchGetParentReconcileList.ServiceStartDate != null)
                //    searchList.Add(new SearchValueData { Name = "ServiceStartDate", Value = Convert.ToDateTime(searchGetParentReconcileList.ServiceStartDate).ToString(Constants.DbDateFormat) });

                //if (searchGetParentReconcileList.ServiceEndDate != null)
                //    searchList.Add(new SearchValueData { Name = "ServiceEndDate", Value = Convert.ToDateTime(searchGetParentReconcileList.ServiceEndDate).ToString(Constants.DbDateFormat) });

                Page<ListReconcile835Model> listReconcile835Model = GetEntityPageList<ListReconcile835Model>(StoredProcedure.HC_GetParentReconcileList, searchList, pageSize, fromIndex, "ReferralID", "ASC");
                response.Data = listReconcile835Model;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.Reconcile835), Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_GetChildReconcileList(long noteId, long batchId)
        {
            var response = new ServiceResponse();
            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "NoteID", Value = Convert.ToString(noteId)},
                    new SearchValueData {Name = "BatchID", Value = Convert.ToString(batchId)},
                };
            ChildReconcileListModel reconcileBatchNoteDetailsModel = GetMultipleEntity<ChildReconcileListModel>(StoredProcedure.HC_GetChildReconcileList, searchlist);
            response.Data = reconcileBatchNoteDetailsModel;
            response.IsSuccess = true;

            return response;
        }

        public ServiceResponse HC_MarkNoteAsLatest01(long BatchNoteID, long BatchID, long NoteID, long? Upload835FileID)
        {
            var response = new ServiceResponse();
            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "BatchNoteID", Value = Convert.ToString(BatchNoteID)},
                    new SearchValueData {Name = "BatchID", Value = Convert.ToString(BatchID)},
                    new SearchValueData {Name = "NoteID", Value = Convert.ToString(NoteID)},
                   };
            GetScalar(StoredProcedure.HC_MarkNoteAsLatest01, searchlist);
            response.IsSuccess = true;
            response.Message = Resource.MasrkAsLatestSuccessfully;
            return response;
        }

        public ServiceResponse HC_ExportReconcileList(SearchReconcile835ListPage searchReconcile835Model, string sortIndex = "", string sortDirection = "")
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            if (searchReconcile835Model != null)
                SetSearchFilterForReconcile835ListPage(searchReconcile835Model, searchList);
            searchList.AddRange(Common.SetPagerValues(0, 0, sortIndex, sortDirection));

            List<ExportReconcile835ListModel> totalData = GetEntityList<ExportReconcile835ListModel>(StoredProcedure.HC_ExportReconcileList, searchList);

            if (totalData.Count > 0)
            {
                string fileName = string.Format("{0}_{1}", Constants.ReportName_ClaimsOutcomeStatus, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);

                string basePath = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_xlsx);
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                CreateExcelFile.CreateExcelDocument(totalData, downloadFileModel.AbsolutePath);
                response.IsSuccess = true;
                response.Data = downloadFileModel;
                response.IsSuccess = true;
            }
            return response;
        }
        #endregion

        #region Generate CMS-1500

        public ServiceResponse GenerateCMS1500(EDIFileSearchModel EDIFileSearchModellong, bool isCaseManagement, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchData = new List<SearchValueData>
                {

                    new SearchValueData("StrNoteIds", Convert.ToString(EDIFileSearchModellong.StrNoteIds)),
                    new SearchValueData("BatchID", Convert.ToString(EDIFileSearchModellong.BatchID)),
                    new SearchValueData("PayorID", Convert.ToString(EDIFileSearchModellong.PayorID)),
                    new SearchValueData("ReferralID", Convert.ToString(EDIFileSearchModellong.ReferralID)),
                    new SearchValueData("fileType", Convert.ToString(EDIFileSearchModellong.FileType)),

                    new SearchValueData("BatchTypeID", Convert.ToString(EDIFileSearchModellong.BatchTypeID)),
                    //new SearchValueData("StartDate", Convert.ToString(EDIFileSearchModellong.StartDate)),
                    //new SearchValueData("EndDate", Convert.ToString(EDIFileSearchModellong.EndDate)),

                    new SearchValueData {Name = "StartDate", Value =Convert.ToDateTime(EDIFileSearchModellong.StartDate).ToString(Constants.DbDateFormat)},
                     new SearchValueData {Name = "EndDate", Value =Convert.ToDateTime(EDIFileSearchModellong.EndDate).ToString(Constants.DbDateFormat)},

                    new SearchValueData("ServiceCodeIDs", Convert.ToString(EDIFileSearchModellong.ServiceCodeIDs)),
                    new SearchValueData("ClientName", Convert.ToString(EDIFileSearchModellong.ClientName)),
                    new SearchValueData("LoggedInID", Convert.ToString(loggedInId))

                };



                List<BatchRelatedAllDataModel> model = null;

                model = GetEntityList<BatchRelatedAllDataModel>(StoredProcedure.HC_GenerateEdiFileModel, searchData);

                //if(!isCaseManagement)
                //    model = GetEntityList<BatchRelatedAllDataModel>(StoredProcedure.HC_GenerateEdiFileModel,searchData);
                //else
                //    model = GetEntityList<BatchRelatedAllDataModel>(StoredProcedure.HC_CM_GenerateEdiFileModel,searchData);

                var cms1500 = model.GroupBy(g => new
                {

                    #region Group By

                    g.BatchID,
                    g.ReferralID,
                    g.PatientName,
                    g.FirstName,
                    g.MiddleName,
                    g.LastName,
                    g.Dob,
                    g.Gender,
                    g.AHCCCSID,
                    g.CISNumber,
                    g.MedicalRecordNumber,
                    g.PolicyNumber,
                    g.AdmissionDate,
                    g.SubscriberID,
                    g.ClaimSubmitterIdentifier,
                    g.PatientAccountNumber,
                    g.PayorID,
                    g.PayorName,
                    g.PayorCity,
                    g.PayorState,
                    g.PayorIdentificationNumber,
                    g.ContactID,
                    g.Address,
                    g.City,
                    g.State,
                    g.ZipCode,
                    g.Phone1,
                    g.PhysicianID,
                    g.PhysicianFullName,
                    g.PhysicianNPINumber,
                    g.NoteID,
                    g.BillingProviderNPI,
                    BillingProviderInfo = g.BillingProviderName +
                                          ("<br/>" + g.BillingProviderAddress) +
                                          ("<br/>" + g.BillingProviderCity) +
                                           ("<br/>" + g.BillingProviderState) +
                                           ("," + g.BillingProviderZipcode),
                    RenderingProviderInfo = g.RenderingProviderName +
                                          ("<br/>" + g.RenderingProviderAddress) +
                                          ("<br/>" + g.RenderingProviderCity) +
                                          ("<br/>" + g.RenderingProviderState) +
                                          ("," + g.RenderingProviderZipcode),
                    g.BillingProviderEIN,
                    g.ReferralBillingAuthorizationID,
                    g.AuthorizationCode,
                    g.ContinuedDX,
                    g.RenderingProviderNPI,
                    g.RenderingProviderFirstName,
                    g.RenderingProviderName

                    #endregion

                }).Select(s => new GenerateEdiFileModel
                {

                    #region Select

                    BatchID = s.Key.BatchID,
                    ReferralID = s.Key.ReferralID,
                    PatientName = s.Key.PatientName,
                    FirstName = s.Key.FirstName,
                    MiddleName = s.Key.MiddleName,
                    LastName = s.Key.LastName,
                    Dob = s.Key.Dob,
                    Gender = s.Key.Gender,
                    AHCCCSID = s.Key.AHCCCSID,
                    CISNumber = s.Key.CISNumber,
                    MedicalRecordNumber = s.Key.MedicalRecordNumber,
                    PolicyNumber = s.Key.PolicyNumber,
                    AdmissionDate = s.Key.AdmissionDate,
                    SubscriberID = s.Key.SubscriberID,
                    ClaimSubmitterIdentifier = s.Key.ClaimSubmitterIdentifier,
                    PatientAccountNumber = s.Key.PatientAccountNumber,
                    DobDD = !string.IsNullOrWhiteSpace(s.Key.Dob) ? Convert.ToDateTime(s.Key.Dob).ToString("dd") : string.Empty,
                    DobMM = !string.IsNullOrWhiteSpace(s.Key.Dob) ? Convert.ToDateTime(s.Key.Dob).ToString("MM") : string.Empty,
                    DobYYYY = !string.IsNullOrWhiteSpace(s.Key.Dob) ? Convert.ToDateTime(s.Key.Dob).ToString("yyyy") : string.Empty,
                    PayorID = s.Key.PayorID,
                    PayorName = s.Key.PayorName,
                    PayorCity = s.Key.PayorCity,
                    PayorState = s.Key.PayorState,
                    PayorIdentificationNumber = s.Key.PayorIdentificationNumber,
                    ContactID = s.Key.ContactID,
                    Address = s.Key.Address,
                    City = s.Key.City,
                    State = s.Key.State,
                    ZipCode = s.Key.ZipCode,
                    Phone1 = !string.IsNullOrEmpty(s.Key.Phone1) ? "(" + s.Key.Phone1.Remove(3, 7) + ")" + s.Key.Phone1.Remove(0, 3) : string.Empty,
                    PhysicianID = s.Key.PhysicianID,
                    PhysicianFullName = s.Key.PhysicianFullName,
                    PhysicianNPINumber = s.Key.PhysicianNPINumber,
                    NoteID = s.Key.NoteID,
                    BillingProviderNPI = s.Key.BillingProviderNPI,
                    BillingProviderInfo = s.Key.BillingProviderInfo,
                    RenderingProviderInfo = s.Key.RenderingProviderInfo,
                    BillingProviderEIN = s.Key.BillingProviderEIN,
                    ReferralBillingAuthorizationID = s.Key.ReferralBillingAuthorizationID,
                    AuthorizationCode = s.Key.AuthorizationCode,
                    DxCodes = !string.IsNullOrWhiteSpace(s.Key.ContinuedDX) ? s.Key.ContinuedDX.Split(',').Select(x => x.Split(':')).ToList() : new List<string[]>(),
                    ServiceLines = s.Select(x => new ServiceLineForEdiFileModel
                    {
                        ServiceDate = x.ServiceDate,
                        ServiceDateDD = x.ServiceDate.ToString("dd"),
                        ServiceDateMM = x.ServiceDate.ToString("MM"),
                        ServiceDateYYYY = x.ServiceDate.ToString("yyyy"),
                        ServiceCode = x.ServiceCode,
                        POS_CMS1500_ID = x.POS_CMS1500_ID,
                        POS_CMS1500 = x.POS_CMS1500,
                        ModifierIDs = x.ModifierIDs.Split(','),
                        ModifierNames = x.ModifierName.Split(','),
                        CalculatedUnit = x.CalculatedUnit.ToString("F"),
                        CalculatedAmount = x.CalculatedAmount,
                        CalculatedAmountInDollars = x.CalculatedAmount.ToString("F").Split('.')[0],
                        CalculatedAmountInCents = x.CalculatedAmount.ToString("F").Split('.')[1],
                        RenderingProviderNPI = x.RenderingProviderNPI
                    }).ToList(),

                    #endregion

                }).ToList();

                cms1500.ForEach(fe =>
                {
                    #region Set Diagnosis Pointer

                    if (fe.DxCodes.Count >= 4)
                    {
                        fe.DiagnosisPointer = "A, B, C, D";
                    }
                    else if (fe.DxCodes.Count == 3)
                    {
                        fe.DiagnosisPointer = "A, B, C";
                    }
                    else if (fe.DxCodes.Count == 2)
                    {
                        fe.DiagnosisPointer = "A, B";
                    }
                    else if (fe.DxCodes.Count == 1)
                    {
                        fe.DiagnosisPointer = "A";
                    }
                    else
                    {
                        fe.DiagnosisPointer = string.Empty;
                    }

                    #endregion

                    fe.TotalCharge1 = fe.ServiceLines.Sum(x => x.CalculatedAmount).HasValue
                                        ? fe.ServiceLines.Sum(x => x.CalculatedAmount).Value.ToString("F").Split('.')[0]
                                        : "0";

                    fe.TotalCharge2 = fe.ServiceLines.Sum(x => x.CalculatedAmount).HasValue
                                        ? fe.ServiceLines.Sum(x => x.CalculatedAmount).Value.ToString("F").Split('.')[1]
                                        : "0";

                    string newGuid = Guid.NewGuid().ToString();
                    var listOfRecords = fe.ServiceLines.Partition(6);
                    List<MultiServiceLineForEdiFileModel> list = new List<MultiServiceLineForEdiFileModel>();
                    foreach (var item in listOfRecords)
                    {
                        list.Add(new MultiServiceLineForEdiFileModel { UniqueId = newGuid, MultiServiceLines = item });
                    }
                    fe.MultiServiceLineForEdiFileModelList = list;
                });

                response.Data = cms1500;
                response.IsSuccess = true;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }
            return response;
        }

        #endregion

        #region Generate UB-04

        public ServiceResponse GenerateUB04(EDIFileSearchModel EDIFileSearchModellong)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<BatchRelatedAllDataModel> model = GetEntityList<BatchRelatedAllDataModel>(StoredProcedure.HC_GenerateEdiFileModel,
                    new List<SearchValueData>
                    {
                        new SearchValueData("BatchID",Convert.ToString(EDIFileSearchModellong.BatchID)),
                        new SearchValueData("PayorID",Convert.ToString(EDIFileSearchModellong.PayorID)),
                        new SearchValueData("ReferralID",Convert.ToString(EDIFileSearchModellong.ReferralID)),
                        new SearchValueData("fileType",Convert.ToString(EDIFileSearchModellong.FileType)),

                        new SearchValueData("BatchTypeID",Convert.ToString(EDIFileSearchModellong.BatchTypeID)),
                        new SearchValueData("StartDate",Convert.ToString(EDIFileSearchModellong.StartDate)),
                        new SearchValueData("EndDate",Convert.ToString(EDIFileSearchModellong.EndDate)),
                        new SearchValueData("ServiceCodeIDs",Convert.ToString(EDIFileSearchModellong.ServiceCodeIDs)),
                        new SearchValueData("ClientName",Convert.ToString(EDIFileSearchModellong.ClientName))

                    });

                DateTime minDate = DateTime.MaxValue;
                DateTime maxDate = DateTime.MinValue;

                List<string> StringDates = new List<string>();
                float totalAmount = 0;

                foreach (var item in model)
                {
                    StringDates.Add(item.ServiceDate.ToString());
                    totalAmount = totalAmount + item.CalculatedAmount;
                }
                foreach (string dateString in StringDates)
                {
                    DateTime date = DateTime.Parse(dateString);
                    if (date < minDate)
                        minDate = date;
                    if (date > maxDate)
                        maxDate = date;
                }

                var ub04 = model.GroupBy(g => new
                {

                    #region Group By
                    g.BatchID,
                    g.ReferralID,
                    g.FirstName,
                    g.MiddleName,
                    g.LastName,
                    g.PatientName,
                    g.PatientDOB,
                    g.Dob,
                    g.Gender,
                    g.Address,
                    g.City,
                    g.State,
                    g.ZipCode,
                    g.PatientAddress,
                    g.AHCCCSID,
                    g.CISNumber,
                    g.MedicalRecordNumber,
                    g.PolicyNumber,
                    g.StrAdmissionDate,
                    g.SubscriberID,
                    g.ClaimSubmitterIdentifier,
                    g.PatientAccountNumber,
                    g.DobDD,
                    g.DobMM,
                    g.DobYYYY,
                    g.AuthorizationCode,
                    g.AdmissionTypeCode_UB04,
                    g.AdmissionSourceCode_UB04,
                    g.PatientStatusCode_UB04,
                    g.BillingProviderName,
                    g.BillingProviderFirstName,
                    g.BillingProviderAddress,
                    g.BillingProviderCity,
                    g.BillingProviderState,
                    g.BillingProviderZipcode,
                    g.BillingProviderNPI,
                    g.BillingProviderEIN,
                    g.PayorID,
                    g.PayorName,
                    g.PayorShortName,
                    g.PayorAddress,
                    g.PayorIdentificationNumber,
                    g.PayorCity,
                    g.PayorState,
                    g.PayorZipcode,
                    g.PhysicianID,
                    g.PhysicianFirstName,
                    g.PhysicianLastName,
                    g.PhysicianNPINumber,
                    g.ContinuedDX,
                    g.RenderingProviderNPI,
                    g.RenderingProviderFirstName,
                    g.RenderingProviderName,
                    g.AdmissionDate,
                    g.Phone1,
                    g.PhysicianFullName,
                    g.Subscriber_SBR02_RelationshipCode,
                    BillingProviderInfo = g.BillingProviderName +
                                          ("<br/>" + g.BillingProviderAddress) +
                                          ("<br/>" + g.BillingProviderCity) +
                                           ("<br/>" + g.BillingProviderState) +
                                           ("," + g.BillingProviderZipcode),
                    RenderingProviderInfo = g.RenderingProviderName +
                                            ("<br/>" + g.RenderingProviderAddress) +
                                            ("<br/>" + g.RenderingProviderCity) +
                                            ("<br/>" + g.RenderingProviderState) +
                                            ("," + g.RenderingProviderZipcode),
                    #endregion

                }).Select(s => new GenerateEdiFileModel
                {

                    #region Select
                    BatchID = s.Key.BatchID,
                    ReferralID = s.Key.ReferralID,
                    PatientName = s.Key.PatientName,
                    FirstName = s.Key.FirstName,
                    MiddleName = s.Key.MiddleName,
                    LastName = s.Key.LastName,
                    Dob = s.Key.Dob,
                    PatientDOB = !string.IsNullOrWhiteSpace(s.Key.Dob) ? Convert.ToDateTime(s.Key.Dob).ToString("MM/dd/yyyy") : string.Empty,
                    Gender = s.Key.Gender,
                    AHCCCSID = s.Key.AHCCCSID,
                    CISNumber = s.Key.CISNumber,
                    MedicalRecordNumber = s.Key.MedicalRecordNumber,
                    PolicyNumber = s.Key.PolicyNumber,
                    AdmissionDate = s.Key.StrAdmissionDate,
                    SubscriberID = s.Key.SubscriberID,
                    ClaimSubmitterIdentifier = s.Key.ClaimSubmitterIdentifier,
                    PatientAccountNumber = s.Key.PatientAccountNumber,
                    DobDD = !string.IsNullOrWhiteSpace(s.Key.Dob) ? Convert.ToDateTime(s.Key.Dob).ToString("dd") : string.Empty,
                    DobMM = !string.IsNullOrWhiteSpace(s.Key.Dob) ? Convert.ToDateTime(s.Key.Dob).ToString("MM") : string.Empty,
                    DobYYYY = !string.IsNullOrWhiteSpace(s.Key.Dob) ? Convert.ToDateTime(s.Key.Dob).ToString("yy") : string.Empty,

                    PayorName = s.Key.PayorName,
                    PayorCity = s.Key.PayorCity,
                    PayorState = s.Key.PayorState,
                    PayorZipcode = s.Key.PayorZipcode,
                    PayorAddress = s.Key.PayorAddress,
                    PayorIdentificationNumber = s.Key.PayorIdentificationNumber,

                    Address = s.Key.Address,
                    City = s.Key.City,
                    State = s.Key.State,
                    ZipCode = s.Key.ZipCode,
                    PatientAddress = s.Key.PatientAddress,

                    PhysicianFirstName = s.Key.PhysicianFirstName,
                    PhysicianLastName = s.Key.PhysicianLastName,
                    PhysicianNPINumber = s.Key.PhysicianNPINumber,

                    BillingProviderName = s.Key.BillingProviderName,
                    BillingProviderFirstName = s.Key.BillingProviderFirstName,
                    BillingProviderAddress = s.Key.BillingProviderAddress,
                    BillingProviderCity = s.Key.BillingProviderCity,
                    BillingProviderState = s.Key.BillingProviderState,
                    BillingProviderZipcode = s.Key.BillingProviderZipcode,
                    BillingProviderNPI = s.Key.BillingProviderNPI,
                    BillingProviderEIN = s.Key.BillingProviderEIN,

                    RenderingProviderNPI = s.Key.RenderingProviderNPI,
                    RenderingProviderFirstName = s.Key.RenderingProviderFirstName,
                    RenderingProviderName = s.Key.RenderingProviderName,

                    AuthorizationCode = s.Key.AuthorizationCode,
                    AdmissionTypeCode_UB04 = s.Key.AdmissionTypeCode_UB04,
                    AdmissionSourceCode_UB04 = s.Key.AdmissionSourceCode_UB04,
                    PatientStatusCode_UB04 = s.Key.PatientStatusCode_UB04,
                    TotalAmount = totalAmount,
                    TotalAmountInDollars = totalAmount.ToString("F").Split('.')[0],
                    TotalAmountInCents = totalAmount.ToString("F").Split('.')[1],
                    ServiceStartDate = minDate.ToString("MM/dd/yy"),
                    ServiceEndDate = maxDate.ToString("MM/dd/yy"),
                    DxCodes = !string.IsNullOrWhiteSpace(s.Key.ContinuedDX) ? s.Key.ContinuedDX.Split(',').Select(x => x.Split(':')).ToList() : new List<string[]>(),
                    Phone1 = s.Key.Phone1,
                    PhysicianFullName = s.Key.PhysicianFullName,
                    BillingProviderInfo = s.Key.BillingProviderInfo,
                    RenderingProviderInfo = s.Key.RenderingProviderInfo,
                    Subscriber_SBR02_RelationshipCode = s.Key.Subscriber_SBR02_RelationshipCode,
                    ServiceLines = s.Select(x => new ServiceLineForEdiFileModel
                    {
                        CareType = x.CareType,
                        RevenueCode = x.RevenueCode,
                        RenderingProviderNPI = x.RenderingProviderNPI,
                        ServiceCode = x.ServiceCode,
                        POS_CMS1500 = x.POS_CMS1500,

                        ModifierNames = x.ModifierName.Split(','),
                        ModifierName = x.ModifierName,

                        Rate = x.Rate,
                        RateInDollars = x.Rate.ToString("F").Split('.')[0],
                        RateInCents = x.Rate.ToString("F").Split('.')[1],

                        ServiceDate = x.ServiceDate,
                        StrServiceDate = x.ServiceDate.ToString("MM/dd/yy"),
                        CalculatedUnit = x.CalculatedUnit.ToString("F"),
                        ServiceDateDD = x.ServiceDate.ToString("dd"),
                        ServiceDateMM = x.ServiceDate.ToString("MM"),
                        ServiceDateYYYY = x.ServiceDate.ToString("yy"),

                        CalculatedUnitInFloat = x.CalculatedUnit,

                        CalculatedAmount = x.CalculatedAmount,
                        CalAmount = x.CalculatedAmount,
                        CalculatedAmountInDollars = x.CalculatedAmount.ToString("F").Split('.')[0],
                        CalculatedAmountInCents = x.CalculatedAmount.ToString("F").Split('.')[1]
                    }).ToList()

                    #endregion

                }).ToList();

                ub04.ForEach(fe =>
                {
                    string newGuid = Guid.NewGuid().ToString();
                    var listOfRecords = fe.ServiceLines.Partition((EDIFileSearchModellong.FileType.Equals("CMS1500")) ? 6 : 22);
                    List<MultiServiceLineForEdiFileModel> list = new List<MultiServiceLineForEdiFileModel>();
                    foreach (var item in listOfRecords)
                    {
                        list.Add(new MultiServiceLineForEdiFileModel { UniqueId = newGuid, MultiServiceLines = item });
                    }
                    fe.MultiServiceLineForEdiFileModelList = list;
                });

                ub04.ForEach(fe =>
                {
                    fe.MultiServiceLineForEdiFileModelList.ForEach(msl =>
                    {
                        msl.FinalAmountInDollars = msl.FinalAmount.ToString("F").Split('.')[0];
                        msl.FinalAmountInCents = msl.FinalAmount.ToString("F").Split('.')[1];
                    });
                });

                response.Data = ub04;
                response.IsSuccess = true;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }
            return response;
        }

        #endregion

        #region EdiFileLog List

        public ServiceResponse HC_SetEdiFileLogListPage()
        {
            var response = new ServiceResponse();
            SetEdiFileLogModelListPage setEdiFileLogModelListPage = GetMultipleEntity<SetEdiFileLogModelListPage>(StoredProcedure.HC_SetEdiFilesLogListPage);
            response.Data = setEdiFileLogModelListPage;
            return response;
        }

        public ServiceResponse HC_GetEdiFileLogList(SearchEdiFileLogListPage searchEdiFileLogListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                if (searchEdiFileLogListPage != null)
                    HC_SetSearchFilterForEdiFilesLogListPage(searchEdiFileLogListPage, searchList);
                Page<ListEdiFileLogModel> listEdiFilesLogModel = GetEntityPageList<ListEdiFileLogModel>(StoredProcedure.HC_GetEdiFileLogList, searchList, pageSize,
                                                              pageIndex, sortIndex, sortDirection);
                response.Data = listEdiFilesLogModel;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.EdiFileLog), Resource.ExceptionMessage);
            }
            return response;
        }

        private static void HC_SetSearchFilterForEdiFilesLogListPage(SearchEdiFileLogListPage searchEdiFileLogListPage, List<SearchValueData> searchList)
        {
            if (searchEdiFileLogListPage.EdiFileLogID > 0)
                searchList.Add(new SearchValueData { Name = "EdiFileLogID", Value = Convert.ToString(searchEdiFileLogListPage.EdiFileLogID) });

            if (searchEdiFileLogListPage.EdiFileTypeID > 0)
                searchList.Add(new SearchValueData { Name = "EdiFileTypeID", Value = Convert.ToString(searchEdiFileLogListPage.EdiFileTypeID) });

            if (searchEdiFileLogListPage.FileName != null)
                searchList.Add(new SearchValueData { Name = "FileName", Value = Convert.ToString(searchEdiFileLogListPage.FileName) });

            if (searchEdiFileLogListPage.EdiFileLogID > 0)
                searchList.Add(new SearchValueData { Name = "FilePath", Value = Convert.ToString(searchEdiFileLogListPage.FilePath) });
        }

        public ServiceResponse HC_DeleteEdiFileLog(SearchEdiFileLogListPage searchEdiFileLogListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                #region Delete File from folder

                List<SearchValueData> deletesearchList = new List<SearchValueData>();
                if (!string.IsNullOrEmpty(searchEdiFileLogListPage.ListOfIdsInCSV))
                    deletesearchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchEdiFileLogListPage.ListOfIdsInCSV });

                List<ListEdiFileLogModel> listEdiFilesLogModel = GetEntityList<ListEdiFileLogModel>(StoredProcedure.HC_GetEdiFileLogsFilePathList, deletesearchList);

                if (listEdiFilesLogModel != null)
                {
                    foreach (var model in listEdiFilesLogModel)
                    {
                        if (model.EdiFileTypeID != (int)EnumEdiFileTypes.Edi835)
                        {
                            string filePath = HttpContext.Current.Server.MapCustomPath(model.FilePath);
                            //if (File.Exists(filePath))
                            //    File.Delete(filePath);

                            FileInfo fi = new FileInfo(filePath);
                            DirectoryInfo di = fi.Directory;
                            if (di.Exists)
                                di.Delete(true);

                            //AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                            //amazonFileUpload.DeleteFile(ConfigSettings.ZarephathBucket, model.FilePath);
                        }
                    }
                }

                #endregion

                #region Delete Record from DataBase


                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                SetSearchFilterForEdiFilesLogListPage(searchEdiFileLogListPage, searchList);

                if (!string.IsNullOrEmpty(searchEdiFileLogListPage.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchEdiFileLogListPage.ListOfIdsInCSV });
                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

                List<ListEdiFileLogModel> totalData = GetEntityList<ListEdiFileLogModel>(StoredProcedure.HC_DeleteEdiFileLog, searchList);
                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListEdiFileLogModel> getEdiFilesLogList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);

                #endregion

                response.Data = getEdiFilesLogList;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.EdiFileLog);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }

        #endregion




        #region Process 270/271


        #region Process 270
        public ServiceResponse HC_Process270_271()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            var searchList = new List<SearchValueData>();
            AddProcess270271Model model = GetMultipleEntity<AddProcess270271Model>(StoredProcedure.HC_GetAddProcess270271Model, searchList);
            model.AddProcess270Model.ServiceIDs = Resource.AllServicesText;
            model.AddProcess270Model.ReferralStatusIDs = Constants.Eligibilty270Status.Split(',').ToList();


            model.SearchProcess270ListPage.ServiceID = Resource.AllServicesText;
            model.SearchProcess270ListPage.IsDeleted = 0;
            model.SearchProcess271ListPage.ServiceID = Resource.AllServicesText;
            model.SearchProcess271ListPage.IsDeleted = 0;


            model.ServiceList = Common.SetServicesFilterFor270Process();
            model.DeleteFilter = Common.SetDeleteFilter();
            model.FileProcessStatus = Common.SetUpload835FileProcessStatusFilter();
            serviceResponse.Data = model;
            return serviceResponse;
        }

        public ServiceResponse HC_Generate270File(AddProcess270Model addProcess270Model, SearchProcess270ListPage searchEdiFileLogListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            Edi270 edi270 = new Edi270();
            string filePath = String.Format(_cacheHelper.EdiFile270UploadPath, _cacheHelper.Domain);

            filePath = string.Format("{0}/", filePath);
            string tempFileName = string.Format("Edi270_{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmssfff"), Constants.FileExtension_Txt);


            var searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "PayorIDs", Value = addProcess270Model.PayorIDs });
            if (addProcess270Model.ReferralStatusIDs.Count > 0)
                searchList.Add(new SearchValueData() { Name = "ReferralStatusIDs", Value = String.Join(",", addProcess270Model.ReferralStatusIDs.Select(x => x.ToString()).ToArray()) });
            searchList.Add(new SearchValueData() { Name = "ServiceIDs", Value = addProcess270Model.ServiceIDs });
            searchList.Add(new SearchValueData() { Name = "ClientName", Value = addProcess270Model.Name });
            //searchList.Add(new SearchValueData() { Name = "ReferralStatusID", Value = ((int)ReferralStatus.ReferralStatuses.Active).ToString() });
            searchList.Add(new SearchValueData() { Name = "AllServiceText", Value = Resource.AllServicesText });

            Parent270DataModel model = GetMultipleEntity<Parent270DataModel>(StoredProcedure.HC_GetClientDetailsFor270Process, searchList);
            if (model.ListClientDetailsfor270Model.Count > 0)
            {
                try
                {
                    PayorEdi270Setting payorEdi270Setting = model.PayorEdi270Setting;
                    Edi270Model edi837Model = GetEdit270Model(ref payorEdi270Setting, model);
                    string fileServerPath = HttpContext.Current.Server.MapCustomPath(filePath);
                    string generatedFilePath = edi270.GenerateEdi270File(edi837Model, fileServerPath, tempFileName);
                    string virtualPath = string.Format("{0}{1}", filePath, tempFileName);
                    SaveEntity(payorEdi270Setting);

                    Edi270271File ediFileLog = new Edi270271File();
                    ediFileLog.FileType = Edi270271FileType.FileType_270.ToString();
                    ediFileLog.FileName = tempFileName;
                    ediFileLog.FilePath = virtualPath;
                    ediFileLog.FileSize =
                        Common.GetFileSizeInBytes(HttpContext.Current.Server.MapCustomPath(ediFileLog.FilePath)).ToString();
                    ediFileLog.PayorIDs = model.AddProcess270Model.PayorIDs;
                    ediFileLog.ReferralStatusIDs = String.Join(",", addProcess270Model.ReferralStatusIDs.Select(x => x.ToString()).ToArray());

                    ediFileLog.ServiceIDs = model.AddProcess270Model.ServiceIDs;
                    ediFileLog.Name = addProcess270Model.Name;
                    ediFileLog.EligibilityCheckDate = addProcess270Model.EligibilityCheckDate;

                    #region amazonefileupload

                    //AmazonFileUpload amazoneFileUpload = new AmazonFileUpload();
                    //string fullpath = HttpContext.Current.Server.MapCustomPath(ediFileLog.FilePath);
                    //if (ediFileLog.FilePath != null)
                    //    ediFileLog.FilePath = amazoneFileUpload.UploadFile(ConfigSettings.ZarephathBucket,
                    //        ediFileLog.FilePath.TrimStart('/'), fullpath, true);

                    #endregion amazonefileupload

                    SaveObject(ediFileLog, loggedInUserId);

                    serviceResponse = GetEdi270FileList(searchEdiFileLogListPage, pageIndex, pageSize, sortIndex,
                        sortDirection);
                    serviceResponse.IsSuccess = true;
                    serviceResponse.Message = Resource.EligibilityCheck270Generated;
                }
                catch (Exception ex)
                {
                    serviceResponse.IsSuccess = false;
                    serviceResponse.Message = Resource.ErrorOccured;
                }
            }
            else
            {
                serviceResponse.IsSuccess = false;
                serviceResponse.Message = Resource.SorryNoClientFound;
            }


            return serviceResponse;

        }

        public ServiceResponse HC_GetEdi270FileList(SearchProcess270ListPage searchProcess270Model, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                if (searchProcess270Model != null)
                    HC_SetSearchFilterForEdi270FilesListPage(searchProcess270Model, searchList);
                Page<ListEdi270FileLogModel01> listEdiFilesLogModel = GetEntityPageList<ListEdi270FileLogModel01>(StoredProcedure.HC_GetEdi270271FileList, searchList, pageSize,
                                                              pageIndex, sortIndex, sortDirection);
                response.Data = listEdiFilesLogModel;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.EdiFileLog), Resource.ExceptionMessage);
            }
            return response;
        }

        private static void HC_SetSearchFilterForEdi270FilesListPage(SearchProcess270ListPage searchEdiFileLogListPage, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "FileType", Value = Convert.ToString(Edi270271FileType.FileType_270) });
            searchList.Add(new SearchValueData { Name = "FileName", Value = searchEdiFileLogListPage.FileName });
            searchList.Add(new SearchValueData { Name = "Comment", Value = searchEdiFileLogListPage.Comment });
            searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchEdiFileLogListPage.PayorID) });
            searchList.Add(new SearchValueData { Name = "ServiceID", Value = Convert.ToString(searchEdiFileLogListPage.ServiceID) });
            searchList.Add(new SearchValueData { Name = "ClientName", Value = Convert.ToString(searchEdiFileLogListPage.Name) });
            searchList.Add(new SearchValueData { Name = "EligibilityCheckDate", Value = Convert.ToString(searchEdiFileLogListPage.EligibilityCheckDate) });
            searchList.Add(new SearchValueData { Name = "Upload271FileProcessStatus", Value = Convert.ToString(searchEdiFileLogListPage.Upload271FileProcessStatus) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchEdiFileLogListPage.IsDeleted) });
            searchList.Add(new SearchValueData() { Name = "AllServiceText", Value = Resource.AllServicesText });
        }

        public ServiceResponse HC_DeleteEdi270File(SearchProcess270ListPage searchProcess270Model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

                HC_SetSearchFilterForEdi270FilesListPage(searchProcess270Model, searchList);
                if (!string.IsNullOrEmpty(searchProcess270Model.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchProcess270Model.ListOfIdsInCSV });
                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                searchList.Add(new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(loggedInUserID) });

                List<ListEdi270FileLogModel> totalData = GetEntityList<ListEdi270FileLogModel>(StoredProcedure.HC_DeleteEdi270271Files, searchList);
                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListEdi270FileLogModel> getEdiFilesLogList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);


                response.Data = getEdiFilesLogList;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.EdiFileLog);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }

        #endregion

        #region Process 271

        public ServiceResponse HC_Upload271File(AddProcess271Model model, HttpRequestBase httpRequestBase, long loggedInUserId)
        {
            var response = new ServiceResponse();
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.Edi271FileLog);
            string basePath = String.Format(_cacheHelper.EdiFile271DownLoadPath, _cacheHelper.Domain);
            HttpPostedFileBase file = httpRequestBase.Files[0];
            response = Common.SaveFile(file, basePath);

            if (response.IsSuccess)
            {

                Edi270271File ediFileLog = new Edi270271File();
                ediFileLog.FileType = Edi270271FileType.FileType_271.ToString();
                ediFileLog.FileName = ((UploadedFileModel)response.Data).FileOriginalName; ;
                ediFileLog.FilePath = ((UploadedFileModel)response.Data).TempFilePath;
                ediFileLog.FileSize = Common.GetFileSizeInBytes(HttpContext.Current.Server.MapCustomPath(ediFileLog.FilePath)).ToString();
                ediFileLog.Comment = model.Comment;

                //AmazonFileUpload amazonFileUpload = new AmazonFileUpload();

                #region  EDI 271 to CSV 271 Parser

                if (string.IsNullOrEmpty(ediFileLog.FilePath))
                {
                    response.Message = Resource.ErrorOccured;
                    return response;
                }
                try
                {
                    Edi271 edi271 = new Edi271();
                    string ediFilePath = ediFileLog.FilePath;

                    if (ediFileLog.FilePath.StartsWith("/"))
                        ediFilePath = HttpContext.Current.Server.MapCustomPath(ediFilePath);
                    else
                        ediFilePath = HttpContext.Current.Server.MapCustomPath("/" + ediFilePath);


                    string ediFile271CsvDownLoadPath = string.Format(_cacheHelper.EdiFile271CsvDownLoadPath, _cacheHelper.Domain);
                    string newRedablefilePath = string.Format("{0}{1}{2}", ediFile271CsvDownLoadPath, Guid.NewGuid(), Constants.FileExtension_Csv);

                    Edi271ResponseModel edi271ResponseModel = edi271.GenerateEdi271Model(ediFilePath, HttpContext.Current.Server.MapCustomPath(newRedablefilePath), newRedablefilePath);
                    ediFileLog.ReadableFilePath = edi271ResponseModel.GeneratedFileRelativePath;

                    //ediFileLog.ReadableFilePath = amazonFileUpload.UploadFile(ConfigSettings.ZarephathBucket,
                    //                                    edi271ResponseModel.GeneratedFileRelativePath.TrimStart('/'), edi271ResponseModel.GeneratedFileAbsolutePath, true);
                    ediFileLog.Upload271FileProcessStatus = (int)EnumUpload835FileProcessStatus.Processed;
                }
                catch (Exception ex)
                {
                    string fileVirtualPath = Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.Edi835FileName, logpath);
                    ediFileLog.ReadableFilePath = fileVirtualPath;
                    //string errorFullpath = HttpContext.Current.Server.MapCustomPath(fileVirtualPath);
                    //if (fileVirtualPath != null)
                    //    ediFileLog.ReadableFilePath = amazonFileUpload.UploadFile(ConfigSettings.ZarephathBucket, fileVirtualPath.TrimStart('/'), errorFullpath, true);
                    ediFileLog.Upload271FileProcessStatus = (int)EnumUpload835FileProcessStatus.ErrorInProcess;
                }//GetScalar(StoredProcedure.UpdateBatchAfter835FileProcessed);


                #endregion

                #region Amazon File Upload

                //string fullpath = HttpContext.Current.Server.MapCustomPath(ediFileLog.FilePath);
                //if (ediFileLog.FilePath != null)
                //    ediFileLog.FilePath = amazonFileUpload.UploadFile(ConfigSettings.ZarephathBucket, ediFileLog.FilePath.TrimStart('/'), fullpath, true);

                #endregion amazonefileupload

                SaveObject(ediFileLog, loggedInUserId);

                response.Message = Resource.Edi271FileUploaded;
            }
            return response;
        }



        public ServiceResponse HC_GetEdi271FileList(SearchProcess271ListPage searchProcess271Model, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                if (searchProcess271Model != null)
                    HC_SetSearchFilterForEdi271FilesListPage(searchProcess271Model, searchList);
                Page<ListEdi271FileLogModel01> listEdiFilesLogModel = GetEntityPageList<ListEdi271FileLogModel01>(StoredProcedure.HC_GetEdi270271FileList, searchList, pageSize,
                                                              pageIndex, sortIndex, sortDirection);
                response.Data = listEdiFilesLogModel;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.EdiFileLog), Resource.ExceptionMessage);
            }
            return response;
        }

        private static void HC_SetSearchFilterForEdi271FilesListPage(SearchProcess271ListPage searchEdiFileLogListPage, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "FileType", Value = Convert.ToString(Edi270271FileType.FileType_271) });
            searchList.Add(new SearchValueData { Name = "FileName", Value = searchEdiFileLogListPage.FileName });
            searchList.Add(new SearchValueData { Name = "Comment", Value = searchEdiFileLogListPage.Comment });
            searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchEdiFileLogListPage.PayorID) });
            searchList.Add(new SearchValueData { Name = "ServiceID", Value = Convert.ToString(searchEdiFileLogListPage.ServiceID) });
            searchList.Add(new SearchValueData { Name = "ClientName", Value = Convert.ToString(searchEdiFileLogListPage.Name) });
            searchList.Add(new SearchValueData { Name = "EligibilityCheckDate", Value = Convert.ToString(searchEdiFileLogListPage.EligibilityCheckDate) });
            searchList.Add(new SearchValueData { Name = "Upload271FileProcessStatus", Value = Convert.ToString(searchEdiFileLogListPage.Upload271FileProcessStatus) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchEdiFileLogListPage.IsDeleted) });
            searchList.Add(new SearchValueData() { Name = "AllServiceText", Value = Resource.AllServicesText });

        }

        public ServiceResponse HC_DeleteEdi271File(SearchProcess271ListPage searchProcess271Model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

                HC_SetSearchFilterForEdi271FilesListPage(searchProcess271Model, searchList);
                if (!string.IsNullOrEmpty(searchProcess271Model.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchProcess271Model.ListOfIdsInCSV });
                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                searchList.Add(new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(loggedInUserID) });

                List<ListEdi271FileLogModel> totalData = GetEntityList<ListEdi271FileLogModel>(StoredProcedure.HC_DeleteEdi270271Files, searchList);
                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListEdi271FileLogModel> getEdiFilesLogList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);


                response.Data = getEdiFilesLogList;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.EdiFileLog);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }


        #endregion


        #endregion

        #region ClaimMDAPI

        
        private List<BatchUploadedClaimMessageModel> BuildBatchUploadedClaimMessagModel(ClaimMDResponse data)
        {
            return data.result.UploadedClaims.Select(c => new BatchUploadedClaimMessageModel()
            {
                LastResponseID = data.result.e_last_responseid,
                BatchID = c.e_batchid,
                Bill_NPI = c.e_bill_npi,
                Bill_TaxID = c.e_bill_taxid,
                ClaimID = c.e_claimid,
                ClaimMD_ID = c.e_claimmd_id,
                FDOS = c.e_fdos,
                FileID = c.e_fileid,
                FileName = c.e_filename,
                INS_Number = c.e_ins_number,
                PayerID = c.e_payerid,
                PCN = c.e_pcn,
                Remote_ClaimID = c.e_remote_claimid,
                Sender_ICN = c.e_sender_icn,
                Sender_Name = c.e_sender_name,
                SenderID = c.e_senderid,
                Status = c.e_status,
                Total_Charge =c.e_total_charge,
                Messages= Convert.ToString(c.messages),
                CreatedDate = DateTime.Now,
            }).ToList();
        }

        public ServiceResponse SyncClaimMessages(bool isSyncAllowed = false,string syncall="")
        {
            ServiceResponse res = new ServiceResponse();
            try
            {
                ClaimMDMessageCheckModel model = GetEntity<ClaimMDMessageCheckModel>(StoredProcedure.HC_GetResponseIDForBatchUploadedClaimMessages, null);
                if (!string.IsNullOrEmpty(syncall) && syncall.ToLower() == "syncall")
                    model.LastResponseID = 0;

                if (model.ServiceCallAllowed || isSyncAllowed)
                {

                    for (int i = 0; i < 10; i++)
                    {

                        ClaimMDApiHelper _helper = new ClaimMDApiHelper();
                        ClaimMDResponse apiRes = _helper.ClaimMessagesRequest(model.LastResponseID);

                        if (apiRes != null && apiRes.result != null && apiRes.result.claim != null)
                        {

                            List<BatchUploadedClaimMessageModel> sqlmodel = BuildBatchUploadedClaimMessagModel(apiRes);
                            DataTable dataTbl = Common.ListToDataTable(sqlmodel);
                            MyEzcareOrganization orgData = GetOrganizationConnectionString();
                            if (orgData != null)
                            {
                                SqlConnection con = new SqlConnection(orgData.CurrentConnectionString);
                                con.Open();
                                SqlCommand cmd = new SqlCommand(StoredProcedure.HC_SaveBatchUploadedClaimMessageTable, con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                var pList = new SqlParameter("@claimObject", SqlDbType.Structured);
                                pList.TypeName = "dbo.BatchUploadedClaimMessages";
                                pList.Value = dataTbl;
                                cmd.Parameters.Add(pList);

                                if (cmd.ExecuteNonQuery() <= 0)
                                {
                                    
                                }
                            }










                            /*
                            foreach (var claim in apiRes.result.UploadedClaims)
                            {
                                try
                                {
                                    List<SearchValueData> searchData = new List<SearchValueData>();
                                    searchData.Add(new SearchValueData("LastResponseID", Convert.ToString(apiRes.result.e_last_responseid)));
                                    searchData.Add(new SearchValueData("BatchID", Convert.ToString(claim.e_batchid)));
                                    searchData.Add(new SearchValueData("Bill_NPI", Convert.ToString(claim.e_bill_npi)));
                                    searchData.Add(new SearchValueData("Bill_TaxID", Convert.ToString(claim.e_bill_taxid)));
                                    searchData.Add(new SearchValueData("ClaimID", Convert.ToString(claim.e_claimid)));
                                    searchData.Add(new SearchValueData("ClaimMD_ID", Convert.ToString(claim.e_claimmd_id)));
                                    searchData.Add(new SearchValueData("FDOS", Convert.ToString(claim.e_fdos)));
                                    searchData.Add(new SearchValueData("FileID", Convert.ToString(claim.e_fileid)));
                                    searchData.Add(new SearchValueData("FileName", Convert.ToString(claim.e_filename)));
                                    searchData.Add(new SearchValueData("INS_Number", Convert.ToString(claim.e_ins_number)));
                                    searchData.Add(new SearchValueData("PayerID", Convert.ToString(claim.e_payerid)));
                                    searchData.Add(new SearchValueData("PCN", Convert.ToString(claim.e_pcn)));
                                    searchData.Add(new SearchValueData("Remote_ClaimID", Convert.ToString(claim.e_remote_claimid)));
                                    searchData.Add(new SearchValueData("Sender_ICN", Convert.ToString(claim.e_sender_icn)));
                                    searchData.Add(new SearchValueData("Sender_Name", Convert.ToString(claim.e_sender_name)));
                                    searchData.Add(new SearchValueData("SenderID", Convert.ToString(claim.e_senderid)));
                                    searchData.Add(new SearchValueData("Status", Convert.ToString(claim.e_status)));
                                    searchData.Add(new SearchValueData("Total_Charge", Convert.ToString(claim.e_total_charge)));
                                    searchData.Add(new SearchValueData("Messages", Convert.ToString(claim.messages)));

                                    GetScalar(StoredProcedure.HC_SaveBatchUploadedClaimMessage, searchData);

                                    

                                }
                                catch (Exception ex) { }
                            }*/


                        }
                        else {
                            break;
                        }

                        model = GetEntity<ClaimMDMessageCheckModel>(StoredProcedure.HC_GetResponseIDForBatchUploadedClaimMessages, null);
                    }
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                res.IsSuccess = false;
                res.Message = ex.Message.ToString();
            }
            return res;
        }




        public ServiceResponse GetClaimMessageList(ClaimModel model)
        {
            ServiceResponse res = new ServiceResponse();
            try
            {

                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "BatchID", Value = Convert.ToString(model.BatchId) });
                searchList.Add(new SearchValueData { Name = "ClaimID", Value = Convert.ToString(model.ClaimUniqueTraceID) });
                searchList.Add(new SearchValueData { Name = "Total_Charge", Value = Convert.ToString(model.CalculatedAmount) });

                List<BatchUploadedClaimMessage> listMessages = GetEntityList<BatchUploadedClaimMessage>(StoredProcedure.HC_GetBatchUploadedClaimMessages, searchList);

                List<ClaimMessageModel> resList = new List<ClaimMessageModel>();
                foreach (var item in listMessages)
                {
                    List<ClaimMessageModel> tempResList = new List<ClaimMessageModel>();
                    string jsonconvertMsg = item.Messages;
                    var tokenMsg = JToken.Parse(jsonconvertMsg);
                    if (tokenMsg is JArray)
                    {
                        tempResList = JsonConvert.DeserializeObject<List<ClaimMessageModel>>(jsonconvertMsg).ToList();
                    }
                    else if (tokenMsg is JObject)
                    {
                        ClaimMessageModel msg = JsonConvert.DeserializeObject<ClaimMessageModel>(jsonconvertMsg);
                        tempResList.Add(msg);
                    }

                    foreach (var newitem in tempResList)
                    {
                        newitem.Sender_Name = item.Sender_Name;
                    }
                    if (tempResList.Count > 0)
                        resList.AddRange(tempResList);
                }

                res.Data = resList;
                res.IsSuccess = true;

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                res.IsSuccess = false;
                res.Message = ex.Message.ToString();
            }
            return res;
        }











        public ServiceResponse UploadClaims(BatchValidationResponseModel claimModel, long loggedInUserId)
        {
            ServiceResponse res = new ServiceResponse();
            try
            {
                ClaimMDApiHelper _helper = new ClaimMDApiHelper();
                ClaimMDResponse apiRes = _helper.UploadClaims(claimModel);
                if (apiRes != null && apiRes.result != null)
                {
                    if (apiRes.result.error != null)
                    {
                        res.IsSuccess = false;
                        res.Message = apiRes.result.error.e_error_mesg;
                    }
                    else
                    {
                        res.IsSuccess = true;
                        res.Message = apiRes.result.e_messages;

                        var logFileName = "ClaimSummary_" + DateTime.Now.TimeOfDay.Ticks.ToString();
                        string logpath = ConfigSettings.LogPath + "ClaimLogs/";
                        Common.CreateLogFile(Newtonsoft.Json.JsonConvert.SerializeObject(apiRes.result), logFileName, logpath);

                        int ClaimCount = 0;
                        if (apiRes.result.UploadedClaim != null)
                        {
                            ClaimCount = 1;
                            SaveUploadedClaimAndErrors(0, apiRes.result.UploadedClaim, ClaimCount);
                        }
                        else if (apiRes.result.UploadedClaims != null)
                        {
                            ClaimCount = apiRes.result.UploadedClaims.Count;
                            foreach (var item in apiRes.result.UploadedClaims)
                            {
                                SaveUploadedClaimAndErrors(0, item, ClaimCount);
                            }
                        }
                        else
                        {
                            res.IsSuccess = false;
                        }

                        if (res.IsSuccess)
                        {

                            SearchBatchList searchBatchList = new SearchBatchList();
                            searchBatchList.ListOfIdsInCSV = Convert.ToString(claimModel.BatchID);
                            searchBatchList.IsSent = true;
                            HC_MarkAsSentBatch(loggedInUserId, searchBatchList);
                            SyncClaimMessages(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                res.IsSuccess = false;
                res.Message = ex.Message.ToString();
            }
            return res;
        }

        public void SaveUploadedClaimAndErrors(long BatchUploadedClaimID, UC_Claims claim, int ClaimCount)
        {
            if (claim != null)
            {
                List<SearchValueData> searchData = new List<SearchValueData>();
                searchData.Add(new SearchValueData("BatchUploadedClaimID", Convert.ToString(BatchUploadedClaimID)));
                searchData.Add(new SearchValueData("BatchID", Convert.ToString(claim.e_batchid)));
                searchData.Add(new SearchValueData("Bill_NPI", Convert.ToString(claim.e_bill_npi)));
                searchData.Add(new SearchValueData("Bill_TaxID", Convert.ToString(claim.e_bill_taxid)));
                searchData.Add(new SearchValueData("ClaimID", Convert.ToString(claim.e_claimid)));
                searchData.Add(new SearchValueData("ClaimMD_ID", Convert.ToString(claim.e_claimmd_id)));
                //searchData.Add(new SearchValueData("MessageID", Convert.ToString(claim.E_Message.e_mesgid)));
                searchData.Add(new SearchValueData("Message", Convert.ToString(claim.messages)));
                //searchData.Add(new SearchValueData("MessageStatus", Convert.ToString(claim.E_Message.e_status)));
                searchData.Add(new SearchValueData("FDOS", Convert.ToString(claim.e_fdos)));
                searchData.Add(new SearchValueData("FileID", Convert.ToString(claim.e_fileid)));
                searchData.Add(new SearchValueData("FileName", Convert.ToString(claim.e_filename)));
                searchData.Add(new SearchValueData("INS_Number", Convert.ToString(claim.e_ins_number)));
                searchData.Add(new SearchValueData("PayerID", Convert.ToString(claim.e_payerid)));
                searchData.Add(new SearchValueData("PCN", Convert.ToString(claim.e_pcn)));
                searchData.Add(new SearchValueData("Remote_ClaimID", Convert.ToString(claim.e_remote_claimid)));
                searchData.Add(new SearchValueData("Sender_ICN", Convert.ToString(claim.e_sender_icn)));
                searchData.Add(new SearchValueData("Sender_Name", Convert.ToString(claim.e_sender_name)));
                searchData.Add(new SearchValueData("SenderID", Convert.ToString(claim.e_senderid)));
                searchData.Add(new SearchValueData("Status", Convert.ToString(claim.e_status)));
                searchData.Add(new SearchValueData("Total_Charge", Convert.ToString(claim.e_total_charge)));
                searchData.Add(new SearchValueData("LoggedInUserId", Convert.ToString(SessionHelper.LoggedInID)));
                searchData.Add(new SearchValueData("CurrentDate", Convert.ToString(Common.GetOrgCurrentDateTime())));
                searchData.Add(new SearchValueData("SystemID", HttpContext.Current.Request.UserHostAddress));

                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "BatchID", Value = Convert.ToString(claim.e_batchid) });
                List<HC_ListOverViewFileModel> listOverViewFileModel = GetEntityList<HC_ListOverViewFileModel>(StoredProcedure.HC_GetOverviewFileList, searchList);
                if (listOverViewFileModel != null && listOverViewFileModel.Count > 0)
                {
                    HC_ListOverViewFileModel claimDetails = listOverViewFileModel.FirstOrDefault();
                    searchData.Add(new SearchValueData("ReferralID", Convert.ToString(claimDetails.ReferralID)));
                    searchData.Add(new SearchValueData("PatientName", Convert.ToString(claimDetails.ClientName)));
                    searchData.Add(new SearchValueData("Payer", Convert.ToString(claimDetails.BatchPayorName)));
                    searchData.Add(new SearchValueData("BillingProvider", Convert.ToString(claimDetails.BillingProviderName)));
                }

                BatchUploadedClaim uploadedClaimModel = GetEntity<BatchUploadedClaim>(StoredProcedure.HC_SaveBatchUploadedClaim, searchData);

                //If Any Error Meesages
                SaveUploadedClaimsErrors(uploadedClaimModel.BatchUploadedClaimID, claim);

                SaveUploadedClaimFile(uploadedClaimModel.BatchUploadedClaimID, claim, ClaimCount);
            }
        }
        public void SaveUploadedClaimsErrors(long BatchUploadedClaimID, UC_Claims UploadedClaim)
        {
            if (BatchUploadedClaimID > 0 && UploadedClaim.messages != null)
            {
                string jsonconvert = JsonConvert.SerializeObject(UploadedClaim.messages);
                var token = JToken.Parse(jsonconvert);
                if (token is JArray)
                {
                    UploadedClaim.MessagesList = JsonConvert.DeserializeObject<List<UC_Message>>(jsonconvert).ToList();
                    foreach (UC_Message msg in UploadedClaim.MessagesList)
                    {
                        AddErrorMessage(BatchUploadedClaimID, msg);
                    }
                }
                else if (token is JObject)
                {
                    UploadedClaim.Message = JsonConvert.DeserializeObject<UC_Message>(jsonconvert);
                    AddErrorMessage(BatchUploadedClaimID, UploadedClaim.Message);
                }
            }
        }
        public void AddErrorMessage(long BatchUploadedClaimID, UC_Message msg)
        {
            List<SearchValueData> searchData = new List<SearchValueData>();
            searchData.Add(new SearchValueData("BatchUploadedClaimID", Convert.ToString(BatchUploadedClaimID)));
            searchData.Add(new SearchValueData("Field", Convert.ToString(msg.e_fields)));
            searchData.Add(new SearchValueData("MsgID", Convert.ToString(msg.e_mesgid)));
            searchData.Add(new SearchValueData("Message", Convert.ToString(msg.e_message)));
            searchData.Add(new SearchValueData("Status", Convert.ToString(msg.e_status)));
            BatchUploadedClaimErrors uploadedClaimErrorModel = GetEntity<BatchUploadedClaimErrors>(StoredProcedure.HC_SaveBatchUploadedClaimErrors, searchData);
        }

        public void SaveUploadedClaimFile(long BatchUploadedClaimID, UC_Claims UploadedClaim, int ClaimCount)
        {
            if (BatchUploadedClaimID > 0 && !string.IsNullOrEmpty(UploadedClaim.e_filename))
            {
                List<SearchValueData> searchData = new List<SearchValueData>();
                searchData.Add(new SearchValueData("ClaimMD_FileID", Convert.ToString(UploadedClaim.e_fileid)));
                searchData.Add(new SearchValueData("FileName", Convert.ToString(UploadedClaim.e_filename)));
                searchData.Add(new SearchValueData("Claims", Convert.ToString(ClaimCount)));
                searchData.Add(new SearchValueData("Amount", Convert.ToString(UploadedClaim.e_total_charge)));
                searchData.Add(new SearchValueData("BatchUploadedClaimID", Convert.ToString(BatchUploadedClaimID)));
                searchData.Add(new SearchValueData("ClaimMD_ID", Convert.ToString(UploadedClaim.e_claimmd_id)));
                BatchUploadedClaimFiles uploadedClaimFileModel = GetEntity<BatchUploadedClaimFiles>(StoredProcedure.HC_SaveBatchUploadedClaimFile, searchData);
            }
        }

        public ServiceResponse GetUploadedClaims(ListManageClaimsModel searchClaimListModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "ReferralID", Value = Convert.ToString(searchClaimListModel.ReferralID)},
                        new SearchValueData {Name = "BatchID", Value = Convert.ToString(searchClaimListModel.BatchID)},
                        new SearchValueData {Name = "PayorID", Value = Convert.ToString(searchClaimListModel.PayorID)},
                        new SearchValueData {Name = "INS_Number", Value = Convert.ToString(searchClaimListModel.INS_Number)},
                        new SearchValueData {Name = "StartDate", Value = !string.IsNullOrEmpty(searchClaimListModel.StartDate) ? Convert.ToDateTime(searchClaimListModel.StartDate).ToString(Constants.DbDateFormat) : null},
                        new SearchValueData {Name = "EndDate", Value = !string.IsNullOrEmpty(searchClaimListModel.EndDate) ? Convert.ToDateTime(searchClaimListModel.EndDate).ToString(Constants.DbDateFormat) : null }
                     };
                Page<ListManageClaimsModel> listBatchUploadedClaims = GetEntityPageList<ListManageClaimsModel>(StoredProcedure.HC_GetBatchUploadedClaims, searchParam, pageSize,
                                                              pageIndex, sortIndex, sortDirection);

                response.Data = listBatchUploadedClaims;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.EdiFileLog), Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_AddManageClaim()
        {
            var response = new ServiceResponse();
            AddManageClaimModel addManageClaimModel = GetMultipleEntity<AddManageClaimModel>(StoredProcedure.HC_AddManageClaim);
            response.Data = addManageClaimModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetLatestERA(long loggedInId)
        {
            var response = new ServiceResponse();
            try
            {
                ClaimMDApiHelper helper = new ClaimMDApiHelper();

                ServiceResponse claimMDResponse = new ServiceResponse();
                claimMDResponse = helper.GetLatestERA();

                if (!claimMDResponse.IsSuccess) {
                    return claimMDResponse;
                }

                var data = (ERAMDResponse)claimMDResponse.Data;

                if (data != null && data.result != null && data.result.era != null && data.result.era.Count > 0)
                {
                    var model = BuildLatestERAModel(loggedInId, data);
                    string systemId = System.Web.HttpContext.Current.Request.UserHostAddress;

                    DataTable dataTbl = Common.ListToDataTable(model);
                    MyEzcareOrganization orgData = GetOrganizationConnectionString();
                    if (orgData != null)
                    {
                        SqlConnection con = new SqlConnection(orgData.CurrentConnectionString);
                        con.Open();
                        SqlCommand cmd = new SqlCommand(StoredProcedure.HC_AddLatestERA, con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Source", "Claim.md");
                        var pList = new SqlParameter("@list", SqlDbType.Structured);
                        pList.TypeName = "dbo.LatestERAs";
                        pList.Value = dataTbl;
                        cmd.Parameters.Add(pList);

                        if (cmd.ExecuteNonQuery() <= 0)
                        {
                            response.IsSuccess = false;
                            response.Message = "No data fetched.";
                            return response;
                        }
                    }
                }
                response.Message = "Data fetched successfully.";
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }

        private List<HC_AddLatestERAModel> BuildLatestERAModel(long loggedInId, ERAMDResponse data)
        {
            return data.result.era.Select(x => new HC_AddLatestERAModel()
            {
                CheckNumber = x.e_check_number,
                CheckType = x.e_check_type,
                ClaimProviderName = x.e_claimmd_prov_name,
                DownTime = x.e_download_time,
                EraID = x.e_eraid,
                PaidAmount = Convert.ToDecimal(x.e_paid_amount),
                PaidDate = x.e_paid_date,
                PayerID = x.e_payerid,
                PayerName = x.e_payer_name,
                ProviderName = x.e_prov_name,
                ProviderTaxID = x.e_prov_taxid,
                ProviderNPI = x.e_prov_npi,
                RecievedTime = !string.IsNullOrEmpty(x.e_received_time) ? Convert.ToDateTime(x.e_received_time) : default(DateTime),
                CreatedBy = loggedInId,
                CreatedDate = DateTime.Now,
                UpdatedBy = loggedInId,
                UpdatedDate = DateTime.Now,
                IsDeleted = false
            }).ToList();
        }

        public ServiceResponse GetLatestERAPDF(string eraId)
        {
            var response = new ServiceResponse();
            try
            {
                ClaimMDApiHelper helper = new ClaimMDApiHelper();
                response.Data = helper.GetLatestERAPDF(eraId);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.EdiFileLog), Resource.ExceptionMessage);
            }
            return response;
        }


        public ServiceResponse GetLatestERA835(string eraId)
        {
            var response = new ServiceResponse();
            try
            {
                ClaimMDApiHelper helper = new ClaimMDApiHelper();
                response.Data = helper.GetLatestERA835(eraId);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }

        public ServiceResponse ProcessERA835(string eraId, long loggedInUserID, bool forceProcess=false)
        {
            var response = new ServiceResponse();
            try
            {


                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "ERA_ID", Value = eraId });
                object data = GetScalar(StoredProcedure.HC_ValdiateAndUpdateERA835ProcessStatus, searchList);

                if ((int)data == -1 && forceProcess==false)
                {
                    HC_BackEndProcessUpload835File();
                    response.Message = "This ERA is already processed. Please check Upload 835 ERA page for more details.";
                }
                else
                {
                    ClaimMDApiHelper helper = new ClaimMDApiHelper();
                    ERAResponse eraResponse = helper.GetLatestERA835(eraId);


                    if (eraResponse == null || string.IsNullOrEmpty(eraResponse.result.data) || string.IsNullOrEmpty(eraResponse.result.era_payerid))
                    {
                        response.Message = string.Format("Unable to fetch ERA data at this time. Please contact admin.");
                        //return response;
                    }
                    else
                    {

                        searchList = new List<SearchValueData>();
                        searchList.Add(new SearchValueData { Name = "PayorIdentificationNumber", Value = eraResponse.result.era_payerid });
                        searchList.Add(new SearchValueData { Name = "PayorSubmissionName", Value = eraResponse.result.era_payer_name });
                        //searchList.Add(new SearchValueData { Name = "PayorNPINumber", Value = eraResponse.result.prov_npi });
                        Payor payor = GetEntity<Payor>(StoredProcedure.HC_GetPayorFromERA835Response, searchList);

                        if (payor == null || payor.PayorID == 0)
                        {
                            response.Message = string.Format("Payor is not mapped in our system for this ERA. Payor ID = {0} and PayorSubmissionName= {1}. Please confirm Payor details.", eraResponse.result.era_payerid, eraResponse.result.era_payer_name);
                            // return response;
                        }
                        else
                        {
                            Upload835File model = new Upload835File();
                            model.PayorID = payor.PayorID;
                            model.EraID = eraId;
                            model.FileName = "ERA_835_" + eraId + ".txt";
                            model.Comment = model.FileName;
                            HC_SaveLatestERA835File(model, eraResponse.result.data, loggedInUserID);
                            HC_BackEndProcessUpload835File();
                            response.IsSuccess = true;
                            response.Message = "ERA is processed successfully.";
                        }
                    }
                }


                searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "EraID", Value = eraId });
                searchList.Add(new SearchValueData { Name = "ValidationMessage", Value = response.Message });
                GetScalar(StoredProcedure.HC_SetERAValidationMessage, searchList);

                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }







        public ServiceResponse HC_SaveLatestERA835File(Upload835File model, string fileContent, long loggedInUserID)
        {
            var response = new ServiceResponse();
            string basePath = String.Format(_cacheHelper.EdiFile835DownLoadPath, _cacheHelper.Domain) + model.PayorID + "/";
            try
            {
                var fileResponse = Common.SaveFileContent(fileContent, basePath, model.FileName);

                if (fileResponse.IsSuccess)
                {

                    Upload835File upload835File = new Upload835File();
                    upload835File.PayorID = model.PayorID;
                    upload835File.FileName = ((UploadedFileModel)fileResponse.Data).FileOriginalName;
                    upload835File.FilePath = ((UploadedFileModel)fileResponse.Data).TempFilePath;
                    upload835File.Comment = model.Comment == "null" ? null : model.Comment;
                    upload835File.IsProcessed = true;
                    upload835File.Upload835FileProcessStatus = (int)EnumUpload835FileProcessStatus.InProcess;
                    upload835File.A835TemplateType = "Edi_File";
                    upload835File.EraID = model.EraID;
                    //upload835File.FileSize = Common.GetFileSizeInBytes(HttpContext.Current.Server.MapCustomPath(upload835File.FilePath)).ToString();
                    upload835File.FileSize = Common.GetFileSizeInBytes(Common.HttpContext_Current_Server_MapPath(upload835File.FilePath)).ToString();

                    SaveObject(upload835File, loggedInUserID);

                    response.IsSuccess = true;
                    response.Message = Resource._835FileUploaded;
                }

                response.Message = Resource.ExceptionMessage;
            }
            catch
            {
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }





        public ServiceResponse ArchieveClaim(string claimId)
        {
            var response = new ServiceResponse();
            try
            {
                ClaimMDApiHelper helper = new ClaimMDApiHelper();
                response.Data = helper.ArchieveClaim(claimId);
                response.IsSuccess = true;
                response.Message = Common.MessageWithTitle(Resource.ERAUpdate, Resource.SuccessArchieve);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.EdiFileLog), Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse GetClaimErrorsList(long BatchUploadedClaimID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "BatchUploadedClaimID", Value = Convert.ToString(BatchUploadedClaimID) });
                List<HC_ListClaimErrors> listClaimErrors = GetEntityList<HC_ListClaimErrors>(StoredProcedure.HC_GetBatchUploadedClaimErrors, searchList);
                response.Data = listClaimErrors;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.EdiFileLog), Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse GetClaimErrorsListAndCMS1500(long BatchUploadedClaimID, EDIFileSearchModel EDIFileSearchModellong, long loggedInId)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();

                // Get Batch List
                searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "BatchID", Value = Convert.ToString(EDIFileSearchModellong.BatchID) });
                List<ListBatchModel> batchList = GetEntityList<ListBatchModel>(StoredProcedure.HC_GetBatchByBatchID, searchList);

                // Get Claim Errors
                searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "BatchUploadedClaimID", Value = Convert.ToString(BatchUploadedClaimID) });
                List<HC_ListClaimErrors> listClaimErrors = GetEntityList<HC_ListClaimErrors>(StoredProcedure.HC_GetBatchUploadedClaimErrors, searchList);

                // Get Claim CMS1500
                ServiceResponse res = GenerateCMS1500(EDIFileSearchModellong, SessionHelper.IsCaseManagement, loggedInId);

                ClaimErrorsAndCMS1500 model = new ClaimErrorsAndCMS1500();
                model.ClaimErrors = listClaimErrors;
                model.CMS1500 = res.Data;
                model.BatchList = batchList;

                response.Data = model;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.EdiFileLog), Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse SaveCMS1500Data(SaveCMS1500Modal saveCMS1500Modal, GenerateEdiFileModel cmsModel)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "BatchID", Value = Convert.ToString(saveCMS1500Modal.BatchID) });
                searchList.Add(new SearchValueData { Name = "NoteID", Value = Convert.ToString(saveCMS1500Modal.NoteID) });
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(saveCMS1500Modal.ReferralID) });

                searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(saveCMS1500Modal.PayorID) });
                searchList.Add(new SearchValueData { Name = "PayorIdentificationNumber", Value = Convert.ToString(saveCMS1500Modal.PayorIdentificationNumber) });
                searchList.Add(new SearchValueData { Name = "PayorName", Value = Convert.ToString(saveCMS1500Modal.PayorName) });

                searchList.Add(new SearchValueData { Name = "AHCCCSID", Value = Convert.ToString(saveCMS1500Modal.AHCCCSID) });

                searchList.Add(new SearchValueData { Name = "ContactID", Value = Convert.ToString(saveCMS1500Modal.ContactID) });
                searchList.Add(new SearchValueData { Name = "PatientAddress", Value = Convert.ToString(saveCMS1500Modal.PatientAddress) });
                searchList.Add(new SearchValueData { Name = "PatientCity", Value = Convert.ToString(saveCMS1500Modal.PatientCity) });
                searchList.Add(new SearchValueData { Name = "PatientState", Value = Convert.ToString(saveCMS1500Modal.PatientState) });
                searchList.Add(new SearchValueData { Name = "PatientZipCode", Value = Convert.ToString(saveCMS1500Modal.PatientZipCode) });

                searchList.Add(new SearchValueData { Name = "PatientDOB", Value = Convert.ToString(saveCMS1500Modal.PatientDOB) });


                searchList.Add(new SearchValueData { Name = "PhysicianID", Value = Convert.ToString(saveCMS1500Modal.PhysicianID) });
                searchList.Add(new SearchValueData { Name = "Ref_NPI", Value = Convert.ToString(saveCMS1500Modal.Ref_NPI) });

                searchList.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(saveCMS1500Modal.ReferralBillingAuthorizationID) });
                searchList.Add(new SearchValueData { Name = "AuthorizationCode", Value = Convert.ToString(saveCMS1500Modal.AuthorizationCode) });

                searchList.Add(new SearchValueData { Name = "ServiceDate", Value = Convert.ToString(saveCMS1500Modal.ServiceDate == "--" ? string.Empty : saveCMS1500Modal.ServiceDate) });
                searchList.Add(new SearchValueData { Name = "PlaceOfServiceID", Value = Convert.ToString(saveCMS1500Modal.PlaceOfServiceID) });
                searchList.Add(new SearchValueData { Name = "PlaceOfService", Value = Convert.ToString(saveCMS1500Modal.PlaceOfService) });
                searchList.Add(new SearchValueData { Name = "ModifierID1", Value = Convert.ToString(saveCMS1500Modal.Mod1_1_ID) });
                searchList.Add(new SearchValueData { Name = "ModifierCode1", Value = Convert.ToString(saveCMS1500Modal.Mod1_1_Code) });
                searchList.Add(new SearchValueData { Name = "ModifierID2", Value = Convert.ToString(saveCMS1500Modal.Mod2_1_ID) });
                searchList.Add(new SearchValueData { Name = "ModifierCode2", Value = Convert.ToString(saveCMS1500Modal.Mod2_1_Code) });
                searchList.Add(new SearchValueData { Name = "ModifierID3", Value = Convert.ToString(saveCMS1500Modal.Mod3_1_ID) });
                searchList.Add(new SearchValueData { Name = "ModifierCode3", Value = Convert.ToString(saveCMS1500Modal.Mod3_1_Code) });
                searchList.Add(new SearchValueData { Name = "ModifierID4", Value = Convert.ToString(saveCMS1500Modal.Mod4_1_ID) });
                searchList.Add(new SearchValueData { Name = "ModifierCode4", Value = Convert.ToString(saveCMS1500Modal.Mod4_1_Code) });

                searchList.Add(new SearchValueData { Name = "BillingProviderEIN", Value = Convert.ToString(saveCMS1500Modal.BillingProviderEIN) });

                searchList.Add(new SearchValueData { Name = "BillingProviderNPI", Value = Convert.ToString(saveCMS1500Modal.BillingProviderNPI) });

                GetEntityList<HC_ListClaimErrors>(StoredProcedure.HC_SaveCMS1500Data, searchList);
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.EdiFileLog), Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse GetNpiDetails()
        {
            ServiceResponse response = new ServiceResponse();
            var searchlist = new List<SearchValueData>
            {
            };
            GetLatestERAModel totalData = GetMultipleEntity<GetLatestERAModel>(StoredProcedure.GetNpiDetails, searchlist);
            response.IsSuccess = true;
            response.Data = totalData;

            return response;
        }

        #endregion

        #endregion

        public ServiceResponse HC_UpdateBatchReconcile(long BatchNoteID, float PaidAmount, long ClaimStatusID, long ClaimStatusCodeID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "BatchNoteID", Value = Convert.ToString(BatchNoteID) });
                searchList.Add(new SearchValueData { Name = "PaidAmount", Value = Convert.ToString(PaidAmount) });
                searchList.Add(new SearchValueData { Name = "ClaimStatusID", Value = Convert.ToString(ClaimStatusID) });
                searchList.Add(new SearchValueData { Name = "ClaimStatusCodeID", Value = Convert.ToString(ClaimStatusCodeID) });

                GetScalar(StoredProcedure.HC_UpdateBatchReconcile, searchList);
                response.IsSuccess = true;
                response.Message = Common.MessageWithTitle(Resource.Success, Resource.BatchReconcileUpdated);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.EdiFileLog), Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_SaveBillingNote(BillingNoteModel BillingNoteModel, long LoggedInID)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData>();

            searchParam.Add(new SearchValueData { Name = "BillingNoteID", Value = Convert.ToString(BillingNoteModel.BillingNoteID) });
            searchParam.Add(new SearchValueData { Name = "BillingNote", Value = Convert.ToString(BillingNoteModel.BillingNote) });
            searchParam.Add(new SearchValueData { Name = "BatchID", Value = Convert.ToString(BillingNoteModel.BatchID) });
            searchParam.Add(new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(LoggedInID) });
            List<BillingNoteModel> model = GetEntityList<BillingNoteModel>("AddBillingNote", searchParam);

            response.Data = model;
            response.IsSuccess = true;
            return response;

        }
        public ServiceResponse HC_GetBillingNote(long BatchID)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData>();
            searchParam.Add(new SearchValueData { Name = "BatchID", Value = Convert.ToString(BatchID) });

            List<BillingNoteModel> model = GetEntityList<BillingNoteModel>("GetBillingNote", searchParam);

            response.Data = model;
            response.IsSuccess = true;
            return response;


        }

        public ServiceResponse HC_UpdateBillingNote(BillingNoteModel BillingNoteModel, long LoggedInID)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData>();

            searchParam.Add(new SearchValueData { Name = "BillingNoteID", Value = Convert.ToString(BillingNoteModel.BillingNoteID) });
            searchParam.Add(new SearchValueData { Name = "BillingNote", Value = Convert.ToString(BillingNoteModel.BillingNote) });
            searchParam.Add(new SearchValueData { Name = "BatchID", Value = Convert.ToString(BillingNoteModel.BatchID) });
            searchParam.Add(new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(LoggedInID) });
            List<BillingNoteModel> model = GetEntityList<BillingNoteModel>("AddBillingNote", searchParam);
            response.Data = model;
            response.IsSuccess = true;
            return response;

        }

        public ServiceResponse HC_DeleteBillingNote(string BillingNoteID, long BatchID)
        {
            ServiceResponse response = new ServiceResponse();
            if (BillingNoteID.Length > 0)
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();

                searchParam.Add(new SearchValueData { Name = "BillingNoteID", Value = Convert.ToString(BillingNoteID) });
                searchParam.Add(new SearchValueData { Name = "BatchID", Value = Convert.ToString(BatchID) });

                List<BillingNoteModel> model = GetEntityList<BillingNoteModel>("DeleteBillingNote", searchParam);

                response.IsSuccess = true;
                response.Message = "Billing Note Deleted Successfully";
                response.Data = model;
            }
            else
                response.Message = "Billing Note Failed";
            return response;
        }


        #region AR Aging Report

        public ServiceResponse SetARAgingReportPage()
        {
            var response = new ServiceResponse();

            SetARAgingReportPage model = GetMultipleEntity<SetARAgingReportPage>(StoredProcedure.HC_SetARAgingReportPage);
            response.IsSuccess = true;
            //SetARAgingReportPage data = new SetARAgingReportPage();
            //model.SearchARAgingReportPage.StrReconcileStatus = Constants.Denied + "," + Constants.ReconcileStatus_NA;
            response.Data = model;//WE'LL ADD IF ANYTHIGN COME IN FUTURE FOR THIS REPORT. FOR NOW THERE WILL BE NO DATA REQUIRE ON PAGE LOAD
            return response;
        }

        public ServiceResponse GetARAgingReport(SearchARAgingReportPage model)
        {
            var response = new ServiceResponse();
            response.IsSuccess = true;

            var searchlist = new List<SearchValueData>
            {
                new SearchValueData {Name = "ReconcileStatus", Value = model.StrReconcileStatus},
                new SearchValueData {Name = "PayorIDs", Value = model.StrPayorIDs},
                new SearchValueData {Name = "ClientName", Value = model.ClientName},
            };

            List<ListARAgingReportModel> listArAgingReportModels = GetEntityList<ListARAgingReportModel>(StoredProcedure.HC_GetARAgingReport, searchlist);
            listArAgingReportModels = GetARAgingReportModelWithFooterRow(listArAgingReportModels);

            response.Data = listArAgingReportModels;//WE'LL ADD IF ANYTHIGN COME IN FUTURE FOR THIS REPORT. FOR NOW THERE WILL BE NO DATA REQUIRE ON PAGE LOAD
            return response;
        }

        private List<ListARAgingReportModel> GetARAgingReportModelWithFooterRow(List<ListARAgingReportModel> listArAgingReportModels)
        {
            if (listArAgingReportModels != null && listArAgingReportModels.Count > 0)
            {
                ListARAgingReportModel footerRow = new ListARAgingReportModel();
                footerRow.AllActivePayorIDs = listArAgingReportModels.First().AllActivePayorIDs;
                footerRow.PayorShortName = Resource.Total;
                footerRow.PendingAmount0_60 = listArAgingReportModels.Sum(c => c.PendingAmount0_60);
                footerRow.PendingAmount61_90 = listArAgingReportModels.Sum(c => c.PendingAmount61_90);
                footerRow.PendingAmount91_120 = listArAgingReportModels.Sum(c => c.PendingAmount91_120);
                footerRow.PendingAmount121_180 = listArAgingReportModels.Sum(c => c.PendingAmount121_180);
                footerRow.PendingAmount181_270 = listArAgingReportModels.Sum(c => c.PendingAmount181_270);
                footerRow.PendingAmount271_365 = listArAgingReportModels.Sum(c => c.PendingAmount271_365);
                footerRow.TotalPendingAmount = listArAgingReportModels.Sum(c => c.TotalPendingAmount);
                footerRow.IsFooterRow = true;
                listArAgingReportModels.Add(footerRow);
                return listArAgingReportModels;
            }

            return listArAgingReportModels;
        }

        public ServiceResponse ExportARAgingReportList(SearchARAgingReportPage model)
        {
            ServiceResponse response = new ServiceResponse();
            var searchlist = new List<SearchValueData>
            {
                new SearchValueData {Name = "ReconcileStatus", Value = model.StrReconcileStatus},
            };

            List<ListARAgingReportModel> totalData = GetEntityList<ListARAgingReportModel>(StoredProcedure.HC_GetARAgingReport, searchlist);
            totalData = GetARAgingReportModelWithFooterRow(totalData);
            if (totalData.Count > 0)
            {
                string fileName = string.Format("{0}_{1}", Constants.ReportName_ARAgingReport, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));

                string basePath = HttpContext.Current.Server.MapPath(ConfigSettings.ReportExcelFilePath);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", ConfigSettings.ReportExcelFilePath, fileName, Constants.Extention_xlsx);
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                CreateExcelFile.CreateExcelDocument(totalData, downloadFileModel.AbsolutePath);
                response.IsSuccess = true;
                response.Data = downloadFileModel;
                response.IsSuccess = true;
            }
            return response;
        }

        #endregion



    }
}
