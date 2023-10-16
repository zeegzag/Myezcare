using System.IO;
using System.Web;
using System.Web.Mvc;
using Zarephath.Core.Infrastructure;

namespace Zarephath.Core.Controllers
{
    public class UploadsController : BaseController
    {
        [HttpGet]
        public ActionResult AccessFile(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string path = Path.GetFileNameWithoutExtension(id);
                path = Crypto.Decrypt(path);
                if (!string.IsNullOrEmpty(path))
                {
                    path = HttpContext.Server.MapCustomPath(path);
                    if (System.IO.File.Exists(path))
                    {
                        string fileName = Path.GetFileName(path);
                        string mimeType = MimeMapping.GetMimeMapping(fileName);
                        return File(path, mimeType);
                    }
                }
            }
            return RedirectToAction("notFound", "security", new { area = "HomeCare" });
        }

    }
}
