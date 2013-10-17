using System.Web.Mvc;

namespace PittyLove.FormsAuth.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Edit()
        {
            return View();
        }
    }
}
