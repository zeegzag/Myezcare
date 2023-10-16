var vm;
controllers.InvoiceListController = function ($scope, $http, $timeout, $window) {
	vm = $scope;

	$scope.parseInt = $window.parseInt;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetInvoiceListPage").val());
    };

    $scope.AddInvoiceDetailURL = HomeCareSiteUrl.InvoiceDetailURL;
    $scope.InvoiceList = [];
    $scope.SelectedInvoiceIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.InvoiceModel = $.parseJSON($("#hdnSetInvoiceListPage").val());
    $scope.SearchInvoiceListPage = $scope.InvoiceModel.SearchInvoiceListPage;
	$scope.TempSearchInvoiceListPage = $scope.InvoiceModel.SearchInvoiceListPage;
	$scope.InvoicesCriteria = $scope.InvoiceModel.InvoicesCriteria;

    $scope.EditInvoice = null;

    $scope.InvoiceListPager = new PagerModule("ReferralInvoiceID", "", "DESC");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            SearchInvoiceListPage: $scope.SearchInvoiceListPage,
            pageSize: $scope.InvoiceListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.InvoiceListPager.sortIndex,
            sortDirection: $scope.InvoiceListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SetPostDataEditInvoice = function () {
        var updateInvoiceDueDatemodel = {
            ReferralInvoiceID: $scope.EditInvoice.ReferralInvoiceID,
            InvoiceDueDate: $scope.EditInvoice.InvoiceDueDate
        };
        return angular.toJson(updateInvoiceDueDatemodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchInvoiceListPage = $.parseJSON(angular.toJson($scope.TempSearchInvoiceListPage));

    };

    $scope.GetInvoiceList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedInvoiceIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchInvoiceListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.InvoiceListPager.currentPage);

        AngularAjaxCall($http, HomeCareSiteUrl.GetInvoiceList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.InvoiceList = response.Data.Items;
                $scope.InvoiceListPager.currentPageSize = response.Data.Items.length;
                $scope.InvoiceListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        //$scope.ResetSearchFilter();
        //$scope.CaseManagerListPager.currentPage = $scope.CaseManagerListPager.currentPage;
        $scope.InvoiceListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');
            $scope.SearchInvoiceListPage = $scope.newInstance().SearchInvoiceListPage;
            $scope.TempSearchInvoiceListPage = $scope.newInstance().SearchInvoiceListPage;
            $scope.TempSearchInvoiceListPage.IsDeleted = "0";
            $scope.InvoiceListPager.currentPage = 1;
            $scope.InvoiceListPager.getDataCallback();
        });
    };
    $scope.SearchInvoice = function () {
        $scope.InvoiceListPager.currentPage = 1;
        $scope.InvoiceListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
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

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedInvoiceIds = [];

        angular.forEach($scope.InvoiceList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedInvoiceIds.push(item.ReferralInvoiceID);
        });
        return true;
    };

    $scope.SelectedInvoices = function () {
        return $scope.SelectedInvoiceIds.join(',');
    };

	$scope.GenerateInvoices = function () {
		var criteria = Object.assign({}, $scope.InvoicesCriteria);
		criteria.ReferralIDs = criteria.ReferralIDs ? criteria.ReferralIDs.join(",") : null;
		criteria.CareTypeIDs = criteria.CareTypeIDs ? criteria.CareTypeIDs.join(",") : null;
		var jsonData = angular.toJson({ criteria });
		AngularAjaxCall($http, HomeCareSiteUrl.GenerateInvoices, jsonData, "Post", "json", "application/json", true).success(function (response) {
			ShowMessages(response);
			if (response.IsSuccess) {
				$scope.Refresh();
			}
		});
    };

    $scope.DeleteInvoices = function (referralInvoiceIDs) {
        var jsonData = angular.toJson({ ReferralInvoiceIDs: referralInvoiceIDs });
        AngularAjaxCall($http, HomeCareSiteUrl.DeleteInvoicesURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.Refresh();
            }
        });
    };

	$scope.UpdatePaymentDetail = function (paymentType, invoiceDetail) {
		var payInvoiceAmountDetail = {
			PaymentType: paymentType,
			InvoiceId: invoiceDetail.ReferralInvoiceID,
			ReferralId: invoiceDetail.ReferralID,
			Amount: 0
		};
		var jsonData = angular.toJson({ payInvoiceAmountDetail: payInvoiceAmountDetail });
		AngularAjaxCall($http, HomeCareSiteUrl.PayInvoiceAmountUrl, jsonData, "Post", "json", "application/json").success(function (response) {
			ShowMessages(response);
			if (response.IsSuccess) {
				$scope.Refresh();
			}
		});
	}

    $scope.DeleteInvoice = function (ReferralInvoiceID, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchInvoiceListPage.ListOfIdsInCsv = ReferralInvoiceID > 0 ? ReferralInvoiceID.toString() : $scope.SelectedInvoiceIds.toString();

                if (ReferralInvoiceID > 0) {
                    if ($scope.InvoiceListPager.currentPage != 1)
                        $scope.InvoiceListPager.currentPage = $scope.InvoiceList.length === 1 ? $scope.InvoiceListPager.currentPage - 1 : $scope.InvoiceListPager.currentPage;
                } else {

                    if ($scope.InvoiceListPager.currentPage != 1 && $scope.SelectedInvoiceIds.length == $scope.InvoiceListPager.currentPageSize)
                        $scope.InvoiceListPager.currentPage = $scope.InvoiceListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedInvoiceIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.InvoiceListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteInvoice, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.InvoiceList = response.Data.Items;
                        $scope.InvoiceListPager.currentPageSize = response.Data.Items.length;
                        $scope.InvoiceListPager.totalRecords = response.Data.TotalItems;
                    }
                });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
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
    $scope.EditInvoiceDueDate = function (referralInvoiceID, invoice) {
        if (referralInvoiceID > 0) {
            $scope.EditInvoice = invoice;
            $('#EditInvoiceDueDateModal').modal({ backdrop: 'static', keyboard: false });
        }
    };
    $scope.CloseEditInvoiceDueDateModal = function () {
        $('#EditInvoiceDueDateModal').modal('hide');
    }
    $scope.SaveEditInvoiceDueDate = function () {
        if ($scope.EditInvoice != null && $scope.EditInvoice.ReferralInvoiceID > 0) {
            var jsonData = $scope.SetPostDataEditInvoice();
            AngularAjaxCall($http, HomeCareSiteUrl.UpdateInvoiceDueDateURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.SearchInvoice();
                    $scope.CloseEditInvoiceDueDateModal();
                }
                ShowMessages(response);
            });
        }
        $('#EditInvoiceDueDateModal').modal('hide');
    }
    
    $scope.InvoiceListPager.getDataCallback = $scope.GetInvoiceList;
    $scope.InvoiceListPager.getDataCallback();
    
};

controllers.InvoiceListController.$inject = ['$scope', '$http', '$timeout', '$window'];

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


   
