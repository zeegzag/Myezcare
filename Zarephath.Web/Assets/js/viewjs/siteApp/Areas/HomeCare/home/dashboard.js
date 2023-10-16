var vm;
controllers.DashboardController = function ($scope, $http) {
    vm = $scope;
    

    $scope.newInstance = function () {
        return $.parseJSON($("#hdn_DashboardModel").val());
    };

    $scope.SelectedScheduleIds = [];
    $scope.SelectedEmployeeIds = [];
    $scope.SelectedEmployeesName = [];

    //#region Referral Internal Message List
    //$scope.EmpClockInOutListEndDate = new Date();

    $scope.ReferralInternalMessageListPager = new PagerModule("ClientName", "#ReferralInternalMessageList");
    $scope.ReferralInternalMessageListPager.pageSize = 10;
    $scope.GetReferralInternamMessageList = function (isSearchFilter) {
        $scope.ReferralInternalMessageListPager.currentPage = isSearchFilter ? 1 : $scope.ReferralInternalMessageListPager.currentPage;
        var pagermodel = {
            pageSize: $scope.ReferralInternalMessageListPager.pageSize,
            pageIndex: $scope.ReferralInternalMessageListPager.currentPage,
            sortIndex: $scope.ReferralInternalMessageListPager.sortIndex,
            sortDirection: $scope.ReferralInternalMessageListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.ReferralInternalMessageAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.ReferralInternalMessageList, jsonData, "Post", "json", "application/json", false).success(function (response) {
            $scope.ReferralInternalMessageList = response.Data.ReferralInternalMessageModel.Items;
            $scope.ReferralInternalMessageListPager.currentPageSize = response.Data.ReferralInternalMessageModel.Items.length;
            $scope.ReferralInternalMessageListPager.totalRecords = response.Data.ReferralInternalMessageModel.TotalItems;
            $scope.ReferralInternalMessageAjaxStart = false;



        });
    };
    $scope.ReferralInternalMessageListPager.getDataCallback = $scope.GetReferralInternamMessageList;
    $scope.ReferralInternalMessageListPager.getDataCallback(true);

    //#region IsResolve 
    $scope.ResolveNote = function (encryptedReferralInternalMessageId, referralId) {

        bootboxDialog(function (result, data) {
            if (result != null) {
                var jsonData = angular.toJson({ EncryptedReferralInternalMessageID: encryptedReferralInternalMessageId, ReferralID: referralId, ResolvedComment: result });
                AngularAjaxCall($http, HomeCareSiteUrl.ResolveReferralInternalMessageURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.ReferralInternalMessageListPager.getDataCallback(false);
                        $scope.ReferralResolvedInternalMessageListPager.getDataCallback(false);
                    }
                });
            }
        }, bootboxDialogType.Prompt, window.Reply, "", bootboxDialogButtonText.Save, btnClass.BtnPrimary);
    };


    //#endregion

    //#endregion

    //#region Referral Resolved Internal Message List
    $scope.ReferralResolvedInternalMessageListPager = new PagerModule("ClientName", "#ReferralResolvedInternalMessageList");
    $scope.ReferralResolvedInternalMessageListPager.pageSize = 10;
    $scope.GetReferralResolvedInternamMessageList = function (isSearchFilter) {
        $scope.ReferralResolvedInternalMessageListPager.currentPage = isSearchFilter ? 1 : $scope.ReferralResolvedInternalMessageListPager.currentPage;
        var pagermodel = {
            pageSize: $scope.ReferralResolvedInternalMessageListPager.pageSize,
            pageIndex: $scope.ReferralResolvedInternalMessageListPager.currentPage,
            sortIndex: $scope.ReferralResolvedInternalMessageListPager.sortIndex,
            sortDirection: $scope.ReferralResolvedInternalMessageListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.ReferralResolvedInternalMessageAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralResolvedInternalMessageURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            $scope.ReferralResolvedInternalMessageList = response.Data.ReferralResolvedInternalMessageListModel.Items;
            $scope.ReferralResolvedInternalMessageListPager.currentPageSize = response.Data.ReferralResolvedInternalMessageListModel.Items.length;
            $scope.ReferralResolvedInternalMessageListPager.totalRecords = response.Data.ReferralResolvedInternalMessageListModel.TotalItems;
            $scope.ReferralResolvedInternalMessageAjaxStart = false;
        });
    };
    $scope.ReferralResolvedInternalMessageListPager.getDataCallback = $scope.GetReferralResolvedInternamMessageList;
    $scope.ReferralResolvedInternalMessageListPager.getDataCallback(true);


    $scope.MarkResolvedMessageAsRead = function (encryptedReferralInternalMessageId, referralId) {
        var jsonData = angular.toJson({
            EncryptedReferralInternalMessageID: encryptedReferralInternalMessageId,
            ReferralID: referralId,
            pageSize: $scope.ReferralResolvedInternalMessageListPager.pageSize,
            pageIndex: $scope.ReferralResolvedInternalMessageListPager.currentPage,
            sortIndex: $scope.ReferralResolvedInternalMessageListPager.sortIndex,
            sortDirection: $scope.ReferralResolvedInternalMessageListPager.sortDirection
        });
        AngularAjaxCall($http, HomeCareSiteUrl.MarkResolvedMessageAsReadURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.ReferralResolvedInternalMessageListPager.getDataCallback(false);
                //$scope.ReferralResolvedInternalMessageList = response.Data.ReferralResolvedInternalMessageListModel.Items;
                //$scope.ReferralResolvedInternalMessageListPager.currentPageSize = response.Data.ReferralResolvedInternalMessageListModel.Items.length;
                //$scope.ReferralResolvedInternalMessageListPager.totalRecords = response.Data.ReferralResolvedInternalMessageListModel.TotalItems;
            }
        });
    };


    $scope.ShowResolvedComment = function (message) {
        bootboxDialog(function (result, data) {
        }, bootboxDialogType.Alert, window.ResolvedComment, message);
    };

    //#endregion

    //#region Employee Clock In - Clock Out List
    $scope.EmpClockInOutListSearchModel = $scope.newInstance().EmpClockInOutListSearchModel;
    $scope.EmpClockInOutListEndDate = $scope.EmpClockInOutListSearchModel.EndDate;
    $scope.EmpClockInOutListTimeSlots = $scope.EmpClockInOutListSearchModel.TimeSlots;
    $scope.EmpClockInOutListPager = new PagerModule("ScheduleStartTime", "#EmpClockInOutList", 'DESC');
    $scope.EmpClockInOutListPager.pageSize = 10;

    $scope.GetEmpClockInOutList = function (isSearchFilter) {
        if ($scope.EmpClockInOutListSearchModel.StartDate > $scope.EmpClockInOutListSearchModel.EndDate) {
            toastr.error("End Date should be greater than Start Date");
            return false;
        }
        $scope.EmpClockInOutListPager.currentPage = isSearchFilter ? 1 : $scope.EmpClockInOutListPager.currentPage;
        var regionId = $scope.EmpClockInOutListSearchModel.RegionID == undefined ? undefined : $scope.EmpClockInOutListSearchModel.RegionID.toString();
        var timeSlots = $scope.EmpClockInOutListSearchModel.TimeSlots ? $scope.EmpClockInOutListSearchModel.TimeSlots.toString() : '';
        var pagermodel = {
            startDate: $scope.EmpClockInOutListSearchModel.StartDate,
            endDate: $scope.EmpClockInOutListSearchModel.EndDate,
            employeeName: $scope.EmpClockInOutListSearchModel.EmployeeName,
            careTypeID: $scope.EmpClockInOutListSearchModel.DDMasterID,
            status: $scope.EmpClockInOutListSearchModel.Status,
            pageSize: $scope.EmpClockInOutListPager.pageSize,
            pageIndex: $scope.EmpClockInOutListPager.currentPage,
            sortIndex: $scope.EmpClockInOutListPager.sortIndex,
            sortDirection: $scope.EmpClockInOutListPager.sortDirection,
            TimeSlots: timeSlots,
            RegionID: regionId
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.EmpClockInOutListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmpClockInOutListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.EmpClockInOutList = response.Data.Items;
                $scope.EmpClockInOutListPager.currentPageSize = response.Data.Items.length;
                $scope.EmpClockInOutListPager.totalRecords = response.Data.TotalItems;
                if ($scope.EmpClockInOutList.length > 0) {
                    $scope.TotalSchedule = $scope.EmpClockInOutList[0].TotalSchedule;
                    $scope.Inprogress = $scope.EmpClockInOutList[0].Inprogress;
                    $scope.ClocOutnDone = $scope.EmpClockInOutList[0].ClocOutnDone;
                    $scope.MissedSchedule = $scope.EmpClockInOutList[0].MissedSchedule;
                    $scope.TotalComplete = $scope.EmpClockInOutList[0].TotalComplete;
                }

                if (response.Data.TotalItems > 0) {
                    $scope.EmpClockInOutListPager.inprogress = $scope.EmpClockInOutList[0].Inprogress;
                    $scope.EmpClockInOutListPager.clocOutnDone = $scope.EmpClockInOutList[0].ClocOutnDone;
                    $scope.EmpClockInOutListPager.missedSchedule = $scope.EmpClockInOutList[0].MissedSchedule;
                    $scope.EmpClockInOutListPager.totalComplete = $scope.EmpClockInOutList[0].TotalComplete;
                    $scope.TotalSchedule = $scope.EmpClockInOutList[0].TotalSchedule;
                    $scope.Inprogress = $scope.EmpClockInOutList[0].Inprogress;
                    $scope.ClocOutnDone = $scope.EmpClockInOutList[0].ClocOutnDone;
                    $scope.MissedSchedule = $scope.EmpClockInOutList[0].MissedSchedule;
                    $scope.TotalComplete = $scope.EmpClockInOutList[0].TotalComplete;
                }
                else {
                    $scope.TotalSchedule = 0;
                    $scope.Inprogress = 0;
                    $scope.ClocOutnDone = 0;
                    $scope.MissedSchedule = 0;
                    $scope.TotalComplete = 0;
                }
            }
            $scope.EmpClockInOutListAjaxStart = false;
        });
    };

    $scope.SearchEmpClockInOutList = function () {
        $scope.EmpClockInOutListPager.currentPage = 1;
        $scope.EmpClockInOutListPager.getDataCallback(true);
        $scope.GetPatientAddress();
    }

    $scope.EmpClockInOutListPager.getDataCallback = $scope.GetEmpClockInOutList;
    $scope.EmpClockInOutListPager.getDataCallback(true);


    $scope.ResetEmpClockInOutList = function () {
        $scope.EmpClockInOutListSearchModel.StartDate = $scope.PatientNewListSearchModel.StartDate,
            $scope.EmpClockInOutListSearchModel.EndDate = $scope.PatientNewListSearchModel.EndDate,
            $scope.EmpClockInOutListSearchModel.EmployeeName = '',
            $scope.EmpClockInOutListSearchModel.DDMasterID = '',
            $scope.EmpClockInOutListSearchModel.Status = '',
            $scope.SelectedScheduleIds = 0,
            $scope.SelectAllCheckbox = false;
        $scope.GetEmpClockInOutList();
    }
    //#endregion

    //#region Employee Over Time List

    $scope.EmpOverTimeListSearchModel = $scope.newInstance().EmpOverTimeListSearchModel;
    $scope.EmpOverTimeListPager = new PagerModule("WeeklyOverTimeHours", "#EmpOverTimeList", 'DESC');
    $scope.EmpOverTimeListPager.pageSize = 10;
    //$scope.GetEmpClockInOutList = function (isSearchFilter) {
    $scope.GetEmpOverTimeList = function (isSearchFilter) {
        if ($scope.EmpOverTimeListSearchModel.StartDate > $scope.EmpOverTimeListSearchModel.EndDate) {
            toastr.error("End Date should be greater than start date");
            return false;
        }
        $scope.EmpOverTimeListPager.currentPage = isSearchFilter ? 1 : $scope.EmpOverTimeListPager.currentPage;
        var pagermodel = {
            startDate: $scope.EmpOverTimeListSearchModel.StartDate,
            endDate: $scope.EmpOverTimeListSearchModel.EndDate,
            pageSize: $scope.EmpOverTimeListPager.pageSize,
            pageIndex: $scope.EmpOverTimeListPager.currentPage,
            sortIndex: $scope.EmpOverTimeListPager.sortIndex,
            sortDirection: $scope.EmpOverTimeListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.EmpOverTimeListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmpOverTimeListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.EmpOverTimeList = response.Data.Items;
                $scope.EmpOverTimeListPager.currentPageSize = response.Data.Items.length;
                $scope.EmpOverTimeListPager.totalRecords = response.Data.TotalItems;
            }
            $scope.EmpOverTimeListAjaxStart = false;
        });
    };

    $scope.SearchEmpOverTimeList = function () {
        $scope.EmpOverTimeListPager.currentPage = 1;
        $scope.EmpOverTimeListPager.getDataCallback(true);
    }

    //$scope.EmpOverTimeListPager.getDataCallback = $scope.GetEmpClockInOutList;
    $scope.EmpOverTimeListPager.getDataCallback = $scope.GetEmpOverTimeList;
    $scope.EmpOverTimeListPager.getDataCallback(true);


    //#endregion

    //#region Patient New
    $scope.PatientNewListSearchModel = $scope.newInstance().PatientNewListSearchModel;
    $scope.PatientNewListPager = new PagerModule("Patient", "#PatientNewList", 'ASC');
    $scope.PatientNewListPager.pageSize = 10;
    $scope.GetPatientNewList = function (isSearchFilter) {
        if ($scope.PatientNewListSearchModel.StartDate > $scope.PatientNewListSearchModel.EndDate) {
            toastr.error("End Date should be greater than Start Date");
            return false;
        }

        $scope.PatientNewListPager.currentPage = isSearchFilter ? 1 : $scope.PatientNewListPager.currentPage;
        var pagermodel = {
            startDate: $scope.PatientNewListSearchModel.StartDate,
            endDate: $scope.PatientNewListSearchModel.EndDate,
            pageSize: $scope.PatientNewListPager.pageSize,
            pageIndex: $scope.PatientNewListPager.currentPage,
            sortIndex: $scope.PatientNewListPager.sortIndex,
            sortDirection: $scope.PatientNewListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.PatientNewListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetPatientNewListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.PatientNewList = response.Data.Items;
                $scope.PatientNewListPager.currentPageSize = response.Data.Items.length;
                $scope.PatientNewListPager.totalRecords = response.Data.TotalItems;
            }
            $scope.PatientNewListAjaxStart = false;
        });
    };
    $scope.PatientNewListPager.getDataCallback = $scope.GetPatientNewList;
    $scope.PatientNewListPager.getDataCallback(true);

    $scope.SearchPatientNewList = function () {
        $scope.PatientNewListPager.currentPage = 1;
        $scope.PatientNewListPager.getDataCallback(true);
    }
    //#endregion
    //#region Get Active Patient Count List
    $scope.ActivePatientCountList = [];
    $scope.ActivePatientCountSearchModel = $scope.newInstance().ActivePatientCountSearchModel;
    $scope.ActivePatientListPager = new PagerModule("Patient", "#ActivePatientCountList", 'ASC');
    $scope.GetActivePatientCountList = function (isSearchFilter) {
        $scope.ActivePatientListPager.currentPage = isSearchFilter ? 1 : $scope.ActivePatientListPager.currentPage;
        var jsonData = angular.toJson();
        $scope.ActivePatientCountListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetActivePatientCountListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.ActivePatientCountList = response.Data.Items;
                $scope.ActivePatientListPager.currentPageSize = response.Data.Items.length;
                $scope.ActivePatientListPager.totalRecords = response.Data.TotalItems;
            }
            $scope.ActivePatientCountListAjaxStart = false;
        });
    };
    $scope.ActivePatientListPager.getDataCallback = $scope.GetActivePatientCountList;
    $scope.ActivePatientListPager.getDataCallback(true);

    //#endregion
    //#region Patient Not scheduled

    $scope.PatientNotScheduleListSearchModel = $scope.newInstance().PatientNotScheduleListSearchModel;
    $scope.PatientNotScheduleListPager = new PagerModule("WeeklyOverTimeHours", "#EmpOverTimeList");
    $scope.PatientNotScheduleListPager.pageSize = 10;
    $scope.GetPatientNotScheduleList = function (isSearchFilter) {
        $scope.PatientNotScheduleListPager.currentPage = isSearchFilter ? 1 : $scope.PatientNotScheduleListPager.currentPage;
        var pagermodel = {
            startDate: $scope.PatientNotScheduleListSearchModel.StartDate,
            endDate: $scope.PatientNotScheduleListSearchModel.EndDate,
            pageSize: $scope.PatientNotScheduleListPager.pageSize,
            pageIndex: $scope.PatientNotScheduleListPager.currentPage,
            sortIndex: $scope.PatientNotScheduleListPager.sortIndex,
            sortDirection: $scope.PatientNotScheduleListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.PatientNotScheduleListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetPatientNotScheduleListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.PatientNotScheduleList = response.Data.Items;
                $scope.PatientNotScheduleListPager.currentPageSize = response.Data.Items.length;
                $scope.PatientNotScheduleListPager.totalRecords = response.Data.TotalItems;
            }
            $scope.PatientNotScheduleListAjaxStart = false;
        });
    };

    $scope.SearchPatientNotScheduleList = function () {
        $scope.PatientNotScheduleListPager.currentPage = 1;
        $scope.PatientNotScheduleListPager.getDataCallback(true);
    }

    $scope.PatientNotScheduleListPager.getDataCallback = $scope.GetPatientNotScheduleList;
    $scope.PatientNotScheduleListPager.getDataCallback(true);


    //#endregion

    //#region Employee Time Statics


    //$scope.EmployeeTimeStaticSearchModel = $scope.newInstance().EmployeeTimeStaticSearchModel;
    //var myChart = {};
    //$scope.GetEmployeeTimeStatics = function (isSearchFilter) {
    //    if ($scope.EmployeeTimeStaticSearchModel.StartDate > $scope.EmployeeTimeStaticSearchModel.EndDate) {
    //        toastr.success("End Date should be greater than start date");
    //        return false;
    //    }
    //    var jsonData = angular.toJson({ startDate: $scope.EmployeeTimeStaticSearchModel.StartDate, endDate: $scope.EmployeeTimeStaticSearchModel.EndDate });
    //    $scope.EmployeeTimeStaticsAjaxStart = true;
    //    AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeTimeStaticsUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {

    //        if (response.IsSuccess) {
    //            $scope.EmployeeList = response.Data.EmployeeList;
    //            $scope.AvgDelayList = response.Data.AvgDelayList;
    //            $scope.ColorList = response.Data.ColorList;

    //            $scope.GrapHeight = $scope.EmployeeList.length * 10 < 100 ? 100 : $scope.EmployeeList.length * 10;


    //            setTimeout(function () {


    //                if (myChart.destroy)
    //                    myChart.destroy();

    //                var ctx = document.getElementById("myChart").getContext('2d');
    //                myChart = new Chart(ctx, {
    //                    //type: 'bar',
    //                    type: 'horizontalBar',
    //                    data: {
    //                        labels: $scope.EmployeeList,
    //                        position: 'bottom',
    //                        datasets: [{
    //                            label: "Employees Avg Delay in Hrs",
    //                            data: $scope.AvgDelayList,
    //                            backgroundColor: $scope.ColorList
    //                            //[28, 48, 40, 19, 86, 27, 90]
    //                            ,
    //                            //backgroundColor: [
    //                            //    'red',
    //                            //    'rgba(54, 162, 235, 0.2)',
    //                            //    'rgba(255, 206, 86, 0.2)',
    //                            //    'rgba(75, 192, 192, 0.2)',
    //                            //    'rgba(153, 102, 255, 0.2)',
    //                            //    'rgba(255, 159, 64, 0.2)'
    //                            //],
    //                            //borderColor: [
    //                            //    'rgba(255,99,132,1)',
    //                            //    'rgba(54, 162, 235, 1)',
    //                            //    'rgba(255, 206, 86, 1)',
    //                            //    'rgba(75, 192, 192, 1)',
    //                            //    'rgba(153, 102, 255, 1)',
    //                            //    'rgba(255, 159, 64, 1)'
    //                            //],
    //                            borderWidth: 1
    //                        }]
    //                    },
    //                    options: {
    //                        elements: {
    //                            rectangle: {
    //                                borderWidth: 2,
    //                            }
    //                        },
    //                        responsive: true,
    //                        legend: {
    //                            position: 'bottom',
    //                        },
    //                        //title: {
    //                        //    display: true,
    //                        //    text: 'Employees AVG Delay Chart'
    //                        //}

    //                    }
    //                });

    //                if ($scope.GrapHeight !== 100) {
    //                    $(".chartAreaWrapper ").css("height", "400px");
    //                    $(".chartAreaWrapper ").css("overflow", "auto");
    //                }
    //            }, 500);
    //            setTimeout(() => {
    //                var ht = $(".dashboard-calander").outerHeight();
    //                $("#WebNotificationList .wrap").css("height", (ht - 109) + "px");
    //            }, 1000);
    //        }
    //        $scope.EmployeeTimeStaticsAjaxStart = false;
    //    });
    //};
    //$scope.GetEmployeeTimeStatics();

    //#endregion

    //#region Web Notifications

    $scope.GetWebNotificationList = function () {
        var jsonData = angular.toJson({});
        $scope.WebNotificationListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetWebNotificationListURL, jsonData, "Get", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                var oldCount = -1;
                if ($scope.WebNotificationList) {
                    oldCount = $scope.WebNotificationList.length;
                }
                $scope.WebNotificationList = response.Data;
                var newCount = $scope.WebNotificationList.length;
                if (oldCount != -1 && oldCount < newCount) {
                    ShowMessage(NewNotificationsCountMessage.replace('{0}', (newCount - oldCount).toString()), 'info');
                }
            }
            $scope.WebNotificationListAjaxStart = false;
        });
    };

    $scope.DeleteWebNotification = function (id) {
        var jsonData = angular.toJson({});
        AngularAjaxCall($http, HomeCareSiteUrl.DeleteWebNotificationURL + id, jsonData, "Get", "json", "application/json", false).success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.GetWebNotificationList();
            }
        });
    };

    $scope.MarkSelectedAsRead = function () {
        var selectedItems = $scope.WebNotificationList.filter(item => item.IsSelected);
        var selectedIDs = selectedItems.map(item => item.WebNotificationID);
        if (selectedIDs.length > 0) {
            var jsonData = angular.toJson({ webNotificationIDs: selectedIDs.join(',') });
            AngularAjaxCall($http, HomeCareSiteUrl.MarkAsReadWebNotificationsURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.GetWebNotificationList();
                }
            });
        }
    };
    //#endregion

    //#region PriorAuthExpiring
    $scope.PriorAuthExpiringListPager = new PagerModule("Patient", "#PriorAuthExpiringList", 'ASC');
    $scope.PriorAuthExpiringListPager.pageSize = 10;
    $scope.GetPriorAuthExpiringList = function (isSearchFilter) {
        $scope.PriorAuthExpiringListPager.currentPage = isSearchFilter ? 1 : $scope.PriorAuthExpiringListPager.currentPage;
        var pagermodel = {
            pageSize: $scope.PriorAuthExpiringListPager.pageSize,
            pageIndex: $scope.PriorAuthExpiringListPager.currentPage,
            sortIndex: $scope.PriorAuthExpiringListPager.sortIndex,
            sortDirection: $scope.PriorAuthExpiringListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.PriorAuthExpiringListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetPriorAuthExpiringURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.PriorAuthExpiringList = response.Data.Items;
                $scope.PriorAuthExpiringListPager.currentPageSize = response.Data.Items.length;
                $scope.PriorAuthExpiringListPager.totalRecords = response.Data.TotalItems;
            }
            $scope.PriorAuthExpiringListAjaxStart = false;
        });
    };
    $scope.PriorAuthExpiringListPager.getDataCallback = $scope.GetPriorAuthExpiringList;
    $scope.PriorAuthExpiringListPager.getDataCallback(true);
    //#endregion


    //#region PriorAuthExpired
    $scope.PriorAuthExpiredListPager = new PagerModule("Patient", "#PriorAuthExpiredList", 'ASC');
    $scope.PriorAuthExpiredListPager.pageSize = 10;
    $scope.GetPriorAuthExpiredList = function (isSearchFilter) {
        $scope.PriorAuthExpiredListPager.currentPage = isSearchFilter ? 1 : $scope.PriorAuthExpiredListPager.currentPage;
        var pagermodel = {
            pageSize: $scope.PriorAuthExpiredListPager.pageSize,
            pageIndex: $scope.PriorAuthExpiredListPager.currentPage,
            sortIndex: $scope.PriorAuthExpiredListPager.sortIndex,
            sortDirection: $scope.PriorAuthExpiredListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.PriorAuthExpiredListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetPriorAuthExpiredURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.PriorAuthExpiredList = response.Data.Items;
                $scope.PriorAuthExpiredListPager.currentPageSize = response.Data.Items.length;
                $scope.PriorAuthExpiredListPager.totalRecords = response.Data.TotalItems;
            }
            $scope.PriorAuthExpiredListAjaxStart = false;
        });
    };
    $scope.PriorAuthExpiredListPager.getDataCallback = $scope.GetPriorAuthExpiredList;
    $scope.PriorAuthExpiredListPager.getDataCallback(true);
    //#endregion
    //#region Patient Birthday
    $scope.PatientBirthdaySearchModel = $scope.newInstance().PatientBirthdaySearchModel;
    $scope.PatientBirthdayListPager = new PagerModule("Birthday", "#PatientBirthdayList", 'ASC');
    $scope.PatientBirthdayListPager.pageSize = 10;
    $scope.GetPatientBirthdayList = function (isSearchFilter) {
        if ($scope.PatientBirthdaySearchModel.StartDate > $scope.PatientBirthdaySearchModel.EndDate) {
            toastr.success("End Date should be greater than Start Date");
            return false;
        }

        $scope.PatientBirthdayListPager.currentPage = isSearchFilter ? 1 : $scope.PatientBirthdayListPager.currentPage;
        var pagermodel = {
            startDate: $scope.PatientBirthdaySearchModel.StartDate,
            endDate: $scope.PatientBirthdaySearchModel.EndDate,
            pageSize: $scope.PatientBirthdayListPager.pageSize,
            pageIndex: $scope.PatientBirthdayListPager.currentPage,
            sortIndex: $scope.PatientBirthdayListPager.sortIndex,
            sortDirection: $scope.PatientBirthdayListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.PatientBirthdayListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetPatientBirthdayURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.PatientBirthdayList = response.Data.Items;
                $scope.PatientBirthdayListPager.currentPageSize = response.Data.Items.length;
                $scope.PatientBirthdayListPager.totalRecords = response.Data.TotalItems;
            }
            $scope.PatientBirthdayListAjaxStart = false;
        });
    };
    $scope.PatientBirthdayListPager.getDataCallback = $scope.GetPatientBirthdayList;
    $scope.PatientBirthdayListPager.getDataCallback(true);

    $scope.SearchPatientBirthdayList = function () {
        $scope.PatientBirthdayListPager.currentPage = 1;
        $scope.PatientBirthdayListPager.getDataCallback(true);
    }
    //#endregion

    //#region Employee Birthday
    $scope.EmpBirthdaySearchModel = $scope.newInstance().EmpBirthdaySearchModel;
    $scope.EmployeeBirthdayListPager = new PagerModule("Birthday", "#EmployeeBirthdayList", 'ASC');
    $scope.EmployeeBirthdayListPager.pageSize = 10;
    $scope.GetEmployeeBirthdayList = function (isSearchFilter) {
        if ($scope.EmpBirthdaySearchModel.StartDate > $scope.EmpBirthdaySearchModel.EndDate) {
            toastr.success("End Date should be greater than Start Date");
            return false;
        }

        $scope.EmployeeBirthdayListPager.currentPage = isSearchFilter ? 1 : $scope.EmployeeBirthdayListPager.currentPage;
        var pagermodel = {
            startDate: $scope.EmpBirthdaySearchModel.StartDate,
            endDate: $scope.EmpBirthdaySearchModel.EndDate,
            pageSize: $scope.EmployeeBirthdayListPager.pageSize,
            pageIndex: $scope.EmployeeBirthdayListPager.currentPage,
            sortIndex: $scope.EmployeeBirthdayListPager.sortIndex,
            sortDirection: $scope.EmployeeBirthdayListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.EmployeeBirthdayListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeBirthdayURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.EmployeeBirthdayList = response.Data.Items;
                $scope.EmployeeBirthdayListPager.currentPageSize = response.Data.Items.length;
                $scope.EmployeeBirthdayListPager.totalRecords = response.Data.TotalItems;
            }
            $scope.EmployeeBirthdayListAjaxStart = false;
        });
    };
    $scope.EmployeeBirthdayListPager.getDataCallback = $scope.GetEmployeeBirthdayList;
    $scope.EmployeeBirthdayListPager.getDataCallback(true);

    $scope.SearchEmployeeBirthdayList = function () {
        $scope.EmployeeBirthdayListPager.currentPage = 1;
        $scope.EmployeeBirthdayListPager.getDataCallback(true);
    }
    //#endregion

    //#region Patient Clock In - Clock Out List
    $scope.PatientClockInOutListSearchModel = $scope.newInstance().PatientClockInOutListSearchModel;
    $scope.PatientClockInOutListEndDate = $scope.PatientClockInOutListSearchModel.EndDate;
    $scope.PatientClockInOutListPager = new PagerModule("ScheduleStartTime", "#PatientClockInOutList", 'DESC');
    $scope.PatientClockInOutListPager.pageSize = 10;

    $scope.GetPatientClockInOutList = function (isSearchFilter) {
        if ($scope.PatientClockInOutListSearchModel.StartDate > $scope.PatientClockInOutListSearchModel.EndDate) {
            toastr.error("End Date should be greater than Start Date");
            return false;
        }
        $scope.PatientClockInOutListPager.currentPage = isSearchFilter ? 1 : $scope.PatientClockInOutListPager.currentPage;
        var pagermodel = {
            startDate: $scope.PatientClockInOutListSearchModel.StartDate,
            endDate: $scope.PatientClockInOutListSearchModel.EndDate,
            patientName: $scope.PatientClockInOutListSearchModel.PatientName,
            pageSize: $scope.PatientClockInOutListPager.pageSize,
            pageIndex: $scope.PatientClockInOutListPager.currentPage,
            sortIndex: $scope.PatientClockInOutListPager.sortIndex,
            sortDirection: $scope.PatientClockInOutListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.PatientClockInOutListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetPatientClockInOutListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.PatientClockInOutList = response.Data.Items;
                $scope.PatientClockInOutListPager.currentPageSize = response.Data.Items.length;
                $scope.PatientClockInOutListPager.totalRecords = response.Data.TotalItems;
                $scope.GetPatientAddress();
                if (response.Data.TotalItems > 0) {
                    $scope.PatientClockInOutListPager.totalPresent = $scope.PatientClockInOutList[0].TotalPresent;
                    $scope.PatientClockInOutListPager.totalAbsent = $scope.PatientClockInOutList[0].TotalAbsent;
                    $scope.TotalPatientSchedule = $scope.PatientClockInOutList[0].TotalPatientSchedule;
                    $scope.TotalPresent = $scope.PatientClockInOutList[0].TotalPresent;
                    $scope.TotalAbsent = $scope.PatientClockInOutList[0].TotalAbsent;
                }
                else {
                    $scope.TotalPatientSchedule = 0;
                    $scope.TotalPresent = 0;
                    $scope.TotalAbsent = 0;
                }

            }
            $scope.PatientClockInOutListAjaxStart = false;
        });

    };

    $scope.SearchPatientClockInOutList = function () {
        $scope.PatientClockInOutListPager.currentPage = 1;
        $scope.PatientClockInOutListPager.getDataCallback(true);
    }

    $scope.PatientClockInOutListPager.getDataCallback = $scope.GetPatientClockInOutList;
    $scope.PatientClockInOutListPager.getDataCallback(true);

    //#endregion

    //#region Patient Discharged
    $scope.PatientDischargedListSearchModel = $scope.newInstance().PatientDischargedListSearchModel;
    $scope.PatientDischargedListPager = new PagerModule("Patient", "#PatientDischargedList", 'ASC');
    $scope.PatientDischargedListPager.pageSize = 10;
    $scope.GetPatientDischargedList = function (isSearchFilter) {
        if ($scope.PatientDischargedListSearchModel.StartDate > $scope.PatientDischargedListSearchModel.EndDate) {
            toastr.error("End Date should be greater than Start Date");
            return false;
        }

        $scope.PatientDischargedListPager.currentPage = isSearchFilter ? 1 : $scope.PatientDischargedListPager.currentPage;
        var pagermodel = {
            startDate: $scope.PatientDischargedListSearchModel.StartDate,
            endDate: $scope.PatientDischargedListSearchModel.EndDate,
            pageSize: $scope.PatientDischargedListPager.pageSize,
            pageIndex: $scope.PatientDischargedListPager.currentPage,
            sortIndex: $scope.PatientDischargedListPager.sortIndex,
            sortDirection: $scope.PatientDischargedListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.PatientDischargedListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetPatientDischargedListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.PatientDischargedList = response.Data.Items;
                $scope.PatientDischargedListPager.currentPageSize = response.Data.Items.length;
                $scope.PatientDischargedListPager.totalRecords = response.Data.TotalItems;
            }
            $scope.PatientDischargedListAjaxStart = false;
        });
    };
    $scope.PatientDischargedListPager.getDataCallback = $scope.GetPatientDischargedList;
    $scope.PatientDischargedListPager.getDataCallback(true);

    $scope.SearchPatientDischargedList = function () {
        $scope.PatientDischargedListPager.currentPage = 1;
        $scope.PatientDischargedListPager.getDataCallback(true);
    }
    //#endregion

    //#region Patient Transfer
    $scope.PatientTransferListSearchModel = $scope.newInstance().PatientTransferListSearchModel;
    $scope.PatientTransferListPager = new PagerModule("Patient", "#PatientTransferList", 'ASC');
    $scope.PatientTransferListPager.pageSize = 10;
    $scope.GetPatientTransferList = function (isSearchFilter) {
        if ($scope.PatientTransferListSearchModel.StartDate > $scope.PatientTransferListSearchModel.EndDate) {
            toastr.error("End Date should be greater than Start Date");
            return false;
        }

        $scope.PatientTransferListPager.currentPage = isSearchFilter ? 1 : $scope.PatientTransferListPager.currentPage;
        var pagermodel = {
            startDate: $scope.PatientTransferListSearchModel.StartDate,
            endDate: $scope.PatientTransferListSearchModel.EndDate,
            pageSize: $scope.PatientTransferListPager.pageSize,
            pageIndex: $scope.PatientTransferListPager.currentPage,
            sortIndex: $scope.PatientTransferListPager.sortIndex,
            sortDirection: $scope.PatientTransferListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.PatientTransferListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetPatientTransferListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.PatientTransferList = response.Data.Items;
                $scope.PatientTransferListPager.currentPageSize = response.Data.Items.length;
                $scope.PatientTransferListPager.totalRecords = response.Data.TotalItems;
            }
            $scope.PatientTransferListAjaxStart = false;
        });
    };
    $scope.PatientTransferListPager.getDataCallback = $scope.GetPatientTransferList;
    $scope.PatientTransferListPager.getDataCallback(true);

    $scope.SearchPatientTransferList = function () {
        $scope.PatientTransferListPager.currentPage = 1;
        $scope.PatientTransferListPager.getDataCallback(true);
    }
    //#endregion

    //#region Patient Pending
    $scope.PatientPendingListSearchModel = $scope.newInstance().PatientPendingListSearchModel;
    $scope.PatientPendingListPager = new PagerModule("Patient", "#PatientPendingList", 'ASC');
    $scope.PatientPendingListPager.pageSize = 10;
    $scope.GetPatientPendingList = function (isSearchFilter) {
        if ($scope.PatientPendingListSearchModel.StartDate > $scope.PatientPendingListSearchModel.EndDate) {
            toastr.error("End Date should be greater than Start Date");
            return false;
        }

        $scope.PatientPendingListPager.currentPage = isSearchFilter ? 1 : $scope.PatientPendingListPager.currentPage;
        var pagermodel = {
            startDate: $scope.PatientPendingListSearchModel.StartDate,
            endDate: $scope.PatientPendingListSearchModel.EndDate,
            pageSize: $scope.PatientPendingListPager.pageSize,
            pageIndex: $scope.PatientPendingListPager.currentPage,
            sortIndex: $scope.PatientPendingListPager.sortIndex,
            sortDirection: $scope.PatientPendingListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.PatientPendingListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetPatientPendingListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.PatientPendingList = response.Data.Items;
                $scope.PatientPendingListPager.currentPageSize = response.Data.Items.length;
                $scope.PatientPendingListPager.totalRecords = response.Data.TotalItems;
            }
            $scope.PatientPendingListAjaxStart = false;
        });
    };
    $scope.PatientPendingListPager.getDataCallback = $scope.GetPatientPendingList;
    $scope.PatientPendingListPager.getDataCallback(true);

    $scope.SearchPatientPendingList = function () {
        $scope.PatientPendingListPager.currentPage = 1;
        $scope.PatientPendingListPager.getDataCallback(true);
    }
    //#endregion

    //#region Patient OnHold
    $scope.PatientOnHoldListSearchModel = $scope.newInstance().PatientOnHoldListSearchModel;
    $scope.PatientOnHoldListPager = new PagerModule("Patient", "#PatientOnHoldList", 'ASC');
    $scope.PatientOnHoldListPager.pageSize = 10;
    $scope.GetPatientOnHoldList = function (isSearchFilter) {
        if ($scope.PatientOnHoldListSearchModel.StartDate > $scope.PatientOnHoldListSearchModel.EndDate) {
            toastr.error("End Date should be greater than Start Date");
            return false;
        }

        $scope.PatientOnHoldListPager.currentPage = isSearchFilter ? 1 : $scope.PatientOnHoldListPager.currentPage;
        var pagermodel = {
            startDate: $scope.PatientOnHoldListSearchModel.StartDate,
            endDate: $scope.PatientOnHoldListSearchModel.EndDate,
            pageSize: $scope.PatientOnHoldListPager.pageSize,
            pageIndex: $scope.PatientOnHoldListPager.currentPage,
            sortIndex: $scope.PatientOnHoldListPager.sortIndex,
            sortDirection: $scope.PatientOnHoldListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.PatientOnHoldListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetPatientOnHoldListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.PatientOnHoldList = response.Data.Items;
                $scope.PatientOnHoldListPager.currentPageSize = response.Data.Items.length;
                $scope.PatientOnHoldListPager.totalRecords = response.Data.TotalItems;
            }
            $scope.PatientOnHoldListAjaxStart = false;
        });
    };
    $scope.PatientOnHoldListPager.getDataCallback = $scope.GetPatientOnHoldList;
    $scope.PatientOnHoldListPager.getDataCallback(true);

    $scope.SearchPatientOnHoldList = function () {
        $scope.PatientOnHoldListPager.currentPage = 1;
        $scope.PatientOnHoldListPager.getDataCallback(true);
    }
    //#endregion

    //#region Patient Medicaid
    //$scope.PatientMedicaidListSearchModel = $scope.newInstance().PatientMedicaidListSearchModel;
    //$scope.PatientMedicaidListPager = new PagerModule("Patient", "#PatientMedicaidList", 'ASC');
    //$scope.PatientMedicaidListPager.pageSize = 10;
    //$scope.GetPatientMedicaidList = function (isSearchFilter) {
    //    if ($scope.PatientMedicaidListSearchModel.StartDate > $scope.PatientMedicaidListSearchModel.EndDate) {
    //        toastr.success("End Date should be greater than Start Date");
    //        return false;
    //    }

    //    $scope.PatientMedicaidListPager.currentPage = isSearchFilter ? 1 : $scope.PatientMedicaidListPager.currentPage;
    //    var pagermodel = {
    //        startDate: $scope.PatientMedicaidListSearchModel.StartDate,
    //        endDate: $scope.PatientMedicaidListSearchModel.EndDate,
    //        pageSize: $scope.PatientMedicaidListPager.pageSize,
    //        pageIndex: $scope.PatientMedicaidListPager.currentPage,
    //        sortIndex: $scope.PatientMedicaidListPager.sortIndex,
    //        sortDirection: $scope.PatientMedicaidListPager.sortDirection
    //    };
    //    var jsonData = angular.toJson(pagermodel);
    //    $scope.PatientMedicaidListAjaxStart = true;
    //    AngularAjaxCall($http, HomeCareSiteUrl.GetPatientMedicaidListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
    //        if (response.IsSuccess) {
    //            $scope.PatientMedicaidList = response.Data.Items;
    //            $scope.PatientMedicaidListPager.currentPageSize = response.Data.Items.length;
    //            $scope.PatientMedicaidListPager.totalRecords = response.Data.TotalItems;
    //        }
    //        $scope.PatientMedicaidListAjaxStart = false;
    //    });
    //};
    //$scope.PatientMedicaidListPager.getDataCallback = $scope.GetPatientMedicaidList;
    //$scope.PatientMedicaidListPager.getDataCallback(true);

    //$scope.SearchPatientMedicaidList = function () {
    //    $scope.PatientMedicaidListPager.currentPage = 1;
    //    $scope.PatientMedicaidListPager.getDataCallback(true);
    //}
    //#endregion

    //#region Employee Clock In - Clock Out List WithOut Status
    $scope.EmpClockInOutListWithOutStatusPager = new PagerModule();
    $scope.GetEmpClockInOutListWithOutStatus = function (isSearchFilter) {
        $scope.EmpClockInOutListWithOutStatusPager.currentPage = isSearchFilter ? 1 : $scope.EmpClockInOutListWithOutStatusPager.currentPage;
        var pagermodel = {
            startDate: $scope.EmpClockInOutListSearchModel.StartDate,
            endDate: $scope.EmpClockInOutListSearchModel.EndDate
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.EmpClockInOutListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmpClockInOutListWithOutStatusURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.EmpClockInOutListWithOutStatus = response.Data.Items;
                $scope.EmpClockInOutListWithOutStatusPager.currentPageSize = response.Data.Items.length;
                $scope.EmpClockInOutListWithOutStatusPager.totalRecords = response.Data.TotalItems;
                if ($scope.EmpClockInOutListWithOutStatus.length > 0) {
                    $scope.TotalScheduleWOStatus = $scope.EmpClockInOutListWithOutStatus[0].TotalSchedule;
                    $scope.InprogressWOStatus = $scope.EmpClockInOutListWithOutStatus[0].Inprogress;
                    $scope.MissedScheduleWOStatus = $scope.EmpClockInOutListWithOutStatus[0].MissedSchedule;
                    $scope.TotalCompleteWOStatus = $scope.EmpClockInOutListWithOutStatus[0].TotalComplete;
                }

                if (response.Data.TotalItems > 0) {
                    $scope.EmpClockInOutListWithOutStatusPager.inprogress = $scope.EmpClockInOutListWithOutStatus[0].Inprogress;
                    $scope.EmpClockInOutListWithOutStatusPager.missedSchedule = $scope.EmpClockInOutListWithOutStatus[0].MissedSchedule;
                    $scope.EmpClockInOutListWithOutStatusPager.totalComplete = $scope.EmpClockInOutListWithOutStatus[0].TotalComplete;
                    $scope.TotalScheduleWOStatus = $scope.EmpClockInOutList[0].TotalSchedule;
                    $scope.InprogressWOStatus = $scope.EmpClockInOutListWithOutStatus[0].Inprogress;
                    $scope.MissedScheduleWOStatus = $scope.EmpClockInOutListWithOutStatus[0].MissedSchedule;
                    $scope.TotalCompleteWOStatus = $scope.EmpClockInOutListWithOutStatus[0].TotalComplete;
                }
                else {
                    $scope.TotalScheduleWOStatus = 0;
                    $scope.InprogressWOStatus = 0;
                    $scope.MissedScheduleWOStatus = 0;
                    $scope.TotalCompleteWOStatus = 0;
                }
            }
            $scope.EmpClockInOutListWithOutStatusAjaxStart = false;
        });
    };

    $scope.SearchEmpClockInOutListWithOutStatus = function () {
        $scope.EmpClockInOutListWithOutStatusPager.currentPage = 1;
        $scope.EmpClockInOutListWithOutStatusPager.getDataCallback(true);
    }

    $scope.EmpClockInOutListWithOutStatusPager.getDataCallback = $scope.GetEmpClockInOutListWithOutStatus;
    $scope.EmpClockInOutListWithOutStatusPager.getDataCallback(true);
    //#endregion

    $scope.GetCareType = function () {
        AngularAjaxCall($http, HomeCareSiteUrl.GetCareTypeListURL, null, "Get", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.GetCareTypes = response.Data;
            }
            else {
                $scope.GetCareTypes = 0;
            }
        });
    };
    $scope.GetRegion = function () {
        AngularAjaxCall($http, HomeCareSiteUrl.GetRegionURL, null, "Get", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.GetRegionList = response.Data;
            }
            else {
                $scope.GetRegionList = 0;
            }
        });
    };

    //$scope.GetReferralPayor = function () {
    //    AngularAjaxCall($http, HomeCareSiteUrl.GetReferralPayorUrl, null, "Get", "json", "application/json", false).success(function (response) {
    //        if (response.IsSuccess) {
    //            $scope.GetPayor = response.Data.Items;
    //            $scope.GetPayor.totalRecords = response.Data.TotalItems;
    //            if (response.Data.TotalItems > 0) {
    //                $scope.TotalPayor = $scope.GetPayor[0].TotalPayor;
    //            }
    //            else {
    //                $scope.TotalPayor = 0;
    //            }
    //        }
    //        else {
    //            $scope.GetPayor = 0;
    //        }

    //    });
    //};

    $scope.EmpClockInOuReport = function (ReportName) {
        $scope.EmpClockInOutListSearchModel.EmployeeName;
        $scope.EmpClockInOutListSearchModel.DDMasterID;
        $scope.EmpClockInOutListSearchModel.Status;
        $scope.SelectedScheduleIds;
        if ($scope.EmpClockInOutListSearchModel.EmployeeName == undefined || $scope.EmpClockInOutListSearchModel.EmployeeName == '') { var parameterValue3 = '' }
        else { var parameterValue3 = $scope.EmpClockInOutListSearchModel.EmployeeName; }

        if ($scope.EmpClockInOutListSearchModel.DDMasterID == undefined || $scope.EmpClockInOutListSearchModel.DDMasterID == '') { var parameterValue4 = 0 }
        else { var parameterValue4 = $scope.EmpClockInOutListSearchModel.DDMasterID; }

        if ($scope.EmpClockInOutListSearchModel.Status == undefined || $scope.EmpClockInOutListSearchModel.Status == '') { var parameterValue5 = '' }
        else { var parameterValue5 = $scope.EmpClockInOutListSearchModel.Status; }

        if ($scope.SelectedScheduleIds == undefined || $scope.SelectedScheduleIds == '') { var parameterValue6 = 0 }
        else { var parameterValue6 = $scope.SelectedScheduleIds; }
        var Domain = window.Domain;
        var ReportURL1 = 'https://';
        var ReportURL2 = '.myezcare.com';
        var ReportURL3 = "/Report/Template?ReportName=";
        var ReportURL4 = "&StartDate=";
        var parameterValue1 = $scope.EmpClockInOutListSearchModel.StartDate;
        var ReportURL5 = "&EndDate=";
        var parameterValue2 = $scope.EmpClockInOutListSearchModel.EndDate;
        var ReportURL6 = "&EmployeeName=";
        var ReportURL7 = "&CareTypeID=";
        var ReportURL8 = "&Status=";
        var ReportURL9 = "&ScheduleID=";

        var url = ReportURL1 + Domain + ReportURL2 + ReportURL3 + ReportName + ReportURL4 + parameterValue1 + ReportURL5 + parameterValue2 +
            ReportURL6 + parameterValue3 + ReportURL7 + parameterValue4 + ReportURL8 + parameterValue5 + ReportURL9 + parameterValue6;
        var width = screen.availWidth - 10;
        var height = screen.availHeight - 60;
        var left = 0;//(screen.availWidth - width) / 2;
        var top = 0;//(screen.availHeight - height) / 2;
        var params = 'width=' + width + ', height=' + height;
        params += ', top=' + top + ', left=' + left;
        var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,' + params;
        var pdfWindow = window.open('about:blank', 'null', winFeature);
        pdfWindow.document.write("<html><head><style> * { box-sizing: border-box; padding: 0; margin: 0; border: 0; }</style>"
            + "<title>" + ReportName + "</title></head><body>"
            + '<embed width="100%" height="100%" name="plugin" src="' + url + '" '
            + 'type="application/pdf" internalinstanceid="21"></body></html>');
        pdfWindow.document.close();

    };

    $scope.SelectSchedule = function (item) {
        if (item.IsChecked) {
            $scope.SelectedScheduleIds.push(item.ScheduleID);
            $scope.SelectedEmployeeIds.push(item.EmployeeID);
            $scope.SelectedEmployeesName.push(item.Employee);
        }
        else {
            $scope.SelectedScheduleIds.remove(item.ScheduleID);
            $scope.SelectedEmployeeIds.remove(item.EmployeeID);
            $scope.SelectedEmployeesName.remove(item.Employee);
        }

        if ($scope.SelectedScheduleIds.length == $scope.EmpClockInOutListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    $scope.SelectAll = function (value) {
        $scope.SelectedScheduleIds = [];
        angular.forEach($scope.EmpClockInOutList, function (item, key) {
            item.IsChecked = value;// event.target.checked;
            if (item.IsChecked) {
                $scope.SelectedScheduleIds.push(item.ScheduleID);
                $scope.SelectedEmployeeIds.push(item.EmployeeID);
                $scope.SelectedEmployeesName.push(item.Employee);
            }
            else {
                $scope.SelectedScheduleIds.remove(item.ScheduleID);
                $scope.SelectedEmployeeIds.remove(item.EmployeeID);
                $scope.SelectedEmployeesName.remove(item.Employee);
            }

            if ($scope.SelectedScheduleIds.length == $scope.EmpClockInOutListPager.currentPageSize)
                $scope.SelectAllCheckbox = true;
            else
                $scope.SelectAllCheckbox = false;
        });

        return true;
    };

    $(document).ready(function () {
        if ($("#WebNotificationList").length) {
            $scope.GetWebNotificationList();
            // 2 MIN = 120000 MS
            setInterval(function () {
                $scope.GetWebNotificationList();
            }, 120000);
        }
        $scope.GetPriorAuthExpiringList();
        $scope.GetPriorAuthExpiredList();
        //$scope.GetReferralPayor();
        $scope.GetCareType();
        $scope.GetRegion();
    });




    //#region Get Count of Visits which are pending for Approval
    $scope.GetPendingBypassVisit = function () {
        AngularAjaxCall($http, HomeCareSiteUrl.GetPendingBypassVisitUrl, null, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.PendingBypassVisitCount = response.Data;
            }
            $scope.PatientNotScheduleListAjaxStart = false;
        });
    };
    $scope.GetPendingBypassVisit();
    //#endregion

    $scope.ViewMoreClick = function (ele, count) {
        if (parseInt(count) > 0) {
            $('html, body').animate({
                scrollTop: $(ele).offset().top - 50
            }, 500);
        }
    };

    $scope.ShowCollpase = function () {
        setTimeout(function () {
            $.each($('.collapseDestination'), function (index, data) {

                $(this).on('show.bs.collapse', function () {
                    $(this).parents("tbody").find(".collapseSource").removeClass("fa-plus-circle").addClass("fa-minus-circle");
                });

                $(this).on('hidden.bs.collapse', function () {
                    $(this).parents("tbody").find(".collapseSource").removeClass("fa-minus-circle").addClass("fa-plus-circle");
                });

            });

        }, 100);
    };
    $scope.ShowCollpase();
    //#region Address on  google map
    $scope.PatientAddressList = [];
    $scope.ClockInClockOutMap = {};
    $scope.GetPatientAddress = function () {
        var jsonData = angular.toJson({
            startDate: $scope.EmpClockInOutListSearchModel.StartDate,
            endDate: $scope.EmpClockInOutListSearchModel.EndDate,

        });
        AngularAjaxCall($http, HomeCareSiteUrl.GetPatientAddressListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            $scope.PatientAddressList = response.Data.Items;
        });
    }

    $scope.Map;
    $scope.GlobalMarkers = [];
    $scope.InfoWindow = new google.maps.InfoWindow();
    var MarkerCluster;
    function initMap() {
        $scope.Map = new google.maps.Map(document.getElementById("dvMap"), {
            center: { lat: 34.397, lng: 150.644 },
            zoom: 3,
            mapTypeId: google.maps.MapTypeId.ROADMAP,

            styles: [
                {
                    featureType: 'transit',
                    elementType: 'labels.icon',
                    stylers: [{ visibility: 'off' }]
                },
                {
                    featureType: 'poi',
                    stylers: [{ visibility: 'off' }]
                },
                {
                    featureType: 'road',
                    stylers: [{ visibility: 'off' }]
                },
            ]
        });
    };

    function DeleteMarkers() {
        for (var i = 0; i < $scope.GlobalMarkers.length; i++) {

            $scope.GlobalMarkers[i].setMap(null);
        }
        $scope.GlobalMarkers = [];
        //markers = [];
    };

    function DrawMarkers(PatientList, URLHost) {
        console.log('Drawing Marker');
        console.log('Patient', PatientList);
        for (var i = 0; i < PatientList.length; i++) {
            var data = PatientList[i];
            DrawsingleMarker(data, URLHost);

        }

        //Adding InfoWindow to the Marker.

        console.log('Global', $scope.GlobalMarkers);
        //Plotting the Marker on the Map.

    }
    function DrawsingleMarker(MarkerData, HostURL) {
        var myLatlng = new google.maps.LatLng(MarkerData.lat, MarkerData.lng);
        var pin = new google.maps.Marker({
            position: myLatlng,
            map: $scope.Map,
            title: MarkerData.Patient
        });
        if (MarkerData.ClockIn == true && MarkerData.ClockOut == true) {
            pin.setIcon(HostURL + '/Assets/images/green-map-pin.png');
        } else {
            pin.setIcon(HostURL + '/Assets/images/red-map-pin.png');
        }


        $scope.GlobalMarkers.push(pin);
        google.maps.event.addListener(pin, 'click', function () {
            $scope.InfoWindow.setContent("<div class='col-md-12 margin-bottom-5 no-padding'><div class='pull-left'><span><strong>EmployeeName : </strong><span>" + MarkerData.Employee + "</span></span></br><span><strong>PatientName : </strong><span>" + MarkerData.Patient + "</span></span></br> <i class='fa fa-phone'></i><strong>&nbsp;PatientContact No:&nbsp;</strong> <span>" + MarkerData.Phone + " </span></div> <div class='pull-right' style='padding-left: 6px;'><span ><strong>StartTime : </strong><span>" + MarkerData.StartTime + " </span></span></br><span><strong>EndTime : </strong><span>" + MarkerData.EndTime + " </span></span></br><span><strong>PatientAddress: </strong><span>" + MarkerData.Address + "</span></span></div> </div>");

            $scope.InfoWindow.open($scope.Map, pin);
        });
    }
    function BoundMap() {
        var bounds = new google.maps.LatLngBounds();
        for (var i = 0; i < $scope.GlobalMarkers.length; i++) {
            //var position = new google.maps.LatLng($scope.GlobalMarkers[i].lat, $scope.GlobalMarkers[i].lng);
            bounds.extend($scope.GlobalMarkers[i].position);

        }
        $scope.Map.setCenter(bounds.getCenter());
        //$scope.Map.fitBounds(bounds);
        if (MarkerCluster != null) {
            MarkerCluster.clearMarkers();
            MarkerCluster = new MarkerClusterer($scope.Map, $scope.GlobalMarkers, {
                imagePath:
                    "https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m",
            });
        } else {
            MarkerCluster = new MarkerClusterer($scope.Map, $scope.GlobalMarkers, {
                imagePath:
                    "https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m",
            });
        }

    }

    function UniqueArraybyId(collection, keyname) {
        var output = [],
            keys = [];

        angular.forEach(collection, function (item) {
            var key = item[keyname];
            if (keys.indexOf(key) === -1) {
                keys.push(key);
                output.push(item);
            }
        });
        return output;
    };
    $scope.OpenClockInClockOutMapModal = function ($event, item) {
        initMap();
        console.log($scope.PatientAddressList);
        $scope.PatientAddressList = UniqueArraybyId($scope.PatientAddressList, "lat");
        //$scope.PatientAddressList = RemoveDuplication($scope.PatientAddressList);
        console.log($scope.PatientAddressList);

        if ($scope.PatientAddressList.length > 0 && $scope.PatientAddressList != undefined) {
            DeleteMarkers();

            var Hostname = window.location.origin;
            DrawMarkers($scope.PatientAddressList, Hostname);
            BoundMap();
            $('#ClockInClockOutAdressModal').modal({
                backdrop: 'static',
                keyboard: false
            });





            ////Looping through the Array and adding Markers.
            //for (var i = 0; i < $scope.Markers.length; i++) {
            //    var data = $scope.Markers[i];
            //    var myLatlng = new google.maps.LatLng(data.lat, data.lng);

            //    //Initializing the Marker object.
            //    if (data.ClockIn == true && data.ClockOut == true) {
            //        var marker = new google.maps.Marker({
            //            position: myLatlng,
            //            map: $scope.Map,
            //            title: data.Patient,
            //            icon: {
            //                url: Hostname + '/Assets/images/green-map-pin.png'

            //            }
            //        });
            //        $scope.Latlngbounds.extend(marker.position);
            //        $scope.AllMarker.push(marker);
            //    }
            //    else {
            //        var marker = new google.maps.Marker({
            //            position: myLatlng,
            //            map: $scope.Map,
            //            title: data.Patient,
            //            icon: {
            //                url: Hostname + '/Assets/images/red-map-pin.png'

            //            }
            //        });
            //        $scope.Latlngbounds.extend(marker.position);
            //        $scope.AllMarker.push(marker);
            //    }

            //    //Adding InfoWindow to the Marker.
            //    (function (marker, data) {
            //        google.maps.event.addListener(marker, "click", function (e) {
            //            //$scope.InfoWindow.setContent("<div style = 'width:200px;min-height:40px'>" + data.Address + "</div>");
            //            $scope.InfoWindow.setContent("<div class='col-md-12 margin-bottom-5 no-padding'><div class='pull-left'><span><strong>EmployeeName : </strong><span>" + data.Employee + "</span></span></br><span><strong>PatientName : </strong><span>" + data.Patient + "</span></span></br> <i class='fa fa-phone'></i><strong>&nbsp;PatientContact No:&nbsp;</strong> <span>" + data.Phone + " </span></div> <div class='pull-right' style='padding-left: 6px;'><span ><strong>StartTime : </strong><span>" + data.StartTime + " </span></span></br><span><strong>EndTime : </strong><span>" + data.EndTime + " </span></span></br><span><strong>PatientAddress: </strong><span>" + data.Address + "</span></span></div> </div>");

            //            $scope.InfoWindow.open($scope.Map, marker);
            //        });
            //    })(marker, data);

            //    //Plotting the Marker on the Map.

            //}

            //// Add a marker clusterer to manage the markers.
            //var abc = new MarkerClusterer($scope.Map, $scope.AllMarker);
            //Adjusting the Map for best display.
            //$scope.Map.setCenter($scope.Latlngbounds.getCenter());
            //$scope.Map.fitBounds($scope.Latlngbounds);
        }




    }

    function RemoveDuplication(DataArray) {

    }
    $scope.ClockInClockOutAdressModelClosed = function () {
        $('#ClockInClockOutAdressModal').modal('hide');
    }
    // #endregion
    $scope.SendSMSModel = {};
    $scope.BroadcastNotificationModal = function () {
        $scope.SendSMSModel.EmployeeIds = $scope.SelectedEmployeeIds.toString();
        $scope.SendSMSModel.Employees = $scope.SelectedEmployeesName.toString();
        $scope.SendSMSModel.NotificationType = 1;
        if ($scope.SelectedEmployeeIds.length > 0) {
            $('#BroadcastNotificationModal').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        else {
            toastr.error("Please select atleast one checkbox");
        }


    }
    $scope.SendBroadcastNotification = function () {
        if ($scope.SelectedEmployeeIds.length === 0) {
            toastr.error("Please select employees to send notification");
            return false;
        }

        if (ValideElement($scope.SendSMSModel.Message) === false) {
            toastr.error("Please enter message to send the employees");
            return false;
        }
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = angular.toJson({
                    model: $scope.SendSMSModel
                });
                AngularAjaxCall($http, HomeCareSiteUrl.BroadcastNotificationUrl, jsonData, "post", "json", "application/json", true).
                    success(function (response, status, headers, config) {

                        if (response.IsSuccess) {
                            $scope.SelectedEmployeeIds = [];
                            $scope.SelectAllCheckbox = false;
                            $scope.RemoveAllSelected();
                            // $scope.SendSMSModel = $scope.newInstance().SendSMSModel;
                        } else {
                            ShowMessages(response);
                        }
                        ShowMessages(response);
                    });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, window.ConfirmBroadcastNotification, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    }
    $scope.BroadcastNotificationModalClosed = function () {
        $scope.SelectedEmployeeIds = [];
        $scope.SelectedEmployeesName = [];
        $scope.SendSMSModel = [];
        $scope.SelectedScheduleIds = 0,
            $scope.SelectAllCheckbox = false;
        $scope.RemoveAllSelected();
        $('#BroadcastNotificationModal').modal('hide');
    }
    $scope.RemoveAllSelected = function () {
        $scope.SelectedEmployeeIds = [];

        angular.forEach($scope.EmpClockInOutList, function (item, key) {
            item.IsChecked = false;
        });

        return true;
    };

    $scope.ChangeSchedulePopup = function(item){
        ChangeSchedulePopup(item);
    }
};

controllers.DashboardController.$inject = ['$scope', '$http'];

//var RefreshEmpClockInOutList = function () {
//    $scope = vm;
//    $scope.GetEmpClockInOutList();
//}
function RefreshEmpClockInOutList(options) {
    var $scope = vm;
    $scope.GetEmpClockInOutList(options);
}