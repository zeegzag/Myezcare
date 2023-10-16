var vm;
controllers.RptScheduleAttendanceController = function ($scope, $http, $window) {
    vm = $scope;
    $scope.NoRecordsFoundFlag = true;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetScheduleAttendanceModel").val());
    };
    // $scope.SearchScheduleAttendanceModel.RegionID = 1;
    $scope.SearchReportModel = $.parseJSON($("#hdnSetScheduleAttendanceModel").val()).SearchScheduleAttendanceModel;
    //$scope.DownloadScheduleAttendanceReport = function () {

    //    var jsonData = angular.toJson({ searchReportModel: $scope.SearchReportModel });
    //    AngularAjaxCall($http, SiteUrl.GetScheduleAttendanceScheduleReportUrl, jsonData, "Post", "json", "application/json").success(function (response) {
    //        if (response.IsSuccess) {
    //            window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
    //            $scope.NoRecordsFoundFlag = false;
    //        } else {
    //            $scope.NoRecordsFoundFlag = true;
    //            ShowMessages(response);
    //        }
    //    });
    //};

    $scope.ResetSearchFilter = function () {
        
        $scope.SearchScheduleAttendanceModel = {};
        $scope.SearchScheduleAttendanceModel.RegionID = '1';
        $scope.NoRecordsFoundFlag = false;
        HideErrors("#frmSearchRptScheduleAttendance");
        return false;
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

controllers.RptScheduleAttendanceController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});