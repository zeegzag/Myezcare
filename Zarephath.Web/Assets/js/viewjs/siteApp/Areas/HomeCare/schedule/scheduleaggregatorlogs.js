var vm;

controllers.ScheduleAggregatorLogsListController = function ($scope, $http, $timeout, $filter) {
    vm = $scope;

    $scope.NewInstance = function () {
        return $.parseJSON($("#hdnSetScheduleAggregatorLogsListPage").val());
    };

    $scope.ScheduleAggregatorLogsList = [];
    $scope.SelectedScheduleAggregatorLogsIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.CnclStatus = window.CancelStatus;
    $scope.EncryptedReferralID = window.EncryptedReferralID;

    $scope.ScheduleAggregatorLogsModel = $.parseJSON($("#hdnSetScheduleAggregatorLogsListPage").val());
    $scope.AddScheduleBatchService = $scope.NewInstance().AddScheduleBatchService;

    $scope.SearchScheduleAggregatorLogsListPage = $scope.ScheduleAggregatorLogsModel.SearchScheduleAggregatorLogsModel;

    $scope.SortingColumn = "StartDate";
    $scope.SortingType = "ASC";
    $scope.SearchScheduleAggregatorLogsListPage.IsPartial = $scope.ScheduleAggregatorLogsModel.IsPartial;
    if ($scope.ScheduleAggregatorLogsModel.IsPartial) {
        $scope.SearchScheduleAggregatorLogsListPage.ReferralID = $scope.EncryptedReferralID;
        $scope.SortingColumn = "StartDate";
        $scope.SortingType = "DESC";
    }

    $scope.TempSearchScheduleAggregatorLogsListPage = {};

    $scope.ScheduleAggregatorLogsListPager = new PagerModule($scope.SortingColumn, null, $scope.SortingType);

    $scope.SetPostData = function (fromIndex, model) {

        var pagermodel = {
            searchScheduleAggregatorLogsModel: $scope.SearchScheduleAggregatorLogsListPage,
            pageSize: $scope.ScheduleAggregatorLogsListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ScheduleAggregatorLogsListPager.sortIndex,
            sortDirection: $scope.ScheduleAggregatorLogsListPager.sortDirection,
            sortIndexArray: $scope.ScheduleAggregatorLogsListPager.sortIndexArray.toString()
        };
        if (model != undefined) {
            pagermodel.scheduleModel = model;
        }
        return angular.toJson(pagermodel);
    };




    $scope.SearchModelMapping = function () {
        $scope.SearchScheduleAggregatorLogsListPage.StartDate = $scope.TempSearchScheduleAggregatorLogsListPage.StartDate;
        $scope.SearchScheduleAggregatorLogsListPage.EndDate = $scope.TempSearchScheduleAggregatorLogsListPage.EndDate;
        $scope.SearchScheduleAggregatorLogsListPage.Name = $scope.TempSearchScheduleAggregatorLogsListPage.Name;
        $scope.SearchScheduleAggregatorLogsListPage.Address = $scope.TempSearchScheduleAggregatorLogsListPage.Address;
        $scope.SearchScheduleAggregatorLogsListPage.EmployeeID = $scope.TempSearchScheduleAggregatorLogsListPage.EmployeeID;
        $scope.SearchScheduleAggregatorLogsListPage.LastSent = $scope.TempSearchScheduleAggregatorLogsListPage.LastSent;
        $scope.SearchScheduleAggregatorLogsListPage.ClaimProcessor = $scope.TempSearchScheduleAggregatorLogsListPage.ClaimProcessor;
        $scope.SearchScheduleAggregatorLogsListPage.Status = $scope.TempSearchScheduleAggregatorLogsListPage.Status;
    };

    $scope.GetScheduleAggregatorLogsList = function (isSearchDataMappingRequire) {

        $scope.SelectAllCheckbox = false;
        $scope.SelectedScheduleAggregatorLogsIds = [];
        $scope.SearchScheduleAggregatorLogsListPage.ListOfIdsInCsv = [];


        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();


        var jsonData = $scope.SetPostData($scope.ScheduleAggregatorLogsListPager.currentPage);
        $scope.AjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetScheduleAggregatorLogsListURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
            $scope.AjaxStart = false;
            if (response.IsSuccess) {
                $timeout(function () {
                    //
                    if (response.Data.CurrentPage == 1)
                        $scope.ScheduleAggregatorLogsList = [];

                    if (response.Data.CurrentPage == 1 || $scope.ScheduleAggregatorLogsListPager.lastPage < response.Data.CurrentPage)
                        Array.prototype.push.apply($scope.ScheduleAggregatorLogsList, response.Data.Items);

                    $scope.ScheduleAggregatorLogsListPager.lastPage = response.Data.CurrentPage;
                    $scope.ScheduleAggregatorLogsListPager.currentPageSize = response.Data.Items.length;
                    $scope.ScheduleAggregatorLogsListPager.totalRecords = response.Data.TotalItems;

                    if (!$scope.$root.$$phase) {
                        $scope.$apply();
                    }
                });
            }
            ShowMessages(response);
        });
    };

    $scope.LogDetails = [];
    $scope.GetLogDetails = function (log) {
        if (!log.IsDetailsCaptured) {
            var model = {
                searchScheduleAggregatorLogsModel: {
                    ScheduleID: log.ScheduleID
                }
            };
            jsonData = angular.toJson(model);
            AngularAjaxCall($http, HomeCareSiteUrl.GetScheduleAggregatorLogsDetailsURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
                if (response.IsSuccess) {
                    $scope.LogDetails[log.ScheduleID] = response.Data;
                    log.IsDetailsCaptured = true;
                }
                ShowMessages(response);
            });
        }
    }

    $scope.Refresh = function () {
        $scope.ScheduleAggregatorLogsListPager.getDataCallback();
    };


    $scope.ResetSearchFilter = function () {
        $timeout(function () {

            //$("#AgencyID").select2("val", '');
            //$("#AgencyLocationID").select2("val", '');

            $scope.SearchScheduleAggregatorLogsListPage.StartDate = '';
            $scope.SearchScheduleAggregatorLogsListPage.EndDate = '';
            $scope.SearchScheduleAggregatorLogsListPage.Name = '';
            $scope.SearchScheduleAggregatorLogsListPage.Address = '';
            $scope.SearchScheduleAggregatorLogsListPage.EmployeeID = '';

            $scope.TempSearchScheduleAggregatorLogsListPage.StartDate = '';
            $scope.TempSearchScheduleAggregatorLogsListPage.EndDate = '';
            $scope.TempSearchScheduleAggregatorLogsListPage.Name = '';
            $scope.TempSearchScheduleAggregatorLogsListPage.Address = '';
            $scope.TempSearchScheduleAggregatorLogsListPage.EmployeeID = '';


            $scope.ScheduleAggregatorLogsListPager.currentPage = 1;
            $scope.ScheduleAggregatorLogsListPager.getDataCallback();
        });
    };

    $scope.ScheduleAggregatorLogsListPager.resetCallback = function () {
        $scope.ScheduleAggregatorLogsListPager.sortIndexArray = [];
        $scope.ScheduleAggregatorLogsListPager.currentPage = 1;
    };

    $scope.SearchScheduleAggregatorLogs = function () {
        $scope.ScheduleAggregatorLogsListPager.currentPage = 1;
        $scope.ScheduleAggregatorLogsListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectScheduleAggregatorLogs = function (schedule) {
        if (schedule.IsChecked)
            $scope.SelectedScheduleAggregatorLogsIds.push(schedule.ScheduleID);
        else
            $scope.SelectedScheduleAggregatorLogsIds.remove(schedule.ScheduleID);

        if ($scope.SelectedScheduleAggregatorLogsIds.length == $scope.ScheduleAggregatorLogsListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        //$scope.SelectedScheduleAggregatorLogsIds = [];
        angular.forEach($scope.ScheduleAggregatorLogsList, function (item, key) {
            item.IsChecked = $scope.SelectAllCheckbox;
            if (item.IsChecked) {
                var i = $.grep($scope.SelectedScheduleAggregatorLogsIds, function (data) {
                    return item.ScheduleID == data;
                });
                if (i.length == 0) {
                    $scope.SelectedScheduleAggregatorLogsIds.push(item.ScheduleID);
                }
            } else {
                $scope.SelectedScheduleAggregatorLogsIds.remove(item.ScheduleID);
            }
        });
        return true;
    };

    $scope.ScheduleAggregatorLogsListPager.getDataCallback = $scope.GetScheduleAggregatorLogsList;

    $scope.GetScheduleAggregatorLogsList();


    $(window).scroll(function () {
        if ($(window).scrollTop() == $(document).height() - $(window).height() && !$scope.AjaxStart) {
            $scope.ScheduleAggregatorLogsListPager.nextPage();
        }
    });

    $scope.LoadMore = function () {
        $scope.ScheduleAggregatorLogsListPager.nextPage();
    }

    $scope.ResendData = function (data) {
        var model = {
            searchScheduleAggregatorLogsModel: {
                ListOfIdsInCsv: data && data.ScheduleID > 0 ? data.ScheduleID.toString() : $scope.SelectedScheduleAggregatorLogsIds.toString()
            }
        };
        jsonData = angular.toJson(model);
        AngularAjaxCall($http, HomeCareSiteUrl.ResendAggregatorDataURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
            if (response.IsSuccess) {
                $scope.Refresh();
            }
            ShowMessages(response);
        });
    }
};
controllers.ScheduleAggregatorLogsListController.$inject = ['$scope', '$http', '$timeout', '$filter'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowScheduleAggregatorLogsMessage");
    var dateformat = GetOrgDateFormat();
    $(".dateInputMask").attr("placeholder", dateformat);
    $('.time').inputmask({
        mask: "h:s t\\m",
        placeholder: "hh:mm a",
        alias: "datetime",
        hourFormat: "12"
    });

});
