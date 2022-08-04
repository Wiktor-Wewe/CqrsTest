using Microsoft.EntityFrameworkCore;

namespace CqrsTest.Data
{

    public class Repository<T> : IRepository where T : Model, new()
    {
        private readonly DataContext _db;
        private readonly DbSet<T> _dbSet;

        public Repository(DataContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        public async Task<T> Create(T model, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(model, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            return model;
        }

        public async Task<T> Update(T model, CancellationToken cancellationToken)
        {
            await _db.SaveChangesAsync(cancellationToken);

            return model;
        }

        public async Task<bool> Delete(Guid id, CancellationToken cancellationToken)
        {
            var model = new T() { Id = id };

            _db.Attach(model).State = EntityState.Deleted;

            var result = await _db.SaveChangesAsync(cancellationToken);

            return result > 0;
        }

        public async Task<T> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }
    }

    public interface IRepository
    {

    }

}
