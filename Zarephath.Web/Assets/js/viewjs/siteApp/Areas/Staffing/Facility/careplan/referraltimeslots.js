var custModel1;

controllers.AddReferralTimeSlotsController = function ($scope, $http) {
    custModel1 = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnRTSModel").val());
    };
    $scope.RTSModel = $scope.newInstance();
    $scope.RTSMaster = $scope.newInstance().RTSMaster;
    $scope.RTSDetail = $scope.newInstance().RTSDetail;
    
    $scope.patientStatusFilters = ['All', 'Expired', 'Active']
    $scope.selectedPatientStatusFilter = 'Active'
    $scope.patientStatusFilter = ['Active','Expired', 'Delete']
    $scope.selectedPatientStatusFilter = 'Active'
    //#region RTS Master Related Code



    //#region RTS Master Paging 
    $scope.RTSMasterList = [];
    $scope.RTSMasterListTemp = [];
    $scope.RTSMasterListPager = new PagerModule("ReferralTimeSlotMasterID");
    $scope.SearchRTSMaster = $scope.RTSModel.SearchRTSMaster;
    $scope.TempSearchRTSMaster = $scope.RTSModel.SearchRTSMaster;

    //#region Employee List For Scheduling
    $scope.SchEmployeeDetailUrl = HomeCareSiteUrl.GetSchEmployeeDetailForPopupURL;
    $scope.SearchSchEmployeeModel = { EmployeeID: 0, StartDate: null, EndDate: null };

    $scope.FilterPatientStatus = function () {
        $scope.TempSearchRTSMaster.Filter = $scope.selectedPatientStatusFilter;
        $scope.GetRTSMasterList(true);
    }

    $scope.SearchPatient = function (data) {

        $scope.ScheduleSearchModel.ReferralName = data.PatientName;
        //$scope.SeachAndGenerateCalenders();
        $.each($('a.webpop'), function (index, ele) {
            $(ele).webuiPopover('hide');
        });
    };

    //$scope.IsMasterChange = function () {
    //    return ($scope.RTSMaster.ReferralTimeSlotMasterID > 0 &&
    //        (!isSameDate($scope.OrgRTSMaster.StartDate, $scope.RTSMaster.StartDate) ||
    //            $scope.OrgRTSMaster.IsEndDateAvailable !== $scope.RTSMaster.IsEndDateAvailable ||
    //            ($scope.RTSMaster.IsEndDateAvailable &&
    //                !isSameDate($scope.OrgRTSMaster.EndDate, $scope.RTSMaster.EndDate))));
    //};

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchRTSMaster: $scope.SearchRTSMaster,
            pageSize: $scope.RTSMasterListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.RTSMasterListPager.sortIndex,
            sortDirection: $scope.RTSMasterListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };
    $scope.SearchModelMapping = function () {
        $scope.SearchRTSMaster = $.parseJSON(angular.toJson($scope.TempSearchRTSMaster));
    };
   

    $scope.GetRTSMasterList = function (isSearchDataMappingRequire) {
        $scope.ResetRTSDetail();
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        var jsonData = $scope.SetPostData($scope.RTSMasterListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetRTSMasterListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.RTSMasterList = response.Data.Items;//.filter(r => r.ActiveStat);
                $scope.RTSMasterListTemp = response.Data.Items;
                $scope.RTSMasterListPager.currentPageSize = response.Data.Items.length;
                $scope.RTSMasterListPager.totalRecords = response.Data.TotalItems;
               
            }
            ShowMessages(response);
        });
    };

    $scope.RTSMasterListPager.getDataCallback = $scope.GetRTSMasterList;
    //$scope.RTSMasterListPager.getDataCallback();

    $scope.Refresh = function () {
        $scope.RTSMasterListPager.getDataCallback();
    };

    $('#rtsMasterModel, #rtsByPriorAuthModel').on('hidden.bs.modal', function () {
        $scope.RTSMasterListPager.getDataCallback();
        //$scope.callCronJob();
    });

    $scope.callCronJob = function () {
        //AngularAjaxCall($http, "/hc/cronjob/generatepatienttimeschedule", null, "get", "json", "application/json").success(function (response) {
        //    //if (response.IsSuccess) {
        //    //    $scope.ETSMasterList = response.Data.Items;
        //    //    $scope.ETSMasterListPager.currentPageSize = response.Data.Items.length;
        //    //    $scope.ETSMasterListPager.totalRecords = response.Data.TotalItems;
        //    //}
        //    //ShowMessages(response);
        //});


        $scope.DayCareTimeSlotModal = {};
        $scope.DayCareTimeSlotModal.ReferralID = $scope.RTSMaster.ReferralID;
        $scope.DayCareTimeSlotModal.ReferralTimeSlotMasterID = $scope.RTSMaster.ReferralTimeSlotMasterID;
        var jsonData = angular.toJson({ modal: $scope.DayCareTimeSlotModal });
        AngularAjaxCall($http, HomeCareSiteUrl.GeneratePatientTimeSchedule, jsonData, "POST", "json", "application/json").success(function (response) {
            ShowMessages(response);
        });

        //GeneratePatientTimeSchedule(DayCareTimeSlotModal modal)
    }

    $scope.SearchRTSMasterList = function (id) {
        $scope.RTSMasterListPager.currentPage = 1;
        $scope.RTSMasterListPager.getDataCallback(true);
    };
    $scope.DeleteRTSMaster = function ($event, RTSMasterId, title) {
        $event.stopPropagation();
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchRTSMaster.ListOfIdsInCsv = RTSMasterId;

                //Reset Selcted Checkbox items and Control
                $scope.SelectedReferralIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.RTSMasterListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteRTSMasterURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.RTSMasterList = response.Data.Items;
                        $scope.RTSMasterListPager.currentPageSize = response.Data.Items.length;
                        $scope.RTSMasterListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };
    $scope.RefreshRTSMaster = function () {
        $scope.RTSMasterListPager.getDataCallback();
    }
    //#endregion

    $scope.resetEndDate = function () {
        $scope.RTSMaster.EndDate = null;
    }

    $scope.ChangeBillingAuthorization = function (rtsMaster) {
        var filteredAuthorizations = $.grep($scope.ReferralBillingAuthorizations, function (item) {
            return item.ReferralBillingAuthorizationID == $scope.RTSMaster.ReferralBillingAuthorizationID;
        });
        if (filteredAuthorizations.length > 0) {
            var selectedAuthorization = filteredAuthorizations[0];
            $scope.RTSMaster.StartDate = moment(selectedAuthorization.StartDate);
            $scope.RTSMaster.EndDate = moment(selectedAuthorization.EndDate);
            $scope.RTSMaster.IsEndDateAvailable = selectedAuthorization.IsEndDateAvailable;
            $scope.LastEndDate = moment($scope.RTSMaster.EndDate).toDate;
        }
    }

    $scope.OpenRTSMasterModal = function ($event, item) {
        debugger
        $event.stopPropagation();
        if (window.isCaseManagement != undefined && window.isCaseManagement == "1") {
            $scope.RTSMaster = $scope.newInstance().RTSMaster;
            $scope.RTSMaster.StartDate = new Date();
            $scope.RTSMaster.ReferralID = $scope.TempSearchRTSMaster.ReferralID;
            var jsonData = angular.toJson({
                rtsMaster: $scope.RTSMaster
            });
            AngularAjaxCall($http, HomeCareSiteUrl.GetReferralAuthorizationsByReferralIDsUrl, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        $scope.ReferralBillingAuthorizations = response.Data;
                        if (response.Data != undefined && response.Data.length > 0) {
                            if (item == undefined) {
                                var firstItem = response.Data[0];
                                $scope.RTSMaster.ReferralBillingAuthorizationID = firstItem.ReferralBillingAuthorizationID;
                                $scope.RTSMaster.StartDate = moment(firstItem.StartDate);
                                $scope.RTSMaster.EndDate = moment(firstItem.EndDate);
                                $scope.RTSMaster.IsEndDateAvailable = firstItem.IsEndDateAvailable;
                                $scope.LastEndDate = moment($scope.RTSMaster.EndDate).toDate;
                            } else {
                                $scope.RTSMaster.ReferralID = item.ReferralID.toString();
                                if (item.ReferralTimeSlotMasterID > 0) {
                                    $scope.RTSMaster.ReferralTimeSlotMasterID = item.ReferralTimeSlotMasterID;
                                    $scope.RTSMaster.ReferralBillingAuthorizationID = item.ReferralBillingAuthorizationID;
                                    $scope.RTSMaster.StartDate = moment(item.StartDate);
                                    $scope.RTSMaster.EndDate = moment(item.EndDate);
                                    $scope.RTSMaster.IsEndDateAvailable = item.IsEndDateAvailable;
                                    $scope.LastEndDate = moment($scope.RTSMaster.EndDate).toDate;
                                }
                            }

                            $('#rtsByPriorAuthModel').modal({
                                backdrop: 'static',
                                keyboard: false
                            });
                        } else {
                            ShowMessages(response);
                        }
                    } else {
                        ShowMessages(response);
                    }
                });
        } else {
            $scope.RTSMaster = $scope.newInstance().RTSMaster;
            $scope.RTSDetail = $scope.newInstance().RTSDetail;
            if (item == undefined) {
                $scope.RTSMaster.ReferralID = $scope.TempSearchRTSMaster.ReferralID;
                $scope.RTSDetail.ReferralID = $scope.TempSearchRTSMaster.ReferralID;
            } else {
                $scope.RTSMaster.ReferralID = item.ReferralID.toString();
                if (item.ReferralTimeSlotMasterID > 0) {
                    $scope.RTSMaster.ReferralTimeSlotMasterID = item.ReferralTimeSlotMasterID;
                    $scope.RTSMaster.StartDate = moment(item.StartDate);
                    $scope.RTSMaster.EndDate = moment(item.EndDate);
                    $scope.RTSMaster.IsEndDateAvailable = item.IsEndDateAvailable;
                    $scope.RTSMaster.CareTypeId = item.CareTypeId;
                    $scope.RTSDetail.ReferralTimeSlotMasterID = item.ReferralTimeSlotMasterID;
                    $scope.RTSDetail.UsedInScheduling = true;
                    $scope.RTSDetail.CareTypeId = $scope.RTSMaster.CareTypeId;
                    $scope.SearchRTSDetail.ReferralTimeSlotMasterID = item.ReferralTimeSlotMasterID;
                    $scope.SearchRTSDetail.CareTypeId = $scope.RTSDetail.CareTypeId;
                    $scope.GetReferralBillingAuthorizations();
                    $scope.RTSDetail.ReferralID = item.ReferralID;
                    if ($scope.SearchRTSDetail.ReferralTimeSlotMasterID > 0)
                        $scope.GetRTSDetailList();
                }
                $scope.SearchRTSDetail.ReferralTimeSlotMasterID = item.ReferralTimeSlotMasterID;
            }
            $scope.LastEndDate = moment($scope.RTSMaster.EndDate).toDate;
            $scope.SetTime($scope.RTSDetail);
            //$scope.OrgRTSMaster = _.cloneDeep($scope.RTSMaster);
            $('#rtsMasterModel').modal({
                backdrop: 'static',
                keyboard: false
            });
            //$('#rtsMasterModel').modal('show');
        }
    }

    $scope.OpenRTSDeleteModal = function ($event, item) {
        $event.stopPropagation;
        if (item.ReferralTimeSlotMasterID > 0) {
            $scope.ResetRTSDetail();
            $scope.SearchRTSMaster.ListOfIdsInCsv = item.ReferralTimeSlotMasterID;
            var jsonData = $scope.SetPostData($scope.RTSMasterListPager.currentPage);
            bootboxDialog(function (result) {
                if (result) {
                    $scope.DeletePatientRTS(jsonData);
                }
            }, bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, window.RefFutureSchDeleteConfirmationMaster, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
        } 
    }

    $scope.GetReferralBillingAuthorizations = function () {
        var jsonData = angular.toJson({
            rtsMaster: {
                ReferralID: $scope.RTSMaster.ReferralID,
                CareTypeID: $scope.SearchRTSDetail.CareTypeId
            }
        });
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralAuthorizationsByReferralIDUrl, jsonData, "post", "json", "application/json", true).
            success(function (response) {
                if (response.IsSuccess) {
                    $scope.ReferralBillingAuthorizations = response.Data;
                    var RBAId = '';
                    if ($scope.ReferralBillingAuthorizations && $scope.ReferralBillingAuthorizations.length > 0) {
                        var RBA = $scope.ReferralBillingAuthorizations.find(val => val.ReferralBillingAuthorizationID == $scope.RTSDetail.ReferralBillingAuthorizationID);
                        if (RBA) { RBAId = RBA.ReferralBillingAuthorizationID; }
                    }
                    $scope.RTSDetail.ReferralBillingAuthorizationID = RBAId;
                    $scope.OnReferralBillingAuthorizationChange();
                } else {
                    ShowMessages(response);
                }
            });
    }

    $scope.OnCareTypeChange = function () {
        $scope.SearchRTSDetail.CareTypeId = $scope.RTSDetail.CareTypeId;
        $scope.GetReferralBillingAuthorizations();
        if ($scope.SearchRTSDetail.ReferralTimeSlotMasterID > 0)
            $scope.GetRTSDetailList();
    };

    $scope.OnReferralBillingAuthorizationChange = function (item) {
        $scope.RTSDetail.SelectedRBA = $scope.ReferralBillingAuthorizations.find(rba => rba.ReferralBillingAuthorizationID == item.ReferralBillingAuthorizationID);
        $scope.RTSDetail.ReferralBillingAuthorizationID = item.ReferralBillingAuthorizationID;
        $scope.RTSDetail.AuthorizationCode = item.AuthorizationCode;
    };

    $scope.SaveRTSMaster = function () {
        debugger
        var isValid = CheckErrors($("#frmRTSMaster"));
        if (isValid) {
            if ($scope.RTSMaster.ReferralTimeSlotMasterID > 0) {
                bootboxDialog(function (result) {
                    if (result) {
                        $scope.SaveRTSMain();
                    }
                }, bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, window.RefFutureSchDeleteConfirmationMaster, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
            } else {
                $scope.SaveRTSMain();
            }
        }
    };

    $scope.DeletePatientRTS = function (jsonData) {
        AngularAjaxCall($http, HomeCareSiteUrl.DeleteRTSMasterURL, jsonData, "post", "json", "application/json", true).
            success(function (response) {
                if (response.IsSuccess) {
                    $scope.RTSMasterList = response.Data.Items;//.filter(r => r.ActiveStat);
                    $scope.RTSMasterListTemp = response.Data.Items;
                    $scope.RTSMasterListPager.currentPageSize = response.Data.Items.length;
                    $scope.RTSMasterListPager.totalRecords = response.Data.TotalItems;
                }

                ShowMessages(response);
            });
    }

    $scope.SaveRTSMain = function () {
        debugger
        var isValid = CheckErrors($("#frmRTSMaster"));
        if (isValid) {
            var jsonData = angular.toJson({
                rtsMaster: $scope.RTSMaster
            });
            if ($scope.RTSMaster.ReferralBillingAuthorizationID != undefined && $scope.RTSMaster.ReferralBillingAuthorizationID > 0) {
                AngularAjaxCall($http, HomeCareSiteUrl.AddRTSByPriorAuthURL, jsonData, "post", "json", "application/json", true).
                    success(function (response) {
                        if (response.IsSuccess) {
                            $('#rtsByPriorAuthModel').modal('hide');
                        }
                        ShowMessages(response);
                    });
            }
            else {
                AngularAjaxCall($http, HomeCareSiteUrl.AddRTSMasterURL, jsonData, "post", "json", "application/json", true).
                    success(function (response) {
                        if (response.IsSuccess) {
                            $scope.RTSMasterListPager.getDataCallback();
                            $scope.RTSMaster.ReferralTimeSlotMasterID = response.Data;
                            $scope.OrgRTSMaster = _.cloneDeep($scope.RTSMaster);
                            $scope.RTSDetail.ReferralTimeSlotMasterID = response.Data;
                            $scope.RTSDetail.UsedInScheduling = true;
                            $scope.RTSDetail.CareTypeId = $scope.RTSMaster.CareTypeId;
                            $scope.SearchRTSDetail.ReferralTimeSlotMasterID = response.Data;
                            $scope.SearchRTSDetail.CareTypeId = $scope.RTSDetail.CareTypeId;
                            $scope.RTSDetailListPager.getDataCallback();

                            $scope.GetReferralBillingAuthorizations();
                            //$('#RTSMasterModel').modal('hide');
                        }

                        ShowMessages(response);
                    });
            }
        }
    };
    //#endregion


    //#region RTS Details Related Code

    $scope.RTSMasterClick = function (item) {
        $scope.SearchRTSDetail.ReferralTimeSlotMasterID = item.ReferralTimeSlotMasterID;
        $scope.RTSDetail.CareTypeId = $scope.RTSMaster.CareTypeId;
        $scope.SearchRTSDetail.CareTypeId = $scope.RTSDetail.CareTypeId;
        $scope.GetReferralBillingAuthorizations();
        if ($scope.SearchRTSDetail.ReferralTimeSlotMasterID > 0)
            $scope.GetRTSDetailList();
    };


    //#region RTS Details Paging 
    $scope.RTSDetailList = [];
    $scope.RTSDetailListPager = new PagerModule("Day");
    $scope.SearchRTSDetail = $scope.newInstance().SearchRTSDetail;
    $scope.TempSearchRTSDetail = $scope.newInstance().SearchRTSDetail;


    $scope.SetPostData01 = function (fromIndex) {
        var pagermodel = {
            searchRTSDetail: $scope.SearchRTSDetail,
            pageSize: $scope.RTSDetailListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.RTSDetailListPager.sortIndex,
            sortDirection: $scope.RTSDetailListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };
    $scope.SearchModelMapping01 = function () {
        $scope.SearchRTSDetail = $.parseJSON(angular.toJson($scope.TempSearchRTSDetail));
    };


    $scope.GetRTSDetailList = function (isSearchDataMappingRequire) {
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping01();

        var jsonData = $scope.SetPostData01($scope.RTSDetailListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetRTSDetailListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.RTSDetailList = response.Data.Items;
                $scope.RTSDetailListPager.currentPageSize = response.Data.Items.length;
                $scope.RTSDetailListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.allItemsSelected = false;
    $scope.deleteButtonDisable = true;
    $scope.RTSDetailListPager.getDataCallback = $scope.GetRTSDetailList;

    $scope.selectEntity = function () {
        // If any entity is not checked, then uncheck the "allItemsSelected" checkbox
        for (var i = 0; i < $scope.RTSDetailList.length; i++) {
            if ($scope.RTSDetailList[i].isChecked == true) {
                $scope.deleteButtonDisable = false;
                break;
            }
            else {
                $scope.deleteButtonDisable = true;
            }
        }

        for (var i = 0; i < $scope.RTSDetailList.length; i++) {
            if (!$scope.RTSDetailList[i].isChecked) {
                $scope.allItemsSelected = false;
                return;
            }
        }

        //If not the check the "allItemsSelected" checkbox
        $scope.model.allItemsSelected = true;
    };
    $scope.SearchRTSDetailList = function () {
        $scope.RTSDetailListPager.currentPage = 1;
        $scope.RTSDetailListPager.getDataCallback(true);
    };

    $scope.AlertOnDelete = function () {
        bootboxDialog(null, bootboxDialogType.Alert, "Alert", "Coming soon - work in progress");
    }

    $scope.selectAll = function () {
        // Loop through all the entities and set their isChecked property
        for (var i = 0; i < $scope.RTSDetailList.length; i++) {
            $scope.RTSDetailList[i].isChecked = $scope.allItemsSelected;
        }
        for (var i = 0; i < $scope.RTSDetailList.length; i++) {
            if ($scope.RTSDetailList[i].isChecked == true) {
                $scope.deleteButtonDisable = false;
                break;
            }
            else {
                $scope.deleteButtonDisable = true;
            }
        }
    };

    $scope.DeleteRTSDetail = function ($event, title) {
        debugger
        $event.stopPropagation();
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                var rtsMasterIds = "";
                for (var i = 0; i < $scope.RTSDetailList.length; i++) {
                    if ($scope.RTSDetailList[i].isChecked == true)
                        rtsMasterIds = rtsMasterIds + "," + $scope.RTSDetailList[i].ReferralTimeSlotDetailID
                }
                rtsMasterIds = rtsMasterIds.replace(/(^,)|(,$)/g, "");

                $scope.SearchRTSDetail.ListOfIdsInCsv = rtsMasterIds;

                //Reset Selcted Checkbox items and Control
                $scope.SelectedReferralIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData01($scope.RTSDetailListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteRTSDetailURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.RTSDetailList = response.Data.Items;
                        $scope.RTSDetailListPager.currentPageSize = response.Data.Items.length;
                        $scope.RTSDetailListPager.totalRecords = response.Data.TotalItems;
                        $scope.deleteButtonDisable = true;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.RefFutureSchDeleteConfirmation, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    }; 


    $scope.ResetRTSDetail = function () {
        $scope.RTSDetailList = [];
        $scope.SearchRTSDetail = $scope.newInstance().SearchRTSDetail;
        $scope.TempSearchRTSDetail = $scope.newInstance().SearchRTSDetail;
    }

    //#endregion
    $scope.SetTime = function (model) {
        $scope.RTSDetail.AnyTimeClockIn = model.IsChecked;
        if (model.IsChecked) {
            $scope.RTSDetail.StrStartTime = "12:00 am";
            $scope.RTSDetail.StrEndTime = "11:59 pm";
            // $scope.RTSDetail.SelectedDays = ["1", "2", "3", "4", "5", "6", "7"];
            $("#RTSDetail_StrStartTime").attr('readonly', 'true');
            $("#RTSDetail_StrEndTime").attr('readonly', 'true');
        } else {
            $scope.RTSDetail.StrStartTime = $scope.RTSDetail.StrStartTime ? $scope.RTSDetail.StrStartTime : "";
            $scope.RTSDetail.StrEndTime = $scope.RTSDetail.StrEndTime ? $scope.RTSDetail.StrEndTime : "";
            $scope.RTSDetail.SelectedDays = null;
            $("#RTSDetail_StrStartTime").removeAttr('readonly', 'true');
            $("#RTSDetail_StrEndTime").removeAttr('readonly', 'true');
        }
    }

    $scope.OpenRTSDetailModal = function ($event, item) {
        debugger
        $event.stopPropagation();
        $scope.RTSDetail = $scope.newInstance().RTSDetail;
        if (item == undefined) {
            $scope.RTSDetail.ReferralTimeSlotMasterID = $scope.SearchRTSDetail.ReferralTimeSlotMasterID;
        } else {
            $scope.RTSDetail.ReferralTimeSlotMasterID = $scope.SearchRTSDetail.ReferralTimeSlotMasterID;
            if (item.ReferralTimeSlotDetailID > 0) {
                $scope.RTSDetail.ReferralTimeSlotDetailID = item.ReferralTimeSlotDetailID;
                $scope.RTSDetail.ReferralTimeSlotMasterID = item.ReferralTimeSlotMasterID;
                $scope.RTSDetail.Day = item.Day;
                $scope.RTSDetail.StartTime = item.StartTime;
                $scope.RTSDetail.EndTime = item.EndTime;
                $scope.RTSDetail.StrStartTime = item.StrStartTime;
                $scope.RTSDetail.StrEndTime = item.StrEndTime;
                $scope.RTSDetail.UsedInScheduling = item.UsedInScheduling;
                $scope.RTSDetail.Notes = item.Notes;
                $scope.RTSDetail.CareTypeId = (item.CareTypeId == 0 || item.CareTypeId == null) ? '' : item.CareTypeId.toString();
                $scope.RTSDetail.ReferralBillingAuthorizationID = (item.ReferralBillingAuthorizationID == 0 || item.ReferralBillingAuthorizationID == null) ? '' : item.ReferralBillingAuthorizationID.toString();
                $scope.RTSDetail.IsChecked = item.AnyTimeClockIn;
                $scope.RTSDetail.AuthorizationCode = item.AuthorizationCode;
                $scope.SetTime($scope.RTSDetail);
                $scope.OnCareTypeChange();
                $scope.OnReferralBillingAuthorizationChange();
                //if (item.CareTypeId != null) {
                //    $scope.SelectedCareType = item.CareTypeId.toString();
                //} else {
                //    $scope.SelectedCareType = null;
                //}
            }
        }
        //$('#RTSDetailModal').modal('show');
    }

    $scope.RTSMaster.ReferralTimeSlotMasterID;

    $scope.SaveRTSDetail = function () {
        debugger
        if (CheckErrors($("#frmRTSDetail"))) {
            $scope.RTSDetail.StartDate = $scope.RTSMaster.StartDate;
            $scope.RTSDetail.EndDate = $scope.RTSMaster.EndDate;
            $scope.RTSDetail.ReferralID = $scope.TempSearchRTSMaster.ReferralID;
            $scope.RTSDetail.ReferralTimeSlotMasterID = $scope.RTSMaster.ReferralTimeSlotMasterID;
            $scope.TempRTSDetail = $scope.RTSDetail;
            var jsonData = angular.toJson({
                rtsDetail: $scope.RTSDetail
            });
            AngularAjaxCall($http, HomeCareSiteUrl.AddRTSDetailURL, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        $scope.RTSDetailListPager.getDataCallback();
                        $scope.ResetRTSDetailModel();
                    }
                    if (response.Data == -3) {
                        bootboxDialog(function (result) {
                            if (result) {
                                jsonData = angular.toJson({
                                    rtsDetail: $scope.TempRTSDetail
                                });
                                AngularAjaxCall($http, HomeCareSiteUrl.ReferralTimeSlotForceUpdateURL, jsonData, "Post", "json", "application/json").success(function (response) {
                                    if (response.IsSuccess) {
                                        $scope.RTSDetailListPager.getDataCallback();
                                        $scope.ResetRTSDetailModel();
                                        $scope.RTSDetail.ReferralTimeSlotMasterID = $scope.RTSMaster.ReferralTimeSlotMasterID;
                                    }
                                    ShowMessages(response);
                                });
                            }
                        }, bootboxDialogType.Confirm, "Force Update", window.TimeSlotForceUpdateConfirmation, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
                    }
                    else if (response.Data == -4) {
                        bootboxDialog(function (result) {
                            if (result) {
                                $scope.TempRTSDetail.IsForcePatientSchedules = true;
                                jsonData = angular.toJson({
                                    rtsDetail: $scope.TempRTSDetail
                                });
                                AngularAjaxCall($http, HomeCareSiteUrl.AddRTSDetailURL, jsonData, "post", "json", "application/json", true).
                                    success(function (response) {
                                        if (response.IsSuccess) {
                                            $scope.RTSDetailListPager.getDataCallback();
                                            $scope.ResetRTSDetailModel();
                                        }
                                        ShowMessages(response);
                                    });
                            }
                        }, bootboxDialogType.Confirm, "Alert", "The Schedule Time is more than allowed time. Would you still want to schedule? ", bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
                    }
                    ShowMessages(response);
                });
            $("#RTSDetail_StrStartTime").removeAttr('readonly', 'true');
            $("#RTSDetail_StrEndTime").removeAttr('readonly', 'true');
        }
    };


    $scope.UpdateRTSDetail = function () {
        debugger
        if (CheckErrors($("#frmRTSDetail"))) {
            bootboxDialog(function (result) {
                if (result) {
                    // $scope.RTSDetail.CareTypeId = $scope.SelectedCareType.toString();
                    $scope.RTSDetail.StartDate = $scope.RTSMaster.StartDate;
                    $scope.RTSDetail.EndDate = $scope.RTSMaster.EndDate;
                    $scope.RTSDetail.AnyTimeClockIn = $scope.RTSDetail.AnyTimeClockIn;

                    $scope.SearchRTSDetail.ListOfIdsInCsv = $scope.RTSDetail.ReferralTimeSlotDetailID;
                    $scope.RTSDetail.ReferralID = $scope.TempSearchRTSMaster.ReferralID;
                    var jsonData = angular.toJson({
                        rtsDetail: $scope.RTSDetail,
                        searchRTSDetail: $scope.SearchRTSDetail,
                        pageSize: $scope.RTSDetailListPager.pageSize,
                        pageIndex: $scope.RTSDetailListPager.fromIndex,
                        sortIndex: $scope.RTSDetailListPager.sortIndex,
                        sortDirection: $scope.RTSDetailListPager.sortDirection
                    });
                    var tempJsonData = jsonData;
                    AngularAjaxCall($http, HomeCareSiteUrl.UpdateRTSDetailURL, jsonData, "post", "json", "application/json", true).
                        success(function (response) {
                            if (response.IsSuccess) {
                                $scope.RTSDetailListPager.getDataCallback();
                                $scope.ResetRTSDetailModel();
                                $('#lblDayAdded').addClass('hide');
                            }
                            if (response.Data == -3) {
                                bootboxDialog(function (result) {
                                    if (result) {
                                        jsonData = tempJsonData;
                                        AngularAjaxCall($http, HomeCareSiteUrl.ReferralTimeSlotForceUpdateURL, jsonData, "Post", "json", "application/json").success(function (response) {
                                            if (response.IsSuccess) {
                                                $scope.RTSDetailListPager.getDataCallback();
                                                $scope.ResetRTSDetailModel();
                                            }
                                            ShowMessages(response);
                                        });
                                    }
                                }, bootboxDialogType.Confirm, "Force Update", window.TimeSlotForceUpdateConfirmation, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
                            }
                            ShowMessages(response);
                        });
                }
            }, bootboxDialogType.Confirm, "Alert", "It seems like you are updating the scheduled day/time. If there are any schedules that exist for the day, will be deleted.", bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
        }
    };



    $scope.ResetRTSDetailModel = function () {
        $('#lblDayAdded').addClass('hide');
        $scope.RTSDetail = $scope.newInstance().RTSDetail;
        $scope.SetTime($scope.RTSDetail);
        //$scope.SelectedCareType = null;
        $scope.RTSDetail.UsedInScheduling = true;
        $scope.RTSDetail.ReferralTimeSlotMasterID = $scope.RTSMaster.ReferralTimeSlotMasterID;
    }

    $scope.OpenEmpRefSchModal = function (event, item) {
        debugger
        $('#emprefschmodal').modal({ backdrop: false, keyboard: false });
        $("#emprefschmodal").modal({
            backdrop: 'static',
            keyboard: false
        });
        var referralid = $scope.TempSearchRTSMaster.ReferralID;
        //var startDate = moment(item.StartDate).format('LL');
        var startDate = moment(new Date()).format('LL');
        var endDate = moment(item.EndDate).format('LL');
        if (endDate == "Invalid date") {
            endDate = moment(new Date().addDays(365)).format('LL');
        }
        $scope.SearchSchEmployeeModel.StartDate = startDate;
        $scope.SearchSchEmployeeModel.EndDate = endDate;

        scopeEmpRefSch.CallOnPopUpLoad(referralid, startDate, null, endDate, item.CareTypeId, item.ReferralTimeSlotMasterID);

    }


    //#endregion

    $("a#CarePlan_ReferralTimeSlots").on('shown.bs.tab', function (e) {
        $scope.SearchRTSMasterList();
        $(".tab-pane a[href='#tab_ReferralTimeSlots']").tab('show');
    });

    $scope.nextOption = function () {
        var length = $scope.RTSModel.ReferralList.length;
        var index = $scope.RTSModel.ReferralList.findIndex(a => a.Value.toString() == $scope.TempSearchRTSMaster.ReferralID);
        if (index + 1 < length) {
            var next = $scope.RTSModel.ReferralList[index + 1].Value;
            $scope.TempSearchRTSMaster.ReferralID = next.toString();
            $scope.SearchRTSMasterList();
        }

    }

    $scope.prevOption = function () {
        var length = $scope.RTSModel.ReferralList.length;
        var index = $scope.RTSModel.ReferralList.findIndex(a => a.Value.toString() == $scope.TempSearchRTSMaster.ReferralID);
        if (index != -1) {
            var next = $scope.RTSModel.ReferralList[index - 1].Value;
            $scope.TempSearchRTSMaster.ReferralID = next.toString();
            $scope.SearchRTSMasterList();
        }
    }
    $scope.OpenMouseMessage = function (event, item) {
        var contentMsg = '<b>' + item.ReferralTimeSlotDetailID + '</b>';
        $(event.target).webuiPopover({ content: contentMsg, animation: 'pop' });
        $(event.target).click();
    }

};

controllers.AddReferralTimeSlotsController.$inject = ['$scope', '$http'];



$(document).ready(function () {
    $('.time').inputmask({
        mask: "h:s t\\m",
        placeholder: "hh:mm a",
        alias: "datetime",
        hourFormat: "12"
    });
});