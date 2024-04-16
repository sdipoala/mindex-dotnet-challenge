using CodeChallenge.Data;
using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public CompensationRepository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            if (_employeeContext.Employees.Any(e => e.EmployeeId == compensation.Employee.EmployeeId))
            {
                _employeeContext.Entry(compensation.Employee).State = EntityState.Modified;
            }

            _employeeContext.Compensations.Add(compensation);
            return compensation;
        }

        public Compensation GetByEmployeeId(string id)
        {
            var compensation = _employeeContext.Compensations.SingleOrDefault(c => c.Employee.EmployeeId == id);
            var employee = _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);

            if (compensation != null)
            {
                compensation.Employee = employee;
            }

            return compensation;
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }
    }
}
