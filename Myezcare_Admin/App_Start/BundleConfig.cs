using System.Web;
using System.Web.Optimization;
using Myezcare_Admin.Infrastructure;

namespace Myezcare_Admin
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            BundleTable.EnableOptimizations = ConfigSettings.EnableBundlingMinification;

            #region Common Layout
            #region Site Layout

            bundles.Add(new StyleBundle("~/sitelayout/css").Include(
                "~/Assets/css/sitecss/opensans.css",
                "~/Assets/css/sitecss/font-awesome.css",
                "~/Assets/css/sitecss/simple-line-icons.css",
                "~/Assets/css/sitecss/bootstrap.css",
                "~/Assets/css/sitecss/uniform.default.css",
                "~/Assets/css/sitecss/components.css",
                "~/Assets/css/sitecss/plugins.css",
                "~/Assets/css/sitecss/layout.css",
                "~/Assets/css/sitecss/default.css",
                "~/Assets/css/sitecss/simple-line-icons.css",
                "~/Assets/css/sitecss/jquery.minicolors.css",
                "~/assets/library/token_input/styles/token-input.css",
                "~/Assets/css/sitecss/toaster.css",
                "~/Assets/css/sitecss/style.css",
                //"~/Assets/css/sitecss/select2.css",
                "~/Assets/css/sitecss/jquery.webui-popover.css",
                "~/Assets/library/bootstrap-select/bootstrap-select.css",
                "~/Assets/library/select2dropdown/select2-bootstrap.css",
                "~/assets/library/datetimepicker/bootstrap-datetimepicker.css",
                "~/Assets/css/sitecss/inbox.css",
                //"~/Assets/css/sitecss/bootstrap-wysihtml5.css",
                "~/Assets/library/bootstrap-wysihtml5/bootstrap-wysihtml5.css",
                "~/Assets/library/bootstrap-wysihtml5/wysiwyg-color.css",
                "~/Assets/library/bootstrap-summernote/summernote.css",


                "~/Assets/library/jQuery-autoComplete-master/jquery.auto-complete.css",
                "~/Assets/css/sitecss/jstree.css",
                "~/Assets/library/jquery.signature/jquery.signature.css",
                "~/Assets/css/sitecss/ladda-themeless.css",
                "~/Assets/css/sitecss/site.css",
                 "~/assets/library/countycode/css/intlTelInput.css",
                 "~/Assets/css/sitecss/custom.css"
                ));

            bundles.Add(new ScriptBundle("~/assets/sitelayout/js").Include(
                "~/Assets/js/sitejs/jquery.js",
                "~/Assets/js/sitejs/jquery-migrate.js",
                "~/Assets/js/sitejs/jquery-ui.js",
                "~/Assets/js/sitejs/jquery.validate*",
                "~/Assets/js/sitejs/expressive.annotations.validate.js",
                "~/Assets/js/sitejs/inputmask.js",
                "~/Assets/js/sitejs/jquery.inputmask.js",
                "~/Assets/js/sitejs/inputmask.date.extensions.js",
                "~/Assets/js/sitejs/tableHeadFixer.js",
                "~/Assets/js/sitejs/bootstrap.js",
                "~/Assets/js/sitejs/jquery.uniform.js",
                "~/Assets/js/sitejs/jquery.cokie.js",
                "~/Assets/js/sitejs/underscore.js",
                "~/Assets/js/sitejs/app.js",
                "~/Assets/js/sitejs/layout.js",
                "~/Assets/js/sitejs/jquery.tabbable.js",
                "~/Assets/js/sitejs/quick-sidebar.js",
                "~/Assets/library/bootstrap-summernote/summernote.js",
                "~/assets/js/sitejs/angular/angular.js",
                "~/assets/js/sitejs/angular/angular-animate.min.js",
                "~/assets/js/sitejs/angular/angular-busy.js",
                "~/assets/js/sitejs/angular/jstree.js",
                "~/assets/js/sitejs/angular/ngJsTree.js",
                "~/assets/js/sitejs/angular/spin.js",
                "~/assets/js/sitejs/angular/angular-sanitize.js",
                "~/assets/js/sitejs/angular/angular-spinner.js",
                "~/assets/js/sitejs/angular/angular-loading-spinner.js",
                "~/Assets/js/sitejs/angular/dirPagination.js",
                "~/Assets/js/sitejs/ladda.js",
                 //"~/Assets/js/sitejs/aws-sdk-2.6.6.js",
                 //"~/Assets/library/select2dropdown/select2.js",
                 //"~/Assets/library/select2dropdown/select2.full.js",
                 "~/Assets/library/bootstrap-select/bootstrap-select.js",
                "~/assets/js/siteJS/toaster.js",
                "~/Assets/js/sitejs/moment/moment.js",
                "~/Assets/js/sitejs/moment/moment-with-locales.js",
                //"~/Assets/js/sitejs/moment/moment-timezone.js",
                //"~/Assets/js/sitejs/moment/moment-timezone-with-data.js",

                "~/Assets/js/sitejs/bootbox.js",
                "~/Assets/js/viewjs/siteApp/app.js",
                "~/assets/library/datetimepicker/bootstrap-datetimepicker.js",
                "~/assets/js/sitejs/jquery.webui-popover.js",
                "~/assets/js/sitejs/inbox.js",
                //"~/assets/js/sitejs/wysihtml5-0.3.0.js",
                //"~/assets/js/sitejs/bootstrap-wysihtml5.js",
                //"~/assets/js/sitejs/wysihtml5.js",
                "~/assets/library/bootstrap-wysihtml5/wysihtml5-0.3.0.js",
                "~/assets/library/bootstrap-wysihtml5/bootstrap-wysihtml5.js",

                "~/Assets/library/jQuery-autoComplete-master/jquery.auto-complete.js",

                "~/Assets/library/bootstrap-summernote/angular-summernote.js",
                "~/assets/library/token_input/src/jquery.tokeninput.js",
                "~/Assets/js/sitejs/jquery.pulsate.js",
                "~/assets/library/jquery.file.upload/jquery.fileupload.js",
                "~/assets/library/countycode/js/intlTelInput.js",
                "~/Assets/js/viewjs/directive.js",
                "~/Assets/js/viewjs/resource.js",
                "~/Assets/js/viewjs/url.js",
                "~/assets/js/sitejs/jquery.slimscroll.js",
                "~/assets/js/viewjs/siteApp/layout.js",
                "~/assets/js/viewJS/common.js"
                ));

            #endregion

            #region Login Layout

            bundles.Add(new StyleBundle("~/loginlayout/css").Include(
                "~/Assets/css/sitecss/font-awesome.css",
                "~/Assets/css/sitecss/bootstrap.css",
                "~/Assets/css/sitecss/uniform.default.css",
                "~/assets/library/datetimepicker/bootstrap-datetimepicker.css",
                "~/Assets/css/sitecss/components.css",
                "~/Assets/css/sitecss/toaster.css",
                "~/Assets/css/sitecss/login.css",
                "~/Assets/css/sitecss/style.css",
                "~/Assets/css/sitecss/site.css"
                ));

            bundles.Add(new ScriptBundle("~/loginlayout/js").Include(
                "~/Assets/js/sitejs/jquery.js",
                "~/Assets/js/sitejs/jquery-migrate.js",
                "~/Assets/js/sitejs/jquery-ui.js",
                "~/Assets/js/sitejs/jquery.validate*",
                "~/Assets/js/sitejs/jquery.cokie.js",
                "~/Assets/js/sitejs/bootstrap.js",
                 // "~/Assets/js/sitejs/aws-sdk-2.6.6.js",
                "~/Assets/js/sitejs/jquery.uniform.js",
                "~/assets/js/sitejs/angular/angular.js",
                "~/assets/js/sitejs/angular/spin.js",
                "~/assets/js/sitejs/angular/angular-spinner.js",
                "~/assets/js/sitejs/angular/angular-loading-spinner.js",
                "~/assets/js/siteJS/toaster.js",
                "~/Assets/js/sitejs/moment/moment.js",
                "~/Assets/js/sitejs/moment/moment-with-locales.js",
                 //"~/Assets/js/sitejs/moment/moment-timezone.js",
                 //"~/Assets/js/sitejs/moment/moment-timezone-with-data.js",
                 "~/Assets/js/sitejs/bootbox.js",
                "~/assets/library/datetimepicker/bootstrap-datetimepicker.js",
                "~/assets/library/jquery.file.upload/jquery.fileupload.js",
                 "~/assets/js/viewJS/loginApp/app.js",
                 "~/Assets/js/viewjs/directive.js",
                 "~/Assets/js/viewjs/url.js",
                 "~/assets/js/viewJS/common.js",
                 "~/Assets/js/viewjs/resource.js"
                            ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/security/login").Include(
                "~/assets/js/viewJS/loginApp/security/index.js"
                ));

            bundles.Add(new ScriptBundle("~/assets/js/viewJS/loginApp/security/setpassword").Include(
                "~/assets/js/viewJS/loginApp/security/setpassword.js"
                ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/loginapp/security/forgotpassword").Include(
               "~/assets/js/viewjs/loginapp/security/forgotpassword.js"
               ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/loginapp/security/resetpassword").Include(
               "~/assets/js/viewjs/loginapp/security/resetpassword.js"
               ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/loginapp/security/editprofile").Include(
             "~/assets/js/viewjs/loginapp/security/editprofile.js"
             ));

            #endregion
            #endregion

            #region ReleaseNote
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/releasenote/addreleasenote").Include(
                "~/Assets/js/viewjs/siteApp/releasenote/addreleasenote.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/releasenote/releasenotelist").Include(
                "~/Assets/js/viewjs/siteApp/releasenote/releasenotelist.js"
                ));
            #endregion

            #region Organization
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/organization/addorganization").Include(
                "~/Assets/js/viewjs/siteApp/organization/addorganization.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/organization/organizationlist").Include(
                "~/Assets/js/viewjs/siteApp/organization/organizationlist.js",
                "~/Assets/js/viewjs/siteApp/organization/uploadexcel.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/organization/organizationesign").Include(
                "~/Assets/js/viewjs/siteApp/organization/organizationesign.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/organization/customeresign").Include(
                "~/Assets/js/viewjs/siteApp/organization/customeresign.js"
                ));
            #endregion

            #region ServicePlan
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/serviceplan/addserviceplan").Include(
                "~/Assets/js/viewjs/siteApp/serviceplan/addserviceplan.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/serviceplan/serviceplanlist").Include(
                "~/Assets/js/viewjs/siteApp/serviceplan/serviceplanlist.js"
                ));
            #endregion


            #region FormList
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/form/formlist").Include(
              "~/Assets/js/viewjs/siteApp/form/formlist.js"
              ));

            #endregion
            #region Permissions
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/permissions/permissionslist").Include(
                "~/Assets/js/viewjs/siteapp/permissions/permissionslist.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/permissions/addpermission").Include(
                "~/Assets/js/viewjs/siteApp/permissions/addpermission.js"
                ));
            #endregion

            #region Invoice
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/Invoice/invoiceList").Include(
                "~/Assets/js/viewjs/siteapp/Invoice/invoiceList.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/Invoice/addInvoice").Include(
                "~/Assets/js/viewjs/siteApp/Invoice/addInvoice.js"
                ));
            #endregion
        }
    }
}