using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirasureYouAI.Context;
using VirasureYouAI.Models;
using System.Text.RegularExpressions;
using System.Net;

namespace VirasureYouAI.ViewComponents.Blog
{
    public class BlogAllBlogViewComponent : ViewComponent
    {
        private readonly VirasureContext _context;

        public BlogAllBlogViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            //var blogs = _context.Articles.Include(c => c.Category).Include(a => a.AppUser).ToList();
            var blogs = _context.Articles.Include(c => c.Category).Include(a => a.AppUser).Include(x => x.Comments)
                .Select(y => new BlogListViewModel
                {
                    ArticleId = y.ArticleId,
                    Author = y.AppUser.Name + " " + y.AppUser.Surname,
                    CategoryName = y.Category.CategoryName,
                    CreatedDate = y.CreatedDate,
                    ImageUrl = y.CoverImageURL,
                    Title = y.Title,
                    CommentCount = y.Comments.Count(),
                    Summary = y.Content != null
        ? GenerateSummary(y.Content)
        : ""
                }).ToList();
            return View(blogs);
        }
        private static string GenerateSummary(string htmlContent)
        {
            var plainText = Regex.Replace(htmlContent, "<.*?>", ""); 
            plainText = WebUtility.HtmlDecode(plainText); 

            return plainText.Length > 300
                ? plainText.Substring(0, 300) + "..."
                : plainText;
        }
    }
}
