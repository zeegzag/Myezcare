var custModel;
controllers.Dmas97AbListController = function ($scope, $http, $timeout) {
    custModel = $scope;
    $scope.EncryptedReferralID = window.EncryptedReferralID;
    $scope.DmasModel = {};
    $scope.IsShow = true;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetDmas97AbListPage").val());
    };

    $scope.DmasModel = $.parseJSON($("#hdnDmas97AbModel").val());
    $scope.DMAS97ABList = [];

    $scope.GetDmas97AbList = function () {
        var jsonData = angular.toJson({ id:$scope.EncryptedReferralID});
        AngularAjaxCall($http, HomeCareSiteUrl.Dmas97AbList1URL, jsonData, "Post", "json", "application/json", true).
            success(function (response) {
                $scope.DMAS97ABList = response.DmasList

            });
    }

    $scope.Refresh = function () {
        $scope.GetDmas97AbList();
    };

    $scope.DeleteDmas97Ab = function (item, title) {
        if (title == undefined) {
            title = "Delete Record";
        }
        bootboxDialog(function (result) {
            if (result) {
                var jsonData = angular.toJson(item);
                AngularAjaxCall($http, HomeCareSiteUrl.Dmas97AbDeleteURL, jsonData, "post", "json", "application/json", true).
                    success(function (response) {
                        if (response.IsSuccess) {
                            toastr.success("DMAS-97A/B Form Deleted Successfully");
                            $scope.GetDmas97AbList();
                        }
                        ShowMessages(response);
                    });
            }
        }, bootboxDialogType.Confirm, title, window.DeleteConfirmationMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);

    }

    $scope.Clone = function (item) {
        item.EncryptedReferralID = window.EncryptedReferralID;
        var jsonData = angular.toJson(item);
        AngularAjaxCall($http, HomeCareSiteUrl.CloneDataDMAS97ABURL, jsonData, "Post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.GetDmas97AbList();
                } else {
                    ShowMessages(response);
                }
            });

    }

    $scope.callBack = function (item) {
        $('.dmas-97-s3').data("DateTimePicker").widgetPositioning({
            horizontal: "right",
            vertical: "bottom"
        }
        );
    };
   
    $scope.AddDmas97 = function (item) {
        if (item == undefined) {
            $scope.Dmas97ID = 0;
        }
        else {
            $scope.Dmas97ID = item.Dmas97ID;
        }
        $scope.EncryptedReferralID = window.EncryptedReferralID;
        
        var jsonData = angular.toJson({ Dmas97ID: $scope.Dmas97ID, EncryptedReferralID:$scope.EncryptedReferralID});
        AngularAjaxCall($http, HomeCareSiteUrl.AddDmas97URL, jsonData, "Post", "json", "application/json", true).
            success(function (response) {
                if (response.IsSuccess) {
                    $scope.DmasModel = response.Data;
                    $scope.IsShow = false;
                } else {
                    ShowMessages(response);
                }

            });
    };

    $scope.SaveDmas97 = function () {
        var isValid = CheckErrors($("#briggsForm"));
        $scope.DmasModel.Dmas97AbModel.Sign = document.getElementById("newSignature").toDataURL("image/png");
        $scope.DmasModel.Dmas97AbModel.RNSign = document.getElementById("newSignature1").toDataURL("image/png");
        $scope.DmasModel.Dmas97AbModel.EncryptedReferralID=$scope.EncryptedReferralID;

        if (isValid) {
            var jsonData = angular.toJson({ dmas: $scope.DmasModel });

            AngularAjaxCall($http, HomeCareSiteUrl.SaveDmas97URL, jsonData, "post", "json", "application/json", true).
                success(function (response, status, headers, config) {
                    if (response.IsSuccess) {
                        toastr.success("DMAS-97A/B Form Save Successfully");
                        $scope.IsShow = true;
                        $scope.GetDmas97AbList();
                    } else {
                        ShowMessages(response);
                    }
                });
        }
    };

    $scope.Cancel = function (item) {
        $scope.IsShow = true;
        $scope.DmasModel = null;
        $scope.GetDmas97AbList();
    }

    $("a#dmas-97").on('shown.bs.tab', function (e) {
        $scope.GetDmas97AbList();
        $scope.IsShow = true;
    });

    $("a#referralForm").on('shown.bs.tab', function (e) {
        $scope.GetDmas97AbList();
    });
};

controllers.Dmas97AbListController.$inject = ['$scope', '$http', '$timeout'];
$(document).ready(function () {
    ShowPageLoadMessage("ShowReleaseNoteMessage");
});
