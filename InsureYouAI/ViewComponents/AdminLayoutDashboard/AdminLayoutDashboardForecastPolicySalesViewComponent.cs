using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using System;
using System.Data;
using VirasureYouAI.Context;
using VirasureYouAI.Entities;
using VirasureYouAI.Models;

namespace VirasureYouAI.ViewComponents.AdminLayoutDashboard
{
    public class AdminLayoutDashboardForecastPolicySalesViewComponent : ViewComponent
    {
        private readonly VirasureContext _context;

        public AdminLayoutDashboardForecastPolicySalesViewComponent(VirasureContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            // 1) Veri Hazırlığı – Son 6 Ay
            var policies = _context.Policies
                .Where(x => x.CreatedDate >= new DateTime(2025, 09, 29) &&
                x.CreatedDate <= new DateTime(2026, 02, 10))
               .AsNoTracking()
               .ToList();

            var rawData = policies
               .GroupBy(x => x.PolicyType)
               .Select(g => new
               {
                PolicyType = g.Key,
                MonthlyCounts = g
               .GroupBy(z => new { z.CreatedDate.Year, z.CreatedDate.Month })
               .Select(s => new
               {
                   Month = s.Key.Month,
                   Count = s.Count()
               })
                  .OrderBy(s => s.Month)
                  .ToList()
               })
                   .ToList();

            int index = 0;
            var ml = new MLContext();

            List<PolicyForecastViewModel> result = new();

            foreach (var item in rawData)
            {
                // ML.NET için input formatı
                var mlData = item.MonthlyCounts.Select(m => new PolicyMonthlyData
                {
                    MonthIndex = index++,
                    Value = m.Count
                });

                var dataView = ml.Data.LoadFromEnumerable(mlData);

                var pipeline = ml.Forecasting.ForecastBySsa(
                    outputColumnName: "Forecast",
                    inputColumnName: "Value",
                    windowSize: 2,
                    seriesLength: 6,
                    trainSize: 6,
                    horizon: 1);

                var model = pipeline.Fit(dataView);

                var forecastEngine = model.CreateTimeSeriesEngine<PolicyMonthlyData, PolicyForecastOutput>(ml);

                var prediction = forecastEngine.Predict();

                int predicted = (int)prediction.Forecast[0];

                result.Add(new PolicyForecastViewModel
                {
                    PolicyType = item.PolicyType,
                    ForecastCount = predicted
                });
            }
            // 3) Yüzde Hesabı
            int total = result.Sum(x => x.ForecastCount);

            foreach (var item in result)
                item.Percentage = total > 0 ? (item.ForecastCount * 250 / total) : 0;

            return View(result);

        }
    }
}
// ML.NET Veri Modelleri
public class PolicyMonthlyData
{
    public float MonthIndex { get; set; }
    public float Value { get; set; }
}

public class PolicyForecastOutput
{
    public float[] Forecast { get; set; }
}
