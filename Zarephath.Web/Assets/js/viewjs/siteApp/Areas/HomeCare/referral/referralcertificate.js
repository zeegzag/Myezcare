    
controllers.ReferralCertificateController = function ($scope, $http, $window, $timeout, ) {
    $scope.EncryptedReferralID = window.EncryptedReferralID;
    $scope.EncryptedEmployeeID = ($("#hdnEmployeeModel").length > 0) ? ($.parseJSON($("#hdnEmployeeModel").val())).Employee.EncryptedEmployeeID : null;
    $scope.EmployeeID = ($("#hdnEmployeeModel").length > 0) ? ($.parseJSON($("#hdnEmployeeModel").val())).Employee.EmployeeID : null;

    $scope.EmployeeModel = $.parseJSON($("#hdnEmployeeModel").val());
    $scope.EmployeeCertificateList = [];

    $scope.IsShow = false;
    $scope.GetUserCertificates = function () {
        var jsonData = angular.toJson({ email: $scope.EmployeeModel.Employee.Email });
        AngularAjaxCall($http, HomeCareSiteUrl.GetUserCertificateURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.EmployeeCertificateList = response.Data;
               // $scope.EmployeeCertificateList.length = response.Data.length;
                
                $scope.IsShow = true;
            }
            ShowMessages(response);
        });
    }
    $scope.UploadStatus = false;
    $scope.InsertData = function () {
        debugger
        var isValid = CheckErrors($("#frmCertificate"));
        var UploadStatus = $scope.UploadStatus;
        //if (!UploadStatus) {
        //    ShowMessage("Please select upload file", "error");
        //}
        if (isValid && UploadStatus) {
            // $scope.Certificate = {};
           // $scope.ReferralCertificate.CFile = $scope.files;
            //$scope.ReferralCertificate.Name = $scope.Certificate.cname;
            //$scope.ReferralCertificate.StartDate = $scope.Certificate.selectStartDate;
            //$scope.ReferralCertificate.EndDate = $scope.Certificate.selectEndDate;
            //$scope.ReferralCertificate.CertificateAuthority = $scope.Certificate.CertificateAuthority;
            $scope.Certificate.EmployeeID = $scope.EmployeeID;
            var jsonData = angular.toJson($scope.Certificate);
            AngularAjaxCall($http, HomeCareSiteUrl.SaveRefrallCertificationURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $("#AddCertificateModel").modal('hide');
                    $scope.GetReferralCertificateList();
                    ShowMessage(response.Data, "Success");
                    $scope.Certificate = {};
                    $scope.UploadStatus = false;
                }
                else {

                    ShowMessages(response);
                }

            });
        }
        else {
            if (!isValid) {
                ShowMessage("Please fill all fields", "error");
            }
            else
            {
                ShowMessage("Please select upload file", "error");
            }
        }
    };
    $scope.GetReferralCertificateList = function () {
        
        $scope.ReferralCertificate = {};
        $scope.EncryptedEmployeeID = ($("#hdnEmployeeModel").length > 0) ? ($.parseJSON($("#hdnEmployeeModel").val())).Employee.EncryptedEmployeeID : null;
        $scope.ReferralCertificate.EmployeeID = ($("#hdnEmployeeModel").length > 0) ? ($.parseJSON($("#hdnEmployeeModel").val())).Employee.EmployeeID : null;
        var jsonData = angular.toJson($scope.ReferralCertificate);
        AngularAjaxCall($http, HomeCareSiteUrl.ReferralCertificateListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                
               //$scope.IsShow = true;
                $scope.CList = response.Data;
            }
           
        });
    }
    $scope.DeleteCertificate = function (certificate) {
        bootboxDialog(function (result) {
            if (result) {
                $scope.CertificateID = certificate.CertificateID;
                var jsonData = angular.toJson({ CertificateID: $scope.CertificateID });

                AngularAjaxCall($http, HomeCareSiteUrl.DeleteEmployeeCertificationURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.GetReferralCertificateList();
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteNoteMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    }
    $scope.OpenModelCaregiver = function (CertificateUrl) {
        $scope.img = CertificateUrl;
        $('#AddreferralCertificateModal').modal({
            backdrop: 'static',
            keyboard: false
        });
    }
    $scope.CertificateAuthorityList = [];
   
    $scope.OpenModel = function () {
        var jsonData = angular.toJson();
        AngularAjaxCall($http, HomeCareSiteUrl.CertificateAuthorityURL, jsonData, "Get", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.CertificateAuthorityList = response.Data;
            }
        });
        $('#AddCertificateModel').modal({
            backdrop: 'static',
            keyboard: false
        });
    }
    $scope.EditCertificate = function (item) {
        $scope.cname = item.Name;
        $scope.CertificateAuthority = item.CertificateAuthority;
        $scope.selectStartDate = item.StartDate;
        $scope.selectEndDate = item.EndDate;
       // $scope.CertificatePath = item.CertificatePath;

        $('#AddCertificateModel').modal({
            backdrop: 'static',
            keyboard: false
        });
    }
    $scope.View = function (item) {
        window.open(item.CertificatePath,'_blank');
    }
    $scope.Reset= function () {
        $scope.Certificate = {};
    }


    $scope.UploadFile = "/hc/referral/UploadCertificate";

    $scope.UploadingFileList = [];
   
    $scope.UploadCertificate = HomeCareSiteUrl.UploadCertificateURL;
    $scope.UploadingFileList = [];
    $scope.ProfileImageBeforeSend = function (e, data) {
        var isValidImage = true;
        var fileName;
        var errorMsg;

        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            if (extension !== "jpg" && extension !== "jpeg" && extension !== "png" && extension !== "bmp" && extension !== "pdf") {
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
            //$scope.EditProfileModel.Employee.ProfileImagePath = model.Data.TempFilePath;
            $scope.UploadingFileList = [];
        } else {
            ShowMessage(model);
        }
        $scope.$apply();
        //window.location.reload();
        $scope.UploadStatus = true;
    };






    $("a#referralCertificate").on('shown.bs.tab', function (e) {
        $scope.GetUserCertificates();
        $scope.GetReferralCertificateList();
    });

   

};