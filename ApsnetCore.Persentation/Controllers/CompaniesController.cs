using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using shared.DataTransferObject;
using ApsnetCore.Persentation.ModelBinders;

namespace ApsnetCore.Persentation.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public CompaniesController(IServiceManager service)=> _service = service;

        [HttpGet]
        public IActionResult GetCompanies()
        {
            var company = _service.CompanyService.GetCompanies(trackChanges: false);
            return Ok(company);
        }

        [HttpGet("{id:guid}", Name = "companyById")]
        public IActionResult GetCompany(Guid id)
        {
            var company = _service.CompanyService.GetCompany(id,trackChanges: false);
            return Ok(company);
        }

        [HttpGet("collection/({ids})" /*, Name = "test"*/)]
        public IActionResult GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder<Guid>))]IEnumerable<Guid> ids)
        {
            var companies = _service.CompanyService.GetByIds(ids, trackChanges: false);
            return Ok(companies);
        }

        [HttpPost]
        public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
        {
            if (company is null)
                return BadRequest("CompanyForCreationDto object is null");

            var createdCompany = _service.CompanyService.CreateCompany(company);

            return CreatedAtRoute("companyById", new { id = createdCompany.Id }, createdCompany);
        }

        [HttpPost("collection")]
        public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var result = _service.CompanyService.CreateCompanyCollection(companyCollection);

            return CreatedAtAction(nameof(GetCompanyCollection), new { result.ids } , result.companies);
        }
    }
}
