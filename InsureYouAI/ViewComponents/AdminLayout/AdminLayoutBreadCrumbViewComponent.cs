using Microsoft.AspNetCore.Mvc;

namespace VirasureYouAI.ViewComponents.AdminLayout
{
    public class AdminLayoutBreadCrumbViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
