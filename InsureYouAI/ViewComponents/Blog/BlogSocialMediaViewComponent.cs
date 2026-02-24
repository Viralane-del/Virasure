using Microsoft.AspNetCore.Mvc;

namespace VirasureYouAI.ViewComponents.Blog
{
    public class BlogSocialMediaViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
