using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAdminstrationMVC.DAL.Models
{
    public class Employee : BaseEntity
    {



        public int? Age { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public double Salary { get; set; }


        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public DateTime HiringDate { get; set; }

        public bool IsDeleted { get; set; }

        public int? WorkForId { get; set; } // Fk
        public Department? WorkFor { get; set; } // Navigational Property

        public string? ImageName { get; set; }

    }
}
