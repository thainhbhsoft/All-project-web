using System.Web;
using System.Web.Optimization;

namespace UET_BTL_VERSION_1
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bootstrap/css")
                .Include(
                       "~/Contents/CSS/bootstrap.min.css",
                       new CssRewriteUrlTransform()
                        )
               .Include(
                       "~/Contents/CSS/dashboard.css"
                        ));
                
            bundles.Add(new ScriptBundle("~/bootstrap-jquery/js").Include(
                      "~/Contents/js/jquery-3.3.1.min.js",
                      "~/Contents/js/bootstrap.min.js"
                      ));
            bundles.Add(new ScriptBundle("~/teacher/js").Include(
                     "~/Contents/js/teacher.js"
                     ));
            bundles.Add(new ScriptBundle("~/student/js").Include(
                     "~/Contents/js/student.js"
                     ));
            bundles.Add(new ScriptBundle("~/subject/js").Include(
                     "~/Contents/js/subject.js"
                     ));
            bundles.Add(new ScriptBundle("~/survey/js").Include(
                     "~/Contents/js/survey.js"
                     ));

            bundles.Add(new StyleBundle("~/login/css").Include(
                     "~/Contents/CSS/Login.css"
                     ));
            BundleTable.EnableOptimizations = true;
        }
    }
}
