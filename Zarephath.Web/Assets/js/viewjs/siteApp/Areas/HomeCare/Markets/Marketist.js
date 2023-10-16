var vm;
controllers.EBMarketsListController = function ($scope, $http, $timeout) {
    vm = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetMarketListPage").val());
    };

    $scope.AddMarketURL = HomeCareSiteUrl.AddMarketURL;
    $scope.MarketList = [];
    $scope.SelectedMarketIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.MarketModel = $.parseJSON($("#hdnSetMarketListPage").val());
    $scope.SearchMarketListPage = $scope.MarketModel.SearchEBMarketsListPage;
    $scope.TempSearchMarketListPage = $scope.MarketModel.SearchEBMarketsListPage;
    $scope.MarketListPager = new PagerModule("ID","", "DESC");
    
    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            SearchEBMarketListPage: $scope.SearchMarketListPage,
            pageSize: $scope.MarketListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.MarketListPager.sortIndex,
            sortDirection: $scope.MarketListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchMarketListPage = $.parseJSON(angular.toJson($scope.TempSearchMarketListPage));
      
    };

    $scope.GetMarketList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedMarketIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchMarketListPage.ListOfIdsInCsv = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping
        
        var jsonData = $scope.SetPostData($scope.MarketListPager.currentPage);

        AngularAjaxCall($http, HomeCareSiteUrl.GetMarketList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.MarketList = response.Data.Items;
                $scope.MarketListPager.currentPageSize = response.Data.Items.length;
                $scope.MarketListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        //$scope.ResetSearchFilter();
        //$scope.CaseManagerListPager.currentPage = $scope.CaseManagerListPager.currentPage;
        $scope.MarketListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');
            $scope.SearchMarketListPage = $scope.newInstance().SearchEBMarketsListPage;
            $scope.TempSearchMarketListPage = $scope.newInstance().SearchEBMarketsListPage;
            $scope.TempSearchMarketListPage.IsDeleted = "0";
            $scope.MarketListPager.currentPage = 1;
            $scope.MarketListPager.getDataCallback();
        });
    };
    $scope.SearchMarket = function () {
        $scope.MarketListPager.currentPage = 1;
        $scope.MarketListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectMarket = function (Market) {
        if (Market.IsChecked)
            $scope.SelectedMarketIds.push(Market.ID);
        else
            $scope.SelectedMarketIds.remove(Market.ID);

        if ($scope.SelectedMarketIds.length == $scope.MarketListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        
        $scope.SelectedMarketIds = [];

        angular.forEach($scope.MarketList, function (item, key) {
            
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedMarketIds.push(item.EBMarketID);
        });
        console.log($scope.SelectedMarketIds);
        return true;
    };
   
    $scope.DeleteMarket = function (MarketId, title) {
       
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
               $scope.SearchMarketListPage.ListOfIdsInCsv = MarketId > 0 ? MarketId.toString() : $scope.SelectedMarketIds.toString();
             //   $scope.SearchMarketListPage.ListOfIdsInCsv =  $scope.SelectedMarketIds.toString();
                if (MarketId > 0) {
                    if ($scope.MarketListPager.currentPage != 1)
                        $scope.MarketListPager.currentPage = $scope.MarketList.length === 1 ? $scope.MarketListPager.currentPage - 1 : $scope.MarketListPager.currentPage;
                } else {

                    if ($scope.MarketListPager.currentPage != 1 && $scope.SelectedMarketIds.length == $scope.MarketListPager.currentPageSize)
                        $scope.MarketListPager.currentPage = $scope.MarketListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedMarketIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.MarketListPager.currentPage);
                console.log(jsonData);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteMarket, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.MarketList = response.Data.Items;
                        $scope.MarketListPager.currentPageSize = response.Data.Items.length;
                        $scope.MarketListPager.totalRecords = response.Data.TotalItems;
                    }
                });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.DatePickerDate = function (modelDate, newDate) {
        var a;
        if (modelDate) {
            var dt = new Date(modelDate);
            dt >= newDate ? a = newDate : a = dt;
        }
        else {
            a = newDate;
        }
        return moment(a).format('L');
    };

    $scope.MarketListPager.getDataCallback = $scope.GetMarketList;
    $scope.MarketListPager.getDataCallback();



    
};

controllers.EBMarketsListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowMarketMessage");
});
