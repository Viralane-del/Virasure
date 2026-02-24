using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using VirasureYouAI.Context;
using VirasureYouAI.Entities;

namespace VirasureYouAI.Controllers
{
    public class TestimonialController : Controller
    {
        private readonly VirasureContext _context;

        public TestimonialController(VirasureContext context)
        {
            _context = context;
        }

        public IActionResult TestimonialList()
        {
            var values = _context.Testimonials.ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult CreateTestimonial()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateTestimonial(Testimonial testimonial)
        {
            _context.Testimonials.Add(testimonial);
            _context.SaveChanges();
            return RedirectToAction("TestimonialList");
        }
        [HttpGet]
        public IActionResult UpdateTestimonial(int id)
        {
            var value = _context.Testimonials.Find(id);
            return View(value);
        }
        [HttpPost]
        public IActionResult UpdateTestimonial(Testimonial testimonial)
        {
            _context.Testimonials.Update(testimonial);
            _context.SaveChanges();
            return RedirectToAction("TestimonialList");
        }
        public IActionResult DeleteTestimonial(int id)
        {
            var value = _context.Testimonials.Find(id);
            _context.Testimonials.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("TestimonialList");
        }
        public async Task<IActionResult> CreateTestimonialWithGeminiAI()
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
"Kurumsal sigorta firması için 6 adet müşteri testimonial üret.\n" +
"SADECE aşağıdaki formatta JSON ARRAY döndür.\n" +
"Başka hiçbir açıklama yazma.\n" +
"Markdown kullanma.\n" +
"JSON dışına çıkma.\n\n" +

"Her obje şu alanları içermek zorunda:\n" +
"- NameSurname (string) → Müşteri adı soyadı\n" +
"- Title (string) → Müşteri unvanı (Örn: Yazılım Müdürü, CEO, Finans Uzmanı, Proje Yöneticisi vb.)\n" +
"- CommentDetail (string) → Sigorta firması hakkında müşteri yorumu\n" +
"- ImageUrl (string) → Geçerli bir profil fotoğraf linki (ASLA boş veya null olamaz)\n\n" +

"ImageUrl için bu formatta link üret:\n" +
"https://randomuser.me/api/portraits/men/1.jpg\n" +
"https://randomuser.me/api/portraits/women/1.jpg\n\n" +

"JSON formatı:\n" +

"[\n" +
"{\"NameSurname\":\"...\",\"Title\":\"...\",\"CommentDetail\":\"...\",\"ImageUrl\":\"...\"}\n" +
"]"



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

            List<Testimonial>? testimonial;
            Console.WriteLine(text);
            try
            {
                testimonial = JsonSerializer.Deserialize<List<Testimonial>>(text);
            }
            catch
            {
                ViewBag.Error = "Gemini JSON formatını bozdu.";
                return View();
            }

            ViewBag.Testimonial = testimonial;
            
            return View();
        }
    }
}
