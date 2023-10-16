var vm;
controllers.CategoryListController = function ($scope, $http, $timeout) {
    vm = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetCategoryListPage").val());
    };

    $scope.AddCategoryURL = HomeCareSiteUrl.AddCategoryURL;
    $scope.CategoryList = [];
    $scope.SelectedCategoryIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.CategoryModel = $.parseJSON($("#hdnSetCategoryListPage").val());
    $scope.SearchCategoryListPage = $scope.CategoryModel.SearchEBCategoryListPage;
    $scope.TempSearchCategoryListPage = $scope.CategoryModel.SearchEBCategoryListPage;
    $scope.CategoryListPager = new PagerModule("ID","", "DESC");
    
    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            SearchEBCategoryListPage: $scope.SearchCategoryListPage,
            pageSize: $scope.CategoryListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.CategoryListPager.sortIndex,
            sortDirection: $scope.CategoryListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchCategoryListPage = $.parseJSON(angular.toJson($scope.TempSearchCategoryListPage));
      
    };

    $scope.GetCategoryList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedCategoryIds = [];
        $scope.SelectAllCheckbox = false;
       $scope.SearchCategoryListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping
        
        var jsonData = $scope.SetPostData($scope.CategoryListPager.currentPage);

        AngularAjaxCall($http, HomeCareSiteUrl.GetCategoryList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.CategoryList = response.Data.Items;
                $scope.CategoryListPager.currentPageSize = response.Data.Items.length;
                $scope.CategoryListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        //$scope.ResetSearchFilter();
        //$scope.CaseManagerListPager.currentPage = $scope.CaseManagerListPager.currentPage;
        $scope.CategoryListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');
            $scope.SearchCategoryListPage = $scope.newInstance().SearchCategoryListPage;
            $scope.TempSearchCategoryListPage = $scope.newInstance().SearchCategoryListPage;
            $scope.TempSearchCategoryListPage.IsDeleted = "0";
            $scope.CategoryListPager.currentPage = 1;
            $scope.CategoryListPager.getDataCallback();
        });
    };
    $scope.SearchCategory = function () {
        $scope.CategoryListPager.currentPage = 1;
        $scope.CategoryListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectCategory = function (Category) {
        if (Category.IsChecked)
            $scope.SelectedCategoryIds.push(Category.ID);
        else
            $scope.SelectedCategoryIds.remove(Category.ID);

        if ($scope.SelectedCategoryIds.length == $scope.CategoryListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedCategoryIds = [];

        angular.forEach($scope.CategoryList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedCategoryIds.push(item.CategoryID);
        });
       
        return true;
    };

    $scope.DeleteCategory = function (CategoryId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchCategoryListPage.ListOfIdsInCsv = CategoryId > 0 ? CategoryId.toString() : $scope.SelectedCategoryIds.toString();

                if (CategoryId > 0) {
                    if ($scope.CategoryListPager.currentPage != 1)
                        $scope.CategoryListPager.currentPage = $scope.CategoryList.length === 1 ? $scope.CategoryListPager.currentPage - 1 : $scope.CategoryListPager.currentPage;
                } else {

                    if ($scope.CategoryListPager.currentPage != 1 && $scope.SelectedCategoryIds.length == $scope.CategoryListPager.currentPageSize)
                        $scope.CategoryListPager.currentPage = $scope.CategoryListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedCategoryIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.CategoryListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteCategory, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.CategoryList = response.Data.Items;
                        $scope.CategoryListPager.currentPageSize = response.Data.Items.length;
                        $scope.CategoryListPager.totalRecords = response.Data.TotalItems;
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

    $scope.CategoryListPager.getDataCallback = $scope.GetCategoryList;
    $scope.CategoryListPager.getDataCallback();



    
};

controllers.CategoryListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowCategoryMessage");
});
