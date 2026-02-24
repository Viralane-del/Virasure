using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.RegularExpressions;
using VirasureYouAI.Context;
using VirasureYouAI.Models;

namespace VirasureYouAI.ViewComponents.UILayout
{
    public class UILayoutSectionFooterLast2ArticleViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public UILayoutSectionFooterLast2ArticleViewComponent(VirasureContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var values = _context.Articles.OrderByDescending(x => x.ArticleId).Skip(3).Take(2).ToList();
            return View(values);
        }
       
    }
}
