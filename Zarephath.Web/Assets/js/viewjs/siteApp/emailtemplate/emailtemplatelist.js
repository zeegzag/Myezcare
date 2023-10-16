var vm;

controllers.EmailTemplateListController = function ($scope, $http) {

    vm = $scope;
    $scope.SetAddEmailTemplatePage = SiteUrl.AddEmailTemplate;
    $scope.EmailTemplateList = [];
    $scope.SelectedEmailTemplateIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetEmailTemplateListPage").val());
    };
    $scope.EmailTemplateModel = $.parseJSON($("#hdnSetEmailTemplateListPage").val());
    $scope.SearchEmailTemplateListPage = $scope.newInstance().SearchEmailTemplateListPage;
    $scope.TempEmailTemplateListPage = $scope.newInstance().SearchEmailTemplateListPage;
    $scope.EmailTemplateListPager = new PagerModule("OrderNumber");

    $scope.SetPostData = function (fromIndex) {

        var pagermodel = {
            searchEmailTemplateListPage: $scope.SearchEmailTemplateListPage,
            pageSize: $scope.EmailTemplateListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.EmailTemplateListPager.sortIndex,
            sortDirection: $scope.EmailTemplateListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchEmailTemplateListPage = $.parseJSON(angular.toJson($scope.TempEmailTemplateListPage));
    };

    $scope.GetEmailTemplateList = function (isSearchDataMappingRequire) {

        //Reset Selcted Checkbox items and Control
        $scope.SelectedEmailTemplateIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchEmailTemplateListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();

        $scope.SearchEmailTemplateListPage.Module = $("#ddlModuleName")
        $scope.SearchEmailTemplateListPage.EmailType = $("#EmailTemplateTypeIDs");
        var jsonData = $scope.SetPostData($scope.EmailTemplateListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetEmailTemplateList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.EmailTemplateList = response.Data.Items;
                $scope.EmailTemplateListPager.currentPageSize = response.Data.Items.length;
                $scope.EmailTemplateListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.SearchEmailTemplateListPage = $scope.newInstance().SearchEmailTemplateListPage;
        $scope.TempEmailTemplateListPage = $scope.newInstance().TempEmailTemplateListPage;
        $scope.EmailTemplateListPager.currentPage = 1;
        $scope.EmailTemplateListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        //Reset 
        $scope.SearchEmailTemplateListPage = $scope.newInstance().SearchEmailTemplateListPage;
        $scope.TempEmailTemplateListPage = $scope.newInstance().TempEmailTemplateListPage;
        //$scope.TempEmailTemplateListPage.IsDeleted = "0";
        $scope.EmailTemplateListPager.currentPage = 1;
        $scope.SearchEmailTemplateListPage.Module = "0";
        $scope.SearchEmailTemplateListPage.EmailType = "0";
        $scope.EmailTemplateListPager.getDataCallback();
    };

    $scope.SearchEmailTemplate = function () {
        $scope.EmailTemplateListPager.currentPage = 1;
        $scope.EmailTemplateListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectEmailTemplate = function (emailTemplateList) {
        if (emailTemplateList.IsChecked)
            $scope.SelectedEmailTemplateIds.push(emailTemplateList.EmailTemplateID);
        else
            $scope.SelectedEmailTemplateIds.remove(emailTemplateList.EmailTemplateID);

        if ($scope.SelectedEmailTemplateIds.length === $scope.EmailTemplateListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedEmailTemplateIds = [];
        angular.forEach($scope.EmailTemplateList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedEmailTemplateIds.push(item.EmailTemplateID);
        });
        return true;
    };

    $scope.DeleteEmailTamplate = function (emailTemplateId) {
        bootboxDialog(function (result) {
            if (result) {

                var jsonData = angular.toJson({ tempid: emailTemplateId });
                AngularAjaxCall($http, SiteUrl.DeleteEmailTemplateList, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.Refresh();
                        ShowMessage(response.Message, "error");
                    }

                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, "Are you sure want to delete template", bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };
    $scope.OpenBody = function (emailTemplateId) {

        var jsonData = angular.toJson({ tempid: emailTemplateId });
        AngularAjaxCall($http, "/emailtemplate/GetTemplateBody/", jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                var myWindow = window.open("", "Template Body", "width=500,height=300");
                myWindow.document.write(response.Data.EmailTemplateBody);

            }

        });



    };




    $scope.EmailTemplateListPager.getDataCallback = $scope.GetEmailTemplateList;
    $scope.EmailTemplateListPager.getDataCallback();
};

controllers.EmailTemplateListController.$inject = ['$scope', '$http'];

//$(document).ready(function () {
//    ShowPageLoadMessage("EmailTemplateUpdateSuccessMessage");
//});