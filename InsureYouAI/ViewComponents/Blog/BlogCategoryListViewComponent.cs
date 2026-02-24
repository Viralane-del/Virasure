using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;
using VirasureYouAI.Models;

namespace VirasureYouAI.ViewComponents.Blog
{
    public class BlogCategoryListViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public BlogCategoryListViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            //var categories = _context.Categories.ToList();
            var categories = _context.Categories.Select(c => new BlogCategoryCountViewModel
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                ArticleCount = c.Articles.Count()
            }).ToList();
            return View(categories);
        }
    }
}
