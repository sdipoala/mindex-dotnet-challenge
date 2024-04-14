using CodeChallenge.Models;
using CodeChallenge.Repositories;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public ReportingStructureService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository) 
        { 
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public ReportingStructure GetReportingStructureByEmployeeId(string employeeId)
        {
            var employee = _employeeRepository.GetById(employeeId);
            var numberOfReports = GetNumberOfReports(employee);

            var reportingStructure = new ReportingStructure()
            {
                Employee = employee,
                NumberOfReports = numberOfReports
            };

            return reportingStructure;
        }

        private int GetNumberOfReports(Employee employee)
        {
            var numberOfReports = 0;

            if (employee.DirectReports != null && employee.DirectReports.Count > 0)
            {
                numberOfReports = employee.DirectReports.Count;

                foreach (var report in employee.DirectReports)
                {
                    numberOfReports += GetNumberOfReports(report);
                }
            }
            else
            {
                numberOfReports = 0;
            }

            return numberOfReports;
        }
    }
}
