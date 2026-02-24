using Microsoft.AspNetCore.Mvc;

namespace VirasureYouAI.ViewComponents.AdminLayout
{
    public class AdminLayoutSwitcherViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
