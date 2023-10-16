var custModel;
controllers.AddReleaseNoteController = function ($scope, $http, $timeout, $window) {
    custModel = $scope;

    var modelJson = $.parseJSON($("#hdnAddreleaseNoteModel").val());
    $scope.ReleaseNoteModel = modelJson;
  
    $scope.SaveReleaseNoteDetails = function () {
        var isValid = CheckErrors($("#frmReleaseNote"));
        if (isValid) {
            var jsonData = angular.toJson($scope.ReleaseNoteModel.ReleaseNote);
            AngularAjaxCall($http, SiteUrl.SaveReleaseNoteURL, jsonData, "Post", "json", "application/json").
                success(function (response) {
                    if (response.IsSuccess) {
                        SetMessageForPageLoad(response.Message, "ShowReleaseNoteMessage");
                        window.location.href = SiteUrl.ReleaseNoteListURL;
                    } else {
                        ShowMessages(response);
                    }
                });
        }
    };

   
};
controllers.AddReleaseNoteController.$inject = ['$scope', '$http', '$timeout','$window'];