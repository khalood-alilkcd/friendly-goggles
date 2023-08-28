using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared.DataTransferObject
{
    public abstract record EmployeeForManipulationDto
    {
        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? Name { get; init; }
        [Range(18, int.MaxValue, ErrorMessage = "age is a required and it can't be Lower than 18.")]
        public int Age { get; init; }
        [Required(ErrorMessage = "Employee position is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Position is 30 characters.")]
        public string? Position { get; init; }
    }
}
