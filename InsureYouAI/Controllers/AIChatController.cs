using Microsoft.AspNetCore.Mvc;

namespace VirasureYouAI.Controllers
{
    public class AIChatController : Controller
    {
        public IActionResult AskAI()
        {
            return View();
        }
    }
}
