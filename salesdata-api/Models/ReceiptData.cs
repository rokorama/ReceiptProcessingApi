namespace salesdata_api.Models;

public class ReceiptData
{
    public Guid Id { get; set; }
    public int ReceiptId { get; set; }
    public int StoreId { get; set; }
    public decimal ItemsSold { get; set; }
    public decimal? TransactionNet { get; set; }
    public DateTime Date { get; set; }
}