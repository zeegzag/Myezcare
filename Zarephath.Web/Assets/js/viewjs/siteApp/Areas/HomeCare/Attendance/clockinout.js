var vm;
controllers.clockinoutController = function ($scope, $http, $window, $timeout) {
    vm = $scope;
    $scope.SaveclockinoutURL = HomeCareSiteUrl.SaveclockinoutURL;
    $scope.AjaxStart = false;
    $scope.AttendanceDetailType = 0;
    $scope.SetClockInOutModel = $.parseJSON($("#hdnClockInOutModel").val());
    var btnClockIn = document.getElementById("btnClockIn");
    btnClockIn.disabled = $scope.SetClockInOutModel.EmployeeAttendanceMaster && $scope.SetClockInOutModel.EmployeeAttendanceMaster.FacilityID != 0 ? false : true;
    $scope.SaveAttendance = function (type) {
        debugger
        $scope.AjaxStart = true;
        if (type == 1) {
            $scope.SetClockInOutModel.EmployeeAttendanceDetail = [];
            $scope.SetClockInOutModel.EmployeeAttendanceDetail.push({ 'Type': 1 });
        }
        if (type == 3 || type == 2) {
            $scope.SetClockInOutModel.EmployeeAttendanceDetail = [];
            $scope.SetClockInOutModel.EmployeeAttendanceDetail.push({ 'Type': type });
        }
        if (type == 4) {
            $scope.SetClockInOutModel.EmployeeAttendanceDetail = [];
            $scope.SetClockInOutModel.EmployeeAttendanceDetail.push({ 'Type': type });
        }
        var jsonData = angular.toJson($scope.SetClockInOutModel);
        AngularAjaxCall($http, HomeCareSiteUrl.SaveclockinoutURL, jsonData, "Post", "json", "application/json").success(function (response) {
            //debugger;
            if (response.IsSuccess) {
                $scope.SetClockInOutModel = response.Data;
                $scope.getDataCallback();
            }        
            setTimeout(function () {
                $scope.AjaxStart = false;
                lyScope.RefreshClockInOut();
            }, 2000);
            //ShowMessages(response);
        });
    };
    $scope.GetAttendance = function () {
        //GetclockinoutURL
        //debugger;
        var jsonData = {};
        AngularAjaxCall($http, HomeCareSiteUrl.GetclockinoutURL, jsonData, "Post", "json", "application/json").success(function (response) {
           // debugger;
            if (response.IsSuccess) {
                $scope.SetClockInOutModel = response.Data;
                if ($scope.SetClockInOutModel.EmployeeAttendanceMaster != null && $scope.SetClockInOutModel.EmployeeAttendanceMaster.FacilityID != undefined) {
                    $scope.SetClockInOutModel.EmployeeAttendanceMaster.FacilityID = '' + $scope.SetClockInOutModel.EmployeeAttendanceMaster.FacilityID;
                }
                if ($scope.SetClockInOutModel.EmployeeAttendanceDetail != null && $scope.SetClockInOutModel.EmployeeAttendanceDetail.length > 0) {
                    try {
                        //$scope.AttendanceDetailType = $scope.SetClockInOutModel.EmployeeAttendanceDetail[0].Type;
                        $scope.AttendanceDetailType = $scope.SetClockInOutModel.EmployeeAttendanceDetail[$scope.SetClockInOutModel.EmployeeAttendanceDetail.length - 1].Type;
                    }
                    catch (e) { }
                }
            }
            //ShowMessages(response);
        });
    };
    $scope.AttendanceDetailName = function (AttendanceDetailId) {
        var AttendanceDetailName = '';
        $scope.SetClockInOutModel.AttendanceDetailList.forEach(function (item, index) {

            if (item.AttendanceDetailId == AttendanceDetailId) {
                AttendanceDetailName = item.AttendanceDetailName;
            }
        });
        return AttendanceDetailName;
    }
    $scope.AttendanceBreakDetailName = function (AttendanceBreakDetailId) {
        var AttendanceBreakDetailId = '';
        $scope.SetClockInOutModel.AttendanceBreakDetailList.forEach(function (item, index) {

            if (item.AttendanceBreakDetailId == AttendanceBreakDetailId) {
                AttendanceBreakDetailId = item.AttendanceBreakDetailId;
            }
        });
        return AttendanceBreakDetailId;
    }
    $scope.GetDate = function (d) {
        var a = '' + d;
        var b = new Date(a.match(/\d+/)[0] * 1);
        return moment(b).format();
    }
    $scope.FacilityChange = function (data) {
        
        if (data.SetClockInOutModel.EmployeeAttendanceMaster.FacilityID != 0)
           btnClockIn.disabled = false;
        else
            btnClockIn.disabled = true;
    }

    $scope.getDataCallback = $scope.GetAttendance;
    $scope.getDataCallback();
};
controllers.clockinoutController.$inject = ['$scope', '$http', '$window', '$timeout'];

$(document).ready(function () {

});
