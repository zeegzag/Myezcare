var checklistModel;
controllers.ReferralCheckListController = function ($scope, $http, $window, $timeout) {
  
    checklistModel = $scope;

    $scope.EncryptedReferralID = window.EncryptedReferralID; //"iSNqtcWbe3gZEhtctmlPcA2";

    $scope.SetReferralCheckListModel = $.parseJSON($("#hdnSetReferralCheckListModel").val());

    $scope.ReferralCheckList = $scope.SetReferralCheckListModel.ReferralCheckList;
    

    $scope.ReferralPostDataModel = function () {
        return angular.toJson({ EncryptedReferralID: $scope.EncryptedReferralID });
    };

    $scope.SetReferralCheckListDetail = function () {
        var jsonData = angular.toJson({ EncryptedReferralID: $scope.EncryptedReferralID });
        AngularAjaxCall($http, SiteUrl.SetReferralCheckListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralCheckList = response.Data.ReferralCheckList;
                $scope.ReferralDetail = response.Data.ReferralDetailModel;
                $scope.DxCodeMappingList = response.Data.DxCodeMappingList;

                if ($scope.DxCodeMappingList.length > 0) {
                    $.each($scope.DxCodeMappingList, function (i, dxcode) {
                        $scope.TempSelectedDXCodeIDs.push(dxcode.DXCodeID);
                    });
                }


                $scope.YesNoList = response.Data.YesNoList;
                $scope.LoadCMDetails();
                $scope.PrintContent = false;
            }
            ShowMessages(response);
        });
    };

    $scope.SaveReferralCheckList = function () {
        var jsonData = angular.toJson({ referralCheckList: $scope.ReferralCheckList, dxCodeMappingList: $scope.DxCodeMappingList, EncryptedReferralID: $scope.EncryptedReferralID });
        if (CheckErrors("#frmReferralCheckList")) {
            HideErrors("#frmReferralCheckList");
            $("#frmReferral").data('changed', false);
            AngularAjaxCall($http, SiteUrl.SaveReferralCheckListURL, jsonData, "Post", "json", "application/json").success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    //$scope.ReferralCheckList = response.Data;
                    $scope.ReferralCheckList = response.Data.ReferralCheckList;
                    $scope.ReferralDetail = response.Data.ReferralDetailModel;
                    $scope.DxCodeMappingList = response.Data.DxCodeMappingList;

                    if ($scope.DxCodeMappingList.length > 0) {
                        $.each($scope.DxCodeMappingList, function (i, dxcode) {
                            $scope.TempSelectedDXCodeIDs.push(dxcode.DXCodeID);
                        });
                    }


                    $scope.YesNoList = response.Data.YesNoList;
                    $scope.LoadCMDetails();
                    $scope.PrintContent = false;
                }

            });
        }
    };

    $scope.ResetReferralCheckList = function () {
        var tempData = {
            CreatedDate: $scope.ReferralCheckList.CreatedDate,
            CreatedBy: $scope.ReferralCheckList.CreatedBy,
            UpdatedDate: $scope.ReferralCheckList.CreatedDate,
            UpdatedBy: $scope.ReferralCheckList.UpdatedBy,
            ReferralSparFormID: $scope.ReferralCheckList.ReferralCheckListID,

            AgencyID: $scope.ReferralCheckList.AgencyID,
            CaseManagerID: $scope.ReferralCheckList.CaseManagerID,
            ReferringAgency: $scope.ReferralCheckList.ReferringAgency,
            FacilatorInformation: $scope.ReferralCheckList.FacilatorInformation,
        };
        $scope.ReferralCheckList = $.parseJSON($("#hdnSetReferralCheckListModel").val()).ReferralCheckList;
        $scope.ReferralCheckList.CreatedDate = tempData.CreatedDate;
        $scope.ReferralCheckList.CreatedBy = tempData.CreatedBy;
        $scope.ReferralCheckList.UpdatedDate = tempData.UpdatedDate;
        $scope.ReferralCheckList.UpdatedBy = tempData.UpdatedBy;
        $scope.ReferralCheckList.ReferralCheckListID = tempData.ReferralSparFormID;

        $scope.ReferralCheckList.AgencyID = tempData.AgencyID;;
        $scope.ReferralCheckList.CaseManagerID = tempData.CaseManagerID;;
        $scope.ReferralCheckList.ReferringAgency = tempData.ReferringAgency;;
        $scope.ReferralCheckList.FacilatorInformation = tempData.FacilatorInformation;;
    };
    
    $scope.$watch(function () {
        return $scope.ReferralCheckList.IsCheckListOffline;
    }, function (newValue) {        
        $scope.ReferralCheckList.IsCheckListCompleted = newValue;
        if (newValue) {
            $(".content-checklist :input:not(.prevent-enable)").attr("disabled", true);
            $(".content-checklist .dis_autocompleter").addClass("disabledContent");
            $(".content-checklist").addClass("form-disable");

        } else {
            $(".content-checklist :input:not(.prevent-enable)").attr("disabled", false);
            $(".content-checklist .dis_autocompleter").removeClass("disabledContent");
            $(".content-checklist").removeClass("form-disable");
        }
    });

    $("a#checklist").on('shown.bs.tab', function (e) {
        $("#frmReferral").data('changed', false);
        $scope.SetReferralCheckListDetail();
    });
    
    $scope.PrintDiv = function (id) {
        myApp.showPleaseWait();
        $scope.PrintContent = true;
        setTimeout(function () {
            printDiv($("#" + id));
            myApp.hidePleaseWait();
            $scope.PrintContent = false;
            $scope.$apply();
        }, 500);
    };



    
    //#region CaseManager Related Code
    $scope.CL_CaseManagerTokenObj = {};
    $scope.LoadCMDetails = function () {
        if ($scope.ReferralCheckList.CaseManagerID > 0) {

            var agencyId= $scope.ReferralCheckList.AgencyID ;
            var caseManagerId=$scope.ReferralCheckList.CaseManagerID ;
            var agencyName = $scope.ReferralCheckList.ReferringAgency;
            var cmName = $scope.ReferralCheckList.FacilatorInformation;

            $scope.CL_CaseManagerTokenObj.clear();

            $scope.ReferralCheckList.AgencyID = agencyId;
            $scope.ReferralCheckList.CaseManagerID = caseManagerId;
            $scope.ReferralCheckList.ReferringAgency = agencyName;
            $scope.ReferralCheckList.FacilatorInformation = cmName;

            $scope.CL_CaseManagerTokenObj.add({
                AgencyID: $scope.ReferralCheckList.AgencyID, CaseManagerID: $scope.ReferralCheckList.CaseManagerID,
                Name: $scope.ReferralCheckList.FacilatorInformation, NickName: $scope.ReferralCheckList.ReferringAgency
            });
            if (!$scope.$root.$$phase) {
                $scope.$apply();
            }
        }
    };

    $scope.GetCaseManagersURL = SiteUrl.GetCaseManagersURL;
    $scope.CL_CaseManagerResultsFormatter = function (item) {
        return "<li id='{0}'>{0}<span class='badge badge-primary' style='float:right;'>{1}</span><br/><small><b>CM: </b>{2}</small><br/><small><b style='color:#ad0303;'>{5}: </b>{3}</small><small><b style='color:#ad0303;padding-left:10px;'>{6}: </b>{4} </small></li>"
            .format(
            item.NickName,
            item.RegionName,
            item.Name,
            item.Phone ? item.Phone : window.NA,
            item.Email ? item.Email : window.NA,
            window.Phone,
            window.Email);
    };
    $scope.CL_CaseManagerTokenFormatter = function (item) {
        return "<li id='{0}'><b>CM:</b> {0} <b>Agency: </b> ({1})</li>".format(item.Name, item.NickName);
    };
    $scope.CL_RemoveCaseManager = function () {
        $scope.ReferralCheckList.AgencyID = null;
        $scope.ReferralCheckList.CaseManagerID = null;
        $scope.ReferralCheckList.ReferringAgency = null;
        $scope.ReferralCheckList.FacilatorInformation = null;
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };
    $scope.CL_CaseManagerAdded = function (item) {
        $scope.ReferralCheckList.AgencyID = item.AgencyID;
        $scope.ReferralCheckList.CaseManagerID = item.CaseManagerID;

        $scope.ReferralCheckList.ReferringAgency = item.NickName;
        $scope.ReferralCheckList.FacilatorInformation = item.Name;

        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
    };

    //#endregion
    

    //#region DX CODE MAPPING RELATED FUNCATION START
    $scope.DxCodeMappingList = [];
    $scope.ReferralDXCodeMapping = {};
    $scope.DxCodeTokenInputObj = {};

    $scope.TokenInputObj = {};
    $scope.SelectedDXCodeIDs = [];
    $scope.TempSelectedDXCodeIDs = [];
    $scope.GetDXCodeListForAutoCompleteURL = SiteUrl.GetDXCodeListForAutoCompleteURL;

    $scope.OnDXCodeAdd = function (item, e) {
        $timeout(function () {
            $scope.ReferralDXCodeMapping.DXCodeID = item.DXCodeID;
            $scope.ReferralDXCodeMapping.DXCodeName = item.DXCodeName;
            $scope.ReferralDXCodeMapping.DXCodeWithoutDot = item.DXCodeWithoutDot;
            $scope.ReferralDXCodeMapping.Description = item.Description;
            $scope.ReferralDXCodeMapping.DxCodeShortName = item.DxCodeShortName;
            $scope.ReferralDXCodeMapping.IsDeleted = false;
        });
    };

    $scope.SaveReferralDXCodeMapping = function () {
        if ($scope.ReferralDXCodeMapping.DXCodeID && $scope.ReferralDXCodeMapping.StartDate && $scope.ReferralDXCodeMapping.Precedence) {
            var precedenceCount = $scope.DxCodeMappingList.filter(function (item) {
                return parseInt(item.Precedence) == parseInt($scope.ReferralDXCodeMapping.Precedence) && item.DXCodeID != $scope.ReferralDXCodeMapping.DXCodeID;
            }).length;
            if (precedenceCount > 0) {
                toastr.error(window.DXCodeWithPrecedenceExists);
            } else {
                var index = $.map($scope.DxCodeMappingList, function (obj, id) {
                    if (obj.DXCodeID == $scope.ReferralDXCodeMapping.DXCodeID) {
                        return id;
                    }
                    return null;
                });
                if (index.length > 0 && $scope.DxCodeMappingList[index].DXCodeID == $scope.ReferralDXCodeMapping.DXCodeID) {
                    $scope.DxCodeMappingList[index].StartDate = $scope.ReferralDXCodeMapping.StartDate;
                    $scope.DxCodeMappingList[index].EndDate = $scope.ReferralDXCodeMapping.EndDate;
                    $scope.DxCodeMappingList[index].Precedence = $scope.ReferralDXCodeMapping.Precedence;
                    $scope.DxCodeTokenInputObj.clear();
                    $scope.ReferralDXCodeMapping = {};
                } else {
                    $scope.DxCodeMappingList.push($scope.ReferralDXCodeMapping);
                    $scope.TempSelectedDXCodeIDs.push($scope.ReferralDXCodeMapping.DXCodeID);
                    $scope.DxCodeTokenInputObj.clear();
                    $scope.ValidateDxCode();
                    $scope.ReferralDXCodeMapping = {};
                }
                //}

            }
        } else {
            $scope.ListofFieldsForDxCode = "";
            var liString = "<li>{0} : {1}</li>";
            $scope.ListofFields = "<ul>";
            if (!$scope.ReferralDXCodeMapping.DXCodeID) {
                $scope.ListofFieldsForDxCode += liString.format(window.Missing, window.DXCode);
            }

            if (!$scope.ReferralDXCodeMapping.StartDate) {
                $scope.ListofFieldsForDxCode += liString.format(window.Missing, window.StartDate);
            }

            if (!$scope.ReferralDXCodeMapping.Precedence) {
                $scope.ListofFieldsForDxCode += liString.format(window.Missing, window.Precedence);
            }

            bootboxDialog(function () {
            }, bootboxDialogType.Alert, window.Alert, window.FieldsIncomplete.format($scope.ListofFieldsForDxCode));
        }
        //}
    };

    $scope.EditDxCodeFromMapping = function (data) {
        $scope.ReferralDXCodeMapping = angular.copy(data);
        $scope.DxCodeTokenInputObj.clear();
        $scope.DxCodeTokenInputObj.add({ DXCodeID: data.DXCodeID, DXCodeName: data.DXCodeName, readonly: true });
    };

    $scope.DeleteDxCodeFromMapping = function (tempObject, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {

            var btnText = result.currentTarget.textContent;
            var isSoftDelete = (btnText !== Delete);
            if (btnText !== Cancel) {

                var jsonData = angular.toJson({
                    isSoftDelte: isSoftDelete, ReferralDXCodeMappingID: tempObject.ReferralDXCodeMappingID, EncryptedReferralID: $scope.EncryptedReferralID,
                    IsEnable: (tempObject.IsDeleted)
                });

                if (tempObject.ReferralDXCodeMappingID > 0 && tempObject.ReferralDXCodeMappingID !== "" && tempObject.ReferralDXCodeMappingID !== undefined) {

                    AngularAjaxCall($http, SiteUrl.DeleteReferralDXCodeMappingURL, jsonData, "Post", "json", "application/json").success(function (response) {
                        //ShowMessages(response);
                        if (response.IsSuccess) {
                            if (isSoftDelete) {
                                tempObject.IsDeleted = !(tempObject.IsDeleted);
                                $timeout(function () {
                                    $scope.$apply();
                                });
                            } else {
                                $scope.DxCodeMappingList.remove(tempObject);
                                $scope.TempSelectedDXCodeIDs.remove(tempObject.DXCodeID);
                            }
                            if (!$scope.$root.$$phase) {
                                $scope.$apply();
                            }
                            $scope.ValidateDxCode();

                            $scope.SetReferralCheckListDetail();
                        }
                    });
                } else {
                    if (isSoftDelete) {
                        tempObject.IsDeleted = !(tempObject.IsDeleted);
                        //ShowToaster('success', (tempObject.IsDeleted) ? DxcodeDisabledSuccessMessage: DxcodeEnabledSuccessMessage);

                    } else {
                        $scope.DxCodeMappingList.remove(tempObject);
                        $scope.TempSelectedDXCodeIDs.remove(tempObject.DXCodeID);
                    }
                    $timeout(function () {
                        $scope.$apply();
                        $scope.ValidateDxCode();
                    });
                }
            }
        }, bootboxDialogType.Dialog, title, window.DeleteDxCodeConfirmationMessage, bootboxDialogButtonText.HardDelete, btnClass.BtnDanger, bootboxDialogButtonText.Cancel, undefined,
        (tempObject.IsDeleted) ? bootboxDialogButtonText.Enable : bootboxDialogButtonText.Disable, (tempObject.IsDeleted) ? btnClass.BtnEnable : btnClass.BtnDisable);
    };

    

    $scope.ValidateDxCode = function () {

        $scope.DXCodeCount = $scope.DxCodeMappingList.filter(function (item) {
            return item.IsDeleted == false;
        }).length;

        //if ($scope.DXCodeCount == 0)
        //    $scope.ReferralStatusChange($scope.ReferralModel.Referral.ReferralStatusID);

        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
        $('#DXCodeCount').valid();
    };

    $scope.isNotDeleted = function (x) {
        return x.IsDeleted == false;
    };

    $scope.OnDxCodeResultsFormatter = function (item) {
        return "<li id='{0}'><b>{3}: </b> {1} ({7})<span class='badge badge-primary' style='float:right;'>{8}</span><br/><b  style='color:#ad0303;'>{4}: </b>{2} </small></li>".
            format(item.DXCodeID, item.DXCodeWithoutDot ? item.DXCodeWithoutDot : 'NA', (item.Description) ? item.Description : 'NA', DXCode, Description, StartDate, EndDate, item.DXCodeName, item.DxCodeShortName);
    };

    //#endregion
};

controllers.ReferralCheckListController.$inject = ['$scope', '$http', '$window', '$timeout'];

