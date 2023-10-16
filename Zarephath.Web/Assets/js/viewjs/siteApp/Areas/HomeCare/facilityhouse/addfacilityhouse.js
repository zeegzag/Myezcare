var vm;
controllers.AddFacilityHouseController = function ($scope, $http, $window) {
    vm = $scope;

    $scope.SetFacilityHouseModel = $.parseJSON($("#hdnFacilityHouseModel").val());
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnFacilityHouseModel").val());
    };
    $scope.FacilityHouseModel = $scope.SetFacilityHouseModel.FacilityHouseModel;
    if ($("#IsPartialView").val() == 1 && $scope.FacilityHouseModel != undefined) {
        $scope.FacilityHouseModel.AgencyID = window.parent.$("#AgencyID").val();
    }
    $scope.ListEquipment = $scope.SetFacilityHouseModel.EquipmentList;
    $scope.TempSelectedEquipmentIDs = [];
    if ($scope.ListEquipment.length > 0) {
        $.each($scope.ListEquipment, function (i, value) {
            $scope.TempSelectedEquipmentIDs.push(value.EquipmentID);
        });
    }

    $scope.SaveFacilityHouseDetails = function () {
        debugger
        if (CheckErrors("#frmaddFacilityId")) {
            var jsonData = angular.toJson({
                Facility: $scope.FacilityHouseModel,
                EquipmentList: $scope.ListEquipment
            });
            AngularAjaxCall($http, HomeCareSiteUrl.AddFacilityHouseURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {                    
                    if ($scope.FacilityHouseModel.FacilityID == 0 || $scope.FacilityHouseModel.FacilityID == undefined) {
                        toastr.success("Facility House Save Successfully");
                        $scope.FacilityHouseModel = null;
                    } else {
                        toastr.success("Facility House Save Successfully");
                    }
                }
                else {
                    ShowMessages(response);
                }
            });
        }
    };

    $scope.CloseModal = function () {
        
        window.parent.$('#AgencyFacility').modal('toggle');
    }

    $scope.Cancel = function () {
        window.location.reload();
        //var EncryptedFacilityID = $scope.SetFacilityHouseModel.FacilityHouseModel.FacilityID;
        //window.location.href= HomeCareSiteUrl.PartialAddFacilityHouseURL + EncryptedFacilityID;
    };

    //#region Facilty House Equipments
    
    $scope.EquipmentTokenObj = {};
    $scope.SearhEquipmentURL = HomeCareSiteUrl.SearhEquipmentURL;

    $scope.EquipmentResultsFormatter = function (item) {
        return "<li id='{0}'>{0}</li>".format(item.EquipmentName);
    };
    $scope.EquipmentTokenFormatter = function (item) {
        return "<li id='{0}'>{0}</li>".format(item.EquipmentName);
    };

    $scope.AddedEquipment = function (item) {
        var push = true;
        if (_.findWhere($scope.ListEquipment, item) == null) {
            angular.forEach($scope.ListEquipment, function (items) {
                if (items.EquipmentName == item.EquipmentName) {
                    push = false;
                }
            });
            if (push) {
                $scope.ListEquipment.push(item);
                $scope.TempSelectedEquipmentIDs.push(item.EquipmentID);
            }
        }
        $scope.EquipmentTokenObj.clear();
    };

    $scope.DeleteEquipment = function (item, index) {
        $scope.ListEquipment = $scope.ListEquipment.filter(function (obj) {
            return obj.EquipmentID !== item.EquipmentID;
        });
        $scope.TempSelectedEquipmentIDs = $scope.TempSelectedEquipmentIDs.filter(function (obj) {
            return obj !== item.EquipmentID;
        });
    };

    //#endregion
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

    var dateformat = GetOrgDateFormat();
    $(".dateInputMask").attr("placeholder", dateformat);
    $('.time').inputmask({
        mask: "h:s t\\m",
        placeholder: "hh:mm a",
        alias: "datetime",
        hourFormat: "12"
    });

});