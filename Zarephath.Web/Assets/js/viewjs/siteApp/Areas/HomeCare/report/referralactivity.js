var vm;


controllers.GroupTimesheetListController = function ($scope, $http, $timeout, StepsService, usSpinnerService) {
    vm = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetGroupTimesheetListPage").val());
    };
    $scope.ReferralActivityList = [];
    $scope.ReferralActivityList = $scope.newInstance();
    $scope.freshReferral = [];
    $scope.freshReferral.push($scope.ReferralActivityList.ReferralNotesList[0]);
    $scope.SelectedScheduleIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.SavedGroupTimesheetList = []
    $scope.Referrals = []
    $scope.ReferralNotes = []
    $scope.SelectedReferral= null
    $scope.IsClockInTimeOverrideValue = false
    $scope.IsClockOutTimeOverrideValue = false
    $scope.ClockInTimeOverrideValue = null
    $scope.ClockOutTimeOverrideValue = null
    $scope.TotalRecord = 0;
    $scope.EditPatient = 0;
    $scope.Year = new Date().getFullYear();
    $scope.selectedMonth = "";
    $scope.AddEdit = "0";
    $scope.showAddNotes = false;
    $scope.showAdd = false;
    $scope.showPrint = true;
    $scope.month = "Select Month";
    $scope.months = ["Select Month", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    $scope.status = 0;

    $scope.ReportNavigation = function () {
        var ReportName = "monthly Activity Report";
        var url = window.location.origin + "/Report/Template?ReportName=" + ReportName;

        var width = screen.availWidth - 10;
        var height = screen.availHeight - 60;
        var left = 0;
        var top = 0;
        var params = 'width=' + width + ', height=' + height;
        params += ', top=' + top + ', left=' + left;
        var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,' + params;
        var pdfWindow = window.open('about:blank', 'null', winFeature);
        pdfWindow.document.write("<html><head><style> * { box-sizing: border-box; padding: 0; margin: 0; border: 0; }</style>"
            + "<title>" + ReportName + "</title></head><body>"
            + '<embed width="100%" height="100%" name="plugin" src="' + url + '" '
            + 'type="application/pdf" internalinstanceid="21"></body></html>');
        pdfWindow.document.close();

    };

    $scope.RefreshReferrals = function (item) {
        var jsonData = angular.toJson({ Month: $scope.month, Year: $scope.Year, AddOrEdit: $scope.AddEdit });

        AngularAjaxCall($http, HomeCareSiteUrl.GetReferrals, jsonData, "Post", "json", "application/json").success(function (response) {
            $scope.Referrals = response.Data;
            $scope.validateReferralSearch();
            $scope.ReferralActivityList = $scope.newInstance();
        });
    }

    $scope.validateReferralSearch = function () {
        if ($scope.Referrals.length == 0 && $scope.AddEdit == "Edit" && $scope.month != "Select Month")
            ShowMessage("No patient entry found for the of " + $scope.month, "warning", 5000);
        return;
    }

    $scope.EnableAddReferral = function () {
        $scope.showAddNotes = true;
    }

    $scope.AddOrEdit = function (item) {
        $scope.showAddNotes = false;
        if ($scope.SearchGroupTimesheetListPage != undefined)
            $scope.SearchGroupTimesheetListPage.ReferralIDs = [];
        if (item == "Add") {
            $scope.showAdd = true;
            $scope.showEdit = false;
            $scope.RefreshReferrals();

        }
        else if (item == "Edit") {
            $scope.showEdit = true;
            $scope.showAdd = false;
            $scope.RefreshReferrals();
            $scope.EditPatient = 0;
        }
        else {
            $scope.showEdit = false;
            $scope.showAdd = false;
        }
        $scope.ReferralActivityList = $scope.newInstance();
        return;
    }

    $scope.SetSearchFilter = function () {
        $scope.SearchGroupTimesheetListPage = $scope.newInstance().SearchGroupTimesheetListPage;
    };

    $scope.SetSearchFilter();

    $scope.CommaSeparated = function (arr) {
        return arr ? (arr.join ? arr.join(',') : arr) : '';
    }

    $scope.SelectAllDays = function (item) {

        if (item.IsChecked == true) {
            item.Day1 = 1;
            item.Day2 = 1;
            item.Day3 = 1;
            item.Day4 = 1;
            item.Day5 = 1;
            item.Day6 = 1;
            item.Day7 = 1;
            item.Day8 = 1;
            item.Day9 = 1;
            item.Day10 = 1;
            item.Day11 = 1;
            item.Day12 = 1;
            item.Day13 = 1;
            item.Day14 = 1;
            item.Day15 = 1;
            item.Day16 = 1;
            item.Day17 = 1;
            item.Day18 = 1;
            item.Day19 = 1;
            item.Day20 = 1;
            item.Day21 = 1;
            item.Day22 = 1;
            item.Day23 = 1;
            item.Day24 = 1;
            item.Day25 = 1;
            item.Day26 = 1;
            item.Day27 = 1;
            item.Day28 = 1;
            item.Day29 = 1;
            item.Day30 = 1;
            item.Day31 = 1;
        }
        else {
            item.Day1 = 0;
            item.Day2 = 0;
            item.Day3 = 0;
            item.Day4 = 0;
            item.Day5 = 0;
            item.Day6 = 0;
            item.Day7 = 0;
            item.Day8 = 0;
            item.Day9 = 0;
            item.Day10 = 0;
            item.Day11 = 0;
            item.Day12 = 0;
            item.Day13 = 0;
            item.Day14 = 0;
            item.Day15 = 0;
            item.Day16 = 0;
            item.Day17 = 0;
            item.Day18 = 0;
            item.Day19 = 0;
            item.Day20 = 0;
            item.Day21 = 0;
            item.Day22 = 0;
            item.Day23 = 0;
            item.Day24 = 0;
            item.Day25 = 0;
            item.Day26 = 0;
            item.Day27 = 0;
            item.Day28 = 0;
            item.Day29 = 0;
            item.Day30 = 0;
            item.Day31 = 0;
        }
        $scope.CalculateDays(item);
    }

    $scope.setTotal = function () {
        //debugger;
        angular.forEach($scope.ReferralActivityList.ReferralActivityList, (item, key) => {
            item.Total = 0;
        });
    }

    function getTrueValue(item) {
        if (item == true || item == 1) {
            return 1;
        }
        else {
            return 0;
        }
    }

    $scope.CalculateDays = function (item) {
        //debugger;
        //angular.forEach($scope.ReferralActivityList.ReferralActivityList, (item, key) => {
        item.Total = getTrueValue(item.Day1) + getTrueValue(item.Day2) + getTrueValue(item.Day3) +
            getTrueValue(item.Day4) + getTrueValue(item.Day5) + getTrueValue(item.Day6) + getTrueValue(item.Day7)
            + getTrueValue(item.Day8) + getTrueValue(item.Day9) + getTrueValue(item.Day10) + getTrueValue(item.Day11)
            + getTrueValue(item.Day12) + getTrueValue(item.Day13) + getTrueValue(item.Day14) +
            getTrueValue(item.Day15) + getTrueValue(item.Day16) + getTrueValue(item.Day17) +
            getTrueValue(item.Day18) + getTrueValue(item.Day19) + getTrueValue(item.Day20) + getTrueValue(item.Day21)
            + getTrueValue(item.Day22) + getTrueValue(item.Day23) + getTrueValue(item.Day24) + getTrueValue(item.Day25)
            + getTrueValue(item.Day26) + getTrueValue(item.Day27) + getTrueValue(item.Day28) + getTrueValue(item.Day29)
            + getTrueValue(item.Day30) + getTrueValue(item.Day31);
        //});
    }

    $scope.setTotal();

    $scope.BindEdit = function (item) {

        $scope.SelectedReferral = item;

        var jsonData = angular.toJson({ Month: $scope.month, Year: $scope.Year, referralId: item });
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferral, jsonData, "Post", "json", "application/json").success(function (response) {
            $scope.ReferralActivityList.ReferralActivityList = response.Data;
        });

        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralNotes, jsonData, "Post", "json", "application/json").success(function (response) {
            $scope.ReferralNotes = response.Data;
        });
    }

    $scope.selectMonth = function (data) {
        $scope.selectedMonth = data;
    }

    $scope.DeleteNotes = function (item) {

        var jsonData = angular.toJson({ ReferralActivityNoteId: item, AddOrEdit: "DELETE" });

        AngularAjaxCall($http, HomeCareSiteUrl.EditDeleteReferralActivityNotes, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                jsonData = angular.toJson({ Month: $scope.month, Year: $scope.Year, referralId: $scope.SelectedReferral });

                AngularAjaxCall($http, HomeCareSiteUrl.GetReferralNotes, jsonData, "Post", "json", "application/json").success(function (response) {
                    $scope.ReferralNotes = response.Data;
                });
            }
        });
    }

    $scope.SaveNotes = function () {

        var jsonData = angular.toJson({
            referralActivityNotesModel: $scope.ReferralActivityList.ReferralNotesList[0],
            ReferralActivityNoteId: $scope.ReferralActivityList.ReferralNotesList[0].ReferralActivityNoteId,
            AddOrEdit: "EDIT"
        });

        AngularAjaxCall($http, HomeCareSiteUrl.EditDeleteReferralActivityNotes, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.status = 0;
                $scope.freshReferral[0].Description = null;
                $scope.freshReferral[0].Initials = null;
                $scope.freshReferral[0].ReferralActivityNoteId = null;
                $scope.ReferralActivityList.ReferralNotesList[0] = $scope.freshReferral[0];
                jsonData = angular.toJson({ Month: $scope.month, Year: $scope.Year, referralId: $scope.SelectedReferral });

                AngularAjaxCall($http, HomeCareSiteUrl.GetReferralNotes, jsonData, "Post", "json", "application/json").success(function (response) {
                    $scope.ReferralNotes = response.Data;
                });
            }
        });
    }

    $scope.EditNotes = function (item) {
        $scope.ReferralActivityList.ReferralNotesList[0].Description = item.Description;
        $scope.ReferralActivityList.ReferralNotesList[0].Initials = item.Initials;
        $scope.ReferralActivityList.ReferralNotesList[0].ReferralActivityNoteId = item.ReferralActivityNoteId;

        $scope.status = 1;
    }

    $scope.CancelEdit = function () {
        $scope.status = 0;
        $scope.freshReferral[0].Description = null;
        $scope.freshReferral[0].Initials = null;
        $scope.freshReferral[0].ReferralActivityNoteId = null;
        $scope.ReferralActivityList.ReferralNotesList[0] = $scope.freshReferral[0];
    }

    $scope.AddNotes = function () {
        
        var jsonData = angular.toJson({
            referralActivityNotesModel: $scope.ReferralActivityList.ReferralNotesList[0],
            referralId: $scope.SelectedReferral, Year: $scope.Year, Month: $scope.month
        });
        AngularAjaxCall($http, HomeCareSiteUrl.AddReferralActivityNotes, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.freshReferral[0].Description = null;
                $scope.freshReferral[0].Initials = null;
                $scope.freshReferral[0].ReferralActivityNoteId = null;
                $scope.ReferralActivityList.ReferralNotesList[0] = $scope.freshReferral[0];
                var jsonData = angular.toJson({ Month: $scope.month, Year: $scope.Year, referralId: $scope.SelectedReferral });

                AngularAjaxCall($http, HomeCareSiteUrl.GetReferralNotes, jsonData, "Post", "json", "application/json").success(function (response) {
                    $scope.ReferralNotes = response.Data;
                });
            }
        });
    }

    $scope.SaveEmployeeData = function () {

        if ($scope.SearchGroupTimesheetListPage == undefined || $scope.SearchGroupTimesheetListPage.ReferralIDs.length == 0) {
            ShowMessage("Please select partient !!", "error", 5000);
            return;
        }
        var jsonData = angular.toJson({ referralActivityModel: $scope.ReferralActivityList, refIds: $scope.SearchGroupTimesheetListPage.ReferralIDs, Year: $scope.Year, Month: $scope.month });
        AngularAjaxCall($http, HomeCareSiteUrl.SaveReferralActivity, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                // alert('Data saved');
                window.location.reload();
            }
        });
    }

    $scope.dateWithoutTimeZone = function (date) {
        if (date) {
            var userTimezoneOffset = date.getTimezoneOffset() * 60000;
            date = new Date(date.getTime() - userTimezoneOffset);
            //return moment.utc(date).valueOf();
        }
        return date;
    }

};

controllers.GroupTimesheetListController.$inject = ['$scope', '$http', '$timeout', 'StepsService', 'usSpinnerService'];

$(document).ready(function () {
    var dateformat = GetOrgDateFormat();
    $(".dateInputMask").attr("placeholder", dateformat);
    $('.time').inputmask({
        mask: "h:s t\\m",
        placeholder: "hh:mm a",
        alias: "datetime",
        hourFormat: "12"
    });

    $('#filters').on('hidden.bs.collapse', function () {
        $('#btn-filters').html('Show Filters');
    })

    $('#filters').on('shown.bs.collapse', function () {
        $('#btn-filters').html('Hide Filters');
    })
});



$("#right-scroll-button").click(function () {
    event.preventDefault();
    $(".table-responsive").animate(
        {
            scrollLeft: "+=300px"
        },
        "slow"
    );
});

$("#left-scroll-button").click(function () {
    event.preventDefault();
    $(".table-responsive").animate(
        {
            scrollLeft: "-=300px"
        },
        "slow"
    );
});
