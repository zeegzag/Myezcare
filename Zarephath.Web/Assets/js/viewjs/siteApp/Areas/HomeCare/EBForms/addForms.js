var custModel;

controllers.AddFormsController = function ($scope, $http, $timeout) {
    custModel = $scope;
    $scope.EBFormsModel = $.parseJSON($("#hdnFormModel").val());
     
    $scope.SaveForms = function () {
       
        var isValid = CheckErrors($("#frmAddForms"));
        if (isValid) { 
            //if ($("#NewPdfURI").get(0).files.length == 0 || $scope.EBFormsModel.EBForms.FormId!=null) {
            //    var label = $("#lblFileMessage");
            //    ShowMessages("Please Select File");
            //   // e.preventDefault();
            //    label.show();
            //    return;
            //}
            //$scope.EBFormsModel.EBForms.files = $("#NewPdfURI").get(0).files[0];
            //$scope.EBFormsModel.EBForms.FileName = $("#NewPdfURI").get(0).files[0].name;
            //$scope.EBFormsModel.EBForms.FilePath = URL.createObjectURL($("#NewPdfURI").get(0).files[0]);
            var jsonData = angular.toJson({ forms: $scope.EBFormsModel.EBForms });
            AngularAjaxCall($http, HomeCareSiteUrl.AddFormURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        SetMessageForPageLoad(response.Message, "ShowFormsMessage");
                        window.location.href = HomeCareSiteUrl.FormListURL;
                    } else {
                        ShowMessages(response);
                    }

                });
        }
    };
};

controllers.AddFormsController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
   
});