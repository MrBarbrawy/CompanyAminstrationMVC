using CompanyAdminstrationMVC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAdminstrationMVC.BLL.Interfaces
{
    public interface IGenericRepository<T>
    {


        Task<IEnumerable<T>> GetAllAsync();


        Task<T> GetAsync(int? id);


        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);




    }
}
