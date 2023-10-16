var vm;

controllers.EdiFileLogListController = function ($scope, $http) {
    vm = $scope;
   // $scope.EdiFileLogModelListURL = SiteUrl.EdiFileLogModelListURL;

    $scope.EdiFileLogList = [];
    $scope.SelectedEdiFileLogIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetEdiFileLogListPage").val());
    };
    $scope.EdiFileLogModel = $.parseJSON($("#hdnSetEdiFileLogListPage").val());
    $scope.SearchEdiFileLogListPage = $scope.EdiFileLogModel.SearchEdiFileLogListPage;
    $scope.TempEdiFileLogListPage = $scope.EdiFileLogModel.SearchEdiFileLogListPage;

    $scope.EdiFileLogListPager = new PagerModule("EdiFileLogID",'','DESC');

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchEdiFileLogModel: $scope.SearchEdiFileLogListPage,
            pageSize: $scope.EdiFileLogListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.EdiFileLogListPager.sortIndex,
            sortDirection: $scope.EdiFileLogListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchEdiFileLogListPage = $.parseJSON(angular.toJson($scope.TempEdiFileLogListPage));
    };

    $scope.GetEdiFileLogList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedEdiFileLogIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchEdiFileLogListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();

        var jsonData = $scope.SetPostData($scope.EdiFileLogListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetEdiFileLogList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.EdiFileLogList = response.Data.Items;
                $scope.EdiFileLogListPager.currentPageSize = response.Data.Items.length;
                $scope.EdiFileLogListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {

        $scope.EdiFileLogListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {

        //Reset 
        $scope.SearchEdiFileLogListPage = $scope.newInstance().SearchEdiFileLogListPage;
        $scope.TempEdiFileLogListPage = $scope.newInstance().SearchEdiFileLogListPage;
        $scope.EdiFileLogListPager.currentPage = 1;
        $scope.EdiFileLogListPager.getDataCallback();
    };

    $scope.SearchEdiFileLog = function () {

        $scope.EdiFileLogListPager.currentPage = 1;
        $scope.EdiFileLogListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectEdiFileLog = function (ediFileLog) {
        if (ediFileLog.IsChecked)
            $scope.SelectedEdiFileLogIds.push(ediFileLog.EdiFileLogID);
        else
            $scope.SelectedEdiFileLogIds.remove(ediFileLog.EdiFileLogID);

        if ($scope.SelectedEdiFileLogIds.length == $scope.EdiFileLogListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedEdiFileLogIds = [];
        angular.forEach($scope.EdiFileLogList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedEdiFileLogIds.push(item.EdiFileLogID);
        });
        return true;
    };

    $scope.DeleteEdiFileLog = function (ediFileLogId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchEdiFileLogListPage.ListOfIdsInCSV = ediFileLogId > 0 ? ediFileLogId.toString() : $scope.SelectedEdiFileLogIds.toString();
                if (ediFileLogId > 0) {
                    if ($scope.EdiFileLogListPager.currentPage != 1)
                        $scope.EdiFileLogListPager.currentPage = $scope.EdiFileLogList.length === 1 ? $scope.EdiFileLogListPager.currentPage - 1 : $scope.EdiFileLogListPager.currentPage;
                } else {
                    if ($scope.EdiFileLogListPager.currentPage != 1 && $scope.SelectedEdiFileLogIds.length == $scope.EdiFileLogListPager.currentPageSize)
                        $scope.EdiFileLogListPager.currentPage = $scope.EdiFileLogListPager.currentPage - 1;
                }
                //Reset Selcted Checkbox items and Control
                $scope.SelectedEdiFileLogIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.EdiFileLogListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteEdiFileLog, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {

                        $scope.EdiFileLogList = response.Data.Items;
                        $scope.EdiFileLogListPager.currentPageSize = response.Data.Items.length;
                        $scope.EdiFileLogListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.EdiFileLogListPager.getDataCallback = $scope.GetEdiFileLogList;
    $scope.EdiFileLogListPager.getDataCallback();
};
controllers.EdiFileLogListController.$inject = ['$scope', '$http'];