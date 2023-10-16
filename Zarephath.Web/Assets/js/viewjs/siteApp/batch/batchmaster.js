
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
    $scope.BatchList = [];
    $scope.SelectedBatchIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.Batch = $scope.newInstance().Batch;
    $scope.BatchModel.ApprovedFacilityList = $scope.newInstance().ApprovedFacilityList;
    $scope.TempBatchList = $scope.newInstance().Batch;
    $scope.BatchListPager = new PagerModule("BatchID", undefined, 'DESC');

    //#region Save Part

    $scope.SaveBatchDetails = function () {
        if (CheckErrors("#FrmBatch")) {

            if ($scope.BatchModel.ApprovedFacilityList.BillingProviderID)
                $scope.BatchModel.Batch.BillingProviderIDs = $scope.BatchModel.ApprovedFacilityList.BillingProviderID.toString();

            if ($scope.BatchModel.ServiceCodeList.ServiceCodeID)
                $scope.BatchModel.Batch.ServiceCodeIDs = $scope.BatchModel.ServiceCodeList.ServiceCodeID.toString();

            var jsonData = angular.toJson({
                addBatchModel: { Batch: $scope.BatchModel.Batch }
            });
            AngularAjaxCall($http, SiteUrl.AddBatchDetailURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {

                    $scope.ResetSearchFilter();
                    ShowMessages(response);
                }
                else {
                    ShowMessages(response);
                }
            });
        }
    };


    $scope.SaveBatchDetails01 = function () {
        $scope.BatchModel.Batch.OldVoidReplacement = true;
        $scope.SaveBatchDetails();
    };
    //#endregion

    //#region Bind DropDownlist

    $scope.$watch('BatchModel.Batch.PayorID', function (newValue, oldValue) {
        //if (newValue > 0) {
        //    $scope.BatchModel.ApprovedFacilityList = {};
        //    $scope.GetApprovedPayorsList(newValue);
        //} else {
        //    $scope.BatchModel.ApprovedFacilityList = {};
        //}
    });
    $scope.GetApprovedPayorsList = function (payorId) {
        var jsonData = angular.toJson({ payorId: payorId });
        AngularAjaxCall($http, SiteUrl.GetApprovedPayorsList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.BatchModel.ApprovedFacilityList = response.Data;
            }
        });
    };

    //#endregion

    //#region Bind Batch List

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchBatchList: $scope.TempBatchList,
            pageSize: $scope.BatchListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.BatchListPager.sortIndex,
            sortDirection: $scope.BatchListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        if ($scope.BatchModel.ApprovedFacilityList.BillingProviderID != null) {
            $scope.BatchModel.Batch.BillingProviderIDs = $scope.BatchModel.ApprovedFacilityList.BillingProviderID.toString();
        }
        $scope.TempBatchList = $.parseJSON(angular.toJson($scope.BatchModel.Batch));
    };

    $scope.GetBatchList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedBatchIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.Batch.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control
        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        $scope.AjaxStart = true;
        var jsonData = $scope.SetPostData($scope.BatchListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetBatchList, jsonData, "Post", "json", "application/json",false).success(function (response) {
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
        $scope.ClearBatchDetails();
        $scope.BatchListPager.currentPage = 1;
        $scope.BatchListPager.getDataCallback(true);
    };

    $scope.ClearBatchDetails = function () {
        $scope.BatchModel.Batch = $scope.newInstance().Batch;
        $scope.BatchModel.ApprovedFacilityList.BillingProviderID = [];
        $scope.BatchModel.ServiceCodeList.ServiceCodeID = [];
        $scope.BatchModel.Batch.IsSentStatus = '-1';
        $scope.BatchModel.Batch.StartDate = null;
        $scope.BatchModel.Batch.EndDate = null;
    };

    $scope.ResetSearchFilter = function () {
        $scope.BatchModel.Batch = $scope.newInstance().Batch;
        $scope.BatchModel.ApprovedFacilityList.BillingProviderID = [];
        $scope.BatchModel.ServiceCodeList.ServiceCodeID = [];
        $scope.BatchModel.Batch.StartDate = null;
        $scope.BatchModel.Batch.EndDate = null;
        $scope.BatchModel.Batch.IsSentStatus = '-1';
        HideErrors("#FrmBatch");
        $scope.BatchListPager.currentPage = 1;
        $scope.BatchListPager.getDataCallback(true);
    };

    $scope.SearchBatch = function () {
        HideErrors("#FrmBatch");
        $scope.BatchListPager.currentPage = 1;
        $scope.BatchListPager.getDataCallback(true);
    };

    $scope.SelectBatch = function (batchList) {
        if (batchList.IsChecked)
            $scope.SelectedBatchIds.push(batchList.BatchID);
        else
            $scope.SelectedBatchIds.remove(batchList.BatchID);
        if ($scope.SelectedBatchIds.length == $scope.BatchListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    $scope.SelectAll = function () {
        $scope.SelectedBatchIds = [];
        angular.forEach($scope.BatchList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedBatchIds.push(item.BatchID);
        });
        return true;
    };

    //#endregion

    //#region Batch Delete  and Mark as Send part

    $scope.DeleteBatch = function (batchId, title) {
        title = window.HardDelete;
        bootboxDialog(function (result) {
            if (result) {

                $scope.TempBatchList.ListOfIdsInCSV = batchId > 0 ? batchId.toString() : $scope.SelectedBatchIds.toString();
                //    $scope.TempBatchList.ListOfIdsInCSV = ',' + $scope.TempBatchList.ListOfIdsInCSV + ',';

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
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.BatchListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteBatch, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.BatchList = response.Data.Items;
                        $scope.BatchListPager.currentPageSize = response.Data.Items.length;
                        $scope.BatchListPager.totalRecords = response.Data.TotalItems;
                        //$scope.ResetSearchFilter();
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.MarkasSentBatch = function (batchId, title) {
        if (title == 'Sent') {
            title = window.MarkAsSent;
            $scope.TempBatchList.IsSent = true;
        } else {
            title = window.MarkAsUnSent;
            $scope.TempBatchList.IsSent = false;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.TempBatchList.ListOfIdsInCSV = batchId > 0 ? batchId.toString() : $scope.SelectedBatchIds.toString();
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
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.BatchListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.MarkasSentBatch, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.BatchList = response.Data.Items;
                        $scope.BatchListPager.currentPageSize = response.Data.Items.length;
                        $scope.BatchListPager.totalRecords = response.Data.TotalItems;
                        // $scope.ResetSearchFilter();
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.MarkAsSentConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnEnable);
    };


    
    $scope.PaperRemitsEOBTemplate = function (batchId) {
        $scope.TempBatchList.ListOfIdsInCSV = batchId > 0 ? batchId.toString() : $scope.SelectedBatchIds.toString();

        var jsonData = { batchid: $scope.TempBatchList.ListOfIdsInCSV };
        AngularAjaxCall($http, SiteUrl.GenratePaperRemitsEOBTemplate, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                if (response.Data.AbsolutePath == "true") {
                    window.location = '/batch/DownloadFiles?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                } else {
                    window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                }
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

    $scope.BatchListPager.getDataCallback = $scope.GetBatchList;
    $scope.BatchListPager.getDataCallback();

    //#region EDI 837 Files Validation And Generation
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

    $scope.ValidateAndGenerateEdi837 = function (validateOnly, fileExtension, batchId) {
        $scope.ListOfIdsInCSV = batchId > 0 ? batchId.toString() : $scope.SelectedBatchIds.toString();
        $scope.ValidateAndGenerateEdi837Model.ValidateOnly = validateOnly;
        $scope.ValidateAndGenerateEdi837Model.PageTitle = validateOnly ? window.ValidateBatches : window.ValidateAndGenerateEDI837;
        $scope.ValidateAndGenerateEdi837Model.ValidateWaitText = window.ValidateWaitText;
        $scope.ValidateAndGenerateEdi837Model.FileExtension = fileExtension;
        $scope.ValidateAndGenerateEdi837Model.FilteredBatchList = $scope.BatchList.filter(function (item) {
            return $scope.FilterBatchForValidation(item);
        });
        $("#model__ValidateAndGenerateEdi837").modal("show");
    };


    $scope.DoEdi837Action = function (ele) {
        $scope.ValidateAndGenerateEdi837Model.ShowLoader = true;
        $(ele).button('loading');

        var url = $scope.ValidateAndGenerateEdi837Model.ValidateOnly ? SiteUrl.ValidateBatches : SiteUrl.GenerateEdi837Files;
        var jsonData = angular.toJson({
            postEdiValidateGenerateModel: {
                ListOfBacthIdsInCsv: $scope.ListOfIdsInCSV, FileExtension: $scope.ValidateAndGenerateEdi837Model.FileExtension,
                GenerateEdiFile: !$scope.ValidateAndGenerateEdi837Model.ValidateOnly
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


    //#region Download Overview File 

    $scope.DownloadOverViewFile = function (batchId) {
        $scope.TempBatchList.ListOfIdsInCSV = batchId > 0 ? batchId.toString() : $scope.SelectedBatchIds.toString();

        //Reset Selcted Checkbox items and Control
        //$scope.SelectedBatchIds = [];
        //$scope.SelectAllCheckbox = false;
        //Reset Selcted Checkbox items and Control

        var jsonData = { batchid: $scope.TempBatchList.ListOfIdsInCSV };
        AngularAjaxCall($http, SiteUrl.GenrateOverViewFile, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                if (response.Data.AbsolutePath == "true") {
                    window.location = '/batch/DownloadFiles?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                } else {
                    window.location = '/report/Download?vpath=' + response.Data.VirtualPath + '&fname=' + response.Data.FileName;
                }
                //$scope.BatchModel.Batch.IsSentStatus = '-1';
                //$scope.BatchListPager.currentPage = 1;
                //$scope.BatchListPager.getDataCallback(true);
            } ShowMessages(response);
        });
    };
    //#endregion
};
controllers.BatchController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    //$(".dateInputMask").inputmask("m/d/y", {
    //    placeholder: "mm/dd/yyyy"
    //});
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});

