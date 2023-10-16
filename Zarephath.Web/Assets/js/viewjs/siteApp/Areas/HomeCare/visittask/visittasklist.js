var vm;


controllers.VisitTaskListController = function ($scope, $http, $timeout) {
    vm = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetVisitTaskListPage").val());
    };

    $scope.AddVisitTaskURL = HomeCareSiteUrl.AddVisitTaskURL;
    $scope.VisitTaskList = [];
    $scope.SelectedVisitTaskIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.VisitTaskModel = $.parseJSON($("#hdnSetVisitTaskListPage").val());
    $scope.SearchVisitTaskListPage = $scope.VisitTaskModel.SearchVisitTaskListPage;
    $scope.TempSearchVisitTaskListPage = $scope.VisitTaskModel.SearchVisitTaskListPage;

    $scope.VisitTaskListPager = new PagerModule("VisitTaskID", "", "DESC");

    $scope.BulkType = [
        { value: "Category", name: "Category" },
        { value: "VisitType", name: "Visit Type" },
        { value: "VisitTaskType", name: "Task Type" },
        { value: "CareType", name: "Care Type" },
        { value: "ServiceCode", name: "Service Code" },
        { value: "IsDefault", name: "IsDefault" },
        { value: "IsRequired", name: "IsRequired" }
    ];

    $scope.selectedBulkType = {};

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            SearchVisitTaskListPage: $scope.SearchVisitTaskListPage,
            pageSize: $scope.VisitTaskListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.VisitTaskListPager.sortIndex,
            sortDirection: $scope.VisitTaskListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchVisitTaskListPage = $.parseJSON(angular.toJson($scope.TempSearchVisitTaskListPage));

    };

    $scope.GetVisitTaskList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedVisitTaskIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchVisitTaskListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.VisitTaskListPager.currentPage);

        AngularAjaxCall($http, HomeCareSiteUrl.GetVisitTaskList, jsonData, "Post", "json", "application/json").success(function (response) {

            if (response.IsSuccess) {
                $scope.VisitTaskList = response.Data.Items;
                $scope.VisitTaskListPager.currentPageSize = response.Data.Items.length;
                $scope.VisitTaskListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.Refresh = function () {
        //$scope.ResetSearchFilter();
        //$scope.CaseManagerListPager.currentPage = $scope.CaseManagerListPager.currentPage;
        $scope.VisitTaskListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');
            $scope.SearchVisitTaskListPage = $scope.newInstance().SearchVisitTaskListPage;
            $scope.TempSearchVisitTaskListPage = $scope.newInstance().SearchVisitTaskListPage;
            $scope.TempSearchVisitTaskListPage.IsDeleted = "0";
            $scope.TempSearchVisitTaskListPage.ServiceCodeID = "";
            $scope.VisitTaskListPager.currentPage = 1;
            $scope.VisitTaskListPager.getDataCallback();
        });
    };
    $scope.SearchVisitTask = function () {
        $scope.VisitTaskListPager.currentPage = 1;
        $scope.VisitTaskListPager.getDataCallback(true);
    };

    $scope.SearchVisitTaskChange = function (type) {


        $scope.SearchVisitTask()
    };

    if ($scope.SelectedVisitTaskIds.length == 0)
        $scope.CloneTaskBtnDisabled = true;

    // This executes when select single checkbox selected in table.
    $scope.SelectVisitTask = function (VisitTask) {
        if (VisitTask.IsChecked)
            $scope.SelectedVisitTaskIds.push(VisitTask.VisitTaskID);
        else
            $scope.SelectedVisitTaskIds.remove(VisitTask.VisitTaskID);

        if ($scope.SelectedVisitTaskIds.length == $scope.VisitTaskListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

        if ($scope.SelectedVisitTaskIds.length > 0)
            $scope.CloneTaskBtnDisabled = false;
        else
            $scope.CloneTaskBtnDisabled = true;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedVisitTaskIds = [];

        angular.forEach($scope.VisitTaskList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedVisitTaskIds.push(item.VisitTaskID);
        });

        if ($scope.SelectedVisitTaskIds.length > 0)
            $scope.CloneTaskBtnDisabled = false;
        else
            $scope.CloneTaskBtnDisabled = true;

        return true;
    };

    $scope.DeleteVisitTask = function (VisitTaskId, title) {

        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchVisitTaskListPage.ListOfIdsInCsv = VisitTaskId > 0 ? VisitTaskId.toString() : $scope.SelectedVisitTaskIds.toString();

                if (VisitTaskId > 0) {
                    if ($scope.VisitTaskListPager.currentPage != 1)
                        $scope.VisitTaskListPager.currentPage = $scope.VisitTaskList.length === 1 ? $scope.VisitTaskListPager.currentPage - 1 : $scope.VisitTaskListPager.currentPage;
                } else {

                    if ($scope.VisitTaskListPager.currentPage != 1 && $scope.SelectedVisitTaskIds.length == $scope.VisitTaskListPager.currentPageSize)
                        $scope.VisitTaskListPager.currentPage = $scope.VisitTaskListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedVisitTaskIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.VisitTaskListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteVisitTask, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.VisitTaskList = response.Data.Items;
                        $scope.VisitTaskListPager.currentPageSize = response.Data.Items.length;
                        $scope.VisitTaskListPager.totalRecords = response.Data.TotalItems;
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
    $scope.VisitTaskListPager.getDataCallback = $scope.GetVisitTaskList;
    $scope.VisitTaskListPager.getDataCallback();

    /*Start Bulk Update Detail*/
    $scope.BulkUpdateDetail = function () {
        debugger
        if ($scope.SelectedVisitTaskIds.length > 0 && $scope.selectedBulkType) {

            var jsonData = { BulkType: $scope.selectedBulkType.Name, VisitTaskIDList: $scope.SelectedVisitTaskIds.toString(), Catrgory: $scope.selectedBulkType.Category };
            AngularAjaxCall($http, HomeCareSiteUrl.BulkUpdateVisitTaskDetail, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.SelectedVisitTaskIds = [];
                    $scope.selectedBulkType = {};
                    $scope.SearchVisitTask();
                }
            });
        }

    };

    /*End Bulk Update Detail*/

    $scope.OpenAddVTModal = function (id) {
        if (id !== undefined) {
            AngularAjaxCall($http, HomeCareSiteUrl.GetAddVisitTaskURL + id, null, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.$broadcast('loadChildData', response.Data);
                    $('#addvisittaskmodel').modal({ backdrop: false, keyboard: false });
                    $("#addvisittaskmodel").modal('show');
                }
            });
        } else {
            $('#addvisittaskmodel').modal({ backdrop: false, keyboard: false });
            $("#addvisittaskmodel").modal('show');
        }
    };

    $scope.CloseAddVTModal = function () {
        $("#addvisittaskmodel").modal('hide');
        HideErrors("#frmVisitTask");
        location.reload();
    };
    $scope.CloseAddVTCModal = function () {
        $("#addCategoryModel").modal('hide');
        HideErrors("#frmVisitTaskCategory");
    };
    $scope.CloneTask = function () {
        $scope.SearchVisitTaskListPage.TargetCareType = "";
        $scope.SearchVisitTaskListPage.TargetServiceCode = "";

        if ($scope.SelectedVisitTaskIds.length > 0)
            $('#CloneTaskModal').modal('show');
    }
    $scope.VisitTaskIDs = [];
    $scope.SelectcloneVisitTask = function (item) {
        
        if (item.IsChecked)
            $scope.VisitTaskIDs.push(item.VisitTaskID);
        else
            $scope.VisitTaskIDs.remove(item.VisitTaskID);

        //if ($scope.SelectedForms.length == $scope.FormList.length)
        //    $scope.SelectAllFormCheckbox = true;
        //else
        //    $scope.SelectAllFormCheckbox = false;

    };
    $scope.SaveCloneTask = function (VisitTaskId, title) {
        
        if (title == undefined) {
            title = window.UpdateRecords;
        }

        $scope.VisitTaskIDs = $scope.SelectedVisitTaskIds.join([separator = ',']);
        $scope.SearchVisitTaskListPage.ListOfIdsInCsv = VisitTaskId > 0 ? VisitTaskId.toString() : $scope.VisitTaskIDs.toString();

        if (VisitTaskId > 0) {
            if ($scope.VisitTaskListPager.currentPage != 1)
                $scope.VisitTaskListPager.currentPage = $scope.VisitTaskList.length === 1 ? $scope.VisitTaskListPager.currentPage - 1 : $scope.VisitTaskListPager.currentPage;
        } else {

            if ($scope.VisitTaskListPager.currentPage != 1 && $scope.SelectedVisitTaskIds.length == $scope.VisitTaskListPager.currentPageSize)
                $scope.VisitTaskListPager.currentPage = $scope.VisitTaskListPager.currentPage - 1;
        }

        //Reset Selcted Checkbox items and Control
        $scope.VisitTaskIDs = [];
        $scope.SelectAllCheckbox = false;
        ////Reset Selcted Checkbox items and Control
        //var jsonData = angular.toJson({ ListOfIdsInCsv: $scope.SearchVisitTaskListPage.ListOfIdsInCsv });
        ////  var jsonData = $scope.SetPostData($scope.VisitTaskListPager.currentPage);

        var jsonData = angular.toJson({ ListOfIdsInCsv: $scope.SearchVisitTaskListPage.ListOfIdsInCsv, TargetCareType: $scope.SearchVisitTaskListPage.TargetCareType, TargetServiceCode: $scope.SearchVisitTaskListPage.TargetServiceCode });

        AngularAjaxCall($http, HomeCareSiteUrl.SaveCloneTaskURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.GetVisitTaskList();

                //$scope.VisitTaskList = response.Data.Items;
                //$scope.VisitTaskListPager.currentPageSize = response.Data.Items.length;
                //$scope.VisitTaskListPager.totalRecords = response.Data.TotalItems;

                $scope.CloneTaskBtnDisabled = true;
            }
        });
        $('#CloneTaskModal').modal('hide');
    }

    $scope.VisittaskEditModel = function (EncryptedVisitTaskID, title) {
        var EncryptedVisitTaskID = EncryptedVisitTaskID;
        $('#visittask_fixedAside').modal({ backdrop: 'static', keyboard: false });
        $('#visittask_fixedAsidelDDLBindIFrame').attr('src', HomeCareSiteUrl.PartialAddVisitTaskURL + EncryptedVisitTaskID);
    }
    $scope.VisittaskEditModelClosed = function () {
        $scope.Refresh();
        $('#visittask_fixedAside').modal('hide');
    }

};

controllers.VisitTaskListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowVisitTaskMessage");
    //$(".dateInputMask").inputmask("m/d/y", {
    //    placeholder: "mm/dd/yyyy"
    //});
    $(".dateInputMask").attr("placeholder", "mm/dd/yy");
});
