using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.SharedKernel.Interfaces.Uow;
public interface IUnitOfWork : IDisposable
{
    event EventHandler Disposed;
    event EventHandler Completed; 
    event EventHandler<UnitOfWorkFailedEventArgs> Failed; 

    void Complete();

    bool IsUnusable();
}
