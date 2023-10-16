var vm;

controllers.ReferralTrackingListController = function ($scope, $http, $window, $timeout) {
    vm = $scope;
    $scope.EncryptedReferralID = "";
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetReferralTrackingListModel").val());
    };
    $scope.SelectedReferralIds = [];
    $scope.ReferralTrackingList = [];
    $scope.SelectAllCheckbox = false;
    $scope.ReferralModel = $.parseJSON($("#hdnSetReferralTrackingListModel").val());
    $scope.SearchReferralTrackingModel = $scope.newInstance().SearchReferralTrackingListModel;
    $scope.TempSearchReferralTrackingModel = $scope.newInstance().SearchReferralTrackingListModel;
    //window.PageSize = 50;
    $scope.ReferralTrackingListPager = new PagerModule("ClientName");
    
    $scope.SetPostData = function (fromIndex, model) {
        $scope.SearchReferralTrackingModel.CreatedDate = $scope.SearchReferralTrackingModel.CreatedDate ? moment(new Date($scope.SearchReferralTrackingModel.CreatedDate).toString()).format() : $scope.SearchReferralTrackingModel.CreatedDate;
        $scope.SearchReferralTrackingModel.ChecklistDate = $scope.SearchReferralTrackingModel.ChecklistDate ? moment(new Date($scope.SearchReferralTrackingModel.ChecklistDate).toString()).format() : $scope.SearchReferralTrackingModel.ChecklistDate;
        $scope.SearchReferralTrackingModel.SparDate = $scope.SearchReferralTrackingModel.SparDate ? moment(new Date($scope.SearchReferralTrackingModel.SparDate).toString()).format() : $scope.SearchReferralTrackingModel.SparDate;
        
        $scope.SearchReferralTrackingModel.CreatedToDate = $scope.SearchReferralTrackingModel.CreatedToDate ? moment(new Date($scope.SearchReferralTrackingModel.CreatedToDate).toString()).format() : $scope.SearchReferralTrackingModel.CreatedToDate;
        $scope.SearchReferralTrackingModel.ChecklistToDate = $scope.SearchReferralTrackingModel.ChecklistToDate ? moment(new Date($scope.SearchReferralTrackingModel.ChecklistToDate).toString()).format() : $scope.SearchReferralTrackingModel.ChecklistToDate;
        $scope.SearchReferralTrackingModel.SparToDate = $scope.SearchReferralTrackingModel.SparToDate ? moment(new Date($scope.SearchReferralTrackingModel.SparToDate).toString()).format() : $scope.SearchReferralTrackingModel.SparToDate;

        var pagermodel = {
            SearchReferralTrackingModel: $scope.SearchReferralTrackingModel,
            pageSize: $scope.ReferralTrackingListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ReferralTrackingListPager.sortIndex,
            sortDirection: $scope.ReferralTrackingListPager.sortDirection
        };
        if (model != undefined) {
            pagermodel.referralStatusModel = model;
        }
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchReferralTrackingModel = $.parseJSON(angular.toJson($scope.TempSearchReferralTrackingModel));
    };

    $scope.ResetSearchFilter = function () {
        $scope.SearchReferralTrackingModel = $scope.newInstance().SearchReferralTrackingListModel;
        $scope.TempSearchReferralTrackingModel = $scope.newInstance().SearchReferralTrackingListModel;
        $scope.ResetDropdown();
        $scope.TempSearchReferralTrackingModel.IsDeleted = "0";
        $scope.ReferralTrackingListPager.currentPage = 1;
        $scope.ReferralTrackingListPager.getDataCallback();
    };

    $scope.ResetDropdown = function () {
        $scope.TempSearchReferralTrackingModel.NotifyCaseManagerID = "-1";
        $scope.TempSearchReferralTrackingModel.ServiceID = "-1";
        $scope.TempSearchReferralTrackingModel.ChecklistID = "-1";
        $scope.TempSearchReferralTrackingModel.ClinicalReviewID = "-1";
        $scope.TempSearchReferralTrackingModel.IsSaveAsDraft = "-1";
        //$("#NotifyCaseManagerID").val("-1");
        //$("#ServiceID").val("-1");
        //$("#ChecklistID").val("-1");
        //$("#ClinicalReviewID").val("-1");
    };

    $scope.GetReferralTrackingList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedReferralIds = [];
        $scope.SelectAllCheckbox = false;
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.ReferralTrackingListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetReferralTrackingListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralTrackingList = response.Data.Items;
                $scope.ReferralTrackingListPager.currentPageSize = response.Data.Items.length;
                $scope.ReferralTrackingListPager.totalRecords = response.Data.TotalItems;
                $scope.ShowCollpase();
            }
            ShowMessages(response);
        });
    };

    $scope.DeleteReferralTracking = function (referral, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        //if (1 != 1) {
        //    bootboxDialog(function () {
        //    }, bootboxDialogType.Alert, window.Alert, window.ReferralDependencyExistMessage);
        //} else {
        bootboxDialog(function (result) {
            if (result) {
                if (referral == undefined)
                    $scope.SearchReferralTrackingModel.ListOfIdsInCsv = $scope.SelectedReferralIds.toString();
                else
                    $scope.SearchReferralTrackingModel.ListOfIdsInCsv = referral.ReferralID > 0 ? referral.ReferralID.toString() : $scope.SelectedReferralIds.toString();

                if (referral != undefined && referral.ReferralID > 0) {
                    if ($scope.ReferralTrackingListPager.currentPage != 1)
                        $scope.ReferralTrackingListPager.currentPage = $scope.ReferralTrackingList.length === 1 ? $scope.ReferralTrackingListPager.currentPage - 1 : $scope.ReferralTrackingListPager.currentPage;
                } else {

                    if ($scope.ReferralTrackingListPager.currentPage != 1 && $scope.SelectedReferralIds.length == $scope.ReferralTrackingListPager.currentPageSize)
                        $scope.ReferralTrackingListPager.currentPage = $scope.ReferralTrackingListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedReferralIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control
                var jsonData = $scope.SetPostData($scope.ReferralTrackingListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteReferralURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {

                        $scope.ReferralTrackingList = response.Data.Items;
                        $scope.ReferralTrackingListPager.currentPageSize = response.Data.Items.length;
                        $scope.ReferralTrackingListPager.totalRecords = response.Data.TotalItems;
                        $scope.ShowCollpase();
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
        //}


    };

    $scope.Refresh = function () {
        $scope.ReferralTrackingListPager.getDataCallback();
    };

    $scope.SearchReferralTracking = function () {
        $scope.ReferralTrackingListPager.currentPage = 1;
        $scope.ReferralTrackingListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectReferral = function (referral) {
        if (referral.IsChecked)
            $scope.SelectedReferralIds.push(referral.ReferralID);
        else
            $scope.SelectedReferralIds.remove(referral.ReferralID);

        if ($scope.SelectedReferralIds.length == $scope.ReferralTrackingListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedReferralIds = [];

        angular.forEach($scope.ReferralTrackingList, function (item, key) {

            item.IsChecked = $scope.SelectAllCheckbox;// event.target.checked;
            if (item.IsChecked)
                $scope.SelectedReferralIds.push(item.ReferralID);
        });

        return true;
    };

    $scope.ReferralTrackingListPager.getDataCallback = $scope.GetReferralTrackingList;
    $scope.ReferralTrackingListPager.getDataCallback();

    $scope.SendReceiptNotificationEmail = function (referral) {
        $scope.EncryptedReferralID = "iSNqtcWbe3gZEhtctmlPcA2"; //referral.$scope.EncryptedReferralID;
        var jsonData = angular.toJson({ EncryptedReferralID: referral.EncryptedReferralID });

        AngularAjaxCall($http, SiteUrl.SendReceiptNotificationEmailURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                referral.NotifyCaseManager = response.Data.NotifyCaseManager;
            }
        });
    };
    $scope.SaveStatus = function (newStatus, referral) {
        if (referral.LastReferralStatusID !== newStatus && newStatus === parseInt(window.Inactive)) {
            var result = confirm(window.SendNotificationToCM);
            return $scope.CoreSaveStatus(newStatus, referral, result);
        }
        else if (referral.LastReferralStatusID !== newStatus && newStatus === parseInt(window.ReferralAccepted)) {
            var result01 = confirm(window.SendNotificationToCMReferralAccepted);
            return $scope.CoreSaveStatus(newStatus, referral, result01);
        }
        else {
            return $scope.CoreSaveStatus(newStatus, referral);
        }
    };

    $scope.CoreSaveStatus = function (newStatus, referral, notifyCm) {
        var model = {
            ReferralID: referral.ReferralID,
            ReferralStatusID: newStatus,
            NotifyCmForInactiveStatus: notifyCm
        };

        return AngularAjaxCall($http, SiteUrl.ReferralTrackingStatusUpdateURL, { referralStatusModel: model }, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                referral.ReferralStatusID = response.Data.ReferralStatusID;
                referral.Status = response.Data.Status;
            }
        });
    };


    
    $scope.SaveComment = function (comment, referral) {        
        var model = {
            ReferralID: referral.ReferralID,
            Comment: comment
        };

        return AngularAjaxCall($http, SiteUrl.ReferralTrackingCommentUpdateURL, { referralCommentModel: model }, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {                
                referral.ReferralTrackingComment = comment;
            }
        });
    };

    $scope.AssigneeFilter = function (value) {
        return function (item) {
            if (item.IsDeleted == 0 || item.EmployeeID == value) {
                return item;
            }
        };
    };

    $scope.SaveAssignee = function (newAssignee, referral) {
        if (referral == undefined)
            $scope.SearchReferralTrackingModel.ListOfIdsInCsv = $scope.SelectedReferralIds.toString();
        else
            $scope.SearchReferralTrackingModel.ListOfIdsInCsv = referral.ReferralID > 0 ? referral.ReferralID.toString() : $scope.SelectedReferralIds.toString();

        if (referral != undefined && referral.ReferralID > 0) {
            if ($scope.ReferralTrackingListPager.currentPage != 1)
                $scope.ReferralTrackingListPager.currentPage = $scope.ReferralTrackingList.length === 1 ? $scope.ReferralTrackingListPager.currentPage - 1 : $scope.ReferralTrackingListPager.currentPage;
        } else {

            if ($scope.ReferralTrackingListPager.currentPage != 1 && $scope.SelectedReferralIds.length == $scope.ReferralTrackingListPager.currentPageSize)
                $scope.ReferralTrackingListPager.currentPage = $scope.ReferralTrackingListPager.currentPage - 1;
        }

        //Reset Selcted Checkbox items and Control
        $scope.SelectedReferralIds = [];
        $scope.SelectAllCheckbox = false;
        //Reset Selcted Checkbox items and Control
        var model = {
            ReferralID: referral.ReferralID,
            AssigneeID: newAssignee
        };
        var jsonData = $scope.SetPostData($scope.ReferralTrackingListPager.currentPage, model);
        return AngularAjaxCall($http, SiteUrl.UpdateAssigneeURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.ReferralTrackingList = response.Data.Items;
                $scope.ReferralTrackingListPager.currentPageSize = response.Data.Items.length;
                $scope.ReferralTrackingListPager.totalRecords = response.Data.TotalItems;
                $scope.ShowCollpase();
            }

        });
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
controllers.ReferralTrackingListController.$inject = ['$scope', '$http', '$window', '$timeout'];