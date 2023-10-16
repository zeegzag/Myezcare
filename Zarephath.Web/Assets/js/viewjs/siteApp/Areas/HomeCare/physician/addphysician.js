var custModel;

controllers.AddPhysicianController = function ($scope, $http, $timeout) {
    custModel = $scope;
    $scope.PhysicianModel = $.parseJSON($("#hdnPhysicianModel").val());

   $scope.SavePhysician = function () {
        var isValid = CheckErrors($("#frmAddPhysician"));
        if (isValid) {
            var jsonData = angular.toJson({ Physician: $scope.PhysicianModel.Physician });
            AngularAjaxCall($http, HomeCareSiteUrl.AddPhysicianURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        if ($scope.PhysicianModel.Physician.PhysicianID == 0 || $scope.PhysicianModel.Physician.PhysicianID == undefined) {
                            toastr.success("Physician Save Successfully");
                           $scope.PhysicianModel = null;
                        }
                        else {
                            toastr.success("Physician Update Successfully");
                        }

                    } else {
                        ShowMessages(response);

                    }

                });
       }
      };
    $scope.Cancel = function () {
        window.location.reload();
    }
    $scope.GetSpecialist = [];
    $scope.SearchSpecialist = function (Search) {
        debugger
        var jsonData = angular.toJson({ searchText: Search, ignoreIds: '', pageSize: 50 });
        AngularAjaxCall($http, HomeCareSiteUrl.GetSpecialistListForAutoCompleteURL, jsonData, "Post", "json", "application/json").success(function (response) {
            $scope.GetSpecialist = response;
            $scope.Search1 = true;
        });
    }
    $scope.SelectSpecialist = function (item) {
        $scope.PhysicianModel.Physician.FirstName = item.FirstName;
        $scope.PhysicianModel.Physician.LastName = item.LastName;
        $scope.PhysicianModel.Physician.Address = item.PracticeAddress;
        $scope.PhysicianModel.Physician.NPINumber = item.NPI;
        $scope.PhysicianModel.Physician.PhysicianTypeName = item.Type;
        if (item.PracticeAddress !="") {
            $scope.AddressSplit(item.PracticeAddress);
        }

        var jsonData = angular.toJson({ Specialist: item.Type, Name: item.Name });
        AngularAjaxCall($http, HomeCareSiteUrl.SaveSpecialistURLs, jsonData, "Post", "json", "application/json", false).success(function (response) {
            console.log(response.Data);
            $scope.PhysicianModel.Physician.PhysicianTypeID = response.Data.PhysicianTypeID;
            ShowMessages(response);
            $scope.PhysicianModel.Physician.PhysicianTypeName = response.Data.Specialist;

        });
        $scope.Search1 = false;
        angular.element('#display').addClass("display");
        angular.element('#glyphicon-remove').addClass("glyphicon-remove");
    }
    $scope.SelectSpecialistForText = function (item) {
        debugger
        var jsonData = angular.toJson({ Specialist: item.Type, Name: item.Name });
        AngularAjaxCall($http, HomeCareSiteUrl.SaveSpecialistURLs, jsonData, "Post", "json", "application/json", false).success(function (response) {
            console.log(response.Data);
            $scope.PhysicianModel.Physician.PhysicianTypeID = response.Data.PhysicianTypeID;
            ShowMessages(response);
            $scope.PhysicianModel.Physician.PhysicianTypeName = response.Data.Specialist;

        });
        $scope.Search1 = false;
        angular.element('#display').addClass("display");
        angular.element('#glyphicon-remove').addClass("glyphicon-remove");
    }
    $scope.SearchSpecialistForText = function (Search) {
        debugger
        var jsonData = angular.toJson({ searchText: Search, ignoreIds: '', pageSize: 50 });
        AngularAjaxCall($http, HomeCareSiteUrl.GetSpecialistListForAutoCompleteURL, jsonData, "Post", "json", "application/json").success(function (response) {
            $scope.GetSpecialistForText = response;
            $scope.Search1 = true;
        });
    }
    $scope.clearText = function () {
        console.log('Clear Text');
        $scope.PhysicianModel.Physician.PhysicianTypeName = null;
        $scope.Search1 = false;
        angular.element('#display').removeClass("display");
    }
    $scope.AddressSplit = function (string) {
        var array = string.split(',');
        $scope.PhysicianModel.Physician.Address = array[0];
        $scope.PhysicianModel.Physician.City = array[1];
        string2 = array[2];
        var array2 = string2.split(' ');
        $scope.PhysicianModel.Physician.AddressNullCode = array2[0];
        $scope.PhysicianModel.Physician.StateCode = array2[1];
        $scope.PhysicianModel.Physician.ZipCode = array2[2];
        
    }
};

controllers.AddPhysicianController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    $("#NPI").inputmask({
        mask: "9999999999",
        placeholder: "XXXXXXXXXX"
    });
});