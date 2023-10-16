var vm;

controllers.ParentListController = function ($scope, $http, $timeout) {
    vm = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetParentListPage").val());
    };

    $scope.AddParentURL = SiteUrl.AddParentURL;
    $scope.ParentList = [];
    $scope.SelectedParentIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.ParentModel = $.parseJSON($("#hdnSetParentListPage").val());
    $scope.SearchParentListPage = $scope.ParentModel.SearchParentListPage;
    $scope.TempSearchParentListPage = $scope.ParentModel.SearchParentListPage;

    $scope.ParentListPager = new PagerModule("Name");

    $scope.SetPostData = function (fromIndex) {

        var pagermodel = {
            SearchParentListPage: $scope.SearchParentListPage,
            pageSize: $scope.ParentListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ParentListPager.sortIndex,
            sortDirection: $scope.ParentListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchParentListPage = $.parseJSON(angular.toJson($scope.TempSearchParentListPage));
        //$scope.SearchParentListPage.Name = $scope.TempSearchParentListPage.Name;
        //$scope.SearchParentListPage.Email = $scope.TempSearchParentListPage.Email;
        //$scope.SearchParentListPage.AgencyID = $scope.TempSearchParentListPage.AgencyID;
        //$scope.SearchParentListPage.AgencyLocationID = $scope.TempSearchParentListPage.AgencyLocationID;
        //$scope.SearchParentListPage.Phone = $scope.TempSearchParentListPage.Phone;
        //$scope.SearchParentListPage.IsDeleted = $scope.TempSearchParentListPage.IsDeleted;
    };

    $scope.GetParentList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedParentIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchParentListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.ParentListPager.currentPage);

        AngularAjaxCall($http, SiteUrl.GetParentList, jsonData, "Post", "json", "application/json").success(function (response) {

            if (response.IsSuccess) {
                
                $scope.ParentList = response.Data.Items;

                $scope.ParentListPager.currentPageSize = response.Data.Items.length;
                $scope.ParentListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        //$scope.ResetSearchFilter();
        //$scope.ParentListPager.currentPage = $scope.ParentListPager.currentPage;
        $scope.ParentListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');
            $scope.SearchParentListPage = $scope.newInstance().SearchParentListPage;
            $scope.TempSearchParentListPage = $scope.newInstance().SearchParentListPage;
            $scope.TempSearchParentListPage.IsDeleted = "0";
            $scope.ParentListPager.currentPage = 1;
            $scope.ParentListPager.getDataCallback();
        });
    };
    $scope.SearchParent = function () {
        $scope.ParentListPager.currentPage = 1;
        $scope.ParentListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectParent = function (Parent) {
        if (Parent.IsChecked)
            $scope.SelectedParentIds.push(Parent.ParentID);
        else
            $scope.SelectedParentIds.remove(Parent.ParentID);

        if ($scope.SelectedParentIds.length == $scope.ParentListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedParentIds = [];

        angular.forEach($scope.ParentList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedParentIds.push(item.ParentID);
        });

        return true;
    };

    $scope.DeleteParent = function (ParentID, title) {
        
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchParentListPage.ListOfIdsInCsv = ParentID > 0 ? ParentID.toString() : $scope.SelectedParentIds.toString();

                if (ParentID > 0) {
                    if ($scope.ParentListPager.currentPage != 1)
                        $scope.ParentListPager.currentPage = $scope.ParentList.length === 1 ? $scope.ParentListPager.currentPage - 1 : $scope.ParentListPager.currentPage;
                } else {

                    if ($scope.ParentListPager.currentPage != 1 && $scope.SelectedParentIds.length == $scope.ParentListPager.currentPageSize)
                        $scope.ParentListPager.currentPage = $scope.ParentListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedParentIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.ParentListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteParent, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.ParentList = response.Data.Items;
                        $scope.ParentListPager.currentPageSize = response.Data.Items.length;
                        $scope.ParentListPager.totalRecords = response.Data.TotalItems;
                    }
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.ParentListPager.getDataCallback = $scope.GetParentList;
    $scope.ParentListPager.getDataCallback();



    $scope.GotoReferralList = function (cmId) {
        //window.location = SiteUrl.ReferralListURL;
        SetCookie(cmId, 'CM_ID');
        window.open(SiteUrl.ReferralListURL, '_blank');
    };
    
};

controllers.ParentListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowParentMessage");
});
