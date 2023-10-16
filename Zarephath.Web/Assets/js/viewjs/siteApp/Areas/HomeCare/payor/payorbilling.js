var vm;
controllers.PayorBillingController = function ($scope, $http, $window, $timeout) {
    //#region page Load Time Stuff

    vm = $scope;
    var modalJson = $.parseJSON($("#hdnAddPayorModel").val());
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnAddPayorModel").val());
    };
    $scope.ServiceCodeModel = modalJson;


    
    
    $("a#Billing").on('shown.bs.tab', function (e) {
        $scope.PayorId = GetCookie("PayorID");

        var jsonData = angular.toJson({ GetPayorBillingSettings: $scope.PayorId });
        debugger;
        AngularAjaxCall($http, HomeCareSiteUrl.GetPayorBillingSetting, jsonData, "Get", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.PayorServiceCodeMappingList = response.Data.Items;
                $scope.PayorServiceCodeMappingListPager.currentPageSize = response.Data.Items.length;
                $scope.PayorServiceCodeMappingListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    });
    
   
};
controllers.PayorBillingController.$inject = ['$scope', '$http', '$window', '$timeout'];