using Microsoft.AspNetCore.Mvc;

namespace VirasureYouAI.Controllers
{
    public class AdminLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
