using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.RegularExpressions;
using VirasureYouAI.Context;
using VirasureYouAI.Models;

namespace VirasureYouAI.ViewComponents.UILayout
{
    public class UILayoutSectionBlogViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public UILayoutSectionBlogViewComponent(VirasureContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var articles = _context.Articles
               .Include(a => a.AppUser)
               .OrderByDescending(a => a.ArticleId)
               .Take(3)
               .Select(a => new UIFooterArticleViewModel
               {
                   Title = a.Title,
                   ImageUrl = a.CoverImageURL,
                   CreatedDate = a.CreatedDate,
                   Author = a.AppUser.Name + " " + a.AppUser.Surname,
                   Summary = GenerateSummary(a.Content)
               })
               .ToList();

            return View(articles);
        }
        private static string GenerateSummary(string htmlContent)
        {
            var decoded = WebUtility.HtmlDecode(htmlContent);
            var plainText = Regex.Replace(decoded, "<.*?>", "");
            plainText = Regex.Replace(plainText, @"\s+", " ").Trim();

            return plainText.Length > 140
                ? plainText.Substring(0, 140) + "..."
                : plainText;
        }
    }
}
