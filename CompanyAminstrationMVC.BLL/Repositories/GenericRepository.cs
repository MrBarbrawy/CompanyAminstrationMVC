using CompanyAdminstrationMVC.BLL.Interfaces;
using CompanyAdminstrationMVC.DAL.Models;
using CompanyAdminstrationMVC.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAdminstrationMVC.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }



        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await _context.Employees.Include(E => E.WorkFor).AsNoTracking().ToListAsync();
            }
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }
        public async Task<T?> GetAsync(int? id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task  AddAsync(T entity)
        {
           await _context.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }



        public void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}
