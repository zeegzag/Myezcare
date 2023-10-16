var ReferralDocumentVM;

controllers.ReferralDocumentController = function ($scope, $http, $window, $timeout) {
    ReferralDocumentVM = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnAddReferralModel").val());
    };

    var modalJson = $.parseJSON($("#hdnAddReferralModel").val());
    $scope.ReferralModel = modalJson;

    $scope.EncryptedReferralID = $scope.ReferralModel.Referral.EncryptedReferralID; //"iSNqtcWbe3gZEhtctmlPcA2";
    $scope.ReferralID = $scope.ReferralModel.Referral.ReferralID; //"iSNqtcWbe3gZEhtctmlPcA2";


    $scope.ListofFields = "";

    //Start: Referral Missing Document

    $scope.SetReferralMissingDocument = function () {
        HideErrors("#frmMissingDocument");
        var jsonData = angular.toJson({ EncryptedReferralID: $scope.EncryptedReferralID });
        AngularAjaxCall($http, HomeCareSiteUrl.SetReferralMissingDocumentURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.MissingDocumentModel = response.Data;
            }
            ShowMessages(response);
        });
    };

    $scope.SendEmail = function () {
        if (CheckErrors("#frmMissingDocument")) {
            var postData = {
                MissingDocumentModel: $scope.MissingDocumentModel,
                EncryptedReferralID: $scope.EncryptedReferralID
            };
            var jsonData = angular.toJson(postData);
            AngularAjaxCall($http, HomeCareSiteUrl.SendEmailForReferralMissingDocumentURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
            });
        }
    };

    //End: Referral Missing Document

    //#region Nisarg

    $scope.UploadFile = HomeCareSiteUrl.UploadFile;

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
            if (extension == "exe") {
                file.FileProgress = 100;
                //$scope.UploadingFileList.remove(file);
                errorMsg = window.InvalidImageUploadMessage;
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
        var jsonData = angular.toJson({ id: $scope.EncryptedReferralID});
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralDocumentList, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralDocumentList = response.Data.Items;
                $scope.ReferralDocumentPager.currentPageSize = response.Data.Items.length;
                $scope.ReferralDocumentPager.totalRecords = response.Data.TotalItems;
                $scope.UploadingFileList.remove(data.File);
            }
            ShowMessages(response);
        });
    };

    $scope.DeleteUploadedFile = function (file) {
        $scope.UploadingFileList.remove(file);
    };

    $scope.RemoveFilesToUploadArray = function (filename) {
        var fileToRemove;

        fileToRemove = $scope.SearchArrayByPropertyValue($scope.ReferralDocumentList, filename); // Using given FILENAME search in array for  delete key find.

        if (fileToRemove != -1)
            $scope.ReferralDocumentList.splice(fileToRemove, 1);
    };

    $scope.SearchArrayByPropertyValue = function (obj, value) {
        var returnKey = -1;
        $.each(obj, function (key, info) {
            if (info.FileName == value) {
                returnKey = key;
                return false;
            }
        });

        return returnKey;
    };

    $scope.ReferralDocumentPager = new PagerModule("ReferralDocumentID");
    
    $scope.SetPostData = function (fromIndex, value) {

        var pagermodel = {
            id: value,
            pageSize: $scope.ReferralDocumentPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ReferralDocumentPager.sortIndex,
            sortDirection: $scope.ReferralDocumentPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            $scope.ReferralDocumentPager.currentPage = 1;
            $scope.ReferralDocumentPager.getDataCallback();
        });
    };

    $scope.GetReferralDocumentList = function () {
        //$scope.ReferralDocumentPager.pageSize = 50;
        
        var jsonData = $scope.SetPostData($scope.ReferralDocumentPager.currentPage, $scope.EncryptedReferralID);
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralDocumentList, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralDocumentList = response.Data.Items;
                $scope.ReferralDocumentPager.currentPageSize = response.Data.Items.length;
                $scope.ReferralDocumentPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.DeleteDocument = function (data) {
        bootboxDialog(function (result) {
            if (result) {

                if ($scope.ReferralDocumentPager.currentPage != 1)
                    $scope.ReferralDocumentPager.currentPage = $scope.ReferralDocumentList.length === 1 ? $scope.ReferralDocumentPager.currentPage - 1 : $scope.ReferralDocumentPager.currentPage;
                var jsonData = $scope.SetPostData($scope.ReferralDocumentPager.currentPage, data.ReferralDocumentID);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteDocumentURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.ReferralDocumentList = response.Data.Items;
                        $scope.ReferralDocumentPager.currentPageSize = response.Data.Items.length;
                        $scope.ReferralDocumentPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteDocumentMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.IsModalShowFirstTime = false;
    $scope.DocumentDetail = $scope.ReferralModel.ReferralDocument;

    $scope.EditDocument = function (data) {
        //$timeout(function () {
        $scope.SelectedDocType = (data.KindOfDocument == "Internal") ? 1 : 2;
            //$scope.DocumentDetail.DocumentTypeID = data.DocumentTypeID;
            //$scope.DocumentDetail.FileName = data.FileName;
            //$scope.DocumentDetail.KindOfDocument = data.KindOfDocument;
            //$scope.DocumentDetail.DocumentTypeName = data.DocumentTypeName;
            //$scope.DocumentDetail.ReferralDocumentID = data.ReferralDocumentID;
        //},200);


        ////$timeout(function () {
        $scope.DocumentDetail = $.parseJSON(JSON.stringify(data));
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
        ////});
    };

    $('#EditDocument').on('hidden.bs.modal', function () {
        //$scope.DocumentDetail = {};
        HideErrors($("#frmDocumentEdit"));
    });

    $scope.CloseEditDocument = function () {
        $scope.DocumentDetail = {};
        HideErrors($("#frmDocumentEdit"));
    };

    //$scope.$watch('DocumentDetail.KindOfDocument', function (newValue, oldValue) {
    //    if ((newValue != "" && oldValue != "" && (newValue != oldValue))) {
    //        $scope.IsModalShowFirstTime = false;
    //    } else {
    //        $scope.IsModalShowFirstTime = true;
    //    }
    //    if ((!$scope.IsModalShowFirstTime)) {
    //        $scope.DocumentDetail.DocumentTypeID = '';
    //    }
    //    $scope.IsModalShowFirstTime = false;
    //});

    //$('#EditDocument').on('show.bs.modal', function (data) {
    //    $scope.IsModalShowFirstTime = true;
    //});

    $scope.SaveDocument = function (data) {
        if ($scope.ReferralDocumentPager.currentPage != 1)
            $scope.ReferralDocumentPager.currentPage = $scope.ReferralDocumentList.length === 1 ? $scope.ReferralDocumentPager.currentPage - 1 : $scope.ReferralDocumentPager.currentPage;
        var jsonData = $scope.SetPostData($scope.ReferralDocumentPager.currentPage, data);
        if (CheckErrors("#frmDocumentEdit")) {
            AngularAjaxCall($http, HomeCareSiteUrl.EditDocumentURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.ReferralDocumentList = response.Data.Items;
                    $scope.ReferralDocumentPager.currentPageSize = response.Data.Items.length;
                    $scope.ReferralDocumentPager.totalRecords = response.Data.TotalItems;
                    $('#EditDocument').modal('hide');
                }
                ShowMessages(response);
            });
        }
    };


    $scope.ReferralDocumentPager.getDataCallback = $scope.GetReferralDocumentList;
    
    $scope.SetFilter = function () {
        
        $scope.SelectedDocType = ($scope.DocumentDetail.KindOfDocument == "Internal") ? 1 : 2;
    };

    //#endregion Nisarg

    $("a#referralDocument, a#referralDocument_clientdocuments").on('shown.bs.tab', function (e) {
        $(".tab-pane a[href='#tab_ClientDocuments']").tab('show');
        $scope.GetReferralDocumentList();
    });

    $("a#referralDocument_missingdocument").on('shown.bs.tab', function (e) {
        $scope.SetReferralMissingDocument();
    });


    $scope.init = function () { console.log('Summernote is launched'); };
    $scope.enter = function () { console.log('Enter/Return key pressed'); };
    $scope.focus = function (e) { console.log('Editable area is focused'); };
    $scope.blur = function (e) { console.log('Editable area loses focus'); };
    $scope.paste = function (e) { console.log('Called event paste'); };
    $scope.change = function (contents) {
        console.log('contents are changed:', contents, $scope.editable);
    };
    $scope.keyup = function (e) { console.log('Key is released:', e.keyCode); };
    $scope.keydown = function (e) { console.log('Key is pressed:', e.keyCode); };
    $scope.imageUpload = function (files) {
        console.log('image upload:', files);
        console.log('image upload\'s editable:', $scope.editable);
    };
    $scope.SliderLeft = function () {
        angular.element('#SliderLeft').addClass("LeftCallShow");
        angular.element('#slider').addClass("LeftCallShow");
        angular.element('#slider').removeClass("RightCallShow");
        angular.element('#rightShow').removeClass("RightCallShow");
    }
    $scope.SliderRight = function () {
        angular.element('#rightShow').addClass("RightCallShow");
        angular.element('#slider').addClass("RightCallShow");
        angular.element('#slider').removeClass("LeftCallShow");
        angular.element('#SliderLeft').removeClass("LeftCallShow");
    }

};

controllers.ReferralDocumentController.$inject = ['$scope', '$http', '$window', '$timeout'];


$(document).ready(function () {
    ShowPageLoadMessage("ShowAddReferralMessage");
});