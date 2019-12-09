using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IBaseRepository <TEntity>
    {
        IEnumerable<TEntity> GetAll();
        Task<TEntity> GetById(int id);
        Task Create(TEntity model);
        Task Update(int id, TEntity model);
        Task Delete(int id);
    }
}
