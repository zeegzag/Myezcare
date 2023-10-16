var vm;

controllers.TransportationAssignmentController = function ($scope, $http, $timeout) {
    vm = $scope;

    $scope.NewInstance = function () {
        var newReference = $.parseJSON($("#hdnTransportationAssignmentModel").val());
        newReference.SearchRefrralForTransportatiAssignment.StartDate = moment(new Date()).format('YYYY/MM/DD');
        return newReference;
    };

    $scope.SetDatePickerDate = function (modelDate, newDate) {
        var a;
        if (modelDate) {
            var dt = new Date(modelDate);
            dt > newDate ? a = newDate : a = dt;
        } else {
            a = newDate;
        }
        return moment(a).format('L');
    };

    $scope.Today = new Date();

    $scope.AssignClient = function (transportationGroup) {
        $scope.TempSearchRefrralForTransportatiAssignment.FacilityID = transportationGroup.TransportationGroup.FacilityID.toString();
        $scope.TempSearchRefrralForTransportatiAssignment.TransportLocationID = transportationGroup.TransportationGroup.LocationID.toString();
        $scope.TempSearchRefrralForTransportatiAssignment.TripDirection = transportationGroup.TransportationGroup.TripDirection;
        $scope.SearchTransportationGroup();
    };

    //#region Referral List 
    $scope.ReferralList = [];

    $scope.SelectedScheduleIDs = [];

    $scope.TransportaionAssignmentModel = $scope.NewInstance();

    $scope.SearchRefrralForTransportatiAssignment = $scope.NewInstance().SearchRefrralForTransportatiAssignment;

    $scope.TempSearchRefrralForTransportatiAssignment = $scope.NewInstance().SearchRefrralForTransportatiAssignment;

    $scope.ReferralListPager = new PagerModule("Name");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchRefrrelForTransportatioGroupList: $scope.SearchRefrralForTransportatiAssignment,
            pageSize: $scope.ReferralListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ReferralListPager.sortIndex,
            sortDirection: $scope.ReferralListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchRefrralForTransportatiAssignment = $.parseJSON(angular.toJson($scope.TempSearchRefrralForTransportatiAssignment));
    };

    $scope.GetTransportatioGroupList = function (isSearchDataMappingRequire) {
        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.ReferralListPager.currentPage);
        $scope.IsClientLoading = true;
        AngularAjaxCall($http, SiteUrl.GetReferralListForTransportationAssignmentURL, jsonData, "Post", "json", "application/json",false).success(function (response) {
            $scope.IsClientLoading = false;
            if (response.IsSuccess) {
                $scope.ReferralList = response.Data.Items;
                $scope.ReferralListPager.currentPageSize = response.Data.Items.length;
                $scope.ReferralListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.SearchTransportationGroup = function () {
        if (!CheckErrors("#frmStartDAte")) {
            return;
        }
        $scope.ReferralListPager.currentPage = 1;
        $scope.ReferralListPager.getDataCallback(true);
    };

    $scope.SelectReferral = function (referal) {
        if (referal.IsChecked)
            $scope.SelectedScheduleIDs.push(referal.ScheduleID);
        else
            $scope.SelectedScheduleIDs.remove(referal.ScheduleID);
    };

    $scope.DeselectAllReferralAndSetFlag = function (unSuccessScheduleIDs) {
        $.each($scope.ReferralList, function (i, item) {
            var scheduleID = null;

            if (unSuccessScheduleIDs) {
                scheduleID=$.grep(unSuccessScheduleIDs, function (id) {
                    return id == item.ScheduleID;
                });
            }
            //
            if (item.IsChecked && (scheduleID==null || scheduleID.length == 0)) {
                if ($scope.SearchRefrralForTransportatiAssignment.TripDirection == window.TripDirectionUp) {
                    item.IsAssignedToTransportationGroupUp = true;
                }
                if ($scope.SearchRefrralForTransportatiAssignment.TripDirection == window.TripDirectionDown) {
                    item.IsAssignedToTransportationGroupDown = true;
                }
            }
            item.IsChecked = false;
        });
        $scope.SelectedScheduleIDs = [];
       // $scope.$apply();
    };

    $scope.ReferralListPager.getDataCallback = $scope.GetTransportatioGroupList;

    //$scope.ReferralListPager.getDataCallback();

    //#endregion Referral List 

    //#region Add/Edit Transportation Group

    $scope.AddTransportationGroupModel = $scope.NewInstance().AddTransportationGroupModel;

    $scope.AddTransportationGroup = function () {
        $scope.DisableTripDropdown = false;
        $scope.AddTransportationGroupModel = $scope.NewInstance().AddTransportationGroupModel;
        $scope.AddTransportationGroupModel.TransportationGroup.TransportationDate = moment($scope.SearchRefrralForTransportatiAssignment.StartDate).format('YYYY/MM/DD');
        $scope.AddTransportationGroupModel.TransportationGroup.FacilityID = null;
        $scope.AddTransportationGroupModel.TransportationGroup.LocationID = null;
    };

    $scope.DisableTripDropdown = false;

    $scope.EditTransportationGroup = function (transportationGroupModel, clintListLength) {
        $scope.DisableTripDropdown = clintListLength > 0 ? true : false;

        $scope.AddTransportationGroupModel = null;
        //$scope.$apply();
        var newReference = $.parseJSON(angular.toJson({
            TransportationGroup: transportationGroupModel,
            SelectedStaffs: transportationGroupModel.StaffIDs ? transportationGroupModel.StaffIDs.split(",") : []
        }));
        newReference.TransportationGroup.FacilityID = newReference.TransportationGroup.FacilityID.toString();
        newReference.TransportationGroup.LocationID = newReference.TransportationGroup.LocationID.toString();
        $scope.AddTransportationGroupModel = newReference;
        $('#confirmModal').modal('show');
    };

    $scope.SaveTransportationGroup = function () {
        if (CheckErrors("#frmAddTranspotationGroup")) {
            var jsonData = angular.toJson({
                transportationGroup: $scope.AddTransportationGroupModel,
                searchTransportatioGroupList: { Date: $scope.TempSearchRefrralForTransportatiAssignment.StartDate }
            });

            AngularAjaxCall($http, SiteUrl.SaveTransportationGroupURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $('#confirmModal').modal('hide');
                    $scope.TransportationGroupUpList = response.Data.UpList;
                    $scope.TransportationGroupDownList = response.Data.DownList;
                }
                ShowMessages(response);
            });
        }
    };

    $scope.RemoveTransportationGroup = function (transportationGroupID) {
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = angular.toJson({
                    transportationGroupID: transportationGroupID,
                    searchTransportatioGroupList: { Date: $scope.TempSearchRefrralForTransportatiAssignment.StartDate }
                });

                AngularAjaxCall($http, SiteUrl.RemoveTransportationGroupURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.TransportationGroupUpList = response.Data.UpList;
                        $scope.TransportationGroupDownList = response.Data.DownList;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteConfirmationMessageForTransportationGroup, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.OnCloseModel = function () {
        HideErrors("#frmAddTranspotationGroup");
    };

    //#endregion

    //#region AssignedClientList

    $scope.TransportationGroupUpList = [];

    $scope.TransportationGroupDownList = [];

    $scope.GetAssignedClientListForTransportationAssignment = function () {
        var jsonData = angular.toJson({
            searchModel: {
                Date: $scope.TempSearchRefrralForTransportatiAssignment.StartDate
            }
        });
        $scope.IsTransportationAssignmentLoading = true;
        AngularAjaxCall($http, SiteUrl.GetAssignedClientListForTransportationAssignmentURL, jsonData, "Post", "json", "application/json",false).success(function (response) {
            $scope.IsTransportationAssignmentLoading = false;
            if (response.IsSuccess) {
                $scope.TransportationGroupUpList = response.Data.UpList;
                $scope.TransportationGroupDownList = response.Data.DownList;
            }
            ShowMessages(response);
        });
    };

    $scope.OnClienrDropperDrop = function (transportationGroup, referralSchedule) {
        //
        //Add TransportationGroupClient when user drop referral dropper.
        referralSchedule.IsChecked = true;
        var jsonData = angular.toJson({
            transportationGroupClient: {
                ScheduleID: referralSchedule.ScheduleID,
                ScheduleIDs: $scope.SelectedScheduleIDs,
                TransportationGroupID: transportationGroup.TransportationGroup.TransportationGroupID
            },
            searchTransportatioGroupList: { Date: $scope.TempSearchRefrralForTransportatiAssignment.StartDate }
        });
        $scope.IsTransportationAssignmentLoading = true;

        AngularAjaxCall($http, SiteUrl.SaveTransportationGroupClientURL, jsonData, "Post", "json", "application/json",false).success(function (response) {
            $scope.IsTransportationAssignmentLoading = false;
            if (response.IsSuccess) {
                $scope.TransportationGroupUpList = response.Data.UpList;
                $scope.TransportationGroupDownList = response.Data.DownList;
                $scope.DeselectAllReferralAndSetFlag();
            }
            else if (response.ErrorCode == window.ErrorCode_Warning) {
                $scope.GetAssignedClientListForTransportationAssignment();
                $scope.DeselectAllReferralAndSetFlag(response.Data);
            }
            ShowMessages(response);
        });

       
    };

    $scope.RemoveTransportationGroupClient = function (transportationGroupClient, transportionGroup) {
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = angular.toJson({
                    transportationGroupClientID: transportationGroupClient.TransportationGroupClientID,
                });
                $scope.IsTransportationAssignmentLoading = true;

                AngularAjaxCall($http, SiteUrl.RemoveTransportationGroupClientURL, jsonData, "Post", "json", "application/json",false).success(function (response) {
                    $scope.IsTransportationAssignmentLoading = false;

                    if (response.IsSuccess) {
                        //transportionGroup.ClientList.remove(transportationGroupClient);
                        $scope.GetTransportatioGroupList();
                        $scope.GetAssignedClientListForTransportationAssignment();
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteConfirmationMessageForTransportationGroupClient, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.SaveTransportationGroupFilter = function (transportionGroupClientId, transportationFilters, popoverFunctions) {
        var jsonData = angular.toJson({
            model: {
                TransportationGroupClientID: transportionGroupClientId,
                TransportationFilters: transportationFilters
            },
            searchTransportatioGroupList: { Date: $scope.TempSearchRefrralForTransportatiAssignment.StartDate }
        });
        $scope.IsTransportationAssignmentLoading = true;

        return AngularAjaxCall($http, SiteUrl.SaveTransportationGroupFilterURL, jsonData, "Post", "json", "application/json").success(function (response) {
            $scope.IsTransportationAssignmentLoading = false;
            if (response.IsSuccess) {
                $scope.TransportationGroupUpList = response.Data.UpList;
                $scope.TransportationGroupDownList = response.Data.DownList;
                popoverFunctions.Hide();
            }
            ShowMessages(response);
        });
    };

    $scope.Hidepopover = function (popoverFunction) {
        popoverFunction.Hide();
    };

    //#endregion

    //#region Other Function

    $scope.ExpandAllDiv = function () {
        $('.transactionGroupContainer a.expand').click();
    };

    $scope.CollapseAllDiv = function () {
        $('.transactionGroupContainer a.collapse').click();
    };

    $scope.$watch(function () { return $scope.TempSearchRefrralForTransportatiAssignment.StartDate; }, function () {
        $scope.SearchTransportationGroup();
        $scope.GetAssignedClientListForTransportationAssignment();
    });

    $scope.PrintDiv = function (id) {
        $('.transactionGroupContainer a.expand').click();
        setTimeout(function () {
            printDiv($("#" + id));
        }, 1000);
    };

    //#endregion

    $scope.IsPastDateSelected = function () {
        var today = new Date(moment($scope.Today).format("YYYY/MM/DD"));
        var selectedDate = new Date($scope.SearchRefrralForTransportatiAssignment.StartDate);
        return selectedDate >= today;
    };

    $scope.FilterDropdown = function (value) {
        return function (item) {
            if (item.IsDeleted == 0 || item.Value == value) {
                return item;
            }
        };
    };

    $scope.FilterStaffDropdown = function (value) {
        return function (item) {
            if (item.IsDeleted == 0 || (value != null && value.indexOf(item.Value.toString()) > -1)) {
                return item;
            }
        };
    };

    $scope.SetPopupModel = function (referral) {
        referral.TransportationFilters = $scope.TransportaionAssignmentModel.TransportationFilters;
        referral.FilterStaffDropdown = $scope.FilterStaffDropdown;
        referral.SaveTransportationGroupFilter = $scope.SaveTransportationGroupFilter;
        referral.Hidepopover = $scope.Hidepopover;
        return referral;
    };

};

controllers.TransportationAssignmentController.$inject = ['$scope', '$http', '$timeout'];