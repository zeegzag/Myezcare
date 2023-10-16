var vm;

controllers.ReferralListController = function ($scope, $http, $window, $timeout) {
    vm = $scope;
    $scope.AddReferralURL = "/referral/addreferral";    //SiteUrl.AddReferralURL;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetReferralListModel").val());
    };
    $scope.SelectedReferralIds = [];
    $scope.ReferralList = [];
    $scope.SelectAllCheckbox = false;
    $scope.ReferralModel = $.parseJSON($("#hdnSetReferralListModel").val());
    $scope.SearchReferralModel = $scope.newInstance().SearchReferralListModel;

    $scope.TempSearchReferralModel = $scope.newInstance().SearchReferralListModel;
    //window.PageSize = 50;
    $scope.ReferralListPager = new PagerModule("ClientName");
    
    $scope.SetPostData = function (fromIndex, model) {
        var pagermodel = {
            SearchReferralModel: $scope.SearchReferralModel,
            pageSize: $scope.ReferralListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ReferralListPager.sortIndex,
            sortDirection: $scope.ReferralListPager.sortDirection
        };
        if (model != undefined) {
            pagermodel.referralStatusModel = model;
        }
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchReferralModel = $.parseJSON(angular.toJson($scope.TempSearchReferralModel));
        //$scope.SearchReferralModel.PayorID = $scope.TempSearchReferralModel.PayorID;
        //$scope.SearchReferralModel.ReferralStatusID = $scope.TempSearchReferralModel.ReferralStatusID;
        //$scope.SearchReferralModel.AssigneeID = $scope.TempSearchReferralModel.AssigneeID;
        //$scope.SearchReferralModel.ClientName = $scope.TempSearchReferralModel.ClientName;
        //$scope.SearchReferralModel.NotifyCaseManagerID = $scope.TempSearchReferralModel.NotifyCaseManagerID;
        //$scope.SearchReferralModel.ChecklistID = $scope.TempSearchReferralModel.ChecklistID;
        //$scope.SearchReferralModel.ClinicalReviewID = $scope.TempSearchReferralModel.ClinicalReviewID;
        //$scope.SearchReferralModel.CaseManagerID = $scope.TempSearchReferralModel.CaseManagerID;
        //$scope.SearchReferralModel.ServiceID = $scope.TempSearchReferralModel.ServiceID;
        //$scope.SearchReferralModel.AgencyID = $scope.TempSearchReferralModel.AgencyID;
        //$scope.SearchReferralModel.AgencyLocationID = $scope.TempSearchReferralModel.AgencyLocationID;
        //$scope.SearchReferralModel.ListOfIdsInCsv = $scope.TempSearchReferralModel.ListOfIdsInCsv;

    };


    $scope.ResetSearchFilter = function () {
        $scope.SearchReferralModel = $scope.newInstance().SearchReferralListModel;
        $scope.TempSearchReferralModel = $scope.newInstance().SearchReferralListModel;
        $scope.ResetDropdown();
        $scope.TempSearchReferralModel.IsDeleted = "0";
        $scope.ReferralListPager.currentPage = 1;
        $scope.ReferralListPager.getDataCallback();
    };

    $scope.ResetDropdown = function () {
        $scope.TempSearchReferralModel.NotifyCaseManagerID = "-1";
        $scope.TempSearchReferralModel.ServiceID = "-1";
        $scope.TempSearchReferralModel.ChecklistID = "-1";
        $scope.TempSearchReferralModel.ClinicalReviewID = "-1";
        $scope.TempSearchReferralModel.IsSaveAsDraft = "-1";
        //$("#NotifyCaseManagerID").val("-1");
        //$("#ServiceID").val("-1");
        //$("#ChecklistID").val("-1");
        //$("#ClinicalReviewID").val("-1");
    };

    $scope.GetReferralList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedReferralIds = [];
        $scope.SelectAllCheckbox = false;
        //Reset Selcted Checkbox items and Control

        
       
        var cmId = GetCookie('CM_ID');
        if (ValideElement(cmId)) {
            $scope.TempSearchReferralModel.CaseManagerID = cmId;
            $scope.SearchReferralModel.CaseManagerID = cmId;
            if (!$scope.$root.$$phase) {
                $scope.$apply();
            }
        }


        
        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        
        $scope.AjaxStart = true;
        var jsonData = $scope.SetPostData($scope.ReferralListPager.currentPage);

        AngularAjaxCall($http, SiteUrl.GetReferralListURL, jsonData, "Post", "json", "application/json",false).success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralList = response.Data.Items;
                $scope.ReferralListPager.currentPageSize = response.Data.Items.length;
                $scope.ReferralListPager.totalRecords = response.Data.TotalItems;
                $scope.ShowCollpase();
            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };

    $scope.DeleteReferral = function (referral, title) {
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
                    $scope.SearchReferralModel.ListOfIdsInCsv = $scope.SelectedReferralIds.toString();
                else
                    $scope.SearchReferralModel.ListOfIdsInCsv = referral.ReferralID > 0 ? referral.ReferralID.toString() : $scope.SelectedReferralIds.toString();

                if (referral != undefined && referral.ReferralID > 0) {
                    if ($scope.ReferralListPager.currentPage != 1)
                        $scope.ReferralListPager.currentPage = $scope.ReferralList.length === 1 ? $scope.ReferralListPager.currentPage - 1 : $scope.ReferralListPager.currentPage;
                } else {

                    if ($scope.ReferralListPager.currentPage != 1 && $scope.SelectedReferralIds.length == $scope.ReferralListPager.currentPageSize)
                        $scope.ReferralListPager.currentPage = $scope.ReferralListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedReferralIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control
                var jsonData = $scope.SetPostData($scope.ReferralListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteReferralURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {

                        $scope.ReferralList = response.Data.Items;
                        $scope.ReferralListPager.currentPageSize = response.Data.Items.length;
                        $scope.ReferralListPager.totalRecords = response.Data.TotalItems;
                        $scope.ShowCollpase();
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
        //}


    };

    $scope.EncryptedReferralID = "m6-gKuJrVRNLZL08U5CmHg2";


    $scope.Refresh = function () {
        $scope.ReferralListPager.getDataCallback();
    };

    $scope.SearchReferral = function () {
        $scope.ReferralListPager.currentPage = 1;
        $scope.ReferralListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectReferral = function (referral) {
        if (referral.IsChecked)
            $scope.SelectedReferralIds.push(referral.ReferralID);
        else
            $scope.SelectedReferralIds.remove(referral.ReferralID);

        if ($scope.SelectedReferralIds.length == $scope.ReferralListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedReferralIds = [];

        angular.forEach($scope.ReferralList, function (item, key) {

            item.IsChecked = $scope.SelectAllCheckbox;// event.target.checked;
            if (item.IsChecked)
                $scope.SelectedReferralIds.push(item.ReferralID);
        });

        return true;
    };

    $scope.ReferralListPager.getDataCallback = $scope.GetReferralList;
    $scope.ReferralListPager.getDataCallback();



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
          return   $scope.CoreSaveStatus(newStatus, referral);
        }
    };

    $scope.CoreSaveStatus = function (newStatus, referral, notifyCm) {
        var model = {
            ReferralID: referral.ReferralID,
            ReferralStatusID: newStatus,
            NotifyCmForInactiveStatus: notifyCm
        };

        return AngularAjaxCall($http, SiteUrl.ReferralStatusUpdateURL, { referralStatusModel: model }, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                referral.ReferralStatusID = response.Data.ReferralStatusID;
                referral.Status = response.Data.Status;
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
            $scope.SearchReferralModel.ListOfIdsInCsv = $scope.SelectedReferralIds.toString();
        else
            $scope.SearchReferralModel.ListOfIdsInCsv = referral.ReferralID > 0 ? referral.ReferralID.toString() : $scope.SelectedReferralIds.toString();

        if (referral != undefined && referral.ReferralID > 0) {
            if ($scope.ReferralListPager.currentPage != 1)
                $scope.ReferralListPager.currentPage = $scope.ReferralList.length === 1 ? $scope.ReferralListPager.currentPage - 1 : $scope.ReferralListPager.currentPage;
        } else {

            if ($scope.ReferralListPager.currentPage != 1 && $scope.SelectedReferralIds.length == $scope.ReferralListPager.currentPageSize)
                $scope.ReferralListPager.currentPage = $scope.ReferralListPager.currentPage - 1;
        }

        //Reset Selcted Checkbox items and Control
        $scope.SelectedReferralIds = [];
        $scope.SelectAllCheckbox = false;
        //Reset Selcted Checkbox items and Control
        var model = {
            ReferralID: referral.ReferralID,
            AssigneeID: newAssignee
        };
        var jsonData = $scope.SetPostData($scope.ReferralListPager.currentPage, model);
        return AngularAjaxCall($http, SiteUrl.UpdateAssigneeURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.ReferralList = response.Data.Items;
                $scope.ReferralListPager.currentPageSize = response.Data.Items.length;
                $scope.ReferralListPager.totalRecords = response.Data.TotalItems;
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
controllers.ReferralListController.$inject = ['$scope', '$http', '$window', '$timeout'];


$(document).ready(function () {
    var cmId = GetCookie('CM_ID');
    if (ValideElement(cmId))
        vm.TempSearchReferralModel.CaseManagerID = cmId;
});