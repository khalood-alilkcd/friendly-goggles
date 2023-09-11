using Contract.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsnetCore.Persentation.Controllers
{
    
    [Route("api/companies")]
    [ApiController]
    public class CompniesV2Controller : ControllerBase
    {
        private readonly IServiceManager _service;

        public CompniesV2Controller(IServiceManager service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _service.CompanyService.GetCompaniesAsync(trackChanges: false);
           
            return Ok(companies);
        }
    }
}
