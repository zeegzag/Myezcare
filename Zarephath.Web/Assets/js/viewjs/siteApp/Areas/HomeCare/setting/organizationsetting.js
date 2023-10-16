var custModel;
controllers.SystemSettingController = function ($scope, $http) {
    custModel = $scope;
    var modelJson = $.parseJSON($("#SettingHiddenModel").val());
    $scope.SettingModel = modelJson.OrganizationSetting;

    $scope.SettingModel.TempLogoImagePath = $scope.SettingModel.SiteLogo;
    $scope.SettingModel.TempFavIconPath = $scope.SettingModel.FavIcon;
    $scope.SettingModel.TempLoginScreenLogoPath = $scope.SettingModel.LoginScreenLogo;
    $scope.SettingModel.TempTemplateLogoPath = $scope.SettingModel.TemplateLogo;

    $scope.nextPage = '';
    $scope.$on('save', function (e, args) {
        $scope.nextPage = args;
        $scope.Save();
    });
    $scope.RadioSelectChange = function (value) {
        $scope.SettingModel.ScheduleType = value == "Y";
    }

    $scope.RadioSelectEnvironemnt = function (value) {
        
        $scope.SettingModel.EnvironmentType = value == "Y";
    }
    $scope.onChangeHasAggregator = function () {
        $scope.SettingModel.HasAggregator = !$scope.SettingModel.HasAggregator;
        var selector = '.has-aggregator';
        if (!$scope.SettingModel.HasAggregator) {
            $(selector).addClass('hide');
        } else {
            $(selector).removeClass('hide');
        }
    }

    $scope.Save = function () {
        
        // if (CheckErrors($("#frmsettings"))) {
        var radioValue = $("input[name='settings']:checked").val();
        var radioValueEnv = $("input[name='radioEnvironmentType']:checked").val();
        var isShedule = (radioValue.toLowerCase() === 'true')//Boolean(radioValue);
        var isChecked = $('#checkEnforce').is(':checked');
        var isEnvType = (radioValueEnv.toLowerCase() === 'true')//Boolean(radioValue);

        $scope.SettingModel.ScheduleType = isShedule;
        $scope.SettingModel.EnforceAcrossAllClients = isChecked;
        $scope.SettingModel.EnvironmentType = isEnvType;
        var jsonData = angular.toJson($scope.SettingModel);
        AngularAjaxCall($http, HomeCareSiteUrl.SaveSettingsURL, jsonData, "post", "json", "application/json").
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                    SetMessageForPageLoad(response.Message, "ShowSettingSaveMessage");
                    if ($('#IsWizard').val() === undefined || $('#IsWizard').val() === 'False') {
                        location.reload();
                    }
                    else {
                        window.location = $scope.nextPage;
                    }
                }
                else { ShowMessages(response); }
            });
        // }
    };

    $scope.UploadFile = HomeCareSiteUrl.CommonUploadFileUrl;
    $scope.UploadingSiteLogoFileList = [];
    $scope.BeforeSend = function (e, data) {
        var isValidImage = true;
        var fileName;
        var errorMsg;

        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            if (extension !== "jpg" && extension !== "jpeg" && extension !== "png" && extension !== "bmp") {
                ShowMessage(window.InvalidImageUploadMessage.replace("{0}", extension), "error");
                isValidImage = false;
            }
            else if (file.size > parseInt(window.MaxImageUploadSizeInBytes)) {
                ShowMessage(window.MaximumUploadImageSizeMessage, "error");
                isValidImage = false;
            }

            fileName = file.name;
        });

        if (isValidImage) {
            $scope.IsFileUploading = true;
        }
        $scope.$apply();
        var response = { IsSuccess: isValidImage, Message: errorMsg };
        return response;
    };
    $scope.Progress = function (e, data) {
        console.log(data.files[0].name);
    };
    $scope.AfterSend = function (e, data) {
        $scope.IsFileUploading = false;
        var model = data.result;
        if (model.IsSuccess) {
            $scope.SettingModel.TempLogoImagePath = model.Data.TempFilePath; //model.Data.items.TempFilePath;
            $scope.SettingModel.LogoImageName = model.Data.FileOriginalName;
            $scope.SettingModel.TempLogoImageName = model.Data.TempFileName;
            $scope.UploadingSiteLogoFileList = [];
        } else {
            ShowMessages(model);
            $scope.SettingModel.TempLogoImagePath = '';
        }
        $scope.$apply();
    };

    $scope.UploadingFavIconFileList = [];
    $scope.FavIconBeforeSend = function (e, data) {
        var isValidImage = true;
        var fileName;
        var errorMsg;

        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            if (extension !== "jpg" && extension !== "jpeg" && extension !== "png" && extension !== "bmp") {
                ShowMessage(window.InvalidImageUploadMessage.replace("{0}", extension), "error");
                isValidImage = false;
            }
            else if (file.size > parseInt(window.MaxImageUploadSizeInBytes)) {
                ShowMessage(window.MaximumUploadImageSizeMessage, "error");
                isValidImage = false;
            }

            fileName = file.name;
        });

        if (isValidImage) {
            $scope.IsFileUploading = true;
        }
        $scope.$apply();
        var response = { IsSuccess: isValidImage, Message: errorMsg };
        return response;
    };
    $scope.FavIconProgress = function (e, data) {
        console.log(data.files[0].name);
    };
    $scope.FavIconAfterSend = function (e, data) {
        $scope.IsFileUploading = false;
        var model = data.result;
        if (model.IsSuccess) {
            $scope.SettingModel.TempFavIconPath = model.Data.TempFilePath; //model.Data.items.TempFilePath;
            $scope.SettingModel.FavIconName = model.Data.FileOriginalName;
            $scope.SettingModel.TempFavIconName = model.Data.TempFileName;
            $scope.UploadingFavIconFileList = [];
        } else {
            ShowMessages(model);
            $scope.SettingModel.TempFavIconPath = '';
        }
        $scope.$apply();
    };

    $scope.UploadingLoginScreenLogoFileList = [];
    $scope.LoginLogoBeforeSend = function (e, data) {
        var isValidImage = true;
        var fileName;
        var errorMsg;

        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            if (extension !== "jpg" && extension !== "jpeg" && extension !== "png" && extension !== "bmp") {
                ShowMessage(window.InvalidImageUploadMessage.replace("{0}", extension), "error");
                isValidImage = false;
            }
            else if (file.size > parseInt(window.MaxImageUploadSizeInBytes)) {
                ShowMessage(window.MaximumUploadImageSizeMessage, "error");
                isValidImage = false;
            }

            fileName = file.name;
        });

        if (isValidImage) {
            $scope.IsFileUploading = true;
        }
        $scope.$apply();
        var response = { IsSuccess: isValidImage, Message: errorMsg };
        return response;
    };
    $scope.LoginLogoProgress = function (e, data) {
        console.log(data.files[0].name);
    };
    $scope.LoginLogoAfterSend = function (e, data) {
        $scope.IsFileUploading = false;
        var model = data.result;
        if (model.IsSuccess) {
            $scope.SettingModel.TempLoginScreenLogoPath = model.Data.TempFilePath; //model.Data.items.TempFilePath;
            $scope.SettingModel.LoginScreenLogoName = model.Data.FileOriginalName;
            $scope.SettingModel.TempLoginScreenLogoName = model.Data.TempFileName;
            $scope.UploadingLoginScreenLogoFileList = [];
        } else {
            ShowMessages(model);
            $scope.SettingModel.TempLoginScreenLogoPath = '';
        }
        $scope.$apply();
    };

    $scope.UploadingTemplateLogoFileList = [];
    $scope.TemplateLogoBeforeSend = function (e, data) {
        var isValidImage = true;
        var fileName;
        var errorMsg;

        $.each(data.files, function (index, file) {
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase();
            if (extension !== "jpg" && extension !== "jpeg" && extension !== "png" && extension !== "bmp") {
                ShowMessage(window.InvalidImageUploadMessage.replace("{0}", extension), "error");
                isValidImage = false;
            }
            else if (file.size > parseInt(window.MaxImageUploadSizeInBytes)) {
                ShowMessage(window.MaximumUploadImageSizeMessage, "error");
                isValidImage = false;
            }

            fileName = file.name;
        });

        if (isValidImage) {
            $scope.IsFileUploading = true;
        }
        $scope.$apply();
        var response = { IsSuccess: isValidImage, Message: errorMsg };
        return response;
    };
    $scope.TemplateLogoProgress = function (e, data) {
        console.log(data.files[0].name);
    };
    $scope.TemplateLogoAfterSend = function (e, data) {
        $scope.IsFileUploading = false;
        var model = data.result;
        if (model.IsSuccess) {
            $scope.SettingModel.TempTemplateLogoPath = model.Data.TempFilePath; //model.Data.items.TempFilePath;
            $scope.SettingModel.TemplateLogoName = model.Data.FileOriginalName;
            $scope.SettingModel.TempTemplateLogoName = model.Data.TempFileName;
            $scope.UploadingTemplateLogoFileList = [];
        } else {
            ShowMessages(model);
            $scope.SettingModel.TempTemplateLogoPath = '';
        }
        $scope.$apply();
    };

    $scope.GoogleAuth = function (OrgId) {
        // GET google drive oAuth Url as per Organization

        var jsonData = angular.toJson({ OrganizationId: OrgId, });
        AngularAjaxCall($http, HomeCareSiteUrl.GoogleAuthUrl, jsonData, "Post", "json", "application/json").success(function (response) {
            var googleAuthUrl = response.url;

            window.location.href = googleAuthUrl;
        });
    }
    $scope.onInvoiceAddressIsBilltoPayorChecked = function () {
        $scope.SettingModel.InvoiceAddressIsBilltoPayor = !($scope.SettingModel.InvoiceAddressIsBilltoPayor || false);
        if ($scope.SettingModel.InvoiceAddressIsBilltoPayor == true) {
            $scope.SettingModel.InvoiceAddressIsIncludePatientAddress = true;
            $scope.SettingModel.InvoiceIsIncludePatientDOB = true;
            $scope.SettingModel.InvoiceAddressIsIncludePatientAddressLine1 = true;
            $scope.SettingModel.InvoiceAddressIsIncludePatientAddressLine2 = true;
            $scope.SettingModel.InvoiceAddressIsIncludePatientAddressZip = true;
        }
        else {
            $scope.SettingModel.InvoiceAddressIsIncludePatientAddress = false;
            $scope.SettingModel.InvoiceIsIncludePatientDOB = false;
            $scope.SettingModel.InvoiceAddressIsIncludePatientAddressLine1 = false;
            $scope.SettingModel.InvoiceAddressIsIncludePatientAddressLine2 = false;
            $scope.SettingModel.InvoiceAddressIsIncludePatientAddressZip = false;
        }

    }
    $scope.onInvoiceAddressIsIncludePatientAddressChecked = function () {
        $scope.SettingModel.InvoiceAddressIsIncludePatientAddress = !($scope.SettingModel.InvoiceAddressIsIncludePatientAddress || false);
        if ($scope.SettingModel.InvoiceAddressIsIncludePatientAddress == true) {
            $scope.SettingModel.InvoiceAddressIsIncludePatientAddressLine1 = true;
            $scope.SettingModel.InvoiceIsIncludePatientDOB = true;
            $scope.SettingModel.InvoiceAddressIsIncludePatientAddressLine2 = true;
            $scope.SettingModel.InvoiceAddressIsIncludePatientAddressZip = true;
        }
        else {
            $scope.SettingModel.InvoiceAddressIsIncludePatientAddressLine1 = false;
            $scope.SettingModel.InvoiceIsIncludePatientDOB = false;
            $scope.SettingModel.InvoiceAddressIsIncludePatientAddressLine2 = false;
            $scope.SettingModel.InvoiceAddressIsIncludePatientAddressZip = false;
        }
    }
    $scope.onChecked = function (chk) {
        $scope.SettingModel[chk] = !$scope.SettingModel[chk];
    }
    $scope.OpenTestEmailPopUp = function () {
        OpenModalPopUp("#TestEmailModal");
    };
    $scope.SendTestEmail = function () {
        // if (CheckErrors($("#frmsettings"))) {
        var jsonData = angular.toJson($scope.SettingModel);
        AngularAjaxCall($http, HomeCareSiteUrl.TestEmailURL, jsonData, "post", "json", "application/json").
            success(function (response, status, headers, config) {
                if (response.IsSuccess) {
                   
                        $("#Alert").text('Email sent successfully');
                        bootbox.alert({
                            title: "<span style='color: red;'>Test email success! </span>",
                            message: "Email sent successfully",
                            size: 'small'
                        });
                        return;
                    }
                    else {
                        $("#Alert").text(response.Message);
                        bootbox.alert({
                            title: "<span style='color: red;'>Test email failed!</span>",
                            message: response.Message,
                            size: 'small'
                        });
                        return;
                    }
                    SetMessageForPageLoad(response.Message, "ShowSettingSaveMessage");
                    if ($('#IsWizard').val() === undefined || $('#IsWizard').val() === 'False') {
                        location.reload();
                    }
                    else {
                        window.location = $scope.nextPage;
                    }
               // }
               //else { ShowMessages(response); }
            });
    }
};

controllers.SystemSettingController.$inject = ['$scope', '$http'];
$(document).ready(function () {
    ShowPageLoadMessage("ShowSettingSaveMessage");


    //$("#OrganizationSetting_TwilioDefaultCountryCode").intlTelInput({
    //    //nationalMode: false,
    //    // allowDropdown: false,
    //    // autoHideDialCode: false,
    //    // autoPlaceholder: "off",
    //    // dropdownContainer: "body",
    //    // excludeCountries: ["us"],
    //    // formatOnDisplay: false,
    //    // geoIpLookup: function(callback) {
    //    //   $.get("http://ipinfo.io", function() {}, "jsonp").always(function(resp) {
    //    //     var countryCode = (resp && resp.country) ? resp.country : "";
    //    //     callback(countryCode);
    //    //   });
    //    // },
    //    // hiddenInput: "full_number",
    //     //initialCountry: "auto",
    //    // localizedCountries: { 'de': 'Deutschland' },

    //    // onlyCountries: ['us', 'gb', 'ch', 'ca', 'do'],
    //    // placeholderNumberType: "MOBILE",
    //    // preferredCountries: ['cn', 'jp'],
    //    //separateDialCode: true,
    //});
});