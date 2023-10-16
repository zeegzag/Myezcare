var vm;

controllers.PayorListController = function ($scope, $http) {
    vm = $scope;
    $scope.SetAddPayorPage = SiteUrl.SetAddPayorPageURL;
    $scope.PayorList = [];
    $scope.SelectedPayorIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetPayorListPage").val());
    };
    $scope.PayorModel = $.parseJSON($("#hdnSetPayorListPage").val());
    $scope.SearchPayorListPage = $scope.newInstance().SearchPayorListPage;
    $scope.TempPayorListPage = $scope.newInstance().SearchPayorListPage;
    $scope.PayorListPager = new PagerModule("PayorName");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchPayorListPage: $scope.SearchPayorListPage,
            pageSize: $scope.PayorListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.PayorListPager.sortIndex,
            sortDirection: $scope.PayorListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchPayorListPage = $.parseJSON(angular.toJson($scope.TempPayorListPage));
    };

    $scope.GetPayorList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedPayorIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchPayorListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();

        var jsonData = $scope.SetPostData($scope.PayorListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetPayorList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.PayorList = response.Data.Items;
                $scope.PayorListPager.currentPageSize = response.Data.Items.length;
                $scope.PayorListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.PayorListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        //Reset 
        $scope.SearchPayorListPage = $scope.newInstance().SearchPayorListPage;
        $scope.TempPayorListPage = $scope.newInstance().SearchPayorListPage;
        $scope.TempPayorListPage.IsDeleted = "0";
        $scope.PayorListPager.currentPage = 1;
        $scope.PayorListPager.getDataCallback();
    };

    $scope.SearchPayor = function () {
        $scope.PayorListPager.currentPage = 1;
        $scope.PayorListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectPayor = function (payorList) {
        if (payorList.IsChecked)
            $scope.SelectedPayorIds.push(payorList.PayorID);
        else
            $scope.SelectedPayorIds.remove(payorList.PayorID);
        
        if ($scope.SelectedPayorIds.length == $scope.PayorListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedPayorIds = [];
        angular.forEach($scope.PayorList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedPayorIds.push(item.PayorID);
        });
        return true;
    };

    $scope.DeletePayor = function (payorId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchPayorListPage.ListOfIdsInCSV = payorId > 0 ? payorId.toString() : $scope.SelectedPayorIds.toString();
                if (payorId > 0) {
                    if ($scope.PayorListPager.currentPage != 1)
                        $scope.PayorListPager.currentPage = $scope.PayorList.length === 1 ? $scope.PayorList.currentPage - 1 : $scope.PayorList.currentPage;
                }
                else {
                    if ($scope.PayorListPager.currentPage != 1 && $scope.SelectedPayorIds.length == $scope.PayorListPager.currentPageSize)
                        $scope.PayorListPager.currentPage = $scope.PayorListPager.currentPage - 1;
                }
                //Reset Selcted Checkbox items and Control
                $scope.SelectedPayorIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.PayorListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeletePayorList, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.PayorList = response.Data.Items;
                        $scope.PayorListPager.currentPageSize = response.Data.Items.length;
                        $scope.PayorListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };
    $scope.PayorListPager.getDataCallback = $scope.GetPayorList;
    $scope.PayorListPager.getDataCallback();
};

controllers.PayorListController.$inject = ['$scope', '$http'];

//$(document).ready(function () {
//    ShowPageLoadMessage("PayorUpdateSuccessMessage");
//});
