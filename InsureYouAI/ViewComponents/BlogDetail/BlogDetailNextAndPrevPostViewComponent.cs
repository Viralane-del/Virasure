using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.BlogDetail
{
    public class BlogDetailNextAndPrevPostViewComponent : ViewComponent
    {
        private readonly VirasureContext _context;

        public BlogDetailNextAndPrevPostViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int id)
        {
            var article = _context.Articles.FirstOrDefault(a => a.ArticleId == id);
            //Önceki Makale Sorgusu
            var prevArticle = _context.Articles.Where(x => x.ArticleId < id).OrderByDescending(x => x.ArticleId)
                .Select(x => x.Title).FirstOrDefault();

            //Sonraki Makale Sorgusu
            var nextArticle = _context.Articles.Where(x => x.ArticleId > id).OrderByDescending(x => x.ArticleId)
                .Select(x => x.Title).FirstOrDefault();


            ViewBag.prevArticleTitle = prevArticle;
            ViewBag.nextArticleTitle = nextArticle;
            return View();
        }
    }
}
