using AutoMapper;
using Contract.Services;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.Extensions.Logging;
using shared.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public CompanyDto CreateCompany(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);
            _repository.CompanyRepository.CreateCompany(companyEntity);
            _repository.Save();
            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
            return companyToReturn;
        }

        public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();

            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);

            foreach ( var companyEntity in companyEntities) {
                _repository.CompanyRepository.CreateCompany(companyEntity);
            }

            _repository.Save();

            var CompanyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            var ids = string.Join(",", CompanyCollectionToReturn.Select(c => c.Id));

            return (companies: CompanyCollectionToReturn, ids: ids);
        }

        public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();
            var companyEntities = _repository.CompanyRepository.GetByIds(ids, trackChanges);

            if(ids.Count() != companyEntities.Count())
                throw new CollectionByIdsBadRequestException();

            var CompaniesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            return  CompaniesToReturn;
        }

        public IEnumerable<CompanyDto> GetCompanies(bool trackChanges)
        {
                var companies = _repository.CompanyRepository.GetAllCompanies(trackChanges);
                var companyDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
                return companyDto;
        }

        public CompanyDto GetCompany(Guid id, bool trackChanges)
        {
            var company = _repository.CompanyRepository.GetCompany(id, trackChanges);
            if (company == null)
                throw new CompanyNotFoundException(id);
            var companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }
    }
}
