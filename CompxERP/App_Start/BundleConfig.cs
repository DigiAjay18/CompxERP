using System.Web;
using System.Web.Optimization;

namespace CompxERP.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js")); //, "~/Scripts/jquery-ui.css" 

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
              "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
               "~/css/bootstrap.min.css",
        "~/css/font-awesome.min.css",
        "~/css/responsive.css",
        "~/css/jquery.smartmenus.bootstrap.css", "~/css/custom.css" ));
             


            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                     "~/Scripts/jquery-2.2.3.js", "~/Scripts/ddaccordion.js",
                     "~/Scripts/bootstrap-datepicker.js",
                     "~/Scripts/jquery-1.7.1.min.js",
                     "~/Scripts/jquery-ui-1.8.20.js",
                     "~/Scripts/jquery.validate.min.js",
                     "~/Scripts/jquery.validate.unobtrusive.min.js",
                     "~/Scripts/jquery.inputmask.js",
                     "~/Scripts/jquery.inputmask.date.extensions.js",
                     "~/Scripts/jquery.inputmask.extensions.js"));

            bundles.Add(new ScriptBundle("~/Scripts/ng").Include(
            "~/Scripts/Angular/angular.js",
            "~/Scripts/Angular/angular-animate.min.js",
            "~/Scripts/Angular/angular-aria.min.js",
            "~/Scripts/Angular/angular-material.js",
            "~/Scripts/Angular/app.js"));

            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
            "~/js/jquery-1.11.3.min.js",
            "~/js/bootstrap.min.js",
            "~/js/jquery.smartmenus.js",
            "~/js/jquery.smartmenus.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/Scripts/boot").Include(
    "~/js/jquery-1.11.3.min.js" ,
    "~/js/bootstrap.min.js" ,
    "~/js/moment.js",
    "~/js/jquery.smartmenus.js",
    "~/js/jquery.smartmenus.bootstrap.js" 
    ));
            BundleTable.EnableOptimizations = true;

        }
    }
}