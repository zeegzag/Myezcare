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
    $scope.IsCheckedFrequencyAll = false;
    $scope.SearchModel.IsVirtualVisit = false;
    $scope.SelectedReferralTimeSlotDetailIDs = [];

    $scope.getCareType = function (payorID) {
        if (payorID != null && payorID > 0) {
            var jsonData = { payorID: payorID };
            AngularAjaxCall($http, HomeCareSiteUrl.GetCareTypesbyPayorID, jsonData, "Post", "json", "application/json", false).success(function (response) {
                if (response.IsSuccess) {
                    $scope.CareTypeList = response.Data;
                }
            });
        }
    };

    $scope.GetAutherizationOptions = function () {
        $scope.EmpRefSchAjaxStart = true;
        var jsonData = $scope.SetSearchPostData($scope.EmployeeTSListPager);
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmpRefSchOptionsURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {

                $scope.SetAutherizationOptions(response);
            }
            $scope.EmpRefSchAjaxStart = false;
            ShowMessages(response);
        });
    };


    $scope.SetAutherizationOptions = function (response) {
        $scope.ReferralAuthorizationServiceCodeList = response.Data.ReferralAuthorizationServiceCodeList;

        if ($scope.ReferralAuthorizationServiceCodeList !== undefined && $scope.ReferralAuthorizationServiceCodeList.length === 1) {
            $scope.SearchModel.ReferralBillingAuthorizationID = $scope.ReferralAuthorizationServiceCodeList[0].ReferralBillingAuthorizationID;
            $scope.PrioAuthorizationList = $scope.ReferralAuthorizationServiceCodeList[0];
        }

        $scope.PatientDetail = response.Data.PatientDetail;
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
    };

    $scope.GetCareTypeOptions = function (callback) {
        $scope.EmpRefSchAjaxStart = true;
        var jsonData = $scope.SetSearchPostData($scope.EmployeeTSListPager);
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmpCareTypeIDURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {

                $scope.CareTypeList = response.Data.CareTypeList;
                if ($scope.CareTypeList !== undefined && $scope.CareTypeList.length === 1) {
                    $scope.SearchModel.CareTypeID = $scope.CareTypeList[0].DDMasterID;
                }
            }
            $scope.EmpRefSchAjaxStart = false;
            ShowMessages(response);
            if (ValideElement(callback)) {
                callback(response);
            }
        });
    };

    $scope.GetEmpRefSchOptions = function (reloadSearch, CareTypeId) {
        if (!ValideElement($scope.SearchModel.StartDate) || !ValideElement($scope.SearchModel.EndDate))
            return false;

        $scope.SearchModel.CareTypeID = CareTypeId > 0 ? CareTypeId : $scope.SearchModel.CareTypeID;

        var jsonData = $scope.SetSearchPostData($scope.EmployeeTSListPager);
        $scope.GetEmpRefSchOptions_PatientVisitFrequency_HC(jsonData);
        $scope.GetEmpRefSchOptions_ClientOnHoldData_HC(jsonData);
        $scope.GetEmpRefSchOptions_ReferralInfo_HC(jsonData);
        $scope.GetEmpRefSchOptions_ScheduleInfo_HC(jsonData);
    }

    $scope.GetEmpRefSchOptions_PatientVisitFrequency_HC = function (jsonData) {
        $scope.EmpRefSchAjaxStart_PatientVisitFrequency = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmpRefSchOptions_PatientVisitFrequency_HC, jsonData, "Post", "json", "application/json", false).success(function (response) {
            $scope.SearchModel.isLoading = false;
            if (response.IsSuccess) {
                $scope.PatientPayorList = response.Data.PatientPayorList;
                $scope.CareTypeList = response.Data.CareTypeList;

                if ($scope.PatientPayorList !== undefined && $scope.PatientPayorList.length === 1) {
                    $scope.SearchModel.PayorID = $scope.PatientPayorList[0].PayorID;
                }

                if ($scope.CareTypeList !== undefined && $scope.CareTypeList.length === 1) {
                    $scope.SearchModel.CareTypeID = $scope.CareTypeList[0].DDMasterID;
                }
            }
            $scope.EmpRefSchAjaxStart_PatientVisitFrequency = false;
            ShowMessages(response);
        });
    }

    $scope.GetEmpRefSchOptions_ClientOnHoldData_HC = function (jsonData) {
        $scope.EmpRefSchAjaxStart_ClientOnHoldData = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmpRefSchOptions_ClientOnHoldData_HC, jsonData, "Post", "json", "application/json", false).success(function (response) {
            $scope.SearchModel.isLoading = false;
            if (response.IsSuccess) {
                $scope.PatientHoldDetailList = response.Data.PatientHoldDetailList;
            }
            $scope.EmpRefSchAjaxStart_ClientOnHoldData = false;
            ShowMessages(response);
        });
    }

    $scope.GetEmpRefSchOptions_ReferralInfo_HC = function (jsonData) {
        if (!ValideElement($scope.SearchModel.StartDate) || !ValideElement($scope.SearchModel.EndDate))
            return false;
        $scope.EmpRefSchAjaxStart_ReferralInfo = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmpRefSchOptions_ReferralInfo_HC, jsonData, "Post", "json", "application/json", false).success(function (response) {
            $scope.SearchModel.isLoading = false;
            if (response.IsSuccess) {
                $scope.PatientDetail = response.Data.PatientDetail;

                if (ValideElement($scope.PatientDetail.PatientPayorID) && $scope.PatientDetail.PatientPayorID > 0 && $scope.SearchModel.ScheduleID > 0) {
                    $scope.SearchModel.PayorID = $scope.PatientDetail.PatientPayorID;
                }

                 $scope.RTSMaster = response.Data.RTSMaster;
                var selectedCareTypeID = $scope.SearchModel.CareTypeID;
                if (selectedCareTypeID && selectedCareTypeID > 0 && ($scope.ListRTSDetail.length  === undefined || $scope.ListRTSDetail.length == 0)) {
                    $scope.ListRTSDetail = response.Data.ListRTSDetail.filter(m => m.CareTypeId == selectedCareTypeID);
                }
            }
            $scope.EmpRefSchAjaxStart_ReferralInfo = false;
            ShowMessages(response);
        });
    }

    $scope.GetEmpRefSchOptions_ScheduleInfo_HC = function (jsonData) {
        $scope.EmpRefSchAjaxStart_ScheduleInfo = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmpRefSchOptions_ScheduleInfo_HC, jsonData, "Post", "json", "application/json", false).success(function (response) {
            $scope.SearchModel.isLoading = false;
            if (response.IsSuccess) {
                $scope.ReferralAuthorizationServiceCodeList = response.Data.ReferralAuthorizationServiceCodeList;
                if ($scope.ReferralAuthorizationServiceCodeList !== undefined && $scope.ReferralAuthorizationServiceCodeList.length === 1) {
                    $scope.SearchModel.ReferralBillingAuthorizationID = $scope.ReferralAuthorizationServiceCodeList[0].ReferralBillingAuthorizationID;
                    $scope.PrioAuthorizationList = $scope.ReferralAuthorizationServiceCodeList[0];
                }
                $scope.EmployeeTSList = response.Data.Page.Items;
                $scope.EmployeeTSListPager.currentPageSize = response.Data.Page.Items.length;
                $scope.EmployeeTSListPager.totalRecords = response.Data.Page.TotalItems;
                if (response.Data.Page.Items.length === 0) {
                    ShowMessage("Selected Employee is not active or schedule not available", "error");
                    return false;
                }
            }
            $scope.EmpRefSchAjaxStart_ScheduleInfo = false;
            ShowMessages(response);
        });
    }

    $scope.SetSearchPostData = function (pager) {
        if (!ValideElement($scope.SearchModel.StartDate) || !ValideElement($scope.SearchModel.EndDate))
            return false;

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
      //  $scope.SelectedReferralTimeSlotDetailIDs = [];
    };
    $scope.ResetSearchEmpRefSchOptions();

    $scope.$watch('SearchModel.ReferralBillingAuthorizationID', function (newValue, oldValue, scope) {
        if (!$scope.SearchModel.TimePickerRequire) {
            if (newValue && $scope.PrioAuthorizationList) {
                if (!$scope.SearchModel.OldDates) {
                    $scope.SearchModel.OldDates = {
                        StartDate: $scope.SearchModel.StartDate,
                        EndDate: $scope.SearchModel.EndDate
                    };
                }
                var newStartDate = moment($scope.PrioAuthorizationList.StartDate);
                $scope.SearchModel.StartDate = newStartDate.format();
                var newEndDate = moment($scope.PrioAuthorizationList.EndDate);
                $scope.SearchModel.EndDate = newEndDate.add(1, 'days').add(-1, 'seconds').format();
            } else if ($scope.SearchModel.OldDates) {
                $scope.SearchModel.StartDate = moment($scope.SearchModel.OldDates.StartDate).format();
                $scope.SearchModel.EndDate = moment($scope.SearchModel.OldDates.EndDate).format();
            }
        }
    });

    $scope.$watch('SearchModel.StartDate', function (newValue, oldValue, scope) {
        if ($scope.SearchModel.TimePickerRequire) {
            if ($scope.SearchModel.EndDate == undefined)
                $scope.SearchModel.EndDate = $scope.SearchModel.StartDate;
        } else {
            if ($scope.SearchModel.EndDate != null && $scope.SearchModel.EndDate != undefined) {
                $scope.SearchModel.EndDate = moment($scope.SearchModel.EndDate).format();
            }
            else {
                $scope.SearchModel.EndDate = moment($scope.SearchModel.StartDate).add(365, 'days').format();
            }
            $scope.SearchModel.MaxEndDate = moment($scope.SearchModel.StartDate).add(365, 'days').format();
        }
    }, true);

    $scope.$watch('SearchModel.EndDate', function (newValue, oldValue, scope) {

        if ($scope.SearchModel.isLoading) return;
        if (ValideElement($scope.SearchModel.StartDate) && ValideElement($scope.SearchModel.EndDate)) {

            if (moment(oldValue).format('L') !== moment(newValue).format('L')) {
                $scope.SearchEmpRefSchOptions();
            }
        }

    });

    $scope.CreateBulkSchedule = function (item, isReschedule) {
        if ($scope.SelectedReferralTimeSlotDetailIDs.length === 0) {
            ShowMessage("Please select atleast one day to schedule", "error");
            return false;
        }
        if ($scope.PatientPayorList.length > 0 && !ValideElement($scope.SearchModel.PayorID)) {
            ShowMessage("Please select the patient payor first", "error");
            return false;
        }

        if ($scope.SearchModel.CareTypeID === undefined) {
            ShowMessage("Please select care type first", "error");
            return false;
        }

        if ($scope.ListRTSDetail === undefined || $scope.ListRTSDetail === null || $scope.ListRTSDetail.length === 0) {
            bootboxDialog(function (result) {
                if (result) {
                    window.location.href = HomeCareSiteUrl.GetCarePlanCaretype;
                }
            }, bootboxDialogType.Confirm, "Alert", "The time slot that you are trying to schedule has no caretype assigned, click here on yes,continue to assign caretype to the visit.", bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
            return false;
        }

        $scope.SearchModel.EmployeeID = item.EmployeeID;
        $scope.SearchModel.StartTimes = item.StartTime;
        $scope.SearchModel.EndTimes = item.EndTime;
        $scope.SearchModel.EmployeeTimeSlotDetailIDs = item.EmployeeTimeSlotDetailIDs;
        $scope.SearchModel.ReferralTimeSlotDetailIDs = $scope.SelectedReferralTimeSlotDetailIDs ? $scope.SelectedReferralTimeSlotDetailIDs.toString() : "";
        $scope.SearchModel.IsRescheduleAction = ValideElement(isReschedule);

        var jsonData = $scope.SetSearchPostData($scope.EmployeeTSListPager);
        AngularAjaxCall($http, HomeCareSiteUrl.CreateBulkScheduleUrl, jsonData, "Post", "json", "application/json", true).success(function (response) {
            if (response.Data == -4) {
                bootboxDialog(function (result) {
                    if (result) {
                        $scope.SearchModel.IsForcePatientSchedules = true;
                        jsonData = $scope.SetSearchPostData($scope.EmployeeTSListPager);
                        AngularAjaxCall($http, HomeCareSiteUrl.CreateBulkScheduleUrl, jsonData, "Post", "json", "application/json", true).success(function (response) {
                            if (response.IsSuccess) {
                                $scope.SearchEmpRefSchOptions();
                                $scope.SchedulesCreated = true;
                                $scope.SearchModel.IsForcePatientSchedules = false;
                            }
                            ShowMessages(response);
                        });
                    }
                }, bootboxDialogType.Confirm, "Alert", "The Schedule Time is more than allowed time. Would you still want to schedule? ", bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
            } else {
                if (response.IsSuccess) {
                    $scope.SearchEmpRefSchOptions();
                    $scope.SchedulesCreated = true;
                }
                ShowMessages(response);
            }
        });
    }

    $scope.CallOnPopUpLoad = function (referralid, startDate, event, endDate, CareTypeId, referralTimeSlotMasterID) {
        debugger
        $scope.GetPageLoadModels();
        $scope.SchedulesCreated = false;
        $scope.SearchModel = {};
        $scope.SearchModel.isLoading = true;
        $scope.SearchModel.ReferralID = referralid;
        $scope.SearchModel.ScheduleID = 0;
        $scope.SearchModel.CareTypeID = CareTypeId;
        $scope.SearchModel.ReferralTimeSlotMasterID = referralTimeSlotMasterID;
        $scope.SearchModel.IsVirtualVisit = false;
        $scope.ListRTSDetail = {}
        $scope.SearchModel.StartDate = moment(startDate);
        $scope.SearchModel.EndDate = endDate != undefined ? moment(endDate) : moment(startDate).add(365, 'days').format();
        if (endDate != null && endDate != undefined && endDate != "Invalid date") {
            $scope.SearchModel.EndDate = moment(endDate).format();
        }
        $scope.SearchModel.MaxEndDate = moment($scope.SearchModel.StartDate).add(365, 'days').format();
        $scope.SearchModel.SameDateWithTimeSlot = false;
        $scope.SearchModel.TimePickerRequire = false;
        $scope.ShowPatientDeniedService = false;
        if (event) {

            $scope.SearchModel.SameDateWithTimeSlot = true;
            $scope.SearchModel.TimePickerRequire = true;

            $scope.SearchModel.ScheduleID = event.scheduleModel.ScheduleID;
            $scope.SearchModel.ReferralTSDateID = event.scheduleModel.ReferralTSDateID;
            var RBAId = event.scheduleModel.ReferralBillingAuthorizationID;
            if (RBAId > 0) { $scope.SearchModel.ReferralBillingAuthorizationID = RBAId; }
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

            $scope.EmployeeTSListPager.sortIndexArray = [$scope.ConflictsVal, $scope.PreferencesVal, $scope.MilesVal, $scope.SkillsVal];
        }, 500);

        if (!ValideElement(CareTypeId)) {
            $scope.GetCareTypeOptions(function (res) {
                if (res.IsSuccess) {
                    $scope.GetEmpRefSchOptions();
                }
            });
        } else {
            $scope.GetEmpRefSchOptions();
        }
    }

    $scope.GetPageLoadModels = function () {
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmpRefSchPageModelUrl, {}, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.Skills = response.Data.Skills;
                $scope.Preferences = response.Data.Preferences;
            }
            ShowMessages(response);
        });

    };

    $scope.SearchEmpRefMatchModel = {};
    $scope.SchEmpRefSkillsUrl = HomeCareSiteUrl.GetSchEmpRefSkillsUrl;
    $scope.GetAssignedEmployeesURL = HomeCareSiteUrl.GetAssignedEmployeesURL;

    $scope.SchEmpRefPreferencesUrl = "";
    $scope.SchEmpRefConflictsUrl = "";

    $scope.DeleteSchedules = function (data) {
        bootboxDialog(function (result) {
            if (result) {
                $scope.DeleteEmpRefScheduleModel = {};
                $scope.DeleteEmpRefScheduleModel.StartDate = moment($scope.SearchModel.StartDate);
                $scope.DeleteEmpRefScheduleModel.EndDate = moment($scope.SearchModel.EndDate);
                $scope.DeleteEmpRefScheduleModel.ReferralTimeSlotDetailID = data.ReferralTimeSlotDetailID;
                $scope.DeleteEmpRefScheduleModel.ReferralTimeSlotMasterID = data.ReferralTimeSlotMasterID;
                $scope.DeleteEmpRefScheduleModel.Day = data.Day;
                var jsonData = angular.toJson($scope.DeleteEmpRefScheduleModel);
                $scope.PatientRefSchAjaxStart = true;
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteEmpRefScheduleURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
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
        $scope.ReferralTimeSlotModel.Day = data.Day;
    }
    $scope.OnPatientDaySelection = function (item) {
        if (item.IsChecked)
            $scope.SelectedReferralTimeSlotDetailIDs.push(item.ReferralTimeSlotDetailID);
        else
            $scope.SelectedReferralTimeSlotDetailIDs.remove(item.ReferralTimeSlotDetailID);
    }

    $scope.OnPatientDaySelectionAll = function (items) {
        $scope.SelectedReferralTimeSlotDetailIDs = [];
        items.forEach(function (item) {
            item.IsChecked = $scope.IsCheckedFrequencyAll ? true : false;

            if (item.IsChecked)
                $scope.SelectedReferralTimeSlotDetailIDs.push(item.ReferralTimeSlotDetailID);
        });
    };

    $scope.ClosePatientHoldModal = function () {
        $("#pationOnHoldModal").modal('hide');
        HideErrors("#modalHoldFrom");
    }
    $scope.OpenPatientHoldModal = function (isHoldAction, patientHoldDetailModel, unHoldRemoveAction) {
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
    }

    $scope.CloseRemoveScheduleModal = function () {
        $("#removeScheduleModal").modal('hide');
        HideErrors("#modalRemoveScheduleForm");
    }

    $scope.OnHoldUnHoldAction = function (item) {
        var isValid = CheckErrors($("#modalHoldFrom"));
        if (isValid) {
            $scope.PatientHoldDetail.ReferralID = $scope.PatientDetail.ReferralID;
            $scope.PatientHoldDetail.UnHoldDate = moment($scope.PatientHoldDetail.UnHoldDate);
            var jsonData = angular.toJson($scope.PatientHoldDetail);
            AngularAjaxCall($http, HomeCareSiteUrl.OnHoldUnHoldActionURL, jsonData, "Post", "json", "application/json").success(function (response) {
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
            AngularAjaxCall($http, HomeCareSiteUrl.RemoveScheduleURL, jsonData, "Post", "json", "application/json").success(function (response) {
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
