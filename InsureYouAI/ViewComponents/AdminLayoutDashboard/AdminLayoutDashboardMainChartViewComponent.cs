using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;
using VirasureYouAI.Models;

namespace VirasureYouAI.ViewComponents.AdminLayoutDashboard
{
    public class AdminLayoutDashboardMainChartViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public AdminLayoutDashboardMainChartViewComponent(VirasureContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var culture = new System.Globalization.CultureInfo("tr-TR");

            //revenue
            var Revenuedata = _context.Revenues
                .GroupBy(x => x.ProcessDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Total = g.Sum(x => x.Amount)
                })
                .OrderBy(x => x.Month)
                .ToList();

            //expense
            var Expensedata = _context.Expenses
                .GroupBy(x => x.ProcessDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Total = g.Sum(x => x.Amount)
                }).OrderBy(x => x.Month).ToList();

            //Tüm Ayları Birleştirme
            var allMonths = Revenuedata.Select(x => x.Month).Union(Expensedata.Select(y => y.Month))
                .OrderBy(x => x).ToList();


            var model = new RevenueExpenseChartViewModel()
            {
                Months = allMonths.Select(x => new System.Globalization.DateTimeFormatInfo().GetAbbreviatedMonthName(x)).ToList(),
                RevenueTotals = allMonths.Select(m => Revenuedata.FirstOrDefault(x => x.Month == m)?.Total ?? 0).ToList(),
                ExpenseTotals = allMonths.Select(m => Expensedata.FirstOrDefault(x => x.Month == m)?.Total ?? 0).ToList()
            };

            ViewBag.v1 = Revenuedata.Sum(x => x.Total);
            ViewBag.v2 = Expensedata.Sum(x => x.Total);

            return View(model);
        }

    }
}
