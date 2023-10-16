var vm;
controllers.ARAgingReportController = function ($scope, $http) {
    vm = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnARAgingReportPageModel").val());
    };


    $scope.ResetSearchFilter = function () {

        $scope.SearchARAgingReportPage = $scope.newInstance().SearchARAgingReportPage;

        if (ValideElement($scope.SearchARAgingReportPage.StrReconcileStatus))
            $scope.SearchARAgingReportPage.ReconcileStatus = $scope.SearchARAgingReportPage.StrReconcileStatus.split(",");

    }

    $scope.ResetSearchFilter();

    $scope.ARAgingList = [];

    $scope.GetARAgingList = function (resetSearchFilter) {

        if (resetSearchFilter)
            $scope.ResetSearchFilter();


        if ($scope.SearchARAgingReportPage.ReconcileStatus)
            $scope.SearchARAgingReportPage.StrReconcileStatus = $scope.SearchARAgingReportPage.ReconcileStatus.toString();
        else
            $scope.SearchARAgingReportPage.StrReconcileStatus = "";

        if ($scope.SearchARAgingReportPage.PayorIDs)
            $scope.SearchARAgingReportPage.StrPayorIDs = $scope.SearchARAgingReportPage.PayorIDs.toString();
        else
            $scope.SearchARAgingReportPage.StrPayorIDs = "";

        //if ($scope.SearchARAgingReportPage.ClientIDs)
            //$scope.SearchARAgingReportPage.StrClientIDs = $scope.SearchARAgingReportPage.ClientIDs.toString();

        var jsonData = angular.toJson($scope.SearchARAgingReportPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetARAgingReportURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
            if (response.IsSuccess) {
                $scope.ARAgingList = response.Data;
            }
            ShowMessages(response);
        });
    };

    $scope.GetARAgingList();


    $scope.ExportARAgingReportList = function (resetSearchFilter) {

        if (resetSearchFilter)
            $scope.ResetSearchFilter();

        var jsonData = angular.toJson($scope.SearchARAgingReportPage);

        AngularAjaxCall($http, HomeCareSiteUrl.ExportARAgingReportListURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
            if (response.IsSuccess) {
                window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
            }
            ShowMessages(response);
        });


    };

    $scope.GotoReconcilePage = function (item, indexForSelectedRange) {
        
        item.IndexForSelectedRange = indexForSelectedRange;
        item.StrReconcileStatus = $scope.SearchARAgingReportPage.StrReconcileStatus;
        item.ClientName = $scope.SearchARAgingReportPage.ClientName;
        var jsonData = angular.toJson(item);
        SetCookie(jsonData, window.Cookie_AgingReportFilters);
        window.open(HomeCareSiteUrl.BatchMasterURL, '_blank');
    };


    $scope.SearchARAgingReport = function () {
        $scope.GetARAgingList();
    }

    $scope.Reset = function () {
        $scope.GetARAgingList(true);
    }




};
controllers.ARAgingReportController.$inject = ['$scope', '$http'];