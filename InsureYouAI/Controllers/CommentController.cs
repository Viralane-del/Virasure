using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirasureYouAI.Context;

namespace VirasureYouAI.Controllers
{
    public class CommentController : Controller
    {
        private readonly VirasureContext _context;

        public CommentController(VirasureContext context)
        {
            _context = context;
        }

        public IActionResult CommentList()
        {
            var comments = _context.Comments.Include(a => a.AppUser).Include(a => a.Article).ToList();
            return View(comments);
        }
    }
}
