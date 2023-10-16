var vm;
controllers.EmployeeBillingReportController = function ($scope, $http, $timeout) {
    vm = $scope;
    $scope.newInstance = function () { 
        return $.parseJSON($("#hdnEmployeeBillingReport").val());
    };

    $scope.EmployeeBillingReportList = [];
    $scope.SelectedEmployeeBillingReport = [];
    $scope.SelectAllCheckbox = false;

    $scope.EmployeeBillingReportModel = $.parseJSON($("#hdnEmployeeBillingReport").val());
    $scope.SearchEmployeeBillingReportListPage = $scope.EmployeeBillingReportModel.SearchEmployeeBillingReportListPage;
    $scope.TempSearchEmployeeBillingReportListPage = $scope.EmployeeBillingReportModel.SearchEmployeeBillingReportListPage;
    $scope.EmployeeBillingReportListPager = new PagerModule("EmployeeID", "", "DESC");

    
    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            SearchEmployeeBillingReportListPage: $scope.SearchEmployeeBillingReportListPage,
            pageSize: $scope.EmployeeBillingReportListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.EmployeeBillingReportListPager.sortIndex,
            sortDirection: $scope.EmployeeBillingReportListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchEmployeeBillingReportListPage = $.parseJSON(angular.toJson($scope.TempSearchEmployeeBillingReportListPage));
    };

    $scope.GetEmployeeBillingReportList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedEmployeeBillingReport = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchEmployeeBillingReportListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control
        
        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();

        var jsonData = $scope.SetPostData($scope.EmployeeBillingReportListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.EmployeeBillingReportListURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
            if (response.IsSuccess) {
                $scope.EmployeeBillingReportList = response.Data.Items;
                $scope.EmployeeBillingReportListPager.currentPageSize = response.Data.Items.length;
                $scope.EmployeeBillingReportListPager.totalRecords = response.Data.TotalItems;
            }
           ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.EmployeeBillingReportListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        //Reset 
        $scope.SearchEmployeeBillingReportListPage = $scope.newInstance().SearchEmployeeBillingReportListPage;
        $scope.TempSearchEmployeeBillingReportListPage = $scope.newInstance().SearchEmployeeBillingReportListPage;
        $scope.EmployeeBillingReportListPager.currentPage = 1;
        $scope.EmployeeBillingReportListPager.getDataCallback();
    };

    $scope.SearchEmployeeBillingReport = function () {
        $scope.EmployeeBillingReportListPager.currentPage = 1;
        $scope.EmployeeBillingReportListPager.getDataCallback(true);
    };

    $scope.ExportToCSV = function (event) { 
        var e = document.getElementById("PayFrequency");
        var payFrequency = e.options[e.selectedIndex].value;
        if (payFrequency !== '') {
            var jsonData = angular.toJson({ employeeBillingReportDetails: $scope.EmployeeBillingReportList, SearchEmployeeBillingReportListPage: $scope.SearchEmployeeBillingReportListPage, PayFrequency: payFrequency });

            AngularAjaxCall($http, HomeCareSiteUrl.ExportToCSV, jsonData, "Post", "json", "application/json", true).success(function (response) {
                window.open(window.location.origin + '/assets/files/EmployeeBillingReport.csv', '_blank');

                event.preventDefault();
            });
        }
        else {
            alert('Please select Pay Frequency to export employee details.');
        }
    };


    // This executes when select single checkbox selected in table.
    $scope.SelectReconcile835File = function (item) {
        if (item.IsChecked)
            $scope.SelectedEmployeeBillingReport.push(item.EmployeeID);
        else
            $scope.SelectedEmployeeBillingReport.remove(item.EmployeeID);

        if ($scope.SelectedEmployeeBillingReport.length == $scope.EmployeeBillingReportListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedEmployeeBillingReport = [];
        angular.forEach($scope.EmployeeBillingReportList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedEmployeeBillingReport.push(item.EmployeeID);
        });
        return true;
    };

    $scope.EmployeeBillingReportListPager.getDataCallback = $scope.GetEmployeeBillingReportList;
    $scope.EmployeeBillingReportListPager.getDataCallback();

};
controllers.EmployeeBillingReportController.$inject = ['$scope', '$http', '$timeout'];


$(document).ready(function () {
    var dateformat = GetOrgDateFormat();
    $(".dateInputMask").attr("placeholder", dateformat);
    $('.time').inputmask({
        mask: "h:s t\\m",
        placeholder: "hh:mm a",
        alias: "datetime",
        hourFormat: "12"
    });

});