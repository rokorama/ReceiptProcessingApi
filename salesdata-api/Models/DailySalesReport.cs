public class DailySalesReport
{
    public Guid Id { get; set; }
    public int StoreId { get; set; }
    public decimal TotalItems { get; set; }
    public decimal? TotalAmount { get; set; }
    public int TotalReceipts { get; set; }
    public DateTime Date { get; set; }
}