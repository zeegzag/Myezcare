var vm;

controllers.EmployeeDayOffListController = function ($scope, $http, $timeout) {
    vm = $scope;
    $scope.AddEmployeeDayOffURL = HomeCareSiteUrl.AddEmployeeDayOffURL;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetEmployeeDayOffListPage").val());
    };

    $scope.EmployeeDayOffList = [];
    $scope.SelectedEmployeeDayOffIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.EmployeeDayOffModel = $.parseJSON($("#hdnSetEmployeeDayOffListPage").val());
    $scope.SearchEmployeeDayOffModel = $scope.EmployeeDayOffModel.SearchEmployeeDayOffModel;
    $scope.TempSearchEmployeeDayOffModel = $scope.EmployeeDayOffModel.SearchEmployeeDayOffModel;
    $scope.EmployeeDayOffListPager = new PagerModule("EmployeeDayOffID");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchEmpDayOffModel: $scope.SearchEmployeeDayOffModel,
            pageSize: $scope.EmployeeDayOffListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.EmployeeDayOffListPager.sortIndex,
            sortDirection: $scope.EmployeeDayOffListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.LoggedInUserId = window.LoggedInID;

    $scope.SearchModelMapping = function () {

        $scope.SearchEmployeeDayOffModel = $.parseJSON(angular.toJson($scope.TempSearchEmployeeDayOffModel));
        //$scope.SearchEmployeeDayOffModel.Name = $scope.TempSearchDepartmentModel.Name;
        //$scope.SearchEmployeeDayOffModel.Email = $scope.TempSearchDepartmentModel.Email;
        //$scope.SearchEmployeeDayOffModel.DepartmentID = $scope.TempSearchDepartmentModel.DepartmentID;
        //$scope.SearchEmployeeDayOffModel.IsSupervisor = $scope.TempSearchDepartmentModel.IsSupervisor;
        //$scope.SearchEmployeeDayOffModel.ListOfIdsInCSV = $scope.TempSearchDepartmentModel.ListOfIdsInCSV;

    };

    $scope.GetEmployeeDayOffList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedEmployeeDayOffIds = [];
        $scope.SelectAllCheckbox = false;
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.EmployeeDayOffListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeDayOffListURL, jsonData, "Post", "json", "application/json").success(function (response) {

            if (response.IsSuccess) {
                $scope.EmployeeDayOffList = response.Data.Items;
                $scope.EmployeeDayOffListPager.currentPageSize = response.Data.Items.length;
                $scope.EmployeeDayOffListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.EmployeeDayOffListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            var EmployeeID = $scope.SearchEmployeeDayOffModel.EmployeeID;
            $scope.SearchEmployeeDayOffModel = $scope.newInstance().SearchEmployeeDayOffModel;
            $scope.TempSearchEmployeeDayOffModel = $scope.newInstance().SearchEmployeeDayOffModel;
            //$scope.SearchEmployeeDayOffModel.IsDeleted = 0;
            $scope.TempSearchEmployeeDayOffModel.IsDeleted = "0";
            if ($scope.EmployeeDayOffModel.IsPartial) {
                $scope.SearchEmployeeDayOffModel.EmployeeID = EmployeeID;
                $scope.TempSearchEmployeeDayOffModel.EmployeeID = EmployeeID;
            }
            else {
                $scope.TempSearchEmployeeDayOffModel.EmployeeID = null;
            }
            //$scope.SearchEmployeeDayOffModel.IsSupervisor = "-1";            
            //$scope.TempSearchEmployeeDayOffModel.IsSupervisor = "-1";


            $scope.EmployeeDayOffListPager.currentPage = 1;
            $scope.EmployeeDayOffListPager.getDataCallback();
        });
    };

    $scope.SearchEmployeeDayOff = function () {
        $scope.EmployeeDayOffListPager.currentPage = 1;
        $scope.EmployeeDayOffListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectEmployeeDayOff = function (item) {
        if (item.IsChecked)
            $scope.SelectedEmployeeDayOffIds.push(item.EmployeeDayOffID);
        else
            $scope.SelectedEmployeeDayOffIds.remove(item.EmployeeDayOffID);

        if ($scope.SelectedEmployeeDayOffIds.length == $scope.EmployeeDayOffListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedEmployeeDayOffIds = [];

        angular.forEach($scope.EmployeeDayOffList, function (item, key) {

            item.IsChecked = $scope.SelectAllCheckbox;// event.target.checked;
            if (item.IsChecked)
                $scope.SelectedEmployeeDayOffIds.push(item.EmployeeDayOffID);
        });

        return true;
    };

    $scope.DeleteEmployeeDayOff = function (EmployeeDayOffId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchEmployeeDayOffModel.ListOfIdsInCsv = EmployeeDayOffId > 0 ? EmployeeDayOffId.toString() : $scope.SelectedEmployeeDayOffIds.toString();

                if (EmployeeDayOffId > 0) {
                    if ($scope.EmployeeDayOffListPager.currentPage != 1)
                        $scope.EmployeeDayOffListPager.currentPage = $scope.EmployeeDayOffList.length === 1 ? $scope.EmployeeDayOffListPager.currentPage - 1 : $scope.EmployeeDayOffListPager.currentPage;
                } else {

                    if ($scope.EmployeeDayOffListPager.currentPage != 1 && $scope.SelectedEmployeeDayOffIds.length == $scope.EmployeeDayOffListPager.currentPageSize)
                        $scope.EmployeeDayOffListPager.currentPage = $scope.EmployeeDayOffListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedEmployeeDayOffIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.EmployeeDayOffListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteEmployeeDayOffURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.EmployeeDayOffList = response.Data.Items;
                        $scope.EmployeeDayOffListPager.currentPageSize = response.Data.Items.length;
                        $scope.EmployeeDayOffListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };
    $scope.EmployeeDayOffListPager.getDataCallback = $scope.GetEmployeeDayOffList;
    $scope.EmployeeDayOffListPager.getDataCallback();

    $scope.OpenEmpDayOffModal = function (item, isActionMode, viewOnly) {
        $scope.EmpScheduleList = [];
        HideErrors("#frmDayOff");
        if (item == undefined) {
            $scope.EmployeeDayOff = $scope.newInstance().EmployeeDayOff;
            $scope.EmployeeDayOff.StartTime = null;
            $scope.EmployeeDayOff.StrStartTime = null;
            $scope.EmployeeDayOff.EndTime = null;
            $scope.EmployeeDayOff.StrEndTime = null;
            $scope.EmployeeDayOff.EmployeeID = $scope.TempSearchEmployeeDayOffModel.EmployeeID;
        } else {
            $scope.EmployeeDayOff = angular.copy(item);
        }
        if (item != undefined) {
            $scope.EmployeeDayOff.StartTime = moment(item.StrStartTime);// moment( moment(item.StartTime).format('MM/DD/YYYY h:mm a'));
            $scope.EmployeeDayOff.EndTime = moment(item.StrEndTime);// moment( moment(item.EndTime).format('MM/DD/YYYY h:mm a'));
        }
        $scope.EmployeeDayOff.IsActionMode = isActionMode;
        $scope.EmployeeDayOff.ViewOnly = viewOnly;

        $("#empDayOffModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    }


    $scope.SaveEmployeeDayOff = function () {
        debugger
        if (CheckErrors($("#frmDayOff"))) {

            $scope.EmployeeDayOff.StrStartTime = moment($scope.EmployeeDayOff.StartTime).format('MM/DD/YYYY h:mm a');
            $scope.EmployeeDayOff.StrEndTime = moment($scope.EmployeeDayOff.EndTime).format('MM/DD/YYYY h:mm a');

            var jsonData = angular.toJson($scope.EmployeeDayOff);
            AngularAjaxCall($http, HomeCareSiteUrl.SaveEmployeeDayOffURL, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        $scope.EmployeeDayOffListPager.getDataCallback();
                        $("#empDayOffModal").modal('hide');
                    }
                    ShowMessages(response);
                });
        }
    };

    $scope.DayOffAction = function (status) {
        $scope.EmployeeDayOff.DayOffStatus = status;

        if (CheckErrors($("#frmDayOff"))) {

            if (status === "Approved") {
                $scope.CheckForEmpSchedules(true);
            } else {
                $scope.SaveDayOffAction();
            }
        }
    };


    $scope.EmpScheduleList = [];
    $scope.CheckForEmpSchedules = function (doSaveAction) {

        var jsonData = angular.toJson($scope.EmployeeDayOff);
        AngularAjaxCall($http, HomeCareSiteUrl.CheckForEmpSchedulesURL, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        $scope.EmpScheduleList = [];
                        $scope.EmpScheduleList = response.Data;

                        if (doSaveAction) {
                            if ($scope.EmpScheduleList == null || $scope.EmpScheduleList.length === 0) {
                                $scope.SaveDayOffAction();
                            }
                        }
                    }
                    ShowMessages(response);
                });
    };

    $scope.SaveDayOffAction = function () {
       
        if (CheckErrors($("#frmDayOff"))) {
            var jsonData = angular.toJson($scope.EmployeeDayOff);
            AngularAjaxCall($http, HomeCareSiteUrl.DayOffActionURL, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        $scope.EmployeeDayOffListPager.getDataCallback();
                        $("#empDayOffModal").modal('hide');
                    }
                    ShowMessages(response);
                });
        }
    };

    $scope.RemoveSchedule = function (item) {
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = { scheduleID: item.ScheduleID };
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteScheduleFromCalenderURL, jsonData, "Post", "json", "application/json").success(function (response) {

                    if (response.IsSuccess) {
                        $scope.CheckForEmpSchedules();
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteConfirmationMessageForSchedule, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);

    };

    $scope.OpenEmpRefSchModal = function (item) {
        
        $("#emprefschmodal").modal({
            backdrop: 'static',
            keyboard: false
        });
        var startDate = item.StrStartTime;

        var event = {};
        event.scheduleModel = {};
        event.scheduleModel.ScheduleID = item.ScheduleID;
        event.scheduleModel.StartDate = item.StrStartTime;
        event.scheduleModel.EndDate = item.StrEndTime;
        scopeEmpRefSch.CallOnPopUpLoad(item.ReferralID, startDate, event);
    }

    $('#emprefschmodal').on('hidden.bs.modal', function (e) {
        $scope.CheckForEmpSchedules(false);
    });


};
controllers.EmployeeDayOffListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowEmployeeDayOffMessage");
    var dateformat = GetOrgDateFormat();
    $(".dateInputMask").attr("placeholder", dateformat);
    $('.time').inputmask({
        mask: "h:s t\\m",
        placeholder: "hh:mm a",
        alias: "datetime",
        hourFormat: "12"
    });
});
