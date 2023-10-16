var custNotificationPrefsModel;

controllers.EmployeeNotificationPrefsController = function ($scope, $http, $compile, $timeout) {
    custNotificationPrefsModel = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnEmpNotificationPrefsModel").val());
    };
    $scope.EmployeeModel = $.parseJSON($("#hdnEmployeeModel").val());
    $scope.EmployeeNotificationPrefsModel = $scope.newInstance();

    $scope.GetNotificationPrefs = function () {
        var jsonData = angular.toJson({ EmployeeID: $scope.EmployeeModel.Employee.EmployeeID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeNotificationPrefsURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.EmployeeNotificationPrefsModel = response.Data;
                $scope.EmployeeNotificationPrefsModel.EmployeeID = $scope.EmployeeModel.Employee.EmployeeID;
            }
            ShowMessages(response);
        });
    }

    $scope.SaveNotificationPrefs = function () {
        var isValid = CheckErrors($("#frmEmployeeNotificationPrefs"));
        if (isValid) {
            var jsonData = angular.toJson({ model: $scope.EmployeeNotificationPrefsModel });
            AngularAjaxCall($http, HomeCareSiteUrl.SaveEmployeeNotificationPrefsURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.EmployeeNotificationPrefsModel = response.Data;
                }
                ShowMessages(response);
            });
        }
    }

    $("a#employeeNotificationPrefs").on('shown.bs.tab', function (e, ui) {
        $scope.GetNotificationPrefs();
    });
};

controllers.EmployeeNotificationPrefsController.$inject = ['$scope', '$http', '$compile', '$timeout'];


