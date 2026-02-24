using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.UILayout
{
    public class UILayoutCounterStatisticViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public UILayoutCounterStatisticViewComponent(VirasureContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke() 
        {
            ViewBag.categoryCount = _context.Categories.Count();
            ViewBag.serviceCount = _context.Services.Count();
            ViewBag.userCount = _context.Users.Count();
            ViewBag.articleCount = _context.Articles.Count();
            return View();
        }
    }
}
