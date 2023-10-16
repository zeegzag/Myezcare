var vm;
controllers.CustomerEsignController = function ($scope, $http, $window) {
    vm = $scope;

    $scope.SetCustomerEsignModel = $.parseJSON($("#hdnCustomerEsignModel").val());
    //$scope.newInstance = function () {
    //    return $.parseJSON($("#hdnCustomerEsignModel").val());
    //};
    $scope.IsFinalPage = false;

    //#region Wizard
    $scope.currentStep = 1;
    $scope.steps = [
        {
            step: 1,
            name: window.FirstStep,
            desc: window.BasicInformation,
            isDone: false
        },
        {
            step: 2,
            name: window.SecondStep,
            desc: window.Esign,
            isDone: false
        }
    ];

    $scope.$watch('currentStep', function (newValue) {
        if (parseInt(newValue) > 0) {
            if (newValue == 1) {
                $scope.steps[0].isDone = true;
            }
        }
    });

    //$scope.$watch(function () {
    //    return $scope.SelectedClients.length;
    //}, function () {
    //    if ($scope.SelectedClients.length == 0) {
    //        $scope.steps[1].isDone = false;
    //        $scope.steps[2].isDone = false;
    //    }
    //});

    //Functions
    //$scope.GroupNoteModel.GN_ServiceCodeTokenObj = {};
    $scope.gotoStep = function (newStep) {
        //var isValid = true;
        if (newStep == 1)
            $scope.currentStep = newStep;

        //if (newStep == 2) {
        //    $timeout(function () {
        //        if ($scope.GroupNoteModel.Note.ServiceCodeID)
        //            $scope.GroupNoteModel.GN_ServiceCodeTokenObj.add({
        //                ServiceCodeID: $scope.GroupNoteModel.Note.ServiceCodeID,
        //                ServiceCode: $scope.GroupNoteModel.Note.ServiceCode,
        //                UnitType: $scope.GroupNoteModel.Note.UnitType
        //            });
        //    });
        //    $scope.currentStep = newStep;
        //}

        if (newStep == 2) {

            $scope.steps[1].isDone = true;
            $scope.steps[0].isDone = true;

            if ($scope.currentStep == 1 || $scope.currentStep == 2) {
                $scope.currentStep = newStep;
            }
        }
    };

    //#endregion




    /* Organization Forms */

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
        $scope.FormListPager.sortIndex = newPredicate != undefined ? newPredicate : sortIndex;
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


        //var jsonData = angular.toJson($scope.orgFormList);
        //AngularAjaxCall($http, HomeCareSiteUrl.SaveOrganizationFormDetailsURL, jsonData, "Post", "json", "application/json").success(function (response) {
        //    if (response.IsSuccess) {
        //        SetMessageForPageLoad(response.Message, "ShowOrgFormSavedDetails");
        //        window.location = HomeCareSiteUrl.OrganizationFormsURL;
        //    }
        //    else
        //        ShowMessages(response);

        //});


    };

    $scope.OpenNewHtmlForm = function (item) {
        var data = $scope.newInstance().ConfigEBFormModel;
        var jsonData = angular.toJson({ "username": data.ResuName, "password": data.ResuKey });
        var loginUrl = data.EBBaseSiteUrl + "/auth/login";
        CrossDomainAngularAjaxCall(loginUrl, jsonData, "POST", "json", "application/json").success(function (response, result) {
            if (result === "success") {
                var newFormUrl = data.EBBaseSiteUrl + "/form/" + item.NameForUrl + "?version=" + item.Version + "&tenantGuid=" + response.tenantGuid;
                $scope.ChildWindow = window.open(newFormUrl, "_blank", "width=" + screen.availWidth + ",height=" + screen.availHeight);
            }
        });
    }

    $scope.OpenNewPDFForm = function (item) {
        var data = $scope.newInstance().ConfigEBFormModel;
        var jsonData = angular.toJson({ "username": data.ResuName, "password": data.ResuKey });
        var loginUrl = data.EBBaseSiteUrl + "/auth/login";
        CrossDomainAngularAjaxCall(loginUrl, jsonData, "POST", "json", "application/json").success(function (response, result) {
            if (result === "success") {
                var newFormUrl = data.EBBaseSiteUrl + "/pdf/" + item.NameForUrl + "?version=" + item.Version + "&tenantGuid=" + response.tenantGuid;
                window.location = newFormUrl;
            }
        });
    }

    /* Organization Forms */




    $scope.CustomerEsignDetails = $scope.SetCustomerEsignModel.CustomerEsignDetails;
    $scope.ServicePlanComponents = $scope.SetCustomerEsignModel.ServicePlanComponents;
    $scope.OrganizationSettingDetails = $scope.SetCustomerEsignModel.OrganizationSettingDetails;
    $scope.TransactionResult = $scope.SetCustomerEsignModel.TransactionResult;

    $scope.ServicePlans = $scope.SetCustomerEsignModel.ServicePlans;
    $scope.ServicePlans = $scope.ServicePlans.map(function (plan) {
        plan.PlanClass = "pricing-table";
        return plan;
    });

    $scope.SaveCustomerEsign = function () {
        if (CheckErrors("#frmCustomerEsign")) {
            $scope.Temp = {};// angular.copy($scope.CustomerEsignDetails);
            $scope.Temp.CustomerEsignDetails = $scope.CustomerEsignDetails;
            $scope.Temp.OrganizationSettingDetails = $scope.OrganizationSettingDetails;
            $scope.Temp.ServicePlans = $.grep($scope.ServicePlans, function (plan) {
                return plan.IsSelected == true;
            });
            if ($scope.Temp.ServicePlans.length == 0) {
                ShowMessage(window.pleaseSelectPlan, "error");
                return;
            } else {
                $scope.SaveEsign();
            }            
        }
    };

    $scope.SaveEsign = function () {
        var jsonData = angular.toJson($scope.Temp);
        AngularAjaxCall($http, SiteUrl.SaveCustomerEsignURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                SetMessageForPageLoad(response.Message, "OrganizationUpdateSuccessMessage");
                $scope.IsFinalPage = true;
            }
            else {
                ShowMessages(response);
            }
        });
    }

    $scope.Cancel = function () {
        $window.location = SiteUrl.OrganizationListURL;
    };

    $scope.SelectPlan = function (index) {
        if (!$scope.ServicePlans[index].IsSelected) {
            $scope.ServicePlans[index].IsSelected = true;
        } else {
            $scope.ServicePlans[index].IsSelected = false;
        }
    }

    $scope.SelectPlanDiv = function (index) {
        if (!$scope.ServicePlans[index].IsSelected) {
            $.each($scope.ServicePlans, function (index, value) {
                value.IsSelected = false;
                value.PlanClass = "pricing-table";
            });
            $scope.ServicePlans[index].IsSelected = true;
            $scope.ServicePlans[index].PlanClass = "pricing-table selected-plan";
        } else {
            $scope.ServicePlans[index].IsSelected = false;
            $scope.ServicePlans[index].PlanClass = "pricing-table";
        }
    }
};
controllers.CustomerEsignController.$inject = ['$scope', '$http', '$window'];

app.filter('filterForms', function () {
    return function (formList, formFilter, orgforms) {
        debugger;
        var results = [];
        var formVersionMap = {};
        $.each(formList, function (index, form) {
            //debugger;
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