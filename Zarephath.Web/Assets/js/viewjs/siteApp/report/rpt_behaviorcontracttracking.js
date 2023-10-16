var vm;
controllers.RptBXContractStatusController = function ($scope, $http, $window) {
    vm = $scope;
    $scope.NoRecordsFoundFlag = false;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetBXContractStatusModel").val());
    };

    $scope.SearchBXContractStatusReport = $.parseJSON($("#hdnSetBXContractStatusModel").val()).SearchBXContractStatusReport;

    $scope.DownloadBXContractStatusReport = function (ele) {
        $scope.BCAjaxCall = true;
        $scope.NoRecordsFoundFlag = false;
        $(ele).button('loading');
        var jsonData = angular.toJson({ searchBxContractStatusReport: $scope.SearchBXContractStatusReport });
        AngularAjaxCall($http, SiteUrl.GetBXContractStatusReportURL, jsonData, "Post", "json", "application/json",false).success(function (response) {
            if (response.IsSuccess) {
                window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                $scope.NoRecordsFoundFlag = false;
            } else {
                $scope.NoRecordsFoundFlag = true;
                ShowMessages(response);
            }
            
            $scope.BCAjaxCall = false;
            $(ele).button('reset');
        });
    };

    $scope.ResetSearchFilter = function () {
        $scope.SearchBXContractStatusReport = {};
        $scope.SearchBXContractStatusReport.ServiceID = '-1';
        $scope.SearchBXContractStatusReport.BXContractStatus = -1;
        $scope.NoRecordsFoundFlag = false;
    };

    $scope.DatePickerDate = function (modelDate) {
        var a;
        if (modelDate) {
            if (modelDate == "0001-01-01T00:00:00Z") {
                $scope.maxDate = new Date();
                $scope.maxDate.setDate($scope.maxDate.getDate() + 1);
                $scope.NewDate = SetExpiryDate();
                a = $scope.NewDate;
            } else {
                var dt = new Date(modelDate);
                a = dt;
            }
        }
        else {
            $scope.maxDate = new Date();
            $scope.maxDate.setDate($scope.maxDate.getDate() + 1);
            $scope.NewDate = SetExpiryDate();
            a = $scope.NewDate;
        }
        return moment(a).format('L');
    };

};
controllers.RptBXContractStatusController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});