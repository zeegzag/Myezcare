var custModel;

controllers.LoginController = function ($scope, $http) {
    custModel = $scope;
    sessionStorage.clear();
    localStorage.clear();

    $scope.LoginUrl = "/security/login";
    $scope.DashboardUrl = "/hc/home/dashboard";
    $scope.SecurityQuestionUrl = "/security/securityquestion";
    $scope.OnBoardingURL = "/hc/onboarding/getstarted/Organization Details";
    $scope.RegenerateVerificationLinkUrl = "/security/regenerateverificationlink";
    $scope.GetAnnouncementUrl = "/security/GetAnnouncement";
    
    var modelJson = $.parseJSON($("#hdnLoginModel").val());
    $scope.LoginModel = modelJson;

    $scope.EmailModel = {
        Email: ""
    };

    $scope.Login = function () {
        // 

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
            AngularAjaxCall($http, $scope.LoginUrl, jsonData, "Post", "json", "application/json").
                success(function (response) {
                    if (response.IsSuccess) {
                        SetMessageForPageLoad(response.Message, "LoginSuccessMessage");
                        if (retrunUrl.length > 0) {
                            window.location = retrunUrl;
                        } else {
                            if (!response.Data.SessionValueData.IsSecurityQuestionSubmitted) {
                                window.location = $scope.SecurityQuestionUrl;
                            }
                            //else if (!response.Data.SessionValueData.IsCompletedWizard) {
                            //    window.location = $scope.OnBoardingURL;
                            //}
                            else {
                                window.location = $scope.DashboardUrl;
                            }
                        }
                    }

                    else {
                        //
                        if (response.Data === window.ShowCaptch || response.Data === window.AccountLocked) {
                            $scope.ShowCaptcha = true;
                            grecaptcha.reset();
                            ShowMessages(response);

                            if (response.Data === window.AccountLocked) {
                                $scope.AccountLocked = true;
                            }

                            return false;
                        }
                        $scope.ShowCaptcha = false;

                        if (response.Data !== null) {
                            if (response.Data.IsNotVerifiedEmail) {
                                ShowMessages(response);
                                $scope.IsNotVerifiedEmail = true;
                                $scope.EmailModel.Email = response.Data.Email;
                            } else {
                                window.location = $scope.SignUpUrl;
                            }
                        } else {
                            ShowMessages(response);
                        }
                    }
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
    $scope.showPassword = false;
    $scope.ViewPassword = function () {
        $scope.showPassword = !$scope.showPassword;
    }

};

controllers.LoginController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowResetPasswordMessage");
    ShowPageLoadMessage("ShowSecurityQuestionMessage");
    ShowPageLoadMessage("ShowCreateLoginMessage");
});
