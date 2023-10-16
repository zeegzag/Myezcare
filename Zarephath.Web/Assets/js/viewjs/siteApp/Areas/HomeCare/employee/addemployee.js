var custModel;


controllers.AddEmployeeController = function ($scope, $http, $timeout, $rootScope) {
    custModel = $scope;
    $scope.EmployeeModel = $.parseJSON($("#hdnEmployeeModel").val());
    $scope.CurrentDate = new Date();
    $scope.AllowSendSMS = ValideElement($scope.EmployeeModel.Employee.MobileNumber);
    
    $scope.ListPreference = $scope.EmployeeModel.PreferenceList;
    $scope.StateList = $scope.EmployeeModel.StateList;
    $scope.SkillList = $scope.EmployeeModel.SkillList;
    $scope.EmployeeSkillList = $scope.EmployeeModel.EmployeeSkillList;
    $scope.CareTypeList = $scope.EmployeeModel.CareTypeList;
    $scope.OrgTypeList = $scope.EmployeeModel.OrgTypeList;
    $scope.FacilityList = $scope.EmployeeModel.FacilityList;
    
    $scope.AssociatedWith = $scope.EmployeeModel.Employee.AssociateWith;
    if ($scope.AssociatedWith) {
        $scope.SelectedOrg = $scope.AssociatedWith.split(',');
        $scope.EmployeeModel.Employee.AssociateWith = $scope.SelectedOrg;
    }


    $scope.FacilityIDs = $scope.EmployeeModel.Employee.FacilityID;
    if ($scope.FacilityIDs) {
        $scope.EmployeeModel.Employee.FacilityID = $scope.FacilityIDs.split(',');
    }

    $scope.SelectedCareType = [];
    $scope.AddEditItemValue = '99999999000011';
    $scope.CareTypeList = $scope.CareTypeList.filter(function (item) {
        return item.Value !== $scope.AddEditItemValue;
    });
    $scope.CareTypeSettings = {
        addEditItem: true,
        smartButtonMaxItems: 3
    };

    $scope.SelectedGroups = [];
    //if ($scope.EmployeeModel.Employee.GroupIDs != null) {
    //    $scope.SelectedGroups = $scope.EmployeeModel.Employee.GroupIDs.split(",");
    //}
    $scope.SetGroups = function () {
        if ($scope.EmployeeModel.Employee.GroupIDs != null) {
            $scope.SelectedGroups = $scope.EmployeeModel.Employee.GroupIDs.split(",");

        }
        else {
            $scope.SelectedGroups = "";
        }
    };
    $scope.SetGroups();
    if ($scope.EmployeeModel.Employee.CareTypeIds != null) {
        var careTypeValues = [];
        careTypeValues = $scope.EmployeeModel.Employee.CareTypeIds.split(",");
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
            $(".caretypedropdown").attr("data-original-title", "Services is required").attr("data-html", "true")
                .addClass("tooltip-danger input-validation-error")
                .tooltip({ html: true });
            return false;
        }
    }

    $scope.Save = function () {

        var sdf = $scope.EmployeeModel.Employee;
        var isValid = CheckErrors($("#frmAddEmployee"));
        var isValidCT = $scope.validationCareType();
        if (!isValid || !isValidCT) {
            toastr.error("Cannot save - required fields are incomplete.");
            return false;
        }

        // CHECK GEO CODES
        $scope.GenerateGeoCode(false, $scope.SaveEmployee, function () {
            toastr.error(window.GeoCodeError);
        });

    };

    $scope.SaveEmployee = function () {

        var isValid = CheckErrors($("#frmAddEmployee"));
        var isValidCT = $scope.validationCareType();
        if (!isValid || !isValidCT) {
            return false;
        }
        if (isValid) {
            if ($scope.SelectedCareType) {
                var careTypeValues = $scope.SelectedCareType;
                $scope.EmployeeModel.Employee.CareTypeIds = Object.keys(careTypeValues).map(function (k) { return careTypeValues[k]["Value"] }).join(",");
            }
            else {
                $scope.EmployeeModel.Employee.CareTypeIds = null;
            }
            // $scope.EmployeeModel.Employee.GroupIDs = $scope.SelectedGroups.join(",");
            $scope.EmployeeModel.Employee.GroupIDs = $scope.SelectedGroups ? $scope.SelectedGroups.join(",") : "";

            var asso = $scope.EmployeeModel.Employee.AssociateWith;
            $scope.EmployeeModel.Employee.AssociateWith = asso ? (Array.isArray(asso) ? asso.join() : asso) : "";
            
            var selectedFacilityID = $scope.EmployeeModel.Employee.FacilityID;
            $scope.EmployeeModel.Employee.FacilityID = selectedFacilityID ? (Array.isArray(selectedFacilityID) ? selectedFacilityID.join() : selectedFacilityID) : "";

            var jsonData = angular.toJson({
                Employee: $scope.EmployeeModel.Employee,
                PreferenceList: $scope.ListPreference,
                StrEmployeeSkillList: $scope.EmployeeSkillList ? $scope.EmployeeSkillList.toString() : "",
                ContactInformationList: $scope.EmployeeModel.ContactInformationList,
            });
            AngularAjaxCall($http, HomeCareSiteUrl.AddEmployeeURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    //ShowMessages(response);
                    if (response.IsSuccess) {
                        toastr.success("Employee saved successfully");
                        window.location.reload();

                        //
                    } else {
                        ShowMessages(response);
                    }

                });
        }
    };
    $scope.$watchCollection('EmployeeModel.ContactInformationList', function (newValue, oldValue) {
        $scope.MissingPC = true; $scope.MissingLG = true; $scope.ContactServiceErrorCount = "";
        angular.forEach($scope.EmployeeModel.ContactInformationList, function (item, key) {
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

    //if ($scope.EmployeeModel.Employee.AssociateWith != null) {
    //    $scope.SelectedOrg = [];
    //    $scope.SelectedOrg = $scope.EmployeeModel.Employee.AssociateWith.split(",");
    //}

    $scope.ResendRegistrationMail = function () {
        var jsonData = angular.toJson({ id: $scope.EmployeeModel.Employee.EmployeeID });
        AngularAjaxCall($http, HomeCareSiteUrl.ResendMail, jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                ShowMessages(response);
            });
    }
    $scope.SaveEmailEmployeeSignature = function () {
        var nm = '';
        var nm = $('#txtFormEmailName').val();
        var Des = $('.note-editable').html();
        var jsonData = angular.toJson({ EmployeeID: $scope.EmployeeModel.Employee.EmployeeID, Name: nm, Description: Des });
        AngularAjaxCall($http, HomeCareSiteUrl.SaveEmployeeEmailSignature, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                ShowMessage("Email signature saved successfully", "success");
            }
        });
    };


    $scope.GetEmailEmployeeSignature = function () {
        var jsonData = angular.toJson({ EmployeeID: $scope.EmployeeModel.Employee.EmployeeID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeEmailSignature, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {

                $('#txtFormEmailName').val(response.Data.Name);
                $('.panel-body').html(response.Data.Description);


            }
        });
    };


    $("a#addEmployee_EmailSignature").on('shown.bs.tab', function (e) {
        $scope.GetEmailEmployeeSignature();
    });


    $scope.ResendRegistrationSMS = function () {
        var jsonData = angular.toJson({ id: $scope.EmployeeModel.Employee.EmployeeID });
        AngularAjaxCall($http, HomeCareSiteUrl.ResendSMS, jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                ShowMessages(response);
            });
    }

    //#region ADD Employee Signature

    $scope.UploadFile = SiteUrl.CommonUploadFileUrl;
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
                ShowMessage(window.MaximumUploadImageSizeMessage, "error");
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
        console.log(data.files[0].name);
    };

    $scope.AfterSend = function (e, data) {
        $scope.IsFileUploading = false;
        var model = data.result;
        if (model.IsSuccess) {
            $scope.EmployeeModel.Employee.TempSignaturePath = model.Data.TempFilePath;
            $scope.UploadingFileList = [];
        } else {
            ShowMessage(model);
        }
        $scope.$apply();
    };

    //$scope.AfterSend = function (model) {
    //    //ShowMessages(data.result);
    //    $scope.IsFileUploading = false;
    //    //var model = data.result;
    //    $scope.EmployeeModel.Employee.TempSignaturePath = model.FilePath;//model.Data.items.TempFilePath;
    //    $scope.UploadingFileList = [];
    //    $scope.$apply();
    //};

    //#endregion


    //public long EmployeePreferenceID { get; set; }
    //public long EmployeeID { get; set; }
    //public long PreferenceID { get; set; }
    //public string PreferenceName { get; set; }





    //#region ADD Skill
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
            AngularAjaxCall($http, HomeCareSiteUrl.DeletePreferenceURL, jsonData, "Post", "json", "application/json", false).
                success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.ListPreference.splice(index, 1);
                    }
                });
        }
    };

    $scope.addSSNLog = function () {

        var jsonData = angular.toJson({
            Employee: $scope.EmployeeModel.Employee
        });
        AngularAjaxCall($http, HomeCareSiteUrl.EmployeeAddSSNLog, jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                //ShowMessages(response);
                if (response.IsSuccess) {
                    console.log('view ssn')
                    //
                } else {
                    console.log('erro view ssn');
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



    //#endregion


    //$scope.GenerateGeoCode = function (isFirstTimeLoad) {
    //    if (isFirstTimeLoad === false && (!ValideElement($scope.EmployeeModel.Employee.Address) ||
    //        !ValideElement($scope.EmployeeModel.Employee.City) ||
    //        !ValideElement($scope.EmployeeModel.Employee.ZipCode) ||
    //        !ValideElement($scope.EmployeeModel.Employee.StateCode))) {
    //        toastr.error(window.AddressMissingMessage);
    //        return false;
    //    }


    //    var address = $scope.EmployeeModel.Employee.Address + ',' + $scope.EmployeeModel.Employee.City
    //        + '-' + $scope.EmployeeModel.Employee.ZipCode + ',' + $scope.EmployeeModel.Employee.StateCode;
    //    var mapElement = "gmap";
    //    GetGeoCodeAndMapFromAddress(address, mapElement, function (lat, long) {
    //        $scope.EmployeeModel.Employee.Latitude = lat;
    //        $scope.EmployeeModel.Employee.Longitude = long;
    //        $("#" + mapElement).css({ 'height': '300px', 'width': '100%' });
    //        if (!$scope.$root.$$phase) {
    //            $scope.$apply();
    //        }

    //    });
    //};



    $scope.GenerateGeoCode = function (isFirstTimeLoad, callback, errorCallback) {
        if (isFirstTimeLoad === false && (!ValideElement($scope.EmployeeModel.Employee.Address) ||
            !ValideElement($scope.EmployeeModel.Employee.City) ||
            !ValideElement($scope.EmployeeModel.Employee.ZipCode) ||
            !ValideElement($scope.EmployeeModel.Employee.StateCode))) {
            toastr.error(window.AddressMissingMessage);
            return false;
        }


        var address = $scope.EmployeeModel.Employee.Address + ',' + $scope.EmployeeModel.Employee.City
            + '-' + $scope.EmployeeModel.Employee.ZipCode + ',' + $scope.EmployeeModel.Employee.StateCode;
        //var mapElement = "gmap";
        GetGeoCodeAndMapFromAddress(address, undefined, function (lat, long) {
            $scope.EmployeeModel.Employee.Latitude = lat;
            $scope.EmployeeModel.Employee.Longitude = long;
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
    // New Task dropdown

    $scope.id = {};


    $scope.AddEmployeeContactURL = "/hc/Employee/AddEmployeeContact";
    $scope.GetEmployeeContactListURL = "/hc/Employee/getEmployeecontactlist";
    $scope.DeleteEmployeeContactURL = "/hc/Employee/DeleteEmployeecontact";
    $scope.GetEmployeeOverTimePayBillingReportURL = "/hc/Employee/GetEmployeeOverTimePayBillingReportList";
    $scope.ContactID = null;
    $scope.IsContactEditMode = false;
    $scope.CanAddContact = true;
    $scope.TempContactTypeID = 0;
    $scope.TempIndexNumberofContactList = 0;
    $scope.ContactInfoList = [];
    $scope.EmployeeErrorCount = {};
    $scope.EmployeeErrorCount.ClientTab = [];
    $scope.EmployeeErrorCount.ContactTab = [];
    $scope.EmployeeErrorCount.ComplianceTab = [];
    $scope.EmployeeErrorCount.ReferralHistoryTab = [];
    $scope.TotalEmployeeErrorCount = 0;
    $scope.DisableContactType = false;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnEmployeeModel").val());
    };
    $scope.OpenAddEmployeeContactModal = function () {
        $timeout(function () {

            $scope.TempContactTypeID = 0;
            $scope.DisableContactType = false;
            HideErrors($("#frmEmployeeAddContact"));
            $(".postzipcode").val('');

            $scope.EmployeeModel.AddAndListContactInformation = $scope.newInstance().AddAndListContactInformation;
            $scope.SetDefaultValuesForAddContact();
            //$("#SearchContactToken").tokenInput("clear");

        });

    };
    //#region ADD/UPDATE NEW CONTACT RELATED FUNCATION START
    $scope.SetDefaultValuesForAddContact = function () {



        $scope.EmployeeModel.AddAndListContactInformation.State = window.defaultStateCode;
        $scope.EmployeeModel.AddAndListContactInformation.ROIExpireDate = null;
        $scope.EmployeeModel.AddAndListContactInformation.IsPrimaryPlacementLegalGuardian = 'false';
        $scope.EmployeeModel.AddAndListContactInformation.IsDCSLegalGuardian = 'false';
        $scope.EmployeeModel.AddAndListContactInformation.IsNoticeProviderOnFile = 'false';
        $scope.EmployeeModel.AddAndListContactInformation.IsEmergencyContact = 'false';
    };
    $scope.SaveEmployeeContact = function () {
        var isValid = CheckErrors($("#frmEmployeeAddContact"));

        if (isValid) {

            if ($scope.EmployeeModel.AddAndListContactInformation.ContactTypeID == window.PrimaryPlacementContactTypeID) {
                $scope.GenerateEmployeeContactGeoCode(false, $scope.SaveContactDetail, function () {
                    toastr.error(window.GeoCodeError);
                });
            } else {
                $scope.SaveContactDetail();
            }

        }
    };
    $scope.SaveContactDetail = function () {
        var isValid = CheckErrors($("#frmEmployeeAddContact"));

        if (isValid) {
            angular.forEach($scope.EmployeeModel.ContactInformationList, function (item, key) {
                if ($scope.TempContactTypeID != parseInt(item.ContactTypeID)) {
                    if (parseInt($scope.EmployeeModel.AddAndListContactInformation.ContactTypeID) == parseInt(item.ContactTypeID)) {
                        //alert("Cannot Added");

                        $scope.CanAddContact = false;
                        bootboxDialog(null, bootboxDialogType.Alert, bootboxDialogTitle.Alert,
                            window.CaontactTypeAlreadyExist.format($scope.EmployeeModel.AddAndListContactInformation.ContactTypeName), bootboxDialogButtonText.Ok);
                        return;
                    }
                }

            });

            if ($scope.CanAddContact) {

                if ($scope.TempPrimaryPlacementLegalGuardian == undefined)
                    $scope.TempPrimaryPlacementLegalGuardian = false;
                //CHECK LEGAL GURDIAN EXIST IF YES THEN SHOW MESSAGE AND INFORM ABOUT LEGAL GUARDINA DELETE
                // IF LEGAL GUARDIAN NOT FOUND THEN ADD NEW LEAGAL GUARDIAN AND ALSO BEFORE ADD SHOW MESSSAGE FOR LEGUAL GURADIN ADD
                if (parseInt($scope.EmployeeModel.AddAndListContactInformation.ContactTypeID) == parseInt(window.PrimaryPlacementContactTypeID)
                    && JSON.parse($scope.EmployeeModel.AddAndListContactInformation.IsPrimaryPlacementLegalGuardian)
                    && JSON.parse($scope.TempPrimaryPlacementLegalGuardian) == false) {

                    var isLegalGuardianExit = false;
                    var legalGuardianRemoveIndex = -1;
                    var legalContactMappingID = 0;

                    angular.forEach($scope.EmployeeModel.ContactInformationList, function (item, key) {
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
                                    if ($scope.EmployeeModel.ContactInformationList != -1) {

                                        $scope.EmployeeModel.ContactInformationList.splice(legalGuardianRemoveIndex, 1);
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
                $scope.EmployeeModel.AddAndListContactInformation.AddNewContactDetails = true;
                var model = $.parseJSON(JSON.stringify($scope.EmployeeModel.AddAndListContactInformation));
                //FOR OTHER ALL TYPE OF CONTACT
                if ($scope.TempContactTypeID > 0) {  //EDIT MODE
                    angular.forEach($scope.EmployeeModel.ContactInformationList, function (item, index) {
                        if (item.ContactTypeID == parseInt($scope.TempContactTypeID)) {
                            $scope.EmployeeModel.ContactInformationList[index] = model;
                        }
                    });
                } else {  // ADD MODE

                    //ADD Primary Placement Contact
                    model.EmpFirstName = window.loggedInUserName;
                    model.ReferenceMasterID = $scope.EmployeeModel.ReferenceMaster.ReferenceID;;
                    $scope.EmployeeModel.ContactInformationList.push(model);
                }

                if (legalContactMappingID >= 0) {
                    //ADD Legal Guardin Contact
                    var model1 = $.parseJSON(JSON.stringify($scope.EmployeeModel.AddAndListContactInformation));
                    model1.ContactMappingID = legalContactMappingID;
                    model1.ContactTypeID = window.LegalGuardianContactTypeID;
                    model1.ContactTypeName = window.LegalGuardian;
                    model1.TempContactMappingID = legalContactMappingID;
                    model1.IsPrimaryPlacementLegalGuardian = false;
                    model1.EmpFirstName = window.loggedInUserName;
                    model1.ReferenceMasterID = $scope.EmployeeModel.ReferenceMaster.ReferenceID;
                    $scope.EmployeeModel.ContactInformationList.push(model1);
                    $scope.AddUpdateContactToDB(model1);
                }
                else {
                    $scope.AddUpdateContactToDB(model);
                }

            });
            $('#model_EmployeeAddContact').modal('hide');

        });


    };

    $scope.AddUpdateContactToDB = function (model) {
        if ($scope.EmployeeModel.Employee.EmployeeID > 0) {
            if (model.ContactID > 0) {
                model.AddNewContactDetails = false;
            }
            else {
                model.AddNewContactDetails = true;
                model.EmployeeID = $scope.EmployeeModel.Employee.EmployeeID;
                model.ClientID = 0;
                model.ReferenceMasterID = $scope.EmployeeModel.ReferenceMaster.ReferenceID;
            }
            //if ($scope.SelectedOrg) {
            //    $scope.EmployeeModel.Employee.AssociateWith = $scope.SelectedOrg.join();
            //}
            var jsonData = angular.toJson({
                Referral: $scope.EmployeeModel.Employee,
                AddAndListContactInformation: model,
            });
            AngularAjaxCall($http, $scope.AddEmployeeContactURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    if (response.Data.AddAndListContactInformation.length == $scope.EmployeeModel.ContactInformationList.length) {
                        angular.forEach($scope.EmployeeModel.ContactInformationList, function (item, index) {
                            if (item.ContactTypeID == parseInt($scope.TempContactTypeID)) {
                                $scope.EmployeeModel.ContactInformationList[index].contactMappingID = response.Data.AddAndListContactInformation[index].contactMappingID;
                            }
                        });
                    }
                } else {
                    //ShowMessages(response);

                }
                $scope.TempContactTypeID = 0;
            });
        } else { $scope.TempContactTypeID = 0; }
    }
    $scope.GenerateEmployeeContactGeoCode = function (isFirstTimeLoad, callback, errorCallback) {
        if (isFirstTimeLoad === false && (!ValideElement($scope.EmployeeModel.AddAndListContactInformation.Address) ||
            !ValideElement($scope.EmployeeModel.AddAndListContactInformation.City) ||
            !ValideElement($scope.EmployeeModel.AddAndListContactInformation.ZipCode) ||
            !ValideElement($scope.EmployeeModel.AddAndListContactInformation.State))) {
            toastr.error(window.AddressMissingMessage);
            return false;
        }

        var address = $scope.EmployeeModel.AddAndListContactInformation.Address + ',' + $scope.EmployeeModel.AddAndListContactInformation.City
            + '-' + $scope.EmployeeModel.AddAndListContactInformation.ZipCode + ',' + $scope.EmployeeModel.AddAndListContactInformation.State;
        //var mapElement = "gmap";
        GetGeoCodeAndMapFromAddress(address, undefined, function (lat, long) {
            $scope.EmployeeModel.AddAndListContactInformation.Latitude = lat;
            $scope.EmployeeModel.AddAndListContactInformation.Longitude = long;
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
    $scope.EditEmployeeContact = function (index, data) {
        //$scope.IsEditModeforContact = true;
        $scope.TempContactTypeID = data.ContactTypeID;

        $scope.EmployeeModel.AddAndListContactInformation.Latitude = data.Latitude;
        $scope.EmployeeModel.AddAndListContactInformation.Longitude = data.Longitude;

        HideErrors($("#frmEmployeeAddContact"));

        var tempIsPrimaryPlacementLegalGuardian = false;
        if (data.ContactTypeID.toString() === $scope.LegalGuardianContactTypeID) {
            angular.forEach($scope.EmployeeModel.ContactInformationList, function (item, key) {
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
            $('#model_EmployeeAddContact').modal({
                backdrop: 'static',
                keyboard: false
            });
            $timeout(function () {
                //$("#SearchContactToken").tokenInput("clear");
                $scope.EmployeeModel.AddAndListContactInformation = $.parseJSON(JSON.stringify($scope.EmployeeModel.ContactInformationList[index]));
                JSON.parse($scope.EmployeeModel.AddAndListContactInformation.IsEmergencyContact) ? $scope.EmployeeModel.AddAndListContactInformation.IsEmergencyContact = "true" : $scope.EmployeeModel.AddAndListContactInformation.IsEmergencyContact = "false";
                JSON.parse($scope.EmployeeModel.AddAndListContactInformation.IsPrimaryPlacementLegalGuardian) ? $scope.EmployeeModel.AddAndListContactInformation.IsPrimaryPlacementLegalGuardian = "true" : $scope.EmployeeModel.AddAndListContactInformation.IsPrimaryPlacementLegalGuardian = "false";
                JSON.parse($scope.EmployeeModel.AddAndListContactInformation.IsDCSLegalGuardian) ? $scope.EmployeeModel.AddAndListContactInformation.IsDCSLegalGuardian = "true" : $scope.EmployeeModel.AddAndListContactInformation.IsDCSLegalGuardian = "false";
                JSON.parse($scope.EmployeeModel.AddAndListContactInformation.IsNoticeProviderOnFile) ? $scope.EmployeeModel.AddAndListContactInformation.IsNoticeProviderOnFile = "true" : $scope.EmployeeModel.AddAndListContactInformation.IsNoticeProviderOnFile = "false";

            });


            $('#model_EmployeeAddContact').on('shown.bs.modal', function (e) {
                // do something...
                //$scope.GenerateGeoCode(true);
            });

        }
    };

    $scope.DeleteEmployeeContact = function (index, item) {
        bootboxDialog(function (result) {
            if (result) {
                if (item.ContactMappingID > 0) {
                    var jsonData = angular.toJson({ contactMappingID: item.ContactMappingID });
                    AngularAjaxCall($http, $scope.DeleteEmployeeContactURL, jsonData, "Post", "json", "application/json").success(function (response) {
                        if (response.IsSuccess) {
                            $scope.EmployeeModel.ContactInformationList.remove(item);
                        }
                        ShowMessages(response);
                    });
                } else {
                    $scope.$apply(function () {
                        $scope.EmployeeModel.ContactInformationList.splice(index, 1);
                    });

                }


            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);

    };

    $scope.EmployeeBillingReportList = [];
    $scope.SelectedEmployeeBillingReport = [];
    $scope.SelectAllCheckbox = false;

    $scope.EmployeeBillingReportModel = $scope.EmployeeModel.SetEmployeeBillingReportListPage;
    $scope.SearchEmployeeBillingReportListPage = $scope.EmployeeBillingReportModel.SearchEmployeeBillingReportListPage;
    $scope.TempSearchEmployeeBillingReportListPage = $scope.EmployeeBillingReportModel.SearchEmployeeBillingReportListPage;
    $scope.EmployeeBillingReportListPager = new PagerModule("EmployeeID", "", "DESC");

    $scope.GetEmployeeOverTimePayBillingReportList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedEmployeeBillingReport = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchEmployeeBillingReportListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();

        var jsonData = $scope.SetPostData($scope.EmployeeBillingReportListPager.currentPage);
        AngularAjaxCall($http, $scope.GetEmployeeOverTimePayBillingReportURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.EmployeeBillingReportList = response.Data.Items;
                $scope.EmployeeBillingReportListPager.currentPageSize = response.Data.Items.length;
                $scope.EmployeeBillingReportListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.EmployeeBillingReportListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        //Reset 
        $scope.SearchEmployeeBillingReportListPage = $scope.newInstance().SetEmployeeBillingReportListPage.SearchEmployeeBillingReportListPage;
        $scope.TempSearchEmployeeBillingReportListPage = $scope.newInstance().SetEmployeeBillingReportListPage.SearchEmployeeBillingReportListPage;
        $scope.EmployeeBillingReportListPager.currentPage = 1;
        $scope.EmployeeBillingReportListPager.getDataCallback();
    };

    $scope.SearchEmployeeBillingReport = function () {
        $scope.EmployeeBillingReportListPager.currentPage = 1;
        $scope.EmployeeBillingReportListPager.getDataCallback(true);
    };
    $scope.SetPostData = function (fromIndex) {
        $scope.SearchEmployeeBillingReportListPage.EmployeeID = $scope.EmployeeModel.Employee.EmployeeID;
        var pagermodel = {
            SearchEmployeeBillingReportListPage: $scope.SearchEmployeeBillingReportListPage,
            pageSize: $scope.EmployeeBillingReportListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.EmployeeBillingReportListPager.sortIndex,
            sortDirection: $scope.EmployeeBillingReportListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchEmployeeBillingReportListPage = $.parseJSON(angular.toJson($scope.TempSearchEmployeeBillingReportListPage));
    };
    $scope.EmployeeBillingReportListPager.getDataCallback = $scope.GetEmployeeOverTimePayBillingReportList;
    $scope.EmployeeBillingReportListPager.getDataCallback();

    $scope.SliderLeft = function () {
        angular.element('#SliderLeft').addClass("LeftCallShow");
        angular.element('#slider').addClass("LeftCallShow");
        angular.element('#slider').removeClass("RightCallShow");
        angular.element('#rightShow').removeClass("RightCallShow");
    }
    $scope.SliderRight = function () {
        angular.element('#rightShow').addClass("RightCallShow");
        angular.element('#slider').addClass("RightCallShow");
        angular.element('#slider').removeClass("LeftCallShow");
        angular.element('#SliderLeft').removeClass("LeftCallShow");
    }
    $scope.EmployeeEditModelClosed = function () {
        $scope.Refresh();
        $('#fixedAside').modal('hide');
    }
    $scope.Cancel = function () {
        window.location.reload();
        //var EncryptedEmployeeID = $scope.EmployeeModel.Employee.EncryptedEmployeeID;
        //window.location.href = HomeCareSiteUrl.PartialAddEmployeeURL + EncryptedEmployeeID;
    }
    $scope.Next = function () {
        angular.element('#s2').addClass("active");
        angular.element('#s1').removeClass("active");
    }
    $scope.Next1 = function () {
        angular.element('#s3').addClass("active");
        angular.element('#s2').removeClass("active");
    }
    $scope.Prev = function () {
        angular.element('#s1').addClass("active");
        angular.element('#s2').removeClass("active");
    }
    $scope.Prev1 = function () {
        angular.element('#s2').addClass("active");
        angular.element('#s3').removeClass("active");
    }
    $scope.Next = function () {
        angular.element('#s2').addClass("active");
        angular.element('#s1').removeClass("active");
    }
    $scope.Next1 = function () {
        angular.element('#s3').addClass("active");
        angular.element('#s2').removeClass("active");
    }
    $scope.Prev = function () {
        angular.element('#s1').addClass("active");
        angular.element('#s2').removeClass("active");
    }
    $scope.Prev1 = function () {
        angular.element('#s2').addClass("active");
        angular.element('#s3').removeClass("active");
    }
    $scope.SaveRegularHours = function () {

        var jsonData = angular.toJson($scope.EmployeeModel.Employee);
        AngularAjaxCall($http, HomeCareSiteUrl.SaveRegularHours, jsonData, "Post", "json", "application/json", false).success(function (response) {
            ShowMessages(response);

        });

    }
    $scope.CancelRegularHours = function () {
        $scope.EmployeeModel.Employee.RegularHours = '';
        $scope.EmployeeModel.Employee.RegularPayHours = '';
        $scope.EmployeeModel.Employee.OvertimePayHours = '';
    }
    $scope.alert = function () {
        alert();
    }
    $scope.SaveEmailEmployeeSignature = function () {
        var nm = '';
        var nm = $('#txtFormEmailName').val();
        var Des = $('.note-editable').html();
        var jsonData = angular.toJson({ EmployeeID: $scope.EmployeeModel.Employee.EmployeeID, Name: nm, Description: Des });
        AngularAjaxCall($http, HomeCareSiteUrl.SaveEmployeeEmailSignature, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                ShowMessage("Email signature saved successfully", "success");
            }
        });
    };


    $scope.GetEmailEmployeeSignature = function () {
        var jsonData = angular.toJson({ EmployeeID: $scope.EmployeeModel.Employee.EmployeeID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeEmailSignature, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {

                $('#txtFormEmailName').val(response.Data.Name);
                $('.panel-body').html(response.Data.Description);


            }
        });
    };


    $("a#addEmployee_EmailSignature").on('shown.bs.tab', function (e) {
        $scope.GetEmailEmployeeSignature();
    });


};

controllers.AddEmployeeController.$inject = ['$scope', '$http', '$timeout', '$rootScope'];


$(document).ready(function () {
    var dateformat = GetOrgDateFormat();
    $(".dateInputMask").attr("placeholder", dateformat);
    $('.time').inputmask({
        mask: "h:s t\\m",
        placeholder: "hh:mm a",
        alias: "datetime",
        hourFormat: "12"
    });
});
