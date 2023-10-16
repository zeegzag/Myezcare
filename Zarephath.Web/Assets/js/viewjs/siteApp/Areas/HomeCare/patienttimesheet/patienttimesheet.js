var vm;
controllers.PatientTimeSheetController = function ($scope, $http, $timeout) {
    vm = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetPatientTimeSheetListPage").val());
    };
    
    $scope.PatientTimeSheetList = [];
    $scope.PatientTimeSheetModel = $.parseJSON($("#hdnSetPatientTimeSheetListPage").val());
    
    $scope.SearchPatientTimeSheetListPage = $scope.PatientTimeSheetModel.SearchPatientTimeSheetListPage;
    $scope.TempSearchPatientTimeSheetListPage = $scope.PatientTimeSheetModel.SearchPatientTimeSheetListPage;

    $scope.PatientTimeSheetListPager = new PagerModule("EmployeeVisitID");

    $scope.SetPostData = function (fromIndex) {

        var pagermodel = {
            SearchPatientTimeSheetListPage: $scope.SearchPatientTimeSheetListPage,
            pageSize: $scope.PatientTimeSheetListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.PatientTimeSheetListPager.sortIndex,
            sortDirection: $scope.PatientTimeSheetListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function ()
    {
        if ($scope.TempSearchPatientTimeSheetListPage.EmployeeIDs) {
            $scope.TempSearchPatientTimeSheetListPage.EmployeeIDs = $scope.TempSearchPatientTimeSheetListPage.EmployeeIDs.toString();
        }
        $scope.SearchPatientTimeSheetListPage = $.parseJSON(angular.toJson($scope.TempSearchPatientTimeSheetListPage));
    };

    $scope.GetPatientTimeSheetList = function (isSearchDataMappingRequire) {
        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping
        
        var jsonData = $scope.SetPostData($scope.PatientTimeSheetListPager.currentPage);
        
        AngularAjaxCall($http, HomeCareSiteUrl.GetPatientTimeSheetList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.PatientTimeSheetList = response.Data.Items;
                $scope.PatientTimeSheetListPager.currentPageSize = response.Data.Items.length;
                $scope.PatientTimeSheetListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };
    $scope.Refresh = function () {
        $scope.PatientTimeSheetListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            $scope.SearchPatientTimeSheetListPage = $scope.newInstance().SearchPatientTimeSheetListPage;
            $scope.TempSearchPatientTimeSheetListPage = $scope.newInstance().SearchPatientTimeSheetListPage;
            $scope.TempSearchPatientTimeSheetListPage.IsDeleted = "0";
            $scope.PatientTimeSheetListPager.currentPage = 1;
            $scope.PatientTimeSheetListPager.getDataCallback();
        });
    };

    $scope.SearchEmployeeVisit = function () {
        $scope.PatientTimeSheetListPager.currentPage = 1;
        $scope.PatientTimeSheetListPager.getDataCallback(true);
    };

    $scope.PatientTimeSheetListPager.getDataCallback = $scope.GetPatientTimeSheetList;
    $scope.PatientTimeSheetListPager.getDataCallback();
    
};
controllers.PatientTimeSheetController.$inject = ['$scope', '$http', '$timeout'];
$(document).ready(function () {
    $(".dateInputMask").attr("placeholder", "mm/dd/yyyy");
    $('.time').inputmask({
        mask: "h:s t\\m",
        placeholder: "hh:mm a",
        alias: "datetime",
        hourFormat: "12"
    });
});