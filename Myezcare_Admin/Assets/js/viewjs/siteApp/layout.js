controllers.LayoutDetailController = function ($scope, $http) {

    //$scope.ReferralURL = SiteUrl.AddReferralPageUrl;
    //$scope.NoteUrl = SiteUrl.NoteUrl;

    //$scope.GetLayoutRelatedDetails = function () {

        //var newCheckTime = GetCookie("NewCheckTime");
        //if (newCheckTime == "null")
        //    newCheckTime = null;
        

        //var searchModel = {
        //    PageSize: 50,
        //    AssigneeLastCheckTime: newCheckTime,
        //    ResolvedLastCheckTime: newCheckTime,
        //};
        //var jsonData = angular.toJson(searchModel);

        //AngularAjaxCall($http, SiteUrl.GetLayoutRelatedDetailsUrl, jsonData, "Post", "json", "application/json",false).success(function (response) {
        //    $scope.LayoutDetailModel = response.Data;
        //    SetCookie($scope.LayoutDetailModel.NewCheckTime, "NewCheckTime");

        //    if (response.Data.NewMessagesCount > 0) {
        //        ShowMessage(response.Data.NewMessagesCountMessage, "info", 0, 0);
        //        GenerateDesktopNotification(response.Data.NewMessagesCountMessage, HomeCareSiteUrl.GetDashboardUrl);
        //    }
        //    if (response.Data.ResolvedMessagesCount > 0) {
        //        ShowMessage(response.Data.ResolvedMessagesCountMessage, "warning", 0, 0);
        //        GenerateDesktopNotification(response.Data.ResolvedMessagesCountMessage, HomeCareSiteUrl.GetDashboardUrl);
        //    }
        //});
    //};

    // 1 MIN = 60000 MS
    //setInterval(function () {
    //    $scope.GetLayoutRelatedDetails();
    //}, 60000);
    
    //$scope.GetLayoutRelatedDetails();

    //$scope.ViewMoreClick = function (ele, count) {
    //    if (parseInt(count) > 0) {
    //        $('html, body').animate({
    //            scrollTop: $(ele).offset().top - 50
    //        }, 500);
    //    }
    //};

};

controllers.LayoutDetailController.$inject = ['$scope', '$http'];

