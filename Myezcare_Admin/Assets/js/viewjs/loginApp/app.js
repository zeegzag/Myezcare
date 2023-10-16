var app = angular.module("app", ['ngLoadingSpinner']);

/** 
 *  The xhr call initiated by Angular doesn’t contain the X-Requested-With header. And this is exactly the header ASP.NET MVC is looking for to detect if it’s an AJAX request or not.
 *  Referene link:  http://bartwullems.blogspot.in/2014/10/angularjs-and-aspnet-mvc-isajaxrequest.html
 */
app.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
}]);

var controllers = {};
app.controller(controllers);
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
//Moved Code To Login Page(Index.cshtml)
//app.config(function(FacebookProvider) {
//    // Set your appId through the setAppId method or
//    // use the shortcut in the initialize method directly.
//    FacebookProvider.init('1544780939176842');
//});

