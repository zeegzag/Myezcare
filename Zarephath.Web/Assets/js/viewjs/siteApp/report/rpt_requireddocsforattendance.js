var vm;
controllers.RptReqDocsForAttendanceModel = function ($scope, $http, $window) {
    vm = $scope;
    $scope.NoRecordsFoundFlag = false;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetReqDocsForAttendanceModel").val());
    };
    // $scope.SearchScheduleAttendanceModel.RegionID = 1;
    $scope.SearchReqDocsForAttendanceModel = $.parseJSON($("#hdnSetReqDocsForAttendanceModel").val()).SearchScheduleAttendanceModel;

    $scope.DownloadReqDocsForAttendanceReport = function (ele) {
        $scope.AttendanceAjaxCall = true;
        $scope.NoRecordsFoundFlag = false;
        $(ele).button('loading');

        var jsonData = angular.toJson({ searchReportModel: $scope.SearchReqDocsForAttendanceModel });
        AngularAjaxCall($http, SiteUrl.GetRequiredDocsForAttendanceReportUrl, jsonData, "Post", "json", "application/json",false).success(function (response) {
            if (response.IsSuccess) {
                window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                $scope.NoRecordsFoundFlag = false;
            } else {
                $scope.NoRecordsFoundFlag = true;
                ShowMessages(response);
            }

            $scope.AttendanceAjaxCall = false;
            $(ele).button('reset');
        });
    };

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

controllers.RptReqDocsForAttendanceModel.$inject = ['$scope', '$http'];

$(document).ready(function () {
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});