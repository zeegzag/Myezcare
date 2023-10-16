var bmModel;

controllers.ReferralBlockEmployeeController = function ($scope, $http, $window, $timeout) {
    bmModel = $scope;
    $scope.EncryptedReferralID = window.EncryptedReferralID;



    //#region Referral Block Employee

    //window.PageSize = 1;
    $scope.newBlockEmpInstance = function () {
        return $.parseJSON($("#hdnSetBlockEmpPageModel").val());
    };

    if ($scope.newBlockEmpInstance()) {

        $scope.SearchRefBlockEmpListModel = $scope.newBlockEmpInstance().SearchRefBlockEmpListModel;
        $scope.TempSearchRefAuditLogListModel = $scope.newBlockEmpInstance().SearchRefBlockEmpListModel;
        $scope.ReferralBlockedEmployee = $scope.newBlockEmpInstance().ReferralBlockedEmployee;
    }

    $scope.BlockEmpListPager = new PagerModule("ReferralBlockedEmployeeID", null, 'DESC');
    $scope.SetBlockEmpPostData = function (fromIndex) {
        var pagermodel = {
            searchModel: $scope.SearchRefBlockEmpListModel,
            pageSize: $scope.BlockEmpListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.BlockEmpListPager.sortIndex,
            sortDirection: $scope.BlockEmpListPager.sortDirection,
            sortIndexArray: $scope.BlockEmpListPager.sortIndexArray.toString()
        };

        return angular.toJson(pagermodel);
    };
    $scope.SearchBlockEmpMapping = function () {
        $scope.SearchRefBlockEmpListModel = $.parseJSON(angular.toJson($scope.TempSearchRefBlockEmpListModel));
    };
    $scope.ResetBlockEmpSearchFilter = function () {
        $scope.SearchRefBlockEmpListModel = $scope.newBlockEmpInstance().SearchRefBlockEmpListModel;
        $scope.TempSearchRefBlockEmpListModel = $scope.newBlockEmpInstance().SearchRefBlockEmpListModel;
        $scope.BlockEmpListPager.currentPage = 1;
        $scope.BlockEmpListPager.getDataCallback();
    };
    $scope.SearchBlockEmp = function () {
        $scope.BlockEmpListPager.currentPage = 1;
        $scope.BlockEmpListPager.getDataCallback(true);
    };


    $scope.BlockEmpList = [];
    $scope.SearchBlockEmpListPage = {};
    $scope.AjaxStartBL = true;
    $scope.GetBlockEmpList = function (isSearchDataMappingRequire) {
        
        if (isSearchDataMappingRequire)
            $scope.SearchBlockEmpMapping();

        $scope.SearchRefBlockEmpListModel.EncryptedReferralID = $scope.EncryptedReferralID;
        var jsonData = $scope.SetBlockEmpPostData($scope.BlockEmpListPager.currentPage);
        $scope.AjaxStartBL = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetBlockEmpListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.BlockEmpList = response.Data.Items;
                $scope.BlockEmpListPager.currentPageSize = response.Data.Items.length;
                $scope.BlockEmpListPager.totalRecords = response.Data.TotalItems;

                if (!$scope.$root.$$phase) {
                    $scope.$apply();
                }

            }
            $scope.AjaxStartBL = false;
            ShowMessages(response);
        });

    };

    $scope.BlockEmpListPager.getDataCallback = $scope.GetBlockEmpList;


    $scope.NewBlockEmpView = function () {
        
        $scope.ReferralBlockedEmployee = $scope.newBlockEmpInstance().ReferralBlockedEmployee;
    };

    $scope.OpenBlockEmpEditModal = function (data) {
        $scope.ReferralBlockedEmployee = angular.copy(data);
        $("#NewBlockEmpView").modal("show");
    };

    $scope.SaveBlockEmp = function () {

        
        //$("#frmAddBlockEmpView").each(function () {
        //    var ele = $(this, "input, textarea, select");
        //    $(ele).removeClass("ignore-element");
        //});


        if (CheckErrors("#frmAddBlockEmpView")) {

            var jsonData = angular.toJson({
                model: $scope.ReferralBlockedEmployee,
                searchModel: $scope.SearchRefBlockEmpListModel,
                pageSize: $scope.BlockEmpListPager.pageSize,
                pageIndex: $scope.BlockEmpListPager.currentPage,
                sortIndex: $scope.BlockEmpListPager.sortIndex,
                sortDirection: $scope.BlockEmpListPager.sortDirection,
            });

            AngularAjaxCall($http, HomeCareSiteUrl.SaveBlockEmpURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $('#NewBlockEmpView').modal('hide');
                    $scope.BlockEmpList = response.Data.Items;
                    $scope.BlockEmpListPager.currentPageSize = response.Data.Items.length;
                    $scope.BlockEmpListPager.totalRecords = response.Data.TotalItems;
                }
                ShowMessages(response);
            });
        }
    };



    $scope.DeleteBlockEmp = function (item) {
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = angular.toJson({
                    model: item,
                    searchModel: $scope.SearchRefBlockEmpListModel,
                    pageSize: $scope.BlockEmpListPager.pageSize,
                    pageIndex: $scope.BlockEmpListPager.currentPage,
                    sortIndex: $scope.BlockEmpListPager.sortIndex,
                    sortDirection: $scope.BlockEmpListPager.sortDirection,
                });

                AngularAjaxCall($http, HomeCareSiteUrl.DeleteBlockEmpURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.BlockEmpList = response.Data.Items;
                        $scope.BlockEmpListPager.currentPageSize = response.Data.Items.length;
                        $scope.BlockEmpListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });

            }
        }, bootboxDialogType.Confirm, window.Delete, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);


    };


    $("a[data-toggle='tab']").on('shown.bs.tab', function (e) {
        if (e.delegateTarget.id == 'blockemployee') {
            $scope.GetBlockEmpList();
        } else
            $scope.ShowSubmitActions = true;

        if (!$scope.$root.$$phase) $scope.$apply();

    });



    //#endregion



};
controllers.ReferralBlockEmployeeController.$inject = ['$scope', '$http', '$window', '$timeout'];



