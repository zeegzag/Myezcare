var vm;
controllers.PhysicianListController = function ($scope, $http, $timeout) {
    vm = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetPhysicianListPage").val());
    };

    $scope.AddPhysicianURL = HomeCareSiteUrl.AddPhysicianURL;
    $scope.PhysicianList = [];
    $scope.SelectedPhysicianIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.PhysicianModel = $.parseJSON($("#hdnSetPhysicianListPage").val());
    $scope.SearchPhysicianListPage = $scope.PhysicianModel.SearchPhysicianListPage;
    $scope.TempSearchPhysicianListPage = $scope.PhysicianModel.SearchPhysicianListPage;
    $scope.PhysicianListPager = new PagerModule("PhysicianID","", "DESC");
    
    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            SearchPhysicianListPage: $scope.SearchPhysicianListPage,
            pageSize: $scope.PhysicianListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.PhysicianListPager.sortIndex,
            sortDirection: $scope.PhysicianListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchPhysicianListPage = $.parseJSON(angular.toJson($scope.TempSearchPhysicianListPage));
      
    };

    $scope.GetPhysicianList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedPhysicianIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchPhysicianListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping
        
        var jsonData = $scope.SetPostData($scope.PhysicianListPager.currentPage);

        AngularAjaxCall($http, HomeCareSiteUrl.GetPhysicianList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.PhysicianList = response.Data.Items;
                $scope.PhysicianListPager.currentPageSize = response.Data.Items.length;
                $scope.PhysicianListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        //$scope.ResetSearchFilter();
        //$scope.CaseManagerListPager.currentPage = $scope.CaseManagerListPager.currentPage;
        $scope.PhysicianListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');
            $scope.SearchPhysicianListPage = $scope.newInstance().SearchPhysicianListPage;
            $scope.TempSearchPhysicianListPage = $scope.newInstance().SearchPhysicianListPage;
            $scope.TempSearchPhysicianListPage.IsDeleted = "0";
            $scope.PhysicianListPager.currentPage = 1;
            $scope.PhysicianListPager.getDataCallback();
        });
    };
    $scope.SearchPhysician = function () {
        $scope.PhysicianListPager.currentPage = 1;
        $scope.PhysicianListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectPhysician = function (Physician) {
        if (Physician.IsChecked)
            $scope.SelectedPhysicianIds.push(Physician.PhysicianID);
        else
            $scope.SelectedPhysicianIds.remove(Physician.PhysicianID);

        if ($scope.SelectedPhysicianIds.length == $scope.PhysicianListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedPhysicianIds = [];

        angular.forEach($scope.PhysicianList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedPhysicianIds.push(item.PhysicianID);
        });
        return true;
    };
   
    $scope.DeletePhysician = function (PhysicianId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchPhysicianListPage.ListOfIdsInCsv = PhysicianId > 0 ? PhysicianId.toString() : $scope.SelectedPhysicianIds.toString();

                if (PhysicianId > 0) {
                    if ($scope.PhysicianListPager.currentPage != 1)
                        $scope.PhysicianListPager.currentPage = $scope.PhysicianList.length === 1 ? $scope.PhysicianListPager.currentPage - 1 : $scope.PhysicianListPager.currentPage;
                } else {

                    if ($scope.PhysicianListPager.currentPage != 1 && $scope.SelectedPhysicianIds.length == $scope.PhysicianListPager.currentPageSize)
                        $scope.PhysicianListPager.currentPage = $scope.PhysicianListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedPhysicianIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.PhysicianListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeletePhysician, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.PhysicianList = response.Data.Items;
                        $scope.PhysicianListPager.currentPageSize = response.Data.Items.length;
                        $scope.PhysicianListPager.totalRecords = response.Data.TotalItems;
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

    $scope.PhysicianListPager.getDataCallback = $scope.GetPhysicianList;
    $scope.PhysicianListPager.getDataCallback();


   $scope.PhysicianEditModel = function (EncryptedPhysicianID, title) {
        var EncryptedPhysicianID = EncryptedPhysicianID;
        $('#physician_fixedAside').modal({ backdrop: 'static', keyboard: false });;
        $('#physician_fixedAsidelDDLBindIFrame').attr('src', HomeCareSiteUrl.PartialAddPhysicianURL + EncryptedPhysicianID);
    }
    $scope.PhysicianEditModelClosed = function () {
        $scope.Refresh();
        $('#physician_fixedAside').modal('hide');
    }
    
};

controllers.PhysicianListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowPhysicianMessage");
});
