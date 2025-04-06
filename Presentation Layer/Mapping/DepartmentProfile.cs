using AutoMapper;
using Data_Access_Layer.Models;
using Presentation_Layer.Dtos;

namespace Presentation_Layer.Mapping
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<CreateDepartmentDto,Department>();
            CreateMap<Department, CreateDepartmentDto>();
        }
    }
}
