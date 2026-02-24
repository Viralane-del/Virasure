using Microsoft.AspNetCore.Mvc;

namespace VirasureYouAI.ViewComponents.AdminLayout
{
    public class AdminLayoutSidebarViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
