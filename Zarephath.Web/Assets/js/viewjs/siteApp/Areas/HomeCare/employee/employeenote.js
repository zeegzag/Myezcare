controllers.EmployeeNoteController = function ($scope, $http, $window, $timeout) {    
    $scope.EmployeeNote = [];
    $scope.EmployeeNoteList = [];
    $scope.EmployeeNoteDetail = null;
    $scope.IsEdit = false;
    $scope.EmployeeModel = $.parseJSON($("#hdnEmployeeModel").val());
    

    $scope.SaveEmployeeNote = function () {                
        var isValid = CheckErrors($("#frmNewEmployeeNote"));
        if (isValid) {                                                
            var jsonData = angular.toJson({ EmployeeID: $scope.EmployeeModel.Employee.EmployeeID, NoteDetail: $scope.EmployeeNoteDetail, IsEdit: $scope.IsEdit, CommonNoteID: $scope.CommonNoteID });
            
            AngularAjaxCall($http, HomeCareSiteUrl.SaveEmployeeNoteURL, jsonData, "Post", "json", "application/json").success(function (response) {
                $scope.IsEdit = false;
                ShowMessages(response);
                if (response.IsSuccess) {
                    $("#AddEmployeeNoteModal").modal('hide');
                    $scope.GetEmployeeNotes();
                }
            });
        }

    };

    $scope.OnAddEmployeeClick = function () {
        $scope.EmployeeNoteDetail = null;
        $scope.IsEdit = false;
        $scope.CommonNoteID = null;
    }

    $scope.GetEmployeeNotes = function () {
        
        var jsonData = angular.toJson({ EmployeeID: $scope.EmployeeModel.Employee.EmployeeID });        
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeNotesURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.EmployeeNoteList = response.Data;
            }
        });
    };

    $scope.EditEmployeeNote = function (EmployeeNote) {

        $scope.EmployeeNoteDetail = EmployeeNote.Note;
        $scope.IsEdit = true;
        $scope.CommonNoteID = EmployeeNote.CommonNoteID;

    };

    $scope.DeleteEmployeeNote = function (EmployeeNote) {

        bootboxDialog(function (result) {
            if (result) {
                $scope.CommonNoteID = EmployeeNote.CommonNoteID;
                var jsonData = angular.toJson({ CommonNoteID: $scope.CommonNoteID });

                AngularAjaxCall($http, HomeCareSiteUrl.DeleteEmployeeNoteURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.GetEmployeeNotes();
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteNoteMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);

    };

    $("a#employeeNote").on('shown.bs.tab', function (e) {
        $scope.GetEmployeeNotes();
    });

};