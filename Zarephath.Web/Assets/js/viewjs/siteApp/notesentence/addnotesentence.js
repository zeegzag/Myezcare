var custModel;

controllers.AddNoteSentenceController = function ($scope, $http, $timeout, $window) {
    custModel = $scope;

    $scope.NoteSentenceModel = $.parseJSON($("#hdnNoteSentenceModel").val());

    $scope.IsEditMode = $scope.NoteSentenceModel.NoteSentenceID > 0;

    $scope.Save = function () {
        var isValid = CheckErrors($("#frmNoteSentence"));
        if (isValid) {
            var jsonData = angular.toJson({ noteSentence: $scope.NoteSentenceModel });

            AngularAjaxCall($http, SiteUrl.AddNoteSentenceURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {

                    if (response.IsSuccess) {
                        if ($scope.NoteSentenceModel.NoteSentenceID == 0 || $scope.NoteSentenceModel.NoteSentenceID == undefined) {
                            toastr.success("Note Sentence Save Successfully");
                            $scope.NoteSentenceModel = null;
                        }
                        else {
                            toastr.success("Note Sentence Save Successfully");
                        }
                    }
                    else {
                        ShowMessages(response);
                    }
                });
        }
    };

    $scope.Cancel = function () {
        window.location.reload();
    };
};

controllers.AddNoteSentenceController.$inject = ['$scope', '$http', '$timeout', '$window'];