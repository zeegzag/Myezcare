$(document).ready(function () {

    $('#txtTermsCondition').summernote({
        height: 300,
        tabsize: 2,
        styleTags: [
            'p',
            { title: 'Blockquote', tag: 'blockquote', className: 'blockquote', value: 'blockquote' },
            'pre', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6'
        ],
        fontSizeUnits: ['px', 'pt'],
        toolbar: [
            ["style", ["style"]],
            ["font", ["bold", "italic", "underline", "clear"]],
            ["fontsize", ["fontsize"]],
            ["color", ["color"]],
            ["para", ["ul", "ol", "paragraph"]],
            ["height", ["height"]],
            ["table", ["table"]],
            ["insert", ["link", "hr"]],
            ["view", ["fullscreen", "codeview"]],
            ["help", ["help"]]
        ],
    });

    $('#btnSubmitTermsCondition').click(function () { 
        $("#loader").show("fast");
        $.ajax({
            type: 'POST',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            url: "/hc/Setting/SaveTermsAndConditions",
            data: "{organizationId: " + $('#organizationId').val() + ",termsAndConditions: " + JSON.stringify($("#txtTermsCondition").val()) + "}",
            success: function (data) {                
                $("#loader").hide("fast");
                ShowMessage(successMessage);
            },
            error: function () {
                $("#loader").hide("fast");
                ShowMessage(errorMessage, 'error');
            }
        });

    });
});