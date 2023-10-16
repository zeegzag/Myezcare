var vm;
controllers.PayBillController = function ($scope, $http, $timeout) {
    vm = $scope;
    $scope.model = {};
    console.log("working this");
    $scope.Tranaction_Succeessfully_completed = false;
    $scope.paynow = function () {
        let errmess = "";
        if ($scope.model.CardNumber == "" || $scope.model.CardNumber == undefined || $scope.model.CardNumber == null) {
            errmess += "Please enter card nummber";
            return ShowMessage("Please enter card nummber", "error", 1000, undefined, undefined, false);

        }
        if ($scope.model.EXPIRYMM == "" || $scope.model.EXPIRYMM == undefined || $scope.model.EXPIRYMM == null) {
            errmess += "Please enter Expiry month";
            return ShowMessage("Please enter Expiry month", "error", 1000, undefined, undefined, false);

        }
        if ($scope.model.EXPIRYYY == "" || $scope.model.EXPIRYYY == undefined || $scope.model.EXPIRYYY == null) {
            errmess += "Please enter expity year";
            return ShowMessage("Please enter expity year", "error", 1000, undefined, undefined, false);

        }
        if ($scope.model.CCV == "" || $scope.model.CCV == undefined || $scope.model.CCV == null) {
            errmess += "Please enter CCV";
            return ShowMessage("Please enter CCV", "error", 1000, undefined, undefined, false);

        }

        if ($scope.model.FirstName == "" || $scope.model.FirstName == undefined || $scope.model.FirstName == null) {
            errmess += "Please enter First Name";
            return ShowMessage("Please enter First Name", "error", 1000, undefined, undefined, false);

        }

        if ($scope.model.LastName == "" || $scope.model.LastName == undefined || $scope.model.LastName == null) {
            errmess += "Please enter Last Name";
            return ShowMessage("Please enter Last Name", "error", 1000, undefined, undefined, false);

        }
        if ($scope.model.Address == "" || $scope.model.Address == undefined || $scope.model.Address == null) {
            errmess += "Please enter Address";
            ShowMessage("Please enter Address", "error", 1000, undefined, undefined, false);

        }
        if ($scope.model.ZipCode == "" || $scope.model.ZipCode == undefined || $scope.model.ZipCode == null) {
            errmess += "Please enter ZipCode";
            return ShowMessage("Please enter ZipCode", "error", 1000, undefined, undefined, false);

        }
        if ($scope.model.Email == "" || $scope.model.Email == undefined || $scope.model.Email == null) {
            errmess += "Please enter Email";
            return ShowMessage("Please enter Email", "error", 1000, undefined, undefined, false);
        }
        if ($scope.model.PhoneNumber == "" || $scope.model.PhoneNumber == undefined || $scope.model.PhoneNumber == null) {
            errmess += "Please enter PhoneNumber";
            return ShowMessage("Please enter PhoneNumber", "error", 1000, undefined, undefined, false);
        }
        if (errmess != "") {
            // ShowMessage();
            return;
        }
        var url_string = window.location.href;
        var url = new URL(url_string);
        var c = url.searchParams.get("am");
        $scope.model.Amount = c;
        $scope.transsucess = false;

        $scope.model.InvoiceNumber = url.searchParams.get("bid");
        $scope.model.BillingMonthDate = url.searchParams.get("bm");

        AngularAjaxCall($http, "/hc/Payment/PaymentFinal", JSON.stringify($scope.model), "Post", "json", "application/json").success(function (response) {
            ////debugger
            if (response.IsSuccess) {
                console.log();
                $scope.transsucess = true;
                $scope.TransactionId = response.Data.TransactionId;
                $scope.model = {};
                $scope.Tranaction_Succeessfully_completed = true;
            } else {
                $scope.TransactionId = response.Data.TransactionId;
                $scope.Tranaction_Succeessfully_completed = false;
                $scope.transsucess = false;

            }
            //ShowMessages(response);
        });
    };
};
controllers.PayBillController.$inject = ['$scope', '$http', '$timeout'];
$(document).ready(function () {
    //$(".dateInputMask").attr("placeholder", "mm/dd/yyyy");
    //$('.time').inputmask({
    //    mask: "h:s t\\m",
    //    placeholder: "hh:mm a",
    //    alias: "datetime",
    //    hourFormat: "12"
    //});
});