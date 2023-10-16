var ReferralDocumentVM;

controllers.ReferralDocumentController = function ($scope, $http, $window, $timeout, $filter) {
    
    $scope.ItemID = '';
    ReferralDocumentVM = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnAddReferralModel").val());
    };

    var modalJson = null;
    //$scope.CurrentDate = new Date();
    $scope.CurrentDate = $filter('date')(new Date(), 'MMddy');
    $scope.DocumentDetail = {};
    $scope.Document = {};

    if ($.parseJSON($("#hdnAddReferralModel").val()) != null) {
        modalJson = $.parseJSON($("#hdnAddReferralModel").val());
        $scope.Model = modalJson;
        $scope.EncryptedReferralID = $scope.Model.Referral.EncryptedReferralID;
        $scope.ReferralID = $scope.Model.Referral.ReferralID;
        $scope.UploadFile = HomeCareSiteUrl.UploadFile;
        $scope.EmployeeID = '0';
        $scope.UserType = "Referral";
        $scope.UploadFileGoogleDrive = HomeCareSiteUrl.UploadFileGoogleDrive;
    } else {
        modalJson = $.parseJSON($("#hdnEmployeeModel").val());
        $scope.Model = modalJson;
        $scope.EncryptedEmployeeID = $scope.Model.Employee.EncryptedEmployeeID;
        $scope.EmployeeID = $scope.Model.Employee.EmployeeID;
        $scope.UploadFile = HomeCareSiteUrl.UploadEmployeeDocument;
        $scope.UserType = "Employee";
        $scope.UploadFileGoogleDrive = HomeCareSiteUrl.UploadFileEmployeeGoogleDrive;
    }



    $scope.SearchReferralDocumentListPage = {};
    $scope.CollapseSectionList = false;
    $scope.ShowFrame = false;
    $scope.SetFullScreen = false;
    $scope.ShowFormNameModal = true;

    $("a#referralDocument, a#referralDocument_clientdocuments, a#employeeDocuments").on('shown.bs.tab', function (e) {
        //$(".tab-pane a[href='#tab_ClientDocuments']").tab('show');
        $scope.GetSectionList();
    });

    $scope.UserRoleList = [];
    $scope.GetSectionList = function () {
        modalJson = $.parseJSON($("#hdnEmployeeModel").val());
        if (modalJson != null) {
            $scope.EmpId = $scope.Model.Employee.EmployeeID;
        } else {
            $scope.EmpId = 0;
        }
        $scope.Model = modalJson;
        var jsonData = angular.toJson({ EncryptedReferralID: $scope.EncryptedReferralID, UserType: $scope.UserType, EmployeeID: $scope.EmpId });
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralSectionList, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.SectionList = response.Data.SectionList;
                $scope.DocumentationTypeList = response.Data.DocumentationTypeList;
                $scope.SetYesNoList = response.Data.SetYesNoList;
                $scope.ConfigEBFormModel = response.Data.ConfigEBFormModel;
                $scope.UserRoleList = response.Data.UserRoleList;
                $scope.UserSelectedRoles = response.Data.SelectedRoles;
            }
            ShowMessages(response);
        });
    };

    $scope.SaveSection = function (data) {
        if (data.UserType == 'Employee') {
            modalJson = $.parseJSON($("#hdnEmployeeModel").val());
            $scope.Model = modalJson;
            data.EmployeeID = $scope.Model.Employee.EmployeeID;
        } else {
            data.ReferralID = $scope.ReferralID;
        }

        var jsonData = angular.toJson(data);
        AngularAjaxCall($http, HomeCareSiteUrl.SaveSectionURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.GetSectionList();
                $timeout(function () {
                    $('#color' + ($scope.SectionList.length - 1)).addClass("SetColor");
                    document.getElementById('color' + ($scope.SectionList.length - 1)).style.backgroundColor = 'rgba(' + $scope.SectionList[$scope.SectionList.length - 1].ColorRGBA + ',0.2)';
                    $scope.GetSubSectionList($scope.SectionList[$scope.SectionList.length - 1], 'color' + ($scope.SectionList.length - 1));
                }, 300);
                $('#addSectionModal').modal('hide');
            }
            ShowMessages(response);
        });
    };


    $scope.SendEmail = function () {
        //var jsonData = angular.toJson(data);
        //AngularAjaxCall($http, HomeCareSiteUrl.SaveSectionURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
        //    if (response.IsSuccess) {
        //        $scope.GetSectionList();
        //        $timeout(function () {
        //            $('#color' + ($scope.SectionList.length - 1)).addClass("SetColor");
        //            document.getElementById('color' + ($scope.SectionList.length - 1)).style.backgroundColor = 'rgba(' + $scope.SectionList[$scope.SectionList.length - 1].ColorRGBA + ',0.2)';
        //            $scope.GetSubSectionList($scope.SectionList[$scope.SectionList.length - 1], 'color' + ($scope.SectionList.length - 1));
        //        }, 300);
        //        $('#addSectionModal').modal('hide');
        //    }
        //    ShowMessages(response);
        //});
    };



    $scope.OpenAddModal = function (data) {
        $scope.Modal = data;
        $scope.AddModal = {};

        $scope.AddModal.DocumentationType = $scope.UserType == "Employee" ? 1 : null;
        $scope.AddModal.SelectedRoles = {};
        $scope.AddModal.SelectedRoles = $scope.UserSelectedRoles;
        $scope.AddModal.ShowToAll = false;

        if (data == "Section") {
            $('#addModal').modal({
                backdrop: 'static',
                keyboard: false
            });
        } else {
            if ($scope.SelectedSection == undefined) {
                ShowMessage("Please select the section where you want to add subsection.", "error");
            } else {
                $('#addModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
        }
    };

    $scope.SaveDetails = function () {
        debugger
        if (CheckErrors("#frmAdd")) {
            $scope.AddModal.UserType = $scope.UserType;
            if ($scope.Modal == "Sub Section") {
                $scope.AddModal.ParentID = $scope.SelectedSection.ComplianceID;
                $scope.SaveSubSection($scope.AddModal);
            } else {
                $scope.SaveSection($scope.AddModal);
            }
            $('#addModal').modal('hide');
        }
    };

    $('#addModal').on('hidden.bs.modal', function () {
        $scope.AddModal = {};
        $(".minicolors-swatch-color").removeAttr("style");
        HideErrors($("#frmAdd"));
    });

    $scope.OpenSubSectionModal = function () {
        if ($scope.SelectedSection == undefined) {
            ShowMessage("Please select the section where you want to add subsection.", "error");
        } else {
            $('#addSubSectionModal').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
    };

    $scope.GetSubSectionList = function (data, id) {
        modalJson = $.parseJSON($("#hdnEmployeeModel").val());
        $scope.Model = modalJson;
        //Remove BG Color from all other div
        $("[id^=color]").removeClass("SetColor");
        $("[id^=color]").css({ "background-color": "rgba(249, 249, 249, 0.9)" });
        $scope.SelectedSectionID = id;
        $scope.SelectedSection = data;
        $scope.SelectedSubSectionID = null;
        $scope.SelectedSubSection = null;
        $scope.ParentID = 0;
        $scope.CollapseSectionList = true;
        $scope.ShowFrame = false;

        if (modalJson != null) {
            $scope.EmployeeID = $scope.Model.Employee.EmployeeID;
        }
        //else {
        //    $scope.EmployeeID = $scope.EncryptedReferralID;
        //}
        var jsonData = angular.toJson({ EncryptedReferralID: $scope.EncryptedReferralID, id: data.ComplianceID, userType: $scope.UserType, EmployeeID: $scope.EmployeeID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralSubSectionList, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.SubSectionList = response.Data;
                $scope.FormList = {};
                $scope.ComplianceID = $scope.SelectedSection.ComplianceID;
                $scope.IsTimeBased = $scope.SelectedSection.IsTimeBased;
                $scope.GetReferralDocumentList();
                $scope.ClearFrame();
                $("html, body").animate({ scrollTop: 0 }, "slow");

                //Set BG Color to slected div
                $('#' + id).addClass("SetColor");
                document.getElementById(id).style.backgroundColor = 'rgba(' + data.ColorRGBA + ',0.2)';
            }
            ShowMessages(response);
        });
    };

    $scope.SaveSubSection = function (data) {
        debugger
        //modalJson = $.parseJSON($("#hdnEmployeeModel").val());
        //$scope.Model = modalJson;
        //data.EmployeeID = $scope.Model.Employee.EmployeeID;
        if (data.UserType == 'Employee') {
            modalJson = $.parseJSON($("#hdnEmployeeModel").val());
            $scope.Model = modalJson;
            data.EmployeeID = $scope.Model.Employee.EmployeeID;
        } else {
            data.ReferralID = $scope.ReferralID;
        }
        var jsonData = angular.toJson(data);
        AngularAjaxCall($http, HomeCareSiteUrl.SaveSubSectionURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.GetSubSectionList($scope.SelectedSection, $scope.SelectedSectionID);
                //$scope.GetSubSectionList($scope.SelectedSection, 'colorSub' + $scope.SubSectionList.length);

                //Set BG Color to slected div
                $timeout(function () {
                    $('#colorSub' + ($scope.SubSectionList.length - 1)).addClass("SetColor");
                    document.getElementById('colorSub' + ($scope.SubSectionList.length - 1)).style.backgroundColor = 'rgba(' + $scope.SelectedSection.ColorRGBA + ',0.2)';
                    $scope.GetFormList($scope.SubSectionList[$scope.SubSectionList.length - 1], 'colorSub' + ($scope.SubSectionList.length - 1));
                }, 300);
                $('#addSubSectionModal').modal('hide');
            }
            ShowMessages(response);
        });
    };

    $('#addSectionModal').on('hidden.bs.modal', function () {
        $scope.Section = {};
        $(".minicolors-swatch-color").removeAttr("style");
        HideErrors($("#frmSection"));
    });

    $('#addSubSectionModal').on('hidden.bs.modal', function () {
        $scope.SubSection = {};
        HideErrors($("#frmSubSection"));
    });

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

    $scope.OpenAddFormModal = function () {
        if ($scope.SelectedSection == undefined && ($scope.SelectedSubSection == undefined || $scope.SelectedSubSection == null)) {
            ShowMessage("Select the Folder or Subfolder to add document.", "error");
        } else {
            $scope.ShowUpload = true;
            $scope.ComplianceID = ($scope.SelectedSubSection == undefined || $scope.SelectedSubSection == null) ? $scope.SelectedSection.ComplianceID : $scope.SelectedSubSection.ComplianceID;
            $('#addFormModal').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
    };

    $scope.OpenMapFormModel = function (IsBindToFrame) {
        $scope.IsBindToFrame = IsBindToFrame;
        AngularAjaxCall($http, HomeCareSiteUrl.GetOrganizationFormListURL, null, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.OrganizationFormList = response.Data;
            }
        });
        $('#mapFormModal').modal({
            backdrop: 'static',
            keyboard: false
        });
    };

    $scope.SelectForm = function (item) {
        $scope.AddModal.EBFormID = item.EBFormID;
        $scope.AddModal.FormName = item.FormLongName;
        $scope.AddModal.NameForUrl = item.NameForUrl;
        $scope.AddModal.Version = item.Version;
        $('#mapFormModal').modal('hide');
        $scope.frmSearch = null;
    };

    $scope.RemoveMappedForm = function () {
        $scope.AddModal.EBFormID = null;
    };

    $scope.IsEditableCompliance = function () {
        return !NotEditableComplianceIDs.includes($scope.ComplianceID) && !NotEditableComplianceIDs.includes($scope.ParentID);
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

                document.getElementById('myIframe').src = newURL;
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

    $('#mapFormModal').on('hidden.bs.modal', function () {
        $scope.frmSearch = null;
    });
    // Save as Code
    function saveAs(blob, fileName) {
        var url = window.URL.createObjectURL(blob);

        var doc = document.createElement("a");
        doc.href = url;
        doc.download = fileName;
        doc.click();
        window.URL.revokeObjectURL(url);
    }

    $scope.OpenSavedForm_new = function (item, isEditMode) {

        var EmployeeID = $scope.EmployeeID == undefined ? 0 : $scope.EmployeeID;
        var ReferralID = $scope.ReferralID == undefined ? 0 : $scope.ReferralID;
        var jsonData = angular.toJson({
            EbriggsFormMppingID: item.EbriggsFormMppingID,
            EmployeeID: EmployeeID,
            ReferralID: ReferralID,
            Form_URL: encodeURIComponent(item.InternalFormPath),
            File_Name: item.Name
        });
        AngularAjaxCall($http, HomeCareSiteUrl.LoadPdfForm_URL, jsonData, "Post", "json", "application/json").success(function (data) {
            window.location = '/Form/Download?fileGuid=' + data.FileGuid
                + '&filename=' + data.FileName;
        });

    };
    $scope.OpenSavedForm = function (item, isEditMode) {
        //saurabh

        //var file = new Blob([item], { type: 'application/pdf' });
        //saveAs(file, 'Claims.pdf');
        if (isEditMode) {
            $scope.frameLoader = true;
            $scope.ShowFrame = true;
            $scope.ShowFormNameModal = false;
            //if (item.IsInternalForm) {
            var EmployeeID = $scope.EmployeeID == undefined ? 0 : $scope.EmployeeID;
            var ReferralID = $scope.ReferralID == undefined ? 0 : $scope.ReferralID;

            //var path = HomeCareSiteUrl.LoadHtmlFormURL;
            //if (item.InternalFormPath.indexOf('.pdf') !== -1) {
            //    //path = HomeCareSiteUrl.LoadPdfFormURL;
            //    path = window.MyezcarePdfFormsUrl;
            //}
            var path = HomeCareSiteUrl.OrbeonLoadHtmlFormURL;
            if (!ValideElement(item.EBriggsFormID)) { item.EBriggsFormID = "0" };
            var formName = $scope.Form == undefined ? null : $scope.Form.FormName;

            var newURL = path
                + "?FormURL=" + encodeURIComponent(item.NameForUrl) + "/edit?form-version=" + item.Version
                + "&OrgPageID=" + "ReferralDocument"
                + "&IsEditMode=" + "true"
                + "&EmployeeID=" + EmployeeID
                + "&ReferralID=" + ReferralID
                + "&EBriggsFormID=" + item.EBriggsFormID
                + "&OriginalEBFormID=" + item.EBFormID
                + "&FormId=" + item.FormId
                + "&UserType=" + $scope.UserType
                + "&SubSectionID=" + $scope.ComplianceID
                + "&FormName=" + formName
                + "&UpdateFormName=0"
                + "&OrganizationId=" + window.OrgID
                + "&UserId=" + window.LUserId
                + "&EbriggsFormMppingID=" + item.EbriggsFormMppingID;


            document.getElementById('myIframe').src = newURL;

            //}
            //else {

            //    var data = $scope.ConfigEBFormModel;
            //    var newFormUrl = data.EBBaseSiteUrl + "/form/" + item.NameForUrl + "?version=" + item.Version + "&id=" + item.EBriggsFormID + "&PageId=" + window.ReferralDocumentPageId;
            //    var newURL = data.MyezcareFormsUrl + "?formURL=" + encodeURIComponent(newFormUrl);
            //    document.getElementById('myIframe').src = newURL;
            //}
        }
        else {
            //PDF Download
            //if (item.IsInternalForm) {
            //    var EmployeeID = $scope.EmployeeID == undefined ? 0 : $scope.EmployeeID;
            //    var ReferralID = $scope.ReferralID == undefined ? 0 : $scope.ReferralID;
            //    var jsonData = angular.toJson({
            //        EbriggsFormMppingID: item.EbriggsFormMppingID,
            //        EmployeeID: EmployeeID,
            //        ReferralID: ReferralID,
            //        Form_URL: encodeURIComponent(item.InternalFormPath),
            //        File_Name: item.Name
            //    });

            //    AngularAjaxCall($http, HomeCareSiteUrl.LoadPdfForm_URL, jsonData, "Post", "json", "application/json").success(function (data) {
            //        window.location = '/Form/Download?fileGuid=' + data.FileGuid + '&filename=' + data.FileName;
            //    });
            //}
            //else {
            //    var data = $scope.ConfigEBFormModel;

            //    var newFormUrl = data.EBBaseSiteUrl + "/pdf/" + item.NameForUrl + "?version=" + item.Version + "&id=" + item.EBriggsFormID;

            //    var newURL = data.MyezcareFormsUrl + "?formURL=" + encodeURIComponent(newFormUrl);

            //    document.getElementById('myIframe').src = newURL;

            //}

            var path = HomeCareSiteUrl.OrbeonLoadHtmlFormURL;
            var newURL = path
                + "?FormURL=" + encodeURIComponent(item.NameForUrl) + "/view?form-version=" + item.Version
                + "&OrgPageID=" + "ReferralDocument"
                + "&IsEditMode=" + "true"
                + "&EmployeeID=" + EmployeeID
                + "&ReferralID=" + ReferralID
                + "&EBriggsFormID=" + item.EBriggsFormID
                + "&OriginalEBFormID=" + item.EBFormID
                + "&FormId=" + item.FormId
                + "&UserType=" + $scope.UserType
                + "&SubSectionID=" + $scope.ComplianceID
                + "&FormName=" + formName
                + "&UpdateFormName=0"
                + "&OrganizationId=" + window.OrgID
                + "&UserId=" + window.LUserId
                + "&EbriggsFormMppingID=" + item.EbriggsFormMppingID;

            document.getElementById('myIframe').src = newURL;
        }
    };

    $scope.OpenUploadedDocument = function (filepath) {
        var splitPath = filepath.split(".");
        var extension = splitPath[splitPath.length - 1];

        if (extension == "doc" || extension == "docx" || extension == "xls" || extension == "xlsx") {
            window.location = filepath;
            return;
        }
        $scope.ShowFrame = true;
        document.getElementById('myIframe').src = filepath;
    };

    $scope.OpenPDF = function (url) {
        //
        PDFObject.embed(url, "#example1");
    };

    $scope.OpenNewHtmlForm = function (item) {

        //  if (item.IsInternalForm) {
        var EmployeeID = $scope.EmployeeID == undefined ? 0 : $scope.EmployeeID;
        var ReferralID = $scope.ReferralID == undefined ? 0 : $scope.ReferralID;

        // var path = HomeCareSiteUrl.LoadHtmlFormURL;
        var path = HomeCareSiteUrl.OrbeonLoadHtmlFormURL;
        if (item.InternalFormPath.indexOf('.pdf') !== -1) {
            //path = HomeCareSiteUrl.LoadPdfFormURL;
            path = window.MyezcarePdfFormsUrl;
        }

        if (!ValideElement(item.EBriggsFormID)) { item.EBriggsFormID = "0" };
        var formName = $scope.Form == undefined ? null : $scope.Form.FormName;

        var newURL = path
            + "?FormURL=" + encodeURIComponent(item.NameForUrl) + "/view?form-version=" + item.Version
            + "&OrgPageID=" + "ReferralDocument"
            + "&IsEditMode=" + "true"
            + "&EmployeeID=" + EmployeeID
            + "&ReferralID=" + ReferralID
            + "&EBriggsFormID=" + item.EBriggsFormID
            + "&OriginalEBFormID=" + item.EBFormID
            + "&FormId=" + item.FormId
            + "&UserType=" + $scope.UserType
            + "&SubSectionID=" + $scope.ComplianceID
            + "&FormName=" + formName
            + "&UpdateFormName=0"
            + "&OrganizationId=" + window.OrgID
            + "&UserId=" + window.LUserId
            + "&EbriggsFormMppingID=" + "0";
        $scope.ChildWindow = window.open(newURL, "_blank", "width=" + screen.availWidth + ",height=" + screen.availHeight);
        //} else {
        //    var data = $scope.ConfigEBFormModel;
        //    var newFormUrl = data.EBBaseSiteUrl + "/form/" + item.NameForUrl + "?version=" + item.Version;// + "&tenantGuid=" + response.tenantGuid;
        //    var newURL = data.MyezcareFormsUrl + "?formURL=" + encodeURIComponent(newFormUrl);
        //    $scope.ChildWindow = window.open(newURL, "_blank", "width=" + screen.availWidth + ",height=" + screen.availHeight);
        //}
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

    $scope.MapForm = function (item) {
        var jsonData = angular.toJson({
            ComplianceID: $scope.ComplianceID,
            EBFormID: item.EBFormID,
            UserType: $scope.UserType,
            MapPermanently: $scope.MapPermanently,
            SectionID: $scope.SelectedSection.ComplianceID
        });
        AngularAjaxCall($http, HomeCareSiteUrl.MapFormURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.SetPreview(response.Data.MapFormDetail);
                $scope.SectionList = response.Data.SectionList;
                $scope.SubSectionList = response.Data.SubSectionList;

                if ($scope.MapPermanently) {
                    $scope.SelectedSection.FormName = item.FormLongName;
                    $scope.SelectedSection.Version = item.Version;
                    $scope.SelectedSection.NameForUrl = item.NameForUrl;
                }

                $timeout(function () {
                    //Set BG Color to slected section
                    $('#' + $scope.SelectedSectionID).addClass("SetColor");
                    document.getElementById($scope.SelectedSectionID).style.backgroundColor = 'rgba(' + $scope.SelectedSection.ColorRGBA + ',0.2)';

                    if ($scope.SelectedSubSectionID != undefined) {
                        $('#' + $scope.SelectedSubSectionID).addClass("SetColor");
                        document.getElementById($scope.SelectedSubSectionID).style.backgroundColor = 'rgba(' + $scope.SelectedSection.ColorRGBA + ',0.2)';
                    }
                }, 100);
            }
            if ($scope.MapPermanently)
                ShowMessages(response);
        });
        $('#mapFormModal').modal('hide');
        $scope.frmSearch = null;
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

    $scope.ShowSectionList = function () {
        $scope.CollapseSectionList = false;
    };

    $scope.ResizeScreen = function (data) {
        $scope.SetFullScreen = data == 1 ? true : false;
    };

    $scope.ClosePreview = function () {
        $scope.ShowFrame = false;
        $scope.SetFullScreen = false;
        $scope.ClearFrame();
    };

    $scope.setColor = function (color, id) {
        if ($('#' + id).hasClass("SetColor") == false)
            document.getElementById(id).style.backgroundColor = 'rgba(' + color + ')';
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

    $('#FormNameModal').on('hidden.bs.modal', function () {
        HideErrors($("#frmFormName"));

        if ($scope.ResData != undefined && ($scope.EbriggsFormMppingID == undefined)) {
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
                "UpdateFormName": true
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
        }

        if ($scope.EbriggsFormMppingID > 0) {
            var jsonData1 = angular.toJson({
                "EbriggsFormMppingID": $scope.EbriggsFormMppingID,
                "FormName": $scope.Form == undefined ? null : $scope.Form.FormName,
                "UpdateFormName": true
            });
            $scope.EbriggsFormMppingID = undefined;
            AngularAjaxCall($http, HomeCareSiteUrl.SaveDocumentFormNameURL, jsonData1, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.ClosePreview();
                    $scope.GetReferralDocumentList();
                    $scope.Form = {};
                }
                ShowMessages(response);
            });
        }
    });

    $scope.DeleteDocument = function (data) {
        var deleteDocumentUrl = HomeCareSiteUrl.DeleteReferralDocumentURL;

        if (data.StoreType == 'Google Drive') { deleteDocumentUrl = HomeCareSiteUrl.DeleteReferralDocumentGoogleURL; }

        //data.ComplianceID = $scope.ComplianceID;
        //data.ReferralID = $scope.ReferralID;
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = angular.toJson(data);
                AngularAjaxCall($http, deleteDocumentUrl, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        //$scope.FormList = response.Data;
                        $scope.GetReferralDocumentList();
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteDocumentMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };


    $scope.SearchForm = function (searchKey) {
        var results = [];
        var myExp = new RegExp(searchKey, "i");

        if (searchKey != "") {
            $.each($scope.TempFormList, function (key, val) {
                var formName = val.FormName + '_' + moment(val.UpdatedDate).format("MM/DD/YYYY h:mm a");
                val.FileName = val.FileName == null ? "" : val.FileName;
                val.FormName = val.FormName == null ? "" : val.FormName;
                val.Tags = val.Tags == null ? "" : val.Tags;
                if ((val.FileName.search(myExp) != -1) || (formName.search(myExp) != -1) || (val.Tags.search(myExp) != -1)) {
                    results.push(val);
                }
            });
            $scope.FormList = results;
        } else {
            $scope.FormList = $scope.TempFormList;
        }
    };

    //region File Upload--------------------------------
    //$scope.UploadFile = HomeCareSiteUrl.UploadFile;

    $scope.UploadingFileList = [];

    $scope.BeforeSend = function (e, data) {
        var isValidImage = true;
        var fileName;
        var errorMsg;
        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            //if (extension == "exe") {
            if (extension != "doc" && extension != "docx" && extension != "txt" && extension != "pdf" && extension != "rtf" && extension != "jpg" && extension != "jpeg"
                && extension != "png" && extension != "xls" && extension != "xlsx") {
                //file.FileProgress = 100;
                $scope.UploadingFileList.remove(file);
                //errorMsg = window.InvalidImageUploadMessage;
                //isValidImage = false;
                ShowMessage(window.InvalidDocumentUploadMessage, "error");
                isValidImage = false;
            }
            if ((file.size / 1024) > parseInt(window.FileSize)) {
                file.FileProgress = 100;
                errorMsg = window.MaximumUploadImageSizeMessage;
                isValidImage = false;
            }

            fileName = file.name;
        });

        if (isValidImage) {
            $scope.IsFileUploading = true;
        }
        $scope.$apply();
        var response = { IsSuccess: isValidImage, Message: errorMsg };
        return response;
    };

    $scope.Progress = function (e, data) {
        console.log(data.files[0].name + ":-" + Math.round((data.loaded / data.total) * 100));
    };

    $scope.AfterSend = function (data) {
        $scope.IsFileUploading = false;
        ShowMessage("File uploaded successfully");
        var sendData = ($scope.SelectedSubSection == undefined || $scope.SelectedSubSection == null) ? $scope.SelectedSection : $scope.SelectedSubSection;
        $scope.ComplianceID = ($scope.SelectedSubSection == undefined || $scope.SelectedSubSection == null) ? $scope.SelectedSection.ComplianceID : $scope.SelectedSubSection.ComplianceID;
        var id = ($scope.SelectedSubSectionID == undefined || $scope.SelectedSubSectionID == null) ? $scope.SelectedSectionID : $scope.SelectedSubSectionID;
        //$scope.GetFormList(sendData, id);
        $scope.GetReferralDocumentList();
        $scope.ShowUpload = false;
        $('#addFormModal').modal('hide');
    };

    $('#addFormModal').on('hidden.bs.modal', function () {
        $scope.ShowUpload = false;
    });

    //endregion File Upload------------------------------


    //region Document List-------------------------------

    $scope.ReferralDocumentPager = new PagerModule("CreatedDate", null, "Desc");

    $scope.SetPostData = function (fromIndex, complianceID) {
        $scope.SearchReferralDocumentListPage.ComplianceID = complianceID;
        $scope.SearchReferralDocumentListPage.EncryptedReferralID = $scope.EncryptedReferralID;
        $scope.SearchReferralDocumentListPage.EncryptedEmployeeID = $scope.EncryptedEmployeeID;
        $scope.SearchReferralDocumentListPage.UserType = $scope.UserType;
        var pagermodel = {
            //id: value,
            //id2: complianceID,
            searchReferralDocument: $scope.SearchReferralDocumentListPage,
            pageSize: $scope.ReferralDocumentPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ReferralDocumentPager.sortIndex,
            sortDirection: $scope.ReferralDocumentPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.SearchReferralDocumentListPage = $.parseJSON(angular.toJson($scope.SearchReferralDocumentListPage));
    };

    $scope.GetReferralDocumentList = function (isSearchDataMappingRequire) {
        //$scope.ReferralDocumentPager.pageSize = 50;
        $scope.SetFullScreen = false;
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();

        var jsonData = $scope.SetPostData($scope.ReferralDocumentPager.currentPage, $scope.ComplianceID);
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralDocumentList, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralDocumentList = response.Data.Items;
                $scope.ReferralDocumentPager.currentPageSize = response.Data.Items.length;
                $scope.ReferralDocumentPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.SearchReferralDocument = function () {
        $scope.ReferralDocumentPager.currentPage = 1;
        $scope.ReferralDocumentPager.getDataCallback(true);
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            $scope.SearchReferralDocumentListPage = {};
            $scope.SearchReferralDocumentListPage.SearchInDate = 'Added';
            $scope.SearchReferralDocumentListPage.SearchType = 'Directory';
            $scope.ReferralDocumentPager.getDataCallback();
        });
    };

    $scope.ReferralDocumentPager.getDataCallback = $scope.GetReferralDocumentList;

    $scope.EditDocument = function (data) {
        $scope.DocumentDetail = $.parseJSON(JSON.stringify(data));
        $scope.DocumentDetail.FileName = data.Name;
        $scope.DocumentDetail.KindOfDocument = (data.KindOfDocument == "Internal") ? 1 : 2;
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
        $('#EditDocument').modal({
            backdrop: 'static',
            keyboard: false
        });
    };

    $scope.SaveDocument = function (data) {
        if (CheckErrors("#frmDocumentEdit")) {
            var jsonData = angular.toJson($scope.DocumentDetail);
            AngularAjaxCall($http, HomeCareSiteUrl.EditNewDocumentURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.GetReferralDocumentList();
                    $('#EditDocument').modal('hide');
                }
                ShowMessages(response);
            });
        }
    };

    //endregion Document List-----------------------------


    //region FormName
    $scope.SaveFormName = function (data) {
        if (CheckErrors("#frmFormName")) {
            $('#FormNameModal').modal('hide');
        }
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
    //endregion
    $scope.SliderLeft = function () {
        angular.element('#SliderLeft').addClass("LeftCallShow");
        angular.element('#slider').addClass("LeftCallShow");
        angular.element('#slider').removeClass("RightCallShow");
        angular.element('#rightShow').removeClass("RightCallShow");
    }
    $scope.SliderRight = function () {
        angular.element('#rightShow').addClass("RightCallShow");
        angular.element('#slider').addClass("RightCallShow");
        angular.element('#slider').removeClass("LeftCallShow");
        angular.element('#SliderLeft').removeClass("LeftCallShow");
    }

    $scope.GetFileFromGoogle = function (data) {
        var jsonData = angular.toJson(data);
        AngularAjaxCall($http, HomeCareSiteUrl.ListFilesGoogleDriveUrl, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.DocumentListGoogleDrive = [];
                $scope.DocumentListGoogleDrive = JSON.parse(response.Data).files;
            }
            ShowMessages(response);
        });
    };



    //$scope.filterIcons('mime2fa', function () {
    //$scope.filterIcons = function (mimeType) {

    //    const iconClasses = {
    //        // Media
    //        'image': 'fa-file-image-o',
    //        'audio': 'fa-file-audio-o',
    //        'video': 'fa-file-video-o',
    //        // Documents
    //        'application/pdf': 'fa-file-pdf-o',
    //        'application/msword': 'fa-file-word-o',
    //        'application/vnd.ms-word': 'fa-file-word-o',
    //        'application/vnd.oasis.opendocument.text': 'fa-file-word-o',
    //        'application/vnd.openxmlformats-officedocument.wordprocessingml': 'fa-file-word-o',
    //        'application/vnd.ms-excel': 'fa-file-excel-o',
    //        'application/vnd.openxmlformats-officedocument.spreadsheetml': 'fa-file-excel-o',
    //        'application/vnd.oasis.opendocument.spreadsheet': 'fa-file-excel-o',
    //        'application/vnd.ms-powerpoint': 'fa-file-powerpoint-o',
    //        'application/vnd.openxmlformats-officedocument.presentationml': 'fa-file-powerpoint-o',
    //        'application/vnd.oasis.opendocument.presentation': 'fa-file-powerpoint-o',
    //        'text/plain': 'fa-file-text-o',
    //        'text/html': 'fa-file-code-o',
    //        'application/json': 'fa-file-code-o',
    //        // Archives
    //        'application/gzip': 'fa-file-archive-o',
    //        'application/zip': 'fa-file-archive-o',
    //        'application/vnd.google-apps.spreadsheet': 'fa-file-excel-o',
    //        'application/vnd.google-apps.document': 'fa-file-word-o',
    //        'application/vnd.google-apps.form': 'fa fa-google'
    //    };

    //    let fa = 'file-o';
    //    for (let key in iconClasses) {
    //        if (iconClasses.hasOwnProperty(key) && mimeType.search(key) === 0) {
    //            fa = iconClasses[key];
    //        }
    //    }
    //    return 'fa fa-2x ' + fa;
    //};

    $scope.SaveDocumentFromGoogleDrive = function (data) {
        var EmployeeID = $scope.EmployeeID == undefined ? 0 : $scope.EmployeeID;
        var ReferralID = $scope.ReferralID == undefined ? 0 : $scope.ReferralID;
        var ComplianceID = ($scope.SelectedSubSection == undefined || $scope.SelectedSubSection == null) ? $scope.SelectedSection.ComplianceID : $scope.SelectedSubSection.ComplianceID;

        var jsonData = angular.toJson({
            EmployeeID: EmployeeID,
            ReferralID: ReferralID,
            ComplianceID: ComplianceID,
            GoogleFileId: data
        });

        //var jsonData = angular.toJson({ "id": data });
        AngularAjaxCall($http, HomeCareSiteUrl.LinkGoogleDocument, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {

                $scope.IsFileUploading = false;
                ShowMessage("File uploaded linked");
                var sendData = ($scope.SelectedSubSection == undefined || $scope.SelectedSubSection == null) ? $scope.SelectedSection : $scope.SelectedSubSection;
                $scope.ComplianceID = ($scope.SelectedSubSection == undefined || $scope.SelectedSubSection == null) ? $scope.SelectedSection.ComplianceID : $scope.SelectedSubSection.ComplianceID;
                var id = ($scope.SelectedSubSectionID == undefined || $scope.SelectedSubSectionID == null) ? $scope.SelectedSectionID : $scope.SelectedSubSectionID;

                $scope.GetReferralDocumentList();
                $scope.ShowUpload = false;
                $('#GetFileFromGoogle').modal('hide');
                $('#addFormModal').modal('hide');
            }
            ShowMessages(response);
        });
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

    $scope.DuplicateOrbeonForm = function (item, successCallback) {
        let myFirstPromise = new Promise((resolve) => {
            if (item) {
                if (!item.IsDuplicate) {
                    resolve(item);
                } else {
                    var jsonData = angular.toJson(item);
                    AngularAjaxCall($http, HomeCareSiteUrl.DuplicateOrbeonFormURL, jsonData, "Post", "json", "application/json").success(function (response) {
                        if (response.IsSuccess) {
                            item.DocumentID = response.Data.DocumentID;
                            resolve(item);
                        } else {
                            ShowMessages(response);
                        }
                    });
                }
            }
        })
        myFirstPromise.then((data) => {
            successCallback(data);
        });
    };

    $scope.OpenSavedFormOrbeon = function (item, mode, isClone) {

        if (mode != '') {
            $scope.Document = {};
            $scope.Document.ReferralDocumentID = item.ReferralDocumentID;
            $scope.Document.Edit = true;
            $scope.Document.Clone = isClone;
            $scope.frameLoader = true;
            $scope.ShowFrame = true;
            $scope.ShowFormNameModal = false;

            var dupForm = { IsDuplicate: isClone, DocumentID: item.GoogleFileId, NameForUrl: item.FilePath };
            $scope.DuplicateOrbeonForm(dupForm, (data) => {
                var EmployeeID = $scope.EmployeeID == undefined ? 0 : $scope.EmployeeID;
                var ReferralID = $scope.ReferralID == undefined ? 0 : $scope.ReferralID;

                var formName = $scope.Form == undefined ? null : $scope.Form.FormName;
                var urlToGet = HomeCareSiteUrl.OrbeonLoadHtmlFormURL + '?FormURL=' + encodeURIComponent(item.FilePath);
                var newURL = urlToGet + '/' + mode + '/' + data.DocumentID
                    + "?orbeon-embeddable=true"
                    + "&OrgPageID=" + window.ReferralDocumentPageId
                    + "&IsEditMode=" + "true"
                    + "&EmployeeID=" + EmployeeID
                    + "&ReferralID=" + ReferralID
                    + "&FormId=" + item.FormId
                    + "&UserType=" + $scope.UserType
                    + "&SubSectionID=" + $scope.ComplianceID
                    + "&FormName=" + formName
                    + "&OrganizationId=" + window.OrgID
                    + "&UserId=" + window.LUserId
                    ;

                document.getElementById('myIframe').src = newURL;
            });

        }
    };
    $scope.MailModel = {};
    $scope.SendReferralAttachment = function () {
        if ($("#FromTo")[0].selectedIndex <= 0) {
            ShowMessage("From email is missing", "error");
            return;
        }
        if ($("#tagsinputTo").val().length < 2) {

            ShowMessage("Send to is missing", "error");
            return;
        }

        if ($("#txtSubject").val().length < 2) {

            ShowMessage("Subject is missing", "error");
            return;
        }

        //validating receiver email address
        if (!IsValidEmail($("#tagsinputTo").val())) {
            ShowMessage("Make sure to enter valid receiver email address(es) and use , for multiple email addresses!", "error");
            return;
        }

        //validating CC email address
        if ($("#tagsinputCC").val().length > 0 && !IsValidEmail($("#tagsinputCC").val())) {
            ShowMessage("Make sure to enter valid CC email address(es) and use , for multiple email addresses!", "error");
            return;
        }

        //validating BCC email address
        if ($("#tagsinputBCC").val().length > 0 != "" && !IsValidEmail($("#tagsinputBCC").val())) {
            ShowMessage("Make sure to enter valid BCC email address(es) and use , for multiple email addresses!", "error");
            return;
        }

        $scope.MailModel.To = $("#tagsinputTo").val();
        $scope.MailModel.CC = $("#tagsinputCC").val();
        $scope.MailModel.From = $scope.OrganizationSettings;
        $scope.MailModel.Subject = $("#txtSubject").val();
        $scope.MailModel.Body = $('.note-editable').html();
        //$scope.MailModel.ResourceFile = $('#EmailAttachment')[0].files[0];
        // $scope.MailModel.Attachment = $('#EmailAttachment')[0].files[0];
        var jsonData = angular.toJson($scope.MailModel);

        AngularAjaxCall($http, "/hc/referral/SendReferralAttachment", jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {

                ShowMessage("Email sent successfully", "Success");
                $('#sendemail').modal('hide');
                $scope.Reset();
            }
            if (response.IsSuccess === false) {
                ShowMessage("Mail delivery failed", "error");

            }
            $scope.Reset();

        });
    };

    $scope.physicanList = [];
    $scope.relative = [];
    $scope.assignee = {};
    $scope.casemanager = {};
    $scope.GetReferralAttachment = function (item) {
        $('#sendemail').modal({
            backdrop: 'static',
            keyboard: false
        });
        var jsonData = angular.toJson({ refID: $scope.Model.Referral.EncryptedReferralID, ReferralDocumentID: item.ReferralDocumentID });
        AngularAjaxCall($http, "/hc/referral/GetReferralAttachment", jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.ReceipentList = response.Data.Receipent;
                $scope.physicanList = response.Data.physicanList;
                $scope.relative = response.Data.relative;
                $scope.casemanager = response.Data.casemanager;
                $scope.assignee = response.Data.assignee;
                //$('#ReferralContact').modal({
                //    backdrop: 'static',
                //    keyboard: false
                //});
            }
            //ShowMessages(response);
        });


    };
    $scope.Emails = [];
    $scope.SelectReferral = function (referral) {
        if (referral.IsChecked)
            $scope.Emails.push(referral.Email);
        else
            $scope.Emails.remove(referral.Email);

        if ($scope.Emails.length == $scope.ReferralListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };
    $scope.AddEmail = function (referral) {
        var x = $scope.Emails.toString();
        $('#tagsinputCC').val(x);
        $('#ReferralContact').modal('hide');
    }
    //$scope.GetReferralAttachment = function (item) {
    //    $('#sendemail').modal({
    //        backdrop: 'static',
    //        keyboard: false
    //    });
    //    var jsonData = angular.toJson({ refID: $scope.Model.Referral.EncryptedReferralID, ReferralDocumentID: item.ReferralDocumentID });
    //    AngularAjaxCall($http, "/hc/referral/GetReferralAttachment", jsonData, "Post", "json", "application/json").success(function (response) {
    //        if (response.IsSuccess) {
    //            if (response.Data.physicanList.length > 0) {
    //                var tbl = '<br/><table>';
    //                tbl += '<tr><td style="font-size:16px;padding-bottom:10px;">Physician</td></tr>';
    //                for (var i = 0; i < response.Data.physicanList.length; i++) {

    //                    tbl += '<tr><td>test' + response.Data.physicanList[i].PhysicianName + '<br/><a  href="#" id="' + response.Data.physicanList[i].Email + '" class="Physician">' + response.Data.physicanList[i].Email + '</a></td></tr>';

    //                }
    //                tbl += '</table>';
    //                $("#Physician").append(tbl);
    //            }

    //            var tbl1 = '<br/><table>';
    //            tbl1 += '<tr><td style="font-size:16px;padding-bottom:10px;">Case Manager</td></tr>';
    //            tbl1 += '<tr><td>' + response.Data.casemanager.CaseManager + '<br/><a  href="#" id="' + response.Data.casemanager.Email + '" class="Physician">' + response.Data.casemanager.Email + '</a></td></tr>';
    //            tbl1 += '</table>';
    //            $("#CaseManager").append(tbl1);


    //            var tbl2 = '<br/><table>';
    //            tbl2 += '<tr><td style="font-size:16px;padding-bottom:10px;">Assignee</td></tr>';
    //            tbl2 += '<tr><td>' + response.Data.assignee.Assignee + '<br/><a  href="#" id="' + response.Data.assignee.Email + '" class="Physician">' + response.Data.assignee.Email + '</a></td></tr>';
    //            tbl2 += '</table>';
    //            $("#Relation").append(tbl2);


    //            var tbl3 = '<br/><table>';
    //            tbl3 += '<tr><td style="font-size:16px;padding-bottom:10px;">Relatives</td></tr>';
    //            for (var i = 0; i < response.Data.relative.length; i++) {

    //                tbl3 += '<tr><td style="padding-bottom:9px;">' + response.Data.relative[i].Relative + '<br/><a  href="#" id="' + response.Data.relative[i].Email + '" class="Physician">' + response.Data.relative[i].Email + '</a></td></tr>';

    //            }
    //            tbl3 += '</table>';
    //            $("#Relative").append(tbl3);



    //        }
    //        //ShowMessages(response);
    //    });
    //};

    $scope.GetTemplateList = function () {
        AngularAjaxCall($http, "/hc/referral/GetTemplateList", "", "Get", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.TemplateList = response.Data;
                $scope.EmailTemplate = "0";
            }
            //ShowMessages(response);
        });
    };
    $scope.getTokens = function () {
        ReferralID = $scope.Model.Referral.ReferralID == undefined ? 0 : $scope.Model.Referral.ReferralID;;
        var jsonData = angular.toJson({ refID: ReferralID });
        AngularAjaxCall($http, "/hc/referral/GetTokenList", jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.TokenList = response.Data;
            }
        });
    }

    $scope.GetOrganizationSettings = function () {
        AngularAjaxCall($http, "/hc/referral/GetOrganizationSettings", "", "Get", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.OrganizationSettingList = response.Data;
                $scope.OrganizationSettings = "0";
            }
            //ShowMessages(response);
        });
    };

    $scope.GetTemplateDetails = function () {
        var jsonData = angular.toJson({ name: $scope.EmailTemplate, ReferralID: $scope.Model.Referral.ReferralID });
        AngularAjaxCall($http, "/hc/referral/GetTemplateDetails", jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.GetSubject = response.Data.EmailTemplateSubject;
                $('.panel-body').html(response.Data.EmailTemplateBody);
            }
            ShowMessages(response);
        });
    };




    var count = 0;
    $scope.GetEmployeeEmail = function () {
        if (count === 0) {
            AngularAjaxCall($http, "/hc/referral/GetEmployeeEmail", "", "Get", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    count = count + 1;
                    $("#tagsinputCC").val(response.Message);
                }
                //ShowMessages(response);
            });
        }
        else {
            $("#tagsinputCC").val('');
            count = 0;
        }

    };

    $scope.SendFax = function () {

        var str = $('.all-mail').text();
        var res = str.replace("x", " ");
        $scope.FaxModel.To = res;
        $scope.FaxModel.DocumentID = $scope.ItemID;
        $scope.FaxModel.ReferralID = $scope.Model.Referral.ReferralID === undefined ? 0 : $scope.Model.Referral.ReferralID;
        var jsonData = angular.toJson($scope.FaxModel);
        AngularAjaxCall($http, "/hc/referral/SendFax", jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.TokenList = response.Data;
            }
        });
    };

    $scope.GetItem = function (item) {
        $scope.ItemID = item.ReferralDocumentID;
    };

    $("a#referralDocument").on('shown.bs.tab', function (e) {
        $scope.getTokens();
        $scope.GetTemplateList();
        $scope.GetOrganizationSettings();
    });


    $scope.Reset = function () {
        $scope.GetSubject = "";
        $('.note-editable').text('');
        $('#selectToken').val("Select");
        $scope.MailModel = {};
        //$scope.EmailTemplate = {};
        $("#FromTo").val(0);
        $("#templates1").val(0);
        $("#AttachedList").empty();
        $("#Physician").empty();
        $("#CaseManager").empty();
        $("#Relation").empty();
        $("#Relative").empty();
    }

};

controllers.ReferralDocumentController.$inject = ['$scope', '$http', '$window', '$timeout', '$filter'];

$(document).ready(function () {

    //ShowPageLoadMessage("ShowAddReferralMessage");
    var eventMethod = window.addEventListener ? "addEventListener" : "attachEvent";
    var eventer = window[eventMethod];
    var messageEvent = eventMethod == "attachEvent" ? "onmessage" : "message";
    // Listen to message from child window
    eventer(messageEvent, function (e) {

        //console.log('parent received message!:  ', e.data);
        if (e.data && typeof (e.data) === 'string') {
            if (e.data.indexOf('ClearFrameCompleted:true') >= 0) {
                var _frame = document.getElementById('myIframe');
                _frame.src = "";
            }
            else if (e.data.indexOf('OrbeonID') >= 0) {
                ReferralDocumentVM.ReceiverOrbeonForm(e.data);
            }
            else if (ValideElement(e.data)) {
                //var res = JSON.parse(e.data);
                var res = JSON.parse(JSON.stringify(e.data));
                if (e.data.indexOf('OrginalFormId') > 0 && res.PageID == "ReferralDocument")
                    ReferralDocumentVM.SavedNewHtmlFormWithSubsection(e.data);

                if (res.OrgPageID == "ReferralDocument") {
                    //ReferralDocumentVM.ClosePreview();
                    //ReferralDocumentVM.GetReferralDocumentList();
                    ReferralDocumentVM.SaveDocumentFormName(res.PrimaryID);
                }
            }
        }


    }, false);

    $("#EmailAttachment").change(function () {
        var fileUpload = $("#EmailAttachment").get(0);
        var files = fileUpload.files;
        // Create FormData object
        var fileData = new FormData();

        for (var i = 0; i < files.length; i++) {
            $("#AttachedList").append('<span style="padding-right: 8px ;font-size: 13px"><i class="fa fa-image" style="font-size: 17px; margin-right:3px"></i>' + files[i].name + ' </span>');
        }
        // Looping over all files and add it to FormData object
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }
        $.ajax({
            url: '/hc/Referral/UploadFiles',
            type: "POST",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            data: fileData,
            async: false,
            success: function (result) {
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    });
});
$(function () {
    $('.sendEmail').on('hidden.bs.modal', function () {
        $(this).removeData('bs.sendEmail');
    });
});
$(document).on('click', 'a.Physician', function () {

    var x = this.id;
    var cur_val = $('#tagsinputTo').val();
    if (cur_val)
        $('#tagsinputTo').val(cur_val + "," + x);
    else
        $('#tagsinputTo').val(x);
    $('#ReferralContact').modal('hide');
});

//$('.all-mail').append('<span class="email-ids">hsdddsdi <span class="cancel-email">x</span></span>');
$(".enter-mail-id").keydown(function (e) {
    if (e.keyCode == 13 || e.keyCode == 32) {
        //alert('You Press enter');
        var getValue = $(this).val();
        $('.all-mail').append('<span class="email-ids">' + getValue + ' <span class="cancel-email">x</span></span>');
        $(this).val('');
    }
});

$(document).on('click', '.cancel-email', function () {

    $(this).parent().remove();

});

//function to validate comma separated email address
function IsValidEmail(email) {
    var regex = /^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[,]{0,1}\s*)+$/;
    if (!regex.test(email)) {
        return false;
    } else {
        return true;
    }
}
  // $('.enter-mail-id').click()
