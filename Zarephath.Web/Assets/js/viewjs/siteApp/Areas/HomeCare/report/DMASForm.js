var vm;


controllers.DMASFormsController = function ($scope, $http, $timeout) {
    vm = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetEmployeeVisitListPage").val());
    };
    $scope.EmployeeVisitList = [];
    $scope.SelectedEmployeeVisitIds = [];
    $scope.SelectedEncryptedVisitIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.EmployeeVisitModel = $.parseJSON($("#hdnSetEmployeeVisitListPage").val());

    $scope.SearchEmployeeVisitListPage = $scope.EmployeeVisitModel.SearchEmployeeVisitListPage;
    $scope.TempSearchEmployeeVisitListPage = $scope.EmployeeVisitModel.SearchEmployeeVisitListPage;
    $scope.SearchRefCalender = $scope.newInstance().SearchRefCalender;
    $scope.EmployeeVisitListPager = new PagerModule("ReferralID");
    $scope.CareTypeID = null;
    if ($scope.SearchRefCalender.EmployeeID)
        $scope.TempSearchEmployeeVisitListPage.EmployeeIDs = $scope.SearchRefCalender.EmployeeID.toString();
    else
        $scope.SearchRefCalender.EmployeeID = "";

    if ($scope.SearchRefCalender.ReferralID)
        $scope.TempSearchEmployeeVisitListPage.ReferralIDs = $scope.SearchRefCalender.ReferralID.toString();
    else
        $scope.SearchRefCalender.ReferralID = "";

    $scope.SetPostData = function (fromIndex) {

        var pagermodel = {
            SearchEmployeeVisitListPage: $scope.SearchEmployeeVisitListPage,
            pageSize: $scope.EmployeeVisitListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.EmployeeVisitListPager.sortIndex,
            sortDirection: $scope.EmployeeVisitListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        if ($scope.TempSearchEmployeeVisitListPage.EmployeeIDs) {
            $scope.TempSearchEmployeeVisitListPage.EmployeeIDs = $scope.TempSearchEmployeeVisitListPage.EmployeeIDs.toString();
        }
        if ($scope.TempSearchEmployeeVisitListPage.ReferralIDs) {
            $scope.TempSearchEmployeeVisitListPage.ReferralIDs = $scope.TempSearchEmployeeVisitListPage.ReferralIDs.toString();
        }
        $scope.SearchEmployeeVisitListPage = $.parseJSON(angular.toJson($scope.TempSearchEmployeeVisitListPage));
    };

    $scope.GetEmployeeVisitList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedEmployeeVisitIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchEmployeeVisitListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.EmployeeVisitListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetDMASForm_90FormListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.EmployeeVisitList = response.Data.Items;
                angular.forEach($scope.EmployeeVisitList, function (data) {
                    data.IsEditable = false;
                    data.StrStartTimeEdit = null;
                    data.StrEndTimeEdit = null;
                });
                $scope.EmployeeVisitListPager.currentPageSize = response.Data.Items.length;
                $scope.EmployeeVisitListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    function toTime(timeString) {
        var timeTokens = timeString.split(':');
        return new Date(1970, 0, 1, timeTokens[0], timeTokens[1], timeTokens[2]);
    }

    $scope.Refresh = function () {
        //$scope.ResetSearchFilter();
        //$scope.CaseManagerListPager.currentPage = $scope.CaseManagerListPager.currentPage;
        $scope.EmployeeVisitListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');
            var ReferralID = $scope.TempSearchEmployeeVisitListPage.ReferralIDs;
            $scope.SearchEmployeeVisitListPage = $scope.newInstance().SearchEmployeeVisitListPage;
            $scope.TempSearchEmployeeVisitListPage = $scope.newInstance().SearchEmployeeVisitListPage;
            $scope.TempSearchEmployeeVisitListPage.IsDeleted = "0";
            $scope.TempSearchEmployeeVisitListPage.ActionTaken = "0";

            if ($scope.EmployeeVisitModel.IsPartial) {
                $scope.SearchEmployeeVisitListPage.ReferralIDs = ReferralID;
                $scope.TempSearchEmployeeVisitListPage.ReferralIDs = ReferralID;
            } else {
                $scope.TempSearchEmployeeVisitListPage.ReferralIDs = null;
            }

            $scope.EmployeeVisitListPager.currentPage = 1;
            $scope.TempSearchEmployeeVisitListPage.ServiceTypeID = null;
            $scope.EmployeeVisitListPager.getDataCallback();
            $scope.ReferralName = null;
            $scope.EmployeeID = null;
            $scope.CareTypeID = null;
        });
    };
    $scope.SearchEmployeeVisit = function () {
        $scope.EmployeeVisitListPager.currentPage = 1;
        $scope.EmployeeVisitListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectEmployeeVisit = function (EmployeeVisit) {
        if (EmployeeVisit.IsPCACompleted && EmployeeVisit.IsChecked)
            $scope.SelectedEncryptedVisitIds.push(EmployeeVisit.EncryptedEmployeeVisitID);
        else
            $scope.SelectedEncryptedVisitIds.remove(EmployeeVisit.EncryptedEmployeeVisitID);

        if (EmployeeVisit.IsChecked)
            $scope.SelectedEmployeeVisitIds.push(EmployeeVisit.EmployeeVisitID);
        else
            $scope.SelectedEmployeeVisitIds.remove(EmployeeVisit.EmployeeVisitID);

        if ($scope.SelectedEmployeeVisitIds.length == $scope.EmployeeVisitListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedEmployeeVisitIds = [];

        angular.forEach($scope.EmployeeVisitList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedEmployeeVisitIds.push(item.EmployeeVisitID);
        });
        return true;
    };

    $scope.GetEmployeeByReferralID = function (ReferralID) {
        var ids = ReferralID.split(',');
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeByReferralIDURL + '?ReferralID=' + ids[2], null, "Get", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.EmployeeVisitModel.SelectedEmployeeList = response.Data;
            }
        });
    };

    $scope.MultiplePDFDownload = function () {
        var jsonData = angular.toJson($scope.SelectedEncryptedVisitIds);
        AngularAjaxCall($http, HomeCareSiteUrl.GenerateMultiplePcaTimeSheetURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                window.location = '/hc/report/download' + '?fpath=' + response.Data.FilePath + '&fname=' + response.Data.FileName + '&d=true';
            }
            ShowMessages(response);
        });
    };

    $scope.EditEmployeeVisit = function (data, id) {

        $scope.TempClockInTime = data.ClockInTime;
        $scope.TempClockOutTime = data.ClockOutTime;
        data.ClockInTime = data.ClockInTime != null ? moment(data.ClockInTime, ['HH:mm:sss']).format('hh:mm a') : null;
        data.ClockOutTime = data.ClockOutTime != null ? moment(data.ClockOutTime, ['HH:mm:sss']).format('hh:mm a') : null;
        data.StrStartTimeEdit = data.ClockInTime;
        data.StrEndTimeEdit = data.ClockOutTime;
        data.IsEditable = true;

        if (id) {
            $timeout(function () {
                $(id).focus();
            }, 100);
        }

    }

    //$scope.EmployeeVisitModel = {};
    $scope.SaveEmployeeVisit = function (item, id) {
        if (id) {
            $(id).focusout();
            $timeout(function () {

                if (!ValideElement(item.StrStartTimeEdit) || !ValideElement(item.StrEndTimeEdit)) {
                    toastr.error(window.StrtEndTimeRequired);
                    return false;
                }

                if (id && $(id).hasClass('input-validation-error')) {
                    return false;
                }

                $scope.EmployeeVisitModel.StrStartTime = item.StrStartTimeEdit;
                $scope.EmployeeVisitModel.StrEndTime = item.StrEndTimeEdit;
                $scope.EmployeeVisitModel.EmployeeVisitID = item.EmployeeVisitID;

                var jsonData = angular.toJson($scope.EmployeeVisitModel);

                AngularAjaxCall($http, HomeCareSiteUrl.SaveEmployeeVisit, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        item.ClockInTime = response.Data.ClockInTime;
                        item.ClockOutTime = response.Data.ClockOutTime;
                        item.IsEditable = false;
                    }
                    ShowMessages(response);
                });
            }, 100);
        }


    }
    $scope.Cancel = function (data) {
        data.ClockInTime = $scope.TempClockInTime;
        data.ClockOutTime = $scope.TempClockOutTime;
        data.IsEditable = false;
    }

    $scope.DeleteEmployeeVisit = function (EmployeeVisitId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchEmployeeVisitListPage.ListOfIdsInCsv = EmployeeVisitId > 0 ? EmployeeVisitId.toString() : $scope.SelectedEmployeeVisitIds.toString();

                if (EmployeeVisitId > 0) {
                    if ($scope.EmployeeVisitListPager.currentPage != 1)
                        $scope.EmployeeVisitListPager.currentPage = $scope.EmployeeVisitList.length === 1 ? $scope.EmployeeVisitListPager.currentPage - 1 : $scope.EmployeeVisitListPager.currentPage;
                } else {

                    if ($scope.EmployeeVisitListPager.currentPage != 1 && $scope.SelectedEmployeeVisitIds.length == $scope.EmployeeVisitListPager.currentPageSize)
                        $scope.EmployeeVisitListPager.currentPage = $scope.EmployeeVisitListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedEmployeeVisitIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.EmployeeVisitListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteEmployeeVisit, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.EmployeeVisitList = response.Data.Items;
                        $scope.EmployeeVisitListPager.currentPageSize = response.Data.Items.length;
                        $scope.EmployeeVisitListPager.totalRecords = response.Data.TotalItems;
                    }
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.DatePickerDate = function (modelDate, newDate) {
        var a;
        if (modelDate) {
            var dt = new Date(modelDate);
            dt >= newDate ? a = newDate : a = dt;
        }
        else {
            a = newDate;
        }
        return moment(a).format('L');
    };
    $scope.EmployeeVisitListPager.getDataCallback = $scope.GetEmployeeVisitList;
    $scope.EmployeeVisitListPager.getDataCallback();

    $scope.OpenVisitNoteListModal = function ($event, item) {
        $('#visitNoteListModal').modal('show');
        $scope.SearchEmployeeVisitNoteListPage.EmployeeVisitID = item.EmployeeVisitID;
        $scope.TempSearchEmployeeVisitNoteListPage.EmployeeVisitID = item.EmployeeVisitID;
        $scope.EmployeeName = item.Name;
        $scope.PatientName = item.PatientName;
        $scope.SurveyCompleted = item.SurveyCompleted;
        $scope.GetEmployeeVisitNoteList();
    }

    $scope.GenerateBillingNote = function () {
        //bootboxDialog(function (result) {
        //    if (result) {
        var jsonData = angular.toJson({ EmployeeVisitID: $scope.TempSearchEmployeeVisitNoteListPage.EmployeeVisitID });
        AngularAjaxCall($http, HomeCareSiteUrl.GenerateBillingNote, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                ShowMessage(response.Message);
            }
        });
        //    }
        //}, bootboxDialogType.Confirm, window.GenerateBillingNote, window.ConfirmationForBillingNote, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    }

    //-----------------------------Employee Visit Note List---------------------------------------
    $scope.EmployeeVisitNoteList = [];
    $scope.SelectedEmployeeVisitNoteIds = [];
    $scope.SelectAllVisitNoteCheckbox = false;
    $scope.EmployeeVisitNoteModel = $.parseJSON($("#hdnSetEmployeeVisitListPage").val());
    $scope.SearchEmployeeVisitNoteListPage = $scope.EmployeeVisitNoteModel.SearchEmployeeVisitNoteListPage;
    $scope.TempSearchEmployeeVisitNoteListPage = $scope.EmployeeVisitNoteModel.SearchEmployeeVisitNoteListPage;

    $scope.EmployeeVisitNoteListPager = new PagerModule("EmployeeVisitNoteID");

    $scope.SetPostVisitNoteData = function (fromIndex) {

        var pagermodel = {
            SearchEmployeeVisitNoteListPage: $scope.SearchEmployeeVisitNoteListPage,
            pageSize: $scope.EmployeeVisitNoteListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.EmployeeVisitNoteListPager.sortIndex,
            sortDirection: $scope.EmployeeVisitNoteListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelVisitNoteMapping = function () {
        $scope.SearchEmployeeVisitNoteListPage = $.parseJSON(angular.toJson($scope.TempSearchEmployeeVisitNoteListPage));
    };

    $scope.GetEmployeeVisitNoteList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedEmployeeVisitNoteIds = [];
        $scope.SelectAllVisitNoteCheckbox = false;
        $scope.SearchEmployeeVisitNoteListPage.ListOfIdsInCSV = [];

        //Reset Selcted Checkbox items and Control
        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelVisitNoteMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostVisitNoteData($scope.EmployeeVisitNoteListPager.currentPage);

        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeVisitNoteList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.EmployeeVisitNoteList = response.Data.Items;
                angular.forEach($scope.EmployeeVisitNoteList, function (item) {
                    item.TimeInMinutes = item.ServiceTime;
                    //var d = moment.duration(item.ServiceTime, "minutes");
                    //item.ServiceTime = moment().startOf('day').add(d).format('HH:mm');
                });
                $scope.EmployeeVisitNoteListPager.currentPageSize = response.Data.Items.length;
                $scope.EmployeeVisitNoteListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.RefreshVisitNote = function () {
        //$scope.ResetSearchFilter();
        //$scope.CaseManagerListPager.currentPage = $scope.CaseManagerListPager.currentPage;
        $scope.EmployeeVisitNoteListPager.getDataCallback();
    };

    $scope.ResetVisitNoteSearchFilter = function () {
        var EmployeeVisitID = $scope.SearchEmployeeVisitNoteListPage.EmployeeVisitID;
        $timeout(function () {
            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');
            $scope.SearchEmployeeVisitNoteListPage = $scope.newInstance().SearchEmployeeVisitNoteListPage;
            $scope.TempSearchEmployeeVisitNoteListPage = $scope.newInstance().SearchEmployeeVisitNoteListPage;
            $scope.SearchEmployeeVisitNoteListPage.EmployeeVisitID = EmployeeVisitID;
            $scope.TempSearchEmployeeVisitNoteListPage.EmployeeVisitID = EmployeeVisitID;
            $scope.TempSearchEmployeeVisitNoteListPage.IsDeleted = "0";
            $scope.EmployeeVisitNoteListPager.currentPage = 1;
            $scope.EmployeeVisitNoteListPager.getDataCallback();
        });
    };
    $scope.SearchEmployeeVisitNote = function () {
        $scope.EmployeeVisitNoteListPager.currentPage = 1;
        $scope.EmployeeVisitNoteListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectEmployeeVisitNote = function (EmployeeVisitNote) {
        if (EmployeeVisitNote.IsChecked)
            $scope.SelectedEmployeeVisitNoteIds.push(EmployeeVisitNote.EmployeeVisitNoteID);
        else
            $scope.SelectedEmployeeVisitNoteIds.remove(EmployeeVisitNote.EmployeeVisitNoteID);
        if ($scope.SelectedEmployeeVisitNoteIds.length == $scope.EmployeeVisitNoteListPager.currentPageSize)
            $scope.SelectAllVisitNoteCheckbox = true;
        else
            $scope.SelectAllVisitNoteCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAllVisitNote = function () {
        $scope.SelectedEmployeeVisitNoteIds = [];
        angular.forEach($scope.EmployeeVisitNoteList, function (item, key) {
            item.IsChecked = $scope.SelectAllVisitNoteCheckbox;
            if (item.IsChecked)
                $scope.SelectedEmployeeVisitNoteIds.push(item.EmployeeVisitNoteID);
        });
        return true;
    };

    $scope.AddVisitNote = function (data) {
        if (data != undefined) {
            $scope.VisitNote.EmployeeVisitNoteID = data.EmployeeVisitNoteID;
            $scope.VisitNote.ReferralTaskMappingID = data.ReferralTaskMappingID;
            $scope.VisitNote.Hours = Math.floor(data.TimeInMinutes / 60);
            $scope.VisitNote.Minutes = (data.TimeInMinutes % 60);
        }
        var jsonData = angular.toJson({ EmployeeVisitID: $scope.SearchEmployeeVisitNoteListPage.EmployeeVisitID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetMappedVisitTask, jsonData, "Post", "json", "application/json", false).success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.MappedTaskList = response.Data.VisitTaskList;
                $scope.HourList = response.Data.HourList;
                $scope.MinuteList = response.Data.MinuteList;
            }
        });
        $('#AddVisitNoteModal').modal('show');
    }

    $scope.VisitNote = {};
    $scope.SaveVisitNote = function (data) {
        var isValid = CheckErrors($("#addVisitNotefrm"));
        if (isValid) {
            $scope.VisitNote.EmployeeVisitID = $scope.SearchEmployeeVisitNoteListPage.EmployeeVisitID;
            //$scope.VisitNote.ReferralTaskMappingID = data.ReferralTaskMappingID;
            //$scope.VisitNote.Hours = data.Hours;
            //$scope.VisitNote.Minutes = data.Minutes;
            var jsonData = angular.toJson($scope.VisitNote);
            AngularAjaxCall($http, HomeCareSiteUrl.SaveVisitNote, jsonData, "Post", "json", "application/json", false).success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.GetEmployeeVisitNoteList();
                    $scope.VisitNote = {};
                    $('#AddVisitNoteModal').modal('hide');
                }
            });
        }
    }

    $('#AddVisitNoteModal').on('hidden.bs.modal', function () {
        $scope.VisitNote = {};
        HideErrors("#addVisitNotefrm");
    });

    $scope.DeleteEmployeeVisitNote = function (EmployeeVisitNoteID, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchEmployeeVisitNoteListPage.ListOfIdsInCsv = EmployeeVisitNoteID > 0 ? EmployeeVisitNoteID.toString() : $scope.SelectedEmployeeVisitNoteIds.toString();

                if (EmployeeVisitNoteID > 0) {
                    if ($scope.EmployeeVisitNoteListPager.currentPage != 1)
                        $scope.EmployeeVisitNoteListPager.currentPage = $scope.EmployeeVisitNoteList.length === 1 ? $scope.EmployeeVisitNoteListPager.currentPage - 1 : $scope.EmployeeVisitNoteListPager.currentPage;
                } else {

                    if ($scope.EmployeeVisitNoteListPager.currentPage != 1 && $scope.SelectedEmployeeVisitNoteIds.length == $scope.EmployeeVisitNoteListPager.currentPageSize)
                        $scope.EmployeeVisitNoteListPager.currentPage = $scope.EmployeeVisitNoteListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedEmployeeVisitNoteIds = [];
                $scope.SelectAllVisitNoteCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostVisitNoteData($scope.EmployeeVisitNoteListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteEmployeeVisitNote, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.EmployeeVisitNoteList = response.Data.EmployeeVisitNoteList.Items;
                        $scope.EmployeeVisitNoteListPager.currentPageSize = response.Data.EmployeeVisitNoteList.Items.length;
                        $scope.EmployeeVisitNoteListPager.totalRecords = response.Data.EmployeeVisitNoteList.TotalItems;

                    }
                    if (response.Data.Result == -1) {
                        ShowMessage("Invoice generated and thier status is in paid or void so we can't delete this task", "error");
                    }
                    else {
                        ShowMessages(response);
                    }
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableVisitNoteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };
    $scope.EmployeeVisitNoteListPager.getDataCallback = $scope.GetEmployeeVisitNoteList;
    $scope.EmployeeVisitNoteListPager.getDataCallback();

    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var target = $(e.target).attr("href"); // activated tab
        if (target == "#conclusion") {
            var jsonData = angular.toJson({ EmployeeVisitID: $scope.SearchEmployeeVisitNoteListPage.EmployeeVisitID });
            AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeVisitConclusionList, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.EmployeeVisitConclusionList = response.Data;
                }
            });
        }
    });

    $scope.AddVisitConclusion = function (data) {
        if (data != undefined) {
            $scope.VisitConclusion.EmployeeVisitNoteID = data.EmployeeVisitNoteID;
            $scope.VisitConclusion.ReferralTaskMappingID = data.ReferralTaskMappingID;
            $scope.VisitConclusion.Answer = data.Description;
        }
        var jsonData = angular.toJson({ EmployeeVisitID: $scope.SearchEmployeeVisitNoteListPage.EmployeeVisitID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetMappedVisitConclusion, jsonData, "Post", "json", "application/json", false).success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.MappedConclusionList = response.Data.VisitTaskList;
                $scope.AnswerList = response.Data.YesNoList;
            }
        });
        $('#AddVisitConclusionModal').modal('show');
    };

    $scope.VisitConclusion = {};
    $scope.SaveVisitConclusion = function (data) {
        var isValid = CheckErrors($("#addVisitConclusionfrm"));
        if (isValid) {
            $scope.VisitConclusion.EmployeeVisitID = $scope.SearchEmployeeVisitNoteListPage.EmployeeVisitID;
            var jsonData = angular.toJson($scope.VisitConclusion);
            AngularAjaxCall($http, HomeCareSiteUrl.SaveVisitConclusion, jsonData, "Post", "json", "application/json", false).success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    var jsonData = angular.toJson({ EmployeeVisitID: $scope.SearchEmployeeVisitNoteListPage.EmployeeVisitID });
                    AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeVisitConclusionList, jsonData, "Post", "json", "application/json", false).success(function (response) {
                        ShowMessages(response);
                        if (response.IsSuccess) {
                            $scope.EmployeeVisitConclusionList = response.Data;
                        }
                    });
                    $scope.VisitConclusion = {};
                    $('#AddVisitConclusionModal').modal('hide');
                }
            });
        }
    };

    $('#AddVisitConclusionModal').on('hidden.bs.modal', function () {
        $scope.VisitConclusion = {};
        HideErrors("#addVisitConclusionfrm");
    });

    $scope.ConclusionAnswer = {};
    $scope.ChangeAnswer = function (data) {
        $scope.ConclusionAnswer.EmployeeVisitNoteID = data.EmployeeVisitNoteID;
        $scope.ConclusionAnswer.Description = data.Desc;
        var jsonData = angular.toJson($scope.ConclusionAnswer);
        AngularAjaxCall($http, HomeCareSiteUrl.ChangeConclusionAnswer, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                ShowMessages(response);
            }
        });
    };

    $('#visitNoteListModal').on('hidden.bs.modal', function () {
        $scope.EmployeeVisitNoteList = {};
        $('#reportTabs a:first').tab('show');
    });

    $scope.BypassDetail = {};
    $scope.OpenBypassActionModal = function ($event, item) {
        $('#ByPassActionModal').modal('show');
        $scope.BypassDetail.IsByPassClockIn = item.IsByPassClockIn;
        $scope.BypassDetail.IsByPassClockOut = item.IsByPassClockOut;
        $scope.BypassDetail.ByPassReasonClockIn = item.ByPassReasonClockIn;
        $scope.BypassDetail.ByPassReasonClockOut = item.ByPassReasonClockOut;
        $scope.BypassDetail.EmployeeVisitID = item.EmployeeVisitID;
        $scope.BypassDetail.IsApprove = false;
        $scope.BypassDetail.RejectReason = item.RejectReason;
        $scope.BypassDetail.IsApprovalRequired = item.IsApprovalRequired;
        $scope.BypassDetail.IVRClockIn = item.IVRClockIn;
        $scope.BypassDetail.IVRClockOut = item.IVRClockOut;
        $scope.BypassDetail.ActionTaken = item.ActionTaken;
    };

    $scope.OpenBypassRejectReasonModal = function (modal) {
        $('#ByPassRejectReasonModal').modal('show');
        $scope.BypassDetail = modal;
    };

    $scope.ActionTaken = function (modal, IsApprove) {
        var isValid = IsApprove ? true : CheckErrors($("#addRejectReasonfrm"));
        if (isValid) {
            modal.IsApprove = IsApprove;
            var jsonData = angular.toJson(modal);
            AngularAjaxCall($http, HomeCareSiteUrl.BypassActionTaken, jsonData, "Post", "json", "application/json", false).success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    if (IsApprove) {
                        $scope.BypassDetail = {};
                        $('#ByPassActionModal').modal('hide');
                    } else {
                        $('#ByPassRejectReasonModal').modal('hide');
                        $scope.BypassDetail.ActionTaken = 3//Reject Action;
                    }
                    $scope.GetEmployeeVisitList();
                }
            });
        }
    };

    $('#ByPassRejectReasonModal').on('hidden.bs.modal', function () {
        HideErrors("#addRejectReasonfrm");
    });

    $('#ByPassActionModal').on('hidden.bs.modal', function () {
        $scope.BypassDetail = {};
    });


    $scope.PrintPCATimeSheet = function (employeeVisitNoteId) {
        var jsonData = angular.toJson({ id: employeeVisitNoteId });
        AngularAjaxCall($http, HomeCareSiteUrl.GeneratePcaTimeSheetPdfURL, jsonData, "Post", "json", "application/json").success(function (response) {
        });
    };


    //DMAS_90_FORM
    $scope.SearchDMAS90Report = function () {
        var jsonData = angular.toJson({
            StartDate: $scope.TempSearchEmployeeVisitListPage.StartDate,
            EndDate: $scope.TempSearchEmployeeVisitListPage.EndDate,
            CareTypeID: $scope.CareTypeID === "" || $scope.CareTypeID === undefined ? null : $scope.CareTypeID,
            ReferralID: $scope.ReferralID,
            ReferralName: $scope.ReferralName,
            ServiceTypeID: $scope.TempSearchEmployeeVisitListPage.ServiceTypeID,
            EmployeeID: $scope.EmployeeID
        });
        AngularAjaxCall($http, HomeCareSiteUrl.GetENewDMAS90ListURLs, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.DMAS_90FormsList = response.Data;
                console.log($scope.DMAS_90FormsList);
            }
        });
    };
    $scope.AdditionNotes = function () {
        $scope.AdditionalNotes = $scope.AdditionalNotes;
        $scope.AdditionalNotes1 = $scope.AdditionalNotes1;
    }
    $scope.CaretypeList = [];
    $scope.GetCaretype = function () {
        var jsonData = angular.toJson({});
        AngularAjaxCall($http, HomeCareSiteUrl.GetCaretypeURL, jsonData, "Get", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.CaretypeList = response.Data;
            }
        });
    };
    $scope.GetCaretype();
    $scope.option = {

    };
    $scope.CloseModel = function (option) {

        $('#myModal').modal('show');
        $scope.ReferralID = option.ReferralID;
        $scope.startDate = option.startDate;
        $scope.EndDate = option.EndDate;
        $scope.CareTypeID = $scope.CareTypeID;
        $scope.AdditionalNote = $scope.AdditionalNotes;
        $scope.AdditionalNote1 = $scope.AdditionalNotes1;


        //$scope.url = "/hc/report/GenerateDMAS_90FormsPdfURL?StartDate=" + $scope.startDate + "&EndDate=" + $scope.EndDate + "&CareTypeID=" + $scope.CareTypeID +"&ReferralID=" + option.ReferralID+"&AdditionalNote="+$scope.AdditionalNotes+"&AdditionalNote1="+$scope.AdditionalNotes1;
        //window.location.href = $scope.url;
        //$('#myModal').modal('hide');
    };
    $scope.DMAS_Print = function () {
        $scope.url = "/hc/report/GenerateDMAS_90FormsPdfURL?StartDate=" + $scope.startDate + "&EndDate=" + $scope.EndDate + "&CareType=" + $scope.CareTypeID + "&ReferralID=" + $scope.ReferralID + "&AdditionalNote=" + $scope.AdditionalNotes + "&AdditionalNote1=" + $scope.AdditionalNotes1;
        window.location.href = $scope.url;
        $('#myModal').modal('hide');
    };
    $scope.WeeklyTimeSheet_Print = function () {
        $scope.url = "/hc/report/GenerateWeeklyTimeSheetPdfURL?StartDate=" + $scope.startDate + "&EndDate=" + $scope.EndDate + "&CareType=" + $scope.CareTypeID + "&ReferralID=" + $scope.ReferralID + "&AdditionalNote=" + $scope.AdditionalNotes + "&AdditionalNote1=" + $scope.AdditionalNotes1;
        window.location.href = $scope.url;
        $('#myModal').modal('hide');
    };
};

controllers.DMASFormsController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    var dateformat = GetOrgDateFormat();
    $(".dateInputMask").attr("placeholder", dateformat);
    $('.time').inputmask({
        mask: "h:s t\\m",
        placeholder: "hh:mm a",
        alias: "datetime",
        hourFormat: "12"
    });
});