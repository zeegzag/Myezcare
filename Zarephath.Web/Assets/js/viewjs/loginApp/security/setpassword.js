var custModel;

controllers.SetPasswordController = function ($scope, $http) {
    custModel = $scope;

    var modelJson = $.parseJSON($("#hdnLoginModel").val());
    $scope.SetPasswordModel = modelJson;

    $scope.SetPassword = function () {
        var isValid = CheckErrors($("#frmSetPassword"));
        if (isValid) {
            var jsonData = angular.toJson($scope.SetPasswordModel);
            var retrunUrl = getParam('returnUrl');
            AngularAjaxCall($http, HomeCareSiteUrl.SaveSetPasswordUrl, jsonData, "Post", "json", "application/json").
                success(function (response) {
                    if (response.IsSuccess) {

                        var msg =  response.Data.IsAdmin ? window.PasswordSetSuccessfullyForAdmin : window.PasswordSetSuccessfullyForOther;

                        bootboxDialog(function (res) {
                            window.location = response.Data.RedirectUrl;
                        }, bootboxDialogType.Alert, bootboxDialogTitle.Alert,
                        msg, bootboxDialogButtonText.Ok);

                    }
                    else {
                        ShowMessages(response);
                    }

                });
        }
    }

};

controllers.SetPasswordController.$inject = ['$scope', '$http'];