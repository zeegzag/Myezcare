var custModel;

$calendar = $('#calendar');

controllers.ScheduleAssignmentController = function ($scope, $http, $compile, $timeout, $filter) {
    custModel = $scope;
    $scope.DateFormat = "YYYY/MM/DD";

    $scope.ShowLoadOnCalenderLoad = false;

    $scope.NewInstance = function () {
        return $.parseJSON($("#hdnScheduleAssignment").val());
    };

    //#region For Update Schedule Status
    $scope.CnclStatus = window.CancelStatus;
    $scope.PickUpLocationFilter = function (value) {
        return function (item) {
            if (item.IsDeleted == 0 || item.TransportLocationID == value) {
                return item;
            }
        };
    };
    $scope.FacilityFilter = function (value) {
        return function (item) {
            if (item.IsDeleted == 0 || item.FacilityID == value) {
                return item;
            }
        };
    };
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
    $scope.ReSchedule = function (scheduleModel) {
        
        var isValid = CheckErrors($("#frmReScheduleClient"));
        if (isValid) {
            var jsonData = { scheduleModel: scheduleModel };
            AngularAjaxCall($http, HomeCareSiteUrl.ReScheduleClientURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.ScheduleDetail.CalenderObj.reloadEvents();
                    $('#RescheduleClient').modal('hide');
                }
            });
        }
    };
    //#endregion

    //#region For Referral Listing 

    $scope.ReferralDetailUrl = HomeCareSiteUrl.GetReferralDetailForPopupURL;

    $scope.ReferralListPager = new PagerModule("ClientName");
    $scope.ShowReferralList = !IsAttandancePage;
    $scope.ShowCalenders = true;

    $scope.TempSearchReferralModel = $scope.NewInstance().SearchReferralModel;
    $scope.TempSearchReferralModel.MaxAge = 100;
    $scope.SearchReferralModel = $scope.NewInstance().SearchReferralModel;
    $scope.SearchReferralModel.MaxAge = 100;
    $scope.RegionList = $scope.NewInstance().RegionList;
    $scope.WeekMaster = $scope.NewInstance().WeekMaster;
    $scope.WeekMasterList = $scope.NewInstance().WeekMasterList;

    $scope.ReferralList = [];

    $scope.SearchModelMapping = function () {
        $scope.SearchReferralModel = $.parseJSON(angular.toJson($scope.TempSearchReferralModel));
    };

    $scope.SetReferalSearchPostData = function (fromIndex) {
        var pagermodel = {
            searchReferralModel: $scope.SearchReferralModel,
            pageSize: $scope.ReferralListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ReferralListPager.sortIndex,
            sortDirection: $scope.ReferralListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.GetReferralList = function (isSearchDataMappingRequire) {
        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        $scope.AjaxStart = true;
        var jsonData = $scope.SetReferalSearchPostData($scope.ReferralListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralListForScheduleURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {

                if (response.Data.CurrentPage == 1)
                    $scope.ReferralList = [];

                if (response.Data.CurrentPage == 1 || $scope.ReferralListPager.lastPage < response.Data.CurrentPage)
                    Array.prototype.push.apply($scope.ReferralList, response.Data.Items);

                $scope.ReferralListPager.lastPage = response.Data.CurrentPage;
                //$scope.ReferralList = response.Data.Items;
                $scope.ReferralListPager.currentPageSize = response.Data.Items.length;
                $scope.ReferralListPager.totalRecords = response.Data.TotalItems;

                if (!$scope.$root.$$phase) {
                    $scope.$apply();
                }
                $scope.ShowCollpase();
            }
            ShowMessages(response);
            $scope.AjaxStart = false;
        });


    };

    $scope.ReferralListPager.getDataCallback = $scope.GetReferralList;
    if (!IsAttandancePage) {
        $scope.ReferralListPager.getDataCallback(true);
    }

    $scope.SearchReferral = function () {
        $scope.ReferralListPager.currentPage = 1;
        $scope.ReferralListPager.pageSize = $scope.TempSearchReferralModel.PageSize;
        $scope.ReferralListPager.getDataCallback(true);
        return true;
    };

    $scope.Refresh = function () {
        $scope.ReferralListPager.getDataCallback();
    };

    $scope.ViewReferralList = function () {
        $scope.ShowReferralList = !$scope.ShowReferralList;
        $timeout(function () {
            $scope.wall.fitWidth();
        });
    };
    $scope.ViewCalenders = function () {
        $scope.ShowCalenders = !$scope.ShowCalenders;

        $timeout(function () {
            $scope.wall.fitWidth();
            if (!$scope.ShowCalenders) {
                $scope.ResetResizable();
            } else {
                $scope.Resizable();
            }
        });
    };

    $scope.ResetArrangment = function () {
        $timeout(function () {
            $scope.ResetResizable();
            //$(".calender > .col-lg-12.margin-top-40-print").resizable('destroy');
            $scope.wall.fitWidth();
            $scope.Resizable();
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

    $scope.$watch(function () {
        return $scope.ScheduleSearchModel.RegionID + $scope.ScheduleSearchModel.WeekMasterID;
    }, function () {
        $scope.IsContentChanged = false;
        if ($scope.ScheduleSearchModel.RegionID && $scope.ScheduleSearchModel.WeekMasterID) {
            $scope.GetRegionWiseWeekFacility($scope.ScheduleSearchModel.RegionID, $scope.ScheduleSearchModel.WeekMasterID);
        }
    });

    $scope.IsContentChanged = false;
    $scope.$watch(function () {
        return $scope.ScheduleSearchModel.FacilityIds;
    }, function () {
        if ($scope.ScheduleSearchModel.FacilityIds) {
            $scope.IsContentChanged = true;
        }
    });


    $scope.GetRegionWiseWeekFacility = function (regionId, weekMasterId) {
        AngularAjaxCall($http, SiteUrl.GetRegionWiseWeekFacilityURL, { regionID: regionId, weekMasterID: weekMasterId }, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.ScheduleSearchModel.FacilityIds = response.Data;
            } else {
                $scope.ScheduleSearchModel.FacilityIds = [];
                $.each($scope.FacilityList, function (index, data) {
                    $scope.ScheduleSearchModel.FacilityIds.push(data.FacilityID.toString());
                });
            }
        });


    };

    $scope.ClientCountAtLocations = {};

    $scope.TokenInputObj = {};

    $scope.LoadMoreReferral = function () {
        $scope.ReferralListPager.nextPage();
    }


    $scope.EmployeeList = $scope.NewInstance().EmployeeList;
    $scope.EmployeeListFilter = function (value) {
        return function (item) {
            if (item.IsDeleted == 0 || item.EmployeeID == value) {
                return item;
            }
        };
    };


    $scope.EmployeeReources = function (refreshCalender, clenderEle) {

        var referralId = $scope.ScheduleSearchModel.ReferralID;
        var employeeName = $scope.ScheduleSearchModel.EmployeeName;

        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeesForSchedulingURL, { referralID: referralId, employeeName: employeeName }, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {


                $scope.EmployeeList = response.Data;
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
    $scope.ReferralScheduleClick = function (item) {

        if ($scope.StopReferralClick === true) {
            $scope.StopReferralClick = false;
            return false;
        }
        $scope.ScheduleSearchModel.EmployeeName = null;
        $scope.ScheduleSearchModel.ReferralID = item.ReferralID;
        $scope.ScheduleSearchModel.ReferralName = item.Name;
        $scope.EmployeeReources();
    }
    $scope.RemoveReferral = function () {
        $scope.ScheduleSearchModel.ReferralID = 0;
        $scope.ScheduleSearchModel.ReferralName = "";
        $scope.EmployeeReources();
    }



    //referral.ReferralID == ScheduleSearchModel.ReferralID
    $scope.SelectedRefList = [];

    $scope.OnReferralSelection = function (isChecked,referralId) {
        
        

        if (isChecked) {
            if ($scope.SelectedRefList.indexOf(referralId) === -1)
                $scope.SelectedRefList.push(referralId);
        } else {
                $scope.SelectedRefList.remove(referralId);
        }

        $scope.GenerateCalenders();
    }
    //#endregion For Referral Listing 

    //#region WeekMaster
    $scope.AddNewWeek = function () {
        $scope.WeekMaster = $scope.NewInstance().WeekMaster;
        //$scope.WeekMaster.TransportationGroup.TransportationDate = moment($scope.SearchRefrralForTransportatiAssignment.StartDate).format('YYYY/MM/DD');
        //$scope.WeekMaster.TransportationGroup.FacilityID = null;
        //$scope.WeekMaster.TransportationGroup.LocationID = null;
    };
    $scope.OnCloseModel = function () {
        HideErrors("#frmWeekMaster");
    };
    $scope.CreateWeek = function () {
        var isValid = CheckErrors($("#frmWeekMaster"));
        if (moment($scope.WeekMaster.EndDate).diff(moment($scope.WeekMaster.StartDate), 'days') > 7) {
            ShowMessage(window.ScheduleStartEndDateMustSevenDay, "error");
            return;
        }


        if (isValid) {

            var jsonData = angular.toJson($scope.WeekMaster);
            AngularAjaxCall($http, SiteUrl.GetCreateWeekURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);

                if (response.IsSuccess) {
                    $scope.WeekMaster = $scope.NewInstance().WeekMaster;
                    $scope.WeekMasterList = response.Data;
                }
                $('#weekMasterModel').modal('hide');
            });

        }
    };
    //#endregion WeekMaster

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
    $scope.ScheduleSearchModel = $scope.NewInstance().ScheduleSearchModel;


    $scope.TransportLocation = $scope.NewInstance().TransportLocation;
    $scope.Facilities = $scope.NewInstance().Facilities;

    $scope.LoadFacility = function () {
        if ($scope.ScheduleSearchModel.RegionID == undefined) {
            $scope.FacilityList = [];
            return;
        }
        var jsonData = { regionID: $scope.ScheduleSearchModel.RegionID };// $scope.TempSearchReferralModel.RegioinID };
        AngularAjaxCall($http, SiteUrl.LoadAllFacilityByRegion, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                
                $scope.FacilityList = response.Data;
                $scope.ScheduleSearchModel.FacilityIds = [];
                $.each($scope.FacilityList, function (index, data) {
                    $scope.ScheduleSearchModel.FacilityIds.push(data.FacilityID.toString());
                });
            }
        });
    };


    $scope.SeachAndGenerateCalenders = function () {
        $scope.EmployeeReources();
    };

    $timeout(function () {
        $scope.SeachAndGenerateCalenders();
    }, 1000);


    $scope.GenerateCalenders = function () {
        if (!CheckErrors("#frmScheduleSearch")) {
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
        $scope.Resizable();
       
    };

    $scope.CreateCalanderEventModel = function (data) {
        
        return {
            start: data.StartDate,//moment(data.StartDate),//(data.StartDate),
            end: data.EndDate,//moment(data.EndDate).add(1, 'day'),// data.EndDate,// 
            title: data.Name,
            scheduleModel: data,
            backgroundColor: $scope.colors[data.ScheduleStatusID - 1],
            allDay: false,
            resourceId: data.EmployeeID,
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
    $scope.GetResourcesList = function (calendarObj, callback) {
        if (timeout != null)
            clearTimeout(timeout);
        timeout = setTimeout(function () {
            callback($scope.EmployeeList);
        }, 100);
    };
    $scope.GetScheduleList = function (calendarObj, start, end, callback) {

        var result = $scope.EmployeeList.map(function (a) { return a.EmployeeID; });
        var jsonData = angular.toJson({
            EmployeeIDs: result,
            //ReferralID: $scope.ScheduleSearchModel.ReferralID,
            ReferralIDs: $scope.SelectedRefList.length > 0 ? $scope.SelectedRefList.toString() : "", //$scope.ScheduleSearchModel.ReferralID ? $scope.ScheduleSearchModel.ReferralID.toString() : "",
            StartDate: start,
            EndDate: moment(end).add(-1, 'day')
        });
        calendarObj.IsLoading = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetScheduleListByEmployeesURL, jsonData, "Post", "json", "application/json", $scope.ShowLoadOnCalenderLoad).success(function (response) {
            calendarObj.IsLoading = false;
            $scope.ShowLoadOnCalenderLoad = false;
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
                removeEvent: $scope.RemoveSchedule,
                updateScheduleStatus: $scope.OpenUpdateScheduleStatusPopup,
            };
            html = $compile(content)(newScope);
        }
        //$(ele).find('.fc-content').html(html);
        $(ele).find('.fc-content').html(html);

    };
    $scope.OnReferralDrop = function (calendarObj, dropperData, date, successCallback, resourceId) {

        if (!CheckErrors("#frmScheduleSearch") || isNaN(parseInt(resourceId))) {
            return;
        }
        if (dropperData) {

            var defaultScheduleDays = dropperData.DefaultScheduleDays;
            if ($scope.DefaultScheduleDaysSet)
                defaultScheduleDays = 1;

            if (dropperData.event) { //Update event

                var sdate = moment(dropperData.event.scheduleModel.StartDate);
                var edate = moment(dropperData.event.scheduleModel.EndDate);

                var daysDiff = edate.diff(sdate, 'days');

                dropperData.event.scheduleModel.StartDate = date.format();//date.format($scope.DateFormat);
                dropperData.event.scheduleModel.EndDate = date.clone().add(2, 'hours').format(); //date.clone().add(daysDiff, 'day').format($scope.DateFormat); //dropperData.event.scheduleModel.DefaultScheduleDays - 1
                //dropperData.event.scheduleModel.FacilityID = calendarObj.Facility.FacilityID;
                dropperData.event.scheduleModel.WeekMasterID = null;//$scope.ScheduleSearchModel.WeekMasterID;
                dropperData.event.scheduleModel.EmployeeID = resourceId;
                calendarObj.IsLoading = true;
                //successCallback(e);
                $scope.SaveSchedule(dropperData.event.scheduleModel, true, function (e) {
                    dropperData.calendarObj.reloadEvents();
                    calendarObj.IsLoading = false;
                    successCallback(e);
                }, function () { calendarObj.IsLoading = false; });
            } else {//Create event
                var newSchedule = $scope.NewInstance().ScheduleMaster;
                newSchedule.StartDate = date.format(); //date.format($scope.DateFormat);
                //newSchedule.EndDate = date.clone().add(dropperData.DefaultScheduleDays - 1, 'day').format($scope.DateFormat);
                newSchedule.EndDate = date.clone().add(6, 'hours').format();  //date.clone().add(defaultScheduleDays - 1, 'day').format($scope.DateFormat);
                //newSchedule.FacilityID = calendarObj.Facility.FacilityID;
                newSchedule.WeekMasterID = null;//$scope.ScheduleSearchModel.WeekMasterID;
                newSchedule.EmployeeID = resourceId;
                newSchedule.ReferralID = dropperData.ReferralID;
                //newSchedule.DropOffLocation = dropperData.DropOffLocation;
                //newSchedule.PickUpLocation = dropperData.PickUpLocation;
                calendarObj.IsLoading = true;
                //successCallback(e);
                $scope.SaveSchedule(newSchedule, false, function (e) {
                    calendarObj.IsLoading = false;
                    successCallback(e);
                }, function () { calendarObj.IsLoading = false; });
            }


        }
    };
    $scope.OnEventChange = function (calendarObj, event, delta, successCallback, errorCallback, resourceId) {

        event.scheduleModel.StartDate = event.start.format();//event.start.format($scope.DateFormat);
        event.scheduleModel.EndDate = event.end.format();//event.end.add(-1, 'day').format($scope.DateFormat);
        event.scheduleModel.WeekMasterID = null;//$scope.ScheduleSearchModel.WeekMasterID;
        event.scheduleModel.EmployeeID = event.resourceId;
        //event.scheduleModel.EndDate = moment(event.scheduleModel.EndDate.addDays(-1)).format($scope.DateFormat);

        //Incase client is already assigned to transportation group then ask for 
        if (event.scheduleModel.IsAssignedToTransportationGroupUp || event.scheduleModel.IsAssignedToTransportationGroupDown) {
            bootboxDialogWithCancel(function (result, action) {
                if (result && (action == 1 || action == 2)) {
                    //
                    if (action == 1) {
                        event.scheduleModel.TransportationAssignmentAction = TransportationAssignmentRemoveAction;
                    } else {
                        event.scheduleModel.TransportationAssignmentAction = TransportationAssignmentKeepAction;
                    }


                    calendarObj.IsLoading = true;
                    $scope.SaveSchedule(event.scheduleModel, true, function (e) {
                        calendarObj.IsLoading = false;
                        successCallback(e);
                    }, function () {
                        calendarObj.IsLoading = false;
                        errorCallback();
                    });
                } else {
                    //
                    errorCallback();
                }


            }, TransportationAssignmentConfirmationTitle, TransportationAssignmentConfirmationMessage,
                RemoveAssignedTransportion, btnClass.BtnEnable, KeepTransportion, btnClass.BtnWarning, 'Cancel', 'btn btn-default');

        }
        else {
            //
            calendarObj.IsLoading = true;
            $scope.SaveSchedule(event.scheduleModel, true, function (e) {
                calendarObj.IsLoading = false;
                successCallback(e);
            }, function (newScheduleData) {
                //InCase Server Side Validation Error.
                if (newScheduleData) {
                    event.scheduleModel.IsAssignedToTransportationGroupUp = newScheduleData.IsAssignedToTransportationGroupUp;
                    event.scheduleModel.IsAssignedToTransportationGroupDown = newScheduleData.IsAssignedToTransportationGroupDown;
                    $scope.OnEventChange(calendarObj, event, delta, successCallback, errorCallback);
                }
                calendarObj.IsLoading = false;
                errorCallback();
            });
        }
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
        AngularAjaxCall($http, HomeCareSiteUrl.SaveScheduleMasterFromCalenderURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
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

    $scope.RemoveSchedule = function (calendarObj, event, popOverFunctions) {
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = { scheduleID: event.scheduleModel.ScheduleID };
                AngularAjaxCall($http, HomeCareSiteUrl.RemoveScheduleFromCalenderURL, jsonData, "Post", "json", "application/json").success(function (response) {

                    if (response.IsSuccess) {
                        popOverFunctions.Hide();
                        calendarObj.reloadEvents();
                        $scope.ReferralListPager.getDataCallback(true);
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteConfirmationMessageForSchedule, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);

    };
    $scope.RemoveSchedulesFromWeekFacility = function (calendarObj, popOverFunctions) {

        bootboxDialog(function (result) {
            if (result) {

                var jsonData = { weekMasterID: $scope.ScheduleSearchModel.WeekMasterID, facilityID: calendarObj.Facility.FacilityID };
                AngularAjaxCall($http, SiteUrl.RemoveSchedulesFromWeekFacilityURL, jsonData, "Post", "json", "application/json").success(function (response) {

                    if (response.IsSuccess) {
                        popOverFunctions.Hide();
                        calendarObj.reloadEvents();
                        $scope.ReferralListPager.getDataCallback(true);
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteConfirmationMessageFromFacility, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);

    };




    //$scope.GetEmployeeMatchingPreferencesUrl = HomeCareSiteUrl.GetEmployeeMatchingPreferencesURL;
    $scope.ResourceRender = function (calendarObj, resourceObj, labelTds, bodyTds) {
         
        var content = $("#empReource").html();
        var empResource = $(labelTds[0]).find(".fc-cell-text");
        resourceObj.Url = HomeCareSiteUrl.GetEmployeeMatchingPreferencesURL;
        resourceObj.ID = resourceObj.EmployeeID + '|' + $scope.ScheduleSearchModel.ReferralID;

        resourceObj.EmpRefSchModal = $scope.EmpRefSchModal;

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
    $scope.CalenderRefresh = function () {

        $scope.ShowLoadOnCalenderLoad = true;

        $scope.EmployeeReources(true, ".customCalender");

        //$('.customCalender').fullCalendar('refetchResources');
        //$('.customCalender').fullCalendar('refetchEvents');
        //$('.customCalender').fullCalendar('rerenderEvents');

        //var viewName = $('.customCalender').fullCalendar('getView');
        //$scope.SeachAndGenerateCalenders();
        //$(".customCalender").fullCalendar('changeView', viewName.name);
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


    //#region Other Calls
    $(".windowTable").scroll(function () {
        if (document.getElementById("windowTable").scrollHeight == $(".windowTable").scrollTop() + $(".windowTable").height() + 15 && !$scope.AjaxStart)
            $scope.ReferralListPager.nextPage();

        //if ($(".windowTable").scrollTop() == $(".schAssignment_table").height() - $(".windowTable").height()) {
        //    
        //    $scope.ReferralListPager.nextPage();
        //}
    });


    $scope.CollapseSourceClick = function () {
        $scope.StopReferralClick = true;
    };

    $scope.ShowCollpase = function () {
        setTimeout(function () {
            $.each($('.collapseDestination'), function (index, data) {
                $(this).on('show.bs.collapse', function () {
                    $(this).css('display', '');
                    $(this).parents("tbody").find(".collapseSource").removeClass("fa-plus-circle").addClass("fa-minus-circle");
                });

                $(this).on('hidden.bs.collapse', function () {
                    $(this).css('display', '');
                    $(this).parents("tbody").find(".collapseSource").removeClass("fa-minus-circle").addClass("fa-plus-circle");
                });

            });

        }, 100);
    };

    $scope.ShowCollpase();

    $scope.MinimizeReferralList = function (clickCount) {

        if (clickCount % 2 == 0) {
            $.each($('.collapseDestination'), function (index, data) {
                $(".schRef").hide();
                $(".schRef").removeClass('in');
                $(this).parents("tbody").find(".collapseSource").removeClass("fa-minus-circle").addClass("fa-plus-circle");
                //$(this).collapse('hide');


            });
        } else {
            $.each($('.collapseDestination'), function (index, data) {
                //$(this).collapse('show');
                $(".schRef").show();
                $(".schRef").addClass('in');
                $(this).parents("tbody").find(".collapseSource").removeClass("fa-plus-circle").addClass("fa-minus-circle");

            });
        }

    };

    $scope.wall = new Freewall("#scroll-content");

    $scope.wall.reset({
        selector: '.calender',
        // animate: true,
        cellW: 300,
        cellH: 'auto',
        onResize: function () {
            //   $scope.wall.fitWidth();
        }
    });



    //#endregion








    $scope.EmpRefSchModal = function () {
        
        $("#emprefschmodal").modal('show');
    }


    //#region In Home Related Code Stuffs


    $scope.DateRange = [];
    $scope.DateRangeLength = 0;

    $scope.SetDateRange = function () {
        //
        $scope.DateRange = [];
        var date1 = new Date($scope.ScheduleSearchModel.StartDate);
        var date2 = new Date($scope.ScheduleSearchModel.EndDate);
        var timeDiff = Math.abs(date2.getTime() - date1.getTime());
        var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
        for (var i = 0; i <= diffDays; i++) {
            var d = new Date(date1.getTime() + (i * 24 * 60 * 60 * 1000));
            $scope.DateRange.push(d);
        }

        $scope.DateRangeLength = $scope.DateRange.length;
        //return days;
    };
    $scope.SetDateRange();




    $scope.GenerateStaffCalenders = function () {
        $calendar.fullCalendar('refetchResources');
        $scope.ReloadProgressBar();
    }

    $scope.ReloadProgressBar = function () {
        setTimeout(function () {
            $.each($(".Resource_Employee"), function (key, elem) {

                if ($(elem).find(".prg")) {
                    $(elem).find(".prg").remove();
                }
                var html = "<span class='prg'><sapn>";
                $(elem).find(".fc-cell-text").after(html);
                var selectedItem = $(elem).find(".prg");

                var percent = $(elem).next().find(".fc-cell-text").text();
                var percentMaster = $(elem).next().find(".hrsprg").text();
                percent = (parseFloat(percent) * 100) / parseFloat(percentMaster);


                $(selectedItem).LineProgressbar({
                    percentage: percent,
                    duration: 'fast',
                    fillBackgroundColor: '#1abc9c'
                });
            });
        }, 1000);
    };





    $scope.Sch_CreateCalanderEventModel = function (data, startDate, resourceId) {
        //id: '1', resourceId: '1', start: '2017-12-07T12:00:00', end: '2017-12-03T14:00:00', title: 'Beardslee, Hayden'

        return {
            start: startDate,//(data.StartDate),
            //end: moment(data.EndDate).add(1, 'day'),// data.EndDate,// 
            title: data.Name,
            resourceId: resourceId,
            data: data
        };
    };

    $scope.Sch_UpdateReferral = function (events) {
        if (events && events.length > 0) {
            $scope.ReferralList.filter(function (item) {
                item.NewRemainingHrs = item.UsedRespiteHours;
            });

            if (!$scope.$root.$$phase) {
                $scope.$apply();
            }

            events.filter(function (event) {
                if (event.data) {

                    var startDate = moment(event.start);

                    var endDate;
                    if (event.end == undefined)
                        endDate = moment(event.start).add(2, 'hours');
                    else
                        endDate = moment(event.end);


                    var duration = moment.duration(endDate.diff(startDate));
                    var hours = duration.asHours();

                    $scope.ReferralList.filter(function (item) {
                        if (event.data.ReferralID === item.ReferralID) {
                            item.NewRemainingHrs = item.NewRemainingHrs - hours;

                            if (item.NewRemainingHrs <= 0)
                                item.NewRemainingHrs = 0;
                        }
                    });
                }
            });


            if (!$scope.$root.$$phase) {
                $scope.$apply();
            }

        }
    };

    $scope.Sch_UpdateEmpTime = function (resources) {

        $scope.EmployeeList.filter(function (item) {
            item.CalculatedEmployeeHours = item.EmployeeHours;//"160";
        });

        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }

        resources.filter(function (item) {
            var resourceEvent = $calendar.fullCalendar('getResourceEvents', item.id);
            if (resourceEvent && resourceEvent.length > 0) {

                resourceEvent.filter(function (event) {
                    if (event.data) {

                        var startDate = moment(event.start);

                        var endDate;
                        if (event.end == undefined)
                            endDate = moment(event.start).add(2, 'hours');
                        else
                            endDate = moment(event.end);


                        var duration = moment.duration(endDate.diff(startDate));
                        var hours = duration.asHours();

                        $scope.EmployeeList.filter(function (item2) {
                            if (item.id == item2.EmployeeID) {

                                item2.CalculatedEmployeeHours = parseFloat(item2.CalculatedEmployeeHours) - hours;
                                //
                                if (parseFloat(item2.CalculatedEmployeeHours) <= 0)
                                    item2.CalculatedEmployeeHours = "0";
                            }
                        });
                    }
                });


                if (!$scope.$root.$$phase) {
                    $scope.$apply();
                }
            }


        });




    }



    var timeout = null;
    $scope.GetFilterEmp = function (callback) {
        //
        if (timeout != null)
            clearTimeout(timeout);

        timeout = setTimeout(function () {

            callback($scope.EmployeeList);
        }, 100);
    };





    //#endregion


};

controllers.ScheduleAssignmentController.$inject = ['$scope', '$http', '$compile', '$timeout', '$filter'];

var wall;
$(document).ready(function () {

    setTimeout(function () {

        $(".dateInputMask").attr("placeholder", "mm/dd/yy");

        /* initialize the external events
        -----------------------------------------------------------------*/

        $('#external-events .fc-event').each(function () {

            // store data so the calendar knows to render an event upon drop
            $(this).data('event', {
                title: $.trim($(this).text()), // use the element's text as the event title
                stick: true // maintain when user navigates (see docs on the renderEvent method)
            });

            // make the event draggable using jQuery UI
            $(this).draggable({
                zIndex: 999,
                revert: true,      // will cause the event to go back to its
                revertDuration: 0  //  original position after the drag
            });

        });


        /* initialize the calendar
        -----------------------------------------------------------------*/

        $calendar.fullCalendar({
            schedulerLicenseKey: 'CC-Attribution-NonCommercial-NoDerivatives',
            now: '2017-12-07',
            editable: true, // enable draggable events
            droppable: true, // this allows things to be dropped onto the calendar
            aspectRatio: 1.5,
            //eventOverlap:false,
            minTime: "10:00:00",
            maxTime: "20:00:00",
            scrollTime: '00:00', // undo default 6am scrollTime
            header: {
                left: 'today prev,next',
                center: 'title',
                right: 'timelineDay,timelineThreeDays,timelineTenDays,timelineMonth,timelineYear'
            },
            defaultView: 'timelineDay',




            views: {
                timelineDay: {
                    buttonText: ':15 slots',
                    slotDuration: '00:15'
                },
                timelineThreeDays: {
                    type: 'timeline',
                    slotDuration: '00:15',
                    duration: { days: 3 }
                },
                timelineTenDays: {
                    type: 'timeline',
                    //slotDuration: '00:15',
                    duration: { days: 10 }
                }
            },
            //resourceAreaWidth: '200px',
            //resourceLabelText: 'Employees',
            resourceColumns: [
                    {
                        labelText: 'Employees',
                        field: 'EmployeeName',
                        width: '150px'
                    },
                    {
                        labelText: 'Hrs',
                        field: 'CalculatedEmployeeHours',
                        width: '50px'

                    }
            ],
            //SearchEmpText

            //resources: custModel.NewInstance().EmployeeList,
            resources: custModel.GetFilterEmp,

            resourceRender: function (resourceObj, labelTds, bodyTds) {
                //
                $(labelTds[0]).addClass('Resource_Employee');

                $(labelTds[1]).addClass('Resource_Employee_Hrs');
                $(labelTds[1]).addClass('cursor-pointer');


                var elem = $(labelTds[1]);
                var html = "<span class='hrsprg' style='display:none;'><sapn>";
                $(elem).find(".fc-cell-text").after(html);
                var selectedItem = $(elem).find(".hrsprg");
                $(selectedItem).text(resourceObj.EmployeeHours);


                $(labelTds[1]).on('click', function () {
                    var action = prompt("Reset hours for -" + resourceObj.EmployeeName, resourceObj.EmployeeHours);


                    if (action != null && action !== "") {
                        custModel.EmployeeList.filter(function (item) {
                            if (item.EmployeeID == resourceObj.id) {
                                item.EmployeeHours = parseFloat(action);
                            }
                        });

                        var x = 0;
                        var intervalId = setInterval(function () {
                            custModel.GenerateStaffCalenders();
                            if (++x === 2) {
                                window.clearInterval(intervalId);
                            }
                        }, 600);

                    }
                });









            },

            eventRender: function (event, ele) {
                console.log('eventRender', event);
            },

            drop: function (date, jsEvent, ui, resourceId) {
                console.log('drop', event);
                var dropperData = $(this).data('eventObject');
                var eventObject = custModel.Sch_CreateCalanderEventModel(dropperData, date.format(), resourceId);
                $calendar.fullCalendar('renderEvent', eventObject, true);
            },
            eventReceive: function (event) { // called when a proper external event is dropped
                console.log('eventReceive', event);
            },
            eventDrop: function (event) { // called when an event (already on the calendar) is moved
                console.log('eventDrop', event);
            },
            eventAfterAllRender: function (view) { // called when an event (already on the calendar) is moved

                console.log('eventAfterAllRender', view);
                var events = $calendar.fullCalendar('clientEvents');
                custModel.Sch_UpdateReferral(events);

                var resources = $calendar.fullCalendar('getResources');
                custModel.Sch_UpdateEmpTime(resources);
            },
            viewRender: function (view, element) {
                custModel.ReloadProgressBar();
            }


        });


        $('.filterEmp').keyup(function () {
            $calendar.fullCalendar('refetchResources');
        });

    }, 1000);
});
