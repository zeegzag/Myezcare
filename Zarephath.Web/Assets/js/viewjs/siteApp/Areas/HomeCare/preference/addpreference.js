var custModel;

controllers.AddPreferenceController = function ($scope, $http, $timeout, $window) {
    custModel = $scope;

    $scope.model = $.parseJSON($("#hdnPreferenceModel").val());
    $scope.Preference = $scope.model.Preference;

    $scope.IsEditMode = $scope.Preference.PreferenceID > 0;

    $scope.Save = function () {
        debugger
        var isValid = CheckErrors($("#frmPreference"));
        if (isValid) {
            var jsonData = angular.toJson({ Preference: $scope.Preference });
            AngularAjaxCall($http, HomeCareSiteUrl.AddPreferenceURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        if ($scope.Preference.PreferenceID == 0 || $scope.Preference.PreferenceID == undefined) {
                            toastr.success("Preference Save Successfully");
                            $scope.Preference = null;
                        }
                        else {
                            toastr.success("Preference Update Successfully");
                        }
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

controllers.AddPreferenceController.$inject = ['$scope', '$http', '$timeout', '$window'];

$(document).ready(function () {
    //$(".dateInputMask").inputmask("m/d/y", {
    //    placeholder: "mm/dd/yyyy"
    //});
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});