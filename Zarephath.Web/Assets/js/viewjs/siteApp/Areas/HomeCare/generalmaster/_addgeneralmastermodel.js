var obj;
controllers.ParentController = function ($scope, $http, $window, $timeout, $rootScope) {
    obj = $scope;
    $scope.ItemTypeID = 0;

    $scope.PageDropDownList = null;
    $scope.DropDownList = {};

    $scope.CareTypeList = [];
    this.CloseModel = function () {
        alert($scope.ItemTypeID);
        //  $scope.PageDropDownList = $scope.DropDownList;
        //$scope.GetDesignation();
        //$scope.GetCaretype();

    }

    $scope.window = $window;
    $scope.IsVisitBilledBy = function (value) {
        return vm_ap.TempPayorModel.Payor.VisitBilledBy == value;
    }

};

controllers.ParentController.$inject = ['$scope', '$http', '$window', '$timeout', '$rootScope'];

$(document).ready(function () {

    $(document).on('focus', ':input', function () {
        $(this).attr('autocomplete', 'autocomplete_off_hack_xfr4!k');
    });

    $(".add-edit-item").on('click', function (event) {
        var selectValue = $(this).attr("data-value");
        if (selectValue.indexOf('9999999900001') > -1) {
            var itemTypeID = (selectValue.split('9999999900001')[1]).trim();
            obj.ItemTypeID = itemTypeID;
            $('#GeneralEmployee').modal('show');
            $('#GeneralDDLBindIFrame').attr('src', HomeCareSiteUrl.PartialAddGeneralMasterURL + itemTypeID);
        }
    })

    $('select').on('change', function (event, b) {
        var selectValue = event.target.value;// $(this).val();
        if (selectValue.indexOf('9999999900001') > -1) {
            var itemTypeID = (selectValue.split('9999999900001')[1]).trim();
            obj.ItemTypeID = itemTypeID;
            $('#GeneralEmployee').modal({
                backdrop: 'static',
                keyboard: false
            });
            $('#GeneralDDLBindIFrame').attr('src', HomeCareSiteUrl.PartialAddGeneralMasterURL + itemTypeID);
        }

    });
});



