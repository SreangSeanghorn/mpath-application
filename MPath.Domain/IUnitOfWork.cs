namespace MPath.Domain
{
    public interface IUnitOfWork
    {
         Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}