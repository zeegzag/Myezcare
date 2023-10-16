var vm;
controllers.AddOrganizationController = function ($scope, $http, $window) {
    vm = $scope;

    $scope.SetOrganizationModel = $.parseJSON($("#hdnOrganizationModel").val());
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnOrganizationModel").val());
    };
    $scope.MyEzcareOrganization = $scope.SetOrganizationModel.MyEzcareOrganization;
    $scope.TempDBPassword = $scope.MyEzcareOrganization.DBPassword;
    $scope.MyEzcareOrganization.DBPassword='';
    $scope.SaveOrganizationDetails = function () {
        if (CheckErrors("#frmOrganization")) {
            $scope.Temp = angular.copy($scope.MyEzcareOrganization);
            var pass = $scope.Temp.DBPassword;
            $scope.Temp.DBPassword = (pass == '' || pass == null) ? $scope.TempDBPassword : pass;
            var jsonData = angular.toJson($scope.Temp);
            AngularAjaxCall($http, SiteUrl.AddOrganizationURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    SetMessageForPageLoad(response.Message, "OrganizationUpdateSuccessMessage");
                    $window.location = SiteUrl.OrganizationListURL;
                }
                else {
                    ShowMessages(response);
                }
            });
        }
    };
    $scope.Cancel = function () {
        $window.location = SiteUrl.OrganizationListURL;
    };
};
controllers.AddOrganizationController.$inject = ['$scope', '$http', '$window'];

$(document).ready(function () {
    $(".dateInputMask").attr("placeholder", "MM/DD/YYYY");
});