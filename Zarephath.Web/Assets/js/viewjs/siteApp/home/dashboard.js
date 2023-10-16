var vm;
controllers.DashboardController = function ($scope, $http) {
    vm = $scope;
    $scope.DashboardModel = $.parseJSON($("#hdnSetDashboardListPage").val());
    $scope.ReferralURL = SiteUrl.AddReferralPageUrl;


    if ($.parseJSON($("#hdnP_AssignedIMNotifications").val())) {
        //#region Referral Internal Message List

        $scope.ReferralInternalMessageListPager = new PagerModule("ClientName", "#ReferralInternalMessageList");
        if ($scope.DashboardModel.ReferralInternalMessageModel.Items!=null && $scope.DashboardModel.ReferralInternalMessageModel.Items.length > 0) {
            $scope.ReferralInternalMessageList = $scope.DashboardModel.ReferralInternalMessageModel.Items;
            $scope.ReferralInternalMessageListPager.currentPageSize = $scope.DashboardModel.ReferralInternalMessageModel.Items.length;
            $scope.ReferralInternalMessageListPager.totalRecords = $scope.DashboardModel.ReferralInternalMessageModel.TotalItems;
        }
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
            AngularAjaxCall($http, SiteUrl.ReferralInternalMessageList, jsonData, "Post", "json", "application/json", false).success(function (response) {
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
            bootboxDialog(function (result) {
                if (result) {
                    var jsonData = angular.toJson({ EncryptedReferralInternalMessageID: encryptedReferralInternalMessageId, ReferralID: referralId });
                    AngularAjaxCall($http, SiteUrl.ResolveReferralInternalMessageURL, jsonData, "Post", "json", "application/json").success(function (response) {
                        ShowMessages(response);
                        if (response.IsSuccess) {
                            $scope.ReferralSparformCheckListPager.getDataCallback = $scope.GetReferralInternamMessageList;
                            $scope.ReferralSparformCheckListPager.getDataCallback(true);
                            $scope.ReferralSparformCheckListPager.getDataCallback = $scope.GetReferralSparformList;
                            $scope.ReferralSparformCheckListPager.getDataCallback(true);
                            $scope.ReferralMissingandExpireDocumentListPager.getDataCallback = $scope.GetReferralMissingandExpireDocumentList;
                            $scope.ReferralSparformCheckListPager.getDataCallback(true);
                        }
                    });
                }
            }, bootboxDialogType.Confirm, bootboxDialogTitle.ResolveNote, window.ResolveNoteConfirmMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnPrimary);
        };


        //#endregion

        //#endregion
    }
    if ($.parseJSON($("#hdnP_ResolvedIMNotifications").val())) {
        //#region Referral Resolved Internal Message List
        $scope.ReferralResolvedInternalMessageListPager = new PagerModule("ClientName", "#ReferralResolvedInternalMessageList");
        if ($scope.DashboardModel.ReferralResolvedInternalMessageListModel.Items != null && $scope.DashboardModel.ReferralResolvedInternalMessageListModel.Items.length > 0) {

            $scope.ReferralResolvedInternalMessageList = $scope.DashboardModel.ReferralResolvedInternalMessageListModel.Items;
            $scope.ReferralResolvedInternalMessageListPager.currentPageSize = $scope.DashboardModel.ReferralResolvedInternalMessageListModel.Items.length;
            $scope.ReferralResolvedInternalMessageListPager.totalRecords = $scope.DashboardModel.ReferralResolvedInternalMessageListModel.TotalItems;
        }
        $scope.GetReferralResolvedInternamMessageList = function (isSearchFilter) {
            $scope.ReferralResolvedInternalMessageListPager.currentPage = isSearchFilter ? 1 : $scope.ReferralInternalMessageListPager.currentPage;
            var pagermodel = {
                pageSize: $scope.ReferralResolvedInternalMessageListPager.pageSize,
                pageIndex: $scope.ReferralResolvedInternalMessageListPager.currentPage,
                sortIndex: $scope.ReferralResolvedInternalMessageListPager.sortIndex,
                sortDirection: $scope.ReferralResolvedInternalMessageListPager.sortDirection
            };
            var jsonData = angular.toJson(pagermodel);
            $scope.ReferralResolvedInternalMessageAjaxStart = true;
            AngularAjaxCall($http, SiteUrl.GetReferralResolvedInternalMessageURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
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
            AngularAjaxCall($http, SiteUrl.MarkResolvedMessageAsReadURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.ReferralResolvedInternalMessageList = response.Data.ReferralResolvedInternalMessageListModel.Items;
                    $scope.ReferralResolvedInternalMessageListPager.currentPageSize = response.Data.ReferralResolvedInternalMessageListModel.Items.length;
                    $scope.ReferralResolvedInternalMessageListPager.totalRecords = response.Data.ReferralResolvedInternalMessageListModel.TotalItems;
                }
            });
        };


        //#endregion
    }
    if ($.parseJSON($("#hdnP_MissingIncompleteCheckListSPAR").val())) {
        //#region Referral Spar form Check List

        $scope.ReferralSparformCheckListPager = new PagerModule("ClientName", "#ReferralSparformCheckList");

        if ($scope.DashboardModel.ReferralSparFormChekListModel.Items != null && $scope.DashboardModel.ReferralSparFormChekListModel.Items.length > 0) {
            $scope.ReferralSparformCheckList = $scope.DashboardModel.ReferralSparFormChekListModel.Items;
            $scope.ReferralSparformCheckListPager.currentPageSize = $scope.DashboardModel.ReferralSparFormChekListModel.Items.length;
            $scope.ReferralSparformCheckListPager.totalRecords = $scope.DashboardModel.ReferralSparFormChekListModel.TotalItems;
        }

        $scope.GetReferralSparformList = function (isSearchFilter) {
            $scope.ReferralSparformCheckListPager.currentPage = isSearchFilter ? 1 : $scope.ReferralSparformCheckListPager.currentPage;
            var pagermodel = {
                pageSize: $scope.ReferralSparformCheckListPager.pageSize,
                pageIndex: $scope.ReferralSparformCheckListPager.currentPage,
                sortIndex: $scope.ReferralSparformCheckListPager.sortIndex,
                sortDirection: $scope.ReferralSparformCheckListPager.sortDirection
            };
            var jsonData = angular.toJson(pagermodel);
            $scope.ReferralSparformCheckListAjaxStart = true;
            AngularAjaxCall($http, SiteUrl.GetReferralSparFormandCheckList, jsonData, "Post", "json", "application/json", false).success(function (response) {
                $scope.ReferralSparformCheckList = response.Data.ReferralSparFormChekListModel.Items;
                $scope.ReferralSparformCheckListPager.currentPageSize = response.Data.ReferralSparFormChekListModel.Items.length;
                $scope.ReferralSparformCheckListPager.totalRecords = response.Data.ReferralSparFormChekListModel.TotalItems;
                $scope.ReferralSparformCheckListAjaxStart = false;
            });
        };

        $scope.ReferralSparformCheckListPager.getDataCallback = $scope.GetReferralSparformList;
        $scope.ReferralSparformCheckListPager.getDataCallback(true);

        //#endregion
    }
    if ($.parseJSON($("#hdnP_MissingExpiredDocuments").val())) {
        //#region Exipre and Missing Document

        $scope.ReferralMissingandExpireDocumentListPager = new PagerModule("ClientName", "#ReferralMissingandExpireDocumentList");
        if ($scope.DashboardModel.ReferralMissingandExpireDocumentListModel.Items != null && $scope.DashboardModel.ReferralMissingandExpireDocumentListModel.Items.length > 0) {
            $scope.ReferralMissingandExpireDocumentList = $scope.DashboardModel.ReferralMissingandExpireDocumentListModel.Items;
            $scope.ReferralMissingandExpireDocumentListPager.currentPageSize = $scope.DashboardModel.ReferralMissingandExpireDocumentListModel.Items.length;
            $scope.ReferralMissingandExpireDocumentListPager.totalRecords = $scope.DashboardModel.ReferralMissingandExpireDocumentListModel.TotalItems;
        }
        $scope.GetReferralMissingandExpireDocumentList = function (isSearchFilter) {

            $scope.ReferralMissingandExpireDocumentListPager.currentPage = isSearchFilter ? 1 : $scope.ReferralMissingandExpireDocumentListPager.currentPage;
            var pagermodel = {
                pageSize: $scope.ReferralMissingandExpireDocumentListPager.pageSize,
                pageIndex: $scope.ReferralMissingandExpireDocumentListPager.currentPage,
                sortIndex: $scope.ReferralMissingandExpireDocumentListPager.sortIndex,
                sortDirection: $scope.ReferralMissingandExpireDocumentListPager.sortDirection
            };
            var jsonData = angular.toJson(pagermodel);
            $scope.ReferralMissingandExpireDocumentAjaxStart = true;
            AngularAjaxCall($http, SiteUrl.GetReferralMissingDocumentList, jsonData, "Post", "json", "application/json", false).success(function (response) {
                $scope.ReferralMissingandExpireDocumentList = response.Data.ReferralMissingandExpireDocumentListModel.Items;
                $scope.ReferralMissingandExpireDocumentListPager.currentPageSize = response.Data.ReferralMissingandExpireDocumentListModel.Items.length;
                $scope.ReferralMissingandExpireDocumentListPager.totalRecords = response.Data.ReferralMissingandExpireDocumentListModel.TotalItems;
                $scope.ReferralMissingandExpireDocumentAjaxStart = false;
                $scope.ShowCollpase();
            });
        };
        $scope.ReferralMissingandExpireDocumentListPager.getDataCallback = $scope.GetReferralMissingandExpireDocumentList;
        $scope.ReferralMissingandExpireDocumentListPager.getDataCallback(true);

        //Get missing Document Part
        $scope.GetMissingDocumentList = function (item, el) {
            if (item.MissingExpiredDocDetails == undefined) {
                var jsonData = angular.toJson({ ReferralID: item.ReferralID });
                $scope.ReferralMissingandExpireDocumentAjaxStart = true;
                AngularAjaxCall($http, SiteUrl.GetReferralMissingDocument, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    if (response.IsSuccess) {
                        item.MissingExpiredDocDetails = response.Data;

                        //collapseIcon(el, true);
                    } else
                        ShowMessage(response);
                    $scope.ReferralMissingandExpireDocumentAjaxStart = false;
                });
            }
            //else {
            //    collapseIcon(el);
            //}
        };

        function collapseIcon(el, firstTime) {
            //if ($(el).hasClass("collapsed") || firstTime) {
            //    $(el).removeClass("fa-plus-circle").addClass("fa-minus-circle");
            //}
            //else {
            //    $(el).addClass("fa-plus-circle").removeClass("fa-minus-circle");
            //}
        }


        //$('.collapseDiv').on('shown.bs.collapse', function () {
        //    $(".glyphicon").removeClass("glyphicon-folder-close").addClass("glyphicon-folder-open");
        //});

        //$('#collapseDiv').on('hidden.bs.collapse', function () {
        //    $(".glyphicon").removeClass("glyphicon-folder-open").addClass("glyphicon-folder-close");
        //});

        //#endregion
    }
    if ($.parseJSON($("#hdnP_MissingInternalDocument").val())) {
        //#region Internal Missing Document

        $scope.ReferralInternalMissingandExpireDocumentListPager = new PagerModule("ClientName", "#ReferralInternalMissingandExpireDocumentList");
        if ($scope.DashboardModel.ReferralInternalMissingandExpireDocumentListModel.Items != null && $scope.DashboardModel.ReferralInternalMissingandExpireDocumentListModel.Items.length > 0) {
            $scope.ReferralInternalMissingandExpireDocumentList = $scope.DashboardModel.ReferralInternalMissingandExpireDocumentListModel.Items;
            $scope.ReferralInternalMissingandExpireDocumentListPager.currentPageSize = $scope.DashboardModel.ReferralInternalMissingandExpireDocumentListModel.Items.length;
            $scope.ReferralInternalMissingandExpireDocumentListPager.totalRecords = $scope.DashboardModel.ReferralInternalMissingandExpireDocumentListModel.TotalItems;
        }
        $scope.GetReferralInternalMissingandExpireDocumentList = function (isSearchFilter) {

            $scope.ReferralInternalMissingandExpireDocumentListPager.currentPage = isSearchFilter ? 1 : $scope.ReferralInternalMissingandExpireDocumentListPager.currentPage;
            var pagermodel = {
                pageSize: $scope.ReferralInternalMissingandExpireDocumentListPager.pageSize,
                pageIndex: $scope.ReferralInternalMissingandExpireDocumentListPager.currentPage,
                sortIndex: $scope.ReferralInternalMissingandExpireDocumentListPager.sortIndex,
                sortDirection: $scope.ReferralInternalMissingandExpireDocumentListPager.sortDirection
            };
            var jsonData = angular.toJson(pagermodel);
            $scope.ReferralInternalMissingandExpireDocumentAjaxStart = true;
            AngularAjaxCall($http, SiteUrl.GetReferralInternalMissingDocumentList, jsonData, "Post", "json", "application/json", false).success(function (response) {
                $scope.ReferralInternalMissingandExpireDocumentList = response.Data.ReferralInternalMissingandExpireDocumentListModel.Items;
                $scope.ReferralInternalMissingandExpireDocumentListPager.currentPageSize = response.Data.ReferralInternalMissingandExpireDocumentListModel.Items.length;
                $scope.ReferralInternalMissingandExpireDocumentListPager.totalRecords = response.Data.ReferralInternalMissingandExpireDocumentListModel.TotalItems;
                $scope.ReferralInternalMissingandExpireDocumentAjaxStart = false;
                $scope.ShowCollpase();
            });
        };
        $scope.ReferralInternalMissingandExpireDocumentListPager.getDataCallback = $scope.GetReferralInternalMissingandExpireDocumentList;
        $scope.ReferralInternalMissingandExpireDocumentListPager.getDataCallback(true);

        //Get Internal Missing Document Part
        $scope.GetInternalMissingDocumentList = function (item, el) {
            if (item.MissingExpiredDocDetails == undefined) {
                var jsonData = angular.toJson({ ReferralID: item.ReferralID });
                $scope.ReferralInternalMissingandExpireDocumentAjaxStart = true;
                AngularAjaxCall($http, SiteUrl.GetReferralInternalMissingDocument, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    if (response.IsSuccess) {
                        item.MissingExpiredDocDetails = response.Data;

                        //collapseIcon(el, true);
                    } else
                        ShowMessage(response);
                    $scope.ReferralInternalMissingandExpireDocumentAjaxStart = false;
                });
            }
            //else {
            //    collapseIcon(el);
            //}
        };

        //#endregion
    }

    if ($.parseJSON($("#hdnP_AnsellCaseyReview").val())) {
        //#region Referral Ansell Case yReview

        $scope.ReferralAnsellCaseyReviewListPager = new PagerModule("ClientName", "#ReferralAnsellCaseyReviewList");
        if ($scope.DashboardModel.ReferralAnsellCaseyReviewListModel.Items != null && $scope.DashboardModel.ReferralAnsellCaseyReviewListModel.Items.length > 0) {

            $scope.ReferralAnsellCaseyReviewList = $scope.DashboardModel.ReferralAnsellCaseyReviewListModel.Items;
            $scope.ReferralAnsellCaseyReviewListPager.currentPageSize = $scope.DashboardModel.ReferralAnsellCaseyReviewListModel.Items.length;
            $scope.ReferralAnsellCaseyReviewListPager.totalRecords = $scope.DashboardModel.ReferralAnsellCaseyReviewListModel.TotalItems;
        }
        $scope.GetReferralAnsellCaseyReviewList = function (isSearchFilter) {
            $scope.ReferralAnsellCaseyReviewListPager.currentPage = isSearchFilter ? 1 : $scope.ReferralAnsellCaseyReviewListPager.currentPage;
            var pagermodel = {
                pageSize: $scope.ReferralAnsellCaseyReviewListPager.pageSize,
                pageIndex: $scope.ReferralAnsellCaseyReviewListPager.currentPage,
                sortIndex: $scope.ReferralAnsellCaseyReviewListPager.sortIndex,
                sortDirection: $scope.ReferralAnsellCaseyReviewListPager.sortDirection
            };
            var jsonData = angular.toJson(pagermodel);
            $scope.ReferralAnsellCaseyReviewAjaxStart = true;
            AngularAjaxCall($http, SiteUrl.GetReferralAnsellCaseyReviewURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                $scope.ReferralAnsellCaseyReviewList = response.Data.ReferralAnsellCaseyReviewListModel.Items;
                $scope.ReferralAnsellCaseyReviewListPager.currentPageSize = response.Data.ReferralAnsellCaseyReviewListModel.Items.length;
                $scope.ReferralAnsellCaseyReviewListPager.totalRecords = response.Data.ReferralAnsellCaseyReviewListModel.TotalItems;
                $scope.ReferralAnsellCaseyReviewAjaxStart = false;
            });
        };
        $scope.ReferralAnsellCaseyReviewListPager.getDataCallback = $scope.GetReferralAnsellCaseyReviewList;
        $scope.ReferralAnsellCaseyReviewListPager.getDataCallback(true);


        //#endregion
    }




    if ($.parseJSON($("#hdnP_AssignedNotesReview").val())) {
        //#region Referral Ansell Case yReview

        $scope.ReferralAssignedNotesReviewListPager = new PagerModule("ClientName", "#ReferralAssignedNotesReviewList");
        if ($scope.DashboardModel.ReferralAssignedNotesReviewListModel.Items != null && $scope.DashboardModel.ReferralAssignedNotesReviewListModel.Items.length > 0) {

            $scope.ReferralAssignedNotesReviewList = $scope.DashboardModel.ReferralAssignedNotesReviewListModel.Items;
            $scope.ReferralAssignedNotesReviewListPager.currentPageSize = $scope.DashboardModel.ReferralAssignedNotesReviewListModel.Items.length;
            $scope.ReferralAssignedNotesReviewListPager.totalRecords = $scope.DashboardModel.ReferralAssignedNotesReviewListModel.TotalItems;
        }
        $scope.GetReferralAssignedNotesReviewList = function (isSearchFilter) {
            $scope.ReferralAssignedNotesReviewListPager.currentPage = isSearchFilter ? 1 : $scope.ReferralAssignedNotesReviewListPager.currentPage;
            var pagermodel = {
                pageSize: $scope.ReferralAssignedNotesReviewListPager.pageSize,
                pageIndex: $scope.ReferralAssignedNotesReviewListPager.currentPage,
                sortIndex: $scope.ReferralAssignedNotesReviewListPager.sortIndex,
                sortDirection: $scope.ReferralAssignedNotesReviewListPager.sortDirection
            };
            var jsonData = angular.toJson(pagermodel);
            $scope.ReferralAssignedNotesReviewAjaxStart = true;
            AngularAjaxCall($http, SiteUrl.GetReferralAssignedNotesReviewURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                $scope.ReferralAssignedNotesReviewList = response.Data.ReferralAssignedNotesReviewListModel.Items;
                $scope.ReferralAssignedNotesReviewListPager.currentPageSize = response.Data.ReferralAssignedNotesReviewListModel.Items.length;
                $scope.ReferralAssignedNotesReviewListPager.totalRecords = response.Data.ReferralAssignedNotesReviewListModel.TotalItems;
                $scope.ReferralAssignedNotesReviewAjaxStart = false;
            });
        };
        $scope.ReferralAssignedNotesReviewListPager.getDataCallback = $scope.GetReferralAssignedNotesReviewList;
        $scope.ReferralAssignedNotesReviewListPager.getDataCallback(true);


        //#endregion
    }




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

controllers.DashboardController.$inject = ['$scope', '$http'];
