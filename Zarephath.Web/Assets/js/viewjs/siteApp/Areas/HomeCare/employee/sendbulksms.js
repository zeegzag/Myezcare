var custModel;


controllers.SendBulkSmsController = function ($scope, $http, $timeout) {

    custModel = $scope;

    $scope.SendBulkSmsModel = $.parseJSON($("#hdnSendBulkSmsModel").val());

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSendBulkSmsModel").val());
    };

    $scope.SearchSBSEmployeeModel = $scope.newInstance().SearchSBSEmployeeModel;
    $scope.SendSMSModel = $scope.newInstance().SendSMSModel;

    $scope.EmployeeList = [];
    $scope.SelectedEmployeeIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.GetEmployeeList = function () {
        
        $scope.SelectedEmployeeIds = [];
        $scope.SelectAllCheckbox = false;

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

    $scope.SendBulkSMS = function () {
        if ($scope.SelectedEmployeeIds.length===0) {
            toastr.error("Please select employees to send sms");
            return false;
        }

        if (ValideElement($scope.SendSMSModel.Message) === false) {
            toastr.error("Please enter message to send the employees");
            return false;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SendSMSModel.EmployeeIds = $scope.SelectedEmployeeIds.toString();
                var jsonData = angular.toJson({
                    model: $scope.SendSMSModel
                });
                AngularAjaxCall($http, HomeCareSiteUrl.SendBulkSMSUrl, jsonData, "post", "json", "application/json", true).
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
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, window.ConfirmSendSMS, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    //#region View SMS LOG TAB

    $scope.SentSmsListPager = new PagerModule("GroupMessageLogID",null,"DESC");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchSentSMSModel: $scope.SearchSentSMSModel,
            pageSize: $scope.SentSmsListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.SentSmsListPager.sortIndex,
            sortDirection: $scope.SentSmsListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchSentSMSModel = $.parseJSON(angular.toJson($scope.TempSearchSentSMSModel));
    };

    $scope.SentSMSList = function (isSearchDataMappingRequire) {
        //
        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.SentSmsListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetSentSmsListUrl, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.SentSmsList = response.Data.Items;
                $scope.SentSmsListPager.currentPageSize = response.Data.Items.length;
                $scope.SentSmsListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };
    $scope.SentSmsListPager.getDataCallback = $scope.SentSMSList;

    //$scope.RefreshSentSMS = function () {
    //    $scope.SentSmsListPager.getDataCallback();
    //};
    //$scope.ResetSentSMSSearchFilter = function () {
    //    $timeout(function () {
    //        $scope.SearchEmployeeModel = $scope.newInstance().SearchEmployeeModel;
    //        $scope.TempSearchEmployeeModel = $scope.newInstance().SearchEmployeeModel;
    //        //$scope.SearchEmployeeModel.IsDeleted = 0;
    //        $scope.TempSearchEmployeeModel.IsDeleted = "0";
    //        //$scope.SearchEmployeeModel.IsSupervisor = "-1";            
    //        //$scope.TempSearchEmployeeModel.IsSupervisor = "-1";


    //        $scope.EmployeeListPager.currentPage = 1;
    //        $scope.EmployeeListPager.getDataCallback();
    //    });
    //};
    //$scope.SearchSentSmsList = function () {
    //    $scope.SentSmsListPager.currentPage = 1;
    //    $scope.SentSmsListPager.getDataCallback(true);
    //};

    $scope.EmployeesListForSentSMS = [];
    $scope.ModalAjaxStart = false;
    $scope.GetEmployeesForSentSMS = function (id) {
        $scope.EmployeesListForSentSMS = [];
        $("#empSentSmsModal").modal("show");
        $scope.ModalAjaxStart = true;
        var jsonData = angular.toJson({ groupMessageLogId: id });
       AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeesForSentSMSUrl, jsonData, "Post", "json", "application/json",false).success(function (response) {
           if (response.IsSuccess) {
               $scope.EmployeesListForSentSMS = [];
               $scope.EmployeesListForSentSMS = response.Data;
            }
           ShowMessages(response);
           $scope.ModalAjaxStart = false;
        });
    };

    $("a#tabsentsms").on('shown.bs.tab', function (e, ui) {
        $scope.SentSMSList();
    });
    //#endregion
};

controllers.SendBulkSmsController.$inject = ['$scope', '$http', '$timeout'];



jQuery(document).ready(function () {

    //if (ValideElement(custModel.EmployeeModel.Employee.Latitude))
    //    custModel.GenerateGeoCode(true);

    //ShowPageLoadMessage("ShowEmployeeMessage");

});
