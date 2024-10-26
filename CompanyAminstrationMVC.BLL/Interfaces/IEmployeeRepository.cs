using CompanyAdminstrationMVC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAdminstrationMVC.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {


        Task<IEnumerable<Employee>> GetByNameAsync(string name);






    }
}
