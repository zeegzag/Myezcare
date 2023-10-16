var vm;
controllers.NotificationController = function ($scope, $http) {
    vm = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetWebNotificationListPage").val());
    };

    //$scope.WebNotificationModel = $scope.HomeModel.WebNotificationModel;
    //$scope.TempSearchWebNotificationModel = $scope.HomeModel.WebNotificationModel;
    $scope.SelectAllCheckbox = false;
    $scope.NotificationList = [];
    $scope.GetNotification = function () {

        var jsonData = angular.toJson({});
        AngularAjaxCall($http, HomeCareSiteUrl.GetLateClockInNotificationURL, jsonData, "Get", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.NotificationList = response.Data;
            }
        });
    };
    $scope.GetNotification();

    $scope.alert = function () {

        alert();
    }

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectAllNotifications = [];

        angular.forEach($scope.WebNotificationsList, function (item, key) {

            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked) {
                $scope.SelectAllNotifications.push(item.WebNotificationID);
                $scope.WebNotificationsList[key].IsSelected = true;
            }
            else {
                $scope.SelectAllNotifications.remove(item.WebNotificationID);
                $scope.WebNotificationsList[key].IsSelected = false;
            }
        });
        console.log('$scope.SelectAllNotifications.', $scope.SelectAllNotifications)
        return true;
    };

    $scope.WebNotificationsList = [];
    //#region Notifications
    $scope.WebNotificationsListPager = new PagerModule("Patient", "#WebNotificationsList", 'ASC');
    $scope.WebNotificationsListPager.pageSize = 10;
    $scope.GetNotificationsList = function (isSearchFilter) {
        $scope.WebNotificationsListPager.currentPage = isSearchFilter ? 1 : $scope.WebNotificationsListPager.currentPage;
        var pagermodel = {
            IsDeleted: $scope.WebNotificationDeleted,
            pageSize: $scope.WebNotificationsListPager.pageSize,
            pageIndex: $scope.WebNotificationsListPager.currentPage,
            sortIndex: $scope.WebNotificationsListPager.sortIndex,
            sortDirection: $scope.WebNotificationsListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);
        $scope.WebNotificationsListAjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetNotificationsListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.WebNotificationsList = response.Data.Items;
                $scope.WebNotificationsListPager.currentPageSize = response.Data.Items.length;
                $scope.WebNotificationsListPager.totalRecords = response.Data.TotalItems;

            }
            $scope.WebNotificationsListAjaxStart = false;
        });
    };
    $scope.WebNotificationsListPager.getDataCallback = $scope.GetNotificationsList;
    $scope.WebNotificationsListPager.getDataCallback(true);

    $scope.DeleteWebNotification = function (id) {
        debugger
        var selectedItems = $scope.WebNotificationsList.filter(item => item.IsSelected);
        var selectedIDs = selectedItems.map(item => item.WebNotificationID);
        var jsonData = '';
        if (selectedIDs.length > 0) {
            jsonData = angular.toJson({ ids: selectedIDs.join(',') });
        }
        else {
            jsonData = angular.toJson({ ids: id });
        }
        AngularAjaxCall($http, HomeCareSiteUrl.DeleteWebNotificationURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.GetNotificationsList();
                $scope.SelectAllCheckbox = false;
            }
        });
    };

    $scope.MarkSelectedAsRead = function (isRead) {
        var selectedItems = $scope.WebNotificationsList.filter(item => item.IsSelected);
        var selectedIDs = selectedItems.map(item => item.WebNotificationID);
        if (selectedIDs.length > 0) {
            var jsonData = angular.toJson({ webNotificationIDs: selectedIDs.join(','), isRead: isRead });
            AngularAjaxCall($http, HomeCareSiteUrl.MarkAsReadWebNotificationsURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.GetNotificationsList();
                    $scope.SelectAllCheckbox = false;
                }
            });
        }
    };

    $scope.SearchWebNotification = function () {
        $scope.WebNotificationsListPager.currentPage = 1;
        $scope.WebNotificationsListPager.getDataCallback(true);
    };

    //#endregion

    $(document).ready(function () {
        $scope.GetNotificationsList();
    });

};

controllers.NotificationController.$inject = ['$scope', '$http'];
