var custModel;

controllers.EditProfileController = function ($scope, $http) {
    custModel = $scope;

    $scope.EditProfileModel = $.parseJSON($("#hdnEditProfileModel").val());
    $scope.SaveEditProfileURL = "/security/saveeditprofile";

    $scope.Save = function () {

        var isValid = CheckErrors($("#frmEditProfile"));

        if (isValid) {
            var jsonData = angular.toJson({ Employee: $scope.EditProfileModel.Employee });

            AngularAjaxCall($http, $scope.SaveEditProfileURL, jsonData, "post", "json", "application/json").
                success(function (response, status, headers, config) {

                    if (response.IsSuccess) {
                        SetMessageForPageLoad(response.Message, "ShowEditProfileMessage");
                        location.reload();
                    }
                    ShowMessages(response);
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
        $scope.EditProfileModel.Employee.TempSignaturePath = model.FilePath;//model.Data.items.TempFilePath;
        $scope.UploadingFileList = [];
        $scope.$apply();
    };
   //Add profile Image start
    $scope.UploadProfileImage = SiteUrl.UploadProfileImageUrls;
    $scope.UploadingFileList = [];
    $scope.ProfileImageBeforeSend = function (e, data) {
        var isValidImage = true;
        var fileName;
        var errorMsg;

        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            if (extension !== "jpg" && extension !== "jpeg" && extension !== "png" && extension !== "bmp") {
                ShowMessage(window.InvalidImageUploadMessage, "error");
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

    $scope.ProfileImageProgress = function (e, data) {
        console.log(data.files[0].name);
    };

    $scope.ProfileImageAfterSend = function (e, data) {
        $scope.IsFileUploading = false;
        var model = data.result;
        if (model.IsSuccess) {
            $scope.EditProfileModel.Employee.ProfileImagePath = model.Data.TempFilePath;
            $scope.UploadingFileList = [];
        } else {
            ShowMessage(model);
        }
        $scope.$apply();
        window.location.reload();
    };

    //  Add profile imahe end
};
controllers.EditProfileController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowEditProfileMessage");
});