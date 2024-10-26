using AutoMapper;
using CompanyAdminstrationMVC.DAL.Models;
using CompanyAdminstrationMVC.PL.ViewModels;

namespace CompanyAdminstrationMVC.PL.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }
    }
}
