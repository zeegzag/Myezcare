var custModel;

controllers.ReferralCalenderController = function ($scope, $http, $compile, $timeout) {    
    custModel = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnRefCalenderModel").val());
    };
    $scope.RefCalenderModel = $scope.newInstance();
    $scope.SearchRefCalender = $scope.newInstance().SearchRefCalender;
    
    $scope.ShowLoadOnCalenderLoad = false;
    //#region Employee Calender Related Code
   

    $scope.CalendarList = [];
    $scope.EmployeeList = [];
    $scope.GenerateCalenders = function () {
        if (!CheckErrors("#frmRefCalender")) {
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
            title: data.Name,
            scheduleModel: data,
            //backgroundColor: $scope.colors[data.ScheduleStatusID - 1],
            allDay: false,
            resourceId: data.EmployeeID
        };
    };
    $scope.SetCalenderData = function (list) {
        var newList = new Array();
        $.each(list, function (index, data) {
            newList.push($scope.CreateCalanderEventModel(data));
        });
        return newList;
    };
    $scope.CalenderItem = {};
    var timeout = null;
    $scope.GetResourcesList = function (calendarObj, callback) {
        if (timeout != null)
            clearTimeout(timeout);
        timeout = setTimeout(function () {
            //callback(calendarObj.EmployeeList);
            callback($scope.EmployeeList);
        }, 100);
    };
    $scope.GetScheduleList = function (calendarObj, start, end, callback) {

        var employeeIDs = $scope.EmployeeList.map(function (a) { return a.EmployeeID; });

        if ($scope.SearchRefCalender.ReferralID)
            $scope.SearchRefCalender.ReferralIDs = $scope.SearchRefCalender.ReferralID.toString();
        else
            $scope.SearchRefCalender.ReferralIDs = "";

        var jsonData = angular.toJson({
            EmployeeIDs: employeeIDs,
            ReferralIDs: $scope.SearchRefCalender.ReferralIDs,
            StartDate: start,
            EndDate: moment(end).add(-1, 'day')
        });
        calendarObj.IsLoading = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetScheduleListByEmployeesURL, jsonData, "Post", "json", "application/json", $scope.ShowLoadOnCalenderLoad).success(function (response) {
            calendarObj.IsLoading = false;
            if (response.IsSuccess) {                
                calendarObj.ScheduleList = $scope.SetCalenderData(response.Data.ScheduleList);
                //calendarObj.Employee = response.Data.Employee;
                calendarObj.DateWiseScheduleCountList = response.Data.DateWiseScheduleCountList;
                //calendarObj.EmployeeList = response.Data.EmployeeList;

                //calendarObj.refetchResources();

                callback(calendarObj.ScheduleList);
            }
            ShowMessages(response);

        });
    };
    $scope.DefaultScheduleDaysSet = false;

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
        $(ele).find('.fc-content').html(html);

    };
    $scope.ResourceRender = function (calendarObj, resourceObj, labelTds, bodyTds) {        
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





    $scope.EmployeeReources = function (refreshCalender, clenderEle) {        
        if ($scope.SearchRefCalender.EmployeeID)
            $scope.SearchRefCalender.EmployeeIDs = $scope.SearchRefCalender.EmployeeID.toString();
        else 
            $scope.SearchRefCalender.EmployeeIDs = "";

        //if ($scope.SearchRefCalender.ReferralID)
        //    $scope.SearchRefCalender.ReferralIDs = $scope.SearchRefCalender.ReferralID.toString();
        //else
        //    $scope.SearchRefCalender.ReferralIDs = "";

        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeesForEmpCalenderURL, { employeeIDs: $scope.SearchRefCalender.EmployeeIDs }, "Post", "json", "application/json").success(function (response) {

            if (response.IsSuccess) {
                $scope.EmployeeList = response.Data;
                //$scope.GenerateCalenders();

                if (refreshCalender) {
                    $(clenderEle).fullCalendar('refetchResources');
                    $(clenderEle).fullCalendar('refetchEvents');
                    $(clenderEle).fullCalendar('rerenderEvents');

                } else {
                    $scope.GenerateCalenders();
                }
            }
        });
    }

    $scope.SeachAndGenerateCalenders = function () {
        $scope.EmployeeReources();
    };

    $scope.CalenderRefresh = function () {
        $scope.ShowLoadOnCalenderLoad = true;
        $scope.EmployeeReources(true, ".customCalender");
    };

    if (!$scope.RefCalenderModel.IsPartial) {
        $scope.SeachAndGenerateCalenders();
    }
    $("a#patientcalender").on('shown.bs.tab', function (e, ui) {
        $scope.SeachAndGenerateCalenders();
    });


    //#endregion

    $scope.printToCart = function (CalenderPrint) {

        var innerContents = document.getElementById(CalenderPrint).innerHTML;
        var popupWinindow = window.open('', '_blank', 'width=700,height=700,scrollbars=no,menubar=yes,toolbar=no,location=no,status=yes,titlebar=no');
        popupWinindow.document.open();
        popupWinindow.document.write('<html><head><script src="/Assets/js/sitejs/jquery.js"></script><link href="/Assets/css/sitecss/font-awesome.css" rel="stylesheet"><link href = "/Assets/library/fullcalendar/fullcalendar.css" rel = "stylesheet" /><h1 style="text-align:center" > MyEzCare </h1 > <script>function onload() {$($(".fc-right button")[0]).css("display","none");$($(".fc-right button")[1]).css("display","none");window.print();}</script> </head > <body onload="onload()" style="-webkit-print-color-adjust: exact;">' + innerContents+ '</html>');
        popupWinindow.document.close();
    }


};

controllers.ReferralCalenderController.$inject = ['$scope', '$http', '$compile', '$timeout'];


