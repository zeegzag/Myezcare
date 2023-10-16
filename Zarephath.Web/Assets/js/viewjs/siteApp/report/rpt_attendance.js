var vm;
controllers.RptAttendanceController = function ($scope, $http, $window) {
    vm = $scope;
    $scope.NoRecordsFoundFlag = false;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetAttendanceModel").val());
    };
    $scope.SearchReportModel = $.parseJSON($("#hdnSetAttendanceModel").val()).SearchReportModel;
    $scope.DownloadAttendanceReport = function (ele) {
        $scope.AttendanceAjaxCall = true;
        $scope.NoRecordsFoundFlag = false;
        $(ele).button('loading');
        var jsonData = angular.toJson({ searchReportModel: $scope.SearchReportModel });
        AngularAjaxCall($http, SiteUrl.GetAttendanceReportUrl, jsonData, "Post", "json", "application/json",false).success(function (response) {
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
        $scope.SearchReportModel = {};
        $scope.SearchReportModel.IsDeleted = "0";
        $scope.SearchReportModel.ChecklistID = "-1";
        $scope.SearchReportModel.ClinicalReviewID = "-1";
        $scope.SearchReportModel.NotifyCaseManagerID = "-1";
        $scope.SearchReportModel.ServiceID = "-1";
        $scope.SearchReportModel.IsSaveAsDraft = "-1";
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


controllers.RptAttendanceController.$inject = ['$scope', '$http'];


$(document).ready(function () {
    //$(".dateInputMask").inputmask("m/d/y", {
    //    placeholder: "mm/dd/yyyy"
    //});
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});