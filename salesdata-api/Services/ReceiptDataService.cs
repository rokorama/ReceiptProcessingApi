using System.Xml.Linq;
using salesdata_api.Helpers;
using salesdata_api.Models;
using salesdata_api.Repositories;

namespace salesdata_api.Services;

public class ReceiptDataService : IReceiptDataService
{
    private readonly IReceiptDataRepository _receiptRepo;

    public ReceiptDataService(IReceiptDataRepository receiptRepo)
    {
        _receiptRepo = receiptRepo;
    }

    public bool ProcessReceipt(XElement receipt)
    {
        if (!ValidateNamespace(receipt))
            return false;
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
    
    public bool ValidateNamespace(XElement receipt)
    {
        return receipt.Name.LocalName == "POSLog" ? true : false;
    }
}