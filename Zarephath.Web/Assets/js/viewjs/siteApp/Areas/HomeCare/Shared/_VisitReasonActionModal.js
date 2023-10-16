var vram;


controllers.VisitReasonActionModalController = function ($scope, $http, $timeout, $window) {
    vram = $scope;

    $scope.data = {};
    $scope.options = {};
    $scope.byPassModal = false;
    $scope.VisitReasons = [];
    $scope.VisitActions = [];

    $scope.ReasonTypes = { edit: { title: 'Edit Visit Reason', ReasonType: 'EditReason', ActionType: 'EditAction' }, cancel: { title: 'Cancel' }, delete: { title: 'Delete' } };

    $scope.SetTypeOptions = function () {
        var reason = $scope.ReasonTypes[$scope.options.ReasonType];
        if (reason) {
            if (!$scope.options.Title) {
                $scope.options.Title = reason.title;
            }
            $scope.options.Type = {};
            $scope.options.Type.ReasonType = reason.ReasonType;
            $scope.options.Type.ActionType = reason.ActionType;
        }
    };

    $scope.SetOptions = function (options, data) {
        var defaultOptions = { CompanyName: data.ClaimProcessor, ReasonType: 'edit', ScheduleID: 0, OnCancel: function (data, save) { }, OnSet: function (data, save) { } };
        $scope.options = Object.assign(defaultOptions, data || {}, options || {});
        $scope.SetTypeOptions();
        $scope.byPassModal = $window.ByPassVisitReasonActionModal || $scope.options.ByPassModal;
        if (!$scope.byPassModal) {
            $scope.GetVisitReasons();
            $scope.GetVisitActions();
            $('#visitReasonActionModal').modal({ backdrop: 'static', keyboard: false });
        } else {
            $scope.options.OnSet($scope.GetData(), $scope.SaveVisitReason);
        }
    }

    $scope.GetVisitReasonModalDetail = function (options, callback) {
        var jsonData = angular.toJson({ ScheduleID: options.ScheduleID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetVisitReasonModalDetailURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                callback(options, response.Data);
            }
        });
    };

    $scope.GetData = function () {
        var data = {};
        if (!$scope.byPassModal) {
            data = Object.assign($scope.data, { ReasonType: $scope.options.ReasonType });
        }
        return data;
    };

    $scope.CloseVisitReasonActionModal = function () {
        $("#visitReasonActionModal").modal('hide');
        $scope.data.Event = 'cancel';
        $scope.options.OnCancel($scope.GetData(), $scope.SaveVisitReason);
    };

    $scope.SaveVisitReason = function () {
        if (!$scope.byPassModal && $scope.options.ScheduleID > 0) {
            var jsonData = angular.toJson({ ScheduleID: $scope.options.ScheduleID, ReasonType: $scope.options.ReasonType, ReasonCode: $scope.data.ReasonCode, ActionCode: $scope.data.ActionCode, CompanyName: $scope.options.CompanyName });
            AngularAjaxCall($http, HomeCareSiteUrl.SaveVisitReasonURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
            });
        }
    };

    $scope.SetVisitReason = function () {
        var isValid = CheckErrors($("#frmVisitReason"));
        if (isValid) {
            $("#visitReasonActionModal").modal('hide');
            $scope.data.Event = 'set';
            $scope.options.OnSet($scope.GetData(), $scope.SaveVisitReason);
        }
    };

    $scope.GetVisitReasonList = function (type, companyName, callback) {
        var jsonData = angular.toJson({ Type: type, CompanyName: companyName });
        AngularAjaxCall($http, HomeCareSiteUrl.GetVisitReasonListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                callback(response.Data);
            }
        });
    };

    $scope.GetVisitReasons = function () {
        if (!$scope.options.HideReasonType) {
            $scope.GetVisitReasonList($scope.options.Type.ReasonType, $scope.options.CompanyName, function (data) {
                $scope.VisitReasons = data;
            });
        }
    };

    $scope.GetVisitActions = function () {
        if (!$scope.options.HideActionType) {
            $scope.GetVisitReasonList($scope.options.Type.ActionType, $scope.options.CompanyName, function (data) {
                $scope.VisitActions = data;
            });
        }
    };

    $scope.Show = function (options) {
        $scope.data = {};
        $scope.GetVisitReasonModalDetail(options, $scope.SetOptions);
    };
};

controllers.VisitReasonActionModalController.$inject = ['$scope', '$http', '$timeout', '$window'];

$(document).ready(function () {

});

function ShowVisitReasonActionModal(options) {
    var $scope = vram;
    $scope.Show(options);
}