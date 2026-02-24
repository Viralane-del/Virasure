using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.AdminLayoutDashboard
{
    public class AdminLayoutDashboardSubWidgetsViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public AdminLayoutDashboardSubWidgetsViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke() 
        {
            ViewBag.totalMessagesCount = _context.Messages.Count();
            ViewBag.totalArticleCount = _context.Articles.Where(x => x.CategoryId == 2).Count();
            ViewBag.totalPoliciesCount = _context.Policies.Count();


            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var startOfNextMonth = startOfMonth.AddMonths(1);
            ViewBag.totalPoliciesByThisMonthCount = _context.Policies.Where(x => x.CreatedDate >= startOfMonth && x.CreatedDate <= startOfNextMonth).Count();


            ViewBag.totalToxicCommentCount = _context.Comments.Where(x => x.Status == "Toksik Yorum").Count();
            ViewBag.totalApprovedCommentCount = _context.Comments.Where(x => x.Status == "Yorum Onaylandı").Count();
            ViewBag.LastRevenueAmount = _context.Revenues.OrderByDescending(x => x.RevenueID).Take(1).Select(a => a.Amount).FirstOrDefault();
            ViewBag.totalPricingPlanCount = _context.PricingPlans.Count();
            return View();
        }
    }
}
