var vm;
controllers.SentReceivedMessageController = function ($scope, $http) {
    vm = $scope;


    $scope.newInstance = function () {
        return $.parseJSON($("#hdn_DashboardModel").val());
    };

    //#region Referral Internal Message List

    $scope.ReferralInternalMessageListPager = new PagerModule("ClientName", "#ReferralInternalMessageList");
    $scope.ReferralInternalMessageListPager.pageSize = 10;
    $scope.GetReferralInternamMessageList = function (isSearchFilter) {
        $scope.ReferralInternalMessageListPager.currentPage = isSearchFilter ? 1 : $scope.ReferralInternalMessageListPager.currentPage;
        var pagermodel = {
            pageSize: $scope.ReferralInternalMessageListPager.pageSize,
            pageIndex: $scope.ReferralInternalMessageListPager.currentPage,
            sortIndex: $scope.ReferralInternalMessageListPager.sortIndex,
            sortDirection: $scope.ReferralInternalMessageListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.ReferralInternalMessageAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.ReferralInternalMessageList, jsonData, "Post", "json", "application/json", false).success(function (response) {
            $scope.ReferralInternalMessageList = response.Data.ReferralInternalMessageModel.Items;
            $scope.ReferralInternalMessageListPager.currentPageSize = response.Data.ReferralInternalMessageModel.Items.length;
            $scope.ReferralInternalMessageListPager.totalRecords = response.Data.ReferralInternalMessageModel.TotalItems;
            $scope.ReferralInternalMessageAjaxStart = false;

           

        });
    };
    $scope.ReferralInternalMessageListPager.getDataCallback = $scope.GetReferralInternamMessageList;
    $scope.ReferralInternalMessageListPager.getDataCallback(true);

    //#region IsResolve 
    $scope.ResolveNote = function (encryptedReferralInternalMessageId, referralId) {
        
        bootboxDialog(function (result, data) {
            
            if (result!=null) {
                var jsonData = angular.toJson({ EncryptedReferralInternalMessageID: encryptedReferralInternalMessageId, ReferralID: referralId, ResolvedComment: result });
                AngularAjaxCall($http, HomeCareSiteUrl.ResolveReferralInternalMessageURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.ReferralInternalMessageListPager.getDataCallback(false);
                        $scope.ReferralResolvedInternalMessageListPager.getDataCallback(false);
                    }
                });
            }
        }, bootboxDialogType.Prompt, window.Reply, "", bootboxDialogButtonText.Save, btnClass.BtnPrimary);
    };


    //#endregion

    //#endregion

    //#region Referral Resolved Internal Message List
    $scope.ReferralResolvedInternalMessageListPager = new PagerModule("ClientName", "#ReferralResolvedInternalMessageList");
    $scope.ReferralResolvedInternalMessageListPager.pageSize = 10;
    $scope.GetReferralResolvedInternamMessageList = function (isSearchFilter) {
        $scope.ReferralResolvedInternalMessageListPager.currentPage = isSearchFilter ? 1 : $scope.ReferralResolvedInternalMessageListPager.currentPage;
        var pagermodel = {
            pageSize: $scope.ReferralResolvedInternalMessageListPager.pageSize,
            pageIndex: $scope.ReferralResolvedInternalMessageListPager.currentPage,
            sortIndex: $scope.ReferralResolvedInternalMessageListPager.sortIndex,
            sortDirection: $scope.ReferralResolvedInternalMessageListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.ReferralResolvedInternalMessageAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralResolvedInternalMessageURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            $scope.ReferralResolvedInternalMessageList = response.Data.ReferralResolvedInternalMessageListModel.Items;
            $scope.ReferralResolvedInternalMessageListPager.currentPageSize = response.Data.ReferralResolvedInternalMessageListModel.Items.length;
            $scope.ReferralResolvedInternalMessageListPager.totalRecords = response.Data.ReferralResolvedInternalMessageListModel.TotalItems;
            $scope.ReferralResolvedInternalMessageAjaxStart = false;
        });
    };
    $scope.ReferralResolvedInternalMessageListPager.getDataCallback = $scope.GetReferralResolvedInternamMessageList;
    $scope.ReferralResolvedInternalMessageListPager.getDataCallback(true);


    $scope.MarkResolvedMessageAsRead = function (encryptedReferralInternalMessageId, referralId) {
        var jsonData = angular.toJson({
            EncryptedReferralInternalMessageID: encryptedReferralInternalMessageId,
            ReferralID: referralId,
            pageSize: $scope.ReferralResolvedInternalMessageListPager.pageSize,
            pageIndex: $scope.ReferralResolvedInternalMessageListPager.currentPage,
            sortIndex: $scope.ReferralResolvedInternalMessageListPager.sortIndex,
            sortDirection: $scope.ReferralResolvedInternalMessageListPager.sortDirection
        });
        AngularAjaxCall($http, HomeCareSiteUrl.MarkResolvedMessageAsReadURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.ReferralResolvedInternalMessageListPager.getDataCallback(false);
                //$scope.ReferralResolvedInternalMessageList = response.Data.ReferralResolvedInternalMessageListModel.Items;
                //$scope.ReferralResolvedInternalMessageListPager.currentPageSize = response.Data.ReferralResolvedInternalMessageListModel.Items.length;
                //$scope.ReferralResolvedInternalMessageListPager.totalRecords = response.Data.ReferralResolvedInternalMessageListModel.TotalItems;
            }
        });
    };

    
    $scope.ShowResolvedComment = function (message) {
        bootboxDialog(function (result, data) {
        }, bootboxDialogType.Alert, window.ResolvedComment, message);
    };

    //#endregion


    $scope.ViewMoreClick = function (ele, count) {
        if (parseInt(count) > 0) {
            $('html, body').animate({
                scrollTop: $(ele).offset().top - 50
            }, 500);
        }
    };

    $scope.ShowCollpase = function () {
        setTimeout(function () {
            $.each($('.collapseDestination'), function (index, data) {

                $(this).on('show.bs.collapse', function () {
                    $(this).parents("tbody").find(".collapseSource").removeClass("fa-plus-circle").addClass("fa-minus-circle");
                });

                $(this).on('hidden.bs.collapse', function () {
                    $(this).parents("tbody").find(".collapseSource").removeClass("fa-minus-circle").addClass("fa-plus-circle");
                });

            });

        }, 100);
    };
    $scope.ShowCollpase();

};

controllers.SentReceivedMessageController.$inject = ['$scope', '$http'];
