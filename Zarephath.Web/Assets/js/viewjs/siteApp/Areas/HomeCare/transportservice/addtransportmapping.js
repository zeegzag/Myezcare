var vm;

controllers.TransportMappingController = function ($scope, $http, $window) {
    vm = $scope;
    /*Start Add Transport Contact */
    $scope.SetTransportAssignmentModel = $.parseJSON($("#hdnSetTransportAssignmentModel").val());
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetTransportAssignmentModel").val());
    };
    $scope.TransportAssignmentModel = $scope.SetTransportAssignmentModel.TransportAssignmentModel;
    //$scope.SearchVehicleModel = $scope.SetTransportContactModel.SearchVehicleModel;
    $scope.SaveTransportAssignment = function () {
        var isValid = CheckErrors($("#frmaddTransportMapping"));
        if (isValid) {
            var jsonData = angular.toJson({ transportAssignmentModel: $scope.TransportAssignmentModel });
            AngularAjaxCall($http, HomeCareSiteUrl.SaveTransportAssignmentURL, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        if ($scope.TransportAssignmentModel.TransportID == 0 || $scope.TransportAssignmentModel.TransportID == undefined) {
                            toastr.success("Transport Contact Save Successfully");
                            $scope.Reset();
                            $scope.ResetSearchFilter();
                        } else {
                            toastr.success("Transport Contact Update Successfully");
                            $scope.Reset();
                            $scope.ResetSearchFilter();
                        }

                    } else {
                        ShowMessages(response);
                    }
                });
        }
    };
    $scope.Reset = function () {
        $scope.EditTransportID = 0;
        //$scope.TransportAssignmentModel = {};
        $scope.TransportAssignmentModel.TransportID = null;
        $scope.TransportAssignmentModel.FacilityID = '0';
        $scope.TransportAssignmentModel.VehicleID = '0';
        $scope.TransportAssignmentModel.OrganizationID = '0';
        $scope.TransportAssignmentModel.RouteCode = '0';
        $scope.TransportAssignmentModel.StartDate = '';
        $scope.TransportAssignmentModel.EndDate = '';
        $scope.TransportAssignmentModel.Attendent = null;
    };
    $scope.Cancel = function () {
        $scope.EditTransportID = 0;
        //$scope.TransportAssignmentModel = {};
        $scope.TransportAssignmentModel.TransportID = null;
        $scope.TransportAssignmentModel.FacilityID = '';
        $scope.TransportAssignmentModel.VehicleID = '';
        $scope.TransportAssignmentModel.OrganizationID = '';
        $scope.TransportAssignmentModel.RouteCode = '';
        $scope.TransportAssignmentModel.StartDate = '';
        $scope.TransportAssignmentModel.EndDate = '';
        $scope.TransportAssignmentModel.Attendent = null;
    };
    $scope.Delete = function (TransportID, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchTransportAssignmentModel.ListOfIdsInCsv = TransportID > 0 ? TransportID.toString() : $scope.SelectedVehicleIds.toString();
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
    $scope.DeleteTransportationLocation = function (transportLocationId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }

        bootboxDialog(function (result) {

            if (result) {
                $scope.SearchTransportAssignmentModel.ListOfIdsInCsv = transportLocationId > 0 ? transportLocationId.toString() : $scope.SelectedTransPortationIds.toString();
                if (transportLocationId > 0) {
                    if ($scope.TransportMappingListPager.currentPage != 1)
                        $scope.TransportMappingListPager.currentPage = $scope.TransPortationList.length === 1 ? $scope.TransPortationList.currentPage - 1 : $scope.TransPortationList.currentPage;
                }
                else {
                    if ($scope.TransportMappingListPager.currentPage != 1 && $scope.SelectedTransPortationIds.length == $scope.TransportMappingListPager.currentPageSize)
                        $scope.TransportMappingListPager.currentPage = $scope.TransportMappingListPager.currentPage - 1;
                }
                //Reset Selcted Checkbox items and Control
                $scope.SelectedTransPortationIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.TransportMappingListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteTransportAssignmentListURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.TransPortationList = response.Data.Items;
                        $scope.TransportMappingListPager.currentPageSize = response.Data.Items.length;
                        $scope.TransportMappingListPager.totalRecords = response.Data.TotalItems;
                        $scope.ResetSearchFilter();
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    /////////////////////////////////////////////////////////////////////////////////////

    //$scope.SetVehicleListModel = $.parseJSON($("#hdnVehicleListPage").val());

    $scope.TransportMappingList = [];
    $scope.SearchTransportAssignmentModel = $scope.SetTransportAssignmentModel.SearchTransportAssignmentModel;
    if ($scope.SearchTransportAssignmentModel != undefined) {
        $scope.SearchTransportAssignmentModel.TransportID = $("#TransportID").val();
    }

    $scope.SelectedVehicleIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.TransportMappingListPager = new PagerModule("TransportID", undefined, "DESC");
    $scope.EditTransportID = 0;
    $scope.SetPostData = function (fromIndex) {

        var pagermodel = {
            searchTransportAssignmentModel: $scope.SearchTransportAssignmentModel,
            pageSize: $scope.TransportMappingListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.TransportMappingListPager.sortIndex,
            sortDirection: $scope.TransportMappingListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.ResetSearchFilter = function () {
        //Reset 
        $scope.SearchTransportAssignmentModel = $.parseJSON($("#hdnSetTransportAssignmentModel").val()).SearchTransportAssignmentModel
        $scope.GetTransportMappingList();
    };
    $scope.SearchVehicle = function () {        //Reset

        $scope.GetTransportMappingList();

        

    };
    $scope.GetTransportAssignmentForEdit = function (item) {
        //$scope.TransportAssignmentModel = '';
        //
        //$scope.TransportAssignmentModel = {};
        $scope.TransportAssignmentModel.TransportID = item.TransportID;
        $scope.TransportAssignmentModel.FacilityID = '' + item.FacilityID;
        $scope.TransportAssignmentModel.VehicleID = '' + item.VehicleID;
        $scope.TransportAssignmentModel.OrganizationID = '' + item.OrganizationID;
        $scope.TransportAssignmentModel.RouteCode = '' + item.RouteCode;
        $scope.TransportAssignmentModel.StartDate = item.StartDate;
        $scope.TransportAssignmentModel.EndDate = item.EndDate;
        $scope.TransportAssignmentModel.Attendent = item.Attendent;
        //
        $scope.EditTransportID = item.TransportID;
    };
    $scope.GetTransportMappingList = function () {
        var jsonData = $scope.SetPostData($scope.TransportMappingListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetTransportAssignmentListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.SelectedVehicleIds = [];
                $scope.SelectAllCheckbox = false;
                $scope.TransportMappingList = response.Data.Items;
                $scope.TransportMappingListPager.currentPageSize = response.Data.Items.length;
                $scope.TransportMappingListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };
    $scope.OpenModal = function (TransportItem) {
        if (moment(TransportItem.EndDate).isAfter(moment())) {
            $('#transportassignpatientpopovermodal').modal({ backdrop: false, keyboard: false });
            $("#transportassignpatientpopovermodal").modal('show');
            scopeTransportAssignPatient.Load(TransportItem);
        }
        else {
            ShowMessages({ IsSuccess: true, ErrorCode: window.ErrorCode_Warning, Message: 'End date is passed' });
        }
    }
    $scope.TransportMappingListPager.getDataCallback = $scope.GetTransportMappingList;
    $scope.GetTransportMappingList();
};
controllers.TransportMappingController.$inject = ['$scope', '$http', '$window'];

$(document).ready(function () {


});