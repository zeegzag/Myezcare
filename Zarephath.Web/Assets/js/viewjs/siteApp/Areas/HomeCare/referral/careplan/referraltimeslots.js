var custModel1;

controllers.AddReferralTimeSlotsController = function ($scope, $http) {
    custModel1 = $scope;
    $scope.EnableLabel = false;
    $scope.toggle = false;
    $scope.IsEditMode = false;
    $scope.SelectedAuthCode = '';
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnRTSModel").val());
    };
    $scope.RTSModel = $scope.newInstance();
    $scope.RTSMaster = $scope.newInstance().RTSMaster;
    $scope.RTSDetail = $scope.newInstance().RTSDetail;


    $scope.patientStatusFilters = ['All', 'Expired', 'Active']
    $scope.selectedPatientStatusFilter = 'Active'
    $scope.patientStatusFilter = ['Active', 'Expired', 'Delete']
    $scope.selectedPatientStatusFilter = 'Active'
    //#region RTS Master Related Code
    //#region RTS Master Paging 
    $scope.RTSMasterList = [];
    $scope.ReferralTimeSlotList = [];
    $scope.SearchReferralTimeSlotList = [];

    $scope.RTSMasterListTemp = [];
    $scope.RTSMasterListPager = new PagerModule("ReferralTimeSlotMasterID");
    $scope.SearchRTSMaster = $scope.RTSModel.SearchRTSMaster;
    $scope.SearchReferralTimeSlotDetail = $scope.RTSModel.SearchReferralTimeSlotDetail;
    $scope.TempSearchReferralTimeSlotDetail = $scope.RTSModel.SearchReferralTimeSlotDetail;
    $scope.TempSearchRTSMaster = $scope.RTSModel.SearchRTSMaster;
    $scope.TempSearchRTSMaster.Filter = $scope.selectedPatientStatusFilter;
    $scope.ReferralTimeSlotDetailPager = new PagerModule("ReferralID");

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

    $scope.IsMasterChange = function () {
        return ($scope.RTSMaster.ReferralTimeSlotMasterID > 0 &&
            (!isSameDate($scope.OrgRTSMaster.StartDate, $scope.RTSMaster.StartDate) ||
                $scope.OrgRTSMaster.IsEndDateAvailable !== $scope.RTSMaster.IsEndDateAvailable ||
                ($scope.RTSMaster.IsEndDateAvailable &&
                    !isSameDate($scope.OrgRTSMaster.EndDate, $scope.RTSMaster.EndDate))));
    };

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
                $scope.SearchRTSMaster.IsDeleted = 1;  //0=Active, 1=DELETE

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
    $scope.ToggleView = function ($event) {

        if (event.target.checked) {//old view
            $scope.toggle = event.target.checked;


        }
        if (!event.target.checked) {//new view
            $scope.toggle = event.target.checked;

        }

        //    $scope.SearchVisitTask();

    }
    $scope.OrgRTSMaster = {};
    
    $scope.OpenRTSMasterModal = function ($event, item) {
        debugger
        $('#lblDayAdded').addClass('hide');
        $('#btnDropdown').text('');
        $event.stopPropagation();
        if (item != null || item != undefined) {
            $scope.EnableLabel = true;
            $scope.OrgRTSMaster.CareTypeID = item.CareTypeID;
        }
        if (window.isCaseManagement != undefined && window.isCaseManagement == "1") {
            // $scope.RTSMaster = $scope.newInstance().RTSMaster;
            $scope.RTSMaster.StartDate = new Date();
            // $scope.RTSMaster.EndDate = new Date();
            $scope.RTSMaster.ReferralID = $scope.TempSearchRTSMaster.ReferralID;
            var jsonData = angular.toJson({
                rtsMaster: $scope.RTSMaster
            });
            AngularAjaxCall($http, HomeCareSiteUrl.GetReferralAuthorizationsByReferralIDUrl, jsonData, "post", "json", "application/json", true).
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
                    $scope.IsEditMode = true;
                    $scope.RTSMaster.ReferralTimeSlotMasterID = item.ReferralTimeSlotMasterID;
                    $scope.RTSMaster.StartDate = moment(item.StartDate);
                    $scope.RTSMaster.EndDate = moment(item.EndDate);
                    $scope.RTSMaster.IsEndDateAvailable = item.IsEndDateAvailable;
                    $scope.RTSMaster.CareTypeID = item.CareTypeID;
                    $scope.RTSMaster.CareType = item.CareType;
                    $scope.RTSMaster.ServiceCode = item.ServiceCode;
                    $scope.RTSMaster.AuthorizationCode = item.AuthorizationCode;
                    $scope.RTSMaster.ReferralBillingAuthorizationID = item.ReferralBillingAuthorizationID;
                    $scope.RTSDetail.ReferralTimeSlotMasterID = item.ReferralTimeSlotMasterID;
                    $scope.RTSDetail.UsedInScheduling = true;
                    //$scope.RTSDetail.CareTypeId = $scope.RTSMaster.CareTypeId;
                    $scope.SearchRTSDetail.ReferralTimeSlotMasterID = item.ReferralTimeSlotMasterID;
                    $scope.SearchRTSDetail.CareTypeId = $scope.RTSMaster.CareTypeID;
                    $scope.RTSMaster.IsWithPriorAuth = item.IsWithPriorAuth;
                    $scope.RTSMaster.IsVisible = item.IsWithPriorAuth;
                    $scope.RTSDetail.ReferralID = item.ReferralID;
                    $scope.IsEditMode = true;
                    if ($scope.SearchRTSDetail.ReferralTimeSlotMasterID > 0 )
                        $scope.GetRTSDetailList();
                }
                $scope.SearchRTSDetail.ReferralTimeSlotMasterID = item.ReferralTimeSlotMasterID;
            }
            $scope.LastEndDate = moment($scope.RTSMaster.EndDate).toDate;
            $scope.SetTime($scope.RTSDetail);
            if (item == undefined) { $scope.GetOrganizationSettings(); }
            $scope.OrgRTSMaster = _.cloneDeep($scope.RTSMaster);
            $scope.GetReferralBillingAuthorizations();
            //  $scope.GetCaretype();
            // $scope.GetPAContract();//$scope.RTSMaster.ReferralID

            $('#rtsMasterModel').modal({
                backdrop: 'static',
                keyboard: false
            });
            $('#rtsMasterModel').modal('show');
        }
    }
    $scope.SetTimeSlotPostData = function (fromIndex) {
        var pagermodel = {
            searchReferralTimeSlotDetail: $scope.SearchReferralTimeSlotDetail,
            pageSize: $scope.ReferralTimeSlotDetailPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ReferralTimeSlotDetailPager.sortIndex,
            sortDirection: $scope.ReferralTimeSlotDetailPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };
    $scope.GetReferralTimeSlotDetail = function ($event, item) {
        $event.stopPropagation();
        $scope.SearchReferralTimeSlotDetail.ReferralID = item.ReferralID;
        $scope.SearchReferralTimeSlotDetail.ReferralTimeSlotMasterID = item.ReferralTimeSlotMasterID;
        $scope.ReferralTimeSlotDetail();
    };

    $scope.ReferralTimeSlotDetail = function () {
       
        var jsonData = $scope.SetTimeSlotPostData($scope.ReferralTimeSlotDetailPager.currentPage);

        $scope.ReferralTimeSlotList.length = 0;
        $scope.SearchReferralTimeSlotList.length = 0;
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralTimeSlotDetailURL, jsonData, "post", "json", "application/json", true).
            success(function (response) {

                if (response.IsSuccess) {
                    $scope.ReferralTimeSlotList = response.Data.Items;
                    if (response.Data != undefined && response.Data.Items.length > 0) {
                       
                        /*$scope.ReferralTimeSlotDetailPager.getDataCallback();*/
                        $scope.ReferralTimeSlotDetailPager.currentPageSize = response.Data.Items.length;
                        $scope.ReferralTimeSlotDetailPager.totalRecords = response.Data.TotalItems;

                        $('#referralTimeSlotDtl').modal({
                            backdrop: 'static',
                            keyboard: false
                        });
                    } else {
                        bootbox.alert({
                            title: "<span style='color: info;'>Schdule date passed!</span>",
                            message: "Schdule date passed",
                            size: 'small'
                        });
                        ShowMessages(response);
                    }
                } else {
                    ShowMessages(response);
                }
            });


    }

    $scope.ReferralTimeSlotDetailPager.getDataCallback = $scope.ReferralTimeSlotDetail;

    $scope.SearchTimeSlotDetail = function () {
       
        $scope.ReferralTimeSlotDetailPager.currentPage = 1;
        $scope.ReferralTimeSlotDetailPager.getDataCallback(true);
    };
    $scope.ResetTimeSlotDetail = function () {
       
        $scope.ReferralTimeSlotDetailPager.currentPage = 1;
        $scope.SearchReferralTimeSlotDetail.ClientName = '';
        $scope.SearchReferralTimeSlotDetail.StartDate = '';
        $scope.SearchReferralTimeSlotDetail.EndDate = '';
        $scope.ReferralTimeSlotDetailPager.getDataCallback(true);
    };
    $scope.ChangeEmployee = function (newEmployeeId, item, listOfIdsInCsv) {
        debugger;

        $scope.SearchReferralTimeSlotDetail.ScheduleID = item.ScheduleID;
        $scope.SearchReferralTimeSlotDetail.EmployeeID = newEmployeeId;

        if (item != undefined && item.ScheduleID > 0) {
            if ($scope.ReferralTimeSlotDetailPager.currentPage != 1)
                $scope.ReferralTimeSlotDetailPager.currentPage = $scope.ReferralList.length === 1 ? $scope.ReferralTimeSlotDetailPager.currentPage - 1 : $scope.ReferralTimeSlotDetailPager.currentPage;
        } else {

            if ($scope.ReferralTimeSlotDetailPager.currentPage != 1 && $scope.SelectedReferralIds.length == $scope.ReferralTimeSlotDetailPager.currentPageSize)
                $scope.ReferralTimeSlotDetailPager.currentPage = $scope.ReferralTimeSlotDetailPager.currentPage - 1;
        }

        var jsonData = $scope.SetTimeSlotPostData($scope.ReferralTimeSlotDetailPager.currentPage);
        
        return AngularAjaxCall($http, HomeCareSiteUrl.UpdateTimeSlotDetailEmployeeURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.ReferralTimeSlotList = response.Data.Items;
                $scope.ReferralTimeSlotDetailPager.currentPageSize = response.Data.Items.length;
                $scope.ReferralTimeSlotDetailPager.totalRecords = response.Data.TotalItems;
                $scope.ShowCollpase();
            }

        });
    };

    $scope.OpenRTSDeleteModal = function ($event, item, isDeleted) {
        $event.stopPropagation;
        if (item.ReferralTimeSlotMasterID > 0) {
            $scope.ResetRTSDetail();
            $scope.SearchRTSMaster.ListOfIdsInCsv = item.ReferralTimeSlotMasterID;
            $scope.SearchRTSMaster.IsDeleted = isDeleted;  //0=> Mark Active, 1=> Mark DELETE
            var jsonData = $scope.SetPostData($scope.RTSMasterListPager.currentPage);

            if (isDeleted == 1) {
                bootboxDialog(function (result) {
                    if (result) {
                        $scope.DeletePatientRTS(jsonData);
                    }
                }, bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, window.RefFutureSchDeleteConfirmationMaster, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
            }
            else {
                bootboxDialog(function (result) {
                    if (result) {
                        $scope.DeletePatientRTS(jsonData);
                    }
                }, bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, window.PatientForceSchedulesConfirmation, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
            }
        }

    }
    $scope.ExistanceOfReferralTimeslot = function () {
        var rtsMaster = {
            ReferralID: $scope.OrgRTSMaster.ReferralID,
            ReferralBillingAuthorizationID: $scope.RTSMaster.ReferralBillingAuthorizationID,
        };

        var jsonData = angular.toJson(rtsMaster);

        AngularAjaxCall($http, HomeCareSiteUrl.ExistanceOfReferralTimeslot, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.OrgRTSMaster.ReferralTimeSlotMasterID = response.Data;
                $scope.RTSDetail.ReferralTimeSlotMasterID = response.Data;
                $scope.HideButton = true;
            }
            else {
                $scope.OrgRTSMaster.ReferralTimeSlotMasterID = response.Data;
                $scope.HideButton = false;
            }

        });
    };

    $scope.GetReferralBillingAuthorizations = function () {
        var jsonData = angular.toJson({
            rtsMaster: {
                ReferralID: $scope.RTSMaster.ReferralID,
                CareTypeID: $scope.RTSMaster.CareTypeID
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
                    // $scope.OnReferralBillingAuthorizationChange();
                } else {
                    // ShowMessages(response);
                }
            });
    }

    $scope.OnCareTypeChange = function () {
        $scope.SearchRTSDetail.CareTypeId = $scope.RTSDetail.CareTypeId;
        $scope.GetReferralBillingAuthorizations($scope.RTSDetail.CareTypeId);
        if ($scope.SearchRTSDetail.ReferralTimeSlotMasterID > 0)
            $scope.GetRTSDetailList();
        $scope.ReferralBillingAuthorizations = null;
        $scope.RTSMaster.AuthorizationCode = null;
    };
    $scope.MakeDateFormat = function (date) {
        var today = moment(date, 'MM/DD/YYYY').format(GetOrgDateFormat());

        return today;
    }
    $scope.OnReferralBillingAuthorizationChange_Old = function (item) {
        debugger
        $scope.RTSDetail.SelectedRBA = $scope.ReferralBillingAuthorizations.find(rba => rba.ReferralBillingAuthorizationID == item.ReferralBillingAuthorizationID);
        $scope.RTSDetail.ReferralBillingAuthorizationID = item.ReferralBillingAuthorizationID;
        $scope.RTSDetail.AuthorizationCode = item.AuthorizationCode;
    };
    $scope.OnReferralBillingAuthorizationChange = function (item, option) {
        debugger
        if (option == true) {/*Invoice base*/
            // $scope.RTSMaster = $scope.newInstance().RTSMaster;
            $scope.RTSMaster.SelectedRBA = $scope.newInstance().SelectedRBA;
            $scope.RTSMaster.SelectedRBA = $scope.ReferralBillingAuthorizations.find(rba => rba.ReferralBillingAuthorizationID == item.ReferralBillingAuthorizationID);
            $scope.RTSMaster.ReferralBillingAuthorizationID = item.ReferralBillingAuthorizationID;
            $scope.RTSMaster.ReferralID = $scope.RTSMaster.ReferralID; //$scope.RTSMaster.SelectedRBA.ReferralID;
            $scope.RTSMaster.AuthorizationCode = item.AuthorizationCode;
            //$scope.RTSMaster.StartDate = $scope.MakeDateFormat(item.StartDate);
            //$scope.RTSMaster.EndDate = $scope.MakeDateFormat(item.EndDate);
            $scope.RTSMaster.StartDate = moment(item.StartDate);
            $scope.RTSMaster.EndDate = moment(item.EndDate);
            $scope.RTSMaster.CareTypeID = item.CareTypeID;
            $scope.RTSDetail.CareTypeId = item.CareTypeID;
            $scope.RTSMaster.CareType = item.CareType;
            $scope.RTSMaster.ServiceCode = item.ServiceCode;
            $scope.RTSMaster.IsEndDateAvailable = true;
            $scope.RTSMaster.IsVisible = option;
            $scope.RTSMaster.ReferralTimeSlotMasterID = $scope.RTSMaster.ReferralTimeSlotMasterID ? $scope.RTSMaster.ReferralTimeSlotMasterID:0;
            //$scope.InvoiceBaseList = _.cloneDeep($scope.RTSDetail);

            $scope.EnableLabel = true;
            var jsonData = { ReferralID: item.ReferralID, BillingAuthorizationID: item.ReferralBillingAuthorizationID };
            AngularAjaxCall($http, "/hc/referral/PrioAuthorization", jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {

                    $scope.PrioAuthorizationList = response.Data;
                    //$scope.AddUnits();
                }

            });
        }
        else if (option == false) {/*With out Invoice base*/
            //  $scope.RTSDetail = $scope.newInstance().RTSDetail;
            $scope.RTSMaster.SelectedRBA = $scope.ReferralBillingAuthorizations.find(rba => rba.ReferralBillingAuthorizationID == item.ReferralBillingAuthorizationID);
            $scope.RTSMaster.ReferralBillingAuthorizationID = item.ReferralBillingAuthorizationID;
            $scope.RTSMaster.ReferralID = $scope.RTSMaster.ReferralID; //$scope.RTSMaster.SelectedRBA.ReferralID;
            $scope.RTSMaster.AuthorizationCode = item.AuthorizationCode;
            $scope.RTSMaster.CareTypeID = item.CareTypeID;
            $scope.RTSDetail.CareTypeId = item.CareTypeID;

            $scope.RTSMaster.CareType = item.CareType;
            $scope.RTSDetail.CareType = item.CareType;

            $scope.RTSMaster.ServiceCode = item.ServiceCode;
            if (item.ReferralBillingAuthorizationID > 0) {
                //$scope.RTSMaster.StartDate = $scope.MakeDateFormat(item.StartDate);
                //$scope.RTSMaster.EndDate = $scope.MakeDateFormat(item.EndDate);
                $scope.RTSMaster.StartDate = moment(item.StartDate);
                $scope.RTSMaster.EndDate = moment(item.EndDate);
                $scope.RTSMaster.IsEndDateAvailable = true;
            }
            else {
                $scope.RTSMaster.StartDate = null;
                $scope.RTSMaster.EndDate = null;
                $scope.RTSMaster.IsEndDateAvailable = false;
            }
            
            $scope.RTSMaster.IsVisible = option;
            //$scope.WithoutInvoiceBaseList = _.cloneDeep($scope.RTSDetail);
            //$scope.RTSMaster.ReferralTimeSlotMasterID = 0;
            if ($scope.RTSMaster.ReferralTimeSlotMasterID > 0)
                $scope.GetRTSDetailList();
        } 
        //$scope.ExistanceOfReferralTimeslot();de
        $scope.RTSDetail.UsedInScheduling = true;
    };



    $scope.GetOrganizationSettings = function () {
        AngularAjaxCall($http, "/hc/referral/GetOrganizationSettings", "", "Get", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                var json = response.Data;

                for (var key in json) {
                    if (json.hasOwnProperty(key)) {
                        var item = json[key];
                        var isScheduleType = item.ScheduleType
                        $scope.RTSMaster.IsVisible = isScheduleType;

                    }
                }

            }
        });

    };
    $scope.IsVisible = false;
    $scope.RadioSelectChange = function () {
        /*$('#btnDropdown').text('');   */
        //$scope.RTSMaster = $scope.newInstance().RTSMaster;
        var radioValue = $("input[name='settings']:checked").val();
        $scope.RTSMaster.IsVisible = radioValue.toLowerCase() === 'true';
        $scope.EnableLabel = false;
        $scope.EnableUpdate = false;
        $scope.HideButton = false;
        //$scope.OrgRTSMaster.ReferralTimeSlotMasterID = 0;
        //$scope.OrgRTSMaster.ReferralTimeSlotMasterID = 0;
        //$scope.RTSDetail.ReferralTimeSlotMasterID = 0;

        $scope.RTSMaster.ReferralID = $scope.RTSDetail.ReferralID.toString();
        $scope.RTSMaster.ReferralTimeSlotMasterID = $scope.RTSDetail.ReferralTimeSlotMasterID;
        $scope.RTSMaster.StartDate = moment($scope.RTSMaster.StartDate);
        $scope.RTSMaster.EndDate = moment($scope.RTSMaster.EndDate);
        $scope.RTSMaster.IsEndDateAvailable = $scope.RTSMaster.IsEndDateAvailable;
        $scope.RTSMaster.CareTypeID = $scope.RTSDetail.CareTypeId;
        $scope.RTSMaster.CareType = $scope.RTSMaster.CareType;
        $scope.RTSMaster.ServiceCode = $scope.RTSMaster.ServiceCode;
        $scope.RTSMaster.AuthorizationCode = $scope.RTSMaster.AuthorizationCode;
        $scope.RTSMaster.ReferralBillingAuthorizationID = $scope.RTSMaster.ReferralBillingAuthorizationID;
        $scope.RTSDetail.ReferralTimeSlotMasterID = $scope.RTSMaster.ReferralTimeSlotMasterID;
        $scope.RTSDetail.UsedInScheduling = true;
        $scope.RTSDetail.CareTypeId = $scope.RTSMaster.CareTypeID;
        $scope.SearchRTSDetail.ReferralTimeSlotMasterID = $scope.RTSMaster.ReferralTimeSlotMasterID;
        $scope.SearchRTSDetail.CareTypeId = $scope.RTSDetail.CareTypeId;

        $scope.RTSDetail.ReferralID = $scope.RTSMaster.ReferralID;
        if ($scope.SearchRTSDetail.ReferralTimeSlotMasterID > 0)
            $scope.GetRTSDetailList();

        $scope.SearchRTSDetail.ReferralTimeSlotMasterID = $scope.RTSMaster.ReferralTimeSlotMasterID;
        $scope.LastEndDate = moment($scope.RTSMaster.EndDate).toDate;



        // $scope.SearchRTSDetail.CareTypeId = 0;
        $scope.GetReferralBillingAuthorizations();
        $("#ddlCaretype").val('');
        //$('#btnDropdown').text('');
        //$('#btnddl1').text('');

    };
    $scope.previewOfferValid = function () {
        //$scope.RTSMaster.ReferralTimeSlotMasterID = 21426;
        return ($scope.RTSMaster.ReferralTimeSlotMasterID > 0 && $scope.SavedSchedule !== null && $scope.SavedSchedule === $scope.RTSMaster.IsVisible)
        //return true;
    }

    $scope.SaveInvoiceTSMaster = function () {
        $scope.SaveRTSMain();
    }

    $scope.SaveWithoutInvoiceTSMaster = function () {
        if ($scope.RTSMaster.CareTypeID > 0) {
            
            $scope.SaveRTSMain();
        }
        else {
            toastr.error("Please select CareType");
        }
       
    }

    $scope.SaveRTSMasterOld = function () {
        debugger
        RadioSelectChange
        var isValid = CheckErrors($("#frmRTSMaster"));
        $scope.RTSMaster.IsVisible = false;
        if (isValid) {
            if ($scope.RTSMaster.ReferralTimeSlotMasterID > 0) {
                bootboxDialog(function (result) {
                    if (result) {
                        $scope.SaveRTSMainSaveRTSMain();
                    }
                }, bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, window.RefFutureSchDeleteConfirmationMaster, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
            } else {
                $scope.SaveRTSMain();
            }
        }
    };
    $scope.SaveRTSMaster = function () {
        debugger
        $scope.CareTypeID = $scope.RTSDetail.CareTypeId;// CareTypeId;
        if ($scope.RTSMaster.IsVisible) {/*On Invoice basis */
            $scope.RTSMaster.IsWithPriorAuth = $scope.RTSMaster.IsVisible;
            $scope.SaveInvoiceTSMaster();
        }
        if ($scope.RTSMaster.ReferralBillingAuthorizationID == null) {
       // if ($scope.RTSMaster.ReferralTimeSlotMasterID == 0) {
            $scope.RTSMaster.IsWithPriorAuth = $scope.RTSMaster.IsVisible;
            $scope.SaveWithoutInvoiceTSMaster();
        }
        else {/* without invoice*/
            //var isValid = CheckErrors($("#frmRTSMaster"));

            //if (isValid) {
            //var jsonData = angular.toJson({ 'ReferralID': referralID });
            // AngularAjaxCall($http, HomeCareSiteUrl.GetReferralBillingAuthorizationDatesURL, jsonData, "Post", "json", "application/json", false).success(function (response) {

            //if (response.IsSuccess) {
            $scope.RTSMaster.IsWithPriorAuth = $scope.RTSMaster.IsVisible;
            var itemList = $scope.ReferralBillingAuthorizations;
            /* $scope.RTSMaster = response.Data;*/

            var stDtArray = [];
            var EndDtArray = [];
            if (itemList.length > 0) {
                for (var key in itemList) {
                    if (itemList.hasOwnProperty(key)) {
                        var item = itemList[key];
                        stDtArray.push({
                            stDate: $scope.MakeDateFormat(item.StartDate),// new Date(item.StartDate).toISOString().slice(0, 10),
                        });
                        EndDtArray.push({
                            endDate: $scope.MakeDateFormat(item.EndDate),// new Date(item.EndDate).toISOString().slice(0, 10),
                        });
                    }
                }


                var selectedStartDateFormatted = $scope.MakeDateFormat(moment($scope.RTSMaster.StartDate));// new Date(selectedStartDate).toISOString().slice(0, 10);

                stDtArray.sort(function (a, b) {
                    return new Date(a.stDate) - new Date(b.stDate);
                });

                var minimumDate = stDtArray[0].stDate;

              //  if (selectedStartDateFormatted < minimumDate) {
                    /*$('#StartDateValidationAlertModal').modal('show');*/

                    //$("#Alert").text('Cannot upate < b > Patient Schedule</b > as the selected start date is less than the < b > Prior Authorization </b > start date');
                    //bootbox.alert({
                    //    title: "<span style='color: red;'>Invalid Start Date!</span>",
                    //    message: "Cannot upate <b>Patient Schedule</b> as the selected start date is less than the <b> Prior Authorization </b> start date",
                    //    size: 'small'
                    //});
                    //alert($("#Alert").text());
                //    return;
                //}


                if ($scope.RTSMaster.EndDate != null) {
                    $scope.RTSMaster.IsEndDateAvailable = true;
                    var selectedEndDateFormatted = $scope.MakeDateFormat(moment($scope.RTSMaster.EndDate)); //new Date(selectedEnddate).toISOString().slice(0, 10);

                    EndDtArray.sort(function (a, b) {
                        return new Date(b.endDate) - new Date(a.endDate);
                    });

                    var maximumDate = EndDtArray[0].endDate;

                    if (selectedEndDateFormatted > maximumDate) {
                        // $('#EndDateValidationAlertModal').modal('show');
                        bootbox.alert({
                            title: "<span style='color: red;'>Invalid End Date!</span>",
                            message: "Cannot upate <b>Patient Schedule</b> as the selected end date is greater than the <b> Prior Authorization </b> end date",
                            size: 'small'
                        });


                        return;
                    }
                }
            }

            //if ($scope.RTSMaster.ReferralTimeSlotMasterID ==0) {
            //    $scope.SaveWithoutInvoiceTSMaster();
            //}
            /*  $scope.SaveRTSMain();*/
            if ($scope.RTSMaster.ReferralTimeSlotMasterID > 0) {
                //$scope.SaveWithoutInvoiceTSMaster();
                bootboxDialog(function (result) {
                    if (result) {
                        $scope.SaveWithoutInvoiceTSMaster();
                    }
                }, bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, window.RefFutureSchDeleteConfirmationMaster, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
            } else {
                if ($scope.RTSMaster.IsVisible == false) {
                    $scope.SaveWithoutInvoiceTSMaster();
                }
            }
            //}
            //});


            // }
        }

    }
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
        //var isValid = CheckErrors($("#frmRTSMaster"));
        // if (isValid) {
        $scope.RTSMaster.ReferralID = $scope.OrgRTSMaster.ReferralID;
        $scope.EnableUpdate = false;
        var jsonData = angular.toJson({
            rtsMaster: $scope.RTSMaster
        });
        if ($scope.RTSMaster.ReferralBillingAuthorizationID != undefined && $scope.RTSMaster.ReferralBillingAuthorizationID > 0) {
            AngularAjaxCall($http, HomeCareSiteUrl.AddRTSByPriorAuthURL, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        $('#rtsByPriorAuthModel').modal('hide');
                        $scope.RTSMasterListPager.getDataCallback();
                        $scope.RTSMaster.ReferralTimeSlotMasterID = response.Data;
                        $scope.RTSDetail.ReferralTimeSlotMasterID = response.Data;
                        $scope.SearchRTSDetail.ReferralTimeSlotMasterID = response.Data;
                        //$scope.RTSDetailListPager.getDataCallback();
                        $scope.OrgRTSMaster = _.cloneDeep($scope.RTSMaster);
                        $scope.EnableUpdate = true;
                        if ($scope.RTSMaster.ReferralTimeSlotMasterID > 0) {
                            $scope.GetRTSDetailList();
                        }
                        ShowMessages(response);
                    }

                });
        }
        else if ($scope.RTSMaster.ReferralBillingAuthorizationID == null && $scope.RTSMaster.IsVisible == true)
            toastr.error("Please select PriorAuthorization");
        else {

            AngularAjaxCall($http, HomeCareSiteUrl.AddRTSMasterURL, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        $scope.RTSMasterListPager.getDataCallback();
                        $scope.RTSMaster.ReferralTimeSlotMasterID = response.Data;
                        $scope.OrgRTSMaster = _.cloneDeep($scope.RTSMaster);
                        $scope.RTSDetail.ReferralTimeSlotMasterID = response.Data;
                        $scope.RTSDetail.UsedInScheduling = true;
                        $scope.RTSDetail.CareTypeId = $scope.RTSDetail.CareTypeId;
                        //$scope.RTSDetail.CareTypeId = $scope.OrgRTSMaster.CareTypeID;
                        $scope.RTSDetail.CareType = $scope.OrgRTSMaster.CareType;
                        $scope.SearchRTSDetail.ReferralTimeSlotMasterID = response.Data;
                        $scope.SearchRTSDetail.CareTypeId = $scope.RTSDetail.CareTypeId;
                        //$scope.RTSDetailListPager.getDataCallback();
                        $scope.SavedSchedule = $scope.RTSMaster.IsVisible;
                        $scope.GetReferralBillingAuthorizations();
                        $scope.RTSDetailList.title = $scope.RTSDetail.CareType;
                        //$('#RTSMasterModel').modal('hide');
                        $scope.EnableUpdate = true;
                        if ($scope.RTSDetail.ReferralTimeSlotMasterID > 0) {
                            $scope.GetRTSDetailList();
                    }
                    }

                    ShowMessages(response);
                });

        }
        // }

    };

    //$scope.SaveRTSMain = function () {
    //    var isValid = CheckErrors($("#frmRTSMaster"));
    //    if (isValid) {
    //        var jsonData = angular.toJson({
    //            rtsMaster: $scope.RTSMaster
    //        });
    //        if ($scope.RTSMaster.ReferralBillingAuthorizationID != undefined && $scope.RTSMaster.ReferralBillingAuthorizationID > 0) {
    //            AngularAjaxCall($http, HomeCareSiteUrl.AddRTSByPriorAuthURL, jsonData, "post", "json", "application/json", true).
    //                success(function (response) {
    //                    if (response.IsSuccess) {
    //                        $('#rtsByPriorAuthModel').modal('hide');
    //                    }
    //                    ShowMessages(response);
    //                });
    //        }
    //        else {
    //            AngularAjaxCall($http, HomeCareSiteUrl.AddRTSMasterURL, jsonData, "post", "json", "application/json", true).
    //                success(function (response) {
    //                    if (response.IsSuccess) {
    //                        $scope.RTSMasterListPager.getDataCallback();
    //                        $scope.RTSMaster.ReferralTimeSlotMasterID = response.Data;
    //                        $scope.OrgRTSMaster = _.cloneDeep($scope.RTSMaster);
    //                        $scope.RTSDetail.ReferralTimeSlotMasterID = response.Data;
    //                        $scope.RTSDetail.UsedInScheduling = true;
    //                        $scope.RTSDetail.CareTypeId = $scope.RTSMaster.CareTypeId;
    //                        $scope.SearchRTSDetail.ReferralTimeSlotMasterID = response.Data;
    //                        $scope.SearchRTSDetail.CareTypeId = $scope.RTSDetail.CareTypeId;
    //                        $scope.RTSDetailListPager.getDataCallback();

    //                        $scope.GetReferralBillingAuthorizations();
    //                        //$('#RTSMasterModel').modal('hide');
    //                    }

    //                    ShowMessages(response);
    //                });
    //        }
    //    }
    //};
    //#endregion


    //#region RTS Details Related Code

    $scope.RTSMasterClick = function (item) {
        $scope.SearchRTSDetail.ReferralTimeSlotMasterID = item.ReferralTimeSlotMasterID;
        $scope.RTSDetail.CareTypeId = $scope.RTSMaster.CareTypeID;
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
                ShowMessages(response);
            }

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
    $scope.ValidateTime = function () {

        $('#lblDayAdded').addClass('hide');
        if ($scope.RTSDetail.StrStartTime && $scope.RTSDetail.StrEndTime) {
            if (new Date(moment($scope.RTSDetail.StrStartTime, "hh:mm a")).getTime() > new Date(moment($scope.RTSDetail.StrEndTime, "hh:mm a")).getTime()) {
                var aa = $scope.RTSDetail.StrStartTime.indexOf('pm');
                var abb = $scope.RTSDetail.StrStartTime.indexOf('am');
                if ($scope.RTSDetail.StrStartTime.indexOf('PM') == -1 && $scope.RTSDetail.StrStartTime.indexOf('AM') == -1) {
                    $('#lblDayAdded').addClass('hide');

                }
                else {
                    $('#lblDayAdded').removeClass('hide');

                }
            }
        }

    };

    $scope.OpenRTSDetailModal = function ($event, item) {
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
                //$scope.OnCareTypeChange();
                //$scope.OnReferralBillingAuthorizationChange();
                //if (item.CareTypeId != null) {
                //    $scope.SelectedCareType = item.CareTypeId.toString();
                //} else {
                //    $scope.SelectedCareType = null;
                //}
            }
        }
        $scope.ValidateTime();
        //$('#RTSDetailModal').modal('show');
    }

    
    $scope.SaveRTSDetail = function () {
        debugger
        $scope.RTSDetail.StartDate = $scope.RTSMaster.StartDate;
        $scope.RTSDetail.EndDate = $scope.RTSMaster.EndDate;
        $scope.RTSDetail.ReferralID = $scope.TempSearchRTSMaster.ReferralID;
        $scope.SearchRTSDetail.ReferralTimeSlotMasterID = $scope.RTSDetail.ReferralTimeSlotMasterID;
        // $scope.RTSDetail.ReferralTimeSlotMasterID = $scope.RTSMaster.ReferralTimeSlotMasterID;
        $scope.RTSDetail.ReferralBillingAuthorizationID = $scope.RTSMaster.ReferralBillingAuthorizationID ? $scope.RTSMaster.ReferralBillingAuthorizationID : $scope.RTSDetail.ReferralBillingAuthorizationID;
        $scope.RTSDetail.CareTypeId = $scope.RTSMaster.CareTypeID ? $scope.RTSMaster.CareTypeID : $scope.RTSDetail.CareTypeId;
        // $scope.RTSDetail.CareTypeId = $scope.RTSMaster.CareTypeID;
        //$scope.RTSDetail.CareTypeId = $scope.OrgRTSMaster.CareTypeId;
        //$scope.RTSDetail.CareTypeId = $scope.CareTypeID;
        //$scope.RTSDetail.CareType = $scope.RTSMaster.CareType;
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
                $scope.populateGrid = true;
            });
        $("#RTSDetail_StrStartTime").removeAttr('readonly', 'true');
        $("#RTSDetail_StrEndTime").removeAttr('readonly', 'true');

        //}
    };
    //$scope.SaveRTSDetail = function () {
    //    debugger
    //    if (CheckErrors($("#frmRTSDetail"))) {
    //        $scope.RTSDetail.StartDate = $scope.RTSMaster.StartDate;
    //        $scope.RTSDetail.EndDate = $scope.RTSMaster.EndDate;
    //        $scope.RTSDetail.ReferralID = $scope.TempSearchRTSMaster.ReferralID;
    //        $scope.RTSDetail.ReferralTimeSlotMasterID = $scope.RTSMaster.ReferralTimeSlotMasterID;
    //        $scope.TempRTSDetail = $scope.RTSDetail;
    //        var jsonData = angular.toJson({
    //            rtsDetail: $scope.RTSDetail
    //        });
    //        AngularAjaxCall($http, HomeCareSiteUrl.AddRTSDetailURL, jsonData, "post", "json", "application/json", true).
    //            success(function (response) {
    //                if (response.IsSuccess) {
    //                    $scope.RTSDetailListPager.getDataCallback();
    //                    $scope.ResetRTSDetailModel();
    //                }
    //                if (response.Data == -3) {
    //                    bootboxDialog(function (result) {
    //                        if (result) {
    //                            jsonData = angular.toJson({
    //                                rtsDetail: $scope.TempRTSDetail
    //                            });
    //                            AngularAjaxCall($http, HomeCareSiteUrl.ReferralTimeSlotForceUpdateURL, jsonData, "Post", "json", "application/json").success(function (response) {
    //                                if (response.IsSuccess) {
    //                                    $scope.RTSDetailListPager.getDataCallback();
    //                                    $scope.ResetRTSDetailModel();
    //                                    $scope.RTSDetail.ReferralTimeSlotMasterID = $scope.RTSMaster.ReferralTimeSlotMasterID;
    //                                }
    //                                ShowMessages(response);
    //                            });
    //                        }
    //                    }, bootboxDialogType.Confirm, "Force Update", window.TimeSlotForceUpdateConfirmation, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    //                }
    //                else if (response.Data == -4) {
    //                    bootboxDialog(function (result) {
    //                        if (result) {
    //                            $scope.TempRTSDetail.IsForcePatientSchedules = true;
    //                            jsonData = angular.toJson({
    //                                rtsDetail: $scope.TempRTSDetail
    //                            });
    //                            AngularAjaxCall($http, HomeCareSiteUrl.AddRTSDetailURL, jsonData, "post", "json", "application/json", true).
    //                                success(function (response) {
    //                                    if (response.IsSuccess) {
    //                                        $scope.RTSDetailListPager.getDataCallback();
    //                                        $scope.ResetRTSDetailModel();
    //                                    }
    //                                    ShowMessages(response);
    //                                });
    //                        }
    //                    }, bootboxDialogType.Confirm, "Alert", "The Schedule Time is more than allowed time. Would you still want to schedule? ", bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    //                }
    //                ShowMessages(response);
    //            });
    //        $("#RTSDetail_StrStartTime").removeAttr('readonly', 'true');
    //        $("#RTSDetail_StrEndTime").removeAttr('readonly', 'true');
    //    }
    //};


    $scope.UpdateRTSDetail = function () {
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

    function ConvertJsonDateToNormal(jsonDate) {
        var dateString = jsonDate.substr(6);
        var currentTime = new Date(parseInt(dateString));
        var month = ("0" + (currentTime.getMonth() + 1)).slice(-2);
        var day = ("0" + (currentTime.getDate())).slice(-2);
        var year = currentTime.getFullYear();
        var date = month + "/" + day + "/" + year;
        return date;
    }

    $('#ul').on('click', 'a', function (event) {

        var authcodeValue = $(this).text();
        var index = $(this).index();

        RefferalbillingauthorizationId = $('#' + index).val();//hiddenfield's value
        var IDs = $('#' + index).val().split(',');

        if (IDs.length > 0)
            $scope.SelectedCareTypeID = parseInt(IDs[1]);//set selected caretypeid

        var array = authcodeValue.split(':');      //split(/\s+/);
        if (array[1].length > 0) {
            var authCode = array[1].replace('CareType', '');
            $scope.SelectedAuthCode = $.trim(authCode);

        }
        $scope.EnableUpdate = true;
        var startDate = array[4] + ":" + array[5] + ":" + array[6] + ":" + array[7].replace('EndDate', '');// array[4].replace('EndDate', '');
        var endDate = array[8] + ":" + array[9] + ":" + array[10] + ":" + array[11];
        $('#btnDropdown').text("StartDate:" + $.trim(startDate) + "," + "EndDate:" + $.trim(endDate));

        $('.caret pull-right"').hide();
        event.preventDefault();
        return false;
    });

    function MakeUL(array) {
        var ul = document.getElementById('ul');
        ul.innerHTML = '';
        var list = document.createElement('li');

        for (var i = 0; i < array.length; i++) {
            var stDate = moment(array[i].StartDate).format(); //moment(array[i]..StartDate).toDate;
            var endDate = moment(array[i].EndDate).format();
            var htmlspan =
                "<span style='font-size:10px;'><b style='color: #c41212;'>AuthCode:&nbsp;</b>" + array[i].AuthorizationCode + "&nbsp;<b style='color: #c41212; '>CareType:&nbsp;</b>" + array[i].CareType + "</span> &nbsp;<span style='font-size:10px;'><b style='color: #c41212;'>ServiceCode:&nbsp;</b>" + array[i].ServiceCode + "</span><br /><span style='font-size:10px;'> <b style='color: #c41212;'>StartDate:&nbsp;</b>" + stDate + " &nbsp;<b style='color: #c41212; '>EndDate:&nbsp;</b>" + endDate + "</span><input type='hidden' id='" + i + "' value='" + array[i].ReferralBillingAuthorizationID + "," + array[i].CareTypeID + "'>";
            var doc = new DOMParser().parseFromString(htmlspan, "text/html");
            var anchortag = document.createElement('a');
            anchortag.style.fontSize = "9px";
            anchortag.append(doc.body);
            list.appendChild(anchortag);

        }
        ul.appendChild(list);
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