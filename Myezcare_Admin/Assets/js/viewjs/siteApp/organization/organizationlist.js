var vm;
controllers.OrganizationListController = function ($scope, $http, $window, $timeout) {
    vm = $scope;
    $scope.AddOrganizationURL = SiteUrl.AddOrganizationURL;
    $scope.OrganizationEsignURL = SiteUrl.OrganizationEsignURL;

    $scope.SetOrganizationListModel = $.parseJSON($("#hdnOrganizationListPage").val());

    //#region Organization List Page

    $scope.OrganizationList = [];
    $scope.SearchOrganizationModel = $scope.SetOrganizationListModel.SearchOrganizationModel;
    $scope.SelectedOrganizationIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.OrganizationListPager = new PagerModule("OrganizationID", undefined, "ASC");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            SearchOrganizationModel: $scope.SearchOrganizationModel,
            pageSize: $scope.OrganizationListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.OrganizationListPager.sortIndex,
            sortDirection: $scope.OrganizationListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.GetOrganizationList = function () {
        var jsonData = $scope.SetPostData($scope.OrganizationListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetOrganizationListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.SelectedOrganizationIds = [];
                $scope.SelectAllCheckbox = false;
                $scope.OrganizationList = response.Data.Items;
                $scope.OrganizationListPager.currentPageSize = response.Data.Items.length;
                $scope.OrganizationListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.OrganizationListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $scope.SearchOrganizationModel = $.parseJSON($("#hdnOrganizationListPage").val()).SearchOrganizationModel;
        $scope.SearchOrganizationModel.IsActive = "1";
        $scope.OrganizationListPager.currentPage = 1;
        $scope.OrganizationListPager.getDataCallback();
    };

    $scope.SearchOrganization = function () {
        $scope.OrganizationListPager.currentPage = 1;
        $scope.OrganizationListPager.getDataCallback();
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

    $scope.SelectAll = function () {
        $scope.SelectedOrganizationIds = [];

        angular.forEach($scope.OrganizationList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedOrganizationIds.push(item.OrganizationID);
        });
        return true;
    };

    $scope.OrganizationListPager.getDataCallback = $scope.GetOrganizationList;
    $scope.OrganizationListPager.getDataCallback();
    
    $scope.SelectedEncryptedOrganizationID = 0;
    $scope.UploadExcel = function (organization) {
        $scope.SelectedEncryptedOrganizationID = organization.EncryptedOrganizationID;
        $('#uploadmodal').modal({ backdrop: false, keyboard: false });
        $("#uploadmodal").modal('show');
    }

    //#endregion


    //#region Organization Add Page

    //addOrgModal
    $scope.OpenAddOrgModal = function () {
        $('#addOrgModal').modal({ backdrop: false, keyboard: false });
        $("#addOrgModal").modal('show');
    }

    $scope.CloseAddOrgModal = function () {
        $scope.AddOrganizationModel = {};
        $("#addOrgModal").modal('hide');
        HideErrors("#addOrgModal");
    }


    $scope.SaveOrgnization = function () {
        if (CheckErrors("#frmSaveOrganization")) {
            var jsonData = angular.toJson($scope.AddOrganizationModel);
            AngularAjaxCall($http, SiteUrl.SaveOrganizationUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.GetOrganizationList();
                    $scope.CloseAddOrgModal();
                }
                ShowMessages(response);
            });
        }
    }

    $scope.SendEsignEmail = function (organization) {

        bootboxDialog(function (result) {
            if (result) {
                var jsonData = angular.toJson(organization);
                AngularAjaxCall($http, SiteUrl.SendEsignEmailURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        organization.OrganizationStatusID = organizationStatusEsignEmailSent;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, window.sendEmail, window.sendEmailConfirmation, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    }

    $scope.EsignPreview = function (organization) {
        alert("Coming soon...");
    }

    //#endregion
};
controllers.OrganizationListController.$inject = ['$scope', '$http', '$window', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("OrganizationUpdateSuccessMessage");
});
