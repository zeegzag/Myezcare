var vm;
controllers.PermissionsListController = function ($scope, $http, $window, $timeout) {
    vm = $scope;
    $scope.AddServicePlanURL = SiteUrl.AddServicePlanURL;

    //$scope.newInstance = function () {
    //    return $.parseJSON($("#hdnSetReleaseNoteListPage").val());
    //};
    //$scope.ReleaseNoteModel = $.parseJSON($("#hdnSetReleaseNoteListPage").val());
    $scope.AddPermissionURL = SiteUrl.SetAddPermissionURL;

    $scope.SetPermissionsListModel = $.parseJSON($("#hdnPermissionListPage").val());
    $scope.PermissionsList = [];
    $scope.PermissionsListOriginal = [];

    $scope.SearchPermissionsModel = $scope.SetPermissionsListModel.SearchPermissionsModel;
    $scope.SelectedPermissionIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.PermissionsListPager = new PagerModule("PermissionID", undefined, "ASC");
    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            SearchPermissionsModel: $scope.SearchPermissionsModel,
            pageSize: $scope.PermissionsListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.PermissionsListPager.sortIndex,
            sortDirection: $scope.PermissionsListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.GetPermissionList = function () {
        var jsonData = $scope.SetPostData($scope.PermissionsListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetPermissionsListURL, jsonData, "Get", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.PermissionsList = response.Data.Items;
                $scope.PermissionsListPager.currentPageSize = response.Data.Items.length;
                $scope.PermissionsListPager.totalRecords = response.Data.TotalItems;
                $scope.PermissionsListOriginal = $scope.PermissionsList;
            }
            ShowMessages(response);
        });
    };
    $scope.GetPermissionList()

    $scope.Refresh = function () {
        $scope.GetPermissionList()
    };

    $scope.ResetSearchFilter = function () {
        $scope.PermissionsList = $scope.PermissionListOriginal;
        $scope.$apply(PermissionsList)
    };

    $scope.DeletePermission = function () {
        var perID = $(event.target).attr("data-ID");
        perID = perID ? parseInt(perID) : 0;

        var jsonData = { "PermissionID": perID };
        bootboxDialog(function (result) {
            if (result) {
                //var jsonData = $scope.SetPostData($scope.ReleaseNoteListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeletePermissionURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.GetPermissionList();
                    }
                });
            }
        }, bootboxDialogType.Confirm, "Permission", window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.SearchPermnmissionList = function () {
        $scope.PermissionsList = $scope.PermissionsListOriginal;
        var pName = $("#SearchPermissionName").val();
        var description = $("#SearchDescription").val();
        var pCode = $("#SearchPermissionCode").val();
        var pPlatform = $("#SearchPlatform").val();
        if (pName != "") {
            $scope.PermissionsList = $scope.PermissionsList.filter(m => m.PermissionName.toLowerCase().includes(pName.toLowerCase()));
        }
        if (description != "") {
            $scope.PermissionsList = $scope.PermissionsList.filter(m => m.Description.toLowerCase().includes(description.toLowerCase()));
        }
        if (pCode != "") {
            $scope.PermissionsList = $scope.PermissionsList.filter(m => m.PermissionCode.toLowerCase().includes(pCode.toLowerCase()));
        }
        if (pPlatform != "") {
            $scope.PermissionsList = $scope.PermissionsList.filter(m => m.PermissionPlatform==pPlatform);
        }
        //$scope.PermissionsList.getDataCallback();
        $scope.$apply(PermissionsList)
    }
};
controllers.PermissionsListController.$inject = ['$scope', '$http', '$window', '$timeout'];
