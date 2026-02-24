using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.AdminLayoutDashboard
{
    public class AdminLayoutDashboardSubChart3ViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;
        public AdminLayoutDashboardSubChart3ViewComponent(VirasureContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            // Sadece içinde bulunulan ayın giderlerini al
            var expenseData = await _context.Expenses
                .Where(e => e.ProcessDate.Month == currentMonth && e.ProcessDate.Year == currentYear)
                .GroupBy(e => e.Detail)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .ToListAsync();

            ViewBag.ExpenseLabels = expenseData.Select(x => x.Category).ToList();
            ViewBag.ExpenseValues = expenseData.Select(x => x.TotalAmount).ToList();

            return View();
        }
    }
}
