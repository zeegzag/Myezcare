var vm;


controllers.GroupTimesheetListController = function ($scope, $http, $timeout, StepsService, usSpinnerService) {
    vm = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetGroupTimesheetListPage").val());
    };
    $scope.GroupTimesheetList = [];
    $scope.GroupTimesheetModel = $scope.newInstance();
    $scope.SelectedScheduleIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.SavedGroupTimesheetList = []
    $scope.IsClockInTimeOverrideValue = false
    $scope.IsClockOutTimeOverrideValue = false
    $scope.ClockInTimeOverrideValue = null
    $scope.ClockOutTimeOverrideValue = null
    $scope.TotalRecord = {};

    $scope.SetSearchFilter = function () {
        $scope.SearchGroupTimesheetListPage = $scope.newInstance().SearchGroupTimesheetListPage;
    };
    $scope.SetSearchFilter();

    $scope.CommaSeparated = function (arr) {
        return arr ? (arr.join ? arr.join(',') : arr) : '';
    }

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            SearchGroupTimesheetListPage: {
                ...$scope.SearchGroupTimesheetListPage,
                EmployeeIDs: $scope.CommaSeparated($scope.SearchGroupTimesheetListPage.EmployeeIDs),
                ReferralIDs: $scope.CommaSeparated($scope.SearchGroupTimesheetListPage.ReferralIDs),
                PayorIDs: $scope.CommaSeparated($scope.SearchGroupTimesheetListPage.PayorIDs),
                CareTypeIDs: $scope.CommaSeparated($scope.SearchGroupTimesheetListPage.CareTypeIDs),
                FacilityIDs: $scope.CommaSeparated($scope.SearchGroupTimesheetListPage.FacilityIDs)
            },
            pageSize: null,//$scope.GroupTimesheetListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.GroupTimesheetListPager.sortIndex,
            sortDirection: $scope.GroupTimesheetListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.OverrideClockInValues = function (isReset, time) {
        if (!isReset) {
            $scope.GroupTimesheetList = $scope.SelectedGroupTimesheetList().map(x => {
                return {
                    ...x,
                    StrStartTimeEdit: time
                }
            })
        } else {
            $scope.GroupTimesheetList = $scope.SavedGroupTimesheetList
        }
    }

    $scope.OverrideClockOutValues = function (isReset, time) {
        if (!isReset) {
            $scope.GroupTimesheetList = $scope.SelectedGroupTimesheetList().map(x => {
                return {
                    ...x,
                    StrEndTimeEdit: time
                }
            })
        } else {
            $scope.GroupTimesheetList = $scope.SavedGroupTimesheetList
        }
    }

    $scope.GetGroupTimesheetList = function () {
        debugger
        var isValid = CheckErrors($("#frmFilters"), true);
        if (isValid) {
            //Reset Selcted Checkbox items and Control
            $scope.SelectedScheduleIds = [];
            $scope.SelectAllCheckbox = false;
            //Reset Selcted Checkbox items and Control

            var jsonData = $scope.SetPostData($scope.GroupTimesheetListPager.currentPage);

            AngularAjaxCall($http, HomeCareSiteUrl.GetGroupTimesheetList, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.GroupTimesheetList = response.Data;
                    $scope.SavedGroupTimesheetList = response.Data;
                    $scope.TotalRecord = response.Data.length;
                    $scope.SelectedItemCount = 0;
                    //$scope.GroupTimesheetListPager.currentPageSize = response.Data.length;
                    //$scope.GroupTimesheetListPager.totalRecords = response.Data.length;

                    angular.forEach($scope.GroupTimesheetList, function (item, key) {
                        item.StrStartTimeEdit = item.StartDate != null ? moment(item.StartDate).format('hh:mm a') : null;
                        item.StrEndTimeEdit = item.EndDate != null ? moment(item.EndDate).format('hh:mm a') : null;

                        item.StrStartDateEdit = item.StartDate != null ? moment(item.StartDate).format('MM/DD/YYYY') : null;
                        item.StrEndDateEdit = item.EndDate != null ? moment(item.EndDate).format('MM/DD/YYYY') : null;
                    });
                }
                ShowMessages(response);
            });
        }
    };

    $scope.GroupTimesheetListPager = new PagerModule("ScheduleID");
    $scope.GroupTimesheetListPager.getDataCallback = $scope.GetGroupTimesheetList;

    $scope.SearchGroupTimesheet = function () {
       /* alert("qwqwqw");*/
        $scope.GroupTimesheetListPager.currentPage = 1;
        $scope.GroupTimesheetListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        HideErrors($("#frmFilters"));
        $scope.SetSearchFilter();
        $scope.GroupTimesheetList = [];
    };
    $scope.SelectedItemCount = {};
    $scope.SelectGroupTimesheet = function (GroupTimesheet) {
        if (GroupTimesheet.IsChecked) {
            $scope.SelectedScheduleIds.push(GroupTimesheet.ScheduleID);
            $scope.SelectedItemCount = $scope.SelectedScheduleIds.length;
        }
        else {
            $scope.SelectedScheduleIds.remove(GroupTimesheet.ScheduleID);
            $scope.SelectedItemCount = $scope.SelectedScheduleIds.length;
        }

        if ($scope.SelectedScheduleIds.length == $scope.GroupTimesheetListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectAllCheckbox = !$scope.SelectAllCheckbox;
        $scope.SelectedScheduleIds = [];
        angular.forEach($scope.GroupTimesheetList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked) {
                $scope.SelectedScheduleIds.push(item.ScheduleID);
                $scope.SelectedItemCount = $scope.SelectedScheduleIds.length;
            }
            if (!item.IsChecked) {
                $scope.SelectedScheduleIds.remove(item.ScheduleID);
                $scope.SelectedItemCount = $scope.SelectedScheduleIds.length;
            }
        });
        return true;
    };

    $scope.SelectedGroupTimesheetList = function () {
        return $scope.GroupTimesheetList.filter(x => $scope.SelectedScheduleIds.indexOf(x.ScheduleID) !== -1);
    };

    $scope.HideErrorsGroupTimesheetList = function () {
        HideErrors($("#frmGroupTimesheetListEditable"));
        $scope.stepPrevious();
    };

    $scope.MappedTaskList = [];
    $scope.SelectedTaskIds = [];
    $scope.GroupVisitTaskOptionList = [];
    $scope.SelectAllTasksCheckbox = false;

    $scope.ValidateGroupTimesheetList = function () {
        var isValid = CheckErrors($("#frmGroupTimesheetListEditable"), true);
        if (isValid) {
            angular.forEach($scope.SelectedGroupTimesheetList(), function (item, key) {
                item.ClockInDateTime = moment(item.StrStartDateEdit + " " + item.StrStartTimeEdit).toDate();
                item.ClockOutDateTime = moment(item.StrEndDateEdit + " " + item.StrEndTimeEdit).toDate();
            });
            $scope.stepNext();

            if ($scope.MappedTaskList.length == 0) {
                var jsonData = angular.toJson({ careType: $scope.CommaSeparated($scope.SearchGroupTimesheetListPage.CareTypeIDs) });
                AngularAjaxCall($http, HomeCareSiteUrl.GetGroupVisitTask, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.MappedTaskList = response.Data.VisitTaskList;
                        $scope.HourList = response.Data.HourList;
                        $scope.MinuteList = response.Data.MinuteList;
                    }
                });
                //////////////////////////////////////////////////////////////////
                AngularAjaxCall($http, HomeCareSiteUrl.GetGroupVisitTaskOptionList, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.GroupVisitTaskOptionList = response.Data;
                    }
                });

                ///////////////////////////////////////////////////////////////////////
            }
        }
    };

    $scope.IsAnyTaskSelected = function () {
        return $scope.MappedTaskList.some(m => m.IsChecked && m.IsChecked === true)
    }

    $scope.SelectVisitTask = function (task) {
        if (task.IsChecked) {
            $scope.SelectedTaskIds.push(task.VisitTaskID);
        }
        else {
            $scope.SelectedTaskIds.remove(task.VisitTaskID);
        }
        $scope.SelectAllTasksCheckbox = ($scope.SelectedTaskIds.length == $scope.MappedTaskList.length);
        //var jsonData = angular.toJson({ VisitTaskID: task.VisitTaskID });
        //AngularAjaxCall($http, HomeCareSiteUrl.GetGroupVisitTaskOptionList, jsonData, "Post", "json", "application/json").success(function (response) {
        //    ShowMessages(response);
        //    if (response.IsSuccess) {
        //        $scope.GroupVisitTaskOptionList = response.Data;
        //    }
        //});
    };

    // This executes when select all checkbox in task table header is checked.
    $scope.SelectAllTasks = function () {
        $scope.SelectAllTasksCheckbox = !$scope.SelectAllTasksCheckbox;
        $scope.SelectedTaskIds = [];
        angular.forEach($scope.MappedTaskList, (item, key) => {
            item.IsChecked = $scope.SelectAllTasksCheckbox;
            if (item.IsChecked) {
                $scope.SelectedTaskIds.push(item.VisitTaskID);
            }
        });
    };

    $scope.SelectedTaskList = function () {
        return $scope.MappedTaskList.filter(x => $scope.SelectedTaskIds.indexOf(x.VisitTaskID) !== -1);
    };

    $scope.ValidateTaskList = function () {
        var isValid = CheckErrors($("#frmTaskListEditable"), true);
        if (isValid) {
            angular.forEach($scope.SelectedTaskList(), function (item, key) {
                if (!item.IsDetail) {
                    item.Hours = 0;
                    item.Minutes = 10;
                }
                item.ServiceTime = (item.Hours * 60) + item.Minutes;
            });
            $scope.stepNext();
        }
    };

    $scope.stepPrevious = function () {
        StepsService.steps().previous();
    };

    $scope.stepNext = function () {
        StepsService.steps().next();
    };

    $scope.stepData = {};
    $scope.submitFinal = function () {
        $scope.stepData.TaskList = $scope.SelectedTaskList();
        $scope.stepData.TimesheetDetails = $scope.SelectedGroupTimesheetList().slice();
        $scope.stepData.TimesheetDetails.forEach(i => {
            i.ClockInDateTime = $scope.dateWithoutTimeZone(i.ClockInDateTime);
            i.ClockOutDateTime = $scope.dateWithoutTimeZone(i.ClockOutDateTime);
        });

        var jsonData = angular.toJson($scope.stepData);
        AngularAjaxCall($http, HomeCareSiteUrl.SaveGroupTimesheetList, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                usSpinnerService.spin("spinner-1");
                window.location.reload();
            }
        });
    }

    $scope.dateWithoutTimeZone = function (date) {
        if (date) {
            var userTimezoneOffset = date.getTimezoneOffset() * 60000;
            date = new Date(date.getTime() - userTimezoneOffset);
            //return moment.utc(date).valueOf();
        }
        return date;
    }

};

controllers.GroupTimesheetListController.$inject = ['$scope', '$http', '$timeout', 'StepsService', 'usSpinnerService'];

$(document).ready(function () {
    var dateformat = GetOrgDateFormat();
    $(".dateInputMask").attr("placeholder", dateformat);
    $('.time').inputmask({
        mask: "h:s t\\m",
        placeholder: "hh:mm a",
        alias: "datetime",
        hourFormat: "12"
    });

    $('#filters').on('hidden.bs.collapse', function () {
        $('#btn-filters').html('Show Filters');
    })

    $('#filters').on('shown.bs.collapse', function () {
        $('#btn-filters').html('Hide Filters');
    })
});



$("#right-scroll-button").click(function () {
    event.preventDefault();
    $(".table-responsive").animate(
        {
            scrollLeft: "+=300px"
        },
        "slow"
    );
});

$("#left-scroll-button").click(function () {
    event.preventDefault();
    $(".table-responsive").animate(
        {
            scrollLeft: "-=300px"
        },
        "slow"
    );
});
