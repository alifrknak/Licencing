using Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class UnitOfWork(DbContext dbContext) : IUnitOfWork
{
    public void BeginTransaction()
    {
        dbContext.Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        dbContext.Database.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        dbContext.Database.RollbackTransaction();
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}

