using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.UILayout
{
    public class UILayoutSectionTestimonialViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public UILayoutSectionTestimonialViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var testimonials = _context.Testimonials.ToList();
            return View(testimonials);
        }
    }
}
