﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.Core.Interfaces.Entities;
public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
}

