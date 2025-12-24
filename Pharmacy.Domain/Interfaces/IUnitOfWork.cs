namespace Pharmacy.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMemberRepository Members { get; }
        IProviderRepository Providers { get; }
        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}