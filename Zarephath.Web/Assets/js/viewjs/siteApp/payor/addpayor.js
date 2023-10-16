var vm;

controllers.AddParyorDetailController = function ($scope, $http, $window) {

    //#region Page Load Time Stuff
    vm = $scope;
    var modalJson = $.parseJSON($("#hdnAddPayorModel").val());
    $scope.PayorModel = modalJson;
    $scope.TempPayorModel = angular.copy($scope.PayorModel);
    $scope.EncryptedPayorID = $scope.PayorModel.Payor.EncryptedPayorID;

    $("a#addPayorDetail").on('shown.bs.tab', function (e) {
        $scope.SetAddPayorPage();
    });

    $scope.PayorSetting = false;
    if ($scope.PayorModel.PayorEdi837Setting == null && $scope.PayorModel.Payor.PayorID > 0) {
        $scope.PayorSetting = true;
    }
    //#endregion


    //#region Save and Get Payor Details Call
    $scope.SavePayorDetails = function () {
        if (CheckErrors("#frmPayor")) {
            var jsonData = angular.toJson({ addPayorModel: { Payor: $scope.PayorModel.Payor } });
            AngularAjaxCall($http, SiteUrl.AddPayorDetailURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    SetMessageForPageLoad(response.Message, "ShowAddPayorMessage");
                    $scope.PayorModel.PayorID = response.Data;
                    $window.location = SiteUrl.SetAddPayorPageURL + response.Data;
                }
                else
                    ShowMessages(response);
            });
        }
    };

    $scope.SetAddPayorPage = function () {
        var jsonData = angular.toJson({ id: $scope.EncryptedPayorID });
        AngularAjaxCall($http, SiteUrl.SetAddPayorPage, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.PayorModel = response.Data;
            }
        });
    };
    //#endregion

};

controllers.AddParyorDetailController.$inject = ['$scope', '$http', '$window'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowAddPayorMessage");
});


