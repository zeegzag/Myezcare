var custModel;

controllers.AddParentController = function ($scope, $http, $timeout, $window) {
    custModel = $scope;

    $scope.ParentModel = $.parseJSON($("#hdnParentModel").val());


    $scope.Save = function () {
        
        var isValid = CheckErrors($("#frmParent"));
        if (isValid) {
            var jsonData = angular.toJson({ model: $scope.ParentModel.Contact });

            AngularAjaxCall($http, SiteUrl.AddParentURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {

                    if (response.IsSuccess) {
                        SetMessageForPageLoad(response.Message, "ShowParentMessage");
                        window.location = SiteUrl.ParentListURL;
                    }
                    else {
                        ShowMessages(response);
                    }
                });
        }
    };

    $scope.Cancel = function () {
        $window.location = SiteUrl.ParentListURL;
    };
};

controllers.AddParentController.$inject = ['$scope', '$http', '$timeout', '$window'];