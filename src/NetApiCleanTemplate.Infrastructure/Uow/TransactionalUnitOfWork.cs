using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using NetApiCleanTemplate.SharedKernel.Interfaces.Uow;

namespace NetApiCleanTemplate.Infrastructure.Uow;
public class TransactionalUnitOfWork : IUnitOfWork
{
    private readonly IDbContextTransaction transaction;
    private bool isUnusable = false;

    public event EventHandler? Disposed;
    public event EventHandler? Completed;
    public event EventHandler<UnitOfWorkFailedEventArgs>? Failed;

    public IDbContextTransaction Transaction => transaction;

    public TransactionalUnitOfWork(IDbContextTransaction transaction) {
        this.transaction = transaction;
    }

    public void Complete()
    {
        CompleteAsync().GetAwaiter().GetResult();
    }
    public async Task CompleteAsync()
    {
        isUnusable = true;
        try
        {
            await transaction.CommitAsync();
            Completed?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            Failed?.Invoke(this, new UnitOfWorkFailedEventArgs(ex));
            throw;
        }
    }

    public void Dispose()
    {
        isUnusable = true;
        transaction.Dispose();
        Disposed?.Invoke(this, EventArgs.Empty);
    }

    public bool IsUnusable()
    {
        return isUnusable;
    }

}
