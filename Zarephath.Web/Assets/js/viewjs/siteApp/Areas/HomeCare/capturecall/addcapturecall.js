var custModel;

controllers.AddCaptureCallController = function ($scope, $http, $timeout, $window) {
    custModel = $scope;
    $scope.form = {};
    $scope.CaptureCallModel = $.parseJSON($("#hdnCaptureCallModel").val());
    $scope.SelectedGroups = [];
    $scope.SetGroups = function () {
        if ($scope.CaptureCallModel.CaptureCall.GroupIDs != null) {
            $scope.CaptureCallModel.CaptureCall.GroupIDs = $scope.CaptureCallModel.CaptureCall.GroupIDs.split(",");
        }
    };
    $scope.SetGroups();
    $scope.SaveCaptureCall = function () {
        debugger
        var isValid = CheckErrors($("#frmAddCaptureCall"));
        $scope.CaptureCallModel.CaptureCall.OrbeonID = $scope.form.OrbeonFormID;
        if (!$scope.CaptureCallModel.CaptureCall.EmployeesIDs) {
            $scope.CaptureCallModel.CaptureCall.EmployeesIDs = new Array();
            angular.forEach($scope.EmployeeList, function (item, i) {
                $scope.CaptureCallModel.CaptureCall.EmployeesIDs.push(item.EmployeeID);
            });
        }
        if (isValid) {
            $scope.RelatedWithPatient = ($scope.CaptureCallModel.CaptureCall.RelatedWithPatient) ? $scope.CaptureCallModel.CaptureCall.RelatedWithPatient.toString() : null;
            $scope.RoleIds = ($scope.CaptureCallModel.CaptureCall.RoleIds) ? $scope.CaptureCallModel.CaptureCall.RoleIds.toString() : null;
            $scope.EmployeesIDs = ($scope.CaptureCallModel.CaptureCall.EmployeesIDs) ? $scope.CaptureCallModel.CaptureCall.EmployeesIDs.toString() : null;
            $scope.CaptureCallModel.CaptureCall.GroupIDs = ($scope.CaptureCallModel.CaptureCall.GroupIDs) ? $scope.CaptureCallModel.CaptureCall.GroupIDs.toString() : null;
            var jsonData = angular.toJson({ CaptureCall: $scope.CaptureCallModel.CaptureCall, RoleIds: $scope.RoleIds, EmployeesIDs: $scope.EmployeesIDs, RelatedWithPatient: $scope.RelatedWithPatient });
            console.log(jsonData)
            AngularAjaxCall($http, HomeCareSiteUrl.AddCaptureCall, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        toastr.success("CaptureCall Save Successfully");
                        $scope.CaptureCallModel.CaptureCall = null;
                        //window.location.href = "/hc/capturecall/CaptureCallList/";
                    } else {
                        //toastr.success("CaptureCall Save Successfully");
                        ShowMessage(response.Message,'error');
                    }
                });
        }
    };
    $scope.Cancel = function () {
        //$('#AddCallCaptureModel').modal('close');
        //$('#AddCallCaptureModel1').modal('hide');
        window.location.reload();
    }
    $scope.PermissionList = [];
    $scope.GetReferralRole = function () {
        AngularAjaxCall($http, HomeCareSiteUrl.RolePermissionsURL, "", "Get").success(function (response) {
            ShowMessages(response);
            $scope.PermissionList = response.RoleList;
        });
    };
    $scope.GetReferralRole();
    $scope.GetReferralEmployee = function () {
        $scope.RoleID = ($scope.CaptureCallModel.CaptureCall.RoleIds) ? $scope.CaptureCallModel.CaptureCall.RoleIds.toString() : null;
        var jsonData = angular.toJson({ RoleID: $scope.RoleID, });
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralEmployeeURL, jsonData, "Post", "json", "application/json").success(function (response) {
            $scope.EmployeeList = response.Data;
            console.log($scope.EmployeeList);
        });
    };
    $scope.GetReferralEmployee();

    $scope.UploadForm = function () {
        $('#addFormModal').modal('show');
    };

    $scope.ShowAF2Form = function () {
        $scope.EmployeeID = window.LUserId;
        $scope.ReferralID = 0;
        $scope.OrganizationID = window.OrgID;
        var AF2Forms = HomeCareSiteUrl.OrbeonLoadHtmlFormURL + '?FormURL=' + '/ezcare/IN-OPE-AF-02' + '/' + 'new' + "?form-version=" + '1'
            + "&orbeon-embeddable=true"
            + "&OrgPageID=" + "EmployeeVisitTaskForm"
            + "&IsEditMode=" + "false"
            + "&ReferralID=" + $scope.ReferralID
            + "&EmployeeID=" + "0"
            + "&FormId=" + "10000057"
            + "&FormName=" + "IN-OPE-AF-02"
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
        var pdfWindow = window.open('-', 'null', winFeature);
        pdfWindow.document.write("<html><head><style> * { box-sizing: border-box; padding: 0; margin: 0; border: 0;}</style>"
            + "<title>" + "AF2 Form" + "</title></head><body>"
            + '<embed width="100%" height="100%" name="plugin" src="' + AF2Forms + '" '
            + 'type="application/pdf" internalinstanceid="21"></body></html>'
        );

        pdfWindow.document.close();
    };

    $scope.OpenMapFormModel = function (IsBindToFrame) {
        $scope.IsBindToFrame = IsBindToFrame;
        AngularAjaxCall($http, HomeCareSiteUrl.GetOrganizationFormListURL, null, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.OrganizationFormList = response.Data;
            }
        });
        //$('#mapFormModal').modal({
        //    backdrop: 'static',
        //    keyboard: false
        //});
        $('#mapFormModal').modal('show');
    };

    $scope.SelectForm = function (item) {
        $scope.AddModal.EBFormID = item.EBFormID;
        $scope.AddModal.FormName = item.FormLongName;
        $scope.AddModal.NameForUrl = item.NameForUrl;
        $scope.AddModal.Version = item.Version;
        $('#mapFormModal').modal('hide');
        $scope.frmSearch = null;
    };

    $scope.SelectForm1 = function (item) {
        bootboxDialog(function (result) {
            if (result) {
                $scope.MapPermanently = true;
                $scope.MapForm(item);
            } else {
                $scope.MapPermanently = false;
                $scope.MapForm(item);
            }
        }, bootboxDialogType.Confirm, "Map Form", "Would you like to preserve this preference for future use?", bootboxDialogButtonText.Yes, btnClass.BtnDanger, bootboxDialogButtonText.No);
    };

    $scope.GetFormList = function (data, id) {
        $scope.SelectedSubSectionID = id;
        $scope.SelectedSubSection = data;
        $scope.ShowFrame = false;
        //Remove BG Color from all other div
        $("[id^=colorSub]").removeClass("SetColor");
        $("[id^=colorSub]").css({ "background-color": "rgba(249, 249, 249, 0.9)" });
        //Set BG Color to slected div
        if (id != undefined) {
            $('#' + id).addClass("SetColor");
            document.getElementById(id).style.backgroundColor = 'rgba(' + $scope.SelectedSection.ColorRGBA + ',0.2)';
        }
        $scope.ParentID = $scope.SelectedSection.ComplianceID;
        $scope.ComplianceID = data.ComplianceID;
        $scope.IsTimeBased = data.IsTimeBased;
        $scope.GetReferralDocumentList();
    };


    $scope.SaveDocumentFormName = function (id) {
        $scope.EbriggsFormMppingID = id;
        if ($scope.ShowFormNameModal) {
            $('#FormNameModal').modal({
                backdrop: 'static',
                keyboard: false
            });
        } else {
            $scope.ShowFormNameModal = true;
            //if ($scope.EbriggsFormMppingID > 0) {
            var jsonData1 = angular.toJson({
                "EbriggsFormMppingID": $scope.EbriggsFormMppingID,
                "FormName": $scope.Form == undefined ? null : $scope.Form.FormName,
                "UpdateFormName": false
            });
            $scope.EbriggsFormMppingID = undefined;
            AngularAjaxCall($http, HomeCareSiteUrl.SaveDocumentFormNameURL, jsonData1, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.ClosePreview();
                    $scope.GetReferralDocumentList();
                }
                ShowMessages(response);
            });
            //}
        }
    };

    $scope.SavedNewHtmlFormWithSubsection = function (resData) {

        $scope.ResData = resData;
        if ($scope.ShowFormNameModal) {
            $('#FormNameModal').modal({
                backdrop: 'static',
                keyboard: false
            });
        } else {
            var resObj = $.parseJSON($scope.ResData);
            var originalEBFormID = resObj.OrginalFormId;
            var eBriggsFormID = resObj.EBriggsFormID;
            var formId = resObj.FormId;
            var jsonData = angular.toJson({
                "EBriggsFormID": eBriggsFormID,
                "OriginalEBFormID": originalEBFormID,
                "FormId": formId,
                "SubSectionID": $scope.ComplianceID,
                "ReferralID": $scope.ReferralID,
                "EmployeeID": $scope.EmployeeID,
                "UserType": $scope.UserType,
                "FormName": $scope.Form == undefined ? null : $scope.Form.FormName,
                "UpdateFormName": false
            });
            $scope.ResData = undefined;
            AngularAjaxCall($http, HomeCareSiteUrl.SavedNewHtmlFormWithSubsectionURL, jsonData, "Post", "json", "application/json", false).
                success(function (response) {
                    $scope.ClearFrame();
                    //$scope.GetFormList($scope.SelectedSubSection, $scope.SelectedSectionID);
                    $scope.GetReferralDocumentList();
                    ShowMessages(response);
                    $("html, body").animate({ scrollTop: 0 }, "slow");
                    $scope.Form = {};
                });
            $scope.ShowFrame = false;
            $scope.ShowFormNameModal = true;
        }
    };

    $scope.ReceiverOrbeonForm = function (data) {
        var DocumentEdit = $scope.Document.Edit == undefined ? false : $scope.Document.Edit;
        var DocumentClone = $scope.Document.Clone == undefined ? false : $scope.Document.Clone;
        var EmployeeID = $scope.EmployeeID == undefined ? 0 : $scope.EmployeeID;
        var ReferralID = $scope.ReferralID == undefined ? 0 : $scope.ReferralID;
        var ReferralDocumentID = $scope.Document.ReferralDocumentID == undefined || DocumentClone ? 0 : $scope.Document.ReferralDocumentID;

        var ComplianceID = ($scope.SelectedSubSection == undefined || $scope.SelectedSubSection == null) ? $scope.SelectedSection.ComplianceID : $scope.SelectedSubSection.ComplianceID;

        var OrbeonID = data.split(':')[1];

        var jsonData = angular.toJson({
            EmployeeID: EmployeeID,
            ReferralID: ReferralID,
            ComplianceID: ComplianceID,
            DocumentID: OrbeonID,
            ReferralDocumentID: ReferralDocumentID,
            DocumentEdit: DocumentEdit,
            DocumentClone: DocumentClone
        });

        if (OrbeonID != '') {
            AngularAjaxCall($http, HomeCareSiteUrl.SaveOrbeonDocumentURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    ShowMessages(response);
                    $scope.ClosePreview();
                    $scope.GetReferralDocumentList();
                    $scope.Form = {};
                    $scope.Document = {};
                    var referralDocument = response.Data;
                    referralDocument.Name = referralDocument.FileName;
                    $scope.EditDocument(referralDocument)
                } else {
                    ShowMessages(response);
                }

            });
        }
    };

    $scope.SetPreview = function (Section, SubSection) {
        $scope.Form = {};
        $scope.Document = {};
        var item = (SubSection == null || SubSection == undefined) ? Section : SubSection;
        $scope.Form.FormName = item.FormName + '_' + $scope.CurrentDate;

        if (item.Version == null && item.NameForUrl == null) {
            $scope.OpenMapFormModel(true);
            $('#addFormModal').modal('hide');

        } else {
            $scope.frameLoader = true;
            $scope.ShowFrame = true;
            // if (item.IsInternalForm || item.IsOrbeonForm) {
            var EmployeeID = $scope.EmployeeID == undefined ? 0 : $scope.EmployeeID;
            var ReferralID = $scope.ReferralID == undefined ? 0 : $scope.ReferralID;

            var path = HomeCareSiteUrl.LoadHtmlFormURL;

            if (item.InternalFormPath.indexOf('.pdf') !== -1) {
                path = window.MyezcarePdfFormsUrl;
            }

            if (!ValideElement(item.EBriggsFormID)) { item.EBriggsFormID = "0" };
            var formName = $scope.Form == undefined ? null : $scope.Form.FormName;

            if (item.IsOrbeonForm) {
                $scope.Document.Edit = false;
                $scope.Document.Clone = false;

                var urlToGet = HomeCareSiteUrl.OrbeonLoadHtmlFormURL + '?FormURL=' + encodeURIComponent(item.NameForUrl);

                var newURL = urlToGet + '/new' + "?form-version=" + item.Version
                    + "&orbeon-embeddable=true"
                    + "&OrgPageID=" + window.ReferralDocumentPageId
                    + "&IsEditMode=" + "true"
                    + "&EmployeeID=" + EmployeeID
                    + "&ReferralID=" + ReferralID
                    + "&OriginalEBFormID=" + item.EBFormID
                    + "&FormId=" + item.FormId
                    + "&UserType=" + $scope.UserType
                    + "&SubSectionID=" + $scope.ComplianceID
                    + "&FormName=" + formName
                    + "&OrganizationId=" + window.OrgID
                    + "&UserId=" + window.LoggedInUserId
                    + "&Version=" + item.Version;

                document.getElementById('preview_window').src = newURL;
                $('#addFormModal').modal('hide');
                //}
                //else {

                //    var newURL = path 
                //        + "?FormURL=" + encodeURIComponent(item.InternalFormPath) + "?form-version=" + item.Version
                //        + "&OrgPageID=" + window.ReferralDocumentPageId
                //        + "&IsEditMode=" + "true"
                //        + "&EmployeeID=" + EmployeeID
                //        + "&ReferralID=" + ReferralID
                //        + "&EBriggsFormID=" + item.EBriggsFormID
                //        + "&OriginalEBFormID=" + item.EBFormID
                //        + "&FormId=" + item.FormId
                //        + "&UserType=" + $scope.UserType
                //        + "&SubSectionID=" + $scope.ComplianceID
                //        + "&FormName=" + formName
                //        + "&UpdateFormName=0"
                //        + "&OrganizationId=" + window.OrgID
                //        + "&UserId=" + window.LUserId
                //        + "&EbriggsFormMppingID=" + "0";

                //    document.getElementById('myIframe').src = newURL;
                //}
            }
            else {
                var data = $scope.ConfigEBFormModel;
                var newFormUrl = data.EBBaseSiteUrl + "/form/" + item.NameForUrl + "?version=" + item.Version + "&PageId=" + window.ReferralDocumentPageId;//+ "&tenantGuid=" + response.tenantGuid;
                var newURL = data.MyezcareFormsUrl + "?formURL=" + encodeURIComponent(newFormUrl);
                document.getElementById('myIframe').src = newURL;
            }
            $('#addFormModal').modal('hide');
        }
    };

    $scope.ClearFrame = function () {
        var _frame = document.getElementById('myIframe');
        var wn = _frame.contentWindow;
        if (wn && _frame.src.indexOf(HomeCareSiteUrl.OrbeonLoadHtmlFormURL) >= 0) {
            // postMessage arguments: data to send, target origin
            wn.postMessage('ClearFrame:true', '*');
        } else {
            _frame.src = "";
        }
        $scope.frameLoader = false;
    };

    $scope.MapForm = function (item) {
        //var jsonData = angular.toJson({
        //    ComplianceID: $scope.ComplianceID,
        //    EBFormID: item.EBFormID,
        //    UserType: $scope.UserType,
        //    MapPermanently: $scope.MapPermanently,
        //    //SectionID: $scope.SelectedSection.ComplianceID
        //    SectionID: $scope.ComplianceID
        //});
        $scope.ReferralID = 0;
        $scope.EmployeeID = window.LUserId;
        $scope.OrganizationID = window.OrgID;
        var Forms = HomeCareSiteUrl.OrbeonLoadHtmlFormURL + '?FormURL=' + item.NameForUrl + '/' + 'new' + "?form-version=" + '1'
            + "&orbeon-embeddable=true"
            + "&OrgPageID=" + "EmployeeVisitTaskForm"
            + "&IsEditMode=" + "false"
            + "&ReferralID=" + $scope.ReferralID
            + "&EmployeeID=" + $scope.EmployeeID
            + "&FormId=" + item.FormId
            + "&FormName=" + item.NameForUrl
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
        var pdfWindow = window.open('-', 'null', winFeature);
        pdfWindow.document.write("<html><head><style> * { box-sizing: border-box; padding: 0; margin: 0; border: 0;}</style>"
            + "<title>" + "Forms" + "</title></head><body>"
            + '<embed width="100%" height="100%" name="plugin" src="' + Forms + '" '
            + 'type="application/pdf" internalinstanceid="21"></body></html>'
        );

        pdfWindow.document.close();
    };

    $scope.SaveOrbeonForm = function (data) {
        if (data != null) {
            var OrbeonID = data.split(':')[1].trim();
            $scope.form.OrbeonFormID = OrbeonID;
        }
        $('#addFormModal').modal('hide');
        $('#mapFormModal').modal('hide');
    };
    $scope.TempData = {};
    $scope.ConvertToReferral = function (item, title) {
        $scope.LoggedInID = window.LUserId;
        if (item.Status != null) {
            if (item.Status == "Waitlisted") { angular.element('#Waitlist').addClass("waitlist"); angular.element('#Complete').removeClass("waitlist"); }
            if (item.Status == "Converted") { angular.element('#Complete').addClass("waitlist"); angular.element('#Waitlist').addClass("waitlist"); }
            if (item.Status !== "Waitlisted" && item.Status !== "Converted") { angular.element('#New').addClass("new"); angular.element('#Complete').removeClass("waitlist"); angular.element('#Waitlist').removeClass("waitlist"); }
        }
        if (item.Id != 0) {
            if (title == undefined) {
                title = "Convert to Referrals"; //window.UpdateRecords;
            }
            if (item.Status == "Converted") {
                bootboxDialog(function (result) {
                    if (result) {
                        item.GroupIDs = ($scope.CaptureCallModel.CaptureCall.GroupIDs) ? $scope.CaptureCallModel.CaptureCall.GroupIDs.toString() : null;
                        var jsonData = angular.toJson(item);
                        AngularAjaxCall($http, HomeCareSiteUrl.ConvertToReferralURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
                            ShowMessages(response);
                            if (response.IsSuccess) {
                                $scope.EncryptedId = response.Data;
                                //angular.element('#Complete').addClass("waitlist");
                                $('#ConvertReferral_fixedAside').modal({ backdrop: 'static', keyboard: false });;
                                $('#ConvertReferral_fixedAsidelDDLBindIFrame').attr('src', HomeCareSiteUrl.PartialAddReferralURL + $scope.EncryptedId);
                            }
                        });

                        //var EncryptedId = EncryptedId;
                        //$('#ConvertReferral_fixedAside').modal({ backdrop: 'static', keyboard: false });;
                        //$('#ConvertReferral_fixedAsidelDDLBindIFrame').attr('src', HomeCareSiteUrl.PartialAddReferralURL + EncryptedId);

                    }
                }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
            }
        }
    }

    $scope.ConvertReferralEditModelClosed = function () {
        $('#ConvertReferral_fixedAside').modal('hide');
    }
};

controllers.AddCaptureCallController.$inject = ['$scope', '$http', '$timeout','$window'];

$(document).ready(function () {
    $("#NPI").inputmask({
        mask: "9999999999",
        placeholder: "XXXXXXXXXX"
    });
    //ShowPageLoadMessage("ShowAddReferralMessage");
    var eventMethod = window.addEventListener ? "addEventListener" : "attachEvent";
    var eventer = window[eventMethod];
    var messageEvent = eventMethod == "attachEvent" ? "onmessage" : "message";
    // Listen to message from child window
    eventer(messageEvent, function (e) {

        if (e.data.indexOf('OrbeonID') >= 0) {
            //custModel.ReceiverOrbeonForm(e.data);
            custModel.SaveOrbeonForm(e.data);
        }
        else if (ValideElement(e.data)) {
            var res = JSON.parse(e.data);
            if (e.data.indexOf('OrginalFormId') > 0 && res.PageID == "ReferralDocument")
                custModel.SavedNewHtmlFormWithSubsection(e.data);

            if (res.OrgPageID == "ReferralDocument") {
                custModel.SaveDocumentFormName(res.PrimaryID);
            }
        }


    }, false);

});