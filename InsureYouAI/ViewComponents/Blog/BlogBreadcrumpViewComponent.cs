using Microsoft.AspNetCore.Mvc;

namespace VirasureYouAI.ViewComponents.Blog
{
    public class BlogBreadcrumpViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
