using AutoMapper;
using Data_Access_Layer.Models;
using Presentation_Layer.Dtos;

namespace Presentation_Layer.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<Employee, CreateEmployeeDto>();

        }
    }
}
