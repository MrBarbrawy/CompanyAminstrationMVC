﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAdminstrationMVC.DAL.Models
{
    public class BaseEntity
    {

        public int id { get; set; }

        [Required(ErrorMessage = "Name is Required :(")]
        public string Name { get; set; }






    }
}
