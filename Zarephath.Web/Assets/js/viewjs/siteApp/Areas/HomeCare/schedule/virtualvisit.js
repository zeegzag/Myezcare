var vm;

controllers.VirtualVisitsListController = function ($scope, $http, $window, $timeout) {
    vm = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetVirtualVisitsListModel").val());
    };
    $scope.SelectedVirtualVisitsIds = [];
    $scope.VirtualVisitsList = [];
    $scope.SelectAllCheckbox = false;
    $scope.VirtualVisitsModel = $.parseJSON($("#hdnSetVirtualVisitsListModel").val());
    $scope.SearchVirtualVisitsModel = $scope.newInstance().SearchVirtualVisitsListModel;

    $scope.TempSearchVirtualVisitsModel = $scope.newInstance().SearchVirtualVisitsListModel;
    $scope.SearchReferralNotesModel = {};
    $scope.GetReferralNotesByModelURL = HomeCareSiteUrl.GetReferralNotesByModelURL;

    $scope.VirtualVisitsListPager = new PagerModule("EmployeeName");

    $scope.ActiveTabId = function (fromIndex, model) {
        return $("ul#visitTypes li.active").first().attr('id');
    };

    $scope.SetPostData = function (fromIndex, model) {
        $scope.SearchVirtualVisitsModel.VisitType = $scope.ActiveTabId();
        var pagermodel = {
            model: $scope.SearchVirtualVisitsModel,
            pageSize: $scope.VirtualVisitsListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.VirtualVisitsListPager.sortIndex,
            sortDirection: $scope.VirtualVisitsListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchVirtualVisitsModel = $.parseJSON(angular.toJson($scope.TempSearchVirtualVisitsModel));
        $scope.SearchVirtualVisitsModel.EmployeeID = null;
        var actTabId = $scope.ActiveTabId();
        var isTodayVisits = actTabId == HashUrl_VirtualVisits_TodaysVisits;
        var isFutureVisits = actTabId == HashUrl_VirtualVisits_FutureVisits;
        var isPastVisits = actTabId == HashUrl_VirtualVisits_PastVisits;
        $scope.SearchVirtualVisitsModel.StartMinDate = isTodayVisits || isFutureVisits ? new Date() : null;
        $scope.SearchVirtualVisitsModel.StartMaxDate = isTodayVisits || isPastVisits ? new Date() : null;
        $scope.SearchVirtualVisitsModel.EndMinDate = isTodayVisits || isFutureVisits ? new Date() : null;
        $scope.SearchVirtualVisitsModel.EndMaxDate = isTodayVisits || isPastVisits ? new Date() : null;
    };
    $scope.SearchModelMapping();

    $scope.GetVirtualVisitsList = function () {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedVirtualVisitsIds = [];
        $scope.SelectAllCheckbox = false;
        //Reset Selcted Checkbox items and Control
        $scope.AjaxStart = true;
        var jsonData = $scope.SetPostData($scope.VirtualVisitsListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.HC_GetVirtualVisitsListURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
            if (response.IsSuccess) {
                $scope.VirtualVisitsList = response.Data.Items;
                $scope.VirtualVisitsListPager.currentPageSize = response.Data.Items.length;
                $scope.VirtualVisitsListPager.totalRecords = response.Data.TotalItems;
                //$scope.ShowCollpase();
            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };    

    $scope.Refresh = function () {
        $scope.VirtualVisitsListPager.getDataCallback();
    };

    $scope.SearchVirtualVisits = function () {
        $scope.VirtualVisitsListPager.currentPage = 1;
        $scope.VirtualVisitsListPager.getDataCallback(true);
    };

    $scope.ResetSearchFilter = function () {
        $scope.SearchModelMapping();
        $scope.SearchVirtualVisits();
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedVirtualVisitsIds = [];

        angular.forEach($scope.VirtualVisitsList, function (item, key) {

            item.IsChecked = $scope.SelectAllCheckbox;// event.target.checked;
            if (item.IsChecked)
                $scope.SelectedVirtualVisitsIds.push(item.VirtualVisitsID);
        });

        return true;
    };

    $scope.OpenJoinMeeting = function (referralid, scheduleid) {
        var domain = GetDomainOnly();
        SetCookie(cAppCompanyName, '__CLIENT_APP_CompanyName_', undefined, domain);
        SetCookie(cAppToken, '__CLIENT_APP_Token_', undefined, domain);
        SetCookie(cAppUserName, '__CLIENT_APP_UserName_', undefined, domain);
        var link = cAppURL + "/#/upcoming-visit-details/" + referralid + "/" + scheduleid;
        var width = screen.availWidth - 10;
        var height = screen.availHeight - 60;
        var left = 0;//(screen.availWidth - width) / 2;
        var top = 0;//(screen.availHeight - height) / 2;
        var params = 'width=' + width + ', height=' + height;
        params += ', top=' + top + ', left=' + left;
        var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,' + params + ',overflow=hidden';
        window.open(link, '_blank', winFeature);
    };

    $scope.SendReminder = function (scheduleid, referralid, employeeid, sendSMS, sendEmail) {
        sendSMS = sendSMS == false ? sendSMS : true;
        sendEmail = sendEmail == false ? sendEmail : true;
        $scope.AjaxStart = true;
        var jsonData = JSON.stringify({ scheduleID: scheduleid, referralID: referralid, employeeID: employeeid, sendSMS, sendEmail });
        AngularAjaxCall($http, HomeCareSiteUrl.SendScheduleReminderUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {
            //if (response.IsSuccess) {
            //}
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };
    $scope.ChangeScheduleEmployeeList = [];
    $scope.ChangeSchedule = function (data) {
        $scope.ScheduleTimeDetail = {};
        $scope.ScheduleTimeDetail.ScheduleID = data.ScheduleID;
        $scope.ScheduleTimeDetail.ReferralID = data.ReferralID;
        $scope.ScheduleTimeDetail.ReferralName = data.ReferralName;
        $scope.ScheduleTimeDetail.StartTime = data.StartTime;
        $scope.ScheduleTimeDetail.EndTime = data.EndTime;
        $scope.ScheduleTimeDetail.ScheduleDate = data.StartDate;
        $scope.ScheduleTimeDetail.EmployeeID = data.EmployeeID ? data.EmployeeID.toString() : "";

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
        var isValid = CheckErrors($("#frmChangeSchedule"));
        var StartTime = moment($scope.ScheduleTimeDetail.StartTime, 'h:mma');
        var EndTime = moment($scope.ScheduleTimeDetail.EndTime, 'h:mma');
        if (isValid) {
            if (StartTime.isBefore(EndTime))
            {
                ShowVisitReasonActionModal({
                    ScheduleID: $scope.ScheduleTimeDetail.ScheduleID,
                    OnSet: function (data, save) {
                        var model = Object.assign({}, $scope.ScheduleTimeDetail);
                        model.StartTime = moment(StartTime).format('hh:mm A');
                        model.EndTime = moment(EndTime).format('hh:mm A');
                        var jsonData = angular.toJson(model);
                        AngularAjaxCall($http, HomeCareSiteUrl.SaveNewScheduleURL, jsonData, "Post", "json", "application/json").success(function (response) {
                            if (response.IsSuccess) {
                                save();
                                $scope.CloseScheduleModal();
                                $scope.Refresh();
                            }
                            ShowMessages(response);
                        });
                    }
                });
            }
            else { ShowMessage("Start Time can not be greater than End Time.", "error");}

        };
    }

    $scope.CloseScheduleModal = function () {
        $("#ChangeScheduleModal").modal('hide');
        HideErrors("#frmChangeSchedule");
    };

    $scope.OpenCancelVisitModal = function (scheduleID) {
        $scope.CancelVisitDetail = {};
        $scope.CancelVisitDetail.ScheduleID = scheduleID;

        $('#CancelVisitModal').modal({ backdrop: false, keyboard: false });
        $("#CancelVisitModal").modal('show');
    };

    $scope.CancelVisit = function () {
        var isValid = CheckErrors($("#frmCancelVisit"));
        if (isValid) {
            var jsonData = { scheduleID: $scope.CancelVisitDetail.ScheduleID, reason: $scope.CancelVisitDetail.CancelReason };
            AngularAjaxCall($http, HomeCareSiteUrl.DeleteScheduleFromCalenderURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.CloseCancelVisitModal();
                    $scope.Refresh();
                }
                ShowMessages(response);
            });
        };
    }

    $scope.CloseCancelVisitModal = function () {
        $("#CancelVisitModal").modal('hide');
        HideErrors("#frmCancelVisit");
    };

    $scope.VirtualVisitsListPager.getDataCallback = $scope.GetVirtualVisitsList;
    $scope.VirtualVisitsListPager.getDataCallback();

    $('#visitTypes a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $scope.ResetSearchFilter();
    });
};
controllers.VirtualVisitsListController.$inject = ['$scope', '$http', '$window', '$timeout'];
