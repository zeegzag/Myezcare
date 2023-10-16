var vm;
controllers.PendingScheduleController = function ($scope, $http, $timeout) {
    vm = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetPendingScheduleListPage").val());
    };
    $scope.PendingScheduleList = [];
    $scope.SelectedPendingScheduleIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.PendingScheduleModel = $.parseJSON($("#hdnSetPendingScheduleListPage").val());
    $scope.SearchPendingScheduleListPage = $scope.PendingScheduleModel.SearchPendingScheduleListPage;
    $scope.TempSearchPendingScheduleListPage = $scope.PendingScheduleModel.SearchPendingScheduleListPage;
    $scope.PendingScheduleListPager = new PagerModule("PendingScheduleID","", "DESC");
    
    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchPendingSchedules: $scope.SearchPendingScheduleListPage,
            pageSize: $scope.PendingScheduleListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.PendingScheduleListPager.sortIndex,
            sortDirection: $scope.PendingScheduleListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchPendingScheduleListPage = $.parseJSON(angular.toJson($scope.TempSearchPendingScheduleListPage));
      
    };

    $scope.GetPendingScheduleList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedPendingScheduleIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchPendingScheduleListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping
        
        var jsonData = $scope.SetPostData($scope.PendingScheduleListPager.currentPage);

        AngularAjaxCall($http, HomeCareSiteUrl.GetPendingScheduleListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.PendingScheduleList = response.Data.Items;
                $scope.PendingScheduleListPager.currentPageSize = response.Data.Items.length;
                $scope.PendingScheduleListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        //$scope.ResetSearchFilter();
        //$scope.CaseManagerListPager.currentPage = $scope.CaseManagerListPager.currentPage;
        $scope.PendingScheduleListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');
            $scope.SearchPendingScheduleListPage = $scope.newInstance().SearchPendingScheduleListPage;
            $scope.TempSearchPendingScheduleListPage = $scope.newInstance().SearchPendingScheduleListPage;
            $scope.TempSearchPendingScheduleListPage.IsDeleted = "0";
            $scope.TempSearchPendingScheduleListPage.EmployeeID = "";
            $scope.TempSearchPendingScheduleListPage.CreatedBy = "";
            $scope.PendingScheduleListPager.currentPage = 1;
            $scope.PendingScheduleListPager.getDataCallback();
        });
    };
    $scope.SearchPendingSchedule = function () {
        $scope.PendingScheduleListPager.currentPage = 1;
        $scope.PendingScheduleListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectPendingSchedule = function (PendingSchedule) {
        if (PendingSchedule.IsChecked)
            $scope.SelectedPendingScheduleIds.push(PendingSchedule.PendingScheduleID);
        else
            $scope.SelectedPendingScheduleIds.remove(PendingSchedule.PendingScheduleID);

        if ($scope.SelectedPendingScheduleIds.length == $scope.PendingScheduleListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedPendingScheduleIds = [];

        angular.forEach($scope.PendingScheduleList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedPendingScheduleIds.push(item.PendingScheduleID);
        });
        return true;
    };

    $scope.DeletePendingSchedule = function (PendingScheduleId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchPendingScheduleListPage.ListOfIdsInCsv = PendingScheduleId > 0 ? PendingScheduleId.toString() : $scope.SelectedPendingScheduleIds.toString();

                if (PendingScheduleId > 0) {
                    if ($scope.PendingScheduleListPager.currentPage != 1)
                        $scope.PendingScheduleListPager.currentPage = $scope.PendingScheduleList.length === 1 ? $scope.PendingScheduleListPager.currentPage - 1 : $scope.PendingScheduleListPager.currentPage;
                } else {

                    if ($scope.PendingScheduleListPager.currentPage != 1 && $scope.SelectedPendingScheduleIds.length == $scope.PendingScheduleListPager.currentPageSize)
                        $scope.PendingScheduleListPager.currentPage = $scope.PendingScheduleListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedPendingScheduleIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.PendingScheduleListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeletePendingScheduleURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.PendingScheduleList = response.Data.Items;
                        $scope.PendingScheduleListPager.currentPageSize = response.Data.Items.length;
                        $scope.PendingScheduleListPager.totalRecords = response.Data.TotalItems;
                    }
                });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.DatePickerDate = function (modelDate, newDate) {
        var a;
        if (modelDate) {
            var dt = new Date(modelDate);
            dt >= newDate ? a = newDate : a = dt;
        }
        else {
            a = newDate;
        }
        return moment(a).format('L');
    };

    $scope.PendingScheduleListPager.getDataCallback = $scope.GetPendingScheduleList;
    $scope.PendingScheduleListPager.getDataCallback();





    //#region Process Schedule

    $scope.OpenProcessSchedule = function (item) {

        //HideErrors("#modalRemoveScheduleForm");
        $scope.ProcessPendingScheduleModel = item;
        $('#processPendingScheduleModal').modal({ backdrop: false, keyboard: false });
        $("#processPendingScheduleModal").modal('show');

    };

    $scope.CheckForProcessSchedule = function (item) {

    }

    $scope.ProcessPendingSchedule = function () {
        var jsonData = angular.toJson($scope.ProcessPendingScheduleModel);
        AngularAjaxCall($http, HomeCareSiteUrl.ProcessPendingScheduleURL, jsonData, "Post", "json", "application/json").success(function (response) {
            //ShowMessages(response);
            if (response.IsSuccess) {
                $scope.GetPendingScheduleList();
                $scope.CloseModal();
            }
            ShowMessages(response);
            
        });
    }

    $scope.CloseModal = function () {
        $("#processPendingScheduleModal").modal('hide');
    }

    //#endregion

    
};

controllers.PendingScheduleController.$inject = ['$scope', '$http', '$timeout'];
