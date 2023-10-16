var custModel;

controllers.AddAssessmentQuestionController = function ($scope, $http, $timeout, $window) {
    custModel = $scope;

    $scope.model = $.parseJSON($("#hdnAssessmentQuestionModel").val());
};

controllers.AddAssessmentQuestionController.$inject = ['$scope', '$http', '$timeout', '$window'];