var vm;

controllers.ManageClaimsListController = function ($scope, $http, $timeout) {
    vm = $scope;
    //$scope.EdiFileLogModelListURL = SiteUrl.EdiFileLogModelListURL;
    $scope.ClaimsList = [];
    $scope.BatchList = [];
    $scope.SelectedClaimsListIds = [];
    $scope.SelectAllCheckbox = false;
    $scope.CurrentBatchUploadedClaimID = 0;
    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetManageClaimListPage").val());
    };
    $scope.ManageClaimModel = $.parseJSON($("#hdnSetManageClaimListPage").val());
    $scope.SearchClaimListPage = $scope.ManageClaimModel.ListManageClaimsModel;
    $scope.TempSearchClaimListPage = $scope.ManageClaimModel.ListManageClaimsModel;

    $scope.ClaimsListPager = new PagerModule("BatchUploadedClaimID", '', 'DESC');

    $scope.PayorNames = [];
    $scope.PayorIDs = [];
    angular.forEach($scope.ManageClaimModel.PayorList, function (data) {
        $scope.PayorNames.push(data.PayorName);
        $scope.PayorIDs.push(data.PayorID);
    });

    

    //var myChart2 = {};
    //var ctx2 = document.getElementById("myChart2").getContext('2d');
    //myChart2 = new Chart(ctx2, {
    //    type: 'bar',
    //    data: {
    //        labels: ['2006', '2007', '2008', '2009', '2010', '2011', '2012'],
    //        position: 'bottom',
    //        datasets: [
    //            {
    //                label: "Series A",
    //                data: [65, 59, 80, 81, 56, 55, 40]
    //            },
    //            {
    //                label: "Series B",
    //                data: [28, 48, 40, 19, 86, 27, 90]
    //            }
    //        ]
    //    },
    //    options: {
    //        elements: {
    //            rectangle: {
    //                borderWidth: 2
    //            }
    //        },
    //        responsive: true,
    //        legend: {
    //            position: 'bottom'
    //        }
    //    }
    //});

    //$scope.labels = ['2006', '2007', '2008', '2009', '2010', '2011', '2012'];
    //$scope.series = ['Series A', 'Series B'];

    //$scope.data = [
    //    [65, 59, 80, 81, 56, 55, 40],
    //    [28, 48, 40, 19, 86, 27, 90]
    //];

    $scope.SetPostData = function (fromIndex) {
        var pagermodel = {
            SearchClaimListPage: $scope.SearchClaimListPage,
            pageSize: $scope.ClaimsListPager.pageSize,
            pageIndex: fromIndex,
            sortIndex: $scope.ClaimsListPager.sortIndex,
            sortDirection: $scope.ClaimsListPager.sortDirection
        };
        return angular.toJson(pagermodel);
    };
    
    $scope.ClaimErrorDetails = [];
    $scope.GetClaimErrorDetails = function (item) {
        if (item.BatchUploadedClaimID > 0) {
            var model = {
                item: item
            };
            jsonData = angular.toJson(model);
            AngularAjaxCall($http, HomeCareSiteUrl.GetClaimErrorsListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                if (response.IsSuccess) {
                    $scope.ClaimErrorDetails[item.BatchUploadedClaimID] = response.Data;
                }
                ShowMessages(response);
            });
        }
    }
    $scope.EDIFileErrors = {
        payer_name: '',     //1
        payerid: '',        //1
        ins_number: '',      //1a
        pat_name_l: '',      //2
        pat_name_f: '',     //2
        pat_dob: '',         // 3
        pat_sex: '',        // 3
        ins_name_l: '',      //4
        ins_name_f: '',      //4
        pat_addr_1: '',      //5
        pat_addr_2: '',      //5
        pat_city: '',        //5
        pat_state: '',      //5
        pat_zip: '',         //5
        pat_country: '',    //5
        pat_rel: '',         //6
        ins_addr_1: '',             //7
        ins_addr_2: '',             //7
        ins_city: '',               //7
        ins_state: '',              //7
        ins_zip: '',                //7
        ins_country: '',            //7
        pat_marital: '',            //8
        pat_employment: '',          //8
        employment_related: '',      //10a
        auto_accident: '',           //10b
        auto_accident_state: '',     //10b
        auto_accident_country: '',   //10b
        other_accident: '',          //10c
        gen_cond_code: '',           //10d
        ins_group: '',              //11
        ins_dob: '',                //11a
        ins_sex: '',                //11a
        ins_employer: "",           //11b
        ins_plan: '',               //11c
        supv_prov_name_l: '',
        supv_prov_name_f: '',
        supv_prov_id: '',
        supv_prov_npi: '',
        cond_date: '',          //14
        ref_name_l: '',         //17
        ref_name_f: '',         //17
        ref_npi: '',            //17a
        ref_id: '',             //17a
        hosp_from_date: '',      //18
        hosp_thru_date: '',     //18
        narrative: '',           //19
        diag_1: '',             //21
        diag_2: '',
        diag_3: '',
        diag_4: '',
        diag_5: '',
        diag_6: '',
        diag_7: '',
        diag_8: '',
        diag_9: '',
        diag_10: '',
        diag_11: '',
        diag_12: '',
        icn_dcn_1: '',       //22
        prior_auth: '',       //23
        from_date_1: '',    //24a
        thru_date_1: '',    //24a
        from_date_2: '',    //24a
        thru_date_2: '',    //24a
        from_date_3: '',    //24a
        thru_date_3: '',    //24a
        from_date_4: '',    //24a
        thru_date_4: '',    //24a
        from_date_5: '',    //24a
        thru_date_5: '',    //24a
        from_date_6: '',    //24a
        thru_date_6: '',    //24a
        place_of_service_1: '', // 24b
        place_of_service_2: '', // 24b
        place_of_service_3: '', // 24b
        place_of_service_4: '', // 24b
        place_of_service_5: '', // 24b
        place_of_service_6: '', // 24b
        emergency_indicator_1: '', // 24c
        emergency_indicator_2: '', // 24c
        emergency_indicator_3: '', // 24c
        emergency_indicator_4: '', // 24c
        emergency_indicator_5: '', // 24c
        emergency_indicator_6: '', // 24c
        proc_code_1: '',    //24d
        proc_code_2: '',    //24d
        proc_code_3: '',    //24d
        proc_code_4: '',    //24d
        proc_code_5: '',    //24d
        proc_code_6: '',    //24d
        mod1_1: '',         //24d
        mod2_1: '',         //24d
        mod3_1: '',         //24d
        mod4_1: '',         //24d
        mod1_2: '',         //24d
        mod2_2: '',         //24d
        mod3_2: '',         //24d
        mod4_2: '',         //24d
        mod1_3: '',         //24d
        mod2_3: '',         //24d
        mod3_3: '',         //24d
        mod4_3: '',         //24d
        mod1_4: '',         //24d
        mod2_4: '',         //24d
        mod3_4: '',         //24d
        mod4_4: '',         //24d
        mod1_5: '',         //24d
        mod2_5: '',         //24d
        mod3_5: '',         //24d
        mod4_5: '',         //24d
        mod1_6: '',         //24d
        mod2_6: '',         //24d
        mod3_6: '',         //24d
        mod4_6: '',         //24d
        diag_ref_1: '',      //24e
        diag_ref_2: '',      //24e
        diag_ref_3: '',      //24e
        diag_ref_4: '',      //24e
        diag_ref_5: '',      //24e
        diag_ref_6: '',      //24e
        charge_1: '',        //24f
        charge_2: '',        //24f
        charge_3: '',        //24f
        charge_4: '',        //24f
        charge_5: '',        //24f
        charge_6: '',        //24f
        units_1: '',         //24g
        units_2: '',         //24g
        units_3: '',         //24g
        units_4: '',         //24g
        units_5: '',         //24g
        units_6: '',         //24g
        bill_taxid: '',         //25
        bill_taxid_type: '',    //25
        pcn: '',                //26
        accept_assign: '',      //27
        total_charge: '',       //28
        amount_paid: '',        //29
        balance_due: '',        //30
        prov_name_l: '',        //31
        prov_name_f: '',        //31
        prov_taxonomy: '',      //31
        prov_npi: '',           //31
        prov_id: '',            //31
        facility_name: '',       //32
        facility_addr_1: '',       //32
        facility_addr_2: '',       //32
        facility_city: '',       //32
        facility_state: '',       //32
        facility_zip: '',       //32
        facility_npi: '',       //32
        facility_id: '',       //32
        bill_name: '',       //33
        bill_addr_1: '',       //33
        bill_addr_2: '',       //33
        bill_city: '',       //33
        bill_state: '',       //33
        bill_zip: '',       //33
        bill_phone: '',       //33
        bill_taxonomy: '',       //33
        bill_npi: '',       // 33a
        bill_id: '',        // 33a
        bill_npi_api: '',   // 33a
    };
    $scope.SetEDIFileErrors = function (claim) {
        var Errors = $scope.ClaimErrorDetails[claim.BatchUploadedClaimID];
        var ErrorKeys = Object.keys($scope.EDIFileErrors);
        angular.forEach(Errors, function (item, key) {
            if (ErrorKeys.includes(item.Field)) {
                $scope.EDIFileErrors[item.Field] = item.Message;
            }
            if (item.Field == "bill_npi,bill_id") {
                $scope.EDIFileErrors["bill_npi_api"] = item.Message;
            }
        });
    };
    $scope.setErrorOnForm = function (field_class, error) {
        if (error != '' && error != undefined) {
            $(field_class).attr("data-original-title", error).attr("data-html", "true")
                .addClass("tooltip-danger input-validation-error").tooltip({ html: true })
        }
    }

    $scope.SaveCMS1500Modal = {
        BatchID: '',
        NoteID: '',
        PayorID: '',
        PayorIdentificationNumber: '',
        PayorName: '',
        ReferralID: '',
        AHCCCSID: '',
        ContactID: '',
        PatientAddress: '',
        PatientCity: '',
        PatientState: '',
        PatientZipCode: '',
        PatientPhone: '',
        PatientDOB: '',
        PhysicianID: '',
        BillingProviderNPI: '',
        ReferralBillingAuthorizationID: '',
        AuthorizationCode: '',
        ServiceDate: '',
        PlaceOfServiceID: '',
        PlaceOfService: '',
        Mod1_1_ID: '',
        Mod1_1_Code: '',
        Mod2_1_ID: '',
        Mod2_1_Code: '',
        Mod3_1_ID: '',
        Mod3_1_Code: '',
        Mod4_1_ID: '',
        Mod4_1_Code: '',
        Ref_NPI: '',
        BillingProviderEIN: ''
    };
    $scope.SetSaveCMS1500Modal = function (cms1500) {
        var Errors = $scope.ClaimErrorDetails[$scope.CurrentBatchUploadedClaimID];
        $scope.SaveCMS1500Modal.BatchUploadedClaimID = $scope.CurrentBatchUploadedClaimID;
        $scope.SaveCMS1500Modal.BatchID = cms1500.BatchID;
        $scope.SaveCMS1500Modal.NoteID = cms1500.NoteID;
        $scope.SaveCMS1500Modal.ContactID = cms1500.ContactID;
        $scope.SaveCMS1500Modal.PatientAddress = cms1500.Address;
        $scope.SaveCMS1500Modal.PatientCity = cms1500.City;
        $scope.SaveCMS1500Modal.PatientState = cms1500.State;
        $scope.SaveCMS1500Modal.PatientZipCode = cms1500.ZipCode;
        angular.forEach(Errors, function (item, key) {
            switch (item.Field) {
                case "payer_name":
                    $scope.SaveCMS1500Modal.PayorName = cms1500.PayorName;
                    $scope.SaveCMS1500Modal.PayorID = cms1500.PayorID;
                    break;
                case "payerid":
                    $scope.SaveCMS1500Modal.PayorIdentificationNumber = cms1500.PayorIdentificationNumber;
                    $scope.SaveCMS1500Modal.PayorID = cms1500.PayorID;
                    break;
                case "ins_number":
                    $scope.SaveCMS1500Modal.AHCCCSID = cms1500.AHCCCSID;
                    $scope.SaveCMS1500Modal.ReferralID = cms1500.ReferralID;
                    break;
                case "pat_dob":
                    $scope.SaveCMS1500Modal.PatientDOB = cms1500.DobYYYY + "-" + cms1500.DobMM + "-" + cms1500.DobDD;
                    $scope.SaveCMS1500Modal.ReferralID = cms1500.ReferralID;
                    break;
                case "prior_auth":
                    $scope.SaveCMS1500Modal.ReferralBillingAuthorizationID = $(".prior_auth_cms_id").text();
                    $scope.SaveCMS1500Modal.AuthorizationCode = $(".prior_auth_cms").val();
                    break;
                case "ref_npi":
                    $scope.SaveCMS1500Modal.PhysicianID = cms1500.PhysicianID;
                    $scope.SaveCMS1500Modal.Ref_NPI = cms1500.PhysicianNPINumber;
                    break;
                case "bill_npi,bill_id":
                    $scope.SaveCMS1500Modal.BillingProviderNPI = cms1500.BillingProviderNPI;
                    break;
                case "bill_npi":
                    $scope.SaveCMS1500Modal.BillingProviderNPI = cms1500.BillingProviderNPI;
                    break;
                case "from_date_1":
                    $scope.SaveCMS1500Modal.ServiceDate = $(".from_date_1_YY_cms").val() + "-" + $(".from_date_1_MM_cms").val() + "-" + $(".from_date_1_DD_cms").val();
                    break;
                case "from_date_2":
                    $scope.SaveCMS1500Modal.ServiceDate = $(".from_date_2_YY_cms").val() + "-" + $(".from_date_2_MM_cms").val() + "-" + $(".from_date_2_DD_cms").val();
                    break;
                case "from_date_3":
                    $scope.SaveCMS1500Modal.ServiceDate = $(".from_date_3_YY_cms").val() + "-" + $(".from_date_3_MM_cms").val() + "-" + $(".from_date_3_DD_cms").val();
                    break;
                case "from_date_4":
                    $scope.SaveCMS1500Modal.ServiceDate = $(".from_date_4_YY_cms").val() + "-" + $(".from_date_4_MM_cms").val() + "-" + $(".from_date_4_DD_cms").val();
                    break;
                case "from_date_5":
                    $scope.SaveCMS1500Modal.ServiceDate = $(".from_date_5_YY_cms").val() + "-" + $(".from_date_5_MM_cms").val() + "-" + $(".from_date_5_DD_cms").val();
                    break;
                case "from_date_6":
                    $scope.SaveCMS1500Modal.ServiceDate = $(".from_date_6_YY_cms").val() + "-" + $(".from_date_6_MM_cms").val() + "-" + $(".from_date_6_DD_cms").val();
                    break;
                case "thru_date_1":
                    $scope.SaveCMS1500Modal.ServiceDate = $(".thru_date_1_YY_cms").val() + "-" + $(".thru_date_1_MM_cms").val() + "-" + $(".thru_date_1_DD_cms").val();
                    break;
                case "thru_date_2":
                    $scope.SaveCMS1500Modal.ServiceDate = $(".thru_date_2_YY_cms").val() + "-" + $(".thru_date_2_MM_cms").val() + "-" + $(".thru_date_2_DD_cms").val();
                    break;
                case "thru_date_3":
                    $scope.SaveCMS1500Modal.ServiceDate = $(".thru_date_3_YY_cms").val() + "-" + $(".thru_date_3_MM_cms").val() + "-" + $(".thru_date_3_DD_cms").val();
                    break;
                case "thru_date_4":
                    $scope.SaveCMS1500Modal.ServiceDate = $(".thru_date_4_YY_cms").val() + "-" + $(".thru_date_4_MM_cms").val() + "-" + $(".thru_date_4_DD_cms").val();
                    break;
                case "thru_date_5":
                    $scope.SaveCMS1500Modal.ServiceDate = $(".thru_date_5_YY_cms").val() + "-" + $(".thru_date_5_MM_cms").val() + "-" + $(".thru_date_5_DD_cms").val();
                    break;
                case "thru_date_6":
                    $scope.SaveCMS1500Modal.ServiceDate = $(".thru_date_6_YY_cms").val() + "-" + $(".thru_date_6_MM_cms").val() + "-" + $(".thru_date_6_DD_cms").val();
                    break;
                case "place_of_service_1":
                    $scope.SaveCMS1500Modal.PlaceOfServiceID = $(".place_of_service_1_cms_id").text();
                    $scope.SaveCMS1500Modal.PlaceOfService = $(".place_of_service_1_cms").val();
                    break;
                case "place_of_service_2":
                    $scope.SaveCMS1500Modal.PlaceOfServiceID = $(".place_of_service_2_cms_id").text();
                    $scope.SaveCMS1500Modal.PlaceOfService = $(".place_of_service_2_cms").val();
                    break;
                case "place_of_service_3":
                    $scope.SaveCMS1500Modal.PlaceOfServiceID = $(".place_of_service_3_cms_id").text();
                    $scope.SaveCMS1500Modal.PlaceOfService = $(".place_of_service_3_cms").val();
                    break;
                case "place_of_service_4":
                    $scope.SaveCMS1500Modal.PlaceOfServiceID = $(".place_of_service_4_cms_id").text();
                    $scope.SaveCMS1500Modal.PlaceOfService = $(".place_of_service_4_cms").val();
                    break;
                case "place_of_service_5":
                    $scope.SaveCMS1500Modal.PlaceOfServiceID = $(".place_of_service_5_cms_id").text();
                    $scope.SaveCMS1500Modal.PlaceOfService = $(".place_of_service_5_cms").val();
                    break;
                case "place_of_service_6":
                    $scope.SaveCMS1500Modal.PlaceOfServiceID = $(".place_of_service_6_cms_id").text();
                    $scope.SaveCMS1500Modal.PlaceOfService = $(".place_of_service_6_cms").val();
                    break;
                case "mod1_1":
                    $scope.SaveCMS1500Modal.Mod1_1_ID = $(".mod1_1_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod1_1_Code = $(".mod1_1_cms").val();
                    break;
                case "mod2_1":
                    $scope.SaveCMS1500Modal.Mod2_1_ID = $(".mod2_1_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod2_1_Code = $(".mod2_1_cms").val();
                    break;
                case "mod3_1":
                    $scope.SaveCMS1500Modal.Mod3_1_ID = $(".mod3_1_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod3_1_Code = $(".mod3_1_cms").val();
                    break;
                case "mod4_1":
                    $scope.SaveCMS1500Modal.Mod4_1_ID = $(".mod4_1_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod4_1_Code = $(".mod4_1_cms").val();
                    break;
                case "mod1_2":
                    $scope.SaveCMS1500Modal.Mod1_1_ID = $(".mod1_2_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod1_1_Code = $(".mod1_2_cms").val();
                    break;
                case "mod2_2":
                    $scope.SaveCMS1500Modal.Mod2_1_ID = $(".mod2_2_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod2_1_Code = $(".mod2_2_cms").val();
                    break;
                case "mod3_2":
                    $scope.SaveCMS1500Modal.Mod3_1_ID = $(".mod3_2_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod3_1_Code = $(".mod3_2_cms").val();
                    break;
                case "mod4_2":
                    $scope.SaveCMS1500Modal.Mod4_1_ID = $(".mod4_2_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod4_1_Code = $(".mod4_2_cms").val();
                    break;
                case "mod1_3":
                    $scope.SaveCMS1500Modal.Mod1_1_ID = $(".mod1_3_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod1_1_Code = $(".mod1_3_cms").val();
                    break;
                case "mod2_3":
                    $scope.SaveCMS1500Modal.Mod2_1_ID = $(".mod2_3_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod2_1_Code = $(".mod2_3_cms").val();
                    break;
                case "mod3_3":
                    $scope.SaveCMS1500Modal.Mod3_1_ID = $(".mod3_3_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod3_1_Code = $(".mod3_3_cms").val();
                    break;
                case "mod4_3":
                    $scope.SaveCMS1500Modal.Mod4_1_ID = $(".mod4_3_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod4_1_Code = $(".mod4_3_cms").val();
                    break;
                case "mod1_4":
                    $scope.SaveCMS1500Modal.Mod1_1_ID = $(".mod1_4_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod1_1_Code = $(".mod1_4_cms").val();
                    break;
                case "mod2_4":
                    $scope.SaveCMS1500Modal.Mod2_1_ID = $(".mod2_4_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod2_1_Code = $(".mod2_4_cms").val();
                    break;
                case "mod3_4":
                    $scope.SaveCMS1500Modal.Mod3_1_ID = $(".mod3_4_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod3_1_Code = $(".mod3_4_cms").val();
                    break;
                case "mod4_4":
                    $scope.SaveCMS1500Modal.Mod4_1_ID = $(".mod4_4_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod4_1_Code = $(".mod4_4_cms").val();
                    break;
                case "mod1_5":
                    $scope.SaveCMS1500Modal.Mod1_1_ID = $(".mod1_5_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod1_1_Code = $(".mod1_5").val();
                    break;
                case "mod2_5":
                    $scope.SaveCMS1500Modal.Mod2_1_ID = $(".mod2_5_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod2_1_Code = $(".mod2_5_cms").val();
                    break;
                case "mod3_5":
                    $scope.SaveCMS1500Modal.Mod3_1_ID = $(".mod3_5_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod3_1_Code = $(".mod3_5_cms").val();
                    break;
                case "mod4_5":
                    $scope.SaveCMS1500Modal.Mod4_1_ID = $(".mod4_5_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod4_1_Code = $(".mod4_5_cms").val();
                    break;
                case "mod1_6":
                    $scope.SaveCMS1500Modal.Mod1_1_ID = $(".mod1_6_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod1_1_Code = $(".mod1_6_cms").val();
                    break;
                case "mod2_6":
                    $scope.SaveCMS1500Modal.Mod2_1_ID = $(".mod2_6_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod2_1_Code = $(".mod2_6_cms").val();
                    break;
                case "mod3_6":
                    $scope.SaveCMS1500Modal.Mod3_1_ID = $(".mod3_6_cms_id").text();
                    $scope.SaveCMS1500Modal.Mod3_1_Code = $(".mod3_6_cms").val();
                    break;
                case "mod4_6":
                    $scope.SaveCMS1500Modal.Mod4_1_ID = $(".mod4_6").text();
                    $scope.SaveCMS1500Modal.Mod4_1_Code = $(".mod4_6_cms").val();
                    break;
                case "bill_taxid":
                    $scope.SaveCMS1500Modal.BillingProviderEIN = $(".bill_taxid_cms").val();
                    break;
            }
        });
    };

    $scope.EDIFileSearchModel = {};
    $scope.ShowCMS1500File = function (claim) {
        $scope.EDIFileSearchModel.PayorID = claim.PayorID;
        $scope.EDIFileSearchModel.ReferralID = claim.ReferralID;
        $scope.EDIFileSearchModel.BatchID = claim.BatchID;
        $scope.EDIFileSearchModel.FileType = 'CMS1500';

        $scope.EDIFileSearchModel.BatchTypeID = claim.BatchTypeID;
        $scope.EDIFileSearchModel.StartDate = null;
        $scope.EDIFileSearchModel.EndDate = null;
        $scope.EDIFileSearchModel.ClientName = null;
        $scope.EDIFileSearchModel.ServiceCodeIDs = '';

        //if ($scope.BatchModel.SearchPatientList.ServiceCodeID)
        //    $scope.EDIFileSearchModel.ServiceCodeIDs = $scope.BatchModel.SearchPatientList.ServiceCodeID.toString();
        //else
        //    $scope.EDIFileSearchModel.ServiceCodeIDs = '';

        var model = {
            item: claim,
            EDIFileSearchModellong: $scope.EDIFileSearchModel
        };

        var jsonData = angular.toJson(model);

        AngularAjaxCall($http, HomeCareSiteUrl.GetClaimErrorsListAndCMS1500, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                $scope.CurrentBatchUploadedClaimID = claim.BatchUploadedClaimID;
                if ($scope.ClaimErrorDetails[claim.BatchUploadedClaimID] == null) {
                    $scope.ClaimErrorDetails[claim.BatchUploadedClaimID] = response.Data.ClaimErrors;
                }
                $scope.SetEDIFileErrors(claim);
                $scope.Cms1500List = response.Data.CMS1500;
                $scope.BatchList = response.Data.BatchList;
                $("#generateCMS1500").modal('show');
                setTimeout(function () {
                    var ErrorKeys = Object.keys($scope.EDIFileErrors);
                    angular.forEach(ErrorKeys, function (item, key) {
                        $scope.setErrorOnForm("." + item + "_cms", $scope.EDIFileErrors[item]);
                    });
                }, 1000)
            }
            else {
                ShowMessages(response);
            }
        });
    };
    $scope.HideCMS1500File = function () {
        $("#generateCMS1500").modal('hide');
    }
    $scope.SaveCMS1500File = function (CMSItem) {
        $scope.AjaxStart = true;
        $scope.SetSaveCMS1500Modal(CMSItem);
        var model = {
            saveCMS1500Modal: $scope.SaveCMS1500Modal,
            cmsModel: CMSItem
        }
        var jsonData = angular.toJson($scope.SaveCMS1500Modal, CMSItem);
        AngularAjaxCall($http, HomeCareSiteUrl.SaveCMS1500DataURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                response.Message = "Claim Data Saved Successfully.Please submit claim again.";
                $scope.HideCMS1500File();
                $scope.ValidateAndGenerateEdi837(false, ".txt", CMSItem.BatchID, 1, "P");
            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };

    $scope.GetClaimsList = function () {
        $scope.AjaxStart = true;
        var jsonData = $scope.SetPostData($scope.ClaimsListPager.currentPage);

        AngularAjaxCall($http, HomeCareSiteUrl.GetClaimsListURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
            if (response.IsSuccess) {
                if(response.Data) {
                    $scope.ClaimsList = response.Data.Items;
                    $scope.ClaimsListPager.currentPageSize = response.Data.Items.length;
                    $scope.ClaimsListPager.totalRecords = response.Data.TotalItems;
                    $scope.ShowCollpase();

                    // Chart implementation starts
                    $scope.ProcessedData = [];
                    $scope.RejectedData = [];
                    $scope.AcknowledgedData = [];
                    $scope.TransmittedData = [];

                    angular.forEach($scope.ManageClaimModel.PayorList, function (data) {
                        if ($scope.ClaimsList !== null && $scope.ClaimsList.length > 0) {
                            $scope.ProcessedData.push($scope.ClaimsList.filter(item => item.PayorID === data.PayorID && item.Status === 'P').length);
                            $scope.RejectedData.push($scope.ClaimsList.filter(item => item.PayorID === data.PayorID && item.Status === 'R').length);
                            $scope.AcknowledgedData.push($scope.ClaimsList.filter(item => item.PayorID === data.PayorID && item.Status === 'A').length);
                            $scope.TransmittedData.push($scope.ClaimsList.filter(item => item.PayorID === data.PayorID && item.Status === 'T').length);
                        }
                        else {
                            $scope.ProcessedData.push(0);
                            $scope.RejectedData.push(0);
                            $scope.AcknowledgedData.push(0);
                            $scope.TransmittedData.push(0);
                        }
                    });

                    var myChart = {};
                    if (myChart.destroy)
                        myChart.destroy();
                    var ctx = document.getElementById("myChart").getContext('2d');
                    myChart = new Chart(ctx, {
                        type: 'bar',
                        data: {
                            labels: $scope.PayorNames,
                            datasets: [
                                {
                                    label: "Processed",
                                    data: $scope.ProcessedData,
                                    backgroundColor: 'rgba(59, 241, 135, 0.76)'
                                },
                                {
                                    label: "Rejected",
                                    data: $scope.RejectedData,
                                    backgroundColor: 'rgba(241, 80, 59, 0.76)'
                                },
                                {
                                    label: "Acknowledged",
                                    data: $scope.AcknowledgedData,
                                    backgroundColor: 'rgba(51, 192, 244, 0.6)'
                                },
                                {
                                    label: "Transmitted",
                                    data: $scope.TransmittedData,
                                    backgroundColor: 'rgba(56, 89, 255, 0.6)'
                                }
                            ]
                        },
                        options: {
                            elements: {
                                rectangle: {
                                    borderWidth: 2
                                }
                            },
                            responsive: true,
                            legend: {
                                position: 'top'
                            }
                        }
                    });
                    // Chart implementation ends
                }
            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    };

    $scope.ClaimsListPager.getDataCallback = $scope.GetClaimsList;
    $scope.ClaimsListPager.getDataCallback();

    $scope.ResetSearchFilter = function () {
        $timeout(function () {
            $scope.SearchClaimListPage = $scope.newInstance().SearchClaimListPage;
            $scope.TempSearchClaimListPage = $scope.newInstance().SearchClaimListPage;
            $scope.SearchClaimListPage.ReferralID = '';
            $scope.SearchClaimListPage.PayorID = '';
            $scope.SearchClaimListPage.BatchID = '';
            $scope.SearchClaimListPage.INS_Number = '';
            $scope.SearchClaimListPage.StartDate = '';
            $scope.SearchClaimListPage.EndDate = '';

            $scope.ClaimsListPager.currentPage = 1;
            $scope.ClaimsListPager.getDataCallback();
        });
    };

    $scope.SearchUpload835Files = function () {
        $scope.ClaimsListPager.currentPage = 1;
        $scope.ClaimsListPager.getDataCallback(true);
    };

    $scope.GetPatientClaims = function (item, trElem) {
        $(trElem).toggleClass("openPatientClaims");
        $scope.ShowCollpase();
    };

    $scope.ShowCollpase = function () {
        setTimeout(function () {
            $.each($('.collapseDestination'), function (index, data) {
                $(this).on('show.bs.collapse', function () {
                    $(this).parents("tbody").find(".collapseSource").removeClass("fa-plus-circle").addClass("fa-minus-circle");
                });

                $(this).on('hidden.bs.collapse', function () {
                    $(this).parents("tbody").find(".collapseSource").removeClass("fa-minus-circle").addClass("fa-plus-circle");
                });

            });

        }, 100);
    };
    $scope.ShowCollpase();

    // EDI 837 Files Validation And Generation
    //#region 

    $scope.ValidateAndGenerateEdi837Model = {};

    $scope.FilterBatchForValidation = function (item) {
        var returnValue = false;
        if ($scope.ListOfIdsInCSV != undefined) {
            $.each($scope.ListOfIdsInCSV.split(','), function (index, data) {
                if (item.BatchID == data)
                    returnValue = true;
            });
        }
        return returnValue;
    };
    $scope.TempBatchIds = [];
    $scope.ValidateAndGenerateEdi837 = function (validateOnly, fileExtension, batchId, ediFileType, ediFileTypeName) {
        $scope.TempBatchIds = [];
        $scope.ListOfIdsInCSV = batchId > 0 ? batchId.toString() : 0;
        var msg = window.ValidateAndGenerateEDI837;
        $scope.ValidateAndGenerateEdi837Model.PageTitle = msg.format(ediFileTypeName);
        $scope.ValidateAndGenerateEdi837Model.ValidateOnly = validateOnly;
        $scope.ValidateAndGenerateEdi837Model.ValidateWaitText = window.ValidateWaitText;
        $scope.ValidateAndGenerateEdi837Model.FileExtension = fileExtension;
        $scope.ValidateAndGenerateEdi837Model.EdiFileType = ediFileType;
        $scope.ValidateAndGenerateEdi837Model.FilteredBatchList = $scope.BatchList.filter(function (item) {
            if (ediFileType == 1) {
                if (item.PayorBillingType == 'Professional') {
                    if ($scope.FilterBatchForValidation(item)) {
                        $scope.TempBatchIds.push(item.BatchID);
                        return true;
                    }
                    return false;
                }
            }
            else {
                if (item.PayorBillingType == 'Institutional') {
                    if ($scope.FilterBatchForValidation(item)) {
                        $scope.TempBatchIds.push(item.BatchID);
                        return true;
                    }
                    return false;
                }
            }
        });
        $("#model__ValidateAndGenerateEdi837").modal('show');
    };

    $scope.SubmitClaim = function (ele) {
        $(ele).button('loading');
        if ($scope.ValidateAndGenerateEdi837Model.FilteredBatchList != null && $scope.ValidateAndGenerateEdi837Model.FilteredBatchList != undefined && $scope.ValidateAndGenerateEdi837Model.FilteredBatchList.length > 0) {
            angular.forEach($scope.ValidateAndGenerateEdi837Model.FilteredBatchList, function (item, key) {
                if (item.ValidationPassed == true && item.Edi837GenerationPassed == true) {
                    var url = "/hc/batch/SubmitClaim/";
                    var jsonData = angular.toJson({
                        claimModel: {
                            BatchID: item.BatchID,
                            FileName: item.FileName,
                            EdiFileTypeID: item.EdiFileTypeID,
                            Edi837GenerationPassed: item.Edi837GenerationPassed,
                            Edi837FilePath: item.Edi837FilePath,
                            ValidationPassed: item.ValidationPassed,
                            ValidationErrorFilePath: item.ValidationErrorFilePath,
                        }
                    });
                    AngularAjaxCall($http, url, jsonData, "Post", "json", "application/json", false).success(function (response) {
                        $(ele).button('reset');
                        ShowMessages(response);
                        $scope.HideSubmitClaimButton();
                        SetMessageForPageLoad(response.Message, "ShowClaimUploadMessage");
                        setTimeout(function () {
                            window.location.reload();
                        }, 1000);
                    });
                }
            });
        }
        else {
            $(ele).button('reset');
            $scope.HideSubmitClaimButton();
        }
    }

    $scope.DoEdi837Action = function (ele) {
        $scope.ListOfIdsInCSV = $scope.TempBatchIds.length > 0 ? $scope.TempBatchIds.toString() : '';
        $scope.ValidateAndGenerateEdi837Model.ShowLoader = true;
        $(ele).button('loading');

        var url = $scope.ValidateAndGenerateEdi837Model.ValidateOnly ? HomeCareSiteUrl.ValidateBatchesURL : HomeCareSiteUrl.GenerateEdi837FilesURL;

        var jsonData = angular.toJson({
            postEdiValidateGenerateModel: {
                ListOfBacthIdsInCsv: $scope.ListOfIdsInCSV,
                FileExtension: $scope.ValidateAndGenerateEdi837Model.FileExtension,
                GenerateEdiFile: !$scope.ValidateAndGenerateEdi837Model.ValidateOnly,
                EdiFileType: $scope.ValidateAndGenerateEdi837Model.EdiFileType
            }
        });

        AngularAjaxCall($http, url, jsonData, "Post", "json", "application/json", false).success(function (response) {
            $(ele).button('reset');
            $scope.ValidateAndGenerateEdi837Model.ShowLoader = false;
            if (response.IsSuccess) {
                if (response.Data) {
                    $scope.ValidateAndGenerateEdi837Model.FilteredBatchList.filter(function (item) {
                        response.Data.filter(function (returnItem) {
                            if (item.BatchID == returnItem.BatchID) {
                                item.ShowResult = true;
                                item.FileName = returnItem.FileName;
                                item.ValidationPassed = returnItem.ValidationPassed;
                                item.ValidationErrorFilePath = returnItem.ValidationErrorFilePath;
                                item.Edi837GenerationPassed = returnItem.Edi837GenerationPassed;
                                item.Edi837FilePath = returnItem.Edi837FilePath;
                            }
                        });
                    });

                    angular.forEach($scope.ValidateAndGenerateEdi837Model.FilteredBatchList, function (item, key) {
                        if (item.ValidationPassed == true && item.Edi837GenerationPassed == true) {
                            $scope.ShowSubmitClaimButton();
                        }
                    });

                    $scope.$$phase || $scope.$apply();
                }
            }
            else {
                ShowMessages(response);
            }
        });
    };

    $('#model__ValidateAndGenerateEdi837').on('hidden.bs.modal', function () {
        $scope.ValidateAndGenerateEdi837Model.FilteredBatchList.filter(function (item) {
            item.ShowResult = false;
            item.FileName = null;
            item.ValidationPassed = null;
            item.ValidationErrorFilePath = null;
            item.Edi837GenerationPassed = null;
            item.Edi837FilePath = null;
        });
    });

    $scope.HideSubmitClaimButton = function () {
        $("#btnSubmitClaim").hide();
        $("#btnConfirm").show();
    }

    $scope.ShowSubmitClaimButton = function () {
        $("#btnConfirm").hide();
        $("#btnSubmitClaim").show();
    }

    //#endregion    
};

controllers.ManageClaimsListController.$inject = ['$scope', '$http', '$timeout'];

$(document).ready(function () {
    ShowPageLoadMessage("ShowClaimUploadMessage");
});