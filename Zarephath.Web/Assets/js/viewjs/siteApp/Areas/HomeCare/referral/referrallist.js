var vm;

controllers.ReferralListController = function ($scope, $http, $window, $timeout) {

    vm = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetReferralListModel").val());
    };
    ReferralBulkUpdateModel = null;
    $scope.SelectedReferralIds = [];
    $scope.ReferralList = [];
    $scope.SelectAllCheckbox = false;
    $scope.ReferralModel = $.parseJSON($("#hdnSetReferralListModel").val());
    $scope.SearchReferralModel = $scope.newInstance().SearchReferralListModel;
    $scope.SearchReferralModel.ReferralStatusID = "1";
    $scope.TempSearchReferralModel = $scope.newInstance().SearchReferralListModel;

    //window.PageSize = 50;
    $scope.ReferralListPager = new PagerModule("ClientName");

    $scope.SetPostData = function (fromIndex, model) {
        var pagermodel = {
            SearchReferralModel: $scope.SearchReferralModel,
            pageSize: $scope.ReferralListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ReferralListPager.sortIndex,
            sortDirection: $scope.ReferralListPager.sortDirection
        };
        if (model != undefined) {
            pagermodel.referralStatusModel = model;
        }
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchReferralModel = $.parseJSON(angular.toJson($scope.TempSearchReferralModel));

    };


    $scope.ResetSearchFilter = function () {
        $scope.SearchReferralModel = $scope.newInstance().SearchReferralListModel;
        $scope.SearchReferralModel.ReferralStatusID = "1";
        $scope.TempSearchReferralModel = $scope.newInstance().SearchReferralListModel;
        $scope.ResetDropdown();
        $scope.TempSearchReferralModel.IsDeleted = "0";
        $scope.ReferralListPager.currentPage = 1;
        $scope.TempSearchReferralModel.ServiceTypeID = null;
        $scope.ReferralListPager.getDataCallback();
    };

    $scope.ResetDropdown = function () {
        $scope.TempSearchReferralModel.NotifyCaseManagerID = "-1";
        //$scope.TempSearchReferralModel.ServiceID = "";
        $scope.TempSearchReferralModel.ChecklistID = "-1";
        $scope.TempSearchReferralModel.ClinicalReviewID = "-1";
        $scope.TempSearchReferralModel.IsSaveAsDraft = "-1";
        $scope.TempSearchReferralModel.ReferralStatusID = "1";
        $scope.TempSearchReferralModel.PayorID = "";
        $scope.SelectedGroups = "";

    };
    $scope.ResetDropdown();

    $scope.ReferralIds = [];
    $scope.GetReferralList = function (isSearchDataMappingRequire) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedReferralIds = [];
        $scope.SelectAllCheckbox = false;
        //Reset Selcted Checkbox items and Control
        var cmId = GetCookie('CM_ID');
        if (ValideElement(cmId)) {
            $scope.TempSearchReferralModel.CaseManagerID = cmId;
            $scope.SearchReferralModel.CaseManagerID = cmId;
            if (!$scope.$root.$$phase) {
                $scope.$apply();
            }
        }

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping


        $scope.AjaxStart = true;
        var jsonData = $scope.SetPostData($scope.ReferralListPager.currentPage);

        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralListURL, jsonData, "Post", "json", "application/json", true).success(function (response) {

            if (response.IsSuccess) {
                $scope.ReferralList = response.Data.Items;
                $scope.ReferralListPager.currentPageSize = response.Data.Items.length;
                $scope.ReferralListPager.totalRecords = response.Data.TotalItems;
                $scope.ShowCollpase();
            }
            $scope.GetReferralAuthorizationsDetails();
            $scope.GetReferralListGroupList();
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };

    $scope.GetReferralAuthorizationsDetails = function () {
        var referralIDs = $scope.ReferralList.map(r => r.ReferralID).join(',');

        var model = {
            referralIDs: referralIDs
        };
        jsonData = angular.toJson(model);
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralAuthorizationsDetails, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $.each($scope.ReferralList, function (index, referral) {
                    $scope.ReferralDetails[referral.ReferralID] = {
                        Authorizations: response.Data.filter(r => referral.ReferralID == r.ReferralID)
                    };
                    referral.IsAuthorizationDetailsCaptured = true;
                });
            }
            ShowMessages(response);
        });
    }

    $scope.ReferralDetails = [];
    $scope.GetReferralDetails = function (referral) {
        if (!referral.IsDetailsCaptured) {
            var model = {
                Referral: {
                    ReferralID: referral.ReferralID
                }
            };
            jsonData = angular.toJson(model);
            AngularAjaxCall($http, HomeCareSiteUrl.GetReferralDetailsURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                if (response.IsSuccess) {
                    $scope.ReferralDetails[referral.ReferralID] = response.Data;
                    referral.IsDetailsCaptured = true;
                }
                ShowMessages(response);
            });
        }
    }

    $scope.GetReferralListGroupList = function () {
        $scope.GroupList = {};
        AngularAjaxCall($http, "/hc/referral/GetReferralListGroupList", "", "Get", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.GroupList = response.Data;

            }
        });
    };


    $scope.ReferralCareGiverDetails = {};
    $scope.OpenCareGiverDetailsPopup = function (referralDetails) {
        $("#caregiverdetailsmodal").modal("show");
        $scope.ReferralCareGiverDetails = referralDetails.ReferralBasicDetails;
    }

    $scope.DeleteReferral = function (referral, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        //if (1 != 1) {
        //    bootboxDialog(function () {
        //    }, bootboxDialogType.Alert, window.Alert, window.ReferralDependencyExistMessage);
        //} else {
        bootboxDialog(function (result) {
            if (result) {
                if (referral == undefined)
                    $scope.SearchReferralModel.ListOfIdsInCsv = $scope.SelectedReferralIds.toString();
                else
                    $scope.SearchReferralModel.ListOfIdsInCsv = referral.ReferralID > 0 ? referral.ReferralID.toString() : $scope.SelectedReferralIds.toString();

                if (referral != undefined && referral.ReferralID > 0) {
                    if ($scope.ReferralListPager.currentPage != 1)
                        $scope.ReferralListPager.currentPage = $scope.ReferralList.length === 1 ? $scope.ReferralListPager.currentPage - 1 : $scope.ReferralListPager.currentPage;
                } else {

                    if ($scope.ReferralListPager.currentPage != 1 && $scope.SelectedReferralIds.length == $scope.ReferralListPager.currentPageSize)
                        $scope.ReferralListPager.currentPage = $scope.ReferralListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedReferralIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control
                var jsonData = $scope.SetPostData($scope.ReferralListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteReferralURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {

                        $scope.ReferralList = response.Data.Items;
                        $scope.ReferralListPager.currentPageSize = response.Data.Items.length;
                        $scope.ReferralListPager.totalRecords = response.Data.TotalItems;
                        $scope.ShowCollpase();
                        $scope.ResetFilter();
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage + ' ' + window.ReferralScheduleDelete, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
        //}


    };

    // $scope.EncryptedReferralID = "m6-gKuJrVRNLZL08U5CmHg2";


    $scope.Refresh = function () {
        $scope.ReferralListPager.getDataCallback();
    };

    $scope.SearchReferral = function () {
        $scope.ReferralListPager.currentPage = 1;
        $scope.ReferralListPager.getDataCallback(true);
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectReferral = function (referral) {
        if (referral.IsChecked)
            $scope.SelectedReferralIds.push(referral.ReferralID);
        else
            $scope.SelectedReferralIds.remove(referral.ReferralID);

        if ($scope.SelectedReferralIds.length == $scope.ReferralListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function (value) {
        $scope.SelectedReferralIds = [];
        angular.forEach($scope.ReferralList, function (item, key) {
            item.IsChecked = value;// event.target.checked;
            if (item.IsChecked)
                $scope.SelectedReferralIds.push(item.ReferralID);
        });

        return true;
    };

    $scope.ReferralListPager.getDataCallback = $scope.GetReferralList;
    $scope.ReferralListPager.getDataCallback();



    $scope.SendReceiptNotificationEmail = function (referral) {
        $scope.EncryptedReferralID = "iSNqtcWbe3gZEhtctmlPcA2"; //referral.$scope.EncryptedReferralID;
        var jsonData = angular.toJson({ EncryptedReferralID: referral.EncryptedReferralID });

        AngularAjaxCall($http, SiteUrl.SendReceiptNotificationEmailURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                referral.NotifyCaseManager = response.Data.NotifyCaseManager;
            }
        });
    };

    //$scope.SaveStatus = function (newStatus, referral) {
    //    return $scope.CoreSaveStatus(newStatus, referral);
    //    if (referral.LastReferralStatusID !== newStatus && newStatus === parseInt(window.Inactive)) {
    //        var result = confirm(window.SendNotificationToCM);
    //        return $scope.CoreSaveStatus(newStatus, referral, result);
    //    }
    //    else if (referral.LastReferralStatusID !== newStatus && newStatus === parseInt(window.ReferralAccepted)) {
    //        var result01 = confirm(window.SendNotificationToCMReferralAccepted);
    //        return $scope.CoreSaveStatus(newStatus, referral, result01);
    //    }
    //    else {
    //      return   $scope.CoreSaveStatus(newStatus, referral);
    //    }
    //};

    $scope.CoreSaveStatus = function (newStatus, referral, notifyCm) {
        var model = {
            ReferralID: referral.ReferralID,
            ReferralStatusID: newStatus,
            NotifyCmForInactiveStatus: notifyCm
        };

        return AngularAjaxCall($http, HomeCareSiteUrl.ReferralStatusUpdateURL, { referralStatusModel: model }, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                referral.ReferralStatusID = response.Data.ReferralStatusID;
                referral.Status = response.Data.Status;
            }
        });
    };

    $scope.AssigneeFilter = function (value) {
        return function (item) {
            if (item.IsDeleted == 0 || item.EmployeeID == value) {
                return item;
            }
        };
    };

    $scope.SaveBulkReferral = function (ReferralBulkUpdateModel, SelectedReferralIds) {
        //Reset Selcted Checkbox items and Control
        $scope.SelectedReferralIds = [];
        $scope.SelectAllCheckbox = false;

        var referralBulkUpdateModel = {
            BulkUpdateType: ReferralBulkUpdateModel.BulkUpdateType,
            ReferralIDs: SelectedReferralIds.toString(),
            AssignedValues: ReferralBulkUpdateModel.AssignedValues.toString()
        };
        var jsonData = angular.toJson(referralBulkUpdateModel);
        return AngularAjaxCall($http, HomeCareSiteUrl.UpdateAssigneeBulkURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.ShowCollpase();
                $scope.GetReferralList();
                $scope.ResetSearchFilter();
                ReferralBulkUpdateModel.BulkUpdateType = ""
                ReferralBulkUpdateModel.AssignedValues = ""
            }
        });
    };

    $scope.SaveAssignee = function (newAssignee, referral, listOfIdsInCsv) {
        if (referral == undefined)
            $scope.SearchReferralModel.ListOfIdsInCsv = $scope.SelectedReferralIds.toString();
        else
            $scope.SearchReferralModel.ListOfIdsInCsv = referral.ReferralID > 0 ? referral.ReferralID.toString() : $scope.SelectedReferralIds.toString();

        if (referral != undefined && referral.ReferralID > 0) {
            if ($scope.ReferralListPager.currentPage != 1)
                $scope.ReferralListPager.currentPage = $scope.ReferralList.length === 1 ? $scope.ReferralListPager.currentPage - 1 : $scope.ReferralListPager.currentPage;
        } else {

            if ($scope.ReferralListPager.currentPage != 1 && $scope.SelectedReferralIds.length == $scope.ReferralListPager.currentPageSize)
                $scope.ReferralListPager.currentPage = $scope.ReferralListPager.currentPage - 1;
        }

        //Reset Selcted Checkbox items and Control
        $scope.SelectedReferralIds = [];
        $scope.SelectAllCheckbox = false;
        //Reset Selcted Checkbox items and Control
        var model = {
            ReferralID: referral.ReferralID,
            AssigneeID: newAssignee
        };
        var jsonData = $scope.SetPostData($scope.ReferralListPager.currentPage, model);
        return AngularAjaxCall($http, HomeCareSiteUrl.UpdateAssigneeBulkURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.ReferralList = response.Data.Items;
                $scope.ReferralListPager.currentPageSize = response.Data.Items.length;
                $scope.ReferralListPager.totalRecords = response.Data.TotalItems;
                $scope.ShowCollpase();
            }

        });
    };

    $scope.SaveStatus = function (newStatus, referral) {
        if (referral == undefined)
            $scope.SearchReferralModel.ListOfIdsInCsv = $scope.SelectedReferralIds.toString();
        else
            $scope.SearchReferralModel.ListOfIdsInCsv = referral.ReferralID > 0 ? referral.ReferralID.toString() : $scope.SelectedReferralIds.toString();

        if (referral != undefined && referral.ReferralID > 0) {
            if ($scope.ReferralListPager.currentPage != 1)
                $scope.ReferralListPager.currentPage = $scope.ReferralList.length === 1 ? $scope.ReferralListPager.currentPage - 1 : $scope.ReferralListPager.currentPage;
        } else {

            if ($scope.ReferralListPager.currentPage != 1 && $scope.SelectedReferralIds.length == $scope.ReferralListPager.currentPageSize)
                $scope.ReferralListPager.currentPage = $scope.ReferralListPager.currentPage - 1;
        }

        //Reset Selcted Checkbox items and Control
        $scope.SelectedReferralIds = [];
        $scope.SelectAllCheckbox = false;
        //Reset Selcted Checkbox items and Control
        var model = {
            ReferralID: referral.ReferralID,
            ReferralStatusID: newStatus
        };
        var jsonData = $scope.SetPostData($scope.ReferralListPager.currentPage, model);
        return AngularAjaxCall($http, HomeCareSiteUrl.UpdateStatusURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.ReferralList = response.Data.Items;
                $scope.ReferralListPager.currentPageSize = response.Data.Items.length;
                $scope.ReferralListPager.totalRecords = response.Data.TotalItems;
                $scope.ShowCollpase();
            }

        });
    };

    $scope.OpenAlertMessage = function (event) {
        $(event.target).webuiPopover({ title: 'prior Authorization', content: 'Content', animation: 'pop' });
        $(event.target).click();
    }

    $scope.OpenNoAuthMessage = function (event) {
        var contentMsg = '<b class="text-danger">No prior authorization is found</b>';
        $(event.target).webuiPopover({ content: contentMsg, animation: 'pop' });
        $(event.target).click();
    }

    $scope.OpenNoActiveAuthMessage = function (event) {
        var contentMsg = '<b class="text-danger">All prior authorizations are expired</b>';
        $(event.target).webuiPopover({ content: contentMsg, animation: 'pop' });
        $(event.target).click();
    }

    var EXPIRE_DAYS = -7;

    $scope.OpenRecentFutureAuthMessage = function (event, referralAuthList) {
        var contentMsg = 'Future EndDate expired soon';
        if (referralAuthList && referralAuthList.length > 0) {
            contentMsg = '<ul class="noListType">';
            for (var i = 0; i < referralAuthList.length; i++) {
                var endDate = new Date(referralAuthList[i].EndDate);
                var currentDate = new Date();
                var diff = currentDate - endDate;
                var days = (24 * 3600 * 1000);
                var days = diff / (24 * 3600 * 1000);
                if (days <= 0 && days > EXPIRE_DAYS) {
                    days = 0 - days;
                    days = parseInt(days);
                    contentMsg = contentMsg + '<li>AuthCode ' + referralAuthList[i].AuthorizationCode + ': Expiring soon in ' + days + ' days</li>'
                }
            }
            contentMsg = contentMsg + '</ul>';
        }

        $(event.target).webuiPopover({ content: contentMsg, animation: 'pop' });
        $(event.target).click();
    }

    $scope.IsActivePayorAuthExist = function (referralAuthList) {
        var result = true;
        if (referralAuthList && referralAuthList.length > 0) {
            var result = false;
            for (var i = 0; i < referralAuthList.length; i++) {
                var endDate = new Date(referralAuthList[i].EndDate);
                var currentDate = new Date();
                var diff = currentDate - endDate;
                var days = (24 * 3600 * 1000);
                var days = diff / (24 * 3600 * 1000);
                if (days <= 0) {
                    result = true;
                    break;
                }
            }
        }
        return result;
    }

    $scope.IsRecentFutureEndDate = function (referralAuthList) {
        var result = false;
        if (referralAuthList && referralAuthList.length > 0) {
            for (var i = 0; i < referralAuthList.length; i++) {
                var endDate = new Date(referralAuthList[i].EndDate);
                var currentDate = new Date();
                var diff = currentDate - endDate;
                var days = (24 * 3600 * 1000);
                var days = diff / (24 * 3600 * 1000);
                if (days <= 0 && days > EXPIRE_DAYS) {
                    result = true;
                    break;
                }
            }
        }
        return result;
    }

    $scope.ShowCollpase = function () {
        setTimeout(function () {
            $.each($('.collapseDestination'), function (index, data) {
                $(this).on('show.bs.collapse', function () {
                    $(this).parents("tbody").find(".collapseSource").removeClass("fa-plus-circle").addClass("fa-minus-circle");
                });

                $(this).on('hidden.bs.collapse', function () {
                    $(this).parents("tbody").find(".collapseSource").removeClass("fa-minus-circle").addClass("fa-plus-circle");
                });

            });

        }, 100);
    };

    $scope.ShowCollpase();

    // Check endDate expired
    $scope.IsEndDatePastMonth = function (eDate) {
        var isPastMonth = false;
        if (eDate) {
            var endDate = new Date(eDate);
            var currentDate = new Date();
            var diff = currentDate - endDate;
            if (diff > 0) {
                var days = (24 * 3600 * 1000);
                var days = diff / (24 * 3600 * 1000);
                if (days >= 1) {
                    isPastMonth = true;
                }
            }
        }
        return isPastMonth;
    }

    $scope.IsEndDateAfterPastMonth = function (eDate) {
        var isPastMonthAfterData = true;
        if (eDate) {
            var endDate = new Date(eDate);
            var currentDate = new Date();
            var diff = currentDate - endDate;
            if (diff > 0) {
                var days = (24 * 3600 * 1000);
                var days = diff / (24 * 3600 * 1000);
                if (days > 30) {
                    isPastMonthAfterData = false;
                }
            }
        }
        return isPastMonthAfterData;
    }
    $scope.ReferralEditModel = function (EncryptedReferralID, title) {
        var EncryptedReferralID = EncryptedReferralID;
        $('#Referral_fixedAsidelDDLBindIFrame').attr('src', HomeCareSiteUrl.PartialAddReferralURL + EncryptedReferralID);
        $('#Referral_fixedAside').modal({ backdrop: 'static', keyboard: false });;
    }
    $scope.ReferralEditModelClosed = function () {
        $scope.Refresh();
        //$scope.ResetFilter();
        $('#Referral_fixedAside').modal('hide');
    }

    $scope.ShowReferralChartForm = function (referral, FormName, mode) {
        $scope.ReferralID = referral.ReferralID;
        $scope.OrganizationID = window.OrgID;
        var ReferralChartURL = HomeCareSiteUrl.OrbeonLoadHtmlFormURL + '?FormURL=' + '/ezcare/' + FormName + '/' + 'pdf' + "?form-version=" + '1'
            + "&orbeon-embeddable=true"
            + "&OrgPageID=" + "EmployeeVisitTaskForm"
            + "&IsEditMode=" + "false"
            + "&ReferralID=" + $scope.ReferralID
            + "&FormId=" + "10000057"
            + "&FormName=" + FormName// "Patient%20Chart"
            + "&OrganizationId=" + window.OrgID
            + "&UserId=" + window.LUserId
            + "&Version=" + "1";

        var width = screen.availWidth - 10;
        var height = screen.availHeight - 60;
        var left = 0;
        var top = 0;
        var params = 'width=' + width + ', height=' + height;
        params += ', top=' + top + ', left=' + left;
        var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,' + params;
        var pdfWindow = window.open('about:blank', 'null', winFeature);
        pdfWindow.document.write("<html><head><style> * { box-sizing: border-box; padding: 0; margin: 0; border: 0; }</style>"
            + "<title>" + "Patient Chart" + "</title></head><body>"
            + '<embed width="100%" height="100%" name="plugin" src="' + ReferralChartURL + '" '
            + 'type="application/pdf" internalinstanceid="21"></body></html>');
        pdfWindow.document.close();
    };

    $scope.ShowReferralIncidentReportForm = function (referral, mode) {
        $scope.ReferralID = referral.ReferralID;
        $scope.OrganizationID = window.OrgID;
        var IncidentReportForms = HomeCareSiteUrl.OrbeonLoadHtmlFormURL + '?FormURL=' + '/ezcare/U-Incident-Report' + '/' + 'new' + "?form-version=" + '2'
            + "&orbeon-embeddable=true"
            + "&OrgPageID=" + "EmployeeVisitTaskForm"
            + "&IsEditMode=" + "false"
            + "&ReferralID=" + $scope.ReferralID
            + "&EmployeeID=" + "0"
            + "&FormId=" + "10000057"
            + "&FormName=" + "U-Incident-Report"
            + "&OrganizationId=" + window.OrgID
            + "&UserId=" + window.LUserId
            + "&Version=" + "2";

        var width = screen.availWidth - 10;
        var height = screen.availHeight - 60;
        var left = 0;
        var top = 0;
        var params = 'width=' + width + ', height=' + height;
        params += ', top=' + top + ', left=' + left;
        var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,' + params;
        var pdfWindow = window.open('-', 'null', winFeature);
        pdfWindow.document.write("<html><head><style> * { box-sizing: border-box; padding: 0; margin: 0; border: 0;}</style>"
            + "<title>" + "Incident Report" + "</title></head><body>"
            + '<embed width="100%" height="100%" name="plugin" src="' + IncidentReportForms + '" '
            + 'type="application/pdf" internalinstanceid="21"></body></html>'
        );

        pdfWindow.document.close();
    };

    $scope.ShowReferralFaceSheetForm = function (referral, mode) {
        if (referral.GoogleFileId == null) {
            $scope.GoogleFileId = referral.GoogleFileId;
            $scope.ReferralDocumentID = referral.ReferralDocumentID;
            $scope.ReferralID = referral.ReferralID;
            $scope.OrganizationID = window.OrgID;
            var ReferralFaceSheetForm = HomeCareSiteUrl.OrbeonLoadHtmlFormURL + '?FormURL=' + '/ezcare/Client-FaceSheet' + '/' + 'new' + "?form-version=" + '1'
                + "&orbeon-embeddable=true"
                + "&OrgPageID=" + window.ReferralDocumentPageId
                + "&IsEditMode=" + "false"
                + "&ReferralID=" + $scope.ReferralID
                + "&FormName=" + "Client-FaceSheet"
                + "&OrganizationId=" + window.OrgID
                + "&UserId=" + window.LUserId

            var width = screen.availWidth - 10;
            var height = screen.availHeight - 60;
            var left = 0;
            var top = 0;
            var params = 'width=' + width + ', height=' + height;
            params += ', top=' + top + ', left=' + left;
            var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,' + params;
            var pdfWindow = window.open('about:blank', 'null', winFeature);
            pdfWindow.document.write("<html><head><style> * { box-sizing: border-box; padding: 0; margin: 0; border: 0; }</style>"
                + "<title>" + "FaceSheet Form" + "</title></head><body>"
                + '<embed width="100%" height="100%" name="plugin" src="' + ReferralFaceSheetForm + '" '
                + 'type="application/pdf" internalinstanceid="21"></body></html>');
            pdfWindow.document.close();
        }
        else {
            $scope.GoogleFileId = referral.GoogleFileId;
            $scope.ReferralID = referral.ReferralID;
            $scope.ReferralDocumentID = referral.ReferralDocumentID;
            $scope.OrganizationID = window.OrgID;
            var ReferralFaceSheetForm = HomeCareSiteUrl.OrbeonLoadHtmlFormURL + '?FormURL=' + '/ezcare/Client-FaceSheet' + '/' + 'edit' + '/' + $scope.GoogleFileId + "?form-version=" + '1'
                + "&orbeon-embeddable=true"
                + "&OrgPageID=" + window.ReferralDocumentPageId
                + "&IsEditMode=" + "true"
                + "&ReferralID=" + $scope.ReferralID
                + "&FormName=" + "Client-FaceSheet"
                + "&OrganizationId=" + window.OrgID
                + "&UserId=" + window.LUserId

            var width = screen.availWidth - 10;
            var height = screen.availHeight - 60;
            var left = 0;
            var top = 0;
            var params = 'width=' + width + ', height=' + height;
            params += ', top=' + top + ', left=' + left;
            var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,' + params;
            var pdfWindow = window.open('about:blank', 'null', winFeature);
            pdfWindow.document.write("<html><head><style> * { box-sizing: border-box; padding: 0; margin: 0; border: 0; }</style>"
                + "<title>" + "FaceSheet Form" + "</title></head><body>"
                + '<embed width="100%" height="100%" name="plugin" src="' + ReferralFaceSheetForm + '" '
                + 'type="application/pdf" internalinstanceid="21"></body></html>');
            pdfWindow.document.close();
        }

    };

    $scope.HideReferralChartForm = function () {
        $('#ShowReferralChartFormModal').modal('hide');
    };

    $scope.IncidentReportForms = {};
    $scope.ReferralFaceSheetForm = {};

    $scope.SaveIncidentReportOrbeonForm = function (data) {
        if (data != null) {
            var OrbeonID = data.split(':')[1].trim();
            $scope.form = {};
            $scope.form.OrbeonFormID = OrbeonID;
            $scope.form.ReferralID = $scope.ReferralID;
            $scope.form.ComplianceID = -3;
            var jsonData = angular.toJson($scope.form);
            if (OrbeonID != '') {
                AngularAjaxCall($http, HomeCareSiteUrl.SaveIncidentReportOrbeonForm, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        ShowMessage(response.Message, "Success");
                        //$scope.HideIncidentReportForm();
                        //$scope.GetMappedVisitTaskForms();
                        //$scope.VisitNoteForms.Form = {};
                    }
                });
            }
        }
    };

    $scope.HideIncidentReportForm = function () {
        $('#ShowIncidentReportFormsModal').modal('hide');
    };

    $scope.SaveReferralFaceSheetForm = function (data) {
        if (data != null) {
            var OrbeonID = data.split(':')[1].trim();
            var formName = 'Client-FaceSheet';
            $scope.form = {};
            $scope.form.FormName = formName;
            $scope.form.OrbeonFormID = OrbeonID;
            $scope.form.ReferralID = $scope.ReferralID;
            $scope.form.ComplianceID = -5;
            $scope.form.ReferralDocumentID = $scope.ReferralDocumentID;
            var jsonData = angular.toJson($scope.form);
            if (OrbeonID != '') {
                AngularAjaxCall($http, HomeCareSiteUrl.SaveReferralFaceSheetFormURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        ShowMessage(response.Message, "Success");

                    }
                });
            }
        }
    };

    $scope.EncryptedReferralID = {};
    $scope.OnAddNoteClick = function (EncryptedReferralID) {
        $scope.EncryptedReferralID = EncryptedReferralID;
        $scope.ReferralNoteDetail = null;
        $scope.IsEdit = false;
        $scope.CommonNoteID = null;
        $('#AddReferralNoteModal').modal('show');
        $scope.GetReferralRole();
        $scope.GetReferralCategory();
        $scope.GetNoteSentenceList();
    }

    $scope.GetReferralEmployee = function () {
        $scope.RoleID = ($scope.SelectedRoleID) ? $scope.SelectedRoleID.toString() : null;
        var jsonData = angular.toJson({ RoleID: $scope.RoleID, });
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralEmployeeURL, jsonData, "Post", "json", "application/json").success(function (response) {
            $scope.EmployeeList = response.Data;
        });
    };
    $scope.PermissionList = [];
    $scope.GetReferralRole = function () {
        AngularAjaxCall($http, HomeCareSiteUrl.RolePermissionsURL, "", "Get").success(function (response) {
            ShowMessages(response);
            $scope.PermissionList = response.RoleList;
        });
    };
    $scope.GetReferralCategory = function () {
        AngularAjaxCall($http, "/hc/referral/GetNoteCategory", "", "Get").success(function (response) {
            //ShowMessages(response);
            $scope.CategoryList = response.Data;
            $scope.selecteditem = "0";
        });
    };

    $scope.GetNoteSentenceList = [];
    $scope.GetNoteSentenceList = function () {
        var jsonData = angular.toJson({ Dmas99ID: $scope.Dmas99ID, EncryptedReferralID: $scope.EncryptedReferralID });
        AngularAjaxCall($http, SiteUrl.GetNoteSentenceList, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.NoteSentenceList = response.Data.Items;
            }
        });
    };
    $scope.SelectNoteSentence = function (NoteSentence, checked) {

        if ($scope.ReferralNoteDetail == null) {
            $scope.ReferralNoteDetail = "";
        }
        if (checked == true) {
            var replacevalue1 = $scope.ReferralNoteDetail + "\n" + NoteSentence;
            $scope.ReferralNoteDetail = replacevalue1.replace(/(^[ \t]*\n)/gm, '');
        }
        else {
            var replacevalue = $scope.ReferralNoteDetail.replace(NoteSentence, '').replace(/(^[ \t]*\n)/gm, '');
            $scope.ReferralNoteDetail = replacevalue;
        }

    };
    $scope.SearchNoteSentence = function () {
        $scope.NoteSentenceListPager.currentPage = 1;
    };
    $scope.SetPostData1 = function (fromIndex) {
        var pagermodel = {
            SearchNoteSentenceListPage: $scope.SearchReferralNoteSentence,
            pageSize: $scope.NoteSentenceListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.NoteSentenceListPager.sortIndex,
            sortDirection: $scope.NoteSentenceListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };
    //$scope.SearchModelMapping = function () {
    //    $scope.SearchReferralNoteSentence = $.parseJSON(angular.toJson($scope.TempSearchNoteSentenceListPage));
    //};
    $scope.SaveReferralNote = function () {
        var isValid = CheckErrors($("#frmNewReferralNote"));
        if (!$scope.SelectedEmployeesID) {
            $scope.SelectedEmployeesID = new Array();
            angular.forEach($scope.EmployeeList, function (item, i) {
                $scope.SelectedEmployeesID.push(item.EmployeeID);
            });
        }
        if (isValid) {
            $scope.EmployeeList.fore
            $scope.RoleID = ($scope.SelectedRoleID) ? $scope.SelectedRoleID.toString() : null;
            $scope.EmployeesID = ($scope.SelectedEmployeesID) ? $scope.SelectedEmployeesID.toString() : null;
            $scope.ReferralNote.EncryptedReferralID = $scope.EncryptedReferralID;
            $scope.ReferralNote.EncryptedEmployeeID = $scope.EncryptedEmployeeID;
            $scope.ReferralNote.NoteDetail = $scope.ReferralNoteDetail;

            var jsonData = angular.toJson({
                RoleID: $scope.RoleID,
                catId: $scope.selecteditem,
                EmployeesID: $scope.EmployeesID,
                EncryptedReferralID: $scope.EncryptedReferralID,
                EncryptedEmployeeID: $scope.EncryptedEmployeeID,
                NoteDetail: $scope.ReferralNoteDetail,
                IsEdit: $scope.IsEdit,
                CommonNoteID: $scope.CommonNoteID
            });
            AngularAjaxCall($http, HomeCareSiteUrl.SaveReferralNoteURL, jsonData, "Post", "json", "application/json").success(function (response) {
                $scope.IsEdit = false;
                ShowMessages(response);
                $("#NoteReq").hide();
                if (response.IsSuccess) {
                    $("#AddReferralNoteModal").modal('hide');
                    $scope.ResetRNote();
                }
            });
        }
        else {
            $("#NoteReq").show();
        }
    };
    $scope.ReferralNote = [];
    $scope.ReferralNoteList = [];
    $scope.ResetRNote = function () {
        $scope.ReferralNoteDetail = '';
        $scope.SelectedRoleID = "";
        $scope.SelectedEmployeesID = "";
        $scope.ReferralNote.SelectedRoleID = "";
        $scope.EmployeesID = "";
        $scope.GetReferralCategory();
        $scope.GetReferralRole();
        $scope.GetReferralEmployee();

    }

    $scope.SendBulkEmail = function (referral, title) {
        $('#SendBulkEmailModel').modal({
            backdrop: 'static',
            keyboard: false
        });

        $scope.ListOfIdsInCsv = $scope.SelectedReferralIds.toString();
        scopeEmpRefEmail.GetReferralEmail($scope.ListOfIdsInCsv);
        $scope.SelectedReferralIds = [];
        $scope.SelectAllCheckbox = false;
        //Reset Selcted Checkbox items and Control
        //var jsonData = $scope.SetPostData($scope.ListOfIdsInCsv);
        //        AngularAjaxCall($http, HomeCareSiteUrl.DeleteReferralURL, jsonData, "Post", "json", "application/json").success(function (response) {
        //            if (response.IsSuccess) {

        //                $scope.ReferralList = response.Data.Items;
        //            }
        //            ShowMessages(response);
        //        });



    }
    $scope.IsPayor = true;

    $scope.ReferralPayor = function () {
        // $scope.ResetSearchFilter();
        //$('#filter').removeClass('active');
        //$('#tabfilter').removeClass('active'); // this deactivates the filter tab
        //$('#census').addClass('active');
        //$('#tabcensus').addClass('active'); // this activates the census tab

        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralPayorsUrl, null, "Get", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.GetPayor = response.Data.Items;
                $scope.GetPayor.totalRecords = response.Data.TotalItems;
                $scope.IsStatus = false;
                $scope.IsCareType = false;
                $scope.IsPayor = true;
                if (response.Data.TotalItems > 0) {
                    $scope.TotalPayor = $scope.GetPayor[0].TotalPayor;
                }
                else {
                    $scope.TotalPayor = 0;
                }
            }
            else {
                $scope.GetPayor = 0;
            }

        });
    };
    $scope.ReferralPayor();
    $scope.ReferralStatus = function () {
        // $scope.ResetSearchFilter();
        //$('#filter').removeClass('active');
        //$('#tabfilter').removeClass('active'); // this deactivates the filter tab
        //$('#census').addClass('active');
        //$('#tabcensus').addClass('active'); // this activates the census tab
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralStatusUrl, null, "Get", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.GetStatus = response.Data.Items;
                $scope.GetStatusID = response.Data.Items.ReferralStatusID;
                $scope.GetStatus.totalRecords = response.Data.TotalItems;
                $scope.IsPayor = false;
                $scope.IsCareType = false;
                $scope.IsStatus = true;
                if (response.Data.TotalItems > 0) {
                    $scope.TotalStatus = $scope.GetStatus[0].TotalStatus;
                }
                else {
                    $scope.TotalStatus = 0;
                }
            }
            else {
                $scope.GetStatus = 0;
            }
        });
    };

    $scope.ReferralCareType = function () {
        // $scope.ResetSearchFilter();
        //$('#filter').removeClass('active');
        //$('#tabfilter').removeClass('active'); // this deactivates the filter tab
        //$('#census').addClass('active');
        //$('#tabcensus').addClass('active'); // this activates the census tab
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralCareTypeUrl, null, "Get", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.GetCareType = response.Data.Items;
                $scope.GetCareType.totalRecords = response.Data.TotalItems;
                $scope.IsPayor = false;
                $scope.IsStatus = false;
                $scope.IsCareType = true;
                if (response.Data.TotalItems > 0) {
                    $scope.TotalCareType = $scope.GetCareType[0].TotalCareType;
                }
                else {
                    $scope.TotalCareType = 0;
                }
            }
            else {
                $scope.GetCareType = 0;
            }

        });
    };

    $scope.ResetFilter = function () {
        $scope.ResetSearchFilter();
        $('#census').removeClass('active');
        $('#tabcensus').removeClass('active'); // this deactivates the census tab
        $('#filter').addClass('active');
        $('#tabfilter').addClass('active'); // this activates the filter tab
    };
    $scope.ShowVitalForm = function (referral, mode) {
        if (referral.GoogleFileId == null) {
            $scope.GoogleFileId = referral.GoogleFileId;
            $scope.ReferralDocumentID = referral.ReferralDocumentID;
            $scope.ReferralID = referral.ReferralID;
            $scope.OrganizationID = window.OrgID;
            var ReferralVSTrackingForm = HomeCareSiteUrl.OrbeonLoadHtmlFormURL + '?FormURL=' + '/ezcare/VS-Tracking' + '/' + 'new' + "?form-version=" + '1'
                + "&orbeon-embeddable=true"
                + "&OrgPageID=" + window.ReferralDocumentPageId
                + "&IsEditMode=" + "false"
                + "&ReferralID=" + $scope.ReferralID
                + "&FormName=" + "VS-Tracking"
                + "&OrganizationId=" + window.OrgID
                + "&UserId=" + window.LUserId

            var width = screen.availWidth - 10;
            var height = screen.availHeight - 60;
            var left = 0;
            var top = 0;
            var params = 'width=' + width + ', height=' + height;
            params += ', top=' + top + ', left=' + left;
            var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,' + params;
            var pdfWindow = window.open('about:blank', 'null', winFeature);
            pdfWindow.document.write("<html><head><style> * { box-sizing: border-box; padding: 0; margin: 0; border: 0; }</style>"
                + "<title>" + "Vital Sign Tracking" + "</title></head><body>"
                + '<embed width="100%" height="100%" name="plugin" src="' + ReferralVSTrackingForm + '" '
                + 'type="application/pdf" internalinstanceid="21"></body></html>');
            pdfWindow.document.close();
        }
        else {
            $scope.GoogleFileId = referral.GoogleFileId;
            $scope.ReferralID = referral.ReferralID;
            $scope.ReferralDocumentID = referral.ReferralDocumentID;
            $scope.OrganizationID = window.OrgID;
            var ReferralVSTrackingForm = HomeCareSiteUrl.OrbeonLoadHtmlFormURL + '?FormURL=' + '/ezcare/VS-Tracking' + '/' + 'edit' + '/' + $scope.GoogleFileId + "?form-version=" + '1'
                + "&orbeon-embeddable=true"
                + "&OrgPageID=" + window.ReferralDocumentPageId
                + "&IsEditMode=" + "true"
                + "&ReferralID=" + $scope.ReferralID
                + "&FormName=" + "VS-Tracking"
                + "&OrganizationId=" + window.OrgID
                + "&UserId=" + window.LUserId

            var width = screen.availWidth - 10;
            var height = screen.availHeight - 60;
            var left = 0;
            var top = 0;
            var params = 'width=' + width + ', height=' + height;
            params += ', top=' + top + ', left=' + left;
            var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,' + params;
            var pdfWindow = window.open('about:blank', 'null', winFeature);
            pdfWindow.document.write("<html><head><style> * { box-sizing: border-box; padding: 0; margin: 0; border: 0; }</style>"
                + "<title>" + "Vital Sign Tracking" + "</title></head><body>"
                + '<embed width="100%" height="100%" name="plugin" src="' + ReferralVSTrackingForm + '" '
                + 'type="application/pdf" internalinstanceid="21"></body></html>');
            pdfWindow.document.close();
        }

    };
    $scope.SaveReferralVitalForm = function (data) {
        if (data != null) {
            var OrbeonID = data.split(':')[1].trim();
            var formName = 'VS-Tracking';
            $scope.form = {};
            $scope.form.FormName = formName;
            $scope.form.OrbeonFormID = OrbeonID;
            $scope.form.ReferralID = $scope.ReferralID;
            $scope.form.ComplianceID = -7;
            $scope.form.ReferralDocumentID = $scope.ReferralDocumentID;
            var jsonData = angular.toJson($scope.form);
            if (OrbeonID != '') {
                AngularAjaxCall($http, HomeCareSiteUrl.SaveVitalFormURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        ShowMessage(response.Message, "Success");

                    }
                });
            }
        }
    };

};
controllers.ReferralListController.$inject = ['$scope', '$http', '$window', '$timeout'];


$(document).ready(function () {
    var eventMethod = window.addEventListener ? "addEventListener" : "attachEvent";
    var eventer = window[eventMethod];
    var messageEvent = eventMethod == "attachEvent" ? "onmessage" : "message";
    // Listen to message from child window
    eventer(messageEvent, function (e) {
        if (typeof (e.data) === 'string' && e.data.indexOf('OrbeonID') >= 0) {
            vm.SaveIncidentReportOrbeonForm(e.data);
        }

    }, false);

    var eventMethod1 = window.addEventListener ? "addEventListener" : "attachEvent";
    var eventer1 = window[eventMethod1];
    var messageEvent1 = eventMethod1 == "attachEvent" ? "onmessage" : "message";
    // Listen to message from child window
    eventer1(messageEvent1, function (e) {
        if (typeof (e.data) === 'string' && e.data.indexOf('OrbeonID') >= 0) {
            vm.SaveReferralFaceSheetForm(e.data);
        }

    }, false);
    var eventMethod2 = window.addEventListener ? "addEventListener" : "attachEvent";
    var eventer2 = window[eventMethod2];
    var messageEvent2 = eventMethod1 == "attachEvent" ? "onmessage" : "message";
    // Listen to message from child window
    eventer2(messageEvent2, function (e) {
        if (typeof (e.data) === 'string' && e.data.indexOf('OrbeonID') >= 0) {
            vm.SaveReferralVitalForm(e.data);
        }

    }, false);

    var cmId = GetCookie('CM_ID');
    if (ValideElement(cmId))
        vm.TempSearchReferralModel.CaseManagerID = cmId;

});

$("#right-scroll-button").click(function () {
    event.preventDefault();
    $(".employeetable").animate(
        {
            scrollLeft: "+=300px"
        },
        "slow"
    );
});

$("#left-scroll-button").click(function () {
    event.preventDefault();
    $(".employeetable").animate(
        {
            scrollLeft: "-=300px"
        },
        "slow"
    );
});

