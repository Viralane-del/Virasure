using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.BlogDetail
{
    public class BlogDetailContentViewComponent : ViewComponent
    {
        private readonly VirasureContext _context;

        public BlogDetailContentViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int id)
        {
            var blog = _context.Articles.Where(a => a.ArticleId == id).Include(x => x.AppUser).Include(y => y.Category).FirstOrDefault();
            ViewBag.commentCount = _context.Comments.Where(x => x.ArticleId == id).Count();
            return View(blog);
        }
    }
}
