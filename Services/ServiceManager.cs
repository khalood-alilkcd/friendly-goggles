using AutoMapper;
using Contract.Services;
using Contracts;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICompanyService> _company;
        private readonly Lazy<IEmployeeService> _employee;
        public ServiceManager(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _company = new Lazy<ICompanyService>(() => new CompanyService(repository, logger, mapper));
            _employee = new Lazy<IEmployeeService>(() => new EmployeeService(repository, logger, mapper));
        }
        public ICompanyService CompanyService => _company.Value;

        public IEmployeeService EmployeeService => _employee.Value;
    }
}
