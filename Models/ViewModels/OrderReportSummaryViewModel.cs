namespace LogiDispatchWeb.Models.ViewModels
{
    public class OrderReportSummaryViewModel
    {
        public string Customer { get; set; } = string.Empty;
        public Dictionary<string, int> Intervals { get; set; } = new();
    }
}
