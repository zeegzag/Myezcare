var vm;
controllers.AgencyListController = function ($scope, $http) {
    vm = $scope;
    $scope.AgencyModelListURL = HomeCareSiteUrl.AddAgencyURL;
    $scope.AgencyList = [];
    $scope.SelectedAgencyIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetAgencyListPage").val());
    };

    $scope.AgencyModel = $.parseJSON($("#hdnSetAgencyListPage").val());
    $scope.SearchAgencyListPage = $scope.AgencyModel.SearchAgencyListPage;
    $scope.TempAgencyListPage = $scope.AgencyModel.SearchAgencyListPage;

    $scope.AgencyListPager = new PagerModule("NickName");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchAgencyListPage: $scope.SearchAgencyListPage,
            pageSize: $scope.AgencyListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.AgencyListPager.sortIndex,
            sortDirection: $scope.AgencyListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchAgencyListPage = $.parseJSON(angular.toJson($scope.TempAgencyListPage));
    };

    $scope.GetAgencyList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedAgencyIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchAgencyListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();

        var jsonData = $scope.SetPostData($scope.AgencyListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetAgencyList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.AgencyList = response.Data.Items;
                $scope.AgencyListPager.currentPageSize = response.Data.Items.length;
                $scope.AgencyListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.SearchAgencyListPage = $scope.newInstance().SearchAgencyListPage;
        $scope.TempAgencyListPage = $scope.newInstance().SearchAgencyListPage;
        $scope.TempAgencyListPage.IsDeleted = "0";
        $scope.AgencyListPager.currentPage = 1;
        $scope.AgencyListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        //Reset 
        $scope.SearchAgencyListPage = $scope.newInstance().SearchAgencyListPage;
        $scope.TempAgencyListPage = $scope.newInstance().SearchAgencyListPage;
        $scope.TempAgencyListPage.IsDeleted = "0";
        $scope.AgencyListPager.currentPage = 1;
        $scope.AgencyListPager.getDataCallback();
    };

    $scope.SearchAgency = function () {
        $scope.AgencyListPager.currentPage = 1;
        $scope.AgencyListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectAgency = function (agency) {
        if (agency.IsChecked)
            $scope.SelectedAgencyIds.push(agency.AgencyID);
        else
            $scope.SelectedAgencyIds.remove(agency.AgencyID);

        if ($scope.SelectedAgencyIds.length == $scope.AgencyListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedAgencyIds = [];
        angular.forEach($scope.AgencyList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedAgencyIds.push(item.AgencyID);
        });
        return true;
    };

    $scope.DeleteAgencyList = function (agencyid, title) {

        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchAgencyListPage.ListOfIdsInCsv = agencyid > 0 ? agencyid.toString() : $scope.SelectedAgencyIds.toString();
                if (agencyid > 0) {
                    if ($scope.AgencyListPager.currentPage != 1)
                        $scope.AgencyListPager.currentPage = $scope.AgencyList.length === 1 ? $scope.AgencyList.currentPage - 1 : $scope.AgencyList.currentPage;
                }
                else {
                    if ($scope.AgencyListPager.currentPage != 1 && $scope.SelectedAgencyIds.length == $scope.AgencyListPager.currentPageSize)
                        $scope.AgencyListPager.currentPage = $scope.AgencyListPager.currentPage - 1;
                }
                //Reset Selcted Checkbox items and Control
                $scope.SelectedAgencyIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Controlpe.SelectAllCheckbox = false;

                var jsonData = $scope.SetPostData($scope.AgencyListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteAgencyListUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.AgencyList = response.Data.Items;
                        $scope.AgencyListPager.currentPageSize = response.Data.Items.length;
                        $scope.AgencyListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.AgencyListPager.getDataCallback = $scope.GetAgencyList;
    $scope.AgencyListPager.getDataCallback();

    $scope.EmployeeEditModel = function (EncryptedEmployeeID, title) {
        var EncryptedEmployeeID = EncryptedEmployeeID;
        $('#Agency_fixedAside').modal({ backdrop: 'static', keyboard: false });
        $('#Agency_fixedAsidelDDLBindIFrame').attr('src', HomeCareSiteUrl.PartialAddAgencyURL + EncryptedEmployeeID);
    }
    $scope.EmployeeEditModelClosed = function () {
        $scope.Refresh();
        $('#Agency_fixedAside').modal('hide');
    }
};

controllers.AgencyListController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    ShowPageLoadMessage("AgencyUpdateSuccessMessage");
});
