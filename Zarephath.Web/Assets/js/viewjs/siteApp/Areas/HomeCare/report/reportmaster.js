var rmScope;

controllers.ReportMasterController = function ($scope, $http) {

    rmScope = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdn_ReportModel").val());
    };

    $scope.ReportMasterList = [];
    $scope.GetReportMasterList = function () {
        var jsonData = angular.toJson({});
        AngularAjaxCall($http, HomeCareSiteUrl.GetReportMasterListUrl, jsonData, "Get", "json", "application/json").success(function (response) {
            $scope.ReportMasterList = response.Data;
        });
    };
    $scope.GetReportMasterList();
    $scope.ReportNavigation = function (ReportURL, ReportName) {
        var url = ReportURL + ReportName;
        var width = screen.availWidth - 10;
        var height = screen.availHeight - 60;
        var left = 0;//(screen.availWidth - width) / 2;
        var top = 0;//(screen.availHeight - height) / 2;
        var params = 'width=' + width + ', height=' + height;
        params += ', top=' + top + ', left=' + left;
        var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,' + params;
        var pdfWindow = window.open('about:blank', 'null', winFeature);
        pdfWindow.document.write("<html><head><style> * { box-sizing: border-box; padding: 0; margin: 0; border: 0; }</style>"
            + "<title>" + ReportName + "</title></head><body>"
            + '<embed width="100%" height="100%" name="plugin" src="' + url + '" '
            + 'type="application/pdf" internalinstanceid="21"></body></html>');
        pdfWindow.document.close();
        //$('#frmReport').attr('src', ReportURL+ ReportName);
        //$('#ReportModal').modal('show');

    };

    //#region Employee Reports
    $scope.EmployeeReportsListPager = new PagerModule("ReportName", "#EmployeeReportsList", 'ASC');
    //$scope.EmployeeReportsListPager.pageSize = 5;
    $scope.GetEmployeeReportsList = function (isSearchFilter) {
        //$scope.EmployeeReportsListPager.currentPage = isSearchFilter ? 1 : $scope.EmployeeReportsListPager.currentPage;
        var pagermodel = {
            reportName: $scope.ReportName,
            reportDescription: $scope.ReportDescription,
            //pageSize: $scope.EmployeeReportsListPager.pageSize,
            //pageIndex: $scope.EmployeeReportsListPager.currentPage,
            sortIndex: $scope.EmployeeReportsListPager.sortIndex,
            sortDirection: $scope.EmployeeReportsListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.EmployeeReportsListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeReportsListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.EmployeeReportsList = response.Data.Items;
                //$scope.EmployeeReportsListPager.currentPageSize = response.Data.Items.length;
                $scope.EmployeeReportsListPager.totalRecords = response.Data.TotalItems;

            }
            $scope.EmployeeReportsListAjaxStart = false;
        });
    };

    $scope.SearchReportsList = function () {
        //$scope.EmployeeReportsListPager.currentPage = 1;
        $scope.EmployeeReportsListPager.getDataCallback(true);
        $scope.PatientReportsListPager.getDataCallback(true);
        $scope.OtherReportsListPager.getDataCallback(true);
    }

    $scope.EmployeeReportsListPager.getDataCallback = $scope.GetEmployeeReportsList;
    $scope.EmployeeReportsListPager.getDataCallback(true);

    $scope.ResetReportsList = function () {
        $scope.ReportName = '',
        $scope.ReportDescription = '',
        $scope.GetEmployeeReportsList();
        $scope.GetPatientReportsList();
        $scope.GetOtherReportsList();
    }

    //#endregion

    //#region Patient Reports
    $scope.PatientReportsListPager = new PagerModule("ReportName", "#PatientReportsList", 'ASC');
    //$scope.PatientReportsListPager.pageSize = 5;
    $scope.GetPatientReportsList = function (isSearchFilter) {
       // $scope.PatientReportsListPager.currentPage = isSearchFilter ? 1 : $scope.PatientReportsListPager.currentPage;
        var pagermodel = {
            reportName: $scope.ReportName,
            reportDescription: $scope.ReportDescription,
            //pageSize: $scope.PatientReportsListPager.pageSize,
            //pageIndex: $scope.PatientReportsListPager.currentPage,
            sortIndex: $scope.PatientReportsListPager.sortIndex,
            sortDirection: $scope.PatientReportsListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.PatientReportsListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetPatientReportsListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.PatientReportsList = response.Data.Items;
                //$scope.PatientReportsListPager.currentPageSize = response.Data.Items.length;
                $scope.PatientReportsListPager.totalRecords = response.Data.TotalItems;

            }
            $scope.PatientReportsListAjaxStart = false;
        });
    };

    $scope.SearchPatientReportsList = function () {
        //$scope.PatientReportsListPager.currentPage = 1;
        $scope.PatientReportsListPager.getDataCallback(true);
    }

    $scope.PatientReportsListPager.getDataCallback = $scope.GetPatientReportsList;
    $scope.PatientReportsListPager.getDataCallback(true);

    //#endregion

    //#region Other Reports
    $scope.OtherReportsListPager = new PagerModule("ReportName", "#OtherReportsList", 'ASC');
    //$scope.OtherReportsListPager.pageSize = 5;
    $scope.GetOtherReportsList = function (isSearchFilter) {
        //$scope.OtherReportsListPager.currentPage = isSearchFilter ? 1 : $scope.OtherReportsListPager.currentPage;
        var pagermodel = {
            reportName: $scope.ReportName,
            reportDescription: $scope.ReportDescription,
            //pageSize: $scope.OtherReportsListPager.pageSize,
            //pageIndex: $scope.OtherReportsListPager.currentPage,
            sortIndex: $scope.OtherReportsListPager.sortIndex,
            sortDirection: $scope.OtherReportsListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.OtherReportsListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetOtherReportsListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.OtherReportsList = response.Data.Items;
                //$scope.OtherReportsListPager.currentPageSize = response.Data.Items.length;
                $scope.OtherReportsListPager.totalRecords = response.Data.TotalItems;

            }
            $scope.OtherReportsListAjaxStart = false;
        });
    };

    $scope.SearchOtherReportsList = function () {
        //$scope.OtherReportsListPager.currentPage = 1;
        $scope.OtherReportsListPager.getDataCallback(true);
    }

    $scope.OtherReportsListPager.getDataCallback = $scope.GetOtherReportsList;
    $scope.OtherReportsListPager.getDataCallback(true);

    //#endregion

};

controllers.ReportMasterController.$inject = ['$scope', '$http'];

