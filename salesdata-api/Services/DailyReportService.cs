public class DailyReportService : IDailyReportService
{
    private readonly IDailyReportRepository _reportRepo;

    public DailyReportService(IDailyReportRepository reportRepo)
    {
        _reportRepo = reportRepo;
    }

    public List<DateRangeSearchResult> GetReportsFromPeriod(DateTime startDate, DateTime endDate)
    {
        var searchResults = new List<DateRangeSearchResult>();

        var reportsFromRange = _reportRepo.GetReportsFromPeriod(startDate, endDate);
        var groupedResults = reportsFromRange.GroupBy(r => r.StoreId);
        foreach (var group in groupedResults)
        {
            var calculatedData = new DateRangeSearchResult()
            {
                StoreId = group.Key,
                TransactionNet = group.Sum(g => g.TotalAmount),
                TotalItems = group.Sum(g => g.TotalItems),
                TotalReceipts = group.Sum(g => g.TotalReceipts)
            };

            searchResults.Add(calculatedData);
        }

        return searchResults;
    }

    public List<DailySalesReport> GenerateReportFromReceipts(List<ReceiptData> receipts)
    {
        var groupedData = receipts.GroupBy(r => r.StoreId);
        List<DailySalesReport> result = new List<DailySalesReport>();

        foreach (var group in groupedData)
        {
            var report = new DailySalesReport()
            {
                Id = Guid.NewGuid(),
                StoreId = group.Key,
                TotalItems = group.Sum(x => x.ItemsSold),
                TotalAmount = group.Sum(x => x.TransactionNet),
                TotalReceipts = group.Count(),
                Date = DateTime.Today.AddDays(-1)
            };

            result.Add(report);
        }

        

        return result;
    }
}