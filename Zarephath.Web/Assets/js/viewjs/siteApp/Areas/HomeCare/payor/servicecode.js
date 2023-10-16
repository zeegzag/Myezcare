var vm;
controllers.AddServiceCodeController = function ($scope, $http, $window, $timeout) {
    //#region page Load Time Stuff

    vm = $scope;
    var modalJson = $.parseJSON($("#hdnAddPayorModel").val());
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnAddPayorModel").val());
    };
    $scope.ServiceCodeModel = modalJson;
    $scope.ModifierList = $scope.ServiceCodeModel.ModifierList;
    
    //$scope.maxDate = new Date();
    //$scope.maxDate.setDate($scope.maxDate.getDate() + 1);
    //$scope.NewDate                 = SetExpiryDate();
    $scope.ServiceCodeModel.PayorServiceCodeMapping.EncryptedPayorId = window.EncryptedPayorID;
    $scope.GetServiceCodeListURL = HomeCareSiteUrl.GetServiceCodeListURL;
    
    $("a#ServiceCode").on('shown.bs.tab', function (e) {
        $scope.ClearServiceCodeDetails();
        $scope.ResetSearchFilter();
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
        if ($scope.SearchModifierID) {
            $scope.TempPayorServiceCodeMappingList.ModifierID = $scope.SearchModifierID.toString();
            $scope.SearchModifierID = $scope.TempPayorServiceCodeMappingList.ModifierID.split(",");
        }
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
        AngularAjaxCall($http, HomeCareSiteUrl.GetServiceCodeMappingList, jsonData, "Post", "json", "application/json").success(function (response) {
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
        $scope.TempPayorServiceCodeMappingList.RevenueCode = "0";
        $scope.TempPayorServiceCodeMappingList.CareType = "-1";
        $scope.PayorServiceCodeMappingListPager.currentPage = 1;
        $scope.PayorServiceCodeMappingListPager.getDataCallback();
        $scope.SearchModifierID = [];
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
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteServiceCodeURL, jsonData, "Post", "json", "application/json").success(function (response) {
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
        return "<li id='{0}'><b style='color:#ad0303;'>Code: </b>{0}<br/><b style='color:#ad0303;'>Name: </b>{1}<br/><b style='color:#ad0303;'>Billable: </b>{2}</li>"
            .format(
            item.ServiceCode,
            item.ServiceName,
            item.IsBillable ? window.Yes : window.No
            );
    };

    $scope.ServiceCodeTokenFormatter = function (item) {
        return "<li id='{0}'>{0}</li>"
           .format(
            item.ServiceCode
       );
    };
    //#endregion

    //#region Save Part

    $scope.SaveServiceCodeDetails = function () {
        if (CheckErrors("#FrmServiceCode")) {
            var jsonData = angular.toJson({
                addPayorServiceCodeMappingModel: $scope.ServiceCodeModel.PayorServiceCodeMapping,
            });
            AngularAjaxCall($http, HomeCareSiteUrl.AddServiceCodeDetail, jsonData, "Post", "json", "application/json").success(function (response) {
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
        $scope.ServiceCodeModel.PayorServiceCodeMapping.CareType = tempData.CareTypeID;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.ServiceCodeID = tempData.ServiceCodeID;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.POSStartDate = tempData.POSStartDate;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.POSEndDate = tempData.POSEndDate;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.IsDeleted = tempData.IsDeleted;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.RevenueCode = tempData.RevenueCodeID;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.Rate = tempData.Rate;
        //$scope.ServiceCodeModel.PayorServiceCodeMapping.UM = tempData.UM;
        //$scope.ServiceCodeModel.PayorServiceCodeMapping.UPCRate = tempData.UPCRate;
        //$scope.ServiceCodeModel.PayorServiceCodeMapping.NegRate = tempData.NegRate;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.EncryptedPayorId = tempData.EncryptedPayorId;

        //$scope.ServiceCodeModel.PayorServiceCodeMapping.ModifierID = tempData.ModifierID;
        //$scope.ServiceCodeModel.PayorServiceCodeMapping.PosID = tempData.PosID;
        //$scope.ServiceCodeModel.PayorServiceCodeMapping.Rate = tempData.Rate;
        // $scope.ServiceCodeModel.PayorServiceCodeMapping.BillingUnitLimit = tempData.BillingUnitLimit;

        $scope.ServiceCodeModel.PayorServiceCodeMapping.UnitType = tempData.UnitType;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.PerUnitQuantity = tempData.PerUnitQuantity;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.RoundUpUnit = tempData.RoundUpUnit;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.MaxUnit = tempData.MaxUnit;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.DailyUnitLimit = tempData.DailyUnitLimit;

        $("#SearchContactToken").tokenInput("clear");
        //$("#SearchContactToken").tokenInput("add", { ServiceCodeID: tempData.ServiceCodeID, ServiceCode: tempData.ServiceCode });
        tempData.ServiceCode = tempData.ServiceCode + ((tempData.ModifierName === null || tempData.ModifierName === '') ? '' : ' - ' + tempData.ModifierName)
        $("#SearchContactToken").tokenInput("add", { ServiceCode: tempData.ServiceCode, CareTypeTitle: tempData.CareType });
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

    $scope.OnUnitTypeChange = function () {
        $scope.ServiceCodeModel.PerUnitQuantity = 0;
        $scope.ServiceCodeModel.RoundUpUnit = 0;
        $scope.ServiceCodeModel.MaxUnit = 0;
        $scope.ServiceCodeModel.DailyUnitLimit = 0;
    };

    $scope.$watch(function () { return $scope.ServiceCodeModel.PayorServiceCodeMapping.UnitType; }, function (newValue, oldValue) {
        if (!ValideElement(newValue)) $scope.SelectedUnit = "";
        if (parseInt(newValue) == parseInt(window.UnitTime)) $scope.SelectedUnit = window.UnitTimeValue;
        if (!$scope.$root.$$phase) $scope.$apply();

    }, true);

    $scope.$watch(function () { return $scope.ServiceCodeModel.PayorServiceCodeMapping.DefaultUnitIgnoreCalculation; }, function (newValue, oldValue) {
        if (!ValideElement(newValue)) $scope.ServiceCodeModel.PayorServiceCodeMapping.DefaultUnitIgnoreCalculation = 0;
        if (!$scope.$root.$$phase) $scope.$apply();
    });

    $scope.$watch(function () { return $scope.ServiceCodeModel.PayorServiceCodeMapping.MaxUnit; }, function (newValue, oldValue) {
        if (!ValideElement(newValue)) $scope.ServiceCodeModel.PayorServiceCodeMapping.MaxUnit = 0;
        if (!$scope.$root.$$phase) $scope.$apply();
    });

    $scope.$watch(function () { return $scope.ServiceCodeModel.PayorServiceCodeMapping.DailyUnitLimit; }, function (newValue, oldValue) {
        if (!ValideElement(newValue)) $scope.ServiceCodeModel.PayorServiceCodeMapping.DailyUnitLimit = 0;
        if (!$scope.$root.$$phase) $scope.$apply();
    });



    $scope.SerivceCodeModifierModel = $scope.ServiceCodeModel.SerivceCodeModifierModel;
    $scope.OpenAddServiceCodeModal = function ($event) {
        $event.stopPropagation();
        $('#addServiceCodeModel').modal({
            backdrop: 'static',
            keyboard: false
        });
    }


    $scope.SaveServiceCodeModal = function () {
        if (!$scope.checkDuplicateModifier()) {
            if (CheckErrors("#FrmAddServiceCode")) {
                var jsonData = angular.toJson($scope.SerivceCodeModifierModel);
                
                AngularAjaxCall($http, "/hc/payor/AddServiceCode", jsonData, "Post", "json", "application/json").
                    success(function (response) {
                    if (response.IsSuccess) {
                        $scope.OnCloseModel();
                    }
                    ShowMessages(response);
                });
            }
        }
        else {
            ShowMessage('Modifier shoud be uniqe.', "error");
        }

        
        
    }

    $scope.checkDuplicateModifier = function ()
    {
        $scope.ModifierList = [];
        $scope.IsDuplicate = false;
        if ($scope.SerivceCodeModifierModel.Modifier1 != null && $scope.SerivceCodeModifierModel.Modifier1 != '')
            $scope.ModifierList.push($scope.SerivceCodeModifierModel.Modifier1.toLowerCase());
        if ($scope.SerivceCodeModifierModel.Modifier2 != null && $scope.SerivceCodeModifierModel.Modifier2 != '')
            $scope.ModifierList.push($scope.SerivceCodeModifierModel.Modifier2.toLowerCase());
        if ($scope.SerivceCodeModifierModel.Modifier3 != null && $scope.SerivceCodeModifierModel.Modifier3 != '')
            $scope.ModifierList.push($scope.SerivceCodeModifierModel.Modifier3.toLowerCase());
        if ($scope.SerivceCodeModifierModel.Modifier4 != null && $scope.SerivceCodeModifierModel.Modifier4 != '')
            $scope.ModifierList.push($scope.SerivceCodeModifierModel.Modifier4.toLowerCase());

        var a = $scope.ModifierList;
        for (var i = 0; i <= a.length; i++) {
            for (var j = i; j <= a.length; j++) {
                if (i != j && a[i] == a[j]) {
                    $scope.IsDuplicate = true;
                }
            }
        }
        return $scope.IsDuplicate;
    }

    $scope.OnCloseModel = function () {
        $scope.SerivceCodeModifierModel.ServiceCode = '';

        $scope.SerivceCodeModifierModel.Modifier1 = '';
        $scope.SerivceCodeModifierModel.Modifier2 = '';
        $scope.SerivceCodeModifierModel.Modifier3 = '';
        $scope.SerivceCodeModifierModel.Modifier4 = '';

        $('#addServiceCodeModel').modal('hide');
        HideErrors("#FrmAddServiceCode");

    }

};
controllers.AddServiceCodeController.$inject = ['$scope', '$http', '$window', '$timeout'];

$(document).ready(function () {
    //$(".dateInputMask").inputmask("m/d/y", {
    //    placeholder: "mm/dd/yyyy"
    //});
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});