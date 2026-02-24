using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using VirasureYouAI.Context;
using VirasureYouAI.Entities;

namespace VirasureYouAI.Controllers
{
    public class AboutItemController : Controller
    {
        private readonly VirasureContext _context;

        public AboutItemController(VirasureContext context)
        {
            _context = context;
        }

        public IActionResult AboutItemList()
        {
            var values = _context.AboutItems.ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult CreateAboutItem()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateAboutItem(AboutItem aboutItem)
        {
            _context.AboutItems.Add(aboutItem);
            _context.SaveChanges();
            return RedirectToAction("AboutItemList");
        }
        [HttpGet]
        public IActionResult UpdateAboutItem(int id)
        {
            var value = _context.AboutItems.Find(id);
            return View(value);
        }
        [HttpPost]
        public IActionResult UpdateAboutItem(AboutItem aboutItem)
        {
            _context.AboutItems.Update(aboutItem);
            _context.SaveChanges();
            return RedirectToAction("AboutItemList");
        }
        public IActionResult DeleteAboutItem(int id)
        {
            var value = _context.AboutItems.Find(id);
            _context.AboutItems.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("AboutItemList");
        }
        [HttpGet]
        public async Task<IActionResult> CreateAboutItemWithGeminiAI()
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
                        text = "Kurumsal bir sigorta firması için etkileyici, güven verici " +
                        "ve profesyonel bir 'Hakkımızda alanları'(about item) yazısı oluştur. " +
                        "Örneğin: 'Geleceğinizi güvence altına alan kapsamlı sigorta çözümleri sunuyoruz.' " +
                        "şeklinde veya bunun gibi ve buna benzer daha zengin içerikler gelsin. " +
                        "En az 10 tane item istiyorum. Her madde başlangıcının başına sayı koy belirt."
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

            // 🔎 LOG 
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
