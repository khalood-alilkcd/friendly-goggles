using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extentions;
using shared.DataTransferObject;
using shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext context): base(context) { }


        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges) 
            => await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        {
            var employees = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
                    .OrderBy(e => e.Name)
                    .FilterEmployee(employeeParameters.MinAge, employeeParameters.MaxAge)
                    .Search(employeeParameters.SearchTerm)
                    .Sort(employeeParameters.OrderBy)
                    .Skip((employeeParameters.PageNumber -1 ) * employeeParameters.PageSize)
                    .Take(employeeParameters.PageSize)
                    .ToListAsync();

            var count = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges).CountAsync();

            return new PagedList<Employee> (employees, count, employeeParameters.PageNumber, employeeParameters.PageSize);
        }
        public void DeleteEmployee(Employee employee)
            => Delete(employee);

        public void CreateEmployeeforCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee) ;
        }
    }
}
