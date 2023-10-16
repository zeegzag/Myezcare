var vm;
controllers.RptDspRosterController = function ($scope, $http) {
    vm = $scope;
    
    $scope.NoRecordsFoundFlag = false;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetDspRosterModel").val());
    };
    $scope.SearchDspRosterModel = $.parseJSON($("#hdnSetDspRosterModel").val()).SearchDspRosterModel;

    $scope.ReferralStatuseModel = $.parseJSON($("#hdnSetDspRosterModel").val()).ReferralStatuses;

    $scope.DownloadDspRosterReport = function (ele) {
        
        $scope.DSPAjaxCall = true;
        $scope.NoRecordsFoundFlag = false;
        $(ele).button('loading');
        var jsonData = angular.toJson({ searchDspRosterModel: $scope.SearchDspRosterModel });
        AngularAjaxCall($http, SiteUrl.GetDspRosterReportUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                $scope.NoRecordsFoundFlag = false;
            } else {
                $scope.NoRecordsFoundFlag = true;
                ShowMessages(response);
            }

            $scope.DSPAjaxCall = false;
            $(ele).button('reset');
        });
    };

    $scope.ResetSearchFilter = function () {

        $scope.SearchDspRosterModel = {};
        $scope.SearchDspRosterModel.IsDeleted = "0";
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

controllers.RptDspRosterController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    //$(".dateInputMask").inputmask("m/d/y", {
    //    placeholder: "mm/dd/yyyy"
    //});
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});

