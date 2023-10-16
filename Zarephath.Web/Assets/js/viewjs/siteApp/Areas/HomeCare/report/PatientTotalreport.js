var vm;
controllers.PatientTotalReportController = function ($scope, $http, $timeout,$window) {
    vm = $scope;
    

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnPatientTotalReport").val());
    };

    $scope.PatientTotalReportList = [];
    $scope.SelectedPatientTotalReport = [];

    $scope.PatientTotalReportModel = $.parseJSON($("#hdnPatientTotalReport").val());
    $scope.SearchPatientTotalReportListPage = $scope.PatientTotalReportModel.SearchPatientTotalReportListPage;
    $scope.TempSearchPatientTotalReportListPage = $scope.PatientTotalReportModel.SearchPatientTotalReportListPage;
    $scope.PatientTotalReportListPager = new PagerModule("ReferralID", "", "DESC");

    
    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            SearchPatientTotalReportListPage: $scope.SearchPatientTotalReportListPage,
            pageSize: $scope.PatientTotalReportListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.PatientTotalReportListPager.sortIndex,
            sortDirection: $scope.PatientTotalReportListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchPatientTotalReportListPage = $.parseJSON(angular.toJson($scope.TempSearchPatientTotalReportListPage));
    };

    $scope.GetPatientTotalReportList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedPatientTotalReport = [];
        $scope.SearchPatientTotalReportListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control
        
        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();

        var jsonData = $scope.SetPostData($scope.PatientTotalReportListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.PatientTotalReportListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.PatientTotalReportList = response.Data.Items;
                $window.localStorage.setItem('saved', response.Data.Items);
                $scope.PatientTotalReportListPager.currentPageSize = response.Data.Items.length;
                $scope.PatientTotalReportListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.ResetSearchFilter = function () {
        //Reset 
        $scope.SearchPatientTotalReportListPage = $scope.newInstance().SearchPatientTotalReportListPage;
        $scope.TempSearchPatientTotalReportListPage = $scope.newInstance().SearchPatientTotalReportListPage;
        $scope.PatientTotalReportListPager.currentPage = 1;
        $scope.PatientTotalReportListPager.getDataCallback();
    };

    $scope.SearchPatientTotalReport = function () {
        $scope.PatientTotalReportListPager.currentPage = 1;
        $scope.StartDate = $scope.TempSearchPatientTotalReportListPage.StartDate;
        $scope.EndDate = $scope.TempSearchPatientTotalReportListPage.EndDate;
        $scope.DateRange = "StartDate="+$scope.TempSearchPatientTotalReportListPage.StartDate+"&EndDate="+$scope.TempSearchPatientTotalReportListPage.EndDate;

        $scope.PatientTotalReportListPager.getDataCallback(true);
    };


    //// This executes when select single checkbox selected in table.
    //$scope.SelectReconcile835File = function (item) {
    //    if (item.IsChecked)
    //        $scope.SelectedPatientTotalReport.push(item.EmployeeID);
    //    else
    //        $scope.SelectedPatientTotalReport.remove(item.EmployeeID);
    //};

    //$scope.Export = function () {
    //    
    //    html2canvas(document.getElementById('tblCustomers'), {
    //        onrendered: function (canvas) {
    //            var data = canvas.toDataURL();
    //            var docDefinition = {
    //                content: [{
    //                    image: data,
    //                    width: 800
    //                }]
    //            };
    //            pdfMake.createPdf(docDefinition).download("ActivePatientDetail.pdf");
    //        }
    //    });
    //}

    $scope.GeneratePatientActivePdfURL = function () {
        
        $scope.SelectedPatientTotalReport = [];
        $scope.SearchPatientTotalReportListPage.ListOfIdsInCSV = [];
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        var jsonData = $scope.SetPostData($scope.PatientTotalReportListPager.currentPage);
        var jsonData1 = angular.toJson($scope.PatientTotalReportModel);
        AngularAjaxCall($http, HomeCareSiteUrl.GeneratePatientActivePdfURL, jsonData, "Get", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                window.location = '/hc/report/GeneratePatientActivePdf';
            }
            ShowMessages(response);
        });
    };

    $scope.PatientTotalReportListPager.getDataCallback = $scope.GetPatientTotalReportList;
    $scope.PatientTotalReportListPager.getDataCallback();

};
controllers.PatientTotalReportController.$inject = ['$scope', '$http', '$timeout','$window'];


$(document).ready(function () {
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});