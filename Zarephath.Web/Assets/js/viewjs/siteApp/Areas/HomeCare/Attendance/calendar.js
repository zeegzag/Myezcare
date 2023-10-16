var vm;
controllers.clockinoutController = function ($scope, $http, $window, $compile, $timeout) {
    vm = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnEmployeeModel").val());
    };
    $scope.EmpCalenderModel = $scope.newInstance();
    $scope.SearchEmpCalender = $scope.newInstance().SearchEmpCalender;
    $scope.ShowLoadOnCalenderLoad = false;
    $scope.cal = $.parseJSON($("#hdnEmployeeModel").val());

    //#region Employee Calender Related Code


    $scope.CalendarList = [];
    //debugger;
    if ($('#hdnPermissionAllEmployee').val() == "1") {
        $scope.EmployeeList = $scope.EmpCalenderModel.EmployeeList;
    } else {
        $scope.EmployeeList = $scope.EmpCalenderModel.EmployeeList.filter(emp => '' + emp.EmployeeID == '' + $('#hdnEmployeeID').val());
    }

    $scope.GenerateCalenders = function () {

        if (!CheckErrors("#frmEmpCalender")) {
            return;
        }

        $scope.CalendarList = [];
        $scope.CalendarList.push({
            IsLoading: false,
            //Facility: data,
            EmployeeList: $scope.EmployeeList,
            ScheduleList: [],
            //RemoveSchedulesFromWeekFacility: $scope.RemoveSchedulesFromWeekFacility,
            //SortBy: $scope.CalenderSortOption.Age,
            //SortIndex: $scope.CalenderSortIndex.Asc
        });

    };
    $scope.CreateCalanderEventModel = function (data) {
        return {
            start: data.StartDate,//moment(data.StartDate),//(data.StartDate),
            end: data.EndDate,//moment(data.EndDate).add(1, 'day'),// data.EndDate,//
            title: data.EmployeeName,
            facility: data.FacilityName,
            scheduleModel: data,
            //backgroundColor: $scope.colors[data.ScheduleStatusID - 1],
            allDay: false,
            resourceId: data.EmployeeID
        };
    };
    $scope.SetCalenderData = function (list) {
        var newList = new Array();
        $.each(list, function (index, data) {
            data.WorkMinutes = $scope.setTime(data.WorkMinutes);
            data.BreakMinutes = $scope.setTime(data.BreakMinutes)
            newList.push($scope.CreateCalanderEventModel(data));
        });
        return newList;
    };
    $scope.CalenderItem = {};
    $scope.GetScheduleList = function (calendarObj, start, end, callback) {
        var result = $scope.EmployeeList.map(function (a) { return a.EmployeeID; });
        //debugger;
        var jsonData = angular.toJson({
            EmployeeIDs: result,
            StartDate: start,
            EndDate: moment(end).add(-1, 'day')
        });
        calendarObj.IsLoading = true;
        AngularAjaxCall($http, HomeCareSiteUrl.EmployeeAttendanceCalendarURL, jsonData, "Post", "json", "application/json", $scope.ShowLoadOnCalenderLoad).success(function (response) {
            calendarObj.IsLoading = false;
            if (response.IsSuccess) {
                calendarObj.ScheduleList = $scope.SetCalenderData(response.Data.EmployeeAttendanceModels);
                //calendarObj.Employee = response.Data.Employee;
                //?calendarObj.DateWiseScheduleCountList = response.Data.DateWiseScheduleCountList;
                //calendarObj.EmployeeList = response.Data.EmployeeList;

                //calendarObj.refetchResources();

                callback(calendarObj.ScheduleList);
            }
            ShowMessages(response);

        });
    };
    var timeout = null;
    $scope.GetResourcesList = function (calendarObj, callback) {
        if (timeout != null)
            clearTimeout(timeout);
        timeout = setTimeout(function () {
            //callback(calendarObj.EmployeeList);
            callback($scope.EmployeeList);
        }, 100);
    };
    $scope.EventRender = function (calendarObj, event, ele) {
        var scheduleModel = $.grep(calendarObj.ScheduleList, function (data, i) {
            if (data.scheduleModel.ScheduleID == event.scheduleModel.ScheduleID)
                return true;
            else
                return false;
        });

        var content = $("#calendereventmarker").html();
        if (scheduleModel != null) {
            var newScope = $scope.$new(true);
            newScope.DataModel = {
                calendarObj: calendarObj,
                event: event,
                updateScheduleStatus: $scope.OpenUpdateScheduleStatusPopup,
            };
            html = $compile(content)(newScope);
        }
        //$(ele).find('.fc-content').html(html);
        ele.context.style.backgroundColor = "rgb(255, 115, 115)";
        $(ele).find('.fc-content').html(html);

    };
    $scope.ResourceRender = function (calendarObj, resourceObj, labelTds, bodyTds) {
        // 
        var content = $("#empReource").html();
        var empResource = $(labelTds[0]).find(".fc-cell-text");
        resourceObj.Url = HomeCareSiteUrl.GetEmployeeMatchingPreferencesURL;
        resourceObj.ID = resourceObj.EmployeeID;//+ '|' + $scope.ScheduleSearchModel.ReferralID;
        var newScope = $scope.$new(true);
        newScope.DataModel = {
            //calendarObj: calendarObj,
            resourceObj: resourceObj
        };
        var html = $compile(content)(newScope);

        $(empResource).html(html);

        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }

    };
    $scope.printToCart = function (CalenderPrint) {

        var innerContents = document.getElementById(CalenderPrint).innerHTML;
        var popupWinindow = window.open('', '_blank', 'width=700,height=700,scrollbars=no,menubar=yes,toolbar=no,location=no,status=yes,titlebar=no');
        popupWinindow.document.open();
        popupWinindow.document.write('<html><head><script src="/Assets/js/sitejs/jquery.js"></script><link href="/Assets/css/sitecss/font-awesome.css" rel="stylesheet"><link href = "/Assets/library/fullcalendar/fullcalendar.css" rel = "stylesheet" /><link href="/Assets/library/ion.rangeslider/css/ion.rangeSlider.css" rel="stylesheet" /><link href="/Assets/library/ion.rangeslider/css/ion.rangeSlider.skinFlat.css" rel="stylesheet" /><link href="/Assets/library/fullcalendar/scheduler.css" rel="stylesheet" /><link href="/Assets/library/LineProgressbar/jquery.lineProgressbar.css" rel="stylesheet" /> <h1 style="text-align:center" > MyEzCare </h1 > <script>function onload() {$($(".fc-right button")[0]).css("display","none");$($(".fc-right button")[1]).css("display","none");$(".ng-hide").css("display","none");window.print();}</script> </head > <body onload="onload()" style="-webkit-print-color-adjust: exact;">' + innerContents + '</html>');

        popupWinindow.document.close();
    }
    $scope.GetDate = function (d) {
        var a = '' + d;
        var b = new Date(a.match(/\d+/)[0] * 1);
        return moment(b).format();
    }
    $scope.setTime = (totalminutes) => {
        return '' +
            (parseInt(totalminutes / 60) < 10 ? '0' + parseInt(totalminutes / 60) : parseInt(totalminutes / 60))
            + ':'
            + (parseInt(totalminutes % 60) < 10 ? '0' + parseInt(totalminutes % 60) : parseInt(totalminutes % 60))
            + ':00';
    }
    $scope.EmployeeReources = function (refreshCalender, clenderEle) {
        //if ($scope.SearchEmpCalender.EmployeeID)
        //    $scope.SearchEmpCalender.EmployeeIDs = $scope.SearchEmpCalender.EmployeeID.toString();
        //else
        //    $scope.SearchEmpCalender.EmployeeIDs = "";
        if (refreshCalender) {
            $(clenderEle).fullCalendar('refetchResources');
            $(clenderEle).fullCalendar('refetchEvents');
            $(clenderEle).fullCalendar('rerenderEvents');

        } else {
            $scope.GenerateCalenders();
        }
    }
    $scope.SeachAndGenerateCalenders = function () {
        $scope.EmployeeReources();
    };

    $scope.CalenderRefresh = function () {
        $scope.ShowLoadOnCalenderLoad = true;
        $scope.EmployeeReources(true, ".customCalender");
    };
    $("a#empCalender").on('shown.bs.tab', function (e, ui) {
        $scope.SeachAndGenerateCalenders();
    });
    $scope.GenerateCalenders();
};
controllers.clockinoutController.$inject = ['$scope', '$http', '$window', '$compile', '$timeout'];

$(document).ready(function () {

});