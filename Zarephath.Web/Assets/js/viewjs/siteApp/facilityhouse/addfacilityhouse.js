var vm;

controllers.AddFacilityHouseController = function ($scope, $http, $window) {
    vm = $scope;
    $scope.IsEdit = false;
    $scope.IsDisable = false;
    $scope.SetFacilityHouseModel = $.parseJSON($("#hdnFacilityHouseModel").val());

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnFacilityHouseModel").val());
    };

    if (($scope.SetFacilityHouseModel.FacilityHouseModel != undefined || $scope.SetFacilityHouseModel.FacilityHouseModel != null) && $scope.SetFacilityHouseModel.FacilityHouseModel.FacilityID > 0) {
        $scope.IsEdit = true;
        if ($scope.SetFacilityHouseModel.FacilityHouseModel.ParentFacilityID > 0) {
            $scope.IsDisable = true;
        }
    }

    $scope.FacilityHouseModel = $scope.SetFacilityHouseModel.FacilityHouseModel;
    $scope.FacilityTransportLocationMapping = $scope.SetFacilityHouseModel.FacilityTransportLocationMapping;
    //$scope.FacilityHouseModel.ParentFacilityID = 0;
    $scope.PayorList = $scope.SetFacilityHouseModel.PayorList;

    $scope.SaveFacilityHouseDetails = function () {
        if (CheckErrors("#frmaddFacilityId")) {
            var jsonData = angular.toJson($scope.FacilityHouseModel);
            AngularAjaxCall($http, SiteUrl.AddFacilityHouseURL, jsonData, "Post", "json", "application/json").success(function (response) {
                //if (response.IsSuccess) {
                //    SetMessageForPageLoad(response.Message, "FacilityHouseUpdateSuccessMessage");
                //    //$window.location = SiteUrl.FacilityHouseListURL;
                //    return;
                //}                
                if (response.IsSuccess) {
                    SetMessageForPageLoad(response.Message, "FacilityHouseUpdateSuccessMessage");
                    //$scope.PayorModel.PayorPayorID = response.Data;
                    $window.location = SiteUrl.AddFacilityHouseURL + response.Data;
                }
                else
                    ShowMessages(response);
            });
        }
    };



    $scope.GetParentFacilityHouse = function (id) {
        if (id > 0) {
            $scope.oldFacilityID = $scope.FacilityHouseModel.FacilityID;
            $scope.CreatedDate = $scope.FacilityHouseModel.CreatedDate;
            $scope.UpdatedDate = $scope.FacilityHouseModel.UpdatedDate;
            var jsonData = angular.toJson({ facilityid: id });
            AngularAjaxCall($http, SiteUrl.GetParentFacilityHouse, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.FacilityHouseModel = response.Data;
                    $scope.FacilityHouseModel.FacilityID = $scope.oldFacilityID;
                    $scope.FacilityHouseModel.CreatedDate = $scope.CreatedDate;
                    $scope.FacilityHouseModel.UpdatedDate = $scope.UpdatedDate;
                    $scope.FacilityHouseModel.FacilityName = null;
                    return;
                }
                ShowMessages(response);
            });
        } else {
            $scope.FacilityHouseModel.ZipCode = null;
            $scope.FacilityHouseModel = {};
            $scope.FacilityHouseModel.State = "AZ";
            $scope.FacilityHouseModel.SelectedPayors = {};
            $('#ZipCodeID').val(null);
            setTimeout(function () {
                $('#FacilityHouseModel_FacilityName').focus();
            }, 100);
        }
    };

    $("a#FacilityTransportLocationMapping").on('shown.bs.tab', function (e) {
        if (!$scope.SetFacilityHouseModel.SelectedFaciltyID)
            $scope.SetFacilityHouseModel.SelectedFaciltyID = $scope.FacilityHouseModel.FacilityID;

        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    });

    $scope.$watch(function () {
        return $scope.SetFacilityHouseModel.SelectedFaciltyID + $scope.SetFacilityHouseModel.SelectedTransportLocationID;
    }, function () {
        if ($scope.SetFacilityHouseModel.SelectedFaciltyID && $scope.SetFacilityHouseModel.SelectedTransportLocationID) {
            $scope.GetFacilityTransportLocationMapping();
        }
    });

    $scope.GetFacilityTransportLocationMapping = function () {
        AngularAjaxCall($http, SiteUrl.GetFacilityTransportLocationMappingURL, { facilityID: $scope.SetFacilityHouseModel.SelectedFaciltyID, transportLocationID: $scope.SetFacilityHouseModel.SelectedTransportLocationID }, "Post", "json", "application/json", true).success(function (response) {
            if (response.IsSuccess) {
                $scope.FacilityTransportLocationMapping = response.Data;
            } else {
                $scope.FacilityTransportLocationMapping = {};
            }
        });
    };

    $scope.SaveFacilityTransportLocationMapping = function () {
        var isValid = CheckErrors($("#frmFacilityTransportLocationMapping"));
        if (isValid) {
            $scope.FacilityTransportLocationMapping.FacilityID = $scope.SetFacilityHouseModel.SelectedFaciltyID;
            $scope.FacilityTransportLocationMapping.TransportLocationID = $scope.SetFacilityHouseModel.SelectedTransportLocationID;
            AngularAjaxCall($http, SiteUrl.SaveFacilityTransportLocationMappingURL, { model: $scope.FacilityTransportLocationMapping }, "Post", "json", "application/json", false).success(function (response) {
                ShowMessages(response);
            });
        }
    };

    $scope.Cancel = function () {
        $window.location = SiteUrl.FacilityHouseListURL;
    };
};
controllers.AddFacilityHouseController.$inject = ['$scope', '$http', '$window'];

$(document).ready(function () {
    $("#AHCCCS_ID").inputmask({
        mask: "999999",
        placeholder: "XXXXXX"
    });
    $("#NPI").inputmask({
        mask: "9999999999",
        placeholder: "XXXXXXXXXX"
    });
    $("#EIN").inputmask({
        mask: "999999999",
        placeholder: "XXXXXXXXX"
    });

    //$(".dateInputMask").inputmask("m/d/y", {
    //    placeholder: "mm/dd/yyyy"
    //});

    $(".dateInputMask").attr("placeholder", "mm/dd/yy");

});