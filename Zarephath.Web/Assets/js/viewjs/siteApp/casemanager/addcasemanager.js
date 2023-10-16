var custModel;

controllers.AddCaseManagerController = function ($scope, $http, $timeout, $window) {
    custModel = $scope;

    $scope.CaseManagerModel = $.parseJSON($("#hdnCaseManagerModel").val());



    //TODO: Do not delete this
    //$scope.$watch('CaseManagerModel.CaseManager.AgencyID', function (newValue, oldValue) {
    //    var agencyLocationId = $scope.CaseManagerModel.CaseManager.AgencyLocationID;
    //    $scope.CaseManagerModel.CaseManager.AgencyLocationID = '';
    //    $timeout(function () {
    //        //$("#AgencyLocationID").select2("val", '');
    //        if (parseInt(newValue) > 0) {
    //            var jsonData = angular.toJson({ agencyID: parseInt(newValue), agencyLocationID: agencyLocationId });
    //            AngularAjaxCall($http, SiteUrl.GetAgencyLocationListURL, jsonData, "Post", "json", "application/json").
    //                   success(function (response, status, headers, config) {
    //                       $scope.CaseManagerModel.AgencyLocationList = response.Data;
    //                       $scope.CaseManagerModel.CaseManager.AgencyLocationID = response.Data[0].AgencyLocationID;
    //                   });
    //        }
    //    });
    //});

    $scope.Save = function () {
        var isValid = CheckErrors($("#frmCaseManager"));
        if (isValid) {
            var jsonData = angular.toJson({ CaseManager: $scope.CaseManagerModel.CaseManager });

            AngularAjaxCall($http, SiteUrl.AddCaseManagerURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {

                    if (response.IsSuccess) {
                        if ($scope.CaseManagerModel.CaseManager.CaseManagerID == 0 || $scope.CaseManagerModel.CaseManager.CaseManagerID == undefined) {
                            toastr.success("Case Manager Save Successfully");
                            $scope.CaseManagerModel = null;
                        }
                        else {
                            toastr.success("Case Manager Save Successfully");
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

controllers.AddCaseManagerController.$inject = ['$scope', '$http', '$timeout', '$window'];