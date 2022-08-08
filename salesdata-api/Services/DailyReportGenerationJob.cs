using Cronos;

namespace salesdata_api.Services;

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
        using IServiceScope scope = _serviceProvider.CreateScope();
        IDailyReportService dailyReportService = scope.ServiceProvider.GetRequiredService<IDailyReportService>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var utcNow = DateTime.UtcNow;
            var nextUtc = _cron.GetNextOccurrence(utcNow);
            await Task.Delay(nextUtc!.Value - utcNow, stoppingToken);
            var reports = dailyReportService.GenerateReportsFromReceipts();
            if (reports is not null)
            {
                dailyReportService.SaveDailyReportsToJson(reports);
                dailyReportService.SaveDailyReportsToDb(reports);
            }
        }
    }


}