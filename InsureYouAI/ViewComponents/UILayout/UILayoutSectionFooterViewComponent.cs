using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.UILayout
{
    public class UILayoutSectionFooterViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public UILayoutSectionFooterViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.description = _context.Contacts.Select(d => d.Description).FirstOrDefault();
            ViewBag.phone = _context.Contacts.Select(d => d.Phone).FirstOrDefault();
            ViewBag.email = _context.Contacts.Select(d => d.Email).FirstOrDefault();
            ViewBag.address = _context.Contacts.Select(d => d.Address).FirstOrDefault();
            return View();
        }
    }
}
