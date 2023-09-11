using ApsnetCore.Persentation.ActionFilters;
using Contract.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using shared.DataTransferObject;
using shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        [HttpHead]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParamters)
        {
            var pagedResult = await _service.EmployeeService.GetEmployeesAsync(companyId,employeeParamters, trackChanges : false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
            return Ok(pagedResult.employees);
        }

        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var emp = await _service.EmployeeService.GetEmployeeAsync(companyId, id, trackChanges: false);
            return Ok(emp);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployeeByCompany(Guid companyId, [FromBody] EmployeeForCreationDto employeeForCreation)
        {
            var employeeForReturn = await _service.EmployeeService.CreateEmployeeForCompanAsync(companyId, employeeForCreation, trackChange: false);
            return CreatedAtRoute("GetEmployeeForCompany",new { companyId, id = employeeForReturn.Id }, employeeForReturn);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployee(Guid companyId, Guid id)
        {
            await _service.EmployeeService.DeleteEmployeeAsync(companyId, id, trackChange: false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateEmployee(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employeeForUpdate)
        {
            await _service.EmployeeService.UpdateEmployeeAsync(companyId, id,employeeForUpdate, compTrackChanges: false, empTrackChanges: true);
            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, 
            [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null.");

            var result =  await _service.EmployeeService.GetEmployeeForPatch(companyId, id, compTrackChanges: false, empTrackChanges: true);
           
            patchDoc.ApplyTo(result.employeeToPatch,ModelState);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            _service.EmployeeService.SaveChangesForPatch(result.employeeToPatch, result.employeeEntity);

            return NoContent();
        }
    }
}
