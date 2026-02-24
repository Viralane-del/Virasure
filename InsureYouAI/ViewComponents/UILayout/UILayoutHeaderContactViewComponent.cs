using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.UILayout
{
    public class UILayoutHeaderContactViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public UILayoutHeaderContactViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.email = _context.Contacts.Select(x => x.Email).FirstOrDefault();
            ViewBag.phone = _context.Contacts.Select(x => x.Phone).FirstOrDefault();
            return View();
        }
    }
}
