var custModel;

controllers.GroupMonthlySummaryController = function ($scope, $http, $timeout) {
    custModel = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnGroupMonthlySummary").val());
    };
    $scope.IsFinalPage = false;
    $scope.GroupNoteModel = $scope.newInstance();
    $scope.SearchClientModel = $scope.newInstance().SearchClientForMonthlySummary;
    $scope.CommonMonthlySummary = $scope.newInstance().CommonMonthlySummary;
    $scope.Coordinationofcare = $scope.newInstance().Coordinationofcare;
    $scope.MonthlySummaryList = [];


    //#region Wizard
    $scope.currentStep = 1;
    $scope.steps = [
        {
            step: 1,
            name: window.FirstStep,
            desc: window.SearchClients,
            isDone: false
        },
        {
            step: 2,
            name: window.SecondStep,
            desc: window.EnterMonthlySummaryDetail,
            isDone: false
        },
        {
            step: 3,
            name: window.ThirdStep,
            desc: window.CompleteGroupMonthlySummary,
            isDone: false
        }
    ];


    //$scope.$watch('currentStep', function (newValue) {
    //    if (parseInt(newValue) > 0) {
    //        if (newValue == 2 && $scope.SelectedClients.length > 0) {
    //            $scope.steps[0].isDone = true;
    //        }
    //    }
    //});

    //$scope.$watch(function () {
    //    return $scope.SelectedClients.length;
    //}, function () {
    //    if ($scope.SelectedClients.length == 0) {
    //        $scope.steps[1].isDone = false;
    //        $scope.steps[2].isDone = false;
    //    }
    //});

    //Functions
    $scope.gotoStep = function (newStep) {
        //var isValid = true;
        if (newStep == 2) {
            $scope.steps[0].isDone = false;
            $scope.steps[1].isDone = false;
            $scope.steps[2].isDone = false;
        }
        else if (newStep == 2) {
            $scope.steps[0].isDone = true;
            $scope.steps[1].isDone = false;
            $scope.steps[2].isDone = false;
        }
        else if (newStep == 3) {
            var isValid = $scope.ValidateCommonForm();
            if (!isValid) {
                $scope.currentStep = 2;
                return;
            }

            if ($scope.currentStep == 1 || $scope.currentStep == 2) {
                $scope.MoveStep3();
            }
        }
        $scope.currentStep = newStep;
        $scope.steps[0].isDone = true;
        $scope.steps[1].isDone = true;
    };

    //#endregion

    //#region Step 1 Code

    $scope.SelectAllCheckbox = false;
    $scope.SelectedClientIds = [];
    $scope.SelectedClients = [];
    $scope.Clients = [];

    $scope.SearchClientForNote = function () {
        var jsonData = {
            searchModel: $scope.SearchClientModel,
            ignoreClientID: $scope.SelectedClientIds
        };
        $scope.GetGroupNoteListAjaxCall = true;
        AngularAjaxCall($http, SiteUrl.SearchClientForMonthlySummary, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.Clients = response.Data.ReferralDetailForNote;
                //$scope.GroupNoteModel.Facilities = response.Data.Facilities;
            }
            $scope.GetGroupNoteListAjaxCall = false;
            ShowMessages(response);
        });
    };

    $scope.SelectClient = function (client) {

        if (client.IsChecked) {
            //if ($scope.SelectedClients.length > 0) {
            //    if ($scope.SelectedClients[0].PayorID == client.PayorID) {
            //        $scope.SelectedClientIds.push(client.ReferralID);
            //        $scope.SelectedClients.push(client);
            //    } else {
            //        toastr.error("Payor should be same for all clients which you select.");
            //    }
            //} else {
            $scope.SelectedClientIds.push(client.ReferralID);
            $scope.SelectedClients.push(client);
            //}

        } else {
            $scope.SelectedClientIds.remove(client.ReferralID);
            $scope.SelectedClients.remove(client);
        }
        if ($scope.SelectedClientIds.length == $scope.Clients.length) {
            $scope.SelectAllCheckbox = true;
            if (!$scope.$root.$$phase) {
                $scope.$apply();
            }
        } else {
            $scope.SelectAllCheckbox = false;
        }
    };

    $scope.SelectAll = function (val) {
        $scope.SelectedClientIds = [];
        $scope.SelectedClients = [];
        angular.forEach($scope.Clients, function (item) {
            item.IsChecked = val; // event.target.checked;
            if (item.IsChecked) {
                $scope.SelectedClientIds.push(item.ReferralID);
                $scope.SelectedClients.push(item);
            }
        });
        return true;
    };

    $scope.CheckIsChecked = function () {
        return function (item) {
            return !item.IsChecked;
        };
    };

    $scope.SearchClientForNote();
    //#endregion

    //#region Step2

    $scope.ValidateCommonForm = function () {
        return CheckErrors("#groupnote-step2", true);
    };


    //#endregion Step2

    //#region Step3

    $scope.MoveStep3 = function () {
        var monthlySummaries = [];
        angular.forEach($scope.SelectedClients, function (item) {

            ReferralMonthalySummary($scope, $http, $timeout, item);

            //$scope.newSummary = $.parseJSON(angular.toJson($scope.CommonMonthlySummary));
            //$scope.newSummary.Name = item.Name;
            //$scope.newSummary.AHCCCSID = item.AHCCCSID;
            //$scope.newSummary.ReferralID = item.ReferralID;
            //$scope.newSummary.CISNumber = item.CISNumber;
            //$scope.newSummary.PayorName = item.PayorName;
            //$scope.newSummary.CaseManager = item.CaseManager;
            //$scope.newSummary.ErrorCount = [];

            ////item.
            ////#region Monthaly Summary Form Validation
            //$scope.$watchCollection('newSummary.MoodforThroughoutWeekendIds', function (newValue, oldValue) {
            //    
            //    if (newValue != null && newValue.length === 0) {
            //        $scope.newSummary.MoodforThroughoutWeekendIds = null;
            //    }
            //});
            //$scope.$watchCollection('newSummary.CoordinationofCareatPickupIds', function (newValue, oldValue) {
            //    
            //    if (newValue != null && newValue.length === 0) {
            //        $scope.newSummary.CoordinationofCareatPickupIds = null;
            //    }
            //});

            //$scope.$watchCollection('newSummary.CoordinationofCareatDropOffIds', function (newValue, oldValue) {
            //    
            //    if (newValue != null && newValue.length === 0) {
            //        $scope.newSummary.CoordinationofCareatDropOffIds = null;
            //    }
            //});



            monthlySummaries.push(item.newSummary);
        });

        $scope.MonthlySummaryList = monthlySummaries;


        $timeout(function () {
            $.each($(".step3GMS .musthave input"), function (index, item) {
                if ($(item).is(":checked")) {
                    if ($(item).parent("span.musthave"))
                        $(item).parent("span.musthave").addClass("checked");
                } else {
                    if ($(item).parent("span.musthave"))
                        $(item).parent("span.musthave").removeClass("checked");
                }
            });
        });
    };


    $scope.RemoveMonthlySummary = function (item) {
        $scope.MonthlySummaryList.remove(item);
        $scope.SelectedClientIds.remove(item.ReferralID);

        $.each($scope.SelectedClients, function (i, data) {
            if (item.ReferralID == data.ReferralID) {
                $scope.SelectedClients.remove(data);
            }
        });

        $scope.SearchClientForNote();
    };


    $scope.ValidateForm = function (form) {
        var isValid = CheckErrors($("#" + form), true);
        $timeout(function () {
            if (isValid)
                $("#" + form).parents('.form-group').find(".error.badge-success").show();
            else
                $("#" + form).parents('.form-group').find(".error.badge-success").hide();

        });
        return isValid;
    };


    $scope.ValidateAll = function () {
        var isAllFormValid = true;
        $.each($scope.MonthlySummaryList, function (i, data) {
            var isValid = $scope.ValidateForm('frmSummary-' + data.ReferralID);
            if (!isValid) {
                isAllFormValid = false;
            }
        });
        return isAllFormValid;

    };

    $scope.SaveAllSummary = function () {
        if (!$scope.ValidateAll()) {
            return;
        }
        var jsonData = angular.toJson({
            monthlySummaries: $scope.MonthlySummaryList
        });
        AngularAjaxCall($http, SiteUrl.SaveMultipleMonthlySummaryURL, jsonData, "post", "json", "application/json", true).
            success(function (response) {
                ShowMessages(response);
                $scope.IsFinalPage = true;
                $scope.GroupNoteMsg = response.Data;
            });
    };

    //#endregion

    $scope.GetSnapshotPrintReport = function (ele) {
        
        if ($scope.GroupNoteMsg.ReferralMonthlySummariesIDs && $scope.GroupNoteMsg.ReferralMonthlySummariesIDs.length > 0) {

            var jsonData = angular.toJson({ searchSnapshotPrintModel: { ReferralMonthlySummariesIDs: $scope.GroupNoteMsg.ReferralMonthlySummariesIDs.join() } });
            AngularAjaxCall($http, SiteUrl.GetSnapshotPrintReportUrl, jsonData, "Post", "json", "application/json", true).success(function (response) {
                if (response.IsSuccess) {
                    window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;                    
                } else {                    
                    ShowMessages(response);
                }
               
            });
        }
    };

    

    //#region Other Functions

    $scope.CollapseAll = function () {
        $('.panel-collapse.in').collapse('hide');
    };

    $scope.ExpandAll = function () {
        $('.panel-collapse:not(".in")').collapse('show');
    };

    $scope.ColSizeOne = false;
    $scope.SwitchView = function () {
        $scope.ColSizeOne = !$scope.ColSizeOne;
    };
    //#endregion Other Functions






    //#region Scrap


    ////#region STEP 2
    //$scope.GroupNoteModel.ValidateServiceCodePassed = 0;//0: failed, 1: passed, 2:failed
    //$scope.GroupNoteModel.Note.ShowTime = false;
    //$scope.GroupNoteModel.Note.ShowMile = false;
    //$scope.GroupNoteModel.CleanAllFields = false;

    ////#region Token input related code for service code
    //$scope.GroupNoteModel.GN_ServiceCodeResultsFormatter = function (item) {

    //    return "<li id='{0}'>{0}: {1}<br/><small><b>{6}:</b> {2}</small><small><b style='padding-left:10px;'>{7}: </b>{3}</small><br/><small><b style='color:#ad0303;'>{8}: </b>{4}</small><small><b style='color:#ad0303;padding-left:10px;'>{9}: </b>{5} </small></li>"
    //          .format(
    //          item.ServiceCode,
    //          item.ServiceName,
    //          item.MaxUnit,
    //          item.DailyUnitLimit,
    //          item.IsBillable ? window.Yes : window.No,
    //          item.HasGroupOption ? window.Yes : window.No,
    //          window.MaxUnit,
    //          window.DailyUnitLimit,
    //          window.Billable,
    //          window.GroupOption);
    //};
    //$scope.GroupNoteModel.GN_ServiceCodeTokenFormatter = function (item) {
    //    return "<li id='{0}'>{0}</li>".format(item.ServiceCode);
    //};

    //$scope.GroupNoteModel.GN_AddedServiceCode = function (item) {

    //    $scope.GroupNoteModel.Note.UnitType = $scope.GroupNoteModel.Note.ServiceCodeID == item.ServiceCodeID ? $scope.GroupNoteModel.Note.UnitType : item.UnitType;
    //    $scope.GroupNoteModel.Note.SelectedServiceCodeDetails = item;
    //    $scope.GroupNoteModel.Note.ServiceCode = item.ServiceCode;
    //    $scope.GroupNoteModel.Note.ServiceCodeID = item.ServiceCodeID;


    //    if (!$scope.$root.$$phase) {
    //        $scope.$apply();
    //    }
    //};


    //$scope.GroupNoteModel.RemoveServiceCode = function () {
    //    $scope.GroupNoteModel.ValidateServiceCodePassed = 0;//0: failed, 1: passed, 2:failed
    //    if ($scope.GroupNoteModel.CleanAllFields) {
    //        $scope.GroupNoteModel.Note.PosID = null;
    //        $scope.GroupNoteModel.Note.ServiceCodeID = null;
    //        $scope.GroupNoteModel.Note.ServiceCode = null;
    //        $scope.GroupNoteModel.Note.UnitType = null;
    //        $scope.GroupNoteModel.PosCodes = [];
    //        $scope.GroupNoteModel.SelectedServiceCodeForPayor = {};
    //        $scope.GroupNoteModel.Note.StartMile = null;
    //        $scope.GroupNoteModel.Note.EndMile = null;
    //        $scope.GroupNoteModel.Note.StrStartTime = null;
    //        $scope.GroupNoteModel.Note.StrEndTime = null;
    //        $scope.GroupNoteModel.Note.NoOfStops = null;
    //        $scope.GroupNoteModel.Note.POSDetail = null;
    //        $scope.GroupNoteModel.Note.CalculatedUnit = 0;
    //    }
    //    $scope.GroupNoteModel.CleanAllFields = true;
    //    if (!$scope.$root.$$phase) {
    //        $scope.$apply();
    //    }
    //};
    ////#endregion

    //$scope.GroupNoteModel.ValidateServiceCode = function () {

    //    $scope.GroupNoteModel.ValidateMsg = "";
    //    if ($scope.GroupNoteModel.Note.ServiceDate && $scope.GroupNoteModel.Note.ServiceCodeType && $scope.GroupNoteModel.Note.ServiceCodeID
    //        && $scope.GroupNoteModel.Note.PosID) {

    //        var payorList = [];
    //        $scope.SelectedClients.filter(function (item, index) {
    //            if (payorList.indexOf(item.PayorID) == -1)
    //                payorList.push(item.PayorID);
    //        });

    //        var jsonData = angular.toJson({
    //            ServiceDate: $scope.GroupNoteModel.Note.ServiceDate,
    //            ServiceCodeID: $scope.GroupNoteModel.Note.ServiceCodeID,
    //            PosID: $scope.GroupNoteModel.Note.PosID,
    //            PayorCsv: payorList.toString()
    //        });
    //        AngularAjaxCall($http, SiteUrl.ValidateServiceCodeDetailsURL, jsonData, "Post", "json", "application/json").success(function (response) {

    //            $scope.GroupNoteModel.ValidateServiceCodePassed = response.IsSuccess ? 1 : 2;
    //            $scope.GroupNoteModel.ValidateServiceCodeMsg = response.Message;

    //            if ($scope.GroupNoteModel.Note.SelectedServiceCodeDetails.UnitType == window.TimeUnit)
    //                $scope.GroupNoteModel.Note.ShowMile = false;
    //            else
    //                $scope.GroupNoteModel.Note.ShowMile = true;


    //        });

    //    } else {
    //        $scope.ListofFieldsForValidation = "";
    //        var liString = "<li>{0} : {1}</li>";
    //        $scope.ListofFields = "<ul>";
    //        if (!$scope.GroupNoteModel.Note.ServiceDate) {
    //            $scope.ListofFieldsForValidation += liString.format(window.Required, window.ServiceDate);
    //        }

    //        if (!$scope.GroupNoteModel.Note.ServiceCodeType) {
    //            $scope.ListofFieldsForValidation += liString.format(window.Required, window.ServiceCodeType);
    //        }

    //        if (!$scope.GroupNoteModel.Note.ServiceCodeID) {
    //            $scope.ListofFieldsForValidation += liString.format(window.Required, window.ServiceCode);
    //        }
    //        if (!$scope.GroupNoteModel.Note.PosID) {
    //            $scope.ListofFieldsForValidation += liString.format(window.Required, window.POS);
    //        }

    //        bootboxDialog(function () {
    //        }, bootboxDialogType.Alert, window.Alert, window.FieldsRequired.format($scope.ListofFieldsForValidation));
    //    }


    //    //value.Note.ServiceDate = $scope.GroupNoteModel.Note.ServiceDate;
    //    //value.Note.ServiceCodeType = $scope.GroupNoteModel.Note.ServiceCodeType;
    //    //value.Note.ServiceCodeID = $scope.GroupNoteModel.Note.ServiceCodeID;
    //    //value.Note.PosID = $scope.GroupNoteModel.Note.PosID;

    //};
    ////#endregion

    ////#region Watchers

    ////SERVICE CODE TYPE AND SERVICE START DATE WATCHER
    //$scope.$watch(function () {
    //    return $scope.GroupNoteModel.Note.ServiceCodeType;
    //}, function (newValue, oldValue) {

    //    if (newValue != oldValue) $scope.GroupNoteModel.ValidateServiceCodePassed = 0; //0: failed

    //    if ($scope.GroupNoteModel.Note.ServiceCodeID > 0) {
    //        $scope.GroupNoteModel.CleanAllFields = true;
    //        try {
    //            $scope.GroupNoteModel.GN_ServiceCodeTokenObj.clear();
    //        } catch (e) {
    //        }
    //    }
    //    if ($scope.GroupNoteModel.Note.ServiceCodeType && $scope.GroupNoteModel.Note.ServiceCodeType.toString() != "") {
    //        $scope.GroupNoteModel.GN_AdditionFilterForServiceCode = angular.toJson({
    //            serviceCodeTypeID: $scope.GroupNoteModel.Note.ServiceCodeType
    //        });

    //    }
    //    if ($scope.GroupNoteModel.Note.ServiceCodeType != $scope.OtherServiceCode) {
    //        $scope.GroupNoteModel.Note.SpokeTo = null;
    //        $scope.GroupNoteModel.Note.Relation = null;
    //        $scope.GroupNoteModel.Note.OtherNoteType = null;
    //        $scope.GroupNoteModel.Note.IsIssue = false;
    //        $scope.GroupNoteModel.Note.IssueAssignID = "";

    //    }


    //    if ($scope.GroupNoteModel.Note.ServiceCodeType == $scope.OtherServiceCode) {
    //        angular.forEach($scope.GroupNoteModel.Facilities, function (value, key) {
    //            if (value.Name == window.FacilityForOtherServiceCode) {
    //                $scope.GroupNoteModel.Note.BillingProviderID = value.Value;
    //                $scope.GroupNoteModel.Note.RenderingProviderID = value.Value;
    //                return;
    //            }
    //        });
    //    }
    //});

    ////PLACE OF SERVICE CHANGE WATCHER
    //$scope.$watch(function () { return $scope.GroupNoteModel.Note.PosID; }, function (newValue, oldValue) {

    //    if (newValue != oldValue) $scope.GroupNoteModel.ValidateServiceCodePassed = 0; //0: failed
    //    if (parseInt(newValue) > 0) {
    //        if (newValue && newValue != $scope.OtherPOS) {
    //            $scope.GroupNoteModel.Note.POSDetail = window.CommunityMentalHealthCenter;
    //        } else {
    //            if (newValue != oldValue)
    //                $scope.GroupNoteModel.Note.POSDetail = null;
    //        }
    //    } else if (newValue != oldValue && parseInt(oldValue) > 0) {
    //        $scope.GroupNoteModel.SelectedServiceCodeForPayor = {};
    //        $scope.GroupNoteModel.Note.StartMile = null;
    //        $scope.GroupNoteModel.Note.EndMile = null;
    //        $scope.GroupNoteModel.Note.StrStartTime = null;
    //        $scope.GroupNoteModel.Note.StrEndTime = null;
    //        $scope.GroupNoteModel.Note.NoOfStops = null;
    //        $scope.GroupNoteModel.Note.POSDetail = null;
    //        //if (!$scope.$root.$$phase) {
    //        //    $scope.$apply();
    //        //}
    //    }
    //    //$scope.GroupNoteModel.Note.PayorServiceCodeMappingID = $scope.GroupNoteModel.SelectedServiceCodeForPayor.PayorServiceCodeMappingID;
    //});

    ////#endregion

    ////#region OTHER Methods

    //$scope.Save = function () {
    //    var isAllFormValid = true;
    //    $('form').each(function () {

    //        var thisForm = $(this);
    //        var isValid = CheckErrors($(this), true);
    //        if (!isValid) {
    //            isAllFormValid = false;
    //           }

    //        $timeout(function() {
    //            if (isValid)
    //                $(thisForm).parents('.form-group').find(".error.badge-success").show();
    //            else
    //                $(thisForm).parents('.form-group').find(".error.badge-success").hide();
    //        });

    //    });

    //    if (isAllFormValid) {
    //        var jsonData = angular.toJson({
    //            saveGroupNoteModel: $scope.SelectedClients
    //        });
    //        AngularAjaxCall($http, SiteUrl.SaveGroupNoteURL, jsonData, "post", "json", "application/json", true).
    //            success(function(response) {
    //                ShowMessages(response);
    //                $scope.IsFinalPage = true;
    //                $scope.GroupNoteMsg = response.Data;
    //            });
    //    } else {
    //        ShowMessage(window.CanNotSave, "error");
    //    }

    //};



    //$scope.SearchClientForNote();

    ////#endregion

    //#endregion



};

var ReferralMonthalySummary = function ($scope, $http, $timeout, item) {
    var self = item;

    self.newSummary = $.parseJSON(angular.toJson($scope.CommonMonthlySummary));
    self.newSummary.Name = item.Name;
    self.newSummary.AHCCCSID = item.AHCCCSID;
    self.newSummary.ReferralID = item.ReferralID;
    self.newSummary.CISNumber = item.CISNumber;
    self.newSummary.PayorName = item.PayorName;
    self.newSummary.CaseManager = item.CaseManager;
    self.newSummary.ErrorCount = [];

    //item.
    //#region Monthaly Summary Form Validation
    $scope.$watchCollection(function () {
        return self.newSummary.MoodforThroughoutWeekendIds;
    }, function (newValue, oldValue) {
        //
        if (newValue != null && newValue.length === 0) {
            self.newSummary.MoodforThroughoutWeekendIds = null;
        }
    });
    $scope.$watchCollection(function () {
        return self.newSummary.CoordinationofCareatPickupIds;
    }, function (newValue, oldValue) {
        //
        if (newValue != null && newValue.length === 0) {
            self.newSummary.CoordinationofCareatPickupIds = null;
        }
    });

    $scope.$watchCollection(function () {
        return self.newSummary.CoordinationofCareatDropOffIds;
    }, function (newValue, oldValue) {
        //
        if (newValue != null && newValue.length === 0) {
            self.newSummary.CoordinationofCareatDropOffIds = null;
        }
    });


    //#region CHECK SERVICE DATE & FACILTY BASED ON 
   
    self.newSummary.FaciltyAndServiceDateMappingIsValide = true;
    self.ValidateFaciltyAndServiceDate = function (startDate, endDate, facilityId, referralId) {
        var data = {
            model: { StartDate: startDate, EndDate: endDate, FacilityId: facilityId, ReferralID: referralId }
        };
        AngularAjaxCall($http, SiteUrl.FindScheduleWithFaciltyAndServiceDateURL, angular.toJson(data), "Post", "json", "application/json", false).success(function(response) {
            if (response.IsSuccess) {
                self.newSummary.FaciltyAndServiceDateMappingIsValide = response.Data;
            }
            ShowMessages(response);
        });
    };

    $scope.$watch(function () {
        return moment(new Date(self.newSummary.MonthlySummaryStartDate)).format("YYYY/MM/DD")
            + moment(new Date(self.newSummary.MonthlySummaryEndDate)).format("YYYY/MM/DD")
            + self.newSummary.FacilityID;
    }, function () {

        if (self.newSummary.FacilityID > 0 && ValideDate(self.newSummary.MonthlySummaryStartDate) && ValideDate(self.newSummary.MonthlySummaryEndDate)) {
            
            var sdate = self.newSummary.MonthlySummaryStartDate;
            var edate = self.newSummary.MonthlySummaryEndDate;
            var fid = self.newSummary.FacilityID;
            var rid = self.newSummary.ReferralID;
            self.ValidateFaciltyAndServiceDate(sdate, edate, fid, rid);
        }
    });
    //#endregion




};

//var ReferralNote = function ($scope, $http, $timeout, model) {
//    var self = model;
//    self.AdditionFilterForServiceCode = angular.toJson({
//        encReferralID: self.EncryptedReferralID,
//        serviceDate: self.Note.ServiceDate,
//        serviceCodeTypeID: self.Note.ServiceCodeType
//    });
//    self.SelectedServiceCodeForPayor = {};
//    self.ServiceCodeTokenObj = {};
//    self.CleanAllFields = true;
//    self.AllowToGetServiceCode = true;
//    self.SelectedDxCodes = [];

//    //#region Token input related code for service code
//    self.ServiceCodeResultsFormatter = function (item) {
//        //self.Note.ServiceCode = item.ServiceCode;
//        return "<li id='{0}'>{0}: {1}<br/><small><b>{6}:</b> {2}</small><small><b style='padding-left:10px;'>{7}: </b>{3}</small><br/><small><b style='color:#ad0303;'>{8}: </b>{4}</small><small><b style='color:#ad0303;padding-left:10px;'>{9}: </b>{5} </small></li>"
//              .format(
//              item.ServiceCode,
//              item.ServiceName,
//              item.MaxUnit,
//              item.DailyUnitLimit,
//              item.IsBillable ? window.Yes : window.No,
//              item.HasGroupOption ? window.Yes : window.No,
//              window.MaxUnit,
//              window.DailyUnitLimit,
//              window.Billable,
//              window.GroupOption);
//    };
//    self.ServiceCodeTokenFormatter = function (item) {
//        return "<li id='{0}'>{0}</li>".format(item.ServiceCode);
//    };

//    self.AddedServiceCode = function (item) {
//        self.Note.SelectedServiceCodeDetails = item;
//        self.Note.ServiceCode = item.ServiceCode;
//        self.Note.ServiceCodeID = item.ServiceCodeID;
//        self.Note.UnitType = item.UnitType;
//        if (!$scope.$root.$$phase) {
//            $scope.$apply();
//        }
//    };

//    self.RemoveServiceCode = function () {
//        if (self.CleanAllFields) {
//            self.Note.PosID = null;
//            self.Note.ServiceCodeID = null;
//            self.Note.ServiceCode = null;
//            self.PosCodes = [];
//            self.SelectedServiceCodeForPayor = {};
//            self.Note.StartMile = null;
//            self.Note.EndMile = null;
//            self.Note.StrStartTime = null;
//            self.Note.StrEndTime = null;
//            self.Note.NoOfStops = null;
//            self.Note.POSDetail = null;
//            self.Note.CalculatedUnit = 0;
//        }
//        self.CleanAllFields = true;
//        if (!$scope.$root.$$phase) {
//            $scope.$apply();
//        }
//    };




//    //#endregion

//    //SERVICE CODE TYPE AND SERVICE DATE CHANGE WATCHER
//    $scope.$watch(function () {
//        return moment(new Date(self.Note.ServiceDate)).format("YYYY/MM/DD") + self.Note.ServiceCodeType;
//    }, function () {
//        if (self.Note.ServiceCodeID > 0) {
//            self.CleanAllFields = true;

//            try {
//                self.ServiceCodeTokenObj.clear();
//            } catch (e) {
//            }
//        }
//        if (! (self.Note.ServiceDate && self.Note.ServiceDate.toString() != "")) {
//            self.Note.ServiceDate = '0001-01-01T00:00:00';
//        }

//        self.SelectFirstDxCode('.dxCode0',0);

//        self.AdditionFilterForServiceCode = angular.toJson({
//            encReferralID: self.EncryptedReferralID,
//            serviceDate: self.Note.ServiceDate,
//            serviceCodeTypeID: self.Note.ServiceCodeType
//        });

//        if (self.Note.ServiceCodeType != $scope.OtherServiceCode) {
//            self.Note.SpokeTo = null;
//            self.Note.Relation = null;
//            self.Note.OtherNoteType = null;
//            self.Note.IsIssue = false;
//            self.Note.IssueAssignID = "";
//            //if (!$scope.$root.$$phase) {
//            //    $scope.$apply();
//            //}
//        }


//        if (self.Note.ServiceCodeType == $scope.OtherServiceCode) {
//            angular.forEach($scope.GroupNoteModel.Facilities, function (value, key) {
//                if (value.Name == window.FacilityForOtherServiceCode) {
//                    self.Note.BillingProviderID = value.Value;
//                    self.Note.RenderingProviderID = value.Value;
//                    return;
//                }
//            });
//        }
//    });


//    //SERVICE CODE  CHANGE WATCHER
//    $scope.$watch(function () {
//        return self.Note.ServiceCodeID;
//    }, function (newValue, oldValue) {
//        if (parseInt(newValue) > 0) {
//            self.GetPosCodes(newValue);
//            self.Note.PayorServiceCodeMappingID = self.SelectedServiceCodeForPayor.PayorServiceCodeMappingID;
//        } else if (oldValue > 0) {
//            self.AllowToGetServiceCode = true;
//        }
//    });

//    self.GetPosCodes = function (value) {
//        if (self.AllowToGetServiceCode) {
//            var jsonData = angular.toJson({
//                encReferralID: self.EncryptedReferralID,
//                serviceDate: self.Note.ServiceDate,
//                serviceCodeID: value,
//                noteID: self.Note.NoteID > 0 ? self.Note.NoteID : 0
//            });
//            AngularAjaxCall($http, SiteUrl.GetPosCodesURL, jsonData, "post", "json", "application/json", true).
//                success(function (response) {
//                    if (response.IsSuccess) {
//                        self.PosCodes = response.Data;
//                        self.Note.PosID = self.Note.PosID ? parseInt(self.Note.PosID):"";
//                        if (self.Note.PosID > 0 && self.Note.ServiceCodeID > 0) {
//                            angular.forEach(self.PosCodes, function (val, key) {
//                                if (val.PosID == self.Note.PosID) {
//                                    self.SelectedServiceCodeForPayor = val;

//                                    if (val.UnitType == window.TimeUnit) {
//                                        self.CalculateTimeUnit(self.Note.StrStartTime, self.Note.StrEndTime, val.UnitType);
//                                    }
//                                    else if (val.UnitType != window.TimeUnit) {
//                                        self.CalculateMileUnit(self.Note.StartMile, self.Note.EndMile, val.UnitType);
//                                    }

//                                    return;
//                                }
//                            });
//                        }

//                    }
//                    self.AllowToGetServiceCode = true;

//                });
//        }
//    };

//    //PLACE OF SERVICE CHANGE WATCHER
//    $scope.$watch(function () { return self.Note.PosID; }, function (newValue, oldValue) {
//        if (parseInt(newValue) > 0) {
//            angular.forEach(self.PosCodes, function (value, key) {
//                if (value.PosID == self.Note.PosID) {
//                    self.SelectedServiceCodeForPayor = value;
//                    return;
//                }
//            });
//            if (self.SelectedServiceCodeForPayor.PosID && self.SelectedServiceCodeForPayor.PosID != $scope.OtherPOS) {
//                self.Note.POSDetail = window.CommunityMentalHealthCenter;
//            } else {
//                if (newValue != oldValue)
//                    self.Note.POSDetail = null;
//            }
//        } else if (newValue != oldValue && parseInt(oldValue) > 0) {
//            self.SelectedServiceCodeForPayor = {};
//            self.Note.StartMile = null;
//            self.Note.EndMile = null;
//            self.Note.StrStartTime = null;
//            self.Note.StrEndTime = null;
//            self.Note.NoOfStops = null;
//            self.Note.POSDetail = null;
//            //if (!$scope.$root.$$phase) {
//            //    $scope.$apply();
//            //}
//        }
//        self.Note.PayorServiceCodeMappingID = self.SelectedServiceCodeForPayor.PayorServiceCodeMappingID;
//    });

//    //#region Calculate Units depends on date and mile value
//    $scope.$watch(function () {
//        return self.Note.StartMile + self.Note.EndMile;
//    }, function () {
//        if (parseInt(self.Note.StartMile) >= 0 && parseInt(self.Note.EndMile) > 0 &&
//            ((self.SelectedServiceCodeForPayor.UnitType && (self.SelectedServiceCodeForPayor.UnitType == window.StopUnit || self.SelectedServiceCodeForPayor.UnitType == window.DistanceInMilesUnit))
//            || (self.Note.UnitType && (self.Note.UnitType == window.StopUnit || self.Note.UnitType == window.DistanceInMilesUnit))
//            )
//        ) {
//            if ((parseInt(self.Note.EndMile) >= parseInt(self.Note.StartMile))) {
//                self.MileDifference = self.Note.EndMile - self.Note.StartMile;
//                var calUnit = Math.round(self.MileDifference / self.SelectedServiceCodeForPayor.PerUnitQuantity);
//                self.SelectedServiceCodeForPayor.UsedUnit = calUnit;

//                self.Note.CalculatedUnit = calUnit;
//                self.SelectedServiceCodeForPayor.CalculatedUnit = calUnit;

//                //if ((self.SelectedServiceCodeForPayor.DailyUnitLimit == 0 && self.SelectedServiceCodeForPayor.MaxUnit == 0) || self.SelectedServiceCodeForPayor.UsedUnit <= self.SelectedServiceCodeForPayor.AvailableDailyUnit) {
//                //    self.Note.CalculatedUnit = self.SelectedServiceCodeForPayor.UsedUnit;
//                //    self.SelectedServiceCodeForPayor.CalculatedUnit = self.SelectedServiceCodeForPayor.UsedUnit;
//                //} else if (self.SelectedServiceCodeForPayor.DailyUnitLimit == 0 && self.SelectedServiceCodeForPayor.MaxUnit != 0) {
//                //    if (self.SelectedServiceCodeForPayor.UsedUnit <= self.SelectedServiceCodeForPayor.AvailableMaxUnit) {
//                //        self.Note.CalculatedUnit = self.SelectedServiceCodeForPayor.UsedUnit;
//                //        self.SelectedServiceCodeForPayor.CalculatedUnit = self.SelectedServiceCodeForPayor.UsedUnit;
//                //    } else {
//                //        self.Note.CalculatedUnit = self.SelectedServiceCodeForPayor.AvailableDailyUnit;
//                //        self.SelectedServiceCodeForPayor.CalculatedUnit = self.SelectedServiceCodeForPayor.AvailableDailyUnit;
//                //    }

//                //} else {
//                //    self.Note.CalculatedUnit = self.SelectedServiceCodeForPayor.AvailableDailyUnit;
//                //    self.SelectedServiceCodeForPayor.CalculatedUnit = self.SelectedServiceCodeForPayor.AvailableDailyUnit;
//                //}
//            } else {
//                self.SelectedServiceCodeForPayor.UsedUnit = 0;
//                self.Note.CalculatedUnit = 0;
//                self.SelectedServiceCodeForPayor.CalculatedUnit = 0;
//            }

//        } else {
//            self.SelectedServiceCodeForPayor.UsedUnit = 0;
//            self.Note.CalculatedUnit = 0;
//            self.SelectedServiceCodeForPayor.CalculatedUnit = 0;
//        }
//        if (self.Note.EndMile == "" || self.Note.StartMile == "") {
//            self.SelectedServiceCodeForPayor.UsedUnit = 0;
//            self.Note.CalculatedUnit = 0;
//            self.SelectedServiceCodeForPayor.CalculatedUnit = 0;
//        }

//        if (!$scope.$root.$$phase) {
//            $scope.$apply();
//        }
//    });

//    $scope.$watch(function () {
//        return self.Note.StrStartTime + self.Note.StrEndTime;
//    }, function () {
//        if (self.Note.StrStartTime && self.Note.StrEndTime && self.CheckValidTime(self.Note.StrStartTime) && self.CheckValidTime(self.Note.StrEndTime) &&
//            ((self.SelectedServiceCodeForPayor.UnitType && self.SelectedServiceCodeForPayor.UnitType == window.TimeUnit) || (self.Note.UnitType && self.Note.UnitType == window.TimeUnit))) {

//            if (new Date(moment(self.Note.StrEndTime, "hh:mm a")).getTime() > new Date(moment(self.Note.StrStartTime, "hh:mm a")).getTime()) {
//                var diff = self.GetTimeDifferenceInMinutes(self.Note.StrStartTime, self.Note.StrEndTime);
//                var calUnit = Math.round(diff / self.SelectedServiceCodeForPayor.PerUnitQuantity);
//                self.SelectedServiceCodeForPayor.UsedUnit = calUnit;

//                self.Note.CalculatedUnit = calUnit;
//                self.SelectedServiceCodeForPayor.CalculatedUnit = calUnit;



//                //if ((self.SelectedServiceCodeForPayor.DailyUnitLimit == 0 && self.SelectedServiceCodeForPayor.MaxUnit == 0) || self.SelectedServiceCodeForPayor.UsedUnit <= self.SelectedServiceCodeForPayor.AvailableDailyUnit) {
//                //    self.Note.CalculatedUnit = self.SelectedServiceCodeForPayor.UsedUnit;
//                //    self.SelectedServiceCodeForPayor.CalculatedUnit = self.SelectedServiceCodeForPayor.UsedUnit;
//                //} else if (self.SelectedServiceCodeForPayor.DailyUnitLimit == 0 && self.SelectedServiceCodeForPayor.MaxUnit != 0) {
//                //    if (self.SelectedServiceCodeForPayor.UsedUnit <= self.SelectedServiceCodeForPayor.AvailableMaxUnit) {
//                //        self.Note.CalculatedUnit = self.SelectedServiceCodeForPayor.UsedUnit;
//                //        self.SelectedServiceCodeForPayor.CalculatedUnit = self.SelectedServiceCodeForPayor.UsedUnit;
//                //    } else {
//                //        self.Note.CalculatedUnit = self.SelectedServiceCodeForPayor.AvailableDailyUnit;
//                //        self.SelectedServiceCodeForPayor.CalculatedUnit = self.SelectedServiceCodeForPayor.AvailableDailyUnit;
//                //    }

//                //} else {
//                //    self.Note.CalculatedUnit = self.SelectedServiceCodeForPayor.AvailableDailyUnit;
//                //    self.SelectedServiceCodeForPayor.CalculatedUnit = self.SelectedServiceCodeForPayor.AvailableDailyUnit;
//                //}
//            }
//            else {
//                self.SelectedServiceCodeForPayor.UsedUnit = 0;
//                self.Note.CalculatedUnit = 0;
//                self.SelectedServiceCodeForPayor.CalculatedUnit = 0;
//            }
//        }
//        if (!$scope.$root.$$phase) {
//            $scope.$apply();
//        }
//    });


//    self.CalculateTimeUnit = function(startValue, endValue, unitType) {

//        if (unitType == window.TimeUnit) //TIME UNIT  CALCULATION
//        {
//            if (startValue && endValue && self.CheckValidTime(startValue) && self.CheckValidTime(endValue)) {

//                if (new Date(moment(endValue, "hh:mm a")).getTime() > new Date(moment(startValue, "hh:mm a")).getTime()) {
//                    var diff = self.GetTimeDifferenceInMinutes(startValue, endValue);
//                    var tempCalUnit = Math.round(diff / self.SelectedServiceCodeForPayor.PerUnitQuantity);
//                    self.SelectedServiceCodeForPayor.UsedUnit = tempCalUnit;
//                    self.Note.CalculatedUnit = tempCalUnit;
//                    self.SelectedServiceCodeForPayor.CalculatedUnit = tempCalUnit;
//                } else {
//                    self.SelectedServiceCodeForPayor.UsedUnit = 0;
//                    self.SelectedServiceCodeForPayor.CalculatedUnit = 0;
//                }
//                if (!$scope.$root.$$phase)
//                    $scope.$apply();
//            }

//        }
//    };


//    self.CalculateMileUnit = function(startValue, endValue, unitType) {

//        if (unitType != window.TimeUnit) //TIME UNIT  CALCULATION
//        {
//            if (parseInt(startValue) >= 0 && parseInt(endValue) > 0) {
//                if ((parseInt(endValue) >= parseInt(startValue))) {
//                    self.MileDifference = endValue - startValue;
//                    var calUnit = Math.round(self.MileDifference / self.SelectedServiceCodeForPayor.PerUnitQuantity);
//                    self.SelectedServiceCodeForPayor.UsedUnit = calUnit;
//                    self.Note.CalculatedUnit = calUnit;
//                    self.SelectedServiceCodeForPayor.CalculatedUnit = calUnit;

//                } else {
//                    self.SelectedServiceCodeForPayor.UsedUnit = 0;
//                    self.SelectedServiceCodeForPayor.CalculatedUnit = 0;
//                }

//            } else {
//                self.SelectedServiceCodeForPayor.UsedUnit = 0;
//                self.SelectedServiceCodeForPayor.CalculatedUnit = 0;
//            }

//            if (!$scope.$root.$$phase)
//                $scope.$apply();
//        }

//    };

//    //#endregion

//    //#endregion

//    self.CheckValidTime = function (str) {
//        var validTime = str.match(/^(0?[0-9]|1[012])(:[0-5]\d) [APap][mM]$/);
//        return validTime;
//    };

//    self.GetTimeDifferenceInMinutes = function (date1, date2) {
//        if (date1 != undefined && date2 != undefined) {
//            var difference = new Date(moment(date2, "hh:mm a")).getTime() - new Date(moment(date1, "hh:mm a")).getTime(); // This will give difference in milliseconds
//            return Math.round(difference / 60000);
//        }
//        return null;
//    };


//    self.ValidationPassed = false;
//    self.ValidateForm = function (form) {
//        var isValid = CheckErrors($("#" + form), true);
//        $timeout(function () {
//            if (isValid)
//                $("#" + form).parents('.form-group').find(".error.badge-success").show();
//            else 
//                $("#" + form).parents('.form-group').find(".error.badge-success").hide();

//        });
//        return isValid;
//    };

//    self.RemoveClient = function (client) {
//        $scope.SelectedClients.remove(client);
//        $scope.SelectedClientIds.remove(client.ReferralID);
//    };

//    self.IsDifferentDxCode = false;
//    self.GroupNoteDxCodeCount = 0;

//    self.SelectFirstDxCode = function (checkbox, index) {
//        $timeout(function () {

//            if (index == 0) {
//                $(checkbox).filter(function (index, data) {
//                    if ($(data).is(':checked')) {
//                        $(data).click();
//                    }
//                    $(data).click();
//                });

//                //if ($(checkbox).is(':checked')) {
//                //    $(checkbox).click();
//                //}
//                //$(checkbox).click();

//                //dxCode.IsChecked = true;
//                // self.SelectDxCode(dxCode);
//            }
//        });
//    };

//    self.SelectDxCode = function (dxCode) {
//        if (dxCode.IsChecked) {
//            var count = 0;
//            if (self.SelectedDxCodes.length > 0) {
//                count = self.SelectedDxCodes.filter(function (item) {
//                    return item.DxCodeShortName != dxCode.DxCodeShortName;
//                }).length;
//            }
//            if (count > 0) {
//                self.IsDifferentDxCode = true;
//                dxCode.IsChecked = false;
//            } else {
//                self.SelectedDxCodes.push(dxCode);
//                self.GroupNoteDxCodeCount += 1;
//                self.IsDifferentDxCode = false;
//                //$(element).valid();               
//                //if (!$scope.$root.$$phase) {
//                //    $scope.$apply();
//                //}
//            }

//        } else {
//            self.SelectedDxCodes.remove(dxCode);
//            self.GroupNoteDxCodeCount -= 1;
//        }


//        if (self.SelectedDxCodes.length == self.DxCodes.length)
//            self.SelectAllDxCodeCheckbox = true;
//        else
//            self.SelectAllDxCodeCheckbox = false;

//    };

//    self.SelectAll = function () {
//        self.SelectedDxCodes = [];
//        self.GroupNoteDxCodeCount = 0;
//        angular.forEach(self.FilterDxCodes, function (item, key) {
//            item.IsChecked = self.SelectAllDxCodeCheckbox;
//            if (item.IsChecked) {
//                self.SelectedDxCodes.push(item);
//                self.GroupNoteDxCodeCount += 1;
//            }

//        });

//        return true;
//    };
//    self.IsSameDxCodeType = true;
//    self.ServiceCodeName = null;

//    self.DxCodeFilter = function () {
//        self.IsSameDxCodeType = true;
//        self.ServiceCodeName = null;
//        return function (item) {
//            if (self.Note.ServiceDate) {
//                var isValid = false;
//                if (moment(self.Note.ServiceDate) >= moment(item.StartDate)) {
//                    if (item.EndDate) {
//                        if (moment(self.Note.ServiceDate) <= moment(item.EndDate)) {
//                            if (self.ServiceCodeName && self.ServiceCodeName != item.DxCodeShortName) {
//                                self.IsSameDxCodeType = false;
//                            }
//                            self.ServiceCodeName = item.DxCodeShortName;
//                            isValid = true;
//                        }
//                        //else {

//                        //    self.IsSameDxCodeType = true;
//                        //}
//                    } else {
//                        if (self.ServiceCodeName && self.ServiceCodeName != item.DxCodeShortName) {
//                            self.IsSameDxCodeType = false;
//                        }
//                        self.ServiceCodeName = item.DxCodeShortName;
//                        isValid = true;
//                    }
//                }
//                //    else {
//                //    self.IsSameDxCodeType = true;
//                //}

//                if (isValid)
//                    return item;


//            }
//        };
//    };

//    return self;
//};



controllers.GroupMonthlySummaryController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {

    //$('.time').inputmask({
    //    mask: "h:s t\\m",
    //    placeholder: "hh:mm a",
    //    alias: "datetime",
    //    hourFormat: "12"
    //});

    ////$(".dateInputMask").inputmask("m/d/y", {
    ////    placeholder: "mm/dd/yyyy"
    ////});
    //$(".dateInputMask").attr("placeholder", "mm/dd/yy");

});