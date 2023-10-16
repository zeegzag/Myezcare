var custModel1;

controllers.ReferralCareTypeTimeSlotsController = function ($scope, $http) {
    custModel1 = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnCareTypeRTSModel").val());
    };
    $scope.RCTSModel = $scope.newInstance();
    $scope.AddTimeSlots = $scope.newInstance().AddTimeSlots;
    $scope.IsEndDateAvailable = false;
    //$scope.RTSMaster =$scope.newInstance().RTSMaster;
    //$scope.RTSDetail = $scope.newInstance().RTSDetail;
    //$scope.CareTypeList = $scope.newInstance().CareTypeList;

    $scope.resetEndDate = function () {
        $scope.AddTimeSlots.EndDate = null;
    };

    $scope.SetFrequencyName = function () {
        
        if ($scope.AddTimeSlots.Frequency == 1) {
            $scope.FrequencyName = "Time";
        } else if ($scope.AddTimeSlots.Frequency == 7) {
            $scope.FrequencyName = "Week";
        } else if ($scope.AddTimeSlots.Frequency == 30) {
            $scope.FrequencyName = "Month";
        } else {
            $scope.FrequencyName = "Year";
        }
    };

    $scope.SaveCareTypeSlot = function () {
        if (CheckErrors("#frmCareTypeSlot")) {
            $scope.AddTimeSlots.ReferralID = $scope.TempSearchCTSchedule.ReferralID;
            var jsonData = angular.toJson($scope.AddTimeSlots);
            AngularAjaxCall($http, HomeCareSiteUrl.AddCareTypeSlotURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.AddTimeSlots = $scope.newInstance().AddTimeSlots;
                    $scope.CTScheduleListPager.getDataCallback();
                }
                ShowMessages(response);
            });
        }
    };

    $scope.EditCareTypeSchedule = function (data) {
        
        $scope.IsEndDateAvailable = (data.EndDate != null) ? true : false;
        $scope.AddTimeSlots = angular.copy(data);
        $scope.AddTimeSlots.Count = data.NumOfTime;
        $scope.SetFrequencyName();
    };

    $scope.Cancel = function () {
        HideErrors($("#frmCareTypeSlot"));
        $scope.AddTimeSlots = $scope.newInstance().AddTimeSlots;
    };



    //#region RTS Master Paging 
    $scope.CTScheduleList = [];
    $scope.CTScheduleListPager = new PagerModule("CareTypeTimeSlotID");
    $scope.SearchCTSchedule = $scope.RCTSModel.SearchCTSchedule;
    $scope.TempSearchCTSchedule = $scope.RCTSModel.SearchCTSchedule;

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchCTSchedule: $scope.SearchCTSchedule,
            pageSize: $scope.CTScheduleListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.CTScheduleListPager.sortIndex,
            sortDirection: $scope.CTScheduleListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };
    $scope.SearchModelMapping = function () {
        $scope.SearchCTSchedule = $.parseJSON(angular.toJson($scope.TempSearchCTSchedule));
    };


    $scope.GetCTScheduleList = function (isSearchDataMappingRequire) {
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        var jsonData = $scope.SetPostData($scope.CTScheduleListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetCTScheduleListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.CTScheduleList = response.Data.Items;
                $scope.CTScheduleListPager.currentPageSize = response.Data.Items.length;
                $scope.CTScheduleListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.CTScheduleListPager.getDataCallback = $scope.GetCTScheduleList;

    $scope.Refresh = function () {
        $scope.CTScheduleListPager.getDataCallback();
    };

    $scope.SearchCTScheduleList = function () {
        $scope.CTScheduleListPager.currentPage = 1;
        $scope.CTScheduleListPager.getDataCallback(true);
    };
    
    $scope.RefreshRTSMaster = function () {
        $scope.CTScheduleListPager.getDataCallback();
    };

    if ($scope.RCTSModel.IsPartial) {
        $scope.SearchCTScheduleList();
    }
    //#endregion
    $scope.nextOption = function () {
        var length = $scope.RCTSModel.ReferralList.length;
        var index = $scope.RCTSModel.ReferralList.findIndex(a => a.Value.toString() == $scope.TempSearchCTSchedule.ReferralID);
        if (index + 1 < length) {
            var next = $scope.RCTSModel.ReferralList[index + 1].Value;
            $scope.TempSearchCTSchedule.ReferralID = next.toString();
            $scope.SearchCTScheduleList();
        }

    };

    $scope.prevOption = function () {
        var length = $scope.RCTSModel.ReferralList.length;
        var index = $scope.RCTSModel.ReferralList.findIndex(a => a.Value.toString() == $scope.TempSearchCTSchedule.ReferralID);
        if (index != -1) {
            var next = $scope.RCTSModel.ReferralList[index - 1].Value;
            $scope.TempSearchCTSchedule.ReferralID = next.toString();
            $scope.SearchCTScheduleList();
        }
    };


};

controllers.ReferralCareTypeTimeSlotsController.$inject = ['$scope', '$http'];

