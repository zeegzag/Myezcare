var vm;
controllers.NotificationConfigurationController = function ($scope, $http, $window, $timeout) {
    vm = $scope;

    $scope.NotificationConfigurationModel = $.parseJSON($("#hdnNotificationConfigurationModel").val());
    $scope.NotificationConfigurationList = [];
    $scope.DetailDisabled = true;

    $scope.GetRoleMapping = function () {
        $scope.DetailDisabled = true;
        $scope.NotificationConfigurationList = [];
        if ($scope.NotificationConfigurationModel.RoleID > 0) {
            var jsonData = { notificationConfiguration: $scope.NotificationConfigurationModel };
            AngularAjaxCall($http, HomeCareSiteUrl.GetNotificationConfigurationDetailsURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.NotificationConfigurationModel = response.Data;
                    $scope.NotificationConfigurationList = $scope.ProcessList(response.Data.NotificationConfigurationList);
                    $scope.DetailDisabled = false;
                }
                ShowMessages(response);
            });
        }
    };

    $scope.ProcessList = function (list) {
        var newList = [{ id: 'parent', parent: '#', text: $("#hdnSelectOrDeselectAll").val(), state: { selected: false, opened: true } }];
        angular.forEach(list, function (data, index) {
            newList.push({ id: data.NotificationConfigurationID, parent: 'parent', text: data.ConfigurationName, state: { selected: data.IsSelected } });
        });
        return newList;
    };

    $scope.Save = function () {
        var isValid = CheckErrors($("#frmNotificationConfiguration"));
        if (isValid) {
            var selectedIds = $('#jstreeid').jstree(true).get_selected().filter(id => id > 0);
            if (selectedIds.length > 0) {
                $scope.NotificationConfigurationModel.SelectedNotificationConfigurationIDs = selectedIds.join(',');
                var jsonData = { notificationConfiguration: $scope.NotificationConfigurationModel };
                AngularAjaxCall($http, HomeCareSiteUrl.SaveNotificationConfigurationDetailsURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.GetRoleMapping();
                    }
                    ShowMessages(response);
                });
            } else {
                ShowMessages({ IsSuccess: false, Message: $("#hdnSelectRoleMessage").val() });
            }
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
                icon: 'fa fa-tag'
            }
        },
        plugins: ['types', 'checkbox']
    };

};
controllers.NotificationConfigurationController.$inject = ['$scope', '$http', '$window', '$timeout'];