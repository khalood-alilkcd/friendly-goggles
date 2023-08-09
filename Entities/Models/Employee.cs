using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Employee
    {
        [Column("EmployeeId")]
        public Guid Id{ get; set; }
        [Required(ErrorMessage = "Employee Name is required field.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? Name{ get; set; }
        [Required(ErrorMessage = "Employee Age is required field.")]
        public int Age{ get; set; }
        [Required(ErrorMessage = "Employee Position is required field.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the Position is 30 characters.")]
        public string? Position{ get; set; }
        [ForeignKey(nameof(Company))]
        public Guid CompanyId{ get; set; }
        public Company? Company{ get; set; }
    }
}
