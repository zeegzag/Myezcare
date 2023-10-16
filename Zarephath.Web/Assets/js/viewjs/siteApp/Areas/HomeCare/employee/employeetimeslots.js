var custModel1;

controllers.AddEmployeeTimeSlotsController = function ($scope, $http) {

    custModel1 = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnETSModel").val());
    };
    $scope.ETSModel = $scope.newInstance();
    $scope.ETSMaster = $scope.newInstance().ETSMaster;
    $scope.ETSDetail = $scope.newInstance().ETSDetail;
    //#region ETS Master Related Code

    //#region ETS Master Paging 
    $scope.ETSMasterList = [];
    $scope.ETSMasterListPager = new PagerModule("EmployeeTimeSlotMasterID");
    $scope.SearchETSMaster = $scope.ETSModel.SearchETSMaster;
    $scope.TempSearchETSMaster = $scope.ETSModel.SearchETSMaster;
    $scope.SelectedDays = [];

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchETSMaster: $scope.SearchETSMaster,
            pageSize: $scope.ETSMasterListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ETSMasterListPager.sortIndex,
            sortDirection: $scope.ETSMasterListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };
    $scope.SearchModelMapping = function () {
        $scope.SearchETSMaster = $.parseJSON(angular.toJson($scope.TempSearchETSMaster));
    };

    $scope.GetETSMasterList = function (isSearchDataMappingRequire) {
        $scope.ResetETSDetail();

        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        var jsonData = $scope.SetPostData($scope.ETSMasterListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetETSMasterListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                
                $scope.ETSMasterList = response.Data.Items;
                
                $scope.ETSMasterListPager.currentPageSize = response.Data.Items.length;
                $scope.ETSMasterListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.ETSMasterListPager.getDataCallback = $scope.GetETSMasterList;
    //$scope.ETSMasterListPager.getDataCallback();

    $scope.Refresh = function () {
        $scope.ETSMasterListPager.getDataCallback();
    };

    $('#etsMasterModel').on('hidden.bs.modal', function () {
        $scope.ETSMasterListPager.getDataCallback();
        //$scope.callCronJob();
    });

    $scope.callCronJob = function () {
        AngularAjaxCall($http, "/hc/cronjob/generateemployeetimeschedule", null, "get", "json", "application/json").success(function (response) {
            //if (response.IsSuccess) {
            //    $scope.ETSMasterList = response.Data.Items;
            //    $scope.ETSMasterListPager.currentPageSize = response.Data.Items.length;
            //    $scope.ETSMasterListPager.totalRecords = response.Data.TotalItems;
            //}
            //ShowMessages(response);
        });
    }

    $scope.SearchETSMasterList = function () {
        $scope.ETSMasterListPager.currentPage = 1;
        $scope.ETSMasterListPager.getDataCallback(true);
    };
    $scope.DeleteETSMaster = function ($event, etsMasterId, title) {
        $event.stopPropagation();
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchETSMaster.ListOfIdsInCsv = etsMasterId;

                //Reset Selcted Checkbox items and Control
                $scope.SelectedEmployeeIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.ETSMasterListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteETSMasterURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        
                        $scope.ETSMasterList = response.Data.Items;
                        $scope.ETSMasterListPager.currentPageSize = response.Data.Items.length;
                        $scope.ETSMasterListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };
    $scope.RefreshETSMaster = function () {
        $scope.ETSMasterListPager.getDataCallback();
    }
    //#endregion

    $scope.resetEndDate = function () {
        $scope.ETSMaster.EndDate = null;
    }

    $scope.OpenEtsMasterModal = function ($event, item) {
        $event.stopPropagation();
        $scope.ETSMaster = $scope.newInstance().ETSMaster;
        $scope.ETSDetail = $scope.newInstance().ETSDetail;
        if (item == undefined) {
            $scope.ETSMaster.EmployeeID = $scope.TempSearchETSMaster.EmployeeID;
            $scope.ETSDetail.EmployeeID = $scope.TempSearchETSMaster.EmployeeID;
        } else {
            $scope.ETSMaster.EmployeeID = item.EmployeeID.toString();
            if (item.EmployeeTimeSlotMasterID > 0) {
                $scope.ETSMaster.EmployeeTimeSlotMasterID = item.EmployeeTimeSlotMasterID;
                $scope.ETSMaster.StartDate = moment(item.StartDate);
                $scope.ETSMaster.EndDate = moment(item.EndDate);
                $scope.ETSMaster.IsEndDateAvailable = item.IsEndDateAvailable;
                $scope.ETSDetail.EmployeeTimeSlotMasterID = item.EmployeeTimeSlotMasterID;
                $scope.SearchETSDetail.EmployeeTimeSlotMasterID = item.EmployeeTimeSlotMasterID;
                $scope.ETSDetail.EmployeeID = item.EmployeeID;
                if ($scope.SearchETSDetail.EmployeeTimeSlotMasterID > 0)
                    $scope.GetETSDetailList();
            }
            $scope.SearchETSDetail.EmployeeTimeSlotMasterID = item.EmployeeTimeSlotMasterID;
        }

        $scope.SetTime($scope.ETSDetail);
        $scope.SelectedDays = [];
        $scope.LastEndDate = moment($scope.ETSMaster.EndDate).toDate();
        $('#etsMasterModel').modal('show');
    }

    $scope.SaveETSMaster = function () {
        var isValid = CheckErrors($("#frmETSMaster"));
        if (isValid) {
            if ($scope.LastEndDate != null && $scope.LastEndDate > moment($scope.ETSMaster.EndDate).toDate()) {
                bootboxDialog(function (result) {
                    if (result) {
                        $scope.SaveETSMain();
                    }
                }, bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, window.EmpFutureSchDeleteConfirmationMaster, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
            } else {
                $scope.SaveETSMain();
            }
        }
    };

    $scope.UpdateETSDetail = function () {
  
        if (CheckErrors($("#frmETSDetail"))) {

            $scope.SearchETSDetail.ListOfIdsInCsv = $scope.ETSDetail.EmployeeTimeSlotDetailID;
            $scope.ETSDetail.EmployeeID = $scope.TempSearchETSMaster.EmployeeID;
            var jsonData = angular.toJson({
                etsDetail: $scope.ETSDetail,
                searchETSDetail: $scope.SearchETSDetail,
                pageSize: $scope.ETSDetailListPager.pageSize,
                pageIndex: $scope.ETSDetailListPager.fromIndex,
                sortIndex: $scope.ETSDetailListPager.sortIndex,
                sortDirection: $scope.ETSDetailListPager.sortDirection
            });
            var tempJsonData = jsonData;
            AngularAjaxCall($http, HomeCareSiteUrl.UpdateETSDetailURL, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        $scope.ETSDetailListPager.getDataCallback();
                        $scope.ResetETSDetailModel();
                    }
                    if (response.Data == -3) {
                        bootboxDialog(function (result) {
                            if (result) {
                                jsonData = tempJsonData;
                                AngularAjaxCall($http, HomeCareSiteUrl.EmployeeTimeSlotForceUpdateURL, jsonData, "Post", "json", "application/json").success(function (response) {
                                    if (response.IsSuccess) {
                                        $scope.ETSDetailListPager.getDataCallback();
                                        $scope.ResetETSDetailModel();
                                    }
                                    ShowMessages(response);
                                });
                            }
                        }, bootboxDialogType.Confirm, "Force Update", window.TimeSlotForceUpdateConfirmation, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
                    }
                    ShowMessages(response);
                });
        }
    };

    $scope.SaveETSMain = function () {
        var isValid = CheckErrors($("#frmETSMaster"));
        if (isValid) {
            var jsonData = angular.toJson({
                etsMaster: $scope.ETSMaster
            });

            AngularAjaxCall($http, HomeCareSiteUrl.AddEtsMasterURL, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        $scope.ETSMasterListPager.getDataCallback();
                        $scope.ETSMaster.EmployeeTimeSlotMasterID = response.Data;
                        $scope.ETSDetail.EmployeeTimeSlotMasterID = response.Data;
                        $scope.SearchETSDetail.EmployeeTimeSlotMasterID = response.Data;
                        $scope.ETSDetailListPager.getDataCallback();
                        //$('#etsMasterModel').modal('hide');
                    }
                    ShowMessages(response);
                });
        }
    };
    //#endregion
    //

    //#region ETS Details Related Code

    $scope.ETSMasterClick = function (item) {
        $scope.SearchETSDetail.EmployeeTimeSlotMasterID = item.EmployeeTimeSlotMasterID;
        if ($scope.SearchETSDetail.EmployeeTimeSlotMasterID > 0)
            $scope.GetETSDetailList();
    };


    //#region ETS Details Paging 
    $scope.ETSDetailList = [];
    $scope.ETSDetailListPager = new PagerModule("EmployeeTimeSlotMasterID");
    $scope.SearchETSDetail = $scope.newInstance().SearchETSDetail;
    $scope.TempSearchETSDetail = $scope.newInstance().SearchETSDetail;


    $scope.SetPostData01 = function (fromIndex) {
        var pagermodel = {
            SearchETSDetail: $scope.SearchETSDetail,
            pageSize: $scope.ETSDetailListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ETSDetailListPager.sortIndex,
            sortDirection: $scope.ETSDetailListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };
    $scope.SearchModelMapping01 = function () {
        $scope.SearchETSDetail = $.parseJSON(angular.toJson($scope.TempSearchETSDetail));
    };


    $scope.GetETSDetailList = function (isSearchDataMappingRequire) {

        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping01();

        var jsonData = $scope.SetPostData01($scope.ETSDetailListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetETSDetailListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.ETSDetailList = response.Data.Items;
                $scope.ETSDetailListPager.currentPageSize = response.Data.Items.length;
                $scope.ETSDetailListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.ETSDetailListPager.getDataCallback = $scope.GetETSDetailList;


    $scope.SearchETSDetailList = function () {
        $scope.ETSDetailListPager.currentPage = 1;
        $scope.ETSDetailListPager.getDataCallback(true);
    };

    $scope.AlertOnDelete = function () {
        bootboxDialog(null, bootboxDialogType.Alert, "Alert", "Coming soon - work in progress");
    }


    $scope.DeleteETSDetail = function ($event, etsMasterId, title) {
        $event.stopPropagation();
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchETSDetail.ListOfIdsInCsv = etsMasterId;
                //Reset Selcted Checkbox items and Control
                $scope.SelectedEmployeeIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData01($scope.ETSDetailListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteETSDetailURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.ETSDetailList = response.Data.Items;
                        $scope.ETSDetailListPager.currentPageSize = response.Data.Items.length;
                        $scope.ETSDetailListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EmpFutureSchDeleteConfirmation, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.ResetETSDetail = function () {
        $scope.ETSDetailList = [];
        $scope.SearchETSDetail = $scope.newInstance().SearchETSDetail;
        $scope.TempSearchETSDetail = $scope.newInstance().SearchETSDetail;
    }

    $scope.ResetETSDetailModel = function () {
        $scope.ETSDetail = $scope.newInstance().ETSDetail;
        HideErrors($("#frmETSDetail"))
        $scope.ETSDetail.EmployeeTimeSlotMasterID = $scope.ETSMaster.EmployeeTimeSlotMasterID;
        $scope.SetTime($scope.ETSDetail);
    }

    $scope.SetTime = function (model, data, call_index) {
        data = data || {};
        call_index = call_index || 0;
        if (call_index == 0) {
            model.Is24Hrs = false;
            $scope.Set24Hrs(model, data, 1);
        }
        if (model.AllDay) {
            $scope.ETSDetail.StrStartTime = "12:00 am";
            $scope.ETSDetail.StrEndTime = "11:59 pm";
            $("#ETSDetail_StrStartTime").attr('readonly', 'true');
            $("#ETSDetail_StrEndTime").attr('readonly', 'true');
        } else {
            $scope.ETSDetail.StrStartTime = data.StrStartTime || "";
            $scope.ETSDetail.StrEndTime = data.StrEndTime || "";
            $("#ETSDetail_StrStartTime").removeAttr('readonly', 'true');
            $("#ETSDetail_StrEndTime").removeAttr('readonly', 'true');
        }
    }

    $scope.Set24Hrs = function (model, data, call_index) {
        data = data || {};
        call_index = call_index || 0;
        if (call_index == 0) {
            model.AllDay = false;
            $scope.SetTime(model, data, 1);
        }
        if (model.Is24Hrs) {
            $scope.ETSDetail.StrStartTime = "";
            $scope.ETSDetail.StrEndTime = "";
            $("#ETSDetail_StrStartTime").attr('readonly', 'true');
            $("#ETSDetail_StrEndTime").attr('readonly', 'true');
        } else {
            $scope.ETSDetail.StrStartTime = data.StrStartTime || "";
            $scope.ETSDetail.StrEndTime = data.StrEndTime || "";
            $("#ETSDetail_StrStartTime").removeAttr('readonly', 'true');
            $("#ETSDetail_StrEndTime").removeAttr('readonly', 'true');
        }
    }

    //#endregion

    $scope.OpenEtsDetailModal = function ($event, item) {
        $event.stopPropagation();
        $scope.ETSDetail = $scope.newInstance().ETSDetail;
        HideErrors($("#frmETSDetail"))
        if (item == undefined) {
            $scope.ETSDetail.EmployeeTimeSlotMasterID = $scope.SearchETSDetail.EmployeeTimeSlotMasterID;
        } else {
            $scope.ETSDetail.EmployeeTimeSlotMasterID = $scope.SearchETSDetail.EmployeeTimeSlotMasterID;
            if (item.EmployeeTimeSlotDetailID > 0) {
                $scope.ETSDetail.EmployeeTimeSlotDetailID = item.EmployeeTimeSlotDetailID;
                $scope.ETSDetail.EmployeeTimeSlotMasterID = item.EmployeeTimeSlotMasterID;
                $scope.ETSDetail.Day = item.Day;
                $scope.ETSDetail.AllDay = item.AllDay;
                $scope.ETSDetail.Is24Hrs = item.Is24Hrs;
                $scope.ETSDetail.StartTime = item.StartTime;
                $scope.ETSDetail.EndTime = item.EndTime;
                $scope.ETSDetail.Notes = item.Notes;
            }
        }
        if ($scope.ETSDetail.Is24Hrs) {
            $scope.Set24Hrs($scope.ETSDetail, item);
        } else {
            $scope.SetTime($scope.ETSDetail, item);
        }
        //$('#etsDetailModal').modal('show');
    };

    $scope.TempETSDetail = {};
    $scope.SaveETSDetail = function () {
        HideErrors($("#frmETSDetail"))
        if ($('#btnSaveETS').css('display') == "none") {
            return;
        }

        if ($scope.SelectedDays) {
            var selectedDays = $scope.SelectedDays;
            $scope.ETSDetail.SelectedDays = Object.keys(selectedDays).map(function (k) { return selectedDays[k]["Value"] }).join(",");
        }
        else {
            $scope.ETSDetail.SelectedDays = null;
        }

        if (CheckErrors($("#frmETSDetail"))) {
            $scope.TempETSDetail = $scope.ETSDetail;
            $scope.ETSDetail.EmployeeID = $scope.TempSearchETSMaster.EmployeeID;
            $scope.ETSDetail.SelectedDays = $scope.TempETSDetail.SelectedDays.split(',');
            var jsonData = angular.toJson({
                etsDetail: $scope.ETSDetail
            });

            AngularAjaxCall($http, HomeCareSiteUrl.AddEtsDetailURL, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        $scope.ETSDetailListPager.getDataCallback();
                        $scope.ETSDetail = $scope.newInstance().ETSDetail;
                        $scope.ETSDetail.EmployeeTimeSlotMasterID = $scope.ETSMaster.EmployeeTimeSlotMasterID;
                    }
                    if (response.Data == -3) {
                        bootboxDialog(function (result) {
                            if (result) {
                                jsonData = angular.toJson({
                                    etsDetail: $scope.TempETSDetail
                                });
                                AngularAjaxCall($http, HomeCareSiteUrl.EmployeeTimeSlotForceUpdateURL, jsonData, "Post", "json", "application/json").success(function (response) {
                                    if (response.IsSuccess) {
                                        $scope.ETSDetailListPager.getDataCallback();
                                        $scope.ETSDetail = $scope.newInstance().ETSDetail;
                                        $scope.ETSDetail.EmployeeTimeSlotMasterID = $scope.ETSMaster.EmployeeTimeSlotMasterID;
                                    }
                                    ShowMessages(response);
                                });
                            }
                        }, bootboxDialogType.Confirm, "Force Update", window.TimeSlotForceUpdateConfirmation, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
                    }

                    $scope.SelectedDays = [];
                    $("#ETSDetail_StrStartTime").removeAttr('readonly');
                    $("#ETSDetail_StrEndTime").removeAttr('readonly');
                    $("#checkalldays").prop("checked", false);

                    ShowMessages(response);
                });
        }
    };



    //#endregion


    $scope.SelectDaySettings = {
        addEditItem: false,
        smartButtonMaxItems: 1
    };

    if ($scope.ETSModel.IsPartial) {
        $scope.SearchETSMasterList();
    }

    $scope.nextOption = function () {
        var length = $scope.ETSModel.EmployeeList.length;
        var index = $scope.ETSModel.EmployeeList.findIndex(a => a.Value.toString() == $scope.TempSearchETSMaster.EmployeeID);
        if (index + 1 < length) {
            var next = $scope.ETSModel.EmployeeList[index + 1].Value;
            $scope.TempSearchETSMaster.EmployeeID = next.toString();
            $scope.SearchETSMasterList();
        }

    }

    $scope.prevOption = function () {
        var length = $scope.ETSModel.EmployeeList.length;
        var index = $scope.ETSModel.EmployeeList.findIndex(a => a.Value.toString() == $scope.TempSearchETSMaster.EmployeeID);
        if (index != -1) {
            var next = $scope.ETSModel.EmployeeList[index - 1].Value;
            $scope.TempSearchETSMaster.EmployeeID = next.toString();
            $scope.SearchETSMasterList();
        }
    }

    //$("#next").click(function () {
    //    var nextElement = $('#ets_emp > option:selected').next('option');
    //    if (nextElement.length > 0) {
    //        $('#ets_emp > option:selected').removeAttr('selected').next('option').attr('selected', 'selected');
    //        $scope.TempSearchETSMaster.EmployeeID = $('#ets_emp').val();
    //        //$scope.SearchETSMasterList();
    //    }
    //});

    //$("#prev").click(function () {
    //    var nextElement = $('#ets_emp > option:selected').prev('option');
    //    if (nextElement.length > 0) {
    //        $('#ets_emp > option:selected').removeAttr('selected').prev('option').attr('selected', 'selected');
    //        $scope.TempSearchETSMaster.EmployeeID = $('#ets_emp').val();
    //        //$scope.SearchETSMasterList();
    //    }
    //});


    $scope.BulkSchedule = function () {
        $scope.ETSDetail.EmployeeIDs = $scope.SelectedEmployeeIds.toString();
        if (ValideElement($scope.ETSDetail.EmployeeIDs)) {
            if (CheckErrors($("#frmBulkETSDetail"))) {
                $scope.TempETSDetail = $scope.ETSDetail;
                var jsonData = angular.toJson({
                    bulkEtsDetail: $scope.ETSDetail
                });
                AngularAjaxCall($http, HomeCareSiteUrl.AddEtsDetailBulkURL, jsonData, "post", "json", "application/json", true).
                    success(function (response) {
                        if (response.IsSuccess) {
                            $('#BulkScheduleModel').modal('hide');
                        }
                        ShowMessages(response);
                    });
            }
        } else {
            toastr.error("Please select atleast one employee.");
        }
    }


};

controllers.AddEmployeeTimeSlotsController.$inject = ['$scope', '$http'];



$(document).ready(function () {
    $('.time').inputmask({
        mask: "h:s t\\m",
        placeholder: "hh:mm a",
        alias: "datetime",
        hourFormat: "12"
    });
});