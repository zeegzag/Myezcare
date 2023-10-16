var vmfacility;

controllers.FacilityHouseListController = function ($scope, $http, $window, $timeout) {
    vmfacility = $scope;
    $scope.IsEdit = false;
    $scope.AddFacilityHouseURL = SiteUrl.AddFacilityHouseURL;

    $scope.SetFacilityHouseListModel = $.parseJSON($("#hdnFacilityHouseListPage").val());
    $scope.FacilityHouseList = [];
    $scope.SearchFacilityHouseModel = $scope.SetFacilityHouseListModel.SearchFacilityHouseModel;
    $scope.SelectedFacilityIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.FacilityHouseListPager = new PagerModule("FacilityName", undefined, "ASC");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchFacilityHouseModel: $scope.SearchFacilityHouseModel,
            pageSize: $scope.FacilityHouseListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.FacilityHouseListPager.sortIndex,
            sortDirection: $scope.FacilityHouseListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.GetFacilityHouseList = function () {
        var jsonData = $scope.SetPostData($scope.FacilityHouseListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetFacilityHouseListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.SelectedFacilityIds = [];
                $scope.SelectAllCheckbox = false;
                $scope.FacilityHouseList = response.Data.Items;
                $scope.FacilityHouseListPager.currentPageSize = response.Data.Items.length;
                $scope.FacilityHouseListPager.totalRecords = response.Data.TotalItems;
                //$scope.$apply();
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.FacilityHouseListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $scope.SearchFacilityHouseModel = $.parseJSON($("#hdnFacilityHouseListPage").val()).SearchFacilityHouseModel;
        $scope.SearchFacilityHouseModel.IsDeleted = "0";
        $scope.FacilityHouseListPager.currentPage = 1;
        $scope.FacilityHouseListPager.getDataCallback();
    };

    $scope.SearchFacilityHouse = function () {
        $scope.FacilityHouseListPager.currentPage = 1;
        $scope.FacilityHouseListPager.getDataCallback();
    };

    $scope.SelectFacilityHouse = function (facilityHouse) {
        if (facilityHouse.IsChecked)
            $scope.SelectedFacilityIds.push(facilityHouse.FacilityID);
        else
            $scope.SelectedFacilityIds.remove(facilityHouse.FacilityID);

        if ($scope.SelectedFacilityIds.length == $scope.FacilityHouseListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    $scope.SelectAll = function () {
        $scope.SelectedFacilityIds = [];

        angular.forEach($scope.FacilityHouseList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedFacilityIds.push(item.FacilityID);
        });
        return true;
    };

    $scope.DeleteFacilityHouse = function (facilityId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchFacilityHouseModel.ListOfIdsInCsv = facilityId > 0 ? facilityId.toString() : $scope.SelectedFacilityIds.toString();
                if (facilityId > 0) {
                    if ($scope.FacilityHouseListPager.currentPage != 1)
                        $scope.FacilityHouseListPager.currentPage = $scope.FacilityHouseList.length === 1 ? $scope.FacilityHouseListPager.currentPage - 1 : $scope.FacilityHouseListPager.currentPage;
                } else {
                    if ($scope.FacilityHouseListPager.currentPage != 1 && $scope.SelectedFacilityIds.length == $scope.FacilityHouseListPager.currentPageSize)
                        $scope.FacilityHouseListPager.currentPage = $scope.FacilityHouseListPager.currentPage - 1;
                }

                $scope.SelectedFacilityIds = [];
                $scope.SelectAllCheckbox = false;

                var jsonData = $scope.SetPostData($scope.FacilityHouseListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteFacilityHouseURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.FacilityHouseList = response.Data.Items;
                        $scope.FacilityHouseListPager.currentPageSize = response.Data.Items.length;
                        $scope.FacilityHouseListPager.totalRecords = response.Data.TotalItems;
                        ShowMessages(response);
                    } else {
                        bootboxDialog(function () {
                        }, bootboxDialogType.Alert, window.Alert, window.FacilityHouseScheduleExistMessage);
                    }

                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };
    $scope.FacilityHouseListPager.getDataCallback = $scope.GetFacilityHouseList;
    $scope.FacilityHouseListPager.getDataCallback();
};
controllers.FacilityHouseListController.$inject = ['$scope', '$http', '$window', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("FacilityHouseUpdateSuccessMessage");
});
