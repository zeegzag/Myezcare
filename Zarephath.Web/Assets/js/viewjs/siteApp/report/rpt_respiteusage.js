var vm;
controllers.RptRespiteUsageController = function ($scope, $http) {
    vm = $scope;
    $scope.NoRecordsFoundFlag = false;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetRespiteUsageModel").val());
    };
    $scope.SearchRespiteUsageModelSearchRespiteUsageModel = $.parseJSON($("#hdnSetRespiteUsageModel").val()).SearchRespiteUsageModel;

    $scope.DownloadRespiteUsageReport = function (ele) {
        $scope.RespiteUsageAjaxCall = true;
        $scope.NoRecordsFoundFlag = false;
        $(ele).button('loading');
        $scope.SearchRespiteUsageModel.FisaclYear = $('#FiscalYear').val();
        var jsonData = angular.toJson({ searchRespiteUsageModel: $scope.SearchRespiteUsageModel });
        AngularAjaxCall($http, SiteUrl.GetRespiteUsageReportUrl, jsonData, "Post", "json", "application/json",false).success(function (response) {
            if (response.IsSuccess) {
                window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                $scope.NoRecordsFoundFlag = false;
            } else {
                $scope.NoRecordsFoundFlag = true;
                //ShowMessages(response);   
            }
            $scope.RespiteUsageAjaxCall = false;
            $(ele).button('reset');
        });
    };

    $scope.ResetSearchFilter = function () {
        $scope.SearchRespiteUsageModel = {};
        $scope.SearchRespiteUsageModel.IsDeleted = '-1';
        //$('#FiscalYear').val(0);
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
                //dt >= newDate ? a = newDate :
                a = dt;
            }
        }
        else {
            $scope.maxDate = new Date();
            $scope.maxDate.setDate($scope.maxDate.getDate() + 1);
            $scope.NewDate = SetExpiryDate();
            a = $scope.NewDate;
            //a = newDate;
        }
        return moment(a).format('L');
    };

};
controllers.RptRespiteUsageController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    //$(".dateInputMask").inputmask("m/d/y", {
    //    placeholder: "mm/dd/yyyy"
    //});
});
