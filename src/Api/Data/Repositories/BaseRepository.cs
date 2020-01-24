using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public abstract class BaseRepository<TEntity>: IBaseRepository<TEntity>
        where TEntity: class
    {
        protected readonly ApplicationDbContext DbContext;
        public BaseRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual async Task<TEntity> Create(TEntity model)
        {
            await DbContext.Set<TEntity>().AddAsync(model);
            await DbContext.SaveChangesAsync();

            return model;
        }

        public virtual async Task Delete(int id)
        {
            var entity = await DbContext.Set<TEntity>().FindAsync(id);
            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await DbContext.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> GetById(int id)
        {
            return await DbContext.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task Update(int id, TEntity entity)
        {
            DbContext.Set<TEntity>().Update(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> Paginate(int itemsCount, int offset)
        {
            return await DbContext.Set<TEntity>()
                .Skip(offset)
                .Take(itemsCount)
                .ToListAsync();
        }

        public virtual async Task<int> GetCountAsync()
        {
            return await DbContext.Set<TEntity>().CountAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> Create(IEnumerable<TEntity> entityList)
        {
            DbContext.Set<TEntity>().AddRange(entityList);
            await DbContext.SaveChangesAsync();

            return entityList;
        }
    }
}
