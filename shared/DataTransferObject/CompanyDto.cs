using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared.DataTransferObject
{
    /// <summary>
    /// we make that attrebute We get an error because XmlSerializer cannot easily serialize our positional record type.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="FullAddress"></param>
    //[Serializable]
    //public record CompanyDto(Guid Id, string Name, string FullAddress);
    public record CompanyDto
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? FullAddress { get; init; }
    }
    
}
