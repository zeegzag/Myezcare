var custModel;

controllers.LoginController = function ($scope, $http) {
    custModel = $scope;

    $scope.LoginUrl = "/security/login";
    $scope.DashboardUrl = "/home/dashboard";
    $scope.SecurityQuestionUrl = "/security/securityquestion";
    $scope.RegenerateVerificationLinkUrl = "/security/regenerateverificationlink";
    
    var modelJson = $.parseJSON($("#hdnLoginModel").val());
    $scope.LoginModel = modelJson;

    $scope.EmailModel = {
        Email: ""
    };

    $scope.Login = function () {
        if ($scope.ShowCaptcha) {
            var response = grecaptcha.getResponse();
            if (!ValideElement(response)) {
                $scope.ShowCaptchaError = true;
                return false;
            } else {
                $scope.ShowCaptchaError = false;
            }
        }

        var isValid = CheckErrors($("#frmLogin"));
        if (isValid) {
            var jsonData = angular.toJson($scope.LoginModel);
            var retrunUrl = getParam('returnUrl');
            AngularAjaxCall($http, SiteUrl.LoginUrl, jsonData, "Post", "json", "application/json").
                success(function (response) {
                    if (response.IsSuccess) {
                        SetMessageForPageLoad(response.Message, "LoginSuccessMessage");
                        if (retrunUrl.length > 0) {
                            window.location = retrunUrl;
                        } else {
                            window.location = $scope.DashboardUrl;

                            //if (!response.Data.SessionValueData.IsSecurityQuestionSubmitted) {
                            //    window.location = $scope.SecurityQuestionUrl;
                            //}
                            //else {
                            //    window.location = $scope.DashboardUrl;
                            //}
                        }
                    }
                    else {
                        ShowMessages(response);
                    }
                    //else {
                    //    //debugger;
                    //    if (response.Data === window.ShowCaptch || response.Data === window.AccountLocked) {
                    //        $scope.ShowCaptcha = true;
                    //        grecaptcha.reset();
                    //        ShowMessages(response);

                    //        if (response.Data === window.AccountLocked) {
                    //            $scope.AccountLocked = true;
                    //        }

                    //        return false;
                    //    }
                    //    $scope.ShowCaptcha = false;

                    //    if (response.Data !== null) {
                    //        if (response.Data.IsNotVerifiedEmail) {
                    //            ShowMessages(response);
                    //            $scope.IsNotVerifiedEmail = true;
                    //            $scope.EmailModel.Email = response.Data.Email;
                    //        } else {
                    //            window.location = $scope.SignUpUrl;
                    //        }
                    //    } else {
                    //        ShowMessages(response);
                    //    }
                    //}
                });
        }
    };


//    grecaptcha.getResponse(
//opt_widget_id
//)


    $scope.ResendVerificationLink = function () {

        var jsonData = angular.toJson({ email: $scope.EmailModel.Email });
        AngularAjaxCall($http, $scope.RegenerateVerificationLinkUrl, jsonData, "Post", "json", "application/json").
            success(function (response, status, headers, config) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.IsNotVerifiedEmail = false;
                }
            });
    };
};

controllers.LoginController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowResetPasswordMessage");
    ShowPageLoadMessage("ShowSecurityQuestionMessage");
});
