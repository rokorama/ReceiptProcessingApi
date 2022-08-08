using System.Xml.Linq;
using salesdata_api.Models;

namespace salesdata_api.Helpers;

public static class XmlParser
{
    public static ReceiptData ProcessReceiptXml(XElement xmlDocument)
    {
        var descs  = xmlDocument.Descendants();
        var receiptNo = Convert.ToInt32(descs.FirstOrDefault(x => x.Name.LocalName =="SequenceNumber")?.Value);
        var itemsSold = Convert.ToDecimal(descs.FirstOrDefault(x => x.Name.LocalName =="Quantity")?.Value);
        var transactionAmount = Convert.ToDecimal(descs.FirstOrDefault(x => (string) x.Attribute("TotalType")! == "TransactionNetAmount")?.Value);
        var storeId = Convert.ToInt32(descs.FirstOrDefault(x => x.Name.LocalName =="UnitID")!.Value);

        return new ReceiptData()
        {
            ReceiptId = receiptNo,
            StoreId = storeId,
            ItemsSold = itemsSold,
            TransactionNet = transactionAmount,
            Date = DateTime.Today
        };
    }
}