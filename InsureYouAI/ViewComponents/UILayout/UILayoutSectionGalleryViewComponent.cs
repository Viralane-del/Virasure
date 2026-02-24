using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.UILayout
{
    public class UILayoutSectionGalleryViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public UILayoutSectionGalleryViewComponent(VirasureContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var gallery = _context.Galleries.ToList();
            return View(gallery);
        }
    }
}
