
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using VirasureYouAI.Context;
using VirasureYouAI.Entities;

namespace VirasureYouAI.Controllers
{
    public class AboutController : Controller
    {
        private readonly VirasureContext _context;

        public AboutController(VirasureContext context)
        {
            _context = context;
        }

        public IActionResult AboutList()
        {
            var values = _context.Abouts.ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult CreateAbout()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateAbout(About About)
        {
            _context.Abouts.Add(About);
            _context.SaveChanges();
            return RedirectToAction("AboutList");
        }
        [HttpGet]
        public IActionResult UpdateAbout(int id)
        {
            var value = _context.Abouts.Find(id);
            return View(value);
        }
        [HttpPost]
        public IActionResult UpdateAbout(About About)
        {
            _context.Abouts.Update(About);
            _context.SaveChanges();
            return RedirectToAction("AboutList");
        }
        public IActionResult DeleteAbout(int id)
        {
            var value = _context.Abouts.Find(id);
            _context.Abouts.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("AboutList");
        }
        [HttpGet]
        public async Task<IActionResult> CreateAboutWithGeminiAI()
        {

            var apiKey = "";
            var model = "gemini-2.5-flash";
            var url = $"https://generativelanguage.googleapis.com/v1/models/{model}:generateContent?key={apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
            new
            {
                parts = new[]
                {
                    new
                    {
                        text = "Kurumsal bir sigorta firması için etkileyici, güven verici ve profesyonel bir 'Hakkımızda' yazısı oluştur."
                    }
                }
            }
        }
            };

            using var httpClient = new HttpClient();
            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync(url, content);
            var responseJson = await response.Content.ReadAsStringAsync();

            // 🔎 LOG (çok önemli)
            Console.WriteLine(responseJson);

            using var jsonDoc = JsonDocument.Parse(responseJson);
            var root = jsonDoc.RootElement;

            // ❗ ERROR KONTROLÜ
            if (root.TryGetProperty("error", out var error))
            {
                ViewBag.value = $"Gemini API Hatası: {error.GetProperty("message").GetString()}";
                return View();
            }

            // ❗ CANDIDATES KONTROLÜ
            if (!root.TryGetProperty("candidates", out var candidates) ||
                candidates.GetArrayLength() == 0)
            {
                ViewBag.value = "Gemini API içerik üretmedi.";
                return View();
            }

            var candidate = candidates[0];

            if (!candidate.TryGetProperty("content", out var contentObj) ||
                !contentObj.TryGetProperty("parts", out var parts) ||
                parts.GetArrayLength() == 0 ||
                !parts[0].TryGetProperty("text", out var textElement))
            {
                ViewBag.value = "Gemini API beklenen formatta cevap döndürmedi.";
                return View();
            }

            ViewBag.value = textElement.GetString();
            return View();
        }

    }
}
