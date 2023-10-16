var vm;
controllers.RolePermissionController = function ($scope, $http, $timeout) {
    vm = $scope;
    $scope.FillRolePermissionModel = $.parseJSON($("#hdnRolePermissionModel").val());
    $scope.SearchRolePermission = $scope.FillRolePermissionModel.SearchRolePermissionModel;
    $scope.RoleWisePermissionList = $scope.FillRolePermissionModel.RoleWisePermissionList;

    $scope.PermissionList = $scope.FillRolePermissionModel.PermissionList;
    $scope.MobilePermissionList = $scope.FillRolePermissionModel.MobilePermissionList;

    $scope.OrganizationList = $scope.FillRolePermissionModel.OrganizationList;
    $scope.ListOfPermissionId = [];
    $scope.ListOfPermissionIdSelected = [];
    $scope.ListOfPermissionIdNotSelected = [];

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

    $scope.GetRolePermission = function () {
        $scope.IsEditRoleName = false;
        var jsonData = angular.toJson($scope.SearchRolePermission);
        if ($scope.SearchRolePermission.OrganizationID != undefined && $scope.SearchRolePermission.OrganizationID !== null && $scope.SearchRolePermission.OrganizationID !== "") {
            AngularAjaxCall($http, SiteUrl.GetRolePermissionURL, jsonData, "Post", "json", "application/json").success(function (response) {                
                $scope.RoleWisePermissionList.length = 0;
                $scope.RoleWisePermissionList = response.Data.RoleWisePermissionList;

                angular.forEach($scope.PermissionList, function (data, index) {
                    data.state = { selected: false };
                });
                angular.forEach($scope.PermissionList, function (data, index) {
                    if ($scope.RoleWisePermissionList.length > 0) {
                        angular.forEach($scope.RoleWisePermissionList, function (item, i) {
                            if (parseInt(data.id) === item.PermissionID)
                                data.state = { selected: true }
                            // Satart Added by Sagar,22 Dec 2019 : disabled Task Entry Type
                            if (data.text.indexOf("Task Entry Type") >= 0) {                                
                                data.state = { selected: false, disabled: true };
                            }
                            //End
                        });
                    } else {                        
                        if (data.text.indexOf("Task Entry Type") >= 0) {
                            data.state = { selected: false, disabled: true };
                        } else {
                            data.state = { selected: false }
                        }
                    }

                });
                $scope.PermissionList = angular.copy($scope.PermissionList);


                angular.forEach($scope.MobilePermissionList, function (data, index) {
                    data.state = { selected: false };
                });
                angular.forEach($scope.MobilePermissionList, function (data, index) {
                    if ($scope.RoleWisePermissionList.length > 0) {
                        angular.forEach($scope.RoleWisePermissionList, function (item, i) {
                            if (parseInt(data.id) === item.PermissionID)
                                data.state = { selected: true }
                            // Start Added by Sagar disabled and uncheck Early Clockin
                            if (parseInt(data.id) === 3024 || parseInt(data.id) === 3038)
                                data.state = { selected: false, disabled: true };
                            // End
                            // Satart Added by Sagar,22 Dec 2019 : disabled Task Entry Type
                            if (data.text.indexOf("Task Entry Type") >= 0) {                                
                                data.state = { selected: false, disabled: true };
                            }
                            //End
                        });
                    } else {                        
                        // Start Added by Sagar disabled and uncheck Early Clockin
                        if (parseInt(data.id) === 3024 || parseInt(data.id) === 3038)
                            data.state = { selected: false, disabled: true };
                        // End
                        // start Added by Sagar,22 Dec 2019 : disabled Task Entry Type
                       else if (data.text.indexOf("Task Entry Type") >= 0) {                            
                            data.state = { selected: false, disabled: true };
                        }
                        else {
                            data.state = { selected: false }
                        }
                        //end
                    }
                        
                });
                $scope.MobilePermissionList = angular.copy($scope.MobilePermissionList);
                if (!response.IsSuccess)
                    ShowMessages(response);
            });
        } else {
            angular.forEach($scope.PermissionList, function (data, index) {
                data.state = { selected: false };
            });
            $scope.PermissionList = angular.copy($scope.PermissionList);
        }
    };

    $scope.ResetRoleModel = function () {
        HideErrors("#frmnewrole");
        setTimeout(function () { $("#SearchRolePermissionModel_RoleName").focus(); }, 200);
        if ($scope.RoleModel != undefined) {
            $scope.RoleModel.RoleName = "";
        }
    };
    //End:  Add new Role using popup
    $scope.SavePermissions = function () {
        $scope.ListOfPermissionIdSelected = [];
        $scope.ListOfPermissionIdNotSelected = [];
        angular.forEach($scope.PermissionList, function (data, index) {
            var node = $('#jstreeid').jstree(true).get_node(data.id);
            if (node.state && node.state.selected) {
                $scope.ListOfPermissionIdSelected.push(data.id);
            } else {
                $scope.ListOfPermissionIdNotSelected.push(data.id);
            }
        })
        angular.forEach($scope.MobilePermissionList, function (data, index) {
            var node = $('#jstreeidMobile').jstree(true).get_node(data.id);
            if (node.state && node.state.selected) {
                $scope.ListOfPermissionIdSelected.push(data.id);
            } else {
                $scope.ListOfPermissionIdNotSelected.push(data.id);
            }
        })
        $scope.SearchRolePermission.ListOfPermissionIdInCsvSelected = $scope.ListOfPermissionIdSelected.toString();
        $scope.SearchRolePermission.ListOfPermissionIdInCsvNotSelected = $scope.ListOfPermissionIdNotSelected.toString();
        var jsonData = angular.toJson($scope.SearchRolePermission);
        //console.log(jsonData);
        AngularAjaxCall($http, SiteUrl.SavePermissionURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            $scope.SearchRolePermission.IsSetToTrue = false;
            $scope.RoleWisePermissionList.length = 0;
            $scope.RoleWisePermissionList = response.Data;
            angular.forEach($scope.MobilePermissionList, function (data, index) {
                data.state = { selected: false };
            });
            angular.forEach($scope.MobilePermissionList, function (data, index) {
                if ($scope.RoleWisePermissionList.length > 0) {
                    angular.forEach($scope.RoleWisePermissionList, function (item, i) {
                        if (parseInt(data.id) === item.PermissionID)
                            data.state = { selected: true }
                        // Start Added by Sagar disabled and uncheck Early Clockin
                        if (parseInt(data.id) === 3024 || parseInt(data.id) === 3038)
                            data.state = { selected: false, disabled: true };
                        // End
                        // Satart Added by Sagar,22 Dec 2019 : disabled Task Entry Type
                        if (data.text.indexOf("Task Entry Type") >= 0)
                            data.state = { selected: false, disabled: true };
                        //End
                    });
                } else
                    data.state = { selected: false }
            });
            $scope.MobilePermissionList = angular.copy($scope.MobilePermissionList);
            ShowMessages(response);
        });

    }
    // Set Permission for perticular Role
    $scope.SaveRoleWisePermission = function (i, data) {
        // console.log($scope.PermissionList);
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
                //angular.forEach(data.node.children, function (data, index) {
                //    $scope.ListOfPermissionId.push(data);
                //});
                //$scope.ListOfPermissionId.push(data.node.id);
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
            // Start Added by Sagar 22 Dec 2019 : Select only one from Simple and details Task Type

            if (data.node.parent && (data.node.text == "Detail" || data.node.text == "Simple")) {
                var TaskparentNode = $('#jstreeid').jstree(true).get_node(data.node.parent);
                if (TaskparentNode.text.indexOf("Task Entry Type") >= 0) {
                    TaskparentNode.state.selected = false;
                    for (var i = 0; i < TaskparentNode.children_d.length; i++) {
                        var d = TaskparentNode.children_d[i];
                        var childTaskType = $('#jstreeid').jstree(true).get_node(d);
                        if (childTaskType.id != data.node.id) {
                            childTaskType.state.selected = false;
                        }
                    }
                }
            }
            $('#jstreeid').jstree(true).redraw(true);
            //End
            $scope.SearchRolePermission.ListOfPermissionIdInCsv = $scope.ListOfPermissionId.toString();
            var jsonData = angular.toJson($scope.SearchRolePermission);
            //AngularAjaxCall($http, HomeCareSiteUrl.SaveRoleWisePermissionURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            //    $scope.SearchRolePermission.IsSetToTrue = false;
            //    ShowMessages(response);
            //});
            $scope.ListOfPermissionId.length = 0;
        }
    };


    $scope.MobileSaveRoleWisePermission = function (i, data) {
        console.log("mobile");
        if (data.action === "select_node" || data.action === "deselect_node") {
            if (data.action === "select_node")
                $scope.SearchRolePermission.IsSetToTrue = true;

            if (data.node.children.length === 0) {
                $scope.ListOfPermissionId.push(data.node.id);
            } else {
                //angular.forEach(data.node.children, function (data, index) {
                //    $scope.ListOfPermissionId.push(data);
                //});
                $scope.ListOfPermissionId.push(data.node.id);
                angular.forEach(data.node.children_d, function (d, index) {
                    var a = $('#jstreeidMobile').jstree(true).get_node(d);
                    if (a.children.length == 0) {
                        $scope.ListOfPermissionId.push(a.id);
                    }
                });
            }
            if (data.node.parent && (data.node.parent == 3038)) {
                var EarlyClocinpatentnode = $('#jstreeidMobile').jstree(true).get_node(data.node.parent);
                if (EarlyClocinpatentnode.children_d.length > 0) {
                    angular.forEach(EarlyClocinpatentnode.children_d, function (d, index) {
                        var childealyclockin = $('#jstreeidMobile').jstree(true).get_node(d);
                        if (data.node.id != childealyclockin.id)
                            childealyclockin.state.selected = false;
                    });
                }

            }
            if (data.node.parent && (data.node.text == "Detail" || data.node.text == "Simple")) {
                var TaskparentNode = $('#jstreeidMobile').jstree(true).get_node(data.node.parent);
                if (TaskparentNode.text.indexOf("Task Entry Type") >= 0) {
                    TaskparentNode.state.selected = false;
                    for (var i = 0; i < TaskparentNode.children_d.length; i++) {
                        var d = TaskparentNode.children_d[i];
                        var childTaskType = $('#jstreeidMobile').jstree(true).get_node(d);
                        if (childTaskType.id != data.node.id) {
                            childTaskType.state.selected = false;
                        }
                    }
                }
            }
            //End

            $('#jstreeidMobile').jstree(true).redraw(true);
            //end

            $scope.SearchRolePermission.ListOfPermissionIdInCsv = $scope.ListOfPermissionId.toString();
            var jsonData = angular.toJson($scope.SearchRolePermission);

            AngularAjaxCall($http, HomeCareSiteUrl.SaveRoleWisePermissionURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                $scope.SearchRolePermission.IsSetToTrue = false;

                $scope.RoleWisePermissionList.length = 0;
                $scope.RoleWisePermissionList = response.Data;

                angular.forEach($scope.MobilePermissionList, function (data, index) {
                    data.state = { selected: false };
                });
                angular.forEach($scope.MobilePermissionList, function (data, index) {
                    if ($scope.RoleWisePermissionList.length > 0) {
                        angular.forEach($scope.RoleWisePermissionList, function (item, i) {
                            if (parseInt(data.id) === item.PermissionID)
                                data.state = { selected: true }
                        });
                    } else
                        data.state = { selected: false }
                });
                $scope.MobilePermissionList = angular.copy($scope.MobilePermissionList);
                ShowMessages(response);
            });
            $scope.ListOfPermissionId.length = 0;
        }
    };

    $scope.treeConfig = {
        multiple: true,
        animation: true,
        core: {
            error: function (error) {
                $log.error('RolePermissionController: error from js tree - ' + angular.toJson(error));
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
};
controllers.RolePermissionController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    $("#jstreeidMobile").jstree("select_node", "ul > li:third");
    var Selectednode = $("#navtree").jstree("get_selected");
    $("#jstreeidMobile").jstree("open_node", Selectednode, false, true);
});