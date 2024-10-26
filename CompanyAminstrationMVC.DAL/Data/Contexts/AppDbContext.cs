using CompanyAdminstrationMVC.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CompanyAdminstrationMVC.Data.Contexts
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.ApplyConfiguration(new DepConfig());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            base.OnModelCreating(modelBuilder);
        }







        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    optionsBuilder.UseSqlServer("Server = .; Database = CompanyMVCG02; Trusted_Connection = True; TrustServerCertificate = True");


            
        //}

        public DbSet<Department> Departments { get; set; }

        public DbSet<Employee> Employees { get; set; }

     



    }

    


}
