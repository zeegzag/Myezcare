var custModel;

controllers.ScheduleEmailController = function ($scope, $http) {
    custModel = $scope;
    $scope.IsEdit = false;
    $scope.ScheduleEmailModel = $.parseJSON($("#hndScheduleMasterModelCancelPage").val());
    $scope.NotificationModel = null;
    $scope.IsShow = true;

    $scope.UpdateScheduleEmailDetails = function () {
        if (CheckErrors("#frmEmailCancellation")) {
            
            var jsonData = angular.toJson({
                cancelEmailDetailModel: {
                    ScheduleMaster: $scope.ScheduleEmailModel.ScheduleMaster,
                    EncryptedMailMessageToken: $scope.ScheduleEmailModel.EncryptedMailMessageToken
                }
            });
            AngularAjaxCall($http, SiteUrl.UpdateScheduleCancelstatus, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.NotificationModel = response.Data;
                    $scope.IsShow = false;
                }
            });
        }
    };
};

controllers.ScheduleEmailController.$inject = ['$scope', '$http'];