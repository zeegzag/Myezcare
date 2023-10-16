var vm;
controllers.TransportContactListController = function ($scope, $http, $window, $timeout) {
    vm = $scope;
    $scope.AddTransportContactURL = HomeCareSiteUrl.AddTransportContactURL;

    $scope.SetTransportContactListModel = $.parseJSON($("#hdnTransportContactListPage").val());

    $scope.TransportContactList = [];
    $scope.SearchTransportContactModel = $scope.SetTransportContactListModel.SearchTransportContactModel;
    if ($scope.SearchTransportContactModel != undefined) {
        $scope.SearchTransportContactModel.ContactID = $("#ContactID").val();
    }

    $scope.SelectedTransportContactIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.TransportContactListPager = new PagerModule("ContactType", undefined, "ASC");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchTransportContactModel: $scope.SearchTransportContactModel,
            pageSize: $scope.TransportContactListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.TransportContactListPager.sortIndex,
            sortDirection: $scope.TransportContactListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.GetTransportContactList = function () {
        var jsonData = $scope.SetPostData($scope.TransportContactListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetTransportContactListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.SelectedTransportContactIds = [];
                $scope.SelectAllCheckbox = false;
                $scope.TransportContactList = response.Data.Items;
                $scope.TransportContactListPager.currentPageSize = response.Data.Items.length;
                $scope.TransportContactListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.TransportContactListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $scope.SearchTransportContactModel = $.parseJSON($("#hdnTransportContactListPage").val()).SearchTransportContactModel;
        if ($scope.SearchTransportContactModel != undefined) {
            $scope.SearchTransportContactModel.ContactID = $("#ContactID").val();
        }
        $scope.SearchTransportContactModel.IsDeleted = "0";
        $scope.TransportContactListPager.currentPage = 1;
        $scope.TransportContactListPager.getDataCallback();
    };

    $scope.SearchTransportContact = function () {
        $scope.TransportContactListPager.currentPage = 1;
        $scope.TransportContactListPager.getDataCallback();
    };

    $scope.SelectTransportContact = function (transportContact) {
        if (transportContact.IsChecked)
            $scope.SelectedTransportContactIds.push(transportContact.ContactID);
        else
            $scope.SelectedTransportContactIds.remove(transportContact.ContactID);

        if ($scope.SelectedTransportContactIds.length == $scope.TransportContactListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    $scope.SelectAll = function () {
        $scope.SelectedTransportContactIds = [];

        angular.forEach($scope.TransportContactList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedTransportContactIds.push(item.ContactID);
        });
        return true;
    };

    $scope.DeleteTransportContact = function (contactID, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchTransportContactModel.ListOfIdsInCsv = contactID > 0 ? contactID.toString() : $scope.SelectedTransportContactIds.toString();
                if (contactID > 0) {
                    if ($scope.TransportContactListPager.currentPage != 1)
                        $scope.TransportContactListPager.currentPage = $scope.TransportContactList.length === 1 ? $scope.TransportContactListPager.currentPage - 1 : $scope.TransportContactListPager.currentPage;
                } else {
                    if ($scope.TransportContactListPager.currentPage != 1 && $scope.SelectedTransportContactIds.length == $scope.TransportContactListPager.currentPageSize)
                        $scope.TransportContactListPager.currentPage = $scope.TransportContactListPager.currentPage - 1;
                }

                $scope.SelectedTransportContactIds = [];
                $scope.SelectAllCheckbox = false;

                var jsonData = $scope.SetPostData($scope.TransportContactListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteTransportContactURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.TransportContactList = response.Data.Items;
                        $scope.TransportContactListPager.currentPageSize = response.Data.Items.length;
                        $scope.TransportContactListPager.totalRecords = response.Data.TotalItems;
                        ShowMessages(response);
                    } else {
                        bootboxDialog(function () {
                        }, bootboxDialogType.Alert, window.Alert, window.FacilityHouseScheduleExistMessage);
                    }

                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.TransportContactListPager.getDataCallback = $scope.GetTransportContactList;
    $scope.TransportContactListPager.getDataCallback();

    $scope.TransportContactEditModel = function (EncryptedContactID, title) {
        var EncryptedContactID = EncryptedContactID;
        $('#TransportContact_fixedAside').modal({ backdrop: 'static', keyboard: false });
        $('#TransportContact_fixedAsidelDDLBindIFrame').attr('src', HomeCareSiteUrl.PartialAddTransportContactURL + EncryptedContactID);
    }

    $scope.OpenAddTransportContactModal = function (id) {
        $('#AddTransportContactIFrame').attr('src', 'about:blank');
        $('#AddTransportContactIFrame').attr('src', HomeCareSiteUrl.PartialAddTransportContactURL + id);
    }

    $scope.TransportContactEditModelClosed = function () {
        $scope.Refresh();
        $('#TransportContact_fixedAside').modal('hide');
    }

};
controllers.TransportContactListController.$inject = ['$scope', '$http', '$window', '$timeout'];

$(document).ready(function () {

});
