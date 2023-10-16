var EmployeeDocumentVM;
controllers.EmployeeDocumentController = function ($scope, $http, $window, $timeout) {
    EmployeeDocumentVM = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnEmployeeModel").val());
    };

    var modalJson = $.parseJSON($("#hdnEmployeeModel").val());
    $scope.EmployeeModel = modalJson;

    $scope.EncryptedEmployeeID = $scope.EmployeeModel.Employee.EncryptedEmployeeID;
    $scope.EmployeeID = $scope.EmployeeModel.Employee.EmployeeID; 

    //File Upload
    //#region
    $scope.UploadFile = HomeCareSiteUrl.UploadEmployeeDocumentURL;
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
        $scope.GetEmployeeDocumentList();
        //$scope.UploadingFileList = [];
        $scope.UploadingFileList.remove(data.File);
    };

    $scope.DeleteUploadedFile = function (file) {
        $scope.UploadingFileList.remove(file);
    };
    //#endregion

    //Get Employee Document List
    //#region
    $scope.EmployeeDocumentPager = new PagerModule("ReferralDocumentID");

    $scope.SetPostData = function (fromIndex, value) {
        var pagermodel = {
            id: value,
            pageSize: $scope.EmployeeDocumentPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.EmployeeDocumentPager.sortIndex,
            sortDirection: $scope.EmployeeDocumentPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.GetEmployeeDocumentList = function () {
        //$scope.EmployeeDocumentPager.pageSize = 50;
        var jsonData = $scope.SetPostData($scope.EmployeeDocumentPager.currentPage, $scope.EncryptedEmployeeID);
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeDocumentList, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.EmployeeDocumentList = response.Data.Items;
                $scope.EmployeeDocumentPager.currentPageSize = response.Data.Items.length;
                $scope.EmployeeDocumentPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.EmployeeDocumentPager.getDataCallback = $scope.GetEmployeeDocumentList;
    $scope.EmployeeDocumentPager.getDataCallback();
    //#endregion

    //Employee Tab Show Tab Show
    //#region
    $("a#employeeDocuments").on('shown.bs.tab', function (e) {
        $(".tab-pane a[href='#tab_Documents']").tab('show');
        $scope.EmployeeDocumentPager.getDataCallback();
    });
    //#endregion

    //Delete Employee Document
    //#region
    $scope.DeleteDocument = function (data) {
        bootboxDialog(function (result) {
            if (result) {
                if ($scope.EmployeeDocumentPager.currentPage != 1)
                    $scope.EmployeeDocumentPager.currentPage = $scope.EmployeeDocumentList.length === 1 ? $scope.EmployeeDocumentPager.currentPage - 1 : $scope.EmployeeDocumentPager.currentPage;

                var jsonData = $scope.SetPostData($scope.EmployeeDocumentPager.currentPage, data.ReferralDocumentID);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteEmployeeDocumentURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.EmployeeDocumentList = response.Data.Items;
                        $scope.EmployeeDocumentPager.currentPageSize = response.Data.Items.length;
                        $scope.EmployeeDocumentPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteDocumentMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };
    //#endregion

    $scope.DocumentDetail = $scope.EmployeeModel.EmployeeDocument;
    $scope.EditDocument = function (data) {
        $scope.DocumentDetail = $.parseJSON(JSON.stringify(data));
        $('#EditEmployeeDocument').modal({
            backdrop: 'static',
            keyboard: false
        });
    };

    $scope.CloseEditDocument = function () {
        HideErrors($("#frmDocumentEdit"));
    };

    $scope.SaveDocument = function (data) {
        if ($scope.EmployeeDocumentPager.currentPage != 1)
            $scope.EmployeeDocumentPager.currentPage = $scope.EmployeeDocumentList.length === 1 ? $scope.EmployeeDocumentPager.currentPage - 1 : $scope.EmployeeDocumentPager.currentPage;
        var jsonData = $scope.SetPostData($scope.EmployeeDocumentPager.currentPage, data);
        if (CheckErrors("#frmDocumentEdit")) {
            AngularAjaxCall($http, HomeCareSiteUrl.SaveEmployeeDocumentURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.EmployeeDocumentList = response.Data.Items;
                    $scope.EmployeeDocumentPager.currentPageSize = response.Data.Items.length;
                    $scope.EmployeeDocumentPager.totalRecords = response.Data.TotalItems;
                    $('#EditEmployeeDocument').modal('hide');
                }
                ShowMessages(response);
            });
        }
    };


    //$scope.Internal = window.Internal;
    //$scope.External = window.External;
    //$scope.InternalShort = window.InternalShort;
    //$scope.ExternalShort = window.ExternalShort;

    
    //$scope.RemoveFilesToUploadArray = function (filename) {
    //    var fileToRemove;
    //    fileToRemove = $scope.SearchArrayByPropertyValue($scope.EmployeeDocumentList, filename); // Using given FILENAME search in array for  delete key find.
    //    if (fileToRemove != -1)
    //        $scope.EmployeeDocumentList.splice(fileToRemove, 1);
    //};

    //$scope.SearchArrayByPropertyValue = function (obj, value) {
    //    var returnKey = -1;
    //    $.each(obj, function (key, info) {
    //        if (info.FileName == value) {
    //            returnKey = key;
    //            return false;
    //        }
    //    });

    //    return returnKey;
    //};

    
    //$scope.IsModalShowFirstTime = false;

    //$('#EditEmployeeDocument').on('show.bs.modal', function (data) {
    //    $scope.IsModalShowFirstTime = true;
    //});

    //$scope.init = function () { console.log('Summernote is launched'); };
    //$scope.enter = function () { console.log('Enter/Return key pressed'); };
    //$scope.focus = function (e) { console.log('Editable area is focused'); };
    //$scope.blur = function (e) { console.log('Editable area loses focus'); };
    //$scope.paste = function (e) { console.log('Called event paste'); };
    //$scope.change = function (contents) {
    //    console.log('contents are changed:', contents, $scope.editable);
    //};
    //$scope.keyup = function (e) { console.log('Key is released:', e.keyCode); };
    //$scope.keydown = function (e) { console.log('Key is pressed:', e.keyCode); };
    //$scope.imageUpload = function (files) {
    //    console.log('image upload:', files);
    //    console.log('image upload\'s editable:', $scope.editable);
    //};
  
};
controllers.EmployeeDocumentController.$inject = ['$scope', '$http', '$window', '$timeout'];
$(document).ready(function () {
    ShowPageLoadMessage("ShowAddEmployeeMessage");
});