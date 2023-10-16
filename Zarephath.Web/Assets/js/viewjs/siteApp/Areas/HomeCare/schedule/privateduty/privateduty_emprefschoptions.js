var scopeEmpRefSch;


controllers.EmpRefSchController = function ($scope, $http, $compile, $timeout, $filter) {
    scopeEmpRefSch = $scope;

    $scope.SearchModel = {};
    $scope.Skills = {};
    $scope.Preferences = {};


    $scope.ListRTSDetail = {};
    $scope.EmployeeTSList = {};
    $scope.EmpRefSkillList = [];
    $scope.EmpRefPreferenceList = [];

    $scope.PatientHoldDetailList = [];
    $scope.PatientHoldDetail = {};
    $scope.PatientHoldDetail.PatientOnHoldAction = false;
    $scope.RemoveScheduleDetail = {};


    //$scope.EmployeeTSListPager = new PagerModule("EmployeeDayOffID");
    //$scope.EmployeeTSListPager.pageSize = 10;


    $scope.GetEmpRefSchOptions = function (reloadSearch) {
        //

        if (!ValideElement($scope.SearchModel.StartDate) || !ValideElement($scope.SearchModel.EndDate))
            return false;


        $scope.EmpRefSchAjaxStart = true;
        var jsonData = $scope.SetSearchPostData($scope.EmployeeTSListPager);
        AngularAjaxCall($http, HomeCareSiteUrl.PrivateDuty_GetEmpRefSchOptionsURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {

                
                $scope.PatientPayorList = response.Data.PatientPayorList;
                $scope.PatientDetail = response.Data.PatientDetail;





                if (ValideElement($scope.PatientDetail.PatientPayorID) && $scope.PatientDetail.PatientPayorID > 0 && $scope.SearchModel.ScheduleID > 0) {
                    $scope.SearchModel.PayorID = $scope.PatientDetail.PatientPayorID;
                }
                //else if ((!ValideElement($scope.SearchModel.PayorID) || $scope.SearchModel.PayorID === 0) && $scope.SearchModel.ScheduleID === 0) {
                //    $scope.PatientPayorList.filter(function (data, index) {
                //        if (data.Precedence === 1)
                //            $scope.SearchModel.PayorID = data.PayorID;
                //    });
                //}


                $scope.RTSMaster = response.Data.RTSMaster;
                $scope.ListRTSDetail = response.Data.ListRTSDetail;
                $scope.EmployeeTSList = response.Data.Page.Items;
                $scope.PatientHoldDetailList = response.Data.PatientHoldDetailList;


                $scope.EmployeeTSListPager.currentPageSize = response.Data.Page.Items.length;
                $scope.EmployeeTSListPager.totalRecords = response.Data.Page.TotalItems;

                var selectedCareTypeID = $scope.SearchModel.CareTypeID;
                if (selectedCareTypeID && selectedCareTypeID > 0) {
                    $scope.ListRTSDetail = $scope.ListRTSDetail.filter(m => m.CareTypeId == selectedCareTypeID);
                }
            }
            $scope.EmpRefSchAjaxStart = false;
            ShowMessages(response);
        });


    }

    //$scope.EmployeeTSListPager.getDataCallback = $scope.GetEmpRefSchOptions;

    $scope.SetSearchPostData = function (pager) {

        $scope.SearchModel.StrSkillList = $scope.EmpRefSkillList ? $scope.EmpRefSkillList.toString() : "";
        $scope.SearchModel.StrPreferenceList = $scope.EmpRefPreferenceList ? $scope.EmpRefPreferenceList.toString() : "";



        var pagermodel = {
            model: $scope.SearchModel,
            pageSize: pager.pageSize,
            pageIndex: pager.currentPage,
            sortIndex: pager.sortIndex,
            sortDirection: pager.sortDirection,
            sortIndexArray: pager.sortIndexArray.toString()
        };

        pagermodel.model.StartDate = moment(pagermodel.model.StartDate).format("YYYY/MM/DD HH:mm:ss");
        pagermodel.model.EndDate = moment(pagermodel.model.EndDate).format("YYYY/MM/DD HH:mm:ss");

        return angular.toJson(pagermodel);
    };

    $scope.SearchEmpRefSchOptions = function () {
        if ($scope.EmployeeTSListPager) {
            $scope.EmployeeTSListPager.getDataCallback();
            $scope.ResetSearchEmpRefSchOptions();
        }
    };

    $scope.ResetSearchEmpRefSchOptions = function () {
        $scope.SelectedReferralTimeSlotDetailIDs = [];
    };
    $scope.ResetSearchEmpRefSchOptions();


    $scope.$watch('SearchModel.StartDate', function (newValue, oldValue, scope) {
        //
        if ($scope.SearchModel.TimePickerRequire) {
            if ($scope.SearchModel.EndDate == undefined)
                $scope.SearchModel.EndDate = $scope.SearchModel.StartDate;
        } else {

            $scope.SearchModel.EndDate = moment($scope.SearchModel.StartDate).add(365, 'days').format();
            $scope.SearchModel.MaxEndDate = moment($scope.SearchModel.StartDate).add(365, 'days').format();
        }

        //$scope.SearchEmpRefSchOptions();
    }, true);

    $scope.$watch('SearchModel.EndDate', function (newValue, oldValue, scope) {

        if (ValideElement($scope.SearchModel.StartDate) && ValideElement($scope.SearchModel.EndDate)) {

            if (moment(oldValue).format('L') !== moment(newValue).format('L')) {
                $scope.SearchEmpRefSchOptions();
            }
        }

    });


    $scope.CreateBulkSchedule = function (item, isReschedule) {


        //if (!ValideElement($scope.SearchModel.PayorID)) {
        //    bootboxDialog(function () { }, bootboxDialogType.Alert, bootboxDialogTitle.Alert, "Please Select The Patient Payor First");
        //    return false;
        //}


        $scope.SearchModel.EmployeeID = item.EmployeeID;
        $scope.SearchModel.EmployeeTimeSlotDetailIDs = item.EmployeeTimeSlotDetailIDs;
        $scope.SearchModel.ReferralTimeSlotDetailIDs = $scope.SelectedReferralTimeSlotDetailIDs ? $scope.SelectedReferralTimeSlotDetailIDs.toString() : "";
        $scope.SearchModel.IsRescheduleAction = ValideElement(isReschedule);

        var jsonData = $scope.SetSearchPostData($scope.EmployeeTSListPager);
        AngularAjaxCall($http, HomeCareSiteUrl.PrivateDuty_CreateBulkScheduleUrl, jsonData, "Post", "json", "application/json", true).success(function (response) {
            if (response.IsSuccess) {
                $scope.SearchEmpRefSchOptions();
                $scope.SchedulesCreated = true;
            }
            ShowMessages(response);
        });

    }



    $scope.CallOnPopUpLoad = function (referralid, startDate, event) {
        $scope.SchedulesCreated = false;
        $scope.SearchModel = {};
        $scope.SearchModel.ReferralID = referralid;
        $scope.SearchModel.ScheduleID = 0;

        $scope.SearchModel.StartDate = moment(startDate);
        $scope.SearchModel.EndDate = moment(startDate).add(365, 'days').format();
        $scope.SearchModel.MaxEndDate = moment($scope.SearchModel.StartDate).add(365, 'days').format();
        //$scope.SearchModel.StartDate = moment($scope.ScheduleSearchModel.StartDate);
        //$scope.SearchModel.EndDate = moment($scope.ScheduleSearchModel.StartDate).add(180, 'days').format();

        $scope.SearchModel.SameDateWithTimeSlot = false;
        $scope.SearchModel.TimePickerRequire = false;
        $scope.ShowPatientDeniedService = false;
        if (event) {

            $scope.SearchModel.SameDateWithTimeSlot = true;
            $scope.SearchModel.TimePickerRequire = true;

            $scope.SearchModel.ScheduleID = event.scheduleModel.ScheduleID;
            $scope.SearchModel.ReferralTSDateID = event.scheduleModel.ReferralTSDateID;
            $scope.ShowPatientDeniedService = true;

            if ($scope.SearchModel.ScheduleID === 0) {
                $scope.SearchModel.StartDate = moment(event.scheduleModel.StartDate);
                $scope.SearchModel.EndDate = moment(event.scheduleModel.EndDate);
            } else {
                $scope.SearchModel.StartDate = moment(event.scheduleModel.StartDate);
                $scope.SearchModel.EndDate = moment(event.scheduleModel.EndDate);
            }
        }


        $scope.EmployeeTSListPager = new PagerModule("EmployeeDayOffID");
        $scope.EmployeeTSListPager.pageSize = 10;
        $scope.EmployeeTSListPager.getDataCallback = $scope.GetEmpRefSchOptions;

        $timeout(function () {

            $scope.SkillsCount = 1;
            $scope.SkillsVal = "Skills DESC";

            $scope.PreferencesCount = 1;
            $scope.PreferencesVal = "Preferences DESC";

            $scope.MilesCount = 1;
            $scope.MilesVal = "Miles DESC";

            $scope.ConflictsCount = 1;
            $scope.ConflictsVal = "Conflicts DESC";




            //$scope.EmployeeTSListPager.sortIndexArray = [$scope.SkillsVal, $scope.PreferencesVal, $scope.MilesVal];
            $scope.EmployeeTSListPager.sortIndexArray = [$scope.ConflictsVal, $scope.PreferencesVal, $scope.MilesVal, $scope.SkillsVal];

            $scope.GetEmpRefSchOptions();
        }, 500);



    }


    $scope.GetPageLoadModels = function () {
        AngularAjaxCall($http, HomeCareSiteUrl.PrivateDuty_GetEmpRefSchPageModelUrl, {}, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.Skills = response.Data.Skills;
                $scope.Preferences = response.Data.Preferences;
            }
            ShowMessages(response);
        });

    };
    $scope.GetPageLoadModels();


    //$scope.DeleteSchedules = function (data) {
    //    $scope.SearchModel.StartDate;
    //    $scope.SearchModel.EndDate;
    //}


    $scope.SearchEmpRefMatchModel = {};
    $scope.SchEmpRefSkillsUrl = HomeCareSiteUrl.PrivateDuty_GetSchEmpRefSkillsUrl;
    $scope.GetAssignedEmployeesURL = HomeCareSiteUrl.PrivateDuty_GetAssignedEmployeesURL;

    $scope.SchEmpRefPreferencesUrl = "";
    $scope.SchEmpRefConflictsUrl = "";



    $scope.DeleteSchedules = function (data) {
        bootboxDialog(function (result) {
            if (result) {
                $scope.DeleteEmpRefScheduleModel = {};
                $scope.DeleteEmpRefScheduleModel.StartDate = moment($scope.SearchModel.StartDate); //$scope.SearchModel.StartDate;
                $scope.DeleteEmpRefScheduleModel.EndDate = moment($scope.SearchModel.EndDate); // $scope.SearchModel.EndDate;
                $scope.DeleteEmpRefScheduleModel.ReferralTimeSlotDetailID = data.ReferralTimeSlotDetailID;
                $scope.DeleteEmpRefScheduleModel.ReferralTimeSlotMasterID = data.ReferralTimeSlotMasterID;
                $scope.DeleteEmpRefScheduleModel.Day = data.Day;
                //$scope.DeleteEmpRefScheduleModel.StartTime = data.StartTime;
                //$scope.DeleteEmpRefScheduleModel.EndTime = data.EndTime;

                var jsonData = angular.toJson($scope.DeleteEmpRefScheduleModel);
                $scope.PatientRefSchAjaxStart = true;
                AngularAjaxCall($http, HomeCareSiteUrl.PrivateDuty_DeleteEmpRefScheduleURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    if (response.IsSuccess) {
                        $scope.GetEmpRefSchOptions();
                    }
                    $scope.PatientRefSchAjaxStart = false;
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, window.UnassignSchedule, window.UnasignScheduleMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    }

    $scope.SetReferralTimeSlotModel = function (data) {
        $scope.ReferralTimeSlotModel = {};
        $scope.ReferralTimeSlotModel.StartDate = $scope.SearchModel.StartDate;
        $scope.ReferralTimeSlotModel.EndDate = $scope.SearchModel.EndDate;
        $scope.ReferralTimeSlotModel.ReferralTimeSlotDetailID = data.ReferralTimeSlotDetailID;
        $scope.ReferralTimeSlotModel.ReferralTimeSlotMasterID = data.ReferralTimeSlotMasterID;
        //$scope.ReferralTimeSlotModel.ReferralID = $scope.SearchModel.ReferralID;
        $scope.ReferralTimeSlotModel.Day = data.Day;
    }

    //$scope.SelectedReferralTimeSlotDetailIDs = [];
    $scope.OnPatientDaySelection = function (item) {
        if (item.IsChecked)
            $scope.SelectedReferralTimeSlotDetailIDs.push(item.ReferralTimeSlotDetailID);
        else
            $scope.SelectedReferralTimeSlotDetailIDs.remove(item.ReferralTimeSlotDetailID);
    }

    $scope.ClosePatientHoldModal = function () {

        $("#pationOnHoldModal").modal('hide');
        HideErrors("#modalHoldFrom");
    }
    $scope.OpenPatientHoldModal = function (isHoldAction, patientHoldDetailModel, unHoldRemoveAction) {
        //
        if (patientHoldDetailModel) {
            //REMOVE MODE - UNHOLD MODE
            $scope.PatientHoldDetail = angular.copy(patientHoldDetailModel);
            $scope.PatientHoldDetail.NotifyEmployee = false;
        } else {
            //ADD MODE - HOLD MODE
            $scope.PatientHoldDetail = {};
            $scope.PatientHoldDetail.NotifyEmployee = true;
            $('#notifyEmp').parent().addClass('checked');
        }

        $scope.PatientHoldDetail.PatientOnHoldAction = isHoldAction;
        $scope.PatientHoldDetail.PatientUnHoldRemoveAction = unHoldRemoveAction;
        if (!isHoldAction && !unHoldRemoveAction) {
            $scope.PatientHoldDetail.StartDate = moment($scope.PatientHoldDetail.StartDate);
            $scope.PatientHoldDetail.EndDate = moment($scope.PatientHoldDetail.EndDate);
        }
        $scope.PatientHoldDetail.UnHoldDate = unHoldRemoveAction ? null : new Date();

        $('#pationOnHoldModal').modal({ backdrop: false, keyboard: false });
        $("#pationOnHoldModal").modal('show');
        //$scope.PatientHoldDetail.PatientOnHoldReason = null;
    }

    $scope.CloseRemoveScheduleModal = function () {

        $("#removeScheduleModal").modal('hide');
        HideErrors("#modalRemoveScheduleForm");
    }

    $scope.OnHoldUnHoldAction = function (item) {

        var isValid = CheckErrors($("#modalHoldFrom"));
        if (isValid) {
            $scope.PatientHoldDetail.ReferralID = $scope.PatientDetail.ReferralID;
            //$scope.PatientHoldDetail.StartDate = moment($scope.SearchModel.StartDate);
            // $scope.PatientHoldDetail.EndDate = moment($scope.SearchModel.EndDate);
            $scope.PatientHoldDetail.UnHoldDate = moment($scope.PatientHoldDetail.UnHoldDate);

            var jsonData = angular.toJson($scope.PatientHoldDetail);

            AngularAjaxCall($http, HomeCareSiteUrl.PrivateDuty_OnHoldUnHoldActionURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $("#pationOnHoldModal").modal('hide');
                    $scope.GetEmpRefSchOptions();
                    $scope.SchedulesCreated = true;
                }
                ShowMessages(response);
            });

        }


    }



    $scope.OpenRemoveScheduleModal = function (item) {
        
        $scope.RemoveScheduleDetail = {};
        $scope.RemoveScheduleDetail.ScheduleID = $scope.SearchModel.ScheduleID;
        $scope.RemoveScheduleDetail.ReferralTSDateID = $scope.SearchModel.ReferralTSDateID;
        
        HideErrors("#modalRemoveScheduleForm");
        $('#removeScheduleModal').modal({ backdrop: false, keyboard: false });
        $("#removeScheduleModal").modal('show');
    }
    $scope.OnRemoveScheduleAction = function () {
        var isValid = CheckErrors($("#modalRemoveScheduleForm"));
        if (isValid) {

            var jsonData = angular.toJson($scope.RemoveScheduleDetail);

            AngularAjaxCall($http, HomeCareSiteUrl.PrivateDuty_RemoveScheduleURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $("#removeScheduleModal").modal('hide');
                    $scope.GetEmpRefSchOptions();
                    $scope.SchedulesCreated = true;
                }
                ShowMessages(response);
            });
        }

    }






    //$scope.OnHoldUnHoldAction = function (item) {
    //    var title = window.PatientHold;
    //    var msg = window.PatientHoldConfirmation;
    //    if (!$scope.PatientHoldDetail.PatientOnHold) {
    //        title = window.PatientUnHold;
    //        msg = window.PatientUnHoldConfirmation;
    //    }

    //    bootboxDialog(function (result) {
    //        if (result) {
    //            alert($scope.PatientHoldDetail.PatientOnHold);

    //            //$scope.PatientHoldDetail.PatientOnHold
    //            //$scope.DeleteEmpRefScheduleModel = {};
    //            $scope.PatientHoldDetail.StartDate = moment($scope.SearchModel.StartDate);
    //            $scope.PatientHoldDetail.EndDate = moment($scope.SearchModel.EndDate);

    //            var jsonData = angular.toJson($scope.PatientHoldDetail);
    //            $scope.PatientRefSchAjaxStart = true;
    //            AngularAjaxCall($http, HomeCareSiteUrl.OnHoldUnHoldActionURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
    //                if (response.IsSuccess) {
    //                    $scope.GetEmpRefSchOptions();
    //                }
    //                $scope.PatientRefSchAjaxStart = false;
    //                ShowMessages(response);
    //            });


    //        }
    //        else {

    //            $scope.PatientHoldDetail.PatientOnHold = !$scope.PatientHoldDetail.PatientOnHold;
    //            if (!$scope.$root.$$phase) 
    //                $scope.$apply();
    //        }
    //    }, bootboxDialogType.Prompt, msg, msg, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);

    //}


};

controllers.EmpRefSchController.$inject = ['$scope', '$http', '$compile', '$timeout', '$filter'];

var wall;
$(document).ready(function () {


});
