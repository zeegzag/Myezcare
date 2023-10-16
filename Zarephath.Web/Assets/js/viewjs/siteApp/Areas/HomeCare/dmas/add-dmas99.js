var custModel;

controllers.DMAS99Controller = function ($scope, $http) {
    custModel = $scope;
    $scope.Dmas99Model = $.parseJSON($("#hdnDmas99Model").val());
    $scope.AddDmas99 = function () {
        var isValid = CheckErrors($("#briggsForm"));
        $scope.Dmas99Model.Sign = document.getElementById("newSignature").toDataURL("image/png");
        $scope.Dmas99Model.RnSign = document.getElementById("newSignature1").toDataURL("image/png");

        if (isValid) {
            var jsonData = angular.toJson({ dmas: $scope.Dmas99Model });
            AngularAjaxCall($http, HomeCareSiteUrl.AddDmas99URL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        toastr.success("DMAS-99 Form Save Successfully");
                        $scope.Dmas99Model.Dmas99 = null;
                        window.location.href = HomeCareSiteUrl.Dmas99ListURL;
                    } else {
                        ShowMessages(response);
                    }
                });
        }
    };


    $scope.Print = function () {
        $http({
            method: 'GET',
            url: 'GenerateDmas97Pdf',
        }).success(function () {
            alert("ok");
        });
    }



};

controllers.DMAS99Controller.$inject = ['$scope', '$http'];

