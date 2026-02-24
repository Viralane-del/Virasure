using Microsoft.AspNetCore.Mvc;

using VirasureYouAI.Context;
using VirasureYouAI.Models;

namespace VirasureYouAI.ViewComponents.AdminLayoutDashboard
{
    public class AdminLayoutDashboardAppUserQuickViewComponent : ViewComponent
    {
        private readonly VirasureContext _context;

        public AdminLayoutDashboardAppUserQuickViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var users = _context.Users
              .Select(user => new UserPolicySummartViewModel
              {
                 UserId = user.Id,
                 FullName = user.Name + " " + user.Surname,
                 PolicyCount = _context.Policies.Count(p => p.AppUserId == user.Id),
                 TotalPremium = _context.Policies
                  .Where(p => p.AppUserId == user.Id)
                  .Sum(p => (decimal?)p.PremiumAmount) ?? 0
              }).OrderByDescending(p => p.PolicyCount).ToList();
            return View(users);
        }
    }
}
