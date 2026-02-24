using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.BlogDetail
{
    public class BlogDetailCommentListViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public BlogDetailCommentListViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int id)
        {
            var comment = _context.Comments.Where(x => x.ArticleId == id && x.Status == "Yorum Onaylandı").Include(a => a.AppUser).ToList();
            return View(comment);
        }
    }
}
