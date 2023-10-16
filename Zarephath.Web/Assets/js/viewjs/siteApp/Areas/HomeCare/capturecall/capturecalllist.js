var vm;
controllers.CaptureCallListController = function ($scope, $http, $timeout) {
    vm = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetCaptureCallListPage").val());
    };

    $scope.AddCaptureCallURL = HomeCareSiteUrl.AddCaptureCallURL;
    $scope.CaptureCallList = [];
    $scope.SelectedCaptureCallIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.CaptureCallModel = $.parseJSON($("#hdnSetCaptureCallListPage").val());
    $scope.SearchCaptureCallListPage = $scope.CaptureCallModel.SearchCaptureCallListPage;
    $scope.TempSearchCaptureCallListPage = $scope.CaptureCallModel.SearchCaptureCallListPage;
    $scope.CaptureCallListPager = new PagerModule("Id", "", "DESC");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            SearchCaptureCallListPage: $scope.SearchCaptureCallListPage,
            pageSize: $scope.CaptureCallListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.CaptureCallListPager.sortIndex,
            sortDirection: $scope.CaptureCallListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchCaptureCallListPage = $.parseJSON(angular.toJson($scope.TempSearchCaptureCallListPage));

    };

    $scope.GetCaptureCallList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedCaptureCallIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchCaptureCallListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.CaptureCallListPager.currentPage);

        AngularAjaxCall($http, HomeCareSiteUrl.GetCaptureCallList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.CaptureCallList = response.Data.Items;
                $scope.CaptureCallListPager.currentPageSize = response.Data.Items.length;
                $scope.CaptureCallListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        //$scope.ResetSearchFilter();
        //$scope.CaseManagerListPager.currentPage = $scope.CaseManagerListPager.currentPage;
        $scope.CaptureCallListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');
            $scope.SearchCaptureCallListPage = $scope.newInstance().SearchCaptureCallListPage;
            $scope.TempSearchCaptureCallListPage = $scope.newInstance().SearchCaptureCallListPage;
            $scope.TempSearchCaptureCallListPage.IsDeleted = "0";
            $scope.CaptureCallListPager.currentPage = 1;
            $scope.CaptureCallListPager.getDataCallback();
        });
    };
    $scope.SearchCaptureCall = function () {
        $scope.CaptureCallListPager.currentPage = 1;
        $scope.CaptureCallListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectCaptureCall = function (CaptureCall) {
        if (CaptureCall.IsChecked)
            $scope.SelectedCaptureCallIds.push(CaptureCall.Id);
        else
            $scope.SelectedCaptureCallIds.remove(CaptureCall.Id);

        if ($scope.SelectedCaptureCallIds.length == $scope.CaptureCallListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedCaptureCallIds = [];

        angular.forEach($scope.CaptureCallList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedCaptureCallIds.push(item.Id);
        });
        return true;
    };

    $scope.DeleteCaptureCall = function (Id, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchCaptureCallListPage.ListOfIdsInCsv = Id > 0 ? Id.toString() : $scope.SelectedCaptureCallIds.toString();

                if (Id > 0) {
                    if ($scope.CaptureCallListPager.currentPage != 1)
                        $scope.CaptureCallListPager.currentPage = $scope.CaptureCallList.length === 1 ? $scope.CaptureCallListPager.currentPage - 1 : $scope.CaptureCallListPager.currentPage;
                } else {

                    if ($scope.CaptureCallListPager.currentPage != 1 && $scope.SelectedCaptureCallIds.length == $scope.CaptureCallListPager.currentPageSize)
                        $scope.CaptureCallListPager.currentPage = $scope.CaptureCallListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedCaptureCallIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.CaptureCallListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteCaptureCall, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.CaptureCallList = response.Data.Items;
                        $scope.CaptureCallListPager.currentPageSize = response.Data.Items.length;
                        $scope.CaptureCallListPager.totalRecords = response.Data.TotalItems;
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

    $scope.CaptureCallListPager.getDataCallback = $scope.GetCaptureCallList;
    $scope.CaptureCallListPager.getDataCallback();


    $scope.CaptureCallEditModel = function (EncryptedId, title) {
        var EncryptedId = EncryptedId;
        $('#CaptureCall_fixedAside').modal({ backdrop: 'static', keyboard: false });;
        $('#CaptureCall_fixedAsidelDDLBindIFrame').attr('src', HomeCareSiteUrl.PartialAddCaptureCallURL + EncryptedId);
    }
    $scope.CaptureCallEditModelClosed = function () {
        $scope.Refresh();
        $('#CaptureCall_fixedAside').modal('hide');
    }

    $scope.EditOrbeonForm = function (item, mode, isClone) {
        if (mode != '') {
            $scope.Document = {};
            $scope.Document.ReferralDocumentID = item.Id;
            $scope.Document.Edit = true;
            $scope.Document.Clone = isClone;
            $scope.frameLoader = true;
            $scope.ShowFrame = true;
            $scope.ShowFormNameModal = false;

            var dupForm = { IsDuplicate: isClone, DocumentID: item.OrbeonID, NameForUrl: item.FilePath };
            $scope.DuplicateOrbeonForm(dupForm, (data) => {
                var EmployeeID = $scope.EmployeeID == undefined ? 0 : $scope.EmployeeID;
                var ReferralID = $scope.ReferralID == undefined ? 0 : $scope.ReferralID;

                var formName = item.FileName == undefined ? null : item.FileName;
                var urlToGet = HomeCareSiteUrl.OrbeonLoadHtmlFormURL + '?FormURL=' + encodeURIComponent(item.FilePath);
                var newURL = urlToGet + '/' + mode + '/' + item.OrbeonID
                    + "?orbeon-embeddable=true"
                    + "&OrgPageID=" + "ReferralDocument"//window.ReferralDocumentPageId
                    + "&IsEditMode=" + "true"
                    // + "&EmployeeID=" + "0"// EmployeeID
                    // + "&ReferralID=" + "0"// ReferralID
                    //   + "&FormId=" + "20019" //item.FormId
                    // + "&UserType=" + "a"//$scope.UserType
                    // + "&SubSectionID=" + $scope.ComplianceID
                    + "&FormName=" + formName
                    + "&OrganizationId=" + "11"// window.OrgID
                    //  + "&UserId=" + "1"// window.LUserId
                    ;

                //document.getElementById('myIframe').src = newURL;
                var width = screen.availWidth - 10;
                var height = screen.availHeight - 60;
                var left = 0;
                var top = 0;
                var params = 'width=' + width + ', height=' + height;
                params += ', top=' + top + ', left=' + left;
                var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,' + params;
                var pdfWindow = window.open('-', 'null', winFeature);
                pdfWindow.document.write("<html><head><style> * { box-sizing: border-box; padding: 0; margin: 0; border: 0;}</style>"
                    + "<title>" + "Forms" + "</title></head><body>"
                    + '<embed width="100%" height="100%" name="plugin" src="' + newURL + '" '
                    + 'type="application/pdf" internalinstanceid="21"></body></html>'
                );

                pdfWindow.document.close();
            });

        }
    };
    $scope.DuplicateOrbeonForm = function (item, successCallback) {
        let myFirstPromise = new Promise((resolve) => {
            if (item) {
                if (!item.IsDuplicate) {
                    resolve(item);
                } else {
                    var jsonData = angular.toJson(item);
                    AngularAjaxCall($http, HomeCareSiteUrl.DuplicateOrbeonFormURL, jsonData, "Post", "json", "application/json").success(function (response) {
                        if (response.IsSuccess) {
                            item.DocumentID = response.Data.DocumentID;
                            resolve(item);
                        } else {
                            ShowMessages(response);
                        }
                    });
                }
            }
        })
        myFirstPromise.then((data) => {
            successCallback(data);
        });
    };
    
};

controllers.CaptureCallListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowCaptureCallMessage");
});
