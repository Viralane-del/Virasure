using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.UILayout
{
    public class UILayoutSectionSkillViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public UILayoutSectionSkillViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var skills = _context.SKills.ToList();
            return View(skills);
        }
    }
}
