﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared.DataTransferObject
{
    public record EmployeeForCreationDto(string Name, int Age, string Position);
}
