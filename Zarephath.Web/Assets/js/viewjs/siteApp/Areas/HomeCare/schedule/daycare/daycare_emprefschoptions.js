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
        AngularAjaxCall($http, HomeCareSiteUrl.DayCare_GetEmpRefSchOptionsURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
            $scope.SearchModel.isLoading = false;
            if (response.IsSuccess) {


                $scope.FacilityList = response.Data.FacilityList;
                $scope.PatientPayorList = response.Data.PatientPayorList;

                $scope.RTSMaster = response.Data.RTSMaster;
                $scope.ListRTSDetail = response.Data.ListRTSDetail;
                $scope.PatientHoldDetailList = response.Data.PatientHoldDetailList;
                $scope.PatientDetail = response.Data.PatientDetail;
                $scope.CareTypeList = response.Data.CareTypeList;
                if (ValideElement($scope.PatientDetail.PatientPayorID) && $scope.PatientDetail.PatientPayorID > 0 && $scope.SearchModel.ScheduleID > 0) {
                    $scope.SearchModel.PayorID = $scope.PatientDetail.PatientPayorID;
                }
                $scope.CreateSchModel = response.Data.CreateSchModel;
                if ($scope.CareTypeList !== undefined && $scope.CareTypeList.length === 1) {
                    if ($scope.CareTypeList[0].CareTypeID != $scope.CreateSchModel.CareTypeID) {
                        $scope.CreateSchModel.CareTypeID = $scope.CareTypeList[0].CareTypeID;
                    }
                }
                $scope.SetReferralBillingAuthorizationID();
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
    };

    $scope.$watch('SearchModel.StartDate', function (newValue, oldValue, scope) {
        //
        if ($scope.SearchModel.TimePickerRequire) {
            if ($scope.SearchModel.EndDate == undefined)
                $scope.SearchModel.EndDate = $scope.SearchModel.StartDate;
        } else {
            if (!$scope.CreateSchModel.ReferralBillingAuthorizationID) {
                $scope.SearchModel.EndDate = moment($scope.SearchModel.StartDate).add(365, 'days').format();
                $scope.SearchModel.MaxEndDate = moment($scope.SearchModel.StartDate).add(365, 'days').format();
            }

        }

        //$scope.SearchEmpRefSchOptions();
    }, true);

    $scope.$watch('SearchModel.EndDate', function (newValue, oldValue, scope) {
        if ($scope.SearchModel.isLoading) return;
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
        var isValid = CheckErrors($("#createSchForm"));
        if (isValid) {
            if ($scope.CreateSchModel.PayorID > 0 && $scope.CreateSchModel.ReferralBillingAuthorizationID > 0 || $scope.CreateSchModel.ReferralBillingAuthorizationID != undefined) {
                $scope.SearchModel.ReferralBillingAuthorizationID = $scope.CreateSchModel.ReferralBillingAuthorizationID;
                $scope.SearchModel.PayorID = $scope.CreateSchModel.PayorID;
                $scope.SearchModel.FacilityID = $scope.CreateSchModel.FacilityID;
                $scope.SearchModel.ReferralTimeSlotDetailIDs = $scope.SelectedReferralTimeSlotDetailIDs ? $scope.SelectedReferralTimeSlotDetailIDs.toString() : "";
                $scope.SearchModel.IsRescheduleAction = $scope.CreateSchModel.ScheduleID > 0; //ValideElement(isReschedule);

                var jsonData = $scope.SetSearchPostData();
                AngularAjaxCall($http, HomeCareSiteUrl.DayCare_CreateBulkScheduleUrl, jsonData, "Post", "json", "application/json", true).success(function (response) {
                    if (response.IsSuccess) {
                        $scope.CreateSchModel = {};
                        $scope.SearchEmpRefSchOptions();
                        $scope.SchedulesCreated = true;
                    }
                    ShowMessages(response);
                    $(".PA").removeClass("input-validation-error").addClass("valid");
                });
            }
            if ($scope.CreateSchModel.PayorID == null || $scope.CreateSchModel.PayorID == 0) {
                $scope.SearchModel.ReferralBillingAuthorizationID = $scope.CreateSchModel.ReferralBillingAuthorizationID;
                $scope.SearchModel.PayorID = $scope.CreateSchModel.PayorID;
                $scope.SearchModel.FacilityID = $scope.CreateSchModel.FacilityID;
                $scope.SearchModel.ReferralTimeSlotDetailIDs = $scope.SelectedReferralTimeSlotDetailIDs ? $scope.SelectedReferralTimeSlotDetailIDs.toString() : "";
                $scope.SearchModel.IsRescheduleAction = $scope.CreateSchModel.ScheduleID > 0; //ValideElement(isReschedule);

                var jsonData = $scope.SetSearchPostData();
                AngularAjaxCall($http, HomeCareSiteUrl.DayCare_CreateBulkScheduleUrl, jsonData, "Post", "json", "application/json", true).success(function (response) {
                    if (response.IsSuccess) {
                        $scope.CreateSchModel = {};
                        $scope.SearchEmpRefSchOptions();
                        $scope.SchedulesCreated = true;
                    }
                    ShowMessages(response);
                });
            }
            else {
                //$(".PA").removeClass("valid");
                //$(".PA").attr("data-original-title", "PriorAuth is required").attr("data-html", "true")
                //    .addClass("tooltip-danger input-validation-error")
                //    .tooltip({ html: true });
            }

        }
    }

    $scope.CallOnPopUpLoad = function (referralid, startDate, event, endDate, CareTypeId, referralTimeSlotMasterID) {
        $scope.SchedulesCreated = false;
        $scope.SearchModel = {};
        $scope.SearchModel.isLoading = true;
        $scope.SearchModel.ReferralID = referralid;
        $scope.SearchModel.ScheduleID = 0;
        $scope.SearchModel.CareTypeID = CareTypeId;
        $scope.SearchModel.ReferralTimeSlotMasterID = referralTimeSlotMasterID;

        $scope.SearchModel.StartDate = moment(startDate);
        $scope.SearchModel.EndDate = endDate != undefined ? moment(endDate) : moment(startDate).add(365, 'days').format();
        if (endDate != null && endDate != undefined && endDate != "Invalid date") {
            $scope.SearchModel.EndDate = moment(endDate).format();
        }
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
            $scope.CreateSchModel = {};
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

        $("#pationOnHoldModal").modal('hide');
        HideErrors("#modalHoldFrom");
    }
    $scope.OpenPatientHoldModal = function (isHoldAction, patientHoldDetailModel, unHoldRemoveAction) {

        //
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

            AngularAjaxCall($http, HomeCareSiteUrl.DayCare_OnHoldUnHoldActionURL, jsonData, "Post", "json", "application/json").success(function (response) {
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

    $scope.OnRefSchBillingAuthorizationChange = function (item) {
        $scope.CreateSchModel.ReferralBillingAuthorizationID = item.ReferralBillingAuthorizationID;
        $scope.CreateSchModel.ReferralBillingAuthorizationName = item.ReferralBillingAuthorizationName;
        $scope.CreateSchModel.ReferralID = item.ReferralID;
        if (!$scope.SearchModel.TimePickerRequire && !$scope.SearchModel.AuthEndDateSet) {
            $scope.SearchModel.StartDate = moment(item.StartDate).format("YYYY-MM-DD");
            $scope.SearchModel.EndDate = moment(item.EndDate).format("YYYY-MM-DD");
            $scope.SearchModel.MaxEndDate = moment(item.EndDate).format("YYYY-MM-DD");
            $scope.SearchModel.ReferralID = item.ReferralID;
            $scope.SearchModel.AuthEndDateSet = true;
        }
        $(".PA").removeClass("tooltip-danger").addClass("valid");
    }

    $scope.SetReferralBillingAuthorizationID = function () {
        if ($scope.ReferralBillingAuthorizationList !== undefined && $scope.ReferralBillingAuthorizationList.length === 1) {
            if ($scope.ReferralBillingAuthorizationList[0].ReferralBillingAuthorizationID != $scope.CreateSchModel.ReferralBillingAuthorizationID) {
                $scope.OnRefSchBillingAuthorizationChange($scope.ReferralBillingAuthorizationList[0]);
            }
        }
    }


    $scope.$watch('CreateSchModel.PayorID', function (newValue, oldValue, scope) {

        if (oldValue != newValue && newValue > 0) {
            var jsonData = { PayorID: newValue, ReferralID: $scope.SearchModel.ReferralID };
            AngularAjaxCall($http, HomeCareSiteUrl.DayCare_GetReferralBillingAuthorizationListURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {

                    $scope.ReferralBillingAuthorizationList = response.Data;
                    $scope.SetReferralBillingAuthorizationID();
                }
            });


        }

        //


    }, true);


};

controllers.EmpRefSchController.$inject = ['$scope', '$http', '$compile', '$timeout', '$filter'];

var wall;
$(document).ready(function () {


});
