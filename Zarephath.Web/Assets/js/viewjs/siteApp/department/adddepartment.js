var vm;


controllers.AddDepartmentController = function($scope, $http, $window) {
    vm = $scope;

    $scope.IsEdit = false;

    $scope.DepartmentModel = $.parseJSON($("#hdnDepartmentModel").val());

    if (($scope.DepartmentModel != null || $scope.DepartmentModel != undefined) && $scope.DepartmentModel.DepartmentID > 0) {
        $scope.IsEdit = true;
    }

    $scope.SaveDepartmentDetails = function() {

        if (CheckErrors("#frmaddDepartmentId")) {
            var jsonData = angular.toJson($scope.DepartmentModel);
            AngularAjaxCall($http, SiteUrl.AddDepartmentURL, jsonData, "Post", "json", "application/json").success(function(response) {
                if (response.IsSuccess) {
                    SetMessageForPageLoad(response.Message, "DepartmentUpdateSuccessMessage");
                    $window.location = SiteUrl.DepartmentListURL;
                    return;
                }
                ShowMessages(response);
            });
        }
    };

    $scope.Cancel = function() {
        $window.location = SiteUrl.DepartmentListURL;
    };

};

controllers.AddDepartmentController.$inject = ['$scope', '$http', '$window'];