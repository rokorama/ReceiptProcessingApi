using System.Xml.Linq;

public interface IReceiptDataService
{
    public bool ProcessReceipt(XElement receipt);
    public ReceiptData? GetReceiptData(int receiptId);
    public List<ReceiptData> GetTodaysReceipts();
}