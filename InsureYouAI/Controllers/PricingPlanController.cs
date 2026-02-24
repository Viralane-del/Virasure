using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using VirasureYouAI.Context;
using VirasureYouAI.Entities;
using VirasureYouAI.Models;

namespace VirasureYouAI.Controllers
{
    public class PricingPlanController : Controller
    {
        private readonly VirasureContext _context;

        public PricingPlanController(VirasureContext context)
        {
            _context = context;
        }

        public IActionResult PricingPlanList()
        {
            var values = _context.PricingPlans.ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult CreatePricingPlan()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreatePricingPlan(PricingPlan pricingPlan)
        {
            pricingPlan.IsFeature = false;
            _context.PricingPlans.Add(pricingPlan);
            _context.SaveChanges();
            return RedirectToAction("PricingPlanList");
        }
        [HttpGet]
        public IActionResult UpdatePricingPlan(int id)
        {
            var value = _context.PricingPlans.Find(id);
            return View(value);
        }
        [HttpPost]
        public IActionResult UpdatePricingPlan(PricingPlan pricingPlan)
        {
            _context.PricingPlans.Update(pricingPlan);
            _context.SaveChanges();
            return RedirectToAction("PricingPlanList");
        }
        public IActionResult DeletePricingPlan(int id)
        {
            var value = _context.PricingPlans.Find(id);
            _context.PricingPlans.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("PricingPlanList");
        }
        [HttpGet]
        public IActionResult CreateUserCustomizePlan() 
        {
            var model = new AIVirasureRecommendationViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUserCustomizePlan(AIVirasureRecommendationViewModel model)
        {
            string apiKey = "";

            var userJson = JsonConvert.SerializeObject(model);

            var prompt = $@"
Sen profesyonel bir sigorta uzmanı AI asistanısın. 
Aşağıdaki kullanıcının bilgilerini analiz ederek en uygun sigorta paketini öner.

Paketler:
1) Premium (599 TL)
2) Standart (449 TL)
3) Ekonomik (339 TL)

Kullanıcı:
{userJson}

SADECE JSON döndür:

{{
  ""RecommendedPackage"": ""Premium | Standart | Ekonomik"",
  ""SecondBestPackage"": ""Premium | Standart | Ekonomik"",
  ""AnalysisText"": ""Kısa analiz metni""
}}
";

            var httpClient = new HttpClient();

            var requestBody = new
            {
                contents = new[]
                {
            new
            {
                role = "user",
                parts = new[]
                {
                    new { text = prompt }
                }
            }
        }
            };

            var jsonBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var geminiModel = "gemini-2.5-flash";

            var url =
                $"https://generativelanguage.googleapis.com/v1/models/{geminiModel}:generateContent?key={apiKey}";

            var response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API Error: {error}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            dynamic gemini = JsonConvert.DeserializeObject(jsonResponse);

            string aiResult =
                gemini.candidates[0].content.parts[0].text;

            aiResult = aiResult
    .Replace("```json", "")
    .Replace("```", "")
    .Trim();

            var result =
                JsonConvert.DeserializeObject<AIVirasureRecommendationViewModel>(aiResult);

            model.RecommendedPackage = result.RecommendedPackage;
            model.SecondBestPackage = result.SecondBestPackage;
            model.AnalysisText = result.AnalysisText;

            return View(model);
        }

    }
}
