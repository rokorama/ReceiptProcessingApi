using salesdata_api.Models;

namespace salesdata_api.Services;

public interface IDailyReportService
{
    public List<DateRangeSearchResult> GetReportsFromPeriod(DateTime startDate, DateTime endDate);
    public List<DailySalesReport> GenerateReportsFromReceipts();
    public void SaveDailyReportsToJson(List<DailySalesReport> reports);
    public bool SaveDailyReportsToDb(List<DailySalesReport> reports);
}