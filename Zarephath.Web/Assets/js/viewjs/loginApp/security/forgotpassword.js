var custModel;

controllers.ForgotPasswordController = function ($scope, $http) {
    custModel = $scope;

    
    $scope.SaveForgotPasswordURL = "/security/saveforgotpassword";
    $scope.ResetPasswordURL = "/security/resetpassword";
    $scope.LoginURL = "/security/index";
    $scope.GetSecurityQuestionURL = "/security/getsecurityquestion";

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

    //Fetch Security Question 20220709 RN
    $scope.getSecurityQuestion = function () {
        // alert('a'); 
            var jsonData = angular.toJson({ ForgotPasswordDetailModel: $scope.ForgotPasswordModel.ForgotPasswordDetailModel });

        AngularAjaxCall($http, $scope.GetSecurityQuestionURL, jsonData, "post", "json", "application/json").
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                       // $scope.ForgotPasswordModel.ForgotPasswordDetailModel.SecurityQuestionID = response.Data;
                       // alert($scope.ForgotPasswordModel.ForgotPasswordDetailModel.SecurityQuestionID); 
                        $scope.ForgotPasswordModel.ForgotPasswordDetailModel.SecurityQuestionID = response.Data.SecurityQuestionID;
                        $scope.ForgotPasswordModel.ForgotPasswordDetailModel.Question = response.Data.Question;

                        SetMessageForPageLoad(response.Message, "ShowSecurityQuestionMessage");
                             } else {
                        ShowMessages(response);
                    }

                    //ShowMessages(response);

                });
        
    };
};

controllers.ForgotPasswordController.$inject = ['$scope', '$http'];