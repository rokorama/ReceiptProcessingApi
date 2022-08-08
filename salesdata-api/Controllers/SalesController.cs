using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using salesdata_api.Models;
using salesdata_api.Services;

namespace salesdata_api.Controllers;

[ApiController]
[Route("[controller]")]
public class SalesController : ControllerBase
{
    private readonly ILogger<SalesController> _logger;
    private readonly IReceiptDataService _receiptService;
    private readonly IDailyReportService _dailyReportService;

    public SalesController(ILogger<SalesController> logger, IReceiptDataService receiptService, IDailyReportService dailyReportService)
    {
        _logger = logger;
        _receiptService = receiptService;
        _dailyReportService = dailyReportService;
    }

    [HttpPost("Receipt")]
    [Consumes("application/xml")]
    public ActionResult ProcessReceipt([FromBody]XElement receipt)
    {
        var success = _receiptService.ProcessReceipt(receipt);
        return (success is true) ? Ok() : BadRequest();
    }

    [HttpGet("Receipt/{receiptId}")]
    public ActionResult<ReceiptData> GetReceiptData([FromRoute]int receiptId)
    {
        var result = _receiptService.GetReceiptData(receiptId);
        return (result is not null) ? Ok(result) : NotFound();
    }

    [HttpGet("Report")]
    public ActionResult<List<DateRangeSearchResult>> GetReportsFromPeriod(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
            return BadRequest(new {message = "Start date for the query cannot be sooner than the end date."});
        return Ok(_dailyReportService.GetReportsFromPeriod(startDate, endDate));
    }
}
