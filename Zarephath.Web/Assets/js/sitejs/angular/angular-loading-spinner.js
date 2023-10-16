(function () {
    angular.module('ngLoadingSpinner', ['angularSpinner'])
    .directive('usSpinner', ['$http', '$rootScope', function ($http, $rootScope) {
        return {
            link: function (scope, elm, attrs) {
                $rootScope.spinnerActive = false;
                scope.isLoading = function () {
                    var showLoading = false;
                    if (!scope.key) {
                        angular.forEach($http.pendingRequests, function (value, index) {

                            if (value.showLoading) {
                                showLoading = true;
                            }
                        });
                    } else {
                        showLoading = !!scope.spinner?.el;
                    }
                    return showLoading;// $.each $http.pendingRequests.length > 0;
                };

                scope.$watch(scope.isLoading, function (loading) {
                   
                    $rootScope.spinnerActive = loading;
                    if (loading) {
                        elm.removeClass('ng-hide');
                    } else {
                        elm.addClass('ng-hide');
                    }
                });
            }
        };

    }]);
}).call(this);