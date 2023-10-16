var vm;

controllers.EmployeeListController = function ($scope, $http, $timeout) {
    vm = $scope;
    $scope.AddEmployeeURL = HomeCareSiteUrl.AddEmployeeURL;
    
    
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetEmployeeListPage").val());
    };
    $scope.ETSModel = $.parseJSON($("#hdnETSModel").val());
    $scope.EmployeeList = [];
    $scope.SelectedEmployeeIds = [];
    $scope.SelectedEmployeeEmailIds = [];
    $scope.SelectAllCheckbox = false;

    $scope.EmployeeModel = $.parseJSON($("#hdnSetEmployeeListPage").val());
    //$scope.EmployeeModel.SearchEmployeeModel.Address = "50133,50132";
    $scope.SearchEmployeeModel = $scope.EmployeeModel.SearchEmployeeModel;
    $scope.TempSearchEmployeeModel = $scope.EmployeeModel.SearchEmployeeModel;
    $scope.EmployeeListPager = new PagerModule("Name");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchEmployeeModel: $scope.SearchEmployeeModel,
            pageSize: $scope.EmployeeListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.EmployeeListPager.sortIndex,
            sortDirection: $scope.EmployeeListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.LoggedInUserId = window.LoggedInID;

    $scope.SearchModelMapping = function () {
        
        $scope.SearchEmployeeModel.Address = $('#multipleSelect1').val();
        $scope.SearchEmployeeModel = $.parseJSON(angular.toJson($scope.TempSearchEmployeeModel));
        //$scope.SearchEmployeeModel.Name = $scope.TempSearchDepartmentModel.Name;
        //$scope.SearchEmployeeModel.Email = $scope.TempSearchDepartmentModel.Email;
        //$scope.SearchEmployeeModel.DepartmentID = $scope.TempSearchDepartmentModel.DepartmentID;
        //$scope.SearchEmployeeModel.IsSupervisor = $scope.TempSearchDepartmentModel.IsSupervisor;
        //$scope.SearchEmployeeModel.ListOfIdsInCSV = $scope.TempSearchDepartmentModel.ListOfIdsInCSV;

    };

    $scope.GetEmployeeList = function (isSearchDataMappingRequire) {

       
      
        //Reset Selcted Checkbox items and Control
        $scope.SelectedEmployeeIds = [];
        $scope.SelectAllCheckbox = false;
        //Reset Selcted Checkbox items and Control

        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping
        //$scope.EmployeeListPager.Address = "50132,50133";
        var jsonData = $scope.SetPostData($scope.EmployeeListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.GetEmployeeListURL, jsonData, "Post", "json", "application/json").success(function (response) {
           
            if (response.IsSuccess) {
                $scope.EmployeeList = response.Data.Items;
                $scope.EmployeeListPager.currentPageSize = response.Data.Items.length;
                $scope.EmployeeListPager.totalRecords = response.Data.TotalItems;
            }
            ShowMessages(response);
        });
    };

    $scope.SendBulkRegistrationEmail = function () {

        if ($scope.SelectedEmployeeIds.toString() === "") {
            ShowMessage("Please select employee to send email", "warning");
            return;
        }

        var model = {
            empIds: $scope.SelectedEmployeeIds.toString()
        };
        var jsonData = angular.toJson(model);
            AngularAjaxCall($http, "/hc/employee/SendRegistration", jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                ShowMessage(response.Message, "Success");
            }
        });
    };

    GetEmployeeGroupList()
   function GetEmployeeGroupList() {
        $scope.GroupList = {};
        AngularAjaxCall($http, "/hc/employee/GetEmployeeGroupList", "", "Get", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.GroupList = response.Data;
                
            }
        });
    };


    $scope.BulkEmployeeGroupUpdate = function () {
        $scope.getids = $('#multipleSelect').val();

        if ($scope.SelectedEmployeeIds.toString() === "") {
            ShowMessage("Please select employee to update group", "warning");
            return;
        }
        if ($scope.getids.toString() === "") {
            ShowMessage("Please select group", "warning");
            return;
        }

        var model = {
            empIds: $scope.SelectedEmployeeIds.toString(),
            groupId: $scope.getids
        };
        var jsonData = angular.toJson(model);
        AngularAjaxCall($http, "/hc/employee/UpdateBulkEmployeeGroup", jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
               
                ShowMessage(response.Message, "Success");
                $('#BulkGroupModel').modal('hide');
            }
        });
    };



    $scope.Refresh = function () {
        $scope.EmployeeListPager.getDataCallback();
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            $scope.SearchEmployeeModel = $scope.newInstance().SearchEmployeeModel;
            $scope.TempSearchEmployeeModel = $scope.newInstance().SearchEmployeeModel;
            $scope.SelectedGroups = null;
            $scope.TempSearchEmployeeModel.IsDeleted = "0";
            //$scope.SearchEmployeeModel.IsSupervisor = "-1";            
            //$scope.TempSearchEmployeeModel.IsSupervisor = "-1";

            $scope.EmployeeListPager.currentPage = 1;
            $scope.EmployeeListPager.getDataCallback();
        });
        $('#fixedAside').modal('hide');
    };

    $scope.SearchEmployee = function () {
        $scope.EmployeeListPager.currentPage = 1;
        $scope.EmployeeListPager.getDataCallback(true);
        $('#fixedAside').modal('hide');
    };

    // This executes when select single checkbox selected in table.
    $scope.SelectEmployee = function (employee) {
        if (employee.IsChecked) {
            $scope.SelectedEmployeeIds.push(employee.EmployeeID);
            $scope.SelectedEmployeeEmailIds.push(employee.Email);
        }
        else {
            $scope.SelectedEmployeeIds.remove(employee.EmployeeID);
            $scope.SelectedEmployeeEmailIds.remove(employee.Email);
        }

        if ($scope.SelectedEmployeeIds.length == $scope.EmployeeListPager.currentPageSize)
            $scope.SelectAllCheckbox = true;
        else
            $scope.SelectAllCheckbox = false;

    };

    // This executes when select all checkbox in table header is checked.
    $scope.SelectAll = function () {
        $scope.SelectedEmployeeIds = [];

        angular.forEach($scope.EmployeeList, function (item, key) {

            item.IsChecked = $scope.SelectAllCheckbox;// event.target.checked;
            if (item.IsChecked) {
                $scope.SelectedEmployeeIds.push(item.EmployeeID);
                $scope.SelectedEmployeeEmailIds.push(item.Email);
            }
            else {
                $scope.SelectedEmployeeIds.remove(item.EmployeeID);
                $scope.SelectedEmployeeEmailIds.remove(item.Email);
            }
        });

        return true;
    };

    $scope.DeleteEmployee = function (employeeId, title) {
        if (title == undefined) {
            title = window.UpdateRecords;
        }
        bootboxDialog(function (result) {
            if (result) {
                $scope.SearchEmployeeModel.ListOfIdsInCsv = employeeId > 0 ? employeeId.toString() : $scope.SelectedEmployeeIds.toString();

                if (employeeId > 0) {
                    if ($scope.EmployeeListPager.currentPage != 1)
                        $scope.EmployeeListPager.currentPage = $scope.EmployeeList.length === 1 ? $scope.EmployeeListPager.currentPage - 1 : $scope.EmployeeListPager.currentPage;
                } else {

                    if ($scope.EmployeeListPager.currentPage != 1 && $scope.SelectedEmployeeIds.length == $scope.EmployeeListPager.currentPageSize)
                        $scope.EmployeeListPager.currentPage = $scope.EmployeeListPager.currentPage - 1;
                }

                //Reset Selcted Checkbox items and Control
                $scope.SelectedEmployeeIds = [];
                $scope.SelectAllCheckbox = false;
                //Reset Selcted Checkbox items and Control

                var jsonData = $scope.SetPostData($scope.EmployeeListPager.currentPage);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteEmployeeURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    if (response.IsSuccess) {
                        $scope.EmployeeList = response.Data.Items;
                        $scope.EmployeeListPager.currentPageSize = response.Data.Items.length;
                        $scope.EmployeeListPager.totalRecords = response.Data.TotalItems;
                        $scope.Refresh();
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, title, window.EnableDisableConfirmationMessage +' '+ window.EmployeeScheduleDelete, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };
    
    $scope.EmployeeListPager.getDataCallback = $scope.GetEmployeeList;
    $scope.EmployeeListPager.getDataCallback();
    $scope.EmployeeEditModel = function (EncryptedEmployeeID, title)
    {
        var EncryptedEmployeeID = EncryptedEmployeeID;
        $('#fixedAside').modal({ backdrop: 'static', keyboard: false });
        $('#fixedAsidelDDLBindIFrame').attr('src', HomeCareSiteUrl.PartialAddEmployeeURL + EncryptedEmployeeID);
    }
    $scope.EmployeeEditModelClosed = function () {
        $scope.Refresh();
        $('#fixedAside').modal('hide');
    }
    $scope.MailModel = {};
    $scope.BulkScheduleModel = function () {
        if ($scope.BulkSchedule == "BulkSchedule") {
            if (ValideElement($scope.SelectedEmployeeIds.toString())) {
                $('#BulkScheduleModel').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
        }
        if ($scope.BulkSchedule == "SendBulkEmail")
        {
            $scope.SendBulkEmail();
        }
        if ($scope.BulkSchedule == "SendBulkRegistrationEmail")
        {
            $scope.SendBulkRegistrationEmail();
        }
        else {
            toastr.error("Please select atleast one employee.");
        }
        $scope.BulkSchedule = '';
    }
     //Bulk Email Send 
    
    $scope.SendBulkEmail = function (employee, title) {
        if ($scope.SelectedEmployeeIds.toString() === "") {
            ShowMessage("Please select employee to send email", "warning");
            return;
        }
        $scope.ListOfEmailsInCsv =  $scope.SelectedEmployeeEmailIds.toString();
        scopeEmpRefEmail.ValueAssignToMailModel($scope.ListOfEmailsInCsv);
        $('#SendBulkEmailModel').modal({
            backdrop: 'static',
            keyboard: false
        });
    }

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
        $scope.ReferralID = 4;
        var jsonData = angular.toJson({ name: $scope.EmailTemplate, ReferralID: $scope.ReferralID });
        AngularAjaxCall($http, "/hc/referral/GetTemplateDetails", jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.GetSubject = response.Data.EmailTemplateSubject;
                $('.panel-body').html(response.Data.EmailTemplateBody);
            }
            ShowMessages(response);
        });
    };

};
$('#btncancel').click(function () {
    $('#BulkGroupModel').modal('hide');
});


controllers.EmployeeListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    //ShowPageLoadMessage("ShowEmployeeMessage");
});

$(document).ready(function () {

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