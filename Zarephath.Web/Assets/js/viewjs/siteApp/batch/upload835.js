var vm;
controllers.Upload835Controller = function ($scope, $http) {
    vm = $scope;
    $scope.Upload835FileListURL = SiteUrl.Upload835FileListURL;
    $scope.Upload835List = [];
    $scope.SelectedUpload835Ids = [];
    $scope.SelectAllCheckbox = false;
    $scope.FormID = "#frmUpload835";
    $scope.FileData = null;

    $scope.CheckUnProcessedFiles = function () {
        $scope.UnProcessedCount = 0;
        $scope.Upload835List.filter(function (data) {
            if (data.Upload835FileProcessStatus == window.UnProcessedStatus)
                $scope.UnProcessedCount = $scope.UnProcessedCount + 1;
        });
    };

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnAddUpload835Model").val());
    };
    $scope.Upload835Model = $.parseJSON($("#hdnAddUpload835Model").val());
    $scope.Upload835Model.SelectFileLabel = window.Select835File;

    $scope.SearchUpload835ListPage = $scope.Upload835Model.SearchUpload835ListPage;
    $scope.TempUpload835ListPage = $scope.Upload835Model.SearchUpload835ListPage;
    $scope.Upload835ListPager = new PagerModule("Upload835FileID", undefined, 'DESC');

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchUpload835Model: $scope.SearchUpload835ListPage,
            pageSize: $scope.Upload835ListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.Upload835ListPager.sortIndex,
            sortDirection: $scope.Upload835ListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchUpload835ListPage = $.parseJSON(angular.toJson($scope.TempUpload835ListPage));
    };

    $scope.GetUpload835List = function (isSearchDataMappingRequire) {

        //Reset Selcted Checkbox items and Control
        $scope.SelectedUpload835Ids = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchUpload835ListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        $scope.AjaxStart = true;
        var jsonData = $scope.SetPostData($scope.Upload835ListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.Upload835FileListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.Upload835List = response.Data.Items;
                $scope.Upload835ListPager.currentPageSize = response.Data.Items.length;
                $scope.Upload835ListPager.totalRecords = response.Data.TotalItems;
                $scope.CheckUnProcessedFiles();
            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.SearchUpload835ListPage = $scope.newInstance().SearchUpload835ListPage;
        $scope.TempUpload835ListPage = $scope.newInstance().SearchUpload835ListPage;
        $scope.Upload835ListPager.currentPage = 1;
        $scope.Upload835ListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        //Reset 
        $scope.SearchUpload835ListPage = $scope.newInstance().SearchUpload835ListPage;
        $scope.TempUpload835ListPage = $scope.newInstance().SearchUpload835ListPage;
        $scope.Upload835ListPager.currentPage = 1;
        $scope.Upload835ListPager.getDataCallback();
    };

    $scope.SearchUpload835Files = function () {
        $scope.Upload835ListPager.currentPage = 1;
        $scope.Upload835ListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectUpload835File = function (upload835) {
        if (upload835.IsChecked)
            $scope.SelectedUpload835Ids.push(upload835.Upload835FileID);
        else
            $scope.SelectedUpload835Ids.remove(upload835.Upload835FileID);

        if ($scope.SelectedUpload835Ids.length == $scope.Upload835ListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedUpload835Ids = [];
        angular.forEach($scope.Upload835List, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            
            if (item.Upload835FileProcessStatus !== parseInt(window.UnProcessedStatus))
                item.IsChecked = false;

            if (item.IsChecked)
                $scope.SelectedUpload835Ids.push(item.Upload835FileID);
        });
        return true;
    };

    $scope.DeleteUpload835File = function (upload835FileId, title) {

        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            //
            if (result) {
                $scope.SearchUpload835ListPage.ListOfIdsInCSV = upload835FileId > 0 ? upload835FileId.toString() : $scope.SelectedUpload835Ids.toString();
                if (upload835FileId > 0) {
                    if ($scope.Upload835ListPager.currentPage != 1)
                        $scope.Upload835ListPager.currentPage = $scope.Upload835List.length === 1 ? $scope.Upload835ListPager.currentPage - 1 : $scope.Upload835ListPager.currentPage;
                } else {
                    if ($scope.Upload835ListPager.currentPage != 1 && $scope.SelectedUpload835Ids.length == $scope.Upload835ListPager.currentPageSize)
                        $scope.Upload835ListPager.currentPage = $scope.Upload835ListPager.currentPage - 1;
                }
                //Reset Selcted Checkbox items and Control
                $scope.SelectedUpload835Ids = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.Upload835ListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteUpload835FileUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.Upload835List = response.Data.Items;
                        $scope.Upload835ListPager.currentPageSize = response.Data.Items.length;
                        $scope.Upload835ListPager.totalRecords = response.Data.TotalItems;
                        $scope.CheckUnProcessedFiles();
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.ProcessUpload835File = function (upload835FileId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchUpload835ListPage.ListOfIdsInCSV = upload835FileId > 0 ? upload835FileId.toString() : $scope.SelectedUpload835Ids.toString();
                //Reset Selcted Checkbox items and Control
                $scope.SelectedUpload835Ids = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.Upload835ListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.ProcessUpload835FileUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.Upload835List = response.Data.Items;
                        $scope.Upload835ListPager.currentPageSize = response.Data.Items.length;
                        $scope.Upload835ListPager.totalRecords = response.Data.TotalItems;
                        $scope.CheckUnProcessedFiles();
                    }
                    ShowMessages(response, 6000);
                });
            }
        }, bootboxDialogType.Confirm, title, window.ProcessConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnInfo);
    };

    $scope.Upload835ListPager.getDataCallback = $scope.GetUpload835List;
    $scope.Upload835ListPager.getDataCallback();

    //#region Upload 835 FILE Related Stuff

    $scope.UploadFile = SiteUrl.SaveUpload835FileUrl;
    $scope.UploadingFileList = [];

    $scope.BeforeSend = function (e, data) {

        var isValidFile = true;
        var fileName;
        var errorMsg;
        
        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            
            if ($scope.Upload835Model.A835TemplateType === window.A835TemplateType_Edi && extension !== "edi" && extension !== "txt") {
                ShowMessage(window.Invalid835UploadMessage.replace("{0}", extension), "error");
                errorMsg = window.Invalid835UploadMessage.replace("{0}", extension);
                isValidFile = false;
            }
            else if ($scope.Upload835Model.A835TemplateType === window.A835TemplateType_Paper && extension !== "csv") {
                ShowMessage(window.InvalidPaper835UploadMessage.replace("{0}", extension), "error");
                errorMsg = window.InvalidPaper835UploadMessage.replace("{0}", extension);
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

    $scope.AfterSend = function (e, data) {

        var response = angular.fromJson(data.result);
        ShowMessages(response);
        $scope.IsFileUploading = false;

        $scope.Upload835Model.PayorID = null;
        $scope.Upload835Model.Comment = null;

        $scope.Upload835Model.SelectFileLabel = window.Select835File;
        $scope.Upload835Model.TempFilePath = null;
        if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
            $scope.$apply();
        }

        $scope.Upload835ListPager.getDataCallback();
        myApp.hidePleaseWait();
        $("#fileupload").empty();
    };

    $scope.OnAdd = function (e, data) {

        $scope.Upload835Model.SelectFileLabel = data.files[0].name;

        var isValidFile = true;
        var fileName;
        var errorMsg;
        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            
            if ($scope.Upload835Model.A835TemplateType === window.A835TemplateType_Edi && extension !== "edi" && extension !== "txt") {
                ShowMessage(window.Invalid835UploadMessage.replace("{0}", extension), "error");
                errorMsg = window.Invalid835UploadMessage.replace("{0}", extension);
                isValidFile = false;
            }
            else if ($scope.Upload835Model.A835TemplateType === window.A835TemplateType_Paper && extension !== "csv") {
                ShowMessage(window.InvalidPaper835UploadMessage.replace("{0}", extension), "error");
                errorMsg = window.InvalidPaper835UploadMessage.replace("{0}", extension);
                isValidFile = false;
            }

            fileName = file.name;
        });
        if (!isValidFile) {
            if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                $scope.$apply();
            }
            return false;
        }

        $scope.Upload835Model.TempFilePath = data.files[0].name;
        if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
            $scope.$apply();
        }
        $('#TempFilePath').valid();

    };

    $scope.Upload835File = function () {
        
        if (CheckErrors($($scope.FormID))) {
            $scope.FileData.formData = {
                PayorID: $scope.Upload835Model.PayorID,
                Comment: $scope.Upload835Model.Comment,
                A835TemplateType: $scope.Upload835Model.A835TemplateType
            };
            $scope.FileData.submit();
        }
    };

    //#endregion





    $scope.SaveComment = function (comment, upload835) {
        
        var model = {
            Upload835FileID: upload835.Upload835FileID,
            Comment: comment
        };
        return AngularAjaxCall($http, SiteUrl.SaveUpload835CommentUrl, { upload835CommentModel: model }, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.Upload835ListPager.getDataCallback();
            }
        });
    };


};
controllers.Upload835Controller.$inject = ['$scope', '$http'];