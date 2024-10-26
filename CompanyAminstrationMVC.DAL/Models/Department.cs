using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAdminstrationMVC.DAL.Models
{
    public class Department : BaseEntity
    {

        public int id { get; set; }

        [Required(ErrorMessage = "Code is Required :(")]
        public string Code { get; set; }



        [DisplayName("Date of Creation")]
        public DateTime DateOfCreation { get; set; }

        public ICollection<Employee>? Employees { get; set; }



    }
}
