var vm;


controllers.DxCodeListController = function ($scope, $http, $timeout) {
    vm = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetDxCodeListPage").val());
    };

    $scope.AddDxCodeURL = SiteUrl.AddDxCodeURL;
    $scope.DxCodeList = [];
    $scope.SelectedDxCodeIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.DxCodeModel = $.parseJSON($("#hdnSetDxCodeListPage").val());
    $scope.SearchDxCodeListPage = $scope.DxCodeModel.SearchDxCodeListPage;
    $scope.TempSearchDxCodeListPage = $scope.DxCodeModel.SearchDxCodeListPage;

    $scope.DxCodeListPager = new PagerModule("DXCodeName");

    $scope.SetPostData = function (fromIndex) {

        var pagermodel = {
            SearchDxCodeListPage: $scope.SearchDxCodeListPage,
            pageSize: $scope.DxCodeListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.DxCodeListPager.sortIndex,
            sortDirection: $scope.DxCodeListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchDxCodeListPage = $.parseJSON(angular.toJson($scope.TempSearchDxCodeListPage));
      
    };

    $scope.GetDxCodeList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedDxCodeIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchDxCodeListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping
        
        var jsonData = $scope.SetPostData($scope.DxCodeListPager.currentPage);

        AngularAjaxCall($http, SiteUrl.GetDxCodeList, jsonData, "Post", "json", "application/json").success(function (response) {

            if (response.IsSuccess) {
                $scope.DxCodeList = response.Data.Items;
                $scope.DxCodeListPager.currentPageSize = response.Data.Items.length;
                $scope.DxCodeListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        //$scope.ResetSearchFilter();
        //$scope.CaseManagerListPager.currentPage = $scope.CaseManagerListPager.currentPage;
        $scope.DxCodeListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');
            $scope.SearchDxCodeListPage = $scope.newInstance().SearchDxCodeListPage;
            $scope.TempSearchDxCodeListPage = $scope.newInstance().SearchDxCodeListPage;
            $scope.TempSearchDxCodeListPage.IsDeleted = "0";
            $scope.DxCodeListPager.currentPage = 1;
            $scope.DxCodeListPager.getDataCallback();
        });
    };
    $scope.SearchDxCode = function () {
        $scope.DxCodeListPager.currentPage = 1;
        $scope.DxCodeListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectDxCode = function (dxcode) {
        if (dxcode.IsChecked)
            $scope.SelectedDxCodeIds.push(dxcode.DXCodeID);
        else
            $scope.SelectedDxCodeIds.remove(dxcode.DXCodeID);

        if ($scope.SelectedDxCodeIds.length == $scope.DxCodeListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedDxCodeIds = [];

        angular.forEach($scope.DxCodeList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedDxCodeIds.push(item.DXCodeID);
        });
        return true;
    };

    $scope.DeleteDxCode = function (dxCodeId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchDxCodeListPage.ListOfIdsInCsv = dxCodeId > 0 ? dxCodeId.toString() : $scope.SelectedDxCodeIds.toString();

                if (dxCodeId > 0) {
                    if ($scope.DxCodeListPager.currentPage != 1)
                        $scope.DxCodeListPager.currentPage = $scope.DxCodeList.length === 1 ? $scope.DxCodeListPager.currentPage - 1 : $scope.DxCodeListPager.currentPage;
                } else {

                    if ($scope.DxCodeListPager.currentPage != 1 && $scope.SelectedDxCodeIds.length == $scope.DxCodeListPager.currentPageSize)
                        $scope.DxCodeListPager.currentPage = $scope.DxCodeListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedDxCodeIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.DxCodeListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteDxCode, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.DxCodeList = response.Data.Items;
                        $scope.DxCodeListPager.currentPageSize = response.Data.Items.length;
                        $scope.DxCodeListPager.totalRecords = response.Data.TotalItems;
                    }
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
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
    $scope.DxCodeListPager.getDataCallback = $scope.GetDxCodeList;
    $scope.DxCodeListPager.getDataCallback();
};

controllers.DxCodeListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowDxCodeMessage");
    //$(".dateInputMask").inputmask("m/d/y", {
    //    placeholder: "mm/dd/yyyy"
    //});
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});
