using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VirasureYouAI.Context;
using VirasureYouAI.Models;

namespace VirasureYouAI.ViewComponents.AdminLayoutDashboard
{
    public class AdminLayoutDashboardSubChart1ViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public AdminLayoutDashboardSubChart1ViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var policiyData = _context.Policies
                 .GroupBy(p => p.PolicyType)
                 .Select(g => new
                 {
                     PolicyType = g.Key,
                     Count = g.Count()
                 }).ToList();

            ViewBag.policyData = JsonConvert.SerializeObject(policiyData);

            return View();
        }
    }
}
