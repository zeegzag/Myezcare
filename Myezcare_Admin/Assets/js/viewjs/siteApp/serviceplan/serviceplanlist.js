var vm;
controllers.ServicePlanListController = function ($scope, $http, $window, $timeout) {
    vm = $scope;
    $scope.AddServicePlanURL = SiteUrl.AddServicePlanURL;

    $scope.SetServicePlanListModel = $.parseJSON($("#hdnServicePlanListPage").val());

    $scope.ServicePlanList = [];
    $scope.SearchServicePlanModel = $scope.SetServicePlanListModel.SearchServicePlanModel;
    $scope.SelectedServicePlanIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.ServicePlanListPager = new PagerModule("ServicePlanID", undefined, "ASC");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            SearchServicePlanModel: $scope.SearchServicePlanModel,
            pageSize: $scope.ServicePlanListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ServicePlanListPager.sortIndex,
            sortDirection: $scope.ServicePlanListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.GetServicePlanList = function () {
        var jsonData = $scope.SetPostData($scope.ServicePlanListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetServicePlanListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.SelectedServicePlanIds = [];
                $scope.SelectAllCheckbox = false;
                $scope.ServicePlanList = response.Data.Items;
                $scope.ServicePlanListPager.currentPageSize = response.Data.Items.length;
                $scope.ServicePlanListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.ServicePlanListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $scope.SearchServicePlanModel = $.parseJSON($("#hdnServicePlanListPage").val()).SearchServicePlanModel;
        $scope.SearchServicePlanModel.IsDeleted = "0";
        $scope.ServicePlanListPager.currentPage = 1;
        $scope.ServicePlanListPager.getDataCallback();
    };

    $scope.SearchServicePlan = function () {
        $scope.ServicePlanListPager.currentPage = 1;
        $scope.ServicePlanListPager.getDataCallback();
    };

    $scope.SelectServicePlan = function (serviceplan) {
        if (serviceplan.IsChecked)
            $scope.SelectedServicePlanIds.push(serviceplan.ServicePlanID);
        else
            $scope.SelectedServicePlanIds.remove(serviceplan.ServicePlanID);

        if ($scope.SelectedServicePlanIds.length == $scope.ServicePlanListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    $scope.DeleteServicePlan = function (servicePlan, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                if (servicePlan == undefined)
                    $scope.SearchServicePlanModel.ListOfIdsInCsv = $scope.SelectedServicePlanIds.toString();
                else
                    $scope.SearchServicePlanModel.ListOfIdsInCsv = servicePlan.ServicePlanID > 0 ? servicePlan.ServicePlanID.toString() : $scope.SelectedServicePlanIds.toString();

                if (servicePlan != undefined && servicePlan.ServicePlanID > 0) {
                    if ($scope.ServicePlanListPager.currentPage != 1)
                        $scope.ServicePlanListPager.currentPage = $scope.ServicePlanList.length === 1 ? $scope.ServicePlanListPager.currentPage - 1 : $scope.ServicePlanListPager.currentPage;
                } else {

                    if ($scope.ServicePlanListPager.currentPage != 1 && $scope.SelectedServicePlanIds.length == $scope.ServicePlanListPager.currentPageSize)
                        $scope.ServicePlanListPager.currentPage = $scope.ServicePlanListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedServicePlanIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control
                var jsonData = $scope.SetPostData($scope.ServicePlanListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteServicePlanURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.ServicePlanList = response.Data.Items;
                        $scope.ServicePlanListPager.currentPageSize = response.Data.Items.length;
                        $scope.ServicePlanListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage + ' ' + window.ServicePlanDelete, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
        //}
    };

    $scope.SelectAll = function () {
        $scope.SelectedServicePlanIds = [];

        angular.forEach($scope.ServicePlanList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedServicePlanIds.push(item.ServicePlanID);
        });
        return true;
    };

    $scope.ServicePlanListPager.getDataCallback = $scope.GetServicePlanList;
    $scope.ServicePlanListPager.getDataCallback();
};
controllers.ServicePlanListController.$inject = ['$scope', '$http', '$window', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("ServicePlanUpdateSuccessMessage");
});
