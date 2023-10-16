var vm;
controllers.RolePermissionController = function ($scope, $http, $timeout) {
    vm = $scope;
    $scope.FillRolePermissionModel = $.parseJSON($("#hdnRolePermissionModel").val());
    $scope.SearchRolePermission = $scope.FillRolePermissionModel.SearchRolePermissionModel;
    $scope.RoleWisePermissionList = $scope.FillRolePermissionModel.RoleWisePermissionList;
    $scope.ListOfReports = [];
    $scope.SelectedReportIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.PermissionType = '';
    $scope.PermissionList = $scope.FillRolePermissionModel.PermissionList;
    $scope.MobilePermissionList = $scope.FillRolePermissionModel.MobilePermissionList;

    $scope.RoleList = $scope.FillRolePermissionModel.RoleList;
    $scope.ListOfPermissionId = [];
    $scope.ListOfPermissionIdSelected = [];
    $scope.ListOfPermissionIdNotSelected = [];
    $scope.IsEditRoleName = false;

    angular.forEach($scope.PermissionList, function (data, index) {
        angular.forEach($scope.RoleWisePermissionList, function (item, i) {
            if (parseInt(data.id) === item.PermissionID)
                data.state = { selected: true };
            // Satart Added by Sagar,22 Dec 2019 : disabled Task Entry Type
            if (data.text.indexOf("Task Entry Type") >= 0)
                data.state = { selected: false, disabled: true };
            //End
        });
    });

    angular.forEach($scope.MobilePermissionList, function (data, index) {
        angular.forEach($scope.RoleWisePermissionList, function (item, i) {
            if (parseInt(data.id) === item.PermissionID)
                data.state = { selected: true };
            // Start Added by Sagar disabled and uncheck Early Clockin
            if (parseInt(data.id) === 3024 || parseInt(data.id) === 3038)
                data.state = { selected: false, disabled: true };
            // End
            // Satart Added by Sagar,22 Dec 2019 : disabled Task Entry Type
            if (data.text.indexOf("Task Entry Type") >= 0)
                data.state = { selected: false, disabled: true };
            //END
            // Start Added by Akhilesh 16  NOV 2019 : Select only one Item from Covid Survey 
            if (data.text.indexOf("Covid_Survey") >= 0)
                data.state = { selected: false, disabled: true };
            //End

            //if (parseInt(data.id) == 3016)
            //    data.state = { opened: true };

        });
    });
    $scope.GetPermissionType = function (permissionType) {
        if (permissionType == 3) {
            $scope.GetReports();
        }
        $scope.PermissionType = permissionType;
    }

    $scope.getSiblings = function (tree, nodeID, parent) {
        var parentNode = tree.get_node(parent),
            aChildren = parentNode.children,
            aSiblings = [];

        aChildren.forEach(function (c) {
            if (c !== nodeID) aSiblings.push(c);
        });
        return aSiblings;
    };

    $scope.ExpandTreeView = function () {
        $scope.treeInstance.jstree(true).close_all();
        $scope.treeInstanceMobile.jstree(true).close_all();
        //$scope.treeInstanceMobile.jstree(true)._model.data[3016].state.opened = true;
        //$scope.treeInstanceMobile.jstree(true)._model.data[3017].state.opened = true;
    };

    $scope.ShowDescription = function (i, data) {
        var id = data.node.a_attr.id;
        $(data.instance.element).find('li a#' + id).attr('title', data.node.original.Description);
        //$(data.instance.element).find('li a#' + id).tooltip();
    };

    $scope.SelectReport = function (report) {
        debugger
        if (report.IsChecked) {
            $scope.SelectedReportIds.push(report.ReportID);
        } else {       
            $scope.SelectedReportIds.remove(report.ReportID);
        }
        //console.log($scope.SelectedReportIds)
    };

    $scope.checkAll = function (selectAll) {
        debugger;
        $scope.SelectedReportIds = [];
        var someData = $scope.ListOfReports;
        if (selectAll) {
            angular.forEach($scope.ListOfReports, function (item, key) {
                someData[key]['IsChecked'] = true;
                $scope.SelectedReportIds.push(item.ReportID);
            });
        } else {
            angular.forEach($scope.ListOfReports, function (item, key) {
                someData[key]['IsChecked'] = false;
                $scope.SelectedReportIds.remove(item.ReportID);
            });
        }
        $scope.ListOfReports = someData
    };

    $scope.GetReports = function () {
        debugger
        var some = [];
        var jsonData = angular.toJson($scope.SearchRolePermission);
        
        if ($scope.SearchRolePermission.RoleID != undefined && $scope.SearchRolePermission.RoleID !== null && $scope.SearchRolePermission.RoleID !== "") {
            AngularAjaxCall($http, HomeCareSiteUrl.GetReportMasterListUrl, jsonData, "Get", "json", "application/json").success(function (response) {
                GetMapReports();
                some = response.Data;
                angular.forEach(some, function (item, key) {
                    some[key].IsChecked = false;
                });
                $scope.ListOfReports = some;
            });
        }
       
    };

    function GetMapReports() {
        debugger
        var reportData = angular.toJson({ 'RoleID': $scope.SearchRolePermission.RoleID, 'ReportID': $scope.SelectedReportIds.join(','), 'IsSetToTrue': true });
        if ($scope.SearchRolePermission.RoleID != undefined && $scope.SearchRolePermission.RoleID !== null && $scope.SearchRolePermission.RoleID !== "") {
            AngularAjaxCall($http, HomeCareSiteUrl.GetMapReportURL, reportData, "Post", "json", "application/json").success(function (response) {
                var allData = [];
                angular.forEach($scope.ListOfReports, function (item, key) {
                    item.IsChecked = false;
                    angular.forEach(response, function (data, key) {
                        if (data.ReportID == item.ReportID) {
                            item.IsChecked = true;
                            $scope.SelectedReportIds.push(item.ReportID)
                        }
                    });
                    allData.push(item);
                });
                $scope.ListOfReports = allData;
            });
        }
    };

    $scope.GetRolePermission = function () {
        $scope.IsEditRoleName = false;
        var jsonData = angular.toJson($scope.SearchRolePermission);
        if ($scope.SearchRolePermission.RoleID != undefined && $scope.SearchRolePermission.RoleID !== null && $scope.SearchRolePermission.RoleID !== "") {
            AngularAjaxCall($http, HomeCareSiteUrl.GetRolePermissionURL, jsonData, "Post", "json", "application/json").success(function (response) {
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

    //Start: Add new Role using popup
    $scope.OpenAddRolePopUp = function () {
        OpenModalPopUp("#AddNewRoleModal", "#SearchRolePermissionModel_AddNewRoleName");
    };

    //Start: Add new Report Mapping using popup
    $scope.OpenMapReportPopUp = function () {
        OpenModalPopUp("#AddMapReportModal");
    };

    $scope.AddNewRole = function () {
        var jsonData = angular.toJson($scope.RoleModel);
        if (CheckErrors("#frmnewrole")) {
            AngularAjaxCall($http, HomeCareSiteUrl.AddNewRoleURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.RoleModel.RoleName = '';
                    $scope.RoleList.push(response.Data);
                    $scope.RoleList = angular.copy($scope.RoleList);
                    $scope.SearchRolePermission.RoleID = response.Data.RoleID;
                    $scope.SearchRolePermission.RoleName = response.Data.RoleName;
                    $scope.GetRolePermission();
                    $("#AddNewRoleModal").modal('hide');
                    ShowMessages(response);
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
    $scope.SavePermissions = function () {
        $scope.ListOfPermissionIdSelected = [];
        $scope.ListOfPermissionIdNotSelected = [];
        angular.forEach($scope.PermissionList, function (data, index) {
            var node = $('#jstreeid').jstree(true).get_node(data.id);
            if (node.state.selected) {
                $scope.ListOfPermissionIdSelected.push(data.id);
            } else {
                $scope.ListOfPermissionIdNotSelected.push(data.id);
            }
        })
        angular.forEach($scope.MobilePermissionList, function (data, index) {
            var node = $('#jstreeidMobile').jstree(true).get_node(data.id);
            if (node.state.selected) {
                $scope.ListOfPermissionIdSelected.push(data.id);
            } else {
                $scope.ListOfPermissionIdNotSelected.push(data.id);
            }
        })
        $scope.SearchRolePermission.ListOfPermissionIdInCsvSelected = $scope.ListOfPermissionIdSelected.toString();
        $scope.SearchRolePermission.ListOfPermissionIdInCsvNotSelected = $scope.ListOfPermissionIdNotSelected.toString();
        var jsonData = angular.toJson($scope.SearchRolePermission);
        //console.log(jsonData);
        AngularAjaxCall($http, HomeCareSiteUrl.SavePermissionURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
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
        if (data.action === "select_node" || data.action === "deselect_node") {
             if (data.action === "select_node") {
                $scope.SearchRolePermission.IsSetToTrue = true;
                $scope.selected = true;
                let pa = $('#jstreeid').jstree(true).get_node(data.node.parent);
                pa.state.selected = true;

            } else {
                $scope.selected = false;
                // Starts here Changes by Aditya bhushan on 02/08/2022
                // Uncheck all children when parent node unchecked
                if (data.node.children.length > 0) {
                    angular.forEach(data.node.children_d, function (d, index) {
                        var a = $('#jstreeid').jstree(true).get_node(d);
                        a.state.selected = false;
                    });
                }

                // Uncheck parent node if there are no children selected
                let pNode = data.instance.get_node(data.node.parent);
                let children = data.instance.get_node(pNode).children;
                if (children.length > 0) {
                    let result = false;
                    for (let i = 0; i < children.length; i++) {
                        result = data.instance.is_selected(children[i]);
                        if (data.instance.is_selected(children[i])) {
                            result = true;
                            break;
                        }
                    }
                    pNode.state.selected = result;
                }
                // Ends here
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
                    // Starts here Changes by Aditya bhushan on 02/08/2022
                    // Check all children when parent node checked
                    if (data.action === "select_node") {
                        a.state.selected = true;
                    }
                    // Ends here
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

            if (data.node.original && data.node.original.PermissionCode == "EmployeeList_Group") {
                if (data.action !== "select_node") {
                    data.instance.deselect_node(data.node.children);
                }
            } else if (data.node.original && (data.node.original.PermissionCode == "EmployeeList_AnyGroup_Employees" || data.node.original.PermissionCode == "EmployeeList_SameGroup_Employees")) {
                if (data.instance.is_selected(data.node.parent)) {
                    var sibl = $scope.getSiblings(data.instance, data.node.id, data.node.parent);
                    if (data.action === "select_node") {
                        data.instance.deselect_node(sibl);
                    }
                    else {
                        data.instance.deselect_node(data.node.id);
                        data.instance.select_node(sibl);
                    }
                } else {
                    data.instance.deselect_node(data.instance.get_node(data.node.parent).children);
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

    $scope.SaveMapReport = function () {
        debugger
        var data = { 'RoleID': $scope.SearchRolePermission.RoleID, 'ReportID': $scope.SelectedReportIds.join(','), 'IsSetToTrue': true }
        var jsonData = angular.toJson(data);
        AngularAjaxCall($http, HomeCareSiteUrl.SaveMapReportURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            $scope.SearchRolePermission.IsSetToTrue = false;
            $("#AddMapReportModal").modal('hide');
            $scope.SelectedReportIds = [];
            ShowMessages(response);
            GetMapReports();
        });
    };

    $scope.MobileSaveRoleWisePermission = function (i, data) {
        if (data.action === "select_node" || data.action === "deselect_node") {
            if (data.action === "select_node") {
                $scope.SearchRolePermission.IsSetToTrue = true;
                var pa = $('#jstreeidMobile').jstree(true).get_node(data.node.parent);
                pa.state.selected = true;
            } else {
                // Starts here Changes by Aditya bhushan on 02/08/2022
                // Uncheck all children when parent node unchecked
                if (data.node.children.length > 0) {
                    angular.forEach(data.node.children_d, function (d, index) {
                        var a = $('#jstreeidMobile').jstree(true).get_node(d);
                        a.state.selected = false;
                    });
                }

                // Uncheck parent node if there are no children selected
                let pNode = data.instance.get_node(data.node.parent);
                let children = data.instance.get_node(pNode).children;
                if (children.length > 0) {
                    let result = false;
                    for (let i = 0; i < children.length; i++) {
                        result = data.instance.is_selected(children[i]);
                        if (data.instance.is_selected(children[i])) {
                            result = true;
                            break;
                        }
                    }
                    pNode.state.selected = result;
                }
                // Ends here
            }
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
                    // Starts here Changes by Aditya bhushan on 02/08/2022
                    if (data.action === "select_node") {
                        a.state.selected = true;
                    }
                    // Ends here
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
            // Start : create by  Sagar :- Auto approved and manul approved required
            if (data.node.parent && (data.node.parent == "3016" || data.node.id == "3016")) {
                var manualapprovereq = $('#jstreeidMobile').jstree(true).get_node(3017);
                if (manualapprovereq.children_d.length > 0) {
                    manualapprovereq.state.selected = false;
                    for (var i = 0; i < manualapprovereq.children_d.length; i++) {
                        var d = manualapprovereq.children_d[i];
                        var childmanualapprovereq = $('#jstreeidMobile').jstree(true).get_node(d);
                        if ((childmanualapprovereq.parent == manualapprovereq.id && childmanualapprovereq.text.indexOf(data.node.text) >= 0) || (data.node.id == "3016")) {
                            childmanualapprovereq.state.selected = false;
                        }
                        if ((childmanualapprovereq.parent == manualapprovereq.id && childmanualapprovereq.id == 3015 && data.node.id == "3008") || (data.node.id == "3016")) {
                            childmanualapprovereq.state.selected = false;
                        }
                        if ((childmanualapprovereq.parent == manualapprovereq.id && childmanualapprovereq.id == 3018 && data.node.id == "3012") || (data.node.id == "3016")) {
                            childmanualapprovereq.state.selected = false;
                        }
                    }
                }

            }
            if (data.node.parent && (data.node.parent == "3017" || data.node.id == "3017")) {
                var manualapprovereq = $('#jstreeidMobile').jstree(true).get_node(3016);
                if (manualapprovereq.children_d.length > 0) {
                    manualapprovereq.state.selected = false;
                    for (var j = 0; j < manualapprovereq.children_d.length; j++) {
                        var d = manualapprovereq.children_d[j];
                        var childmanualapprovereq = $('#jstreeidMobile').jstree(true).get_node(d);
                        if ((childmanualapprovereq.parent == manualapprovereq.id && childmanualapprovereq.text.indexOf(data.node.text) >= 0) || (data.node.id == "3017")) {
                            childmanualapprovereq.state.selected = false;
                        }
                        if ((childmanualapprovereq.parent == manualapprovereq.id && childmanualapprovereq.id == 3008 && data.node.id == "3015") || (data.node.id == "3017")) {
                            childmanualapprovereq.state.selected = false;
                        }
                        if ((childmanualapprovereq.parent == manualapprovereq.id && childmanualapprovereq.id == 3012 && data.node.id == "3018") || (data.node.id == "3017")) {
                            childmanualapprovereq.state.selected = false;
                        }
                    }
                }
            }
            // Start Added by Sagar 22 Dec 2019 : Select only one from Simple and details Task Type
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
            // Start Added by Akhilesh 16  NOV 2019 : Select only one Item from Covid Survey 
            if (data.node.parent && (data.node.text == "Mandatory" || data.node.text == "NotRequired" || data.node.text == "Optional")) {
                var TaskparentNode = $('#jstreeidMobile').jstree(true).get_node(data.node.parent);
                if (TaskparentNode.text.indexOf("Covid_Survey") >= 0) {
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

            // Changed by Sagar
            //AngularAjaxCall($http, HomeCareSiteUrl.SaveRoleWisePermissionURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            //    $scope.SearchRolePermission.IsSetToTrue = false;

            //    $scope.RoleWisePermissionList.length = 0;
            //    $scope.RoleWisePermissionList = response.Data;

            //    angular.forEach($scope.MobilePermissionList, function (data, index) {
            //        data.state = { selected: false };
            //    });
            //    angular.forEach($scope.MobilePermissionList, function (data, index) {
            //        if ($scope.RoleWisePermissionList.length > 0) {
            //            angular.forEach($scope.RoleWisePermissionList, function (item, i) {
            //                if (parseInt(data.id) === item.PermissionID)
            //                    data.state = { selected: true }
            //            });
            //        } else
            //            data.state = { selected: false }
            //    });
            //    $scope.MobilePermissionList = angular.copy($scope.MobilePermissionList);
            //    ShowMessages(response);
            //});
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
        console.log(jsonData);
        if (CheckErrors("#frmrolepermission")) {
            AngularAjaxCall($http, HomeCareSiteUrl.UpdateRoleNameURL, jsonData, "Post", "json", "application/json").success(function (response) {
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
        checkbox: { cascade: "", three_state: false },
        plugins: ['types', 'checkbox']
    };
};
controllers.RolePermissionController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    $("#jstreeidMobile").jstree("select_node", "ul > li:third");
    var Selectednode = $("#navtree").jstree("get_selected");
    $("#jstreeidMobile").jstree("open_node", Selectednode, false, true);

    $("#jstreeid")
        .on('select_node.jstree', function (e, data) {
            if (data.event) {
                if (data.node.original && data.node.original.PermissionCode != "EmployeeList_Group") {
                    data.instance.select_node(data.node.children);
                }
            }
        })
        .on('deselect_node.jstree', function (e, data) {
            if (data.event) {
                data.instance.deselect_node(data.node.children);
            }
        });
});