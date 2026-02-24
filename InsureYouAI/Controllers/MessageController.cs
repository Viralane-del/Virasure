using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;
using VirasureYouAI.Entities;
using VirasureYouAI.Services;

namespace VirasureYouAI.Controllers
{
    public class MessageController : Controller
    {
        private readonly VirasureContext _context;
        private readonly AIService _AIService;

        public MessageController(VirasureContext context, AIService aIService)
        {
            _context = context;
            _AIService = aIService;
        }

        public IActionResult MessageList()
        {
            var values = _context.Messages.ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult CreateMessage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(Message message)
        {
            var combinedtext = $"{message.Subject} - {message.Detail}";
            var predictedCategory = await _AIService.PredictCategoryAsync(combinedtext);
            var priority = await _AIService.PredictPriorityAsync(combinedtext);

            message.Priority = priority;
            message.AICategory = predictedCategory;

            message.SendDate = DateTime.Now;
            message.IsRead = false;
            _context.Messages.Add(message);
            _context.SaveChanges();
            return RedirectToAction("MessageList");
        }
        [HttpGet]
        public IActionResult UpdateMessage(int id)
        {
            var value = _context.Messages.Find(id);
            return View(value);
        }
        [HttpPost]
        public IActionResult UpdateMessage(Message message)
        {
            _context.Messages.Update(message);
            _context.SaveChanges();
            return RedirectToAction("MessageList");
        }
        public IActionResult DeleteMessage(int id)
        {
            var value = _context.Messages.Find(id);
            _context.Messages.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("MessageList");
        }
    }
}
