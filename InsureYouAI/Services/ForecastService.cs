using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

namespace VirasureYouAI.Services
{
    public class PoliciySalesData 
    {
        public DateTime Date { get; set; }
        public float SalesCount { get; set; }
    }
    public class PolicySalesForecast 
    {
        public float[] ForecastedValues { get; set; }
        public float[] LowerBoundValues { get; set; }
        public float[] UpperBoundValues { get; set; }
    }
    public class ForecastService
    {
        private readonly MLContext _mlContext;

        public ForecastService()
        {
            _mlContext = new MLContext();
        }
        public PolicySalesForecast GetForecast(List<PoliciySalesData> salesData, int horizon = 3) 
        {
            var dataView = _mlContext.Data.LoadFromEnumerable(salesData);
            var forecastingPipeline = _mlContext.Forecasting.ForecastBySsa(
                outputColumnName: "ForecastedValues",
                inputColumnName: "SalesCount",
                windowSize: 3,
                seriesLength: salesData.Count,
                trainSize: salesData.Count,
                horizon: horizon,
                confidenceLevel: 0.95f,
                confidenceLowerBoundColumn: "LowerBoundValues",
                confidenceUpperBoundColumn: "UpperBoundValues");

            var model = forecastingPipeline.Fit(dataView);
            var forecastingEngine = model.CreateTimeSeriesEngine<PoliciySalesData, PolicySalesForecast>(_mlContext);
            return forecastingEngine.Predict();
        }
    }
}
