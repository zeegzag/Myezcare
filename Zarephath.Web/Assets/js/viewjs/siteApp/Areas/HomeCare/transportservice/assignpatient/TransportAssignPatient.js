var scopeTransportAssignPatient;

controllers.TransportAssignPatientPopoverController = function ($scope, $http, $compile, $timeout, $filter) {
    scopeTransportAssignPatient = $scope;
    $scope.newInstance = undefined;
    $scope.TempSearchReferralModel = {};
    $scope.SearchReferralModel = {};
    $scope.TransportItem = {};
    $scope.Agencies = [];
    $scope.GroupList = {};
    $scope.ReferralListPager = new PagerModule("ClientName");
    $scope.ReferralListPager.pageSize = 10;
    $scope.SelectedReferralIds = [];
    $scope.ReferralList = [];
    $scope.ReferralListTemp = [];
    $scope.ReferralTempEdit = '';
    $scope.SelectAllCheckbox = false;
    $scope.TransportAssignPatientModel = {}//to save
    //
    $scope.SearchModel = {};
    $scope.ListRTSDetail = {};
    $scope.PatientHoldDetailList = [];
    $scope.PatientHoldDetail = {};
    $scope.PatientHoldDetail.PatientOnHoldAction = false;
    $scope.RemoveScheduleDetail = {};
    $scope.CreateSchModel = {};
    $scope.FirstTimeLoad = true;
    $scope.GetAssignedFacilitiesURL = HomeCareSiteUrl.GetAssignedFacilitiesURL;
    //


    $scope.GetEmpRefSchOptions = function (reloadSearch) {
        if (!ValideElement($scope.SearchModel.StartDate) || !ValideElement($scope.SearchModel.EndDate))
            return false;

        $scope.EmpRefSchAjaxStart = true;
        var jsonData = $scope.SetSearchPostData();
        AngularAjaxCall($http, HomeCareSiteUrl.CaseManagement_GetEmpRefSchOptionsURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
            if (response.IsSuccess) {
                $scope.PatientHoldDetailList = response.Data.PatientHoldDetailList;
                $scope.PatientDetail = response.Data.PatientDetail;


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
    //


    $scope.SetPostData = function (fromIndex, model) {
        var pagermodel = {
            searchTransportAssignPatientListModel: $scope.SearchReferralModel,
            pageSize: $scope.ReferralListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ReferralListPager.sortIndex,
            sortDirection: $scope.ReferralListPager.sortDirection
        };
        if (model != undefined) {
            pagermodel.referralStatusModel = model;
        }
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchReferralModel = $.parseJSON(angular.toJson($scope.TempSearchReferralModel));

    };


    $scope.ResetSearchFilter = function () {
        $scope.SearchReferralModel = $scope.newInstance.SearchTransportAssignPatientListModel;
        $scope.TempSearchReferralModel = $scope.newInstance.SearchTransportAssignPatientListModel;
        $scope.TransportAssignPatientModel = $scope.newInstance.TransportAssignPatientModel;
        $scope.SearchReferralModel.TransportID = '' + $scope.TransportItem.TransportID;
        $scope.TempSearchReferralModel.TransportID = '' + $scope.TransportItem.TransportID;
        $scope.TransportAssignPatientModel.TransportID = '' + $scope.TransportItem.TransportID;
        //
        $scope.TempSearchReferralModel.ClientName = '';
        $scope.TempSearchReferralModel.AHCCCSID = '';
        $scope.ResetDropdown();
        $scope.TempSearchReferralModel.IsDeleted = "0";
        $scope.ReferralListPager.currentPage = 1;
        $scope.TempSearchReferralModel.ServiceTypeID = null;
        $scope.ReferralListPager.getDataCallback();
    };

    $scope.ResetDropdown = function () {
        $scope.TempSearchReferralModel.NotifyCaseManagerID = "-1";
        //$scope.TempSearchReferralModel.ServiceID = "";
        $scope.TempSearchReferralModel.ChecklistID = "-1";
        $scope.TempSearchReferralModel.ClinicalReviewID = "-1";
        $scope.TempSearchReferralModel.IsSaveAsDraft = "-1";
        $scope.TempSearchReferralModel.ReferralStatusID = "";
        $scope.TempSearchReferralModel.PayorID = "";
        $scope.SelectedGroups = "";

    };
    $scope.ReferralIds = [];
    $scope.GetReferralList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control

        $scope.SelectedReferralIds = [];
        $scope.SelectAllCheckbox = false;
        //Reset Selcted Checkbox items and Control
        var cmId = GetCookie('CM_ID');
        if (ValideElement(cmId)) {
            $scope.TempSearchReferralModel.CaseManagerID = cmId;
            $scope.SearchReferralModel.CaseManagerID = cmId;
            if (!$scope.$root.$$phase) {
                $scope.$apply();
            }
        }

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping


        $scope.AjaxStart = true;
        var jsonData = $scope.SetPostData($scope.ReferralListPager.currentPage);

        AngularAjaxCall($http, HomeCareSiteUrl.GetTransportAssignmentReferralListURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralList = response.Data.Items;
                $scope.ReferralListTemp = response.Data.Items;
                $scope.ReferralListPager.currentPageSize = response.Data.Items.length;
                $scope.ReferralListPager.totalRecords = response.Data.TotalItems;
            }
            $scope.AjaxStart = false;
        });
    };

    $scope.Refresh = function () {
        $scope.ReferralListPager.getDataCallback();
    };
    $scope.AssignReferral = function (AssignReferralitem) {

        $scope.TransportAssignPatientModel.TransportAssignPatientID = '' + AssignReferralitem.TransportAssignPatientID;
        $scope.TransportAssignPatientModel.ReferralID = '' + AssignReferralitem.ReferralID;
        $scope.TransportAssignPatientModel.TransportID = '' + $scope.TransportItem.TransportID;
        $scope.TransportAssignPatientModel.Note = (AssignReferralitem.Note != null ? '' + AssignReferralitem.Note : '');
        $scope.TransportAssignPatientModel.IsBillable = '' + AssignReferralitem.IsBillable;
        $scope.TransportAssignPatientModel.Startdate = '' + AssignReferralitem.Startdate;
        $scope.TransportAssignPatientModel.EndDate = '' + AssignReferralitem.EndDate;

        var jsonData = angular.toJson({ transportAssignPatientModel: $scope.TransportAssignPatientModel });
        AngularAjaxCall($http, HomeCareSiteUrl.SaveTransportAssignPatientURL, jsonData, "Post", "json", "application/json", true).success(function (response) {

            if (response.IsSuccess) {
                $scope.ResetSearchFilter();
            }
            $scope.AjaxStart = false;
        });
        //        
    }
    $scope.EditTransportAssignmentPatient = function (AssignReferralitem) {

        $scope.TransportAssignPatientModel.TransportAssignPatientID = '' + AssignReferralitem.TransportAssignPatientID;
        $scope.TransportAssignPatientModel.ReferralID = '' + AssignReferralitem.ReferralID;
        $scope.TransportAssignPatientModel.TransportID = '' + $scope.TransportItem.TransportID;
        $scope.TransportAssignPatientModel.Note = (AssignReferralitem.Note != null ? '' + AssignReferralitem.Note : '');
        $scope.TransportAssignPatientModel.IsBillable = '' + AssignReferralitem.IsBillable;
        $scope.TransportAssignPatientModel.Startdate = '' + AssignReferralitem.Startdate;
        $scope.TransportAssignPatientModel.EndDate = '' + AssignReferralitem.EndDate;
        $scope.TransportAssignPatientModel
        var jsonData = angular.toJson({ transportAssignPatientModel: $scope.TransportAssignPatientModel });
        AngularAjaxCall($http, HomeCareSiteUrl.SaveTransportAssignPatientURL, jsonData, "Post", "json", "application/json", true).success(function (response) {

            if (response.IsSuccess) {
                $scope.ResetSearchFilter();
            }
            $scope.AjaxStart = false;
        });
    }
    $scope.GetTransportAssignmentPatientForEdit = function (AssignReferralitem) {
        AssignReferralitem.IsEdit = true;
    }
    $scope.SelectAll = function () {
        $scope.SelectedReferralIds = [];
        angular.forEach($scope.ReferralList, function (item, key) {
            if (item.TransportAssignPatientID > 0) {
                item.IsChecked = $scope.SelectAllCheckbox;
                if (item.IsChecked) {
                    $scope.SelectedBatchIds.push(item.BatchID);
                }
            }
        });
        return true;
    };
    $scope.DeleteTransportAssignmentPatient = function (AssignReferralitem, title) {

        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchReferralModel.ListOfIdsInCsv = AssignReferralitem.TransportAssignPatientID > 0 ? AssignReferralitem.TransportAssignPatientID.toString() : $scope.SelectedReferralIds.toString();
                if (AssignReferralitem.TransportAssignPatientID > 0) {
                    if ($scope.ReferralListPager.currentPage != 1)
                        $scope.ReferralListPager.currentPage = $scope.VehicleList.length === 1 ? $scope.ReferralListPager.currentPage - 1 : $scope.ReferralListPager.currentPage;
                } else {
                    if ($scope.ReferralListPager.currentPage != 1 && $scope.SelectedReferralIds.length == $scope.ReferralListPager.currentPageSize)
                        $scope.ReferralListPager.currentPage = $scope.ReferralListPager.currentPage - 1;
                }

                $scope.SelectedReferralIds = [];
                $scope.SelectAllCheckbox = false;

                var jsonData = $scope.SetPostData($scope.ReferralListPager.currentPage);


                AngularAjaxCall($http, HomeCareSiteUrl.DeleteTransportAssignmentReferralListURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {

                        $scope.ReferralList = response.Data.Items;
                        $scope.ReferralListPager.currentPageSize = response.Data.Items.length;
                        $scope.ReferralListPager.totalRecords = response.Data.TotalItems;
                        ShowMessages(response);
                    } else {
                        bootboxDialog(function () {
                        }, bootboxDialogType.Alert, window.Alert, window.FacilityHouseScheduleExistMessage);
                    }

                });

            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);

    }
    $scope.CancelEditTransportAssignmentPatient = function (AssignReferralitem) {
        AssignReferralitem.IsEdit = false;
        $scope.ReferralListPager.getDataCallback();
    }
    $scope.SearchReferral = function () {
        $scope.ReferralListPager.currentPage = 1;
        $scope.ReferralListPager.getDataCallback(true);
    };
    $scope.GetReferralListGroupList = function () {
        $scope.GroupList = {};
        AngularAjaxCall($http, "/hc/referral/GetReferralListGroupList", "", "Get", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.GroupList = response.Data;

            }
        });
    };
    $scope.Load = function (TransportItem) {

        $scope.TransportItem = TransportItem;
        if (!($scope.newInstance)) {
            $scope.LoadPatient(TransportItem);
        } else {
            $scope.ResetSearchFilter();
        }
    };
    $scope.LoadPatient = function (TransportItem) {
        $scope.GetReferralListGroupList();
        AngularAjaxCall($http, HomeCareSiteUrl.TransportAssignmentListURL, {}, "Post", "json", "application/json")
            .success(function (response) {
                $scope.newInstance = response;

                $scope.ResetSearchFilter();
            });
    };
    $scope.ReferralListPager.getDataCallback = $scope.GetReferralList;
};

controllers.TransportAssignPatientPopoverController.$inject = ['$scope', '$http', '$compile', '$timeout', '$filter'];

var wall;
$(document).ready(function () {


});
