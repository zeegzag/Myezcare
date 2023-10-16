var custModel;

controllers.NoteController = function ($scope, $http, $timeout) {

    custModel = $scope;
    $scope.EncryptedReferralID = window.EncryptedReferralID;
    $scope.SelectedServiceCodeForPayor = {};
    $scope.AddNotePageModel = null;
    $scope.DistanceInMilesUnit = parseInt(window.DistanceInMilesUnit);
    $scope.StopUnit = parseInt(window.StopUnit);
    $scope.OtherServiceCode = parseInt(window.OtherServiceCode);
    $scope.OtherPOS = parseInt(window.OtherPOS);
    $scope.GetServiceCodesURL = SiteUrl.GetServiceCodesURL;
    $scope.GetBatchURL = SiteUrl.GetServiceCodesURL;
    $scope.GetDTRDetailsURL = SiteUrl.GetDTRDetailsURL;
    //$scope.AddNotePageModel.SelectedDxCodes = [];
    $scope.SelectAllDxCodeCheckbox = false;

    //#region Note Listing Related Stuff
    $scope.GetReferralsURL = SiteUrl.GetReferralInfoURL;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetNoteListPage").val());
    };
    $scope.SelectedNoteIds = [];
    $scope.NoteList = [];
    $scope.SelectAllCheckbox = false;
    $scope.NoteModel = $.parseJSON($("#hdnSetNoteListPage").val());
    $scope.SearchNoteModel = $scope.newInstance().SearchNoteListModel;
    $scope.TempSearchNoteModel = $scope.newInstance().SearchNoteListModel;


    $scope.SetPostData = function (fromIndex, childPager) {
        var pagermodel = {};
        if (childPager) {
            pagermodel = {
                searchNoteModel: $scope.SearchNoteModel,
                pageSize: childPager.NoteListPager.pageSize,
                pageIndex: fromIndex,
                sortIndex: childPager.NoteListPager.sortIndex,
                sortDirection: childPager.NoteListPager.sortDirection
            };
        } else {
            pagermodel = {
                searchNoteModel: $scope.SearchNoteModel,
                pageSize: $scope.NoteClientListPager.pageSize,
                pageIndex: fromIndex,
                sortIndex: $scope.NoteClientListPager.sortIndex,
                sortDirection: $scope.NoteClientListPager.sortDirection
            };
        }
        return angular.toJson(pagermodel);
    };
    $scope.SearchModelMapping = function () {
        $scope.SearchNoteModel = $.parseJSON(angular.toJson($scope.TempSearchNoteModel));
        if ($scope.SearchNoteModel.ServiceCodeIDs != undefined)
            $scope.SearchNoteModel.ServiceCodeIDs = $scope.SearchNoteModel.ServiceCodeIDs.toString();

        if ($scope.SearchNoteModel.CreatedByIDs != undefined)
            $scope.SearchNoteModel.CreatedByIDs = $scope.SearchNoteModel.CreatedByIDs.toString();

        if ($scope.SearchNoteModel.AssigneeID != undefined)
            $scope.SearchNoteModel.AssigneeID = $scope.SearchNoteModel.AssigneeID.toString();
    };
    $scope.ResetSearchFilter = function () {



        $scope.SearchNoteModel = $scope.newInstance().SearchNoteListModel;
        $scope.TempSearchNoteModel = $scope.newInstance().SearchNoteListModel;
        $scope.TempSearchNoteModel.IsBillable = "-1";
        $scope.TempSearchNoteModel.IsDeleted = "0";
        $("#SearchServiceCodeToken").tokenInput("clear");

        if (!$scope.NoteModel.IsPartial) {
            $("#ReferralIDTkn").tokenInput("clear");
        }
        $scope.NoteClientList = [];

    };


    //#region Note Client List  OR Note List GROUP By Clients
    $scope.NoteClientList = [];
    $scope.NoteClientListPager = new PagerModule("ReferralName", null, "ASC");
    $scope.GetNoteClientList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedNoteIds = [];
        $scope.SelectAllCheckbox = false;
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        $scope.GetNoteClientListAjaxCall = true;
        var jsonData = $scope.SetPostData($scope.NoteClientListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetNoteClientListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.NoteClientList = response.Data.Items;
                $scope.NoteClientListPager.currentPageSize = response.Data.Items.length;
                $scope.NoteClientListPager.totalRecords = response.Data.TotalItems;
                $scope.ShowCollpase();
            }
            ShowMessages(response);
            $scope.GetNoteClientListAjaxCall = false;
        });
    };
    $scope.NoteClientListPager.getDataCallback = $scope.GetNoteClientList;



    $scope.Refresh = function () {
        if (CheckErrors($("#frm_clientNote")))
            $scope.NoteClientListPager.getDataCallback(true);
    };
    $scope.SearchNote = function () {
        if (CheckErrors($("#frm_clientNote"))) {
            //$scope.NoteListPager.currentPage = 1;
            //$scope.NoteListPager.getDataCallback(true);
            $scope.NoteClientListPager.currentPage = 1;
            $scope.NoteClientListPager.getDataCallback(true);
        }
    };



    $scope.ExportNoteList = function () {

        $scope.SearchModelMapping();
        $scope.GetNoteClientListAjaxCall = true;
        var jsonData = $scope.SetPostData($scope.NoteClientListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetExportNoteListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
            }
            ShowMessages(response);
            $scope.GetNoteClientListAjaxCall = false;
        });


    };


    //#endregion Note Client List



    //#region Individual Client's Note List Histories
    //$scope.NoteListPager = new PagerModule("NoteID", null, "DESC");
    $scope.GetClientNoteDetailList = function (item, isSearchDataMappingRequire, donotLoad) {
        if (item.NoteListPager == undefined) {
            item.NoteListPager = new PagerModule("NoteID", null, "DESC");
            item.NoteListPager.getDataCallback = $scope.GetClientNoteDetailList;
        }

        if (item.ClientNoteDetailList == undefined || !donotLoad) {
            //Reset Selcted Checkbox items and Control
            $scope.SelectedNoteIds = [];
            $scope.SelectAllCheckbox = false;
            //Reset Selcted Checkbox items and Control

            //STEP 1:   Seach Model Mapping
            if (isSearchDataMappingRequire)
                $scope.SearchModelMapping();
            //STEP 1:   Seach Model Mapping

            var oldReferralId = $scope.SearchNoteModel.ReferralID;
            $scope.SearchNoteModel.ReferralID = item.ReferralID;

            item.GetNoteClientListAjaxCall = true;
            var jsonData = $scope.SetPostData(item.NoteListPager.currentPage, item);
            AngularAjaxCall($http, SiteUrl.GetNoteListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                if (response.IsSuccess) {
                    item.ClientNoteDetailList = response.Data.Items;
                    item.NoteListPager.currentPageSize = response.Data.Items.length;
                    item.NoteListPager.totalRecords = response.Data.TotalItems;
                    $scope.ShowCollpase(true);
                }
                ShowMessages(response);
                item.GetNoteClientListAjaxCall = false;
            });
            $scope.SearchNoteModel.ReferralID = oldReferralId;
        }
    };
    //$scope.NoteListPager.getDataCallback = $scope.GetClientNoteDetailList;
    $scope.DeleteNote = function (item, note, title) {
        //if (note != undefined && !note.AllowEdit) {
        if (note != undefined && 1 === 2) {
            ShowMessage(window.NoteDeleteExistMessage, "error");
        } else {
            var isDeleted = false;
            if (title == undefined) {
                title = window.UpdateRecords;
            } else {
                isDeleted = title === window.Disable;
            }
            bootboxDialog(function (result) {
                if (result) {
                    $scope.SearchNoteModel.ListOfIdsInCsv = note.NoteID > 0 ? note.NoteID.toString() : $scope.SelectedNoteIds.toString();
                    var oldReferralId = $scope.SearchNoteModel.ReferralID;
                    $scope.SearchNoteModel.ReferralID = item.ReferralID;
                    var jsonData = $scope.SetPostData(item.NoteListPager.currentPage, item);
                    item.GetNoteClientListAjaxCall = true;
                    AngularAjaxCall($http, SiteUrl.DeleteNoteURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                        if (response.IsSuccess) {
                            if (note != undefined)
                                note.IsDeleted = isDeleted;
                            item.ClientNoteDetailList = response.Data.Items;
                            item.NoteListPager.currentPageSize = response.Data.Items.length;
                            item.NoteListPager.totalRecords = response.Data.TotalItems;
                            $scope.ShowCollpase(true);
                        }
                        ShowMessages(response);
                        item.GetNoteClientListAjaxCall = false;
                    });
                    $scope.SearchNoteModel.ReferralID = oldReferralId;
                }
            }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
            //}

        }
    };


    //#endregion



    // This executes when select single checkbox selected in table.
    $scope.SelectNote = function (note) {
        if (note.IsChecked)
            $scope.SelectedNoteIds.push(note.NoteID);
        else
            $scope.SelectedNoteIds.remove(note.NoteID);

        if ($scope.SelectedNoteIds.length == $scope.NoteListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };
    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedNoteIds = [];
        angular.forEach($scope.NoteList, function (item, key) {

            item.IsChecked = $scope.SelectAllCheckbox;// event.target.checked;
            if (item.IsChecked)
                $scope.SelectedNoteIds.push(item.NoteID);
        });

        return true;
    };

    $("a#referralNote").on('shown.bs.tab', function (e) {
        $scope.GetNoteList();
        $scope.ShowCollpase();
    });


    if (!$scope.NoteModel.IsPartial) {
        //$scope.GetNoteList();
    }



    $scope.DownloadMonthlySummaryPdf = function (monthlySummaryIds) {
        //
        if (monthlySummaryIds) {
            var jsonData = angular.toJson({ searchSnapshotPrintModel: { ReferralMonthlySummariesIDs: monthlySummaryIds } });
            AngularAjaxCall($http, SiteUrl.GetSnapshotPrintReportUrl, jsonData, "Post", "json", "application/json", true).success(function (response) {
                if (response.IsSuccess) {
                    window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                } else {
                    ShowMessages(response);
                }
            });
        }
    };

    //#endregion

    //#region ADD NOTE Related Stuff
    //#region Add Note
    $scope.ServiceCodeTokenObj = {};
    $scope.ReferralTokenObj = {};
    $scope.AllowToGetServiceCode = true;

    //#region Token input related code for service code
    $scope.ShowGropCodeSelectedWarning = false; // GROUP CODE MEANS, CODE HAS MODIFIER
    $scope.ServiceCodeResultsFormatter = function (item) {
        $scope.ShowGropCodeSelectedWarning = true;
        var data = "";
        if (item.ModifierName)
            data = "<small><b style='color:#007bff;'>" + item.ModifierName + "</b></small><br/>";
        return "<li id='{0}' class='token-seprator'>{0}: {1}<br/>{10}<small><b>{6}:</b> {2}</small><small><b style='padding-left:10px;'>{7}: </b>{3}</small><br/><small><b style='color:#ad0303;'>{8}: </b>{4}</small><small><b style='color:#ad0303;padding-left:10px;'>{9}: </b>{5} </small></li>"
            .format(
            item.ServiceCode,
            item.ServiceName,
            item.MaxUnit,
            item.DailyUnitLimit,
            item.IsBillable ? window.Yes : window.No,
            item.HasGroupOption ? window.Yes : window.No,
            window.MaxUnit,
            window.DailyUnitLimit,
            window.Billable,
            window.GroupOption,
            data);
    };
    $scope.ServiceCodeTokenFormatter = function (item) {
        return "<li id='{0}'>{0}</li>".format(item.ServiceCode);
    };
    $scope.RemoveServiceCode = function () {
        if ($scope.CleanAllFields) {
            $scope.AddNotePageModel.Note.PosID = 0;
            if (!$scope.NotCleanServiceCode)
                $scope.AddNotePageModel.Note.ServiceCodeID = null;
            $scope.AddNotePageModel.PosCodes = [];
            $scope.SelectedServiceCodeForPayor = {};
            $scope.AddNotePageModel.Note.StartMile = null;
            $scope.AddNotePageModel.Note.EndMile = null;
            $scope.AddNotePageModel.Note.StrStartTime = null;
            $scope.AddNotePageModel.Note.StrEndTime = null;
            $scope.AddNotePageModel.Note.NoOfStops = null;
            $scope.AddNotePageModel.Note.POSDetail = null;
            $scope.AddNotePageModel.Note.CalculatedUnit = 0;
        }
        $scope.CleanAllFields = true;
        $scope.NotCleanServiceCode = false;
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };
    $scope.AddedServiceCode = function (item) {
        if ($scope.ShowGropCodeSelectedWarning && ValideElement(item.ModifierName) && item.ServiceCode === AlertServiceCodeOnNotePage) {
            bootboxDialog(function () { }, bootboxDialogType.Alert, bootboxDialogTitle.Alert, window.GroupCodelAlert, bootboxDialogButtonText.Ok, btnClass.BtnDanger);
        }

        $scope.AddNotePageModel.Note.SelectedServiceCodeDetails = item;
        $scope.AddNotePageModel.Note.ServiceCode = item.ServiceCode;
        $scope.AddNotePageModel.Note.Description = item.Description;
        $scope.AddNotePageModel.Note.RandomServiceCodeGroupID = item.RandomGroupID;
        $scope.AddNotePageModel.Note.ServiceCodeID = item.ServiceCodeID;
        $scope.AddNotePageModel.Note.UnitType = item.UnitType;
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };
    //#endregion

    //#region Token input related code for service code
    $scope.ReferralDetailResultsFormatter = function (item) {
        return "<li id='{0}' class='token-seprator'>{0}: {1}<br/><small><b>{4}:</b> {2}</small><small><b style='padding-left:10px;'>{5}: </b>{3}</small><br/><small><b style='color:#ad0303;'>{6}:</b> {7}</small><br/><small><b style='color:#ad0303;'>{8}:</b> {9}</small></li>"
            .format(
                window.Name,
                item.Name,
                item.AHCCCSID ? item.AHCCCSID : 'N/A',
                item.CISNumber ? item.CISNumber : 'N/A',
                window.AHCCCSID,
                window.CISNumber,
                window.Phone,
                item.Phone1 ? item.Phone1 : 'N/A',
                window.Address,
                item.Address ? item.Address : 'N/A'
            );
    };
    $scope.ReferralTokenFormatter = function (item) {
        return "<li id='{0}'>{0}</li>".format(item.Name);
    };

    //#endregion

    $scope.CleanAllFields = true;
    $scope.TempNoteID = 0;
    $scope.OpenAddNoteModel = function (data, selectedNoteSubDetailsItem) {
        HideErrors("#frmAddNote");
        if (data != undefined && data.NoteID == 0) {  //ADD NEW NOTE FOR CLIENT LEVEL
            $("#model_AddBillalbeNote").modal("show");
            $scope.EncryptedReferralID = data.EncryptedReferralID;
            $scope.ReferralTokenObj.add({ EncryptedReferralID: $scope.EncryptedReferralID, Name: data.ReferralName });
            $scope.TempNoteID = data.NoteID;
        } else {

            //ADD NEW AND UPDATE EXISTING NOTE FOR CLIENT LEVEL
            $scope.SelectedNoteSubDetailsItem = selectedNoteSubDetailsItem;
            if (data != undefined && !data.AllowEdit) {
                ShowMessage(window.NoteExistMessage, "error");
            } else {
                $("#model_AddBillalbeNote").modal("show");
                if ($scope.NoteModel.IsPartial) {
                    $scope.LoadNoteModel(data);
                } else if (data && data.NoteID > 0) {
                    $scope.EncryptedReferralID = data.EncryptedReferralID;
                    $scope.ReferralTokenObj.add({ EncryptedReferralID: $scope.EncryptedReferralID, Name: data.ReferralName });
                    $scope.TempNoteID = data.NoteID;
                    //$scope.LoadNoteModel(data);
                }
            }
        }
    };

    $scope.RemoveReferral = function () {
        $scope.EncryptedReferralID = null;
        $scope.AddNotePageModel.Note = {};
        $scope.AddNotePageModel.ClientInfo = {};
        $scope.AddNotePageModel.SelectedDxCodes = [];
        $scope.SelectAllDxCodeCheckbox = false;
        $scope.TempNoteID = 0;
        $scope.ReferralTokenObj.clear();
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };

    $scope.LoadNoteModel = function (data, noteSubDetailsItem) {
        //$scope.AddNotePageModel.NoteSubDetailsItem = $scope.SelectedNoteSubDetailsItem;
        var jsonData = angular.toJson({
            id: $scope.EncryptedReferralID,
            noteID: data != undefined ? data.NoteID : 0
        });
        AngularAjaxCall($http, SiteUrl.SetAddNotePageURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.AddNotePageModel = response.Data;
                $scope.CurrentDate = moment();
                if (!$scope.NoteModel.IsPartial && $scope.AddNotePageModel.Note.noteID > 0) {
                    $scope.EncryptedReferralID = $scope.AddNotePageModel.EncryptedReferralID;
                }
                if ($scope.AddNotePageModel.Note.NoteID == 0) {
                    $scope.AddNotePageModel.Note.ServiceCodeID = null;
                    $scope.AddNotePageModel.Note.ServiceDate = null;
                    $scope.AddNotePageModel.Note.StartMile = null;
                    $scope.AddNotePageModel.Note.EndMile = null;
                    $scope.AddNotePageModel.Note.ServiceDate = new Date();
                    //$scope.SelectAllDxCodeCheckbox = false;

                    $timeout(function () {
                        if ($scope.IsSameDxCodeType) {
                            $scope.SelectAllDxCodeCheckbox = true;
                            $scope.SelectAllDxCode(true);
                        }
                    }, 1000);
                }
                else if ($scope.AddNotePageModel.Note.ServiceCodeID > 0) {
                    //$scope.CleanAllFields = false;

                    $timeout(function () {
                        $scope.ServiceCodeTokenObj.add({ ServiceCodeID: $scope.AddNotePageModel.Note.ServiceCodeID, ServiceCode: $scope.AddNotePageModel.Note.ServiceCode });
                    });

                    $scope.AllowToGetServiceCode = false;
                    if ($scope.AddNotePageModel.Note.NoteID > 0 && $scope.AddNotePageModel.Note.PosID && $scope.AddNotePageModel.Note.PosID > 0) {
                        $timeout(function () {
                            $scope.AddNotePageModel.Note.PosID = $scope.AddNotePageModel.Note.PosID.toString();
                            $scope.$apply();
                        });
                    }

                }

                if ($scope.AddNotePageModel.Note.NoteID > 0) {
                    $scope.AddNotePageModel.SelectedDxCodes = [];
                    angular.forEach($scope.AddNotePageModel.DxCodes, function (item, key) {
                        if (item.IsChecked) {
                            $scope.AddNotePageModel.NoteDxCodeCount += 1;
                            $scope.AddNotePageModel.SelectedDxCodes.push(item);
                        }


                        if ($scope.AddNotePageModel.SelectedDxCodes.length == $scope.AddNotePageModel.DxCodes.length)
                            $scope.SelectAllDxCodeCheckbox = true;
                        else
                            $scope.SelectAllDxCodeCheckbox = false;
                    });
                }

                $scope.SetProvidersBasedOnPayor();

            } else {
                ShowMessages(response);
            }

        });
    };


    $scope.CheckNoteValidation = function () {

        var liString = "<li>{0} : {1}</li>";
        var isValid = true;
        $scope.ListofFields = "<ul>";

        if ($scope.AddNotePageModel.TempNoteList == undefined || $scope.AddNotePageModel.TempNoteList.length === 0) {
            $scope.ListofFields += liString.format(window.Missing, window.Services);
            isValid = false;
        }

        if (!ValideElement($scope.AddNotePageModel.Note.Assessment) && $scope.AddNotePageModel.Note.ServiceCodeType !== $scope.OtherServiceCode) {
            $scope.ListofFields += liString.format(window.Missing, window.Assessment);
            isValid = false;
        }

        if (($scope.AddNotePageModel.Note.MarkAsComplete === false || $scope.AddNotePageModel.Note.MarkAsComplete === undefined) && ValideElement($scope.AddNotePageModel.Note.NoteAssignee) === false) {
            $scope.ListofFields += liString.format(window.Missing, window.NoteAssignee);
            isValid = false;
        }
        //if (($scope.AddNotePageModel.Note.MarkAsComplete === false || $scope.AddNotePageModel.Note.MarkAsComplete === undefined) && ValideElement($scope.AddNotePageModel.Note.NoteComments) === false) {
        //    $scope.ListofFields += liString.format(window.Missing, window.NoteComments);
        //    isValid = false;
        //}
        return isValid;
    }

    $scope.SaveNote = function (selectedNoteSubDetailsItem) {
        //var isValid = CheckErrors($("#frmAddNote"));
        //if ($scope.AddNotePageModel.Note.NoteAssignee && $scope.AddNotePageModel.Note.NoteComments)
        //    var valid = 1;
        //else if ($scope.AddNotePageModel.Note.MarkAsComplete === false) {
        //    toastr.error(window.CanNotSaveNoteAssignmentMissing);
        //    return false;
        //}


        var isValid = $scope.CheckNoteValidation();
        if (!isValid) {
            bootboxDialog(function () {
            }, bootboxDialogType.Alert, window.Alert, window.NoteIncomplete.format($scope.ListofFields));
        }

        //var isValid = $scope.AddNotePageModel.TempNoteList.length > 0;
        if (isValid) {
            var jsonData = angular.toJson({
                note: $scope.AddNotePageModel.Note,
                referralID: $scope.EncryptedReferralID,
                selectedServiceCodeDetail: $scope.SelectedServiceCodeForPayor,
                dxCodes: $scope.AddNotePageModel.SelectedDxCodes,
                tempNoteList: $scope.AddNotePageModel.TempNoteList
            });
            //AngularAjaxCall($http, SiteUrl.SaveNoteURL, jsonData, "post", "json", "application/json", true).
            AngularAjaxCall($http, SiteUrl.SaveMultiNoteURL, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        SetMessageForPageLoad(response.Message, "ShowNoteMessage");
                        if (!$scope.NoteModel.IsPartial) {
                            $scope.ReferralTokenObj.clear();
                            $scope.IsDifferentDxCode = false;
                            if (!$scope.$root.$$phase) {
                                $scope.$apply();
                            }
                        }
                        $("#model_AddBillalbeNote").modal("hide");

                        if ($scope.NoteClientList.length != 0) {
                            if (selectedNoteSubDetailsItem) //CHILD DIVISION CALLED HERE
                                selectedNoteSubDetailsItem.NoteListPager.getDataCallback(selectedNoteSubDetailsItem, true);
                            else { //PARENT DIVISION CALLED HERE
                                $scope.NoteClientListPager.getDataCallback();
                            }
                        }
                    }

                    ShowMessages(response);

                });
        } else {
            $('.tbl_ReqServiceMsg').focus();
            toastr.error(window.CanNotSave);
        }
    };


    $scope.SaveAndContinue = function (selectedNoteSubDetailsItem) {
        //var isValid = CheckErrors($("#frmAddNote"));
        var isValid = $scope.AddNotePageModel.TempNoteList.length > 0;
        if (isValid) {
            var jsonData = angular.toJson({
                note: $scope.AddNotePageModel.Note,
                referralID: $scope.EncryptedReferralID,
                selectedServiceCodeDetail: $scope.SelectedServiceCodeForPayor,
                dxCodes: $scope.AddNotePageModel.SelectedDxCodes,
                tempNoteList: $scope.AddNotePageModel.TempNoteList
            });
            //AngularAjaxCall($http, SiteUrl.SaveNoteURL, jsonData, "post", "json", "application/json", true).
            AngularAjaxCall($http, SiteUrl.SaveMultiNoteURL, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        SetMessageForPageLoad(response.Message, "ShowNoteMessage");
                        $("#ServiceCodeIDToken").tokenInput("clear");
                        $("#Note_ZarephathService").focus();
                        $scope.AddNotePageModel.Note.NoteID = 0;

                        if ($scope.NoteClientList.length != 0) {
                            if (selectedNoteSubDetailsItem) //CHILD DIVISION CALLED HERE
                                selectedNoteSubDetailsItem.NoteListPager.getDataCallback(selectedNoteSubDetailsItem, true);
                            else { //PARENT DIVISION CALLED HERE
                                $scope.NoteClientListPager.getDataCallback();
                            }
                        }
                    }

                    ShowMessages(response);

                });
        } else {
            $('.tbl_ReqServiceMsg').focus();
            toastr.error(window.CanNotSave);
        }
    };

    $scope.CloseAddNoteModel = function () {


        bootboxDialog(function (result) {
            if (result) {
                $("#model_AddBillalbeNote").modal("hide");
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, window.CloseNoteEntryAlert);

    };


    //#region Temp Note

    $scope.AddTempNote = function (note) {
        var isValid = CheckErrors($("#frmAddNote"));

        if (isValid) {
            note.SelectedServiceCodeForPayor = angular.copy($scope.SelectedServiceCodeForPayor);

            //#region Set Unique ID TO Each Note
            if (note.DummyNoteID === undefined || note.DummyNoteID === null) {
                note.DummyNoteID = GenerateGuid();
            }

            var isNew = true;
            $.each($scope.AddNotePageModel.TempNoteList, function (index, data) {
                if ((data.NoteID > 0 && data.NoteID === note.NoteID) || note.DummyNoteID === data.DummyNoteID) {
                    $scope.AddNotePageModel.TempNoteList[index] = angular.copy(note);
                    isNew = false;
                    return;
                }

            });

            //#endregion

            //#region Set Unique Random Group Code for Group Services  EX: Set a Unique Code for The A0120 & S0250 Service Codes
            if ($scope.AddNotePageModel.Note.RandomServiceCodeGroupID) {
                if ($scope.AddNotePageModel.Note.GroupIDForMileServices === undefined || $scope.AddNotePageModel.Note.GroupIDForMileServices === null) {
                    $scope.AddNotePageModel.Note.GroupIDForMileServices = GenerateGuid();
                }
            }

            //#endregion


            if (isNew)
                $scope.AddNotePageModel.TempNoteList.push(angular.copy(note));


            if ($scope.AddNotePageModel.Note.RandomServiceCodeGroupID && isNew) {
                bootboxDialog(function (result) {
                    if (result == undefined) { // FOR TYPE ALERT
                        //Reset Selcted Checkbox items and Control
                        var jsonData = angular.toJson({
                            encReferralID: $scope.EncryptedReferralID,
                            tempNote: $scope.AddNotePageModel.Note
                        });
                        AngularAjaxCall($http, SiteUrl.GetAutoCreateServiceInformationURL, jsonData, "post", "json", "application/json", true).
                            success(function (response) {
                                if (response.IsSuccess) {
                                    if (response.Data && response.Data.length > 0) {
                                        $.each(response.Data, function (index, data) {
                                            data.DummyNoteID = GenerateGuid();
                                            $scope.AddNotePageModel.TempNoteList.push(angular.copy(data));
                                        });
                                    } else {
                                        toastr.info(window.GroupServicesNotConfigured.format(note.ServiceCode, $scope.AddNotePageModel.ClientInfo.PayorShortName));
                                    }
                                }
                            });

                    }

                    note.NoteID = 0;
                    note.DummyNoteID = null;
                    $timeout(function () {
                        $(".cancel.ClearDetails").click();
                    });

                }, bootboxDialogType.OnlyConfirm, bootboxDialogTitle.Alert, window.CreateGroupServiceConfrimation.format(note.ServiceCode));
            }
            else if ($scope.AddNotePageModel.Note.GroupIDForMileServices && !isNew) {
                var jsonData = angular.toJson({
                    encReferralID: $scope.EncryptedReferralID,
                    tempNote: $scope.AddNotePageModel.Note
                });
                AngularAjaxCall($http, SiteUrl.GetAutoCreateServiceInformationURL, jsonData, "post", "json", "application/json", true).
                    success(function (response) {
                        if (response.IsSuccess) {
                            if (response.Data && response.Data.length > 0) {

                                $.each(response.Data, function (index, data) {

                                    $.each($scope.AddNotePageModel.TempNoteList, function (index, dataItem) {

                                        if (dataItem.GroupIDForMileServices === $scope.AddNotePageModel.Note.GroupIDForMileServices) {
                                            if ((dataItem.NoteID > 0 && dataItem.NoteID !== $scope.AddNotePageModel.Note.NoteID)
                                                || ((dataItem.NoteID === 0 || dataItem.NoteID === undefined) && dataItem.DummyNoteID !== $scope.AddNotePageModel.Note.DummyNoteID)) {

                                                $scope.AddNotePageModel.TempNoteList[index] = angular.copy(data);
                                                $scope.AddNotePageModel.TempNoteList[index].NoteID = dataItem.NoteID;
                                                $scope.AddNotePageModel.TempNoteList[index].GroupIDForMileServices = dataItem.GroupIDForMileServices;
                                                $scope.AddNotePageModel.TempNoteList[index].DummyNoteID = dataItem.DummyNoteID;
                                            }
                                        }

                                    });

                                });
                            } else {
                                toastr.info(window.GroupServicesNotConfigured.format(note.ServiceCode, $scope.AddNotePageModel.ClientInfo.PayorShortName));
                            }
                        }
                        note.NoteID = 0;
                        note.DummyNoteID = null;
                        $timeout(function () {
                            $(".cancel.ClearDetails").click();
                        });
                    });

            }
            else {

                note.NoteID = 0;
                note.DummyNoteID = null;
                $timeout(function () {
                    $(".cancel.ClearDetails").click();
                });

            }


            //note.NoteID = 0;
            //note.DummyNoteID = null;
            //$timeout(function () {
            //    $(".cancel.ClearDetails").click();
            //});

        } else {
            toastr.error(window.CanNotSave);
            $('#model_AddBillalbeNote').animate({ scrollTop: 50 }, 'slow');
        }


    };
    $scope.ClearTempNote = function (loadAtList) {
        $scope.CleanAllFields = true;
        $scope.ServiceCodeTokenObj.clear();
        $scope.AddNotePageModel.Note.DummyNoteID = null;
        $scope.AddNotePageModel.Note.ServiceCodeType = 0;

        //var date = $scope.AddNotePageModel.Note.ServiceDate;
        //$scope.AddNotePageModel.Note = {};
        //$scope.AddNotePageModel.Note.ServiceDate = date;

        //
        var CL_TempData = angular.copy($scope.AddNotePageModel.Note);
        $scope.AddNotePageModel.Note = {};
        $scope.AddNotePageModel.Note.NoteAssignee = CL_TempData.NoteAssignee;
        $scope.AddNotePageModel.Note.NoteComments = CL_TempData.NoteComments;
        $scope.AddNotePageModel.Note.ServiceDate = CL_TempData.ServiceDate;
        $scope.AddNotePageModel.Note.Assessment = CL_TempData.Assessment;
        $scope.AddNotePageModel.Note.ActionPlan = CL_TempData.ActionPlan;
        $scope.AddNotePageModel.Note.Signature = CL_TempData.Signature;
        $scope.AddNotePageModel.Note.MarkAsComplete = CL_TempData.MarkAsComplete;
        $scope.AddNotePageModel.Note.EmpSignatureDetails = CL_TempData.EmpSignatureDetails;
        $scope.AddNotePageModel.Note.PayorID = CL_TempData.PayorID;

        $scope.TempNoteID = 0;
        if (loadAtList) {
            $('.tbl_ReqServiceMsg').focus();
        }
        else
            $('#model_AddBillalbeNote').animate({ scrollTop: 0 }, 'slow');
    };
    $scope.NotCleanServiceCode = false;
    $scope.EditTempNote = function (tempNote) {
        //
        if (((tempNote.NoteID === 0 || tempNote.NoteID === undefined) && tempNote.DummyNoteID === $scope.AddNotePageModel.Note.DummyNoteID) || (tempNote.NoteID > 0 && tempNote.NoteID === $scope.AddNotePageModel.Note.NoteID)) {
            $('#model_AddBillalbeNote').animate({ scrollTop: 0 }, 'slow');
            return;
        }
        //$scope.TempEditFirstLoad = true;
        $scope.CleanAllFields = true;
        $scope.NotCleanServiceCode = true;
        $scope.ServiceCodeTokenObj.clear();
        $scope.AddNotePageModel.Note.ServiceCodeID = 0;

        setTimeout(function () {
           // 
            var data = angular.copy(tempNote);
            $scope.SelectedServiceCodeForPayor = data.SelectedServiceCodeForPayor;
            //$scope.AddNotePageModel.Note = data;
            
            var CL_TempData = angular.copy($scope.AddNotePageModel.Note);
            //$scope.AddNotePageModel.Note = {};
            $scope.AddNotePageModel.Note = data;
            $scope.AddNotePageModel.Note.NoteAssignee = CL_TempData.NoteAssignee;
            $scope.AddNotePageModel.Note.NoteComments = CL_TempData.NoteComments;
            $scope.AddNotePageModel.Note.Assessment = CL_TempData.Assessment;
            $scope.AddNotePageModel.Note.ActionPlan = CL_TempData.ActionPlan;
            $scope.AddNotePageModel.Note.Signature = CL_TempData.Signature;
            $scope.AddNotePageModel.Note.MarkAsComplete = CL_TempData.MarkAsComplete;
            $scope.AddNotePageModel.Note.EmpSignatureDetails = CL_TempData.EmpSignatureDetails;



            if (!$scope.$root.$$phase)
                $scope.$apply();
            if ($scope.AddNotePageModel.Note.ServiceCodeID && $scope.AddNotePageModel.Note.ServiceCodeType !== $scope.OtherServiceCode) {
                $timeout(function () {
                    $scope.ServiceCodeTokenObj.add({ ServiceCodeID: $scope.AddNotePageModel.Note.ServiceCodeID, ServiceCode: $scope.AddNotePageModel.Note.ServiceCode, Description: $scope.AddNotePageModel.Note.Description });
                });
            }

        });

        //$timeout(function () {
        //    $scope.ServiceCodeTokenObj.add({ ServiceCodeID: $scope.AddNotePageModel.Note.ServiceCodeID, ServiceCode: $scope.AddNotePageModel.Note.ServiceCode });
        //});


        $('#model_AddBillalbeNote').animate({ scrollTop: 0 }, 'slow');
        //$scope.TempNoteID = 0;
    };

    $scope.DisableTempNote = function (tempNote, index, itemText) {

        if (tempNote.NoteID > 0) {
            $scope.DeleteNote(tempNote, itemText);
        } else {
            $scope.AddNotePageModel.TempNoteList.splice(index, 1);
        }

        $scope.ClearTempNote(true);

        //if (tempNote.NoteID <= 0 || tempNote.NoteID == undefined)
        //    $scope.AddNotePageModel.TempNoteList.splice(index, 1);
        //else {
        //    $scope.DeleteNote(tempNote, itemText);
        //    $scope.ClearTempNote(true);
        //}
        //$scope.TempNoteID = 0;
    };

    //#endregion



    //function() { return  $scope.AddNotePageModel.Note.ServiceCodeID; }  //'AddNotePageModel.Note.ServiceCodeID'
    $scope.$watch(function () { return $scope.AddNotePageModel.Note.ServiceCodeID; }, function (newValue, oldValue) {
        if (parseInt(newValue) > 0) {
            $scope.GetPosCodes(newValue);
            $scope.AddNotePageModel.Note.PayorServiceCodeMappingID = $scope.SelectedServiceCodeForPayor.PayorServiceCodeMappingID;
        } else if (oldValue > 0) {
            $scope.AllowToGetServiceCode = true;
        }

        if (!$scope.$root.$$phase)
            $scope.$apply();
    }, true);

    $scope.$watch('EncryptedReferralID', function (newValue, oldValue) {
        if (newValue != null) {
            var data = {
                NoteID: $scope.TempNoteID
            };
            $scope.LoadNoteModel(data);
        }

        if (!$scope.$root.$$phase)
            $scope.$apply();
    });

    $scope.$watch(function () { return $scope.AddNotePageModel.Note.PosID; }, function (newValue, oldValue) {
        if (parseInt(newValue) > 0) {
            angular.forEach($scope.AddNotePageModel.PosCodes, function (value, key) {
                if (value.PosID == $scope.AddNotePageModel.Note.PosID) {
                    $scope.SelectedServiceCodeForPayor = value;
                    $scope.WatchFuncForMileUnitCalculation();
                    $scope.WatchFuncForTimeUnitCalculation();
                    return;
                }
            });

            if ($scope.SelectedServiceCodeForPayor.PosID != $scope.OtherPOS) {
                $scope.AddNotePageModel.Note.POSDetail = window.CommunityMentalHealthCenter;
            } else if (!$scope.AddNotePageModel.Note.NoteID || (parseInt(oldValue) > 0 && parseInt(newValue) != parseInt(oldValue))) {
                if ($scope.AddNotePageModel.Note.DummyNoteID == undefined || $scope.AddNotePageModel.Note.DummyNoteID == null)
                    $scope.AddNotePageModel.Note.POSDetail = null;
            }

        } else if (parseInt(oldValue) > 0) {
            $scope.SelectedServiceCodeForPayor = {};
            $scope.AddNotePageModel.Note.StartMile = null;
            $scope.AddNotePageModel.Note.EndMile = null;
            $scope.AddNotePageModel.Note.StrStartTime = null;
            $scope.AddNotePageModel.Note.StrEndTime = null;
            $scope.AddNotePageModel.Note.NoOfStops = null;
            $scope.AddNotePageModel.Note.POSDetail = null;
            if (!$scope.$root.$$phase) {
                $scope.$apply();
            }
        }
        $scope.AddNotePageModel.Note.PayorServiceCodeMappingID = $scope.SelectedServiceCodeForPayor.PayorServiceCodeMappingID;
    });




    $scope.AdditionFilterForServiceCode = null;

    $('#model_AddBillalbeNote').on('hidden.bs.modal', function () {
        $scope.CleanAllFields = true;
        $scope.ServiceCodeTokenObj.clear();
        $scope.SelectedServiceCodeForPayor = {};
        if (!$scope.NoteModel.IsPartial) {
            $("#AddNoteReferralIDTkn").tokenInput("clear");
            $scope.EncryptedReferralID = null;
            $scope.TempNoteID = 0;
            $scope.AddNotePageModel.Note = {};
        }
    });
    $scope.IsDifferentDxCode = false;
    $scope.SelectDxCode = function (dxCode) {
        if (dxCode.IsChecked) {
            var count = 0;
            if ($scope.AddNotePageModel.SelectedDxCodes.length > 0) {
                count = $scope.AddNotePageModel.SelectedDxCodes.filter(function (item) {
                    return item.DxCodeShortName != dxCode.DxCodeShortName;
                }).length;
            }
            if (count > 0) {
                $scope.IsDifferentDxCode = true;
                dxCode.IsChecked = false;
            } else {
                $scope.AddNotePageModel.SelectedDxCodes.push(dxCode);
                $scope.AddNotePageModel.NoteDxCodeCount += 1;
                $scope.IsDifferentDxCode = false;
                //$('#NoteDxCodeCount').valid();               
                //if (!$scope.$root.$$phase) {
                //    $scope.$apply();
                //}
            }

        } else {
            $scope.AddNotePageModel.SelectedDxCodes.remove(dxCode);
            $scope.AddNotePageModel.NoteDxCodeCount -= 1;
        }


        if ($scope.AddNotePageModel.SelectedDxCodes.length == $scope.AddNotePageModel.DxCodes.length)
            $scope.SelectAllDxCodeCheckbox = true;
        else
            $scope.SelectAllDxCodeCheckbox = false;

    };

    $scope.FilterDxCodes = [];
    $scope.SelectAllDxCode = function (data) {
        $scope.AddNotePageModel.SelectedDxCodes = [];
        $scope.AddNotePageModel.NoteDxCodeCount = 0;
        angular.forEach($scope.FilterDxCodes, function (item, key) {
            item.IsChecked = data;
            if (item.IsChecked) {
                $scope.AddNotePageModel.SelectedDxCodes.push(item);
                $scope.AddNotePageModel.NoteDxCodeCount += 1;
            }
        });

        return true;
    };
    $scope.IsSameDxCodeType = true;
    $scope.ServiceCodeName = null;
    $scope.DxCodeFilter = function () {
        $scope.IsSameDxCodeType = true;
        $scope.ServiceCodeName = null;
        return function (item) {
            if ($scope.AddNotePageModel.Note.ServiceDate) {
                var isValid = false;
                if (moment($scope.AddNotePageModel.Note.ServiceDate) >= moment(item.StartDate)) {
                    if (item.EndDate) {
                        if (moment($scope.AddNotePageModel.Note.ServiceDate) <= moment(item.EndDate)) {
                            if ($scope.ServiceCodeName && $scope.ServiceCodeName != item.DxCodeShortName) {
                                $scope.IsSameDxCodeType = false;
                            }
                            $scope.ServiceCodeName = item.DxCodeShortName;
                            isValid = true;
                        }
                        //else {

                        //    $scope.IsSameDxCodeType = true;
                        //}
                    } else {
                        if ($scope.ServiceCodeName && $scope.ServiceCodeName != item.DxCodeShortName) {
                            $scope.IsSameDxCodeType = false;
                        }
                        $scope.ServiceCodeName = item.DxCodeShortName;
                        isValid = true;
                    }
                }
                //    else {
                //    $scope.IsSameDxCodeType = true;
                //}
                if (isValid)
                    return item;
            }
        };
    };
    $scope.SelectFirstDxCode = function (checkbox, index) {
        $timeout(function () {

            if (index == 0) {
                if ($(checkbox).is(':checked')) {
                    $(checkbox).click();
                }
                $(checkbox).click();

                //dxCode.IsChecked = true;
                // self.SelectDxCode(dxCode);
            }
        });
    };

    $scope.$watch(function () {
        return moment(new Date($scope.AddNotePageModel.Note.ServiceDate)).format("YYYY/MM/DD") + $scope.AddNotePageModel.Note.ServiceCodeType;
    }, function () {
        if ($scope.AddNotePageModel.Note.ServiceCodeID > 0) {
            $scope.CleanAllFields = true;
            $scope.ServiceCodeTokenObj.clear();
        }

        if (!($scope.AddNotePageModel.Note.ServiceDate && $scope.AddNotePageModel.Note.ServiceDate.toString() != "")) {
            $scope.AddNotePageModel.Note.ServiceDate = '0001-01-01T00:00:00';
        }

        //scope.SelectFirstDxCode('.dxCode0', 0);


        //if (($scope.AddNotePageModel.Note.ServiceDate && $scope.AddNotePageModel.Note.ServiceCodeType && $scope.AddNotePageModel.Note.ServiceDate.toString() != "" && $scope.AddNotePageModel.Note.ServiceCodeType.toString() != "")) {
        //    $scope.AddNotePageModel.Note.ServiceDate = $scope.AddNotePageModel.Note.ServiceDate;
        //} else {
        //    $scope.AddNotePageModel.Note.ServiceDate = '0001-01-01T00:00:00';
        //}

        $scope.AdditionFilterForServiceCode = angular.toJson({
            encReferralID: $scope.EncryptedReferralID,
            serviceDate: $scope.AddNotePageModel.Note.ServiceDate,
            serviceCodeTypeID: $scope.AddNotePageModel.Note.ServiceCodeType
        });


        if ($scope.AddNotePageModel.Note.ServiceCodeType != $scope.OtherServiceCode) {
            $scope.AddNotePageModel.Note.SpokeTo = null;
            $scope.AddNotePageModel.Note.Relation = null;
            $scope.AddNotePageModel.Note.OtherNoteType = null;
            if (!$scope.$root.$$phase) {
                $scope.$apply();
            }
        }

        if ($scope.AddNotePageModel.Note.ServiceCodeType == $scope.OtherServiceCode && $scope.AddNotePageModel.Note.NoteID == 0) {
            angular.forEach($scope.AddNotePageModel.Facilities, function (value, key) {
                if (value.Name == window.FacilityForOtherServiceCode) {
                    $scope.AddNotePageModel.Note.BillingProviderID = value.Value;
                    $scope.AddNotePageModel.Note.RenderingProviderID = value.Value;
                    return;
                }
            });
        } else {
            // $scope.AddNotePageModel.Note.BillingProviderID = '';
            //$scope.AddNotePageModel.Note.RenderingProviderID = '';
        }

    });



    //#region Calculate Units depends on date and mile value
    $scope.$watch(function () {
        return $scope.AddNotePageModel.Note.StartMile + $scope.AddNotePageModel.Note.EndMile;
    }, function () {
        $scope.WatchFuncForMileUnitCalculation();
    });
    $scope.WatchFuncForMileUnitCalculation = function () {
        if (parseInt($scope.AddNotePageModel.Note.EndMile) > 0 && ($scope.SelectedServiceCodeForPayor.UnitType == window.StopUnit || $scope.SelectedServiceCodeForPayor.UnitType == window.DistanceInMilesUnit)) {
            if ((parseInt($scope.AddNotePageModel.Note.EndMile) >= parseInt($scope.AddNotePageModel.Note.StartMile))) {
                $scope.MileDifference = $scope.AddNotePageModel.Note.EndMile - $scope.AddNotePageModel.Note.StartMile;

                var calUnit = 1;
                if ($scope.MileDifference > $scope.SelectedServiceCodeForPayor.PerUnitQuantity)
                    $scope.MileDifference = $scope.MileDifference - $scope.SelectedServiceCodeForPayor.PerUnitQuantity;
                else
                    $scope.MileDifference = 0;

                calUnit = calUnit + Math.round($scope.MileDifference / $scope.SelectedServiceCodeForPayor.PerUnitQuantity);

                //var calUnit = Math.round($scope.MileDifference / $scope.SelectedServiceCodeForPayor.PerUnitQuantity);
                $scope.SelectedServiceCodeForPayor.UsedUnit = calUnit;

                $scope.SelectedServiceCodeForPayor.MileDiff = $scope.MileDifference;
                $scope.AddNotePageModel.Note.CalculatedUnit = calUnit;
                $scope.SelectedServiceCodeForPayor.CalculatedUnit = calUnit;

                if ($scope.SelectedServiceCodeForPayor.UnitType == window.StopUnit || $scope.SelectedServiceCodeForPayor.DefaultUnitIgnoreCalculation > 0) {
                    $scope.AddNotePageModel.Note.CalculatedUnit = $scope.SelectedServiceCodeForPayor.DefaultUnitIgnoreCalculation;
                    $scope.SelectedServiceCodeForPayor.CalculatedUnit = $scope.SelectedServiceCodeForPayor.DefaultUnitIgnoreCalculation;
                }

                //if (($scope.SelectedServiceCodeForPayor.DailyUnitLimit == 0 && $scope.SelectedServiceCodeForPayor.MaxUnit == 0) || $scope.SelectedServiceCodeForPayor.UsedUnit <= $scope.SelectedServiceCodeForPayor.AvailableDailyUnit) {
                //    $scope.AddNotePageModel.Note.CalculatedUnit = $scope.SelectedServiceCodeForPayor.UsedUnit;
                //    $scope.SelectedServiceCodeForPayor.CalculatedUnit = $scope.SelectedServiceCodeForPayor.UsedUnit;
                //} else if ($scope.SelectedServiceCodeForPayor.DailyUnitLimit == 0 && $scope.SelectedServiceCodeForPayor.MaxUnit != 0) {
                //    if ($scope.SelectedServiceCodeForPayor.UsedUnit <= $scope.SelectedServiceCodeForPayor.AvailableMaxUnit) {
                //        $scope.AddNotePageModel.Note.CalculatedUnit = $scope.SelectedServiceCodeForPayor.UsedUnit;
                //        $scope.SelectedServiceCodeForPayor.CalculatedUnit = $scope.SelectedServiceCodeForPayor.UsedUnit;
                //    } else {
                //        $scope.AddNotePageModel.Note.CalculatedUnit = $scope.SelectedServiceCodeForPayor.AvailableDailyUnit;
                //        $scope.SelectedServiceCodeForPayor.CalculatedUnit = $scope.SelectedServiceCodeForPayor.AvailableDailyUnit;
                //    }

                //} else {
                //    $scope.AddNotePageModel.Note.CalculatedUnit = $scope.SelectedServiceCodeForPayor.AvailableDailyUnit;
                //    $scope.SelectedServiceCodeForPayor.CalculatedUnit = $scope.SelectedServiceCodeForPayor.AvailableDailyUnit;
                //}
            } else {
                $scope.SelectedServiceCodeForPayor.UsedUnit = 0;
                $scope.AddNotePageModel.Note.CalculatedUnit = 0;
                $scope.SelectedServiceCodeForPayor.CalculatedUnit = 0;
            }

        }
        if ($scope.AddNotePageModel.Note.EndMile == "" || $scope.AddNotePageModel.Note.StartMile == "") {
            $scope.SelectedServiceCodeForPayor.UsedUnit = 0;
            $scope.AddNotePageModel.Note.CalculatedUnit = 0;
            $scope.SelectedServiceCodeForPayor.CalculatedUnit = 0;
        }

        if (!$scope.$root.$$phase)
            $scope.$apply();
    }



    $scope.$watch(function () {
        return $scope.AddNotePageModel.Note.StrStartTime + $scope.AddNotePageModel.Note.StrEndTime;
    }, function () {
        $scope.WatchFuncForTimeUnitCalculation();
    });

    $scope.WatchFuncForTimeUnitCalculation = function () {
        if ($scope.AddNotePageModel.Note.StrStartTime && $scope.AddNotePageModel.Note.StrEndTime && $scope.CheckValidTime($scope.AddNotePageModel.Note.StrStartTime) && $scope.CheckValidTime($scope.AddNotePageModel.Note.StrEndTime) && $scope.SelectedServiceCodeForPayor.UnitType == 1) {
            if (new Date(moment($scope.AddNotePageModel.Note.StrEndTime, "hh:mm a")).getTime() > new Date(moment($scope.AddNotePageModel.Note.StrStartTime, "hh:mm a")).getTime()) {
                var diff = $scope.GetTimeDifferenceInMinutes($scope.AddNotePageModel.Note.StrStartTime, $scope.AddNotePageModel.Note.StrEndTime);
                $scope.SelectedServiceCodeForPayor.MinutesDiff = parseInt(diff);

                var calUnit = 1;
                if (diff > $scope.SelectedServiceCodeForPayor.PerUnitQuantity)
                    diff = diff - $scope.SelectedServiceCodeForPayor.PerUnitQuantity;
                else
                    diff = 0;
                calUnit = calUnit + Math.round(diff / $scope.SelectedServiceCodeForPayor.PerUnitQuantity);

                //var calUnit = Math.round(diff / $scope.SelectedServiceCodeForPayor.PerUnitQuantity);
                //$scope.SelectedServiceCodeForPayor.MinutesDiff = parseInt(diff);

                $scope.SelectedServiceCodeForPayor.UsedUnit = calUnit;

                $scope.AddNotePageModel.Note.CalculatedUnit = calUnit;
                $scope.SelectedServiceCodeForPayor.CalculatedUnit = calUnit;



                if ($scope.SelectedServiceCodeForPayor.DefaultUnitIgnoreCalculation > 0) {
                    $scope.AddNotePageModel.Note.CalculatedUnit = $scope.SelectedServiceCodeForPayor.DefaultUnitIgnoreCalculation;
                    $scope.SelectedServiceCodeForPayor.CalculatedUnit = $scope.SelectedServiceCodeForPayor.DefaultUnitIgnoreCalculation;
                }
                //if (($scope.SelectedServiceCodeForPayor.DailyUnitLimit == 0 && $scope.SelectedServiceCodeForPayor.MaxUnit == 0) || $scope.SelectedServiceCodeForPayor.UsedUnit <= $scope.SelectedServiceCodeForPayor.AvailableDailyUnit) {
                //    $scope.AddNotePageModel.Note.CalculatedUnit = $scope.SelectedServiceCodeForPayor.UsedUnit;
                //    $scope.SelectedServiceCodeForPayor.CalculatedUnit = $scope.SelectedServiceCodeForPayor.UsedUnit;
                //} else if ($scope.SelectedServiceCodeForPayor.DailyUnitLimit == 0 && $scope.SelectedServiceCodeForPayor.MaxUnit != 0) {
                //    if ($scope.SelectedServiceCodeForPayor.UsedUnit <= $scope.SelectedServiceCodeForPayor.AvailableMaxUnit) {
                //        $scope.AddNotePageModel.Note.CalculatedUnit = $scope.SelectedServiceCodeForPayor.UsedUnit;
                //        $scope.SelectedServiceCodeForPayor.CalculatedUnit = $scope.SelectedServiceCodeForPayor.UsedUnit;
                //    } else {
                //        $scope.AddNotePageModel.Note.CalculatedUnit = $scope.SelectedServiceCodeForPayor.AvailableDailyUnit;
                //        $scope.SelectedServiceCodeForPayor.CalculatedUnit = $scope.SelectedServiceCodeForPayor.AvailableDailyUnit;
                //    }

                //} else {
                //    $scope.AddNotePageModel.Note.CalculatedUnit = $scope.SelectedServiceCodeForPayor.AvailableDailyUnit;
                //    $scope.SelectedServiceCodeForPayor.CalculatedUnit = $scope.SelectedServiceCodeForPayor.AvailableDailyUnit;
                //}
            }
            else {
                $scope.SelectedServiceCodeForPayor.UsedUnit = 0;
                $scope.AddNotePageModel.Note.CalculatedUnit = 0;
                $scope.SelectedServiceCodeForPayor.CalculatedUnit = 0;
            }

            if (!$scope.$root.$$phase)
                $scope.$apply();
        }
    }
    //#endregion





    //#region Get Dropdown list for Service codes
    $scope.GetServiceCodes = function () {
        var jsonData = angular.toJson({
            encReferralID: $scope.EncryptedReferralID,
            serviceDate: $scope.AddNotePageModel.Note.ServiceDate,
            serviceCodeTypeID: $scope.AddNotePageModel.Note.ServiceCodeType
        });
        AngularAjaxCall($http, SiteUrl.GetServiceCodesURL, jsonData, "post", "json", "application/json", true).
            success(function (response) {
                if (response.IsSuccess) {
                    $scope.AddNotePageModel.ServiceCodes = response.Data;
                    //if ($scope.AddNotePageModel.Note.ServiceCodeID > 0) {
                    //    $("#ServiceCodeSearchToken").tokenInput("clear");
                    //    $("#ServiceCodeSearchToken").tokenInput("add", { ServiceCodeID: $scope.AddNotePageModel.Note.ServiceCodeID, ServiceCode: $scope.AddNotePageModel.Note.ServiceCode });
                    //}
                }
            });
    };

    $scope.GetPosCodes = function (value) {
        //
        if ($scope.AllowToGetServiceCode) {
            var jsonData = angular.toJson({
                encReferralID: $scope.EncryptedReferralID,
                serviceDate: $scope.AddNotePageModel.Note.ServiceDate,
                serviceCodeID: value,
                noteID: $scope.AddNotePageModel.Note.NoteID > 0 ? $scope.AddNotePageModel.Note.NoteID : 0,
                payorID: $scope.AddNotePageModel.Note.PayorID
            });
            AngularAjaxCall($http, SiteUrl.GetPosCodesURL, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        $scope.AddNotePageModel.PosCodes = response.Data;
                        $scope.AddNotePageModel.Note.PosID = $scope.AddNotePageModel.Note.PosID ? $scope.AddNotePageModel.Note.PosID.toString() : "";
                        if (!$scope.$root.$$phase)
                            $scope.$apply();

                        if ($scope.AddNotePageModel.Note.PosID > 0 && $scope.AddNotePageModel.Note.ServiceCodeID > 0) {
                            angular.forEach($scope.AddNotePageModel.PosCodes, function (val, key) {
                                if (val.PosID == $scope.AddNotePageModel.Note.PosID) {
                                    $scope.SelectedServiceCodeForPayor = val;
                                    $scope.AddNotePageModel.Note.PayorServiceCodeMappingID = $scope.SelectedServiceCodeForPayor.PayorServiceCodeMappingID;
                                    $scope.WatchFuncForMileUnitCalculation();
                                    $scope.WatchFuncForTimeUnitCalculation();
                                    return;
                                }
                            });
                        }

                    }
                    $scope.AllowToGetServiceCode = true;
                });
        }
    };
    //#endregion

    //#region Common FunctionsRemoveReferral
    $scope.GetTimeDifferenceInMinutes = function (date1, date2) {
        if (date1 != undefined && date2 != undefined) {
            var difference = new Date(moment(date2, "hh:mm a")).getTime() - new Date(moment(date1, "hh:mm a")).getTime(); // This will give difference in milliseconds
            return Math.round(difference / 60000);
        }
        return null;
    };

    $scope.CheckValidTime = function (str) {
        var validTime = str.match(/^(0?[0-9]|1[012])(:[0-5]\d) [APap][mM]$/);
        return validTime;
    };

    $scope.DatePickerDate = function (modelDate, newDate) {
        var a;
        if (modelDate) {
            var dt = new Date(modelDate);
            dt >= newDate ? a = newDate : a = dt;
        } else {
            a = newDate;
        }
        return moment(a).format('L');
    };

    $scope.ExpandCollapse = function () {
        $.each($('.collapseDest'), function (index, data) {
            $(this).on('shown.bs.collapse', function () {
                $(this).parent().find(".collapseSrc").removeClass("fa-plus-circle").addClass("fa-minus-circle");
            });

            $(this).on('hidden.bs.collapse', function () {
                $(this).parent().find(".collapseSrc").removeClass("fa-minus-circle").addClass("fa-plus-circle");
            });

        });
    };

    //#endregion

    //#region On Referral Select Load
    $scope.SetProvidersBasedOnPayor = function () {
        //Removing as we have set default Provider at Payor Level

        //var opPayor = $scope.AddNotePageModel.ClientInfo.PayorID == window.Payor_MMIC || $scope.AddNotePageModel.ClientInfo.PayorID == window.Payor_UHC;
        //if (opPayor) {
        //    angular.forEach($scope.AddNotePageModel.Facilities, function(value, key) {
        //        if (value.Name == window.FacilityForOtherServiceCode) {
        //            $scope.AddNotePageModel.Note.BillingProviderID = value.Value;
        //            //$scope.AddNotePageModel.Note.RenderingProviderID = value.Value;
        //            return;
        //        }
        //    });
        //}
    };
    //#endregion



    $scope.NoteSentenceSelectionClick = function (data) {

        if ($scope.AddNotePageModel.Note.NoteDetails) {
            $scope.AddNotePageModel.Note.NoteDetails = $scope.AddNotePageModel.Note.NoteDetails + " " + data;
        } else {
            $scope.AddNotePageModel.Note.NoteDetails = data;
        }
    };

    //#endregion

    //#endregion

    $scope.ShowCollpase = function (lvl2) {
        setTimeout(function () {
            $.each($('.collapseDestination'), function (index, data) {
                var parent = data;
                $(parent).on('show.bs.collapse', function (e) {
                    if ($(this).is(e.target)) {
                        $(this).css('display', '');
                        $(this).parent("tbody").find(".collapseSource").removeClass("fa-plus-circle").addClass("fa-minus-circle");
                    }
                });

                $(parent).on('hidden.bs.collapse', function (e) {
                    if ($(this).is(e.target)) {
                        $(this).css('display', '');
                        $(this).parent("tbody").find(".collapseSource").removeClass("fa-minus-circle").addClass("fa-plus-circle");
                    }
                });

            });

        }, 100);
    };


};

controllers.NoteController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    $('.time').inputmask({
        mask: "h:s t\\m",
        placeholder: "hh:mm a",
        alias: "datetime",
        hourFormat: "12"
    });


    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});



