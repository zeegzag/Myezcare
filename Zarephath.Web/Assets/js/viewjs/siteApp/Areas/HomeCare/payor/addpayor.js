var vm_ap;
var vm_pb;

controllers.AddParyorDetailController = function ($scope, $http, $window) {

    //#region Page Load Time Stuff
    vm_ap = $scope;
    var modalJson = $.parseJSON($("#hdnAddPayorModel").val());
    $scope.PayorModel = modalJson;
    $scope.TempPayorModel = angular.copy($scope.PayorModel);
    $scope.EncryptedPayorID = $scope.PayorModel.Payor.EncryptedPayorID;

    //SetCookie($scope.PayorModel.Payor.PayorID,"PayorID");
    
    $("a#addPayorDetail").on('shown.bs.tab', function (e) {
        $scope.SetAddPayorPage();
    });

    $scope.$on("loadPayorModelData", function (event, args) {
        $scope.PayorModel.Payor = args;
        $scope.EncryptedPayorID = $scope.PayorModel.Payor.EncryptedPayorID;
    });

    $scope.PayorSetting = false;
    if ($scope.PayorModel.PayorEdi837Setting == null && $scope.PayorModel.Payor.PayorID > 0) {
        $scope.PayorSetting = true;
    }
    //#endregion


    //#region Save and Get Payor Details Call
    $scope.SavePayorDetails = function () {
        if (CheckErrors("#frmPayor")) {
            var jsonData = angular.toJson({ addPayorModel: { Payor: $scope.PayorModel.Payor } });
            AngularAjaxCall($http, HomeCareSiteUrl.AddPayorDetailURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    if ($scope.PayorModel.Payor.PayorID == 0 || $scope.PayorModel.Payor.PayorID == undefined) {
                        toastr.success("Payor Save Successfully");
                        $scope.PayorModel = null;
                    }
                    else {
                        toastr.success("Payor Update Successfully");
                    }
                    $scope.SetAddPayorPage();
                }
                else {
                    ShowMessages(response);
                }
                   
            });
        }
    };

    $scope.SetAddPayorPage = function () {
        $scope.ShowEnrollmentAction = true;
        var jsonData = angular.toJson({ id: $scope.EncryptedPayorID });
        AngularAjaxCall($http, HomeCareSiteUrl.SetAddPayorPage, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.PayorModel = response.Data;
                $scope.TempPayorModel = angular.copy($scope.PayorModel);
            }
        });
    };
    //#endregion
    $scope.Cancel = function () {
        window.location.reload();
    }

    $scope.ShowSearchPayorModalClick = function () {
        $scope.SearchPayorModel = {};
        $scope.PayorList = [];
        $("#model__SeachPayor").modal('show');
    };

    
    

    $scope.EnrollmentClick = function (item, type) {

        if (ValideElement(item.EraPayorID)) {
            var model = { PayorID: item.PayorID, EraPayorID: item.EraPayorID, EnrollType: type, ProviderTaxID: item.ProviderTaxID };
            var jsonData = angular.toJson({ model: model });
            AngularAjaxCall($http, HomeCareSiteUrl.PayorEnrollmentURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {

                    if (type == window.Enroll_ERA)
                        $scope.PayorModel.Payor.ERAEnroll_Status = "1";
                    else if (type == window.Enroll_CMS1500)
                        $scope.PayorModel.Payor.CMS1500Enroll_Status = "1";
                }
                else {
                    ShowMessages(response);
                    if (type == window.Enroll_ERA)
                        $scope.PayorModel.Payor.ERAEnroll_Status = "0";
                    else if (type == window.Enroll_CMS1500)
                        $scope.PayorModel.Payor.CMS1500Enroll_Status = "0";
                }
            });
        }
        else {
            bootboxDialog(function () {
            }, bootboxDialogType.Alert, window.Alert, "ERA Payor Id is required.");
            
        }
    };

    $scope.SearchPayorModel = {};
    $scope.SearchPayorClick = function () {
        var jsonData = angular.toJson({ model: $scope.SearchPayorModel });
        AngularAjaxCall($http, HomeCareSiteUrl.SearchPayorPageURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {

                if (response.Data != null) {
                    $scope.PayorList = response.Data;

                    if ($scope.PayorList != undefined && $scope.PayorList.length == undefined) {
                        var arr = [];
                        arr.push($scope.PayorList);
                        $scope.PayorList = arr;
                    }


                    $.each($scope.PayorList, function (index, data) {
                        if (data.payer_alt_names != undefined && data.payer_alt_names.length == undefined) {
                            var arr = [];
                            arr.push(data.payer_alt_names);
                            data.payer_alt_names = arr;
                        }
                    });
                }
                else {
                    $scope.PayorList = [];
                }
            }
            else {
                ShowMessages(response);
            }
        });
    };

    $scope.ShowEnrollmentAction = true;

    $scope.SelectPayorClick = function (item) {
        $scope.PayorModel.Payor.PayorIdentificationNumber = item.payerid;
        $scope.PayorModel.Payor.EraPayorID = item.payerid;
        $scope.PayorModel.Payor.PayorName = item.payer_name;
        $scope.PayorModel.Payor.ShortName = item.payer_name;
        $scope.PayorModel.Payor.StateCode = item.payer_state;
        $scope.ShowEnrollmentAction = false;
        $("#model__SeachPayor").modal('hide');
    };



    $(document).on("click", ".collapseSource", function () {

        var hasClassFaMinusCircle = $(this).hasClass("fa-minus-circle");
        if (hasClassFaMinusCircle == false) {
            $(this).removeClass("fa-plus-circle").addClass("fa-minus-circle");
        }
        else {
            $(this).removeClass("fa-minus-circle").addClass("fa-plus-circle");
        }

    });
};

controllers.AddParyorDetailController.$inject = ['$scope', '$http', '$window'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowAddPayorMessage");
    $("#NPI").inputmask({
        mask: "9999999999",
        placeholder: "XXXXXXXXXX"
    });
});

controllers.PayorBillingController = function ($scope, $http, $window, $timeout) {
    //#region page Load Time Stuff

    vm_pb = $scope;

    $scope.EDI837SettingModel = {};
    $scope.AllBiilingSettingList = [];
    

    $("a#Billing").on('shown.bs.tab', function (e) {
        $scope.GetAllEdi837Setting();
    });

    $("a#GeneralSetting").on('shown.bs.tab', function (e) {
        $scope.ResetSearchFilter();
    });

    $scope.GetAllEdi837Setting = function () {
        $scope.EDI837SettingModel.PayorID = vm_ap.PayorModel.Payor.PayorID;
        var jsonData = angular.toJson($scope.EDI837SettingModel);
        AngularAjaxCall($http,HomeCareSiteUrl.GetAllBillingSettings, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.AllBiilingSettingList = response.Data;
            }
        });
    }

    $scope.ResetSearchFilter = function () {
        $scope.EDI837SettingModel.searchtext = null;
        $scope.GetAllEdi837Setting();
    }
   
    $scope.EditEdi837Setting = function (data, id) {
        data.TempVal = data.Val;
        data.IsEditable = true;
        if (id) {
            $timeout(function () {
                $(id).focus();
            }, 100);
        }
    }

    $scope.SaveEDI837Setting = function (item, id) {
        if (id) {
            $(id).focusout();

            $scope.EDI837SettingModel.PayorID = vm_ap.PayorModel.Payor.PayorID;
            $scope.EDI837SettingModel.Key = item.Key;
            $scope.EDI837SettingModel.Val = item.Val;
            var jsonData = angular.toJson($scope.EDI837SettingModel);

            AngularAjaxCall($http, HomeCareSiteUrl.SaveBillingSetting, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    item.IsEditable = false;
                }
                ShowMessages(response);
            });
        }
    }
    $scope.CancelEDI837Setting = function (data) {
        data.Val = data.TempVal;
        data.IsEditable = false;
    }

    $scope.getLimits = function (array) {
        var item = [];
        var first = Math.floor(array.length / 2) + (array.length % 2 == 0 ? 0 : 1);
        var sec = -Math.floor(array.length / 2);

        if (array.length == 1) {
            item.push(first);
        }
        else if (array.length > 1) {
            item.push(first);
            item.push(sec);
        }
        else {
            return item;
        }
        return item;
    };
    $scope.Cancel = function () {
        window.location.reload();
    }

    //$("a#GeneralSetting01").on('shown.bs.tab', function (e) {
    //    $scope.PayorId = vm_ap.PayorModel.Payor.PayorID;
    //    var jsonData = angular.toJson({ PayorID: $scope.PayorId });
    //    AngularAjaxCall($http, HomeCareSiteUrl.GetPayorBillingSetting, jsonData, "Post", "json", "application/json").success(function (response) {
    //        if (response.IsSuccess) {
    //            $scope.PayorModel.PayorEdi837Setting = response.Data;
    //        }
    //    });
    //});

    //$scope.SaveGeneralSetting = function () {
    //    if (CheckErrors("#frmGeneralSetting")) {
    //        $scope.SaveSettings();
    //    }
    //}

    //$scope.SaveReceiverDetail = function () {
    //    if (CheckErrors("#frmReceiverSetting")) {
    //        $scope.SaveSettings();
    //    }
    //}

    //$scope.SaveCMS1500 = function () {
    //    if (CheckErrors("#frmCMS1500")) {
    //        $scope.SaveSettings();
    //    }
    //}
    //$scope.SaveUB04 = function () {
    //    if (CheckErrors("#frmUB04")) {
    //        $scope.SaveSettings();
    //    }
    //}

    //$scope.SaveSettings = function () {
    //    $scope.PayorModel.PayorEdi837Setting.PayorID=$scope.PayorId;
    //    var jsonData = angular.toJson({ payorEdi837Setting: $scope.PayorModel.PayorEdi837Setting });
    //    AngularAjaxCall($http, HomeCareSiteUrl.SavePayorBillingSettingURL, jsonData, "Post", "json", "application/json").success(function (response) {
    //        if (response.IsSuccess) {
    //            ShowMessages(response);
    //        }
    //        else
    //            ShowMessages(response);
    //    });
    //}
};
controllers.PayorBillingController.$inject = ['$scope', '$http', '$window', '$timeout'];