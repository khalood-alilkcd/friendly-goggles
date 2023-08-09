﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Company
    {
        [Column("CompanyId")]
        public Guid Id{ get; set; }
        [Required(ErrorMessage = "Conpany Name is required field.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? Name{ get; set; }    
        [Required(ErrorMessage = "Conpany Address is required field.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the Address is 30 characters.")]
        public string? Address {get; set; }
        public string? Country{ get; set; }
        public ICollection<Employee> Employees{ get; set; }
    }
}
