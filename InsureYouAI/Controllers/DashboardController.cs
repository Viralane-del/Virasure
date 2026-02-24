using Microsoft.AspNetCore.Mvc;

namespace VirasureYouAI.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
