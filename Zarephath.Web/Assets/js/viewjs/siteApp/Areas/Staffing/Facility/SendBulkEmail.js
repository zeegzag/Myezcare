var scopeEmpRefEmail;

controllers.SendBulkEmailController = function ($scope, $http, $window, $timeout) {
    scopeEmpRefEmail = $scope;
    //alert("SendBulkEmailController");
    $scope.GetTemplateList = function () {
        AngularAjaxCall($http, "/hc/referral/GetTemplateList", "", "Get", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.TemplateList = response.Data;
                $scope.EmailTemplate = "0";
            }
            //ShowMessages(response);
        });
    };
    $scope.GetTemplateList();
    //$scope.getTokens = function () {
    //    ReferralID = $scope.Model.Referral.ReferralID == undefined ? 0 : $scope.Model.Referral.ReferralID;;
    //    var jsonData = angular.toJson({ refID: ReferralID });
    //    AngularAjaxCall($http, "/hc/referral/GetTokenList", jsonData, "Post", "json", "application/json").success(function (response) {
    //        if (response.IsSuccess) {
    //            $scope.TokenList = response.Data;
    //        }
    //    });
    //}
    $scope.MailModel = {};
    $scope.GetOrganizationSettings = function () {
        AngularAjaxCall($http, "/hc/referral/GetOrganizationSettings", "", "Get", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.OrganizationSettingList = response.Data;
                $scope.OrganizationSettings = "0";
            }
            //ShowMessages(response);
        });
    };
    $scope.GetOrganizationSettings();
    $scope.GetTemplateDetails = function () {
        $scope.ReferralID = {};
        $scope.ReferralID = 0;
        var jsonData = angular.toJson({ name: $scope.EmailTemplate, ReferralID: $scope.ReferralID });
        AngularAjaxCall($http, "/hc/referral/GetTemplateDetails", jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.GetSubject = response.Data.EmailTemplateSubject;
                $('.panel-body').html(response.Data.EmailTemplateBody);
            }
            ShowMessages(response);
        });
    };
    $scope.ValueAssignToMailModel = function (item) {
        $scope.ReferralEmailList = [];
        $scope.MailModel.To = item; //$("#tagsinputTo").val();
        $scope.MailModel.CC = $("#tagsinputCC").val();
        $scope.MailModel.From = $scope.OrganizationSettings;
        $scope.MailModel.Subject = $("#txtSubject").val();
        $scope.MailModel.Body = $('.note-editable').html();
        $scope.ReferralEmailList = item;

    }

    $scope.SendReferralAttachment = function (item) {
        $scope.MailModel.To = $scope.ReferralEmailList; //$scope.MailModel.To; 
        $scope.MailModel.CC = $("#tagsinputCC").val();
        $scope.MailModel.From = $scope.OrganizationSettings;
        $scope.MailModel.Subject = $("#txtSubject").val();
        $scope.MailModel.Body = $('.note-editable').html();
        var jsonData = angular.toJson($scope.MailModel);

        AngularAjaxCall($http, "/hc/referral/SendBulkEmail", jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {

                ShowMessage("Email sent successfully", "Success");

            }
            if (response.IsSuccess === false) {
                ShowMessage("Mail delivery failed", "error");

            }
            $scope.Reset();
            $('#SendBulkEmailModel').modal('hide');
        });
    };
    $scope.Reset = function () {
        $scope.GetSubject = "";
        $('.note-editable').text('');
        $('#selectToken').val("Select");
        $scope.MailModel = {};
        $("#FromTo").val(0);
        $("#templates1").val(0);
        $("#AttachedList").empty();
        $('#tagsinputTo').text('');
        
    }
    var count = 0;
    $scope.GetEmployeeEmail = function () {
        if (count === 0) {
            AngularAjaxCall($http, "/hc/referral/GetEmployeeEmail", "", "Get", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    count = count + 1;
                    $("#tagsinputCC").val(response.Message);
                }
            });
        }
        else {
            $("#tagsinputCC").val('');
            count = 0;
        }

    };
    
    
    $scope.ReferralEmailList = {};
    $scope.RefEmailList = [];
    $scope.GetReferralEmail = function (ReferralID) {
        $scope.ReferralEmailList = [];
        //$scope.RefEmailList = [];
        var jsonData = angular.toJson({ ReferralID: $scope.ListOfIdsInCsv });
        AngularAjaxCall($http, "/hc/referral/GetReferralEmail", jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.RefEmailList = response.Data;
                angular.forEach($scope.RefEmailList, function (item, key) {
                    if (item.Email != null)
                    {
                        $scope.ReferralEmailList.push(item.Email);
                    }
                    
                });
                $scope.ReferralEmailList = $scope.ReferralEmailList.toString();
            }
           
            $scope.ValueAssignToMailModel($scope.ReferralEmailList);
        });
       
    };
    $scope.physicanList = [];
    $scope.relative = [];
    $scope.assignee = {};
    $scope.casemanager = {};
    $scope.GetReferralAttachment = function (item) {
       
        if ($scope.ListOfIdsInCsv != undefined) {
            var jsonData = angular.toJson({ ReferralId: $scope.ListOfIdsInCsv });
            AngularAjaxCall($http, "/hc/referral/AddRecipient", jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.ReceipentList = response.Data.Receipent;
                    $scope.physicanList = response.Data.physicanList;
                    $scope.relative = response.Data.relative;
                    $scope.casemanager = response.Data.casemanager;
                    $scope.assignee = response.Data.assignee;
                    $('#AddRecipientModal').modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                }
                //ShowMessages(response);
            });
        }
        else
        {
            ShowMessage("No Record found", "error");
        }

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
    $scope.AddEmail = function (referral)
    {
        var x = $scope.Emails.toString();
        $('#tagsinputCC').val(x);
        $('#AddRecipientModal').modal('hide');
    }
};
  

controllers.SendBulkEmailController.$inject = ['$scope', '$http', '$window', '$timeout'];
$(document).ready(function () {

    $("#EmailAttachment").change(function () {
        var fileUpload = $("#EmailAttachment").get(0);
        var files = fileUpload.files;
        // Create FormData object
        var fileData = new FormData();

        //for (var i = 0; i < files.length; i++) {
        //    $("#AttachedList").append('<span style="padding-right: 8px ;font-size: 13px"><i class="fa fa-image" style="font-size: 17px; margin-right:3px"></i>' + files[i].name + ' </span>');
        //}
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
$(document).on('click', 'a.Physician', function () {
    var x = this.id;
    var cur_val = $('#tagsinputCC').val();
    if (cur_val)
        $('#tagsinputCC').val(cur_val + "," + x);
    else
        $('#tagsinputCC').val(x);
    $('#AddRecipientModal').modal('hide');
});