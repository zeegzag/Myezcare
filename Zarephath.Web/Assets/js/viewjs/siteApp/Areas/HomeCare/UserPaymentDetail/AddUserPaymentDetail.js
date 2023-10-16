var custModel;


controllers.AddUserPaymentDetailController = function ($scope, $http) {
    custModel = $scope;
    $scope.UserPaymentDetailModel = $.parseJSON($("#hdnUserPaymentDetailModel").val());

    $scope.Save = function () {
        debugger;
        var isValid = CheckErrors($("#frmAddUserPaymentDetail"));
        // var isValidCT = $scope.validationCareType();
        if (!isValid) { //|| !isValidCT) {
            toastr.error("Cannot save - required fields are incomplete.");
            return false;
        }

        if (isValid) {
            var jsonData = angular.toJson({
                piUserPaymentDetailModel: $scope.UserPaymentDetailModel
            });
            //HomeCareSiteUrl.AddBillingPaymentDetailURL
            AngularAjaxCall($http, HomeCareSiteUrl.AddBillingPaymentDetailURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    //ShowMessages(response);
                    if (response.IsSuccess) {
                        toastr.success("Payment Done successfully");
                        //window.location.reload();
                        window.opener.location.reload();
                        window.close();

                        //
                    } else {
                        ShowMessages(response);
                    }

                });
        }


        $scope.alert = function () {
            alert();
        }

    };
};
controllers.AddUserPaymentDetailController.$inject = ['$scope', '$http'];

document.getElementById("Save").disabled = true;


$('#TermsNConditions').change(
    function (e) {
        var checked = $(this).attr('checked');
        if (checked) {
            $('#Save').removeAttr('disabled');
        }

        else {
            $('#Save').attr('disabled', true);
        }
    }
);

