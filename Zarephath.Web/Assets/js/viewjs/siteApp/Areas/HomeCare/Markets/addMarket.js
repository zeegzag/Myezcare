var custModel;

controllers.AddEBMarketsController = function ($scope, $http, $timeout) {
    custModel = $scope;
    $scope.EBMarketsModel = $.parseJSON($("#hdnEBMarketsModel").val());
     
    $scope.SaveMarkets = function () {
       
        var isValid = CheckErrors($("#frmAddMarket"));
        if (isValid) {
            var jsonData = angular.toJson({ markets: $scope.EBMarketsModel.EBMarkets });
            AngularAjaxCall($http, HomeCareSiteUrl.AddMarketURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        SetMessageForPageLoad(response.Message, "ShowCategoryMessage");
                        window.location.href = HomeCareSiteUrl.MarketListURL;
                    } else {
                        ShowMessages(response);
                    }

                });
        }
    };
};

controllers.AddEBMarketsController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
   
});