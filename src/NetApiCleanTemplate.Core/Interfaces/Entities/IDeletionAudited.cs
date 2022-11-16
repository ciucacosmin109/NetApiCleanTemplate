﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.Core.Interfaces.Entities;
public interface IHasDeletionTime : ISoftDeletable
{
    DateTime? DeletionTime { get; set; }
}
public interface IDeletionAudited<T> : IHasDeletionTime
{
    T? DeleterUserId { get; set; }
}
public interface IDeletionAudited : IDeletionAudited<string> { }

