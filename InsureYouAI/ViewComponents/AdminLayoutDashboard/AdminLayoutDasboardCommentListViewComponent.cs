using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.AdminLayoutDashboard
{
    public class AdminLayoutDasboardCommentListViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public AdminLayoutDasboardCommentListViewComponent(VirasureContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke() 
        {
            var comment = _context.Comments.OrderByDescending(y => y.CommentID).Include(x => x.AppUser).Include(x => x.Article).Take(7).ToList();
            return View(comment);
        }
    }
}
