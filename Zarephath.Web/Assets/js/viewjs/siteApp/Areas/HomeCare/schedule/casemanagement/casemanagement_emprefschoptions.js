var scopeEmpRefSch;

controllers.EmpRefSchController = function ($scope, $http, $compile, $timeout, $filter) {
    scopeEmpRefSch = $scope;

    $scope.SearchModel = {};
    $scope.ListRTSDetail = {};
    $scope.PatientHoldDetailList = [];
    $scope.PatientHoldDetail = {};
    $scope.PatientHoldDetail.PatientOnHoldAction = false;
    $scope.RemoveScheduleDetail = {};
    $scope.CreateSchModel = {};
    $scope.FirstTimeLoad = true;
    $scope.GetAssignedFacilitiesURL = HomeCareSiteUrl.GetAssignedFacilitiesURL;


    $scope.GetEmpRefSchOptions = function (reloadSearch) {
        if (!ValideElement($scope.SearchModel.StartDate) || !ValideElement($scope.SearchModel.EndDate))
            return false;
        
        $scope.EmpRefSchAjaxStart = true;
        var jsonData = $scope.SetSearchPostData();
        AngularAjaxCall($http, HomeCareSiteUrl.CaseManagement_GetEmpRefSchOptionsURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
            if (response.IsSuccess) {


                //$scope.FacilityList = response.Data.FacilityList;
                //$scope.PatientPayorList = response.Data.PatientPayorList;

                //$scope.RTSMaster = response.Data.RTSMaster;
                //$scope.ListRTSDetail = response.Data.ListRTSDetail;
                $scope.PatientHoldDetailList = response.Data.PatientHoldDetailList;
                $scope.PatientDetail = response.Data.PatientDetail;

                //if (ValideElement($scope.PatientDetail.PatientPayorID) && $scope.PatientDetail.PatientPayorID > 0 && $scope.SearchModel.ScheduleID > 0) {
                //    $scope.SearchModel.PayorID = $scope.PatientDetail.PatientPayorID;
                //}
                //$scope.CreateSchModel = response.Data.CreateSchModel;

                if (!$scope.$root.$$phase) {
                    $scope.$apply();
                }




            }
            $scope.EmpRefSchAjaxStart = false;
            ShowMessages(response);
        });


    };

    $scope.SetSearchPostData = function () {
        var pagermodel = { model: $scope.SearchModel };
        pagermodel.model.StartDate = moment(pagermodel.model.StartDate).format("YYYY/MM/DD HH:mm:ss");
        pagermodel.model.EndDate = moment(pagermodel.model.EndDate).format("YYYY/MM/DD HH:mm:ss");
        return angular.toJson(pagermodel);
    };

    $scope.SearchEmpRefSchOptions = function () {
        $scope.ResetSearchEmpRefSchOptions();
        $scope.GetEmpRefSchOptions();
    };

    $scope.ResetSearchEmpRefSchOptions = function () {
        $scope.SelectedReferralTimeSlotDetailIDs = [];
        $scope.CreateSchModel = {};

    };
    
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

                if ($scope.FirstTimeLoad) {
                    $scope.FirstTimeLoad = false;
                } else {
                    $scope.SearchEmpRefSchOptions();
                }

            }
        }
    });


    $scope.CreateBulkSchedule = function (isReschedule) {
        //
        var isValid = CheckErrors($("#createSchForm"));
        if (isValid) {
            $scope.SearchModel.PayorID = $scope.CreateSchModel.PayorID;
            $scope.SearchModel.FacilityID = $scope.CreateSchModel.FacilityID;
            $scope.SearchModel.ReferralTimeSlotDetailIDs = $scope.SelectedReferralTimeSlotDetailIDs ? $scope.SelectedReferralTimeSlotDetailIDs.toString() : "";
            $scope.SearchModel.IsRescheduleAction = $scope.CreateSchModel.ScheduleID > 0; //ValideElement(isReschedule);

            var jsonData = $scope.SetSearchPostData();
            AngularAjaxCall($http, HomeCareSiteUrl.DayCare_CreateBulkScheduleUrl, jsonData, "Post", "json", "application/json", true).success(function (response) {
                if (response.IsSuccess) {
                    $scope.SearchEmpRefSchOptions();
                    $scope.SchedulesCreated = true;
                }
                ShowMessages(response);
            });
        }
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


        $timeout(function () {
            HideErrors("#createSchForm");
            $scope.FirstTimeLoad = true;
            $scope.ResetSearchEmpRefSchOptions();
            $scope.GetEmpRefSchOptions();
        }, 500);

    }



    $scope.SearchEmpRefMatchModel = {};
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
                //$scope.PatientRefSchAjaxStart = true;
                AngularAjaxCall($http, HomeCareSiteUrl.DayCare_DeleteEmpRefScheduleURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
                    if (response.IsSuccess) {
                        $scope.GetEmpRefSchOptions();
                        $scope.SchedulesCreated = true;
                    }
                    //$scope.PatientRefSchAjaxStart = false;
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
        $(".emprefschmod").css("min-height", "300px");
        $(".emprefschmod-width").css("width", "30%");
        $("#pationOnHoldModal").modal('hide');
        HideErrors("#modalHoldFrom");
    }
    $scope.OpenPatientHoldModal = function (isHoldAction, patientHoldDetailModel, unHoldRemoveAction) {

        //

        $(".emprefschmod").css("min-height", "500px");
        $(".emprefschmod-width").css("width", "40%");


        if (patientHoldDetailModel) {
            //REMOVE MODE - UNHOLD MODE
            $scope.PatientHoldDetail = angular.copy(patientHoldDetailModel);
        } else {
            //ADD MODE - HOLD MODE
            $scope.PatientHoldDetail = {};
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

            AngularAjaxCall($http, HomeCareSiteUrl.CaseManagement_OnHoldUnHoldActionURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.ClosePatientHoldModal();
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

            AngularAjaxCall($http, HomeCareSiteUrl.DayCare_RemoveScheduleURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $("#removeScheduleModal").modal('hide');
                    $scope.GetEmpRefSchOptions();
                    $scope.SchedulesCreated = true;
                }
                ShowMessages(response);
            });
        }

    }


};

controllers.EmpRefSchController.$inject = ['$scope', '$http', '$compile', '$timeout', '$filter'];

var wall;
$(document).ready(function () {


});
