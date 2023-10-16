var custModel;

controllers.AddVisitTaskController = function ($scope, $http, $timeout, $window) {
    custModel = $scope;
    $scope.model = $.parseJSON($("#hdnVisitTaskModel").val());
    
    $scope.VisitTask = $scope.model.VisitTask;
    $scope.MappedFormList = $scope.model.MappedFormList;
    $scope.Category = $scope.model.Category;
    $scope.TaskFrequencyCodeList = $scope.model.TaskFrequencyCodeList;
    $scope.ComplianceList = $scope.model.ComplianceList;
    $scope.IsEditMode = $scope.VisitTask.VisitTaskID > 0;
    $scope.ConfigEBFormModel = $scope.model.ConfigEBFormModel;

    $scope.$on("loadChildData", function (event, args) {
        $scope.MappedFormList = args.MappedFormList;
        $scope.Category = args.Category;
        $scope.TaskFrequencyCodeList = args.TaskFrequencyCodeList;
        $scope.IsEditMode = args.VisitTask.VisitTaskID > 0;
        $scope.ConfigEBFormModel = args.ConfigEBFormModel;
        $scope.VisitTaskTypes = args.VisitTaskTypes;
        $scope.VisitTypeList = args.VisitTypeList;
        $scope.VisitTask = args.VisitTask;

        var jsonData = angular.toJson({
            VisitTypeID: $scope.VisitTask.VisitType
        });
        AngularAjaxCall($http, "/hc/visittask/getcaretypelistfromvisittype", jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.CareTypeList = response.Data;
                    $scope.VisitTask.CareType = Number($scope.VisitTask.CareType);
                }
                else {
                    ShowMessages(response);
                }
            });
    });
    //$scope.SelectTaskOption = [];
    $scope.TaskOption = function () {
        if ($scope.VisitTask.TaskOption != null) {
            $scope.VisitTask.TaskOption = $scope.VisitTask.TaskOption.split(",");
        }
    };
    $scope.TaskOption();

    $scope.OnVisitTypeChange = function () {
        var jsonData = angular.toJson({
            VisitTypeID: $scope.VisitTask.VisitType
        });
        AngularAjaxCall($http, "/hc/visittask/getcaretypelistfromvisittype", jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.CareTypeList = response.Data;
                }
                else {
                    ShowMessages(response);
                }
            });
    }

    $scope.isTaskType = function () {
        return $scope.VisitTask.VisitTaskType == window.TaskTypeTask;
    }

    $scope.GetVisitTaskCategory = function () {
        if (!$scope.isTaskType()) {
            $scope.VisitTask.VisitType = 0;
            $scope.OnVisitTypeChange();
        }
        if ($scope.VisitTask.VisitTaskType === 'Conclusion' || $scope.VisitTask.VisitTaskType === '')
        {   
            $timeout(function () {
                if ($scope.VisitTask.ServiceCodeID > 0) {
                    $scope.VisitTask.ServiceCodeID = null;
                    $("#SearchContactToken").tokenInput("clear");
                }
            }, 1000);
        }

        var VisitTaskType = $scope.VisitTask.VisitTaskType;// != null ? $scope.VisitTask.VisitTaskType : $scope.Category.VisitTaskType;
        var jsonData = angular.toJson({ VisitTaskType: VisitTaskType });
        AngularAjaxCall($http, HomeCareSiteUrl.GetVisitTaskCategoryURL, jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.VisitTaskCategories = response.Data;
                }
                else {
                    ShowMessages(response);
                }
            });
    }

    

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
                + "&FormName=" + item.Name

                + "&FormPath=" + item.NameForUrl;

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


    $scope.GetVisitTaskCategoryByModel = function () {
        var VisitTaskType = $scope.Category.VisitTaskType;// != null ? $scope.Category.VisitTaskType : $scope.VisitTask.VisitTaskType;
        var jsonData = angular.toJson({ VisitTaskType: VisitTaskType });
        AngularAjaxCall($http, HomeCareSiteUrl.GetVisitTaskCategoryURL, jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.PopupVisitTaskCategories = response.Data;
                }
                else {
                    ShowMessages(response);
                }
            });
    }

    $scope.GetVisitTaskSubCategory = function () {
        var jsonData = angular.toJson({ VisitTaskCategoryID: $scope.VisitTask.VisitTaskCategoryID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetVisitTaskSubCategoryURL, jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.VisitTaskSubCategories = response.Data;
                }
                else {
                    ShowMessages(response);
                }
            });
    }

    if ($scope.VisitTask.VisitTaskType != null) {
        $scope.GetVisitTaskCategory();
        $scope.GetVisitTaskSubCategory();
    }

    $scope.Save = function () {
        var isValid = CheckErrors($("#frmVisitTask"));
        if (isValid) {
            if ($scope.VisitTask.TaskOption!=null) {
                $scope.VisitTask.TaskOption = $scope.VisitTask.TaskOption.toString();
            }
            var jsonData = angular.toJson({ VisitTask: $scope.VisitTask });
            AngularAjaxCall($http, HomeCareSiteUrl.AddVisitTaskURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {

                    if (response.IsSuccess) {
                        if ($scope.VisitTask.VisitTaskID == 0 || $scope.VisitTask.VisitTaskID == undefined) {
                            toastr.success("VisitTask Save Successfully");
                            $scope.VisitTask = null;
                        }
                        else {
                            toastr.success("VisitTask Update Successfully");
                        }
                    }
                    else {
                        ShowMessages(response);
                    }
                });
        }
    };

    $scope.Cancel = function () {
        window.location.reload();
    };

    $scope.OpenCategoryModal = function ($event, item) {
        $event.stopPropagation();
        if (item == 1) {
            $scope.GetModelCategoryList(true);
            $('#addCategoryModel').modal('show');
        } else {
            $scope.GetModelCategoryList(false);
            $('#addSubCategoryModel').modal('show');
        }
    }

    $scope.Search = {
        SubCategotyName: null,
        CategoryName: null,
        Type: null
    }

    $scope.ResetCategory = function () {
        $scope.Category.VisitTaskCategoryID = null;
        $scope.Category.CategoryName = null;
        $scope.Category.SubCategoryName = null;
        $scope.Category.VisitTaskType = null;
        $scope.Category.ParentCategoryLevel = null;
    };

    $scope.GetModelCategoryList = function (isCategoryList) {
        var jsonData = angular.toJson({
            CategoryName: $scope.Search.CategoryName,
            SubCategoryName: $scope.Search.SubCategoryName,
            Type: $scope.Search.Type,
            IsCategoryList: isCategoryList
        });
        AngularAjaxCall($http, HomeCareSiteUrl.GetModelCategoryListURL, jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.Categories = response.Data;
                }
                else {
                    ShowMessages(response);
                }
            });
    }

    $scope.Reset = function (isCategoryList) {
        $scope.Search = {
            SubCategotyName: null,
            CategoryName: null,
            Type: null
        }
        $scope.GetModelCategoryList(isCategoryList);
    }

    $scope.EditCategory = function (item) {
        $scope.Category.VisitTaskCategoryID = item.VisitTaskCategoryID;
        $scope.Category.VisitTaskType = item.VisitTaskCategoryType;
        $scope.Category.CategoryName = item.VisitTaskCategoryName;
    }

    $scope.SaveCategory = function () {
        var isValid = CheckErrors($("#frmVisitTaskCategory"));
        if (isValid) {
            var jsonData = angular.toJson({ Category: $scope.Category });
            AngularAjaxCall($http, HomeCareSiteUrl.SaveCategoryURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        $scope.GetModelCategoryList(true);
                        $scope.Category.VisitTaskCategoryID = null;
                        $scope.Category.VisitTaskType = null;
                        $scope.Category.CategoryName = null;
                    }
                        ShowMessages(response);
                });
        }
    }

    $scope.EditSubCategory = function (item) {
        $scope.Category.VisitTaskCategoryID = item.VisitTaskCategoryID;
        $scope.Category.VisitTaskType = item.ParentTaskType;
        $scope.Category.SubCategoryName = item.VisitTaskCategoryName;
        $scope.Category.ParentCategoryLevel = item.ParentCategoryLevel;
        $scope.GetVisitTaskCategoryByModel();
    }

    $scope.CustomValidate = function () {

    }

    $scope.SaveSubCategory = function () {
        if ($scope.Category.ParentCategoryLevel == null) {
            $('#parentCategoryLevel').addClass('tooltip-danger');
        }
        var isValid = CheckErrors($("#frmVisitTaskSubCategory"));
        if (isValid) {
            var jsonData = angular.toJson({ Category: $scope.Category });
            AngularAjaxCall($http, HomeCareSiteUrl.SaveSubCategoryURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        $scope.GetModelCategoryList(false);
                        $scope.Category.VisitTaskCategoryID = null;
                        $scope.Category.SubCategoryName = null;
                        $scope.Category.VisitTaskType = null;
                        $scope.Category.ParentCategoryLevel = null;
                    }
                    ShowMessages(response);
                });
        }
    }

    $('#addCategoryModel').on('hidden.bs.modal', function () {
        $scope.Category.VisitTaskCategoryID = null;
        $scope.Category.VisitTaskType = null;
        $scope.Category.CategoryName = null;
        $scope.Search.CategoryName = null;
        $scope.Search.Type = null;
        $scope.GetVisitTaskCategory();
    });

    $('#addSubCategoryModel').on('hidden.bs.modal', function () {
        $scope.Category.VisitTaskCategoryID = null;
        $scope.Category.SubCategoryName = null;
        $scope.Category.VisitTaskType = null;
        $scope.Category.ParentCategoryLevel = null;
        $scope.Search.CategoryName = null;
        $scope.Search.SubCategoryName = null;
        $scope.GetVisitTaskSubCategory();
    });

    //Service Code AutoCompleter
    //#region 

    $scope.GetServiceCodeListURL = "/hc/visittask/getservicecodelist"; //HomeCareSiteUrl.GetServiceCodeListURL;
    
    $timeout(function () {
        if ($scope.VisitTask.ServiceCodeID > 0) {
            $("#SearchContactToken").tokenInput("clear");
            $("#SearchContactToken").tokenInput("add", { ServiceCodeID: $scope.VisitTask.ServiceCodeID, ServiceCode: $scope.VisitTask.ServiceCode});
        }
    },1000);
    $scope.ServiceCodeResultsFormatter = function (item) {
        return "<li id='{0}'><b style='color:#ad0303;'>Code: </b>{0}<br/><b style='color:#ad0303;'>Name: </b>{1}<br/><b style='color:#ad0303;'>Billable: </b>{2}</li>"
            .format(
            item.ServiceCode,
            item.ServiceName,
            item.IsBillable ? window.Yes : window.No
            );
    };
    $scope.ServiceCodeTokenFormatter = function (item) {
        return "<li id='{0}'>{0}</li>"
            .format(
            item.ServiceCode
            );
    };
    //#endregion


    //VisitType AND CareType Drop Down
    //#region  
    $scope.CareTypeList = [];

    

    if ($scope.VisitTask.VisitTaskID > 0) {
        $scope.OnVisitTypeChange();
        $scope.VisitTask.CareType = Number($scope.VisitTask.CareType);
    }
    //#endregion

    //#region form mapping

    $scope.OpenAddFormModal = function (data) {
        if (data.VisitTaskID == 0) {
            bootboxDialog(function (result) {
                if (result) {
                    $scope.Save();
                }
            }, bootboxDialogType.Confirm, window.Title, window.SaveConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
        } else {
            AngularAjaxCall($http, HomeCareSiteUrl.GetOrgFormListURL, null, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.OrganizationFormList = response.Data;
                }
            });
            $('#mapFormModal').modal('show');
        }
    };

    $scope.SelectedForms = [];
    $scope.SelectForm = function (form) {
        if (form.IsChecked)
            $scope.SelectedForms.push(form.EBFormID);
        else
            $scope.SelectedForms.remove(form.EBFormID);
    };

    $scope.MapSelectedForms = function () {
        if ($scope.SelectedForms.length == 0) {
            ShowMessage("Please select the form.", "error");
        } else {
            var jsonData = angular.toJson({ VisitTaskID: $scope.VisitTask.VisitTaskID, EBFormIDs: $scope.SelectedForms.toString() });
            AngularAjaxCall($http, HomeCareSiteUrl.MapSelectedFormURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.GetTaskFormList();
                    $('#mapFormModal').modal('hide');
                }
                $scope.SelectedForms = [];
                ShowMessages(response);
            });
        }
    };

    $scope.GetTaskFormList = function () {
        var jsonData = angular.toJson({ id: $scope.VisitTask.VisitTaskID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetTaskFormListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.MappedFormList = response.Data;
            }
        });
    };

    $scope.VisitTaskFormEditCompliance = function (item) {
        var postData = {
            TaskFormMappingID: item.TaskFormMappingID,
            ComplianceID: item.ComplianceID
        };

        var jsonData = angular.toJson(postData);
        AngularAjaxCall($http, HomeCareSiteUrl.VisitTaskFormEditComplianceURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {

                $scope.GetTaskFormList();
            }
        });
    };

    $scope.OnFormChecked = function (item) {
        var postData = {
            TaskFormMappingID: item.TaskFormMappingID,
            IsRequired: item.IsRequired
        };

        var jsonData = angular.toJson(postData);
        AngularAjaxCall($http, HomeCareSiteUrl.OnFormCheckedURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {

                $scope.GetTaskFormList();
            }
        });
    };

    $scope.DeleteMappedForm = function (TaskFormMappingID,title) {
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = angular.toJson({ id: TaskFormMappingID });
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteMappedFormURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    if (response.IsSuccess) {
                        $scope.GetTaskFormList();
                        ShowMessages(response);
                    }
                });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };

    //#endregion
};

controllers.AddVisitTaskController.$inject = ['$scope', '$http', '$timeout', '$window'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowVisitTaskMessage");
    //$(".dateInputMask").inputmask("m/d/y", {
    //    placeholder: "mm/dd/yyyy"
    //});
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});