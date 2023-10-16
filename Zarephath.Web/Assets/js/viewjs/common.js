/// <reference path="../sitejs/jquery.js" />

//moment.tz.setDefault("America/New_York");

var serverSideDateFormat = "YYYY/MM/DD";
function GetHtmlString(string) {
    return $("<div/>").html(string).text();
}

function GenerateGuid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
      s4() + '-' + s4() + s4() + s4();
}

//for getting days frm two days
function daydiff(first, second) {
    return (second - first) / (1000 * 60 * 60 * 24);
}

String.prototype.format = function () {
    var str = this;
    for (var i = 0; i < arguments.length; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        str = str.replace(reg, arguments[i]);
    }
    return str;
};



Array.prototype.remove = function (a) {
    for (var c = this.t(), d = [], f = "function" == typeof a ? a : function (e) { return e === a }, g = 0; g < c.length; g++) {
        var e = c[g];
        f(e) && (0 === d.length && this.K(), d.push(e), c.splice(g, 1), g--)
    }
    d.length && this.J();
    return d;
};

function getParam(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return results[1];
}

function CheckErrors(currentForm, isDynamic) {
    var form = jQuery(currentForm);
    if (isDynamic) {
        form = $(currentForm)
                .removeData("validator") /* added by the raw jquery.validate plugin */
                .removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse(form);
    }

    if (!form.valid()) {
        $('.field-validation-error.tooltip-danger').tooltip('hide');
        $('.select-validatethis.tooltip-danger').tooltip('hide');
        $('.jcf-class-validatethis.tooltip-danger').tooltip('hide');

        return false;
    }
    return true;
}

function HideErrors(currentForm) {
    if ($(currentForm) && $(currentForm).length >0) {
        $(currentForm).validate().resetForm();
        $(currentForm).find('.tooltip-danger').tooltip('destroy');
        $(currentForm).find('.tooltip-danger').removeClass('tooltip-danger');
    }
}

$(['zprequiredif']).each(function (index, validationName) {
    $.validator.addMethod(validationName,
            function (value, element, parameters) {
                // Get the name prefix for the target control, depending on the validated control name
                var prefix = "";
                var lastDot = element.name.lastIndexOf('.');
                if (lastDot != -1) {
                    prefix = element.name.substring(0, lastDot + 1).replace('.', '_');
                }
                var id = '#' + prefix + parameters['dependentproperty'];
                // get the target value
                var targetvalue = parameters['targetvalue'];
                targetvalue = (targetvalue == null ? '' : targetvalue).toString();
                // get the actual value of the target control
                var control = $(id);
                if (control.length == 0 && prefix.length > 0) {
                    // Target control not found, try without the prefix
                    control = $('#' + parameters['dependentproperty']);
                }
                if (control.length > 0) {
                    var controltype = control.attr('type');
                    var actualvalue = "";
                    switch (controltype) {
                        case 'checkbox':
                            actualvalue = control.attr('checked').toString(); break;
                        case 'select':
                            actualvalue = $('option:selected', control).text(); break;
                        default:
                            actualvalue = control.val(); break;
                    }
                    // if the condition is true, reuse the existing validator functionality
                    var rule;

                    var ruleparam;
                    if (targetvalue.toLowerCase() === actualvalue.toLowerCase()) {
                        rule = parameters['rule'];
                        ruleparam = parameters['ruleparam'];
                        return $.validator.methods[rule].call(this, value, element, ruleparam);
                    }
                    // this condition will used for check validation if main property is not null and dependent property is required in that condition. 
                    if (targetvalue.toLowerCase() === 'null' && actualvalue.length > 0) {
                        rule = parameters['rule'];
                        ruleparam = parameters['ruleparam'];
                        return $.validator.methods[rule].call(this, value, element, ruleparam);
                    }


                }
                return true;
            }
        );

    $.validator.unobtrusive.adapters.add(validationName, ['dependentproperty', 'targetvalue', 'rule', 'ruleparam'], function (options) {
        var rp = options.params['ruleparam'];
        options.rules[validationName] = {
            dependentproperty: options.params['dependentproperty'],
            targetvalue: options.params['targetvalue'],
            rule: options.params['rule']
        };
        if (rp) {
            options.rules[validationName].ruleparam = rp.charAt(0) == '[' ? eval(rp) : rp;
        }
        options.messages[validationName] = options.message;
    });
});

$.validator.setDefaults({
    ignore: ".ignore-validation-true, .ignore-element",
    //ignore: ":not(.validatealways):hidden , .ignorevalidation",

    showErrors: function (errorMap, errorList) {
        this.defaultShowErrors();

        var allValidation = this.elements();
        // destroy tooltips on valid elements --remove erro from parent row-block
        var validElement = $("." + this.settings.validClass);

        // this will return the total number of errors in form
        var errors = this.numberOfInvalids();
        $.each(validElement, function (i, val) {
            //if ($(val).parent().hasClass("validate-group") || $(val).parent().hasClass("select2require")) {




            if (val.id) {
                var element = null;
                if (val.form.id) {
                    element = "#" + val.form.id + " #" + val.id;
                } else {
                    element = "#" + val.id;
                }
                val = element;
            }


            if ($(val).parent().hasClass("validate-group")) {
                val = $(val).parent();
            }

            if ($(val).hasClass("error_wysihtml5")) {
                val = $(val).parent().find("iframe");
            }

            if ($(val).hasClass("error_summer_note")) {
                val = $(val).parent().find(".note-editor");
            }
            if ($(val).parent().hasClass("date")) {
                val = $(val).parent();
            }

            if ($(val).hasClass("groupError") && $(val).parents(".groupErrorParent")) {
                val = $(val).parents(".groupErrorParent");
            }


            //replce Error
            if ($(val).hasClass("replaceErrorSource")) {
                val = $(val).parent().find(".replaceErrorDest");
            }




            if (!$(val).hasClass('ko-validation')) {
                $(val).removeClass("tooltip-danger").tooltip("destroy").closest('.row-block').removeClass('error');
                $(val).siblings('.select-validatethis').removeClass("tooltip-danger").tooltip("destroy");
                $(val).siblings('.jcf-class-validatethis').removeClass("tooltip-danger").tooltip("destroy");
            }
        });

        // add/update tooltips 
        for (var i = 0; i < errorList.length; i++) {
            //
            var error = errorList[i];

            var element = null;
            if (error.element.form.id) {
                element = error.element.form.id + " #" + error.element.id;
            } else {
                element = error.element.id;
            }
            //error.element.id = element;

            //$("#" + error.element.id).tooltip();
            $("#" + element).closest('.row-block').addClass('error');
            $("#" + element).siblings('.select-validatethis').tooltip("hide");
            if ($("#" + element).hasClass('jcf-hidden')) {
                if ($("#" + element).prop('tagName') == 'SELECT') {
                    $("#" + element).siblings('.select-validatethis')
                        .attr("data-original-title", error.message)
                        .attr("data-html", "true")
                        .addClass("tooltip-danger")
                        .tooltip({ html: true });
                    //.tooltip('show', { html: true });
                } else if ($("#" + element).attr('type') == 'checkbox') {
                    $("#" + element).siblings('.jcf-class-validatethis')
                        .attr("data-original-title", error.message)
                        .attr("data-html", "true")
                        .addClass("tooltip-danger")
                        .tooltip({ html: true });//.tooltip('show', { html: true });
                }
            }
            else if ($("#" + element).parent().hasClass("validate-group")) {
                $("#" + element).parent(".validate-group")
                    .attr("data-original-title", error.message)
                    .attr("data-html", "true")
                    .addClass("tooltip-danger")
                    .tooltip({ html: true });
            }

            else if ($("#" + element).hasClass("error_wysihtml5")) {
                $("#" + element).parent().find("iframe")
                    .attr("data-original-title", error.message)
                    .attr("data-html", "true")
                    .addClass("tooltip-danger")
                    .tooltip({ html: true });
            }
            else if ($("#" + element).hasClass("error_summer_note")) {
                $("#" + element).parent().find(".note-editor")
                    .attr("data-original-title", error.message)
                    .attr("data-html", "true")
                    .addClass("tooltip-danger")
                    .tooltip({ html: true });
            }
            else if ($("#" + element).parent().hasClass("date")) {
                $("#" + element).parent()
                    .attr("data-original-title", error.message)
                    .attr("data-html", "true")
                    .addClass("tooltip-danger")
                    .tooltip({ html: true });
            }
            else if ($("#" + element).hasClass("groupError")) {
                $("#" + element).parents(".groupErrorParent")
                    .attr("data-original-title", error.message)
                    .attr("data-html", "true")
                    .addClass("tooltip-danger input-validation-error")
                    .tooltip({ html: true });
            }
            else if ($("#" + element).hasClass("replaceErrorSource")) {

                var ele = $("#" + element).parent().find(".replaceErrorDest");
                //if (ele.length===0)
                //    ele = $("#" + element).parent(".replaceErrorDest");

                $(ele)
                    .attr("data-original-title", error.message)
                    .attr("data-html", "true")
                    .addClass("tooltip-danger input-validation-error")
                    .tooltip({ html: true });
            } else {
                $("#" + element)
                    .attr("data-original-title", error.message)
                    .attr("data-html", "true")
                    .addClass("tooltip-danger")
                    .tooltip({ html: true }); //.tooltip('show', { html: true });//.tooltip({ "html": true });
                //.attr("data-placement", "right")
                //.tooltip({trigger:'click'});
            }
        }
    }
});


//$.validator.setDefaults({
//    ignore: ".ignore-validation-true, .ignore-element",
//    //ignore: ":not(.validatealways):hidden , .ignorevalidation",

//    showErrors: function (errorMap, errorList) {
//        this.defaultShowErrors();
//        //
//        var allValidation = this.elements();
//        // destroy tooltips on valid elements --remove erro from parent row-block
//        var validElement = $("." + this.settings.validClass);

//        // this will return the total number of errors in form
//        var errors = this.numberOfInvalids();
//        $.each(validElement, function (i, val) {
//            //if ($(val).parent().hasClass("validate-group") || $(val).parent().hasClass("select2require")) {
//            if ($(val).parent().hasClass("validate-group")) {
//                val = $(val).parent();
//            }

//            if ($(val).hasClass("error_wysihtml5")) {
//                val = $(val).parent().find("iframe");
//            }

//            if ($(val).hasClass("error_summer_note")) {
//                val = $(val).parent().find(".note-editor");
//            }
//            if ($(val).parent().hasClass("date")) {
//                val = $(val).parent();
//            }

//            if ($(val).hasClass("groupError") && $(val).parents(".groupErrorParent")) {
//                val = $(val).parents(".groupErrorParent");
//            }


//            //replce Error
//            if ($(val).hasClass("replaceErrorSource")) {
//                var pastVal = val;
//                val = $(val).parent().find(".replaceErrorDest");

//                if (val == undefined || val.length === 0) {
//                    val = $(pastVal).parents().find(".replaceErrorDest01");
//                }
//            }



//            if (!$(val).hasClass('ko-validation')) {
//                $(val).removeClass("tooltip-danger").tooltip("destroy").closest('.row-block').removeClass('error');
//                $(val).siblings('.select-validatethis').removeClass("tooltip-danger").tooltip("destroy");
//                $(val).siblings('.jcf-class-validatethis').removeClass("tooltip-danger").tooltip("destroy");
//            }
//        });

//        // add/update tooltips 
//        for (var i = 0; i < errorList.length; i++) {
//            //
//            var error = errorList[i];
//            //$("#" + error.element.id).tooltip();
//            $("#" + error.element.id).closest('.row-block').addClass('error');
//            $("#" + error.element.id).siblings('.select-validatethis').tooltip("hide");
//            if ($("#" + error.element.id).hasClass('jcf-hidden')) {
//                if ($("#" + error.element.id).prop('tagName') == 'SELECT') {
//                    $("#" + error.element.id).siblings('.select-validatethis')
//                        .attr("data-original-title", error.message)
//                        .attr("data-html", "true")
//                        .addClass("tooltip-danger")
//                        .tooltip({ html: true });
//                    //.tooltip('show', { html: true });
//                } else if ($("#" + error.element.id).attr('type') == 'checkbox') {
//                    $("#" + error.element.id).siblings('.jcf-class-validatethis')
//                        .attr("data-original-title", error.message)
//                    .attr("data-html", "true")
//                        .addClass("tooltip-danger")
//                        .tooltip({ html: true });//.tooltip('show', { html: true });
//                }
//            }
//            else if ($("#" + error.element.id).parent().hasClass("validate-group")) {
//                $("#" + error.element.id).parent(".validate-group")
//                   .attr("data-original-title", error.message)
//                   .attr("data-html", "true")
//                   .addClass("tooltip-danger")
//                   .tooltip({ html: true });
//            }

//            else if ($("#" + error.element.id).hasClass("error_wysihtml5")) {
//                $("#" + error.element.id).parent().find("iframe")
//                   .attr("data-original-title", error.message)
//                   .attr("data-html", "true")
//                   .addClass("tooltip-danger")
//                   .tooltip({ html: true });
//            }
//            else if ($("#" + error.element.id).hasClass("error_summer_note")) {
//                $("#" + error.element.id).parent().find(".note-editor")
//                   .attr("data-original-title", error.message)
//                   .attr("data-html", "true")
//                   .addClass("tooltip-danger")
//                   .tooltip({ html: true });
//            }
//            else if ($("#" + error.element.id).parent().hasClass("date")) {
//                $("#" + error.element.id).parent()
//                   .attr("data-original-title", error.message)
//                   .attr("data-html", "true")
//                   .addClass("tooltip-danger")
//                   .tooltip({ html: true });
//            }
//            else if ($("#" + error.element.id).hasClass("groupError")) {
//                $("#" + error.element.id).parents(".groupErrorParent")
//                   .attr("data-original-title", error.message)
//                   .attr("data-html", "true")
//                   .addClass("tooltip-danger input-validation-error")
//                   .tooltip({ html: true });
//            }
//            else if ($("#" + error.element.id).hasClass("replaceErrorSource")) {

//                var ele = $("#" + error.element.id).parent().find(".replaceErrorDest");
//                //
//                if (ele == undefined || ele.length === 0) {
//                    ele = $("#" + error.element.id).parents().find(".replaceErrorDest01");
//                }

//                //if (ele.length===0)
//                //    ele = $("#" + error.element.id).parent(".replaceErrorDest");

//                $(ele)
//               .attr("data-original-title", error.message)
//               .attr("data-html", "true")
//               .addClass("tooltip-danger input-validation-error")
//               .tooltip({ html: true });
//            } else {
//                $("#" + error.element.id)
//                    .attr("data-original-title", error.message)
//                    .attr("data-html", "true")
//                    .addClass("tooltip-danger")
//                    .tooltip({ html: true }); //.tooltip('show', { html: true });//.tooltip({ "html": true });
//                //.attr("data-placement", "right")
//                //.tooltip({trigger:'click'});
//            }
//        }
//    }
//});





// Ajax call code 
// TODO :: This is used in our site to show spinner https://github.com/adonespitogo/angular-loading-spinner
// Ref. : https://github.com/urish/angular-spinner   , http://fgnass.github.io/spin.js/
function AngularAjaxCall($angularHttpObejct, url, postData, httpMethod, callDataType, contentType, showLoading, allowCrossOrgin) {
    if (!ValideElement(allowCrossOrgin))
        allowCrossOrgin = false;

    var headerText = "";
    if (allowCrossOrgin) {
        headerText = {
            'Content-Type': contentType,
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Methods":"post",
            "Access-Control-Allow-Headers": "Content-Type, Authorization"
        };
    } else {
        headerText = { 'Content-Type': contentType };
    }


    //myApp.showPleaseWait();
    if (contentType == undefined)
        contentType = "application/json";

    if (callDataType == undefined)
        callDataType = "json";

    if (showLoading == undefined)
        showLoading = true;

    return $angularHttpObejct({
        method: httpMethod,
        responseType: callDataType,
        url: url,
        data: postData,
        dataType: 'jsonp',
        crossOrigin: true,
        headers: headerText,//{ 'Content-Type': contentType },
        showLoading: showLoading,
    }).error(function (data, error) {
        if (error != null) {
            if (error == 500) {
                window.location = "/security/internalerror";
            }
            else if (error == 403) {
                window.location = "/security/accessdenied";
            }
            else if (error == 308) {
                window.location = data.Link;
            }
        }

        // alert(1);
    });
    //    .finally(function () {
    //    myApp.hidePleaseWait();
    //});
}


function CrossDomainAngularAjaxCall(url, postData, httpMethod, callDataType, contentType, showLoading) {


    if (contentType == undefined)
        contentType = "application/json";

    if (callDataType == undefined)
        callDataType = "json";

    if (showLoading == undefined)
        showLoading = true;

    return $.ajax({
        method: httpMethod,
        responseType: callDataType,
        url: url,
        data: postData,
        
        async: false,
        crossDomain: true,
        xhrFields: {
            withCredentials: true
        },
        header: {
            'Content-Type': contentType
        },

        showLoading: showLoading
    }).error(function (data, error) {
        
        if (error != null) {
            if (error == 500) {
                window.location = "/security/internalerror";
            }
            else if (error == 403) {
                window.location = "/security/accessdenied";
            }
            else if (error == 308) {
                window.location = data.Link;
            }
        }

        // alert(1);
    });
    //    .finally(function () {
    //    myApp.hidePleaseWait();
    //});
}



// code for show toaster messages
function ShowMessages(data, timeInMs) {
    if (data.IsSuccess != undefined && data.Message != undefined) {

        if (data.IsSuccess) {
            if (data.ErrorCode && data.ErrorCode == window.ErrorCode_Warning) {
                ShowMessage(data.Message, "warning", timeInMs);
            } else {
                ShowMessage(data.Message, 'success', timeInMs);
            }

        } else {
            ShowMessage(data.Message, "error", timeInMs);
        }
    }
}

function ShowMessage(message, type, timeInMs, extendedTimeOut, position, stack) {
    if (stack != true)
        toastr.clear();

    toastr.options.positionClass = null;

    if (position != undefined)
        toastr.options.positionClass = position;
    else
        toastr.options.positionClass = 'toast-top-right';

    //alert(toastr.options.positionClass);

    toastr.options.closeButton = true;

    toastr.options.timeOut = timeInMs == undefined ? 4000 : timeInMs;
    toastr.options.extendedTimeOut = extendedTimeOut == undefined ? 4000 : extendedTimeOut;

    if (type != undefined && type == 'error')
        toastr.error(message);
    else if (type != undefined && type == 'warning')
        toastr.warning(message);
    else if (type != undefined && type == 'info')
        toastr.info(message);
    else
        toastr.success(message);
}


function ShowToaster(type, message, options, callback) {

    toastr.clear();
    toastr.options.closeButton = true;
    toastr.options.timeOut = 2500;
    toastr.options.positionClass = 'toast-top-right';
    if (callback != undefined)
        toastr.options.onHidden = callback;

    if (type == 'success' && message != undefined && message != '') {
        toastr.success(message, '', options);
    }
    else if (type == 'warning' && message != undefined && message != '') {
        toastr.warning(message);
    }
    else if (type == 'error' && message != undefined && message != '') {
        toastr.error(message);
        //if (options == undefined) {
        //    toastr.error(message, '', { timeOut: 0, positionClass: 'toast-top-center', extendedTimeOut: 0, closeButton: true });
        //  //  toastr.success('hi', '', { timeOut: 0000, positionClass: 'toast-top-left', extendedTimeOut: 0, closeButton: true })
        //} else {
        //    toastr.error(message, '', options);
        //}
    }
    else if (type == 'errorDialog' && message != undefined && message != '') {
        //toastr.error(message);
        //if (options == undefined) {
        //    toastr.error(message, '', { timeOut: 0, positionClass: 'toast-top-center', extendedTimeOut: 0, closeButton: true });
        //  //  toastr.success('hi', '', { timeOut: 0000, positionClass: 'toast-top-left', extendedTimeOut: 0, closeButton: true })
        //} else {
        //    toastr.error(message, '', options);
        //}
        ShowDialogMessage(window.Oops, BootstrapDialog.TYPE_DANGER, message);
    }
}


var myApp;
myApp = myApp || (function () {
    var pleaseWaitDiv = $('<div class="modal bootstrap-dialog type-default" id="myModal" style="height:100%;overflow:hidden;"><div class="modal-dialog" style="padding-top: 290px;width:100%; height:100%; position: absolute; margin-top:0; text-align:center;"><div class="modal-content" style="background:transparent; box-shadow:none; border:none;"><div class="modal-body" style="padding:0px"><img height="50" src="/assets/images/ajax-loader.gif"/></div></div></div></div>');
    return {
        showPleaseWait: function () {
            $('body').css('height', '100%');
            pleaseWaitDiv.modal();
        },
        hidePleaseWait: function () {
            pleaseWaitDiv.modal('hide');
        }
    };
})();

function ShowDialogMessage(header, type, message) {
    BootstrapDialog.show({
        type: type,
        title: header,
        message: message,
        closable: false,
        closeByBackdrop: false,
        closeByKeyboard: false,
        buttons: [{
            label: window.Ok,
            cssClass: 'btn-custom-error',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }]
    });
}

function SelectDeselectAll(array, value) {
    array.forEach(function (item) {
        item.IsSelected(value);
        return;
    });

}

function SetMessageForPageLoad(data, cookieName) {
    $.cookie(cookieName, JSON.stringify(data), { path: '/' });
}

function ShowPageLoadMessage(cookieName) {
    if ($.cookie(cookieName) != null && $.cookie(cookieName) != "null") {
        //ShowMessages($.parseJSON($.cookie(cookieName)));
        toastr.success(($.cookie(cookieName)));
        $.cookie(cookieName, null, { path: '/' });
    }
}

function SetCookie(data, cookieName, expires, domain) {
    $.cookie(cookieName, null, { path: '/', domain: domain });
    $.cookie(cookieName, data, { path: '/', expires: expires, domain: domain });
}

function GetDomainOnly() {
    var domain = document.domain;
    var p = domain.split('.');
    domain = p.slice(-2).join('.');
    return (domain !== 'localhost' ? '.' : '') + domain;
}

function GetCookie(cookieName) {
    if ($.cookie(cookieName) != null) {
        var data = $.cookie(cookieName);
        $.cookie(cookieName, null, { path: '/' });
        return data;
    }
}

function GetCookieWithoutRemoving(cookieName) {
    if ($.cookie(cookieName) != null) {
        var data = $.cookie(cookieName);
        return data;
    }
}


function isLocationCookieExist() {
    if (($.cookie(window.LocationIDCookieName) == null || $.cookie(window.LocationIDCookieName) == "" || $.cookie(window.LocationIDCookieName) == undefined)) {
        return false;
    }
    var data = $.cookie(LocationID);
    return true;
}

function ParseJsonDate(jsondate) {
    return (eval((jsondate).replace(/\/Date\((\d+)\)\//gi, "new Date($1)")));
}

function getDisplayDate(jsondate) {
    return moment(eval((jsondate).replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).format(DateTimeFormat);
}

// code for save scope and location
function Redirect(location, url) {
    location.path(url);
}

function SaveCommonDirectives($scope, $http, $location) {

    // code for redirect to page
    $scope.RedirectToPage = function (path) {
        Redirect($location, path);
    };

    $scope.dateOptions = {
        showOtherMonths: true,
        selectOtherMonths: true,
        autoclose: true,
        dateFormat: 'dd/mm/yy'
    };

    $scope.NoRecordsFound = function (recordCount) {
        if (recordCount == "0") {
            $(".norecordfound").css("display", "block");
        }
    };

    $scope.addTemplateForAngu = function ($templateCache) {
        $templateCache.removeAll();
        $templateCache.put('/angucomplete-alt/index.html',
            '<div class="angucomplete-holder" ng-class="{\'angucomplete-dropdown-visible\': showDropdown}">' +
                '  <input id="{{id}}_value" name="{{inputName}}" required=""   ng-class="{\'angucomplete-input-not-empty\': notEmpty}" ng-model="searchStr"  spacenotonfirst="" ng-disabled="disableInput" type="{{inputType}}" placeholder="{{placeholder}}" maxlength="{{maxlength}}" ng-focus="onFocusHandler()" class="{{inputClass}}" ng-focus="resetHideResults()" ng-blur="hideResults($event)" autocapitalize="off" autocorrect="off" autocomplete="off" ng-change="inputChangeHandler(searchStr)" />' +
                '  <div id="{{id}}_dropdown" class="angucomplete-dropdown" ng-show="showDropdown">' +
                '    <div class="angucomplete-searching" ng-show="searching" ng-bind="textSearching"></div>' +
                '    <div class="angucomplete-searching" ng-show="!searching && (!results || results.length == 0)" ng-bind="textNoResults"></div>' +
                '    <div class="angucomplete-row" ng-repeat="result in results" ng-click="selectResult(result)" ng-mouseenter="hoverRow($index)" ng-class="{\'angucomplete-selected-row\': $index == currentIndex}">' +
                '      <div ng-if="imageField" class="angucomplete-image-holder">' +
                '        <img ng-if="result.image && result.image != \'\'" ng-src="{{result.image}}" class="angucomplete-image"/>' +
                '        <div ng-if="!result.image && result.image != \'\'" class="angucomplete-image-default"></div>' +
                '      </div>' +
                '      <div class="angucomplete-title" ng-if="matchClass" ng-bind-html="result.title"></div>' +
                '      <div class="angucomplete-title" ng-if="!matchClass">{{ result.title }}</div>' +
                '      <div ng-if="matchClass && result.description && result.description != \'\'" class="angucomplete-description" ng-bind-html="result.description"></div>' +
                '      <div ng-if="!matchClass && result.description && result.description != \'\'" class="angucomplete-description">{{result.description}}</div>' +
                '    </div>' +
                '  </div>' +
                '</div>'
        );
    };
}

function GetGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function ShowBootstrapConfirmModel(title, message, callback, btnCancelLabel, btnOkLabel, type, closable, draggable, btnCancelClass, btnOkClass) {
    if (title == undefined)
        title = window.Confirmation;

    if (message == undefined)
        message = "";

    if (type == undefined)
        type = BootstrapDialog.TYPE_PRIMARY;

    if (closable == undefined)
        closable = true;

    if (draggable == undefined)
        draggable = true;

    if (btnCancelLabel == undefined)
        btnCancelLabel = "";

    if (btnCancelClass == undefined)
        btnCancelClass = "";


    if (btnOkLabel == undefined)
        btnOkLabel = window.Ok;

    if (btnOkClass == undefined)
        btnOkClass = "btn-primary";

    if (callback == undefined)
        callback = function () {
        };

    BootstrapDialog.confirm({
        title: title,
        message: message,
        type: type,
        closable: closable,
        draggable: draggable,
        btnCancelLabel: btnCancelLabel,
        btnCancelClass: btnCancelClass,
        btnOKLabel: btnOkLabel,
        btnOKClass: btnOkClass, // <-- If you didn't specify it, dialog type will be used,
        callback: callback
    });
}

function ShowSwalConfirmModel(title, message, callback, showCancelButton, btnCancelLabel, btnOkLabel, type, allowOutsideClick) {
    if (type == undefined)
        type = window.Warning;

    if (showCancelButton == undefined)
        showCancelButton = false;

    if (btnCancelLabel == undefined)
        btnCancelLabel = window.No;

    if (btnOkLabel == undefined)
        btnOkLabel = window.Yes;

    if (allowOutsideClick == undefined)
        allowOutsideClick = false;

    if (callback == undefined)
        callback = function () {
        };

    swal({
        title: title,
        text: message,
        type: type,
        allowOutsideClick: allowOutsideClick,
        showCancelButton: showCancelButton,
        cancelButtonText: btnCancelLabel,
        confirmButtonText: btnOkLabel,
        confirmButtonColor: "#DD6B55",
        closeOnConfirm: true,
        closeOnCancel: true
    }, function (isConfirm) {
        callback(isConfirm);
    });
}


app.directive('activeLink', ['$location', function (location) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs, controller) {
            var clazz = attrs.activeLink;
            var path = attrs.href;
            path = path.substring(1); //hack because path does not return including hashbang
            scope.location = location;
            var newPath = scope.location["$$absUrl"];

            if (newPath === window.basePath || newPath === window.basePath + '/' || newPath === window.basePath + 'security' || newPath === window.basePath + 'security/') {
                $(".loginLink").parent().addClass(clazz);
            } else if (newPath.search(path) > 0) {
                element.parent().addClass(clazz);
            } else {
                element.parent().removeClass(clazz);
            }
            //scope.$watch('location.path()', function (newPath) {
            //    if (path === newPath) {
            //        element.addClass(clazz);
            //    } else {
            //        element.removeClass(clazz);
            //    }
            //});
        }
    };
}]);

var flagForActive = true;
app.directive('activeSiteLink', ['$location', function (location) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs, controller) {
            var clazz = attrs.activeSiteLink;
            var path = element.parent().attr("href");
            path = path.substring(1); //hack because path does not return including hashbang
            scope.location = location;
            var newPath = scope.location["$$absUrl"];

            //if (newPath.search(path) > 0) {
            //    element.parents("ul")[0].closest("li").addClass("active");
            //    element.addClass(clazz);
            //}

            //if (flagForActive) {
            //    $("#side-menu").children().removeClass("active");
            //    flagForActive = false;
            //}
        }
    };
}]);




jQuery.validator.addMethod("telinputValidator", function (value, element) {
    if ($(element).val().length == 0) {
        return true;
    }
    return $(element).intlTelInput("isValidNumber");
}, window.MobileNumberInvalid);



function Digit(objTextbox, event) {
    var keyCode = (event.which) ? event.which : (window.event) ? window.event.keyCode : -1;
    if (keyCode >= 48 && keyCode <= 57) {
        return true;
    }
    if (keyCode == 8 || keyCode == -1) {
        return true;
    }
    else {
        return false;
    }
}

String.prototype.format = function () {
    var str = this;
    for (var i = 0; i < arguments.length; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        str = str.replace(reg, arguments[i]);
    }
    return str;
}

function SetPageTitle(pageTitle) {
    window.document.title = pageTitle;
};

function SwalDeletePopup(deleteText, successCallback) {
    swal({
        title: window.AreYouSure,
        text: window.WouldYouLikeToDeleteThis + deleteText + "!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: window.YesDeleteIt,
        cancelButtonText: window.NoCancelPlease,
        closeOnConfirm: false,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            successCallback().success(function (data) {
                if (data.IsSuccess) {
                    swal(window.DeletedInCommonText, window.ImaginaryRecordDeleted, window.SuccessInCommonText);
                }
            });
        }
    });
}

/* Common Pager */
function ScrollToElement(ele) {
    $('html, body').animate({
        scrollTop: $(ele).offset().top - 50
    }, 0);
}

var PagerModule = function (sortIndex, resultElement, sortType) {
    var $scope = this;
    $scope.getDataCallback = function () {
        alert("Not Implemented GetDataCallback Function");
    };
    $scope.maxSize = 3;
    $scope.currentPage = 1;
    $scope.lastPage = 1;
    $scope.pageSize = window.PageSize ? window.PageSize : 10;
    $scope.totalRecords = 0;
    $scope.currentPageSize = 0;
    $scope.sortIndex = sortIndex;
    if (sortType != undefined) {
        $scope.sortDirection = sortType;
        $scope.reverse = $scope.sortDirection == "DESC" ? true : false; // asc and desc
    }
    else {
        $scope.sortDirection = "ASC";
        $scope.reverse = false; // asc and desc
    }
    $scope.pageChanged = function (newPage, arg1) {
        $scope.currentPage = newPage;
        if (arg1)
            $scope.getDataCallback(arg1);
        else
            $scope.getDataCallback();
        if (resultElement != undefined) {
            ScrollToElement(resultElement);
        }
    };

    $scope.nextPage = function () {

        var numberOfPages = Math.floor($scope.totalRecords / $scope.pageSize);
        if ($scope.totalRecords % $scope.pageSize != 0) {
            numberOfPages = numberOfPages + 1;
        }

        if ($scope.currentPage < numberOfPages) {
            $scope.currentPage = $scope.currentPage + 1;
            $scope.getDataCallback();
        }


        //if (resultElement != undefined) {
        //    ScrollToElement(resultElement);
        //}
    };

    $scope.TotalPages = function () {
        return parseInt($scope.totalRecords / $scope.pageSize) + 1;
    };

    //-----------------------------------------CODE FOR SORT-------------------------------------------

    //$scope.predicate = predicate; // coulumn name
    $scope.sortIndexArray = [];
    $scope.resetCallback = undefined;
    $scope.sortColumn = function (newPredicate, reset, args1) {
        $scope.reverse = ($scope.sortIndex === newPredicate) ? !$scope.reverse : false;
        // $scope.predicate = newPredicate;
        $scope.sortIndex = newPredicate != undefined ? newPredicate : sortIndex;
        $scope.sortDirection = $scope.reverse === true ? "DESC" : "ASC";
        $scope.sortIndexArray = [];
        if (reset) {
            $scope.currentPage = 1;
            if ($scope.resetCallback)
                $scope.resetCallback();
        }
        //
        if (args1)
            $scope.getDataCallback(args1);
        else
            $scope.getDataCallback();
    };


    $scope.sortMultiColumn = function (newPredicate, isCallForRmove, reset) {
        if (newPredicate.split(" ").length == 2) {
            var column = newPredicate.split(" ")[0];
            var sortDirection = newPredicate.split(" ")[1];
            //var obj = { 'column': column, 'sortDirection': sortDirection };
            var newItem = true;
            if ($scope.sortIndexArray == undefined)
                $scope.sortIndexArray = [];
            var selectedIndex = -1;
            $scope.sortIndexArray.filter(function (data, index) {
                var tempColumn = data.split(" ")[0];
                //var tempSortDirection = data.split(" ")[1];
                if (tempColumn == column) {
                    $scope.sortIndexArray[index] = newPredicate; //data.sortDirection = sortDirection;
                    newItem = false;
                    if (isCallForRmove) selectedIndex = index;
                }
            });

            if (!isCallForRmove && (newItem || $scope.sortIndexArray.length == 0))
                $scope.sortIndexArray.push(newPredicate);

            if (selectedIndex != -1) {
                $scope.sortIndexArray.splice(selectedIndex, 1);
            }

            $scope.sortIndex = "";
            if (reset)
                $scope.currentPage = 1;
            $scope.getDataCallback();
        }

        //$scope.removeSortMultiColumn= function (newPredicate) {
        //    if (newPredicate.split(" ").length == 2) {
        //        var column = newPredicate.split(" ")[0];
        //        var sortDirection = newPredicate.split(" ")[1];

        //        var tempIndex = -1;
        //        $scope.sortIndexArray.filter(function (data, index) {
        //            var tempColumn = data.split(" ")[0];
        //            //var tempSortDirection = data.split(" ")[1];
        //            if (tempColumn == column) {
        //                tempIndex = -1;
        //            }
        //        });

        //        if (tempIndex!=-1)
        //            $scope.sortIndexArray.push(newPredicate);

        //        $scope.getDataCallback();
        //    }


    };

    //-----------------------------------------End CODE FOR SORT-------------------------------------------
};
/*End Common Pager*/


Array.prototype.remove = function (i) {
    var index = this.indexOf(i);
    if (index > -1) {
        this.splice(index, 1);
    }
};



function bootboxDialog(callback, type, titleText, message, btnOkLabel, btnOkClass, btnCancelLabel, btnCancelClass, btn2ActionLabel, btn2ActionClass) {
    
    if (titleText == undefined)
        titleText = window.Confirmation;

    if (message == undefined)
        message = "Are you sure?";

    if (btnCancelLabel == undefined)
        btnCancelLabel = bootboxDialogButtonText.Cancel;

    if (btn2ActionLabel == undefined)
        btn2ActionLabel = bootboxDialogButtonText.SoftDelete;

    if (btnCancelClass == undefined)
        btnCancelClass = "";

    if (btn2ActionClass == undefined)
        btn2ActionClass = "";

    if (btnOkLabel == undefined)
        btnOkLabel = bootboxDialogButtonText.Ok;

    if (btnOkClass == undefined)
        btnOkClass = btnClass.primary;

    if (callback == undefined || callback == null || callback == '')
        callback = function () {
        };


    if (type === bootboxDialogType.Confirm) {
        bootbox.confirm({
            title: '<b>' + titleText + '</b>',
            message: message,
            buttons: {
                'cancel': {
                    label: btnCancelLabel,
                    className: btnCancelClass
                },
                'confirm': {
                    label: btnOkLabel,
                    className: btnOkClass
                }
            },
            callback: callback
        });
    }
    if (type === bootboxDialogType.OnlyConfirm) {
        bootbox.alert({
            title: '<b>' + titleText + '</b>',
            message: message,
            buttons: {
                'ok': {
                    label: btnOkLabel,
                    className: btnOkClass
                }
            },
            callback: callback
        });
    }
    if (type === bootboxDialogType.Dialog) {
        bootbox.dialog({
            title: '<b>' + titleText + '</b>',
            message: message,
            buttons: {
                'cancel': {
                    label: btnCancelLabel,
                    className: btnCancelClass
                },
                'softdelete': {
                    label: btn2ActionLabel,
                    className: btn2ActionClass,
                    callback: callback
                },
                'confirm': {
                    label: btnOkLabel,
                    className: btnOkClass,
                    callback: callback
                }
            }
        });
    }

    if (type === bootboxDialogType.TwoActionOnly) {
        bootbox.dialog({
            title: '<b>' + titleText + '</b>',
            message: message,
            buttons: {
                'softdelete': {
                    label: btn2ActionLabel,
                    className: btn2ActionClass,
                    callback: callback
                },
                'confirm': {
                    label: btnOkLabel,
                    className: btnOkClass,
                    callback: callback
                },

            }
        });
    }



    if (type === bootboxDialogType.Alert) {
        bootbox.alert({
            title: '<b>' + titleText + '</b>',
            message: message,
            buttons: {
                'ok': {
                    label: btnOkLabel,
                    className: btnOkClass
                }
            },
            callback: callback
        });
    }


    if (type === bootboxDialogType.Prompt) {
        bootbox.prompt({
            title: '<b>' + titleText + '</b>',
            inputType: 'textarea',
            //message: message,
            buttons: {
                'cancel': {
                    label: btnCancelLabel,
                    className: btnCancelClass
                },
                'confirm': {
                    label: btnOkLabel,
                    className: btnOkClass
                }
            },
            callback: callback
        });
    }


    if (type === bootboxDialogType.PromptWithMsg) {
        bootbox.prompt({
            title: '<b>' + titleText + '</b>',
            inputType: 'textarea',
            message: message,
            buttons: {
                'cancel': {
                    label: btnCancelLabel,
                    className: btnCancelClass
                },
                'confirm': {
                    label: btnOkLabel,
                    className: btnOkClass
                }
            },
            callback: callback
        });
    }
};


function bootboxDialogWithCancel(callback, titleText, message, btnAction1Label, btnAction1Class, btn2ActionLabel, btn2ActionClass, btnCancelLabel, btnCancelClass) {
    if (titleText == undefined)
        titleText = window.Confirmation;

    if (message == undefined)
        message = "Are you sure?";

    if (btnCancelLabel == undefined)
        btnCancelLabel = bootboxDialogButtonText.Cancel;

    if (btn2ActionLabel == undefined)
        btn2ActionLabel = bootboxDialogButtonText.SoftDelete;

    if (btnCancelClass == undefined)
        btnCancelClass = "";

    if (btn2ActionClass == undefined)
        btn2ActionClass = "";

    if (btnAction1Label == undefined)
        btnAction1Label = bootboxDialogButtonText.Ok;

    if (btnAction1Class == undefined)
        btnAction1Class = btnClass.primary;

    if (callback == undefined || callback == null || callback == '')
        callback = function () {
        };



    bootbox.dialog({
        title: '<b>' + titleText + '</b>',
        message: message,
        closeButton: false,
        buttons: {

            'action1': {
                label: btnAction1Label,
                className: btnAction1Class,
                callback: function (e) { callback(e, 1); }
            },
            'action2': {
                label: btn2ActionLabel,
                className: btn2ActionClass,
                callback: function (e) { callback(e, 2); }
            }, 'cancel': {
                label: btnCancelLabel,
                className: btnCancelClass,
                callback: function (e) { callback(e, 0); }
            }
        }
    });



};



function OpenModalPopUp(modalId, txtboxId) {
    $(modalId).modal({
        "backdrop": "static",
        "keyboard": true,
        "show": true
    });
    setTimeout(function () { $(txtboxId).focus(); }, 500);

}


function UpdateDatesJs(obj, propertylist) {
    jQuery.each(propertylist, function (i, val) {
        if (val != undefined && obj[val] != undefined) {
            obj[val] = new Date(moment(obj[val]).toString());   //moment(obj[val]).format("L");
        }
    });
};

function SetExpiryDate() {
    return new Date();
};

function GetDisplayDate(date) {
    return moment(new Date(date)).format('MM/DD/YYYY');
}



function GetServerSideFormat(date) {
    return moment(new Date(date)).format(serverSideDateFormat);
}

Date.prototype.addHours = function (h) {
    this.setHours(this.getHours() + h);
    return this;
};

Date.prototype.addDays = function (days) {
    this.setDate(this.getDate() + parseInt(days));
    return this;
};

function printDiv(ele) {
    var contents = $(ele).get(0).outerHTML;
    var frame1 = $('<iframe />');
    //frame1.remove();
    frame1[0].name = "frame1";
    frame1.css({ "position": "absolute", "top": "-1000000px" });
    $("body").append(frame1);
    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
    frameDoc.document.open();
    frameDoc.document.write('<head>');
    frameDoc.document.write('<link href="/sitelayout/css?abc" rel="stylesheet" type="text/css"  />');
    frameDoc.document.write('<link href="/Assets/css/sitecss/bootstrap.css" rel="stylesheet" type="text/css" />');
    frameDoc.document.write('<link href="/Assets/library/fullcalendar/fullcalendar.css" rel="stylesheet" type="text/css" />');
    frameDoc.document.write('<link href="/Assets/library/fullcalendar/fullcalendar.print.css" rel="stylesheet" type="text/css" />');
    frameDoc.document.write('<link href="/Assets/css/sitecss/uniform.default.css" rel="stylesheet" type="text/css" />');
    frameDoc.document.write('<link href="/Assets/css/sitecss/font-awesome.css" rel="stylesheet" type="text/css" />');
    frameDoc.document.write('<link href="/Assets/css/sitecss/opensans.css" rel="stylesheet" type="text/css" />');
    frameDoc.document.write('<link href="/Assets/css/sitecss/components.css" rel="stylesheet" type="text/css" />');
    frameDoc.document.write('<link href="/Assets/css/sitecss/style.css" rel="stylesheet" type="text/css" />');
    frameDoc.document.write('<link href="/Assets/css/sitecss/site.css" rel="stylesheet" type="text/css" />');
    frameDoc.document.write('<script src="/Assets/js/sitejs/jquery.js"></script>');
    frameDoc.document.write('</head>');
    frameDoc.document.write('<body>');

    frameDoc.document.write(contents);
    frameDoc.document.write('<script>$(window).load(function(){ setTimeout(function () { document.title=".";  window.print();  }, 1000); });</script>');
    frameDoc.document.write('</body>');
    frameDoc.document.write('</html>');
    frameDoc.document.close();
    window.frames["frame1"].focus();




};


function CheckValidTime(str) {
    var validTime = str.match(/^(0?[0-9]|1[012])(:[0-5]\d) [APap][mM]$/);
    return validTime;
};

function generateUUID(id) {
    var enc_id = id;
    $.cookie("enc_userid", enc_id);
    var d = new Date();
    var milliseconds = d.getMilliseconds();

    var userid = $.cookie('enc_userid');
    var str = milliseconds + userid;
    var hex_chr = "0123456789abcdef";
    hash = '';
    for (j = 0; j < str.length; j++)
        hash += hex_chr.charAt((str >> (j * 8 + 4)) & 0x0F) +
        hex_chr.charAt((str >> (j * 8)) & 0x0F);
    var seconds = d.getSeconds();
    if (seconds < 10) { seconds = '0' + seconds; }
    var minutes = d.getMinutes();
    if (minutes < 10) { minutes = '0' + minutes; }
    var hours = d.getHours();
    if (hours < 10) { hours = '0' + hours; }
    var day = d.getDate();
    if (day < 10) { day = '0' + day; }
    var month = d.getMonth() + 1;
    if (month < 10) { month = '0' + month; }
    var year = d.getFullYear();
    var uuid = "" + hash + "" + "-" + "" + seconds + "" + minutes + "" + hours + "" + "-" + "" + month + "" + day + "" + year + "";
    return uuid;
}


function createNotification(message, link) {
    var options = {
        body: $("<div/>").html(message).text(),
        icon: window.LiveSiteURL + window.NotificationLogo,/*'/assets/images/logo-blue@2x.png',*/
        dir: 'ltr'
    };

    var notification = new Notification("Hi there", options);

    notification.onclick = function () {
        window.open(window.LiveSiteURL + link);
    };
}

function GenerateDesktopNotification(message, link) {
    if (!("Notification" in window)) {
        alert("This browser does not support desktop notification");
    } else if (Notification.permission === "granted") {

        createNotification(message, link);

    } else if (Notification.permission !== 'denied') {
        Notification.requestPermission(function (permission) {
            if (!('permission' in Notification)) {
                Notification.permission = permission;
            }
            if (permission === 'granted') {

                createNotification();
            }
        });
    }
}


ValideDate = function (date) {
    if (date != null && date !== '0001-01-01T00:00:00' && date !== '' )
        return true;
    return false;
};

ValideElement = function (item) {
    if (item === null || item === undefined || item === "" || item === '' || item === 'null')
        return false;
    return true;
};


$(document).ready(function () {

    window.CurrentURL = window.location.href;
    //$("body").removeClass("bodySection");
    //$('form:first *:not(.no-focus):input[type!=hidden]:first').focus();
    setTimeout(function () {
        $("form").find('input[type=text]:not(.no-focus),textarea,select').filter(':visible:first').focus();
    }, 500);
    $(".hideContent").removeClass("hideContent");
    //$(".menu-toggler.sidebar-toggler").click();

    $(".remove-record").click(function () {
        var currentRecord = $(this);
        var confirmText;
        if (currentRecord.hasClass('btn-enabled')) {
            confirmText = window.enable;
        } else {
            confirmText = window.disable;
        }
        swal({
            title: window.AreYouSure,
            text: window.WouldYouLikeTo + confirmText + window.thisText + window.DeleteText + "!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: window.YesInCommonText + confirmText + window.ItInCommonText,
            cancelButtonText: window.NoCancelPlease,
            closeOnConfirm: false,
            closeOnCancel: true
        }, function (isConfirm) {
            if (isConfirm) {
                swal(window.DeletedInCommonText, window.ImaginaryRecordDeleted, window.SuccessInCommonText);

                if (currentRecord.hasClass('btn-enabled')) {
                    $(currentRecord).removeClass("btn-primary").addClass("btn-danger");
                    $(currentRecord).removeClass("btn-enabled").addClass("btn-disabled");
                    $(currentRecord).text("D");
                } else {
                    $(currentRecord).removeClass("btn-danger").addClass("btn-primary");
                    $(currentRecord).removeClass("btn-disabled").addClass("btn-enabled");
                    $(currentRecord).attr('title', 'Enable');
                    $(currentRecord).text("E");
                }
            }
        });
    });

    function checkMiniNavBar() {

        if (!$('body').hasClass('mini-navbar')) {
            $("#side-menu .nav-second-level li > .has-fixed-sidebar").css("display", "block");
            $("#side-menu .nav-second-level li > a.has-mini-sidebar").css("display", "none");
            $("#side-menu .nav-second-level li > span.has-mini-sidebar").css("display", "none");
        } else {
            $("#side-menu .nav-second-level li > .has-fixed-sidebar").css("display", "none");
            $("#side-menu .nav-second-level li > a.has-mini-sidebar").css("display", "block");
            $("#side-menu .nav-second-level li > span.has-mini-sidebar").css("display", "block");
        }
    }

    $(window).trigger('resize');

    $(".passwordEye").click(function () {
        var ele = "#" + $(this).attr("data-for");
        if ($(ele).attr("type") === "password") {
            $(ele).attr("type", "text");
            $(this).find("i").addClass("fa-eye-slash").removeClass("fa-eye");
        } else {
            $(ele).attr("type", "password");
            $(this).find("i").addClass("fa-eye").removeClass("fa-eye-slash");
        }
    });


   
    $('.Aadhaarformat').keydown(function (e) {
        debugger
        var key = e.charCode || e.keyCode || 0;
        $text = $(this);
        if (key !== 8 && key !== 9) {
            if ($text.val().length === 4) {
                $text.val($text.val() + '-');
            }
            if ($text.val().length === 9) {
                $text.val($text.val() + '-');
            }

        }

        return (key == 8 || key == 9 || key == 46 || (key >= 48 && key <= 57) || (key >= 96 && key <= 105));
    });
    $('.ssnformat').keydown(function (e) {
        var key = e.charCode || e.keyCode || 0;
        $text = $(this);
        if (key !== 8 && key !== 9) {
            if ($text.val().length === 3) {
                $text.val($text.val() + '-');
            }
            if ($text.val().length === 6) {
                $text.val($text.val() + '-');
            }

        }

        return (key == 8 || key == 9 || key == 46 || (key >= 48 && key <= 57) || (key >= 96 && key <= 105));
    });

    $(".intl-tel-input").css("width", "100%");

    if (window.LocationIDCookieName) {
        var oldLocationID = GetCookieWithoutRemoving(window.LocationIDCookieName);

        $(window).on('focus', function () {
            if (oldLocationID != GetCookieWithoutRemoving(window.LocationIDCookieName)) {
                location.reload();
            }
        });
    }

    //google address auto complete 
    $(".address-autocomplete").focusin(function () {
        $(document).keypress(function (e) {
            if (e.which == 13) {
                e.preventDefault();
            }

        });
    });

});


function GetGeoCodeAndMapFromAddress(addressValue, mapElementId, callback) {
    // Load google map
    if (mapElementId) {

        //var map = new google.maps.Map(document.getElementById(mapElementId), {
        //    center: new google.maps.LatLng(0, 0),
        //    zoom: 3,
        //    mapTypeId: google.maps.MapTypeId.ROADMAP,
        //    panControl: false,
        //    streetViewControl: false,
        //    mapTypeControl: false
        //});

        function loadGMap(coords) {
            //

            var map = new google.maps.Map(document.getElementById(mapElementId), {
                center: new google.maps.LatLng(0, 0),
                zoom: 3,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                panControl: false,
                streetViewControl: false,
                mapTypeControl: false
            });


            //new google.maps.event.addDomListener(window, "resize", function () {
            //    var center = map.getCenter();
            //    google.maps.event.trigger(map, "resize");
            //    map.setCenter(center);
            //});


            map.setCenter(coords);
            map.setZoom(18);

            var marker = new google.maps.Marker({
                position: coords,
                map: map,
                title: addressValue,
            });
        }
    }



    var geocoder = new google.maps.Geocoder();
    return geocoder.geocode({
        address: addressValue,
        region: 'no'
    },
        function (results, status) {
            if (status.toLowerCase() == 'ok') {
                // Get center
                var coords = new google.maps.LatLng(
                    results[0]['geometry']['location'].lat(),
                    results[0]['geometry']['location'].lng()
                );
                callback(coords.lat(), coords.lng());
                if (mapElementId) {
                    loadGMap(coords);
                }
            }
            else {
                callback(null,null);
            }
        }
    );
    

}






function go_full_screen(element) {
    var elem = element[0];//document.getElementById(elementID);
    if (elem.requestFullscreen) {
        elem.requestFullscreen();
    } else if (elem.msRequestFullscreen) {
        elem.msRequestFullscreen();
    } else if (elem.mozRequestFullScreen) {
        elem.mozRequestFullScreen();
    } else if (elem.webkitRequestFullscreen) {
        elem.webkitRequestFullscreen();
    }
}

/* Close fullscreen */
function closeFullscreen() {
    if (document.exitFullscreen) {
        document.exitFullscreen();
    } else if (document.mozCancelFullScreen) { /* Firefox */
        document.mozCancelFullScreen();
    } else if (document.webkitExitFullscreen) { /* Chrome, Safari and Opera */
        document.webkitExitFullscreen();
    } else if (document.msExitFullscreen) { /* IE/Edge */
        document.msExitFullscreen();
    }
}
//#endregion

function isSameDate(date1, date2) {
    return moment(date1).isSame(date2, 'day')
}

function GetOSName() {
    var Name = navigator.platform;
    if (navigator.userAgent.indexOf("Win") != -1) Name =
        "Windows";
    if (navigator.userAgent.indexOf("Mac") != -1) Name =
        "Macintosh";
    if (navigator.userAgent.indexOf("Linux") != -1) Name =
        "Linux";
    if (navigator.userAgent.indexOf("Android") != -1) Name =
        "Android";
    if (navigator.userAgent.indexOf("like Mac") != -1) Name =
        "iOS";
    return Name;
}

function openMapApp(location, mode) {
    // If it's an iPhone..
    if (GetOSName().toLowerCase() == 'ios') {
        if (mode == "") {
            window.open("maps://www.google.com/maps/dir/?api=1&travelmode=driving&layer=traffic&destination=" + location);
        } else {
            window.open("comgooglemaps://?center=" + location + "&mapmode=streetview");
        }
    }
    else {
        if (mode == "") {
            window.open("https://www.google.com/maps/dir/?api=1&travelmode=driving&layer=traffic&destination=" + location);
        }
        else {
            window.open("https://www.google.com/maps?q=" + location + "&mapmode=streetview");
        }
    }
}
