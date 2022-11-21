using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NetApiCleanTemplate.Infrastructure.Data;
using NetApiCleanTemplate.SharedKernel.Interfaces.Uow;
using NetCore.AutoRegisterDi;

namespace NetApiCleanTemplate.Infrastructure.Uow;

[RegisterAsScoped]
public class TransactionalUnitOfWorkManager : IUnitOfWorkManager // Scoped dependency !!
{
    private readonly AppDbContext context;

    public TransactionalUnitOfWorkManager(
        AppDbContext context
    ) {
        this.context = context;
    }

    // The current unit of work
    private TransactionalUnitOfWork? current;
    public IUnitOfWork? Current {
        get {
            if (current == null || current.IsUnusable())
            {
                current = null;
            }

            return current;
        }
    }

    // Creates a new unit of work if there is none already created 
    public IUnitOfWork Begin()
    {
        return BeginAsync().GetAwaiter().GetResult();
    }
    public async Task<IUnitOfWork> BeginAsync()
    {
        // There is already an outter unit of work
        if (current != null && !current.IsUnusable())
        {
            await context.Database.UseTransactionAsync(current.Transaction.GetDbTransaction());
            return new NullUnitOfWork(); // When we call .Complete() on an inner UOW, the transaction should not be commited
        }

        // Create a new unit of work
        var transaction = context.Database.CurrentTransaction ?? await context.Database.BeginTransactionAsync();
        current = new TransactionalUnitOfWork(transaction);

        current.Disposed += (s, e) => current = null;
        current.Completed += (s, e) => current = null;
        current.Failed += (s, e) => current = null;

        return current;
    }
}
