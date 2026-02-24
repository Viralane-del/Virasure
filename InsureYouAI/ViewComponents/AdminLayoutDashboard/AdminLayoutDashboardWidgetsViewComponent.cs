using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.AdminLayoutDashboard
{
    public class AdminLayoutDashboardWidgetsViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public AdminLayoutDashboardWidgetsViewComponent(VirasureContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke() 
        {
            ViewBag.articles = _context.Articles.Count();
            ViewBag.categories = _context.Categories.Count();
            ViewBag.users = _context.Users.Count();
            ViewBag.comments = _context.Comments.Count();
            return View();
        }
    }
}
