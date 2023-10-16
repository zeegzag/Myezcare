
//#region UI Related Third Party Controls and Plugin Related Directive Stuff
var ClientDateFormat = "MM/DD/YYYY";
var ClientDateTimeFormat = "YYYY-MM-DDTHH:mm:ss";
//format('YYYY-MM-DDTHH:mm:ss')

app.directive('deleteIfEmpty', function () {
    return {
        restrict: 'A',
        scope: {
            ngModel: '='
        },
        link: function (scope, element, attrs) {
            scope.$watch("ngModel", function (newValue, oldValue) {
                if (ValideElement(scope.ngModel) === false || (typeof scope.ngModel !== 'undefined' && scope.ngModel.length === 0)) {
                    delete scope.ngModel;
                }
            });
        }
    };
});


app.directive('countrycode', function () {
    return {
        restrict: 'C',
        scope: {
        },
        link: function (scope, elem, attr, ctrl) {
            //alert($(elem).intlTelInput);
            $(elem).intlTelInput({});
            $(elem).on("countrychange", function (e, countryData) {

                scope.$parent.SettingModel.TwilioDefaultCountryCode = "+" + countryData.dialCode;
                setTimeout(function () {
                    if (!scope.$root.$$phase) {
                        scope.$apply();
                    }
                }, 100);
            });


        }
    };
});












//#region Directive Autocomplete
app.directive('autoComplete', function () {
    return {
        restrict: 'C',
        scope: {
            searchurl: '@',
            dtrtype: '@'
        },
        link: function (scope, elem, attr, ctrl) {
            var xhr;
            $(elem).autoComplete({
                minChars: 1,
                source: function (term, response) {
                    try { xhr.abort(); } catch (e) { }
                    xhr = $.post(scope.searchurl, { term: term, type: scope.dtrtype, pageSize: 1000 }, function (data) { response(JSON.parse(data)); }, 'json');
                },
                onSelect: function (e, term, item) {
                }
            });

        }
    };
});
//#endregion

//#region Line Progress Bar

app.directive('lineProgressbar', function () {
    return {
        restrict: 'C',
        scope: {
            usedhours: '@',
            reamininghours: '@',
            totalhours: '@'
        },
        link: function (scope, elem, attr, ctrl) {

            //var percent = $(elem).next().find(".fc-cell-text").text();
            //var percentMaster = $(elem).next().find(".hrsprg").text();
            //var percent = parseFloat(scope.totalhours) > 0 ? (parseFloat(scope.reamininghours) * 100) / parseFloat(scope.totalhours) : 0;

            if (scope.usedhours == null || scope.usedhours == undefined) {
                scope.usedhours = 0;
            }
            var percent = parseFloat(scope.totalhours) > 0 ? (parseFloat(scope.usedhours) * 100) / parseFloat(scope.totalhours) : 0;
            $(elem).LineProgressbar({
                percentage: percent,
                duration: 'fast',
                fillBackgroundColor: '#1abc9c'
            });

        }
    };
});

//#endregion


//#region Directive Pulsate

app.directive('pulsateControls', function () {
    return {
        restrict: 'C',
        link: function (scope, element, attrs) {
            $(element).pulsate({
                color: "#cc1212",
                reach: 15,
                repeat: true,
                speed: 600,
                pause: 400,
                glow: false
            });

        },
        replace: true
    };
});
//#endregion


//#region Directive change Expire Date color

app.directive('changecolorexpireduedate', function () {
    return {
        //scope: {
        //    changecolor: '=',
        //},
        link: function (scope, element) {
            scope.$watch(function () {
                return element.val();
            }, function () {
                if (element.val()) {
                    var now = moment(new Date()).format("MM/DD/YYYY");
                    var checkdate = new Date();
                    if (new Date(element.val()) >= (new Date(now))) {
                        var days = daydiff(checkdate, new Date(element.val()));
                        if (days <= 90) {
                            element.css('background-color', 'yellow');
                        }
                        else {
                            element.css('background-color', 'white');
                        }
                    } else {
                        element.css('background-color', '#f4cccc');
                    }
                }
            });
        }
    };
});



app.directive('lblexpireduedate', function () {
    return {
        scope: {
            lblexpireduedate: '=',
        },
        link: function (scope, element) {
            scope.$watch(function () {
                return scope.lblexpireduedate;
            }, function () {
                if (scope.lblexpireduedate) {
                    var now = moment(new Date()).format("MM/DD/YYYY");
                    var checkdate = new Date();
                    if (new Date(scope.lblexpireduedate) >= (new Date(now))) {
                        var days = daydiff(checkdate, new Date(scope.lblexpireduedate));
                        if (days <= 30) {
                            element.css('background-color', 'yellow');
                            element.css('padding', '0 5px');
                        }
                        //else {
                        //    element.css('background-color', 'white');
                        //}
                    } else {
                        //element.css('background-color', '#f4cccc'); #e00000
                        element.css('background-color', '#ec1616');
                        element.css('color', '#FFF');
                        element.css('padding', '0 5px');
                    }
                } else {
                    element.html('&nbsp;');
                    element.css('background-color', '#ec1616');
                    element.css('color', '#FFF');
                    element.css('padding', '0 35px');
                    element.css('margin', '10px 0');
                }
            });
        }
    };
});

//#endregion

//#region Directive change DDL color

app.directive('ddlchangecolor', function () {
    return {
        link: function (scope, element) {
            scope.$watch(function () { return element.val(); }, function () {
                if (element.val() == 1 || element.val() == 'true' || element.val() == 'Y') {
                    element.css('background-color', '#c9daf8');
                }
                else if (element.val() == 0 || element.val() == 'false' || element.val() == 'N') {
                    element.css('background-color', '#f4cccc');
                } else {
                    element.css('background-color', 'white');
                }
            });
        }
    };
});

//#endregion

//#region Directive Uni CheckBox
app.directive('unicheckbox', function () {
    //PLEASE USE "uniformControls" directory instead of this
    return {
        restrict: 'C',
        scope: {
            val: "=",
            optval: "=?N"

        },
        link: function (scope, element, attrs) {
            scope.$watch(function () { return scope.val; }, function (newValue, oldValue) {

                if (newValue === true || newValue === 'Y') {
                    $(element).parent().addClass('checked');
                } else {
                    $(element).parent().removeClass('checked');
                }


                if (newValue)
                    scope.optval = "Y";
                else
                    scope.optval = "N";
            });


            scope.$watch(function () { return scope.optval; }, function (newValue, oldValue) {

                if (newValue === 'Y') {
                    $(element).parent().addClass('checked');
                } else {
                    $(element).parent().removeClass('checked');
                }
            });



            if ($(element).parents(".checker").size() === 0) {
                $(element).show();
                $(element).uniform();
            }
        },
        replace: true,
    };
});



app.directive('uniformControls', function () {
    return {
        restrict: 'AC',
        scope: {
            uniformValue: "="
        },
        link: function (scope, element, attrs) {
            scope.$watch('uniformValue', function (newValue, oldValue) {

                //
                if (newValue === true || newValue === 'true' || newValue === 'True') {
                    $(element).parent().addClass('checked');
                } else if (newValue === false || newValue === 'false' || newValue === 'False') {
                    $(element).parent().removeClass('checked');
                } else {
                    $(element).parent().removeClass('checked');
                }
            });

            $(element).uniform();
        },
        replace: true,
    };
});
//#endregion

//#region Directive For PopOver(webuiPopover) Control
app.directive('popoverHtml', ['$compile', function ($compile) {
    return {
        restrict: 'A',
        scope: {
            savecallback: "&",
            popoverHtml: "@",
            ngHeader: "@",
            ngValue: "=",
            ngData: "=",
            ngWidth: "@"
        },
        link: function (scope, elem, attrs) {
            var content = $(scope.popoverHtml).html();
            //not(this).popover('hide');                        
            var popOverContent = function () {
                return $compile(content)(scope);
            };
            scope.oldValue = scope.ngValue;
            elem.webuiPopover({
                html: true,
                width: scope.ngWidth ? scope.ngWidth : 300,
                container: 'body',
                content: popOverContent,
                closable: true,
                placement: 'auto',
                animation: 'pop',
                multi: false,
                dismissible: false,
                title: scope.ngHeader,
                trigger: attrs.popoverTrigger ? attrs.popoverTrigger : "click"
            });

            scope.Save = function () {
                scope.savecallback()(scope.ngValue, scope.ngData).then(function (response) {
                    if (response.data != null && response.data.IsSuccess) {
                        elem.popover('hide');
                        elem.triggerHandler('click');
                        elem.attr('disabled', false);
                    }
                });
            };

            scope.Cancel = function () {
                scope.ngValue = scope.oldValue;
                elem.popover('hide');
                elem.triggerHandler('click');
                elem.attr('disabled', false);
            };



        }
    };
}]);
app.directive('emailConfirmation', ['$compile', '$timeout', function ($compile, $timeout) {
    return {
        restrict: 'A',
        scope: {
            onCancel: '&',
            header: '@',
            yesCallback: '&',
            width: '@'
        },
        link: function (scope, elem, attrs) {
            var content = '<div class="text-center"><div class="btn-group"><a class="btn btn-sm btn-success" ng-click="yes()"><i class="glyphicon glyphicon-ok"></i> Yes</a><a class="btn btn-sm btn-danger" ng-click="no()"><i class="glyphicon glyphicon-remove"></i> No</a></div></div>';
            popOverContent = function () {
                return $compile(content)(scope);
            };
            elem.webuiPopover({
                width: scope.width ? scope.width : 300,
                html: true,
                content: popOverContent,
                placement: 'left',
                title: scope.header,
                trigger: "click",
                animation: 'pop',
                multi: false,
                dismissible: false,
            });

            elem.on('shown.bs.popover', function () {
                elem.attr('disabled', true);
                $('.link-popover').not(this).popover('hide');
            });

            scope.yes = function () {
                scope.yesCallback();
                elem.popover('hide');
                elem.triggerHandler('click');
                elem.attr('disabled', false);
            };

            scope.no = function () {
                elem.webuiPopover('hide');
                //elem.triggerHandler('click');
                //elem.attr('disabled', false);
            };
        }
    };
}]);

app.directive('dirPopover', function () {
    return {
        restrict: 'A',
        link: function (scope, el, attrs) {
            var setStyle = attrs.popoverWidth == undefined ? "popoverwidth" : attrs.popoverWidth;
            //scope.label = attrs.popoverLabel;

            if (attrs.popoverTitle == undefined)
                attrs.popoverTitle = "";

            if (attrs.popoverPlacement == undefined)    // left, top bottom,right
                attrs.popoverPlacement = "horizontal";

            if (attrs.popoverTrigger == undefined)      //click, hover, focus, manual
                attrs.popoverTrigger = "click";

            if (attrs.popoverClosable == undefined)
                attrs.popoverClosable = true;

            if (attrs.popoverAnimation == undefined)
                attrs.popoverAnimation = "pop";

            if (attrs.popoverPadding == undefined)
                attrs.popoverPadding = true;

            if (attrs.popoverContent == undefined)
                attrs.popoverContent = "";

            if (attrs.popoverContent.length > 0) {
                $(el).webuiPopover({
                    html: true,
                    width: attrs.popoverWidth,
                    height: attrs.popoverHeight,
                    container: document.body,
                    title: attrs.popoverTitle,
                    placement: attrs.popoverPlacement,
                    trigger: attrs.popoverTrigger,
                    closeable: attrs.popoverClosable,
                    animation: attrs.popoverAnimation,
                    padding: attrs.popoverPadding,
                    content: attrs.popoverContent,
                    style: setStyle
                });
            }
        }
    };
});
app.directive('referralDetailPopover', ['$compile', function ($compile) {
    return {
        restrict: 'A',
        scope: {
            ngReferalId: '=',
            ngTemplateSelector: "@",
            ngHeader: "@"
        },
        controller: ['$scope', '$http', function ($scope, $http) {
            $scope.IsLoadingDetails = false;
            $scope.DetailModel = null;

            $scope.LoadDetail = function () {
                $scope.IsLoadingDetails = true;
                var jsonData = angular.toJson({ referralID: $scope.ngReferalId });
                AngularAjaxCall($http, SiteUrl.GetReferralDetailForPopupURL, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    $scope.IsLoadingDetails = false;
                    if (response.IsSuccess) {
                        $scope.DetailModel = response.Data;
                    } else {
                        ShowMessages(response);
                    }
                });
                $scope.$apply();
            };


        }],
        link: function (scope, element, attrs) {
            var content = $(scope.ngTemplateSelector).html();
            var popOverContent = function () {
                return $compile(content)(scope);
            };
            element.webuiPopover({
                html: true,
                container: document.body,
                content: popOverContent,
                closeable: true,
                placement: 'auto',
                animation: 'pop',
                width: 500,
                multi: false,
                trigger: "click",
                onShow: function (e) {
                    if (!scope.DetailModel) {
                        scope.LoadDetail();
                    }
                }
            });
        }
    };
}]);

app.directive('itemDetailPopup', ['$compile', function ($compile) {
    return {
        restrict: 'A',
        scope: {
            ngSearchModel: '=',
            ngUrl: '=',
            ngTemplateSelector: "@",
            ngCallback: "&",
            ngHeader: "@",
            ngWidth: '='
        },
        controller: ['$scope', '$http', function ($scope, $http) {
            $scope.IsLoadingDetails = false;
            $scope.DetailModel = null;
            $scope.LoadDetail = function () {
                $scope.IsLoadingDetails = true;
                var jsonData = angular.toJson({ model: $scope.ngSearchModel });
                AngularAjaxCall($http, $scope.ngUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    $scope.IsLoadingDetails = false;

                    if (response.IsSuccess) {
                        $scope.DetailModel = response.Data;
                        $scope.DetailModel.CallBack = $scope.ngCallback();
                    } else {
                        ShowMessages(response);
                    }
                });
                $scope.$apply();
            };

        }],
        link: function (scope, element, attrs) {
            var content = $(scope.ngTemplateSelector).html();
            var popOverContent = function () {
                return $compile(content)(scope);
            };
            element.webuiPopover({
                html: true,
                container: document.body,
                content: popOverContent,
                closeable: true,
                placement: 'auto',
                animation: 'pop',
                width: scope.ngWidth ? scope.ngWidth : 690,
                multi: false,
                trigger: "click",
                onShow: function (e) {
                    if (!scope.DetailModel) {
                        scope.LoadDetail();
                    }
                }
            });


        }
    };
}]);

app.directive('itemDetailPopupNew', ['$compile', function ($compile) {
    return {
        restrict: 'A',
        scope: {
            ngModel: '=',
            ngTemplateSelector: "@",
            ngHeader: "@"
        },
        controller: ['$scope', '$http', function ($scope, $http) {
            $scope.IsLoadingDetails = false;
            $scope.DetailModel = null;
            $scope.LoadDetail = function () {
                $scope.IsLoadingDetails = true;
                $scope.DetailModel = $scope.ngModel;
                $scope.$apply();
            };

        }],
        link: function (scope, element, attrs) {
            var content = $(scope.ngTemplateSelector).html();
            var popOverContent = function () {
                return $compile(content)(scope);
            };
            element.webuiPopover({
                html: true,
                container: document.body,
                content: popOverContent,
                closeable: true,
                placement: 'auto',
                animation: 'pop',
                width: scope.ngWidth ? scope.ngWidth : 690,
                multi: false,
                trigger: "click",
                onShow: function (e) {
                    if (!scope.DetailModel) {
                        scope.LoadDetail();
                    }
                }
            });


        }
    };
}]);

app.directive('referralDetailPopup', ['$compile', function ($compile) {
    return {
        restrict: 'A',
        scope: {
            ngReferalId: '=',
            ngUrl: '=',
            ngTemplateSelector: "@",
            ngHeader: "@"
        },
        controller: ['$scope', '$http', function ($scope, $http) {
            $scope.IsLoadingDetails = false;
            $scope.DetailModel = null;

            $scope.LoadDetail = function () {
                $scope.IsLoadingDetails = true;
                var jsonData = angular.toJson({ referralID: $scope.ngReferalId });
                AngularAjaxCall($http, $scope.ngUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    $scope.IsLoadingDetails = false;
                    if (response.IsSuccess) {
                        $scope.DetailModel = response.Data;
                    } else {
                        ShowMessages(response);
                    }
                });
                $scope.$apply();
            };


        }],
        link: function (scope, element, attrs) {
            var content = $(scope.ngTemplateSelector).html();
            var popOverContent = function () {
                return $compile(content)(scope);
            };
            element.webuiPopover({
                html: true,
                container: document.body,
                content: popOverContent,
                closeable: true,
                placement: 'auto',
                animation: 'pop',
                width: 690,
                multi: false,
                trigger: "click",
                onShow: function (e) {
                    if (!scope.DetailModel) {
                        scope.LoadDetail();
                    }
                }
            });


        }
    };
}]);



app.directive('ajaxDetailPopup', ['$compile', function ($compile) {
    return {
        restrict: 'A',
        scope: {
            ngId: '=',
            ngUrl: '=',
            ngTemplateSelector: "@",
            ngHeader: "@"
        },
        controller: ['$scope', '$http', function ($scope, $http) {
            $scope.IsLoadingDetails = false;
            $scope.DetailModel = null;

            $scope.LoadDetail = function () {
                $scope.IsLoadingDetails = true;
                var jsonData = angular.toJson({ id: $scope.ngId });
                AngularAjaxCall($http, $scope.ngUrl, jsonData, "Post", "json", "application/json", false).success(function (response) {
                    $scope.IsLoadingDetails = false;
                    if (response.IsSuccess) {
                        $scope.DetailModel = response.Data;
                    } else {
                        ShowMessages(response);
                    }
                });
                $scope.$apply();
            };


        }],
        link: function (scope, element, attrs) {
            var content = $(scope.ngTemplateSelector).html();
            var popOverContent = function () {
                return $compile(content)(scope);
            };
            element.webuiPopover({
                html: true,
                container: document.body,
                content: popOverContent,
                closeable: true,
                placement: 'auto',
                animation: 'pop',
                width: 500,
                multi: false,
                trigger: attrs.popoverTrigger ? attrs.popoverTrigger : "click",
                onShow: function (e) {
                    if (!scope.DetailModel) {
                        scope.LoadDetail();
                    }
                }
            });


        }
    };
}]);





app.directive('commonDetailPopover', ['$compile', function ($compile) {
    return {
        restrict: 'A',
        scope: {
            ngTemplateSelector: "@",
            ngHeader: "@",
            commonDetailPopover: "=",
            popoverTrigger: "@",
            popoverWidth: "@",
            popoverCloseable: "@",
            popOverFunctions: "="
        },
        link: function (scope, element, attrs) {
            var content = $(scope.ngTemplateSelector).html();
            scope.popOverFunctions = {};

            if (!scope.popoverTrigger) {
                scope.popoverTrigger = "hover";
            }
            if (!scope.popoverWidth) {
                scope.popoverWidth = 500;
            }
            if (!attrs.popoverHeight) {
                attrs.popoverHeight = 'auto';
            }
            if (!scope.popoverCloseable) {
                scope.popoverCloseable = "true";
            }
            if (!attrs.popoverPlacement) {
                attrs.popoverPlacement = 'auto';
            }

            var popOverContent = function () {
                return scope.$apply(function () {
                    return $compile(content)(scope);
                });
            };
            element.webuiPopover({
                title: scope.ngHeader,
                html: true,
                cache: false,
                container: document.body,
                content: popOverContent,
                closeable: scope.popoverCloseable === "true",
                placement: attrs.popoverPlacement,
                animation: 'pop',
                width: scope.popoverWidth,
                height: attrs.popoverHeight,
                multi: false,
                trigger: scope.popoverTrigger
            });
            scope.popOverFunctions.Hide = function () {
                element.webuiPopover('hide');
            };

            $(element).on("remove", function () {
                scope.popOverFunctions.Hide();
            });
        }
    };
}]);


//#endregion

app.directive('betaTag', function () {
    return {
        template: '<div class="custom-title-text-beta" common-detail-popover data-content="{{window.BetaTagComment}}" popover-closeable="false">beta</div>'
    };
});

//#region Directive For Wysihtml Editor Control
app.directive('dirWysihtmlEditor', function () {
    return {

        restrict: "AC",
        require: 'ngModel',
        link: function (scope, element, attrs, ctrl) {
            $(element).addClass('wysihtml5');

            var textarea = element.wysihtml5({
                "html": true, "stylesheets": false
            });
            var editor = textarea.data('wysihtml5').editor;

            // on change text area value.
            editor.on('change', function () {
                scope.$apply(function () {
                    ctrl.$setViewValue(editor.getValue());
                });
            });

            //editor.on("load", function () {
            //    editor.focus();
            //    editor.composer.commands.exec("insertHTML", editor.getValue());
            //});

            // for update ui-view(set textarea value)   

            ctrl.$render = function () {
                var a = ctrl.$viewValue;
                if ($(a).find('table').html() != undefined) {
                    a = $(a).find('table').html();
                }
                textarea.html(a);
                editor.setValue(a);
            };
            ctrl.$render();
        }
    };
});

app.directive('dirSummerNoteEditor', function () {
    return {

        restrict: "AC",
        require: 'ngModel',
        scope: {
            ngModel: "="
        },
        link: function (scope, element, attrs, ctrl, ngModel) {
            $.summernote.pluginEvents['tab'] = function (event, editor, layoutInfo) { };
            $.summernote.pluginEvents['untab'] = function (event, editor, layoutInfo) { };

            $(element).summernote({
                height: attrs.height ? attrs.height : attrs.minHeight ? attrs.minHeight : null,
                minHeight: attrs.minHeight ? attrs.minHeight : null,
                maxHeight: attrs.maxHeight ? attrs.maxHeight : null,
                //toolbar: [
                //    // [groupName, [list of button]]
                //    ['style', ['bold', 'italic', 'underline', 'clear']],
                //    ['font', ['strikethrough', 'superscript', 'subscript']],
                //    ['fontsize', ['fontsize']],
                //    ['color', ['color']],
                //    ['para', ['ul', 'ol', 'paragraph']],
                //    ['height', ['height']]
                //]
            });

            $(element).on('summernote.change', function () {
                console.log('summernote\'s content is changed.');
                scope.$apply(function () {
                    var markupStr = $(element).val();//summernote('code');
                    if (markupStr) {
                        ctrl.$setViewValue(markupStr);
                    }
                });

            });

            scope.$watch(function () { return scope.ngModel; }, function (newVal, oldVal) {
                //$(element).summernote("code", newVal);
                //console.log('summernote\'s watcher.');
                //$(element).summernote('code',newVal);
                //$(element).summernote('destroy');
                //$(element).summernote('code', markupStr);
                //$(element).code(newVal);
                //$(element).change();
            });
        }
    };
});


app.directive('colorPicker', function () {
    return {

        restrict: "AC",
        require: 'ngModel',
        scope: {
            ngModel: "="
        },
        link: function (scope, element, attrs, ctrl, ngModel) {
            $(element).minicolors({
                change: function (value, opacity) {
                    console.log(value + ' - ' + opacity);
                },
                theme: 'bootstrap',
                position: 'top',
                defaultValue: ValideElement(scope.ngModel) ? scope.ngModel : ""
            });


            scope.$watch(function () { return scope.ngModel; }, function () {
                if (ValideElement(scope.$parent.$parent.ResetColorValue) && scope.$parent.$parent.ResetColorValue) {
                    scope.$parent.$parent.ResetColorValue = false;
                    $(element).minicolors("settings",
                        {
                            change: function (value, opacity) {
                                console.log(value + ' - ' + opacity);
                            },
                            theme: 'bootstrap',
                            position: 'top',
                            defaultValue: ValideElement(scope.ngModel) ? scope.ngModel : "",

                            animationEasing: "swing",
                            animationSpeed: 50,
                            changeDelay: 0,
                            control: "hue",
                            dataUris: true,
                            format: "hex",
                            hide: null,
                            hideSpeed: 100,
                            inline: false
                        });

                }
            });


        }
    };
});

//#endregion

//#region Directive For File Upload Control
app.directive('fileupload', function () {
    return {
        restrict: 'A',
        scope: {
            fileUploadurl: "@",
            beforesend: "&",
            aftersend: "&",
            progress: "&",
            limitmultifileuploads: "=",
            validfiletype: "@",
            primaryId: "=",
            extraValue: "=",
            filelist: '='

        },
        link: function (scope, element, attrs) {
            if (scope.filelist == undefined || !Array.isArray(scope.filelist)) {
                scope.filelist = new Array();
            }

            if (attrs.autoupload == undefined) {
                attrs.autoupload = true;
            } else {
                attrs.autoupload = attrs.autoupload.toLowerCase() === "true" || attrs.autoupload.toLowerCase() === "1";
            }

            if (scope.limitmultifileuploads == undefined)
                scope.limitmultifileuploads = 100;
            $(element).fileupload({
                dataType: 'json',
                url: scope.fileUploadurl,
                autoUpload: attrs.autoupload,
                singleFileUploads: false,
                formData: { id: scope.primaryId, data: scope.extraValue },
                send: function (e, data) {
                    var response = scope.beforesend()(e, data);

                    if (response.IsSuccess) {
                        return true;
                    } else {
                        data.files[0].IsError = true;
                        data.files[0].ErrorMessage = response.Message;
                        scope.$apply();
                        return false;
                    }
                },
                done: function (e, data) {
                    scope.filelist.remove(data.files[0]);
                    return scope.aftersend()(e, data);
                },
                progress: function (e, data) {
                    data.files[0].FileProgress = Math.round((data.loaded / data.total) * 100);
                    scope.$apply();
                    if (scope.progress) {
                        return scope.progress()(e, data);
                    }
                    return true;
                },
                fail: function (e, data) {
                    data.files[0].IsError = true;
                    data.files[0].ErrorMessage = data.files[0].ErrorMessage != undefined ? data.files[0].ErrorMessage : "Error";
                }
            }).on("fileuploadadd", function (e, data) {
                data.files[0].FileProgress = 0;
                data.files[0].IsError = false;
                data.files[0].ErrorMessage = null;
                scope.filelist.push(data.files[0]);
                scope.$apply();
            });
        }
    };
});


app.directive('fileuploadonsubmit', function () {
    return {
        restrict: 'A',
        scope: {
            fileUploadurl: "@",
            submitButtonId: "@",
            beforesend: "&",
            onsubmitclick: "&",
            onadd: "&",
            aftersend: "&",
            progress: "&",
            limitmultifileuploads: "=",
            validfiletype: "@",
            primaryId: "=",
            filelist: '=',
            filedata: '='
        },
        link: function (scope, element, attrs) {
            if (scope.filelist == undefined || !Array.isArray(scope.filelist)) {
                scope.filelist = new Array();
            }
            attrs.autoupload = attrs.autoupload == undefined || attrs.autoupload.toLowerCase() === "true" || attrs.autoupload.toLowerCase() === "1";

            if (scope.limitmultifileuploads == undefined)
                scope.limitmultifileuploads = 1;

            var sendData = true;
            $(element).fileupload({
                dataType: 'json',
                url: scope.fileUploadurl,
                autoUpload: attrs.autoupload,
                singleFileUploads: true,
                //formData: { id: "2342" },
                send: function (e, data) {

                    var response = scope.beforesend()(e, data);
                    if (response.IsSuccess) {
                        return true;
                    } else {
                        data.files[0].IsError = true;
                        data.files[0].ErrorMessage = response.Message;
                        scope.$apply();
                        return false;
                    }
                },
                done: function (e, data) {
                    scope.filelist.remove(data.files[0]);
                    return scope.aftersend()(e, data);
                },
                progress: function (e, data) {
                    data.files[0].FileProgress = Math.round((data.loaded / data.total) * 100);
                    scope.$apply();
                    if (scope.progress) {
                        return scope.progress()(e, data);
                    }
                    return true;
                },
                fail: function (e, data) {
                    data.files[0].IsError = true;
                    data.files[0].ErrorMessage = data.files[0].ErrorMessage != undefined ? data.files[0].ErrorMessage : "Error";
                },
                add: function (e, data) {
                    data.files[0].FileProgress = 0;
                    data.files[0].IsError = false;
                    data.files[0].ErrorMessage = null;
                    scope.filedata = data;
                    scope.onadd()(e, data);
                }
            });
        }
    };
});


app.directive('amazonfileupload', function () {

    return {
        restrict: 'A',
        scope: {
            fileUploadurl: "@",
            beforesend: "&",
            aftersend: "&",
            progress: "&",
            limitmultifileuploads: "=",
            validfiletype: "@",
            primaryId: "=",
            filelist: '=',
            ngAwsSettingsModel: '='
        },
        link: function (scope, element, attrs) {
            if (scope.filelist == undefined || !Array.isArray(scope.filelist)) {
                scope.filelist = new Array();
            }

            if (attrs.autoupload == undefined) {
                attrs.autoupload = true;
            } else {
                attrs.autoupload = attrs.autoupload.toLowerCase() === "true" || attrs.autoupload.toLowerCase() === "1";
            }

            if (scope.limitmultifileuploads == undefined)
                scope.limitmultifileuploads = 1;
            $(element).fileupload({
                dataType: 'json',
                url: scope.fileUploadurl,
                autoUpload: attrs.autoupload,
                singleFileUploads: true,
                formData: { id: scope.primaryId }
            }).on("fileuploadadd", function (e, data) {
                data.files[0].FileProgress = 0;
                data.files[0].IsError = false;
                data.files[0].ErrorMessage = null;
                scope.filelist.push(data.files[0]);
                scope.$apply();
                var response = scope.beforesend()(e, data);
                if (response.IsSuccess) {
                    var filename = data.files[0].name.split(".");
                    var ext = filename[filename.length - 1];
                    //var name=generateUUID(scope.ngAwsSettingsModel.enc_userid);
                    ext = ext.toLowerCase();

                    var awsSettingModel = scope.ngAwsSettingsModel;
                    var name = generateUUID(awsSettingModel.UserID);
                    var new_filename = name + "." + ext;
                    //data.files[0].new_filename_search = awsSettingModel.Folder + "/" + new_filename; //For used in remove file.

                    if (awsSettingModel.Folder) {
                        new_filename = awsSettingModel.Folder + new_filename;
                        /**---------------Upload to amazone Start---------------**/
                        var fd = new FormData();
                        fd.append('key', new_filename);
                        fd.append('acl', awsSettingModel.ACL);
                        //fd.append('Content-Type', file.type);
                        fd.append('AWSAccessKeyId', awsSettingModel.AccessKey);
                        fd.append('policy', awsSettingModel.Policy);
                        fd.append('signature', awsSettingModel.Signature);
                        fd.append('success_action_status', awsSettingModel.SuccessAction);
                        fd.append("file", data.files[0]);


                        var xhr = new XMLHttpRequest();
                        //xhr.setRequestHeader('Origin', document.domain);
                        xhr.upload.addEventListener("progress", function (oEvent) {
                            /**-------Uploading In progress-------**/
                            data.files[0].FileProgress = Math.round((oEvent.loaded / oEvent.total) * 100);
                            scope.$apply();
                            if (scope.progress) {
                                return scope.progress()(e, data);
                            }
                            return true;

                            //progressbardiv.style.width = percent + '%';
                            //progressbardiv.width(percent + '%');
                        }, false);


                        //xhr.addEventListener("load", function () {                            
                        //}, false);


                        //xhr.addEventListener("loadstart", function () {                            
                        //});


                        xhr.addEventListener("loadend", function (oEvent) {
                            if (awsSettingModel.SuccessAction == oEvent.currentTarget.status) {
                                scope.aftersend()({
                                    FilePath: new_filename,
                                    FileName: data.files[0].name,
                                    FileSize: data.files[0].size,
                                    File: data.files[0]
                                    //Search_FileName:New_FileName
                                }); // Store file in successCallback after successfully uploaded on amazone.

                            } else {
                                data.files[0].IsError = true;
                                data.files[0].ErrorMessage = data.files[0].ErrorMessage != undefined ? data.files[0].ErrorMessage : "Error";
                            }
                        });


                        xhr.addEventListener("error", function () {
                            data.files[0].IsError = true;
                            data.files[0].ErrorMessage = data.files[0].ErrorMessage != undefined ? data.files[0].ErrorMessage : "Error";
                        }, false);

                        xhr.addEventListener("abort", function () {
                        }, false);

                        xhr.open('POST', awsSettingModel.URL, true); //MUST BE LAST LINE BEFORE YOU SEND

                        xhr.send(fd);


                    }
                } else {
                    data.files[0].IsError = true;
                    data.files[0].ErrorMessage = response.Message;
                    scope.$apply();
                    return false;
                }

            });
        }
    };
});
//#endregion

//#region Directive For Range Slider Control
app.directive('rangeSlider', function () {
    return {
        restrict: 'A',
        scope: {
            ngFrom: "=",
            ngTo: "=",
            ngMax: "@",
            ngMin: "@",
            ngStep: "@",
            ngPrefix: "@",
            ngPostfix: "@"
        },
        link: function (scope, element, attrs) {
            if (!scope.ngStep) {
                scope.ngStep = 1;
            }
            $(element).ionRangeSlider({
                type: "double",
                //grid: true,
                min: scope.ngMin,
                max: scope.ngMax,
                from: scope.ngFrom.toString(),
                to: scope.ngTo.toString(),
                step: scope.ngStep,
                prefix: scope.ngPrefix,
                postfix: scope.ngPostfix,
                decorate_both: true

            });
            scope.IsChangesByElement = false;

            scope.$watch(function () { return scope.ngFrom + ";" + scope.ngTo; }, function () {
                if (!scope.IsChangesByElement) {
                    var slider = $(element).data("ionRangeSlider");
                    slider.update({
                        from: scope.ngFrom.toString(),
                        to: scope.ngTo.toString()
                    });

                    scope.IsChangesByElement = false;
                }
            });

            $(element).val(scope.ngMin + ";" + scope.ngMax);

            $(element).change(function () {
                var value = $(element).val();
                var valArray = value.split(";");
                scope.ngFrom = valArray[0];
                scope.ngTo = valArray[1];
                scope.IsChangesByElement = true;
                if (!scope.$root.$$phase) {
                    scope.$apply();
                }
            });

        }
    };
});
//#endregion

//#region Directive For PulSate Control
app.directive('pulsate', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var options = {
                color: '#ed0e0e',
                reach: 20,                              // how far the pulse goes in px
                speed: 1000,                            // how long one pulse takes in ms
                pause: 0,                               // how long the pause between pulses is in ms
                glow: true,                             // if the glow should be shown too
                repeat: true,                           // will repeat forever if true, if given a number will repeat for that many times
                onHover: false
            };
            $(element).pulsate(options);
        }
    };
});
//#endregion

//#region Directive For Shorten JS Control
app.directive('shorten', function () {
    return {
        restrict: 'A',
        scope: {
            ngCharcount: "@"
        },
        link: function (scope, element, attrs) {
            scope.$watch('myDirectiveVar', function (newValue, oldValue) {
                if (scope.ngCharcount == undefined) {
                    scope.ngCharcount = 100;
                } else {
                    scope.ngCharcount = parseInt(scope.ngCharcount);
                }
                $(element).hide();
                // setTimeout(function () {
                $(element).addClass('readmore');
                $(element).shorten({
                    "showChars": scope.ngCharcount,
                    "moreText": window.ShowMore,
                    "lessText": window.ShowLess,
                });
                $(element).show();
            }, true);
        },
        replace: true,
    };
});
//#endregion

//#region Directive For Token Input Control
app.directive('tokenInput', function () {
    return {
        restrict: 'A',
        scope: {
            value: '=ngVal',
            prePopulate: '=',
            additionalFilterText: '@',
            searchurl: '@',
            additionalFilterValue: '=',
            onaddedcallback: '&',
            ondeletecallback: '&',
            onresultcallback: '&',
            onresultsformatter: '&',
            ontokenformatter: '&',
            tokeninputobj: '=',
            //clearfunction: '=',
            tabindex: '@'

        },
        link: function (scope, element, attrs) {
            var onAddedCallback = scope.onaddedcallback();
            var onResultCallback = scope.onresultcallback();
            var onResultsFormatter = scope.onresultsformatter();
            var onDeleteCallback = scope.ondeletecallback();
            var onTokenFormatter = scope.ontokenformatter();

            //var minChars = attrs.minchars != undefined ? attrs.minchars : 0;

            //var propertyToSearch = attrs.propertyToSearch;
            var placeholder = attrs.placeholder != undefined ? attrs.placeholder : window.EnterSearchText;
            var enableCahing = attrs.enablecahing != undefined ? attrs.enablecahing : false;
            //var tokenValue = attrs.tokenValue;

            var prePopulate = scope.prePopulate != undefined ? scope.prePopulate : [];
            var valueField = attrs.valuefield;
            var textField = attrs.textfield;
            var value = scope.value;


            if (valueField == undefined || textField == undefined) {
                alert(window.TokenInputError);
                throw (window.TokenInputErrorInBindingHandler);
            }

            var customClass = attrs.customclass;
            if (customClass == undefined)
                customClass = '';

            var searchLimit = attrs.searchlimit;
            if (searchLimit == undefined)
                searchLimit = '10';

            var tokenLimit = attrs.tokenlimit;
            if (tokenLimit == undefined)
                tokenLimit = null;

            var additionalFilterText = attrs.additionalFilterText;
            if (additionalFilterText == undefined)
                additionalFilterText = null;

            var additionalFilterValue = scope.additionalFilterValue;
            if (additionalFilterValue == undefined)
                additionalFilterValue = null;

            var autoSelectFirstResult = attrs.autoselectfirstresult;
            if (autoSelectFirstResult == undefined)
                autoSelectFirstResult = false;
            else
                autoSelectFirstResult = attrs.autoselectfirstresult.toLowerCase() === "true" || attrs.autoselectfirstresult.toLowerCase() === "1";

            var minChars = attrs.minchars != undefined ? attrs.minchars : autoSelectFirstResult ? 0 : 1;

            if (tokenLimit == null || tokenLimit > 1) {
                if (!$.isArray(scope.value)) {
                    scope.value = new Array();
                }
            }

            $.each(prePopulate, function (index, item) {
                if ($.isArray(scope.value)) {
                    scope.value.push(item[valueField]);
                } else {
                    scope.value = item[valueField];
                }
            });
            //scope.clearfunction = function () {
            //    $(element).tokenInput("clear");
            //};
            if (!scope.$root.$$phase) {
                scope.$apply();
            }

            if (onAddedCallback == undefined) {
                onAddedCallback = function (item, e) {
                    if ($.isArray(scope.value)) {
                        scope.value.push(item[valueField]);
                    } else {
                        scope.value = item[valueField];
                    }
                    //set focus on next input after select item from token input dropdown
                    var inputs = $(this).closest('form').find(':input:enabled');
                    //([type=image],[type=button],[type=submit])
                    inputs.eq(inputs.index(this) + 1).focus();
                    //
                    if (!scope.$root.$$phase) {
                        scope.$apply();
                    }
                };
            }

            if (onDeleteCallback == undefined) {
                onDeleteCallback = function (item, e) {
                    if ($.isArray(scope.value)) {
                        var index = scope.value.remove(item[valueField]);
                    }
                    else
                        scope.value = null;
                    if (!scope.$root.$$phase) {
                        scope.$apply();
                    }
                };
            }

            if (onResultCallback == undefined) {
                onResultCallback = function (results) {
                    results = $.grep(results, function (n, i) {
                        if ($.isArray(scope.value)) {
                            {
                                if (scope.value.indexOf(n[valueField]) == -1)
                                    return n;
                            }
                        } else {
                            if (scope.value != n[valueField]) {
                                return n;
                            }
                        }
                        return false;
                    });
                    return results;
                };
            }

            if (onResultsFormatter == undefined) {
                onResultsFormatter = function (item) {
                    return "<li id='" + item[valueField] + "'>" + item[textField] + "</li>";
                };
            }

            $(element).tokenInput(scope.searchurl, {
                method: "POST",
                queryParam: "searchText",
                searchLimitText: 'pageSize',
                searchLimit: searchLimit,
                additionalFilterText: additionalFilterText,
                additionalFilterValue: additionalFilterValue,
                tokenLimit: tokenLimit,
                customClass: customClass,
                placeholder: placeholder,
                prePopulate: prePopulate,
                tokenFormatter: onTokenFormatter,
                //hintText: "Start typing to find existing Related Items…",
                minChars: minChars,
                showing_all_results: true,
                preventDuplicates: true,
                tokenValue: valueField,
                propertyToSearch: textField,
                onAdd: onAddedCallback,
                onDelete: onDeleteCallback,
                onResult: onResultCallback,
                resultsFormatter: onResultsFormatter,
                autoSelectFirstResult: autoSelectFirstResult,
                enableCahing: enableCahing// function(item) { return "<li id='" + item.id + "'>" + item.name + "</li>"; }
            });
            element.isModified = 0;


            if (scope.tokeninputobj) {
                scope.tokeninputobj.add = function (obj) {
                    element.tokenInput("add", obj);
                };
                scope.tokeninputobj.remove = function (obj) {
                    element.tokenInput("remove", obj);
                };
                scope.tokeninputobj.clear = function () {
                    element.tokenInput("clear");
                };
            }

            $($(element).siblings('.token-input-list').find('input')[0]).attr("placeholder", placeholder).addClass('form-control');
            if (!($(element).attr('tabindex') == null || $(element).attr('tabindex') == undefined)) {
                $($(element).siblings('.token-input-list').find('input')[0]).attr("tabindex", $(element).attr('tabindex'));
            }

            $(window).resize(function () {
                $('#content').height($(window).height() - 46);
                var tokenHintTextWidth = $($(element).siblings('.token-input-list')).width() + 1;
                $(".token-input-dropdown").css("width", tokenHintTextWidth);
            });
            $(window).trigger('resize');

            scope.$watch('additionalFilterValue', function (newValue, oldValue) {
                additionalFilterText = scope.additionalFilterText;
                if (additionalFilterText == undefined)
                    additionalFilterText = null;

                additionalFilterValue = scope.additionalFilterValue;
                if (additionalFilterValue == undefined)
                    additionalFilterValue = null;

                $(element).tokenInput('setOptions', {
                    additionalFilterText: additionalFilterText,
                    additionalFilterValue: additionalFilterValue
                });
            }, true);
        }
    };
});



app.directive('tokenInputLocal', function () {
    return {
        restrict: 'A',
        scope: {
            value: '=ngVal',
            prePopulate: '=',
            additionalFilterText: '@',
            localData: '=',
            additionalFilterValue: '=',
            onaddedcallback: '&',
            ondeletecallback: '&',
            onresultcallback: '&',
            onresultsformatter: '&',
            ontokenformatter: '&',
            tokeninputobj: '=',

            //clearfunction: '=',
            tabindex: '@'

        },
        link: function (scope, element, attrs) {


            var onAddedCallback = scope.onaddedcallback();
            var onResultCallback = scope.onresultcallback();
            var onResultsFormatter = scope.onresultsformatter();
            var onDeleteCallback = scope.ondeletecallback();
            var onTokenFormatter = scope.ontokenformatter();



            //var propertyToSearch = attrs.propertyToSearch;
            var placeholder = attrs.placeholder != undefined ? attrs.placeholder : window.EnterSearchText;
            var enableCahing = attrs.enablecahing != undefined ? attrs.enablecahing : false;
            //var tokenValue = attrs.tokenValue;

            var prePopulate = scope.prePopulate != undefined ? scope.prePopulate : [];
            var valueField = attrs.valuefield;
            var textField = attrs.textfield;
            var value = scope.value;




            if (valueField == undefined || textField == undefined) {
                alert(window.TokenInputError);
                throw (window.TokenInputErrorInBindingHandler);
            }

            var customClass = attrs.customclass;
            if (customClass == undefined)
                customClass = '';

            var searchLimit = attrs.searchlimit;
            if (searchLimit == undefined)
                searchLimit = '10';

            var tokenLimit = attrs.tokenlimit;
            if (tokenLimit == undefined)
                tokenLimit = null;

            var additionalFilterText = attrs.additionalFilterText;
            if (additionalFilterText == undefined)
                additionalFilterText = null;

            var autoSelectFirstResult = attrs.autoselectfirstresult;
            if (autoSelectFirstResult == undefined)
                autoSelectFirstResult = true;
            else
                autoSelectFirstResult = attrs.autoselectfirstresult.toLowerCase() === "true" || attrs.autoselectfirstresult.toLowerCase() === "1";

            var minChars = attrs.minchars != undefined ? attrs.minchars : autoSelectFirstResult ? 0 : 1;

            var additionalFilterValue = scope.additionalFilterValue;
            if (additionalFilterValue == undefined)
                additionalFilterValue = null;

            if (tokenLimit == null || tokenLimit > 1) {
                if (!$.isArray(scope.value)) {
                    scope.value = new Array();
                }
            }

            $.each(prePopulate, function (index, item) {
                if ($.isArray(scope.value)) {
                    scope.value.push(item[valueField]);
                } else {
                    scope.value = item[valueField];
                }
            });
            //scope.clearfunction = function () {
            //    $(element).tokenInput("clear");
            //};
            if (!scope.$root.$$phase) {
                scope.$apply();
            }

            if (onAddedCallback == undefined) {
                onAddedCallback = function (item, e) {
                    if ($.isArray(scope.value)) {
                        scope.value.push(item[valueField]);
                    } else {
                        scope.value = item[valueField];
                    }
                    //set focus on next input after select item from token input dropdown
                    var inputs = $(this).closest('form').find(':input:enabled');
                    //([type=image],[type=button],[type=submit])
                    inputs.eq(inputs.index(this) + 1).focus();
                    //
                    if (!scope.$root.$$phase) {
                        scope.$apply();
                    }
                };
            }

            if (onDeleteCallback == undefined) {
                onDeleteCallback = function (item, e) {
                    if ($.isArray(scope.value)) {
                        var index = scope.value.remove(item[valueField]);
                    }
                    else
                        scope.value = null;
                    if (!scope.$root.$$phase) {
                        scope.$apply();
                    }
                };
            }

            if (onResultCallback == undefined) {
                onResultCallback = function (results) {
                    results = $.grep(results, function (n, i) {
                        if ($.isArray(scope.value)) {
                            {
                                if (scope.value.indexOf(n[valueField]) == -1)
                                    return n;
                            }
                        } else {
                            if (scope.value != n[valueField]) {
                                return n;
                            }
                        }
                        return false;
                    });
                    return results;
                };
            }
            if (onResultsFormatter == undefined) {
                onResultsFormatter = function (item) {
                    return "<li id='" + item[valueField] + "'>" + item[textField] + "</li>";
                };
            }

            $(element).tokenInput(scope.localData, {
                method: "POST",
                queryParam: "searchText",
                searchLimitText: 'pageSize',
                searchLimit: searchLimit,
                additionalFilterText: additionalFilterText,
                additionalFilterValue: additionalFilterValue,
                tokenLimit: tokenLimit,
                customClass: customClass,
                placeholder: placeholder,
                prePopulate: prePopulate,
                tokenFormatter: onTokenFormatter,
                //hintText: "Start typing to find existing Related Items…",
                minChars: minChars,
                showing_all_results: true,
                preventDuplicates: true,
                tokenValue: valueField,
                propertyToSearch: textField,
                onAdd: onAddedCallback,
                onDelete: onDeleteCallback,
                onResult: onResultCallback,
                resultsFormatter: onResultsFormatter,
                autoSelectFirstResult: autoSelectFirstResult,
                enableCahing: enableCahing// function(item) { return "<li id='" + item.id + "'>" + item.name + "</li>"; }
            });
            element.isModified = 0;


            if (scope.tokeninputobj) {
                scope.tokeninputobj.add = function (obj) {
                    element.tokenInput("add", obj);
                };
                scope.tokeninputobj.remove = function (obj) {
                    element.tokenInput("remove", obj);
                };
                scope.tokeninputobj.clear = function () {
                    element.tokenInput("clear");
                };
            }


            $($(element).siblings('.token-input-list').find('input')[0]).attr("placeholder", placeholder).addClass('form-control');
            if (!($(element).attr('tabindex') == null || $(element).attr('tabindex') == undefined)) {
                $($(element).siblings('.token-input-list').find('input')[0]).attr("tabindex", $(element).attr('tabindex'));
            }

            $(window).resize(function () {
                $('#content').height($(window).height() - 46);
                var tokenHintTextWidth = $($(element).siblings('.token-input-list')).width() + 1;
                $(".token-input-dropdown").css("width", tokenHintTextWidth);
            });
            $(window).trigger('resize');


            scope.$watch('additionalFilterValue', function (newValue, oldValue) {
                additionalFilterText = scope.additionalFilterText;
                if (additionalFilterText == undefined)
                    additionalFilterText = null;

                additionalFilterValue = scope.additionalFilterValue;
                if (additionalFilterValue == undefined)
                    additionalFilterValue = null;


                $(element).tokenInput('setOptions', {
                    additionalFilterText: additionalFilterText,
                    additionalFilterValue: additionalFilterValue
                });
            }, true);

            scope.$watchCollection('localData', function (newValue, oldValue) {
                if (newValue != undefined) {
                    $(element).data("settings").local_data = scope.localData;
                }
            });
        }
    };
});
//#endregion

//#region Directive For ZipCode Control
app.directive('zipcode', function () {
    return {
        restrict: 'E',
        scope: {
            value: '=ngModel',
            required: '@',
            modelid: '@'

        },
        //compile: function (scope, element, attrs) {
        //    //
        //},
        controller: ['$scope', function ($scope) {
            $scope.ZipCode = "";
            $scope.PostZipCode = "";
            $scope.TokenInputObj = {};
            $scope.GetZipCodeListURL = SiteUrl.GetZipCodeListURL;

            $scope.OnResultsFormatter = function (item) {
                return "<li id='" + item['ZipCode'] + "'>" + item['ZipCode'] + " " + item['City'] + " " + item['StateCode'] + "</li>";
            };

            $scope.$watch(function () { return $scope.value; }, function (newval, oldval) {
                if ($scope.value && $scope.value.length >= 5) {
                    $scope.ZipCode = $scope.value.substring(0, 5);
                    $scope.PostZipCode = $scope.value.substring(5, $scope.value.length);
                    $scope.TokenInputObj.clear();
                    $scope.TokenInputObj.add({ ZipCode: $scope.ZipCode });
                    $('.postzipcode').val($scope.PostZipCode);
                } else {
                    $scope.ZipCode = "";
                    $scope.TokenInputObj.clear();
                }
            });

            $scope.$watch(function () { return $scope.ZipCode + $scope.PostZipCode; }, function (newval, oldval) {
                $scope.value = $scope.ZipCode + $scope.PostZipCode;
            });

        }],

        link: function (scope, element, attrs) {

            $(element).find('.postzipcode').on('keydown', function (e) {
                if (e.keyCode == 9) {
                    return true;
                }

                if (e.keyCode == 8) {
                    if ($(this).val().length == 0) {
                        var z = scope.ZipCode;
                        scope.ZipCode = "";
                        scope.$apply();
                        $(element).find('.token-input-list input').val(z.substr(0, 4));
                        $(element).find('.token-input-list input').keydown();
                        $(element).find('.token-input-list input').focus();
                        $(element).find('.token-input-list input')[0].setSelectionRange($(element).find('.token-input-list input').val().length, $(element).find('.token-input-list input').val().length);
                        e.preventDefault();
                    }
                    return true;
                }
                if ((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105)) {
                    if (e.keyCode != 9 && $(this).val().length >= 4) {
                        $(this).val($(this).val().substr(0, 4));
                        return false;
                    }
                    return true;
                }
                return false;
            });
            $(element).find('.token-input-list input').on('keydown', function (e) {
                if (e.keyCode == 9) {
                    if (scope.ZipCode == undefined || scope.ZipCode == "") {
                        if (e.shiftKey) {

                        } else {
                            $.tabNext(2);
                            e.preventDefault();
                        }
                    }
                }
            });
            $(element).find('.postzipcode').on('focusin', function (e) {
                if (scope.ZipCode == undefined || scope.ZipCode == "") {
                    $(element).find('.token-input-list input').focus();

                }
            });
            $(element).find('.token-input-list input').addClass('ignorevalidation').attr("maxlength", 5);
            $($(element).find('.zipcode')[0]).attr('id', scope.modelid);
            $($(element).find('.zipcode')[0]).attr('name', scope.modelid);
            //$($(element).find('.field-validation-valid')[0]).attr('data-valmsg-for', scope.modelid);

            if (scope.required) {
                $(element).find('.token-input-list').attr("data-original-title", scope.required);

                $($(element).find('.zipcode')[0]).rules("add", {
                    zipcoderequired: true
                });
                //$($(element).find('.zipcode')[0]).
                $($(element).find('.zipcode')[0]).on('change', function () {

                    $(this).valid();
                });
            }

        },
        template: " <div class='zipcodediv no-padding '>              " +
            "  <input token-input type='text'                  " +
            "      pre-populate='BusinessServiceInputs'        " +
            "      ng-val='ZipCode'                            " +
            "      autoSelectFirstResult=true                " +
            "      onresultsformatter='OnResultsFormatter'     " +
            "      tokeninputobj='TokenInputObj'               " +
            "      textfield='ZipCode'                         " +
            "      valuefield='ZipCode'                        " +
            "      searchurl='{{GetZipCodeListURL}}'           " +
            "      placeholder='xxxxx'                         " +
            "      customclass='form-control input-sm '   " +
            "      tokenlimit='1' minChars='1' class='zipcode validatealways replaceErrorSource' />   " +
            "       <input placeholder='- xxxx' type='text' class='postzipcode form-control' data-ng-model='PostZipCode' id='ZipCodeID'/> " +
            " </div>",

        replace: true,
    };
});


jQuery.validator.addMethod("zipcoderequired", function (value, element) {
    if ($(element).val() == undefined || $(element).val() == "") {
        $(element).parent('.zipcodediv').find('.token-input-list').attr("data-html", "true")
            .addClass("tooltip-danger")
            .tooltip({ html: true });

        return false;

    } else {

        $(element).parent('.zipcodediv').find('.token-input-list').removeClass("tooltip-danger").tooltip('destroy');
    }
    return true;
}, "ZipCode is Required");
//#endregion

app.directive('validationTimecompare', ['$timeout', function ($timeout) {
    return {
        restrict: 'A',
        scope: {
            validationTimecompare: '=ngModel',
            compareWith: "@"
        },
        link: function (scope, element) {
            $timeout(function () {
                var a = scope.compareWith;//element.context.attributes.getNamedItem("compare-with").value;
                var formId = $(element).closest('form')[0].id;
                if (formId) {
                    $("#" + formId).validate({
                        errorPlacement: $.noop
                    });
                }
                $(element).rules("add", {
                    timecompare: true
                });

                $(a).on("change", function () {
                    $(element).validate();
                });
            });

        }
    };
}]);

app.directive('dateInputMask', ['$filter', function ($filter) {
    return {
        link: function (scope, element, attrs, ctrl) {
            //$(element).inputmask("m/d/y", {
            //    placeholder: "mm/dd/yyyy"
            //});
            $(element).attr("placeholder", "mm/dd/yy");
        }
    };
}]);

app.directive('timeInputMask', ['$filter', function ($filter) {
    return {
        link: function (scope, element, attrs, ctrl) {
            $(element).inputmask({
                mask: "h:s t\\m",
                placeholder: "hh:mm a",
                alias: "datetime",
                hourFormat: "12"
            });
        }
    };
}]);

jQuery.validator.addMethod("timecompare", function (value, element) {
    var a = element.attributes.getNamedItem("compare-with").value;
    if (value && $(a).val()) {
        if (CheckValidTime(value) && CheckValidTime($(a).val())) {
            if (new Date(moment($(a).val(), "hh:mm a")).getTime() > new Date(moment(value, "hh:mm a")).getTime()) {
                if ($(a).val().indexOf('pm') == -1 && $(a).val().indexOf('am') == -1) {
                    $('#lblDayAdded').addClass('hide');
                    return true;
                }
                else {
                    $('#lblDayAdded').removeClass('hide');
                    return true;
                }

            }
        }
    }
    $('#lblDayAdded').addClass('hide');
    return true;
}, "");

//#region Multiple Select selectpicker



app.directive('selectpicker', function () {
    return {
        restrict: 'A',
        scope: {
            selectpicker: '=',
            maxSelection: '@',
            actionsBox: '=',
        },
        link: function (scope, element, attrs) {

            $(element).selectpicker({
                maxOptions: scope.maxSelection ? scope.maxSelection : false,
                style: attrs.buttonstyle != undefined ? attrs.buttonstyle : "btn-sm btn-default",
                liveSearch: true,
                liveSearchPlaceholder: attrs.liveSearchPlaceholder ? attrs.liveSearchPlaceholder : window.TypeInASearchTerm,
                actionsBox: scope.actionsBox ? scope.actionsBox : false
            });
            scope.$watch(function () { return scope.selectpicker; }, function () {
                $(element).selectpicker('val', scope.selectpicker);
            });
            scope.$watch(function () { return $(element).html(); }, function () {
                $(element).selectpicker('refresh');
                $(element).selectpicker('val', scope.selectpicker);
            });
            $(element).change(function () {
                var newVal = $(element).val();
                if ($(element).parent().parent().hasClass('replaceErrorDest')) {
                    var parent = $(element).parent().parent('.replaceErrorDest').parent().find('.replaceErrorSource');
                    if (parent != undefined) {
                        if (newVal != undefined && newVal.length) {
                            $(parent).removeClass("input-validation-error").addClass("valid");
                        } else {
                            $(parent).removeClass("valid").addClass("input-validation-error");
                        }
                    }
                }
                scope.selectpicker = newVal;
                scope.$apply();
            });


        }
        //template: "<input type='text' class='form-control' />",
        //replace: true,
    };
});



app.directive('selectpickerLargedataset', function () {
    return {
        restrict: 'A',
        scope: {
            selectpicker: '=',
            options: '=',
            maxSelection: '@',
            actionsBox: '=',
        },
        link: function (scope, element, attrs) {

            $(element).selectpicker({
                maxOptions: scope.maxSelection ? scope.maxSelection : false,
                style: attrs.buttonstyle != undefined ? attrs.buttonstyle : "btn-sm btn-default",
                liveSearch: true,
                liveSearchPlaceholder: attrs.liveSearchPlaceholder ? attrs.liveSearchPlaceholder : window.TypeInASearchTerm,
                actionsBox: scope.actionsBox ? scope.actionsBox : false
            });
            scope.$watch(function () { return scope.selectpicker; }, function () {
                $(element).selectpicker('val', scope.selectpicker);
            });
            //scope.$watch(function () { return $(element).html(); }, function () {
            //    $(element).selectpicker('refresh');
            //    $(element).selectpicker('val', scope.selectpicker);
            //});
            $(element).change(function () {
                scope.selectpicker = $(element).val();
                scope.$apply();
            });

            $(element).on('show.bs.select', function (e) {
                // do something...

                $(element).selectpicker('refresh');
                $(element).selectpicker('val', scope.selectpicker);
            });

        }
        //template: "<input type='text' class='form-control' />",
        //replace: true,
    };
});


//#endregion

//#region Multiple checkbox dropdown
app.directive('ngDropdownMultiselect', ['$filter', '$document', '$compile', '$parse', function ($filter, $document, $compile, $parse) {

    return {
        restrict: 'AE',
        scope: {
            selectedModel: '=',
            options: '=',
            extraSettings: '=',
            events: '=',
            searchFilter: '=?',
            translationTexts: '=',
            groupBy: '@'
        },
        template: function (element, attrs) {
            var checkboxes = attrs.checkboxes ? true : false;
            var groups = attrs.groupBy ? true : false;

            var template = '<div class="multiselect-parent btn-group dropdown-multiselect">';
            template += '<button type="button" class="dropdown-toggle col-md-12 caretypedropdown multiselectcheckbox" ng-class="settings.buttonClasses" ng-click="toggleDropdown()">{{getButtonText()}}&nbsp;<span class="caret"></span></button>';
            template += '<ul class="dropdown-menu dropdown-menu-form" ng-style="{display: open ? \'block\' : \'none\', height : settings.scrollable ? settings.scrollableHeight : \'auto\' }">';
            template += '<li ng-hide="!settings.showCheckAll || settings.selectionLimit > 0"><a data-ng-click="selectAll()"><span class="glyphicon glyphicon-ok"></span>  {{texts.checkAll}}</a>';
            template += '<li ng-show="settings.showUncheckAll"><a data-ng-click="deselectAll();"><span class="glyphicon glyphicon-remove"></span>   {{texts.uncheckAll}}</a></li>';
            //template += '<li ng-hide="(!settings.showCheckAll || settings.selectionLimit > 0) && !settings.showUncheckAll" class="divider"></li>';
            //template += '<li ng-show="settings.enableSearch"><div class="dropdown-header"><input type="text" class="form-control" style="width: 100%;" ng-model="searchFilter" placeholder="{{texts.searchPlaceholder}}" /></li>';
            //template += '<li ng-show="settings.enableSearch" class="divider"></li>';
            template += '<li ng-show="settings.addEditItem"><button type="button" class="btn btn-primary add-edit-item" data-value="99999999000011" style="margin-left:15px;padding:2px;margin-top:10px;width:85%;"><i class="icon-plus"></i> Add/Edit Item</button></li>';

            if (groups) {
                template += '<li ng-repeat-start="option in orderedItems | filter: searchFilter" ng-show="getPropertyForObject(option, settings.groupBy) !== getPropertyForObject(orderedItems[$index - 1], settings.groupBy)" role="presentation" class="dropdown-header">{{ getGroupTitle(getPropertyForObject(option, settings.groupBy)) }}</li>';
                template += '<li ng-repeat-end role="presentation">';
            } else {
                template += '<li role="presentation" ng-repeat="option in options | filter: searchFilter">';
            }

            template += '<a role="menuitem" tabindex="-1" ng-click="setSelectedItem(getPropertyForObject(option,settings.idProp))">';

            if (checkboxes) {
                template += '<div class=""><label><input class="checkboxInput" data-value="settings.idProp" type="checkbox" ng-click="checkboxClick($event, getPropertyForObject(option,settings.idProp))" ng-checked="isChecked(getPropertyForObject(option,settings.idProp))" /> {{getPropertyForObject(option, settings.displayProp)}}</label></div></a>';
            } else {
                template += '<span data-ng-class="{\'glyphicon glyphicon-ok\': isChecked(getPropertyForObject(option,settings.idProp))}"></span> {{getPropertyForObject(option, settings.displayProp)}}</a>';
            }

            template += '</li>';

            template += '<li class="divider" ng-show="settings.selectionLimit > 1"></li>';
            template += '<li role="presentation" ng-show="settings.selectionLimit > 1"><a role="menuitem">{{selectedModel.length}} {{texts.selectionOf}} {{settings.selectionLimit}} {{texts.selectionCount}}</a></li>';

            template += '</ul>';
            template += '</div>';

            element.html(template);
        },
        link: function ($scope, $element, $attrs) {
            var $dropdownTrigger = $element.children()[0];

            $scope.toggleDropdown = function () {
                $scope.open = !$scope.open;
            };

            $scope.checkboxClick = function ($event, id) {
                $scope.setSelectedItem(id);
                $event.stopImmediatePropagation();
            };

            $scope.externalEvents = {
                onItemSelect: angular.noop,
                onItemDeselect: angular.noop,
                onSelectAll: angular.noop,
                onDeselectAll: angular.noop,
                onInitDone: angular.noop,
                onMaxSelectionReached: angular.noop
            };

            $scope.settings = {
                dynamicTitle: true,
                scrollable: false,
                scrollableHeight: '250px',
                closeOnBlur: true,
                displayProp: 'Name',
                idProp: 'Value',
                externalIdProp: 'Value',
                enableSearch: false,
                addEditItem: false,
                selectionLimit: 0,
                showCheckAll: true,
                showUncheckAll: true,
                closeOnSelect: false,
                buttonClasses: 'btn btn-default',
                closeOnDeselect: false,
                groupBy: $attrs.groupBy || undefined,
                groupByTextProvider: null,
                smartButtonMaxItems: 0,
                smartButtonTextConverter: angular.noop,
                requiredMessage: 'Select'
            };

            $scope.texts = {
                checkAll: 'Check All',
                uncheckAll: 'Uncheck All',
                selectionCount: 'checked',
                selectionOf: '/',
                searchPlaceholder: 'Search...',
                buttonDefaultText: 'Select',
                dynamicButtonTextSuffix: 'checked'
            };

            $scope.searchFilter = $scope.searchFilter || '';

            if (angular.isDefined($scope.settings.groupBy)) {
                $scope.$watch('options', function (newValue) {
                    if (angular.isDefined(newValue)) {
                        $scope.orderedItems = $filter('orderBy')(newValue, $scope.settings.groupBy);
                    }
                });
            }

            angular.extend($scope.settings, $scope.extraSettings || []);
            angular.extend($scope.externalEvents, $scope.events || []);
            angular.extend($scope.texts, $scope.translationTexts);

            $scope.singleSelection = $scope.settings.selectionLimit === 1;

            function getFindObj(id) {
                var findObj = {};

                if ($scope.settings.externalIdProp === '') {
                    findObj[$scope.settings.idProp] = id;
                } else {
                    findObj[$scope.settings.externalIdProp] = id;
                }

                return findObj;
            }

            function clearObject(object) {
                for (var prop in object) {
                    delete object[prop];
                }
            }

            if ($scope.singleSelection) {
                if (angular.isArray($scope.selectedModel) && $scope.selectedModel.length === 0) {
                    clearObject($scope.selectedModel);
                }
            }

            if ($scope.settings.closeOnBlur) {
                $document.on('click', function (e) {
                    var target = e.target.parentElement;
                    var parentFound = false;

                    while (angular.isDefined(target) && target !== null && !parentFound) {
                        if (_.contains(target.className.split(' '), 'multiselect-parent') && !parentFound) {
                            if (target === $dropdownTrigger) {
                                parentFound = true;
                            }
                        }
                        target = target.parentElement;
                    }

                    if (!parentFound) {
                        $scope.$apply(function () {
                            $scope.open = false;
                        });
                    }
                });
            }

            $scope.getGroupTitle = function (groupValue) {
                if ($scope.settings.groupByTextProvider !== null) {
                    return $scope.settings.groupByTextProvider(groupValue);
                }

                return groupValue;
            };

            $scope.getButtonText = function () {
                if ($scope.settings.dynamicTitle && $scope.selectedModel && ($scope.selectedModel.length > 0 || (angular.isObject($scope.selectedModel) && _.keys($scope.selectedModel).length > 0))) {
                    if ($scope.settings.smartButtonMaxItems > 0) {
                        var itemsText = [];
                        var itemsTitleText = [];
                        angular.forEach($scope.options, function (optionItem) {
                            if ($scope.isChecked($scope.getPropertyForObject(optionItem, $scope.settings.idProp))) {
                                var displayText = $scope.getPropertyForObject(optionItem, $scope.settings.displayProp);
                                var converterResponse = $scope.settings.smartButtonTextConverter(displayText, optionItem);

                                itemsText.push(converterResponse ? converterResponse : displayText);
                                itemsTitleText.push(converterResponse ? converterResponse : displayText);
                            }
                        });

                        if ($scope.selectedModel.length > $scope.settings.smartButtonMaxItems) {
                            itemsText = itemsText.slice(0, $scope.settings.smartButtonMaxItems);
                            itemsText.push('...');
                        }
                        if (itemsTitleText.length == 0)
                            $(".multiselectcheckbox").attr("title", $scope.settings.requiredMessage);
                        else
                            $(".multiselectcheckbox").attr("title", itemsTitleText.join(', '));
                        return itemsText.join(', ');
                    } else {
                        var totalSelected;

                        if ($scope.singleSelection) {
                            totalSelected = ($scope.selectedModel !== null && angular.isDefined($scope.selectedModel[$scope.settings.idProp])) ? 1 : 0;
                        } else {
                            totalSelected = angular.isDefined($scope.selectedModel) ? $scope.selectedModel.length : 0;
                        }

                        if (totalSelected === 0) {
                            return $scope.texts.buttonDefaultText;
                        } else {
                            return totalSelected + ' ' + $scope.texts.dynamicButtonTextSuffix;
                        }
                    }
                } else {
                    $(".multiselectcheckbox").attr("title", $scope.settings.requiredMessage);
                    return $scope.texts.buttonDefaultText;
                }
            };

            $scope.getPropertyForObject = function (object, property) {
                if (angular.isDefined(object) && object.hasOwnProperty(property)) {
                    return object[property];
                }

                return '';
            };

            $scope.selectAll = function () {
                $scope.deselectAll(false);
                $scope.externalEvents.onSelectAll();

                angular.forEach($scope.options, function (value) {
                    $scope.setSelectedItem(value[$scope.settings.idProp], true);
                });
            };

            $scope.deselectAll = function (sendEvent) {
                sendEvent = sendEvent || true;

                if (sendEvent) {
                    $scope.externalEvents.onDeselectAll();
                }

                if ($scope.singleSelection) {
                    clearObject($scope.selectedModel);
                } else {
                    $scope.selectedModel.splice(0, $scope.selectedModel.length);
                }
            };

            $scope.setSelectedItem = function (id, dontRemove) {
                var findObj = getFindObj(id);
                var finalObj = null;

                if ($scope.settings.externalIdProp === '') {
                    finalObj = _.find($scope.options, findObj);
                } else {
                    finalObj = findObj;
                }

                if ($scope.singleSelection) {
                    clearObject($scope.selectedModel);
                    angular.extend($scope.selectedModel, finalObj);
                    $scope.externalEvents.onItemSelect(finalObj);
                    if ($scope.settings.closeOnSelect) $scope.open = false;

                    return;
                }

                dontRemove = dontRemove || false;

                var exists = _.findIndex($scope.selectedModel, findObj) !== -1;

                if (!dontRemove && exists) {
                    $scope.selectedModel.splice(_.findIndex($scope.selectedModel, findObj), 1);
                    $scope.externalEvents.onItemDeselect(findObj);
                } else if (!exists && ($scope.settings.selectionLimit === 0 || $scope.selectedModel.length < $scope.settings.selectionLimit)) {
                    $scope.selectedModel.push(finalObj);
                    $scope.externalEvents.onItemSelect(finalObj);
                }
                if ($scope.settings.closeOnSelect) $scope.open = false;
            };

            $scope.isChecked = function (id) {
                if ($scope.singleSelection) {
                    return $scope.selectedModel !== null && angular.isDefined($scope.selectedModel[$scope.settings.idProp]) && $scope.selectedModel[$scope.settings.idProp] === getFindObj(id)[$scope.settings.idProp];
                }

                return _.findIndex($scope.selectedModel, getFindObj(id)) !== -1;
            };

            $scope.externalEvents.onInitDone();
        }
    };
}]);


app.directive('ngDropdownMultiselectAll', ['$filter', '$document', '$compile', '$parse', function ($filter, $document, $compile, $parse) {

    return {
        restrict: 'AE',
        scope: {
            selectedModel: '=',
            options: '=',
            extraSettings: '=',
            events: '=',
            searchFilter: '=?',
            translationTexts: '=',
            groupBy: '@'
        },
        template: function (element, attrs) {
            var checkboxes = attrs.checkboxes ? true : false;
            var groups = attrs.groupBy ? true : false;

            var template = '<div class="multiselect-parent btn-group dropdown-multiselect">';
            template += '<button type="button" class="dropdown-toggle col-md-12 caretypedropdown multiselectcheckbox" ng-class="settings.buttonClasses" ng-click="toggleDropdown()">{{getButtonText()}}&nbsp;<span class="caret"></span></button>';
            template += '<ul class="dropdown-menu dropdown-menu-form" ng-style="{display: open ? \'block\' : \'none\', height : settings.scrollable ? settings.scrollableHeight : \'auto\' }">';
            template += '<li><a data-ng-click="checkAllDays()"><input id="checkalldays" type="checkbox" ng-checked="isSelectAll" ng-model="items.allItemsSelected"/> {{texts.selectAll}}</a>';
            //template += '<li ng-hide="!settings.showCheckAll || settings.selectionLimit > 0"><a data-ng-click="selectAll()"><input type="checkbox" name="chkAll" checked="true"/> {{texts.checkAll}}</a>';
            //template += '<li ng-show="settings.showUncheckAll"><a data-ng-click="deselectAll();"><input type="checkbox" name="uncheckAll" checked="false"/>  {{texts.uncheckAll}}</a></li>';
            //template += '<li ng-hide="(!settings.showCheckAll || settings.selectionLimit > 0) && !settings.showUncheckAll" class="divider"></li>';
            //template += '<li ng-show="settings.enableSearch"><div class="dropdown-header"><input type="text" class="form-control" style="width: 100%;" ng-model="searchFilter" placeholder="{{texts.searchPlaceholder}}" /></li>';
            //template += '<li ng-show="settings.enableSearch" class="divider"></li>';
            template += '<li ng-show="settings.addEditItem"><button type="button" class="btn btn-primary add-edit-item" data-value="99999999000011" style="margin-left:15px;padding:2px;margin-top:10px;width:85%;"><i class="icon-plus"></i> Add/Edit Item</button></li>';

            if (groups) {
                template += '<li ng-repeat-start="option in orderedItems | filter: searchFilter" ng-show="getPropertyForObject(option, settings.groupBy) !== getPropertyForObject(orderedItems[$index - 1], settings.groupBy)" role="presentation" class="dropdown-header">{{ getGroupTitle(getPropertyForObject(option, settings.groupBy)) }}</li>';
                template += '<li ng-repeat-end role="presentation">';
            } else {
                template += '<li role="presentation" ng-repeat="option in options | filter: searchFilter">';
            }

            template += '<a role="menuitem" tabindex="-1" ng-click="setSelectedItem(getPropertyForObject(option,settings.idProp))">';

            if (checkboxes) {
                template += '<div class=""><label><input class="checkboxInput" data-value="settings.idProp" type="checkbox" ng-click="checkboxClick($event, getPropertyForObject(option,settings.idProp))" ng-checked="isChecked(getPropertyForObject(option,settings.idProp))" /> {{getPropertyForObject(option, settings.displayProp)}}</label></div></a>';
            } else {
                template += '<span data-ng-class="{\'glyphicon glyphicon-ok\': isChecked(getPropertyForObject(option,settings.idProp))}"></span> {{getPropertyForObject(option, settings.displayProp)}}</a>';
            }

            template += '</li>';

            template += '<li class="divider" ng-show="settings.selectionLimit > 1"></li>';
            template += '<li role="presentation" ng-show="settings.selectionLimit > 1"><a role="menuitem">{{selectedModel.length}} {{texts.selectionOf}} {{settings.selectionLimit}} {{texts.selectionCount}}</a></li>';

            template += '</ul>';
            template += '</div>';

            element.html(template);
        },
        link: function ($scope, $element, $attrs) {
            var $dropdownTrigger = $element.children()[0];

            $scope.toggleDropdown = function () {
                $scope.open = !$scope.open;
            };

            $scope.checkboxClick = function ($event, id) {
                $scope.setSelectedItem(id);
                $event.stopImmediatePropagation();
            };

            $scope.externalEvents = {
                onItemSelect: angular.noop,
                onItemDeselect: angular.noop,
                onSelectAll: angular.noop,
                onDeselectAll: angular.noop,
                onInitDone: angular.noop,
                onMaxSelectionReached: angular.noop
            };

            $scope.settings = {
                dynamicTitle: true,
                scrollable: false,
                scrollableHeight: '250px',
                closeOnBlur: true,
                displayProp: 'Name',
                idProp: 'Value',
                externalIdProp: 'Value',
                enableSearch: false,
                addEditItem: false,
                selectionLimit: 0,
                showCheckAll: true,
                showUncheckAll: true,
                closeOnSelect: false,
                buttonClasses: 'btn btn-default',
                closeOnDeselect: false,
                groupBy: $attrs.groupBy || undefined,
                groupByTextProvider: null,
                smartButtonMaxItems: 0,
                smartButtonTextConverter: angular.noop,
                requiredMessage: 'Select'
            };

            $scope.texts = {
                checkAll: 'Check All',
                uncheckAll: 'Uncheck All',
                selectionCount: 'checked',
                selectionOf: '/',
                searchPlaceholder: 'Search...',
                buttonDefaultText: 'Select',
                dynamicButtonTextSuffix: 'checked',
                selectAll: 'Select All Day'
            };

            $scope.searchFilter = $scope.searchFilter || '';

            if (angular.isDefined($scope.settings.groupBy)) {
                $scope.$watch('options', function (newValue) {
                    if (angular.isDefined(newValue)) {
                        $scope.orderedItems = $filter('orderBy')(newValue, $scope.settings.groupBy);
                    }
                });
            }

            angular.extend($scope.settings, $scope.extraSettings || []);
            angular.extend($scope.externalEvents, $scope.events || []);
            angular.extend($scope.texts, $scope.translationTexts);

            $scope.singleSelection = $scope.settings.selectionLimit === 1;

            function getFindObj(id) {
                var findObj = {};

                if ($scope.settings.externalIdProp === '') {
                    findObj[$scope.settings.idProp] = id;
                } else {
                    findObj[$scope.settings.externalIdProp] = id;
                }

                return findObj;
            }

            function clearObject(object) {
                for (var prop in object) {
                    delete object[prop];
                }
            }

            if ($scope.singleSelection) {
                if (angular.isArray($scope.selectedModel) && $scope.selectedModel.length === 0) {
                    clearObject($scope.selectedModel);
                }
            }

            if ($scope.settings.closeOnBlur) {
                $document.on('click', function (e) {
                    var target = e.target.parentElement;
                    var parentFound = false;

                    while (angular.isDefined(target) && target !== null && !parentFound) {
                        if (_.contains(target.className.split(' '), 'multiselect-parent') && !parentFound) {
                            if (target === $dropdownTrigger) {
                                parentFound = true;
                            }
                        }
                        target = target.parentElement;
                    }

                    if (!parentFound) {
                        $scope.$apply(function () {
                            $scope.open = false;
                        });
                    }
                });
            }

            $scope.getGroupTitle = function (groupValue) {
                if ($scope.settings.groupByTextProvider !== null) {
                    return $scope.settings.groupByTextProvider(groupValue);
                }

                return groupValue;
            };

            $scope.getButtonText = function () {
                if ($scope.settings.dynamicTitle && $scope.selectedModel && ($scope.selectedModel.length > 0 || (angular.isObject($scope.selectedModel) && _.keys($scope.selectedModel).length > 0))) {
                    if ($scope.settings.smartButtonMaxItems > 0) {
                        var itemsText = [];
                        var itemsTitleText = [];
                        angular.forEach($scope.options, function (optionItem) {
                            if ($scope.isChecked($scope.getPropertyForObject(optionItem, $scope.settings.idProp))) {
                                var displayText = $scope.getPropertyForObject(optionItem, $scope.settings.displayProp);
                                var converterResponse = $scope.settings.smartButtonTextConverter(displayText, optionItem);

                                itemsText.push(converterResponse ? converterResponse : displayText);
                                itemsTitleText.push(converterResponse ? converterResponse : displayText);
                            }
                        });

                        if ($scope.selectedModel.length > $scope.settings.smartButtonMaxItems) {
                            itemsText = itemsText.slice(0, $scope.settings.smartButtonMaxItems);
                            itemsText.push('...');
                        }
                        if (itemsTitleText.length == 0)
                            $(".multiselectcheckbox").attr("title", $scope.settings.requiredMessage);
                        else
                            $(".multiselectcheckbox").attr("title", itemsTitleText.join(', '));
                        return itemsText.join(', ');
                    } else {
                        var totalSelected;

                        if ($scope.singleSelection) {
                            totalSelected = ($scope.selectedModel !== null && angular.isDefined($scope.selectedModel[$scope.settings.idProp])) ? 1 : 0;
                        } else {
                            totalSelected = angular.isDefined($scope.selectedModel) ? $scope.selectedModel.length : 0;
                        }

                        if (totalSelected === 0) {
                            return $scope.texts.buttonDefaultText;
                        } else {
                            return totalSelected + ' ' + $scope.texts.dynamicButtonTextSuffix;
                        }
                    }
                } else {
                    $(".multiselectcheckbox").attr("title", $scope.settings.requiredMessage);
                    return $scope.texts.buttonDefaultText;
                }
            };

            $scope.getPropertyForObject = function (object, property) {
                if (angular.isDefined(object) && object.hasOwnProperty(property)) {
                    return object[property];
                }

                return '';
            };

            $scope.selectAll = function () {
                $scope.deselectAll(false);
                $scope.externalEvents.onSelectAll();

                angular.forEach($scope.options, function (value) {
                    $scope.setSelectedItem(value[$scope.settings.idProp], true);
                });
            };

            $scope.deselectAll = function (sendEvent) {
                sendEvent = sendEvent || true;

                if (sendEvent) {
                    $scope.externalEvents.onDeselectAll();
                }

                if ($scope.singleSelection) {
                    clearObject($scope.selectedModel);
                } else {
                    $scope.selectedModel.splice(0, $scope.selectedModel.length);
                }
            };

            $scope.isSelectAll = false;
            $scope.checkAllDays = function () {
                if (!$scope.isSelectAll) {
                    $scope.isSelectAll = true;
                    $scope.selectAll();
                } else {
                    $scope.isSelectAll = false;
                    $scope.deselectAll();
                }
            }

            $scope.setSelectedItem = function (id, dontRemove) {
                var findObj = getFindObj(id);
                var finalObj = null;

                if ($scope.settings.externalIdProp === '') {
                    finalObj = _.find($scope.options, findObj);
                } else {
                    finalObj = findObj;
                }

                if ($scope.singleSelection) {
                    clearObject($scope.selectedModel);
                    angular.extend($scope.selectedModel, finalObj);
                    $scope.externalEvents.onItemSelect(finalObj);
                    if ($scope.settings.closeOnSelect) $scope.open = false;

                    return;
                }

                dontRemove = dontRemove || false;

                var exists = _.findIndex($scope.selectedModel, findObj) !== -1;

                if (!dontRemove && exists) {
                    $scope.selectedModel.splice(_.findIndex($scope.selectedModel, findObj), 1);
                    $scope.externalEvents.onItemDeselect(findObj);
                } else if (!exists && ($scope.settings.selectionLimit === 0 || $scope.selectedModel.length < $scope.settings.selectionLimit)) {
                    $scope.selectedModel.push(finalObj);
                    $scope.externalEvents.onItemSelect(finalObj);
                }
                if ($scope.settings.closeOnSelect) $scope.open = false;

                if ($scope.selectedModel.length == 7) {
                    $scope.isSelectAll = true;
                }
                else {
                    $scope.isSelectAll = false;
                }
            };

            $scope.isChecked = function (id) {
                if ($scope.singleSelection) {
                    return $scope.selectedModel !== null && angular.isDefined($scope.selectedModel[$scope.settings.idProp]) && $scope.selectedModel[$scope.settings.idProp] === getFindObj(id)[$scope.settings.idProp];
                }

                return _.findIndex($scope.selectedModel, getFindObj(id)) !== -1;
            };

            $scope.externalEvents.onInitDone();
        }
    };
}]);
//#endregion




//#region Directive For Signature JS
app.directive('dirSignature', ['$compile', function ($compile) {
    return {
        restrict: 'C',
        scope: {
            ngValue: "=",
        },
        link: function (scope, elem, attrs) {

            //scope.ngValue = '{"lines":[[[38.25,92],[38.25,91],[38.25,88],[38.25,83],[40.25,72],[43.25,56],[45.25,38],[45.25,23],[45.25,14],[45.25,11],[45.25,13],[45.25,16],[46.25,20],[46.25,27],[47.25,41],[51.25,55],[52.25,69],[53.25,82],[56.25,88],[56.25,94],[56.25,97],[56.25,98],[56.25,99],[55.25,101],[53.25,101],[52.25,101],[49.25,102],[41.25,102],[32.25,105],[26.25,106],[25.25,106],[24.25,106]],[[70.25,76],[70.25,78],[70.25,81],[70.25,84],[70.25,95],[71.25,99],[71.25,102]],[[73.25,50],[72.25,50],[69.25,50],[68.25,51],[65.25,54],[63.25,55],[63.25,56],[63.25,57],[63.25,58],[65.25,58],[68.25,56]],[[110.25,32],[110.25,33],[110.25,36],[109.25,40],[108.25,53],[108.25,65],[108.25,83],[108.25,99],[113.25,111],[113.25,112]],[[103.25,89],[106.25,89],[110.25,89],[115.25,89],[118.25,89],[127.25,86],[132.25,84],[134.25,82]],[[135.25,80],[136.25,80],[137.25,80],[138.25,80],[139.25,80],[140.25,80],[143.25,77],[144.25,75],[144.25,73],[145.25,68],[145.25,63],[144.25,60],[138.25,59],[131.25,59],[123.25,66],[118.25,78],[116.25,93],[116.25,107],[124.25,121],[133.25,129],[142.25,134],[150.25,136],[154.25,136],[162.25,126],[166.25,117]],[[151.25,80],[150.25,79],[151.25,79],[155.25,82],[160.25,89],[161.25,94],[166.25,107],[168.25,117],[168.25,122],[168.25,124],[167.25,119],[167.25,114],[167.25,108],[170.25,95],[173.25,87],[176.25,83],[177.25,82],[179.25,83],[180.25,85],[181.25,90],[182.25,92],[184.25,102],[184.25,109],[186.25,115],[187.25,118]],[[205.25,79],[204.25,79],[204.25,82],[204.25,85],[203.25,94],[202.25,104],[202.25,111],[202.25,116],[204.25,117],[205.25,117],[207.25,117],[212.25,109],[216.25,96],[217.25,82],[219.25,69],[217.25,55],[212.25,46],[211.25,42],[210.25,42],[210.25,44],[211.25,53],[213.25,63],[216.25,77],[217.25,86],[218.25,91],[219.25,96]],[[230.25,89],[230.25,90],[230.25,91],[231.25,92],[232.25,92],[233.25,92],[235.25,87],[237.25,84],[237.25,79],[237.25,68],[237.25,60],[236.25,54],[234.25,54],[230.25,54],[226.25,60],[226.25,67],[226.25,74],[229.25,78],[232.25,78],[236.25,78],[240.25,78],[242.25,78],[243.25,79],[246.25,81],[248.25,85],[248.25,87],[248.25,90],[248.25,93],[248.25,95],[248.25,97],[248.25,98],[249.25,97]],[[254.25,59],[253.25,59],[253.25,64],[253.25,68],[253.25,75],[254.25,81],[256.25,84],[257.25,84],[259.25,84],[262.25,84],[267.25,72],[271.25,62],[272.25,56],[272.25,54],[272.25,55],[272.25,59],[272.25,68],[272.25,76],[272.25,85],[273.25,92]]]}';
            scope.SignatureChanged = function () {
                var data = $(elem).signature('toJSON');
                if (data == '{"lines":[]}')
                    data = null;
                scope.ngValue = data;
                if (!scope.$root.$$phase)
                    scope.$apply();
            };


            $(elem).signature({
                background: '#ffffff', // Colour of the background 
                color: '#000000', // Colour of the signature 
                thickness: 2, // Thickness of the lines 
                guideline: true, // Add a guide line or not? 
                guidelineColor: '#a0a0a0', // Guide line colour guidelineColor: '#008000'
                guidelineOffset: 25, // Guide line offset from the bottom 
                guidelineIndent: 10, // Guide line indent from the edges 
                // Error message when no canvas 
                notAvailable: 'Your browser doesn\'t support signing',
                syncField: null, // Selector for synchronised text field 
                change: scope.SignatureChanged, // Callback when signature changed 
                disabled: attrs.ngEnable == 'false' ? true : false
            });



            scope.$watch(function () { return scope.ngValue; }, function (newValue, oldValue) {
                if (scope.ngValue)
                    $(elem).signature('draw', scope.ngValue);
            });



            var clrSign = $(elem).next();
            if ($(clrSign).hasClass('clearSignature')) {
                $(document).on('click', '.clearSignature', function () {
                    $(elem).signature('clear');
                });
            }

        }
    };
}]);
//#endregion

//#endregion

//#region Additional UI Level Plugins And Attribute Related Directive Stuff

//#region Directive For Show Error Count on Form Validation
app.directive('validateElement', function () {

    return {
        restrict: 'C',
        //require: 'ngModel',
        scope: {
            errorCount: "=",
            //model: '=ngModel'
        },
        link: function (scope, element, attrs, ctrl) {

            function checkElementError() {
                setTimeout(function () {
                    scope.$apply(function () {

                        var uniqueId = $(element).attr('id');

                        if ($(element).hasClass('groupError')) {
                            //tooltip - danger
                            //CUSTOMIZE: NO NEED TO Take this scope for new implementation
                            if (($(element).val() == '' || $(element).val() == undefined || $(element).val() == null) && $(element).parents('.groupErrorParent').hasClass('tooltip-danger')) {
                                if (scope.errorCount.indexOf(uniqueId) == -1)
                                    scope.errorCount.push(uniqueId);
                            } else {
                                if (scope.errorCount.length > 0 && scope.errorCount.indexOf(uniqueId) >= 0)
                                    scope.errorCount.remove(uniqueId);
                            }


                        } else {

                            if ($(element).hasClass('input-validation-error')) {
                                if (scope.errorCount.indexOf(uniqueId) == -1)
                                    scope.errorCount.push(uniqueId);
                                //} else {
                                //    //try {
                                //    if (scope.errorCount.length > 0 && scope.errorCount.indexOf(uniqueId) >= 0)
                                //        scope.errorCount.remove(uniqueId);
                                //    //} catch (ex) {
                                //    //    
                                //    //}
                            }
                        }

                        //console.log("LOG");
                        //clearTimeout();
                    });

                }, 500);
            }


            $(element).change(function () {
                checkElementError();
            });

            $(element).on("keyup", function () {
                checkElementError();
            });

            $(element).on("onblur", function () {
                checkElementError();
            });
            //scope.$watch("model", function (data) {
            //    checkElementError();
            //});


            scope.$watch(function () { return element.attr('class'); }, function (newValue, oldValue) {
                checkElementError();
            });


        },
        replace: true,
    };
});
//#endregion

//#region Directive For Side Bar Menu Selection
app.directive('selectmenu', function () {

    return {
        restrict: 'A',
        scope: {
            selectmenu: "@"
        },
        link: function (scope, element, attrs) {
            if (scope.selectmenu == window.SelectedMenuItem) {
                $(element).addClass("active open");
                if ($(element).parents(".nav-item")) {
                    $.each($(element).parents(".nav-item"), function (index, data) {
                        $(this).addClass("active open");
                        $(this).find("> a .arrow").click();
                    });
                }

            }
        },
        replace: true,
    };
});
//#endregion

//#region Directive For Table Header Fix

app.directive('tableHeadFixer', ['$timeout', function ($timeout) {
    return {
        restrict: 'A',
        scope: {
            val: "="
        },
        link: function (scope, el, attrs) {
            if (attrs.head == undefined)
                attrs.head = false;

            if (attrs.foot == undefined)
                attrs.foot = false;

            if (attrs.left == undefined)
                attrs.left = 0;

            if (attrs.right == undefined)
                attrs.right = 0;

            scope.$watch(function () { return scope.val; }, function (newValue, oldValue) {
                $(el).parent().removeAttr('style');
                if (scope.val.length > 0) {
                    $timeout(function () { $(el).tableHeadFixer({ "head": attrs.head, "foot": attrs.foot, "left": attrs.left, "right": attrs.right }); });
                }
            });
        }
    };
}]);

//#endregion

//#region Directive For Convert DropDown Selection into 'Number' and 'Boolean'

app.directive('onlyDigits', function () {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, element, attr, ctrl) {
            function inputValue(val) {
                if (val) {
                    var digits = val.replace(/[^0-9]/g, '');

                    if (digits !== val) {
                        ctrl.$setViewValue(digits);
                        ctrl.$render();
                    }
                    return parseInt(digits, 10);
                }
                return undefined;
            }
            ctrl.$parsers.push(inputValue);
        }
    };
});

app.directive('convertToNumber', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            ngModel.$parsers.push(function (val) {
                return val ? parseInt(val, 10) : null;
            });
            ngModel.$formatters.push(function (val) {
                return val ? '' + val : null;
            });
        }
    };
});
app.directive('convertToBoolean', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            ngModel.$parsers.push(function (value) {
                return value == 'true' || value == true;
            });
            ngModel.$formatters.push(function (value) {
                return value && value != 'false' ? 'true' : 'false';
            });
        }
    };
});

//#endregion

//#region Directive For Set Name for DropDown Selection
app.directive('ngSetname', function () {
    return {
        restrict: 'A',
        scope: {
            ngSetname: "="
        },
        link: function (scope, element, attrs) {
            $(element).change(function () {
                scope.ngSetname = $(element).find("option:selected").text();
                scope.$apply();
            });
        }
    };
});
//#endregion

//#region Directive For Set Phone Number Input Masking
app.directive('phoneInput', ['$filter', '$browser', function ($filter, $browser) {
    return {
        require: 'ngModel',
        link: function ($scope, $element, $attrs, ngModelCtrl) {
            var listener = function () {
                var value = $element.val().replace(/[^0-9]/g, '');
                $element.val($filter('tel')(value, false));
            };

            // This runs when we update the text field
            ngModelCtrl.$parsers.push(function (viewValue) {
                return viewValue.replace(/[^0-9]/g, '').slice(0, 10);
            });

            // This runs when the model gets updated on the scope directly and keeps our view in sync
            ngModelCtrl.$render = function () {
                $element.val($filter('tel')(ngModelCtrl.$viewValue, false));
            };

            $element.bind('change', listener);
            $element.bind('keydown', function (event) {
                var key = event.keyCode;
                // If the keys include the CTRL, SHIFT, ALT, or META keys, or the arrow keys, do nothing.
                // This lets us support copy and paste too
                if (key == 91 || (15 < key && key < 19) || (37 <= key && key <= 40)) {
                    return;
                }
                $browser.defer(listener); // Have to do this or changes don't get picked up properly
            });

            $element.bind('paste cut', function () {
                $browser.defer(listener);
            });
        }

    };
}]);


app.directive('phoneFormat', ['$filter', '$browser', function ($filter, $browser) {
    return {
        restrict: 'A',
        scope: {
            phoneFormat: "="
        },
        link: function ($scope, $element, $attrs, ngModelCtrl) {
            if ($scope.phoneFormat != undefined)
                $scope.phoneFormat = $scope.phoneFormat.replace(/(\d{3})(\d{3})(\d{4})/, "($1) $2-$3");

        }

    };
}]);
//Referral_CISNumber

//#endregion

//#region Directive For Nice Scroll
app.directive('ngNicescroll', function () {
    return {
        restrict: 'A',
        scope: {
        },
        link: function (scope, element, attrs) {
            $(element).niceScroll({
                horizrailenabled: false,
                cursorcolor: "rgba(0,0,0,0.4)"
            });
        }
    };
});


//#endregion

//#region Directive For ToolTip
app.directive('ngTooltip', function () {
    return {
        restrict: 'A',
        scope: {
            ngTooltip: "@",
            ngCustomClass: "@",
            ngbgColorClass: "@",
            ngbgArrowColorClass: "@"
        },
        link: function (scope, element, attrs) {
            $(element).attr("title", scope.ngTooltip);
            if (scope.ngCustomClass != undefined) {
                template = '<div class="tooltip {0}"><div class="tooltip-arrow {2}"></div><div class="tooltip-inner {1}"></div></div>'.format(scope.ngCustomClass, scope.ngbgColorClass, scope.ngbgArrowColorClass);
                $(element).tooltip({
                    container: 'body',
                    html: true,
                    animation: true,
                    template: template
                });
            } else {

                $(element).tooltip({
                    container: 'body',
                    html: true,
                    animation: true
                });
            }

        }
    };
});
//#endregion

//#region Directive For KeyPress Validation
app.directive('keyPressValidation', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var valications = {
                Digit: function (objTextbox, event) {
                    var keyCode = (event.which) ? event.which : (window.event) ? window.event.keyCode : -1;
                    if (keyCode >= 48 && keyCode <= 57) {
                        return true;
                    }
                    if (keyCode == 8 || keyCode == -1) {
                        return true;
                    } else {
                        return false;
                    }
                }
            };

            var validation = attrs.keyPressValidation;

            jQuery(element).keypress(function (event) {
                return valications[validation](this, event);
            }).bind('paste', function (e) {
                // return false;
                var regex = /\d+/g;
                var string = e.originalEvent.clipboardData.getData('text');
                var matches = string.match(regex);  // creates array from matches

                jQuery(element).val(matches);
                return false;

            });



        }
    };
});
//#endregion

//#region Directive For Enter Event Handle
app.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {

                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter, { 'event': event });
                });

                event.preventDefault();
            }
        });
    };
});
//#endregion

app.directive('htmlBind', function () {
    return {
        restrict: 'A',
        scope: {
            htmlBind: "&",

        },
        link: function (scope, element, attrs) {
            $(element).html(scope.htmlBind);
            //$(element).html($(element).find('table').html());

        }
    };
});


app.directive('htmlBindDynamic', function () {
    return {
        restrict: 'A',
        scope: {
            htmlBindDynamic: "=",

        },
        link: function (scope, element, attrs) {


            scope.$watch(function () { return scope.htmlBindDynamic; }, function (newVal, oldVal) {
                $(element).html(scope.htmlBindDynamic);
                console.log("Hello Run this item.");
            });
        }
    };
});

//#endregion

//#region  Date/DateTime/Calender Related Directive Stuff

//#region Directive For Date and DateTime Picker Control
app.directive('datepicker', function () {
    return {
        restrict: 'A',
        scope: {
            ngDateval: "=",
            ngMaxdate: "=?",
            ngMindate: "=?"
        },
        link: function (scope, element, attrs) {
            var serverSideDateFormat = "YYYY/MM/DD";
            //console.log("Datepicker linked");
            //data-ng-maxdate="now"Referral_ZSPRespiteExpirationDate
            if (attrs.datepicker == "ServiceCodeModel.PayorServiceCodeMapping.POSStartDate") {
            }
            if (scope.ngDateval == null || scope.ngDateval == undefined || scope.ngDateval.toString() === "0001-01-01T00:00:00" || scope.ngDateval.toString() === "0001-01-01T00:00:00Z") {
                scope.ngDateval = "";
                $(element).find('input').val("");




            }

            if (scope.ngMaxdate == "0001-01-01T00:00:00Z") {
                scope.ngMaxdate = undefined;
            }
            //if (attrs.datepicker == "ServiceCodeModel.PayorServiceCodeMapping.POSStartDate") {

            //}
            //scope.asd = GetOrgDateFormat(); 
            var dateformat = GetOrgDateFormat(); //'DD/MM/YYYY';
            $(element).datetimepicker(
                {
                    maxDate: scope.ngMaxdate,
                    locale: moment.locale('en', {
                        week: {
                            dow: ValideElement(window.CalWeekStartDay) ? parseInt(window.CalWeekStartDay) : 1
                        }
                    }),
                    //format: 'DD/MM/YYYY',
                    format: dateformat,
                    //useStrict: true,
                    //keepOpen: true,
                    useCurrent: false,
                    minDate: scope.ngMindate
                }
            );



            if (!$(element).is('input')) {
                $(element).find('input').on("focusin", function () {
                    $(element).data("DateTimePicker").show();
                });

                $(element).find('input').on("focusout", function () {

                    //$(element).data("DateTimePicker").show();
                });
            } else {
                $(element).on("focusin", function () {
                    $(element).data("DateTimePicker").show();
                });

                $(element).on("focusout", function () {
                    //alert(2);
                    //$(element).data("DateTimePicker").show();
                });
            }


            $(element).on("dp.change", function (e) {
                //scope.ngDateval = "2015/01/01";
                if (e.date) {
                    var date = new Date(e.date.format());
                    scope.ngDateval = moment(date).format(serverSideDateFormat);
                } else {

                    scope.ngDateval = null;
                }
                if (!scope.$root.$$phase) {
                    scope.$apply();
                }
            });



            scope.$watch('ngMaxdate', function (value, oldvalue) {
                //if (value != null && new Date(value) != 'Invalid Date' && scope.ngDateval != null && scope.ngDateval.toString() != "0001-01-01T00:00:00" && value != "0001-01-01T00:00:00") {
                if (ValideElement(value) && new Date(value) != 'Invalid Date') {
                    $(element).data("DateTimePicker").maxDate(moment(value));
                }
                else {
                    $(element).data("DateTimePicker").maxDate(false);
                }
            });

            scope.$watch('ngMindate', function (value, oldvalue) {
                //if (value != null && new Date(value) != 'Invalid Date' && scope.ngDateval != null && scope.ngDateval.toString() != "0001-01-01T00:00:00" && value != "0001-01-01T00:00:00") {
                if (ValideElement(value) && new Date(value) != 'Invalid Date') {
                    $(element).data("DateTimePicker").minDate(moment(value));
                }
                else {
                    $(element).data("DateTimePicker").minDate(false);
                }
            });

            scope.$watch(function () { return scope.ngDateval; }, function (value, oldvalue) {
                //if (scope.ngDateval == "2016-07-09T22:34:55") {
                //    
                //}
                if (scope.ngDateval == null || scope.ngDateval == undefined || scope.ngDateval.toString() === "0001-01-01T00:00:00" || scope.ngDateval == "") {

                    //scope.ngDateval = "";
                    //change for when not getting date at that time clear the date
                    $(element).data("DateTimePicker").date(null);
                    //$(element).find('input').val("");
                } else {
                    //if (new Date(value) != 'Invalid Date')                     
                    $(element).data("DateTimePicker").date(moment(value));
                }
            });

            if (scope.ngDateval) {
                $(element).data("DateTimePicker").date(moment(scope.ngDateval));
            }

        }
    };
});

app.directive('datetimepicker', function () {
    return {
        restrict: 'A',
        scope: {
            ngDateval: "=",
            ngMaxdate: "=",
            ngMindate: "="
        },
        link: function (scope, element, attrs) {
            var serverSideDateTimeFormat = "YYYY/MM/DD HH:mm:ss";
            if (scope.ngDateval == null || scope.ngDateval == undefined || scope.ngDateval == "" || scope.ngDateval.toString() === "0001-01-01T00:00:00" || scope.ngDateval.toString() === "0001-01-01T00:00:00Z") {
                scope.ngDateval = "";
                $(element).find('input').val("");
            }
            var dateformat = GetOrgDateFormat();
            $(element).datetimepicker(
                {
                    locale: moment.locale('en', {
                        week: {
                            dow: ValideElement(window.CalWeekStartDay) ? parseInt(window.CalWeekStartDay) : 1
                        }
                    }),
                    format: dateformat + ' hh:mm a',
                    maxDate: scope.ngMaxdate,
                    minDate: scope.ngMindate,
                    useCurrent: false,
                }
            );
            if (!$(element).is('input')) {

                $(element).find('input').on("focusin", function () {
                    $(element).data("DateTimePicker").show();
                });
            } else {
                $(element).on("focusin", function () {
                    $(element).data("DateTimePicker").show();
                });
            }

            $(element).on("dp.change", function (e) {
                if (e.date) {
                    var date = new Date(e.date.format());
                    scope.ngDateval = moment(date).format(serverSideDateTimeFormat);
                } else {
                    scope.ngDateval = null;
                }
                if (!scope.$root.$$phase) {
                    scope.$apply();
                }
            });

            scope.$watch('ngMindate', function (value, oldvalue) {
                if (ValideElement(value) && new Date(value) != 'Invalid Date') {
                    $(element).data("DateTimePicker").minDate(value);
                }
                else {
                    $(element).data("DateTimePicker").minDate(false);
                }
            });
            scope.$watch('ngMaxdate', function (value, oldvalue) {
                if (ValideElement(value) && new Date(value) != 'Invalid Date') {
                    $(element).data("DateTimePicker").maxDate(value);
                }
                else {
                    $(element).data("DateTimePicker").maxDate(false);
                }
            });
            scope.$watch('ngDateval', function (value, oldvalue) {
                if (value != null && new Date(value) != 'Invalid Date') {
                    $(element).data("DateTimePicker").date(moment(value));
                }
                else {
                    $(element).data("DateTimePicker").date(null);
                }

            });

            scope.$watch(function () { return scope.ngDateval; }, function (value, oldvalue) {
                if (scope.ngDateval == null || scope.ngDateval == undefined || scope.ngDateval.toString() === "0001-01-01T00:00:00" || scope.ngDateval == "") {
                    $(element).data("DateTimePicker").date(null);
                } else {
                    $(element).data("DateTimePicker").date(moment(value));
                }
            });

            if (scope.ngDateval) {
                $(element).data("DateTimePicker").date(moment(scope.ngDateval));
            }

        }
    };
});


//#endregion

//#region Directive For Full Calender Control

//*For Calender
app.directive('calenderDropper', function () {
    return {
        restrict: 'A',
        scope: {
            calenderDropper: '=',
            revertDrag: '@'
        },
        link: function (scope, element, attrs) {
            // store the Event Object in the DOM element so we can get to it later
            if (scope.revertDrag == undefined || scope.revertDrag.toLowerCase() === "true" || scope.revertDrag.toLowerCase() === "1")
                scope.revertDrag = true;
            else
                scope.revertDrag = false;

            element.data('eventObject', scope.calenderDropper);
            // make the event draggable using jQuery UI
            if (scope.calenderDropper.event && scope.calenderDropper.event.editable === false) {
                return;
            }
            element.draggable({
                zIndex: 999,
                snap: true,
                scroll: false,
                revert: scope.revertDrag, // will cause the event to go back to its
                revertDuration: 0, //  original position after the drag
                start: function () {
                    $('.selected:not(.ui-draggable-dragging)').addClass('dragging').hide();

                },
                stop: function () {
                    $('.dragging').removeClass('dragging').show();

                }
            });


        }
    };
});

app.directive('ngCalender', function () {
    return {
        restrict: 'A',
        scope: {
            ngCalender: '=',
            ngGetEventList: '&',
            ngOnDrop: '&',
            ngOnEventChange: '&',
            ngOnEventRender: '&',
            ngOnDayRender: '&',
            ngOnEventOrder: '&',
            ngAfterAllEventRender: '&',
            ngStartdate: '=',
            ngHiddendays: '='

        },
        link: function (scope, element, attrs) {
            //Header Setting
            var headerSetting = {
                left: '',
                center: '',
                right: ''
            };
            //Event Hendaling Functions
            var getEventList = function (start, end, timezone, callback) {
                scope.ngGetEventList()(scope.ngCalender, start, end, callback);
            };
            var onDrop = function (date, event, ui, resourceId) { // this function is called when something is dropped
                // retrieve the dropped element's stored Event Object
                var dropperData = $(this).data('eventObject');
                scope.ngOnDrop()(scope.ngCalender, dropperData, date, scope.ngCalender.reloadEvents);
            };

            var onEventResize = function (event, delta, revertFunc) {
                scope.ngOnEventChange()(scope.ngCalender, event, delta, scope.ngCalender.reloadEvents, revertFunc);
            };

            var onEventDrop = function (event, delta, revertFunc) {
                scope.ngOnEventChange()(scope.ngCalender, event, delta, scope.ngCalender.reloadEvents, revertFunc);
            };



            //Function That will be called in Controllers when required.
            scope.ngCalender.reloadEvents = function () {
                $(element).fullCalendar('refetchEvents');
                $('.fc-highlight-skeleton').remove();

            };
            scope.ngCalender.addEvent = function (eventObj) {
                $(element).fullCalendar('renderEvent', eventObj, true);

            };

            var defaultDate = new Date();

            if (scope.ngStartdate == undefined) {
                defaultDate = moment(defaultDate).add(6, 'day');
            } else {
                defaultDate = moment(scope.ngStartdate);
            }


            if (scope.ngHiddendays == undefined) {
                scope.ngHiddendays = [];
            }

            //if (moment(defaultDate).day() > 5) {

            //}

            $(element).fullCalendar({ //re-initialize the calendar
                header: false,
                defaultView: 'basicWeek', // change default view with available options from http://arshaw.com/fullcalendar/docs/views/Available_Views/ 
                slotMinutes: 15,
                firstDay: moment(defaultDate).day(),
                hiddenDays: scope.ngHiddendays,
                editable: true,
                draggable: false,
                droppable: true, // this allows things to be dropped onto the calendar !!!                
                dragRevertDuration: 0,
                drop: onDrop,
                defaultDate: defaultDate,
                height: 'auto',
                events: getEventList,
                eventDrop: onEventDrop,
                eventResize: onEventResize,
                eventOrder: function (a, b) {
                    if (scope.ngOnEventOrder()) {
                        return scope.ngOnEventOrder()(a, b, scope.ngCalender);
                    }
                    return 1;
                },
                eventRender: function (event, ele) {
                    if (scope.ngOnEventRender()) {
                        scope.ngOnEventRender()(scope.ngCalender, event, ele);
                    }
                },
                dayRender: function (date, cell) {
                    if (scope.ngOnEventRender()) {
                        scope.ngOnDayRender()(scope.ngCalender, date, cell);
                    }
                },
                eventAfterAllRender: function (view) {
                    if (scope.ngAfterAllEventRender()) {
                        scope.ngAfterAllEventRender()(scope.ngCalender, view);
                    }
                },

                eventDragStop: function (event, jsEvent, ui, view) {
                    if (event.popOverObj) {
                        event.popOverObj.Hide();
                    }
                }

            });


            scope.ngCalender.element = element;
        }
    };
});

app.directive('ngAttendanceCalender', function () {
    return {
        restrict: 'A',
        scope: {
            ngAttendanceCalender: '=',
            ngGetEventList: '&',
            ngOnEventRender: '&',
            ngOnClick: '&',
            ngHeight: '@',
            ngDragscroll: '@',
            ngDroppable: '@',
            ngDefaultview: '@',
            ngDisabledragging: '@'
        },
        link: function (scope, element, attrs) {
            //Header Setting
            var headerSetting = {
                left: '',
                center: '',
                right: 'title'
            };
            //Event Hendaling Functions
            var getEventList = function (start, end, timezone, callback) {
                scope.ngGetEventList()(scope.ngAttendanceCalender, start, end, callback);
            };

            //Function That will be called in Controllers when required.
            scope.ngAttendanceCalender.reloadEvents = function () {
                $(element).fullCalendar('refetchEvents');

            };
            scope.ngAttendanceCalender.addEvent = function (eventObj) {
                $(element).fullCalendar('renderEvent', eventObj, true);

            };


            var defaultDate = new Date();
            //if (moment(defaultDate).day() > 5) {
            defaultDate = moment(defaultDate).add(6, 'day');
            //}


            scope.ngAttendanceCalender.updateEvent = function (eventObj) {
                $(element).fullCalendar('updateEvent', eventObj);

            };


            if (scope.ngDroppable == null || scope.ngDroppable == undefined) {
                scope.ngDroppable = true;
            }

            if (scope.ngDisabledragging == null || scope.ngDisabledragging == undefined) {
                scope.ngDisabledragging = false;
            }


            if (scope.ngDragscroll == null || scope.ngDragscroll == undefined) {
                scope.ngDragscroll = false;
            }
            setTimeout(function () {
                $(element).fullCalendar({ //re-initialize the calendar
                    header: headerSetting,
                    defaultView: scope.ngDefaultview ? scope.ngDefaultview : 'month', // change default view with available options from http://arshaw.com/fullcalendar/docs/views/Available_Views/ 
                    slotMinutes: 15,
                    firstDay: 5,
                    defaultDate: defaultDate,
                    disableDragging: Boolean(scope.ngDisabledragging),
                    dragScroll: Boolean(scope.ngDragscroll),
                    droppable: Boolean(scope.ngDroppable), // this allows things to be dropped onto the calendar !!!
                    height: 'auto',//scope.ngHeight ? parseInt(scope.ngHeight) : "",
                    events: getEventList,
                    eventRender: function (event, ele) {
                        if (scope.ngOnEventRender()) {
                            scope.ngOnEventRender()(scope.ngAttendanceCalender, event, ele);
                        }
                    },
                    eventClick: function (calEvent, jsEvent, view) {
                        if (scope.ngOnClick()) {
                            scope.ngOnClick()(scope.ngAttendanceCalender, calEvent, jsEvent, view);
                        }
                    }
                });


                scope.ngAttendanceCalender.element = element;
            });
        }
    };
});

app.directive('droppable', function () {
    return {
        restrict: 'A',
        scope: {
            droppable: '=',
            onDrop: '&',
            ngAccept: '@'
        },
        link: function (scope, element, attrs) {
            // store the Event Object in the DOM element so we can get to it later

            if (!scope.ngAccept) {
                scope.ngAccept = '[calenderDropper]';
            }

            // make the event draggable using jQuery UI
            element.droppable({
                accept: scope.ngAccept,
                drop: function (event, ui) {
                    var objectData = $(ui.draggable).data('eventObject');
                    scope.onDrop()(scope.droppable, objectData);
                }
            });
        }
    };
});

//*For Calender
app.directive('dirValidNumber', function () {
    return {
        require: '?ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            if (!ngModelCtrl) {
                return;
            }

            ngModelCtrl.$parsers.push(function (val) {
                var clean = val.replace(/[^0-9]+/g, '');
                if (val !== clean) {
                    ngModelCtrl.$setViewValue(clean);
                    ngModelCtrl.$render();
                }
                return clean;
            });

            element.bind('keyup', function (event) {
                if (event.keyCode === 32) {
                    event.preventDefault();
                }
            });
        }
    };
});


app.directive('dirValidDecimal', function () {
    return {
        require: '?ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            if (!ngModelCtrl) {
                return;
            }



            ngModelCtrl.$parsers.push(function (val) {

                var clean = val.replace(/[^0-9\.]/g, '');
                if (val !== clean) {
                    ngModelCtrl.$setViewValue(clean);
                    ngModelCtrl.$render();
                }


                //CHECK FOR VALID DECIMAL POINTS
                if (attrs.maxDecimals == undefined)
                    attrs.maxDecimals = 2;

                var regexString, validRegex;
                if (attrs.maxDecimals > 0) {
                    regexString = "^-?\\d*\\.?\\d{0," + attrs.maxDecimals + "}$";
                } else {
                    regexString = "^-?\\d*$";
                }
                validRegex = new RegExp(regexString);
                if (!validRegex.test(val)) {
                    clean = parseFloat(clean).toFixed(2);
                    ngModelCtrl.$setViewValue(clean);
                    ngModelCtrl.$render();
                }

                return clean;
            });

            element.bind('keypress', function (event) {
                if (event.keyCode === 32) {
                    event.preventDefault();
                }
            });
        }
    };
});




//#region Vertical Resource

app.directive('ngScalender', function () {
    return {
        restrict: 'A',
        scope: {
            ngScalender: '=',
            ngGetEventList: '&',
            ngOnDrop: '&',
            ngOnEventChange: '&',
            ngOnEventRender: '&',
            ngOnDayRender: '&',
            ngOnResourceRender: '&',
            ngOnEventOrder: '&',
            ngAfterAllEventRender: '&',
            ngStartdate: '=',
            ngWeekstartday: '=',
            ngHiddendays: '=',
            ngResourcelist: '&',
            ngResourcename: '&',
            ngShowloader: '&',
            ngRefresh: '&',
            ngEditable: '=',
            ngDraggable: '=',
            ngDroppable: '=',
            ngDefaultview: '=',
            ngAspectratio: '='


        },
        link: function (scope, element, attrs) {
            scope.ngCalender = scope.ngScalender;
            //Header Setting
            var headerSetting = {
                left: '',
                center: '',
                right: ''
            };


            //$(element).fullCalendar('refetchResources');



            //Event Hendaling Functions


            scope.ngCalender.RefreshCalender = function () {
                if (scope.ngRefresh()) {
                    scope.ngRefresh()(scope.ngCalender);
                }
            };

            scope.ngCalender.refetchResources = function () {
                $(element).fullCalendar('refetchResources');
            };

            scope.ngCalender.CalendarElement = function () {
                return $(element);
            };

            scope.ngCalender.GetView = function () {
                return $(element).fullCalendar('getView');
            };

            scope.ngCalender.StartDate = function () {
                return $(element).fullCalendar('getView').start;
            };

            scope.ngCalender.EndDate = function () {
                var endDate = $(element).fullCalendar('getView').end;
                return moment(endDate).add(-1, 'day');
            };


            var showLoaderCallBack = function (isLoading, view) {
                if (scope.ngShowloader()) {
                    scope.ngShowloader()(isLoading, view);
                }
            };


            scope.ngCalender.getResourcelistFromMemory = function (callback) {
                return $(element).fullCalendar('getResources');
            };


            var getResourceName = function () {

                if (scope.ngResourcename())
                    return scope.ngResourcename()();
                else {
                    return [
                        {
                            labelText: 'Employees', //'Employees',
                            field: 'EmployeeName',
                            width: '150px'
                        },
                        {
                            labelText: 'Hrs',
                            field: 'EmployeeRemainingHours',
                            width: '50px'

                        }
                    ];
                }
            };


            var getResourcelist = function (callback) {
                scope.ngResourcelist()(scope.ngCalender, callback);
            };

            var getEventList = function (start, end, timezone, callback) {
                scope.ngGetEventList()(scope.ngCalender, start, end, callback);
            };
            var onDrop = function (date, event, ui, resourceId) { // this function is called when something is dropped
                // retrieve the dropped element's stored Event Object
                var resourceData = $(element).fullCalendar("getResourceById", resourceId);
                var dropperData = $(this).data('eventObject');
                scope.ngOnDrop()(scope.ngCalender, dropperData, date, scope.ngCalender.reloadEvents, resourceData);
            };

            var onEventResize = function (event, delta, revertFunc) {

                scope.ngOnEventChange()(scope.ngCalender, event, delta, scope.ngCalender.reloadEvents, revertFunc);
            };

            var onEventDrop = function (event, delta, revertFunc) {
                scope.ngOnEventChange()(scope.ngCalender, event, delta, scope.ngCalender.reloadEvents, revertFunc);
            };

            //Function That will be called in Controllers when required.
            scope.ngCalender.reloadEvents = function () {
                //$(element).fullCalendar('refetchResources');
                $(element).fullCalendar('refetchEvents');

                //$('.fc-highlight-skeleton').remove();
            };

            scope.ngCalender.addEvent = function (eventObj) {
                $(element).fullCalendar('renderEvent', eventObj, true);

            };

            var defaultDate = new Date();

            if (scope.ngStartdate == undefined) {
                defaultDate = moment(defaultDate).add(6, 'day');
            } else {
                defaultDate = moment(scope.ngStartdate);
            }


            if (scope.ngHiddendays == undefined) {
                scope.ngHiddendays = [];
            }


            $(element).fullCalendar({
                //re-initialize the calendar
                schedulerLicenseKey: 'CC-Attribution-NonCommercial-NoDerivatives',
                editable: scope.ngEditable == undefined ? true : scope.ngEditable,
                draggable: scope.ngDraggable == undefined ? true : scope.ngDraggable,
                droppable: scope.ngDroppable == undefined ? true : scope.ngDroppable, // this allows things to be dropped onto the calendar !!! 
                //aspectRatio: scope.ngAspectratio == undefined ? 1.55 : scope.ngAspectratio,
                contentHeight: 580,
                handleWindowResize: true,
                showNonCurrentDates: false,
                eventOverlap: true,
                //lazyFetching: true,
                loading: showLoaderCallBack,
                refetchResourcesOnNavigate: true,

                //agendaEventMinHeight:50,
                //aspectRatio: 1.2,
                //refetchResourcesOnNavigate:true,
                // minTime: "10:00:00",
                // maxTime: "20:00:00",
                //businessHours: {
                //    start: '10:00', // a start time (10am in this example)
                //    end: '20:00',//, // an end time (8pm in this example)
                //    dow: [0,1, 2, 3, 4, 5, 6]
                //},
                // scrollTime: '00:00', // undo default 6am scrollTime
                //header: false,
                //theme:true,
                customButtons: {
                    DayBtn: {
                        text: 'Day',
                        click: function () {
                            //$(element).fullCalendar('changeView', "timelineDay");
                            $(element).fullCalendar('changeView', "timelineOneDays");
                            showLoaderCallBack(true);

                        }
                    },
                    T3dayBtn: {
                        text: '3 Days',
                        click: function () {
                            $(element).fullCalendar('changeView', "timelineThreeDays");
                            showLoaderCallBack(true);
                        }
                    },
                    WeekBtn: {
                        text: 'Week',
                        click: function () {
                            //$(element).fullCalendar('changeView', "agendaWeek");
                            $(element).fullCalendar('changeView', "timelineWeekDays");
                            showLoaderCallBack(true);
                        }
                    },
                    MonthBtn: {
                        text: 'Month',
                        click: function () {
                            $(element).fullCalendar('changeView', "month");
                            showLoaderCallBack(true);
                        }
                    },
                    TodayBtn: {
                        text: 'Today',
                        click: function () {
                            $(element).fullCalendar('today');
                            showLoaderCallBack(true);
                        }
                    },
                    PrevBtn: {
                        icon: 'left-single-arrow',
                        click: function () {
                            $(element).fullCalendar('prev');
                            showLoaderCallBack(true);
                            if (scope.ngRefresh()) {
                                //scope.ngRefresh()(scope.ngCalender);
                            }


                        }
                    },
                    NextBtn: {
                        icon: 'right-single-arrow',
                        click: function () {
                            $(element).fullCalendar('next');
                            showLoaderCallBack(true);
                            if (scope.ngRefresh()) {
                                //scope.ngRefresh()(scope.ngCalender);
                            }

                        }
                    },
                    RefreshBtn: {
                        text: 'Refresh',
                        //icon: "glyphicon glyphicon-refresh",
                        click: function () {

                            if (scope.ngRefresh()) {
                                showLoaderCallBack(true);
                                scope.ngRefresh()(scope.ngCalender);
                            }
                            $(element).fullCalendar('refetchResources');
                            //$(element).fullCalendar('today');
                        }
                    }


                },
                header: {
                    left: 'DayBtn,WeekBtn,MonthBtn', //'  month,timelineDay,timelineThreeDays,timelineTenDays,timelineMonth,timelineYear,agendaWeek',
                    center: 'title',
                    right: 'RefreshBtn TodayBtn PrevBtn,NextBtn'
                },
                views: {
                    timelineWeekDays: {
                        type: 'timeline',
                        duration: { days: 7 }
                    },
                    timelineOneDays: {
                        type: 'timeline',
                        duration: { days: 1 },
                        slotDuration: '01:00:00'
                    },

                    month: {
                        editable: false,
                        //droppable: false
                    }

                },
                defaultView: scope.ngDefaultview == undefined ? 'timelineWeek' : scope.ngDefaultview, // change default view with available options from http://arshaw.com/fullcalendar/docs/views/Available_Views/ 
                slotDuration: '24:00:00',
                slotEventOverlap: true,


                firstDay: ValideElement(scope.ngWeekstartday) ? parseInt(scope.ngWeekstartday) : 1,
                hiddenDays: scope.ngHiddendays,
                dragRevertDuration: 0,
                defaultDate: defaultDate,
                allDaySlot: false,

                //eventOverlap:false, 


                resourceColumns: getResourceName(),
                //[
                //    {
                //        labelText: 'Employees',//'Employees',
                //        field: 'EmployeeName',
                //        width: '150px'
                //    },
                //    {
                //        labelText: 'Hrs',
                //        field: 'EmployeeRemainingHours',
                //        width: '50px'

                //    }
                //],
                //resources: custModel.NewInstance().EmployeeList,
                resources: getResourcelist,
                resourceRender: function (resourceObj, labelTds, bodyTds) {
                    if (scope.ngOnResourceRender()) {
                        scope.ngOnResourceRender()(scope.ngCalender, resourceObj, labelTds, bodyTds);
                    }


                    //$(labelTds[0]).addClass('Resource_Employee');

                    //$(labelTds[1]).addClass('Resource_Employee_Hrs');
                    //$(labelTds[1]).addClass('cursor-pointer');


                    //var elem = $(labelTds[1]);
                    //var html = "<span class='hrsprg' style='display:none;'><sapn>";
                    //$(elem).find(".fc-cell-text").after(html);
                    //var selectedItem = $(elem).find(".hrsprg");
                    //$(selectedItem).text(resourceObj.EmployeeHours);


                    //$(labelTds[1]).on('click', function () {
                    //    var action = prompt("Reset hours for -" + resourceObj.EmployeeName, resourceObj.EmployeeHours);


                    //    if (action != null && action !== "") {
                    //        custModel.EmployeeList.filter(function (item) {
                    //            if (item.EmployeeID == resourceObj.id) {
                    //                item.EmployeeHours = parseFloat(action);
                    //            }
                    //        });

                    //        var x = 0;
                    //        var intervalId = setInterval(function () {
                    //            custModel.GenerateStaffCalenders();
                    //            if (++x === 2) {
                    //                window.clearInterval(intervalId);
                    //            }
                    //        }, 600);

                    //    }
                    //});


                },

                eventColor: '#378006',
                drop: onDrop,
                eventConstraint: {

                    start: moment().format('YYYY-MM-DD'),
                    end: '2100-01-01' // hard coded goodness unfortunately
                },
                events: getEventList,
                eventDrop: onEventDrop,
                eventResize: onEventResize,
                eventOrder: function (a, b) {
                    if (scope.ngOnEventOrder()) {
                        return scope.ngOnEventOrder()(a, b, scope.ngCalender);
                    }
                    return 1;
                },
                eventRender: function (event, ele) {
                    if (scope.ngOnEventRender()) {
                        scope.ngOnEventRender()(scope.ngCalender, event, ele);
                    }
                },
                dayRender: function (date, cell) {

                    //dayRender: function (date, cell) {
                    //cell.css("background-color", "red");
                    //cell.text("Not Schedule");

                    if (scope.ngOnDayRender()) {
                        scope.ngOnDayRender()(scope.ngCalender, date, cell);
                    }
                },
                eventAfterAllRender: function (view) {
                    if (scope.ngAfterAllEventRender()) {
                        scope.ngAfterAllEventRender()(scope.ngCalender, view);
                    }

                    showLoaderCallBack(false);
                },
                eventDragStop: function (event, jsEvent, ui, view) {
                    if (event.popOverObj) {
                        event.popOverObj.Hide();
                    }
                }

            });


            scope.ngCalender.element = element;
        }
    };
});


app.directive('ngDaycarecalender', function () {
    return {
        restrict: 'A',
        scope: {
            ngDaycarecalender: '=',
            ngGetEventList: '&',
            ngOnDrop: '&',
            ngOnEventChange: '&',
            ngOnEventRender: '&',
            ngOnDayRender: '&',
            ngOnResourceRender: '&',
            ngOnEventOrder: '&',
            ngAfterAllEventRender: '&',
            ngStartdate: '=',
            ngHiddendays: '=',
            ngResourcelist: '&',
            ngResourcename: '&',
            ngShowloader: '&',
            ngRefresh: '&',
            ngEditable: '=',
            ngDraggable: '=',
            ngDroppable: '=',
            ngDefaultview: '=',
            ngAspectratio: '='


        },
        link: function (scope, element, attrs) {
            scope.ngCalender = scope.ngDaycarecalender;
            //Header Setting
            var headerSetting = {
                left: '',
                center: '',
                right: ''
            };


            //$(element).fullCalendar('refetchResources');



            //Event Hendaling Functions


            scope.ngCalender.RefreshCalender = function () {
                if (scope.ngRefresh()) {
                    scope.ngRefresh()(scope.ngCalender);
                }
            };

            scope.ngCalender.refetchResources = function () {
                $(element).fullCalendar('refetchResources');
            };

            scope.ngCalender.CalendarElement = function () {
                return $(element);
            };

            scope.ngCalender.GetView = function () {
                return $(element).fullCalendar('getView');
            };

            scope.ngCalender.StartDate = function () {
                return $(element).fullCalendar('getView').start;
            };

            scope.ngCalender.EndDate = function () {
                var endDate = $(element).fullCalendar('getView').end;
                return moment(endDate).add(-1, 'day');
            };


            var showLoaderCallBack = function (isLoading, view) {
                if (scope.ngShowloader()) {
                    scope.ngShowloader()(isLoading, view);
                }
            };


            scope.ngCalender.getResourcelistFromMemory = function (callback) {
                return $(element).fullCalendar('getResources');
            };


            var getResourceName = function () {

                if (scope.ngResourcename())
                    return scope.ngResourcename()();
                else {
                    return [
                        {
                            labelText: 'Employees', //'Employees',
                            field: 'EmployeeName',
                            width: '150px'
                        },
                        {
                            labelText: 'Hrs',
                            field: 'EmployeeRemainingHours',
                            width: '50px'

                        }
                    ];
                }
            };


            var getResourcelist = function (callback) {
                scope.ngResourcelist()(scope.ngCalender, callback);
            };

            var getEventList = function (start, end, timezone, callback) {
                scope.ngGetEventList()(scope.ngCalender, start, end, callback);
            };
            var onDrop = function (date, event, ui, resourceId) { // this function is called when something is dropped
                // retrieve the dropped element's stored Event Object
                var resourceData = $(element).fullCalendar("getResourceById", resourceId);
                var dropperData = $(this).data('eventObject');
                scope.ngOnDrop()(scope.ngCalender, dropperData, date, scope.ngCalender.reloadEvents, resourceData);
            };

            var onEventResize = function (event, delta, revertFunc) {

                scope.ngOnEventChange()(scope.ngCalender, event, delta, scope.ngCalender.reloadEvents, revertFunc);
            };

            var onEventDrop = function (event, delta, revertFunc) {
                scope.ngOnEventChange()(scope.ngCalender, event, delta, scope.ngCalender.reloadEvents, revertFunc);
            };

            //Function That will be called in Controllers when required.
            scope.ngCalender.reloadEvents = function () {
                //$(element).fullCalendar('refetchResources');
                $(element).fullCalendar('refetchEvents');

                //$('.fc-highlight-skeleton').remove();
            };

            scope.ngCalender.addEvent = function (eventObj) {
                $(element).fullCalendar('renderEvent', eventObj, true);

            };

            var defaultDate = new Date();

            if (scope.ngStartdate == undefined) {
                defaultDate = moment(defaultDate).add(6, 'day');
            } else {
                defaultDate = moment(scope.ngStartdate);
            }


            if (scope.ngHiddendays == undefined) {
                scope.ngHiddendays = [];
            }


            $(element).fullCalendar({
                //re-initialize the calendar
                schedulerLicenseKey: 'CC-Attribution-NonCommercial-NoDerivatives',
                editable: scope.ngEditable == undefined ? true : scope.ngEditable,
                draggable: scope.ngDraggable == undefined ? true : scope.ngDraggable,
                droppable: scope.ngDroppable == undefined ? true : scope.ngDroppable, // this allows things to be dropped onto the calendar !!! 
                //aspectRatio: scope.ngAspectratio == undefined ? 1.55 : scope.ngAspectratio,
                contentHeight: 580,
                handleWindowResize: true,
                showNonCurrentDates: false,
                eventOverlap: true,
                //lazyFetching: true,
                loading: showLoaderCallBack,
                refetchResourcesOnNavigate: true,

                customButtons: {
                    DayBtn: {
                        text: 'Day',
                        click: function () {
                            //$(element).fullCalendar('changeView', "timelineDay");
                            $(element).fullCalendar('changeView', "timelineOneDays");
                            showLoaderCallBack(true);

                        }
                    },
                    T3dayBtn: {
                        text: '3 Days',
                        click: function () {
                            $(element).fullCalendar('changeView', "timelineThreeDays");
                            showLoaderCallBack(true);
                        }
                    },
                    WeekBtn: {
                        text: 'Week',
                        click: function () {
                            //$(element).fullCalendar('changeView', "agendaWeek");
                            $(element).fullCalendar('changeView', "timelineWeekDays");
                            showLoaderCallBack(true);
                        }
                    },
                    MonthBtn: {
                        text: 'Month',
                        click: function () {
                            $(element).fullCalendar('changeView', "month");
                            showLoaderCallBack(true);
                        }
                    },
                    TodayBtn: {
                        text: 'Today',
                        click: function () {
                            $(element).fullCalendar('today');
                            showLoaderCallBack(true);
                        }
                    },
                    PrevBtn: {
                        icon: 'left-single-arrow',
                        click: function () {
                            $(element).fullCalendar('prev');
                            showLoaderCallBack(true);
                            if (scope.ngRefresh()) {
                                //scope.ngRefresh()(scope.ngCalender);
                            }


                        }
                    },
                    NextBtn: {
                        icon: 'right-single-arrow',
                        click: function () {
                            $(element).fullCalendar('next');
                            showLoaderCallBack(true);
                            if (scope.ngRefresh()) {
                                //scope.ngRefresh()(scope.ngCalender);
                            }

                        }
                    },
                    RefreshBtn: {
                        text: 'Refresh',
                        //icon: "glyphicon glyphicon-refresh",
                        click: function () {

                            if (scope.ngRefresh()) {
                                showLoaderCallBack(true);
                                scope.ngRefresh()(scope.ngCalender);
                            }
                            //$(element).fullCalendar('today');
                        }
                    }


                },
                header: {
                    left: 'DayBtn,WeekBtn,MonthBtn', //'  month,timelineDay,timelineThreeDays,timelineTenDays,timelineMonth,timelineYear,agendaWeek',
                    center: 'title',
                    right: 'RefreshBtn TodayBtn PrevBtn,NextBtn'
                },
                views: {
                    timelineWeekDays: {
                        type: 'timeline',
                        duration: { days: 7 }
                    },
                    timelineOneDays: {
                        type: 'timeline',
                        duration: { days: 1 },
                        slotDuration: '01:00:00'
                    },

                    month: {
                        editable: false,
                        //droppable: false
                    }

                },
                defaultView: scope.ngDefaultview == undefined ? 'timelineWeek' : scope.ngDefaultview, // change default view with available options from http://arshaw.com/fullcalendar/docs/views/Available_Views/ 
                slotDuration: '24:00:00',
                slotEventOverlap: true,


                firstDay: moment(defaultDate).day(),
                hiddenDays: scope.ngHiddendays,
                dragRevertDuration: 0,
                defaultDate: defaultDate,
                allDaySlot: false,

                //eventOverlap:false, 


                resourceColumns: getResourceName(),
                resources: getResourcelist,
                resourceRender: function (resourceObj, labelTds, bodyTds) {
                    if (scope.ngOnResourceRender()) {
                        scope.ngOnResourceRender()(scope.ngCalender, resourceObj, labelTds, bodyTds);
                    }
                },

                eventColor: '#378006',
                drop: onDrop,
                eventConstraint: {

                    start: moment().format('YYYY-MM-DD'),
                    end: '2100-01-01' // hard coded goodness unfortunately
                },
                events: getEventList,
                eventDrop: onEventDrop,
                eventResize: onEventResize,
                eventOrder: function (a, b) {
                    if (scope.ngOnEventOrder()) {
                        return scope.ngOnEventOrder()(a, b, scope.ngCalender);
                    }
                    return 1;
                },
                eventRender: function (event, ele) {
                    if (scope.ngOnEventRender()) {
                        scope.ngOnEventRender()(scope.ngCalender, event, ele);
                    }
                },
                dayRender: function (date, cell) {

                    if (scope.ngOnDayRender()) {
                        scope.ngOnDayRender()(scope.ngCalender, date, cell);
                    }
                },
                eventAfterAllRender: function (view) {
                    if (scope.ngAfterAllEventRender()) {
                        scope.ngAfterAllEventRender()(scope.ngCalender, view);
                    }

                    showLoaderCallBack(false);
                },
                eventDragStop: function (event, jsEvent, ui, view) {
                    if (event.popOverObj) {
                        event.popOverObj.Hide();
                    }
                }

            });


            scope.ngCalender.element = element;
        }
    };
});


app.directive('ngCasemanagementcalender', function () {
    return {
        restrict: 'A',
        scope: {
            ngCasemanagementcalender: '=',
            ngGetEventList: '&',
            ngOnDrop: '&',
            ngOnEventChange: '&',
            ngOnEventRender: '&',
            ngOnDayRender: '&',
            ngOnResourceRender: '&',
            ngOnEventOrder: '&',
            ngAfterAllEventRender: '&',
            ngStartdate: '=',
            ngHiddendays: '=',
            ngResourcelist: '&',
            ngResourcename: '&',
            ngShowloader: '&',
            ngRefresh: '&',
            ngEditable: '=',
            ngDraggable: '=',
            ngDroppable: '=',
            ngDefaultview: '=',
            ngAspectratio: '='


        },
        link: function (scope, element, attrs) {
            scope.ngCalender = scope.ngCasemanagementcalender;
            //Header Setting
            var headerSetting = {
                left: '',
                center: '',
                right: ''
            };


            //$(element).fullCalendar('refetchResources');



            //Event Hendaling Functions


            scope.ngCalender.RefreshCalender = function () {
                if (scope.ngRefresh()) {
                    scope.ngRefresh()(scope.ngCalender);
                }
            };

            scope.ngCalender.refetchResources = function () {
                $(element).fullCalendar('refetchResources');
            };

            scope.ngCalender.CalendarElement = function () {
                return $(element);
            };

            scope.ngCalender.GetView = function () {
                return $(element).fullCalendar('getView');
            };

            scope.ngCalender.StartDate = function () {
                return $(element).fullCalendar('getView').start;
            };

            scope.ngCalender.EndDate = function () {
                var endDate = $(element).fullCalendar('getView').end;
                return moment(endDate).add(-1, 'day');
            };


            var showLoaderCallBack = function (isLoading, view) {
                if (scope.ngShowloader()) {
                    scope.ngShowloader()(isLoading, view);
                }
            };


            scope.ngCalender.getResourcelistFromMemory = function (callback) {
                return $(element).fullCalendar('getResources');
            };


            var getResourceName = function () {

                if (scope.ngResourcename())
                    return scope.ngResourcename()();
                else {
                    return [
                        {
                            labelText: 'Employees', //'Employees',
                            field: 'EmployeeName',
                            width: '150px'
                        },
                        {
                            labelText: 'Hrs',
                            field: 'EmployeeRemainingHours',
                            width: '50px'

                        }
                    ];
                }
            };


            var getResourcelist = function (callback) {
                scope.ngResourcelist()(scope.ngCalender, callback);
            };

            var getEventList = function (start, end, timezone, callback) {
                scope.ngGetEventList()(scope.ngCalender, start, end, callback);
            };
            var onDrop = function (date, event, ui, resourceId) { // this function is called when something is dropped
                // retrieve the dropped element's stored Event Object
                var resourceData = $(element).fullCalendar("getResourceById", resourceId);
                var dropperData = $(this).data('eventObject');
                scope.ngOnDrop()(scope.ngCalender, dropperData, date, scope.ngCalender.reloadEvents, resourceData);
            };

            var onEventResize = function (event, delta, revertFunc) {

                scope.ngOnEventChange()(scope.ngCalender, event, delta, scope.ngCalender.reloadEvents, revertFunc);
            };

            var onEventDrop = function (event, delta, revertFunc) {
                scope.ngOnEventChange()(scope.ngCalender, event, delta, scope.ngCalender.reloadEvents, revertFunc);
            };

            //Function That will be called in Controllers when required.
            scope.ngCalender.reloadEvents = function () {
                //$(element).fullCalendar('refetchResources');
                $(element).fullCalendar('refetchEvents');

                //$('.fc-highlight-skeleton').remove();
            };

            scope.ngCalender.addEvent = function (eventObj) {
                $(element).fullCalendar('renderEvent', eventObj, true);

            };

            var defaultDate = new Date();

            if (scope.ngStartdate == undefined) {
                defaultDate = moment(defaultDate).add(6, 'day');
            } else {
                defaultDate = moment(scope.ngStartdate);
            }


            if (scope.ngHiddendays == undefined) {
                scope.ngHiddendays = [];
            }


            $(element).fullCalendar({
                //re-initialize the calendar
                schedulerLicenseKey: 'CC-Attribution-NonCommercial-NoDerivatives',
                editable: scope.ngEditable == undefined ? true : scope.ngEditable,
                draggable: scope.ngDraggable == undefined ? true : scope.ngDraggable,
                droppable: scope.ngDroppable == undefined ? true : scope.ngDroppable, // this allows things to be dropped onto the calendar !!! 
                //aspectRatio: scope.ngAspectratio == undefined ? 1.55 : scope.ngAspectratio,
                contentHeight: 580,
                handleWindowResize: true,
                showNonCurrentDates: false,
                eventOverlap: true,
                //lazyFetching: true,
                loading: showLoaderCallBack,
                refetchResourcesOnNavigate: true,

                customButtons: {
                    DayBtn: {
                        text: 'Day',
                        click: function () {
                            //$(element).fullCalendar('changeView', "timelineDay");
                            $(element).fullCalendar('changeView', "timelineOneDays");
                            showLoaderCallBack(true);

                        }
                    },
                    T3dayBtn: {
                        text: '3 Days',
                        click: function () {
                            $(element).fullCalendar('changeView', "timelineThreeDays");
                            showLoaderCallBack(true);
                        }
                    },
                    WeekBtn: {
                        text: 'Week',
                        click: function () {
                            //$(element).fullCalendar('changeView', "agendaWeek");
                            $(element).fullCalendar('changeView', "timelineWeekDays");
                            showLoaderCallBack(true);
                        }
                    },
                    MonthBtn: {
                        text: 'Month',
                        click: function () {
                            $(element).fullCalendar('changeView', "month");
                            showLoaderCallBack(true);
                        }
                    },
                    TodayBtn: {
                        text: 'Today',
                        click: function () {
                            $(element).fullCalendar('today');
                            showLoaderCallBack(true);
                        }
                    },
                    PrevBtn: {
                        icon: 'left-single-arrow',
                        click: function () {
                            $(element).fullCalendar('prev');
                            showLoaderCallBack(true);
                            if (scope.ngRefresh()) {
                                //scope.ngRefresh()(scope.ngCalender);
                            }


                        }
                    },
                    NextBtn: {
                        icon: 'right-single-arrow',
                        click: function () {
                            $(element).fullCalendar('next');
                            showLoaderCallBack(true);
                            if (scope.ngRefresh()) {
                                //scope.ngRefresh()(scope.ngCalender);
                            }

                        }
                    },
                    RefreshBtn: {
                        text: 'Refresh',
                        //icon: "glyphicon glyphicon-refresh",
                        click: function () {

                            if (scope.ngRefresh()) {
                                showLoaderCallBack(true);
                                scope.ngRefresh()(scope.ngCalender);
                            }
                            //$(element).fullCalendar('today');
                        }
                    }


                },
                header: {
                    left: 'DayBtn,WeekBtn,MonthBtn', //'  month,timelineDay,timelineThreeDays,timelineTenDays,timelineMonth,timelineYear,agendaWeek',
                    center: 'title',
                    right: 'RefreshBtn TodayBtn PrevBtn,NextBtn'
                },
                views: {
                    timelineWeekDays: {
                        type: 'timeline',
                        duration: { days: 7 }
                    },
                    timelineOneDays: {
                        type: 'timeline',
                        duration: { days: 1 },
                        slotDuration: '01:00:00'
                    },

                    month: {
                        editable: false,
                        //droppable: false
                    }

                },
                defaultView: scope.ngDefaultview == undefined ? 'timelineWeek' : scope.ngDefaultview, // change default view with available options from http://arshaw.com/fullcalendar/docs/views/Available_Views/ 
                slotDuration: '24:00:00',
                slotEventOverlap: true,


                firstDay: moment(defaultDate).day(),
                hiddenDays: scope.ngHiddendays,
                dragRevertDuration: 0,
                defaultDate: defaultDate,
                allDaySlot: false,

                //eventOverlap:false, 


                resourceColumns: getResourceName(),
                resources: getResourcelist,
                resourceRender: function (resourceObj, labelTds, bodyTds) {
                    if (scope.ngOnResourceRender()) {
                        scope.ngOnResourceRender()(scope.ngCalender, resourceObj, labelTds, bodyTds);
                    }
                },

                eventColor: '#378006',
                drop: onDrop,
                eventConstraint: {

                    start: moment().format('YYYY-MM-DD'),
                    end: '2100-01-01' // hard coded goodness unfortunately
                },
                events: getEventList,
                eventDrop: onEventDrop,
                eventResize: onEventResize,
                eventOrder: function (a, b) {
                    if (scope.ngOnEventOrder()) {
                        return scope.ngOnEventOrder()(a, b, scope.ngCalender);
                    }
                    return 1;
                },
                eventRender: function (event, ele) {
                    if (scope.ngOnEventRender()) {
                        scope.ngOnEventRender()(scope.ngCalender, event, ele);
                    }
                },
                dayRender: function (date, cell) {

                    if (scope.ngOnDayRender()) {
                        scope.ngOnDayRender()(scope.ngCalender, date, cell);
                    }
                },
                eventAfterAllRender: function (view) {
                    if (scope.ngAfterAllEventRender()) {
                        scope.ngAfterAllEventRender()(scope.ngCalender, view);
                    }

                    showLoaderCallBack(false);
                },
                eventDragStop: function (event, jsEvent, ui, view) {
                    if (event.popOverObj) {
                        event.popOverObj.Hide();
                    }
                }

            });


            scope.ngCalender.element = element;
        }
    };
});

//#endregion




//#endregion




//#endregion

//#region Graph Drrective
app.directive('jqChart', ['$compile', '$timeout', function ($compile, $timeout) {
    return {
        restrict: 'A',
        scope: {
            ngTitle: "@",
            ngSeries: "=",

        },
        link: function (scope, element, attrs) {


            var background = {
                type: 'linearGradient',
                x0: 0,
                y0: 0,
                x1: 0,
                y1: 1,
                colorStops: [
                    { offset: 0, color: '#d2f7f1' },
                    { offset: 1, color: 'white' }
                ]
            };




            scope.$watch(function () { return scope.ngSeries; }, function () {

                $(element).jqChart({
                    title: { text: scope.ngTitle },
                    border: { strokeStyle: '#6ba851' },
                    background: background,
                    animation: { duration: 1 },
                    shadows: {
                        enabled: true
                    },
                    axes: [
                        {
                            type: 'linear',
                            location: 'bottom',
                            minimum: 0,
                            maximum: 5,
                            zoomEnabled: false

                        }
                    ],
                    series: scope.ngSeries,
                    toolbar: {
                        visibility: 'auto', // auto, visible, hidden
                        resetZoomTooltipText: 'Reset Zoom (100%)',
                        zoomingTooltipText: 'Zoom in to selection area',
                        panningTooltipText: 'Pan the chart'
                    },
                    mouseInteractionMode: 'zooming', // zooming, panning

                });


            });

        }
    };
}]);

//#endregion

//#region CheckList Model

app.directive('checklistModel', ['$parse', '$compile', function ($parse, $compile) {
    // contains
    function contains(arr, item, comparator) {
        if (angular.isArray(arr)) {
            for (var i = arr.length; i--;) {
                if (comparator(arr[i], item)) {
                    return true;
                }
            }
        }
        return false;
    }

    // add
    function add(arr, item, comparator) {

        arr = angular.isArray(arr) ? arr : [];
        if (!contains(arr, item, comparator)) {
            arr.push(item);
        }
        return arr;
    }

    // remove
    function remove(arr, item, comparator) {
        if (angular.isArray(arr)) {
            for (var i = arr.length; i--;) {
                if (comparator(arr[i], item)) {
                    arr.splice(i, 1);
                    break;
                }
            }
        }
        return arr;
    }

    // http://stackoverflow.com/a/19228302/1458162
    function postLinkFn(scope, elem, attrs) {
        //console.log(attrs.checklistModel);
        // exclude recursion, but still keep the model
        var checklistModel = attrs.checklistModel;
        attrs.$set("checklistModel", null);
        // compile with `ng-model` pointing to `checked`
        $compile(elem)(scope);
        attrs.$set("checklistModel", checklistModel);

        // getter / setter for original model
        var getter = $parse(checklistModel);
        var setter = getter.assign;
        var checklistChange = $parse(attrs.checklistChange);
        var checklistBeforeChange = $parse(attrs.checklistBeforeChange);

        // value added to list
        var value = attrs.checklistValue ? $parse(attrs.checklistValue)(scope.$parent) : attrs.value;


        var comparator = angular.equals;

        if (attrs.hasOwnProperty('checklistComparator')) {
            if (attrs.checklistComparator[0] == '.') {
                var comparatorExpression = attrs.checklistComparator.substring(1);
                comparator = function (a, b) {
                    return a[comparatorExpression] === b[comparatorExpression];
                };

            } else {
                comparator = $parse(attrs.checklistComparator)(scope.$parent);
            }
        }

        // watch UI checked change
        scope.$watch(attrs.ngModel, function (newValue, oldValue) {

            if (newValue === oldValue) {
                return;
            }
            if (checklistBeforeChange && (checklistBeforeChange(scope) === false)) {
                scope[attrs.ngModel] = contains(getter(scope.$parent), value, comparator);
                return;
            }
            setValueInChecklistModel(value, newValue);

            if (checklistChange) {
                checklistChange(scope);
            }
            if (newValue) {
                if ($(elem).parent("span.musthave"))
                    $(elem).parent("span.musthave").addClass("checked");
            } else {
                if ($(elem).parent("span.musthave"))
                    $(elem).parent("span.musthave").removeClass("checked");
            }

        });

        function setValueInChecklistModel(value, checked) {
            var current = getter(scope.$parent);
            if (angular.isFunction(setter)) {
                if (checked === true) {
                    setter(scope.$parent, add(current, value, comparator));
                } else {
                    setter(scope.$parent, remove(current, value, comparator));
                }
            }

        }

        // declare one function to be used for both $watch functions
        function setChecked(newArr, oldArr) {
            if (checklistBeforeChange && (checklistBeforeChange(scope) === false)) {
                setValueInChecklistModel(value, scope[attrs.ngModel]);
                return;
            }
            scope[attrs.ngModel] = contains(newArr, value, comparator);
        }

        // watch original model change
        // use the faster $watchCollection method if it's available
        if (angular.isFunction(scope.$parent.$watchCollection)) {
            scope.$parent.$watchCollection(checklistModel, setChecked);
        } else {
            scope.$parent.$watch(checklistModel, setChecked, true);
        }
    }

    return {
        restrict: 'A',
        priority: 1000,
        terminal: true,
        scope: true,

        compile: function (tElement, tAttrs) {

            if ((tElement[0].tagName !== 'INPUT' || tAttrs.type !== 'checkbox') && (tElement[0].tagName !== 'MD-CHECKBOX') && (!tAttrs.btnCheckbox)) {
                throw 'checklist-model should be applied to `input[type="checkbox"]` or `md-checkbox`.';
            }

            var el = $(tElement[0]);
            if (!tAttrs.checklistValue && !tAttrs.value) {
                throw 'You should provide `value` or `checklist-value`.';
            }

            // by default ngModel is 'checked', so we set it if not specified
            if (!tAttrs.ngModel) {
                // local scope var storing individual checkbox model
                tAttrs.$set("ngModel", "checked");
            }

            return postLinkFn;
        }
    };
}]);

//#endregion
var DBdateFormat = '';
function GetOrgDateFormat() {
    if (!DBdateFormat) {
        $.ajax({
            url: HomeCareSiteUrl.OrganizationPreferenceDateFormatURL,
            type: "get",
            async: false,
            success: function (response) {
                DBdateFormat = response;
                if (response = '' || response == null) {
                    DBdateFormat = 'DD/MM/YYYY';
                }
            }
        });
    }
    return DBdateFormat;
}

var DBCurrencyFormat = '';
function GetOrgCurrencyFormat() {
    if (!DBCurrencyFormat) {
        $.ajax({
            url: HomeCareSiteUrl.OrganizationPreferenceCurrencyFormatURL,
            type: "get",
            async: false,
            success: function (response) {
                DBCurrencyFormat = response;
                if (response = '' || response == null) {
                    DBCurrencyFormat = '$';
                }
            }
        });
    }
    return DBCurrencyFormat;
}

app.filter('orgdate', function () {
    return function (input) {
        var dtFormat = GetOrgDateFormat();
        return moment(new Date(input)).format(dtFormat);
    }
});

app.filter('orgtime', function () {
    return function (input) {
        return moment(new Date(input)).format('hh:mm a');
    }
});

app.filter('orgdatetime', function () {
    return function (input) {
        var dtFormat = GetOrgDateFormat();
        return moment(new Date(input)).format(dtFormat + ' hh:mm a');
    }
});

app.filter('orgcurrency', function ($filter) {
    return function (input) {
        if (!input || input == 0 || input == 0.00)
            return GetOrgCurrencyFormat() + ' -';
        else
            return $filter('currency')(input, GetOrgCurrencyFormat());

    };
});

