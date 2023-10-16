using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;
using System.IO;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using ExportToExcel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class NoteDataProvider : BaseDataProvider, INoteDataProvider
    {
        #region Note

        public ServiceResponse SetNoteListPage(long referralID, long? loggedInUserID = null, long? assigneeID = null)
        {
            ServiceResponse response = new ServiceResponse();

            SetNoteListModel setNoteListModel = GetMultipleEntity<SetNoteListModel>(StoredProcedure.SetNoteListPage, new List<SearchValueData> { new SearchValueData { Name = "ReferralID", Value = referralID.ToString() } });
            setNoteListModel.NoteTypes = Common.SetNoteTypes();
            setNoteListModel.CompletedFilter = Common.SetNoteCompletedFilter();
            setNoteListModel.DeleteFilter = Common.SetDeleteFilter();
            setNoteListModel.NoteKinds = Common.SetNoteKind();
            setNoteListModel.SearchNoteListModel.IsBillable = -1;
            setNoteListModel.SearchNoteListModel.ServiceCodeTypeID = -1;
            setNoteListModel.SearchNoteListModel.PayorID = -1;
            setNoteListModel.SearchNoteListModel.CreatedByIDs = Convert.ToString(loggedInUserID);
            setNoteListModel.SearchNoteListModel.AssigneeID = assigneeID ?? 0;
            setNoteListModel.SearchNoteListModel.IsDeleted = 0;
            setNoteListModel.SearchNoteListModel.IsCompleted = -1;
            setNoteListModel.SearchNoteListModel.ReferralID = referralID;

            setNoteListModel.Assignees.Add(new NameValueData() { Name = "No Staff", Value = -2 });
            setNoteListModel.Assignees.AddRange(setNoteListModel.Employees);


            response.Data = setNoteListModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse SetAddNote(long referralID, long loggedInUserID, long noteID = 0)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = referralID.ToString()},
                    new SearchValueData {Name = "NoteID", Value = noteID.ToString()},
                    new SearchValueData {Name = "PrimaryContactTypeID", Value = ((int)ContactType.ContactTypes.Primary_Placement).ToString()},
                    new SearchValueData {Name = "LoggedInUserID", Value = loggedInUserID.ToString()}
                };

            NoteDetailModel noteDetail = GetMultipleEntity<NoteDetailModel>(StoredProcedure.SetAddNotePage, searchParam);

            if (noteDetail.Note.BillingProviderID == null && noteDetail.DefaultProviders != null)
                noteDetail.Note.BillingProviderID = noteDetail.DefaultProviders.DefaultBillingProviderID;
            if (noteDetail.Note.RenderingProviderID == null && noteDetail.DefaultProviders != null)
                noteDetail.Note.RenderingProviderID = noteDetail.DefaultProviders.DefaultRenderingProviderID;

            //noteDetail.Note.Signature = noteDetail.EmpSignatureDetails;

            noteDetail.Note.EmpSignatureDetails = noteDetail.EmpSignatureDetails;

            if (noteDetail.Note.NoteID == 0)
            {
                noteDetail.Note.NoteAssignee = loggedInUserID;
                //noteDetail.Note.NoteComments = Resource.NoteAssignedForReview;
            }

            if (noteDetail.NotePayor != null)
            {
                noteDetail.Note.PayorID = noteDetail.NotePayor.NotePayorID;
                noteDetail.Note.PayorShortName = noteDetail.NotePayor.NotePayorName;
            }


            if ((noteDetail.Note.ServiceCodeID.HasValue && noteDetail.Note.ServiceCodeID > 0) && (noteDetail.Note.ModifierID.HasValue && noteDetail.Note.ModifierID > 0))
            {
                noteDetail.Note.ServiceCode = String.Format("{0} - {1}", noteDetail.Note.ServiceCode, GetModifiersName.GetName(noteDetail.Note.ModifierID.Value));
            }

            #region Set TempNoteList

            foreach (var temonote in noteDetail.TempNoteList)
            {
                if (temonote.SelectedServiceCodeForPayor == null)
                    temonote.SelectedServiceCodeForPayor = new PosDropdownModel();

                temonote.SelectedServiceCodeForPayor.UnitType = temonote.UnitType;
                temonote.SelectedServiceCodeForPayor.PerUnitQuantity = temonote.PerUnitQuantity;
                temonote.SelectedServiceCodeForPayor.PayorServiceCodeMappingID = temonote.PayorServiceCodeMappingID;
                temonote.SelectedServiceCodeForPayor.CalculatedUnit = temonote.CalculatedUnit;
                temonote.SelectedServiceCodeForPayor.PosID = Convert.ToInt16(temonote.PosID);

                if (temonote.UnitType == (int)EnumUnitType.Time)
                {
                    if (temonote.StartTime != null && temonote.EndTime != null)
                    {

                        double timeDifference = temonote.EndTime.Value.Subtract(temonote.StartTime.Value).TotalMinutes;
                        temonote.SelectedServiceCodeForPayor.MinutesDiff = Convert.ToInt32(timeDifference);
                        temonote.SelectedServiceCodeForPayor.CalculatedUnit = temonote.CalculatedUnit;
                    }
                }
                else
                {
                    if (temonote.StartMile.HasValue && temonote.EndMile.HasValue)
                    {
                        long mileDifference = temonote.EndMile.Value - temonote.StartMile.Value;
                        temonote.SelectedServiceCodeForPayor.MileDiff = mileDifference;
                        temonote.SelectedServiceCodeForPayor.CalculatedUnit = temonote.CalculatedUnit;
                    }
                }


                if ((temonote.ServiceCodeID.HasValue && temonote.ServiceCodeID > 0) && (temonote.ModifierID.HasValue && temonote.ModifierID > 0))
                {
                    temonote.ServiceCode = String.Format("{0} - {1}", temonote.ServiceCode, GetModifiersName.GetName(temonote.ModifierID.Value));
                }
            }
            #endregion



            noteDetail.Services = Common.SetNoteServices();
            noteDetail.Relations = Common.SetNoteRelations();
            noteDetail.KindOfNotes = Common.SetNoteKind();

            response.Data = noteDetail;
            response.IsSuccess = true;

            return response;
        }

        public ServiceResponse GetDtrDetails(string term, string type, int pageSize)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "Term", Value = term.Trim()},
                    new SearchValueData {Name = "Type", Value = type.Trim()},
                    new SearchValueData {Name = "PageSize", Value = pageSize.ToString()},
                };

            List<string> dtrDetailsDataModel = GetStringList(StoredProcedure.GetDTRDetailsList, searchParam);
            //dtrDetailsDataModel.First().ToString()
            //List<string> listString=new List<string>();
            //foreach (var model in dtrDetailsDataModel)
            //{
            //    listString.Add(model.ToString());
            //}

            // listString = (List<string>) dtrDetailsDataModel.SelectMany(d => d as List<string> ?? new List<string>() { d });
            response.Data = dtrDetailsDataModel;
            return response;
        }

        public ServiceResponse ExportNoteList(SearchNoteListModel searchNoteModel, string sortIndex = "", string sortDirection = "")
        {
            CacheHelper _cacheHelper = new CacheHelper();
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            if (searchNoteModel != null)
                SetSearchFilterForNoteListPage(searchNoteModel, searchList);
            searchList.AddRange(Common.SetPagerValues(0, 0, sortIndex, sortDirection));

            List<ExportNoteList> totalData = GetEntityList<ExportNoteList>(StoredProcedure.ExportNoteList, searchList);

            if (totalData.Count > 0)
            {
                string fileName = string.Format("{0}_{1}", Constants.ReportName_ClientNotes, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath,_cacheHelper.Domain);

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

        public ServiceResponse GetNoteList(SearchNoteListModel searchNoteModel, int pageIndex = 1, int pageSize = 10,
                                           string sortIndex = "", string sortDirection = "")
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchNoteModel != null)
                SetSearchFilterForNoteListPage(searchNoteModel, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<NoteList> totalData = GetEntityList<NoteList>(StoredProcedure.GetNoteList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<NoteList> noteList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = noteList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForNoteListPage(SearchNoteListModel searchNoteModel, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(searchNoteModel.AHCCCSID))
                searchList.Add(new SearchValueData { Name = "AHCCCSID", Value = Convert.ToString(searchNoteModel.AHCCCSID) });

            if (!string.IsNullOrEmpty(searchNoteModel.SearchText))
                searchList.Add(new SearchValueData { Name = "SearchText", Value = Convert.ToString(searchNoteModel.SearchText) });

            if (!string.IsNullOrEmpty(searchNoteModel.NoteKind))
                searchList.Add(new SearchValueData { Name = "NoteKind", Value = Convert.ToString(searchNoteModel.NoteKind) });

            searchList.Add(new SearchValueData { Name = "CISNumber", Value = Convert.ToString(searchNoteModel.CISNumber) });

            if (searchNoteModel.ReferralID > 0)
                searchList.Add(new SearchValueData
                    {
                        Name = "ReferralID",
                        Value = Convert.ToString(searchNoteModel.ReferralID)
                    });
            if (searchNoteModel.BatchID > 0)
                searchList.Add(new SearchValueData
                    {
                        Name = "BatchID",
                        Value = Convert.ToString(searchNoteModel.BatchID)
                    });

            if (searchNoteModel.NoteID > 0)
                searchList.Add(new SearchValueData
                    {
                        Name = "NoteID",
                        Value = Convert.ToString(searchNoteModel.NoteID)
                    });


            searchList.Add(new SearchValueData
            {
                Name = "ServiceCodeTypeID",
                Value = Convert.ToString(searchNoteModel.ServiceCodeTypeID)
            });


            searchList.Add(new SearchValueData
            {
                Name = "PayorID",
                Value = Convert.ToString(searchNoteModel.PayorID)
            });


            searchList.Add(new SearchValueData { Name = "BillingProviderID", Value = Convert.ToString(searchNoteModel.BillingProviderID) });
            searchList.Add(new SearchValueData { Name = "RegionID", Value = Convert.ToString(searchNoteModel.RegionID) });
            searchList.Add(new SearchValueData { Name = "DepartmentID", Value = Convert.ToString(searchNoteModel.DepartmentID) });
            //searchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(searchNoteModel.EmployeeID) });

            searchList.Add(new SearchValueData
                {
                    Name = "AllowEditStatuses",
                    Value = string.Join(",", new List<String>
                        {
                            ((int) EnumBatchNoteStatus.Addded).ToString(),
                            ((int) EnumBatchNoteStatus.MarkAsUnSent).ToString(),
                            ((int) EnumBatchNoteStatus.Declined).ToString(),
                            ((int) EnumBatchNoteStatus.ValidateFailed).ToString(),
                        })
                });


            if (searchNoteModel.ServiceDateStart.HasValue)
            {
                searchList.Add(new SearchValueData
                {
                    Name = "ServiceDateStart",
                    Value = searchNoteModel.ServiceDateStart.Value.Date.ToString(Constants.DbDateFormat)
                });
            }


            if (searchNoteModel.ServiceDateEnd.HasValue)
            {
                searchList.Add(new SearchValueData
                {
                    Name = "ServiceDateEnd",
                    Value = searchNoteModel.ServiceDateEnd.Value.Date.ToString(Constants.DbDateFormat)
                });
            }

            searchList.Add(new SearchValueData
            {
                Name = "IsCompleted",
                Value = Convert.ToString(searchNoteModel.IsCompleted)
            });


            searchList.Add(new SearchValueData
            {
                Name = "IsBillable",
                Value = Convert.ToString(searchNoteModel.IsBillable)
            });

            //if (searchNoteModel.ServiceCodeID > 0)
            //{
            //    searchList.Add(new SearchValueData{Name = "ServiceCodeID",Value = Convert.ToString(searchNoteModel.ServiceCodeID)});
            //}


            searchList.Add(new SearchValueData { Name = "ServiceCodeIDs", Value = Convert.ToString(searchNoteModel.ServiceCodeIDs) });
            searchList.Add(new SearchValueData { Name = "CreatedByIDs", Value = Convert.ToString(searchNoteModel.CreatedByIDs) });
            searchList.Add(new SearchValueData { Name = "AssigneeID", Value = Convert.ToString(searchNoteModel.AssigneeID) });


            searchList.Add(new SearchValueData
            {
                Name = "IsDeleted",
                Value = Convert.ToString(searchNoteModel.IsDeleted)
            });

        }

        public ServiceResponse DeleteNote(SearchNoteListModel searchNoteModel, int pageIndex, int pageSize,
                                          string sortIndex, string sortDirection, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForNoteListPage(searchNoteModel, searchList);

            if (!string.IsNullOrEmpty(searchNoteModel.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchNoteModel.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInId) });



            //DateTime getDate;
            //if (DateTime.Now.Month >= ConfigSettings.ResetRespiteUsageMonth)
            //    getDate = new DateTime(DateTime.Now.Year, ConfigSettings.ResetRespiteUsageMonth, ConfigSettings.ResetRespiteUsageDay);
            //else
            //{
            //    DateTime lastYear = DateTime.Today.AddYears(-(ConfigSettings.ResetRespiteUsageDay));
            //    getDate = new DateTime(lastYear.Year, ConfigSettings.ResetRespiteUsageMonth, ConfigSettings.ResetRespiteUsageDay);
            //}

            //searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(getDate).ToString(Constants.DbDateFormat) });
            //searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(getDate.AddYears(ConfigSettings.ResetRespiteUsageDay).AddDays(-(ConfigSettings.ResetRespiteUsageDay))).ToString(Constants.DbDateFormat) });




            List<NoteList> totalData = GetEntityList<NoteList>(StoredProcedure.DeleteNote, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            //if (count == 0)
            //{
            //    response.Message = Common.MessageWithTitle(string.Format(Resource.DeleteFailed, Resource.Referral), Resource.ReferralDependencyExistMessage);
            //    return response;
            //}

            Page<NoteList> noteList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);

            response.Data = noteList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Note);

            return response;
        }

        public ServiceResponse SaveNote(Note note, long referralID, PosDropdownModel selectedServiceCodeDetail, List<DXCodeMappingList> dxCodes = null, long loggedInUserID = 0, string loggedInName = null)
        {
            ServiceResponse response = new ServiceResponse();
            if (referralID > 0)
            {
                Referral referral = GetEntity<Referral>(referralID);

                #region INTERNAL MESSASGE SET
                if (note.IsIssue && note.IssueAssignID.HasValue)
                {
                    ReferralInternalMessage referralInternalMessage = new ReferralInternalMessage();
                    if (note.IssueID > 0)
                    {
                        var seachList = new List<SearchValueData>();
                        seachList.Add(new SearchValueData
                        {
                            Name = "ReferralInternalMessageID",
                            Value = note.IssueID.ToString()
                        });
                        referralInternalMessage = GetEntity<ReferralInternalMessage>(seachList) ??
                                                  new ReferralInternalMessage();
                    }

                    referralInternalMessage.Note = note.NoteDetails;
                    referralInternalMessage.Assignee = note.IssueAssignID.Value;
                    referralInternalMessage.IsDeleted = false;
                    referralInternalMessage.ReferralID = referral.ReferralID;
                    referralInternalMessage.IsResolved = false;
                    SaveObject(referralInternalMessage, loggedInUserID);

                    note.IssueID = referralInternalMessage.ReferralInternalMessageID;
                }
                #endregion

                note.ReferralID = referralID;
                if (note.NoteID == 0 && note.ServiceCodeType == (int)ServiceCodeType.ServiceCodeTypes.Other && note.StartTime == null)
                {
                    note.StartTime = note.ServiceDate;
                    note.EndTime = note.ServiceDate;
                }

                bool isEditMode = note.NoteID > 0;
                //if (isEditMode && !note.IsDeleted)
                //    CheckRespiteHours(note.ReferralID, note.NoteID, isAddHrsCall: false);


                #region Caluclate unit count

                if (note.ServiceCodeID.HasValue)
                {
                    long tempPayorServiceCodeMappingId = selectedServiceCodeDetail == null ? note.PayorServiceCodeMappingID : selectedServiceCodeDetail.PayorServiceCodeMappingID;

                    List<SearchValueData> payorServiceCodeSearchParam = new List<SearchValueData>
                        {
                            new SearchValueData {Name = "ReferralID", Value = referralID.ToString()},
                            new SearchValueData
                                {
                                    Name = "ServiceDate",
                                    Value = note.ServiceDate.ToString(Constants.DbDateFormat)
                                },
                            new SearchValueData {Name = "ServiceCodeID", Value = note.ServiceCodeID.ToString()},
                            new SearchValueData
                                {
                                    Name = "PayorServiceCodeMappingID",
                                    Value =  tempPayorServiceCodeMappingId.ToString()// selectedServiceCodeDetail.PayorServiceCodeMappingID.ToString()
                                },
                                new SearchValueData {Name = "PosID", Value = Convert.ToString(note.PosID)},
                                new SearchValueData {Name = "PayorID", Value = Convert.ToString(note.PayorID)}
                        };

                    if (note.NoteID > 0)
                    {
                        payorServiceCodeSearchParam.Add(new SearchValueData { Name = "NoteID", Value = note.NoteID.ToString() });
                    }
                    PosDropdownModel tempPayorServiceCodeMapping = GetEntity<PosDropdownModel>(StoredProcedure.GetPOS,
                                                                                               payorServiceCodeSearchParam);


                    if (tempPayorServiceCodeMappingId == 0)
                    {
                        tempPayorServiceCodeMappingId = tempPayorServiceCodeMapping.PayorServiceCodeMappingID;
                        note.PayorServiceCodeMappingID = tempPayorServiceCodeMappingId;
                    }

                    if (selectedServiceCodeDetail == null)
                    //This will be the only Case when we update the Exisitng Note
                    {
                        selectedServiceCodeDetail = new PosDropdownModel();
                        selectedServiceCodeDetail.PerUnitQuantity = tempPayorServiceCodeMapping.PerUnitQuantity;
                        selectedServiceCodeDetail.UnitType = tempPayorServiceCodeMapping.UnitType;
                    }


                    if (selectedServiceCodeDetail.DefaultUnitIgnoreCalculation > 0)
                    {
                        note.CalculatedUnit = selectedServiceCodeDetail.DefaultUnitIgnoreCalculation;
                        tempPayorServiceCodeMapping.CalculatedUnit = selectedServiceCodeDetail.DefaultUnitIgnoreCalculation;
                    }
                    else if (selectedServiceCodeDetail.UnitType == (int)EnumUnitType.Time)
                    {
                        if (note.StartTime != null && note.EndTime != null)
                        {
                            double timeDifference = note.EndTime.Value.Subtract(note.StartTime.Value).TotalMinutes;
                            //tempPayorServiceCodeMapping.CalculatedUnit = (float)Math.Round(timeDifference / selectedServiceCodeDetail.PerUnitQuantity);

                            tempPayorServiceCodeMapping.CalculatedUnit = 1;
                            if (timeDifference > selectedServiceCodeDetail.PerUnitQuantity)
                                timeDifference = timeDifference - selectedServiceCodeDetail.PerUnitQuantity;
                            else
                                timeDifference = 0;

                            tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit +
                                (float)Math.Round(timeDifference / selectedServiceCodeDetail.PerUnitQuantity);



                            //if (tempPayorServiceCodeMapping.CalculatedUnit > tempPayorServiceCodeMapping.AvailableDailyUnit)
                            //{
                            //    tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                            //    note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                            //}
                            if ((tempPayorServiceCodeMapping.DailyUnitLimit == 0 && tempPayorServiceCodeMapping.MaxUnit == 0) || tempPayorServiceCodeMapping.CalculatedUnit <= tempPayorServiceCodeMapping.AvailableDailyUnit)
                            {
                                tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                                note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                            }
                            else if (tempPayorServiceCodeMapping.DailyUnitLimit == 0 &&
                                     tempPayorServiceCodeMapping.MaxUnit != 0)
                            {
                                if (tempPayorServiceCodeMapping.CalculatedUnit <= tempPayorServiceCodeMapping.AvailableMaxUnit)
                                {
                                    note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                                }
                                else
                                {
                                    note.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                                    tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                                }
                            }
                            else
                            {
                                note.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                                tempPayorServiceCodeMapping.CalculatedUnit =
                                    tempPayorServiceCodeMapping.AvailableDailyUnit;
                            }
                        }
                    }
                    else if (selectedServiceCodeDetail.UnitType == (int)EnumUnitType.DistanceInMiles)
                    {
                        if (note.StartMile.HasValue && note.EndMile.HasValue)
                        {
                            long mileDifference = note.EndMile.Value - note.StartMile.Value;
                            //tempPayorServiceCodeMapping.CalculatedUnit = (float)Math.Round(mileDifference / selectedServiceCodeDetail.PerUnitQuantity);


                            tempPayorServiceCodeMapping.CalculatedUnit = 1;
                            if (mileDifference > selectedServiceCodeDetail.PerUnitQuantity)
                                mileDifference = mileDifference - Convert.ToInt16(selectedServiceCodeDetail.PerUnitQuantity);
                            else
                                mileDifference = 0;


                            tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit +
                                (float)Math.Round(mileDifference / selectedServiceCodeDetail.PerUnitQuantity);


                            //if (tempPayorServiceCodeMapping.CalculatedUnit > tempPayorServiceCodeMapping.AvailableDailyUnit)
                            //{
                            //    tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                            //    note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                            //}

                            if ((tempPayorServiceCodeMapping.DailyUnitLimit == 0 && tempPayorServiceCodeMapping.MaxUnit == 0) || tempPayorServiceCodeMapping.CalculatedUnit <= tempPayorServiceCodeMapping.AvailableDailyUnit)
                            {
                                tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                                note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                            }
                            else if (tempPayorServiceCodeMapping.DailyUnitLimit == 0 &&
                                     tempPayorServiceCodeMapping.MaxUnit != 0)
                            {
                                if (tempPayorServiceCodeMapping.CalculatedUnit <= tempPayorServiceCodeMapping.AvailableMaxUnit)
                                {
                                    note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                                }
                                else
                                {
                                    note.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                                    tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                                }
                            }
                            else
                            {
                                note.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                                tempPayorServiceCodeMapping.CalculatedUnit =
                                    tempPayorServiceCodeMapping.AvailableDailyUnit;
                            }

                        }
                    }
                    else if (selectedServiceCodeDetail.UnitType == (int)EnumUnitType.Stop)
                    {
                        note.CalculatedUnit = 1;
                        tempPayorServiceCodeMapping.CalculatedUnit = 1;
                        if (tempPayorServiceCodeMapping.CalculatedUnit > tempPayorServiceCodeMapping.AvailableDailyUnit)
                        {
                            note.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                            tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                        }

                    }


                    if (tempPayorServiceCodeMapping != null)
                    {
                        if ((int)Math.Ceiling(tempPayorServiceCodeMapping.CalculatedUnit) < (int)Math.Ceiling(selectedServiceCodeDetail.CalculatedUnit))
                        {
                            response.Message = Resource.OverLimitMsg;
                            tempPayorServiceCodeMapping.CalculatedUnit = selectedServiceCodeDetail.CalculatedUnit;
                            note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                            //return response;
                        }
                        else if ((int)Math.Ceiling(tempPayorServiceCodeMapping.CalculatedUnit) > (int)Math.Ceiling(selectedServiceCodeDetail.CalculatedUnit))
                        {
                            response.Message = Resource.UnitCountNotMatched;
                            return response;
                        }


                        note.CalculatedAmount = note.CalculatedUnit * tempPayorServiceCodeMapping.Rate;


                    }
                }
                #endregion

                #region Save service code detail in Note table

                if (note.ServiceCodeID.HasValue)
                {
                    ServiceCodes serviceCode = GetEntity<ServiceCodes>(note.ServiceCodeID.Value);
                    note.ServiceCode = serviceCode.ServiceCode;
                    note.ServiceName = serviceCode.ServiceName;
                    note.Description = serviceCode.Description;
                    note.MaxUnit = serviceCode.MaxUnit;
                    note.DailyUnitLimit = serviceCode.DailyUnitLimit;
                    note.IsBillable = serviceCode.IsBillable;
                    note.UnitType = serviceCode.UnitType;
                    note.PerUnitQuantity = serviceCode.PerUnitQuantity;
                    //note.ServiceCodeType = serviceCode.ServiceCodeType;
                    note.ServiceCodeStartDate = serviceCode.ServiceCodeStartDate;
                    note.ServiceCodeEndDate = serviceCode.ServiceCodeEndDate;
                    note.CheckRespiteHours = serviceCode.CheckRespiteHours;


                }



                #endregion

                #region Payor service code mapping in Note table
                if (note.PayorServiceCodeMappingID > 0)
                {
                    PayorServiceCodeMapping payorServiceCodeMapping = GetEntity<PayorServiceCodeMapping>(note.PayorServiceCodeMappingID);
                    note.PayorServiceCodeMappingID = payorServiceCodeMapping.PayorServiceCodeMappingID;
                    note.PayorID = payorServiceCodeMapping.PayorID;
                    note.ModifierID = payorServiceCodeMapping.ModifierID;
                    note.PosID = payorServiceCodeMapping.PosID;
                    note.Rate = payorServiceCodeMapping.Rate;
                    note.POSStartDate = payorServiceCodeMapping.POSStartDate;
                    note.POSEndDate = payorServiceCodeMapping.POSEndDate;
                }

                #endregion

                #region Payor data in Note table

                var payorData = new Payor();
                if (note.PayorID > 0)
                {
                    payorData = GetEntity<Payor>(note.PayorID) ?? new Payor();
                    if (payorData.PayorID > 0)
                    {
                        note.PayorName = payorData.PayorName;
                        note.PayorShortName = payorData.ShortName;
                        note.PayorAddress = payorData.Address;
                        note.PayorIdentificationNumber = payorData.PayorIdentificationNumber;
                        note.PayorCity = payorData.City;
                        note.PayorState = payorData.StateCode;
                        note.PayorZipcode = payorData.ZipCode;
                    }
                }

                #endregion

                #region Billing provider data in Note table
                if (note.BillingProviderID.HasValue)
                {
                    if (payorData.PayorID > 0 && payorData.BillingProviderID.HasValue && note.BillingProviderID != payorData.BillingProviderID && note.IsBillable)
                        note.BillingProviderID = payorData.BillingProviderID;


                    var billingProviderDetail = GetEntity<Facility>(note.BillingProviderID.Value);
                    if (billingProviderDetail != null)
                    {
                        note.BillingProviderName = billingProviderDetail.FacilityName;
                        note.BillingProviderAddress = billingProviderDetail.Address;
                        note.BillingProviderCity = billingProviderDetail.City;
                        note.BillingProviderEIN = billingProviderDetail.EIN;
                        note.BillingProviderGSA = billingProviderDetail.GSA;
                        note.BillingProviderNPI = billingProviderDetail.NPI;
                        note.BillingProviderState = billingProviderDetail.State;
                        note.BillingProviderZipcode = billingProviderDetail.ZipCode;
                        note.BillingProviderAHCCCSID = billingProviderDetail.AHCCCSID;
                    }
                }
                #endregion

                #region Rendering provider data in Note table
                if (note.RenderingProviderID.HasValue)
                {
                    if (payorData.PayorID > 0 && payorData.RenderingProviderID.HasValue && note.RenderingProviderID != payorData.RenderingProviderID && note.IsBillable)
                        note.RenderingProviderID = payorData.RenderingProviderID;

                    var renderingProviderDetail = GetEntity<Facility>(note.RenderingProviderID.Value);
                    if (renderingProviderDetail != null)
                    {
                        note.RenderingProviderName = renderingProviderDetail.FacilityName;
                        note.RenderingProviderAddress = renderingProviderDetail.Address;
                        note.RenderingProviderCity = renderingProviderDetail.City;
                        note.RenderingProviderEIN = renderingProviderDetail.EIN;
                        note.RenderingProviderGSA = renderingProviderDetail.GSA;
                        note.RenderingProviderNPI = renderingProviderDetail.NPI;
                        note.RenderingProviderState = renderingProviderDetail.State;
                        note.RenderingProviderZipcode = renderingProviderDetail.ZipCode;
                        note.RenderingProviderAHCCCSID = renderingProviderDetail.AHCCCSID;
                    }
                }
                #endregion

                if (note.MarkAsComplete)
                {
                    note.SignatureDate = DateTime.UtcNow;
                }
                else
                {
                    note.SignatureDate = null;
                }
                note.ReferralID = referral.ReferralID;
                note.AHCCCSID = referral.AHCCCSID;
                note.CISNumber = referral.CISNumber;

                if (note.NoteAssignee.HasValue)
                {
                    note.NoteAssignedBy = loggedInUserID;
                    note.NoteAssignedDate = DateTime.UtcNow;
                }


                SaveObject(note, loggedInUserID);

                #region Save DTR Related Informations
                List<SearchValueData> searchDtrDetails = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "VehicleNumber", Value = note.VehicleNumber},
                        new SearchValueData {Name = "VehicleType", Value = note.VehicleType},
                        new SearchValueData {Name = "PickUpAddress", Value = note.PickUpAddress},
                        new SearchValueData {Name = "DropOffAddress", Value = note.DropOffAddress}
                    };

                GetScalar(StoredProcedure.CheckDTRDetails, searchDtrDetails);

                #endregion

                #region Additional client detail save like AHCCCSID and CISNumber

                List<SearchValueData> searchParamDxCode = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "ReferralID", Value = referralID.ToString()},
                        new SearchValueData {Name = "NoteID", Value = note.NoteID.ToString()}
                    };

                if (dxCodes != null)
                {
                    searchParamDxCode.Add(new SearchValueData
                        {
                            Name = "ReferralDXCodeMappingID",
                            Value = string.Join(",", dxCodes.Select(m => m.ReferralDXCodeMappingID).ToList())
                        });

                    GetScalar(StoredProcedure.ReSyncNoteDxCodeMappings, searchParamDxCode);
                }

                #endregion

                #region First Dos Code

                //if (note.IsBillable)
                //{
                GetScalar(StoredProcedure.UpdateFirstDos,
                          new List<SearchValueData> { new SearchValueData { Name = "ReferralID", Value = referralID.ToString() } });
                //}
                #endregion

                #region Respite hour code
                CheckRespiteHours(note.ReferralID, note.NoteID, note.ServiceDate);
                #endregion


                #region Signature log related code in Note table
                //if (note.MarkAsComplete)
                //{
                //    Employee employee = GetEntity<Employee>(loggedInUserID);
                //    GetScalar(string.Format("UPDATE SignatureLogs set IsActive=0 where NoteID={0}", note.NoteID));
                //    SignatureLog signatureLog = new SignatureLog
                //        {
                //            Date = DateTime.UtcNow,
                //            NoteID = note.NoteID,
                //            Signature = note.Signature,
                //            EmployeeSignatureID = employee.EmployeeSignatureID,
                //            SignatureBy = loggedInUserID,
                //            Name = loggedInName,
                //            IsActive = true,
                //            MacAddress = Common.GetMAcAddress()
                //        };
                //    SaveObject(signatureLog);
                //}

                if (note.MarkAsComplete)
                {
                    if (note.EmpSignatureDetails.SignatureLogID == 0)
                    {
                        Employee employee = GetEntity<Employee>(note.EmpSignatureDetails.EmployeeID);
                        GetScalar(string.Format("UPDATE SignatureLogs set IsActive=0 where NoteID={0}", note.NoteID));
                        SignatureLog signatureLog = new SignatureLog
                        {
                            Date = DateTime.UtcNow,
                            NoteID = note.NoteID,
                            Signature = note.EmpSignatureDetails.Signature,
                            EmployeeSignatureID = employee.EmployeeSignatureID,
                            SignatureBy = employee.EmployeeID,
                            Name = employee.LastName + ", " + employee.FirstName,
                            IsActive = true,
                            MacAddress = Common.GetMAcAddress()
                        };
                        SaveObject(signatureLog);

                    }
                }
                else
                {
                    GetScalar(string.Format("UPDATE SignatureLogs set IsActive=0 where NoteID={0}", note.NoteID));
                }
                #endregion

                response.IsSuccess = true;
                response.Message = !string.IsNullOrEmpty(response.Message)
                                       ? Resource.NoteSavedSuccessfully + string.Format(" " + response.Message, note.ServiceCode)
                                       : Resource.NoteSavedSuccessfully;
            }
            else
            {
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }

        public ServiceResponse SaveMultiNote(Note tempNote, long referralID, PosDropdownModel selectedServiceCodeDetail, List<DXCodeMappingList> dxCodes, List<Note> tempNoteList, long loggedInUserID = 0, string loggedInName = null)
        {
            ServiceResponse response = new ServiceResponse();
            if (referralID > 0)
            {
                Referral referral = GetEntity<Referral>(referralID);

                var firstOrDefault = tempNoteList.FirstOrDefault(c => !string.IsNullOrEmpty(c.RandomGroupID));
                var randomGroupId = firstOrDefault != null ? firstOrDefault.RandomGroupID : Common.GenerateRandomNumber();

                foreach (var note in tempNoteList)
                {

                    bool isEditMode = note.NoteID > 0;
                    //if (isEditMode && !note.IsDeleted)
                    //    CheckRespiteHours(note.ReferralID, note.NoteID, isAddHrsCall: false);

                    selectedServiceCodeDetail = note.SelectedServiceCodeForPayor;
                    note.RandomGroupID = randomGroupId;
                    //note.GroupIDForMileServices=
                    note.Assessment = tempNote.Assessment;
                    note.ActionPlan = tempNote.ActionPlan;
                    note.Signature = tempNote.Signature;
                    note.MarkAsComplete = tempNote.MarkAsComplete;
                    note.NoteComments = tempNote.NoteComments;
                    note.EmpSignatureDetails = tempNote.EmpSignatureDetails;

                    if (note.NoteID == 0 || note.NoteAssignee != tempNote.NoteAssignee)
                    {
                        note.NoteAssignee = tempNote.NoteAssignee;
                        note.NoteAssignedBy = loggedInUserID;
                        note.NoteAssignedDate = DateTime.UtcNow;
                    }



                    if (note.NoteID == 0 && note.ServiceCodeType == (int)ServiceCodeType.ServiceCodeTypes.Other && note.StartTime == null)
                    {
                        note.StartTime = note.ServiceDate;
                        note.EndTime = note.ServiceDate;
                    }



                    if (note.IsIssue && note.IssueAssignID.HasValue)
                    {
                        ReferralInternalMessage referralInternalMessage = new ReferralInternalMessage();
                        if (note.IssueID > 0)
                        {
                            var seachList = new List<SearchValueData>();
                            seachList.Add(new SearchValueData
                            {
                                Name = "ReferralInternalMessageID",
                                Value = note.IssueID.ToString()
                            });
                            referralInternalMessage = GetEntity<ReferralInternalMessage>(seachList) ??
                                                      new ReferralInternalMessage();
                        }

                        referralInternalMessage.Note = note.NoteDetails;
                        referralInternalMessage.Assignee = note.IssueAssignID.Value;
                        referralInternalMessage.IsDeleted = false;
                        referralInternalMessage.ReferralID = referral.ReferralID;
                        referralInternalMessage.IsResolved = false;
                        SaveObject(referralInternalMessage, loggedInUserID);

                        note.IssueID = referralInternalMessage.ReferralInternalMessageID;
                    }

                    note.ReferralID = referralID;

                    #region Caluclate unit count

                    if (note.ServiceCodeID.HasValue && note.ServiceCodeID.Value > 0)
                    {
                        long tempPayorServiceCodeMappingId = selectedServiceCodeDetail == null ? note.PayorServiceCodeMappingID : selectedServiceCodeDetail.PayorServiceCodeMappingID;

                        List<SearchValueData> payorServiceCodeSearchParam = new List<SearchValueData>
                        {
                            new SearchValueData {Name = "ReferralID", Value = referralID.ToString()},
                            new SearchValueData
                            {
                                Name = "ServiceDate",
                                Value = note.ServiceDate.ToString(Constants.DbDateFormat)
                            },
                            new SearchValueData {Name = "ServiceCodeID", Value = note.ServiceCodeID.ToString()},
                            new SearchValueData {Name = "IsDeleted", Value = "-1"},
                            new SearchValueData
                            {
                                Name = "PayorServiceCodeMappingID",
                                Value = tempPayorServiceCodeMappingId.ToString()//selectedServiceCodeDetail==null?note.PayorServiceCodeMappingID.ToString(): selectedServiceCodeDetail.PayorServiceCodeMappingID.ToString()
                            },
                            new SearchValueData {Name = "PosID", Value = Convert.ToString(note.PosID)},
                            new SearchValueData {Name = "PayorID", Value = Convert.ToString(note.PayorID)}
                        };

                        if (note.NoteID > 0)
                        {
                            payorServiceCodeSearchParam.Add(new SearchValueData { Name = "NoteID", Value = note.NoteID.ToString() });
                            //payorServiceCodeSearchParam.Add(new SearchValueData { Name = "PosID", Value = Convert.ToString(note.PosID) });
                        }
                        PosDropdownModel tempPayorServiceCodeMapping =
                            GetEntity<PosDropdownModel>(StoredProcedure.GetPOS,
                                payorServiceCodeSearchParam);


                        if (tempPayorServiceCodeMappingId == 0)
                        {
                            tempPayorServiceCodeMappingId = tempPayorServiceCodeMapping.PayorServiceCodeMappingID;
                            note.PayorServiceCodeMappingID = tempPayorServiceCodeMappingId;
                        }

                        if (selectedServiceCodeDetail == null)
                        //This will be the only Case when we update the Exisitng Note
                        {
                            selectedServiceCodeDetail = new PosDropdownModel();
                            selectedServiceCodeDetail.PerUnitQuantity = tempPayorServiceCodeMapping.PerUnitQuantity;
                            selectedServiceCodeDetail.UnitType = tempPayorServiceCodeMapping.UnitType;
                        }


                        if (selectedServiceCodeDetail.DefaultUnitIgnoreCalculation > 0)
                        {
                            note.CalculatedUnit = selectedServiceCodeDetail.DefaultUnitIgnoreCalculation;
                            tempPayorServiceCodeMapping.CalculatedUnit = selectedServiceCodeDetail.DefaultUnitIgnoreCalculation;
                        }
                        else if (selectedServiceCodeDetail.UnitType == (int)EnumUnitType.Time)
                        {
                            if (note.StartTime != null && note.EndTime != null)
                            {
                                double timeDifference = note.EndTime.Value.Subtract(note.StartTime.Value).TotalMinutes;


                                tempPayorServiceCodeMapping.CalculatedUnit = 1;
                                if (timeDifference > selectedServiceCodeDetail.PerUnitQuantity)
                                    timeDifference = timeDifference - selectedServiceCodeDetail.PerUnitQuantity;
                                else
                                    timeDifference = 0;

                                tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit +
                                    (float)Math.Round(timeDifference / selectedServiceCodeDetail.PerUnitQuantity);

                                //NOTE Following Calcaution was based on Unit avaibility & we were restircting client to avoid over Unit.
                                //Later client asked to remove Restriction hence code are updated below but logic was at it was. So in future we can chnage if same requirement will come.

                                if (selectedServiceCodeDetail.PayorServiceCodeMappingID == 0)
                                    selectedServiceCodeDetail.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;


                                if ((tempPayorServiceCodeMapping.DailyUnitLimit == 0 &&
                                     tempPayorServiceCodeMapping.MaxUnit == 0) ||
                                    tempPayorServiceCodeMapping.CalculatedUnit <=
                                    tempPayorServiceCodeMapping.AvailableDailyUnit)
                                {
                                    tempPayorServiceCodeMapping.CalculatedUnit =
                                        tempPayorServiceCodeMapping.CalculatedUnit;
                                    note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                                }
                                else if (tempPayorServiceCodeMapping.DailyUnitLimit == 0 &&
                                         tempPayorServiceCodeMapping.MaxUnit != 0)
                                {
                                    if (tempPayorServiceCodeMapping.CalculatedUnit <=
                                        tempPayorServiceCodeMapping.AvailableMaxUnit)
                                    {
                                        note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                                    }
                                    else
                                    {
                                        note.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                                        tempPayorServiceCodeMapping.CalculatedUnit =
                                            tempPayorServiceCodeMapping.AvailableDailyUnit;
                                    }
                                }
                                else
                                {
                                    note.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                                    tempPayorServiceCodeMapping.CalculatedUnit =
                                        tempPayorServiceCodeMapping.AvailableDailyUnit;
                                }
                            }
                        }
                        else if (selectedServiceCodeDetail.UnitType == (int)EnumUnitType.DistanceInMiles)
                        {
                            if (note.StartMile.HasValue && note.EndMile.HasValue)
                            {
                                long mileDifference = note.EndMile.Value - note.StartMile.Value;

                                tempPayorServiceCodeMapping.CalculatedUnit = 1;
                                if (mileDifference > selectedServiceCodeDetail.PerUnitQuantity)
                                    mileDifference = mileDifference - Convert.ToInt16(selectedServiceCodeDetail.PerUnitQuantity);
                                else
                                    mileDifference = 0;


                                tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit +
                                    (float)Math.Round(mileDifference / selectedServiceCodeDetail.PerUnitQuantity);


                                if (selectedServiceCodeDetail.PayorServiceCodeMappingID == 0)
                                    selectedServiceCodeDetail.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;

                                if ((tempPayorServiceCodeMapping.DailyUnitLimit == 0 &&
                                     tempPayorServiceCodeMapping.MaxUnit == 0) ||
                                    tempPayorServiceCodeMapping.CalculatedUnit <=
                                    tempPayorServiceCodeMapping.AvailableDailyUnit)
                                {
                                    tempPayorServiceCodeMapping.CalculatedUnit =
                                        tempPayorServiceCodeMapping.CalculatedUnit;
                                    note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                                }
                                else if (tempPayorServiceCodeMapping.DailyUnitLimit == 0 &&
                                         tempPayorServiceCodeMapping.MaxUnit != 0)
                                {
                                    if (tempPayorServiceCodeMapping.CalculatedUnit <=
                                        tempPayorServiceCodeMapping.AvailableMaxUnit)
                                    {
                                        note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                                    }
                                    else
                                    {
                                        note.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                                        tempPayorServiceCodeMapping.CalculatedUnit =
                                            tempPayorServiceCodeMapping.AvailableDailyUnit;
                                    }
                                }
                                else
                                {
                                    note.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                                    tempPayorServiceCodeMapping.CalculatedUnit =
                                        tempPayorServiceCodeMapping.AvailableDailyUnit;
                                }

                            }
                        }
                        else if (selectedServiceCodeDetail.UnitType == (int)EnumUnitType.Stop)
                        {
                            note.CalculatedUnit = 1;
                            tempPayorServiceCodeMapping.CalculatedUnit = 1;
                            if (tempPayorServiceCodeMapping.CalculatedUnit > tempPayorServiceCodeMapping.AvailableDailyUnit)
                            {
                                note.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                                tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                            }

                        }

                        if (tempPayorServiceCodeMapping != null)
                        {
                            if ((int)Math.Ceiling(tempPayorServiceCodeMapping.CalculatedUnit) <
                                (int)Math.Ceiling(selectedServiceCodeDetail.CalculatedUnit))
                            {
                                response.Message = Resource.OverLimitMsg;
                                tempPayorServiceCodeMapping.CalculatedUnit = selectedServiceCodeDetail.CalculatedUnit;
                                note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                                //return response;
                            }
                            else if ((int)Math.Ceiling(tempPayorServiceCodeMapping.CalculatedUnit) >
                                     (int)Math.Ceiling(selectedServiceCodeDetail.CalculatedUnit))
                            {
                                response.Message = Resource.UnitCountNotMatched;
                                return response;
                            }
                            note.CalculatedAmount = note.CalculatedUnit * tempPayorServiceCodeMapping.Rate;
                        }
                    }
                    else
                    {
                        note.ServiceCode = null;
                        note.ServiceName = null;
                        note.Description = null;
                        note.MaxUnit = 0;
                        note.DailyUnitLimit = 0;
                        note.IsBillable = false;
                        note.UnitType = 0;
                        note.PerUnitQuantity = 0;
                        note.CheckRespiteHours = false;
                        note.ServiceCodeStartDate = null;
                        note.ServiceCodeEndDate = null;
                        note.ServiceCodeID = null;
                        //note.StartTime = null;
                        //note.EndTime = null;
                        note.StartMile = null;
                        note.EndMile = null;
                    }

                    #endregion

                    #region Save service code detail in Note table

                    if (note.ServiceCodeID.HasValue)
                    {
                        ServiceCodes serviceCode = GetEntity<ServiceCodes>(note.ServiceCodeID.Value);
                        note.ServiceCode = serviceCode.ServiceCode;
                        note.ServiceName = serviceCode.ServiceName;
                        note.Description = serviceCode.Description;
                        note.MaxUnit = serviceCode.MaxUnit;
                        note.DailyUnitLimit = serviceCode.DailyUnitLimit;
                        note.IsBillable = serviceCode.IsBillable;
                        note.UnitType = serviceCode.UnitType;
                        note.PerUnitQuantity = serviceCode.PerUnitQuantity;
                        //note.ServiceCodeType = serviceCode.ServiceCodeType;
                        note.ServiceCodeStartDate = serviceCode.ServiceCodeStartDate;
                        note.ServiceCodeEndDate = serviceCode.ServiceCodeEndDate;
                        note.CheckRespiteHours = serviceCode.CheckRespiteHours;
                    }




                    #endregion

                    #region Payor service code mapping in Note table

                    if (note.PayorServiceCodeMappingID > 0)
                    {
                        PayorServiceCodeMapping payorServiceCodeMapping =
                            GetEntity<PayorServiceCodeMapping>(note.PayorServiceCodeMappingID);
                        note.PayorServiceCodeMappingID = payorServiceCodeMapping.PayorServiceCodeMappingID;
                        note.PayorID = payorServiceCodeMapping.PayorID;
                        note.ModifierID = payorServiceCodeMapping.ModifierID;
                        note.PosID = payorServiceCodeMapping.PosID;
                        note.Rate = payorServiceCodeMapping.Rate;
                        note.POSStartDate = payorServiceCodeMapping.POSStartDate;
                        note.POSEndDate = payorServiceCodeMapping.POSEndDate;
                    }

                    #endregion

                    #region Payor data in Note table

                    var payorData = new Payor();
                    if (note.PayorID > 0)
                    {
                        payorData = GetEntity<Payor>(note.PayorID) ?? new Payor();
                        if (payorData.PayorID > 0)
                        {
                            note.PayorName = payorData.PayorName;
                            note.PayorShortName = payorData.ShortName;
                            note.PayorAddress = payorData.Address;
                            note.PayorIdentificationNumber = payorData.PayorIdentificationNumber;
                            note.PayorCity = payorData.City;
                            note.PayorState = payorData.StateCode;
                            note.PayorZipcode = payorData.ZipCode;
                        }
                    }

                    #endregion

                    #region Billing provider data in Note table

                    if (note.BillingProviderID.HasValue)
                    {
                        if (payorData.PayorID > 0 && payorData.BillingProviderID.HasValue &&
                            note.BillingProviderID != payorData.BillingProviderID && note.IsBillable)
                            note.BillingProviderID = payorData.BillingProviderID;


                        var billingProviderDetail = GetEntity<Facility>(note.BillingProviderID.Value);
                        if (billingProviderDetail != null)
                        {
                            note.BillingProviderName = billingProviderDetail.FacilityName;
                            note.BillingProviderAddress = billingProviderDetail.Address;
                            note.BillingProviderCity = billingProviderDetail.City;
                            note.BillingProviderEIN = billingProviderDetail.EIN;
                            note.BillingProviderGSA = billingProviderDetail.GSA;
                            note.BillingProviderNPI = billingProviderDetail.NPI;
                            note.BillingProviderState = billingProviderDetail.State;
                            note.BillingProviderZipcode = billingProviderDetail.ZipCode;
                            note.BillingProviderAHCCCSID = billingProviderDetail.AHCCCSID;
                        }
                    }

                    #endregion

                    #region Rendering provider data in Note table

                    if (note.RenderingProviderID.HasValue)
                    {
                        if (payorData.PayorID > 0 && payorData.RenderingProviderID.HasValue &&
                            note.RenderingProviderID != payorData.RenderingProviderID && note.IsBillable)
                            note.RenderingProviderID = payorData.RenderingProviderID;

                        var renderingProviderDetail = GetEntity<Facility>(note.RenderingProviderID.Value);
                        if (renderingProviderDetail != null)
                        {
                            note.RenderingProviderName = renderingProviderDetail.FacilityName;
                            note.RenderingProviderAddress = renderingProviderDetail.Address;
                            note.RenderingProviderCity = renderingProviderDetail.City;
                            note.RenderingProviderEIN = renderingProviderDetail.EIN;
                            note.RenderingProviderGSA = renderingProviderDetail.GSA;
                            note.RenderingProviderNPI = renderingProviderDetail.NPI;
                            note.RenderingProviderState = renderingProviderDetail.State;
                            note.RenderingProviderZipcode = renderingProviderDetail.ZipCode;
                            note.RenderingProviderAHCCCSID = renderingProviderDetail.AHCCCSID;
                        }
                    }

                    #endregion

                    if (note.MarkAsComplete)
                    {
                        note.SignatureDate = DateTime.UtcNow;
                    }
                    else
                    {
                        note.SignatureDate = null;
                    }
                    note.ReferralID = referral.ReferralID;
                    note.AHCCCSID = referral.AHCCCSID;
                    note.CISNumber = referral.CISNumber;

                    #region Update DTR Detail Set Value Base on Selection

                    if (!note.DTRIsOnline)
                    {
                        //    note.VehicleNumber = note.VehicleNumber;
                        //    note.VehicleType = note.VehicleType;
                        //    note.PickUpAddress = note.PickUpAddress;
                        //    note.DropOffAddress = note.DropOffAddress;
                        //    note.DriverID = note.DriverID;
                        //    note.EscortName = note.EscortName;
                        //    note.Relationship = note.Relationship;
                        //    note.RoundTrip = note.RoundTrip;
                        //    note.OneWay = note.OneWay;
                        //    note.MultiStops = note.MultiStops;
                        //}
                        //else
                        //{
                        note.VehicleNumber = null;
                        note.VehicleType = null;
                        note.PickUpAddress = null;
                        note.DropOffAddress = null;
                        note.EscortName = null;
                        note.Relationship = null;
                        note.DriverID = null;
                        note.RoundTrip = false;
                        note.OneWay = false;
                        note.MultiStops = false;
                    }

                    //note.DTRIsOnline = note.DTRIsOnline;

                    #endregion

                    SaveObject(note, loggedInUserID);

                    #region Save DTR Related Informations

                    List<SearchValueData> searchDtrDetails = new List<SearchValueData>
                        {
                            new SearchValueData {Name = "VehicleNumber", Value = note.VehicleNumber},
                            new SearchValueData {Name = "VehicleType", Value = note.VehicleType},
                            new SearchValueData {Name = "PickUpAddress", Value = note.PickUpAddress},
                            new SearchValueData {Name = "DropOffAddress", Value = note.DropOffAddress}
                        };
                    GetScalar(StoredProcedure.CheckDTRDetails, searchDtrDetails);

                    #endregion

                    #region Additional client detail save like AHCCCSID and CISNumber

                    List<SearchValueData> searchParamDxCode = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "ReferralID", Value = referralID.ToString()},
                        new SearchValueData {Name = "NoteID", Value = note.NoteID.ToString()}
                    };

                    if (dxCodes != null)
                    {
                        searchParamDxCode.Add(new SearchValueData
                        {
                            Name = "ReferralDXCodeMappingID",
                            Value = string.Join(",", dxCodes.Select(m => m.ReferralDXCodeMappingID).ToList())
                        });

                        GetScalar(StoredProcedure.ReSyncNoteDxCodeMappings, searchParamDxCode);
                    }

                    #endregion

                    #region First Dos Code

                    //if (note.IsBillable)
                    //{
                    GetScalar(StoredProcedure.UpdateFirstDos,
                        new List<SearchValueData>
                            {
                                new SearchValueData {Name = "ReferralID", Value = referralID.ToString()}
                            });
                    //}

                    #endregion

                    #region Respite hour code
                    CheckRespiteHours(note.ReferralID, note.NoteID, note.ServiceDate);
                    #endregion

                    #region Signature log related code in Note table

                    if (note.MarkAsComplete)
                    {
                        if (note.EmpSignatureDetails.SignatureLogID == 0)
                        {
                            Employee employee = GetEntity<Employee>(note.EmpSignatureDetails.EmployeeID);
                            GetScalar(string.Format("UPDATE SignatureLogs set IsActive=0 where NoteID={0}", note.NoteID));
                            SignatureLog signatureLog = new SignatureLog
                            {
                                Date = DateTime.UtcNow,
                                NoteID = note.NoteID,
                                Signature = note.EmpSignatureDetails.Signature,
                                EmployeeSignatureID = employee.EmployeeSignatureID,
                                SignatureBy = employee.EmployeeID,
                                Name = employee.LastName + ", " + employee.FirstName,
                                IsActive = true,
                                MacAddress = Common.GetMAcAddress()
                            };
                            SaveObject(signatureLog);

                        }
                    }
                    else
                    {
                        GetScalar(string.Format("UPDATE SignatureLogs set IsActive=0 where NoteID={0}", note.NoteID));
                    }

                    #endregion

                    //response.IsSuccess = true;
                    //response.Message = !string.IsNullOrEmpty(response.Message)
                    //                       ? Resource.NoteSavedSuccessfully + string.Format(" " + response.Message, note.ServiceCode)
                    //                       : Resource.NoteSavedSuccessfully;
                }

                response.IsSuccess = true;
                response.Message = Resource.NoteSavedSuccessfully;

            }
            else
            {
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }


        private void CheckRespiteHours(long referralId, long noteId, DateTime? serviceDate)
        {
            #region Respite hour code

            List<SearchValueData> searchParam = new List<SearchValueData>
                                {
                                    new SearchValueData {Name = "ReferralID", Value = referralId.ToString()},
                                    new SearchValueData {Name = "NoteID", Value = noteId.ToString()},   
                                };
            GetScalar(StoredProcedure.UpdateRespiteHours, searchParam);

            #region SCRAP CODE

            //if (serviceDate != null)
            //{
            //    DateTime selectedDate = serviceDate.Value;

            //    DateTime getDate;
            //    if (selectedDate.Month >= ConfigSettings.ResetRespiteUsageMonth)
            //        getDate = new DateTime(selectedDate.Year, ConfigSettings.ResetRespiteUsageMonth, ConfigSettings.ResetRespiteUsageDay);
            //    else
            //    {
            //        DateTime lastYear = selectedDate.AddYears(-(ConfigSettings.ResetRespiteUsageDay));
            //        getDate = new DateTime(lastYear.Year, ConfigSettings.ResetRespiteUsageMonth, ConfigSettings.ResetRespiteUsageDay);
            //    }

            //    List<SearchValueData> searchParam = new List<SearchValueData>
            //                    {
            //                        new SearchValueData {Name = "StartDate",Value =Convert.ToDateTime(getDate).ToString(Constants.DbDateFormat)},
            //                        new SearchValueData {Name = "EndDate",Value =Convert.ToDateTime( getDate.AddYears(ConfigSettings.ResetRespiteUsageDay).AddDays(-(ConfigSettings.ResetRespiteUsageDay))).ToString(Constants.DbDateFormat)},
            //                        new SearchValueData {Name = "ReferralID", Value = referralId.ToString()},
            //                        new SearchValueData {Name = "NoteID", Value = noteId.ToString()},   
            //                        //new SearchValueData {Name = "IsAddHrsCall", Value = isAddHrsCall?"1":"0"}
            //                    };
            //    GetScalar(StoredProcedure.UpdateRespiteHours, searchParam);
            //}
            #endregion
            #endregion
        }

        public List<ServiceCodes> GetServiceCodes(long referralID, long loggedInId, DateTime serviceDate, int serviceCodeTypeID, int pageSize, string searchText = null)
        {
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = referralID.ToString()},
                    new SearchValueData {Name = "ServiceCodeTypeID", Value = serviceCodeTypeID.ToString()},
                    new SearchValueData {Name = "ServiceDate", Value = serviceDate.ToString(Constants.DbDateFormat)},
                    new SearchValueData {Name = "SearchText", Value = searchText},
                    new SearchValueData {Name = "LoggedInId", Value = loggedInId.ToString()},
                    new SearchValueData {Name = "CredentialBHP", Value = EmployeeCredentialEnum.BHP.ToString()},
                    new SearchValueData {Name = "PageSize", Value = pageSize.ToString()}
                };

            return GetEntityList<ServiceCodes>(StoredProcedure.GetServiceCodes, searchParam);
        }

        public List<ReferralDetailForNote> GetReferralInfo(int pageSize, string searchText = null)
        {
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "SearchText", Value = searchText},
                    new SearchValueData {Name = "PageSize", Value =pageSize.ToString()}
                };

            return GetEntityList<ReferralDetailForNote>(StoredProcedure.GetNotePageReferrals, searchParam);
        }

        public ServiceResponse GetPosCodes(long referralID, DateTime? serviceDate, int serviceCodeID, long noteID, long payorID)
        {
            ServiceResponse response = new ServiceResponse();
            if (serviceDate.HasValue)
            {
                List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = referralID.ToString()},
                    new SearchValueData {Name = "ServiceDate", Value = serviceDate.Value.ToString(Constants.DbDateFormat)},
                    new SearchValueData {Name = "ServiceCodeID", Value = serviceCodeID.ToString()},
                    new SearchValueData {Name = "NoteID", Value = noteID.ToString()},
                    new SearchValueData {Name = "PayorID", Value = payorID.ToString()}
                };

                List<PosDropdownModel> serviceCodeList =
                    GetEntityList<PosDropdownModel>(StoredProcedure.GetPOS, searchParam);

                response.Data = serviceCodeList;
            }
            else
                response.Data = null;
            response.IsSuccess = true;

            return response;
        }

        public ServiceResponse GetAutoCreateServiceInformation(long referralID, Note tempNote)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = referralID.ToString()},
                    new SearchValueData {Name = "ServiceDate", Value = tempNote.ServiceDate.ToString(Constants.DbDateFormat)},
                    new SearchValueData {Name = "ServiceCodeID", Value = tempNote.ServiceCodeID.ToString()},
                    new SearchValueData {Name = "NoteID", Value = tempNote.NoteID.ToString()},
                    new SearchValueData {Name = "PosID", Value = tempNote.PosID.ToString()}
                };

            List<PosDropdownModel> serviceCodeList = GetEntityList<PosDropdownModel>(StoredProcedure.GetAutoCreateServiceInformation, searchParam);

            List<Note> tempNotes = new List<Note>();
            foreach (var selectedServiceCodeForPayor in serviceCodeList)
            {
                Note note = JsonConvert.DeserializeObject<Note>(JsonConvert.SerializeObject(tempNote));
                note.ServiceCodeID = selectedServiceCodeForPayor.ServiceCodeID;
                note.ServiceCode = selectedServiceCodeForPayor.ServiceCode;
                note.Description = selectedServiceCodeForPayor.Description;
                note.PayorServiceCodeMappingID = selectedServiceCodeForPayor.PayorServiceCodeMappingID;
                note.SelectedServiceCodeForPayor = selectedServiceCodeForPayor;
                note.UnitType = selectedServiceCodeForPayor.UnitType;



                if (note.UnitType == (int)EnumUnitType.Time)
                {
                    if (note.StartTime != null && note.EndTime != null)
                    {

                        double timeDifference = note.EndTime.Value.Subtract(note.StartTime.Value).TotalMinutes;
                        note.SelectedServiceCodeForPayor.MinutesDiff = Convert.ToInt16(timeDifference);
                        note.SelectedServiceCodeForPayor.CalculatedUnit = (float)Math.Round(timeDifference / note.SelectedServiceCodeForPayor.PerUnitQuantity);
                    }
                }
                else if (note.UnitType == (int)EnumUnitType.DistanceInMiles)
                {
                    if (note.StartMile.HasValue && note.EndMile.HasValue)
                    {
                        long mileDifference = note.EndMile.Value - note.StartMile.Value;
                        note.SelectedServiceCodeForPayor.MileDiff = Convert.ToInt16(mileDifference);
                        note.SelectedServiceCodeForPayor.CalculatedUnit = (float)Math.Round(mileDifference / note.SelectedServiceCodeForPayor.PerUnitQuantity);
                    }

                }
                else if (note.UnitType == (int)EnumUnitType.Stop)
                {
                    if (note.StartMile.HasValue && note.EndMile.HasValue)
                    {
                        long mileDifference = note.EndMile.Value - note.StartMile.Value;
                        note.SelectedServiceCodeForPayor.MileDiff = Convert.ToInt16(mileDifference);
                        note.SelectedServiceCodeForPayor.CalculatedUnit = 1;
                    }

                }


                tempNotes.Add(note);
            }
            response.Data = tempNotes;
            response.IsSuccess = true;

            return response;
        }

        public ServiceResponse SaveGeneralNote(long referralID, string message, string source, long loggedInUserID, string spokeTo, string relation, string kindOfNote, string AttachmentURL = null, string MonthlySummaryIds = null)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "ReferralID", Value = referralID.ToString() });
                searchParam.Add(new SearchValueData { Name = "NoteDetail", Value = message });
                searchParam.Add(new SearchValueData { Name = "ServiceCodeTypeID", Value = ((int)ServiceCodeType.ServiceCodeTypes.Other).ToString() });
                searchParam.Add(new SearchValueData { Name = "Relation", Value = relation });
                searchParam.Add(new SearchValueData { Name = "SpokeTo", Value = spokeTo });
                searchParam.Add(new SearchValueData { Name = "KindOfNote", Value = !string.IsNullOrEmpty(kindOfNote) ? kindOfNote : Resource.Other });
                searchParam.Add(new SearchValueData { Name = "LoggedInUserID", Value = loggedInUserID.ToString() });
                searchParam.Add(new SearchValueData { Name = "SystemID", Value = HttpContext.Current.Request.UserHostAddress });
                searchParam.Add(new SearchValueData { Name = "Source", Value = source });
                searchParam.Add(new SearchValueData { Name = "AttachmentURL", Value = AttachmentURL });
                searchParam.Add(new SearchValueData { Name = "MonthlySummaryIds", Value = MonthlySummaryIds });
                GetScalar(StoredProcedure.SaveGeneralNote, searchParam);
                response.IsSuccess = true;
                
             }
            catch(Exception ex) 
            {

            }
            return response; 
            //#region First Dos Code

            ////if (note.IsBillable)
            ////{
            //GetScalar(StoredProcedure.UpdateFirstDos,
            //    new List<SearchValueData>
            //                {
            //                    new SearchValueData {Name = "ReferralID", Value = referralID.ToString()}
            //                });
            ////}

            //#endregion

        }

        #endregion

        #region Client Note

        public ServiceResponse GetNoteClientList(SearchNoteListModel searchNoteModel, int pageIndex = 1, int pageSize = 10,
                                          string sortIndex = "", string sortDirection = "")
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchNoteModel != null)
                SetSearchFilterForNoteListPage(searchNoteModel, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<NoteClientList> totalData = GetEntityList<NoteClientList>(StoredProcedure.GetNoteClientList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<NoteClientList> noteList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = noteList;
            response.IsSuccess = true;
            return response;
        }
        #endregion

        #region Group Note

        public ServiceResponse SetAddGroupNote(long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "LoggedInUserID", Value = loggedInUserID.ToString()}
                };

            GroupNoteModel noteDetail = GetMultipleEntity<GroupNoteModel>(StoredProcedure.SetAddGroupNotePage, searchParam);

            //noteDetail.Note.Signature = noteDetail.EmpSignature;
            noteDetail.Note.EmpSignatureDetails = noteDetail.EmpSignatureDetails;

            noteDetail.Note.NoteAssignee = loggedInUserID;
            //noteDetail.Note.NoteComments = Resource.NoteAssignedForReview;

            noteDetail.Services = Common.SetNoteServices();
            noteDetail.Relations = Common.SetNoteRelations();

            noteDetail.KindOfNotes = Common.SetNoteKind();
            //if (noteDetail.Payors.Count > 0)
            //    noteDetail.SearchGroupNoteClient.PayorID = noteDetail.Payors[0].Value;

            response.Data = noteDetail;
            response.IsSuccess = true;

            return response;
        }

        public ServiceResponse SearchClientForNote(SaerchGroupNoteClient searchGroupNoteClient, List<long> ignoreClientID)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ClientName", Value = searchGroupNoteClient.ClientName},                 
                    new SearchValueData {Name = "PayorID", Value = searchGroupNoteClient.PayorID.ToString()},                 
                    new SearchValueData {Name = "FacilityID", Value = searchGroupNoteClient.FacilityID.ToString()}                 
                };

            if (ignoreClientID != null)
                searchParam.Add(new SearchValueData { Name = "IgnoreClientID", Value = string.Join(",", ignoreClientID) });

            if (searchGroupNoteClient.EndDate.HasValue)
                searchParam.Add(new SearchValueData { Name = "EndDate", Value = searchGroupNoteClient.EndDate.Value.ToString(Constants.DbDateFormat) });

            if (searchGroupNoteClient.StartDate.HasValue)
                searchParam.Add(new SearchValueData { Name = "StartDate", Value = searchGroupNoteClient.StartDate.Value.ToString(Constants.DbDateFormat) });

            if (searchGroupNoteClient.PageSize > 0)
                searchParam.Add(new SearchValueData { Name = "PageSize", Value = Convert.ToString(searchGroupNoteClient.PageSize) });

            searchParam.Add(new SearchValueData { Name = "ContactTypeID", Value = ((int)Common.ContactTypes.PrimaryPlacement).ToString() });

            ClientForGroupNote clientList =
                GetMultipleEntity<ClientForGroupNote>(StoredProcedure.GetClientsForGroupNote, searchParam);

            response.IsSuccess = true;
            response.Data = clientList;
            return response;
        }

        public List<ServiceCodes> GetGroupNoteServiceCodes(long payorID, DateTime serviceDate, int serviceCodeTypeID, int pageSize, string searchText = null)
        {
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "PayorID", Value = payorID.ToString()},
                    new SearchValueData {Name = "ServiceCodeTypeID", Value = serviceCodeTypeID.ToString()},
                    new SearchValueData {Name = "ServiceDate", Value = serviceDate.ToString(Constants.DbDateFormat)},
                    new SearchValueData {Name = "SearchText", Value = searchText},
                    new SearchValueData {Name = "PageSize", Value = pageSize.ToString()}
                };

            return GetEntityList<ServiceCodes>(StoredProcedure.GetGroupNotePageServiceCode, searchParam);
        }

        public ServiceResponse SaveGroupNote(List<SaveGroupNoteModel> saveGroupNoteModel, long loggedInUserID, string loggedInName = null)
        {
            ServiceResponse response = new ServiceResponse();
            GroupNoteMsg noteMsg = new GroupNoteMsg();
            var logFileName = "GroupNote_" + DateTime.Now.TimeOfDay.Ticks.ToString();
            string logpath = ConfigSettings.LogPath + ConfigSettings.GroupNoteLog;
            string msg = string.Empty;
            foreach (var groupNoteModel in saveGroupNoteModel)
            {

                List<Note> noteList = (List<Note>)GetAutoCreateServiceInformation(groupNoteModel.ReferralID, groupNoteModel.Note).Data;
                if (noteList != null && noteList.Count > 0)
                {
                    groupNoteModel.Note.GroupIDForMileServices = Common.GenerateRandomNumber();
                    groupNoteModel.Note.RandomGroupID = Common.GenerateRandomNumber();
                }

                #region Update DTR Detail Set Value Base on Selection

                if (!groupNoteModel.Note.DTRIsOnline)
                {
                    groupNoteModel.Note.VehicleNumber = null;
                    groupNoteModel.Note.VehicleType = null;
                    groupNoteModel.Note.PickUpAddress = null;
                    groupNoteModel.Note.DropOffAddress = null;
                    groupNoteModel.Note.EscortName = null;
                    groupNoteModel.Note.Relationship = null;
                    groupNoteModel.Note.DriverID = null;
                    groupNoteModel.Note.RoundTrip = false;
                    groupNoteModel.Note.OneWay = false;
                    groupNoteModel.Note.MultiStops = false;
                }

                #endregion

                ServiceResponse res = SaveNote(groupNoteModel.Note, groupNoteModel.ReferralID,
                                               groupNoteModel.SelectedServiceCodeForPayor, groupNoteModel.SelectedDxCodes, loggedInUserID, loggedInName);
                if (res.IsSuccess)
                {
                    res.Message = string.IsNullOrEmpty(res.Message) ? Resource.Passed : res.Message;
                    noteMsg.SuccessCount = 1;
                    noteMsg.SuccessMsg += "<li><b>" + groupNoteModel.Name + "</b> (" + @Resource.AHCCCSID + ": " + groupNoteModel.AHCCCSID +
                                          ", " + @Resource.ServiceCode + ": " + groupNoteModel.Note.ServiceCode + ") :- " + res.Message + "</li>";
                }
                else
                {
                    res.Message = string.IsNullOrEmpty(res.Message) ? Resource.NoteFailed : res.Message;
                    noteMsg.ErrorCount = 1;
                    noteMsg.ErrorMsg += "<li><b>" + groupNoteModel.Name + "</b> (" + @Resource.AHCCCSID + ": " + groupNoteModel.AHCCCSID +
                                       ", " + @Resource.ServiceCode + ": " + groupNoteModel.Note.ServiceCode + ") :- " + res.Message + "</li>";
                }
                msg += "<br><b>" + groupNoteModel.Name + "</b> (" + @Resource.AHCCCSID + ": " + groupNoteModel.AHCCCSID +
                                         ", " + @Resource.ServiceCode + ": " + groupNoteModel.Note.ServiceCode + ") :- " + res.Message;



                #region TO Check Group Service Codes Relations And Add Automatic Related Service Codes

                if (noteList == null) continue;
                foreach (var note in noteList)
                {
                    note.GroupIDForMileServices = groupNoteModel.Note.GroupIDForMileServices;
                    note.RandomGroupID = groupNoteModel.Note.RandomGroupID;

                    #region Update DTR Detail Set Value Base on Selection

                    if (!groupNoteModel.Note.DTRIsOnline)
                    {
                        note.VehicleNumber = null;
                        note.VehicleType = null;
                        note.PickUpAddress = null;
                        note.DropOffAddress = null;
                        note.EscortName = null;
                        note.Relationship = null;
                        note.DriverID = null;
                        note.RoundTrip = false;
                        note.OneWay = false;
                        note.MultiStops = false;
                    }

                    #endregion

                    ServiceResponse resExtra = SaveNote(note, groupNoteModel.ReferralID,
                        note.SelectedServiceCodeForPayor, groupNoteModel.SelectedDxCodes, loggedInUserID, loggedInName);
                    if (resExtra.IsSuccess)
                    {
                        resExtra.Message = string.IsNullOrEmpty(resExtra.Message) ? Resource.Passed : resExtra.Message;
                        noteMsg.SuccessCount = 1;
                        noteMsg.SuccessMsg += "<li><b>" + groupNoteModel.Name + "</b> (" + @Resource.AHCCCSID + ": " + groupNoteModel.AHCCCSID +
                                              ", " + @Resource.ServiceCode + ": " + note.ServiceCode + ") :- " + resExtra.Message + "</li>";
                    }
                    else
                    {
                        resExtra.Message = string.IsNullOrEmpty(resExtra.Message) ? Resource.NoteFailed : resExtra.Message;
                        noteMsg.ErrorCount = 1;
                        noteMsg.ErrorMsg += "<li><b>" + groupNoteModel.Name + "</b> (" + @Resource.AHCCCSID + ": " + groupNoteModel.AHCCCSID +
                                            ", " + @Resource.ServiceCode + ": " + note.ServiceCode + ") :- " + resExtra.Message + "</li>";
                    }
                    msg += "<br><b>" + groupNoteModel.Name + "</b> (" + @Resource.AHCCCSID + ": " + groupNoteModel.AHCCCSID +
                          ", " + @Resource.ServiceCode + ": " + note.ServiceCode + ") :- " + resExtra.Message;

                }

                #endregion




            }
            Common.CreateLogFile(msg, logFileName, logpath);
            noteMsg.SuccessMsg += "</ul>";
            noteMsg.ErrorMsg += "</ul>";
            response.Data = noteMsg;
            response.IsSuccess = true;
            //response.Message = Resource.GroupNoteSavedSuccessfully;
            return response;
        }

        public List<ServiceCodes> GetServiceCodeList(string searchText, long loggedInId, int pageSize, int serviceCodeTypeID)
        {
            List<ServiceCodes> contactlist = GetEntityList<ServiceCodes>(StoredProcedure.GetServiceCodeListForAutoCompleter,
                                            new List<SearchValueData>
                                                {
                                                    new SearchValueData {Name = "SearchText", Value = searchText},
                                                    new SearchValueData {Name = "PageSize", Value = pageSize.ToString()},
                                                    new SearchValueData {Name = "ServiceCodeTypeID", Value = serviceCodeTypeID.ToString()},
                                                    new SearchValueData {Name = "LoggedInId", Value = loggedInId.ToString()},
                                                    new SearchValueData {Name = "CredentialBHP", Value = EmployeeCredentialEnum.BHP.ToString()}
                                                    
                                                });
            return contactlist;
        }

        public ServiceResponse ValidateServiceCodeDetails(ValidateServiceCodeModel model)
        {
            ServiceResponse response = new ServiceResponse();

            List<InvalidPayorsList> invalidPayorList = GetEntityList<InvalidPayorsList>(StoredProcedure.ValidateServiceCodeForNotes,
                                            new List<SearchValueData>
                                                {
                                                    new SearchValueData {Name = "PayorCsv", Value = model.PayorCsv},
                                                    new SearchValueData {Name = "ServiceCodeID", Value = model.ServiceCodeID},
                                                    new SearchValueData {Name = "PosID", Value = model.PosID},
                                                    new SearchValueData {Name = "ServiceDate", Value = model.ServiceDate.ToString(Constants.DbDateFormat)}
                                                });

            if (invalidPayorList.Count == 0)
            {
                response.IsSuccess = true;
                response.Message = "";
            }
            else
            {
                response.Message = String.Format(Resource.GroupNoteValidationErrorMsg, String.Join(", ", invalidPayorList.Select(c => c.ShortName)));
            }
            return response;
        }

        #endregion

        #region Note Service Code Change Mapping
        public ServiceResponse ChangeServiceCode(long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData> { new SearchValueData { Name = "LoggedInUserID", Value = loggedInUserID.ToString() } };

            ChangeServiceCodeModel noteDetail = GetMultipleEntity<ChangeServiceCodeModel>(StoredProcedure.SetChangeServiceCodePage, searchParam);
            response.Data = noteDetail;
            response.IsSuccess = true;

            return response;
        }
        public ServiceResponse SearchNoteForChangeServiceCode(SearchNote searchNote)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ClientName", Value = searchNote.ClientName},                 
                    new SearchValueData {Name = "PayorID", Value = searchNote.PayorID.ToString()},                 
                    new SearchValueData {Name = "CreatedBy", Value = searchNote.CreatedBy.ToString()},                 
                    new SearchValueData {Name = "ServiceCodeID", Value = searchNote.ServiceCodeID.ToString()},                 
                    new SearchValueData {Name = "PosID", Value = searchNote.PosID.ToString()},                 
                };

            if (searchNote.EndDate.HasValue)
                searchParam.Add(new SearchValueData { Name = "EndDate", Value = searchNote.EndDate.Value.ToString(Constants.DbDateFormat) });

            if (searchNote.StartDate.HasValue)
                searchParam.Add(new SearchValueData { Name = "StartDate", Value = searchNote.StartDate.Value.ToString(Constants.DbDateFormat) });


            List<ChangeServiceCodeNotes> data = GetEntityList<ChangeServiceCodeNotes>(StoredProcedure.GetNotesForChangeServiceCode, searchParam);

            response.IsSuccess = true;
            response.Data = data;
            return response;
        }
        public ServiceResponse ValidateChangeServiceCode(SearchNote searchNote)
        {
            ServiceResponse response = new ServiceResponse();

            List<SelectedServiceCodeModel> mappings = GetEntityList<SelectedServiceCodeModel>(StoredProcedure.ValidateChangeServiceCode,
                                            new List<SearchValueData>
                                                {
                                                    new SearchValueData {Name = "PayorID", Value = searchNote.PayorID.ToString()},
                                                    new SearchValueData {Name = "PosID", Value = searchNote.PosID.ToString()},
                                                    new SearchValueData {Name = "NewServiceCodeID", Value = searchNote.NewServiceCodeID.ToString()},
                                                    new SearchValueData {Name = "NewServiceStartDate", Value = searchNote.ServiceStartDate.ToString(Constants.DbDateFormat)},
                                                });

            if (mappings.Count == 1)
            {
                response.IsSuccess = true;
                response.Data = mappings;
            }
            else
            {
                response.Message = String.Format(Resource.ServiceCodeMappingError);
            }
            return response;
        }
        
        public ServiceResponse ReplaceServiceCode(string noteIds, SearchNote searchNote, long loggedInUserID)
        {
            ServiceResponse response1 = new ServiceResponse();
            GroupNoteMsg noteMsg = new GroupNoteMsg();
            var logFileName = "ChangeServiceCode_" + DateTime.Now.TimeOfDay.Ticks.ToString();
            string logpath = ConfigSettings.LogPath + ConfigSettings.ChangeServiceCodeLogs;
            string msg = string.Empty;

            List<Note> notes = GetEntityList<Note>(null, "NoteID IN (" + noteIds + ")");
            foreach (var note in notes)
            {
                note.ServiceCodeID = searchNote.NewServiceCodeID;
                note.PayorServiceCodeMappingID = searchNote.PayorServiceCodeMappingID;
                note.PosID = searchNote.PosID;


                #region Note Service Code Update Code
                ServiceResponse response = new ServiceResponse();


                #region Caluclate unit count

                if (note.ServiceCodeID.HasValue)
                {
                    long tempPayorServiceCodeMappingId = note.PayorServiceCodeMappingID; //TODO: CHANGE

                    List<SearchValueData> payorServiceCodeSearchParam = new List<SearchValueData>
                        {
                            new SearchValueData {Name = "ReferralID", Value = note.ReferralID.ToString()},
                            new SearchValueData
                                {
                                    Name = "ServiceDate",Value = note.ServiceDate.ToString(Constants.DbDateFormat)
                                },
                            new SearchValueData {Name = "ServiceCodeID", Value = note.ServiceCodeID.ToString()},
                            new SearchValueData{
                                    Name = "PayorServiceCodeMappingID",Value =  tempPayorServiceCodeMappingId.ToString()// selectedServiceCodeDetail.PayorServiceCodeMappingID.ToString()
                                },
                                new SearchValueData {Name = "PosID", Value = Convert.ToString(note.PosID)},
                                new SearchValueData {Name = "PayorID", Value = Convert.ToString(note.PayorID)}
                        };

                    if (note.NoteID > 0)
                    {
                        payorServiceCodeSearchParam.Add(new SearchValueData { Name = "NoteID", Value = note.NoteID.ToString() });
                    }

                    PosDropdownModel tempPayorServiceCodeMapping = GetEntity<PosDropdownModel>(StoredProcedure.GetPOS, payorServiceCodeSearchParam);

                    //if (tempPayorServiceCodeMapping == null)
                    //{
                    //    return response;
                    //}
                    //continue;

                    if (tempPayorServiceCodeMappingId == 0)
                    {
                        tempPayorServiceCodeMappingId = tempPayorServiceCodeMapping.PayorServiceCodeMappingID;
                        note.PayorServiceCodeMappingID = tempPayorServiceCodeMappingId;
                    }


                    PosDropdownModel selectedServiceCodeDetail = new PosDropdownModel();
                    selectedServiceCodeDetail.PerUnitQuantity = tempPayorServiceCodeMapping.PerUnitQuantity;
                    selectedServiceCodeDetail.UnitType = tempPayorServiceCodeMapping.UnitType;
                    selectedServiceCodeDetail.DefaultUnitIgnoreCalculation = tempPayorServiceCodeMapping.DefaultUnitIgnoreCalculation;


                    if (selectedServiceCodeDetail.DefaultUnitIgnoreCalculation > 0)
                    {
                        note.CalculatedUnit = selectedServiceCodeDetail.DefaultUnitIgnoreCalculation;
                        tempPayorServiceCodeMapping.CalculatedUnit = selectedServiceCodeDetail.DefaultUnitIgnoreCalculation;
                        selectedServiceCodeDetail.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                    }
                    else if (selectedServiceCodeDetail.UnitType == (int)EnumUnitType.Time)
                    {
                        if (note.StartTime != null && note.EndTime != null)
                        {
                            double timeDifference = note.EndTime.Value.Subtract(note.StartTime.Value).TotalMinutes;
                            //tempPayorServiceCodeMapping.CalculatedUnit = (float)Math.Round(timeDifference / selectedServiceCodeDetail.PerUnitQuantity);

                            tempPayorServiceCodeMapping.CalculatedUnit = 1;
                            if (timeDifference > selectedServiceCodeDetail.PerUnitQuantity)
                                timeDifference = timeDifference - selectedServiceCodeDetail.PerUnitQuantity;
                            else
                                timeDifference = 0;

                            tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit +
                                (float)Math.Round(timeDifference / selectedServiceCodeDetail.PerUnitQuantity);

                            selectedServiceCodeDetail.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;

                            //if (tempPayorServiceCodeMapping.CalculatedUnit > tempPayorServiceCodeMapping.AvailableDailyUnit)
                            //{
                            //    tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                            //    note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                            //}
                            if ((tempPayorServiceCodeMapping.DailyUnitLimit == 0 && tempPayorServiceCodeMapping.MaxUnit == 0) || tempPayorServiceCodeMapping.CalculatedUnit <= tempPayorServiceCodeMapping.AvailableDailyUnit)
                            {
                                tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                                note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                            }
                            else if (tempPayorServiceCodeMapping.DailyUnitLimit == 0 &&
                                     tempPayorServiceCodeMapping.MaxUnit != 0)
                            {
                                if (tempPayorServiceCodeMapping.CalculatedUnit <= tempPayorServiceCodeMapping.AvailableMaxUnit)
                                {
                                    note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                                }
                                else
                                {
                                    note.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                                    tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                                }
                            }
                            else
                            {
                                note.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                                tempPayorServiceCodeMapping.CalculatedUnit =
                                    tempPayorServiceCodeMapping.AvailableDailyUnit;
                            }
                        }
                    }
                    else if (selectedServiceCodeDetail.UnitType == (int)EnumUnitType.DistanceInMiles)
                    {
                        if (note.StartMile.HasValue && note.EndMile.HasValue)
                        {
                            long mileDifference = note.EndMile.Value - note.StartMile.Value;
                            //tempPayorServiceCodeMapping.CalculatedUnit = (float)Math.Round(mileDifference / selectedServiceCodeDetail.PerUnitQuantity);


                            tempPayorServiceCodeMapping.CalculatedUnit = 1;
                            if (mileDifference > selectedServiceCodeDetail.PerUnitQuantity)
                                mileDifference = mileDifference - Convert.ToInt16(selectedServiceCodeDetail.PerUnitQuantity);
                            else
                                mileDifference = 0;


                            tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit +
                                (float)Math.Round(mileDifference / selectedServiceCodeDetail.PerUnitQuantity);

                            selectedServiceCodeDetail.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                            //if (tempPayorServiceCodeMapping.CalculatedUnit > tempPayorServiceCodeMapping.AvailableDailyUnit)
                            //{
                            //    tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                            //    note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                            //}

                            if ((tempPayorServiceCodeMapping.DailyUnitLimit == 0 && tempPayorServiceCodeMapping.MaxUnit == 0) || tempPayorServiceCodeMapping.CalculatedUnit <= tempPayorServiceCodeMapping.AvailableDailyUnit)
                            {
                                tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                                note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                            }
                            else if (tempPayorServiceCodeMapping.DailyUnitLimit == 0 &&
                                     tempPayorServiceCodeMapping.MaxUnit != 0)
                            {
                                if (tempPayorServiceCodeMapping.CalculatedUnit <= tempPayorServiceCodeMapping.AvailableMaxUnit)
                                {
                                    note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                                }
                                else
                                {
                                    note.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                                    tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                                }
                            }
                            else
                            {
                                note.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                                tempPayorServiceCodeMapping.CalculatedUnit =
                                    tempPayorServiceCodeMapping.AvailableDailyUnit;
                            }

                        }
                    }
                    else if (selectedServiceCodeDetail.UnitType == (int)EnumUnitType.Stop)
                    {
                        note.CalculatedUnit = 1;
                        tempPayorServiceCodeMapping.CalculatedUnit = 1;
                        if (tempPayorServiceCodeMapping.CalculatedUnit > tempPayorServiceCodeMapping.AvailableDailyUnit)
                        {
                            note.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                            tempPayorServiceCodeMapping.CalculatedUnit = tempPayorServiceCodeMapping.AvailableDailyUnit;
                        }
                        selectedServiceCodeDetail.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;

                    }


                    if ((int)Math.Ceiling(tempPayorServiceCodeMapping.CalculatedUnit) < (int)Math.Ceiling(selectedServiceCodeDetail.CalculatedUnit))
                    {
                        response.Message = Resource.OverLimitMsg;
                        tempPayorServiceCodeMapping.CalculatedUnit = selectedServiceCodeDetail.CalculatedUnit;
                        note.CalculatedUnit = tempPayorServiceCodeMapping.CalculatedUnit;
                        //return response;
                    }
                    else if ((int)Math.Ceiling(tempPayorServiceCodeMapping.CalculatedUnit) > (int)Math.Ceiling(selectedServiceCodeDetail.CalculatedUnit))
                    {
                        response.Message = Resource.UnitCountNotMatched;
                        return response;
                    }


                    note.CalculatedAmount = note.CalculatedUnit * tempPayorServiceCodeMapping.Rate;
                }
                #endregion

                #region Save service code detail in Note table

                if (note.ServiceCodeID.HasValue)
                {
                    ServiceCodes serviceCode = GetEntity<ServiceCodes>(note.ServiceCodeID.Value);
                    note.ServiceCode = serviceCode.ServiceCode;
                    note.ServiceName = serviceCode.ServiceName;
                    note.Description = serviceCode.Description;
                    note.MaxUnit = serviceCode.MaxUnit;
                    note.DailyUnitLimit = serviceCode.DailyUnitLimit;
                    note.IsBillable = serviceCode.IsBillable;
                    note.UnitType = serviceCode.UnitType;
                    note.PerUnitQuantity = serviceCode.PerUnitQuantity;
                    //note.ServiceCodeType = serviceCode.ServiceCodeType;
                    note.ServiceCodeStartDate = serviceCode.ServiceCodeStartDate;
                    note.ServiceCodeEndDate = serviceCode.ServiceCodeEndDate;
                    note.CheckRespiteHours = serviceCode.CheckRespiteHours;


                }



                #endregion

                #region Payor service code mapping in Note table
                if (note.PayorServiceCodeMappingID > 0)
                {
                    PayorServiceCodeMapping payorServiceCodeMapping = GetEntity<PayorServiceCodeMapping>(note.PayorServiceCodeMappingID);
                    note.PayorServiceCodeMappingID = payorServiceCodeMapping.PayorServiceCodeMappingID;
                    note.PayorID = payorServiceCodeMapping.PayorID;
                    note.ModifierID = payorServiceCodeMapping.ModifierID;
                    note.PosID = payorServiceCodeMapping.PosID;
                    note.Rate = payorServiceCodeMapping.Rate;
                    note.POSStartDate = payorServiceCodeMapping.POSStartDate;
                    note.POSEndDate = payorServiceCodeMapping.POSEndDate;
                }

                #endregion

                SaveObject(note, loggedInUserID);

                response.IsSuccess = true;
                response.Message = !string.IsNullOrEmpty(response.Message)
                                       ? Resource.NoteUpdatedSuccessfully + string.Format(" " + response.Message, note.ServiceCode)
                                       : Resource.NoteUpdatedSuccessfully;


                #endregion


                if (response.IsSuccess)
                {
                    response.Message = string.IsNullOrEmpty(response.Message) ? Resource.Passed : response.Message;
                    noteMsg.SuccessCount = 1;
                    noteMsg.SuccessMsg += "<li><b>Note #" + note.NoteID + "</b> (" + @Resource.AHCCCSID + ": " + note.AHCCCSID +
                                          ", " + @Resource.ServiceCode + ": " + note.ServiceCode + ") :- " + response.Message + "</li>";
                }
                else
                {
                    response.Message = string.IsNullOrEmpty(response.Message) ? Resource.NoteFailed : response.Message;
                    noteMsg.ErrorCount = 1;
                    noteMsg.ErrorMsg += "<li><b>Note #" + note.NoteID + "</b> (" + @Resource.AHCCCSID + ": " + note.AHCCCSID +
                                       ", " + @Resource.ServiceCode + ": " + note.ServiceCode + ") :- " + response.Message + "</li>";
                }
                msg += "<br><b>Note #" + note.NoteID + "</b> (" + @Resource.AHCCCSID + ": " + note.AHCCCSID +
                                         ", " + @Resource.ServiceCode + ": " + note.ServiceCode + ") :- " + response.Message;



            }
            Common.CreateLogFile(msg, logFileName, logpath);
            noteMsg.SuccessMsg += "</ul>";
            noteMsg.ErrorMsg += "</ul>";
            response1.Data = noteMsg;
            response1.IsSuccess = true;
            //response.Message = Resource.GroupNoteSavedSuccessfully;
            return response1;
















        }
       
        #endregion

    }
}
