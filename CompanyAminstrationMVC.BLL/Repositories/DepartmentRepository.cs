using CompanyAdminstrationMVC.BLL.Interfaces;
using CompanyAdminstrationMVC.DAL.Models;
using CompanyAdminstrationMVC.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAdminstrationMVC.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {

        public DepartmentRepository(AppDbContext context) : base(context) 
        {
        }

     

        

   

    }
}
