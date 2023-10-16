var vm;


controllers.PreferenceListController = function ($scope, $http, $timeout) {
    vm = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetPreferenceListPage").val());
    };

    $scope.AddPreferenceURL = HomeCareSiteUrl.AddPreferenceURL;
    $scope.PreferenceList = [];
    $scope.SelectedPreferenceIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.PreferenceModel = $.parseJSON($("#hdnSetPreferenceListPage").val());
    
    $scope.SearchPreferenceListPage = $scope.PreferenceModel.SearchPreferenceListPage;
    $scope.TempSearchPreferenceListPage = $scope.PreferenceModel.SearchPreferenceListPage;
    
    $scope.PreferenceListPager = new PagerModule("PreferenceID", "", "DESC");
    

    $scope.SetPostData = function (fromIndex) {

        var pagermodel = {
            SearchPreferenceListPage: $scope.SearchPreferenceListPage,
            pageSize: $scope.PreferenceListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.PreferenceListPager.sortIndex,
            sortDirection: $scope.PreferenceListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchPreferenceListPage = $.parseJSON(angular.toJson($scope.TempSearchPreferenceListPage));
      
    };

    $scope.GetPreferenceList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedPreferenceIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchPreferenceListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping
        
        var jsonData = $scope.SetPostData($scope.PreferenceListPager.currentPage);

        AngularAjaxCall($http, HomeCareSiteUrl.GetPreferenceList, jsonData, "Post", "json", "application/json").success(function (response) {

            if (response.IsSuccess) {
                $scope.PreferenceList = response.Data.Items;
                $scope.PreferenceListPager.currentPageSize = response.Data.Items.length;
                $scope.PreferenceListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        //$scope.ResetSearchFilter();
        //$scope.CaseManagerListPager.currentPage = $scope.CaseManagerListPager.currentPage;
        $scope.PreferenceListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');
            $scope.SearchPreferenceListPage = $scope.newInstance().SearchPreferenceListPage;
            $scope.TempSearchPreferenceListPage = $scope.newInstance().SearchPreferenceListPage;
            $scope.TempSearchPreferenceListPage.IsDeleted = "0";
            $scope.TempSearchPreferenceListPage.ServiceCodeID = "";
            $scope.PreferenceListPager.currentPage = 1;
            $scope.PreferenceListPager.getDataCallback();
        });
    };
    $scope.SearchPreference = function () {
        $scope.PreferenceListPager.currentPage = 1;
        $scope.PreferenceListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectPreference = function (Preference) {
        if (Preference.IsChecked)
            $scope.SelectedPreferenceIds.push(Preference.PreferenceID);
        else
            $scope.SelectedPreferenceIds.remove(Preference.PreferenceID);

        if ($scope.SelectedPreferenceIds.length == $scope.PreferenceListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedPreferenceIds = [];

        angular.forEach($scope.PreferenceList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedPreferenceIds.push(item.PreferenceID);
        });
        return true;
    };

    $scope.DeletePreference = function (PreferenceId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchPreferenceListPage.ListOfIdsInCsv = PreferenceId > 0 ? PreferenceId.toString() : $scope.SelectedPreferenceIds.toString();

                if (PreferenceId > 0) {
                    if ($scope.PreferenceListPager.currentPage != 1)
                        $scope.PreferenceListPager.currentPage = $scope.PreferenceList.length === 1 ? $scope.PreferenceListPager.currentPage - 1 : $scope.PreferenceListPager.currentPage;
                } else {

                    if ($scope.PreferenceListPager.currentPage != 1 && $scope.SelectedPreferenceIds.length == $scope.PreferenceListPager.currentPageSize)
                        $scope.PreferenceListPager.currentPage = $scope.PreferenceListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedPreferenceIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.PreferenceListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeletePreference, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.PreferenceList = response.Data.Items;
                        $scope.PreferenceListPager.currentPageSize = response.Data.Items.length;
                        $scope.PreferenceListPager.totalRecords = response.Data.TotalItems;
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
    $scope.PreferenceListPager.getDataCallback = $scope.GetPreferenceList;
    $scope.PreferenceListPager.getDataCallback();

    $scope.PreferenceEditModel = function (EncryptedPreferenceID, title) {
        var EncryptedPreferenceID = EncryptedPreferenceID;
        $('#preference_fixedAside').modal({ backdrop: 'static', keyboard: false });
        $('#preference_fixedAsidelDDLBindIFrame').attr('src', HomeCareSiteUrl.PartialAddPreferenceURL + EncryptedPreferenceID);
    }
    $scope.PreferenceEditModelClosed = function () {
        $scope.Refresh();
        $('#preference_fixedAside').modal('hide');
    }
};

controllers.PreferenceListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowPreferenceMessage");
    //$(".dateInputMask").inputmask("m/d/y", {
    //    placeholder: "mm/dd/yyyy"
    //});
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});
