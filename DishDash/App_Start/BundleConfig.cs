using System.Web;
using System.Web.Optimization;

namespace DishDash
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*",
                        "~/Scripts/aos.js",
                        "~/Scripts/bootstrap.budle.min.js",
                        "~/Scripts/custome.js",
                        "~/Scripts/jquery.fancybox.min.js",
                        "~/Scripts/jquery-2.2.4.min.js",
                        "~/Scripts/owl.carousel.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/aos.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/jquery.fancybox.min.css",
                      "~/Content/owl.carousel.min.css",
                      "~/Content/responsive.css",
                      "~/Content/slick.css",
                      "~/Content/header_style.css"
                      ));
        }
    }
}
