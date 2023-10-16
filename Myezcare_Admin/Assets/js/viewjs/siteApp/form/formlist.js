var vm;
controllers.FormListController = function ($scope, $http, $window, $timeout) {
    vm = $scope;


    $scope.newInstance = function () {
        return $.parseJSON($("#hdnFormListPage").val());
    };
    $scope.SetFormListPageModel = $scope.newInstance();

    $scope.FormList = [];
    $scope.SelectedFormIds = [];
    $scope.SelectAllCheckbox = false;


    $scope.SearchFormModel = $scope.newInstance().SearchFormModel;
    $scope.TempSearchFormModel = $scope.newInstance().SearchFormModel;
    $scope.FormListPager = new PagerModule("EBFormID");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            model: $scope.SearchFormModel,
            pageSize: $scope.FormListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.FormListPager.sortIndex,
            sortDirection: $scope.FormListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };
    $scope.SearchModelMapping = function () {
        $scope.SearchFormModel = $.parseJSON(angular.toJson($scope.TempSearchFormModel));
    };

    $scope.GetFormList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedFormIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchFormModel.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();

        var jsonData = $scope.SetPostData($scope.FormListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetFormListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.FormList = response.Data.Items;
                $scope.FormListPager.currentPageSize = response.Data.Items.length;
                $scope.FormListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.FormListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        //Reset 
        $scope.SearchFormModel = $scope.newInstance().SearchFormModel;
        $scope.TempSearchFormModel = $scope.newInstance().SearchFormModel;
        $scope.TempSearchFormModel.IsDeleted = "0";
        $scope.FormListPager.currentPage = 1;
        $scope.FormListPager.getDataCallback();
    };

    $scope.SearchFormList = function () {
        $scope.FormListPager.currentPage = 1;
        $scope.FormListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectForm = function (formList) {
        if (formList.IsChecked)
            $scope.SelectedFormIds.push(formList.EBFormID);
        else
            $scope.SelectedFormIds.remove(formList.EBFormID);

        if ($scope.SelectedFormIds.length == $scope.FormListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };
    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedFormIds = [];
        angular.forEach($scope.FormList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedFormIds.push(item.EBFormID);
        });
        return true;
    };

    $scope.FormListPager.getDataCallback = $scope.GetFormList;
    $scope.FormListPager.getDataCallback();





    $scope.DeleteForm = function (form, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }

        var ebFormId = ValideElement(form) ? form.EBFormID : undefined;
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchFormModel.ListOfIdsInCSV = ValideElement(ebFormId) ? ebFormId.toString() : $scope.SelectedFormIds.toString();
                if (ValideElement(ebFormId)) {
                    if ($scope.FormListPager.currentPage != 1)
                        $scope.FormListPager.currentPage = $scope.FormList.length === 1 ? $scope.FormList.currentPage - 1 : $scope.FormList.currentPage;
                }
                else {
                    if ($scope.FormListPager.currentPage != 1 && $scope.FormListPager.length == $scope.FormListPager.currentPageSize)
                        $scope.FormListPager.currentPage = $scope.FormListPager.currentPage - 1;
                }
                //Reset Selcted Checkbox items and Control
                $scope.SelectedFormIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.FormListPager.currentPage);

                AngularAjaxCall($http, SiteUrl.DeleteFormURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.FormList = response.Data.Items;
                        $scope.FormListPager.currentPageSize = response.Data.Items.length;
                        $scope.FormListPager.totalRecords = response.Data.TotalItems;
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.EditFormPrice = function (data, id) {
        $scope.TempFormPrice = data.FormPrice;
        data.IsEditable = true;
        if (id) {
            $timeout(function () {
                $(id).focus();
            }, 100);
        }

    }

    $scope.SaveFormPrice = function (item, id) {
        if (id) {
            $(id).focusout();
            $timeout(function () {

                if (!ValideElement(item.FormPrice)) {
                    toastr.error(window.CanNotSave);
                    return false;
                }

                if (id && $(id).hasClass('input-validation-error')) {
                    toastr.error(window.CanNotSave);
                    return false;
                }

                var jsonData = angular.toJson(item);
                AngularAjaxCall($http, SiteUrl.UpdateFormPriceUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        item.IsEditable = false;
                    }
                    ShowMessages(response);
                });
            }, 100);
        }


    }

    $scope.CancelFormPrice = function (data) {
        data.FormPrice = $scope.TempFormPrice;
        data.IsEditable = false;
    }




    $scope.SyncFormList = function () {
        AngularAjaxCall($http, SiteUrl.SyncFormListURL, null, "POST", "json", "application/json").success(function (response) {
            $scope.FormListPager.getDataCallback();
            ShowMessages(response);
        });
    };

    $scope.OpenNewHtmlForm = function (item) {
        debugger;

        if (item.IsInternalForm) {
            
            var newURL = "/form/loadhtmlform" + "?formURL=" + encodeURIComponent(item.InternalFormPath);
            $scope.ChildWindow = window.open(newURL, "_blank", "width=" + screen.availWidth + ",height=" + screen.availHeight);
        }
        else {
            var data = $scope.newInstance().ConfigEBFormModel;
            var newFormUrl = data.EBBaseSiteUrl + "/form/" + item.NameForUrl + "?version=" + item.Version;// + "&tenantGuid=" + response.tenantGuid;
            var newURL = data.MyezcareFormsUrl + "?formURL=" + encodeURIComponent(newFormUrl);
            $scope.ChildWindow = window.open(newURL, "_blank", "width=" + screen.availWidth + ",height=" + screen.availHeight);
        }
    }



};
controllers.FormListController.$inject = ['$scope', '$http', '$window', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("ServicePlanUpdateSuccessMessage");
});
