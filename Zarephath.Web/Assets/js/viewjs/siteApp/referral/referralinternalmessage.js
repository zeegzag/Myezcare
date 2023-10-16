var rimModel;

controllers.ReferralInternalMessageController = function ($scope, $http, $window, $timeout) {
    rimModel = $scope;
    
    $scope.EncryptedReferralID = window.EncryptedReferralID; //"iSNqtcWbe3gZEhtctmlPcA2";

    $scope.SetReferralInternalMessageModel = $.parseJSON($("#hdnSetReferralInternalMessageModel").val());
    $scope.EmployeeList = $scope.SetReferralInternalMessageModel.EmployeeList;
    $scope.ReferralInternalMessage = $scope.SetReferralInternalMessageModel.SetReferralInternalMessagePageLoad.ReferralInternalMessage;
    $scope.SearchReferralInternalMessage = $scope.SetReferralInternalMessageModel.SetReferralInternalMessagePageLoad.SearchReferralInternalMessage;
    $scope.IsEditNote = false;

    $scope.ReferralInternalMessageListPager = new PagerModule("ReferralInternalMessageID","","DESC");

    $scope.SetReferralInternamMessageDetail = function (isSearchFilter) {
        $scope.ReferralInternalMessageListPager.currentPage = isSearchFilter ? 1 : $scope.ReferralInternalMessageListPager.currentPage;
        var pagermodel = {
            EncryptedReferralID: $scope.EncryptedReferralID,
            SearchReferralInternalMessage: $scope.SearchReferralInternalMessage,
            pageSize: $scope.ReferralInternalMessageListPager.pageSize,
            pageIndex: $scope.ReferralInternalMessageListPager.currentPage,
            sortIndex: $scope.ReferralInternalMessageListPager.sortIndex,
            sortDirection: $scope.ReferralInternalMessageListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);

        AngularAjaxCall($http, SiteUrl.SetReferralInternalMessageListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            $scope.IsEditNote = false;
            $scope.ReferralInternalMessageList = response.Data.Items;
            $scope.ReferralInternalMessageListPager.currentPageSize = response.Data.Items.length;
            $scope.ReferralInternalMessageListPager.totalRecords = response.Data.TotalItems;
        });
    };

    $scope.LoggedInUserId = window.LoggedInUserId;
    $scope.ReferralInternalMessageListPager.getDataCallback = $scope.SetReferralInternamMessageDetail;

    $scope.SaveReferralInternalMessage = function () {
        $scope.ReferralInternalMessage.EncryptedReferralID = $scope.EncryptedReferralID;
        var tempData = angular.copy($scope.ReferralInternalMessage);
        //tempData.Note = $scope.ReferralInternalMessage.TempNote;

        var postData = {
            ReferralInternalMessage: tempData,
            EncryptedReferralID: $scope.EncryptedReferralID
        };
        var jsonData = angular.toJson(postData);
        if (CheckErrors("#frmNewReferralInternalMessage")) {
            AngularAjaxCall($http, SiteUrl.SaveReferralInternalMessageURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                $scope.IsEditNote = false;
                if (response.IsSuccess) {
                    $scope.ReferralInternalMessage.Note = $scope.ReferralInternalMessage.TempNote;
                    $("#AddReferralInternalMessageModal").modal('hide');
                    $scope.ReferralInternalMessageListPager.getDataCallback(true);
                    //$scope.SetReferralInternamMessageDetail(true);
                } else {
                    $scope.ReferralInternalMessage.Note = $scope.ReferralInternalMessage.TempNote;
                }
            });
        }
    };
    

    $scope.AssigneeFilter = function (value) {
        return function (item) {
            if (item.IsDeleted == 0 || item.EmployeeID == value) {
                return item;
            }
        };
    };

    $scope.EditNote = function (referralInternalMessage) {
        $scope.IsEditNote = true;
        $scope.ReferralInternalMessage = $.parseJSON(JSON.stringify(referralInternalMessage));
        //$scope.ReferralInternalMessage = referralInternalMessage;
        //$scope.ReferralInternalMessage.TempNote = $scope.ReferralInternalMessage.Note;
        //$timeout(function () {
        //    $("#AssignEmployee").select2("val", $scope.ReferralInternalMessage.Assignee.toString());
        //});
    };

    $scope.ResolveNote = function (referralInternalMessage) {
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = angular.toJson({ EncryptedReferralInternalMessageID: referralInternalMessage.EncryptedReferralInternalMessageID, ReferralID: referralInternalMessage.ReferralID });
                AngularAjaxCall($http, SiteUrl.ResolveReferralInternalMessageURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        referralInternalMessage.IsResolved = response.Data.IsResolved;
                        $timeout(function () {
                            $scope.$apply();
                        });
                    }
                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.ResolveNote, window.ResolveNoteConfirmMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnPrimary);
    };

    $scope.DeleteNote = function (referralInternalMessage) {
        bootboxDialog(function (result) {
            if (result) {
                if ($scope.ReferralInternalMessageListPager.currentPage !== 1 && $scope.ReferralInternalMessageListPager.length === $scope.ReferralInternalMessageListPager.currentPageSize)
                    $scope.ReferralInternalMessageListPager.currentPage = $scope.ReferralInternalMessageListPager.currentPage - 1;

                var pagermodel = {
                    EncryptedReferralInternalMessageID: referralInternalMessage.EncryptedReferralInternalMessageID,
                    ReferralID: referralInternalMessage.ReferralID,
                    pageSize: $scope.ReferralInternalMessageListPager.pageSize,
                    pageIndex: $scope.ReferralInternalMessageListPager.currentPage,
                    sortIndex: $scope.ReferralInternalMessageListPager.sortIndex,
                    sortDirection: $scope.ReferralInternalMessageListPager.sortDirection
                };
                var jsonData = angular.toJson(pagermodel);

                AngularAjaxCall($http, SiteUrl.DeleteReferralInternalMessageURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.ReferralInternalMessageList = response.Data.Items;
                        $scope.ReferralInternalMessageListPager.currentPageSize = response.Data.Items.length;
                        $scope.ReferralInternalMessageListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteNoteMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.ResetReferralInternamMessage = function () {
        $scope.IsEditNote = false;
        $scope.ReferralInternalMessage = $.parseJSON($("#hdnSetReferralInternalMessageModel").val()).SetReferralInternalMessagePageLoad.ReferralInternalMessage;
        HideErrors("#frmNewReferralInternalMessage");
        $scope.$apply();
    };

    $("#AddReferralInternalMessageModal").on('hidden.bs.modal', function () {
        $scope.ResetReferralInternamMessage();
    });

    $("a#internalMessage").on('shown.bs.tab', function (e) {
        $scope.SetReferralInternamMessageDetail();
    });
    
};
controllers.ReferralInternalMessageController.$inject = ['$scope', '$http', '$window', '$timeout'];



