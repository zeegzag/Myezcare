var custModel;


controllers.AddEmployeeController = function ($scope, $http) {

    custModel = $scope;

    $scope.EmployeeModel = $.parseJSON($("#hdnEmployeeModel").val());

    $scope.Save = function () {

        var isValid = CheckErrors($("#frmAddEmployee"));
        if (isValid) {
            var jsonData = angular.toJson({ Employee: $scope.EmployeeModel.Employee });

            AngularAjaxCall($http, SiteUrl.AddEmployeeURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        SetMessageForPageLoad(response.Message, "ShowEmployeeMessage");
                        window.location = SiteUrl.EmployeelistURL;
                    } else {
                        ShowMessages(response);
                    }

                });
        }
    };


    //#region ADD Employee Signature
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
        $scope.EmployeeModel.Employee.TempSignaturePath = model.FilePath;//model.Data.items.TempFilePath;
        $scope.UploadingFileList = [];
        $scope.$apply();
    };

    //#endregion
};

controllers.AddEmployeeController.$inject = ['$scope', '$http'];
