controllers.ReferralCaseLoadController = function ($scope, $http, $window, $timeout) {
    //#region RCL Master Paging 
    var minRCLDate = new Date();
    minRCLDate.setDate(minRCLDate.getDate());
    minRCLDate.setHours(0, 0, 0, 0);
    $scope.NewDate = minRCLDate;
    $scope.ReferralCaseLoadList = [];
    $scope.ReferralCaseLoadListPager = new PagerModule("ReferralCaseLoadID", "", "DESC");

    $scope.EncryptedReferralID = window.EncryptedReferralID;
    $scope.ReferralID = $("#hdnRCLReferralID").val();
    $scope.SearchRCLMaster = {};
    $scope.SearchRCLMaster.ReferralID = $scope.ReferralID;
    $scope.SearchRCLMaster.EncryptedReferralID = $scope.EncryptedReferralID;
    $scope.TempSearchRCLMaster = {};
    $scope.TempSearchRCLMaster.ReferralID = $scope.ReferralID;
    $scope.TempSearchRCLMaster.EncryptedReferralID = $scope.EncryptedReferralID;


    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchRCLMaster: $scope.SearchRCLMaster,
            pageSize: $scope.ReferralCaseLoadListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ReferralCaseLoadListPager.sortIndex,
            sortDirection: $scope.ReferralCaseLoadListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };
    $scope.SearchModelMapping = function () {
        $scope.SearchRCLMaster = $.parseJSON(angular.toJson($scope.TempSearchRCLMaster));
    };


    $scope.GetRCLMasterList = function (isSearchDataMappingRequire) {
        //$scope.ResetRTSDetail();
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        var jsonData = $scope.SetPostData($scope.ReferralCaseLoadListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetRCLMasterListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralCaseLoadList = response.Data.Items;
                $scope.ReferralCaseLoadListPager.currentPageSize = response.Data.Items.length;
                $scope.ReferralCaseLoadListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.ReferralCaseLoadListPager.getDataCallback = $scope.GetRCLMasterList;
    $scope.ReferralCaseLoadListPager.getDataCallback();

    $scope.Refresh = function () {
        $scope.ReferralCaseLoadListPager.getDataCallback();
    };

    $('#rclMasterModel').on('hidden.bs.modal', function () {
        $scope.ReferralCaseLoadListPager.getDataCallback();
    });

    $('#rclTemporaryMasterModel').on('hidden.bs.modal', function () {
        if ($scope.IsTemporaryCaseLoadEditMode) {
            $scope.IsTemporaryCaseLoadEditMode = false;
            $scope.ReferralCaseLoadListPager.getDataCallback();
        }
    });

    $scope.SearchRCLMasterList = function () {
        $scope.ReferralCaseLoadListPager.currentPage = 1;
        $scope.ReferralCaseLoadListPager.getDataCallback(true);
    };

    $scope.RemoveCaseLoad = function (item) {
        bootboxDialog(function (result) {
            if (result) {
                item.ListOfIdsInCsv = item.ReferralCaseLoadID;
                $scope.SearchRCLMaster = item;
                var jsonData = $scope.SetPostData($scope.ReferralCaseLoadListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.RemoveCaseLoadURL, jsonData, "post", "json", "application/json", true).
                        success(function (response) {
                            if (response.IsSuccess) {
                                $scope.ReferralCaseLoadList = response.Data.Items;
                                $scope.ReferralCaseLoadListPager.currentPageSize = response.Data.Items.length;
                                $scope.ReferralCaseLoadListPager.totalRecords = response.Data.TotalItems;
                            }
                            ShowMessages(response);
                        });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, window.RemoveCaseLoadConfirmation, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    }

    $scope.RefreshRCLMaster = function () {
        $scope.ReferralCaseLoadListPager.getDataCallback();
    }
    //#endregion


    // #region Employee List

    $scope.SearchModel = {};
    $scope.Skills = {};
    $scope.Preferences = {};

    $scope.EmployeeTSList = {};
    $scope.EmpRefSkillList = [];
    $scope.EmpRefPreferenceList = [];

    $scope.GetEmpRefSchOptions = function (reloadSearch) {
        if (!ValideElement($scope.SearchModel.StartDate) || !ValideElement($scope.SearchModel.EndDate))
            return false;

        $scope.EmpRefSchAjaxStart = true;
        var jsonData = $scope.SetSearchPostData($scope.EmployeeTSListPager);
        AngularAjaxCall($http, HomeCareSiteUrl.GetRCLEmpRefSchOptionsURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.EmployeeTSList = response.Data.Page.Items;
                $scope.EmployeeTSListPager.currentPageSize = response.Data.Page.Items.length;
                $scope.EmployeeTSListPager.totalRecords = response.Data.Page.TotalItems;
            }
            $scope.EmpRefSchAjaxStart = false;
            ShowMessages(response);
        });
    }
    //$scope.EmployeeTSListPager.getDataCallback = $scope.GetEmpRefSchOptions;

    $scope.SetSearchPostData = function (pager) {
        $scope.SearchModel.StrSkillList = $scope.EmpRefSkillList ? $scope.EmpRefSkillList.toString() : "";
        $scope.SearchModel.StrPreferenceList = $scope.EmpRefPreferenceList ? $scope.EmpRefPreferenceList.toString() : "";

        var pagermodel = {
            model: $scope.SearchModel,
            pageSize: pager.pageSize,
            pageIndex: pager.currentPage,
            sortIndex: pager.sortIndex,
            sortDirection: pager.sortDirection,
            sortIndexArray: pager.sortIndexArray.toString()
        };

        pagermodel.model.StartDate = moment(pagermodel.model.StartDate).format("YYYY/MM/DD HH:mm:ss");
        pagermodel.model.EndDate = moment(pagermodel.model.EndDate).format("YYYY/MM/DD HH:mm:ss");

        return angular.toJson(pagermodel);
    };

    $scope.SearchEmpRefSchOptions = function () {
        if ($scope.EmployeeTSListPager) {
            $scope.EmployeeTSListPager.getDataCallback();
            //$scope.ResetSearchEmpRefSchOptions();
        }
    };

    $scope.ResetEmpRefSchOptions = function () {
        $scope.SearchModel.EmployeeName = '';
        $scope.SearchModel.MileRadius = '';
        $scope.EmpRefSkillList = [];
        $scope.EmpRefPreferenceList = [];
        $scope.EmployeeTSListPager.getDataCallback();
    };

    $scope.CallOnPopUpLoad = function (referralid, startDate) {
        $scope.SchedulesCreated = false;
        $scope.SearchModel = {};
        $scope.SearchModel.ReferralID = referralid;
        
        $scope.SearchModel.StartDate = moment(startDate);
        $scope.SearchModel.EndDate = moment(startDate).add(365, 'days').format();
        $scope.SearchModel.MaxEndDate = moment($scope.SearchModel.StartDate).add(365, 'days').format();

        $scope.EmployeeTSListPager = new PagerModule("EmployeeDayOffID");
        $scope.EmployeeTSListPager.pageSize = 10;
        $scope.EmployeeTSListPager.getDataCallback = $scope.GetEmpRefSchOptions;

        $timeout(function () {

            $scope.SkillsCount = 1;
            $scope.SkillsVal = "Skills DESC";

            $scope.PreferencesCount = 1;
            $scope.PreferencesVal = "Preferences DESC";

            $scope.MilesCount = 1;
            $scope.MilesVal = "Miles DESC";

            $scope.ConflictsCount = 1;
            $scope.ConflictsVal = "Conflicts DESC";

            $scope.EmployeeTSListPager.sortIndexArray = [$scope.ConflictsVal, $scope.PreferencesVal, $scope.MilesVal, $scope.SkillsVal];

            $scope.GetEmpRefSchOptions();
        }, 500);
    }

    $scope.GetPageLoadModels = function () {
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmpRefSchPageModelUrl, {}, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.Skills = response.Data.Skills;
                $scope.Preferences = response.Data.Preferences;
            }
            ShowMessages(response);
        });

    };
    $scope.GetPageLoadModels();

    // #endregion


    //#region Add
    $scope.OpenRCLMasterModal = function (caseLoadType) {
        $scope.RCLMaster = {};
        $scope.RCLMaster.ReferralID = $scope.TempSearchRCLMaster.ReferralID;
        $scope.RCLMaster.CaseLoadType = caseLoadType;
        $scope.CallOnPopUpLoad($scope.RCLMaster.ReferralID, new Date(), false);
        $('#rclMasterModel').modal('show');
    }

    $scope.SaveRCLMaster = function (item) {
        $scope.RCLMaster.EmployeeID = item.EmployeeID;
        var isValid = true;
        if (isValid) {
            bootboxDialog(function (result) {
                if (result) {
                    if ($scope.RCLMaster.CaseLoadType == $("#hdnPermanentCaseLoadString").val()) {
                        $scope.SaveRTSMain();
                    } else {
                        $scope.OpenTemporaryRCLMasterModal();
                    }
                }
            }, bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, window.AddCaseLoadConfirmation, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
        }
    };

    //$scope.$watchCollection('ReferralCaseLoadList', function (newVal, oldVal) {
    //    if (newVal != oldVal) {
    //        angular.forEach($scope.ReferralCaseLoadList, function (item, key) {
    //            item.CustomizedMinStartDate = item.StartDate;
    //        });
    //    }
    //});

    $scope.OpenTemporaryRCLMasterModal = function (item) {
        if (item != undefined) {
            $scope.IsTemporaryCaseLoadEditMode = true;
            if ($scope.RCLMaster == undefined) {
                $scope.RCLMaster = {};
            }
            $scope.RCLMaster.ReferralCaseLoadID = item.ReferralCaseLoadID;
            $scope.RCLMaster.ReferralID = item.ReferralID;
            $scope.RCLMaster.CaseLoadType = item.CaseLoadType;
            $scope.RCLMaster.AllowedToEditStartDate = item.AllowedToEditStartDate;
            $scope.RCLMaster.CustomizedMinStartDate = moment(item.CustomizedMinStartDate);
            $scope.RCLMaster.StartDate = moment(item.StartDate);
            $scope.RCLMaster.EndDate = moment(item.EndDate);
            $scope.RCLMaster.EmployeeID = item.EmployeeID;
        } else {
            $scope.RCLMaster.CustomizedMinStartDate = $scope.NewDate;
        }
        $('#rclTemporaryMasterModel').modal('show');
    }

    $scope.SaveTemporaryCaseLoad = function () {
        $scope.SaveRTSMain();
    }

    $scope.SaveRTSMain = function () {
        var isPermanent = $scope.RCLMaster.CaseLoadType == $("#hdnPermanentCaseLoadString").val();
        var isValid = true;
        if (!isPermanent) {
            isValid = CheckErrors($("#frmAddTempCaseLoad"));
        }
        if (isValid) {
            var jsonData = angular.toJson({
                rclMaster: $scope.RCLMaster
            });
            AngularAjaxCall($http, HomeCareSiteUrl.AddRCLMasterURL, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        if (!isPermanent) {
                            $('#rclTemporaryMasterModel').modal('hide');
                        }
                        $('#rclMasterModel').modal('hide');
                        $scope.RCLMaster.ReferralCaseLoadID = response.Data;
                    }
                    ShowMessages(response);
                });
        }
    };
    //#endregion
};