using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.UILayout
{
    public class UILayoutSectionSliderViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public UILayoutSectionSliderViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var sectionSliders = _context.Sliders.ToList();
            return View(sectionSliders);
        }
    }
}
