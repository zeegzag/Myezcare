var custModel;

controllers.AddGeneralMasterController = function ($scope, $http, $timeout, $window) {
    custModel = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnGeneralMasterModel").val());
    };


    $scope.DDMasterList = [];
    $scope.SelectedDDMasterIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.DDMasterModel = $.parseJSON($("#hdnGeneralMasterModel").val());
    $scope.SearchDDMasterListPage = $scope.DDMasterModel.SearchDDMasterListPage;
    $scope.TempSearchDDMasterListPage = $scope.DDMasterModel.SearchDDMasterListPage;
    
    $scope.SaveDDMaster = function () {
        var isValid = CheckErrors($("#frmDDMaster"));
        if (isValid) {
            var jsonData = angular.toJson({ DDMaster: $scope.DDMasterModel.DDMaster });
            AngularAjaxCall($http, HomeCareSiteUrl.SaveDDmasterURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    $scope.DDMasterModel.DDMaster = $scope.newInstance().DDMaster;
                    ShowMessages(response);
                    $scope.GetGeneralMasterList();
                });
        }
    };

    $scope.Cancel = function () {
        $scope.DDMasterModel.DDMaster = $scope.newInstance().DDMaster;
    };

    $scope.EditDDMaster = function (data) {
        $scope.DDMasterModel.DDMaster.DDMasterID = data.DDMasterID;
        $scope.DDMasterModel.DDMaster.ItemType = data.DDMasterTypeID;
        $scope.DDMasterModel.DDMaster.Title = data.Title;
    }


    $scope.DDMasterListPager = new PagerModule("DDMasterID", "", "DESC");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            SearchDDMasterListPage: $scope.SearchDDMasterListPage,
            pageSize: $scope.DDMasterListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.DDMasterListPager.sortIndex,
            sortDirection: $scope.DDMasterListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchDDMasterListPage = $.parseJSON(angular.toJson($scope.TempSearchDDMasterListPage));
    };

    $scope.GetGeneralMasterList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedDDMasterIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchDDMasterListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.DDMasterListPager.currentPage);

        AngularAjaxCall($http, HomeCareSiteUrl.GetGeneralMasterList, jsonData, "Post", "json", "application/json").success(function (response) {

            if (response.IsSuccess) {
                $scope.DDMasterList = response.Data.Items;
                $scope.DDMasterListPager.currentPageSize = response.Data.Items.length;
                $scope.DDMasterListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            $scope.SearchDDMasterListPage = $scope.newInstance().SearchDDMasterListPage;
            $scope.TempSearchDDMasterListPage = $scope.newInstance().SearchDDMasterListPage;
            $scope.TempSearchDDMasterListPage.IsDeleted = "0";
            $scope.DDMasterListPager.currentPage = 1;
            $scope.DDMasterListPager.getDataCallback();
        });
    };
    $scope.SearchDDMaster = function () {
        $scope.DDMasterListPager.currentPage = 1;
        $scope.DDMasterListPager.getDataCallback(true);
    };

    $scope.SelectDDMaster = function (DDMaster) {
        if (DDMaster.IsChecked)
            $scope.SelectedDDMasterIds.push(DDMaster.DDMasterID);
        else
            $scope.SelectedDDMasterIds.remove(DDMaster.DDMasterID);

        if ($scope.SelectedDDMasterIds.length == $scope.DDMasterListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    $scope.SelectAll = function () {
        $scope.SelectedDDMasterIds = [];

        angular.forEach($scope.DDMasterList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedDDMasterIds.push(item.DDMasterID);
        });
        return true;
    };

    $scope.DeleteDDMaster = function (DDMasterId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchDDMasterListPage.ListOfIdsInCsv = DDMasterId > 0 ? DDMasterId.toString() : $scope.SelectedDDMasterIds.toString();

                if (DDMasterId > 0) {
                    if ($scope.DDMasterListPager.currentPage != 1)
                        $scope.DDMasterListPager.currentPage = $scope.DDMasterList.length === 1 ? $scope.DDMasterListPager.currentPage - 1 : $scope.DDMasterListPager.currentPage;
                } else {

                    if ($scope.DDMasterListPager.currentPage != 1 && $scope.SelectedDDMasterIds.length == $scope.DDMasterListPager.currentPageSize)
                        $scope.DDMasterListPager.currentPage = $scope.DDMasterListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedDDMasterIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.DDMasterListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteDDMaster, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    //if (response.IsSuccess) {
                        $scope.DDMasterList = response.Data.Items;
                        $scope.DDMasterListPager.currentPageSize = response.Data.Items.length;
                        $scope.DDMasterListPager.totalRecords = response.Data.TotalItems;
                    //}
                });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };


    $scope.DDMasterListPager.getDataCallback = $scope.GetGeneralMasterList;
    $scope.DDMasterListPager.getDataCallback();
};

controllers.AddGeneralMasterController.$inject = ['$scope', '$http', '$timeout', '$window'];

$(document).ready(function () {
});