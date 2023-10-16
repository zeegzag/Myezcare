var rimModel;

controllers.ReferralTaskMappingController = function ($scope, $http, $window, $timeout) {
    rimModel = $scope;
    $scope.OrgInfo = 'test org info';
    $scope.IsvisibleReport = false;
    $scope.toggle = false;
    $scope.IsDefaultCaretype = true;
    $scope.IsDefaultTasktype = true;
    $scope.CheckMe = false;
    $scope.SelectedTaskMappingIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.TaskMappingModel = $.parseJSON($("#hdnSetReferralTaskMappingModel").val());
    $scope.SearchTaskMappingListPage = $scope.TaskMappingModel.SearchTaskMappingListPage;
    $scope.TempSearchTaskMappingListPage = $scope.TaskMappingModel.SearchTaskMappingListPage;
    $scope.Frequency = 0;
    $scope.SelectedFrequency = 0;
    $scope.EncryptedReferralID = window.EncryptedReferralID;
    //$scope.isChangeEvent[VisitTaskCategoryID] = false;
    $scope.SearchVisitTask = {};

    $scope.VisitTaskListPager = new PagerModule("VisitTaskID", "", "DESC");
    $scope.subCount = 0;
    $scope.VisitTaskList = [];
    $scope.VisitTaskList1 = [];
    $scope.GetVisitTaskList = function (isSearchFilter) {
        $scope.VisitTaskListPager.currentPage = isSearchFilter ? 1 : $scope.VisitTaskListPager.currentPage;

        $scope.SearchVisitTask.IgnoreIds = "";
        $scope.SearchVisitTask.EncryptedReferralID = $scope.EncryptedReferralID;
        $scope.SearchVisitTask.CareTypeID = $scope.CareTypeID;
        var pagermodel = {
            SearchVisitTaskListPage: $scope.SearchVisitTask,
            pageSize: $scope.VisitTaskListPager.pageSize,
            pageIndex: $scope.VisitTaskListPager.currentPage,
            sortIndex: $scope.VisitTaskListPager.sortIndex,
            sortDirection: $scope.VisitTaskListPager.sortDirection
        };
        var jsonData = angular.toJson(pagermodel);

        AngularAjaxCall($http, HomeCareSiteUrl.GetVisitTaskListForRef, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $timeout(function () {

                    if (response.Data.CurrentPage == 1)
                        $scope.VisitTaskList = [];

                    if (response.Data.CurrentPage == 1 || $scope.VisitTaskListPager.lastPage < response.Data.CurrentPage)
                        Array.prototype.push.apply($scope.VisitTaskList, response.Data.Items);


                    $scope.VisitTaskListPager.lastPage = response.Data.CurrentPage;
                    $scope.VisitTaskListPager.currentPageSize = response.Data.Items.length;
                    $scope.VisitTaskListPager.totalRecords = response.Data.TotalItems;
                    $scope.VisitTaskList1 = response.Data.Items;

                    if (!$scope.$root.$$phase) {
                        $scope.$apply();
                    }
                });
            }
            //ShowMessages(response);
        });

    };
    //$scope.isMapped = function () {
    //    alert("mapped");
    //}

    $scope.printPdf = function (CalenderPrint) {

        var innerContents = document.getElementById(CalenderPrint).innerHTML;
        var popupWinindow = window.open('', '_blank', 'width=700,height=700,scrollbars=no,menubar=yes,toolbar=no,location=no,status=yes,titlebar=no');
        popupWinindow.document.open();
        popupWinindow.document.write('<html><head><script src="/Assets/js/sitejs/jquery.js"></script><link href="/Assets/css/sitecss/font-awesome.css" rel="stylesheet"><link href = "/Assets/library/fullcalendar/fullcalendar.css" rel = "stylesheet" /><link href="/Assets/library/ion.rangeslider/css/ion.rangeSlider.css" rel="stylesheet" /><link href="/Assets/library/ion.rangeslider/css/ion.rangeSlider.skinFlat.css" rel="stylesheet" /><link href="/Assets/library/fullcalendar/scheduler.css" rel="stylesheet" /><link href="/Assets/library/LineProgressbar/jquery.lineProgressbar.css" rel="stylesheet" />  <script>function onload() {$($(".fc-right button")[0]).css("display","none");$($(".fc-right button")[1]).css("display","none");$(".ng-hide").css("display","none");window.print();}</script> </head > <body onload="onload()" style="-webkit-print-color-adjust: exact;">' + innerContents + '</html>');

        popupWinindow.document.close();
    }
    
    $scope.CareTypeChange = function () {
         $scope.CareTypeID = $scope.SearchVisitTask.CareTypeID;
        if (!$scope.toggle) {
            if ($scope.SearchVisitTask.CareTypeID != null) {
                //$scope.SearchVisitTasks();
                $scope.IsDefaultCaretype = false;
            }
            else {
                //if ($scope.SearchVisitTask.VisitTaskType != 'Task') {//conclusion
                //    $scope.SearchVisitTasks();
                //}
                $scope.IsDefaultCaretype = true;
            }
        }
        else {
            if ($scope.CareTypeID != null) {
                /*$scope.SearchVisitTasks();*/
                $scope.IsDefaultCaretype = false;
            }
            else {
                //if ($scope.SearchVisitTask.VisitTaskType != 'Task') {//conclusion
                //    $scope.SearchVisitTasks();
                //}
                $scope.IsDefaultCaretype = true;
            }
        }
        $scope.SearchVisitTasks();
    }

    $scope.VisitTypeChange = function () {
        if ($scope.SearchVisitTask.VisitTaskType != 'Task') {
            $scope.SearchVisitTasks();
            $scope.IsDefaultTasktype = false;
        }
        else {
            $scope.IsDefaultTasktype = true;
        }
        $scope.ResetCaretype();
        $scope.CareTypeChange();
    }

    $scope.ResetCaretype = function () {
        $scope.SearchVisitTask.CareTypeID = null;
        $scope.CareTypeID = null;

        if ($scope.SearchVisitTask.VisitTaskType == 'Task') {
            $scope.IsDefaultTasktype = true;
            $scope.IsDefaultCaretype = true;
        }

    }

    $scope.SearchVisitTasks = function () {
            if (!$scope.toggle) {
                var postData = {
                    EncryptedReferralID: $scope.EncryptedReferralID,
                    VisitTaskType: $scope.SearchVisitTask.VisitTaskType,
                    CareTypeID: $scope.SearchVisitTask.CareTypeID,
                };
                var jsonData = angular.toJson(postData);
                AngularAjaxCall($http, HomeCareSiteUrl.GetReferralTaskMappingsURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    if (response.IsSuccess) {
                        $scope.PatientMappedList = [];
                        $scope.PatientUnmappedList = [];
                        $scope.WholeData = [];
                        $scope.parentList = [];
                        $scope.childList = [];
                        $scope.TaskMappedDays = [];
                        $scope.Days = [];
                        $scope.checkme = false;
                        $scope.selectedDays = [];
                        var isMultipleDays = false;
                        $scope.PatientMappedList = response.Data.RefMappedList;
                        $scope.PatientUnmappedList = response.Data.RefUnMappeddList;
                        $scope.WholeData = response.Data.RefMappedList.concat(response.Data.RefUnMappeddList);                        
                        $scope.GetCaretype();
                        for (var i = 0; i < $scope.CaretypeList.length; i++) {
                            var arr = [];
                            var careType = $scope.CaretypeList[i].CareType;
                            var arr2 = $scope.WholeData.filter(function (item) {
                                return item.CareTypeID === $scope.CaretypeList[i].CareTypeID;
                            });
                            if (arr2 != undefined && arr2.length > 0) {
                                arr2.filter(function (item) {//Main and sub category filtering
                                    var parentExist = arr.find(val => val.VisitTaskCategoryID == item.VisitTaskCategoryID);
                                    if (parentExist != undefined) {
                                        //skip
                                    }
                                    else {
                                        if (item.ParentCategoryLevel == 0) {
                                            arr.push(item);

                                        }
                                        //else {------------Subtasks---------------------
                                        //    var subExist = $scope.childList.find(val => val.VisitTaskCategoryID == item.VisitTaskCategoryID);
                                        //    if (subExist != undefined) {
                                        //        //skip
                                        //    }
                                        //    else {
                                        //        $scope.childList.push(item);
                                        //    }


                                        //}
                                    }
                                });
                                $scope.parentList.push({ CareType: careType, parentList: arr });
                            }
                        }
                    }
                });
            }
            if ($scope.toggle) {
                /*old view*/
                $scope.VisitTaskListPager.getDataCallback();
                $scope.GetCaretype();
                $scope.GetVisitTaskCategory();
                $scope.GetVisitTaskSubCategory();
                $scope.GetPatientTaskMappings();
                $scope.SelectAll();
                $scope.SelectAllConclusion();
                $scope.VisitTaskListPager.getDataCallback = $scope.GetVisitTaskList; //$/*scope.GetVisitTaskList;*/

            }
     
    }
    $scope.ToggleView = function ($event) {

        $scope.CareTypeID = $scope.SearchVisitTask.CareTypeID;

        if (event.target.checked) {//old view
            $scope.toggle = event.target.checked;
            $scope.VisitTaskListPager.getDataCallback = $scope.GetVisitTaskList; //$/*scope.GetVisitTaskList;*/
            if ($scope.CareTypeID != null) {
                $scope.SearchVisitTasks();
                $scope.IsDefaultCaretype = false;
            }
            else {
                if ($scope.SearchVisitTask.VisitTaskType != 'Task') {//conclusion
                    $scope.SearchVisitTasks();
                }
                $scope.IsDefaultCaretype = true;
            }

        }
        if (!event.target.checked) {//new view
            $scope.toggle = event.target.checked;
            if ($scope.SearchVisitTask.CareTypeID != null) {
                $scope.SearchVisitTasks();
                $scope.IsDefaultCaretype = false;
            }
            else {
                if ($scope.SearchVisitTask.VisitTaskType != 'Task') {//conclusion
                    $scope.SearchVisitTasks();
                }
                $scope.IsDefaultCaretype = true;
            }
        }
        
        //    $scope.SearchVisitTask();

    }


    //$scope.GetTaskMapReport = function () {
    //    $scope.IsvisibleReport = true;
    //    var postData = {
    //        EncryptedReferralID: $scope.EncryptedReferralID,

    //    };
    //    var jsonData = angular.toJson(postData);
    //    AngularAjaxCall($http, HomeCareSiteUrl.GetReferralTaskMappingsReportURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
    //        if (response.IsSuccess) {
    //            $scope.Refinfo = response.Data.Refinfo;
    //        }
    //    });

    //}

    $scope.GetSubCount = function (VisitTaskID, parentVisitTaskCategoryID) {
        debugger
        $scope.Sub = $scope.childList.filter(item => {
            return item.VisitTaskID == VisitTaskID && item.ParentCategoryLevel == parentVisitTaskCategoryID;

        });
        $scope.subCount = $scope.subCount + $scope.Sub.length;
        //return $scope.subCount;
    }


    $scope.uniqueIds = [];
    $scope.MatchingSubTasks = function (VisitTaskID, parentVisitTaskCategoryID, careTypeID) {

        $scope.CheckMe1 = false;
        $scope.CheckMe2 = false;
        var isMultipleDays = false;
        $scope.selectedDays = [];
        $scope.TaskMappedDays = [];
        $scope.FreqComments = [];
        var postData = {
            EncryptedReferralID: $scope.EncryptedReferralID,
            VisitTaskType: $scope.SearchVisitTask.VisitTaskType,
        };

        $scope.Sub = $scope.WholeData.filter(item => {
            //return item.VisitTaskID == VisitTaskID /*&& item.VisitTaskCategoryID == subVisitTaskCategoryID */ && item.ParentCategoryLevel == parentVisitTaskCategoryID;
            return /*item.ParentCategoryLevel == parentVisitTaskCategoryID &&*/ item.CareTypeID == careTypeID && item.VisitTaskType == 'Task' && item.VisitTaskCategoryID == parentVisitTaskCategoryID;

        });
        $scope.ConclusionList = $scope.WholeData.filter(item => {
            return item.CareTypeID == careTypeID && item.ParentCategoryLevel == 0 && item.VisitTaskType=='Conclusion';

        });
        $scope.TaskWithoutSubList = $scope.WholeData.filter(item => {
            return item.CareTypeID == careTypeID && item.ParentCategoryLevel == 0 && item.VisitTaskType == 'Task' && item.VisitTaskCategoryID == parentVisitTaskCategoryID;

        });
        

        $scope.unique = $scope.Sub.filter(element => {
            const isDuplicate = $scope.uniqueIds.includes(element.VisitTaskCategoryID);

            if (!isDuplicate) {
                $scope.uniqueIds.push(element.VisitTaskCategoryID);

                return true;
            }
        });

        if ($scope.Sub.length>0)
            $scope.subCount = $scope.Sub.length;
        else
            $scope.subCount = $scope.TaskWithoutSubList.length;;

        var allDay = '';
        //var mappedDays = $scope.TaskMappedDays.find(val => val.VisitTaskID == VisitTaskID && val.VisitTaskCategoryID == subVisitTaskCategoryID);

        if ($scope.Sub.length > 0) {
            // $scope.unique.filter(item => {
            for (var item = 0; item < $scope.Sub.length; item++) {
                //if ($scope.unique[item]!= undefined) {
                $scope.VTDetailText = '';
                $scope.selectedDays = [];
                var days = $scope.Sub[item].Days;

                if (days.startsWith(",")) {
                    days = days.replace(',', '');
                }

                if (days != null && days != undefined && days != '0' && days != '') {

                    isMultipleDays = days.includes(",");

                    if (isMultipleDays)
                        $scope.selectedDays = days.split(',');

                    else
                        $scope.selectedDays = days;//single day

                }
                else {//No days mapped
                    $scope.selectedDays = "0";
                }

                $scope.TaskMappedDays = [];
                //$scope.VTDetailText = $scope.Sub[item].VisitTaskDetail.replace(/ /g, '')

                if ($scope.selectedDays != undefined && $scope.selectedDays != '' && $scope.selectedDays != '0') {
                    if ($scope.selectedDays.length != 1) {
                        $scope.selectedDays.filter(function (i) {
                            $scope.TaskMappedDays.push({ Days: i, Checked: true, VisitTaskID: $scope.Sub[item].VisitTaskID, VisitTaskCategoryID: $scope.Sub[item].VisitTaskCategoryID, Frequency: $scope.Sub[item].Frequency, comment: $scope.Sub[item].comment, CareTypeID: $scope.Sub[item].CareTypeID, VisitTaskDetail: $scope.Sub[item].VisitTaskDetail });

                        });
                    }
                    else {
                        $scope.TaskMappedDays.push({ Days: $scope.selectedDays[0], Checked: true, VisitTaskID: $scope.Sub[item].VisitTaskID, VisitTaskCategoryID: $scope.Sub[item].VisitTaskCategoryID, Frequency: $scope.Sub[item].Frequency, comment: $scope.Sub[item].comment, CareTypeID: $scope.Sub[item].CareTypeID, VisitTaskDetail: $scope.Sub[item].VisitTaskDetail });

                    }

                }
                else {//No days mapped

                    $scope.TaskMappedDays.push({ Days: 0, Checked: false, VisitTaskID: $scope.Sub[item].VisitTaskID, VisitTaskCategoryID: $scope.Sub[item].VisitTaskCategoryID, Frequency: $scope.Sub[item].Frequency, comment: $scope.Sub[item].comment, CareTypeID: $scope.Sub[item].CareTypeID, VisitTaskDetail: $scope.Sub[item].VisitTaskDetail });

                }
                allDay = '';
                $scope.TaskMappedDays.filter(e => {


                    if (e.Days === "1") {
                        $scope.CheckMe1 = e.Checked;

                        $("#1_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        allDay = allDay + '1';

                    }
                    if (e.Days === "2") {
                        $scope.CheckMe2 = e.Checked;
                        $("#2_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        allDay = allDay + '2';
                    }
                    if (e.Days === "3") {

                        $("#3_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        allDay = allDay + '3';
                    }
                    if (e.Days === "4") {

                        $("#4_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        allDay = allDay + '4';
                    }
                    if (e.Days === "5") {

                        $("#5_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        allDay = allDay + '5';

                    }
                    if (e.Days === "6") {
                        $("#6_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        allDay = allDay + '6';

                    }
                    if (e.Days === "7") {
                        $("#7_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        allDay = allDay + '7';

                    }

                    if (e.Days === 0) {//unmapped days                         

                        $("#1_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        $("#2_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        $("#3_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        $("#4_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        $("#5_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        $("#6_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        $("#7_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        $("#1234567_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        $("#freq_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop("disabled", true);

                    }

                    var sort = allDay.split('').sort((a, b) => a.localeCompare(b)).join('');
                    if (sort == '1234567') {


                        $("#1_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        $("#2_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        $("#3_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        $("#4_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        $("#5_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        $("#6_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        $("#7_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        $("#1234567_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                    }
                    else {
                        $("#1234567_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', false);
                    }

                    $scope.SelectedFrequency = e.Frequency;
                    $scope.SelectedComment = e.Comment;


                });
                //if ($scope.Sub[item].IsDefault) {//true
                //    $("#1_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                //    $("#2_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop('disabled', true);
                //    $("#3_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                //    $("#4_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                //    $("#5_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                //    $("#6_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                //    $("#7_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                //    $("#1234567_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                //}
            }
        }
        else {
            if ($scope.ConclusionList.length > 0) {
                for (var item = 0; item < $scope.ConclusionList.length; item++) {
                    //if ($scope.unique[item]!= undefined) {
                    $scope.VTDetailText = '';
                    $scope.selectedDays = [];
                    var days = $scope.ConclusionList[item].Days;

                    if (days.startsWith(",")) {
                        days = days.replace(',', '');
                    }

                    if (days != null && days != undefined && days != '0' && days != '') {

                        isMultipleDays = days.includes(",");

                        if (isMultipleDays)
                            $scope.selectedDays = days.split(',');

                        else
                            $scope.selectedDays = days;//single day

                    }
                    else {//No days mapped
                        $scope.selectedDays = "0";
                    }

                    $scope.TaskMappedDays = [];
                    //$scope.VTDetailText = $scope.Sub[item].VisitTaskDetail.replace(/ /g, '')

                    if ($scope.selectedDays != undefined && $scope.selectedDays != '' && $scope.selectedDays != '0') {
                        if ($scope.selectedDays.length != 1) {
                            $scope.selectedDays.filter(function (i) {
                                $scope.TaskMappedDays.push({ Days: i, Checked: true, VisitTaskID: $scope.ConclusionList[item].VisitTaskID, VisitTaskCategoryID: $scope.ConclusionList[item].VisitTaskCategoryID, Frequency: $scope.ConclusionList[item].Frequency, comment: $scope.ConclusionList[item].comment, CareTypeID: $scope.ConclusionList[item].CareTypeID, VisitTaskDetail: $scope.ConclusionList[item].VisitTaskDetail });

                            });
                        }
                        else {
                            $scope.TaskMappedDays.push({ Days: $scope.selectedDays[0], Checked: true, VisitTaskID: $scope.ConclusionList[item].VisitTaskID, VisitTaskCategoryID: $scope.ConclusionList[item].VisitTaskCategoryID, Frequency: $scope.ConclusionList[item].Frequency, comment: $scope.ConclusionList[item].comment, CareTypeID: $scope.ConclusionList[item].CareTypeID, VisitTaskDetail: $scope.ConclusionList[item].VisitTaskDetail });

                        }

                    }
                    else {//No days mapped

                        $scope.TaskMappedDays.push({ Days: 0, Checked: false, VisitTaskID: $scope.ConclusionList[item].VisitTaskID, VisitTaskCategoryID: $scope.ConclusionList[item].VisitTaskCategoryID, Frequency: $scope.ConclusionList[item].Frequency, comment: $scope.ConclusionList[item].comment, CareTypeID: $scope.ConclusionList[item].CareTypeID, VisitTaskDetail: $scope.ConclusionList[item].VisitTaskDetail });

                    }
                    allDay = '';
                    $scope.TaskMappedDays.filter(e => {


                        if (e.Days === "1") {
                            $scope.CheckMe1 = e.Checked;

                            $("#1_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            allDay = allDay + '1';

                        }
                        if (e.Days === "2") {
                            $scope.CheckMe2 = e.Checked;
                            $("#2_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            allDay = allDay + '2';
                        }
                        if (e.Days === "3") {

                            $("#3_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            allDay = allDay + '3';
                        }
                        if (e.Days === "4") {

                            $("#4_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            allDay = allDay + '4';
                        }
                        if (e.Days === "5") {

                            $("#5_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            allDay = allDay + '5';

                        }
                        if (e.Days === "6") {
                            $("#6_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            allDay = allDay + '6';

                        }
                        if (e.Days === "7") {
                            $("#7_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            allDay = allDay + '7';

                        }

                        if (e.Days === 0) {//unmapped days                         

                            $("#1_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            $("#2_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            $("#3_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            $("#4_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            $("#5_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            $("#6_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            $("#7_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            $("#1234567_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            $("#freq_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop("disabled", true);

                        }

                        var sort = allDay.split('').sort((a, b) => a.localeCompare(b)).join('');
                        if (sort == '1234567') {


                            $("#1_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            $("#2_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            $("#3_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            $("#4_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            $("#5_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            $("#6_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            $("#7_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            $("#1234567_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                        }
                        else {
                            $("#1234567_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', false);
                        }

                        $scope.SelectedFrequency = e.Frequency;
                        $scope.SelectedComment = e.Comment;


                    });
                    //if ($scope.Sub[item].IsDefault) {//true
                    //    $("#1_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                    //    $("#2_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop('disabled', true);
                    //    $("#3_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                    //    $("#4_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                    //    $("#5_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                    //    $("#6_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                    //    $("#7_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                    //    $("#1234567_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                    //}
                }
            }
            else {
                if ($scope.TaskWithoutSubList.length > 0) {
                    for (var item = 0; item < $scope.TaskWithoutSubList.length; item++) {
                        //if ($scope.unique[item]!= undefined) {
                        $scope.VTDetailText = '';
                        $scope.selectedDays = [];
                        var days = $scope.TaskWithoutSubList[item].Days;

                        if (days.startsWith(",")) {
                            days = days.replace(',', '');
                        }

                        if (days != null && days != undefined && days != '0' && days != '') {

                            isMultipleDays = days.includes(",");

                            if (isMultipleDays)
                                $scope.selectedDays = days.split(',');

                            else
                                $scope.selectedDays = days;//single day

                        }
                        else {//No days mapped
                            $scope.selectedDays = "0";
                        }

                        $scope.TaskMappedDays = [];
                        //$scope.VTDetailText = $scope.Sub[item].VisitTaskDetail.replace(/ /g, '')

                        if ($scope.selectedDays != undefined && $scope.selectedDays != '' && $scope.selectedDays != '0') {
                            if ($scope.selectedDays.length != 1) {
                                $scope.selectedDays.filter(function (i) {
                                    $scope.TaskMappedDays.push({ Days: i, Checked: true, VisitTaskID: $scope.TaskWithoutSubList[item].VisitTaskID, VisitTaskCategoryID: $scope.TaskWithoutSubList[item].VisitTaskCategoryID, Frequency: $scope.TaskWithoutSubList[item].Frequency, comment: $scope.TaskWithoutSubList[item].comment, CareTypeID: $scope.TaskWithoutSubList[item].CareTypeID, VisitTaskDetail: $scope.TaskWithoutSubList[item].VisitTaskDetail });

                                });
                            }
                            else {
                                $scope.TaskMappedDays.push({ Days: $scope.selectedDays[0], Checked: true, VisitTaskID: $scope.TaskWithoutSubList[item].VisitTaskID, VisitTaskCategoryID: $scope.TaskWithoutSubList[item].VisitTaskCategoryID, Frequency: $scope.TaskWithoutSubList[item].Frequency, comment: $scope.TaskWithoutSubList[item].comment, CareTypeID: $scope.TaskWithoutSubList[item].CareTypeID, VisitTaskDetail: $scope.TaskWithoutSubList[item].VisitTaskDetail });

                            }

                        }
                        else {//No days mapped

                            $scope.TaskMappedDays.push({ Days: 0, Checked: false, VisitTaskID: $scope.TaskWithoutSubList[item].VisitTaskID, VisitTaskCategoryID: $scope.TaskWithoutSubList[item].VisitTaskCategoryID, Frequency: $scope.TaskWithoutSubList[item].Frequency, comment: $scope.TaskWithoutSubList[item].comment, CareTypeID: $scope.TaskWithoutSubList[item].CareTypeID, VisitTaskDetail: $scope.TaskWithoutSubList[item].VisitTaskDetail });

                        }
                        allDay = '';
                        $scope.TaskMappedDays.filter(e => {


                            if (e.Days === "1") {
                                $scope.CheckMe1 = e.Checked;

                                $("#1_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                allDay = allDay + '1';

                            }
                            if (e.Days === "2") {
                                $scope.CheckMe2 = e.Checked;
                                $("#2_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                allDay = allDay + '2';
                            }
                            if (e.Days === "3") {

                                $("#3_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                allDay = allDay + '3';
                            }
                            if (e.Days === "4") {

                                $("#4_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                allDay = allDay + '4';
                            }
                            if (e.Days === "5") {

                                $("#5_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                allDay = allDay + '5';

                            }
                            if (e.Days === "6") {
                                $("#6_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                allDay = allDay + '6';

                            }
                            if (e.Days === "7") {
                                $("#7_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                allDay = allDay + '7';

                            }

                            if (e.Days === 0) {//unmapped days                         

                                $("#1_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                $("#2_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                $("#3_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                $("#4_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                $("#5_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                $("#6_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                $("#7_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                $("#1234567_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                $("#freq_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop("disabled", true);

                            }

                            var sort = allDay.split('').sort((a, b) => a.localeCompare(b)).join('');
                            if (sort == '1234567') {


                                $("#1_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                $("#2_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                $("#3_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                $("#4_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                $("#5_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                $("#6_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                $("#7_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                                $("#1234567_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', e.Checked);
                            }
                            else {
                                $("#1234567_" + e.VisitTaskCategoryID + e.CareTypeID + e.VisitTaskID).prop('checked', false);
                            }

                            $scope.SelectedFrequency = e.Frequency;
                            $scope.SelectedComment = e.Comment;


                        });
                        //if ($scope.Sub[item].IsDefault) {//true
                        //    $("#1_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                        //    $("#2_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop('disabled', true);
                        //    $("#3_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                        //    $("#4_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                        //    $("#5_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                        //    $("#6_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                        //    $("#7_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                        //    $("#1234567_" + $scope.Sub[item].VisitTaskCategoryID + $scope.Sub[item].CareTypeID + $scope.Sub[item].VisitTaskID).prop("disabled", true);
                        //}
                    }
                }
            }
        }
       

    }
    $scope.OpenShowGoalModal = function () {
        $scope.GetReferralGoal();
        $('#ShowGoalModal').modal({ backdrop: 'static', keyboard: false });
    }
    $scope.CloseShowGoalModal = function () {
        $('#ShowGoalModal').modal('hide');
    }
    $scope.OpenAddGoalModal = function () {
        //$scope.GetReferralGoal();
        $scope.Goal = { GoalID: 0, Goal: "" };
        $('#AddGoalModal').modal({ backdrop: 'static', keyboard: false });
    }
    $scope.LoadGoal = function (goal) {
        $scope.Goal = goal;
        $('#AddGoalModal').modal({ backdrop: 'static', keyboard: false });
    }
    $scope.CloseAddGoalModal = function () {
        $('#AddGoalModal').modal('hide');
    }
    $scope.UpdateGoalIsActiveIsDeletedFlag = function (isActive, isDeleted, goalID) {
        var postData = {
            EncryptedReferralID: $scope.EncryptedReferralID,
            GoalIDs: goalID,
            IsActive: isActive,
            IsDeleted: isDeleted
        };
        var jsonData = angular.toJson(postData);
        AngularAjaxCall($http, HomeCareSiteUrl.UpdateIsActiveIsDeletedReferralGoalURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.GetReferralGoal();
            }
        });
    }
    $scope.UpdateGoalIsDeletedFlag = function (isDeleted, goalID) {
        var postData = {
            EncryptedReferralID: $scope.EncryptedReferralID,
            GoalIDs: goalID,
            IsDeleted: isDeleted
        };
        var jsonData = angular.toJson(postData);
        AngularAjaxCall($http, HomeCareSiteUrl.UpdateIsActiveIsDeletedReferralGoalURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.GetReferralGoal();
            }
        });
    }
    $scope.BulkDeleteGoals = function () {
        var goalIDs = $scope.BulkDeleteGoalArray.join(',');
        var postData = {
            EncryptedReferralID: $scope.EncryptedReferralID,
            GoalIDs: goalIDs,
            IsDeleted: true
        };
        var jsonData = angular.toJson(postData);
        AngularAjaxCall($http, HomeCareSiteUrl.UpdateIsActiveIsDeletedReferralGoalURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.BulkDeleteGoalArray = [];
                $scope.GetReferralGoal();
            }
        });
    }
    $scope.BulkDeleteGoalArray = [];
    $scope.ShowHideBulkDeleteButton = function (goalID) {
        if ($scope.BulkDeleteGoalArray.length === 0 || $scope.BulkDeleteGoalArray.indexOf(goalID) === -1) {
            $scope.BulkDeleteGoalArray.push(goalID);
        } else {
            $scope.BulkDeleteGoalArray.splice($scope.BulkDeleteGoalArray.indexOf(goalID), 1);
        }
    }
    $scope.AddGoalForReferral = function (goalID) {
        var postData = {
            EncryptedReferralID: $scope.EncryptedReferralID,
            Goal: $scope.Goal.Goal,
            GoalID: goalID
        };
        var jsonData = angular.toJson(postData);
        AngularAjaxCall($http, HomeCareSiteUrl.SaveReferralGoalURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $('#AddGoalModal').modal('hide');
                $scope.GetReferralGoal();
            }
        });
    }

    $scope.GetTaskMapReport = function (PrintPDFRefTaskMapping) {
        //$scope.IsvisibleReport = true;
        var jsPrintString = "function addScript(url) { var script = document.createElement('script'); script.type = 'application/javascript'; script.src = url; ";
        jsPrintString += " document.head.appendChild(script); } addScript('https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.10.1/html2pdf.bundle.min.js'); setTimeout(function(){ html2pdf(document.body); }, 2000); " ;
        
        
        $scope.GetTaskMapReportData();
        setTimeout(function () {
            var innerContents = document.getElementById(PrintPDFRefTaskMapping).innerHTML;            
            var popupWinindow = window.open('', '_blank', 'width=700,height=1000,scrollbars=no,menubar=yes,toolbar=no,location=no,status=yes,titlebar=no');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head><script src="/Assets/js/sitejs/jquery.js"></script><link href="/Assets/css/sitecss/font-awesome.css" rel="stylesheet"><link href = "/Assets/library/fullcalendar/fullcalendar.css" rel = "stylesheet" /> <script> function onload() {$($(".fc-right button")[0]).css("display","none");$($(".fc-right button")[1]).css("display","none"); ' + jsPrintString + ' }</script> </head > <body onload="onload()" style="-webkit-print-color-adjust: exact;">' + innerContents + '</html>');
            popupWinindow.document.close();
        }, 2000);
    }
    $scope.GetReferralGoal = function () {
        var postData = {
            EncryptedReferralID: $scope.EncryptedReferralID,
        };
        var jsonData = angular.toJson(postData);
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralGoalURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                console.log(response.Data);
                $scope.Goals = response.Data;
            }
        });
    }
    $scope.GetTaskMapReportData = function () {
        $scope.ReportData = [];
        $scope.MedicationData = [];
        $scope.ServicePlanData = [];
        var postData = {
            EncryptedReferralID: $scope.EncryptedReferralID,
        };
        var jsonData = angular.toJson(postData);
        AngularAjaxCall($http, HomeCareSiteUrl.GetReferralTaskMappingsReportURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.SiteLogo = response.Data.OrgInfo[0].SiteLogo;//0
                $scope.OrgInfo = response.Data.OrgInfo[0].OrgInfo;//1

                $scope.ReportData.push($scope.SiteLogo);
                $scope.ReportData.push($scope.OrgInfo);

                $scope.ReferralName = response.Data.ReferralInfo[0].ReferralName;
                $scope.AboutInfo = response.Data.AboutAndAddressInfo[0].AboutInfo;
                $scope.RefAddress = response.Data.AboutAndAddressInfo[0].AddressInfo;

                $scope.GoalArray = []; 
                for (var i = 0; i < response.Data.GoalInfo.length; i++) {
                    $scope.GoalArray.push(response.Data.GoalInfo[i]);
                }


                $scope.ReportData.push($scope.ReferralName);//2
                $scope.ReportData.push($scope.AboutInfo);//3
                $scope.ReportData.push($scope.RefAddress);//4
                $scope.ReportData.push($scope.GoalArray);//5


                if (response.Data.MedicationInfo.length > 0) {
                    for (var item = 0; item < response.Data.MedicationInfo.length; item++) {
                        $scope.MedicationName = response.Data.MedicationInfo[item].MedicationName;
                        $scope.Dose = response.Data.MedicationInfo[item].Dose;
                        $scope.Route = response.Data.MedicationInfo[item].Route;
                        $scope.MedFrequency = response.Data.MedicationInfo[item].Frequency;
                        $scope.PatientInstructions = response.Data.MedicationInfo[item].PatientInstructions;

                        $scope.MedicationData.push({ MedicationName: $scope.MedicationName ,Dose: $scope.Dose, Route: $scope.Route, Frequency: $scope.MedFrequency, PatientInstructions: $scope.PatientInstructions });

                    }

                }
                if (response.Data.ServicePlanInfo.length > 0) {
                    for (var item = 0; item < response.Data.ServicePlanInfo.length; item++) {
                        $scope.VisitTaskCategoryName = response.Data.ServicePlanInfo[item].VisitTaskCategoryName;
                        $scope.VisitTaskDetail = response.Data.ServicePlanInfo[item].VisitTaskDetail;
                        $scope.VisitTaskCategoryID = response.Data.ServicePlanInfo[item].VisitTaskCategoryID;


                        $scope.ServicePlanData.push({ VisitTaskCategoryName: $scope.VisitTaskCategoryName, VisitTaskDetail: $scope.VisitTaskDetail, VisitTaskCategoryID: $scope.VisitTaskCategoryID });

                    }
                    $scope.uniqueMainTaskDetail = [];
                    $scope.uniqueMainTaskDetail = $scope.ServicePlanData.filter(element => {
                        const isDuplicate = $scope.uniqueIds.includes(element.VisitTaskCategoryID);

                        if (!isDuplicate) {
                            $scope.uniqueIds.push(element.VisitTaskCategoryID);

                            return true;
                        }
                    });

                }
                if (response.Data.TaskCodes1.length > 0) {//Monday
                    $scope.TaskCodes1Detail = [];
                    for (var item = 0; item < response.Data.TaskCodes1.length; item++) {
                        $scope.TaskCodes1Detail.push({ Day: response.Data.TaskCodes1[item].Day1, VisitTaskDetail: response.Data.TaskCodes1[item].VisitTaskDetail1 })
                    }

                }
                if (response.Data.TaskCodes2.length > 0) {//Tuesday
                    $scope.TaskCodes2Detail = [];
                    for (var item = 0; item < response.Data.TaskCodes2.length; item++) {
                        $scope.TaskCodes2Detail.push({ Day: response.Data.TaskCodes2[item].Day2, VisitTaskDetail: response.Data.TaskCodes2[item].VisitTaskDetail2 })
                    }

                }
                if (response.Data.TaskCodes3.length > 0) {//wednesday
                    $scope.TaskCodes3Detail = [];
                    for (var item = 0; item < response.Data.TaskCodes3.length; item++) {
                        $scope.TaskCodes3Detail.push({ Day: response.Data.TaskCodes3[item].Day3, VisitTaskDetail: response.Data.TaskCodes3[item].VisitTaskDetail3 })
                    }

                }
                if (response.Data.TaskCodes4.length > 0) {//Thursday
                    $scope.TaskCodes4Detail = [];
                    for (var item = 0; item < response.Data.TaskCodes4.length; item++) {
                        $scope.TaskCodes4Detail.push({ Day: response.Data.TaskCodes4[item].Day4, VisitTaskDetail: response.Data.TaskCodes4[item].VisitTaskDetail4 })
                    }

                }
                if (response.Data.TaskCodes5.length > 0) {//Friday
                    $scope.TaskCodes5Detail = [];
                    for (var item = 0; item < response.Data.TaskCodes5.length; item++) {
                        $scope.TaskCodes5Detail.push({ Day: response.Data.TaskCodes5[item].Day5, VisitTaskDetail: response.Data.TaskCodes5[item].VisitTaskDetail5 })
                    }

                }
                if (response.Data.TaskCodes6.length > 0) {//saturday
                    $scope.TaskCodes6Detail = [];
                    for (var item = 0; item < response.Data.TaskCodes6.length; item++) {
                        $scope.TaskCodes6Detail.push({ Day: response.Data.TaskCodes6[item].Day6, VisitTaskDetail: response.Data.TaskCodes6[item].VisitTaskDetail6 })
                    }

                }
                if (response.Data.TaskCodes7.length > 0) {//Sunday
                    $scope.TaskCodes7Detail = [];
                    for (var item = 0; item < response.Data.TaskCodes7.length; item++) {
                        $scope.TaskCodes7Detail.push({ Day: response.Data.TaskCodes7[item].Day7, VisitTaskDetail: response.Data.TaskCodes7[item].VisitTaskDetail7 })
                    }

                }
            }
        });
    }

    $scope.GetSPTaskDetail = function (VisitTaskCategoryID) {
        $scope.SubTaskDetail = [];

        $scope.SubTaskDetail = $scope.ServicePlanData.filter(item => {
            return item.VisitTaskCategoryID == VisitTaskCategoryID;
        });

    }
    // select options
    $scope.events = [];
    $scope.selectedEvents = [''];
    $scope.FrequencyChange = function (sub, FrequencyId) {
        debugger
        $scope.Frequency = FrequencyId;// $("#Frequency-"+sub.VisitTaskCategoryID).find(":selected").val();
        $scope.Comment = angular.element('#Comment_' + sub.VisitTaskCategoryID + sub.CareTypeID + sub.VisitTaskID).val();
        $scope.AddReferralTaskMapping(sub);
        $scope.SelectedFrequency = FrequencyId;

    }

    $scope.CaretypeChange = function (CaretypeID) {
        $scope.SearchVisitTask();
    }

    $scope.AddReferralTaskMapping = function (item) {
        debugger;
        if ($scope.toggle) {
            var postData = {
                VisitTaskID: item.VisitTaskID,
                IsRequired: item.IsRequired,
                EncryptedReferralID: $scope.EncryptedReferralID,
                Toggle: $scope.toggle
            };
            var jsonData = angular.toJson(postData);
            AngularAjaxCall($http, HomeCareSiteUrl.SaveReferralTaskMappingURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.GetVisitTaskList();
                    $scope.GetPatientTaskMappings();
                }
            });


        }

        if (!$scope.toggle) {
            if (item.Frequency == 'Nil')
                $scope.Frequency = '';
            var postData = {
                VisitTaskID: item.VisitTaskID,
                IsRequired: false,//item.IsRequired,
                EncryptedReferralID: $scope.EncryptedReferralID,
                Frequency: $scope.Frequency,
                Days: item.Days,
                Comment: $scope.Comment
            };
            var jsonData = angular.toJson(postData);
            AngularAjaxCall($http, HomeCareSiteUrl.SaveReferralTaskMappingURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    if (response.Data == 1) {
                        //$("#Alert").text('Days updated successfully');
                        //bootbox.alert({
                        //    title: "<span style='color: red;'>Days mapped!</span>",
                        //    message: "Days updated successfully",
                        //    size: 'small'
                        //});
                        //return;
                    }
                    if (response.Data == 2) {
                        //$("#Alert").text('Days added successfully');
                        //bootbox.alert({
                        //    title: "<span style='color: red;'>Days mapped!</span>",
                        //    message: "Days added successfully",
                        //    size: 'small'
                        //});
                        //return;
                    }

                }
            });

        }


    };

    $scope.TaskModel = {}
    $scope.OpenTaskCommentModal = function (item) {
        $scope.TaskModel.ReferralTaskMappingID = item.ReferralTaskMappingID;
        $scope.TaskModel.Frequency = item.Frequency;
        $scope.TaskModel.Comment = item.Comment;
        $('#TaskCommentModal').modal('show');
    }

    $scope.SaveTaskDetail = function () {
        var jsonData = angular.toJson($scope.TaskModel);
        AngularAjaxCall($http, HomeCareSiteUrl.SaveTaskDetailURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.TaskModel = {};
                $scope.GetPatientTaskMappings();
                $('#TaskCommentModal').modal('hide');
            }
        });

    }

    $('#TaskCommentModal').on('hidden.bs.modal', function () {
        $scope.TaskModel = {};
        HideErrors($("#addTaskCommentFrm"));
    });

    $scope.PatientTaskList = [];
    $scope.PatientConclusionList = [];
    $scope.PatientTaskList1 = [];

    $scope.GetPatientTaskMappings = function () {
        var postData = {
            EncryptedReferralID: $scope.EncryptedReferralID,
            CareTypeID: $scope.CareTypeID,
        };
        var jsonData = angular.toJson(postData);
        AngularAjaxCall($http, HomeCareSiteUrl.GetPatientTaskMappingsURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.PatientTaskList = [];
                $scope.PatientConclusionList = [];
                $scope.PatientTaskList = response.Data.PatientTaskList;
                $scope.PatientConclusionList = response.Data.PatientConclusionList;
                $scope.TaskFrequencyCodeList = response.Data.TaskFrequencyCodeList;
                $scope.PatientTaskList1 = response.Data.Items;
            }
        });
    };

    $scope.DeleteRefTaskMapping = function (item, type) {
        var message = window.DeleteConclusionConfirmationMessage;
        if (type === 'task')
            message = window.DeleteTaskConfirmationMessage;

        bootboxDialog(function (result) {
            if (result) {
                var postData = {
                    EncryptedReferralID: $scope.EncryptedReferralID,
                    ReferralTaskMappingID: item.ReferralTaskMappingID
                };
                var jsonData = angular.toJson(postData);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteRefTaskMappingURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    if (response.IsSuccess) {

                        $scope.PatientTaskList = [];
                        $scope.PatientConclusionList = [];
                        $scope.PatientTaskList = response.Data.PatientTaskList;
                        $scope.PatientConclusionList = response.Data.PatientConclusionList;

                        $scope.GetVisitTaskList();
                        $scope.SearchVisitTask();
                    }
                });
            }
        },
            bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, message, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);

    };

    $scope.DeleteRefTaskMappingBulk = function (item, type) {

        var message = window.DeleteConclusionConfirmationMessage;
        if (type === 'task')
            message = window.DeleteTaskConfirmationMessage;
        var postData = {
            EncryptedReferralID: $scope.EncryptedReferralID,
            ReferralTaskMappingID: item.ReferralTaskMappingID
        };
        var jsonData = angular.toJson(postData);
        AngularAjaxCall($http, HomeCareSiteUrl.DeleteRefTaskMappingURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {

                $scope.PatientTaskList = [];
                $scope.PatientConclusionList = [];
                $scope.PatientTaskList = response.Data.PatientTaskList;
                $scope.PatientConclusionList = response.Data.PatientConclusionList;

                $scope.GetVisitTaskList();
            }
        });

    };

    $scope.OnTaskChecked = function (item) {
        var postData = {
            EncryptedReferralID: $scope.EncryptedReferralID,
            ReferralTaskMappingID: item.ReferralTaskMappingID,
            IsRequired: item.IsRequired
        };

        var jsonData = angular.toJson(postData);
        AngularAjaxCall($http, HomeCareSiteUrl.OnTaskCheckedURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {


                $scope.PatientTaskList = [];
                $scope.PatientConclusionList = [];
                $scope.PatientTaskList = response.Data.PatientTaskList;
                $scope.PatientConclusionList = response.Data.PatientConclusionList;

                $scope.GetVisitTaskList();
            }
        });
    };

    $scope.SavePopupDataAndAddMapping = function () {
        if ($scope.SelectedCareType && $scope.SelectedActivity && $scope.SelectedVisitTask.length > 0) {
            $.each($scope.SelectedVisitTask, function (index, visitTaskId) {
                var postData = {
                    VisitTaskID: visitTaskId,
                    IsRequired: true,
                    EncryptedReferralID: $scope.EncryptedReferralID
                };
                var jsonData = angular.toJson(postData);
                AngularAjaxCall($http, HomeCareSiteUrl.SaveReferralTaskMappingURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    ShowMessages(response);
                    if (response.IsSuccess) {
                        $scope.GetPatientTaskMappings();
                        $('#model_AddCarePlan').modal('hide');
                    }
                });
            });
            $scope.SelectedVisitTask = [];
            $scope.SelectedActivity = "";
            $scope.SelectedCareType = "";
        }
        else {
            bootboxDialog(null, bootboxDialogType.Alert, bootboxDialogTitle.Alert,
                "Please select all the fields", bootboxDialogButtonText.Ok);
        }
    };

    $scope.AddCarePlanModal = function () {
        $scope.SelectedVisitTask = [];
        $scope.SelectedActivity = "";
        $scope.SelectedCareType = "";
        //$scope.VisitTaskList = [];
        $scope.SearchVisitTasks();
        $('#model_AddCarePlan').modal({
            backdrop: 'static',
            keyboard: false
        });
    }

    $scope.ResetAddCarePlanModal = function () {
        //$scope.SelectedVisitTask = [];
        $scope.SelectedActivity = "";
        $scope.SelectedCareType = "";
        $scope.VisitTaskList = [];
        //$scope.VisitTaskCategories = [];
        //$scope.VisitTaskSubCategories = [];
        $('#model_AddCarePlan').modal('hide');

    }

    $scope.Refresh = function () {
        $scope.GetPatientTaskMappings();
    };

    $scope.ResetSearchFilter = function (Search) {
        Search.VisitTaskDetail = "";
        Search.CareTypeID = "";
        $scope.GetPatientTaskMappings();
    };

    $scope.SelectTaskMapping = function (item) {
        if (item.IsChecked)
            $scope.SelectedTaskMappingIds.push(item.ReferralTaskMappingID);
        else
            $scope.SelectedTaskMappingIds.remove(item.ReferralTaskMappingID);

        if ($scope.SelectedTaskMappingIds.length == $scope.TaskMappingListPager.currentPageSize) {
            $scope.SelectAllCheckbox = true;
        }
        else {
            $scope.SelectAllCheckbox = false;
        }
    };

    $scope.GetVisitTaskCategory = function () {
        var VisitTaskType = $scope.SearchVisitTask.VisitTaskType;
        var CareTypeID = $scope.CareType;
        var jsonData = angular.toJson({ VisitTaskType, CareTypeID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetVisitTaskCategoryURL1, jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.VisitTaskCategories = response.Data;
                    $scope.VisitTaskSubCategories = [];
                }
                else {
                    ShowMessages(response);
                }
            });
    }

    $scope.GetVisitTaskSubCategory = function () {
        var VisitTaskType = $scope.SearchVisitTask.VisitTaskType;
        var CareTypeID = $scope.CareType;
        var VisitTaskCategoryID = $scope.VisitTaskCategoryID;
        var jsonData = angular.toJson({ VisitTaskType, CareTypeID, VisitTaskCategoryID });
        AngularAjaxCall($http, HomeCareSiteUrl.GetVisitTaskSubCategoryURL1, jsonData, "post", "json", "application/json", true).
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    $scope.VisitTaskSubCategories = response.Data;
                }
                else {
                    ShowMessages(response);
                }
            });
    }

    $scope.SelectAll = function (SelectAllCheckbox) {
        $scope.SelectedTaskMappingIds = [];

        angular.forEach($scope.PatientTaskList, function (item, key) {
            item.IsChecked = SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedTaskMappingIds.push(item.ReferralTaskMappingID);
        });

        return true;
    };

    $scope.SelectAllConclusion = function (SelectAllCheckbox) {
        $scope.SelectedTaskMappingIds = [];

        angular.forEach($scope.PatientConclusionList, function (item, key) {
            item.IsChecked = SelectAllCheckbox;
            if (item.IsChecked)
                $scope.SelectedTaskMappingIds.push(item.ReferralTaskMappingID);
        });

        return true;
    };

    $scope.SearchVisitTasksMapping = function (Search) {
        var postData = {
            VisitTaskDetail: Search.VisitTaskDetail,
            CareTypeID: Search.CareTypeID,
            EncryptedReferralID: $scope.EncryptedReferralID,
        };
        var jsonData = angular.toJson(postData);
        AngularAjaxCall($http, HomeCareSiteUrl.GetPatientTaskMappingsURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.PatientTaskList = response.Data.PatientTaskList;
                $scope.PatientConclusionList = response.Data.PatientConclusionList;
                $scope.TaskFrequencyCodeList = response.Data.TaskFrequencyCodeList;
            }
        });
    };

    $scope.DeleteSelectTaskMapping = function (item, type) {
        debugger
        if (item == undefined) {
            $scope.ListOfIdsInCsv = $scope.SelectedTaskMappingIds.toString();
            var message = window.DeleteMultipleConfirmationMessage;
        }

        bootboxDialog(function (result) {
            if (result) {
                var postData = {
                    EncryptedReferralID: $scope.EncryptedReferralID,
                    ListOfIdsInCsv: $scope.ListOfIdsInCsv
                };
                var jsonData = angular.toJson(postData);
                AngularAjaxCall($http, HomeCareSiteUrl.DeleteRefTaskMappingURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    if (response.IsSuccess) {

                        $scope.PatientTaskList = [];
                        $scope.PatientConclusionList = [];
                        $scope.PatientTaskList = response.Data.PatientTaskList;
                        $scope.PatientConclusionList = response.Data.PatientConclusionList;

                        $scope.GetVisitTaskList();
                        $scope.SelectAllCheckbox = false;
                        $(".checked").removeClass("checked");
                    }
                });
            }
        },
            bootboxDialogType.Confirm, bootboxDialogTitle.Confirmation, message, bootboxDialogButtonText.YesContinue, btnClass.BtnDanger);

    };

    $("#AddReferralTaskMappingModal").on('hidden.bs.modal', function () {
        $scope.ResetReferralInternamMessage();
    });

    $("a#CarePlan_ReferralTaskMapping").on('shown.bs.tab', function (e) {
        $scope.GetVisitTaskList();
        $scope.GetPatientTaskMappings();
    });


    $scope.ResultForms = [];
    $scope.SelectedForms = [];
    $scope.SelectMappedForms = [];
    $scope.SelectConclusionMappedForms = [];
    $scope.SelectAllFormCheckbox = false;
    $scope.checkedStatus = false;
    $scope.SelectForm = function (item, $event) {
        if ($scope.toggle) {//old view
            if (item.IsChecked)
                $scope.SelectedForms.push(item);
            else
                $scope.SelectedForms.remove(item);

        }

        if (!$scope.toggle) {//new
            debugger;
            $scope.TotalDays = [];
            var chkId = event.target.id;
            var token = chkId.split('_');

           
            if (item.Days !== null) {
                if (item.Days.startsWith(",")) {
                    item.Days = item.Days.replace(',', '');
                }

                if (item.Days != 0) {
                    if (!item.Days.includes(token[0])) {
                        item.Days = item.Days + ',' + token[0];
                        /* item.Frequency = 'Nil';*/
                        $("#freq_" + item.VisitTaskCategoryID + item.CareTypeID + item.VisitTaskID).prop("disabled", false);

                    }
                    else {
                        $scope.SelectedDay + token[0] == false;
                        item.Days = item.Days.replace(token[0], '');

                        var d = item.Days.replace(/,/g, '');
                        if (d.length != 0) {
                            $("#freq_" + item.VisitTaskCategoryID + item.CareTypeID + item.VisitTaskID).prop("disabled", false);

                        }
                        else {
                            item.Frequency = 'Nil';
                            $("#freq_" + item.VisitTaskCategoryID + item.CareTypeID + item.VisitTaskID).prop("disabled", true);

                        }


                    }
                }
                else {
                    item.Days = token[0];
                    item.Frequency = 'Nil';
                    $("#freq_" + item.VisitTaskCategoryID + item.CareTypeID + item.VisitTaskID).prop("disabled", false);

                }

            }
            else {
                item.Days = token[0];
                item.Frequency = 'Nil';
                $("#freq_" + item.VisitTaskCategoryID + item.CareTypeID + item.VisitTaskID).prop("disabled", false);

            }

            //const { checked } = event.target;
            // $scope.CheckMe = event.target.checked;
            $scope.Comment = angular.element('#Comment_' + item.VisitTaskCategoryID + item.CareTypeID + item.VisitTaskID).val();

            $scope.AddReferralTaskMapping(item);
        }

    };
    $scope.SelectAllForms = function (cat, sub, $event, val) {

        if ($scope.toggle) {//old
            angular.forEach($scope.VisitTaskList, function (item, key) {
                item.IsChecked = val;//event.target.checked; //$scope.SelectAllFormCheckbox;//

                if (item.IsChecked) {
                    if ($scope.SelectedForms.indexOf(item) == -1)
                        $scope.SelectedForms.push(item);
                } else {
                    if ($scope.SelectedForms.indexOf(item) !== -1)
                        $scope.SelectedForms.remove(item);
                }
            });

            return true;
        }

        if (!$scope.toggle) {//new
            if (event.target.checked) {

                sub.Days = '1,2,3,4,5,6,7';
                $("#freq_" + sub.VisitTaskCategoryID + sub.CareTypeID + sub.VisitTaskID).prop("disabled", false);

            }
            else {

                sub.Days = '0';
                sub.Frequency = 'Nil';
            }
            $scope.Comment = angular.element('#Comment_' + sub.VisitTaskCategoryID + sub.CareTypeID + sub.VisitTaskID).val();
            $scope.AddReferralTaskMapping(sub);
        }

    };
    $scope.SelectAllMappedForms = function (val) {
        //$scope.SelectedForms = [];
        angular.forEach($scope.PatientTaskList, function (item, key) {
            item.IsChecked = val;//event.target.checked; //$scope.SelectAllFormCheckbox;//

            if (item.IsChecked) {
                if ($scope.SelectedForms.indexOf(item) == -1)
                    $scope.SelectedForms.push(item);
            } else {
                if ($scope.SelectedForms.indexOf(item) !== -1)
                    $scope.SelectedForms.remove(item);
            }
        });

        return true;
    };
    $scope.SelectAllConclusionMappedForms = function (val) {
        //$scope.SelectedForms = [];
        angular.forEach($scope.PatientConclusionList, function (item, key) {
            item.IsChecked = val;//event.target.checked; //$scope.SelectAllFormCheckbox;//

            if (item.IsChecked) {
                if ($scope.SelectConclusionMappedForms.indexOf(item) == -1)
                    $scope.SelectConclusionMappedForms.push(item);
            } else {
                if ($scope.SelectConclusionMappedForms.indexOf(item) !== -1)
                    $scope.SelectConclusionMappedForms.remove(item);
            }
        });

        return true;
    };
    $scope.MoveFromTaskFormToMappedForm = function () {
        var selectedIds = [];
        var list = $scope.SearchVisitTask.VisitTaskType == Resource_Conclusion ? $scope.PatientConclusionList : $scope.PatientTaskList;
        $.each($scope.SelectedForms, function (index, item) {
            if (list.indexOf(item) === -1) {
                item.IsChecked = false;
                list.push(item);
                $scope.VisitTaskList.remove(item);
                selectedIds.push(item.VisitTaskID);
            }
        });
        if (selectedIds.length > 0) {
            var postData = {
                ListOfIdsInCsv: selectedIds.join(','),
                EncryptedReferralID: $scope.EncryptedReferralID,
                Toggle: $scope.toggle
            };
            var jsonData = angular.toJson(postData);
            AngularAjaxCall($http, HomeCareSiteUrl.SaveReferralTaskMappingURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                ShowMessages(response);
                if (response.IsSuccess) {
                    $scope.GetVisitTaskList();
                    $scope.GetPatientTaskMappings();
                    $scope.SelectedForms = [];
                    $scope.SelectAllFormCheckbox = false;
                    $scope.SelectAllForms();
                }
            });
        }
    };

    $scope.MoveFromMappedFormToTaskForm = function () {
        var selectedIds = [];
        $.each($scope.SelectedForms, function (index, item) {
            if ($scope.VisitTaskList.indexOf(item) === -1) {
                item.IsChecked = false;
                $scope.VisitTaskList.push(item);
                $scope.PatientTaskList.remove(item);
                selectedIds.push(item.ReferralTaskMappingID);
            }
        });
        $.each($scope.SelectConclusionMappedForms, function (index, item) {
            if ($scope.VisitTaskList.indexOf(item) === -1) {
                item.IsChecked = false;
                $scope.VisitTaskList.push(item);
                $scope.PatientConclusionList.remove(item);
                selectedIds.push(item.ReferralTaskMappingID);
            }
        });
        if (selectedIds.length > 0) {
            var postData = {
                EncryptedReferralID: $scope.EncryptedReferralID,
                ListOfIdsInCsv: selectedIds.join(',')
            };
            var jsonData = angular.toJson(postData);
            AngularAjaxCall($http, HomeCareSiteUrl.DeleteRefTaskMappingURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                if (response.IsSuccess) {

                    $scope.PatientTaskList = [];
                    $scope.PatientConclusionList = [];
                    $scope.PatientTaskList = response.Data.PatientTaskList;
                    $scope.PatientConclusionList = response.Data.PatientConclusionList;

                    $scope.GetVisitTaskList();
                    $scope.SelectedForms = [];
                    $scope.SelectAllFormCheckbox = false;
                    $scope.SelectAllForms();
                }
            });
        }
    };
    $scope.SaveTaskFormDetails = function () {
        window.location.reload();
    }

    $scope.CaretypeList = [];
    $scope.GetCaretype = function () {
        var postData = {
            EncryptedReferralID: $scope.EncryptedReferralID,
        };
        var jsonData = angular.toJson(postData);
        AngularAjaxCall($http, HomeCareSiteUrl.GetCarePlanCaretypes, jsonData, "Post", "json", "application/json").success(function (response) {
            ShowMessages(response);
            if (response.IsSuccess) {
                $scope.CaretypeList = response.Data;

            }
        });
    };

    $("a#CarePlan, a#CarePlan_ReferralTaskMapping").on('shown.bs.tab', function (e) {
        $(".tab-pane a[href='#tab_ReferralTaskMapping']").tab('show');
        /* $scope.SearchVisitTasks();*/
        $scope.GetCaretype();
        $scope.IsDefaultCaretype = true;
        $scope.IsDefaultTasktype = true;
        $scope.SearchVisitTasks();
    });

};
controllers.ReferralTaskMappingController.$inject = ['$scope', '$http', '$window', '$timeout'];


app.filter('filterForms', function () {
    return function (formList, formFilter, orgforms) {
        var results = [];
        var formVersionMap = {};
        $.each(formList, function (index, form) {
            var existingVersionForm = formVersionMap[form.Name];
            var isCorrectVersion = !existingVersionForm || form.Version > existingVersionForm.Version;
            if (isCorrectVersion && existingVersionForm) {
                var existingIndex = results.indexOf(existingVersionForm);
                if (existingIndex >= 0) {
                    results.splice(existingIndex, 1);
                }
            }

            if (isCorrectVersion) {
                formVersionMap[form.Name] = form;
            }

            //var isCorrectVersion = true;
            if (form.IsActive && isCorrectVersion) {
                var hasShortName = ValideElement(formFilter.FormNumber) === false || form.Name.toUpperCase().indexOf(formFilter.FormNumber.toUpperCase()) > -1;
                var hasLongName = ValideElement(formFilter.FormName) === false || form.FormLongName.toUpperCase().indexOf(formFilter.FormName.toUpperCase()) > -1;
                //var hasLongName = (formFilter.FormName === '' || formFilter.FormName === null) || form.FormLongName.toUpperCase().indexOf(formFilter.FormName.toUpperCase()) > -1;
                var hasCategory = ValideElement(formFilter.FormCategoryID) === false || formFilter.FormCategoryID == 0 || form.EBCategoryID == formFilter.FormCategoryID;

                var hasPkg = false;

                if (hasShortName && hasLongName && hasCategory) {
                    $.each(form.EbMarketIDList, function (index, item) {

                        if (ValideElement(item) === false || item == '' || ValideElement(formFilter.MarketID) === false) {
                            hasPkg = true;
                            return false;
                        }

                        if (item == formFilter.MarketID) {
                            hasPkg = true;
                            return false;
                        }
                    });
                }

                if (hasShortName && hasLongName && hasCategory && hasPkg) {
                    results.push(form);
                }
            }
        });

        if (orgforms === "orgforms")
            vm.ResultOrgForms = results;
        else
            vm.ResultForms = results;
        return results;
    }
});