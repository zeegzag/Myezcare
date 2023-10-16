var vm;

controllers.CaseManagerListController = function ($scope, $http, $timeout) {
    vm = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetCaseManagerListPage").val());
    };

    $scope.AddCaseManagerURL = SiteUrl.AddCaseManagerURL;
    $scope.CaseManagerList = [];
    $scope.SelectedCaseManagerIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.CaseManagerModel = $.parseJSON($("#hdnSetCaseManagerListPage").val());
    $scope.SearchCaseManagerListPage = $scope.CaseManagerModel.SearchCaseManagerListPage;
    $scope.TempSearchCaseManagerListPage = $scope.CaseManagerModel.SearchCaseManagerListPage;

    $scope.CaseManagerListPager = new PagerModule("Name");

    $scope.SetPostData = function (fromIndex) {

        var pagermodel = {
            SearchCaseManagerListPage: $scope.SearchCaseManagerListPage,
            pageSize: $scope.CaseManagerListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.CaseManagerListPager.sortIndex,
            sortDirection: $scope.CaseManagerListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchCaseManagerListPage = $.parseJSON(angular.toJson($scope.TempSearchCaseManagerListPage));
        //$scope.SearchCaseManagerListPage.Name = $scope.TempSearchCaseManagerListPage.Name;
        //$scope.SearchCaseManagerListPage.Email = $scope.TempSearchCaseManagerListPage.Email;
        //$scope.SearchCaseManagerListPage.AgencyID = $scope.TempSearchCaseManagerListPage.AgencyID;
        //$scope.SearchCaseManagerListPage.AgencyLocationID = $scope.TempSearchCaseManagerListPage.AgencyLocationID;
        //$scope.SearchCaseManagerListPage.Phone = $scope.TempSearchCaseManagerListPage.Phone;
        //$scope.SearchCaseManagerListPage.IsDeleted = $scope.TempSearchCaseManagerListPage.IsDeleted;
    };

    $scope.GetCaseManagerList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedCaseManagerIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchCaseManagerListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.CaseManagerListPager.currentPage);

        AngularAjaxCall($http, SiteUrl.GetCaseManagerList, jsonData, "Post", "json", "application/json").success(function (response) {

            if (response.IsSuccess) {
                
                $scope.CaseManagerList = response.Data.Items;

                $scope.CaseManagerListPager.currentPageSize = response.Data.Items.length;
                $scope.CaseManagerListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        //$scope.ResetSearchFilter();
        //$scope.CaseManagerListPager.currentPage = $scope.CaseManagerListPager.currentPage;
        $scope.CaseManagerListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');
            $scope.SearchCaseManagerListPage = $scope.newInstance().SearchCaseManagerListPage;
            $scope.TempSearchCaseManagerListPage = $scope.newInstance().SearchCaseManagerListPage;
            $scope.TempSearchCaseManagerListPage.IsDeleted = "0";
            $scope.CaseManagerListPager.currentPage = 1;
            $scope.CaseManagerListPager.getDataCallback();
        });
    };
    $scope.SearchCaseManager = function () {
        $scope.CaseManagerListPager.currentPage = 1;
        $scope.CaseManagerListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectCaseManager = function (caseManager) {
        if (caseManager.IsChecked)
            $scope.SelectedCaseManagerIds.push(caseManager.CaseManagerID);
        else
            $scope.SelectedCaseManagerIds.remove(caseManager.CaseManagerID);

        if ($scope.SelectedCaseManagerIds.length == $scope.CaseManagerListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedCaseManagerIds = [];

        angular.forEach($scope.CaseManagerList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedCaseManagerIds.push(item.CaseManagerID);
        });

        return true;
    };

    $scope.DeleteCaseManager = function (caseManagerID, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchCaseManagerListPage.ListOfIdsInCsv = caseManagerID > 0 ? caseManagerID.toString() : $scope.SelectedCaseManagerIds.toString();

                if (caseManagerID > 0) {
                    if ($scope.CaseManagerListPager.currentPage != 1)
                        $scope.CaseManagerListPager.currentPage = $scope.CaseManagerList.length === 1 ? $scope.CaseManagerListPager.currentPage - 1 : $scope.CaseManagerListPager.currentPage;
                } else {

                    if ($scope.CaseManagerListPager.currentPage != 1 && $scope.SelectedCaseManagerIds.length == $scope.CaseManagerListPager.currentPageSize)
                        $scope.CaseManagerListPager.currentPage = $scope.CaseManagerListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedCaseManagerIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.CaseManagerListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteCaseManager, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.CaseManagerList = response.Data.Items;
                        $scope.CaseManagerListPager.currentPageSize = response.Data.Items.length;
                        $scope.CaseManagerListPager.totalRecords = response.Data.TotalItems;
                    }
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.CaseManagerListPager.getDataCallback = $scope.GetCaseManagerList;
    $scope.CaseManagerListPager.getDataCallback();



    $scope.GotoReferralList = function (cmId) {
        //window.location = SiteUrl.ReferralListURL;
        SetCookie(cmId, 'CM_ID');
        window.open(SiteUrl.ReferralListURL, '_blank');
    };
    $scope.CaseManagerEditModel = function (EncryptedCaseManagerID, title) {
        var EncryptedCaseManagerID = EncryptedCaseManagerID;
        $('#CaseManager_fixedAside').modal({ backdrop: 'static', keyboard: false });
        $('#CaseManager_fixedAsidelDDLBindIFrame').attr('src', HomeCareSiteUrl.PartialAddCaseManagerURL + EncryptedCaseManagerID);
    }
    $scope.CaseManagerEditModelClosed = function () {
        $scope.Refresh();
        $('#CaseManager_fixedAside').modal('hide');
    }
    
};

controllers.CaseManagerListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowCaseManagerMessage");
});
