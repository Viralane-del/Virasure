using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VirasureYouAI.Context;
using VirasureYouAI.Models;

namespace VirasureYouAI.ViewComponents.AdminLayoutDashboard
{
    public class AdminLayoutDashboardSubChartViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public AdminLayoutDashboardSubChartViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke() 
        {          
            return View();
        }
    }
}
