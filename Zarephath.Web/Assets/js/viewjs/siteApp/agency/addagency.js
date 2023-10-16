var custModel;
controllers.AddAgencyController = function ($scope, $http) {
    
    custModel = $scope;
    $scope.AgencyModel = $.parseJSON($("#hdnAgencyModel").val());
    $scope.Save = function () {
        var isValid = CheckErrors($("#frmAgency"));
        if (isValid) {
            var jsonData = angular.toJson({ Agency: $scope.AgencyModel.Agency });
            AngularAjaxCall($http, SiteUrl.AddAgencyURL, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        SetMessageForPageLoad(response.Message, "AgencyUpdateSuccessMessage");
                        window.location = SiteUrl.GetAgencyListPageURL;
                    } else {
                        ShowMessages(response);
                    }
                });
        }
    };

    $scope.Cancel = function () {
        window.location.reload();
    };
};
controllers.AddAgencyController.$inject = ['$scope', '$http'];