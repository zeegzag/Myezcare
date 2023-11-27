var custModel;

controllers.ComplianceController = function ($scope, $http, $timeout, $window, $filter) {
    custModel = $scope;
    $scope.arrowShow = false;
    $scope.OnlyFolder = false;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnComplianceModel").val());
    };
    $scope.ComplianceModel = $.parseJSON($("#hdnComplianceModel").val());
    $scope.Compliance = $scope.ComplianceModel.Compliance;
    $scope.SubSectionList = $scope.ComplianceModel.SubSectionList;
    $scope.ConfigEBFormModel = $scope.ComplianceModel.ConfigEBFormModel;

    $scope.OnUserTypeChange = function (userType) {
        if (userType == 1)
            $scope.Compliance.DocumentationType = 1;
        HideErrors($("#frmCompliance"));
    }

    $scope.RolesList = $scope.ComplianceModel.RolesList;
    $scope.SelectedRole = [];
    $scope.RoleSettings = {
        smartButtonMaxItems: 2,
        requiredMessage: "Role is required",
    };

    $scope.ComplianceList = [];
    $scope.SelectedComplianceIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.SearchComplianceListPage = $scope.ComplianceModel.SearchComplianceListPage;
    $scope.TempSearchComplianceListPage = $scope.ComplianceModel.SearchComplianceListPage;
    $scope.TempSearchComplianceListPage.ShowToAll = '-1';

    $scope.validationSelectRoles = function () {
        if ($scope.SelectedRole.length > 0) {
            $(".multiselectcheckbox").removeClass("input-validation-error").addClass("valid");
            return true;
        }
        else {
            $(".multiselectcheckbox").removeClass("valid");
            $(".multiselectcheckbox").attr("data-original-title", "Role is required").attr("data-html", "true")
                .addClass("tooltip-danger input-validation-error")
                .tooltip({ html: true });
            return false;
        }
    }

    $scope.SaveCompliance = function () {
        var isValid = CheckErrors($("#frmCompliance"));
        var isValidRole = $scope.validationSelectRoles();
        if (isValid && isValidRole) {
            if ($scope.SelectedRole) {
                var RoleValues = $scope.SelectedRole;
                $scope.Compliance.SelectedRoles = Object.keys(RoleValues).map(function (k) { return RoleValues[k]["Value"] }).join(",");
            }
            else {
                $scope.Compliance.SelectedRoles = null;
            }
            var jsonData = angular.toJson($scope.Compliance);
            AngularAjaxCall($http, HomeCareSiteUrl.SaveComplianceURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    $scope.Cancel();
                    ShowMessages(response);
                    $scope.GetComplianceList();
                });
        }
    };
    //$scope.Sorting = function (index) {

    //    console.log(index);

    //    $scope.ComplianceList.splice(index, 1);
    //    var model = [];

    //    angular.forEach($scope.ComplianceList, function (val, index) {
    //        val.SortingID = index + 1;
    //        model.push(val);
    //    })
    //    var jsonData = angular.toJson({ model });
    //    $http.post('/hc/Compliance/SaveTaskOrder', jsonData).then(function (response) {
    //        //you can write more code here after save new order 
    //    }).finally(function () {
    //        model.IsProcessing = false;
    //    })
    //}
    $scope.GetAssigneeList = function () {
        let jsonData = angular.toJson({ UserType: $scope.Compliance.UserType });
        AngularAjaxCall($http, HomeCareSiteUrl.GetAssigneeList, jsonData, "post", "json", "application/json", true).
            success(function (response) {
                $scope.ComplianceModel.AssigneeList = response;
            });
    };

    $scope.GetDirectoryList = function (item) {
        if (item != undefined || item != null) {
            $scope.Compliance.UserType = item;
        }
        let jsonData = angular.toJson({ UserType: $scope.Compliance.UserType });
        AngularAjaxCall($http, HomeCareSiteUrl.GetDirectoryListURL, jsonData, "post", "json", "application/json", true).
            success(function (response) {
                $scope.ComplianceModel.DirectoryList = response;
            });
    };

    $scope.Cancel = function () {
        HideErrors($("#frmCompliance"));
        $scope.Compliance = $scope.newInstance().Compliance;
        $scope.Compliance.IsTimeBased = "0";
        $scope.SelectedRole = [];
    };

    $scope.EditCompliance = function (data) {
        $scope.GetAssigneeList(data.UserType);
        $scope.GetDirectoryList(data.UserType);
        //console.log(data)
        //$scope.Compliance.Assignee = $()
        $scope.Compliance = angular.copy(data);
        //$scope.Compliance.ComplianceID = data.ComplianceID;
        //$scope.Compliance.UserType = data.UserType;
        //$scope.Compliance.DocumentationType = data.DocumentationType;
        //$scope.Compliance.DocumentName = data.DocumentName;
        //$scope.Compliance.SectionID = data.SectionID;
        //$scope.Compliance.SubSectionID = data.SubSectionID;
        //$scope.Compliance.EBFormID = data.EBFormID;
        //$scope.Compliance.FormName = data.FormName;
        //$scope.Compliance.NameForUrl = data.NameForUrl;
        //$scope.Compliance.Version = data.Version;
        $("html, body").animate({ scrollTop: $(".page-title").offset().top }, "slow");

        if ($scope.Compliance.SelectedRoles != null) {
            var RoleValues = [];
            $scope.SelectedRole = [];
            RoleValues = $scope.Compliance.SelectedRoles.split(",");
            angular.forEach(RoleValues, function (value, key) {
                $scope.SelectedRole.push({ "Value": parseInt(value) });
            });
        }

        if (data.IsTimeBased == true)
            $scope.Compliance.IsTimeBased = "1";
        else
            $scope.Compliance.IsTimeBased = "0";
        if (data.ShowToAll == true)
            $scope.Compliance.ShowToAll = "1";
        else
            $scope.Compliance.ShowToAll = "0";

        //$scope.GetDirectoryList();
        HideErrors($("#frmCompliance"));
    };

    $scope.ComplianceListPager = new PagerModule("SortingID", "", "DESC");

    $scope.SetPostData = function (fromIndex) {
        if ($scope.OnlyFolder == true) {
            $scope.SearchComplianceListPage.Type = "Directory";
        }
        var pagermodel = {
            SearchComplianceListPage: $scope.SearchComplianceListPage,
            pageSize: $scope.ComplianceListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ComplianceListPager.sortIndex,
            sortDirection: $scope.ComplianceListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchComplianceListPage = $.parseJSON(angular.toJson($scope.TempSearchComplianceListPage));
    };

    $scope.GetComplianceList = function (isSearchDataMappingRequire) {

        //Reset Selcted Checkbox items and Control
        $scope.SelectedComplianceIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchComplianceListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.ComplianceListPager.currentPage);

        AngularAjaxCall($http, HomeCareSiteUrl.GetComplianceListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ComplianceList = response.Data.Items;
                $scope.ComplianceListPager.currentPageSize = response.Data.Items.length;
                $scope.ComplianceListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        $scope.ComplianceListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            $scope.SearchComplianceListPage = $scope.newInstance().SearchComplianceListPage;
            $scope.TempSearchComplianceListPage = $scope.newInstance().SearchComplianceListPage;
            $scope.TempSearchComplianceListPage.IsDeleted = "0";
            $scope.TempSearchComplianceListPage.UserType = "-1";
            $scope.TempSearchComplianceListPage.DocumentationType = "-1";
            $scope.TempSearchComplianceListPage.IsTimeBased = "-1";
            $scope.TempSearchComplianceListPage.ShowToAll = "-1";
            $scope.TempSearchComplianceListPage.SectionID = "0";
            $scope.TempSearchComplianceListPage.SubSectionID = "0";
            $scope.ComplianceListPager.currentPage = 1;
            $scope.ComplianceListPager.getDataCallback();
        });
        $scope.arrowShow = false;
        $scope.OnlyFolder = false;
    };

    $scope.SearchCompliance = function () {


        $scope.ComplianceListPager.currentPage = 1;
        $scope.ComplianceListPager.getDataCallback(true);
    };

    $scope.SelectCompliance = function (Compliance) {
        if (Compliance.IsChecked)
            $scope.SelectedComplianceIds.push(Compliance.ComplianceID);
        else
            $scope.SelectedComplianceIds.remove(Compliance.ComplianceID);

        if ($scope.SelectedComplianceIds.length == $scope.ComplianceListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    $scope.IsEditable = function (item) {
        return !NotEditableComplianceIDs.includes(item.ComplianceID) && !NotEditableComplianceIDs.includes(item.ParentID);
    };

    $scope.SelectAll = function () {
        $scope.SelectedComplianceIds = [];

        angular.forEach($scope.ComplianceList, function (item, key) {
            if ($scope.IsEditable(item)) {
                item.IsChecked = $scope.SelectAllCheckbox;
                if (item.IsChecked)
                    $scope.SelectedComplianceIds.push(item.ComplianceID);
            }
        });
        return true;
    };

    $scope.DeleteCompliance = function (Data, title) {
        var Message = "";
        if (Data != undefined) {
            var ComplianceId = Data.ComplianceID;
            Message = (title == "Disable" && Data.DocumentCount > 0) ? window.DeleteConfirmationMessageWithNote : window.DeleteConfirmationMessage;
        } else {
            Message = window.DeleteConfirmationMessageWithNote;
        }
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchComplianceListPage.ListOfIdsInCsv = ComplianceId > 0 ? ComplianceId.toString() : $scope.SelectedComplianceIds.toString();

                if (ComplianceId > 0) {
                    if ($scope.ComplianceListPager.currentPage != 1)
                        $scope.ComplianceListPager.currentPage = $scope.ComplianceList.length === 1 ? $scope.ComplianceListPager.currentPage - 1 : $scope.ComplianceListPager.currentPage;
                } else {

                    if ($scope.ComplianceListPager.currentPage != 1 && $scope.SelectedComplianceIds.length == $scope.ComplianceListPager.currentPageSize)
                        $scope.ComplianceListPager.currentPage = $scope.ComplianceListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedComplianceIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.ComplianceListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteComplianceURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.ComplianceList = response.Data.Items;
                        $scope.ComplianceListPager.currentPageSize = response.Data.Items.length;
                        $scope.ComplianceListPager.totalRecords = response.Data.TotalItems;
                    }
                });
            }
        }, bootboxDialogType.Confirm, title, Message, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    $scope.OpenMapFormModel = function () {
        AngularAjaxCall($http, HomeCareSiteUrl.GetOrganizationFormListURL, null, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.OrganizationFormList = response.Data;
                //$scope.OrganizationFormList = response.Data.OrganizationFormList;
                //$scope.ConfigEBFormModel = response.Data.ConfigEBFormModel;
            }
        });
        $('#mapFormModal').modal('show');
    }

    $('#mapFormModal').on('hidden.bs.modal', function () {
        $scope.frmSearch = null;
    });

    $scope.OpenNewHtmlForm = function (item) {
        if (item.IsOrbeonForm) {

            var newURL = HomeCareSiteUrl.OrbeonLoadHtmlFormURL
                + "?FormURL=" + encodeURIComponent(item.NameForUrl)
                + "/view?form-version=" + item.Version
                + "&OrgPageID=" + "FormsLibrary"
                + "&IsEditMode=" + "false"
                + "&EmployeeID=0" //+ $scope.TempSearchFormModel.EmployeeID
                + "&ReferralID=0" //+ $scope.TempSearchFormModel.ReferralID
                + "&FormId=" + item.FormId
                + "&OrganizationId=" + window.OrgID
                + "&UserId=" + window.LUserId
                + "&AppName=" + item.NameForUrl
                + "&FormName=" + item.FormName

                + "&FormPath=" + item.NameForUrl;
            console.log(newURL);

            $scope.ChildWindow = window.open(newURL, "_blank", "width=" + screen.availWidth + ",height=" + screen.availHeight);


        }
        else if (item.IsInternalForm) {

            var path = HomeCareSiteUrl.LoadHtmlFormURL;
            if (item.InternalFormPath.indexOf('.pdf') !== -1) {
                path = window.MyezcarePdfFormsUrl; //'http://localhost:58997/pdfform/LoadPdfForm'; //HomeCareSiteUrl.LoadPdfFormURL;
            }



            if (!ValideElement(item.EBriggsFormID)) { item.EBriggsFormID = "0" };

            var newURL = path
                + "?FormURL=" + encodeURIComponent(item.InternalFormPath)
                + "&OrgPageID=" + "FormsLibrary"
                + "&IsEditMode=" + "false"
                + "&EmployeeID=0" //+ $scope.TempSearchFormModel.EmployeeID
                + "&ReferralID=0" //+ $scope.TempSearchFormModel.ReferralID
                + "&EBriggsFormID=" + item.EBriggsFormID
                + "&OriginalEBFormID=" + item.EBFormID
                + "&FormId=" + item.FormId
                + "&OrganizationId=" + window.OrgID
                + "&UserId=" + window.LUserId
                + "&EbriggsFormMppingID=0";
            $scope.ChildWindow = window.open(newURL, "_blank", "width=" + screen.availWidth + ",height=" + screen.availHeight);



        }
        else {
            var data = $scope.ConfigEBFormModel;
            var newFormUrl = data.EBBaseSiteUrl + "/form/" + item.NameForUrl + "?version=" + item.Version;// + "&tenantGuid=" + response.tenantGuid;
            var newURL = data.MyezcareFormsUrl + "?formURL=" + encodeURIComponent(newFormUrl);
            $scope.ChildWindow = window.open(newURL, "_blank", "width=" + screen.availWidth + ",height=" + screen.availHeight);
        }
    }

    $scope.SelectForm = function (item) {
        $scope.Compliance.EBFormID = item.EBFormID;
        $scope.Compliance.FormName = item.FormLongName;
        $scope.Compliance.NameForUrl = item.NameForUrl;
        $scope.Compliance.Version = item.Version;
        $('#mapFormModal').modal('hide');
        $scope.frmSearch = null;
    }

    $scope.RemoveMappedForm = function () {
        $scope.Compliance.EBFormID = null;
    }

    $scope.ComplianceListPager.getDataCallback = $scope.GetComplianceList;
    $scope.ComplianceListPager.getDataCallback();

    $scope.OpenModel = function () {
        $scope.OnlyFolder = true;
        $('#ChangeDisplayOrderModel').modal({
            backdrop: 'static',
            keyboard: false
        });
        $scope.arrowShow = true;
    }
    $scope.hide = function () {
        $scope.done = true;
    };
    $scope.OnCloseChangeDisplayOrderModel = function () {
        $('#ChangeDisplayOrderModel').modal('hide');
        $scope.OnlyFolder = false;
    }

    var move = function (origin, destination, ComplianceID) {
        console.log(origin, destination);
        var jsonData = angular.toJson({ originID: origin, destinationID: destination, ComplianceID: ComplianceID });
        AngularAjaxCall($http, HomeCareSiteUrl.ChangeSortingOrderURL, jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                ShowMessages(response);
                $scope.GetComplianceList();
            });
    };

    $scope.moveUp = function (index, Compliance) {
        Preindex = index - 1;
        var SortingID = $scope.ComplianceList[Preindex].SortingID;
        move(Compliance.SortingID, SortingID, Compliance.ComplianceID);

    };

    $scope.moveDown = function (index, Compliance) {
        Nextindex = index + 1;
        var SortingID = $scope.ComplianceList[Nextindex].SortingID;
        move(Compliance.SortingID, SortingID, Compliance.ComplianceID);

    };


};

controllers.ComplianceController.$inject = ['$scope', '$http', '$timeout', '$window', '$filter'];

$(document).ready(function () {
});