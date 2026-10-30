using NovaCart.Application.Interfaces;
using NovaCart.Infrastructure.Data;

namespace NovaCart.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGenericRepository<Domain.Entities.Category> Categories { get; private set; }
        public IGenericRepository<Domain.Entities.Product> Products { get; private set; }
        public IGenericRepository<Domain.Entities.Order> Orders { get; private set; }
        public IGenericRepository<Domain.Entities.OrderItem> OrderItems { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Categories = new GenericRepository<Domain.Entities.Category>(_context);
            Products = new GenericRepository<Domain.Entities.Product>(_context);
            Orders = new GenericRepository<Domain.Entities.Order>(_context);
            OrderItems = new GenericRepository<Domain.Entities.OrderItem>(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}