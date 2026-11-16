namespace NovaCart.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Domain.Entities.Category> Categories { get; }
        IGenericRepository<Domain.Entities.Product> Products { get; }
        IGenericRepository<Domain.Entities.Order> Orders { get; }
        IGenericRepository<Domain.Entities.OrderItem> OrderItems { get; }

        Task<int> CompleteAsync();
    }
}