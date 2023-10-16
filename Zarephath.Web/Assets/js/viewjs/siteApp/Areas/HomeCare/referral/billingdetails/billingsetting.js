var rimModel1;

controllers.BillingSettingController = function ($scope, $http, $window, $timeout) {
    rimModel1 = $scope;

    var modalJson = $.parseJSON($("#hdnSetBillingSettingModel").val());
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetBillingSettingModel").val());
    };
    $scope.ReferralModel = modalJson;

    $scope.ReferralBillingSetting = $scope.ReferralModel.ReferralBillingSetting;
    $scope.ReferralBillingSetting.EncryptedReferralId = window.EncryptedReferralID;

    $scope.ReferralBillingAuthorization = $scope.ReferralModel.ReferralBillingAuthorization || {};
    $scope.ReferralBillingAuthorization.ReferralID = $scope.ReferralModel.Referral.ReferralID;
    $scope.GetReferralBillingSetting = function () {
        var jsonData = angular.toJson($scope.ReferralModel.ReferralBillingSetting);
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralBillingSettingURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.ReferralModel.ReferralBillingSetting = response.Data;
            }
        });
    }

    $scope.SaveReferralBillingSetting = function (isProfessionalAuthCode) {
        if (isProfessionalAuthCode)
            $scope.ReferralModel.ReferralBillingSetting.AuthrizationCodeType = 1;
        else
            $scope.ReferralModel.ReferralBillingSetting.AuthrizationCodeType = 2;

        var jsonData = angular.toJson($scope.ReferralModel.ReferralBillingSetting);

        AngularAjaxCall($http, HomeCareSiteUrl.AddReferralBillingSettingURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.ReferralModel.ReferralBillingSetting = response.Data;
            }
        });
    };

    $scope.SaveReferralBillingAuthorization = function (isProfessionalAuthCode) {
        $scope.AuthFormID = '#' + isProfessionalAuthCode;
        if (CheckErrors($scope.AuthFormID)) {
            //$scope.ReferralModel.ReferralBillingAuthorization;
            if ($scope.ReferralModel.ReferralBillingAuthorization.AllowedTimeType == null) {
                $scope.ReferralModel.ReferralBillingAuthorization.AllowedTimeType = "Minutes";
            }
            if ($scope.IsProfessionalAuthCode) {
                $scope.ReferralModel.ReferralBillingAuthorization.AuthType = 1;
                $scope.TempReferralBillingAuthorization.Type = 1;
            }
            else {
                $scope.ReferralModel.ReferralBillingAuthorization.AuthType = 2;
                $scope.TempReferralBillingAuthorization.Type = 2;
                //$scope.ReferralModel.ReferralBillingAuthorization.AuthorizationCode = $scope.ReferralModel.ReferralBillingAuthorization.AuthorizationCodeUB04;
                //$scope.ReferralModel.ReferralBillingAuthorization.StartDate = $scope.ReferralModel.ReferralBillingAuthorization.StartDateUB04;
                //$scope.ReferralModel.ReferralBillingAuthorization.EndDate = $scope.ReferralModel.ReferralBillingAuthorization.EndDateUB04;
            }

            if ($scope.ReferralModel.ReferralBillingAuthorization.ServiceCodeID.length > 0)
                $scope.ReferralModel.ReferralBillingAuthorization.StrServiceCodeIDs = $scope.ReferralModel.ReferralBillingAuthorization.ServiceCodeID.toString();
            //if (ValideElement($scope.ReferralModel.ReferralBillingSetting.POS_CMS1500) && rimModel1.ReferralModel.ReferralBillingSetting.POS_CMS1500 > 0) {
            if (ValideElement($scope.ReferralModel.ReferralBillingAuthorization.FacilityCode) && rimModel1.ReferralModel.ReferralBillingAuthorization.FacilityCode > 0) {
                if ($scope.ReferralModel.ReferralBillingAuthorization.PayorID != 0 && $scope.ReferralModel.ReferralBillingAuthorization.PayorID != null && $scope.ReferralModel.ReferralBillingAuthorization.StrServiceCodeIDs != 0) {
                    if ($scope.ReferralModel.ReferralBillingAuthorization.DxCode != null && $scope.ReferralModel.ReferralBillingAuthorization.DxCode != "" || $scope.PayorDetal.PayorInvoiceType == "2") {
                        if ($scope.ReferralModel.ReferralBillingAuthorization.PayorID > 0 && $scope.ReferralModel.ReferralBillingAuthorization.Rate > 0) {
                            var jsonData = angular.toJson({ PayorID: $scope.ReferralModel.ReferralBillingAuthorization.PayorID });
                            AngularAjaxCall($http, HomeCareSiteUrl.GetPayorIdentificationNumberURL, jsonData, "Post", "json", "application/json").success(function (response) {
                                ShowMessages(response);
                                if (response.IsSuccess) {
                                    $scope.PayorIdentificationNumber = response.Data.PayorIdentificationNumber;
                                    if ($scope.PayorIdentificationNumber == "VAMCD" && $scope.ReferralModel.ReferralBillingAuthorization.TaxonomyID == "0") {
                                        $("#referralBillingTaxonomyCode").removeClass("valid");
                                        $("#referralBillingTaxonomyCode").attr("data-original-title", "Taxonomy is required").attr("data-html", "true")
                                            .addClass("tooltip-danger input-validation-error")
                                            .tooltip({ html: true });
                                    }
                                    else {

                                        var jsonData = angular.toJson($scope.ReferralModel.ReferralBillingAuthorization);
                                        AngularAjaxCall($http, HomeCareSiteUrl.AddReferralBillingAuthorization, jsonData, "Post", "json", "application/json").success(function (response) {
                                            ShowMessages(response);
                                            if (response.IsSuccess) {
                                                //$scope.ReferralModel.ReferralBillingSetting = response.Data;
                                                $scope.ReferralBillingAuthorizationPager.getDataCallback();
                                                $scope.AuthorizationLinkup = {};
                                                $scope.AuthorizationLinkupList = [];
                                                $scope.AuthorizationLinkup.ReferralBillingAuthorizationID = response.Data;
                                                $scope.GetAuthorizationLinkupList();
                                                $scope.ClearDetails();
                                            }
                                        });
                                    }
                                }
                            });

                        }
                    }
                    else {
                        toastr.error('DxCode Missing');
                    }


                }
                else {
                    toastr.error('Reuired filed are missing');
                }
            }
            else {
                toastr.error('Facility Code Reuired');
            }
        }
    };

    $scope.AuthorizationLinkupList = [];
    $scope.GetAuthorizationLinkupList = function () {
        var jsonData = angular.toJson({ referralBillingAuthorizationID: $scope.AuthorizationLinkup.ReferralBillingAuthorizationID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetAuthorizationLinkupListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.AuthorizationLinkupList = response.Data;
                if ($scope.AuthorizationLinkupList.length > 0) {
                    $scope.ShowAuthorizationLinkupListModal();
                }
            }
        });
    };

    $scope.ShowAuthorizationLinkupListModal = function () {
        $('#authorizationLinkupListModal').modal({ backdrop: 'static', keyboard: false });
    };

    $scope.AuthorizationLinkupSelectedIDs = function () {
        if ($scope.AuthorizationLinkupList) {
            return $scope.AuthorizationLinkupList.filter(i => i.IsChecked).map(i => i.ScheduleIDs).join();
        }
        return false;
    };

    $scope.AuthorizationLinkupSelected = function () {
        $scope.UpdateAuthorizationLinkup($scope.AuthorizationLinkupSelectedIDs());
    };

    $scope.CloseAuthorizationLinkupListModal = function () {
        $("#authorizationLinkupListModal").modal('hide');
    };

    $scope.ScheduleTypesActiveTabId = function (fromIndex, model) {
        return $("ul#scheduleTypes li.active").first().attr('id');
    };

    $scope.ResetAuthorizationScheduleLinkList = function () {
        $scope.SearchAuthorizationScheduleLinkList();
        $scope.GetAuthorizationScheduleLinkList();
    };

    $('#scheduleTypes a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $scope.ResetAuthorizationScheduleLinkList();
    });

    $scope.SearchAuthorizationScheduleLinkListModel = {};
    $scope.SearchAuthorizationScheduleLinkList = function () {
        var actTabId = $scope.ScheduleTypesActiveTabId();
        var isFuture = actTabId == HashUrl_BillingSettings_AuthScheduleLink_Future;
        var isPast = actTabId == HashUrl_BillingSettings_AuthScheduleLink_Past;
        var date = new Date();
        if (isPast) { date.setDate(date.getDate() - 1) };

        $scope.SearchAuthorizationScheduleLinkListModel.StartDate = isFuture ? date : null;
        $scope.SearchAuthorizationScheduleLinkListModel.EndDate = isPast ? date : null;

        $scope.SearchAuthorizationScheduleLinkListModel.StartMinDate
            = $scope.SearchAuthorizationScheduleLinkListModel.EndMinDate = isFuture ? date : null;
        $scope.SearchAuthorizationScheduleLinkListModel.StartMaxDate
            = $scope.SearchAuthorizationScheduleLinkListModel.EndMaxDate = isPast ? new Date() : null;
        setTimeout(() => {
            $scope.SearchAuthorizationScheduleLinkListModel.StartMaxDate
                = $scope.SearchAuthorizationScheduleLinkListModel.EndMaxDate = isPast ? date : null;
        }, 500);

    };


    $scope.AuthorizationScheduleLinkList = [];
    $scope.GetAuthorizationScheduleLinkList = function () {
        $scope.AuthorizationScheduleLinkList = [];
        var jsonData = angular.toJson($scope.SearchAuthorizationScheduleLinkListModel);
        AngularAjaxCall($http, HomeCareSiteUrl.GetAuthorizationScheduleLinkListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.AuthorizationScheduleLinkList = response.Data;
            }
        });
    };

    $scope.ShowAuthorizationScheduleLinkListModal = function (data) {
        $scope.LinkAuthorization = data.AuthorizationCode;
        $scope.LinkServiceCode = data.ServiceCodeName;
        $scope.LinkModifier = data.ModifierID;
        $('#authorizationScheduleLinkListModal').modal({ backdrop: 'static', keyboard: false });
        $scope.SearchAuthorizationScheduleLinkListModel = {};
        $scope.SearchAuthorizationScheduleLinkListModel.ReferralBillingAuthorizationID = data.ReferralBillingAuthorizationID;
        $scope.SearchAuthorizationScheduleLinkList();
        $scope.GetAuthorizationScheduleLinkList();
    };

    $scope.CloseAuthorizationScheduleLinkListModal = function () {
        $("#authorizationScheduleLinkListModal").modal('hide');
    };

    $scope.UpdateAuthorizationLinkup = function (scheduleIDs, isScheduleLink) {
        var RBAId = isScheduleLink ? $scope.SearchAuthorizationScheduleLinkListModel.ReferralBillingAuthorizationID : $scope.AuthorizationLinkup.ReferralBillingAuthorizationID;
        var jsonData = angular.toJson({ ReferralBillingAuthorizationID: RBAId, ScheduleIDs: scheduleIDs });
        AngularAjaxCall($http, HomeCareSiteUrl.UpdateAuthorizationLinkupURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                isScheduleLink ? $scope.GetAuthorizationScheduleLinkList() : $scope.GetAuthorizationLinkupList();
            }
        });
    };

    $scope.ClearDetails = function () {
        //$scope.ReferralModel.ReferralBillingAuthorization.ReferralBillingAuthorizationID = null;
        //$scope.ReferralModel.ReferralBillingAuthorization.AuthorizationCode = null;
        //$scope.ReferralModel.ReferralBillingAuthorization.StartDate = null;
        //$scope.ReferralModel.ReferralBillingAuthorization.EndDate = null;
        $scope.ReferralModel.ReferralBillingAuthorization = $scope.newInstance().ReferralBillingAuthorization;
        $scope.ReferralModel.ReferralBillingAuthorization.ReferralID = $scope.ReferralModel.Referral.ReferralID;
        $("#referralBillingTaxonomyCode").removeClass("tooltip-danger input-validation-error").addClass("valid");
    };


    //#region Authrization List
    $scope.ReferralBillingAuthorizationList = [];
    $scope.SelectedReferralBillingAuthorizationIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.SearchReferralBillingAuthorization = $scope.newInstance().SearchReferralBillingAuthorization;
    $scope.TempReferralBillingAuthorization = $scope.newInstance().SearchReferralBillingAuthorization;
    $scope.ReferralBillingAuthorizationPager = new PagerModule("ReferralBillingAuthorizationID");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            encryptedReferralId: window.EncryptedReferralID,
            searchReferralBillingAuthorization: $scope.SearchReferralBillingAuthorization,
            pageSize: $scope.ReferralBillingAuthorizationPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ReferralBillingAuthorizationPager.sortIndex,
            sortDirection: $scope.ReferralBillingAuthorizationPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchReferralBillingAuthorization = $scope.TempReferralBillingAuthorization;
    };

    $scope.GetReferralBillingAuthorizationList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedReferralBillingAuthorizationIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchReferralBillingAuthorization.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();

        var jsonData = $scope.SetPostData($scope.ReferralBillingAuthorizationPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralBillingAuthorizationList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralBillingAuthorizationList = response.Data.Items;
                $scope.ReferralBillingAuthorizationPager.currentPageSize = response.Data.Items.length;
                $scope.ReferralBillingAuthorizationPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.ReferralBillingAuthorizationPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function (data) {
        $scope.SearchReferralBillingAuthorization = $scope.newInstance().SearchReferralBillingAuthorization;
        $scope.TempReferralBillingAuthorization = $scope.newInstance().SearchReferralBillingAuthorization;

        $scope.TempReferralBillingAuthorization.AuthorizationCode = null;
        $scope.TempReferralBillingAuthorization.StartDate = null;
        $scope.TempReferralBillingAuthorization.EndDate = null;
        $scope.TempReferralBillingAuthorization.IsDeleted = "0";
        if (data == "FormAuthCMS1500") {
            $scope.TempReferralBillingAuthorization.Type = 1;
        } else if (data == "FormAuthUB04") {
            $scope.TempReferralBillingAuthorization.Type = 2;
        }
        $scope.ReferralBillingAuthorizationPager.currentPage = 1;
        $scope.ReferralBillingAuthorizationPager.getDataCallback(true);

    };

    $scope.SearchBillingAuthorization = function () {
        $scope.ReferralBillingAuthorizationPager.currentPage = 1;
        $scope.ReferralBillingAuthorizationPager.getDataCallback(true);
    };

    $scope.SelectedReferralBillingAuthorization = function (ReferralBillingAuthorizationList) {
        if (ReferralBillingAuthorizationList.IsChecked)
            $scope.SelectedReferralBillingAuthorizationIds.push(ReferralBillingAuthorizationList.ReferralBillingAuthorizationID);
        else
            $scope.SelectedReferralBillingAuthorizationIds.remove(ReferralBillingAuthorizationList.ReferralBillingAuthorizationID);
        if ($scope.SelectedReferralBillingAuthorizationIds.length == $scope.ReferralBillingAuthorizationPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    $scope.SelectAll = function () {
        $scope.SelectedReferralBillingAuthorizationIds = [];
        angular.forEach($scope.ReferralBillingAuthorizationList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedReferralBillingAuthorizationIds.push(item.ReferralBillingAuthorizationID);
        });
        return true;
    };

    $scope.DeleteReferralBillingAuthorization = function (referralBillingAuthorizationID, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchReferralBillingAuthorization.ListOfIdsInCSV = referralBillingAuthorizationID > 0 ? referralBillingAuthorizationID.toString() : $scope.SelectedReferralBillingAuthorizationIds.toString();

                if (referralBillingAuthorizationID > 0) {
                    if ($scope.ReferralBillingAuthorizationPager.currentPage != 1)
                        $scope.ReferralBillingAuthorizationPager.currentPage = $scope.ReferralBillingAuthorizationList.length === 1 ? $scope.ReferralBillingAuthorizationList.currentPage - 1 : $scope.ReferralBillingAuthorizationList.currentPage;
                }
                else {
                    if ($scope.ReferralBillingAuthorizationPager.currentPage != 1 && $scope.SelectedReferralBillingAuthorizationIds.length == $scope.ReferralBillingAuthorizationPager.currentPageSize)
                        $scope.ReferralBillingAuthorizationPager.currentPage = $scope.ReferralBillingAuthorizationPager.currentPage - 1;
                }
                //Reset Selcted Checkbox items and Control
                $scope.SelectedReferralBillingAuthorizationIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.ReferralBillingAuthorizationPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteReferralBillingAuthorization, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.ReferralBillingAuthorizationList = response.Data.Items;
                        $scope.ReferralBillingAuthorizationPager.currentPageSize = response.Data.Items.length;
                        $scope.ReferralBillingAuthorizationPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationReferralAuthorizationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.EditReferralBillingAuthorization = function (isProfessionalAuthCode, data) {
        document.getElementById(isProfessionalAuthCode).scrollIntoView();
        if ($scope.ModifierListForDropDown.length == 0) {

            //$scope.OnServiceCodeChange();
        }
        var tempData = $.parseJSON(angular.toJson(data));
        $scope.ReferralModel.ReferralBillingAuthorization.ReferralBillingAuthorizationID = tempData.ReferralBillingAuthorizationID;
        $scope.ReferralModel.ReferralBillingAuthorization.AuthorizationCode = tempData.AuthorizationCode;
        $scope.ReferralModel.ReferralBillingAuthorization.StartDate = tempData.StartDate;
        $scope.ReferralModel.ReferralBillingAuthorization.EndDate = tempData.EndDate;
        $scope.ReferralModel.ReferralBillingAuthorization.IsDeleted = tempData.IsDeleted;
        $scope.ReferralModel.ReferralBillingAuthorization.PriorAuthorizationFrequencyType = tempData.PriorAuthorizationFrequencyType;
        $scope.ReferralModel.ReferralBillingAuthorization.PayorID = tempData.PayorID;
        $scope.ReferralModel.ReferralBillingAuthorization.AllowedTime = tempData.AllowedTime;
        $scope.ReferralModel.ReferralBillingAuthorization.ServiceCodeID = tempData.ServiceCodeID;
        $scope.ReferralModel.ReferralBillingAuthorization.AllowedTimeType = tempData.AllowedTimeType;
        $scope.ReferralModel.ReferralBillingAuthorization.DxCode = tempData.DxCode;
        $scope.ReferralModel.ReferralBillingAuthorization.DxCodeID = tempData.DxCodeID;
        $scope.ReferralModel.ReferralBillingAuthorization.PayRate = tempData.PayRate;
        $scope.ReferralModel.ReferralBillingAuthorization.FacilityCode = tempData.FacilityCode;
        $scope.PayorDetal.PayorInvoiceType = tempData.PayorInvoiceType;

        $scope.DXCodeIDs = [];
        $scope.DXCodes = [];
        $scope.DXcodeListDD.forEach(function (item, index) {

            if (tempData.DxCodeID && tempData.DxCodeID.split(',').indexOf('' + item.DXCodeID) > -1) {
                item.IsChecked = true;
                $scope.DXCodeIDs.push('' + item.DXCodeID);
                $scope.DXCodes.push('' + item.DXCodeWithoutDot);
            }
            else {
                item.IsChecked = false;
            }

        });
        // New fields as per new Billing change request
        // Kundan Kumar Rai
        $scope.ReferralModel.ReferralBillingAuthorization.ServiceCodeID = tempData.ServiceCodeID;
        $scope.ReferralModel.ReferralBillingAuthorization.Rate = tempData.Rate;
        $scope.ReferralModel.ReferralBillingAuthorization.RevenueCode = tempData.RevenueCode;
        $scope.ReferralModel.ReferralBillingAuthorization.UnitType = tempData.UnitType;
        $scope.ReferralModel.ReferralBillingAuthorization.PerUnitQuantity = tempData.PerUnitQuantity;
        $scope.ReferralModel.ReferralBillingAuthorization.RoundUpUnit = tempData.RoundUpUnit;
        $scope.ReferralModel.ReferralBillingAuthorization.MaxUnit = tempData.MaxUnit;
        $scope.ReferralModel.ReferralBillingAuthorization.DailyUnitLimit = tempData.DailyUnitLimit;
        $scope.ReferralModel.ReferralBillingAuthorization.UnitLimitFrequency = tempData.UnitLimitFrequency;
        $scope.ReferralModel.ReferralBillingAuthorization.CareType = tempData.CareType;
        $scope.ReferralModel.ReferralBillingAuthorization.TaxonomyID = tempData.TaxonomyID;
        $scope.ReferralModel.ReferralBillingAuthorization.ModifierID = tempData.ModifierID;

        $scope.Attachment(tempData.AttachmentFileName, tempData.AttachmentFilePath)
        $("#referralBillingTaxonomyCode").removeClass("tooltip-danger input-validation-error").addClass("valid");
    };

    $scope.ReferralBillingAuthorizationPager.getDataCallback = $scope.GetReferralBillingAuthorizationList;

    $scope.OnUnitTypeChange = function () {
        $scope.ReferralModel.ReferralBillingAuthorization.PerUnitQuantity = 0;
        $scope.ReferralModel.ReferralBillingAuthorization.RoundUpUnit = 0;
        $scope.ReferralModel.ReferralBillingAuthorization.MaxUnit = 0;
        $scope.ReferralModel.ReferralBillingAuthorization.DailyUnitLimit = 0;
    };



    $scope.CheckDxCode = function (reffer) {
        if (reffer != undefined) {
            var jsonData = angular.toJson({
                'refferalId': reffer,
                PayorID: $scope.ReferralModel.ReferralBillingAuthorization.PayorID
            });
            AngularAjaxCall($http, HomeCareSiteUrl.CheckDxCode, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response) { }
                if (!response) { toastr.error("Dx Code is not added"); }
            });
        }
    };
    














    $scope.AutoPopulateServiceCode = function (reffer) {
        $scope.CheckDxCode(reffer);
        if ($scope.ReferralModel.ReferralBillingAuthorization.PayorID > 0 && $scope.ReferralModel.ReferralBillingAuthorization.ServiceCodeID > 0 && $scope.ReferralModel.ReferralBillingAuthorization.CareType > 0) {
            var jsonData = angular.toJson({
                'CareType': $scope.ReferralModel.ReferralBillingAuthorization.CareType, 'ServiceCode': $scope.ReferralModel.ReferralBillingAuthorization.ServiceCodeID,
                'ModifierID': null, 'PosID': null, 'POSStartDate': null, 'POSEndDate': null, 'PayorID': $scope.ReferralModel.ReferralBillingAuthorization.PayorID
            });
            AngularAjaxCall($http, HomeCareSiteUrl.GetServiceCodeMappingListNew, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    var PayorServiceCodeMappings = response.Data.Items[0] || {};
                    $scope.ReferralModel.ReferralBillingAuthorization.UnitType = PayorServiceCodeMappings.UnitType;
                    $scope.ReferralModel.ReferralBillingAuthorization.StartDate = PayorServiceCodeMappings.POSStartDate;
                    $scope.ReferralModel.ReferralBillingAuthorization.EndDate = PayorServiceCodeMappings.POSEndDate;
                    $scope.ReferralModel.ReferralBillingAuthorization.Rate = PayorServiceCodeMappings.Rate;
                    $scope.ReferralModel.ReferralBillingAuthorization.RevenueCode = PayorServiceCodeMappings.RevenueCodeID;
                    $scope.ReferralModel.ReferralBillingAuthorization.PerUnitQuantity = PayorServiceCodeMappings.PerUnitQuantity;
                    $scope.ReferralModel.ReferralBillingAuthorization.RoundUpUnit = PayorServiceCodeMappings.RoundUpUnit;
                    $scope.ReferralModel.ReferralBillingAuthorization.MaxUnit = PayorServiceCodeMappings.MaxUnit;
                    $scope.ReferralModel.ReferralBillingAuthorization.DailyUnitLimit = PayorServiceCodeMappings.DailyUnitLimit;
                }
                ShowMessages(response);
            });

        }
        $scope.GetReferralPayorDetail($scope.ReferralModel.ReferralBillingAuthorization)

    };
    $scope.ModifierListForDropDown = [];
    $scope.OnServiceCodeChange = function () {
        $scope.ReferralModel.ReferralBillingAuthorization.ServiceCodeID = $scope.ReferralModel.ReferralBillingAuthorization.ServiceCodeID ? $scope.ReferralModel.ReferralBillingAuthorization.ServiceCodeID : 0;
        var jsonData = angular.toJson({ 'ServiceCodeID': $scope.ReferralModel.ReferralBillingAuthorization.ServiceCodeID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetModifierByServiceCodeListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.ModifierListForDropDown = response.Data;
                if ($scope.ModifierListForDropDown !== undefined && $scope.ModifierListForDropDown.length === 1) {
                    $scope.ReferralModel.ReferralBillingAuthorization.ModifierID = 0; //$scope.ModifierListForDropDown[0].ModifierID;
                }
                else {
                    $scope.ReferralModel.ReferralBillingAuthorization.ModifierID = 0;
                }
            }
        });
        $scope.AutoPopulateServiceCode();
    };

    $scope.$watch(function () { return $scope.ReferralModel.ReferralBillingAuthorization.UnitType; }, function (newValue, oldValue) {
        if (!ValideElement(newValue)) $scope.SelectedUnit = "";
        if (parseInt(newValue) === parseInt(window.UnitTime)) $scope.SelectedUnit = window.UnitTimeValue;
        if (!$scope.$root.$$phase) $scope.$apply();

    }, true);

    $scope.$watch(function () { return $scope.ReferralModel.ReferralBillingAuthorization.DefaultUnitIgnoreCalculation; }, function (newValue, oldValue) {
        if (!ValideElement(newValue)) $scope.ReferralModel.ReferralBillingAuthorization.DefaultUnitIgnoreCalculation = 0;
        if (!$scope.$root.$$phase) $scope.$apply();
    });

    $scope.$watch(function () { return $scope.ReferralModel.ReferralBillingAuthorization.MaxUnit; }, function (newValue, oldValue) {
        if (!ValideElement(newValue)) $scope.ReferralModel.ReferralBillingAuthorization.MaxUnit = 0;
        if (!$scope.$root.$$phase) $scope.$apply();
    });

    $scope.$watch(function () { return $scope.ReferralModel.ReferralBillingAuthorization.DailyUnitLimit; }, function (newValue, oldValue) {
        if (!ValideElement(newValue)) $scope.ReferralModel.ReferralBillingAuthorization.DailyUnitLimit = 0;
        if (!$scope.$root.$$phase) $scope.$apply();
    });

    $scope.GetAuthorizationLoadModel = function () {
        var jsonData = angular.toJson({ 'encryptedReferralId': window.EncryptedReferralID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetAuthorizationLoadModelURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.PatientPayorList = response.Data.PatientPayorList;
            }
        });
    }



    $scope.$watch('ReferralModel.ReferralBillingAuthorization.PayorID', function (newValue, oldValue) {
        if (ValideElement(newValue)) {
            var jsonData = angular.toJson({ 'PayorID': newValue });
            AngularAjaxCall($http, HomeCareSiteUrl.GetPayorMappedServiceCodesURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.PayorMappedServiceCodeList = response.Data;
                }
            });
        }

    });

    $scope.SearchAuthServiceCodesModel = {};
    $scope.GetAuthServiceCodesURL = HomeCareSiteUrl.GetAuthServiceCodesURL;


    // Check endDate expired
    $scope.IsEndDateExp = function (eDate) {
        var isPastMonth = false;
        if (eDate) {
            var endDate = new Date(eDate);
            var currentDate = new Date();
            var diff = currentDate - endDate;
            if (diff > 0) {
                var days = (24 * 3600 * 1000);
                var days = diff / (24 * 3600 * 1000);
                if (days >= 1) {
                    isPastMonth = true;
                }
            }
        }
        return isPastMonth;
    }



    //$scope.GetPayorMappedServiceCodeListURL = HomeCareSiteUrl.GetPayorMappedServiceCodeListURL;

    //$scope.ServiceCodeResultsFormatter = function (item) {
    //    return "<li id='{0}'><b style='color:#ad0303;'>Code: </b>{0}<br/><b style='color:#ad0303;'>Name: </b>{1}<br/><b style='color:#ad0303;'>Billable: </b>{2}</li>"
    //        .format(
    //            item.ServiceCode,
    //            item.ServiceName,
    //            item.IsBillable ? window.Yes : window.No
    //        );
    //};
    //$scope.ServiceCodeTokenFormatter = function (item) {
    //    return "<li id='{0}'>{0}</li>"
    //        .format(
    //            item.ServiceCode
    //        );
    //};


    //#endregion


    $("a#billings_detailsbillingsettings, a#billings_detailsbillingsettings_CMS1500").on('shown.bs.tab', function (e) {
        window.location.hash = $(this).attr('id');
        $scope.GetReferralBillingSetting();
        $scope.GetAuthorizationLoadModel();
        $scope.TempReferralBillingAuthorization.Type = 1;
        $scope.IsProfessionalAuthCode = true;
        $scope.ReferralBillingAuthorizationPager.getDataCallback(true);
        $scope.ClearDetails();
        if ($scope.AuthFormID)
            HideErrors($scope.AuthFormID);
        $scope.GetDXcodeListDD($scope.ReferralModel.ReferralBillingAuthorization);
        //$('a[href="#billingSetting_CMS1500"]').tab('show');
    });

    $("a#billings_detailsbillingsettings_UB04").on('shown.bs.tab', function (e) {
        window.location.hash = $(this).attr('id');
        $scope.GetReferralBillingSetting();
        $scope.GetAuthorizationLoadModel();
        $scope.TempReferralBillingAuthorization.Type = 2;
        $scope.IsProfessionalAuthCode = false;
        $scope.ReferralBillingAuthorizationPager.getDataCallback(true);
        $scope.ClearDetails();
        if ($scope.AuthFormID)
            HideErrors($scope.AuthFormID);
    });






    //#endregion

    //#region New Billing Prior Authroization Details

    //#region Add Prior Authorization Modal

    $scope.AddPriorAuthorizationModel = {};
    $scope.OpenPriorAuthorizationModal = function (item) {

        $scope.AddPriorAuthorizationModel = {};
        HideErrors($("#frmAddPriorAuthorization"));
        $('#model_AddPriorAuthorization').modal({
            backdrop: 'static',
            keyboard: false
        });

        if (ValideElement(item)) {

            var tempData = $.parseJSON(angular.toJson(item));
            $scope.AddPriorAuthorizationModel.ReferralBillingAuthorizationID = tempData.ReferralBillingAuthorizationID;
            $scope.AddPriorAuthorizationModel.AuthorizationCode = tempData.AuthorizationCode;
            $scope.AddPriorAuthorizationModel.StartDate = tempData.StartDate;
            $scope.AddPriorAuthorizationModel.EndDate = tempData.EndDate;
            $scope.AddPriorAuthorizationModel.IsDeleted = tempData.IsDeleted;
            $scope.AddPriorAuthorizationModel.PayorID = tempData.PayorID;

        }






    }
    $scope.ClosePriorAuthorizationModal = function () {
        $('#model_AddPriorAuthorization').modal('hide');
        $scope.AddPriorAuthorizationModel = {};
        HideErrors($("#frmAddPriorAuthorization"));
    };
    $scope.SavePriorAuthorization = function (isProfessionalAuthCode) {

        $scope.IsProfessionalAuthCode = 1;
        $scope.AuthFormID = '#frmAddPriorAuthorization';//+ isProfessionalAuthCode;
        if (CheckErrors($scope.AuthFormID)) {

            if ($scope.IsProfessionalAuthCode) {
                $scope.AddPriorAuthorizationModel.AuthType = 1;
                $scope.TempReferralBillingAuthorization.Type = 1;
            }
            else {
                $scope.AddPriorAuthorizationModel.AuthType = 2;
                $scope.TempReferralBillingAuthorization.Type = 2;
            }


            $scope.AddPriorAuthorizationModel.ReferralID = $scope.ReferralModel.Referral.ReferralID;

            var jsonData = angular.toJson($scope.AddPriorAuthorizationModel);
            AngularAjaxCall($http, HomeCareSiteUrl.SavePriorAuthorizationUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.PriorAuthorizationPager.getDataCallback();
                    $scope.ClosePriorAuthorizationModal();
                }
            });
        }
    };

    //#endregion Add Prior Authorization Modal

    //#region New Authrization Search & List
    $scope.PriorAuthorizationList = [];
    $scope.SearchPriorAuthorization = $scope.newInstance().SearchReferralBillingAuthorization;
    $scope.TempSearchPriorAuthorization = $scope.newInstance().SearchReferralBillingAuthorization;
    $scope.PriorAuthorizationPager = new PagerModule("ReferralBillingAuthorizationID");
    $scope.PriorAuthorizationPager.pageSize = 100;

    $scope.SetPAPostData = function (fromIndex) {
        var pagermodel = {
            encryptedReferralId: window.EncryptedReferralID,
            searchReferralBillingAuthorization: $scope.SearchPriorAuthorization,
            pageSize: $scope.PriorAuthorizationPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.PriorAuthorizationPager.sortIndex,
            sortDirection: $scope.PriorAuthorizationPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchPriorAuthorizationModelMapping = function () {
        $scope.SearchPriorAuthorization = $scope.TempSearchPriorAuthorization;
    };

    $scope.GetPriorAuthorizationList = function (isSearchDataMappingRequire) {

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchPriorAuthorizationModelMapping();

        var jsonData = $scope.SetPAPostData($scope.PriorAuthorizationPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetPriorAuthorizationListUrl, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.PriorAuthorizationList = response.Data.Items;
                $scope.PriorAuthorizationPager.currentPageSize = response.Data.Items.length;
                $scope.PriorAuthorizationPager.totalRecords = response.Data.TotalItems;

                $timeout(function () {
                    $.each($scope.PriorAuthorizationList, function (index, data) {
                        $scope.GetPAServiceCodeList(data);
                    });
                })

            }
            ShowMessages(response);
        });
    };

    $scope.RefreshPriorAuthorization = function () {
        $scope.PriorAuthorizationPager.getDataCallback();
    };

    $scope.ResetPriorAuthorizationSearchFilter = function (data) {
        $scope.SearchPriorAuthorization = $scope.newInstance().SearchReferralBillingAuthorization;
        $scope.TempSearchPriorAuthorization = $scope.newInstance().SearchReferralBillingAuthorization;

        $scope.TempSearchPriorAuthorization.AuthorizationCode = null;
        $scope.TempSearchPriorAuthorization.StartDate = null;
        $scope.TempSearchPriorAuthorization.EndDate = null;
        $scope.TempSearchPriorAuthorization.IsDeleted = "0";

        if (data == "FormAuthCMS1500") {
            $scope.TempReferralBillingAuthorization.Type = 1;
        } else if (data == "FormAuthUB04") {
            $scope.TempReferralBillingAuthorization.Type = 2;
        }

        $scope.PriorAuthorizationPager.currentPage = 1;
        $scope.PriorAuthorizationPager.getDataCallback(true);
    };

    $scope.SearchPriorAuthorizationClick = function () {
        $scope.PriorAuthorizationPager.currentPage = 1;
        $scope.PriorAuthorizationPager.getDataCallback(true);
    };
    $scope.PriorAuthorizationPager.getDataCallback = $scope.GetPriorAuthorizationList;




    $scope.DeletePriorAuthorization = function (referralBillingAuthorizationID, title) {

        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchPriorAuthorization.ListOfIdsInCSV = referralBillingAuthorizationID > 0 ? referralBillingAuthorizationID.toString() : $scope.SelectedReferralBillingAuthorizationIds.toString();

                if (referralBillingAuthorizationID > 0) {
                    if ($scope.PriorAuthorizationPager.currentPage != 1)
                        $scope.PriorAuthorizationPager.currentPage = $scope.PriorAuthorizationList.length === 1 ? $scope.PriorAuthorizationList.currentPage - 1 : $scope.PriorAuthorizationList.currentPage;
                }
                else {
                    if ($scope.PriorAuthorizationPager.currentPage != 1 && $scope.SelectedReferralBillingAuthorizationIds.length == $scope.PriorAuthorizationPager.currentPageSize)
                        $scope.PriorAuthorizationPager.currentPage = $scope.PriorAuthorizationPager.currentPage - 1;
                }
                //Reset Selcted Checkbox items and Control
                $scope.SelectedReferralBillingAuthorizationIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPAPostData($scope.PriorAuthorizationPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeletePriorAuthorizationUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.PriorAuthorizationList = response.Data.Items;
                        $scope.PriorAuthorizationPager.currentPageSize = response.Data.Items.length;
                        $scope.PriorAuthorizationPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationReferralAuthorizationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };


    //$scope.GetAuthorizationLoadModel = function () {
    //    var jsonData = angular.toJson({ 'encryptedReferralId': window.EncryptedReferralID });
    //    AngularAjaxCall($http, HomeCareSiteUrl.GetAuthorizationLoadModelURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
    //        ShowMessages(response);
    //        if (response.IsSuccess) {
    //            $scope.PatientPayorList = response.Data.PatientPayorList;
    //        }
    //    });
    //}

    $("a#billings_PriorAuthorization").on('shown.bs.tab', function (e) {
        window.location.hash = $(this).attr('id');
        $scope.GetPriorAuthorizationList();
        $scope.GetAuthorizationLoadModel();
        $scope.TempReferralBillingAuthorization.Type = 1;
        $scope.IsProfessionalAuthCode = true;
    });

    //#endregion


    //#region Add Service Code For Prior Authorization Modal

    $scope.AddPAServiceCodeModel = {};
    $scope.OpenPAServiceCodeModal = function (authorizationModel, paServiceCodeModel) {

        $scope.AddPAServiceCodeModal = {};
        HideErrors($("#frmAddPAServiceCode"));
        $('#model_AddPAServiceCode').modal({
            backdrop: 'static',
            keyboard: false
        });
        var tempData = null;
        if (ValideElement(authorizationModel)) {

            tempData = $.parseJSON(angular.toJson(authorizationModel));
            $scope.AddPAServiceCodeModel.ReferralBillingAuthorizationID = tempData.ReferralBillingAuthorizationID;
            $scope.AddPAServiceCodeModel.AuthorizationCode = tempData.AuthorizationCode;
            $scope.AddPAServiceCodeModel.StartDate = tempData.StartDate;
            $scope.AddPAServiceCodeModel.EndDate = tempData.EndDate;
            $scope.AddPAServiceCodeModel.IsDeleted = tempData.IsDeleted;
            $scope.AddPAServiceCodeModel.PayorID = tempData.PayorID;
            $scope.AddPAServiceCodeModel.PayorName = tempData.PayorName;

        }

        if (ValideElement(paServiceCodeModel)) {
            tempData = $.parseJSON(angular.toJson(paServiceCodeModel));
            $scope.AddPAServiceCodeModel.ReferralBillingAuthorizationServiceCodeID = tempData.ReferralBillingAuthorizationServiceCodeID;
            $scope.AddPAServiceCodeModel.ReferralBillingAuthorizationID = tempData.ReferralBillingAuthorizationID;
            $scope.AddPAServiceCodeModel.ReferralID = tempData.ReferralID;
            $scope.AddPAServiceCodeModel.ServiceCodeID = tempData.ServiceCodeID;
            $scope.AddPAServiceCodeModel.DailyUnitLimit = tempData.DailyUnitLimit;
            $scope.AddPAServiceCodeModel.MaxUnitLimit = tempData.MaxUnitLimit;


            $("#SearchPAServiceCodeToken").tokenInput("clear");
            //$("#SearchContactToken").tokenInput("add", { ServiceCodeID: tempData.ServiceCodeID, ServiceCode: tempData.ServiceCode });
            // tempData.ServiceCode = tempData.ServiceCode + ((tempData.ModifierName === null || tempData.ModifierName === '') ? '' : ' - ' + tempData.ModifierName)
            $("#SearchPAServiceCodeToken").tokenInput("add", { ServiceCode: tempData.ServiceCode, ServiceCodeID: tempData.ServiceCodeID });
            // $scope.NewDate = $scope.ServiceCodeModel.PayorServiceCodeMapping.POSStartDate;
        }


    }
    $scope.ClosePAServiceCodeModal = function () {
        $('#model_AddPAServiceCode').modal('hide');
        $("#SearchPAServiceCodeToken").tokenInput("clear");
        $scope.AddPAServiceCodeModel = {};
        HideErrors($("#frmAddPAServiceCode"));
    };

    $scope.GetPAServiceCodeList = function (item) {

        var jsonData = angular.toJson({
            ReferralBillingAuthorizationID: item.ReferralBillingAuthorizationID
        });

        item.IsLoading = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetPAServiceCodeListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            item.IsLoading = false;
            if (response.IsSuccess) {
                item.PAServiceCodeList = response.Data;
            }
            ShowMessages(response);

        });
    };

    $scope.SavePAServiceCodeDetails = function (item) {

        $scope.ServiceCodeFormID = '#frmAddPAServiceCode';
        if (CheckErrors($scope.ServiceCodeFormID)) {

            var jsonData = angular.toJson($scope.AddPAServiceCodeModel);
            AngularAjaxCall($http, HomeCareSiteUrl.SavePriorAuthorizationServiceCodeDetailsUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.GetPAServiceCodeList(item)
                    $scope.ClosePriorAuthorizationModal();
                }
            });
        }
    };


    $scope.DeletePAServiceCode = function (referralBillingAuthorizationServiceCodeID, title) {

        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = { 'referralBillingAuthorizationServiceCodeID': referralBillingAuthorizationServiceCodeID };
                AngularAjaxCall($http, HomeCareSiteUrl.DeletePAServiceCodeUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {

                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationReferralPAServiceCodeMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };







    //#region Service Code AutoCompleter
    $scope.GetPayorServiceCodeListURL = HomeCareSiteUrl.GetPayorServiceCodeListURL;
    $scope.ServiceCodeResultsFormatter = function (item) {
        return "<li id='{0}'><b style='color:#ad0303;'>Code: </b>{0}<br/><b style='color:#ad0303;'>Name: </b>{1}<br/><b style='color:#ad0303;'>Billable: </b>{2}</li>"
            .format(
                item.ServiceCode,
                item.ServiceName,
                item.IsBillable ? window.Yes : window.No
            );
    };

    $scope.ServiceCodeTokenFormatter = function (item) {
        return "<li id='{0}'>{0}</li>"
            .format(
                item.ServiceCode
            );
    };



    $scope.RemovedPAServiceToken = function () {

        $scope.AddPAServiceCodeModel.TempDailyUnitLimit = undefined;
        $scope.AddPAServiceCodeModel.TempMaxUnit = undefined;
        $scope.AddPAServiceCodeModel.TempUnitType = undefined;
        $scope.AddPAServiceCodeModel.TempPerUnitQuantity = undefined;


        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };
    $scope.AddedPAServiceToken = function (item) {

        if ($scope.AddPAServiceCodeModel.ReferralBillingAuthorizationServiceCodeID == 0 ||
            !ValideElement($scope.AddPAServiceCodeModel.ReferralBillingAuthorizationServiceCodeID)) {

            $scope.AddPAServiceCodeModel.TempDailyUnitLimit = item.DailyUnitLimit;
            $scope.AddPAServiceCodeModel.TempMaxUnit = item.MaxUnit;
            $scope.AddPAServiceCodeModel.TempUnitType = item.UnitType;
            $scope.AddPAServiceCodeModel.TempPerUnitQuantity = item.PerUnitQuantity;

            $scope.AddPAServiceCodeModel.DailyUnitLimit = item.DailyUnitLimit;
            $scope.AddPAServiceCodeModel.MaxUnitLimit = item.MaxUnit;
            $scope.AddPAServiceCodeModel.ServiceCodeID = item.ServiceCodeID;
        }




        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };

    $scope.UploadingFileList = [];
    $scope.UploadFileURL = HomeCareSiteUrl.UploadFile;
    $scope.BeforeSend = function (e, data) {
        var isValidImage = true;
        var fileName;
        var errorMsg;
        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            //if (extension == "exe") {
            if (extension != "doc" && extension != "docx" && extension != "txt" && extension != "pdf" && extension != "rtf" && extension != "jpg" && extension != "jpeg"
                && extension != "png" && extension != "xls" && extension != "xlsx") {
                //file.FileProgress = 100;
                $scope.UploadingFileList.remove(file);
                //errorMsg = window.InvalidImageUploadMessage;
                //isValidImage = false;
                ShowMessage(window.InvalidDocumentUploadMessage, "error");
                isValidImage = false;
            }
            if ((file.size / 1024) > parseInt(window.FileSize)) {
                file.FileProgress = 100;
                errorMsg = window.MaximumUploadImageSizeMessage;
                isValidImage = false;
            }

            fileName = file.name;
        });

        if (isValidImage) {
            $scope.IsFileUploading = true;
        }
        $scope.$apply();
        var response = { IsSuccess: isValidImage, Message: errorMsg };
        return response;
    };

    $scope.Progress = function (e, data) {
        console.log(data.files[0].name + ":-" + Math.round((data.loaded / data.total) * 100));
    };

    $scope.AfterSend = function (e, data) {
        var result = data.result;
        if (result.IsSuccess) {
            ShowMessage("File uploaded successfully");
            var file = result.Data[0];
            $scope.Attachment(file.FileName, file.Path);
            $scope.$apply();
        } else {
            ShowMessages(result);
        }
    };

    $scope.Attachment = function (fileName, path) {
        $scope.ReferralModel.ReferralBillingAuthorization.AttachmentFileName = fileName;
        $scope.ReferralModel.ReferralBillingAuthorization.AttachmentFilePath = path;
    };

    //#endregion
    $scope.GetDXcodeListDD = function (item) {
        $scope.DXcodeListDD = [];
        var jsonData = angular.toJson({ ReferralID: item.ReferralID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetDXcodeListDDURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.DXcodeListDD = response.Data;
            }
            ShowMessages(response);
        });

    };
    $scope.OpenDXmodel = function () {

        $('#ReferralDxCodeModal').modal({
            backdrop: 'static',
            keyboard: false
        });
    }
    $scope.CloseMode = function () {
        $('#ReferralDxCodeModal').modal('hide');
    }
    $scope.moveUp = function (index, item) {
        Preindex = index - 1;
        var Precedence = $scope.DXcodeListDD[Preindex].Precedence;
        move(item.Precedence, Precedence, item.ReferralDXCodeMappingID);

    };

    $scope.moveDown = function (index, item) {
        Nextindex = index + 1;
        var Precedence = $scope.DXcodeListDD[Nextindex].Precedence;
        move(item.Precedence, Precedence, item.ReferralDXCodeMappingID);

    };

    var move = function (origin, destination, ReferralDXCodeMappingID) {
        console.log(origin, destination);
        var jsonData = angular.toJson({ originID: origin, destinationID: destination, ReferralDXCodeMappingID: ReferralDXCodeMappingID });
        AngularAjaxCall($http, HomeCareSiteUrl.DxChangeSortingOrderURL, jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                $scope.GetDXcodeListDD({ ReferralID: $scope.ReferralModel.ReferralBillingAuthorization.ReferralID });
            });
    };
    $scope.DXCodeIDs = [];
    $scope.DXCodes = [];
    $scope.SelectDxCode = function (item) {
        if (item.IsChecked) {
            $scope.DXCodeIDs.push(item.DXCodeID);
            $scope.DXCodes.push(item.DXCodeWithoutDot);
        }
        else {
            $scope.DXCodeIDs.remove(item.DXCodeID);
            $scope.DXCodes.remove(item.DXCodeWithoutDot);
        }
    };
    $scope.SaveDxCode = function () {
        $scope.DXCodeID = null;
        
        
        $scope.ReferralModel.ReferralBillingAuthorization.DxCode = $scope.DXCodes.toString();
        $scope.ReferralModel.ReferralBillingAuthorization.DxCodeID = $scope.DXCodeIDs.toString();
        /**/
        $scope.DXCode = $scope.DXCodes.toString();
        $scope.DXCodeID = $scope.DXCodeIDs.toString();
        $scope.ReferralBillingAuthorizationID = $scope.ReferralModel.ReferralBillingAuthorization.ReferralBillingAuthorizationID;
        $scope.DXCodeIDs = [];
        $scope.DXCodes = [];
        //var jsonData = angular.toJson({ DXCodeID: $scope.DXCodeID, ReferralBillingAuthorizationID: $scope.ReferralBillingAuthorizationID });
        //AngularAjaxCall($http, HomeCareSiteUrl.SaveDXcodeListDDURL, jsonData, "post", "json", "application/json", true).
        //    success(function (response, status, headers, config) {
        //        ShowMessages(response);
        //    });
        $('#ReferralDxCodeModal').modal('hide');
    };
    $scope.PayorDetal = [];
    $scope.GetReferralPayorDetail = function (ReferralBillingAuthorization) {
        var jsonData = angular.toJson({ PayorID: ReferralBillingAuthorization.PayorID, ReferralID: ReferralBillingAuthorization.ReferralID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetPayorDetailsURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.PayorDetal = response.Data;
            }
            ShowMessages(response);
        });
    };

    //#endregion Add Service Code For Prior Authorization Modal




    //#endregion


    $scope.OpenDxCodeModel = function () {
        $scope.GetDXcodeListDD($scope.ReferralModel.ReferralBillingAuthorization);
        $('#ReferralDxCodeModal').modal({
            backdrop: 'static',
            keyboard: false
        });
    }

   


    };

controllers.BillingSettingController.$inject = ['$scope', '$http', '$window', '$timeout'];



