namespace FacultyFeedbackSystem.ViewModels
{
    public class FeedbackReportViewModel
    {
        public string FacultyName { get; set; }
        public string SubjectName { get; set; }
        public int TotalFeedbacks { get; set; }
        public double AverageDiscipline { get; set; }
        public double AverageClarity { get; set; }
        public double AverageTeaching { get; set; }
        public double AverageKnowledge { get; set; }
        public double AveragePunctuality { get; set; }
        public double OverallAverage { get; set; }
        public List<string> Comments { get; set; } = new List<string>();
        public List<string> ImprovementSuggestions { get; set; } = new List<string>();
    }
}
