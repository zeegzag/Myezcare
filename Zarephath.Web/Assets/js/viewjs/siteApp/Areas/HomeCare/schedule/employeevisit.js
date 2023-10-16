var vm;

controllers.EmployeeVisitController = function ($scope, $http, $window, $timeout) {
    vm = $scope;

    $scope.newInstance = function () {
        return $.parseJSON($("#hdnSetEmployeeVisitsPage").val());
    };
    //$scope.datetimeformat = dateformat + ' hh:mm a';
    $scope.ReferralEmployeeModelList = [];
    $scope.ReferralSelected = {};
    $scope.VirtualVisitsList = [];
    $scope.IsToday = false;
    
    $scope.SelectAllCheckbox = false;
    $scope.EmployeeVisitsModel = $.parseJSON($("#hdnSetEmployeeVisitsPage").val());
    $scope.SearchReferralEmployeeModel = $scope.newInstance().SearchReferralEmployeeModel;
    //$scope.SaveEmployeeVisitsTransportLog = $scope.newInstance().SaveEmployeeVisitsTransportLog;
    $scope.SaveEmployeeVisitsTransportLog = {};
    $scope.TempSearchReferralEmployeeModel = $scope.newInstance().SearchReferralEmployeeModel;
    $scope.SearchReferralNotesModel = {};

    $scope.directionsService;
    $scope.directionsRenderer;
    $scope.map;

    $scope.GetReferralNotesByModelURL = HomeCareSiteUrl.GetReferralNotesByModelURL;

    $scope.VirtualVisitsListPager = new PagerModule("EmployeeName");

    $scope.ActiveTabId = function (fromIndex, model) {
        return $("ul#visitTypes li.active").first().attr('id');
    };
    $scope.$watch('SearchReferralEmployeeModel.SlotDate', function () {
        if (moment().startOf('day').format('') == moment($scope.SearchReferralEmployeeModel.SlotDate).startOf('day').format('')) {
            $scope.IsToday = true;
        }
        else {
            $scope.IsToday = false;
        }
        $scope.change();
    })
    $scope.change = function () {
        $scope.SearchModelMapping();
    };

    $scope.SearchModelMapping = function () {
        var jsonData = angular.toJson($scope.SearchReferralEmployeeModel);
        $scope.AjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.HC_GetReferralEmployeeVisitsURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
            if (response.IsSuccess) {
                $scope.ReferralEmployeeModelList = response.Data;
                if ($scope.ReferralEmployeeModelList.length > 0) {
                   $scope.googlemapInit($scope.ReferralEmployeeModelList[0]);
                }
            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
        
       
    };
    $scope.SearchModelMapping();
    $scope.ViewReferral = function (item) {
        $scope.ReferralSelected = item;
        $('#view').modal('show');
    }
    $scope.getLocation = function (callback) {
        try {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(callback, callback);
            } else {
                //x.innerHTML = "Geolocation is not supported by this browser.";
                $scope.SavePickUpDropCall(null);
            }
        }
        catch (err) {
            $scope.SavePickUpDropCall(null);
        }
    }
    $scope.SavePickUpDrop = function (isPickup) {
        var sevt = $scope.SaveEmployeeVisitsTransportLog;
        sevt.EmployeeID = $scope.SearchReferralEmployeeModel.EmployeeID;
        sevt.TransportationType = $scope.SearchReferralEmployeeModel.TransportationType;
        sevt.TransportGroupID = $scope.ReferralSelected.TransportGroupID||null;
        sevt.TransportAssignPatientID = $scope.ReferralSelected.TransportAssignPatientID||null;
        sevt.EmployeeVisitsTransportLogId = $scope.ReferralSelected.EmployeeVisitsTransportLogId;
        sevt.EmployeeVisitsTransportLogDetailId = $scope.ReferralSelected.EmployeeVisitsTransportLogDetailId;
        sevt.ReferralID = $scope.ReferralSelected.ReferralID;
        if (isPickup == 1) {
            sevt.ClockInTime = moment.utc().format();
        } else {
            sevt.ClockOutTime = moment.utc().format();
        }
        $scope.getLocation($scope.SavePickUpDropCall);
    }
    $scope.SavePickUpDropCall = function (position) {
        //position.coords.latitude + "<br>Longitude: " + position.coords.longitude
        var sevt = $scope.SaveEmployeeVisitsTransportLog;
        if (position.code == undefined) {
            sevt.Latitude = position.coords.latitude;
            sevt.Longitude = position.coords.longitude;
        }
        var jsonData = angular.toJson(sevt);
        $scope.AjaxStart = true;
        AngularAjaxCall($http, HomeCareSiteUrl.SavePickUpDropCallURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
            if (response.IsSuccess) {
                $scope.SearchModelMapping();
                $('#view').modal('hide');
            }
            $scope.AjaxStart = false;
            ShowMessages(response);
        });
    }
    $scope.GetVirtualVisitsList = function () {
        ////Reset Selcted Checkbox items and Control
        //$scope.SelectedVirtualVisitsIds = [];
        //$scope.SelectAllCheckbox = false;
        ////Reset Selcted Checkbox items and Control
        //$scope.AjaxStart = true;
        //var jsonData = $scope.SetPostData($scope.VirtualVisitsListPager.currentPage);
        //AngularAjaxCall($http, HomeCareSiteUrl.HC_GetVirtualVisitsListURL, jsonData, "Post", "json", "application/json", true).success(function (response) {
        //    if (response.IsSuccess) {
        //        $scope.VirtualVisitsList = response.Data.Items;
        //        $scope.VirtualVisitsListPager.currentPageSize = response.Data.Items.length;
        //        $scope.VirtualVisitsListPager.totalRecords = response.Data.TotalItems;
        //        //$scope.ShowCollpase();
        //    }
        //    $scope.AjaxStart = false;
        //    ShowMessages(response);
        //});
    };

    $scope.Refresh = function () {
        //$scope.VirtualVisitsListPager.getDataCallback();
    };
    
    $scope.googlemapInit = function (referral) {
        EmployeeVisitShowMap(referral);
        EmployeeVisitAddMarker($scope.ReferralEmployeeModelList);
        //$scope.ReferralEmployeeModelList
        //if ($scope.directionsService == undefined) {
        //    $scope.directionsService = new google.maps.DirectionsService();
        //}
        //if ($scope.directionsRenderer == undefined) {
        //    $scope.directionsRenderer = new google.maps.DirectionsRenderer();
        //}
        //$scope.map = new google.maps.Map(document.getElementById("map"), {
        //    zoom: 10,
        //    center: { lat: referral.Latitude, lng: referral.Longitude },
        //});
        //setTimeout(function () {
        //    debugger;
        //    var mapvar = $scope.map;
        //    const infoWindow = new google.maps.InfoWindow();
        //    $scope.ReferralEmployeeModelList.forEach((referralitem, i) => {
        //        debugger;
        //        var position = { lat: referralitem.Latitude, lng: referralitem.Longitude };
        //        var title = referralitem.Name;
        //        const marker = new google.maps.Marker({
        //            position,
        //            mapvar,
        //            title: `${i + 1}. ${title}`,
        //            label: `${i + 1}`,
        //            optimized: false,
        //        });

        //        // Add a click listener for each marker, and set up the info window.
        //        marker.addListener("click", () => {
        //            infoWindow.close();
        //            infoWindow.setContent(marker.getTitle());
        //            infoWindow.open(marker.getMap(), marker);
        //        });

        //    });
        //}, 2000);
        
    }

    $scope.calculateAndDisplayRoute = function () {
        var directionsService = $scope.directionsService;
        var directionsRenderer = $scope.directionsRenderer;
        const waypts = [];

        for (let i = 0; i < $scope.ReferralEmployeeModelList.length; i++) {
            if ($scope.ReferralEmployeeModelList[i].Latitude != null)
                waypts.push({
                    location: $scope.ReferralEmployeeModelList[i].Latitude + ', ' + $scope.ReferralEmployeeModelList[i].Longitude,
                    stopover: true,
                });
        }
        
        $scope.directionsService
            .route({
                origin: $scope.ReferralEmployeeModelList[0].Latitude + ', ' + $scope.ReferralEmployeeModelList[0].Longitude,
                destination: $scope.ReferralEmployeeModelList[0].Latitude + ', ' + $scope.ReferralEmployeeModelList[0].Longitude,
                waypoints: waypts,
                optimizeWaypoints: true,
                travelMode: google.maps.TravelMode.DRIVING,
            })
            .then((response) => {
                $scope.directionsRenderer.setDirections(response);

            })
            .catch((e) => window.alert("Directions request failed due to " + status));
    }

    $scope.VirtualVisitsListPager.getDataCallback = $scope.GetVirtualVisitsList;
    $scope.VirtualVisitsListPager.getDataCallback();
    if (moment().startOf('day').format('') == moment($scope.SearchReferralEmployeeModel.SlotDate).startOf('day').format('')) {
        $scope.IsToday = true;
    }
    $scope.Openmap = function (ReferralSelected) {
        openMapApp('' + ReferralSelected.Latitude +','+ReferralSelected.Longitude,'');
    }
    $('#visitTypes a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $scope.ResetSearchFilter();
    });
    
    
};
controllers.VirtualVisitsListController.$inject = ['$scope', '$http', '$window', '$timeout'];
