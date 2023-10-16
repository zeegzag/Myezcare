
var custModel;

controllers.BatchController = function ($scope, $http) {
    custModel = $scope;
    //$scope.tetbxShow = false;
    var modalJson = $.parseJSON($("#hdnBatchModel").val());
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnBatchModel").val());
    };
    $scope.BatchModel = modalJson;
    //$scope.BillingNote = modalJson;


    $scope.BatchViewHtml = $scope.BatchModel.SearchBatchList.BatchView;

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
    $scope.SelectedTotalClaimAmount = 0;
    $scope.SelectedTotalLineItems = 0;
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

                    $scope.SelectedPatientIds = [];
                    $scope.SelectedTotalClaimAmount = 0;
                    $scope.SelectedTotalLineItems = 0;
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
        $scope.BatchModel.SearchPatientList.CreatePatientWiseBatch = false;
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

        if (patientList.IsChecked) {
            $scope.SelectedPatientIds.push(patientList.ReferralID);
            $scope.SelectedTotalClaimAmount = $scope.SelectedTotalClaimAmount + patientList.TotalAmount;
            $scope.SelectedTotalLineItems = $scope.SelectedTotalLineItems + patientList.TotalClaims;
        }
        else {
            $scope.SelectedPatientIds.remove(patientList.ReferralID);
            $scope.SelectedTotalClaimAmount = $scope.SelectedTotalClaimAmount - patientList.TotalAmount;
            $scope.SelectedTotalLineItems = $scope.SelectedTotalLineItems - patientList.TotalClaims;
        }

        if ($scope.SelectedPatientIds.length == $scope.PatientList.length)
            $scope.SelectAllPatientsCheckbox = true;
        else
            $scope.SelectAllPatientsCheckbox = false;
    };

    $scope.SelectAllPatient = function () {
        $scope.SelectedPatientIds = [];
        $scope.SelectedTotalClaimAmount = 0;
        $scope.SelectedTotalLineItems = 0;
        angular.forEach($scope.PatientList, function (item, key) {
            item.IsChecked = $scope.SelectAllPatientsCheckbox;
            if (item.IsChecked) {
                $scope.SelectedPatientIds.push(item.ReferralID);

                $scope.SelectedTotalClaimAmount = $scope.SelectedTotalClaimAmount + item.TotalAmount;
                $scope.SelectedTotalLineItems = $scope.SelectedTotalLineItems + item.TotalClaims;
            }

        });
        return true;
    };
    //#endregion

    // get All Details of Claim(Notes) (Child Note Details)
    //#region
    $scope.GetChildNoteDetailsOfClaim = function (item, claimItem, parentItem, forceLoad, forceLoadItemObj) {
        if (item.ChildNoteList == undefined || forceLoad == true) {
            $scope.TempSearchPatientList.ReferralID = item.ReferralID;
            $scope.TempSearchPatientList.BatchID = parentItem == undefined ? 0 : parentItem.BatchID;
            $scope.TempSearchPatientList.NoteIDs = claimItem.StrNoteIds;

            var jsonData = angular.toJson($scope.TempSearchPatientList);
            AngularAjaxCall($http, HomeCareSiteUrl.GetChildNoteDetailsURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {

                    if (forceLoad) {


                        $.each(item.ChildNoteList, function (index, tempItem) {

                            var claim = undefined;
                            if (forceLoadItemObj != undefined) {
                                claim = response.Data.filter(function (data) { return data.NoteID == forceLoadItemObj.NoteID })[0];
                            }

                            if (claim != undefined && claim.NoteID > 0 && tempItem.NoteID == claim.NoteID) {


                                tempItem.CalculatedAmount = claim.CalculatedAmount;
                                tempItem.ClaimAllowedAmount = claim.ClaimAllowedAmount;
                                tempItem.ClaimBilledAmount = claim.ClaimBilledAmount;
                                tempItem.ClaimPaidAmount = claim.ClaimPaidAmount;

                                tempItem.ClaimAdjustmentReason = claim.ClaimAdjustmentReason;
                                tempItem.ClaimAdjustmentTypeID = claim.ClaimAdjustmentTypeID;

                                tempItem.MPP_AdjustmentGroupCodeID = claim.MPP_AdjustmentGroupCodeID;
                                tempItem.MPP_AdjustmentGroupCodeName = claim.MPP_AdjustmentGroupCodeName;
                                tempItem.MPP_AdjustmentAmount = claim.MPP_AdjustmentAmount;
                                tempItem.MPP_AdjustmentComment = claim.MPP_AdjustmentComment;

                                //$scope.$$phase || $scope.$apply();
                                return true;    //break out of the loop
                            }
                            else if (claim == undefined) {
                                $.each(response.Data, function (index, tempServerItem) {
                                    if (tempItem.BatchNoteID == tempServerItem.BatchNoteID) {
                                        tempItem.CalculatedAmount = tempServerItem.CalculatedAmount;
                                        tempItem.ClaimAllowedAmount = tempServerItem.ClaimAllowedAmount;
                                        tempItem.ClaimBilledAmount = tempServerItem.ClaimBilledAmount;
                                        tempItem.ClaimPaidAmount = tempServerItem.ClaimPaidAmount;

                                        tempItem.ClaimAdjustmentReason = tempServerItem.ClaimAdjustmentReason;
                                        tempItem.ClaimAdjustmentTypeID = tempServerItem.ClaimAdjustmentTypeID;

                                        tempItem.MPP_AdjustmentGroupCodeID = tempServerItem.MPP_AdjustmentGroupCodeID;
                                        tempItem.MPP_AdjustmentGroupCodeName = tempServerItem.MPP_AdjustmentGroupCodeName;
                                        tempItem.MPP_AdjustmentAmount = tempServerItem.MPP_AdjustmentAmount;
                                        tempItem.MPP_AdjustmentComment = tempServerItem.MPP_AdjustmentComment;
                                    }

                                });
                            }



                        });
                        if (!$scope.$root.$$phase) $scope.$apply();


                        if (forceLoadItemObj != undefined)
                            $scope.GetBatchNoteDetails(forceLoadItemObj.cnItem, forceLoad);


                    }
                    else {

                        item.ChildNoteList = response.Data;

                        $scope.ShowCollpase();
                        $scope.SetClaimsError();
                    }
                }
                ShowMessages(response);
                if (!$scope.$root.$$phase) $scope.$apply();
            });
        }

        //if ($(trElem).hasClass("fa-minus-circle") == false) {
        //    $(trElem).removeClass("fa-plus-circle").addClass("fa-minus-circle");
        //}
        //else {
        //    $(trElem).removeClass("fa-minus-circle").addClass("fa-plus-circle");
        //}
    };





    $scope.SyncClaimMessages = function () {
        var jsonData = {};
        $scope.ClaimMessagesList = [];
        AngularAjaxCall($http, HomeCareSiteUrl.SyncClaimMessagesURL, jsonData, "Post", "json", "application/json").success(function (response) {

        });
    };

    //$scope.SyncClaimMessages();


    $scope.GetClaimMessagesList = function (item) {

        $scope.ClaimMessagesList = [];
        $("#_ViewClaimMDMessages").modal('show');
        $scope.ClaimMessagesList = item.ClaimMessages;


        /*
        var jsonData = angular.toJson(
            { model: item }
        );
        $scope.ClaimMessagesList = [];
        AngularAjaxCall($http, HomeCareSiteUrl.GetClaimMessageListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ClaimMessagesList = response.Data;
                if ($scope.ClaimMessagesList != null)
                    $("#_ViewClaimMDMessages").modal('show');
                ShowMessages(response);
            }
        });

        */
    };





    $scope.HideClaimMDMessages = function () {
        $("#_ViewClaimMDMessages").modal('hide');
    };



    $scope.HideMannualPaymentPosting = function () {
        $("#_MannualPaymentPosting").modal('hide');
    };


    $scope.SetMannualPaymentPosting = function (item, parentClaimEleRefreshId) {

        $scope.MannualPaymentPosting = JSON.parse(JSON.stringify(item));
        $scope.MannualPaymentPosting.ParentClaimEleRefreshId = parentClaimEleRefreshId;

        var isUpdateMode = ValideElement(item.MPP_AdjustmentGroupCodeID);
        $scope.MannualPaymentPosting.MPP_AdjustmentGroupCodeID = isUpdateMode ? item.MPP_AdjustmentGroupCodeID : 'NA';
        $scope.MannualPaymentPosting.MPP_AdjustmentAmount = isUpdateMode ? (item.MPP_AdjustmentAmount).toFixed(2) : (item.ClaimBilledAmount - item.ClaimPaidAmount).toFixed(2);
        $scope.MannualPaymentPosting.MPP_AdjustmentGroupCodeName_Visible = item.MPP_AdjustmentGroupCodeID == 'OA' ? true : false;
        $("#_MannualPaymentPosting").modal('show');

        //OK

    };

    $scope.CheckValidationForMannualPaymentPostingDetails = function (item) {

        var liString = "<li>{0} : {1}</li>";
        var isValid = true;
        $scope.ListofFields = "<ul>";
       
        if (!ValideElement(item.MPP_AdjustmentGroupCodeID) || item.MPP_AdjustmentGroupCodeID =='NA') {
            $scope.ListofFields += liString.format(window.Missing, window.AdjustmentType);
            isValid = false;
        }
        if (!ValideElement(item.MPP_AdjustmentGroupCodeName) && item.MPP_AdjustmentGroupCodeID == 'OA') {
            $scope.ListofFields += liString.format(window.Missing, window.OtherAdjustmentType);
            isValid = false;
        }

        if (!ValideElement(item.MPP_AdjustmentAmount) || item.MPP_AdjustmentAmount <= 0) {
            $scope.ListofFields += liString.format(window.Missing, window.AdjustmentAmount);
            isValid = false;
        }
        
        return isValid;
    };
    $scope.SaveMannualPaymentPostingDetails = function (item) {
        $scope.BatchID = item.BatchID;
        var jsonData = angular.toJson({ model: item });

        var isValid = $scope.CheckValidationForMannualPaymentPostingDetails(item);
        if (!isValid) {
            bootboxDialog(function () {
            }, bootboxDialogType.Alert, window.Alert, window.FieldsIncomplete.format($scope.ListofFields));
        }
        else {

            if (CheckErrors("#frm_MannualPaymentPosting")) {
                AngularAjaxCall($http, HomeCareSiteUrl.SaveMannualPaymentPostingDetailsUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    if (response.IsSuccess) {
                        ShowMessages(response);
                    }

                    $scope.HideMannualPaymentPosting();
                    setTimeout(function () {
                        if (item.ParentClaimEleRefreshId)
                            $("#" + item.ParentClaimEleRefreshId).click();
                    });
                });
            }
        }
    };


    $scope.$watch('MannualPaymentPosting.MPP_AdjustmentGroupCodeID', function (newValue, oldValue) {

        if ($scope.MannualPaymentPosting && ValideElement(oldValue)) {



            $.each($scope.BatchModel.MPPAdjustmentTypes, function (index, item) {
                if (item.Value == newValue && newValue != 'OA') {
                    $scope.MannualPaymentPosting.MPP_AdjustmentGroupCodeName = item.Name;
                }
            });


            $scope.MannualPaymentPosting.MPP_AdjustmentGroupCodeName_Visible = false;
            if (newValue !== null && newValue == 'OA') {
                $scope.MannualPaymentPosting.MPP_AdjustmentGroupCodeName_Visible = true;
                $scope.MannualPaymentPosting.MPP_AdjustmentGroupCodeName = '';
            }


            if (!$scope.$$phase) {
                $scope.$apply();
            }
        }
    });


    $scope.ShowCollpase = function () {
        //setTimeout(function () {
        //    $.each($('.collapseDestination'), function (index, data) {
        //        var parent = data;
        //        //$(parent).parent("tbody").find(".collapseSource").removeClass("fa-plus-circle").addClass("fa-minus-circle");

        //        $(parent).on('show.bs.collapse', function (e) {
        //            if ($(this).is(e.target))
        //                $(parent).parent("tbody").find(".collapseSource").removeClass("fa-plus-circle").addClass("fa-minus-circle");
        //        });
        //        $(parent).on('hidden.bs.collapse', function (e) {
        //            if ($(this).is(e.target))
        //                $(parent).parent("tbody").find(".collapseSource").removeClass("fa-minus-circle").addClass("fa-plus-circle");
        //        });
        //    });
        //}, 100);
    };

    //#endregion

    // For View Batch Details in Edit Mode/ View Details Mode
    //#region
    $scope.PatientClaimsList = [];
    $scope.EDI837Model = [];
    $scope.ViewBatchDetailsModel = {};


    $(document).on("click", ".collapseSource", function () {

        var hasClassFaMinusCircle = $(this).hasClass("fa-minus-circle");
        if (hasClassFaMinusCircle == false) {
            $(this).removeClass("fa-plus-circle").addClass("fa-minus-circle");
        }
        else {
            $(this).removeClass("fa-minus-circle").addClass("fa-plus-circle");
        }

    });

    $scope.OnPage_GetBacthClaimDetails = function (item, elem, forceLoad, forceLoadItemObj) {

        var hasClassFaMinusCircle = $(elem).hasClass("fa-minus-circle");

        //Enable this to keep only single expand / collapse
        //$("[id!=OnPage_ChildNoteDetails-" + item.BatchID + "][id^=OnPage_ChildNoteDetails]").removeClass("in");
        //$("[id^=OnPage_CNDetails]").removeClass("fa-minus-circle").addClass("fa-plus-circle");

        if (hasClassFaMinusCircle == false || forceLoad) {
            //$(elem).removeClass("fa-plus-circle").addClass("fa-minus-circle");
            $scope.OnPage_ViewBatchDetails(item, forceLoad, forceLoadItemObj);
        }
        else {
            //$(elem).removeClass("fa-minus-circle").addClass("fa-plus-circle");
        }




    };
    $scope.BatchID = {};
    $scope.ViewBillingNote = function (item) {
        $scope.BatchID = item.BatchID;
        $('#model_AllBillingNotes').modal({
            backdrop: 'static',
            keyboard: false
        });
    }

    $('#model_AddBillingNote').on('hidden.bs.modal', function () {
        $scope.PatientClaimsList = [];
        $scope.EDI837Model = [];
    });

    $scope.HideAddBillingNoteModal = function () {
        $("#model_AddBillingNote").modal('hide');
    }

    $('#model_AllBillingNotes').on('hidden.bs.modal', function () {
        $scope.PatientClaimsList = [];
        $scope.EDI837Model = [];
    });

    $scope.HideAllBillingNoteModal = function (item) {
        var jsonData = angular.toJson({ BatchID: item.BatchID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetBillingNotesUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                if (response.Data.length > 0) {
                    $scope.GetBillingNoteList = response.Data;
                }
            }
        });
        $scope.BNoteID = null;
    }

    //Save Data



    $scope.OnPage_ViewBatchDetails = function (item, forceLoad, forceLoadItemObj) {

        if (item.ViewBatchDetailsModel == undefined || forceLoad) {
            item.ViewBatchDetailsModel = {};
            item.ViewBatchDetailsModel.BatchID = item.BatchID;
            item.ViewBatchDetailsModel.IsBatchSent = item.IsSent;
            item.ViewBatchDetailsModel.PayorName = item.PayorName;
            item.ViewBatchDetailsModel.StartDate = item.StartDate;
            item.ViewBatchDetailsModel.EndDate = item.EndDate;

            var jsonData = angular.toJson({
                BatchId: item.BatchID
            });

            item.ViewBatchDetailsAjaxStart = true;
            AngularAjaxCall($http, HomeCareSiteUrl.GetBatchDetailsURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                item.ViewBatchDetailsAjaxStart = false;
                if (response.IsSuccess) {

                    if (forceLoad && 1 == 2) {

                        var PatientClaim = response.Data.filter(function (data) { return data.ReferralID == forceLoadItemObj.ReferralID })[0];
                        $.each(item.PatientClaimsList, function (index, tempItem) {

                            if (tempItem.ReferralID == PatientClaim.ReferralID) {
                                tempItem.TotalAllowedAmount = PatientClaim.TotalAllowedAmount;
                                tempItem.TotalAmount = PatientClaim.TotalAmount;
                                tempItem.TotalBilledAmount = PatientClaim.TotalBilledAmount;
                                tempItem.TotalClaims = PatientClaim.TotalClaims;
                                tempItem.TotalPaidAmount = PatientClaim.TotalPaidAmount;
                                $scope.$$phase || $scope.$apply();
                                return true;    //breaks out of he loop
                            }


                        });

                        $scope.GetChildNoteDetailsOfClaim(forceLoadItemObj.itemPT, '#CNDetails-' + forceLoadItemObj.item.BatchID + forceLoadItemObj.itemPT.ReferralID, forceLoadItemObj.item, forceLoad, forceLoadItemObj)



                    }
                    else {
                        //item.PatientClaimsList = response.Data;

                        item.EDI837Model = response.Data;

                        ShowMessages(response);
                    }
                }
                else {
                    ShowMessages(response);
                }
            });

        }
        else {
            $scope.ViewBatchDetailsModel = item.ViewBatchDetailsModel;
            $scope.PatientClaimsList = item.EDI837Model;
            $scope.EDI837Model = item.EDI837Model;
        }
    };










    $scope.ViewBatchDetails = function (item) {
        var jsonData = angular.toJson({
            BatchId: item.BatchID
        });
        $scope.ViewBatchDetailsModel.BatchID = item.BatchID;
        $scope.ViewBatchDetailsModel.IsBatchSent = item.IsSent;
        $scope.ViewBatchDetailsModel.PayorName = item.PayorName;
        $scope.ViewBatchDetailsModel.StartDate = item.StartDate;
        $scope.ViewBatchDetailsModel.EndDate = item.EndDate;

        AngularAjaxCall($http, HomeCareSiteUrl.GetBatchDetailsURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.PatientClaimsList = response.Data;
                $scope.PatientClaimsList.Model = { BatchID: item.BatchID }

                $scope.EDI837Model = response.Data;
                $scope.EDI837Model.Model = { BatchID: item.BatchID }

                $("#model__ViewBatch").modal("show");
                ShowMessages(response);
            }
            else {
                ShowMessages(response);
            }
        });
    };

    $scope.CheckClaimCount = function (gathered, rolledUp) {
        if (gathered === 0 || rolledUp === 0) {
            ShowMessage("No claim(s) available for this operation on the selected batch, kindly regenerate batch with correct data.", "error");
        }
    };

    $('#model__ViewBatch').on('hidden.bs.modal', function () {
        $scope.PatientClaimsList = [];
        $scope.EDI837Model = [];

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
        $scope.BatchModel.Batch.CreatePatientWiseBatch = $scope.BatchModel.SearchPatientList.CreatePatientWiseBatch;

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

    $scope.GenerateDataValidationBatch = function () {
        //$(ele).button('loading');

        if ($scope.BatchModel.ApprovedFacilityList.BillingProviderID)
            $scope.BatchModel.Batch.BillingProviderIDs = $scope.BatchModel.ApprovedFacilityList.BillingProviderID.toString();

        if ($scope.BatchModel.ServiceCodeList.ServiceCodeID)
            $scope.BatchModel.Batch.ServiceCodeIDs = $scope.BatchModel.ServiceCodeList.ServiceCodeID.toString();

        //if ($scope.DataValidationSelectedBatchIds)
        //    $scope.BatchModel.Batch.DataValidationSelectedBatchIds = $scope.DataValidationSelectedBatchIds.toString();


        var jsonData = { model: $scope.BatchModel.Batch };
        AngularAjaxCall($http, SiteUrl.GenerateDataValidationBatchUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {
            //$(ele).button('reset');
            //$scope.ValidateAndGenerateEdi837Model.ShowLoader = false;
            if (response.IsSuccess) {
                $scope.ResetSearchFilter();
            }
            ShowMessages(response);
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

        if (item.Claim != undefined)
            $scope.EDIFileSearchModel.StrNoteIds = item.Claim.StrNoteIds;

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
    $scope.ShowEmployeeVisits = function (itemPT, item) {

        itemPT.StartDate = item.ServiceStartDate;
        itemPT.EndDate = item.ServiceEndDate;

        var jsonData = $scope.SetEMPVisitsPostData(itemPT);
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

                        $scope.BatchListPager.getDataCallback();
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
                        item.ChildNoteList.splice(index, 1);
                        item.TotalClaims = item.TotalClaims - 1;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.TwoActionOnly, window.Confirm, window.DeleteClaimConfirmationMessage, "Exclude", 'btn btn-primary', '', '', "Permanent Delete", 'btn btn-danger actionTrue  display-none');
    };

    //#endregion


    //#region Reconcilaton


    $scope.GetBatchNoteDetails = function (item, forceLoad) {
        if (item.AHList == undefined || forceLoad == true) {
            var jsonData = angular.toJson({ NoteID: item.NoteID, BatchID: item.BatchID });
            AngularAjaxCall($http, HomeCareSiteUrl.GetChildReconcileListUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {


                    item.AHList = response.Data.AdjudicationDetailsModelList;
                    item.BHList = response.Data.BatchHistoryModelList;
                    item.NHModel = response.Data.NoteHistoryModel;

                }
                ShowMessages(response);
                if (!$scope.$root.$$phase) $scope.$apply();
            });
        }
    };


    $scope.MarkNoteAsLatest = function (data, cnItem, itemPT, item) {
        var jsonData = angular.toJson({ BatchNoteID: data.BatchNoteID, BatchID: data.BatchID, NoteID: data.NoteID });
        AngularAjaxCall($http, HomeCareSiteUrl.MarkNoteAsLatest01Url, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {

                var itemObj = {
                    ReferralID: itemPT.ReferralID,
                    NoteID: cnItem.NoteID,
                    BatchNoteID: data.BatchNoteID,


                    data: data,
                    cnItem: cnItem,
                    itemPT: itemPT,
                    item: item
                }

                $scope.OnPage_GetBacthClaimDetails(item, '#OnPage_CNDetails-' + item.BatchID, true, itemObj)

            }
            ShowMessages(response);

            if (!$scope.$root.$$phase) {
                $scope.$apply();
            }
            //$scope.Refresh();
        });
    };

    //#endregion

    $scope.GetERAPDF = function (era_id) {
        $scope.AjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetLatestERAPDF, { eraId: era_id }, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.ShowCollpase();
                var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,width=' + window.outerWidth / 2 + ',height=' + window.outerHeight / 1.12 + ', overflow= hidden';
                // let pdfWindow = window.open('about:blank', '', "directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no, width=absolute,height=absolute");
                let pdfWindow = window.open('about:blank', 'null', winFeature);
                pdfWindow.document.write("<html<head><title>" + "ERA_" + era_id + ".pdf" + "</title><style>body{margin: 0px;overflow:hidden}iframe{border-width: 0px;}</style></head>");
                pdfWindow.document.write("<body><embed width='100%' height='100%' src='data:application/pdf;base64, " + encodeURI(response.Data.result.data) + "#toolbar=0&navpanes=0&scrollbar=0'></embed></body></html>");
            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };



    //#region Claim econcilatin

    var data = null;
    $scope.SetClaimAdjustmentFlag = function (cnItem, itemPT, item, type) {


        $('#ClaimAdjustmentReason-modal').modal('show');
        $scope.SubmitClaimAdjustmentReasonModel = {};


        var msg =
            type === window.AdustmentType_Resend ? window.ResendConfirmationMessage :
                type === window.AdustmentType_Replacement ? window.ReplacementConfirmationMessage :
                    type === window.AdustmentType_Void ? window.VoidConfirmationMessage :
                        type === window.AdustmentType_WriteOff ? window.WriteOffConfirmationMessage :
                            type === window.AdustmentType_PayorChange ? window.PayorChangeConfirmationMessage :
                                type === window.AdustmentType_DataValidation ? window.DataValidationConfirmationMessage :
                                    type === window.AdustmentType_Other ? window.OtherConfirmationMessage :
                                        window.RemoveStatusMessage;


        //data = item;
        //$scope.SubmitClaimAdjustmentReasonModel.BulkAction = false;
        //$scope.SubmitClaimAdjustmentReasonModel.AdjustmentType = type;
        //$scope.SubmitClaimAdjustmentReasonModel.Message = msg;
        //$scope.SubmitClaimAdjustmentReasonModel.AdjustmentReason = item.ClaimAdjustmentReason;
        //$scope.SubmitClaimAdjustmentReasonModel.Item = cnItem;

        $scope.SubmitClaimAdjustmentReasonModel.Item = {};
        $scope.SubmitClaimAdjustmentReasonModel.Item.AdjustmentReason = "";
        $scope.SubmitClaimAdjustmentReasonModel.Item.AdjustmentType = type;
        $scope.SubmitClaimAdjustmentReasonModel.Item.Message = msg;


        if (item != null) $scope.SubmitClaimAdjustmentReasonModel.Item.BatchIDs = item.BatchID;
        if (itemPT != null) $scope.SubmitClaimAdjustmentReasonModel.Item.ReferralIDs = itemPT.ReferralID;
        if (cnItem != null) $scope.SubmitClaimAdjustmentReasonModel.Item.NoteIDs = cnItem.NoteID;



        $scope.SubmitClaimAdjustmentReasonModel.cnItem = cnItem;
        $scope.SubmitClaimAdjustmentReasonModel.itemPT = itemPT;
        $scope.SubmitClaimAdjustmentReasonModel.MainItem = item;

    };

    $scope.SetBulkClaimAdjustmentFlag = function (cnItem, itemPT, item, type, adjustmentAppliedOn) {


        $('#ClaimAdjustmentReason-modal').modal('show');
        $scope.SubmitClaimAdjustmentReasonModel = {};

        var msg =
            type === window.AdustmentType_Resend ? window.ResendConfirmationMessage :
                type === window.AdustmentType_Replacement ? window.ReplacementConfirmationMessage :
                    type === window.AdustmentType_Void ? window.VoidConfirmationMessage :
                        type === window.AdustmentType_WriteOff ? window.WriteOffConfirmationMessage :
                            type === window.AdustmentType_PayorChange ? window.PayorChangeConfirmationMessage :
                                type === window.AdustmentType_DataValidation ? window.DataValidationConfirmationMessage :
                                    type === window.AdustmentType_Other ? window.OtherConfirmationMessage :
                                        window.RemoveConfirmationMessage;


        //data = item;
        $scope.SubmitClaimAdjustmentReasonModel.Item = {};
        $scope.SubmitClaimAdjustmentReasonModel.Item.AdjustmentReason = "";
        $scope.SubmitClaimAdjustmentReasonModel.Item.AdjustmentType = type;
        $scope.SubmitClaimAdjustmentReasonModel.Item.Message = msg;



        if (adjustmentAppliedOn == window.ClaimAdjustmentLevel_Batch) {
            //$scope.SubmitClaimAdjustmentReasonModel.Item.BatchIDs = $scope.SetClaimAdjustmentFlag_BatchIDs.toString();
            $scope.SubmitClaimAdjustmentReasonModel.Item.BatchIDs = $scope.SelectedBatchIds.toString();

        }
        else if (adjustmentAppliedOn == window.ClaimAdjustmentLevel_Claim) {
            if (item != null) {
                $scope.SubmitClaimAdjustmentReasonModel.Item.BatchIDs = item.BatchID.toString();
                $scope.SubmitClaimAdjustmentReasonModel.Item.ReferralIDs = item.SetClaimAdjustmentFlag_ReferralIDs.toString();
                $scope.SubmitClaimAdjustmentReasonModel.Item.NoteIDs = item.SetClaimAdjustmentFlag_StrNotIDs.toString();
            }
        }
        else if (adjustmentAppliedOn == window.ClaimAdjustmentLevel_Line) {
            if (item != null) $scope.SubmitClaimAdjustmentReasonModel.Item.BatchIDs = item.BatchID.toString();
            if (itemPT != null) $scope.SubmitClaimAdjustmentReasonModel.Item.ReferralIDs = itemPT.ReferralID;
            $scope.SubmitClaimAdjustmentReasonModel.Item.NoteIDs = itemPT.SetClaimAdjustmentFlag_NoteIDs.toString();

        }


        if (itemPT != null) {
            $scope.SubmitClaimAdjustmentReasonModel.Item.ReferralIDs = itemPT.ReferralID;
        }
        if (cnItem != null) {

            $scope.SubmitClaimAdjustmentReasonModel.Item.NoteIDs = $scope.SetClaimAdjustmentFlag_NoteIDs.toString();
        }


        $scope.SubmitClaimAdjustmentReasonModel.cnItem = cnItem;
        $scope.SubmitClaimAdjustmentReasonModel.itemPT = itemPT;
        $scope.SubmitClaimAdjustmentReasonModel.MainItem = item;

    };


    $scope.SubmitClaimAdjustmentReason = function (cnItem, itemPT, item) {

        var jsonData = angular.toJson({
            model: $scope.SubmitClaimAdjustmentReasonModel.Item
        });


        AngularAjaxCall($http, HomeCareSiteUrl.BulkSetClaimAdjustmentFlagUrl, jsonData, "Post", "json", "application/json").success(function (response) {

            if (response.IsSuccess) {

                //if (itemPT)
                //    itemPT.SetClaimAdjustmentFlag_NoteIDs = [];


                //if (item)
                //    item.SetClaimAdjustmentFlag_ReferralIDs = [];

                $scope.SubmitClaimAdjustmentReasonModel = {};
            }
            ShowMessages(response);
            $('#ClaimAdjustmentReason-modal').modal('hide');
            //$scope.Refresh();



        });

    };

    $scope.SetClaimAdjustmentFlag_ReferralIDs = [];
    $scope.SetClaimAdjustmentFlag_StrNotIDs = [];
    $scope.SetClaimAdjustmentFlag_NoteIDs = [];

    $scope.SetClaimAdjustmentFlag_ClaimItemCheckBox = function (itemPT, mainItem) {

        if (mainItem.SetClaimAdjustmentFlag_ReferralIDs == undefined)
            mainItem.SetClaimAdjustmentFlag_ReferralIDs = [];

        if (mainItem.SetClaimAdjustmentFlag_StrNotIDs == undefined)
            mainItem.SetClaimAdjustmentFlag_StrNotIDs = [];

        if (itemPT.IsChecked) {

            if (mainItem.SetClaimAdjustmentFlag_ReferralIDs.indexOf(itemPT.ReferralID) == -1)
                mainItem.SetClaimAdjustmentFlag_ReferralIDs.push(itemPT.ReferralID);

            mainItem.SetClaimAdjustmentFlag_StrNotIDs.push(itemPT.Claim.StrNoteIds);
        }
        else {

            if (mainItem.SetClaimAdjustmentFlag_ReferralIDs.indexOf(itemPT.ReferralID) != -1)
                mainItem.SetClaimAdjustmentFlag_ReferralIDs.remove(itemPT.ReferralID);


            mainItem.SetClaimAdjustmentFlag_StrNotIDs.remove(itemPT.Claim.StrNoteIds);
        }
    };



    $scope.SetClaimAdjustmentFlag_LineItemCheckBox = function (cnItem, itemPT) {

        if (itemPT.SetClaimAdjustmentFlag_NoteIDs == undefined)
            itemPT.SetClaimAdjustmentFlag_NoteIDs = [];


        if (cnItem.IsChecked)
            itemPT.SetClaimAdjustmentFlag_NoteIDs.push(cnItem.NoteID);
        else
            itemPT.SetClaimAdjustmentFlag_NoteIDs.remove(cnItem.NoteID);
    };

    //#endregion



    /*Add Billing Note*/
    $scope.SaveBillingNote = function (item) {
        if ($scope.BillingNoteModel != null && $scope.BillingNoteModel.BillingNote != "" && $scope.BillingNoteModel != undefined) {
            $scope.BillingNoteModel.BatchID = $scope.BatchID;
            var jsonData = angular.toJson($scope.BillingNoteModel);
            AngularAjaxCall($http, HomeCareSiteUrl.SaveBillingNoteUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {
                if (response.IsSuccess) {
                    $scope.ABC = response.Data;
                    $scope.BillingNoteModel.BillingNote = null;
                    ShowMessages(response);
                    $scope.GetBillingNotes($scope.BillingNoteModel);
                    toastr.success("Billing Note Added Successfully ");
                    //$("#model_AddBillingNote").modal('hide');
                    $scope.BillingNoteModel = null;
                }
            });
        }
        else { toastr.error("Please Enter Note ") }
    };

    /*Get Billing Note*/
    $scope.GetBillingNotes = function (item) {
        $scope.BatchID = item.BatchID;
        var jsonData = angular.toJson({ BatchID: item.BatchID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetBillingNotesUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.GetBillingNoteList = response.Data;
                ShowMessages(response);
                $('#model_AllBillingNotes').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
        });
    };


    $scope.strLimit = 250;

    $scope.showMore = function (BillingNote) {
        debugger
        $scope.strLimit = BillingNote.length;
    };
    $scope.showLess = function () {
        $scope.strLimit = 250;
    };

    /*Update Billing Note*/
    $scope.BNoteID = {};
    $scope.UpdateBillinNoteButton = function (GetBillingData) {
        $scope.BNoteID = GetBillingData.BillingNoteID;
    }
    $scope.UpdateBillingNote = function (GetBillingData) {
        if (GetBillingData.BillingNote != "") {
            var jsonData = angular.toJson(GetBillingData);
            AngularAjaxCall($http, HomeCareSiteUrl.UpdateBillingNoteUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {
                if (response.IsSuccess) {
                    toastr.success("Billing Note is updated successfully");
                    $scope.BNoteID = null;
                    $scope.GetBillingNoteList = response.Data;
                }
                ShowMessages(response);
            });
        }
        else { toastr.error("Please Enter Note ") }
    }

    $scope.DeleteBillingNote = function (GetBillingData) {
        $scope.BillingNoteID = GetBillingData.BillingNoteID;
        $scope.BatchID = GetBillingData.BatchID;
        var jsonData = angular.toJson({ BillingNoteID: $scope.BillingNoteID, BatchID: $scope.BatchID });

        AngularAjaxCall($http, HomeCareSiteUrl.DeleteBillingNoteUrl, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.GetBillingNoteList = response.Data;

            }
            ShowMessages(response);
        });



    };
    $scope.hoverIn = function (BillingNoteID) {
        $scope.BtNoteID = BillingNoteID;
        document.getElementById("Actionbtn").style.display = "";
    };

    $scope.hoverOut = function (BillingNoteID) {
        $scope.BtNoteID = BillingNoteID;
        document.getElementById("Actionbtn").style.display = "none";
    };
}
/*Delete Billing Note*/







controllers.BatchController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    $(".dateInputMask").attr("placeholder", "mm/dd/yyyy");

    $('#model__ValidateAndGenerateEdi837').on('hidden.bs.modal', function () {
        $("#btnConfirm").show();
        $("#btnSubmitClaim").hide();
    });

    GetCookie(window.Cookie_AgingReportFilters);
    //setTimeout(function () { $("#PaymentStatusModal4").modal("hide"); }, 5000);

});



