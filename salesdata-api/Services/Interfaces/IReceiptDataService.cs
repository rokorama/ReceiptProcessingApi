using System.Xml.Linq;
using salesdata_api.Models;

namespace salesdata_api.Services;

public interface IReceiptDataService
{
    public bool ProcessReceipt(XElement receipt);
    public ReceiptData? GetReceiptData(int receiptId);
    public List<ReceiptData> GetTodaysReceipts();
}