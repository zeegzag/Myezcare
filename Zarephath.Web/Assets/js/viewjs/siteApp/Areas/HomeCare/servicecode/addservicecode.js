var vm;

controllers.AddServiceCodeController = function ($scope, $http, $window) {

    //#region Page Load Time Stuff
    vm = $scope;
    var modalJson = $.parseJSON($("#hdnAddServiceCodeModel").val());
    $scope.ServiceCodeModel = modalJson.ServiceCodes;
    $scope.TempServiceCodeModel = angular.copy($scope.ServiceCodeModel);
    $scope.EncryptedServiceCodeID = $scope.ServiceCodeModel.EncryptedServiceCodeID;
    $scope.ModifierModel = modalJson.ModifierModel;
    $scope.ModifierList = modalJson.ModifierList;
    $scope.ModifierListForDropDown = modalJson.ModifierList;
    $scope.PayorList = modalJson.PayorsList;
    //#endregion

    if ($scope.ServiceCodeModel.ModifierID != null) {
        $scope.SelectedModifierID = [];
        $scope.SelectedModifierID = $scope.ServiceCodeModel.ModifierID.split(",");
    }

    $scope.$on("loadChildData", function (event, args) {
        $scope.ServiceCodeModel = args;
        if ($scope.ServiceCodeModel.ModifierID !== null) {
            $scope.SelectedModifierID = [];
            $scope.SelectedModifierID = $scope.ServiceCodeModel.ModifierID.split(",");
        }
    });
    //#region Save and Get ServiceCode Details
    $scope.Save = function () {
        var isValid = (CheckErrors("#frmServiceCodes"))
        if (isValid) {
            $scope.ServiceCodeModel.ModifierID = ($scope.SelectedModifierID) ? $scope.SelectedModifierID.toString() : null;
            var jsonData = angular.toJson({ addServiceCodeModel: { ServiceCodes: $scope.ServiceCodeModel } });
            AngularAjaxCall($http, HomeCareSiteUrl.SetAddServiceCodePageURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    if ($scope.ServiceCodeModel.ServiceCodeID == 0 || $scope.ServiceCodeModel.ServiceCodeID == undefined) {
                        toastr.success("Service Code Save Successfully");
                        $scope.ServiceCodeModel = null;
                        $scope.SelectedModifierID = '';
                    }
                    else {
                        toastr.success("Service Code Update Successfully");
                    }
                }
                else
                    ShowMessages(response);
            });
        }
    };

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

    //#endregion

    $scope.SearchModifierModel = {
        ModifierCode: null,
        ModifierName: null,
        IsDeleted: '0',
        ListOfIdsInCSV: []
    }
    $scope.SelectedModifierIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.OpenModifierModal = function ($event) {
        $event.stopPropagation();

        $scope.SearchModifierModel.ModifierCode = '';
        $scope.SearchModifierModel.ModifierName = '';
        $scope.SearchModifierModel.IsDeleted = '0';

        $scope.GetModifierList();
        $('#addModifierModel').modal('show');
    }
    $scope.OnCloseModel = function () {
        $scope.SearchModifierModel.ModifierCode = '';
        $scope.SearchModifierModel.ModifierName = '';
        $scope.SearchModifierModel.IsDeleted = '0';

        $scope.ModifierModel.ModifierCode = '';
        $scope.ModifierModel.ModifierName = '';
        $scope.ModifierModel.ModifierID = 0;

        $scope.UpdateModifierListForDropdown();

        $('#addModifierModel').modal('hide');
    }
    $scope.ResetModifier = function () {
        $scope.ModifierModel.ModifierCode = '';
        $scope.ModifierModel.ModifierName = '';
        $scope.ModifierModel.ModifierID = 0;
    }

    $scope.UpdateModifierListForDropdown = function () {
        var jsonData = angular.toJson({
            ModifierCode: "",
            ModifierName: "",
            IsDeleted: 0
        });
        AngularAjaxCall($http, HomeCareSiteUrl.GetModifierListURL, jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.ModifierListForDropDown = response.Data;
                }
                else {
                    ShowMessages(response);
                }
            });
    }

    $scope.GetModifierList = function () {

        $scope.SelectedModifierIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchModifierModel.ListOfIdsInCSV = [];

        var jsonData = angular.toJson({
            ModifierCode: $scope.SearchModifierModel.ModifierCode,
            ModifierName: $scope.SearchModifierModel.ModifierName,
            IsDeleted: $scope.SearchModifierModel.IsDeleted
        });
        AngularAjaxCall($http, HomeCareSiteUrl.GetModifierListURL, jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.ModifierList = response.Data;
                }
                else {
                    ShowMessages(response);
                }
            });
    }
    $scope.Reset = function () {
        $scope.SearchModifierModel.ModifierCode = null;
        $scope.SearchModifierModel.ModifierName = null;
        $scope.SearchModifierModel.IsDeleted = '0';

        $scope.GetModifierList();
    }

    $scope.EditModifier = function (item) {
        $scope.ModifierModel.ModifierID = item.ModifierID;
        $scope.ModifierModel.ModifierCode = item.ModifierCode;
        $scope.ModifierModel.ModifierName = item.ModifierName;
    }

    $scope.SaveModifier = function () {
        var isValid = CheckErrors($("#frmAddModifier"));
        if (isValid) {
            var jsonData = angular.toJson($scope.ModifierModel);
            AngularAjaxCall($http, HomeCareSiteUrl.SaveModifierURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        $scope.ResetModifier();
                        $scope.Reset();
                        $scope.GetModifierList();
                    }
                    ShowMessages(response);
                });
        }
    }

    $scope.SelectModifier = function (item) {
        if (item.IsChecked)
            $scope.SelectedModifierIds.push(item.ModifierID);
        else
            $scope.SelectedModifierIds.remove(item.ModifierID);

        if ($scope.SelectedModifierIds.length == $scope.ModifierList.length)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    $scope.SelectAll = function () {
        $scope.SelectedModifierIds = [];
        angular.forEach($scope.ModifierList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedModifierIds.push(item.ModifierID);
        });
        return true;
    };

    $scope.DeleteModifier = function (modifierId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {

                $scope.SearchModifierModel.ListOfIdsInCSV = modifierId > 0 ? modifierId.toString() : $scope.SelectedModifierIds.toString();

                $scope.SelectedModifierIds = [];
                $scope.SelectAllCheckbox = false;

                var jsonData = angular.toJson($scope.SearchModifierModel);

                AngularAjaxCall($http, HomeCareSiteUrl.DeleteModifierURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.GetModifierList();
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.Refresh = function () {
        $scope.GetModifierList();
    };
    $scope.Cancel = function () {
        window.location.reload();
    }



    /**************************  Ticket##2frxen  *****************************/
    $scope.EncryptedPayorId = 0;

    $scope.PayorServiceCodeMappingList = [];
    $scope.SelectedPayorServiceCodeMappingIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.SearchServiceCodeMappingList = [];
    $scope.TempPayorServiceCodeMappingList = [];
    $scope.GetServiceCodeListURL = HomeCareSiteUrl.GetServiceCodeListURL;
    $("#ServiceCodes_ServiceCode").attr("disable", "disable");
    $scope.PayorServiceCodeMappingListPager = new PagerModule("PayorServiceCodeMappingID");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            payorId: $scope.ServiceCodeModel.PayorServiceCodeMapping.PayorID,
            serviceCode: $scope.ServiceCodeModel.ServiceCode,
            CareType: "-1",
            listOfIdsInCSV: "",
            searchServiceCodeMappingList: $scope.SearchServiceCodeMappingList,
            pageSize: $scope.PayorServiceCodeMappingListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.PayorServiceCodeMappingListPager.sortIndex,
            sortDirection: $scope.PayorServiceCodeMappingListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.ListMappedServiceCode = function (payerId) {
        
        if (payerId > 0) {
            var jsonData = $scope.SetPostData($scope.PayorServiceCodeMappingListPager.currentPage);
            AngularAjaxCall($http, HomeCareSiteUrl.GetServiceCodeMappingListNew, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.PayorServiceCodeMappingList = response.Data.Items;
                    $scope.PayorServiceCodeMappingListPager.currentPageSize = response.Data.Items.length;
                    $scope.PayorServiceCodeMappingListPager.totalRecords = response.Data.TotalItems;
                }
                ShowMessages(response);
            });

            $scope.IsAddVisible = false;
        }
        else {
            $scope.PayorServiceCodeMappingList = [];
        }
    }

    $scope.SearchModelMapping = function () {
        if ($scope.SearchModifierID) {
            $scope.TempPayorServiceCodeMappingList.ModifierID = $scope.SearchModifierID.toString();
            $scope.SearchModifierID = $scope.TempPayorServiceCodeMappingList.ModifierID.split(",");
        }
        $scope.SearchServiceCodeMappingList = $.parseJSON(angular.toJson($scope.TempPayorServiceCodeMappingList));
    };

    //This will hide the DIV by default.
    $scope.IsAddVisible = false;
    $scope.ShowHideAdd = function () {

        //If DIV is visible it will be hidden and vice versa.
        $scope.IsAddVisible = $scope.IsAddVisible ? false : true;
    }

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
                $scope.PayorServiceCodeMappingListPager.ListOfIdsInCSV = payorServiceCodeMappingId > 0 ? payorServiceCodeMappingId.toString() : $scope.SelectedPayorServiceCodeMappingIds.toString();
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

                var pagermodel = {
                    payorId: $scope.ServiceCodeModel.PayorServiceCodeMapping.PayorID,//$scope.EncryptedPayorId, //"W_6GplXhhdqph21g0NeJqw2", 
                    serviceCode: $scope.ServiceCodeModel.ServiceCode,
                    CareType: "-1",
                    listOfIdsInCSV: $scope.PayorServiceCodeMappingListPager.ListOfIdsInCSV,
                    searchServiceCodeMappingList: $scope.SearchServiceCodeMappingList,
                    pageSize: $scope.PayorServiceCodeMappingListPager.pageSize,
                    pageIndex: $scope.PayorServiceCodeMappingListPager.currentPage,
                    sortIndex: $scope.PayorServiceCodeMappingListPager.sortIndex,
                    sortDirection: $scope.PayorServiceCodeMappingListPager.sortDirection
                };

                var jsonData = angular.toJson(pagermodel);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteServiceCodeURLNew, jsonData, "Post", "json", "application/json").success(function (response) {
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

    $scope.SaveServiceCodeDetails = function () {

        if (CheckErrors("#FrmServiceCodeMapping")) {
            //var jsonData = angular.toJson({
            //    addPayorServiceCodeMappingModel: $scope.ServiceCodeModel.PayorServiceCodeMapping,
            //});
            var obj = $scope.ServiceCodeModel.PayorServiceCodeMapping;
            var postmodel = {
                PayorID: obj.PayorID,
                RevenueCode: obj.RevenueCode,
                POSStartDate: obj.POSStartDate,
                POSEndDate: obj.POSEndDate,
                PayorServiceCodeMappingID: obj.PayorServiceCodeMappingID,
                CareType: obj.CareType,
                ServiceCodeID: $scope.ServiceCodeModel.ServiceCodeID,
                IsDeleted: obj.IsDeleted,
                Rate: obj.Rate,
                EncryptedPayorId: obj.EncryptedPayorId,
                UnitType: obj.UnitType,
                PerUnitQuantity: obj.PerUnitQuantity,
                RoundUpUnit: obj.RoundUpUnit,
                MaxUnit: obj.MaxUnit,
                DailyUnitLimit: obj.DailyUnitLimit
            };

            var jsonData = angular.toJson(postmodel);
            AngularAjaxCall($http, HomeCareSiteUrl.AddServiceCodeDetailNew, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.ClearServiceCodeDetails();
                }
            });
        }
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

        //$("#ServiceCodeToken").tokenInput("clear");
        //tempData.ServiceCode = tempData.ServiceCode + ((tempData.ModifierName === null || tempData.ModifierName === '') ? '' : ' - ' + tempData.ModifierName)
        //$("#ServiceCodeToken").tokenInput("add", { ServiceCode: tempData.ServiceCode, CareTypeTitle: tempData.CareType });

        $scope.IsAddVisible = true;
    };


    $scope.ClearServiceCodeDetails = function () {

        var _PayorId = $scope.ServiceCodeModel.PayorServiceCodeMapping.PayorID;

        $scope.ServiceCodeModel.PayorServiceCodeMapping = [];

        $scope.ServiceCodeModel.PayorServiceCodeMapping.PayorID = _PayorId
        $scope.ListMappedServiceCode($scope.ServiceCodeModel.PayorServiceCodeMapping.PayorID);



        //$("#SearchContactToken").tokenInput("clear");

        $scope.IsAddVisible = false;
    };

    /**************************  Ticket##2frxen  *****************************/
    $scope.EncryptedPayorId = 0;
    $scope.PayorServiceCodeMappingList = [];
    $scope.SelectedPayorServiceCodeMappingIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.SearchServiceCodeMappingList = [];
    $scope.TempPayorServiceCodeMappingList = [];
    $scope.GetServiceCodeListURL = HomeCareSiteUrl.GetServiceCodeListURL;
    $("#ServiceCodes_ServiceCode").attr("disable", "disable");
    $scope.PayorServiceCodeMappingListPager = new PagerModule("PayorServiceCodeMappingID");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            payorId: null,
            serviceCode: $scope.ServiceCodeModel.ServiceCode,
            CareType: "-1",
            listOfIdsInCSV: "",
            searchServiceCodeMappingList: $scope.SearchServiceCodeMappingList,
            pageSize: $scope.PayorServiceCodeMappingListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.PayorServiceCodeMappingListPager.sortIndex,
            sortDirection: $scope.PayorServiceCodeMappingListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };


    $scope.ListMappedServiceCode = function (payerId) {

        if (payerId > 0) {
            var jsonData = $scope.SetPostData($scope.PayorServiceCodeMappingListPager.currentPage);
            AngularAjaxCall($http, HomeCareSiteUrl.GetServiceCodeMappingListNew, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.PayorServiceCodeMappingList = response.Data.Items;
                    $scope.PayorServiceCodeMappingListPager.currentPageSize = response.Data.Items.length;
                    $scope.PayorServiceCodeMappingListPager.totalRecords = response.Data.TotalItems;
                }
                ShowMessages(response);
            });

            $scope.IsAddVisible = false;
        }
        else {
            $scope.PayorServiceCodeMappingList = [];
        }
    }

    $scope.SearchModelMapping = function () {
        if ($scope.SearchModifierID) {
            $scope.TempPayorServiceCodeMappingList.ModifierID = $scope.SearchModifierID.toString();
            $scope.SearchModifierID = $scope.TempPayorServiceCodeMappingList.ModifierID.split(",");
        }
        $scope.SearchServiceCodeMappingList = $.parseJSON(angular.toJson($scope.TempPayorServiceCodeMappingList));
    };

    //This will hide the DIV by default.
    $scope.IsAddVisible = false;
    $scope.ShowHideAdd = function () {

        //If DIV is visible it will be hidden and vice versa.
        $scope.IsAddVisible = $scope.IsAddVisible ? false : true;
    }

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
                $scope.PayorServiceCodeMappingListPager.ListOfIdsInCSV = payorServiceCodeMappingId > 0 ? payorServiceCodeMappingId.toString() : $scope.SelectedPayorServiceCodeMappingIds.toString();
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

                var pagermodel = {
                    payorId: $scope.ServiceCodeModel.PayorServiceCodeMapping.PayorID,//$scope.EncryptedPayorId, //"W_6GplXhhdqph21g0NeJqw2", 
                    serviceCode: $scope.ServiceCodeModel.ServiceCode,
                    CareType: "-1",
                    listOfIdsInCSV: $scope.PayorServiceCodeMappingListPager.ListOfIdsInCSV,
                    searchServiceCodeMappingList: $scope.SearchServiceCodeMappingList,
                    pageSize: $scope.PayorServiceCodeMappingListPager.pageSize,
                    pageIndex: $scope.PayorServiceCodeMappingListPager.currentPage,
                    sortIndex: $scope.PayorServiceCodeMappingListPager.sortIndex,
                    sortDirection: $scope.PayorServiceCodeMappingListPager.sortDirection
                };

                var jsonData = angular.toJson(pagermodel);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteServiceCodeURLNew, jsonData, "Post", "json", "application/json").success(function (response) {
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

    $scope.SaveServiceCodeDetails = function () {

        if (CheckErrors("#FrmServiceCodeMapping")) {
            var obj = $scope.ServiceCodeModel.PayorServiceCodeMapping;
            var postmodel = {
                PayorID: obj.PayorID,
                RevenueCode: obj.RevenueCode,
                POSStartDate: obj.POSStartDate,
                POSEndDate: obj.POSEndDate,
                PayorServiceCodeMappingID: obj.PayorServiceCodeMappingID,
                CareType: obj.CareType,
                ServiceCodeID: $scope.ServiceCodeModel.ServiceCodeID,
                IsDeleted: obj.IsDeleted,
                Rate: obj.Rate,
                EncryptedPayorId: obj.EncryptedPayorId,
                UnitType: obj.UnitType,
                PerUnitQuantity: obj.PerUnitQuantity,
                RoundUpUnit: obj.RoundUpUnit,
                MaxUnit: obj.MaxUnit,
                DailyUnitLimit: obj.DailyUnitLimit,
                SelectedPayors: obj.PayorServiceCodeMappingID == undefined ? obj.SelectedPayors : null
            };

            var jsonData = angular.toJson(postmodel);
            AngularAjaxCall($http, HomeCareSiteUrl.AddServiceCodeDetailNew, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {

                    $scope.tempPayorServiceCodeMappingList = null;
                    $scope.SetServiceCodeMapping();

                    $scope.ClearServiceCodeDetails();
                }
            });
        }
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
        $scope.ServiceCodeModel.PayorServiceCodeMapping.EncryptedPayorId = tempData.EncryptedPayorId;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.UnitType = tempData.UnitType;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.PerUnitQuantity = tempData.PerUnitQuantity;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.RoundUpUnit = tempData.RoundUpUnit;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.MaxUnit = tempData.MaxUnit;
        $scope.ServiceCodeModel.PayorServiceCodeMapping.DailyUnitLimit = tempData.DailyUnitLimit;

        $scope.IsAddVisible = true;
    };


    $scope.ClearServiceCodeDetails = function () {

        $scope.ServiceCodeModel.PayorServiceCodeMapping = [];
        $scope.IsAddVisible = false;
    };

    $scope.tempPayorServiceCodeMappingList = null;
    $("a#ServiceCodeMapping").on('shown.bs.tab', function (e) {
        $scope.SetServiceCodeMapping();
    });

    $scope.SetServiceCodeMapping = function () {
        if ($scope.tempPayorServiceCodeMappingList == null) {
            var jsonData = $scope.SetPostData($scope.PayorServiceCodeMappingListPager.currentPage);
            AngularAjaxCall($http, HomeCareSiteUrl.GetServiceCodeMappingListNew, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.PayorServiceCodeMappingList = response.Data.Items;
                    $scope.PayorServiceCodeMappingListPager.currentPageSize = response.Data.Items.length;
                    $scope.PayorServiceCodeMappingListPager.totalRecords = response.Data.TotalItems;

                    $scope.tempPayorServiceCodeMappingList = response.Data.Items;
                }
                ShowMessages(response);

                $scope.dataloaded = false;
            });
        }
    };

};

controllers.AddServiceCodeController.$inject = ['$scope', '$http', '$window'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowAddServiceCodeMessage");
});


