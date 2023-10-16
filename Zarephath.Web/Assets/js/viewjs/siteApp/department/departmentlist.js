var vm;

controllers.DepartmentListController = function ($scope, $http, $timeout) {
    vm = $scope;
    $scope.AddDepartmentURL = SiteUrl.AddDepartmentURL;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnDepartmentListModel").val());
    };

    $scope.DepartmentModel = $.parseJSON($("#hdnDepartmentListModel").val());
    $scope.TempSearchDepartmentModel = $scope.DepartmentModel.SearchDepartmentModel;
    $scope.SearchDepartmentModel = $scope.DepartmentModel.SearchDepartmentModel;
    $scope.DepartmentListPager = new PagerModule("DepartmentName");

    $scope.DepartmentList = [];
    $scope.SelectedDepartmentIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.SetPostData = function (fromIndex) {

        var pagermodel = {
            searchDepartmentModel: $scope.SearchDepartmentModel,
            pageSize: $scope.DepartmentListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.DepartmentListPager.sortIndex,
            sortDirection: $scope.DepartmentListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };


    $scope.SearchModelMapping = function () {
        $scope.SearchDepartmentModel = $.parseJSON(angular.toJson($scope.TempSearchDepartmentModel));
        //$scope.SearchDepartmentModel.DepartmentID = $scope.TempSearchDepartmentModel.DepartmentID;
        //$scope.SearchDepartmentModel.EmployeeID = $scope.TempSearchDepartmentModel.EmployeeID;
        //$scope.SearchDepartmentModel.Address = $scope.TempSearchDepartmentModel.Address;
        //$scope.SearchDepartmentModel.Location = $scope.TempSearchDepartmentModel.Location;
        //$scope.SearchDepartmentModel.ListOfIdsInCSV = $scope.TempSearchDepartmentModel.ListOfIdsInCSV;
        //$scope.SearchDepartmentModel.IsDeleted = $scope.TempSearchDepartmentModel.IsDeleted;

    };

    $scope.GetDepartmentList = function (isSearchDataMappingRequire) {

        //Reset Selcted Checkbox items and Control
        $scope.SelectedDepartmentIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchDepartmentModel.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control


        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping



        //STEP 2:   Seaching with Paging
        var jsonData = $scope.SetPostData($scope.DepartmentListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetDepartmentListURL, jsonData, "Post", "json", "application/json").success(function (response) {

            if (response.IsSuccess) {
                $scope.DepartmentList = response.Data.Items;
                $scope.DepartmentListPager.currentPageSize = response.Data.Items.length;
                $scope.DepartmentListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
        //STEP 2:   Seaching with Paging

    };

    $scope.Refresh = function () {
        $scope.DepartmentListPager.getDataCallback();
    };


    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            //$("#SearchDepartment").select2("val", '');
            //$("#SearchEmployee").select2("val", '');
            $scope.SearchDepartmentModel = $scope.newInstance().SearchDepartmentModel;
            $scope.TempSearchDepartmentModel = $scope.newInstance().SearchDepartmentModel;
            $scope.TempSearchDepartmentModel.IsDeleted = "0";
            $scope.DepartmentListPager.currentPage = 1;
            $scope.DepartmentListPager.getDataCallback();
        });
    };

    $scope.SearchDepartment = function () {
        $scope.DepartmentListPager.currentPage = 1;
        $scope.DepartmentListPager.getDataCallback(true);
    };

    $scope.SelectDepartment = function (department) {

        if (department.IsChecked)
            $scope.SelectedDepartmentIds.push(department.DepartmentID);
        else
            $scope.SelectedDepartmentIds.remove(department.DepartmentID);

        if ($scope.SelectedDepartmentIds.length == $scope.DepartmentListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    $scope.SelectAll = function () {
        $scope.SelectedDepartmentIds = [];
        angular.forEach($scope.DepartmentList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedDepartmentIds.push(item.DepartmentID);
        });

        return true;
    };

    $scope.DeleteDepartment = function (department, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        // #region Scrap Code
        //if (department != undefined && department.EmpCount > 0) {
        //    bootboxDialog(function () {
        //    }, bootboxDialogType.Alert, window.Alert, window.DepartmentEmployeeExistMessage);
        //} else {
        //    bootboxDialog(function (result) {
        //        if (result) {

        //            if (department == undefined)
        //                $scope.SearchDepartmentModel.ListOfIdsInCSV = $scope.SelectedDepartmentIds.toString();
        //            else
        //                $scope.SearchDepartmentModel.ListOfIdsInCSV = department.DepartmentID > 0 ? department.DepartmentID.toString() : $scope.SelectedDepartmentIds.toString();

        //            if (department != undefined && department.DepartmentID > 0) {
        //                if ($scope.DepartmentListPager.currentPage != 1)
        //                    $scope.DepartmentListPager.currentPage = $scope.DepartmentList.length === 1 ? $scope.DepartmentListPager.currentPage - 1 : $scope.DepartmentListPager.currentPage;
        //            } else {

        //                if ($scope.DepartmentListPager.currentPage != 1 && $scope.SelectedDepartmentIds.length == $scope.DepartmentListPager.currentPageSize)
        //                    $scope.DepartmentListPager.currentPage = $scope.DepartmentListPager.currentPage - 1;
        //            }


        //            var jsonData = $scope.SetPostData($scope.DepartmentListPager.currentPage);
        //            AngularAjaxCall($http, SiteUrl.DeleteDepartmentURL, jsonData, "Post", "json", "application/json").success(function (response) {
        //                if (response.IsSuccess) {
        //                    //Reset Selcted Checkbox items and Control
        //                    $scope.SelectedDepartmentIds = [];
        //                    $scope.SelectAllCheckbox = false;
        //                    //Reset Selcted Checkbox items and Control
        //                    $scope.DepartmentList = response.Data.Items;
        //                    $scope.DepartmentListPager.currentPageSize = response.Data.Items.length;
        //                    $scope.DepartmentListPager.totalRecords = response.Data.TotalItems;
        //                    ShowMessages(response);
        //                } else {
        //                    bootboxDialog(function () {
        //                    }, bootboxDialogType.Alert, window.Alert, window.DepartmentEmployeeExistMessage);
        //                }

        //            });
        //        }
        //    }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
        //}
        //#endregion
        bootboxDialog(function (result) {
            if (result) {

                if (department == undefined)
                    $scope.SearchDepartmentModel.ListOfIdsInCSV = $scope.SelectedDepartmentIds.toString();
                else
                    $scope.SearchDepartmentModel.ListOfIdsInCSV = department.DepartmentID > 0 ? department.DepartmentID.toString() : $scope.SelectedDepartmentIds.toString();

                if (department != undefined && department.DepartmentID > 0) {
                    if ($scope.DepartmentListPager.currentPage != 1)
                        $scope.DepartmentListPager.currentPage = $scope.DepartmentList.length === 1 ? $scope.DepartmentListPager.currentPage - 1 : $scope.DepartmentListPager.currentPage;
                } else {

                    if ($scope.DepartmentListPager.currentPage != 1 && $scope.SelectedDepartmentIds.length == $scope.DepartmentListPager.currentPageSize)
                        $scope.DepartmentListPager.currentPage = $scope.DepartmentListPager.currentPage - 1;
                }


                var jsonData = $scope.SetPostData($scope.DepartmentListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteDepartmentURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        //Reset Selcted Checkbox items and Control
                        $scope.SelectedDepartmentIds = [];
                        $scope.SelectAllCheckbox = false;
                        //Reset Selcted Checkbox items and Control
                        $scope.DepartmentList = response.Data.Items;
                        $scope.DepartmentListPager.currentPageSize = response.Data.Items.length;
                        $scope.DepartmentListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);

                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.DepartmentListPager.getDataCallback = $scope.GetDepartmentList;
    $scope.DepartmentListPager.getDataCallback();

};

controllers.DepartmentListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("DepartmentUpdateSuccessMessage");
});