var vm;


controllers.NurseSignatureController = function ($scope, $http, $timeout) {
    vm = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetNurseSignaturePage").val());
    };

    $scope.NurseSignatureModel = $scope.newInstance();
    $scope.NurseSignatureList = [];
    $scope.ApprovedEmployeeVisitIds = [];


    $scope.SetSearchFilter = function () {
        $scope.SearchNurceTimesheetListPage = $scope.newInstance().SearchNurceTimesheetListPage;
        

    };
    $scope.SetSearchFilter();

    $scope.CommaSeparated = function (arr) {
        return arr ? (arr.join ? arr.join(',') : arr) : '';
    }

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            SearchNurceTimesheetListPage: {
                ...$scope.SearchNurceTimesheetListPage,
                EmployeeIDs: $scope.CommaSeparated($scope.SearchNurceTimesheetListPage.EmployeeIDs),
                ReferralIDs: $scope.CommaSeparated($scope.SearchNurceTimesheetListPage.ReferralIDs),
                CareTypeIDs: $scope.CommaSeparated($scope.SearchNurceTimesheetListPage.CareTypeIDs),
            },
            pageSize: $scope.NurseSignatureListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.NurseSignatureListPager.sortIndex,
            sortDirection: $scope.NurseSignatureListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };
    $scope.Cancel1 = function (data) {
        data.IsEditable = false;
    }

    // This executes when select single checkbox selected in table.   
    $scope.SelectEmployeeVisitApprove = function (EmployeeVisit) {
        if (!EmployeeVisit.CanApprove) {
            EmployeeVisit.IsChecked = false;
        }

        if (EmployeeVisit.IsChecked)
            $scope.ApprovedEmployeeVisitIds.push(EmployeeVisit.EmployeeVisitID);
        else
            $scope.ApprovedEmployeeVisitIds.remove(EmployeeVisit.EmployeeVisitID);

        if ($scope.ApprovedEmployeeVisitIds.length > 0 &&
            $scope.ApprovedEmployeeVisitIds.length == $scope.NurseSignatureList.filter(item => item.CanApprove).length)
            $scope.SelectAllApproveCheckbox = true;
        else
            $scope.SelectAllApproveCheckbox = false;
    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAllApprove = function (oldValue) {
        var newValue = !oldValue;
        $scope.ApprovedEmployeeVisitIds = [];

        angular.forEach($scope.NurseSignatureList, function (item, key) {
            if (!item.CanApprove) {
                item.IsChecked = false;
            } else {
                item.IsChecked = newValue;
            }
            if (item.IsChecked) {
                $scope.ApprovedEmployeeVisitIds.push(item.EmployeeVisitID);
            }
        });

        if ($scope.ApprovedEmployeeVisitIds.length > 0 &&
            $scope.ApprovedEmployeeVisitIds.length == $scope.NurseSignatureList.filter(item => item.CanApprove).length)
            $scope.SelectAllApproveCheckbox = true;
        else
            $scope.SelectAllApproveCheckbox = false;
        return true;
    };

    $scope.GetNurseSignatureList = function () {
        var jsonData = $scope.SetPostData($scope.NurseSignatureListPager.currentPage);
        $scope.NurseSignatureListAjaxStart = true;
        $scope.NurseSignatureList.length = 0;

        AngularAjaxCall($http, HomeCareSiteUrl.GetNurseSignatureList, jsonData, "Post", "json", "application/json").success(function (response) {

            if (response.IsSuccess) {
                $scope.NurseSignatureList = response.Data.Items;

                $scope.NurseSignatureListPager.currentPageSize = response.Data.Items.length;
                $scope.NurseSignatureListPager.totalRecords = response.Data.TotalItems;

                var selectAll = false;
                $scope.NurseSignatureList.forEach(v => {
                    v.CanApprove = true;
                    if (v.CanApprove) { selectAll = true; }
                });
                $scope.SelectAllApproveCheckbox = false;
                if (selectAll) { $scope.SelectAllApprove(false); }

            }
            $scope.NurseSignatureListAjaxStart = false;
            ShowMessages(response);
        });
    };

    $scope.NurseSignatureListPager = new PagerModule("EmployeeVisitID");
    $scope.NurseSignatureListPager.sortDirection = "DESC";
    $scope.NurseSignatureListPager.getDataCallback = $scope.GetNurseSignatureList;

    $scope.SearchNurseSignature = function () {
        /* alert("qwqwqw");*/
        $scope.NurseSignatureListPager.currentPage = 1;
        $scope.NurseSignatureListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {

        $scope.SetSearchFilter();
        $scope.NurseSignatureListPager.currentPage = 1;
        if ($scope.NurseSignatureListPager) {
            $scope.NurseSignatureListPager.sortDirection = "DESC";
            $scope.NurseSignatureListPager.sortIndex = "EmployeeVisitID";
        }
        $scope.NurseSignatureListPager.getDataCallback();
    };

    $scope.Refresh = function () {

        $scope.NurseSignatureList = [];
        $scope.ApprovedEmployeeVisitIds = [];

        $scope.NurseSignatureListPager.getDataCallback();
    };

    $scope.Refresh();

    $(document).on("click", ".collapseSource", function () {

        var hasClassFaMinusCircle = $(this).hasClass("fa-minus-circle");
        if (hasClassFaMinusCircle == false) {
            $(this).removeClass("fa-plus-circle").addClass("fa-minus-circle");
        }
        else {
            $(this).removeClass("fa-minus-circle").addClass("fa-plus-circle");
        }

    });

    $scope.OnPage_GetApprovalVisitDetails = function (item, elem) {

        var hasClassFaMinusCircle = $(elem).hasClass("fa-minus-circle");
        if (hasClassFaMinusCircle == false) {
            $scope.GetApprovalVisitDetails(item);
        }

    };

    $scope.GetApprovalVisitDetails = function (item) {

        if (item.ApprovalVisitDetails == undefined) {
            item.ApprovalVisitDetails = {};
            item.ApprovalVisitDetails.EmployeeVisitID = item.EmployeeVisitID;
            item.ApprovalVisitDetails.PayorName = item.PayorName;
            item.ApprovalVisitDetails.AuthorizationCode = item.AuthorizationCode;
            item.ApprovalVisitDetails.ClockInTime = item.ClockInTime;
            item.ApprovalVisitDetails.ClockOutTime = item.ClockOutTime;
            item.ApprovalVisitDetails.EmployeeVisitNoteList = [];
            item.ApprovalVisitDetails.EmployeeVisitNoteList1 = [];
            item.ApprovalVisitDetails.EmployeeVisitNoteList2 = [];
            item.ApprovalVisitDetails.EmployeeVisitConclusionList = [];

            var pagermodel = {
                SearchEmployeeVisitNoteListPage: { EmployeeVisitID: item.EmployeeVisitID },
                pageSize: 999999,
                pageIndex: 1,
                sortIndex: "EmployeeVisitNoteID",
                sortDirection: "DESC"
            };
            var jsonData = angular.toJson(pagermodel);

            AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeVisitNoteList, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    item.ApprovalVisitDetails.EmployeeVisitNoteList = response.Data.Items;
                    angular.forEach(item.ApprovalVisitDetails.EmployeeVisitNoteList, function (item) {
                        item.TimeInMinutes = item.ServiceTime;
                    });
                    var list = item.ApprovalVisitDetails.EmployeeVisitNoteList.slice();
                    var middleIndex = Math.ceil(list.length / 2);

                    item.ApprovalVisitDetails.EmployeeVisitNoteList1 = list.splice(0, middleIndex);
                    item.ApprovalVisitDetails.EmployeeVisitNoteList2 = list.splice(-middleIndex);
                }
                ShowMessages(response);
            });

            var jsonData = angular.toJson({ EmployeeVisitID: item.EmployeeVisitID });
            AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeVisitConclusionList, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    item.ApprovalVisitDetails.EmployeeVisitConclusionList = response.Data;
                }
            });
        }

    };

    $scope.OpenApproverSignModel = function () {
        $('#ApproverSignatureModel').modal({
            backdrop: 'static',
            keyboard: false
        });
    };
    $scope.CloseApproverSignModel = function () {
        $('#ApproverSignatureModel').modal('hide');
        $scope.ClearSignatureImage();
    };


    $scope.signModel = {};

    $scope.SaveSignatureImage = function () {
        $scope.signModel = $scope.accept(); // default method for get signatureobject

        if ($scope.signModel.isEmpty == true || ($scope.signModel.isEmpty == false && $scope.signModel.dataUrl == undefined)) {
            ShowMessage("Please provide a signature first", "error");
            return;
        }
        else {
            $scope.NurseSignatureModel.Signature = $scope.signModel.dataUrl;
            $scope.CloseApproverSignModel();
            $scope.approveVisits();
        }
    };

    $scope.ClearSignatureImage = function () {
        $scope.clear(); // default method for cleare signature board
    };

    $scope.approveVisits = function () {
        var list = $scope.NurseSignatureList.filter(v => v.IsChecked).map(v => ({
            EmployeeVisitID: v.EmployeeVisitID,
            SignNote: v.ApproveNote
        }));
        var jsonData = angular.toJson({ List: list, Signature: $scope.NurseSignatureModel.Signature });
        AngularAjaxCall($http, HomeCareSiteUrl.NurseSignature, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.Refresh();
            }
        });
    };

    $scope.PrintTimeSheetReportHomecare = function (EmployeeVisitID) {
        var Domain = window.Domain;
        var ReportName = 'TimeSheet';
        var ReportURL1 = 'https://';
        //var ReportURL2 = ':51285';
        var ReportURL2 = '.myezcare.com';
        var ReportURL3 = "/Report/Template?ReportName=";
        var ReportURL4 = "&EmployeeVisitID=";
        var parameterValue1 = EmployeeVisitID;
        var ReportURL5 = "&TaskType=";
        var parameterValue2 = 'Task';
        var ReportURL6 = "&ConclusionType=";
        var parameterValue3 = 'Conclusion';



        var url = ReportURL1 + Domain + ReportURL2 + ReportURL3 + ReportName + ReportURL4 + parameterValue1 + ReportURL5 + parameterValue2 + ReportURL6 + parameterValue3;
        var width = screen.availWidth - 0;
        var height = screen.availHeight - 60;
        var left = 0;//(screen.availWidth - width) / 2;
        var top = 0;//(screen.availHeight - height) / 2;
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

    $scope.PrintTimeSheetReportDaycare = function (EmployeeVisitID) {
        var Domain = window.Domain;
        var ReportName = 'TimeSheet-Daycare';
        var ReportURL1 = 'https://';
        //var ReportURL2 = ':51285';
        var ReportURL2 = '.myezcare.com';
        var ReportURL3 = "/Report/Template?ReportName=";
        var ReportURL4 = "&EmployeeVisitID=";
        var parameterValue1 = EmployeeVisitID;
        var ReportURL5 = "&TaskType=";
        var parameterValue2 = 'Task';
        var ReportURL6 = "&ConclusionType=";
        var parameterValue3 = 'Conclusion';



        var url = ReportURL1 + Domain + ReportURL2 + ReportURL3 + ReportName + ReportURL4 + parameterValue1 + ReportURL5 + parameterValue2 + ReportURL6 + parameterValue3;
        var width = screen.availWidth - 10;
        var height = screen.availHeight - 60;
        var left = 0;//(screen.availWidth - width) / 2;
        var top = 0;//(screen.availHeight - height) / 2;
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
    $scope.PrintBulkTimeSheetReportHomecare = function () {
        var list = [];
        angular.forEach($scope.NurseSignatureList, function (item, key) {
            if (item.IsChecked) {
                list.push(item.EmployeeVisitID);
            }
        });
        $scope.ListOfIdsInCsv = list.toString();
        var Domain = window.Domain;
        var ReportName = 'TimeSheet-Bulk';
        var ReportURL1 = 'https://';
        //  var ReportURL2 = ':51285';
        var ReportURL2 = '.myezcare.com';
        var ReportURL3 = "/Report/Template?ReportName=";
        var ReportURL4 = "&EmployeeVisitID=";
        var parameterValue1 = $scope.ListOfIdsInCsv;
        var ReportURL5 = "&TaskType=";
        var parameterValue2 = 'Task';
        var ReportURL6 = "&ConclusionType=";
        var parameterValue3 = 'Conclusion';



        var url = ReportURL1 + Domain + ReportURL2 + ReportURL3 + ReportName + ReportURL4 + parameterValue1 + ReportURL5 + parameterValue2 + ReportURL6 + parameterValue3;
        var width = screen.availWidth - 0;
        var height = screen.availHeight - 60;
        var left = 0;//(screen.availWidth - width) / 2;
        var top = 0;//(screen.availHeight - height) / 2;
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
    $scope.MultiplePDFDownload = function () {
        var list = [];
        angular.forEach($scope.NurseSignatureList, function (item, key) {
            if (item.IsChecked) {
                list.push(item.EncryptedEmployeeVisitID);
            }
        });
        debugger;
        var jsonData = angular.toJson(list);
        AngularAjaxCall($http, HomeCareSiteUrl.GenerateMultiplePcaTimeSheetURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                window.location = '/hc/report/download' + '?fpath=' + response.Data.FilePath + '&fname=' + response.Data.FileName + '&d=true';
            }
            ShowMessages(response);
        });
    };

};

controllers.NurseSignatureController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {


});
