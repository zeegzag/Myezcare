var model;

controllers.AddReferralController = function ($scope, $http, $window, $timeout, $filter) {
    //#region REFERRAL RELATED GLOBAL FUNCATION
    model = $scope;
    $scope.CurrentDate = SetExpiryDate();
    $scope.newInstance = function () {        
        return $.parseJSON($("#hdnAddReferralModel").val());
    };
    
    var modalJson = $.parseJSON($("#hdnAddReferralModel").val());

    $scope.ShowSubmitActions = true;
    $scope.DxCodeTokenInputObj = {};
    $scope.ReferralSiblingTokenInputObj = {};
    $scope.CaseManagerTokenObj = {};
    $scope.ReferralModel = modalJson;
    $scope.DosageTimes = $scope.ReferralModel.DosageTimes;
    $scope.DxCodeMappingList = $scope.ReferralModel.DxCodeMappingList;
    $scope.TempReferralModel = angular.copy($scope.ReferralModel);
    $scope.EncryptedReferralID = $scope.ReferralModel.Referral.EncryptedReferralID;
    $scope.EncryptedIDForZero = window.EncryptedIDForZero;
    $scope.PrimaryPlacementContactTypeID = PrimaryPlacementContactTypeID;
    $scope.LegalGuardianContactTypeID = LegalGuardianContactTypeID;
    $scope.NewDate = SetExpiryDate();
    $scope.DOBValidDate = moment().subtract(18, "years").format(window.DateFormat.toUpperCase());
    $scope.GetCaseManagersURL = SiteUrl.GetCaseManagersURL;
    $scope.ListPreference = $scope.ReferralModel.PreferenceList;
    $scope.SkillList = $scope.ReferralModel.SkillList;
    $scope.ReferralSkillList = $scope.ReferralModel.ReferralSkillList;
    $scope.CareTypeList = $scope.ReferralModel.CareTypeList;
    $scope.ServiceTypeList = $scope.ReferralModel.ServiceType;
    $scope.InternalDocumentList = $scope.ReferralModel.InternalDocumentList;
    $scope.ExternalDocumentList = $scope.ReferralModel.ExternalDocumentList;
    $scope.InternalDocumentSectionList = $scope.ReferralModel.InternalDocumentSectionList;
    $scope.ExternalDocumentSectionList = $scope.ReferralModel.ExternalDocumentSectionList;

    $scope.LastReferralStatusID = angular.copy($scope.ReferralModel.Referral.ReferralStatusID);

    $scope.SelectedGroups = [];

    $scope.SetGroups = function () {
        if ($scope.ReferralModel.Referral.GroupIDs != null) {
            $scope.SelectedGroups = $scope.ReferralModel.Referral.GroupIDs.split(",");
        }
    };
    $scope.SetGroups();

    $scope.LoadRefferalCMDetails = function () {
        $timeout(function () {
            if ($scope.ReferralModel.ReferralPhysicians.length > 0) {

                for (var i = 0; i < $scope.ReferralModel.ReferralPhysicians; i++) {
                    var currentItem = $scope.ReferralModel.ReferralPhysicians[i];
                    var physicianId = currentItem.PhysicianID;
                    var fname = currentItem.FirstName;
                    var mname = currentItem.MiddleName;
                    var lname = currentItem.LastName;
                    var email = currentItem.Email;
                    var phone = currentItem.Phone;
                    var address = currentItem.Address;
                    var npiNumber = currentItem.NPINumber;

                    $scope.PhysicianTokenInputObj.add({
                        PhysicianID: physicianId,
                        FirstName: fname,
                        MiddleName: mname,
                        LastName: lname,
                        NPINumber: npiNumber,
                        Email: email,
                        Phone: phone,
                        Address: address
                    });
                }
            } else if ($scope.ReferralModel.Referral.PhysicianID > 0) {

                var physicianId = $scope.ReferralModel.Referral.PhysicianID;
                var fname = $scope.ReferralModel.PhysicianModel.FirstName;
                var mname = $scope.ReferralModel.PhysicianModel.MiddleName;
                var lname = $scope.ReferralModel.PhysicianModel.LastName;
                var email = $scope.ReferralModel.PhysicianModel.Email;
                var phone = $scope.ReferralModel.PhysicianModel.Phone;
                var address = $scope.ReferralModel.PhysicianModel.Address;
                var npiNumber = $scope.ReferralModel.PhysicianModel.NPINumber;

                $scope.PhysicianTokenInputObj.add({
                    PhysicianID: physicianId,
                    FirstName: fname,
                    MiddleName: mname,
                    LastName: lname,
                    NPINumber: npiNumber,
                    Email: email,
                    Phone: phone,
                    Address: address
                });

                //$scope.ReferralModel.PhysicianModel.Email = email;
                //$scope.ReferralModel.PhysicianModel.Phone = phone;
                //$scope.ReferralModel.PhysicianModel.Address = address;
            }

            if ($scope.ReferralModel.Referral.CaseManagerID > 0) {
                var agencyId = $scope.ReferralModel.Referral.AgencyID;
                var caseManagerId = $scope.ReferralModel.Referral.CaseManagerID;
                var agencyName = $scope.ReferralModel.Referral.AgencyName;
                var cmName = $scope.ReferralModel.Referral.CaseManager;
                var email = $scope.ReferralModel.Referral.Email;
                var phone = $scope.ReferralModel.Referral.Phone;
                var fax = $scope.ReferralModel.Referral.Fax;

                if ($scope.CaseManagerTokenObj.clear) {
                    $scope.CaseManagerTokenObj.clear();
                }


                $scope.ReferralModel.Referral.AgencyID = agencyId;
                $scope.ReferralModel.Referral.CaseManagerID = caseManagerId;
                $scope.ReferralModel.Referral.AgencyName = agencyName;
                $scope.ReferralModel.Referral.CaseManager = cmName;

                if ($scope.CaseManagerTokenObj.add) {
                    $scope.CaseManagerTokenObj.add({ AgencyID: $scope.ReferralModel.Referral.AgencyID, CaseManagerID: $scope.ReferralModel.Referral.CaseManagerID, Name: $scope.ReferralModel.Referral.CaseManager, NickName: $scope.ReferralModel.Referral.AgencyName });
                }
                $scope.ReferralModel.CaseManager.Email = email;
                $scope.ReferralModel.CaseManager.Phone = phone;
                $scope.ReferralModel.CaseManager.Fax = fax;
            }
        });
    };
    $scope.LoadRefferalCMDetails();

    $scope.ActiveStatus = parseInt(window.ActiveStatus);

    $scope.SetDatePickerDate = function (modelDate, newDate) {
        var a;
        if (modelDate) {
            var dt = new Date(modelDate);
            dt > newDate ? a = newDate : a = dt;
        } else {
            a = newDate;
        }
        return moment(a).format('L');
    };


    $scope.saveAllergy = function () {
        if ($scope.ReferralAllergyModel.Id == undefined) {
            $scope.ReferralAllergyModel.Id = 0;
        }
        else {
            $scope.ReferralAllergyModel.Id = $scope.ReferralAllergyModel.Id;
        }
        $scope.ReferralAllergyModel.ReportedBy = $scope.selecteditem;
        $scope.ReferralAllergyModel.Status = $scope.ReferralAllergyModel.StatusID;
        $scope.ReferralAllergyModel.Patient = $scope.ReferralModel.Referral.EncryptedReferralID;
        var jsonData = angular.toJson($scope.ReferralAllergyModel);
        AngularAjaxCall($http, "/hc/referral/SaveReferralAllergy", jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $("#AddAllergyModal").modal('hide');
                $scope.GetReferralAllergyList();
                ShowMessage("Allergy Saved Successfully", "Success");
                $scope.ResetAllergy();
            }
            else {

                ShowMessages(response);
            }
        });
    }

    $scope.GetReferralAllergyList = function () {
        if ($scope.searchAllergy === '') {
            $scope.searchAllergy = '';
        }
        var jsonData = angular.toJson({ referralId: $scope.ReferralModel.Referral.EncryptedReferralID, allergy: $scope.searchAllergy, status: true });
        AngularAjaxCall($http, "/hc/referral/GetReferralAllergy", jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralAllergy = {};
                $scope.ReferralAllergy = response.Data;
            }

        });
    }

    $scope.ResetReferralAllergyList = function () {

        $scope.searchAllergy = '';
        $scope.GetReferralAllergyList();
        $('#txtsearch').val("");
        $("#txtsearch").focus();

    }

    $scope.ResetAllergy = function () {
        $scope.ReferralAllergyModel = {};
        $scope.selecteditem = "0";
    }

    $scope.DeleteAllergy = function (allery) {
        bootboxDialog(function (result) {
            if (result) {
                $scope.AllergyId = allery.Id;
                var jsonData = angular.toJson({ AllergyId: $scope.AllergyId });

                AngularAjaxCall($http, "/hc/referral/DeleteAllergy", jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.GetReferralAllergyList();
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, 'Are you sure you want to delete this allergy?', bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    }

    $scope.TitleList = [];

    $scope.GetAlergyTtitle = function () {
        AngularAjaxCall($http, "/hc/referral/GetAllergyTitle", "", "Get").success(function (response) {
            //ShowMessages(response);
            $scope.TitleList = response.Data;

        });
    };

    $scope.ServiceType = [];

    $scope.SetServiceType = function () {
        if ($scope.ReferralModel.Referral.ServiceType != null) {
            $scope.ReferralModel.Referral.ServiceType = $scope.ReferralModel.Referral.ServiceType.split(",");
        }
    };

    $scope.SetServiceType();

    $("a#addReferralDetails_Allergy").on('shown.bs.tab', function (e) {
        $scope.GetReferralAllergyList();
        $scope.ReportedList();
        $scope.GetAlergyTtitle();
    });

    $scope.ReportedList = function () {
        $scope.referralId = $scope.ReferralModel.Referral.EncryptedReferralID;
        var jsonData = angular.toJson({ referralId: $scope.referralId });
        AngularAjaxCall($http, "/hc/referral/GetReportedBy", jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ReportedBy = response.Data;
                $scope.selecteditem = "0";
            }

        });
    }

    $scope.UpdateAllergy = function (item) {
        $scope.ReferralAllergyModel = item;
        $scope.selecteditem = item.ReportedBy;
        $scope.ReferralAllergyModel.StatusID = item.StatusID;
        //$scope.ReferralAllergyModel.ReportedBy = item.ReportedBy;
    }

    $scope.detailView = function (index) {
        $scope.rowNumber = index
    }

    $scope.ContactID = null;
    $scope.IsContactEditMode = false;
    $scope.CanAddContact = true;
    $scope.TempContactTypeID = 0;
    $scope.TempIndexNumberofContactList = 0;
    $scope.AddReferralURL = "/hc/referral/addreferral";
    $scope.ReferralDxCodeMappingURL = "/hc/referral/ReferralDxCodeMapping";
    $scope.SetAddReferralPageURL = "/hc/referral/setaddreferralpage";
    $scope.AddContactURL = "/hc/referral/addcontact";
    $scope.GetContactListURL = "/hc/referral/getcontactlist";
    $scope.DeleteContactURL = "/hc/referral/deletecontact";
    $scope.DeleteReferralPayorMappingURL = "/hc/referral/deletereferralpayormapping";
    $scope.GetAgencyLocationListURL = "/casemanager/getagencylocation";
    $scope.GetCaseManagerListURL = "/casemanager/getcasemanagers";
    $scope.GetCaseManagerDetail = "/casemanager/getcasemanagerdetail";
    $scope.ReferralModel.ReferralPayorMapping.TempPayorID = $scope.ReferralModel.ReferralPayorMapping.PayorID;
    $scope.ContactInfoList = [];
    $scope.ListofFields = "";

    //#endregion

    //#region ON LOAD FUNCTION START

    $scope.SetAddReferralPage = function () {
        var jsonData = angular.toJson({ id: $scope.EncryptedReferralID });
        AngularAjaxCall($http, $scope.SetAddReferralPageURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralModel = null;
                $scope.ReferralModel = response.Data;
                $scope.SetGroups();
                $scope.InternalDocumentList = $scope.ReferralModel.InternalDocumentList;
                $scope.ExternalDocumentList = $scope.ReferralModel.ExternalDocumentList;
                //$scope.ReferralModel.Referral.Dob = ParseJsonDate($scope.ReferralModel.Referral.Dob);

                //var originalAgencyLocationId = $scope.ReferralModel.Referral.AgencyLocationID;
                //$scope.ReferralModel.Referral.AgencyLocationID = 0;

                //var originalAgencyId = $scope.ReferralModel.Referral.AgencyID;
                //$scope.ReferralModel.Referral.AgencyID = 0;

                //$timeout(function () {
                //    $scope.ReferralModel.Referral.AgencyLocationID = originalAgencyLocationId;
                //    $scope.ReferralModel.Referral.AgencyID = originalAgencyId;
                //    $scope.ReferralModel.CaseManager.Email = $scope.ReferralModel.Referral.Email;
                //    $scope.ReferralModel.CaseManager.Phone = $scope.ReferralModel.Referral.Phone;
                //    $scope.ReferralModel.CaseManager.Fax = $scope.ReferralModel.Referral.Fax;
                //    $scope.$apply();
                //});
                $scope.LoadRefferalCMDetails();
            }
        });
    };

    $scope.SelectedCareType = [];
    $scope.CareTypeSettings = {
        smartButtonMaxItems: 1
    };
    if ($scope.ReferralModel.Referral.CareTypeIds != null) {
        var careTypeValues = [];
        careTypeValues = $scope.ReferralModel.Referral.CareTypeIds.split(",");
        angular.forEach(careTypeValues, function (value, key) {
            $scope.SelectedCareType.push({ "Value": value });
        });
    }

    $scope.validationCareType = function () {
        if ($scope.SelectedCareType.length > 0) {
            $(".caretypedropdown").removeClass("input-validation-error").addClass("valid");
            return true;
        }
        else {
            $(".caretypedropdown").removeClass("valid");
            $(".caretypedropdown").attr("data-original-title", "Service is required").attr("data-html", "true")
                .addClass("tooltip-danger input-validation-error")
                .tooltip({ html: true });
            return false;
        }
    }

    $("a#addReferralDetails").on('shown.bs.tab', function (e, ui) {
        $("#frmReferral").data('changed', false);
        $scope.SetAddReferralPage();
    });

    //#endregion 


    //prevent Enter on Modal 
    $('#model_AddContact').on('keypress', function (e) {
        if (e.keyCode === 13) {
            e.preventDefault();
        }
    });
    $('#model_AddMedication').on('keypress', function (e) {
        if (e.keyCode === 13) {
            e.preventDefault();
        }
    });

    //ssn region 

    $scope.addSSNLog = function () {

        var jsonData = angular.toJson({
            Referral: $scope.ReferralModel.Referral
        });
        AngularAjaxCall($http, HomeCareSiteUrl.ReferralAddSSNLog, jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                //ShowMessages(response);
                if (response.IsSuccess) {
                    //
                } else {
                }

            });

    }

    $scope.showSSN = function () {

        if ($scope.isEyeOpen) {
            $scope.canSeeSSN = false;
            $scope.canSeeLastFourDigit = false;
            $scope.isEyeOpen = false;
        } else {
            var canSeeSSN = $('#hdCanSeeSSN').val();
            var canSeeLastFourDigit = $('#hdCanSeeLastFourDigit').val();
            if (canSeeSSN === "True") {
                $scope.canSeeSSN = true;
                $scope.canSeeLastFourDigit = false;
                $scope.isEyeOpen = true;
                $scope.addSSNLog();
            } else if (canSeeLastFourDigit === "True") {
                $scope.canSeeSSN = false;
                $scope.canSeeLastFourDigit = true;
                $scope.isEyeOpen = true;
                $scope.addSSNLog();
            } else {
                $scope.canSeeSSN = false;
                $scope.canSeeLastFourDigit = false;
            }
        }

    }
    //end ssn region

    //#region ON SAVE REFERRAL RELATED FUNCATION START

    $scope.$watch('ReferralModel.Referral.ReferralStatusID', function (newValue, oldValue) {
        $scope.ReferralStatusChange(newValue, oldValue);
    });

    //$scope.$watch('ReferralModel.ReferralBeneficiaryTypeDetail.BeneficiaryType', function () {
    //    var element = document.getElementById("BeneficiaryType");
    //    $scope.BeneficiaryName = element.options[element.selectedIndex].text;
    //});

    $scope.$watch('ReferralModel.ReferralBeneficiaryTypeDetail.BeneficiaryTypeID', function () {
        var element = document.getElementById("BeneficiaryTypeID");
        if ($("#BeneficiaryTypeID").val() != "") {
            $scope.BeneficiaryTypeName = element.options[element.selectedIndex].text;
        }
    });

    $scope.ReferralStatusChange = function (statusId) {
        if (statusId == window.RefStatus_ConnectingFamilies)
            $scope.ReferralModel.DXCodeCount = 1;
        else {

            $scope.ReferralModel.DXCodeCount = $scope.ReferralModel.DxCodeMappingList.filter(function (item) {
                return item.IsDeleted == false;
            }).length;

        }
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };

    $scope.GetAgeInYears = function () {
        if ($scope.ReferralModel.Referral.Dob != null && $scope.ReferralModel.Referral.Dob != '') {
            var diffdate = new Date() - new Date($scope.ReferralModel.Referral.Dob);
            var numYears = diffdate / 31536000000;
            return Math.floor(numYears);
        }
    };

    $scope.calculateAge = function (selecteddate) { // pass in player.dateOfBirth

        if (selecteddate != null && selecteddate != '') {

            //Get 1 day in milliseconds
            // var one_day = 1000 * 60 * 60 * 24;
            // 86400000 milli second = 1 Day
            // 31536000000 milli second = 1 Year
            // 2628000000 milli second = 1 month

            var diff_date = new Date() - new Date(selecteddate);

            var num_years = diff_date / 31536000000;
            var num_months = (diff_date % 31536000000) / 2628000000;
            var num_days = ((diff_date % 31536000000) % 2628000000) / 86400000;

            return Math.floor(num_years) + 'Y ' + Math.floor(num_months) + 'M '; // + Math.floor(num_days) + 'D';
        }
        return "";
    };

    $scope.$watch('ReferralModel.AddAndListContactInformation.ContactTypeID', function (newValue, oldValue) {

        if (parseInt($scope.ReferralModel.AddAndListContactInformation.ContactTypeID) == parseInt(window.PrimaryPlacementContactTypeID)) {
            $scope.ReferralModel.AddAndListContactInformation.IsDCSLegalGuardian = 'false';
            $scope.ReferralModel.AddAndListContactInformation.IsNoticeProviderOnFile = 'false';
        }

        if (parseInt($scope.ReferralModel.AddAndListContactInformation.ContactTypeID) == parseInt(window.LegalGuardianContactTypeID)) {
            $scope.ReferralModel.AddAndListContactInformation.IsPrimaryPlacementLegalGuardian = 'false';
        }

        if (parseInt($scope.ReferralModel.AddAndListContactInformation.ContactTypeID) != parseInt(window.PrimaryPlacementContactTypeID) && parseInt($scope.ReferralModel.AddAndListContactInformation.ContactTypeID) != parseInt(window.LegalGuardianContactTypeID)) {
            $scope.ReferralModel.AddAndListContactInformation.IsDCSLegalGuardian = 'false';
            $scope.ReferralModel.AddAndListContactInformation.IsNoticeProviderOnFile = 'false';
            $scope.ReferralModel.AddAndListContactInformation.IsPrimaryPlacementLegalGuardian = 'false';
        }

    });

    $scope.$watchGroup(['ReferralModel.Referral.RespiteService', 'ReferralModel.Referral.LifeSkillsService', 'ReferralModel.Referral.CounselingService', 'ReferralModel.Referral.ConnectingFamiliesService'], function (newValue, oldValue) {
        if (!(JSON.parse($scope.ReferralModel.Referral.RespiteService) || JSON.parse($scope.ReferralModel.Referral.LifeSkillsService) || JSON.parse($scope.ReferralModel.Referral.CounselingService)
            || JSON.parse($scope.ReferralModel.Referral.ConnectingFamiliesService))) {
            $scope.ZSPServiceErrorCount = "";
        } else {
            $scope.ZSPServiceErrorCount = "Updated";
        }
    });

    $scope.$watchCollection('ReferralModel.ContactInformationList', function (newValue, oldValue) {
        $scope.MissingPC = true; $scope.MissingLG = true; $scope.ContactServiceErrorCount = "";
        angular.forEach($scope.ReferralModel.ContactInformationList, function (item, key) {
            if (parseInt(window.PrimaryPlacementContactTypeID) == parseInt(item.ContactTypeID)) {
                $scope.MissingPC = false;
            }
            if (parseInt(window.LegalGuardianContactTypeID) == parseInt(item.ContactTypeID)) {
                $scope.MissingLG = false;
            }
        });

        if ($scope.MissingPC == false)
            $scope.ContactServiceErrorCount = "Updated";
        else
            $scope.ContactServiceErrorCount = "";
    });

    $scope.CheckValidationForSaveDraft = function () {

        var liString = "<li>{0} : {1}</li>";
        var isValid = true;
        $scope.ListofFields = "<ul>";
        if ($scope.ReferralModel.Referral.FirstName == null || $scope.ReferralModel.Referral.FirstName == undefined || $scope.ReferralModel.Referral.FirstName == '') {
            $scope.ListofFields += liString.format(window.Missing, window.FirstName);
            isValid = false;
        }
        if ($scope.ReferralModel.Referral.LastName == null || $scope.ReferralModel.Referral.LastName == undefined || $scope.ReferralModel.Referral.LastName == '') {
            // $scope.ListofFields = $scope.ListofFields.length > 0 ? $scope.ListofFields + ", Last Name" : "Last Name";
            $scope.ListofFields += liString.format(window.Missing, window.LastName);
            isValid = false;
        }
        if ($scope.ReferralModel.Referral.AHCCCSID == null || $scope.ReferralModel.Referral.AHCCCSID == undefined || $scope.ReferralModel.Referral.AHCCCSID == '') {
            //$scope.ListofFields = $scope.ListofFields.length > 0 ? $scope.ListofFields + ", AHCCCS ID" : "AHCCCS ID";

            $scope.ListofFields += liString.format(window.Missing, window.Account);
            isValid = false;
        } else {
            //var regexAhccccid = /^[a-zA-Z]{1}[0-9]{1,9}$/;
            //if (!regexAhccccid.test($scope.ReferralModel.Referral.AHCCCSID)) {
            //    $scope.ListofFields += liString.format(window.Invalid, window.AHCCCSID);
            //    isValid = false;
            //}
        }
        //if ($scope.ReferralModel.Referral.CISNumber == null || $scope.ReferralModel.Referral.CISNumber == undefined || $scope.ReferralModel.Referral.CISNumber == '') {
        //    //$scope.ListofFields = $scope.ListofFields.length > 0 ? $scope.ListofFields + ", CIS Number" : "CIS Number";
        //    $scope.ListofFields += liString.format(window.Missing, window.CISNumberLabel);
        //    isValid = false;
        //} else {
        //    var regexCisNumber = /^\d{10}$/;
        //    if (!regexCisNumber.test($scope.ReferralModel.Referral.CISNumber)) {
        //        $scope.ListofFields += liString.format(window.Invalid, window.CISNumberLabel);
        //        isValid = false;
        //    }
        //}
        if ($scope.ReferralModel.ReferralPayorMapping.PayorID > 0 && !$scope.ReferralModel.ReferralPayorMapping.PayorEffectiveDate) {
            //$scope.ListofFields = $scope.ListofFields.length > 0 ? $scope.ListofFields + ", Date of Birth" : "Date of Birth";
            $scope.ListofFields += liString.format(window.Missing, window.PayorEffectiveDate);
            isValid = false;
        }
        if ($scope.ReferralModel.Referral.Dob == null || $scope.ReferralModel.Referral.Dob == undefined || $scope.ReferralModel.Referral.Dob == '') {
            //$scope.ListofFields = $scope.ListofFields.length > 0 ? $scope.ListofFields + ", Date of Birth" : "Date of Birth";
            $scope.ListofFields += liString.format(window.Missing, window.DOB);
            isValid = false;
        }
        if ($scope.ReferralModel.Referral.Gender == null || $scope.ReferralModel.Referral.Gender == undefined || $scope.ReferralModel.Referral.Gender == '') {
            //$scope.ListofFields = $scope.ListofFields.length > 0 ? $scope.ListofFields + ", Gender" : "Gender";
            $scope.ListofFields += liString.format(window.Missing, window.Gender);
            isValid = false;
        }

        if ($scope.ReferralModel.Referral.ReferralStatusID == null || $scope.ReferralModel.Referral.ReferralStatusID == undefined || $scope.ReferralModel.Referral.ReferralStatusID == '') {
            //$scope.ListofFields = $scope.ListofFields.length > 0 ? $scope.ListofFields + ", Gender" : "Gender";
            $scope.ListofFields += liString.format(window.Missing, window.Status);
            isValid = false;
        }
        if ($scope.ReferralModel.Referral.Assignee == null || $scope.ReferralModel.Referral.Assignee == undefined || $scope.ReferralModel.Referral.Assignee == '') {
            //$scope.ListofFields = $scope.ListofFields.length > 0 ? $scope.ListofFields + ", Gender" : "Gender";
            $scope.ListofFields += liString.format(window.Missing, window.Assignee);
            isValid = false;
        }
        return isValid;
    };

    $scope.CheckActiveStatusForSaveDraft = function (status) {
        var isValid = true;
        if (status == parseInt(window.ActiveStatus)) {
            bootboxDialog(function () {
            }, bootboxDialogType.Alert, window.Alert, window.ActiveStatusNotAllowedForSaveAsDraft);
            isValid = false;
        }
        return isValid;
    };

    $scope.CheckClientServiceAndContacts = function () {


        //CHECK ZSP SERVICE EXIST OR NOT

        if (!(JSON.parse($scope.ReferralModel.Referral.RespiteService) || JSON.parse($scope.ReferralModel.Referral.LifeSkillsService) || JSON.parse($scope.ReferralModel.Referral.CounselingService)
            || JSON.parse($scope.ReferralModel.Referral.ConnectingFamiliesService))) {
            $scope.ZSPServiceErrorCount = "";
        } else {
            $scope.ZSPServiceErrorCount = "Updated";
        }

        //CHECK CLIENT REQUIRED CONTACT DETAILS ARE EXIST OR NOT
        $scope.MissingPC = true; $scope.MissingLG = true; $scope.ContactServiceErrorCount = "";
        angular.forEach($scope.ReferralModel.ContactInformationList, function (item, key) {
            if (parseInt(window.PrimaryPlacementContactTypeID) == parseInt(item.ContactTypeID)) {
                $scope.MissingPC = false;
            }
            if (parseInt(window.LegalGuardianContactTypeID) == parseInt(item.ContactTypeID)) {
                $scope.MissingLG = false;
            }
        });

        if ($scope.MissingPC == false && $scope.MissingLG == false)
            $scope.ContactServiceErrorCount = "Updated";
        else
            $scope.ContactServiceErrorCount = "";
        //CheckErrors("#frmReferral");

    };

    $scope.Save = function (isSaveAsDraft) {
        if (isSaveAsDraft) {
            // This block will execute when the user has selected to created a referral as a Draft.
            var validStatus = $scope.CheckActiveStatusForSaveDraft($scope.ReferralModel.Referral.ReferralStatusID);
            if (validStatus) {
                var isValid = $scope.CheckValidationForSaveDraft();
                if (isValid && isValidCareType) {
                    $scope.ReferralModel.Referral.IsSaveAsDraft = true;
                    $scope.SaveReferral(true);
                } else {
                    bootboxDialog(function () {
                    }, bootboxDialogType.Alert, window.Alert, window.DraftIncomplete.format($scope.ListofFields));
                }
            }
        } else {
            //var frmAddBXContractView = "#frmAddBXContractView :input,#frmAddBXContractView textarea,#frmAddBXContractView select";
            //var frmRefSuspention = "#frmRefSuspention :input,#frmRefSuspention textarea,#frmRefSuspention select";

            var frmAddBlockEmpView = "#frmAddBlockEmpView :input,#frmAddBlockEmpView textarea,#frmAddBlockEmpView select";

            $(frmAddBlockEmpView).each(function () {
                $(this).addClass("ignore-element");
            });

            var frmEmpSmsView = "#emp-sms :input,#emp-sms textarea,#emp-sms select";
            $(frmEmpSmsView).each(function () {
                $(this).addClass("ignore-element");
            });

            var isValid = false;
            try {
                isValid = CheckErrors($("#frmReferral"));
            }
            catch { }
            isValidCareType = $scope.validationCareType();
            if (isValidCareType && isValid) {
                $scope.SaveReferral(isSaveAsDraft);
            }
            else {
                toastr.error(window.CanNotSave);
            }

            //if (CheckErrors("#frmReferral", true)) {


            //    //if ($scope.LastReferralStatusID !== $scope.ReferralModel.Referral.ReferralStatusID && $scope.ReferralModel.Referral.ReferralStatusID === parseInt(window.Inactive)) {
            //    //    bootboxDialog(function (result) {
            //    //        $scope.SaveReferral(isSaveAsDraft, result);
            //    //    }, bootboxDialogType.Confirm, window.Confirm, window.SendNotificationToCM, window.YesContinue, 'btn btn-primary actionTrue', '', '', window.NoContinue);
            //    //}
            //    //else if ($scope.LastReferralStatusID !== $scope.ReferralModel.Referral.ReferralStatusID && $scope.ReferralModel.Referral.ReferralStatusID === parseInt(window.ReferralAccepted)) {
            //    //    //$scope.SaveReferral(isSaveAsDraft, true);
            //    //    bootboxDialog(function (result) {
            //    //        $scope.SaveReferral(isSaveAsDraft, result);
            //    //    }, bootboxDialogType.Confirm, window.Confirm, window.SendNotificationToCMReferralAccepted, window.YesContinue, 'btn btn-primary actionTrue', '', '', window.NoContinue);
            //    //}
            //    //else {
            //    //    $scope.SaveReferral(isSaveAsDraft);
            //    //}

            //    $scope.SaveReferral(isSaveAsDraft);

            //} else {
            //    toastr.error(window.CanNotSave);
            //}
        }

    };

    $scope.SaveReferral = function (isSaveAsDraft, notifyCm) {
        $("#frmReferral").data('changed', false);
        var isHavePrimaryDxCode = $scope.CheckPrimaryDxCode();
        if (isHavePrimaryDxCode) {

            $scope.ReferralModel.Referral.IsSaveAsDraft = isSaveAsDraft;
            if ($scope.SelectedCareType) {
                var careTypeValues = $scope.SelectedCareType;
                $scope.ReferralModel.Referral.CareTypeIds = Object.keys(careTypeValues).map(function (k) { return careTypeValues[k]["Value"] }).join(",");
            }
            else {
                $scope.ReferralModel.Referral.CareTypeIds = null;
            }

            //$scope.ReferralModel.Referral.CareTypeIds = ($scope.SelectedCareType) ? $scope.SelectedCareType.toString() : null;

            $scope.DocumentList = [];
            angular.forEach($scope.InternalDocumentList, function (item) {
                $scope.DocumentList.push(item);
            });
            angular.forEach($scope.ExternalDocumentList, function (item) {
                $scope.DocumentList.push(item);
            });

            $scope.ReferralModel.Referral.ServiceType = $scope.ReferralModel.Referral.ServiceType ? $scope.ReferralModel.Referral.ServiceType.toString() : "";
            $scope.ReferralModel.Referral.GroupIDs = $scope.SelectedGroups ? $scope.SelectedGroups.join(",") : "";
            $scope.ReferralModel.Referral.BMI = document.getElementById("bmi-results").value;

            if ($scope.ReferralModel.Referral.Dob == "" || $scope.ReferralModel.Referral.Dob == null) {
                // "Akhil"
                $('#Referral_Dob').addClass('tooltip-danger');
            }
            else {

                var jsonData = angular.toJson({
                    NotifyCmForInactiveStatus: notifyCm,
                    Referral: $scope.ReferralModel.Referral,
                    ContactInformationList: $scope.ReferralModel.ContactInformationList,
                    // DxCodeMappingList: $scope.ReferralModel.DxCodeMappingList,
                    ReferralPhysicians: $scope.ReferralModel.ReferralPhysicians,
                    ReferralBeneficiaryTypes: $scope.ReferralModel.ReferralBeneficiaryTypes,
                    ReferralPayorMapping: $scope.ReferralModel.ReferralPayorMapping,
                    ReferralSiblingMappingList: $scope.ReferralModel.ReferralSiblingMappingList,
                    PreferenceList: $scope.ListPreference,
                    StrReferralSkillList: $scope.ReferralSkillList ? $scope.ReferralSkillList.toString() : "",
                    DocumentList: $scope.DocumentList
                });
                AngularAjaxCall($http, $scope.AddReferralURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $('#Referral_Dob').removeClass('tooltip-danger');
                        SetMessageForPageLoad(response.Message, "ShowAddReferralMessage");
                        if (!response.Data.Referral.IsEditMode) {
                            if (ValideElement(window.frameElement)) {
                                //window.location.href = HomeCareSiteUrl.PartialAddReferralURL + response.Data.Referral.EncryptedReferralID;
                            } else {
                                //window.location.href = HomeCareSiteUrl.AddReferralPageUrl;/* + response.Data.Referral.EncryptedReferralID;*/
                            }
                            $scope.EncryptedReferralID = response.Data.Referral.EncryptedReferralID;
                        } else {
                            //location.reload();
                        }
                    } else
                        ShowMessages(response);
                });
            }
        }
        else {
            toastr.error(window.PrimaryDXCodeValidation);
        }

    };

    //#region Compliance Information
    $scope.SaveReferralCompliance = function () {

        $scope.DocumentList = [];
        angular.forEach($scope.InternalDocumentList, function (item) {
            $scope.DocumentList.push(item);
        });
        angular.forEach($scope.ExternalDocumentList, function (item) {
            $scope.DocumentList.push(item);
        });
        var jsonData = angular.toJson({
            DocumentList: $scope.DocumentList,
            ReferralID: $scope.ReferralModel.Referral.ReferralID
        });

        AngularAjaxCall($http, HomeCareSiteUrl.SaveReferralComplianceURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                ShowMessages(response);
            } else
                ShowMessages(response);
        });
    }
    //#endregion Compliance Information

    $scope.Cancel = function () {
        window.location.reload();
    };

    //#endregion

    //#region Schedule Request Related Changes

    //$scope.$watch('ReferralModel.Referral.ScheduleRequestDates', function (newValue, oldValue) {

    //    $scope.ReferralModel.Referral.ScheduleRequestDateErrorCount = 0;

    //    if (newValue != undefined && newValue.length > 0) {
    //        $.each(newValue.split(","), function (index, data) {
    //            var datePair = data.trim().split("-");
    //            if (datePair.length === 2) {
    //                $.each(datePair, function (index1, data1) {
    //                    if (!moment(data1.trim(), ["MM/DD/YY", "MM/DD/YYYY"], true).isValid())
    //                        $scope.ReferralModel.Referral.ScheduleRequestDateErrorCount = 1;
    //                });
    //            } else
    //                $scope.ReferralModel.Referral.ScheduleRequestDateErrorCount = 1;
    //        });

    //    } else
    //        $scope.ReferralModel.Referral.ScheduleRequestDateErrorCount = 0;


    //});

    //#endregion

    //#region AGENCY CHANGE RELATED FUNCATION START

    $scope.AgencyFirstTimeLoad = true;
    //$scope.$watch('ReferralModel.Referral.AgencyID', function (newValue, oldValue) {
    //    $timeout(function () {
    //        $scope.AgencyFirstTimeLoad = false;
    //    });
    //    if (newValue !== oldValue) {
    //        $scope.ReferralModel.Referral.AgencyLocationID = 0;
    //    }
    //    if (newValue === $scope.TempReferralModel.Referral.AgencyID) {
    //        $scope.ReferralModel.Referral.AgencyLocationID = $scope.TempReferralModel.Referral.AgencyLocationID;
    //    }

    //});

    $scope.AgencyLocationFirstTimeLoad = true;
    //$scope.$watch('ReferralModel.Referral.AgencyLocationID', function (newValue, oldValue) {
    //    var caseManagerId = $scope.ReferralModel.Referral.CaseManagerID;
    //    if (newValue == null || newValue == '') {
    //        $timeout(function () {
    //            $scope.ReferralModel.CaseManagerList = null;
    //            // $scope.ReferralModel.Referral.CaseManagerID = '';
    //        });
    //    }

    //    if (newValue !== oldValue) {
    //        $scope.ReferralModel.Referral.CaseManagerID = 0;
    //    }
    //    if (newValue === $scope.TempReferralModel.Referral.AgencyLocationID) {
    //        $scope.ReferralModel.Referral.CaseManagerID = $scope.TempReferralModel.Referral.CaseManagerID;
    //    }

    //    if (parseInt(newValue) > 0) {
    //        var jsonData = angular.toJson({ agencyLocationID: parseInt(newValue), CaseManagerID: caseManagerId });
    //        AngularAjaxCall($http, $scope.GetCaseManagerListURL, jsonData, "Post", "json", "application/json", false).
    //            success(function (response, status, headers, config) {
    //                $timeout(function () {
    //                    $scope.ReferralModel.CaseManagerList = [];
    //                    $scope.ReferralModel.CaseManagerList = response.Data;

    //                    angular.forEach($scope.ReferralModel.CaseManagerList, function (item) {
    //                        if (item.CaseManagerID == $scope.ReferralModel.Referral.CaseManagerID) {
    //                            $scope.ReferralModel.CaseManager.Email = item.Email;
    //                            $scope.ReferralModel.CaseManager.Phone = item.Phone;
    //                            $scope.ReferralModel.CaseManager.Fax = item.Fax;
    //                        }
    //                    });
    //                });
    //            });
    //    }

    //});

    //$scope.$watch('ReferralModel.Referral.AgencyID', function (newValue, oldValue) {
    //    var caseManagerId = $scope.ReferralModel.Referral.CaseManagerID;
    //    $timeout(function () {
    //        $scope.AgencyFirstTimeLoad = false;
    //    });

    //    if (newValue == null || newValue == '') {
    //        $timeout(function () {
    //            $scope.ReferralModel.CaseManagerList = null;
    //            // $scope.ReferralModel.Referral.CaseManagerID = '';
    //        });
    //    }

    //    //if (newValue !== oldValue) {
    //    //    $scope.ReferralModel.Referral.CaseManagerID = 0;
    //    //    alert("Sdfsdf");
    //    //}
    //    if (newValue === $scope.TempReferralModel.Referral.AgencyID) {
    //        $scope.ReferralModel.Referral.CaseManagerID = $scope.TempReferralModel.Referral.CaseManagerID;
    //    }

    //    if (parseInt(newValue) > 0) {
    //        var jsonData = angular.toJson({ agencyID: parseInt(newValue), CaseManagerID: caseManagerId });
    //        AngularAjaxCall($http, $scope.GetCaseManagerListURL, jsonData, "Post", "json", "application/json", false).
    //            success(function (response, status, headers, config) {
    //                //$timeout(function () {
    //                $scope.ReferralModel.CaseManagerList = [];
    //                $scope.ReferralModel.CaseManagerList = response.Data;
    //                angular.forEach($scope.ReferralModel.CaseManagerList, function (item) {
    //                    if (item.CaseManagerID == $scope.ReferralModel.Referral.CaseManagerID) {
    //                        $scope.ReferralModel.CaseManager.Email = item.Email;
    //                        $scope.ReferralModel.CaseManager.Phone = item.Phone;
    //                        $scope.ReferralModel.CaseManager.Fax = item.Fax;
    //                    }
    //                });
    //                //});
    //            });
    //    }
    //});

    $scope.RefreshCaseManager = function () {
        var caseManagerId = $scope.ReferralModel.Referral.CaseManagerID;
        var agencyID = $scope.ReferralModel.Referral.AgencyID;
        if (parseInt(agencyID) > 0) {
            var jsonData = angular.toJson({ agencyID: parseInt(agencyID), CaseManagerID: caseManagerId });
            AngularAjaxCall($http, $scope.GetCaseManagerListURL, jsonData, "Post", "json", "application/json", false).
                success(function (response) {
                    $timeout(function () {
                        $scope.ReferralModel.CaseManagerList = [];
                        $scope.ReferralModel.CaseManagerList = response.Data;

                        angular.forEach($scope.ReferralModel.CaseManagerList, function (item) {
                            if (item.CaseManagerID == $scope.ReferralModel.Referral.CaseManagerID) {
                                $scope.ReferralModel.CaseManager.Email = item.Email;
                                $scope.ReferralModel.CaseManager.Phone = item.Phone;
                                $scope.ReferralModel.CaseManager.Fax = item.Fax;
                            }
                        });
                    });
                });
        }
    };

    $scope.$watch('ReferralModel.Referral.CaseManagerID', function (newValue, oldValue) {
        $timeout(function () {
            if (parseInt(newValue) > 0) {
                //$scope.ReferralModel.CaseManager.Email = '';
                //$scope.ReferralModel.CaseManager.Phone = '';
                //$scope.ReferralModel.CaseManager.Fax = '';
                angular.forEach($scope.ReferralModel.CaseManagerList, function (item) {
                    if (item.CaseManagerID == $scope.ReferralModel.Referral.CaseManagerID) {
                        $scope.ReferralModel.CaseManager.Email = item.Email;
                        $scope.ReferralModel.CaseManager.Phone = item.Phone;
                        $scope.ReferralModel.CaseManager.Fax = item.Fax;
                    }
                });
            }
        }, 500);

    });

    //#endregion

    //#region ADD/UPDATE NEW CONTACT RELATED FUNCATION START
    $scope.SetDefaultValuesForAddContact = function () {



        $scope.ReferralModel.AddAndListContactInformation.State = window.defaultStateCode;
        $scope.ReferralModel.AddAndListContactInformation.ROIExpireDate = null;
        $scope.ReferralModel.AddAndListContactInformation.IsPrimaryPlacementLegalGuardian = 'false';
        $scope.ReferralModel.AddAndListContactInformation.IsDCSLegalGuardian = 'false';
        $scope.ReferralModel.AddAndListContactInformation.IsNoticeProviderOnFile = 'false';
        $scope.ReferralModel.AddAndListContactInformation.IsEmergencyContact = 'false';



        //$scope.ReferralModel.AddAndListContactInformation.FirstName = 'Jitendra';
        //$scope.ReferralModel.AddAndListContactInformation.LastName = 'Yadav';
        //$scope.ReferralModel.AddAndListContactInformation.Phone1 = '9537760878';
        //$scope.ReferralModel.AddAndListContactInformation.Address = 'Address';
        //$scope.ReferralModel.AddAndListContactInformation.City = 'Tucson';
        //$scope.ReferralModel.AddAndListContactInformation.ZipCode = '382424';
        //$scope.ReferralModel.AddAndListContactInformation.LanguageID = 1;
    };

    $scope.OpenAddContactModal = function () {
        $timeout(function () {

            $scope.TempContactTypeID = 0;
            $scope.DisableContactType = false;
            HideErrors($("#frmAddContact"));
            $(".postzipcode").val('');

            $scope.ReferralModel.AddAndListContactInformation = $scope.newInstance().AddAndListContactInformation;
            $scope.SetDefaultValuesForAddContact();
            //$("#SearchContactToken").tokenInput("clear");

        });

    };

    $scope.$watch('ReferralModel.AddAndListContactInformation.ContactTypeID', function (newValue, oldValue) {
        if ($scope.ReferralModel.AddAndListContactInformation.ContactMappingID === 0) {
            if (newValue == window.PrimaryPlacementContactTypeID) {

                $scope.ReferralModel.AddAndListContactInformation.FirstName = $scope.ReferralModel.Referral.FirstName;
                $scope.ReferralModel.AddAndListContactInformation.LastName = $scope.ReferralModel.Referral.LastName;

            } else {
                $scope.ReferralModel.AddAndListContactInformation.FirstName = null;
                $scope.ReferralModel.AddAndListContactInformation.LastName = null;

            }
        }
    });

    $scope.DisableContactType = false;

    $scope.EditContact = function (index, data) {
        //$scope.IsEditModeforContact = true;
        $scope.TempContactTypeID = data.ContactTypeID;

        $scope.ReferralModel.AddAndListContactInformation.Latitude = data.Latitude;
        $scope.ReferralModel.AddAndListContactInformation.Longitude = data.Longitude;

        HideErrors($("#frmAddContact"));

        var tempIsPrimaryPlacementLegalGuardian = false;
        if (data.ContactTypeID.toString() === $scope.LegalGuardianContactTypeID) {
            angular.forEach($scope.ReferralModel.ContactInformationList, function (item, key) {
                if (parseInt($scope.PrimaryPlacementContactTypeID) == parseInt(item.ContactTypeID)) {
                    tempIsPrimaryPlacementLegalGuardian = JSON.parse(item.IsPrimaryPlacementLegalGuardian);
                }
            });

        }

        if (tempIsPrimaryPlacementLegalGuardian) {
            bootboxDialog(null, bootboxDialogType.Alert, bootboxDialogTitle.Information, window.CanNotUpdateLegalGuardianInformation, bootboxDialogButtonText.Ok);
        } else {
            if (data.ContactTypeID.toString() === $scope.PrimaryPlacementContactTypeID || data.ContactTypeID.toString() === $scope.LegalGuardianContactTypeID) {
                $scope.DisableContactType = true;
                if (data.ContactTypeID.toString() === $scope.PrimaryPlacementContactTypeID)
                    $scope.TempPrimaryPlacementLegalGuardian = data.IsPrimaryPlacementLegalGuardian;

            } else {
                $scope.DisableContactType = false;
            }
            $('#model_AddContact').modal({
                backdrop: 'static',
                keyboard: false
            });
            $timeout(function () {
                //$("#SearchContactToken").tokenInput("clear");
                $scope.ReferralModel.AddAndListContactInformation = $.parseJSON(JSON.stringify($scope.ReferralModel.ContactInformationList[index]));
                JSON.parse($scope.ReferralModel.AddAndListContactInformation.IsEmergencyContact) ? $scope.ReferralModel.AddAndListContactInformation.IsEmergencyContact = "true" : $scope.ReferralModel.AddAndListContactInformation.IsEmergencyContact = "false";
                JSON.parse($scope.ReferralModel.AddAndListContactInformation.IsPrimaryPlacementLegalGuardian) ? $scope.ReferralModel.AddAndListContactInformation.IsPrimaryPlacementLegalGuardian = "true" : $scope.ReferralModel.AddAndListContactInformation.IsPrimaryPlacementLegalGuardian = "false";
                JSON.parse($scope.ReferralModel.AddAndListContactInformation.IsDCSLegalGuardian) ? $scope.ReferralModel.AddAndListContactInformation.IsDCSLegalGuardian = "true" : $scope.ReferralModel.AddAndListContactInformation.IsDCSLegalGuardian = "false";
                JSON.parse($scope.ReferralModel.AddAndListContactInformation.IsNoticeProviderOnFile) ? $scope.ReferralModel.AddAndListContactInformation.IsNoticeProviderOnFile = "true" : $scope.ReferralModel.AddAndListContactInformation.IsNoticeProviderOnFile = "false";

            });


            $('#model_AddContact').on('shown.bs.modal', function (e) {
                // do something...
                //$scope.GenerateGeoCode(true);
            });

        }
    };

    $scope.DeleteContact = function (index, item) {

        bootboxDialog(function (result) {
            if (result) {
                if (item.ContactMappingID > 0) {

                    var jsonData = angular.toJson({ contactMappingID: item.ContactMappingID });
                    AngularAjaxCall($http, $scope.DeleteContactURL, jsonData, "Post", "json", "application/json").success(function (response) {
                        if (response.IsSuccess) {
                            $scope.ReferralModel.ContactInformationList.remove(item);
                        }
                        ShowMessages(response);
                    });
                } else {
                    $scope.$apply(function () {
                        $scope.ReferralModel.ContactInformationList.splice(index, 1);
                    });

                }


            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);

    };

    $scope.ContactAddCallback = function (item, e) {

        $scope.$apply(function () {
            $scope.IsContactEditMode = true;

            //$scope.ReferralModel.AddAndListContactInformation = item;
            $scope.ReferralModel.TempAddAndListContactInformation = angular.copy($scope.ReferralModel.AddAndListContactInformation);


            $scope.ReferralModel.AddAndListContactInformation.ContactID = item.ContactID;
            $scope.ReferralModel.AddAndListContactInformation.FirstName = item.FirstName;
            $scope.ReferralModel.AddAndListContactInformation.LastName = item.LastName;
            $scope.ReferralModel.AddAndListContactInformation.Email = item.Email;
            $scope.ReferralModel.AddAndListContactInformation.EmpFullName = window.loggedInUserName;
            $scope.ReferralModel.AddAndListContactInformation.Address = item.Address;
            $scope.ReferralModel.AddAndListContactInformation.City = item.City;
            //$scope.ReferralModel.AddAndListContactInformation.State = item.State;
            $scope.ReferralModel.AddAndListContactInformation.ZipCode = item.ZipCode;
            $scope.ReferralModel.AddAndListContactInformation.Phone1 = item.Phone1;
            $scope.ReferralModel.AddAndListContactInformation.Phone2 = item.Phone2;
            $scope.ReferralModel.AddAndListContactInformation.LanguageID = item.LanguageID;

            // This function will set the default values for the Contact
            //$scope.SetDefaultValuesForAddContact();


        });
    };

    $scope.ContactDeleteCallback = function (item, e) {

        $scope.$apply(function () {
            if ($scope.ReferralModel.TempAddAndListContactInformation != null || $scope.ReferralModel.TempAddAndListContactInformation != undefined) {

                $scope.IsContactEditMode = false;
                $scope.ReferralModel.AddAndListContactInformation.MasterContactUpdated = false;

                $scope.ReferralModel.AddAndListContactInformation.ContactID = $scope.ReferralModel.TempAddAndListContactInformation.ContactID;
                $scope.ReferralModel.AddAndListContactInformation.FirstName = $scope.ReferralModel.TempAddAndListContactInformation.FirstName;
                $scope.ReferralModel.AddAndListContactInformation.LastName = $scope.ReferralModel.TempAddAndListContactInformation.LastName;
                $scope.ReferralModel.AddAndListContactInformation.Email = $scope.ReferralModel.TempAddAndListContactInformation.Email;
                $scope.ReferralModel.AddAndListContactInformation.Address = $scope.ReferralModel.TempAddAndListContactInformation.Address;
                $scope.ReferralModel.AddAndListContactInformation.City = $scope.ReferralModel.TempAddAndListContactInformation.City;
                $scope.ReferralModel.AddAndListContactInformation.ZipCode = $scope.ReferralModel.TempAddAndListContactInformation.ZipCode;
                $scope.ReferralModel.AddAndListContactInformation.Phone1 = $scope.ReferralModel.TempAddAndListContactInformation.Phone1;
                $scope.ReferralModel.AddAndListContactInformation.Phone2 = $scope.ReferralModel.TempAddAndListContactInformation.Phone2;
                $scope.ReferralModel.AddAndListContactInformation.LanguageID = $scope.ReferralModel.TempAddAndListContactInformation.LanguageID;

            } else {
                $scope.SetDefaultValuesForAddContact();
            }


        });
    };

    $scope.ContactResultsFormatter = function (item) {
        return $scope.GetContactResultFormat(item);
    };

    $scope.ContactTokenFormatter = function (item) {
        return $scope.GetContactResultFormat(item);
    };

    $scope.GetContactResultFormat = function (item) {
        //item.Email = item.Email == undefined ? NA : item.Email;
        //item.Phone1 = item.Phone1 == undefined ? NA : item.Phone1;
        //item.Address = item.Address == undefined ? NA : item.Address;
        return "<li id='{0}'>{1} {2} ({3})<br/><small><b  style='color:#ad0303;'>{9}: </b>{4}</small><br/><small ><b style='color:#ad0303;'>{10}: </b>{5}, {6}, {7} - {8}</small></li>".format(
            item.ContactID, item.FullName, '', item.Email == undefined ? 'NA' : item.Email, item.Phone1 == undefined ? 'NA' : item.Phone1.replace(/(\d{3})(\d{3})(\d{4})/, "($1) $2-$3"),
            item.Address == undefined ? 'NA' : item.Address, item.City, item.State, item.ZipCode, Phone, Address);
    };

    $scope.SaveContact = function () {
        var isValid = CheckErrors($("#frmAddContact"));

        if (isValid) {

            if ($scope.ReferralModel.AddAndListContactInformation.ContactTypeID == window.PrimaryPlacementContactTypeID) {
                $scope.GenerateGeoCode(false, $scope.SaveContactDetail, function () {
                    toastr.error(window.GeoCodeError);
                });
            } else {
                $scope.SaveContactDetail();
            }

        }
    };

    $scope.SaveContactDetail = function () {
        var isValid = CheckErrors($("#frmAddContact"));

        if (isValid) {
            angular.forEach($scope.ReferralModel.ContactInformationList, function (item, key) {
                if ($scope.TempContactTypeID != parseInt(item.ContactTypeID)) {
                    if (parseInt($scope.ReferralModel.AddAndListContactInformation.ContactTypeID) == parseInt(item.ContactTypeID)) {
                        //alert("Cannot Added");

                        $scope.CanAddContact = false;
                        bootboxDialog(null, bootboxDialogType.Alert, bootboxDialogTitle.Alert,
                            window.CaontactTypeAlreadyExist.format($scope.ReferralModel.AddAndListContactInformation.ContactTypeName), bootboxDialogButtonText.Ok);
                        return;
                    }
                }

            });

            if ($scope.CanAddContact) {

                if ($scope.TempPrimaryPlacementLegalGuardian == undefined)
                    $scope.TempPrimaryPlacementLegalGuardian = false;
                //CHECK LEGAL GURDIAN EXIST IF YES THEN SHOW MESSAGE AND INFORM ABOUT LEGAL GUARDINA DELETE
                // IF LEGAL GUARDIAN NOT FOUND THEN ADD NEW LEAGAL GUARDIAN AND ALSO BEFORE ADD SHOW MESSSAGE FOR LEGUAL GURADIN ADD
                if (parseInt($scope.ReferralModel.AddAndListContactInformation.ContactTypeID) == parseInt(window.PrimaryPlacementContactTypeID)
                    && JSON.parse($scope.ReferralModel.AddAndListContactInformation.IsPrimaryPlacementLegalGuardian)
                    && JSON.parse($scope.TempPrimaryPlacementLegalGuardian) == false) {

                    var isLegalGuardianExit = false;
                    var legalGuardianRemoveIndex = -1;
                    var legalContactMappingID = 0;

                    angular.forEach($scope.ReferralModel.ContactInformationList, function (item, key) {
                        if (item.ContactTypeID == window.LegalGuardianContactTypeID) {
                            isLegalGuardianExit = true;
                            legalGuardianRemoveIndex = key;
                            legalContactMappingID = item.ContactMappingID == 0 ? item.TempContactMappingID : item.ContactMappingID;
                        }
                    });

                    var message = isLegalGuardianExit ? window.LegalGuardianExists : window.AddLegalGuardian;
                    bootboxDialog(function (result) {
                        if (result) {
                            if (isLegalGuardianExit) {
                                //DELETE Legal Guardian Information
                                $scope.$apply(function () {
                                    //$scope.ReferralModel.ContactInformationList.remove(item);
                                    if ($scope.ReferralModel.ContactInformationList != -1) {

                                        $scope.ReferralModel.ContactInformationList.splice(legalGuardianRemoveIndex, 1);
                                    }
                                });
                            }
                            $scope.AddUpdateContactDetails(legalContactMappingID);
                        }

                    }, bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, message, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);


                } else {

                    $scope.AddUpdateContactDetails();
                }
            }
            $scope.CanAddContact = true;
        }
    };

    $scope.AddUpdateContactDetails = function (legalContactMappingID) {
        $timeout(function () {
            $scope.$apply(function () {
                $scope.ReferralModel.AddAndListContactInformation.AddNewContactDetails = true;
                var model = $.parseJSON(JSON.stringify($scope.ReferralModel.AddAndListContactInformation));
                //FOR OTHER ALL TYPE OF CONTACT
                if ($scope.TempContactTypeID > 0) {  //EDIT MODE
                    angular.forEach($scope.ReferralModel.ContactInformationList, function (item, index) {
                        if (item.ContactTypeID == parseInt($scope.TempContactTypeID)) {
                            $scope.ReferralModel.ContactInformationList[index] = model;
                        }
                    });
                } else {  // ADD MODE

                    //ADD Primary Placement Contact
                    model.EmpFirstName = window.loggedInUserName;
                    $scope.ReferralModel.ContactInformationList.push(model);
                }

                if (legalContactMappingID >= 0) {
                    //ADD Legal Guardin Contact
                    var model1 = $.parseJSON(JSON.stringify($scope.ReferralModel.AddAndListContactInformation));
                    model1.ContactMappingID = legalContactMappingID;
                    model1.ContactTypeID = window.LegalGuardianContactTypeID;
                    model1.ContactTypeName = window.LegalGuardian;
                    model1.TempContactMappingID = legalContactMappingID;
                    model1.IsPrimaryPlacementLegalGuardian = false;
                    model1.EmpFirstName = window.loggedInUserName;
                    $scope.ReferralModel.ContactInformationList.push(model1);
                    $scope.AddUpdateContactToDB(model1);
                }
                else {
                    $scope.AddUpdateContactToDB(model);
                }

            });
            $('#model_AddContact').modal('hide');
            $scope.TempContactTypeID = 0;
        });


    };

    $scope.AddUpdateContactToDB = function (model) {
        if ($scope.ReferralModel.Referral.ReferralID > 0) {
            if (model.ContactID > 0) {
                model.AddNewContactDetails = false;
            }
            else {
                model.AddNewContactDetails = true;
                model.ReferralID = $scope.ReferralModel.Referral.ReferralID;
                model.ClientID = $scope.ReferralModel.Referral.ClientID;
            }
            var jsonData = angular.toJson({
                Referral: $scope.ReferralModel.Referral,
                AddAndListContactInformation: model,
            });
            AngularAjaxCall($http, $scope.AddContactURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {

                } else {
                    //ShowMessages(response);
                }
            });
        }
    }

    $scope.EditSearchContact = function () {
        $scope.IsContactEditMode = false;
        $scope.ReferralModel.AddAndListContactInformation.MasterContactUpdated = true;
    };

    $scope.GenerateGeoCode = function (isFirstTimeLoad, callback, errorCallback) {
        if (isFirstTimeLoad === false && (!ValideElement($scope.ReferralModel.AddAndListContactInformation.Address) ||
            !ValideElement($scope.ReferralModel.AddAndListContactInformation.City) ||
            !ValideElement($scope.ReferralModel.AddAndListContactInformation.ZipCode) ||
            !ValideElement($scope.ReferralModel.AddAndListContactInformation.State))) {
            toastr.error(window.AddressMissingMessage);
            return false;
        }

        var address = $scope.ReferralModel.AddAndListContactInformation.Address + ',' + $scope.ReferralModel.AddAndListContactInformation.City
            + '-' + $scope.ReferralModel.AddAndListContactInformation.ZipCode + ',' + $scope.ReferralModel.AddAndListContactInformation.State;
        //var mapElement = "gmap";
        GetGeoCodeAndMapFromAddress(address, undefined, function (lat, long) {
            $scope.ReferralModel.AddAndListContactInformation.Latitude = lat;
            $scope.ReferralModel.AddAndListContactInformation.Longitude = long;
            //$("#" + mapElement).css({ 'height': '300px', 'width': '100%' });
            if (!$scope.$root.$$phase) {
                $scope.$apply();
            }

            if (callback && ValideElement(lat) && ValideElement(long)) {
                callback();
            }
            else {
                if (errorCallback)
                    errorCallback();
            }

        });
    };

    //$scope.GenerateGeoCode = function (isFirstTimeLoad) {
    //    if (isFirstTimeLoad === false && (!ValideElement($scope.ReferralModel.AddAndListContactInformation.Address) ||
    //        !ValideElement($scope.ReferralModel.AddAndListContactInformation.City) ||
    //        !ValideElement($scope.ReferralModel.AddAndListContactInformation.ZipCode) ||
    //        !ValideElement($scope.ReferralModel.AddAndListContactInformation.State))) {
    //        toastr.error(window.AddressMissingMessage);
    //        return false;
    //    }


    //    var address = $scope.ReferralModel.AddAndListContactInformation.Address + ',' + $scope.ReferralModel.AddAndListContactInformation.City
    //        + '-' + $scope.ReferralModel.AddAndListContactInformation.ZipCode + ',' + $scope.ReferralModel.AddAndListContactInformation.State;
    //    var mapElement = "gmap";
    //    GetGeoCodeAndMapFromAddress(address, mapElement, function (lat, long) {
    //        $scope.ReferralModel.AddAndListContactInformation.Latitude = lat;
    //        $scope.ReferralModel.AddAndListContactInformation.Longitude = long;
    //        $("#" + mapElement).css({ 'height': '300px', 'width': '100%' });
    //        if (!$scope.$root.$$phase) {
    //            $scope.$apply();
    //        }

    //    });
    //};



    //#endregion

    //#region physician Mapping
    $scope.PhysicianTokenInputObj = {};
    $scope.GetPhysicianListForAutoCompleteURL = "/hc/physician/getphysicianlistforautocomplete";

    $scope.AddPhysician = function () {
        if ($scope.ReferralModel.ReferralPhysicians == null || $scope.ReferralModel.ReferralPhysicians == undefined) {
            $scope.ReferralModel.ReferralPhysicians = [];
        }
        var currentPhysician = $scope.ReferralModel.ReferralPhysicians.filter(function (item) {
            return item.PhysicianID == $scope.ReferralModel.PhysicianModel.PhysicianID;
        });
        if (currentPhysician.length == 0) {
            var item = angular.copy($scope.ReferralModel.PhysicianModel);
            $scope.ReferralModel.ReferralPhysicians.push(item);
        }
        $scope.PhysicianTokenInputObj.clear();
        $scope.ReferralModel.PhysicianModel = {};
    }

    $scope.CancelPhysician = function () {
        $scope.PhysicianTokenInputObj.clear();
        $scope.ReferralModel.PhysicianModel = {};
    }

    $scope.DeletePhysician = function (tempObject, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                if (tempObject.ReferralPhysicianID > 0) {
                    var jsonData = angular.toJson({
                        ReferralPhysicianID: tempObject.ReferralPhysicianID,
                        EncryptedReferralID: $scope.EncryptedReferralID
                    });
                    AngularAjaxCall($http, HomeCareSiteUrl.DeletePhysicianURL, jsonData, "Post", "json", "application/json").success(function (response) {
                        if (response.IsSuccess) {
                            $scope.ReferralModel.ReferralPhysicians.remove(tempObject);
                        }
                    });
                }
                else {
                    $scope.ReferralModel.ReferralPhysicians.remove(tempObject);
                }
            }
        }, bootboxDialogType.Confirm, title, window.DeletePhysicianMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.PhysicianResultsFormatter = function (item) {
        return "<li id='{0}'>{1} {2} {3} <br/><small><b>Address: </b> {5} </small ><br /><small><b style='color:#ad0303;'>Phone:</b>{6}</small ><small><b style='color:#ad0303;padding-left:10px;'>Email: </b>{7} </small><br><small><b style='color:#ad0303;'>Specialist:</b>{8}</small></li >"
            .format(
                item.PhysicianID,
                item.FirstName,
                item.MiddleName,
                item.LastName,
                item.NPINumber ? item.NPINumber : window.NA,
                item.Address ? item.Address : window.NA,
                item.Phone ? item.Phone : window.NA,
                item.Email ? item.Email : window.NA,
                item.PhysicianTypeID
            );
    };
    $scope.PhysicianTokenFormatter = function (item) {
        return "<li id='{0}'>{1} {2} {3}{4} </li>".format(item.PhysicianID, item.FirstName, item.MiddleName, item.LastName, item.NPINumber, item.PhysicianTypeID);
    };

    $scope.RemovePhysician = function () {
        $scope.ReferralModel.Referral.PhysicianID = null;
        $scope.ReferralModel.PhysicianModel.Email = '';
        $scope.ReferralModel.PhysicianModel.Phone = '';
        $scope.ReferralModel.PhysicianModel.Address = '';
        $scope.ReferralModel.PhysicianModel.PhysicianTypeName = '';
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };
    $scope.OnPhysicianAdded = function (item) {
        $scope.ReferralModel.Referral.PhysicianID = item.PhysicianID;
        $scope.ReferralModel.PhysicianModel.PhysicianID = item.PhysicianID;
        $scope.ReferralModel.PhysicianModel.Email = item.Email;
        $scope.ReferralModel.PhysicianModel.Phone = item.Phone;
        $scope.ReferralModel.PhysicianModel.Address = item.Address;
        $scope.ReferralModel.PhysicianModel.FirstName = item.FirstName;
        $scope.ReferralModel.PhysicianModel.MiddleName = item.MiddleName;
        $scope.ReferralModel.PhysicianModel.LastName = item.LastName;
        $scope.ReferralModel.PhysicianModel.PhysicianTypeID = item.PhysicianTypeID;
        $scope.AddPhysician();
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };

    //#endregion

    //#region DX CODE MAPPING RELATED FUNCATION START

    $scope.TokenInputObj = {};
    $scope.SelectedDXCodeIDs = [];
    $scope.TempSelectedDXCodeIDs = [];
    $scope.GetDXCodeListForAutoCompleteURL = HomeCareSiteUrl.GetDXCodeListForAutoCompleteURL;

    $scope.CheckPrimaryDxCode = function () {
        var isHavePrimaryDxcode = false;
        if ($scope.ReferralModel.DxCodeMappingList.length > 0) {
            $scope.ReferralModel.DxCodeMappingList.forEach(function (item) {
                if (parseInt(item.Precedence) === 1)
                    isHavePrimaryDxcode = true;
            });
        } else {
            isHavePrimaryDxcode = true;
        }
        return isHavePrimaryDxcode;
    };

    $scope.OnDXCodeAdd = function (item, e) {
        $timeout(function () {
            $scope.ReferralModel.ReferralDXCodeMapping.DXCodeID = item.DXCodeID;
            $scope.ReferralModel.ReferralDXCodeMapping.DXCodeName = item.DXCodeName;
            $scope.ReferralModel.ReferralDXCodeMapping.DXCodeWithoutDot = item.DXCodeWithoutDot;
            $scope.ReferralModel.ReferralDXCodeMapping.Description = item.Description;
            $scope.ReferralModel.ReferralDXCodeMapping.DxCodeShortName = item.DxCodeShortName;
            $scope.ReferralModel.ReferralDXCodeMapping.IsDeleted = false;
            //$scope.TokenInputObj.clear();
            //$scope.ReferralModel.DxCodeMappingList.push(item);
            //$scope.$apply();
        });
    };

    $scope.SaveReferralDXCodeMapping = function () {
        if (!$scope.ReferralModel.ReferralDXCodeMapping.Precedence) {
            $scope.ReferralModel.ReferralDXCodeMapping.Precedence = $scope.GetMaxPrecedence();
        }
        
        if ($scope.ReferralModel.ReferralDXCodeMapping != null && $scope.ReferralModel.ReferralDXCodeMapping.Precedence != undefined) {
            if ($scope.ReferralModel.ReferralDXCodeMapping.RandomID === undefined || $scope.ReferralModel.ReferralDXCodeMapping.RandomID === null) {
                $scope.ReferralModel.ReferralDXCodeMapping.RandomID = GenerateGuid();
            }

            if ($scope.ReferralModel.ReferralDXCodeMapping.Precedence != 0) {
                if ($scope.ReferralModel.ReferralDXCodeMapping.DXCodeID && $scope.ReferralModel.ReferralDXCodeMapping.Precedence) {

                    var precedenceCount = $scope.ReferralModel.DxCodeMappingList.filter(function (item) {                        
                        var prcd = parseInt($scope.ReferralModel.ReferralDXCodeMapping.Precedence);
                        var rnId = $scope.ReferralModel.ReferralDXCodeMapping.RandomID;
                        //var dxcodeId = parseInt($scope.ReferralModel.ReferralDXCodeMapping.DXCodeID);

                        var iprcd = parseInt(item.Precedence);
                        var irnId = item.RandomID;
                        //var idxcodeId = parseInt(item.DXCodeID);
                        if ((iprcd === prcd) && irnId !== rnId) {
                            return true;
                            $scope.Search = null;
                            angular.element('#display').removeClass("display");
                        }
                        return false;
                        //return parseInt(item.Precedence) == parseInt($scope.ReferralModel.ReferralDXCodeMapping.Precedence) && item.DXCodeID != $scope.ReferralModel.ReferralDXCodeMapping.DXCodeID;
                    }).length;

                    if (precedenceCount > 0) {
                        toastr.error(window.DXCodeWithPrecedenceExists);
                    }
                    else {

                        var index = $.map($scope.ReferralModel.DxCodeMappingList, function (obj, id) {
                            if (obj.RandomID == $scope.ReferralModel.ReferralDXCodeMapping.RandomID) {
                                return id;
                            }
                            return null;
                        });

                        if (index.length > 0 && $scope.ReferralModel.DxCodeMappingList[index].RandomID == $scope.ReferralModel.ReferralDXCodeMapping.RandomID) {
                            $scope.ReferralModel.DxCodeMappingList[index].DXCodeID = $scope.ReferralModel.ReferralDXCodeMapping.DXCodeID;
                            $scope.ReferralModel.DxCodeMappingList[index].Precedence = $scope.ReferralModel.ReferralDXCodeMapping.Precedence;
                            var jsonData = angular.toJson({
                                Referral: $scope.ReferralModel.Referral,
                                DxCodeMappingList: $scope.ReferralModel.DxCodeMappingList,
                                PreferenceList: $scope.ListPreference,
                            });
                            AngularAjaxCall($http, $scope.ReferralDxCodeMappingURL, jsonData, "Post", "json", "application/json").success(function (response) {
                                if (response.IsSuccess) {
                                    toastr.success("DX Code Update Successfully");
                                    if (!response.Data.Referral.IsEditMode) {
                                        $scope.EncryptedReferralID = response.Data.Referral.EncryptedReferralID;
                                    } else {
                                    }
                                } else
                                    toastr.success("DX Code Update Successfully");
                            });
                        } else {
                            // for save dx code akhilesh
                            var precedenceCount = $scope.ReferralModel.DxCodeMappingList.filter(function (item) {
                                var prcd = parseInt($scope.ReferralModel.ReferralDXCodeMapping.Precedence);

                            }).length
                            $scope.ReferralModel.DxCodeMappingList.push($scope.ReferralModel.ReferralDXCodeMapping);
                            $scope.TempSelectedDXCodeIDs.push($scope.ReferralModel.ReferralDXCodeMapping.DXCodeID);
                            $scope.ValidateDxCode();
                            var isHavePrimaryDxCode = $scope.CheckPrimaryDxCode();
                            if (isHavePrimaryDxCode) {

                                $scope.ReferralModel.Referral.IsSaveAsDraft = false;
                                if ($scope.SelectedCareType) {
                                    var careTypeValues = $scope.SelectedCareType;
                                    $scope.ReferralModel.Referral.CareTypeIds = Object.keys(careTypeValues).map(function (k) { return careTypeValues[k]["Value"] }).join(",");
                                }
                                else {
                                    $scope.ReferralModel.Referral.CareTypeIds = null;
                                }
                                var jsonData = angular.toJson({
                                    Referral: $scope.ReferralModel.Referral,
                                    DxCodeMappingList: $scope.ReferralModel.DxCodeMappingList,
                                    PreferenceList: $scope.ListPreference,
                                });
                                AngularAjaxCall($http, $scope.ReferralDxCodeMappingURL, jsonData, "Post", "json", "application/json").success(function (response) {
                                    if (response.IsSuccess) {
                                        $scope.ReferralModel.DxCodeMappingList = response.Data.DxCodeMappingList;
                                        $scope.Save();
                                        toastr.success("DX Code Save Successfully");
                                        if (!response.Data.Referral.IsEditMode) {
                                            $scope.EncryptedReferralID = response.Data.Referral.EncryptedReferralID;
                                        } else {
                                        }
                                    } else
                                        ShowMessages("DX Code Save Successfully");
                                });
                                $scope.Search = null;
                                angular.element('#display').removeClass("display");
                            }
                            else {
                                $scope.ReferralModel.DxCodeMappingList.remove($scope.ReferralModel.ReferralDXCodeMapping);
                                $scope.TempSelectedDXCodeIDs.remove($scope.ReferralModel.ReferralDXCodeMapping.DXCodeID);
                                toastr.error(window.PrimaryDXCodeValidation);
                            }
                        }
                        //}

                    }
                } else {
                    $scope.ListofFieldsForDxCode = "";
                    var liString = "<li>{0} : {1}</li>";
                    $scope.ListofFields = "<ul>";
                    if (!$scope.ReferralModel.ReferralDXCodeMapping.DXCodeID) {
                        $scope.ListofFieldsForDxCode += liString.format(window.Missing, window.DXCode);
                    }

                    if (!$scope.ReferralModel.ReferralDXCodeMapping.Precedence) {
                        $scope.ListofFieldsForDxCode += liString.format(window.Missing, window.Precedence);
                    }

                    bootboxDialog(function () {
                    }, bootboxDialogType.Alert, window.Alert, window.FieldsIncomplete.format($scope.ListofFieldsForDxCode));
                    $scope.Search = $scope.ReferralModel.ReferralDXCodeMapping.DXCodeName;
                }
            }
            else {
                toastr.error("Please choose other Precedence");

            }
        }
        else {
            toastr.error("Please fill all fields");
        }

        $scope.ReferralModel.Precedence = null;
        $scope.CancelDXCodeMapping();

    };

    $scope.GetMaxPrecedence = function () {
        
        return $scope.ReferralModel.DxCodeMappingList.length == 0 ? 1 : 1 + (Math.max.apply(Math, $scope.ReferralModel.DxCodeMappingList.map(function (o) { return parseInt(o.Precedence); })));
    };

    $scope.CancelDXCodeMapping = function () {
        $scope.Search = null;
        $scope.ReferralModel.ReferralDXCodeMapping = null;
        $scope.ReferralModel.ReferralDXCodeMapping = {};
        angular.element('#display').removeClass("display");
        $scope.ReferralModel.ReferralDXCodeMapping = null;
        $scope.ReferralModel.Precedence = null;
        $scope.Search1 = false;
        $scope.isDisabled = false;
    };

    $scope.ReferralModel.Precedence = {};

    $scope.isDisabled = false;

    $scope.EditDxCodeFromMapping = function (data) {
        angular.element('#glyphicon-remove').removeClass("glyphicon-remove");
        $scope.ReferralModel.Precedence = data.Precedence
        $scope.ReferralModel.ReferralDXCodeMapping = angular.copy(data);
        $scope.ReferralModel.ReferralDXCodeMapping.DXCodeID = data.DXCodeID;
        $scope.Search = data.DXCodeName;
        $scope.isDisabled = true;
        //  $scope.DxCodeTokenInputObj.add({ DXCodeID: data.DXCodeID, DXCodeName: data.DXCodeName, readonly: true });
    };

    $scope.DeleteDxCodeFromMapping = function (tempObject, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                if (tempObject.ReferralDXCodeMappingID > 0) {
                    var jsonData = angular.toJson({
                        ReferralDXCodeMappingID: tempObject.ReferralDXCodeMappingID,
                        EncryptedReferralID: $scope.EncryptedReferralID
                    });
                    AngularAjaxCall($http, HomeCareSiteUrl.DeleteReferralDXCodeMappingURL, jsonData, "Post", "json", "application/json").success(function (response) {
                        if (response.IsSuccess) {
                            $scope.ReferralModel.DxCodeMappingList = response.Data;
                            $scope.DxCodeMappingList = response.Data;
                            $scope.Save();
                            $timeout(function () {
                                $scope.$apply();
                                $scope.ValidateDxCode();
                            });
                        }
                    });
                }
                else {
                    $scope.ReferralModel.DxCodeMappingList.remove(tempObject);
                    $scope.TempSelectedDXCodeIDs.remove(tempObject.DXCodeID);
                }
                $scope.ReferralModel.DxCodeMappingList.remove(tempObject);
                $scope.TempSelectedDXCodeIDs.remove(tempObject.DXCodeID);
                $timeout(function () {
                    $scope.$apply();
                    $scope.ValidateDxCode();
                });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteDxCodeConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    if ($scope.ReferralModel.DxCodeMappingList.length > 0) {
        $.each($scope.ReferralModel.DxCodeMappingList, function (i, dxcode) {
            $scope.TempSelectedDXCodeIDs.push(dxcode.DXCodeID);
        });
    }

    $scope.ValidateDxCode = function () {
        $scope.ReferralModel.DXCodeCount = $scope.ReferralModel.DxCodeMappingList.filter(function (item) {
            return item.IsDeleted == false;
        }).length;

        if ($scope.ReferralModel.DXCodeCount == 0)
            $scope.ReferralStatusChange($scope.ReferralModel.Referral.ReferralStatusID);

        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
        //$('#DXCodeCount').valid();
    };

    $scope.isNotDeleted = function (x) {
        return x.IsDeleted == false;
    };

    $scope.OnDxCodeResultsFormatter = function (item) {
        return "<li id='{0}'><b>{3}: </b> {1} ({7})<span class='badge badge-primary' style='float:right;'>{8}</span><br/><b  style='color:#ad0303;'>{4}: </b>{2} </small></li>".
            format(item.DXCodeID, item.DXCodeWithoutDot ? item.DXCodeWithoutDot : NA, (item.Description) ? item.Description : NA, DXCode, Description, StartDate, EndDate, item.DXCodeName, item.DxCodeShortName);
    };

    //#endregion

    //#region Beneficiary Type Start

    $scope.SaveReferralBeneficiaryType = function () {
        if ($scope.ReferralModel.ReferralBeneficiaryTypeDetail.RandomID === undefined || $scope.ReferralModel.ReferralBeneficiaryTypeDetail.RandomID === null) {
            $scope.ReferralModel.ReferralBeneficiaryTypeDetail.RandomID = GenerateGuid();
        }

        if ($scope.ReferralModel.ReferralBeneficiaryTypeDetail.BeneficiaryTypeID && $scope.ReferralModel.ReferralBeneficiaryTypeDetail.BeneficiaryNumber) {

            var duplicateRecords = $scope.ReferralModel.ReferralBeneficiaryTypes.filter(function (item) {
                var bType = $scope.ReferralModel.ReferralBeneficiaryTypeDetail.BeneficiaryTypeID;
                var rnId = $scope.ReferralModel.ReferralBeneficiaryTypeDetail.RandomID;

                var iBenType = item.BeneficiaryTypeID;
                var irnId = item.RandomID;

                if (iBenType === bType && irnId !== rnId) {
                    return true;
                }

                return false;
            }).length;

            if (duplicateRecords > 0) {
                toastr.error(window.BeneficiaryTypeExists);
            }
            else {
                if ($scope.BeneficiaryTypeName) {
                    $scope.ReferralModel.ReferralBeneficiaryTypeDetail.BeneficiaryTypeName = $scope.BeneficiaryTypeName;
                }
                var index = $.map($scope.ReferralModel.ReferralBeneficiaryTypes, function (obj, id) {
                    if (obj.RandomID == $scope.ReferralModel.ReferralBeneficiaryTypeDetail.RandomID) {
                        return id;
                    }
                    return null;
                });

                if (index.length > 0 && $scope.ReferralModel.ReferralBeneficiaryTypes[index].RandomID == $scope.ReferralModel.ReferralBeneficiaryTypeDetail.RandomID) {
                    $scope.ReferralModel.ReferralBeneficiaryTypes[index].BeneficiaryTypeID = $scope.ReferralModel.ReferralBeneficiaryTypeDetail.BeneficiaryTypeID;
                    $scope.ReferralModel.ReferralBeneficiaryTypes[index].BeneficiaryTypeName = $scope.ReferralModel.ReferralBeneficiaryTypeDetail.BeneficiaryTypeName;
                    $scope.ReferralModel.ReferralBeneficiaryTypes[index].BeneficiaryNumber = $scope.ReferralModel.ReferralBeneficiaryTypeDetail.BeneficiaryNumber;
                    $scope.ReferralModel.ReferralBeneficiaryTypeDetail = {};
                } else {
                    $scope.ReferralModel.ReferralBeneficiaryTypes.push($scope.ReferralModel.ReferralBeneficiaryTypeDetail);
                    $scope.ReferralModel.ReferralBeneficiaryTypeDetail = {};
                }
                $('#AddReferralBeneficiaryTypeModal').modal('hide');
            }
        } else if (!$scope.ReferralModel.ReferralBeneficiaryTypeDetail.BeneficiaryTypeID) {
            toastr.error(window.SelectBeneficiaryType);
        } else {
            toastr.error(window.SelectBeneficiaryNumber);
        }
    };

    $scope.CancelReferralBeneficiaryType = function () {
        $scope.ReferralModel.ReferralBeneficiaryTypeDetail = {};
        $('#AddReferralBeneficiaryTypeModal').modal('hide');
    };

    $scope.EditReferralBeneficiaryType = function (data) {
        $scope.ReferralModel.ReferralBeneficiaryTypeDetail = angular.copy(data);
        $scope.ReferralModel.ReferralBeneficiaryTypeDetail.BeneficiaryTypeID = data.BeneficiaryTypeID;
        $scope.ReferralModel.ReferralBeneficiaryTypeDetail.BeneficiaryTypeName = data.BeneficiaryTypeName;
        $scope.ReferralModel.ReferralBeneficiaryTypeDetail.BeneficiaryNumber = data.BeneficiaryNumber;
        $('#AddReferralBeneficiaryTypeModal').modal({
            backdrop: 'static',
            keyboard: false
        });
    };

    $scope.DeleteReferralBeneficiaryType = function (tempObject, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                if (tempObject.ReferralBeneficiaryTypeID > 0) {
                    var jsonData = angular.toJson({
                        ReferralBeneficiaryTypeID: tempObject.ReferralBeneficiaryTypeID
                    });
                    AngularAjaxCall($http, HomeCareSiteUrl.DeleteReferralBeneficiaryTypeURL, jsonData, "Post", "json", "application/json").success(function (response) {
                        if (response.IsSuccess) {
                            $scope.ReferralModel.ReferralBeneficiaryTypes.remove(tempObject);
                        }
                    });
                }
                else {
                    $scope.ReferralModel.ReferralBeneficiaryTypes.remove(tempObject);
                }
            }
        }, bootboxDialogType.Confirm, title, window.DeleteBeneficiaryTypeMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    //#endregion Beneficiary Type End

    //#region Payor MAPPING RELATED FUNCATION START
    $scope.DeleteReferralPayorMapping = function (item) {
        bootboxDialog(function (result) {
            if (result) {
                if (item.ReferralPayorMappingID > 0) {
                    var jsonData = angular.toJson({ referralPayorMappingID: item.ReferralPayorMappingID });
                    AngularAjaxCall($http, $scope.DeleteReferralPayorMappingURL, jsonData, "Post", "json", "application/json").success(function (response) {
                        if (response.IsSuccess)
                            $scope.ReferralModel.ReferralPayorMappingList.remove(item);
                        ShowMessages(response);
                    });
                }
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeletePayorConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.AssigneeFilter = function (value) {
        return function (item) {
            if (item.IsDeleted == 0 || item.EmployeeID == value) {
                return item;
            }
        };
    };

    $scope.MarkPayorAsActive = function (referralPayorMappingID) {
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = angular.toJson({ referralID: $scope.ReferralModel.Referral.ReferralID, referralPayorMappingID: referralPayorMappingID });
                AngularAjaxCall($http, SiteUrl.MarkPayorAsActive, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.ReferralModel.ReferralPayorMappingList = [];
                        $scope.ReferralModel.ReferralPayorMappingList = response.Data.ListReferralPayorMapping;
                        response.Data.ReferralPayorMapping.PayorEffectiveDate = moment(response.Data.ReferralPayorMapping.PayorEffectiveDate).toDate();
                        response.Data.ReferralPayorMapping.PayorEffectiveEndDate = moment(response.Data.ReferralPayorMapping.PayorEffectiveEndDate).toDate();

                        if (response.Data.ReferralPayorMapping.PayorEffectiveEndDate == 'Invalid Date')
                            response.Data.ReferralPayorMapping.PayorEffectiveEndDate = null;

                        $scope.ReferralModel.ReferralPayorMapping = response.Data.ReferralPayorMapping;
                        if (!$scope.$root.$$phase) {
                            $scope.$apply();
                        }
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, window.EnableDisablePayorMappingConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.EditReferralPayor = function (referralPayorMappingId) {
        HideErrors($("#frmUpdateReferralPayorInfo"));
        var jsonData = angular.toJson({ referralPayorMappingID: referralPayorMappingId });
        AngularAjaxCall($http, SiteUrl.GetReferralPayorDetail, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                response.Data.PayorEffectiveDate = moment(response.Data.PayorEffectiveDate).toDate();
                response.Data.PayorEffectiveEndDate = moment(response.Data.PayorEffectiveEndDate).toDate();
                $scope.ReferralModel.UpdateReferralPayorMapping = response.Data;
                $('#model_updateReferralPayorInfo').modal({
                    backdrop: 'static',
                    keyboard: false
                });
                if (!$scope.$root.$$phase) {
                    $scope.$apply();
                }
            }
        });

    };

    $scope.UpdateReferralPayorInformation = function () {
        var isValid = CheckErrors($("#frmUpdateReferralPayorInfo"));

        if (isValid) {
            var jsonData = angular.toJson({ model: $scope.ReferralModel.UpdateReferralPayorMapping });
            AngularAjaxCall($http, SiteUrl.UpdateReferralPayorInformation, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.ReferralModel.ReferralPayorMappingList = [];
                    $scope.ReferralModel.ReferralPayorMappingList = response.Data.ListReferralPayorMapping;

                    response.Data.ReferralPayorMapping.PayorEffectiveDate = moment(response.Data.ReferralPayorMapping.PayorEffectiveDate).toDate();
                    response.Data.ReferralPayorMapping.PayorEffectiveEndDate = moment(response.Data.ReferralPayorMapping.PayorEffectiveEndDate).toDate();
                    $scope.ReferralModel.ReferralPayorMapping = response.Data.ReferralPayorMapping;
                    $('#model_updateReferralPayorInfo').modal('hide');
                    if (!$scope.$root.$$phase) {
                        $scope.$apply();
                    }
                }
                ShowMessages(response);
            });

        }
    };


    //#endregion

    //#region Referral Sibling 

    $scope.TokenInputObjSibling = {};
    $scope.SelectedReferralIDs = [];
    $scope.TempSelectedReferralIDs = [];
    $scope.TempSelectedReferralIDs.push($scope.ReferralModel.Referral.ReferralID);

    $scope.GetReferralSiblingURL = SiteUrl.GetReferralSiblingURL;

    $scope.OnReferralSiblingAdd = function (item, e) {
        $scope.ReferralModel.ReferralSiblingMappings.ReferralID = item.ReferralID;
        $scope.ReferralModel.ReferralSiblingMappings.Name = item.Name;
        $scope.ReferralModel.ReferralSiblingMappings.AHCCCSID = item.AHCCCSID;
        $scope.ReferralModel.ReferralSiblingMappings.CISNumber = item.CISNumber;
        $scope.ReferralModel.ReferralSiblingMappings.Phone1 = item.Phone1;
        $scope.ReferralModel.ReferralSiblingMappings.ParentName = item.ParentName;
        $scope.ReferralModel.ReferralSiblingMappings.Email = item.Email;
        $scope.ReferralModel.ReferralSiblingMappings.Age = item.Age;
        $scope.ReferralModel.ReferralSiblingMappings.Gender = item.Gender;
        $scope.ReferralModel.ReferralSiblingMappings.Status = item.Status;
        $scope.ReferralModel.ReferralSiblingMappings.EncryptedReferralID = item.EncryptedReferralID;
        $scope.SaveReferralReferalSiblingMapping();
    };

    if ($scope.ReferralModel.ReferralSiblingMappingList.length > 0) {
        $.each($scope.ReferralModel.ReferralSiblingMappingList, function (i, referral) {
            $scope.TempSelectedReferralIDs.push(referral.ReferralID);
        });
    }

    $scope.SaveReferralReferalSiblingMapping = function () {
        if ($scope.ReferralModel.ReferralSiblingMappings.ReferralID) {
            var count = $scope.ReferralModel.ReferralSiblingMappingList.filter(function (item) {
                return parseInt(item.ReferralID) == parseInt($scope.ReferralModel.ReferralSiblingMappings.ReferralID);
            }).length;
            if (count > 0) {
                //toastr.error(window.ReferralSiblingMappingExists);
            } else {
                var index = $.map($scope.ReferralModel.ReferralSiblingMappingList, function (obj, id) {
                    if (obj.ReferralID == $scope.ReferralModel.ReferralSiblingMappingList.ReferralID) {
                        return id;
                    }
                    return null;
                });
                if (index.length > 0 && $scope.ReferralModel.ReferralSiblingMappingList[index].ReferralID == $scope.ReferralModel.ReferralSiblingMappings.ReferralID) {
                    $scope.ReferralModel.ReferralSiblingMappingList[index].ReferralID = $scope.ReferralModel.ReferralDXCodeMapping.ReferralID;
                    $scope.ReferralModel.ReferralSiblingMappingList[index].Name = $scope.ReferralModel.ReferralSiblingMappings.Name;
                    $scope.ReferralModel.ReferralSiblingMappingList[index].AHCCCSID = $scope.ReferralModel.ReferralSiblingMappings.AHCCCSID;
                    $scope.ReferralModel.ReferralSiblingMappingList[index].CISNumber = $scope.ReferralModel.ReferralSiblingMappings.CISNumber;
                    $scope.ReferralModel.ReferralSiblingMappingList[index].Phone1 = $scope.ReferralModel.ReferralSiblingMappings.Phone1;
                    $scope.ReferralModel.ReferralSiblingMappingList[index].Address = $scope.ReferralModel.ReferralSiblingMappings.Address;
                    $scope.ReferralModel.ReferralSiblingMappingList[index].Email = $scope.ReferralModel.ReferralSiblingMappings.Email;
                    $scope.ReferralModel.ReferralSiblingMappingList[index].Age = $scope.ReferralModel.ReferralSiblingMappings.Age;
                    $scope.ReferralModel.ReferralSiblingMappingList[index].Gender = $scope.ReferralModel.ReferralSiblingMappings.Gender;
                    $scope.ReferralModel.ReferralSiblingMappingList[index].ParentName = $scope.ReferralModel.ReferralSiblingMappings.ParentName;
                    $scope.ReferralModel.ReferralSiblingMappingList[index].Status = $scope.ReferralModel.ReferralSiblingMappings.Status;
                    $scope.ReferralSiblingTokenInputObj.clear();
                    $scope.ReferralModel.ReferralSiblingMappings = {};
                } else {
                    $scope.ReferralModel.ReferralSiblingMappings.ReferralID2 = $scope.ReferralModel.ReferralSiblingMappings.ReferralID;
                    $scope.ReferralModel.ReferralSiblingMappingList.push($scope.ReferralModel.ReferralSiblingMappings);
                    $scope.TempSelectedReferralIDs.push($scope.ReferralModel.ReferralSiblingMappings.ReferralID);
                    $scope.ReferralSiblingTokenInputObj.clear();
                    $scope.ReferralModel.ReferralSiblingMappings = {};
                    $scope.$apply();
                }
            }
        }
        //} else {
        //    $scope.ListofFieldsReferralSibling = "";
        //    var liString = "<li>{0} : {1}</li>";
        //    $scope.ListofFielReferralSiblingds = "<ul>";
        //    if (!$scope.ReferralModel.ReferralSiblingMappings.ReferralID) {
        //        $scope.ListofFieldsReferralSibling += liString.format(window.Missing, window.DXCode);
        //    }
        //    if (!$scope.ReferralModel.ReferralDXCodeMapping.StartDate) {
        //        $scope.ListofFieldsForDxCode += liString.format(window.Missing, window.StartDate);
        //    } 
        //    if (!$scope.ReferralModel.ReferralDXCodeMapping.Precedence) {
        //        $scope.ListofFieldsForDxCode += liString.format(window.Missing, window.Precedence);
        //    }
        //    bootboxDialog(function () {
        //    }, bootboxDialogType.Alert, window.Alert, window.FieldsIncomplete.format($scope.ListofFieldsForDxCode));
        //}
    };

    $scope.ReferralSiblingDetailResultsFormatter = function (item) {
        return "<li id='{0}'>{1} <small><b>{4}:</b> {2}</small><small><br/><b>{5}: </b>{3}</small> <small><b style='color:#ad0303;'>{6}:</b> {7}</small><br/><small><b style='color:#ad0303;'>{8}:</b> {9}</small> <small><b style='color:#ad0303;'>{10}:</b> {11}</small> <br/><small><b style='color:#ad0303;'>{12}:</b> {13}</small></li>"
            .format(
                window.Name,
                item.Name,
                item.AHCCCSID ? item.AHCCCSID : 'N/A',
                item.CISNumber ? item.CISNumber : 'N/A',
                window.AHCCCSID,
                window.CISNumberShort,
                window.ParentName,
                item.ParentName ? item.ParentName : 'N/A',
                window.Email,
                item.Email ? item.Email : 'N/A',
                window.Phone,
                item.Phone1 ? item.Phone1 : 'N/A',
                window.Address,
                item.Address ? item.Address : 'N/A'
            );
    };

    $scope.ReferralSiblingTokenFormatter = function (item) {
        return "<li id='{0}'>{0}</li>".format(item.Name);
    };

    $scope.DeleteReferralSiblingMapping = function (tempObject, title) {
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = angular.toJson({
                    referralSiblingMappingId: tempObject.ReferralSiblingMappingID
                });
                if (tempObject.ReferralSiblingMappingID > 0 && tempObject.ReferralSiblingMappingID !== "" && tempObject.ReferralSiblingMappingID !== undefined) {
                    AngularAjaxCall($http, SiteUrl.DeleteReferralSiblingMappingURL, jsonData, "Post", "json", "application/json").success(function (response) {
                        if (response.IsSuccess) {
                            $scope.ReferralModel.ReferralSiblingMappingList.remove(tempObject);
                            $scope.TempSelectedReferralIDs.remove(tempObject.ReferralID2);
                        }
                    });
                } else {
                    $scope.ReferralModel.ReferralSiblingMappingList.remove(tempObject);
                    $scope.TempSelectedReferralIDs.remove(tempObject.ReferralID2);
                    $scope.TempSelectedReferralIDs.remove(tempObject.ReferralID1);
                    $scope.TempSelectedReferralIDs.remove(tempObject.ReferralID);

                    $timeout(function () {
                        $scope.$apply();
                    });
                }
            }
        }, bootboxDialogType.Confirm, title, window.DeleteReferralSiblingConfirmationMessage, bootboxDialogButtonText.HardDelete, btnClass.BtnDanger);
    };

    //#endregion

    //#region 
    //CaseManager Related Code
    $scope.CaseManagerResultsFormatter = function (item) {
        let cell = item.Cell ? item.Cell : window.NA;
        let email = item.Email ? item.Email : window.NA;
        return "<li id='{0}'>{0}<br /><small><b style='color:#ad0303;'>{1}: </b>{2}</small> <small><b style='color:#ad0303;padding-left:10px;'>{3}: </b>{4} </small></li >"
            .format(item.Name, window.Email, email, window.Phone, cell);
    };
    $scope.CaseManagerTokenFormatter = function (item) {
        return "<li id='{0}'><b>CM:</b> {0}</li>".format(item.Name);
    };
    $scope.RemoveCaseManager = function () {
        $scope.ReferralModel.Referral.AgencyID = null;
        $scope.ReferralModel.Referral.CaseManagerID = null;
        $scope.ReferralModel.CaseManager.Email = '';
        $scope.ReferralModel.CaseManager.Phone = '';
        $scope.ReferralModel.CaseManager.Fax = '';
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };
    $scope.CaseManagerAdded = function (item) {
        $scope.ReferralModel.Referral.AgencyID = item.AgencyID;
        $scope.ReferralModel.Referral.CaseManagerID = item.CaseManagerID;
        $scope.ReferralModel.CaseManager.Email = item.Email;
        $scope.ReferralModel.CaseManager.Phone = item.Cell;
        $scope.ReferralModel.CaseManager.Fax = item.Fax;

        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };
    //#endregion

    //#region AudtLog
    //window.PageSize = 20;
    $scope.newAuditInstance = function () {
        var auditInst = $.parseJSON($("#hdnSetAuditPageModel").val());
        return auditInst ? auditInst : { SearchRefAuditLogListModel: null };
    };

    $scope.SearchRefAuditLogListModel = $scope.newAuditInstance().SearchRefAuditLogListModel;

    $scope.TempSearchRefAuditLogListModel = $scope.newAuditInstance().SearchRefAuditLogListModel;

    $scope.AuditLogListPager = new PagerModule("AuditLogID", null, 'DESC');

    $scope.SetPostData = function (fromIndex, model) {
        var pagermodel = {
            searchModel: $scope.SearchRefAuditLogListModel,
            pageSize: $scope.AuditLogListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.AuditLogListPager.sortIndex,
            sortDirection: $scope.AuditLogListPager.sortDirection,
            sortIndexArray: $scope.AuditLogListPager.sortIndexArray.toString()
        };

        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchRefAuditLogListModel = $.parseJSON(angular.toJson($scope.TempSearchRefAuditLogListModel));
    };

    $scope.ResetSearchFilter = function () {
        $scope.SearchRefAuditLogListModel = $scope.newAuditInstance().SearchRefAuditLogListModel;
        $scope.TempSearchRefAuditLogListModel = $scope.newAuditInstance().SearchRefAuditLogListModel;
        $scope.AuditLogListPager.currentPage = 1;
        $scope.AuditLogListPager.getDataCallback();
    };

    $scope.SearchRefAudit = function () {
        $scope.AuditLogListPager.currentPage = 1;
        $scope.AuditLogListPager.getDataCallback(true);
    };

    $scope.AuditLogList = [];
    $scope.SearchAuditLogListPage = {};
    $scope.AjaxStart = true;
    $scope.GetAuditLogList = function (isSearchDataMappingRequire) {
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        $scope.SearchRefAuditLogListModel.EncryptedReferralID = $scope.EncryptedReferralID;

        var jsonData = $scope.SetPostData($scope.AuditLogListPager.currentPage);
        $scope.AjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralAuditLogListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                //$scope.AuditLogList.splice(0, $scope.AuditLogList.length);// = [];
                $scope.AuditLogList = response.Data.Items;
                $scope.AuditLogListPager.currentPageSize = response.Data.Items.length;
                $scope.AuditLogListPager.totalRecords = response.Data.TotalItems;

                if (!$scope.$root.$$phase) {
                    $scope.$apply();
                }

            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });

    };

    $scope.AuditLogListPager.getDataCallback = $scope.GetAuditLogList;

    $scope.AuditView = function (data) {
        $scope.AuditViewDetail = $.parseJSON(JSON.stringify(data));
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }

    };

    $scope.ReferralDxCodeMappingList1 = function () {
        var jsonData = angular.toJson({ RefID: $scope.ReferralID });
        AngularAjaxCall($http, HomeCareSiteUrl.DxCodeMappingList1URL, jsonData, "Post", "json", "application/json", true).
            success(function (response) {
                $scope.mappingList = response.Data
            });
    }

    $("#addReferralDetails_DxCodeDetails").on('shown.bs.tab', function (e) {
        $scope.ReferralDxCodeMappingList1();
    })


    $('#AuditView').on('shown.bs.modal', function () {
        $(".CallGetTableDisplayValue").click();
    });

    //$("#addReferralDetails_DxCodeDetails").on('shown.bs.tab', function (e) {
    //    $scope.ReferralDxCodeMappingList1();
    //});

    // Medication operations starts
    $scope.ReferralID = $scope.ReferralModel.Referral.ReferralID;
    $scope.ShowMedicationList = false;
    $scope.ReferralMedication = { IsActive: true };
    $scope.SaveMedication = function () {

        var isValid = CheckErrors($("#frmAddMedication"));
        if (isValid) {
            var jsonData = angular.toJson({
                Medication: {
                    MedicationName: $scope.ReferralModel.AddMedicationModel.MedicationName,
                    Generic_Name: $scope.ReferralModel.AddMedicationModel.Generic_Name,
                    Brand_Name: $scope.ReferralModel.AddMedicationModel.Brand_Name,
                    Product_Type: $scope.ReferralModel.AddMedicationModel.Product_Type,
                    Route: $scope.ReferralModel.AddMedicationModel.Route,
                    Dosage_Form: $scope.ReferralModel.AddMedicationModel.Dosage_Form
                }
            });

            AngularAjaxCall($http, HomeCareSiteUrl.SaveMedicationURL, jsonData, "Post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    //ShowMessages(response.Message);
                    $("#txtSearchMedication").val($scope.ReferralModel.AddMedicationModel.MedicationName);
                    $("#hdnMedicationId").val(response.Data);
                    $('#AddNewMedication').modal('hide');
                    $scope.ReferralModel.AddMedicationModel = null;
                    toastr.success('Medication saved successfully');
                });
        }
    };
    $scope.SaveReferralMedication = function () {
        var isValid = CheckErrors($("#frmReferralMedication"));
        var StartDate = $scope.ReferralMedication.StartDate;
        var EndDate = $scope.ReferralMedication.EndDate;
        $scope.ReferralMedication.DosageTime = ($scope.ReferralMedication.DosageTime !== undefined && $scope.ReferralMedication.DosageTime !== "")
            ? $scope.ReferralMedication.DosageTime.join(",") : '';
        if (StartDate > EndDate) {
            isValid = false;
            toastr.error("Start date should be less than End date");
        }

        if (isValid) {
            var jsonData = angular.toJson({
                ReferralMedication: {
                    ReferralID: $scope.ReferralID,
                    //MedicationId: $("#hdnMedicationId").val(),
                    MedicationId: $scope.ReferralMedication.MedicationId,
                    // PhysicianID: $("#SearchPhysicianToken").val(),
                    PhysicianID: $scope.ReferralMedication.PhysicianID,
                    Dose: $scope.ReferralMedication.Dose,
                    Frequency: $scope.ReferralMedication.Frequency,
                    Route: $scope.ReferralMedication.Route[0],
                    Unit: $scope.ReferralMedication.Unit,
                    DosageTime: $scope.ReferralMedication.DosageTime,
                    Quantity: $scope.ReferralMedication.Quantity,
                    StartDate: $scope.ReferralMedication.StartDate,
                    EndDate: $scope.ReferralMedication.EndDate,
                    IsActive: $scope.ReferralMedication.IsActive, //true,
                    // HealthDiagnostics: $scope.ReferralMedication.HealthDiagnostics,
                    HealthDiagnostics: $scope.ReferralMedication.DXCodeID,
                    DosageTime: $scope.ReferralMedication.DosageTime,
                    PatientInstructions: $scope.ReferralMedication.PatientInstructions,
                    PharmacistInstructions: $scope.ReferralMedication.PharmacistInstructions,
                    AddMedicationModel: {
                        MedicationName: $scope.ReferralMedication.AddMedicationModel != undefined ? $scope.ReferralMedication.AddMedicationModel.MedicationName : $scope.ReferralMedication.SearchMedicationResult,
                        Generic_Name: $scope.ReferralMedication.AddMedicationModel != undefined ? $scope.ReferralMedication.AddMedicationModel.MedicationName : $scope.ReferralMedication.SearchMedicationResult,
                        Brand_Name: $scope.ReferralMedication.AddMedicationModel != undefined ? $scope.ReferralMedication.AddMedicationModel.Brand_Name : null,
                        Product_Type: $scope.ReferralMedication.AddMedicationModel != undefined ? $scope.ReferralMedication.AddMedicationModel.Product_Type : null,
                        Route: $scope.ReferralMedication.AddMedicationModel != undefined ? $scope.ReferralMedication.AddMedicationModel.Route : $scope.ReferralMedication.Route[0],
                        Dosage_Form: $scope.ReferralMedication.AddMedicationModel != undefined ? $scope.ReferralMedication.AddMedicationModel.Dosage_Form : $scope.ReferralMedication.Dose
                    }
                }
            });

            AngularAjaxCall($http, HomeCareSiteUrl.SaveReferralMedicationURL, jsonData, "Post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        $scope.ReferralMedication = [];
                        $scope.ReferralMedication.IsActive = true;
                        $scope.GetReferralMedication(true);
                        $scope.GetReferralMedication(false);
                        $scope.PhysicianTokenInputObj.clear();
                        $scope.clearTextSearchMedication();
                        $("#SelectedDosageTime").val('').selectpicker("refresh");
                        //ShowMessages(response);
                    }
                    else {
                        //ShowMessages(response);
                    }
                });
        }
    };

    $scope.SaveReferralMedication1 = function () {
        var isValid = CheckErrors($("#frmReferralMedication"));
        var StartDate = $scope.ReferralMedication.StartDate;
        var EndDate = $scope.ReferralMedication.EndDate;
        if (StartDate > EndDate) {
            isValid = false;
            toastr.error("Start date should be less than End date");
        }
        if (isValid) {
            //$scope.MedicationId = $("#hdnMedicationId").val(); $scope.ReferralMedication.MedicationId;

            $scope.ReferralMedication.DosageTime = ($scope.ReferralMedication.DosageTime !== "" && $scope.ReferralMedication.DosageTime !== undefined) ?
                $scope.ReferralMedication.DosageTime.join(",") : '';

            var jsonData = angular.toJson({
                ReferralMedication: {
                    ReferralID: $scope.ReferralID,
                    //MedicationId: $("#hdnMedicationId").val(),
                    MedicationId: $scope.ReferralMedication.MedicationId,
                    PhysicianID: $("#SearchPhysicianToken").val(),
                    Dose: $scope.ReferralMedication.Dose,
                    Frequency: $scope.ReferralMedication.Frequency,
                    Route: $scope.ReferralMedication.Route,
                    Unit: $scope.ReferralMedication.Unit,
                    Quantity: $scope.ReferralMedication.Quantity,
                    StartDate: $scope.ReferralMedication.StartDate,
                    EndDate: $scope.ReferralMedication.EndDate,
                    IsActive: $scope.ReferralMedication.IsActive,
                    HealthDiagnostics: $scope.ReferralMedication.HealthDiagnostics,
                    PatientInstructions: $scope.ReferralMedication.PatientInstructions,
                    PharmacistInstructions: $scope.ReferralMedication.PharmacistInstructions,
                    ReferralMedicationID: $scope.ReferralMedication.ReferralMedicationID,
                    DosageTime: $scope.ReferralMedication.DosageTime
                }
            });

            AngularAjaxCall($http, HomeCareSiteUrl.SaveReferralMedicationURL, jsonData, "Post", "json", "application/json", true).
                success(function (response, status, headers, config) {

                    if (response.IsSuccess) {
                        $scope.ReferralMedication = [];
                        $scope.ReferralMedication.IsActive = true;
                        $scope.GetReferralMedication(true);
                        $scope.GetReferralMedication(false);
                        $scope.PhysicianTokenInputObj.clear();
                        $scope.clearTextSearchMedication();
                        $("#SelectedDosageTime").val('').selectpicker("refresh");
                        $scope.ReferralMedication.SearchMedicationResult = '';
                        //ShowMessages(response);
                    }
                    else {
                        //ShowMessages(response);
                    }
                });
        }
    };

    $scope.CancelReferralMedication = function () {
        $scope.ReferralMedication = [];
        $scope.ReferralMedication.IsActive = true;
        $scope.GetReferralMedication(true);
        $scope.PhysicianTokenInputObj.clear();
        $scope.clearTextSearchMedication();
        $("#SelectedDosageTime").val('').selectpicker("refresh");
    }
    $scope.MedicationList = [];
    $scope.ReferralMedications = [];
    $scope.Show = false;

    $scope.GetReferralMedication = function (isActive) {
        $scope.Show = true;
        var jsonData = angular.toJson({
            ReferralMedication: {
                ReferralID: $scope.ReferralModel.Referral.ReferralID,
                IsActive: isActive
            }
        });
        $scope.AjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralMedicationURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                if (isActive) {
                    $scope.ActiveReferralMedications = response.Data;
                }
                else {
                    $scope.InActiveReferralMedications = response.Data;
                }
            }
            $scope.AjaxStart = false;
        });
    };

    $scope.OpenReferralMedicationDetails = function (event, refMedId, isActive) {
        var contentMsg = 'No details found';
        var refMedDetails = [];
        if (refMedId && refMedId.length > 0) {
            if (isActive) {
                refMedDetails = $scope.ActiveReferralMedications.filter(function (item) {
                    return item.ReferralMedicationID == refMedId;
                });
            }
            else {
                refMedDetails = $scope.InActiveReferralMedications.filter(function (item) {
                    return item.ReferralMedicationID == refMedId;
                });
            }
            contentMsg = '<ul class="noListType">';
            contentMsg = contentMsg + '<li>Health Diagnostics ' + refMedDetails.HealthDiagnostics + '</li>'
            contentMsg = contentMsg + '<li>Patient Instructions ' + refMedDetails.PatientInstructions + '</li>'
            contentMsg = contentMsg + '<li>Pharmacis Instructions ' + refMedDetails.PharmacistInstructions + '</li>'
            contentMsg = contentMsg + '</ul>';
        }

        $(event.target).webuiPopover({ content: contentMsg, animation: 'pop' });
        $(event.target).click();
    }

    $scope.GetMedicationsList = [];

    $scope.SearchReferralMedication = function (Search) {
        $scope.GetMedicationsList = [];
        var url = "https://api.fda.gov/drug/ndc.json?search=brand_name_base:<<drugname>>&limit=25";
        url = url.replace("<<drugname>>", Search);
        var jsonData = angular.toJson({});
        AngularAjaxCall($http, url, jsonData, "Get", "json", "application/json").success(function (response) {
            $scope.GetMedicationsList = response.results;
        }).error(function (response) {
            $scope.GetMedicationsList = [];
        });
        $scope.ShowMedicationList = false;
    };

    //$scope.SearchDBReferralMedication = function () {
    //    alert('sddfdf');
    //    debugger
    //    $scope.GetMedicationsList1 = []
    //     var jsonData = angular.toJson({ Search: 'ta'});
    //     AngularAjaxCall($http, HomeCareSiteUrl.SearchReferralMedicationsURL, jsonData, "Get", "json", "application/json").success(function (response) {
    //            $scope.GetMedicationsList1 = response.results;
    //    }).error(function (response) {
    //        $scope.GetMedicationsList1 = [];
    //    });
    //    //$scope.ShowMedicationList = false;
    //};

    $scope.ClickReferralMedication = function (item) {
        $scope.ReferralModel.AddMedicationModel = [];
        $scope.ReferralModel.AddMedicationModel.MedicationName = item.generic_name;
        $scope.ReferralModel.AddMedicationModel.Generic_Name = item.generic_name;
        $scope.ReferralModel.AddMedicationModel.Brand_Name = item.brand_name;
        $scope.ReferralModel.AddMedicationModel.Product_Type = item.product_type;
        $scope.ReferralModel.AddMedicationModel.Route = item.route;
        $scope.ReferralModel.AddMedicationModel.Dosage_Form = item.dosage_form;
        $scope.IsMedicationDisabled = true;
        $scope.OpenAddMedicationModal();

        $('#model_AddMedication').modal({
            backdrop: 'static',
            keyboard: false
        });
        $scope.ReferralMedication.SearchMedicationResult = '';
        $scope.ShowMedicationList = false;
        angular.element('#txtSearchMedication').addClass("display");
        angular.element('#glyphicon-remove').addClass("glyphicon-remove");
    }
    $scope.clearTextSearchMedication = function () {
        $scope.ReferralMedication.SearchMedicationResult = '';
        //$scope.ReferralMedication.SearchMedicationResult = '';
        $scope.ShowMedicationList = false;
        angular.element("#hdnMedicationId").val("");
        angular.element('#txtSearchMedication').removeClass("display");
    }

    $scope.OpenAddMedicationModal = function () {
        $scope.GetMedicationsList = [];
        $scope.GetMedicationsList1 = [];
        $scope.GetMedicationsList2 = [];
        $scope.ShowMedicationList = false;
        var Search = angular.element("#txtSearchMedication").val();
        if (Search != "") {
            var searchText = Search;
            var jsonData = angular.toJson({ Search: searchText });
            AngularAjaxCall($http, HomeCareSiteUrl.SearchReferralMedicationsURL, jsonData, "Post", "json", "application/json").success(function (response) {
                $scope.GetMedicationsList1 = response.Data;
                $scope.GetMedicationsList = angular.extend($scope.GetMedicationsList, $scope.GetMedicationsList1);
                $scope.ShowMedicationList = true;

            }).error(function (response) {
                $scope.GetMedicationsList = [];
            });

            if ($scope.GetMedicationsList.length < 1) {
                var url = "https://api.fda.gov/drug/ndc.json?search=brand_name_base:<<drugname>>&limit=10";
                url = url.replace("<<drugname>>", Search);
                var jsonData = angular.toJson({});
                AngularAjaxCall($http, url, jsonData, "Get", "json", "application/json").success(function (response) {
                    $scope.ShowMedicationList = true;
                    $scope.GetMedicationsList = response.results;
                });
            }
        }
        else {
            toastr.error("Please enter search item");
        }
    };


    $scope.SelectMedication = function (selectedMedication) {
        $scope.ReferralMedication.AddMedicationModel = [];
        $scope.ReferralMedication.AddMedicationModel.MedicationName = selectedMedication.generic_name;
        $scope.ReferralMedication.AddMedicationModel.Generic_Name = selectedMedication.generic_name;
        $scope.ReferralMedication.AddMedicationModel.Brand_Name = selectedMedication.brand_name;
        $scope.ReferralMedication.AddMedicationModel.Product_Type = selectedMedication.product_type;
        $scope.ReferralMedication.AddMedicationModel.Route = selectedMedication.route;
        $scope.ReferralMedication.AddMedicationModel.Dosage_Form = selectedMedication.dosage_form;
        $scope.ReferralMedication.Route = selectedMedication.route;
        if (selectedMedication.active_ingredients != undefined) {
            $scope.ReferralMedication.Dose = selectedMedication.active_ingredients[0].strength.split(' ')[0];
            $scope.ReferralMedication.Unit = selectedMedication.active_ingredients[0].strength.split(' ').pop();
        }
        $scope.ReferralMedication.SearchMedicationResult = selectedMedication.generic_name;
        angular.element('#cardMedicationName').text(selectedMedication.generic_name);
        angular.element('#cardBrandName').text(selectedMedication.brand_name);
        angular.element('#cardRoute').text(selectedMedication.route);
        angular.element('#cardDosageForm').text(selectedMedication.dosage_form);
    };
    $scope.DeleteReferralMedication = function (item) {
        bootboxDialog(function (result) {
            if (result) {
                $scope.ReferralMedicationID = item.ReferralMedicationID;
                var jsonData = angular.toJson({ ReferralMedicationID: $scope.ReferralMedicationID });

                AngularAjaxCall($http, HomeCareSiteUrl.DeleteReferralMedicationURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.GetReferralMedication(true);
                        $scope.GetReferralMedication(false);
                    }
                    ShowMessages(response);
                });
                $scope.GetReferralMedication(true);
                $scope.GetReferralMedication(false);
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteNoteMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };
    $scope.OpenAddNewMedication = function () {
        $('#AddNewMedication').modal({ backdrop: 'static', keyboard: false });
    }

    $scope.EditReferralMedication = function (item) {
        $scope.ReferralMedication = item;
        $scope.ReferralMedication.IsActive = item.IsActive;
        $scope.ReferralMedication.SearchMedicationResult = item.MedicationName;
        $scope.ReferralMedication.PhysicianID = item.PhysicianID;
        $scope.ReferralMedication.DXCodeID = item.HealthDiagnostics;
        $scope.ReferralMedication.DosageTime = (item.DosageTime !== undefined && item.DosageTime !== "")
            ? item.DosageTime.split(',') : [];

        $('#SelectedDosageTime').selectpicker('val', $scope.ReferralMedication.DosageTime).trigger("change");

        //  $scope.ReferralMedication.PhysicianName = item.PhysicianName;
        //SearchPhysicianToken: item.phys;
        //$scope.ReferralMedicationID = item.ReferralMedicationID;
        //var jsonData = angular.toJson({ ReferralMedicationID: $scope.ReferralMedicationID });

        //AngularAjaxCall($http, HomeCareSiteUrl.EditReferralMedicationURL, jsonData, "Post", "json", "application/json").success(function (response) {
        //    ShowMessages(response);
        //    if (response.IsSuccess) {
        //        $scope.ReferralMedication = response.Data;
        //        $scope.ReferralMedication.SearchMedicationResult = response.Data.MedicationName;
        //        txtSearchMedication: response.Data.MedicationId;
        //    }
        //    ShowMessages(response);
        //});

    }
    // Medication operations end
    $scope.GetTableDisplayValue = function (refItem) {
        if (refItem.TableName && !refItem.IsLoaded) {
            refItem.LoadInProgress = true;
            var jsonData = angular.toJson({ model: refItem });
            AngularAjaxCall($http, SiteUrl.GetTableDisplayValue, jsonData, "Post", "json", "application/json", false).success(function (response) {
                if (response.IsSuccess) {
                    if (response.Data != null) {
                        refItem.ValueBefore = response.Data.ValueBefore != null ? response.Data.ValueBefore : refItem.ValueBefore;
                        refItem.ValueAfter = response.Data.ValueAfter != null ? response.Data.ValueAfter : refItem.ValueAfter;
                        //refItem.ValueBefore = response.Data.ValueBefore != null ? response.Data.ValueBefore + ' (#' + refItem.ValueBefore + ')' : refItem.ValueBefore;
                        //refItem.ValueAfter = response.Data.ValueAfter != null ? response.Data.ValueAfter + ' (#' + refItem.ValueAfter + ')' : refItem.ValueAfter;
                    }
                    refItem.IsLoaded = true;
                }
                refItem.LoadInProgress = false;
            });
        }

        //if (data)
        //$scope.AuditViewDetail = $.parseJSON(JSON.stringify(data));
    };



    $("a[data-toggle='tab']").on('shown.bs.tab', function (e) {

        if (e.delegateTarget.id == 'addreferraldetails_auditlogs') {
            $scope.ShowSubmitActions = false;
            if (e.delegateTarget.id == 'addreferraldetails_auditlogs') $scope.GetAuditLogList();


        } else if (e.delegateTarget.id == 'addReferralDetails_Medication') {
            $scope.GetReferralMedication(true);
            $scope.GetReferralMedication(false);
            $scope.ShowSubmitActions = false;
        } else if (e.delegateTarget.id == 'addReferralDetails_checklistitems') {
            $scope.ShowSubmitActions = false;
        } else if (e.delegateTarget.id == 'addReferralDetails_referralHistory') {
            $scope.GetReferralHistory();
            $scope.ShowReferralHistory = true;
            $scope.ShowSubmitActions = false;
        } else if (e.delegateTarget.id == 'addReferralDetails_DxCodeDetails') {
            $scope.ShowSubmitActions = false;
        }
        //else if (e.delegateTarget.id == 'addReferralDetails_PhysicianDetails') {
        //    $scope.ShowSubmitActions = false;
        //}
        else if (e.delegateTarget.id == 'addReferralDetails_Allergy') {
            $scope.ShowSubmitActions = false;
        }
        else {
            $scope.ShowSubmitActions = true;
            $scope.ShowReferralHistory = false;
            $scope.Show = false;
        }
        if (!$scope.$root.$$phase) $scope.$apply();

    });


    //#endregion AuditLog


    //#region

    $('#UpdateAHCCCSID').on('hidden.bs.modal', function () {
        $scope.ReferralModel.ReferralAhcccsDetails = $scope.newInstance().ReferralAhcccsDetails;
    });

    $scope.UpdateAHCCCSIDView = function () {
        $scope.ReferralModel.ReferralAhcccsDetails = $scope.newInstance().ReferralAhcccsDetails;
    };

    $scope.IsValidAHCCCS = true;
    $scope.UpdateAHCCCSID = function () {

        var ele = $("input#NewAHCCCSID");

        var ahcccsRegExCheck = new RegExp(window.RegxClientAhcccsId);
        if (ValideElement(ele.val()) === false || ahcccsRegExCheck.test(ele.val()) === false)
            $scope.IsValidAHCCCS = false;
        else $scope.IsValidAHCCCS = true;

        if ($scope.IsValidAHCCCS) {
            $scope.ReferralModel.ReferralAhcccsDetails.AHCCCSID = $scope.ReferralModel.Referral.AHCCCSID;
            $scope.ReferralModel.ReferralAhcccsDetails.ReferralID = $scope.ReferralModel.Referral.ReferralID;
            var jsonData = angular.toJson({
                model: $scope.ReferralModel.ReferralAhcccsDetails,
                referral: $scope.ReferralModel.Referral
            });
            AngularAjaxCall($http, HomeCareSiteUrl.UpdateAccountURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $('#UpdateAHCCCSID').modal('hide');
                    $scope.ReferralModel.Referral.AHCCCSID = $scope.ReferralModel.ReferralAhcccsDetails.NewAHCCCSID;
                }
                ShowMessages(response);
            });
        }
    };

    //#endregion
    //$scope.ListPreference = [];
    $scope.PreferenceTokenObj = {};
    $scope.SearhSkillURL = HomeCareSiteUrl.SearhSkillURL;

    $scope.PreferenceResultsFormatter = function (item) {
        return "<li id='{0}'>{0}</li>".format(item.PreferenceName);
    };
    $scope.PreferenceTokenFormatter = function (item) {
        return "<li id='{0}'>{0}</li>".format(item.PreferenceName);
    };

    $scope.AddedPreference = function (item) {
        //$scope.ListPreference.push(item);
        //$scope.PreferenceTokenObj.clear();

        var push = true;
        if (_.findWhere($scope.ListPreference, item) == null) {
            angular.forEach($scope.ListPreference, function (items) {
                if (items.PreferenceName == item.PreferenceName) {
                    push = false;
                }
            });
            if (push) {
                $scope.ListPreference.push(item);
            }
        }
        $scope.PreferenceTokenObj.clear();
    };

    $scope.DeletePreference = function (item, index) {
        if (item.EmployeePreferenceID == 0) {
            $scope.ListPreference.splice(index, 1);
        } else {
            var jsonData = angular.toJson({ model: item });
            AngularAjaxCall($http, HomeCareSiteUrl.DeleteReferralPreferenceURL, jsonData, "Post", "json", "application/json", false).
                success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.ListPreference.splice(index, 1);
                    }
                });
        }
    };

    //#region Location Auto Completer

    if ($scope.ReferralModel.Referral.RegionID != null) {
        {
            $timeout(function () {
                $scope.RegionTokenObj.add(
                    {
                        RegionID: $scope.ReferralModel.Referral.RegionID,
                        RegionName: $scope.ReferralModel.Referral.RegionName
                    });
            });
        }
    }

    $scope.RegionTokenObj = {};
    $scope.SearhRegionURL = HomeCareSiteUrl.SearhRegionURL;

    $scope.RegionResultsFormatter = function (item) {
        return "<li id='{0}'>{1}</li>".format(item.RegionID, item.RegionName);
    };
    $scope.RegionTokenFormatter = function (item) {
        $scope.ReferralModel.Referral.RegionName = item.RegionName;
        return "<li id='{0}'>{1}</li>".format(item.RegionID, item.RegionName);
    };
    $scope.RemoveRegion = function () {
        $scope.ReferralModel.Referral.RegionID = null;
        $scope.RegionTokenObj.clear();
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };
    //#endregion

    //$scope.GenerateGeoCode = function (isFirstTimeLoad, callback, errorCallback) {
    //    if (isFirstTimeLoad === false && (!ValideElement($scope.EmployeeModel.Employee.Address) ||
    //        !ValideElement($scope.EmployeeModel.Employee.City) ||
    //        !ValideElement($scope.EmployeeModel.Employee.ZipCode) ||
    //        !ValideElement($scope.EmployeeModel.Employee.StateCode))) {
    //        toastr.error(window.AddressMissingMessage);
    //        return false;
    //    }


    //    var address = $scope.EmployeeModel.Employee.Address + ',' + $scope.EmployeeModel.Employee.City
    //        + '-' + $scope.EmployeeModel.Employee.ZipCode + ',' + $scope.EmployeeModel.Employee.StateCode;
    //    //var mapElement = "gmap";
    //    GetGeoCodeAndMapFromAddress(address, undefined, function (lat, long) {
    //        $scope.EmployeeModel.Employee.Latitude = lat;
    //        $scope.EmployeeModel.Employee.Longitude = long;
    //        //$("#" + mapElement).css({ 'height': '300px', 'width': '100%' });
    //        if (!$scope.$root.$$phase) {
    //            $scope.$apply();
    //        }


    //        if (callback && ValideElement(lat) && ValideElement(long)) {
    //            callback();
    //        }
    //        else {
    //            if (errorCallback)
    //                errorCallback();
    //                errorCallback();
    //        }


    //    });
    //};
    //$scope.MedicationList = [];
    //var url = "https://api.fda.gov/drug/ndc.json"
    //$scope.GetMedication = function () {
    //    debugger;
    //    var jsonData = angular.toJson({  });
    //    AngularAjaxCall($http, url, jsonData, "Post", "json", "application/json").success(function (response) {
    //        ShowMessages(response);
    //        if (response.IsSuccess) {
    //            $scope.MedicationList = response.Data;
    //        }
    //    });
    //};
    //$scope.GetMedication();

    $scope.Medication = function () {
        //     var isValid = CheckErrors($("#frmNewReferralNote"));
        $scope.EncryptedReferralID = $scope.EncryptedReferralID;
        var jsonData = angular.toJson({
            RoleID: $scope.RoleID,
            EmployeesID: $scope.EmployeesID,
            EncryptedReferralID: $scope.EncryptedReferralID,
            EncryptedEmployeeID: $scope.EncryptedEmployeeID,
            TypeOfMedication: $scope.TypeOfMedication,
            PrescribedBy: $scope.PrescribedBy,
            PharmacyName: $scope.PharmacyName,
            Amount: $scope.Amount,
            Frequency: $scope.Frequency,
            GivenFor: $scope.GivenFor,
            Instruction: $scope.Instruction,
            Dosage: $scope.Dosage,
            Route: $scope.Route,
            TimeofDay: $scope.TimeofDay,
            Precautions: $scope.Precautions,
            DosageTime: $scope.DosageTime,
        });
        AngularAjaxCall($http, HomeCareSiteUrl.SaveReferralMedicationURL, jsonData, "Post", "json", "application/json").success(function (response) {
            $scope.IsEdit = false;
            ShowMessages(response);
        });
    }
    //Get Diagnosis code from API Start
    $scope.GetICD10CodeList = [];
    //$scope.SearchGetICD10Code = function (Search) {
    //    $scope.Search1 = '';
    //    var BaseUrl = "https://www.hipaaspace.com/api/icd10/search?q=";
    //    var token = "&token=3932f3b0-cfab-11dc-95ff-0800200c9a663932f3b0-cfab-11dc-95ff-0800200c9a66";
    //    var url = BaseUrl + Search + token
    //    $http.get(url)
    //        .then(function (response) {
    //            $scope.GetICD10CodeList = response.data.ICD10;
    //        });

    //}
    $scope.SearchGetICD10Code = function (Search) {
        var jsonData = angular.toJson({ searchText: Search, ignoreIds: '', pageSize: 50 });
        AngularAjaxCall($http, HomeCareSiteUrl.GetDXCodeListForAutoCompleteURLs, jsonData, "Post", "json", "application/json").success(function (response) {
            $scope.GetICD10CodeList = response;
            $scope.Search1 = true;
        });
    }

    $scope.DxCodeMappingList = []
    $scope.Click = function (item) {
        var jsonData = angular.toJson({ DXCodeName: item.DXCodeName, DXCodeWithoutDot: item.DXCodeWithoutDot, DxCodeType: item.DxCodeType, Description: item.Description, DxCodeShortName: item.DxCodeShortName });
        AngularAjaxCall($http, HomeCareSiteUrl.SaveDxCodeURLs, jsonData, "Post", "json", "application/json", false).success(function (response) {
            $scope.ReferralModel.ReferralDXCodeMapping = response.Data;
            ShowMessages(response);
            //$scope.ReferralModel.ReferralDXCodeMapping.DXCodeName = null;
            //$scope.ReferralModel.ReferralDXCodeMapping.DXCodeWithoutDot = null;
            //$scope.ReferralModel.ReferralDXCodeMapping.Description = null;
            //$scope.ReferralModel.ReferralDXCodeMapping.DxCodeShortName = null;
            if ($scope.ReferralModel.Precedence > 0) {
                $scope.ReferralModel.ReferralDXCodeMapping.Precedence = $scope.ReferralModel.Precedence;
            }
        });
        if (item.DXCodeName == null) {
            $scope.Search = item.DXCodeWithoutDot;
        }
        else {
            $scope.Search = item.DXCodeName;
        }
        $scope.Search1 = false;
        angular.element('#display').addClass("display");
        angular.element('#glyphicon-remove').addClass("glyphicon-remove");
    }
    $scope.test = function () {
        $scope.DxCodeMappingList = $scope.ReferralModel.DxCodeMappingList;
    }
    $scope.test();
    $scope.clearText = function () {
        $scope.Search = null;
        $scope.Search1 = false;
        $scope.ReferralModel.ReferralDXCodeMapping = null;
        angular.element('#display').removeClass("display");
    }
    //Get Diagnosis code from API End
    $scope.Sorting = function (index) {
        $scope.DxCodeMappingList.splice(index, 1);
        var model = [];
        var RefID = $scope.ReferralID;
        angular.forEach($scope.DxCodeMappingList, function (val, index) {
            val.Precedence = index + 1;
            model.push(val);
        })
        var jsonData = angular.toJson({ model, RefID });
        $http.post('/hc/Referral/SaveTaskOrder', jsonData).then(function (response) {
            //you can write more code here after save new order 
        }).finally(function () {
            model.IsProcessing = false;
        })
    }

    $scope.GetReferralHistory = function () {
        var jsonData = angular.toJson({
            referralID: $scope.ReferralModel.Referral.ReferralID,
        });
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralHistoryURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.ReferralHistoryList = response.Data;
            }
        });
    };

    $scope.EditReferralHistoryItem = function (data) {
        $scope.ReferralModel.ReferralHistoryItem = angular.copy(data);
        $scope.ReferralModel.ReferralHistoryItem.IsEdit = true;
    };

    $scope.CancelReferralHistoryItem = function () {
        $scope.ReferralModel.ReferralHistoryItem = null;
        HideErrors($("#frmReferralHistory"));
    };

    $scope.SaveReferralHistoryItem = function () {
        var isValid = CheckErrors($("#frmReferralHistory"));

        if (isValid) {
            $scope.ReferralModel.ReferralHistoryItem.ReferralID = $scope.ReferralModel.Referral.ReferralID;
            var date = $scope.ReferralModel.ReferralHistoryItem.CreatedDate;
            $scope.ReferralModel.ReferralHistoryItem.CreatedDate = date ? moment(date).format() : date;
            var jsonData = angular.toJson({
                referralHistoryModel: $scope.ReferralModel.ReferralHistoryItem
            });
            AngularAjaxCall($http, HomeCareSiteUrl.SaveReferralHistoryItemURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.GetReferralHistory();
                    $scope.CancelReferralHistoryItem();
                }
            });
        }
    };

    $scope.DeleteReferralHistoryItem = function (tempObject) {
        bootboxDialog(function (result) {
            if (result) {
                if (tempObject.ReferralHistoryID > 0) {
                    var jsonData = angular.toJson({
                        referralHistoryID: tempObject.ReferralHistoryID
                    });
                    AngularAjaxCall($http, HomeCareSiteUrl.DeleteReferralHistoryItemURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
                        if (response.IsSuccess) {
                            $scope.GetReferralHistory();
                        }
                    });
                }
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteRHConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.CancelNewMedication = function () {
        $('#AddNewMedication').modal('hide');
        $scope.ReferralModel.AddMedicationModel = null;
    }

    $("a#CarePlan").on('shown.bs.tab', function (e) {
        if (isDayCare) {
            $(".tab-pane a[href='#tab_ReferralTimeSlots']").tab('show');
        } else {
            $(".tab-pane a[href='#tab_ReferralTaskMapping']").tab('show');
        }
    });

    $scope.UploadReferralProfileImage = HomeCareSiteUrl.UploadReferralProfileImageUrls;

    $scope.UploadingFileList = [];

    $scope.BeforeSend = function (e, data) {
        var isValidImage = true;
        var fileName;
        var errorMsg;

        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            if (extension !== "jpg" && extension !== "jpeg" && extension !== "png" && extension !== "bmp") {
                ShowMessage(window.InvalidImageUploadMessage, "error");
                isValidImage = false;
            }
            else if ((file.size / 1024) > 2048) {
                //file.FileProgress = 100;
                ShowMessage(window.MaximumUploadImageSizeMessage2MB, "error");
                errorMsg = window.MaximumUploadImageSizeMessage2MB;
                $scope.UploadingFileList = [];
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
    };

    $scope.AfterSend = function (e, data) {
        $scope.IsFileUploading = false;
        var model = data.result;
        if (model.IsSuccess) {
            $scope.ReferralModel.Referral.ProfileImagePath = model.Data.TempFilePath;
            $scope.UploadingFileList = [];
        } else {
            ShowMessage(model);
        }
        $scope.$apply();
    };

    $scope.removeImage = function () {
        $scope.ReferralModel.Referral.ProfileImagePath = '';
        $scope.UploadingFileList = [];
    };

    $scope.MedicationReport = function (ReportName) {
        var Domain = window.Domain;
        var ReportURL1 = 'https://';
        var ReportURL2 = '.myezcare.com';
        var ReportURL3 = "/Report/Template?ReportName=";
        var parameterValue1 = "&ReferralID=";
        var parameterValue2 = $scope.ReferralID;
        var url = ReportURL1 + Domain + ReportURL2 + ReportURL3 + ReportName + parameterValue1 + parameterValue2;
        var width = screen.availWidth - 10;
        var height = screen.availHeight - 60;
        var left = 0;//(screen.availWidth - width) / 2;
        var top = 0;//(screen.availHeight - height) / 2;
        var params = 'width=' + width + ', height=' + height;
        params += ', top=' + top + ', left=' + left;
        var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,' + params;
        var pdfWindow = window.open('about:blank', 'null', winFeature);
        pdfWindow.document.write("<html><head><style> * { box-sizing: border-box; padding: 0; margin: 0; border: 0; }</style>"
            + "<title>" + ReportName + "</title></head><body>"
            + '<embed width="100%" height="100%" name="plugin" src="' + url + '" '
            + 'type="application/pdf" internalinstanceid="21"></body></html>');
        pdfWindow.document.close();

    };

    $scope.MedicationLogReport = function (ReportName) {
        var Domain = window.Domain;
        var ReportURL1 = 'https://';
        var ReportURL2 = '.myezcare.com';
        var ReportURL3 = "/Report/Template?ReportName=";
        var parameterValue1 = "&ReferralID=";
        var parameterValue2 = $scope.ReferralID;
        var url = ReportURL1 + Domain + ReportURL2 + ReportURL3 + ReportName + parameterValue1 + parameterValue2;
        var width = screen.availWidth - 10;
        var height = screen.availHeight - 60;
        var left = 0;//(screen.availWidth - width) / 2;
        var top = 0;//(screen.availHeight - height) / 2;
        var params = 'width=' + width + ', height=' + height;
        params += ', top=' + top + ', left=' + left;
        var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,' + params;
        var pdfWindow = window.open('about:blank', 'null', winFeature);
        pdfWindow.document.write("<html><head><style> * { box-sizing: border-box; padding: 0; margin: 0; border: 0; }</style>"
            + "<title>" + ReportName + "</title></head><body>"
            + '<embed width="100%" height="100%" name="plugin" src="' + url + '" '
            + 'type="application/pdf" internalinstanceid="21"></body></html>');
        pdfWindow.document.close();

    };
};

controllers.AddReferralController.$inject = ['$scope', '$http', '$window', '$timeout'];

$(document).ready(function () {

    var frmReferralControls = "#frmReferral :input:not('.exclude-reload-alert'),#frmReferral select:not('.exclude-reload-alert')";
    var frmAddContactControls = "#frmAddContact :input,#frmAddContact select";
    var frmRefChekListControls = "#frmReferralCheckList :input,#frmReferralCheckList select";
    var frmReferralsparformControls = "#frmreferralsparform :input,#frmreferralsparform select";

    $(frmReferralControls + "," + frmAddContactControls + "," + frmRefChekListControls + "," + frmReferralsparformControls).change(function (ev, data) {
        if ($(ev.currentTarget).parents('.frmAuditLog').length == 0
            && $(ev.currentTarget).parents('.frmAddBXContractView').length == 0
            && $(ev.currentTarget).parents('.frmRefSuspention').length == 0)
            $("#frmReferral").data('changed', true);
    });

    $("#frmReferral :input.dateInputMask,#frmAddContact :input.dateInputMask,#frmReferralCheckList :input.dateInputMask,#frmreferralsparform :input.dateInputMask").on("focusout", function (ev) {
        if (!($(this) != undefined && $(this).hasClass("exclude-reload-alert"))) {
            $("#frmReferral").data('changed', true);
        }
    });

    $(window).bind("beforeunload", function () {
        if ($("#frmReferral").data('changed')) {
            return "Good bye";
        }
    });

    var dateformat = GetOrgDateFormat();
    $(".dateInputMask").attr("placeholder", dateformat);
    $('.time').inputmask({
        mask: "h:s t\\m",
        placeholder: "hh:mm a",
        alias: "datetime",
        hourFormat: "12"
    });

    //$("#AHCCCS_ID,#NewAHCCCSID").inputmask({
    //    mask: "A99999999",
    //    placeholder: "AXXXXXXXX"
    //});

    //$("#Referral_CISNumber").inputmask({
    //    mask: "9999999999",
    //    placeholder: "XXXXXXXXXX"
    //});

    ShowPageLoadMessage("ShowAddReferralMessage");

    $('#model_AddContact').on('hidden.bs.modal', function () {
        if (custModel) custModel.TempContactTypeID = 0;
    });

    CheckTabAndLoad(window.location.hash);
});

function CheckTabAndLoad(hashUrl) {
    if (hashUrl) {
        var hashArray = hashUrl.split("_");
        $.each(hashArray, function (index, data) {

            if (index == 0)
                $("a" + data).tab('show');
            else {

                if (hashArray.length === 2)
                    $("a" + hashUrl).tab('show');
                else {
                    var newHashUrl = hashArray[0] + "_" + hashArray[1];
                    $("a" + newHashUrl).tab('show');
                }
            }
            $(window).scrollTop(0);
        });
    } else {
        $("a#" + model.ReferralModel.DefaultTab).tab('show');
        $(window).scrollTop(0);
    }

    setTimeout(function () {
        $(window).scrollTop(0);
    }, 100);

    $("a[data-toggle=tab]").on('shown.bs.tab', function (e, ui) {
        window.location.hash = $(this).attr('id');
        $(window).scrollTop(0);
    });
}

controllers.ParentReferralController = function ($scope, $http, $window, $timeout) {
    $scope.IsChecklistRemaining = true;
    $scope.ReferralErrorCount = {};
    $scope.ReferralErrorCount.ClientTab = [];
    $scope.ReferralErrorCount.ContactTab = [];
    $scope.ReferralErrorCount.ComplianceTab = [];
    $scope.ReferralErrorCount.ReferralHistoryTab = [];
    $scope.TotalReferralErrorCount = 0;

    $scope.ShowChecklistTab = function () {
        $("a#addReferralDetails").tab('show');
        $("a#addReferralDetails_checklistitems").tab('show');
    }

    $scope.ReferralDetailResultsFormatter = function (item) {
        return "<li id='{0}'>{1}<br/><small><b>{4} #:</b> {2}</small><small><br/><b>{5} #: </b>{3}</small><br/><small><b style='color:#ad0303;'>{6}:</b> {7}</small><br/><small><b style='color:#ad0303;'>{8}:</b> {9}</small></li>"
            .format(
                window.Name,
                item.Name,
                item.AHCCCSID ? item.AHCCCSID : 'N/A',
                item.CISNumber ? item.CISNumber : 'N/A',
                window.Account,
                window.Medicaid,
                window.Phone,
                item.Phone1 ? item.Phone1 : 'N/A',
                window.Address,
                item.Address ? item.Address : 'N/A'
            );
    };

    $scope.ReferralTokenFormatter = function (item) {
        return "<li id='{0}'>{0}</li>".format(item.Name);
    };

    $scope.OnAddReferral = function (item) {
        window.location = HomeCareSiteUrl.AddReferralPageUrl + item.EncryptedReferralID;
        return false;
    };

    $scope.GetIsChecklistRemaining = function () {
        var model = {
            ChecklistItemTypeID: checklistTypePatient,
            EncryptedPrimaryID: window.EncryptedReferralID
        }
        var jsonData = angular.toJson({ model: model });
        AngularAjaxCall($http, HomeCareSiteUrl.GetIsChecklistRemainingURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                if (response.Data == false) {
                    $scope.IsChecklistRemaining = false;
                } else {
                    $scope.IsChecklistRemaining = true;
                }
            }
        });
    };
    //$scope.GetIsChecklistRemaining();

    //#region Token input related code for service code
    $scope.GetReferralsURL = SiteUrl.GetReferralInfoURL;
    //#endregion
};
controllers.ParentReferralController.$inject = ['$scope', '$http', '$window', '$timeout'];

