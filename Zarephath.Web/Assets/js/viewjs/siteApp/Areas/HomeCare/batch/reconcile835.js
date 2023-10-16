var vm;

controllers.Reconcile835Controller = function ($scope, $http, $timeout) {
    vm = $scope;

    $scope.Reconcile835ListURL = HomeCareSiteUrl.Reconcile835ListURL;
    $scope.Reconcile835List = [];
    $scope.SelectedReconcile835Ids = [];
    $scope.SelectAllCheckbox = false;
    $scope.FormID = "#frmReconcile835";






    $scope.newInstance = function () {
        return $.parseJSON($("#hdnAddReconcile835Model").val());
    };
    $scope.Reconcile835Model = $.parseJSON($("#hdnAddReconcile835Model").val());
    //$scope.Reconcile835Model.SelectFileLabel = window.Select835File;

    $scope.SearchReconcile835ListPage = $scope.Reconcile835Model.SearchReconcile835ListPage;
    $scope.TempReconcile835ListPage = $scope.Reconcile835Model.SearchReconcile835ListPage;
    $scope.Reconcile835ListPager = new PagerModule("ServiceDate", undefined, 'DESC');

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchReconcile835Model: $scope.SearchReconcile835ListPage,
            pageSize: $scope.Reconcile835ListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.Reconcile835ListPager.sortIndex,
            sortDirection: $scope.Reconcile835ListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchReconcile835ListPage = $.parseJSON(angular.toJson($scope.TempReconcile835ListPage));
    };

    $scope.GetReconcile835List = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedReconcile835Ids = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchReconcile835ListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control
        
        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();

        if ($scope.SearchReconcile835ListPage.ServiceCodeID)
        $scope.SearchReconcile835ListPage.StrServiceCodeID = $scope.SearchReconcile835ListPage.ServiceCodeID.toString();

        $scope.AjaxStart = true;
        var jsonData = $scope.SetPostData($scope.Reconcile835ListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.Reconcile835ListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.Reconcile835List = response.Data.Items;
                $scope.Reconcile835ListPager.currentPageSize = response.Data.Items.length;
                $scope.Reconcile835ListPager.totalRecords = response.Data.TotalItems;
                $scope.ShowCollpase();
            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.Reconcile835ListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        //Reset 
        $scope.Upload835TokenObj.clear();
        $scope.SearchReconcile835ListPage = $scope.newInstance().SearchReconcile835ListPage;
        $scope.TempReconcile835ListPage = $scope.newInstance().SearchReconcile835ListPage;
        $scope.TempReconcile835ListPage.Str835ProcessedOnlyID = "0";
        $scope.TempReconcile835ListPage.ServiceCodeID = null;
        $scope.SearchReconcile835ListPage.ServiceCodeID = null;
        $scope.Reconcile835ListPager.currentPage = 1;
        $scope.Reconcile835ListPager.getDataCallback();
    };

    $scope.SearchReconcile835Files = function () {
        $scope.Reconcile835ListPager.currentPage = 1;
        $scope.Reconcile835ListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectReconcile835File = function (reconcile835) {
        if (reconcile835.IsChecked)
            $scope.SelectedReconcile835Ids.push(reconcile835.Reconcile835FileID);
        else
            $scope.SelectedReconcile835Ids.remove(reconcile835.Reconcile835FileID);

        if ($scope.SelectedReconcile835Ids.length == $scope.Reconcile835ListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedReconcile835Ids = [];
        angular.forEach($scope.Reconcile835List, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedReconcile835Ids.push(item.Reconcile835FileID);
        });
        return true;
    };



    $scope.Reconcile835ListPager.getDataCallback = $scope.GetReconcile835List;
    $scope.Reconcile835ListPager.getDataCallback();



    //Get missing Document Part
    $scope.GetBatchNoteDetails = function (item, runAlways) {
        if (item.CSModel == undefined || runAlways) {

            //BatchNoteID is Removed because we are not taking it from DB and List are Group BY NOTE AND BATCH
            var jsonData = angular.toJson({ BatchNoteID: item.BatchNoteID, BatchID: item.BatchID, NoteID: item.NoteID, Upload835FileID: $scope.SearchReconcile835ListPage.Upload835FileID });
            AngularAjaxCall($http, HomeCareSiteUrl.GetReconcileBatchNoteDetailsUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {

                    //AdjudicationDetailsModelList
                    item.AHList = response.Data.AdjudicationDetailsModelList;
                    //ClaimSubmissionDetailsModel
                    item.CSModel = response.Data.ClaimSubmissionDetailsModel;
                }
                ShowMessages(response);
            });
        }
    };


    $scope.MarkNoteAsLatest = function (data, item) {
        var jsonData = angular.toJson({ BatchNoteID: data.BatchNoteID, BatchID: data.BatchID, NoteID: data.NoteID, Upload835FileID: $scope.SearchReconcile835ListPage.Upload835FileID });
        AngularAjaxCall($http, HomeCareSiteUrl.MarkNoteAsLatestUrl, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.GetBatchNoteDetails(item, true);
            }
            ShowMessages(response);

            if (!$scope.$root.$$phase) {
                $scope.$apply();
            }
        });
    };


    //$scope.SetClaimAdjustmentFlag = function (item, type) {
    //    
    //    var msg = type === window.AdustmentType_Replacement ? window.ReplacementConfirmationMessage :
    //        type === window.AdustmentType_Void ? window.VoidConfirmationMessage : window.RemoveStatusMessage;
    //    bootboxDialog(function (result) {
    //        if (result) {
    //            var jsonData = angular.toJson({ BatchID: item.BatchID, NoteID: item.NoteID, ClaimAdjustmentType: type });
    //            AngularAjaxCall($http, HomeCareSiteUrl.SetClaimAdjustmentFlagUrl, jsonData, "Post", "json", "application/json").success(function (response) {
    //                
    //                if (response.IsSuccess) {
    //                    if (type == window.AdustmentType_Remove)
    //                        type = null;
    //                    item.ClaimAdjustmentTypeID = type;
    //                }
    //                ShowMessages(response);
    //            });
    //        }
    //    }, bootboxDialogType.Confirm, type, msg, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);

    //};

    var data = null;
    $scope.SetClaimAdjustmentFlag = function (item, type) {
        
        $('#ClaimAdjustmentReason-modal').modal('show');
        $scope.SubmitClaimAdjustmentReasonModel = {};
        var msg = type === window.AdustmentType_Replacement ? window.ReplacementConfirmationMessage :
            type === window.AdustmentType_Void ? window.VoidConfirmationMessage :
            type === window.AdustmentType_WriteOff ? window.WriteOffConfirmationMessage : window.RemoveStatusMessage;
        data = item;
        $scope.SubmitClaimAdjustmentReasonModel.Item = item;
        $scope.SubmitClaimAdjustmentReasonModel.BulkAction = false;
        $scope.SubmitClaimAdjustmentReasonModel.AdjustmentType = type;
        $scope.SubmitClaimAdjustmentReasonModel.Message = msg;
        $scope.SubmitClaimAdjustmentReasonModel.AdjustmentReason = item.ClaimAdjustmentReason;

    };


    $scope.SubmitClaimAdjustmentReason = function (item) {
        var jsonData = angular.toJson({
            batchId: $scope.SubmitClaimAdjustmentReasonModel.Item.BatchID,
            noteId: $scope.SubmitClaimAdjustmentReasonModel.Item.NoteID,
            claimAdjustmentType: $scope.SubmitClaimAdjustmentReasonModel.AdjustmentType,
            claimAdjustmentReason: $scope.SubmitClaimAdjustmentReasonModel.AdjustmentReason
        });
        AngularAjaxCall($http, HomeCareSiteUrl.SetClaimAdjustmentFlagUrl, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                if ($scope.SubmitClaimAdjustmentReasonModel.AdjustmentType === window.AdustmentType_Remove) {
                    $scope.SubmitClaimAdjustmentReasonModel.AdjustmentType = null;
                    $scope.SubmitClaimAdjustmentReasonModel.AdjustmentReason = null;
                }
                data.ClaimAdjustmentReason = $scope.SubmitClaimAdjustmentReasonModel.AdjustmentReason;
                data.ClaimAdjustmentTypeID = $scope.SubmitClaimAdjustmentReasonModel.AdjustmentType;
            }
            ShowMessages(response);
            $('#ClaimAdjustmentReason-modal').modal('hide');
        });

    };


    $scope.ShowCollpase = function () {
        setTimeout(function () {
            $.each($('.collapseDestination'), function (index, data) {
                var parent = data;
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


    //#region Uploaded 835 File Auto Completer
    $scope.GetUpload835URL = HomeCareSiteUrl.GetUpload835FilesURL;
    $scope.Upload835TokenObj = {};
    $scope.AdditionFilterForUpload835 = angular.toJson({
        PayorID: parseInt($scope.TempReconcile835ListPage.PayorID) > 0 ? $scope.TempReconcile835ListPage.PayorID : 0
    });
    $scope.$watch(function () { return $scope.TempReconcile835ListPage.PayorID; }, function () {
        $scope.AdditionFilterForUpload835 = angular.toJson({
            PayorID: parseInt($scope.TempReconcile835ListPage.PayorID) > 0 ? $scope.TempReconcile835ListPage.PayorID : 0
        });

    });



    $scope.Upload835ResultsFormatter = function (item) {
        return "<li id='{0}'>{1} <br/><small><b style='color:#ad0303;'>{6}:</b> {2}</small><br/><small><b'>{7}: </b>{4}</small><span class='pull-right badge badge-info'>{3}</span></li>"
            .format(
                item.Upload835FileID,
                item.FileName,
                item.Comment,
                item.Payor,
                moment(item.CreatedDate).format('MM/DD/YYYY'),
                item.Upload835FileProcessStatus,
                window.Comment,
                window.Uploaded
            );
    };
    $scope.Upload835TokenFormatter = function (item) {
        
        return "<li id='{0}'>{0}</li>".format(item.FileName);
    };
    $scope.RemoveUpload835 = function () {
        $scope.TempReconcile835ListPage.Upload835FileID = 0;
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };
    $scope.AddedUpload835 = function (item) {
        $scope.TempReconcile835ListPage.Upload835FileID = item.Upload835FileID;
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };
    //#endregion


    $scope.ExportReconcileList = function () {
        $scope.SearchModelMapping();
        $scope.AjaxStart = true;


        if ($scope.SearchReconcile835ListPage.ServiceCodeID)
            $scope.SearchReconcile835ListPage.StrServiceCodeID = $scope.SearchReconcile835ListPage.ServiceCodeID.toString();

        var jsonData = $scope.SetPostData($scope.Reconcile835ListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetExportReconcile835ListListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
            }
            ShowMessages(response);
            $scope.AjaxStart = false;
        });


    };




    //#region CheckBOX Select - UnSelecet Actions
    
    $scope.SelectedReconcileIds = [];
    $scope.SelectAllCheckbox = false;
    // This executes when select single checkbox selected in table.
    $scope.SelectReconcile = function (item) {
        var itemId = item.BatchID + '|' + item.NoteID;
        if (item.IsChecked)
            $scope.SelectedReconcileIds.push(itemId);
        else
            $scope.SelectedReconcileIds.remove(itemId);

        if ($scope.SelectedReconcileIds.length == $scope.Reconcile835ListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedReconcileIds = [];
        angular.forEach($scope.Reconcile835List, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked) {
                var itemId = item.BatchID + '|' + item.NoteID;
                $scope.SelectedReconcileIds.push(itemId);
            }
        });
        return true;
    };


    $scope.BulkSetClaimAdjustmentFlag = function (type) {
        
        $('#ClaimAdjustmentReason-modal').modal('show');
        $scope.SubmitClaimAdjustmentReasonModel = {};
        var msg = type === window.AdustmentType_Replacement ? window.ReplacementConfirmationMessage :
            type === window.AdustmentType_Void ? window.VoidConfirmationMessage :
            type === window.AdustmentType_WriteOff ? window.WriteOffConfirmationMessage : window.RemoveStatusMessage;
        //data = item;
        //$scope.SubmitClaimAdjustmentReasonModel.Item = item;
        $scope.SubmitClaimAdjustmentReasonModel.BulkAction = true;
        $scope.SubmitClaimAdjustmentReasonModel.AdjustmentType = type;
        $scope.SubmitClaimAdjustmentReasonModel.Message = msg;
        $scope.SubmitClaimAdjustmentReasonModel.AdjustmentReason = item.ClaimAdjustmentReason;

    };
    
    $scope.BulkSubmitClaimAdjustmentReason = function () {
        var jsonData = angular.toJson({
            itemId: $scope.SelectedReconcileIds.toString(),
            claimAdjustmentType: $scope.SubmitClaimAdjustmentReasonModel.AdjustmentType,
            claimAdjustmentReason: $scope.SubmitClaimAdjustmentReasonModel.AdjustmentReason
        });
        AngularAjaxCall($http, HomeCareSiteUrl.BulkSetClaimAdjustmentFlagUrl, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                if ($scope.SubmitClaimAdjustmentReasonModel.AdjustmentType === window.AdustmentType_Remove) {
                    $scope.SubmitClaimAdjustmentReasonModel.AdjustmentType = null;
                    $scope.SubmitClaimAdjustmentReasonModel.AdjustmentReason = null;
                }
            }
            ShowMessages(response);
            $('#ClaimAdjustmentReason-modal').modal('hide');
            $scope.Refresh();
        });

    };
    //#endregion

};
controllers.Reconcile835Controller.$inject = ['$scope', '$http', '$timeout'];


$(document).ready(function () {
    //$(".dateInputMask").inputmask("m/d/y", {
    //    placeholder: "mm/dd/yyyy"
    //});
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});