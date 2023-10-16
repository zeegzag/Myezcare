var vm;
controllers.VehicleListController = function ($scope, $http, $window, $timeout) {
    vm = $scope;
    $scope.AddVehicleURL = HomeCareSiteUrl.AddVehicleURL;

    $scope.SetVehicleListModel = $.parseJSON($("#hdnVehicleListPage").val());
    
    $scope.VehicleList = [];
    $scope.SearchVehicleModel = $scope.SetVehicleListModel.SearchVehicleModel;
    if ($scope.SearchVehicleModel != undefined) {
        $scope.SearchVehicleModel.VehicleID = $("#VehicleID").val();
    }

    $scope.SelectedVehicleIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.VehicleListPager = new PagerModule("VehicleType", undefined, "ASC");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchVehicleModel: $scope.SearchVehicleModel,
            pageSize: $scope.VehicleListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.VehicleListPager.sortIndex,
            sortDirection: $scope.VehicleListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.GetVehicleList = function () {
        var jsonData = $scope.SetPostData($scope.VehicleListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetVehicleListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.SelectedVehicleIds = [];
                $scope.SelectAllCheckbox = false;
                $scope.VehicleList = response.Data.Items;
                $scope.VehicleListPager.currentPageSize = response.Data.Items.length;
                $scope.VehicleListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.VehicleListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $scope.SearchVehicleModel = $.parseJSON($("#hdnVehicleListPage").val()).SearchVehicleModel;
        if ($scope.SearchVehicleModel != undefined) {
            $scope.SearchVehicleModel.VehicleID = $("#VehicleID").val();
        }
        $scope.SearchVehicleModel.IsDeleted = "0";
        $scope.VehicleListPager.currentPage = 1;
        $scope.VehicleListPager.getDataCallback();
    };

    $scope.SearchVehicle = function () {
        $scope.VehicleListPager.currentPage = 1;
        $scope.VehicleListPager.getDataCallback();
    };

    $scope.SelectVehicle = function (vehicle) {
        if (vehicle.IsChecked)
            $scope.SelectedVehicleIds.push(vehicle.VehicleID);
        else
            $scope.SelectedVehicleIds.remove(vehicle.VehicleID);

        if ($scope.SelectedVehicleIds.length == $scope.VehicleListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    $scope.SelectAll = function () {
        $scope.SelectedVehicleIds = [];

        angular.forEach($scope.VehicleList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedVehicleIds.push(item.VehicleID);
        });
        return true;
    };

    $scope.DeleteVehicle = function (vehicleID, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchVehicleModel.ListOfIdsInCsv = vehicleID > 0 ? vehicleID.toString() : $scope.SelectedVehicleIds.toString();
                if (vehicleID > 0) {
                    if ($scope.VehicleListPager.currentPage != 1)
                        $scope.VehicleListPager.currentPage = $scope.VehicleList.length === 1 ? $scope.VehicleListPager.currentPage - 1 : $scope.VehicleListPager.currentPage;
                } else {
                    if ($scope.VehicleListPager.currentPage != 1 && $scope.SelectedVehicleIds.length == $scope.VehicleListPager.currentPageSize)
                        $scope.VehicleListPager.currentPage = $scope.VehicleListPager.currentPage - 1;
                }

                $scope.SelectedVehicleIds = [];
                $scope.SelectAllCheckbox = false;

                var jsonData = $scope.SetPostData($scope.VehicleListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteVehicleURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.VehicleList = response.Data.Items;
                        $scope.VehicleListPager.currentPageSize = response.Data.Items.length;
                        $scope.VehicleListPager.totalRecords = response.Data.TotalItems;
                        ShowMessages(response);
                    } else {
                        bootboxDialog(function () {
                        }, bootboxDialogType.Alert, window.Alert, window.FacilityHouseScheduleExistMessage);
                    }

                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.VehicleListPager.getDataCallback = $scope.GetVehicleList;
    $scope.VehicleListPager.getDataCallback();

    $scope.VehicleEditModel = function (EncryptedVehicleID, title) {
        var EncryptedVehicleID = EncryptedVehicleID;
        $('#Vehicle_fixedAside').modal({ backdrop: 'static', keyboard: false });
        $('#Vehicle_fixedAsidelDDLBindIFrame').attr('src', HomeCareSiteUrl.PartialAddVehicleURL + EncryptedVehicleID);
    }

    $scope.OpenAddVehicleModal = function (id) {
        $('#AddVehicleIFrame').attr('src', 'about:blank');
        $('#AddVehicleIFrame').attr('src', HomeCareSiteUrl.PartialAddVehicleURL + id);
    }

    $scope.VehicleEditModelClosed = function () {
        $scope.Refresh();
        $('#Vehicle_fixedAside').modal('hide');
    }

};
controllers.VehicleListController.$inject = ['$scope', '$http', '$window', '$timeout'];

$(document).ready(function () {

});
