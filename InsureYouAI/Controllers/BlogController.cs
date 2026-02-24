using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VirasureYouAI.Context;
using VirasureYouAI.Entities;

namespace VirasureYouAI.Controllers
{
    public class BlogController : Controller
    {
        private readonly VirasureContext _context;

        public BlogController(VirasureContext context)
        {
            _context = context;
        }

        public IActionResult BlogList()
        {
            return View();
        }
        public IActionResult GetBlogByCategories(int id) 
        {
            ViewBag.c = id;
            return View();
        }
        public IActionResult BlogDetail(int id)
        {
            ViewBag.BlogId = id;
            return View();
        }

        public PartialViewResult BlogSearch() 
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult BlogSearch(string keyword)
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult AddComment() 
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            comment.CommentDate = DateTime.Now;
            comment.AppUserId = "";

            var client = new HttpClient();

            var apiKey = "";
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                   new { role = "system", content = "Translate the following Turkish text to English. Only return the translation." },
                   new { role = "user", content = comment.CommentDetail }
                },
                temperature = 0
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(responseString);

            var englishText = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();


            var toxicRequestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content =  @"You are a strict content moderator.

Classify the comment as Toxic ONLY if it clearly contains:
- direct insults
- hate speech
- profanity
- threats
- harassment

Negative opinions, complaints, or criticism WITHOUT insults are NOT toxic.

Reply with EXACTLY one word:
Toxic
Normal"

                    },
                    new
                    {
                        role = "user",
                        content = englishText
                    }
                },
                temperature = 0
            };

            var toxicJson = JsonSerializer.Serialize(toxicRequestBody);
            var toxicContent = new StringContent(toxicJson, Encoding.UTF8, "application/json");
            var toxicResponse = await client.PostAsync("https://api.openai.com/v1/chat/completions", toxicContent);
            if (!toxicResponse.IsSuccessStatusCode)
                throw new Exception(await toxicResponse.Content.ReadAsStringAsync());

            var toxicResponseString = await toxicResponse.Content.ReadAsStringAsync();
            using var toxicDoc = JsonDocument.Parse(toxicResponseString);

            var toxicityStatus = toxicDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString()                
                ?.Trim()
                .ToLowerInvariant();

            comment.CommentDetail = englishText;
            comment.Status = "Onay Bekliyor";

            if (toxicityStatus == "toxic")
            {
                comment.Status = "Toksik Yorum";
            }
            else if (toxicityStatus == "normal")
            {
                comment.Status = "Yorum Onaylandı";
            }
            else
            {
                comment.Status = "Onay Bekliyor";
            }

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("BlogList");
        }
    }
}
