var vm;
controllers.ScheduleBatchServiceListController = function ($scope, $http) {    
    vm = $scope;
    //$scope.ScheduleBatchServiceModelListURL = SiteUrl.EdiFileLogModelListURL;
    $scope.ScheduleBatchServiceList = [];
    $scope.SelectedScheduleBatchServiceIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetScheduleBatchServiceListPage").val());
    };
    $scope.ScheduleBatchServiceModel = $.parseJSON($("#hdnSetScheduleBatchServiceListPage").val());
    $scope.SearchScheduleBatchServiceListPage = $scope.ScheduleBatchServiceModel.SearchScheduleBatchServiceModel;
    $scope.TempScheduleBatchServiceListPage = $scope.ScheduleBatchServiceModel.SearchScheduleBatchServiceModel;

    $scope.ScheduleBatchServiceListPager = new PagerModule("ScheduleBatchServiceID", "ScheduleBatchServiceListPagerID", 'DESC');

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchScheduleBatchServiceModel: $scope.SearchScheduleBatchServiceListPage,
            pageSize: $scope.ScheduleBatchServiceListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ScheduleBatchServiceListPager.sortIndex,
            sortDirection: $scope.ScheduleBatchServiceListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchScheduleBatchServiceListPage = $.parseJSON(angular.toJson($scope.TempScheduleBatchServiceListPage));
    };

    $scope.GetScheduleBatchServiceList = function (isSearchDataMappingRequire) {
        
        //Reset Selcted Checkbox items and Control

        $scope.SelectedScheduleBatchServiceIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchScheduleBatchServiceListPage.ListOfIdsInCSV = [];

        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();

        var jsonData = $scope.SetPostData($scope.ScheduleBatchServiceListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetScheduleBatchServiceListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ScheduleBatchServiceList = response.Data.Items;
                $scope.ScheduleBatchServiceListPager.currentPageSize = response.Data.Items.length;
                $scope.ScheduleBatchServiceListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.ScheduleBatchServiceListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        //Reset 
        $scope.SearchScheduleBatchServiceListPage = $scope.newInstance().SearchScheduleBatchServiceModel;
        $scope.TempScheduleBatchServiceListPage = $scope.newInstance().SearchScheduleBatchServiceModel;
        $scope.ScheduleBatchServiceListPager.currentPage = 1;
        $scope.ScheduleBatchServiceListPager.getDataCallback();
    };

    $scope.SearchScheduleBatchService = function () {
        $scope.ScheduleBatchServiceListPager.currentPage = 1;
        $scope.ScheduleBatchServiceListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectScheduleBatchService = function (scheduleBatchService) {
        if (scheduleBatchService.IsChecked)
            $scope.SelectedScheduleBatchServiceIds.push(scheduleBatchService.ScheduleBatchServiceID);
        else
            $scope.SelectedScheduleBatchServiceIds.remove(scheduleBatchService.ScheduleBatchServiceID);

        if ($scope.SelectedScheduleBatchServiceIds.length == $scope.ScheduleBatchServiceListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedScheduleBatchServiceIds = [];
        angular.forEach($scope.ScheduleBatchServiceList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedScheduleBatchServiceIds.push(item.ScheduleBatchServiceID);
        });
        return true;
    };

    $scope.DeleteScheduleBatchService = function (scheduleBatchServiceId, title) {

        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchScheduleBatchServiceListPage.ListOfIdsInCSV = scheduleBatchServiceId > 0 ? scheduleBatchServiceId.toString() : $scope.SelectedScheduleBatchServiceIds.toString();
                if (scheduleBatchServiceId > 0) {
                    if ($scope.ScheduleBatchServiceListPager.currentPage != 1)
                        $scope.ScheduleBatchServiceListPager.currentPage = $scope.ScheduleBatchServiceList.length === 1 ? $scope.ScheduleBatchServiceListPager.currentPage - 1 : $scope.ScheduleBatchServiceListPager.currentPage;
                } else {
                    if ($scope.ScheduleBatchServiceListPager.currentPage != 1 && $scope.SelectedScheduleBatchServiceIds.length == $scope.ScheduleBatchServiceListPager.currentPageSize)
                        $scope.ScheduleBatchServiceListPager.currentPage = $scope.ScheduleBatchServiceListPager.currentPage - 1;
                }
                //Reset Selcted Checkbox items and Control
                $scope.SelectedScheduleBatchServiceIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.ScheduleBatchServiceListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteScheduleBatchServiceURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.ScheduleBatchServiceList = response.Data.Items;
                        $scope.ScheduleBatchServiceListPager.currentPageSize = response.Data.Items.length;
                        $scope.ScheduleBatchServiceListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.ScheduleBatchServiceListPager.getDataCallback = $scope.GetScheduleBatchServiceList;
    $scope.ScheduleBatchServiceListPager.getDataCallback();
};
controllers.ScheduleBatchServiceListController.$inject = ['$scope', '$http'];