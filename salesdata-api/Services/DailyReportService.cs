using Newtonsoft.Json;
using salesdata_api.Models;
using salesdata_api.Repositories;

namespace salesdata_api.Services;

public class DailyReportService : IDailyReportService
{
    private readonly IDailyReportRepository _reportRepo;
    private readonly IReceiptDataRepository _receiptRepo;

    public DailyReportService(IDailyReportRepository reportRepo, IReceiptDataRepository receiptRepo)
    {
        _reportRepo = reportRepo;
        _receiptRepo = receiptRepo;
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

    public List<DailySalesReport> GenerateReportsFromReceipts()
    {
        var receipts = _receiptRepo.GetReceipts(DateTime.Today.AddDays(-1));
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

    public void SaveDailyReportsToJson(List<DailySalesReport> reports)
    {
        var date = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
        using StreamWriter file = File.AppendText($"../SampleOutput/SalesReport_{date}.json");
        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(file, reports);
    }

    public bool SaveDailyReportsToDb(List<DailySalesReport> reports)
    {
        return _reportRepo.SaveReports(reports);
    }

}