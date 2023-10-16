var custModel;

controllers.CreateLoginController = function ($scope, $http) {
    custModel = $scope;

    var modelJson = $.parseJSON($("#hdnEmployeeModel").val());
    $scope.EmployeeModel = modelJson;

    $scope.ListPreference = $scope.EmployeeModel.PreferenceList;
    $scope.StateList = $scope.EmployeeModel.StateList;
    $scope.SkillList = $scope.EmployeeModel.SkillList;
    $scope.EmployeeSkillList = $scope.EmployeeModel.EmployeeSkillList;
    $scope.CareTypeList = $scope.EmployeeModel.CareTypeList;
    $scope.OrgTypeList = $scope.EmployeeModel.OrgTypeList;

    $scope.SelectedCareType = [];
    $scope.AddEditItemValue = '99999999000011';

    $scope.AddEmployeeLoginURL = "/hc/employee/AddEmployeeLogin/",
    $scope.LoginUrl = "/security/index";
    $scope.CreateLogin = function () {
        var isValid = CheckErrors($("#frmCreateLogin"));
        if (isValid) {
            if ($scope.SelectedCareType) {
                var careTypeValues = $scope.SelectedCareType;
                $scope.EmployeeModel.Employee.CareTypeIds = Object.keys(careTypeValues).map(function (k) { return careTypeValues[k]["Value"] }).join(",");
            }
            else {
                $scope.EmployeeModel.Employee.CareTypeIds = null;
            }

            var jsonData = angular.toJson({
                Employee: $scope.EmployeeModel.Employee,
                PreferenceList: $scope.ListPreference,
                StrEmployeeSkillList: $scope.EmployeeSkillList ? $scope.EmployeeSkillList.toString() : ""
            });
            AngularAjaxCall($http, $scope.AddEmployeeLoginURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        SetMessageForPageLoad(response.Message, "ShowCreateLoginMessage");
                        window.location = $scope.LoginUrl;
                    } else {
                        ShowMessages(response);
                    }
                });
        }
    };

};

controllers.CreateLoginController.$inject = ['$scope', '$http'];