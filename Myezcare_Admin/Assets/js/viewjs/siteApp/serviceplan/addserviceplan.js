var vm;
controllers.AddServicePlanController = function ($scope, $http, $window, $timeout) {
    vm = $scope;

    $scope.SetServicePlanModel = $.parseJSON($("#hdnServicePlanModel").val());
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnServicePlanModel").val());
    };
    $scope.ServicePlan = $scope.SetServicePlanModel.ServicePlan;
    $scope.ServicePlanModules = $scope.SetServicePlanModel.ServicePlanModules;
    $scope.ListServicePlanComponent = $scope.SetServicePlanModel.ServicePlanComponentList;
    $scope.ShowWebPermissionError = false;
    $scope.ShowMobilePermissionError = false;

    $scope.SaveServicePlanDetails = function () {
        if (CheckErrors("#frmServicePlan",true)) {
            $scope.Temp = {};//angular.copy($scope.newInstance);
            $scope.Temp.ServicePlan = $scope.ServicePlan;
            $scope.Temp.ServicePlanComponentList = $scope.ListServicePlanComponent;

            var webPermissionArray = [], mobilePermissionArray = []; 
            $.each($('#jstreeid').jstree('get_selected'), function (i, e) {
                webPermissionArray.push(e);
            });
            $scope.ShowWebPermissionError = webPermissionArray.length == 0;
            $.each($('#jstreeidMobile').jstree('get_selected'), function (i, e) {
                mobilePermissionArray.push(e);
            });
            $scope.ShowMobilePermissionError = mobilePermissionArray.length == 0;
            if (webPermissionArray.length == 0 || mobilePermissionArray.length == 0) {
                ShowMessage(window.pleaseSelectPermission, "error");
                return;
            }

            $scope.Temp.ListOfPermissionIdInCsv = webPermissionArray.join(",");
            $scope.Temp.ListOfMobilePermissionIdInCsv = mobilePermissionArray.join(",");
            $scope.Temp.ServicePlanModules = $scope.ServicePlanModules;
            var jsonData = angular.toJson({ servicePlan: $scope.Temp });
            AngularAjaxCall($http, SiteUrl.AddServicePlanURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    SetMessageForPageLoad(response.Message, "ServicePlanUpdateSuccessMessage");
                    $window.location = SiteUrl.ServicePlanListURL;
                }
                else {
                    ShowMessages(response);
                }
            });
        }
    };
    $scope.Cancel = function () {
        $window.location = SiteUrl.ServicePlanListURL;
    };

    /*Start: Role Permissions */

    $scope.FillRolePermissionModel = $scope.SetServicePlanModel.SetRolePermissionModel;
    $scope.SearchRolePermission = $scope.FillRolePermissionModel.SearchRolePermissionModel;
    $scope.RoleWisePermissionList = $scope.FillRolePermissionModel.RoleWisePermissionList;

    $scope.PermissionList = $scope.FillRolePermissionModel.PermissionList;
    $scope.MobilePermissionList = $scope.FillRolePermissionModel.MobilePermissionList;

    $scope.ListOfPermissionId = [];
    $scope.ListOfMobilePermissionId = [];

    $scope.IsEditRoleName = false;

    angular.forEach($scope.PermissionList, function (data, index) {
        angular.forEach($scope.RoleWisePermissionList, function (item, i) {
            if (parseInt(data.id) === item.PermissionID)
                data.state = { selected: true };
        });
    });

    angular.forEach($scope.MobilePermissionList, function (data, index) {
        angular.forEach($scope.RoleWisePermissionList, function (item, i) {
            if (parseInt(data.id) === item.PermissionID)
                data.state = { selected: true };
        });
    });

    $scope.ExpandTreeView = function () {
        $scope.treeInstance.jstree(true).close_all();
        $scope.treeInstanceMobile.jstree(true).close_all();
    };

    $scope.ShowDescription = function (i, data) {
        var id = data.node.a_attr.id;
        $(data.instance.element).find('li a#' + id).attr('title', data.node.original.Description);
        //$(data.instance.element).find('li a#' + id).tooltip();
    };

    // Set Permission for perticular Role
    $scope.SaveRoleWisePermission = function (i, data) {
        if (data.action === "select_node" || data.action === "deselect_node") {
            if (data.action === "select_node") {
                $scope.SearchRolePermission.IsSetToTrue = true;
                $scope.selected = true;
            } else {
                $scope.selected = false;
            }

            if (data.node.children.length > 0 && data.node.parents.length > 0) {
                $scope.ListOfPermissionId.push(data.node.id);
            }

            if (data.node.children.length === 0) {
                $scope.ListOfPermissionId.push(data.node.id);
            } else {
                angular.forEach(data.node.children_d, function (d, index) {
                    var a = $('#jstreeid').jstree(true).get_node(d);
                    if (a.children.length == 0) {
                        $scope.ListOfPermissionId.push(a.id);
                    }
                });
            }

            data.node.parents.splice(-1, 1);
            angular.forEach(data.node.parents, function (parentId, index) {
                var paretnData = $('#jstreeid').jstree(true).get_node(parentId);
                if (paretnData.state.selected == $scope.selected) {
                    $scope.ListOfPermissionId.push(parentId);
                }
            });

            $scope.SearchRolePermission.ListOfPermissionIdInCsv = $scope.ListOfPermissionId.toString();
        }
    };

    $scope.MobileSaveRoleWisePermission = function (i, data) {

        if (data.action === "select_node" || data.action === "deselect_node") {
            if (data.action === "select_node")
                $scope.SearchRolePermission.IsSetToTrue = true;

            if (data.node.children.length === 0) {
                $scope.ListOfMobilePermissionId.push(data.node.id);
            } else {
                $scope.ListOfMobilePermissionId.push(data.node.id);
                angular.forEach(data.node.children_d, function (d, index) {
                    var a = $('#jstreeidMobile').jstree(true).get_node(d);
                    if (a.children.length == 0) {
                        $scope.ListOfMobilePermissionId.push(a.id);
                    }
                });
            }

            $scope.SearchRolePermission.ListOfMobilePermissionIdInCsv = $scope.ListOfMobilePermissionId.toString();
        }
    };

    //Set default values for jstree like, icons i have set (icon: 'glyphicon glyphicon-flash').
    $scope.treeConfig = {
        multiple: true,
        animation: true,
        core: {
            error: function (error) {
                $log.error('AddServicePlanController: error from js tree - ' + angular.toJson(error));
            },
            check_callback: true
        },
        types: {
            default: {
                icon: 'glyphicon glyphicon-flash'
            }
        },
        plugins: ['types', 'checkbox']
    };

    /* End: Role Permissions */

    //#region Service Plan Components
    $scope.ServicePlanComponentTokenObj = {};
    $scope.SearhServicePlanComponentURL = SiteUrl.SearhServicePlanComponentURL;

    $scope.ServicePlanComponentResultsFormatter = function (item) {
        return "<li id='{0}'>{0}</li>".format(item.Title);
    };
    $scope.ServicePlanComponentTokenFormatter = function (item) {
        return "<li id='{0}'>{0}</li>".format(item.Title);
    };

    $scope.AddedServicePlanComponent = function (item) {
        //$scope.ListServicePlanComponent.push(item);
        //$scope.ServicePlanComponentTokenObj.clear();

        var push = true;
        if (_.findWhere($scope.ListServicePlanComponent, item) == null) {
            angular.forEach($scope.ListServicePlanComponent, function (items) {
                if (items.Title == item.Title) {
                    push = false;
                }
            });
            if (push) {
                $scope.ListServicePlanComponent.push(item);
            }
        }
        $scope.ServicePlanComponentTokenObj.clear();
    };

    $scope.DeleteServicePlanComponent = function (item, index) {
        $scope.ListServicePlanComponent = $scope.ListServicePlanComponent.filter(function (obj) {
            return obj.DDMasterID !== item.DDMasterID;
        });
    };

    //#endregion


};
controllers.AddServicePlanController.$inject = ['$scope', '$http', '$window', '$timeout'];