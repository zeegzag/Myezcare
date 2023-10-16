var vm;

controllers.AddServiceCodeController = function ($scope, $http, $window) {

    //#region Page Load Time Stuff
    vm = $scope;
    var modalJson = $.parseJSON($("#hdnAddServiceCodeModel").val());
    $scope.ServiceCodeModel = modalJson.ServiceCodes;
    $scope.TempServiceCodeModel = angular.copy($scope.ServiceCodeModel);
    $scope.EncryptedServiceCodeID = $scope.ServiceCodeModel.EncryptedServiceCodeID;

    //#endregion


    //#region Save and Get ServiceCode Details
    $scope.Save = function () {
        if (CheckErrors("#frmServiceCode")) {
            var jsonData = angular.toJson({ addServiceCodeModel: { ServiceCodes: $scope.ServiceCodeModel } });
            AngularAjaxCall($http, SiteUrl.SetAddServiceCodePageURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    SetMessageForPageLoad(response.Message, "ShowAddServiceCodeMessage");
                    $window.location = SiteUrl.SetServiceCodeList;
                }
                else
                    ShowMessages(response);
            });
        }
    };



    //

    $scope.$watch(function () { return $scope.ServiceCodeModel.UnitType; }, function (newValue, oldValue) {

        if (!ValideElement(newValue)) $scope.SelectedUnit = "";
        if (newValue === parseInt(window.UnitTime)) $scope.SelectedUnit = window.UnitTimeValue;
        if (newValue === parseInt(window.UnitMiles)) $scope.SelectedUnit = window.UnitMilesValue;
        if (newValue === parseInt(window.UnitStop)) $scope.SelectedUnit = window.UnitStopValue;
        if (!$scope.$root.$$phase) $scope.$apply();

    }, true);


    $scope.$watch(function () { return $scope.ServiceCodeModel.DefaultUnitIgnoreCalculation; }, function (newValue, oldValue) {
        if (!ValideElement(newValue)) $scope.ServiceCodeModel.DefaultUnitIgnoreCalculation = 0;
        if (!$scope.$root.$$phase) $scope.$apply();
    });

    $scope.$watch(function () { return $scope.ServiceCodeModel.MaxUnit; }, function (newValue, oldValue) {
        if (!ValideElement(newValue)) $scope.ServiceCodeModel.MaxUnit = 0;
        if (!$scope.$root.$$phase) $scope.$apply();
    });

    $scope.$watch(function () { return $scope.ServiceCodeModel.DailyUnitLimit; }, function (newValue, oldValue) {
        if (!ValideElement(newValue)) $scope.ServiceCodeModel.DailyUnitLimit = 0;
        if (!$scope.$root.$$phase) $scope.$apply();
    });

    //#endregion

};

controllers.AddServiceCodeController.$inject = ['$scope', '$http', '$window'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowAddServiceCodeMessage");
});


