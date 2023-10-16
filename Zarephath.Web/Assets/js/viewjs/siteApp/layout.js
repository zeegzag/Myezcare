var lyScope;

controllers.LayoutDetailController = function ($scope, $http, $window, $timeout) {

    lyScope = $scope;

    $scope.ReferralURL = SiteUrl.AddReferralPageUrl;
    $scope.NoteUrl = SiteUrl.NoteUrl;
    $scope.SetClockInOutModel = {};
    $scope.AttendanceDetailType = 0;
    $scope.GetLayoutRelatedDetails = function () {

        var newCheckTime = GetCookie("NewCheckTime");
        var newCheckTimeForPendingVisit = GetCookie("NewCheckTimeForPendingVisit");

        if (newCheckTime == "null")
            newCheckTime = null;

        if (newCheckTimeForPendingVisit == "null")
            newCheckTimeForPendingVisit = null;

        var searchModel = {
            PageSize: 50,
            AssigneeLastCheckTime: newCheckTime,
            ResolvedLastCheckTime: newCheckTime,
            PendingVisitLastCheckTime: newCheckTimeForPendingVisit
        };
        var jsonData = angular.toJson(searchModel);

        AngularAjaxCall($http, SiteUrl.GetLayoutRelatedDetailsUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {
            $scope.LayoutDetailModel = response.Data;
            SetCookie($scope.LayoutDetailModel.NewCheckTime, "NewCheckTime");
            SetCookie($scope.LayoutDetailModel.NewCheckTimeForPendingVisit, "NewCheckTimeForPendingVisit");

            if (response.Data.NewMessagesCount > 0) {
                ShowMessage(response.Data.NewMessagesCountMessage, "info", 0, 0);
                GenerateDesktopNotification(response.Data.NewMessagesCountMessage, HomeCareSiteUrl.GetDashboardUrl);
            }
            if (response.Data.ResolvedMessagesCount > 0) {
                ShowMessage(response.Data.ResolvedMessagesCountMessage, "warning", 0, 0);
                GenerateDesktopNotification(response.Data.ResolvedMessagesCountMessage, HomeCareSiteUrl.GetDashboardUrl);
            }
            if (response.Data.PendingVisitCount > 0 && response.Data.CanHaveApprovePermission) {
                ShowMessage(response.Data.PendingVisitCountMessage, "info", 0, 0);
                GenerateDesktopNotification(response.Data.PendingVisitCountMessage, HomeCareSiteUrl.GetPendingEmployeeVisitListURL);
            }
        });
    };

    // 1 MIN = 60000 MS
    //setInterval(function () {
    //    $scope.GetLayoutRelatedDetails();
    //}, 60000);

    // $scope.GetLayoutRelatedDetails();

    $scope.ViewMoreClick = function (ele, count) {
        if (parseInt(count) > 0) {
            $('html, body').animate({
                scrollTop: $(ele).offset().top - 50
            }, 500);
        }
    };



    var rn_cookieName = window.RN_CookieName;
    $scope.ShowReleaseNote = false;
    $scope.CheckForShowReleaseNote = function () {
        if (ValideElement(window.frameElement)) { return; }
        var cmId = GetCookieWithoutRemoving(rn_cookieName);
        if (ValideElement(cmId) && cmId === "false") {
            $scope.ShowReleaseNote = false;
        } else {
            //
            $scope.ShowReleaseNote = true;
            $(".releaseNote").removeClass("display-none");
        }
    };
    $scope.CheckForShowReleaseNote();

    $scope.CloseReleaseNote = function () {

        SetCookie(false, rn_cookieName, 365); //expires after 365 days
        $scope.ShowReleaseNote = false;
    };

    $scope.ChangeOrgType = function (data) {
        $scope.OrgType = data;
        var jsonData = angular.toJson({ OrgType: data });
        AngularAjaxCall($http, HomeCareSiteUrl.ChangeOrgTypeURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            location.reload();
        });
    };

    $scope.NotificationModel = function () {
        $('#Notification').modal('show');
    }
    $scope.NotificationList = [];
    $scope.GetNotification = function () {
        if (ValideElement(window.frameElement)) { return; }
        var jsonData = angular.toJson({});
        AngularAjaxCall($http, HomeCareSiteUrl.GetLateClockInNotificationURL, jsonData, "Get", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.NotificationList = response.Data;
            }
        });
    };
    $scope.GetNotification();

    $scope.remindMeLaterAfter30Min = function () {
        setInterval(function () {

            $("#PaymentStatusModal2").show();

        }, 9000);
    };

    $scope.payNowNavigation = function () {
        //var url = ReportURL + ReportName;
        var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,width=' + window.outerWidth / 1.2 + ',height=' + window.outerHeight / 1 + ', overflow= hidden';
        var pdfWindow = window.open('about:blank', 'null', winFeature);
        pdfWindow.document.write("<html><head><title>Invoice</title></head><body>"
            + '<embed width="100%" height="1000px" name="plugin" src="/hc/Invoice/CompanyClientInvoice"'
            + 'type="application/pdf" internalinstanceid="21"></body></html>');
        pdfWindow.document.close();
        //$('#frmReport').attr('src', ReportURL+ ReportName);
        //$('#ReportModal').modal('show');

    };

    //For dark and light Themes Start
    $scope.AddmyEzCarelightClass = function () {
        angular.element('#myEzCarelight').removeClass("myEzCarelight");
    }
    $scope.ActivePatientReport = function () {
        window.location.href = "/hc/report/reportmaster?reportName=ActivePatient";
    }
    $scope.InvoicePayNotification = function () {
        $("#PaymentStatusModal4").modal("show");
        $("#PaymentStatusModal33").modal("hide");
    }
    $scope.CallCaptureModel = function (id, title) {
        if (id === 1) {
            $('#CallCaptureModelDDLBindIFrame').attr('src', HomeCareSiteUrl.AddCaptureCall + id);
        }
        else {
            $('#CallCaptureModelDDLBindIFrame').attr('src', HomeCareSiteUrl.CaptureCallList);
        }

        $('#AddCallCaptureModel').modal({ backdrop: 'static', keyboard: false });

    }

    $scope.NotificationEditModel = function () {
        $('#sidebar').modal('show');
        $scope.GetWebNotificationList();
    }

    $scope.NotificationModelClosed = function () {
        $('#sidebar').modal('hide');
        $scope.GetWebNotificationCount();
    }

    $scope.GetWebNotificationCount = function () {
        var jsonData = angular.toJson({});
        $scope.WebNotificationCountAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetWebNotificationCountURL, jsonData, "Get", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                var oldCount = -1;
                if ($scope.WebNotificationCount) {
                    oldCount = $scope.WebNotificationCount.length;
                }
                $scope.WebNotificationCount = response.Data;
                if ($scope.WebNotificationCount.length > 0) {
                    $scope.WebNotificationsCount = $scope.WebNotificationCount[0].MsgCount;
                }
                var newCount = $scope.WebNotificationCount.length;
                if (oldCount != -1 && oldCount < newCount) {
                    ShowMessage(NewNotificationsCountMessage.replace('{0}', (newCount - oldCount).toString()), 'info');
                }
            }
            $scope.WebNotificationCountAjaxStart = false;
        });
    };

    $scope.GetWebNotificationList = function () {
        if (ValideElement(window.frameElement)) { return; }
        var jsonData = angular.toJson({});
        $scope.WebNotificationListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetWebNotificationListURL, jsonData, "Get", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                var oldCount = -1;
                if ($scope.WebNotificationList) {
                    oldCount = $scope.WebNotificationList.length;
                }
                $scope.WebNotificationList = response.Data;
                if ($scope.WebNotificationList.length > 0) {
                    $scope.WebNotificationListCount = $scope.WebNotificationList[0].MsgCount;
                }
                var newCount = $scope.WebNotificationList.length;
                if (oldCount != -1 && oldCount < newCount) {
                    ShowMessage(NewNotificationsCountMessage.replace('{0}', (newCount - oldCount).toString()), 'info');
                }
            }
            $scope.WebNotificationListAjaxStart = false;
        });
    };

    //$scope.GetWebNotificationList();
    $scope.GetWebNotificationCount();

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

    $scope.DeleteWebNotification = function (id) {
        var jsonData = angular.toJson({});
        AngularAjaxCall($http, HomeCareSiteUrl.DeleteWebNotificationURL + id, jsonData, "Get", "json", "application/json", false).success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.GetWebNotificationList();
            }
        });
    };

    $scope.ViewAllNotification = function () {
        $window.open('/hc/home/webnotification');
    };
    $scope.RefreshClockInOut = function () {
        var jsonData = {};
        AngularAjaxCall($http, HomeCareSiteUrl.GetclockinoutURL, jsonData, "Post", "json", "application/json")
            .success(function (response) {
                if (response.IsSuccess) {
                    $scope.SetClockInOutModel = response.Data;
                    if ($scope.SetClockInOutModel.EmployeeAttendanceDetail != null && $scope.SetClockInOutModel.EmployeeAttendanceDetail.length > 0) {
                        $scope.AttendanceDetailType = $scope.SetClockInOutModel.EmployeeAttendanceDetail[$scope.SetClockInOutModel.EmployeeAttendanceDetail.length-1].Type;
                    }
                    if ($scope.AttendanceDetailType != 2) {
                        $scope.setTime();
                    }
                    else {
                        //$scope.timer = "00:00:00";
                        var totalminutes = $scope.GetCountOfMinutes($scope.SetClockInOutModel.EmployeeAttendanceDetail[0].CreatedDate, $scope.SetClockInOutModel.EmployeeAttendanceDetail[$scope.SetClockInOutModel.EmployeeAttendanceDetail.length - 1].CreatedDate)

                        $scope.timer = totalminutes;
                        $scope.Expiry = true;
                    }
                    
                }
            });
    }

    $scope.RefreshClockInOut();
    $scope.startTime = 0;
    $scope.endTime = 0;
    $scope.currentTime = 0;
    $scope.timeDiff = 0;
    $scope.timer = "00:00:00";
    $scope.Expiry = false;
    

    $scope.setTime = () => {

        $scope.startTime = (($scope.AttendanceDetailType != 0) ?
            ($scope.GetCountOfMinutes($scope.SetClockInOutModel.EmployeeAttendanceDetail[0].CreatedDate, $scope.SetClockInOutModel.CurrentTimeZoneDate)) : '00:00:00');
        $scope.timer = $scope.startTime;
        $scope.countDown();

        // ------------------- Old Funcation
           //var totalminutes = $scope.SetClockInOutModel.EmployeeAttendanceMaster.WorkMinutes +
        //    (($scope.AttendanceDetailType == 4 || $scope.AttendanceDetailType != 1) ?
        //    moment().diff($scope.GetDate($scope.SetClockInOutModel.EmployeeAttendanceDetail[0].CreatedDate), 'minutes') : 0);
      
        //$scope.startTime = '' +
        //    (parseInt(totalminutes / 60) < 10 ? '0' + parseInt(totalminutes / 60) : parseInt(totalminutes / 60))
        //    + ':'
        //    + (parseInt(totalminutes % 60) < 10 ? '0' + parseInt(totalminutes % 60) : parseInt(totalminutes % 60))
        //    + ':00'; //document.getElementById("timeStart").value;
        //document.getElementById("timer").innerHTML = $scope.startTime;
        //$scope.timer = $scope.startTime;
        //$scope.countDown();
      
    }    
    
    $scope.timeCount = () => {
        $scope.currentTime = $scope.timer;

        if ($scope.currentTime != "00:00:00") {
            $scope.startTime = moment($scope.timer, "hh:mm:ss");
            console.log($scope.startTime);
        } else {
            $scope.startTime = moment(document.getElementById("timeStart").value, "hh:mm:ss");
        }

        $scope.endTime = moment(document.getElementById("timeEnd").value, "hh:mm:ss");

        $scope.timeDiff = $scope.endTime.diff($scope.startTime);

        $scope.timer = setInterval($scope.countDown, 1000);
        
    }
    
    $scope.countDown = () => {
        if ($scope.AttendanceDetailType == 1 || $scope.AttendanceDetailType == 4) {
            console.log("Clockin");
            //$scope.startTime.add(1, "second");
            $scope.startTime = moment($scope.startTime, "hh:mm:ss").add(1, "second").format("HH:mm:ss");
            $scope.timer = $scope.startTime;
            //document.getElementById("timer").innerHTML = startTime.format("HH:mm:ss");
            $scope.timeDiff = moment("23:59:59", "hh:mm:ss").diff(moment($scope.startTime, "hh:mm:ss"));

            //Testing..for 1 min
            //$scope.timeDiff = moment("00:01:00", "hh:mm:ss").diff(moment($scope.startTime, "hh:mm:ss"));

            if ($scope.timeDiff > 0) {
                //console.log("no expiry:" + $scope.timeDiff);
                $timeout($scope.countDown, 1000);
            }
            else {
                $scope.timer = "00:00:00";
                $scope.Expiry = true;

                $scope.SetClockInOutModel.EmployeeAttendanceDetail = [];
                $scope.SetClockInOutModel.EmployeeAttendanceDetail.push({ 'Type': 2 });

                var jsonData = angular.toJson($scope.SetClockInOutModel);
                AngularAjaxCall($http, HomeCareSiteUrl.SaveclockinoutURL, jsonData, "Post", "json", "application/json").success(function (response) {

                    if (response.IsSuccess) {
                        $scope.SetClockInOutModel = response.Data;                        
                    }
                    $scope.AjaxStart = false;
                    $scope.RefreshClockInOut();
                    window.location.reload();
                });
            }
        }
        
    }
    $scope.GetDate = function (d) {
        var a = '' + d;
        var b = new Date(a.match(/\d+/)[0] * 1);
        return moment(b);
    }
    $scope.GetConvertedDate = function (d) {
        var a = '' + d;
        var b = new Date(a.match(/\d+/)[0] * 1);
        console.log(moment(b))
        return b;
    }

    $scope.GetCountOfMinutes = function (fromMinute, toMinute) {
        var startDate = $scope.GetConvertedDate(fromMinute);
        var endDate = $scope.GetConvertedDate(toMinute);
        var milisecondsDiff = endDate - startDate;

        return Math.floor(milisecondsDiff / (1000 * 60 * 60)).toLocaleString(undefined, { minimumIntegerDigits: 2 }) + ":" + (Math.floor(milisecondsDiff / (1000 * 60)) % 60).toLocaleString(undefined, { minimumIntegerDigits: 2 }) + ":" + (Math.floor(milisecondsDiff / 1000) % 60).toLocaleString(undefined, { minimumIntegerDigits: 2 });

    }
};

controllers.LayoutDetailController.$inject = ['$scope', '$http', '$window', '$timeout'];

function getTime() {
    return new Date().getTime();
}

var LAST_ACCESS_TIME_KEY = '__STORE_lastAccess'
var IDLE_TIMEOUT_COUNTER_KEY = '__STORE_idleCounter'
var IDLE_TIMEOUT = 600; //in seconds (10 min)
var _idleSecondsTimer = null;
localStorage.setItem(LAST_ACCESS_TIME_KEY, getTime());

document.onclick = function () {
    localStorage.setItem(LAST_ACCESS_TIME_KEY, getTime());
};

document.onmousemove = function () {
    localStorage.setItem(LAST_ACCESS_TIME_KEY, getTime());
};

document.onkeypress = function () {
    localStorage.setItem(LAST_ACCESS_TIME_KEY, getTime());
};

if (!window.frameElement) {
    function CheckIdleTime() {
        var _idleSecondsCounter = parseInt(localStorage.getItem(LAST_ACCESS_TIME_KEY) || getTime());
        _idleSecondsCounter = (getTime() - _idleSecondsCounter) / 1000;
        localStorage.setItem(IDLE_TIMEOUT_COUNTER_KEY, _idleSecondsCounter);
        if (_idleSecondsCounter >= IDLE_TIMEOUT) {
            window.clearInterval(_idleSecondsTimer);
            window.location = HomeCareSiteUrl.LogoutURL;
        }
    }

    _idleSecondsTimer = window.setInterval(CheckIdleTime, 1000);
}
