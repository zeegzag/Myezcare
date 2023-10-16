var custModel;

controllers.PreferenceController = function ($scope, $http, $timeout) {
    custModel = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnOrgPreferenceModel").val());
    };


    $scope.OrganizationPreferenceURL = HomeCareSiteUrl.OrganizationPreferenceURL;
    $scope.OrgPreferenceModel = $.parseJSON($("#hdnOrgPreferenceModel").val());
    //$scope.LanguageList = [];
    //$scope.TimeZones = [];

    $scope.OrgPreferenceModel.OrganizationPreference.Language = $scope.OrgPreferenceModel.OrganizationPreference.Language;
    
    $scope.SavePreference = function () {
        var isValid = CheckErrors($("#frmPreference"));
        if (isValid) {
            var jsonData = angular.toJson({ Preference: $scope.OrgPreferenceModel.OrganizationPreference });
            console.log(jsonData);

            AngularAjaxCall($http, HomeCareSiteUrl.OrganizationPreferenceSaveURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        if ($scope.OrgPreferenceModel.OrganizationPreference.OrganizationID == 0 || $scope.OrgPreferenceModel.OrganizationPreference.OrganizationID == undefined) {
                            toastr.success("Preference Save Successfully");
                            $scope.OrgPreferenceModel = null;
                        }
                        else {
                            toastr.success("Preference Update Successfully");
                        }

                    } else {
                        ShowMessages(response);
                    }
                });
        }
    };

    $scope.Cancel = function () {
        window.location.reload();
    }
};

controllers.PreferenceController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    //$("#NPI").inputmask({
    //    mask: "9999999999",
    //    placeholder: "XXXXXXXXXX"
    //});
});