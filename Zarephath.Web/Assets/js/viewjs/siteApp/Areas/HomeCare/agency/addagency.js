var custModel;
controllers.AddAgencyController = function ($scope, $http) {
    custModel = $scope;
    $scope.AgencyModel = $.parseJSON($("#hdnAgencyModel").val());
    $scope.Save = function () {
        var isValid = CheckErrors($("#frmAgency"));
        if (isValid) {
            var jsonData = angular.toJson({ Agency: $scope.AgencyModel.Agency, AgencyTaxonomies: $scope.AgencyModel.AgencyTaxonomies });
            AngularAjaxCall($http, HomeCareSiteUrl.AddAgencyURL, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        if ($scope.AgencyModel.Agency.AgencyID == 0 || $scope.AgencyModel.Agency.AgencyID == undefined) {
                            $scope.AgencyModel = null;
                        }
                    }
                    ShowMessages(response);
                });
        }
    };

    $scope.FetchNPIData = function () {
        if ($scope.AgencyModel.Agency.NPI !== undefined && $scope.AgencyModel.Agency.NPI !== '' && $scope.AgencyModel.Agency.NPI !== null) {
            var jsonData = angular.toJson({ number: $scope.AgencyModel.Agency.NPI });
            AngularAjaxCall($http, HomeCareSiteUrl.GetNPIAPI, jsonData, "post", "json", "application/json", true).
                success(function (response) {
                    if (response.IsSuccess) {
                        var npiDataObject = JSON.parse(response.Data);
                        if (npiDataObject && npiDataObject.results && npiDataObject.results.length > 0) {
                            var npiData = npiDataObject.results[0];
                            $scope.AgencyModel.Agency.NickName = npiData.basic.organization_name;
                            $scope.AgencyModel.Agency.StateCode = npiData.addresses[0].state;
                            $scope.AgencyModel.Agency.Phone = npiData.addresses[0].telephone_number;
                            $scope.AgencyModel.Agency.Address = npiData.addresses[0].address_1;
                            $scope.AgencyModel.Agency.City = npiData.addresses[0].city;
                            $scope.AgencyModel.Agency.ZipCode = npiData.addresses[0].postal_code;
                            $scope.AgencyModel.AgencyTaxonomies = [];
                            npiData.taxonomies.forEach(function (item) {
                                $scope.AgencyModel.AgencyTaxonomies.push({ Code: item.code, Description: item.desc, IsPrimary: item.primary, State: item.state, License: item.license, TaxonomyGroup: item.taxonomy_group });
                            });
                        }
                        else {
                            ShowMessages({ IsSuccess: false, Message: window.NPIInvalid})
                        }
                    } else {
                        ShowMessages(response);
                    }
                });
        }
    };

    $scope.Cancel = function () {
        window.location.reload();
    };
};
controllers.AddAgencyController.$inject = ['$scope', '$http'];