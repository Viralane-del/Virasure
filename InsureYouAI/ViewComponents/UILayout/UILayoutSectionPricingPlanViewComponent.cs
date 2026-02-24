using Microsoft.AspNetCore.Mvc;
using VirasureYouAI.Context;

namespace VirasureYouAI.ViewComponents.UILayout
{
    public class UILayoutSectionPricingPlanViewComponent:ViewComponent
    {
        private readonly VirasureContext _context;

        public UILayoutSectionPricingPlanViewComponent(VirasureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var pricingPlan1 = _context.PricingPlans.Where(x => x.IsFeature == true).FirstOrDefault();
            ViewBag.PricingPlan1Title = pricingPlan1.Title;
            ViewBag.PricingPlan1Price = pricingPlan1.Price;
            ViewBag.pricingPlan1Id = pricingPlan1.PricingPlanId;

            var pricingPlan2 = _context.PricingPlans.Where(x => x.IsFeature == true).OrderByDescending(y => y.PricingPlanId).FirstOrDefault();
            ViewBag.PricingPlan2Title = pricingPlan2.Title;
            ViewBag.PricingPlan2Price = pricingPlan2.Price;
            ViewBag.pricingPlan2Id = pricingPlan2.PricingPlanId;

            var pricingPlanItems = _context.PricingPlanItems.Where(x => x.PricingPlanId == pricingPlan1.PricingPlanId || 
            x.PricingPlanId == pricingPlan2.PricingPlanId).ToList();
            return View(pricingPlanItems);
        }
    }
}
