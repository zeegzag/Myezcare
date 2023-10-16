var vm_careform;
controllers.MIFFormController = function ($scope, $http, $window, $timeout, $filter) {
    
    var modalJson = $.parseJSON($("#hdnSetAddReferralModel").val());

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetAddReferralModel").val());
    };
    vm_careform = $scope;

    $scope.EncryptedReferralID = window.EncryptedReferralID;
    $scope.ReferralModel = modalJson;

    $scope.OpenMIFForm = function () {
        AngularAjaxCall($http, HomeCareSiteUrl.SetMIFFormURL, { referralID: $scope.ReferralModel.Referral.ReferralID }, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.MIFDetail = response.Data.MIFDetail;
                
                $scope.MIFPriorAuthorization = response.Data.PriorAuthorizationDetail;

                if ($scope.MIFPriorAuthorization.StartDate != null)
                    $scope.MIFDetail.PriorAuthorizationDateFrom = moment($scope.MIFPriorAuthorization.StartDate).format('MM/DD/YYYY');
                if ($scope.MIFPriorAuthorization.EndDate != null)
                    $scope.MIFDetail.PriorAuthorizationDateTo = moment($scope.MIFPriorAuthorization.EndDate).format('MM/DD/YYYY');
                $scope.MIFDetail.PriorAuthorizationNo = $scope.MIFPriorAuthorization.AuthorizationCode;
                $("#model_MIFForm").modal({ backdrop: 'static' });
            }
            ShowMessages(response);
        });
    }

    $('#model_MIFForm').on('hidden.bs.modal', function () {
        $scope.MIFDetail = {};
    })

    $scope.SaveMIFDetail = function (SaveAndPrint = false) {
        $scope.MIFDetail.ReferralID = $scope.ReferralModel.Referral.ReferralID;
        AngularAjaxCall($http, HomeCareSiteUrl.SaveMIFDetailURL, { model: $scope.MIFDetail }, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                if (SaveAndPrint) {
                    window.location = HomeCareSiteUrl.GenerateMIFPdfURL + response.Data.EncryptedMIFFormID;
                }
                $scope.GetReferralMIFForms();
                $("#model_MIFForm").modal('hide');
            }
            ShowMessages(response);
        });
    }

    $scope.OpenClientSignModel = function () {
        $('#SignatureModel').modal({
            backdrop: 'static',
            keyboard: false
        });
    };
    $scope.CloseClientSignModel = function () {
        $('#SignatureModel').modal('hide');
        $scope.ClearSignatureImage();
    };

    $scope.signModel = {};

    $scope.SaveSignatureImage = function () {
        $scope.signModel = $scope.accept(); // default method for get signatureobject
        if ($scope.signModel.isEmpty == true || ($scope.signModel.isEmpty == false && $scope.signModel.dataUrl == undefined)) {
            ShowMessage("Please provide a signature first", "error");
            return;
        }
        else {
            $scope.MIFDetail.SignaturePath = $scope.signModel.dataUrl;
            var jsonData = angular.toJson($scope.MIFDetail);
            AngularAjaxCall($http, HomeCareSiteUrl.SaveMIFSignURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.ClearSignatureImage();
                    $scope.MIFDetail.TempSignaturePath = response.Data;
                    $('#SignatureModel').modal('hide');
                }
                ShowMessages(response);
            });
        }
    };

    $scope.ClearSignatureImage = function () {
        $scope.clear(); // default method for cleare signature board
    };

    $scope.GetReferralMIFForms = function () {
        var jsonData = angular.toJson({ EncryptedReferralID: $scope.EncryptedReferralID});
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralMIFFormsURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.MIFFormList = response.Data;
                
            }
        });
    };

    $("a#referralMIF").on('shown.bs.tab', function (e) {
        $scope.GetReferralMIFForms();
    });
};
controllers.MIFFormController .$inject = ['$scope', '$http', '$window', '$timeout', '$filter'];
$(document).ready(function () {
    $(".dateInputMask").attr("placeholder", "mm/dd/yyyy");
});