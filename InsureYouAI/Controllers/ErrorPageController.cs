using Microsoft.AspNetCore.Mvc;

namespace VirasureYouAI.Controllers
{
    public class ErrorPageController : Controller
    {
        [Route("ErrorPage/404")]
        public IActionResult Page404()
        {
            return View();
        }
    }
}
