controllers.ReferralNoteController = function ($scope, $http, $window, $timeout) {
  
    $scope.EncryptedReferralID = window.EncryptedReferralID;
    $scope.EncryptedEmployeeID = ($("#hdnEmployeeModel").length > 0) ? ($.parseJSON($("#hdnEmployeeModel").val())).Employee.EncryptedEmployeeID : null;
    $scope.EmployeeID = ($("#hdnEmployeeModel").length > 0) ? ($.parseJSON($("#hdnEmployeeModel").val())).Employee.EmployeeID : null;


    $scope.ReferralNote = [];
    $scope.ReferralNoteList = [];
    
    $scope.ReferralNoteDetail = null;
    $scope.IsEdit = false;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetReferralNoteSentenceListModel").val());
    };

    $scope.SearchReferralNoteSentence = $scope.newInstance().SearchReferralNoteSentenceList;
    $scope.TempSearchNoteSentenceListPage = $scope.newInstance().SearchReferralNoteSentenceList;
    $scope.NoteSentenceListPager = new PagerModule("NoteSentenceName");
    $scope.GetNoteSentenceList = [];

    $scope.SaveReferralNote = function () {
       // alert($scope.selecteditem)

        var isValid = CheckErrors($("#frmNewReferralNote"));
        if (!$scope.SelectedEmployeesID) {
            $scope.SelectedEmployeesID = new Array();
            angular.forEach($scope.EmployeeList, function (item, i) {
                $scope.SelectedEmployeesID.push(item.EmployeeID);
            });
        }
        console.log($scope.SelectedEmployeesID);
        if (isValid) {
            $scope.EmployeeList.fore
            $scope.RoleID = ($scope.SelectedRoleID) ? $scope.SelectedRoleID.toString() : null;
            $scope.EmployeesID = ($scope.SelectedEmployeesID) ? $scope.SelectedEmployeesID.toString() : null;
            $scope.ReferralNote.EncryptedReferralID = $scope.EncryptedReferralID;
            $scope.ReferralNote.EncryptedEmployeeID = $scope.EncryptedEmployeeID;
            $scope.ReferralNote.NoteDetail = $scope.ReferralNoteDetail;
            //$scope.ReferralNote.catId = $scope.selecteditem
            
            var jsonData = angular.toJson({
                RoleID: $scope.RoleID,
                catId: $scope.selecteditem,
                EmployeesID: $scope.EmployeesID,
                EncryptedReferralID: $scope.EncryptedReferralID,
                EncryptedEmployeeID: $scope.EncryptedEmployeeID,
                NoteDetail: $scope.ReferralNoteDetail,
                IsEdit: $scope.IsEdit,
                CommonNoteID: $scope.CommonNoteID
              
            });
            AngularAjaxCall($http, HomeCareSiteUrl.SaveReferralNoteURL, jsonData, "Post", "json", "application/json").success(function (response) {
                $scope.IsEdit = false;
                ShowMessages(response);
                $("#NoteReq").hide();
                if (response.IsSuccess) {
                    $("#AddReferralNoteModal").modal('hide');
                    $scope.GetReferralNotes();
                    $scope.ResetRNote();
                }
            });
        }
        else {
            $("#NoteReq").show();
        }
    };
    $scope.Onclick = function () {
        $("#NoteReq").hide();
    }

    $scope.OnAddReferralClick = function () {
        $scope.ReferralNoteDetail = null;
        $scope.IsEdit = false;
        $scope.CommonNoteID = null;
    }

    $scope.GetReferralNotes = function () {
        var jsonData = angular.toJson({ EncryptedReferralID: $scope.EncryptedReferralID, EncryptedEmployeeID: $scope.EncryptedEmployeeID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralNotesURL, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            console.log(response.Data)
            if (response.IsSuccess) {
                $scope.ReferralNoteList = response.Data;
            }
        });
    };

    $scope.GetNoteSentenceList = function (isSearchDataMappingRequire) {
        //STEP 1:   Seach Model Mapping
        if (isSearchDataMappingRequire)
            $scope.SearchModelMapping();
        //STEP 1:   Seach Model Mapping
        var jsonData = $scope.SetPostData($scope.NoteSentenceListPager.currentPage);
        AngularAjaxCall($http, SiteUrl.GetNoteSentenceList, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.NoteSentenceList = response.Data.Items;
                $scope.NoteSentenceListPager.currentPageSize = response.Data.Items.length;
                $scope.NoteSentenceListPager.totalRecords = response.Data.TotalItems;
            }
        });
    };

    $scope.SearchModelMapping = function () {
         //$scope.SearchReferralPayorMappingList = $.parsejson(angular.tojson($scope.TempReferralPayorMappingList));
        $scope.SearchReferralNoteSentence = $.parseJSON(angular.toJson($scope.TempSearchNoteSentenceListPage));
    };

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            $scope.SearchReferralNoteSentence = $scope.newInstance().SearchReferralNoteSentenceList;
            $scope.TempSearchNoteSentenceListPage = $scope.newInstance().SearchReferralNoteSentenceList;
            $scope.NoteSentenceListPager.currentPage = 1;
            $scope.NoteSentenceListPager.getDataCallback();
        });
    };

    $scope.SearchNoteSentence = function () {
        $scope.NoteSentenceListPager.currentPage = 1;
        $scope.NoteSentenceListPager.getDataCallback(true);
    };

    $scope.SetPostData = function (fromIndex) {

        var pagermodel = {
            SearchNoteSentenceListPage: $scope.SearchReferralNoteSentence,
            pageSize: $scope.NoteSentenceListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.NoteSentenceListPager.sortIndex,
            sortDirection: $scope.NoteSentenceListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope._selected = [];

    $scope.SelectNoteSentence = function (NoteSentence, index) {
        NoteSentence.selected ? $scope._selected.push(NoteSentence) : $scope.ReferralNoteDetail
    }

    $scope.SelectNoteSentence = function (NoteSentence, checked) {

        if ($scope.ReferralNoteDetail == null) {
            $scope.ReferralNoteDetail = "";
        }
        if (checked == true) {
            var replacevalue1 = $scope.ReferralNoteDetail + "\n" + NoteSentence;
            $scope.ReferralNoteDetail = replacevalue1.replace(/(^[ \t]*\n)/gm, '');
            //$scope.ReferralNoteDetail = $scope.ReferralNoteDetail + " \n" + NoteSentence;
        }
        else {
            var replacevalue = $scope.ReferralNoteDetail.replace(NoteSentence, '').replace(/(^[ \t]*\n)/gm, '');
            $scope.ReferralNoteDetail = replacevalue;
        }

    };

    $scope.NoteSentenceListPager.getDataCallback = $scope.GetNoteSentenceList;

   

    $scope.EditReferralNote = function (referralNote) {
        $scope.ReferralNoteDetail = referralNote.Note;
        $scope.IsEdit = true;
        $scope.CommonNoteID = referralNote.CommonNoteID;
    };

    $scope.DeleteReferralNote = function (referralNote) {
        bootboxDialog(function (result) {
            if (result) {
                $scope.CommonNoteID = referralNote.CommonNoteID;
                var jsonData = angular.toJson({ CommonNoteID: $scope.CommonNoteID });

                AngularAjaxCall($http, HomeCareSiteUrl.DeleteReferralNoteURL, jsonData, "Post", "json", "application/json").success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.GetReferralNotes();
                    }
                    ShowMessages(response);
                });
            }
        }, bootboxDialogType.Confirm, bootboxDialogTitle.Delete, window.DeleteNoteMessage, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);
    };
    $scope.PermissionList = [];
    $scope.GetReferralRole = function () {

        AngularAjaxCall($http, HomeCareSiteUrl.RolePermissionsURL, "", "Get").success(function (response) {
            ShowMessages(response);
            $scope.PermissionList = response.RoleList;
        });
    };

    $scope.GetReferralCategory = function () {
        AngularAjaxCall($http, "/hc/referral/GetNoteCategory", "", "Get").success(function (response) {
            //ShowMessages(response);
            $scope.CategoryList = response.Data;
            $scope.selecteditem ="0";
        });
    };

    $scope.ResetRNote = function () {
       
        $scope.ReferralNoteDetail = '';
        $scope.ReferralNote.SelectedRoleID = "";
        $scope.EmployeesID = "";
        $scope.GetReferralCategory();
        $scope.GetReferralRole();
        $scope.GetReferralEmployee();
        
    }
    //   $scope.EmployeeList = [];
    $scope.GetReferralEmployee = function () {
        $scope.RoleID = ($scope.SelectedRoleID) ? $scope.SelectedRoleID.toString() : null;
        var jsonData = angular.toJson({ RoleID: $scope.RoleID, });
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralEmployeeURL, jsonData, "Post", "json", "application/json").success(function (response) {
            $scope.EmployeeList = response.Data;
            console.log($scope.EmployeeList);
        });
    };



    $("a#referralNote").on('shown.bs.tab', function (e) {
        $scope.GetReferralNotes();
        $scope.GetNoteSentenceList();
        $scope.GetReferralRole();
        $scope.GetReferralCategory();
        $scope.GetReferralEmployee();
    });

    $("a#employeeNote").on('shown.bs.tab', function (e) {
        $scope.GetReferralNotes();
        $scope.GetNoteSentenceList();
        $scope.GetReferralCategory();
        $scope.GetReferralEmployee();
        $scope.GetReferralRole();
    });
};