var vm;

controllers.ScheduleMasterListController = function ($scope, $http, $timeout, $filter) {
    vm = $scope;
    //
    $scope.NewInstance = function () {
        return $.parseJSON($("#hdnSetScheduleMasterListPage").val());
    };

    $scope.ScheduleMasterList = [];
    $scope.SelectedScheduleMasterIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.CnclStatus = window.CancelStatus;
    $scope.EncryptedReferralID = window.EncryptedReferralID;

    $scope.ScheduleMasterModel = $.parseJSON($("#hdnSetScheduleMasterListPage").val());
    $scope.AddScheduleBatchService = $scope.NewInstance().AddScheduleBatchService;

    $scope.SearchScheduleMasterListPage = $scope.ScheduleMasterModel.SearchScheduleMasterModel;

    $scope.SortingColumn = "StartDate";
    $scope.SortingType = "ASC";
    $scope.SearchScheduleMasterListPage.IsPartial = $scope.ScheduleMasterModel.IsPartial;
    if ($scope.ScheduleMasterModel.IsPartial) {
        $scope.SearchScheduleMasterListPage.ReferralID = $scope.EncryptedReferralID;
        $scope.SortingColumn = "StartDate";
        $scope.SortingType = "DESC";
    }

    $scope.TempSearchScheduleMasterListPage = {};

    $scope.ScheduleMasterListPager = new PagerModule($scope.SortingColumn, null, $scope.SortingType);

    //$scope.DoMultipleSorting

    $scope.SetPostData = function (fromIndex, model) {

        var pagermodel = {
            searchScheduleMasterModel: $scope.SearchScheduleMasterListPage,
            pageSize: $scope.ScheduleMasterListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ScheduleMasterListPager.sortIndex,
            sortDirection: $scope.ScheduleMasterListPager.sortDirection,
            sortIndexArray: $scope.ScheduleMasterListPager.sortIndexArray.toString()
        };
        if (model != undefined) {
            pagermodel.scheduleModel = model;
        }
        return angular.toJson(pagermodel);
    };


    $scope.OpenScheduleNotificationLogModel = function (item) {
        $scope.IsEmailModel = true;
        var jsonData = { scheduleId: item.ScheduleID };
        AngularAjaxCall($http, HomeCareSiteUrl.GetScheduleNotificationLogsURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $('#ScheduleNotificationLog-modal').modal('show');
                $scope.ScheduleNotificationDetails = response.Data;
            }
            //ShowMessages(response);
        });
    };


    $scope.IsEmailModel = false;

    $scope.OpenEmailPopup = function (item) {
        $scope.IsEmailModel = true;
        var jsonData = { scheduleId: item.ScheduleID };
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmailDetailURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ScheduleMasterModel.ScheduleEmailModel = response.Data;
                $scope.EmailModel = angular.copy($scope.ScheduleMasterModel.ScheduleEmailModel);
                $scope.EmailModel.ParentName = item.ParentName;
                $scope.EmailModel.ReferralID = item.ReferralID;
                $scope.EmailModel.Phone = item.Phone1;
                $('#email-modal').modal('show');

            } ShowMessages(response);
        });
    };

    $scope.OpenEmpEmailPopup = function (item) {
        $scope.IsEmailModel = true;
        var jsonData = { scheduleId: item.ScheduleID };
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmpEmailDetailURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ScheduleMasterModel.ScheduleEmailModel = response.Data;
                
                $scope.EmpEmailModel = angular.copy($scope.ScheduleMasterModel.ScheduleEmailModel);
                $scope.EmpEmailModel.ParentName = item.ParentName;
                $scope.EmpEmailModel.ReferralID = item.ReferralID;
                $scope.EmpEmailModel.Phone = item.Phone1;
                $('#empemail-modal').modal('show');

            } ShowMessages(response);
        });
    };

    $scope.PrintNoticeScheduleNotification = function (scheduleId) {
        var jsonData = { scheduleId: scheduleId };
        AngularAjaxCall($http, SiteUrl.PrintNoticeScheduleNotificationURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
            } ShowMessages(response);
        });
    };

    $scope.OpenSMSPopup = function (item, phone) {
        var jsonData = { scheduleId: item.ScheduleID };
        AngularAjaxCall($http, HomeCareSiteUrl.GetSMSDetailURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ScheduleMasterModel.ScheduleSmsModel = response.Data;
                $scope.SmsModel = angular.copy($scope.ScheduleMasterModel.ScheduleSmsModel);
                if (phone != undefined) {
                    $scope.SmsModel.ToNumber = phone;
                }
                $scope.SmsModel.ParentName = item.ParentName;
                $scope.SmsModel.ReferralID = item.ReferralID;
                $scope.SmsModel.Phone = item.Phone1;
                $('#sms-modal').modal('show');
            } ShowMessages(response);
        });
    };





    $scope.SendParentEmail = function () {
        var isValid = CheckErrors($("#parent-email"));
        if (isValid) {
            var jsonData = { scheduleEmailModel: $scope.EmailModel };
            AngularAjaxCall($http, HomeCareSiteUrl.SendParentEmailURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $('#email-modal').modal('hide');
                    $scope.IsEmailModel = false;
                }
            });
        }
    };

    $scope.SendEmpEmail = function () {
        var isValid = CheckErrors($("#emp-email"));
        if (isValid) {
            var jsonData = { scheduleEmailModel: $scope.EmpEmailModel };
            AngularAjaxCall($http, HomeCareSiteUrl.SendEmpEmailURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $('#empemail-modal').modal('hide');
                    $scope.IsEmailModel = false;
                }
            });
        }
    };

    $scope.SendParentSMS = function () {
        var isValid = CheckErrors($("#parent-sms"));
        if (isValid) {
            var jsonData = { scheduleSmsModel: $scope.SmsModel };
            AngularAjaxCall($http, HomeCareSiteUrl.SendParentSMSURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $('#sms-modal').modal('hide');
                }
            });
        }
    };

    $scope.SetUp = function (schedule) {
        schedule.OpenEmailPopup = $scope.OpenEmailPopup;
        schedule.OpenScheduleNotificationLogModel = $scope.OpenScheduleNotificationLogModel;
        schedule.OpenSMSPopup = $scope.OpenSMSPopup;
        schedule.SendParentSMS = $scope.SendParentSMS;
        schedule.PrintNoticeScheduleNotification = $scope.PrintNoticeScheduleNotification;
        return schedule;
    };


    $scope.SetUpEmployeeDetails = function (schedule) {
        schedule.OpenEmpSMSPopup = $scope.OpenEmpSMSPopup;
        schedule.SendEmpSMS = $scope.SendEmpSMS;

        schedule.OpenEmpEmailPopup = $scope.OpenEmpEmailPopup;


        return schedule;
    };

    $scope.OpenEmpSMSPopup = function (item, phone) {
        var jsonData = { scheduleId: item.ScheduleID };
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmpSMSDetailURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ScheduleMasterModel.ScheduleSmsModel = response.Data;
                $scope.EmpSmsModel = angular.copy($scope.ScheduleMasterModel.ScheduleSmsModel);
                if (phone != undefined) {
                    $scope.EmpSmsModel.ToNumber = phone;
                }
                $scope.EmpSmsModel.ParentName = item.ParentName;
                $scope.EmpSmsModel.ReferralID = item.ReferralID;
                $scope.EmpSmsModel.Phone = item.Phone1;
                $('#emp-sms-modal').modal('show');
            } ShowMessages(response);
        });
    };
    $scope.SendEmpSMS = function () {
        
        var isValid = CheckErrors($("#emp-sms"));
        if (isValid) {
            var jsonData = { scheduleSmsModel: $scope.EmpSmsModel };
            AngularAjaxCall($http, HomeCareSiteUrl.SendEmpSMSURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $('#emp-sms-modal').modal('hide');
                }
            });
        }
    };



    $scope.SearchModelMapping = function () {
        $scope.SearchScheduleMasterListPage.StartDate = $scope.TempSearchScheduleMasterListPage.StartDate;
        $scope.SearchScheduleMasterListPage.EndDate = $scope.TempSearchScheduleMasterListPage.EndDate;
        $scope.SearchScheduleMasterListPage.ScheduleStatusID = $scope.TempSearchScheduleMasterListPage.ScheduleStatusID;
        $scope.SearchScheduleMasterListPage.Name = $scope.TempSearchScheduleMasterListPage.Name;
        $scope.SearchScheduleMasterListPage.ParentName = $scope.TempSearchScheduleMasterListPage.ParentName;
        $scope.SearchScheduleMasterListPage.DropOffLocation = $scope.TempSearchScheduleMasterListPage.DropOffLocation;
        $scope.SearchScheduleMasterListPage.FacilityID = $scope.TempSearchScheduleMasterListPage.FacilityID;
        $scope.SearchScheduleMasterListPage.EmployeeID = $scope.TempSearchScheduleMasterListPage.EmployeeID;
        $scope.SearchScheduleMasterListPage.RegionID = $scope.TempSearchScheduleMasterListPage.RegionID;
        $scope.SearchScheduleMasterListPage.LanguageID = $scope.TempSearchScheduleMasterListPage.LanguageID;
        $scope.SearchScheduleMasterListPage.PreferredCommunicationMethodID = $scope.TempSearchScheduleMasterListPage.PreferredCommunicationMethodID;
        $scope.SearchScheduleMasterListPage.ListOfIdsInCsv = $scope.TempSearchScheduleMasterListPage.ListOfIdsInCsv;
        $scope.SearchScheduleMasterListPage.AttendanceStatus = $scope.TempSearchScheduleMasterListPage.AttendanceStatus;
        if ($scope.ScheduleMasterModel.IsPartial) {
            $scope.SearchScheduleMasterListPage.ReferralID = $scope.EncryptedReferralID;
        }
    };

    $scope.GetScheduleMasterList = function (isSearchDataMappingRequire) {
        //
        //Reset Selcted Checkbox items and Control
        //$scope.SelectedScheduleMasterIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchScheduleMasterListPage.ListOfIdsInCsv = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.ScheduleMasterListPager.currentPage);
        $scope.AjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetDayCare_GetScheduleMasterListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $timeout(function () {
                    //
                    if (response.Data.CurrentPage == 1)
                        $scope.ScheduleMasterList = [];

                    if (response.Data.CurrentPage == 1 || $scope.ScheduleMasterListPager.lastPage < response.Data.CurrentPage)
                        Array.prototype.push.apply($scope.ScheduleMasterList, response.Data.Items);

                    $scope.SetIsChecked();
                    $scope.ScheduleMasterListPager.lastPage = response.Data.CurrentPage;
                    $scope.ScheduleMasterListPager.currentPageSize = response.Data.Items.length;
                    $scope.ScheduleMasterListPager.totalRecords = response.Data.TotalItems;

                    if (!$scope.$root.$$phase) {
                        $scope.$apply();
                    }
                });
            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.ScheduleMasterListPager.getDataCallback();
    };


    $scope.ResetSearchFilter = function () {
        $timeout(function () {

            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');

            $scope.SearchScheduleMasterListPage.StartDate = '';
            $scope.SearchScheduleMasterListPage.EndDate = '';
            $scope.SearchScheduleMasterListPage.ScheduleStatusID = '';
            $scope.SearchScheduleMasterListPage.Name = '';
            $scope.SearchScheduleMasterListPage.ParentName = '';
            $scope.SearchScheduleMasterListPage.DropOffLocation = '';
            $scope.SearchScheduleMasterListPage.FacilityID = '';
            $scope.SearchScheduleMasterListPage.EmployeeID = '';
            $scope.SearchScheduleMasterListPage.RegionID = '';
            $scope.SearchScheduleMasterListPage.LanguageID = '';
            $scope.SearchScheduleMasterListPage.PreferredCommunicationMethodID = '';


            $scope.TempSearchScheduleMasterListPage.StartDate = '';
            $scope.TempSearchScheduleMasterListPage.EndDate = '';
            $scope.TempSearchScheduleMasterListPage.ScheduleStatusID = '';
            $scope.TempSearchScheduleMasterListPage.Name = '';
            $scope.TempSearchScheduleMasterListPage.ParentName = '';
            $scope.TempSearchScheduleMasterListPage.DropOffLocation = '';
            $scope.TempSearchScheduleMasterListPage.FacilityID = '';
            $scope.TempSearchScheduleMasterListPage.EmployeeID = '';
            $scope.TempSearchScheduleMasterListPage.RegionID = '';
            $scope.TempSearchScheduleMasterListPage.LanguageID = '';
            $scope.TempSearchScheduleMasterListPage.PreferredCommunicationMethodID = '';


            $scope.ParentNameCount = 0;
            $scope.ClientNameCount = 0;
            $scope.StatusCount = 0;
            $scope.StartDateCount = 0;
            $scope.EndDateCount = 0;
            $scope.LocationCount = 0;
            $scope.FacilityCount = 0;
            $scope.RegionCount = 0;
            $scope.ScheduleMasterListPager.sortIndexArray = [];

            $scope.ScheduleMasterListPager.currentPage = 1;
            $scope.ScheduleMasterListPager.getDataCallback();
        });
    };

    $scope.ScheduleMasterListPager.resetCallback = function () {
        $scope.ParentNameCount = 0;
        $scope.ClientNameCount = 0;
        $scope.StatusCount = 0;
        $scope.StartDateCount = 0;
        $scope.EndDateCount = 0;
        $scope.LocationCount = 0;
        $scope.FacilityCount = 0;
        $scope.RegionCount = 0;
        $scope.ScheduleMasterListPager.sortIndexArray = [];
        $scope.ScheduleMasterListPager.currentPage = 1;
    };

    $scope.SearchScheduleMaster = function () {
        $scope.ScheduleMasterListPager.currentPage = 1;
        $scope.ScheduleMasterListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectScheduleMaster = function (schedule) {
        if (schedule.IsChecked)
            $scope.SelectedScheduleMasterIds.push(schedule.ScheduleID);
        else
            $scope.SelectedScheduleMasterIds.remove(schedule.ScheduleID);

        if ($scope.SelectedScheduleMasterIds.length == $scope.ScheduleMasterListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        //$scope.SelectedScheduleMasterIds = [];
        angular.forEach($scope.ScheduleMasterList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked) {
                var i = $.grep($scope.SelectedScheduleMasterIds, function (data) {
                    return item.ScheduleID == data;
                });
                if (i.length == 0) {
                    $scope.SelectedScheduleMasterIds.push(item.ScheduleID);
                }
            } else {
                $scope.SelectedScheduleMasterIds.remove(item.ScheduleID);
            }
        });
        return true;
    };
    $scope.SetIsChecked = function () {
        //$scope.SelectedScheduleMasterIds = [];
        angular.forEach($scope.ScheduleMasterList, function (item, key) {
            var i = $.grep($scope.SelectedScheduleMasterIds, function (data) {
                return item.ScheduleID == data;
            });
            if (i.length > 0) {
                item.IsChecked = true;
            }

        });

        return true;
    };

    $scope.DeleteSchedule = function (data) {
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchScheduleMasterListPage.ListOfIdsInCsv = data && data.ScheduleID > 0 ? data.ScheduleID.toString() : $scope.SelectedScheduleMasterIds.toString();

                if (data && data.ScheduleID > 0) {
                    if ($scope.ScheduleMasterListPager.currentPage != 1)
                        $scope.ScheduleMasterListPager.currentPage = $scope.ScheduleMasterList.length === 1 ? $scope.ScheduleMasterListPager.currentPage - 1 : $scope.ScheduleMasterListPager.currentPage;
                } else {

                    if ($scope.ScheduleMasterListPager.currentPage != 1 && $scope.SelectedScheduleMasterIds.length == $scope.ScheduleMasterListPager.currentPageSize)
                        $scope.ScheduleMasterListPager.currentPage = $scope.ScheduleMasterListPager.currentPage - 1;
                }
                $scope.SearchScheduleMasterListPage.IsShowListing = false;
                var jsonData = $scope.SetPostData($scope.ScheduleMasterListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DayCare_DeleteScheduleMasterURL, jsonData, "Post", "json", "application/json").success(function (response) {

                    if (response.IsSuccess) {
                        if (data && data.ScheduleID > 0) {
                            var index = $scope.ScheduleMasterList.indexOf(data);
                            $scope.ScheduleMasterList.splice(index, 1);
                            $scope.ScheduleMasterListPager.totalRecords -= 1;
                        } else {
                            angular.forEach($scope.SelectedScheduleMasterIds, function (scheduleMasterId) {
                                var item = $filter('filter')($scope.ScheduleMasterList, { ScheduleID: scheduleMasterId })[0];
                                if (item) {
                                    var index1 = $scope.ScheduleMasterList.indexOf(item);
                                    $scope.ScheduleMasterList.splice(index1, 1);
                                    $scope.ScheduleMasterListPager.totalRecords -= 1;
                                }
                            });
                        }

                        //$scope.ScheduleMasterList = response.Data.Items;
                        //$scope.ScheduleMasterListPager.currentPageSize = response.Data.Items.length;
                        //$scope.ScheduleMasterListPager.totalRecords = response.Data.TotalItems;


                        //Reset Selcted Checkbox items and Control
                        $scope.SelectedScheduleMasterIds = [];
                        $scope.SelectAllCheckbox = false;
                        //Reset Selcted Checkbox items and Control
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteConfirmationMessageForSchedule, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.ScheduleDetail = $scope.ScheduleMasterModel.ScheduleMaster;

    $scope.EditSchedule = function (data) {
        $scope.topper = document.body.scrollTop;
        $scope.CancelStatus = window.CancelStatus;
        $scope.ScheduleDetail = $.parseJSON(JSON.stringify(data));
    };

    $scope.SaveScheduleClick = function (item) {
        $scope.SaveSchedule(item);
    };

    $scope.SaveSchedule = function (schedule) {
        var isValid = CheckErrors($("#frmScheduleEdit"));
        if (isValid) {
            if (schedule != undefined && schedule.ScheduleId > 0) {
                if ($scope.ScheduleMasterListPager.currentPage != 1)
                    $scope.ScheduleMasterListPager.currentPage = $scope.ScheduleMasterList.length === 1 ? $scope.ScheduleMasterListPager.currentPage - 1 : $scope.ScheduleMasterListPager.currentPage;
            } else {

                if ($scope.ScheduleMasterListPager.currentPage != 1 && $scope.SelectedScheduleMasterIds.length == $scope.ScheduleMasterListPager.currentPageSize)
                    $scope.ScheduleMasterListPager.currentPage = $scope.ScheduleMasterListPager.currentPage - 1;
            }

            //Reset Selcted Checkbox items and Control
            $scope.SelectedScheduleMasterIds = [];
            $scope.SelectAllCheckbox = false;
            //Reset Selcted Checkbox items and Control
            var jsonData = $scope.SetPostData($scope.ScheduleMasterListPager.currentPage, schedule);
            AngularAjaxCall($http, HomeCareSiteUrl.UpdateScheduleMasterURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.ScheduleMasterList.filter(function (item, index) {
                        if (item.ScheduleID == schedule.ScheduleID) {
                            $scope.ScheduleMasterList[index] = response.Data.Items[0];
                        }
                    });

                    if (!$scope.$root.$$phase) {
                        $scope.$apply();
                    }

                    $('#EditSchedule').modal('hide');
                    //$timeout(function () {
                    //    if ($scope.topper > 0) {
                    //        window.scrollTo(0, $scope.topper);
                    //    }
                    //});


                    //if (schedule.ScheduleStatusID == window.CancelStatus) {
                    //    bootboxDialog(function (result) {
                    //        if (result) {
                    //            $timeout(function () {
                    //                $scope.ScheduleDetail.StartDate = schedule.StartDate; //$filter('date')(new Date(), 'L');
                    //                $scope.ScheduleDetail.EndDate = schedule.EndDate; //$filter('date')(new Date(), 'L');
                    //                $scope.ScheduleDetail.ScheduleStatusID = 0;
                    //                $scope.ScheduleDetail.Comments = null;

                    //            });
                    //            $('#RescheduleClient').modal('show');
                    //        }
                    //    }, bootboxDialogType.Confirm, "Reschedule", schedule.IsReschedule ? window.AlreadyRescheduleConfirm : window.RescheduleConfirm, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);

                    //}
                }

            });

        }
    };

    $scope.DatePickerDate = function (modelDate, newDate) {
        var a;
        if (modelDate) {
            var dt = new Date(modelDate);
            dt >= newDate ? a = newDate : a = dt;
        } else {
            a = newDate;
        }
        return moment(a).format('L');
    };

    $scope.ReSchedule = function (schedule) {
        var isValid = CheckErrors($("#frmReScheduleClient"));

        if (isValid) {
            if (schedule != undefined && schedule.ScheduleId > 0) {
                if ($scope.ScheduleMasterListPager.currentPage != 1)
                    $scope.ScheduleMasterListPager.currentPage = $scope.ScheduleMasterList.length === 1 ? $scope.ScheduleMasterListPager.currentPage - 1 : $scope.ScheduleMasterListPager.currentPage;
            } else {

                if ($scope.ScheduleMasterListPager.currentPage != 1 && $scope.SelectedScheduleMasterIds.length == $scope.ScheduleMasterListPager.currentPageSize)
                    $scope.ScheduleMasterListPager.currentPage = $scope.ScheduleMasterListPager.currentPage - 1;
            }

            //Reset Selcted Checkbox items and Control
            $scope.SelectedReferralIds = [];
            $scope.SelectAllCheckbox = false;
            //Reset Selcted Checkbox items and Control
            var model = schedule;
            var jsonData = $scope.SetPostData($scope.ScheduleMasterListPager.currentPage, model);
            AngularAjaxCall($http, SiteUrl.ReScheduleClientURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    //$scope.ScheduleMasterList = response.Data.Items;
                    //$scope.ScheduleMasterListPager.currentPageSize = response.Data.Items.length;
                    //$scope.ScheduleMasterListPager.totalRecords = response.Data.TotalItems;
                    //$timeout(function () {
                    //    if ($scope.topper > 0) {
                    //        window.scrollTo(0, $scope.topper + 10);
                    //    }
                    //});

                    //var item = $filter('filter')($scope.ScheduleMasterList, { ScheduleID: schedule.ScheduleID })[0];
                    //if (item) {
                    //    var index = $scope.ScheduleMasterList.indexOf(item);
                    //    if (index > 0) {
                    //        $scope.ScheduleMasterList[index] = schedule;
                    //    }
                    //}
                    $('#RescheduleClient').modal('hide');
                }

            });
        }
    };

    $scope.OpenScheduleBatchServicePopup = function (serviceType, header) {
        $scope.AddScheduleBatchService = $scope.NewInstance().AddScheduleBatchService;
        $scope.AddScheduleBatchService.ScheduleBatchServiceType = serviceType;
        bootboxDialog(function (result) {
            if (result) {
                $scope.SaveScheduleBatchService();
            }
        }, bootboxDialogType.Confirm, header, ScheduleBatchServiceConfirmation, bootboxDialogButtonText.YesContinue, btnClass.BtnSuccess);
        //$('#AddScheduleBatchServicePopup').modal('show');
    };

    $scope.SaveScheduleBatchService = function () {
        //if (CheckErrors($("#frmAddScheduleBatchService"))) {
        $scope.AddScheduleBatchService.ScheduleIDs = $scope.SelectedScheduleMasterIds.join(",");
        var jsonData = angular.toJson({ batchService: $scope.AddScheduleBatchService });
        AngularAjaxCall($http, SiteUrl.SaveScheduleBatchServiceURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $('#AddScheduleBatchServicePopup').modal('hide');
                $scope.SelectAllCheckbox = false;
                $scope.SelectAll();
                $scope.SelectedScheduleMasterIds = [];
                $scope.AddScheduleBatchService = $scope.NewInstance().AddScheduleBatchService;
            }
        });
        //}
    };

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
    $scope.EmployeeFilter = function (value) {
        return function (item) {
            if (item.IsDeleted == 0 || item.EmployeeID == value) {
                return item;
            }
        };
    };

    //$scope.$watch(function () {
    //    return $scope.ScheduleDetail.FacilityID;
    //}, function (newVal) {
    //    
    //    if (newVal && (newVal == parseInt(window.WaitingListPHX) || newVal == parseInt(window.WaitingListTUC) || newVal == parseInt(window.WaitingListYuma))) {
    //        $scope.ScheduleDetail.ScheduleStatusID = parseInt(window.WaitingListStatus);
    //    }
    //});

    $scope.ScheduleMasterListPager.getDataCallback = $scope.GetScheduleMasterList;

    $("a#referralHistory").on('shown.bs.tab', function (e) {
        $(".tab-pane a[href='#tab_ReferralScheduleHistory']").tab('show');
        $scope.GetScheduleMasterList();
    });

    if (!$scope.ScheduleMasterModel.IsPartial) {
        $scope.GetScheduleMasterList();
    }


    $(window).scroll(function () {
        if ($(window).scrollTop() == $(document).height() - $(window).height() && !$scope.AjaxStart) {
            $scope.ScheduleMasterListPager.nextPage();
        }
    });

    $scope.LoadMore = function () {
        $scope.ScheduleMasterListPager.nextPage();
    }





    $scope.AttendanceActionClick = function (item) {

        $scope.ScheduleAttendaceDetail = {};
        $scope.ScheduleAttendaceDetail.ScheduleID = item.ScheduleID;
        $scope.ScheduleAttendaceDetail.ReferralID = item.ReferralID;
        $scope.ScheduleAttendaceDetail.AbsentReason = item.AbsentReason;
        $scope.ScheduleAttendaceDetail.TempIsPatientAttendedSchedule = item.IsPatientAttendedSchedule;

        $('#schAttendenceModal').modal({ backdrop: false, keyboard: false });
        $("#schAttendenceModal").modal('show');
    }

    $scope.SavePatientAttendance = function () {
        var isValid = CheckErrors($("#frmschAttendenceModal"));
        if (isValid) {

            $scope.ScheduleAttendaceDetail.IsPatientAttendedSchedule = $scope.ScheduleAttendaceDetail.TempIsPatientAttendedSchedule;

            var jsonData = angular.toJson($scope.ScheduleAttendaceDetail);
            AngularAjaxCall($http, HomeCareSiteUrl.DayCare_SavePatientAttendanceURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.ScheduleMasterListPager.getDataCallback();
                    $("#schAttendenceModal").modal('hide');
                    
                }
                ShowMessages(response);
            });
        }
    };

    $scope.ClosePatientAttendanceActionModal = function () {
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
        $("#schAttendenceModal").modal('hide');
        HideErrors("#frmschAttendenceModal");
    };


    $scope.UploadPatientCSV = function () {
        $('#uploadpatientcsvmodal').modal({ backdrop: false, keyboard: false });
        $("#uploadpatientcsvmodal").modal('show');
    }

    $('#uploadpatientcsvmodal').on('hidden.bs.modal', function(e) {
        $scope.ScheduleMasterListPager.getDataCallback();
    });

};
controllers.ScheduleMasterListController.$inject = ['$scope', '$http', '$timeout', '$filter'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowScheduleMasterMessage");
    //$(".dateInputMask").inputmask("m/d/y", {
    //    placeholder: "mm/dd/yyyy"
    //});
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});
