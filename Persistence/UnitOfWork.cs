using Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class UnitOfWork(DbContext dbContext) : IUnitOfWork
{
    public void BeginTransaction()
    {
        if (dbContext.Database.CurrentTransaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }
        dbContext.Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        if (dbContext.Database.CurrentTransaction == null)
        {
            throw new InvalidOperationException("No transaction is in progress.");
        }
        dbContext.Database.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        if (dbContext.Database.CurrentTransaction == null)
        {
            throw new InvalidOperationException("No transaction is in progress.");
        }
        dbContext.Database.RollbackTransaction();
    }

    public async Task SaveChangesAsync()
    {
        if (dbContext.ChangeTracker.HasChanges())
        {
            await dbContext.SaveChangesAsync();
        }
    }
}