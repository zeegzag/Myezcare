var vm;

controllers.LatestERAController = function ($scope, $http) {
    vm = $scope;

    var modalJson = $.parseJSON($("#hdnERAModel").val());
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnERAModel").val());
    };
    $scope.ERAModel = modalJson;

    $scope.ERAsList = [];
    $scope.SelectedERAsListIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.TempSearchERAList = $scope.newInstance().SearchERAList;
    $scope.ERAsListPager = new PagerModule("ReceivedTime", '', "DESC");

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            searchERAListModel: $scope.ERAModel.SearchERAList,
            pageSize: $scope.ERAsListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ERAsListPager.sortIndex,
            sortDirection: $scope.ERAsListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };

    $scope.SearchModelMapping = function () {
        $scope.TempSearchERAList = $.parseJSON(angular.toJson($scope.ERAModel.SearchERAList));
    };

    // Region start ERAs
    $scope.GetERAsList = function () {
        $scope.AjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetERAsListURL, null, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.HCGetERAsList();
                $scope.AjaxStart = false;
            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };

    $scope.HCGetERAsList = function () {
        $scope.AjaxStart = true;
        var jsonData = $scope.SetPostData($scope.ERAsListPager.currentPage);
        AngularAjaxCall($http, HomeCareSiteUrl.HC_GetERAsListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.ERAsList = response.Data.Items;
                $scope.ERAsListPager.currentPageSize = response.Data.Items.length;
                $scope.ERAsListPager.totalRecords = response.Data.TotalItems;
                $scope.AjaxStart = false;
            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };

    $scope.ERAsListPager.getDataCallback = $scope.HCGetERAsList;
    $scope.ERAsListPager.getDataCallback();

    $scope.Reset = function () {
        $scope.ERAModel.SearchERAList = $scope.newInstance().SearchERAList;
        $scope.ERAsListPager.currentPage = 1;
        $scope.ERAsListPager.getDataCallback(true);
    };

    $scope.Refresh = function () {
        $scope.ERAsListPager.currentPage = 1;
        $scope.ERAsListPager.getDataCallback(true);
    };

    $scope.ShowCollpase = function () {
        setTimeout(function () {
            $.each($('.collapseDestination'), function (index, data) {
                var parent = data;
                //$(parent).parent("tbody").find(".collapseSource").removeClass("fa-plus-circle").addClass("fa-minus-circle");

                $(parent).on('show.bs.collapse', function (e) {
                    if ($(this).is(e.target))
                        $(parent).parent("tbody").find(".collapseSource").removeClass("fa-plus-circle").addClass("fa-minus-circle");
                });
                $(parent).on('hidden.bs.collapse', function (e) {
                    if ($(this).is(e.target))
                        $(parent).parent("tbody").find(".collapseSource").removeClass("fa-minus-circle").addClass("fa-plus-circle");
                });
            });
        }, 100);
    };

    $scope.GetERAPDF = function (era_id) {
        $scope.AjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetLatestERAPDF, { eraId: era_id }, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.ShowCollpase();
                var winFeature = 'location=no,toolbar=no,menubar=no,scrollbars=no,resizable=yes,width=' + window.outerWidth / 2 + ',height=' + window.outerHeight / 1.12 + ', overflow= hidden';
                // let pdfWindow = window.open('about:blank', '', "directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no, width=absolute,height=absolute");
                let pdfWindow = window.open('about:blank', 'null', winFeature);
                pdfWindow.document.write("<html<head><title>" + "ERA_" + era_id + ".pdf" + "</title><style>body{margin: 0px;overflow:hidden}iframe{border-width: 0px;}</style></head>");
                pdfWindow.document.write("<body><embed width='100%' height='100%' src='data:application/pdf;base64, " + encodeURI(response.Data.result.data) + "#toolbar=0&navpanes=0&scrollbar=0'></embed></body></html>");
            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };

    $scope.GetERA835 = function (era_id) {


        $scope.AjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.GetLatestERA835, { eraId: era_id }, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {

                //Convert the Byte Data to BLOB object.
                var blob = new Blob([response.Data.result.data], { type: "application/text" });

                //Check the Browser type and download the File.
                var isIE = false || !!document.documentMode;
                if (isIE) {
                    window.navigator.msSaveBlob(blob, "ERA_" + era_id + ".txt");
                } else {
                    var url = window.URL || window.webkitURL;
                    link = url.createObjectURL(blob);
                    var a = $("<a />");
                    a.attr("id", "tempERA835");
                    a.attr("download", "ERA_835_" + era_id + ".txt");
                    a.attr("href", link);
                    $("body").append(a);
                    a[0].click();
                    setTimeout(function () {
                        $("#tempERA835").remove();
                        //$("body").remove(a);
                    }, 2000);

                }

            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };


    $scope.ProcessERA835 = function (era_id, forceProcess) {

        var title = "Are you sure to process this ERA file ?";
        if (forceProcess)
             title = "Are you sure to process this ERA file again?";

        bootboxDialog(function (result) {
            if (result) {

                $scope.AjaxStart = true;
                AngularAjaxCall($http, HomeCareSiteUrl.ProcessERA835, { eraId: era_id, forceProcess: forceProcess }, "Post", "json", "application/json", false).success(function (response) {
                    // if (response.IsSuccess) {
                    $scope.HCGetERAsList();
                    //}
                    $scope.AjaxStart = false;
                    ShowMessages(response);
                });

            }
        }, bootboxDialogType.Confirm, "Confirm", title, bootboxDialogButtonText.YesContinue, btnClass.BtnInfo);


    };





    $scope.ArchieveClaim = function (claim_id) {
        $scope.AjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.ArchieveClaim, { claimId: claim_id }, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                $scope.ShowCollpase();
                ShowMessages(response);
                $scope.Refresh();
            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };



    $scope.ProcessAllERA835 = function (url) {
        bootboxDialog(function (result) {
            if (result) {
                //var win = window.open(url, '_blank');
                //if (win) {
                //    //Browser has allowed it to be opened
                //    win.focus();
                //} else {
                //    //Browser has blocked it
                //    alert('Please allow popups for this website');
                //}

                $("#modal__ProcessAllERA").modal("show");
                $("#iframeProcessERA").attr('src', url);
            }
        }, bootboxDialogType.Confirm, "Confirm", "Are you sure to process all ERA files?", bootboxDialogButtonText.YesContinue, btnClass.BtnInfo);
    };


    // Region end ERAs  
    $scope.ViewBatchDetails_LatestERA = function (batchID, item) {


        var url = "/hc/batch/batchmaster/" + batchID;
        $('#iframe_viewbatch').attr('src', url);
        $("#model__ViewBatch").modal("show");

    }


    $scope.ResizeIframe = function (iframe) {
        var iframe = document.getElementById(iframe);
        if (iframe) {
            //var newHeight = iframe.contentWindow.document.body.scrollHeight;

            try {
                var newHeight = iframe.contentWindow.document.body.getElementsByClassName("batchcontainer")[0].scrollHeight;

                if (newHeight > 500)
                    newHeight = 500;

                if (newHeight < 200)
                    newHeight = 200;


                if (newHeight + "px" != iframe.height)
                    iframe.height = newHeight + "px";
            }
            catch (ex) {

                iframe.height = iframe.height + "px";
            }
        }
    }

    $scope.ViewBatchDetailsModel = {};
    $scope.ViewBatchDetails = function (batchID, item) {
        var jsonData = angular.toJson({
            BatchId: batchID
        });
        $scope.ViewBatchDetailsModel.BatchID = batchID;
        $scope.ViewBatchDetailsModel.IsBatchSent = true;
        $scope.ViewBatchDetailsModel.PayorName = item.PayerName;
        //$scope.ViewBatchDetailsModel.StartDate = item.StartDate;
        //$scope.ViewBatchDetailsModel.EndDate = item.EndDate;

        AngularAjaxCall($http, HomeCareSiteUrl.GetBatchDetailsURL, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.PatientClaimsList = response.Data;
                $scope.PatientClaimsList.Model = { BatchID: batchID }
                $("#model__ViewBatch").modal("show");
                ShowMessages(response);
            }
            else {
                ShowMessages(response);
            }
        });
    };

    $scope.TempSearchPatientList = {};
    $scope.GetChildNoteDetailsOfClaim = function (item, trElem, parentItem, forceLoad, forceLoadItemObj) {
        if (item.ChildNoteList == undefined || forceLoad == true) {
            $scope.TempSearchPatientList.ReferralID = item.ReferralID;
            $scope.TempSearchPatientList.BatchID = parentItem == undefined ? 0 : parentItem.BatchID;

            var jsonData = angular.toJson($scope.TempSearchPatientList);
            AngularAjaxCall($http, HomeCareSiteUrl.GetChildNoteDetailsURL, jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {

                    if (forceLoad) {


                        $.each(item.ChildNoteList, function (index, tempItem) {

                            var claim = undefined;
                            if (forceLoadItemObj != undefined) {
                                claim = response.Data.filter(function (data) { return data.NoteID == forceLoadItemObj.NoteID })[0];
                            }

                            if (claim != undefined && claim.NoteID > 0 && tempItem.NoteID == claim.NoteID) {


                                tempItem.CalculatedAmount = claim.CalculatedAmount;
                                tempItem.ClaimAllowedAmount = claim.ClaimAllowedAmount;
                                tempItem.ClaimBilledAmount = claim.ClaimBilledAmount;
                                tempItem.ClaimPaidAmount = claim.ClaimPaidAmount;

                                tempItem.ClaimAdjustmentReason = claim.ClaimAdjustmentReason;
                                tempItem.ClaimAdjustmentTypeID = claim.ClaimAdjustmentTypeID;

                                //$scope.$$phase || $scope.$apply();
                                return true;    //break out of the loop
                            }
                            else if (claim == undefined) {
                                $.each(response.Data, function (index, tempServerItem) {
                                    if (tempItem.BatchNoteID == tempServerItem.BatchNoteID) {
                                        tempItem.CalculatedAmount = tempServerItem.CalculatedAmount;
                                        tempItem.ClaimAllowedAmount = tempServerItem.ClaimAllowedAmount;
                                        tempItem.ClaimBilledAmount = tempServerItem.ClaimBilledAmount;
                                        tempItem.ClaimPaidAmount = tempServerItem.ClaimPaidAmount;

                                        tempItem.ClaimAdjustmentReason = tempServerItem.ClaimAdjustmentReason;
                                        tempItem.ClaimAdjustmentTypeID = tempServerItem.ClaimAdjustmentTypeID;
                                    }

                                });
                            }



                        });
                        if (!$scope.$root.$$phase) $scope.$apply();


                        if (forceLoadItemObj != undefined)
                            $scope.GetBatchNoteDetails(forceLoadItemObj.cnItem, forceLoad);


                    }
                    else {

                        item.ChildNoteList = response.Data;

                        $scope.ShowCollpase();
                        $scope.SetClaimsError();
                    }
                }
                ShowMessages(response);
                if (!$scope.$root.$$phase) $scope.$apply();
            });
        }

        //if ($(trElem).hasClass("fa-minus-circle") == false) {
        //    $(trElem).removeClass("fa-plus-circle").addClass("fa-minus-circle");
        //}
        //else {
        //    $(trElem).removeClass("fa-minus-circle").addClass("fa-plus-circle");
        //}
    };

};

controllers.LatestERAController.$inject = ['$scope', '$http'];

$(document).ready(function () {
    // Try to change the iframe size every 2 seconds
    setInterval(function () {
        vm.ResizeIframe('iframe_viewbatch');
    }, 500);
});