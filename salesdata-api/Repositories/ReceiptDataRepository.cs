using salesdata_api.Models;

namespace salesdata_api.Repositories;

public class ReceiptDataRepository : IReceiptDataRepository
{
    private readonly AppDbContext _dbContext;

    public ReceiptDataRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ReceiptData? GetReceipt(int receiptNumber)
    {
        return _dbContext.ReceiptData.FirstOrDefault(x => x.ReceiptId == receiptNumber);
    }

    public List<ReceiptData> GetReceipts(DateTime date)
    {
        return _dbContext.ReceiptData.ToList();
    }

    public bool SaveDailyReport(DailySalesReport report)
    {
        _dbContext.DailySalesReports.Add(report);
        return _dbContext.SaveChanges() > 0;
    }

    public bool SaveReceiptData(ReceiptData receiptData)
    {
        _dbContext.ReceiptData.Add(receiptData);
        return _dbContext.SaveChanges() > 0;
    }
}