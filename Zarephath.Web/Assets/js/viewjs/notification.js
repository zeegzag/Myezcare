

controllers.NotificationController = function ($scope, $http) {
    $scope.LoginUrl = "/security/index";
    $scope.RegenerateVerificationLinkUrl = "/security/regenerateverificationlink";
    $scope.IsNotVerifiedEmail = false;

    var modelJson = $.parseJSON($("#NotificationModel").val());
    $scope.NotificationModel = modelJson;

    $scope.EmailModel = {
        Email: $scope.NotificationModel.Email
    };

    $scope.ResendVerificationLink = function () {
        var jsonData = angular.toJson($scope.EmailModel);
        AngularAjaxCall($http, $scope.RegenerateVerificationLinkUrl, jsonData, "Post", "json", "application/json").
            success(function (response, status, headers, config) {
                window.location = $scope.LoginUrl;
                SetMessageForPageLoad(response.Message, "ShowSignUpMessage");
            });
    };
};

controllers.NotificationController.$inject = ['$scope', '$http'];
