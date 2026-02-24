using Microsoft.AspNetCore.Mvc;

namespace VirasureYouAI.ViewComponents.Blog
{
    public class BlogTagsViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
