(function(){
    var App = angular.module('App', []);
    App.config(['$locationProvider', '$httpProvider', function($locationProvider, $httpProvider) {
        if(window.history && window.history.pushState){
            $locationProvider.html5Mode({
                enabled: true,
                requireBase: false,
                rewriteLinks: false
            });
        }
        else{
            $locationProvider.html5Mode(false);
        }

        $httpProvider.defaults.withCredentials = true;
        $httpProvider.interceptors.push(pfBriggsHttpInterceptors.csrfInterceptor);
        $httpProvider.interceptors.push(pfBriggsHttpInterceptors.redirectInterceptor);
    }]);
    App.controller('AppController', ['$scope','$http','$location', '$httpParamSerializerJQLike', function($scope, $http, $location, $httpParamSerializerJQLike){
        //Initialize the pfUrl object
        pfUrl.init($location);
        $scope.pfUrl = pfUrl;

        var formShortNameSplit = window.location.pathname.split('/');

        $scope.metadata = {
            showMenuBtn: $location.search().ShowMainMenu === '1',
            id: $location.search().id,
            formShortNameRaw: formShortNameSplit[formShortNameSplit.length - 1],
            form: {},
            getFormShortName: function() {
                return this.formShortNameRaw.replace('_', '/');
            }
        };

        $scope.briggs = {
            BRIGGS_PF_FORM: {
                FORM_SHORT_NAME: $scope.metadata.getFormShortName(),
                FORM_VERSION: parseFloat($location.search().version),
                FORM_TIMESTAMP: ''
            }
        };

        window.addEventListener('beforeunload', function(event){
            if(hasChanges()){
                var message = 'Any unsaved changes will be lost.';
                (event || window.event).returnValue = message;
                return message;
            }
        });

        $scope.backup = JSON.stringify($scope.briggs);

        $scope.goto = {
            click: function(event){
                var currentTarget = event.currentTarget;
                if($(currentTarget).attr('href').indexOf('html') > -1){
                    event.preventDefault();

                    if(hasChanges()){
                        $scope.saveWarningModal.savePlusAction = this.saveAndGoTo(currentTarget);
                        $scope.saveWarningModal.dontSaveAction = this.doGoTo(currentTarget);
                        $('#saveChangesWarning').modal({
                            backdrop: 'static'
                        });
                    }
                    else{
                        this.doGoTo(currentTarget)();
                    }
                }
            },
            doGoTo: function(currentTarget){
                //Return a no-arg function so that the closure can contain the parameter
                return function(){
                    //Grab all the values from the current url
                    var origin = window.location.origin;
                    var pathname = window.location.pathname;
                    var search = window.location.search.replace('\?','').replace(/&page=\d\d/,'');

                    //Grab values from the link href
                    var targetHref = $(currentTarget).attr('href');
                    var page = targetHref.split('#')[0].replace('.html','').split('-')[2];
                    var goto = $(currentTarget).attr('data-briggsgoto');

                    window.location.href = origin + pathname + '?' + search + '&page=' + page + '#' + goto;
                }
            },
            saveAndGoTo: function(currentTarget){
                //Return a no-arg function so that the closure can contain the parameter
                return function(){
                    $scope.submit.submitForm(null, $scope.goto.doGoTo(currentTarget));
                }
            }
        };

        $scope.pages = (function(){
            function ensureTwoDigits(number){
                if(('' + number).length === 1){
                    return '0' + number;
                }

                return number;
            }

            return {
                currentPage: 1,
                pageCount: 1,
                pageLinks: [],
                pageRange: 5,
                change: function(value){
                    var num = parseInt(value);
                    if(isNaN(num)){
                        if('<<' === value){
                            num = 1;
                        }
                        else if('<' === value){
                            num = this.currentPage - 1;
                        }
                        else if('>' === value){
                            num = this.currentPage + 1;
                        }
                        else if('>>' === value){
                            num = this.pageCount;
                        }
                    }
                    else if(this.currentPage === num){
                        return;
                    }

                    if(hasChanges()){
                        $scope.saveWarningModal.savePlusAction = this.saveAndChangePage(num);
                        $scope.saveWarningModal.dontSaveAction = this.doChangePage(num);
                        $('#saveChangesWarning').modal({
                            backdrop: 'static'
                        });
                    }
                    else{
                        this.doChangePage(num)();
                    }
                },
                doChangePage: function(value){
                    //Return a function so the url parameter can be passed in to be invoked later on
                    return function(){
                        var href = window.location.href;
                        if(href.search(/page=\d\d/) !== -1){
                            var newHref = window.location.href.replace(/page=\d\d/, 'page=' + ensureTwoDigits(value));
                        }
                        else{
                            var newHref = window.location.href + '&page=' + ensureTwoDigits(value);
                        }

                        var hashIndex = newHref.indexOf('#');
                        if(hashIndex > -1){
                            newHref = newHref.substring(0, hashIndex);
                        }

                        window.location.href = newHref;
                    }
                },
                saveAndChangePage: function(value){
                    //Return a function so the url parameter can be passed in to be invoked later on
                    return function(){
                        $scope.submit.submitForm(null, $scope.pages.doChangePage(value));
                    }
                },
                computePageLinks: function(){
                    this.pageLinks = [];

                    var newUrl = window.location.href.replace(window.location.search,  '?version=' + $location.search().version);
                    if($scope.metadata.id){
                        newUrl = newUrl + '&id=' + $location.search().id;
                    }

                    if($scope.metadata.showMenuBtn){
                        newUrl = newUrl + '&ShowMainMenu=1';
                    }

                    newUrl = pfUrl.addParamsToUrl(newUrl);

                    if(this.currentPage > 1){
                        this.pageLinks.push({
                            value: '<<',
                            link: newUrl + '&page=01'
                        });
                        this.pageLinks.push({
                            value: '<',
                            link: newUrl + '&page=' + ensureTwoDigits(this.currentPage - 1)
                        });
                    }

                    for(var i = 0; i < this.pageCount; i++){
                        var pageNumber = i + 1;
                        if(pageNumber >= this.currentPage - this.pageRange && pageNumber <= this.currentPage + this.pageRange){
                            this.pageLinks.push({
                                value: pageNumber,
                                link: newUrl + '&page=' + ensureTwoDigits(pageNumber)
                            });
                        }
                    }

                    if(this.currentPage < this.pageCount){
                        this.pageLinks.push({
                            value: '>',
                            link: newUrl + '&page=' + ensureTwoDigits(this.currentPage + 1)
                        });
                        this.pageLinks.push({
                            value: '>>',
                            link: newUrl + '&page=' + ensureTwoDigits(this.pageCount)
                        });
                    }
                }
            }
        })();

        $scope.alert = pfAlert;

        $scope.signature = (function() {
            var currentSignatureImageTag = null;
            var signaturePad = null;

            var canvas = $('#signature-pad')[0];
            var signatureModal = $('#signatureModal')[0];

            function resizeCanvas() {
                var ratio =  Math.max(window.devicePixelRatio || 1, 1);
                canvas.width = canvas.offsetWidth * ratio;
                canvas.height = canvas.offsetHeight * ratio;
                canvas.getContext('2d').scale(ratio, ratio);
            }

            function displaySignature() {
                //Need to test for this because this might run before the modal has ever been opened
                if (signaturePad) {
                    resizeCanvas();
                    signaturePad.clear();

                    if (currentSignatureImageTag.src) {
                        try {
                            signaturePad.fromDataURL(currentSignatureImageTag.src);
                        }
                        catch (e) {}
                    }
                }
            }

            window.addEventListener('orientationchange', createDebounce(displaySignature), false);
            window.addEventListener('resize', createDebounce(displaySignature), false);

            function openModal(event) {
                var target = event.target;
                if ('img' === target.tagName.toLowerCase()) {
                    currentSignatureImageTag = event.target;
                }
                else {
                    currentSignatureImageTag = $(event.target).find('img')[0];
                }

                $(signatureModal).modal({
                    backdrop: 'static'
                });

                signaturePad = new SignaturePad(canvas, {
                    backgroundColor: 'rgba(0, 0, 0, 0)',
                    onEnd: function() {
                        //This exists to fix the choppy/stutter bug on Edge & IE when drawing the signature
                        signaturePad.fromData(signaturePad.toData());
                    }
                });

                displaySignature();
            }

            function save() {
                $(currentSignatureImageTag).removeClass('pf-img-init');
                var modelKey = $(currentSignatureImageTag).attr('old-ng-model');
                modelKey = modelKey.replace('briggs.', '');
                var imgBase64 = signaturePad.toDataURL();
                if (signaturePad.isEmpty()) {
                    //Clear any existing saved image
                    delete $scope.briggs[modelKey];
                    $(currentSignatureImageTag).attr('src', imgBase64); //Still put the blank image here to avoid weird UI artifacts
                }
                else {
                    //Save the image
                    $scope.briggs[modelKey] = {
                        image: true,
                        data: imgBase64
                    };
                    $(currentSignatureImageTag).attr('src', imgBase64);
                }
                signaturePad.clear();
                $(signatureModal).modal('hide');
            }

            function undo() {
                if (signaturePad) {
                    var data = signaturePad.toData();

                    if (data) {
                        data.pop();
                        signaturePad.fromData(data);
                    }
                }
            }

            function clear() {
                if (signaturePad) {
                    signaturePad.clear();
                }
            }

            return {
                openModal: openModal,
                save: save,
                clear: clear,
                undo: undo
            }
        })();

        //Doing it the longer way because IE doesn't support Object.assign()
        $scope.pdf = pfPdf($scope, $http);
        $scope.pdf.saveAndPrint = function(){
            $scope.submit.submitForm(null, function(){
                $scope.pdf.doPrint();
                $scope.$apply(); //This is necessary to work around certain modal-specific bugs
            });
        };
        $scope.pdf.doPrint = function(){
            $scope.pdf.printClick.formClick($scope.metadata);
        };
        $scope.pdf.print = function(){
            if(hasChanges()){
                $scope.saveWarningModal.savePlusAction = this.saveAndPrint;
                $scope.saveWarningModal.dontSaveAction = this.doPrint;
                $('#saveChangesWarning').modal({
                    backdrop: 'static'
                });
            }
            else{
                this.doPrint();
            }
        };

        $scope.saveWarningModal = {
            dontSaveAction: function(){
                //Do nothing, will be replaced
            },
            savePlusAction: function(){
                //Do nothing, will be replaced
            }
        };

        $scope.submit = {
            submitForm: function(event, successCallbackFn){
                console.log('Submitting form data');

                $scope.briggs.BRIGGS_PF_FORM.FORM_TIMESTAMP = pfDate.toTimestampString(new Date());

                var json = JSON.stringify($scope.briggs);

                var url = '../save/' + $scope.metadata.formShortNameRaw;
                var additionalQueryString = 'version=' + $location.search().version + ($scope.metadata.id ? '&id=' + $scope.metadata.id : '');
                url = pfUrl.addParamsToUrl(url, additionalQueryString);

                $http.post(url, json, {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(function(res){
						
						
                            $scope.briggs = res.data;
                            $scope.submissionId = res.data._id.$oid;

                            $scope.backup = JSON.stringify($scope.briggs);
                            if(!$scope.metadata.id){
                                var currentUrl = window.location.href;
                                var hashSection = '';
                                var hashIndex = currentUrl.indexOf('#');
                                if(hashIndex > -1){
                                    hashSection = currentUrl.substring(hashIndex);
                                    currentUrl = currentUrl.substring(0, hashIndex);
                                }

                                var newUrl = currentUrl + '&id=' + res.data._id.$oid + hashSection;
                                window.history.pushState({path:newUrl},'',newUrl);

                                $scope.metadata.id = res.data._id.$oid;
                                $scope.metadata.form.FilledPdfURI = $scope.metadata.form.NewPdfURI + '&id=' + $scope.metadata.id;
                                $scope.metadata.form.FilledHtmlURI = $scope.metadata.form.NewHtmlURI + '&id=' + $scope.metadata.id;
                                if($scope.metadata.form.EmailBlankPdfURI){
                                    $scope.metadata.form.EmailFilledPdfURI = $scope.metadata.form.EmailBlankPdfURI + '&id=' + $scope.metadata.id;
                                }
                            }

                            updateSignatureImages();
							
							
							var resData={
								"OrginalFormId":$scope.metadata.form._id.$oid,
								"EBriggsFormID":$scope.submissionId,
								"FormId":$scope.metadata.form.FormId,
								"PageID": $scope.getQueryStringValueFromUrl("PageId")
							}
							
							resData=JSON.stringify(resData);
							
							if(window.opener)
							{
								window.opener.postMessage(resData, "*");
							}
							if(parent)
							{
								parent.postMessage(resData, "*");
							}
							
                            $scope.alert.showSuccess('Successfully submitted form data.', successCallbackFn);

                        },
                        function(res){
                            console.log('Unable to submit form data');
                            $scope.alert.showFail('Unable to submit form data: ', res);
                        });
            },
            visible: false
        };
		
		
		
		$scope.getQueryStringValueFromUrl= function(name) {
					name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
					var regexS = "[\\?&]" + name + "=([^&#]*)";
					var regex = new RegExp(regexS);
					var results = regex.exec(window.location.href);
					if (results == null)
						return "";
					else
						return results[1];
			}
		
		
		

        $scope.blockModal = {
            title: '',
            body: ''
        };

        $scope.mainMenu = {
            mainMenuClick: function(){
                if(hasChanges()){
                    $scope.saveWarningModal.savePlusAction = this.saveAndGoMainMenu;
                    $scope.saveWarningModal.dontSaveAction = this.goMainMenu;
                    $('#saveChangesWarning').modal({
                        backdrop: 'static'
                    });
                }
                else{
                    this.goMainMenu();
                }
            },
            saveAndGoMainMenu: function(){
                $scope.submit.submitForm(null, $scope.mainMenu.goMainMenu);
            },
            goMainMenu: function(){
                var url = '../forms/forms-menu.html';
                url = pfUrl.addParamsToUrl(url);
                url = url + pfUrl.getRedirectToLogin();

                window.location.href = url;
            }
        };

        function hasChanges() {
            var json = JSON.stringify($scope.briggs);
            return $scope.backup !== json;
        }

        function updateSignatureImages() {
            $.each($('.pf-signature-view > img'), function(index, signatureImage) {
                var oldNgModel = $(signatureImage).attr('old-ng-model');
                var modelKey = oldNgModel.replace('briggs.', '');
                if ($scope.briggs[modelKey] && $scope.briggs[modelKey].data) {
                    $(signatureImage).removeClass('pf-img-init');
                    $(signatureImage).attr('src', $scope.briggs[modelKey].data);
                }
            });
        }

        //IIFE to do initialization stuff when the page loads
        (function() {
            //Handle the currentPage
            var currentPage = parseInt($location.search().page);
            $scope.pages.currentPage = !isNaN(currentPage) ? currentPage : 1;

            var metadataUrl = '../metadata/' + $scope.metadata.formShortNameRaw + '?version=' + $location.search().version;
            var additionalQueryString = $scope.metadata.id ? 'id=' + $scope.metadata.id : '';
            metadataUrl = pfUrl.addParamsToUrl(metadataUrl, additionalQueryString);

            $http.get(metadataUrl)
                .then(function(res){
                        if(res.data === ''){
                            console.log('No form metadata found');
                            $scope.alert.showFail('No form metadata found');
                        }
                        else{
                            console.log('Form metadata loaded successfully');
                            $scope.metadata.form = res.data;
                            window.document.title = $scope.metadata.form.Name + ' - ' + $scope.metadata.form.FormLongName;

                            //Get the total number of pages
                            $scope.pages.pageCount = $scope.metadata.form.PageCount;
                            $scope.pages.computePageLinks();
                        }
                    },
                    function (res) {
                        console.log('Unable to load form metadata');
                        $scope.alert.showFail('Unable to load form metadata: ', res);
                    });

            //If necessary, load the data for a form submission
            if($location.search().id){
                var formDataUrl = '../load/' + $scope.metadata.formShortNameRaw + '?version=' + $location.search().version + '&id=' + $location.search().id;
                formDataUrl = pfUrl.addParamsToUrl(formDataUrl);

                $http.get(formDataUrl)
                    .then(function(res){
                            console.log('Form data loaded successfully');
                            if(res.data === ''){
                                console.log('No matching records found for form submission ID: ' + $location.search().id);
                                $scope.alert.showFail('No matching records found for form submission ID: ' + $location.search().id, res);

                                var newUrl = window.location.href.replace(window.location.search, '');
                                newUrl = pfUrl.addParamsToUrl(newUrl, 'page=' + $location.search().page + '&version=' + $location.search().version);
                                window.history.pushState({path:newUrl}, '' ,newUrl);
                                updateSignatureImages();
                            }
                            else{
                                $scope.briggs = res.data;
                                $scope.submissionId = res.data._id.$oid;
                                $scope.backup = JSON.stringify(res.data);
                                updateSignatureImages();
                            }

                            $scope.submit.visible = true;
                        },
                        function(res){
                            console.log('Unable to load form data');
                            $scope.alert.showFail('Unable to load form data: ', res);
                            $scope.submit.visible = true;
                        });
            }
            else{
                $scope.submit.visible = true;
            }
        })();

    }]);
})();

