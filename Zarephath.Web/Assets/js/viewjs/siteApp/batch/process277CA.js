var vm;
controllers.Process277Controller = function ($scope, $http) {
    vm = $scope;
    $scope.Process277FileListURL = SiteUrl.Process277FileListURL;
    $scope.Process277List = [];
    $scope.SelectedProcess277Ids = [];
    $scope.SelectAllCheckbox = false;
    $scope.FormID = "#frmProcess277";
    $scope.FileData = null;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnAddProcess277Model").val());
    };
    $scope.Process277Model = $.parseJSON($("#hdnAddProcess277Model").val());
    $scope.AddProcess277Model = $scope.Process277Model.AddProcess277Model;

    $scope.AddProcess277Model.SelectFileLabel = window.Select277File;


    $scope.SearchProcess277ListPage = $scope.Process277Model.SearchProcess277ListPage;
    $scope.TempSearchProcess277ListPage = $scope.Process277Model.SearchProcess277ListPage;
    $scope.Process277ListPager = new PagerModule("Edi277FileID", undefined, 'DESC');


    $scope.SetPostData = function (fromIndex) {
        if ($scope.AddProcess277Model.PayorIDs)
            $scope.AddProcess277Model.PayorIDs = $scope.AddProcess277Model.PayorIDs.toString();
        var pagermodel = {
            searchProcess277Model: $scope.SearchProcess277ListPage,
            model: $scope.AddProcess277Model,
            pageSize: $scope.Process277ListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.Process277ListPager.sortIndex,
            sortDirection: $scope.Process277ListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };
    $scope.SearchModelMapping = function () {
        $scope.SearchProcess277ListPage = $.parseJSON(angular.toJson($scope.TempSearchProcess277ListPage));
    };

    $scope.GetProcess277List = function (isSearchDataMappingRequire) {

        //Reset Selcted Checkbox items and Control
        $scope.SelectedProcess277Ids = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchProcess277ListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        $scope.AjaxStart = true;
        var jsonData = $scope.SetPostData($scope.Process277ListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetEdi277FileListUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.Process277List = response.Data.Items;
                $scope.Process277ListPager.currentPageSize = response.Data.Items.length;
                $scope.Process277ListPager.totalRecords = response.Data.TotalItems;
            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };
    $scope.Refresh = function () {
        $scope.Process277ListPager.currentPage = 1;
        $scope.Process277ListPager.getDataCallback();
    };
    $scope.ResetSearchFilter = function () {
        $scope.AddProcess277Model = $scope.newInstance().AddProcess277Model;
        $scope.SearchProcess277ListPage = $scope.newInstance().SearchProcess277ListPage;
        $scope.TempSearchProcess277ListPage = $scope.newInstance().SearchProcess277ListPage;
        $scope.TempSearchProcess277ListPage.IsDeleted = "0";
        $scope.AddProcess277Model.SelectFileLabel = window.Select277File;
        $scope.Process277ListPager.currentPage = 1;
        $scope.Process277ListPager.getDataCallback();
    };
    $scope.SearchProcess277Files = function () {
        $scope.Process277ListPager.currentPage = 1;
        $scope.Process277ListPager.getDataCallback(true);
    };
    // This executes when select single checkbox selected in table.
    $scope.SelectProcess277File = function (item) {
        
        if (item.IsChecked)
            $scope.SelectedProcess277Ids.push(item.Edi277FileID);
        else
            $scope.SelectedProcess277Ids.remove(item.Edi277FileID);

        if ($scope.SelectedProcess277Ids.length == $scope.Process277ListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };
    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedProcess277Ids = [];
        angular.forEach($scope.Process277List, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedProcess277Ids.push(item.Edi277FileID);
        });
        return true;
    };
    $scope.DeleteProcess277File = function (edi277FileId, title) {

        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            //
            if (result) {
                $scope.SearchProcess277ListPage.ListOfIdsInCSV = edi277FileId > 0 ? edi277FileId.toString() : $scope.SelectedProcess277Ids.toString();
                if (edi277FileId > 0) {
                    if ($scope.Process277ListPager.currentPage != 1)
                        $scope.Process277ListPager.currentPage = $scope.Process277List.length === 1 ? $scope.Process277ListPager.currentPage - 1 : $scope.Process277ListPager.currentPage;
                } else {
                    if ($scope.Process277ListPager.currentPage != 1 && $scope.SelectedProcess277Ids.length == $scope.Process277ListPager.currentPageSize)
                        $scope.Process277ListPager.currentPage = $scope.Process277ListPager.currentPage - 1;
                }
                //Reset Selcted Checkbox items and Control
                $scope.SelectedProcess277Ids = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.Process277ListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteEdi277FileUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.Process277List = response.Data.Items;
                        $scope.Process277ListPager.currentPageSize = response.Data.Items.length;
                        $scope.Process277ListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteConfirmation277Message, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.Process277ListPager.getDataCallback = $scope.GetProcess277List;
    $scope.Process277ListPager.getDataCallback();




    //#region Upload 277 File Code

    $scope.UploadFile = SiteUrl.Upload277FileUrl;
    $scope.UploadingFileList = [];

    $scope.BeforeSend = function (e, data) {

        var isValidFile = true;
        var fileName;
        var errorMsg;

        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            if (extension !== "txt" && extension !== "edi" && extension !== "dat") {
                ShowMessage(window.Invalid277UploadMessage.replace("{0}", extension), "error");
                errorMsg = window.Invalid277UploadMessage.replace("{0}", extension);
                isValidFile = false;
            }
            fileName = file.name;
        });

        if (isValidFile) {
            $scope.IsFileUploading = true;
            $scope.AddProcess277Model.SelectFileLabel = window.Select277File;
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
        $scope.AddProcess277Model.Comment = null;
        $scope.AddProcess277Model.PayorID = null;

        $scope.AddProcess277Model.SelectFileLabel = window.Select277File;
        if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
            $scope.$apply();
        }

        $scope.Process277ListPager.getDataCallback();
        myApp.hidePleaseWait();
        data.files = [];

        $('#fileupload input[type="file"]').val('');
    };

    $scope.OnAdd = function (e, data) {

        $scope.AddProcess277Model.SelectFileLabel = data.files[0].name;

        var isValidFile = true;
        var fileName;
        var errorMsg;
        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            if (extension !== "txt" && extension !== "edi" && extension !== "dat") {
                ShowMessage(window.Invalid277UploadMessage.replace("{0}", extension), "error");
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

        $scope.AddProcess277Model.TempFilePath = data.files[0].name;
        if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
            $scope.$apply();
        }
        $('#AddProcess277Model_TempFilePath').valid();

    };

    $scope.Upload277File = function () {
        if (CheckErrors($($scope.FormID))) {
            $scope.FileData.formData = {
                PayorID: $scope.AddProcess277Model.PayorID ? $scope.AddProcess277Model.PayorID : 0,
                Comment: $scope.AddProcess277Model.Comment ? $scope.AddProcess277Model.Comment : ""
            };
            $scope.FileData.submit();
        }
    };


    $scope.Download277RedableFile = function (edi277FileId) {
        
        var jsonData = angular.toJson({ id: edi277FileId });
        AngularAjaxCall($http, SiteUrl.Download277RedableFileUrl, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
            }
            ShowMessages(response);
        });
    };


    //#endregion

};
controllers.Process277Controller.$inject = ['$scope', '$http'];