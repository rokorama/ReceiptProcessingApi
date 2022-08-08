using salesdata_api.Models;

namespace salesdata_api.Repositories;

public class DailyReportRepository : IDailyReportRepository
{
    private readonly AppDbContext _dbContext;

    public DailyReportRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<DailySalesReport> GetReportsFromPeriod(DateTime startDate, DateTime endDate)
    {
        return _dbContext.DailySalesReports.Where(r => r.Date >= startDate && r.Date <= endDate).ToList();
    }

    public bool SaveReports(List<DailySalesReport> reports)
    {
        foreach (var report in reports)
        {
            _dbContext.DailySalesReports.Add(report);
        }

        var savedEntries = _dbContext.SaveChanges();
        return (savedEntries == reports.Count);
    }
}