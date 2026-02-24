using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.UILayout
{
    public class UILayoutSectionServiceViewComponent : ViewComponent
    {
        private readonly VirasureContext _context;

        public UILayoutSectionServiceViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var services = _context.Services.ToList();
            return View(services);
        }
    }
}
