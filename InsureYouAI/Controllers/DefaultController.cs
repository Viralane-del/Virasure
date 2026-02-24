
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Org.BouncyCastle.Tls;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using VirasureYouAI.Context;
using VirasureYouAI.Entities;

namespace VirasureYouAI.Controllers
{    
    public class DefaultController : Controller
    {
        private readonly VirasureContext _context;

        public DefaultController(VirasureContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public PartialViewResult SendMessage() 
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(Message message) 
        {
            message.IsRead = false;
            message.SendDate = DateTime.Now;
            _context.Messages.Add(message);
            _context.SaveChanges();
          
            #region Claude_AI_Analiz
            string apiKey = "";
            string prompt = $"Sen bir sigorta firmasının müşteri iletişim asistanısın.\r\n\r\nKurumsal ama samimi, net ve anlaşılır bir dille yaz.\r\n\r\nYanıtlarını 2–3 paragrafla sınırla.\r\n\r\nEksik bilgi (poliçe numarası, kimlik vb.) varsa kibarca talep et.\r\n\r\nFiyat, ödeme, teminat gibi kritik konularda kesin rakam verme, müşteri temsilcisine yönlendir.\r\n\r\nHasar ve sağlık gibi hassas durumlarda empati kur.\r\n\r\nCevaplarını teşekkür ve iyi dilekle bitir.\r\n\r\n Kullanıcının sana gönderdiği mesaj şu şekilde: {message.Detail}.";

            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.anthropic.com/");
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
            client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var requestBody = new
            {
                model = "claude-3-haiku-20240307",
                max_tokens = 300,
                temperature = 0.4,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("v1/messages", jsonContent);
            var responseString = await response.Content.ReadAsStringAsync();

            var json = JsonNode.Parse(responseString);
            string textContent = "Mesajınız alınmıştır, en kısa sürede dönüş yapılacaktır.";

            if (json?["content"] is JsonArray contentArray && contentArray.Count > 0)
            {
                textContent = contentArray[0]?["text"]?.ToString() ?? textContent;
            }
            //ViewBag.Text = textContent; 

            #endregion
            #region Email Gönderme
            MimeMessage mimeMessage = new MimeMessage();
            MailboxAddress mailboxAddressFrom = new MailboxAddress("Virasure Admin", "emirkale03@gmail.com");
            mimeMessage.From.Add(mailboxAddressFrom);

            MailboxAddress mailboxAddressTo = new MailboxAddress("User", message.Email);
            mimeMessage.To.Add(mailboxAddressTo);

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = textContent;
            bodyBuilder.HtmlBody = $@"
              <p>Merhaba,</p>
              <p>{textContent.Replace("\n", "<br>")}</p>
              <p><b>Virasure</b></p>";
            mimeMessage.Subject = "Virasure Email Yanıtı";
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Connect("smtp.gmail.com", 587, false);
            smtpClient.Authenticate("emirkale03@gmail.com", "xypr dwga nesi gqet");
            smtpClient.Send(mimeMessage);
            smtpClient.Disconnect(true);
            #endregion
            #region ClaudeAI Mesaj Kaydı
            ClaudeAIMessage claudeAIMessage = new ClaudeAIMessage()
            {
                Detail = textContent,
                ReceiveEmail = message.Email,
                ReceiveNameSurname = message.NameSurname,
                SendDate = DateTime.Now,
            };
            _context.ClaudeAIMessages.Add(claudeAIMessage);
            _context.SaveChanges();
            #endregion
            return RedirectToAction("Index");
        }
        public PartialViewResult SubscribeEmail()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult SubscribeEmail(string message)
        {
            return View();
        }
        public IActionResult Support() 
        {
            return View();
        }
    }
}
