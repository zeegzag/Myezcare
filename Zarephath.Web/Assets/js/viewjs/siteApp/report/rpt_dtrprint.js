var encvm;
controllers.RptDTRPrintController = function ($scope, $http) {
    encvm = $scope;
    $scope.NoRecordsFoundFlag = false;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetDTRPrintModel").val());
    };
    $scope.GetDTRDetailsURL = SiteUrl.GetDTRDetailsURL;

    $scope.GetReferralInfoforReportURL = SiteUrl.GetReferralInfoforReportURL;

    $scope.SearchDTRPrintModel = $.parseJSON($("#hdnSetDTRPrintModel").val()).SearchDTRPrintModel;

    $scope.DownloadDTRReport = function (ele) {
        $scope.DTRAjaxCall = true;
        $scope.NoRecordsFoundFlag = false;
        $(ele).button('loading');
        var jsonData = angular.toJson({ searchDTRPrintModel: $scope.SearchDTRPrintModel });
        AngularAjaxCall($http, SiteUrl.GetEDTRPrintReportUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                $scope.NoRecordsFoundFlag = false;
            } else {
                $scope.NoRecordsFoundFlag = true;
                ShowMessages(response);
            }

            $scope.DTRAjaxCall = false;
            $(ele).button('reset');
        });
    };
    $scope.ResetSearchFilter = function () {
        $scope.SearchDTRPrintModel = {};;
        $("#AddNoteReferralIDTkn").tokenInput("clear");
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

    //#region Token input related code for Client Detail 
    $scope.DTRTokenObj = {};
    $scope.ReferralDetailResultsFormatter = function (item) {
        return "<li id='{0}'>{0}: {1}<br/><small><b>{4}:</b> {2}</small><small><b style='padding-left:10px;'>{5}: </b>{3}</small><br/><small><b style='color:#ad0303;'>{6}:</b> {7}</small><br/><small><b style='color:#ad0303;'>{8}:</b> {9}</small></li>"
            .format(
                window.Name,
                item.Name,
                item.AHCCCSID ? item.AHCCCSID : 'N/A',
                item.CISNumber ? item.CISNumber : 'N/A',
                window.AHCCCSID,
                window.CISNumber,
                window.Phone,
                item.Phone1 ? item.Phone1 : 'N/A',
                window.Address,
                item.Address ? item.Address : 'N/A'
            );
    };

    $scope.ReferralTokenFormatter = function (item) {
        return "<li id='{0}'>{0}</li>".format(item.Name);
    };

    $scope.RemoveReferral = function () {
        $scope.SearchDTRPrintModel.ReferralID = null;
        $scope.DTRTokenObj.clear();
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };

    //#endregion

};

controllers.RptDTRPrintController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});