using AutoMapper;
using Contract.Services;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Repository;
using shared.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChange)
        {
            var company = _repository.CompanyRepository.GetCompany(companyId, trackChange);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var EmployeeEntity = _mapper.Map<Employee>(employeeForCreation);
            _repository.EmployeeRepository.CreateEmployeeforCompany(companyId, EmployeeEntity);
            _repository.Save();

            var EmployeeForReturn = _mapper.Map<EmployeeDto>(EmployeeEntity);
            return EmployeeForReturn;
        }

        public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges) 
        {
            var company = _repository.CompanyRepository.GetCompany(companyId, trackChanges); 
            if (company is null) 
                throw new CompanyNotFoundException(companyId); 
            var employeeDb = _repository.EmployeeRepository.GetEmployee(companyId, id, trackChanges); 
            if (employeeDb is null) 
                throw new EmployeeNotFoundException(id);
            var employee = _mapper.Map<EmployeeDto>(employeeDb);
            return employee; 
        }

        public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
        {
            var company = _repository.CompanyRepository.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeesFromDb = _repository.EmployeeRepository.GetEmployees(companyId, trackChanges);
            var employeeDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
            return employeeDto; 
        }
    }
}
