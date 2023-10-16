var vm;
controllers.AddVehicleController = function ($scope, $http, $window) {
    vm = $scope;

    $scope.SetVehicleModel = $.parseJSON($("#hdnVehicleModel").val());
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnVehicleModel").val());
    };
    $scope.VehicleModel = $scope.SetVehicleModel.VehicleModel;
    $scope.SetTransportContactModel = $.parseJSON($("#hdnTransportContactModel", parent.document).val());
    if ($scope.SetTransportContactModel != undefined) {
        $scope.VehicleModel.ContactID = $scope.SetTransportContactModel.TransportContactModel.ContactID;
    }
    $scope.SaveVehicleDetails = function () {
        var isValid = CheckErrors($("#frmaddVehicle"));
        if (isValid) {
            var jsonData = angular.toJson({ vehicleModel: $scope.SetVehicleModel });

            AngularAjaxCall($http, HomeCareSiteUrl.AddVehicleURL, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        if ($scope.VehicleModel.VehicleID == 0 || $scope.VehicleModel.VehicleID == undefined) {
                            toastr.success("Vehicle Save Successfully");
                            $scope.Reset();
                        } else {
                            toastr.success("Vehicle Update Successfully");
                        }

                    } else {
                        ShowMessages(response);
                    }
                });
        }
    };

    $scope.Reset = function () {
        $scope.VehicleModel = '';
    };

    $scope.Cancel = function () {
        window.location.reload();
    };

};
controllers.AddVehicleController.$inject = ['$scope', '$http', '$window'];

$(document).ready(function () {


});