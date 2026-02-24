using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.RegularExpressions;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.Blog
{
    public class BlogListByCategoryViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public BlogListByCategoryViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int id)
        {
            var articles = _context.Articles
                .Where(x => x.CategoryId == id)
                .Include(c => c.Category)
                .Include(a => a.AppUser)
                .Include(x => x.Comments)
                .ToList();

            foreach (var item in articles)
            {
                item.Content = GenerateSummary(item.Content);
            }

            return View(articles);
        }
        private static string GenerateSummary(string htmlContent)
        {
            if (string.IsNullOrEmpty(htmlContent))
                return "";

            var plainText = Regex.Replace(htmlContent, "<.*?>", "");
            plainText = WebUtility.HtmlDecode(plainText);

            return plainText.Length > 300
                ? plainText.Substring(0, 300) + "..."
                : plainText;
        }
    }
}
