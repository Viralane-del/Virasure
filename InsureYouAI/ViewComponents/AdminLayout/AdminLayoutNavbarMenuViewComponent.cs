using Microsoft.AspNetCore.Mvc;

namespace VirasureYouAI.ViewComponents.AdminLayout
{
    public class AdminLayoutNavbarMenuViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
