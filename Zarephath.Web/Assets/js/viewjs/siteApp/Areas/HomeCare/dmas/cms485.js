var custModel;
controllers.Cms485Controller = function ($scope, $http, $timeout) {
    custModel = $scope;
    $scope.EncryptedReferralID = window.EncryptedReferralID;
    $scope.IsShow = true;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnCms485").val());
    };

    $scope.GetCms485Model = $.parseJSON($("#hdnCms485").val());

    $scope.Cms485List = [];

    $scope.GetCms485List = function () {
        var jsonData = angular.toJson({ id: $scope.EncryptedReferralID });
        AngularAjaxCall($http, HomeCareSiteUrl.Cms485ListURL, jsonData, "Post", "json", "application/json", true).
            success(function (response) {
                $scope.Cms485List = response.Cms485FormList
            });
    }
  
    $scope.AddCms485 = function (item) {
    $scope.IsShow = false;
        if (item == undefined) {
            $scope.Cms485ID = 0;
        }
        else {
            $scope.Cms485ID = item.Cms485ID;
        }
        $scope.EncryptedReferralID = window.EncryptedReferralID;
        var jsonData = angular.toJson({ Cms485ID: $scope.Cms485ID, EncryptedReferralID: $scope.EncryptedReferralID });
        AngularAjaxCall($http, HomeCareSiteUrl.Cms485AddURL, jsonData, "Post", "json", "application/json", true).
            success(function (response) {
                if (response.IsSuccess) {
                    $scope.GetCms485Model = response.Data;
                    $scope.IsShow = false;
                } else {
                    ShowMessages(response);
                }
            });
    };

    $scope.SaveCms485 = function () {
        var isValid = CheckErrors($("#briggsForm"));
        $scope.GetCms485Model.Cms485Model = $scope.GetCms485Model.Cms485Model || {}
        $scope.GetCms485Model.Cms485Model.NurseSignOfVerbalSOC = document.getElementById("newSignature").toDataURL("image/png");
        $scope.GetCms485Model.Cms485Model.AttendingPhysicianSign = document.getElementById("newSignature1").toDataURL("image/png");
        $scope.GetCms485Model.Cms485Model.EncryptedReferralID = $scope.EncryptedReferralID;
        if (isValid) {
            var jsonData = angular.toJson({ cms: $scope.GetCms485Model });
            AngularAjaxCall($http, HomeCareSiteUrl.Cms485SaveURL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        toastr.success("CMS-485 Form Save Successfully");
                        $scope.IsShow = true;
                        $scope.GetCms485List();
                    } else {
                        ShowMessages(response);
                    }
                });
        }
    };

    $scope.DeleteCms485 = function (item, title) {
        if (title == undefined) {
            title = "Delete Record";
        }
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = angular.toJson(item);
                AngularAjaxCall($http, HomeCareSiteUrl.Cms485DeleteURL, jsonData, "post", "json", "application/json", true).
                    success(function (response) {
                        if (response.IsSuccess) {
                            toastr.success("CMS-485 Form Deleted Successfully");
                            $scope.GetCms485List();
                        }
                        ShowMessages(response);
                    });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);

    }

    $scope.Clone = function (item) {
        item.EncryptedReferralID = window.EncryptedReferralID;
        var jsonData = angular.toJson(item);
        AngularAjaxCall($http, HomeCareSiteUrl.Cms485CloneURL, jsonData, "Post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.GetCms485List();
                } else {
                    ShowMessages(response);
                }
            });

    }

    $scope.callBack = function (item) {
        $('#startcaredate').data("DateTimePicker").widgetPositioning({
            horizontal: "right",
            vertical: "top"
        });

        $('#attendingdate').data("DateTimePicker").widgetPositioning({
            horizontal: "right",
            vertical: "top"
        });
    };

    $scope.Refresh = function () {
        $scope.GetCms485List();
    };

    $scope.Cancel = function (item) {
        $scope.IsShow = true;
    }

    $("a#cms-485").on('shown.bs.tab', function (e) {
        $scope.GetCms485List();
        $scope.IsShow = true;
    });

};

controllers.Cms485Controller.$inject = ['$scope', '$http', '$timeout'];

