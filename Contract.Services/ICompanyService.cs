using shared.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Services
{
    public interface ICompanyService
    {
        IEnumerable<CompanyDto> GetCompanies(bool trackChanges);
        CompanyDto GetCompany(Guid id, bool trackChanges);
        CompanyDto CreateCompany(CompanyForCreationDto company);
        IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges);

        (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection);
    }
}
