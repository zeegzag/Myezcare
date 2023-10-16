var vm;


controllers.EmployeeVisitNoteListController = function ($scope, $http, $timeout) {
    vm = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetEmployeeVisitNoteListPage").val());
    };
    
    $scope.EmployeeVisitNoteList = [];
    $scope.SelectedEmployeeVisitNoteIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.EmployeeVisitNoteModel = $.parseJSON($("#hdnSetEmployeeVisitNoteListPage").val());
    $scope.SearchEmployeeVisitNoteListPage = $scope.EmployeeVisitNoteModel.SearchEmployeeVisitNoteListPage;
    $scope.TempSearchEmployeeVisitNoteListPage = $scope.EmployeeVisitNoteModel.SearchEmployeeVisitNoteListPage;
    
    $scope.EmployeeVisitNoteListPager = new PagerModule("EmployeeVisitNoteID");

    $scope.SetPostData = function (fromIndex) {

        var pagermodel = {
            SearchEmployeeVisitNoteListPage: $scope.SearchEmployeeVisitNoteListPage,
            pageSize: $scope.EmployeeVisitNoteListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.EmployeeVisitNoteListPager.sortIndex,
            sortDirection: $scope.EmployeeVisitNoteListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchEmployeeVisitNoteListPage = $.parseJSON(angular.toJson($scope.TempSearchEmployeeVisitNoteListPage));
      
    };

    $scope.GetEmployeeVisitNoteList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedEmployeeVisitNoteIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchEmployeeVisitNoteListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control
        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping
        
        var jsonData = $scope.SetPostData($scope.EmployeeVisitNoteListPager.currentPage);

        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeVisitNoteList, jsonData, "Post", "json", "application/json").success(function (response) {

            if (response.IsSuccess) {
                $scope.EmployeeVisitNoteList = response.Data.Items;
                $scope.EmployeeVisitNoteListPager.currentPageSize = response.Data.Items.length;
                $scope.EmployeeVisitNoteListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        //$scope.ResetSearchFilter();
        //$scope.CaseManagerListPager.currentPage = $scope.CaseManagerListPager.currentPage;
        $scope.EmployeeVisitNoteListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');
            $scope.SearchEmployeeVisitNoteListPage = $scope.newInstance().SearchEmployeeVisitNoteListPage;
            $scope.TempSearchEmployeeVisitNoteListPage = $scope.newInstance().SearchEmployeeVisitNoteListPage;
            $scope.TempSearchEmployeeVisitNoteListPage.IsDeleted = "0";
            $scope.EmployeeVisitNoteListPager.currentPage = 1;
            $scope.EmployeeVisitNoteListPager.getDataCallback();
        });
    };
    $scope.SearchEmployeeVisitNote = function () {
        $scope.EmployeeVisitNoteListPager.currentPage = 1;
        $scope.EmployeeVisitNoteListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectEmployeeVisitNote = function (EmployeeVisitNote) {
        if (EmployeeVisitNote.IsChecked)
            $scope.SelectedEmployeeVisitNoteIds.push(EmployeeVisitNote.EmployeeVisitNoteID);
        else
            $scope.SelectedEmployeeVisitNoteIds.remove(EmployeeVisitNote.EmployeeVisitNoteID);

        if ($scope.SelectedEmployeeVisitNoteIds.length == $scope.EmployeeVisitNoteListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedEmployeeVisitNoteIds = [];

        angular.forEach($scope.EmployeeVisitNoteList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedEmployeeVisitNoteIds.push(item.EmployeeVisitNoteID);
        });
        return true;
    };

    $scope.DeleteEmployeeVisitNote = function (EmployeeVisitNoteID, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchEmployeeVisitNoteListPage.ListOfIdsInCsv = EmployeeVisitNoteID > 0 ? EmployeeVisitNoteID.toString() : $scope.SelectedEmployeeVisitNoteIds.toString();

                if (EmployeeVisitNoteID > 0) {
                    if ($scope.EmployeeVisitNoteListPager.currentPage != 1)
                        $scope.EmployeeVisitNoteListPager.currentPage = $scope.EmployeeVisitNoteList.length === 1 ? $scope.EmployeeVisitNoteListPager.currentPage - 1 : $scope.EmployeeVisitNoteListPager.currentPage;
                } else {

                    if ($scope.EmployeeVisitNoteListPager.currentPage != 1 && $scope.SelectedEmployeeVisitNoteIds.length == $scope.EmployeeVisitNoteListPager.currentPageSize)
                        $scope.EmployeeVisitNoteListPager.currentPage = $scope.EmployeeVisitNoteListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedEmployeeVisitNoteIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.EmployeeVisitNoteListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteEmployeeVisitNote, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.EmployeeVisitNoteList = response.Data.Items;
                        $scope.EmployeeVisitNoteListPager.currentPageSize = response.Data.Items.length;
                        $scope.EmployeeVisitNoteListPager.totalRecords = response.Data.TotalItems;
                    }
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
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
    $scope.EmployeeVisitNoteListPager.getDataCallback = $scope.GetEmployeeVisitNoteList;
    $scope.EmployeeVisitNoteListPager.getDataCallback();
   
};

controllers.EmployeeVisitNoteListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    var dateformat = GetOrgDateFormat();
    $(".dateInputMask").attr("placeholder", dateformat);
    $('.time').inputmask({
        mask: "h:s t\\m",
        placeholder: "hh:mm a",
        alias: "datetime",
        hourFormat: "12"
    });

});
