var custModel;

controllers.AddDxCodeController = function ($scope, $http, $timeout, $window) {
    custModel = $scope;
    $scope.DxCodeModel = $.parseJSON($("#hdnDxCodeModel").val());

    $scope.IsEditMode = ($scope.DxCodeModel.DxCode != null) ? ($scope.DxCodeModel.DxCode.DXCodeID >0) : false;

    $scope.Save = function () {
        var isValid = CheckErrors($("#frmDxCode"));
        if (isValid) {
            var jsonData = angular.toJson({ DxCode: $scope.DxCodeModel.DxCode });

            AngularAjaxCall($http, HomeCareSiteUrl.AddDxCodeURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {

                    if (response.IsSuccess) {
                     //   SetMessageForPageLoad(response.Message, "ShowDxCodeMessage");
                        // window.location = HomeCareSiteUrl.DxCodeListURL;
                        toastr.success("DxCode Save Successfully");
                    }
                    else {
                        ShowMessages(response);
                    }
                });
        }
    };

    $scope.Cancel = function () {
        window.location.reload();
    };
};

controllers.AddDxCodeController.$inject = ['$scope', '$http', '$timeout', '$window'];

$(document).ready(function () {
    //$(".dateInputMask").inputmask("m/d/y", {
    //    placeholder: "mm/dd/yyyy"
    //});
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});