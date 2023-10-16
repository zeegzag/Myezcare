var custModel;
controllers.Dmas99ListController = function ($scope, $http, $timeout) {
    custModel = $scope;
    $scope.EncryptedReferralID = window.EncryptedReferralID;
    $scope.IsShow = true;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetDmas99ListPage").val());
    };

    $scope.GetDmas99Model = $.parseJSON($("#hdnDmas99Model").val());
    
    $scope.Dmas99List = [];

    $scope.GetDmas99List = function () {
        var jsonData = angular.toJson({ id: $scope.EncryptedReferralID });
        AngularAjaxCall($http, HomeCareSiteUrl.Dmas99List1URL, jsonData, "Post", "json", "application/json", true).
            success(function (response) {
                $scope.Dmas99List = response.Dmas99FormList
            });
    }

    $scope.Refresh = function () {
        $scope.GetDmas99List();
    };

    $scope.DeleteDmas99 = function (item, title) {
        if (title == undefined) {
            title = "Delete Record";
        }
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = angular.toJson(item);
                AngularAjaxCall($http, HomeCareSiteUrl.Dmas99DeleteURL, jsonData, "post", "json", "application/json", true).
                    success(function (response) {
                        if (response.IsSuccess) {
                            toastr.success("DMAS-99 Form Deleted Successfully");
                            $scope.GetDmas99List();
                        }
                        ShowMessages(response);
                    });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);

    }

    $scope.Clone = function (item) {
        item.EncryptedReferralID = window.EncryptedReferralID;
        var jsonData = angular.toJson(item);
        AngularAjaxCall($http, HomeCareSiteUrl.CloneDataDMAS99URL, jsonData, "Post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.GetDmas99List();
                } else {
                    ShowMessages(response);
                }
            });

    }

    $scope.callBack = function (item) {
        $('#dmas99assessmentdate').data("DateTimePicker").widgetPositioning({
            horizontal: "right",
            vertical: "bottom"
        });
        $('#dmas99hospitalizationdate').data("DateTimePicker").widgetPositioning({
            horizontal: "right",
            vertical: "top"
        });

        $('#dmas-99-RnSignDate').data("DateTimePicker").widgetPositioning({
            horizontal: "right",
            vertical: "top"
        });
        $('#dmas-99-SignDate').data("DateTimePicker").widgetPositioning({
            horizontal: "right",
            vertical: "top"
        });
    };

    $scope.AddDmas99 = function (item) {
       
        if (item == undefined) {
            $scope.Dmas99ID = 0;
        }
        else {
            $scope.Dmas99ID = item.Dmas99ID;
        }
        $scope.EncryptedReferralID = window.EncryptedReferralID;
        var jsonData = angular.toJson({ Dmas99ID: $scope.Dmas99ID, EncryptedReferralID: $scope.EncryptedReferralID });
        AngularAjaxCall($http, HomeCareSiteUrl.AddDmas99URL, jsonData, "Post", "json", "application/json", true).
            success(function (response) {
                if (response.IsSuccess) {
                    $scope.GetDmas99Model = response.Data;
                    $scope.IsShow = false;
                } else {
                    ShowMessages(response);
                }

            });
    };

    $scope.SaveDmas99 = function () {
        var isValid = CheckErrors($("#briggsForm"));
        $scope.GetDmas99Model.Dmas99Model.Sign = document.getElementById("newSignature").toDataURL("image/png");
       $scope.GetDmas99Model.Dmas99Model.RnSign = document.getElementById("newSignature1").toDataURL("image/png");
        $scope.GetDmas99Model.Dmas99Model.EncryptedReferralID = $scope.EncryptedReferralID;
        if (isValid) {
            var jsonData = angular.toJson({ dmas: $scope.GetDmas99Model });
            AngularAjaxCall($http, HomeCareSiteUrl.SaveDmas99URL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        toastr.success("DMAS-99 Form Save Successfully");
                        $scope.IsShow = true;
                        $scope.GetDmas99List();
                    } else {
                        ShowMessages(response);
                    }
                });
        }
    };

    $scope.Cancel = function (item) {
        $scope.IsShow = true;
        $scope.Dmas99Model = null;
        $scope.GetDmas99List();
    }

    $("a#dmas-99").on('shown.bs.tab', function (e) {
        $scope.GetDmas99List();
        $scope.IsShow = true;
    });

    };


controllers.Dmas99ListController.$inject = ['$scope', '$http', '$timeout'];
$(document).ready(function () {
    ShowPageLoadMessage("ShowReleaseNoteMessage");
});