using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;
using VirasureYouAI.Entities;

namespace VirasureYouAI.Controllers
{
    public class SliderController : Controller
    {
        private readonly VirasureContext _context;

        public SliderController(VirasureContext context)
        {
            _context = context;
        }

        public IActionResult SliderList()
        {
            var values = _context.Sliders.ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult CreateSlider()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateSlider(Slider Slider)
        {
            _context.Sliders.Add(Slider);
            _context.SaveChanges();
            return RedirectToAction("SliderList");
        }
        [HttpGet]
        public IActionResult UpdateSlider(int id)
        {
            var value = _context.Sliders.Find(id);
            return View(value);
        }
        [HttpPost]
        public IActionResult UpdateSlider(Slider Slider)
        {
            _context.Sliders.Update(Slider);
            _context.SaveChanges();
            return RedirectToAction("SliderList");
        }
        public IActionResult DeleteSlider(int id)
        {
            var value = _context.Sliders.Find(id);
            _context.Sliders.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("SliderList");
        }
    }
}
