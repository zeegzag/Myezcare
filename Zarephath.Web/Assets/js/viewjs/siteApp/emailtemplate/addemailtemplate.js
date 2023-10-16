var custModel;
controllers.AddEmailTemplateController = function ($scope, $http, $window) {
    custModel = $scope;
    //$scope.IsEdit = false;

    $scope.EmailTemplateModel = $.parseJSON($("#hdnEmailTemplateModel").val());



    $scope.SaveEmailTemplateDetails = function () {
        var selectedID = $("#EmailTemplateTypeIDs").val();
        var selectedModuleID = $("#ddlModuleName").val();
        $scope.EmailTemplateModel.EmailTemplate.GetTokens = $scope.EmailTemplateModel.EmailTemplate.Token;
        $scope.EmailTemplateModel.EmailTemplate.Email = selectedID;
        $scope.EmailTemplateModel.EmailTemplate.Module = selectedModuleID;
        //$scope.EmailTemplateModel.EmailTemplate.EmailTemplateBody = $('.panel-body').text();
        var jsonData = angular.toJson({
            emailTemplate: {
                EmailTemplate: $scope.EmailTemplateModel.EmailTemplate

            }
        });
        AngularAjaxCall($http, SiteUrl.AddEmailTemplate, jsonData, "Post", "json", "application/json").success(function (response) {
            if (response.IsSuccess) {
                SetMessageForPageLoad(response.Message, "EmailTemplateUpdateSuccessMessage");

                $window.location = SiteUrl.EmailTemplateList;
                $window.location = SiteUrl.EmailTemplateList;
            }
            else {
                ShowMessages(response);
            }
        });
        //}
    };

    getTokens();
    function getTokens() {
        var moduleName = $('#ddlModuleName :selected').text();
        if (moduleName !== null) {
            var jsonData = angular.toJson({ module: 'Patient' });
            AngularAjaxCall($http, "/emailtemplate/GetTokenList/", jsonData, "Post", "json", "application/json").success(function (response) {
                if (response.IsSuccess) {
                    $scope.TokenList = response.Data;
                }

            });
        }
    }

    $scope.Cancel = function () {
        $window.location = SiteUrl.EmailTemplateList;
    };


};

controllers.AddEmailTemplateController.$inject = ['$scope', '$http', '$window'];
function SelectFields() {
    var fields = document.getElementById("selectToken").value;
    pasteHtmlAtCaret(fields);
}

function pasteHtmlAtCaret(html) {
    var sel, range;
    if (window.getSelection) {
        // IE9 and non-IE
        sel = window.getSelection();
        if (sel.getRangeAt && sel.rangeCount) {
            range = sel.getRangeAt(0);
            range.deleteContents();

            // Range.createContextualFragment() would be useful here but is
            // non-standard and not supported in all browsers (IE9, for one)
            var el = document.createElement("div");
            el.innerHTML = html;
            var frag = document.createDocumentFragment(), node, lastNode;
            while ((node = el.firstChild)) {
                lastNode = frag.appendChild(node);
            }
            range.insertNode(frag);

            // Preserve the selection
            if (lastNode) {
                range = range.cloneRange();
                range.setStartAfter(lastNode);
                range.collapse(true);
                sel.removeAllRanges();
                sel.addRange(range);
            }
        }
    } else if (document.selection && document.selection.type != "Control") {
        // IE < 9
        document.selection.createRange().pasteHTML(html);
    }
}