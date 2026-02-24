
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using VirasureYouAI.Context;
using VirasureYouAI.Entities;

namespace VirasureYouAI.Controllers
{
    public class ArticleController : Controller
    {
        private readonly VirasureContext _context;

        public ArticleController(VirasureContext context)
        {
            _context = context;
        }

        public IActionResult ArticleList()
        {
            var values = _context.Articles.Include(a => a.AppUser).ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult CreateArticle()
        {
            var categories = _context.Categories
               .Select(c => new SelectListItem
               {
                   Value = c.CategoryId.ToString(),
                   Text = c.CategoryName

               }).ToList();
            ViewBag.Categories = categories;

            var author = _context.Users
               .Select(c => new SelectListItem
               {
                   Value = c.Id.ToString(),
                   Text = c.Name + " " + c.Surname

               }).ToList();
            ViewBag.Author = author;
            return View();
        }
        [HttpPost]
        public IActionResult CreateArticle(Article article)
        {
            article.CreatedDate = DateTime.Now;                
            _context.Articles.Add(article);
            _context.SaveChanges();
            return RedirectToAction("ArticleList");
        }
        [HttpGet]
        public IActionResult UpdateArticle(int id)
        {
            var categories = _context.Categories
              .Select(c => new SelectListItem
              {
                  Value = c.CategoryId.ToString(),
                  Text = c.CategoryName

              }).ToList();
            ViewBag.Categories = categories;

            var author = _context.Users
               .Select(c => new SelectListItem
               {
                   Value = c.Id.ToString(),
                   Text = c.Name + " " + c.Surname

               }).ToList();
            ViewBag.Author = author;
            var value = _context.Articles.Find(id);
            return View(value);
        }
        [HttpPost]
        public IActionResult UpdateArticle(Article article)
        {
            article.CreatedDate = DateTime.Now;
            _context.Articles.Update(article);
            _context.SaveChanges();
            return RedirectToAction("ArticleList");
        }
        public IActionResult DeleteArticle(int id)
        {
            var value = _context.Articles.Find(id);
            _context.Articles.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("ArticleList");
        }
        [HttpGet]
        public IActionResult CreateArticleWithOpenAI()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateArticleWithOpenAI(string prompt)
        {
            var apiKey = "";

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var requestData = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "Sen bir sigorta şirketi için çalışan, içerik yazarlığı  yapan bir yapay zekasın. Kullanıcının verdiği özet ve anahtar kelimelere göre, sigortacılık sektörü ile ilgili makale üret. En az 3000 karakter olsun." },
                    new { role = "user", content = prompt }
                },
                temperature = 0.7
            };
            var response = await httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestData);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
                var content = result.choices[0].message.content;
                ViewBag.article = content;
            }
            else
            {
                ViewBag.article = "Hata oluştu: " + response.StatusCode;
            }
            return View();
        }
        public class OpenAIResponse
        {
            public List<Choice> choices { get; set; }
        }
        public class Choice
        {
            public Message message { get; set; }
        }
        public class Message
        {
            public string role { get; set; }
            public string content { get; set; }
        }
    }
}
//
