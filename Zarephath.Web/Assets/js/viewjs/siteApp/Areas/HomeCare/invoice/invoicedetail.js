var model;
controllers.InvoiceDetailController = function ($scope, $http, $window, $timeout, $filter) {
    //#region REFERRAL RELATED GLOBAL FUNCATION
	model = $scope;

    $scope.parseInt = $window.parseInt;
    $scope.isPayor = $("#hdBillToPayor").val();//1=true
    $scope.InvoiceAddressIsIncludePatientAddress = ($("#hdInvoiceAddressIsIncludePatientAddress").val() == '1' ? true : false);
    $scope.InvoiceAddressIsIncludePatientAddressLine1 = ($("#hdInvoiceAddressIsIncludePatientAddressLine1").val() == '1' ? true : false);
    $scope.InvoiceAddressIsIncludePatientAddressLine2 = ($("#hdInvoiceAddressIsIncludePatientAddressLine2").val() == '1' ? true : false);
    $scope.InvoiceAddressIsIncludePatientAddressZip = ($("#hdInvoiceAddressIsIncludePatientAddressZip").val() == '1' ? true : false);
    $scope.InvoiceIsIncludePatientDOB = ($("#hdInvoiceIsIncludePatientDOB").val() == '1' ? true : false);

    $scope.payorId = null;

    $scope.InvoiceDetail = $.parseJSON($("#hdInvoiceDetail").val());
    if ('' + $scope.isPayor == '2') {
        try {
            $scope.payorId = $scope.InvoiceDetail.Payors[0].EncryptedPayorID;
        }
        catch (err) {

        }
    }
    $scope.UpdatePaymentDetail = function (paymentType) {
        var payInvoiceAmountDetail = {
            PaymentType: paymentType,
            InvoiceId: $scope.InvoiceDetail.InvoiceDetailModel.ReferralInvoiceID,
            ReferralId: $scope.InvoiceDetail.InvoiceDetailModel.ReferralID,
            Amount: 0
        };
        var jsonData = angular.toJson({ payInvoiceAmountDetail: payInvoiceAmountDetail });
        AngularAjaxCall($http, HomeCareSiteUrl.PayInvoiceAmountUrl, jsonData, "Post", "json", "application/json").success(function (response) {
            $scope.InvoiceDetail.InvoiceDetailModel = response.Data.InvoiceDetailModel;
            $scope.InvoiceDetail.InvoiceTransactionDetailModelList = response.Data.InvoiceTransactionDetailModelList;
            $scope.InvoiceDetail.InvoiceTransactionEmployeeScheduleModelList = response.Data.InvoiceTransactionEmployeeScheduleModelList;
            $scope.InvoiceDetail.ReferralPaymentHistoriesDetailList = response.Data.ReferralPaymentHistoriesDetailList;
            if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                $scope.$apply();
            }
            ShowMessages(response);
        });
    }

    $scope.OpenPartialPaymentPopup = function () {
        $scope.PartialPaymentAmount = null;
        $('#UpdateReferralInvoiceDetailPopup').modal({
            backdrop: 'static',
            keyboard: false
        });
    }

    $scope.UpdatePartialPaymentDetail = function (paymentType) {
        var isValid = CheckErrors($("#UpdateReferralInvoiceDetailfrm"));
        if (isValid) {
            if ($scope.PartialPaymentAmount <= 0) {
                ShowMessage(window.EnterAmountGreaterThanZero, "error");
                return;
            }
            var extraAmount = $scope.PartialPaymentAmount - $scope.InvoiceDetail.InvoiceDetailModel.TotalPayableAmount;
            if (extraAmount > 0) {
                ShowMessage(window.PartialPaymentLessThanEqualPayableAmount, "error");
                return;
            }
            var payInvoiceAmountDetail = {
                PaymentType: paymentType,
                InvoiceId: $scope.InvoiceDetail.InvoiceDetailModel.ReferralInvoiceID,
                ReferralId: $scope.InvoiceDetail.InvoiceDetailModel.ReferralID,
                Amount: $scope.PartialPaymentAmount
            };
            var jsonData = angular.toJson({ payInvoiceAmountDetail: payInvoiceAmountDetail });
            AngularAjaxCall($http, HomeCareSiteUrl.PayInvoiceAmountUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                $scope.InvoiceDetail.InvoiceDetailModel = response.Data.InvoiceDetailModel;
                $scope.InvoiceDetail.InvoiceTransactionDetailModelList = response.Data.InvoiceTransactionDetailModelList;
                $scope.InvoiceDetail.InvoiceTransactionEmployeeScheduleModelList = response.Data.InvoiceTransactionEmployeeScheduleModelList;
                $scope.InvoiceDetail.ReferralPaymentHistoriesDetailList = response.Data.ReferralPaymentHistoriesDetailList;
                if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                    $scope.$apply();
                }
                ShowMessages(response);
                $('#UpdateReferralInvoiceDetailPopup').modal('hide');
            });
        }
    }
    $scope.onInvoiceAddressIsIncludePatientAddressChecked = function () {
        $scope.InvoiceAddressIsIncludePatientAddress = !($scope.InvoiceAddressIsIncludePatientAddress || false);
        if ($scope.InvoiceAddressIsIncludePatientAddress == true) {
            $scope.InvoiceAddressIsIncludePatientAddressLine1 = true;
            $scope.InvoiceAddressIsIncludePatientAddressLine2 = true;
            $scope.InvoiceAddressIsIncludePatientAddressZip = true;
            $scope.InvoiceIsIncludePatientDOB = true;
        }
        else {
            $scope.InvoiceAddressIsIncludePatientAddressLine1 = false;
            $scope.InvoiceAddressIsIncludePatientAddressLine2 = false;
            $scope.InvoiceAddressIsIncludePatientAddressZip = false;
            $scope.InvoiceIsIncludePatientDOB = false;
        }
    }
    $scope.onChecked = function (chk) {
        $scope[chk] = !$scope[chk];
    }
};
controllers.InvoiceDetailController.$inject = ['$scope', '$http', '$window', '$timeout'];