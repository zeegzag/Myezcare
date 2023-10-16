var custModel;

controllers.ChangeServiceCodeController = function ($scope, $http, $timeout) {
    custModel = $scope;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnChangeServiceCodeModel").val());
    };
    $scope.IsFinalPage = false;
    $scope.CurrentDate = moment();
    $scope.ChnageServiceCodeModel = $.parseJSON($("#hdnChangeServiceCodeModel").val());

    $scope.NewGroupNote = $scope.newInstance().Note;
    $scope.SelectedNoteIds = [];
    $scope.SelectedNotes = [];
    $scope.SearchNoteModel = $scope.ChnageServiceCodeModel.SearchNote;



    $scope.SelectAllCheckbox = false;

    //#region Wizard
    $scope.currentStep = 1;
    $scope.steps = [
        {
            step: 1,
            name: window.FirstStep,
            desc: window.SearchNotes,
            isDone: false
        },
        {
            step: 2,
            name: window.SecondStep,
            desc: window.EnterNewServiceCodeDetail,
            isDone: false
        }
        //,
        //{
        //    step: 3,
        //    name: window.ThirdStep,
        //    desc: window.Complete,
        //    isDone: false
        //}
    ];
    $scope.user = {};

    $scope.$watch('currentStep', function (newValue) {
        if (parseInt(newValue) > 0) {
            if (newValue == 2 && $scope.SelectedNotes.length > 0) {
                $scope.steps[0].isDone = true;
            }
        }
    });

    $scope.$watch(function () {
        return $scope.SelectedNotes.length;
    }, function () {
        if ($scope.SelectedNotes.length == 0) {
            $scope.steps[1].isDone = false;
            //$scope.steps[2].isDone = false;
        }
    });

    //Functions
    // $scope.ChnageServiceCodeModel.GN_ServiceCodeTokenObj = {};
    $scope.gotoStep = function (newStep) {
        //var isValid = true;
        if (newStep == 1)
            $scope.currentStep = newStep;
        if (newStep == 2) {
            $scope.currentStep = newStep;
            $scope.SearchNoteModel.ValidateServiceCodePassed = 0;
        }
       


    };

    //#endregion
    

    //#region Step 1 Code

    $scope.SelectedNotes = [];
    $scope.SearchNotes = function () {

        var isValid = CheckErrors($("#searchCSC"), true);
        if (isValid) {
            var jsonData = {
                searchNote: $scope.SearchNoteModel,
                ignoreNoteID: $scope.SelectedNoteIds
            };
            $scope.GetGroupNoteListAjaxCall = true;
            AngularAjaxCall($http, SiteUrl.SearchNoteForChangeServiceCodeURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                if (response.IsSuccess) {
                    $scope.AllNotes = response.Data;
                    $scope.SelectedNotes = [];
                    $scope.SelectAllCheckbox = false;
                    //$scope.ChnageServiceCodeModel.Facilities = response.Data.Facilities;
                }
                $scope.GetGroupNoteListAjaxCall = false;
                ShowMessages(response);
            });
        }


    };
    $scope.SelectNote = function (note) {

        if (note.IsChecked) {
            $scope.SelectedNoteIds.push(note.NoteID);
            $scope.SelectedNotes.push(note);
        } else {
            $scope.SelectedNoteIds.remove(note.NoteID);
            $scope.SelectedNotes.remove(note);
        }
        if ($scope.SelectedNoteIds.length === $scope.AllNotes.length) {
            $scope.SelectAllCheckbox = true;
            if (!$scope.$root.$$phase) {
                $scope.$apply();
            }
        } else {
            $scope.SelectAllCheckbox = false;
        }
    };
    $scope.SelectAll = function (val) {
   
        $scope.SelectedNoteIds = [];
        $scope.SelectedNotes = [];
        angular.forEach($scope.AllNotes, function (item) {
            item.IsChecked = val; // event.target.checked;
            if (item.IsChecked) {
                $scope.SelectedNoteIds.push(item.NoteID);
                $scope.SelectedNotes.push(item);
            }
        });
        return true;
    };


    $scope.$watch(function () {
        return $scope.SearchNoteModel.NewServiceCodeID;
    }, function (newValue, oldValue) {
        if (newValue !== oldValue) $scope.SearchNoteModel.ValidateServiceCodePassed = 0;
    });

    $scope.$watch(function () {
        return $scope.SearchNoteModel.StartDate;
    }, function (newValue, oldValue) {
        $scope.SearchNoteModel.ServiceStartDate = $scope.SearchNoteModel.StartDate;
    });



    $scope.ValidateServiceCode = function () {
        var isValid = CheckErrors($("#searchCSC2"), true);
        if (isValid) {
            var jsonData = {
                searchNote: $scope.SearchNoteModel
            };
            AngularAjaxCall($http, SiteUrl.ValidateChangeServiceCodeURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.SelectedServiceCode = response.Data;
                }
                $scope.SearchNoteModel.ValidateServiceCodePassed = response.IsSuccess ? 1 : 2;
                $scope.SearchNoteModel.ValidateServiceCodeMsg = response.Message;
                ShowMessages(response);
            });
        }

    }

    $scope.ReplaceServiceCode = function () {
        var isValid = CheckErrors($("#searchCSC2"), true);
        if (isValid) {
            $scope.SearchNoteModel.PayorServiceCodeMappingID = $scope.SelectedServiceCode[0].PayorServiceCodeMappingID;
            var jsonData = {
                noteIds: $scope.SelectedNoteIds.toString(),
                searchNote: $scope.SearchNoteModel
            };
            AngularAjaxCall($http, SiteUrl.ReplaceServiceCodeURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.IsFinalPage = true;
                    $scope.GroupNoteMsg = response.Data;
                }
                ShowMessages(response);
            });
        }

    }
    

    //#endregion
};

controllers.ChangeServiceCodeController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {

    $(".dateInputMask").attr("placeholder", "mm/dd/yy");

});