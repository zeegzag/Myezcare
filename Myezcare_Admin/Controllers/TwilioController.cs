using System.Web.Mvc;

namespace Myezcare_Admin.Controllers
{
    public class TwilioController : Controller
    {
        //
        // GET: /Twilio/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit(string id)
        {
            ViewBag.id = id;
            return View();
        }
        public ActionResult Usage()
        {
            return View();
        }
    }
}
