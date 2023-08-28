using AutoMapper;
using Contract.Services;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.Extensions.Logging;
using shared.DataTransferObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
        public async Task<IEnumerable<CompanyDto>> GetCompaniesAsync(bool trackChanges)
        {
                var companies = await _repository.CompanyRepository.GetAllCompaniesAsync(trackChanges);
                var companyDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
                return companyDto;
        }
        public async Task<CompanyDto> GetCompanyAsync(Guid id, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(id, trackChanges);
            var companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }
        public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();
            var companyEntities = await _repository.CompanyRepository.GetByIdsAsync(ids, trackChanges);

            if(ids.Count() != companyEntities.Count())
                throw new CollectionByIdsBadRequestException();

            var CompaniesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            return  CompaniesToReturn;
        }

        public async Task<(IEnumerable<CompanyDto> companies, string ids)> 
            CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();

            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);

            foreach (var companyEntity in companyEntities)
            {
                _repository.CompanyRepository.CreateCompany(companyEntity);
            }

            await _repository.SaveAsync();

            var CompanyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            var ids = string.Join(",", CompanyCollectionToReturn.Select(c => c.Id));

            return (companies: CompanyCollectionToReturn, ids: ids);
        }

        public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);
            _repository.CompanyRepository.CreateCompany(companyEntity);
            await _repository.SaveAsync();
            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
            return companyToReturn;
        }

        public async Task DeleteCompanyAsync(Guid companyId, bool trackChages)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChages);
            _repository.CompanyRepository.DeleteCompany(company);
            await _repository.SaveAsync();
        }

        public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            _mapper.Map(companyForUpdate, company);
            await _repository.SaveAsync(); 
        }

        public async Task<(CompanyForUpdateDto companyForPatch, Company companyEntity)> GetCompanyForPatch(Guid companyId, bool trackChanges)
        {
            var companyEntity = await _repository.CompanyRepository.GetCompanyAsync(companyId, trackChanges);
            if (companyEntity is null)
                throw new CompanyNotFoundException(companyId);

            var companyForPatch = _mapper.Map<CompanyForUpdateDto>(companyEntity);
            return (companyForPatch, companyEntity);
        }

        public void SaveCompanyForPatch(CompanyForUpdateDto companyForPatch, Company companyEntity)
        {
            _mapper.Map(companyForPatch, companyEntity);
            _repository.SaveAsync();
        }

        private async Task<Company> GetCompanyAndCheckIfItExists(Guid id , bool trackChanges)
        {
            var company = await _repository.CompanyRepository.GetCompanyAsync(id, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(id);
            return company;
        }
       
    }
}
