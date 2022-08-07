using System.Xml.Linq;

public class ReceiptDataService : IReceiptDataService
{
    private readonly IReceiptDataRepository _receiptRepo;

    public ReceiptDataService(IReceiptDataRepository receiptRepo)
    {
        _receiptRepo = receiptRepo;
    }

    public bool ProcessReceipt(XElement receipt)
    {
        var processedData = XmlParser.ProcessReceiptXml(receipt);
        return SaveToDb(processedData);
    }

    public bool SaveToDb(ReceiptData data)
    {
        return _receiptRepo.SaveReceiptData(data);
    }

    public ReceiptData? GetReceiptData(int receiptId)
    {
        return _receiptRepo.GetReceipt(receiptId);
    }

    public List<ReceiptData> GetTodaysReceipts()
    {
        return _receiptRepo.GetReceipts(DateTime.Today.AddDays(-1));
    }
}