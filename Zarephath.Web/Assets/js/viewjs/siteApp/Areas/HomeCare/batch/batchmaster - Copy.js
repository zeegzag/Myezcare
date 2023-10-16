
var custModel;

controllers.BatchController = function ($scope, $http) {
    custModel = $scope;
    var modalJson = $.parseJSON($("#hdnBatchModel").val());
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnBatchModel").val());
    };
    $scope.BatchModel = modalJson;

    $scope.maxDate = new Date();
    $scope.maxDate.setDate($scope.maxDate.getDate() + 1);
    $scope.NewDate = SetExpiryDate();

    $scope.Batch = $scope.newInstance().Batch;
    $scope.BatchModel.ApprovedFacilityList = $scope.newInstance().ApprovedFacilityList;

    // get Referral/Patient List with Claim Number
    //#region
    $scope.OpenCreateBatchModel = function () {
        $scope.ClaimValidationMsg = "";
        $("#model__AddBatch").modal("show");
    };
    $('#model__AddBatch').on('hidden.bs.modal', function () {
        $scope.ResetPatientSearchFilter();
    });

    $scope.PatientList = [];
    $scope.SelectedPatientIds = [];
    $scope.SelectAllPatientsCheckbox = false;
    $scope.TempSearchPatientList = {};
    $scope.IsPageLoad = true;
    $scope.GetPatientList = function () {
        if (CheckErrors("#FrmBatch")) {
            if ($scope.BatchModel.SearchPatientList.ServiceCodeID)
                $scope.BatchModel.SearchPatientList.ServiceCodeIDs = $scope.BatchModel.SearchPatientList.ServiceCodeID.toString();
            else
                $scope.BatchModel.SearchPatientList.ServiceCodeIDs = '';

            $scope.TempSearchPatientList = angular.copy($scope.BatchModel.SearchPatientList);

            var jsonData = angular.toJson(
                $scope.BatchModel.SearchPatientList
            );

            AngularAjaxCall($http, HomeCareSiteUrl.GetPatientListURl, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.PatientList = response.Data.ListPatientModel;
                    $scope.IsPageLoad = false;
                }
                else {
                    ShowMessages(response);
                }
            });
        }
    };

    $scope.ResetPatientSearchFilter = function () {
        $scope.BatchModel.SearchPatientList.BatchTypeID = '1';
        $scope.BatchModel.SearchPatientList.PayorID = '';
        $scope.BatchModel.SearchPatientList.ServiceCodeIDs = '';
        $scope.BatchModel.SearchPatientList.ServiceCodeID = [];
        $scope.BatchModel.SearchPatientList.StartDate = null;
        $scope.BatchModel.SearchPatientList.EndDate = null;
        $scope.BatchModel.SearchPatientList.Comment = null;
        $scope.BatchModel.SearchPatientList.ClientName = null;
        $scope.PatientList = [];
        $scope.SelectedPatientIds = [];
        $scope.SelectAllPatientsCheckbox = false;
        $scope.TempSearchPatientList = {};
        HideErrors("#FrmBatch");
        $scope.IsPageLoad = true;
    };

    $scope.BackToSearchFilter = function () {
        $scope.PatientList = [];
        $scope.SelectedPatientIds = [];
        $scope.SelectAllPatientsCheckbox = false;
    };

    $scope.SelectPatient = function (patientList) {
        if (patientList.IsChecked)
            $scope.SelectedPatientIds.push(patientList.ReferralID);
        else
            $scope.SelectedPatientIds.remove(patientList.ReferralID);

        if ($scope.SelectedPatientIds.length == $scope.PatientList.length)
            $scope.SelectAllPatientsCheckbox = true;
        else
            $scope.SelectAllPatientsCheckbox = false;
    };

    $scope.SelectAllPatient = function () {
        $scope.SelectedPatientIds = [];
        angular.forEach($scope.PatientList, function (item, key) {
            item.IsChecked = $scope.SelectAllPatientsCheckbox;
            if (item.IsChecked)
                $scope.SelectedPatientIds.push(item.ReferralID);
        });
        return true;
    };
    //#endregion

    // get All Details of Claim(Notes) (Child Note Details)
    //#region


    




    $scope.GetChildNoteDetailsOfClaim = function (item, trElem) {


        if (item.ChildNoteList == undefined) {
            $scope.TempSearchPatientList.ReferralID = item.ReferralID;
            $scope.TempSearchPatientList.BatchID = item.BatchID;

            var jsonData = angular.toJson($scope.TempSearchPatientList);
            AngularAjaxCall($http, HomeCareSiteUrl.GetChildNoteDetailsURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    item.ChildNoteList = response.Data;
                    //$scope.ShowCollpase();
                    $scope.SetClaimsError();
                }
                ShowMessages(response);
            });
        }


        if ($(trElem).hasClass("fa-minus-circle") == false) {
            $(trElem).removeClass("fa-plus-circle").addClass("fa-minus-circle");
        }
        else {
            $(trElem).removeClass("fa-minus-circle").addClass("fa-plus-circle");
        }
    };

    $scope.ShowCollpase = function () {
        setTimeout(function () {
            $.each($('.collapseDestination'), function (index, data) {
                var parent = data;
                //$(parent).parent("tbody").find(".collapseSource").removeClass("fa-plus-circle").addClass("fa-minus-circle");

                $(parent).on('show.bs.collapse', function (e) {
                    if ($(this).is(e.target))
                        $(parent).parent("tbody").find(".collapseSource").removeClass("fa-plus-circle").addClass("fa-minus-circle");
                });
                $(parent).on('hidden.bs.collapse', function (e) {
                    if ($(this).is(e.target))
                        $(parent).parent("tbody").find(".collapseSource").removeClass("fa-minus-circle").addClass("fa-plus-circle");
                });
            });
        }, 100);
    };

    //#endregion

    // For View Batch Details in Edit Mode/ View Details Mode
    //#region
    $scope.PatientClaimsList = [];
    $scope.ViewBatchDetailsModel = {};

    
    $scope.GetBacthClaimDetails = function (item, elem) {

        var hasClassFaMinusCircle = $(elem).hasClass("fa-minus-circle");

        //Enable this to keep only single expand / collapse
        //$("[id!=OnPage_ChildNoteDetails-" + item.BatchID + "][id^=OnPage_ChildNoteDetails]").removeClass("in");
        //$("[id^=OnPage_CNDetails]").removeClass("fa-minus-circle").addClass("fa-plus-circle");

        if (hasClassFaMinusCircle == false) {
            $(elem).removeClass("fa-plus-circle").addClass("fa-minus-circle");
            $scope.OnPage_ViewBatchDetails(item);
        }
        else {
            $(elem).removeClass("fa-minus-circle").addClass("fa-plus-circle");
        }



        
    };


    $scope.OnPage_ViewBatchDetails = function (item) {

        if (item.ViewBatchDetailsModel == undefined) {

            $scope.ViewBatchDetailsModel.BatchID = item.BatchID;
            $scope.ViewBatchDetailsModel.IsBatchSent = item.IsSent;
            $scope.ViewBatchDetailsModel.PayorName = item.PayorName;
            $scope.ViewBatchDetailsModel.StartDate = item.StartDate;
            $scope.ViewBatchDetailsModel.EndDate = item.EndDate;


            item.ViewBatchDetailsModel = $scope.ViewBatchDetailsModel;

            var jsonData = angular.toJson({
                BatchId: item.BatchID
            });

            AngularAjaxCall($http, HomeCareSiteUrl.GetBatchDetailsURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.PatientClaimsList = response.Data;

                    item.PatientClaimsList = $scope.PatientClaimsList;
                    //$("#model__ViewBatch").modal("show");
                    ShowMessages(response);
                }
                else {
                    ShowMessages(response);
                }
            });

        }
        else {
            $scope.ViewBatchDetailsModel = item.ViewBatchDetailsModel;
            $scope.PatientClaimsList = item.PatientClaimsList;
        }
    };




    $scope.ViewBatchDetails = function (item) {

        if (item.ViewBatchDetailsModel == undefined) {
            var jsonData = angular.toJson({
                BatchId: item.BatchID
            });
            $scope.ViewBatchDetailsModel.BatchID = item.BatchID;
            $scope.ViewBatchDetailsModel.IsBatchSent = item.IsSent;
            $scope.ViewBatchDetailsModel.PayorName = item.PayorName;
            $scope.ViewBatchDetailsModel.StartDate = item.StartDate;
            $scope.ViewBatchDetailsModel.EndDate = item.EndDate;

            item.ViewBatchDetailsModel = $scope.ViewBatchDetailsModel;


            AngularAjaxCall($http, HomeCareSiteUrl.GetBatchDetailsURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.PatientClaimsList = response.Data;
                    item.PatientClaimsList = $scope.PatientClaimsList;
                    $("#model__ViewBatch").modal("show");
                    //$("#model__ViewBatch-"+item.BatchID).modal("show");
                    ShowMessages(response);
                }
                else {
                    ShowMessages(response);
                }
            });
        }
        else {
            $("#model__ViewBatch").modal("show");
            $scope.ViewBatchDetailsModel = item.ViewBatchDetailsModel;
            $scope.PatientClaimsList = item.PatientClaimsList;
        }
    };

    $scope.CheckClaimCount = function (gathered, rolledUp) {
        if (gathered === 0 || rolledUp === 0) {
            ShowMessage("No claim(s) available for this operation on the selected batch, kindly regenerate batch with correct data.", "error");
        }
    };

    $('#model__ViewBatch').on('hidden.bs.modal', function () {
        $scope.PatientClaimsList = [];
    });

    //#endregion

    //Bind Batch List 
    //#region 
    $scope.BatchList = [];
    $scope.SelectedBatchIds = [];
    $scope.SelectAllBatchCheckbox = false;

    $scope.TempSearchBatchList = $scope.newInstance().SearchBatchList;
    $scope.BatchListPager = new PagerModule("BatchID", undefined, 'DESC');

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchBatchList: $scope.TempSearchBatchList,
            pageSize: $scope.BatchListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.BatchListPager.sortIndex,
            sortDirection: $scope.BatchListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.TempSearchBatchList = $.parseJSON(angular.toJson($scope.BatchModel.SearchBatchList));
    };

    $scope.GetBatchList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedBatchIds = [];
        $scope.SelectAllBatchCheckbox = false;
        $scope.BatchModel.SearchBatchList.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control
        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();

        $scope.AjaxStart = true;

        var jsonData = $scope.SetPostData($scope.BatchListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetBatchListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.BatchList = response.Data.Items;
                $scope.BatchListPager.currentPageSize = response.Data.Items.length;
                $scope.BatchListPager.totalRecords = response.Data.TotalItems;
            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.BatchListPager.currentPage = 1;
        $scope.BatchListPager.getDataCallback(true);
    };

    $scope.ResetBatchSearchFilter = function () {
        $scope.BatchModel.SearchBatchList = $scope.newInstance().SearchBatchList;
        $scope.BatchModel.SearchBatchList.BatchTypeID = null;
        $scope.BatchModel.SearchBatchList.PayorID = null;
        $scope.BatchModel.SearchBatchList.StartDate = null;
        $scope.BatchModel.SearchBatchList.EndDate = null;
        $scope.BatchModel.SearchBatchList.Comment = null;
        $scope.BatchModel.SearchBatchList.IsSentStatus = '-1';

        $scope.BatchListPager.currentPage = 1;
        $scope.BatchListPager.getDataCallback(true);
    };

    $scope.SearchBatch = function () {
        $scope.BatchListPager.currentPage = 1;
        $scope.BatchListPager.getDataCallback(true);
    };

    $scope.SelectBatch = function (batchList) {
        if (batchList.IsChecked)
            $scope.SelectedBatchIds.push(batchList.BatchID);
        else
            $scope.SelectedBatchIds.remove(batchList.BatchID);
        if ($scope.SelectedBatchIds.length == $scope.BatchListPager.currentPageSize)
            $scope.SelectAllBatchCheckbox = true;
        else
            $scope.SelectAllBatchCheckbox = false;
    };

    $scope.SelectAll = function () {
        $scope.SelectedBatchIds = [];
        angular.forEach($scope.BatchList, function (item, key) {
            item.IsChecked = $scope.SelectAllBatchCheckbox;
            if (item.IsChecked)
                $scope.SelectedBatchIds.push(item.BatchID);
        });
        return true;
    };

    $scope.BatchListPager.getDataCallback = $scope.GetBatchList;
    $scope.BatchListPager.getDataCallback();
    //#endregion

    // Batch Delete and Mark as sent 
    //#region 

    $scope.MarkasSentBatch = function (batchId, title) {
        if (title == 'Sent') {
            title = window.MarkAsSent;
            $scope.TempSearchBatchList.IsSent = true;
        } else {
            title = window.MarkAsUnSent;
            $scope.TempSearchBatchList.IsSent = false;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.TempSearchBatchList.ListOfIdsInCSV = batchId > 0 ? batchId.toString() : $scope.SelectedBatchIds.toString();
                if (batchId > 0) {
                    if ($scope.BatchListPager.currentPage != 1)
                        $scope.BatchListPager.currentPage = $scope.BatchList.length === 1 ? $scope.BatchList.currentPage - 1 : $scope.BatchList.currentPage;
                }
                else {
                    if ($scope.BatchListPager.currentPage != 1 && $scope.SelectedBatchIds.length == $scope.BatchListPager.currentPageSize)
                        $scope.BatchListPager.currentPage = $scope.BatchListPager.currentPage - 1;
                }
                //Reset Selcted Checkbox items and Control
                $scope.SelectedBatchIds = [];
                $scope.SelectAllBatchCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.BatchListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.MarkasSentBatchURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.BatchList = response.Data.Items;
                        $scope.BatchListPager.currentPageSize = response.Data.Items.length;
                        $scope.BatchListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.MarkAsSentConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnEnable);
    };

    $scope.DeleteBatch = function (batchId, batchIndex) {
        title = window.HardDelete;
        bootboxDialog(function (result) {
            if (result) {

                $scope.TempSearchBatchList.ListOfIdsInCSV = batchId > 0 ? batchId.toString() : $scope.SelectedBatchIds.toString();
                //    $scope.TempSearchBatchList.ListOfIdsInCSV = ',' + $scope.TempSearchBatchList.ListOfIdsInCSV + ',';

                if (batchId > 0) {
                    if ($scope.BatchListPager.currentPage != 1)
                        $scope.BatchListPager.currentPage = $scope.BatchList.length === 1 ? $scope.BatchList.currentPage - 1 : $scope.BatchList.currentPage;
                }
                else {
                    if ($scope.BatchListPager.currentPage != 1 && $scope.SelectedBatchIds.length == $scope.BatchListPager.currentPageSize)
                        $scope.BatchListPager.currentPage = $scope.BatchListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedBatchIds = [];
                $scope.SelectAllBatchCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.BatchListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteBatchURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {

                        $scope.BatchList.splice(batchIndex, 1);
                        $scope.BatchListPager.totalRecords = $scope.BatchListPager.totalRecords - 1;
                        $scope.BatchListPager.currentPageSize = $scope.BatchListPager.currentPageSize - 1;
                        //item.TotalClaims = item.TotalClaims - 1;

                        //$scope.BatchList = response.Data.Items;
                        //$scope.BatchListPager.currentPageSize = response.Data.Items.length;
                        //$scope.BatchListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    //#endregion

    // Save/Create Batch 
    //#region 

    $scope.CreateBatch = function () {

        $scope.BatchModel.Batch = $scope.TempSearchPatientList;
        $scope.BatchModel.Batch.Comment = $scope.BatchModel.SearchPatientList.Comment;

        if ($scope.SelectedPatientIds.length > 0)
            $scope.BatchModel.Batch.PatientIds = $scope.SelectedPatientIds.toString();

        var jsonData = angular.toJson({
            addBatchModel: { Batch: $scope.BatchModel.Batch }
        });
        AngularAjaxCall($http, HomeCareSiteUrl.AddBatchDetailURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ClaimValidationMsg = response.Message;
                $scope.ClaimValidationStatus = "1";
                ShowMessages(response);
                bootboxDialog(function (result, data) {
                    $scope.ResetPatientSearchFilter();
                    $scope.BatchListPager.getDataCallback();
                    $("#model__AddBatch").modal("hide");
                }, bootboxDialogType.Alert, "Message", response.Message);
            }
            else {

                if (response.ErrorCode != undefined) {
                    //debugger;
                    ShowMessages(response);
                    $scope.ClaimValidationMsg = response.Message;
                    $scope.ClaimValidationStatus = "0";
                    if (response.Data && response.Data.length > 0) {
                        $scope.ClientErrorList = response.Data;
                        $scope.SetClaimsError();
                    }
                }
            }
            
        });
    };



    $scope.SetClaimsError = function () {
        if ($scope.PatientList != undefined) {
            $.each($scope.PatientList, function (index, patient) {



                //#region Check on Patient Level Error
                patient.ErrorMessage = "";
                patient.ErrorCount = 0;

                if ($scope.ClientErrorList != undefined) {
                    $.each($scope.ClientErrorList, function (index, clientError) {
                        if (clientError.ReferralID == patient.ReferralID) {
                            patient.ErrorMessage = "Claim Validaiton failed for this client. Please check & correct.";
                            patient.ErrorCount = 1;
                        }
                    });
                }
                //#endregion 










                if (patient.ChildNoteList != undefined) {
                    $.each(patient.ChildNoteList, function (index, childNote) {

                        if ($scope.ClientErrorList != undefined) {
                            var errorFound = false;
                            childNote.ErrorMessage = "";
                            childNote.ErrorCount = 0;


                            $.each($scope.ClientErrorList, function (index, clientError) {

                                if (clientError.NoteID == childNote.NoteID) {
                                    childNote.ErrorMessage = clientError.ErrorMessage;
                                    childNote.ErrorCount = clientError.ErrorCount;
                                    errorFound = true;
                                }


                                //#region Check on Patient Level Error
                                if (clientError.ReferralID == patient.ReferralID) {
                                    patient.ErrorMessage = "Claim Validaiton failed for this client. Please check & correct.";
                                    patient.ErrorCount = 1;
                                    errorFound = true;
                                }


                                //#endregion 

                            });


                        }

                    });
                }
            });
        }

    }

    //#endregion

    // Run 837Service For Refresh And Grouping Notes 
    //#region
    $scope.Run837ServiceForRefreshAndGroupingNotes = function () {
        AngularAjaxCall($http, HomeCareSiteUrl.RefreshAndGroupingNotesServiceURL, null, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                ShowMessages(response);
            }
            else {
                ShowMessages(response);
            }
        });
    };
    //#endregion

    // Show CMS1500 and UB04 File 
    //#region
    $scope.EDIFileSearchModel = {};

    $scope.ShowCMS1500File = function (item) {
        $scope.EDIFileSearchModel.PayorID = item.PayorID;
        $scope.EDIFileSearchModel.ReferralID = item.ReferralID;
        $scope.EDIFileSearchModel.BatchID = item.BatchID == null ? 0 : item.BatchID;
        $scope.EDIFileSearchModel.FileType = 'CMS1500';

        $scope.EDIFileSearchModel.BatchTypeID = $scope.BatchModel.SearchPatientList.BatchTypeID;
        $scope.EDIFileSearchModel.StartDate = $scope.BatchModel.SearchPatientList.StartDate;
        $scope.EDIFileSearchModel.EndDate = $scope.BatchModel.SearchPatientList.EndDate;
        $scope.EDIFileSearchModel.ClientName = $scope.BatchModel.SearchPatientList.ClientName;

        if ($scope.BatchModel.SearchPatientList.ServiceCodeID)
            $scope.EDIFileSearchModel.ServiceCodeIDs = $scope.BatchModel.SearchPatientList.ServiceCodeID.toString();
        else
            $scope.EDIFileSearchModel.ServiceCodeIDs = '';

        var jsonData = angular.toJson(
            $scope.EDIFileSearchModel
        );

        AngularAjaxCall($http, HomeCareSiteUrl.GenerateCMS1500Url, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.Cms1500List = response.Data;
                $("#generateCMS1500").modal('show');
            }
            else {
                ShowMessages(response);
            }
        });
    };

    $scope.HideCMS1500File = function () {
        $("#generateCMS1500").modal('hide');
    }

    $scope.EmployeeVisitList = [];
    $scope.ShowEmployeeVisits = function (item) {
        var jsonData = $scope.SetEMPVisitsPostData(item);
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeVisitList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.EmployeeVisitList = response.Data.Items;
                $("#viewtimesheetdetails").modal('show');
            }
            else {
                ShowMessages(response);
            }
        });
    };

    $scope.HideEmployeeVisits = function () {
        $("#viewtimesheetdetails").modal('hide');
    };

    $scope.SetEMPVisitsPostData = function (item) {

        var pagermodel = {
            SearchEmployeeVisitListPage: { PayorIDs: item.PayorID, ReferralIDs: item.ReferralID, StartDate: item.StartDate, EndDate: item.EndDate },
            pageSize: 100,
            pageIndex: 1,
            sortIndex: '',
            sortDirection: ''
        };
        return angular.toJson(pagermodel);
    };

    $scope.ShowUB04File = function (item) {

        $scope.EDIFileSearchModel.PayorID = item.PayorID;
        $scope.EDIFileSearchModel.ReferralID = item.ReferralID;
        $scope.EDIFileSearchModel.BatchID = item.BatchID == null ? 0 : item.BatchID;
        $scope.EDIFileSearchModel.FileType = 'UB04';

        $scope.EDIFileSearchModel.BatchTypeID = $scope.BatchModel.SearchPatientList.BatchTypeID;
        $scope.EDIFileSearchModel.StartDate = $scope.BatchModel.SearchPatientList.StartDate;
        $scope.EDIFileSearchModel.EndDate = $scope.BatchModel.SearchPatientList.EndDate;
        $scope.EDIFileSearchModel.ClientName = $scope.BatchModel.SearchPatientList.ClientName;

        if ($scope.BatchModel.SearchPatientList.ServiceCodeID)
            $scope.EDIFileSearchModel.ServiceCodeIDs = $scope.BatchModel.SearchPatientList.ServiceCodeID.toString();
        else
            $scope.EDIFileSearchModel.ServiceCodeIDs = '';

        var jsonData = angular.toJson(
            $scope.EDIFileSearchModel
        );

        AngularAjaxCall($http, HomeCareSiteUrl.GenerateUB04Url, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.UB04List = response.Data;
                $("#generateUB04").modal('show');
            }
            else {
                ShowMessages(response);
            }
        });
    };

    $scope.HideUB04File = function () {
        $("#generateUB04").modal('hide');
    }
    //#endregion

    // EDI 837 Files Validation And Generation
    //#region 

    $scope.ValidateAndGenerateEdi837Model = {};

    $scope.FilterBatchForValidation = function (item) {
        var returnValue = false;
        if ($scope.ListOfIdsInCSV != undefined) {
            $.each($scope.ListOfIdsInCSV.split(','), function (index, data) {
                if (item.BatchID == data)
                    returnValue = true;
            });
        }
        return returnValue;
    };
    $scope.TempBatchIds = [];
    $scope.ValidateAndGenerateEdi837 = function (validateOnly, fileExtension, batchId, ediFileType, ediFileTypeName) {
        $scope.TempBatchIds = [];
        $scope.ListOfIdsInCSV = batchId > 0 ? batchId.toString() : $scope.SelectedBatchIds.toString();

        if (validateOnly) {
            $scope.ValidateAndGenerateEdi837Model.PageTitle = window.ValidateBatches;
        }
        else {
            var msg = window.ValidateAndGenerateEDI837;
            $scope.ValidateAndGenerateEdi837Model.PageTitle = msg.format(ediFileTypeName);
        }

        $scope.ValidateAndGenerateEdi837Model.ValidateOnly = validateOnly;
        // $scope.ValidateAndGenerateEdi837Model.PageTitle = validateOnly ? window.ValidateBatches : window.ValidateAndGenerateEDI837;
        $scope.ValidateAndGenerateEdi837Model.ValidateWaitText = window.ValidateWaitText;
        $scope.ValidateAndGenerateEdi837Model.FileExtension = fileExtension;
        $scope.ValidateAndGenerateEdi837Model.EdiFileType = ediFileType;
        $scope.ValidateAndGenerateEdi837Model.FilteredBatchList = $scope.BatchList.filter(function (item) {
            if (ediFileType == 1) {
                if (item.PayorBillingType == 'Professional') {
                    if ($scope.FilterBatchForValidation(item)) {
                        $scope.TempBatchIds.push(item.BatchID);
                        return true;
                    }
                    return false;
                }
            }
            else {
                if (item.PayorBillingType == 'Institutional') {
                    if ($scope.FilterBatchForValidation(item)) {
                        $scope.TempBatchIds.push(item.BatchID);
                        return true;
                    }
                    return false;
                }
            }
            // return $scope.FilterBatchForValidation(item);
        });
        $("#model__ValidateAndGenerateEdi837").modal("show");
    };

    $scope.SubmitClaim = function (ele) {
        $(ele).button('loading');
        if ($scope.ValidateAndGenerateEdi837Model.FilteredBatchList != null && $scope.ValidateAndGenerateEdi837Model.FilteredBatchList != undefined && $scope.ValidateAndGenerateEdi837Model.FilteredBatchList.length > 0) {
            angular.forEach($scope.ValidateAndGenerateEdi837Model.FilteredBatchList, function (item, key) {
                if (item.ValidationPassed == true && item.Edi837GenerationPassed == true) {
                    var url = "/hc/batch/SubmitClaim/";
                    var jsonData = angular.toJson({
                        claimModel: {
                            BatchID: item.BatchID,
                            FileName: item.FileName,
                            EdiFileTypeID: item.EdiFileTypeID,
                            Edi837GenerationPassed: item.Edi837GenerationPassed,
                            Edi837FilePath: item.Edi837FilePath,
                            ValidationPassed: item.ValidationPassed,
                            ValidationErrorFilePath: item.ValidationErrorFilePath,
                        }
                    });
                    AngularAjaxCall($http, url, jsonData, "Post", "json", "application/json", false).success(function (response) {
                        $(ele).button('reset');
                        ShowMessages(response);
                        $scope.HideSubmitClaimButton();
                        $("#model__ValidateAndGenerateEdi837").modal("hide");
                    });
                }
            });
        }
        else {
            $(ele).button('reset');
            $scope.HideSubmitClaimButton();
        }
    }

    $scope.DoEdi837Action = function (ele) {
        $scope.ListOfIdsInCSV = $scope.TempBatchIds.length > 0 ? $scope.TempBatchIds.toString() : '';
        $scope.ValidateAndGenerateEdi837Model.ShowLoader = true;
        $(ele).button('loading');

        var url = $scope.ValidateAndGenerateEdi837Model.ValidateOnly ? HomeCareSiteUrl.ValidateBatchesURL : HomeCareSiteUrl.GenerateEdi837FilesURL;

        var jsonData = angular.toJson({
            postEdiValidateGenerateModel: {
                ListOfBacthIdsInCsv: $scope.ListOfIdsInCSV,
                FileExtension: $scope.ValidateAndGenerateEdi837Model.FileExtension,
                GenerateEdiFile: !$scope.ValidateAndGenerateEdi837Model.ValidateOnly,
                EdiFileType: $scope.ValidateAndGenerateEdi837Model.EdiFileType
            }
        });

        AngularAjaxCall($http, url, jsonData, "Post", "json", "application/json", false).success(function (response) {
            $(ele).button('reset');
            $scope.ValidateAndGenerateEdi837Model.ShowLoader = false;
            if (response.IsSuccess) {
                if (response.Data) {
                    // $scope.$apply(function() {
                    $scope.ValidateAndGenerateEdi837Model.FilteredBatchList.filter(function (item) {
                        response.Data.filter(function (returnItem) {
                            if (item.BatchID == returnItem.BatchID) {
                                item.ShowResult = true;
                                item.FileName = returnItem.FileName;
                                item.ValidationPassed = returnItem.ValidationPassed;
                                item.ValidationErrorFilePath = returnItem.ValidationErrorFilePath;
                                item.Edi837GenerationPassed = returnItem.Edi837GenerationPassed;
                                item.Edi837FilePath = returnItem.Edi837FilePath;

                            }
                        });
                    });

                    angular.forEach($scope.ValidateAndGenerateEdi837Model.FilteredBatchList, function (item, key) {
                        if (item.ValidationPassed == true && item.Edi837GenerationPassed == true) {
                            $scope.ShowSubmitClaimButton();
                        }
                    });

                    $scope.$$phase || $scope.$apply();
                    //});
                }
            }
            else {
                ShowMessages(response);
            }
        });

    };

    $('#model__ValidateAndGenerateEdi837').on('hidden.bs.modal', function () {
        $scope.ValidateAndGenerateEdi837Model.FilteredBatchList.filter(function (item) {
            item.ShowResult = false;
            item.FileName = null;
            item.ValidationPassed = null;
            item.ValidationErrorFilePath = null;
            item.Edi837GenerationPassed = null;
            item.Edi837FilePath = null;

        });
    });

    //#endregion    

    // PaperRemitsEOBTemplate
    //#region 

    $scope.PaperRemitsEOBTemplate = function (batchId) {
        $scope.TempSearchBatchList.ListOfIdsInCSV = batchId > 0 ? batchId.toString() : $scope.SelectedBatchIds.toString();

        var jsonData = { batchid: $scope.TempSearchBatchList.ListOfIdsInCSV };
        AngularAjaxCall($http, HomeCareSiteUrl.GenratePaperRemitsEOBTemplateURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                if (response.Data.AbsolutePath == "true") {
                    window.location = '/hc/batch/DownloadFiles?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                } else {
                    window.location = '/hc/batch/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                }
            } ShowMessages(response);
        });
    };




    //#endregion    

    //Download Overview File
    //#region  

    $scope.DownloadOverViewFile = function (batchId) {
        $scope.TempSearchBatchList.ListOfIdsInCSV = batchId > 0 ? batchId.toString() : $scope.SelectedBatchIds.toString();

        //Reset Selcted Checkbox items and Control
        $scope.SelectedBatchIds = [];
        $scope.SelectAllBatchCheckbox = false;
        //Reset Selcted Checkbox items and Control

        var jsonData = { batchid: $scope.TempSearchBatchList.ListOfIdsInCSV };
        AngularAjaxCall($http, HomeCareSiteUrl.GenrateOverViewFileURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                if (response.Data.AbsolutePath == "true") {
                    window.location = '/hc/batch/DownloadFiles?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                } else {
                    window.location = '/hc/batch/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                }
                $scope.BatchModel.Batch.IsSentStatus = '-1';
                $scope.BatchListPager.currentPage = 1;
                $scope.BatchListPager.getDataCallback(true);
            } ShowMessages(response);
        });
    };

    //#endregion

    $scope.DatePickerDate = function (modelDate) {
        var a;
        if (modelDate) {
            if (modelDate == "0001-01-01T00:00:00Z") {
                $scope.maxDate = new Date();
                $scope.maxDate.setDate($scope.maxDate.getDate() + 1);
                $scope.NewDate = SetExpiryDate();
                a = $scope.NewDate;
            } else {
                var dt = new Date(modelDate);
                a = dt;
            }
        }
        else {
            $scope.maxDate = new Date();
            $scope.maxDate.setDate($scope.maxDate.getDate() + 1);
            $scope.NewDate = SetExpiryDate();
            a = $scope.NewDate;
        }
        return moment(a).format('L');
    };

    $scope.HideSubmitClaimButton = function () {
        $("#btnSubmitClaim").hide();
        $("#btnConfirm").show();
    }
    $scope.ShowSubmitClaimButton = function () {
        $("#btnConfirm").hide();
        $("#btnSubmitClaim").show();
    }







    //#region Handle Temporary Notes


    $scope.DeleteTemproryNote = function (noteId, item, index) {

        bootboxDialog(function (result) {
            if (result) {

                var PermanentDelete = $(result.currentTarget).hasClass('actionTrue');
                var jsonData = { "NoteID": noteId, "PermanentDelete": PermanentDelete };
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteNote_Temporary, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        debugger;
                        item.ChildNoteList.splice(index, 1);
                        item.TotalClaims = item.TotalClaims - 1;
                    }
                    ShowMessages(response);
                });
            }
            //}, bootboxDialogType.Confirm, title, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
        }, bootboxDialogType.TwoActionOnly, window.Confirm, window.DeleteClaimConfirmationMessage, "Exclude", 'btn btn-primary', '', '',
            "Permanent Delete", 'btn btn-danger actionTrue-E2  display-none');
    };

    //#endregion




};
controllers.BatchController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    $(".dateInputMask").attr("placeholder", "mm/dd/yyyy");

    $('#model__ValidateAndGenerateEdi837').on('hidden.bs.modal', function () {
        $("#btnConfirm").show();
        $("#btnSubmitClaim").hide();
    });
});