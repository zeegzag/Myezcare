var vm;
controllers.AddPermissionsController = function ($scope, $http, $window, $timeout) {
    vm = $scope;

    var allPers = $("#hdnPerList").val();
    if (allPers) {
        var allPermissions = JSON.parse(allPers);
        allPermissions = allPermissions.filter(m => m.PermissionName != "");
        $scope.AllPermissions = allPermissions;
    }

    var modelJson = $.parseJSON($("#hdnAddPermissionModel").val());
    $scope.PermissionModel = modelJson;
    setTimeout(function () {
        var parentID = $("#selectedParentID").val();
        $("#ParentID").val(parentID);
    }, 1000)

    $scope.SetPermissionList = function () {
        $scope.SetPermission = [];
        var jsonData = {};
        AngularAjaxCall($http, SiteUrl.SetAddPermissionURL, jsonData, "Get", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.SetPermission = response.Data.Items;
            }
            ShowMessages(response);
        });
    };

    $scope.SavePermission = function () {
        var isValid = CheckErrors($("#frmSavePermission"));
        if (isValid) {
            var jsonData = angular.toJson($scope.PermissionModel);
            AngularAjaxCall($http, SiteUrl.AddPermissionURL, jsonData, "Post", "json", "application/json").
                success(function (response) {
                    if (response.IsSuccess) {
                        //SetMessageForPageLoad(response.Message, "RecordCreatedSuccessfully");
                        ShowMessages(response, 3000);
                        window.location.href = SiteUrl.PermissionList;
                    } else {
                        ShowMessages(response, 3000);
                    }
                });
        }
    };

};
controllers.AddPermissionsController.$inject = ['$scope', '$http', '$window', '$timeout'];