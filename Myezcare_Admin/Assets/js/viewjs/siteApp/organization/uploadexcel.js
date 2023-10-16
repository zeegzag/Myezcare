var scopeRefCsv;


controllers.UploadExcelController = function ($scope, $http, $compile, $timeout, $filter) {
    scopeRefCsv = $scope;

    $scope.ImportDataTypeModel = $.parseJSON($("#hdnImportModel").val());

    $scope.UploadFile = SiteUrl.SaveFileUrl;
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
            $scope.ImportDataTypeModel.FilePath = model.Data.TempFilePath;
            $scope.SelectFileLabel = model.Data.FileOriginalName;
            $scope.UploadingFileList = [];
            ShowMessage(model.Message);
        } else {
            ShowMessage(model);
        }
        myApp.hidePleaseWait();
        $scope.$apply();
    };

    $scope.ImportDataInDatabase = function () {

        var isValid = CheckErrors($("#ImportFrm"));
        
        if ($scope.ImportDataTypeModel==null || $scope.ImportDataTypeModel.FilePath == null || $scope.ImportDataTypeModel.FilePath == undefined) {
            $("#excelUpload").addClass("tooltip-danger");
            $scope.errorMsg = true;
            isValid = false;
        } else {
            $("#excelUpload").removeClass("tooltip-danger");
            $scope.errorMsg = false;
        }

        if (isValid) {
            if ($scope.ImportDataTypeModel == undefined) {
                $scope.ImportDataTypeModel = {};
            }
            $scope.ImportDataTypeModel.EncryptedOrganizationID = $scope.SelectedEncryptedOrganizationID;
            var jsonData = angular.toJson($scope.ImportDataTypeModel);

            AngularAjaxCall($http, SiteUrl.ImportDataInDatabaseUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    if (typeof (scopeEmpRefSch)  != "undefined" && scopeEmpRefSch.SchedulesCreated)
                        $scope.SeachAndGenerateCalenders();
                    $scope.ImportDataTypeModel = null;
                    $scope.Patients = [];
                    $scope.PatientContacts = [];
                    $scope.Employees = [];
                    $("#uploadmodal").modal('hide');
                    $scope.SelectFileLabel = window.SelectCSVFile;
                }
                else {
                    if (response.Data != null || response.Data != undefined) {
                        if ($scope.ImportDataTypeModel.ImportDataType == 'Patient') {
                            $scope.Patients = response.Data.Patients;
                            $scope.PatientContacts = response.Data.PatientContacts;
                        } else if ($scope.ImportDataTypeModel.ImportDataType == 'Employee') {
                            $scope.Employees = response.Data.Employees;
                        }
                    }
                }
                ShowMessages(response);
            });
        }
    }

    $('#uploadmodal').on('hidden.bs.modal', function (e) {
        // do something...
        //debugger;
        if (typeof (scopeEmpRefSch) != "undefined" && scopeEmpRefSch.SchedulesCreated)
            $scope.SeachAndGenerateCalenders();
        $scope.ImportDataTypeModel = null;
        $scope.errorMsg = false;
        $scope.ErrorList = [];
        $scope.SelectFileLabel = window.SelectCSVFile;
        HideErrors("#ImportFrm");
    });

};

controllers.UploadExcelController.$inject = ['$scope', '$http', '$compile', '$timeout', '$filter'];

var wall;
$(document).ready(function () {


});
