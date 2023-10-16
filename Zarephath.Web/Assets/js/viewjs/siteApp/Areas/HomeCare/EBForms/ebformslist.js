var vm;  
controllers.EBFormsListController = function ($scope, $http, $timeout) {
    vm = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetFormListPage").val());
    };

    $scope.AddFormURL = HomeCareSiteUrl.AddFormURL;
    $scope.FormList = [];
    $scope.SelectedFormIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.FormModel = $.parseJSON($("#hdnSetFormListPage").val());
    $scope.SearchFormListPage = $scope.FormModel.SearchEBFormsListPage;
    $scope.TempSearchFormListPage = $scope.FormModel.SearchEBFormsListPage;
    $scope.FormListPager = new PagerModule("ID","", "DESC");
    
    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            SearchEBFormsListPage: $scope.SearchFormListPage,
            pageSize: $scope.FormListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.FormListPager.sortIndex,
            sortDirection: $scope.FormListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchFormListPage = $.parseJSON(angular.toJson($scope.TempSearchFormListPage));
      
    };

    $scope.GetFormList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedFormIds = [];
        $scope.SelectAllCheckbox = false;
       $scope.SearchFormListPage.ListOfIdsInCsv = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping
        
        var jsonData = $scope.SetPostData($scope.FormListPager.currentPage);

        AngularAjaxCall($http, HomeCareSiteUrl.GetFormList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.FormList = response.Data.Items;
                $scope.FormListPager.currentPageSize = response.Data.Items.length;
                $scope.FormListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        //$scope.ResetSearchFilter();
        //$scope.CaseManagerListPager.currentPage = $scope.CaseManagerListPager.currentPage;
        $scope.FormListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');
            $scope.SearchFormListPage = $scope.newInstance().SearchEBFormsListPage;
            $scope.TempSearchFormListPage = $scope.newInstance().SearchEBFormsListPage;
            $scope.TempSearchFormListPage.IsDeleted = "0";
            $scope.FormListPager.currentPage = 1;
            $scope.FormListPager.getDataCallback();
        });
    };
    $scope.SearchForm = function () {
        $scope.FormListPager.currentPage = 1;
        $scope.FormListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectForm = function (Form) {
        if (Form.IsChecked)
            $scope.SelectedFormIds.push(Form.ID);
        else
            $scope.SelectedFormIds.remove(Form.ID);

        if ($scope.SelectedFormIds.length == $scope.FormListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        
        $scope.SelectedFormIds = [];

        angular.forEach($scope.FormList, function (item, key) {
            
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedFormIds.push(item.FormID);
        });
        console.log($scope.SelectedFormIds);
        return true;
    };
   
    $scope.DeleteForm = function (FormId, title) {
       
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchFormListPage.ListOfIdsInCsv = FormId > 0 ? FormId.toString() : $scope.SelectedFormIds.toString();
             //   $scope.SearchFormListPage.ListOfIdsInCsv =  $scope.SelectedFormIds.toString();
                if (FormId > 0) {
                    if ($scope.FormListPager.currentPage != 1)
                        $scope.FormListPager.currentPage = $scope.FormList.length === 1 ? $scope.FormListPager.currentPage - 1 : $scope.FormListPager.currentPage;
                } else {

                    if ($scope.FormListPager.currentPage != 1 && $scope.SelectedFormIds.length == $scope.FormListPager.currentPageSize)
                        $scope.FormListPager.currentPage = $scope.FormListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedFormIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.FormListPager.currentPage);
                console.log(jsonData);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteForm, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.FormList = response.Data.Items;
                        $scope.FormListPager.currentPageSize = response.Data.Items.length;
                        $scope.FormListPager.totalRecords = response.Data.TotalItems;
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

    $scope.FormListPager.getDataCallback = $scope.GetFormList;
    $scope.FormListPager.getDataCallback();



    
};

controllers.EBFormsListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowFormMessage");
});
