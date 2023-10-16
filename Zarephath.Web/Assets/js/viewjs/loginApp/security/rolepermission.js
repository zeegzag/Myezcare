var vm;


controllers.RolePermissionController = function ($scope, $http, $timeout) {
    vm = $scope;
    $scope.FillRolePermissionModel = $.parseJSON($("#hdnRolePermissionModel").val());
    $scope.SearchRolePermission = $scope.FillRolePermissionModel.SearchRolePermissionModel;
    $scope.RoleWisePermissionList = $scope.FillRolePermissionModel.RoleWisePermissionList;
    $scope.PermissionList = $scope.FillRolePermissionModel.PermissionList;
    $scope.RoleList = $scope.FillRolePermissionModel.RoleList;
    $scope.ListOfPermissionId = [];

    $scope.IsEditRoleName = false;

    angular.forEach($scope.PermissionList, function (data, index) {
        angular.forEach($scope.RoleWisePermissionList, function (item, i) {
            if (parseInt(data.id) === item.PermissionID)
                data.state = { selected: true };
        });
    });

    $scope.ExpandTreeView = function () {
        $scope.treeInstance.jstree(true).open_all();
    };

    $scope.ShowDescription = function (i, data) {
        var id = data.node.a_attr.id;
        $(data.instance.element).find('li a#' + id).attr('title', data.node.original.Description);
        //$(data.instance.element).find('li a#' + id).tooltip();
    };

    $scope.GetRolePermission = function () {
        $scope.IsEditRoleName = false;
        var jsonData = angular.toJson($scope.SearchRolePermission);
        if ($scope.SearchRolePermission.RoleID != undefined && $scope.SearchRolePermission.RoleID !== null && $scope.SearchRolePermission.RoleID !== "") {
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
                        });
                    } else
                        data.state = { selected: false }
                });
                $scope.PermissionList = angular.copy($scope.PermissionList);
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

    //Start: Add new Role using popup
    $scope.OpenAddRolePopUp = function () {
        OpenModalPopUp("#AddNewRoleModal", "#SearchRolePermissionModel_AddNewRoleName");
    };

    $scope.AddNewRole = function () {
        var jsonData = angular.toJson($scope.RoleModel);
        if (CheckErrors("#frmnewrole")) {
            AngularAjaxCall($http, SiteUrl.AddNewRoleURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.RoleModel.RoleName = '';
                    $scope.RoleList.push(response.Data);
                    $scope.RoleList = angular.copy($scope.RoleList);
                    $scope.SearchRolePermission.RoleID = response.Data.RoleID;
                    $scope.SearchRolePermission.RoleName = response.Data.RoleName;
                    $scope.GetRolePermission();
                    $("#AddNewRoleModal").modal('hide');
                } else
                    ShowMessages(response);
            });
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

    // Set Permission for perticular Role
    $scope.SaveRoleWisePermission = function (i, data) {
        if (data.action === "select_node" || data.action === "deselect_node") {
            if (data.action === "select_node")
                $scope.SearchRolePermission.IsSetToTrue = true;



            if (data.node.children.length === 0) {
                $scope.ListOfPermissionId.push(data.node.id);
            } else {
                //angular.forEach(data.node.children, function (data, index) {
                //    $scope.ListOfPermissionId.push(data);
                //});
                angular.forEach(data.node.children_d, function (d, index) {
                    var a = $('#jstreeid').jstree(true).get_node(d);
                    if (a.children.length == 0) {
                        $scope.ListOfPermissionId.push(a.id);
                    }
                });
            }
            $scope.SearchRolePermission.ListOfPermissionIdInCsv = $scope.ListOfPermissionId.toString();
            var jsonData = angular.toJson($scope.SearchRolePermission);
            AngularAjaxCall($http, SiteUrl.SaveRoleWisePermissionURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                $scope.SearchRolePermission.IsSetToTrue = false;
                ShowMessages(response);
            });
            $scope.ListOfPermissionId.length = 0;
        }
    };
    // Edit/Update RoleName

    //Start: Edit/Update RoleName
    $scope.EditRoleName = function () {
        $scope.IsEditRoleName = true;
        setTimeout(function () { $("#SearchRolePermissionModel_RoleName").focus(); }, 200);
    };

    $scope.UpdateRoleName = function () {
        var jsonData = angular.toJson($scope.SearchRolePermission);
        if (CheckErrors("#frmrolepermission")) {
            AngularAjaxCall($http, SiteUrl.UpdateRoleNameURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.IsEditRoleName = false;
                    angular.forEach($scope.RoleList, function (data, i) {
                        if (data.RoleID === response.Data.RoleID)
                            data.RoleName = response.Data.RoleName;
                    });
                } else {
                    ShowMessages(response);
                    $("#SearchRolePermissionModel_RoleName").focus();
                }
            });
        }
    };
    //End: Edit/Update RoleName

    //Set default values for jstree like, icons i have set (icon: 'glyphicon glyphicon-flash').
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
        'checkbox': {
            three_state: false,
            cascade: 'up'
        },
        plugins: ['checkbox']
    };
};
controllers.RolePermissionController.$inject = ['$scope', '$http', '$timeout'];