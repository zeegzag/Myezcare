var vm;
controllers.CompanyInvoiceListController = function ($scope, $http, $timeout) {
    vm = $scope;

    $scope.InvoiceList = [];
    
    // Ninja Invoice list
    $scope.GetInvoiceList = function () {
        var jsonData = angular.toJson();
        AngularAjaxCall($http, HomeCareSiteUrl.NinjaInvoiceList, jsonData, "Get", "json", "application/json").success(function (response) {
            $scope.InvoiceList = response;
            $scope.InvoiceList.length = response.length;
            ShowMessages(response);
        });
    };
    $scope.GetInvoiceList();
    $scope.Refresh = function () {
        $scope.GetInvoiceList();
    };
    $scope.ResetSearchFilter = function () {
        $scope.InvoiceModel.InvoiceDate = "";
        $scope.InvoiceModel.DueDate = "";
        $scope.InvoiceModel.InvoiceAmount = "";
        $scope.InvoiceModel.PaidAmount = "";
        $scope.InvoiceModel.IsPaid = "";

    };
    $scope.SearchInvoice = function (InvoiceModel) {

    };
   
    
    $scope.OpenInvoice = function (invoicepath) {
        var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,width=' + window.outerWidth / 1.2 + ',height=' + window.outerHeight / 1 + ', overflow= hidden';
        var pdfWindow = window.open('about:blank', 'null', winFeature);
        pdfWindow.document.write("<html><head><title>Invoice</title></head><body>"
            + '<embed width="100%" height="100%" name="plugin" src="' + invoicepath + '"'
            + 'type="application/pdf" internalinstanceid="21"></body></html>');
    };
    
    $scope.OpenInvoiceforPayment = function (piinvoiceID, piinvoiceAmount, piclient_id) {

            var post = $http({
            method: "POST",
            url: HomeCareSiteUrl.ProcessPaymentURL,//"/Invoice/StartProcessingPayment",
            dataType: 'json',
                data: { invoiceId: piinvoiceID, invoiceAmount: piinvoiceAmount, client_id: piclient_id },
            headers: { "Content-Type": "application/json" }
        });

        post.success(function (response, status) {
            // $window.alert("Hello: " + data.Name + " .\nCurrent Date and Time: " + data.DateTime);


            if (response == "1") {
                //window.open("../UserPaymentDetail/AddPaymentDetail/" + piinvoiceID + "/" + piinvoiceAmount, "", "height='" + window.outerHeight / 1.2 + "',width='" + window.outerWidth / 1 + "',scrolling=yes,resizable=no");

                var url = "../UserPaymentDetail/AddPaymentDetail/" + piinvoiceID + "/" + piinvoiceAmount;
                var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,width=' + window.outerWidth / 1.5 + ',height=' + window.outerHeight / 1.2 + ', overflow= hidden';
                var pdfWindow = window.open('about:blank', 'null', winFeature);
                pdfWindow.document.write("<html><head><title>Invoice</title></head><body>"
                    + '<embed width="100%" height="100%" name="plugin" src="' + url + '"'
                    + 'type="application/pdf" internalinstanceid="21"></body></html>')


            }
            if (response == "2") {
                toastr.success("Payment Done successfully");
                window.location.reload();
            }
            if (response == "3") {
                toastr.success("Payment failed.");
                // window.location.reload();
            }

        });

        post.error(function (data, status) {
            alert(data.Message);
        });





    };









};

controllers.CompanyInvoiceListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    //$('#InvoiceDatePicker').focusout();
    ShowPageLoadMessage("ShowPhysicianMessage");


});

window.onload = function () {
    //document.getElementById("refreshBtn").focus();
    //$('input').blur();

    //var elelist = document.getElementsByTagName("input");
    //for (var i = 0; i < elelist.length; i++) {
    //    elelist[i].blur();
    //}
}; 


/*


var vm;
controllers.CompanyInvoiceListController = function ($scope, $http, $timeout) {

    vm = $scope;
    //debugger
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetCompanyInvoiceListPage").val());
    };

    $scope.AddInvoiceDetailURL = HomeCareSiteUrl.CompanyClientInvoiceListURL;
    $scope.InvoiceList = [];
    $scope.SelectedInvoiceIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.InvoiceModel = $.parseJSON($("#hdnSetCompanyInvoiceListPage").val());
    $scope.InvoiceModel.Status = 0;
    $scope.SearchInvoiceListPage = $scope.InvoiceModel;
    $scope.TempSearchInvoiceListPage = $scope.InvoiceModel;
    $scope.InvoiceListPager = new PagerModule("OrganizationId", "", "DESC");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            model: $scope.InvoiceModel,
            pageSize: $scope.InvoiceListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.InvoiceListPager.sortIndex,
            sortDirection: $scope.InvoiceListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchInvoiceListPage = $.parseJSON(angular.toJson($scope.TempSearchInvoiceListPage));
    };

    $scope.GetInvoiceList = function (isSearchDataMappingRequire) {
        debugger
        //Reset Selcted Checkbox items and Control
        $scope.SelectedInvoiceIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchInvoiceListPage = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.InvoiceListPager.currentPage);

        AngularAjaxCall($http, HomeCareSiteUrl.CompanyClientInvoiceListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.InvoiceList = response.Data.Items;
                $scope.InvoiceListPager.currentPageSize = response.Data.Items.length;
                $scope.InvoiceListPager.totalRecords = response.Data.TotalItems;
                // console.log('$scope.InvoiceList=', $scope.InvoiceList);
            }
            ShowMessages(response);
        });
    };


    $scope.Refresh = function () {
        //debugger
        $scope.ResetSearchFilter();
        $scope.SearchInvoiceModel.IsAll = true;
        $scope.InvoiceListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        //debugger
        //$scope.SearchInvoiceModel.IsAll = true;
        $scope.InvoiceModel.InvoiceDate = "";
        $scope.InvoiceModel.DueDate = "";
        $scope.InvoiceModel.InvoiceAmount = "";
        $scope.InvoiceModel.PaidAmount = "";
        $scope.InvoiceModel.IsPaid = "";
        //$scope.SearchInvoiceModel.OrganizationName = '';
        //$scope.SearchInvoiceModel.InvoiceDate = '';
        //$scope.SearchInvoiceModel.DueDate = '';
        //$scope.SearchInvoiceModel.InvoiceStatus = '';
        //$scope.SearchInvoiceModel.InvoiceAmount = '';
        //$scope.SearchInvoiceModel.PaidAmount = '';
        //$scope.SearchInvoiceModel.AccountStauts = '';
        //$scope.SearchInvoiceModel.AccountStauts = '';
        //$scope.SearchInvoiceModel.IsActive = "1";
        $scope.InvoiceListPager.currentPage = 1;
        $scope.InvoiceListPager.getDataCallback();
    };


    //$scope.Refresh = function () {
    //    //debugger
    //    $scope.ResetSearchFilter();
    //    $scope.SearchInvoiceListPage.currentPage = $scope.InvoiceListPager.currentPage;
    //    $scope.InvoiceListPager.getDataCallback();
    //};

    //$scope.ResetSearchFilter = function () {
    //    //debugger
    //    $timeout(function () {
    //        //$("#AgencyID").select2("val", '');
    //        //$("#AgencyLocationID").select2("val", '');
    //        $scope.SearchInvoiceListPage = $scope.newInstance().SearchInvoiceListPage;
    //        $scope.TempSearchInvoiceListPage = $scope.newInstance().SearchInvoiceListPage;
    //        $scope.TempSearchInvoiceListPage.IsDeleted = "0";
    //        $scope.InvoiceListPager.currentPage = 1;
    //        $scope.InvoiceListPager.getDataCallback();
    //    });
    //};
    $scope.SearchInvoice = function () {
        $scope.InvoiceModel.Status = $scope.InvoiceModel.IsPaid1 == "1" ? 1 : 0;
        $scope.InvoiceListPager.currentPage = 1;
        $scope.InvoiceListPager.currentPage = 1;
        $scope.InvoiceListPager.getDataCallback(true);
    };

    //// This executes when select single checkbox selected in table.
    $scope.SelectInvoice = function (Invoice) {
        if (Invoice.IsChecked)
            $scope.SelectedInvoiceIds.push(Invoice.ReferralInvoiceID);
        else
            $scope.SelectedInvoiceIds.remove(Invoice.ReferralInvoiceID);

        if ($scope.SelectedInvoiceIds.length == $scope.InvoiceListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    //// This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedInvoiceIds = [];

        angular.forEach($scope.InvoiceList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedInvoiceIds.push(item.ReferralInvoiceID);
        });
        return true;
    };


    $scope.selectedBill = function (encryptedInvoiceNumber, encryptedAmount, encryptedMonthDate) {
        ////debugger
        //window.location.href = HomeCareSiteUrl.PaymentBillURL + "?bid=" + encryptedInvoiceNumber + "&am=" + encryptedAmount;
        //window.open(
        //    HomeCareSiteUrl.PaymentBillURL + "?bid=" + encryptedInvoiceNumber + "&am=" + encryptedAmount + "&bm=" + encryptedMonthDate,
        //    '_blank' // <- This is what makes it open in a new window.
        //);
        var url = HomeCareSiteUrl.PaymentBillURL + "?bid=" + encryptedInvoiceNumber + "&am=" + encryptedAmount + "&bm=" + encryptedMonthDate
        var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,width=' + window.outerWidth / 1.2 + ',height=' + window.outerHeight / 1 + ', overflow= hidden';
        var pdfWindow = window.open('about:blank', 'null', winFeature);
        pdfWindow.document.write("<html><head><title>Invoice</title></head><body>"
            + '<embed width="100%" height="1000px" name="plugin" src="' + url + '"'
            + 'type="application/pdf" internalinstanceid="21"></body></html>');
    };

   
    $scope.OpenInvoiceforPayment = function (piinvoiceID, piinvoiceAmount) {
      
        //window.open("../UserPaymentDetail/AddPaymentDetail/" + piinvoiceID + "/" + piinvoiceAmount, "", "height='" + window.outerHeight / 1.2 + "',width='" + window.outerWidth / 1.2 + "',scrolling=yes,resizable=no");

         var post = $http({
            method: "POST",
             url: HomeCareSiteUrl.ProcessPaymentURL,//"/Invoice/StartProcessingPayment",
            dataType: 'json',
             data: { invoiceId: piinvoiceID, invoiceAmount: piinvoiceAmount },
            headers: { "Content-Type": "application/json" }
        });

        post.success(function (response, status) {
           // $window.alert("Hello: " + data.Name + " .\nCurrent Date and Time: " + data.DateTime);
           

            if (response == "1") {
                //window.open("../UserPaymentDetail/AddPaymentDetail/" + piinvoiceID + "/" + piinvoiceAmount, "", "height='" + window.outerHeight / 1.2 + "',width='" + window.outerWidth / 1 + "',scrolling=yes,resizable=no");

                var url = "../UserPaymentDetail/AddPaymentDetail/" + piinvoiceID + "/" + piinvoiceAmount;
                var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,width=' + window.outerWidth / 1.5 + ',height=' + window.outerHeight / 1.2 + ', overflow= hidden';
                var pdfWindow = window.open('about:blank', 'null', winFeature);
                pdfWindow.document.write("<html><head><title>Invoice</title></head><body>"
                    + '<embed width="100%" height="100%" name="plugin" src="' + url + '"'
                    + 'type="application/pdf" internalinstanceid="21"></body></html>')


            }
            if (response == "2") {
                toastr.success("Payment Done successfully");
                window.location.reload();
            }
            if (response == "3") {
                toastr.success("Payment failed.");
               // window.location.reload();
            }

        });

        post.error(function (data, status) {
             alert(data.Message);
        });
        



        //AngularAjaxCall($http, HomeCareSiteUrl.AddBillingPaymentDetailURL, jsonData, "post", "json", "application/json", true).
        //    success(function (response, status, headers, config) {
        //        //ShowMessages(response);
        //        if (response.IsSuccess) {
        //     window.open("../UserPaymentDetail/AddPaymentDetail/" + piinvoiceID, "", "height='" + window.outerHeight / 1.2 + "',width='" + window.outerWidth / 1.2 + "',scrolling=yes,resizable=no");

        //        } 

        //    });

       
    };





    $scope.OpenInvoice = function (invoicepath) {
        var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,width=' + window.outerWidth / 1.2 + ',height=' + window.outerHeight / 1 + ', overflow= hidden';
        var pdfWindow = window.open('about:blank', 'null', winFeature);
        pdfWindow.document.write("<html><head><title>Invoice</title></head><body>"
            + '<embed width="100%" height="100%" name="plugin" src="' + invoicepath + '"'
            + 'type="application/pdf" internalinstanceid="21"></body></html>');
    };



    $scope.DatePickerDate = function (modelDate, newDate) {
        var a;
        if (modelDate) {
            var dt = new Date(modelDate);
            dt >= newDate ? a = newDate : a = dt;
        }
        else {
            a = newDate;
        }
        return moment(a).format('L');
    };

    $scope.InvoiceListPager.getDataCallback = $scope.GetInvoiceList;
    $scope.InvoiceListPager.getDataCallback();

};

controllers.CompanyInvoiceListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    //$('#InvoiceDatePicker').focusout();
    ShowPageLoadMessage("ShowPhysicianMessage");


});

window.onload = function () {
    //document.getElementById("refreshBtn").focus();
    //$('input').blur();

    //var elelist = document.getElementsByTagName("input");
    //for (var i = 0; i < elelist.length; i++) {
    //    elelist[i].blur();
    //}
};


function OpenInvoice(id) {
    //window.location.href = "";
    alert('hi');
}


*/




