var encvm;
controllers.RptGeneralNoticeController = function ($scope, $http, $window) {
    encvm = $scope;
    $scope.NoRecordsFoundFlag = false;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetGeneralNoticeModel").val());
    };

    $scope.GetReferralInfoforReportURL = SiteUrl.GetReferralInfoforReportURL;

    $scope.SearchGeneralNoticeModel = $.parseJSON($("#hdnSetGeneralNoticeModel").val()).SearchGeneralNoticeModel;
    $scope.GeneralNoticeTokenObj = {};

    
    $scope.DownloadGeneralNoticeReport = function (ele) {
        $scope.GeneralNoticeAjaxCall = true;
        $scope.NoRecordsFoundFlag = false;
        $(ele).button('loading');
        var jsonData = angular.toJson({ searchGeneralNoticeModel: $scope.SearchGeneralNoticeModel });
        AngularAjaxCall($http, SiteUrl.GetGeneralNoticeReportUrl, jsonData, "Post", "json", "application/json",false).success(function (response) {
            if (response.IsSuccess) {
                window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                $scope.NoRecordsFoundFlag = false;
            } else {
                $scope.NoRecordsFoundFlag = true;
                ShowMessages(response);
            }
            $scope.GeneralNoticeAjaxCall = false;
            $(ele).button('reset');
        });
    };

    $scope.ResetSearchFilter = function () {
        $scope.SearchGeneralNoticeModel = {};;
        $("#AddGeneralReferralIDTkn").tokenInput("clear");
        $scope.NoRecordsFoundFlag = false;
    };

    

    //#region Token input related code for Client Detail 

    $scope.GN_ReferralDetailResultsFormatter = function (item) {
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

    $scope.GN_ReferralTokenFormatter = function (item) {
        return "<li id='{0}'>{0}</li>".format(item.Name);
    };

    $scope.GN_RemoveReferral = function () {
        $scope.SearchGeneralNoticeModel.ReferralID = null;
        $scope.GeneralNoticeTokenObj.clear();
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };

    //#endregion

};

controllers.RptGeneralNoticeController.$inject = ['$scope', '$http'];

