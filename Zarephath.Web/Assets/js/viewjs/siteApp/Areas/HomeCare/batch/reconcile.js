var vm;

controllers.Reconcile835Controller = function ($scope, $http, $timeout) {
    vm = $scope;

    $scope.ReconcileListURL = HomeCareSiteUrl.ReconcileListURL;
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
    $scope.EditBatchNoteID = 0;
    $scope.TempClaimStatusCode = $scope.Reconcile835Model.ClaimStatusCode;
    $scope.TempClaimStatus = $scope.Reconcile835Model.ClaimStatus;
    $scope.BatchTemp = { PaidAmount: 0 };

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
        AngularAjaxCall($http, HomeCareSiteUrl.ReconcileListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
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
    //$scope.Reconcile835ListPager.getDataCallback();

    //$scope.ParentReconcileListPager = new PagerModule("ReferralID", undefined, 'ASC');
    
    $scope.GetParentReconcile = function (item, trElem) {
        
        $(trElem).toggleClass("openParentReconcile");
        $scope.ShowCollpase();
        if (item.ParentReconcileList == undefined) {
            item.ParentReconcileListPager = new PagerModule("ReferralID", undefined, 'ASC');
            //BatchNoteID is Removed because we are not taking it from DB and List are Group BY NOTE AND BATCH
            
            //var jsonData = angular.toJson({
            //    searchGetParentReconcileList: {
            //        ReferralID: item.ReferralID,
            //        ServiceStartDate: $scope.TempReconcile835ListPage.ServiceStartDate, ServiceEndDate: $scope.TempReconcile835ListPage.ServiceEndDate,
            //        fromIndex: item.ParentReconcileListPager.currentPage, pageSize: item.ParentReconcileListPager.pageSize
            //    }
            //});
            $scope.SearchReconcile835ListPage.ReferralID = item.ReferralID;
            //var jsonData = angular.toJson($scope.SearchReconcile835ListPage);
            var jsonData = $scope.SetPostData($scope.SearchReconcile835ListPage);
            
            AngularAjaxCall($http, HomeCareSiteUrl.ParentReconcileListURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    //AdjudicationDetailsModelList
                    item.ParentReconcileList = response.Data.Items;
                    item.ParentReconcileListPager.currentPageSize = response.Data.Items.length;
                    item.ParentReconcileListPager.totalRecords = response.Data.TotalItems;
                    item.ParentReconcileListPager.getDataCallback = $scope.GetParentReconcilePagination;
                    item.PagerID = Math.floor(Math.random() * 1000000000);
                }
                ShowMessages(response);
            });
        }
    };

    $scope.GetParentReconcilePagination = function (item) {
        var jsonData = angular.toJson({
            ReferralID: item.ReferralID,
            ServiceStartDate: $scope.TempReconcile835ListPage.ServiceStartDate, ServiceEndDate: $scope.TempReconcile835ListPage.ServiceEndDate,
            fromIndex: item.ParentReconcileListPager.currentPage, pageSize: item.ParentReconcileListPager.pageSize
        });
        AngularAjaxCall($http, HomeCareSiteUrl.ParentReconcileListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                //AdjudicationDetailsModelList
                item.ParentReconcileList = response.Data.Items;
                item.ParentReconcileListPager.currentPageSize = response.Data.Items.length;
                item.ParentReconcileListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };


    //Get missing Document Part
    $scope.GetBatchNoteDetails = function (item, trElem) {
        $(trElem).toggleClass("openTrReconcile");

        if (item.AHList == undefined) {
            //BatchNoteID is Removed because we are not taking it from DB and List are Group BY NOTE AND BATCH
            var jsonData = angular.toJson({NoteID: item.NoteID});
            AngularAjaxCall($http, HomeCareSiteUrl.GetChildReconcileListUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    //AdjudicationDetailsModelList
                    item.AHList = response.Data.AdjudicationDetailsModelList;
                    item.BHList = response.Data.BatchHistoryModelList;
                    item.NHModel = response.Data.NoteHistoryModel;
                }
                ShowMessages(response);
            });
        }
    };

    $scope.MarkNoteAsLatest = function (data, item) {
        var jsonData = angular.toJson({ BatchNoteID: data.BatchNoteID, BatchID: data.BatchID, NoteID: data.NoteID, Upload835FileID: $scope.SearchReconcile835ListPage.Upload835FileID });
        AngularAjaxCall($http, HomeCareSiteUrl.MarkNoteAsLatest01Url, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.GetBatchNoteDetails(item, true);
            }
            ShowMessages(response);

            if (!$scope.$root.$$phase) {
                $scope.$apply();
            }
        });
    };


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

            $.each($('.collapseDestination1'), function (index, data) {
                var parent = data;
                $(parent).on('show.bs.collapse', function (e) {
                    if ($(this).is(e.target))
                        $(parent).parent("tbody").find(".collapseSource1").removeClass("fa-plus-circle").addClass("fa-minus-circle");

                });
                $(parent).on('hidden.bs.collapse', function (e) {
                    if ($(this).is(e.target))
                        $(parent).parent("tbody").find(".collapseSource1").removeClass("fa-minus-circle").addClass("fa-plus-circle");
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
        AngularAjaxCall($http, HomeCareSiteUrl.GetExportReconcileListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
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

    //#region - Edit Status
    $scope.EditParentReconcileList = function (batch) {
        $scope.EditBatchNoteID = batch.BatchNoteID;
        $scope.BatchTemp.PaidAmount = batch.PaidAmount;
    };

    $scope.UpdateParentReconcileList = function (batch) {
        var jsonData = angular.toJson({
            batchNoteID: batch.BatchNoteID,
            paidAmount: $scope.BatchTemp.PaidAmount,
            claimStatusID: $scope.TempClaimStatus.ClaimStatusID !== null && $scope.TempClaimStatus.ClaimStatusID !== undefined ? $scope.TempClaimStatus.ClaimStatusID : 0,
            claimStatusCodeID: $scope.TempClaimStatusCode.ClaimStatusCodeID !== null && $scope.TempClaimStatusCode.ClaimStatusCodeID !== undefined ? $scope.TempClaimStatusCode.ClaimStatusCodeID : 0
        });
        
        AngularAjaxCall($http, HomeCareSiteUrl.UpdateBatchReconcile, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                batch.PaidAmount = $scope.BatchTemp.PaidAmount;
                batch.Status = $scope.Reconcile835Model.ClaimStatusList.find(x => x.ClaimStatusID === $scope.TempClaimStatus.ClaimStatusID).StatusName;
                batch.ClaimStatus = $scope.Reconcile835Model.ClaimStatusCodeList.find(x => x.ClaimStatusCodeID === $scope.TempClaimStatusCode.ClaimStatusCodeID).ClaimStatusName;
                $scope.TempClaimStatus.ClaimStatusID = 0;
                $scope.TempClaimStatusCode.ClaimStatusCodeID = 0;
            }
            ShowMessages(response);
        });

        $scope.EditBatchNoteID = 0;
    };

    $scope.CancelUpdate = function () {
        $scope.EditBatchNoteID = 0;
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