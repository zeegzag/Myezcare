var vm;
controllers.FormListController = function ($scope, $http, $window, $timeout) {
    vm = $scope;

    
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnFormListPage").val());
    };
    $scope.SetFormListPageModel = $scope.newInstance();


    //#region Form list Page

    $scope.FormList = [];
    $scope.SelectedFormIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.SearchFormModel = $scope.newInstance().SearchFormModel;
    $scope.TempSearchFormModel = $scope.newInstance().SearchFormModel;
    $scope.FormListPager = new PagerModule("FormLongName");

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
        AngularAjaxCall($http, HomeCareSiteUrl.GetFormListURL, jsonData, "Post", "json", "application/json").success(function (response) {
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

    if (!$scope.newInstance().IsPartial)
        $scope.FormListPager.getDataCallback();

    $('a#ebForms_newFormList,a#ebForms').on('shown.bs.tab', function () {
        $(".tab-pane a[href='#newFormList']").tab('show');
        $scope.ResetSearchFilter();
    });

    $("a#ebForms_savedFormList").on('shown.bs.tab', function (e) {
        $scope.ResetSavedFormSearchFilter();
    });

    $scope.OpenNewHtmlForm = function (item) {
        if (item.IsInternalForm) {

            var path = HomeCareSiteUrl.LoadHtmlFormURL;
            if (item.InternalFormPath.indexOf('.pdf') !== -1) {
                path = window.MyezcarePdfFormsUrl; //'http://localhost:58997/pdfform/LoadPdfForm'; //HomeCareSiteUrl.LoadPdfFormURL;
            }

            if(!ValideElement(item.EBriggsFormID)) { item.EBriggsFormID = "0"};

            var newURL = path
                + "?FormURL=" + encodeURIComponent(item.InternalFormPath)
                + "&OrgPageID=" + "ReferralForm"
                + "&IsEditMode=" + "true"
                + "&EmployeeID=" + $scope.TempSearchFormModel.EmployeeID
                + "&ReferralID=" + $scope.TempSearchFormModel.ReferralID
                + "&EBriggsFormID=" + item.EBriggsFormID
                + "&OriginalEBFormID=" + item.EBFormID
                + "&FormId=" + item.FormId
                + "&OrganizationId=" + window.OrgID
                + "&UserId=" + window.LUserId
                + "&EbriggsFormMppingID=0";
            $scope.ChildWindow = window.open(newURL, "_blank", "width=" + screen.availWidth + ",height=" + screen.availHeight);
        }
        else {
            var data = $scope.newInstance().ConfigEBFormModel;
            var newFormUrl = data.EBBaseSiteUrl + "/form/" + item.NameForUrl + "?version=" + item.Version + "&PageId=" + window.ReferralFormPageId;// + "&tenantGuid=" + response.tenantGuid;
            var newURL = data.MyezcareFormsUrl + "?formURL=" + encodeURIComponent(newFormUrl);
            $scope.ChildWindow = window.open(newURL, "_blank", "width=" + screen.availWidth + ",height=" + screen.availHeight);
        }

        //var jsonData = angular.toJson({ "username": data.ResuName, "password": data.ResuKey });
        //var loginUrl = data.EBBaseSiteUrl + "/auth/login";
        //CrossDomainAngularAjaxCall(loginUrl, jsonData, "POST", "json", "application/json").success(function (response, result) {
        //    if (result === "success") {
        //        var newFormUrl = data.EBBaseSiteUrl + "/form/" + item.NameForUrl + "?version=" + item.Version + "&tenantGuid=" + response.tenantGuid;
        //        $scope.ChildWindow = window.open(newFormUrl, "_blank", "width=" + screen.availWidth + ",height=" + screen.availHeight);
        //    }
        //});
    }
    $scope.OpenNewPDFForm = function (item) {
        var data = $scope.newInstance().ConfigEBFormModel;
        var newFormUrl = data.EBBaseSiteUrl + "/pdf/" + item.NameForUrl + "?version=" + item.Version;// + "&tenantGuid=" + response.tenantGuid;
        var newURL = data.MyezcareFormsUrl + "?formURL=" + encodeURIComponent(newFormUrl);
        $scope.ChildWindow = window.open(newURL, "_blank", "width=" + screen.availWidth + ",height=" + screen.availHeight);


        //var jsonData = angular.toJson({ "username": data.ResuName, "password": data.ResuKey });
        //var loginUrl = data.EBBaseSiteUrl + "/auth/login";
        //CrossDomainAngularAjaxCall(loginUrl, jsonData, "POST", "json", "application/json").success(function (response, result) {
        //    if (result === "success") {
        //        var newFormUrl = data.EBBaseSiteUrl + "/pdf/" + item.NameForUrl + "?version=" + item.Version + "&tenantGuid=" + response.tenantGuid;
        //        window.location = newFormUrl;
        //    }
        //});
    }

    $scope.SavedNewHtmlForm = function (resData) {
        if ($scope.ChildWindow && $scope.ChildWindow.close) $scope.ChildWindow.close();

        var resObj = $.parseJSON(resData);

        var originalEBFormID = resObj.OrginalFormId;
        var eBriggsFormID = resObj.EBriggsFormID;
        var formId = resObj.FormId;

        var jsonData = angular.toJson({
            "EBriggsFormID": eBriggsFormID,
            "OriginalEBFormID": originalEBFormID,
            "FormId": formId,
            "EmployeeID": $scope.TempSearchFormModel.EmployeeID,
            "ReferralID": $scope.TempSearchFormModel.ReferralID
        });
        AngularAjaxCall($http, HomeCareSiteUrl.SaveNewEBFormURL, jsonData, "Post", "json", "application/json", false).
            success(function (response) {
                ShowMessages(response);
            });
    }

    //#endregion Form list Page


    //#region Saved Form list Page

    $scope.SavedFormList = [];
    $scope.SelectedSavedFormIds = [];
    $scope.SelectAllSavedFormCheckbox = false;

    $scope.SearchSavedFormModel = $scope.newInstance().SearchFormModel;
    $scope.TempSearchSavedFormModel = $scope.newInstance().SearchFormModel;
    $scope.SavedFormListPager = new PagerModule("FormLongName");

    $scope.SetSavedFormPostData = function (fromIndex) {
        var pagermodel = {
            model: $scope.SearchSavedFormModel,
            pageSize: $scope.SavedFormListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.SavedFormListPager.sortIndex,
            sortDirection: $scope.SavedFormListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };
    $scope.SearchSavedFormModelMapping = function () {
        $scope.SearchSavedFormModel = $.parseJSON(angular.toJson($scope.TempSearchSavedFormModel));
    };
    $scope.GetSavedFormList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedSavedFormIds = [];
        $scope.SelectAllSavedFormCheckbox = false;
        $scope.SearchSavedFormModel.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchSavedFormModelMapping();

        var jsonData = $scope.SetSavedFormPostData($scope.FormListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetSavedFormListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.SavedFormList = response.Data.Items;
                $scope.SavedFormListPager.currentPageSize = response.Data.Items.length;
                $scope.SavedFormListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };
    $scope.SavedFormRefresh = function () {
        $scope.SavedFormListPager.getDataCallback();
    };
    $scope.ResetSavedFormSearchFilter = function () {
        //Reset 
        $scope.SearchSavedFormModel = $scope.newInstance().SearchFormModel;
        $scope.TempSearchSavedFormModel = $scope.newInstance().SearchFormModel;
        $scope.TempSearchSavedFormModel.IsDeleted = "0";
        $scope.SavedFormListPager.currentPage = 1;
        $scope.SavedFormListPager.getDataCallback();
    };
    $scope.SearchSavedFormList = function () {
        $scope.SavedFormListPager.currentPage = 1;
        $scope.SavedFormListPager.getDataCallback(true);
    };
    $scope.SelectSavedForm = function (formList) {
        if (formList.IsChecked)
            $scope.SelectedSavedFormIds.push(formList.EBFormID);
        else
            $scope.SelectedSavedFormIds.remove(formList.EBFormID);

        if ($scope.SelectedSavedFormIds.length == $scope.FormListPager.currentPageSize)
            $scope.SelectAllSavedFormCheckbox = true;
        else
            $scope.SelectAllSavedFormCheckbox = false;
    };
    $scope.SelectAllSavedForm = function () {
        $scope.SelectedSavedFormIds = [];
        angular.forEach($scope.SavedFormList, function (item, key) {
            item.IsChecked = $scope.SelectAllSavedFormCheckbox;
            if (item.IsChecked)
                $scope.SelectedSavedFormIds.push(item.EBFormID);
        });
        return true;
    };

    $scope.SavedFormListPager.getDataCallback = $scope.GetSavedFormList;

    if (!$scope.newInstance().IsPartial)
        $scope.SavedFormListPager.getDataCallback();

    $scope.OpenSavedHtmlForm = function (item) {

        if (item.IsInternalForm) {

            var path = HomeCareSiteUrl.LoadHtmlFormURL;
            //if (item.InternalFormPath.indexOf('.pdf') !== -1) {
            //    path = HomeCareSiteUrl.LoadPdfFormURL;
            //}

            if (item.InternalFormPath.indexOf('.pdf') !== -1) {
                path = window.MyezcarePdfFormsUrl; //'http://localhost:58997/pdfform/LoadPdfForm'; //HomeCareSiteUrl.LoadPdfFormURL;
            }

            if (!ValideElement(item.EBriggsFormID)) { item.EBriggsFormID = "0" };
            var newURL = path
                + "?FormURL=" + encodeURIComponent(item.InternalFormPath)
                + "&OrgPageID=" + "ReferralForm"
                + "&IsEditMode=" + "true"
                + "&EmployeeID=" + $scope.TempSearchFormModel.EmployeeID
                + "&ReferralID=" + $scope.TempSearchFormModel.ReferralID
                + "&EBriggsFormID=" +item.EBriggsFormID
                + "&OriginalEBFormID=" + item.EBFormID
                + "&FormId=" + item.FormId
                + "&OrganizationId="+ window.OrgID
                + "&UserId="+ window.LUserId
                + "&EbriggsFormMppingID=" + item.EbriggsFormMppingID;
            $scope.ChildWindow = window.open(newURL, "_blank", "width=" + screen.availWidth + ",height=" + screen.availHeight);
        }
        else {

            var data = $scope.newInstance().ConfigEBFormModel;
            var newFormUrl = data.EBBaseSiteUrl + "/form/" + item.NameForUrl + "?version=" + item.Version + "&id=" + item.EBriggsFormID + "&PageId=" + window.ReferralFormPageId;
            var newURL = data.MyezcareFormsUrl + "?formURL=" + encodeURIComponent(newFormUrl);
            $scope.ChildWindow = window.open(newURL, "_blank", "width=" + screen.availWidth + ",height=" + screen.availHeight);
        }

    };



    $scope.OpenSavedPDFForm = function (item) {
        var data = $scope.newInstance().ConfigEBFormModel;
        var newFormUrl = data.EBBaseSiteUrl + "/pdf/" + item.NameForUrl + "?version=" + item.Version + "&id=" + item.EBriggsFormID;
        var newURL = data.MyezcareFormsUrl + "?formURL=" + encodeURIComponent(newFormUrl);
        $scope.ChildWindow = window.open(newURL, "_blank", "width=" + screen.availWidth + ",height=" + screen.availHeight);

    };

    //#endregion Saved Form list Page
};
controllers.FormListController.$inject = ['$scope', '$http', '$window', '$timeout'];

$(document).ready(function () {
    var eventMethod = window.addEventListener ? "addEventListener" : "attachEvent";
    var eventer = window[eventMethod];
    var messageEvent = eventMethod == "attachEvent" ? "onmessage" : "message";
    // Listen to message from child window
    eventer(messageEvent, function (e) {
        //console.log('parent received message!:  ', e.data);
        if ((typeof (e.data) === 'string' && e.data.indexOf('ClearFrameCompleted:true') >= 0) || (typeof (e.data) === 'string' && e.data.indexOf('OrbeonID') >= 0)) { }
        else {
            var res = {};
            if (typeof (e.data) === 'string') {
                res = JSON.parse(JSON.stringify(e.data));
            }
            if (ValideElement(e.data) && (typeof (e.data) === 'string' &&  e.data.indexOf('OrginalFormId') > 0) && res.PageID == "ReferralForm")
                vm.SavedNewHtmlForm(e.data);

            if (ValideElement(e.data) && res.OrgPageID == "ReferralForm") {
                if (vm.ChildWindow && vm.ChildWindow.close)
                    vm.ChildWindow.close();
            }
        }

    }, false);
});
