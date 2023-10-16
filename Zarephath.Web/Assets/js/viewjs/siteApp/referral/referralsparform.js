var sparModel;

controllers.ReferralSparFormController = function ($scope, $http, $window, $timeout) {
    sparModel = $scope;


    $scope.EncryptedReferralID = window.EncryptedReferralID; //"iSNqtcWbe3gZEhtctmlPcA2";

    $scope.SetReferralSparFormModel = $.parseJSON($("#hdnSetReferralSparFormModel").val());

    $scope.ReferralSparForm = $scope.SetReferralSparFormModel.ReferralSparForm;

    
    $scope.SetReferralSparFormNullValues = function (response) {
        if (!ValideElement(response.Data.ReferralSparForm.AssessmentCompletedAndSignedByBHP))
            response.Data.ReferralSparForm.AssessmentCompletedAndSignedByBHP = "-1";
        if (!ValideElement(response.Data.ReferralSparForm.IdentifyDTSDTOBehavior))
            response.Data.ReferralSparForm.IdentifyDTSDTOBehavior = "-1";

        if (!ValideElement(response.Data.ReferralSparForm.ServicePlanCompleted))
            response.Data.ReferralSparForm.ServicePlanCompleted = "-1";
        if (!ValideElement(response.Data.ReferralSparForm.ServicePlanSignedDatedByBHP))
            response.Data.ReferralSparForm.ServicePlanSignedDatedByBHP = "-1";
        if (!ValideElement(response.Data.ReferralSparForm.ServicePlanIdentified))
            response.Data.ReferralSparForm.ServicePlanIdentified = "-1";
        if (!ValideElement(response.Data.ReferralSparForm.ServicePlanHasFrequency))
            response.Data.ReferralSparForm.ServicePlanHasFrequency = "-1";

        
    };


    $scope.SetReferralSparFormDetail = function () {
        if ((window.EncryptedReferralID != null || window.EncryptedReferralID != undefined)) {
            var jsonData = angular.toJson({ EncryptedReferralID: $scope.EncryptedReferralID });
            $scope.ReferralDetail = {};
            AngularAjaxCall($http, SiteUrl.SetReferralSparFormURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                if (response.IsSuccess) {
                    $scope.SetReferralSparFormNullValues(response);
                    $scope.ReferralSparForm = response.Data.ReferralSparForm;
                    $scope.ReferralDetail = response.Data.ReferralForReferralSparFormModel;
                    $scope.PrintContent = false;
                }
                ShowMessages(response);
            });
        }
    };

    $scope.SaveReferralSparForm = function () {
        var jsonData = angular.toJson({ referralSparForm: $scope.ReferralSparForm, EncryptedReferralID: $scope.EncryptedReferralID });
        if (CheckErrors("#frmreferralsparform")) {
            $("#frmReferral").data('changed', false);
            AngularAjaxCall($http, SiteUrl.SaveReferralSparFormURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    // $scope.ReferralSparForm = response.Data;
                    
                    $scope.SetReferralSparFormNullValues(response);
                    $scope.ReferralSparForm = response.Data.ReferralSparForm;
                    $scope.ReferralDetail = response.Data.ReferralForReferralSparFormModel;
                    $scope.PrintContent = false;

                }
            });
        }
    };

    $scope.ResetReferralSparForm = function () {
        var tempData = {
            CreatedDate: $scope.ReferralSparForm.CreatedDate,
            CreatedBy: $scope.ReferralSparForm.CreatedBy,
            UpdatedDate: $scope.ReferralSparForm.CreatedDate,
            UpdatedBy: $scope.ReferralSparForm.UpdatedBy,
            ReviewDate: $scope.ReferralSparForm.ReviewDate,
            AdmissionDate: $scope.ReferralSparForm.AdmissionDate,
            ReferralSparFormID: $scope.ReferralSparForm.ReferralSparFormID,

        };
        $scope.ReferralSparForm = $.parseJSON($("#hdnSetReferralSparFormModel").val()).ReferralSparForm;

        $scope.ReferralSparForm.CreatedDate = tempData.CreatedDate;
        $scope.ReferralSparForm.CreatedBy = tempData.CreatedBy;
        $scope.ReferralSparForm.UpdatedDate = tempData.UpdatedDate;
        $scope.ReferralSparForm.UpdatedBy = tempData.UpdatedBy;
        $scope.ReferralSparForm.AdmissionDate = tempData.AdmissionDate;
        $scope.ReferralSparForm.ReviewDate = tempData.ReviewDate;
        $scope.ReferralSparForm.ReferralSparFormID = tempData.ReferralSparFormID;
        $scope.ReferralSparForm.Date = new Date().toString();
    };

    $("a#sparform").on('shown.bs.tab', function (e) {
        $("#frmReferral").data('changed', false);
        $scope.SetReferralSparFormDetail();
    });

    $scope.$watch(function () {
        return $scope.ReferralSparForm.IsSparFormOffline;
    }, function (newValue) {        
        $scope.ReferralSparForm.IsSparFormCompleted = newValue;
        if (newValue) {
            $(".content-sparform :input:not(.prevent-enable)").attr("disabled", true);
            $(".content-sparform").addClass("form-disable");

        } else {
            $(".content-sparform :input:not(.prevent-enable)").attr("disabled", false);
            $(".content-sparform").removeClass("form-disable");
        }
    });

    $scope.PrintDiv = function (id) {
        myApp.showPleaseWait();
        $scope.PrintContent = true;
        setTimeout(function () {
            printDiv($("#" + id));
            myApp.hidePleaseWait();
            $scope.PrintContent = false;
            $scope.$apply();
        }, 500);
    };
};

controllers.ReferralSparFormController.$inject = ['$scope', '$http', '$window', '$timeout'];

$(document).ready(function () {
    //$(".dateInputMask").inputmask("m/d/y", {
    //    placeholder: "mm/dd/yyyy"
    //});
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});