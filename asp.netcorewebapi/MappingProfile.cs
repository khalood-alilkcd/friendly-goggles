﻿using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using shared.DataTransferObject;
using System.Runtime.CompilerServices;

namespace asp.netcorewebapi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Company, CompanyDto>().ForCtorParam("FullAddress", opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
            CreateMap<Company, CompanyDto>().ForMember(c => c.FullAddress, opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
            CreateMap<Employee, EmployeeDto>();
            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<EmployeeForCreationDto, Employee>();
            ///The ReverseMap method is also going to configure this rule to execute reverse mapping if we ask for it.
            CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();
            CreateMap<CompanyForUpdateDto, Company>();
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}
