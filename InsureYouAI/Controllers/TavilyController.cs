using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace VirasureYouAI.Controllers
{
   
    public class TavilyController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string tavilyApiKey = "";
        private readonly string geminiAIApiKey = "";

        public TavilyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private async Task<string> CallTavilyAsync(string query)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://api.tavily.com/");

            var requestBody = new
            {
                api_key = tavilyApiKey,
                query = query,
                include_answer = true,
                max_results = 5
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("search", content);

            return await response.Content.ReadAsStringAsync();
        }
        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SearchWithTavily(string query)
        {

            if (string.IsNullOrEmpty(query))
            {
                ViewBag.Error = "Lütfen bir arama sorgusu girin.";
                return View("Index");
            }

            // 1) Tavily web araması
            var tavilyResponse = await CallTavilyAsync(query);

            // 2) Gemini ile sonuçların analiz edilmesi
            var geminiAIResponse = await SummarizeWithGemini(query, tavilyResponse);

            ViewBag.Query = query;
            ViewBag.TavilyRaw = tavilyResponse;
            ViewBag.geminiAIResult = geminiAIResponse;

            return View("Search");
        }

        private async Task<string> SummarizeWithGemini(string query, string tavilyJson)
        {
            var client = _httpClientFactory.CreateClient();

            var prompt = $@"
Kullanıcının sorusu: {query}

Aşağıdaki Tavily web araması sonuçlarını oku ve kullanıcıya kısa, net ve akademik bir açıklama yap.
Önemli noktaları sade şekilde özetle. Gereksiz cümle kurma.

Tavily sonuçları:
{tavilyJson}
";

            var requestBody = new
            {
                contents = new[]
                {
            new
            {
                parts = new[]
                {
                    new { text = prompt }
                }
            }
        }
            };

            var json = JsonConvert.SerializeObject(requestBody);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={geminiAIApiKey}";

            var response = await client.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(responseString);

            return result?.candidates?[0]?.content?.parts?[0]?.text
                   ?? "Gemini yanıt üretemedi.";
        }
    }
}
