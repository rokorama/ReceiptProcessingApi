using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReceiptData>()
            .HasKey(o => o.Id);
    }

    public DbSet<DailySalesReport> DailySalesReports => Set<DailySalesReport>();
    public DbSet<ReceiptData> ReceiptData => Set<ReceiptData>();
}