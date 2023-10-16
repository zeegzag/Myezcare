var custModel;

controllers.AttendanceMasterController = function ($scope, $http, $compile) {
    custModel = $scope;

    $scope.NewInstance = function () {
        return $.parseJSON($("#hdnAttendanceMaster").val());
    };

    $scope.AttendanceModel = $scope.NewInstance();

    $scope.TempSearchAttendanceModel = $scope.AttendanceModel.AttendanceMasterSearchModel;

    $scope.colors = ["#95a5a6", "#1bbc9b", "#f3565d"];
    $scope.SelectedFacilityIDs = [];
    $scope.Calendar = {};

    $scope.OnFacilitySelect = function () {
        // $scope.SelectedFacilityIDs.push($scope.TempSearchAttendanceModel.FacilityID);
        //$scope.Calendar = {
        //    AttendanceList: []
        //};
        //$scope.$apply();

        $scope.CalenderRefresh();
    };


    $scope.CreateCalanderEventModel = function (data) {
        return {
            start: data.StartDate,//(data.StartDate),
            end: (new Date(data.EndDate)).addDays(1),// data.EndDate,// 
            title: data.Name,
            attendanceModel: data,
            setComment: $scope.UpdateCommentForAttendance,
            backgroundColor: data.AttendanceStatus != null ? $scope.colors[data.AttendanceStatus] : '#95a5a6',
            allDay: true
        };
    };

    $scope.SetCalenderData = function (list) {
        var newList = new Array();
        $.each(list, function (index, data) {
            newList.push($scope.CreateCalanderEventModel(data));
        });
        return newList;
    };

    $scope.GetAttendanceList = function (calendarObj, start, end, callback) {
        var jsonData = angular.toJson({
            FacilityID: calendarObj.Facility.FacilityID,//$scope.TempSearchAttendanceModel.FacilityID,
            ClientName: $scope.TempSearchAttendanceModel.ClientName,
            StartDate: start,
            EndDate: moment(end).add(-1, 'day')
        });
        AngularAjaxCall($http, SiteUrl.GetAttendanceListByFacilityURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                calendarObj.AttendanceList = $scope.SetCalenderData(response.Data.AttendanceDetails);
                calendarObj.Facility = response.Data.FacilityDetail;
                callback(calendarObj.AttendanceList);
            }
            ShowMessages(response);
        });
    };

    $scope.EventRender = function (calendarObj, event, ele) {
        var attendanceModel = $.grep(calendarObj.AttendanceList, function (data, i) {
            if (data.attendanceModel.AttendanceMasterID == event.attendanceModel.AttendanceMasterID)
                return true;
            else
                return false;
        });
        var content = $("#calendereventmarker").html();
        if (attendanceModel != null) {
            var newScope = $scope.$new(true);
            newScope.DataModel = {
                calendarObj: calendarObj,
                event: event,
                removeEvent: $scope.RemoveSchedule
            };
            html = $compile(content)(newScope);
        }

        $(ele).find('.fc-content').html(html);
    };

    $scope.ClickEvent = function (calendarObj, event, dataFromCalender) {

        if (event.attendanceModel.AttendanceStatus && dataFromCalender.ScheduleMasterID == undefined)  //1== Present
        {
            $scope.EditSchedule(calendarObj, event);
            $('#EditSchedule').modal('show');
            
        } else {

            if (dataFromCalender.ScheduleMasterID != undefined) {
                var isValid = CheckErrors($("#frmScheduleEdit"));
                if (!isValid)
                    return false;

            }
           
            var jsonData = angular.toJson({
                attendanceDetail: dataFromCalender.ScheduleMasterID!=undefined ? dataFromCalender : event.attendanceModel //event.attendanceModel
            });
            AngularAjaxCall($http, SiteUrl.UpdateAttendanceURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    if (event.attendanceModel != null) {
                        //event.attendanceModel.AttendanceStatus = response.Data.AttendanceStatus;
                        //event.attendanceModel.UpdatedDate = response.Data.UpdatedDate;
                        //event.attendanceModel.UpdatedByName = response.Data.UpdatedByName;
                        
                        event.attendanceModel = response.Data;

                        //event.attendanceModel.Comment = response.Data.Comment;
                        event.backgroundColor = event.attendanceModel.AttendanceStatus != null ? $scope.colors[event.attendanceModel] : '#95a5a6';
                        calendarObj.updateEvent(event);
                        calendarObj.element.fullCalendar('refetchEvents');
                        $('#EditSchedule').modal('hide');
                    }
                }
                ShowMessages(response);
            });
        }

    };
    
    $scope.SaveAbsentRecordFoClient = function (scheduleDetail) {
        
    $scope.ClickEvent(scheduleDetail.TempCalendarObj, scheduleDetail.TempEvent, scheduleDetail);
};


    $scope.UpdateCommentForAttendance = function (calendarObj, event) {
        var jsonData = angular.toJson({
            attendanceDetail: event.attendanceModel
        });
        AngularAjaxCall($http, SiteUrl.UpdateCommentAttendanceURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                if (event.attendanceModel != null) {
                    event.attendanceModel.Comment = response.Data.Comment;
                    event.attendanceModel.UpdatedDate = response.Data.UpdatedDate;
                    event.attendanceModel.UpdatedByName = response.Data.UpdatedByName;
                    event.popOverObj.Hide();
                    calendarObj.updateEvent(event);
                }
            }
            ShowMessages(response);
        });

    };

    $scope.CalenderNext = function () {
        $(".calendarLT").fullCalendar('next');
    };
    $scope.CalenderPrev = function () {
        $(".calendarLT").fullCalendar('prev');
    };
    $scope.CalenderRefresh = function () {
        $(".calendarLT").fullCalendar('refetchEvents');
    };
    $scope.CalenderChangeView = function (view) {
        $(".calendarLT").fullCalendar('changeView', view);
        $(".calendarLT").fullCalendar('refetchEvents');
    };
    $scope.ShowCalenders = true;
    $scope.ViewCalenders = function () {
        $scope.ShowCalenders = !$scope.ShowCalenders;
    };




    $scope.CalendarList = [];
    $scope.TokenInputObj = {};
    $scope.GetFacilutyListForAutoCompleteURL = SiteUrl.GetFacilutyListForAutoCompleteURL;

    $scope.LoadAllFacility = function (regionID) {

        var jsonData = { regionID: regionID };// $scope.TempSearchReferralModel.RegioinID };
        AngularAjaxCall($http, SiteUrl.LoadAllFacilityByRegion, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.TokenInputObj.clear();
                $.each(response.Data, function (i, item) {
                    $scope.TokenInputObj.add(item);
                });
            }

        });

    };

    $scope.OnFacilityAdd = function (item, e) {
        $scope.SelectedFacilityIDs.push(item.FacilityID);
        $scope.CalendarList.push({
            Facility: item,
            ScheduleList: []
        });
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };

    $scope.OnFacilityRemove = function (item, e) {
        $scope.SelectedFacilityIDs.remove(item.FacilityID);

        var removedItem = $.grep($scope.CalendarList, function (data, i) {
            if (data.Facility.FacilityID == item.FacilityID)
                return true;
            else
                return false;
        });
        if (removedItem.length > 0)
            $scope.CalendarList.remove(removedItem[0]);
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }

    };

    $scope.EditSchedule = function (calendarObj, event) {
        $scope.topper = document.body.scrollTop;
        $scope.CancelStatus = window.CancelStatus;
        $scope.ScheduleDetail = $.parseJSON(JSON.stringify(event.attendanceModel));
        $scope.ScheduleDetail.TempEvent = event;
        $scope.ScheduleDetail.TempCalendarObj = calendarObj;
        $scope.$apply();
    };








};
controllers.AttendanceMasterController.$inject = ['$scope', '$http', '$compile'];

