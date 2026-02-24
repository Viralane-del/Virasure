using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using VirasureYouAI.Context;
using VirasureYouAI.Entities;

namespace VirasureYouAI.Controllers
{
    public class ServiceController : Controller
    {
        private readonly VirasureContext _context;

        public ServiceController(VirasureContext context)
        {
            _context = context;
        }

        public IActionResult ServiceList()
        {
            var values = _context.Services.ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult CreateService()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateService(Service service)
        {
            _context.Services.Add(service);
            _context.SaveChanges();
            return RedirectToAction("ServiceList");
        }
        [HttpGet]
        public IActionResult UpdateService(int id)
        {
            var value = _context.Services.Find(id);
            return View(value);
        }
        [HttpPost]
        public IActionResult UpdateService(Service service)
        {
            _context.Services.Update(service);
            _context.SaveChanges();
            return RedirectToAction("ServiceList");
        }
        public IActionResult DeleteService(int id)
        {
            var value = _context.Services.Find(id);
            _context.Services.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("ServiceList");
        }
        public async Task<IActionResult> CreateServiceWithGeminiAI()
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
                                text =
                                "Kurumsal bir sigorta firması için güven verici  10  hizmeti yaz. " +
                                "Her biri maksimum 100 karakter olsun. " +
                                "SADECE aşağıdaki formatta JSON ARRAY döndür. " +
                                "Markdown veya açıklama YAZMA.\n\n" +
                                "[\"Hizmet 1\",\"Hizmet 2\",\"Hizmet 3\"]"
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

            Console.WriteLine(responseJson);

            using var jsonDoc = JsonDocument.Parse(responseJson);
            var root = jsonDoc.RootElement;

            // ❌ API ERROR
            if (root.TryGetProperty("error", out var error))
            {
                ViewBag.Error = error.GetProperty("message").GetString();
                return View();
            }

            // ❌ CONTENT KONTROL
            if (!root.TryGetProperty("candidates", out var candidates) ||
                candidates.GetArrayLength() == 0)
            {
                ViewBag.Error = "Gemini içerik üretmedi.";
                return View();
            }

            var text = candidates[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            if (string.IsNullOrEmpty(text))
            {
                ViewBag.Error = "Gemini boş cevap döndürdü.";
                return View();
            }

            // 🔥 MARKDOWN TEMİZLE
            text = text
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            List<string>? services;

            try
            {
                services = JsonSerializer.Deserialize<List<string>>(text);
            }
            catch
            {
                ViewBag.Error = "Gemini JSON formatını bozdu.";
                return View();
            }

            ViewBag.Services = services;
            return View();

        }
    }
}
