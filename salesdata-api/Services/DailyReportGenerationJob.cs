using Cronos;
using Newtonsoft.Json;

public class DailyReportGenerationJob : BackgroundService
{
    private const string schedule = "0 1 * * *"; // every day 1:00
    private readonly IServiceProvider _serviceProvider;
    private readonly CronExpression _cron;

    public DailyReportGenerationJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _cron = CronExpression.Parse(schedule);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var utcNow = DateTime.UtcNow;
            var nextUtc = _cron.GetNextOccurrence(utcNow);
            await Task.Delay(nextUtc!.Value - utcNow, stoppingToken);
            var reports = GenerateDailyReports();
            if (reports is not null)
            {
                SaveDailyReportsToJson(reports);
                SaveDailyReportsToDb(reports);
            }
        }
    }

    public List<DailySalesReport> GenerateDailyReports()
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        IReceiptDataService receiptService = scope.ServiceProvider.GetRequiredService<IReceiptDataService>();
        IDailyReportService dailyReportService = scope.ServiceProvider.GetRequiredService<IDailyReportService>();

        var accumulatedData = receiptService.GetTodaysReceipts();
        return dailyReportService.GenerateReportFromReceipts(accumulatedData);
    }

    public void SaveDailyReportsToJson(List<DailySalesReport> reports)
    {
        var date = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
        using StreamWriter file = File.AppendText($"../SampleOutput/SalesReport_{date}.json");
        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(file, reports);
    }

    public void SaveDailyReportsToDb(List<DailySalesReport> reports)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        IDailyReportRepository dailyReportRepo = scope.ServiceProvider.GetRequiredService<IDailyReportRepository>();

        dailyReportRepo.SaveReports(reports);
    }
}