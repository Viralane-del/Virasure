using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.Blog
{
    public class BlogLast3RecentBlogViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public BlogLast3RecentBlogViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var blog = _context.Articles.OrderByDescending(x =>x.ArticleId).Take(3).ToList();
            return View(blog);
        }
    }
}
