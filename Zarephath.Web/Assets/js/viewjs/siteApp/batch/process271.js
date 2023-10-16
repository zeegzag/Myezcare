var vm;
controllers.Process271Controller = function ($scope, $http) {

    vm = $scope;
    $scope.Process271FileListURL = SiteUrl.Process271FileListURL;
    $scope.Process271List = [];
    $scope.SelectedProcess271Ids = [];
    $scope.SelectAllCheckbox = false;
    $scope.FormID = "#frmProcess271";
    $scope.FileData = null;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnAddProcess271Model").val());
    };
    $scope.Process271Model = $.parseJSON($("#hdnAddProcess271Model").val());
    $scope.AddProcess271Model = $scope.Process271Model.AddProcess271Model;
    $scope.AddProcess271Model.SelectFileLabel = window.Select271File;


    $scope.SearchProcess271ListPage = $scope.Process271Model.SearchProcess271ListPage;
    $scope.TempSearchProcess271ListPage = $scope.Process271Model.SearchProcess271ListPage;
    $scope.Process271ListPager = new PagerModule("Edi270271FileID", undefined, 'DESC');

    $scope.SetPostData = function (fromIndex) {
        if ($scope.AddProcess271Model.PayorIDs)
            $scope.AddProcess271Model.PayorIDs = $scope.AddProcess271Model.PayorIDs.toString();
        var pagermodel = {
            searchProcess271Model: $scope.SearchProcess271ListPage,
            model: $scope.AddProcess271Model,
            pageSize: $scope.Process271ListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.Process271ListPager.sortIndex,
            sortDirection: $scope.Process271ListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };
    $scope.SearchModelMapping = function () {
        $scope.SearchProcess271ListPage = $.parseJSON(angular.toJson($scope.TempSearchProcess271ListPage));
    };

    $scope.GetProcess271List = function (isSearchDataMappingRequire) {

        //Reset Selcted Checkbox items and Control
        $scope.SelectedProcess271Ids = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchProcess271ListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        $scope.AjaxStart = true;
        var jsonData = $scope.SetPostData($scope.Process271ListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetEdi271FileListUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.Process271List = response.Data.Items;
                $scope.Process271ListPager.currentPageSize = response.Data.Items.length;
                $scope.Process271ListPager.totalRecords = response.Data.TotalItems;
            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };
    $scope.Refresh = function () {
        $scope.Process271ListPager.currentPage = 1;
        $scope.Process271ListPager.getDataCallback();
    };
    $scope.ResetSearchFilter = function () {
        $scope.AddProcess271Model = $scope.newInstance().AddProcess271Model;
        $scope.SearchProcess271ListPage = $scope.newInstance().SearchProcess271ListPage;
        $scope.TempSearchProcess271ListPage = $scope.newInstance().SearchProcess271ListPage;
        $scope.TempSearchProcess271ListPage.IsDeleted = "0";
        $scope.AddProcess271Model.SelectFileLabel = window.Select271File;
        $scope.Process271ListPager.currentPage = 1;
        $scope.Process271ListPager.getDataCallback();
    };
    $scope.SearchProcess271Files = function () {
        $scope.Process271ListPager.currentPage = 1;
        $scope.Process271ListPager.getDataCallback(true);
    };
    // This executes when select single checkbox selected in table.
    $scope.SelectProcess271File = function (item) {
        if (item.IsChecked)
            $scope.SelectedProcess271Ids.push(item.Edi270271FileID);
        else
            $scope.SelectedProcess271Ids.remove(item.Edi270271FileID);

        if ($scope.SelectedProcess271Ids.length == $scope.Process271ListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };
    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedProcess271Ids = [];
        angular.forEach($scope.Process271List, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedProcess271Ids.push(item.Edi270271FileID);
        });
        return true;
    };
    $scope.DeleteProcess271File = function (edi271FileId, title) {

        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            //
            if (result) {
                $scope.SearchProcess271ListPage.ListOfIdsInCSV = edi271FileId > 0 ? edi271FileId.toString() : $scope.SelectedProcess271Ids.toString();
                if (edi271FileId > 0) {
                    if ($scope.Process271ListPager.currentPage != 1)
                        $scope.Process271ListPager.currentPage = $scope.Process271List.length === 1 ? $scope.Process271ListPager.currentPage - 1 : $scope.Process271ListPager.currentPage;
                } else {
                    if ($scope.Process271ListPager.currentPage != 1 && $scope.SelectedProcess271Ids.length == $scope.Process271ListPager.currentPageSize)
                        $scope.Process271ListPager.currentPage = $scope.Process271ListPager.currentPage - 1;
                }
                //Reset Selcted Checkbox items and Control
                $scope.SelectedProcess271Ids = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.Process271ListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteEdi271FileUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.Process271List = response.Data.Items;
                        $scope.Process271ListPager.currentPageSize = response.Data.Items.length;
                        $scope.Process271ListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteConfirmation271Message, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.Process271ListPager.getDataCallback = $scope.GetProcess271List;
    //$scope.Process271ListPager.getDataCallback();
    $("a#process271").on('shown.bs.tab', function (e) {
        if ($scope.Process271List.length === 0) {
            $(".tab-pane a[href='#tab_Process271']").tab('show');
            $scope.Process271ListPager.getDataCallback();
        }
    });




    //#region Upload 271 File Code

    $scope.UploadFile = SiteUrl.Upload271FileUrl;
    $scope.UploadingFileList = [];

    $scope.BeforeSend = function (e, data) {

        var isValidFile = true;
        var fileName;
        var errorMsg;
        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            if (extension !== "txt") {
                ShowMessage(window.Invalid271UploadMessage.replace("{0}", extension), "error");
                errorMsg = window.Invalid271UploadMessage.replace("{0}", extension);
                isValidFile = false;
            }
            fileName = file.name;
        });

        if (isValidFile) {
            $scope.IsFileUploading = true;
            $scope.AddProcess271Model.SelectFileLabel = window.Select271File;
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
        
        $scope.AddProcess271Model.Comment = null;
       
        $scope.AddProcess271Model.SelectFileLabel = window.Select271File;
        if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
            $scope.$apply();
        }

        $scope.Process271ListPager.getDataCallback();
        myApp.hidePleaseWait();
        data.files = [];

        $('#fileupload input[type="file"]').val('');
    };

    $scope.OnAdd = function (e, data) {

        $scope.AddProcess271Model.SelectFileLabel = data.files[0].name;

        var isValidFile = true;
        var fileName;
        var errorMsg;
        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            if (extension !== "txt") {
                ShowMessage(window.Invalid271UploadMessage.replace("{0}", extension), "error");
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

        $scope.AddProcess271Model.TempFilePath = data.files[0].name;
        if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
            $scope.$apply();
        }
        $('#AddProcess271Model_TempFilePath').valid();

    };

    $scope.Upload271File = function () {
        if (CheckErrors($($scope.FormID))) {
            $scope.FileData.formData = {
                Comment: $scope.AddProcess271Model.Comment? $scope.AddProcess271Model.Comment:"",
            };
            $scope.FileData.submit();
        }
    };

    //#endregion

};
controllers.Process271Controller.$inject = ['$scope', '$http'];