using AutoMapper;
using CompanyAdminstrationMVC.DAL.Models;
using CompanyAdminstrationMVC.PL.ViewModels;

namespace CompanyAdminstrationMVC.PL.Mapping.User
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Map from AppUser to UserViewModel and reverse
            CreateMap<AppUser, UserViewModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))            // Maps Email
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))    // Maps FirstName
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))      // Maps LastName
                .ForMember(dest => dest.Roles, opt => opt.Ignore());                       // Ignore Roles here, you may populate them elsewhere
            // Reverse map for creating or updating AppUser from UserViewModel
            CreateMap<UserViewModel, AppUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))            // Maps Email
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))    // Maps FirstName
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))      // Maps LastName
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
             
        }
    }
}

