var vm;
controllers.OrganizationEsignController = function ($scope, $http, $window) {
    vm = $scope;

    $scope.SetOrganizationEsignModel = $.parseJSON($("#hdnOrganizationEsignModel").val());
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnOrganizationEsignModel").val());
    };
    $scope.OrganizationDetails = $scope.SetOrganizationEsignModel.OrganizationDetails;
    $scope.ServicePlanComponents = $scope.SetOrganizationEsignModel.ServicePlanComponents;
    $scope.TransactionResult = $scope.SetOrganizationEsignModel.TransactionResult;
    if ($scope.TransactionResult.TransactionResultId == -1) {
        $("#frmOrganizationEsign").hide();
        ShowMessage(esignAlreadyProcessed, "error");
    }
    $scope.ServicePlans = $scope.SetOrganizationEsignModel.ServicePlans;
    // add PlanClass propery for each object
    $scope.ServicePlans = $scope.ServicePlans.map(function (plan) {
        if (plan.IsSelected) {
            plan.PlanClass = "pricing-table selected-plan";
        } else {
            plan.PlanClass = "pricing-table";
        }
        return plan;
    });

    $scope.SaveOrganizationEsignDraft = function () {
        $scope.Temp = {};// angular.copy($scope.OrganizationDetails);
        $scope.Temp.OrganizationDetails = $scope.OrganizationDetails;
        $scope.Temp.OrganizationDetails.IsInProcess = true;
        $scope.Temp.ServicePlans = $.grep($scope.ServicePlans, function (plan) {
            return plan.IsSelected == true;
        });
        $scope.SaveEsign();
    };

    $scope.SaveOrganizationEsign = function () {
        if (CheckErrors("#frmOrganizationEsign")) {
            $scope.Temp = {};// angular.copy($scope.OrganizationDetails);
            $scope.Temp.OrganizationDetails = $scope.OrganizationDetails;
            $scope.Temp.OrganizationDetails.IsCompleted = true;
            //$scope.Temp.OrganizationDetails.IsInProcess = true;
            $scope.Temp.ServicePlans = $.grep($scope.ServicePlans, function (plan) {
                return plan.IsSelected == true;
            });
            if ($scope.Temp.ServicePlans.length == 0) {
                ShowMessage(pleaseSelectPlan, "error");
                return;
            } else {
                $scope.SaveEsign();
            }
        }
    };

    $scope.SaveEsign = function () {
        var jsonData = angular.toJson($scope.Temp);
        AngularAjaxCall($http, SiteUrl.SaveOrganizationEsignURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                SetMessageForPageLoad(response.Message, "OrganizationUpdateSuccessMessage");
                $window.location = SiteUrl.OrganizationListURL;
            }
            else {
                ShowMessages(response);
            }
        });
    }

    $scope.Cancel = function () {
        $window.location = SiteUrl.OrganizationListURL;
    };

    $scope.SelectPlan = function (index) {
        if (!$scope.ServicePlans[index].IsSelected) {
            $scope.ServicePlans[index].IsSelected = true;
        } else {
            $scope.ServicePlans[index].IsSelected = false;
        }
    }

    $scope.SelectPlanDiv = function (index) {
        if (!$scope.ServicePlans[index].IsSelected) {
            $scope.ServicePlans[index].PlanClass = "pricing-table selected-plan";
            $scope.ServicePlans[index].IsSelected = true;
        } else {
            $scope.ServicePlans[index].PlanClass = "pricing-table";
            $scope.ServicePlans[index].IsSelected = false;
        }
    }
};
controllers.OrganizationEsignController.$inject = ['$scope', '$http', '$window'];

$(document).ready(function () {
    $(".dateInputMask").attr("placeholder", "MM/DD/YYYY");
});