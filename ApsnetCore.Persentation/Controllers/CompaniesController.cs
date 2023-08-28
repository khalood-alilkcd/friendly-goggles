using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract.Services;
using Microsoft.AspNetCore.Mvc;
using shared.DataTransferObject;
using ApsnetCore.Persentation.ModelBinders;
using Microsoft.AspNetCore.JsonPatch;
using ApsnetCore.Persentation.ActionFilters;

namespace ApsnetCore.Persentation.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public CompaniesController(IServiceManager service)=> _service = service;

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var company = await _service.CompanyService.GetCompaniesAsync(trackChanges: false);
            return Ok(company);
        }

        [HttpGet("{id:guid}", Name = "companyById")]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var company = await _service.CompanyService.GetCompanyAsync(id, trackChanges: false);
            return Ok(company);
        }

        [HttpGet("collection/({ids})" /*, Name = "test"*/)]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            var companies = await _service.CompanyService.GetByIdsAsync(ids, trackChanges: false);
            return Ok(companies);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            var createdCompany =await _service.CompanyService.CreateCompanyAsync(company);

            return CreatedAtRoute("companyById", new { id = createdCompany.Id }, createdCompany);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var result = await _service.CompanyService.CreateCompanyCollectionAsync(companyCollection);

            return CreatedAtAction(nameof(GetCompanyCollection), new { result.ids } , result.companies);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await _service.CompanyService.DeleteCompanyAsync(id, trackChages: false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            await _service.CompanyService.UpdateCompanyAsync(id, company, trackChanges: false);
            return NoContent();
        }
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartiallyUpdatCompany(Guid id ,[FromBody] JsonPatchDocument<CompanyForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null.");

            var result = await _service.CompanyService.GetCompanyForPatch(id, trackChanges: false);
            patchDoc.ApplyTo(result.companyForPatch);
            _service.CompanyService.SaveCompanyForPatch(result.companyForPatch, result.companyEntity);
            return NoContent();
        }
    }
}
