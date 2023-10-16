var rrm;
controllers.ReferralReviewMeasurementController = function ($scope, $http, $window, $timeout) {
    rrm = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnAddReferralModel").val());
    };

    var modalJson = $.parseJSON($("#hdnAddReferralModel").val());
    $scope.ReferralModel = modalJson;
    $scope.Referral = $scope.ReferralModel.Referral;
    $scope.EncryptedReferralID = $scope.ReferralModel.Referral.EncryptedReferralID; //"iSNqtcWbe3gZEhtctmlPcA2";

   
    //#region Tab loads
    $("a#rm").on('shown.bs.tab', function (e) {
        if($.parseJSON($("#hdnOMPermission").val()))
            $(".tab-pane a[href='#tab_OutcomesMeasurement']").tab('show');
        else if ($.parseJSON($("#hdnACPermission").val()))
            $(".tab-pane a[href='#tab_AnsellCaseyReview']").tab('show');
        else if ($.parseJSON($("#hdnMSPermission").val()))
            $(".tab-pane a[href='#tab_ReferralsMonthlySummary']").tab('show');

    });

    $("a#rm_OutcomesMeasurement").on('shown.bs.tab', function (e) {
        $scope.GetReferralOutcomeMeasurementList();
    });

    $("a#rm_AnsellCaseyReview").on('shown.bs.tab', function (e, ui) {
        $scope.GetReferralReviewAssessmentList();
    });

    $("a#rm_ReferralsMonthlySummary").on('shown.bs.tab', function (e, ui) {
        $scope.GetReferralMonthlySummaryList();
        $scope.GetFacilityList();

    });

    //#endregion

    //#region  Referral Review Assessment
    $scope.ReferralAssessmentReviewModel = $scope.newInstance().ReferralAssessmentReview;
    $scope.ExistingReferralAssessmentReview = $scope.newInstance().ReferralAssessmentReview;
    $scope.SearchReferralAssessmentReview = $scope.newInstance().SearchReferralAssessmentReview;
    $scope.ReferralAssessmentReviewList = [];
    $scope.GraphSeriesList = [];

    $scope.OpenAddReferralReviewAssessment = function (referralAssessmentID) {
        AngularAjaxCall($http, SiteUrl.GetReferralReviewAssessmentURL, { referralID: $scope.ReferralModel.Referral.ReferralID, referralAssessmentID: referralAssessmentID }, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.ExistingReferralAssessmentReview = response.Data.PastReferralAssessmentReview;
                $scope.ReferralAssessmentReviewModel = response.Data.ReferralAssessmentReview;
                $scope.ReferralAssessmentAmazonSettingModel = response.Data.AmazonSettingModel;
                $("#model_ReviewAndAssessment").modal({ backdrop: 'static' });
            }
            ShowMessages(response);
        });
    };

    $scope.PrintDiv = function (id, referralAssessmentID) {
        AngularAjaxCall($http, SiteUrl.GetReferralReviewAssessmentURL, { referralID: $scope.ReferralModel.Referral.ReferralID, referralAssessmentID: referralAssessmentID }, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.ExistingReferralAssessmentReview = response.Data.PastReferralAssessmentReview;
                $scope.ReferralAssessmentReviewModel = response.Data.ReferralAssessmentReview;
                //     $("#model_ReviewAndAssessment").modal({ backdrop: 'static' });
            }
            //ShowMessages(response);
        });
        myApp.showPleaseWait();
        $scope.PrintContent = true;
        setTimeout(function () {
            printDiv($("#" + id));
            myApp.hidePleaseWait();
            $scope.PrintContent = false;
            $scope.$apply();
        }, 500);
    };


    $scope.SaveReferralAssessmentReview = function () {
        if (CheckErrors("#frmAnsellCaseyReview")) {
            $scope.ReferralAssessmentReviewModel.ReferralID = $scope.ReferralModel.Referral.ReferralID;
            AngularAjaxCall($http, SiteUrl.SaveReferralReviewAssessmentURL, { model: $scope.ReferralAssessmentReviewModel }, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $("#model_ReviewAndAssessment").modal('hide');
                    $scope.ReferralAssessmentReviewModel = $scope.newInstance().ReferralAssessmentReview;
                    $scope.ExistingReferralAssessmentReview = $scope.newInstance().ReferralAssessmentReview;
                    $scope.GetReferralReviewAssessmentList();
                }
                ShowMessages(response);
            });
        }
    };

    $scope.GetReferralReviewAssessmentList = function () {
        $scope.SearchReferralAssessmentReview.ReferralID = $scope.Referral.ReferralID;
        AngularAjaxCall($http, SiteUrl.GetReferralReviewAssessmentList, { searchModel: $scope.SearchReferralAssessmentReview }, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralAssessmentReviewList = response.Data.ReferralAssessmentList;
                $scope.GraphSeriesList = response.Data.GraphSeriesList;
            }
            ShowMessages(response);
        });
    };

    $scope.ResetReferralReviewAssessment = function () {
        $scope.SearchReferralAssessmentReview = $scope.newInstance().SearchReferralAssessmentReview;
        $scope.GetReferralReviewAssessmentList();
    };

    $scope.PrintGraph = function () {
        $(".jqChart-Graph").jqChart('exportToPdf');
    };


    //#region Upload Assesment Result
    $scope.ReferralAssessmentReviewModel.SaveConditionMessage = false;
    $scope.UploadAssessmentResult = SiteUrl.UploadAssessmentResult;
    $scope.UploadingFileList = [];
    $scope.BeforeSend = function (e, data) {
        $scope.ReferralAssessmentReviewModel.SaveConditionMessage = false;
        var isValidImage = true;
        var fileName;
        var errorMsg;

        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            if (extension == "exe") {
                file.FileProgress = 100;
                //$scope.UploadingFileList.remove(file);
                errorMsg = window.InvalidImageUploadMessage;
                isValidImage = false;
            }
            if ((file.size / 1024) > parseInt(window.FileSize)) {
                file.FileProgress = 100;
                errorMsg = window.MaximumUploadImageSizeMessage;
                isValidImage = false;
            }

            fileName = file.name;
            //var a = {
            //    FileName: fileName
            //};
            //$scope.ReferralDocumentList.push(a);
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
    $scope.AfterSend = function (data) {
        $scope.IsFileUploading = false;
        $scope.ReferralAssessmentReviewModel.FileName = data.FileName;
        $scope.ReferralAssessmentReviewModel.TempAssessmentResultUrl = data.FilePath;
        $scope.UploadingFileList = [];
        $scope.$apply();


        //var jsonData = angular.toJson({ file: data.FilePath, fileName: data.FileName, id: $scope.EncryptedReferralID });
        //AngularAjaxCall($http, SiteUrl.UploadAssessmentResult, jsonData, "Post", "json", "application/json", false).success(function (response) {
        //    if (response.IsSuccess) {
        //        $scope.AssessmentResultUrl = response.Data;
        //        $scope.UploadingFileList.remove(data.File);
        //    }
        //    ShowMessages(response);
        //});

    };

    $scope.DeleteAssessmentResult = function () {

        if ($scope.ReferralAssessmentReviewModel.AWSSignedFilePath) { //$scope.UploadingFileList.remove(file);
            $scope.ReferralAssessmentReviewModel.SaveConditionMessage = true;
        }

        $scope.ReferralAssessmentReviewModel.FileName = null;
        $scope.ReferralAssessmentReviewModel.FilePath = null;
        $scope.ReferralAssessmentReviewModel.AWSSignedFilePath = null;
        $scope.ReferralAssessmentReviewModel.TempAssessmentResultUrl = null;
    };
    //#endregion



    $scope.DeleteReferralAssessmentReview = function (referralAssessmentId) {
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchReferralAssessmentReview.ReferralID = $scope.Referral.ReferralID;
                $scope.SearchReferralAssessmentReview.ReferralAssessmentID = referralAssessmentId;
                var jsonData = { searchModel: $scope.SearchReferralAssessmentReview }

                AngularAjaxCall($http, SiteUrl.DeleteReferralReviewAssessmentUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.ReferralAssessmentReviewList = response.Data.ReferralAssessmentList;
                        $scope.GraphSeriesList = response.Data.GraphSeriesList;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, window.Delete, window.DeleteAnsellCaseyConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    //#endregion Referral Review Assessment
    
    //#region  Referral Outcome Measurement
    $scope.ReferralOutcomeMeasurementModel = $scope.newInstance().ReferralOutcomeMeasurement;
    $scope.SearchReferralOutcomeMeasurement = $scope.newInstance().SearchReferralOutcomeMeasurement;
    $scope.ReferralOutcomeMeasurementList = [];
    $scope.GraphSeriesOutcomeMeasurementList = [];


    $scope.OpenReferralOutcomeMeasurement = function (referralOutcomeMeasurementID) {

        AngularAjaxCall($http, SiteUrl.GetReferralOutcomeMeasurementURL, { referralID: $scope.ReferralModel.Referral.ReferralID, referralOutcomeMeasurementID: referralOutcomeMeasurementID }, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralOutcomeMeasurementModel = response.Data;
                $("#model_OutComeMeasurement").modal({ backdrop: 'static' });
            }
            ShowMessages(response);
        });

    };
    $scope.SaveReferralOutcomeMeasurement = function () {
        if (CheckErrors("#frmModelOM")) {
            $scope.ReferralOutcomeMeasurementModel.ReferralID = $scope.ReferralModel.Referral.ReferralID;
            AngularAjaxCall($http, SiteUrl.SaveReferralOutcomeMeasurementURL, { model: $scope.ReferralOutcomeMeasurementModel }, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $("#model_OutComeMeasurement").modal('hide');
                    $scope.ReferralOutcomeMeasurementModel = $scope.newInstance().ReferralOutcomeMeasurement;
                    $scope.GetReferralOutcomeMeasurementList();
                }
                ShowMessages(response);
            });
        }
    };

    $scope.GetReferralOutcomeMeasurementList = function () {
        $scope.SearchReferralOutcomeMeasurement.ReferralID = $scope.Referral.ReferralID;
        AngularAjaxCall($http, SiteUrl.GetReferralOutcomeMeasurementList, { searchModel: $scope.SearchReferralOutcomeMeasurement }, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralOutcomeMeasurementList = response.Data.ReferralOutcomeMeasurementList;
                $scope.GraphSeriesOutcomeMeasurementList = response.Data.GraphSeriesOutcomeMeasurementList;
                $scope.CalcOutcomeMeasurementAVG();
            }
            ShowMessages(response);
        });
    };
    $scope.ResetReferralOutcomeMeasurement = function () {

        $scope.SearchReferralOutcomeMeasurement = $scope.newInstance().SearchReferralOutcomeMeasurement;
        $scope.GetReferralOutcomeMeasurementList();
    };

    $scope.CalcOutcomeMeasurementAVG = function () {
        $scope.TotalROMCount = $scope.ReferralOutcomeMeasurementList.length;
        $scope.TotalROM1 = 0;
        $scope.TotalROM1 = 0;
        $scope.TotalROM2 = 0;
        $scope.TotalROM3 = 0;
        $scope.TotalROM4 = 0;
        $scope.TotalROM5 = 0;
        $scope.TotalROM6 = 0;
        $scope.TotalROM7 = 0;
        $scope.TotalROM8 = 0;
        $scope.TotalROM9 = 0;
        $scope.TotalROM10 = 0;
        $scope.TotalROM11 = 0;
        $scope.TotalROM12 = 0;
        $scope.TotalROM13 = 0;

        $scope.ReferralOutcomeMeasurementList.filter(function (item) {
            $scope.TotalROM1 = ($scope.TotalROM1 * 1) + (item.WorkCommunity * 1);
            $scope.TotalROM2 = ($scope.TotalROM2 * 1) + (item.ScheduledAppointment * 1);
            $scope.TotalROM3 = ($scope.TotalROM3 * 1) + (item.AskForHelp * 1);
            $scope.TotalROM4 = ($scope.TotalROM4 * 1) + (item.CommunicateEffectively * 1);
            $scope.TotalROM5 = ($scope.TotalROM5 * 1) + (item.PositiveBehavior * 1);
            $scope.TotalROM6 = ($scope.TotalROM6 * 1) + (item.QualityFriendship * 1);
            $scope.TotalROM7 = ($scope.TotalROM7 * 1) + (item.RespectOther * 1);
            $scope.TotalROM8 = ($scope.TotalROM8 * 1) + (item.StickUp * 1);
            $scope.TotalROM9 = ($scope.TotalROM9 * 1) + (item.LifeSkillGoals * 1);
            $scope.TotalROM10 = ($scope.TotalROM10 * 1) + (item.StayOutOfTrouble * 1);
            $scope.TotalROM11 = ($scope.TotalROM11 * 1) + (item.SuccessfulInSchool * 1);
            $scope.TotalROM12 = ($scope.TotalROM12 * 1) + (item.SuccessfulInLiving * 1);
            $scope.TotalROM13 = ($scope.TotalROM13 * 1) + (item.AdulthoodNeeds * 1);
        });

        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };

    $scope.PrintOutcomeMeasurementGraph = function () {
        $(".jqChart-Graph-OM").jqChart('exportToPdf');
    };


    $scope.DeleteReferralOutcomeMeasurement = function (referralOutcomeMeasurementId) {
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchReferralOutcomeMeasurement.ReferralID = $scope.Referral.ReferralID;
                $scope.SearchReferralOutcomeMeasurement.ReferralOutcomeMeasurementID = referralOutcomeMeasurementId;
                var jsonData = { searchModel: $scope.SearchReferralOutcomeMeasurement }

                AngularAjaxCall($http, SiteUrl.DeleteReferralOutcomeMeasurementUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.ReferralOutcomeMeasurementList = response.Data.ReferralOutcomeMeasurementList;
                        $scope.GraphSeriesOutcomeMeasurementList = response.Data.GraphSeriesOutcomeMeasurementList;
                        $scope.CalcOutcomeMeasurementAVG();
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, window.Delete, window.DeleteOutComeConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };
    //#endregion Referral Outcome Measurement

    //#region Referrals Monthly Summary
    $scope.ReferralMonthlySummaryModel = $scope.newInstance().ReferralMonthlySummary;
    $scope.SearchReferralMonthlySummary = $scope.newInstance().SearchReferralMonthlySummary;
    $scope.ReferralMonthlySummaryList = [];
    $scope.FacilityList = [];

    $scope.OpenReferralMonthlySummary = function (referralMonthlySummariesId,referralId) {
        if (referralId == undefined || referralId === 0)
            referralId = $scope.ReferralModel.Referral.ReferralID;

        //$scope.ReferralMonthlySummaryModel.ReferralID = referralId;
        AngularAjaxCall($http, SiteUrl.GetSetReferralMonthlySummaryURL,
                { referralID: referralId, referralMonthlySummariesID: referralMonthlySummariesId },
                "Post", "json", "application/json", false).success(function (response) {
                    if (response.IsSuccess) {
                        $scope.ReferralMonthlySummaryModel = response.Data.ReferralMonthlySummary;
                        //$scope.FacilityList = response.Data.FacilityList;
                        $("#model_MonthlySummery").modal({ backdrop: 'static' });
                    }
                    ShowMessages(response);
                });
    };

    $scope.GetSnapshotPrintReport = function (referralMonthlySummarieId) {
        if (referralMonthlySummarieId > 0) {
            var jsonData = angular.toJson({ searchSnapshotPrintModel: { ReferralMonthlySummariesIDs: referralMonthlySummarieId } });
            AngularAjaxCall($http, SiteUrl.GetSnapshotPrintReportUrl, jsonData, "Post", "json", "application/json", true).success(function (response) {
                if (response.IsSuccess) {
                    window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                } else {
                    ShowMessages(response);
                }
            });
        }
    };

    $scope.SaveReferralMonthlySummary = function () {
        if (CheckErrors("#frmModelMS")) {
            if ($scope.ReferralMonthlySummaryModel.ReferralID == undefined || $scope.ReferralMonthlySummaryModel.ReferralID === 0)
                $scope.ReferralMonthlySummaryModel.ReferralID = $scope.ReferralModel.Referral.ReferralID;
            AngularAjaxCall($http, SiteUrl.SaveReferralMonthlySummaryURL, { model: $scope.ReferralMonthlySummaryModel }, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $("#model_MonthlySummery").modal('hide');
                    $scope.ReferralMonthlySummaryModel = $scope.newInstance().ReferralMonthlySummary;
                    $scope.GetReferralMonthlySummaryList();
                }
                ShowMessages(response);
            });
        }
    };

    $scope.ReferralMonthlySummaryListPager = new PagerModule("ClientName", '', 'ASC');

    $scope.TempSearchReferralMonthlySummary = $scope.newInstance().SearchReferralMonthlySummary;

    $scope.SetPostData = function (fromIndex) {
        $scope.SearchReferralMonthlySummary.ReferralID = $scope.Referral.ReferralID;
        var pagermodel = {
            searchModel: $scope.SearchReferralMonthlySummary,
            pageSize: $scope.ReferralMonthlySummaryListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ReferralMonthlySummaryListPager.sortIndex,
            sortDirection: $scope.ReferralMonthlySummaryListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchReferralMonthlySummary = $.parseJSON(angular.toJson($scope.TempSearchReferralMonthlySummary));
        //$scope.ReferralMonthlySummary = $.parseJSON(angular.toJson($scope.ReferralMonthlySummary));
    };

    $scope.SearchReferralMonthly = function () {
        $scope.ReferralMonthlySummaryListPager.currentPage = 1;
        $scope.ReferralMonthlySummaryListPager.getDataCallback(true);
    };
    

    // This executes when select single checkbox selected in table.
    $scope.SelectReferralMS = function (referralMS) {
        if (referralMS.IsChecked)
            $scope.SelectedReferralMSIds.push(referralMS.ReferralMonthlySummariesID);
        else
            $scope.SelectedReferralMSIds.remove(referralMS.ReferralMonthlySummariesID);

        if ($scope.SelectedReferralMSIds.length == $scope.ReferralMonthlySummaryListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedReferralMSIds = [];

        angular.forEach($scope.ReferralMonthlySummaryList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedReferralMSIds.push(item.ReferralMonthlySummariesID);
        });
        return true;
    };

    $scope.DeleteReferralMS = function (referralMsId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchReferralMonthlySummary.ListOfIdsInCsv = referralMsId > 0 ? referralMsId.toString() : $scope.SelectedReferralMSIds.toString();

                if (referralMsId > 0) {
                    if ($scope.ReferralMonthlySummaryListPager.currentPage != 1)
                        $scope.ReferralMonthlySummaryListPager.currentPage = $scope.ReferralMonthlySummaryList.length === 1 ? $scope.ReferralMonthlySummaryListPager.currentPage - 1 : $scope.ReferralMonthlySummaryListPager.currentPage;
                } else {

                    if ($scope.ReferralMonthlySummaryListPager.currentPage != 1 && $scope.SelectedDxCodeIds.length == $scope.ReferralMonthlySummaryListPager.currentPageSize)
                        $scope.ReferralMonthlySummaryListPager.currentPage = $scope.ReferralMonthlySummaryListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedReferralMSIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.ReferralMonthlySummaryListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteReferralMonthlySummaryURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.ReferralMonthlySummaryList = response.Data.Items;
                        $scope.ReferralMonthlySummaryListPager.currentPageSize = response.Data.Items.length;
                        $scope.ReferralMonthlySummaryListPager.totalRecords = response.Data.TotalItems;
                    }
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableMonthlySummaryConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };



    $scope.GetReferralMonthlySummaryList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedReferralMSIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchReferralMonthlySummary.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control


        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();

        var jsonData = $scope.SetPostData($scope.ReferralMonthlySummaryListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetReferralMonthlySummaryListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {

                $scope.ReferralMonthlySummaryList = response.Data.Items;
                $scope.ReferralMonthlySummaryListPager.currentPageSize = response.Data.Items.length;
                $scope.ReferralMonthlySummaryListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        //$scope.ResetSearchFilter();
        //$scope.CaseManagerListPager.currentPage = $scope.CaseManagerListPager.currentPage;
        $scope.ReferralMonthlySummaryListPager.getDataCallback();
    };


    $scope.ResetReferralMonthlySummary = function () {

        $timeout(function () {
            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');
            $scope.SearchReferralMonthlySummary = $scope.newInstance().SearchReferralMonthlySummary;
            $scope.TempSearchReferralMonthlySummary = $scope.newInstance().SearchReferralMonthlySummary;
            $scope.TempSearchReferralMonthlySummary.IsDeleted = "0";
            $scope.ReferralMonthlySummaryListPager.currentPage = 1;
            $scope.ReferralMonthlySummaryListPager.getDataCallback();
        });

        //$scope.SearchReferralMonthlySummary = $scope.newInstance().SearchReferralMonthlySummary;
        //$scope.GetReferralMonthlySummaryList();
    };

    $scope.ReferralMonthlySummaryListPager.getDataCallback = $scope.GetReferralMonthlySummaryList;
    //if (!$scope.ReferralMonthlySummaryModel.IsMonthlySummaryListView)
    //    $scope.ReferralMonthlySummaryListPager.getDataCallback();



    //#region CHECK SERVICE DATE & FACILTY BASED ON 
    $scope.GetFacilityList = function () {
        if ($scope.FacilityList.length === 0) {
            AngularAjaxCall($http, SiteUrl.GetFacilityListURL, null, "Post", "json", "application/json", false).success(function (response) {
                if (response.IsSuccess) {
                    $scope.FacilityList = response.Data;
                }
            });
        }
    };
    $scope.ReferralMonthlySummaryModel.FaciltyAndServiceDateMappingIsValide = true;
    $scope.ValidateFaciltyAndServiceDate = function (startDate, endDate, facilityId, referralId) {

        var data = {
            model: { StartDate: startDate, EndDate: endDate, FacilityId: facilityId, ReferralID: referralId }
        };

        AngularAjaxCall($http, SiteUrl.FindScheduleWithFaciltyAndServiceDateURL, angular.toJson(data), "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralMonthlySummaryModel.FaciltyAndServiceDateMappingIsValide = response.Data;
            }
            ShowMessages(response);
        });

    };

    $scope.$watch(function () {
        return moment(new Date($scope.ReferralMonthlySummaryModel.MonthlySummaryStartDate)).format("YYYY/MM/DD")
            + moment(new Date($scope.ReferralMonthlySummaryModel.MonthlySummaryEndDate)).format("YYYY/MM/DD")
            + $scope.ReferralMonthlySummaryModel.FacilityID;
    }, function () {

        if ($scope.ReferralMonthlySummaryModel.FacilityID > 0 &&
            ValideDate($scope.ReferralMonthlySummaryModel.MonthlySummaryStartDate) && ValideDate($scope.ReferralMonthlySummaryModel.MonthlySummaryEndDate)) {

            var sdate = $scope.ReferralMonthlySummaryModel.MonthlySummaryStartDate;
            var edate = $scope.ReferralMonthlySummaryModel.MonthlySummaryEndDate;
            var fid = $scope.ReferralMonthlySummaryModel.FacilityID;
            var rid = $scope.ReferralMonthlySummaryModel.ReferralID;
            $scope.ValidateFaciltyAndServiceDate(sdate, edate, fid, rid);
        }
    });
    //#endregion




    //#region Monthaly Summary Form Validation

    $scope.$watchCollection('ReferralMonthlySummaryModel.MoodforThroughoutWeekendIds', function (newValue, oldValue) {
        if (newValue != null && newValue.length === 0) {
            $scope.ReferralMonthlySummaryModel.MoodforThroughoutWeekendIds = null;
        }
    });

    $scope.$watchCollection('ReferralMonthlySummaryModel.CoordinationofCareatPickupIds', function (newValue, oldValue) {
        if (newValue != null && newValue.length === 0) {
            $scope.ReferralMonthlySummaryModel.CoordinationofCareatPickupIds = null;
        }
    });

    $scope.$watchCollection('ReferralMonthlySummaryModel.CoordinationofCareatDropOffIds', function (newValue, oldValue) {
        if (newValue != null && newValue.length === 0) {
            $scope.ReferralMonthlySummaryModel.CoordinationofCareatDropOffIds = null;
        }
    });

    //#endregion


    //#Monthly Summary List Load Code
    $scope.MonthlySummaryListLoad = function () {
        if (window.location.pathname === '/referral/monthlysummarylist') {
            $scope.GetReferralMonthlySummaryList();
            $scope.GetFacilityList();
        }
    };

    if ($.parseJSON($("#hdnIsMonthlySummaryListView").val())) {
        $scope.MonthlySummaryListLoad();
    }
    //#endregion


    //$scope.Checked = function () {
    //    
    //    $scope.Checkedval = "";
    //    $scope.CheckedId = "";
    //    $scope.spn = false;
    //    angular.forEach($scope.ReferralModel.CheckBoxItems, function (value, key) {
    //        if (value.checked) {
    //            $scope.spn = true;
    //            if ($scope.Checkedval.length == 0) {
    //                $scope.Checkedval = value.Text;
    //                $scope.CheckedId = value.Value;
    //            } else {
    //                $scope.Checkedval += ", " + value.Text;
    //                $scope.CheckedId += ", " + value.Value;
    //            }
    //        }
    //    });
    //};

    //#endregion Referrals Monthly Summary

};

controllers.ReferralReviewMeasurementController.$inject = ['$scope', '$http', '$window', '$timeout'];


$(document).ready(function () {
    ShowPageLoadMessage("ShowAddReferralMessage");
});