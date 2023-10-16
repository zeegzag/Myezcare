var vm;

controllers.EmployeeListController = function ($scope, $http, $timeout) {
    vm = $scope;
    $scope.AddEmployeeURL = SiteUrl.AddEmployeeURL;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetEmployeeListPage").val());
    };

    $scope.EmployeeList = [];
    $scope.SelectedEmployeeIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.EmployeeModel = $.parseJSON($("#hdnSetEmployeeListPage").val());
    $scope.SearchEmployeeModel = $scope.EmployeeModel.SearchEmployeeModel;
    $scope.TempSearchEmployeeModel = $scope.EmployeeModel.SearchEmployeeModel;
    $scope.EmployeeListPager = new PagerModule("Name");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchEmployeeModel: $scope.SearchEmployeeModel,
            pageSize: $scope.EmployeeListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.EmployeeListPager.sortIndex,
            sortDirection: $scope.EmployeeListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.LoggedInUserId = window.LoggedInID;

    $scope.SearchModelMapping = function () {

        $scope.SearchEmployeeModel = $.parseJSON(angular.toJson($scope.TempSearchEmployeeModel));
        //$scope.SearchEmployeeModel.Name = $scope.TempSearchDepartmentModel.Name;
        //$scope.SearchEmployeeModel.Email = $scope.TempSearchDepartmentModel.Email;
        //$scope.SearchEmployeeModel.DepartmentID = $scope.TempSearchDepartmentModel.DepartmentID;
        //$scope.SearchEmployeeModel.IsSupervisor = $scope.TempSearchDepartmentModel.IsSupervisor;
        //$scope.SearchEmployeeModel.ListOfIdsInCSV = $scope.TempSearchDepartmentModel.ListOfIdsInCSV;

    };

    $scope.GetEmployeeList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedEmployeeIds = [];
        $scope.SelectAllCheckbox = false;
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.EmployeeListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetEmployeeListURL, jsonData, "Post", "json", "application/json").success(function (response) {

            if (response.IsSuccess) {
                $scope.EmployeeList = response.Data.Items;

                $scope.EmployeeListPager.currentPageSize = response.Data.Items.length;
                $scope.EmployeeListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.EmployeeListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            $scope.SearchEmployeeModel = $scope.newInstance().SearchEmployeeModel;
            $scope.TempSearchEmployeeModel = $scope.newInstance().SearchEmployeeModel;
            //$scope.SearchEmployeeModel.IsDeleted = 0;
            $scope.TempSearchEmployeeModel.IsDeleted = "0";
            //$scope.SearchEmployeeModel.IsSupervisor = "-1";            
            //$scope.TempSearchEmployeeModel.IsSupervisor = "-1";


            $scope.EmployeeListPager.currentPage = 1;
            $scope.EmployeeListPager.getDataCallback();
        });
    };

    $scope.SearchEmployee = function () {
        $scope.EmployeeListPager.currentPage = 1;
        $scope.EmployeeListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectEmployee = function (employee) {
        if (employee.IsChecked)
            $scope.SelectedEmployeeIds.push(employee.EmployeeID);
        else
            $scope.SelectedEmployeeIds.remove(employee.EmployeeID);

        if ($scope.SelectedEmployeeIds.length == $scope.EmployeeListPager.currentPageSize)
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

    $scope.DeleteEmployee = function (employeeId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchEmployeeModel.ListOfIdsInCsv = employeeId > 0 ? employeeId.toString() : $scope.SelectedEmployeeIds.toString();

                if (employeeId > 0) {
                    if ($scope.EmployeeListPager.currentPage != 1)
                        $scope.EmployeeListPager.currentPage = $scope.EmployeeList.length === 1 ? $scope.EmployeeListPager.currentPage - 1 : $scope.EmployeeListPager.currentPage;
                } else {

                    if ($scope.EmployeeListPager.currentPage != 1 && $scope.SelectedEmployeeIds.length == $scope.EmployeeListPager.currentPageSize)
                        $scope.EmployeeListPager.currentPage = $scope.EmployeeListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedEmployeeIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.EmployeeListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteEmployeeURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.EmployeeList = response.Data.Items;
                        $scope.EmployeeListPager.currentPageSize = response.Data.Items.length;
                        $scope.EmployeeListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };
    $scope.EmployeeListPager.getDataCallback = $scope.GetEmployeeList;
    $scope.EmployeeListPager.getDataCallback();
};
controllers.EmployeeListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowEmployeeMessage");
});
