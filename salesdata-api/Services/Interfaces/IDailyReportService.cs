public interface IDailyReportService
{
    public List<DateRangeSearchResult> GetReportsFromPeriod(DateTime startDate, DateTime endDate);
    public List<DailySalesReport> GenerateReportFromReceipts(List<ReceiptData> receipts);
}