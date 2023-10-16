var vm_careform;
controllers.CareFormController = function ($scope, $http, $window, $timeout, $filter) {

    var modalJson = $.parseJSON($("#hdnSetAddReferralModel").val());

    $scope.newInstance = function () {
            return $.parseJSON($("#hdnSetAddReferralModel").val());
    };
    vm_careform = $scope;

    $scope.EncryptedReferralID = window.EncryptedReferralID;
    $scope.AddReferralModel = modalJson;

    $scope.CareForm = $scope.AddReferralModel.CareForm;
    $scope.VisitTypeList = $scope.AddReferralModel.VisitTypeList;

    $scope.SaveCareFormDetails = function () {
        var clientSign = $scope.CareForm.TempClientSignaturePath;
        if (clientSign == null || clientSign == '' || clientSign == undefined) {
            ShowMessage("Please provide a client's signature first", "error");
            return;
        }

        if ($scope.CareForm.IsPhysiciansOrdersNeeded == false)
            $scope.CareForm.PhysicianOrdersDescription = null;

        if ($scope.CareForm.IsChargesForServicesRendered == 1 || $scope.CareForm.IsChargesForServicesRendered == 2)
            $scope.CareForm.OnRequest = false;

        $scope.CareForm.ServiceRequested = ($scope.SelectedServiceRequeste) ? $scope.SelectedServiceRequeste.toString() : null;
        $scope.CareForm.EncryptedReferralID = $scope.EncryptedReferralID;

        $scope.CareForm.CareFormDate = $("#txt_careformDate").val();
        $scope.CareForm.ClientSignatureDate = $("#txt_ClientSignatureDate").val();
        $scope.CareForm.NurseSignatureDate = $("#txt_NurseSignatureDate").val();

        var jsonData = angular.toJson($scope.CareForm);
        AngularAjaxCall($http, HomeCareSiteUrl.SaveCareFormDetailsURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.EncryptedCareFormID = response.Data;
                $('#CareFormHtml').modal('hide');
                $scope.CareForm = {};
                $scope.SelectedServiceRequeste = [];
                //$scope.CareForm.IsChargesForServicesRendered = true;
                window.location = HomeCareSiteUrl.GenerateCareFormPdfURL + $scope.EncryptedCareFormID;
            }
            ShowMessages(response);
        });
    };

    $scope.SearchCareFormDetails = {};
    $scope.GetCareFormDetails = function () {
        $scope.SearchCareFormDetails.EncryptedReferralID = $scope.EncryptedReferralID;
        var jsonData = angular.toJson($scope.SearchCareFormDetails);
        AngularAjaxCall($http, HomeCareSiteUrl.GetCareFormDetailsURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.CareForm.NurseSignature = response.Data.EmployeeSignaturePath;
                $scope.CareForm.ClientName = response.Data.ClientName;
                $scope.CareForm.RecordID = response.Data.RecordID;
                $scope.CareForm.LocationOfService = response.Data.LocationOfService;
                $scope.CareForm.Phone = response.Data.Phone;
                $scope.CareForm.Cell = response.Data.Cell;
                $scope.CareForm.Email = response.Data.Email;
            }
            else
                ShowMessages(response);
        });
    };

    $scope.GenerateForm = function () {
        $scope.GetCareFormDetails();
        if ($scope.SelectedServiceRequeste != null && $scope.SelectedServiceRequeste != undefined && $scope.SelectedServiceRequeste.length > 0) {
            $scope.strServiceRequestedList = [];
            $.each($scope.SelectedServiceRequeste, function (index, item) {
                var temp = $scope.VisitTypeList.find(x => x.Value == item);
                $scope.strServiceRequestedList.push(temp.Name);
            });
            $scope.CareForm.Str_ServiceRequested = $scope.strServiceRequestedList.join(', ');
        }
        else {
            $scope.CareForm.Str_ServiceRequested = '';
        }
        $('#CareFormHtml').modal({
            backdrop: 'static',
            keyboard: false
        });
    };



$("a#CarePlan, a#CarePlan_CareForm").on('shown.bs.tab', function (e) {
    $(".tab-pane a[href='#tab_CareForm']").tab('show');
});

// cilent signature

 $scope.OpenClientSignModel = function () {
     $('#ClientSignatureModel').modal({
         backdrop: 'static',
         keyboard: false
     });
};
 $scope.CloseClientSignModel = function () {
    $('#ClientSignatureModel').modal('hide');
     $scope.ClearSignatureImage(); 
};

$scope.signModel = {};

$scope.SaveSignatureImage = function () {
    $scope.signModel = $scope.accept(); // default method for get signatureobject

    if ($scope.signModel.isEmpty == true || ($scope.signModel.isEmpty == false && $scope.signModel.dataUrl == undefined)) {
        ShowMessage("Please provide a client's signature first", "error");
        return;
    }
    else
    {
        $scope.CareForm.ClientSignature = $scope.signModel.dataUrl;
        var jsonData = angular.toJson($scope.CareForm);
        AngularAjaxCall($http, HomeCareSiteUrl.SaveClientSignURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ClearSignatureImage();
                $scope.CareForm.TempClientSignaturePath = response.Data;
                $('#ClientSignatureModel').modal('hide');
            }
            ShowMessages(response);
        });
    }
};

$scope.ClearSignatureImage = function () {
    $scope.clear(); // default method for cleare signature board
};
//END cilent signature 

};
controllers.CareFormController.$inject = ['$scope', '$http', '$window', '$timeout','$filter'];
$(document).ready(function () {
    $(".dateInputMask").attr("placeholder", "mm/dd/yyyy");
});


