using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/reportingstructure")]
    public class ReportingStructureController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportingStructureService;

        public ReportingStructureController(ILogger<EmployeeController> logger, IReportingStructureService reportingStructureService)
        {
            _logger = logger;
            _reportingStructureService = reportingStructureService;
        }

        [HttpGet("{id}", Name = "getReportingStructureByEmployeeId")]
        public IActionResult GetReportingStructureByEmployeeId(String id)
        {
            _logger.LogDebug($"Received reporting structure get request for employee '{id}'");

            var reportingStructure = _reportingStructureService.GetReportingStructureByEmployeeId(id);

            if (reportingStructure == null)
                return NotFound();

            return Ok(reportingStructure);
        }
    }
}
