var vm;
controllers.BillingSummaryController = function ($scope, $http, $window) {
    vm = $scope;
    $scope.NoRecordsFoundFlag = false;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSearchBillingSummaryReport").val());
    };
    $scope.SearchReportModel = $.parseJSON($("#hdnSearchBillingSummaryReport").val());

    $scope.DownloadClientInformationReport = function (ele) {
        
        $scope.ClientInformationAjaxCall = true;
        $scope.NoRecordsFoundFlag = false;
        $(ele).button('loading');
        var jsonData = angular.toJson({ searchBillingSummaryReport: $scope.SearchReportModel });
        AngularAjaxCall($http, SiteUrl.GetBillingSummaryReportUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                $scope.NoRecordsFoundFlag = false;
            } else {
                $scope.NoRecordsFoundFlag = true;
                ShowMessages(response);
            }
            $scope.ClientInformationAjaxCall = false;
            $(ele).button('reset');
        });
    };

    $scope.ResetSearchFilter = function () {
        $scope.SearchReportModel = {};
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
controllers.BillingSummaryController.$inject = ['$scope', '$http'];

//var downloadFileUrl = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
//window.open(downloadFileUrl, '_blank');

$(document).ready(function () {
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});