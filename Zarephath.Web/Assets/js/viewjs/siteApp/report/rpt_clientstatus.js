var vm;
controllers.RptClientStatusController = function ($scope, $http) {
    vm = $scope;
    $scope.NoRecordsFoundFlag = false;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetClientStatusModel").val());
    };
    $scope.SearchReportModel = $.parseJSON($("#hdnSetClientStatusModel").val()).SearchReportModel;

    $scope.DownloadClientStatusReport = function (ele) {
        $scope.ClientStatusAjaxCall = true;
        $scope.NoRecordsFoundFlag = false;
        $(ele).button('loading');
        var jsonData = angular.toJson({ searchReportModel: $scope.SearchReportModel });
        AngularAjaxCall($http, SiteUrl.GetClientStatusReportUrl, jsonData, "Post", "json", "application/json",false).success(function (response) {
            if (response.IsSuccess) {
                //var downloadFileUrl = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                //window.open(downloadFileUrl, '_self');
                window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                $scope.NoRecordsFoundFlag = false;
            } else {
                $scope.NoRecordsFoundFlag = true;
                ShowMessages(response);
            }
            
            $scope.ClientStatusAjaxCall = false;
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

controllers.RptClientStatusController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});

