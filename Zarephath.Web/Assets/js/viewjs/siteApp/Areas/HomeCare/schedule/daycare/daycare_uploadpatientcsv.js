var scopeRefCsv;


controllers.UploadRefCsvController = function ($scope, $http, $compile, $timeout, $filter) {
    scopeRefCsv = $scope;

    $scope.UploadFile = HomeCareSiteUrl.SaveReferralCSVFileUrl;
    $scope.UploadingFileList = [];

    $scope.BeforeSend = function (e, data) {
        var isValidFile = true;
        var fileName;
        var errorMsg;

        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            if (extension !== "xls" && extension !== "xlsx") {
                ShowMessage(window.InvalidCSVUploadMessage.replace("{0}", extension), "error");
                errorMsg = window.InvalidCSVUploadMessage.replace("{0}", extension);
                isValidFile = false;
            }

            fileName = file.name;
        });

        if (isValidFile) {
            $scope.IsFileUploading = true;
        }
        if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
            $scope.$apply();
        }
        var response = { IsSuccess: isValidFile, Message: errorMsg };
        return response;
    };

    $scope.Progress = function (e, data) {
        myApp.showPleaseWait();
        console.log(data.files[0].name);
    };

    $scope.SelectFileLabel = window.SelectCSVFile;

    $scope.AfterSend = function (e, data) {
        var model = angular.fromJson(data.result);
        $scope.IsFileUploading = false;
        if (model.IsSuccess) {
            $scope.ReferralCsvModel.FilePath = model.Data.TempFilePath;
            $scope.SelectFileLabel = model.Data.FileOriginalName;
            $scope.UploadingFileList = [];

            
            ShowMessage(model.Message);
        } else {
            ShowMessage(model);
        }
        myApp.hidePleaseWait();
        $scope.$apply();
    };

    $scope.CreateBulkScheduleUsingCSV = function () {

        var isValid = CheckErrors($("#ReferralCsvFrm"));
        
        if ($scope.ReferralCsvModel==null || $scope.ReferralCsvModel.FilePath == null || $scope.ReferralCsvModel.FilePath == undefined) {
            $("#excelUpload").addClass("tooltip-danger");
            $scope.errorMsg = true;
            isValid = false;
        } else {
            $("#excelUpload").removeClass("tooltip-danger");
            $scope.errorMsg = false;
        }

        if (isValid) {
            var jsonData = angular.toJson($scope.ReferralCsvModel);

            AngularAjaxCall($http, HomeCareSiteUrl.CreateBulkScheduleUsingCSVURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    if (typeof (scopeEmpRefSch)  != "undefined" && scopeEmpRefSch.SchedulesCreated)
                        $scope.SeachAndGenerateCalenders();
                    $scope.ReferralCsvModel = null;
                    $scope.ErrorList = [];
                    $("#uploadpatientcsvmodal").modal('hide');
                    $scope.SelectFileLabel = window.SelectCSVFile;
                }
                else {
                    if (response.Data != null || response.Data != undefined)
                        $scope.ErrorList = response.Data;
                }
                ShowMessages(response);
            });
        }
    }

    $('#uploadpatientcsvmodal').on('hidden.bs.modal', function (e) {
        // do something...
        //
        if (typeof (scopeEmpRefSch) != "undefined" && scopeEmpRefSch.SchedulesCreated)
            $scope.SeachAndGenerateCalenders();
        $scope.ReferralCsvModel = null;
        $scope.errorMsg = false;
        $scope.ErrorList = [];
        $scope.SelectFileLabel = window.SelectCSVFile;
        HideErrors("#ReferralCsvFrm");
    });

};

controllers.UploadRefCsvController.$inject = ['$scope', '$http', '$compile', '$timeout', '$filter'];

var wall;
$(document).ready(function () {


});
