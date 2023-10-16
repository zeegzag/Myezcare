var app = angular.module("myApp", ['ngLoadingSpinner', 'angularUtils.directives.dirPagination', 'ngJsTree', 'cgBusy', 'ngSanitize', 'summernote']);

var controllers = {};
app.controller(controllers);

/** 
 *  The xhr call initiated by Angular doesn’t contain the X-Requested-With header. And this is exactly the header ASP.NET MVC is looking for to detect if it’s an AJAX request or not.
 *  Referene link:  http://bartwullems.blogspot.in/2014/10/angularjs-and-aspnet-mvc-isajaxrequest.html
 */
app.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
}]);


//DISABLE TAB & APPLY BROWSER TAB FUNCTIONALITY ON SUMMER NOTE
$.summernote.pluginEvents['tab'] = function (event, editor, layoutInfo) { };
$.summernote.pluginEvents['untab'] = function (event, editor, layoutInfo) { };

$.summernote.addPlugin({
    name: 'customEnter',
    events: {
        'insertParagraph': function (evt) {
            if (evt.which === 13 || evt.keyCode === 13)
                evt.shiftKey = true;
        }
    }
});
$.summernote.options.toolbar = [
    //['style', ['style']],
    ['font', ['bold', 'italic', 'underline']],
    //['fontname', ['fontname']],
    //['color', ['color']],
     ['para', ['ul', 'ol']],
     ['height', ['height']],
    //['table', ['table']],
    //['insert', ['link', 'picture', 'hr']],
     ['view', ['codeview']],
];



//#region Angular Application Filters

app.filter('numberFixedLen', ['$filter', function ($filter) {
    return function (num, len) {
        if (num % 1 != 0) {
            if (len == undefined) len = 2;
            return $filter('number')(num, len);
        } else
            return num;
    };
}]);

app.filter('chatdateformat', function () {
    return function (dateString, format) {
        return dateString != null ? moment(dateString).fromNow() : '';
    };
});

app.filter('dateformat', function () {
    return function (dateString, format) {
        return dateString != null ? moment(dateString).format(format ? format : 'MM/DD/YYYY') : '';
    };
});

app.filter('datetimeformat', function () {
    return function (dateString, format) {
        return dateString != null ? moment(dateString).format(format ? format : 'MM/DD/YYYY h:mm a') : '';
    };
});


app.filter('twodatetimeformat', function () {
    return function (dateString, dateString2, format) {
        //debugger;
        if (moment(dateString).format('MM/DD/YYYY') === moment(dateString2).format('MM/DD/YYYY')) {

            var d1Str = dateString != null ? moment(dateString).format('H:mm') : '';
            var d2Str = dateString2 != null ? moment(dateString2).format('H:mm') : '';
            return d1Str + " - " + d2Str;

        } else {

            var d1Str = dateString != null ? moment(dateString).format(format ? format : 'MM/DD/YYYY h:mm a') : '';
            var d2Str = dateString2 != null ? moment(dateString2).format(format ? format : 'MM/DD/YYYY h:mm a') : '';
            return d1Str + " - " + d2Str;
        }

        //return dateString != null ? moment(dateString).format(format ? format : 'MM/DD/YYYY h:mm a') : '';
    };
});



app.filter('datetimeformatlll', function () {
    return function (dateString, format) {
        return dateString != null ? moment(dateString).format('lll') : '';
    };
});

app.filter('timeformat', function () {
    return function (dateString, format) {
        return dateString != null ? moment(dateString).format('hh:mm a') : 'N/A';

        //To convert date time in local time
        //return dateString != null ? moment.utc(dateString).local().format('hh:mm a') : 'N/A';
    };
});

app.filter('timeformat12hrs', function () {
    return function (dateString, format) {
        return dateString != null ? moment(dateString.substr(0, 8), ['HH:mm:sss']).format('hh:mm a') : 'N/A';
    };
});


app.filter('timeinhrs', function () {
    return function (dateString, format) {
        var d = moment.duration(dateString, "minutes");
        return dateString = moment().startOf('day').add(d).format('HH:mm');
        //return dateString != null ? moment(dateString, ['HH:mm:sss']).format('hh:mm a') : 'N/A';
    };
});

app.filter('counterValue', function () {
    return function (value) {
        var valueInt = parseInt(value);
        if (!isNaN(value) && value >= 0 && value < 10)
            return "0" + valueInt;
        return "";
    };
});

app.filter('removeSpaces', [function () {
    return function (string) {
        if (!angular.isString(string)) {
            return string;
        }
        return string.replace(/[\s//]/g, '');
    };
}]);

app.filter('isStatus', function () {
    return function (option, secondParam, thirdParam) { // filter arguments        
        return option.IsDeleted == 0; // implementation
    };
});

app.filter('capitalize', function () {
    return function (input) {
        return (!!input) ? input.charAt(0).toUpperCase() + input.substr(1).toLowerCase() : '';
    };
});

//app.filter('san', function () {
//    return function (input) {
//        
//        return $sce.trustAsHtml(input);
//    };
//});

app.filter('phoneformat', function () {
    return function (value) {
        return value ? value.replace(/(\d{3})(\d{3})(\d{4})/, "($1) $2-$3") : 'N/A';
    };
});

app.filter('stringsubstring', function () {
    return function (value, length) {
        var str = value ? value.substring(0, length) + "<b>...</b>" : 'N/A';
        return $("<div/>").html(str).text();
    };
});


app.filter('stringsubstringsimple', function () {
    return function (value, length) {
        var str = value ? value.substring(0, length) + "<b>...</b>" : 'N/A';
        return str;
    };
});

app.filter('htmlString', function () {
    return function (value) {
        return $("<div/>").html(value).text();
    };
});


//app.filter('amazonPreSignedURL', function () {
//    return function (value) {
//        if (value) {
//            AWS.config = {
//                accessKeyId: window.AccessKeyID,
//                //sessionToken: window.Signature
//                secretAccessKey: window.SecretAccessKeyID
//            };
//            //AWS.config.loadFromPath('/Amazon-Credentials.json');
//            var s3 = new AWS.S3();
//            var params = { Bucket: window.ZarephathBucket, Key: value }; //, Expires: 10000
//            var url = s3.getSignedUrl('getObject', params);
//            //console.log("The URL is", url);
//            return url;
//        } else {
//            return null;
//        }
//    };
//});


app.filter('tel', function () {
    return function (tel) {
        if (!tel) { return ''; }

        var value = tel.toString().trim().replace(/^\+/, '');

        if (value.match(/[^0-9]/)) {
            return tel;
        }

        var country, city, number;

        switch (value.length) {
            case 1:
            case 2:
            case 3:
                city = value;
                break;

            default:
                city = value.slice(0, 3);
                number = value.slice(3);
        }

        if (number) {
            if (number.length > 3) {
                number = number.slice(0, 3) + '-' + number.slice(3, 7);
            }
            else {
                number = number;
            }

            return ("(" + city + ") " + number).trim();
        }
        else {
            return "(" + city;
        }

    };
});


app.filter('unique', function () {

    return _.memoize(function (items, filterOn) {
        if (filterOn === false) {
            return items;
        }
        if (items != undefined && (filterOn || angular.isUndefined(filterOn)) && angular.isArray(items)) {
            var hashCheck = {}, newItems = [];

            var extractValueToCompare = function (item) {
                if (angular.isObject(item) && angular.isString(filterOn)) {
                    return item[filterOn];
                } else {
                    return item;
                }
            };


            angular.forEach(items, function (item) {
                var valueToCheck, isDuplicate = false;
                for (var i = 0; i < newItems.length; i++) {
                    if (angular.equals(extractValueToCompare(newItems[i]), extractValueToCompare(item))) {
                        isDuplicate = true;
                        break;
                    }
                }
                if (!isDuplicate) {
                    newItems.push(item);
                }

            });
            items = newItems;
        }
        return items;
    });
});
//#endregion

