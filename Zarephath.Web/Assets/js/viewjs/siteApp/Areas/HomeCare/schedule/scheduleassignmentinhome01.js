﻿var custModel;
$calendar = $('#calendar');


controllers.ScheduleAssignmentController = function ($scope, $http, $compile, $timeout, $filter, $rootScope) {
    custModel = $scope;
    
    $scope.DateFormat = "YYYY/MM/DD";
    

    $scope.ShowLoadOnCalenderLoad = false;

    $scope.NewInstance = function () {
        return $.parseJSON($("#hdnScheduleAssignment").val());
    };

    
    if ($scope.NewInstance() != null) {
        $scope.ScheduleSearchModel = $scope.NewInstance().ScheduleSearchModel;
        $scope.ScheduleSearchModel.StartDate = moment();
        $scope.TempSearchSchEmployeeModel = $scope.NewInstance().SearchSchEmployeeModel;
        $scope.SearchSchEmployeeModel = $scope.NewInstance().SearchSchEmployeeModel;
        $scope.Skills = $scope.NewInstance().Skills;
        $scope.Preference = $scope.NewInstance().Preference;

    }
    //$scope.ScheduleSearchModel.StartDate = moment($scope.ScheduleSearchModel.StartDate).format(ClientDateFormat);
    //$scope.ScheduleSearchModel.EndDate = moment($scope.ScheduleSearchModel.EndDate).format(ClientDateFormat);
    

    //#region Employee List For Scheduling
    $scope.SchEmployeeDetailUrl = HomeCareSiteUrl.GetSchEmployeeDetailForPopupURL;

    
    $scope.SchEmployeeListPager = new PagerModule("Name");
    
   
    $scope.EmpSkillList = [];
    $scope.EmpPreferenceList = [];
    $scope.ScheduleNoteDetail = {};


    $scope.SchEmployeeList = [];
    $scope.SearchModelMapping = function () {
        
        $scope.SearchSchEmployeeModel = $.parseJSON(angular.toJson($scope.TempSearchSchEmployeeModel));
        $scope.SearchSchEmployeeModel.StartDate = moment();
    };
    
    $scope.SetSchEmployeeSearchPostData = function (fromIndex) {
        
        $scope.SearchSchEmployeeModel.StartDate = $scope.ScheduleSearchModel.StartDate;
        $scope.SearchSchEmployeeModel.EndDate = $scope.ScheduleSearchModel.EndDate;
        
        $scope.SearchSchEmployeeModel.StrSkillList = $scope.EmpSkillList ? $scope.EmpSkillList.toString() : "";
        $scope.SearchSchEmployeeModel.StrPreferenceList = $scope.EmpPreferenceList ? $scope.EmpPreferenceList.toString() : "";

        var pagermodel = {
            searchSchEmployeeModel: $scope.SearchSchEmployeeModel,
            pageSize: $scope.SchEmployeeListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.SchEmployeeListPager.sortIndex,
            sortDirection: $scope.SchEmployeeListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.GetSchEmployeeList = function (isSearchDataMappingRequire) {
        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        $scope.AjaxStart = true;
        var jsonData = $scope.SetSchEmployeeSearchPostData($scope.SchEmployeeListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetSchEmployeeListForScheduleURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
               
                $.each($scope.SelectedEmpList, function(index,item) {
                    $.each(response.Data.Items, function(index1,item1) {
                        if (item1.EmployeeID == item)
                            item1.IsChecked = true;
                    });
                });


                if (response.Data.CurrentPage == 1)
                    $scope.SchEmployeeList = [];

                if (response.Data.CurrentPage == 1 || $scope.SchEmployeeListPager.lastPage < response.Data.CurrentPage)
                    Array.prototype.push.apply($scope.SchEmployeeList, response.Data.Items);


                //$scope.SelectedEmpList
                //$scope.SchEmployeeList

               

                $scope.SchEmployeeList.filter(item => item.EmployeeID > 6)


                $scope.SchEmployeeListPager.lastPage = response.Data.CurrentPage;
                //$scope.ReferralList = response.Data.Items;
                $scope.SchEmployeeListPager.currentPageSize = response.Data.Items.length;
                $scope.SchEmployeeListPager.totalRecords = response.Data.TotalItems;

                if (!$scope.$root.$$phase) {
                    $scope.$apply();
                }
                //$scope.ShowCollpase();
            }
            ShowMessages(response);
            $scope.AjaxStart = false;
        });


    };

    $scope.SchEmployeeListPager.getDataCallback = $scope.GetSchEmployeeList;

    $scope.SearchSchEmployee = function () {
        $scope.SchEmployeeListPager.currentPage = 1;
        $scope.SchEmployeeListPager.pageSize = $scope.TempSearchSchEmployeeModel.PageSize;

        var selectedEmpListNotEmpty = $scope.SelectedEmpList.length > 0;
        $scope.SelectedEmpList = [];

        $scope.SchEmployeeListPager.getDataCallback(true);
        if (selectedEmpListNotEmpty) {
            $scope.SeachAndGenerateCalenders();
        }
        return true;
    };

    $scope.Refresh = function () {
        $scope.SchEmployeeListPager.getDataCallback();
    };


    
    $scope.LoadMoreSchEmployee = function () {
        $scope.SchEmployeeListPager.nextPage();
    }

    //$scope.SchEmployeeListPager.getDataCallback(true);



    $scope.SelectedEmpList = [];
    $scope.OnEmployeeSelection = function (isChecked, employeeID) {

        if (isChecked) {
            if ($scope.SelectedEmpList.indexOf(employeeID) === -1)
                $scope.SelectedEmpList.push(employeeID);
        } else {
            $scope.SelectedEmpList.remove(employeeID);
        }

        $scope.CalenderRefresh($scope.TempCalendarObj);
        //$scope.GenerateCalenders();
    }
    //#endregion


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

    //#region For Referral Listing 

    //$scope.ReferralDetailUrl = HomeCareSiteUrl.GetReferralDetailForPopupURL;
    
    if (typeof IsAttandancePage != 'undefined') {
        $scope.ShowEmployeeList = !IsAttandancePage;
    }
    
    $scope.ShowCalenders = true;

    
    $scope.ViewSchEmpoyeeList = function () {
        $scope.ShowEmployeeList = !$scope.ShowEmployeeList;
        $timeout(function () {
            // $scope.wall.fitWidth();
        });
    };
   
    $scope.Resizable = function () {
        $timeout(function () {
            $(".schedules").draggable({ handle: "#schedule-assignment-header" });
            $(".schedules").resizable();
            //$(".calender > .col-lg-12.margin-top-40-print").resizable();           
        });
    };
    $scope.ResetResizable = function () {
        $timeout(function () {
            $(".schedules").draggable('destroy');
            $(".schedules").resizable('destroy');
            //$(".calender > .col-lg-12.margin-top-40-print").resizable();           
        });
    };


    

    
    //#endregion For Referral Listing 

    //#region For Calender 

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
    $scope.FacilityList = [];

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
        if ($("#frmScheduleSearch").length == 0) {
            return;
        }
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
        var color = $scope.colors[data.ScheduleStatusID - 1];
        var editable = true;
        var isPastEvent = false;
        if (data.UnAllocated) {
            color = "#e7505a !important";
            editable = false;
        }
        if (moment(moment(data.StartDate).format("L")).toDate() < moment(moment(new Date()).format("L")).toDate()) {
            isPastEvent  = true;
            editable = false;
        }
        if (!data.UsedInScheduling) color = "#f4cd41 !important";

        if (data.OnHold) color = "#007fff !important";
        if (data.IsDenied) color = "#00BCD4 !important";
        if (data.ClockInTime != null && data.ClockOutTime != null && data.IsPCACompleted) {
            data.ScheduleStatusName = "Completed";
            color = "#D3D3D3 !important";
        }
        if (data.IsApprovalRequired) {
            data.ScheduleStatusName = "Approval Required";
            color = "#FFFF99 !important"
        }
        if (data.IsVirtualVisit) color = "#ff6a00 !important";
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
        return newList;
    };
    $scope.CalenderItem = {};
    var timeout = null;

    $scope.TempCalendarObj = null;
    if ($scope.NewInstance() != null) {
        $scope.ReferralList = $scope.NewInstance().ReferralList;
    }
    
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
                width: '140px'
            },
            {
                labelText: 'Hrs',
                //field: 'NewAllocatedHrs',
                text: function (resource) {
                    
                    return resource.NewUsedHrs + "/" + resource.NewAllocatedHrs;
                },

                width: '40px'

            }
        ];
    };
    $scope.RefetchResources = function (calendarObj,callback) {
        
        if (calendarObj) {
            $scope.TempCalendarObj = calendarObj;
            $scope.ScheduleSearchModel.StartDate = calendarObj.StartDate().format();
            $scope.ScheduleSearchModel.EndDate = calendarObj.EndDate().format();
        }

        var data = angular.toJson($scope.ScheduleSearchModel);
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralForSchedulingURL, data, "Post", "json", "application/json", false).success(function (response) {
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

                $scope.GetSchEmployeeList();
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
                EmployeeIDs: $scope.SelectedEmpList.length > 0 ? $scope.SelectedEmpList.toString() : "", //$scope.ScheduleSearchModel.ReferralID ? $scope.ScheduleSearchModel.ReferralID.toString() : "",
                StartDate: start,
                EndDate: moment(end).add(-1, 'day'),
                SchStatus: $scope.ScheduleSearchModel.SchStatus,
                ServiceTypeID: $scope.ScheduleSearchModel.ServiceTypeID
            });
            calendarObj.IsLoading = true;
            AngularAjaxCall($http, HomeCareSiteUrl.GetScheduleListByReferralsURL, jsonData, "Post", "json", "application/json", $scope.ShowLoadOnCalenderLoad).success(function(response) {
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
                ChangeSchedule : $scope.ChangeSchedule,
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
    $scope.OnEventChange = function (calendarObj, event, delta, successCallback, errorCallback) {
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
            },10);
        });
    }

    $scope.RemoveSchedule = function (calendarObj, event, popOverFunctions) {
        
        //$('.webpopUiCls').webuiPopover('hideAll');
        $scope.HildeAllPopUp();
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = { scheduleID: event.scheduleModel.ScheduleID };
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteScheduleFromCalenderURL, jsonData, "Post", "json", "application/json").success(function (response) {

                    if (response.IsSuccess) {
                        $scope.SeachAndGenerateCalenders(true);
                        popOverFunctions.Hide();
                        calendarObj.reloadEvents();
                        $scope.SchEmployeeListPager.getDataCallback(true);
                        //$scope.ReferralListPager.getDataCallback(true);
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
        var careTypeId = event.scheduleModel.CareTypeId;
        $scope.OpenEmpRefSchModal(referralId, event, careTypeId);
    };


    $scope.NoteModalAction = function (calendarObj, event) {        
        var referralId = event.scheduleModel.ReferralID;
        var scheduleId = event.scheduleModel.ScheduleID;
        $scope.ScheduleNoteDetail = {};
        $scope.ScheduleNoteDetail.ScheduleID = scheduleId;
        $scope.calendarObj12 = calendarObj;
        $scope.ScheduleNoteDetail.RemoveScheduleReason = event.scheduleModel.ScheduleComment;
        //$scope.scheduleId = scheduleId;
        //$('.webpopUiCls').webuiPopover('hideAll');
        $scope.HildeAllPopUp();
        $('#notesModal').modal({ backdrop: false, keyboard: false });
        $("#notesModal").modal('show');        
    };
    $scope.ChangeScheduleEmployeeList = [];
    
    $scope.ChangeSchedule = function (calendarObj, event) {
        
        if (event.scheduleModel == undefined) {
            var referralId = calendarObj.ReferralID;
            var scheduleId = calendarObj.ScheduleID;
            $scope.ScheduleTimeDetail = {};
            $scope.ScheduleTimeDetail.ScheduleID = scheduleId;
            $scope.ScheduleTimeDetail.ReferralID = referralId;
            $scope.ScheduleTimeDetail.ReferralName = calendarObj.ReferralName;
            $scope.ScheduleTimeDetail.StartTime = calendarObj.StartTime;
            $scope.ScheduleTimeDetail.EndTime = calendarObj.EndTime;
            $scope.ScheduleTimeDetail.ScheduleDate = calendarObj.StartDate;
            $scope.ScheduleTimeDetail.EmployeeID = calendarObj.EmployeeID ? calendarObj.EmployeeID.toString() : "";

        }
        else {
            var referralId = event.scheduleModel.ReferralID;
            var scheduleId = event.scheduleModel.ScheduleID;
            $scope.ScheduleTimeDetail = {};
            $scope.ScheduleTimeDetail.ScheduleID = scheduleId;
            $scope.ScheduleTimeDetail.ReferralID = referralId;
            $scope.ScheduleTimeDetail.ReferralName = event.scheduleModel.ReferralName;
            $scope.ScheduleTimeDetail.StartTime = event.scheduleModel.StartTime;
            $scope.ScheduleTimeDetail.EndTime = event.scheduleModel.EndTime;
            $scope.ScheduleTimeDetail.ScheduleDate = event.scheduleModel.StartDate;
            $scope.ScheduleTimeDetail.EmployeeID = event.scheduleModel.EmployeeID ? event.scheduleModel.EmployeeID.toString() : "";
        }
        
        $scope.calendarObj12 = calendarObj;
        //$scope.scheduleId = scheduleId;
        //$('.webpopUiCls').webuiPopover('hideAll');
        $scope.HildeAllPopUp();
        $('#ChangeScheduleModal').modal({ backdrop: false, keyboard: false });
        $("#ChangeScheduleModal").modal('show');

        var jsonDataEmployeeList = angular.toJson({});
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeList, jsonDataEmployeeList, "Get", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                
                $scope.ChangeScheduleEmployeeList = response.Data;
            }
        });

    };

    $scope.SaveNewSchedule = function () {
        debugger
        var isValid = CheckErrors($("#frmSchedule"));
        var StartTime = moment($scope.ScheduleTimeDetail.StartTime , 'h:mma');
        var EndTime = moment($scope.ScheduleTimeDetail.EndTime, 'h:mma');
        //console.log(StartTime.isBefore(EndTime));
        if (isValid) {
            if (StartTime.isBefore(EndTime)) {

                ShowVisitReasonActionModal({
                    ScheduleID: $scope.ScheduleTimeDetail.ScheduleID,
                    OnSet: function (data, save) {
                        var model = Object.assign({}, $scope.ScheduleTimeDetail);
                        model.StartTime = moment(StartTime).format('hh:mm A');
                        model.EndTime = moment(EndTime).format('hh:mm A');
                        var jsonData = angular.toJson(model);
                        // alert(jsonData);

                        AngularAjaxCall($http, HomeCareSiteUrl.SaveNewScheduleURL, jsonData, "Post", "json", "application/json").success(function (response) {
                            if (response.IsSuccess) {
                                save();
                                $("#ChangeScheduleModal").modal('hide');
                                
                                if (typeof RefreshEmpClockInOutList != 'undefined') {
                                    RefreshEmpClockInOutList();
                                }
                                else {
                                    $scope.calendarObj12.RefreshCalender();
                                }
                               
                            }

                            ShowMessages(response);
                            //$scope.ScheduleNoteDetail.IsPatientDeniedService = null;
                            //$scope.ScheduleNoteDetail.RemoveScheduleReason = null;
                        });
                    }
                });
            }
            else {
                ShowMessage("Start Time can not be greater than End Time.","error");
            }
        };
    }

    $scope.CloseScheduleModal = function () {
        $("#ChangeScheduleModal").modal('hide');
        HideErrors("#frmSchedule");
    
    };

    $scope.SaveNote = function () {
        $scope.ScheduleNoteDetail.IsSaveNoteOnly = !$scope.ScheduleNoteDetail.IsPatientDeniedService;
        var isValid = CheckErrors($("#frmNote"));        
        if (isValid) {
            
                var jsonData = angular.toJson($scope.ScheduleNoteDetail);                
               // alert(jsonData);

                AngularAjaxCall($http, HomeCareSiteUrl.RemoveScheduleURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $("#notesModal").modal('hide');
                        $scope.calendarObj12.RefreshCalender();
                    }

                    $scope.ScheduleNoteDetail.IsPatientDeniedService = false;
                    var el = $('#IsPatientDeniedService').parent();
                    if (el.hasClass('checked'))
                        el.removeClass('checked');

                    ShowMessages(response);
                    //$scope.ScheduleNoteDetail.IsPatientDeniedService = null;
                   //$scope.ScheduleNoteDetail.RemoveScheduleReason = null;
                });            
        };                
    }

    $scope.CloseNotesModal = function () {
        $("#notesModal").modal('hide');
        HideErrors("#frmNote");
        $scope.ScheduleNoteDetail.IsPatientDeniedService = false;
        var el = $('#IsPatientDeniedService').parent();
        if (el.hasClass('checked'))
            el.removeClass('checked');
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


    

    
    $scope.OpenEmpRefSchModal = function (referralid, event, careTypeId) {
        $('#emprefschmodal').modal({ backdrop: false, keyboard: false });
        $("#emprefschmodal").modal('show');
        var startDate = $scope.ScheduleSearchModel.StartDate;
        scopeEmpRefSch.CallOnPopUpLoad(referralid, startDate, event, null, careTypeId);

        //
        //scopeEmpRefSch.SchedulesCreated = false;
        //scopeEmpRefSch.SearchModel = {};
        //scopeEmpRefSch.SearchModel.ReferralID = referralid;
        //scopeEmpRefSch.SearchModel.ScheduleID = 0;
        //scopeEmpRefSch.SearchModel.StartDate = $scope.ScheduleSearchModel.StartDate;
        //scopeEmpRefSch.SearchModel.EndDate = moment($scope.ScheduleSearchModel.StartDate).add(30, 'days');
        
        //scopeEmpRefSch.SearchModel.StartDateDisable = false;
        //if (event) {
        //    scopeEmpRefSch.SearchModel.ScheduleID = event.scheduleModel.ScheduleID;
        //    scopeEmpRefSch.SearchModel.StartDateDisable = true;
        //    scopeEmpRefSch.SearchModel.StartDate = event.scheduleModel.StartDate;
        //    scopeEmpRefSch.SearchModel.EndDate = event.scheduleModel.StartDate;
        //}
        
        //scopeEmpRefSch.GetEmpRefSchOptions();
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
        //
        if(scopeEmpRefSch.SchedulesCreated)
            $scope.SeachAndGenerateCalenders();
    });

    $scope.ShowAlertMsg= function(msg) {
        toastr.error(msg);
    }
    // Print Schedule Calender
    $scope.printToCart = function (CalenderPrint) {
        
        var innerContents = document.getElementById(CalenderPrint).innerHTML;
        var popupWinindow = window.open('', '_blank', 'width=700,height=700,scrollbars=no,menubar=yes,toolbar=no,location=no,status=yes,titlebar=no');
        popupWinindow.document.open();
        popupWinindow.document.write('<html><head><link href = "/Assets/library/fullcalendar/fullcalendar.css" rel = "stylesheet" /><link href="/Assets/library/ion.rangeslider/css/ion.rangeSlider.css" rel="stylesheet" /><link href="/Assets/library/ion.rangeslider/css/ion.rangeSlider.skinFlat.css" rel="stylesheet" /><link href="/Assets/library/fullcalendar/scheduler.css" rel="stylesheet" /><link href="/Assets/library/LineProgressbar/jquery.lineProgressbar.css" rel="stylesheet" /> <h1 style="text-align:center" > MyEzCare </h1 > </head > <body onload="window.print()" >' + innerContents + '</html>');
        popupWinindow.document.close();
    }
};

controllers.ScheduleAssignmentController.$inject = ['$scope', '$http', '$compile', '$timeout', '$filter'];

var wall;
$(document).ready(function () {
    var dateformat = GetOrgDateFormat();
    $(".dateInputMask").attr("placeholder", dateformat);
    $('.time').inputmask({
        mask: "h:s t\\m",
        placeholder: "hh:mm a",
        alias: "datetime",
        hourFormat: "12"
    });

  
});
ChangeSchedulePopup = function (obj) {
    $scope = custModel;
    $scope.ChangeSchedule(obj,obj)
}