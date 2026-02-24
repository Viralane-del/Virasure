namespace VirasureYouAI.Models
{
    public class AIVirasureRecommendationViewModel
    {
        //Kullanıcı Alanları
        public int? Age { get; set; }
        public string? Occupation { get; set; }
        public string? City { get; set; }
        public string? MarialStatus { get; set; }
        public int? ChildrenCount { get; set; }
        public string? TravelFrequency { get; set; }
        public decimal? MonthlyBudget { get; set; }
        public bool HasChronicDisease { get; set; }
        public string? ChronicDiseaseDetails { get; set; }
        public string? CoveragePriority { get; set; }

        //AI Çıktısı Alanı
        public string? RecommendedPackage { get; set; }
        public string? SecondBestPackage { get; set; }
        public string? AnalysisText { get; set; }
    }
}
