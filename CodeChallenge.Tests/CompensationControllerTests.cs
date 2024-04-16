
using System;
using System.Net;
using System.Net.Http;
using System.Text;

using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateCompensationNewEmployee_Returns_Created()
        {
            // Arrange
            var employee = new Employee()
            {
                EmployeeId = new Guid().ToString(),
                Department = "Complaints",
                FirstName = "Debbie",
                LastName = "Downer",
                Position = "Receiver",
            };

            var compensation = new Compensation()
            {
                Employee = employee,
                Salary = 85000.00,
                EffectiveDate = DateTime.Parse("2024-04-16")
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();

            Assert.IsNotNull(newCompensation.Employee);
            Assert.AreEqual(employee.EmployeeId, newCompensation.Employee.EmployeeId);
            Assert.AreEqual(employee.FirstName, newCompensation.Employee.FirstName);
            Assert.AreEqual(employee.LastName, newCompensation.Employee.LastName);
            Assert.AreEqual(employee.Department, newCompensation.Employee.Department);
            Assert.AreEqual(employee.Position, newCompensation.Employee.Position);
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
        }

        [TestMethod]
        public void CreateCompensationExistingEmployee_Returns_Created()
        {
            // Arrange
            var employee = new Employee()
            {
                EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f"
            };

            var compensation = new Compensation()
            {
                Employee = employee,
                Salary = 95000.00,
                EffectiveDate = DateTime.Parse("2024-04-17")
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();

            Assert.IsNotNull(newCompensation.Employee);
            Assert.AreEqual(employee.EmployeeId, newCompensation.Employee.EmployeeId);
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
        }

        [TestMethod]
        public void GetCompensationByEmployeeId_Returns_Ok()
        {
            // Arrange
            var expectedEmployee = new Employee()
            {
                EmployeeId = "4da0ce6b-2dd4-4b73-946f-8013b320b023",
                FirstName = "Neil",
                LastName = "Peart"
            };

            var expectedCompensation = new Compensation()
            {
                Employee = expectedEmployee,
                Salary = 65000.00,
                EffectiveDate = DateTime.Parse("2024-04-15")
            };

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{expectedEmployee.EmployeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var compensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(expectedEmployee.FirstName, compensation.Employee.FirstName);
            Assert.AreEqual(expectedEmployee.LastName, compensation.Employee.LastName);
            Assert.AreEqual(expectedCompensation.Salary, compensation.Salary);
            Assert.AreEqual(expectedCompensation.EffectiveDate, compensation.EffectiveDate);
        }
    }
}
