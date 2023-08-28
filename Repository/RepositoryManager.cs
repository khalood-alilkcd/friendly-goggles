using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private readonly Lazy<ICompanyRepository> _company;
        private readonly Lazy<IEmployeeRepository> _employee;
        public RepositoryManager(RepositoryContext context) 
        {
            _context = context;
            _company = new Lazy<ICompanyRepository>(() => new CompanyRepository(_context));
            _employee = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(_context));
        }
        public ICompanyRepository CompanyRepository => _company.Value;

        public IEmployeeRepository EmployeeRepository => _employee.Value;
        public async Task SaveAsync() => await _context.SaveChangesAsync();
        
    }
}
