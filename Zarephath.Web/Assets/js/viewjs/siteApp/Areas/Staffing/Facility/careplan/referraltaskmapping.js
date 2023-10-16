var rimModel;

controllers.ReferralTaskMappingController = function ($scope, $http, $window, $timeout) {
    rimModel = $scope;
    $scope.SelectedTaskMappingIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.TaskMappingModel = $.parseJSON($("#hdnSetReferralTaskMappingModel").val());
    $scope.SearchTaskMappingListPage = $scope.TaskMappingModel.SearchTaskMappingListPage;
    $scope.TempSearchTaskMappingListPage = $scope.TaskMappingModel.SearchTaskMappingListPage;

    $scope.EncryptedReferralID = window.EncryptedReferralID;

    $scope.SearchVisitTask = {};

    $scope.VisitTaskListPager = new PagerModule("VisitTaskID", "", "DESC");

    $scope.VisitTaskList = [];
    $scope.VisitTaskList1 = [];
    $scope.GetVisitTaskList = function (isSearchFilter) {
        $scope.VisitTaskListPager.currentPage = isSearchFilter ? 1 : $scope.VisitTaskListPager.currentPage;

        $scope.SearchVisitTask.IgnoreIds = "";
        $scope.SearchVisitTask.EncryptedReferralID = $scope.EncryptedReferralID;
        $scope.SearchVisitTask.CareTypeID = $scope.CareTypeID;
        var pagermodel = {
            SearchVisitTaskListPage: $scope.SearchVisitTask,  
            pageSize: $scope.VisitTaskListPager.pageSize,
            pageIndex: $scope.VisitTaskListPager.currentPage,
            sortIndex: $scope.VisitTaskListPager.sortIndex,
            sortDirection: $scope.VisitTaskListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);

        AngularAjaxCall($http, HomeCareSiteUrl.GetVisitTaskListForRef, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $timeout(function () {

                    if (response.Data.CurrentPage == 1)
                        $scope.VisitTaskList = [];

                    if (response.Data.CurrentPage == 1 || $scope.VisitTaskListPager.lastPage < response.Data.CurrentPage)
                        Array.prototype.push.apply($scope.VisitTaskList, response.Data.Items);


                    $scope.VisitTaskListPager.lastPage = response.Data.CurrentPage;
                    $scope.VisitTaskListPager.currentPageSize = response.Data.Items.length;
                    $scope.VisitTaskListPager.totalRecords = response.Data.TotalItems;
                    $scope.VisitTaskList1 = response.Data.Items;

                    if (!$scope.$root.$$phase) {
                        $scope.$apply();
                    }
                });
            }
            //ShowMessages(response);
        });

    };

    $scope.VisitTaskListPager.getDataCallback = $scope.GetVisitTaskList;
    $scope.SearchVisitTasks = function () {
        $scope.VisitTaskListPager.getDataCallback();
        $scope.GetCaretype();
        $scope.GetVisitTaskCategory();
        $scope.GetVisitTaskSubCategory();
        $scope.GetPatientTaskMappings();
        $scope.SelectAll();
        $scope.SelectAllConclusion();
    }


    $scope.AddReferralTaskMapping = function (item) {
        var postData = {
            VisitTaskID: item.VisitTaskID,
            IsRequired: item.IsRequired,
            EncryptedReferralID: $scope.EncryptedReferralID
        };
        var jsonData = angular.toJson(postData);
        AngularAjaxCall($http, HomeCareSiteUrl.SaveReferralTaskMappingURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.GetVisitTaskList();
                    $scope.GetPatientTaskMappings();
                }
            });

    };

    $scope.TaskModel={}
    $scope.OpenTaskCommentModal = function (item) {
        $scope.TaskModel.ReferralTaskMappingID = item.ReferralTaskMappingID;
        $scope.TaskModel.Frequency = item.Frequency;
        $scope.TaskModel.Comment = item.Comment;
        $('#TaskCommentModal').modal('show');
    }

    $scope.SaveTaskDetail = function () {
            var jsonData = angular.toJson($scope.TaskModel);
            AngularAjaxCall($http, HomeCareSiteUrl.SaveTaskDetailURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.TaskModel = {};
                    $scope.GetPatientTaskMappings();
                    $('#TaskCommentModal').modal('hide');
                }
            });
        
    }

    $('#TaskCommentModal').on('hidden.bs.modal', function () {
        $scope.TaskModel = {};
        HideErrors($("#addTaskCommentFrm"));
    });

    $scope.PatientTaskList = [];
    $scope.PatientConclusionList = [];
    $scope.PatientTaskList1 = [];

    $scope.GetPatientTaskMappings = function () {
        var postData = {
            EncryptedReferralID: $scope.EncryptedReferralID,
            CareTypeID: $scope.CareTypeID,
        };
        var jsonData = angular.toJson(postData);
        AngularAjaxCall($http, HomeCareSiteUrl.GetPatientTaskMappingsURL, jsonData, "Post", "json", "application/json",false).success(function (response) {
            if (response.IsSuccess) {
                $scope.PatientTaskList = [];
                $scope.PatientConclusionList = [];
                $scope.PatientTaskList = response.Data.PatientTaskList;
                $scope.PatientConclusionList = response.Data.PatientConclusionList;
                $scope.TaskFrequencyCodeList = response.Data.TaskFrequencyCodeList;
                $scope.PatientTaskList1 = response.Data.Items;
            }
        });
    };
    
    $scope.DeleteRefTaskMapping = function (item, type) {
        var message = window.DeleteConclusionConfirmationMessage;
        if (type === 'task') 
            message = window.DeleteTaskConfirmationMessage;

        bootboxDialog(function (result) {
            if (result) {
                var postData = {
                    EncryptedReferralID: $scope.EncryptedReferralID,
                    ReferralTaskMappingID: item.ReferralTaskMappingID
                };
                var jsonData = angular.toJson(postData);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteRefTaskMappingURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    if (response.IsSuccess) {

                        $scope.PatientTaskList = [];
                        $scope.PatientConclusionList = [];
                        $scope.PatientTaskList = response.Data.PatientTaskList;
                        $scope.PatientConclusionList = response.Data.PatientConclusionList;

                        $scope.GetVisitTaskList();
                    }
                });
            }
        },
            bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, message, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);

    };

    $scope.DeleteRefTaskMappingBulk = function (item, type) {

        var message = window.DeleteConclusionConfirmationMessage;
        if (type === 'task')
            message = window.DeleteTaskConfirmationMessage;
                var postData = {
                    EncryptedReferralID: $scope.EncryptedReferralID,
                    ReferralTaskMappingID: item.ReferralTaskMappingID
                };
                var jsonData = angular.toJson(postData);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteRefTaskMappingURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    if (response.IsSuccess) {

                        $scope.PatientTaskList = [];
                        $scope.PatientConclusionList = [];
                        $scope.PatientTaskList = response.Data.PatientTaskList;
                        $scope.PatientConclusionList = response.Data.PatientConclusionList;

                        $scope.GetVisitTaskList();
                    }
                });
           
    };

    $scope.OnTaskChecked = function (item) {
        var postData = {
            EncryptedReferralID: $scope.EncryptedReferralID,
            ReferralTaskMappingID: item.ReferralTaskMappingID,
            IsRequired: item.IsRequired
        };
        
        var jsonData = angular.toJson(postData);
        AngularAjaxCall($http, HomeCareSiteUrl.OnTaskCheckedURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {


                $scope.PatientTaskList = [];
                $scope.PatientConclusionList = [];
                $scope.PatientTaskList = response.Data.PatientTaskList;
                $scope.PatientConclusionList = response.Data.PatientConclusionList;

                $scope.GetVisitTaskList();
            }
        });
    };

    $scope.SavePopupDataAndAddMapping = function () {
        if ($scope.SelectedCareType && $scope.SelectedActivity && $scope.SelectedVisitTask.length > 0) {
            $.each($scope.SelectedVisitTask, function (index, visitTaskId) {
                var postData = {
                    VisitTaskID: visitTaskId,
                    IsRequired: true,
                    EncryptedReferralID: $scope.EncryptedReferralID
                };
                var jsonData = angular.toJson(postData);
                AngularAjaxCall($http, HomeCareSiteUrl.SaveReferralTaskMappingURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.GetPatientTaskMappings();
                        $('#model_AddCarePlan').modal('hide');
                    }
                });
            });
            $scope.SelectedVisitTask = [];
            $scope.SelectedActivity = "";
            $scope.SelectedCareType = "";
        }
        else {
            bootboxDialog(null, bootboxDialogType.Alert, bootboxDialogTitle.Alert,
                "Please select all the fields", bootboxDialogButtonText.Ok);
        }
    };

    $scope.AddCarePlanModal = function () {
        $scope.SelectedVisitTask = [];
        $scope.SelectedActivity = "";
        $scope.SelectedCareType = "";
        //$scope.VisitTaskList = [];
        $scope.SearchVisitTasks();
        $('#model_AddCarePlan').modal({
            backdrop: 'static',
            keyboard: false
        });
    }

    $scope.ResetAddCarePlanModal = function () {
        //$scope.SelectedVisitTask = [];
        $scope.SelectedActivity = "";
        $scope.SelectedCareType = "";
        $scope.VisitTaskList = [];
        //$scope.VisitTaskCategories = [];
        //$scope.VisitTaskSubCategories = [];
        $('#model_AddCarePlan').modal('hide');

    }

    $scope.Refresh = function () {
        $scope.GetPatientTaskMappings();
    };

    $scope.ResetSearchFilter = function (Search) {
        Search.VisitTaskDetail = "";
        Search.CareTypeID = "";
        $scope.GetPatientTaskMappings();
    };

    $scope.SelectTaskMapping = function (item) {
        if (item.IsChecked)
            $scope.SelectedTaskMappingIds.push(item.ReferralTaskMappingID);
        else
            $scope.SelectedTaskMappingIds.remove(item.ReferralTaskMappingID);

        if ($scope.SelectedTaskMappingIds.length == $scope.TaskMappingListPager.currentPageSize) {
            $scope.SelectAllCheckbox = true;
        }
        else {
            $scope.SelectAllCheckbox = false;
        }
    };

    $scope.GetVisitTaskCategory = function () {
        var VisitTaskType = $scope.SearchVisitTask.VisitTaskType;
        var CareTypeID = $scope.CareType;
        var jsonData = angular.toJson({ VisitTaskType, CareTypeID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetVisitTaskCategoryURL1, jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.VisitTaskCategories = response.Data;
                    $scope.VisitTaskSubCategories = [];
                }
                else {
                    ShowMessages(response);
                }
            });
    }

    $scope.GetVisitTaskSubCategory = function () {
        var VisitTaskType = $scope.SearchVisitTask.VisitTaskType;
        var CareTypeID = $scope.CareType;
        var VisitTaskCategoryID = $scope.VisitTaskCategoryID;
        var jsonData = angular.toJson({ VisitTaskType, CareTypeID, VisitTaskCategoryID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetVisitTaskSubCategoryURL1, jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.VisitTaskSubCategories = response.Data;
                }
                else {
                    ShowMessages(response);
                }
            });
    }

    $scope.SelectAll = function (SelectAllCheckbox) {
        $scope.SelectedTaskMappingIds = [];

        angular.forEach($scope.PatientTaskList, function (item, key) {
            item.IsChecked = SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedTaskMappingIds.push(item.ReferralTaskMappingID);
        });

        return true;
    };

    $scope.SelectAllConclusion = function (SelectAllCheckbox) {
        $scope.SelectedTaskMappingIds = [];

        angular.forEach($scope.PatientConclusionList, function (item, key) {
            item.IsChecked = SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedTaskMappingIds.push(item.ReferralTaskMappingID);
        });

        return true;
    };

    $scope.SearchVisitTasksMapping = function (Search) {
        var postData = {
            VisitTaskDetail: Search.VisitTaskDetail,
            CareTypeID: Search.CareTypeID,
            EncryptedReferralID: $scope.EncryptedReferralID,
        };
        var jsonData = angular.toJson(postData);
        AngularAjaxCall($http, HomeCareSiteUrl.GetPatientTaskMappingsURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.PatientTaskList = response.Data.PatientTaskList;
                $scope.PatientConclusionList = response.Data.PatientConclusionList;
                $scope.TaskFrequencyCodeList = response.Data.TaskFrequencyCodeList;
            }
        });
    };

    $scope.DeleteSelectTaskMapping = function (item, type) {
        debugger
        if (item == undefined) {
            $scope.ListOfIdsInCsv = $scope.SelectedTaskMappingIds.toString();
            var message = window.DeleteMultipleConfirmationMessage;
        }
           
        bootboxDialog(function (result) {
            if (result) {
                var postData = {
                    EncryptedReferralID: $scope.EncryptedReferralID,
                    ListOfIdsInCsv: $scope.ListOfIdsInCsv
                };
                var jsonData = angular.toJson(postData);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteRefTaskMappingURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    if (response.IsSuccess) {

                        $scope.PatientTaskList = [];
                        $scope.PatientConclusionList = [];
                        $scope.PatientTaskList = response.Data.PatientTaskList;
                        $scope.PatientConclusionList = response.Data.PatientConclusionList;

                        $scope.GetVisitTaskList();
                        $scope.SelectAllCheckbox = false;
                        $(".checked").removeClass("checked");
                    }
                });
            }
        },
            bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, message, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);

    };

    $("#AddReferralTaskMappingModal").on('hidden.bs.modal', function () {
        $scope.ResetReferralInternamMessage();
    });

    $("a#CarePlan_ReferralTaskMapping").on('shown.bs.tab', function (e) {
        $scope.GetVisitTaskList();
        $scope.GetPatientTaskMappings();
    });

  
    $scope.ResultForms = [];
    $scope.SelectedForms = [];
    $scope.SelectMappedForms = [];
    $scope.SelectConclusionMappedForms = [];
    $scope.SelectAllFormCheckbox = false;
    $scope.SelectForm = function (item) {
        if (item.IsChecked)
            $scope.SelectedForms.push(item);
        else
            $scope.SelectedForms.remove(item);

        //if ($scope.SelectedForms.length == $scope.FormList.length)
        //    $scope.SelectAllFormCheckbox = true;
        //else
        //    $scope.SelectAllFormCheckbox = false;

    };
    $scope.SelectAllForms = function (val) {
        //$scope.SelectedForms = [];
        angular.forEach($scope.VisitTaskList, function (item, key) {
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
    $scope.SelectAllMappedForms = function (val) {
        //$scope.SelectedForms = [];
        angular.forEach($scope.PatientTaskList, function (item, key) {
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
    $scope.SelectAllConclusionMappedForms = function (val) {
        //$scope.SelectedForms = [];
        angular.forEach($scope.PatientConclusionList, function (item, key) {
            item.IsChecked = val;//event.target.checked; //$scope.SelectAllFormCheckbox;//

            if (item.IsChecked) {
                if ($scope.SelectConclusionMappedForms.indexOf(item) == -1)
                    $scope.SelectConclusionMappedForms.push(item);
            } else {
                if ($scope.SelectConclusionMappedForms.indexOf(item) !== -1)
                    $scope.SelectConclusionMappedForms.remove(item);
            }
        });

        return true;
    };
    $scope.MoveFromTaskFormToMappedForm = function () {
        var selectedIds = [];
        var list = $scope.SearchVisitTask.VisitTaskType == Resource_Conclusion ? $scope.PatientConclusionList : $scope.PatientTaskList;
        $.each($scope.SelectedForms, function (index, item) {
            if (list.indexOf(item) === -1) {
                item.IsChecked = false;
                list.push(item);
                $scope.VisitTaskList.remove(item);
                selectedIds.push(item.VisitTaskID);
            }
        });
        if (selectedIds.length > 0) {
            var postData = {
                ListOfIdsInCsv: selectedIds.join(','),
                EncryptedReferralID: $scope.EncryptedReferralID
            };
            var jsonData = angular.toJson(postData);
            AngularAjaxCall($http, HomeCareSiteUrl.SaveReferralTaskMappingURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.GetVisitTaskList();
                    $scope.GetPatientTaskMappings();
                    $scope.SelectedForms = [];
                    $scope.SelectAllFormCheckbox = false;
                    $scope.SelectAllForms();
                }
            });
        }
    };
    $scope.MoveFromMappedFormToTaskForm = function () {
        var selectedIds = [];
        $.each($scope.SelectedForms, function (index, item) {
            if ($scope.VisitTaskList.indexOf(item) === -1) {
                item.IsChecked = false;
                $scope.VisitTaskList.push(item);
                $scope.PatientTaskList.remove(item);
                selectedIds.push(item.ReferralTaskMappingID);
            } 
        });
        $.each($scope.SelectConclusionMappedForms, function (index, item) {
            if ($scope.VisitTaskList.indexOf(item) === -1) {
                item.IsChecked = false;
                $scope.VisitTaskList.push(item);
                $scope.PatientConclusionList.remove(item);
                selectedIds.push(item.ReferralTaskMappingID);
            }
        });
        if (selectedIds.length > 0) {
            var postData = {
                EncryptedReferralID: $scope.EncryptedReferralID,
                ListOfIdsInCsv: selectedIds.join(',')
            };
            var jsonData = angular.toJson(postData);
            AngularAjaxCall($http, HomeCareSiteUrl.DeleteRefTaskMappingURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                if (response.IsSuccess) {

                    $scope.PatientTaskList = [];
                    $scope.PatientConclusionList = [];
                    $scope.PatientTaskList = response.Data.PatientTaskList;
                    $scope.PatientConclusionList = response.Data.PatientConclusionList;

                    $scope.GetVisitTaskList();
                    $scope.SelectedForms = [];
                    $scope.SelectAllFormCheckbox = false;
                    $scope.SelectAllForms();
                }
            });
        }
    };
    $scope.SaveTaskFormDetails = function () {
        window.location.reload();
    }

    $scope.CaretypeList = [];
    $scope.GetCaretype = function () {
        var postData = {
            EncryptedReferralID: $scope.EncryptedReferralID,
        };
        var jsonData = angular.toJson(postData);
        AngularAjaxCall($http, HomeCareSiteUrl.GetCarePlanCaretypes, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.CaretypeList = response.Data;
            }
        });
    };

    $("a#CarePlan, a#CarePlan_ReferralTaskMapping").on('shown.bs.tab', function (e) {
        $scope.GetCaretype();
        $(".tab-pane a[href='#tab_ReferralTaskMapping']").tab('show');
    });
      
};
controllers.ReferralTaskMappingController.$inject = ['$scope', '$http', '$window', '$timeout'];


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
