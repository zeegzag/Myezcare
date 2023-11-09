var rimModel;

controllers.ReferralPatientsPayorController = function ($scope, $http, $window, $timeout) {
    rimModel = $scope;
    $scope.CurrentDate = SetExpiryDate();
    var modalJson = $.parseJSON($("#hdnSetReferralPatientsPayorModel").val());
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetReferralPatientsPayorModel").val());
    };
    $scope.ReferralModel = modalJson;
    $scope.ReferralPayorMapping = $scope.ReferralModel.ReferralPayorMapping;
    $scope.ReferralPayorMapping.EncryptedReferralId = window.EncryptedReferralID;
    //$scope.ReferralModel.ReferralPayorMapping = null;
    $scope.ReferralPayorMappingList = [];
    $scope.SelectedReferralPayorMappingIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.PayorList = $scope.newInstance().PayorList;
    $scope.SearchReferralPayorMappingList = $scope.newInstance().SearchReferralPayorMapping;
    $scope.TempReferralPayorMappingList = $scope.newInstance().SearchReferralPayorMapping;
    $scope.ReferralPayorMappingListPager = new PagerModule("PayorID");
    $scope.Insured_Detail = false;
    $scope.CheckPrimaryInsured = function () {
        if ($scope.ReferralModel.Contact !== undefined && $scope.ReferralModel.Contact !== null && $scope.ReferralPayorMapping.IsPayorNotPrimaryInsured === false) {

            $scope.ReferralPayorMapping.InsuredFirstName = $scope.ReferralModel.Contact.FirstName;
            $scope.ReferralPayorMapping.InsuredLastName = $scope.ReferralModel.Contact.LastName;
            $scope.ReferralPayorMapping.InsuredAddress = $scope.ReferralModel.Contact.Address;
            $scope.ReferralPayorMapping.InsuredCity = $scope.ReferralModel.Contact.City;
            $scope.ReferralPayorMapping.InsuredState = $scope.ReferralModel.Contact.State;
            $scope.ReferralPayorMapping.InsuredZipCode = $scope.ReferralModel.Contact.ZipCode;
            $scope.ReferralPayorMapping.InsuredPhone = $scope.ReferralModel.Contact.Phone1;
            $scope.ReferralPayorMapping.InsuredDateOfBirth = $scope.ReferralModel.Referral.Dob;
            $scope.ReferralPayorMapping.InsuredGender = $scope.ReferralModel.Referral.Gender;
            $scope.Insured_Detail = true;
        }
        else {
            $scope.ReferralPayorMapping.InsuredFirstName = '';
            $scope.ReferralPayorMapping.InsuredLastName = '';
            $scope.ReferralPayorMapping.InsuredAddress = '';
            $scope.ReferralPayorMapping.InsuredCity = '';
            $scope.ReferralPayorMapping.InsuredState = '';
            $scope.ReferralPayorMapping.InsuredZipCode = '';
            $scope.ReferralPayorMapping.InsuredPhone = '';
            $scope.ReferralPayorMapping.InsuredDateOfBirth = '';
            $scope.ReferralPayorMapping.InsuredGender = '';
            $scope.Insured_Detail = false;
        }
    };

    $scope.CheckPrimaryInsured();

    $scope.PrimaryInsured = function (Precedence) {
        if (Precedence != 1) {
            $scope.Insured_Detail = false;
        }
        else {
            $scope.Insured_Detail = false;
        }
    }
    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            encryptedReferralId: window.EncryptedReferralID,
            searchReferralPayorMapping: $scope.SearchReferralPayorMappingList,
            pageSize: $scope.ReferralPayorMappingListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ReferralPayorMappingListPager.sortIndex,
            sortDirection: $scope.ReferralPayorMappingListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.$watch('ReferralModel.ReferralPayorMapping.PayorID', function (newValue, oldValue) {
        $scope.ReferralModel.Referral.SelectedPayor = newValue;
        $scope.ReferralModel.Referral.SelectedPayorClaimProcessor = null;
        var payor = $scope.PayorList.find(p => p.PayorID === $scope.ReferralModel.Referral.SelectedPayor);
        if (payor) {
            $scope.ReferralModel.Referral.SelectedPayorClaimProcessor = payor.ClaimProcessor;
        }
        if (!$scope.ShowJurisdictionAndTimeZone()) {
            $scope.ReferralModel.ReferralPayorMapping.MasterJurisdictionID = null;
            $scope.ReferralModel.ReferralPayorMapping.MasterTimezoneID = null;
        }
        $scope.GetMasterJurisdictionList();
        $scope.GetMasterTimezoneList();
    });

    $scope.GetMasterJurisdictionList = function () {
        var model = {
            claimProcessor: $scope.ReferralModel.Referral.SelectedPayorClaimProcessor
        };
        var jsonData = angular.toJson(model);
        AngularAjaxCall($http, HomeCareSiteUrl.GetMasterJurisdictionListUrl, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralModel.MasterJurisdictionList = response.Data;
            }
            ShowMessages(response);
        });
    };

    $scope.ShowJurisdictionAndTimeZone = function () {
        return window.SandataClaimProcessor == $scope.ReferralModel.Referral.SelectedPayorClaimProcessor;
    }

    $scope.GetMasterTimezoneList = function () {
        var model = {
            claimProcessor: $scope.ReferralModel.Referral.SelectedPayorClaimProcessor
        };
        var jsonData = angular.toJson(model);
        AngularAjaxCall($http, HomeCareSiteUrl.GetMasterTimezoneListUrl, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralModel.MasterTimezoneList = response.Data;
            }
            ShowMessages(response);
        });
    };

    $scope.SearchModelMapping = function () {
       // $scope.SearchReferralPayorMappingList = $.parsejson(angular.tojson($scope.TempReferralPayorMappingList));
        $scope.SearchReferralPayorMappingList = $scope.TempReferralPayorMappingList;
    };

    $scope.GetReferralPayorMappingList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedReferralPayorMappingIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchReferralPayorMappingList.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();

        var jsonData = $scope.SetPostData($scope.ReferralPayorMappingListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralPayorMappingListUrl, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralPayorMappingList = response.Data.Items;
                $scope.ReferralPayorMappingListPager.currentPageSize = response.Data.Items.length;
                $scope.ReferralPayorMappingListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.ReferralPayorMappingListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $scope.SearchReferralPayorMappingList = $scope.newInstance().SearchReferralPayorMapping;
        $scope.TempReferralPayorMappingList = $scope.newInstance().SearchReferralPayorMapping;

        $scope.TempReferralPayorMappingList.PayorName = null;
        $scope.TempReferralPayorMappingList.IsDeleted = "0";
        $scope.TempReferralPayorMappingList.Precedence ="0";
        $scope.TempReferralPayorMappingList.PayorEffectiveDate =null;
        $scope.TempReferralPayorMappingList.PayorEffectiveEndDate = null;

        $scope.ReferralPayorMappingListPager.currentPage = 1;
        $scope.ReferralPayorMappingListPager.getDataCallback();
    };

    $scope.SearchReferralPayorMapping = function () {
        $scope.ReferralPayorMappingListPager.currentPage = 1;
        $scope.ReferralPayorMappingListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectReferralPayorMapping = function (ReferralPayorMappingList) {
        if (ReferralPayorMappingList.IsChecked)
            $scope.SelectedReferralPayorMappingIds.push(ReferralPayorMappingList.ReferralPayorMappingID);
        else
            $scope.SelectedReferralPayorMappingIds.remove(ReferralPayorMappingList.ReferralPayorMappingID);
        if ($scope.SelectedReferralPayorMappingIds.length == $scope.ReferralPayorMappingListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.

    $scope.SelectAll = function () {
        $scope.SelectedReferralPayorMappingIds = [];
        angular.forEach($scope.ReferralPayorMappingList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedReferralPayorMappingIds.push(item.ReferralPayorMappingID);
        });
        return true;
    };

    $scope.DeleteReferralPayorMapping = function (referralPayorMappingID, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchReferralPayorMappingList.ListOfIdsInCSV = referralPayorMappingID > 0 ? referralPayorMappingID.toString() : $scope.SelectedReferralPayorMappingIds.toString();

                if (referralPayorMappingID > 0) {
                    if ($scope.ReferralPayorMappingListPager.currentPage != 1)
                        $scope.ReferralPayorMappingListPager.currentPage = $scope.ReferralPayorMappingList.length === 1 ? $scope.ReferralPayorMappingList.currentPage - 1 : $scope.ReferralPayorMappingList.currentPage;
                }
                else {
                    if ($scope.ReferralPayorMappingListPager.currentPage != 1 && $scope.SelectedReferralPayorMappingIds.length == $scope.ReferralPayorMappingListPager.currentPageSize)
                        $scope.ReferralPayorMappingListPager.currentPage = $scope.ReferralPayorMappingListPager.currentPage - 1;
                }
                //Reset Selcted Checkbox items and Control
                $scope.SelectedReferralPayorMappingIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.ReferralPayorMappingListPager.currentPage);
                AngularAjaxCall($http, '/hc/Referral/DeleteReferralPayorMapping', jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.ReferralPayorMappingList = response.Data.listModel.Items;
                        $scope.ReferralPayorMappingListPager.currentPageSize = response.Data.listModel.Items.length;
                        $scope.ReferralPayorMappingListPager.totalRecords = response.Data.listModel.TotalItems;
                    }
                    if (response.Data.DuplicateRecords > 0) 
                        ShowMessage("Some Records are conflicted with existing Records. so, we can't actived that.", "warning");
                    else
                        ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

   

    $scope.SaveReferralPayorMappingDetails = function () {
        debugger
        if (CheckErrors("#FrmPatientsPayor")) {
            if ($scope.ReferralModel.ReferralPayorMapping.Precedence != null || ReferralModel.ReferralPayorMapping.Precedence != undefined) {
                var jsonData = angular.toJson($scope.ReferralModel.ReferralPayorMapping);
                AngularAjaxCall($http, '/hc/referral/AddReferralPayorMapping', jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.ClearReferralPayorMappingDetails();
                        $scope.GetReferralPayorMappingList(true);
                    }
                });
            }
            else {
                toastr.error("Precedence is required");
            }
        }
    };
    $scope.ReferralModel.ReferralPayorMapping.IsPayorNotPrimaryInsured = true;
    $scope.ClearReferralPayorMappingDetails = function () {
        $scope.ReferralModel.ReferralPayorMapping.ReferralPayorMappingID = null;
        $scope.ReferralModel.ReferralPayorMapping.PayorID = null;
        $scope.ReferralModel.ReferralPayorMapping.PayorEffectiveDate = null;
        $scope.ReferralModel.ReferralPayorMapping.PayorEffectiveEndDate = null;
        $scope.ReferralModel.ReferralPayorMapping.Precedence = null;

        $scope.ReferralModel.ReferralPayorMapping.IsPayorNotPrimaryInsured = true;
        $scope.ReferralModel.ReferralPayorMapping.InsuredId = null;
        $scope.ReferralModel.ReferralPayorMapping.InsuredFirstName = null;
        $scope.ReferralModel.ReferralPayorMapping.InsuredMiddleName = null;
        $scope.ReferralModel.ReferralPayorMapping.InsuredLastName = null;
        $scope.ReferralModel.ReferralPayorMapping.InsuredAddress = null;
        $scope.ReferralModel.ReferralPayorMapping.InsuredCity = null;
        $scope.ReferralModel.ReferralPayorMapping.InsuredState = null;
        $scope.ReferralModel.ReferralPayorMapping.InsuredZipCode = null;
        $scope.ReferralModel.ReferralPayorMapping.InsuredPhone = null;
        $scope.ReferralModel.ReferralPayorMapping.InsuredPolicyGroupOrFecaNumber = null;
        $scope.ReferralModel.ReferralPayorMapping.InsuredDateOfBirth = null;
        $scope.ReferralModel.ReferralPayorMapping.InsuredGender = null;
        $scope.ReferralModel.ReferralPayorMapping.InsuredEmployersNameOrSchoolName = null;
        $scope.ReferralModel.ReferralPayorMapping.BeneficiaryTypeID = null;
        $scope.ReferralModel.ReferralPayorMapping.BeneficiaryNumber = null;
        $scope.ReferralModel.ReferralPayorMapping.MemberID = null;
        $scope.ReferralModel.ReferralPayorMapping.MasterJurisdictionID = null;
        $scope.ReferralModel.ReferralPayorMapping.MasterTimezoneID = null;
    };

    $scope.EditPayorCodeMapping = function (data) {
        var tempData = $.parseJSON(angular.toJson(data));
        $scope.ReferralModel.ReferralPayorMapping.ReferralPayorMappingID = tempData.ReferralPayorMappingID;
        $scope.ReferralModel.ReferralPayorMapping.PayorID = tempData.PayorID;
        $scope.ReferralModel.ReferralPayorMapping.PayorEffectiveDate = tempData.PayorEffectiveDate;
        $scope.ReferralModel.ReferralPayorMapping.PayorEffectiveEndDate = tempData.PayorEffectiveEndDate;
        $scope.ReferralModel.ReferralPayorMapping.Precedence = tempData.Precedence;
        $scope.ReferralModel.ReferralPayorMapping.IsDeleted = tempData.IsDeleted;

        $scope.ReferralModel.ReferralPayorMapping.IsPayorNotPrimaryInsured = tempData.IsPayorNotPrimaryInsured;
        $scope.ReferralModel.ReferralPayorMapping.InsuredId = tempData.InsuredId;
        $scope.ReferralModel.ReferralPayorMapping.InsuredFirstName = tempData.InsuredFirstName;
        $scope.ReferralModel.ReferralPayorMapping.InsuredMiddleName = tempData.InsuredMiddleName;
        $scope.ReferralModel.ReferralPayorMapping.InsuredLastName = tempData.InsuredLastName;
        $scope.ReferralModel.ReferralPayorMapping.InsuredAddress = tempData.InsuredAddress;
        $scope.ReferralModel.ReferralPayorMapping.InsuredCity = tempData.InsuredCity;
        $scope.ReferralModel.ReferralPayorMapping.InsuredState = tempData.InsuredState;
        $scope.ReferralModel.ReferralPayorMapping.InsuredZipCode = tempData.InsuredZipCode;
        $scope.ReferralModel.ReferralPayorMapping.InsuredPhone = tempData.InsuredPhone;
        $scope.ReferralModel.ReferralPayorMapping.InsuredPolicyGroupOrFecaNumber = tempData.InsuredPolicyGroupOrFecaNumber;
        $scope.ReferralModel.ReferralPayorMapping.InsuredDateOfBirth = tempData.InsuredDateOfBirth === '1900-01-01T00:00:00' ? null : tempData.InsuredDateOfBirth;
        $scope.ReferralModel.ReferralPayorMapping.InsuredGender = tempData.InsuredGender;
        $scope.ReferralModel.ReferralPayorMapping.InsuredEmployersNameOrSchoolName = tempData.InsuredEmployersNameOrSchoolName;
        $scope.ReferralModel.ReferralPayorMapping.BeneficiaryTypeID = tempData.BeneficiaryTypeID === '0' ? null : tempData.BeneficiaryTypeID;
        $scope.ReferralModel.ReferralPayorMapping.BeneficiaryNumber = tempData.BeneficiaryNumber;
        $scope.ReferralModel.ReferralPayorMapping.MemberID = tempData.MemberID;
        $scope.ReferralModel.ReferralPayorMapping.MasterJurisdictionID = tempData.MasterJurisdictionID;
        $scope.ReferralModel.ReferralPayorMapping.MasterTimezoneID = tempData.MasterTimezoneID;

        if ($scope.ReferralModel.ReferralPayorMapping.IsPayorNotPrimaryInsured === true && $scope.ReferralModel.ReferralPayorMapping.InsuredFirstName === '') {
            $scope.ReferralPayorMapping.InsuredFirstName = $scope.ReferralModel.Contact.FirstName;
            $scope.ReferralPayorMapping.InsuredLastName = $scope.ReferralModel.Contact.LastName;
            $scope.ReferralPayorMapping.InsuredAddress = $scope.ReferralModel.Contact.Address;
            $scope.ReferralPayorMapping.InsuredCity = $scope.ReferralModel.Contact.City;
            $scope.ReferralPayorMapping.InsuredState = $scope.ReferralModel.Contact.State;
            $scope.ReferralPayorMapping.InsuredZipCode = $scope.ReferralModel.Contact.ZipCode;
            $scope.ReferralPayorMapping.InsuredPhone = $scope.ReferralModel.Contact.Phone1;
            $scope.ReferralPayorMapping.InsuredDateOfBirth = $scope.ReferralModel.Referral.Dob;
            $scope.ReferralPayorMapping.InsuredGender = $scope.ReferralModel.Referral.Gender;
        }
        if (tempData.Precedence != 1) {
            $scope.Insured_Detail = true;
        }
        else {
            $scope.Insured_Detail = false;
        }
    };


   
    $scope.ReferralPayorMappingListPager.getDataCallback = $scope.GetReferralPayorMappingList;
    

    $("a#billings,a#billings_patientpayorsmapping").on('shown.bs.tab', function (e) {
        HideErrors('#FrmPatientsPayor');
        //$scope.ClearReferralPayorMappingDetails();
        $scope.CheckPrimaryInsured();
        $scope.ResetSearchFilter();
        $scope.ReferralPayorMappingListPager.getDataCallback();
        $scope.ClearReferralPayorMappingDetails();

        var dateformat = GetOrgDateFormat();
        $(".dateInputMask").attr("placeholder", dateformat);
    });

    
    
};
controllers.ReferralPatientsPayorController.$inject = ['$scope', '$http', '$window', '$timeout'];


$(document).ready(function () {
    debugger
    var dateformat = GetOrgDateFormat();
    $(".dateInputMask").attr("placeholder", dateformat);
    $('.time').inputmask({
        mask: "h:s t\\m",
        placeholder: "hh:mm a",
        alias: "datetime",
        hourFormat: "12"
    });
});
