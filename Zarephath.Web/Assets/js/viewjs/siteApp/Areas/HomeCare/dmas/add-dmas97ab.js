var custModel;

controllers.DMASController = function ($scope, $http) {
    custModel = $scope;
    $scope.DmasModel = $.parseJSON($("#hdnDmas97AbModel").val());

    $scope.AddDmas97 = function () {
        debugger
        var isValid = CheckErrors($("#briggsForm"));
        $scope.DmasModel.Dmas97AbModel.Sign = document.getElementById("newSignature").toDataURL("image/png");
        $scope.DmasModel.Dmas97AbModel.RNSign = document.getElementById("newSignature1").toDataURL("image/png");

        if (isValid) {
            var jsonData = angular.toJson({ dmas: $scope.DmasModel });
            
            AngularAjaxCall($http, HomeCareSiteUrl.AddDmas97URL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        toastr.success("DMAS-97A/B Form Save Successfully");
                        window.location.href = HomeCareSiteUrl.Dmas97AbListURL;
                    } else {
                        ShowMessages(response);
                    }
                });
        }
    };

    $scope.GetData = function () {
        debugger
            var jsonData = angular.toJson();
            console.log(jsonData)
        AngularAjaxCall($http, HomeCareSiteUrl.AddDmas97URL1, jsonData, "get", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        $scope.Dmas97AbModel = response.Data;
                    } else {
                        ShowMessages(response);
                    }
                });
        
    }


    $scope.Print = function () {
        debugger
        $http({
            method: 'GET',
            url: 'GenerateDmas97Pdf',
        }).success(function () {
            debugger
            alert("ok");
        });
    }


 


};



controllers.DMASController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    $("#NPI").inputmask({
        mask: "9999999999",
        placeholder: "XXXXXXXXXX"
    });
});