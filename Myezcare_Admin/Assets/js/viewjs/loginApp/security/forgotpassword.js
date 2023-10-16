var custModel;

controllers.ForgotPasswordController = function ($scope, $http) {
    custModel = $scope;

    debugger;
    $scope.SaveForgotPasswordURL = "/security/saveforgotpassword";
    $scope.ResetPasswordURL = "/security/resetpassword";
    $scope.LoginURL = "/security/index";

    var modelJson = $.parseJSON($("#ForgotPasswordHiddenModel").val());

    $scope.ForgotPasswordModel = modelJson;

    $scope.Save = function () {

        var isValid = CheckErrors($("#frmForgotPassword"));

        if (isValid) {

            var jsonData = angular.toJson({ ForgotPasswordDetailModel: $scope.ForgotPasswordModel.ForgotPasswordDetailModel, IsUnlockAccountPage: $scope.ForgotPasswordModel.IsUnlockAccountPage });

            AngularAjaxCall($http, $scope.SaveForgotPasswordURL, jsonData, "post", "json", "application/json").
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        SetMessageForPageLoad(response.Message, "ShowSecurityQuestionMessage");
                        window.location = $scope.LoginURL;
                        //if (response.Data != null && response.Data.ForgotPasswordDetailModel != null && response.Data.ForgotPasswordDetailModel.EncryptedValue != null)
                        //    window.location = $scope.ResetPasswordURL + "/" + response.Data.ForgotPasswordDetailModel.EncryptedValue;

                    } else {
                        ShowMessages(response);
                    }

                    //ShowMessages(response);

                });
        }
    };
};

controllers.ForgotPasswordController.$inject = ['$scope', '$http'];