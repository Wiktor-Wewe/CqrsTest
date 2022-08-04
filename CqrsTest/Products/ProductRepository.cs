using CqrsTest.Data;

namespace CqrsTest.Products
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DataContext db) : base(db)
        {

        }
    }

    public interface IProductRepository
    {
        Task<Product> Create(Product model, CancellationToken cancellationToken);
        Task<Product> Update(Product model, CancellationToken cancellationToken);
        Task<bool> Delete(Guid id, CancellationToken cancellationToken);
        Task<Product> GetById(Guid id, CancellationToken cancellationToken);
    }
}
