using Microsoft.AspNetCore.Mvc;

namespace VirasureYouAI.ViewComponents.UILayout
{
    public class UILayoutNavbarViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
