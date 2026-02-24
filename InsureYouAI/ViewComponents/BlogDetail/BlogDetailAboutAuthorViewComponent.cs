using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.BlogDetail
{
    public class BlogDetailAboutAuthorViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public BlogDetailAboutAuthorViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int id)
        {
            string appUserId = _context.Articles.Where(x => x.ArticleId == id).Select(a => a.AppUserId).FirstOrDefault();
            var userValue = _context.Users.Where(x => x.Id == appUserId).FirstOrDefault();
            return View(userValue);
        }
    }
}
