﻿var custModel;

controllers.AddPayorGroupController = function ($scope, $http, $timeout, $window, $filter) {
    custModel = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnPayorGroupModel").val());
    };

    $scope.DDMasterList = [];
    $scope.SelectedDDMasterIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.DDMasterModel = $.parseJSON($("#hdnPayorGroupModel").val());
    $scope.SearchDDMasterListPage = $scope.DDMasterModel.SearchDDMasterListPage;
    $scope.TempSearchDDMasterListPage = $scope.DDMasterModel.SearchDDMasterListPage;
    $scope.DDTypeList = $scope.DDMasterModel.TypeList;
    $scope.DDMasterModel.DDMaster.IsDisplayValue = false;
    $scope.MappingDDMaster = $scope.DDMasterModel.MappingDDMaster;

    $scope.TempSearchDDMasterListPage.ItemType = $scope.DDMasterModel.DDMaster.ItemType;



    $scope.SaveDDMaster = function () {
        var isValid = CheckErrors($("#frmDDMaster"));
        if (isValid) {

            $scope.DDMasterModel.DDMaster.IsMultiSelect = $scope.IsMultiSelect;

            if (ValideElement($scope.DDMasterModel.DDMaster.SelectedParentChildValueItem)) {
                $scope.DDMasterModel.DDMaster.SelectedParentChildValueItems.push($scope.DDMasterModel.DDMaster.SelectedParentChildValueItem);
            }

            var jsonData = angular.toJson({ DDMaster: $scope.DDMasterModel.DDMaster, childTaskIds: $scope.DDMasterModel.DDMaster.SelectedParentChildValueItems });
            AngularAjaxCall($http, HomeCareSiteUrl.SaveDDmasterURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    $scope.DDMasterModel.DDMaster = $scope.newInstance().DDMaster;
                    ShowMessages(response);
                    $scope.GetGeneralMasterList();
                });
        }
    };

    $scope.Cancel = function () {
        HideErrors($("#frmDDMaster"));
        $scope.DDMasterModel.DDMaster = $scope.newInstance().DDMaster;
    };

    $scope.WZCancel = function () {
        HideErrors($("#frmDDMaster"));
        $scope.DDMasterModel.DDMaster.DDMasterID = '';
        $scope.DDMasterModel.DDMaster.Title = '';
        $scope.DDMasterModel.DDMaster.Value = '';
    };

    $scope.EditDDMaster = function (data) {
        $scope.DDMasterModel.DDMaster.DDMasterID = data.DDMasterID;
        $scope.DDMasterModel.DDMaster.ItemType = data.DDMasterTypeID;
        $scope.DDMasterModel.DDMaster.Title = data.Title;
        $scope.DDMasterModel.DDMaster.Value = data.Value;

        $("html, body").animate({ scrollTop: 0 }, "slow");

        HideErrors($("#frmDDMaster"));
        $scope.ResetColorValue = true;
        $scope.CheckForParentChildMapping(data.DDMasterTypeID, data.DDMasterID);
    }


    $scope.DDMasterListPager = new PagerModule("ItemType", "", "ASC");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            SearchDDMasterListPage: $scope.SearchDDMasterListPage,
            pageSize: $scope.DDMasterListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.DDMasterListPager.sortIndex,
            sortDirection: $scope.DDMasterListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchDDMasterListPage = $.parseJSON(angular.toJson($scope.TempSearchDDMasterListPage));
    };

    $scope.GetGeneralMasterList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedDDMasterIds = [];
        $scope.SelectAllCheckbox = false;
        $scope.SearchDDMasterListPage.ListOfIdsInCSV = [];
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping

        var jsonData = $scope.SetPostData($scope.DDMasterListPager.currentPage);

        AngularAjaxCall($http, HomeCareSiteUrl.GetGeneralMasterList, jsonData, "Post", "json", "application/json").success(function (response) {

            if (response.IsSuccess) {
                $scope.DDMasterList = response.Data.Items;
                $scope.DDMasterListPager.currentPageSize = response.Data.Items.length;
                $scope.DDMasterListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            $scope.SearchDDMasterListPage = $scope.newInstance().SearchDDMasterListPage;
            $scope.TempSearchDDMasterListPage = $scope.newInstance().SearchDDMasterListPage;
            $scope.TempSearchDDMasterListPage.IsDeleted = "0";
            $scope.DDMasterListPager.currentPage = 1;
            $scope.DDMasterListPager.getDataCallback();
        });
    };
    $scope.SearchDDMaster = function () {
        $scope.DDMasterListPager.currentPage = 1;
        $scope.DDMasterListPager.getDataCallback(true);
    };

    $scope.SelectDDMaster = function (DDMaster) {
        if (DDMaster.IsChecked)
            $scope.SelectedDDMasterIds.push(DDMaster.DDMasterID);
        else
            $scope.SelectedDDMasterIds.remove(DDMaster.DDMasterID);

        if ($scope.SelectedDDMasterIds.length == $scope.DDMasterListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;
    };

    $scope.SelectAll = function () {
        $scope.SelectedDDMasterIds = [];

        angular.forEach($scope.DDMasterList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedDDMasterIds.push(item.DDMasterID);
        });
        return true;
    };

    $scope.DeleteDDMaster = function (DDMasterId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchDDMasterListPage.ListOfIdsInCsv = DDMasterId > 0 ? DDMasterId.toString() : $scope.SelectedDDMasterIds.toString();

                if (DDMasterId > 0) {
                    if ($scope.DDMasterListPager.currentPage != 1)
                        $scope.DDMasterListPager.currentPage = $scope.DDMasterList.length === 1 ? $scope.DDMasterListPager.currentPage - 1 : $scope.DDMasterListPager.currentPage;
                } else {

                    if ($scope.DDMasterListPager.currentPage != 1 && $scope.SelectedDDMasterIds.length == $scope.DDMasterListPager.currentPageSize)
                        $scope.DDMasterListPager.currentPage = $scope.DDMasterListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedDDMasterIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.DDMasterListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteDDMaster, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.DDMasterList = response.Data.Items;
                        $scope.DDMasterListPager.currentPageSize = response.Data.Items.length;
                        $scope.DDMasterListPager.totalRecords = response.Data.TotalItems;
                    }
                });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };


    $scope.DDMasterListPager.getDataCallback = $scope.GetGeneralMasterList;
    $scope.DDMasterListPager.getDataCallback();

    $scope.$watch('DDMasterModel.DDMaster.ItemType', function (newValue, oldValue) {
        if (newValue !== null) {
            var filterDdType = $filter('filter')($scope.DDTypeList, { DDMasterTypeID: newValue }, true);
            if (filterDdType.length > 0) {
                $scope.DDMasterModel.DDMaster.IsDisplayValue = filterDdType[0].IsDisplayValue;
            } else {
                $scope.DDMasterModel.DDMaster.IsDisplayValue = false;
            }
        }
        else {
            $scope.DDMasterModel.DDMaster.IsDisplayValue = false;
        }


        if (ValideElement(filterDdType)) {
            $scope.CheckForParentChildMapping(filterDdType[0].DDMasterTypeID, $scope.DDMasterModel.DDMaster.DDMasterID);
        }


    });

    $scope.$watch('DDMasterModel.DDMaster.SelectedParentChild', function (newValue, oldValue) {
        if (newValue !== null) {
            var filterDdType = $filter('filter')($scope.ParentChildList, { DDMasterTypeID: parseInt(newValue) }, true);
            if (ValideElement(filterDdType) && filterDdType.length > 0) {
                $scope.DDMasterModel.DDMaster.SelectedParentChildText = filterDdType[0].Name;
            }
        }
        $scope.ShowParentChildList = false;
    });

    $scope.CheckForParentChildMapping = function (dDMasterTypeID, dDMasterID) {
        $scope.ShowParentChildList = true;
        $scope.DDMasterModel.DDMaster.SelectedParentChildValueItems = [];
        $scope.DDMasterModel.DDMaster.SelectedParentChildValueItem = "";
        if (dDMasterTypeID > 0) {
            var model = { DDMasterTypeID: dDMasterTypeID, DDMasterID: dDMasterID }
            var jsonData = angular.toJson(model);
            AngularAjaxCall($http, HomeCareSiteUrl.CheckForParentChildMappingUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.ParentChildList = response.Data.ParentChildList;

                    if ($scope.ParentChildList.length > 0)
                        $scope.DDMasterModel.DDMaster.SelectedParentChild = $scope.ParentChildList[0].DDMasterTypeID;


                    $scope.ParentChildValueList = response.Data.DDMasterList;
                    $scope.IsMultiSelect = response.Data.IsMultiSelect;

                    if (response.Data.SelectedParentChildValueItems.length > 0) {
                        if ($scope.IsMultiSelect) {
                            $scope.DDMasterModel.DDMaster.SelectedParentChildValueItems = response.Data.SelectedParentChildValueItems;
                        } else {
                            $scope.DDMasterModel.DDMaster.SelectedParentChildValueItem = response.Data.SelectedParentChildValueItems[0];
                        }
                    }

                    $timeout(function () {
                        $scope.ShowParentChildList = false;
                    });

                }
            });
        }
    };


    $scope.ShowMappingPopup = function (isOpen) {
        if (isOpen === true) {
            $('#addDDMasterModel').modal({
                backdrop: 'static',
                keyboard: false
            });
            $scope.MappingDDMaster.LuDDTypesParent = '';
        }
        else {
            $('#addDDMasterModel').modal('hide');
        }
    }

    $scope.ChildItemType = [];
    $scope.$watch('MappingDDMaster.LuDDTypesParent', function (newValue, oldValue) {
        if (newValue !== null) {
            var jsonData = angular.toJson({ DDMasterTypeID: newValue, isFetchParentRecord: false, parentID: 0 });
            AngularAjaxCall($http, HomeCareSiteUrl.GetParentChildMappingDDMasterUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.ChildItemType = response.Data.DDMasterTypesList;
                    $scope.DDParentMasterList = response.Data.DDMasterList;

                    $scope.MappingDDMaster.LuDDTypesChild = '';
                    $scope.MappingDDMaster.DDMasterIDParent = '';
                }
            });
        }
        else {
            $scope.ChildItemType = [];
            $scope.DDParentMasterList = [];
        }
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    });

    $scope.$watch('MappingDDMaster.LuDDTypesChild', function (newValue, oldValue) {
        if (newValue !== null) {
            var jsonData = angular.toJson({ DDMasterTypeID: newValue, isFetchParentRecord: true, parentID: $scope.MappingDDMaster.DDMasterIDParent });
            AngularAjaxCall($http, HomeCareSiteUrl.GetParentChildMappingDDMasterUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.DDChildMasterList = response.Data.DDMasterList;
                    if (response.Data.DDMasterIDChild.length > 0) {
                        $scope.DDMasterIDChild = response.Data.DDMasterIDChild;
                    }
                }
            });
        }
        else {
            $scope.DDChildMasterList = [];
        }
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    });

    $scope.$watch('MappingDDMaster.DDMasterIDParent', function (newValue, oldValue) {
        if (newValue !== null) {
            var jsonData = angular.toJson({ DDMasterTypeID: $scope.MappingDDMaster.LuDDTypesChild, isFetchParentRecord: true, parentID: $scope.MappingDDMaster.DDMasterIDParent });
            AngularAjaxCall($http, HomeCareSiteUrl.GetParentChildMappingDDMasterUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.DDChildMasterList = response.Data.DDMasterList;
                    if (response.Data.DDMasterIDChild.length > 0) {
                        $scope.DDMasterIDChild = response.Data.DDMasterIDChild;
                    }
                }
            });
        }
        else {
            $scope.DDChildMasterList = [];
        }
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    });

    $scope.SaveGeneralDDTasks = function () {
        var isValid = CheckErrors($("#frmDDMasterModelPopup"));
        if (isValid) {
            var jsonData = angular.toJson({ parentTaskId: $scope.MappingDDMaster.DDMasterIDParent, childTaskIds: $scope.DDMasterIDChild });
            AngularAjaxCall($http, HomeCareSiteUrl.SaveParentChildMappingUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.ShowMappingPopup(false);
                }
            });
        }
    }



};

controllers.AddPayorGroupController.$inject = ['$scope', '$http', '$timeout', '$window', '$filter'];

$(document).ready(function () {
});