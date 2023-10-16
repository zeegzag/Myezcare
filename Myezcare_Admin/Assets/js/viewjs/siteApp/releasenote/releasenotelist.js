var custModel;
controllers.ReleaseNoteListController = function ($scope, $http, $timeout) {
    custModel = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetReleaseNoteListPage").val());
    };
    $scope.ReleaseNoteModel = $.parseJSON($("#hdnSetReleaseNoteListPage").val());

    $scope.AddReleaseNoteURL = SiteUrl.AddReleaseNoteURL;
    $scope.ViewReleaseNoteURL = SiteUrl.ViewReleaseNoteURL;
    $scope.ReleaseNoteList = [];
    $scope.SelectedReleaseNoteIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.SearchReleaseNoteListPage = $scope.ReleaseNoteModel.SearchReleaseNoteListPage;
    $scope.TempSearchReleaseNoteListPage = $scope.ReleaseNoteModel.SearchReleaseNoteListPage;
    $scope.ReleaseNoteListPager = new PagerModule("StartDate", "", "DESC");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            SearchReleaseNoteListPage: $scope.SearchReleaseNoteListPage,
            pageSize: $scope.ReleaseNoteListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ReleaseNoteListPager.sortIndex,
            sortDirection: $scope.ReleaseNoteListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchReleaseNoteListPage = $.parseJSON(angular.toJson($scope.TempSearchReleaseNoteListPage));
    };

    $scope.GetReleaseNoteList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedReleaseNoteIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchReleaseNoteListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.ReleaseNoteListPager.currentPage);

        AngularAjaxCall($http, SiteUrl.GetReleaseNoteList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ReleaseNoteList = response.Data.Items;
                $scope.ReleaseNoteListPager.currentPageSize = response.Data.Items.length;
                $scope.ReleaseNoteListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        //$scope.ResetSearchFilter();
        //$scope.CaseManagerListPager.currentPage = $scope.CaseManagerListPager.currentPage;
        $scope.ReleaseNoteListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            $scope.SearchReleaseNoteListPage = $scope.newInstance().SearchReleaseNoteListPage;
            $scope.TempSearchReleaseNoteListPage = $scope.newInstance().SearchReleaseNoteListPage;
            $scope.TempSearchReleaseNoteListPage.IsDeleted = "0";
            $scope.ReleaseNoteListPager.currentPage = 1;
            $scope.ReleaseNoteListPager.getDataCallback();
        });
    };

    $scope.SearchReleaseNote = function () {
        $scope.ReleaseNoteListPager.currentPage = 1;
        $scope.ReleaseNoteListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectReleaseNote = function (ReleaseNote) {
        if (ReleaseNote.IsChecked)
            $scope.SelectedReleaseNoteIds.push(ReleaseNote.ReleaseNoteID);
        else
            $scope.SelectedReleaseNoteIds.remove(ReleaseNote.ReleaseNoteID);

        if ($scope.SelectedReleaseNoteIds.length == $scope.ReleaseNoteListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedReleaseNoteIds = [];

        angular.forEach($scope.ReleaseNoteList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedReleaseNoteIds.push(item.ReleaseNoteID);
        });
        return true;
    };

    $scope.DeleteReleaseNote = function (ReleaseNoteId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchReleaseNoteListPage.ListOfIdsInCsv = ReleaseNoteId > 0 ? ReleaseNoteId.toString() : $scope.SelectedReleaseNoteIds.toString();

                if (ReleaseNoteId > 0) {
                    if ($scope.ReleaseNoteListPager.currentPage != 1)
                        $scope.ReleaseNoteListPager.currentPage = $scope.ReleaseNoteList.length === 1 ? $scope.ReleaseNoteListPager.currentPage - 1 : $scope.ReleaseNoteListPager.currentPage;
                } else {

                    if ($scope.ReleaseNoteListPager.currentPage != 1 && $scope.SelectedReleaseNoteIds.length == $scope.ReleaseNoteListPager.currentPageSize)
                        $scope.ReleaseNoteListPager.currentPage = $scope.ReleaseNoteListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedReleaseNoteIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.ReleaseNoteListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteReleaseNoteURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.ReleaseNoteList = response.Data.Items;
                        $scope.ReleaseNoteListPager.currentPageSize = response.Data.Items.length;
                        $scope.ReleaseNoteListPager.totalRecords = response.Data.TotalItems;
                    }
                });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.DatePickerDate = function (modelDate, newDate) {
        var a;
        if (modelDate) {
            var dt = new Date(modelDate);
            dt >= newDate ? a = newDate : a = dt;
        }
        else {
            a = newDate;
        }
        return moment(a).format('L');
    };
    $scope.ReleaseNoteListPager.getDataCallback = $scope.GetReleaseNoteList;
    $scope.ReleaseNoteListPager.getDataCallback();
};

controllers.ReleaseNoteListController.$inject = ['$scope', '$http', '$timeout'];
$(document).ready(function () {
    ShowPageLoadMessage("ShowReleaseNoteMessage");
});
