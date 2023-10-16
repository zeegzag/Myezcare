var custModel;

$calendar = $('#calendar');

controllers.ScheduleAssignmentController = function ($scope, $http, $compile, $timeout, $filter) {
    custModel = $scope;
    $scope.DateFormat = "YYYY/MM/DD";

    $scope.ShowLoadOnCalenderLoad = false;

    $scope.NewInstance = function () {
        return $.parseJSON($("#hdnScheduleAssignment").val());
    };

    $scope.FacilityList = $scope.NewInstance().FacilityList;

    $scope.ScheduleSearchModel = $scope.NewInstance().ScheduleSearchModel;

    //#region For Update Schedule Status
    $scope.CnclStatus = window.CancelStatus;
  
    $scope.OpenUpdateScheduleStatusPopup = function (calendarObj, event, popOverFunctions) {
        $scope.topper = document.body.scrollTop;
        $scope.CancelStatus = window.CancelStatus;
        $scope.ScheduleDetail = $.parseJSON(JSON.stringify(event.scheduleModel));
        $scope.ScheduleDetail.CalenderObj = calendarObj;
        if (window.IsAttandancePage) {
            if ($scope.ScheduleDetail.ScheduleStatusID.toString() == window.UnconfirmedStatus.toString()) {
                $scope.ScheduleDetail.ScheduleStatusID = window.ConfirmedStatus;
                $scope.UpdateScheduleStatus($scope.ScheduleDetail);
                return;
            }
        }


        //$('#EditSchedule').modal({ backdrop: false, keyboard: false });
        $('#EditSchedule').modal('show');
        
    };
    $scope.UpdateScheduleStatus = function (scheduleModel) {
        var isValid = CheckErrors($("#frmScheduleEdit"));
        if (isValid) {
            var jsonData = { scheduleModel: scheduleModel };
            AngularAjaxCall($http, HomeCareSiteUrl.UpdateScheduleMasterURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    if (!$scope.$root.$$phase) {
                        $scope.$apply();
                    }
                    $scope.ScheduleDetail.CalenderObj.reloadEvents();
                    $('#EditSchedule').modal('hide');
                    //if (scheduleModel.ScheduleStatusID == window.CancelStatus) {
                    //    bootboxDialog(function (result) {
                    //        if (result) {
                    //            $timeout(function () {
                    //                $scope.ScheduleDetail.StartDate = scheduleModel.StartDate; //$filter('date')(new Date(), 'L');
                    //                $scope.ScheduleDetail.EndDate = scheduleModel.EndDate; //$filter('date')(new Date(), 'L');
                    //                $scope.ScheduleDetail.ScheduleStatusID = 0;
                    //                $scope.ScheduleDetail.Comments = null;
                    //            });
                    //            $('#RescheduleClient').modal('show');
                    //        }
                    //    }, bootboxDialogType.Confirm, "Reschedule", scheduleModel.IsReschedule ? window.AlreadyRescheduleConfirm : window.RescheduleConfirm, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
                    //}
                }
            });
        }
    };
   
    //#endregion

    //#region For Calender 
    $scope.ShowCalenders = true;
    $scope.Resizable = function () {
        $timeout(function () {
            $(".schedules").draggable({ handle: "#schedule-assignment-header" });
            $(".schedules").resizable();
            //$(".calender > .col-lg-12.margin-top-40-print").resizable();           
        });
    };


    $scope.CalenderSortOption = {
        Name: "Name",
        Age: "Age"
    };
    $scope.CalenderSortIndex = {
        Asc: "asc",
        Desc: "desc"
    };

    $scope.colors = ["#61AEE4 !important", "#6aa84f !important", "#e69138 !important", "#e69138 !important", "#e69138 !important", "#cc0000 !important",
                "#cc0000 !important", "#e69138 !important", "#cc0000 !important"];//[ "#3598DC", "#26C281", "#2AB4C0", "#E35B5A", "#F2784B", "#C8D046", "#9B59B6"];
    $scope.SelectedFacilityIDs = [];
    $scope.CalendarList = [];
    $scope.GetFacilutyListForAutoCompleteURL = SiteUrl.GetFacilutyListForAutoCompleteURL;
    
    $scope.SeachAndGenerateCalenders = function (searchClickFlag) {
        $scope.SearchClickFlag = searchClickFlag == undefined ? false : searchClickFlag;
        $scope.ReloadLoadCalender = false;
        $scope.CreateCalender = true;
        //$scope.RefetchResources();
        $scope.GenerateCalenders();
    };

    $timeout(function () {
        $scope.SeachAndGenerateCalenders(true);
    }, 1000);

    $scope.GenerateCalenders = function () {
        if (!CheckErrors("#frmScheduleSearch")) {
            return;
        }

        $scope.CalendarList = [];
        $scope.CalendarList.push({
            IsLoading: false,
            //Facility: data,
            //EmployeeList: $scope.EmployeeList,
            ReferralList: $scope.ReferralList,
            ScheduleList: [],
            //RemoveSchedulesFromWeekFacility: $scope.RemoveSchedulesFromWeekFacility,
            //SortBy: $scope.CalenderSortOption.Age,
            //SortIndex: $scope.CalenderSortIndex.Asc
        });
        $scope.Resizable();
    };
    $scope.CreateCalanderEventModel = function (data) {
        var color = "#ffffff !important";  //        $scope.colors[data.ScheduleStatusID - 1];
        //var editable = true;
        var editable = false;
        var isPastEvent = false;

        if (data.ClockInTime != null && !data.IsPCACompleted) {
            color = "#FF0000 !important";
        }
        else if (data.IsPCACompleted) {
            color = "#00FF00 !important";
        }
        //if (data.ReferralTSDateID) {
        //    color = "#e7505a !important";
        //    editable = false;
        //}
        //else {

        //    if (data.IsPatientAttendedSchedule) {
        //        color = "#6aa84f !important";
        //    } else if (data.IsPatientAttendedSchedule==false) {
        //        color = "#09b1f1 !important";
        //    }
        //    else if (!data.IsPatientAttendedSchedule) {
        //        color = "#b9b9b9 !important";
        //    }
        //}


        //if (moment(moment(data.StartDate).format("L")).toDate() < moment(moment(new Date()).format("L")).toDate()) {
        //    isPastEvent  = true;
        //    editable = false;
        //}

        //if (!data.UsedInScheduling) color = "#f4cd41 !important";
        if (data.OnHold) color = "#007fff !important";
       
        return {
            start: data.StartDate,//moment(data.StartDate),//(data.StartDate),
            end: data.EndDate,//moment(data.EndDate).add(1, 'day'),// data.EndDate,// 
            title: data.Name,
            scheduleModel: data,
            backgroundColor: color,
            allDay: false,
            resourceId: data.ReferralID,
            editable: editable,
            isPastEvent:isPastEvent,
            droppable: true,

            OpenEmpRefSchModal: $scope.OpenEmpRefSchModal,
            ShowAlertMsg:  $scope.ShowAlertMsg
            //overlap: editable
            //rendering: "inverse-background",//data.rendering,
            //backgroundColor: "#ff9f89",
            //color: '#ff9f89'
        };
    };
    $scope.SetCalenderData = function (list) {
        var newList = new Array();
        $.each(list, function (index, data) {
            newList.push($scope.CreateCalanderEventModel(data));
        });
        console.log(newList);
        return newList;
    };
    $scope.CalenderItem = {};
    var timeout = null;

    $scope.TempCalendarObj = null;
    $scope.ReferralList = $scope.NewInstance().ReferralList;
    $scope.ReferralListFilter = function (value) {
        return function (item) {
            if (item.IsDeleted == 0 || item.ReferralID == value) {
                return item;
            }
        };
    };
    $scope.ResourceColumns = function () {
        return [
            {
                labelText: 'Patients', //'Employees',
                field: 'ReferralName',
                width: '100px'
            }
            //,
            //{
            //    labelText: 'Hrs',
            //    text: function (resource) {
                    
            //        return resource.NewUsedHrs + "/" + resource.NewAllocatedHrs;
            //    },

            //    width: '40px'

            //}
        ];
    };
    $scope.RefetchResources = function (calendarObj,callback) {
        
        if (calendarObj) {
            $scope.TempCalendarObj = calendarObj;
            $scope.ScheduleSearchModel.StartDate = calendarObj.StartDate().format();
            $scope.ScheduleSearchModel.EndDate = calendarObj.EndDate().format();
        }

        var data = angular.toJson($scope.ScheduleSearchModel);
        AngularAjaxCall($http, HomeCareSiteUrl.CaseManagement_GetReferralForSchedulingURL, data, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {

                $scope.ReferralList = response.Data;

                $.each($scope.SelectedReferralList, function (index, item) {
                    $.each($scope.ReferralList, function (index, subitem) {
                        if (subitem.ReferralID === item) {
                            subitem.IsPatientChecked = true;
                            return false;
                        }
                    });
                });

                //$scope.SelectedReferralList

                //if (calendarObj) {
                //    $(calendarObj.CalendarElement()).fullCalendar('refetchResources');
                //}

                if (callback) {
                    callback($scope.ReferralList);
                    if ($scope.SearchClickFlag && $scope.ReferralList != undefined && $scope.ReferralList.length > 0) {
                        $scope.OnAllScheduleSearch(true, false);
                        $scope.SearchClickFlag = false;
                    }
                }

                if ($scope.CreateCalender) {
                    $scope.CreateCalender = false;
                    if (calendarObj) {
                        calendarObj.reloadEvents();
                    } else {
                        $scope.GenerateCalenders();
                    }
                }

                if ($scope.ReloadLoadCalender && calendarObj) {
                    $scope.ReloadLoadCalender = false;
                    calendarObj.reloadEvents();
                }

               
            }
        });
    }

    $scope.GetResourcesList = function (calendarObj, callback) {
        if (timeout != null)
            clearTimeout(timeout);
        timeout = setTimeout(function () {

            $scope.RefetchResources(calendarObj, callback);
            //callback($scope.ReferralList);
        }, 100);
    };

    $scope.SelectedReferralList = [];

    $scope.OnAllScheduleSearch = function (IsChecked, realodCalender) {

        if ($scope.ReferralList == undefined || $scope.ReferralList.length == 0) {
            return false;
        }

        var listResource = $scope.TempCalendarObj.getResourcelistFromMemory();

        if (IsChecked) {
            $scope.IsAllPatientChecked = true;
            if ($scope.SelectedReferralList.length > 0) {
                $scope.SelectedReferralList = [];
            }

            //var result = $scope.ReferralList.map(function (a) { return a.ReferralID; });
            var result = listResource.map(function (a) { return a.ReferralID; });
            $scope.SelectedReferralList = result;
            
            $.each(listResource, function (index, data) {
                var resourceObject = data;
                resourceObject.IsPatientChecked = true;
            });
            
        }
        else {
            $scope.IsAllPatientChecked = false;
            $scope.SelectedReferralList = [];
            $.each(listResource, function (index, data) {
                var resourceObject = data;
                resourceObject.IsPatientChecked = false;
            });
        }

        if (realodCalender) {
            $scope.TempCalendarObj.reloadEvents();
        }
    }

    $scope.OnScheduleSearch = function (resource) {
        if (resource.IsPatientChecked) {
            var index = $scope.SelectedReferralList.indexOf(resource.ReferralID);

            if (index === -1)
                $scope.SelectedReferralList.push(resource.ReferralID);
        }
        else {
            var index = $scope.SelectedReferralList.indexOf(resource.ReferralID);
            $scope.SelectedReferralList.splice(index, 1);
        }

        if ($scope.ReferralList.length === $scope.SelectedReferralList.length)
            $scope.IsAllPatientChecked = true;
        else
            $scope.IsAllPatientChecked = false;

        resource.calendarObj.reloadEvents();
    }

    $scope.SearchClickFlag = true;
    $scope.GetScheduleList = function (calendarObj, start, end, callback) {
        //if ($scope.ReferralList) {
        //var result = $scope.ReferralList.map(function(a) { return a.ReferralID; });

        if ($scope.SelectedReferralList.length > 0) {

            var result = $scope.SelectedReferralList;
            var jsonData = angular.toJson({
                ReferralIDs: result,
                EmployeeIDs: "", //$scope.ScheduleSearchModel.ReferralID ? $scope.ScheduleSearchModel.ReferralID.toString() : "",
                StartDate: start,
                EndDate: moment(end).add(-1, 'day'),
                SchStatus: $scope.ScheduleSearchModel.SchStatus,
                FacilityIDs: $scope.ScheduleSearchModel.FacilityIDs
            });
            calendarObj.IsLoading = true;
            AngularAjaxCall($http, HomeCareSiteUrl.CaseManagement_GetScheduleListByReferralsURL, jsonData, "Post", "json", "application/json", $scope.ShowLoadOnCalenderLoad).success(function (response) {
                calendarObj.IsLoading = false;
                $scope.ShowLoadOnCalenderLoad = false;
                if (response.IsSuccess) {
                    calendarObj.ScheduleList = $scope.SetCalenderData(response.Data.ScheduleList);
                    callback(calendarObj.ScheduleList);
                }
                ShowMessages(response);

            });
        }
        else {
            $scope.templist = [];
            calendarObj.ScheduleList = $scope.SetCalenderData($scope.templist);
            callback(calendarObj.ScheduleList);
        }
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
                removeEvent: $scope.RemoveSchedule,
                ReplaceSchedule: $scope.ReplaceSchedule,
                NoteModalAction: $scope.NoteModalAction,
                PatientAttendanceActionModal: $scope.PatientAttendanceActionModal
            };
            html = $compile(content)(newScope);
        }
        //$(ele).find('.fc-content').html(html);
        $(ele).find('.fc-content').html(html);

    };

    $scope.OnEventDrop = function (calendarObj, dropperData, date, successCallback, resourceData) {

        if (!CheckErrors("#frmScheduleSearch") || isNaN(parseInt(resourceData.id))) {
            return;
        }
        
        if (dropperData) {
            var newSchedule = {};

            if (dropperData.event) { //Update event
                var event = dropperData.event;
                newSchedule = angular.copy(dropperData.event.scheduleModel);
                newSchedule.StartDate = event.start.format();//date.format($scope.DateFormat);
                newSchedule.EndDate = event.end.format();//date.clone().add(daysDiff, 'day').format($scope.DateFormat); //dropperData.event.scheduleModel.DefaultScheduleDays - 1
                newSchedule.EmployeeID = event.scheduleModel.EmployeeID;
                newSchedule.ReferralID = resourceData.id;
            } else {//Create event

                newSchedule = $scope.NewInstance().ScheduleMaster;
                newSchedule.StartDate = date.format(); //date.format($scope.DateFormat);
                newSchedule.EndDate = date.format();//date.clone().add(2, 'hours').format();  //date.clone().add(defaultScheduleDays - 1, 'day').format($scope.DateFormat);
                newSchedule.EmployeeID = dropperData.EmployeeID;
                newSchedule.ReferralID = resourceData.id;
            }
            newSchedule.DayView = calendarObj.GetView().name === 'timelineOneDays' ? true : false;
            console.log(newSchedule.StartDate);
            $scope.SaveSchedule(newSchedule, false, function (e) {
                successCallback(e);
                //calendarObj.reloadEvents();
                //$scope.SchEmployeeListPager.getDataCallback(true);
                calendarObj.RefreshCalender();
            }, function () {
                // errorCallback();
            });


        }
    };
    $scope.OnEventChange = function (calendarObj, event,delta, successCallback, errorCallback) {
        
        var newSchedule = angular.copy(event.scheduleModel);
        newSchedule.StartDate = event.start.format();//event.start.format($scope.DateFormat);
        newSchedule.EndDate = event.end.format();//event.end.add(-1, 'day').format($scope.DateFormat);
        newSchedule.EmployeeID = event.scheduleModel.EmployeeID;
        newSchedule.ReferralID = event.resourceId;
        newSchedule.DayView = calendarObj.GetView().name === 'timelineOneDays' ? true : false;
        
        console.log(newSchedule.StartDate);
        calendarObj.IsLoading = true;
        $scope.SaveSchedule(newSchedule, true, function (e) {
            successCallback(e);
            //calendarObj.reloadEvents();
            //$scope.SchEmployeeListPager.getDataCallback(true);
            calendarObj.RefreshCalender();
        }, function () {
            errorCallback();
        });
    };

    $scope.UploadPatientCSV = function () {
        $('#uploadpatientcsvmodal').modal({ backdrop: false, keyboard: false });
        $("#uploadpatientcsvmodal").modal('show');
    }

    $scope.SaveSchedule = function (newSchedule, isEditing, successCallback, errorCallback) {

        //If Schedule EndDate Greate then Week End Date Of Start Date is Less Then Week Start Date Dont Save show Error Message.
        //if (moment(newSchedule.StartDate) < moment($scope.ScheduleSearchModel.StartDate) || moment(newSchedule.EndDate) > moment($scope.ScheduleSearchModel.EndDate)) {
        //    ShowMessage(window.ScheduleStartEndDateBetweenWeekStartEndDate, "error");
        //    if (errorCallback)
        //        errorCallback();
        //    return;
        //}

        var jsonData = angular.toJson({
            scheduleMaster: {
                ScheduleMaster: newSchedule
            }
        });
        AngularAjaxCall($http, HomeCareSiteUrl.SaveScheduleFromCalenderURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                var eventObject = $scope.CreateCalanderEventModel(response.Data);
                successCallback(eventObject);
                //$scope.ReferralListPager.getDataCallback(true);
            }
            else if (response.ErrorCode == window.ErrorCode_Warning) {
                if (errorCallback) {
                    errorCallback(response.Data);
                }
                return;
            } else {
                if (errorCallback) {
                    errorCallback();
                }
            }
            ShowMessages(response);
        });
    };
    $scope.EventOrder = function (a, b, calenderObj) {
        if (calenderObj.SortBy == $scope.CalenderSortOption.Name) {
            if (a.scheduleModel.LastName < b.scheduleModel.LastName) {
                if (calenderObj.SortIndex == $scope.CalenderSortIndex.Asc)
                    return -1;
                else
                    return 1;
            }
            if (a.scheduleModel.LastName > b.scheduleModel.LastName) {
                if (calenderObj.SortIndex == $scope.CalenderSortIndex.Asc)
                    return 1;
                else
                    return -1;

            }
        }
        if (calenderObj.SortBy == $scope.CalenderSortOption.Age) {
            //if (parseFloat(a.scheduleModel.Age) < parseFloat(b.scheduleModel.Age)) {
            //    if (calenderObj.SortIndex == $scope.CalenderSortIndex.Asc)
            //        return -1;
            //    else
            //        return 1;
            //}
            //if ( parseFloat(a.scheduleModel.Age) > parseFloat(b.scheduleModel.Age)) {
            //    if (calenderObj.SortIndex == $scope.CalenderSortIndex.Asc)
            //        return 1;
            //    else
            //        return -1;
            //}

            var dateA = new Date(a.scheduleModel.Dob).getTime();
            var dateB = new Date(b.scheduleModel.Dob).getTime();

            if (parseFloat(dateA) < parseFloat(dateB)) {
                if (calenderObj.SortIndex == $scope.CalenderSortIndex.Asc)
                    return -1;
                else
                    return 1;
            }
            if (parseFloat(dateA) > parseFloat(dateB)) {
                if (calenderObj.SortIndex == $scope.CalenderSortIndex.Asc)
                    return 1;
                else
                    return -1;
            }




        }

        return 1;

    };

    $scope.HildeAllPopUp= function() {
        $.each($('.webui-popover'), function(index, item) {
            
            setTimeout(function() {
                $(item).webuiPopover('hideAll');
            },100);
        });
    }

    $scope.RemoveSchedule = function (calendarObj, event, popOverFunctions) {
        //$('.webpopUiCls').webuiPopover('hideAll');
        $scope.HildeAllPopUp();
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = { scheduleID: event.scheduleModel.ScheduleID };
                AngularAjaxCall($http, HomeCareSiteUrl.DayCare_DeleteScheduleFromCalenderURL, jsonData, "Post", "json", "application/json").success(function (response) {

                    if (response.IsSuccess) {
                        popOverFunctions.Hide();
                        calendarObj.reloadEvents();
                       
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteConfirmationMessageForSchedule, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);

    };
    
    $scope.ReplaceSchedule = function (calendarObj, event) {
        //        
        //$('.webpopUiCls').webuiPopover('hideAll');
        $scope.HildeAllPopUp();
        var referralId = event.scheduleModel.ReferralID;
        $scope.OpenEmpRefSchModal(referralId,event);
    };


    $scope.PatientAttendanceActionModal = function (calendarObj, event) {
       
        $scope.ScheduleAttendaceDetail = {};
        $scope.ScheduleAttendaceDetail.ScheduleID = event.scheduleModel.ScheduleID;
        $scope.ScheduleAttendaceDetail.ReferralID = event.scheduleModel.ReferralID;
        $scope.ScheduleAttendaceDetail.AbsentReason = event.scheduleModel.AbsentReason;
        $scope.ScheduleAttendaceDetail.IsPatientAttendedSchedule = event.scheduleModel.TempIsPatientAttendedSchedule;
        $scope.calendarObj13 = calendarObj;
        $scope.event13 = event;

        $scope.HildeAllPopUp();
        $('#schAttendenceModal').modal({ backdrop: false, keyboard: false });
        $("#schAttendenceModal").modal('show');

    };


    $scope.SavePatientAttendance = function () {
        var isValid = CheckErrors($("#frmschAttendenceModal"));
        if (isValid) {
            var jsonData = angular.toJson($scope.ScheduleAttendaceDetail);
            AngularAjaxCall($http, HomeCareSiteUrl.DayCare_SavePatientAttendanceURL, jsonData, "Post", "json", "application/json").success(function(response) {
                if (response.IsSuccess) {
                    $scope.event13.scheduleModel.IsPatientAttendedSchedule = $scope.event13.scheduleModel.TempIsPatientAttendedSchedule;
                    $("#schAttendenceModal").modal('hide');
                    $scope.calendarObj13.RefreshCalender();
                }
                ShowMessages(response);
            });
        }
    };

    $scope.ClosePatientAttendanceActionModal = function () {
        $scope.event13.scheduleModel.TempIsPatientAttendedSchedule = $scope.event13.scheduleModel.IsPatientAttendedSchedule;
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
        $("#schAttendenceModal").modal('hide');
        HideErrors("#frmschAttendenceModal");
    };


    $scope.NoteModalAction = function (calendarObj, event) {        
        var referralId = event.scheduleModel.ReferralID;
        var scheduleId = event.scheduleModel.ScheduleID;
        $scope.ScheduleNoteDetail = {};
        $scope.ScheduleNoteDetail.ScheduleID = scheduleId;
        $scope.calendarObj12 = calendarObj;
        //$scope.scheduleId = scheduleId;
        //$('.webpopUiCls').webuiPopover('hideAll');
        $scope.HildeAllPopUp();
        $('#notesModal').modal({ backdrop: false, keyboard: false });
        $("#notesModal").modal('show');        
    };

    $scope.SaveNote = function () {
        $scope.ScheduleNoteDetail.IsSaveNoteOnly = !$scope.ScheduleNoteDetail.IsPatientDeniedService;
        var isValid = CheckErrors($("#frmNote"));        
        if (isValid) {
                var jsonData = angular.toJson($scope.ScheduleNoteDetail);                
                AngularAjaxCall($http, HomeCareSiteUrl.DayCare_RemoveScheduleURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $("#notesModal").modal('hide');
                        $scope.calendarObj12.RefreshCalender();
                    }
                    ShowMessages(response);
                    //$scope.ScheduleNoteDetail.IsPatientDeniedService = null;
                   //$scope.ScheduleNoteDetail.RemoveScheduleReason = null;
                });            
        };                
    }

    $scope.CloseNotesModal = function () {
        $("#notesModal").modal('hide');
        HideErrors("#frmNote");
                
    };

    //$scope.GetEmployeeMatchingPreferencesUrl = HomeCareSiteUrl.GetEmployeeMatchingPreferencesURL;
    $scope.ResourceRender = function (calendarObj, resourceObj, labelTds, bodyTds) {
         //
        var content = $("#empReource").html();
        var empResource = $(labelTds[0]).find(".fc-cell-text");
        resourceObj.Url = HomeCareSiteUrl.GetEmployeeMatchingPreferencesURL;
        resourceObj.ID = resourceObj.EmployeeID + '|' + $scope.ScheduleSearchModel.ReferralID;
        resourceObj.OpenEmpRefSchModal = $scope.OpenEmpRefSchModal;
        resourceObj.OnScheduleSearch = $scope.OnScheduleSearch;
        resourceObj.calendarObj = calendarObj;

        var newScope = $scope.$new(true);
        newScope.DataModel = {
            //calendarObj: calendarObj,
            resourceObj: resourceObj
        };
        var html = $compile(content)(newScope);

        $(empResource).html(html);


        //$(ele).find('.fc-content').html(html);
        //$(ele).find('.fc-content').html(html);

    };


    $scope.ShowCalenderLoaderFlag = false;
    $scope.ShowCalenderLoader= function(isLoading, view) {
        
        $scope.ShowCalenderLoaderFlag = isLoading;
        //if (isLoading)
        //    alert(1);
        //else
        //    alert(2);

    }


    $scope.DayRender = function (calendarObj, date, cell) {

        //var thisDate = new Date(date.format());
        //if (!calendarObj.DateWiseScheduleCountList) {
        //    return;
        //}
        //var scheduleModel = $.grep(calendarObj.DateWiseScheduleCountList, function (data, i) {
        //    
        //    var itemDate = new Date(moment(data.Date).format());
        //    if (thisDate == itemDate)
        //        return true;
        //    else
        //        return false;
        //});

        //if (scheduleModel != null) {
        //    if (scheduleModel.TotalScheduleCount > Facility.BadCapacity) {
        //        $(cell).addClass('outofcapacity');
        //    }
        //    var remainingCapacity = Facility.BadCapacity - scheduleModel.TotalScheduleCount;
        //    $(cell).attr("title", (remainingCapacity < 0 ? 0 : remainingCapacity) + "Bads remains");
        //}
    };
    $scope.AllEventRender = function (calendarObj, view) {
        $(view.el).find('.outofcapacity').removeClass("outofcapacity");
        $.each(calendarObj.DateWiseScheduleCountList, function (index, data) {
            var offSet = view.dayGrid.dateToCellOffset(moment(data.Date)) + 1;
            if (offSet >= 0) {
                var cell = view.dayGrid.dayEls[offSet];
                if (calendarObj.Facility.BadCapacity < data.TotalScheduleCount && calendarObj.Facility.BadCapacity > 0) {
                    $(cell).addClass('outofcapacity');
                }
                //var remainingCapacity = calendarObj.Facility.BadCapacity - data.TotalScheduleCount;
                //$(cell).attr("title", (remainingCapacity < 0 ? 0 : remainingCapacity) + " Bads remains");
            }

        });
        //$timeout(function () {
        //    $scope.wall.fitWidth();
        //}, 2000);
    };
    $scope.SortCalenderEvent = function (calendarObj, sortby) {
        if (calendarObj.SortBy == sortby) {
            calendarObj.SortIndex = calendarObj.SortIndex == $scope.CalenderSortIndex.Asc ? $scope.CalenderSortIndex.Desc : $scope.CalenderSortIndex.Asc;
        } else {
            calendarObj.SortBy = sortby;
            calendarObj.SortIndex = $scope.CalenderSortIndex.Asc;
        }
        calendarObj.reloadEvents();
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };
    $scope.hasActive = function (id) {
        var st = "#" + id + " div .fc-view-container .fc-view";
        return $(st).hasClass("fc-month-view");
    };



    $scope.CalenderNext = function () {
        // $(".customCalender").fullCalendar('next');
    };
    $scope.CalenderPrev = function () {
        // $(".customCalender").fullCalendar('prev');
    };

    $scope.CalenderRefresh = function (calendarObj) {

        $scope.ReloadLoadCalender = true;
        $scope.ShowCalenderLoader(true);
        calendarObj.refetchResources();
        //
        //$(calendarObj.CalendarElement()).fullCalendar('refetchResources');
    };
    $scope.CalenderChangeView = function (view) {
        // $(".customCalender").fullCalendar('changeView', view);
        //$(".customCalender").fullCalendar('refetchEvents');
    };
    $scope.PrintDiv = function (id) {
        var style = $("#scroll-content").attr("style");
        $("#scroll-content")[0].removeAttribute("style");
        myApp.showPleaseWait();
        //$scope.PrintContent = true;
        setTimeout(function () {
            printDiv($("#" + id));
            myApp.hidePleaseWait();
            //$scope.PrintContent = false;
            $("#scroll-content")[0].setAttribute("style", style);
            $scope.$apply();
        }, 500);
    };

    //#endregion For Calender 


    

    
    $scope.OpenEmpRefSchModal = function (referralid,event) {
        //
        $('#emprefschmodal').modal({ backdrop: false, keyboard: false });
        $("#emprefschmodal").modal('show');
        var startDate = $scope.ScheduleSearchModel.StartDate;
        scopeEmpRefSch.CallOnPopUpLoad(referralid,startDate,event);
    }
    
    $scope.SearchPatient = function (data) {

        $scope.ScheduleSearchModel.ReferralName = data.PatientName;
        $scope.SeachAndGenerateCalenders();
        $.each($('a.webpop'), function (index,ele) {
            $(ele).webuiPopover('hide');
        });

        
        
    }

    //$scope.CloseEmpRefSchModal= function() {
    //    
    //    $('#emprefschmodal').modal("hide");
    //    if(scopeEmpRefSch.SchedulesCreated)
    //        $scope.SeachAndGenerateCalenders();

        
    //}

    $('#emprefschmodal').on('hidden.bs.modal', function(e) {
        // do something...
        
        if(scopeEmpRefSch.SchedulesCreated)
            $scope.SeachAndGenerateCalenders();
    });

    $scope.ShowAlertMsg= function(msg) {
        toastr.error(msg);
    }
};

controllers.ScheduleAssignmentController.$inject = ['$scope', '$http', '$compile', '$timeout', '$filter'];

var wall;
$(document).ready(function () {

  
});
