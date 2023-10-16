var vm;
controllers.LSTMCaseloadListController = function ($scope, $http, $window, $timeout) {
    vm = $scope;
    $scope.EncryptedReferralID = "";
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetLSTeamMemberCaseloadPageModel").val());
    };
    $scope.SelectedReferralIds = [];
    $scope.LSTMCaseloadList = [];
    $scope.SelectAllCheckbox = false;

    $scope.ReferralModel = $.parseJSON($("#hdnSetLSTeamMemberCaseloadPageModel").val());

    $scope.SearchLSTMCaseloadModel = $scope.newInstance().SearchLSTMCaseloadListModel;

    $scope.TempSearchLSTMCaseloadModel = $scope.newInstance().SearchLSTMCaseloadListModel;

    $scope.LSTMCaseloadListPager = new PagerModule("ClientName");

    $scope.SetPostData = function (fromIndex, model) {
        var pagermodel = {
            SearchLSTMCaseloadModel: $scope.SearchLSTMCaseloadModel,
            pageSize: $scope.LSTMCaseloadListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.LSTMCaseloadListPager.sortIndex,
            sortDirection: $scope.LSTMCaseloadListPager.sortDirection
        };
        if (model != undefined) {
            pagermodel.referralStatusModel = model;
        }
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchLSTMCaseloadModel = $.parseJSON(angular.toJson($scope.TempSearchLSTMCaseloadModel));
    };

    $scope.ResetSearchFilter = function () {
        $scope.SearchLSTMCaseloadModel = $scope.newInstance().SearchLSTMCaseloadListModel;
        $scope.TempSearchLSTMCaseloadModel = $scope.newInstance().SearchLSTMCaseloadListModel;
        $scope.ResetDropdown();
        $scope.TempSearchLSTMCaseloadModel.IsDeleted = "0";
        $scope.LSTMCaseloadListPager.currentPage = 1;
        $scope.LSTMCaseloadListPager.getDataCallback();
    };

    $scope.ResetDropdown = function () {
        $scope.TempSearchLSTMCaseloadModel.NotifyCaseManagerID = "-1";
        $scope.TempSearchLSTMCaseloadModel.ServiceID = "-1";
        $scope.TempSearchLSTMCaseloadModel.ChecklistID = "-1";
        $scope.TempSearchLSTMCaseloadModel.ClinicalReviewID = "-1";
        $scope.TempSearchLSTMCaseloadModel.IsSaveAsDraft = "-1";
        //$("#NotifyCaseManagerID").val("-1");
        //$("#ServiceID").val("-1");
        //$("#ChecklistID").val("-1");
        //$("#ClinicalReviewID").val("-1");
    };

    $scope.GetLSTMCaseloadList = function (isSearchDataMappingRequire) {

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.LSTMCaseloadListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetLSTMCaseloadListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.LSTMCaseloadList = response.Data.Items;
                $scope.LSTMCaseloadListPager.currentPageSize = response.Data.Items.length;
                $scope.LSTMCaseloadListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.LSTMCaseloadListPager.getDataCallback();
    };

    $scope.SearchLSTMCaseload = function () {
        $scope.LSTMCaseloadListPager.currentPage = 1;
        $scope.LSTMCaseloadListPager.getDataCallback(true);
    };

    $scope.LSTMCaseloadListPager.getDataCallback = $scope.GetLSTMCaseloadList;
    $scope.LSTMCaseloadListPager.getDataCallback();

    $scope.GetDueDateClass = function (date) {

    };

    $scope.SaveComment = function (comment, referral) {
        var model = {
            ReferralID: referral.ReferralID,
            Comment: comment
        };
        return AngularAjaxCall($http, SiteUrl.SaveReferralLSTMCaseloadsCommentURL, { referralCommentModel: model }, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.LSTMCaseloadListPager.getDataCallback();
                //referral.ReferralLSTMCaseloadsComment = comment;
            }
        });
    };

};
controllers.LSTMCaseloadListController.$inject = ['$scope', '$http', '$window', '$timeout'];