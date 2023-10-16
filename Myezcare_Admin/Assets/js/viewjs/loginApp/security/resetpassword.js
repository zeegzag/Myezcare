var custModel;

controllers.ResetPasswordController = function ($scope, $http) {
    custModel = $scope;

    $scope.SaveResetPasswordURL = "/security/saveresetpassword";
    $scope.LoginURL = "/security/index";

    var modelJson = $.parseJSON($("#hdnResetPasswordModel").val());

    $scope.ResetPasswordModel = modelJson;

    $scope.Save = function () {

        var isValid = CheckErrors($("#frmResetPassword"));

        if (isValid) {
            var jsonData = angular.toJson({ model: $scope.ResetPasswordModel });

            AngularAjaxCall($http, $scope.SaveResetPasswordURL, jsonData, "post", "json", "application/json").
                success(function (response, status, headers, config) {

                    if (response.IsSuccess) {
                        SetMessageForPageLoad(response.Message, "ShowResetPasswordMessage");

                        window.location = $scope.LoginURL;

                    } else {
                        ShowMessages(response);
                    }

                });
        }
    };
};
controllers.ResetPasswordController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowSecurityQuestionMessage");
});