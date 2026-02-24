using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;
using VirasureYouAI.Models;

namespace VirasureYouAI.ViewComponents.AdminLayoutDashboard
{
    public class AdminLayoutPolicyTypesViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public AdminLayoutPolicyTypesViewComponent(VirasureContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke() 
        {
            var result = _context.Policies
                .GroupBy(x => x.PolicyType)
                .Select(g => new PolicyGroupViewModel
                {
                    PolicyType = g.Key,
                    Count = g.Count()
                }).ToList();
            ViewBag.TotalPolicyCount = result.Sum(x =>x.Count);
            return View(result);
        }
    }
}
