var vm;
controllers.AddServiceCodeController = function ($scope, $http, $window, $timeout) {
    //#region page Load Time Stuff

    vm = $scope;
    var modalJson = $.parseJSON($("#hdnAddPayorModel").val());
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnAddPayorModel").val());
    };
    $scope.ServiceCodeModel = modalJson;
    //$scope.maxDate = new Date();
    //$scope.maxDate.setDate($scope.maxDate.getDate() + 1);
    //$scope.NewDate                 = SetExpiryDate();
    $scope.ServiceCodeModel.PayorServiceCodeMapping.EncryptedPayorId = window.EncryptedPayorID;
    $scope.GetServiceCodeListURL = "/payor/getServicecodeList";

    $("a#ServiceCode").on('shown.bs.tab', function (e) {
        $scope.SetPayorServiceCodeMappingDetail();
    });

    $scope.PayorServiceCodeMappingList = [];
    $scope.SelectedPayorServiceCodeMappingIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.SearchServiceCodeMappingList = $scope.newInstance().SearchServiceCodeMappingList;
    $scope.TempPayorServiceCodeMappingList = $scope.newInstance().SearchServiceCodeMappingList;
    $scope.PayorServiceCodeMappingListPager = new PagerModule("PayorServiceCodeMappingID");

    //#endregion

    //#region ServiceCode Listing Part
    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            EncryptedPayorId: window.EncryptedPayorID,
            searchServiceCodeMappingList: $scope.SearchServiceCodeMappingList,
            pageSize: $scope.PayorServiceCodeMappingListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.PayorServiceCodeMappingListPager.sortIndex,
            sortDirection: $scope.PayorServiceCodeMappingListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchServiceCodeMappingList = $.parseJSON(angular.toJson($scope.TempPayorServiceCodeMappingList));
    };

    $scope.SetPayorServiceCodeMappingDetail = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedPayorServiceCodeMappingIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchServiceCodeMappingList.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();

        var jsonData = $scope.SetPostData($scope.PayorServiceCodeMappingListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetServiceCodeMappingList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.PayorServiceCodeMappingList = response.Data.Items;
                $scope.PayorServiceCodeMappingListPager.currentPageSize = response.Data.Items.length;
                $scope.PayorServiceCodeMappingListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.PayorServiceCodeMappingListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        //Reset 
        $scope.SearchServiceCodeMappingList = $scope.newInstance().SearchServiceCodeMappingList;
        $scope.TempPayorServiceCodeMappingList = $scope.newInstance().SearchServiceCodeMappingList;
        $scope.TempPayorServiceCodeMappingList.IsDeleted = "0";
        $scope.TempPayorServiceCodeMappingList.ModifierID = "0";
        $scope.TempPayorServiceCodeMappingList.PosID = "0";
        $scope.PayorServiceCodeMappingListPager.currentPage = 1;
        $scope.PayorServiceCodeMappingListPager.getDataCallback();
    };

    $scope.SearchPayorServiceCodeMapping = function () {
        $scope.PayorServiceCodeMappingListPager.currentPage = 1;
        $scope.PayorServiceCodeMappingListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectPayor = function (payorServiceCodeMappingList) {
        if (payorServiceCodeMappingList.IsChecked)
            $scope.SelectedPayorServiceCodeMappingIds.push(payorServiceCodeMappingList.PayorServiceCodeMappingID);
        else
            $scope.SelectedPayorServiceCodeMappingIds.remove(payorServiceCodeMappingList.PayorServiceCodeMappingID);
        if ($scope.SelectedPayorServiceCodeMappingIds.length == $scope.PayorServiceCodeMappingListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.

    $scope.SelectAll = function () {
        $scope.SelectedPayorServiceCodeMappingIds = [];
        angular.forEach($scope.PayorServiceCodeMappingList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedPayorServiceCodeMappingIds.push(item.PayorServiceCodeMappingID);
        });
        return true;
    };

    $scope.DeletePayorCodeMapping = function (payorServiceCodeMappingId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchServiceCodeMappingList.ListOfIdsInCSV = payorServiceCodeMappingId > 0 ? payorServiceCodeMappingId.toString() : $scope.SelectedPayorServiceCodeMappingIds.toString();
                if (payorServiceCodeMappingId > 0) {
                    if ($scope.PayorServiceCodeMappingListPager.currentPage != 1)
                        $scope.PayorServiceCodeMappingListPager.currentPage = $scope.PayorServiceCodeMappingList.length === 1 ? $scope.PayorServiceCodeMappingList.currentPage - 1 : $scope.PayorServiceCodeMappingList.currentPage;
                }
                else {
                    if ($scope.PayorServiceCodeMappingListPager.currentPage != 1 && $scope.SelectedPayorServiceCodeMappingIds.length == $scope.PayorServiceCodeMappingListPager.currentPageSize)
                        $scope.PayorServiceCodeMappingListPager.currentPage = $scope.PayorServiceCodeMappingListPager.currentPage - 1;
                }
                //Reset Selcted Checkbox items and Control
                $scope.SelectedPayorServiceCodeMappingIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.PayorServiceCodeMappingListPager.currentPage);
                AngularAjaxCall($http, SiteUrl.DeleteServiceCodeURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {

                        $scope.PayorServiceCodeMappingList = response.Data.Items;
                        $scope.PayorServiceCodeMappingListPager.currentPageSize = response.Data.Items.length;
                        $scope.PayorServiceCodeMappingListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };
    //#endregion

    //#region Service Code AutoCompleter
    $scope.ServiceCodeResultsFormatter = function (item) {
        return "<li id='{0}'>{0}: {1}<br/><small><b>{6}:</b> {2}</small><small><b style='padding-left:10px;'>{7}: </b>{3}</small><br/><small><b style='color:#ad0303;'>{8}: </b>{4}</small><small><b style='color:#ad0303;padding-left:10px;'>{9}: </b>{5} </small></li>"
            .format(
            item.ServiceCode,
            item.ServiceName,
            item.MaxUnit,
            item.DailyUnitLimit,
            item.IsBillable ? window.Yes : window.No,
            item.HasGroupOption ? window.Yes : window.No,
            window.MaxUnit,
            window.DailyUnitLimit,
            window.Billable,
            window.GroupOption);
    };
    $scope.ServiceCodeTokenFormatter = function (item) {

        return "<li id='{0}'>{0}</li>".format(item.ServiceCode);
        //return "<li id='{0}'>{0}:{1}</li>".format(item.ServiceCode, item.ServiceName);
    };
    //#endregion

    //#region Save Part

    $scope.SaveServiceCodeDetails = function () {
        if (CheckErrors("#FrmServiceCode")) {
            var jsonData = angular.toJson({
                addPayorServiceCodeMappingModel: $scope.ServiceCodeModel.PayorServiceCodeMapping,
            });
            AngularAjaxCall($http, SiteUrl.AddServiceCodeDetail, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.ClearServiceCodeDetails();
                    $scope.SetPayorServiceCodeMappingDetail(true);
                }
            });
        }
    };

    $scope.ClearServiceCodeDetails = function () {
        $scope.ServiceCodeModel.PayorServiceCodeMapping = $scope.newInstance().PayorServiceCodeMapping;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.EncryptedPayorId = window.EncryptedPayorID;
        //$scope.ServiceCodeModel.PayorServiceCodeMapping.POSStartDate = null;
        //$scope.ServiceCodeModel.PayorServiceCodeMapping.POSEndDate = null;
        $("#SearchContactToken").tokenInput("clear");
    };

    $scope.EditPayorCodeMapping = function (data) {
        var tempData = $.parseJSON(angular.toJson(data));
        $scope.ServiceCodeModel.PayorServiceCodeMapping.PayorServiceCodeMappingID = tempData.PayorServiceCodeMappingID;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.PayorID = tempData.PayorID;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.ServiceCodeID = tempData.ServiceCodeID;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.ModifierID = tempData.ModifierID;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.PosID = tempData.PosID;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.Rate = tempData.Rate;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.POSStartDate = tempData.POSStartDate;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.POSEndDate = tempData.POSEndDate;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.IsDeleted = tempData.IsDeleted;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.EncryptedPayorId = tempData.EncryptedPayorId;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.BillingUnitLimit = tempData.BillingUnitLimit;
        $("#SearchContactToken").tokenInput("clear");
        $("#SearchContactToken").tokenInput("add", { ServiceCodeID: tempData.ServiceCodeID, ServiceCode: tempData.ServiceCode });
        // $scope.NewDate = $scope.ServiceCodeModel.PayorServiceCodeMapping.POSStartDate;
    };

    //#endregion

    $scope.DatePickerDate = function (modelDate) {
        var a;
        if (modelDate) {
            if (modelDate == "0001-01-01T00:00:00Z" || modelDate == "0001-01-01T00:00:00") {
                $scope.maxDate = new Date();
                $scope.maxDate.setDate($scope.maxDate.getDate() + 1);
                $scope.NewDate = SetExpiryDate();
                a = $scope.NewDate;
            } else {
                var dt = new Date(modelDate);
                //dt >= newDate ? a = newDate :
                a = dt;
            }
        }
        else {
            $scope.maxDate = new Date();
            $scope.maxDate.setDate($scope.maxDate.getDate() + 1);
            $scope.NewDate = SetExpiryDate();
            a = $scope.NewDate;
            //a = newDate;
        }
        return moment(a).format('L');
    };
    $scope.PayorServiceCodeMappingListPager.getDataCallback = $scope.SetPayorServiceCodeMappingDetail;
};
controllers.AddServiceCodeController.$inject = ['$scope', '$http', '$window', '$timeout'];

$(document).ready(function () {
    //$(".dateInputMask").inputmask("m/d/y", {
    //    placeholder: "mm/dd/yyyy"
    //});
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});