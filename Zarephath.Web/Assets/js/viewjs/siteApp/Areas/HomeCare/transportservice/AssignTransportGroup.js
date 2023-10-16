var vm;

controllers.AssignTransportGroupController = function ($scope, $http, $window) {
    vm = $scope;
    $scope.SetTransportAssignmentGroupModel = $.parseJSON($("#hdnSetTransportAssignmentGroupModel").val());
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetTransportAssignmentGroupModel").val());
    };
    $scope.PatientList = [];
    $scope.TransportGroup = [];
    $scope.TransportGroupAssignPatient = [];
    $scope.SearchTransportAssignmentGroupModel = $scope.SetTransportAssignmentGroupModel.SearchTransportAssignmentGroupModel;
    $scope.TempSearchTransportAssignmentGroupModel = $scope.SetTransportAssignmentGroupModel.SearchTransportAssignmentGroupModel;
    $scope.SelectedPatientIds = [];
    $scope.SelectedTransportGroup = {};
    $scope.FormTransportGroupID = 0;
    $scope.FormTitle = '';
    $scope.SelectAllCheckbox = false;
    $scope.SearchTransportGroupDetailAjaxStart = false;
    $scope.TransportGroupModel = $scope.SetTransportAssignmentGroupModel.TransportGroupModel;
    $scope.TransportGroupAssignPatientModel = $scope.SetTransportAssignmentGroupModel.TransportGroupAssignPatientModel;
    //To be added in .net code after new tables ready
    //$scope.AssignTransportationGroupModel = $scope.SetTransportAssignmentGroupModel.AssignTransportationGroup;
    $scope.SearchTransportAssignmentGroupListPager = new PagerModule("ClientName", undefined, "ASC");
    $scope.PatientListPager = new PagerModule("ClientName", undefined, "ASC");
    $scope.PatientListPager.pageSize = 20;
    $scope.selectedValue = "Select";
    $scope.ResetSearchFilter = function () {
        $scope.SearchTransportAssignmentGroupModel.FacilityID = '';
        $scope.SearchTransportAssignmentGroupModel.TripDirectionId = '';
        $scope.SearchTransportAssignmentGroupModel.StartDate = '';
        $scope.SearchTransportAssignmentGroupModel.EndDate = '';
        $scope.SearchTransportAssignmentGroupModel.ClientName = '';
        $scope.SearchTransportAssignmentGroupModel.Address = '';
        $scope.SearchTransportAssignmentGroupModel.RegionID = 0;
        $scope.SearchTransportAssignmentGroupModel.TransportGroupID = 0;
        SearchPatients();
    }
    $scope.SearchPatients = function () {
        //debugger;
        //
        var jsonData = $scope.SetPostData($scope.PatientListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.SearchTransportAssignmentGroupListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.SelectedPatientIds = [];
                $scope.SelectAllCheckbox = false;
                $scope.PatientList = response.Data.Items;
                $scope.PatientListPager.currentPageSize = response.Data.Items.length;
                $scope.PatientListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
        var jsonData = $scope.SetPostData($scope.PatientListPager.currentPage);

    }
    $scope.SearchTransportGroup = function () {
        //
        AngularAjaxCall($http, HomeCareSiteUrl.GetTransportGroupURL,
            angular.toJson({
                FacilityID: $scope.SearchTransportAssignmentGroupModel.FacilityID,
                StartDate: $scope.SearchTransportAssignmentGroupModel.StartDate,
                EndDate: $scope.SearchTransportAssignmentGroupModel.EndDate
            }),
            "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.TransportGroup = response.Data;
                    if ($scope.TransportGroup.length > 0) {
                        $scope.SearchTransportAssignmentGroupModel.TransportGroupID = $scope.TransportGroup[0].TransportGroupID;
                        setTimeout(function () {

                            $('[name="SearchTransportAssignmentGroupModel.TransportGroupID"]').val(vm.TransportGroup[0].TransportGroupID);
                        }, 1000);
                        //$('[name="SearchTransportAssignmentGroupModel.TransportGroupID"]').val($scope.TransportGroup[0].TransportGroupID)
                    }
                    $scope.SearchTransportGroupDetail();
                }
            });
        //
    }
    $scope.SearchTransportGroupDetail = function (tgitem) {
        //debugger;
        //
        $scope.TransportGroup.forEach(function (item, index) {
            if (item.TransportGroupID == $scope.SearchTransportAssignmentGroupModel.TransportGroupID) {
                $scope.SelectedTransportGroup = item;
            }
        });

        $scope.SearchTransportGroupDetailAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetTransportGroupDetailURL, angular.toJson({ TransportGroupID: $scope.SearchTransportAssignmentGroupModel.TransportGroupID }), "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.TransportGroupAssignPatient = response.Data;
                $scope.SearchTransportGroupDetailAjaxStart = false;
            }
        });
        //GetTransportGroupDetailURL
    }
    $scope.SearchGroup = function () {

    }
    $scope.SaveGroup = function () {

    }
    $scope.CreateGroup = function () {

    }
    $scope.SetPostData = function (fromIndex) {

        var pagermodel = {
            searchTransportAssignmentGroupModel: $scope.SearchTransportAssignmentGroupModel,
            pageSize: $scope.PatientListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.PatientListPager.sortIndex,
            sortDirection: $scope.PatientListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };
    $scope.SetPostDataTransportGroupDelete = function (referral, fromIndex) {

        var pagermodel = {
            transportGroupAssignPatientModel: referral,
            pageSize: $scope.PatientListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.PatientListPager.sortIndex,
            sortDirection: $scope.PatientListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };
    $scope.SetPostDataTransportGroup = function (fromIndex) {

        var pagermodel = {
            transportGroupModel: $scope.TransportGroupModel,
            pageSize: $scope.PatientListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.PatientListPager.sortIndex,
            sortDirection: $scope.PatientListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };
    //
    $scope.GroupFormModel = function (TransportGroupId, title) {
        $scope.TransportGroupModel = $scope.SetTransportAssignmentGroupModel.TransportGroupModel;
        FormTransportGroupID = TransportGroupId;
        $scope.TransportGroupModel.TransportGroupID = 0;
        $scope.TransportGroupModel.Name = '';
        $scope.TransportGroupModel.FacilityID = '';
        $scope.TransportGroupModel.TripDirection = '';
        $scope.TransportGroupModel.VehicleID = '';
        $scope.TransportGroupModel.RouteDesc = '';
        $scope.TransportGroupModel.StartDate = '';
        $scope.TransportGroupModel.EndDate = '';
        $scope.FormTitle = title;
        if (TransportGroupId > 0) {
            $scope.TransportGroup.forEach(function (item, index) {
                if (item.TransportGroupID == $scope.SearchTransportAssignmentGroupModel.TransportGroupID) {
                    $scope.TransportGroupModel.TransportGroupID = item.TransportGroupID;
                    $scope.TransportGroupModel.Name = item.Name;
                    $scope.TransportGroupModel.FacilityID = '' + item.FacilityID;
                    $scope.TransportGroupModel.TripDirection = '' + item.TripDirection;
                    $scope.TransportGroupModel.VehicleID = '' + item.VehicleID;
                    $scope.TransportGroupModel.RouteDesc = item.RouteDesc;
                    $scope.TransportGroupModel.StartDate = item.StartDate;
                    $scope.TransportGroupModel.EndDate = item.EndDate;
                }
            });
        }
        $('#fixedAside').modal({ backdrop: 'static', keyboard: false });
        //$('#fixedAsidelDDLBindIFrame').attr('src', HomeCareSiteUrl.PartialAddEmployeeURL + EncryptedEmployeeID);
    }
    $scope.GroupFormModelClosed = function () {
        //$scope.Refresh();
        $('#fixedAside').modal('hide');
    }
    $scope.SelectReferral = function (referral) {
        if (referral.IsChecked)
            $scope.SelectedPatientIds.push(referral.ReferralID);
        else
            $scope.SelectedPatientIds.remove(referral.ReferralID);

        if ($scope.SelectedPatientIds.length == $scope.PatientListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function (value) {
        $scope.SelectedPatientIds = [];
        angular.forEach($scope.PatientList, function (item, key) {
            item.IsChecked = value;// event.target.checked;
            if (item.IsChecked)
                $scope.SelectedPatientIds.push(item.ReferralID);
        });

        return true;
    };
    $scope.AssignReferral = function () {
        var dEndDate = moment(vm.SelectedTransportGroup.EndDate);
        var dToday = moment(new Date());
        if (dToday <= dEndDate) {
            if ($scope.SelectedPatientIds.length > 0 && $scope.SearchTransportAssignmentGroupModel.TransportGroupID > 0) {
                $scope.SearchTransportAssignmentGroupModel.ListOfIdsInCsv = ($scope.SelectedPatientIds).join();
                var jsonData = $scope.SetPostData($scope.PatientListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.SaveTransportGroupAssignPatientURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {

                        $scope.SearchPatients();
                        $scope.SearchTransportGroupDetail();
                    }
                    ShowMessages(response);
                });
                var jsonData = $scope.SetPostData($scope.PatientListPager.currentPage);
            }
        }
        else {
            ShowMessages({ IsSuccess: false, ErrorCode: window.ErrorCode_Warning, Message: "End Date is passed" });
        }
    }
    $scope.UnAssignReferral = function (referral) {
        if ($scope.SearchTransportAssignmentGroupModel.TransportGroupID > 0) {
            //$scope.SearchTransportAssignmentGroupModel.ReferralID = referral.ReferralID;
            referral.TransportGroupID = $scope.SearchTransportAssignmentGroupModel.TransportGroupID;
            referral.IsDeleted = true;
            var jsonData = $scope.SetPostDataTransportGroupDelete(referral, 0);
            AngularAjaxCall($http, HomeCareSiteUrl.DeleteTransportGroupAssignPatientNoteURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.SearchPatients();
                    $scope.SearchTransportGroupDetail();
                }
                ShowMessages(response);
            });
            var jsonData = $scope.SetPostData($scope.PatientListPager.currentPage);
        }
    }
    $scope.AssignReferralNote = function () {
        if ($scope.SearchTransportAssignmentGroupModel.TransportGroupID > 0) {
            //$scope.SearchTransportAssignmentGroupModel.ReferralID = referral.ReferralID;
            $scope.TransportGroupAssignPatientModel.TransportGroupID = $scope.SearchTransportAssignmentGroupModel.TransportGroupID;
            //referral.IsDeleted = true;
            var jsonData = $scope.SetPostDataTransportGroupDelete($scope.TransportGroupAssignPatientModel, 0);
            AngularAjaxCall($http, HomeCareSiteUrl.SaveTransportGroupAssignPatientNoteURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.SearchPatients();
                    $scope.SearchTransportGroupDetail();
                    $scope.CloseAssignReferralNoteModal();
                }
                ShowMessages(response);
            });
            var jsonData = $scope.SetPostData($scope.PatientListPager.currentPage);
        }
    }
    $scope.CancelSaveTransportGroup = function () {
        $scope.TransportGroupModel.Name = '';
        $scope.TransportGroupModel.FacilityID = 0;
        $scope.TransportGroupModel.TripDirection = 0;
        $scope.TransportGroupModel.VehicleID = 0;
        $scope.TransportGroupModel.StartDate = '';
        $scope.TransportGroupModel.EndDate = '';
        $scope.TransportGroupModel.RouteDesc = '';
    }
    $scope.SaveTransportGroup = function () {
        var jsonData = $scope.SetPostDataTransportGroup(0);
        $scope.SearchTransportAssignmentGroupModel.FacilityID = $scope.TransportGroupModel.FacilityID;
        AngularAjaxCall($http, HomeCareSiteUrl.SaveTransportGroupURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.GroupFormModelClosed();
                $scope.SearchPatients();
                $scope.SearchTransportGroup()
                //$scope.SearchTransportGroupDetail();

            }
            ShowMessages(response);
        });
        var jsonData = $scope.SetPostData($scope.PatientListPager.currentPage);
    }
    $scope.AssignValue = function (VehicleID, VINNumber) {
        $scope.TransportGroupModel.VehicleID = VehicleID;
        $scope.selectedValue = VINNumber;
    }
    $scope.CheckAndAssign = function (referral) {
        referral.IsChecked = true;
        $scope.SelectReferral(referral);
        $scope.AssignReferral();
    }
    $scope.OpenAssignReferralModal = function (referral) {
        $scope.TransportGroupAssignPatientModel = referral;
        $('#AssignReferralNoteModal').modal({ backdrop: 'static', keyboard: false });
    }
    $scope.CloseAssignReferralNoteModal = function () {
        $('#AssignReferralNoteModal').modal('hide');
    }
    $scope.TransportGroupAssignPatientGetFacility = function (FacilityID) {
        //vm.SetTransportAssignmentGroupModel.FacilityListModel
        //item.FacilityID  item.FacilityName
        var FacilityName = '';
        $scope.SetTransportAssignmentGroupModel.FacilityListModel.forEach(function (item, index) {

            if (item.FacilityID == FacilityID) {
                FacilityName = item.FacilityName;
            }
        });
        return FacilityName;
    }
    $scope.TransportGroupAssignPatientGetRegion = function (RegionID) {
        var RegionName = '';
        $scope.SetTransportAssignmentGroupModel.RegionModel.forEach(function (item, index) {

            if (item.RegionID == RegionID) {
                RegionName = item.RegionName;
            }
        });
        return RegionName;
    }
    $scope.TransportGroupAssignPatientGetTripDirection = function (TripDirectionId) {
        var TripDirectionName = '';
        $scope.SetTransportAssignmentGroupModel.TripDirectionList.forEach(function (item, index) {

            if (item.TripDirectionId == TripDirectionId) {
                TripDirectionName = item.TripDirectionName;
            }
        });
        return TripDirectionName;
    }
    $scope.TransportGroupAssignPatientGetVehicleName = function (VehicleID) {
        var VehicleName = '';
        $scope.SetTransportAssignmentGroupModel.VehicleNameListModel.forEach(function (item, index) {

            if (item.VehicleID == VehicleID) {
                VehicleName = item.VehicleName;
            }
        });
        return VehicleName;
    }
    $scope.PatientListPager.getDataCallback = $scope.SearchPatients;
};
controllers.AssignTransportGroupController.$inject = ['$scope', '$http', '$window'];
$(document).ready(function () {


});
