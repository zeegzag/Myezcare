var custModel;

controllers.AddCategoryController = function ($scope, $http, $timeout) {
    custModel = $scope;
    $scope.EBCategoryModel = $.parseJSON($("#hdnCategoryModel").val());
     
    $scope.SaveCategory = function () {
        
        var isValid = CheckErrors($("#frmAddCategory"));
        if (isValid) {
            var jsonData = angular.toJson({ category: $scope.EBCategoryModel.EBCategory });
            AngularAjaxCall($http, HomeCareSiteUrl.AddCategoryURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        SetMessageForPageLoad(response.Message, "ShowCategoryMessage");
                        window.location.href = HomeCareSiteUrl.CategoryListURL;
                    } else {
                        ShowMessages(response);
                    }

                });
        }
    };
};

controllers.AddCategoryController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
   
});