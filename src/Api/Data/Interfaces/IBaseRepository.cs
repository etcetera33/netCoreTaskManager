using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IBaseRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetById(int id);
        Task<TEntity> Create(TEntity model);
        Task<IEnumerable<TEntity>> Create(IEnumerable<TEntity> entityList);
        Task Update(int id, TEntity model);
        Task Delete(int id);
    }
}
