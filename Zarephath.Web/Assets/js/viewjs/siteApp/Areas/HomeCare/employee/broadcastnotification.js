var custModel;
controllers.SendBulkSmsController = function ($scope, $http, $timeout) {
    custModel = $scope;
    $scope.SendBulkSmsModel = $.parseJSON($("#hdnSendBulkSmsModel").val());
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSendBulkSmsModel").val());
    };
    $scope.SearchSBSEmployeeModel = $scope.newInstance().SearchSBSEmployeeModel;
    $scope.SendSMSModel = $scope.newInstance().SendSMSModel;
    $scope.PatientModel = $scope.newInstance().PatientModel;
    $scope.IsModelDetailBind = $scope.newInstance().IsModelDetailBind;
    $scope.ScheduleNotificationMessageContent = $scope.newInstance().ScheduleNotificationMessageContent;
    $scope.EmployeeList = [];
    $scope.SelectedEmployeeIds = [];
    $scope.SelectAllCheckbox = false;
    if ($scope.IsModelDetailBind) {
        $scope.SendSMSModel.Message = $scope.ScheduleNotificationMessageContent;
    }
    
    $scope.GetEmployeeList = function () {

        $scope.SelectedEmployeeIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchSBSEmployeeModel.EncryptedId = $scope.PatientModel.EncryptedId;
        var jsonData = angular.toJson({
            model: $scope.SearchSBSEmployeeModel
        });
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeListForSendSmsUrl, jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {

                    $scope.EmployeeList = [];
                    $scope.EmployeeList = response.Data;

                } else {
                    ShowMessages(response);
                }

            });

    };
    $scope.GetEmployeeList();

    // This executes when select single checkbox selected in table.
    $scope.SelectEmployee = function (employee) {
        if (employee.IsChecked)
            $scope.SelectedEmployeeIds.push(employee.EmployeeID);
        else
            $scope.SelectedEmployeeIds.remove(employee.EmployeeID);

        if ($scope.SelectedEmployeeIds.length == $scope.EmployeeList.length)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedEmployeeIds = [];

        angular.forEach($scope.EmployeeList, function (item, key) {

            item.IsChecked = $scope.SelectAllCheckbox;// event.target.checked;
            if (item.IsChecked)
                $scope.SelectedEmployeeIds.push(item.EmployeeID);
        });

        return true;
    };

    $scope.RemoveAllSelected = function () {
        $scope.SelectedEmployeeIds = [];

        angular.forEach($scope.EmployeeList, function (item, key) {
            item.IsChecked = false;
        });

        return true;
    };

    $scope.SendBroadcastNotification = function () {
        if ($scope.SelectedEmployeeIds.length === 0) {
            toastr.error("Please select employees to send notification");
            return false;
        }

        if (ValideElement($scope.SendSMSModel.Message) === false) {
            toastr.error("Please enter message to send the employees");
            return false;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SendSMSModel.EmployeeIds = $scope.SelectedEmployeeIds.toString();
                $scope.SendSMSModel.EncryptedId = $scope.PatientModel.EncryptedId;
                var jsonData = angular.toJson({
                    model: $scope.SendSMSModel
                });
                AngularAjaxCall($http, HomeCareSiteUrl.BroadcastNotificationUrl, jsonData, "post", "json", "application/json", true).
                    success(function (response, status, headers, config) {
                        
                        if (response.IsSuccess) {
                            $scope.SelectedEmployeeIds = [];
                            $scope.SelectAllCheckbox = false;
                            $scope.RemoveAllSelected();
                            $scope.SendSMSModel = $scope.newInstance().SendSMSModel;
                        } else {
                            ShowMessages(response);
                        }
                        ShowMessages(response);
                    });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, window.ConfirmBroadcastNotification, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    //#region View Broadcast notification LOG TAB

    $scope.SentBroadcastNotificationListPager = new PagerModule("NotificationId", null, "DESC");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchSentSMSModel: $scope.SearchSentBroadcastNotificationModel,
            pageSize: $scope.SentBroadcastNotificationListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.SentBroadcastNotificationListPager.sortIndex,
            sortDirection: $scope.SentBroadcastNotificationListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchSentBroadcastNotificationModel = angular.copy($scope.TempSearchSentSMSModel); //$.parseJSON(angular.toJson($scope.TempSearchSentSMSModel));
    };

    $scope.SentBroadcastNotificationList = function (isSearchDataMappingRequire) {
        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.SentBroadcastNotificationListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetBroadcastNotificationUrl, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.SentBroadcastNotificationList = response.Data.Items;
                $scope.SentBroadcastNotificationListPager.currentPageSize = response.Data.Items.length;
                $scope.SentBroadcastNotificationListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };
    $scope.SentBroadcastNotificationListPager.getDataCallback = $scope.SentBroadcastNotificationList;

    $scope.EmployeesListForBroadcastNotification = [];
    $scope.ModalAjaxStart = false;
    $scope.GetEmployeesForBroadcastNotification = function (id, notificationtype) {
        $scope.DisplaySearchFilter = (notificationtype == 2) ? true : false;
        $scope.EmployeesListForBroadcastNotification = [];
        $("#empSentSmsModal").modal("show");
        $scope.ModalAjaxStart = true;
        
        $scope.SearchSBSEmployeeModel.NotificationId = id;
        $scope.SearchSBSEmployeeModel.ScheduleNotificationAction = $scope.SearchSBSEmployeeModel.ScheduleNotificationAction == null ? '-1' : $scope.SearchSBSEmployeeModel.ScheduleNotificationAction;
        //var jsonData = angular.toJson({ notificationId: id });
        var jsonData = angular.toJson($scope.SearchSBSEmployeeModel);

        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeesForBroadcastNotificationsUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.EmployeesListForBroadcastNotification = [];
                $scope.EmployeesListForBroadcastNotification = response.Data;
            }
            ShowMessages(response);
            $scope.ModalAjaxStart = false;
        });
    };

    $('#empSentSmsModal').on('hidden.bs.modal', function (e) {
        $scope.SearchSBSEmployeeModel.NotificationId = null;
        $scope.SearchSBSEmployeeModel.ScheduleNotificationAction = null;
    });

    $scope.SearchEmployee = function () {
        $scope.GetEmployeesForBroadcastNotification($scope.SearchSBSEmployeeModel.NotificationId,2);
    }

    $("a#tabBroadcastNotificationSms").on('shown.bs.tab', function (e, ui) {
        $scope.SentBroadcastNotificationList();
    });

    $scope.calculateAge = function (selecteddate) { // pass in player.dateOfBirth

        if (selecteddate != null && selecteddate != '') {

            //Get 1 day in milliseconds
            // var one_day = 1000 * 60 * 60 * 24;
            // 86400000 milli second = 1 Day
            // 31536000000 milli second = 1 Year
            // 2628000000 milli second = 1 month

            var diff_date = new Date() - new Date(selecteddate);

            var num_years = diff_date / 31536000000;
            var num_months = (diff_date % 31536000000) / 2628000000;
            var num_days = ((diff_date % 31536000000) % 2628000000) / 86400000;

            return Math.floor(num_years) + 'Y ' + Math.floor(num_months) + 'M '; // + Math.floor(num_days) + 'D';
        }
        return "";
    };

    //#endregion    
};
controllers.SendBulkSmsController.$inject = ['$scope', '$http', '$timeout'];
