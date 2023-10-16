var vm;
controllers.AddInvoiceController = function ($scope, $http, $window, $timeout) {
    vm = $scope;
    $scope.AllMonthList = [];
    $scope.OrganizationList = [];
    $scope.PlanList = [];


    //____________________________________Predefine_Data_______________________________
    $scope.AllMonthList.push({ Name: "January", Id: "1" });
    $scope.AllMonthList.push({ Name: "February", Id: "2" });
    $scope.AllMonthList.push({ Name: "March", Id: "3" });
    $scope.AllMonthList.push({ Name: "April", Id: "4" });
    $scope.AllMonthList.push({ Name: "May", Id: "5" });
    $scope.AllMonthList.push({ Name: "June", Id: "6" });
    $scope.AllMonthList.push({ Name: "July", Id: "7" });
    $scope.AllMonthList.push({ Name: "August", Id: "8" });
    $scope.AllMonthList.push({ Name: "September", Id: "9" });
    $scope.AllMonthList.push({ Name: "October", Id: "10" });
    $scope.AllMonthList.push({ Name: "November", Id: "11" });
    $scope.AllMonthList.push({ Name: "December", Id: "12" });

    $scope.PlanList.push({ Name: "Basic", Id: "1" });
    $scope.PlanList.push({ Name: "Essential", Id: "2" });
    $scope.PlanList.push({ Name: "Free Forever", Id: "3" });
    $scope.PlanList.push({ Name: "Plus", Id: "4" });

    //____________________________________Predefine_Data-END_______________________________


    $scope.SaveInvoice = function () {


        var isValid = CheckErrors($("#frmSaveInvoice"));
        var isVliadation = $scope.validationAmount();

        if (isValid && isVliadation) {
            

            if ($("#invoiceFileId") == undefined || $("#invoiceFileId").length <= 0 || $("#invoiceFileId")[0] == undefined || $("#invoiceFileId")[0] == null || $("#invoiceFileId")[0].files.length <= 0 || $("#invoiceFileId")[0].files[0] == undefined || $("#invoiceFileId")[0].files[0] == null) {
                ShowMessage("File is required", "error", 5000, 5000, undefined, false);
                return;
            }
            let fileExtn = "";
            $.each($("#invoiceFileId")[0].files, function (index, file) {
                debugger
                var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
                //if (extension == "exe") {
                fileExtn = extension;
                if (extension != "pdf") {

                    //ShowMessage("Please add PDF file", "error");
                    isValidImage = false;
                    return;
                }
                if ((file.size / 1024) > parseInt(window.FileSize)) {
                    file.FileProgress = 100;
                    errorMsg = window.MaximumUploadImageSizeMessage;
                    isValidImage = false;
                }
            });
            if ("pdf" != fileExtn) {
                ShowMessage("Please add PDF file", "error");
                return;
            }
            switch ($scope.InvoiceModel.InvoiceStatus) {
                case '1':
                    $scope.InvoiceModel.IsPaid = true;
                    break;
                case '2':
                    $scope.InvoiceModel.IsPaid = false;
                    break;
                case '3':
                    $scope.InvoiceModel.IsPaid = false;
                    break;
                case '4':
                    $scope.InvoiceModel.IsPaid = false;
                    break;
                case '5':
                    $scope.InvoiceModel.IsPaid = false;
                    break;
            }
            var jsonData = angular.toJson($scope.InvoiceModel);
            AngularAjaxCall($http, SiteUrl.SaveInvoiceURL, jsonData, "Post", "json", "application/json").
                success(function (response) {
                    if (response.IsSuccess) {
                        $scope.fileUpload(response.Data);
                        //SetMessageForPageLoad(response.Message, "RecordCreatedSuccessfully");
                        //ShowMessages(response, 5000);
                        //window.location.href = "/Invoice/InvoiceList/";
                    } else {
                        ShowMessages(response, 5000);
                    }
                });
        }
    };


    

    $scope.UpdateInvoice = function () {

        debugger
        var isValid = CheckErrors($("#frmSaveInvoice"));
        var isVliadation = $scope.validationAmount();


        //if ($scope.InvoiceModel.FilePath != undefined && $scope.InvoiceModel.FilePath != null && $scope.InvoiceModel.FilePath != "" && $scope.InvoiceModel.OrginalFileName != undefined && $scope.InvoiceModel.OrginalFileName != null && $scope.InvoiceModel.OrginalFileName != "") {

        //}
        if (($("#invoiceFileId")[0].files.length > 0 && $("#invoiceFileId")[0].files[0] != undefined && $("#invoiceFileId")[0].files[0] != null)) {
            $scope.InvoiceModel.FilePath = "";
            $scope.InvoiceModel.OrginalFileName = "";
        }

        if (isValid && isVliadation) {
            switch ($scope.InvoiceModel.InvoiceStatus) {
                case '1':
                    $scope.InvoiceModel.IsPaid = true;
                    break;
                case '2':
                    $scope.InvoiceModel.IsPaid = false;
                    break;
                case '3':
                    $scope.InvoiceModel.IsPaid = false;
                    break;
                case '4':
                    $scope.InvoiceModel.IsPaid = false;
                    break;
                case '5':
                    $scope.InvoiceModel.IsPaid = false;
                    break;
            }
            var jsonData = angular.toJson($scope.InvoiceModel);
            AngularAjaxCall($http, SiteUrl.InvoiceUpdateURL, jsonData, "Post", "json", "application/json").
                success(function (response) {
                    if (response.IsSuccess) {
                        if (($("#invoiceFileId")[0].files.length > 0 && $("#invoiceFileId")[0].files[0] != undefined && $("#invoiceFileId")[0].files[0] != null)) {
                            $scope.fileUpload(response.Data);
                        } else {
                            SetMessageForPageLoad(response.Message, "UpdateSuccessMessage");
                            ShowMessages(response, 9000);
                            setTimeout(function Abc() {
                                window.location.href = SiteUrl.InvoiceListURL;
                            }, 9000);
                            //window.location.href = SiteUrl.InvoiceListURL;
                        }
                    } else {
                        ShowMessages(response, 3000);
                    }
                });
        }
    };

    $scope.validationAmount = function myfunction() {
        if ($scope.InvoiceModel.ActivePatientQuantity != undefined &&
            $scope.InvoiceModel.ActivePatientQuantity != null &&
            $scope.InvoiceModel.ActivePatientQuantity != "" &&
            ($scope.InvoiceModel.ActivePatientUnit == undefined ||
                $scope.InvoiceModel.ActivePatientUnit == null ||
                $scope.InvoiceModel.ActivePatientUnit == "")) {
            //
            ShowMessage("Please add Active Patient Quantity", "warning", 3000, 3000, undefined, false);
            return false;
        }
        if ($scope.InvoiceModel.ActivePatientQuantity == undefined ||
            $scope.InvoiceModel.ActivePatientQuantity == null ||
            $scope.InvoiceModel.ActivePatientQuantity == "" &&
            ($scope.InvoiceModel.ActivePatientUnit != undefined &&
                $scope.InvoiceModel.ActivePatientUnit != null &&
                $scope.InvoiceModel.ActivePatientUnit != "")) {
            //
            //alert("Please add Active Patient Quantity!!!");
            ShowMessage("Active Patient Quantity is required", "error", 5000, 5000, undefined, false);
            return false;
        }

        if ($scope.InvoiceModel.NumberOfTimeSheetQuantity != undefined && $scope.InvoiceModel.NumberOfTimeSheetQuantity != null &&
            $scope.InvoiceModel.NumberOfTimeSheetQuantity != "" &&
            ($scope.InvoiceModel.NumberOfTimeSheetUnit == undefined ||
                $scope.InvoiceModel.NumberOfTimeSheetUnit == null ||
                $scope.InvoiceModel.NumberOfTimeSheetUnit == "")) {
            //
            alert("Please add NumberOfTime Sheet Unit!!!");

            return false;

        }

        if (($scope.InvoiceModel.NumberOfTimeSheetQuantity == undefined || $scope.InvoiceModel.NumberOfTimeSheetQuantity == null ||
            $scope.InvoiceModel.NumberOfTimeSheetQuantity == "") &&
            ($scope.InvoiceModel.NumberOfTimeSheetUnit != undefined &&
                $scope.InvoiceModel.NumberOfTimeSheetUnit != null &&
                $scope.InvoiceModel.NumberOfTimeSheetUnit != "")) {
            //
            alert("Please add NumberOfTime Sheet Quantity!!!");

            return false;

        }


        //IVR

        if (($scope.InvoiceModel.IVRQuantity != undefined &&
            $scope.InvoiceModel.IVRQuantity != null &&
            $scope.InvoiceModel.IVRQuantity != "") &&
            ($scope.InvoiceModel.IVRUnit == undefined ||
                $scope.InvoiceModel.IVRUnit == null ||
                $scope.InvoiceModel.IVRUnit == "")) {
            //
            alert("Please add IVR Unit!!!");

            return false;

        }

        if (($scope.InvoiceModel.IVRQuantity == undefined ||
            $scope.InvoiceModel.IVRQuantity == null ||
            $scope.InvoiceModel.IVRQuantity == "") &&
            ($scope.InvoiceModel.IVRUnit != undefined &&
                $scope.InvoiceModel.IVRUnit != null &&
                $scope.InvoiceModel.IVRUnit != "")) {
            //
            alert("Please add IVR Quantity !!!");

            return false;

        }


        //MessageQuantity

        if (($scope.InvoiceModel.MessageQuantity != undefined &&
            $scope.InvoiceModel.MessageQuantity != null &&
            $scope.InvoiceModel.MessageQuantity != "") &&
            ($scope.InvoiceModel.MessageUnit == undefined ||
                $scope.InvoiceModel.MessageUnit == null ||
                $scope.InvoiceModel.MessageUnit == "")) {
            //
            alert("Please add Message Unit !!!");

            return false;

        }

        if (($scope.InvoiceModel.MessageQuantity == undefined ||
            $scope.InvoiceModel.MessageQuantity == null ||
            $scope.InvoiceModel.MessageQuantity == "") &&
            ($scope.InvoiceModel.MessageUnit != undefined &&
                $scope.InvoiceModel.MessageUnit != null &&
                $scope.InvoiceModel.MessageUnit != "")) {
            //
            alert("Please add Message Quantity !!!");

            return false;

        }


        //ClaimsQuantity
        if (($scope.InvoiceModel.ClaimsQuantity != undefined &&
            $scope.InvoiceModel.ClaimsQuantity != null &&
            $scope.InvoiceModel.ClaimsQuantity != "") &&
            ($scope.InvoiceModel.ClaimsUnit == undefined ||
                $scope.InvoiceModel.ClaimsUnit == null ||
                $scope.InvoiceModel.ClaimsUnit == "")) {
            //
            alert("Please add Claims Unit !!!");

            return false;

        }

        if (($scope.InvoiceModel.ClaimsQuantity == undefined ||
            $scope.InvoiceModel.ClaimsQuantity == null ||
            $scope.InvoiceModel.ClaimsQuantity == "") &&
            ($scope.InvoiceModel.ClaimsUnit != undefined &&
                $scope.InvoiceModel.ClaimsUnit != null &&
                $scope.InvoiceModel.ClaimsUnit != "")) {
            //

            alert("Please add Claims Quantity !!!");

            return false;

        }

        //FormsQuantity

        if (($scope.InvoiceModel.FormsQuantity != undefined &&
            $scope.InvoiceModel.FormsQuantity != null &&
            $scope.InvoiceModel.FormsQuantity != "") &&
            ($scope.InvoiceModel.FormsUnit == undefined ||
                $scope.InvoiceModel.FormsUnit == null ||
                $scope.InvoiceModel.FormsUnit == "")) {
            //
            alert("Please add Forms Quantity !!!");

            return false;

        }

        if (($scope.InvoiceModel.FormsQuantity == undefined ||
            $scope.InvoiceModel.FormsQuantity == null ||
            $scope.InvoiceModel.FormsQuantity == "") &&
            ($scope.InvoiceModel.FormsUnit != undefined &&
                $scope.InvoiceModel.FormsUnit != null &&
                $scope.InvoiceModel.FormsUnit != "")) {
            //
            alert("Please add Forms Quantity !!!");

            return false;

        }
        return true;
    }

    $scope.checkDecmalNumber = function (evt) {
        var self = $(this);
        evt.key.replace(/[^0-9\.]/g, '');
        if ((evt.which != 46 || self.val().indexOf('.') != -1) && (evt.which < 48 || evt.which > 57)) {
            evt.preventDefault();
        }
    };

    $scope.getkeys = function (val, evt) {
        var rgx = /^[+-]?(?:\d+\.?\d*|\d*\.?\d+)[\r\n]*$/.test(evt.target.value);
        if (!rgx) {
            //evt.preventDefault();
            //if (evt.keyCode == 110 &&
            //    !evt.target.value.indexOf('.') !== -1) {
            evt.target.value = evt.target.value.substring(0, evt.target.value.length - 1);
            //}
            return;
        }

        //$scope.checkDecmalNumber();

        if ($scope.InvoiceModel.ActivePatientQuantity != undefined && $scope.InvoiceModel.ActivePatientQuantity != null && $scope.InvoiceModel.ActivePatientQuantity != "" && $scope.InvoiceModel.ActivePatientUnit != undefined && $scope.InvoiceModel.ActivePatientUnit != null && $scope.InvoiceModel.ActivePatientUnit != "") {
            $scope.InvoiceModel.ActivePatientAmount = $scope.InvoiceModel.ActivePatientQuantity * $scope.InvoiceModel.ActivePatientUnit;
        } else {
            $scope.InvoiceModel.ActivePatientAmount = 0;
        }

        if ($scope.InvoiceModel.NumberOfTimeSheetQuantity != undefined && $scope.InvoiceModel.NumberOfTimeSheetQuantity != null && $scope.InvoiceModel.NumberOfTimeSheetQuantity != "" && $scope.InvoiceModel.NumberOfTimeSheetUnit != undefined && $scope.InvoiceModel.NumberOfTimeSheetUnit != null && $scope.InvoiceModel.NumberOfTimeSheetUnit != "") {
            $scope.InvoiceModel.NumberOfTimeSheetAmount = $scope.InvoiceModel.NumberOfTimeSheetQuantity * $scope.InvoiceModel.NumberOfTimeSheetUnit;
        }
        else {
            $scope.InvoiceModel.NumberOfTimeSheetAmount = 0;
        }

        if ($scope.InvoiceModel.IVRQuantity != undefined && $scope.InvoiceModel.IVRQuantity != null
            && $scope.InvoiceModel.IVRQuantity != "" && $scope.InvoiceModel.IVRUnit != undefined && $scope.InvoiceModel.IVRUnit != null && $scope.InvoiceModel.IVRUnit != "") {
            $scope.InvoiceModel.IVRAmount = $scope.InvoiceModel.IVRQuantity * $scope.InvoiceModel.IVRUnit;
        }
        else {
            $scope.InvoiceModel.IVRAmount = 0;
        }

        if ($scope.InvoiceModel.MessageQuantity != undefined && $scope.InvoiceModel.MessageQuantity != null && $scope.InvoiceModel.MessageQuantity != "" && $scope.InvoiceModel.MessageUnit != undefined && $scope.InvoiceModel.MessageUnit != null && $scope.InvoiceModel.MessageUnit != "") {
            $scope.InvoiceModel.MessageAmount = $scope.InvoiceModel.MessageQuantity * $scope.InvoiceModel.MessageUnit;
        }
        else {
            $scope.InvoiceModel.MessageAmount = 0;
        }

        if ($scope.InvoiceModel.ClaimsQuantity != undefined && $scope.InvoiceModel.ClaimsQuantity != null && $scope.InvoiceModel.ClaimsQuantity != "" && $scope.InvoiceModel.ClaimsUnit != undefined && $scope.InvoiceModel.ClaimsUnit != null && $scope.InvoiceModel.ClaimsUnit != "") {
            $scope.InvoiceModel.ClaimsAmount = $scope.InvoiceModel.ClaimsQuantity * $scope.InvoiceModel.ClaimsUnit;
        }
        else {
            $scope.InvoiceModel.ClaimsAmount = 0;
        }

        if ($scope.InvoiceModel.FormsQuantity != undefined && $scope.InvoiceModel.FormsQuantity != null && $scope.InvoiceModel.FormsQuantity != "" && $scope.InvoiceModel.FormsUnit != undefined && $scope.InvoiceModel.FormsUnit != null && $scope.InvoiceModel.FormsUnit != "") {
            $scope.InvoiceModel.FormsAmount = $scope.InvoiceModel.FormsQuantity * $scope.InvoiceModel.FormsUnit;
        }
        else {
            $scope.InvoiceModel.FormsAmount = 0;
        }

        $scope.totalAmount();
    };

    $scope.totalAmount = function myfunction() {
        $scope.InvoiceModel.InvoiceAmount = 0;
        $scope.InvoiceModel.InvoiceAmount += $scope.InvoiceModel.ActivePatientAmount != undefined && $scope.InvoiceModel.ActivePatientAmount != null && $scope.InvoiceModel.ActivePatientAmount != "" ? $scope.InvoiceModel.ActivePatientAmount : 0;
        $scope.InvoiceModel.InvoiceAmount += $scope.InvoiceModel.NumberOfTimeSheetAmount != undefined && $scope.InvoiceModel.NumberOfTimeSheetAmount != "" && $scope.InvoiceModel.NumberOfTimeSheetAmount != null ? $scope.InvoiceModel.NumberOfTimeSheetAmount : 0;
        $scope.InvoiceModel.InvoiceAmount += $scope.InvoiceModel.IVRAmount != undefined && $scope.InvoiceModel.IVRAmount != null && $scope.InvoiceModel.IVRAmount != "" ? $scope.InvoiceModel.IVRAmount : 0;
        $scope.InvoiceModel.InvoiceAmount += $scope.InvoiceModel.MessageAmount != undefined && $scope.InvoiceModel.MessageAmount != null && $scope.InvoiceModel.MessageAmount != "" ? $scope.InvoiceModel.MessageAmount : 0;
        $scope.InvoiceModel.InvoiceAmount += $scope.InvoiceModel.ClaimsAmount != undefined && $scope.InvoiceModel.ClaimsAmount != null && $scope.InvoiceModel.ClaimsAmount != "" ? $scope.InvoiceModel.ClaimsAmount : 0;
        $scope.InvoiceModel.InvoiceAmount += $scope.InvoiceModel.FormsAmount != undefined && $scope.InvoiceModel.FormsAmount != null && $scope.InvoiceModel.FormsAmount != "" ? $scope.InvoiceModel.FormsAmount : 0;
        //$scope.InvoiceModel.InvoiceAmount.toFixed(2);
    };

    $scope.getOrganizationList = function () {
        debugger
        var jsonData = {};
        AngularAjaxCall($http, SiteUrl.GetAllOrganizationListURL, jsonData, "GET", "json", "application/json").success(function (response) {
            if (response != null && response.length > 0) {

                $scope.SelectedOrganizationIds = [];
                $scope.SelectAllCheckbox = false;
                $scope.OrganizationList = response;
            }
            ShowMessages(response);
        });
    };

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            //SearchOrganizationModel: $scope.SearchOrganizationModel,
            //pageSize: $scope.OrganizationListPager.pageSize,
            //pageIndex: fromIndex,
            //sortIndex: $scope.OrganizationListPager.sortIndex,
            //sortDirection: $scope.OrganizationListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.getOrganizationList();


    $scope.fileUpload = function (val) {

        var fd = new FormData();
        var f = $("#invoiceFileId")[0].files[0];
        fd.append('file', f);

        $http.post(SiteUrl.InvoiceFileUploadURL, fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        }).success(function (response) {
            if (response.IsSuccess) {
                SetMessageForPageLoad(response.Message, "RecordCreatedSuccessfully");
                ShowMessages(response, 9000);
                setTimeout(function Abc() {
                    window.location.href = SiteUrl.InvoiceListURL;
                }, 9000);

            } else {
                ShowMessages(response, 5000);
            }
        });
    };

    //region File Upload--------------------------------
    $scope.UploadingFileList = [];

    $scope.BeforeSend = function (e, data) {
        var isValidImage = true;
        var fileName;
        var errorMsg;
        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            //if (extension == "exe") {
            if (extension != "pdf") {
                //file.FileProgress = 100;
                $scope.UploadingFileList.remove(file);
                //errorMsg = window.InvalidImageUploadMessage;
                //isValidImage = false;
                ShowMessage(window.InvalidDocumentUploadMessage, "error");
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
        console.log(data.files[0].name + ":-" + Math.round((data.loaded / data.total) * 100));
    };

    $scope.AfterSend = function (data) {
        $scope.IsFileUploading = false;
        ShowMessage("File uploaded successfully");

        //$scope.GetFormList(sendData, id);
        $scope.ShowUpload = false;
        //$('#addFormModal').modal('hide');
    };

    function InvoiceStatusFX (val) {
        
    }

    //endregion File Upload------------------------------

    $scope.onload = function () {
        var modelJson = $.parseJSON($("#hdnInvoiceModel").val());
        if (modelJson.InvoiceNumber !== "") {
            $scope.InvoiceModel = modelJson;
            $scope.InvoiceModel.IsUpdate = false;

        }

        if (modelJson.InvoiceAmount != undefined && modelJson.InvoiceAmount !== "" && modelJson.InvoiceAmount != null) {
            $scope.InvoiceModel = modelJson;
            $scope.InvoiceModel.IsUpdate = true;
        }

    };
    $scope.onload();

};
controllers.AddInvoiceController.$inject = ['$scope', '$http', '$window', '$timeout'];