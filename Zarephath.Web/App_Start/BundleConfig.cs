using System.Web;
using System.Web.Optimization;
using Zarephath.Core.Infrastructure;

namespace Zarephath.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = ConfigSettings.EnableBundlingMinification;
            bundles.IgnoreList.Clear();

            #region Common With Respite Care

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
                 "~/assets/css/sitecss/myEzCarelight.css"
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
                "~/Assets/js/sitejs/plugins/signature/signature_pad.js",
                "~/assets/js/sitejs/angular/angular.js",
                "~/assets/js/sitejs/angular/angular-animate.min.js",
                "~/assets/js/sitejs/angular/angular-busy.js",
                "~/assets/js/sitejs/angular/jstree.js",
                "~/assets/js/sitejs/angular/ngJsTree.js",
                "~/Assets/js/sitejs/angular/angular-steps.js",
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
                "~/Assets/js/sitejs/plugins/signature/signature.js",
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
                "~/assets/js/sitejs/getAcrobatDetails.js",
                "~/assets/js/viewJS/common.js",
                "~/Assets/js/sitejs/angular-drag-and-drop-lists.js"
                            ));

            #endregion
            #region Site Layout for SevaArpan
            bundles.Add(new StyleBundle("~/sitelayoutSevaArpan/css").Include(
                 "~/Assets/css/sitecss/opensans.css",
                 "~/Assets/css/sitecss/font-awesome.css",
                 "~/Assets/css/sitecss/simple-line-icons.css",
                 "~/Assets/css/sitecss/bootstrap.css",
                 "~/Assets/css/sitecss/uniform.default.css",
                 "~/Assets/css/sitecss/components_SevaArpan.css",
                 "~/Assets/css/sitecss/plugins.css",
                 "~/Assets/css/sitecss/layout_SevaArpan.css",
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
                 "~/Assets/css/sitecss/site_SevaArpan.css",
                  "~/assets/library/countycode/css/intlTelInput.css",
                  "~/assets/css/sitecss/myEzCarelight_SevaArpan.css",
                  "~/assets/css/sitecss/UIKit.css"
                 ));

            bundles.Add(new ScriptBundle("~/assets/sitelayoutSevaArpan/js").Include(
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
                "~/Assets/js/sitejs/plugins/signature/signature_pad.js",
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
                "~/Assets/js/sitejs/plugins/signature/signature.js",
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
                "~/assets/js/sitejs/getAcrobatDetails.js",
                "~/assets/js/viewJS/common.js"
                            ));

            #endregion

            #region Login Layout


            bundles.Add(new StyleBundle("~/printlayout/css").Include(
               "~/Assets/css/sitecss/font-awesome.css",
               "~/Assets/css/sitecss/bootstrap.css",
               "~/Assets/css/sitecss/uniform.default.css",
               "~/assets/library/datetimepicker/bootstrap-datetimepicker.css",
               "~/Assets/css/sitecss/components.css",
               "~/Assets/css/sitecss/toaster.css",
               "~/Assets/css/sitecss/style.css",
               "~/Assets/css/sitecss/site.css"
               ));

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
            #region Login css for Seva Arpan
            bundles.Add(new StyleBundle("~/loginlayoutSevaArpan/css").Include(
               "~/Assets/css/sitecss/font-awesome.css",
               "~/Assets/css/sitecss/bootstrap.css",
               "~/Assets/css/sitecss/uniform.default.css",
               "~/assets/library/datetimepicker/bootstrap-datetimepicker.css",
               "~/Assets/css/sitecss/components.css",
               "~/Assets/css/sitecss/toaster.css",
               "~/Assets/css/sitecss/login_SevaArpan.css",
               "~/Assets/css/sitecss/style.css",
               "~/Assets/css/sitecss/site.css"
               ));
            #endregion

            bundles.Add(new StyleBundle("~/generatehtmltopdflayout/css").Include(
               "~/Assets/css/sitecss/font-awesome.css",
               "~/Assets/css/sitecss/bootstrap.css",
               "~/Assets/css/sitecss/uniform.default.css",
               "~/assets/library/datetimepicker/bootstrap-datetimepicker.css",
               "~/Assets/css/sitecss/style.css",
               "~/Assets/css/sitecss/site.css"
               ));

            bundles.Add(new ScriptBundle("~/generatehtmltopdflayout/js").Include(
               "~/Assets/js/sitejs/jquery.js"
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
                 "~/Assets/js/viewjs/resource.js",
                 "~/Assets/js/sitejs/angular/angular-sanitize.js"
                            ));



            bundles.Add(new ScriptBundle("~/assets/js/viewjs/security/login").Include(
                "~/assets/js/viewJS/loginApp/security/index.js"
                ));

            bundles.Add(new ScriptBundle("~/assets/js/viewJS/loginApp/security/setpassword").Include(
                "~/assets/js/viewJS/loginApp/security/setpassword.js"
                ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/loginapp/security/securityquestion").Include(
               "~/assets/js/viewjs/loginapp/security/securityquestion.js"
               ));

            bundles.Add(new ScriptBundle("~/viewjs/siteApp/home/dashboard").Include(
             "~/assets/js/viewjs/siteapp/home/dashboard.js"
             ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/loginapp/security/forgotpassword").Include(
               "~/assets/js/viewjs/loginapp/security/forgotpassword.js"
               ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/loginapp/security/resetpassword").Include(
               "~/assets/js/viewjs/loginapp/security/resetpassword.js"
               ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/notificationjs").Include(
              "~/assets/js/viewJS/notification.js"
              ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/loginapp/security/editprofile").Include(
             "~/assets/js/viewjs/loginapp/security/editprofile.js"
             ));

            bundles.Add(new ScriptBundle("~/assets/js/viewJS/loginApp/security/createlogin").Include(
             "~/assets/js/viewJS/loginApp/security/createlogin.js"
             ));

            #endregion

            #region Department

            bundles.Add(new ScriptBundle("~/viewjs/siteApp/department/adddepartment").Include(
                "~/assets/library/token_input/src/jquery.tokeninput.js",
               "~/Assets/js/viewjs/siteApp/department/adddepartment.js"
               ));

            bundles.Add(new ScriptBundle("~/viewjs/siteApp/department/departmentlist").Include(
                "~/Assets/js/viewjs/siteApp/department/departmentlist.js"
                ));

            #endregion Department

            #region Facility House

            bundles.Add(new ScriptBundle("~/viewjs/siteApp/facilityhouse/addfacilityhouse").Include(
                "~/assets/js/sitejs/jquery.minicolors.js",
                "~/assets/library/token_input/src/jquery.tokeninput.js",
               "~/Assets/js/viewjs/siteApp/facilityhouse/addfacilityhouse.js"
               ));

            bundles.Add(new ScriptBundle("~/viewjs/siteApp/facilityhouse/facilityhouselist").Include(
                "~/Assets/js/viewjs/siteApp/facilityhouse/facilityhouselist.js"
                ));

            #endregion Facility House

            #region Employee Master




            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/signature").Include(
                 "~/assets/library/jquery.signature/excanvas.js",
                "~/assets/library/jquery.signature/jquery.ui.touch-punch.js",
                "~/assets/library/jquery.signature/jquery.signature.js"
                            ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/employee/addemployee").Include(
                "~/assets/js/viewjs/siteApp/employee/addemployee.js"
                            ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/employee/employeelist").Include(
             "~/assets/js/viewjs/siteApp/employee/employeelist.js"
             ));

            #endregion

            #region RolePermission
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/security/rolepermission").Include(
                "~/assets/js/viewJS/loginApp/security/rolepermission.js"
                ));
            #endregion RolePermission

            #region Case Manager

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/casemanager/addcasemanager").Include(
              "~/assets/js/viewjs/siteApp/casemanager/addcasemanager.js"
              ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/casemanager/casemanagerlist").Include(
             "~/assets/js/viewjs/siteApp/casemanager/casemanagerlist.js"
             ));

            #endregion

            #region Parent

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/parent/addparent").Include(
              "~/assets/js/viewjs/siteApp/parent/addparent.js"
              ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/parent/parentlist").Include(
             "~/assets/js/viewjs/siteApp/parent/parentlist.js"
             ));

            #endregion



            #region Referral

            bundles.Add(new ScriptBundle("~/viewjs/siteApp/referral/addreferral").Include(
                "~/assets/js/viewjs/siteApp/referral/addreferral.js",
                "~/assets/js/viewjs/siteApp/referral/referraldocument.js",
                "~/assets/js/viewjs/siteApp/referral/referralchecklist.js",
                "~/assets/js/viewjs/siteApp/referral/referralsparform.js",
                "~/assets/js/viewjs/siteApp/referral/referralinternalmessage.js",
                "~/assets/js/viewjs/siteApp/referral/referralreviewandmeasurement.js",
                "~/assets/js/viewjs/siteApp/schedule/schedulemaster.js",
                "~/assets/js/viewjs/siteApp/note/index.js",
                "~/assets/js/viewjs/siteApp/referral/ui-general.js"
             ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/referral/referrallist").Include(
             "~/assets/js/viewjs/siteApp/referral/referrallist.js"
             ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/referral/referraltracking").Include(
             "~/assets/js/viewjs/siteApp/referral/referraltrackinglist.js"
             ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/note/addnote").Include(
                        "~/assets/js/viewjs/siteApp/note/index.js"
                        ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/note/clientnotes").Include(
                        "~/assets/js/viewjs/siteApp/note/clientnotes.js"
                        ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/note/groupnote").Include(
                        "~/assets/js/viewjs/siteApp/note/groupnote.js"
                        ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/note/changeservicecode").Include(
                        "~/assets/js/viewjs/siteApp/note/changeservicecode.js"
                        ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/referral/groupmonthlysummary").Include(
                       "~/assets/js/viewjs/siteApp/referral/groupmonthlysummary.js"
                       ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/referral/referralreviewandmeasurement").Include(
                "~/assets/js/viewjs/siteApp/referral/referralreviewandmeasurement.js"
                ));



            #endregion

            #region Schedule

            bundles.Add(new StyleBundle("~/assets/css/viewjs/siteApp/schedule/scheduleassignment").Include(
               "~/assets/library/ion.rangeslider/css/ion.rangeSlider.css",
               "~/assets/library/ion.rangeslider/css/ion.rangeSlider.skinFlat.css",
               "~/assets/library/fullcalendar/fullcalendar.css"
               ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/schedule/scheduleassignment").Include(
                "~/Assets/js/sitejs/jquery.nicescroll.js",
                "~/Assets/js/sitejs/freewall.js",
                "~/assets/library/token_input/src/jquery.tokeninput.js",
                "~/assets/library/fullcalendar/fullcalendar.js",
               "~/assets/library/ion.rangeslider/js/ion.rangeSlider.js",
               "~/assets/js/viewjs/siteApp/schedule/scheduleassignment.js",
               "~/assets/js/sitejs/lodash.js"
            ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/schedule/schedulemaster").Include(
                "~/assets/js/viewjs/siteApp/schedule/schedulemaster.js"
                            ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/schedule/schedulebatchservicelist").Include(
                "~/assets/js/viewjs/siteApp/schedule/schedulebatchservicelist.js"
                            ));

            #endregion

            #region Transportation Location

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/transportlocation/addtransportlocation").Include(
                 "~/assets/library/token_input/src/jquery.tokeninput.js",
                "~/Assets/js/sitejs/jquery.pulsate.js",
                "~/assets/library/jquery.file.upload/jquery.fileupload.js",
                "~/assets/js/viewjs/siteApp/referral/ui-general.js",
              "~/assets/js/viewjs/siteApp/transportlocation/addtransportlocation.js"
              ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/transportlocation/transportlocationlist").Include(
                "~/assets/js/viewjs/siteApp/transportlocation/transportlocationlist.js"
              ));

            #endregion

            #region TransPortationGroup

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/transportationgroup/transportationassignment").Include(
                "~/Assets/js/sitejs/jquery.nicescroll.js",
                "~/assets/js/viewjs/siteApp/transportationgroup/transportationassignment.js"
              ));

            #endregion

            #region Cancellation Email

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/schedule/scheduleemailcancellation").Include(
                "~/assets/js/viewjs/siteApp/schedule/scheduleemailcancellation.js"
              ));

            #endregion

            #region PayorDetail

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/payor/addpayor").Include(
                "~/assets/js/viewjs/siteApp/payor/addpayor.js",
                "~/assets/js/viewjs/siteApp/payor/servicecode.js"
              ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/payor/payorlist").Include(
                "~/assets/js/viewjs/siteApp/payor/payorlist.js"
              ));

            #endregion


            #region Service Code

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/servicecode/addservicecode").Include(
                "~/assets/js/viewjs/siteApp/servicecode/addservicecode.js"
              ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/servicecode/servicecodelist").Include(
                "~/assets/js/viewjs/siteApp/servicecode/servicecodelist.js"
              ));

            #endregion

            #region Attendance Master

            bundles.Add(new StyleBundle("~/assets/css/viewjs/siteApp/attendancemaster/attendancemaster").Include(
               "~/assets/library/fullcalendar/fullcalendar.css"
               ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/attendancemaster/attendancemaster").Include(
                "~/assets/library/fullcalendar/fullcalendar.js",
             "~/assets/js/viewjs/siteApp/attendancemaster/attendancemaster.js"
            ));

            #endregion

            #region DashBoard

            bundles.Add(new ScriptBundle("~/Assets/js/viewjs/siteApp/home/dashboard/referraldocumentlist").Include(
             "~/Assets/js/viewjs/siteApp/home/dashboard/referraldocumentlist.js"
             ));

            bundles.Add(new ScriptBundle("~/Assets/js/viewjs/siteApp/home/dashboard/referralinternalmessagelist").Include(
              "~/Assets/js/viewjs/siteApp/home/dashboard/referralinternalmessagelist.js"
            ));
            bundles.Add(new ScriptBundle("~/Assets/js/viewjs/siteApp/home/dashboard/referralsparformlist").Include(
           "~/Assets/js/viewjs/siteApp/home/dashboard/referralsparformlist.js"
           ));
            #endregion

            #region Dx Code

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/dxcode/adddxcode").Include(
              "~/assets/js/viewjs/siteApp/dxcode/adddxcode.js"
              ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/dxcode/dxcodelist").Include(
             "~/assets/js/viewjs/siteApp/dxcode/dxcodelist.js"
             ));

            #endregion  Dx Code


            #region Note Sentence

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/notesentence/addnotesentence").Include(
              "~/assets/js/viewjs/siteApp/notesentence/addnotesentence.js"
              ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/notesentence/notesentencelist").Include(
             "~/assets/js/viewjs/siteApp/notesentence/notesentencelist.js"
             ));

            #endregion  Note Sentence

            #region EmailTemplate

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/emailtemplate/addemailtemplate").Include(
              "~/assets/js/viewjs/siteApp/emailtemplate/addemailtemplate.js"

              ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/emailtemplate/emailtemplatelist").Include(
             "~/assets/js/viewjs/siteApp/emailtemplate/emailtemplatelist.js"
             ));

            #endregion  Dx Code

            #region Batch

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/batch/batchmaster").Include(
            "~/assets/js/viewjs/siteApp/batch/batchmaster.js"
            ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/batch/edifilelog").Include(
         "~/assets/js/viewjs/siteApp/batch/edifileloglist.js"
         ));

            #endregion

            #region Batch Upload 835

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/batch/upload835").Include(
            "~/assets/js/viewjs/siteApp/batch/upload835.js"
            ));

            #endregion

            #region Batch Upload 835

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/batch/reconcile835").Include(
            "~/assets/js/viewjs/siteApp/batch/reconcile835.js"
            ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/batch/reconcile").Include(
           "~/assets/js/viewjs/siteApp/batch/reconcile.js"
           ));
            #endregion

            #region Reports

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/report/reportjs").Include(
                "~/assets/js/viewjs/siteApp/report/rpt_attendance.js",
                "~/assets/js/viewjs/siteApp/report/rpt_behaviorcontracttracking.js",
                "~/assets/js/viewjs/siteApp/report/rpt_clientInformation.js",
                "~/assets/js/viewjs/siteApp/report/rpt_clientstatus.js",
                "~/assets/js/viewjs/siteApp/report/rpt_internalserviceplan.js",
                "~/assets/js/viewjs/siteApp/report/rpt_referraldetails.js",
                "~/assets/js/viewjs/siteApp/report/rpt_respiteusage.js",
                "~/assets/js/viewjs/siteApp/report/rpt_snapshotprint.js",
                "~/assets/js/viewjs/siteApp/report/rpt_encounterprint.js",
                "~/assets/js/viewjs/siteApp/report/rpt_dtrprint.js",
                "~/assets/js/viewjs/siteApp/report/rpt_generalnotice.js",
                "~/assets/js/viewjs/siteApp/report/rpt_dsproster.js",
                "~/assets/js/viewjs/siteApp/report/rpt_scheduleattendance.js",
                "~/assets/js/viewjs/siteApp/report/rpt_requireddocsforattendance.js",
                "~/assets/js/viewjs/siteApp/report/rpt_lifeskillsoutcomemeasurements.js",
                "~/assets/js/viewjs/siteApp/report/rpt_lsteammembercaseload.js",
                "~/assets/js/viewjs/siteApp/report/rpt_requestclientlist.js"

                ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/report/billingsummary").Include(
                "~/assets/js/viewjs/siteApp/report/rpt_billingsummury.js"
                ));

            #endregion

            #region Agency

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/agency/addagency").Include(
                "~/assets/js/viewjs/siteApp/agency/addagency.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/agency/agencylist").Include(
                "~/assets/js/viewjs/siteApp/agency/agencylist.js"
                ));

            #endregion

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/report/lsteammembercaseload").Include(
                "~/assets/js/viewjs/siteApp/report/lsteammembercaseloadlist.js"
                ));


            #region Process 270 & 271
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/batch/process270_271").Include(
                "~/assets/js/viewjs/siteApp/batch/process270.js",
                "~/assets/js/viewjs/siteApp/batch/process271.js"
                ));
            #endregion



            #region Process 277CA
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/batch/process277CA").Include(
                "~/assets/js/viewjs/siteApp/batch/process277CA.js"
                ));
            #endregion


            #endregion

            #region In Home Care Related

            #region Referral
            bundles.Add(new ScriptBundle("~/viewjs/siteApp/homecare/referral/addreferral").Include(
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/addreferral.js",
                    //"~/assets/js/viewjs/siteApp/Areas/Staffing/facility/addreferral.js",
                    //"~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referraltaskmapping.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referralinternalmessage.js",
                    // "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referraltimeslots.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referralblockemployee.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referralnote.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referralchecklist.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referraldocument.js",
                    //"~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referraldocument01.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referraldocument1.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/mifdetail.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/ui-general.js",
                    "~/assets/js/sitejs/jquery.minicolors.js",
                    "~/assets/library/pdfobject/pdfobject.js"
                    //"~/assets/js/viewjs/siteApp/Areas/HomeCare/dmas/dmas97abList.js",
                    //"~/assets/js/viewjs/siteApp/Areas/HomeCare/dmas/add-dmas97ab.js",
                    //"~/assets/js/viewjs/siteApp/Areas/HomeCare/dmas/dmas99List.js",
                    //"~/assets/js/viewjs/siteApp/Areas/HomeCare/dmas/add-dmas99.js",
                    //"~/assets/js/viewjs/siteApp/Areas/HomeCare/dmas/cms485.js",
                    //"~/assets/js/sitejs/signature.js"

                 ));

            bundles.Add(new ScriptBundle("~/viewjs/siteApp/homecare/referral/careplan").Include(
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/careplan/careform.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/careplan/referraltaskmapping.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/careplan/referraltimeslots.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/careplan/referralcaretypetimeslots.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/careplan/referralcaseload.js"
                 ));

            bundles.Add(new ScriptBundle("~/viewjs/siteApp/homecare/referral/billingdetails").Include(
                   "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/billingdetails/referralpatientspayor.js",
                   "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/billingdetails/billingsetting.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/homecare/referral/referrallist").Include(
                 "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referrallist.js",
                 //"~/assets/js/viewjs/siteApp/Areas/Staffing/facility/referrallist.js",
                 "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/SendBulkEmail.js"
                 ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/staffing/facility/referrallist").Include(
                "~/assets/js/viewjs/siteApp/Areas/staffing/facility/referrallist.js",
                "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/SendBulkEmail.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referraldocument1").Include(
                 "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referraldocument1.js",
                 "~/assets/js/sitejs/jquery.minicolors.js"
                 ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/homecare/schedule/virtualvisit").Include(
                 "~/assets/js/viewjs/siteApp/Areas/HomeCare/schedule/virtualvisit.js"
                 ));
            #endregion
           

            #region TaskQuestion
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/visittask/addvisittask").Include(
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/visittask/addvisittask.js"
                 ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/visittask/visittasklist").Include(
             "~/assets/js/viewjs/siteApp/Areas/HomeCare/visittask/visittasklist.js"
             ));
            #endregion

            #region AssessmentQuestion
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/assessmentquestion/addassessmentquestion").Include(
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/assessmentquestion/addassessmentquestion.js"
                 ));
            #endregion

            #region Preference
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/preference/addpreference").Include(
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/preference/addpreference.js"
                 ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/preference/preferencelist").Include(
             "~/assets/js/viewjs/siteApp/Areas/HomeCare/preference/preferencelist.js"
             ));
            #endregion

            #region Employee



            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/sendbulksms").Include(
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/sendbulksms.js"
                 ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/broadcastnotification").Include(
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/broadcastnotification.js"
                 ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/addemployee").Include(
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/addemployee.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeetimeslots.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeedayofflist.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referralnote.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referralcertificate.js"
                 ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeelist").Include(
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeelist.js",
                     //"~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeelist.js",
                     "~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeetimeslots.js",
                     "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/SendBulkEmail.js"

                 ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeedayofflist").Include(
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeedayofflist.js"
                 ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeedocument").Include(
                "~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeedocument.js"
            ));





            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeetimeslots").Include(
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeetimeslots.js"
                 ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referraltimeslots").Include(
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/careplan/referraltimeslots.js",
                     "~/assets/js/sitejs/lodash.js"
                 ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referralcaretypetimeslots").Include(
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/careplan/referralcaretypetimeslots.js"
                 ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/formlist").Include(
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/formlist.js"
                 ));



            //bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeecalender").Include(
            //        "~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeecalender.js"
            //     ));



            bundles.Add(new StyleBundle("~/assets/css/viewjs/siteApp/employee/employeecalender").Include(
                "~/assets/library/fullcalendar/fullcalendar.css",
                "~/assets/library/fullcalendar/scheduler.css",
                "~/assets/library/LineProgressbar/jquery.lineProgressbar.css",
                "~/assets/css/sitecss/timeline.css"
             ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeecalender").Include(
               "~/Assets/js/sitejs/jquery.nicescroll.js",
               "~/Assets/js/sitejs/freewall.js",
               "~/assets/library/token_input/src/jquery.tokeninput.js",
               "~/assets/library/fullcalendar/fullcalendar-schedular.js",
               "~/assets/library/fullcalendar/scheduler.js",
              "~/assets/library/ion.rangeslider/js/ion.rangeSlider.js",
              "~/assets/library/LineProgressbar/jquery.lineProgressbar.js",
              "~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeecalender.js",
              "~/assets/js/sitejs/lodash.js"
           ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeechecklist").Include(
               "~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeechecklist.js"
            ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeenotificationprefs").Include(
               "~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/employeenotificationprefs.js"
            ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referralcalender").Include(
              "~/Assets/js/sitejs/jquery.nicescroll.js",
              "~/Assets/js/sitejs/freewall.js",
              "~/assets/library/token_input/src/jquery.tokeninput.js",
              "~/assets/library/fullcalendar/fullcalendar-schedular.js",
              "~/assets/library/fullcalendar/scheduler.js",
             "~/assets/library/ion.rangeslider/js/ion.rangeSlider.js",
             "~/assets/library/LineProgressbar/jquery.lineProgressbar.js",
             "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referralcalender.js",
             "~/assets/js/sitejs/lodash.js"
          ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/sentreceivedsms").Include(
               "~/assets/js/viewjs/siteApp/Areas/HomeCare/employee/sentreceivedsms.js"
           ));




            #endregion

            #region Report
            bundles.Add(new StyleBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/report/employeevisitlist").Include(
                "~/assets/js/viewjs/siteApp/Areas/HomeCare/report/employeevisitlist.js"
           ));
            bundles.Add(new StyleBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/report/employeevisitnotelist").Include(
                "~/assets/js/viewjs/siteApp/Areas/HomeCare/report/employeevisitnotelist.js"
           ));

            bundles.Add(new StyleBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/report/employeebillingreport").Include(
              "~/assets/js/viewjs/siteApp/Areas/HomeCare/report/employeebillingreport.js"
         ));

            bundles.Add(new StyleBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/report/PatientTotalreport").Include(
              "~/assets/js/viewjs/siteApp/Areas/HomeCare/report/PatientTotalreport.js"
              ));
            bundles.Add(new StyleBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/report/DMASForm").Include(
             "~/assets/js/viewjs/siteApp/Areas/HomeCare/report/DMASForm.js"
             ));
            bundles.Add(new StyleBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/report/nursesignature").Include(
                "~/assets/js/viewjs/siteApp/Areas/HomeCare/report/nursesignature.js"
            ));
            #endregion

            #region Agency

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/homecare/agency/addagency").Include(
                "~/assets/js/viewjs/siteApp/areas/homecare/agency/addagency.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/homecare/agency/agencylist").Include(
                "~/assets/js/viewjs/siteApp/areas/homecare/agency/agencylist.js"
                ));

            #endregion

            #region Patient Time Sheet
            bundles.Add(new StyleBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/patienttimesheet/patienttimesheet").Include(
                "~/assets/js/viewjs/siteApp/Areas/HomeCare/patienttimesheet/patienttimesheet.js"
            ));
            #endregion

            #region Service Code

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/servicecode/addservicecode").Include(
                "~/assets/js/viewjs/siteApp/areas/homeCare/servicecode/addservicecode.js"
              ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/servicecode/servicecodelist").Include(
                "~/assets/js/viewjs/siteApp/areas/homeCare/servicecode/servicecodelist.js"
              ));

            #endregion

            #region Scheduling

            bundles.Add(new StyleBundle("~/assets/css/viewjs/siteApp/schedule/scheduleassignmentinhome").Include(
                 "~/assets/library/ion.rangeslider/css/ion.rangeSlider.css",
                 "~/assets/library/ion.rangeslider/css/ion.rangeSlider.skinFlat.css",
                 "~/assets/library/fullcalendar/fullcalendar.css",
                 "~/assets/library/fullcalendar/scheduler.css",
                 "~/assets/library/LineProgressbar/jquery.lineProgressbar.css",
                 "~/assets/css/sitecss/timeline.css"

           ));

            bundles.Add(new StyleBundle("~/nurseschedule/css").Include(
                "~/assets/library/bootstrap-jsyearcalender/js-year-calendar.min.css",
                 "~/assets/library/bootstrap-jsyearcalender/bootstrap-datepicker.standalone.min.css",
                 "~/assets/library/semantic-multiselect/semantic.min.css"
                ));

            bundles.Add(new ScriptBundle("~/nurseschedule/js").Include(
                "~/assets/library/bootstrap-jsyearcalender/js-year-calendar.min.js",
                "~/assets/library/bootstrap-jsyearcalender/bootstrap-datepicker.min.js",
                "~/assets/library/bootstrap-jsyearcalender/popper.min.js",
                "~/assets/library/bootstrap-jsyearcalender/yearlycalender-script.js",
                "~/assets/library/semantic-multiselect/semantic.min.js"
            ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/schedule/scheduleassignmentinhome").Include(
               "~/Assets/js/sitejs/jquery.nicescroll.js",
               "~/Assets/js/sitejs/freewall.js",
               "~/assets/library/token_input/src/jquery.tokeninput.js",
               "~/assets/library/fullcalendar/fullcalendar-schedular.js",
               "~/assets/library/fullcalendar/scheduler.js",
              "~/assets/library/ion.rangeslider/js/ion.rangeSlider.js",
              "~/assets/library/LineProgressbar/jquery.lineProgressbar.js",
              "~/assets/js/viewjs/siteApp/areas/homeCare/schedule/scheduleassignmentinhome.js",
              "~/assets/js/sitejs/lodash.js"
           ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/schedule/scheduleassignmentinhome01").Include(
               "~/Assets/js/sitejs/jquery.nicescroll.js",
               "~/Assets/js/sitejs/freewall.js",
               //"~/assets/library/token_input/src/jquery.tokeninput.js",
               "~/assets/library/fullcalendar/fullcalendar-schedular.js",
               "~/assets/library/fullcalendar/scheduler.js",
              //"~/assets/library/ion.rangeslider/js/ion.rangeSlider.js",
              "~/assets/library/LineProgressbar/jquery.lineProgressbar.js",
              "~/assets/js/viewjs/siteApp/areas/homeCare/schedule/scheduleassignmentinhome01.js",
              "~/assets/js/sitejs/lodash.js"
           ));



            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/schedule/privateduty/privateduty_scheduleassignmentinhome").Include(
               "~/Assets/js/sitejs/jquery.nicescroll.js",
               "~/Assets/js/sitejs/freewall.js",
               //"~/assets/library/token_input/src/jquery.tokeninput.js",
               "~/assets/library/fullcalendar/fullcalendar-schedular.js",
               "~/assets/library/fullcalendar/scheduler.js",
              //"~/assets/library/ion.rangeslider/js/ion.rangeSlider.js",
              "~/assets/library/LineProgressbar/jquery.lineProgressbar.js",
              "~/assets/js/viewjs/siteApp/areas/homeCare/schedule/privateduty/privateduty_scheduleassignmentinhome.js",
              "~/assets/js/sitejs/lodash.js"
           ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/schedule/privateduty/privateduty_emprefschoptions").Include(
               "~/assets/js/viewjs/siteApp/areas/homeCare/schedule/privateduty/privateduty_emprefschoptions.js"
                           ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/schedule/schedulemaster").Include(
               "~/assets/js/viewjs/siteApp/areas/homeCare/schedule/schedulemaster.js"
                           ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/schedule/daycare/schedulemaster").Include(
            "~/assets/js/viewjs/siteApp/areas/homeCare/schedule/daycare/daycare_schedulemaster.js"
                        ));



            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/schedule/emprefschoptions").Include(
               "~/assets/js/viewjs/siteApp/areas/homeCare/schedule/emprefschoptions.js"
                           ));






            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/schedule/daycare/scheduleassignment").Include(
                  "~/Assets/js/sitejs/jquery.nicescroll.js",
                  "~/Assets/js/sitejs/freewall.js",
                  //"~/assets/library/token_input/src/jquery.tokeninput.js",
                  "~/assets/library/fullcalendar/fullcalendar-schedular.js",
                  "~/assets/library/fullcalendar/scheduler.js",
                 //"~/assets/library/ion.rangeslider/js/ion.rangeSlider.js",
                 "~/assets/library/LineProgressbar/jquery.lineProgressbar.js",
                 "~/assets/js/viewjs/siteApp/areas/homeCare/schedule/daycare/daycare_scheduleassignment.js",
                 "~/assets/js/sitejs/lodash.js"
              ));




            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/schedule/casemanagement/scheduleassignment").Include(
                "~/Assets/js/sitejs/jquery.nicescroll.js",
                "~/Assets/js/sitejs/freewall.js",
                //"~/assets/library/token_input/src/jquery.tokeninput.js",
                "~/assets/library/fullcalendar/fullcalendar-schedular.js",
                "~/assets/library/fullcalendar/scheduler.js",
                //"~/assets/library/ion.rangeslider/js/ion.rangeSlider.js",
                "~/assets/library/LineProgressbar/jquery.lineProgressbar.js",
                "~/assets/js/viewjs/siteApp/areas/homeCare/schedule/casemanagement/casemanagement_scheduleassignment.js",
                "~/assets/js/sitejs/lodash.js"
            ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/schedule/casemanagement/emprefschoptions").Include(
                "~/assets/js/viewjs/siteApp/areas/homeCare/schedule/casemanagement/casemanagement_emprefschoptions.js"
            ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/schedule/daycare/emprefschoptions").Include(
              "~/assets/js/viewjs/siteApp/areas/homeCare/schedule/daycare/daycare_emprefschoptions.js"
                          ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/schedule/daycare/scheduleattendence").Include(
              "~/assets/js/viewjs/siteApp/areas/homeCare/schedule/daycare/daycare_scheduleattendence.js"
                          ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/schedule/daycare/uploadpatientcsv").Include(
                "~/assets/js/viewjs/siteApp/Areas/HomeCare/schedule/daycare/daycare_uploadpatientcsv.js"
                          ));











            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/dashboard").Include(
               "~/assets/js/viewjs/siteApp/areas/homeCare/home/dashboard.js",
                "~/assets/js/viewjs/siteApp/areas/homeCare/home/Notification.js",
               //"~/assets/js/sitejs/chart/chartist.js"
               "~/assets/js/sitejs/chart/Chart.bundle.js",
               "~/assets/js/sitejs/chart/Chart.js"


                           ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/home/Notification").Include(
               "~/assets/js/viewjs/siteApp/areas/homeCare/home/Notification.js"
                ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/schedule/pendingschedules").Include(
              "~/assets/js/viewjs/siteApp/Areas/HomeCare/schedule/pendingschedules.js"
            ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/chart").Include(
               "~/assets/js/sitejs/chart/Chart.bundle.js",
               "~/assets/js/sitejs/chart/Chart.js"
               ));

            #endregion

            #region RolePermission
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/security/rolepermission.js").Include(
                "~/assets/js/viewjs/siteApp/Areas/HomeCare/security/rolepermission.js"
                ));
            #endregion RolePermission

            #region PayorDetail

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/payor/addpayor").Include(
                "~/assets/js/viewjs/siteApp/Areas/HomeCare/payor/addpayor.js",
                "~/assets/js/viewjs/siteApp/Areas/HomeCare/payor/servicecode.js"
              //,"~/assets/js/viewjs/siteApp/Areas/HomeCare/payor/payorbilling.js"
              ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/payor/payorlist").Include(
                "~/assets/js/viewjs/siteApp/Areas/HomeCare/payor/payorlist.js"
              ));

            #endregion

            #region OrganizationSetting

            //bundles.Add(new ScriptBundle("~/assets/library/countycode/css/intlTelInput").Include(
            //   "~/assets/library/countycode/css/intlTelInput.css",
            //   "~/assets/library/countycode/css/demo.css"
            // ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/setting/organizationsetting").Include(
                "~/assets/js/viewjs/siteApp/Areas/HomeCare/setting/organizationsetting.js"
             ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/setting/termsandconditionsetting").Include(
    "~/assets/js/viewjs/siteApp/Areas/HomeCare/setting/termsandconditions.js"
 ));
            #endregion

            #region DDMaster
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/generalmaster/addgeneralmaster").Include(
                "~/assets/js/sitejs/jquery.minicolors.js",
               "~/assets/js/viewjs/siteApp/Areas/HomeCare/generalmaster/addgeneralmaster.js"
             ));
            #endregion


            #region General DDL Changes
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/generalmaster/_addgeneralmastermodel").Include(

                "~/Assets/js/viewjs/siteApp/Areas/HomeCare/generalmaster/_addgeneralmastermodel.js",
                "~/Assets/js/viewjs/siteApp/Areas/HomeCare/generalmaster/google-address-library.js"
             ));
            #endregion

            #region Compliance
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/compliance/compliance").Include(
               "~/assets/js/viewjs/siteApp/Areas/HomeCare/compliance/compliance.js",
               "~/assets/js/sitejs/jquery.minicolors.js"
             ));
            #endregion

            #region Physician
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/physician/addphysician").Include(
               "~/assets/js/viewjs/siteApp/Areas/HomeCare/physician/addphysician.js"
             ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/physician/physicianlist").Include(
               "~/assets/js/viewjs/siteApp/Areas/HomeCare/physician/physicianlist.js"
             ));
            #endregion

            #region Category
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/category/addcategory").Include(
              "~/assets/js/viewjs/siteApp/Areas/HomeCare/category/addcategory.js"
            ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/category/categorylist").Include(
               "~/assets/js/viewjs/siteApp/Areas/HomeCare/category/categorylist.js"
             ));
            #endregion

            #region EbMarkets
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/markets/addmarket").Include(
              "~/assets/js/viewjs/siteApp/Areas/HomeCare/markets/addmarket.js"
            ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/markets/marketlist").Include(
              "~/assets/js/viewjs/siteApp/Areas/HomeCare/markets/marketlist.js"
            ));
            #endregion

            #region EbForms
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/ebforms/addforms").Include(
              "~/assets/js/viewjs/siteApp/Areas/HomeCare/ebforms/addforms.js"
            ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/ebforms/ebformslist").Include(
             "~/assets/js/viewjs/siteApp/Areas/HomeCare/ebforms/ebformslist.js"
           ));
            #endregion

            #region Dx Code

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/dxcode/adddxcode").Include(
              "~/assets/js/viewjs/siteApp/areas/homeCare/dxcode/adddxcode.js"
              ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/dxcode/dxcodelist").Include(
             "~/assets/js/viewjs/siteApp/areas/homeCare/dxcode/dxcodelist.js"
             ));

            #endregion  Dx Code

            #region Batch
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/batch/batchmaster").Include(
                "~/assets/js/viewjs/siteApp/areas/homeCare/batch/batchmaster.js"
                ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/batch/edifilelog").Include(
                "~/assets/js/viewjs/siteApp/areas/homeCare/batch/edifileloglist.js"
                ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/batch/process270_271").Include(
                "~/assets/js/viewjs/siteApp/areas/homeCare/batch/process270.js",
                "~/assets/js/viewjs/siteApp/areas/homeCare/batch/process271.js"
                ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/batch/process277CA").Include(
                "~/assets/js/viewjs/siteApp/areas/homeCare/batch/process277CA.js"
                ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/batch/reconcile").Include(
                "~/assets/js/viewjs/siteApp/areas/homeCare/batch/reconcile.js"
                ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/batch/reconcile835").Include(
                "~/assets/js/viewjs/siteApp/areas/homeCare/batch/reconcile835.js"
                ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/batch/upload835").Include(
                "~/assets/js/viewjs/siteApp/areas/homeCare/batch/upload835.js"
                ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/batch/latesERA").Include(
                "~/assets/js/viewjs/siteApp/areas/homeCare/batch/latestERA.js"
                ));

            bundles.Add(new StyleBundle("~/assets/css/sitecss/cms1500").Include(
                "~/assets/css/sitecss/CMS1500.css"
                ));
            bundles.Add(new StyleBundle("~/assets/css/sitecss/ub04").Include(
               "~/assets/css/sitecss/UB04.css"
               ));
            #endregion




            #region AR Agign Report

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/batch/aragingreport").Include(
                "~/assets/js/viewjs/siteApp/areas/homeCare/batch/aragingreport.js"
            ));

            #endregion

            #region FacilityHouse
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/facilityhouse/addfacilityhouse").Include(
                "~/assets/js/sitejs/jquery.minicolors.js",
                "~/assets/library/token_input/src/jquery.tokeninput.js",
               "~/assets/js/viewjs/siteApp/Areas/HomeCare/facilityhouse/addfacilityhouse.js"
             ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/facilityhouse/facilityhouselist").Include(
               "~/assets/js/viewjs/siteApp/Areas/HomeCare/facilityhouse/facilityhouselist.js"
             ));
            #endregion

            #region Invoice

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/areas/homecare/invoice/invoicedetail").Include(
               "~/assets/js/viewjs/siteApp/areas/homecare/invoice/invoicedetail.js"
             ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/areas/homecare/invoice/invoicelist").Include(
               "~/assets/js/viewjs/siteApp/areas/homecare/invoice/invoicelist.js"
             ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/areas/homecare/invoice/companyClientInvoiceList").Include(
               "~/assets/js/viewjs/siteApp/areas/homecare/invoice/companyClientInvoiceList.js"
             ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/areas/homecare/UserPaymentDetail/AddUserPaymentDetail").Include(
         "~/assets/js/viewjs/siteApp/areas/homecare/UserPaymentDetail/AddUserPaymentDetail.js"
            ));
            #endregion


            #region FormList
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/areas/homecare/form/formlist").Include(
              "~/Assets/js/viewjs/siteApp/areas/homecare/form/formlist.js"
              ));


            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/areas/homecare/form/organizationform").Include(
                "~/Assets/js/viewjs/siteApp/areas/homecare/form/organizationform.js"
            ));
            #endregion

            #region On Boarding
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/areas/homecare/onboarding").Include(
                "~/Assets/js/viewjs/siteApp/areas/homecare/onboarding/getstarted.js"
            ));

            bundles.Add(new StyleBundle("~/onboarding/css").Include(
               "~/Assets/fontawesome/css/all.min.css",
               //"~/Assets/css/sitecss/font-awesome.css",
               "~/Assets/css/sitecss/bootstrap.css",
               "~/Assets/css/sitecss/uniform.default.css",
               "~/assets/library/datetimepicker/bootstrap-datetimepicker.css",
               "~/Assets/css/sitecss/onboarding.css",
               "~/Assets/fontawesome/css/all.min.css"
               ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/generalmaster/partial/bsinessline").Include(
                "~/assets/js/sitejs/jquery.minicolors.js",
               "~/assets/js/viewjs/siteApp/Areas/HomeCare/generalmaster/partial/bsinessline.js"
             ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/generalmaster/partial/npioption").Include(
                "~/assets/js/sitejs/jquery.minicolors.js",
               "~/assets/js/viewjs/siteApp/Areas/HomeCare/generalmaster/partial/npioption.js"
             ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/generalmaster/partial/payorgroup").Include(
                "~/assets/js/sitejs/jquery.minicolors.js",
               "~/assets/js/viewjs/siteApp/Areas/HomeCare/generalmaster/partial/payorgroup.js"
             ));
            #endregion

            #region Notification Configuration
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/notificationconfiguration/index").Include(
               "~/assets/js/viewjs/siteApp/Areas/HomeCare/notificationconfiguration/index.js"
               ));
            #endregion

            #endregion





            bundles.Add(new StyleBundle("~/CronJoblayout/css").Include(
                "~/Assets/css/sitecss/font-awesome.css",
                "~/Assets/css/sitecss/bootstrap.css",
                "~/Assets/css/sitecss/uniform.default.css",
                "~/assets/library/datetimepicker/bootstrap-datetimepicker.css",
                "~/Assets/css/sitecss/components.css",
                "~/Assets/css/sitecss/toaster.css",
                "~/Assets/css/sitecss/style.css",
                "~/Assets/css/sitecss/site.css"
                ));



            bundles.Add(new ScriptBundle("~/assets/sitelayout/cronjobjs").Include(

                "~/Assets/js/viewjs/cronjob.js"
               
                            ));


            #region Transport Service

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/transportservice/addtransportservice").Include(
               "~/assets/js/viewjs/siteApp/Areas/HomeCare/transportservice/addtransportservice.js"
               ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/transportservice/transportservicelist").Include(
                "~/assets/js/viewjs/siteApp/Areas/HomeCare/transportservice/transportservicelist.js"
                ));

            #endregion Transport Service

            #region Vehicle

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/transportservice/addvehicle").Include(
               "~/assets/js/viewjs/siteApp/Areas/HomeCare/transportservice/addvehicle.js"
               ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/transportservice/vehiclelist").Include(
                "~/assets/js/viewjs/siteApp/Areas/HomeCare/transportservice/vehiclelist.js"
                ));
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/transportservice/addtransportmapping").Include(
               "~/assets/js/viewjs/siteApp/Areas/HomeCare/transportservice/addtransportmapping.js"
               ));
            #endregion Vehicle

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/HomeCare/Attendance/calendar").Include(
               "~/Assets/js/sitejs/jquery.nicescroll.js",
               "~/Assets/js/sitejs/freewall.js",
               "~/assets/library/token_input/src/jquery.tokeninput.js",
               "~/assets/library/fullcalendar/fullcalendar-schedular.js",
               "~/assets/library/fullcalendar/scheduler.js",
              "~/assets/library/ion.rangeslider/js/ion.rangeSlider.js",
              "~/assets/library/LineProgressbar/jquery.lineProgressbar.js",
              "~/assets/js/viewjs/siteApp/Areas/HomeCare/Attendance/calendar.js",
              "~/assets/js/sitejs/lodash.js"
           ));

            #region Staffing
            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteApp/Areas/staffing/facility/referraltimeslots").Include(
                   "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/careplan/referraltimeslots.js",
                    "~/assets/js/sitejs/lodash.js"
                ));
            bundles.Add(new ScriptBundle("~/viewjs/siteApp/staffing/facility/careplan").Include(
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/careplan/careform.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/careplan/referraltaskmapping.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/careplan/referraltimeslots.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/careplan/referralcaretypetimeslots.js",
                    "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/careplan/referralcaseload.js"
                 ));
            bundles.Add(new ScriptBundle("~/viewjs/siteApp/staffing/facility/addreferral").Include(
                  //"~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/addreferral.js",
                  "~/assets/js/viewjs/siteApp/Areas/Staffing/facility/addreferral.js",
                  "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referralinternalmessage.js",
                  "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referralblockemployee.js",
                  "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referralnote.js",
                  "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referralchecklist.js",
                  "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referraldocument.js",
                  "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/referraldocument1.js",
                  "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/mifdetail.js",
                  "~/assets/js/viewjs/siteApp/Areas/HomeCare/referral/ui-general.js",
                  "~/assets/js/sitejs/jquery.minicolors.js",
                  "~/assets/library/pdfobject/pdfobject.js"

               ));
            #endregion
        }
    }
}