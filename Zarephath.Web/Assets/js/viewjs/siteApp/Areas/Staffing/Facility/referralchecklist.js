var vmPIC;
controllers.ReferralChecklistController = function ($scope, $http, $window, $timeout) {
    vmPIC = $scope;
    $scope.EncryptedReferralID = window.EncryptedReferralID;
    $scope.ChecklistItemTypeMonthlyVisitID = window.checklistTypeMonthlyVisit;
    $scope.ChecklistItemTypeAnnualVisitID = window.checklistTypeAnnualVisit;
    $scope.ChecklistItemTypes = [];
    $scope.CurrentVisitChecklist = {};    
    $scope.ChecklistItemModel = {
        EndDate: moment(new Date())
    }
    $scope.ChecklistItemModel.StartDate = moment($scope.ChecklistItemModel.EndDate).subtract(365, 'days').format();

    $scope.ResetControls = function () {
        $scope.ChecklistItems = [];
        $scope.IsAllCheckboxSelected = false;
        $scope.IsVisitChecklistItemsLoaded = false;
        $scope.ChecklistSaved = false;
        $scope.VisitChecklistItems = [];
    }
    $scope.ResetControls();

    $scope.GetChecklistItemTypes = function () {
        if ($scope.ChecklistItemTypes.length == 0) {
            AngularAjaxCall($http, HomeCareSiteUrl.GetChecklistItemTypesURL, {}, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess && response.Data.length > 0) {
                    $scope.ChecklistItemTypes = response.Data;
                    $scope.SelectedChecklistItemTypeID = $scope.ChecklistItemTypes[0].ChecklistItemTypeID;
                    $scope.LoadChecklistItems();
                }
            });
        } else if ($scope.ChecklistItemTypes.length > 0) {
            $scope.SelectedChecklistItemTypeID = $scope.ChecklistItemTypes[0].ChecklistItemTypeID;
            $scope.LoadChecklistItems();
        }
    }

    $scope.OpenVisitChecklistModal = function (item) {
        if (item != undefined) {
            var model = {
                EmployeeVisitID: item.EmployeeVisitID,
                ScheduleID : item.ScheduleID,
                ReferralID: item.ReferralID
            }
            var jsonData = angular.toJson({ model: model });
            AngularAjaxCall($http, HomeCareSiteUrl.GetVisitChecklistItemDetailURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess && response.Data.length > 0) {
                    $scope.CurrentVisitChecklist = response.Data[0];
                    $("#VisitModal").modal("show");
                }
            });
        }
    }

    $scope.ChecklistItemTypeChange = function () {

        $scope.LoadChecklistItems();
    }

    $scope.LoadChecklistItems = function () {
        $scope.ResetControls();
        if ($scope.SelectedChecklistItemTypeID != checklistTypeMonthlyVisit && $scope.SelectedChecklistItemTypeID != checklistTypeAnnualVisit) {
            $scope.GetChecklistItems();
        }
    }

    $scope.GetChecklistItems = function () {
        var model = {
            ChecklistItemTypeID: $scope.SelectedChecklistItemTypeID,
            EncryptedPrimaryID: $scope.EncryptedReferralID
        }
        var jsonData = angular.toJson({ model: model });
        AngularAjaxCall($http, HomeCareSiteUrl.GetChecklistItemsURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.ChecklistItems = response.Data.ChecklistItems;
                if (response.Data.TransactionResult.TransactionResultId == 0) {
                    $scope.$parent.$parent.IsChecklistRemaining = false;
                    $scope.ChecklistSaved = true;
                } else {
                    $scope.$parent.$parent.IsChecklistRemaining = true;
                    $scope.ChecklistSaved = false;
                }
            }
        });
    };

    $scope.GetVisitChecklistItems = function () {
        if (CheckErrors("#frmChecklistSearch", true)) {
            var model = {
                ChecklistItemTypeID: $scope.SelectedChecklistItemTypeID,
                EncryptedPrimaryID: $scope.EncryptedReferralID,
                StartDate: $scope.ChecklistItemModel.StartDate,
                EndDate: $scope.ChecklistItemModel.EndDate,
                VisitType: $scope.SelectedChecklistItemTypeID == window.checklistTypeMonthlyVisit ? window.checklistVisitTypeMonthlyVisit : window.checklistVisitTypeAnnualVisit
            }
            var jsonData = angular.toJson({ model: model });
            AngularAjaxCall($http, HomeCareSiteUrl.GetVisitChecklistItemsURL, jsonData, "Post", "json", "application/json").success(function (response) {
                $scope.IsVisitChecklistItemsLoaded = true;
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.VisitChecklistItems = response.Data;
                }
            });
        }
    };

    $scope.OpenEmpSMSPopup = function (item, phone) {
        if (item.EmployeeID == 0) {
            toastr.error(window.EmployeeNotAssigned);
            return;
        }
        var jsonData = { scheduleId: item.ScheduleID, templateId: window.checklistEmailTemplateId };
        AngularAjaxCall($http, HomeCareSiteUrl.ChecklistGetEmpSMSDetailURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ScheduleSmsModel = response.Data;
                $scope.EmpSmsModel = angular.copy($scope.ScheduleSmsModel);
                if (phone != undefined) {
                    $scope.EmpSmsModel.ToNumber = phone;
                }
                $scope.EmpSmsModel.ParentName = item.ParentName;
                $scope.EmpSmsModel.ReferralID = item.ReferralID;
                $scope.EmpSmsModel.Phone = item.Phone1;
                $('#emp-sms-modal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            } else {
                response.Message = window.ScheduleDetailsMissing;
            }
            ShowMessages(response);
        });
    };

    $scope.SendEmpSMS = function () {
        var isValid = CheckErrors($("#emp-sms"));
        if (isValid) {
            var jsonData = { scheduleSmsModel: $scope.EmpSmsModel };
            AngularAjaxCall($http, HomeCareSiteUrl.ChecklistSendEmpSMSURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $('#emp-sms-modal').modal('hide');
                    $("#frmReferral").data('changed', false);
                }
            });
        }
    };

    $scope.OpenEmpNotificationPopup = function (item) {
        if (item.EmployeeID == 0) {
            toastr.error(window.EmployeeNotAssigned);
            return;
        }
        $scope.SendSMSModel = {};
        $scope.SendSMSModel.EmployeeIds = item.EmployeeID.toString();
        $scope.SendSMSModel.EmployeeName = item.EmployeeName;
        $scope.SendSMSModel.ReferralID = item.ReferralID;
        $scope.SendSMSModel.NotificationType = window.checklistNotificationId;
        $('#emp-notification-modal').modal({
            backdrop: 'static',
            keyboard: false
        });
    };

    $scope.SendEmpNotification = function () {

        if (ValideElement($scope.SendSMSModel.Message) === false) {
            toastr.error("Please enter message to send");
            return false;
        }
        //$scope.SendSMSModel.EmployeeIds = $scope.SelectedEmployeeIds.toString();
        //$scope.SendSMSModel.EncryptedId = $scope.PatientModel.EncryptedId;
        var jsonData = angular.toJson({
            model: $scope.SendSMSModel
        });
        AngularAjaxCall($http, HomeCareSiteUrl.BroadcastNotificationUrl, jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.SendSMSModel = {};
                    $('#emp-notification-modal').modal('hide');
                    $("#frmReferral").data('changed', false);
                }
                ShowMessages(response);
            });
    };

    $scope.ChecklistItemChange = function (itemChecked) {
        //item.IsDocumentUploaded = item.IsChecked;
        var uncheckedMandatoryItems = $.grep($scope.ChecklistItems, function (item) {
            return item.IsMandatory == true && (item.IsChecked == false);
        });
        $scope.IsAllCheckboxSelected = uncheckedMandatoryItems.length == 0;
    }

    $scope.SavePatientIntakeChecklist = function () {
        if ($scope.IsAllCheckboxSelected) {
            var model = {
                ChecklistItems: $scope.ChecklistItems,
                EncryptedPrimaryID: $scope.EncryptedReferralID,
                ChecklistItemTypeID: window.checklistTypePatient
            }
            var jsonData = angular.toJson({ model: model });
            AngularAjaxCall($http, HomeCareSiteUrl.SaveChecklistItemsURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.$parent.$parent.IsChecklistRemaining = false;
                    $scope.ChecklistSaved = true;
                }
            });
        } else {
            toastr.error(window.SelectAllChecklistItems);
        }
    }

    $("a#addReferralDetails_checklistitems").on('shown.bs.tab', function (e) {
        $scope.GetChecklistItemTypes();
    });
};