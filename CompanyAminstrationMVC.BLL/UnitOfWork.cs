using CompanyAdminstrationMVC.BLL.Interfaces;
using CompanyAdminstrationMVC.BLL.Repositories;
using CompanyAdminstrationMVC.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C42_G01_MVC04.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDepartmentRepository _departmentRepository;
        private IEmployeeRepository _employeeRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _departmentRepository = new DepartmentRepository(context);
            _employeeRepository = new EmployeeRepository(context);
        }

        public IEmployeeRepository EmployeeRepository => _employeeRepository;
        public IDepartmentRepository DepartmentRepository => _departmentRepository;

        public async Task<int> CompleteAsync() 
        { 
        
        return await _context.SaveChangesAsync();
        
        
        }



    }

}
