using Contract.Services;
using Microsoft.AspNetCore.Mvc;
using shared.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsnetCore.Persentation.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly IServiceManager _service;

        public EmployeeController(IServiceManager service)
            => _service = service;

        [HttpGet]
        public IActionResult GetEmployeesForCompany(Guid companyId)
        {
            var employee = _service.EmployeeService.GetEmployees(companyId, trackChanges : false);
            return Ok(employee);
        }

        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var emp = _service.EmployeeService.GetEmployee(companyId, id, trackChanges: false);
            return Ok(emp);
        }

        [HttpPost]
        public IActionResult CreateEmployeeByCompany(Guid companyId, [FromBody] EmployeeForCreationDto employeeForCreation)
        {
            if (employeeForCreation is null)
                return BadRequest("EmployeeForCreationDto object is null");

            var employeeForReturn = _service.EmployeeService.CreateEmployeeForCompany(companyId, employeeForCreation, trackChange: false);
            return CreatedAtRoute("GetEmployeeForCompany",new { companyId, id = employeeForReturn.Id }, employeeForReturn);
        }
    }
}
