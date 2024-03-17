using AutoMapper;
using PermissionManagement.Model;
using PermissionManagement.ViewModels;

namespace PermissionManagement.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PermissionViewModel, permissionModel>().
                ForMember(dest=>dest.updatedDate, opt => opt.MapFrom(src=>DateTime.Now)).
                ReverseMap();

            CreateMap<EmployeeViewModel, EmployeeModel>().
                ForMember(dest => dest.updatedDate, opt => opt.MapFrom(src => DateTime.Now)).
                ReverseMap();

            CreateMap<PermissionTypeViewModel, PermissionTypeModel>().
                ForMember(dest => dest.updatedDate, opt => opt.MapFrom(src => DateTime.Now)).
                ReverseMap();
        }
    }
}
