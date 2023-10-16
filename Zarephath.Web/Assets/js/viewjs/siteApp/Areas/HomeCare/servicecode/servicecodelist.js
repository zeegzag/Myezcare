var vm;

controllers.ServiceCodeListController = function ($scope, $http) {
    vm = $scope;
    $scope.SetAddServiceCodePage = HomeCareSiteUrl.SetAddServiceCodePageURL;
    $scope.ServiceCodeList = [];
    $scope.SelectedServiceCodeIds = [];
    $scope.SelectAllCheckbox = false;
    
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetServiceCodeListPage").val());
    };
    $scope.ServiceCodeListModel = $.parseJSON($("#hdnSetServiceCodeListPage").val());
    $scope.ModifierList = $scope.ServiceCodeListModel.ModifierList;
    $scope.SearchServiceCodeListPage = $scope.newInstance().SearchServiceCodeListPage;
    $scope.TempServiceCodeListPage = $scope.newInstance().SearchServiceCodeListPage;
    $scope.ServiceCodeListPager = new PagerModule("ServiceCodeName");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchServiceCodeListPage: $scope.SearchServiceCodeListPage,
            pageSize: $scope.ServiceCodeListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ServiceCodeListPager.sortIndex,
            sortDirection: $scope.ServiceCodeListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModifierID = [];

    $scope.SearchModelMapping = function () {
        if ($scope.SearchModifierID) {
            $scope.TempServiceCodeListPage.ModifierID = $scope.SearchModifierID.toString();
            $scope.SearchModifierID = $scope.TempServiceCodeListPage.ModifierID.split(",");
        }
        $scope.SearchServiceCodeListPage = $.parseJSON(angular.toJson($scope.TempServiceCodeListPage));
    };

    $scope.GetServiceCodeList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedServiceCodeIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchServiceCodeListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();

        var jsonData = $scope.SetPostData($scope.ServiceCodeListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetServiceCodeList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ServiceCodeList = response.Data.Items;
                $scope.ServiceCodeListPager.currentPageSize = response.Data.Items.length;
                $scope.ServiceCodeListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.ServiceCodeListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        //Reset 
        $scope.SearchServiceCodeListPage = $scope.newInstance().SearchServiceCodeListPage;
        $scope.TempServiceCodeListPage = $scope.newInstance().SearchServiceCodeListPage;
        $scope.TempServiceCodeListPage.IsDeleted = "0";
        $scope.ServiceCodeListPager.currentPage = 1;
        $scope.ServiceCodeListPager.getDataCallback();
        $scope.SearchModifierID = [];
    };

    $scope.SearchServiceCode = function () {
        $scope.ServiceCodeListPager.currentPage = 1;
        $scope.ServiceCodeListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectServiceCode = function (item) {
        if (item.IsChecked)
            $scope.SelectedServiceCodeIds.push(item.ServiceCodeID);
        else
            $scope.SelectedServiceCodeIds.remove(item.ServiceCodeID);
        
        if ($scope.SelectedServiceCodeIds.length == $scope.ServiceCodeListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedServiceCodeIds = [];
        angular.forEach($scope.ServiceCodeList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedServiceCodeIds.push(item.ServiceCodeID);
        });
        return true;
    };

    $scope.DeleteServiceCode = function (serviceCodeId, title) {
        
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchServiceCodeListPage.ListOfIdsInCSV = serviceCodeId > 0 ? serviceCodeId.toString() : $scope.SelectedServiceCodeIds.toString();
                if (serviceCodeId > 0) {
                    if ($scope.ServiceCodeListPager.currentPage != 1)
                        $scope.ServiceCodeListPager.currentPage = $scope.ServiceCodeList.length === 1 ? $scope.ServiceCodeList.currentPage - 1 : $scope.ServiceCodeList.currentPage;
                }
                else {
                    if ($scope.ServiceCodeListPager.currentPage != 1 && $scope.SelectedServiceCodeIds.length == $scope.ServiceCodeListPager.currentPageSize)
                        $scope.ServiceCodeListPager.currentPage = $scope.ServiceCodeListPager.currentPage - 1;
                }
                //Reset Selcted Checkbox items and Control
                $scope.SelectedServiceCodeIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.ServiceCodeListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteServiceCode, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.GetServiceCodeList();

                        //$scope.ServiceCodeList = response.Data.Items;
                        //$scope.ServiceCodeListPager.currentPageSize = response.Data.Items.length;
                        //$scope.ServiceCodeListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.OpenAddSSCModal = function (id) {
        if (id !== undefined) {
            AngularAjaxCall($http, HomeCareSiteUrl.SetGetServiceCodePageURL + id, null, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.$broadcast('loadChildData', response.Data.ServiceCodes);
                    $('#addservicecodemodel').modal({ backdrop: false, keyboard: false });
                    $("#addservicecodemodel").modal('show');
                }
            });
        } else {
            $('#addservicecodemodel').modal({ backdrop: false, keyboard: false });
            $("#addservicecodemodel").modal('show');
        }
    };

    $scope.CloseAddSSCModal = function () {
        $("#addservicecodemodel").modal('hide');
        HideErrors("#frmServiceCode");
        location.reload();
    };

    $scope.ServiceCodeListPager.getDataCallback = $scope.GetServiceCodeList;
    $scope.ServiceCodeListPager.getDataCallback();

    $scope.ServicecodeEditModel = function (EncryptedServiceCodeID, title) {
        var EncryptedServiceCodeID = EncryptedServiceCodeID;
        $('#servicecode_fixedAside').modal({ backdrop: 'static', keyboard: false });
        $('#servicecode_fixedAsidelDDLBindIFrame').attr('src', HomeCareSiteUrl.SetPartialAddServiceCodePageURL + EncryptedServiceCodeID);
    }
    $scope.ServicecodeEditModelClosed = function () {
        $scope.Refresh();
        $('#servicecode_fixedAside').modal('hide');
    }
};

controllers.ServiceCodeListController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowAddServiceCodeMessage");
});
