var custModel;
controllers.CronJobController = function ($scope, $http) {

    custModel = $scope;
    var model = $.parseJSON($("#hdnCronJobServiceModel").val());

    $scope.CronJobServiceProgressModel = {};
    $scope.ServiceCompleted = 0;

    $scope.InitiateService = function () {

        if (model != undefined && model.ServiceURL != undefined) {

            jQuery.ajax({
                method: "Post",
                url: model.ServiceURL,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (a, b, c) {
                    debugger;
                    var d = 1;
                },

                success: function (response, textStatus, XMLHttpRequest) {
                    response= JSON.parse(response);
                    //$scope.ServiceFinalStatus = response.Message;

                   // $scope.CronJobServiceProgressModel.PercentComplete == '100';
                    //clearInterval(refreshIntervalId);


                    if (!$scope.$root.$$phase) $scope.$apply();
                },
                async: true
            });

            //AngularAjaxCall($http, model.ServiceURL, null, "Post", "json", "application/json").success(function (response) {
            //    if (response.IsSuccess) {
            //        $scope.ServiceFinalStatus = response.Message;

            //        $scope.CronJobServiceProgressModel.PercentComplete == '100';
            //        clearInterval(refreshIntervalId);

            //    }

            //    ShowMessages(response);
            //});
        }
        else {
            console.log("Service URL is not defined.")
        }
    };

    

    $scope.Service_ProgressStatusUpdate = function () {

        if (model != undefined && model.ServiceProgressURL != undefined) {
            $scope.ServiceCompleted = 1;

            jQuery.ajax({
                method: "Post",
                url: model.ServiceProgressURL,
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                error: function (a, b, c) {
                    debugger;
                    var d = 1;
                },


                success: function (response, textStatus, XMLHttpRequest) {
                    
                    response = JSON.parse(response);

                    if (response.Data != null)
                        $scope.CronJobServiceProgressModel = response.Data;


                    if (!$scope.$root.$$phase) $scope.$apply();

                    if ($scope.CronJobServiceProgressModel != null && $scope.CronJobServiceProgressModel.PercentComplete == '100') {
                        $scope.ServiceCompleted = 2;
                    }
                    else {
                        setTimeout($scope.Service_ProgressStatusUpdate,2000);
                    }
                    if (!$scope.$root.$$phase) $scope.$apply();
                    
                },
                async: true
            });



            //AngularAjaxCall($http, model.ServiceProgressURL, null, "Post", "json", "application/json", false).success(function (response) {
            //    if (response.IsSuccess) {
            //        $scope.CronJobServiceProgressModel = response.Data;

            //        if ($scope.CronJobServiceProgressModel.PercentComplete == '100') {
            //            clearInterval(refreshIntervalId);
            //        }
            //    }
            //});
        }
        else {
            console.log("Service Progress URL is not defined.")
        }
    };




   
    
    $scope.InitiateService();
    $scope.Service_ProgressStatusUpdate();

};
controllers.CronJobController.$inject = ['$scope', '$http'];

$(document).ready(function () {



});
