var custChecklistModel;

controllers.EmployeeChecklistController = function ($scope, $http, $compile, $timeout) {
    custChecklistModel = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnEmpChecklistModel").val());
    };
    $scope.EmployeeModel = $.parseJSON($("#hdnEmployeeModel").val());
    $scope.EmployeeChecklistModel = $scope.newInstance();

    $scope.GetEmployeeChecklist = function () {
        var jsonData = angular.toJson({ EmployeeID: $scope.EmployeeModel.Employee.EmployeeID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeChecklistURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.EmployeeChecklistModel = response.Data;
                $scope.EmployeeChecklistModel.EmployeeID = $scope.EmployeeModel.Employee.EmployeeID;
            }
            ShowMessages(response);
        });
    }

    $scope.CheckboxChange = function (itemType, itemName) {
        if (itemType == "Identification") {
            if (itemName != "DriverLicense") {
                $scope.EmployeeChecklistModel.DriverLicense = false;
            }
            if (itemName != "StateID") {
                $scope.EmployeeChecklistModel.StateID = false;
            }
            if (itemName != "AlienCard") {
                $scope.EmployeeChecklistModel.AlienCard = false;
            }
            if (itemName != "Passport") {
                $scope.EmployeeChecklistModel.Passport = false;
            }
        } else if (itemType == "Competency") {
            if (itemName != "RN") {
                $scope.EmployeeChecklistModel.RN = false;
            }
            if (itemName != "LPN") {
                $scope.EmployeeChecklistModel.LPN = false;
            }
            if (itemName != "LSW") {
                $scope.EmployeeChecklistModel.LSW = false;
            }
            if (itemName != "CNA") {
                $scope.EmployeeChecklistModel.CNA = false;
            }
            if (itemName != "Other") {
                $scope.EmployeeChecklistModel.Other = false;
                $scope.EmployeeChecklistModel.OtherText = '';
            }
        } else if (itemType == "Agreement") {
            if (itemName != "IndependentAgreement") {
                $scope.EmployeeChecklistModel.IndependentAgreement = false;
            }
            if (itemName != "EmployeeAgreement") {
                $scope.EmployeeChecklistModel.EmployeeAgreement = false;
            }
        } else if (itemType == "EmployeeForm") {
            if (itemName != "EmployeeW4Form") {
                $scope.EmployeeChecklistModel.EmployeeW4Form = false;
            }
            if (itemName != "ISCW9Form") {
                $scope.EmployeeChecklistModel.ISCW9Form = false;
            }
        }
    }

    $scope.SaveEmployeeChecklist = function () {
        var isValid = CheckErrors($("#frmEmployeeChecklist"));
        if (isValid) {
            var jsonData = angular.toJson({ model: $scope.EmployeeChecklistModel });
            AngularAjaxCall($http, HomeCareSiteUrl.SaveEmployeeChecklistURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.EmployeeChecklistModel = response.Data;
                }
                ShowMessages(response);
            });
        }
    }

    $scope.callBack = function (item) {
        if ($('#date-IDate').data("DateTimePicker")) {
            $('#date-IDate').data("DateTimePicker").widgetPositioning({
                horizontal: "right",
                vertical: "bottom"
            });
            $('#date-ICDate').data("DateTimePicker").widgetPositioning({
                horizontal: "right",
                vertical: "bottom"
            });
            $('#date-SSCDate').data("DateTimePicker").widgetPositioning({
                horizontal: "right",
                vertical: "bottom"
            });

            $('#date-SSCCDate').data("DateTimePicker").widgetPositioning({
                horizontal: "right",
                vertical: "bottom"
            });
            $('#date-CDate').data("DateTimePicker").widgetPositioning({
                horizontal: "right",
                vertical: "bottom"
            });
            $('#date-CCDate').data("DateTimePicker").widgetPositioning({
                horizontal: "right",
                vertical: "bottom"
            });
            $('#date-ECCDate').data("DateTimePicker").widgetPositioning({
                horizontal: "right",
                vertical: "bottom"
            });
            $('#date-ECCCompletionDate').data("DateTimePicker").widgetPositioning({
                horizontal: "right",
                vertical: "bottom"
            });
        }
        
    };


    $("a#employeeChecklist").on('shown.bs.tab', function (e, ui) {
        $scope.GetEmployeeChecklist();
    });
};

controllers.EmployeeChecklistController.$inject = ['$scope', '$http', '$compile', '$timeout'];



