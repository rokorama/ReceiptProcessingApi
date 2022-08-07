public interface IDailyReportRepository
{
    public bool SaveReports(List<DailySalesReport> reports);
    public List<DailySalesReport> GetReportsFromPeriod(DateTime startDate, DateTime endDate);
}