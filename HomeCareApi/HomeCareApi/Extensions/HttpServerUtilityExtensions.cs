using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace HomeCareApi
{
    public static class HttpServerUtilityExtensions
    {
        public static string MapCustomPath(this HttpServerUtility server, string path) =>
            MapCustomPath(path);

        public static string MapCustomPath(this HttpServerUtilityBase server, string path) =>
            MapCustomPath(path);

        private static string MapCustomPath(string path)
        {
            const string PARENT_DIR = "..";
            const string PROJECT_DIR = "~/";
            const string COLON = ":";
            const string NETWORK_SLASH = "\\\\";
            if (path.Contains(COLON) || path.StartsWith(NETWORK_SLASH)) { return path; }
            else if (path.StartsWith(PROJECT_DIR) && path.Contains(PARENT_DIR))
            {
                var projectPhysicalPath = HostingEnvironment.MapPath(PROJECT_DIR);
                return path.Replace(PROJECT_DIR, projectPhysicalPath);
            }
            else { return HostingEnvironment.MapPath(path); }
        }
    }
}
