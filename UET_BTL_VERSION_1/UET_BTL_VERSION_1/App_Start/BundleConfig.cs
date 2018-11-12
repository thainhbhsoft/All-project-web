using System.Web;
using System.Web.Optimization;

namespace UET_BTL_VERSION_1
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/header/css")
                .Include(
                       "~/Contents/CSS/bootstrap.min.css",
                       new CssRewriteUrlTransform()
                        )
               .Include(
                       "~/Contents/CSS/dashboard.css"
                        ));
                
            bundles.Add(new ScriptBundle("~/footer/js").Include(
                      "~/Contents/js/jquery-3.3.1.min.js",
                      "~/Contents/js/bootstrap.min.js",
                      "~/Contents/js/admin.js"
                      ));

            bundles.Add(new StyleBundle("~/login/css").Include(
                     "~/Contents/CSS/Login.css"
                     ));
            BundleTable.EnableOptimizations = true;
        }
    }
}
