var vm;
controllers.Process270Controller = function ($scope, $http) {

    vm = $scope;
    $scope.Process270FileListURL = SiteUrl.Process270FileListURL;
    $scope.Process270List = [];
    $scope.SelectedProcess270Ids = [];
    $scope.SelectAllCheckbox = false;
    $scope.FormID = "#frmProcess270";
    $scope.FileData = null;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnAddProcess270Model").val());
    };
    $scope.Process270Model = $.parseJSON($("#hdnAddProcess270Model").val());
    $scope.AddProcess270Model = $scope.Process270Model.AddProcess270Model;


    $scope.Generate270File = function () {
        
        if (CheckErrors("#frmProcess270")) {
            var jsonData = $scope.SetPostData($scope.Process270ListPager.currentPage);
            //jsonData.model = $scope.AddProcess270Model;
            AngularAjaxCall($http, SiteUrl.Generate270FileUrl, jsonData, "Post", "json", "application/json").success(function(response) {
                if (response.IsSuccess) {
                    $scope.Process270List = response.Data.Items;
                    $scope.Process270ListPager.currentPageSize = response.Data.Items.length;
                    $scope.Process270ListPager.totalRecords = response.Data.TotalItems;
                    $scope.AddProcess270Model = $scope.newInstance().AddProcess270Model;
                }
                ShowMessages(response, 6000);
            });

        }
    };
    
    $scope.SearchProcess270ListPage = $scope.Process270Model.SearchProcess270ListPage;
    $scope.TempSearchProcess270ListPage = $scope.Process270Model.SearchProcess270ListPage;
    $scope.Process270ListPager = new PagerModule("Edi270271FileID", undefined, 'DESC');
    
    $scope.SetPostData = function (fromIndex) {
        if ($scope.AddProcess270Model.PayorIDs)
            $scope.AddProcess270Model.PayorIDs = $scope.AddProcess270Model.PayorIDs.toString();
        var pagermodel = {
            searchProcess270Model: $scope.SearchProcess270ListPage,
            model: $scope.AddProcess270Model,
            pageSize: $scope.Process270ListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.Process270ListPager.sortIndex,
            sortDirection: $scope.Process270ListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };
    $scope.SearchModelMapping = function () {
        $scope.SearchProcess270ListPage = $.parseJSON(angular.toJson($scope.TempSearchProcess270ListPage));
    };

    $scope.GetProcess270List = function (isSearchDataMappingRequire) {

        //Reset Selcted Checkbox items and Control
        $scope.SelectedProcess270Ids = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchProcess270ListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control


        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        $scope.AjaxStart = true;
        var jsonData = $scope.SetPostData($scope.Process270ListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetEdi270FileListUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.Process270List = response.Data.Items;
                $scope.Process270ListPager.currentPageSize = response.Data.Items.length;
                $scope.Process270ListPager.totalRecords = response.Data.TotalItems;
            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };
    $scope.Refresh = function () {
        $scope.Process270ListPager.currentPage = 1;
        $scope.Process270ListPager.getDataCallback();
    };
    $scope.ResetSearchFilter = function () {
        $scope.AddProcess270Model = $scope.newInstance().AddProcess270Model;
        $scope.SearchProcess270ListPage = $scope.newInstance().SearchProcess270ListPage;
        $scope.TempSearchProcess270ListPage = $scope.newInstance().SearchProcess270ListPage;
        $scope.TempSearchProcess270ListPage.IsDeleted = "0";
        $scope.Process270ListPager.currentPage = 1;
        $scope.Process270ListPager.getDataCallback();
    };
    $scope.SearchProcess270Files = function () {
        $scope.Process270ListPager.currentPage = 1;
        $scope.Process270ListPager.getDataCallback(true);
    };
    // This executes when select single checkbox selected in table.
    $scope.SelectProcess270File = function (item) {
        if (item.IsChecked)
            $scope.SelectedProcess270Ids.push(item.Edi270271FileID);
        else
            $scope.SelectedProcess270Ids.remove(item.Edi270271FileID);

        if ($scope.SelectedProcess270Ids.length == $scope.Process270ListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };
    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedProcess270Ids = [];
        angular.forEach($scope.Process270List, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedProcess270Ids.push(item.Edi270271FileID);
        });
        return true;
    };
    $scope.DeleteProcess270File = function (edi270FileId, title) {

        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            //
            if (result) {
                $scope.SearchProcess270ListPage.ListOfIdsInCSV = edi270FileId > 0 ? edi270FileId.toString() : $scope.SelectedProcess270Ids.toString();
                if (edi270FileId > 0) {
                    if ($scope.Process270ListPager.currentPage != 1)
                        $scope.Process270ListPager.currentPage = $scope.Process270List.length === 1 ? $scope.Process270ListPager.currentPage - 1 : $scope.Process270ListPager.currentPage;
                } else {
                    if ($scope.Process270ListPager.currentPage != 1 && $scope.SelectedProcess270Ids.length == $scope.Process270ListPager.currentPageSize)
                        $scope.Process270ListPager.currentPage = $scope.Process270ListPager.currentPage - 1;
                }
                //Reset Selcted Checkbox items and Control
                $scope.SelectedProcess270Ids = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.Process270ListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteEdi270FileUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.Process270List = response.Data.Items;
                        $scope.Process270ListPager.currentPageSize = response.Data.Items.length;
                        $scope.Process270ListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteConfirmation270Message, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.Process270ListPager.getDataCallback = $scope.GetProcess270List;
    $scope.Process270ListPager.getDataCallback();

    //$("a#process270").on('shown.bs.tab', function (e) {
    //    $(".tab-pane a[href='#tab_Process270']").tab('show');
    //    $scope.Process270ListPager.getDataCallback();
    //});



};
controllers.Process270Controller.$inject = ['$scope', '$http'];