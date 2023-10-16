var vm;
controllers.WizardController = function ($scope, $http) {
    vm = $scope;

    $scope.save = function (url) {
        $scope.$broadcast('save', '/hc/onboarding/movenext/' + url);
    };
};
controllers.WizardController.$inject = ['$scope', '$http'];
$(document).ready(function () {
    if ($('#organizationsetting').find('ul>li.completed').length === 2) {
        $('#organizationsettingul').addClass('completed');
        $('#organizationsettingli').addClass('completed');
    }
    else if ($('#organizationsetting').find('ul>li.ongoing').length !== 0) {
        $('#organizationsettingul').addClass('ongoing');
        $('#organizationsettingli').addClass('ongoing');
    }

    if ($('#addservicecode').find('ul>li.completed').length !== 0) {
        $('#addservicecodeul').addClass('completed');
        $('#addservicecodeli').addClass('completed');
    }
    else if ($('#addservicecode').find('ul>li.ongoing').length !== 0) {
        $('#addservicecodeul').addClass('ongoing');
        $('#addservicecodeli').addClass('ongoing');
    }

    if ($('#addpayor').find('ul>li.completed').length !== 0) {
        $('#addpayorul').addClass('completed');
        $('#addpayorli').addClass('completed');
    }
    else if ($('#addpayor').find('ul>li.ongoing').length !== 0) {
        $('#addpayorul').addClass('ongoing');
        $('#addpayorli').addClass('ongoing');
    }

    if ($('#addvisittask').find('ul>li.completed').length !== 0) {
        $('#addvisittaskul').addClass('completed');
        $('#addvisittaskli').addClass('completed');
    }
    else if ($('#addvisittask').find('ul>li.ongoing').length !== 0) {
        $('#addvisittaskul').addClass('ongoing');
        $('#addvisittaskli').addClass('ongoing');
    }

    if ($('#generalmasterdetail').find('ul>li.completed').length === 2) {
        $('#generalmasterdetailul').addClass('completed');
        $('#generalmasterdetailli').addClass('completed');
    }
    else if ($('#generalmasterdetail').find('ul>li.ongoing').length !== 0) {
        $('#generalmasterdetailul').addClass('ongoing');
        $('#generalmasterdetailli').addClass('ongoing');
    }  
});