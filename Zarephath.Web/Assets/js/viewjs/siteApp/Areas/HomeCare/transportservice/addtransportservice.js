var vm;
controllers.AddTransportContactController = function ($scope, $http, $window, $timeout) {
    vm = $scope;
    $scope.SearchOrganizationNameURL = HomeCareSiteUrl.SearchOrganizationNameURL;
    /*Start Add Transport Contact */
    $scope.SetTransportContactModel = $.parseJSON($("#hdnTransportContactModel").val());
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnTransportContactModel").val());
    };
    $scope.TransportContactModel = $scope.SetTransportContactModel.TransportContactModel;
    $scope.SearchVehicleModel = $scope.SetTransportContactModel.SearchVehicleModel;
    $scope.SaveTransportContactDetails = function () {
        var isValid = CheckErrors($("#frmaddTransportContact"));
        if (isValid) {
            var jsonData = angular.toJson({ transportContactModel: $scope.SetTransportContactModel });
            AngularAjaxCall($http, HomeCareSiteUrl.AddTransportContactURL, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        if ($scope.TransportContactModel.ContactID == 0 || $scope.TransportContactModel.ContactID == undefined) {
                            toastr.success("Transport Contact Save Successfully");
                            $scope.Reset();
                        } else {
                            toastr.success("Transport Contact Update Successfully");
                        }

                    } else {
                        ShowMessages(response);
                    }
                });
        }
    };

    $scope.Reset = function () {
        $scope.TransportContactModel = '';
    };

    $scope.Cancel = function () {
        window.location.reload();
    };
    $scope.VehicleListPager = new PagerModule("VehicleType", undefined, "ASC");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchVehicleModel: $scope.SearchVehicleModel,
            pageSize: $scope.VehicleListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.VehicleListPager.sortIndex,
            sortDirection: $scope.VehicleListPager.sortDirection
        };
        if ($scope.TransportContactModel.ContactID == 0 || $scope.TransportContactModel.ContactID == undefined) {
            pagermodel.searchVehicleModel.ContactID = 0;
        } else {
            pagermodel.searchVehicleModel.ContactID = $scope.TransportContactModel.ContactID;
        }
        return angular.toJson(pagermodel);
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
    $scope.VehicleListPager.getDataCallback = $scope.GetVehicleList;
    $scope.VehicleListPager.getDataCallback();
    $scope.VehicleEditModel = function (EncryptedVehicleID, title) {
        var EncryptedVehicleID = EncryptedVehicleID;
        $('#Vehicle_fixedAside').modal({ backdrop: 'static', keyboard: false });
        $('#Vehicle_fixedAsidelDDLBindIFrame').attr('src', HomeCareSiteUrl.PartialAddVehicleURL + EncryptedVehicleID);
    }
    $scope.VehicleEditModelClosed = function () {
        $scope.Refresh();
        $('#Vehicle_fixedAside').modal('hide');
    }
    $scope.Refresh = function () {
        $scope.VehicleListPager.getDataCallback();
    };

    if ($scope.TransportContactModel.OrganizationID != null) {
        
        $timeout(function () {
            
            $scope.RegionTokenObj.add(
                {
                    OrganizationID: $scope.TransportContactModel.OrganizationID
                });
        });
    }

    $scope.RegionTokenObj = {};
    //if ($scope.TransportContactModel.OrganizationID != null) {

    //    $scope.RegionTokenObj.add(
    //        {
    //            OrganizationID: $scope.TransportContactModel.OrganizationID
    //        });

    //}
    $scope.RegionResultsFormatter = function (item) {
        return "<li id='{0}'>{1}</li>".format(item.OrganizationID, item.OrganizationID);
    };
    $scope.RegionTokenFormatter = function (item) {
        //$scope.ReferralModel.Referral.RegionName = item.RegionName;
        return "<li id='{0}'>{1}</li>".format(item.OrganizationID, item.OrganizationID);
    };
    $scope.RemoveRegion = function () {
        $scope.TransportContactModel.OrganizationID = null;
        $scope.RegionTokenObj.clear();
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };
    /* End Add Transport Contact */

};
controllers.AddTransportContactController.$inject = ['$scope', '$http', '$window', '$timeout'];

$(document).ready(function () {


});