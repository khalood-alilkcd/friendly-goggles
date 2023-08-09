using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    internal sealed class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext context): base(context) 
        {
            
        }

        public void CreateCompany(Company company) => Create(company);

        public IEnumerable<Company> GetAllCompanies(bool trackChanges)
        {
            var companies =  FindAll(trackChanges).OrderBy(c => c.Name).ToList();
            return companies;
        }

        public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        => FindByCondition(c => ids.Contains(c.Id), trackChanges).ToList();

        public Company GetCompany(Guid id, bool trackChanges)
        {
            var company = FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefault();
            return company;
        }
    }
}
