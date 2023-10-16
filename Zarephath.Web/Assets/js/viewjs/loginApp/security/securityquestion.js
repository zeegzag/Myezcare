var custModel;

controllers.SetSecurityQuestionController = function ($scope, $http) {
    custModel = $scope;

    $scope.SaveSecurityQuestionURL = "/security/savesecurityquestion";
    $scope.DashboardURL = "/hc/home/dashboard";
    $scope.Wizard = "/hc/Onboarding/GetStarted/Organization Details";

    var modelJson = $.parseJSON($("#hdnSecurityQuestionModel").val());

    $scope.SecurityQuestionModel = modelJson;

    $scope.Save = function () {
        var isValid = CheckErrors($("#frmSecurityQuestion"));
        if (isValid) {
            var jsonData = angular.toJson({ SecurityQuestionDetailModel: $scope.SecurityQuestionModel.SecurityQuestionDetailModel });

            AngularAjaxCall($http, $scope.SaveSecurityQuestionURL, jsonData, "post", "json", "application/json").
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        SetMessageForPageLoad(response.Message, "ShowSecurityQuestionMessage");
                        if (!response.Data) {
                            window.location = $scope.Wizard;
                        } else {
                            window.location = $scope.DashboardURL;
                        }
                    } else {
                        ShowMessages(response);
                    }

                });
        }
    };

    $scope.UploadFile = SiteUrl.CommonUploadFileUrl;
    $scope.UploadingFileList = [];
    $scope.BeforeSend = function (e, data) {
        var isValidImage = true;
        var fileName;
        var errorMsg;

        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            if (extension !== "jpg" && extension !== "jpeg" && extension !== "png" && extension !== "bmp") {
                ShowMessage(window.InvalidImageUploadMessage.replace("{0}", extension), "error");
                isValidImage = false;
            }
            else if ((file.size / 1024) > 2048) {
                //file.FileProgress = 100;
                ShowMessage(window.MaximumUploadImageSizeMessage, "error");
                errorMsg = window.MaximumUploadImageSizeMessage;
                isValidImage = false;
            }
            fileName = file.name;
        });

        if (isValidImage) {
            $scope.IsFileUploading = true;
        }
        $scope.$apply();
        var response = { IsSuccess: isValidImage, Message: errorMsg };
        return response;
    };

    $scope.Progress = function (e, data) {
        console.log(data.files[0].name);
    };

    $scope.AfterSend = function (model) {
        //ShowMessages(data.result);
        $scope.IsFileUploading = false;
        //var model = data.result;
        $scope.SecurityQuestionModel.SecurityQuestionDetailModel.TempSignaturePath = model.FilePath;//model.Data.TempFilePath;//model.Data.items.TempFilePath;
        $scope.UploadingFileList = [];
        $scope.$apply();
    };


};

controllers.SetSecurityQuestionController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    ShowPageLoadMessage("LoginSuccessMessage");
});