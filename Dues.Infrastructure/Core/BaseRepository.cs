using Microsoft.EntityFrameworkCore;
using Dues.Infrastructure.Context;

namespace Dues.Infrastructure.Core
{
    public abstract class BaseRepository
    {
        protected readonly DuesContext Db;

        protected BaseRepository(DuesContext db)
        {
            Db = db;
        }
    }

    public abstract class BaseRepository<TEntity> : BaseRepository where TEntity : class
    {
        protected BaseRepository(DuesContext db) : base(db)
        {
        }

        public virtual async Task<List<TEntity>> GetAllAsync() =>
            await Db.Set<TEntity>().ToListAsync();

        public virtual async Task<TEntity?> GetByIdAsync(int id) =>
            await Db.Set<TEntity>().FindAsync(id);

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await Db.Set<TEntity>().AddAsync(entity);
            await Db.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            Db.Set<TEntity>().Update(entity);
            await Db.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            Db.Set<TEntity>().Remove(entity);
            await Db.SaveChangesAsync();
        }
    }
}
