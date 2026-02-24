using Microsoft.AspNetCore.Mvc;

namespace VirasureYouAI.ViewComponents.AdminLayout
{
    public class AdminLayoutNavbarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
