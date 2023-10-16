var vm;

controllers.TransportaLocationListController = function ($scope, $http) {
    vm = $scope;
    
    $scope.TransPortationModelListURL = SiteUrl.TransPortationModelListURL;
    $scope.TransPortationList = [];
    $scope.SelectedTransPortationIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetTransportLocationListPage").val());
    };

    $scope.TransPortationModel = $.parseJSON($("#hdnSetTransportLocationListPage").val());

    $scope.SearchTransPortationListPage = $scope.TransPortationModel.SearchTransPortationListPage;
    $scope.TempTransPortationListPage = $scope.TransPortationModel.SearchTransPortationListPage;

    $scope.TransPortationListPager = new PagerModule("Location");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchTransportLocationModel: $scope.SearchTransPortationListPage,
            pageSize: $scope.TransPortationListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.TransPortationListPager.sortIndex,
            sortDirection: $scope.TransPortationListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchTransPortationListPage = $.parseJSON(angular.toJson($scope.TempTransPortationListPage));
        //$scope.SearchTransPortationListPage.Location = $scope.TempTransPortationListPage.Location;
        //$scope.SearchTransPortationListPage.LocationCode = $scope.TempTransPortationListPage.LocationCode;
        //$scope.SearchTransPortationListPage.State = $scope.TempTransPortationListPage.State;
        //$scope.SearchTransPortationListPage.Address = $scope.TempTransPortationListPage.Address;
        //$scope.SearchTransPortationListPage.Phone = $scope.TempTransPortationListPage.Phone;
        //$scope.SearchTransPortationListPage.IsDeleted = $scope.TempTransPortationListPage.IsDeleted;
    };

    $scope.GetTransportationLocationList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedTransPortationIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchTransPortationListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();

        var jsonData = $scope.SetPostData($scope.TransPortationListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetTransportationList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.TransPortationList = response.Data.Items;
                $scope.TransPortationListPager.currentPageSize = response.Data.Items.length;
                $scope.TransPortationListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.TransPortationListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        //Reset 
        $scope.SearchTransPortationListPage = $scope.newInstance().SearchTransPortationListPage;
        $scope.TempTransPortationListPage = $scope.newInstance().SearchTransPortationListPage;
        $scope.TempTransPortationListPage.IsDeleted = "0";
        $scope.TransPortationListPager.currentPage = 1;
        $scope.TransPortationListPager.getDataCallback();
    };

    $scope.SearchTransportation = function () {
        $scope.TransPortationListPager.currentPage = 1;
        $scope.TransPortationListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectTransportationLocation = function (transportationLocation) {
        if (transportationLocation.IsChecked)
            $scope.SelectedTransPortationIds.push(transportationLocation.TransportLocationID);
        else
            $scope.SelectedTransPortationIds.remove(transportationLocation.TransportLocationID);

        if ($scope.SelectedTransPortationIds.length == $scope.TransPortationListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedTransPortationIds = [];
        angular.forEach($scope.TransPortationList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedTransPortationIds.push(item.TransportLocationID);
        });
        return true;
    };

    $scope.DeleteTransportationLocation = function (transportLocationId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            
            if (result) {
                $scope.SearchTransPortationListPage.ListOfIdsInCSV = transportLocationId > 0 ? transportLocationId.toString() : $scope.SelectedTransPortationIds.toString();
                if (transportLocationId > 0) {
                    if ($scope.TransPortationListPager.currentPage != 1)
                        $scope.TransPortationListPager.currentPage = $scope.TransPortationList.length === 1 ? $scope.TransPortationList.currentPage - 1 : $scope.TransPortationList.currentPage;
                }
                else {
                    if ($scope.TransPortationListPager.currentPage != 1 && $scope.SelectedTransPortationIds.length == $scope.TransPortationListPager.currentPageSize)
                        $scope.TransPortationListPager.currentPage = $scope.TransPortationListPager.currentPage - 1;
                }
                //Reset Selcted Checkbox items and Control
                $scope.SelectedTransPortationIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.TransPortationListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteTransportation, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.TransPortationList = response.Data.Items;
                        $scope.TransPortationListPager.currentPageSize = response.Data.Items.length;
                        $scope.TransPortationListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };
    

    $scope.TransPortationListPager.getDataCallback = $scope.GetTransportationLocationList;
    $scope.TransPortationListPager.getDataCallback();
};

controllers.TransportaLocationListController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    ShowPageLoadMessage("TransportationUpdateSuccessMessage");
});
