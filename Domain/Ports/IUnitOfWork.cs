namespace Domain.Ports;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
    void BeginTransaction();
    void CommitTransaction();
    void RollbackTransaction();
}
