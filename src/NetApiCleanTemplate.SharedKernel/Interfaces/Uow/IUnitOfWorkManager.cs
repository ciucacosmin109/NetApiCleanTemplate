using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NetApiCleanTemplate.SharedKernel.Interfaces.Uow;
public interface IUnitOfWorkManager
{
    IUnitOfWork? Current { get; }

    IUnitOfWork Begin();
    Task<IUnitOfWork> BeginAsync();

    // TODO
    //IUnitOfWork Begin(UnitOfWorkOptions options);
}
