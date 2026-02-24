using Microsoft.AspNetCore.Mvc;

namespace VirasureYouAI.ViewComponents.UILayout
{
    public class UILayoutHeaderViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
