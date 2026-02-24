using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VirasureYouAI.Context;
using VirasureYouAI.Entities;

namespace VirasureYouAI.Controllers
{
    public class AppUserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly VirasureContext _context;

        public AppUserController(UserManager<AppUser> userManager, VirasureContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult UserList()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }
        public async Task<IActionResult> UserProfilWithAI(string id) 
        {
            var profile = await _userManager.FindByIdAsync(id);
            ViewBag.name = profile.Name;
            ViewBag.surname = profile.Surname;
            ViewBag.image = profile.ImageURL;
            ViewBag.about = profile.Description;
            ViewBag.titlevalue = profile.Title;
            ViewBag.education = profile.Education;
            ViewBag.city = profile.City;

            //Kullanıcı Verisi Çekme
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
                return NotFound();

            //Kullanıcıya ait makaleleri çekme
            var articles =await _context.Articles.Where(a => a.AppUserId == id).Select(c => c.Content).ToListAsync();
            if (articles.Count == 0) 
            {
                ViewBag.ArticleResult = "Bu kullanıcıya ait analiz makale bulunmamaktadır.";
                return View(user);
            }

            //Makale içeriklerini birleştirme
            var allArticle = string.Join("\n\n", articles);

            var apiKey = "";

            //Prompt Oluşturma
            var prompt = $@"Sen bir sigorta sektöründe uzman bir içerik analistisin. " +
                $"Elinizde, bir sigorta şirketinin çalışanının yazdığı tüm makaleler var." +
                $"Bu makaleler üzerinden çalışanın içerik üretim tarzını analiz et." +
                $"Analiz Başlıkları:" +
                $"1 - Konu çeşitliliği ve odak alanları (sağlık, hayat, kasko ,tamamlayıcı , BES vb.)" +
                $"2 - Hedef Kitle tahmini (bireysel/kurumsal, segment, persona)" +
                $"3 - Dil ve Anlatım Tarzı (tekniklik seviyesi, okunabilirlik ve ikna gücü)" +
                $"4 - Sigorta terimlerini kullanma ve doğruluk düzeyi" +
                $"5 - Müşteri ihtiyaçlarına ve risk yönetimine odaklanma" +
                $"6 - Pazarlama/satış vurgusu, CTA netliği" +
                $"7 - Geliştirilmesi gereken alanlar ve net aksiyon maddeleri " +
                $"Makaleler:" +
                $"{allArticle}" +
                $"Lütfen çıktıyı profesyonel rapor formatında, madde madde ve en sonda 5 maddelik aksiyon listesi ile ver.";


            //OpenAI Chat Completions
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var body = new
            {
                model = "gpt-4o-mini",
                messages = new object[]
                {
                    new {role = "system", content = "Sen bir sigorta sektöründe içerik analizi yapan bir uzmansın" },
                    new {role = "user", content = prompt }
                },
                max_tokens = 1000,
                temperature = 0.2
            };

            //Json Dönüştürme
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var respText = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode) 
            {
                ViewBag.ArticleResult = "Open AI Hatası: " + httpResponse.StatusCode;
                return View(user);
            }

            //Json Yapısı İçinden Veriyi Okuma
            try
            {
                using var doc = JsonDocument.Parse(respText);
                var aitText = doc.RootElement.GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();
                ViewBag.ArticleResult = aitText ?? "Boş yanıt döndü";
            }
            catch
            {
                ViewBag.ArticleResult = "OpenAI yanıtı beklenen formatta değil.";
            }
            return View(user);
        }
        public async Task<IActionResult> UserCommentWithAI(string id) 
        {
            var profile = await _userManager.FindByIdAsync(id);
            ViewBag.name = profile.Name;
            ViewBag.surname = profile.Surname;
            ViewBag.image = profile.ImageURL;
            ViewBag.about = profile.Description;
            ViewBag.titlevalue = profile.Title;
            ViewBag.education = profile.Education;
            ViewBag.city = profile.City;

            //Kullanıcı Verisi Çekme
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            //Kullanıcıya ait makaleleri çekme
            var comments = await _context.Comments.Where(a => a.AppUserId == id).Select(c => c.CommentDetail).ToListAsync();
            if (comments.Count == 0)
            {
                ViewBag.CommentResult = "Bu kullanıcıya ait analiz yorumu bulunmamaktadır.";
                return View(user);
            }

            //Makale içeriklerini birleştirme
            var allComments = string.Join("\n\n", comments);

            var apiKey = "";

            //Prompt Oluşturma
            var prompt = $@"Sen kullanıcı davranış analizi yapan bir yapay zeka uzmanısın.
                Aşağıda ki yorumlara göre kullanıcı değerlendir." +
                $"Analiz Başlıkları:" +
                $"1 - Genel Duygu Durumu (pozitif,negatif ve nötr)" +
                $"2 - Toksik içerik var mı? (örnekleriyle)" +
                $"3 - İletişim Tarzı (samimi, resmi , dürüst ve agresif vb.)" +
                $"4 - İlgi Alanları / Konu Başlıkları" +
                $"5 - Geliştirilmesi gereken iletişim alanları" +
                $"6 - 5 Maddelik Kısa Özet" +
                $"Yorumlar:" +
                $"{allComments}";



            //OpenAI Chat Completions
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var body = new
            {
                model = "gpt-4o-mini",
                messages = new object[]
                {
                    new {role = "system", content = "Sen bir sigorta sektöründe içerik analizi yapan bir uzmansın" },
                    new {role = "user", content = prompt }
                },
                max_tokens = 1000,
                temperature = 0.2
            };

            //Json Dönüştürme
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var respText = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                ViewBag.CommentResult = "Open AI Hatası: " + httpResponse.StatusCode;
                return View(user);
            }

            //Json Yapısı İçinden Veriyi Okuma
            try
            {
                using var doc = JsonDocument.Parse(respText);
                var aitText = doc.RootElement.GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();
                ViewBag.CommentResult = aitText ?? "Boş yanıt döndü";
            }
            catch
            {
                ViewBag.CommentResult = "OpenAI yanıtı beklenen formatta değil.";
            }
            return View(user);
           
        }
    }
}
