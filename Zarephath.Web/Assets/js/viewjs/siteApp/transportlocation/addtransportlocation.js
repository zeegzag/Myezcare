var custModel;

controllers.AddtransportationLocationController = function ($scope, $http, $window) {
    custModel = $scope;
    $scope.IsEdit = false;
    $scope.TransPortationModel = $.parseJSON($("#hdnTransPortationModel").val());

    if (($scope.TransPortationModel != null || $scope.TransPortationModel != undefined) && $scope.TransPortationModel.TransportLocationID > 0) {
        $scope.IsEdit = true;
    }

    $scope.SaveTransPortationLocationDetails = function () {
        if (CheckErrors("#frmTransPortationLocation")) {
            var jsonData = angular.toJson({
                transportlocation: {
                    TransportLocation: $scope.TransPortationModel.TransportLocation
                }
            });
            AngularAjaxCall($http, SiteUrl.TransPortationModelListURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    
                    SetMessageForPageLoad(response.Message, "TransportationUpdateSuccessMessage");
                    $window.location = SiteUrl.GetTransPortationListURL;
                }
                else {
                    ShowMessages(response);
                }
            });
        }
    };

    //Upload Image File
    $scope.UploadFile = SiteUrl.CommonUploadFileUrl;
    $scope.Internal = window.Internal;
    $scope.External = window.External;
    $scope.InternalShort = window.InternalShort;
    $scope.ExternalShort = window.ExternalShort;
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

            if ((file.size / 1024) > parseInt(window.FileSize)) {
                file.FileProgress = 100;
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
        $scope.TransPortationModel.TransportLocation.MapImage = model.FilePath;//model.Data.TempFilePath;//model.Data.items.TempFilePath;
        $scope.$apply();
    };

    $scope.Cancel = function () {
        $window.location = SiteUrl.GetTransPortationListURL;
    };
};

controllers.AddtransportationLocationController.$inject = ['$scope', '$http', '$window'];

$(document).ready(function () {
    ShowPageLoadMessage("TransportationUpdateSuccessMessage");
});