using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using NetApiCleanTemplate.SharedKernel.Interfaces.Uow;

namespace NetApiCleanTemplate.Infrastructure.Uow;

public class NullUnitOfWork : IUnitOfWork
{
    private bool isUnusable = false;

    public event EventHandler? Disposed;
    public event EventHandler? Completed;
    public event EventHandler<UnitOfWorkFailedEventArgs>? Failed;

    public NullUnitOfWork() { }

    public void Complete()
    {
        isUnusable = true;
        Completed?.Invoke(this, EventArgs.Empty);
    }
    public async Task CompleteAsync()
    {
        await Task.FromResult(0);
        Complete();
    }

    public void Dispose()
    {
        isUnusable = true;
        Disposed?.Invoke(this, EventArgs.Empty);
    }

    public bool IsUnusable()
    {
        return isUnusable;
    }

}
