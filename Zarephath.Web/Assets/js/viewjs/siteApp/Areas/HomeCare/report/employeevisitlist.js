var vm;


controllers.EmployeeVisitListController = function ($scope, $http, $timeout) {
    vm = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetEmployeeVisitListPage").val());
    };
    $scope.ARPcount = 0;
    $scope.ARAcount = 0;
    $scope.ARRcount = 0;
    $scope.InvalidVisitCount = 0;
    $scope.DeniedCount = 0;
    $scope.PaidCount = 0;
    $scope.BilledCount = 0;
    $scope.NotBilledCount = 0;
    $scope.PRcategory = false;
    $scope.IsPCACompletedcount = 0;
    $scope.NotIsPCACompletedcount = 0;
    $scope.IsLoadedEmployeeVisitSummary = false;
    $scope.EmployeeVisitList = [];
    $scope.SelectedEmployeeVisitIds = [];
    $scope.SelectedEncryptedVisitIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.EmployeeVisitModel = $.parseJSON($("#hdnSetEmployeeVisitListPage").val());
    $scope.PCADetail = $scope.EmployeeVisitModel.PCADetail;
    $scope.SearchEmployeeVisitListPage = $scope.EmployeeVisitModel.SearchEmployeeVisitListPage;
    $scope.TempSearchEmployeeVisitListPage = $scope.EmployeeVisitModel.SearchEmployeeVisitListPage;
    $scope.SearchRefCalender = $scope.newInstance().SearchRefCalender;
    $scope.EmployeeVisitListPager = new PagerModule("EmployeeVisitID");
    $scope.TempSearchEmployeeVisitListPage.IsAuthExpired = "-1";

    $scope.showBilling = true;
    $scope.showTimesheet = false;

    $scope.ByBilling = function () {
        $scope.showBilling = true;
        $scope.showTimesheet = false;
    }

    $scope.ByTimesheet = function () {
        $scope.showTimesheet = true;
        $scope.showBilling = false;
    }

    if ($scope.SearchRefCalender.EmployeeID)
        $scope.TempSearchEmployeeVisitListPage.EmployeeIDs = $scope.SearchRefCalender.EmployeeID.toString();
    else
        $scope.SearchRefCalender.EmployeeID = "";

    if ($scope.SearchRefCalender.ReferralID)
        $scope.TempSearchEmployeeVisitListPage.ReferralIDs = $scope.SearchRefCalender.ReferralID.toString();
    else
        $scope.SearchRefCalender.ReferralID = "";

    if ($scope.SearchRefCalender.PayorID)
        $scope.TempSearchEmployeeVisitListPage.PayorIDs = $scope.SearchRefCalender.PayorID.toString();
    else
        $scope.SearchRefCalender.PayorID = "";

    if ($scope.SearchRefCalender.CareTypeID)
        $scope.TempSearchEmployeeVisitListPage.CareTypeIDs = $scope.SearchRefCalender.CareTypeID.toString();
    else
        $scope.SearchRefCalender.CareTypeID = "";

    var todayDate = new Date().setHours(23, 59, 59, 999)
    $scope.SearchEmployeeVisitListPage.StartDate = moment(todayDate).add(-1, 'months').toDate();
    $scope.SearchEmployeeVisitListPage.EndDate = todayDate;

    $scope.SearchAuthServiceCodesModel = {};
    $scope.GetAuthServiceCodesURL = HomeCareSiteUrl.GetAuthServiceCodesURL;

    $scope.OnLeaveAuthServiceCodesModel = function () {
        //console.log('leaved');
        $scope.SearchAuthServiceCodesModel = {};
        $scope.GetAuthServiceCodesURL = HomeCareSiteUrl.GetAuthServiceCodesURL;
        $scope.SearchAuthServiceCodesModel.ReferralBillingAuthorizationID = 0;
    };


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
        if ($scope.TempSearchEmployeeVisitListPage.PayorIDs) {
            $scope.TempSearchEmployeeVisitListPage.PayorIDs = $scope.TempSearchEmployeeVisitListPage.PayorIDs.toString();
        }
        if ($scope.TempSearchEmployeeVisitListPage.CareTypeIDs) {
            $scope.TempSearchEmployeeVisitListPage.CareTypeIDs = $scope.TempSearchEmployeeVisitListPage.CareTypeIDs.toString();
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


        //execlude pending  when filter incomplete 
        if ($scope.SearchEmployeeVisitListPage.ActionTaken === "4")
            $scope.SearchEmployeeVisitListPage.ExecludeIncompletePending = true;
        else
            $scope.SearchEmployeeVisitListPage.ExecludeIncompletePending = false;

        var jsonData = $scope.SetPostData($scope.EmployeeVisitListPager.currentPage);
        $scope.EmployeeVisitListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeVisitList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.EmployeeVisitList = response.Data.Items;
                if ($scope.EmployeeVisitList.length > 0) {
                    // if ($scope.ARPcount <= 0)
                    $scope.ARPcount = $scope.EmployeeVisitList[0].ARPcount;
                    //if ($scope.ARAcount <= 0)
                    $scope.ARAcount = $scope.EmployeeVisitList[0].ARAcount;
                    //if ($scope.ARRcount <= 0)
                    $scope.ARRcount = $scope.EmployeeVisitList[0].ARRcount;
                    //if ($scope.IsPCACompletedcount <= 0)
                    $scope.DeniedCount = $scope.EmployeeVisitList[0].DeniedCount;
                    $scope.InvalidVisitCount = $scope.EmployeeVisitList[0].InvalidVisitCount;
                    $scope.NotBilledCount = $scope.EmployeeVisitList[0].NotBilledCount;
                    //if ($scope.ARAcount <= 0)
                    $scope.PaidCount = $scope.EmployeeVisitList[0].PaidCount;
                    //if ($scope.ARRcount <= 0)
                    $scope.BilledCount = $scope.EmployeeVisitList[0].BilledCount;
                    $scope.IsPCACompletedcount = $scope.EmployeeVisitList[0].IsPCACompletedcount;
                    // if ($scope.NotIsPCACompletedcount <= 0)
                    $scope.NotIsPCACompletedcount = $scope.EmployeeVisitList[0].NotIsPCACompletedcount;
                }


                angular.forEach($scope.EmployeeVisitList, function (data) {
                    data.IsEditable = false;
                    data.StrStartTimeEdit = null;
                    data.StrEndTimeEdit = null;
                    data.IsPCACompletedBit = data.IsPCACompleted;
                    if (data.IsPCACompleted) {
                        data.RowColor = '#90EE90';
                        data.IsPCACompleted = 'Completed';
                        data.RowMarkerClass = 'row-mark-class1';
                    } else {
                        data.RowColor = '#FF7373';
                        data.IsPCACompleted = '';
                        data.RowMarkerClass = 'row-mark-class2';
                    }

                    if (data.ClockInTime == null) {
                        data.RowColor = '#d3d3d3';// Grey
                        data.RowMarkerClass = 'row-mark-class3';
                    }
                    if (data.ActionTaken == 1) {
                        data.RowColor = '#F0E68C';//yellow
                        data.RowMarkerClass = 'row-mark-class4';
                    }
                    //clock In Mode
                    if (data.ClockInTime && !data.IVRClockIn) {
                        data.ClockInMode = 'Mobile';
                    }
                    if (data.IsByPassClockIn) {
                        data.ClockInMode = 'Mobile';
                    }
                    if (data.IVRClockIn) {
                        data.ClockInMode = 'IVR';
                    }

                    // Clock Out Mode
                    if (data.ClockOutTime && !data.IVRClockOut) {
                        data.ClockOutMode = "Mobile";
                    }
                    if (!data.ClockOutTime && data.IVRClockOut) {
                        data.ClockOutMode = "IVR";
                    }
                    if (data.IsByPassClockOut) {
                        data.ClockOutMode = 'Mobile';
                    }
                    if (data.ClockOutTime && data.IsByPassClockOut) {
                        data.ClockOutMode = 'Mobile';
                    }
                    //if (data.ClockOutTime && data.IVRClockOut) {
                    //    data.ClockOutMode = "IVR";
                    //}     

                    //added by kunal: #2njec1 : Add Employee Visit more information Tip on the report.
                    if (data.TotalcreatedTask > 0 &&
                        data.SurveyCompleted == true &&
                        data.IsPCACompletedBit == true) {

                        data.AnyActionMissing = 1;
                    }
                    else {
                        data.AnyActionMissing = 0;
                    }
                });
                $scope.EmployeeVisitListPager.currentPageSize = response.Data.Items.length;
                $scope.EmployeeVisitListPager.totalRecords = response.Data.TotalItems;
                //angular.forEach($scope.EmployeeVisitList, function (item) {
                //    item.ClockInTime = (item.ClockInTime != null) ? moment.utc(item.ClockInTime).local().format('hh:mm a') : "N/A";
                //    item.ClockOutTime=(item.ClockOutTime != null) ? moment.utc(item.ClockOutTime).local().format('hh:mm a') : "N/A";
                //});

                //  console.log($scope.EmployeeVisitList);
            }
            $scope.EmployeeVisitListAjaxStart = false;
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
            $scope.TempSearchEmployeeVisitListPage.IsAuthExpired = "-1";
            $scope.SearchEmployeeVisitListPage.IsAuthExpired = "-1";
            $scope.TempSearchEmployeeVisitListPage.ActionTaken = "0";
            var Payorid = $scope.TempSearchEmployeeVisitListPage.PayorIDs;
            var CareTypeId = $scope.TempSearchEmployeeVisitListPage.CareTypeIDs;
            if ($scope.EmployeeVisitModel.IsPartial) {
                $scope.SearchEmployeeVisitListPage.ReferralIDs = ReferralID;
                $scope.TempSearchEmployeeVisitListPage.ReferralIDs = ReferralID;
                $scope.SearchEmployeeVisitListPage.PayorIDs = Payorid;
                $scope.TempSearchEmployeeVisitListPage.PayorIDs = Payorid;
                $scope.SearchEmployeeVisitListPage.CareTypeIDs = CareTypeId;
                $scope.TempSearchEmployeeVisitListPage.CareTypeIDs = CareTypeId;
            } else {
                $scope.TempSearchEmployeeVisitListPage.ReferralIDs = null;
                $scope.TempSearchEmployeeVisitListPage.PayorIDs = null;
                $scope.TempSearchEmployeeVisitListPage.CareTypeIDs = null;
            }

            $scope.EmployeeVisitListPager.currentPage = 1;
            $scope.EmployeeVisitListPager.getDataCallback();
        });
    };
    $scope.SearchEmployeeVisit = function () {
        $scope.EmployeeVisitListPager.currentPage = 1;
        $scope.EmployeeVisitListPager.getDataCallback(true);
    };
    $scope.SearchServiceType = function () {
        $scope.EmployeeVisitListPager.currentPage = 1;
        $scope.EmployeeVisitListPager.getDataCallback(true);
    };
    // This executes when select single checkbox selected in table.   
    $scope.SelectEmployeeVisit = function (EmployeeVisit) {
        //if ((!EmployeeVisit.PayorName && EmployeeVisit.PayorCount > 1) || (!EmployeeVisit.PayorName && EmployeeVisit.PayorCount == 0)) {
        //    EmployeeVisit.IsChecked = false;
        //    var PayorList = $scope.EmployeeVisitModel.ReferralPayorList.filter(x => x.ReferralID == EmployeeVisit.ReferralID);
        //    if (PayorList.length > 0) {
        //        $scope.ReferralPayorList = $scope.EmployeeVisitModel.PayorList.filter(x => PayorList[0].PayorID.indexOf(x.PayorID) >= 0 ? x.PayorID : "")
        //    } else {
        //        $scope.ReferralPayorList = $scope.EmployeeVisitModel.PayorList;
        //    }

        //    $scope.ReferralEmployeeVisit = EmployeeVisit;
        //    $('#ReferralEmployeeVisitmodel').modal({
        //        backdrop: 'static',
        //        keyboard: false
        //    });
        //} else {
        if (EmployeeVisit.IsPCACompleted && EmployeeVisit.IsChecked)
            $scope.SelectedEncryptedVisitIds.push(EmployeeVisit.EncryptedEmployeeVisitID);
        else
            $scope.SelectedEncryptedVisitIds.remove(EmployeeVisit.EncryptedEmployeeVisitID);

        if (EmployeeVisit.IsChecked)
            $scope.SelectedEmployeeVisitIds.push(EmployeeVisit.EmployeeVisitID);
        else
            $scope.SelectedEmployeeVisitIds.remove(EmployeeVisit.EmployeeVisitID);
        // }
        if ($scope.SelectedEmployeeVisitIds.length == $scope.EmployeeVisitListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedEmployeeVisitIds = [];

        angular.forEach($scope.EmployeeVisitList, function (item, key) {
            if ((!item.PayorName && item.PayorCount > 1) || (!item.PayorName && item.PayorCount == 0)) {
                item.IsChecked = false;
            } else {
                item.IsChecked = $scope.SelectAllCheckbox;
            }
            if (item.IsChecked) {
                $scope.SelectedEmployeeVisitIds.push(item.EmployeeVisitID);
            }
        });
        return true;
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
        //data.ClockInTime = data.ClockInTime != null ? moment(data.ClockInTime, ['HH:mm:sss']).format('hh:mm a') : null;
        //data.ClockOutTime = data.ClockOutTime != null ? moment(data.ClockOutTime, ['HH:mm:sss']).format('hh:mm a') : null;
        //data.StrStartTimeEdit = data.ClockInTime;
        //data.StrEndTimeEdit = data.ClockOutTime;

        data.StrStartTimeEdit = data.ClockInTime != null ? moment(data.ClockInTime).format('hh:mm a') : null;
        data.StrEndTimeEdit = data.ClockOutTime != null ? moment(data.ClockOutTime).format('hh:mm a') : null;

        data.StrStartDateEdit = data.ClockInTime != null ? moment(data.ClockInTime).format('MM/DD/YYYY') : null;
        data.StrEndDateEdit = data.ClockOutTime != null ? moment(data.ClockOutTime).format('MM/DD/YYYY') : null;

        data.IsEditable = true;
        if (id) {
            $timeout(function () {
                $(id).focus();
            }, 100);
        }

    };

    $scope.EditPayorAuth = function (data) {
        data.IsEditableAuth = true;
    };

    $scope.CancelEditPayorAuth = function (data) {
        data.IsEditableAuth = false;
    };

    //$scope.EmployeeVisitModel = {};
    $scope.SaveEmployeeVisit = function (item, id) {
        if (id) {
            $(id).focusout();
            $timeout(function () {

                var starttime = item.StrStartTimeEdit;
                var starthours = Number(starttime.match(/^(\d+)/)[1]);
                var startminutes = Number(starttime.match(/:(\d+)/)[1]);
                var startAMPM = starttime.match(/\s(.*)$/)[1];
                if (startAMPM == "pm" && starthours < 12) starthours = starthours + 12;
                if (startAMPM == "am" && starthours == 12) starthours = starthours - 12;
                var startsHours = starthours.toString();
                var startsMinutes = startminutes.toString();
                if (starthours < 10) startsHours = "0" + startsHours;
                if (startminutes < 10) startsMinutes = "0" + startsHours;

                var endtime = item.StrEndTimeEdit;
                var endhours = Number(endtime.match(/^(\d+)/)[1]);
                var endminutes = Number(endtime.match(/:(\d+)/)[1]);
                var endAMPM = endtime.match(/\s(.*)$/)[1];
                if (endAMPM == "pm" && endhours < 12) endhours = endhours + 12;
                if (endAMPM == "am" && endhours == 12) endhours = endhours - 12;
                var endsHours = endhours.toString();
                var endsMinutes = endminutes.toString();
                if (endhours < 10) endsHours = "0" + endsHours;
                if (endminutes < 10) endsMinutes = "0" + endsMinutes;

                var strStart = new Date(new Date(item.StrStartDateEdit).getUTCFullYear(), new Date(item.StrStartDateEdit).getUTCMonth(), new Date(item.StrStartDateEdit).getUTCDate(), startsHours, startsMinutes, 0, 0);
                var strEnd = new Date(new Date(item.StrEndDateEdit).getUTCFullYear(), new Date(item.StrEndDateEdit).getUTCMonth(), new Date(item.StrEndDateEdit).getUTCDate(), endsHours, endsMinutes, 0, 0);
                //alert(((strEnd - strStart) / (60 *  1000)));
                //return false;
                if ((((Math.abs(strStart - strEnd)) / (60 * 60 * 1000))) > 24) {
                    toastr.error("ClockIn time and ClockOut time difference should not be greater than 24hrs !!");
                    return false;
                }

                if ((((strEnd - strStart) / (60 * 1000)) < 1)) {
                    toastr.error("ClockOut time should be greater than ClockIn time!!");
                    return false;
                }
                if (!ValideElement(item.StrStartTimeEdit) || !ValideElement(item.StrEndTimeEdit)) {
                    toastr.error(window.StrtEndTimeRequired);
                    return false;
                }

                if (id && $(id).hasClass('input-validation-error')) {
                    return false;
                }
                //if (item.StrStartTimeEdit > item.StrEndTimeEdit) {
                //    toastr.error('ClockInTime time should be less than ClockOutTime');
                //    return false;
                //}
                $scope.EmployeeVisitModel.StrStartTime = item.StrStartTimeEdit;
                $scope.EmployeeVisitModel.StrEndTime = item.StrEndTimeEdit;
                $scope.EmployeeVisitModel.ClockInDate = item.StrStartDateEdit;
                $scope.EmployeeVisitModel.ClockOutDate = item.StrEndDateEdit;
                $scope.EmployeeVisitModel.EmployeeVisitID = item.EmployeeVisitID;

                ShowVisitReasonActionModal({
                    ScheduleID: item.ScheduleID,
                    OnSet: function (data, save) {
                        var jsonData = angular.toJson($scope.EmployeeVisitModel);
                        AngularAjaxCall($http, HomeCareSiteUrl.SaveEmployeeVisit, jsonData, "Post", "json", "application/json").success(function (response) {
                            if (response.IsSuccess) {
                                save();
                                item.ClockInTime = response.Data.ClockInTime;
                                item.ClockOutTime = response.Data.ClockOutTime;
                                // $scope.EmployeeVisit.ClockInTime = new Date(response.Data.ClockInTime);
                                // $scope.EmployeeVisit.ClockOutTime = new Date(response.Data.ClockOutTime);
                                item.IsEditable = false;
                            }
                            ShowMessages(response);
                            $scope.GetEmployeeVisitList();
                        });
                    }
                });
            }, 100);
        }
    }

    $scope.Cancel = function (data) {
        data.ClockInTime = $scope.TempClockInTime;
        data.ClockOutTime = $scope.TempClockOutTime;
        data.IsEditable = false;
    }
    $scope.Cancel1 = function (data) {
        data.IsEditable = false;
    }
    $scope.NotcreatedTaksList = "";
    $('#ReferralEmployeeVisitTaskmodel').on('hidden.bs.modal', function () {
        $scope.NotcreatedTaksList = "";
    });
    $scope.MarkAsCompleteEmployeeVisit = function (EmployeeVisitId, title) {
        $scope.ListOfIdsInCsv = EmployeeVisitId;
        $scope.AllEmployeeVisitNoteList;
        $scope.NotcreatedTaksList = "";
        //for (var t = 0; t < $scope.SelectedEmployeeVisitIds.length; t++) {
        //    var Empvisitobj = $scope.EmployeeVisitList.find(x => x.EmployeeVisitID == $scope.SelectedEmployeeVisitIds[t]);
        //    if (Empvisitobj.TotalcreatedTask == 0) {
        //        if ($scope.NotcreatedTaksList) {
        //            $scope.NotcreatedTaksList += "\n" + Empvisitobj.Name + " (" + Empvisitobj.PatientName + ") ";
        //        } else { $scope.NotcreatedTaksList = Empvisitobj.Name + " (" + Empvisitobj.PatientName + ") "; }
        //    }
        //}
        if (!$scope.NotcreatedTaksList) {
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
                    AngularAjaxCall($http, HomeCareSiteUrl.MarkAsCompleteEmployeeVisit, jsonData, "Post", "json", "application/json").success(function (response) {
                        ShowMessages(response);
                        if (response.IsSuccess) {
                            if (response.Data) {
                                $scope.EmployeeVisitList = response.Data.Items;
                                $scope.EmployeeVisitListPager.currentPageSize = response.Data.Items.length;
                                $scope.EmployeeVisitListPager.totalRecords = response.Data.TotalItems;
                            }
                            $scope.ResetSearchFilter();
                        }
                    });
                }
            }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
        } else {
            $('#ReferralEmployeeVisitTaskmodel').modal({
                backdrop: 'static',
                keyboard: false
            });
        }


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
                        //  $scope.EmployeeVisitList = response.Data.Items;
                        // $scope.EmployeeVisitListPager.currentPageSize = response.Data.Items.length;
                        //$scope.EmployeeVisitListPager.totalRecords = response.Data.TotalItems;
                        $scope.EmployeeVisitListPager.getDataCallback();
                    }
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.VisitApprovalModal = {};
    $scope.VisitApprovalModal.VisitApprovalList = [];
    $scope.ApprovedEmployeeVisitIds = [];
    // This executes when select single checkbox selected in table.   
    $scope.SelectEmployeeVisitApprove = function (EmployeeVisit) {
        if (!EmployeeVisit.CanApprove) {
            EmployeeVisit.IsChecked = false;
        }

        if (EmployeeVisit.IsChecked)
            $scope.ApprovedEmployeeVisitIds.push(EmployeeVisit.EmployeeVisitID);
        else
            $scope.ApprovedEmployeeVisitIds.remove(EmployeeVisit.EmployeeVisitID);

        if ($scope.ApprovedEmployeeVisitIds.length > 0 &&
            $scope.ApprovedEmployeeVisitIds.length == $scope.VisitApprovalModal.VisitApprovalList.filter(item => item.CanApprove).length)
            $scope.SelectAllApproveCheckbox = true;
        else
            $scope.SelectAllApproveCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAllApprove = function (oldValue) {
        var newValue = !oldValue;
        $scope.ApprovedEmployeeVisitIds = [];

        angular.forEach($scope.VisitApprovalModal.VisitApprovalList, function (item, key) {
            if (!item.CanApprove) {
                item.IsChecked = false;
            } else {
                item.IsChecked = newValue;
            }
            if (item.IsChecked) {
                $scope.ApprovedEmployeeVisitIds.push(item.EmployeeVisitID);
            }
        });

        if ($scope.ApprovedEmployeeVisitIds.length > 0 &&
            $scope.ApprovedEmployeeVisitIds.length == $scope.VisitApprovalModal.VisitApprovalList.filter(item => item.CanApprove).length)
            $scope.SelectAllApproveCheckbox = true;
        else
            $scope.SelectAllApproveCheckbox = false;
        return true;
    };

    // This executes when select single checkbox selected in table.   
    $scope.SelectEmployeeVisitClockInTimeChanged = function (EmployeeVisit) {
        if (!EmployeeVisit.CanApprove) {
            EmployeeVisit.IsClockInTimeChanged = false;
        }

        var tempApprovedEmployeeVisitIds = [];

        angular.forEach($scope.VisitApprovalModal.VisitApprovalList, function (item, key) {
            if (item.IsClockInTimeChanged) {
                tempApprovedEmployeeVisitIds.push(item.EmployeeVisitID);
            }
        });

        if (tempApprovedEmployeeVisitIds.length > 0 &&
            tempApprovedEmployeeVisitIds.length == $scope.VisitApprovalModal.VisitApprovalList.filter(item => item.CanApprove).length)
            $scope.SelectAllIsClockInTimeChangedCheckbox = true;
        else
            $scope.SelectAllIsClockInTimeChangedCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAllIsClockInTimeChanged = function (oldValue) {
        var newValue = !oldValue;
        var tempApprovedEmployeeVisitIds = [];

        angular.forEach($scope.VisitApprovalModal.VisitApprovalList, function (item, key) {
            if (!item.CanApprove) {
                item.IsClockInTimeChanged = false;
            } else {
                item.IsClockInTimeChanged = newValue;
            }
            if (item.IsClockInTimeChanged) {
                tempApprovedEmployeeVisitIds.push(item.EmployeeVisitID);
            }
        });

        if (tempApprovedEmployeeVisitIds.length > 0 &&
            tempApprovedEmployeeVisitIds.length == $scope.VisitApprovalModal.VisitApprovalList.filter(item => item.CanApprove).length)
            $scope.SelectAllIsClockInTimeChangedCheckbox = true;
        else
            $scope.SelectAllIsClockInTimeChangedCheckbox = false;
        return true;
    };

    // This executes when select single checkbox selected in table.   
    $scope.SelectEmployeeVisitClockOutTimeChanged = function (EmployeeVisit) {
        if (!EmployeeVisit.CanApprove) {
            EmployeeVisit.IsClockOutTimeChanged = false;
        }

        var tempApprovedEmployeeVisitIds = [];

        angular.forEach($scope.VisitApprovalModal.VisitApprovalList, function (item, key) {
            if (item.IsClockOutTimeChanged) {
                tempApprovedEmployeeVisitIds.push(item.EmployeeVisitID);
            }
        });

        if (tempApprovedEmployeeVisitIds.length > 0 &&
            tempApprovedEmployeeVisitIds.length == $scope.VisitApprovalModal.VisitApprovalList.filter(item => item.CanApprove).length)
            $scope.SelectAllIsClockOutTimeChangedCheckbox = true;
        else
            $scope.SelectAllIsClockOutTimeChangedCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAllIsClockOutTimeChanged = function (oldValue) {
        var newValue = !oldValue;
        var tempApprovedEmployeeVisitIds = [];

        angular.forEach($scope.VisitApprovalModal.VisitApprovalList, function (item, key) {
            if (!item.CanApprove) {
                item.IsClockOutTimeChanged = false;
            } else {
                item.IsClockOutTimeChanged = newValue;
            }
            if (item.IsClockOutTimeChanged) {
                tempApprovedEmployeeVisitIds.push(item.EmployeeVisitID);
            }
        });

        if (tempApprovedEmployeeVisitIds.length > 0 &&
            tempApprovedEmployeeVisitIds.length == $scope.VisitApprovalModal.VisitApprovalList.filter(item => item.CanApprove).length)
            $scope.SelectAllIsClockOutTimeChangedCheckbox = true;
        else
            $scope.SelectAllIsClockOutTimeChangedCheckbox = false;
        return true;
    };

    $scope.GetVisitApprovalList = function () {
        var jsonData = angular.toJson({ employeeVisitIDs: $scope.EmployeeVisitIds });

        AngularAjaxCall($http, HomeCareSiteUrl.GetVisitApprovalList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.VisitApprovalModal.VisitApprovalList = response.Data;

                var selectAll = false;
                $scope.VisitApprovalModal.VisitApprovalList.forEach(v => {
                    if (v.CanApprove) { selectAll = true; }
                    v.IsClockInTimeChanged = false;
                    v.IsClockOutTimeChanged = false;
                });
                $scope.SelectAllApproveCheckbox = false;
                if (selectAll) { $scope.SelectAllApprove(false); }

            }
            ShowMessages(response);
        });
    };

    $scope.OpenVisitApprovalModal = function () {
        $scope.VisitApprovalModal = {};
        $scope.VisitApprovalModal.VisitApprovalList = [];
        $scope.ApprovedEmployeeVisitIds = [];

        $scope.GetVisitApprovalList();
        $('#VisitApprovalModal').modal({
            backdrop: 'static',
            keyboard: false
        });
    };

    $scope.CloseVisitApprovalModal = function () {
        $('#VisitApprovalModal').modal("hide");
        $scope.item.value = '';
        //Reset Selcted Checkbox items and Control
        $scope.SelectedEmployeeVisitIds = [];
        $scope.SelectAllCheckbox = false;
        //Reset Selcted Checkbox items and Control
        $scope.SelectAll(); // after setting SelectAllCheckbox to reset items
    };

    $(document).on("click", ".collapseSource", function () {

        var hasClassFaMinusCircle = $(this).hasClass("fa-minus-circle");
        if (hasClassFaMinusCircle == false) {
            $(this).removeClass("fa-plus-circle").addClass("fa-minus-circle");
        }
        else {
            $(this).removeClass("fa-minus-circle").addClass("fa-plus-circle");
        }

    });

    $scope.OnPage_GetApprovalVisitDetails = function (item, elem) {

        var hasClassFaMinusCircle = $(elem).hasClass("fa-minus-circle");
        if (hasClassFaMinusCircle == false) {
            $scope.GetApprovalVisitDetails(item);
        }

    };

    $scope.GetApprovalVisitDetails = function (item) {

        if (item.ApprovalVisitDetails == undefined) {
            item.ApprovalVisitDetails = {};
            item.ApprovalVisitDetails.EmployeeVisitID = item.EmployeeVisitID;
            item.ApprovalVisitDetails.PayorName = item.PayorName;
            item.ApprovalVisitDetails.AuthorizationCode = item.AuthorizationCode;
            item.ApprovalVisitDetails.ClockInTime = item.ClockInTime;
            item.ApprovalVisitDetails.ClockOutTime = item.ClockOutTime;
            item.ApprovalVisitDetails.EmployeeVisitNoteList = [];
            item.ApprovalVisitDetails.EmployeeVisitNoteList1 = [];
            item.ApprovalVisitDetails.EmployeeVisitNoteList2 = [];
            item.ApprovalVisitDetails.EmployeeVisitConclusionList = [];

            var pagermodel = {
                SearchEmployeeVisitNoteListPage: { EmployeeVisitID: item.EmployeeVisitID },
                pageSize: 999999,
                pageIndex: 1,
                sortIndex: "EmployeeVisitNoteID",
                sortDirection: "ASC"
            };
            var jsonData = angular.toJson(pagermodel);

            AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeVisitNoteList, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    item.ApprovalVisitDetails.EmployeeVisitNoteList = response.Data.Items;
                    angular.forEach(item.ApprovalVisitDetails.EmployeeVisitNoteList, function (item) {
                        item.TimeInMinutes = item.ServiceTime;
                    });
                    var list = item.ApprovalVisitDetails.EmployeeVisitNoteList.slice();
                    var middleIndex = Math.ceil(list.length / 2);

                    item.ApprovalVisitDetails.EmployeeVisitNoteList1 = list.splice(0, middleIndex);
                    item.ApprovalVisitDetails.EmployeeVisitNoteList2 = list.splice(-middleIndex);
                }
                ShowMessages(response);
            });

            var jsonData = angular.toJson({ EmployeeVisitID: item.EmployeeVisitID });
            AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeVisitConclusionList, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    item.ApprovalVisitDetails.EmployeeVisitConclusionList = response.Data;
                }
            });
        }

    };

    $scope.approveVisits = function () {
        var timeChange = $scope.VisitApprovalModal.VisitApprovalList.filter(v => v.IsChecked && (v.IsClockInTimeChanged || v.IsClockOutTimeChanged)).length > 0;
        if (timeChange) {
            var title = 'Approve Visit';
            var message = 'Are you sure you want to update the clock-in/clock-out time for Employee Visit for the record(s)?';
            bootboxDialog(function (result) {
                if (result) {
                    $scope.ApproveVisitList();
                }
            }, bootboxDialogType.Confirm, title, message, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
        }
        else {
            $scope.ApproveVisitList();
        }
    };

    $scope.ApproveVisitList = function () {
        var list = $scope.VisitApprovalModal.VisitApprovalList.filter(v => v.IsChecked).map(v => ({
            EmployeeVisitID: v.EmployeeVisitID,
            ApproveNote: v.ApproveNote,
            ClockInTime: v.IsClockInTimeChanged ? v.StartDate : v.ClockInTime,
            ClockOutTime: v.IsClockOutTimeChanged ? v.EndDate : v.ClockOutTime,
        }));
        var jsonData = angular.toJson({ List: list, Signature: $scope.VisitApprovalModal.Signature });
        AngularAjaxCall($http, HomeCareSiteUrl.ApproveVisitList, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.CloseVisitApprovalModal();
            }
        });
    };

    $scope.VisitTaskDocumentListModal = { DocumentList: [] };
    $scope.OpenVisitTaskDocumentListModal = function ($event, item) {
        $scope.VisitTaskDocumentListModal = { DocumentList: [] };
        $scope.VisitTaskDocumentListModal.EmployeeVisitID = item.EmployeeVisitID;
        $scope.VisitTaskDocumentListModal.EmployeeID = item.EmployeeID;
        $scope.VisitTaskDocumentListModal.ReferralID = item.ReferralID;
        $scope.GetVisitTaskDocumentList();
        $('#VisitTaskDocumentListModal').modal({
            backdrop: 'static',
            keyboard: false
        });
    };

    $scope.CloseVisitTaskDocumentListModal = function () {
        $("#VisitTaskDocumentListModal").modal("hide");
    };

    $scope.GetVisitTaskDocumentList = function () {
        var jsonData = angular.toJson({ employeeVisitID: $scope.VisitTaskDocumentListModal.EmployeeVisitID });

        AngularAjaxCall($http, HomeCareSiteUrl.GetVisitTaskDocumentList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.VisitTaskDocumentListModal.DocumentList = response.Data;
            }
            ShowMessages(response);
        });
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
    if (!ValideElement(window.frameElement)) { $scope.EmployeeVisitListPager.getDataCallback(); }

    $scope.OpenVisitNoteListModal = function ($event, item) {
        $scope.ResetVisitNoteSearchFilter();
        $('#visitNoteListModal').modal({
            backdrop: 'static',
            keyboard: false
        });
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
        if ($scope.EmployeeVisit.ClockInTime != null) {
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
                    $scope.id = false;
                }
            });
            $('#AddVisitNoteModal').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        else {
            toastr.error("ClockIn time required");
        }
    }

    $scope.VisitNote = {};
    $scope.SaveVisitNote = function (data) {
        var isValid = CheckErrors($("#addVisitNotefrm"));
        if (isValid && $scope.EmployeeVisit.ClockInTime != null && $scope.EmployeeVisit.ClockOutTime != null) {
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
                    $scope.ResetSearchFilter();
                }
            });
        }
        else {
            toastr.error("ClockIn time and ClockOut time required");
        }
    }
    $scope.VisitNote1 = {
        Hours: 0,
        Minutes: 10
    };
    $scope.SaveVisitNote1 = function (item) {
        var isValid = CheckErrors($("#addVisitNotefrm_Detail"));
        if (isValid) {
            $scope.VisitNote.EmployeeVisitID = $scope.SearchEmployeeVisitNoteListPage.EmployeeVisitID;
            $scope.VisitNote.ReferralTaskMappingID = item;
            // $scope.VisitNote.ReferralTaskMappingID = $scope.VisitNote.ReferralTaskMappingID.toString();
            $scope.VisitNote.Hours = 0;
            $scope.VisitNote.Minutes = 10;
            var jsonData = angular.toJson($scope.VisitNote);
            AngularAjaxCall($http, HomeCareSiteUrl.SaveVisitNote, jsonData, "Post", "json", "application/json", false).success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.GetEmployeeVisitNoteList();
                    $scope.VisitNote = {};
                    $('#AddVisitNoteModal').modal('hide');
                    //$scope.ResetSearchFilter();
                }
            });
        }
    }
    $scope.SaveVisitNote11 = function () {

        if ($scope.ReferralTaskMappingIDs.length > 0) {
            var isValid = CheckErrors($("#addVisitNotefrm_Detail"));
            $.each($scope.ReferralTaskMappingIDs, function (index, item) {

                if (isValid) {
                    $scope.VisitNote.EmployeeVisitID = $scope.SearchEmployeeVisitNoteListPage.EmployeeVisitID;
                    $scope.VisitNote.ReferralTaskMappingID = item;
                    // $scope.VisitNote.ReferralTaskMappingID = $scope.VisitNote.ReferralTaskMappingID.toString();
                    $scope.VisitNote.Hours = 0;
                    $scope.VisitNote.Minutes = 10;
                    var jsonData = angular.toJson($scope.VisitNote);
                    AngularAjaxCall($http, HomeCareSiteUrl.SaveVisitNoteTimeSheet, jsonData, "Post", "json", "application/json", false).success(function (response) {
                        ShowMessages(response);
                        if (response.IsSuccess) {

                            $scope.GetEmployeeVisitNoteList();
                            //$scope.ResetSearchFilter();
                        }
                    });


                    $scope.VisitNote = {};
                }
            });

            $('#AddVisitNoteModal').modal('hide');


            $scope.ReferralTaskMappingIDs = [];
            $scope.SelectAllFormCheckbox = false;
            $scope.EmployeeVisit.ClockInTime = $scope.EmployeeVisit.ClockInTime;
            //  $scope.VisitNote.SelectVisitTask();
            $scope.GetEmployeeVisitNoteList();
        }
        else {
            toastr.error("Please select VisitTask");
        }

    };
    $scope.SelectVisitTask = [];
    $scope.ReferralTaskMappingIDs = [];
    $scope.SelectVisitTask = function (item) {
        if (item.IsChecked)
            $scope.ReferralTaskMappingIDs.push(item.ReferralTaskMappingID);
        else
            $scope.ReferralTaskMappingIDs.remove(item.ReferralTaskMappingID);
        if ($scope.ReferralTaskMappingIDs.length == $scope.EmployeeVisitNoteListPager.currentPageSize)
            $scope.SelectAllVisitNoteCheckbox = true;
        else
            $scope.SelectAllVisitNoteCheckbox = false;

    };
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
    if (!ValideElement(window.frameElement)) { $scope.EmployeeVisitNoteListPager.getDataCallback(); }

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

    $("a#patientTimesheet").on('shown.bs.tab', function (e) {
        $scope.EmployeeVisitListPager.getDataCallback();
        $scope.EmployeeVisitNoteListPager.getDataCallback();
    });

    $scope.AddVisitConclusion = function (data) {
        if ($scope.EmployeeVisit.ClockInTime != null) {
            if (data !== undefined) {
                $scope.VisitConclusion.EmployeeVisitNoteID = data.EmployeeVisitNoteID;
                $scope.VisitConclusion.ReferralTaskMappingID = data.ReferralTaskMappingID;
                //   $scope.VisitConclusion.Answer = data.Description;
                $scope.VisitConclusion.VisitTaskDetail = data.VisitTaskDetail;
                $scope.VisitConclusion.Description = data.Description;
                $scope.VisitConclusion.AlertComment = data.AlertComment;
            }
            var jsonData = angular.toJson({ EmployeeVisitID: $scope.SearchEmployeeVisitNoteListPage.EmployeeVisitID });
            AngularAjaxCall($http, HomeCareSiteUrl.GetMappedVisitConclusion, jsonData, "Post", "json", "application/json", false).success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.MappedConclusionList = response.Data.VisitTaskList;
                    //  $scope.AnswerList = response.Data.Description;
                    //  $scope.AlertComment = response.Data.AlertComment;
                }
            });
            $('#AddVisitConclusionModal').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        else {
            toastr.error("ClockIn time required");
        }
    };

    $scope.VisitConclusion = {};
    $scope.SaveVisitConclusion = function (data) {
        var isValid = CheckErrors($("#addVisitConclusionfrm"));
        if (isValid) {
            if ($scope.EmployeeVisit.ClockInTime != null) {
                $scope.VisitConclusion.EmployeeVisitID = $scope.SearchEmployeeVisitNoteListPage.EmployeeVisitID;
                if ($scope.VisitConclusion.Description == "No") {
                    $scope.VisitConclusion.AlertComment = '';
                }
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
                        // $scope.VisitConclusion = {};
                        $('#AddVisitConclusionModal').modal('hide');
                    }
                });
            }
            else {
                toastr.error("ClockIn time required");
            }
        }
        else {
            toastr.error("Please select Conclusion");
        }
    }

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
    }

    $('#visitNoteListModal').on('hidden.bs.modal', function () {
        $scope.EmployeeVisitNoteList = {};
        $('#reportTabs a:first').tab('show');
    });

    $scope.BypassDetail = {};
    $scope.OpenBypassActionModal = function ($event, item) {
        debugger
        if (item.ClockInTime != null) {
            $('#ByPassActionModal').modal({
                backdrop: 'static',
                keyboard: false
            });
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
            $scope.BypassDetail.ScheduleID = item.ScheduleID;
            $scope.EmployeeVisit.ClockInTime = item.ClockInTime;
            $scope.EmployeeVisit.ClockOutTime = item.ClockOutTime
        }
        else {
            toastr.error("ClockIn time required");
        }
    };

    $scope.OpenBypassRejectReasonModal = function (modal) {
        $('#ByPassActionModal').modal('hide');
        $('#ByPassRejectReasonModal').modal({
            backdrop: 'static',
            keyboard: false
        });
        $scope.BypassDetail = modal;
    };

    $scope.TimeSheetInfoModal = function ($event, item) {
        $('#TimeSheetInfoModal').modal({
            backdrop: 'static',
            keyboard: false
        });

        $scope.EmployeeVisitMark = item;
    };

    $scope.ActionTaken = function (EmployeeVisit, IsApprove) {
        debugger
        if ($scope.EmployeeVisit.ClockInTime != null && $scope.EmployeeVisit.ClockOutTime != null) {
            var isValid = IsApprove ? true : CheckErrors($("#addRejectReasonfrm"));
            if (isValid) {
                EmployeeVisit.IsApprove = IsApprove;
                EmployeeVisit.ByPassReasonClockIn = EmployeeVisit.ByPassReasonsClockIn;
                ShowVisitReasonActionModal({
                    ScheduleID: EmployeeVisit.ScheduleID,
                    OnSet: function (data, save) {
                        var jsonData = angular.toJson(EmployeeVisit);
                        AngularAjaxCall($http, HomeCareSiteUrl.BypassActionTaken, jsonData, "Post", "json", "application/json", false).success(function (response) {
                            ShowMessages(response);
                            if (response.IsSuccess) {
                                save();
                                if (IsApprove) {
                                    $scope.BypassDetail = {};
                                    $('#ByPassActionModal').modal('hide');
                                    $scope.EmployeeVisit.ActionTaken = 2;
                                    $scope.EmployeeVisit.ByPassReasonClockIn = $scope.EmployeeVisit.ByPassReasonClockIn;
                                    $scope.EmployeeVisit.ByPassReasonClockIn = $scope.EmployeeVisit.ByPassReasonClockIn;
                                    $scope.EmployeeVisit.Reason = "Approval";
                                } else {
                                    $('#ByPassRejectReasonModal').modal('hide');
                                    $scope.BypassDetail.ActionTaken = 3; //Reject Action;
                                    $scope.EmployeeVisit.ActionTaken = 3;
                                    $scope.EmployeeVisit.RejectReason = $scope.EmployeeVisit.RejectReason;
                                    $scope.EmployeeVisit.Reason = "Rejection";
                                }
                                $scope.GetEmployeeVisitList();
                            }
                        });
                    }
                });
            }
        }
        else {
            toastr.error("ClockIn time and ClockOut time required");
        }
    };

    $('#ByPassRejectReasonModal').on('hidden.bs.modal', function () {
        HideErrors("#addRejectReasonfrm");
    });

    $('#ByPassRejectReasonModal').on('hidden.bs.modal', function () {
        $scope.BypassDetail = {};
    });


    $scope.PrintPCATimeSheet = function (employeeVisitNoteId) {
        var jsonData = angular.toJson({ id: employeeVisitNoteId });
        AngularAjaxCall($http, HomeCareSiteUrl.GeneratePcaTimeSheetPdfURL, jsonData, "Post", "json", "application/json").success(function (response) {
        });
    };
    $('#ClockInClockOutAdressModal').on('hidden.bs.modal', function () {
        $scope.ClockInClockOutMap = {};
    });
    $scope.ClockInClockOutMap = {};
    $scope.OpenClockInClockOutMapModal = function ($event, item, ClickMode) {
        $('#ClockInClockOutAdressModal').modal({
            backdrop: 'static',
            keyboard: false
        });
        $scope.ClockInClockOutMap = item;
        $timeout(function () {
            ClockInOutinitMap($scope.ClockInClockOutMap, ClickMode);
        }, 500);

        //$scope.SearchEmployeeVisitNoteListPage.EmployeeVisitID = item.EmployeeVisitID;
        //$scope.TempSearchEmployeeVisitNoteListPage.EmployeeVisitID = item.EmployeeVisitID;
        //$scope.EmployeeName = item.Name;
        //$scope.PatientName = item.PatientName;
        //$scope.SurveyCompleted = item.SurveyCompleted;
        //$scope.GetEmployeeVisitNoteList();
    }
    $scope.ReferralPayorList = [];
    $scope.ReferralEmployeeVisit = {};
    $('#ReferralEmployeeVisitmodel').on('hidden.bs.modal', function () {
        $scope.ReferralPayorList = [];
        $scope.ReferralEmployeeVisit = {};
    });
    $scope.saveSelectedReferralPayorId = function () {
        $scope.ReferralPayorList;
        $scope.ReferralEmployeeVisit;
        var SelectedReferralPayorID = $("#drpReferralPayorList option:selected").val();
        if (SelectedReferralPayorID) {
            var EmployeeVisitPayermodal = {
                EmployeeVisitID: $scope.ReferralEmployeeVisit.EmployeeVisitID,
                ReferralPayorID: SelectedReferralPayorID
            };
            var jsonData = angular.toJson(EmployeeVisitPayermodal);
            AngularAjaxCall($http, HomeCareSiteUrl.SaveEmployeeVisitPayer, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.ResetSearchFilter();
                }
                $('#ReferralEmployeeVisitmodel').modal('hide');
                ShowMessages(response);
            });
        }
    };
    var id = true;
    $scope.Change = function (id) {
        if (id !== true) {

            $('#addVisitNotefrm').html('show');
            //   $('#addVisitNotefrm_Detail').html('hide');
        }
        else {
            $('#addVisitNotefrm_Detail').html('show');
            //  $('#addVisitNotefrm').html('hide');
        }

    };


    $scope.Today = new Date();
    $scope.missingTimesheetsPager = [];
    $scope.TempSearchMissingTsListPage = [];
    $scope.MissingTimeSheetList = [];
    $scope.SelectedMissingTs = [];
    $scope.MissingTimeSheetModel = $.parseJSON($("#hdnSetMissingTSListPage").val());
    //$scope.SearchMissingTSListPage = $scope.MissingTimeSheetModel.SearchMissingTSListPage;
    //$scope.SearchMissingTSListPage.StartDate = new Date()
    //$scope.SearchMissingTSListPage.EndDate = new Date();

    $scope.SetMissingTS_PostData = function (fromIndex) {
        var startDate = $scope.TempSearchMissingTsListPage.StartDate;
        var endDate = $scope.TempSearchMissingTsListPage.EndDate;
        var employeeList = $scope.TempSearchMissingTsListPage.EmployeeID ? $scope.TempSearchMissingTsListPage.EmployeeID.toString() : "";
        var referralList = $scope.TempSearchMissingTsListPage.ReferralID ? $scope.TempSearchMissingTsListPage.ReferralID.toString() : "";

        var SearchMissingTSListPage = {
            "StartDate": startDate,
            "EndDate": endDate,
            "EmployeeIDs": employeeList,
            "ReferralIDs": referralList,
        };
        return angular.toJson(SearchMissingTSListPage);
    };
    $scope.ResetTS_PostData = function () {
        $scope.TempSearchMissingTsListPage.StartDate = undefined;
        $scope.TempSearchMissingTsListPage.EndDate = undefined;
        $scope.TempSearchMissingTsListPage.EmployeeID = [];
        $scope.TempSearchMissingTsListPage.ReferralID = [];
    };
    $scope.OpenMissingTimesheetModal = function () {
        $("#MissingTimeSheetModal").modal({
            backdrop: 'static',
            keyboard: false
        });
        $scope.SelectedMissingTs = [];

        $scope.TempSearchMissingTsListPage.EmployeeID = $scope.EmployeeVisitModel.SearchEmployeeVisitListPage.EmployeeIDs;
        $scope.TempSearchMissingTsListPage.ReferralID = $scope.EmployeeVisitModel.SearchEmployeeVisitListPage.ReferralIDs;
    }
    $scope.GetMissingTimeSheet = function (isSearchDataMappingRequire) {

        var jsonData = $scope.SetMissingTS_PostData(1);

        AngularAjaxCall($http, HomeCareSiteUrl.GetMissingTimeSheet, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.MissingTimeSheetList = response.Data.Items;
                // console.log('response', response);
                $scope.missingTimesheetsPager.currentPageSize = response.Data.Items.length;
                $scope.missingTimesheetsPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };
    $scope.CloseMissingTimeSheetModal = function () {
        $("#MissingTimeSheetModal").modal("hide");
        $scope.MissingTimeSheetList = null;
        $scope.ResetTS_PostData();
    }


    $scope.AddMissingTimeASheet = function () {
        var jsonData = angular.toJson($scope.SelectedMissingTs);
        AngularAjaxCall($http, HomeCareSiteUrl.AddMissingTimeASheet, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.GetMissingTimeSheet();
                $scope.SelectedMissingTs = [];
                //console.log('response', response);
                $scope.GetEmployeeVisitList();
            }
            ShowMessages(response);
            $scope.ResetTS_PostData();
        });
    }

    $scope.SelectMissingTS = function (MissingTimeSheet) {
        if (MissingTimeSheet.IsChecked) {
            $scope.SelectedMissingTs.push(MissingTimeSheet);
        }
        else {
            $scope.SelectedMissingTs.remove(MissingTimeSheet);
        }
    };
    $scope.PriorAutherizationCodeList = [];
    $scope.GetPriorAutherizationCodes = function (item) {
        item.PayorID = item.PayorID === null || item.PayorID === undefined ? 0 : item.PayorID;
        var jsonData = angular.toJson({ payorID: item.PayorID, referralID: item.ReferralID });
        AngularAjaxCall($http, HomeCareSiteUrl.HC_GetPriorAutherizationCodeByPayorAndRererrals, jsonData, "Post", "json", "application/json").success(function (response) {
            //ShowMessages(response);
            if (response.IsSuccess) {
                $scope.PriorAutherizationCodeList = response.Data;
                //$scope.UpdateEmployeeVisitPayorAutherizationCode(item);
            }
        });
    };
    $scope.ReferralBillingAuthorizationID = {}
    $scope.AuthorizationCode = {};
    $scope.SelectValue = function (item) {
        $scope.ReferralBillingAuthorizationID = item.Value;
        $scope.EmployeeVisit.AuthorizationCode = item.Name;
    }
    $scope.UpdateEmployeeVisitPayorAutherizationCode = function (item) {
        if ($scope.ReferralBillingAuthorizationID != undefined && $scope.ReferralBillingAuthorizationID != null) {
            item.ReferralBillingAuthorizationID = $scope.ReferralBillingAuthorizationID;
        }
        ShowVisitReasonActionModal({
            ScheduleID: item.ScheduleID,
            OnSet: function (data, save) {
                var jsonData = angular.toJson({ EmployeeVisitID: item.EmployeeVisitID, PayorID: item.PayorID, ReferralBillingAuthorizationID: item.ReferralBillingAuthorizationID, EmployeeID: item.EmployeeID });
                AngularAjaxCall($http, HomeCareSiteUrl.UpdateEmployeeVisitPayorAutherizationCode, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        save();
                        item.IsEditableAuth = false;
                        if (response.Data != null && response.Data != undefined) {
                            $scope.EmployeeVisit.AuthorizationCode = response.Data.AuthorizationCode;
                            $scope.EmployeeVisit.PayorName = response.Data.PayorName;
                            $scope.EmployeeVisit.Name = response.Data.Name;
                        }
                        else {
                            $scope.EmployeeVisit.AuthorizationCode = '';
                            $scope.EmployeeVisit.PayorName = '';
                        }
                        $scope.GetEmployeeVisitList();
                    }
                });
            }
        });
    };


    $scope.EmployeeVisit = [];
    $scope.ReferralPayorsMappingList = [];
    $scope.EmployeeList = [];
    $scope.OpenVisitNoteListModal1 = function ($event, EmployeeVisit) {
        $scope.EmployeeVisit = EmployeeVisit;
        $('#ByPassRejectReasonModal').modal('hide');
        // $scope.EmployeeVisit.push(EmployeeVisit);
        //   $scope.AddVisitNote();
        var jsonDataEmployeeList = angular.toJson({});
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeList, jsonDataEmployeeList, "Get", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.EmployeeList = response.Data;
            }
        });



        var jsonDataPayor = angular.toJson({ referralID: $scope.EmployeeVisit.ReferralID, startDate: $scope.EmployeeVisit.StartDate });
        AngularAjaxCall($http, HomeCareSiteUrl.HC_GetReferralPayorsMapping, jsonDataPayor, "Post", "json", "application/json").success(function (response) {
            //ShowMessages(response);
            if (response.IsSuccess) {
                $scope.ReferralPayorsMappingList = response.Data;
            }
        });

        $scope.EmployeeVisit.PayorID = $scope.EmployeeVisit.PayorID === null || $scope.EmployeeVisit.PayorID === undefined ? 0 : $scope.EmployeeVisit.PayorID;
        var jsonDataAuth = angular.toJson({ payorID: $scope.EmployeeVisit.PayorID, referralID: $scope.EmployeeVisit.ReferralID });
        AngularAjaxCall($http, HomeCareSiteUrl.HC_GetPriorAutherizationCodeByPayorAndRererrals, jsonDataAuth, "Post", "json", "application/json").success(function (response) {
            //ShowMessages(response);
            if (response.IsSuccess) {
                $scope.PriorAutherizationCodeList = response.Data;
            }
        });

        var jsonData = angular.toJson({ EmployeeVisitID: $scope.EmployeeVisit.EmployeeVisitID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeVisitConclusionList, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.EmployeeVisitConclusionList = response.Data;
            }
        });
        AngularAjaxCall($http, HomeCareSiteUrl.GetDeviationNotesURL, jsonData, "Post", "json", "application/json").success(function (response) {
            $scope.DeviationNotesList = response.Data.DeviationNoteModelList;
            $scope.EmployeeVisitModel.DeviationList = response.Data.DeviationList;
        })
        $scope.SearchEmployeeVisitNoteListPage.EmployeeVisitID = EmployeeVisit.EmployeeVisitID;
        $scope.TempSearchEmployeeVisitNoteListPage.EmployeeVisitID = EmployeeVisit.EmployeeVisitID;
        $scope.EmployeeName = EmployeeVisit.Name;
        $scope.PatientName = EmployeeVisit.PatientName;
        $scope.SurveyCompleted = EmployeeVisit.SurveyCompleted;
        var EmployeeVisitID = $scope.EmployeeVisit.EmployeeVisitID;
        if (EmployeeVisit.ActionTaken == 2) {
            $scope.EmployeeVisit.ByPassReasonsClockIn = EmployeeVisit.ByPassReasonClockIn;
            $scope.EmployeeVisit.Reason = "Approval";
        }
        if (EmployeeVisit.ActionTaken == 3) {
            $scope.EmployeeVisit.ByPassReasonsClockIn = EmployeeVisit.RejectReason;
            $scope.EmployeeVisit.Reason = "Rejection";
        }
        if (EmployeeVisit.ActionTaken != 2 && EmployeeVisit.ActionTaken != 3) {
            $scope.EmployeeVisit.ByPassReasonsClockIn = EmployeeVisit.RejectReason;
            $scope.EmployeeVisit.Reason = "0";
        }

        $('#AddVisitNoteModal').modal('hide');
        $('#AddVisitConclusionModal').modal('hide');
        $('#ByPassActionModal').modal('hide');
        $('#ClockInClockOutAdressModal').modal('hide');
        $('#EditTimeSheet').modal({
            backdrop: 'static',
            keyboard: false
        }); //.modal('show');

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

    }
    $scope.CloseEditTimeSheet = function () {
        $('#EditTimeSheet').modal('hide');
    }

    $scope.SavePCAComplete = function (EmployeeVisit) {
        if (EmployeeVisit.ClockInTime != null) {
            $scope.EmployeeVisitModel.EmployeeVisitID = EmployeeVisit.EmployeeVisitID;
            var jsonData = angular.toJson({ EmployeeVisitID: EmployeeVisit.EmployeeVisitID });
            if ($scope.EmployeeVisitNoteList.length > 0 || $scope.EmployeeVisitConclusionList.length > 0) {
                AngularAjaxCall($http, HomeCareSiteUrl.SavePCAComplete, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        ShowMessages(response);
                    }
                    //ShowMessages(response);
                    $scope.Refresh();
                    $scope.GetEmployeeVisitList();
                    $scope.EmployeeVisit.IsPCACompleted = 1;
                });
            }
            else {
                toastr.error("VisitTask/Conclusion Required");
            }
        }
        else {
            toastr.error("ClockIn time required");
        }
    }
    $scope.VisitTaskModelClosed = function () {
        $('#AddVisitNoteModal').modal('hide');
        $('#AddVisitConclusionModal').modal('hide');
        $('#ByPassActionModal').modal('hide');
        $('#ClockInClockOutAdressModal').modal('hide');
    }
    $scope.VisitTaskModelClosed = function () {
        $('#AddVisitNoteModal').modal('hide');
    }
    $scope.ConclusionModelClosed = function () {
        $('#AddVisitConclusionModal').modal('hide');
    }
    $scope.ByPassActionModelClosed = function () {
        $('#ByPassActionModal').modal('hide');
    }
    $scope.ClockInClockOutAdressModelClosed = function () {
        $('#ClockInClockOutAdressModal').modal('hide');
    }
    $scope.TimeSheetInfoModalClosed = function () {
        $('#TimeSheetInfoModal').modal('hide');
    }
    $scope.PrintPCATimeSheet = function (EncryptedEmployeeVisitID) {
        GeneratePcaTimeSheetPdf = "generatepcatimesheetpdf/";
        location.href = GeneratePcaTimeSheetPdf + EncryptedEmployeeVisitID;

    }
    $scope.DeleteEmployeeVisitConclusion = function (EmployeeVisitConclusion, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchEmployeeVisitNoteListPage.ListOfIdsInCsv = EmployeeVisitConclusion.EmployeeVisitNoteID > 0 ? EmployeeVisitConclusion.EmployeeVisitNoteID.toString() : $scope.SelectedEmployeeVisitNoteIds.toString();

                //if (EmployeeVisitNoteID > 0) {
                //    if ($scope.EmployeeVisitNoteListPager.currentPage != 1)
                //        $scope.EmployeeVisitNoteListPager.currentPage = $scope.EmployeeVisitNoteList.length === 1 ? $scope.EmployeeVisitNoteListPager.currentPage - 1 : $scope.EmployeeVisitNoteListPager.currentPage;
                //} else {

                //    if ($scope.EmployeeVisitNoteListPager.currentPage != 1 && $scope.SelectedEmployeeVisitNoteIds.length == $scope.EmployeeVisitNoteListPager.currentPageSize)
                //        $scope.EmployeeVisitNoteListPager.currentPage = $scope.EmployeeVisitNoteListPager.currentPage - 1;
                //}

                //Reset Selcted Checkbox items and Control
                //$scope.SelectedEmployeeVisitNoteIds = [];
                //$scope.SelectAllVisitNoteCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostVisitNoteData($scope.EmployeeVisitNoteListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteEmployeeVisitNote, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        var jsonData = angular.toJson({ EmployeeVisitID: $scope.SearchEmployeeVisitNoteListPage.EmployeeVisitID });
                        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeVisitConclusionList, jsonData, "Post", "json", "application/json").success(function (response) {
                            ShowMessages(response);
                            if (response.IsSuccess) {
                                $scope.EmployeeVisitConclusionList = response.Data;
                            }
                        })

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

    $scope.EditByPassReasonNote = function () {
        $scope.IsShow = true;
        $('#EditByPassReasonNote').modal({
            backdrop: 'static',
            keyboard: false
        });
    }
    $scope.SaveByPassReasonNotes = function (EmployeeVisit) {
        ShowVisitReasonActionModal({
            ScheduleID: EmployeeVisit.ScheduleID,
            OnSet: function (data, save) {
                $scope.IsShow = false;
                var jsonData = angular.toJson(EmployeeVisit);
                AngularAjaxCall($http, HomeCareSiteUrl.SaveByPassReasonNoteURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    toastr.success("successfully saved ByPass Notes");
                    if (response.IsSuccess) {
                        save();
                        if (IsApprove) {
                            $scope.BypassDetail = {};
                            $('#ByPassActionModal').modal('hide');
                        } else {
                            $('#ByPassRejectReasonModal').modal('hide');
                            $scope.BypassDetail.ActionTaken = 3;
                            $scope.EmployeeVisit.ActionTaken = 3;
                        }
                        $scope.GetEmployeeVisitList();
                    }
                });
            }
        });
    }

    $scope.EditByPassReasonCancel = function () {
        $scope.IsShow = false;
    }
    $scope.EditByPassReasonNoteClose = function () {
        $('#EditByPassReasonNote').modal('hide');
    }
    $scope.SaveByPassReasonNote = function (modal, IsApprove) {
        var isValid = IsApprove ? true : CheckErrors($("#addRejectReasonfrm"));
        if (isValid) {
            if (modal.ActionTaken == 3) {
                modal.RejectReason = modal.ByPassReasonsClockIn;
            }
            else {
                modal.ByPassReasonClockIn = modal.ByPassReasonsClockIn;
            }
            var jsonData = angular.toJson(modal);
            AngularAjaxCall($http, HomeCareSiteUrl.SaveByPassReasonNoteURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                toastr.success("successfully save Approval Required/Notes");
                if (response.IsSuccess) {
                    if (IsApprove) {
                        $scope.BypassDetail = {};
                        $('#ByPassActionModal').modal('hide');
                        //  $scope.EmployeeVisit.ActionTaken =2;
                    } else {
                        $('#ByPassRejectReasonModal').modal('hide');
                        $scope.BypassDetail.ActionTaken = 3; //Reject Action;
                        $scope.EmployeeVisit.ActionTaken = 3;
                    }
                    $scope.GetEmployeeVisitList();
                }
            });
        }
        $('#EditByPassReasonNote').modal('hide');
    };
    $scope.SaveByPassReasonNote1 = function (EmployeeVisit, IsApprove) {
        ShowVisitReasonActionModal({
            ScheduleID: EmployeeVisit.ScheduleID,
            OnSet: function (data, save) {
                var isValid = IsApprove ? true : CheckErrors($("#addRejectReasonfrm"));
                EmployeeVisit.ActionTaken = 3;
                if (isValid) {
                    if (EmployeeVisit.ActionTaken == 3) {
                        EmployeeVisit.RejectReason = EmployeeVisit.ByPassReasonsClockIn;
                        EmployeeVisit.IsApprove = false;
                        EmployeeVisit.ActionTaken = 3;
                    }
                    else {
                        //  BypassDetail.ByPassReasonClockIn = BypassDetail.ByPassReasonsClockIn;
                    }
                    var jsonData = angular.toJson(EmployeeVisit);
                    AngularAjaxCall($http, HomeCareSiteUrl.BypassActionTaken, jsonData, "Post", "json", "application/json", false).success(function (response) {
                        toastr.success("successfully save Approval Required/Notes");
                        if (response.IsSuccess) {
                            save();
                            if (IsApprove) {
                                $scope.BypassDetail = {};
                                $('#ByPassActionModal').modal('hide');
                                $scope.EmployeeVisit.ActionTaken = 3; //Reject Action;
                                $scope.EmployeeVisit.ActionTaken = 3;
                                $scope.EmployeeVisit.RejectReason = EmployeeVisit.RejectReason;
                                $scope.EmployeeVisit.Reason = "Rejection";
                            } else {
                                $('#ByPassRejectReasonModal').modal('hide');
                                $scope.EmployeeVisit.ActionTaken = 3; //Reject Action;
                                $scope.EmployeeVisit.ActionTaken = 3;
                                $scope.EmployeeVisit.RejectReason = BypassDetail.RejectReason;
                                $scope.EmployeeVisit.Reason = "Rejection";
                            }
                            $scope.GetEmployeeVisitList();
                            $scope.EmployeeVisit.ActionTaken = 3; //Reject Action;
                            $scope.EmployeeVisit.ActionTaken = 3;
                            $scope.EmployeeVisit.RejectReason = EmployeeVisit.RejectReason;
                            $scope.EmployeeVisit.Reason = "Rejection";
                        }
                    });
                }
                $('#EditByPassReasonNote').modal('hide');
            }
        });
    };
    $scope.SaveNote = function (modal, IsApprove) {
        var isValid = IsApprove ? true : CheckErrors($("#addRejectReasonfrm"));
        if (isValid) {
            modal.IsApprove = IsApprove;
            modal.ByPassReasonClockIn = modal.ByPassReasonsClockIn;
            var jsonData = angular.toJson(modal);
            AngularAjaxCall($http, HomeCareSiteUrl.BypassActionTaken, jsonData, "Post", "json", "application/json", false).success(function (response) {
                toastr.success("successfully save Approval Required/Notes");
                if (response.IsSuccess) {
                    if (IsApprove) {
                        $scope.BypassDetail = {};
                        $('#ByPassActionModal').modal('hide');
                        $scope.EmployeeVisit.ActionTaken = 2;
                    } else {
                        $('#ByPassRejectReasonModal').modal('hide');
                        $scope.BypassDetail.ActionTaken = 3; //Reject Action;
                        $scope.EmployeeVisit.ActionTaken = 3;
                    }
                    $scope.GetEmployeeVisitList();
                }
            });
        }
        $('#EditByPassReasonNote').modal('hide');
    };
    $scope.Deviation = [];
    $scope.SaveDeviationNotes = function (data) {
        if ($scope.Deviation.DeviationID != null) {
            if ($scope.Deviation.DeviationNotes != null) {
                $scope.Deviation.EmployeeVisitID = $scope.EmployeeVisit.EmployeeVisitID;
                var jsonData = angular.toJson({ DeviationID: $scope.Deviation.DeviationID, DeviationNoteID: $scope.Deviation.DeviationNoteID, DeviationNotes: $scope.Deviation.DeviationNotes, EmployeeVisitID: $scope.EmployeeVisit.EmployeeVisitID, Hours: $scope.Deviation.Hours, Minutes: $scope.Deviation.Minutes });
                AngularAjaxCall($http, HomeCareSiteUrl.SaveDeviationNotesURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        AngularAjaxCall($http, HomeCareSiteUrl.GetDeviationNotesURL, jsonData, "Post", "json", "application/json").success(function (response) {
                            $scope.DeviationNotesList = response.Data.DeviationNoteModelList;
                            $scope.EmployeeVisitModel.DeviationList = response.Data.DeviationList;
                        })
                        $scope.DeviationID = '';
                        $scope.DeviationNotes = '';
                        $scope.Deviation.DeviationNoteID = '';
                        $scope.Deviation.Minutes = '';
                        $scope.Deviation.Hours = '';
                    }
                    $("#deviationModal").modal("hide");
                });

            }
            else {
                toastr.error("Deviation Notes should not blank");
            }
        }
        else {
            toastr.error("Please select Deviation type");
        }

    };
    $scope.AddClassTask = function () {
        angular.element('#task').removeClass("hide");
        angular.element('#Deviation1').addClass("hide");
        angular.element('#conclusion1').addClass("hide");
    }
    $scope.RemoveClassConclusion = function () {
        angular.element('#task').addClass("hide");
        angular.element('#Deviation1').addClass("hide");
        angular.element('#conclusion1').removeClass("hide");
    }
    $scope.RemoveClassDeviation = function () {
        angular.element('#task').addClass("hide");
        angular.element('#conclusion1').addClass("hide");
        angular.element('#Deviation1').removeClass("hide");
    }
    $scope.OpenDeviationModal = function () {
        if ($scope.EmployeeVisit.ClockInTime != null) {
            $scope.Deviation.DeviationID = null;
            $scope.Deviation.DeviationNotes = null;
            $scope.Deviation.Hours = null;
            $scope.Deviation.Minutes = null;
            var jsonData = angular.toJson({ EmployeeVisitID: $scope.SearchEmployeeVisitNoteListPage.EmployeeVisitID });
            AngularAjaxCall($http, HomeCareSiteUrl.GetMappedVisitTask, jsonData, "Post", "json", "application/json", false).success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.HourList = response.Data.HourList;
                    $scope.MinuteList = response.Data.MinuteList;
                    $scope.id = false;
                }
            });
            $('#deviationModal').modal({ backdrop: 'static', keyboard: false });
        }
        else {
            toastr.error("ClockIn time required");
        }

    }
    $scope.GetDeviationNotesList = function () {
        $scope.DeviationNotesList = [];
        var jsonData = angular.toJson({});
        AngularAjaxCall($http, HomeCareSiteUrl.GetDeviationNotesURL, jsonData, "Get", "json", "application/json").success(function (response) {
            $scope.DeviationNotesList = response.Data.DeviationNoteModelList;
            $scope.EmployeeVisitModel.DeviationList = response.Data.DeviationList;
        });
    };
    $scope.DeleteDeviation = function (Deviation, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        $scope.ListOfIdsInCsv = Deviation.DeviationNoteID
        bootboxDialog(function (result) {
            if (result) {
                // $scope.ListOfIdsInCsv = Deviation.DeviationNoteID //> 0 ? Deviation.DeviationNoteID.toString() : $scope.SelectedDeviationNoteIDs.toString();
                var jsonData = angular.toJson({ ListOfIdsInCsv: $scope.ListOfIdsInCsv, EmployeeVisitID: $scope.EmployeeVisit.EmployeeVisitID });
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteDeviationNoteURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        AngularAjaxCall($http, HomeCareSiteUrl.GetDeviationNotesURL, jsonData, "Post", "json", "application/json").success(function (response) {
                            $scope.DeviationNotesList = response.Data.DeviationNoteModelList;
                            $scope.EmployeeVisitModel.DeviationList = response.Data.DeviationList;
                        })

                    }

                });
            }
        }, bootboxDialogType.Confirm, title, "Are you sure you want to delete Deviation Note record(s)?", bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };
    $scope.EditDeviation = function (data) {
        $scope.OpenDeviationModal();
        if (data != undefined) {
            $scope.Deviation.DeviationID = data.DeviationID;
            $scope.Deviation.DeviationNotes = data.DeviationNotes;
            $scope.Deviation.DeviationType = data.DeviationType;
            $scope.Deviation.Hours = Math.floor(data.DeviationTime / 60);
            $scope.Deviation.Minutes = (data.DeviationTime % 60);
        }
        $scope.Deviation.DeviationID = data.DeviationID;
        $scope.Deviation.DeviationNotes = data.DeviationNotes;
        $scope.Deviation.DeviationNoteID = data.DeviationNoteID;
        $scope.Deviation.DeviationType = data.DeviationType;
        $('#deviationModal').modal({
            backdrop: 'static',
            keyboard: false
        });
    }

    $scope.VisitNoteForms = {};
    $scope.GetMappedVisitTaskForms = function () {
        var jsonData = angular.toJson({ employeeVisitID: $scope.EmployeeVisit.EmployeeVisitID, referralTaskMappingID: $scope.VisitNoteForms.ReferralTaskMappingID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetMappedVisitTaskForms, jsonData, "Post", "json", "application/json", false).success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.VisitNoteForms.TaskFormList = response.Data;
            }
        });
    };

    $scope.ShowVisitNoteForms = function (visitNote) {
        $scope.VisitNoteForms = {};
        if (visitNote != undefined) {
            $scope.VisitNoteForms.ReferralTaskMappingID = visitNote.ReferralTaskMappingID;
            $scope.VisitNoteForms.VisitTaskDetail = visitNote.VisitTaskDetail;
        }
        $scope.GetMappedVisitTaskForms();
        $('#ShowVisitNoteFormsModal').modal({
            backdrop: 'static',
            keyboard: false
        });
    };

    $scope.HideVisitNoteForms = function () {
        $('#ShowVisitNoteFormsModal').modal('hide');
    };

    $scope.ShowVisitNoteForm = function (item, mode, data) {
        data = data || {};
        $scope.VisitNoteForms.Form = item;
        $scope.VisitNoteForms.Form.EmployeeVisitID = data.EmployeeVisitID ? data.EmployeeVisitID : $scope.EmployeeVisit.EmployeeVisitID;
        $scope.VisitNoteForms.Form.EmployeeID = data.EmployeeID ? data.EmployeeID : $scope.EmployeeVisit.EmployeeID;
        $scope.VisitNoteForms.Form.ReferralID = data.ReferralID ? data.ReferralID : $scope.EmployeeVisit.ReferralID;
        $scope.VisitNoteForms.Form.ReferralTaskMappingID = $scope.VisitNoteForms.ReferralTaskMappingID;
        $scope.VisitNoteForms.Form.SectionName = $scope.EmployeeVisit.CareType || moment().format('YYYY-MM-DD HH:mm:ss');
        $scope.VisitNoteForms.Form.SubSectionName = moment($scope.EmployeeVisit.ClockInTime).format('MM/DD/YYYY');


        if (item.IsOrbeonForm) {
            var record = mode == 'new' ? '' : '/' + item.OrbeonFormID;
            $scope.VisitNoteForms.Form.URL = HomeCareSiteUrl.OrbeonLoadHtmlFormURL + '?FormURL=' + item.NameForUrl + '/' + mode + record + "?form-version=" + item.Version
                + "&orbeon-embeddable=true"
                + "&OrgPageID=" + "EmployeeVisitTaskForm"
                + "&IsEditMode=" + "true"
                + "&EmployeeID=" + $scope.VisitNoteForms.Form.EmployeeID
                + "&ReferralID=" + $scope.VisitNoteForms.Form.ReferralID
                + "&OriginalEBFormID=" + item.EBFormID
                + "&FormId=" + item.FormId
                + "&FormName=" + item.Name
                + "&OrganizationId=" + window.OrgID
                + "&UserId=" + window.LUserId
                + "&Version=" + item.Version;
        }
        $('#ShowVisitNoteFormModal').modal({
            backdrop: 'static',
            keyboard: false
        });
    };

    $scope.HideVisitNoteForm = function () {
        $('#ShowVisitNoteFormModal').modal('hide');
    };

    $scope.SaveVisitNoteOrbeonForm = function (data) {
        var form = JSON.parse(JSON.stringify($scope.VisitNoteForms.Form || {}));
        if (form.ReferralTaskMappingID > 0) {
            var OrbeonID = data.split(':')[1].trim();
            form.OrbeonFormID = OrbeonID;
            var jsonData = angular.toJson(form);

            if (OrbeonID != '') {
                AngularAjaxCall($http, HomeCareSiteUrl.SaveVisitNoteOrbeonForm, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.HideVisitNoteForm();
                        $scope.GetMappedVisitTaskForms();
                        $scope.VisitNoteForms.Form = {};
                    }
                });
            }
        }
    };

    $scope.DeleteVisitNoteForm = function (item) {
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = angular.toJson({ referralTaskFormMappingID: item.ReferralTaskFormMappingID });
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteVisitNoteForm, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.GetMappedVisitTaskForms();
                    }
                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.VTFDeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.BulkTypeList = [
        { value: "CareType", name: "Care Type" },
        { value: "AuthorizationCode", name: "Authorization Code" },
        { value: "Payor", name: "Payor" },
        { value: "MarkAsComplete", name: "Mark As Complete" },
        { value: "PrintTimesheets", name: "Print Timesheets" },
        { value: "DeleteSelected", name: "Delete Selected" },
        { value: "ApproveSelected", name: "Approve Selected" }
    ];
    $scope.CategoryList = [];
    $scope.selectedBulkType = {};
    $scope.GetCategoryList = function (item) {

        $scope.CategoryList = [];
        $scope.selectedBulkType.Name = item;
        $scope.EmployeeVisitIds = $scope.SelectedEmployeeVisitIds.toString();

        if (item == "MarkAsComplete") {
            $scope.MarkAsCompleteEmployeeVisit($scope.EmployeeVisitIds);
        }
        else if (item == "DeleteSelected") {
            $scope.DeleteEmployeeVisit();
        }
        else if (item == "PrintTimesheets") {
            $scope.MultiplePDFDownload();
        }
        else if (item == "ApproveSelected") {
            $scope.OpenVisitApprovalModal();
        }
        else if (item) {
            var jsonData = { Category: item, EmployeeVisitIDList: $scope.SelectedEmployeeVisitIds.toString() };
            AngularAjaxCall($http, HomeCareSiteUrl.CategoriesListURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
               
                if (response.IsSuccess) {
                    $scope.CategoryList = response.Data;
                }
                if (item == "AuthorizationCode") {
                    $scope.EmployeeVisit.AuthorizationCode = 'Auth Code';
                    $scope.PRcategory = true;
                }
                else {
                    $scope.PRcategory = false;
                }   
            });
        }
    }

    $scope.selectedBulkType = {};
    $scope.SelectCategoryValue = function (item) {
        $scope.selectedBulkType.Category = item.Value;
        $scope.EmployeeVisit.AuthorizationCode = item.Name;
    }
    $scope.BulkUpdateDetail = function (selectedBulkType) {

        if (selectedBulkType.Name == undefined || selectedBulkType.Name == null) {
            ShowMessage("Please select Bulk type/Category.","error",4000);
            return;
        }
        var name = "";
        if (selectedBulkType.Name != undefined && selectedBulkType.Name != null &&
            (selectedBulkType.Name == "CareType" || selectedBulkType.Name == "AuthorizationCode" ||
            selectedBulkType.Name =="Payor")) {
            angular.forEach($scope.EmployeeVisitList, function (data) {
                angular.forEach($scope.SelectedEmployeeVisitIds, function (dataId) {
                    if (data.EmployeeVisitID == dataId) {
                        if (name == "")
                            name = data.PatientName;
                        if (name != "" && name != data.PatientName) {
                            ShowMessage("Bulk update operation cannot be performed.<br/>Hint: Select multiple timesheets for a single patient to bulk update Care type, Payor or Prior Authorization", "error", 5000);
                           
                            return;
                        }
                    }
                });
            });
        }
        
        if ($scope.SelectedEmployeeVisitIds.length > 0 && $scope.selectedBulkType) {
            var jsonData = { BulkType: $scope.selectedBulkType.Name, EmployeeVisitIDList: $scope.SelectedEmployeeVisitIds.toString(), Catrgory: $scope.selectedBulkType.Category };
            AngularAjaxCall($http, HomeCareSiteUrl.BulkUpdateVisitReportURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.Refresh();
                    $scope.CategoryList = [];
                    $scope.selectedBulkType = {};
                }
            });
        }

    };
    $scope.PrintTimeSheetReport = function (EmployeeVisitID) {
        var Domain = window.Domain;
        var ReportName = 'PCA TimeSheet Report';
        var ReportURL1 = 'https://';
        //var ReportURL2 = ':51285';
        var ReportURL2 = '.myezcare.com';
        var ReportURL3 = "/Report/Template?ReportName=";
        var ReportURL4 = "&EmployeeVisitID=";
        var parameterValue1 = EmployeeVisitID;
        var ReportURL5 = "&TaskType=";
        var parameterValue2 = 'Task';
        var ReportURL6 = "&ConclusionType=";
        var parameterValue3 = 'Conclusion';

        var url = ReportURL1 + Domain + ReportURL2 + ReportURL3 + ReportName + ReportURL4 + parameterValue1 + ReportURL5 + parameterValue2 + ReportURL6 + parameterValue3;
        var width = screen.availWidth - 10;
        var height = screen.availHeight - 60;
        var left = 0;//(screen.availWidth - width) / 2;
        var top = 0;//(screen.availHeight - height) / 2;
        var params = 'width=' + width + ', height=' + height;
        params += ', top=' + top + ', left=' + left;
        var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,' + params;
        var pdfWindow = window.open('about:blank', 'null', winFeature);
        pdfWindow.document.write("<html><head><style> * { box-sizing: border-box; padding: 0; margin: 0; border: 0; }</style>"
            + "<title>" + ReportName + "</title></head><body>"
            + '<embed width="100%" height="100%" name="plugin" src="' + url + '" '
            + 'type="application/pdf" internalinstanceid="21"></body></html>');
        pdfWindow.document.close();

    };
    $scope.PrioAuthorizationList = [];
    $scope.GetPriorAuthDetails = function (EmployeeVisit) {
        if (EmployeeVisit.AuthorizationCode != null) {
            var jsonData = { ReferralID: EmployeeVisit.ReferralID, BillingAuthorizationID: EmployeeVisit.ReferralBillingAuthorizationID };
            AngularAjaxCall($http, HomeCareSiteUrl.PrioAuthorizationURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.PrioAuthorizationList = response.Data;
                }

            });
        }
        //$('#PriorAuth').modal({
        //    backdrop: 'static',
        //    keyboard: false
        //});
    }
    $scope.PrintTimeSheetReportHomecare = function (EmployeeVisitID) {
        var Domain = window.Domain;
        var ReportName = 'TimeSheet';
        var ReportURL1 = 'https://';
        //var ReportURL2 = ':51285';
        var ReportURL2 = '.myezcare.com';
        var ReportURL3 = "/Report/Template?ReportName=";
        var ReportURL4 = "&EmployeeVisitID=";
        var parameterValue1 = EmployeeVisitID;
        var ReportURL5 = "&TaskType=";
        var parameterValue2 = 'Task';
        var ReportURL6 = "&ConclusionType=";
        var parameterValue3 = 'Conclusion';



        var url = ReportURL1 + Domain + ReportURL2 + ReportURL3 + ReportName + ReportURL4 + parameterValue1 + ReportURL5 + parameterValue2 + ReportURL6 + parameterValue3;
        var width = screen.availWidth - 0;
        var height = screen.availHeight - 60;
        var left = 0;//(screen.availWidth - width) / 2;
        var top = 0;//(screen.availHeight - height) / 2;
        var params = 'width=' + width + ', height=' + height;
        params += ', top=' + top + ', left=' + left;
        var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,' + params;
        var pdfWindow = window.open('about:blank', 'null', winFeature);
        pdfWindow.document.write("<html><head><style> * { box-sizing: border-box; padding: 0; margin: 0; border: 0; }</style>"
            + "<title>" + ReportName + "</title></head><body>"
            + '<embed width="100%" height="100%" name="plugin" src="' + url + '" '
            + 'type="application/pdf" internalinstanceid="21"></body></html>');
        pdfWindow.document.close();



    };

    $scope.PrintTimeSheetReportDaycare = function (EmployeeVisitID) {
        var Domain = window.Domain;
        var ReportName = 'TimeSheet-Daycare';
        var ReportURL1 = 'https://';
        //var ReportURL2 = ':51285';
        var ReportURL2 = '.myezcare.com';
        var ReportURL3 = "/Report/Template?ReportName=";
        var ReportURL4 = "&EmployeeVisitID=";
        var parameterValue1 = EmployeeVisitID;
        var ReportURL5 = "&TaskType=";
        var parameterValue2 = 'Task';
        var ReportURL6 = "&ConclusionType=";
        var parameterValue3 = 'Conclusion';



        var url = ReportURL1 + Domain + ReportURL2 + ReportURL3 + ReportName + ReportURL4 + parameterValue1 + ReportURL5 + parameterValue2 + ReportURL6 + parameterValue3;
        var width = screen.availWidth - 10;
        var height = screen.availHeight - 60;
        var left = 0;//(screen.availWidth - width) / 2;
        var top = 0;//(screen.availHeight - height) / 2;
        var params = 'width=' + width + ', height=' + height;
        params += ', top=' + top + ', left=' + left;
        var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,' + params;
        var pdfWindow = window.open('about:blank', 'null', winFeature);
        pdfWindow.document.write("<html><head><style> * { box-sizing: border-box; padding: 0; margin: 0; border: 0; }</style>"
            + "<title>" + ReportName + "</title></head><body>"
            + '<embed width="100%" height="100%" name="plugin" src="' + url + '" '
            + 'type="application/pdf" internalinstanceid="21"></body></html>');
        pdfWindow.document.close();



    };
    $scope.PrintBulkTimeSheetReportHomecare = function () {
        $scope.ListOfIdsInCsv = $scope.SelectedEmployeeVisitIds.toString();
        var Domain = window.Domain;
        var ReportName = 'TimeSheet-Bulk';
        var ReportURL1 = 'https://';
        //  var ReportURL2 = ':51285';
        var ReportURL2 = '.myezcare.com';
        var ReportURL3 = "/Report/Template?ReportName=";
        var ReportURL4 = "&EmployeeVisitID=";
        var parameterValue1 = $scope.ListOfIdsInCsv;
        var ReportURL5 = "&TaskType=";
        var parameterValue2 = 'Task';
        var ReportURL6 = "&ConclusionType=";
        var parameterValue3 = 'Conclusion';



        var url = ReportURL1 + Domain + ReportURL2 + ReportURL3 + ReportName + ReportURL4 + parameterValue1 + ReportURL5 + parameterValue2 + ReportURL6 + parameterValue3;
        var width = screen.availWidth - 0;
        var height = screen.availHeight - 60;
        var left = 0;//(screen.availWidth - width) / 2;
        var top = 0;//(screen.availHeight - height) / 2;
        var params = 'width=' + width + ', height=' + height;
        params += ', top=' + top + ', left=' + left;
        var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,' + params;
        var pdfWindow = window.open('about:blank', 'null', winFeature);
        pdfWindow.document.write("<html><head><style> * { box-sizing: border-box; padding: 0; margin: 0; border: 0; }</style>"
            + "<title>" + ReportName + "</title></head><body>"
            + '<embed width="100%" height="100%" name="plugin" src="' + url + '" '
            + 'type="application/pdf" internalinstanceid="21"></body></html>');
        pdfWindow.document.close();



    };
};

controllers.EmployeeVisitListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    var dateformat = GetOrgDateFormat();
    $(".dateInputMask").attr("placeholder", dateformat);
    $('.time').inputmask({
        mask: "h:s t\\m",
        placeholder: "hh:mm a",
        alias: "datetime",
        hourFormat: "12"
    });

    var eventMethod = window.addEventListener ? "addEventListener" : "attachEvent";
    var eventer = window[eventMethod];
    var messageEvent = eventMethod == "attachEvent" ? "onmessage" : "message";
    // Listen to message from child window
    eventer(messageEvent, function (e) {

        if (typeof (e.data) === 'string' && e.data.indexOf('OrbeonID') >= 0) {
            vm.SaveVisitNoteOrbeonForm(e.data);
        }

    }, false);
    if ($('#emp-visit-start-date').data("DateTimePicker")) {
        $('#emp-visit-start-date').data("DateTimePicker").widgetPositioning({
            horizontal: "right",
            vertical: "bottom"
        });
        if ($('#start-visit-end-date') && $('#start-visit-end-date').data("DateTimePicker")) {
            $('#start-visit-end-date').data("DateTimePicker").widgetPositioning({
                horizontal: "right",
                vertical: "bottom"
            });
        }
    }

});



$("#right-scroll-button").click(function (event) {
    event.preventDefault();
    $(".table-responsive").animate(
        {
            scrollLeft: "+=300px"
        },
        "slow"
    );
});

$("#left-scroll-button").click(function (event) {
    event.preventDefault();
    $(".table-responsive").animate(
        {
            scrollLeft: "-=300px"
        },
        "slow"
    );
});
