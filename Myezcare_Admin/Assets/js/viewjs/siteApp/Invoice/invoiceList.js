var vm;
controllers.InvoiceListController = function ($scope, $http, $window, $timeout) {
    vm = $scope;
    $scope.SetInvoiceListModel = $.parseJSON($("#hdnInvoiceListPage").val());

    //#region Invoice List Page

    $scope.InvoiceList = [];
    $scope.SearchInvoiceModel = $scope.SetInvoiceListModel;
    $scope.SelectedInvoiceIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.InvoiceListPager = new PagerModule("CompanyName", undefined, "ASC");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            invoiceModel: $scope.SearchInvoiceModel,
            pageSize: $scope.InvoiceListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.InvoiceListPager.sortIndex,
            sortDirection: $scope.InvoiceListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.GetInvoiceList = function (val) {
        debugger;
        $scope.SearchInvoiceModel.IsAll;
        var jsonData = $scope.SetPostData($scope.InvoiceListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.InvoiceGetInvoiceListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.SelectedOrganizationIds = [];
                $scope.SelectAllCheckbox = false;
                $scope.InvoiceList = response.Data.Items;

                if ($scope.InvoiceList.length > 0) {
                    $.grep($scope.InvoiceList, function (val1) {
                        if (val1.InvoiceStatusName == "Paid" || val1.InvoiceStatusName == "Unpaid") {
                            val1.InvoiceStatusName1 = "Active";
                        }
                        if (val1.InvoiceStatusName == "Overdue") {
                            val1.InvoiceStatusName1 = "Block";
                        } if (val1.InvoiceStatusName == "WriteOff") {
                            val1.InvoiceStatusName1 = "Active";
                        }
                    });
                }
                $scope.InvoiceListPager.currentPageSize = response.Data.Items.length;
                $scope.InvoiceListPager.totalRecords = response.Data.TotalItems;
                console.log('InvoiceList=', $scope.InvoiceList);
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.SearchInvoiceModel.IsAll = true;
        $scope.InvoiceListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $scope.SearchInvoiceModel.IsAll = true;

        $scope.SearchInvoiceModel.OrganizationName = '';
        $scope.SearchInvoiceModel.InvoiceDate = '';
        $scope.SearchInvoiceModel.DueDate = '';
        $scope.SearchInvoiceModel.InvoiceStatus = '';
        $scope.SearchInvoiceModel.InvoiceAmount = '';
        $scope.SearchInvoiceModel.PaidAmount = '';
        $scope.SearchInvoiceModel.AccountStauts = '';
        $scope.SearchInvoiceModel.AccountStauts = '';
        $scope.SearchInvoiceModel.IsActive = "1";
        $scope.InvoiceListPager.currentPage = 1;
        $scope.InvoiceListPager.getDataCallback();
    };

    $scope.SearchOrganization = function () {
        $scope.SearchInvoiceModel.IsAll = false;
        $scope.InvoiceListPager.currentPage = 1;
        $scope.InvoiceListPager.getDataCallback();
    };

    $scope.SelectOrganization = function (organization) {
        if (organization.IsChecked)
            $scope.SelectedOrganizationIds.push(organization.OrganizationID);
        else
            $scope.SelectedOrganizationIds.remove(organization.OrganizationID);

        if ($scope.SelectedOrganizationIds.length == $scope.OrganizationListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    $scope.InvoiceListPager.getDataCallback = $scope.GetInvoiceList;
    $scope.InvoiceListPager.getDataCallback();

    $scope.SelectedEncryptedOrganizationID = 0;


    //#endregion

    //#region PageOnload
    $scope.onload = function () {
        $scope.SearchInvoiceModel.IsAll = true;
        $scope.GetInvoiceList();
    };
    $scope.onload();
    //#endregion

};
controllers.InvoiceListController.$inject = ['$scope', '$http', '$window', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("OrganizationUpdateSuccessMessage");
});
