var pfDate = (function(){
    function ensureTwoDigits(number){
        var text = '' + number;
        if(text.length === 1){
            text = '0' + text;
        }

        return text;
    }

    return {
        toTimestampString: function (date) {
            //yyyy-MM-dd hh:mm:ss
            return date.getUTCFullYear() + '-' + ensureTwoDigits(date.getMonth() + 1) + '-' +
                ensureTwoDigits(date.getUTCDate()) + ' ' + ensureTwoDigits(date.getHours()) + ':' +
                ensureTwoDigits(date.getMinutes()) + ':' + ensureTwoDigits(date.getSeconds());
        }
    }
})();

var pfAlert = (function(){
    function parseError(obj, msg, res){
        if(res !== undefined && res !== null){
            if(res.status === -1){
                obj.msg = msg + 'Cannot communicate with server';
                return;
            }

            obj.msg = msg + res.status + ' ' + res.statusText;
            if(res.data !== undefined && res.data !== null){
                var rawJson = JSON.stringify(res.data, null, 2);
                if(rawJson.length > 0){
                    window.rawJson = rawJson;
                    obj.json = rawJson;
                }
            }
        }
        else{
            obj.msg = msg;
        }
    }

    function showModal(postCloseFn){
        var alertModal = $('#alertModal');

        $(alertModal).on('hidden.bs.modal', function(event){
            $(event.currentTarget).unbind(); //This is necessary so that multiple calls to this don't lead to a stack of bound events
            if(postCloseFn){
                postCloseFn();
            }
        });

        $(alertModal).modal({
            backdrop: 'static'
        });
    }

    return {
        title: '',
        msg: '',
        json: '',
        type: '',
        showFail: function(msg, res, postCloseFn){
            this.title = 'Error';
            this.type = 'danger';
            this.msg = '';
            this.json = '';
            parseError(this, msg, res);
            showModal(postCloseFn);
        },
        showSuccess: function(msg, postCloseFn){
            this.title = 'Success';
            this.type = 'success';
            this.msg = msg;
            this.json = '';
            showModal(postCloseFn);
        }
    }
})();

var pfUrl = {
    tenant: '',
    group: '',
    redirectToLogin: false,
    addParamsToUrl: function(url, additionalQueryString) {
        var hasParams = url.includes('?');
        if (this.tenant) {
            url = url + (hasParams ? '&' : '?') + 'tenantGuid=' + this.tenant;
            hasParams = true;
        }

        if (this.group) {
            url = url + (hasParams ? '&' : '?') + 'groupGuid=' + this.group;
            hasParams = true;
        }

        // if (this.redirectToLogin) {
        //     url = url + (hasParams ? '&' : '?') + 'RedirectToLogin=true';
        //     hasParams = true;
        // }

        if (additionalQueryString) {
            url = url + (hasParams ? '&' : '?') + additionalQueryString;
        }

        return url;
    },
    init: function($location) {
        var searchObj = $location.search();
        this.tenant = searchObj.tenantGuid;
        this.group = searchObj.groupGuid;
        this.redirectToLogin = 'true' === searchObj.RedirectToLogin;
    },
    getRedirectToLogin: function() {
        var isHosted = false; //This is replaced by the RegEx Processor when this file is delivered
        if (!isHosted && this.redirectToLogin) {
            return '&RedirectToLogin=true';
        }
        return '';
    }
};

function pfPdf($scope, $http){

    function handlePdfClick(){
        if(!$scope.pdf.current.hasEmail){
            window.open($scope.pdf.current.downloadUri + pfUrl.getRedirectToLogin());
        }
        else{
            $scope.pdf.current.sendTo = '';
            $('#pdfModal').modal({
                backdrop: 'static'
            });
        }
    }

    return {
        current: {
            downloadUri: '',
            emailUri: '',
            hasEmail: false,
            shortName: '',
            longName: '',
            sendTo: ''
        },
        printClick: {
            formListClick: function(form){
                $scope.pdf.current.downloadUri = form.NewPdfURI;
                $scope.pdf.current.hasEmail = form.EmailPdfURI !== undefined;
                $scope.pdf.current.shortName = form.Name;
                $scope.pdf.current.longName = form.FormLongName;
                $scope.pdf.current.emailUri = form.EmailPdfURI;

                handlePdfClick();
            },
            searchResultClick: function(result){
                $scope.pdf.current.downloadUri = result.PrintPdfURI;
                $scope.pdf.current.hasEmail = result.EmailPdfURI !== undefined;
                $scope.pdf.current.shortName = result.BRIGGS_PF_FORM.FORM_SHORT_NAME;
                $scope.pdf.current.longName = $scope.alias_search.getFormLongName(result.BRIGGS_PF_FORM.FORM_SHORT_NAME);
                $scope.pdf.current.emailUri = result.EmailPdfURI;

                handlePdfClick();
            },
            formClick: function(metadata){
                if(metadata.id){
                    $scope.pdf.current.downloadUri = metadata.form.FilledPdfURI;
                    $scope.pdf.current.hasEmail = metadata.form.EmailFilledPdfURI !== undefined;
                    $scope.pdf.current.emailUri = metadata.form.EmailFilledPdfURI;
                }
                else{
                    $scope.pdf.current.downloadUri = metadata.form.NewPdfURI;
                    $scope.pdf.current.hasEmail = metadata.form.EmailBlankPdfURI !== undefined;
                    $scope.pdf.current.emailUri = metadata.form.EmailBlankPdfURI;
                }

                $scope.pdf.current.shortName = metadata.form.Name;
                $scope.pdf.current.longName = metadata.form.FormLongName;

                handlePdfClick();
            }
        },
        printAction: {
            download: function(){
                //This function just closes the modal when downloading, because data-dismiss breaks the download
                // $('#pdfModal').modal('hide');
                window.open($scope.pdf.current.downloadUri + pfUrl.getRedirectToLogin());
            },
            sendEmail: function(){
                if(!$scope.pdf.current.sendTo){
                    $('#pdfModal').modal('hide');
                    pfAlert.showFail('Cannot send email without specifying a Send To email address.');
                }
                else{
                    var sendTo = $scope.pdf.current.sendTo;
                    var body = JSON.stringify({
                        sendTo: sendTo
                    });

                    $scope.blockModal.title = 'Sending PDF Email';
                    $scope.blockModal.body = 'Sending PDF in an email, please wait...';

                    $('#pdfModal').modal('hide');
                    $('#blockModal').modal({
                        backdrop: 'static'
                    });

                    $http.post($scope.pdf.current.emailUri, body, {
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    })
                        .then(function(res){
                                $('#blockModal').modal('hide');
                                console.log('Successfully sent email');
                                pfAlert.showSuccess('Email has been sent to: "' + $scope.pdf.current.sendTo + '". Delivery status is not available.');
                            },
                            function(res){
                                $('#blockModal').modal('hide');
                                console.log('Failed to send email');
                                pfAlert.showFail('Failed to send email.', res);
                            });
                }
            }
        }
    }
}

var inputPatterns = (function(){
    var patternTypes = {
        zip: 'zip code',
        zipLast4: 'zip code last 4',
        date: 'date',
        time: 'time'
    };

    return {
        types: patternTypes,
        patterns: [
            {
                name: patternTypes.zip,
                pattern: '^[0-9]{5}$',
                patternText: 'ddddd'
            },
            {
                name: patternTypes.zipLast4,
                pattern: '^[0-9]{4}$',
                patternText: 'dddd'
            },
            {
                name: patternTypes.date,
                pattern: '^[0-9]{4}-[0-9]{2}-[0-9]{2}$',
                patternText: 'yyyy-MM-dd'
            },
            {
                name: patternTypes.time,
                pattern: '^([0-1]?)[0-9]:[0-9]{2} (AM|PM)$',
                patternText: 'HH:mm AM'
            }
        ],
        findPattern: function(name) {
            return this.patterns.find(function(item){
                return item.name === name;
            });
        },
        createMessages: function(type, formName) {
            return function() {
                var ngMessages = $('<div></div>')
                    .attr('ng-messages', formName + '.' + this.name + '.$error')
                    .attr('ng-if', formName + '.' + this.name + '.$touched');


                var ngMessage = $('<span></span>')
                    .attr('ng-message', 'pattern')
                    .text(type + ' value must be in (' + inputPatterns.findPattern(type).patternText + ') format.');

                $(ngMessages).append(ngMessage);

                return ngMessages;
            }
        }
    }
})();

var pfBriggsHttpInterceptors = (function() {
    var CSRF_HEADER = 'X-CSRF-TOKEN';
    var GET_METHOD = 'GET';

    var csrfToken;

    function csrfInterceptor() {
        return {
            request: function(config) {
                if (GET_METHOD === config.method) {
                    config.headers[CSRF_HEADER] = 'Fetch';
                }
                else if (csrfToken) {
                    config.headers[CSRF_HEADER] = csrfToken;
                }

                return config;
            },
            response: function(response) {
                var headers = response.headers();
                var key = Object.keys(headers).find(function(headerKey) {
                    return headerKey.toUpperCase() === CSRF_HEADER;
                });

                if (key) {
                    csrfToken = headers[key];
                }

                return response;
            }
        }
    }

    function redirectInterceptor($q) {
        return {
            responseError: function(response) {
                var isHosted = false; //The false is replaced by a RegEx Processor when this code is returned from the server
                if (!isHosted && pfUrl.redirectToLogin && (response.status === 401 || response.status === 403)) {
                    window.location.href = '../auth/loginpage';
                }

                return $q.reject(response);
            }
        }
    }

    return {
        csrfInterceptor: csrfInterceptor,
        redirectInterceptor: redirectInterceptor
    }
})();

//returns a function that wraps the argument function in a "debounce",
// meaning if the returned function is called multiple times in rapid succession only the last one will run
function createDebounce(fn, delayMS) {
    var timeout = 0;
    if (!delayMS) {
        delayMS = 250;
    }

    return function() {
        clearTimeout(timeout);
        timeout = setTimeout(fn, delayMS);
    }
}

var pfBrowsers = {
    isIE: function() {
        return navigator.userAgent.toLowerCase().includes('trident/') || navigator.userAgent.toLowerCase().includes('msie');
    },
    isEdge: function() {
        return navigator.userAgent.toLowerCase().includes('edge');
    },
    isChrome: function() {
        return navigator.userAgent.toLowerCase().includes('chrome') && navigator.vendor.toLowerCase().includes('google');
    },
    isFirefox: function() {
        return navigator.userAgent.toLowerCase().includes('firefox');
    },
    isSafari: function() {
        return navigator.userAgent.toLowerCase().includes('safari') && !navigator.userAgent.toLowerCase().includes('chrome');
    }
};

$(document).ready(function(){

    var subIdInput = $('<input />')
        .attr('type', 'hidden')
        .attr('id', 'PF_BRIGGS_SUBMISSION_ID')
        .attr('ng-value', 'submissionId');
    $('#briggsForm').append(subIdInput);

    //A little cheat here to workaround incompatibilities with the forms-menu page and the form html pages
    var formName = (function() {
        if ($('form[name = "briggsForm"]').length > 0) {
            return 'briggsForm';
        }
        else if ($('form[name = "aliasSearchForm"]').length > 0) {
            return 'aliasSearchForm';
        }
        else {
            console.log('There is no form to attach messages to');
            return '';
        }
    })();

    var dateInputs = $('input[type = "date"]');
    $(dateInputs)
        .attr('type', 'text')
        .attr('old-type', 'date')
        .attr('pattern-type', inputPatterns.types.date)
        .attr('pattern', inputPatterns.findPattern(inputPatterns.types.date).pattern)
        .addClass('briggs-validate');
        // .after(inputPatterns.createMessages(inputPatterns.types.date, formName));
    if ($.ui) {
        $(dateInputs)
            .click(function(e){
                e.preventDefault();
            })
            .datepicker({
                dateFormat: "yy-mm-dd", //This is actually a 4-digit year, for some reason this is how they represent it
                autoSize: true,
                changeMonth: true,
                changeYear: true,
                minDate: new Date(1970, 1, 1),
                yearRange: '1970:c+20'
            });
    }

    //Fix Zip Code inputs
    var zipCodeInputs = $('input[briggs-zipcode]');
    $(zipCodeInputs)
        .attr('pattern-type', inputPatterns.types.zip)
        .attr('pattern', inputPatterns.findPattern(inputPatterns.types.zip).pattern)
        .addClass('briggs-validate');
        // .after(inputPatterns.createMessages(inputPatterns.types.zip, formName));
    $(zipCodeInputs)
        .filter('[type = "number"]')
        .attr('type', 'text')
        .attr('old-type', 'number');

    //Fix Zip Code Last 4 inputs
    var zipCodeLast4Inputs = $('input[briggs-zipcodelast4]');
    $(zipCodeLast4Inputs)
        .attr('pattern-type', inputPatterns.types.zipLast4)
        .attr('pattern', inputPatterns.findPattern(inputPatterns.types.zipLast4).pattern)
        .addClass('briggs-validate');
        // .after(inputPatterns.createMessages(inputPatterns.types.zipLast4, formName));
    $(zipCodeLast4Inputs)
        .filter('[type = "number"]')
        .attr('type', 'text')
        .attr('old-type', 'number');

    //Fix Time inputs
    var timeInputs = $('input[type = "time"]');
    $(timeInputs)
        .attr('pattern-type', inputPatterns.types.time)
        .attr('pattern', inputPatterns.findPattern(inputPatterns.types.time).pattern)
        .addClass('briggs-validate')
        .attr('type', 'text')
        .attr('old-type', 'time');
    $.each(timeInputs, function(index, field) {
        $(field).addClass('input-small');
        var parent = $(field).parent();
        $(field).remove();
        var div = $('<div class="input-group bootstrap-timepicker timepicker"></div>');
        $(parent).append(div);
        $(div).append(field);
        $(field).timepicker({
            defaultTime: false,
            minuteStep: 1,
            snapToStep: true
        });
    });

    //Fix Signature inputs
    var signatureInputs = $('[briggs-signature]');
    $.each(signatureInputs, function(index, input) {
        var height = $(input).css('height');
        var parent = $(input).parent();

        $(input).remove();

        var div = $('<div class="pf-signature-view" ng-click="signature.openModal($event)"></div>');
        var img = $('<img class="pf-img-init" />');
        $(img).attr('old-ng-model', $(input).attr('ng-model')).attr('id', $(input).attr('id'));
        $(div).append(img);
        $(parent).append(div);
        $(div).css('height', height);
    });

    //Find and fix any GoTo links that point to other pages
    $('a[data-briggsgoto]').attr('ng-click', 'goto.click($event)');

});