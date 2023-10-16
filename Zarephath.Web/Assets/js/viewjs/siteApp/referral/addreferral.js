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
    $scope.TempReferralModel = angular.copy($scope.ReferralModel);
    $scope.EncryptedReferralID = $scope.ReferralModel.Referral.EncryptedReferralID;
    $scope.EncryptedIDForZero = window.EncryptedIDForZero;
    $scope.PrimaryPlacementContactTypeID = PrimaryPlacementContactTypeID;
    $scope.LegalGuardianContactTypeID = LegalGuardianContactTypeID;
    $scope.NewDate = SetExpiryDate();
    $scope.DOBValidDate = moment().subtract(18, "years").format(window.DateFormat.toUpperCase());
    $scope.GetCaseManagersURL = SiteUrl.GetCaseManagersURL;


    $scope.LastReferralStatusID = angular.copy($scope.ReferralModel.Referral.ReferralStatusID);
    
    $scope.LoadRefferalCMDetails = function () {
        $timeout(function () {
            if ($scope.ReferralModel.Referral.CaseManagerID > 0) {
                var agencyId = $scope.ReferralModel.Referral.AgencyID;
                var caseManagerId = $scope.ReferralModel.Referral.CaseManagerID;
                var agencyName = $scope.ReferralModel.Referral.AgencyName;
                var cmName = $scope.ReferralModel.Referral.CaseManager;
                var email = $scope.ReferralModel.Referral.Email;
                var phone = $scope.ReferralModel.Referral.Phone;
                var fax = $scope.ReferralModel.Referral.Fax;

                $scope.CaseManagerTokenObj.clear();

                $scope.ReferralModel.Referral.AgencyID = agencyId;
                $scope.ReferralModel.Referral.CaseManagerID = caseManagerId;
                $scope.ReferralModel.Referral.AgencyName = agencyName;
                $scope.ReferralModel.Referral.CaseManager = cmName;

                $scope.CaseManagerTokenObj.add({ AgencyID: $scope.ReferralModel.Referral.AgencyID, CaseManagerID: $scope.ReferralModel.Referral.CaseManagerID, Name: $scope.ReferralModel.Referral.CaseManager, NickName: $scope.ReferralModel.Referral.AgencyName });

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

    $scope.ContactID = null;
    $scope.IsContactEditMode = false;
    $scope.CanAddContact = true;
    $scope.TempContactTypeID = 0;
    $scope.TempIndexNumberofContactList = 0;
    $scope.AddReferralURL = "/referral/addreferral";
    $scope.SetAddReferralPageURL = "/referral/setaddreferralpage";
    $scope.AddContactURL = "/referral/addcontact";
    $scope.GetContactListURL = "/referral/getcontactlist";
    $scope.DeleteContactURL = "/referral/deletecontact";
    $scope.DeleteReferralPayorMappingURL = "/referral/deletereferralpayormapping";
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


    $("a#addReferralDetails").on('shown.bs.tab', function (e, ui) {
        $("#frmReferral").data('changed', false);
        $scope.SetAddReferralPage();
    });

    //#endregion 

    //#region ON SAVE REFERRAL RELATED FUNCATION START

    $scope.$watch('ReferralModel.Referral.ReferralStatusID', function (newValue, oldValue) {
        $scope.ReferralStatusChange(newValue, oldValue);
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

        if ($scope.MissingPC == false && $scope.MissingLG == false)
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

            $scope.ListofFields += liString.format(window.Missing, window.AHCCCSID);
            isValid = false;
        } else {
            var regexAhccccid = /^[a-zA-Z]{1}[0-9]{1,9}$/;
            if (!regexAhccccid.test($scope.ReferralModel.Referral.AHCCCSID)) {
                $scope.ListofFields += liString.format(window.Invalid, window.AHCCCSID);
                isValid = false;
            }
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
                if (isValid) {
                    $scope.ReferralModel.Referral.IsSaveAsDraft = true;
                    $scope.SaveReferral(true);
                } else {
                    bootboxDialog(function () {
                    }, bootboxDialogType.Alert, window.Alert, window.DraftIncomplete.format($scope.ListofFields));
                }
            }
        } else {
            var frmAddBXContractView = "#frmAddBXContractView :input,#frmAddBXContractView textarea,#frmAddBXContractView select";
            var frmRefSuspention = "#frmRefSuspention :input,#frmRefSuspention textarea,#frmRefSuspention select";

            $(frmAddBXContractView + "," + frmRefSuspention).each(function () {
                $(this).addClass("ignore-element");
            });


            if (CheckErrors("#frmReferral", true)) {

                //var isLegalParentSet = false;
                //$scope.ReferralModel.ContactInformationList.filter(function (data, index) {
                //    if ($.parseJSON(data.IsPrimaryPlacementLegalGuardian) || $.parseJSON(data.IsDCSLegalGuardian)) {
                //        isLegalParentSet = true;
                //        return;
                //    }
                //});
                //if (!$.parseJSON(isLegalParentSet)) {
                //    toastr.error(window.MustSetLegalGuardian);
                //    return false;
                //}

                if ($scope.LastReferralStatusID !== $scope.ReferralModel.Referral.ReferralStatusID && $scope.ReferralModel.Referral.ReferralStatusID === parseInt(window.Inactive)) {
                    bootboxDialog(function (result) {
                        $scope.SaveReferral(isSaveAsDraft, result);
                    }, bootboxDialogType.Confirm, window.Confirm, window.SendNotificationToCM, window.YesContinue, 'btn btn-primary actionTrue', '', '', window.NoContinue);
                }
                else if ($scope.LastReferralStatusID !== $scope.ReferralModel.Referral.ReferralStatusID && $scope.ReferralModel.Referral.ReferralStatusID === parseInt(window.ReferralAccepted)) {
                    //$scope.SaveReferral(isSaveAsDraft, true);
                    bootboxDialog(function (result) {
                        $scope.SaveReferral(isSaveAsDraft, result);
                    }, bootboxDialogType.Confirm, window.Confirm, window.SendNotificationToCMReferralAccepted, window.YesContinue, 'btn btn-primary actionTrue', '', '', window.NoContinue);
                }
                else {
                    $scope.SaveReferral(isSaveAsDraft);
                }



            } else {
                toastr.error(window.CanNotSave);
            }
        }

    };

    $scope.SaveReferral = function (isSaveAsDraft, notifyCm) {

        $("#frmReferral").data('changed', false);
        $scope.ReferralModel.Referral.IsSaveAsDraft = isSaveAsDraft;
        var jsonData = angular.toJson({ NotifyCmForInactiveStatus: notifyCm, Referral: $scope.ReferralModel.Referral, ContactInformationList: $scope.ReferralModel.ContactInformationList, DxCodeMappingList: $scope.ReferralModel.DxCodeMappingList, ReferralPayorMapping: $scope.ReferralModel.ReferralPayorMapping, ReferralSiblingMappingList: $scope.ReferralModel.ReferralSiblingMappingList });
        AngularAjaxCall($http, $scope.AddReferralURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                SetMessageForPageLoad(response.Message, "ShowAddReferralMessage");

                if (!response.Data.Referral.IsEditMode) {
                    window.location.href = SiteUrl.AddReferralPageUrl + response.Data.Referral.EncryptedReferralID;
                    $scope.EncryptedReferralID = response.Data.Referral.EncryptedReferralID;
                } else {
                    location.reload();
                }
            } else
                ShowMessages(response);
        });
    };

    $scope.Cancel = function () {
        $window.location = SiteUrl.ReferralListURL;
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
            $("#SearchContactToken").tokenInput("clear");

        });

    };

    $scope.DisableContactType = false;
    $scope.EditContact = function (index, data) {
        //$scope.IsEditModeforContact = true;
        $scope.TempContactTypeID = data.ContactTypeID;

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

            $('#model_AddContact').modal('show');
            $timeout(function () {
                $("#SearchContactToken").tokenInput("clear");
                $scope.ReferralModel.AddAndListContactInformation = $.parseJSON(JSON.stringify($scope.ReferralModel.ContactInformationList[index]));
                JSON.parse($scope.ReferralModel.AddAndListContactInformation.IsEmergencyContact) ? $scope.ReferralModel.AddAndListContactInformation.IsEmergencyContact = "true" : $scope.ReferralModel.AddAndListContactInformation.IsEmergencyContact = "false";
                JSON.parse($scope.ReferralModel.AddAndListContactInformation.IsPrimaryPlacementLegalGuardian) ? $scope.ReferralModel.AddAndListContactInformation.IsPrimaryPlacementLegalGuardian = "true" : $scope.ReferralModel.AddAndListContactInformation.IsPrimaryPlacementLegalGuardian = "false";
                JSON.parse($scope.ReferralModel.AddAndListContactInformation.IsDCSLegalGuardian) ? $scope.ReferralModel.AddAndListContactInformation.IsDCSLegalGuardian = "true" : $scope.ReferralModel.AddAndListContactInformation.IsDCSLegalGuardian = "false";
                JSON.parse($scope.ReferralModel.AddAndListContactInformation.IsNoticeProviderOnFile) ? $scope.ReferralModel.AddAndListContactInformation.IsNoticeProviderOnFile = "true" : $scope.ReferralModel.AddAndListContactInformation.IsNoticeProviderOnFile = "false";
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
                }
            });
            $('#model_AddContact').modal('hide');
            $scope.TempContactTypeID = 0;
        });


    };

    $scope.EditSearchContact = function () {
        $scope.IsContactEditMode = false;
        $scope.ReferralModel.AddAndListContactInformation.MasterContactUpdated = true;
    };

    //#endregion

    //#region DX CODE MAPPING RELATED FUNCATION START

    $scope.TokenInputObj = {};
    $scope.SelectedDXCodeIDs = [];
    $scope.TempSelectedDXCodeIDs = [];
    $scope.GetDXCodeListForAutoCompleteURL = SiteUrl.GetDXCodeListForAutoCompleteURL;

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
        
        if ($scope.ReferralModel.ReferralDXCodeMapping.RandomID === undefined || $scope.ReferralModel.ReferralDXCodeMapping.RandomID === null) {
            $scope.ReferralModel.ReferralDXCodeMapping.RandomID = GenerateGuid();
        }

        if ($scope.ReferralModel.ReferralDXCodeMapping.DXCodeID && $scope.ReferralModel.ReferralDXCodeMapping.StartDate && $scope.ReferralModel.ReferralDXCodeMapping.EndDate && $scope.ReferralModel.ReferralDXCodeMapping.Precedence) {
            var precedenceCount = $scope.ReferralModel.DxCodeMappingList.filter(function (item) {

                var sd = (new Date($scope.ReferralModel.ReferralDXCodeMapping.StartDate)).valueOf();
                var ed = (new Date($scope.ReferralModel.ReferralDXCodeMapping.EndDate)).valueOf(); 
                var prcd = parseInt($scope.ReferralModel.ReferralDXCodeMapping.Precedence);
                var rnId = $scope.ReferralModel.ReferralDXCodeMapping.RandomID;

                var isd = (new Date(item.StartDate)).valueOf();
                var ied = (new Date(item.EndDate)).valueOf();
                var iprcd = parseInt(item.Precedence);
                var irnId = item.RandomID;

                if (
                    (sd >= isd && sd <= ied) || (ed >= isd && ed <= ied) || (sd < isd && ed > ied)
                ) {
                    return iprcd === prcd && irnId !== rnId;
                }
                return false;
                //return parseInt(item.Precedence) == parseInt($scope.ReferralModel.ReferralDXCodeMapping.Precedence) && item.DXCodeID != $scope.ReferralModel.ReferralDXCodeMapping.DXCodeID;
            }).length;
            if (precedenceCount > 0) {
                toastr.error(window.DXCodeWithPrecedenceExists);
            } else {
                var index = $.map($scope.ReferralModel.DxCodeMappingList, function (obj, id) {
                    if (obj.RandomID == $scope.ReferralModel.ReferralDXCodeMapping.RandomID) {
                        return id;
                    }
                    return null;
                });
                if (index.length > 0 && $scope.ReferralModel.DxCodeMappingList[index].RandomID == $scope.ReferralModel.ReferralDXCodeMapping.RandomID) {

                    $scope.ReferralModel.DxCodeMappingList[index].DXCodeID = $scope.ReferralModel.ReferralDXCodeMapping.DXCodeID;
                    $scope.ReferralModel.DxCodeMappingList[index].StartDate = $scope.ReferralModel.ReferralDXCodeMapping.StartDate;
                    $scope.ReferralModel.DxCodeMappingList[index].EndDate = $scope.ReferralModel.ReferralDXCodeMapping.EndDate;
                    $scope.ReferralModel.DxCodeMappingList[index].Precedence = $scope.ReferralModel.ReferralDXCodeMapping.Precedence;
                    $scope.DxCodeTokenInputObj.clear();
                    $scope.ReferralModel.ReferralDXCodeMapping = {};
                } else {
                    $scope.ReferralModel.DxCodeMappingList.push($scope.ReferralModel.ReferralDXCodeMapping);
                    $scope.TempSelectedDXCodeIDs.push($scope.ReferralModel.ReferralDXCodeMapping.DXCodeID);
                    $scope.DxCodeTokenInputObj.clear();
                    $scope.ValidateDxCode();
                    $scope.ReferralModel.ReferralDXCodeMapping = {};
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

            if (!$scope.ReferralModel.ReferralDXCodeMapping.StartDate) {
                $scope.ListofFieldsForDxCode += liString.format(window.Missing, window.StartDate);
            }

            if (!$scope.ReferralModel.ReferralDXCodeMapping.EndDate) {
                $scope.ListofFieldsForDxCode += liString.format(window.Missing, window.EndDate);
            }

            if (!$scope.ReferralModel.ReferralDXCodeMapping.Precedence) {
                $scope.ListofFieldsForDxCode += liString.format(window.Missing, window.Precedence);
            }

            bootboxDialog(function () {
            }, bootboxDialogType.Alert, window.Alert, window.FieldsIncomplete.format($scope.ListofFieldsForDxCode));
        }
        //}
    };

    $scope.EditDxCodeFromMapping = function (data) {
        $scope.ReferralModel.ReferralDXCodeMapping = angular.copy(data);
        $scope.DxCodeTokenInputObj.clear();
        $scope.DxCodeTokenInputObj.add({ DXCodeID: data.DXCodeID, DXCodeName: data.DXCodeName, readonly: true });
    };

    $scope.DeleteDxCodeFromMapping = function (tempObject, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {

            var btnText = result.currentTarget.textContent;
            var isSoftDelete = (btnText !== Delete);
            if (btnText !== Cancel) {

                var jsonData = angular.toJson({
                    isSoftDelte: isSoftDelete, ReferralDXCodeMappingID: tempObject.ReferralDXCodeMappingID, EncryptedReferralID: $scope.EncryptedReferralID,
                    IsEnable: (tempObject.IsDeleted)
                });

                if (tempObject.ReferralDXCodeMappingID > 0 && tempObject.ReferralDXCodeMappingID !== "" && tempObject.ReferralDXCodeMappingID !== undefined) {

                    AngularAjaxCall($http, SiteUrl.DeleteReferralDXCodeMappingURL, jsonData, "Post", "json", "application/json").success(function (response) {
                        //ShowMessages(response);
                        if (response.IsSuccess) {
                            if (isSoftDelete) {
                                tempObject.IsDeleted = !(tempObject.IsDeleted);
                                $timeout(function () {
                                    $scope.$apply();
                                });
                            } else {
                                $scope.ReferralModel.DxCodeMappingList.remove(tempObject);
                                $scope.TempSelectedDXCodeIDs.remove(tempObject.DXCodeID);
                            }
                            if (!$scope.$root.$$phase) {
                                $scope.$apply();
                            }
                            $scope.ValidateDxCode();

                            //$scope.ReferralModel.DXCodeCount = $scope.ReferralModel.DxCodeMappingList.filter($scope.isNotDeleted).length;
                            //alert($scope.ReferralModel.DXCodeCount);
                            //$scope.$apply();
                            //$('#DXCodeCount').valid();
                        }
                    });
                } else {
                    if (isSoftDelete) {
                        tempObject.IsDeleted = !(tempObject.IsDeleted);
                        //ShowToaster('success', (tempObject.IsDeleted) ? DxcodeDisabledSuccessMessage: DxcodeEnabledSuccessMessage);

                    } else {
                        $scope.ReferralModel.DxCodeMappingList.remove(tempObject);
                        $scope.TempSelectedDXCodeIDs.remove(tempObject.DXCodeID);
                    }
                    $timeout(function () {
                        $scope.$apply();
                        $scope.ValidateDxCode();
                    });
                }
            }
        }, bootboxDialogType.Dialog, title, window.DeleteDxCodeConfirmationMessage, bootboxDialogButtonText.HardDelete, btnClass.BtnDanger, bootboxDialogButtonText.Cancel, undefined,
        (tempObject.IsDeleted) ? bootboxDialogButtonText.Enable : bootboxDialogButtonText.Disable, (tempObject.IsDeleted) ? btnClass.BtnEnable : btnClass.BtnDisable);
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
        $('#DXCodeCount').valid();
    };

    $scope.isNotDeleted = function (x) {
        return x.IsDeleted == false;
    };

    $scope.OnDxCodeResultsFormatter = function (item) {
        return "<li id='{0}'><b>{3}: </b> {1} ({7})<span class='badge badge-primary' style='float:right;'>{8}</span><br/><b  style='color:#ad0303;'>{4}: </b>{2} </small></li>".
            format(item.DXCodeID, item.DXCodeWithoutDot ? item.DXCodeWithoutDot : NA, (item.Description) ? item.Description : NA, DXCode, Description, StartDate, EndDate, item.DXCodeName, item.DxCodeShortName);
    };

    //#endregion

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
                        //
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
        //
        HideErrors($("#frmUpdateReferralPayorInfo"));
        var jsonData = angular.toJson({ referralPayorMappingID: referralPayorMappingId });
        AngularAjaxCall($http, SiteUrl.GetReferralPayorDetail, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                response.Data.PayorEffectiveDate = moment(response.Data.PayorEffectiveDate).toDate();
                response.Data.PayorEffectiveEndDate = moment(response.Data.PayorEffectiveEndDate).toDate();
                $scope.ReferralModel.UpdateReferralPayorMapping = response.Data;
                $('#model_updateReferralPayorInfo').modal('show');
                if (!$scope.$root.$$phase) {
                    $scope.$apply();
                }
            }
        });

    };


    $scope.UpdateReferralPayorInformation = function () {
        //

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

    //#region CaseManager Related Code
    $scope.CaseManagerResultsFormatter = function (item) {
        return "<li id='{0}'>{0}<span class='badge badge-primary' style='float:right;'>{1}</span><br/><small><b>CM: </b>{2}</small><br/><small><b style='color:#ad0303;'>{5}: </b>{3}</small><small><b style='color:#ad0303;padding-left:10px;'>{6}: </b>{4} </small></li>"
            .format(
            item.NickName,
            item.RegionName,
            item.Name,
            item.Phone ? item.Phone : window.NA,
            item.Email ? item.Email : window.NA,
            window.Phone,
            window.Email);
    };
    $scope.CaseManagerTokenFormatter = function (item) {
        return "<li id='{0}'><b>CM:</b> {0} <b>Agency: </b> ({1})</li>".format(item.Name, item.NickName);
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
        $scope.ReferralModel.CaseManager.Phone = item.Phone;
        $scope.ReferralModel.CaseManager.Fax = item.Fax;

        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };

    //#endregion

    //#region Referral Audit Log 

    window.PageSize = 20;
    $scope.newAuditInstance = function () {
        return $.parseJSON($("#hdnSetAuditPageModel").val());
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
        AngularAjaxCall($http, SiteUrl.GetAuditLogListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
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

    $('#AuditView').on('shown.bs.modal', function () {
        $(".CallGetTableDisplayValue").click();
    });

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


        if (e.delegateTarget.id == 'addreferraldetails_auditlogs' || e.delegateTarget.id == 'addreferraldetails_behaviorcontract') {
            $scope.ShowSubmitActions = false;
            if (e.delegateTarget.id == 'addreferraldetails_auditlogs') $scope.GetAuditLogList();
            if (e.delegateTarget.id == 'addreferraldetails_behaviorcontract') {
                $scope.GetBXContractList();
                $scope.GetRefSuspension();
            }
        } else
            $scope.ShowSubmitActions = true;

        if (!$scope.$root.$$phase) $scope.$apply();

    });


    //$scope.ShowSubmitActions

    //$(".windowTable").scroll(function () {
    //    if (document.getElementById("windowTable").scrollHeight == $(".windowTable").scrollTop() + $(".windowTable").height() + 15 && !$scope.AjaxStart)
    //        $scope.AuditLogListPager.nextPage();
    //});


    //#endregion

    //#region Referral BX Contract

    //window.PageSize = 1;
    $scope.newBXContractInstance = function () {
        return $.parseJSON($("#hdnSetBXContractPageModel").val());
    };

    $scope.SearchRefBXContractListModel = $scope.newBXContractInstance().SearchRefBXContractListModel;
    $scope.TempSearchRefAuditLogListModel = $scope.newBXContractInstance().SearchRefBXContractListModel;
    $scope.ReferralBehaviorContract = $scope.newBXContractInstance().ReferralBehaviorContract;
    $scope.ReferralSuspention = $scope.newBXContractInstance().ReferralSuspention;

    $scope.BXContractListPager = new PagerModule("WarningDate", null, 'DESC');

    $scope.SetBXContractPostData = function (fromIndex) {
        var pagermodel = {
            searchModel: $scope.SearchRefBXContractListModel,
            pageSize: $scope.BXContractListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.BXContractListPager.sortIndex,
            sortDirection: $scope.BXContractListPager.sortDirection,
            sortIndexArray: $scope.BXContractListPager.sortIndexArray.toString()
        };

        return angular.toJson(pagermodel);
    };

    $scope.SearchBXModelMapping = function () {
        $scope.SearchRefBXContractListModel = $.parseJSON(angular.toJson($scope.TempSearchRefBXContractListModel));
    };

    $scope.ResetBXSearchFilter = function () {
        $scope.SearchRefBXContractListModel = $scope.newAuditInstance().SearchRefBXContractListModel;
        $scope.TempSearchRefBXContractListModel = $scope.newAuditInstance().SearchRefBXContractListModel;
        $scope.BXContractListPager.currentPage = 1;
        $scope.BXContractListPager.getDataCallback();
    };

    $scope.SearchRefBX = function () {
        $scope.BXContractListPager.currentPage = 1;
        $scope.BXContractListPager.getDataCallback(true);
    };


    $scope.BXContractList = [];
    $scope.SearchBXContractListPage = {};
    $scope.AjaxStart = true;
    $scope.GetBXContractList = function (isSearchDataMappingRequire) {

        if (isSearchDataMappingRequire)
            $scope.SearchBXModelMapping();

        $scope.SearchRefBXContractListModel.EncryptedReferralID = $scope.EncryptedReferralID;
        var jsonData = $scope.SetBXContractPostData($scope.BXContractListPager.currentPage);
        $scope.AjaxStart = true;
        AngularAjaxCall($http, SiteUrl.GetBXContractListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.BXContractList = response.Data.Items;
                $scope.BXContractListPager.currentPageSize = response.Data.Items.length;
                $scope.BXContractListPager.totalRecords = response.Data.TotalItems;

                if (!$scope.$root.$$phase) {
                    $scope.$apply();
                }

            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });

    };

    $scope.BXContractListPager.getDataCallback = $scope.GetBXContractList;


    $scope.NewBXContractView = function () {
        $scope.ReferralBehaviorContract = $scope.newAuditInstance().ReferralBehaviorContract;
    };

    $scope.OpenBXContractEditModal = function (data) {
        $scope.ReferralBehaviorContract = angular.copy(data);
        $("#NewBXContractView").modal("show");
    };

    $scope.SaveBXContract = function () {


        $("#frmAddBXContractView").each(function () {
            var ele = $(this, "input, textarea, select");
            $(ele).removeClass("ignore-element");
        });


        if (CheckErrors("#frmAddBXContractView")) {

            var jsonData = angular.toJson({
                ReferralBehaviorContract: $scope.ReferralBehaviorContract,
                SearchRefBXContractListModel: $scope.SearchRefBXContractListModel,
                pageSize: $scope.BXContractListPager.pageSize,
                pageIndex: $scope.BXContractListPager.currentPage,
                sortIndex: $scope.BXContractListPager.sortIndex,
                sortDirection: $scope.BXContractListPager.sortDirection,
                sortIndexArray: $scope.BXContractListPager.sortIndexArray.toString()
            });

            AngularAjaxCall($http, SiteUrl.SaveBXContractURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $('#NewBXContractView').modal('hide');
                    $scope.BXContractList = response.Data.Items;
                    $scope.BXContractListPager.currentPageSize = response.Data.Items.length;
                    $scope.BXContractListPager.totalRecords = response.Data.TotalItems;
                    $scope.GetRefSuspension();
                }
                ShowMessages(response);
            });
        }
    };
    $scope.UpdateBXContractStatus = function (bxItem) {
        var jsonData = angular.toJson({ model: bxItem });
        AngularAjaxCall($http, SiteUrl.UpdateBXContractStatusURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            ShowMessages(response);
            $scope.GetRefSuspension();
        });
    };
    $scope.IsEligibleForSuspension = false;
    $scope.GetRefSuspension = function () {
        var jsonData = angular.toJson({ EncryptedReferralID: $scope.EncryptedReferralID });
        AngularAjaxCall($http, SiteUrl.GetSuspensionURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralSuspention = response.Data.ReferralSuspention;
                $scope.IsEligibleForSuspension = response.Data.IsEligibleForSuspension;
            }
        });
    };

    $scope.ResetRefSuspension = function () {
        bootboxDialog(function (data) {
            data = $(data.currentTarget).hasClass('actionTrue');
            $scope.ReferralSuspention = $scope.newAuditInstance().ReferralSuspention;
            var jsonData = angular.toJson({
                model: $scope.ReferralSuspention,
                EncryptedReferralID: $scope.EncryptedReferralID,
                ResetSuspension: true,
                ResetBXContract: data ? true : false
            });
            AngularAjaxCall($http, SiteUrl.SaveSuspensionURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess && data) {
                    $scope.BXContractList.filter(function (item) {
                        item.IsActive = false;
                    });
                    $scope.GetRefSuspension();
                }
            });
        }, bootboxDialogType.TwoActionOnly, window.Confirm, window.InactiveAllBXContract, window.YesContinue, 'btn btn-primary actionTrue', '', '', window.NoContinue);
    };

    $scope.SaveRefSuspensionWithStatus = function () {
        $scope.ReferralSuspention.MakeClientInActive = false;
        if ($scope.ReferralSuspention.SuspentionType == 'Termination') {
            bootboxDialog(function (data) {
                data = $(data.currentTarget).hasClass('actionTrue');
                if (data == true) {
                    $scope.ReferralSuspention.MakeClientInActive = true;
                }
                $scope.SaveRefSuspension();
            }, bootboxDialogType.TwoActionOnly, window.Confirm, window.TerminatePopUpMessage, window.YesContinue, 'btn btn-primary actionTrue', '', '', window.NoContinue);
        } else {
            $scope.SaveRefSuspension();
        }
    };

    $scope.SaveRefSuspension = function () {
        $("#frmRefSuspention").each(function () {
            var ele = $(this, "input, textarea, select");
            $(ele).removeClass("ignore-element");
        });
        if (CheckErrors("#frmRefSuspention")) {
            var jsonData = angular.toJson({
                model: $scope.ReferralSuspention,
                EncryptedReferralID: $scope.EncryptedReferralID
            });
            AngularAjaxCall($http, SiteUrl.SaveSuspensionURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.ReferralSuspention = response.Data;
                    //if ($scope.ReferralSuspention.InactiveallStatus) {
                    //    //$scope.BXContractList.filter(function (item) {
                    //    //    item.IsActive = false;
                    //    //});
                    //    ////$scope.GetRefSuspension();
                    //}
                }
            });
        }
    };
    //#endregion


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

            AngularAjaxCall($http, SiteUrl.UpdateAHCCCSIDURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $('#UpdateAHCCCSID').modal('hide');
                    $scope.ReferralModel.Referral.AHCCCSID = $scope.ReferralModel.ReferralAhcccsDetails.NewAHCCCSID;
                }
                ShowMessages(response);
            });
        }
    };

    //#endregion
};

controllers.AddReferralController.$inject = ['$scope', '$http', '$window', '$timeout'];

$(document).ready(function () {

    var frmReferralControls = "#frmReferral :input,#frmReferral select";
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
        $("#frmReferral").data('changed', true);
    });


    $(window).bind("beforeunload", function () {
        if ($("#frmReferral").data('changed')) {
            return "Good bye";
        }
    });





    //$(".dateInputMask").inputmask("m/d/y", {
    //    placeholder: "mm/dd/yyyy",
    //    forceParse: true
    //});
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");

    $("#AHCCCS_ID,#NewAHCCCSID").inputmask({
        mask: "A99999999",
        placeholder: "AXXXXXXXX"
    });

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
            else
                $("a" + hashUrl).tab('show');
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

//#region SCRAP CODE
/*
//TODO:WE'll remove this if all slection work fine
function NotInUse_CheckTabAndLoad() {
    alert(window.location.hash);
    if (window.CurrentURL.indexOf("#rd_clientInformation") != -1) {
        $("a#addReferralDetails").tab('show');
        $("a#rd_clientInformation").tab('show');
    }
    else if (window.CurrentURL.indexOf("#rd_contactInformation") != -1) {
        $("a#addReferralDetails").tab('show');
        $("a#rd_contactInformation").tab('show');
    }
    else if (window.CurrentURL.indexOf("#rd_complianceInformation") != -1) {
        $("a#addReferralDetails").tab('show');
        $("a#rd_complianceInformation").tab('show');
    }
    else if (window.CurrentURL.indexOf("#rd_referralHistory") != -1) {
        $("a#addReferralDetails").tab('show');
        $("a#rd_referralHistory").tab('show');
    }
    else if (window.CurrentURL.indexOf("#addReferralDetails") != -1) {
        $("a#addReferralDetails").tab('show');
    } else if (window.CurrentURL.indexOf("#referralDocument") != -1) {
        $("a#referralDocument").tab('show');
    }
    else if (window.CurrentURL.indexOf("#checklist") != -1) {
        $("a#checklist").tab('show');
    }
    else if (window.CurrentURL.indexOf("#sparform") != -1) {
        $("a#sparform").tab('show');
    }
    else if (window.CurrentURL.indexOf("#internalMessage") != -1) {
        $("a#internalMessage").tab('show');
    } else if (window.CurrentURL.indexOf("#referralHistory") != -1) {
        $("a#referralHistory").tab('show');
    }
    else if (window.CurrentURL.indexOf("#referralNote") != -1) {
        $("a#referralNote").tab('show');
    }

    setTimeout(function () {
        $(window).scrollTop(0);
    }, 100);


    $("a[data-toggle=tab]").on('shown.bs.tab', function (e, ui) {
        var hashUrl = $(this).attr('id');
        window.location.hash = hashUrl;
        $(window).scrollTop(0);
    });
}
*/

//#endregion

controllers.ParentReferralController = function ($scope, $http, $window, $timeout) {
    $scope.ReferralErrorCount = {};
    $scope.ReferralErrorCount.ClientTab = [];
    $scope.ReferralErrorCount.ContactTab = [];
    $scope.ReferralErrorCount.ComplianceTab = [];
    $scope.ReferralErrorCount.ReferralHistoryTab = [];
    $scope.TotalReferralErrorCount = 0;

    $scope.ReferralDetailResultsFormatter = function (item) {
        return "<li id='{0}'>{1}<br/><small><b>{4}:</b> {2}</small><small><br/><b>{5}: </b>{3}</small><br/><small><b style='color:#ad0303;'>{6}:</b> {7}</small><br/><small><b style='color:#ad0303;'>{8}:</b> {9}</small></li>"
            .format(
                window.Name,
                item.Name,
                item.AHCCCSID ? item.AHCCCSID : 'N/A',
                item.CISNumber ? item.CISNumber : 'N/A',
                window.AHCCCSID,
                window.CISNumberShort,
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
        window.location = SiteUrl.AddReferralPageUrl + item.EncryptedReferralID;
        return false;
    };

    //#region Token input related code for service code
    $scope.GetReferralsURL = SiteUrl.GetReferralInfoURL;


    //#endregion

};
controllers.ParentReferralController.$inject = ['$scope', '$http', '$window', '$timeout'];