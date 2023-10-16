var vm;
controllers.OrganizationFormsController = function ($scope, $http, $window, $timeout) {
    vm = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnOrganizationFormPage").val());
    };


    $scope.EbFormModel = $scope.newInstance();
    $scope.MarketList = $scope.newInstance().MarketList;
    $scope.FormCategoryList = $scope.newInstance().FormCategoryList;
    $scope.FormList = $scope.newInstance().FormList;
    $scope.OrgFormList = $scope.newInstance().OrganizationFormList;
    $scope.TempSearchFormModel = $scope.newInstance().SearchFormModel;
    $scope.TempSearchOrgFormModel = $scope.newInstance().SearchFormModel;
    $scope.ConfigEBFormModel = $scope.newInstance().ConfigEBFormModel;

    //#region Available Forms

    $scope.FormListPager = new PagerModule();
    $scope.FormListPager.currentPage = 1;
    $scope.FormListPager.pageSize = 1000000;
    $scope.FormListPager.sortIndex = "FormLongName";
    $scope.FormListPager.sortDirection = "ASC";

    $scope.FormListPager.SortClm = function (newPredicate) {
        $scope.FormListPager.reverse = ($scope.FormListPager.sortIndex === newPredicate) ? !$scope.FormListPager.reverse : false;
        $scope.FormListPager.sortIndex = newPredicate !== undefined ? newPredicate : sortIndex;
        $scope.FormListPager.sortDirection = $scope.FormListPager.reverse === true ? "DESC" : "ASC";
    };
    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            $scope.SearchFormModel = $scope.newInstance().SearchFormModel;
            $scope.TempSearchFormModel = $scope.newInstance().SearchFormModel;
            $scope.FormListPager.currentPage = 1;
        });
    }


    $scope.ResultForms = [];
    $scope.SelectedForms = [];
    $scope.SelectAllFormCheckbox = false;
    $scope.SelectForm = function (form) {
        if (form.IsChecked)
            $scope.SelectedForms.push(form);
        else
            $scope.SelectedForms.remove(form);

        //if ($scope.SelectedForms.length == $scope.FormList.length)
        //    $scope.SelectAllFormCheckbox = true;
        //else
        //    $scope.SelectAllFormCheckbox = false;

    };
    $scope.SelectAllForms = function (val) {
        //$scope.SelectedForms = [];
        angular.forEach($scope.ResultForms, function (item, key) {
            item.IsChecked = val;//event.target.checked; //$scope.SelectAllFormCheckbox;//

            if (item.IsChecked) {
                if ($scope.SelectedForms.indexOf(item) == -1)
                    $scope.SelectedForms.push(item);
            } else {
                if ($scope.SelectedForms.indexOf(item) !== -1)
                    $scope.SelectedForms.remove(item);
            }
        });

        return true;
    };

    //#endregion


    //#region Selected Forms

    $scope.OrgFormListPager = new PagerModule();
    $scope.OrgFormListPager.currentPage = 1;
    $scope.OrgFormListPager.pageSize = 1000000;
    $scope.OrgFormListPager.sortIndex = "FormLongName";
    $scope.OrgFormListPager.sortDirection = "ASC";
    $scope.OrgFormListPager.SortClm = function (newPredicate) {
        $scope.OrgFormListPager.reverse = ($scope.OrgFormListPager.sortIndex === newPredicate) ? !$scope.OrgFormListPager.reverse : false;
        $scope.OrgFormListPager.sortIndex = newPredicate != undefined ? newPredicate : sortIndex;
        $scope.OrgFormListPager.sortDirection = $scope.OrgFormListPager.reverse === true ? "DESC" : "ASC";
    };
    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            $scope.SearchOrgFormModel = $scope.newInstance().SearchFormModel;
            $scope.TempSearchOrgFormModel = $scope.newInstance().SearchFormModel;
            $scope.OrgFormListPager.currentPage = 1;
        });
    }


    $scope.ResultOrgForms = [];
    $scope.SelectedOrgForms = [];
    $scope.SelectAllOrgFormCheckbox = false;
    $scope.SelectOrgForm = function (form) {

        if (form.IsChecked)
            $scope.SelectedOrgForms.push(form);
        else
            $scope.SelectedOrgForms.remove(form);

        //if ($scope.SelectedOrgForms.length == $scope.OrgFormList.length)
        //    $scope.SelectAllOrgFormCheckbox = true;
        //else
        //    $scope.SelectAllOrgFormCheckbox = false;

    };
    $scope.SelectAllOrgForms = function (val) {
        //$scope.SelectedOrgForms = [];
        angular.forEach($scope.ResultOrgForms, function (item, key) {
            item.IsChecked = val;//$scope.SelectAllOrgFormCheckbox;// event.target.checked;
            if (item.IsChecked) {
                if ($scope.SelectedOrgForms.indexOf(item) == -1)
                    $scope.SelectedOrgForms.push(item);
            } else {
                if ($scope.SelectedOrgForms.indexOf(item) !== -1)
                    $scope.SelectedOrgForms.remove(item);
            }

        });

        return true;
    };

    //#endregion

    $scope.FormFilterOnPageLoad = function () {
        var filtered = [];
        $.each($scope.OrgFormList, function (index, item) {
            $scope.FormList.filter(function (frmItem, frmIndex) {
                if (frmItem.EBFormID === item.EBFormID) {
                    filtered.push(frmItem);
                    return;
                }
            });
        });

        $.each(filtered, function (index, item) {
            $scope.FormList.remove(item);
        });
    };
    $scope.FormFilterOnPageLoad();

    $scope.MoveFromEbFormToOrgForm = function () {
        $.each($scope.SelectedForms, function (index, item) {
            if ($scope.OrgFormList.indexOf(item) === -1) {
                item.IsChecked = false;
                $scope.OrgFormList.push(item);
                $scope.FormList.remove(item);
            }
        });
        $scope.SelectedForms = [];
        $scope.SelectAllFormCheckbox = false;
        $scope.SelectAllForms();
    };

    $scope.MoveFromOrgFormToEbFrom = function () {
        $.each($scope.SelectedOrgForms, function (index, item) {
            if ($scope.FormList.indexOf(item) === -1) {
                item.IsChecked = false;
                $scope.FormList.push(item);
                $scope.OrgFormList.remove(item);
            }
        });

        $scope.SelectedOrgForms = [];
        $scope.SelectAllOrgFormCheckbox = false;
        $scope.SelectAllOrgForms();
    };


    $scope.SaveOrganizationFormDetails = function () {
        $scope.orgFormList = [];
        $.each($scope.OrgFormList, function (index, item) {
            $scope.orgFormList.push({ "OrganizationFormID": item.OrganizationFormID, "EBFormID": item.EBFormID });
        });


        var jsonData = angular.toJson($scope.orgFormList);
        AngularAjaxCall($http, HomeCareSiteUrl.SaveOrganizationFormDetailsURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                //SetMessageForPageLoad(response.Message, "ShowOrgFormSavedDetails");
                //if (window.IsPartial == 'False')
                //    window.location = HomeCareSiteUrl.OrganizationFormsURL;
                //else
                //    window.location = HomeCareSiteUrl.OrganizationSettingURL;
                toastr.success("Form Library Save Successfully");
            }
            else
                ShowMessages(response);

        });


    };

    $scope.OpenNewHtmlForm = function (item) {
        debugger;
        console.log(item);
        if (item.IsOrbeonForm) {
            //alert('In Progress.....');

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
                + "&FormName=" + item.Name
                
                + "&FormPath=" + item.NameForUrl;
            console.log(newURL);

            $scope.ChildWindow = window.open(newURL, "_blank", "width=" + screen.availWidth + ",height=" + screen.availHeight);


        }
        else if (item.IsInternalForm) {
            //var path = HomeCareSiteUrl.LoadHtmlFormURL;
            //if (item.InternalFormPath.indexOf('.pdf') !== -1) {
            //    path = HomeCareSiteUrl.LoadPdfFormURL;
            //}

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
            var data = $scope.newInstance().ConfigEBFormModel;
            var newFormUrl = data.EBBaseSiteUrl + "/form/" + item.NameForUrl + "?version=" + item.Version;// + "&tenantGuid=" + response.tenantGuid;
            var newURL = data.MyezcareFormsUrl + "?formURL=" + encodeURIComponent(newFormUrl);
            $scope.ChildWindow = window.open(newURL, "_blank", "width=" + screen.availWidth + ",height=" + screen.availHeight);
        }
    }

    $scope.OpenNewPDFForm = function (item) {
        var data = $scope.newInstance().ConfigEBFormModel;
        var newFormUrl = data.EBBaseSiteUrl + "/pdf/" + item.NameForUrl + "?version=" + item.Version;// + "&tenantGuid=" + response.tenantGuid;
        var newURL = data.MyezcareFormsUrl + "?formURL=" + encodeURIComponent(newFormUrl);
        $scope.ChildWindow = window.open(newURL, "_blank", "width=" + screen.availWidth + ",height=" + screen.availHeight);

    }

    $scope.EditFormName = function (data, id) {
        data.OrganizationFriendlyFormName = data.FormLongName;
        data.IsEditable = true;
        if (id) {
            $timeout(function () {
                $(id).focus();
            }, 100);
        }
    }

    $scope.SaveFormName = function (item, id) {
        if (id) {
            $(id).focusout();
            $timeout(function () {

                if (!ValideElement(item.OrganizationFriendlyFormName)) {
                    toastr.error(window.FormNameRequired);
                    return false;
                }

                var jsonData = angular.toJson(item);
                AngularAjaxCall($http, HomeCareSiteUrl.SaveOrganizationFormNameURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        item.IsEditable = false;
                        item.FormLongName = item.OrganizationFriendlyFormName;
                    }
                    ShowMessages(response);
                });
            }, 100);
        }
    }

    $scope.CancelFormName = function (data) {
        //data.FormLongName = $scope.TempFormLongName;
        data.IsEditable = false;
    }

    $scope.OpenFormTagModal = function (data) {
        $scope.OriginalFormName = data.OriginalFormName;
        $scope.OrganizationFormID = data.OrganizationFormID;
        var jsonData = angular.toJson({ id: data.OrganizationFormID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetOrgFormTagListURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.TagList = response.Data;
            }
        });
        $('#formTagModal').modal({
            backdrop: 'static',
            keyboard: false
        });
    }

    //#region ADD Tags
    $scope.TagTokenObj = {};
    $scope.SearchTagURL = HomeCareSiteUrl.SearchTagURL;

    $scope.TagResultsFormatter = function (item) {
        return "<li id='{0}'>{0}</li>".format(item.FormTagName);
    };
    $scope.TagTokenFormatter = function (item) {
        return "<li id='{0}'>{0}</li>".format(item.FormTagName);
    };

    $scope.AddedTag = function (item) {
        //$scope.ListPreference.push(item);
        //$scope.PreferenceTokenObj.clear();

        var push = true;
        if (_.findWhere($scope.TagList, item) == null) {
            angular.forEach($scope.TagList, function (items) {
                if (items.FormTagName == item.FormTagName) {
                    push = false;
                }
            });
            if (push) {
                $scope.TagList.push(item);
                item.OrganizationFormID = $scope.OrganizationFormID;
                var jsonData = angular.toJson(item);
                AngularAjaxCall($http, HomeCareSiteUrl.AddOrgFormTagURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        ShowMessages(response);
                    }
                });
            }
        }
        $scope.TagTokenObj.clear();
    };


    $scope.DeleteTag = function (item, index) {
        if (item.OrganizationFormTagID == 0) {
            $scope.TagList.splice(index, 1);
        } else {
            var jsonData = angular.toJson({ id: item.OrganizationFormTagID });
            AngularAjaxCall($http, HomeCareSiteUrl.DeleteFormTagURL, jsonData, "Post", "json", "application/json", false).
                success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.TagList.splice(index, 1);
                    }
                });
        }
    };
    //#endregion
};
controllers.OrganizationFormsController.$inject = ['$scope', '$http', '$window', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowOrgFormSavedDetails");
});



app.filter('filterForms', function () {
    return function (formList, formFilter, orgforms) {
        var results = [];
        var formVersionMap = {};
        $.each(formList, function (index, form) {
            var existingVersionForm = formVersionMap[form.Name];
            var isCorrectVersion = !existingVersionForm || form.Version > existingVersionForm.Version;
            if (isCorrectVersion && existingVersionForm) {
                var existingIndex = results.indexOf(existingVersionForm);
                if (existingIndex >= 0) {
                    results.splice(existingIndex, 1);
                }
            }

            if (isCorrectVersion) {
                formVersionMap[form.Name] = form;
            }

            //var isCorrectVersion = true;
            if (form.IsActive && isCorrectVersion) {
                var hasShortName = ValideElement(formFilter.FormNumber) === false || form.Name.toUpperCase().indexOf(formFilter.FormNumber.toUpperCase()) > -1;
                var hasLongName = ValideElement(formFilter.FormName) === false || form.FormLongName.toUpperCase().indexOf(formFilter.FormName.toUpperCase()) > -1;
                //var hasLongName = (formFilter.FormName === '' || formFilter.FormName === null) || form.FormLongName.toUpperCase().indexOf(formFilter.FormName.toUpperCase()) > -1;
                var hasCategory = ValideElement(formFilter.FormCategoryID) === false || formFilter.FormCategoryID == 0 || form.EBCategoryID == formFilter.FormCategoryID;

                var hasPkg = false;

                if (hasShortName && hasLongName && hasCategory) {
                    $.each(form.EbMarketIDList, function (index, item) {

                        if (ValideElement(item) === false || item == '' || ValideElement(formFilter.MarketID) === false) {
                            hasPkg = true;
                            return false;
                        }

                        if (item == formFilter.MarketID) {
                            hasPkg = true;
                            return false;
                        }
                    });
                }

                if (hasShortName && hasLongName && hasCategory && hasPkg) {
                    results.push(form);
                }
            }
        });

        if (orgforms === "orgforms")
            vm.ResultOrgForms = results;
        else
            vm.ResultForms = results;
        return results;
    }
});