using Microsoft.AspNetCore.Mvc;

namespace INDO_FIN_NET.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public ActionResult toTest()
        {
            return View();
        }
    }
}
