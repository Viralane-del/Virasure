using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.UILayout
{
    public class UILayoutSectionAboutViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public UILayoutSectionAboutViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.title = _context.Abouts.Select(t => t.Title).FirstOrDefault();
            ViewBag.description = _context.Abouts.Select(t => t.Description).FirstOrDefault();
            ViewBag.ımage = _context.Abouts.Select(t => t.ImageURL).FirstOrDefault();

            var aboutItems = _context.AboutItems.ToList();
            return View(aboutItems);
        }
    }
}
