using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.AdminLayoutDashboard
{
    public class AdminLayoutDashboardSubChart2ViewComponent : ViewComponent
    {
        private readonly VirasureContext _context;
        public AdminLayoutDashboardSubChart2ViewComponent(VirasureContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentYear = DateTime.Now.Year;

            var monthlyData = await _context.Policies
                .Where(p => p.StartDate.Year == currentYear)
                .GroupBy(p => p.StartDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalPremium = g.Sum(x => x.PremiumAmount)
                })
                .ToListAsync();

            // 12 aylık dizi (boş ayları 0 olarak gösterecek)
            decimal[] revenues = new decimal[12];
            foreach (var item in monthlyData)
            {
                revenues[item.Month - 1] = item.TotalPremium;
            }

            ViewBag.MonthlyRevenues = revenues;

            return View();
        }
    }
}
