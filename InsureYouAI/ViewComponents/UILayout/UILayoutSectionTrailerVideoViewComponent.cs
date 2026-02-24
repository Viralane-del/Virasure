using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.UILayout
{
    public class UILayoutSectionTrailerVideoViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public UILayoutSectionTrailerVideoViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var trailerVideo = _context.TrailerVideos.ToList();
            return View(trailerVideo);
        }
    }
}
