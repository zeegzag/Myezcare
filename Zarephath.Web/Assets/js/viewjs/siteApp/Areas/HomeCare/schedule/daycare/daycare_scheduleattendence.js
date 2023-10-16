var custModel;



controllers.ScheduleDayCareAttendenceController = function ($scope, $http, $compile, $timeout, $filter) {
    custModel = $scope;

    $scope.NewInstance = function () {
        return $.parseJSON($("#hdnHC_DayCare_SetScheduleAttendenceModel").val());
    };

    $scope.FacilityList = $scope.NewInstance().FacilityList;
    $scope.SearchScheduledPatientModel = $scope.NewInstance().SearchScheduledPatientModel;
    $scope.TempSearchScheduledPatientModel = $scope.NewInstance().SearchScheduledPatientModel;

    $scope.Relations = [];
    $scope.SaveEditProfileURL = "/security/saveeditprofile";
    $scope.UploadFile = SiteUrl.CommonUploadFileUrl;

    $scope.GetScheduledPatientList = function (makefullScreen) {
        var jsonData = { model: $scope.TempSearchScheduledPatientModel };

        AngularAjaxCall($http, HomeCareSiteUrl.DayCare_GetScheduledPatientListURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
            if (response.IsSuccess) {

                $scope.ScheduledPatientList = response.Data;
            }

            ShowMessages(response);
        });
    };

    $scope.GetRelations = function () {
        var jsonData = { type: 33 }; //33=Relation Type
        AngularAjaxCall($http, HomeCareSiteUrl.DayCare_GetRelationTypeListURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
            if (response.IsSuccess) {
                $scope.Relations = response.Data;
            }
            ShowMessages(response);
        });
    };

    $scope.GetRelations();

    $scope.ChangeIsSelf = function () {
        //$scope.SelectedPatient.IsSelf = !$scope.SelectedPatient.IsSelf;
    };


    $scope.SearchClick = function () {
        $scope.GetScheduledPatientList(true);
    }

    $scope.Section_PatientList = "1";
    $scope.Section_TaskList = "2";
    $scope.Section_CheckIn_CheckOut = "3";
    $scope.Section_ViewAll = "4";


    $scope.CheckType_CheckIn = "1";
    $scope.CheckType_CheckOut = "2";
    $scope.PreSelectedPatient = "";
    $scope.ShowTaskAsPermission = function (section, selectedPatient) {
        $scope.PreSelectedPatient = selectedPatient;
        if (AttendanceSkipPatientTaskPermission) {

            bootboxDialog(function (result) {
                var isYes = $(result.currentTarget).hasClass('actionTrue');
                if (isYes) {
                    $scope.Show_Section($scope.Section_CheckIn_CheckOut, $scope.PreSelectedPatient );
                }
                else {
                    $scope.Show_Section($scope.Section_TaskList, $scope.PreSelectedPatient);
                }
                $scope.$apply(); 
            },

                bootboxDialogType.TwoActionOnly, "", window.SkipPatientTaskMessage, window.Yes, 'btn btn-primary actionTrue', '', '', window.No);
        }
        else {
            $scope.Show_Section($scope.Section_CheckIn_CheckOut, $scope.PreSelectedPatient );
        }
    }


    $scope.Show_Section = function (section, selectedPatient) {
        $scope.SelectedPatient = selectedPatient;
        console.log("section", section);
        console.log("selectedPatient", selectedPatient);
       

       
        if (section == $scope.Section_PatientList) {
            // First Section - Patient Scheduled Screen
            $scope.Section_PatientList_Visible = true;
            $scope.Section_TaskList_Visible = false;
            $scope.Section_CheckIn_CheckOut_Visible = false;

            $scope.SelectedPatientTaskList = [];
            $scope.SelectedPatient = {};
            $scope.SelectedReferralTaskMappingIDs = [];
            $scope.ShowTaskList = false;
        }
        else if (section == $scope.Section_TaskList) {
            // Second Section - Patient Task Selection Screen
            $scope.DayCareGetSchedulePatientTasks(true);
            $scope.Section_PatientList_Visible = false;
            $scope.Section_TaskList_Visible = true;
            $scope.Section_CheckIn_CheckOut_Visible = false;
            $scope.ShowTaskList = false;
        }
        else if (section == $scope.Section_CheckIn_CheckOut) {
            // Third Section - Patient Check In / Out & Signature Screen
            $scope.ClearSignaturePad();
            $scope.Section_PatientList_Visible = false;
            $scope.Section_TaskList_Visible = false;
            $scope.Section_CheckIn_CheckOut_Visible = true;


            if ($scope.SelectedPatient.PatientSignature_ClockIN) {
                $scope.SignaturePad_ClockIN.fromDataURL($scope.SelectedPatient.PatientSignature_ClockIN);
            }

            if ($scope.SelectedPatient.PatientSignature_ClockOut) {
                $scope.SignaturePad_ClockOut.fromDataURL($scope.SelectedPatient.PatientSignature_ClockOut);
            }

        }
        else if (section == $scope.Section_ViewAll) {

            $scope.SelectedPatientTaskList = [];
            $scope.SelectedReferralTaskMappingIDs = [];
            $scope.ShowTaskList = false;

            $scope.DayCareGetSchedulePatientTasks();
            $scope.ClearSignaturePad();
            $scope.Section_PatientList_Visible = false;
            $scope.Section_TaskList_Visible = false;
            $scope.Section_CheckIn_CheckOut_Visible = true;

            if ($scope.SelectedPatient.PatientSignature_ClockIN) {
                $scope.SignaturePad_ClockIN.fromDataURL($scope.SelectedPatient.PatientSignature_ClockIN);
            }

            if ($scope.SelectedPatient.PatientSignature_ClockOut) {
                $scope.SignaturePad_ClockOut.fromDataURL($scope.SelectedPatient.PatientSignature_ClockOut);
            }
        }
        $scope.SelectedPatient.StrClockInTime = $filter('date')(new Date(), 'hh:mm a');
        $scope.SelectedPatient.StrClockOutTime = $filter('date')(new Date(), 'hh:mm a');
        if ($scope.SelectedPatient.Name == null || $scope.SelectedPatient.Name == "") {
            $scope.SelectedPatient.IsSelf = true;
        }
        if ($scope.SelectedPatient.Relation != null && $scope.SelectedPatient.Relation != "") {
            $scope.SelectedPatient.Relation = Number($scope.SelectedPatient.Relation);
        }

        //$scope.SelectedPatient.IsSelf = true;
        //$scope.SelectedPatient.Name = '';
        //$scope.SelectedPatient.Relation = '';
        
        $scope.UploadProfileImage = HomeCareSiteUrl.UploadProfileImageUrls + '?referralID=' + $scope.SelectedPatient.ReferralID;
    };
    $scope.VisitTaskOptionList = [];
    $scope.DayCareGetSchedulePatientTasks = function (getPatientAlltask) {

        if (getPatientAlltask == true) {
            $scope.SelectedPatient.ScheduleID = 0;
        }

        var jsonData = { Daycare_GetScheduledPatientList: $scope.SelectedPatient };

        AngularAjaxCall($http, HomeCareSiteUrl.DayCare_GetSchedulePatientTasks, jsonData, "Post", "json", "application/json", true).success(function (response) {
            if (response.IsSuccess) {
                $scope.SelectedPatientTaskList = response.Data.DayCare_GetSchedulePatientTask;
                $scope.VisitTaskOptionList = response.Data.VisitTaskOptionList;
                $scope.SelectedPatient.ReferralTaskMappingIDs = "";
                $scope.SelectedReferralTaskMappingIDs = [];
            }
            ShowMessages(response);
        });

    };


    $scope.SelectedReferralTaskMappingIDs = [];
    $scope.schAttendenceTypeModalID = "";
    $scope.schAttendenceTypeModalVal = "";
    $scope.schAttendenceReferralTaskMappingID = "";
 
    $scope.SelectTask = function (selectedTask) {
        if (selectedTask.IsSelected) {
          //  $scope.schAttendenceTypeModalID = selectedTask.ReferralTaskMappingID;
          //  $scope.schAttendenceReferralTaskMappingID = selectedTask.ReferralTaskMappingID;
          ////  $('#schAttendenceTypeModal').modal({ backdrop: false, keyboard: false });

            //  $scope.SelectedReferralTaskMappingIDs.push(selectedTask.ReferralTaskMappingID);
            if (!$scope.VisitTaskOptionList.length>0) {
                $scope.SelectedReferralTaskMappingIDs.push(selectedTask.ReferralTaskMappingID);
            }
            else { }

           
        }
        else
            $scope.SelectedReferralTaskMappingIDs.remove(selectedTask.ReferralTaskMappingID);
      
    };
    $scope.SelectAttendanceTask = function (schAttendenceReferralTaskMappingID, schAttendenceTypeModalVal) {
        if (schAttendenceTypeModalVal!="") {
            $scope.SelectedReferralTaskMappingIDs.push(schAttendenceReferralTaskMappingID, schAttendenceTypeModalVal);
        }
        else
            $scope.SelectedReferralTaskMappingIDs.remove(schAttendenceReferralTaskMappingID, schAttendenceTypeModalVal);

        $scope.form = {};
        $('#frmschAttendenceTypeModal')[0].reset();
        $("#schAttendenceTypeModal").modal('hide');
        
    };
    $scope.TaskSubmit = function (selectedPatient) {
        $scope.SelectedPatient.ShowTaskError = false;
        if ($scope.SelectedReferralTaskMappingIDs.length == 0) {
            $scope.SelectedPatient.ShowTaskError = true;
            return false;
        }

        selectedPatient.ReferralTaskMappingIDs = $scope.SelectedReferralTaskMappingIDs.toString();
        $scope.ShowTaskList = true;
        $scope.Show_Section($scope.Section_CheckIn_CheckOut, selectedPatient);
    }
    $scope.SelectToption = function (item) {
        if (item != null) {
            $scope.SelectedReferralTaskMappingIDs.push(item.ReferralTaskMappingID, item.TaskOptionID);
        }
        else
            $scope.SelectedReferralTaskMappingIDs.remove(item.ReferralTaskMappingIDs, item.TaskOptionID);

        $scope.form = {};
    }

    $scope.CheckInOutSubmit = function (checktype) {
        $scope.SelectedPatient.ShowSignatureError_ClockIN = false;
        $scope.SelectedPatient.ShowSignatureError_ClockOut = false;


        if ($scope.SelectedPatient.IsClockInCompleted == false) {
            if ($scope.SignaturePad_ClockIN.isEmpty()) {
                $scope.SelectedPatient.ShowSignatureError_ClockIN = true;
                return false;
            }
        }


        if ($scope.SelectedPatient.IsClockInCompleted == true) {
            if ($scope.SignaturePad_ClockOut.isEmpty()) {
                $scope.SelectedPatient.ShowSignatureError_ClockOut = true;
                return false;
            }
        }



        var data = $scope.SignaturePad_ClockIN.toDataURL();
        if (data) {
            $scope.SelectedPatient.PatientSignature_ClockIN = data;
        }

        data = $scope.SignaturePad_ClockOut.toDataURL();
        if (data) {
            $scope.SelectedPatient.PatientSignature_ClockOut = data;
        }



        var jsonData = { Daycare_GetScheduledPatientList: $scope.SelectedPatient, SearchScheduledPatientModel: $scope.TempSearchScheduledPatientModel };

        AngularAjaxCall($http, HomeCareSiteUrl.Daycare_PatientClockInClockOut, jsonData, "Post", "json", "application/json", true).success(function (response) {
            if (response.IsSuccess) {
                $scope.Show_Section(1);
                $scope.GetScheduledPatientList();
            }
            ShowMessages(response);
        });

    }


    $scope.ClearSignaturePad = function () {
        $scope.SignaturePad_ClockIN.clear();
        $scope.SignaturePad_ClockOut.clear();

    }

    $scope.Show_Section($scope.Section_PatientList);




    $scope.PatientAttendanceActionModal = function (selectedPatient) {

        $scope.ScheduleAttendaceDetail = {};
        $scope.ScheduleAttendaceDetail.ScheduleID = selectedPatient.ScheduleID;
        $scope.ScheduleAttendaceDetail.ReferralID = selectedPatient.ReferralID;
        $scope.ScheduleAttendaceDetail.AbsentReason = selectedPatient.AbsentReason;
        $scope.ScheduleAttendaceDetail.IsPatientAttendedSchedule = !selectedPatient.IsPatientNotAttendedSchedule;



        $('#schAttendenceModal').modal({ backdrop: false, keyboard: false });
        $("#schAttendenceModal").modal('show');


    };




    $scope.SavePatientAttendance = function () {
        var isValid = CheckErrors($("#frmschAttendenceModal"));
        if (isValid) {
            var jsonData = angular.toJson($scope.ScheduleAttendaceDetail);
            AngularAjaxCall($http, HomeCareSiteUrl.DayCare_SavePatientAttendanceURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.ClosePatientAttendanceActionModal();
                    //$("#schAttendenceModal").modal('hide');
                }
                ShowMessages(response);
            });
        }
    };

    $scope.SavePatientTypeAttendance = function () {
        var isValid = CheckErrors($("#frmschAttendenceTypeModal"));
        if (isValid) {
            var jsonData = angular.toJson($scope.ScheduleAttendaceDetail);
            AngularAjaxCall($http, HomeCareSiteUrl.DayCare_SavePatientAttendanceURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.ClosePatientAttendanceActionModal();
                    //$("#schAttendenceModal").modal('hide');
                }
                ShowMessages(response);
            });
        }
    };

    $scope.ClosePatientAttendanceActionModal = function () {
        if (!$scope.$root.$$phase) {
            $scope.$apply();
        }
        $scope.GetScheduledPatientList();

        $("#schAttendenceModal").modal('hide');
        HideErrors("#frmschAttendenceModal");
    };
    $scope.ClosePatientAttendanceTypeActionModal = function () {
        $("#schAttendenceTypeModal").modal('hide');
    };

    $scope.ZoomInBtnShow = true;
    $scope.ZoomInOutClick = function (option) {
        if (option == "zoomin") {
            go_full_screen($(".page-content"));
            $scope.ZoomInBtnShow = false;
        }
        else {
            closeFullscreen();
            $scope.ZoomInBtnShow = true;
        }
    };



    /************______************/
    //Add profile Image start
    //$scope.UploadProfileImage = HomeCareSiteUrl.UploadProfileImageUrls + '?referralID=17';
    $scope.UploadingFileList = [];
    $scope.ProfileImageBeforeSend = function (e, data) {
        var isValidImage = true;
        var fileName;
        var errorMsg;

        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            if (extension !== "jpg" && extension !== "jpeg" && extension !== "png" && extension !== "bmp") {
                ShowMessage(window.InvalidImageUploadMessage, "error");
                isValidImage = false;
            }
            else if ((file.size / 1024) > 2048) {
                //file.FileProgress = 100;
                ShowMessage(window.MaximumUploadImageSizeMessage, "error");
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

    $scope.ProfileImageProgress = function (e, data) {
        console.log(data.files[0].name);
    };

    $scope.ProfileImageAfterSend = function (e, data) {
        $scope.IsFileUploading = false;
        var model = data.result;
        if (model.IsSuccess) {
            $scope.SelectedPatient.ProfileImagePath = model.Data.TempFilePath;
            $scope.UploadingFileList = [];
        } else {
            ShowMessage(model);
        }
        $scope.$apply();
        //window.location.reload();
    };

    //  Add profile imahe end

};

controllers.ScheduleDayCareAttendenceController.$inject = ['$scope', '$http', '$compile', '$timeout', '$filter'];


$(document).ready(function () {

    //var canvas = document.querySelector("canvas");
    var CheckInSignature = document.getElementById("CheckInSignature");
    var signaturePad = new SignaturePad(CheckInSignature);
    custModel.SignaturePad_ClockIN = signaturePad;


    var CheckOutSignature = document.getElementById("CheckOutSignature");
    signaturePad = new SignaturePad(CheckOutSignature);
    custModel.SignaturePad_ClockOut = signaturePad;


});
