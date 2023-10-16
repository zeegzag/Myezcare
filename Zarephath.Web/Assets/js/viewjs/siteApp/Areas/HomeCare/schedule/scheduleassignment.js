﻿var custModel;

controllers.ScheduleAssignmentController = function ($scope, $http, $compile, $timeout,$filter) {
    custModel = $scope;
    $scope.DateFormat = "YYYY/MM/DD";

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
            AngularAjaxCall($http, SiteUrl.UpdateScheduleMasterURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    if (!$scope.$root.$$phase) {
                        $scope.$apply();
                    }
                    $scope.ScheduleDetail.CalenderObj.reloadEvents();
                    $('#EditSchedule').modal('hide');
                    if (scheduleModel.ScheduleStatusID == window.CancelStatus) {
                        bootboxDialog(function (result) {
                            if (result) {
                                $timeout(function () {
                                    $scope.ScheduleDetail.StartDate = scheduleModel.StartDate; //$filter('date')(new Date(), 'L');
                                    $scope.ScheduleDetail.EndDate = scheduleModel.EndDate; //$filter('date')(new Date(), 'L');
                                    $scope.ScheduleDetail.ScheduleStatusID = 0;
                                    $scope.ScheduleDetail.Comments = null;
                                });
                                $('#RescheduleClient').modal('show');
                            }
                        }, bootboxDialogType.Confirm, "Reschedule", scheduleModel.IsReschedule ? window.AlreadyRescheduleConfirm : window.RescheduleConfirm, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
                    }
                }
            });
        }
    };
    $scope.ReSchedule = function (scheduleModel) {
        var isValid = CheckErrors($("#frmReScheduleClient"));
        if (isValid) {
            var jsonData = { scheduleModel: scheduleModel };
            AngularAjaxCall($http, SiteUrl.ReScheduleClientURL, jsonData, "Post", "json", "application/json").success(function (response) {
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
    $scope.ReferralListPager = new PagerModule("ClientName");
    $scope.ShowReferralList = !IsAttandancePage;
    $scope.ShowCalenders = true;

    $scope.TempSearchReferralModel = $scope.NewInstance().SearchReferralModel;
    $scope.SearchReferralModel = $scope.NewInstance().SearchReferralModel;
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
        AngularAjaxCall($http, SiteUrl.GetReferralListForScheduleURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
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
            $(".calender").draggable({ handle: "#schedule-assignment-header" });
            $(".calender").resizable();
            //$(".calender > .col-lg-12.margin-top-40-print").resizable();           
        });
    };

    $scope.ResetResizable = function () {
        $timeout(function () {
            $(".calender").draggable('destroy');
            $(".calender").resizable('destroy');
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

            $scope.GenerateCalenders();

        });
    };

    $scope.ClientCountAtLocations = {};
    $scope.SaveRegionWiseWeekFacility = function (regionId, weekMasterId) {
        AngularAjaxCall($http, SiteUrl.SaveRegionWiseWeekFacilityURL, { regionID: regionId, weekMasterID: weekMasterId, facilites: $scope.ScheduleSearchModel.FacilityIds.join() }, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                var upTripList = response.Data.UpTripList;
                var downTripList = response.Data.DownTripList;

                $scope.ClientCountAtLocations.HeadCellsUPTrip = _.keys(_.groupBy(upTripList, function (upTripList) { return upTripList.SchStartDate }));
                var itemsSorted1 = $filter('orderBy')(upTripList, 'Location');
                $scope.ClientCountAtLocations.rowsUPTrip = _.groupBy(itemsSorted1, function (itemsSorted1) { return itemsSorted1.Location });
                $scope.ClientCountAtLocations.sortByStartDateProp = function (values, headerDate) {
                    return _.filter(values, function (value) {
                        return value.SchStartDate === headerDate;
                    });
                }
                //$scope.ClientCountAtLocations.sortByStartDateProp = function (values) {
                //    return _.sortBy(values, function (value) {
                //        return value.SchStartDate;
                //    });
                //}


                $scope.ClientCountAtLocations.HeadCellsDownTrip = _.keys(_.groupBy(downTripList, function (downTripList) { return downTripList.SchEndDate }));
                var itemsSorted2 =  $filter('orderBy')(downTripList, 'Location');
                $scope.ClientCountAtLocations.rowsDownTrip = _.groupBy(itemsSorted2, function (itemsSorted2) { return itemsSorted2.Location });
                $scope.ClientCountAtLocations.sortByEndDateProp = function (values, headerDate) {
                    return _.filter(values, function (value) {
                        return value.SchEndDate === headerDate;
                    });
                }

                //$scope.ClientCountAtLocations.sortByEndDateProp = function (values, headerDate) {
                //    
                //    return _.sortBy(values, function (value) {
                //        return value.SchEndDate;
                //    });
                //}

                //$scope.ScheduleSearchModel.FacilityIds = response.Data;
            }
        });
    };

    $scope.TokenInputObj = {};


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

    $scope.SelectedWeekObj = {};

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

    $scope.GenerateCalenders = function () {
        if (!CheckErrors("#frmScheduleSearch")) {
            return;
        }
        $scope.CalendarList = [];
        var selectedWeek = $.grep($scope.WeekMasterList, function (d, i) {
            return $scope.ScheduleSearchModel.WeekMasterID == d.WeekMasterID.toString();
        });
        if (selectedWeek.length > 0) {
            $scope.SelectedWeekObj = selectedWeek[0];
        }
        $.each($scope.FacilityList, function (index, data) {
            var array = $.grep($scope.ScheduleSearchModel.FacilityIds, function (d, i) {
                return data.FacilityID.toString() == d;
            });
            if (array.length > 0) {
                $scope.CalendarList.push({
                    IsLoading: false,
                    Facility: data,
                    ScheduleList: [],
                    RemoveSchedulesFromWeekFacility: $scope.RemoveSchedulesFromWeekFacility,
                    SortBy: $scope.CalenderSortOption.Age,
                    SortIndex: $scope.CalenderSortIndex.Asc
                });
            }
        });
        $scope.SaveRegionWiseWeekFacility($scope.ScheduleSearchModel.RegionID, $scope.ScheduleSearchModel.WeekMasterID);
        $scope.Resizable();
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

    $scope.CreateCalanderEventModel = function (data) {
        return {
            start: moment(data.StartDate),//(data.StartDate),
            end: moment(data.EndDate).add(1, 'day'),// data.EndDate,// 
            title: data.Name,
            scheduleModel: data,
            backgroundColor: $scope.colors[data.ScheduleStatusID - 1],
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

    $scope.GetScheduleList = function (calendarObj, start, end, callback) {
        
        var jsonData = angular.toJson({
            FacilityID: calendarObj.Facility.FacilityID,
            WeekMasterID: $scope.ScheduleSearchModel.WeekMasterID
            //StartDate: start,
            //EndDate: moment(end).add(-1, 'day')
        });
        calendarObj.IsLoading = true;
        AngularAjaxCall($http, SiteUrl.GetScheduleListByFacilityURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            calendarObj.IsLoading = false;
            if (response.IsSuccess) {
                calendarObj.ScheduleList = $scope.SetCalenderData(response.Data.ScheduleList);
                calendarObj.Facility = response.Data.Facility;
                calendarObj.DateWiseScheduleCountList = response.Data.DateWiseScheduleCountList;
                callback(calendarObj.ScheduleList);
            }
            ShowMessages(response);

        });
    };


    $scope.DefaultScheduleDaysSet = false;
    $scope.OnReferralDrop = function (calendarObj, dropperData, date, successCallback) {
        if (!CheckErrors("#frmScheduleSearch")) {
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

                dropperData.event.scheduleModel.StartDate = date.format($scope.DateFormat);
                dropperData.event.scheduleModel.EndDate = date.clone().add(daysDiff, 'day').format($scope.DateFormat); //dropperData.event.scheduleModel.DefaultScheduleDays - 1
                dropperData.event.scheduleModel.FacilityID = calendarObj.Facility.FacilityID;
                dropperData.event.scheduleModel.WeekMasterID = $scope.ScheduleSearchModel.WeekMasterID;
                calendarObj.IsLoading = true;
                $scope.SaveSchedule(dropperData.event.scheduleModel, true, function (e) {
                    dropperData.calendarObj.reloadEvents();
                    calendarObj.IsLoading = false;
                    successCallback(e);
                }, function () { calendarObj.IsLoading = false; });
            } else {//Create event
                var newSchedule = $scope.NewInstance().ScheduleMaster;
                newSchedule.StartDate = date.format($scope.DateFormat);
                //newSchedule.EndDate = date.clone().add(dropperData.DefaultScheduleDays - 1, 'day').format($scope.DateFormat);
                newSchedule.EndDate = date.clone().add(defaultScheduleDays - 1, 'day').format($scope.DateFormat);
                newSchedule.FacilityID = calendarObj.Facility.FacilityID;
                newSchedule.WeekMasterID = $scope.ScheduleSearchModel.WeekMasterID;
                newSchedule.ReferralID = dropperData.ReferralID;
                newSchedule.DropOffLocation = dropperData.DropOffLocation;
                newSchedule.PickUpLocation = dropperData.PickUpLocation;
                calendarObj.IsLoading = true;

                $scope.SaveSchedule(newSchedule, false, function (e) {
                    calendarObj.IsLoading = false;
                    successCallback(e);
                }, function () { calendarObj.IsLoading = false; });
            }


        }
    };

    $scope.OnEventChange = function (calendarObj, event, delta, successCallback, errorCallback) {
        event.scheduleModel.StartDate = event.start.format($scope.DateFormat);
        event.scheduleModel.EndDate = event.end.add(-1, 'day').format($scope.DateFormat);
        event.scheduleModel.WeekMasterID = $scope.ScheduleSearchModel.WeekMasterID;
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
                    $scope.SaveSchedule(event.scheduleModel, true, function(e) {
                        calendarObj.IsLoading = false;
                        successCallback(e);
                    }, function() {
                        calendarObj.IsLoading = false;
                        errorCallback();
                    });
                } else {
                    //
                    errorCallback();
                } 


            },TransportationAssignmentConfirmationTitle, TransportationAssignmentConfirmationMessage,
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
        if (moment(newSchedule.StartDate) < moment($scope.SelectedWeekObj.StartDate) || moment(newSchedule.EndDate) > moment($scope.SelectedWeekObj.EndDate)) {
            ShowMessage(window.ScheduleStartEndDateBetweenWeekStartEndDate, "error");
            if (errorCallback)
                errorCallback();
            return;
        }

        var jsonData = angular.toJson({
            scheduleMaster: {
                ScheduleMaster: newSchedule
            }
        });
        AngularAjaxCall($http, SiteUrl.SaveScheduleMasterFromCalenderURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
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
        $(ele).find('.fc-content').html(html);
    };

    $scope.RemoveSchedule = function (calendarObj, event, popOverFunctions) {
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = { scheduleID: event.scheduleModel.ScheduleID };
                AngularAjaxCall($http, SiteUrl.RemoveScheduleFromCalenderURL, jsonData, "Post", "json", "application/json").success(function (response) {

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

    //#region other
    $(".windowTable").scroll(function () {
        if (document.getElementById("windowTable").scrollHeight == $(".windowTable").scrollTop() + $(".windowTable").height() + 15 && !$scope.AjaxStart)
            $scope.ReferralListPager.nextPage();

        //if ($(".windowTable").scrollTop() == $(".schAssignment_table").height() - $(".windowTable").height()) {
        //    
        //    $scope.ReferralListPager.nextPage();
        //}
    });

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
};

controllers.ScheduleAssignmentController.$inject = ['$scope', '$http', '$compile', '$timeout', '$filter'];

var wall;
$(document).ready(function () {
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});
