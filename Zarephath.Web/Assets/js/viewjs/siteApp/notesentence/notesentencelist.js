var vm;
var vm;


controllers.NoteSentenceListController = function ($scope, $http, $timeout) {
    vm = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetNoteSentenceListPage").val());
    };

    $scope.AddNoteSentenceURL = SiteUrl.AddNoteSentenceURL;
    $scope.NoteSentenceList = [];
    $scope.SelectedNoteSentenceIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.NoteSentenceModel = $.parseJSON($("#hdnSetNoteSentenceListPage").val());
    $scope.SearchNoteSentenceListPage = $scope.NoteSentenceModel.SearchNoteSentenceListPage;
    $scope.TempSearchNoteSentenceListPage = $scope.NoteSentenceModel.SearchNoteSentenceListPage;

    $scope.NoteSentenceListPager = new PagerModule("NoteSentenceName");

    $scope.SetPostData = function (fromIndex) {

        var pagermodel = {
            SearchNoteSentenceListPage: $scope.SearchNoteSentenceListPage,
            pageSize: $scope.NoteSentenceListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.NoteSentenceListPager.sortIndex,
            sortDirection: $scope.NoteSentenceListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchNoteSentenceListPage = $.parseJSON(angular.toJson($scope.TempSearchNoteSentenceListPage));
      
    };

    $scope.GetNoteSentenceList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedNoteSentenceIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchNoteSentenceListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping
        
        var jsonData = $scope.SetPostData($scope.NoteSentenceListPager.currentPage);

        AngularAjaxCall($http, SiteUrl.GetNoteSentenceList, jsonData, "Post", "json", "application/json").success(function (response) {

            if (response.IsSuccess) {
                $scope.NoteSentenceList = response.Data.Items;
                debugger;
                $scope.NoteSentenceListPager.currentPageSize = response.Data.Items.length;
                $scope.NoteSentenceListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        //$scope.ResetSearchFilter();
        //$scope.CaseManagerListPager.currentPage = $scope.CaseManagerListPager.currentPage;
        $scope.NoteSentenceListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');
            $scope.SearchNoteSentenceListPage = $scope.newInstance().SearchNoteSentenceListPage;
            $scope.TempSearchNoteSentenceListPage = $scope.newInstance().SearchNoteSentenceListPage;
            $scope.TempSearchNoteSentenceListPage.IsDeleted = "0";
            $scope.NoteSentenceListPager.currentPage = 1;
            $scope.NoteSentenceListPager.getDataCallback();
        });
    };
    $scope.SearchNoteSentence = function () {
        $scope.NoteSentenceListPager.currentPage = 1;
        $scope.NoteSentenceListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectNoteSentence = function (noteSentence) {
        if (noteSentence.IsChecked)
            $scope.SelectedNoteSentenceIds.push(noteSentence.NoteSentenceID);
        else
            $scope.SelectedNoteSentenceIds.remove(noteSentence.NoteSentenceID);

        if ($scope.SelectedNoteSentenceIds.length == $scope.NoteSentenceListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedNoteSentenceIds = [];

        angular.forEach($scope.NoteSentenceList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedNoteSentenceIds.push(item.NoteSentenceID);
        });
        return true;
    };

    $scope.DeleteNoteSentence = function (noteSentenceId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchNoteSentenceListPage.ListOfIdsInCsv = noteSentenceId > 0 ? noteSentenceId.toString() : $scope.SelectedNoteSentenceIds.toString();

                if (noteSentenceId > 0) {
                    if ($scope.NoteSentenceListPager.currentPage != 1)
                        $scope.NoteSentenceListPager.currentPage = $scope.NoteSentenceList.length === 1 ? $scope.NoteSentenceListPager.currentPage - 1 : $scope.NoteSentenceListPager.currentPage;
                } else {

                    if ($scope.NoteSentenceListPager.currentPage != 1 && $scope.SelectedNoteSentenceIds.length == $scope.NoteSentenceListPager.currentPageSize)
                        $scope.NoteSentenceListPager.currentPage = $scope.NoteSentenceListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedNoteSentenceIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.NoteSentenceListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteNoteSentence, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.NoteSentenceList = response.Data.Items;
                        $scope.NoteSentenceListPager.currentPageSize = response.Data.Items.length;
                        $scope.NoteSentenceListPager.totalRecords = response.Data.TotalItems;
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
    $scope.NoteSentenceListPager.getDataCallback = $scope.GetNoteSentenceList;
    $scope.NoteSentenceListPager.getDataCallback();
    $scope.EmployeeEditModel = function (EncryptedNoteSentenceID, title) {
        var EncryptedNoteSentenceID = EncryptedNoteSentenceID;
        $('#NoteSentence_fixedAside').modal({ backdrop: 'static', keyboard: false });
        $('#NoteSentence_fixedAsidelDDLBindIFrame').attr('src', HomeCareSiteUrl.PartialAddNoteSentenceURL + EncryptedNoteSentenceID);
    }
    $scope.EmployeeEditModelClosed = function () {
        $scope.Refresh();
        $('#NoteSentence_fixedAside').modal('hide');
    }
};

controllers.NoteSentenceListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowNoteSentenceMessage");
    //$(".dateInputMask").inputmask("m/d/y", {
    //    placeholder: "mm/dd/yyyy"
    //});
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});
