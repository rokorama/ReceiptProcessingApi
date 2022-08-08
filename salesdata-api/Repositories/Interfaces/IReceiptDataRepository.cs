using salesdata_api.Models;

namespace salesdata_api.Repositories;

public interface IReceiptDataRepository
{
    public bool SaveDailyReport(DailySalesReport report);
    public bool SaveReceiptData(ReceiptData receiptData);
    public List<ReceiptData> GetReceipts(DateTime date);
    public ReceiptData? GetReceipt(int receiptNumber);
}